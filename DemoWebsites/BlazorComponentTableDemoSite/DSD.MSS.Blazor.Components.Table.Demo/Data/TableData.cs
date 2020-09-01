using DSD.MSS.Blazor.Components.Table.Demo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.Table.Demo.Data
{
    public class TableData
    {
        public static List<TableModel> GetData()
        {
            List<TableModel> list = new List<TableModel>();
            for (int i = 0; i < 150; i++)
            {
                list.Add(new TableModel()
                {
                    Id = i.ToString(),
                    FirstName = "Mark",
                    LastName = "Otto",
                    Handle = "@mdo" + i.ToString()
                });
            }
            
            return list;
        }
    }
}
