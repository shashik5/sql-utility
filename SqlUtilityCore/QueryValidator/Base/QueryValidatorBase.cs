using System;
using System.Collections.Generic;
using SqlUtilityCore.Interface;
using SqlUtilityCore.Model;
using SqlUtilityCore.Utilities;

namespace SqlUtilityCore.QueryValidator.Base
{
    /// <summary>
    /// Base class to validate query.
    /// </summary>
    public class QueryValidatorBase : IQueryValidator
    {
        /// <summary>
        /// Connection string to connect to database.
        /// </summary>
        protected string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// List of tables.
        /// </summary>
        protected List<TableSchema> TableList { get; set; } = new List<TableSchema>();

        /// <summary>
        /// Flag to check if table is created.
        /// </summary>
        protected Dictionary<string, bool> IsTableCreated = new Dictionary<string, bool>();

        /// <summary>
        /// Column name and alias details for validating response.
        /// </summary>
        private Dictionary<string, string> ColumnAliasMap = new Dictionary<string, string>();

        /// <summary>
        /// List of column names to validate response.
        /// </summary>
        protected List<string> ExpectedColumnList = new List<string>();

        /// <summary>
        /// Constructor with table list and connection string as arguments.
        /// </summary>
        /// <param name="tableList">List of TableSchema model.</param>
        /// <param name="connectionString">Connection string to connect to database.</param>
        public QueryValidatorBase(List<TableSchema> tableList, string connectionString)
        {
            /* Set table list. */
            TableList = tableList;

            /* Set connection string. */
            ConnectionString = connectionString;

            /* Create all tables in database. */
            foreach (var table in tableList)
            {
                CreateTable(table);
            }
        }

        /// <summary>
        /// Constructor with connection staring as argument.
        /// </summary>
        /// <param name="connectionString">Connection string to connect to database.</param>
        public QueryValidatorBase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Method to execute query command.
        /// </summary>
        /// <param name="queryString">Query command to be excuted.</param>
        /// <returns>Returns list of column name from response.</returns>
        public virtual List<string> ExecuteQuery(string queryString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to execute non-query command.
        /// </summary>
        /// <param name="queryString">Query command to be excuted.</param>
        /// <returns>Returns result as object.</returns>
        public virtual object ExecuteNonQuery(string queryString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to create table.
        /// </summary>
        /// <param name="table">Table data.</param>
        protected virtual void CreateTable(TableSchema table)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to destroy/delete tables from database.
        /// </summary>
        public virtual void Destroy()
        {
            /* Drop each table from data. */
            foreach (var table in TableList)
            {
                try
                {
                    /* Get table as in database. */
                    string parsedTableName = Utility.GetTableName(table.Name);

                    /* Create and execute Drop query. */
                    ExecuteNonQuery(string.Concat("DROP TABLE [dbo].[", parsedTableName, "]"));

                    /* Set IsTableCreated flag for the table to false. */
                    IsTableCreated[parsedTableName] = false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Method to validate response by comparing column names from query response and column names from executed query.
        /// </summary>
        /// <param name="columnsInQueryResponse">List of column names from response.</param>
        /// <returns>Returns true if query is valid.</returns>
        protected virtual bool ValidateResponse(List<string> columnsInQueryResponse)
        {
            bool isValid = true;

            foreach (var columnName in ExpectedColumnList)
            {
                if (!columnsInQueryResponse.Contains(GetColumnAliasName(columnName)))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Method to add table for validator.
        /// </summary>
        /// <param name="table">Table data.</param>
        public virtual void AddTable(TableSchema table)
        {
            string parsedTableName = Utility.GetTableName(table.Name);

            /* Create table if not exists in database. */
            if (!IsTableCreated.ContainsKey(parsedTableName) || !IsTableCreated[parsedTableName])
            {
                /* Create table. */
                CreateTable(table);

                /* Add table to table list. */
                TableList.Add(table);
            }
        }

        /// <summary>
        /// Method to get column alias from column name.
        /// </summary>
        /// <param name="columnName">Column name as in database.</param>
        /// <returns>Returns column alias name. If alias does not exists then returns column name.</returns>
        protected string GetColumnAliasName(string columnName)
        {
            /* Return column alias name if exists else return column name. */
            if (ColumnAliasMap.ContainsKey(columnName))
            {
                return ColumnAliasMap[columnName];
            }
            else
            {
                return columnName;
            }
        }

        /// <summary>
        /// Method to validate query response object with QueryResponseObject as argument.
        /// </summary>
        /// <param name="queryObject">Query response object to be validated.</param>
        /// <param name="executeNonQuery">Pass true if query does not return data.</param>
        /// <returns>Returns true if query is valid.</returns>
        public virtual bool ValidateQuery(QueryResponseObject queryObject, bool executeNonQuery = false)
        {
            if (!executeNonQuery)
            {
                /* Set column alias map. */
                ColumnAliasMap = queryObject.ColumnAliasMap;

                /* Set expected column list. */
                ExpectedColumnList = queryObject.ExpectedColumnList;

                /* Execute query, validate response and return result. */
                return ValidateResponse(ExecuteQuery(queryObject.Query));
            }
            else
            {
                try
                {
                    /* Execute non query command and retrurn true if command executed successfully. */
                    ExecuteNonQuery(queryObject.Query);
                    return true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Method to validate query response object with query string as argument.
        /// </summary>
        /// <param name="queryString">Query string to be validated.</param>
        /// <param name="executeNonQuery">Pass true if query does not return data.</param>
        /// <returns>Returns true if query is valid.</returns>
        public virtual bool ValidateQuery(string queryString, bool executeNonQuery = false)
        {
            if (!executeNonQuery)
            {
                /* Execute query, validate response and return result. */
                return ValidateResponse(ExecuteQuery(queryString));
            }
            else
            {
                try
                {
                    /* Execute non query command and retrurn true if command executed successfully. */
                    ExecuteNonQuery(queryString);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
