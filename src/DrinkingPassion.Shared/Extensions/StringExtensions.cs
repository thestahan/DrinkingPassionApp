using System.Text;
using System.Text.RegularExpressions;

namespace DrinkingPassion.Shared.Extensions;

public static class StringExtensions
{
    public static string RemoveAccents(this string text)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        byte[] tempBytes;
        tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(text);
        string asciiStr = Encoding.UTF8.GetString(tempBytes);

        return asciiStr;
    }

    public static string Slugify(this string phrase)
    {
        string output = phrase.RemoveAccents().ToLower();

        // Remove all special characters from the string.
        output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

        // Remove all additional spaces in favour of just one.
        output = Regex.Replace(output, @"\s+", " ").Trim();

        // Replace all spaces with the hyphen.
        output = Regex.Replace(output, @"\s", "-");

        return output;
    }
}
