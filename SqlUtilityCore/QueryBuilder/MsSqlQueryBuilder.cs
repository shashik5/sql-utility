using SqlUtilityCore.QueryBuilder.Base;

namespace SqlUtilityCore.QueryBuilder
{
    /// <summary>
    /// Class to build MS SQL query.
    /// </summary>
    public class MsSqlQueryBuilder : QueryBuilderBase
    {
        /// <summary>
        /// Constructor without arguments.
        /// </summary>
        public MsSqlQueryBuilder() { }

        /// <summary>
        /// Constructor with json string as argument.
        /// </summary>
        /// <param name="queryModelJson">JSON string to be parsed.</param>
        public MsSqlQueryBuilder(string queryModelJson) : base(queryModelJson) { }
    }
}
