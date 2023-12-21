using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.IO.Pipes;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Solution.Services
{
    public enum LeaderboardTimeperiod
    {
        AllTime = 0,
        Year = 1,
        Month = 2,
        Week = 3
    }

    public enum ApiContentType
    {
        GeneratedText,
        User,
        Score,
        UserList,
        ScoreList
    }

    public class ApiClient
    {
        private HttpClient client;
        private string ApiPrefix = "/api";
        private static readonly string TokenFile = "auth.token";

        public ApiClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://keyblitz.gewoonyorick.nl");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        private async Task<ApiResponse> GetResult(string url, HttpMethod method, ApiContentType contentType, StringContent? content = null, bool auth = true)
        {
            HttpRequestMessage request = new HttpRequestMessage(method, GetEndpoint(url));

            if (auth)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
                Console.WriteLine("Added token...");
            }

            if (content != null)
            {
                request.Content = content;
                Console.WriteLine("Added content...");
            }

            HttpResponseMessage response = await client.SendAsync(request);
            string result = await response.Content.ReadAsStringAsync();
            return HandleResult(result, contentType);
        }


        private static StringContent GetParameters(JsonObject obj)
        {
            return new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
        }

        private string GetEndpoint(string endpoint)
        {
            return ApiPrefix + endpoint;
        }

        public static void SetToken(string token)
        {
            try
            {
                File.WriteAllText(TokenFile, token);
                Console.WriteLine("Token saved successfully. " + Directory.GetCurrentDirectory() + "\\" + TokenFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving token: " + ex.Message);
            }
        }

        private static string GetToken()
        {
            try
            {
                string token = File.ReadAllText(TokenFile);
                Console.WriteLine("Token read successfully. " + Directory.GetCurrentDirectory() + "\\" + TokenFile);
                return token;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Token file not found. Returning empty string.");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading token: " + ex.Message);
                return string.Empty;
            }
        }

        private ApiResponse HandleResult(string response, ApiContentType contentType)
        {
            File.WriteAllText("apiclient.debug", response);
            ApiResponse result = new ApiResponse(contentType);

            // Read the response content as a JsonDocument
            using (JsonDocument document = JsonDocument.Parse(response))
            {
                // Token
                try
                {
                    string? token = document.RootElement.GetProperty("token").GetString();
                    if (token != null)
                    {
                        SetToken(token);
                        result.TokenRegistered = true;
                        Console.WriteLine("A new token was found, registering token!");
                    }
                }
                catch (KeyNotFoundException) { }

                // Data
                try
                {
                    string? data = document.RootElement.GetProperty("data").GetRawText();
                    if (data == null)
                    {
                        Console.WriteLine("No data was found in the request.");
                        return result;
                    }

                    switch (contentType)
                    {
                        case ApiContentType.GeneratedText:
                            result.GeneratedText = document.RootElement.GetProperty("data").GetString();
                            break;
                        case ApiContentType.User:
                            result.User = JsonSerializer.Deserialize<User>(data);
                            break;
                        case ApiContentType.Score:
                            result.Score = JsonSerializer.Deserialize<Score>(data);
                            break;
                        case ApiContentType.ScoreList:
                            result.ScoreList = JsonSerializer.Deserialize<List<Score>>(data);
                            break;
                    }

                    return result;
                }
                catch (KeyNotFoundException) { }
            }

            Console.WriteLine("Something went wrong...");
            return result;
        }

        //
        // Routes
        //

        // Register user
        public async Task<ApiResponse> Register(string username, string password)
        {
            JsonObject content = new JsonObject();
            content.Add("username", username);
            content.Add("password", password);

            return await GetResult("/register", HttpMethod.Post, ApiContentType.User, GetParameters(content), false);
        }

        // Login User
        public async Task<ApiResponse> Login(string username, string password)
        {
            JsonObject content = new JsonObject();
            content.Add("username", username);
            content.Add("password", password);

            return await GetResult("/login", HttpMethod.Post, ApiContentType.User, GetParameters(content), false);
        }

        // Userinfo - Get user info
        public async Task<ApiResponse> GetUserInfo()
        {
            return await GetResult("/user", HttpMethod.Get, ApiContentType.User);
        }

        // Generate textscore
        public async Task<ApiResponse> GetText(string TextType, string Language, string Length, string Subject, string Complexity)
        {
            JsonObject content = new JsonObject();
            content.Add("texttype", TextType);
            content.Add("language", Language);
            content.Add("textlength", Length);
            content.Add("textsubject", Subject);
            content.Add("complexity", Complexity);

            return await GetResult("/generate/", HttpMethod.Post, ApiContentType.GeneratedText, GetParameters(content));
        }

        // Scores - save score
        public async Task<ApiResponse> SaveScore(int accuracy, int cpm, int wpm, int? multiplayerSessionId = null)
        {
            JsonObject content = new JsonObject();
            content.Add("accuracy", accuracy);
            content.Add("cpm", cpm);
            content.Add("wpm", wpm);
            content.Add("multiplayer_session_id", multiplayerSessionId);

            return await GetResult("/scores", HttpMethod.Post, ApiContentType.Score, GetParameters(content));
        }

        // Scores - Get own scores
        public async Task<ApiResponse> GetPlayerScores()
        {
            return await GetResult("/scores/player/", HttpMethod.Get, ApiContentType.ScoreList);
        }

        // Scores - leaderboard
        public async Task<ApiResponse> GetLeaderboard(LeaderboardTimeperiod period)
        {
            return await GetResult("/scores/leaderboard/" + period, HttpMethod.Get, ApiContentType.ScoreList);
        }
    }

    public class User
    {
        public int Id { get; set; }
        public int Username { get; set; }
    }

    public class Score
    {
        public int Id { get; }
        public int UserId { get; }
        public User? User { get; }
        public int MultiplayerId { get; }
        public int Accuracy { get; }
        public int Cpm { get; }
        public int Wpm { get; }
        public int OriginalScore { get; }
        public DateTime Date { get; }
    }

    public class ApiResponse
    {
        public bool Success { get; set; } = false;
        public bool TokenRegistered { get; set; } = false;
        public User? User { get; set; }
        public Score? Score { get; set; }
        public List<Score>? ScoreList { get; set; }
        public string? GeneratedText { get; set; }
        private ApiContentType type;

        public ApiResponse(ApiContentType type)
        {
            this.type = type;
        }

        public object? GetData()
        {
            return type switch
            {
                ApiContentType.GeneratedText => GeneratedText,
                ApiContentType.Score => Score,
                ApiContentType.ScoreList => ScoreList,
                ApiContentType.User => User,
                _ => null,
            };
        }
    }
}
