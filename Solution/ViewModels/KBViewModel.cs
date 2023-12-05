using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows;
using System.Windows.Threading;
using Solution.Helpers;

namespace Solution.ViewModels
{
    public class KBViewModel : BaseViewModel, INotifyPropertyChanged
    {

      public DispatcherTimer timer;
      public Stopwatch stopWatch;

      //Slaat stopwatch value op en wordt gebruikt om te binden aan een label
      private string _elapsedTime = String.Empty;
      public string ElapsedTime {
        get {
          return _elapsedTime;
        }
        set {
          _elapsedTime = value;
          OnPropertyChanged(nameof(ElapsedTime));
        }
      }

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
          stopWatch = new Stopwatch();
          timer = new DispatcherTimer();
          //live timer event
          timer.Tick += timer_Tick;
            // array waar woorden in komen
            woorden3 = new string[10];
            // woorden counter
            woordencount = 0;

            // Functie voor backspace
            RemoveCharacter = new RelayCommand(DeleteCharacter);
            // Functie voor spatie
            Spatie = new RelayCommand(Space);

        }

        void timer_Tick(object sender, EventArgs e)
        {
          //Als stopwatch runt
          if (stopWatch.IsRunning)
          {
            //Haalt time span op en format deze
            TimeSpan ts = stopWatch.Elapsed;
            ElapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
              ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
          }
        }

        // Comand voor backspace
        public ICommand RemoveCharacter { get; }

        // Comand voor spatie
        public ICommand Spatie { get; }


        // Functie voor zichtbaar maken van laatst getypte karakter
        private void UpdateDisplayCharacter()
        {
            DisplayCharacter = !string.IsNullOrEmpty(InputText) ? InputText[InputText.Length - 1] : default(char);

            stopWatch.Start();
            timer.Start();

            if (woordencount >= 5)
            {
              stopWatch.Stop();
            }
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
