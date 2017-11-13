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

using ZgqClassPub;
using LoadDll;


namespace PathHISJK
{
    class mdjpg
    {
        private LoadDllapi dllxx = new LoadDllapi();
        private static string bggs = "";
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public delegate int JPG2PDF(string jpgname, string pdfname);
        public static string szweb = "";
        public static int bgx = 0;
        public static int bgy = 0;

        public void BMPTOJPG(string F_blh, ref string jpgname, string bglx, string bgxh)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            ////���c:\tempĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
            {

                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                catch
                {
                }
            }

       
            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";
        
            downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath);
           if (localpath == "")
            {
                jpgname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            if (bgxh == "")
                bgxh = "0";
            string sJPGNAME = localpath + "\\" +F_blh +"_"+bglx+"_"+bgxh+ ".jpg";
           
            if(bglx=="")
               sJPGNAME = localpath + "\\" + F_blh + ".jpg";
           
        
            string sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                 string bcbddytx = f.ReadString("bcbddytx", "bcbddytx", "").Replace("\0", "");
            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");
            if (inibglj != "")
            {
           
                sBGGSName = inibglj + "\\rpt\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            }

       
            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
            if (f.ReadInteger("rpt", "szqm", 0) == 1)
            {
                string bmppath = f.ReadString("mdbmp", "ysbmp", "d:\\pathqc\\rpt\\ysbmp").Replace("\0", "");
                stxsm = stxsm + " ,";
                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }
       

            //����ͼƬͳһ�ò����.jpg
         

            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

            if (bglx == "bd")
            {
                string rptname="����.frf";
                if (bcbddytx=="1")
                    rptname = "����"+ "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

                sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + rptname;
                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                bggs = "����.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt\\" + rptname;
                }
                sJPGNAME = localpath + "\\" + F_blh.Trim() + "_bd_"+bgxh + ".jpg";
            }
            if (bglx == "bc")
            {

                string rptname = "����.frf";
                if (bcbddytx == "1")
                    rptname = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

                sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + rptname;

                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt\\" + rptname;
                }
                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                bggs = "����.frf";
                sJPGNAME = localpath + "\\" + F_blh.Trim() + "_bc_" + bgxh + ".jpg";
            }
            System.Threading.Thread.Sleep(2000);
            prreport pr = new prreport();
            //pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            // sJPGNAME = localpath + "\\" + F_blh.Trim() + ".bmp";
            // pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            jpgname = localpath + "\\" + F_blh.Trim() + "_1.jpg";

