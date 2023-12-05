
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Xml.Schema;

namespace Solution.ViewModels
{
    public class KBViewModel : ViewModelBase, INotifyPropertyChanged
    {
        // Input veld waarde
        private string _inputText;
        // laatst getype karakter
        private char _displayCharacter;
        // string met getypte woorden
        private string[] woorden3;
        // Array cel waarde
        public int woordencount;


        // Getter en setter Input veld waarde
        public string InputText
        {
            get { return _inputText; }
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    OnPropertyChanged();
                    UpdateDisplayCharacter();
                }
            }
        }

        // Getter en setter laatst getype karakter
        public char DisplayCharacter
        {
            get { return _displayCharacter; }
            set
            {
                if (_displayCharacter != value)
                {
                    _displayCharacter = value;
                    OnPropertyChanged();
                }
            }
        }

        public KBViewModel()
        {
            // array waar woorden in komen
            woorden3 = new string[10];
            // woorden counter
            woordencount = 0;

            // Functie voor backspace
            RemoveCharacter = new RelayCommand(DeleteCharacter);
            // Functie voor spatie
            Spatie = new RelayCommand(Space);

        }

        // Comand voor backspace
        public ICommand RemoveCharacter { get; }

        // Comand voor spatie
        public ICommand Spatie { get; }
  

        // Functie voor zichtbaar maken van laatst getypte karakter
        private void UpdateDisplayCharacter()
        {
            DisplayCharacter = !string.IsNullOrEmpty(InputText) ? InputText[InputText.Length - 1] : default(char);

           
        }
        // Functie voor backspace
        public void DeleteCharacter()
        {
            if (woordencount == 0 && string.IsNullOrEmpty(InputText))
            {
                MessageBox.Show("geen woorden beschikbaar");

            }
            if (woordencount != 0 && string.IsNullOrEmpty(InputText))
            {
                woorden3[woordencount] = "";
                woordencount--;
                InputText = woorden3[woordencount] + " ";
            }
        }
        // Functie voor spatie
        public void Space()
        {
            woorden3[woordencount] = InputText;
            woordencount++;
            InputText = "";
        }
   

       // event voor het live bijwerken
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
