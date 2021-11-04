namespace DSD.MSS.Blazor.Components.Table.Models
{
    /// <summary>
    /// Defines a table filter.
    /// </summary>
    public class TableFilter
    {
        /// <summary>
        /// Gets or sets the display value for the filter
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// Gets or sets the value to sort the filter by
        /// </summary>
        public object SortValue { get; set; }
    }
}
