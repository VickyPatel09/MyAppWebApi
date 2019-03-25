using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility.Datatable
{
    public static class Parameter
    {
        public static SqlParameter CreateParameter(
            this SqlParameter parameter,
            string parameterName,
            string parameterValue,
            int size = 50,
            ParameterDirection dir = ParameterDirection.Input)
        {
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.Size = size;
            parameter.SqlDbType = SqlDbType.NVarChar;
            parameter.Direction = dir;
            return parameter;
        }

        public static SqlParameter CreateParameter(
            this SqlParameter parameter,
            string parameterName,
            int parameterValue,
            ParameterDirection dir = ParameterDirection.Input)
        {
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.SqlDbType = SqlDbType.Int;
            parameter.Direction = dir;
            return parameter;
        }

        public static SqlParameter CreateParameter(
            this SqlParameter parameter,
            string parameterName,
            decimal parameterValue,
            ParameterDirection dir = ParameterDirection.Input)
        {
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.SqlDbType = SqlDbType.Decimal;
            parameter.Direction = dir;
            return parameter;
        }

        public static SqlParameter CreateParameter(
            this SqlParameter parameter,
            string parameterName,
            float parameterValue,
            ParameterDirection dir = ParameterDirection.Input)
        {
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.SqlDbType = SqlDbType.Float;
            parameter.Direction = dir;
            return parameter;
        }

        public static SqlParameter CreateParameter(
            this SqlParameter parameter,
            string parameterName,
            DateTime parameterValue,
            ParameterDirection dir = ParameterDirection.Input)
        {
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.SqlDbType = SqlDbType.DateTime;
            parameter.Direction = dir;
            return parameter;
        }

        public static SqlParameter CreateParameter(
            this SqlParameter parameter,
            string parameterName,
            DataTable parameterValue,
            ParameterDirection dir = ParameterDirection.Input)
        {
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.Direction = dir;
            return parameter;
        }
    }
}
