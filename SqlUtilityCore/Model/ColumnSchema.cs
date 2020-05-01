namespace SqlUtilityCore.Model
{
    /// <summary>
    /// Column data class.
    /// </summary>
    public class ColumnSchema
    {
        /// <summary>
        /// Column name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Data type of the column.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Max length for the column.
        /// </summary>
        public string MaxLength { get; set; } = string.Empty;

        /// <summary>
        /// Set true to allow null.
        /// </summary>
        public bool AllowNull { get; set; } = true;
    }
}
