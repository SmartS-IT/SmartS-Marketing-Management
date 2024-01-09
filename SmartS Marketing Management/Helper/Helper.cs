using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SmartS_Marketing_Management.Helper
{
    public struct ReturnParams
    {
        public bool Status { get; set; }
        public string ErrorString { get; set; }
        public object Data { get; set; }
    }
    public static class Helper
    { 
        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row => {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex) { }
                    }
                }
                return objT;
            }).ToList();
        }

        public static ReturnParams ConvertToJsonString(bool status, string errorString, object data)
        {
            var myData = new ReturnParams()
            {
                Status = status,
                ErrorString = errorString,
                Data = data,
            };

            return myData;
        }
    }
}