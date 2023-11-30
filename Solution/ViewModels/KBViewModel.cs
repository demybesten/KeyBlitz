using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using GalaSoft.MvvmLight.Command;
using System.Diagnostics;
using System.Windows.Threading;

namespace Solution.ViewModels
{
  public class KBViewModel : BaseViewModel
  {
    private DispatcherTimer timer;
    private Stopwatch stopWatch;

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
      timer = new DispatcherTimer();
      stopWatch = new Stopwatch();
      timer.Tick += timer_Tick;

      /*Stack aangemaakt voor front-end */
      FruitStack = new ObservableCollection<string>();
      /*verwijder command */
      RemoveCharacter = new RelayCommand(DeleteCharacter);
      Spatie = new RelayCommand(Space);
      /*add command */
      AddToKeyStackCommand = new RelayCommand<string>(key => AddToKeyStack(key));

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

    /*functie voor toevoegen karakter*/
    public void AddToKeyStack(string key)
    {
      stopWatch.Start();
      timer.Start();

      /* Ingedrukte toets waarde omzetten naar char*/
      char eersteChar = key[0];
      /*Ingedrukte toets waarde op stack plaatsen voor back-end*/
      CharacterStack.Push(eersteChar);
      /*Ingedrukte toets waarde op stack plaatsen voor front-end*/
      FruitStack.Add(key);

      if (FruitStack.Count >= 30)
      {
        stopWatch.Stop();
      }
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
