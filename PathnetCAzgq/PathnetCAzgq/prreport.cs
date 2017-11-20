using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data.Odbc;
using LGZGQClass;
using PathnetCAzgq;



namespace PathHISZGQJK
{
    class prreport
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        StringBuilder retVal,
            int size,
        string lpFileName
        );

        [DllImport("reportdll2.dll")]
        private static extern long report32(
            //IntPtr ahandle,
        int dy,
        string conn,
        string sqlm,
        string image,
        string imagename,
        string yymc,
        string bbbm,
        string outfilename
        );

        public delegate long report2(
        int dy,
        string conn,
        string sqlm,
        string image,
        string imagename,
        string yymc,
        string bbbm,
        string outfilename
        );


        private LoadDllapi dllload = new LoadDllapi();

        protected String ConnectionString;
        protected string x1;
        protected int dy;



        private delegate  bool wt_zoominout(
            //IntPtr ahandle,
        string sourcefile,
        string targetfile,
        int picx,
        int picy        
        );


        public static bool txzoom(string sor, string dst, int picx, int picy)
        {

            try
            {
                LoadDllapi dllloadz = new LoadDllapi();

                if ((int)dllloadz.initPath("imagedll.dll") == 0)
                {
                    //MessageBox.Show("��ӡ�ؼ����ô���");
                    log.WriteMyLog("��ӡ�ؼ����ô���");
                    return false;

                }

                wt_zoominout xx = (wt_zoominout)dllloadz.InvokeMethod("wt_zoominout", typeof(wt_zoominout));

                return xx(sor, dst, picx, picy);
            }
            catch
            {
                return false;
            }
        }


        public prreport()
        {
            string pathstr = Application.StartupPath + "\\sz.ini";
            //WritePrivateProfileString("StudentInfo", "Name", strName, pathstr);
            StringBuilder strTemp = new StringBuilder(255);
            int i = GetPrivateProfileString("sqlserver", "Server", "", strTemp, 255, pathstr);
            string server = strTemp.ToString().Trim();
            i = GetPrivateProfileString("sqlserver", "DataBase", "", strTemp, 255, pathstr);
            string database = strTemp.ToString().Trim();
            i = GetPrivateProfileString("sqlserver", "UserID", "", strTemp, 255, pathstr);
            string userid = strTemp.ToString().Trim();
            i = GetPrivateProfileString("sqlserver", "PassWord", "", strTemp, 255, pathstr);
            string password = strTemp.ToString().Trim();
            try
            {
                i = GetPrivateProfileString("bbsj", "sj", "", strTemp, 255, pathstr);

                dy = Convert.ToInt16(strTemp.ToString().Trim());
            }
            catch
            {
                dy = 1;
            }
            ConnectionString = "Provider=MSDASQL.1;Persist Security Info=True;DRIVER=SQL Server;pwd=" + password + ";SERVER=" + server + ";DATABASE=" + database + ";UID=" + userid + ";APP=pasnet";
            if (server.Trim() == "")
            {
                OdbcConnection oconn = new OdbcConnection("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p");
                oconn.Open();
                string oserver = oconn.DataSource.ToString();
                string odatabase = oconn.Database.ToString();
                oconn.Close();


     //           string odbcstring = "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p";
//
   //             server = odbcstring.Substring(odbcstring.IndexOf("DSN=") + 4, odbcstring.IndexOf(";") - 4);
  //              RegistryKey rs;
  //              rs = Registry.LocalMachine.OpenSubKey("SOFTWARE\\ODBC\\ODBC.INI\\" + server);
   //             if (rs != null)
   //             {
    //                server = rs.GetValue("Server", "").ToString();
//
    //            }
  //              rs.Close();


                ConnectionString = "Provider=MSDASQL.1;Persist Security Info=True;DRIVER=SQL Server;pwd=4s3c2a1p;SERVER=" + oserver + ";DATABASE=" + odatabase + ";UID=pathnet;APP=pasnet";

            }
            //sqlserverdatabase obj = new sqlserverdatabase();

            x1 = Application.StartupPath;
        }
        public void print(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname)
        {

            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));

            string yymc = "";

            long longxx = xx(10, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            
            //string yymc = "��������";
            //report2(10, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();

        }

        public void printpdf(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname)
        {

            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));

            string yymc = "";

            long longxx = xx(13, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);

            //string yymc = "��������";
            //report2(10, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();

        }


        public void print(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname,string  debug)
        {

            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return;
            }


            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));

            string yymc = "";
            if (debug == "1")
            {
                log.WriteMyLog(ConnectionString + "\r\n" + sqlstring + "\r\n" + image + "\r\n" + imagename + "\r\n" + bgname + "\r\n" + jpgname);
            }
            long longxx = xx(10, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            if (debug == "1")
            {
                log.WriteMyLog("��ӡ�ؼ����÷��أ�" + longxx.ToString());
            }
            //string yymc = "��������";
            //report2(10, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();

        }

        public void printpdf(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname,string debug)
        {
            if (debug == "1")
                log.WriteMyLog("��������pdf����");

            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));

            if (debug == "1")
                log.WriteMyLog("��ʼ��reportdll2.dll�ɹ�,��ʼ����pdf");

            long longxx = xx(13, ConnectionString, sqlstring, image, imagename, "", bgname, jpgname);

            if (debug == "1")
                log.WriteMyLog("����pdf�ɹ�");

            dllload.freeLoadDll();
            
            if(debug=="1")
                log.WriteMyLog("����pdf�ɹ�");
        }

    }
}
