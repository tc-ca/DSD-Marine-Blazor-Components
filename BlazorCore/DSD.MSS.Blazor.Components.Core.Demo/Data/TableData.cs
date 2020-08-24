using DSD.MSS.Blazor.Components.Core.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Core.Demo.Data
{
    public class TableData
    {
        public static List<TableModel> GetData()
        {
            List<TableModel> list = new List<TableModel>();

            list.Add(new TableModel()
            {
                Id = "1",
                FirstName = "Mark",
                LastName = "Otto",
                Handle = "@mdo"
            });
            list.Add(new TableModel()
            {
                Id = "2",
                FirstName = "Jacob",
                LastName = "Thornton",
                Handle = "@fat"
            });
            list.Add(new TableModel()
            {
                Id = "3",
                FirstName = "Larry",
                LastName = "the Bird",
                Handle = "@twitter"
            });
            return list;
        }
    }
}
