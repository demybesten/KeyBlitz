using System;

namespace Solution.Services;

public class PassTestStats : IDataService
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
        OnDataChanged();
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
        OnDataChanged();
      }
    }
  }

  private double _amountOfCorrectWords;
  public double AmountOfCorrectWords
  {
    get { return _amountOfCorrectWords; }
    set
    {
      if (_amountOfCorrectWords != value)
      {
        _amountOfCorrectWords = value;
        OnDataChanged();
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
        OnDataChanged();
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
        OnDataChanged();
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
        OnDataChanged();
      }
    }
  }
  private int _accuracy;
  public int Accuracy {
    get { return _accuracy; }
    set
    {
      if (_accuracy != value)
      {
        _accuracy = value;
        OnDataChanged();
      }
    }
  }

  //Slaat stopwatch value op en wordt gebruikt om te binden aan een label
  private string _elapsedTime;
  public string ElapsedTime {
    get {
      return _elapsedTime;
    }
    set {
      if (_elapsedTime != value)
      {
        _elapsedTime = value;
        OnDataChanged();
      }
    }
  }
  public event EventHandler DataChanged;
  protected virtual void OnDataChanged()
  {
    DataChanged?.Invoke(this, EventArgs.Empty);
  }
}
