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

  // public static char GetLastCharacter(string input)
  // {
  //   if (string.IsNullOrEmpty(input))
  //   {
  //     // You can handle this case according to your requirements.
  //     // For now, let's return a default character.
  //     return '\0';
  //   }
  //
  //   return input[input.Length - 1];
  // }
}
