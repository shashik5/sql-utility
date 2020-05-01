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
    public class SelectWithWhereClause : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test simple select query with WHERE clause '=' operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithWhereClause_SingleCondition_EqualToOperator()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color = 'black'\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        if (!reader.GetString(4).Equals("Black", StringComparison.OrdinalIgnoreCase))
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
        /// To test simple select query with WHERE clause 'LIKE' operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithWhereClause_SingleCondition_LikeOperator()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"EnglishProductName LIKE '%Crankarm'\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        if (!reader.GetString(3).EndsWith("Crankarm", StringComparison.OrdinalIgnoreCase))
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
        /// To test simple select query with WHERE clause Comparison operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithWhereClause_SingleCondition_ComparisonOperator()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"ProductKey <= 20\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) > 20)
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
        /// To test simple select query with WHERE clause Between operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithWhereClause_SingleCondition_BetweenOperator()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"ProductKey BETWEEN 12 AND 25\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        var key = reader.GetInt32(0);
                        if (key < 12 && key > 25)
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
        /// To test simple select query with WHERE clause IN operation in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithWhereClause_SingleCondition_InOperator()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color IN ('Black', 'Silver')\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;

                    while (reader.Read())
                    {
                        var color = reader.GetString(4);
                        if (!color.Equals("Black", StringComparison.OrdinalIgnoreCase) && !color.Equals("Silver", StringComparison.OrdinalIgnoreCase))
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
        /// To test simple select query with WHERE clause '=', 'LIKE' and 'AND' operations in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithWhereClause_MultiCondition_1()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"},{\"ColumnName\":\"Color\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"EnglishProductName LIKE 'Chainring%' AND Color = 'Black'\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    reader.Read();
                    return reader.GetString(3).StartsWith("Chainring", StringComparison.OrdinalIgnoreCase) && (reader.GetInt32(0) == 11);
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
