using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using dbbase;
using PathHISJK;
using HL7;

namespace LGDICOM
{
    class psdicom
    {


     
        public bool psdicomfile(string blh)
        {

            bool dicom = true;
            IniFiles2 f = new IniFiles2(Application.StartupPath + "\\sz.ini");
            string ftplocal = f.ReadString("Dicom", "local", @"c:\temp\").Replace("\0", "") + blh;
            try
            {

                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
               
                DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "dytx");
                string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                if (System.IO.Directory.Exists(ftplocal))
                { }
                else
                {
                    System.IO.Directory.CreateDirectory(ftplocal);
                }
                DataTable bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

                string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
                int count = 0;
                log.WriteMyLog(blh + ",开始FTP图像下载");
                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    string ftpstatus = "";
                    fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                    if (ftpstatus == "Error")
                    {
                        log.WriteMyLog(blh + ",图像下载失败！" + txlb.Rows[i]["F_txm"].ToString().Trim());
                        System.IO.Directory.Delete(ftplocal, true);
                        return false;
                    }
                    log.WriteMyLog(blh + ",图像下载成功" + txlb.Rows[i]["F_txm"].ToString().Trim());

                    string bmpmc = ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim();
                    string dcmmc = ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim().Replace(".", "") + ".dcm";

                    dicomfile xx = new dicomfile();
                  string errMsg=  xx.createdicom(blh, aa, bmpmc, ref dcmmc);
                  if (errMsg != "true")
                      log.WriteMyLog("[生成idcom文件失败]" + errMsg);
                  else
                    log.WriteMyLog("[生成idcom文件名]" + dcmmc);
                    if (dcmmc != "")
                    {

                       
                        Process prc = new Process();
                        prc.StartInfo.FileName = @"cmd.exe";
                        prc.StartInfo.UseShellExecute = false;
                        prc.StartInfo.RedirectStandardInput = true;
                        prc.StartInfo.RedirectStandardOutput = true;
                        prc.StartInfo.RedirectStandardError = true;
                        prc.StartInfo.CreateNoWindow = true;

                        prc.Start();

                        string server = f.ReadString("Dicom", "server", "194.1.13.169").Replace("\0", "");
                        string port = f.ReadString("Dicom", "port", "5500").Replace("\0", "");
                        string Aec = f.ReadString("Dicom", "Aec", "KC_IPEX_P001").Replace("\0", "");

                        string dos_cmd = @"storescu -aet LGPACS -aec " + Aec + " " + server + " " + port + " " + dcmmc;

                        prc.StandardInput.WriteLine(dos_cmd);

                        prc.StandardInput.Close();

                        string output = prc.StandardOutput.ReadToEnd();
                        //string output = prc.StandardOutput.ReadLine();


                        prc.WaitForExit();
                        string err = errcode(output);
                        if (err != "")
                        {
                            log.WriteMyLog(txlb.Rows[i]["F_txm"].ToString().Trim() + ",发送失败" + err + " " + output);
                        }
                        else
                        {
                            log.WriteMyLog(txlb.Rows[i]["F_txm"].ToString().Trim() + ",发送成功 " + output);
                            count = count + 1;
                        }

                    }
                    else
                    {
                        log.WriteMyLog(blh + ",医院名称为空不处理");
                        return false;
                    }
                    
                }
              //  System.IO.Directory.Delete(ftplocal, true);
                if (count == txlb.Rows.Count)
                    return true;
                else
                    return false;
            }
            catch(Exception   ee)
            {
                log.WriteMyLog(blh + ",程序异常:" + ee.Message);
              //  System.IO.Directory.Delete(ftplocal, true);
                return false;
            }
        }

        private string errcode(string input)
        {
            //cannot access file: dicom文件不存在
            //Request Failed 
            //Association Rejected  ,Called AE Title Not Recognized, Calling AE Title Not Recognized
            //

            if (input.IndexOf("F: ") > 0)
            {
                if (input.IndexOf("cannot access file") > 0) return "dicom文件不存在！";
                if (input.IndexOf("Called AE Title Not Recognized") > 0) return "call aet不正确！";
                if (input.IndexOf("Calling AE Title Not Recognized") > 0) return "calling aet不正确！";
                if (input.IndexOf("Request Failed") > 0) return "dicom服务器连接失败！";
                return "未知错误";

            }
            else
            {

                return "";
            }
        }
    }
}
