//-----------------------------------------------------------------------
//   Copyright (c) Microsoft Corporation. All rights reserved.
//   Licensed under the MIT License.
//-----------------------------------------------------------------------

using Newtonsoft.Json.Linq;

namespace MapReferenceToPaper
{
    /// <summary>
    /// Contains information about a reference => paper mapping
    /// </summary>
    public class ReferenceMapping
    {
        /// <summary>
        /// The original reference string provided as input
        /// </summary>
        public string OriginalReference { get; set; }

        /// <summary>
        /// The absolute % of the normalized reference string that was able to be mapped
        /// </summary>
        public double PercentOfReferenceMapped { get; set; }

        /// <summary>
        /// The normalized reference string that was used by Interpert
        /// </summary>
        public string NormalizedReference { get; set; }

        /// <summary>
        /// Terms from the normalized reference that could not be mapped to attributes in the top matching paper
        /// </summary>
        public string ReferenceNotMapped { get; set; }

        /// <summary>
        /// Version of the normalized reference string with embedded XML tags indicating how different terms were mapped
        /// </summary>
        public string MappedReference { get; set; }

        /// <summary>
        /// JSON object representing the mapped paper entity
        /// </summary>
        public JToken MappedPaper { get; set; }
    }
}
