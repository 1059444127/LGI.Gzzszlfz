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
    /// //上海市嘉定区中医医院--复高--webservices
    /// http://192.168.1.158:1405/services/FGPLService?wsdl
    /// </summary>
    class shhjdqzyyy
    {

        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug1)
        {

            string msg = f.ReadString("savetohis", "msg", "");
            string debug = f.ReadString("savetohis", "debug", "");
            string URL = f.ReadString("savetohis", "URL", "");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            shsjdqzyyyweb.FGPLService shjd = new PathHISZGQJK.shsjdqzyyyweb.FGPLService();
            if (URL.Trim() != "")
                shjd.Url = URL;

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
               //回传状态

                if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "住院" && bljc.Rows[0]["F_brlb"].ToString().Trim() != "门诊")
                {
                    if (debug == "1")
                        MessageBox.Show("非住院或门诊病人，不处理！");
                    log.WriteMyLog("非住院或门诊病人，不处理！");
                    return;
                }

                 string sqxh=bljc.Rows[0]["F_SQXH"].ToString().Trim();
                string mzh = bljc.Rows[0]["F_MZH"].ToString().Trim();
                    string zyh = bljc.Rows[0]["F_ZYH"].ToString().Trim();
                    string ghxh = bljc.Rows[0]["F_BRBH"].ToString().Trim();
                    string brlb = bljc.Rows[0]["F_BRLB"].ToString().Trim();

                    if (brlb == "住院") brlb = "I";
                    if (brlb == "门诊") brlb = "O";
                if (sqxh.Trim()!="")
                {
                    //回传状态
                    string bgztbm = "FN";
                    string State_XML = "<ApplyInfo><OutPatientNo>" + mzh + "</OutPatientNo><InPatientNo>" + zyh + "</InPatientNo>";
                    State_XML = State_XML + "<RegCode>" + ghxh + "</RegCode>";
                    State_XML = State_XML + "<PatientType>" + brlb + "</PatientType>";
         
                    State_XML = State_XML + "<RequestId>" + sqxh + "</RequestId>";
                    State_XML = State_XML + "<RequestStatus>" + bgztbm + "</RequestStatus>";
                    State_XML = State_XML + "<DepartCode></DepartCode>";
                    State_XML = State_XML + "<OprCode></OprCode>";
                    string ItemCode = bljc.Rows[0]["F_YZXM"].ToString().Trim();
                    if (ItemCode.Contains("^"))
                        State_XML = State_XML + "<ItemCode>" + ItemCode.Split('^')[0].Trim() + "</ItemCode>";
                    else
                        State_XML = State_XML + "<ItemCode>" + "" + "</ItemCode>";
                    State_XML = State_XML + "</ApplyInfo>";

                    if (debug == "1")
                        MessageBox.Show("状态变更传入XML：" + State_XML);
                    try
                    {
                        string State_RTN = shjd.UpdateApplyBillState(State_XML);
                        if (debug == "1")
                            MessageBox.Show("状态变更返回XML：" + State_RTN);
                        try
                        {
                            XmlNode xmlok_DATA_Info = null;
                            XmlDocument xd_Info = new XmlDocument();
                            xd_Info.LoadXml(State_RTN);
                            xmlok_DATA_Info = xd_Info.SelectSingleNode("/Result");
                            if (xmlok_DATA_Info["Status"].InnerText != "T")
                                log.WriteMyLog("状态变更返回XML解析异常：" + State_RTN);

                        }
                        catch
                        {
                            log.WriteMyLog("状态变更返回XML解析异常：" + State_RTN);
                            if (debug == "1")
                                MessageBox.Show("状态变更返回XML解析异常");
                        }
                    }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog("状态变更执行异常：" + ee2.ToString());
                        if (debug == "1")
                            MessageBox.Show("状态变更执行异常");
                    }

                }
                //回传报告


                string  bgbm="PF";
                try
                {
                    if (bljc.Rows[0]["F_HXBJ"].ToString().Trim() == "1")
                        bgbm = "RE";
                }
                catch
                {
                }
                        string  Report_XML="<ReportData>";
                        Report_XML=Report_XML+"<OutPatientNo>"+mzh+"</OutPatientNo>";
                        Report_XML=Report_XML+"<InPatientNo>"+zyh+"</InPatientNo>";
                        Report_XML=Report_XML+"<RegCode>"+ghxh+"</RegCode>";
                        Report_XML=Report_XML+"<EncounterType>"+brlb+"</EncounterType>";
                        Report_XML=Report_XML+"<Requests><Request>";
                        Report_XML=Report_XML+"<RequestId>"+sqxh+"</RequestId>";
                        Report_XML = Report_XML + "<Items><Item>";
                        Report_XML=Report_XML+"<ItemCode></ItemCode>";
                        Report_XML=Report_XML+"<ItemName></ItemName>";
                        Report_XML=Report_XML+"<CheckPointCode></CheckPointCode>";
                        Report_XML=Report_XML+"<CheckPointName></CheckPointName>";
                        Report_XML=Report_XML+"</Item></Items>";
                        Report_XML=Report_XML+"</Request></Requests>";
                        Report_XML=Report_XML+"<ReportHead>";
                        Report_XML=Report_XML+"<CheckId>"+blh+"</CheckId>";
                        Report_XML=Report_XML+"<ReportID>"+blh+"</ReportID>";
                        Report_XML=Report_XML+"<ReportStatus>"+bgbm+"</ReportStatus>";
                        Report_XML = Report_XML + "<ResultDate>" +DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMdd") + "</ResultDate>";
                        Report_XML = Report_XML + "<CheckDoctorCode>" + getyygh(bljc.Rows[0]["F_BGYS"].ToString().Trim()) + "</CheckDoctorCode>";
                        Report_XML=Report_XML+"<CheckDoctorName>"+bljc.Rows[0]["F_BGYS"].ToString().Trim()+"</CheckDoctorName>";
                        Report_XML = Report_XML + "<CheckDate>" +DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMdd") + "</CheckDate>";
                        Report_XML = Report_XML + "<VerifyDoctorCode>" + getyygh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDoctorCode>";
                        Report_XML = Report_XML + "<VerifyDoctorName>" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</VerifyDoctorName>";
                        Report_XML=Report_XML+"<MethodCode></MethodCode>";
                        Report_XML=Report_XML+"<MethodName></MethodName>";
                        Report_XML=Report_XML+"<EquipmentCode></EquipmentCode>";
                        Report_XML=Report_XML+"<EquipmentName></EquipmentName>";
                        Report_XML = Report_XML + "<CheckFinding>" + bljc.Rows[0]["F_RYSJ"].ToString().Trim() +"\r\n"+bljc.Rows[0]["F_JXSJ"].ToString().Trim()+"</CheckFinding>";
                        Report_XML = Report_XML + "<CheckResult>" + bljc.Rows[0]["F_BLZD"].ToString().Trim() +"\r\n"+ bljc.Rows[0]["F_TSJC"].ToString().Trim() + "</CheckResult>";
                        Report_XML=Report_XML+"<Memo></Memo></ReportHead></ReportData>";
                        if (debug == "1")
                        {
                            MessageBox.Show("报告回写传入XML：" + Report_XML);
                            log.WriteMyLog("报告回写传入XML：" + Report_XML);
                        }
                        try
                        {
                            string Report_RTN = shjd.SendReport(Report_XML);
                            if (debug == "1")
                            {
                                MessageBox.Show("报告回写返回XML：" + Report_RTN);
                                log.WriteMyLog("报告回写返回XML：" + Report_RTN);
                            }
                            try
                            {
                                XmlNode xmlok_DATA_Info = null;
                                XmlDocument xd_Info = new XmlDocument();
                                xd_Info.LoadXml(Report_RTN);
                                xmlok_DATA_Info = xd_Info.SelectSingleNode("/Result");
                                if (xmlok_DATA_Info["Status"].InnerText != "T")
                                    log.WriteMyLog("报告回写错误，返回XML：" + Report_RTN);
                                else
                                    aa.ExecuteSQL("update  T_jcxx set F_HXBJ='1' where F_blh='" + blh + "'");
                                return;
                            }
                            catch
                            {
                                if (debug == "1")
                                    MessageBox.Show("报告回写返回XML解析异常");

                                log.WriteMyLog("报告回写返回XML解析异常");
                                return;
                            }

                        }
                        catch (Exception eee)
                        {
                            if (debug == "1")
                            MessageBox.Show("报告回写，执行异常："+eee.ToString());
                        log.WriteMyLog("报告回写，执行异常：" + eee.ToString());
                        return;
                        }



            }
            else
            {
                string sqxh=bljc.Rows[0]["F_SQXH"].ToString().Trim();
                if (sqxh.Trim()=="")
                {
                    log.WriteMyLog("无申请序号，不回传状态！");
                    return;
                }

                string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
                if (bgzt == "已登记" || bgzt == "已取材")
                {

                    //回传状态
                    string mzh = bljc.Rows[0]["F_MZH"].ToString().Trim();
                    string zyh = bljc.Rows[0]["F_ZYH"].ToString().Trim();
                    string ghxh = bljc.Rows[0]["F_BRBH"].ToString().Trim();
                    string brlb = bljc.Rows[0]["F_BRLB"].ToString().Trim();

                    if (brlb == "住院") brlb = "I";
                    if (brlb == "门诊") brlb = "O";

                    string bgztbm = "RG";

                    string State_XML = "<ApplyInfo><OutPatientNo>" + mzh + "</OutPatientNo><InPatientNo>" + zyh + "</InPatientNo>";
                    State_XML = State_XML + "<RegCode>" + ghxh + "</RegCode>";
                    State_XML = State_XML + "<PatientType>" + brlb + "</PatientType>";
                    State_XML = State_XML + "<RequestId>" + sqxh + "</RequestId>";
                    State_XML = State_XML + "<RequestStatus>" + bgztbm + "</RequestStatus>";
                    State_XML = State_XML + "<DepartCode></DepartCode>";
                    State_XML = State_XML + "<OprCode></OprCode>";
                    string ItemCode= bljc.Rows[0]["F_YZXM"].ToString().Trim();
                    if(ItemCode.Contains("^"))
                    State_XML = State_XML + "<ItemCode>"+ItemCode.Split('^')[0].Trim()+"</ItemCode>";
                    else
                    State_XML = State_XML + "<ItemCode>"+""+"</ItemCode>";
                    State_XML = State_XML + "</ApplyInfo>";

                    if (debug == "1")
                        MessageBox.Show("状态变更传入XML：" + State_XML);
                    try
                    {
                        string State_RTN = shjd.UpdateApplyBillState(State_XML);
                        if (debug == "1")
                            MessageBox.Show("状态变更返回XML：" + State_RTN);
                        try
                        {
                            XmlNode xmlok_DATA_Info = null;
                            XmlDocument xd_Info = new XmlDocument();
                            xd_Info.LoadXml(State_RTN);
                            xmlok_DATA_Info = xd_Info.SelectSingleNode("/Result");
                            if (xmlok_DATA_Info["Status"].InnerText != "T")
                                log.WriteMyLog("状态变更返回XML解析异常：" + State_RTN);
                            return;
                        }
                        catch
                        {
                            log.WriteMyLog("状态变更返回XML解析异常：" + State_RTN);
                            if (debug == "1")
                                MessageBox.Show("状态变更返回XML解析异常");
                            return;
                        }
                    }
                    catch (Exception ee2)
                    {
                        log.WriteMyLog("状态变更执行异常：" + ee2.ToString());
                        if (debug == "1")
                            MessageBox.Show("状态变更执行异常");
                    }

                }
            }
        }

        public string getyygh(string  ysxm)
        { 
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable bljc = new DataTable();
                bljc = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_yhmc='" + ysxm + "'", "bljc");
                if (bljc.Rows.Count > 0)
                    return bljc.Rows[0][0].ToString();
                else
                    return "0000";
            }
            catch
            {
                return "0000";
            }

        }
    }



}