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

namespace Solution.ViewModels
{
    public class KBViewModel : ViewModelBase, INotifyPropertyChanged
    {

     

        /*Command aangemaakt voor verwijderen van een karkter  */
        public ICommand RemoveCharacter { get; }

        /*Command aangemaakt voor Spatie*/
        public ICommand Spatie { get; }

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
        private ObservableCollection<string> _woorden2;

        public ObservableCollection<string> woorden2
        {
            get { return _woorden2; }
            set
            {
                _woorden2 = value;
                // Voeg eventueel hier logica toe om wijzigingen in de array af te handelen
            }
        }

        public KBViewModel()
        {
            woorden2 = new ObservableCollection<string>();

            /*Stack aangemaakt voor front-end */
            /*verwijder command */
            RemoveCharacter = new RelayCommand(DeleteCharacter);
            Spatie = new RelayCommand(Space);

            AddCharacterToString = new RelayCommand<string>(key => AddToString(key));

            /*add command */

        }

  


        public void AddToString(string key)
        {

            woord = woord + key;



        }


        /*functie voor verwijderen karakter*/

        private void DeleteCharacter()
        {

            if (woordencount > 0)
            {
                woordencount--;

                // Haal de oude waarde op
                woord = woorden2[woordencount];
                woord = woord.Substring(0, woord.Length - 1);

                // Wijzig de oude waarde (hier kun je je eigen logica voor wijziging toevoegen)
                // Bijvoorbeeld, je zou een nieuw venster kunnen openen om de waarde te laten bewerken.
                // Hieronder wordt eenvoudig de originele waarde met " - gewijzigd" toegevoegd.
                gewijzigdeWaarde = woord;

                // Plaats de gewijzigde waarde op de oorspronkelijke positie
                woorden2[woordencount] = gewijzigdeWaarde;
                if (gewijzigdeWaarde.Length == 0) { woordencount--; }
                MessageBox.Show($"Gewijzigde waarde: {gewijzigdeWaarde}");
                woordencount++;
            }

            else
            {
                MessageBox.Show("Geen oude waarde beschikbaar om te wijzigen.");
            }

        }
        /*functie voor spatie karakter*/
        public void Space()
        {
            try
            {

                if (woord.Length > 0)
                {

                    woorden2.Add(woord);

                    woordencount++;

                    MessageBox.Show("Waarde staat in de lijst");
                    woord = string.Empty; // Reset het woord voor de volgende invoer
                }
                else
                {
                    MessageBox.Show("Het woord mag niet leeg zijn!");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}");
            }
        }




    }
}