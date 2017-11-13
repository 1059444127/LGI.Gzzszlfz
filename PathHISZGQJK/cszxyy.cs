
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
using System.Xml;
using System.Xml.XPath;
using System.Net;
using ZgqClassPub;


namespace PathHISZGQJK
 {   class cszxyy
    {
     private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
     
     public void pdf(string blh, string bglx, string bgxh, string msg1, string debug, string[] cslb)
        {
 
            try
            {
                
                debug = f.ReadString("savetohis", "debug", "");
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


                string qxsh = "";
                string xdj = "";
                bglx = bglx.ToLower();

                if (cslb.Length == 5)
                {
                    if (cslb[4].ToLower() == "qxsh")
                    {
                        //取消审核动作
                        qxsh = "1";
                    }
                }

                if (bglx == "")
                    bglx = "cg";

                if (bgxh == "")
                    bgxh = "0";

                if (bglx.ToLower().Trim() == "bd")
                    return;

                string bgzt2 = "";

                try
                {
                    if (bglx.ToLower().Trim() == "bd")
                    {
                        DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    }
                    if (bglx.ToLower().Trim() == "bc")
                    {
                        DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                        bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                    }
                    if (bglx.ToLower().Trim() == "cg")
                    {
                        // DataTable jcxx2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                        bgzt2 = bljc.Rows[0]["F_BGZT"].ToString();
                    }
                }
                catch
                {}
                if (qxsh == "1")
                    bgzt2 = "取消审核";
                string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                //生成pdf
                if (bgzt2 == "已审核" || bgzt2 == "取消审核")
                    scpdf(blh, bglx, bgxh, bgzt2, ML);
               
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
            }
        

        }

     private void scpdf(string blh, string bglx, string bgxh, string bgzt2, string ML)
     {
         try
         {
             dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
             if (bgzt2.Trim() == "")
             {
                 log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);
             }

             if (bgzt2.Trim() == "已审核")
             {

            
                 //string ML = "";
                 string message = ""; string jpgname = "";
                 ZgqPDFJPG zgq = new ZgqPDFJPG();
                 bool isrtn = zgq.CreatePDF(blh, bglx, bgxh,ZgqPDFJPG.Type.PDF  ,  ref jpgname,"", ref message);

                 string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                 if (isrtn)
                 {
                     bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                     if (ssa == true)
                     {
                         jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                         ZgqClass.BGHJ(blh, "批量上传", "审核", "生成PDF成功:" + ML + "\\" + jpgname, "ZGQJK", "生成PDF");

                         aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                         aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                         aa.ExecuteSQL("update T_JCXX_FS set F_bz='生成PDF成功',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                     }
                     else
                     {
                         log.WriteMyLog(message);
                         ZgqClass.BGHJ(blh, "批量上传", "审核", message, "ZGQJK", "生成PDF");
                         aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_bz='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                     }
                     zgq.DelTempFile(blh);
                 }
                 else
                     aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                 return;

             }
             else
             {
                 if (bgzt2 == "取消审核")
                 {
                     DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                     if (dt2.Rows.Count <= 0)
                         aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='取消审核,删除PDF成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt2 + "'");
                     else
                     {
                         aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                         ZgqPDFJPG zgq = new ZgqPDFJPG();
                         string rtn_msg = "";
                         zgq.DelPDFFile(dt2.Rows[0]["F_ml"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
                         aa.ExecuteSQL("update T_JCXX_FS set F_bz='取消审核,删除PDF成功！',F_JPG_errmsg='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                     }
                     return;
                 }
             }
             return;
         }
         catch
         {
         }
     }

     public void pathtohis(string blh, string yymc,string bglx)
     {
         string pathweburl = f.ReadString("savetohis", "weburl", "");

         string msg = f.ReadString("savetohis", "msg", "");
         string debug = f.ReadString("savetohis", "debug", "");


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
         if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
         {
             log.WriteMyLog("无申请序号（单据号），不处理！");
             return;
         }


         if (bglx.ToLower().Trim() == "bd")
             return;
         if (bglx.ToLower().Trim() == "bc")
             return;
         //DataTable T_SZ = new DataTable();
         //T_SZ = aa.GetDataTable("select F_SZZ,F_XL from T_SZ where F_DL='JK' and (F_XL='JK_MSG')", "T_SZ");

         //for (int y = 0; y < T_SZ.Rows.Count; y++)
         //{
         //    if (T_SZ.Rows[0]["F_XL"].ToString().Trim() == "JK_MSG")
         //    {
         //        if (msg.Trim() == "")
         //            msg = T_SZ.Rows[0]["F_SZZ"].ToString().Trim();
         //    }
         //}

         //------------------------------------------------------------------------------
         //--------------------体检回传-------------------------------------------------
         if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("体检"))
         {
             if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核")
             {
                 DataTable TJ_bljc = new DataTable();
                 TJ_bljc = aa.GetDataTable(" select F_TBSzd,F_TBS_BCYJ1,F_TBS_WSW2,F_TBS_WSW1,F_TBS_WSW3,F_TBS_BDXM1,F_TBS_YZCD,F_TBS_FXJ,F_TBS_NZJ,F_TBS_XJ,F_TBS_QT  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");


                 string yzlx = "1";
                 string yzid = bljc.Rows[0]["F_sqxh"].ToString().Trim();	//查询语句的ID
                 string bgsj = bljc.Rows[0]["F_BGRQ"].ToString().Trim();
                 string bgrq = bgsj.Replace('-', '/');	//报告日期 yyyy/mm/dd	
                 string zsms = "";    //看到
                 //诊断
                 string blzd = "诊断:" + bljc.Rows[0]["F_blzd"].ToString().Trim();
                 if (TJ_bljc.Rows.Count > 0)
                 {

                     if (bljc.Rows[0]["F_blk"].ToString().Contains("TCT"))
                     {
                         //病原微生物
                         zsms = TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_FXJ"].ToString().Trim() + ";   " + TJ_bljc.Rows[0]["F_TBS_NZJ"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_XJ"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_QT"].ToString().Trim() + ";";

                         //炎症程度
                         string yzcd = "炎症程度:" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + ";";

                         blzd = yzcd + "  " + blzd;

                     }
                     if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                         blzd = blzd + "   建议:" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim();

                     if (bljc.Rows[0]["F_blk"].ToString().Contains("HPV"))
                     {
                         blzd = TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString()
                   + "\r\n" + TJ_bljc.Rows[0]["F_TBSzd"].ToString();
                         zsms = blzd;
                     }
                 }


                 string yxzd = blzd;	//结论	
                 string shys = bljc.Rows[0]["F_SHYS"].ToString().Trim();     //发报告医生
                 string bgks = "病理科";     //报告科室
                 string MSG = "";
                 try
                 {
                   //  cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();
                 cszxyyweb2.NYwclService  cs=new PathHISZGQJK.cszxyyweb2.NYwclService();
                 cs.Url = "http://192.168.3.19:8080/Services/NYwclService?wsdl";

                     if (pathweburl != "")
                         cs.Url = pathweburl;

                   //  MSG = cs.SetJcjg(yzlx, yzid, bgrq, zsms, yxzd, shys, bgks);
                    MSG = cs.SetJcjgb(yzlx, yzid, bgrq, zsms, yxzd, shys, bgks);

                     if (MSG == "1")
                     {
                         if (debug == "1")
                             MessageBox.Show("回传体检信息成功！");
                         //      return;

                     }
                     else
                     {
                         if (msg == "1")
                             MessageBox.Show("回传体检信息失败！原因：" + MSG);
                         log.WriteMyLog("体检，" + bljc.Rows[0]["F_blh"].ToString().Trim() + "，" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "，回传的体检错误：" + MSG);
                         //    return;
                     }
                 }
                 catch (Exception ee)
                 {
                     if (msg == "1")
                         MessageBox.Show("回传体检信息异常：" + ee.ToString());
                     log.WriteMyLog("回传体检信息异常：" + ee.ToString());
                     return;
                 }

             }
             return;
         }
         ///////////////////////////////////////////////////////////////////////////////
         //////////////////////////////////////////////////////////////////////////////
         //------------------------------------------------------------------------------
         string xmlstr1 = "";
         string xmlstr2 = "";
         if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("门诊"))
         {
             xmlstr1 = "his_clinic";
             xmlstr2 = "20100425";//20100456回报告
         }
         if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("住院"))
         {
             xmlstr1 = "his_iinhos";
             xmlstr2 = "20100424";//20100457回报告
         }
         if (xmlstr1 == "")
         {
             log.WriteMyLog("病人类别为空，不处理");
             return;
         }

         //---------------------------------------------------------------------------------------
         //-------------------------非体检回传-------------------------------------------------------
         string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

         string sqzt = bljc.Rows[0]["F_sqzt"].ToString().Trim();
         string sqbj = "0";
         if (sqzt == "2")
         {
             //不写标记
         }
         else
         {

             if (bgzt != "已审核")
             {
                 if (sqzt == "")
                 {
                     sqbj = "1";//要执行写标记
                 }
                 else
                 {
                     //不写标记
                 }
             }
         }
         if (sqbj == "1")
         {

             string bgztID = "";

             if (bgzt == "已审核")
             {
                 bgztID = "2";
             }
             else
             {
                 bgztID = "1";
             }
             string inxml = "";
             inxml = inxml + "<?xml version='1.0' encoding='gb2312' standalone='no' ?>";
             inxml = inxml + "<root>";
             inxml = inxml + "<arg1>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</arg1>";
             inxml = inxml + "<arg2>" + bgztID + "</arg2>";
             inxml = inxml + "<arg3>" + bljc.Rows[0]["F_JSY"].ToString().Trim() + "</arg3>";
             inxml = inxml + "<arg4></arg4><arg5></arg5>";
             inxml = inxml + "<result><XML1></XML1><XML2></XML2><XML3></XML3><XML4></XML4><XML5></XML5><XML6></XML6><XML7></XML7><XML8></XML8><XML9></XML9></result>";
             inxml = inxml + "</root>";

             string Ipstr = "127.0.0.1";
             try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
             catch { Ipstr = "127.0.0.1"; }
             try
             {
                 //cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();
                 cszxyyweb2.NYwclService cs = new PathHISZGQJK.cszxyyweb2.NYwclService();
                 cs.Url = "http://192.168.3.19:8080/Services/NYwclService?wsdl";

                 if (pathweburl != "")
                     cs.Url = pathweburl;


                 string messagexml = cs.business(3626, "9d8a121ce581499d", Ipstr, xmlstr1, xmlstr2, 1, inxml);
                 XmlNode xmlok = null;
                 XmlDocument xd = new XmlDocument();

                 xd.LoadXml(messagexml);
                 xmlok = xd.SelectSingleNode("/rtns/rtn");

                 if (xmlok["state"].InnerText.ToString() == "true")
                 {
                     if (debug == "1")
                         MessageBox.Show("回传状态成功！");
                     //      return;

                 }
                 else
                 {
                     string Mess = xmlok["errortext"].InnerText.ToString();
                     if (msg == "1")
                         MessageBox.Show("回传状态失败！原因：" + Mess);
                     log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传的状态错误：" + Mess);
                     //    return;
                 }
             }
             catch (Exception ee)
             {
                 if (msg == "1")
                     MessageBox.Show("回传状态异常！原因：" + ee.ToString());

                 log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传状态异常：" + ee.ToString());
                 return;
             }

         }
         if (bgzt == "已审核")
         {



             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("门诊"))
             {
                 //xmlstr1 = "his_clinic";
                 xmlstr2 = "20100456";//20100456回报告
             }
             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("住院"))
             {
                 //xmlstr1 = "his_iinhos";
                 xmlstr2 = "20100457";//20100457回报告
             }

             string inxml = "";
             inxml = inxml + "<?xml version='1.0' encoding='gb2312' standalone='no' ?>";
             inxml = inxml + "<root>";
             inxml = inxml + "<arg1>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</arg1>";
             inxml = inxml + "<arg2></arg2>";
             inxml = inxml + "<arg3>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</arg3>";
             inxml = inxml + "<arg4>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</arg4>";



             DataTable TJ_bljc = new DataTable();
             TJ_bljc = aa.GetDataTable(" select F_TBSzd,F_TBS_BCYJ1 from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

             //诊断
             string blzd = bljc.Rows[0]["F_blzd"].ToString().Trim();
             if (TJ_bljc.Rows.Count > 0)
             {

                 if (bljc.Rows[0]["F_blk"].ToString().Contains("HPV"))
                 {
                     blzd = TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString()
               + "\r\n" + TJ_bljc.Rows[0]["F_TBSzd"].ToString();
                 }
             }




             inxml = inxml + "<arg5>" + System.Security.SecurityElement.Escape(blzd) + "</arg5>";

             inxml = inxml + "<arg6>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + "</arg6>";
             inxml = inxml + "<arg7>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</arg7>";

             inxml = inxml + "<result><XML1></XML1><XML2></XML2><XML3></XML3><XML4></XML4><XML5></XML5><XML6></XML6><XML7></XML7><XML8></XML8><XML9></XML9></result>";
             inxml = inxml + "</root>";

             string Ipstr = "127.0.0.1";
             try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
             catch { Ipstr = "127.0.0.1"; }
             try
             {
               //  cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();
                 cszxyyweb2.NYwclService cs = new PathHISZGQJK.cszxyyweb2.NYwclService();
                 cs.Url = "http://192.168.3.19:8080/Services/NYwclService?wsdl";

                 if (pathweburl != "")
                     cs.Url = pathweburl;
                 string messagexml = cs.business(3626, "9d8a121ce581499d", Ipstr, xmlstr1, xmlstr2, 1, inxml);
                 XmlNode xmlok = null;
                 XmlDocument xd = new XmlDocument();


                 xd.LoadXml(messagexml);
                 xmlok = xd.SelectSingleNode("/rtns/rtn");

                 if (xmlok["state"].InnerText.ToString() == "true")
                 {
                     if (debug == "1")
                         MessageBox.Show("回传报告成功！");
                     return;

                 }
                 else
                 {
                     string Mess = xmlok["errortext"].InnerText.ToString();
                     if (msg == "1")
                         MessageBox.Show("回传报告失败！原因：" + Mess);

                     log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + ",回传报告的错误：" + Mess);
                     return;
                 }
             }
             catch (Exception ee)
             {
                 if (msg == "1")
                     MessageBox.Show("回传报告异常：" + ee.ToString());

                 log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传报告异常：" + ee.ToString());
                 return;
             }
         }

         ///延期报告---回写
         if (bgzt == "报告延期" && bljc.Rows[0]["F_blk"].ToString().Trim() == "常规")
         {
             string blzd = bljc.Rows[0]["F_blzd"].ToString().Trim();
             string bz = bljc.Rows[0]["F_bz"].ToString().Trim();
             if (blzd != "" || bz == "")
             {
                 return;
             }
             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("门诊"))
             {
                 //xmlstr1 = "his_clinic";
                 xmlstr2 = "20100456";//20100456回报告
             }
             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("住院"))
             {
                 //xmlstr1 = "his_iinhos";
                 xmlstr2 = "20100457";//20100457回报告
             }

             if (MessageBox.Show("此报告为延期报告,将回传初步诊断。\n请确认是否回传？？？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
             {

                 string inxml = "";
                 inxml = inxml + "<?xml version='1.0' encoding='gb2312' standalone='no' ?>";
                 inxml = inxml + "<root>";
                 inxml = inxml + "<arg1>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</arg1>";
                 inxml = inxml + "<arg2></arg2>";
                 inxml = inxml + "<arg3>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</arg3>";
                 inxml = inxml + "<arg4>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</arg4>";



                 //DataTable TJ_bljc = new DataTable();
                 //TJ_bljc = aa.GetDataTable(" select F_TBSzd,F_TBS_BCYJ1 from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

                 //诊断

                 //if (TJ_bljc.Rows.Count > 0)
                 //{

                 //    if (bljc.Rows[0]["F_blk"].ToString() == "HPV")
                 //    {
                 //        blzd = " 低危型：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString()
                 //  + "高危型：" + TJ_bljc.Rows[0]["F_TBSzd"].ToString();
                 //    }
                 //}




                 inxml = inxml + "<arg5>" + System.Security.SecurityElement.Escape(bz) + "</arg5>";

                 inxml = inxml + "<arg6>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + "</arg6>";
                 inxml = inxml + "<arg7>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</arg7>";

                 inxml = inxml + "<result><XML1></XML1><XML2></XML2><XML3></XML3><XML4></XML4><XML5></XML5><XML6></XML6><XML7></XML7><XML8></XML8><XML9></XML9></result>";
                 inxml = inxml + "</root>";

                 string Ipstr = "127.0.0.1";
                 try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
                 catch { Ipstr = "127.0.0.1"; }
                 try
                 {
                   //  cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();
                     cszxyyweb2.NYwclService cs = new PathHISZGQJK.cszxyyweb2.NYwclService();
                     cs.Url = "http://192.168.3.19:8080/Services/NYwclService?wsdl";

                     if (pathweburl != "")
                         cs.Url = pathweburl;
                     string messagexml = cs.business(3626, "9d8a121ce581499d", Ipstr, xmlstr1, xmlstr2, 1, inxml);
                     XmlNode xmlok = null;
                     XmlDocument xd = new XmlDocument();


                     xd.LoadXml(messagexml);
                     xmlok = xd.SelectSingleNode("/rtns/rtn");

                     if (xmlok["state"].InnerText.ToString() == "true")
                     {
                         if (debug == "1")
                             MessageBox.Show("回传报告成功！");
                         return;

                     }
                     else
                     {
                         string Mess = xmlok["errortext"].InnerText.ToString();
                         if (msg == "1")
                             MessageBox.Show("回传报告失败！原因：" + Mess);
                         log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + ",回传报告的错误：" + Mess);
                         return;
                     }
                 }
                 catch (Exception ee)
                 {
                     if (msg == "1")
                         MessageBox.Show("回传报告异常：" + ee.ToString());

                     log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传报告异常：" + ee.ToString());
                     return;
                 }
             } return;
         }


     }
     //2015-12-29 修改前
     public void pathtohis_old20151229(string blh, string yymc)
     {
         string pathweburl = f.ReadString("savetohis", "webservicesurl", "");

         string msg = f.ReadString("savetohis", "msg", "");
         string debug = f.ReadString("savetohis", "debug", "");


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
         if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
         {
             log.WriteMyLog("无申请序号（单据号），不处理！");
             return;
         }

         //DataTable T_SZ = new DataTable();
         //T_SZ = aa.GetDataTable("select F_SZZ,F_XL from T_SZ where F_DL='JK' and (F_XL='JK_MSG')", "T_SZ");

         //for (int y = 0; y < T_SZ.Rows.Count; y++)
         //{
         //    if (T_SZ.Rows[0]["F_XL"].ToString().Trim() == "JK_MSG")
         //    {
         //        if (msg.Trim() == "")
         //            msg = T_SZ.Rows[0]["F_SZZ"].ToString().Trim();
         //    }
         //}

         //------------------------------------------------------------------------------
         //--------------------体检回传-------------------------------------------------
         if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("体检"))
         {
             if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核")
             {
                 DataTable TJ_bljc = new DataTable();
                 TJ_bljc = aa.GetDataTable(" select F_TBSzd,F_TBS_BCYJ1,F_TBS_WSW2,F_TBS_WSW1,F_TBS_WSW3,F_TBS_BDXM1,F_TBS_YZCD,F_TBS_FXJ,F_TBS_NZJ,F_TBS_XJ,F_TBS_QT  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");


                 string yzlx = "1";
                 string yzid = bljc.Rows[0]["F_sqxh"].ToString().Trim();	//查询语句的ID
                 string bgsj = bljc.Rows[0]["F_BGRQ"].ToString().Trim();
                 string bgrq = bgsj.Replace('-', '/');	//报告日期 yyyy/mm/dd	
                 string zsms = "";    //看到
                 //诊断
                 string blzd = "诊断:" + bljc.Rows[0]["F_blzd"].ToString().Trim();
                 if (TJ_bljc.Rows.Count > 0)
                 {

                     if (bljc.Rows[0]["F_blk"].ToString().Contains("TCT"))
                     {
                         //病原微生物
                         zsms = TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_FXJ"].ToString().Trim() + ";   " + TJ_bljc.Rows[0]["F_TBS_NZJ"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_XJ"].ToString().Trim() + ";  " + TJ_bljc.Rows[0]["F_TBS_QT"].ToString().Trim() + ";";

                         //炎症程度
                         string yzcd = "炎症程度:" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + ";";

                         blzd = yzcd + "  " + blzd;

                     }
                     if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                         blzd = blzd + "   建议:" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim();

                     if (bljc.Rows[0]["F_blk"].ToString().Contains("HPV"))
                     {
                         blzd = TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString()
                   + "\r\n" + TJ_bljc.Rows[0]["F_TBSzd"].ToString();
                         zsms = blzd;
                     }
                 }


                 string yxzd = blzd;	//结论	
                 string shys = bljc.Rows[0]["F_SHYS"].ToString().Trim();     //发报告医生
                 string bgks = "病理科";     //报告科室
                 string MSG = "";
                 try
                 {
                     cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();
                     MSG = cs.SetJcjg(yzlx, yzid, bgrq, zsms, yxzd, shys, bgks);


                     if (MSG == "1")
                     {
                         if (debug == "1")
                             MessageBox.Show("回传体检信息成功！");
                         //      return;

                     }
                     else
                     {
                         if (msg == "1")
                             MessageBox.Show("回传体检信息失败！原因：" + MSG);
                         log.WriteMyLog("体检，" + bljc.Rows[0]["F_blh"].ToString().Trim() + "，" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "，回传的体检错误：" + MSG);
                         //    return;
                     }
                 }
                 catch (Exception ee)
                 {
                     if (msg == "1")
                         MessageBox.Show("回传体检信息异常：" + ee.ToString());
                     log.WriteMyLog("回传体检信息异常：" + ee.ToString());
                     return;
                 }

             }
             return;
         }
         ///////////////////////////////////////////////////////////////////////////////
         //////////////////////////////////////////////////////////////////////////////
         //------------------------------------------------------------------------------
         string xmlstr1 = "";
         string xmlstr2 = "";
         if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("门诊"))
         {
             xmlstr1 = "his_clinic";
             xmlstr2 = "20100425";//20100456回报告
         }
         if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("住院"))
         {
             xmlstr1 = "his_iinhos";
             xmlstr2 = "20100424";//20100457回报告
         }
         if (xmlstr1 == "")
         {
             log.WriteMyLog("病人类别为空，不处理");
             return;
         }

         //---------------------------------------------------------------------------------------
         //-------------------------非体检回传-------------------------------------------------------
         string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

         string sqzt = bljc.Rows[0]["F_sqzt"].ToString().Trim();
         string sqbj = "0";
         if (sqzt == "2")
         {
             //不写标记
         }
         else
         {

             if (bgzt != "已审核")
             {
                 if (sqzt == "")
                 {
                     sqbj = "1";//要执行写标记
                 }
                 else
                 {
                     //不写标记
                 }
             }
         }
         if (sqbj == "1")
         {

             string bgztID = "";

             if (bgzt == "已审核")
             {
                 bgztID = "2";
             }
             else
             {
                 bgztID = "1";
             }
             string inxml = "";
             inxml = inxml + "<?xml version='1.0' encoding='gb2312' standalone='no' ?>";
             inxml = inxml + "<root>";
             inxml = inxml + "<arg1>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</arg1>";
             inxml = inxml + "<arg2>" + bgztID + "</arg2>";
             inxml = inxml + "<arg3>" + bljc.Rows[0]["F_JSY"].ToString().Trim() + "</arg3>";
             inxml = inxml + "<arg4></arg4><arg5></arg5>";
             inxml = inxml + "<result><XML1></XML1><XML2></XML2><XML3></XML3><XML4></XML4><XML5></XML5><XML6></XML6><XML7></XML7><XML8></XML8><XML9></XML9></result>";
             inxml = inxml + "</root>";

             string Ipstr = "127.0.0.1";
             try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
             catch { Ipstr = "127.0.0.1"; }
             try
             {
                 cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();


                 string messagexml = cs.business(3626, "111", Ipstr, xmlstr1, xmlstr2, 1, inxml);
                 XmlNode xmlok = null;
                 XmlDocument xd = new XmlDocument();

                 xd.LoadXml(messagexml);
                 xmlok = xd.SelectSingleNode("/rtns/rtn");

                 if (xmlok["state"].InnerText.ToString() == "true")
                 {
                     if (debug == "1")
                         MessageBox.Show("回传状态成功！");
                     //      return;

                 }
                 else
                 {
                     string Mess = xmlok["errortext"].InnerText.ToString();
                     if (msg == "1")
                         MessageBox.Show("回传状态失败！原因：" + Mess);
                     log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传的状态错误：" + Mess);
                     //    return;
                 }
             }
             catch (Exception ee)
             {
                 if (msg == "1")
                     MessageBox.Show("回传状态异常！原因：" + ee.ToString());

                 log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传状态异常：" + ee.ToString());
                 return;
             }

         }
         if (bgzt == "已审核")
         {



             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("门诊"))
             {
                 //xmlstr1 = "his_clinic";
                 xmlstr2 = "20100456";//20100456回报告
             }
             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("住院"))
             {
                 //xmlstr1 = "his_iinhos";
                 xmlstr2 = "20100457";//20100457回报告
             }

             string inxml = "";
             inxml = inxml + "<?xml version='1.0' encoding='gb2312' standalone='no' ?>";
             inxml = inxml + "<root>";
             inxml = inxml + "<arg1>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</arg1>";
             inxml = inxml + "<arg2></arg2>";
             inxml = inxml + "<arg3>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</arg3>";
             inxml = inxml + "<arg4>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</arg4>";



             DataTable TJ_bljc = new DataTable();
             TJ_bljc = aa.GetDataTable(" select F_TBSzd,F_TBS_BCYJ1 from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

             //诊断
             string blzd = bljc.Rows[0]["F_blzd"].ToString().Trim();
             if (TJ_bljc.Rows.Count > 0)
             {

                 if (bljc.Rows[0]["F_blk"].ToString().Contains("HPV"))
                 {
                     blzd = TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString()
               + "\r\n" + TJ_bljc.Rows[0]["F_TBSzd"].ToString();
                 }
             }




             inxml = inxml + "<arg5>" + System.Security.SecurityElement.Escape(blzd) + "</arg5>";

             inxml = inxml + "<arg6>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + "</arg6>";
             inxml = inxml + "<arg7>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</arg7>";

             inxml = inxml + "<result><XML1></XML1><XML2></XML2><XML3></XML3><XML4></XML4><XML5></XML5><XML6></XML6><XML7></XML7><XML8></XML8><XML9></XML9></result>";
             inxml = inxml + "</root>";

             string Ipstr = "127.0.0.1";
             try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
             catch { Ipstr = "127.0.0.1"; }
             try
             {
                 cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();

                 string messagexml = cs.business(3626, "111", Ipstr, xmlstr1, xmlstr2, 1, inxml);
                 XmlNode xmlok = null;
                 XmlDocument xd = new XmlDocument();


                 xd.LoadXml(messagexml);
                 xmlok = xd.SelectSingleNode("/rtns/rtn");

                 if (xmlok["state"].InnerText.ToString() == "true")
                 {
                     if (debug == "1")
                         MessageBox.Show("回传报告成功！");
                     return;

                 }
                 else
                 {
                     string Mess = xmlok["errortext"].InnerText.ToString();
                     if (msg == "1")
                         MessageBox.Show("回传报告失败！原因：" + Mess);

                     log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + ",回传报告的错误：" + Mess);
                     return;
                 }
             }
             catch (Exception ee)
             {
                 if (msg == "1")
                     MessageBox.Show("回传报告异常：" + ee.ToString());

                 log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传报告异常：" + ee.ToString());
                 return;
             }
         }

         ///延期报告---回写
         if (bgzt == "报告延期" && bljc.Rows[0]["F_blk"].ToString().Trim() == "常规")
         {
             string blzd = bljc.Rows[0]["F_blzd"].ToString().Trim();
             string bz = bljc.Rows[0]["F_bz"].ToString().Trim();
             if (blzd != "" || bz == "")
             {
                 return;
             }
             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("门诊"))
             {
                 //xmlstr1 = "his_clinic";
                 xmlstr2 = "20100456";//20100456回报告
             }
             if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("住院"))
             {
                 //xmlstr1 = "his_iinhos";
                 xmlstr2 = "20100457";//20100457回报告
             }

             if (MessageBox.Show("此报告为延期报告,将回传初步诊断。\n请确认是否回传？？？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
             {

                 string inxml = "";
                 inxml = inxml + "<?xml version='1.0' encoding='gb2312' standalone='no' ?>";
                 inxml = inxml + "<root>";
                 inxml = inxml + "<arg1>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</arg1>";
                 inxml = inxml + "<arg2></arg2>";
                 inxml = inxml + "<arg3>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</arg3>";
                 inxml = inxml + "<arg4>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</arg4>";



                 //DataTable TJ_bljc = new DataTable();
                 //TJ_bljc = aa.GetDataTable(" select F_TBSzd,F_TBS_BCYJ1 from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

                 //诊断

                 //if (TJ_bljc.Rows.Count > 0)
                 //{

                 //    if (bljc.Rows[0]["F_blk"].ToString() == "HPV")
                 //    {
                 //        blzd = " 低危型：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString()
                 //  + "高危型：" + TJ_bljc.Rows[0]["F_TBSzd"].ToString();
                 //    }
                 //}




                 inxml = inxml + "<arg5>" + System.Security.SecurityElement.Escape(bz) + "</arg5>";

                 inxml = inxml + "<arg6>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + "</arg6>";
                 inxml = inxml + "<arg7>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</arg7>";

                 inxml = inxml + "<result><XML1></XML1><XML2></XML2><XML3></XML3><XML4></XML4><XML5></XML5><XML6></XML6><XML7></XML7><XML8></XML8><XML9></XML9></result>";
                 inxml = inxml + "</root>";

                 string Ipstr = "127.0.0.1";
                 try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
                 catch { Ipstr = "127.0.0.1"; }
                 try
                 {
                     cszxyyWeb.NYwclService cs = new PathHISZGQJK.cszxyyWeb.NYwclService();

                     string messagexml = cs.business(3626, "111", Ipstr, xmlstr1, xmlstr2, 1, inxml);
                     XmlNode xmlok = null;
                     XmlDocument xd = new XmlDocument();


                     xd.LoadXml(messagexml);
                     xmlok = xd.SelectSingleNode("/rtns/rtn");

                     if (xmlok["state"].InnerText.ToString() == "true")
                     {
                         if (debug == "1")
                             MessageBox.Show("回传报告成功！");
                         return;

                     }
                     else
                     {
                         string Mess = xmlok["errortext"].InnerText.ToString();
                         if (msg == "1")
                             MessageBox.Show("回传报告失败！原因：" + Mess);
                         log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + ",回传报告的错误：" + Mess);
                         return;
                     }
                 }
                 catch (Exception ee)
                 {
                     if (msg == "1")
                         MessageBox.Show("回传报告异常：" + ee.ToString());

                     log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "," + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "回传报告异常：" + ee.ToString());
                     return;
                 }
             } return;
         }


     }

    }
}
