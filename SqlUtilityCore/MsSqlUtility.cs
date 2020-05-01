using System;
using System.Collections.Generic;
using System.Linq;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityCore.SchemaParser;

namespace SqlUtilityCore
{
    public class MsSqlUtility : IDisposable
    {
        /// <summary>
        /// Variable to store list of table data.
        /// </summary>
        private List<TableSchema> TableList = new List<TableSchema>();

        /// <summary>
        /// Variable to store MsSqlQueryValidator instance.
        /// </summary>
        private MsSqlQueryValidator Validator = new MsSqlQueryValidator();

        /// <summary>
        /// Variable to store MsSqlQueryBuilder instance.
        /// </summary>
        private MsSqlQueryBuilder QueryBuilder = new MsSqlQueryBuilder();

        /// <summary>
        /// Method add/import new table data from schema xml.
        /// </summary>
        /// <param name="xmlPathOrString">XML file path or xml string.</param>
        /// <returns>Returns true if successfully added.</returns>
        public bool AddTableSchema(string xmlPathOrString)
        {
            try
            {
                TableList.Add(MsSqlSchemaParser.Parse(xmlPathOrString));
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method remove table data from TableList.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>Returns true if successfully removed.</returns>
        public bool RemoveTableSchema(string tableName)
        {
            try
            {
                TableList.RemoveAll(n => string.Equals(n.Name, tableName, StringComparison.OrdinalIgnoreCase));
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to remove all table schema data.
        /// </summary>
        /// <returns>Returns true if successfully removed.</returns>
        public bool RemoveAllTableSchema()
        {
            try
            {
                TableList.Clear();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to get table data/schema from TableList.
        /// </summary>
        /// <param name="tableName">Table name.</param>
        /// <returns>Returns table data if found.</returns>
        public TableSchema GetTableSchema(string tableName)
        {
            return TableList.FirstOrDefault(n => string.Equals(n.Name, tableName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Method to get all loaded table data/schema from TableList. 
        /// </summary>
        /// <returns>Returns TableList.</returns>
        public List<TableSchema> GetAllTableSchema()
        {
            return TableList;
        }

        /// <summary>
        /// Method to generate query string from JSON string.
        /// </summary>
        /// <param name="jsonString">JSON data to generate query.</param>
        /// <returns>Generated query string.</returns>
        public string GenerateQuery(string jsonString)
        {
            try
            {
                QueryBuilder.LoadJson(jsonString);
                var queryObject = QueryBuilder.GenerateQuery();
                LoadTablesToValidator();
                Validator.ValidateQuery(queryObject, IsRequiredToExecuteAsNonQuery());
                return queryObject.Query;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to load all table data into validator table list.
        /// </summary>
        /// <returns>Returns true if success.</returns>
        private bool LoadTablesToValidator()
        {
            try
            {
                foreach (var tableData in TableList)
                {
                    Validator.AddTable(tableData);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method to check if query needs to executed as non query.
        /// </summary>
        /// <returns>Returns true if query is to be executed as non query.</returns>
        private bool IsRequiredToExecuteAsNonQuery()
        {
            return !string.Equals(QueryBuilder.GetQueryType(), "select", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Method to destroy all tables from DB and dispose the current instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                RemoveAllTableSchema();
                Validator.Destroy();
            }
            catch (Exception)
            {

            }
        }
    }
}
