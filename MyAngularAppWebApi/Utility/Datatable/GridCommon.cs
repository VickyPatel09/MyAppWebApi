using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility.Datatable
{
    public class GridCommon
    {
        public async Task<GridFinalData> GetJsonAsync(GridData objGrid)
        {
            var dt = await GetDataTable(objGrid);
            var obj = new GridFinalData
            {
                Data = dt.Item2,
                Draw = Convert.ToString(objGrid.Draw, CultureInfo.CurrentCulture),
                RecordsFiltered = dt.Item1.ToString(CultureInfo.CurrentCulture),
                RecordsTotal = dt.Item1.ToString(CultureInfo.CurrentCulture)
            };
            return obj;
        }

        private static async Task<Tuple<int, List<Dictionary<string, object>>>> GetDataTable(GridData objGrid)
        {
            var parameter = new object[7];
            parameter[0] = new SqlParameter().CreateParameter("@TableName", objGrid.TableName, -1);
            parameter[1] = new SqlParameter().CreateParameter("@ColumnsName", objGrid.ColumnsName, -1);
            parameter[2] = new SqlParameter().CreateParameter("@SortOrder", objGrid.SortOrder);
            parameter[3] = new SqlParameter().CreateParameter("@SortColumn", objGrid.SortColumn);
            parameter[4] = new SqlParameter().CreateParameter("@PageNumber", objGrid.PageNumber);
            parameter[5] = new SqlParameter().CreateParameter("@RecordPerPage", objGrid.RecordPerPage);
            parameter[6] = new SqlParameter().CreateParameter("@WhereClause", objGrid.WhereClause, -1);
            var dr = await SqlHelper.ExecuteReaderAsync(objGrid.ConnectionString, "GetGridData", parameter);
            var lst = new List<Dictionary<string, object>>();
            var total = 0;
            while (await dr.ReadAsync())
            {
                var obj = new Dictionary<string, object>();
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    if (total == 0 && dr.GetName(i).ToLower() == "totalrows")
                    {
                        total = Convert.ToInt32(dr[i]);
                    }

                    obj[dr.GetName(i)] = dr[i].GetType().Name == "DBNull" ? null : dr[i];
                }

                lst.Add(obj);
            }

            return new Tuple<int, List<Dictionary<string, object>>>(total, lst);
        }

    }
}
