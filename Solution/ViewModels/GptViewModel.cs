using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using Solution.Services;

namespace Solution.ViewModels
{
  public class GptViewModel : INotifyPropertyChanged
  {
    private string _responseText;

    public string ResponseText
    {
      get { return _responseText; }
      set
      {
        if (_responseText != value)
        {
          _responseText = value;
          OnPropertyChanged();
        }
      }
    }

    private string _subject;

    public string Subject
    {
      get { return _subject; }
      set
      {
        if (_subject != value)
        {
          _subject = value;
          OnPropertyChanged();
        }
      }
    }

    private int _length;

    public int Length
    {
      get { return _length; }
      set
      {
        if (_length != value)
        {
          _length = value;
          OnPropertyChanged();
        }
      }
    }

    private string _complexity;

    public string Complexity
    {
      get { return _complexity; }
      set
      {
        if (_complexity != value)
        {
          _complexity = value;
          OnPropertyChanged();
        }
      }
    }

    private string _language;

    public string Language
    {
      get { return _language; }
      set
      {
        if (_language != value)
        {
          _language = value;
          OnPropertyChanged();
        }
      }
    }
    string _type;
    private string _texttype;
    public string TextType
    {
      get { return _texttype; }
      set
      {
        if (_texttype != value)
        {
          _texttype = value;
          OnPropertyChanged();
        }
      }
    }

    public RelayCommand SendPromptCommand { get; }

    public GptViewModel()
    {
      SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);
    }

    public async Task SendPrompt()
    {
      Dictionary<string, string> complexities = new Dictionary<string, string>
      {
        ["basic"] = "basic (no capital letters or punctuation, simple words)",
        ["average"] = "average (include capital letters and punctuation)",
        ["advanced"] = "advanced (include capital letters and punctuation, use difficult words)",
      };

      if (Subject == "")
      {
        Subject = "random";
      }

      if (TextType.ToUpper() == "WORDS")
      {
        _type = "random words (don't use any capital letters or punctuation)";
      }else if (TextType.ToUpper() == "SENTENCES")
      {
        _type = "Completely random sentences that don't have any connection, separated by dots. Use capital letters.";
      }else if (TextType.ToUpper() == "STORY")
      {
        foreach (var variable in complexities)
        {
          if (variable.Key == Complexity.ToLower())
          {
            Complexity = variable.Value;
          }
        }
      }
      string prompt = $"Generate a text for typing practice using the following specifications. DON'T use newlines, ever!\n" +
                      $"Generate completely random {_type} \n" +
                      $"Language: {Language} [EXTREMELY IMPORTANT]\n" +
                      $"Length: EXACTLY {Length} words long.\n" +
                      $"Subject: {Subject}\n" +
                      $"Text/word complexity: {Complexity}\n\n" +
                      "PROVIDE THE TEXT IN A SINGLE STRING WITHOUT QUATATION MARKS OR ANY OTHER TEXT (DON'T generate code, use plaintext!)\n" +
                      "Do NOT go over the specified wordcount!!!\n" +
                      "DO NOT USE ANY CHARACTERS OUTSIDE THE LATIN ALPHABET, REPLACE FOREIGN LETTERS WITH ALPHANUMERIC CHARACTERS!!!";




      // Declare the API key
      var apiKey = "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";

// Create an instance of the OpenAIService class
      var gpt3 = new OpenAIService(new OpenAiOptions()
        { ApiKey = apiKey });

// Create a chat completion request
      var completionResult = await gpt3.ChatCompletion.CreateCompletion
      (new ChatCompletionCreateRequest()
      {
        Messages = new List<ChatMessage>(new ChatMessage[]
          { new ChatMessage("system", prompt) }),
        Model = "gpt-4-1106-preview",
        MaxTokens = 100,
        N = 1
      });

// Check if the completion result was successful and handle the response
      StringBuilder resultBuilder = new StringBuilder();

      if (completionResult.Successful)
      {
        foreach (var choice in completionResult.Choices)
        {
          resultBuilder.AppendLine(choice.Message.Content);
        }
      }
      else
      {
        resultBuilder.AppendLine("Error occurred");
      }

      ResponseText = resultBuilder.ToString();

      // Return the concatenated results as a string
      Console.WriteLine(resultBuilder.ToString());
    }

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
