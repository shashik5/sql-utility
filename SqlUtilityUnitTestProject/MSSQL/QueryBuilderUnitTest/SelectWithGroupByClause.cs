using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityCore.SchemaParser;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;
using System.Collections.Generic;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class SelectWithGroupByClause : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test simple select query with GROUP BY clause with SUM operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithGroupByClause_WithSumOperation()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Level\",\"ColumnOperation\":\"SUM\"}],\"TableClause\":[{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    List<string> colorList = new List<string>();

                    while (reader.Read())
                    {
                        var color = reader.GetString(0);
                        if (colorList.Contains(color) && reader.GetInt32(1) > -1)
                        {
                            isValid = false;
                            break;
                        }

                        colorList.Add(color);
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
        /// To test simple select query with GROUP BY clause with SUM operation along with WHERE clause in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithGroupByClause_WithSumOperation_WithWhereClause_1()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Level\",\"ColumnOperation\":\"SUM\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color IN ('Black', 'Grey')\"},{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        var color = reader.GetString(0);
                        if (!(color.Equals("Black", StringComparison.OrdinalIgnoreCase) || color.Equals("Grey", StringComparison.OrdinalIgnoreCase)) && reader.GetInt32(1) > -1)
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
        /// To test simple select query with GROUP BY clause with SUM operation along with WHERE clause in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithGroupByClause_WithSumOperation_WithWhereClause_2()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Level\",\"ColumnOperation\":\"SUM\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color in ('Black', 'Grey')\"},{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        var color = reader.GetString(0);
                        if (!(color.Equals("Black", StringComparison.OrdinalIgnoreCase) || color.Equals("Grey", StringComparison.OrdinalIgnoreCase)) && reader.GetInt32(1) > -1)
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
