using Blazorade.Bootstrap.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;

namespace DSD.MSS.Blazor.Components.Table
{
    /// <summary>
    /// Table Column
    /// </summary>
    /// <typeparam name="TableItem"></typeparam>
    public partial class TableHeaderFilter<TableItem> : ComponentBase
    {

        private Timer typingTimer;

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
        /// Serach Typeing Delay
        /// </summary>
        [Parameter]
        public int SearchTypingDelay { get; set; } = 500;

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
        /// Shows the clear filters button on top.
        /// </summary>
        [Parameter]
        public bool ShowClearFilterOnTop { get; set; }

        /// <summary>
        /// Hide/Show the filter button.
        /// </summary>
        [Parameter]
        public bool HideFilterButton { get; set; }

        /// <summary>
        /// Hide/Show the config button.
        /// </summary>
        [Parameter]
        public bool HideConfigButton { get; set; }

        /// <summary>
        /// Configure Modal
        /// </summary>
        protected Modal ConfigureModal { get; set; }

        /// <summary>
        /// Configure Edit Context
        /// </summary>
        protected EditContext ConfigureContext { get; set; }

        /// <summary>
        /// OnInitialized
        /// </summary>
        protected override void OnInitialized()
        {
            typingTimer = new Timer(SearchTypingDelay);
            typingTimer.Elapsed += OnUserFinishTypingSearch;
            typingTimer.AutoReset = false;
            base.OnInitialized();
        }

        /// <summary>
        /// OnUserFinishTypingSearch
        /// <param name="source"></param>
        /// <param name="e"></param>
        /// </summary>
        private void OnUserFinishTypingSearch(Object source, ElapsedEventArgs e)
        {
            InvokeAsync(() =>
            {
                TableRef.Update();
                StateHasChanged();
            });
        }

        /// <summary>
        /// HandleSearchInputKeyUp
        /// <param name="e"></param>
        /// </summary>
        void HandleSearchInputKeyUp(KeyboardEventArgs e)
        {
            // remove previous one
            typingTimer.Stop();

            // new timer
            typingTimer.Start();
        }

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
            TableRef.PageNumber = 0;
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