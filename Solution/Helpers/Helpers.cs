using System;

namespace Solution.Helpers;

public class Helpers
{
  public static int CountWords(string input)
  {
    return input.Split(new char[] { ' ', '.', ',', ';', '!' }, StringSplitOptions.RemoveEmptyEntries).Length;
  }
}
