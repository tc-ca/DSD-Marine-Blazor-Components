
using DSD.MSS.Blazor.Components.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Models
{
    /// <summary>
    /// Model used for populating information in RequestHistory Component
    /// </summary>
    public class StatusHistoryItem
    {
        public string Id { get; set; }

       public string StatusText { get; set; }

        public string ProcessingPhase { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public DateTime? RequestStatusTime { get; set; }
        public string ChangedBy { get; set; }
    }
}
