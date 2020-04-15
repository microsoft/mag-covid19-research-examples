//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All rights reserved.
//   Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace MapReferenceToPaper
{
    /// <summary>
    /// Configuration options
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Interpret API method base URL
        /// </summary>
        public string InterpretBaseUrl { get; set; }

        /// <summary>
        /// Evaluate API method base URL
        /// </summary>
        public string EvaluateBaseUrl { get; set; }

        /// <summary>
        /// Maximum interpret API method call duration
        /// </summary>
        public int InterpretTimeout { get; set; } = 2000;

        /// <summary>
        /// Number of concurrent API requests to make when (i.e. number of rows to process in parallel)
        /// </summary>
        public int ApiRequestConcurrency { get; set; } = 1;

        public int CandidatesPerRow { get; set; } = 1;

        /// <summary>
        /// Defines the different types of academic data available in the input TSV file and their column index (zero-based)
        /// 
        /// Available types:
        ///     title
        ///     authors
        ///     year
        ///     venue
        ///     volume
        ///     issue
        ///     firstPage
        ///     lastPage
        ///     doi
        /// </summary>
        public Dictionary<string, int> InputColumnMapping { get; set; }

        /// <summary>
        /// The academic paper attributes to add to the output TSV
        /// 
        /// Available attributes (see https://docs.microsoft.com/en-us/academic-services/project-academic-knowledge/reference-paper-entity-attributes):
        ///     confidence
        ///     mapping
        ///     id
        ///     familyId
        ///     pubmedId
        ///     title
        ///     authors
        ///     year
        ///     venue
        ///     volume
        ///     issue
        ///     firstPage
        ///     lastPage
        ///     doi
        /// </summary>
        public List<string> OutputColumns { get; set; }
    }
}
