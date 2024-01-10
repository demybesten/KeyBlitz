using System;
namespace Solution.Services;
public interface IDataService
{
    double AmountOfCorrectChars { get; set; }
    double AmountOfTypedChars { get; set; }
    double AmountOfTypedWords { get; set; }

    int Score { get; set; }

    int Wpm { get; set; }

    int Cpm { get; set; }

    int Accuracy { get; set; }

    string ElapsedTime { get; set; }
    string Text {  get; set; }

    bool Multiplayer {  get; set; }
}