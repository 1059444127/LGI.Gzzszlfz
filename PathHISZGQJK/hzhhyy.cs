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
    /// 杭州红会医院，体检接口，上传报告JPG，共享方式
    /// </summary>
    class hzhhyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string yymc)
        {
           
           
            string msg = f.ReadString("savetohis", "msg", "");
            string tjtxpath = f.ReadString("savetohis", "tjtxpath", "");
 if (msg == "1")
                MessageBox.Show("接受参数blh:" + blh);
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
            if (!bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("体检"))
            {
                log.WriteMyLog("非体检病人，不处理！");
                return;
            }
            if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无体检号，不处理！");
                return;
            }
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim()=="已审核")
            {
              //  string bglx = "tj"; string bgxh = "1";
                // 生成jpg格式报告文件
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
                MessageBox.Show("生成jpg成功");
                //---上传jpg----------
                //----------------上传签章jpg至ftp---------------------

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

                   
                    //判断ftp上是否存在该jpg文件
                    if (fw.fileCheckExist(ftpURI, blh + "_1.jpg"))
                    {
                        //删除ftp上的jpg文件
                        fw.fileDelete(ftpURI, blh + "_1.jpg").ToString();
                    }
                    //上传新生成的jpg文件
                    fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_1.jpg", "", out status);

                    if (status == "Error")
                    {
                        MessageBox.Show("报告jpg上传失败，请重新审核！");
                        return;
                    }
                   }
                    catch
                    {
                        if (msg == "1")
                        MessageBox.Show("上传报告jpg文件异常");
                    return;
                    }
                  }
                else
                {
                    if (tjtxpath == "")
                    {
                        log.WriteMyLog("sz.ini txpath图像目录未设置");
                        return;
                    }
                        try
                        {

                            File.Copy(ftplocal + "\\" + blh + "\\" + blh + "_1.jpg", tjtxpath + "\\" + blh + "_1.jpg", true);
                        }
                        catch(Exception  ee)
                        {
                            log.WriteMyLog("源路径：" + ftplocal + "\\" + blh + "_1.jpg" + "\n" + " 目标路径：" + tjtxpath + "\\" + "_1.jpg");
                            log.WriteMyLog("复制文件异常！"+ee.ToString());
                            return;
                        }
                    
                }
                if (msg == "1")
                    MessageBox.Show("上传报告jpg文件完成");
                    try
                    {
                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch
                    {
                        log.WriteMyLog("删除临时目录" + blh + "失败"); return;
                    }
      
              
            }
            return;
        }
    }

}
