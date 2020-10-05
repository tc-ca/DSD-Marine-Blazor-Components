using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Lookup
{
    public static class DataTableExtensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                EnumerableRowCollection<DataRow> rows = table.AsEnumerable();
                foreach (var row in rows)
                {
                    T obj = new T();

                    PropertyInfo[] props = obj.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            object value = row[prop.Name];
                            propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                        }
                        catch(Exception)
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

    }
}