            System.Threading.Thread.Sleep(2000);
         
           
        }

        public void BMPTOJPG(string F_blh, ref string jpgname, string bglx, string bgxh,string rptpath)
        {
         
            if (bglx == "")
                bglx = "CG";
            if (bgxh == "")
                bgxh = "1";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
         
            //���c:\tempĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
            {
                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                catch
                {
                }
            }

            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";
            downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath);
            if (localpath == "")
            {
                jpgname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            string sJPGNAME = localpath + "\\" +DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") +"_"+ F_blh + "_" + bglx + "_" + bgxh + ".jpg";
            //if (bglx == "" && bgxh == "")
            //    sJPGNAME = localpath + "\\" + F_blh + ".jpg";
            string  rptpath2="rpt";
            if(rptpath.Trim()!="")
                rptpath2=rptpath;
            string sBGGSName = Application.StartupPath.ToString() + "\\"+rptpath2+"\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
          
            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");
            if (inibglj != "")
            {
                sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            }


            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
            if (f.ReadInteger("rpt", "szqm", 0) == 1)
            {
                string bmppath = f.ReadString("mdbmp", "ysbmp", "d:\\pathqc\\rpt\\ysbmp").Replace("\0", "");
                stxsm = stxsm + " ,";
                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }
          
            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

            bool bcbddytx = false;
            if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
                bcbddytx = true;

            if (bglx.ToLower() == "bd")
            {
                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\����.frf";
                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                if (bcbddytx)
                    bggs = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                else
                bggs = "����.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\����.frf";
                }
                sJPGNAME = localpath + "\\" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") +"_"+F_blh.Trim() + "_" + bglx + "_" + bgxh  + ".jpg";
            }
            if (bglx.ToLower() == "bc")
            {
                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\����.frf";

                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\����.frf";
                }
                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                if (bcbddytx)
                    bggs = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                else
                    bggs = "����.frf";
                sJPGNAME = localpath + "\\" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") +"_"+ F_blh.Trim() + "_" + bglx + "_" + bgxh + ".jpg";
           
            }

            prreport pr = new prreport();
            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            jpgname = sJPGNAME.Replace(".", "_1.");



        }

        public void BMPTOJPG(string F_blh, ref string jpgname,string rptpath)
        {

   
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            //���c:\tempĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
            {
                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                catch
                {
                }
            }

            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";
            downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath);
            if (localpath == "")
            {
                jpgname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            string sJPGNAME = localpath + "\\" +  F_blh + ".jpg";

            string rptpath2 = "rpt";
            if (rptpath.Trim() != "")
                rptpath2 = rptpath;
            string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");
            if (inibglj != "")
            {
                sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            }


            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
            if (f.ReadInteger("rpt", "szqm", 0) == 1)
            {
                string bmppath = f.ReadString("mdbmp", "ysbmp", "d:\\pathqc\\rpt\\ysbmp").Replace("\0", "");
                stxsm = stxsm + " ,";
                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }

            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

           

            prreport pr = new prreport();
            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            jpgname = sJPGNAME.Replace(".", "_1.");



        }

        public static void downtx(string ftp_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath)
        {
            //���c:\temp_srĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + ftp_blh))
            {

                System.IO.Directory.CreateDirectory(@"c:\temp\" + ftp_blh);

            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + ftp_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + ftp_blh);
                }
                catch(Exception e1)
                {
                    MessageBox.Show(e1.Message.ToString());
                }
            }
            localpath = @"c:\temp\" + ftp_blh;

            //����FTP����
            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            //����Ŀ¼
            string gxml = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");


            DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + ftp_blh + "' and F_sfdy='1'", "txlb");
            string txm = "";

            if (ftps == "1")//FTP���ط�ʽ
            {

                for (int i = 0; i < txlb.Rows.Count; i++)
                {

                    txm = txlb.Rows[i]["F_txm"].ToString().Trim();


                    string ftpstatus = "";
                    try
                    {
                     
                        fw.Download(localpath, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                        if (ftpstatus == "Error")
                        {
                            log.WriteMyLog("FTP����ͼ�����");
                            localpath = "";
                            return;
                        }
                        else
                        {
                          if (f.ReadInteger("TX", "ZOOM", 0) == 1)
                            {
                                int picx = f.ReadInteger("TX", "picx", 320);
                                int picy = f.ReadInteger("TX", "picy", 240);
                                try
                                {
                                    prreport.txzoom(localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
                                }
                                catch (Exception ee2)
                                {
                                    log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                }

                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }
                    }
                    catch
                    {
                       
                        log.WriteMyLog("FTP����ͼ�����");
                    }


                }

            }
            else //�������ط�ʽ
            {
                if (txpath == "")
                {
                    log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                    return;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {

                    txm = txlb.Rows[i]["F_txm"].ToString().Trim();

                    try
                    {
                        try
                        {
                            File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                         
                                if (f.ReadInteger("TX", "ZOOM", 0) == 1)
                                {
                                    int picx = f.ReadInteger("TX", "picx", 320);
                                    int picy = f.ReadInteger("TX", "picy", 240);
                                    try
                                    {
                                        prreport.txzoom(localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
                                    }
                                    catch (Exception ee2)
                                    {
                                        log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                    }

                                }
                          
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }

                        catch
                        { }


                    }
                    catch
                    {
                        log.WriteMyLog("����Ŀ¼�����ڣ�");
                        localpath = "";
                        return;
                    }

                }

            }


        }


    }
}
namespace PathHISZGQJK
{
    class mdjpg
    {
        private LoadDllapi dllxx = new LoadDllapi();
        private static string bggs = "";
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public delegate int JPG2PDF(string jpgname, string pdfname);
        public static string szweb = "";
        public static int bgx = 0;
        public static int bgy = 0;

        public void BMPTOJPG(string F_blh, ref string jpgname, string bglx, string bgxh)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            ////���c:\tempĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
            {

                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                catch
                {
                }
            }


            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";

            downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath);
            if (localpath == "")
            {
                jpgname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            if (bgxh == "")
                bgxh = "0";
            string sJPGNAME = localpath + "\\" + F_blh + "_" + bglx + "_" + bgxh + ".jpg";

            if (bglx == "")
                sJPGNAME = localpath + "\\" + F_blh + ".jpg";


            string sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string bcbddytx = f.ReadString("bcbddytx", "bcbddytx", "").Replace("\0", "");
            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");
            if (inibglj != "")
            {

                sBGGSName = inibglj + "\\rpt\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            }


            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
            if (f.ReadInteger("rpt", "szqm", 0) == 1)
            {
                string bmppath = f.ReadString("mdbmp", "ysbmp", "d:\\pathqc\\rpt\\ysbmp").Replace("\0", "");
                stxsm = stxsm + " ,";
                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }


            //����ͼƬͳһ�ò����.jpg


            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

            if (bglx == "bd")
            {
                string rptname = "����.frf";
                if (bcbddytx == "1")
                    rptname = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

                sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + rptname;
                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                bggs = "����.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt\\" + rptname;
                }
                sJPGNAME = localpath + "\\" + F_blh.Trim() + "_bd_" + bgxh + ".jpg";
            }
            if (bglx == "bc")
            {

                string rptname = "����.frf";
                if (bcbddytx == "1")
                    rptname = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

                sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + rptname;

                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt\\" + rptname;
                }
                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                bggs = "����.frf";
                sJPGNAME = localpath + "\\" + F_blh.Trim() + "_bc_" + bgxh + ".jpg";
            }
            System.Threading.Thread.Sleep(2000);
            prreport pr = new prreport();
            //pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            pr.printjpg(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, "");
            // sJPGNAME = localpath + "\\" + F_blh.Trim() + ".bmp";
            // pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            jpgname = localpath + "\\" + F_blh.Trim() + "_1.jpg";

            System.Threading.Thread.Sleep(2000);


        }

        public void BMPTOJPG(string F_blh, ref string jpgname, string bglx, string bgxh, string rptpath)
        {

            if (bglx == "")
                bglx = "CG";
            if (bgxh == "")
                bgxh = "1";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            //���c:\tempĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
            {
                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                catch
                {
                }
            }

            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";
            downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath);
            if (localpath == "")
            {
                jpgname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            string sJPGNAME = localpath + "\\" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "_" + F_blh + "_" + bglx + "_" + bgxh + ".jpg";
            //if (bglx == "" && bgxh == "")
            //    sJPGNAME = localpath + "\\" + F_blh + ".jpg";
            string rptpath2 = "rpt";
            if (rptpath.Trim() != "")
                rptpath2 = rptpath;
            string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");
            if (inibglj != "")
            {
                sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            }


            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
            if (f.ReadInteger("rpt", "szqm", 0) == 1)
            {
                string bmppath = f.ReadString("mdbmp", "ysbmp", "d:\\pathqc\\rpt\\ysbmp").Replace("\0", "");
                stxsm = stxsm + " ,";
                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }

            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

            bool bcbddytx = false;
            if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
                bcbddytx = true;

            if (bglx.ToLower() == "bd")
            {
                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\����.frf";
                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                if (bcbddytx)
                    bggs = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                else
                    bggs = "����.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\����.frf";
                }
                sJPGNAME = localpath + "\\" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "_" + F_blh.Trim() + "_" + bglx + "_" + bgxh + ".jpg";
            }
            if (bglx.ToLower() == "bc")
            {
                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\����.frf";

                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\����.frf";
                }
                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                if (bcbddytx)
                    bggs = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                else
                    bggs = "����.frf";
                sJPGNAME = localpath + "\\" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "_" + F_blh.Trim() + "_" + bglx + "_" + bgxh + ".jpg";

            }

            prreport pr = new prreport();
            pr.printjpg(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,"");
            jpgname = sJPGNAME.Replace(".", "_1.");



        }

        public void BMPTOJPG(string F_blh, ref string jpgname, string rptpath)
        {


            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            //���c:\tempĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
            {
                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                catch
                {
                }
            }

            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";
            downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath);
            if (localpath == "")
            {
                jpgname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            string sJPGNAME = localpath + "\\" + F_blh + ".jpg";

            string rptpath2 = "rpt";
            if (rptpath.Trim() != "")
                rptpath2 = rptpath;
            string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");
            if (inibglj != "")
            {
                sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            }


            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
            if (f.ReadInteger("rpt", "szqm", 0) == 1)
            {
                string bmppath = f.ReadString("mdbmp", "ysbmp", "d:\\pathqc\\rpt\\ysbmp").Replace("\0", "");
                stxsm = stxsm + " ,";
                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }

            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";



            prreport pr = new prreport();
            pr.printjpg(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,"");
            jpgname = sJPGNAME.Replace(".", "_1.");



        }

        public static void downtx(string ftp_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath)
        {
            //���c:\temp_srĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + ftp_blh))
            {

                System.IO.Directory.CreateDirectory(@"c:\temp\" + ftp_blh);

            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + ftp_blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + ftp_blh);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message.ToString());
                }
            }
            localpath = @"c:\temp\" + ftp_blh;

            //����FTP����
            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            //����Ŀ¼
            string gxml = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");


            DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + ftp_blh + "' and F_sfdy='1'", "txlb");
            string txm = "";

            if (ftps == "1")//FTP���ط�ʽ
            {

                for (int i = 0; i < txlb.Rows.Count; i++)
                {

                    txm = txlb.Rows[i]["F_txm"].ToString().Trim();


                    string ftpstatus = "";
                    try
                    {

                        fw.Download(localpath, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                        if (ftpstatus == "Error")
                        {
                            log.WriteMyLog("FTP����ͼ�����");
                            localpath = "";
                            return;
                        }
                        else
                        {
                            if (f.ReadInteger("TX", "ZOOM", 0) == 1)
                            {
                                int picx = f.ReadInteger("TX", "picx", 320);
                                int picy = f.ReadInteger("TX", "picy", 240);
                                try
                                {
                                    prreport.txzoom(localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
                                }
                                catch (Exception ee2)
                                {
                                    log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                }

                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }
                    }
                    catch
                    {

                        log.WriteMyLog("FTP����ͼ�����");
                    }


                }

            }
            else //�������ط�ʽ
            {
                if (txpath == "")
                {
                    log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                    return;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {

                    txm = txlb.Rows[i]["F_txm"].ToString().Trim();

                    try
                    {
                        try
                        {
                            File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);

                            if (f.ReadInteger("TX", "ZOOM", 0) == 1)
                            {
                                int picx = f.ReadInteger("TX", "picx", 320);
                                int picy = f.ReadInteger("TX", "picy", 240);
                                try
                                {
                                    prreport.txzoom(localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
                                }
                                catch (Exception ee2)
                                {
                                    log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                }

                            }

                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }

                        catch
                        { }


                    }
                    catch
                    {
                        log.WriteMyLog("����Ŀ¼�����ڣ�");
                        localpath = "";
                        return;
                    }

                }

            }


        }


    }
}
