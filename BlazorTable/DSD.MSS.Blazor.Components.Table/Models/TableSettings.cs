using System;
using System.Collections.Generic;
using System.Text;

namespace DSD.MSS.Blazor.Components.Table.Models
{
    /// <summary>
    /// Table settings
    /// </summary>
    /// <typeparam name="TableItem"></typeparam>
    public class TableSettings<TableItem>
    {
        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Page Number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Global Search
        /// </summary>
        public string GlobalSearch { get; set; }

        /// <summary>
        /// Columns
        /// </summary>
        public List<IColumn<TableItem>> Columns { get; } = new List<IColumn<TableItem>>();
    }
}
