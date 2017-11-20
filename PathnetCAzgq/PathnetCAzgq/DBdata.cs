using System;
using System.Collections.Generic;
using System.Text;
using readini;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using LGZGQClass;

namespace PathnetCAzgq
{
    class DBdata
    {
    
      private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        string Server = f.ReadString("sqlserver", "Server", "");
        string DataBase = f.ReadString("sqlserver", "DataBase", "");
        string UserID = f.ReadString("sqlserver", "UserID", "");
        string PassWord = f.ReadString("sqlserver", "PassWord", "");
  
         //string orcon_str = "Provider='MSDAORA'; data source=DBSERVER;User ID=DHC;Password=DHC;";
        string orcon_str = "Provider='MSDAORA';data source=DBSERVER;user id =DHC;password=DHC;";   
        string odbcsql = f.ReadString("SF", "odbcsql", "");
        public DBdata()
        { }
        public DBdata(string dataBase)
        {
            DataBase = dataBase;
        }     


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
            catch 
            {
               
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
            catch
            {

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
                log.WriteMyLog("orcle数据库查询操作错误，" + sm + "--" + orcl_ee.ToString());
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
                log.WriteMyLog("插入orcal数据库插入错误" + sm + "--" + insert_ee.ToString());
                return 0;
            }
            return x;

        }
    }
}
