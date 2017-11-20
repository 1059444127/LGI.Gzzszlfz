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
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    class NBYZRMYY
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
                    int index = 0;
                     try
                     { xd1.LoadXml(sSampleInfo);
                        XmlNodeList xnl = xd1.SelectNodes("/SAMPLES/SAMPLE");
                       
                         index = xnl.Count;
                         
                         for (int x = 0; x < xnl.Count; x++)
                           {
                               XmlNode xmlok1 = xnl[x];
                               bbmc = bbmc.Trim() + "  " + xmlok1["COLLECTBODY"].InnerText;
                             if (bbbm == "")
                             bbbm = xmlok1["SAMPLENO"].InnerText;
                         else
                             bbbm = bbbm.Trim() + "," + xmlok1["SAMPLENO"].InnerText;
                             }
                     }
                     catch { if (Debug == "1") MessageBox.Show("标本XML解析错误"); return "0"; }
                     if (Mes != "")
                     { 
                         MessageBox.Show(Mes); return "0"; }
                    
                     if (index > 1)
                     { 
                     
                      string bbbm_1 = w.ReadString("pathgethis", "bbmh", "");
                     string bbsl_1 = w.ReadString("pathgethis", "bbsl", "");
                     string sqxh_1 = w.ReadString("pathgethis", "sqxh", "");
                         
                     string newsqxh = xmlok["HIS_KEYNO"].InnerText;
                     //string ysbbbm_1 = f.ReadString("pathgethis", "ysbbmh", "");
                     //string ysbbsl = f.ReadString("pathgethis", "ysbbsl", "");
                     if (sqxh_1 != newsqxh && sqxh_1 != bbsl_1)
                     {
                         if (MessageBox.Show("此病人共有" + index + "个标本\n标本还未扫完，是否确认扫描新病人", "是否继续", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                       {
                           
                           w.WriteString("pathgethis", "bbsl", "");
                           w.WriteString("pathgethis", "bbmh", "");
                           w.WriteString("pathgethis", "ysbbmh", "");
                           w.WriteString("pathgethis", "ysbbsl", "");
                           w.WriteString("pathgethis", "sqxh", "");
                           bbbm_1 = w.ReadString("pathgethis", "bbmh", "");
                           bbsl_1 = w.ReadString("pathgethis", "bbsl", "");
                           sqxh_1 = w.ReadString("pathgethis", "sqxh", "");

                          

                           w.WriteString("pathgethis", "bbsl", "");
                           w.WriteString("pathgethis", "bbmh", "");
                           w.WriteString("pathgethis", "ysbbmh", "");
                           w.WriteString("pathgethis", "ysbbsl", "");
                           w.WriteString("pathgethis", "sqxh", "");

                       

                       }
                       else

                           return xmlstr();
                     }
               
                     if (bbbm_1 == "" && bbsl_1 == "" && sqxh_1 == "")
                     {
                        
                     //  string bbmhstr= w.ReadString("pathgethis", "bbmh", "");
                         w.WriteString("pathgethis", "bbsl", index.ToString());
                         w.WriteString("pathgethis", "bbmh", bbbm);
                         w.WriteString("pathgethis", "sqxh", newsqxh);
                         bbbm_1 = w.ReadString("pathgethis", "bbmh", "");
                         bbsl_1 = w.ReadString("pathgethis", "bbsl", "");
                         sqxh_1 = w.ReadString("pathgethis", "sqxh", "");
                     }
                         
                     if (bbbm_1 != "" && bbsl_1 != "" && sqxh_1 != "")
                     { //写已收标本编码和数量--------------------------------------------------------------
                         //string ysqxh = w.ReadString("pathgethis", "sqxh", "");
                    
                             string ysbbmc = w.ReadString("pathgethis", "ysbbmh", "");
                             if (ysbbmc == "")
                                 w.WriteString("pathgethis", "ysbbmh", Ssbz);
                             else
                             {
                                 if (ysbbmc.Contains(Ssbz))
                                 {
                                     if (msg == "1") MessageBox.Show("重复有条形码，请重新扫描，"); return xmlstr();
                                 }
                                 w.WriteString("pathgethis", "ysbbmh", ysbbmc + "," + Ssbz);
                             }
                             //写已收标本数量------------------------------------------------------------------------

                             string ysbbsl = w.ReadString("pathgethis", "ysbbsl", "");
                             if (ysbbsl == "")
                                 w.WriteString("pathgethis", "ysbbsl", "1");
                             else
                             {
                                 int y = int.Parse(ysbbsl) + 1;
                                 w.WriteString("pathgethis", "ysbbsl", y.ToString());
                             }
                         }

                    //-----------设置标本数量和编码----------------------------------- 
                             //如果申请序号不相同 清空pathgethis.ini并从写
                             //清除pathgethis。ini的数据
                       //---判断是否完成接受标本--------------------------------------------------
                    string bbbm_2 = w.ReadString("pathgethis","bbmh", "");
                     string bbsl_2 = w.ReadString("pathgethis","bbsl", "");
                     string ysbbbm_2 = w.ReadString("pathgethis","ysbbmh", "");
                     string ysbbsl_2 = w.ReadString("pathgethis","ysbbsl", "");
               

                     if (bbsl_2 != ysbbsl_2)
                    {
                         int y=int.Parse(bbsl_2)-int.Parse(ysbbsl_2);
                       if(msg=="1")
                         MessageBox.Show("此病人共有" + index + "个标本\n标本还未扫描完成，请继续...\n还有" + y.ToString() + "个标本未扫描");

                         return xmlstr();
                       }
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
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString()+"岁";
                            if(nl=="0岁")
                                nl = (DateTime.Now.DayOfYear - DateTime.Parse(nl1).DayOfYear).ToString() + "天";
                        }
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
                        xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        try
                        {
                            xml = xml + "备用2=" + (char)34 + xmlok["PBIRTHDAY"].InnerText + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "备用2=" + (char)34 +"" + (char)34 + " ";
                        }
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
                            bbmc = bbmc.Trim() + "  " + xmlok1["COLLECTBODY"].InnerText;
                            if (bbbm == "")
                                bbbm = xmlok1["SAMPLENO"].InnerText;
                            else
                                bbbm = bbbm.Trim() + "," + xmlok1["SAMPLENO"].InnerText;
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
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "岁";
                        if (nl == "0岁")
                            nl = (DateTime.Now.DayOfYear - DateTime.Parse(nl1).DayOfYear).ToString() + "天";
                        }
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
                        xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        try
                        {
                            xml = xml + "备用2=" + (char)34 + xmlok["PBIRTHDAY"].InnerText + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        }
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
                        inxml = inxml + "<SQDBH>" + decimal.Parse(Ssbz.ToString().Trim()) + "</SQDBH>";
                        inxml = inxml + "<ZT>" + "98" + "</ZT>";
                        inxml = inxml + "<JSRY>" + "" + "</JSRY>";
                        inxml = inxml + "<JSSJ></JSSJ>";
                        inxml = inxml + "<BGRY></BGRY>";
                        inxml = inxml + "<BGSJ></BGSJ>";
                        inxml = inxml + "<SHRY></SHRY>";
                        inxml = inxml + "<SHSJ></SHSJ>";
                        inxml = inxml + "<CXRY>" + "" + "</CXRY>";
                        inxml = inxml + "<CXSJ>" + "" + "</CXSJ>";
                        inxml = inxml + "<JCSJ></JCSJ>";
                        inxml = inxml + "<JCJL></JCJL>";
                        inxml = inxml + "<WEBURL></WEBURL>";
                        inxml = inxml + "<JCH></JCH>";
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
                        outxml = yzyyweb.SetPISReportInfo(decimal.Parse(Ssbz) , inxml);
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
                //--------------- 体检---------
               
                if (Sslbx == "体检号")
                {
                  
                    DataSet ds = new DataSet();
                    SqlConnection sqlcon=null;
                    string odbcstr = f.ReadString("体检号", "odbcsql", ""); //获取sz.ini中设置的odbcsql
                    string sqlstr = "select * from dbo.V_TJ_TO_BL where brbh='" + Ssbz.Trim() + "'";
                    try
                    {
                         sqlcon = new SqlConnection(odbcstr);
                        SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, sqlcon);
                        sqlcon.Open();
                        
                        sqlda.Fill(ds, "table");
                        sqlda.Dispose();
                       
                    }
                    catch
                    {
                        MessageBox.Show("体检系统数据库连接错误！！！");
                        return "0";
                    }
                    finally
                    {
                        sqlcon.Close();
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("此体检号无记录！");
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        //

                        LGHISJKZGQ.Frm_nbyzrmyytj tj = new LGHISJKZGQ.Frm_nbyzrmyytj(ds);
                        tj.ShowDialog();
                        //获取体检项目编号
                        string tjxmbh = f.ReadString("体检号", "odbcsql", "");

                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {

                            if (ds.Tables[0].Rows[x]["xmbh"].ToString() == w.ReadString("tijian", "xmbh", ""))
                            {
                                //
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[x]["brbh"].ToString() + (char)34 + " ";
                                xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[x]["xmbh"].ToString() + (char)34 + " ";
                                xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[x]["mzhm"].ToString() + (char)34 + " ";
                                xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[x]["zyhm"].ToString() + (char)34 + " ";
                                xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[x]["brxm"].ToString() + (char)34 + " ";

                                xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[x]["brxb"].ToString() + (char)34 + " ";
                                string nl1 = ds.Tables[0].Rows[x]["csny"].ToString();
                                string nl = "";
                                try
                                { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "岁";
                                if (nl == "0岁")
                                    nl = (DateTime.Now.DayOfYear - DateTime.Parse(nl1).DayOfYear).ToString() + "天";
                                }
                                catch
                                { nl = ""; }
                                xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";

                                xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[x]["hyzk"].ToString() + (char)34 + " ";
                                xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[x]["addr"].ToString() + (char)34 + "   ";
                                xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[x]["sjhm"].ToString() + (char)34 + " ";
                                xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                                //xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                                // xml = xml + "床号=" + (char)34 +""+ (char)34 + " ";

                                xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[x]["sfzh"].ToString() + (char)34 + " ";
                                xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                                xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[x]["srks"].ToString() + (char)34 + " ";
                                xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[x]["ygxm"].ToString() + (char)34 + " ";
                                //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                                //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

                                //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                                //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[x]["xmmc"].ToString() + (char)34 + " ";
                                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                                xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";

                                xml = xml + "病人类别=" + (char)34 + ds.Tables[0].Rows[x]["brlb"].ToString() + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                xml = xml + "</LOGENE>";

                                return xml;
                            }
                        }
                        
                    }
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["brbh"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["xmbh"].ToString()+ (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["mzhm"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[0]["zyhm"].ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["brxm"].ToString()+ (char)34 + " ";

                        xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[0]["brxb"].ToString() + (char)34 + " ";
                        string nl1 = ds.Tables[0].Rows[0]["csny"].ToString();
                        string nl = "0岁";
                        try
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "岁";
                        if (nl == "0岁")
                            nl = (DateTime.Now.DayOfYear - DateTime.Parse(nl1).DayOfYear).ToString() + "天";
                        
                        }
                        catch
                        { nl = "0岁"; }
                        xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[0]["hyzk"].ToString() + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[0]["addr"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["sjhm"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 +"" + (char)34 + " ";
                        //xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        // xml = xml + "床号=" + (char)34 +""+ (char)34 + " ";

                        xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[0]["sfzh"].ToString() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["srks"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["ygxm"].ToString() + (char)34 + " ";
                        //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                        //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

                        //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                        //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[0]["xmmc"].ToString() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "病人类别=" + (char)34 + ds.Tables[0].Rows[0]["brlb"].ToString() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    } return "0";
                    
                }

                //---------
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

