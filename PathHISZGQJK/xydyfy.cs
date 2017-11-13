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
    class xydyfy //新医大一附院ＣＡ电子签章
    {
        //string CN = string.Empty;
        //string DN = string.Empty;
        //string SN = string.Empty;
        //string webservice = "https://203.207.203.16:8443/webServices/authService";
        //string userid = "2c9484f231dca3ad0131f9cd380f002c";


       
     //------------------------------------------------------------------------------------------
        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetCertDN", CharSet = CharSet.Ansi, SetLastError = false)]
        // 作用：通过cn取得证书所有信息
        public static extern string XJCA_TechGet(string ip, string cn);
        // 作用：通过cn判断证书可用/不可用
        public static extern string  XJCA_TechCheck(string ip,string cn);
       	// 作用：通过应用ID和cn，来判断该证书对于该应用是否有效/无效
        public static extern string XJCA_TechAppAuth(string ip, string appid, string sn);


                // 获取客户端程序状态dll
         public static extern int SSLStatus();
	        // 作用：启动、关闭SSL客户端服务，注销身份消息
         public static extern int SSLService(string  wParam);
	        // 作用：获取SSL当前用户的CN
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
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }

            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
            if (bgzt == "已审核")
            {





            }
        }




    }
}
