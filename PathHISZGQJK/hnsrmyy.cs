
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
    /// //湖南省人民医院-体检接口，JPG
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
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {

                if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "体检")
                {
                    log.WriteMyLog("非体检病人，不处理！");
                    return;
                }
                if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog("无体检号，不处理！");
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
                        MessageBox.Show("生成JPG异常：" + ee.Message);
                    log.WriteMyLog(blh + ",生成JPG异常：" + ee.Message);
                }

                if (!File.Exists("C:\\temp\\" + blh + "\\" + blh + "_1.jpg"))
                {
                    if (msg == "1")
                        MessageBox.Show("未找到文件" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg");
                    log.WriteMyLog(blh + ",未找到文件" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg");
                    return;
                }
                else
                {
                    if (msg == "1")
                        MessageBox.Show("生成jpg成功");
                }
                //---上传jpg----------
                //----------------上传签章jpg至ftp---------------------

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
                    catch (Exception ee)
                    {
                        log.WriteMyLog("源路径：" + ftplocal + "\\" + blh + "_1.jpg" + "\n" + " 目标路径：" + tjtxpath + "\\" + "_1.jpg");
                        log.WriteMyLog("复制文件异常！" + ee.ToString());
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
            else
            {
                if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "体检")
                {
                    log.WriteMyLog("非体检病人，不处理！");
                    return;
                }
                if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog("无体检号，不处理！");
                    return;
                }

                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已写报告")
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


                            //判断ftp上是否存在该jpg文件
                            if (fw.fileCheckExist(ftpURI, blh + "_1.jpg"))
                            {
                                //删除ftp上的jpg文件
                                fw.fileDelete(ftpURI, blh + "_1.jpg").ToString();
                            }
                            return;

                        }
                        catch
                        {
                            if (msg == "1")
                                MessageBox.Show("报告jpg文件删除异常");
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

                            File.Delete(tjtxpath + "\\" + blh + "_1.jpg");
                        }
                        catch (Exception ee)
                        {

                            log.WriteMyLog("删除jpg文件异常！" + ee.ToString());
                            return;
                        }
                    }

                }
            }
        }
    }
}