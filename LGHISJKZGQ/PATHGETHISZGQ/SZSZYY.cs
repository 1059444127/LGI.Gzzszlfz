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
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
namespace LGHISJKZGQ
{
    class SZSZYY
    {
        //苏州盛泽医院
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
       
       
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
             
            if (Sslbx != "")
            {
               
                if (Sslbx == "就诊卡号")
                {
                    string orgID = f.ReadString(Sslbx, "orgID", ""); //从sz.ini 中获取科室编码，要求不写死
                   
                    LGHISJKZGQ.szyyWEB.Service szyy = new LGHISJKZGQ.szyyWEB.Service();
                    string xmlstr = "";
                    string weburl = "";
                    weburl = f.ReadString(Sslbx, "webserviceURL", "");//从sz.ini 中获取webservice的地址，要求不写死
                   if (weburl != "")
                     szyy.Url = weburl;
                    
                    
                    try
                    {
                       
                        szyy.Init();   //初始化，这个要先初始化，后面的函数还能使用
                      
                        xmlstr = szyy.GetPatiInfoByOrg(Ssbz, 0, orgID); //获取门诊病人信息 ，参数2（ 0表示门诊，1表示住院）
                        szyy.Dispose();
                        }
                    catch (Exception ee) 
                      {
                         
                        if (Debug == "1")
                    log.WriteMyLog("接收参数:"  + Sslbx + "," + Ssbz + "," +ee.ToString()); return "0"; }

                    //xml解析
                    if (xmlstr != "")
                    {
                        DataSet ds1 = new DataSet();
                        
                        XmlDocument xd = new XmlDocument();
                        try
                        {
                           
                            StringReader sr = new StringReader(xmlstr);
                            XmlReader xr = new XmlTextReader(sr);
                            ds1.ReadXml(xr);
                            if (Debug == "1") MessageBox.Show(xmlstr);
                        }
                        catch { if (Debug == "1") log.WriteMyLog("接收参数:" + Sslbx + "," + Ssbz + "," + "XML解析错误"); return "0"; }

                        try
                        {
                          
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["CARDNO"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ORDID"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + Ssbz + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["PATINAME"].ToString() + (char)34 + " ";
                            string PSEX = ds1.Tables[0].Rows[0]["SEXID"].ToString();
                            if (PSEX == "1") { PSEX = "女"; }
                            else
                            {
                                if (PSEX == "0") PSEX = "男";
                                else PSEX = " ";
                            }
                            xml = xml + "性别=" + (char)34 + PSEX + (char)34 + " ";
                            string nl1 = ds1.Tables[0].Rows[0]["BIRTHDATE"].ToString();
                            string nl = "0岁";

                            try
                            { nl = (DateTime.Now.Year - DateTime.Parse(nl1.Substring(0, 10)).Year).ToString() + "岁"; }
                            catch
                            { nl = "0岁"; }
                            xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + ds1.Tables[0].Rows[0]["ORGNAME"].ToString() + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                            
                            xml = xml + "身份证号=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["ORGNAME1"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["EMPNAME"].ToString() + (char)34 + " ";
                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["DIAGNAME"].ToString() + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";
                          
                            return xml;
                        }
                        catch (Exception eee)
                        {
                            if (Debug == "1")
                            MessageBox.Show(eee.ToString());
                            return "0";
                        }

                    }
                }
                if (Sslbx == "住院号")
                {
                    string orgID = f.ReadString(Sslbx, "orgID", "");  //从sz.ini 中获取科室编码
                    string weburl = "";
                    try
                    {
                        weburl = f.ReadString(Sslbx, "webserviceURL", ""); //从sz.ini 中获取webservice的地址
                    }
                    catch { MessageBox.Show("请在sz.ini中设置webservice的URL");return "0"; }
                     LGHISJKZGQ.szyyWEB.Service szyy = new LGHISJKZGQ.szyyWEB.Service();
                    if (weburl != "")
                        szyy.Url = weburl;     
                   
                    string xmlstr = "";
                    
                    try
                    {
                       
                        szyy.Init();   //先要初始化
                      
                        xmlstr = szyy.GetPatiInfoByOrg(Ssbz, 1, orgID); //获取住院病人的信息 1表示住院病人
                        szyy.Dispose();
                        }
                    catch (Exception ee) 
                      {
                         
                        if (Debug == "1")
                    log.WriteMyLog("接收参数:"  + Sslbx + "," + Ssbz + "," +ee.ToString()); return "0"; }


                    if (xmlstr != "")
                    {
                        DataSet ds1 = new DataSet();
                        
                        XmlDocument xd = new XmlDocument();
                        if (Debug == "1") MessageBox.Show(xmlstr);
                        try
                        {
                         
                            StringReader sr = new StringReader(xmlstr);
                            XmlReader xr = new XmlTextReader(sr);
                            ds1.ReadXml(xr);
                       
                        }
                        catch { if (Debug == "1") log.WriteMyLog("接收参数:" + Sslbx + "," + Ssbz + "," + "XML解析错误"); return "0"; }
                       
                        try
                        {
                           
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["CARDNO"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ORDID"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + Ssbz + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["PATINAME"].ToString() + (char)34 + " ";
                            string PSEX = ds1.Tables[0].Rows[0]["SEXID"].ToString();
                            if (PSEX == "1") { PSEX = "女"; }
                            else
                            {
                                if (PSEX == "0") PSEX = "男";
                                else PSEX = " ";
                            }
                            xml = xml + "性别=" + (char)34 + PSEX + (char)34 + " ";
                            string nl1 = ds1.Tables[0].Rows[0]["BIRTHDATE"].ToString();
                            string nl = "0岁";
                          
                            try
                            { nl = (DateTime.Now.Year - DateTime.Parse(nl1.Substring(0, 10)).Year).ToString() + "岁"; }
                            catch
                            { nl = "0岁"; }
                            xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + ds1.Tables[0].Rows[0]["ORGNAME"].ToString() + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["BEDDES"].ToString() + (char)34 + " ";
                            xml = xml + "身份证号=" +(char)34+ " " + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["ORGNAME1"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["EMPNAME"].ToString() + (char)34 + " ";
                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            string DIAGNAME = "";
                            try
                            {
                                DIAGNAME = ds1.Tables[0].Rows[0]["DIAGNAME"].ToString();//有些xml中 可能是没有DIAGNAME字段的
                            }
                            catch
                            {DIAGNAME="";
                            }
                            xml = xml + "<临床诊断><![CDATA[" + DIAGNAME+ "]]></临床诊断>";
                            xml = xml + "</LOGENE>";
                           
                            return xml;
                        }
                        catch(Exception eee)
                        {
                            if (Debug == "1")
                                log.WriteMyLog("接收参数:" + Sslbx + "," + Ssbz + "," + eee.ToString());
                            return "0";
                        }
                    }
                    else
                    {
                    
                        MessageBox.Show("无此住院号病人信息，请重新输入！");
                        return "0";
                    }

                }

                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    return "0";
                }



            }
            else
            {
                MessageBox.Show("无此" + Sslbx);
                if (Debug == "1")
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");

                return "0";
            }




        }

    }
}
