using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityCore.SchemaParser;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class SingleSelect : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test simple select query with single table.
        /// </summary>
        [TestMethod]
        public void SimpleSelectQueryWithSingleTable()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"}],\"TableClause\":[]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                Assert.IsTrue(validator.ValidateQuery(queryBuilder.GenerateQuery()));
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
