using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility.Datatable
{
    public static class SqlHelperParameterCache
    {
        private static readonly Hashtable ParamCache = Hashtable.Synchronized(new Hashtable());

        public static void CacheParameterSet(
            string connectionString,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            var hashKey = connectionString + ":" + commandText;

            ParamCache[hashKey] = commandParameters;
        }

        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            var hashKey = connectionString + ":" + commandText;

            if (!(ParamCache[hashKey] is SqlParameter[] cachedParameters))
            {
                return null;
            }

            return CloneParameters(cachedParameters);
        }

        public static SqlParameter[]
            GetstoredProcedureParameteret(string connectionString, string storedProcedureName) =>
            GetstoredProcedureParameteret(connectionString, storedProcedureName, false);

        public static SqlParameter[] GetstoredProcedureParameteret(
            string connectionString,
            string storedProcedureName,
            bool includeReturnValueParameter)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            using (var connection = new SqlConnection(connectionString))
            {
                return GetstoredProcedureParameteretInternal(
                    connection,
                    storedProcedureName,
                    includeReturnValueParameter);
            }
        }

        internal static SqlParameter[] GetstoredProcedureParameteret(
            SqlConnection connection,
            string storedProcedureName) =>
            GetstoredProcedureParameteret(connection, storedProcedureName, false);

        internal static SqlParameter[] GetstoredProcedureParameteret(
            SqlConnection connection,
            string storedProcedureName,
            bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            using (var clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetstoredProcedureParameteretInternal(
                    clonedConnection,
                    storedProcedureName,
                    includeReturnValueParameter);
            }
        }

        private static SqlParameter[] CloneParameters(IList<SqlParameter> originalParameters)
        {
            var clonedParameters = new SqlParameter[originalParameters.Count];

            for (int i = 0, j = originalParameters.Count; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        private static SqlParameter[] DiscoverstoredProcedureParameteret(
            SqlConnection connection,
            string storedProcedureName,
            bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            var cmd = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure };

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            var discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (var discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }

            return discoveredParameters;
        }

        private static SqlParameter[] GetstoredProcedureParameteretInternal(
            SqlConnection connection,
            string storedProcedureName,
            bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            var hashKey = connection.ConnectionString + ":" + storedProcedureName
                          + (includeReturnValueParameter ? ":include ReturnValue Parameter" : string.Empty);

            if (ParamCache[hashKey] is SqlParameter[] cachedParameters)
            {
                return CloneParameters(cachedParameters);
            }

            var storedProcedureParameter = DiscoverstoredProcedureParameteret(
                connection,
                storedProcedureName,
                includeReturnValueParameter);
            ParamCache[hashKey] = storedProcedureParameter;
            cachedParameters = storedProcedureParameter;

            return CloneParameters(cachedParameters);
        }
    }
}
