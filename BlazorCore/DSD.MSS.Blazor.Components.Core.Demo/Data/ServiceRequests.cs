using DSD.MSS.Blazor.Components.Core.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Demo.Data
{
    public class ServiceRequests
    {
        public static List<ServiceRequestModel> GetServiceRequests()
        {
            List<ServiceRequestModel> serviceRequests = new List<ServiceRequestModel>();
            serviceRequests.Add(new ServiceRequestModel()
            {
                RequestId = "0000000001",
                RegistryType = "Small Vessel",
                RequestType = "First Registry",
                AssigneeEmployeeName = "Kajal Chaudhari",
                RequestStatus = "In Progress",
                RequestorName = "Bob Ferguson",
                TimeBeforeDeadline = "30 Days"
            });
            serviceRequests.Add(new ServiceRequestModel()
            {
                RequestId = "0000000002",
                RegistryType = "Large Vessel",
                RequestType = "Re-registry",
                AssigneeEmployeeName = "Kajal Chaudhari",
                RequestStatus = "In Progress",
                RequestorName = "Peter Williams",
                TimeBeforeDeadline = "2 days overdue"
            });
            serviceRequests.Add(new ServiceRequestModel()
            {
                RequestId = "0000000003",
                RegistryType = "Large Vessel",
                RequestType = "First Registry",
                AssigneeEmployeeName = "Kajal Chaudhari",
                RequestStatus = "Close",
                RequestorName = "Nancy Smith",
                TimeBeforeDeadline = "2 days"
            });
            return serviceRequests;
        }
    }
}
