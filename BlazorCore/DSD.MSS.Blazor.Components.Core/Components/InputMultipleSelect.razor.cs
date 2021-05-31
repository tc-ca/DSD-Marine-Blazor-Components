using DSD.MSS.Blazor.Components.Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
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
        /// Specifies the Field ID
        /// </summary>
        [Parameter] public string Id { get; set; }

        /// <summary>
        /// Checkbox list
        /// </summary>
        [Parameter]
        public List<SelectListItem> CheckBoxList { get; set; }
    }
}
