using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Table.Demo.Model
{
    public class TableModel
    {
        public string Id{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Handle { get; set; }
        public DateTime TestingDate { get; set; }
        public string TestingDateValue => TestingDate.ToString("MMM dd, yyyy");
    }
}
