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
    public class SelectWithHavingClause : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test simple select query with GROUP BY and HAVING clause with SUM operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithHavingClause_WithGroupByClause_WithSumOperation()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Level\",\"ColumnOperation\":\"SUM\"}],\"TableClause\":[{\"ClauseType\":\"HAVING\",\"ClauseValue\":\"SUM(SafetyStockLevel) < 500\"},{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        var color = reader.GetString(0);
                        if (!(color.Equals("Grey", StringComparison.OrdinalIgnoreCase) || color.Equals("Multi", StringComparison.OrdinalIgnoreCase) || color.Equals("White", StringComparison.OrdinalIgnoreCase)) && reader.GetInt32(1) > -1)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    return isValid;
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

        /// <summary>
        /// To test simple select query with WHERE, GROUP BY and HAVING clause with COUNT operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithHavingClause_WithGroupByAndWhereClause_WithCountOperation()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Count\",\"ColumnOperation\":\"COUNT\"}],\"TableClause\":[{\"ClauseType\":\"HAVING\",\"ClauseValue\":\"SUM(SafetyStockLevel) > 500\"},{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color IN ('Black','Red')\"},{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        var color = reader.GetString(0);
                        if (!(color.Equals("Black", StringComparison.OrdinalIgnoreCase) || color.Equals("Red", StringComparison.OrdinalIgnoreCase)) && reader.GetInt32(1) > -1)
                        {
                            isValid = false;
                            break;
                        }
                    }
                    return isValid;
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
