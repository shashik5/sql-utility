using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;
using System.Collections.Generic;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class SelectWithJoin : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// Query Validator instance.
        /// </summary>
        MsSqlQueryValidator Validator = new MsSqlQueryValidator();

        /// <summary>
        /// Query object for create table query.
        /// </summary>
        QueryResponseObject CreateResponseObject = new MsSqlQueryBuilder("{\"QueryType\":\"Create\",\"QueryName\":\"CreateTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"DataType\":\"INT\",\"IsUnique\":true},{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]},{\"TableName\":\"TestTable2\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Address\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"500\"},{\"ColumnName\":\"Contact_Number\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"10\"}]},{\"TableName\":\"TestTable3\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"OrderId\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"10\"}]}],\"QueryClause\":[]}").GenerateQuery();

        /// <summary>
        /// Query object for drop table query.
        /// </summary>
        QueryResponseObject DropResponseObject = new MsSqlQueryBuilder("{\"QueryType\":\"Drop\",\"QueryName\":\"DropTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable1\"},{\"TableName\":\"TestTable2\"},{\"TableName\":\"TestTable3\"}]}").GenerateQuery();

        /// <summary>
        /// Method to create table in db.
        /// </summary>
        /// <returns>Returns true if success.</returns>
        private bool CreateTable()
        {
            try
            {
                ValidateByExecutingNonQuery(CreateResponseObject.Query);
                Validator.ExecuteNonQuery(CreateResponseObject.Query);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to drop table in db.
        /// </summary>
        /// <returns>Returns true if success.</returns>
        private bool DropTable()
        {
            try
            {
                ValidateByExecutingNonQuery(DropResponseObject.Query);
                Validator.ExecuteNonQuery(DropResponseObject.Query);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// To test SELECT query with JOIN with two tables.
        /// </summary>
        [TestMethod]
        public void SelectWithJoin_InnerJoin_2_Tables()
        {
            MsSqlQueryBuilder joinQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SelectTableQueryWithJoin\",\"QueryData\":[{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"UserName\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"Email\",\"BelongsToTable\":\"TestTable1\",\"Alias\":\"Email\"},{\"ColumnName\":\"Address\",\"BelongsToTable\":\"TestTable2\"},{\"ColumnName\":\"Contact_Number\",\"BelongsToTable\":\"TestTable2\",\"Alias\":\"Contact\"}],\"TableClause\":[{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable2.UserName\",\"JoinType\":\"Inner\",\"JoinTableName\":\"TestTable2\"}]}],\"QueryClause\":[]}");

            QueryResponseObject joinQueryResponseObject = joinQueryBuilder.GenerateQuery();
            ColumnAliasMap = joinQueryResponseObject.ColumnAliasMap;

            try
            {
                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    List<string> resultantColumns = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultantColumns.Add(reader.GetName(i));
                    }

                    foreach (var expectedColumnName in joinQueryResponseObject.ExpectedColumnList)
                    {
                        if (!resultantColumns.Contains(GetColumnAliasName(expectedColumnName)))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    return isValid;
                }));

                CreateTable();
                Assert.IsTrue(ValidateUsingDbResponse(joinQueryResponseObject.Query, predicateFunction) && Validator.ValidateQuery(joinQueryResponseObject));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DropTable();
            }
        }

        /// <summary>
        /// To test SELECT query with JOIN with three tables.
        /// </summary>
        [TestMethod]
        public void SelectWithJoin_InnerJoin_3_Tables()
        {
            MsSqlQueryBuilder joinQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SelectTableQueryWithJoin\",\"QueryData\":[{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"UserName\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"Email\",\"BelongsToTable\":\"TestTable1\",\"Alias\":\"Email\"},{\"ColumnName\":\"Address\",\"BelongsToTable\":\"TestTable2\"},{\"ColumnName\":\"Contact_Number\",\"BelongsToTable\":\"TestTable2\",\"Alias\":\"Contact\"},{\"ColumnName\":\"OrderId\",\"BelongsToTable\":\"TestTable3\"}],\"TableClause\":[{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable2.UserName\",\"JoinType\":\"Inner\",\"JoinTableName\":\"TestTable2\"},{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable3.UserName\",\"JoinType\":\"Inner\",\"JoinTableName\":\"TestTable3\"}]}],\"QueryClause\":[]}");

            QueryResponseObject joinQueryResponseObject = joinQueryBuilder.GenerateQuery();
            ColumnAliasMap = joinQueryResponseObject.ColumnAliasMap;

            try
            {
                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    List<string> resultantColumns = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultantColumns.Add(reader.GetName(i));
                    }

                    foreach (var expectedColumnName in joinQueryResponseObject.ExpectedColumnList)
                    {
                        if (!resultantColumns.Contains(GetColumnAliasName(expectedColumnName)))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    return isValid;
                }));

                CreateTable();
                Assert.IsTrue(ValidateUsingDbResponse(joinQueryResponseObject.Query, predicateFunction) && Validator.ValidateQuery(joinQueryResponseObject));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DropTable();
            }
        }

        /// <summary>
        /// To test SELECT query with Left with join with three tables.
        /// </summary>
        [TestMethod]
        public void SelectWithJoin_LeftJoin_3_Tables()
        {
            MsSqlQueryBuilder joinQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SelectTableQueryWithJoin\",\"QueryData\":[{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"UserName\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"Email\",\"BelongsToTable\":\"TestTable1\",\"Alias\":\"Email\"},{\"ColumnName\":\"Address\",\"BelongsToTable\":\"TestTable2\"},{\"ColumnName\":\"Contact_Number\",\"BelongsToTable\":\"TestTable2\",\"Alias\":\"Contact\"},{\"ColumnName\":\"OrderId\",\"BelongsToTable\":\"TestTable3\"}],\"TableClause\":[{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable2.UserName\",\"JoinType\":\"LEFT\",\"JoinTableName\":\"TestTable2\"},{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable3.UserName\",\"JoinType\":\"LEFT\",\"JoinTableName\":\"TestTable3\"}]}],\"QueryClause\":[]}");

            QueryResponseObject joinQueryResponseObject = joinQueryBuilder.GenerateQuery();
            ColumnAliasMap = joinQueryResponseObject.ColumnAliasMap;

            try
            {
                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    List<string> resultantColumns = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultantColumns.Add(reader.GetName(i));
                    }

                    foreach (var expectedColumnName in joinQueryResponseObject.ExpectedColumnList)
                    {
                        if (!resultantColumns.Contains(GetColumnAliasName(expectedColumnName)))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    return isValid;
                }));

                CreateTable();
                Assert.IsTrue(ValidateUsingDbResponse(joinQueryResponseObject.Query, predicateFunction) && Validator.ValidateQuery(joinQueryResponseObject));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DropTable();
            }
        }

        /// <summary>
        /// To test SELECT query with Left and Right join with three tables.
        /// </summary>
        [TestMethod]
        public void SelectWithJoin_LeftRightJoin_3_Tables()
        {
            MsSqlQueryBuilder joinQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SelectTableQueryWithJoin\",\"QueryData\":[{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"UserName\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"Email\",\"BelongsToTable\":\"TestTable1\",\"Alias\":\"Email\"},{\"ColumnName\":\"Address\",\"BelongsToTable\":\"TestTable2\"},{\"ColumnName\":\"Contact_Number\",\"BelongsToTable\":\"TestTable2\",\"Alias\":\"Contact\"},{\"ColumnName\":\"OrderId\",\"BelongsToTable\":\"TestTable3\"}],\"TableClause\":[{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable2.UserName\",\"JoinType\":\"LEFT\",\"JoinTableName\":\"TestTable2\"},{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable3.UserName\",\"JoinType\":\"RIGHT\",\"JoinTableName\":\"TestTable3\"}]}],\"QueryClause\":[]}");

            QueryResponseObject joinQueryResponseObject = joinQueryBuilder.GenerateQuery();
            ColumnAliasMap = joinQueryResponseObject.ColumnAliasMap;

            try
            {
                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    List<string> resultantColumns = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultantColumns.Add(reader.GetName(i));
                    }

                    foreach (var expectedColumnName in joinQueryResponseObject.ExpectedColumnList)
                    {
                        if (!resultantColumns.Contains(GetColumnAliasName(expectedColumnName)))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    return isValid;
                }));

                CreateTable();
                Assert.IsTrue(ValidateUsingDbResponse(joinQueryResponseObject.Query, predicateFunction) && Validator.ValidateQuery(joinQueryResponseObject));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DropTable();
            }
        }

        /// <summary>
        /// To test SELECT query with Full and Left join with three tables.
        /// </summary>
        [TestMethod]
        public void SelectWithJoin_CrossLeft_3_Tables()
        {
            MsSqlQueryBuilder joinQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Select\",\"QueryName\":\"SelectTableQueryWithJoin\",\"QueryData\":[{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"UserName\",\"BelongsToTable\":\"TestTable1\"},{\"ColumnName\":\"Email\",\"BelongsToTable\":\"TestTable1\",\"Alias\":\"Email\"},{\"ColumnName\":\"Address\",\"BelongsToTable\":\"TestTable2\"},{\"ColumnName\":\"Contact_Number\",\"BelongsToTable\":\"TestTable2\",\"Alias\":\"Contact\"},{\"ColumnName\":\"OrderId\",\"BelongsToTable\":\"TestTable3\"}],\"TableClause\":[{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable2.UserName\",\"JoinType\":\"FULL\",\"JoinTableName\":\"TestTable2\"},{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"TestTable1.UserName = TestTable3.UserName\",\"JoinType\":\"LEFT\",\"JoinTableName\":\"TestTable3\"}]}],\"QueryClause\":[]}");

            QueryResponseObject joinQueryResponseObject = joinQueryBuilder.GenerateQuery();
            ColumnAliasMap = joinQueryResponseObject.ColumnAliasMap;

            try
            {
                PredicateFunction predicateFunction = new PredicateFunction(new Func<SqlDataReader, bool>(reader =>
                {
                    bool isValid = true;
                    List<string> resultantColumns = new List<string>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        resultantColumns.Add(reader.GetName(i));
                    }

                    foreach (var expectedColumnName in joinQueryResponseObject.ExpectedColumnList)
                    {
                        if (!resultantColumns.Contains(GetColumnAliasName(expectedColumnName)))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    return isValid;
                }));

                CreateTable();
                Assert.IsTrue(ValidateUsingDbResponse(joinQueryResponseObject.Query, predicateFunction) && Validator.ValidateQuery(joinQueryResponseObject));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DropTable();
            }
        }
    }
}