using Blazorade.Bootstrap.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace DSD.MSS.Blazor.Components.Table
{
    /// <summary>
    /// Table Column
    /// </summary>
    /// <typeparam name="TableItem"></typeparam>
    public partial class TableHeaderFilter<TableItem> : ComponentBase
    {
        /// <summary>
        /// Parent Table
        /// </summary>
        [Parameter]
        public ITable<TableItem> TableRef { get; set; }

        /// <summary>
        /// Global Search
        /// </summary>
        [Parameter]
        public string GlobalSearch { get; set; }

        /// <summary>
        /// Header filter changed
        /// </summary>
        [Parameter]
        public Action HeaderFilterChanged { get; set; }

        /// <summary>
        /// Shows Search Bar above the table
        /// </summary>
        [Parameter]
        public bool ShowSearchBar { get; set; }

        /// <summary>
        /// Configure Modal
        /// </summary>
        protected Modal ConfigureModal { get; set; }

        /// <summary>
        /// Configure Edit Context
        /// </summary>
        protected EditContext ConfigureContext { get; set; }

        /// <summary>
        /// Cancel click handler
        /// </summary>
        protected void OnCancelClicked()
        {
            foreach (var column in TableRef.Columns)
            {
                column.ColumnFilterSelectedItems.Clear();
            }
            TableRef.ShowSearchBar = false;
            HeaderFilterChanged();
            StateHasChanged();
        }

        /// <summary>
        /// Configure Column checkbox event handler
        /// </summary>
        protected void OnConfigureChecked(IColumn<TableItem> column, object value)
        {
            if (column != null)
            {
                column.ShowColumn = (bool)value;
                column.ShowHeaderRowFilterable = (column.DefaultShowHeaderFilter != null && column.DefaultShowHeaderFilter != true) ? false : column.ShowColumn;
                TableRef.Update();
                StateHasChanged();
            }
        }

        /// <summary>
        /// Configure Column click event handler
        /// </summary>
        protected void HandleConfigureClick()
        {
            ConfigureModal.Show();
        }

        /// <summary>
        /// Checkbox click event handler
        /// </summary>
        /// <param name="column"></param>
        /// <param name="itemName"></param>
        /// <param name="value"></param>
        protected void OnCheckboxClicked(IColumn<TableItem> column, string itemName, object value)
        {
            if (column == null)
            {
                return;
            }

            if ((bool)value)
            {
                //Add filter
                if (!column.ColumnFilterSelectedItems.Contains(itemName))
                {
                    column.ColumnFilterSelectedItems.Add(itemName);
                }
            }
            else
            {
                //Remove filter
                if (column.ColumnFilterSelectedItems.Contains(itemName))
                {
                    column.ColumnFilterSelectedItems.Remove(itemName);
                }
            }
            column.UpdateColumnFilter();
        }


    }
}