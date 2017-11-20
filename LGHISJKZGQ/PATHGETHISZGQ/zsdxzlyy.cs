using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;
using LGHISJKZGQ;
using System.Runtime.InteropServices;

namespace LGHISJKZGQ
{
    //中山大学附属肿瘤医院  webservice
    class zsdxzlyy
    {
        [DllImport(("rfid.dll"), EntryPoint = "LinkCard", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern bool LinkCard();

        [DllImport(("rfid.dll"), EntryPoint = "UnlinkCard", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool UnlinkCard();

        [DllImport(("rfid.dll"), EntryPoint = "HexReadCardData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool HexReadCardData(StringBuilder sData, int nBlock, int sPassType, string sPassWord);

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)

        {
         string exp = "";
         string pathWEB = f.ReadString(Sslbx, "wsurl", ""); //获取sz.ini中设置的webservicesurl
         string sjdw = f.ReadString(Sslbx, "sjdw", "1"); //获取sz.ini中设置的webservicesurl
         string mrks = f.ReadString(Sslbx, "mrks", ""); //获取sz.ini中设置的mrks
         debug = f.ReadString(Sslbx, "debug", ""); //获取sz.ini中设置的mrks
       
            if (Sslbx != "")
            {
                if (Sslbx == "病历号")
                {
                    int len = Ssbz.Trim().Length;
                    if (len < 10)
                    {
                        for (int z = 0; z < 10 - len; z++)
                        {
                            Ssbz = "0" + Ssbz;
                        }
                    }

                }
                string rtn_XML = "";

  
              
              if (Sslbx == "诊疗卡读卡" || Sslbx == "诊疗卡" || Sslbx == "病历号")
                {
                    string jzkh="";
                    if (Sslbx == "诊疗卡读卡" )
                    {
                        if (LinkCard())
                        {
                            StringBuilder buff = new StringBuilder(100);
                            if (HexReadCardData(buff, 1, 0, "ff ff ff ff ff ff"))
                            {
                               jzkh=(buff.ToString().Substring(0, 8));
                                UnlinkCard();
                               if(jzkh.Trim()!="")
                               Ssbz=jzkh;
                                Sslbx="诊疗卡";
                            }
                            else
                            {  MessageBox.Show("读卡器失败");
                               UnlinkCard();
                               return "0";
                            }
                        }
                        else
                        { MessageBox.Show("连接读卡器失败"); return "0"; }
                    }



                    string type = "1";
                    if (Sslbx == "诊疗卡")
                    {
                        type = "0";  
                    }
                    string RUCAN = "<Request><Num>" + Ssbz.ToUpper().Trim() + "</Num><Type>" + type + "</Type></Request>";

                    ZSZLWeb.Service zszl = new LGHISJKZGQ.ZSZLWeb.Service();
                    if (pathWEB != "")
                        zszl.Url = pathWEB;

                    try
                    {
                        if (debug == "1")
                            MessageBox.Show("调阅webservice地址：" + zszl.Url);
                        rtn_XML = zszl.GetPatientInfoByCardorIcCard(RUCAN);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接WebService服务器异常:"+ee.Message.ToString());
                        log.WriteMyLog("参数：" + Ssbz + ";异常消息：" + ee.Message.ToString());
                        return "0";
                    }

                    if (debug == "1")
                        log.WriteMyLog(RUCAN+"\r\n"+rtn_XML);

                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("取信息异常，返回消息为空");
                        return "0";
                    }

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(rtn_XML);
                        string ResultCode = xd2.SelectSingleNode("/Response/ResultCode").InnerText;
                        string ErrorMsg = xd2.SelectSingleNode("/Response/ErrorMsg").InnerText;
                        if (ResultCode != "0")
                        {
                          //  MessageBox.Show("提取信息失败！" + ErrorMsg.Trim());
                            //取基本信息
                            return getbrxx(zszl.Url, RUCAN, sjdw, debug, mrks);
                        }
                        else
                            xmlok_DATA = xd2.SelectSingleNode("/Response/PatientInfoList");
                    }
                    catch (Exception xmlok_e)
                    {
                         return  "0";
                    }
                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        //MessageBox.Show("未找到对应的记录！");
                        //return "0";
                        //取基本信息
                        return getbrxx(zszl.Url, RUCAN, sjdw, debug, mrks);
                    }

                    DataSet ds = new DataSet();

                    try
                    {
                        StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);

                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("转dataset异常：" + eee.Message.ToString());
                        log.WriteMyLog("转dataset异常:" + eee);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {

                    //    //无申请单信息 区基本信息

                    //    MessageBox.Show("12");
                    //    rtn_XML = "";
                    //    try
                    //    {
                    //        rtn_XML = zszl.GetPatientBaseInfoByCardorIcCard(RUCAN);
                    //    }
                    //    catch (Exception ee)
                    //    {
                    //        MessageBox.Show("连接WebService服务器异常:" + ee.Message.ToString());
                    //        log.WriteMyLog("参数：" + Ssbz + ";异常消息：" + ee.Message.ToString());
                    //        return "0";
                    //    }

                    //    //if (debug == "1")
                    //    //    log.WriteMyLog(RUCAN + "\r\n" + rtn_XML);

                    //    if (rtn_XML.Trim() == "")
                    //    {
                    //        MessageBox.Show("取信息异常，返回消息为空");
                    //        return "0";
                    //    }

                    //    XmlNode xmlok_DATA_1 = null;
                    //    XmlDocument xd2_1 = new XmlDocument();
                    //    try
                    //    {
                    //        xd2_1.LoadXml(rtn_XML);
                    //        string ResultCode_1 = xd2_1.SelectSingleNode("/Response/ResultCode").InnerText;
                    //        string ErrorMsg_1 = xd2_1.SelectSingleNode("/Response/ErrorMsg").InnerText;
                    //        if (ResultCode_1 != "0")
                    //        {
                    //            MessageBox.Show("提取信息失败！" + ErrorMsg_1.Trim());
                    //            return "0";
                    //        }
                    //        else
                    //            xmlok_DATA_1 = xd2_1.SelectSingleNode("/Response/PatientInfoList");
                    //    }
                    //    catch (Exception xmlok_e2)
                    //    {
                    //        MessageBox.Show("解析XML异常：" + xmlok_e2.ToString());
                    //        return "0";
                    //    }
                    //    if (xmlok_DATA_1.InnerXml.Trim() == "")
                    //    {
                    //        MessageBox.Show("未找到对应的记录！");
                    //        return "0";
                    //    }

                    ////    DataSet ds = new DataSet();

                    //    try
                    //    {
                    //        StringReader sr = new StringReader(xmlok_DATA_1.OuterXml);
                    //        XmlReader xr = new XmlTextReader(sr);
                    //        ds.ReadXml(xr);

                    //    }
                    //    catch (Exception eee)
                    //    {
                    //        MessageBox.Show("转dataset异常：" + eee.Message.ToString());
                    //        log.WriteMyLog("转dataset异常:" + eee);
                    //        return "0";
                    //    }



                    //    if (ds.Tables[0].Rows.Count < 1)
                    //    {
                    //        MessageBox.Show("未查到相应的记录！"); return  "0";
                    //    }

                        return getbrxx(zszl.Url, RUCAN, sjdw, debug, mrks);
                    }
                    DataTable dt = new DataTable();
                  //  dt = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                       
                        string Columns = f.ReadString(Sslbx, "Columns", "PATIENTID,CARD_NO,IC_CARDNO,NAME");//显示的项目
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "PATIENTID,CARD_NO,IC_CARDNO,NAME");//显示的项目
                        string Col = f.ReadString(Sslbx, "RowFilter", ""); //选择条件的项目
                        string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目

                        FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0], -1, Columns, ColumnsName, Col, xsys);
                        yc.ShowDialog();
                        string string1 = yc.F_STRING[0];
                        string string2 = yc.F_STRING[1];
                        string string3 = yc.F_STRING[2];
                        string string4 = yc.F_STRING[3];

                        if (string1.Trim() == "" && string2.Trim() == "")
                        {
                            MessageBox.Show("未选择记录");
                            return "0";
                        }
                        DataView view = new DataView();
                        view.Table = ds.Tables[0];


                        string odr = "" + ds.Tables[0].Columns[0].ColumnName + "='" + string1 + "'  and  " + ds.Tables[0].Columns[1].ColumnName + "='" + string2 + "'  and  " + ds.Tables[0].Columns[2].ColumnName + "='" + string3 + "' and  " + ds.Tables[0].Columns[3].ColumnName + "='" + string4 + "'";

                        if (Col.Trim() != "")
                        {
                            string[] colsss = Col.Split(',');
                            odr = "" + colsss[0] + "='" + yc.F_STRING[0] + "'";
                            if (colsss.Length > 1)
                            {
                                for (int i = 1; i < colsss.Length; i++)
                                {
                                    if (i < 4)
                                        odr = odr + " and  " + colsss[i] + "='" + yc.F_STRING[i] + "' ";
                                }
                            }
                        }

                        view.RowFilter = odr;

                        dt = view.ToTable();
                    }
                    else
                        dt = ds.Tables[0];
                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["PATIENTID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["IC_CARDNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        string zyh = "";
                        //string mzh = ""; 
                        //if (dt.Rows[0]["FLAG"].ToString().Trim()=="1")
                            zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();
                        //else
                        //    mzh = dt.Rows[0]["CARD_NO"].ToString().Trim();

                     
                        try
                        {
                          
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                           }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + zyh+ (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string xb = dt.Rows[0]["SEX_CODE"].ToString().Trim();
                            if (xb.Trim() == "F") xb = "女";
                            else if (xb.Trim() == "M") xb = "男";
                            else xb = "";
                            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                         
                      
                           
                            string CSRQ = dt.Rows[0]["BIRTHDAY"].ToString().Trim().Substring(0,10);
                        
                            string datatime = DateTime.Today.Date.ToString();

                            if (CSRQ != "")
                            {
                                if (CSRQ.Contains("-"))
                                    CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                                int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                                if (Year >=1)
                                {
                                   //xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                    if (Month > 0)
                                        xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                    if (Month < 0)
                                        xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                    if (Month == 0)
                                    {
                                        if (day >= 0)
                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                        else
                                            xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                    }
                                }
                                else
                                 if (Year == 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                            int m = day1 / 30;
                                            int d = day1 % 30;
                                            xml = xml + "年龄=" + (char)34 + m + "月" + d + "天" + (char)34 + " ";
                                        }
                            }
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string hy = dt.Rows[0]["MARI"].ToString().Trim();
                            if (hy == "M") hy = "已婚";
                            else if (hy == "B") hy = "未婚";
                            else hy = "";
                            //else if (hy == "D") hy = "离婚";
                            //else if (hy == "W") hy = "丧偶";
                          
                            xml = xml + "婚姻=" + (char)34 + hy + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                           // xml = xml + "地址=" + (char)34 + dt.Rows[0]["HOME"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt.Rows[0]["HOME_TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt.Rows[0]["BED_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["IDENNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["DEPT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["DOCT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        if(sjdw.Trim()=="0")
                        xml = xml + "送检医院=" + (char)34 + "" + (char)34 + " ";
                        else
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["ITEM_NAME"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 +""+ (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            if (dt.Rows[0]["FLAG"].ToString().Trim() == "1")
                                      xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                                else
                                      xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";

                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["DIAG_NAME"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                      

                        return xml;
                    }
                    catch (Exception e)
                    {
                     
                        MessageBox.Show("提取信息出错;"+e.ToString());
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                        log.WriteMyLog(Sslbx + Ssbz + "不存在！");

                    return "0";

                }
            } return "0";


        }
        
        public static string getbrxx(string url, string RUCAN,string  sjdw, string debug,string mrks)
        {
         string exp = "";
               ZSZLWeb.Service zszl = new LGHISJKZGQ.ZSZLWeb.Service();
                    zszl.Url =url;
               string rtn_XML = "";
                    try
                    {
                        if (debug == "1")
                            MessageBox.Show("调阅webservice地址：" + zszl.Url);
                        rtn_XML = zszl.GetPatientBaseInfoByCardorIcCard(RUCAN);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接WebService服务器异常:"+ee.Message.ToString());
                     
                        return "0";
                    }

                    if (debug == "1")
                        log.WriteMyLog(RUCAN+"\r\n"+rtn_XML);

                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("取信息异常，返回消息为空");
                        return "0";
                    }

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(rtn_XML);
                        string ResultCode = xd2.SelectSingleNode("/Response/ResultCode").InnerText;
                        string ErrorMsg = xd2.SelectSingleNode("/Response/ErrorMsg").InnerText;
                        if (ResultCode != "0")
                        {
                            MessageBox.Show("提取信息失败！HIS返回：" + ErrorMsg.Trim());
                            return "0";
                        }
                        else
                            xmlok_DATA = xd2.SelectSingleNode("/Response/PatientInfoList");
                    }
                    catch (Exception xmlok_e)
                    {
                        MessageBox.Show("解析XML异常：" + xmlok_e.ToString());
                        return "0";
                    }
                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        MessageBox.Show("未找到对应的记录！");
                        return "0";
                    }

                    DataSet ds = new DataSet();

                    try
                    {
                        StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);

                    }
                    catch (Exception eee)
                    {
                            MessageBox.Show("转dataset异常：" + eee.Message.ToString());
                        log.WriteMyLog("转dataset异常:" + eee);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {

                            MessageBox.Show("未查到相应的记录！"); return  "0";
       
                    }
                    DataTable dt = new DataTable();
                   dt = ds.Tables[0];
                 
                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["PATIENTID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["IC_CARDNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                      //  string zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();
                        string zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();

                        //if (dt.Rows[0]["FLAG"].ToString().Trim()=="1")
                        //    zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();
                        //else
                        //    mzh = dt.Rows[0]["CARD_NO"].ToString().Trim();

                     
                        try
                        {
                          
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                           }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + zyh +(char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string xb = dt.Rows[0]["SEX_CODE"].ToString().Trim();
                            if (xb.Trim() == "F") xb = "女";
                            else if (xb.Trim() == "M") xb = "男";
                            else xb = "";
                            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                         
                      
                          
                            string CSRQ = dt.Rows[0]["BIRTHDAY"].ToString().Trim().Substring(0,10);
                        
                            string datatime = DateTime.Today.Date.ToString();

                            if (CSRQ != "")
                            {
                                if (CSRQ.Contains("-"))
                                    CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                                int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                                if (Year >=1)
                                {
                                   //xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                    if (Month > 0)
                                        xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                    if (Month < 0)
                                        xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                    if (Month == 0)
                                    {
                                        if (day >= 0)
                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                        else
                                            xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                    }
                                }
                                else
                                 if (Year == 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                            int m = day1 / 30;
                                            int d = day1 % 30;
                                            xml = xml + "年龄=" + (char)34 + m + "月" + d + "天" + (char)34 + " ";
                                        }
                            }
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string hy = dt.Rows[0]["MARI"].ToString().Trim();
                            if (hy == "M") hy = "已婚";
                            else if (hy == "B") hy = "未婚";
                            else hy = "";
                            //else if (hy == "D") hy = "离婚";
                            //else if (hy == "W") hy = "丧偶";
                          
                            xml = xml + "婚姻=" + (char)34 + hy + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                           // xml = xml + "地址=" + (char)34 + dt.Rows[0]["HOME"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt.Rows[0]["HOME_TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt.Rows[0]["BED_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["IDENNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            if (mrks.Trim()!="")
                             xml = xml + "送检科室=" + (char)34 + mrks.Trim() + (char)34 + " ";
                            else
                            xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["DEPT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        if(sjdw.Trim()=="0")
                        xml = xml + "送检医院=" + (char)34 + "" + (char)34 + " ";
                        else
                               xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 +""+ (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            if (dt.Rows[0]["FLAG"].ToString().Trim() == "1")
                                xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                            else
                                xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";


                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                      

                        return xml;
                    }
                    catch (Exception e)
                    {
                     
                        MessageBox.Show("提取信息出错;"+e.ToString());
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                }
               
        

        }
    }

