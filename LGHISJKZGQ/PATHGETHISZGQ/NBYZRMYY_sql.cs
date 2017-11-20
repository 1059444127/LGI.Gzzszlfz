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

namespace LGHISJKZGQ
{
    class NBYZRMYY_sql
    {
        //宁波鄞州人民医院
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static IniFiles w = new IniFiles(Application.StartupPath + "\\pathgethis.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
        
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            
            string pathWEB = f.ReadString("条形码", "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
            string msg = w.ReadString("pathgethis", "msg", "");
            if (Sslbx != "")
            {

                if (Sslbx == "条形码")
                {
                    
                    LGHISJKZGQ.nbyzrmyyWeb.PisServiceLJ yzyy = new LGHISJKZGQ.nbyzrmyyWeb.PisServiceLJ();
                    if (pathWEB != "") yzyy.Url = pathWEB;
                    string sBillInfo = "";
                    string sSampleInfo = "";
                    string Mes = "";
                    try { 
                        Mes = yzyy.PatBillWritePIS(Ssbz, ref sBillInfo, ref sSampleInfo, 1);
                        //log.WriteMyLog(sBillInfo + "&" + sSampleInfo);
                        }
                    catch (Exception ee) 
                    {
                        MessageBox.Show("此标本条形码不存在，请核对！");
                        if (Debug == "1")
                        MessageBox.Show(ee.ToString());
                          log.WriteMyLog("参数：" + Ssbz + "返回值：" + sBillInfo + "提示信息：" + Mes);
                        
                        return "0";
                    }

                    if (Mes != "")
                    {
                        MessageBox.Show("HIS："+Mes); return "0";
                    }
                    //-------------------------------

                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(sBillInfo);
                        xmlok = xd.SelectSingleNode("/BILL");
                     }
                    catch { if (Debug == "1") MessageBox.Show("XML解析错误"); return "0"; }
                        //标本名称------------------------------------------------------------
                   
                     XmlDocument xd1 = new XmlDocument(); 
                    string bbmc = "";
                    string bbbm = "";
                    string bbsl="0";
                    string sqxh="";
                    int index = 0;
                    dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
                     try
                     { xd1.LoadXml(sSampleInfo);
                        XmlNodeList xnl = xd1.SelectNodes("/SAMPLES/SAMPLE");
                       
                         index = xnl.Count;
                         int ii;
                      
                              
                               sqxh =xmlok["HIS_KEYNO"].InnerText;
                               bbmc =xmlok["COLLECTBODY"].InnerText;
                               bbbm =Ssbz;
                               bbsl=index.ToString();
                            
                          ii= aa.ExecuteSQL("insert into t_txm (sqxh,bbbm,sl,sdrq) values('"+sqxh+"','"+bbbm+"','"+bbsl+"','"+DateTime.Now+"')");
                             
                     }
                     catch { if (Debug == "1") MessageBox.Show("标本XML解析错误"); return "0"; }

                     DataTable  dt=  aa.GetDataTable("select * from t_txm where sqxh='"+sqxh+"'","tablename");
                    int x=0; 
                    if (dt!=null)
                          x=dt.Rows.Count;
                     if (index > x)
                     {
                         MessageBox.Show("标本还未扫描完，请继续。。。");
                         return "" ;
                          
                     }
                     
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + xmlok["MEDRECNO"].InnerText + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + xmlok["HIS_KEYNO"].InnerText + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + xmlok["OUTPATIENTNO"].InnerText + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + xmlok["INHOSPITALNO"].InnerText + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + xmlok["PNAME"].InnerText + (char)34 + " ";
                        string PSEX = xmlok["PSEX"].InnerText;
                        if (PSEX == "F") PSEX = "女";
                        else
                        {
                            if (PSEX == "M") PSEX = "男"; else PSEX = " ";
                        }
                        xml = xml + "性别=" + (char)34 + PSEX + (char)34 + " ";
                        string nl1 = xmlok["PBIRTHDAY"].InnerText;
                        string nl = "0岁";
                        try
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "岁"; }
                        catch
                        { nl = "0岁"; }
                        xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                        string MARRIED = xmlok["MARRIED"].InnerText;
                        switch (MARRIED)
                        {
                            case "Y": MARRIED = "已婚"; break;
                            case "W": MARRIED = "未婚"; break;
                            case "L": MARRIED = "离异"; break;
                            case "S": MARRIED = "丧偶"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "婚姻=" + (char)34 + MARRIED + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + xmlok["ADDRESS"].InnerText + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + xmlok["PTELEPHONENO"].InnerText + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + xmlok["WARD"].InnerText + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + xmlok["BEDNO"].InnerText + (char)34 + " ";
                        //xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        // xml = xml + "床号=" + (char)34 +""+ (char)34 + " ";

                        xml = xml + "身份证号=" + (char)34 + xmlok["IDCARD"].InnerText + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + xmlok["VOCATION"].InnerText + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + xmlok["SUBMITDEPT"].InnerText + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + xmlok["SUBMITDOC"].InnerText + (char)34 + " ";
                        //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                        //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

                        //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                        //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + bbmc + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + xmlok["MEDICALTYPE"].InnerText + (char)34 + " ";
                        string PATIENTTYPE = xmlok["PATIENTTYPE"].InnerText;
                        if (PATIENTTYPE == "1000") PATIENTTYPE = "门诊";
                        if (PATIENTTYPE == "2000") PATIENTTYPE = "急诊";
                        if (PATIENTTYPE == "3000") PATIENTTYPE = "住院";
                        if (PATIENTTYPE == "4000") PATIENTTYPE = "体检";
                        xml = xml + "病人类别=" + (char)34 + PATIENTTYPE + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + xmlok["HISCONSULTATION"].InnerText + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + xmlok["DISEASE"].InnerText + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        w.WriteString("pathgethis", "bbsl", "");
                        w.WriteString("pathgethis", "bbmh", "");
                        w.WriteString("pathgethis", "ysbbmh", "");
                        w.WriteString("pathgethis", "ysbbsl", "");
                        w.WriteString("pathgethis", "sqxh", "");
                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        return "0";
                    }
                }
                //----------------------------------------------------------------------------
                if (Sslbx == "申请单号")
                {

                    LGHISJKZGQ.nbyzrmyyWeb.PisServiceLJ yzyy = new LGHISJKZGQ.nbyzrmyyWeb.PisServiceLJ();
                    if (pathWEB != "") yzyy.Url = pathWEB;
                    string sBillInfo = "";
                    string sSampleInfo = "";
                    string Mes = "";
                    try
                    {
                        Mes = yzyy.PatBillWritePIS(Ssbz, ref sBillInfo, ref sSampleInfo, 0);
                        //log.WriteMyLog(sBillInfo + "&" + sSampleInfo);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("此申请单号不存在，请核对！");
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        log.WriteMyLog("参数：" + Ssbz + "返回值：" + sBillInfo + "提示信息：" + Mes);

                        return "0";
                    }
                    //--------------------------------
                    
                   
                

                    //-------------------------------

                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(sBillInfo);
                        xmlok = xd.SelectSingleNode("/BILL");
                    }
                    catch { if (Debug == "1") MessageBox.Show("XML解析错误"); return "0"; }
                    //标本名称------------------------------------------------------------

                    XmlDocument xd1 = new XmlDocument();
                    string bbmc = "";
                    string bbbm = "";
                    int index = 0;
                    try
                    {
                        xd1.LoadXml(sSampleInfo);
                        XmlNodeList xnl = xd1.SelectNodes("/SAMPLES/SAMPLE");

                        index = xnl.Count;
                        if (index>1)
                        MessageBox.Show("此病人共有" + index + "个标本!");
                        for (int x = 0; x < xnl.Count; x++)
                        {
                            XmlNode xmlok1 = xnl[x];
                            bbmc = bbmc + "  " + xmlok1["COLLECTBODY"].InnerText;
                            if (bbbm == "")
                                bbbm = xmlok1["SAMPLENO"].InnerText;
                            else
                                bbbm = bbbm + "," + xmlok1["SAMPLENO"].InnerText;
                        }
                    }
                    catch { if (Debug == "1") MessageBox.Show("标本XML解析错误"); return "0"; }
                    if (Mes != "")
                    {
                        MessageBox.Show(Mes); return "0";
                    }

                    
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + xmlok["MEDRECNO"].InnerText + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + xmlok["HIS_KEYNO"].InnerText + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + xmlok["OUTPATIENTNO"].InnerText + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + xmlok["INHOSPITALNO"].InnerText + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + xmlok["PNAME"].InnerText + (char)34 + " ";
                        string PSEX = xmlok["PSEX"].InnerText;
                        if (PSEX == "F") PSEX = "女";
                        else
                        {
                            if (PSEX == "M") PSEX = "男"; else PSEX = " ";
                        }
                        xml = xml + "性别=" + (char)34 + PSEX + (char)34 + " ";
                        string nl1 = xmlok["PBIRTHDAY"].InnerText;
                        string nl = "0岁";
                        try
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "岁"; }
                        catch
                        { nl = "0岁"; }
                        xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                        string MARRIED = xmlok["MARRIED"].InnerText;
                        switch (MARRIED)
                        {
                            case "Y": MARRIED = "已婚"; break;
                            case "W": MARRIED = "未婚"; break;
                            case "L": MARRIED = "离异"; break;
                            case "S": MARRIED = "丧偶"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "婚姻=" + (char)34 + MARRIED + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + xmlok["ADDRESS"].InnerText + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + xmlok["PTELEPHONENO"].InnerText + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + xmlok["WARD"].InnerText + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + xmlok["BEDNO"].InnerText + (char)34 + " ";
                        //xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        // xml = xml + "床号=" + (char)34 +""+ (char)34 + " ";

                        xml = xml + "身份证号=" + (char)34 + xmlok["IDCARD"].InnerText + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + xmlok["VOCATION"].InnerText + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + xmlok["SUBMITDEPT"].InnerText + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + xmlok["SUBMITDOC"].InnerText + (char)34 + " ";
                        //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                        //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

                        //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                        //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + bbmc + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + xmlok["MEDICALTYPE"].InnerText + (char)34 + " ";
                        string PATIENTTYPE = xmlok["PATIENTTYPE"].InnerText;
                        if (PATIENTTYPE == "1000") PATIENTTYPE = "门诊";
                        if (PATIENTTYPE == "2000") PATIENTTYPE = "急诊";
                        if (PATIENTTYPE == "3000") PATIENTTYPE = "住院";
                        if (PATIENTTYPE == "4000") PATIENTTYPE = "体检";
                        xml = xml + "病人类别=" + (char)34 + PATIENTTYPE + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + xmlok["HISCONSULTATION"].InnerText + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + xmlok["DISEASE"].InnerText + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        return "0";
                    }
                }
                if (Sslbx == "撤销申请单")
                {
                   
                    string inxml = "";
                    inxml = inxml + "<?xml version='1.0' encoding='GB2312'?>";
                    inxml = inxml + "<REPORTINFO>";
                    inxml = inxml + "<ITEM>";
                    inxml = inxml + "<SQDBH>" + Ssbz + "</SQDBH>";
                    inxml = inxml + "<ZT>" + "99" + "</ZT>";
                    inxml = inxml + "<JSRY>" + "" + "</JSRY>";
                    inxml = inxml + "<JSSJ>" + "" + "</JSSJ>";
                    inxml = inxml + "<BGRY>" + "" + "</BGRY>";
                    inxml = inxml + "<BGSJ>" + "" + "</BGSJ>";
                    inxml = inxml + "<SHRY>" + "" + "</SHRY>";
                    inxml = inxml + "<SHSJ>" + "" + "</SHSJ>";
                    inxml = inxml + "<CXRY>" + "" + "</CXRY>";
                    inxml = inxml + "<CXSJ>" + DateTime.Now.ToString("yyyyMMddHHMMss") + "</CXSJ>";
                    inxml = inxml + "<JCSJ>" + "" + "</JCSJ>";
                    inxml = inxml + "<JCJL>" + "" + "</JCJL>";
                    inxml = inxml + "<WEBURL>" + "" + "</WEBURL>";
                    inxml = inxml + "</ITEM>";
                    inxml = inxml + "</REPORTINFO>";

                    if (Debug == "1")
                    {
                        log.WriteMyLog("回传的xml数据：" + inxml);
                    }
                    string outxml = "";
                    try
                    {
                        LGHISJKZGQ.nbyzrmyyWeb.PisServiceLJ yzyyweb = new LGHISJKZGQ.nbyzrmyyWeb.PisServiceLJ();
                        if (pathWEB != "") yzyyweb.Url = pathWEB;
                        outxml = yzyyweb.SetPISReportInfo(decimal.Parse(Ssbz), inxml);
                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show("申请单撤销失败！请确认申请单号是否存在！");
                        log.WriteMyLog("申请单撤销失败！调用HIS接口出错：" + ee.ToString());

                        return "0";
                    }

                    if (outxml == "")
                    {

                        MessageBox.Show("申请单撤销成功！");
                    }
                    else
                    {
                        log.WriteMyLog("申请单撤销失败！原因：" + outxml);
                        if (Debug == "1")
                            MessageBox.Show("申请单撤销失败！" + outxml);
                    }
                    return "0";
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
        public  static string xmlstr()
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
            xml = xml + "送检医院=" + (char)34 + (char)34 + " ";
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
      }
}

