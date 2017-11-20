using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace LGHISJKZGQ
{
    class zjgsdyrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            string exp = "";
            string wsweb = f.ReadString(Sslbx, "wsweb", ""); //获取sz.ini中设置的webservicesurl
            Debug = f.ReadString(Sslbx, "debug", "");

            if (Sslbx != "")
            {
                zjgsdyrmyyweb.PlatformCommonSvr zjg = new LGHISJKZGQ.zjgsdyrmyyweb.PlatformCommonSvr();
                if (wsweb != "")
                    zjg.Url = wsweb;
                string rtn_XML = "";

                #region 门诊
                if (Sslbx == "门诊号")
                {

                    string xmlcontext = "<Response><bill_id>" + Ssbz + "</bill_id></Response>";

                    if (Debug == "1")
                        log.WriteMyLog(xmlcontext);
                    try
                    {
                        rtn_XML = zjg.CurServer("LANGJIA", "OS001", xmlcontext);
                        if (Debug == "1")
                            log.WriteMyLog("返回：" + rtn_XML);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接服务器异常！" + ee.Message);

                        log.WriteMyLog("异常消息：" + ee.ToString());
                        return "0";
                    }
                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("取信息异常，返回消息为空");
                        return "0";
                    }

                    DataSet ds = new DataSet();
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        StringReader sr = new StringReader(rtn_XML);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("转dataset异常：" + eee.Message);
                        log.WriteMyLog("转dataset异常:" + eee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("未查到相应的记录！");
                        return "0";
                    }

                    try
                    {
                        MessageBox.Show(ds.Tables[0].Rows[0]["Msg"].ToString());
                        return "0";
                    }
                    catch
                    {
                    }
                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";

                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["CardID"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["cureno"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["Name"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            string xb = ds.Tables[0].Rows[0]["Gender"].ToString().Trim();
                            if (xb == "1") xb = "男";
                            if (xb == "0") xb = "女";
                            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[0]["age"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[0]["address"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["DeptName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["doctorname"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        //----------------------------------------------------------
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        //----------------------------------------------------------
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        if (Debug == "1")
                            log.WriteMyLog(xml);

                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("提取信息出错，请重新操作");
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                }
                #endregion 

                #region 住院号
                ///
                if (Sslbx == "住院号")
                {
                   
                       string  xmlcontext="<Response><hospitalid>" + Ssbz + "</hospitalid></Response>";
    
                    if (Debug == "1")
                        log.WriteMyLog(xmlcontext);
                    try
                    {
                        rtn_XML = zjg.CurServer("LANGJIA", "IS001", xmlcontext);
                        if (Debug == "1")
                            log.WriteMyLog("返回："+rtn_XML);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接服务器异常！" + ee.Message);

                        log.WriteMyLog("异常消息：" + ee.ToString());
                        return "0";
                    }
                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("取信息异常，返回消息为空");
                        return "0";
                    }

                    DataSet ds = new DataSet();
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        StringReader sr = new StringReader(rtn_XML);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("转dataset异常：" + eee.Message);
                        log.WriteMyLog("转dataset异常:" + eee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("未查到相应的记录！");
                        return "0";
                    }
      
                    try
                    {
                        MessageBox.Show(ds.Tables[0].Rows[0]["Msg"].ToString());
                        return "0";
                    }
                    catch
                    {
                    }
                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                       
                       xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["his_id"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                       xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                       xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                       xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[0]["hospitalid"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["patname"].ToString().Trim() + (char)34 + " ";
                       
                        //----------------------------------------------------------
                        try
                        {
                            string xb = ds.Tables[0].Rows[0]["sex"].ToString().Trim();
                            if (xb == "M") xb = "男";
                            if (xb == "F") xb = "女";
                            xml = xml + "性别=" + (char)34 +xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            int age = DateTime.Today.Year - DateTime.Parse(ds.Tables[0].Rows[0]["birth"].ToString().Trim()).Year;
                         
                            xml = xml + "年龄=" + (char)34 + age.ToString() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                       
                         xml = xml + "婚姻=" + (char)34 +"" + (char)34 + " ";
                       
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[0]["address"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["telepone"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[0]["wardname"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["Bed"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                       
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["wardname"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["maindoctor"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                            xml = xml + "费别=" + (char)34 +"" + (char)34 + " ";
                          
                            xml = xml + "病人类别=" + (char)34 +"住院" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        if (Debug == "1")
                            log.WriteMyLog(xml);

                        return xml;

                    }
                    catch (Exception e)
                    {

                        MessageBox.Show("提取信息错误：" + e.Message);
                        log.WriteMyLog("提取信息错误：" + e.Message);
                        return "0";
                    }
                 
               
                
                }
   #endregion
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    return "0";

                }
            } return "0";


        }
    }
}
