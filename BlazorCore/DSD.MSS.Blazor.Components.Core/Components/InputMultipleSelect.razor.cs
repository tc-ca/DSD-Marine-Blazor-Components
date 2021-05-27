using DSD.MSS.Blazor.Components.Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSD.MSS.Blazor.Components.Core.Components
{
    public partial class InputMultipleSelect
    {
        /// <summary>
        /// Title for multiple selection
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title for multiple selection
        /// </summary>
        [Parameter]
        public string SelectTitle { get; set; }

        /// <summary>
        /// Flag that indicates to use selected items as the SelectTitle
        /// </summary> 
        [Parameter]
        public bool UseSelectedItemsAsSelectTitle { get; set; } = false;
        
        /// <summary>
        /// Specifies the Field ID
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Checkbox list
        /// </summary>
        [Parameter]
        public List<SelectListItem> CheckBoxList { get; set; }

        [Parameter]
        public RenderFragment<string> SelectedItemTemplate { get; set; }
        
        /// <summary>
        /// Gets all selected items 
        /// </summary>
        private List<SelectListItem> SelectedItems => this.CheckBoxList.Where(x => x.Value).ToList();

        /// <summary>
        /// Flag to determine whether to show the selected items as the select title for the input multiple select
        /// </summary>
        private bool ShowSelectedItemsAsSelectTitle => this.UseSelectedItemsAsSelectTitle && this.SelectedItems.Count > 0;
    }
}
