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

    class TextHelper
    {
        public static double MeasureTextWidth(string text, TextBlock textBox)
        {
            // Create a new FormattedText object with the same properties as the TextBox
            FormattedText formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch),
                textBox.FontSize,
                Brushes.Black, // The brush doesn't affect the size
                VisualTreeHelper.GetDpi(textBox).PixelsPerDip // This ensures the correct scaling for the device's DPI settings
            );

            // Return the width of the text
            return formattedText.Width;
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

        /*public void updateText(List<Word> words)
        {
            DynamicTextDisplay.Inlines.Clear();

            string currentLine = "";
            double lineLen = this.ActualWidth*0.75;

            foreach(var word in words)
            {
                double nextLen = TextHelper.MeasureTextWidth(currentLine + word.Text, DynamicTextDisplay);
                if (nextLen > lineLen)
                {
                    currentLine = "";
                    DynamicTextDisplay.Inlines.Add("\n");
                }
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

                    if (i >= DynamicTextDisplay.Inlines.Count)
                    {
                        DynamicTextDisplay.Inlines.Add(new Run(character.ToString()) { Foreground = color });
                    } else
                    {
                        Run myRun = (Run)DynamicTextDisplay.Inlines.ElementAt(i);
                        myRun.Foreground = color;
                        myRun.Text = word.Text;
                    }
                }
                // implement some kind of word wrapping here
                DynamicTextDisplay.Inlines.Add(" ");
                currentLine = currentLine + word.Text + " ";
            }
        }*/

        private List<Word> previousWords;
        private List<int> wordsPerLine;

        public void updateText(List<Word> words, bool resetWordWrap = false)
        {
            double lineLen = this.ActualWidth * 0.75;

            if (previousWords == null || resetWordWrap)
            {
                previousWords = new List<Word>();
                wordsPerLine = new List<int>();
                CalculateWordsPerLine(words, lineLen);
            }

            int inlineIndex = 0, wordIndex = 0, currentLineIndex = 0, currentLineWordCount = 0;
            string currentLine = "";

            while (wordIndex < words.Count)
            {
                var word = words[wordIndex];
                var previousWord = wordIndex < previousWords.Count ? previousWords[wordIndex] : null;

                if (previousWord != null && word.Indices.SequenceEqual(previousWord.Indices))
                {
                    // Skip unchanged word
                    if (currentLineIndex < wordsPerLine.Count && currentLineWordCount >= wordsPerLine[currentLineIndex])
                    {
                        currentLineIndex++;
                        currentLineWordCount = 0;
                        inlineIndex++;
                    }
                        inlineIndex += word.Text.Length + 1; // +1 for the space after the word
                    currentLineWordCount++;
                }
                else
                {
                    if (currentLineIndex < wordsPerLine.Count && currentLineWordCount >= wordsPerLine[currentLineIndex])
                    {
                        int wpl = wordsPerLine[currentLineIndex];
                        System.Diagnostics.Debug.WriteLine(wpl);
                        currentLine = "";
                        UpdateOrAddInline(ref inlineIndex, "\n");
                        currentLineIndex++;
                        currentLineWordCount = 0;
                    }

                    for (int i = 0; i < word.Text.Length; i++)
                    {
                        char character = word.Text[i];
                        Brush color = GetBrushForWord(word, i);

                        UpdateOrAddInline(ref inlineIndex, character.ToString(), color);
                    }

                    // Add space after the word
                    UpdateOrAddInline(ref inlineIndex, " ");
                    currentLine += word.Text + " ";
                    currentLineWordCount++;
                }

                wordIndex++;
            }

            // Ensure a new line is not added unnecessarily at the end
            if (inlineIndex < DynamicTextDisplay.Inlines.Count && DynamicTextDisplay.Inlines.ElementAt(inlineIndex) is Run lastRun && lastRun.Text == "\n")
            {
                DynamicTextDisplay.Inlines.Remove(lastRun);
            }

            // Remove any remaining inlines from previous content
            while (DynamicTextDisplay.Inlines.Count > inlineIndex)
            {
                DynamicTextDisplay.Inlines.Remove(DynamicTextDisplay.Inlines.ElementAt(inlineIndex));
            }

            previousWords = new List<Word>(words);
        }

        private void CalculateWordsPerLine(List<Word> words, double lineLen)
        {
            string currentLine = "";
            int wordCount = 0;

            foreach (var word in words)
            {
                double nextLen = TextHelper.MeasureTextWidth(currentLine + word.Text + " ", DynamicTextDisplay); // Include space in measurement
                if (nextLen > lineLen && currentLine != "")
                {
                    wordsPerLine.Add(wordCount);
                    currentLine = "";
                    wordCount = 0;
                }

                currentLine += word.Text + " ";
                wordCount++;
            }

            if (wordCount > 0 || wordsPerLine.Count == 0) // Handle case for very first word and last line
            {
                wordsPerLine.Add(wordCount);
            }
            //System.Diagnostics.Debug.WriteLine("Line 2 " + wordsPerLine[1]);
        }



        private void UpdateOrAddInline(ref int inlineIndex, string text, Brush color = null)
        {
            if (inlineIndex >= DynamicTextDisplay.Inlines.Count)
            {
                DynamicTextDisplay.Inlines.Add(new Run(text) { Foreground = color });
            }
            else
            {
                var myRun = (Run)DynamicTextDisplay.Inlines.ElementAt(inlineIndex);

                // Check if the current inline contains a space or newline
                bool isCurrentInlineSpaceOrNewline = myRun.Text == " " || myRun.Text == "\n";
                // Check if the new text is not a space or newline
                bool isNewTextNotSpaceOrNewline = text != " " && text != "\n";

                if (isCurrentInlineSpaceOrNewline && isNewTextNotSpaceOrNewline)
                {
                    // Insert a new inline at this position, moving the rest forwards
                    DynamicTextDisplay.Inlines.InsertBefore(myRun, new Run(text) { Foreground = color });
                }
                else
                {
                    // Update the existing inline
                    myRun.Foreground = color ?? myRun.Foreground;
                    myRun.Text = text;
                }
            }
            inlineIndex++;
        }


        private Brush GetBrushForWord(Word word, int index)
        {
            Brush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#777777")); // Default color

            if (index < word.Indices.Count && word.Indices[index] >= 0 && word.Indices[index] < Colors.Count)
            {
                string hex = Colors[word.Indices[index]];
                color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
            }

            return color;
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
