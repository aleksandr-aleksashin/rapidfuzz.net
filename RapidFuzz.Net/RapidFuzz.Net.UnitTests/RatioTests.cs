using RapidFuzz.Net.Delegates;
using Xunit;

namespace RapidFuzz.Net.UnitTests
{
    // Tests data borrowed from rapidfuzz-cpp
    public class RatioTests
    {
        private class Metric
        {
            public Scorer Scorer { get; }
            public bool Symmetric { get; }

            public Metric(Scorer scorer, bool symmetric)
            {
                Scorer = scorer;
                Symmetric = symmetric;
            }
        }

        private static List<Metric> Metrics = new List<Metric>()
        {
            new(RapidFuzz.Ratio, true),
            new(RapidFuzz.PartialRatio, false),
            new(RapidFuzz.TokenSetRatio, true),
            new(RapidFuzz.TokenSortRatio, true),
            new(RapidFuzz.TokenRatio, true),
            new(RapidFuzz.PartialTokenSetRatio, false),
            new(RapidFuzz.PartialTokenSortRatio, false),
            new(RapidFuzz.PartialTokenRatio, false),
            new(RapidFuzz.WRatio, false),
            new(RapidFuzz.QRatio, true)
        };
        const string s1 = "new york mets";
        const string s2 = "new YORK mets";
        const string s3 = "the wonderful new york mets";
        const string s4 = "new york mets vs atlanta braves";
        const string s5 = "atlanta braves vs new york mets";
        const string s6 = "new york mets - atlanta braves";
        const string s7 = "new york city mets - atlanta braves";
        // test silly corner cases
        const string s8 = "{";
        const string s9 = "{a";
        const string s10 = "a{";
        const string s10a = "{b";

        [Fact]
        public void Equal()
        {
            Assert.Equal(100, RapidFuzz.Ratio(s1, s1));
            Assert.Equal(100, RapidFuzz.Ratio("test", "test"));
            Assert.Equal(100, RapidFuzz.Ratio(s8, s8));
            Assert.Equal(100, RapidFuzz.Ratio(s9, s9));
        }

        [Fact]
        public void PartialRatio()
        {
            Assert.Equal(100, RapidFuzz.PartialRatio(s1, s1));
            Assert.Equal(65, RapidFuzz.Ratio(s1, s3));
            Assert.Equal(100, RapidFuzz.PartialRatio(s1, s3));
        }

        [Fact]
        public void TokenSortRatio()
        {
            Assert.Equal(100, RapidFuzz.TokenSortRatio(s1, s1));
            Assert.Equal(100, RapidFuzz.TokenSortRatio("metss new york hello", "metss new york hello"));
        }

        [Fact]
        public void TokenSetRatio()
        {
            Assert.Equal(100, RapidFuzz.TokenSetRatio(s4, s5));
            Assert.Equal(100, RapidFuzz.TokenSetRatio(s8, s8));
            Assert.Equal(100, RapidFuzz.TokenSetRatio(s9, s9));
            Assert.Equal(50, RapidFuzz.TokenSetRatio(s10, s10a));
        }

        [Fact]
        public void PartialTokenSetRatio()
        {
            Assert.Equal(100, RapidFuzz.PartialTokenSetRatio(s4, s7));
        }

        [Fact]
        public void WRatioEqual()
        {
            Assert.Equal(100, RapidFuzz.WRatio(s1, s1));
        }

        [Fact]
        public void WRatioPartialMatch()
        {
            // a partial match is scaled by .9
            Assert.Equal(90, RapidFuzz.WRatio(s1, s3));
        }

        [Fact]
        public void WRatioMisorderedMatch()
        {
            // misordered full matches are scaled by .95
            Assert.Equal(95, RapidFuzz.WRatio(s4, s5));
        }

