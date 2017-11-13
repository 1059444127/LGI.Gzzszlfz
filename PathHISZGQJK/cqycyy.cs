using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class cqycyy
    {
        private static IniFiles f = new IniFiles("sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {
            string con_str = f.ReadString("savetohis", "odbcsql", "Data Source=ORCL_100;User ID=jk_ycxd;Password=123;");
            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");

           
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            if (bljc == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                log.WriteMyLog("�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                log.WriteMyLog("������д���");
                return;
            }
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {

                log.WriteMyLog("��������ţ����ݺţ���������");
                return;
            }
            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
            if(bgzt == "�����" || bgzt == "��д����")
            {
                    string bgztID="0";
                    if (bgzt == "�����")
                    {
                        bgztID = "1";
                    } 
                 OracleConnection orcon = new OracleConnection(con_str);
                 try
                 {

                     OracleCommand com = new OracleCommand();
                     com.Connection = orcon;
                     com.CommandType = CommandType.StoredProcedure;
                     com.CommandText = "p_pathology_updatestate";

                     OracleParameter op = new OracleParameter();
                     op.ParameterName = "applid_id_in";
                     op.OracleType = OracleType.NVarChar;
                     op.Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();
                     com.Parameters.Add(op);

                     OracleParameter op1 = new OracleParameter();
                     op1.ParameterName = "type_in";
                     op1.OracleType = OracleType.NVarChar;
                     op1.Value = bgztID;
                     com.Parameters.Add(op1);
                     orcon.Open();
                   
                     int x = com.ExecuteNonQuery();

                 

                     orcon.Close();
                     com.Dispose();
                 }
                 catch (Exception e)
                 {
                     MessageBox.Show(e.ToString());
                     log.WriteMyLog("����״̬��дʧ�ܣ�" + e.ToString());
                 }
                 finally
                 {
                     if(orcon.State==ConnectionState.Open)
                         orcon.Close();


                 }
            }
        



        }
    }
}
