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
    public class SelectWithColumnOperation : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test select query with column SUM operation in single table
        /// </summary>
        [TestMethod]
        public void SimpleSelectQueryWithSingleTable_Sum()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator();
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"SafetyStockLevel\",\"ColumnOperation\":\"SUM\",\"Alias\":\"Safety Stock Level Total\"}],\"TableClause\":[]}],\"QueryClause\":[]}");

                validator.AddTable(table);

                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    reader.Read();
                    return reader.GetInt32(0) == 300092;
                }));

                Assert.IsTrue(ValidateUsingDbResponse(queryResponseObject.Query, predicateFunction) && validator.ValidateQuery(queryResponseObject));
            }
            catch (Exception)
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
