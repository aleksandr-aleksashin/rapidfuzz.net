using System.Linq;
using RapidFuzz.Net.Delegates;

namespace RapidFuzz.Net;

public static class DefaultPreprocessor
{
    /// <summary>
    /// This function preprocesses a string by:
    /// removing all non alphanumeric characters
    /// trimming whitespaces
    /// converting all characters to lower case
    /// </summary>
    public static Preprocessor Instance = Default;

    private static string Default(string s)
    {
        return new string(s.Where(c => (char.IsLetterOrDigit(c) ||
                                        char.IsWhiteSpace(c)))
                           .ToArray()).Trim()
                                      .ToLower();
    }
}
