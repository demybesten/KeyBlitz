using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using Solution.Services;
using static Solution.Helpers.Helpers;

namespace Solution.ViewModels
{
    public class NewTestViewModel : BaseViewModel{

        public NewTestViewModel() {
            _textLength = 20;
            ComplexityLevels.Add("basic");
            ComplexityLevels.Add("average");
            ComplexityLevels.Add("advanced");
            TextTypes.Add("story");
            TextTypes.Add("sentences");
            TextTypes.Add("words");
            Languages.Add("english");
            Languages.Add("dutch");
            Languages.Add("german");
            Languages.Add("french");


            SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);


        }


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

        private int _textLength;
        public int TextLength {
            get {
                return _textLength;
            }
            set {
                _textLength = value;
                OnPropertyChanged(nameof(TextLength));
            }
        }

        private ObservableCollection<string> _complexityLevels = new ObservableCollection<string>();
        public ObservableCollection<string> ComplexityLevels {
            get {
                return _complexityLevels;
            }
            set {
                _complexityLevels = value;
                OnPropertyChanged(nameof(ComplexityLevels));
            }
        }

        private ObservableCollection<string> _textTypes = new ObservableCollection<string>();
        public ObservableCollection<string> TextTypes {
            get {
                return _textTypes;
            }
            set {
                _textTypes = value;
                OnPropertyChanged(nameof(TextTypes));
            }
        }

        private string _textType = "story";
        public string TextType {
            get {
                return _textType;
            }
            set {
                _textType = value;
                OnPropertyChanged(nameof(TextType));
            }
        }

        private string _complexityLevel;
        public string ComplexityLevel {
          get {
            return _complexityLevel;
          }
          set {
            _complexityLevel = value;
            OnPropertyChanged(nameof(ComplexityLevel));
          }
        }

        private ObservableCollection<string> _languages = new ObservableCollection<string>();
        public ObservableCollection<string> Languages {
            get {
                return _languages;
            }
            set {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        private string _language = "english";
        public string Language {
            get {
                return _language;
            }
            set {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        private string _textSubject = "random";
        public string TextSubject {
            get {
                return _textSubject;
            }
            set {
                _textSubject = value;
                OnPropertyChanged(nameof(TextSubject));
            }
        }

         public RelayCommand SendPromptCommand { get; }



    public async Task SendPrompt()
    {
      Console.WriteLine("Generating....");
      Dictionary<string, string> complexities = new Dictionary<string, string>
      {
        ["basic"] = "basic (no capital letters or punctuation, simple words)",
        ["average"] = "average (include capital letters and punctuation)",
        ["advanced"] = "advanced (include capital letters and punctuation, use difficult words)",
      };

      if (TextSubject == "")
      {
        _textSubject = "random";
      }

      if (TextType.ToUpper() == "WORDS")
      {
        _textType = "random words (don't use any capital letters or punctuation)";
      }else if (TextType.ToUpper() == "SENTENCES")
      {
        _textType = "Completely random sentences that don't have any connection, separated by dots. Use capital letters.";
      }else if (TextType.ToUpper() == "STORY")
      {
        foreach (var variable in complexities)
        {
          if (variable.Key == ComplexityLevel)
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
        // MaxTokens = 4096,
        N = 10
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
        Console.WriteLine($"Length({_textLength}): {ResponseTextArray.Length} {currentString}|||\n");
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

      ResponseText = closestString;
      Console.WriteLine($"Final: {ResponseText}");
    }
  }
}
