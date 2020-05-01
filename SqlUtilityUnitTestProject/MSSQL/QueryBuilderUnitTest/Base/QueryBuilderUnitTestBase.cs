using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base
{
    public class QueryBuilderUnitTestBase
    {
        /// <summary>
        /// Connection string to connect to MS SQL DB.
        /// </summary>
        protected string ConnectionString = @"Data Source=SHASHI-PC;Initial Catalog=AdventureWorksDW2012;Persist Security Info=True;User ID=sa;Password=sa123";

        /// <summary>
        /// Column name and alias details for validating response.
        /// </summary>
        protected Dictionary<string, string> ColumnAliasMap = new Dictionary<string, string>();

        /// <summary>
        /// Delegate to predicate function to validate response data.
        /// </summary>
        /// <param name="reader">SqlDataReader response object</param>
        /// <returns>Return true if response data satisfies the query conditions.</returns>
        protected delegate bool PredicateFunction(SqlDataReader reader);

        /// <summary>
        /// Common method to create sql connection, execute query and call predicate function to check if it satisfies the query conditions.
        /// </summary>
        /// <param name="queryString">Query to be executed.</param>
        /// <param name="predicate"></param>
        /// <returns>Return true if predicate function satisfies the query conditions.</returns>
        protected bool ValidateUsingDbResponse(string queryString, PredicateFunction predicate)
        {
            try
            {
                bool isValid = false;

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    /* Invoke predicate function to validate result. */
                    isValid = predicate(reader);

                    reader.Close();
                }

                return isValid;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Common method to create sql connection, execute query and return true if query successfully executed.
        /// </summary>
        /// <param name="queryString">Query to be executed.</param>
        /// <returns>Returns true if query successfully executed.</returns>
        protected bool ValidateByExecutingNonQuery(string queryString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
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
    }
}
