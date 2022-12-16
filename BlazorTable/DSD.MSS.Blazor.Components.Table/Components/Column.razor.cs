using DSD.MSS.Blazor.Components.Table.Models;
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
    public partial class Column<TableItem> : IColumn<TableItem>
    {
        /// <summary>
        /// Parent Table
        /// </summary>
        [CascadingParameter(Name = "Table")]
        public ITable<TableItem> Table { get; set; }

        private string _title;

        /// <summary>
        /// Title (Optional, will use Field Name if null)
        /// </summary>
        [Parameter]
        public string Title
        {
            get { return _title ?? Field.GetPropertyMemberInfo()?.Name; }
            set { _title = value; }
        }

        /// <summary>
        /// Width auto|value|initial|inherit
        /// </summary>
        [Parameter]
        public string Width { get; set; }

        /// <summary>
        /// Column can be sorted
        /// </summary>
        [Parameter]
        public bool Sortable { get; set; }

        /// <summary>
        /// Column can be filtered
        /// </summary>
        [Parameter]
        public bool Filterable { get; set; }

        /// <summary>
        /// Column can be filtered in header row
        /// </summary>
        public bool ShowHeaderRowFilterable { get; set; }

        /// <inheritdoc/>
        [Parameter]
        public SortOrder FilterSort { get; set; } = SortOrder.NONE;

        /// <summary>
        /// Show header filter by default
        /// </summary>
        [Parameter]
        public bool? DefaultShowHeaderFilter { get; set; }

        /// <summary>
        /// Normal Item Template
        /// </summary>
        [Parameter]
        public RenderFragment<TableItem> Template { get; set; }

        /// <summary>
        /// Edit Mode Item Template
        /// </summary>
        [Parameter]
        public RenderFragment<TableItem> EditTemplate { get; set; }

        /// <summary>
        /// Place custom controls which implement IFilter
        /// </summary>
        [Parameter]
        public RenderFragment<IColumn<TableItem>> CustomIFilters { get; set; }

        /// <summary>
        /// Field which this column is for<br />
        /// Required when Sortable = true<br />
        /// Required when Filterable = true
        /// </summary>
        [Parameter]
        public Expression<Func<TableItem, object>> Field { get; set; }

        /// <inheritdoc/>
        [Parameter]
        public Expression<Func<TableItem, object>> SortFieldValue { get; set; }

        /// <inheritdoc/>
        [Parameter]
        public Expression<Func<TableItem, object>> FilterFieldValue
        {
            get { return this.filterFieldValue ?? this.Field; }
            set { this.filterFieldValue = value; }
        }

        /// <summary>
        /// Horizontal alignment
        /// </summary>
        [Parameter]
        public Align Align { get; set; }

        /// <summary>
        /// Set the format for values if no template
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// Column CSS Class
        /// </summary>
        [Parameter]
        public string Class { get; set; }

        /// <summary>
        /// Show Column by default
        /// </summary>
        [Parameter]
        public bool? DefaultShowColumn { get; set; }

        /// <summary>
        /// Show Column
        /// </summary>
        public bool ShowColumn { get; set; } = true;

        /// <summary>
        /// Filter expression
        /// </summary>
        public Expression<Func<TableItem, bool>> Filter { get; set; }

        /// <summary>
        /// True if this is the default Sort Column
        /// </summary>
        [Parameter]
        public bool? DefaultSortColumn { get; set; }

        /// <summary>
        /// Direction of default sorting
        /// </summary>
        [Parameter]
        public bool? DefaultSortDescending { get; set; }

        /// <summary>
        /// True if this is the current Sort Column
        /// </summary>
        public bool SortColumn { get; set; }

        /// <summary>
        /// Direction of sorting
        /// </summary>
        public bool SortDescending { get; set; }

        /// <summary>
        /// Filter Panel is open
        /// </summary>
        public bool FilterOpen { get; private set; }

        /// <summary>
        /// Column Filter Items
        /// </summary>
        public List<TableFilter> ColumnFilterItems { get; set; }

        [Parameter]
        public List<TableFilter> CustomFilterList { get; set; }

        public List<TableFilter> SetFilters { get; set; } = new List<TableFilter>();

        /// <summary>
        /// Column Filter Selected Items
        /// </summary>
        public List<string> ColumnFilterSelectedItems { get; set; }

        /// <summary>
        /// Column Data Type
        /// </summary>
        [Parameter]
        public Type Type { get; set; }

        /// <summary>
        /// Filter Icon Element
        /// </summary>
        public ElementReference FilterRef { get; set; }

        /// <summary>
        /// Currently applied Filter Control
        /// </summary>
        public IFilter<TableItem> FilterControl { get; set; }

        private Expression<Func<TableItem, object>> filterFieldValue;

        /// <summary>
        /// On Initialized
        /// </summary>
        protected override void OnInitialized()
        {
            Table.AddColumn(this);
            ColumnFilterItems = new List<TableFilter>();
            ColumnFilterSelectedItems = new List<string>();
            CustomFilterList = new List<TableFilter>();

            if (DefaultShowColumn.HasValue)
            {
                this.ShowColumn = DefaultShowColumn.Value;
            }

            if(DefaultShowHeaderFilter.HasValue)
            {
                this.ShowHeaderRowFilterable = DefaultShowHeaderFilter.Value;
            }

            if (DefaultSortDescending.HasValue)
            {
                this.SortDescending = DefaultSortDescending.Value;
            }

            if (DefaultSortColumn.HasValue)
            {
                this.SortColumn = DefaultSortColumn.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateColumnFilter()
        {
            if (ColumnFilterSelectedItems.Any())
            {
                Expression<Func<TableItem, bool>> expression = GetStringFilter(this, ColumnFilterSelectedItems.First());
                Expression body = expression.Body;
                foreach (var itemName in ColumnFilterSelectedItems.Skip(1))
                {
                    expression = GetStringFilter(this, itemName);
                    body = Expression.Or(body, expression.Body);
                }
                Filter = Expression.Lambda<Func<TableItem, bool>>(body, Field.Parameters);
                Table.Update();
                Table.FirstPage();
            }
            else if (SetFilters.Any()){
            // Expression<Func<TableItem, bool>> expression = GetStringFilter(this, CustomFilterList.Where(x => x.DisplayValueID == SetFilters.First().DisplayValueID).Select(x => x.DisplayValue).First());
            Expression<Func<TableItem, bool>> expression = GetStringFilter(this, SetFilters.First().DisplayValue);

            Expression body = expression.Body;
            foreach (var itemName in SetFilters.Skip(1))
            {
              // var stringval = CustomFilterList.Where(x => x.DisplayValueID == itemName.DisplayValueID).Select(x => x.DisplayValue).First();
               expression = GetStringFilter(this, itemName.DisplayValue);
               body = Expression.Or(body, expression.Body);
            }
            Filter = Expression.Lambda<Func<TableItem, bool>>(body, Field.Parameters);
            Table.Update();
            Table.FirstPage();
         }
            else
            {
                Filter = null;
                Table.Update();
            }
        }

        /// <summary>
        /// Gets the value to filter against the encapsulating column
        /// </summary>
        /// <param name="column">The column</param>
        /// <param name="FilterText">The value to filter against</param>
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

        protected override void OnParametersSet()
        {
            if ((Sortable && Field == null) || (Filterable && Field == null))
            {
                throw new InvalidOperationException($"Column {Title} Property parameter is null");
            }

            if (Title == null && Field == null)
            {
                throw new InvalidOperationException("A Column has both Title and Property parameters null");
            }

            if (Type == null)
            {
                Type = Field?.GetPropertyMemberInfo().GetMemberUnderlyingType();
            }
        }

        /// <summary>
        /// Opens/Closes the Filter Panel
        /// </summary>
        public void ToggleFilter()
        {
            FilterOpen = !FilterOpen;
            Table.Refresh();
        }

        /// <summary>
        /// Sort by this column
        /// </summary>
        public void SortBy()
        {
            if (Sortable)
            {
                if (SortColumn)
                {
                    SortDescending = !SortDescending;
                }

                Table.Columns.ForEach(x => x.SortColumn = false);

                SortColumn = true;

                Table.Update();
            }
        }

        public object GenerateFilterSortValue(TableItem item)
        {
            if(this.filterValueCompiled == null)
            {
                this.filterValueCompiled = this.FilterFieldValue.Compile();
            }

            return this.filterValueCompiled.Invoke(item);
        }

        private Func<TableItem, object> filterValueCompiled;

        /// <summary>
        /// Render a default value if no template
        /// </summary>
        /// <param name="data">data row</param>
        /// <returns></returns>
        public string Render(TableItem data)
        {
            if (data == null || Field == null) return string.Empty;

            if (renderCompiled == null)
                renderCompiled = Field.Compile();

            var value = renderCompiled.Invoke(data);

            if (value == null) return string.Empty;

            if (string.IsNullOrEmpty(Format))
                return value.ToString();

            return string.Format(CultureInfo.CurrentCulture, $"{{0:{Format}}}", value);
        }

        /// <summary>
        /// Save compiled renderCompiled property to avoid repeated Compile() calls
        /// </summary>
        private Func<TableItem, object> renderCompiled;
    }
}