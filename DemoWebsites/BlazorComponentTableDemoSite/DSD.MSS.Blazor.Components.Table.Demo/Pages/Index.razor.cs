﻿namespace DSD.MSS.Blazor.Components.Table.Demo.Pages
{
    using DSD.MSS.Blazor.Components.Table.Demo.Data;
    using DSD.MSS.Blazor.Components.Table.Demo.Model;
    using DSD.MSS.Blazor.Components.Table.Models;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class Index
    {
        [Inject]
        public IMemoryCache MemoryCache { get; set; }

        public List<TableModel> TableData = new List<TableModel>();

        protected bool LastNameFilter { get; set; } = true;

        protected bool FirstNameFilter { get; set; } = true;

        protected bool ShowFilterHeader { get; set; } = true;

        protected TableSettings<TableModel> tableSettings { get; set; }

        protected Table<TableModel> TableRef { get; set; }


        public List<TableFilter> TableFilters { get; set; }

        private const string tableSettingKey = "TableSettings";

        public void OnFilterChanged(TableSettings<TableModel> settings)
        {

            
        var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(2)
            };

            if (TableRef != null)
            {
                this.MemoryCache.Set("TableSettings", settings);
            }
        }

        private void ToggleLastNameFilter()
        {
            LastNameFilter = !LastNameFilter;
            TableRef.Update();
        }

        private void ToggleFirstNameFilter()
        {
            FirstNameFilter = !FirstNameFilter;
            TableRef.Update();
        }

        protected override async Task OnInitializedAsync()
        {
         this.TableFilters = new List<TableFilter>
         {
            new TableFilter() { DisplayValue = "Mark 0", DisplayValueID = 0 },
            new TableFilter() { DisplayValue = "MOQ2", DisplayValueID = 1 },
            new TableFilter() { DisplayValue = "MOQ3", DisplayValueID = 2 },
            new TableFilter() { DisplayValue = "MOQ4", DisplayValueID = 3 }
         };


         //await Task.Delay(TimeSpan.FromSeconds(5));
         this.TableData = TableDataLoader.GetData();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (this.MemoryCache.TryGetValue("TableSettings", out TableSettings<TableModel> settings))
                {
                    tableSettings = settings;
                    TableRef.Update();
                }
            }
        }

        protected void OnAfterTableDataLoaded()
        {
            if (tableSettings != null)
            {
                TableRef.ResetTableSettings(tableSettings);
                TableRef.UpdatePage(tableSettings.PageNumber);
                tableSettings = null;
            }
        }

        protected void HandleHeaderFilterChanged()
        {
            StateHasChanged();
        }
        protected void HandleFilterButtonClick()
        {
            ShowFilterHeader = !ShowFilterHeader;
        }
    }
}
