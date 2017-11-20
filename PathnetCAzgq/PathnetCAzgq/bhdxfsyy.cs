using System;
using System.Collections.Generic;
using System.Text;
using readini;
using System.Windows.Forms;
using dbbase;
using PathHISZGQJK;
using System.Data;
using LGZGQClass;

namespace PathnetCAzgq
{
    //北华大学附属医院
    class bhdxfsyy
    {
        IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private string ysbmppath = "";
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "pathnet", "4s3c2a1p");
        public string ca(string yhxx)
        {
            string msg = f.ReadString("CA", "msg", "");
            //string ftpServerIP = f.ReadString("ftp", "ftpip", "");
            //string ftpUserID = f.ReadString("ftp", "user", "");
            //string ftpPassword = f.ReadString("ftp", "pwd", "");
            //string ftpRemotePath = f.ReadString("ftp", "szqmPath", "pathimages/szqm");

       
            string getblh = "";
            string type = "";
            string type2 = "";
            string yhm = "";

            string yhmc = "";
            string yhbh = "";
            string yhmm = "";
            string bglx = "";
            string bgxh = "";
            string keyname = "";

            string[] getyhxx = yhxx.Split('^');
            if (getyhxx.Length == 5)
            {
                type = getyhxx[0];
                yhm = getyhxx[1];
                yhmc = getyhxx[3];
                yhbh = getyhxx[2];
                yhmm = getyhxx[4];
            }
            else
            {
                type2 = getyhxx[0];
                getblh = getyhxx[1];
                bgxh = getyhxx[2];
                bglx = getyhxx[3].ToLower();
                type = getyhxx[4];
                yhm = getyhxx[5];
                yhmc = getyhxx[6];
                yhbh = getyhxx[7];
                yhmm = getyhxx[8];
            }
      
             if (type == "SH")
             {
                 return calz(yhmc);
             }
             if (type == "QZ")
             {
                return caqm(yhxx);
             }
             if (type == "QXSH")
                 return "1";
           
             if (type == "QXQZ")
             {  string sqlstring = "";
                 if (bglx == "bc")
                 {
                     sqlstring = "update T_bcbg set F_qzid=' ' where F_blh='" + getblh + "' and F_bc_bgxh='" + bgxh + "'";
                 }
                 else if (bglx == "bd")
                 {
                     sqlstring = "update T_bdbg set F_qzid=' ' where F_blh='" + getblh + "' and F_bd_bgxh='" + bgxh + "'";
                 }
                 else
                 {
                     sqlstring = "update T_jcxx set F_qzid=' ' where F_blh='" + getblh + "'";
                 }
                 aa.ExecuteSQL(sqlstring);
                 return "1";
             } return "1";
        }
        public string caqm(string cslb)
        {
            if (f.ReadInteger("CA", "msg", 0) == 1)
            {
                MessageBox.Show(cslb);
            }
            
            string  ysxm = "";
            //if (cslb.Split('^')[0].ToUpper() == "SH")
            //{
                
            //    frm_bhdxfsyy newfrm2 = new frm_bhdxfsyy("sh", "");
            //    newfrm2.ShowDialog();
            //    ysxm = newfrm2.ysxm;
            //    MessageBox.Show(ysxm);
            //    if (ysxm == "0")
            //    {
            //        return "0";
            //    }
            //    //else
            //    //{
            //    //    return "1";
            //    //}
            //}
           
            string[] castr = cslb.Split('^');
            string blh = "";
            string bgxh = "";
            string bglx = "";
            string dz = "";
            string sqlstring = "";

            ysxm = "";
            if (castr.Length > 6)
            {
                blh = castr[1];
                bgxh = castr[2];
                bglx = castr[3].ToLower();
                dz = castr[4].ToUpper();
            }
        
            if (blh.Trim() == "")
                return "0";

            if (bglx == "bc")
            {
                sqlstring = "select F_blh,F_bc_bgxh,F_bczd as 病理诊断,F_qzid from T_bcbg where F_blh='" + blh + "' and F_bc_bgxh='" + bgxh + "'";
            }
            else if (bglx == "bd")
            {
                sqlstring = "select F_blh,F_bd_bgxh,F_bdzd as 病理诊断,F_qzid from T_bdbg where F_blh='" + blh + "' and F_bd_bgxh='" + bgxh + "'";
            }
            else
            {
                sqlstring = "select F_blh,F_blzd as 病理诊断,F_qzid from T_jcxx  where F_blh='" + blh + "'";
            }
      
            System.Data.DataTable dt1 = aa.GetDataTable(sqlstring, "jcxx");
            if (dt1.Rows.Count < 1)
                return "0"; //不可能出现，以防万一

            string blzd =dt1.Rows[0]["F_blh"].ToString().Trim() +"^"+ bglx +"^"+ bgxh+"^"+dt1.Rows[0]["病理诊断"].ToString().Trim();
            if (f.ReadInteger("CA", "msg", 0) == 1)
                MessageBox.Show(dt1.Rows[0]["F_qzid"].ToString().Trim());
         
            frm_bhdxfsyy newfrm = new frm_bhdxfsyy("sh", blzd);
            newfrm.ShowDialog();
            ysxm = newfrm.ysxm;
            newfrm.Close();
            newfrm.Dispose();
           
            if (ysxm == "0")
            {
                MessageBox.Show("数字签名失败！");
                return "0";
            }
            else
            {
                if (f.ReadInteger("CA", "msg", 0) == 1)
                    MessageBox.Show("数字签名成功！");
                if (bglx == "bc")
                    sqlstring = "update T_bcbg set F_qzid='" + ysxm + "' where F_blh='" + blh + "' and F_bc_bgxh='" + bgxh + "'";
                else if (bglx == "bd")
                    sqlstring = "update T_bdbg set F_qzid='" + ysxm + "' where F_blh='" + blh + "' and F_bd_bgxh='" + bgxh + "'";
                else
                    sqlstring = "update T_jcxx set F_qzid='" + ysxm + "' where F_blh='" + blh + "'";
               
               aa.ExecuteSQL(sqlstring);
           
                    return "1";
            }
        }
        public string calz(string yhmc)
        {
            frm_bhdxfsyy newfrm = new frm_bhdxfsyy("login", "");
            newfrm.ShowDialog();
            string ysxm = newfrm.ysxm;
            string SN_ID = newfrm.SN_ID;
            string SU_ID = newfrm.SU_ID;
         
                //             MessageBox.Show("获取签章图片完成，开始上传。。");
                //             //----------------------------------------------
                //             //----------------上传签章图片至ftp---------------------
                //             //----------------------------------------------
                //             string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                //             string ftpUserID = f.ReadString("ftp", "user", "");
                //             string ftpPassword = f.ReadString("ftp", "pwd", "");
                //             string ftpRemotePath = f.ReadString("ftp", "szqmPath", "/pathimages/rpt-szqm/YSBMP/");

                //             string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                //             string status = "";
                //            FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

                //             fw.Upload("D:\\pathqc\\rpt-szqm\\YSBMP\\" + keyname + ".bmp", "", out status);
                //             if (status == "Error")
                //             {
                //                 MessageBox.Show("签章图片上传失败，请重新审核！");
                //                 return "0";
                //             }
                //             if (msg == "1")
                //             MessageBox.Show("签章图片上传成功");
                //             return "1";

            if (ysxm == "0" || SN_ID == "" || SU_ID=="")
            {
                return "0";
            }
            else
            {

                if (yhmc.Trim() == ysxm.Trim())
                {
                    DataTable dt = new DataTable();
                   dt= aa.GetDataTable("select * from T_YH where F_YHMC='" + ysxm.Trim() + "'","yh");
                   if (dt.Rows.Count > 0)
                   {
                       if (dt.Rows[0]["SN_ID"].ToString() != SN_ID.Trim() || dt.Rows[0]["SU_ID"].ToString() != SU_ID.Trim())
                       {
                           MessageBox.Show("ukey信息验证未通过,请检查ukey是否过期!");
                           return "0";
                       }
                   }
                   else
                   {
                       MessageBox.Show("系统中无此ukey用户信息,不能签字!");
                       return "0";
                   }
                }
                else
                {
                    MessageBox.Show("当前用户与ukey用户不一致,不能签字!");
                    return "0";
                }

           
                if (System.IO.File.Exists("D:\\pathqc\\rpt-szqm\\ysbmp\\" + ysxm + ".bmp"))
                {
                    //----------------------------------------------
                    //----------------上传签章图片至ftp---------------------
                    //----------------------------------------------
                    string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                    string ftpUserID = f.ReadString("ftp", "user", "");
                    string ftpPassword = f.ReadString("ftp", "pwd", "");
                    string ftpRemotePath = f.ReadString("ftp", "szqmPath", "/pathimages/rpt-szqm/ysbmp/");

                    string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                    string status = "";
                    FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

                  
                    fw.Upload("D:\\pathqc\\rpt-szqm\\ysbmp\\" + ysxm + ".bmp", "", out status);
                 
                    if (status == "Error")
                    {
                        MessageBox.Show("签章图片上传失败！");
                        return "0";
                    }
                }
                //dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "pathnet", "4s3c2a1p");
                //System.Data.DataTable dt = aa.GetDataTable("select * from T_yh where F_yhmc='" + ysxm.Trim() + "'", "yhb");
                //if (dt.Rows.Count > 0)
                //{
                //    return dt.Rows[0]["F_yhm"].ToString().Trim() + "^" + dt.Rows[0]["F_yhmm"].ToString().Trim();
                //}
                //else
                //{
                //    MessageBox.Show("此用户尚未在病理系统中登记！");
                //    return "0";
                //}
                return "1";

            }


        }
    }
}
