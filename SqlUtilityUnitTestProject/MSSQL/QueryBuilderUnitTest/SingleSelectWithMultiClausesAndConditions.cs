using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityCore.SchemaParser;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class SingleSelectWithMultiClausesAndConditions : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test simple select query with WHERE clause along with TOP clause in single table.
        /// </summary>
        [TestMethod]
        public void SingleSelectWithMultiClausesAndConditions_Top_WhereClauseWithSingleCondition()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color = 'black'\"},{\"ClauseType\":\"TOP\",\"ClauseValue\":\"20\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    int rowCount = 0;

                    while (reader.Read())
                    {
                        if (!reader.GetString(4).Equals("Black", StringComparison.OrdinalIgnoreCase))
                        {
                            isValid = false;
                            break;
                        }
                        rowCount++;
                    }
                    return isValid && rowCount == 20;
                }));

                Assert.IsTrue(ValidateUsingDbResponse(queryResponseObject.Query, predicateFunction) && validator.ValidateQuery(queryResponseObject));
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                validator.Destroy();
            }
        }
    }
}
