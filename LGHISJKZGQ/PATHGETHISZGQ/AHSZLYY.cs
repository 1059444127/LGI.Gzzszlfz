using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Resources;
using System.Net;

namespace LGHISJKZGQ
{
   public class AHSZLYY
    {
        //安徽省肿瘤医院
       //webservices
       private static IniFiles f = new IniFiles(Application.StartupPath +"\\sz.ini");
       
        
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            if (Sslbx != "")
            {

                string pathWEB = f.ReadString(Sslbx, "webservicesurl", "");
                string sfzzk = f.ReadString(Sslbx, "sfzzk", "");
                 Debug = f.ReadString(Sslbx, "Debug", "");

                 #region  登记号
                 if (Sslbx == "登记号")
                {
                   
                    string rtn = "";
                    try
                    {
                        LGHISJKZGQ.ahzlyyWeb.XDInterface ahzlyy = new LGHISJKZGQ.ahzlyyWeb.XDInterface();
                       if (pathWEB != "") ahzlyy.Url = pathWEB;
                        rtn = ahzlyy.BLAPPLYBYAPPCardNo(Ssbz);
                        ahzlyy.Dispose();
                    }
                    catch(Exception ee)
                    {
                        if (Debug=="1")
                        log.WriteMyLog("调用webservices出现问题！"+ee.ToString());
                         return "0";
                    }
                    if (rtn.Trim() == "" || rtn == null)
                    {
                        MessageBox.Show("没有该病人信息，请核实申请号是否正确！");
                        return "0";
                    }
                       if(rtn=="<Response></Response>")
                            {
                                 MessageBox.Show("没有该病人信息，请核实申请号是否正确！");
                        return "0";
                            }
                             XmlNode xmlok = null;
                             XmlDocument xd = new XmlDocument();
                        try
                         {
                             xd.LoadXml(rtn);
                             xmlok = xd.SelectSingleNode("/Response");
                         }
                        catch(Exception e)
                        {
                            log.WriteMyLog(e.ToString());
                            MessageBox.Show("没有该病人信息，请核实号码是否正确！");
                            return "0";
                        }
                     DataSet ds = new DataSet();
                        try
                        {
                           

                            StringReader sr = new StringReader(xmlok.OuterXml);
                            XmlReader xr = new XmlTextReader(sr);
                            ds.ReadXml(xr);

                            //MessageBox.Show(ds.Tables[0].Rows.Count.ToString());
                        }
                        catch
                        {

                        }
                        string xml = "";
                        if (ds.Tables[0].Rows.Count <= 0)
                        {
                         
                            MessageBox.Show("没有该病人信息，请核实号码是否正确！");
                            return "0";
                        }
                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            AHSZLYY_Frm ah = new AHSZLYY_Frm(ds);
                            ah.ShowDialog();
                            string string1 = ah.F_STRING[0];
                            string string2 = ah.F_STRING[1];
                            string string3 = ah.F_STRING[2];
                            string string4 = ah.F_STRING[3];

                            if (string1.Trim() == "" && string2.Trim() == "")
                            {
                                MessageBox.Show("未选择记录");
                                return "0";
                            }
                            DataView view = new DataView();
                            view.Table = ds.Tables[0];


                            string odr = "" + ds.Tables[0].Columns[0].ColumnName + "='" + string1 + "'  and  " + ds.Tables[0].Columns[1].ColumnName + "='" + string2 + "'  and  " + ds.Tables[0].Columns[2].ColumnName + "='" + string3 + "' and  " + ds.Tables[0].Columns[3].ColumnName + "='" + string4 + "'";
                  
                        }
                        else
                        {
                            xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";

                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["PATIENTID"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + ds.Tables[0].Rows[0]["APPNO"].ToString() + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["NOTENO"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["OUTPATIENTNO"].ToString() + (char)34 + " ";

                            xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[0]["INPATIENTNO"].ToString() + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["NAME"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[0]["SEX"].ToString() + (char)34 + " ";

                            int nl = DateTime.Now.Year - DateTime.Parse(ds.Tables[0].Rows[0]["BIRTHDAY"].ToString()).Year;
                            xml = xml + "年龄=" + (char)34 + nl.ToString() + "岁" + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[0]["STUDYROOM"].ToString() + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["BEDNO"].ToString() + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["CDEPTCODE"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["DOCTNAME"].ToString() + (char)34 + " ";
                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[0]["PATSPECI"].ToString() + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + (char)34 + " ";
                            string brlb = ds.Tables[0].Rows[0]["PATIENTSOURCE"].ToString();
                            if (brlb == "2") brlb = "住院";
                            if (brlb == "1") brlb = "门诊";
                            xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                            xml = xml + "/>";
                            if (sfzzk == "1")
                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            else
                                xml = xml + "<临床病史><![CDATA[" + ds.Tables[0].Rows[0]["PATCLINICRECD"].ToString() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[0]["DIAGNAME"].ToString() + "]]></临床诊断>";


                            xml = xml + "</LOGENE>";
                        }
                      
                        if (sfzzk == "1")
                        {
                           
                          xml= gyxxml.zh(xml);
                        }
                        if (Debug == "1")
                        {
                            MessageBox.Show(xml);
                            log.WriteMyLog(xml);
                        }
                        return xml;

                    }

                 #endregion

                 #region  卡号
                    if (Sslbx == "卡号")
                    {
                        string rtn = "";
                        try
                        {
                            LGHISJKZGQ.ahzlyyWeb.XDInterface ahzlyy = new LGHISJKZGQ.ahzlyyWeb.XDInterface();
                            if (pathWEB != "") ahzlyy.Url = pathWEB;
                            rtn = ahzlyy.BLAPPLYByCardNo(Ssbz);
                            ahzlyy.Dispose();
                        }
                        catch (Exception ee)
                        {
                            if (Debug == "1")
                                log.WriteMyLog("调用webservices出现问题！" + ee.ToString());
                            return "0";
                        }
                        if (rtn.Trim() == "" || rtn == null)
                        {
                            MessageBox.Show("没有该病人信息，请核实申请号是否正确！");
                            return "0";
                        }
                        if (rtn == "<Response></Response>")
                        {
                            MessageBox.Show("没有该病人信息，请核实申请号是否正确！");
                            return "0";
                        }
                        XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();
                        try
                        {
                            xd.LoadXml(rtn);
                            xmlok = xd.SelectSingleNode("/Response/BL");
                        }
                        catch (Exception e)
                        {
                            log.WriteMyLog(e.ToString());
                            MessageBox.Show("没有该病人信息，请核实医嘱号是否正确！");
                            return "0";
                        }
                       
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + xmlok["PATIENTID"].InnerText + (char)34 + " ";
                        xml = xml + "就诊ID=" +  (char)34 + (char)34+" ";
                        xml = xml + "申请序号=" + (char)34 + xmlok["NOTENO"].InnerText + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + xmlok["OUTPATIENTNO"].InnerText + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + xmlok["INPATIENTNO"].InnerText + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + xmlok["NAME"].InnerText + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + xmlok["SEX"].InnerText + (char)34 + " ";
                        int nl = DateTime.Now.Year - DateTime.Parse(xmlok["BIRTHDAY"].InnerText).Year;
                        xml = xml + "年龄=" + (char)34 + nl.ToString() + "岁" + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + xmlok["STUDYROOM"].InnerText + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + xmlok["BEDNO"].InnerText + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + (char)34 + " ";
                       
                        xml = xml + "送检科室=" + (char)34 + xmlok["CDEPTCODE"].InnerText + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + xmlok["DOCTNAME"].InnerText + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" +  (char)34 + (char)34 +" ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34  + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + (char)34 + " ";
                      
                        string brlb = xmlok["PATIENTSOURCE"].InnerText;
                        if (brlb == "2") brlb = "住院";
                        if (brlb == "1") brlb = "门诊";
                        xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + xmlok["DIAGNAME"].InnerText + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";




                        if (sfzzk == "1")
                        {

                            xml = gyxxml.zh(xml);
                        }
                        if (Debug == "1")
                        {
                            MessageBox.Show(xml);
                            log.WriteMyLog(xml);
                        }

                        return xml;

                    }
                    # endregion
                    MessageBox.Show("无此" + Sslbx);
                    return "0";
                  
                }


                MessageBox.Show("无此" + Sslbx);
                return "0";


        }
    }
}

