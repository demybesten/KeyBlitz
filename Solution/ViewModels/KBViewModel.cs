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

namespace Solution.ViewModels
{
    public class KBViewModel : ViewModelBase
    {

        /*Collectie aangemaakt voor het front-end gedeelte */
        public ObservableCollection<string> FruitStack { get; set; }

        /*Stack aangemaakt voor het back-end gedeelte */
        Stack<char> CharacterStack = new Stack<char>();

        /*Command aangemaakt voor verwijderen van een karkter  */
        public ICommand RemoveCharacter { get; }

        /*Command aangemaakt voor Spatie*/
        public ICommand Spatie { get; }

        /*Command aangemaakt voor toevoegen van een karakter */
        public RelayCommand<string> AddToKeyStackCommand { get; }

        public KBViewModel()
        {
            /*Stack aangemaakt voor front-end */
            FruitStack = new ObservableCollection<string>();
            /*verwijder command */
            RemoveCharacter = new RelayCommand(DeleteCharacter);
            Spatie = new RelayCommand(Space);
            /*add command */
            AddToKeyStackCommand = new RelayCommand<string>(key => AddToKeyStack(key));

        }

        /*functie voor toevoegen karakter*/
        public void AddToKeyStack(string key)
        {
            /* Ingedrukte toets waarde omzetten naar char*/
            char eersteChar = key[0];
            /*Ingedrukte toets waarde op stack plaatsen voor back-end*/
            CharacterStack.Push(eersteChar);
            /*Ingedrukte toets waarde op stack plaatsen voor front-end*/
            FruitStack.Add(key);

        }

        /*functie voor verwijderen karakter*/
        private void DeleteCharacter()
        {
            /*Controleer of er items in de stack staan*/
            if (CharacterStack.Count > 0)
            {
                /*Verwijder karakter van de stack voor back-end*/
                CharacterStack.Pop();
                /*Verwijder karakter van de stack voor front-end*/
                FruitStack.RemoveAt(CharacterStack.Count);
            }

        }
        /*functie voor spatie karakter*/
        public void Space()
        {
            /*Spatie op stack plaatsen voor back-end*/
            CharacterStack.Push(' ');
        
            /*Spatie op stack plaatsen voor front-end*/
            FruitStack.Add(" ");
        }
      
        
    }
}