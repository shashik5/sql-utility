using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlUtilityCore.Interface;
using SqlUtilityCore.Model;
using Newtonsoft.Json;
using SqlUtilityCore.Utilities;

namespace SqlUtilityCore.QueryBuilder.Base
{
    /// <summary>
    /// Base class to build query string.
    /// </summary>
    public class QueryBuilderBase : IQueryBuilder
    {
        /// <summary>
        /// Generated query string.
        /// </summary>
        protected StringBuilder Query = new StringBuilder();

        /// <summary>
        /// Query model object.
        /// </summary>
        protected QueryModel QueryModelObj;

        /// <summary>
        /// Column alias map to compare with query response.
        /// </summary>
        protected Dictionary<string, string> ColumnAliasMap = new Dictionary<string, string>();

        /// <summary>
        /// Expected list of parsed column names to validate query response.
        /// </summary>
        protected List<string> ExpectedColumnList = new List<string>();

        /// <summary>
        /// Constructor with json string as argument.
        /// </summary>
        /// <param name="queryModelJson">JSON string to be parsed.</param>
        public QueryBuilderBase(string queryModelJson)
        {
            LoadJson(queryModelJson);
        }

        /// <summary>
        /// Constructor without argument.
        /// </summary>
        public QueryBuilderBase() { }

