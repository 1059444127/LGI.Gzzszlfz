using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using LGHISJKZGQ;
namespace LGHISJKZGQ
{
    // 山西肿瘤医院
    class SXSZLYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {

                if (Sslbx == "住院号")
                {
                    DataSet ds;
                    string pathWEB = f.ReadString("住院号", "webservicesurl", ""); //获取sz.ini中设置的webservicesurl

                    string debug = f.ReadString("住院号", "debug", "0");
                    sxzlyyWeb.Service1 sxzl = new LGHISJKZGQ.sxzlyyWeb.Service1();
                    if (pathWEB!="")
                    sxzl.Url = pathWEB;
                   
                    try
                    {
                         ds = sxzl.GetPatientInfo(Ssbz.Trim());

                        if (ds.Tables[0].Rows.Count < 1||ds== null)
                        { MessageBox.Show("没有此" + Sslbx + Ssbz + "的病人信息");
                        return "0";
                        }


                        
                    }
                       
                    catch(Exception e)
                    {
                        log.WriteMyLog("获取his数据错误:" + e.ToString());
                        return "0";

                    }

                    try
                    {
                        if (debug == "1")
                        {
                            ds.WriteXml(Application.StartupPath + "\\log\\returnXML.XML");
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["DiagnoseId"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + "" + (char)34 + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[0]["cPatientCode"].ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["cPatientName"].ToString() + (char)34 + " ";
                        string xbid = ds.Tables[0].Rows[0]["iSex"].ToString();
                        string xb = "";
                        if (xbid == "2")
                            xb = "女";
                        if (xbid == "1")
                            xb = "男";

                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[0]["cAge"].ToString() + "岁" + (char)34 + " ";
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[0]["MarriageName"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        try
                        {
                              xml = xml + "地址=" + (char)34 +ds.Tables[0].Rows[0]["cFamilyAddress"].ToString() + (char)34 + "   ";
                        }
                        catch
                        {
                            xml = xml + "地址=" + (char)34 +"" + (char)34 + "   ";
                        }
                        try
                        {
                            xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["cFamilyTel"].ToString()+ (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "病区=" + (char)34 + (char)34 + " ";
                        try
                        {
                        xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["BedCode"].ToString()+ (char)34 + " ";
                        }
                        catch
                        {
                              xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[0]["cIdCode"].ToString() + (char)34 + " ";
                        try
                        {
                        xml = xml + "民族=" + (char)34 +ds.Tables[0].Rows[0]["NationName"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                               xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        }
                        try
                        {
                            xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[0]["JobName"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["DeptName"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["StaffName"].ToString() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        try
                        {
                        xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[0]["cSampleSource"].ToString()+(char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                        }
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "病人类别=" + (char)34 +"住院" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[0]["DiagResult"].ToString() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        
                        return xml;
                    }
                    catch(Exception ee)
                    {
                        if(Debug=="1")
                        MessageBox.Show("返回<LOGENE>xml错误");
                    log.WriteMyLog("返回<LOGENE>xml错误:"+ee.ToString());
                        return "0";
                    }
                }



                if (Sslbx == "发票号")
                {
                    DataSet ds;
                    string pathWEB = f.ReadString("发票号", "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
                    string debug = f.ReadString("发票号", "debug", "0");
                    sxzlyyWeb.Service1 sxzl = new LGHISJKZGQ.sxzlyyWeb.Service1();
                    if (pathWEB != "")
                        sxzl.Url = pathWEB;
                  
                    try
                    {
                        ds = sxzl.GetPatienInfo_MZ(Ssbz.Trim());

                        if (ds.Tables[0].Rows.Count < 1 || ds == null)
                        {
                            MessageBox.Show("没有此" + Sslbx + Ssbz + "的病人信息");
                            return "0";
                        }

                    }

                    catch (Exception e)
                    {
                        log.WriteMyLog("获取his数据错误:" + e.ToString());
                        return "0";
                    }


                    try
                    {
                        if (debug == "1")
                        {
                            ds.WriteXml(Application.StartupPath + "\\log\\returnXML.XML");
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "就诊ID=" + "" + (char)34 + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["cBillCode"].ToString() + (char)34 + " ";
                        try
                        {
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["DiagnoseId"].ToString()+ (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["cPatientName"].ToString() + (char)34 + " ";
                       xml = xml + "性别=" + (char)34 + ""+ (char)34 + " ";
                        string nl = "";
                        if (ds.Tables[0].Rows[0]["cAge"].ToString().Trim() == "" || ds.Tables[0].Rows[0]["cAge"].ToString() == null)
                            nl = "";
                        else
                            nl = ds.Tables[0].Rows[0]["cAge"].ToString() + "岁";
                        xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";

                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[0]["MarriageName"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        try
                        {
                              xml = xml + "地址=" + (char)34 +ds.Tables[0].Rows[0]["cFamilyAddress"].ToString() + (char)34 + "   ";
                        }
                        catch
                        {
                            xml = xml + "地址=" + (char)34 +"" + (char)34 + "   ";
                        }
                        try
                        {
                            xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["cFamilyTel"].ToString()+ (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }

                        xml = xml + "病区=" + (char)34 + (char)34 + " ";
                        try
                        {
                        xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["BedCode"].ToString()+ (char)34 + " ";
                        }
                        catch
                        {
                              xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[0]["cIdCode"].ToString() + (char)34 + " ";
                        try
                        {
                        xml = xml + "民族=" + (char)34 +ds.Tables[0].Rows[0]["NationName"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                               xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        }
                        try
                        {
                            xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[0]["JobName"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                      

                       
                        xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["DeptName"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["StaffName"].ToString() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                     
                        try
                        {
                        xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[0]["cSampleSource"].ToString()+(char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                        }
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;
                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("返回<LOGENE>xml错误");
                        log.WriteMyLog("返回<LOGENE>xml错误:" + ee.ToString());
                        return "0";
                    }
                } 


                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");
                    return "0";
                }

            }
            else
            {
                MessageBox.Show("无此" + Sslbx);
                log.WriteMyLog(Sslbx + Ssbz + "不存在！");
                return "0";
            }
            
        }
    }
}
