using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Windows.Forms;
using System.Data;
using PathHISJK;
using HL7;
namespace GLYYTEST
{
    public class createdicom
    {
        public string cdicom(string blh,DataTable bljc,sqldb aa,ref string dcmlb)
        {
            string ftplocal = @"c:\temp_sr\" + blh;

            try
            {
                System.IO.Directory.CreateDirectory(ftplocal);
            }
            catch
            { }
            IniFiles2 f = new IniFiles2(Application.StartupPath + "\\sz.ini");
            
            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            //string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp_sr").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
            DataTable txlb = aa.GetDataTable("select top 4 * from V_DYTX where F_blh='" + blh + "' and (F_pacs IS NULL OR f_PACS<>'3')", "txlb");
            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                string ftpstatus = "";
                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                if (ftpstatus == "Error")
                {
                    log.WriteMyLog("图像下载失败！");
                    return "";
                }

            }

            

            //DataTable txb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txb");
            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                string dcmname = @"c:\temp_sr\" + blh + @"\" + i.ToString() + ".dcm";
                dicomfilezh df = new dicomfilezh();

                df.createdicom(blh, aa, @"c:\temp_sr\" + blh + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ref dcmname,"");
                
                if (dcmname == "")
                {
                    log.WriteMyLog("重新生成一次！");
                    df.createdicom(blh, aa, @"c:\temp_sr\" + blh + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ref dcmname,"");
                }
                if (dcmname != "")
                {
                    aa.ExecuteSQL("update T_tx set F_pacs='" + dcmname + "' where F_id='" + txlb.Rows[i]["F_id"].ToString().Trim() + "'");
                    dcmlb = dcmlb + dcmname + "^";
                }
            }
            return "true";
        }
    }
}
