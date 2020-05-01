using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using SqlUtilityCore.QueryValidator.Base;
using SqlUtilityCore.Model;
using SqlUtilityCore.Utilities;

namespace SqlUtilityCore.QueryValidator
{
    /// <summary>
    /// Class which inherits QueryValidatorBase to validate MS SQL query..
    /// </summary>
    public class MsSqlQueryValidator : QueryValidatorBase
    {
        /// <summary>
        /// Constructor with table list as argument.
        /// </summary>
        /// <param name="tableList">Table list.</param>
        public MsSqlQueryValidator(List<TableSchema> tableList) : base(tableList, Utility.GetConnectionString("MSSQL")) { }

        /// <summary>
        /// Constructor without argument.
        /// </summary>
        public MsSqlQueryValidator() : base(Utility.GetConnectionString("MSSQL")) { }

        /// <summary>
        /// Method to create table.
        /// </summary>
        /// <param name="table">Table data.</param>
        protected override void CreateTable(TableSchema table)
        {
            try
            {
                /* Column name list to be appended. */
                List<string> columnList = new List<string>();

                /* Get table name without schema name. */
                string parsedTableName = Utility.GetTableName(table.Name);

                /* Loop through each column to build column name list. */
                foreach (var column in table.Columns)
                {
                    /* Variable to build column config to create table. */
                    StringBuilder columnConfigString = new StringBuilder();

                    /* Append column name. */
                    columnConfigString.Append(string.Concat("[", column.Name, "] ", column.DataType));

                    /* Append max length if exists. */
                    if (!string.IsNullOrWhiteSpace(column.MaxLength))
                    {
                        columnConfigString.Append(string.Concat(" (", column.MaxLength, ")"));
                    }

                    /* If AllowNull is false then append 'NOT' before appending 'NULL' to make column not nullable.  */
                    if (!column.AllowNull)
                    {
                        columnConfigString.Append(" NOT");
                    }

                    columnConfigString.Append(" NULL");

                    /* Add currently generated column config string to column list. */
                    columnList.Add(columnConfigString.ToString());
                }

                /* Execute non query statement to create table using column config list. */
                ExecuteNonQuery(string.Concat("CREATE TABLE [dbo].[", parsedTableName, "] ( ", string.Join(", ", columnList), " )"));

                /* Set IsTableCreated flag for the current table name to true. */
                IsTableCreated.Add(parsedTableName, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to execute MS SQL query command.
        /// </summary>
        /// <param name="queryString">Query command to be excuted.</param>
        /// <returns>Returns list of column name from response.</returns>
        public override List<string> ExecuteQuery(string queryString)
        {
            try
            {
                /* Variable to store column names from response.  */
                List<string> resultantColumns = new List<string>();

                /* Create connection. */
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    /* Create command to excute. */
                    SqlCommand command = new SqlCommand(queryString, connection);

                    /* Open connection. */
                    connection.Open();

                    /* Execute command. */
                    SqlDataReader reader = command.ExecuteReader();

                    /* Add all column names form the response. */
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultantColumns.Add(reader.GetName(i));
                    }

                    /* Close connection. */
                    reader.Close();
                }

                return resultantColumns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to execute MS SQL non-query command.
        /// </summary>
        /// <param name="queryString">Query command to be excuted.</param>
        /// <returns>Returns number rows affected.</returns>
        public override object ExecuteNonQuery(string queryString)
        {
            try
            {
                /* Variable to store rows affected. */
                int rowsAffected;

                /* Create connection. */
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    /* Create command to excute. */
                    SqlCommand command = new SqlCommand(queryString, connection);

                    /* Open connection. */
                    connection.Open();

                    /* Execute command. */
                    rowsAffected = command.ExecuteNonQuery();
                }
                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
