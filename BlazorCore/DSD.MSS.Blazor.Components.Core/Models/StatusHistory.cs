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

        public string Status { get; set; }

        public string ProcessingPhase { get; set; }

       public RequestStatus RequestStatus
        {
            get {
                switch (this.Status)
                {
                    case "New": return RequestStatus.NEW; 
                    case "In Progress": return RequestStatus.IN_PROGRESS;
                    case "Pending": return RequestStatus.PENDING;
                    case "Complete": return RequestStatus.COMPLETE;
                    case "Cancelled": return RequestStatus.CANCELLED;
                    case "Unknown": return RequestStatus.UNKNOWN;
                    default: return RequestStatus.NEW;
                }
                    
                    }
            set { }
        }

        public System.DateTimeOffset? RequestStatusTime { get; set; }

        public string AssignedTo { get; set; }
    }
}
