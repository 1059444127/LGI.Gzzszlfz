using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.Runtime.InteropServices;

using System.IO;
using System.Security.Cryptography;
using ZgqClassPub;


namespace PathHISZGQJK
{
    class xjzlyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");


        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string dz)
        {
            //安全校验凭证，凭证既是接口的检验码，也是调用方的身份标识，由集成平台提供给LIS、PACS.

                bglx = bglx.ToLower();
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";

                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;

              
            try
            {
                string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "");
                string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "");
                string CAQZ = f.ReadString("savetohis", "CAQZ", "1").Replace("\0", "");
                string certificate = "7pzOrESsiv8VnB6RD2FXmndLaJCYpiY7";
                debug = f.ReadString("savetohis", "debug", "");
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable bljc = new DataTable();
                bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

                string logname = "PATHHISZGQJK";

                if (bljc == null)
                {

                    log.WriteMyLog("病理数据库设置有问题！", logname);
                    return;
                }
                if (bljc.Rows.Count < 1)
                {
                    log.WriteMyLog("病理号有错误", logname);
                    return;
                }

                string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
                DataTable dt_bc = new DataTable();
                if (bglx == "bc")
                { try
                    {
                    dt_bc = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "'  and F_bc_bgxh='" + bgxh + "'", "bc");
                    if (dt_bc.Rows.Count >= 0)
                        bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString().Trim();   
                    }
                    catch 
                    {}
                }
               
                DataTable dt_bd = new DataTable();
                if (bglx == "bd")
                {
                    try
                    {
                        dt_bd = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "'  and F_bd_bgxh='" + bgxh + "'", "bd");
                        if (dt_bc.Rows.Count >= 0)
                            bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString().Trim();
                    }
                    catch
                    {}
                }
                if (dz == "qxsh")
                    bgzt = "取消审核";
                //CA
                if (bgzt == "已审核" && CAQZ == "1")
                {
                    XJCAClass xjca = new XJCAClass();
                    xjca.CaQZ(blh, bglx, bgxh, debug, bljc, yhmc, bgzt);
                }

                if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "" || bljc.Rows[0]["F_BY2"].ToString().Trim() == "")
                {
                    log.WriteMyLog("无申请序号或医嘱项目为空,不处理！", logname);
                    return;
                }


                if (bglx == "bd")
                    return;

                try
                {
                    aa.ExecuteSQL("update [pathnet].[dbo].T_SQD set F_SQDZT='已登记'  where  F_SQXH='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "' ");
                }
                catch
                {
                }

                string ocr1 = "SC";
                string ocr5 = "IP";
                int reAuditFlag = 0;//是否重复审核
                if (bgzt == "已登记")
                {
                    ocr1 = "SC"; ocr5 = "IP";
                }
                if (bgzt == "已取材")
                {
                    ocr1 = "SC"; ocr5 = "A";
                }

                if (bgzt == "已写报告")
                {
                    ocr1 = "SC"; ocr5 = "RC";
                }
                if (bgzt == "已审核")
                {
                    ocr1 = "SC"; ocr5 = "CM";
                }
                if (bgzt == "取消审核")
                {
                    ocr1 = "OC"; ocr5 = "CM";
                }


                int F_CFSH = 0;

                try
                {
                    F_CFSH = int.Parse(bljc.Rows[0]["F_CFSH"].ToString().Trim());
                }
                catch
                {
                }

                if (bgzt == "已审核")
                {
                    if (F_CFSH >= 1)
                        reAuditFlag = 1;
                }


                if (bglx == "bc"&&bgzt == "已审核")
                {
                    reAuditFlag = 1;
                    ocr1 = "SC"; ocr5 = "CM";
                }
            


                string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();

                if (brlb == "住院") brlb = "I";
                else brlb = "O";

                string xb = bljc.Rows[0]["F_XB"].ToString().Trim();
                if (xb == "女") xb = "F";
                else if (xb.Trim() == "M男")
                    xb = "M";
                else xb = "";

                string ZYH = bljc.Rows[0]["F_ZYH"].ToString().Trim();
                if (brlb != "住院")
                    ZYH = bljc.Rows[0]["F_MZH"].ToString().Trim();

                if (brlb == "住院")
                    brlb = "I";
                else if (brlb == "急诊")
                    brlb = "E";
                else if (brlb == "体检")
                    brlb = "P";
                else
                    brlb = "O";

           

                string Status_Hl7 = "MSH|^~\\&|P05||Z01||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.7" + "\r"
                 + "PID|||" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "^" + ZYH + "||" + bljc.Rows[0]["F_XM"].ToString().Trim() + "|||" + xb + "|||||||||\r"
                 + "PV1||" + brlb + "|" + bljc.Rows[0]["F_BQ"].ToString().Trim() + "^^" + bljc.Rows[0]["F_CH"].ToString().Trim() + "||||||||||||||||" + ZYH + "\r"
                 + "ORC|" + ocr1 + "|" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "|" + blh + "||" + ocr5 + "|||||" + yhbh + "^" + yhmc + "||||||0\r"
                 + "BLG||||05^^^" + bljc.Rows[0]["F_YZXM"].ToString().Trim().Replace("^^","^") + "\r";

                try
                {
                    XJZLYYWEB.WSInterface xjzl = new XJZLYYWEB.WSInterface();
                    //string webServicesURL = f.ReadString("savetohis", "WSURL", "");
                    //if (webServicesURL.Trim() != "")
                    //    xjzl.Url = webServicesURL;

                    if (debug == "1")
                        log.WriteMyLog("状态消息:" + Status_Hl7,logname);
                    string ztstr = xjzl.CallInterface("SendMessageByHL7", Status_Hl7, yhmc, certificate);
                    if (debug == "1")
                        log.WriteMyLog("状态返回:"+ztstr, logname);
                    if (!ztstr.Contains("AA"))
                    {
                        log.WriteMyLog("回传报告状态失败！原因:" + ztstr, logname);
                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("回传报告状态成功", logname);
                    }
                }
                catch (Exception e2)
                {
                    log.WriteMyLog("回写报告状态异常：" + e2.Message, logname);
                    ZgqClass.BGHJ(blh, yhmc, "ZGQJK", "[" + bgzt + "]回写状态异常:" + e2.Message, "ZGQJK", "ZGQJK");
                }

                try
                {
                    if (bgzt == "已审核")
                        aa.ExecuteSQL("update T_JCXX set F_CFSH=F_CFSH+1 where F_BLH='" + blh + "'");
                }
                catch
                {
                }

                return;
            }
            catch
            {
            }
        }

   
    }
}
