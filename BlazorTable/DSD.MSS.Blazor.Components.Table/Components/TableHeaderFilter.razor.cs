using Microsoft.AspNetCore.Components;
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
            UpdateColumnFilter(column);
        }

        private void UpdateColumnFilter(IColumn<TableItem> column)
        {
            if (column.ColumnFilterSelectedItems.Any())
            {
                Expression<Func<TableItem, bool>> expression = GetStringFilter(column, column.ColumnFilterSelectedItems.First());
                Expression body = expression.Body;
                foreach (var itemName in column.ColumnFilterSelectedItems.Skip(1))
                {
                    expression = GetStringFilter(column, itemName);
                    body = Expression.Or(body, expression.Body);
                }
                column.Filter = Expression.Lambda<Func<TableItem, bool>>(body, column.Field.Parameters);
                column.Table.Update();
                column.Table.FirstPage();
            }
            else
            {
                column.Filter = null;
                column.Table.Update();
            }
        }

        /// <summary>
        /// Get string filter
        /// </summary>
        /// <param name="column"></param>
        /// <param name="FilterText"></param>
        /// <returns></returns>
        private Expression<Func<TableItem, bool>> GetStringFilter(IColumn<TableItem> column, string FilterText)
        {
            return Expression.Lambda<Func<TableItem, bool>>(
                       Expression.AndAlso(
                           Expression.NotEqual(column.Field.Body, Expression.Constant(null)),
                           Expression.Call(
                               column.Field.Body,
                               typeof(string).GetMethod(nameof(string.Equals), new[] { typeof(string), typeof(StringComparison) }),
                               new[] { Expression.Constant(FilterText), Expression.Constant(StringComparison.OrdinalIgnoreCase) })),
                       column.Field.Parameters);
        }
    }
}