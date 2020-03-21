//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All rights reserved.
//   Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace MapReferenceToPaper
{


    class Program
    {
        // Avoid creating new HttpClient for each request
        static HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
            if (args == null || args.Length < 6)
            {
                Console.WriteLine("MapReferenceToPaper");
                Console.WriteLine("Attempts to map academic reference strings composed from rows/columns in a TSV file to a Microsoft Academic Graph (MAG) paper entity using Project Academic Knowledge Interpret method");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("MapReferenceToPaper.exe <input.tsv> <output.tsv> <subscription_key> <attributes_to_map> <additional_attributes_to_return> <columns_from_input>");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("MapReferenceToPaper.exe input.tsv output.tsv 123 \"Ti,AA.AuN,C.CN,J.JN,Y,DOI,V,I,FP,LP\" \"Id,DN,AA.DAuN,VFN,Y,DOI,V,I,FP,LP,FamId,S\" \"0,3,4,5\" ");

                return;
            }
            else
            {
                var subscriptionKey = args[2];
                var attributesToMap = args[3];
                var attributesToReturn = args[4];

                using (var inputFile = new StreamReader(args[0]))
                using (var outputFile = new StreamWriter(args[1]))
                {
                    string input = "";
                    while ((input = inputFile.ReadLine()) != null)
                    {
                        try
                        {
                            var split = input.Split('\t');

                            // Generate reference string that will be mapped using the data in the specified columns
                            var referenceString = string.Empty;
                            foreach (var column in args[5].Split(','))
                            {
                                referenceString += (referenceString == "" ? "" : " ") + split[Convert.ToInt32(column)];
                            }

                            // Map the reference
                            var mappedReference = MapReferenceToPaper(referenceString, subscriptionKey, attributesToMap, attributesToReturn);

                            // Generate paper-specific column data 
                            var paperPubMed = "";
                            var paperDoi = "";
                            var paperFamilyId = "";

                            if (mappedReference.MappedPaper["S"] != null)
                            {
                                if (mappedReference.MappedPaper["S"].Any(a => a.Value<string>("U").StartsWith("https://www.ncbi.nlm.nih.gov/pubmed/")))
                                {
                                    paperPubMed = mappedReference.MappedPaper["S"].First(a => a.Value<string>("U").StartsWith("https://www.ncbi.nlm.nih.gov/pubmed/")).Value<string>("U").Replace("https://www.ncbi.nlm.nih.gov/pubmed/", "");
                                }
                            }

                            if (mappedReference.MappedPaper["DOI"] != null)
                            {
                                paperDoi = mappedReference.MappedPaper.Value<string>("DOI");
                            }

                            if (mappedReference.MappedPaper["FamId"] != null)
                            {
                                paperDoi = mappedReference.MappedPaper.Value<long>("FamId").ToString();
                            }

                            // SPECIAL CASE: If there is an exact match for DOI, override confidence score to a 1, as DOI is generally 1:1
                            if(mappedReference.MappedReference.Contains("<attr confidence=\"1\" name=\"academic#DOI\">"))
                            {
                                mappedReference.PercentOfReferenceMapped = 1.0;
                            }

                            // Output mapping data + original row columns
                            var output = $"{mappedReference.PercentOfReferenceMapped}\t{mappedReference.MappedReference}\t{mappedReference.MappedPaper.Value<long>("Id")}\t{paperFamilyId}\t{paperDoi}\t{paperPubMed}\t{input}";

                            Console.WriteLine(output);
                            Console.WriteLine();

                            outputFile.WriteLine(output);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("EXCEPTION WHEN PROCESSING:");
                            Console.WriteLine(input);
                            Console.WriteLine();
                            Console.WriteLine(ex.ToString());
                            Console.WriteLine();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Maps an academic reference string to a Microsoft Academic paper entity using Project Academic Knowledge Interpret method
        /// </summary>
        static ReferenceMapping MapReferenceToPaper(string referenceString, string subscriptionKey, string attributesToMap, string attributesToReturn)
        {
            var resultString = GetQueryInterpretations(
                query: referenceString,
                subscriptionKey: subscriptionKey,
                complete: false,
                interpretationCount: 1,
                entityCount: 1,
                attributes: attributesToMap + "," + attributesToReturn,

                // NOTE: Longer timeouts can sometimes provide better quality interpretations, though generally 500ms is sufficient for most reference strings
                timeout: 500);

            var result = JObject.Parse(resultString);

            if (result["interpretations"] != null && result["interpretations"].Count() > 0)
            {
                var toMap = new HashSet<string>(attributesToMap.Split(','));

                // We only care about the first interpretation
                var firstInterpretation = result["interpretations"].First();

                // Extract relevance fields from JSON response
                var interpretedQuery = result.Value<string>("query");
                var newParse = interpretedQuery;
                var entity = firstInterpretation["rules"].First()["output"]["entities"].First();

                // Interpretation of the reference string is not guaranteed to map all parts of said string, as it will stop 
                // once it has "enough" to generate a matching entity and leave the rest of the query unprocessed.
                // 
                // Filler, stop words, misspelled words or unknown synonyms are all possibilities for the remaining terms not matched
                //
                // To generate a "confidence" score we do a "re-mapping" of the top entity's values based on Jaccard distance

                // First remove the enclosing "rule" tags from XML parse
                var modifiedParse = newParse;
                var remainingUnmatched = string.Empty;
                var unmatchedChunk = interpretedQuery;

                // Break up parse into chunks based on parts of string that were matched
                var reducedChunk = unmatchedChunk.Trim();
                var expandedChunk = reducedChunk;

                // Generate mappings for all of the entity's attributes
                var entityAttributeMapping = new List<Tuple<double, string, string, string>>();
                foreach (var attribute in entity)
                {
                    var prop = (JProperty)attribute;

                    if (prop.Value is JArray)
                    {
                        // Map composite attribute arrays
                        foreach (var innerObject in (JArray)prop.Value)
                        {
                            foreach (var innerAttribute in (JObject)innerObject)
                            {
                                var name = $"{prop.Name}.{innerAttribute.Key}";

                                if (toMap.Contains(name))
                                {
                                    var value = innerAttribute.Value.ToString();

                                    var nearestSubstring = FindBestSubstring(reducedChunk, value);

                                    entityAttributeMapping.Add(new Tuple<double, string, string, string>(nearestSubstring.Item1, name, value, nearestSubstring.Item2));
                                }
                            }
                        }
                    }
                    else if (prop.Value is JObject)
                    {
                        // Map composite attributes
                        foreach (var innerAttribute in (JObject)prop.Value)
                        {
                            var name = $"{prop.Name}.{innerAttribute.Key}";

                            if (toMap.Contains(name))
                            {
                                var value = innerAttribute.Value.ToString();

                                var nearestSubstring = FindBestSubstring(reducedChunk, value);

                                entityAttributeMapping.Add(new Tuple<double, string, string, string>(nearestSubstring.Item1, name, value, nearestSubstring.Item2));
                            }
                        }
                    }
                    else
                    {
                        // Map simple value attributes
                        if (toMap.Contains(prop.Name))
                        {
                            var value = prop.Value.ToString();

                            var nearestSubstring = FindBestSubstring(reducedChunk, value);

                            entityAttributeMapping.Add(new Tuple<double, string, string, string>(nearestSubstring.Item1, prop.Name, value, nearestSubstring.Item2));
                        }
                    }
                }

                // Reduce the mapping down to only the most confident ones based on inverse Jaccard distance
                var confidentMappings = new List<Tuple<double, string, string, string>>();
                foreach (var b in entityAttributeMapping.OrderByDescending(a => a.Item1).ThenByDescending(a => a.Item4.Length))
                {
                    if (b.Item1 > 0.8 && reducedChunk.Contains(b.Item4))
                    {
                        confidentMappings.Add(b);
                        reducedChunk = reducedChunk.Replace(b.Item4, "");
                    }
                }

                // For each mapping we're confident with, generate a mapping string with embedded XML tags for each match
                foreach (var b in confidentMappings.OrderByDescending(a => a.Item1).ThenByDescending(a => a.Item4.Length))
                {
                    if (b.Item3 == b.Item4)
                    {
                        expandedChunk = Regex.Replace(expandedChunk, $"({Regex.Escape(b.Item4)})(?![^<]*>|[^<>]*</)", $" <attr confidence=\"{b.Item1}\" name=\"academic#{b.Item2}\">{b.Item4}</attr> ");
                    }
                    else
                    {
                        expandedChunk = Regex.Replace(expandedChunk, $"({Regex.Escape(b.Item4)})(?![^<]*>|[^<>]*</)", $" <attr confidence=\"{b.Item1}\" name=\"academic#{b.Item2}\" canonical=\"{b.Item3}\">{b.Item4}</attr> ");
                    }
                }

                remainingUnmatched += (remainingUnmatched == "" ? "" : " ") + reducedChunk;
                newParse = Regex.Replace(newParse, $"({Regex.Escape(unmatchedChunk)})(?![^<]*>|[^<>]*</)", expandedChunk);

                // Collapse whitespace down to single space
                remainingUnmatched = Regex.Replace(remainingUnmatched, "\\s+", " ").Trim();
                newParse = Regex.Replace(newParse, "\\s+", " ").Trim();

                var response = new ReferenceMapping
                {
                    PercentOfReferenceMapped = 1.0 - ((double)remainingUnmatched.Length / (double)interpretedQuery.Length),
                    NormalizedReference = interpretedQuery,
                    MappedPaper = entity,
                    MappedReference = newParse,
                    OriginalReference = referenceString,
                    ReferenceNotMapped = remainingUnmatched
                };

                return response;
            }

            return null;
        }

        /// <summary>
        /// Generates interpretation of query using Project Academic Knowledge Interpret method
        /// </summary>
        /// <param name="query">Natural language query to interpret</param>
        /// <returns>JSON string of the Interpret method response. See https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-interpret-method</returns>
        static string GetQueryInterpretations(
            string query,
            string subscriptionKey,
            bool complete = false,
            int interpretationCount = 5,
            int entityCount = 0,
            string attributes = "",
            int timeout = 500
            )
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var encodedQuery = HttpUtility.UrlEncode(query);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters
            queryString["entityCount"] = entityCount.ToString();
            queryString["attributes"] = attributes;
            queryString["complete"] = complete ? "1" : "0";
            queryString["count"] = interpretationCount.ToString();
            queryString["offset"] = "0";
            queryString["timeout"] = timeout.ToString();
            queryString["model"] = "latest";

            var uri = $"https://api.labs.cognitive.microsoft.com/academic/v1.0/interpret?query={encodedQuery}&" + queryString;

            var response = client.GetAsync(uri).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Find source substring that is most similar to target string based on Jaccard distance
        /// </summary>
        /// <param name="source">Source string to find substring in</param>
        /// <param name="target">Target string to use for finding best substring</param>
        /// <returns>Tuple with the best substring and its inverse Jaccard distance from target string</returns>
        static Tuple<double, string> FindBestSubstring(string source, string target)
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
                var distance = 1.0 - target.JaccardDistance(ngram);
                if (distance > bestDistance || (distance == bestDistance && ngram.Length > nearestSubstring.Length))
                {
                    nearestSubstring = ngram;
                    bestDistance = distance;
                }
            }

            return new Tuple<double, string>(bestDistance, nearestSubstring);
        }
    }
}