using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.Model;
using SqlUtilityCore.QueryValidator;
using SqlUtilityCore.SchemaParser;

namespace SqlUtilityUnitTestProject.MSSQL
{
    [TestClass]
    public class MsSqlQueryValidatorUnitTest
    {
        /// <summary>
        /// Variable to store MsSqlQueryValidator instance.
        /// </summary>
        MsSqlQueryValidator Validator = new MsSqlQueryValidator();

        /// <summary>
        /// Variable to store parsed table data.
        /// </summary>
        TableSchema Table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

        /// <summary>
        /// Method to initialize table data in validator.
        /// </summary>
        private void InitTable()
        {
            Validator.AddTable(Table);
        }

        /// <summary>
        /// Method to destroy table added in validator.
        /// </summary>
        private void Destroy()
        {
            Validator.Destroy();
        }

        /// <summary>
        /// Test case to test query of query type.
        /// </summary>
        [TestMethod]
        public void MsSqlQueryValidatorUnitTest_QueryType()
        {
            try
            {
                string query = "SELECT TOP (1000) * FROM [DimProduct];";
                InitTable();
                Assert.IsTrue(Validator.ValidateQuery(query));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Destroy();
            }
        }

        /// <summary>
        /// Test case to test non-query type.
        /// </summary>
        [TestMethod]
        public void MsSqlQueryValidatorUnitTest_NonQueryType()
        {
            try
            {
                string query = "INSERT INTO DimProduct (ProductKey,EnglishProductName,SpanishProductName,FrenchProductName,FinishedGoodsFlag,Color) VALUES(1,'test','test','test',0,'Silver');DELETE FROM DimProduct WHERE EnglishProductName = 'test';";
                InitTable();
                Assert.IsTrue(Validator.ValidateQuery(query, true));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Destroy();
            }
        }
    }
}
