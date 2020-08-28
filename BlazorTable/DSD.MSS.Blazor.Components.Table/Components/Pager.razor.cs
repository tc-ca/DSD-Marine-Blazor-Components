using Microsoft.AspNetCore.Components;

namespace DSD.MSS.Blazor.Components.Table
{
    /// <summary>
    /// DSD.MSS.Blazor.Components.Table Pager
    /// </summary>
    public partial class Pager
    {
        [CascadingParameter(Name = "Table")]
        public ITable Table { get; set; }

        /// <summary>
        /// Always show Pager, otherwise only show if TotalPages > 1
        /// </summary>
        [Parameter]
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// Show current page number
        /// </summary>
        [Parameter]
        public bool ShowPageNumber { get; set; }

        /// <summary>
        /// Show total item count
        /// </summary>
        [Parameter]
        public bool ShowTotalCount { get; set; }

        /// <summary>
        /// Update page size
        /// </summary>
        /// <param name="e"></param>
        protected void UpdatePageSize(ChangeEventArgs e)
        {
            Table.PageSize = int.Parse(e.Value.ToString());
            Table.PageNumber = 0;
            Table.Update();
        }

        /// <summary>
        /// Page max count
        /// </summary>
        /// <returns></returns>
        protected int PageMaxCount()
        {
            var count = Table.PageSize * (Table.PageNumber + 1);
            count = count > Table.TotalCount ? Table.TotalCount : count;
            return count;
        }
    }
}
