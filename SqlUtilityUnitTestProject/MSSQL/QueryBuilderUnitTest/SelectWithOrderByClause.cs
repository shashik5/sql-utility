using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityCore.SchemaParser;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;
using System.Collections.Generic;
using System.Linq;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class SelectWithOrderByClause : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// To test simple select query with ORDER BY ascending and with WHERE clause in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithOrderByClause_WithWhereClause_Asc()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color IN ('Black', 'Silver')\"},{\"ClauseType\":\"ORDERBY\",\"ClauseValue\":\"ProductAlternateKey ASC\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    List<string> listAsInResponse = new List<string>(), sortedList = new List<string>();

                    while (reader.Read())
                    {
                        var value = reader.GetString(1);
                        listAsInResponse.Add(value);
                        sortedList.Add(value);
                    }

                    sortedList.OrderBy(x => x);

                    return listAsInResponse.SequenceEqual(sortedList);
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
        /// To test simple select query with ORDER BY ascending and with GROUP BY clause in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithOrderByClause_WithGroupByClause_Asc()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Sum\",\"ColumnOperation\":\"SUM\"}],\"TableClause\":[{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"},{\"ClauseType\":\"ORDERBY\",\"ClauseValue\":\"SUM(SafetyStockLevel) ASC\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    List<int> listAsInResponse = new List<int>(), sortedList = new List<int>();

                    while (reader.Read())
                    {
                        var value = reader.GetInt32(1);
                        listAsInResponse.Add(value);
                        sortedList.Add(value);
                    }

                    sortedList.OrderBy(x => x);

                    return listAsInResponse.SequenceEqual(sortedList);
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
        /// To test simple select query with ORDER BY descending and with WHERE clause in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithOrderByClause_WithWhereClause_Desc()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"Color IN ('Black', 'Silver')\"},{\"ClauseType\":\"ORDERBY\",\"ClauseValue\":\"ProductAlternateKey DESC\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    List<string> listAsInResponse = new List<string>(), sortedList = new List<string>();

                    while (reader.Read())
                    {
                        var value = reader.GetString(1);
                        listAsInResponse.Add(value);
                        sortedList.Add(value);
                    }

                    sortedList.OrderByDescending(x => x);

                    return listAsInResponse.SequenceEqual(sortedList);
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
        /// To test simple select query with ORDER BY descending and with GROUP BY clause in single table.
        /// </summary>
        [TestMethod]
        public void SelectWithOrderByClause_WithGroupByClause_Desc()
        {
            MsSqlQueryValidator validator = new MsSqlQueryValidator(); ;
            try
            {
                TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

                MsSqlQueryBuilder queryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"AdventureWorksDW2012.dbo.DimProduct\",\"ColumnList\":[{\"ColumnName\":\"Color\"},{\"ColumnName\":\"SafetyStockLevel\",\"Alias\":\"Safety Stock Sum\",\"ColumnOperation\":\"SUM\"}],\"TableClause\":[{\"ClauseType\":\"GROUPBY\",\"ClauseValue\":\"Color\"},{\"ClauseType\":\"ORDERBY\",\"ClauseValue\":\"SUM(SafetyStockLevel) DESC\"}]}],\"QueryClause\":[]}");

                validator.AddTable(table);
                QueryResponseObject queryResponseObject = queryBuilder.GenerateQuery();

                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    List<int> listAsInResponse = new List<int>(), sortedList = new List<int>();

                    while (reader.Read())
                    {
                        var value = reader.GetInt32(1);
                        listAsInResponse.Add(value);
                        sortedList.Add(value);
                    }

                    sortedList.OrderByDescending(x => x);

                    return listAsInResponse.SequenceEqual(sortedList);
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
