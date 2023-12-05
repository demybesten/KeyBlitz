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

    public class RelayCommand : ICommand
    {
        private Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }

    public class TypeTextViewModel : BaseViewModel
    {
        private readonly ITextUpdater? _textUpdater;
        //public ObservableCollection<FormattedTextLine> Lines { get; set; }

        public ICommand MyCommand { get; }

        public TypeTextViewModel()
        {
            MyCommand = new RelayCommand(ExecuteMyCommand);
            //Lines = new ObservableCollection<FormattedTextLine>();

            //InitializeText();

        }

        private void ExecuteMyCommand()
        {
            ITextUpdater? _textUpdater = ServiceLocator.GetTextUpdater();
            if (_textUpdater != null)
            {
                _textUpdater.updateText(new List<Word>{
                    new Word("Working!", new List<int> {0,0,0}),
                });
            }
        }

        public void InitializeText()
        {
            /*var line1 = new FormattedTextLine();
            line1.AddRun("Hell", Brushes.Black);
            line1.AddRun("o ", Brushes.Orange);
            line1.AddRun("World", Brushes.Red);
            Lines.Add(line1);*/
        }
    }
}
