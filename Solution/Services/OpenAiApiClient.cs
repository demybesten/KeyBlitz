using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Solution.Services;

public class OpenAiApiClient
{
  private readonly HttpClient _httpClient;
  private readonly string _apiKey;

  public OpenAiApiClient(string apiKey)
  {
    _apiKey = apiKey;
    _httpClient = new HttpClient();
    _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
  }
}