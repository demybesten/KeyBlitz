namespace Solution.Helpers;

public class GetLastChar
{
    public static char GetLastCharacter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            // You can handle this case according to your requirements.
            // For now, let's return a default character.
            return '\0';
        }

        return input[input.Length - 1];
    }

}