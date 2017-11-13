
using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OracleClient;
using System.Drawing;
using ZgqClassPub;
namespace PathHISZGQJK
{
    /// <summary>
    /// //����ʡ����ҽԺ-���ӿڣ�JPG
    /// </summary>
    class hnsrmyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug)
        {

            string msg = f.ReadString("savetohis", "debug", "");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

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
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
            {

                if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "���")
                {
                    log.WriteMyLog("����첡�ˣ�������");
                    return;
                }
                if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog("�����ţ�������");
                    return;
                }


                string jpgname = "";
                string ftpstatus = "";
                //mdjpg mdj = new mdjpg();

                //try
                //{
                //    mdj.BMPTOJPG(blh, ref jpgname, "", "", "rpt//szqm");
                //}
                //catch (Exception ee)
                //{
                //    MessageBox.Show(ee.ToString());
                //}

                 mdjpg mdj = new mdjpg();
                try
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (!File.Exists("C:\\temp\\" + blh + "\\" + blh + "_1.jpg"))
                        {
                            mdj.BMPTOJPG(blh, ref jpgname, "rpt//szqm");
                        }
                        else
                        {
                            break;
                        }
                        
                    }
                }
                catch (Exception ee)
                {
                   if (msg == "1")
                        MessageBox.Show("����JPG�쳣��" + ee.Message);
                    log.WriteMyLog(blh + ",����JPG�쳣��" + ee.Message);
                }

                if (!File.Exists("C:\\temp\\" + blh + "\\" + blh + "_1.jpg"))
                {
                    if (msg == "1")
                        MessageBox.Show("δ�ҵ��ļ�" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg");
                    log.WriteMyLog(blh + ",δ�ҵ��ļ�" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg");
                    return;
                }
                else
                {
                    if (msg == "1")
                        MessageBox.Show("����jpg�ɹ�");
                }
                //---�ϴ�jpg----------
                //----------------�ϴ�ǩ��jpg��ftp---------------------

                string status = "";
                string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/bgjpg").Replace("\0", "");
                string tjtxpath = f.ReadString("savetohis", "tjtxpath", "bgjpg");
                if (ftps == "1")
                {
                    FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                    string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                    try
                    {


                        //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                        if (fw.fileCheckExist(ftpURI, blh + "_1.jpg"))
                        {
                            //ɾ��ftp�ϵ�jpg�ļ�
                            fw.fileDelete(ftpURI, blh + "_1.jpg").ToString();
                        }
                        //�ϴ������ɵ�jpg�ļ�
                        fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_1.jpg", "", out status);

                        if (status == "Error")
                        {
                            MessageBox.Show("����jpg�ϴ�ʧ�ܣ���������ˣ�");
                            return;
                        }
                    }
                    catch
                    {
                        if (msg == "1")
                            MessageBox.Show("�ϴ�����jpg�ļ��쳣");
                        return;
                    }
                }
                else
                {
                    if (tjtxpath == "")
                    {
                        log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                        return;
                    }
                    try
                    {

                        File.Copy(ftplocal + "\\" + blh + "\\" + blh + "_1.jpg", tjtxpath + "\\" + blh + "_1.jpg", true);
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("Դ·����" + ftplocal + "\\" + blh + "_1.jpg" + "\n" + " Ŀ��·����" + tjtxpath + "\\" + "_1.jpg");
                        log.WriteMyLog("�����ļ��쳣��" + ee.ToString());
                        return;
                    }

                }
                if (msg == "1")
                    MessageBox.Show("�ϴ�����jpg�ļ����");
                try
                {
                    if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                        System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                }
                catch
                {
                    log.WriteMyLog("ɾ����ʱĿ¼" + blh + "ʧ��"); return;
                }


            }
            else
            {
                if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "���")
                {
                    log.WriteMyLog("����첡�ˣ�������");
                    return;
                }
                if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog("�����ţ�������");
                    return;
                }

                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "��д����")
                {

                    string status = "";
                    string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                    string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                    string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                    string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                    string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/bgjpg").Replace("\0", "");
                    string tjtxpath = f.ReadString("savetohis", "tjtxpath", "bgjpg");
                    if (ftps == "1")
                    {
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                        string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                        try
                        {


                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            if (fw.fileCheckExist(ftpURI, blh + "_1.jpg"))
                            {
                                //ɾ��ftp�ϵ�jpg�ļ�
                                fw.fileDelete(ftpURI, blh + "_1.jpg").ToString();
                            }
                            return;

                        }
                        catch
                        {
                            if (msg == "1")
                                MessageBox.Show("����jpg�ļ�ɾ���쳣");
                            return;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                            return;
                        }
                        try
                        {

                            File.Delete(tjtxpath + "\\" + blh + "_1.jpg");
                        }
                        catch (Exception ee)
                        {

                            log.WriteMyLog("ɾ��jpg�ļ��쳣��" + ee.ToString());
                            return;
                        }
                    }

                }
            }
        }
    }
}