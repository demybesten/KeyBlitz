using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using System.Reflection;

namespace Solution.ViewModels
{
    public class KBViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _inputText;
        private char _displayCharacter;
        public string[] woorden3;
        public int woordencount2;
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

        private void UpdateDisplayCharacter()
        {
          
            DisplayCharacter = !string.IsNullOrEmpty(InputText) ? InputText[woordencount] : default(char);

            // Ieder getypte karakter wordt opgeslagen in de array
            woorden3[woordencount2] = woorden3[woordencount2] + DisplayCharacter.ToString();

            woordencount++;

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public ICommand RemoveCharacter { get; }

        public ICommand Spatie { get; }
        public ICommand Ent { get; }

        public RelayCommand<string> AddCharacterToString { get; }
        /*Command aangemaakt voor toevoegen van een karakter */
        public RelayCommand<string> AddToKeyStackCommand { get; }

        private string _woord;
        public string gewijzigdeWaarde;
        public string woord
        {
            get { return _woord; }
            set
            {
                if (_woord != value)
                {
                    _woord = value;
                    RaisePropertyChanged(nameof(woord));
                }
            }
        }

        public int woordencount = 0;
        
       
        

        public KBViewModel()
        {
            // Dit is de array waarin de woorden worden opgeslagen, de 5 is puur voor het testen
            woorden3 = new string[5];
            // command voor de backspace functie
            RemoveCharacter = new RelayCommand(DeleteCharacter);
            // command voor de spatie functie
            Spatie = new RelayCommand(Space);
            // command voor de test functie met enter
            Ent = new RelayCommand(Enter);
            

        }


        public void DeleteCharacter()
        {
            MessageBox.Show("int waarde is " + woordencount2);
            if (woorden3[0] == null)
            {
                MessageBox.Show("type eerst wat");
            }
            else
            {
                if (woorden3[woordencount2 + 1] != null)
                {
                    MessageBox.Show($"Er staat data in array[1]: {woorden3[woordencount2]}");
                    string origineleWaarde = woorden3[woordencount2];

                    // Verwijder de laatste letter
                    string nieuweWaarde = origineleWaarde.Substring(0, origineleWaarde.Length - 1);
                    woorden3[woordencount2] = nieuweWaarde;
                }
                else
                {
                    MessageBox.Show("Er staat geen data in array[1].");
                    woordencount2--;
                }
            }
        }

        // Dit is de spatie functie, het is de bedoeling
        // dat na iedere spatie een nieuwe waarde in de array wordt gemaakt,
        // dus na een spatie gaat de array van array[0] naar array[1]
        public void Space()
        {
            MessageBox.Show(woorden3[woordencount2]);

            woordencount2++;

        }
        // Dit is een test functie, om te kijken of de woorden goed worden opgeslagen in de array

        public void Enter()
        {
            foreach (string getal in woorden3)
            {
                MessageBox.Show(getal);
            }
        }
    }
}

