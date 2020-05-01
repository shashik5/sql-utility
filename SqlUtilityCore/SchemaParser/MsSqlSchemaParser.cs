using System.Xml;
using SqlUtilityCore.Model;

namespace SqlUtilityCore.SchemaParser
{
    /// <summary>
    /// Class to parse Microsoft SQL server schema.
    /// </summary>
    public static class MsSqlSchemaParser
    {
        /// <summary>
        /// Method which accepts XML string or file path as argument to parse into table schema/data.
        /// </summary>
        /// <param name="pathOrXml">XML string or file path</param>
        public static TableSchema Parse(string pathOrXml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                try
                {
                    /* Try loading pathOrXml as XML string. */
                    xmlDoc.LoadXml(pathOrXml);
                }
                catch (System.Exception)
                {
                    /* Try loading pathOrXml as XML file path. */
                    xmlDoc.Load(pathOrXml);
                }

                /* Begin parsing. */
                return BeginParse(xmlDoc);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Method which accepts XML schema document to parse into table schema/data.
        /// </summary>
        /// <param name="xmlDoc">XML schema document to be parsed</param>
        /// <returns></returns>
        public static TableSchema Parse(XmlDocument xmlDoc)
        {
            return BeginParse(xmlDoc);
        }

        /// <summary>
        /// Method which begins parsing xml document into table data/schema.
        /// </summary>
        /// <param name="xml">XML schema document to be parsed</param>
        private static TableSchema BeginParse(XmlDocument xml)
        {
            TableSchema table = new TableSchema();

            /* Add xml namesapce to select nodes with namespace. */
            XmlNamespaceManager nsManager = new XmlNamespaceManager(xml.NameTable);
            nsManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");

            /* Get table name. */
            table.Name = xml.SelectSingleNode("xsd:schema/xsd:element", nsManager).Attributes["name"].Value;

            /* Get and parse column nodes. */
            XmlNodeList columnNodes = xml.SelectNodes("xsd:schema/xsd:element/xsd:complexType/xsd:attribute", nsManager);
            foreach (XmlNode node in columnNodes)
            {
                table.Columns.Add(ParseColumnNode(node, nsManager));
            }

            return table;
        }

        /// <summary>
        /// Method to parse column node.
        /// </summary>
        /// <param name="node">Column node to be parsed</param>
        /// <param name="nsManager">Namespace manager to select nodes with namespace</param>
        /// <returns></returns>
        private static ColumnSchema ParseColumnNode(XmlNode node, XmlNamespaceManager nsManager)
        {
            ColumnSchema column = new ColumnSchema();

            /* Get column name. */
            column.Name = node.Attributes["name"].Value;

            /* Check if column allows null value. */
            XmlAttribute useAttribute = node.Attributes["use"];
            if (useAttribute != null)
            {
                column.AllowNull = !useAttribute.Value.Equals("required", System.StringComparison.OrdinalIgnoreCase);
            }

            /* Get datatype value from column node. */
            XmlAttribute dataTypeAttribute = node.Attributes["type"];
            XmlNode optionsNode = node.SelectSingleNode("xsd:simpleType/xsd:restriction", nsManager);
            if (dataTypeAttribute == null && optionsNode != null)
            {
                /* If datatype value not found in column node, then try in restriction node.  */
                dataTypeAttribute = optionsNode.Attributes["base"];

                /* Get max length data from maxLength node within restriction. */
                XmlNode maxLengthNode = optionsNode.SelectSingleNode("xsd:maxLength", nsManager);
                if (maxLengthNode != null)
                {
                    column.MaxLength = maxLengthNode.Attributes["value"].Value;
                }
            }

            /* Store dataType value in DataType property. */
            if (dataTypeAttribute != null)
            {
                column.DataType = dataTypeAttribute.Value.Replace("sqltypes:", "").ToUpper();
                if (string.Equals(column.DataType, "DBOBJECT", System.StringComparison.OrdinalIgnoreCase))
                {
                    column.DataType = "VARBINARY";
                    column.MaxLength = "MAX";
                }
            }

            return column;
        }
    }
}