        [Fact]
        public void TwoEmptyStrings()
        {
            Assert.Equal(100, RapidFuzz.Ratio("", ""));
            Assert.Equal(100, RapidFuzz.PartialRatio("", ""));
            Assert.Equal(100, RapidFuzz.TokenSortRatio("", ""));
            Assert.Equal(0, RapidFuzz.TokenSetRatio("", ""));
            Assert.Equal(100, RapidFuzz.PartialTokenSortRatio("", ""));
            Assert.Equal(0, RapidFuzz.PartialTokenSetRatio("", ""));
            Assert.Equal(100, RapidFuzz.TokenRatio("", ""));
            Assert.Equal(100, RapidFuzz.PartialTokenRatio("", ""));
            Assert.Equal(0, RapidFuzz.WRatio("", ""));
            Assert.Equal(0, RapidFuzz.QRatio("", ""));
        }

        [Fact]
        public void FirstStringEmpty()
        {
            foreach (var metric in Metrics)
            {
                Assert.Equal(0, metric.Scorer("test", "", 0));

                if (metric.Symmetric)
                {
                    Assert.Equal(0, metric.Scorer("", "test", 0));
                }
            }
        }

        [Fact]
        public void SecondStringEmpty()
        {
            foreach (var metric in Metrics)
            {
                Assert.Equal(0, metric.Scorer("", "test", 0));

                if (metric.Symmetric)
                {
                    Assert.Equal(0, metric.Scorer("test", "", 0));
                }
            }
        }

        [Fact]
        public void PartialRatioShortNeedle()
        {
            Assert.Equal(33.3333333, RapidFuzz.PartialRatio("001", "220222"), 1e-7);
            Assert.Equal(100, RapidFuzz.PartialRatio("physics 2 vid", "study physics physics 2 video"));
        }

        [Fact]
        public void Issue206() /* test for https://github.com/maxbachmann/RapidFuzz/issues/206 */
        {
            var str1 = "South Korea";
            var str2 = "North Korea";

            foreach (var metric in Metrics)
            {
                double score = metric.Scorer(str1, str2, 0);
                Assert.Equal(0, metric.Scorer(str1, str2, score + 0.0001));
                Assert.Equal(score, metric.Scorer(str1, str2, score - 0.0001));
            }
        }

        [Fact]
        public void Issue210() /* test for https://github.com/maxbachmann/RapidFuzz/issues/210 */
        {
            var str1 = "bc";
            var str2 = "bca";

            foreach (var metric in Metrics)
            {
                double score = metric.Scorer(str1, str2, 0);
                Assert.Equal(0, metric.Scorer(str1, str2, score + 0.0001));
                Assert.Equal(score, metric.Scorer(str1, str2, score - 0.0001));
            }
        }

        [Fact]
        public void Issue257() /* test for https://github.com/maxbachmann/RapidFuzz/issues/257 */
        {
            var str1 = "aaaaaaaaaaaaaaaaaaaaaaaabacaaaaaaaabaaabaaaaaaaababbbbbbbbbbabbcb";
            var str2 = "aaaaaaaaaaaaaaaaaaaaaaaababaaaaaaaabaaabaaaaaaaababbbbbbbbbbabbcb";

            Assert.Equal(98.4615385, RapidFuzz.PartialRatio(str1, str2), 1e-7);
            Assert.Equal(98.4615385, RapidFuzz.PartialRatio(str2, str1), 1e-7);
        }

