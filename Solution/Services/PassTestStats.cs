using System;
using System.ComponentModel;
using Solution.Helpers;
namespace Solution.Services;

public class PassTestStats : IDataService, INotifyPropertyChanged
{
    private double _amountOfCorrectChars;
    public double AmountOfCorrectChars
    {
        get { return _amountOfCorrectChars; }
        set
        {
            if (_amountOfCorrectChars != value)
            {
                _amountOfCorrectChars = value;
                OnPropertyChanged(nameof(AmountOfCorrectChars));
            }
        }
    }

    private double _amountOfTypedChars;
    public double AmountOfTypedChars
    {
        get { return _amountOfTypedChars; }
        set
        {
            if (_amountOfTypedChars != value)
            {
                _amountOfTypedChars = value;
                OnPropertyChanged(nameof(AmountOfTypedChars));
            }
        }
    }

    private double _amountOfTypedWords;
    public double AmountOfTypedWords
    {
        get { return _amountOfTypedWords; }
        set
        {
            if (_amountOfTypedWords != value)
            {
                _amountOfTypedWords = value;
                OnPropertyChanged(nameof(AmountOfTypedWords));
            }
        }
    }

    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            if (_score != value)
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }
    }

    private int _wpm;
    public int Wpm
    {
        get { return _wpm; }
        set
        {
            if (_wpm != value)
            {
                _wpm = value;
                OnPropertyChanged(nameof(Wpm));
            }
        }
    }

    private int _cpm;
    public int Cpm
    {
        get { return _cpm; }
        set
        {
            if (_cpm != value)
            {
                _cpm = value;
                OnPropertyChanged(nameof(Cpm));
            }
        }
    }

    private int _accuracy;
    public int Accuracy
    {
        get { return _accuracy; }
        set
        {
            if (_accuracy != value)
            {
                _accuracy = value;
                OnPropertyChanged(nameof(Accuracy));
            }
        }
    }
    private bool _multiplayer;
    public bool Multiplayer
    {
        get { return _multiplayer; }
        set
        {
            if (_multiplayer != value)
            {
                _multiplayer = value;
                OnPropertyChanged(nameof(Multiplayer));
            }
        }
    }
    private string _elapsedTime;
    public string ElapsedTime
    {
        get { return _elapsedTime; }
        set
        {
            if (_elapsedTime != value)
            {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }
    }
    public string? Text { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}