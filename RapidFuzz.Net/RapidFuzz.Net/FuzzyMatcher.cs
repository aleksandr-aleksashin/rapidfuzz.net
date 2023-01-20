using System.Collections.Generic;
using RapidFuzz.Net.Delegates;
using RapidFuzz.Net.Interop;

namespace RapidFuzz.Net;

public static class FuzzyMatcher
{
    public static IEnumerable<(double Score, int Index, string Value)> Extract(string text,
                                                                               IEnumerable<string> values,
                                                                               Scorer scorer,
                                                                               Preprocessor? preprocessor = null,
                                                                               double scoreCutOff = 0)
    {
        var index = 0;
        var preprocessedText = text;

        if (preprocessor is not null)
        {
            preprocessedText = preprocessor(preprocessedText);
        }

        foreach (var value in values)
        {
            var preprocessed = value;

            if (preprocessor is not null)
            {
                preprocessed = preprocessor(preprocessed);
            }

            var score = scorer(preprocessedText, preprocessed, scoreCutOff);

            if (score > scoreCutOff)
            {
                yield return (score, index, value);
            }
        }
    }

    public static double Extract(string s1, string s2, Scorer ratio, Preprocessor? preprocessor = null, double scoreCutOff = 0)
    {
        var preprocessedS1 = s1;
        var preprocessedS2 = s2;

        if (preprocessor is not null)
        {
            preprocessedS1 = preprocessor(preprocessedS1);
            preprocessedS2 = preprocessor(preprocessedS2);
        }

        return ratio(preprocessedS1, preprocessedS2, scoreCutOff);
    }

    /// <summary>Calculates a simple ratio between two strings</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  Ratio("this is a test", "this is a test!") //score 96.55
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double Ratio(string s1,
                               string s2,
                               double scoreCutOff = 0)
        => RapidFuzzInterop.Ratio(s1, s2, scoreCutOff);

    /// <summary>Helper method that returns the maximum of TokenSetRation and TokenSortRation(faster than manually executing the two functions)</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double TokenRatio(string s1,
                                    string s2,
                                    double scoreCutOff = 0)
        => RapidFuzzInterop.TokenRatio(s1, s2, scoreCutOff);

    /// <summary>Calculates the Ratio of the optimal string alignment</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  PartialRatio("this is a test", "this is a test!") //score 100
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double PartialRatio(string s1,
                                      string s2,
                                      double scoreCutOff = 0)
        => RapidFuzzInterop.PartialRatio(s1, s2, scoreCutOff);

    /// <summary>Compares the words in the strings based on unique and common words between them using Ratio</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  TokenSortRatio("fuzzy was a bear", "fuzzy fuzzy was a bear") //score 83.87
    ///  TokenSetRatio("fuzzy was a bear", "fuzzy fuzzy was a bear") //score 100
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double TokenSetRatio(string s1,
                                       string s2,
                                       double scoreCutOff = 0)
        => RapidFuzzInterop.TokenSetRatio(s1, s2, scoreCutOff);

    /// <summary>Sorts the words in the strings and calculates the Ratio between them</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  TokenSortRatio("fuzzy wuzzy was a bear", "wuzzy fuzzy was a bear") // score 100
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double TokenSortRatio(string s1,
                                        string s2,
                                        double scoreCutOff = 0)
        => RapidFuzzInterop.TokenSortRatio(s1, s2, scoreCutOff);

    /// <summary>Helper method that returns the maximum of PartialTokenSetRatio and PartialTokenSortRatio (faster than manually executing the two functions)</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double PartialTokenRatio(string s1,
                                           string s2,
                                           double scoreCutOff = 0)
        => RapidFuzzInterop.PartialTokenRatio(s1, s2, scoreCutOff);

    /// <summary>Compares the words in the strings based on unique and common words between them using PartialRatio</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double PartialTokenSetRatio(string s1,
                                              string s2,
                                              double scoreCutOff = 0)
        => RapidFuzzInterop.PartialTokenSetRatio(s1, s2, scoreCutOff);

    /// <summary>Sorts the words in the strings and calculates the PartialRatio between them</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double PartialTokenSortRatio(string s1,
                                               string s2,
                                               double scoreCutOff = 0)
        => RapidFuzzInterop.PartialTokenSortRatio(s1, s2, scoreCutOff);

    /// <summary>Calculates a quick ratio between two strings using Ratio</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double QRatio(string s1,
                                string s2,
                                double scoreCutOff = 0)
        => RapidFuzzInterop.QRatio(s1, s2, scoreCutOff);

    /// <summary>Calculates a weighted ratio based on the other ratio algorithms</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static double WRatio(string s1,
                                string s2,
                                double scoreCutOff = 0)
        => RapidFuzzInterop.WRatio(s1, s2, scoreCutOff);
}
