
using DSD.MSS.Blazor.Components.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Models
{
    public class StatusHistory
    {
        public string Id { get; set; }

       public string StatusText { get; set; }

        public string ProcessingPhase { get; set; }

        public RequestStatus RequestStatus { get; set; }

        public DateTimeOffset? RequestStatusTime { get; set; }

        public string AssignedTo { get; set; }
    }
}
