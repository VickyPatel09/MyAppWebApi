using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility.Datatable
{
    public static class SqlHelper
    {
        public static Task<DataSet> ExecuteDatasetAsync(
            string connectionString,
            CommandType commandType,
            string commandText) =>
            ExecuteDatasetAsync(connectionString, commandType, commandText, null);

        public static async Task<DataSet> ExecuteDatasetAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                // Call the overload that takes a connection in place of the connection string
                return await ExecuteDatasetAsync(connection, commandType, commandText, commandParameters)
                           .ConfigureAwait(false);
            }
        }

        public static async Task<DataSet> ExecuteDatasetAsync(
            SqlConnection connection,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            var mustCloseConnection =
                await PrepareCommandAsync(cmd, connection, null, commandType, commandText, commandParameters)
                    .ConfigureAwait(false);

            // Create the DataAdapter & DataSet
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                {
                    connection.Close();
                }

                // Return the dataset
                return ds;
            }
        }

        public static async Task<DataSet> ExecuteDatasetAsync(
            SqlTransaction transaction,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.",
                    nameof(transaction));
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            await PrepareCommandAsync(
                cmd,
                transaction.Connection,
                transaction,
                commandType,
                commandText,
                commandParameters).ConfigureAwait(false);

            // Create the DataAdapter & DataSet
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                // Return the dataset
                return ds;
            }
        }

        public static async Task<DataTable> ExecuteDataTableAsync(
            string connectionString,
            CommandType commandType,
            string commandText)
        {
            var datasetResult = await ExecuteDatasetAsync(connectionString, commandType, commandText, null);

            return datasetResult != null && datasetResult.Tables.Count > 0 ? datasetResult.Tables[0] : new DataTable();
        }

        public static async Task<DataTable> ExecuteDataTableAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var datasetResult = await ExecuteDatasetAsync(connection, commandType, commandText, commandParameters);
                if (datasetResult != null && datasetResult.Tables.Count > 0)
                {
                    return datasetResult.Tables[0];
                }

                return new DataTable();
            }
        }

        public static async Task<DataTable> ExecuteDataTableAsync(
            string connectionString,
            string storedProcedureName,
            params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            DataSet datasetResult;

            // If we receive parameter values, we need to figure out where they go
            if (parameterValues != null && parameterValues.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                var commandParameters =
                    SqlHelperParameterCache.GetstoredProcedureParameteret(connectionString, storedProcedureName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                datasetResult = new DataSet();
                datasetResult = await ExecuteDatasetAsync(
                                    connectionString,
                                    CommandType.StoredProcedure,
                                    storedProcedureName,
                                    commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                datasetResult = new DataSet();
                datasetResult = await ExecuteDatasetAsync(
                                    connectionString,
                                    CommandType.StoredProcedure,
                                    storedProcedureName);
            }

            if (datasetResult != null && datasetResult.Tables.Count > 0)
            {
                return datasetResult.Tables[0];
            }

            return new DataTable();
        }

        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return await ExecuteNonQueryAsync(connection, commandType, commandText, commandParameters)
                           .ConfigureAwait(false);
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(
            SqlConnection connection,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var cmd = new SqlCommand();
            var mustCloseConnection =
                await PrepareCommandAsync(cmd, connection, null, commandType, commandText, commandParameters)
                    .ConfigureAwait(false);

            var retval = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

            cmd.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }

            return retval;
        }

        public static Task<SqlDataReader> ExecuteReaderAsync(
            string connectionString,
            CommandType commandType,
            string commandText) =>
            ExecuteReaderAsync(connectionString, commandType, commandText, null);

        public static async Task<SqlDataReader> ExecuteReaderAsync(
            string connectionString,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                await connection.OpenAsync().ConfigureAwait(false);

                // Call the private overload that takes an internally owned connection in place of the connection string
                return await ExecuteReaderAsync(
                           connection,
                           null,
                           commandType,
                           commandText,
                           commandParameters,
                           SqlConnectionOwnership.Internal).ConfigureAwait(false);
            }
            catch
            {
                connection?.Close();
                throw;
            }
        }

        public static Task<SqlDataReader> ExecuteReaderAsync(
            string connectionString,
            string storedProcedureName,
            params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(storedProcedureName))
            {
                throw new ArgumentNullException(nameof(storedProcedureName));
            }

            // If we receive parameter values, we need to figure out where they go
            if (parameterValues == null || parameterValues.Length <= 0)
            {
                return ExecuteReaderAsync(connectionString, CommandType.StoredProcedure, storedProcedureName);
            }

            var commandParameters =
                SqlHelperParameterCache.GetstoredProcedureParameteret(connectionString, storedProcedureName);

            AssignParameterValues(commandParameters, parameterValues);

            return ExecuteReaderAsync(
                connectionString,
                CommandType.StoredProcedure,
                storedProcedureName,
                commandParameters);

            // Otherwise we can just call the SP without params
        }

        public static Task<SqlDataReader> ExecuteReaderAsync(
            SqlConnection connection,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters) =>
            ExecuteReaderAsync(
                connection,
                null,
                commandType,
                commandText,
                commandParameters,
                SqlConnectionOwnership.External);

        public static Task<SqlDataReader> ExecuteReaderAsync(
            SqlTransaction transaction,
            CommandType commandType,
            string commandText,
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.",
                    nameof(transaction));
            }

            // Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReaderAsync(
                transaction.Connection,
                transaction,
                commandType,
                commandText,
                commandParameters,
                SqlConnectionOwnership.External);
        }

        private static void AssignParameterValues(
            IReadOnlyList<SqlParameter> commandParameters,
            IReadOnlyList<object> parameterValues)
        {
            if (commandParameters == null || parameterValues == null)
            {
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Count != parameterValues.Count)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = commandParameters.Count; i < j; i++)
            {
                switch (parameterValues[i])
                {
                    case IDbDataParameter value:
                        {
                            var paramInstance = value;
                            commandParameters[i].Value = paramInstance.Value ?? DBNull.Value;
                            break;
                        }

                    case null:
                        commandParameters[i].Value = DBNull.Value;
                        break;
                    default:
                        commandParameters[i].Value = parameterValues[i];
                        break;
                }
            }
        }

        private static void AttachParameters(SqlCommand command, IEnumerable<SqlParameter> commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (commandParameters == null)
            {
                return;
            }

            foreach (var p in commandParameters.Where(p => p != null))
            {
                // Check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input)
                    && p.Value == null)
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        private static async Task<SqlDataReader> ExecuteReaderAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            CommandType commandType,
            string commandText,
            IEnumerable<SqlParameter> commandParameters,
            SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            var mustCloseConnection = false;

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            try
            {
                mustCloseConnection = await PrepareCommandAsync(
                                          cmd,
                                          connection,
                                          transaction,
                                          commandType,
                                          commandText,
                                          commandParameters).ConfigureAwait(false);

                // Create a reader
                SqlDataReader dataReader;

                // Call ExecuteReaderAsync with the appropriate CommandBehavior
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                }
                else
                {
                    dataReader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
                }

                // Detach the SqlParameters from the command object, so they can be used again.
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can´t set its values. 
                // When this happen, the parameters can´t be used again in other command.
                var canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }

                throw;
            }
        }

        private static async Task<bool> PrepareCommandAsync(
            SqlCommand command,
            SqlConnection connection,
            SqlTransaction transaction,
            CommandType commandType,
            string commandText,
            IEnumerable<SqlParameter> commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            var mustCloseConnection = false;

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                await connection.OpenAsync().ConfigureAwait(false);
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException(
                        "The transaction was rollbacked or commited, please provide an open transaction.",
                        nameof(transaction));
                }

                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return mustCloseConnection;
        }
    }
}
