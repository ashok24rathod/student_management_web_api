using System.Data.SqlClient;
using System.Data;

namespace StudentManagementApi
{
    public class DBHelper
    {
        public static DataTable GetTableFromSP(string connectionString, string sp, SqlParameter[] prms)
        {
            DataTable dt = null;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                SqlCommand command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
                connection.Open();

                if (prms != null && prms.Length > 0)
                {
                    command.Parameters.AddRange(prms);
                }

                DataSet dataSet = new DataSet();
                (new SqlDataAdapter(command)).Fill(dataSet);
                command.Parameters.Clear();

                if (dataSet.Tables.Count > 0)
                {
                    dt = dataSet.Tables[0];
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static DataTable GetTableFromSP(string connectionString, string sp)
        {
            DataTable dt = null;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            try
            {
                command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
                connection.Open();

                DataSet dataSet = new DataSet();
                (new SqlDataAdapter(command)).Fill(dataSet);
                command.Parameters.Clear();

                if (dataSet.Tables.Count > 0)
                {
                    dt = dataSet.Tables[0];
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return dt;
        }

        public static int ExecuteNonQuery(string connectionString, string sp, SqlParameter[] prms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            try
            {
                command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
                connection.Open();
                command.Parameters.AddRange(prms);
                return command.ExecuteNonQuery();
            }
            catch
            {
                throw;

            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public static int ExecuteNonQuery(string connectionString, string sp, SqlParameter prms)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            try
            {
                command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
                connection.Open();
                prms.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(prms);
                return command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public static string ExecuteScalarForSP(string connectionString, string sp, SqlParameter[] prms)
        {
            string result = "";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            try
            {
                connection.Open();
                command = new SqlCommand(sp, connection) { CommandType = CommandType.StoredProcedure, CommandTimeout = connection.ConnectionTimeout };
                command.Parameters.AddRange(prms);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0 & dt.Rows[0].ItemArray.Length > 0)
                {
                    result = Convert.ToString(dt.Rows[0][0]);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return result;
        }
    }
}