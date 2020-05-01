using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlUtilityCore.SchemaParser;
using SqlUtilityCore.Model;
using System.Xml;
using System.Linq;
using System.IO;

namespace SqlUtilityUnitTestProject.MSSQL
{
    [TestClass]
    public class MsSqlSchemaParserUnitTest
    {
        /* Test cases using DimProduct.xml */

        /// <summary>
        /// To test table name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_TableName_1()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"..\..\Data\DimProduct.xml");
            TableSchema table = MsSqlSchemaParser.Parse(xmlDoc);

            Assert.AreEqual(table.Name, "AdventureWorksDW2012.dbo.DimProduct");
        }

        /// <summary>
        /// To test first column name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnName_1()
        {
            string xmlContent = File.ReadAllText(@"..\..\Data\DimProduct.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);
            TableSchema table = MsSqlSchemaParser.Parse(xmlDoc);

            Assert.AreEqual(table.Columns.First().Name, "ProductKey");
        }

        /// <summary>
        /// To test last cloumn name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnName_1()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

            Assert.AreEqual(table.Columns.Last().Name, "Status");
        }

        /// <summary>
        /// To test AllowNull property in first column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnIsAllowNull_1()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

            Assert.AreEqual(table.Columns.First().AllowNull, false);
        }

        /// <summary>
        /// To test data type of first column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnDataType_1()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

            Assert.AreEqual(table.Columns.First().DataType, "INT");
        }

        /// <summary>
        /// To test AllowNull property in last column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnIsAllowNull_1()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

            Assert.AreEqual(table.Columns.Last().AllowNull, true);
        }

        /// <summary>
        /// To test data type of last column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnDataType_1()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

            Assert.AreEqual(table.Columns.Last().DataType, "NVARCHAR");
        }

        /* Test cases using DimProductCategory.xml */

        /// <summary>
        /// To test table name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_TableName_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Name, "AdventureWorksDW2012.dbo.DimProductCategory");
        }

        /// <summary>
        /// To test first column name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnName_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Columns.First().Name, "ProductCategoryKey");
        }

        /// <summary>
        /// To test last cloumn name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnName_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Columns.Last().Name, "FrenchProductCategoryName");
        }

        /// <summary>
        /// To test AllowNull property in first column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnIsAllowNull_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Columns.First().AllowNull, false);
        }

        /// <summary>
        /// To test data type of first column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnDataType_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Columns.First().DataType, "INT");
        }

        /// <summary>
        /// To test AllowNull property in last column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnIsAllowNull_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Columns.Last().AllowNull, false);
        }

        /// <summary>
        /// To test data type of last column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnDataType_2()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductCategory.xml");

            Assert.AreEqual(table.Columns.Last().DataType, "NVARCHAR");
        }

        /* Test cases using DimProductSubcategory.xml */

        /// <summary>
        /// To test table name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_TableName_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Name, "AdventureWorksDW2012.dbo.DimProductSubcategory");
        }

        /// <summary>
        /// To test first column name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnName_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Columns.First().Name, "ProductSubcategoryKey");
        }

        /// <summary>
        /// To test last cloumn name.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnName_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Columns.Last().Name, "ProductCategoryKey");
        }

        /// <summary>
        /// To test AllowNull property in first column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnIsAllowNull_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Columns.First().AllowNull, false);
        }

        /// <summary>
        /// To test data type of first column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_FirstColumnDataType_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Columns.First().DataType, "INT");
        }

        /// <summary>
        /// To test AllowNull property in last column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnIsAllowNull_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Columns.Last().AllowNull, true);
        }

        /// <summary>
        /// To test data type of last column.
        /// </summary>
        [TestMethod]
        public void TestXmlParsing_LastColumnDataType_3()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProductSubcategory.xml");

            Assert.AreEqual(table.Columns.Last().DataType, "INT");
        }

        /* Test cases to test constructors in MsSqlSchemaParser class */

        /// <summary>
        /// To test table name by passing XML file path as constructor argument.
        /// </summary>
        [TestMethod]
        public void TestMsSqlSchemaParserConstructor_AsFilePath()
        {
            TableSchema table = MsSqlSchemaParser.Parse(@"..\..\Data\DimProduct.xml");

            Assert.AreEqual(table.Name, "AdventureWorksDW2012.dbo.DimProduct");
        }

        /// <summary>
        /// To test table name by passing XML string path as constructor argument.
        /// </summary>
        [TestMethod]
        public void TestMsSqlSchemaParserConstructor_AsXmlString()
        {
            string xmlString = File.ReadAllText(@"..\..\Data\DimProduct.xml");
            TableSchema table = MsSqlSchemaParser.Parse(xmlString);

            Assert.AreEqual(table.Name, "AdventureWorksDW2012.dbo.DimProduct");
        }
    }
}
