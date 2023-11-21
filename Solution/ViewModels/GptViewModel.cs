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

    public ICommand SendPromptCommand { get; }

    public GptViewModel()
    {
      SendPromptCommand = new RelayCommand(async () => await SendPrompt(), () => true);
    }

    public async Task SendPrompt()
    {
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
          { new ChatMessage("user", "Generate a text that is 5 words long dont include any special characters") }),
        Model = Models.ChatGpt3_5Turbo,
        Temperature = 0.5F,
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
