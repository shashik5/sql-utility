using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore;

namespace SqlUtilityUnitTestProject.MSSQL
{
    [TestClass]
    public class MsSqlUtilityUnitTest: QueryBuilderUnitTest.Base.QueryBuilderUnitTestBase
    {
        [TestMethod]
        public void MsSqlUtilityUnitTest_SelectFromSingleTable()
        {
            using (MsSqlUtility utility = new MsSqlUtility())
            {
                try
                {
                    utility.AddTableSchema(@"..\..\Data\DimProduct.xml");
                    utility.GenerateQuery("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\"},{\"ColumnName\":\"ProductAlternateKey\",\"Alias\":\"Product Alternate Key\"},{\"ColumnName\":\"SafetyStockLevel\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\"}],\"TableClause\":[]}],\"QueryClause\":[]}");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [TestMethod]
        public void MsSqlUtilityUnitTest_SelectFromMultiTable()
        {
            using (MsSqlUtility utility = new MsSqlUtility())
            {
                try
                {
                    utility.AddTableSchema(@"..\..\Data\DimProduct.xml");
                    utility.AddTableSchema(@"..\..\Data\DimProductSubcategory.xml");
                    utility.GenerateQuery("{\"QueryType\":\"Select\",\"QueryName\":\"SimpleSelectQueryWithSingleTable\",\"QueryData\":[{\"TableName\":\"DimProduct\",\"ColumnList\":[{\"ColumnName\":\"ProductKey\",\"Alias\":\"Product Key\",\"BelongsToTable\":\"DimProduct\"},{\"ColumnName\":\"EnglishProductName\",\"Alias\":\"Product Name\",\"BelongsToTable\":\"DimProduct\"},{\"ColumnName\":\"EnglishProductSubcategoryName\",\"Alias\":\"Product Subcategory Name\",\"BelongsToTable\":\"DimProductSubcategory\"}],\"TableClause\":[{\"ClauseType\":\"JOIN\",\"ClauseValue\":\"DimProduct.ProductSubcategoryKey = DimProductSubcategory.ProductSubcategoryKey\",\"JoinType\":\"Inner\",\"JoinTableName\":\"DimProductSubcategory\"}]}],\"QueryClause\":[]}");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
