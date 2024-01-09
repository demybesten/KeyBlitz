using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using Solution.Helpers;
using static Solution.Helpers.Helpers;
using Solution.ViewModels;

namespace Solution.Services;

public class SendPrompt: ObservableObject
{
  private string _apiKey;

  string closestString = null;

  private string _responseText;
  private string[] ResponseTextArray;

  public string ResponseText
  {
    get { return _responseText; }
    set
    {
      if (_responseText != value)
      {
        _responseText = value;
        OnPropertyChanged(nameof(ResponseText));
      }
    }
  }

  public SendPrompt()
  {
    var config = new ConfigurationBuilder().AddUserSecrets<NewTestViewModel>().Build();
    _apiKey = config["ApiKey"];
  }

  public async Task<string> GeneratePrompt(string _textSubject, string _textType, int _textLength, string _complexityLevel,string _language)
    {
      // ShowLoading = true;
      // Console.WriteLine(_showLoading);
      // Console.WriteLine("Generating....");
      Dictionary<string, string> complexities = new Dictionary<string, string>
      {
        ["basic"] = "basic (no capital letters or punctuation, simple words)",
        ["average"] = "average (include capital letters and punctuation)",
        ["advanced"] = "advanced (include capital letters and punctuation, use difficult words)",
      };

      if (_textSubject == "")
      {
        _textSubject = "random";
      }

      if (_textType.ToUpper() == "WORDS")
      {
        _textType = "random words (don't use any capital letters or punctuation)";
      }else if (_textType.ToUpper() == "SENTENCES")
      {
        _textType = "Completely random sentences that don't have any connection, separated by dots. Use capital letters.";
      }else if (_textType.ToUpper() == "STORY")
      {
        foreach (var variable in complexities)
        {
          if (variable.Key == _complexityLevel)
          {
            _complexityLevel = variable.Value;
          }
        }
      }
      string prompt = $"Generate a text for typing practice using the following specifications. DON'T use newlines, ever!\n" +
                      $"Generate completely random {_textType} \n" +
                      $"Language: {_language} [EXTREMELY IMPORTANT]\n" +
                      $"Length: EXACTLY {_textLength} words long.\n" +
                      $"Subject: {_textSubject}\n" +
                      $"Text/word complexity: {_complexityLevel}\n\n" +
                      "PROVIDE THE TEXT IN A SINGLE STRING WITHOUT QUATATION MARKS OR ANY OTHER TEXT (DON'T generate code, use plaintext!)\n" +
                      "Do NOT go over the specified wordcount!!!\n" +
                      "DO NOT USE ANY CHARACTERS OUTSIDE THE LATIN ALPHABET, REPLACE FOREIGN LETTERS WITH ALPHANUMERIC CHARACTERS!!!";

// Create an instance of the OpenAIService class
      var gpt3 = new OpenAIService(new OpenAiOptions()
        { ApiKey = _apiKey });
// Create a chat completion request
      var completionResult = await gpt3.ChatCompletion.CreateCompletion
      (new ChatCompletionCreateRequest()
      {
        Messages = new List<ChatMessage>(new ChatMessage[]
          { new ChatMessage("system", prompt) }),
        Model = "gpt-4-1106-preview",
        // MaxTokens = 4096,
        N = 1
      });

// Check if the completion result was successful and handle the response
      StringBuilder resultBuilder = new StringBuilder();
      List<string> resultArray = new List<string>();
      if (completionResult.Successful)
      {
        foreach (var choice in completionResult.Choices)
        {
          resultBuilder.AppendLine(choice.Message.Content);
          resultArray.Add(resultBuilder.ToString());
          resultBuilder = new StringBuilder();
        }
      }
      else
      {
        resultBuilder.AppendLine("Error occurred");
      }
      int closestDifference = int.MaxValue;

      foreach (string currentString in resultArray)
      {
        ResponseTextArray = currentString.Split(' ');
        // Console.WriteLine($"Length({_textLength}/{ResponseTextArray.Length}):  {currentString}|||\n");
        // Calculate the absolute difference between the target word count and the current string's word count
        int currentWordCount = CountWords(currentString);
        int difference = Math.Abs(_textLength - currentWordCount);

        // Check if the current string is closer to the target word count
        if (difference < closestDifference)
        {
          closestString = currentString;
          closestDifference = difference;
        }
      }
      return closestString;
    }

}