        /// <summary>
        /// Method to load JSON string to generate query.
        /// </summary>
        /// <param name="queryModelJson">JSON string to be parsed.</param>
        public void LoadJson(string queryModelJson)
        {
            try
            {
                QueryModelObj = JsonConvert.DeserializeObject<QueryModel>(queryModelJson.Replace("'", "\'"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to append TOP clause to query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void Top(QueryDataModel table)
        {
            var clause = table.TableClause.FirstOrDefault(n => n.ClauseType.Equals("TOP", StringComparison.OrdinalIgnoreCase));
            if (clause != null)
            {
                Query.Append(string.Concat(" TOP (", clause.ClauseValue, ")"));
            }
        }

        /// <summary>
        /// Method to append DISTINCT clause to query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void Distinct(QueryDataModel table)
        {
            var clause = table.TableClause.FirstOrDefault(n => n.ClauseType.Equals("DISTINCT", StringComparison.OrdinalIgnoreCase));
            if (clause != null)
            {
                Query.Append(" DISTINCT");
            }
        }

        /// <summary>
        /// Method to append HAVING clause to query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void Having(QueryDataModel table)
        {
            var clause = table.TableClause.FirstOrDefault(n => n.ClauseType.Equals("HAVING", StringComparison.OrdinalIgnoreCase));
            if (clause != null)
            {
                Query.Append(string.Concat(" HAVING ", clause.ClauseValue));
            }
        }

        /// <summary>
        /// Method to append JOIN clause to the query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void Join(QueryDataModel table)
        {
            var joinClauseList = table.TableClause.Where(n => n.ClauseType.Equals("JOIN", StringComparison.OrdinalIgnoreCase));

            foreach (var clause in joinClauseList)
            {
                Query.Append(string.Concat(" ", clause.JoinType.ToUpper(), " JOIN ", clause.JoinTableName, " ON ", clause.ClauseValue));
            }
        }

        /// <summary>
        /// Method to append WHERE clause to query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void Where(QueryDataModel table)
        {
            var clause = table.TableClause.FirstOrDefault(n => n.ClauseType.Equals("WHERE", StringComparison.OrdinalIgnoreCase));
            if (clause != null)
            {
                Query.Append(string.Concat(" WHERE ", clause.ClauseValue));
            }
        }

        /// <summary>
        /// Method to append GROUP BY clause to query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void GroupBy(QueryDataModel table)
        {
            var clause = table.TableClause.FirstOrDefault(n => n.ClauseType.Equals("GROUPBY", StringComparison.OrdinalIgnoreCase));
            if (clause != null)
            {
                Query.Append(string.Concat(" GROUP BY ", clause.ClauseValue));
            }
        }

        /// <summary>
        /// Method to append ORDER BY clause to query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void OrderBy(QueryDataModel table)
        {
            var clause = table.TableClause.FirstOrDefault(n => n.ClauseType.Equals("ORDERBY", StringComparison.OrdinalIgnoreCase));
            if (clause != null)
            {
                Query.Append(string.Concat(" ORDER BY ", clause.ClauseValue));
            }
        }

        /// <summary>
        /// Method to initiate SELECT query.
        /// </summary>
        protected virtual void Select()
        {
            Query.Append("SELECT");
        }

        /// <summary>
        /// Method to initiate DELETE query.
        /// </summary>
        protected virtual void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to create column config string from column data.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected virtual string CreateColumnConfigString(ColumnModel column, bool skipConstraints = false)
        {
            StringBuilder columnConfig = new StringBuilder();

            columnConfig.Append(string.Concat(column.ColumnName, " ", column.DataType));

            /* Append max length if defined. */
            if (!string.IsNullOrEmpty(column.MaxLength))
            {
                columnConfig.Append(string.Concat("(", column.MaxLength, ")"));
            }

            /* Make column primary or unique if defined. */
            if (!skipConstraints && column.IsPrimary)
            {
                columnConfig.Append(" PRIMARY KEY");
            }
            else if (!skipConstraints && column.IsUnique)
            {
                columnConfig.Append(" UNIQUE");
            }

            /* Set default value for the column if defined. */
            if (!string.IsNullOrEmpty(column.DefaultValue))
            {
                columnConfig.Append(string.Concat(" DEFAULT('", column.DefaultValue, "')"));
            }

            return columnConfig.ToString();
        }

        /// <summary>
        /// Method to generate CREATE TABLE query.
        /// </summary>
        /// <param name="table">Table data from parsed table.</param>
        protected virtual void CreateTable(QueryDataModel table)
        {
            List<string> columnConfigList = new List<string>();
            Query.Append(string.Concat("CREATE TABLE ", Utility.GetTableName(table.TableName), " ( "));

            /* Build column config string for each column. */
            foreach (var column in table.ColumnList)
            {
                /* Add column config string to list. */
                columnConfigList.Add(CreateColumnConfigString(column));
            }

            /* Append column config string to query. */
            Query.Append(string.Concat(string.Join(", ", columnConfigList), ")"));
        }

        /// <summary>
        /// Method to generate DROP TABLE query.
        /// </summary>
        /// <param name="tableName">Table data from parsed table.</param>
        protected virtual void DropTable(string tableName)
        {
            Query.Append(string.Concat("DROP TABLE ", tableName, ";"));
        }

        /// <summary>
        /// Method to generate ALTER TABLE query.
        /// </summary>
        /// <param name="tableName">Table data from parsed table.</param>
        protected virtual void AlterTable(QueryDataModel table)
        {
            //TODO: handle column contraints.
            string alterTableBaseQuery = string.Concat("ALTER TABLE ", Utility.GetTableName(table.TableName), " ");
            List<ColumnModel> columnList;

            /* Build query to alter columns. */
            columnList = table.ColumnList.FindAll(x => string.Equals(x.ColumnOperation, "alter", StringComparison.OrdinalIgnoreCase));
            if (columnList.Count > 0)
            {
                foreach (var columnConfig in columnList)
                {
                    /* Drop constraints before altering column if exists. */
                    Query.Append(CreateDropConstraintQuery(table.TableName, columnConfig));
                    Query.Append(string.Concat(alterTableBaseQuery, "ALTER COLUMN ", CreateColumnConfigString(columnConfig, true), ";"));
                }
            }

            /* Build query to add new columns. */
            columnList = table.ColumnList.FindAll(x => string.Equals(x.ColumnOperation, "add", StringComparison.OrdinalIgnoreCase));
            if (columnList.Count > 0)
            {
                List<string> columnConfigStringList = new List<string>();

                foreach (var columnConfig in columnList)
                {
                    columnConfigStringList.Add(CreateColumnConfigString(columnConfig));
                }

                Query.Append(string.Concat(alterTableBaseQuery, "ADD ", string.Join(", ", columnConfigStringList), ";"));
            }

            /* Build query to drop columns. */
            columnList = table.ColumnList.FindAll(x => string.Equals(x.ColumnOperation, "drop", StringComparison.OrdinalIgnoreCase));
            if (columnList.Count > 0)
            {
                List<string> columnNameString = new List<string>();

                foreach (var columnConfig in columnList)
                {
                    /* Drop constraints before droping column if exists. */
                    Query.Append(CreateDropConstraintQuery(table.TableName, columnConfig));
                    columnNameString.Add(columnConfig.ColumnName);
                }

                Query.Append(string.Concat(alterTableBaseQuery, "DROP COLUMN ", string.Join(", ", columnNameString), ";"));
            }
        }

        /// <summary>
        /// Method to create drop constraint query.
        /// </summary>
        /// <param name="tableName">Table Name</param>
        /// <param name="column">Column Name</param>
        /// <returns>Drop constraint query if constraint exists else returns empty string.</returns>
        protected virtual string CreateDropConstraintQuery(string tableName, ColumnModel column)
        {
            string constraintType = GetConstraintType(column);

            if (string.IsNullOrEmpty(constraintType))
            {
                return string.Empty;
            }

            return string.Concat("DECLARE @SQL VARCHAR(MAX);SET @SQL = 'ALTER TABLE ", tableName, " DROP CONSTRAINT |ConstraintName|; 'SET @SQL = REPLACE(@SQL, '|ConstraintName|', (SELECT CONSTRAINT_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE column_name = '", column.ColumnName, "' AND TABLE_NAME = '", tableName, "'));EXEC(@SQL);");
        }

        /// <summary>
        /// Method to get constraint type for the column.
        /// </summary>
        /// <param name="column">Column name to check for constraint.</param>
        /// <returns>Constraint type if exists else returns empty string.</returns>
        protected virtual string GetConstraintType(ColumnModel column)
        {
            string constraintType = string.Empty;

            if (column.IsPrimary)
            {
                constraintType = "PK";
            }

            if (column.IsUnique)
            {
                constraintType = "UQ";
            }

            return constraintType;
        }

        /// <summary>
        /// Method to generate INSERT query.
        /// </summary>
        /// <param name="requestData">Table data to generate INSERT query.</param>
        protected virtual void InsertRecord(QueryDataModel requestData)
        {
            /* Get number of records to be filled by getting count in row value. */
            int numberOfRecords = requestData.ColumnList.First().Values.Count;

            /* Variable to store record number and records data. */
            Dictionary<int, List<string>> recordsData = new Dictionary<int, List<string>>();

            /* Variable to store list of column name. */
            List<string> columnNameList = new List<string>(),
                /* Variable to store list of parsed record values as string  */
                parsedRecords = new List<string>();

            Query.Append(string.Concat("INSERT INTO ", Utility.GetTableName(requestData.TableName), " ("));

            /* To create values group. */
            for (int i = 0; i < numberOfRecords; i++)
            {
                /* Loop through each column. */
                foreach (var column in requestData.ColumnList)
                {
                    /* Add column name to columnNameList if it does not exists. */
                    if (!columnNameList.Contains(column.ColumnName))
                    {
                        columnNameList.Add(column.ColumnName);
                    }

                    /* Add recordsData item if does not exists. */
                    if (!recordsData.ContainsKey(i))
                    {
                        recordsData.Add(i, new List<string>());
                    }

                    /* Add value to recordsData. */
                    recordsData[i].Add(string.Concat("'", column.Values[i], "'"));
                }
            }

            /* Construct parsed record string from value group. */
            foreach (var valueGroup in recordsData)
            {
                parsedRecords.Add(string.Concat("(", string.Join(", ", valueGroup.Value), ")"));
            }

            /* Append column name list and values record. */
            Query.Append(string.Concat(string.Join(", ", columnNameList), ") VALUES ", string.Join(", ", parsedRecords), ";"));
        }

        /// <summary>
        /// Method to generate UPADTE query.
        /// </summary>
        /// <param name="requestData">Table data to generate INSERT query.</param>
        protected virtual void UpdateRecord(QueryDataModel requestData)
        {
            /* Variable to store list of column name and values to be set. */
            List<string> nameValueList = new List<string>();

            Query.Append(string.Concat("UPDATE ", Utility.GetTableName(requestData.TableName), " SET "));

            /* Construct list of column name and values to be set. */
            foreach (var column in requestData.ColumnList)
            {
                nameValueList.Add(string.Concat(column.ColumnName, " = '", column.Values.FirstOrDefault(), "'"));
            }

            /* Append column name and values to be updated. */
            Query.Append(string.Join(", ", nameValueList));

            /* Append where condition if exists. */
            Where(requestData);

            /* Append semicolon to mark end of statement. */
            Query.Append(";");
        }

        /// <summary>
        /// Method to parse and load column names.
        /// </summary>
        protected void LoadColumnsToBeSelected()
        {
            /* Variable to store list of parsed column names. */
            List<string> columnList = new List<string>();

            /* Loop into each table. */
            foreach (var tableData in QueryModelObj.QueryData)
            {
                bool isJoinClauseExists = (tableData.TableClause.Where(n => n.ClauseType.Equals("JOIN", StringComparison.OrdinalIgnoreCase)).Count() > 0);

                /* Loop into each column and parse column name. */
                foreach (var column in tableData.ColumnList)
                {
                    string columnName = string.Concat((isJoinClauseExists ? string.Concat("[", column.BelongsToTable, "].") : ""), "[", column.ColumnName, "]"), columnAlias = column.Alias.Trim();

                    /* Append column operation if defined. */
                    if (!string.IsNullOrWhiteSpace(column.ColumnOperation))
                    {
                        columnName = string.Concat(column.ColumnOperation, "(", (isJoinClauseExists ? string.Concat("[", column.BelongsToTable, "].") : ""), "[", column.ColumnName, "])");
                    }

                    /* Append column name alias if defined. */
                    if (!string.IsNullOrWhiteSpace(columnAlias))
                    {
                        columnName = string.Concat(columnName, " AS ", "'", columnAlias, "'");
                        ColumnAliasMap.Add(column.ColumnName, columnAlias);
                    }

                    /* Add parsed column name to list. */
                    columnList.Add(columnName);
                    ExpectedColumnList.Add(column.ColumnName);
                }

                /* Append column names and table name to list. */
                Query.Append(string.Concat(" ", String.Join(", ", columnList), " FROM ", Utility.GetTableName(tableData.TableName)));
            }
        }

        /// <summary>
        /// Method to generate query string.
        /// </summary>
        /// <returns>Returns new QueryResponseObject with query string, column alias map and expected column list to validate response.</returns>
        public QueryResponseObject GenerateQuery()
        {
            /* Type of query to be parsed. */
            string queryType = QueryModelObj.QueryType;

            /* Flow to generate query. */
            switch (queryType.ToLower())
            {
                case "select":
                    foreach (var table in QueryModelObj.QueryData)
                    {
                        Select();
                        Top(table);
                        Distinct(table);
                        LoadColumnsToBeSelected();
                        Join(table);
                        Where(table);
                        GroupBy(table);
                        Having(table);
                        OrderBy(table);
                    }
                    break;

                case "create":
                    foreach (var table in QueryModelObj.QueryData)
                    {
                        CreateTable(table);
                    }
                    break;

                case "drop":
                    foreach (var table in QueryModelObj.QueryData)
                    {
                        DropTable(table.TableName);
                    }
                    break;

                case "insert":
                    foreach (var table in QueryModelObj.QueryData)
                    {
                        InsertRecord(table);
                    }
                    break;

                case "alter":
                    foreach (var table in QueryModelObj.QueryData)
                    {
                        AlterTable(table);
                    }
                    break;

                case "update":
                    foreach (var table in QueryModelObj.QueryData)
                    {
                        UpdateRecord(table);
                    }
                    break;

                default:
                    break;
            }

            return new QueryResponseObject
            {
                /* Generated query string. */
                Query = Query.ToString(),

                /* Column alias map to validate response. */
                ColumnAliasMap = ColumnAliasMap,

                /* Expected column list to validate response. */
                ExpectedColumnList = ExpectedColumnList
            };
        }

        /// <summary>
        /// Method to get query type.
        /// </summary>
        /// <returns>Returns query type.</returns>
        public string GetQueryType()
        {
            try
            {
                return QueryModelObj.QueryType;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
