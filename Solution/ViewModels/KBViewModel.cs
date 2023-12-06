using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Solution.Helpers;
using System;
using System.Linq;

namespace Solution.ViewModels
{
    public class KBViewModel : BaseViewModel, INotifyPropertyChanged
    {
        // Input veld waarde
        private string _inputText;
        // laatst getype karakter
        private char _displayCharacter;
        // string met getypte woorden
        private string[] woorden3;
        // Array cel waarde
        public int woordencount;
        public int Karaktercount;
        public string[] woorden4 = { "Volvo", "BMW", "Ford", "Mazda" };

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
            Karaktercount = 0;
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
            
            Validatie2();
            Karaktercount++;

        }
        // Functie voor backspace
        public void DeleteCharacter()
        {
            if (woordencount == 0 && string.IsNullOrEmpty(InputText))
            {
                MessageBox.Show("geen woorden beschikbaar");

            }
            if (!string.IsNullOrEmpty(InputText))
            {
                Karaktercount--;
                Karaktercount--;
            }
            if (woordencount != 0 && string.IsNullOrEmpty(InputText))
            {
                ;
                woorden3[woordencount] = "";
                woordencount--;
                InputText = woorden3[woordencount] + " ";
                int stringLength = InputText.Length;
                stringLength = stringLength - 2;
                Karaktercount = stringLength;
            } 
        }
        // Functie voor spatie
        public void Space()
        {
            woorden3[woordencount] = InputText;
            Validatie();
            woordencount++;
            InputText = "";
            Karaktercount = 0;
        }
        public void Validatie()
        {
            string geselecteerdWoord = woorden4[woordencount];

            // Splits de string in een array van karakters
            char[] karaktersArray = geselecteerdWoord.ToCharArray();
            if (karaktersArray.Length == InputText.Length)
            {
                if (woorden3[woordencount].Equals(woorden4[woordencount]))
                {
                    MessageBox.Show("correct");
                }
            }
            else if (karaktersArray.Length > InputText.Length)
            {
                for (int i = Karaktercount; i != karaktersArray.Length; i++)
                {
                    MessageBox.Show(karaktersArray[i] + "  mist (kleurt Geel)");
                }
            }
          
        }
        public void Validatie2()
        {
                string geselecteerdWoord = woorden4[woordencount];

                 // Splits de string in een array van karakters
                char[] karaktersArray = geselecteerdWoord.ToCharArray();
        /*    if (DisplayCharacter != '\0')*/
        if(karaktersArray.Length >= InputText.Length)
            {
                if (karaktersArray[Karaktercount].Equals(DisplayCharacter))
                {
                    MessageBox.Show(DisplayCharacter + "  is correct (kleurt Wit)");
                } 
                else 
                {
                    MessageBox.Show(DisplayCharacter + " is incorrect (kleurt Rood)");
                }

            }
            else if (karaktersArray.Length < InputText.Length)
            {
                MessageBox.Show(DisplayCharacter + " is een extra karakter (kleurt Donker Rood)");

            }



        }



        // event voor het live bijwerken
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
