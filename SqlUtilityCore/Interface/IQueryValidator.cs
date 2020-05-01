using SqlUtilityCore.Model;

namespace SqlUtilityCore.Interface
{
    public interface IQueryValidator
    {
        /// <summary>
        /// Method to validate query response object with query string as argument.
        /// </summary>
        /// <param name="queryString">Query string to be validated.</param>
        /// <returns>Returns true if query is valid.</returns>
        bool ValidateQuery(string queryString, bool executeNonQuery);

        /// <summary>
        /// Method to validate query response object with QueryResponseObject as argument.
        /// </summary>
        /// <param name="queryObject">Query response object to be validated.</param>
        /// <returns>Returns true if query is valid.</returns>
        bool ValidateQuery(QueryResponseObject queryObject, bool executeNonQuery);

        /// <summary>
        /// Method to destroy/delete tables from database.
        /// </summary>
        void Destroy();
    }
}
