using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryBuilder;
using SqlUtilityCore.QueryValidator;
using SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest.Base;

namespace SqlUtilityUnitTestProject.MSSQL.QueryBuilderUnitTest
{
    [TestClass]
    public class CreateAlterAndDropTable : QueryBuilderUnitTestBase
    {
        /// <summary>
        /// Query Validator instance.
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
        /// To test CREATE and DROP table query.
        /// </summary>
        [TestMethod]
        public void CreateAlterAndDropTable_CreateSingleTable()
        {
            MsSqlQueryBuilder createQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Create\",\"QueryName\":\"CreateTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]}],\"QueryClause\":[]}");

            MsSqlQueryBuilder dropQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Drop\",\"QueryName\":\"DropTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\"}]}");

            QueryResponseObject createQueryResponseObject = createQueryBuilder.GenerateQuery();
            QueryResponseObject dropQueryResponseObject = dropQueryBuilder.GenerateQuery();

            try
            {
                Assert.IsTrue(ValidateByExecutingNonQuery(createQueryResponseObject.Query) && ValidateByExecutingNonQuery(dropQueryResponseObject.Query) && Validator.ValidateQuery(createQueryResponseObject, true) && Validator.ValidateQuery(dropQueryResponseObject, true));
            }
            catch (Exception)
            {
                ValidateByExecutingNonQuery(dropQueryResponseObject.Query);
                throw;
            }
        }

        /// <summary>
        /// To test CREATE and DROP table query.
        /// </summary>
        [TestMethod]
        public void CreateAlterAndDropTable_CreateMultipleTable()
        {
            MsSqlQueryBuilder createQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Create\",\"QueryName\":\"CreateTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]},{\"TableName\":\"TestTable1\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]}],\"QueryClause\":[]}");

            MsSqlQueryBuilder dropQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Drop\",\"QueryName\":\"DropTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\"},{\"TableName\":\"TestTable1\"}]}");

            QueryResponseObject createQueryResponseObject = createQueryBuilder.GenerateQuery();
            QueryResponseObject dropQueryResponseObject = dropQueryBuilder.GenerateQuery();

            try
            {
                Assert.IsTrue(ValidateByExecutingNonQuery(createQueryResponseObject.Query) && ValidateByExecutingNonQuery(dropQueryResponseObject.Query) && Validator.ValidateQuery(createQueryResponseObject, true) && Validator.ValidateQuery(dropQueryResponseObject, true));
            }
            catch (Exception)
            {
                ValidateByExecutingNonQuery(dropQueryResponseObject.Query);
                throw;
            }
        }

        /// <summary>
        /// To test CREATE and DROP table query Error test.
        /// </summary>
        [TestMethod]
        public void CreateAlterAndDropTable_CreateMultipleTable_ErrorTest()
        {
            MsSqlQueryBuilder createQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Create\",\"QueryName\":\"CreateTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]},{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnName\":\"UserName\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\",\"IsPrimary\":true},{\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"50\"},{\"ColumnName\":\"Email\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"200\",\"IsUnique\":true}]}],\"QueryClause\":[]}");

            MsSqlQueryBuilder dropQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Drop\",\"QueryName\":\"DropTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\"}]}");

            QueryResponseObject createQueryResponseObject = createQueryBuilder.GenerateQuery();
            QueryResponseObject dropQueryResponseObject = dropQueryBuilder.GenerateQuery();

            try
            {
                ValidateByExecutingNonQuery(createQueryResponseObject.Query);
                ValidateByExecutingNonQuery(dropQueryResponseObject.Query);
                Validator.ValidateQuery(createQueryResponseObject, true);
                Validator.ValidateQuery(dropQueryResponseObject, true);
            }
            catch (Exception)
            {
                try
                {
                    ValidateByExecutingNonQuery(dropQueryResponseObject.Query);
                }
                catch (Exception)
                {
                    throw;
                }
                Assert.IsTrue(true);
            }
        }

        /// <summary>
        /// To test ALTER table query without constraints.
        /// </summary>
        [TestMethod]
        public void CreateAlterAndDropTable_AlterSingleTable_WithoutConstraints()
        {
            MsSqlQueryBuilder alterQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Alter\",\"QueryName\":\"AlterTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnOperation\":\"Alter\",\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"100\"},{\"ColumnOperation\":\"Add\",\"ColumnName\":\"Phone\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"10\"},{\"ColumnOperation\":\"Drop\",\"ColumnName\":\"Phone\"}]}],\"QueryClause\":[]}");

            QueryResponseObject alterQueryResponseObject = alterQueryBuilder.GenerateQuery();

            try
            {
                CreateTable();
                Assert.IsTrue(ValidateByExecutingNonQuery(alterQueryResponseObject.Query) && Validator.ValidateQuery(alterQueryResponseObject, true));
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
        /// To test ALTER table query with constraints.
        /// </summary>
        [TestMethod]
        public void CreateAlterAndDropTable_AlterSingleTable_WithConstraints()
        {
            MsSqlQueryBuilder alterQueryBuilder = new MsSqlQueryBuilder("{\"QueryType\":\"Alter\",\"QueryName\":\"AlterTableQuery\",\"QueryData\":[{\"TableName\":\"TestTable\",\"ColumnList\":[{\"ColumnOperation\":\"Alter\",\"ColumnName\":\"Password\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"100\"},{\"ColumnOperation\":\"Add\",\"ColumnName\":\"Phone\",\"DataType\":\"VARCHAR\",\"MaxLength\":\"10\",\"DefaultValue\":\"test123\"},{\"ColumnOperation\":\"Drop\",\"ColumnName\":\"Email\",\"IsUnique\":true}]}],\"QueryClause\":[]}");

            QueryResponseObject alterQueryResponseObject = alterQueryBuilder.GenerateQuery();

            try
            {
                CreateTable();
                Assert.IsTrue(ValidateByExecutingNonQuery(alterQueryResponseObject.Query) && Validator.ValidateQuery(alterQueryResponseObject, true));
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
