﻿using Solution.Helpers;
using Solution.Services;
using Solution.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Solution.Helpers;
using System.Windows;

namespace Solution.ViewModels
{
    /*public class FormattedTextLine
    {
        public ObservableCollection<Run> TextRuns { get; set; }

        public FormattedTextLine()
        {
            TextRuns = new ObservableCollection<Run>();
        }

        public void AddRun(string text, SolidColorBrush color)
        {
            TextRuns.Add(new Run(text) { Foreground = color });
        }
    }*/

    public class CharacterEventCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CharacterEventCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }

    public class TypeTextViewModel : BaseViewModel
    {
        private readonly ITextUpdater? _textUpdater;
        //public ObservableCollection<FormattedTextLine> Lines { get; set; }

        public ICommand MyCommand { get; private set; }

        public ICommand PressChar { get; private set; }

        public ICommand PressBackspace { get; private set; }

        public TypeTextViewModel()
        {
            MyCommand = new RelayCommand(ExecuteMyCommand);
            PressChar = new CharacterEventCommand(ProcessChar);
            PressBackspace = new RelayCommand(DeleteCharacter);
            tempList = new List<int> { };
            TheText = new List<string> { "Hello", "my", "beautiful", "world!" };
            UserInput = new List<string> { "" };

            ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
        }

        private void updateText(List<Word> words)
        {
            ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
            if (_textUpdater != null)
            {
                _textUpdater.updateText(words);
            }
        }

        public List<string> TheText;
        public List<string> UserInput;
        public List<int> tempList;

        private void ProcessChar(object parameter)
        {
            /*var text = parameter as string;
            if (text != null)
            {
                tempList.Add(0);
                updateText(new List<Word>
                {
                    new Word("temp", tempList),
                });
            } else
            {
                updateText(new List<Word>
                {
                    new Word("no text", new List<int> {}),
                });
            }*/
            var text = parameter as string;
            if (text == " ")
            {
                UserInput.Add("");
            } else if (text?.Length == 1)
            {
                UserInput[UserInput.Count - 1] = UserInput[UserInput.Count - 1] + text;
            }
            updateInput();
        }

        private void updateInput()
        {
            List<Word> myList = new List<Word> { };
            for (int w = 0; w < TheText.Count; w++)
            {
                string word = TheText[w];
                string typedWord = "";
                if (w < UserInput.Count)
                {
                    typedWord = UserInput[w];
                }
                List<int> intList = new List<int> { };

                for (int i = 0; i < word.Length; i++)
                {
                    //char character = word[i];
                    if (i < typedWord.Length)
                    {
                        if (typedWord[i].Equals(word[i]))
                        {
                            intList.Add(0);
                        } else
                        {
                            intList.Add(1);
                        }
                    }
                    else if (w < UserInput.Count - 1)
                    {
                        intList.Add(2);
                    }
                }

                if (typedWord.Length > word.Length)
                {
                    for (int i = word.Length; i < typedWord.Length; i++)
                    {
                        intList.Add(3);
                    }
                    word = word + typedWord.Substring(word.Length, typedWord.Length - word.Length);
                }

                myList.Add(new Word(word, intList));
            }
            updateText(myList);
        }

        private void ExecuteMyCommand()
        {
            ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
            updateInput();
        }

        public void DeleteCharacter()
        {
            if (UserInput[UserInput.Count-1].Length > 0) {
                UserInput[UserInput.Count-1] = UserInput[UserInput.Count-1].Substring(0, UserInput[UserInput.Count - 1].Length - 1);
                updateInput();
            } else if (UserInput.Count > 1) {
                UserInput.RemoveAt(UserInput.Count - 1);
                updateInput();
            }
        }
    }
}
