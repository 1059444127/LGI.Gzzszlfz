using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Windows.Forms;
using System.IO;
using ZgqClassPub;

namespace PathHISZGQJK
{
    /// <summary>
    /// ���ݺ��ҽԺ�����ӿڣ��ϴ�����JPG������ʽ
    /// </summary>
    class hzhhyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string yymc)
        {
           
           
            string msg = f.ReadString("savetohis", "msg", "");
            string tjtxpath = f.ReadString("savetohis", "tjtxpath", "");
 if (msg == "1")
                MessageBox.Show("���ܲ���blh:" + blh);
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
            if (!bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("���"))
            {
                log.WriteMyLog("����첡�ˣ�������");
                return;
            }
            if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                log.WriteMyLog("�����ţ�������");
                return;
            }
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim()=="�����")
            {
              //  string bglx = "tj"; string bgxh = "1";
                // ����jpg��ʽ�����ļ�
                string jpgname = "";
                string ftpstatus = "";
                mdjpg mdj = new mdjpg();

                try
                {
                    mdj.BMPTOJPG(blh, ref jpgname, "", "");
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
                if (msg == "1")
                MessageBox.Show("����jpg�ɹ�");
                //---�ϴ�jpg----------
                //----------------�ϴ�ǩ��jpg��ftp---------------------

                string status = "";
                string ftps = f.ReadString("ftpup_tj", "ftp", "").Replace("\0", "");
                string ftpServerIP = f.ReadString("ftpup_tj", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftpup_tj", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftpup_tj", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftpup_tj", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp_tj", "bgjpgPath", "pathimages").Replace("\0", "");
              
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
                        catch(Exception  ee)
                        {
                            log.WriteMyLog("Դ·����" + ftplocal + "\\" + blh + "_1.jpg" + "\n" + " Ŀ��·����" + tjtxpath + "\\" + "_1.jpg");
                            log.WriteMyLog("�����ļ��쳣��"+ee.ToString());
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
            return;
        }
    }

}
