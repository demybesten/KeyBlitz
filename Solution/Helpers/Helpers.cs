using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Solution.Helpers;

public class Helpers
{
  public static int CountWords(string input)
  {
    return input.Split(new char[] { ' ', '.', ',', ';', '!' }, StringSplitOptions.RemoveEmptyEntries).Length;
  }
}
