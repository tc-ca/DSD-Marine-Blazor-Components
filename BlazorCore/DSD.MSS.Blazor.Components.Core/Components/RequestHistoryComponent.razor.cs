namespace DSD.MSS.Blazor.Components.Core.Components
{
    using DSD.MSS.Blazor.Components.Core.Models;
    using DSD.MSS.Blazor.Components.Core.Constants;
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public partial class RequestHistoryComponent
    {
        [Parameter]
        public List<StatusHistory> StatusHistories { get; set; }

        public string SetListCSS(int index)
        {
            switch (index)
            {
                case 0: return "first";
                default: return "not-first";
            }
        }
    }
}
