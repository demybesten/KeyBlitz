using Solution.Services;
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

        public TypeTextView()
        {
            InitializeComponent();
            ServiceLocator.RegisterTextUpdater(this);
            Colors = new List<String> { "#FFFFFF", "#CC4C4C", "#F2B233", "#752B33" };
            /*Word myWord = new Word("Test", new List<int> { 0, 0 });
            updateText(new List<Word> {
                new Word("Helloo", new List<int> { 0, 0, 1, 0, 0, 3}),
                new Word("beautiful", new List<int> { 0, 0, 0, 0, 0, 0, 0, 2, 2}),
                new Word("world!", new List<int>{ 0, 1, 0, 0})
            });*/
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
                        //color = (Brush)Colors[word.Indices[i]];
                        //color = Brushes.Red;
                        String hex = Colors[word.Indices[i]];
                        Console.WriteLine(hex);
                        color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
                    }

                    DynamicTextDisplay.Inlines.Add(new Run(character.ToString()) { Foreground=color});
                }
                // implement some kind of word wrapping here
                DynamicTextDisplay.Inlines.Add(" ");
            }
        }
    }
}
