using System.Collections.Generic;

namespace SqlUtilityCore.Model
{
    /// <summary>
    /// Table schema model class.
    /// </summary>
    public class TableSchema
    {
        /// <summary>
        /// Table name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Column data list.
        /// </summary>
        public List<ColumnSchema> Columns = new List<ColumnSchema>();
    }
}
