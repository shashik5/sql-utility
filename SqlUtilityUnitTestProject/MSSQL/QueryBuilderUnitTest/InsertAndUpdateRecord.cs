using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class InsertAndUpdateRecord : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// Query validator instance.
        /// </summary>
        MsSqlQueryValidator Validator = new MsSqlQueryValidator();

        /// <summary>
        /// Query object for create table query.
        /// </summary>
        QueryResponseObject CreateResponseObject = new MsSqlQueryBuilder("{\"QueryType\":\"Create\",\"QueryName\":\"CreateTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"DataType\":\"INT\",\"IsUnique\":true},{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]}],\"QueryClause\":[]}").GenerateQuery();

        /// <summary>
        /// Query object for drop table query.
        /// </summary>
        QueryResponseObject DropResponseObject = new MsSqlQueryBuilder("{\"QueryType\":\"Drop\",\"QueryName\":\"DropTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\"}]}").GenerateQuery();

        /// <summary>
        /// Query object for insert query.
        /// </summary>
        QueryResponseObject InsertResponseObject = new MsSqlQueryBuilder("{\"QueryType\":\"Insert\",\"QueryName\":\"InsertRecordQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"Values\":[1,2,5,3]},{\"ColumnName\":\"UserName\",\"Values\":[\"t1\",\"t2\",\"t5\",\"t3\"]},{\"ColumnName\":\"Password\",\"Values\":[\"pass1\",\"pass2\",\"pass5\",\"pass3\"]},{\"ColumnName\":\"Email\",\"Values\":[\"email1\",\"email2\",\"email5\",\"email3\"]}]}]}").GenerateQuery();

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
        /// Method to insert records in db.
        /// </summary>
        /// <returns>Returns true if success.</returns>
        private bool InsertDummyRecords()
        {
            try
            {
                ValidateByExecutingNonQuery(InsertResponseObject.Query);
                Validator.ExecuteNonQuery(InsertResponseObject.Query);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// To test INSERT query with single record in single table.
        /// </summary>
        [TestMethod]
        public void InsertAndUpdateRecord_InsertSingleRecord_SingleTable()
        {
            MsSqlQueryBuilder insertQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Insert\",\"QueryName\":\"InsertRecordQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"Values\":[1]},{\"ColumnName\":\"UserName\",\"Values\":[\"t1\"]},{\"ColumnName\":\"Password\",\"Values\":[\"pass1\"]},{\"ColumnName\":\"Email\",\"Values\":[\"email1\"]}]}]}");

            QueryResponseObject insertQueryResponseObject = insertQueryBuilder.GenerateQuery();

            try
            {
                CreateTable();
                Assert.IsTrue(ValidateByExecutingNonQuery(insertQueryResponseObject.Query) && Validator.ValidateQuery(insertQueryResponseObject, true));
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
        /// To test INSERT query with multiple record in single table.
        /// </summary>
        [TestMethod]
        public void InsertAndUpdateRecord_InsertMultipleRecord_SingleTable()
        {
            MsSqlQueryBuilder insertQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Insert\",\"QueryName\":\"InsertRecordQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"ID\",\"Values\":[1,2,5,3]},{\"ColumnName\":\"UserName\",\"Values\":[\"t1\",\"t2\",\"t5\",\"t3\"]},{\"ColumnName\":\"Password\",\"Values\":[\"pass1\",\"pass2\",\"pass5\",\"pass3\"]},{\"ColumnName\":\"Email\",\"Values\":[\"email1\",\"email2\",\"email5\",\"email3\"]}]}]}");

            QueryResponseObject insertQueryResponseObject = insertQueryBuilder.GenerateQuery();

            try
            {
                CreateTable();
                Assert.IsTrue(ValidateByExecutingNonQuery(insertQueryResponseObject.Query) && Validator.ValidateQuery(insertQueryResponseObject, true));
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
        /// To test UPDATE query with single record in single table.
        /// </summary>
        [TestMethod]
        public void InsertAndUpdateRecord_UpdateSingleRecord_SingleTable()
        {
            MsSqlQueryBuilder updateQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Update\",\"QueryName\":\"UpdateRecordQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"Password\",\"Values\":[\"UpdatedValue\"]}],\"TableClause\":[{\"ClauseType\":\"WHERE\",\"ClauseValue\":\"ID = '5'\"}]}],\"QueryClause\":[]}");

            QueryResponseObject updateQueryResponseObject = updateQueryBuilder.GenerateQuery();

            try
            {
                CreateTable();
                InsertDummyRecords();
                Assert.IsTrue(ValidateByExecutingNonQuery(updateQueryResponseObject.Query) && Validator.ValidateQuery(updateQueryResponseObject, true));
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
        /// To test UPDATE query with multiple record in single table.
        /// </summary>
        [TestMethod]
        public void InsertAndUpdateRecord_UpdateMultipleRecord_SingleTable()
        {
            MsSqlQueryBuilder updateQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Update\",\"QueryName\":\"UpdateRecordQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"Password\",\"Values\":[\"UpdatedValue\"]}]}],\"QueryClause\":[]}");

            QueryResponseObject updateQueryResponseObject = updateQueryBuilder.GenerateQuery();

            try
            {
                CreateTable();
                InsertDummyRecords();
                Assert.IsTrue(ValidateByExecutingNonQuery(updateQueryResponseObject.Query) && Validator.ValidateQuery(updateQueryResponseObject, true));
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
