using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;

namespace PATHGETHISZGQ
{
    class DBdata
    {
    
      private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        string Server = f.ReadString("sqlserverzgq", "Server", "");
        string DataBase = f.ReadString("sqlserverzgq", "DataBase", "");
        string UserID = f.ReadString("sqlserverzgq", "UserID", "");
        string PassWord = f.ReadString("sqlserverzgq", "PassWord", "");

         string orcon_str = "Provider='MSDAORA'; data source=DBSERVER;User ID=DHC;Password=DHC;";
            string odbcsql = f.ReadString("SF", "odbcsql", "");

        public int Execute_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = con = new SqlConnection(constr);
            SqlCommand sqlcom = null;
            try
            {
                sqlcom = new SqlCommand(sqlstr, con);
                con.Open();
                int x = sqlcom.ExecuteNonQuery();
                con.Close();
                sqlcom.Clone();

                return x;
            }
            catch(Exception  ee) 
            {
                log.WriteMyLog("执行SQL语句异常，"+sqlstr+",\r\n 异常原因："+ee.ToString());
                con.Close();
                sqlcom.Clone();
                return -1;
            }
        }
        public DataTable select_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = new SqlConnection(constr);
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, con);
                con.Open();
                sqlda.Fill(dt);
                con.Close();

                return dt;
            }
            catch(Exception ee)
            {
                log.WriteMyLog("执行SQL查询语句异常，" + sqlstr + ",\r\n 异常原因：" + ee.ToString());
                con.Close();
                return dt;
            }

        }
        public DataTable select_orcl(string orcl_strsql, string sm)
        {
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbDataAdapter orcl_dap = new OleDbDataAdapter(orcl_strsql, orcl_con);
            DataTable dt_bill_items = new DataTable();
            try
            {
                orcl_con.Open();
                orcl_dap.Fill(dt_bill_items);
                orcl_con.Close();
                return dt_bill_items;
            }
            catch (Exception orcl_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("执行ORACLE查询语句异常，" + orcl_strsql + ",\r\n 异常原因：" + orcl_ee.ToString());
                return dt_bill_items;
                
            }

        }
        public int Execute_orcl(string orcl_strsql, string sm)
        {

            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbCommand ocdc = new OleDbCommand(orcl_strsql, orcl_con);
            int x = 0;
            try
            {
                orcl_con.Open();
                x = ocdc.ExecuteNonQuery();
                orcl_con.Close();
                ocdc.Dispose();
            }
            catch (Exception insert_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("执行ORACLE语句异常，" + orcl_strsql + ",\r\n 异常原因：" + insert_ee.ToString());
                return 0;
            }
            return x;

        }
    }
}
