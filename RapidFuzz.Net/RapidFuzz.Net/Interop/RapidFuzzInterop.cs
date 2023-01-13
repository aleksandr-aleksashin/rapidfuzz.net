using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RapidFuzz.Net.Interop;

[SuppressUnmanagedCodeSecurity]
internal static class RapidFuzzInterop
{
    private const string _lib = "NativeRapidFuzz";

    static RapidFuzzInterop()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return;
        }

        throw new NotSupportedException("Unsupported operation system.");
    }

    /// <summary>Calculates a simple ratio between two strings</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  Ratio("this is a test", "this is a test!") //score 96.55
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double Ratio(string s1,
                                      string s2,
                                      double scoreCutOff)
        => ratio(s1, s2, scoreCutOff);

    /// <summary>Helper method that returns the maximum of TokenSetRation and TokenSortRation(faster than manually executing the two functions)</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double TokenRatio(string s1,
                                           string s2,
                                           double scoreCutOff)
        => token_ratio(s1, s2, scoreCutOff);

    /// <summary>Calculates the Ratio of the optimal string alignment</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  PartialRatio("this is a test", "this is a test!") //score 100
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double PartialRatio(string s1,
                                             string s2,
                                             double scoreCutOff)
        => partial_ratio(s1, s2, scoreCutOff);

    /// <summary>Compares the words in the strings based on unique and common words between them using Ratio</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <example>
    ///  TokenSortRatio("fuzzy was a bear", "fuzzy fuzzy was a bear") //score 83.87
    ///  TokenSetRatio("fuzzy was a bear", "fuzzy fuzzy was a bear") //score 100
    /// </example>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double TokenSetRatio(string s1,
                                              string s2,
                                              double scoreCutOff)
        => token_set_ratio(s1, s2, scoreCutOff);

    /// <summary>Helper method that returns the maximum of PartialTokenSetRatio and PartialTokenSortRatio (faster than manually executing the two functions)</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double PartialTokenRatio(string s1,
                                                  string s2,
                                                  double scoreCutOff)
        => partial_token_ratio(s1, s2, scoreCutOff);

    /// <summary>Compares the words in the strings based on unique and common words between them using PartialRatio</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double PartialTokenSetRatio(string s1,
                                                     string s2,
                                                     double scoreCutOff)
        => partial_token_set_ratio(s1, s2, scoreCutOff);

    /// <summary>Sorts the words in the strings and calculates the PartialRatio between them</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double PartialTokenSortRatio(string s1,
                                                      string s2,
                                                      double scoreCutOff)
        => partial_token_sort_ratio(s1, s2, scoreCutOff);

    /// <summary>Sorts the words in the strings and calculates the Ratio between them</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double TokenSortRatio(string s1,
                                               string s2,
                                               double scoreCutOff)
        => token_sort_ratio(s1, s2, scoreCutOff);

    /// <summary>Calculates a quick ratio between two strings using Ratio</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double QRatio(string s1,
                                       string s2,
                                       double scoreCutOff)
        => q_ratio(s1, s2, scoreCutOff);

    /// <summary>Calculates a weighted ratio based on the other ratio algorithms</summary>
    /// <param name="s1"><paramref name="s1"/>  string to compare with <paramref name="s2"/></param>
    /// <param name="s2"><paramref name="s2"/> string to compare with <paramref name="s1"/> </param>
    /// <param name="scoreCutOff">Optional argument for a score threshold between 0% and 100%. Matches with a lower score than this number will not be returned. Defaults to 0.</param>
    /// <returns>The ratio between <paramref name="s1"/> and <paramref name="s2"/> or 0 when ratio less than <paramref name="scoreCutOff"/></returns>
    public static unsafe double WRatio(string s1,
                                       string s2,
                                       double scoreCutOff)
        => w_ratio(s1, s2, scoreCutOff);

    #region library calls

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double token_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                    [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                    double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double partial_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                      [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                      double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double token_set_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                        [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                        double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double partial_token_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                            [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                            double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double partial_token_set_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                                [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                                double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double partial_token_sort_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                                 [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                                 double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                              [MarshalAs(UnmanagedType.LPStr)] string s2,
                                              double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double token_sort_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                         [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                         double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double w_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                double score_cut_off);

    [DllImport(_lib, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern unsafe double q_ratio([MarshalAs(UnmanagedType.LPStr)] string s1,
                                                [MarshalAs(UnmanagedType.LPStr)] string s2,
                                                double score_cut_off);

    #endregion
}
