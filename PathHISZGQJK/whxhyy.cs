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
{
    class whxhyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {
            string pathweburl = f.ReadString("savetohis", "webservicesurl", "");
            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
            
          
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

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
            {

                //回传xml
                string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
                if (bgzt == "已登记")
                {
                    string jsybh = getymh(bljc.Rows[0]["F_jsy"].ToString().Trim());
                    string jsy = bljc.Rows[0]["F_jsy"].ToString().Trim();
                    if (jsy.Trim() == "")
                    { jsy = "接收员"; jsybh = "00001"; }
                    string xmlstr = "<Request><RegInfos><RegInfo>"
                                    + "<OrdRowID>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</OrdRowID>"
                                    + "<StudyNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</StudyNo>"
                                    + "<EQInfo>" + "" + "</EQInfo>"
                                    + "<OperatorCode>" + jsybh + "</OperatorCode>"
                                    + "<Operator>" + jsy + "</Operator>"
                                    + "<MainDocCode>" + jsybh + "</MainDocCode>"
                                    + "<MainDoc>" + jsy + "</MainDoc>"
                                    + "<RegDate>" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).Date.ToString("yyyy-MM-dd") + "</RegDate>"
                                    + "<RegTime>" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).TimeOfDay.ToString() + "</RegTime>"
                                    + "</RegInfo></RegInfos></Request>";
                    string str_msg = "";
                    try
                    {
                        PathHISZGQJK.whxhyyWeb.WebRisService whxhyy = new PathHISZGQJK.whxhyyWeb.WebRisService();
                        if (pathweburl != "") whxhyy.Url = pathweburl;
                        str_msg = whxhyy.GetRegInfo(xmlstr);
                    }
                    catch (Exception ee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(ee.ToString());
                        return;
                    }
                    string messae = "";
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(str_msg);
                        xmlok = xd.SelectSingleNode("/Response");
                        messae = xmlok["ResultContent"].InnerText.ToString();
                    }
                    catch (Exception eee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(eee.ToString());
                        return;
                    }
                    if (msg == "1") MessageBox.Show("登记：" + messae);
                    if (messae != "成功") log.WriteMyLog(messae);
                    return;

                }
                if (bgzt == "已审核")
                {
                    string xmlstr = "<Request><ReturnReports><ReturnReport>"
                + "<OrdRowID>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</OrdRowID>"
                + "<StudyNo>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</StudyNo>"
                + "<GetDocCode>" + getymh(bljc.Rows[0]["F_qcys"].ToString().Trim()) + "</GetDocCode>"
                + "<GetDoc>" + bljc.Rows[0]["F_qcys"].ToString().Trim() + "</GetDoc>"
                + "<ReportStatusCode></ReportStatusCode>"
                + "<ReportStatus>已审核</ReportStatus>"
                + "<UnsendCause></UnsendCause>"
                + "<ReportDocCode>" + getymh(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</ReportDocCode>"
                + "<ReportDoc>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</ReportDoc>"
                + "<AuditDocCode>" + getymh(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</AuditDocCode>"
                + "<AuditDoc>" + bljc.Rows[0]["F_shys"].ToString().Trim() + "</AuditDoc>"
                + "<ReportDate>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Date.ToString("yyyy-MM-dd") + "</ReportDate>"
                + "<AuditDate>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).Date.ToString("yyyy-MM-dd") + "</AuditDate>"
                + "<ReportTime>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).TimeOfDay.ToString() + "</ReportTime>"
                + "<AuditTime>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).TimeOfDay.ToString() + "</AuditTime>"
                + "<Memo>" + bljc.Rows[0]["F_bz"].ToString().Trim() + "</Memo>"
                + "<ImageFile></ImageFile>"
                + "<HisArchiveTag></HisArchiveTag>"
                + "<EyeSee>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</EyeSee>"
                + "<ExamSee>" + bljc.Rows[0]["F_jxsj"].ToString().Trim() + "</ExamSee>"
                + "<Diagnose>" + bljc.Rows[0]["F_blzd"].ToString().Trim() + "</Diagnose>"
                + "</ReturnReport></ReturnReports></Request>";

                    string str_msg = "";
                    try
                    {
                        PathHISZGQJK.whxhyyWeb.WebRisService whxhyy = new PathHISZGQJK.whxhyyWeb.WebRisService();
                        if (pathweburl != "") whxhyy.Url = pathweburl;
                        str_msg = whxhyy.GetReturnReports(xmlstr);
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog(xmlstr + "#" + str_msg);
                        if (msg == "1")
                            log.WriteMyLog(ee.ToString());
                        return;
                    }
                    string messae = "";
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(str_msg);
                        xmlok = xd.SelectSingleNode("/Response");
                        messae = xmlok["ResultContent"].InnerText.ToString();
                    }
                    catch (Exception eee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(eee.ToString());
                        return;
                    }
                    if (msg == "1") MessageBox.Show("审核：" + messae);
                    if (messae != "成功") log.WriteMyLog(messae);
                    return;
                }
                if (bgzt == "已写报告")    //取消审核
                {
                    string xmlstr = "<Request><CancelReports><CancelReport>"
                                         + "<OrdRowID>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</OrdRowID>"
                                         + "<StudyNo>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</StudyNo>"
                                         + "<CancelDate>" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "</CancelDate>"
                                         + "<CancelTime>" + DateTime.Today.TimeOfDay.ToString() + "</CancelTime>"
                                         + "<CancelDocCode>" + getymh(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</CancelDocCode>"
                                         + "<CancelDoc>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</CancelDoc>"
                                        + "<CacelReportReason></CacelReportReason>"
                                         + "</CancelReport></CancelReports></Request>";


                    string str_msg = "";
                    try
                    {
                        PathHISZGQJK.whxhyyWeb.WebRisService whxhyy = new PathHISZGQJK.whxhyyWeb.WebRisService();
                        if (pathweburl != "") whxhyy.Url = pathweburl;
                        str_msg = whxhyy.GetCancelReport(xmlstr);
                    }
                    catch (Exception ee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(ee.ToString());
                        return;
                    }
                    string messae = "";
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(str_msg);
                        xmlok = xd.SelectSingleNode("/Response");
                        messae = xmlok["ResultContent"].InnerText.ToString();
                    }
                    catch (Exception eee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(eee.ToString());
                        return;
                    }
                    if (msg == "1") MessageBox.Show("报告：" + messae);
                    if (messae != "成功") log.WriteMyLog(messae);
                    return;
                }
            }


        }

        public string getymh(string yhmc)//通过医生名称 获取医生编码
        {
            if (yhmc.Trim() != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select F_yhm from T_YH where F_yhmc='" + yhmc + "'", "blxx");
                    return bljc.Rows[0]["F_yhm"].ToString().Trim();
                }
                catch (Exception ee)
                {
                    log.WriteMyLog("转换医生工号出错！原因：" + ee.ToString());
                    return "";
                }
            } return "";

        }

    }
}
