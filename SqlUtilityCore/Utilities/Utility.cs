using System.Linq;

namespace SqlUtilityCore.Utilities
{
    /// <summary>
    /// Static class which contains utility methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Variable to indicate if service hosted in local.
        /// </summary>
        private static bool IsHostedInLocal = true;

        /// <summary>
        /// Method to generate table name with schema.
        /// </summary>
        /// <param name="tableName">Table name from schema.</param>
        /// <returns>Returns table name with schema.</returns>
        public static string ConstructTableNameWithSchema(string tableName)
        {
            var nameArray = tableName.Split('.');
            return string.Concat("[", string.Join("].[", nameArray), "]");
        }

        /// <summary>
        /// Method to extract table name from 
        /// </summary>
        /// <param name="tableName">Table name from schema.</param>
        /// <returns>Returns table name without schema.</returns>
        public static string GetTableName(string tableName)
        {
            var nameArray = tableName.Split('.');
            return nameArray.Last().Replace("[", "").Replace("]", "");
        }

        /// <summary>
        /// Method to get connection string based on database type.
        /// </summary>
        /// <param name="dbType">Database type.</param>
        /// <returns>Returns connection string for the selected database type.</returns>
        public static string GetConnectionString(string dbType)
        {
            string connectionString = string.Empty;

            switch (dbType.ToUpper())
            {
                case "MSSQL":
                    if (IsAppHostedInLocal())
                    {
                        connectionString = @"Data Source=SHASHI-PC;Initial Catalog=SqlUtilityDB;Persist Security Info=True;User ID=sa;Password=sa123";
                    }
                    break;
                default:
                    break;
            }

            return connectionString;
        }

        /// <summary>
        /// Method to check if service hosted in local.
        /// </summary>
        /// <returns>Returns true if service hosted in local.</returns>
        public static bool IsAppHostedInLocal()
        {
            return IsHostedInLocal;
        }

        /// <summary>
        /// Method to set service hosted in server.
        /// </summary>
        public static void SetAsAppHostedInServer()
        {
            IsHostedInLocal = false;
        }
    }
}
