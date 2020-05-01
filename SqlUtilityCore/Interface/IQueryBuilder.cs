using SqlUtilityCore.Model;

namespace SqlUtilityCore.Interface
{
    public interface IQueryBuilder
    {
        /// <summary>
        /// Method to generate query string.
        /// </summary>
        /// <returns>Returns new QueryResponseObject with query string, column alias map and expected column list to validate response.</returns>
        QueryResponseObject GenerateQuery();

        /// <summary>
        /// Method to load JSON string to generate query.
        /// </summary>
        /// <param name="queryModelJson">JSON string to be parsed.</param>
        void LoadJson(string queryModelJson);
    }
}
