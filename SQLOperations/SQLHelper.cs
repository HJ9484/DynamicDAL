using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.SQLOperations
{
    public static class SQLHelper
    {
        #region Private Memebrs
        private static System.Data.SqlClient.SqlConnection _connection;
        // به دلیل تکرار این کد در تعدادی از توابع این متد را به منظور جلوگیری از تکرار نوشتیم
        private static System.Data.SqlClient.SqlCommand CreateCommand(string commandText, System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand()
            {
                CommandText = commandText,
                CommandType = commandType,
                Connection = Connection
            };
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            return command;
        }

        private static async Task<System.Data.SqlClient.SqlCommand> CreateCommandAsync(string commandText, System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
            await Task.Run(() =>
            {
                command.CommandText = commandText;
                command.CommandType = commandType;
                command.Connection = Connection;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

            });
            return command;
        }

        #endregion


        #region Constructors 
        static SQLHelper()
        {
            //_connection = new System.Data.SqlClient.SqlConnection("");
        }
        #endregion


        #region Class Members
        public static System.Data.SqlClient.SqlConnection Connection
        {
            get
            {
              
                if (_connection == null)
                    _connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ADOConnection"]?.ConnectionString ?? string.Empty);
                if (_connection.State == System.Data.ConnectionState.Closed)
                    _connection.Open();
                return _connection;
               
            }
        }
        public static void ExecuteReaderCommand(string commandText, Action<System.Data.SqlClient.SqlDataReader> handler /*Callback Method*/) // در صورتیکه بخواهیم از دلیگیت استاندارد خود ویژوال استدیو  استفاده کنیم
        {
            ExecuteReaderCommand(commandText, handler, null);
        }
        public static void ExecuteReaderCommand(string commandText, Action<System.Data.SqlClient.SqlDataReader> handler /*Callback Method*/ , params System.Data.SqlClient.SqlParameter[] parameters)
        {
            ExecuteReaderCommand(commandText, handler, System.Data.CommandType.Text, parameters);
        }
        public static void ExecuteReaderCommand(string commandText, Action<System.Data.SqlClient.SqlDataReader> handler /*Callback Method*/ , System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = CreateCommand(commandText, commandType, parameters))
                {
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        handler?.Invoke(reader);
                    }
                    reader.Close();
                }
            }
            catch
            {
                throw;
            }
        }


        public static async Task ExecuteReaderCommandAsync(string commandText, Action<System.Data.SqlClient.SqlDataReader> handler /*Callback Method*/) // در صورتیکه بخواهیم از دلیگیت استاندارد خود ویژوال استدیو  استفاده کنیم
        {
            await ExecuteReaderCommandAsync(commandText, handler, null);
        }
        public static async Task ExecuteReaderCommandAsync(string commandText, Action<System.Data.SqlClient.SqlDataReader> handler /*Callback Method*/ , params System.Data.SqlClient.SqlParameter[] parameters)
        {
            await ExecuteReaderCommandAsync(commandText, handler, System.Data.CommandType.Text, parameters);
        }
        public static async Task ExecuteReaderCommandAsync(string commandText, Action<System.Data.SqlClient.SqlDataReader> handler /*Callback Method*/ , System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = await CreateCommandAsync(commandText, commandType, parameters))
                {
                    System.Data.SqlClient.SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        handler?.Invoke(reader);
                    }
                    reader.Close();
                }
            }
            catch
            {
                throw;
            }
        }


        public static System.Data.DataTable ExecuteReaderDataTable(string commandText)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = CreateCommand(commandText, System.Data.CommandType.Text))
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    var reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public static async Task<System.Data.DataTable> ExecuteReaderDataTableAsync(string commandText)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = await CreateCommandAsync(commandText, System.Data.CommandType.Text))
                {
                    System.Data.DataTable dt = new System.Data.DataTable();
                    var reader = await command.ExecuteReaderAsync();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        public static object ExecuteScalarCommand(string commandText)
        {
            return ExecuteScalarCommand(commandText, null);
        }
        public static object ExecuteScalarCommand(string commandText, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            return ExecuteScalarCommand(commandText, System.Data.CommandType.Text, parameters);
        }
        public static object ExecuteScalarCommand(string commandText, System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = CreateCommand(commandText, commandType, parameters))
                {
                    return command.ExecuteScalar();
                }
            }
            catch
            {

                throw;
            }
        }


        public static async Task<object> ExecuteScalarCommandAsync(string commandText)
        {
            return await ExecuteScalarCommandAsync(commandText, null);
        }
        public static async Task<object> ExecuteScalarCommandAsync(string commandText, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            return await ExecuteScalarCommandAsync(commandText, System.Data.CommandType.Text, parameters);
        }
        public static async Task<object> ExecuteScalarCommandAsync(string commandText, System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = CreateCommand(commandText, commandType, parameters))
                {
                    return await command.ExecuteScalarAsync();
                }
            }
            catch
            {

                throw;
            }
        }


        public static int ExecuteNonQueryCommand(string commandText)
        {
            return ExecuteNonQueryCommand(commandText, null);
        }
        public static int ExecuteNonQueryCommand(string commandText, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            return ExecuteNonQueryCommand(commandText, System.Data.CommandType.Text, parameters);
        }
        public static int ExecuteNonQueryCommand(string commandText, System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = CreateCommand(commandText, commandType, parameters))
                {
                    return command.ExecuteNonQuery();
                }
            }
            catch
            {

                throw;
            }
        }


        public static async Task<int> ExecuteNonQueryCommandAsync(string commandText)
        {
            return await ExecuteNonQueryCommandAsync(commandText, null);
        }
        public static async Task<int> ExecuteNonQueryCommandAsync(string commandText, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            return await ExecuteNonQueryCommandAsync(commandText, System.Data.CommandType.Text, parameters);
        }
        public static async Task<int> ExecuteNonQueryCommandAsync(string commandText, System.Data.CommandType commandType, params System.Data.SqlClient.SqlParameter[] parameters)
        {
            try
            {
                using (System.Data.SqlClient.SqlCommand command = CreateCommand(commandText, commandType, parameters))
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
            catch
            {

                throw;
            }
        }


        #endregion

    }
}