        [Fact]
        public void Issue219() /* test for https://github.com/maxbachmann/RapidFuzz/issues/219 */
        {
            var str1 =
                "TTAGCGCTACCGGTCGCCACCATGGTTTTCTAAGGGGAGGCCGTCATCAAAAGAGTTCATGTAGCACGAAGTCCACCTTTGAAGGATCGATGAATG" +
                "GCCATGAATTCGAAATCGAGGGGAGGGCGAGAGAGGGCCGGCCTTACGAGGGCACACCCAAACTGCCAAACTGAAAGTGACCAAAGGCGGCCCGTT" +
                "ACCATTCTCCTGGGACATACTGTAAGTGCATGGCACCACGCTCTATTTCTTAAAAAAAGTGTAGGGTCTGGCGCCCTCGGGGGCGGCTTAGGAAAA" +
                "GAGGCCTGACCAATTTTTGTCTCTTATAGGTCACCACAGTTCATGTACGGAAGCAGAGCGTTCACGAAGCACCCAGCTGACATCCCGGACTACTAT" +
                "GACAGAGCTTCCCGGAAGGACTCAAGTGGGAGCGGGTCATGAACTTCGAGGACGGTGGGGCAGTGACTGTGACACAGGACACCAGCCTGAAGATGG" +
                "AACTCTTATCTACAAAGTAAAGCTAAGAGGAACCAACTTCCCGCCAGATGGGCCCGTTATGCAAAAGAAAACGATGGGGTGGGAAGCTTCTGCAGA" +
                "GCGCCTTTACCCCGAGGATGGCGTCCTTAAGGGGGATATCAAAATGGCGCTACGCCTTAAGGATGGAGGCAGATATTTGGCAGACTTCAAAACAAC" +
                "ATTACAAGGCGAAGAAGCCAGTCCAGATGCCTGGAGCTTGCAATGGTAAGCACCTCTGCCTGCCCCGCTAGTTGGGTGTGAGTGGCCCAGGCAGCC" +
                "GCCTGCATTTAGCTCTAGCCGGGGTACGGGTGCCCCTTGATGCCTGAGGCCTCTCCTGTGGCTGAGGCGACTGGCCCAGAGTCTGGGTCTCCTCGA" +
                "GGGTGGCCATCTGGCGTCACCTGTCATCTGCCACCTCTGACCCCTGCCTCTCTCCTCACAGTTGACCGGAAGCTCGACATAACGAGTCACAACGAG" +
                "GACTACACAGTTGTCGAGCAGTACGAACGTTCCGAGGGTCGACACTCAACTGGCAGGATGGATGAGCTTTTACAAAGGGCGGGGGCGGAGGAAGCG" +
                "GAGGAGGAGGAAGTGGTGGAGGAGGCTCGAAAGGTAAGTATCAGGGTTGCAGCGTTTCTCTGACCTCATATTCCAATGGATGTGTGAGAAGCATAG" +
                "TGAGATCCGTTTACCCCTTTTGCTCAATTCTCACGTGGCTGTAGTCGTGTTTATAAGTCTGATCGTAATGGCAGCTTGGTCTGCGTGCCTTGAAAT" +
                "TGTGGCCCCCACATGCATAATAAACGATCCTCTAGCACTACTTTCTGTCGAGCCACCTCAGCGCCCGTACAGTAATGTCTACAGCGCGTCTAACCC" +
                "GACAAATGCGTTTCTTTCTCTCCTAGAACGAAAGATTACGGATCACAGAAACGTCTCGGAAAGTCCAAATAGAAAGAACGAGAAAGAAGAAAGTGA" +
                "AGGATCACAAGAGCAACTCGAAAGAAAGAGACATAAGAAGGAACTCAGAAAAGGATGACAAGTATAAAAACAAAGTGAAGAAAAGAGCGAAGAGCA" +
                "GAGTAGAAGCAAGAGTAAAGAGAAGAAGAGCAAATCGAAGGAAAGGTAAGTGGCTTTCAAGAACATTGGTAAAACGTCATGTGTATTGCGGTTCCA" +
                "TGCTTACACAAATTCGTTCGCTTGTTTTCAGGGACTCGAAACACAACAGAAACGAAGAGAAGAGAATGAGAAGCAGAAGCAAAGGAAGAGACCATG" +
                "AAAATGTCAAGGAAAAAGAAAAAACAGTCCGATAGCAAAGGCAAAGACCAGGAGCGGTCTCGGTCGAAGGAAAAATCTAAACAACTTGAATCAAAA" +
                "TCTAACGAGCATGGTAAGTTCGCGAGACACTAAGTTGATTCTTAGTGTTTAGACGTGAAACTCCCTTGGAAGGTTTAACGAATACTGTTAATATTT" +
                "TCAGATCACTCAAAATCCAAAAGAACCGACGGGCACAATCCCGGAGCCGTGAATGTGATATAACCAAGGAAGCACAGTTGCAATTCGAGAACAAGA" +
                "GAAAGAAGCAGAAGTAGAGAGATCGCTCGAGAAGAGTGAGAAGCAGAACACATGATAGAGACAGAAGCCGGTCGAAAGAATACCACCGCTACAGAG" +
                "AACAAGGTAAGCATGACTACTTGAGTGTAAATACGTTGTGATAGAGATGAAAAACAAAACCGAACATTACTTTGGGTAATAATTAACTTTTTTTTA" +
                "ATAGAATATCGGGAGAAAGGAAGGTCGAGAAGCAGAGAAAGAAGGACGCCTCAGGAAGAAGCCGTTCGAAAGACAGAAGGAGAAGGAGAAGAGATT" +
                "CGAAAGTTCAGAGCGTGAAGAGTCTCAATCGCGTAATAAAGACAAGTACGGGAACCAAGAAAGTAAAAGTTCCCACAGGAAGAACTCTGAAGAGCG" +
                "AGAAAAGTAAAAAAGGGTTTCCTGTTTTTTGCCTATTTTGGGTAAAGGGGTTGATGGAGAAACAGGTGTGTGGACTGCTGAGGAGTGAGTTAGAAT" +
                "AAATGGTGGTATCACTTCTTCAATGCTACTACAATGGAACAACAGTCGTTACCTGTTTTAAGTTCGTGGCGTCTTATGCTCCGGACAGGGACAGAT" +
                "AGGCGGTTGACAGAGAGTTAAGATCTAGTACACTGGGTTTCCTAAATGTAAGAATTGGCCCGAATCCGGCCTAATATGCGAACTTTGTGCTACCAA" +
                "GCGAGCGGGAAGCTAAGGGTGGGGAATTGCGGGTTTAATGGACCATCTCATGAGTCTAGCAGTTAATGTATCCTATCTTCCAAACAGGAATGTATT" +
                "CGAAAGAGTAGAGACCATAATTCGTCTAACAACTCAAGGAAAAGAAGGCGGAGTAGAGCCGATTCCGAACCCTTTGCTAGGACTAGATAGCACGTG" +
                "AACCTAGACTGTCTCTGAGACTGCGCCATTACGTCTCGATCAGTAACGATTGCATCGCGAGGCTGTGGATGTAAAACCTCTGCTGACCTTGACTGA" +
                "CTGAGATACAATGCCTTCAGCAATGCGTGGCAG";

            var str2 =
                "GTAAGGGTTTCCTGTTTTTTGCCTATTTTGGGTAAAGGGGGGTTGATGGAGAAACAGGTGTGTGGACTGCTGAGGAGTGAGTTAGAATAAATGGTG" +
                "GTATCACTTCTTCAATGCTACAATGGAACAACAGTCGTTACCTGTTTTAAGTTCGTGGCGTCTTATGCTCCGGACAGGGACAGATAGGCGGTTAGA" +
                "CAGAGAGTTAAGATCTAGTACACTGGGTTTCCTAAATGTAAAAATTGGCCCGAATCCGGCCTAATATGCGAACTTTGTGCTACCAAGCGAGCGGGA" +
                "AGCTAAGGGTGGGGAGTGCGGGTTTAATGGACCATCTCGCAGGTCTAGCAGTTAATGTATCCTATCTTCCAAACAG";

            Assert.Equal(97.5274725, RapidFuzz.PartialRatio(str1, str2), 1e-7);
            Assert.Equal(97.5274725, RapidFuzz.PartialRatio(str2, str1), 1e-7);
            Assert.Equal(97.5274725, RapidFuzz.PartialRatio(str1, str2, 97.5), 1e-7);
            Assert.Equal(97.5274725, RapidFuzz.PartialRatio(str2, str1, 97.5), 1e-7);
        }
    }
}
