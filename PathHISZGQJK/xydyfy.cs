using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dbbase;
using WindowsApplication2;
using System.ComponentModel;
using System.Drawing;
using ZgqClassPub;


namespace PathHISZGQJK
{
    class xydyfy //��ҽ��һ��Ժ�ã�����ǩ��
    {
        //string CN = string.Empty;
        //string DN = string.Empty;
        //string SN = string.Empty;
        //string webservice = "https://203.207.203.16:8443/webServices/authService";
        //string userid = "2c9484f231dca3ad0131f9cd380f002c";


       
     //------------------------------------------------------------------------------------------
        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetCertDN", CharSet = CharSet.Ansi, SetLastError = false)]
        // ���ã�ͨ��cnȡ��֤��������Ϣ
        public static extern string XJCA_TechGet(string ip, string cn);
        // ���ã�ͨ��cn�ж�֤�����/������
        public static extern string  XJCA_TechCheck(string ip,string cn);
       	// ���ã�ͨ��Ӧ��ID��cn�����жϸ�֤����ڸ�Ӧ���Ƿ���Ч/��Ч
        public static extern string XJCA_TechAppAuth(string ip, string appid, string sn);


                // ��ȡ�ͻ��˳���״̬dll
         public static extern int SSLStatus();
	        // ���ã��������ر�SSL�ͻ��˷���ע�������Ϣ
         public static extern int SSLService(string  wParam);
	        // ���ã���ȡSSL��ǰ�û���CN
         public static extern string   SSLCurrentUsingCN();


         //------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------

        private static IniFiles f = new IniFiles("sz.ini");

        public void pathtohis(string blh, string yymc)
        {

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

            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
            if (bgzt == "�����")
            {





            }
        }




    }
}
