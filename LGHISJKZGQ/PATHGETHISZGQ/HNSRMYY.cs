using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 湖南省人民医院  webservices
    /// </summary>
    class HNSRMYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
           
            string exp = "";
            string pathWEB = f.ReadString(Sslbx, "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
            Debug = f.ReadString(Sslbx, "debug", "");

            if (Sslbx != "")
            {
                LGHISJKZGQ.hnsrmyyWeb.TrasenWS rmyy = new LGHISJKZGQ.hnsrmyyWeb.TrasenWS();
                if (pathWEB != "")
                    rmyy.Url = pathWEB;
                string rtn_XML = "";

                //<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo>20140017175</TrmtNo><Fph></Fph></HEAD><DATA></DATA></PAYMENT>
                //<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo></TrmtNo><Fph>73400684</Fph></HEAD><DATA></DATA></PAYMENT>
                if (Sslbx == "卡号" || Sslbx == "发票号" || Sslbx == "挂号序号" || Sslbx == "身份证读卡" || Sslbx == "诊疗卡读卡")
                {

                    string ssbz="";
                    try
                    {
                      ssbz= Ssbz.Trim();
                    }
                    catch
                    {
                    }
                    string RUCAN ="";
                    if (Sslbx == "发票号")
                    {
                        if (ssbz.Length == 10)
                        {
                            ssbz = ssbz.Substring(0, 9);
                        }
                        RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo></TrmtNo><Fph>" + ssbz.Trim() + "</Fph><Ghxh></Ghxh></HEAD><DATA></DATA></PAYMENT>";
                    }
                    if (Sslbx == "挂号序号")
                        RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo></TrmtNo><Fph></Fph><Ghxh>" + ssbz.Trim() + "</Ghxh></HEAD><DATA></DATA></PAYMENT>";


                    if (Sslbx == "卡号" || Sslbx == "诊疗卡读卡" || Sslbx == "身份证读卡")
                    {
                        if (Sslbx == "诊疗卡读卡")
                        {
                            try
                            {
                                ssbz = ReadCardNo();
                                if (ssbz.Trim() == "") return "0";
                             }
                            catch (Exception e1)
                            {
                                MessageBox.Show(e1.Message); return "0";
                            }
                        }
                        if (Sslbx == "身份证读卡")
                        {
                            try
                            {
                                ssbz = getIDCardNo();
                                if (ssbz.Trim() == "") return "0";
                            }
                            catch (Exception e1)
                            {
                                MessageBox.Show(e1.Message);return "0";
                            }
                        }
                        RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo>" + ssbz.Trim() + "</TrmtNo><Fph></Fph><Ghxh></Ghxh></HEAD><DATA></DATA></PAYMENT>";
                    }

                    if (RUCAN == "")
                    {
                        MessageBox.Show("无效参数");
                        return "0";
                    }
                    try
                    {

                        if (Debug == "1")
                            log.WriteMyLog(RUCAN);

                        rtn_XML = rmyy.GetMPatinfo(RUCAN);

                        if (Debug == "1")
                            log.WriteMyLog(rtn_XML);

                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接wen服务器异常，可能网络故障！" + ee.Message);
                        log.WriteMyLog("参数：" + Ssbz + ";异常消息：" + ee.Message);
                        return "0";
                    }
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
                        xmlok_DATA = xd2.SelectSingleNode("/PAYMENT/DATA");
                    }
                    catch (Exception xmlok_e)
                    {
                        log.WriteMyLog(rtn_XML);
                        MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                        return xmlstr();
                    }
                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        log.WriteMyLog(rtn_XML);
                        MessageBox.Show("未找到对应的记录！(" + Sslbx + ":" + ssbz + ")");
                        return xmlstr();
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
                        if (Debug == "1")
                            MessageBox.Show("转dataset异常：" + eee.Message);
                        log.WriteMyLog("转dataset异常:" + eee.Message);
                        return xmlstr();
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("未查到相应的记录！(" + Sslbx +":"+ssbz+ ")");
                        return xmlstr();
                    }
                    // DataTable dt = new DataTable();

                    int z = 0;
                    if (ds.Tables[0].Rows.Count > 1)
                    {

                        DataColumn dc0 = new DataColumn("序号");
                        ds.Tables[0].Columns.Add(dc0);

                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                        }

                        Frm_hnsrmyy yc = new Frm_hnsrmyy(ds, "");
                        yc.ShowDialog();
                        //string GHRQ = yc.GHRQ;
                        //string FPH = yc.FPH;
                        //string GHXH = yc.GHXH;
                        //string CARDNO = yc.CARDNO;
                        string xh = yc.F_LAST_XH;

                        if (xh == "")
                        {
                            MessageBox.Show("未选择记录"); return "0";
                        }

                        z = int.Parse(xh);
                        //DataView view = new DataView();
                        //view.Table = ds.Tables[0];
                        //view.RowFilter = "GHRQ = '" + GHRQ + "' and FPH='" + FPH + "'";
                        // dt = ds.Tables[0];

                    }
                    //else
                    //{
                    //    dt = ds.Tables[0];
                    //}

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[z]["GHXH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + ds.Tables[0].Rows[z]["FPH"].ToString().Trim() + (char)34 + " ";
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
                        try
                        {
                            xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[z]["CARDNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[z]["NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[z]["SEX"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[z]["AGE"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[z]["MARTIAL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[z]["ADDREEST"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[z]["TEL"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[z]["SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + ds.Tables[0].Rows[z]["MINZU"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[z]["ZHIYE"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[z]["KSMC"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[z]["YSMC"].ToString().Trim() + (char)34 + " ";
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
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + ds.Tables[0].Rows[z]["FEELB"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
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
                            xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[z]["ZDMC"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("提取信息出错，请重新操作");
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                }

                ///
                ///
                ///
                if (Sslbx == "住院号")
                {
                    string zyh = Ssbz.Trim();
                    if (zyh.Length <= 6)
                        zyh = "00" + zyh;
                    string RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><InpatientNo>" + zyh + "</InpatientNo></HEAD><DATA></DATA></PAYMENT>";
                    try
                    {

                        if (Debug == "1")
                            log.WriteMyLog(RUCAN);

                        rtn_XML = rmyy.GetZPatinfo(RUCAN);

                        if (Debug == "1")
                            log.WriteMyLog(rtn_XML);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接服务器异常，可能网络故障！" + ee.Message);

                        log.WriteMyLog("参数：" + Ssbz + ";异常消息：" + ee.Message.ToString());
                        return "0";
                    }
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
                        xmlok_DATA = xd2.SelectSingleNode("/PAYMENT/DATA");

                    }
                    catch (Exception xmlok_e)
                    {
                        log.WriteMyLog(rtn_XML + "---" + xmlok_e.Message);
                        MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                        return xmlstr();
                    }

                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        MessageBox.Show("未找到对应的记录！");
                        return xmlstr();
                    }

                    DataSet ds = new DataSet();
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("转dataset异常：" + eee.Message);
                        log.WriteMyLog("转dataset异常:" + eee.Message);
                        return xmlstr();
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("未查到相应的记录！");
                        return xmlstr();
                    }

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["INPATIENTID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + ds.Tables[0].Rows[0]["TIMES"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[0]["INPATIENTNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["HZXM"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string age = ds.Tables[0].Rows[0]["AGE"].ToString().Trim();
                            ////if (age.Contains("年"))
                            ////{
                            ////    age = age.Substring(0, age.IndexOf("年"))+"岁";
                            ////}
                            xml = xml + "年龄=" + (char)34 + age + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[0]["MARTIAL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[0]["ADDREEST"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[0]["INDEPT"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["BEDNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[0]["SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + ds.Tables[0].Rows[0]["MINZU"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[0]["ZHIYE"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["SJDEPT"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["YSMC"].ToString().Trim() + (char)34 + " ";
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
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + ds.Tables[0].Rows[0]["JSFSNAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病人类别=" + (char)34 + ds.Tables[0].Rows[0]["WardOrReg"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
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
                            xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[0]["ZDMC"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;

                    }
                    catch (Exception e)
                    {

                        MessageBox.Show("提取信息出错，请重新操作");
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                }

                ///
                ///体检
                ///
                if (Sslbx == "体检号" || Sslbx == "体检条码号")
                {
                    DataSet ds = new DataSet();
                    string sqlstr = "select djh as 病人编号, '体检' as 病人类别,'' as 费别,'' as 住院号,djh as 门诊号,name as 姓名,sex as 性别,cast((datediff(year,sr,getdate())) as varchar(10))+'岁'  as 年龄,' ' as 婚姻,address as 地址,phone1 as 电话,'' AS 病区,'' as 床号,sfzhm as 身份证号,'' as 民族,' ' as 职业,ksmc as 送检科室,sqr AS 送检医生,'' as 临床诊断, ' ' AS 临床病史,'' as 收费,xmdm as 就诊ID,' ' as 申请序号,' ' as 标本名称,'本院' AS 送检医院,xmmc as 医嘱项目 from  v_bl_tj where  STUDYMETHOD='病理' and djh= 'f_sbh'";
                    SqlConnection sqlcon = null;
                    string odbcstr = f.ReadString(Sslbx, "odbcsql", "Server=192.168.10.28;Database=TJ_Trasen;User Id=emr;Password=rmyyemr;"); //获取sz.ini中设置的odbcsql
                    sqlstr = f.ReadString(Sslbx, "hissql", sqlstr);
                    sqlstr = sqlstr.Replace("f_sbh", Ssbz.Trim());
                    if (Debug == "1")
                        MessageBox.Show(sqlstr);
                    try
                    {
                        sqlcon = new SqlConnection(odbcstr);
                        SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, sqlcon);
                        sqlda.Fill(ds, "tjxx");
                        sqlda.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("体检系统数据库连接错误！！！");
                        return "0";
                    }
                    finally
                    {
                        if (sqlcon.State == ConnectionState.Open)
                            sqlcon.Close();
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("此体检号无记录！");
                        return "0";
                    }

                    //DataTable dt = new DataTable();
                    //if (ds.Tables[0].Rows.Count > 1)
                    //{
                    //    Frm_hnsrmyy yc = new Frm_hnsrmyy(ds, "体检");
                    //    yc.ShowDialog();
                    //    string xmid = yc.FPH;
                    //    string tjh = yc.CARDNO;
                    //    if (xmid.Trim() == "")
                    //    {
                    //        MessageBox.Show("未选择记录");
                    //        return "0";
                    //    }
                    //    DataView view = new DataView();
                    //    view.Table = ds.Tables[0];
                    //    view.RowFilter = "就诊ID='" + xmid + "'  and  病人编号='" + tjh + "'";
                    //    dt = view.ToTable();
                    //}
                    //else
                    //    dt = ds.Tables[0];

                    int z = 0;
                    if (ds.Tables[0].Rows.Count > 1)
                    {

                        DataColumn dc0 = new DataColumn("序号");
                        ds.Tables[0].Columns.Add(dc0);

                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                        }

                        Frm_hnsrmyy yc = new Frm_hnsrmyy(ds, "");
                        yc.ShowDialog();
                        string xh = yc.F_LAST_XH;

                        if (xh == "")
                        {
                            MessageBox.Show("未选择记录"); return "0";
                        }

                        z = int.Parse(xh);
                    }

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[z]["病人编号"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + ds.Tables[0].Rows[z]["就诊ID"].ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[z]["申请序号"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[z]["门诊号"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[z]["住院号"].ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[z]["姓名"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[z]["性别"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[z]["年龄"].ToString() + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[z]["婚姻"].ToString() + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[z]["地址"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[z]["电话"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[z]["病区"].ToString() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[z]["床号"].ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[z]["身份证号"].ToString() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + ds.Tables[0].Rows[z]["民族"].ToString() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[z]["职业"].ToString() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[z]["送检科室"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[z]["送检医生"].ToString() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + ds.Tables[0].Rows[z]["收费"].ToString() + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[z]["标本名称"].ToString() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + ds.Tables[0].Rows[z]["送检医院"].ToString() + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + ds.Tables[0].Rows[z]["医嘱项目"].ToString() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + ds.Tables[0].Rows[z]["费别"].ToString() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + ds.Tables[0].Rows[z]["病人类别"].ToString() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + ds.Tables[0].Rows[z]["临床病史"].ToString() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[z]["临床诊断"].ToString() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("提取信息出错，请重新操作");
                        log.WriteMyLog("xml解析错误---" + e.ToString());
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
            } return "0";


        }
        public static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + (char)34 + " ";
            xml = xml + "门诊号=" + (char)34 + (char)34 + " ";
            xml = xml + "住院号=" + (char)34 + (char)34 + " ";
            xml = xml + "姓名=" + (char)34 + (char)34 + " ";

            xml = xml + "性别=" + (char)34 + (char)34 + " ";

            xml = xml + "年龄=" + (char)34 + (char)34 + " ";
            xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + (char)34 + "   ";
            xml = xml + "电话=" + (char)34 + (char)34 + " ";
            xml = xml + "病区=" + (char)34 + (char)34 + " ";
            xml = xml + "床号=" + (char)34 + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + (char)34 + " ";
            //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
            //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + (char)34 + " ";

            xml = xml + "病人类别=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + "]]></临床诊断>";
            xml = xml + "</LOGENE>";
            return xml;
        }

        public static string getIDCardNo()
        {

            try
            {
                byte[] pucIIN1 = new byte[4];
                iPort = Syn_FindUSBReader();
                if (Syn_OpenPort(iPort) != 0)
                {
                    Syn_ClosePort(iPort);
                    MessageBox.Show("读卡器连接失败"); return "";
                }
               
                if (Syn_SelectIDCard(iPort, ref  pucIIN1[0], 0) != 0)
                {
                    //Syn_ClosePort(iPort);
                    //MessageBox.Show("未找到卡"); return;
                }

              
                byte[] pucIIN = new byte[4];
                byte[] pucSN = new byte[8];
                byte[] cardid = new byte[8];

                IDCardData CardMsg = new IDCardData();
                Syn_ClosePort(iPort);
                int nRet = Syn_OpenPort(iPort);
                if (nRet == 0)
                {
                    nRet = Syn_GetSAMStatus(iPort, 0);
                    nRet = Syn_StartFindIDCard(iPort, ref pucIIN[0], 0);
                    nRet = Syn_SelectIDCard(iPort, ref pucSN[0], 0);
                    if (Syn_ReadMsg(iPort, 0, ref  CardMsg) == 0)
                    {
                        Syn_ClosePort(iPort);
                        return CardMsg.IDCardNo;
                        //listBox1.Items.Add("姓名：" + CardMsg.Name);
                        //listBox1.Items.Add("性别：" + CardMsg.Sex);
                        //listBox1.Items.Add("民族：" + CardMsg.Nation);
                        //listBox1.Items.Add("出生日期：" + CardMsg.Born);
                        //listBox1.Items.Add("住址：" + CardMsg.Address);
                        //listBox1.Items.Add("身份证号：" + CardMsg.IDCardNo);
                        //listBox1.Items.Add("发证机关：" + CardMsg.GrantDept);
                        //listBox1.Items.Add("有效期：" + CardMsg.UserLifeBegin + "-" + CardMsg.UserLifeEnd);
                        //listBox1.Items.Add("照片文件名：" + CardMsg.PhotoFileName);
                    }
                    else
                    {
                        Syn_ClosePort(iPort);
                        MessageBox.Show("读二代证信息错误!"); return "";
                    }
                }
                else
                {
                    Syn_ClosePort(iPort);
                    MessageBox.Show("打开端口错误"); return "";
                }
            }
            catch (Exception e)
            {
                Syn_ClosePort(iPort);
                MessageBox.Show("读身份证号异常："+e.Message); return "";
            }

        }

        //读卡
        private static string ReadCardNo()
        {
            string strCardNo = "";
            try
            {

                int nRet, nPort;
              
                byte KeyType = 0; // 秘钥A验证
                byte[] pkey = new byte[6];
                byte[] data = new byte[8];


                byte[] pucIIN = new byte[4];
                iPort = Syn_FindUSBReader();
                if (Syn_OpenPort(iPort) != 0)
                {
                    MessageBox.Show("读卡器连接失败"); return "";
                }
                Syn_ClosePort(iPort);

                if (Syn_SelectIDCard(iPort, ref  pucIIN[0], 0) != 0)
                {

                }

                #region 取ini配置
                byte BlackNo = 1;
                byte DataNo = 0;
                int khcd = 11;

                int dataLength = 11;
                //默认不需要
                string strToHex = "false";
                //默认不加
                string khwsjl = "false";
                //默认不验证
                string strIsCheck = "false";
                //默认11
                byte Check_SID = 11;
                //默认0
                byte Check_BID = 0;
                //默认加密
             
                #endregion

                pkey[0] = 255; pkey[1] = 255; pkey[2] = 255; pkey[3] = 255; pkey[4] = 255; pkey[5] = 255;
                nPort = iPort;

                if (Syn_OpenPort(nPort) == 0)
                {
                    nRet = Syn_USBHIDM1AuthenKey(nPort, KeyType, BlackNo, ref pkey[0]);
                    if (nRet == 0)
                    {
                        #region 是否需要检验
                        if (strIsCheck.Trim().ToLower() == "true" || strIsCheck.Trim().ToLower() == "是")
                        {
                            bool bCheckSucess = false;//检验是否通过
                            int ret_check = Syn_USBHIDM1AuthenKey(nPort, KeyType, Check_SID, ref pkey[0]);// 
                            if (ret_check == 0)
                            {
                                byte[] check_data = new byte[8];
                                ret_check = Syn_USBHIDM1ReadBlock(nPort, (byte)(Check_SID * 4 + Check_BID), ref  check_data[0]);
                                if (ret_check == 0)
                                {
                                    string check = new System.Text.ASCIIEncoding().GetString(check_data).ToString();
                                    //if (check.Length > strCheck.Length) check = check.Substring(0, strCheck.Length);//截断长度
                                    //if (check.Substring(0, strCheck.Length) == strCheck)
                                    //{
                                    //    bCheckSucess = true;

                                    //}
                                }
                            }
                            if (bCheckSucess == false)
                            {
                                MessageBox.Show("校验错误");
                                return strCardNo;
                            }

                        }
                        #endregion
                        data = new byte[dataLength];
                        nRet = Syn_USBHIDM1ReadBlock(nPort, (byte)(BlackNo * 4 + DataNo), ref data[0]); ;  //计算块号
                        if (nRet == 0)
                        {
                            string _kh = new System.Text.ASCIIEncoding().GetString(data).ToString(); //System.Text.ASCIIEncoding.ASCII.GetString(pblock);
                            //是否需要转16进制
                            if (strToHex.Trim().ToLower() == "true" || strToHex.Trim().ToLower() == "是") _kh = byteToHexStr(data);
                            //是否是加密卡，是就需要解密
                            //if(strIsCrry.Trim().ToLower()=="true" || strToHex.Trim().ToLower()=="是")  _kh=TrasenClasses.GeneralClasses.Crypto.Instance().UnCryp(_kh);
                            _kh = _kh.Trim().TrimEnd('\0');
                            if (_kh.Length > khcd) _kh = _kh.Substring(0, khcd); //取需要的长度
                            //长度不足【卡号长度】需要末尾补零
                            if (khwsjl.Trim().ToLower() == "true" || khwsjl.Trim().ToLower() == "是") _kh = _kh.PadLeft(khcd, '0');//补零
                            strCardNo = _kh;
                        }
                        else
                        {
                            MessageBox.Show("读取数据失败");
                        }
                    }
                    else
                    {
                        MessageBox.Show("读诊疗卡失败");
                    }
                }
                else
                {
                    MessageBox.Show("打开端口失败");
                }
                Syn_ClosePort(nPort);
            }
            catch (System.Exception err)
            {
                MessageBox.Show("读卡异常：" + err.Message);
            }
            return strCardNo;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct IDCardData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name; //姓名   
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string Sex;   //性别
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string Nation; //名族
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string Born; //出生日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 72)]
            public string Address; //住址
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string IDCardNo; //身份证号
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string GrantDept; //发证机关
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeBegin; // 有效开始日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeEnd;  // 有效截止日期
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string reserved; // 保留
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public string PhotoFileName; // 照片路径
        }
        /************************端口类API *************************/

        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_OpenPort", CharSet = CharSet.Ansi)]
        public static extern int Syn_OpenPort(int iPortID);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ClosePort", CharSet = CharSet.Ansi)]
        public static extern int Syn_ClosePort(int iPortID);

        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindUSBReader", CharSet = CharSet.None)]
        private static extern int Syn_FindUSBReader();
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindReader", CharSet = CharSet.None)]
        private static extern int Syn_FindReader();

        /************************ SAM类API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMStatus", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMStatus(int iPortID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ResetSAM", CharSet = CharSet.Ansi)]
        public static extern int Syn_ResetSAM(int iPortID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMID", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMID(int iPortID, ref byte pucSAMID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMIDToStr", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMIDToStr(int iPortID, ref byte pcSAMID, int iIfOpen);

        /********************身份证卡类API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_StartFindIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_StartFindIDCard(int iPortID, ref byte pucManaInfo, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SelectIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_SelectIDCard(int iPortID, ref byte pucManaMsg, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadMsg", CharSet = CharSet.Auto)]
        public static extern int Syn_ReadMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData);

        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDGetSecCardID", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDGetSecCardID(int iPortID, ref byte pucManaMsg);
        /********************附加类API *****************************/
        //[DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SendSound", CharSet = CharSet.Ansi)]
        //public static extern int Syn_SendSound(int iCmdNo);
        //[DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_DelPhotoFile", CharSet = CharSet.Ansi)]
        //public static extern void Syn_DelPhotoFile();
        /***********************M1卡操作函数 (A16D-HF) ********************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1Reset", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1Reset(int iPort, ref uint pdwCardSN, ref byte pbSize);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1AuthenKey", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1AuthenKey(int iPort, byte KeyType, byte BlockNo, ref byte pKey);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1ReadBlock", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1ReadBlock(int iPort, byte BlockNo, ref byte pBlock);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1WriteBlock", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1WriteBlock(int iPort, byte BlockNo, ref byte pBlock);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1Halt", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1Halt(int iPort);
        static int iPort;
    }
}
