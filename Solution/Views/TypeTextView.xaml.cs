using Solution.Services;
using Solution.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Solution.Views
{

    public class Word
    {
        public string Text { get; set; }
        public List<int> Indices { get; set; }

        public Word(string text, List<int> indices) {
            this.Text = text;
            this.Indices = indices;
        }
    }

    public partial class TypeTextView : UserControl, ITextUpdater
    {
        public List<String> Colors { get; set; }

        public ICommand OnTextInputCommand
        {
            get { return (ICommand)GetValue(OnTextInputCommandProperty); }
            set { SetValue(OnTextInputCommandProperty, value); }
        }

        public static readonly DependencyProperty OnTextInputCommandProperty =
            DependencyProperty.Register("OnTextInputCommand", typeof(ICommand), typeof(TypeTextView), new PropertyMetadata(null));



        public TypeTextView()
        {
            InitializeComponent();
            this.Loaded += (s, e) => this.Focus();
            ServiceLocator.RegisterTextUpdater(this);
            Colors = new List<String> { "#FFFFFF", "#CC4C4C", "#F2B233", "#752B33" };
        }

        public void updateText(List<Word> words)
        {
            DynamicTextDisplay.Inlines.Clear();

            foreach(var word in words)
            {
                for (int i = 0; i < word.Text.Length; i++)
                {
                    char character = word.Text[i];
                    Brush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#777777"));

                    if (i < word.Indices.Count && word.Indices[i] >= 0 && word.Indices[i] < Colors.Count)
                    {
                        String hex = Colors[word.Indices[i]];
                        // Console.WriteLine(hex);
                        color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
                    }

                    DynamicTextDisplay.Inlines.Add(new Run(character.ToString()) { Foreground=color});
                }
                // implement some kind of word wrapping here
                DynamicTextDisplay.Inlines.Add(" ");
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            string inputText = e.Text;
            System.Diagnostics.Debug.WriteLine($"Input text: {inputText}");

            OnTextInputCommand?.Execute(e.Text);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Back)
            {
                var viewModel = DataContext as TypeTextViewModel;

                if (viewModel != null)
                {
                    viewModel.DeleteCharacter();
                }

                e.Handled = true;
            } else if (e.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TypeTextViewModel;

            if (viewModel != null)
            {
                OnTextInputCommand = viewModel.PressChar;
            }
        }
    }
}
