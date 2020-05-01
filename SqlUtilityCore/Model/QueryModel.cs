using System.Collections.Generic;

namespace SqlUtilityCore.Model
{
    /// <summary>
    /// Table/query model.
    /// </summary>
    public class QueryDataModel
    {
        /// <summary>
        /// Table name.
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// List of column data.
        /// </summary>
        public List<ColumnModel> ColumnList = new List<ColumnModel>();

        /// <summary>
        /// List of table clause.
        /// </summary>
        public List<TableClauseModel> TableClause = new List<TableClauseModel>();
    }

    /// <summary>
    /// Table clause model.
    /// </summary>
    public class TableClauseModel
    {
        /// <summary>
        /// Type of the clause to be used.
        /// </summary>
        public string ClauseType { get; set; } = string.Empty;

        /// <summary>
        /// Value for the clause.
        /// </summary>
        public string ClauseValue { get; set; } = string.Empty;

        /// <summary>
        /// Type of join to be applied if clause type is 'JOIN'.
        /// </summary>
        public string JoinType { get; set; } = string.Empty;

        /// <summary>
        /// Table name for join to be applied if clause type is 'JOIN'.
        /// </summary>
        public string JoinTableName { get; set; } = string.Empty;
    }

    /// <summary>
    /// Column model class.
    /// </summary>
    public class ColumnModel
    {
        /// <summary>
        /// Column name as in database.
        /// </summary>
        public string ColumnName { get; set; } = string.Empty;

        /// <summary>
        /// Alias for column.
        /// </summary>
        public string Alias { get; set; } = string.Empty;

        /// <summary>
        /// Operation to be performed on the column.
        /// </summary>
        public string ColumnOperation { get; set; } = string.Empty;

        /// <summary>
        /// Data type of the column.
        /// </summary>
        public string DataType { get; set; } = string.Empty;

        /// <summary>
        /// Max length for the column.
        /// </summary>
        public string MaxLength { get; set; } = string.Empty;

        /// <summary>
        /// Set true to allow null.
        /// </summary>
        public bool AllowNull { get; set; } = true;

        /// <summary>
        /// Set true if column is primary column.
        /// </summary>
        public bool IsPrimary { get; set; } = false;

        /// <summary>
        /// Set true if column is unique column.
        /// </summary>
        public bool IsUnique { get; set; } = false;

        /// <summary>
        /// Default value for the column.
        /// </summary>
        public string DefaultValue { get; set; } = string.Empty;

        /// <summary>
        /// Values to be inserted into database.
        /// </summary>
        public List<string> Values { get; set; } = new List<string>();

        /// <summary>
        /// Table name where this column belongs to. Used while creating select query with JOIN.
        /// </summary>
        public string BelongsToTable { get; set; } = string.Empty;
    }

    /// <summary>
    /// Query clause model.
    /// </summary>
    public class QueryClauseModel
    {

    }

    /// <summary>
    /// Query model.
    /// </summary>
    public class QueryModel
    {
        /// <summary>
        /// Type of the query.
        /// </summary>
        public string QueryType { get; set; } = string.Empty;

        /// <summary>
        /// Query name.
        /// </summary>
        public string QueryName { get; set; } = string.Empty;

        /// <summary>
        /// List of table/query data to build query string.
        /// </summary>
        public List<QueryDataModel> QueryData = new List<QueryDataModel>();

        /// <summary>
        /// List of query clause.
        /// </summary>
        public List<QueryClauseModel> QueryClause = new List<QueryClauseModel>();
    }
}
