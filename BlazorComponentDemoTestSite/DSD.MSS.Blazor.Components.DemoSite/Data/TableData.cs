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
            for (int i = 0; i < 75; i = i + 2)
            {
                list.Add(new TableModel()
                {
                    Id = i.ToString(),
                    FirstName = "Mark",
                    LastName = "Otto",
                    Handle = "@mdo" + i.ToString()
                });
                list.Add(new TableModel()
                {
                    Id = (i + 1).ToString(),
                    FirstName = "Jacob",
                    LastName = "Thornton",
                    Handle = "@mdo" + i.ToString()
                });
            }
            return list;
        }
    }
}
