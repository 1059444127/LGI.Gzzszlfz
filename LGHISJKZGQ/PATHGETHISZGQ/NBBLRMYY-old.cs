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
    public class NBBLRMYY
    {
        //宁波北仑人民医院
        //webservices
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            
            if (Sslbx != "")
            {
                
                if (Sslbx == "申请单号")
                {
                   
                    PATHGETHISZGQ.BLWebReference.PisServiceLJ blyy = new PATHGETHISZGQ.BLWebReference.PisServiceLJ();
                    string sBillInfo = "";
                    string sSampleInfo = "";
                    string Mes="";
                 try {
                    
                     Mes = blyy.PatBillWritePIS(Ssbz.Trim(), ref sBillInfo, ref sSampleInfo,0);
                    
                     }
                 catch (Exception ee) { MessageBox.Show(ee.ToString()); return "0"; }
                   
                 if (Debug == "1")
                 { MessageBox.Show("mes"+Mes); 
                     MessageBox.Show("sBillInfo"+sBillInfo); 
                     MessageBox.Show("sSampleInfo"+sSampleInfo);
                 }
             
                     if (Mes != "") {

                         MessageBox.Show(Mes); return "0";
                     }


                  
                        XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();
                       
                       try
                       {
                          
                           xd.LoadXml(sBillInfo);
                           xmlok = xd.SelectSingleNode("/BILL");
                          
                        }
                          catch { if (Debug == "1") MessageBox.Show("XML解析错误"); return "0"; }

                    //------------------- 收费金额-------------------------
                    //    int jine = 0;//金额
                    //string  sBillInfo1="";
                    //string sSampleInfo1="";
                    //string schargeInfo1="" ;//收费信息
                    //string mesaage = blyy.PatBillChargeWritePIS(Ssbz.Trim(), ref  sBillInfo1, ref  sSampleInfo1, ref  schargeInfo1, 0);
                    //log.WriteMyLog(schargeInfo1);
                    //XmlNode xmlok = null;
                    //XmlDocument xd = new XmlDocument();

                    //try
                    //{
                    //    xd.LoadXml(sBillInfo);
                    //    xmlok = xd.SelectSingleNode("/BILL");
                    //}
                    //catch {
                    //    if (Debug == "1")log.WriteMyLog("XML解析错误");
                    //    throw;
                    //}

                    //-------------------------------------------------------------------------------------
                   
                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + xmlok["MEDRECNO"].InnerText + (char)34 + " "; 
                            xml = xml + "就诊ID=" +(char)34+""+(char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + xmlok["HIS_KEYNO"].InnerText + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + xmlok["OUTPATIENTNO"].InnerText + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + xmlok["INHOSPITALNO"].InnerText + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + xmlok["PNAME"].InnerText + (char)34 + " ";
                            string PSEX = xmlok["PSEX"].InnerText;
                            if (PSEX == "F") PSEX = "女";
                            else
                            {
                                if (PSEX == "M") PSEX = "男";
                                else PSEX = "";
                            }
                            xml = xml + "性别=" + (char)34 + PSEX + (char)34 + " ";
                            string nl1 = xmlok["PBIRTHDAY"].InnerText;
                            string nl = "0岁";
                            try
                            { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString()+"岁";}
                            catch
                            {nl = "0岁";}
                            xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                            string MARRIED = xmlok["MARRIED"].InnerText;
                            switch(MARRIED)
                            {
                                case "Y": MARRIED = "已婚"; break;
                                case "W": MARRIED = "未婚"; break;
                                case "L": MARRIED = "离异"; break;
                                case "S": MARRIED = "丧偶"; break;
                                default: MARRIED = ""; break;
                                
                            }
                          
                            xml = xml + "婚姻=" + (char)34 + MARRIED + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + xmlok["ADDRESS"].InnerText  + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + xmlok["PTELEPHONENO"].InnerText + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + xmlok["WARD"].InnerText + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + xmlok["BEDNO"].InnerText + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + xmlok["IDCARD"].InnerText + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " "+ (char)34 + " "; 
                            xml = xml + "职业=" + (char)34 + xmlok["VOCATION"].InnerText + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + xmlok["SUBMITDEPT"].InnerText + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + xmlok["SUBMITDOC"].InnerText + (char)34 + " "; 
                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34  + (char)34 + " ";
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




                    if (Sslbx == "条码号")
                    {

                        PATHGETHISZGQ.BLWebReference.PisServiceLJ blyy = new PATHGETHISZGQ.BLWebReference.PisServiceLJ();
                        string sBillInfo = "";
                        string sSampleInfo = "";
                        string Mes = "";
                        try
                        {

                            Mes = blyy.PatBillWritePIS(Ssbz.Trim(), ref sBillInfo, ref sSampleInfo, 1);

                        }
                        catch (Exception ee) { MessageBox.Show(ee.ToString()); return "0"; }

                        if (Debug == "1")
                        {
                            MessageBox.Show("mes" + Mes);
                            MessageBox.Show("sBillInfo" + sBillInfo);
                            MessageBox.Show("sSampleInfo" + sSampleInfo);
                        }

                        if (Mes != "")
                        {

                            MessageBox.Show(Mes); return "0";
                        }



                        XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        try
                        {

                            xd.LoadXml(sBillInfo);
                            xmlok = xd.SelectSingleNode("/BILL");

                        }
                        catch { if (Debug == "1") MessageBox.Show("XML解析错误"); return "0"; }

                        //------------------- 收费金额-------------------------
                        //    int jine = 0;//金额
                        //string  sBillInfo1="";
                        //string sSampleInfo1="";
                        //string schargeInfo1="" ;//收费信息
                        //string mesaage = blyy.PatBillChargeWritePIS(Ssbz.Trim(), ref  sBillInfo1, ref  sSampleInfo1, ref  schargeInfo1, 0);
                        //log.WriteMyLog(schargeInfo1);
                        //XmlNode xmlok = null;
                        //XmlDocument xd = new XmlDocument();

                        //try
                        //{
                        //    xd.LoadXml(sBillInfo);
                        //    xmlok = xd.SelectSingleNode("/BILL");
                        //}
                        //catch {
                        //    if (Debug == "1")log.WriteMyLog("XML解析错误");
                        //    throw;
                        //}

                        //-------------------------------------------------------------------------------------

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
                                if (PSEX == "M") PSEX = "男";
                                else PSEX = "";
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
                            xml = xml + "身份证号=" + (char)34 + xmlok["IDCARD"].InnerText + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + xmlok["VOCATION"].InnerText + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + xmlok["SUBMITDEPT"].InnerText + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + xmlok["SUBMITDOC"].InnerText + (char)34 + " ";
                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
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
                        inxml = inxml + "<ZT>" + "99"+ "</ZT>";
                        inxml = inxml + "<JSRY>" + "" + "</JSRY>";
                        inxml = inxml + "<JSSJ>" + "" + "</JSSJ>";
                        inxml = inxml + "<BGRY>" + "" + "</BGRY>";
                        inxml = inxml + "<BGSJ>" + "" + "</BGSJ>";
                        inxml = inxml + "<SHRY>" + "" + "</SHRY>";
                        inxml = inxml + "<SHSJ>" + "" + "</SHSJ>";
                        inxml = inxml + "<CXRY>" + "" + "</CXRY>";
                        inxml = inxml + "<CXSJ>" + DateTime.Now.ToString("yyyyMMddHHMMss")+"</CXSJ>";
                        inxml = inxml + "<JCSJ>" + "" + "</JCSJ>";
                        inxml = inxml + "<JCJL>" + "" + "</JCJL>";
                        inxml = inxml + "<WEBURL>" + "" + "</WEBURL>";
                        inxml = inxml + "</ITEM>";
                        inxml = inxml + "</REPORTINFO>";

                        if (Debug == "1")
                        {
                            log.WriteMyLog("回传的xml数据：" + inxml);
                            MessageBox.Show(inxml);
                        }
                        string outxml = "";
                        try
                        {
                            PATHGETHISZGQ.BLWebReference.PisServiceLJ nbblweb = new PATHGETHISZGQ.BLWebReference.PisServiceLJ();
                           
                            outxml = nbblweb.SetPISReportInfo(decimal.Parse(Ssbz), inxml);
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

        //public static string RunProc_ds(string Ssbz, string Debug)
        //{
          
        //    PATHGETHISZGQ.BLWebReference.PisServiceLJ blyy = new PATHGETHISZGQ.BLWebReference.PisServiceLJ();
          
        //    //string sBillInfo ="<?xml version='1.0' encoding='GB2312'?>"+
        //    //   " <BILL> <PATIENTTYPE></PATIENTTYPE>" +     //就诊类别
        //    //    "<HIS_KEYNO></HIS_KEYNO>" +                //his申请单号
        //    //     "<INHOSPITALNO></INHOSPITALNO>"+          //住院号
        //    //    "<OUTPATIENTNO></OUTPATIENTNO>" +          //门诊号
        //    //    "<PNAME></PNAME>" +                        //姓名
        //    //    "<PSEX></PSEX>" +                          //性别
        //    //    "<PBIRTHDAY></PBIRTHDAY>" +                //出生年月
        //    //    "<SUBMITDEPT></SUBMITDEPT>" +              //送检科室
        //    //    "<SUBMITDOC></SUBMITDOC>" +                //送检医生
        //    //    "<PTELEPHONENO></PTELEPHONENO>" +          //患者电话
        //    //    "<MARRIED></MARRIED>" +                    //婚姻
        //    //    "<VOCATION></VOCATION>" +                  //职业
        //    //    "<IDCARD></IDCARD>" +                      //身份证号
        //    //    "<ADDRESS></ADDRESS>" +                    //地址
        //    //    "<MEDICALTYPE></MEDICALTYPE>" +            //医保类型
        //    //    "<WARD></WARD>" +                          //病区
        //    //    "<BEDNO></BEDNO>" +                        //床号h
        //    //    "<SUBMITDATE></SUBMITDATE>" +              //送检日期
        //    //    "<DISEASE></DISEASE>" +                    //临床诊断
        //    //    "<MEDRECNO></MEDRECNO>" +                  //病人编号
        //    //     "</BILL>";
        //    //string sSampleInfo = "<?xml version='1.0' encoding='GB2312'?>" +
        //    //    "<SAMPLES><SAMPLE>" +
        //    //    "<COLLECTBODY></COLLECTBODY>" +            //标本名称
        //    //     "</SAMPLE>" +
        //    //     "<SAMPLE></SAMPLE>" +
        //    //     "</SAMPLES>";
        //    string xmlStr = "";
        //    string sBillInfo = "";
        //    string sSampleInfo = "";
         
        //    try { xmlStr = blyy.PatBillWritePIS(Ssbz, ref sBillInfo, ref sSampleInfo,0); }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.ToString());
        //    }
           
        //    if (Debug=="1")
        //    MessageBox.Show(sBillInfo);
        //     String[] xmlstr = new String[2];
        //    name[0]=
        //    return sBillInfo;

        //}
    }
}
