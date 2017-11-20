using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
namespace LGHISJKZGQ
{
    class webservertohis
    {
        private static IniFiles f = new IniFiles("sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            if (Sslbx != "")
            {
                 if (Sslbx == "门诊号")
                {
                    DataSet ds1 = new DataSet(); XmlNode xn;
                    try
                    {
                        LGHISJKZGQ.webServerAA.Service ws = new LGHISJKZGQ.webServerAA.Service();
                        string xmlStr = ws.HelloWorld(Ssbz);
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(xmlStr);

                         xn = xd.SelectSingleNode("//row");
                    }
                    catch
                    {
                        MessageBox.Show("读取XML文件错误");
                        return "0";
                    }
                    try
                    {
                        string aa = xn.OuterXml;
                        aa = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>" + aa;
                        StringReader sr = new StringReader(aa);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                    }
                    catch
                    {
                        MessageBox.Show("XML转DataSet出错");
                        return "0";
                    }
                    
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                      try
                    {
                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["病人编号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["门诊号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["住院号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["姓名"].ToString().Trim() + (char)34 + " ";

                        //string xb = "";
                        //if (ds1.Tables[0].Rows[0]["XB"].ToString().Trim() == "1") xb = "男";
                        //if (ds1.Tables[0].Rows[0]["XB"].ToString().Trim() == "2") xb = "女";

                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["性别"].ToString().Trim() + (char)34 + " ";

                        //string nl = "0岁";
                        //try
                        //{
                        //    nl = datediff(DateTime.Now, Convert.ToDateTime(ds1.Tables[0].Rows[0]["CSRQ"].ToString().Trim()));
                        //}
                        //catch
                        //{
                        //    nl = "0岁";
                        //}
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["年龄"].ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "地址:" + ds1.Tables[0].Rows[0]["地址"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "电话=" + (char)34 + "电话:" + ds1.Tables[0].Rows[0]["电话"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[0]["身份证号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + ds1.Tables[0].Rows[0]["民族"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + ds1.Tables[0].Rows[0]["职业"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                        //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + ds1.Tables[0].Rows[0]["费别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + " " + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                    }
                    catch
                    {
                        MessageBox.Show("列名字段出错");
                        return "0";
                    }
                    return xml;

                }
                //直接读取xml节点
                if (Sslbx == "住院号")
                { XmlNode xmlok=null;
                    try
                    {
                        LGHISJKZGQ.webServerAA.Service ws = new LGHISJKZGQ.webServerAA.Service();
                        string xmlStr = ws.HelloWorld(Ssbz);
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(xmlStr);
                         xmlok = xd.SelectSingleNode("/LOGENE");
                    }
                    catch { MessageBox.Show("XML读取错误"); return "0"; }
                   
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + xmlok["row"].GetAttribute("病人编号").ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + xmlok["row"].GetAttribute("就诊ID").ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + xmlok["row"].GetAttribute("申请序号").ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + xmlok["row"].GetAttribute("门诊号").ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + xmlok["row"].GetAttribute("住院号").ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + xmlok["row"].GetAttribute("姓名").ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + xmlok["row"].GetAttribute("病人编号").ToString() + (char)34 + " ";
                        //string nl = "0岁";
                        //try
                        //{
                        //    nl = datediff(DateTime.Now, Convert.ToDateTime(ds1.Tables[0].Rows[0]["CSRQ"].ToString().Trim()));
                        //}
                        //catch
                        //{
                        //    nl = "0岁";
                        //}

                        xml = xml + "年龄=" + (char)34 + xmlok["row"].GetAttribute("年龄").ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + xmlok["row"].GetAttribute("婚姻").ToString() + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "地址:" + xmlok["row"].GetAttribute("地址").ToString() + (char)34 + " ";
                        xml = xml + "电话=" + (char)34 + "电话:" + xmlok["row"].GetAttribute("电话").ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + xmlok["row"].GetAttribute("病区").ToString() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + xmlok["row"].GetAttribute("床号").ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + xmlok["row"].GetAttribute("身份证号").ToString() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + xmlok["row"].GetAttribute("民族").ToString() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + xmlok["row"].GetAttribute("职业").ToString() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + xmlok["row"].GetAttribute("送检科室").ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + xmlok["row"].GetAttribute("送检医生").ToString() + (char)34 + " ";
                        //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                        //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + xmlok["row"].GetAttribute("收费").ToString() + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + xmlok["row"].GetAttribute("标本名称").ToString() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + xmlok["临床病史"].InnerText + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + xmlok["临床诊断"].InnerText + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                    
                    return xml;

                }
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    if (Debug == "1")
                        log.WriteMyLog(Sslbx + Ssbz + "不存在！");

                    return "0";
                }

            }

            MessageBox.Show(Sslbx + "类型未设置！");
            if (Debug == "1")
                log.WriteMyLog(Sslbx + "类型未设置！");
            return "0";
        }

        public static XmlDocument RunProc_ds(string Ssbz)
        {
            LGHISJKZGQ.webServerAA.Service ws = new LGHISJKZGQ.webServerAA.Service();

            string xmlStr = ws.HelloWorld(Ssbz);
            XmlDocument dom = new XmlDocument();
            dom.LoadXml(xmlStr);
            return dom;

        }
    }
}
