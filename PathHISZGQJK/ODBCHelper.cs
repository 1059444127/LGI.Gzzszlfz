using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace PathHISJK
{
    class ODBCHelper
    {
        private static string dbConnectionString = "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p";

        private OdbcConnection connection;
        public ODBCHelper()
        {
            this.connection = CreateConnection();
        }

        public ODBCHelper(string connectionString)
        {
            this.connection = CreateConnection(connectionString);

        }

        public static OdbcConnection CreateConnection()
        {
          
            OdbcConnection dbconn = new OdbcConnection(ODBCHelper.dbConnectionString);
            return dbconn;
        }

        public static OdbcConnection CreateConnection(string connectionString)
        {
            OdbcConnection dbconn = new OdbcConnection(connectionString);
            return dbconn;
        }

        public OdbcCommand GetStoredProcCommond(string storedProcedure)
        {
            OdbcCommand dbCommand = new OdbcCommand();
            dbCommand.Connection = connection;
            dbCommand.CommandText = storedProcedure;
            dbCommand.CommandType = CommandType.StoredProcedure;
            return dbCommand;
        }
        public OdbcCommand GetOdbcStringCommond(string sqlQuery)
        {

            OdbcCommand dbCommand = new OdbcCommand(sqlQuery, connection);
            dbCommand.CommandType = CommandType.Text;
            return dbCommand;
        }

        //增加参数#region 增加参数
        #region
        public void AddParameterCollection(OdbcCommand cmd, OdbcParameterCollection dbParameterCollection)
        {
            foreach (OdbcParameter dbParameter in dbParameterCollection)
            {
                cmd.Parameters.Add(dbParameter);
            }
        }

        public void AddOutParameter(OdbcCommand cmd, string parameterName, DbType dbType, int size)
        {
            OdbcParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Size = size;
            dbParameter.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(dbParameter);
         
        }

        public void AddInParameter(OdbcCommand cmd, string parameterName, DbType dbType, object value)
        {
            OdbcParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Value = value;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);
          
        }

        public void AddReturnParameter(OdbcCommand cmd, string parameterName, DbType dbType)
        {
            OdbcParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = dbType;
            dbParameter.ParameterName = parameterName;
            dbParameter.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(dbParameter);
        }

        public OdbcParameter GetParameter(OdbcCommand cmd, string parameterName)
        {
            return cmd.Parameters[parameterName];
        }

        #endregion

        //执行#region 执行
        #region

        //public DataSet ExecuteDataSet(OdbcCommand cmd)
        //{
        //    OdbcProviderFactory dbfactory = OdbcProviderFactories.GetFactory(DBHelper.dbProviderName);
        //    OdbcDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
        //    dbDataAdapter.SelectCommand = cmd;
        //    DataSet ds = new DataSet();
        //    dbDataAdapter.Fill(ds);
        //    return ds;
        //}

        //public DataTable ExecuteDataTable(OdbcCommand cmd)
        //{
        //    OdbcProviderFactory dbfactory = OdbcProviderFactories.GetFactory(DBHelper.dbProviderName);
        //    OdbcDataAdapter dbDataAdapter = dbfactory.CreateDataAdapter();
        //    dbDataAdapter.SelectCommand = cmd;
        //    DataTable dataTable = new DataTable();
        //    dbDataAdapter.Fill(dataTable);
        //    return dataTable;
        //}

        public OdbcDataReader ExecuteReader(OdbcCommand cmd)
        {
            cmd.Connection.Open();
            OdbcDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }

        public int ExecuteNonQuery(OdbcCommand cmd)
        {
            cmd.Connection.Open();
              int ret = cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return ret;
        }

        public object ExecuteScalar(OdbcCommand cmd)
        {
            cmd.Connection.Open();
            object ret = cmd.ExecuteScalar();
            cmd.Connection.Close();
            return ret;
        }

        #endregion



    }
}
