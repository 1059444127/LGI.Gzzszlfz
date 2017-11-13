using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using HL7;


namespace PathHISJK
{
    public class odbcsql
    {
        /// <summary>
        /// 声明读取ini文件    
        /// </summary>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        StringBuilder retVal,
            int size,
        string lpFileName
        );

        /// <summary>
        /// 保护变量，数据库连接。
        /// </summary>
        protected OdbcConnection Connection;

        /// <summary>
        /// 保护变量，数据库连接串。
        /// </summary>
        protected String ConnectionString;
        /// </summary>
        /// <param name="DatabaseConnectionString">数据库连接串</param>
        /// 

        public odbcsql(string odbcstring, string ouid, string opwd)
        {
            log.WriteMyLog("1");

            ConnectionString = odbcstring;
            

        }
        /// <summary>
        /// 保护方法，打开数据库连接。
        /// </summary>
        protected void Open()
        {
            if (Connection == null)
            {
                try
                {
                    Connection = new OdbcConnection(ConnectionString);
                }
                catch (Exception e)
                {
                    log.WriteMyLog(e.Message);
                }
            }
            if (Connection.State.Equals(ConnectionState.Closed))
            {
                try
                {
                    Connection.Open();
                }
                catch (Exception e)
                {
                    log.WriteMyLog(e.Message);
                }
            }
        }


        /// <summary>
        /// 公有方法，关闭数据库连接。
        /// </summary>
        public void Close()
        {
            try
            {
                if (Connection != null)
                    Connection.Close();
            }
            catch (Exception e)
            {
                log.WriteMyLog(e.Message);
            }
        }
        /// <summary>
        /// 公有方法，获取数据，返回一个SqlDataReader （调用后主意调用SqlDataReader.Close()）。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>SqlDataReader</returns>
        public OdbcDataReader GetDataReader(String SqlString)
        {
            Open();
            try
            {
                OdbcCommand cmd = new OdbcCommand(SqlString, Connection);
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                log.WriteMyLog(e.Message);

                return null;
            }

        }
        /// <summary>
        /// 公有方法，获取数据，返回一个DataTable。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(String SqlString, String tablename)
        {
            DataSet dataset = GetDataSet(SqlString, tablename);
            dataset.CaseSensitive = false;
            return dataset.Tables[tablename];
        }


        /// <summary>
        /// 公有方法，执行Sql语句。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>对Update、Insert、Delete为影响到的行数，其他情况为-1</returns>
        public int ExecuteSQL(String SqlString)
        {
            int count = -1;
            Open();
            try
            {
                OdbcCommand cmd = new OdbcCommand(SqlString, Connection);
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.WriteMyLog(e.Message);

                count = -1;
            }
            finally
            {
                Close();
            }
            return count;
        }
        /// <summary>
        /// 公有方法，执行Sql语句,带参数。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>对Update、Insert、Delete为影响到的行数，其他情况为-1</returns>
        public int ExecuteSQL(String SqlString, string Parameters, byte[] bytes)
        {
            int count = -1;
            Open();
            try
            {
                OdbcCommand cmd = new OdbcCommand(SqlString, Connection);
                OdbcParameter spFile = new OdbcParameter(Parameters, OdbcType.Image);
                spFile.Value = bytes;
                cmd.Parameters.Add(spFile);
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.WriteMyLog(e.Message);

                count = -1;
            }
            finally
            {
                Close();
            }
            return count;
        }
        /// <summary>
        /// 公有方法，获取数据，返回一个DataSet。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(String SqlString, String tablename)
        {
            DataSet dataset = new DataSet();
            Open();
            try
            {
                OdbcDataAdapter adapter = new OdbcDataAdapter(SqlString, Connection);
                adapter.Fill(dataset, tablename);
            }
            catch (Exception e2)
            {
                e2.ToString();
                log.WriteMyLog(e2.Message);
                //int x = 1;
                //log.WriteLog(EventLogEntryType.Warning, "GetDataSet失败，SqlString=" + SqlString + ",系统异常信息：" + e.Message);
            }
            finally
            {
                Close();
            }
            return dataset;
        }

        /// <summary>
        /// 公有方法，执行一组Sql语句。
        /// </summary>
        /// <param name="SqlStrings">Sql语句组</param>
        /// <returns>是否成功</returns>
        public bool ExecuteSQL(String[] SqlStrings)
        {
            bool success = true;
            Open();
            OdbcCommand cmd = new OdbcCommand();
            OdbcTransaction trans = Connection.BeginTransaction();
            cmd.Connection = Connection;
            cmd.Transaction = trans;

            int i = 0;
            try
            {
                foreach (String str in SqlStrings)
                {
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    i++;
                }
                trans.Commit();
            }
            catch (Exception e)
            {
                log.WriteMyLog(e.Message);

                // log.WriteLog(EventLogEntryType.Error, "ExecuteSQL失败，SqlString=" + SqlStrings + ",系统异常信息：" + e.Message);
                success = false;
                trans.Rollback();
            }
            finally
            {
                Close();
            }
            return success;
        }
    }
}
