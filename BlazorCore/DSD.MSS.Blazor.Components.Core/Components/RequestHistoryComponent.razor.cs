using DSD.MSS.Blazor.Components.Core.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Components
{
    public partial class RequestHistoryComponent
    {
        [Parameter]
        public List<StatusHistory> StatusHistories { get; set; }
    }
}
