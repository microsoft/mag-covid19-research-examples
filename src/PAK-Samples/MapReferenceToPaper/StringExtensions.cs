//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All rights reserved.
//   Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MapReferenceToPaper
{
    /// <summary>
    /// String extension methods used by sample project
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Find source substring that is most similar to target string based on Jaccard distance
        /// </summary>
        /// <param name="source">Source string to find substring in</param>
        /// <param name="target">Target string to use for finding best substring</param>
        /// <returns>Tuple with the best substring and its inverse Jaccard distance from target string</returns>
        public static Tuple<double, string> FindBestSubstring(this string source, string target)
        {
            // If either input is empty return null
            if (string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
            {
                return null;
            }

            // If the source contains an exact match, skip fuzzy matching as we already have what we need
            if (Regex.IsMatch(source, $"\\b{Regex.Escape(target)}\\b"))
            {
                return new Tuple<double, string>(1, target);
            }

            // Break target into words
            var targetWords = target.Split(' ');

            // Generate source n-grams of target word count +/- 1
            var sourceNgrams = source.CreateStringWordNgrams(Math.Max(targetWords.Length - 1, 1), targetWords.Length + 1);

            // Iterate through each n-gram to find the best match based on Jaccard distance w/greater n-gram length as a tie-breaker
            double bestDistance = 0;
            var nearestSubstring = string.Empty;
            foreach (var ngram in sourceNgrams)
            {
                var cos = new Cosine();

                var distance = cos.Similarity(target, ngram);
                if (distance > bestDistance || (distance == bestDistance && ngram.Length > nearestSubstring.Length))
                {
                    nearestSubstring = ngram;
                    bestDistance = distance;
                }
            }

            return new Tuple<double, string>(bestDistance, nearestSubstring);
        }

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