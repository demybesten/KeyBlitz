using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

      private int _complexity;
      public int Complexity
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
    public RelayCommand SendPromptCommand { get; }

    public GptViewModel()
    {
      SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);
    }

    public async Task SendPrompt()
    {
        string userMessage = "a short text about baby seals";
        switch (Complexity)
        {
            case 1:
                userMessage = $"Generate a text on the subject of '{Subject}'" +
                                     $" that is {Length} words long with Simple sentences and phrases and common and frequently used words also don't use any special characters";
                break;
            
           case 2:
                userMessage = $"Generate a text on the subject of '{Subject}'" +
                                    $" that is {Length} words long with Expanded vocabulary with common words and basic sentence structures with some variety ";
                break;
           
           case 3:
               userMessage = $"Generate a text on the subject of '{Subject}'" +
                             $" that is {Length} words long with More varied sentence structures and a mix of common and less common words ";
               break;
           
            case 4:
                userMessage = $"Generate a text on the subject of '{Subject}'" +
                              $" that is {Length} words long with Complex sentence structures and advanced vocabulary and varied word choices";
                break;
            case 5:
                userMessage = $"Generate a text on the subject of '{Subject}'" +
                              $" that is {Length} words long with advanced and nuanced language and rich vocabulary with technical or specialized terms";
                break;
            
        }

        
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
          { new ChatMessage("user", userMessage) }),
        Model = Models.ChatGpt3_5Turbo,
        Temperature = 0.1F,
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
