using System.Collections.Generic;

namespace SqlUtilityCore.Model
{
    /// <summary>
    /// Query response model.
    /// </summary>
    public class QueryResponseObject
    {
        /// <summary>
        /// Generated query string.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Column alias map to validate response.
        /// </summary>
        public Dictionary<string, string> ColumnAliasMap { get; set; }

        /// <summary>
        /// Expected column list to validate response.
        /// </summary>
        public List<string> ExpectedColumnList { get; set; }
    }
}
