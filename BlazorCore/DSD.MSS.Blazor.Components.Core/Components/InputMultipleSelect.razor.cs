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
        public bool UseSelectionAsSelectTitle { get; set; } = false;
        
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
        
        /// <summary>
        /// Gets all selected items 
        /// </summary>
        public List<SelectListItem> SelectedItems => this.CheckBoxList.Where(x => x.Value).ToList();

        /// <summary>
        /// Gets all text from selected items, comma seperated
        /// </summary>
        public string SelectedText => string.Join(',', this.SelectedItems.Select(x => x.Text));
    }
}
