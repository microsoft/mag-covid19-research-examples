//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All rights reserved.
//   Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace MapReferenceToPaper
{
    /// <summary>
    /// String extension methods used by sample project
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Calculates the Jaccard distance for two strings
        /// </summary>
        /// <remarks>See https://en.wikipedia.org/wiki/Jaccard_index</remarks>
        public static double JaccardDistance(this string source, string target)
        {
            return 1 - (Convert.ToDouble(source.Intersect(target).Count())) / (Convert.ToDouble(source.Union(target).Count()));
        }

        /// <summary>
        /// Generates word ngrams of the specified sizes from a given text string
        /// </summary>
        /// <param name="source">String to create ngrams for</param>
        /// <param name="ngramMinSize">Optional minimum size of word ngram. Defaults to 1 (unigram)/</param>
        /// <param name="ngramMaxSize">Optional maximum size of word ngram. Defaults to -1, which means to generate all possible word ngram sizes based on the total number of words in text string</param>
        /// <returns>List of word ngrams of text string, sorted first by number of words and second by position of ngram in the original text string</returns>
        public static List<string> CreateStringWordNgrams(this string source, int ngramMinSize = 1, int ngramMaxSize = -1)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            var words = source.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var distinctNgrams = new HashSet<string>();
            var allNgrams = new List<string>();

            ngramMaxSize = ngramMaxSize == -1 ? words.Length : ngramMaxSize;

            for (int ngramPosition = 0, ngramSize = ngramMinSize; ngramSize <= ngramMaxSize; ngramPosition++)
            {
                if (ngramPosition + ngramSize > words.Length)
                {
                    ngramSize++;
                    ngramPosition = -1;
                }
                else
                {
                    var ngram = string.Join(" ", words.Skip(ngramPosition).Take(ngramSize));

                    if (ngram.Length > 2 && !distinctNgrams.Contains(ngram))
                    {
                        allNgrams.Add(ngram);
                        distinctNgrams.Add(ngram);
                    }
                }
            }

            return allNgrams.ToList();
        }
    }
}