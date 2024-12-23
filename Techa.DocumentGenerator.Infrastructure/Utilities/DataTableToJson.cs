using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace Techa.DocumentGenerator.Infrastructure.Utilities
{
    public static class DataTableToJson
    {
        public static string ConvertDataSetToString(this System.Data.DataSet source)
        {
            string result = string.Empty;

            foreach (DataTable item in source.Tables)
            {
                result += JsonConvert.SerializeObject(DataTableToJson.ToJson(item));
            }

            return result;
        }

        public static JArray ToJson(this System.Data.DataTable source)
        {
            JArray result = new JArray();
            JObject row;
            foreach (System.Data.DataRow dr in source.Rows)
            {
                row = new JObject();
                foreach (System.Data.DataColumn col in source.Columns)
                {
                    row.Add(col.ColumnName.Trim(), JToken.FromObject(dr[col]));
                }
                result.Add(row);
            }
            return result;
        }
    }
}
