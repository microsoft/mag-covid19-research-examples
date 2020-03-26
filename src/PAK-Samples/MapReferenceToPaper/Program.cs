//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All rights reserved.
//   Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MapReferenceToPaper
{


    public class Program
    {
        public static Config configuration;

        static void Main(string[] args)
        {
            if (args == null || args.Length < 3)
            {
                Console.WriteLine("MapReferenceToPaper");
                Console.WriteLine("Attempts to map academic reference strings composed from rows/columns in a TSV file to a Microsoft Academic Graph (MAG) paper entity using Project Academic Knowledge Interpret method");
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("MapReferenceToPaper.exe <input.tsv> <output.tsv> <configuration.json>");
                Console.WriteLine();
                Console.WriteLine("Example:");
                Console.WriteLine("MapReferenceToPaper.exe sampleData/2020-03-20-WorldHealthOrganization-COVID-19-Full-Database.txt mapped.txt who-covid-19-pak-config.json");

                return;
            }
            else
            {
                // Load configuration
                try
                {
                    var configContent = File.ReadAllText(args[2]);
                    configuration = JsonConvert.DeserializeObject<Config>(configContent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: Unable to deserialize configuration file");
                    return;
                }

                var attributesToMap = GetInputEntityAttributesFromConfig();
                var attributesToReturn = GetOutputEntityAttributesFromConfig();

                var resultQueue = new ConcurrentQueue<string>();
                var activeThreadCount = 0;
                var processedCount = 0;

                using (var inputFile = new StreamReader(args[0]))
                using (var outputFile = new StreamWriter(args[1]))
                {
                    string input = "";
                    while ((input = inputFile.ReadLine()) != null)
                    {
                        if (activeThreadCount < configuration.ApiRequestConcurrency)
                        {
                            Interlocked.Increment(ref activeThreadCount);
                            var inputCopy = input;

                            Task.Run(() =>
                            {
                                try
                                {
                                    ReferenceMapping mappedReference = null;
                                    var split = inputCopy.Split('\t');

                                    // If DOI is present in the input, first try to match it directly against papers using Evaluate API as it's fast and highly accurate
                                    if (configuration.InputColumnMapping.ContainsKey("doi") && !string.IsNullOrEmpty(split[configuration.InputColumnMapping["doi"]]))
                                    {
                                        var doiColumn = split[configuration.InputColumnMapping["doi"]];

                                        var evaluateResult =
                                            EvaluateQueryExpression(
                                                expression: $"DOI='{doiColumn}'",
                                                count: 1,
                                                offset: 0,
                                                attributes: attributesToMap + "," + attributesToReturn
                                                );

                                        var evaluateResultObject = JObject.Parse(evaluateResult);

                                        if (evaluateResultObject["entities"] != null && evaluateResultObject["entities"].Any())
                                        {
                                            mappedReference =
                                                MapReferenceToPaperEntity(
                                                    doiColumn,
                                                    doiColumn,
                                                    doiColumn,
                                                    new HashSet<string>(attributesToMap.Split(',')),
                                                    attributesToReturn,
                                                    evaluateResultObject["entities"].First());
                                        }
                                    }

                                    // If DOI map failed, do full reference matching
                                    if (mappedReference == null)
                                    {
                                        // Generate reference string that will be mapped using the data in the specified columns (sans last column)
                                        var referenceString = string.Empty;
                                        foreach (var column in configuration.InputColumnMapping)
                                        {
                                            if (column.Key != "doi")
                                            {
                                                referenceString += (referenceString == "" ? "" : " ") + split[column.Value];
                                            }
                                        }

                                        // Map the reference
                                        mappedReference = MapReferenceToPaper(referenceString, attributesToMap, attributesToReturn);
                                    }

                                    resultQueue.Enqueue($"{inputCopy}\t{GetOutputColumns(mappedReference)}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("EXCEPTION WHEN PROCESSING:");
                                    Console.WriteLine(inputCopy);
                                    Console.WriteLine();
                                    Console.WriteLine(ex.ToString());
                                    Console.WriteLine();
                                }
                                finally
                                {
                                    Interlocked.Decrement(ref activeThreadCount);
                                }
                            });
                        }

                        while (activeThreadCount >= configuration.ApiRequestConcurrency)
                        {
                            // Clear out queue
                            while (resultQueue.TryDequeue(out var output))
                            {
                                Console.WriteLine($"{++processedCount}: {output}");
                                Console.WriteLine();

                                outputFile.WriteLine(output);
                            }

                            Thread.Sleep(50);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates MAKES/PAK entity attributes that need to be requested for input mapping
        /// </summary>
        public static string GetInputEntityAttributesFromConfig()
        {
            var attributes = new List<string>();

            foreach (var input in configuration.InputColumnMapping)
            {
                switch (input.Key)
                {
                    case "title":
                        attributes.Add("Ti");
                        break;

                    case "authors":
                        attributes.Add("AA.AuN");
                        break;

                    case "year":
                        attributes.Add("Y");
                        break;

                    case "venue":
                        attributes.Add("J.JN");
                        attributes.Add("C.CN");
                        break;

                    case "volume":
                        attributes.Add("V");
                        break;

                    case "issue":
                        attributes.Add("I");
                        break;

                    case "firstPage":
                        attributes.Add("FP");
                        break;

                    case "lastPage":
                        attributes.Add("LP");
                        break;

                    case "doi":
                        attributes.Add("DOI");
                        break;
                }
            }

            return string.Join(",", attributes);
        }

        /// <summary>
        /// Generates MAKES/PAK entity attributes that need to be requested for output mapping
        /// </summary>
        public static string GetOutputEntityAttributesFromConfig()
        {
            var attributes = new List<string>();

            foreach (var output in configuration.OutputColumns)
            {
                switch (output)
                {
                    case "id":
                        attributes.Add("Id");
                        break;

                    case "familyId":
                        attributes.Add("FamId");
                        break;

                    case "pubmedId":
                        attributes.Add("S");
                        break;

                    case "title":
                        attributes.Add("DN");
                        break;

                    case "authors":
                        attributes.Add("AA.DAuN");
                        break;

                    case "year":
                        attributes.Add("Y");
                        break;

                    case "venue":
                        attributes.Add("VFN");
                        break;

                    case "volume":
                        attributes.Add("V");
                        break;

                    case "issue":
                        attributes.Add("I");
                        break;

                    case "firstPage":
                        attributes.Add("FP");
                        break;

                    case "lastPage":
                        attributes.Add("LP");
                        break;

                    case "doi":
                        attributes.Add("DOI");
                        break;
                }
            }

            return string.Join(",", attributes);
        }

        /// <summary>
        /// Returns JToken attribute if it exists, otherwise an empty string
        /// </summary>
        public static string GetAttributeIfExists(JToken paper, string attribute)
        {
            if (paper[attribute] != null)
            {
                return ((JValue)paper[attribute]).ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the output columns for the current mapping based on output column configuration
        /// </summary>
        public static string GetOutputColumns(ReferenceMapping mappedReference)
        {
            var outputColumns = new List<string>();

            // SPECIAL CASE: If there is an exact match for DOI, override confidence score to a 1, as DOI is generally 1:1
            if (mappedReference.MappedReference.Contains("<attr confidence=\"1\" name=\"academic#DOI\">"))
            {
                mappedReference.PercentOfReferenceMapped = 1.0;
            }

            foreach (var attribute in configuration.OutputColumns)
            {
                switch (attribute)
                {
                    case "score":
                        outputColumns.Add(mappedReference.PercentOfReferenceMapped.ToString());
                        break;

                    case "mapping":
                        outputColumns.Add(mappedReference.MappedReference);
                        break;

                    case "id":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "Id"));
                        break;

                    case "familyId":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "FamId"));
                        break;

                    case "pubmedId":
                        if (mappedReference.MappedPaper["S"] != null)
                        {
                            if (mappedReference.MappedPaper["S"].Any(a => a.Value<string>("U").StartsWith("https://www.ncbi.nlm.nih.gov/pubmed/")))
                            {
                                outputColumns.Add(mappedReference.MappedPaper["S"].First(a => a.Value<string>("U").StartsWith("https://www.ncbi.nlm.nih.gov/pubmed/")).Value<string>("U").Replace("https://www.ncbi.nlm.nih.gov/pubmed/", ""));
                            }
                        }
                        break;

                    case "title":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "DN"));
                        break;

                    case "authors":
                        var authors = new List<string>();

                        if (mappedReference.MappedPaper["AA"] != null)
                        {
                            foreach (var author in mappedReference.MappedPaper["AA"])
                            {
                                authors.Add(GetAttributeIfExists(mappedReference.MappedPaper, "DAuN"));
                            }
                        }

                        outputColumns.Add(string.Join(", ", authors));
                        break;

                    case "year":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "Y"));
                        break;

                    case "venue":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "VFN"));
                        break;

                    case "volume":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "V"));
                        break;

                    case "issue":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "I"));
                        break;

                    case "firstPage":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "FP"));
                        break;

                    case "lastPage":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "LP"));
                        break;

                    case "doi":
                        outputColumns.Add(GetAttributeIfExists(mappedReference.MappedPaper, "DOI"));
                        break;
                }
            }

            // Output mapping data + original row columns
            return string.Join("\t", outputColumns);
        }

        /// <summary>
        /// Maps an academic reference string to a Microsoft Academic paper entity using Project Academic Knowledge Interpret method
        /// </summary>
        public static ReferenceMapping MapReferenceToPaper(string referenceString, string attributesToMap, string attributesToReturn)
        {
            var resultString = GetQueryInterpretations(
                query: referenceString,
                complete: false,
                interpretationCount: 1,
                entityCount: 5,
                attributes: attributesToMap + "," + attributesToReturn);

            var result = JObject.Parse(resultString);

            if (result["interpretations"] != null && result["interpretations"].Count() > 0)
            {
                var toMap = new HashSet<string>(attributesToMap.Split(','));

                // We only care about the first interpretation
                var firstInterpretation = result["interpretations"].First();

                // Extract relevance fields from JSON response
                var interpretedQuery = result.Value<string>("query");
                var newParse = interpretedQuery;
                var candidates = new List<ReferenceMapping>();

                foreach (var entity in firstInterpretation["rules"].First()["output"]["entities"])
                {
                    candidates.Add(MapReferenceToPaperEntity(referenceString, interpretedQuery, newParse, toMap, attributesToReturn, entity));
                }

                // Return only the most promising candidate
                return candidates.OrderByDescending(candidate => candidate.PercentOfReferenceMapped).First();
            }

            return null;
        }

        /// <summary>
        /// Maps an academic reference string to a Microsoft Academic paper entity using Project Academic Knowledge Interpret method
        /// </summary>
        public static ReferenceMapping MapReferenceToPaperEntity(string referenceString, string interpretedQuery, string newParse, HashSet<string> toMap, string attributesToReturn, JToken entity)
        {
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

                                var nearestSubstring = reducedChunk.FindBestSubstring(value);

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

                            var nearestSubstring = reducedChunk.FindBestSubstring(value);

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

                        var nearestSubstring = reducedChunk.FindBestSubstring(value);

                        entityAttributeMapping.Add(new Tuple<double, string, string, string>(nearestSubstring.Item1, prop.Name, value, nearestSubstring.Item2));
                    }
                }
            }

            // Reduce the mapping down to only the most confident ones based on inverse Jaccard distance
            var confidentMappings = new List<Tuple<double, string, string, string>>();
            foreach (var b in entityAttributeMapping.OrderByDescending(a => a.Item1).ThenByDescending(a => a.Item4.Length))
            {
                if (b.Item1 > 0.5 && reducedChunk.Contains(b.Item4))
                {
                    confidentMappings.Add(b);
                    reducedChunk = reducedChunk.Replace(b.Item4, "");
                }
            }

            // For each mapping we're confident with, generate a mapping string with embedded XML tags for each match
            double lengthByConfidence = 0.0;
            foreach (var b in confidentMappings.OrderByDescending(a => a.Item1).ThenByDescending(a => a.Item4.Length))
            {
                var newExpandedChunk = string.Empty;
                if (b.Item3 == b.Item4)
                {
                    newExpandedChunk = Regex.Replace(expandedChunk, $"({Regex.Escape(b.Item4)})(?![^<]*>|[^<>]*</)", $" <attr confidence=\"{b.Item1}\" name=\"academic#{b.Item2}\">{b.Item4}</attr> ");
                }
                else
                {
                    newExpandedChunk = Regex.Replace(expandedChunk, $"({Regex.Escape(b.Item4)})(?![^<]*>|[^<>]*</)", $" <attr confidence=\"{b.Item1}\" name=\"academic#{b.Item2}\" canonical=\"{b.Item3}\">{b.Item4}</attr> ");
                }

                if (newExpandedChunk != expandedChunk)
                {
                    lengthByConfidence += b.Item1 * (double)b.Item4.Length;
                    expandedChunk = newExpandedChunk;
                }
            }

            remainingUnmatched += (remainingUnmatched == "" ? "" : " ") + reducedChunk;
            newParse = Regex.Replace(newParse, $"({Regex.Escape(unmatchedChunk)})(?![^<]*>|[^<>]*</)", expandedChunk);

            // Collapse whitespace down to single space
            remainingUnmatched = Regex.Replace(remainingUnmatched, "\\s+", " ").Trim();
            newParse = Regex.Replace(newParse, "\\s+", " ").Trim();

            return new ReferenceMapping
            {
                PercentOfReferenceMapped = lengthByConfidence / (double)interpretedQuery.Length, // 1.0 - ((double)remainingUnmatched.Length / (double)interpretedQuery.Length),
                NormalizedReference = interpretedQuery,
                MappedPaper = entity,
                MappedReference = newParse,
                OriginalReference = referenceString,
                ReferenceNotMapped = remainingUnmatched
            };
        }

        /// <summary>
        /// Generates interpretation of query using Interpet API method from either Project Academic Knowledge or Microsoft Academic Knowledge Exploration Service
        /// </summary>
        /// <returns>JSON string of the Interpret method response</returns>
        public static string GetQueryInterpretations(
            string query,
            bool complete = false,
            int interpretationCount = 5,
            int entityCount = 0,
            string attributes = ""
            )
        {
            HttpClient client = new HttpClient();

            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var encodedQuery = HttpUtility.UrlEncode(query);

            // Request parameters
            queryString["entityCount"] = entityCount.ToString();
            queryString["attributes"] = attributes;
            queryString["complete"] = complete ? "1" : "0";
            queryString["count"] = interpretationCount.ToString();
            queryString["offset"] = "0";
            queryString["timeout"] = configuration.InterpretTimeout.ToString();
            queryString["model"] = "latest";

            var uri = configuration.InterpretBaseUrl + $"query={encodedQuery}&{queryString}";

            var response = client.GetAsync(uri).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Evaluates query expression using Evaluate API method from either Project Academic Knowledge or Microsoft Academic Knowledge Exploration Service
        /// </summary>
        /// <returns>JSON string of the Evaluate method response</returns>
        public static string EvaluateQueryExpression(
            string expression,
            int count,
            int offset,
            string attributes = ""
            )
        {
            HttpClient client = new HttpClient();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request parameters
            queryString["expr"] = expression;
            queryString["attributes"] = attributes;
            queryString["count"] = count.ToString();
            queryString["offset"] = offset.ToString();
            queryString["model"] = "latest";

            var uri = configuration.EvaluateBaseUrl + queryString;

            var response = client.GetAsync(uri).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}