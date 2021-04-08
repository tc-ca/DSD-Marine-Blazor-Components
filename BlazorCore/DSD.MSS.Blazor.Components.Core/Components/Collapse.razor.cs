using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSD.MSS.Blazor.Components.Core.Components
{
    public partial class Collapse
    {
        [Parameter] public string Icon { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string HeaderBackgroundColor { get; set; }
        [Parameter] public string HeaderTitleTextColor { get; set; }
        [Parameter] public bool ShowBorder { get; set; }
        [Parameter] public RenderFragment BodyContent { get; set; }
    }
}
