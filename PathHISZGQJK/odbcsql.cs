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
        /// ������ȡini�ļ�    
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
        /// �������������ݿ����ӡ�
        /// </summary>
        protected OdbcConnection Connection;

        /// <summary>
        /// �������������ݿ����Ӵ���
        /// </summary>
        protected String ConnectionString;
        /// </summary>
        /// <param name="DatabaseConnectionString">���ݿ����Ӵ�</param>
        /// 

        public odbcsql(string odbcstring, string ouid, string opwd)
        {
            log.WriteMyLog("1");

            ConnectionString = odbcstring;
            

        }
        /// <summary>
        /// ���������������ݿ����ӡ�
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
        /// ���з������ر����ݿ����ӡ�
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
        /// ���з�������ȡ���ݣ�����һ��SqlDataReader �����ú��������SqlDataReader.Close()����
        /// </summary>
        /// <param name="SqlString">Sql���</param>
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
        /// ���з�������ȡ���ݣ�����һ��DataTable��
        /// </summary>
        /// <param name="SqlString">Sql���</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(String SqlString, String tablename)
        {
            DataSet dataset = GetDataSet(SqlString, tablename);
            dataset.CaseSensitive = false;
            return dataset.Tables[tablename];
        }


        /// <summary>
        /// ���з�����ִ��Sql��䡣
        /// </summary>
        /// <param name="SqlString">Sql���</param>
        /// <returns>��Update��Insert��DeleteΪӰ�쵽���������������Ϊ-1</returns>
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
        /// ���з�����ִ��Sql���,��������
        /// </summary>
        /// <param name="SqlString">Sql���</param>
        /// <returns>��Update��Insert��DeleteΪӰ�쵽���������������Ϊ-1</returns>
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
        /// ���з�������ȡ���ݣ�����һ��DataSet��
        /// </summary>
        /// <param name="SqlString">Sql���</param>
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
                //log.WriteLog(EventLogEntryType.Warning, "GetDataSetʧ�ܣ�SqlString=" + SqlString + ",ϵͳ�쳣��Ϣ��" + e.Message);
            }
            finally
            {
                Close();
            }
            return dataset;
        }

        /// <summary>
        /// ���з�����ִ��һ��Sql��䡣
        /// </summary>
        /// <param name="SqlStrings">Sql�����</param>
        /// <returns>�Ƿ�ɹ�</returns>
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

                // log.WriteLog(EventLogEntryType.Error, "ExecuteSQLʧ�ܣ�SqlString=" + SqlStrings + ",ϵͳ�쳣��Ϣ��" + e.Message);
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
