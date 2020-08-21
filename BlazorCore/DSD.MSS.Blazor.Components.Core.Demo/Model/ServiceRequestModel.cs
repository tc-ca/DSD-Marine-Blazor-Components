using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Demo.Model
{
    public class ServiceRequestModel
    {
        public string RequestId{ get; set; }
        public string RegistryType { get; set; }
        public string RequestType { get; set; }
        public string RequestorName { get; set; }
        public string RequestStatus { get; set; }
        public string AssigneeEmployeeName { get; set; }
        public string TimeBeforeDeadline { get; set; }
    }
}
