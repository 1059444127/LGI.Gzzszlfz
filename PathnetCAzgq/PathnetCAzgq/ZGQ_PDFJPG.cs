using System;
using System.Collections.Generic;
using System.Text;
using readini;
using System.Data;
using dbbase;
using System.Windows.Forms;
using PathHISZGQJK;
using System.Net;
using System.IO;
using LGZGQClass;

namespace PathnetCAzgq
{
    class ZGQ_PDFJPG1111
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");



        private LoadDllapi dllxx = new LoadDllapi();
        private static string bggs = "";
        public delegate int JPG2PDF(string jpgname, string pdfname);
        public static string szweb = "";
        public static int bgx = 0;
        public static int bgy = 0;

        public void BMPTOJPG(string F_blh, ref string pdfname, string bglx, string bgxh)
        {
            string bglj = f.ReadString("dybg", "dybglj", "d:\\pathqc").Replace("\0", "");
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

            //������ʱĿ¼
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


            DataTable txlb = aa.GetDataTable("select top 10  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
            string txlbs = "";
            string localpath = "";
            if (txlb.Rows.Count > 0)
            {
                downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), txlb, ref txlbs, ref localpath);
            }
            else
            {
                localpath = @"c:\temp\" + F_blh;
            }

            if (localpath == "")
            {
                pdfname = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            string sJPGNAME = localpath + "\\" + F_blh.Trim() + ".pdf";
            string sBGGSName = Application.StartupPath.ToString() + "\\rpt\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string inibglj = f.ReadString("dybg", "dybglj", "").Replace("\0", "");

            log.WriteMyLog(bglj);
            log.WriteMyLog(inibglj);
            log.WriteMyLog(Application.StartupPath);

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

                string sYSZMZDLB = f.ReadString("all", "yszmlb", "f_shys");

                DataTable yszmRST = aa.GetDataTable("select " + sYSZMZDLB + " from t_jcxx where f_blh='" + F_blh + "'", "yszmb");
                string sYSZMXMLB = "";
                if (yszmRST.Rows.Count > 0)
                {
                    for (int col = 0; col < yszmRST.Columns.Count; col++)
                    {
                        if (yszmRST.Rows[0][col].ToString().Trim() != "")
                        {
                            if (sYSZMXMLB == "")
                            {
                                sYSZMXMLB = yszmRST.Rows[0][col].ToString().Trim();
                            }
                            else
                            {
                                sYSZMXMLB = sYSZMXMLB + "/" + yszmRST.Rows[0][col].ToString().Trim();
                            }
                        }

                    }
                    if (sYSZMXMLB != "")
                    {
                        sYSZMXMLB = sYSZMXMLB.Replace(",", "/");
                        string[] R = sYSZMXMLB.Split('/');
                        string xmlb = "";
                        for (int ri = 0; ri < R.Length; ri++)
                        {
                            if (xmlb == "")
                            {
                                xmlb = R[ri].ToString();
                            }
                            else
                            {
                                if (xmlb.IndexOf(R[ri]) > -1)
                                {
                                }
                                else
                                {
                                    xmlb = xmlb + "/" + R[ri];
                                }
                            }

                        }

                        string[] xinR = xmlb.Split('/');

                        for (int xin = 0; xin < xinR.Length; xin++)
                        {
                            stxsm = stxsm + " ,";
                            sbmp = sbmp + bmppath + "\\" + xinR[xin] + ".bmp,";

                        }




                    }
                    else
                    {
                        //��Ϊ����˺���ϴ������Կ϶������ҽ����
                        stxsm = stxsm + " ,";
                        sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                    }

                }
                else
                {
                    //��Ϊ����˺���ϴ������Կ϶������ҽ����
                    stxsm = stxsm + " ,";
                    sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                }






                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }
            }
            log.WriteMyLog(sBGGSName);

            //����ͼƬͳһ�ò����.jpg


            bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

            if (bglx == "bd")
            {
                sBGGSName = Application.StartupPath.ToString() + "\\rpt\\����.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt\\����.frf";
                }
                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                bggs = "����.frf";
                //sJPGNAME = localpath + "\\" + F_blh.Trim() + "_bd_" + dtbd.Rows[j]["F_bd_bgxh"].ToString() + ".jpg";
            }
            if (bglx == "bc")
            {
                sBGGSName = Application.StartupPath.ToString() + "\\rpt\\����.frf";
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\rpt\\����.frf";
                }
                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                bggs = "����.frf";
                //sJPGNAME = localpath + "\\" + F_blh.Trim() + "_bc_" + dtbc.Rows[m]["F_bc_bgxh"].ToString() + ".jpg";
            }


            prreport pr = new prreport();
            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            pdfname = localpath + "\\" + F_blh.Trim() + ".pdf";

        }

    
        public void CreateJPG(string F_blh, string bglx, string bgxh,ref string jpgName)
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
            if (txlb.Rows.Count > 0)
            {
                downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), txlb, ref txlbs, ref localpath);
            }
            if (localpath == "")
            {
                jpgName = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            if (bgxh == "")
                bgxh = "1";

            string sJPGNAME = localpath + "\\" + jpgName;



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
            prreport pr = new prreport();
            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            jpgName = localpath + "\\" + jpgName;
        }

        public void CreateJPG(string F_blh, string bglx, string bgxh, string jpgName, ref string jpgPath)
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
            if (txlb.Rows.Count > 0)
            {
                downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), txlb, ref txlbs, ref localpath);
            }
            if (localpath == "")
            {
                jpgPath = "";
                return;
            }
            string sbmp = "";
            string stxsm = "";
            if (bgxh == "")
                bgxh = "1";

            string sJPGNAME = localpath + "\\" + jpgName;



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
            prreport pr = new prreport();
            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME);
            jpgPath = localpath + "\\" + jpgName + "_1.jpg";
        }

        public static void downtx(string ftp_blh, string txml,DataTable  dt_tx, ref string txlbs, ref string localpath)
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
                catch
                {
                }
            }
            if (localpath=="")
            localpath = @"c:\temp\" + ftp_blh;
            else
            localpath = localpath + ftp_blh;

            //����FTP����
            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            //����Ŀ¼
            string gxml = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");

            //DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + ftp_blh + "' and F_sfdy='1'", "txlb");
            string txm = "";

            if (ftps == "1")//FTP���ط�ʽ
            {
                for (int i = 0; i < dt_tx.Rows.Count; i++)
                {
                    txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();
                    string ftpstatus = "";
                    try
                    {
                        fw.Download(localpath, txml + "/" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), dt_tx.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                        if (ftpstatus == "Error")
                        {
                            log.WriteMyLog("FTP����ͼ�����");
                            localpath = "";
                            return;
                        }
                        else
                        {
                            if (f.ReadInteger("TX", "ZOOM", 1) == 1)
                            {
                                int picx = f.ReadInteger("TX", "picx", 320);
                                int picy = f.ReadInteger("TX", "picy", 240);
                                try
                                {
                                    prreport.txzoom(localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
                                }
                                catch
                                {

                                }

                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
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

                for (int i = 0; i < dt_tx.Rows.Count; i++)
                {

                    txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();

                    try
                    {
                        try
                        {
                            File.Copy(txpath + txml + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), true);
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
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
        public bool CreatePDFJPG(DataTable jcxx,DataTable dt_bd,DataTable dt_bc, string blh, string bgxh, string bglx, type type1,ref string fileName,ref string filePath, ref string errmsg, string debug)
        {
           
            try
            {
                filePath = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                string blbh = blh + bgxh + bgxh;

                if (jcxx.Rows.Count < 1)
                {
                    errmsg = "������д���";
                    return false;
                }

                string bgzt = "";

                string shrq = "";
                if (bglx.ToLower() == "bd")
                {
                    if (dt_bd.Rows.Count <=0)
                    {
                        errmsg = "�������������ݣ�";
                        return false;
                    }
                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    shrq = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                }
                if (bglx.ToLower() == "bc")
                {
                    if (dt_bd.Rows.Count<=0)
                    {
                        errmsg = "������������ݣ�";
                        return false;
                    }
                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                    shrq = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
                }
                if (bglx.ToLower() == "cg")
                {
                    bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                    shrq = jcxx.Rows[0]["F_SPARE5"].ToString();
                }
                if (bgzt != "�����" && bgzt != "�ѷ���")
                {
                    errmsg = "����δ���";
                    return false;
                }
                if (bgzt == "�����" || bgzt == "�ѷ���")
                {
                    if (filePath.Trim() == "")
                    {
                        errmsg = "���ڲ���Ϊ��";
                        return false;
                    }
                    string filename = "";
                    try
                    {
                        filename = blh + "_" + bgxh + "_" + bgxh + "_" + DateTime.Parse(shrq).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                        filename = blh + "_" + bgxh + "_" + bgxh + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }

                    string rptpath = ZGQClass.GetSZ("zgqjk", "rptpath", "rpt").Replace("\0", "").Trim();

                    //����PDF
                    string ErrMsg = "";

                    bool pdf1 = false;
                    PDF_JPG(blh, bglx, bgxh, ref fileName, rptpath, type1, ref  ErrMsg);
    


                    if (File.Exists(filePath))
                    {
                        return true;
                    }
                    else
                    {
                        ZGQClass.BGHJ(blh, "����PDF", "���", "����PDFʧ�ܣ�δ�ҵ��ļ�" + filePath, "ZGQJK", "����PDF");
                        log.WriteMyLog("δ�ҵ��ļ�" + filePath);
                        errmsg = "δ�ҵ��ļ�" + filePath;
                        return false;
                    }

                }
                else
                {
                    errmsg = "����δ���";
                    return false;
                }

            }
            catch (Exception e4)
            {
                errmsg = "SC_PDF�����쳣��" + e4.Message;
                return false;
            }

        }

        public bool PDF_JPG(string F_blh, string bglx, string bgxh,ref string fileName, string rptpath, type type1, ref  string message)
        {
         
            bool rtn = false;
            try
            {
                string status = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "0";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                    return false;
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
                message = "";

                if (!downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath, ref message))
                {

                    log.WriteMyLog("����ͼƬʧ�ܣ�" + message);
                    return false;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    if (File.Exists(localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
                    {
                        message = "����ͼ�����δ�ҵ�ͼƬ��" + localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString();
                        return false;
                    }
                }

                if (localpath == "")
                {
                    message = "PDF_JPG��������ͼ�����";
                    return false;
                }

                string sbmp = "";
                string stxsm = "";
                string sJPGNAME = localpath + "\\" + fileName;

                //   string sJPGNAME = localpath + "\\" + F_blh + ".jpg";
                string rptpath2 = "rpt";
                if (rptpath.Trim() != "")
                    rptpath2 = rptpath;

                string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                string inibglj = ZGQClass.GetSZ("dybg", "dybglj", "").Replace("\0", "");

                if (inibglj.Trim() == "")
                {
                    inibglj = Application.StartupPath.ToString();
                }
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                    sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
                }
                if (ZGQClass.GetSZ("rpt", "szqm", "0") == "1" && bglx.ToLower() == "cg")
                {
                    //  string bmppath = getSZ_String("mdbmp", "ysbmp", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");

                    string bmppath = ZGQClass.GetSZ("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                    stxsm = stxsm + " ,";


                    string yszmlb = ZGQClass.GetSZ("All", "yszmlb", "f_shys");
                    bool bj = true;
                    foreach (string ysname in yszmlb.Split(','))
                    {
                        if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj == true)
                        {
                            if (ZGQClass.GetSZ("rpt", "bgys2shys", "1") == "1")
                            {
                                if (jcxx.Rows[0]["F_shys"].ToString().Trim() == jcxx.Rows[0]["F_bgys"].ToString().Trim())
                                    bj = false;
                            }

                            if (ysname.ToLower().Trim() == "f_shys")
                            {
                                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                            }

                            if (ysname.ToLower().Trim() == "f_bgys")
                            {
                                foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                            // }
                        }




                        if (ysname.ToLower().Trim() == "f_fzys")
                        {
                            foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                            {
                                if (name.Trim() != "")
                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                            }
                        }
                    }


                    sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    }
                }

                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                //  string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

                string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
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
                    sJPGNAME = localpath + "\\" + fileName;
                }
                if (bglx.ToLower() == "bc")
                {
                    DataTable BCBG = new DataTable();
                    try
                    {
                        BCBG = aa.GetDataTable("select * from T_BCBG  where F_blh='" + F_blh + "'  and F_BC_BGXH='" + bgxh + "'", "bcbg");
                    }
                    catch (Exception ex)
                    {
                        message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                        return false;
                    }


                    string bc_bggs = "����";

                    try
                    {
                        if (BCBG.Rows.Count > 0)
                        {
                            try
                            {
                                bc_bggs = BCBG.Rows[0]["F_BC_BGGS"].ToString().Trim();
                            }
                            catch
                            {
                                bc_bggs = "����";
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (bc_bggs.Trim() == "")
                        bc_bggs = "����";

                    if (bcbddytx)
                        bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ";


                    sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";

                    if (inibglj != "")
                    {


                        sBGGSName = inibglj + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";
                    }

                    if (ZGQClass.GetSZ("rpt", "bcbgszqm", "0") == "1")
                    {
                        string bmppath = ZGQClass.GetSZ("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                        stxsm = stxsm + " ,";


                        string yszmlb = ZGQClass.GetSZ("All", "yszmlb", "f_shys");



                        bool bj2 = true;
                        foreach (string ysname in yszmlb.Split(','))
                        {

                            if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj2 == true)
                            {
                                if (ZGQClass.GetSZ("rpt", "bgys2shys", "1") == "1")
                                {
                                    if (BCBG.Rows[0]["F_bc_shys"].ToString().Trim() == BCBG.Rows[0]["F_bc_bgys"].ToString().Trim())
                                        bj2 = false;
                                }
                                if (ysname.ToLower().Trim() == "f_shys")
                                {
                                    sbmp = sbmp + bmppath + "\\" + BCBG.Rows[0]["F_bc_shys"].ToString().Trim() + ".bmp,";
                                }
                                if (ysname.ToLower().Trim() == "f_bgys")
                                {
                                    foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {

                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }

                        // sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                        if (bcbddytx)
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                        else
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + ".frf";
                        if (inibglj != "")
                        {
                            if (bcbddytx)
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                            else
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + ".frf";

                        }
                    }


                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                    if (bcbddytx)
                        bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    else
                        bggs = bc_bggs + ".frf";
                    sJPGNAME = localpath + "\\" + fileName;

                }
                //�жϱ����ʽ�Ƿ����
                if (!File.Exists(sBGGSName))
                {
                    message = "�����ʽ������:" + sBGGSName;
                    return false;
                }
                string debug = f.ReadString("savetohis", "debug2", "");
                for (int x = 0; x < 3; x++)
                {

                    prreport pr = new prreport();
                    try
                    {

                        if (type1.ToString().Trim().ToLower() == "jpg")
                        {
                            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print���");
                            fileName = sJPGNAME.Replace(".", "_1.");

                        }
                        else
                        {
                            //if (debug == "1")
                            //    log.WriteMyLog("pr.print��ʼ\r\n" + sSQL_DY + "\r\n" + sbmp + "\r\n" + stxsm + "\r\n" + sBGGSName + "\r\n" + sJPGNAME);
                            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print���");
                            fileName = sJPGNAME;
                        }

                    }
                    catch (Exception e3)
                    {
                        message = "����pdf�쳣������prreport�쳣:" + e3.Message; rtn = false;
                    }

                    if (!File.Exists(fileName))
                    {
                        message = "PDF_JPG-����δ�ҵ��ļ�1";
                        rtn = false;
                        continue;
                    }
                    else
                    {
                        message = "";
                        rtn = true;
                        break;
                    }

                }
                return rtn;

            }
            catch (Exception e4)
            {
                message = "PDF_JPG�����쳣:" + e4.Message;
                return false;
            }
        }

        public bool C_PDF(string blh, string bglx, string bgxh, string ml, type type1, ref string err_msg, ref string jpgname)
        {
            try
            {
                string filename = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    err_msg = ("�������ݿ��쳣:" + ex.Message);
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "�������ݿ����������⣡";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "������д���";
                    return false;
                }

                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                string bgzt = "";

                filename = "";
                if (bglx.ToLower() == "bd")
                {
                    dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");

                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                }
                if (bglx.ToLower() == "bc")
                {
                    dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                    filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
                }
                if (bglx.ToLower() == "cg")
                {
                    bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                    filename = jcxx.Rows[0]["F_SPARE5"].ToString();
                }




                if (bgzt == "�����" || bgzt == "�ѷ���")
                {
                    if (filename.Trim() == "")
                    {
                        err_msg = "���ڲ���Ϊ��";
                        return false;
                    }

                    if (type1.ToString().ToLower() == "pdf")
                        filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
                    else
                        filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";


                    string rptpath = ZGQClass.GetSZ("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();
                    jpgname = "";

                    //����PDF
                    string msg = "";

                    bool pdf1 = CreatePDF(blh, bglx, bgxh, ref filename, rptpath.Trim(), type1, ref msg);

                    if (!pdf1)
                    {
                        err_msg = msg;
                        return false;
                    }
                    if (File.Exists(jpgname))
                        return true;
                    else
                    {
                        err_msg = "δ�ҵ�PDF�ļ�";
                        return false;
                    }
                }
                else
                {
                    err_msg = "����δ���";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "CreatePDF�����쳣��" + e4.Message;
                return false;
            }
        }


        public enum type { JPG, PDF };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="F_blh"></param>
        /// <param name="bglx"></param>
        /// <param name="bgxh"></param>
        /// <param name="jpgname"></param>
        /// <param name="rptpath"></param>
        /// <param name="type1"></param>
        /// <param name="filename"></param>
        /// <param name="message"></param>
        /// <returns></returns>

        public bool CreatePDF(string F_blh, string bglx, string bgxh, ref string filename, string rptpath, type type1, ref  string message)
        {

            bool rtn = false;
            try
            {
                string status = "";
                if (bglx == "")
                    bglx = "cg";
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
                    message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                    return false;
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
                message = "";

                if (!downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath, ref message))
                {
                    log.WriteMyLog("����ͼƬʧ�ܣ�" + message);
                    return false;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    if (File.Exists(localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
                    {
                        filename = "";
                        message = "����ͼ�����δ�ҵ�ͼƬ��" + localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString();
                        return false;
                    }
                }

                if (localpath == "")
                {
                    filename = "";
                    message = "PDF_JPG��������ͼ�����";
                    return false;
                }

                string sbmp = "";
                string stxsm = "";
                string sJPGNAME = localpath + "\\" + filename;

                //   string sJPGNAME = localpath + "\\" + F_blh + ".jpg";
                string rptpath2 = "rpt";
                if (rptpath.Trim() != "")
                    rptpath2 = rptpath;

                string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                string inibglj = ZGQClass.GetSZ("dybg", "dybglj", "").Replace("\0", "");

                if (inibglj.Trim() == "")
                {
                    inibglj = Application.StartupPath.ToString();
                }
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                    sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
                }
                if (ZGQClass.GetSZ("rpt", "szqm", "0") == "1" && bglx.ToLower() == "cg")
                {
                    //  string bmppath = ZGQClass.GetSZ("mdbmp", "ysbmp", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");

                    string bmppath = ZGQClass.GetSZ("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                    stxsm = stxsm + " ,";


                    string yszmlb = ZGQClass.GetSZ("All", "yszmlb", "f_shys");
                    bool bj = true;
                    foreach (string ysname in yszmlb.Split(','))
                    {
                        if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj == true)
                        {
                            if (ZGQClass.GetSZ("rpt", "bgys2shys", "1") == "1")
                            {
                                if (jcxx.Rows[0]["F_shys"].ToString().Trim() == jcxx.Rows[0]["F_bgys"].ToString().Trim())
                                    bj = false;
                            }

                            if (ysname.ToLower().Trim() == "f_shys")
                            {
                                sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                            }

                            if (ysname.ToLower().Trim() == "f_bgys")
                            {
                                foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                            // }
                        }




                        if (ysname.ToLower().Trim() == "f_fzys")
                        {
                            foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                            {
                                if (name.Trim() != "")
                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                            }
                        }
                    }


                    sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    }
                }

                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                //  string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

                string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
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
                    sJPGNAME = localpath + "\\" + filename;
                }
                if (bglx.ToLower() == "bc")
                {
                    DataTable BCBG = new DataTable();
                    try
                    {
                        BCBG = aa.GetDataTable("select * from T_BCBG  where F_blh='" + F_blh + "'  and F_BC_BGXH='" + bgxh + "'", "bcbg");
                    }
                    catch (Exception ex)
                    {
                        message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                        return false;
                    }


                    string bc_bggs = "����";

                    try
                    {
                        if (BCBG.Rows.Count > 0)
                        {
                            try
                            {
                                bc_bggs = BCBG.Rows[0]["F_BC_BGGS"].ToString().Trim();
                            }
                            catch
                            {
                                bc_bggs = "����";
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (bc_bggs.Trim() == "")
                        bc_bggs = "����";

                    if (bcbddytx)
                        bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ";


                    sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";

                    if (inibglj != "")
                    {


                        sBGGSName = inibglj + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";
                    }

                    if (ZGQClass.GetSZ("rpt", "bcbgszqm", "0") == "1")
                    {
                        string bmppath = ZGQClass.GetSZ("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                        stxsm = stxsm + " ,";


                        string yszmlb = ZGQClass.GetSZ("All", "yszmlb", "f_shys");



                        bool bj2 = true;
                        foreach (string ysname in yszmlb.Split(','))
                        {

                            if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj2 == true)
                            {
                                if (ZGQClass.GetSZ("rpt", "bgys2shys", "1") == "1")
                                {
                                    if (BCBG.Rows[0]["F_bc_shys"].ToString().Trim() == BCBG.Rows[0]["F_bc_bgys"].ToString().Trim())
                                        bj2 = false;
                                }
                                if (ysname.ToLower().Trim() == "f_shys")
                                {
                                    sbmp = sbmp + bmppath + "\\" + BCBG.Rows[0]["F_bc_shys"].ToString().Trim() + ".bmp,";
                                }
                                if (ysname.ToLower().Trim() == "f_bgys")
                                {
                                    foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {

                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }

                        // sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                        if (bcbddytx)
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                        else
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + ".frf";
                        if (inibglj != "")
                        {
                            if (bcbddytx)
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                            else
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + ".frf";

                        }
                    }


                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                    if (bcbddytx)
                        bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    else
                        bggs = bc_bggs + ".frf";
                    sJPGNAME = localpath + "\\" + filename;

                }
                //�жϱ����ʽ�Ƿ����
                if (!File.Exists(sBGGSName))
                {
                    message = "�����ʽ������:" + sBGGSName;
                    return false;
                }
                string debug = f.ReadString("savetohis", "debug2", "");
                for (int x = 0; x < 3; x++)
                {

                    prreport pr = new prreport();
                    try
                    {

                        if (type1.ToString().Trim().ToLower() == "jpg")
                        {
                            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print���");
                            filename = sJPGNAME.Replace(".", "_1.");

                        }
                        else
                        {
                            //if (debug == "1")
                            //    log.WriteMyLog("pr.print��ʼ\r\n" + sSQL_DY + "\r\n" + sbmp + "\r\n" + stxsm + "\r\n" + sBGGSName + "\r\n" + sJPGNAME);
                            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print���");
                            filename = sJPGNAME;
                        }

                    }
                    catch (Exception e3)
                    {
                        message = "����pdf�쳣������prreport�쳣:" + e3.Message; rtn = false;
                    }

                    if (!File.Exists(filename))
                    {
                        message = "PDF_JPG-����δ�ҵ��ļ�1";
                        rtn = false;
                        continue;
                    }
                    else
                    {
                        message = "";
                        rtn = true;
                        break;
                    }

                }
                return rtn;

            }
            catch (Exception e4)
            {
                message = "PDF_JPG�����쳣:" + e4.Message;
                return false;
            }
        }

        public bool downtx(string f_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath, ref string err_msg)
        {
            try
            {
                //���c:\tempĿ¼
                if (!System.IO.Directory.Exists(@"c:\temp\" + f_blh))
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + f_blh);
                else
                {
                    try
                    {
                        System.IO.Directory.Delete(@"c:\temp\" + f_blh, true);
                        System.IO.Directory.CreateDirectory(@"c:\temp\" + f_blh);
                    }
                    catch (Exception e1)
                    {
                    }
                }
                //��ʱĿ¼
                localpath = @"c:\temp\" + f_blh;

                //����FTP����
                string ftpserver = ZGQClass.GetSZ("ftp", "ftpip", "").Replace("\0", "");
                string ftpuser = ZGQClass.GetSZ("ftp", "user", "ftpuser").Replace("\0", "");
                string ftppwd = ZGQClass.GetSZ("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = ZGQClass.GetSZ("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
                string ftpremotepath = ZGQClass.GetSZ("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                string ftps = ZGQClass.GetSZ("ftp", "ftp", "").Replace("\0", "");
                string txpath = ZGQClass.GetSZ("txpath", "txpath", "").Replace("\0", "");
                //����Ŀ¼
                string gxml = ZGQClass.GetSZ("txpath", "txpath", "").Replace("\0", "");
                //string gxuid = ZGQClass.GetSZ("txpath", "username", "").Replace("\0", "");
                //string gxpwd = ZGQClass.GetSZ("txpath", "password", "").Replace("\0", "");


                DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + f_blh + "' and F_sfdy='1'", "txlb");
                string txm = "";

                if (ftps == "1")//FTP���ط�ʽ
                {
                    FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
                    for (int i = 0; i < txlb.Rows.Count; i++)
                    {

                        txm = txlb.Rows[i]["F_txm"].ToString().Trim();
                        string ftpstatus = "";
                        try
                        {
                           
                            err_msg = "";
                            fw.Download(localpath, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                     
                        
                                if (ftpstatus != "Error")
                                    break;
               

                            if (ftpstatus == "Error")
                            {
                                localpath = "";
                                return false;
                            }
                            else
                            {

                                if (f.ReadInteger("TX", "ZOOM", 1) == 1)
                                {

                                    int picx = f.ReadInteger("TX", "picx", 320);
                                    int picy = f.ReadInteger("TX", "picy", 240);
                                    try
                                    {

                                        prreport.txzoom(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);

                                    }
                                    catch (Exception ee2)
                                    {
                                        log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                    }

                                }
                                txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";

                            }
                        }
                        catch (Exception ee2)
                        {
                            err_msg = "FTP����ͼ�����:" + ee2.Message;
                            localpath = "";
                            return false;
                        }
                    }

                    return true;
                }
                else //�������ط�ʽ
                {
                    if (txpath == "")
                    {
                        err_msg = "sz.ini txpathͼ��Ŀ¼δ����";
                        localpath = "";
                        return false;
                    }
                    for (int i = 0; i < txlb.Rows.Count; i++)
                    {
                        txm = txlb.Rows[i]["F_txm"].ToString().Trim();

                        try
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                                if (File.Exists(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim()))
                                    break;
                            }
                            if (File.Exists(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim()))
                            {
                                if (f.ReadInteger("TX", "ZOOM", 1) == 1)
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
                            }
                            else
                            {
                                err_msg = "��������ͼ��ʧ��";
                                return false;
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }

                        catch (Exception ee3)
                        {
                            err_msg = "��������ͼ�����:" + ee3.Message;
                            localpath = "";
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception e4)
            {

                err_msg = "����ͼ���쳣:" + e4.Message;
                return false;
            }

        }

        /// 
        /// <summary>
        /// �ϴ�
        /// </summary>
        /// <param name="blh">�����</param>
        /// <param name="jpgpath">�ļ���������·��</param>
        /// <param name="ml">Ŀ¼</param>
        /// <param name="err_msg">������Ϣ</param>
        /// <param name="lb">1:ָ������Ŀ¼�ϴ���[ZGQJK]toPDFPath=)��2:����·���ϴ�([txpath]txpath=)��3:ftp�ϴ�([ftp]);����ftp�ϴ�([ftpup])  </param>
        /// <returns></returns>
        /// 
        public bool UpFtp(string blh, string jpgpath, string ml, int lb, ref string err_msg, ref string pdfpath)
        {
            try
            {
                string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
                //---�ϴ�jpg----------
                //----------------�ϴ���ftp---------------------
                string status = "";
                string ftps = string.Empty;
                string ftpServerIP = string.Empty;
                string ftpUserID = string.Empty; ;
                string ftpPassword = string.Empty;
                string ftplocal = string.Empty;
                string ftpRemotePath = string.Empty;
                string tjtxpath = ZGQClass.GetSZ("savetohis", "PDFPath", "");
                string debug = ZGQClass.GetSZ("savetohis", "debug", "0");
                string txpath = ZGQClass.GetSZ("txpath", "txpath", @"E:\pathimages\");


                if (lb == 0)
                {
                    if (ZGQClass.GetSZ("ftp", "ftp", "").Replace("\0", "").Trim() == "1")
                        lb = 3;
                    else
                    {
                        lb = 1;
                    }

                }
                if (lb == 1)
                {
                    ftps = "0";
                    if (tjtxpath == "")
                        tjtxpath = txpath + "pdfbg";
                    else
                        tjtxpath = txpath + tjtxpath;
                }

                if (lb == 3)
                {
                    ftps = ZGQClass.GetSZ("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = ZGQClass.GetSZ("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZGQClass.GetSZ("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZGQClass.GetSZ("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZGQClass.GetSZ("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZGQClass.GetSZ("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
                }
                if (lb == 4)
                {
                    ftps = ZGQClass.GetSZ("ftpup", "ftp", "1").Replace("\0", "");
                    ftpServerIP = ZGQClass.GetSZ("ftpup", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZGQClass.GetSZ("ftpup", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZGQClass.GetSZ("ftpup", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZGQClass.GetSZ("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZGQClass.GetSZ("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");

                }
                if (File.Exists(jpgpath))
                {
                    if (ftps == "1")
                    {
                       
                        string ftpURI = @"ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                        try
                        {

                            if (debug == "1")
                                log.WriteMyLog("���ftpĿ¼������");

                            //�յ�����Ŀ¼
                            if (ml.Trim() != "")
                            {
                                //�ж�Ŀ¼�Ƿ����
                                if (!ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword,ftpURI,ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                   
                                    if ( ZgqFtpWeb.FtpMakedir(ftpUserID, ftpPassword,ftpURI, ml, ref  err_msg))
                                    {
                                        err_msg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                            //�����Ŀ¼
                            //�ж�Ŀ¼�Ƿ����
                            // MessageBox.Show("1--"+ftpURI);
                            if (blh.Trim() != "")
                            {
                                
                                if (!ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword,ftpURI, blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";

                                    if (ZgqFtpWeb.FtpMakedir(ftpUserID, ftpPassword, ftpURI, blh, ref  err_msg))
                                    {
                                        err_msg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI + blh + "/";
                            }


                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            for (int x = 0; x < 3; x++)
                            {
                                
                                if (ZgqFtpWeb.FtpUpload(ftpUserID, ftpPassword,ftpURI, jpgpath,ref err_msg))
                                    break;
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + err_msg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {
                                if (ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword,ftpURI,jpgname))
                                {
                                    status = "OK";
                                    pdfpath = ftpURI + jpgname;
                                    return true;
                                }
                                else
                                {
                                    err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                    return false;
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + @"\" + ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + @"\" + ml);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + @"\" + ml + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + @"\" + ml;
                            }
                            if (blh.Trim() != "")
                            {
                                //�ж�blhĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + blh + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + blh;
                            }

                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //ɾ�������ϵ�pdf�ļ�
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            for (int x = 0; x < 3; x++)
                            {
                                File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                                if (File.Exists(tjtxpath + "\\" + jpgname))
                                    break;
                            }
                   ;
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                status = "OK";
                                pdfpath = tjtxpath + "\\" + jpgname;
                            }
                            else
                            {
                                status = "";
                                err_msg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
                            return false;
                        }
                    }

                    if (status == "OK")
                        return true;
                    else
                        return false;
                }
                else
                {
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="ml">Ŀ¼</param>
        /// <param name="jpgname">�ļ���</param>
        /// <param name="err_msg">������Ϣ</param>
        /// <returns></returns>
        public bool DelFtp(string ml, string jpgname, ref string err_msg)
        {

            string status = "";
            string ftps = ZGQClass.GetSZ("ftp", "ftp", "").Replace("\0", "");
            string ftpServerIP = ZGQClass.GetSZ("ftp", "ftpip", "").Replace("\0", "");
            string ftpUserID = ZGQClass.GetSZ("ftp", "user", "ftpuser").Replace("\0", "");
            string ftpPassword = ZGQClass.GetSZ("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = ZGQClass.GetSZ("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpRemotePath = ZGQClass.GetSZ("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
            string tjtxpath = ZGQClass.GetSZ("JK_ZGQ", "PDFPath", "e:\\pathimages\\jpgbg");

            if (ftps == "1")
            {
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";

                try
                {

                    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                    if (fw.fileCheckExist(ftpURI, jpgname))
                    {
                        //ɾ��ftp�ϵ�jpg�ļ�
                        fw.fileDelete(ftpURI, jpgname).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    err_msg = "ɾ��ftp��PDF�쳣:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    err_msg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
                    return false;
                }
                try
                {
                    if (ml == "")
                        File.Delete(tjtxpath + "\\" + jpgname);
                    else
                        File.Delete(tjtxpath + "\\" + ml + "\\" + jpgname);

                    return true;
                }
                catch (Exception ee)
                {
                    err_msg = "ɾ���ļ��쳣:" + ee.Message.ToString();
                    return false;
                }

            }

        }

        public void DeleteTempFile(string blh)
        {
            try
            {
                System.IO.Directory.Delete(@"c:\temp\" + blh, true);
            }
            catch
            {
            }
        }

        public bool UpPDF(string blh, string jpgpath, string ml, ref string err_msg, int lb, ref string pdfpath)
        {
            try
            {
                pdfpath = "";
                err_msg = "";
                string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
                //---�ϴ�jpg----------
                //----------------�ϴ���ftp---------------------
                string status = "";
                string ftps = ZGQClass.GetSZ("ftpup", "ftp", "").Replace("\0", "");
                string ftpServerIP = ZGQClass.GetSZ("ftpup", "ftpip", "").Replace("\0", "");
                string ftpUserID = ZGQClass.GetSZ("ftpup", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = ZGQClass.GetSZ("ftpup", "pwd", "ftp").Replace("\0", "");
                string ftplocal = ZGQClass.GetSZ("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = ZGQClass.GetSZ("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");
                string tjtxpath = ZGQClass.GetSZ("savetohis", "toPDFPath", @"\\192.0.19.147\GMS");
                string debug = ZGQClass.GetSZ("savetohis", "debug", "0");

                string txpath = ZGQClass.GetSZ("txpath", "txpath", @"E:\pathimages");

                if (lb == 3)
                {
                    ftps = ZGQClass.GetSZ("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = ZGQClass.GetSZ("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZGQClass.GetSZ("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZGQClass.GetSZ("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZGQClass.GetSZ("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZGQClass.GetSZ("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
                }
                if (lb == 1)
                {
                    ftps = "0";
                }
                if (lb == 2)
                {
                    ftps = "0"; tjtxpath = txpath;
                }


                if (File.Exists(jpgpath))
                {
                    if (ftps == "1")
                    {
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                        string ftpURI = @"ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                        try
                        {

                            if (debug == "1")
                                log.WriteMyLog("���ftpĿ¼������");

                            //�յ�����Ŀ¼
                            if (ml.Trim() != "")
                            {
                                //�ж�Ŀ¼�Ƿ����
                                if (!fw.fileCheckExist(ftpURI, ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    fw.Makedir(ml, out stat);
                                    if (stat != "OK")
                                    {
                                        err_msg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                            //�����Ŀ¼
                            //�ж�Ŀ¼�Ƿ����
                            // MessageBox.Show("1--"+ftpURI);
                            if (!fw.fileCheckExist(ftpURI, blh))
                            {
                                //Ŀ¼�����ڣ�����
                                string stat = "";

                                fw.Makedir(ftpURI, blh, out stat);

                                if (stat != "OK")
                                {
                                    err_msg = "FTP����Ŀ¼�쳣";
                                    return false;
                                }
                            }
                            ftpURI = ftpURI + "/" + blh + "/";

                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            fw.Upload(jpgpath, ml + "/" + blh, out status, ref err_msg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + err_msg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                    pdfpath = ftpURI + "/" + jpgname;
                                }
                                else
                                {
                                    err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml + "\\" + blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml + "\\" + blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + ml + "\\" + blh;
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //ɾ�������ϵ�pdf�ļ�
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�

                            File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                            // Thread.Sleep(1000);
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                status = "OK";
                                pdfpath = tjtxpath + "\\" + jpgname;
                            }
                            else
                            {
                                err_msg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
                            return false;
                        }
                    }

                    if (status == "OK")
                    {

                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// 

        public bool FtpDownload(string ftpUserID, string ftpPassword, string ftpURL, string fileName, string localPath, string localName, ref string ErrMsg)
        {
       
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath + "\\" + localName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message);
                ErrMsg = "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message;
                return false;
            }
        }

        public bool FtpDownload(string ftpUserID, string ftpPassword, string ftpPath, string localPath, ref string err_msg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                err_msg = "Download Error -->" + localPath + "-->" + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="status"></param>

        public bool FtpMakedir(string ftpUserID, string ftpPassword, string ftpURI, string dirname, ref  string ErrMsg)
        {

            string uri = ftpURI + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                return false;
            }
        }

   
        /// <summary>
        /// �ϴ�
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="status"></param>
        /// <param name="msg"></param>

        public bool FtpUpload(string ftpUserID, string ftpPassword, string ftpURI, string filename, string ml, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + ml + "/" + fileInf.Name;
            if (ml == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public bool FtpUpload(string ftpUserID, string ftpPassword, string ftpURI, string filename, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }
   


        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filePath"></param>

        public bool FtpDelete(string ftpUserID, string ftpPassword, string ftpURL, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpURL + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public bool FtpDelete(string ftpUserID, string ftpPassword, string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }

        /// <summary>
        /// �ļ����ڼ��
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>

        public bool FtpCheckFile(string ftpUserID,string ftpPassword,string ftpPath, string ftpName)
        {
            bool success = false;
            string url = ftpPath;
          
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {

                }
            }
            return success;

        }

        public class Insert_Standard_ErrorLog
        {
            public static void Insert(string x, string y)
            {
                //  MessageBox.Show(y);
                log.WriteMyLog(y);
                Application.Exit();
            }
        }
    }

    class ZgqFtpWeb
    {
          string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        /// <summary>
        /// ����FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP���ӵ�ַ</param>
        /// <param name="FtpRemotePath">ָ��FTP���ӳɹ���ĵ�ǰĿ¼, �����ָ����Ĭ��Ϊ��Ŀ¼</param>
        /// <param name="FtpUserID">�û���</param>
        /// <param name="FtpPassword">����</param>
        public ZgqFtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            if (FtpRemotePath != "")
            {
                ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            }
            else
            {
                ftpURI = "ftp://" + ftpServerIP + "/";
            }
           
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// 

        public bool FtpDownload(string ftpURL,string ml, string fileName, string localPath, string localName, ref string ErrMsg)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath + "\\" + localName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI +ml+ fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message);
                ErrMsg = "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message;
                return false;
            }
        }

        public bool FtpDownload(string ftpPath, string localPath, ref string err_msg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                err_msg = "Download Error -->" + localPath + "-->" + ex.Message;
                return false;
            }
        }

        public static  bool FtpDownload(string ftpUser, string ftpPwd, string ftpURL, string fileName, string localPath, string localName, ref string ErrMsg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath + "\\" + localName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message);
                ErrMsg = "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message;
                return false;
            }
        }

        public static bool FtpDownload(string ftpUser, string ftpPwd, string ftpPath, string localPath, ref string err_msg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                err_msg = "Download Error -->" + localPath + "-->" + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="status"></param>
        public bool FtpMakedir(string ftpURI, string dirname, ref  string ErrMsg)
        {

            string uri = ftpURI + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                return false;
            }
        }

        public static bool FtpMakedir(string ftpUser, string ftpPwd, string ftpURI, string dirname, ref  string ErrMsg)
        {

            string uri = ftpURI + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// �ϴ�
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        public bool FtpUpload(string filename, string ml, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + ml + "/" + fileInf.Name;
            if (ml == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public bool FtpUpload(string ftpURI, string ml, string filename, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + ml + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public static bool FtpUpload(string ftpUser, string ftpPwd, string ftpURI, string filename, string ml, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + ml + "/" + fileInf.Name;
            if (ml == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public static bool FtpUpload(string ftpUser, string ftpPwd, string ftpURI, string filename, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }



        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filePath"></param>
        public bool FtpDelete(string ftpURL, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpURL + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public bool FtpDelete(string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public static bool FtpDelete(string ftpUser, string ftpPwd, string ftpURL, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpURL + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public static bool FtpDelete(string ftpUser, string ftpPwd, string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }

        /// <summary>
        /// �ļ����ڼ��
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public bool FtpCheckFile(string ftpPath, string ftpName)
        {
          
            string url = ftpPath;
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {

                }
            }
            return success;

        }

        public static bool FtpCheckFile(string ftpUser, string ftpPwd, string ftpPath, string ftpName)
        {
         
            string url = ftpPath;
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {

                }
            }
            return success;

        }

        public class Insert_Standard_ErrorLog
        {
            public static void Insert(string x, string y)
            {
                //  MessageBox.Show(y);
                log.WriteMyLog(y);
                Application.Exit();
            }
        }
    }
}
