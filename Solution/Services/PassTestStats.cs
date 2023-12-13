using System;

namespace Solution.Services;

public class PassTestStats : IDataService
{
  public double AmountOfCorrectChars { get; set; }
  public double AmountOfTypedChars { get; set; }
  public double AmountOfCorrectWords { get; set; }
  public int Score { get; set; }

  public int Wpm { get; set; }

  public int Cpm { get; set; }

  public int Accuracy { get; set; }

  public string ElapsedTime { get; set; }

  public string? Text { get; set; }
}
