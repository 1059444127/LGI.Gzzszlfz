using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Data;
using dbbase;
using System.Runtime.InteropServices;

namespace LGHISJKZGQ
{
   public class zsdxzlyyPT
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
          
            debug = f.ReadString(Sslbx, "debug", ""); //获取sz.ini中设置的mrks

            if (Sslbx != "")
            {
                if (Sslbx == "标本号")
                { dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                   DataTable  dt_sqdbbxx = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_BBH= '" + Ssbz.Trim() + "'", "bbxx");
                   if (dt_sqdbbxx == null)
                   { MessageBox.Show("查询标本号失败"); return "0"; }
                   else
                   {
                       if (dt_sqdbbxx.Rows.Count <= 0)
                       {  MessageBox.Show("未查询到此标本记录"); return "0";}
                       else
                           return GetSQDxx("申请单号", dt_sqdbbxx.Rows[0]["F_SQXH"].ToString().Trim(), debug);

                   }
                }
                if (Sslbx == "体检号")
                {
                    int len = Ssbz.Trim().Length;

                    Ssbz = "0000000000" + Ssbz;
                    Ssbz = Ssbz.Substring(Ssbz.Length - 10, 10);
                }

                if (Sslbx == "病历号")
                {
                    int len = Ssbz.Trim().Length;

                    Ssbz = "0000000000" + Ssbz;
                    Ssbz = Ssbz.Substring(Ssbz.Length - 10, 10);
                }

                if (Sslbx == "读诊疗卡" || Sslbx == "诊疗卡读卡" || Sslbx == "诊疗卡" || Sslbx == "病历号" || Sslbx == "病历号PT" || Sslbx == "申请单号" || Sslbx == "门诊号" || Sslbx == "住院号")
                {
                    string jzkh="";
                    if (Sslbx == "诊疗卡读卡" || Sslbx == "读诊疗卡")
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
                   string  rtnXml="0";
                
                     rtnXml=  GetSQDxx(Sslbx,Ssbz.Trim(),debug);
                 

                    if(rtnXml=="0")
                        return GetYZxx(Sslbx, Ssbz, debug);
                    else
                        return  rtnXml;

                    #region  his  ws  不用
                    //                    string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
//                    xml = xml + "<PRPA_IN201305UV02 xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/PRPA_IN201305UV02.xsd\">";
//                    ////<!-- UUID,交互实例唯一码-->
//                    xml = xml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
//                    xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
//                    xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN201305UV02\"/>";
//                    xml = xml + "<processingCode code=\"P\"/>";
//                    xml = xml + "<processingModeCode code=\"R\"/>";
//                    xml = xml + "<acceptAckCode code=\"AL\"/>";
//                    xml = xml + "<receiver typeCode=\"RCV\">";
//                    ////"<!-- 可以填写电话信息或者URL-->
//                    xml = xml + "<telecom value=\"\"/>";
//                    xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
//                    xml = xml + "<id root=\"2.999.1.97\" extension=\"HIS\"/>";
//                    xml = xml + "<telecom value=\"设备编号\"/>";
//                    xml = xml + "<softwareName code=\"HIS\" displayName=\"HIS\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
//                    xml = xml + "</device>";
//                    xml = xml + "</receiver>";

//                    xml = xml + "<sender typeCode=\"SND\">";
//                    xml = xml + "<telecom value=\"\"/>";
//                    xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
//                    xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
//                    xml = xml + "<telecom value=\"设备编号\"/>";
//                    xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
//                    xml = xml + "</device>";
//                    xml = xml + "</sender>";

//                    xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
//                    xml = xml + "<code code=\"PRPA_TE201305UV02\" codeSystem=\"2.16.840.1.113883.1.6\"/>";
//                    xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
//                    xml = xml + "<signatureText></signatureText>";
//                    xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
//                    xml = xml + "</authorOrPerformer>";
//                    xml = xml + "<queryByParameter>";
//                    xml = xml + "<queryId root=\"2.999.1.90\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
//                    xml = xml + "<statusCode code=\"new\"/>";
//                    xml = xml + "<initialQuantity value=\"2\"/>";
//                    xml = xml + "<matchCriterionList>";
//                    xml = xml + "<minimumDegreeMatch>";
//                    xml = xml + "<value xsi:type=\"INT\" value=\"100\"/>";
//                    xml = xml + "<semanticsText></semanticsText>";
//                    xml = xml + "</minimumDegreeMatch>";
//                    xml = xml + "</matchCriterionList>";
//                    xml = xml + "<parameterList>";
//                    xml = xml + "<livingSubjectId>";
//                    xml = xml + "<value root=\"2.999.1.41\" extension=\"" + Ssbz + "\"/>";
//                    xml = xml + "<semanticsText>" + Ssbz + "</semanticsText>";
//                    xml = xml + "</livingSubjectId>";
//                    xml = xml + "</parameterList>";
//                    xml = xml + "</queryByParameter>";
//                    xml = xml + "</controlActProcess>";
//                    xml = xml + "</PRPA_IN201305UV02>";
                  
//                    string  rtnxml="";
//                    try
//                    {
//                        //HIS提供的是wcf,接口无法调用,在病理服务器建webservice用于转发
//                        // ServiceReference1.PatientInfoFacadeClient  ss=new MQTest.ServiceReference1.PatientInfoFacadeClient();
//                           // zlyy.Url = "http://172.16.99.9:8181/Neusoft.FSH.ExternalInterface.Bll.Facade.Facade.IPatientInfoFacade.svc?wsdl";       
//                       //  rtnxml= ss.GetPatientPerson(xml);

//                        try
//                        {
////////查询患者基本信息
////////GetPatientPerson
////////查询住院患者就诊信息
////////GetInpatientInfo

////////患者医嘱接口服务
////////GetPatientOrder
//                            ZszlHISWEB_ZGQ.Service zgq = new LGHISJKZGQ.ZszlHISWEB_ZGQ.Service();
//                            rtnxml = zgq.GetPatientPerson(xml);
                         
//                        }
//                        catch(Exception ee)
//                        {
//                            MessageBox.Show(ee.Message); return "0";
//                        }

//                        if (debug == "1")
//                            log.WriteMyLog(rtnxml);
//                       try
//                       {
//                           if (rtnxml.Trim() == "")
//                           {
//                               MessageBox.Show("返回数据为空"); return "0";
//                           }
//                           if (rtnxml.Substring(0, 3) == "ERR")
//                           {
//                               MessageBox.Show(rtnxml); return "0";
//                           }
//                       }
//                       catch
//                       {
//                           MessageBox.Show("获取到的数据异常：" + rtnxml); return "0";
//                       }
                      
//                    }
//                    catch (Exception ee1)
//                    {
//                        MessageBox.Show(ee1.Message); return "0";
//                    }

                 
//                    if (rtnxml == "")
//                    {
//                        MessageBox.Show("提取错误：返回为空"); return "0";
//                    }
//                    try
//                    {
//                        StringReader xmlstr = null;
//                        XmlTextReader xmread = null;
//                        xmlstr = new StringReader(rtnxml);
//                        xmread = new XmlTextReader(xmlstr);
//                        XmlDocument readxml2 = new XmlDocument();
//                        try
//                        {
//                            readxml2.Load(xmread);
//                        }
//                        catch (Exception ee2)
//                        {
//                            MessageBox.Show("读XML失败：" + ee2.Message);
//                            return "0";
//                        }
                    
//                        XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
//                        nsMgr.AddNamespace("ns", "urn:hl7-org:v3");
//                        //错误代码
//                        XmlNode ppp = readxml2.SelectSingleNode("/ns:PRPA_IN201308UV02/ns:acknowledgement", nsMgr);
//                        string typeCode = ppp.Attributes["typeCode"].Value.ToString();
//                        if (typeCode.Trim() != "AA")
//                        {
//                            //错误信息
//                            ppp = readxml2.SelectSingleNode("/ns:PRPA_IN201308UV02/ns:acknowledgement/ns:acknowledgementDetail", nsMgr);
//                            MessageBox.Show("提取病人信息失败："+ppp["text"].InnerText);
//                            return "0";
//                        }
//                        else
//                        {
                        
//                            PT_XML pt = new PT_XML();

//                            XmlNodeList ppplist = readxml2.SelectNodes("/ns:PRPA_IN201308UV02/ns:controlActProcess/ns:subject/ns:registrationEvent/ns:subject1/ns:patient/ns:id", nsMgr);
//                            foreach (XmlNode ppp2 in ppplist)
//                            {
//                                //id
//                                if (ppp2.Attributes["root"].Value == "2.999.1.41")
//                                {
//                                    pt.myDictionary["病人编号"] = ppp2.Attributes["extension"].Value.ToString();
//                                }
//                            }

//                            ppplist = readxml2.SelectNodes("/ns:PRPA_IN201308UV02/ns:controlActProcess/ns:subject/ns:registrationEvent/ns:subject1/ns:patient/ns:patientPerson/ns:id", nsMgr);
//                            foreach (XmlNode ppp2 in ppplist)
//                            {
//                                //身份证号
//                                if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.3")
//                                {
//                                    pt.myDictionary["身份证号"] = ppp2.Attributes["extension"].Value.ToString();
//                                }
//                            }

//                            ppp = readxml2.SelectSingleNode("/ns:PRPA_IN201308UV02/ns:controlActProcess/ns:subject/ns:registrationEvent/ns:subject1/ns:patient/ns:patientPerson", nsMgr);
//                            // 姓名
//                            pt.myDictionary["姓名"] = ppp["name"].InnerText;
//                            //电话
//                            pt.myDictionary["电话"] = ppp["telecom"].Attributes["value"].Value.ToString().Trim();
//                            // 性别
//                            string xb = ppp["administrativeGenderCode"].Attributes["code"].Value.ToString();
//                            if (xb == "1")
//                                pt.myDictionary["性别"] = "男";
//                            if (xb == "2")
//                                pt.myDictionary["性别"] = "女";
//                            //出生日期
//                            pt.myDictionary["出生日期"] = ppp["birthTime"].Attributes["value"].Value.ToString();
//                            //年龄
//                            if (pt.myDictionary["出生日期"].Trim() != "")
//                                pt.myDictionary["年龄"] = ZGQClass.CsrqToAge(pt.myDictionary["出生日期"]);
//                            //婚姻
//                            string hy = ppp["maritalStatusCode"].Attributes["code"].Value.ToString();
//                            if(hy=="10")
//                              pt.myDictionary["婚姻"]="未婚";
//                            else if(hy=="30")
//                              pt.myDictionary["婚姻"]="丧偶";
//                            else if(hy=="40")
//                              pt.myDictionary["婚姻"]="离婚";
//                           else if(hy=="90")
//                              pt.myDictionary["婚姻"]="其他";
//                           else if(hy.Contains("2"))
//                              pt.myDictionary["婚姻"]="已婚";
                         
//                            //民族
//                           pt.myDictionary["民族"]= ppp["ethnicGroupCode"].Attributes["displayName"].Value.ToString();

//                            //ppp = readxml2.SelectSingleNode("/ns:PRPA_IN201308UV02/ns:controlActProcess/ns:subject/ns:registrationEvent/ns:subject1/ns:patient/ns:patientPerson/ns:addr", nsMgr);
//                            //地址
//                            //string dz = ppp["streetAddressLine"].InnerText;
//                           string exep = "";
//                           return pt.rtn_XML(ref exep);
//                        }
//                    }
//                    catch (Exception ee3)
//                    {
//                        MessageBox.Show("解析异常" + ee3.Message); return "0";
//                    }

                    #endregion
                }


                    if (Sslbx == "取消登记")
                   {
                     #region
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqd = new DataTable();
                    dt_sqd = aa.GetDataTable("select *  from  T_SQD  where F_sqxh='" +  Ssbz + "'","sqd");
                    if (dt_sqd==null)
                    {
                       MessageBox.Show("连接数据库失败");return "0";
                    }
                    if (dt_sqd.Rows.Count <=0)
                    {
                        MessageBox.Show("无此申请单记录,无需取消登记"); return "0";
                    }
                    if (dt_sqd.Rows[0]["f_sqdzt"].ToString().Trim() == "已登记")
                    {
                        aa.ExecuteSQL("update T_sqd set f_sqdzt='' where f_sqxh='" + Ssbz + "' and f_sqdzt='已登记'");
                        MessageBox.Show("取消登记成功");
                    }
                    else
                    {
                        MessageBox.Show("此申请单未登记,无需取消登记");
                    }

                        return "0";
                     #endregion
                    }
                    if (Sslbx == "取消申请单")
                    {
            
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable dt_sqd = new DataTable();
                        dt_sqd = aa.GetDataTable("select *  from  T_SQD  where F_sqxh='" + Ssbz + "'", "sqd");
                        if (dt_sqd == null)
                        {
                            MessageBox.Show("连接数据库失败"); return "0";
                        }
                        if (dt_sqd.Rows.Count <= 0)
                        {
                            MessageBox.Show("无此申请单记录"); return "0";
                        }
                        string yh = f.ReadString("yh", "yhmc", "");
                        string yhbh = f.ReadString("yh", "yhbh", "");

                          string  GUID= Guid.NewGuid().ToString() ;
                        #region  申请单撤销
                       string xml = "";
                        try
                        {
                            xml = xml + "<?xml version=\"1.0\"?>";
                            xml = xml + "<PRPA_IN000003UV02>";
                            xml = xml + "<!-- UUID,交互实例唯一码-->";
                            xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                            xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                            xml = xml + "<interactionId extension=\"PRPA_IN000003UV02\" root=\"2.16.840.1.113883.1.6\"/>";

                            xml = xml + "<receiver typeCode=\"RCV\">";
                            xml = xml + "<!-- 可以填写电话信息或者URL-->";
                            xml = xml + "<telecom value=\"\"/>";
                            xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                            xml = xml + "<telecom value=\"设备编号\"/>";
                            xml = xml + "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                            xml = xml + "</device>";
                            xml = xml + "</receiver>";

                            xml = xml + "<sender typeCode=\"SND\">";
                            xml = xml + "<telecom value=\"\"/>";
                            xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                            xml = xml + "<telecom value=\"设备编号\"/>";
                            xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                            xml = xml + "</device>";
                            xml = xml + "</sender>";

                            xml = xml + "<acknowledgement typeCode=\"AA\">";
                            xml = xml + "<targetMessage>";
                            xml = xml + "<!--请求的消息ID-->";
                            xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                            xml = xml + "</targetMessage>";
                            xml = xml + "<acknowledgementDetail>";
                            xml = xml + "<!--处理结果说明-->";
                            xml = xml + "<text>处理成功</text>";
                            xml = xml + "<resultMessage>";		
                            xml = xml + "<!--患者ID-->";
                            xml = xml + "<patientID>" + dt_sqd.Rows[0]["F_brbh"].ToString() + "</patientID>";
                            xml = xml + "<!--就诊流水号-->";
                            xml = xml + "<patientSerialNO>" + dt_sqd.Rows[0]["F_JZLSH"].ToString() + "</patientSerialNO>";
                            xml = xml + "<!--就诊类型-->";
                            xml = xml + "<encounterType>" + dt_sqd.Rows[0]["F_JZLB"].ToString() + "</encounterType>";
                            xml = xml + "<!--申请单号-->";
                            xml = xml + "<applicationNo>" + dt_sqd.Rows[0]["F_sqxh"].ToString() + "</applicationNo>";
                            xml = xml + "<!--退回原因-->";
                            xml = xml + "<reason></reason>";
                            xml = xml + "<!--操作员编码-->";
                            xml = xml + "<operCode>"+yhbh+"</operCode>";
                            xml = xml + "<!--操作员姓名-->";
                            xml = xml + "<operName>"+yh+"</operName>";
                            xml = xml + "<!--操作时间-->";
                            xml = xml + "<operDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</operDateTime>";
                            xml = xml + "<!--备注-->";
                            xml = xml + "<payMode></payMode>";
                            xml = xml + "</resultMessage>";
                            xml = xml + "</acknowledgementDetail>";
                            xml = xml + "</acknowledgement></PRPA_IN000003UV02>";
                        }
                        catch (Exception ee4)
                        {
                            MessageBox.Show("取消申请单状态：生成xml异常：" + ee4.Message);
                            return "0";
                        }

                        if (xml.Trim() == "")
                        {
                            MessageBox.Show("取消申请单状态：生成xml为空");
                            return "0";
                        }

                        if (debug == "1")
                            log.WriteMyLog("取消申请单状态---" + xml + "");


                        try
                        {
                            string msgtxt = "";
                            ZSZLWebMQ_ZGQ.MQService MQSer = new LGHISJKZGQ.ZSZLWebMQ_ZGQ.MQService();
                            MQSer.Url = "http://172.16.95.230/MQService/MQService.asmx";
                            if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_037","PIS_ExamnExpired",GUID,"取消申请单"))
                            {

                                MessageBox.Show("取消申请单状态成功");
                                aa.ExecuteSQL("update T_SQD  set F_msgxml='取消申请单', F_sqdzt='取消申请' where F_SQXH='" + dt_sqd.Rows[0]["F_sqxh"].ToString() + "'");
                            }
                            else
                            {
                                MessageBox.Show("取消申请单状态失败：" + msgtxt); return "0";
                            }
                        }
                        catch (Exception ee4)
                        {
                            MessageBox.Show("取消申请单状态异常：" + ee4.Message); return "0";
                        }
                        return "0";
                        #endregion
                    }

                    if (Sslbx == "取消审核11111")
                    {
                        #region
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable jcxx = new DataTable();
                        try
                        {
                            jcxx = aa.GetDataTable("select * from T_sqd where F_sqxh='" + Ssbz + "'", "jcxx");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                            return "0";
                        }
                        if (jcxx == null)
                        {
                            MessageBox.Show("数据库连接异常");
                            return "0";
                        }
                        if (jcxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("取消无效，未查询到对应申请单记录");
                            return "0";
                        }
                        else
                        {
                            //取消申请

                            string xml = "";
                            try
                            {
                                xml = xml + "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
                                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
                                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                                xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"COMT_IN001103UV01\"/>";
                                xml = xml + "<processingCode code=\"T\"/>";
                                xml = xml + "<processingModeCode code=\"I\"/>";
                                xml = xml + "<acceptAckCode code=\"AA\"/>";

                                xml = xml + "<receiver typeCode=\"RCV\">";
                                xml = xml + "<!-- 可以填写电话信息或者URL-->";
                                xml = xml + "<telecom value=\"\"/>";
                                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                                xml = xml + "<id root=\"2.999.1.97\" extension=\"医院信息平台\"/>";
                                xml = xml + "<telecom value=\"设备编号\"/>";
                                xml = xml + "<softwareName code=\"HIP\" displayName=\"医院信息平台\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                                xml = xml + "</device>";
                                xml = xml + "</receiver>";

                                xml = xml + "<sender typeCode=\"SND\">";
                                xml = xml + "<telecom value=\"\"/>";
                                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                                xml = xml + "<telecom value=\"设备编号\"/>";
                                xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                                xml = xml + "</device>";
                                xml = xml + "</sender>";

                                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                                xml = xml + "<signatureText></signatureText>";
                                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                                xml = xml + "</authorOrPerformer>";
                                xml = xml + "<subject typeCode=\"SUBJ\">";
                                xml = xml + "<actGenericStatus classCode=\"DOCCLIN\" moodCode=\"EVN\">";
                                xml = xml + "<!--业务活动ID（报告ID）-->";
                                xml = xml + "<id root=\"2.16.156.10011.1.1\" extension=\"\"/>";
                                xml = xml + "<!--业务活动类别 状态名称-->";


                                xml = xml + "<code code=\"30\" displayName=\"收费\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";


                                xml = xml + "<!--业务活动状态 全为completed-->";
                                xml = xml + "<statusCode code=\"completed\"/>";
                                xml = xml + "<!--业务活动期间-->";
                                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                                xml = xml + "<!--执行开始时间-->";
                                xml = xml + "<low value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                                xml = xml + "<!--执行结束时间-->";
                                xml = xml + "<high value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                                xml = xml + "</effectiveTime>";
                                xml = xml + "<!--执行者0..*-->";
                                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                                xml = xml + "<!--医务人员ID-->";
                                xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\"010929\"/>";
                                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                                xml = xml + "<name>" + "" + "</name>";
                                xml = xml + "</assignedPerson>";
                                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                                xml = xml + "<!--医疗卫生机构（科室）ID-->";
                                xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                                xml = xml + "<name>病理科</name>";
                                xml = xml + "</representedOrganization>";
                                xml = xml + "</assignedEntity>";
                                xml = xml + "</authorOrPerformer>";
                                xml = xml + "<!--所执行申请单或医嘱-->";
                                xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                                xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                                xml = xml + "<!--电子申请单编号-->";
                                xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" + Ssbz + "\"/>";
                                xml = xml + "<!--医嘱ID-->";
                                string[] yzhs = jcxx.Rows[0]["F_yzid"].ToString().Split('^');
                                foreach (string yzh in yzhs)
                                {
                                    xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + yzh + "\"/>";
                                }
                                xml = xml + "</actIntent>";
                                xml = xml + "</inFulfillmentOf>";
                                xml = xml + "<componentOf typeCode=\"COMP\" xsi:nil=\"false\">";
                                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                                xml = xml + "<!--门(急)诊流水号-->";
                                xml = xml + "<id root=\"2.999.1.91\" extension=\"" + jcxx.Rows[0]["F_mzh"].ToString() + "\"/>";
                                xml = xml + "<!--住院流水号 -->";
                                xml = xml + "<id root=\"2.999.1.42\" extension=\"" + jcxx.Rows[0]["F_zyh"].ToString() + "\"/>";

                                string brlb = jcxx.Rows[0]["F_BRLB"].ToString();
                                if (brlb == "门诊") brlb = "1";
                                if (brlb == "急诊") brlb = "2";
                                if (brlb == "住院") brlb = "3";
                                if (brlb == "其他") brlb = "9";

                                xml = xml + "<code code=\"" + brlb + "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"患者类型代码表\" displayName=\"" + jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                                xml = xml + "<statusCode/>";
                                xml = xml + "<subject typeCode=\"SBJ\">";
                                xml = xml + "<patient classCode=\"PAT\">";
                                xml = xml + "<!--平台注册的患者ID -->";
                                xml = xml + "<id root=\"2.999.1.37\" extension=\"患者ID\"/>";
                                xml = xml + "<!--本地系统的患者ID -->";
                                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + jcxx.Rows[0]["F_BRBH"].ToString() + "\"/>";
                                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + jcxx.Rows[0]["F_MZH"].ToString() + "\"/>";
                                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + jcxx.Rows[0]["F_ZYH"].ToString() + "\"/>";
                                xml = xml + "<statusCode/>";
                                xml = xml + "<patientPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                                xml = xml + "<!-- 患者姓名  -->";
                                xml = xml + "<name>" + jcxx.Rows[0]["F_xm"].ToString() + "</name>";
                                xml = xml + "</patientPerson>";
                                xml = xml + "</patient>";
                                xml = xml + "</subject>";
                                xml = xml + "</encounter>";
                                xml = xml + "</componentOf>";
                                xml = xml + "</actGenericStatus>";
                                xml = xml + "</subject>";
                                xml = xml + "</controlActProcess>";
                                xml = xml + "</COMT_IN001103UV01>";
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show("取消审核，生成XML异常：" + ee.Message);
                                return "0";
                            }
                            string msgtxt = "";
                            try
                            {
                                ZSZLWebMQ_ZGQ.MQService zgq = new LGHISJKZGQ.ZSZLWebMQ_ZGQ.MQService();

                                if (zgq.SendMessage(xml, ref msgtxt, "QI1_037", string.Empty))
                                {
                                    aa.ExecuteSQL("update  T_sqd set F_djzt=''  where F_sqxh='" + Ssbz + "'");
                                    MessageBox.Show("取消申请成功！" + msgtxt);
                                    return "0";
                                }
                                else
                                {
                                    MessageBox.Show("取消申请失败！" + msgtxt);
                                    return "0";
                                }
                            }
                            catch (Exception ee4)
                            {
                                MessageBox.Show("取消申请异常！" + ee4.Message);
                                return "0";
                            }

                        }
                        #endregion
                    }
                    if (Sslbx == "取消登记11111")
                    {
                        #region
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable dt_sqd = new DataTable();
                        dt_sqd = aa.GetDataTable("select *  from  T_SQD  where F_sqxh='" + Ssbz + "'", "sqd");
                        if (dt_sqd == null)
                        {
                            MessageBox.Show("连接数据库失败"); return "0";
                        }
                        if (dt_sqd.Rows.Count <= 0)
                        {
                            MessageBox.Show("无此申请单记录,无需取消登记"); return "0";
                        }
                        string yh = f.ReadString("yh", "yhmc", "");
                        string yhbh = f.ReadString("yh", "yhbh", "");

                        #region  状态变更
                        string xml = "";

                        try
                        {

                            string brlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
                            xml = xml + "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
                            xml = xml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
                            xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                            xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"COMT_IN001103UV01\"/>";
                            xml = xml + "<processingCode code=\"T\"/>";
                            xml = xml + "<processingModeCode code=\"I\"/>";
                            xml = xml + "<acceptAckCode code=\"AA\"/>";

                            xml = xml + "<receiver typeCode=\"RCV\">";
                            xml = xml + "<!-- 可以填写电话信息或者URL-->";
                            xml = xml + "<telecom value=\"\"/>";
                            xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<id root=\"2.999.1.97\" extension=\"医院信息平台\"/>";
                            xml = xml + "<telecom value=\"设备编号\"/>";
                            xml = xml + "<softwareName code=\"HIP\" displayName=\"医院信息平台\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                            xml = xml + "</device>";
                            xml = xml + "</receiver>";

                            xml = xml + "<sender typeCode=\"SND\">";
                            xml = xml + "<telecom value=\"\"/>";
                            xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                            xml = xml + "<telecom value=\"设备编号\"/>";
                            xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                            xml = xml + "</device>";
                            xml = xml + "</sender>";

                            xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                            xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                            xml = xml + "<signatureText></signatureText>";
                            xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                            xml = xml + "</authorOrPerformer>";
                            xml = xml + "<subject typeCode=\"SUBJ\">";
                            xml = xml + "<actGenericStatus classCode=\"DOCCLIN\" moodCode=\"EVN\">";
                            xml = xml + "<!--业务活动ID（报告ID）-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.1\" extension=\"" + "" + "\"/>";
                            xml = xml + "<!--业务活动类别 状态名称-->";
                            xml = xml + "<code code=\"30\" displayName=\"已收费\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                            xml = xml + "<!--业务活动状态 全为completed-->";
                            xml = xml + "<statusCode code=\"completed\"/>";
                            xml = xml + "<!--业务活动期间-->";
                            xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                            xml = xml + "<!--执行开始时间-->";
                            xml = xml + "<low value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                            xml = xml + "<!--执行结束时间-->";
                            xml = xml + "<high value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                            xml = xml + "</effectiveTime>";
                            xml = xml + "<!--执行者0..*-->";
                            xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                            xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                            xml = xml + "<!--医务人员ID-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\"\"/>";
                            xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<name></name>";
                            xml = xml + "</assignedPerson>";
                            xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<!--医疗卫生机构（科室）ID-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                            xml = xml + "<name>病理科</name>";
                            xml = xml + "</representedOrganization>";
                            xml = xml + "</assignedEntity>";
                            xml = xml + "</authorOrPerformer>";
                            xml = xml + "<!--所执行申请单或医嘱-->";
                            xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                            xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                            xml = xml + "<!--电子申请单编号-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" + dt_sqd.Rows[0]["F_sqxh"].ToString() + "\"/>";
                            xml = xml + "<!--医嘱ID-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + dt_sqd.Rows[0]["F_YZID"].ToString() + "\"/>";
                            xml = xml + "</actIntent>";
                            xml = xml + "</inFulfillmentOf>";
                            xml = xml + "<componentOf typeCode=\"COMP\" xsi:nil=\"false\">";
                            xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                            xml = xml + "<!--门(急)诊流水号-->";
                            xml = xml + "<id root=\"2.999.1.91\" extension=\"" + dt_sqd.Rows[0]["F_mzh"].ToString() + "\"/>";
                            xml = xml + "<!--住院流水号 -->";
                            xml = xml + "<id root=\"2.999.1.42\" extension=\"" + dt_sqd.Rows[0]["F_zyh"].ToString() + "\"/>";
                            xml = xml + "<code code=\"" + brlb + "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"患者类型代码表\" displayName=\"" + dt_sqd.Rows[0]["F_brlb"].ToString() + "\"></code>";
                            xml = xml + "<statusCode/>";
                            xml = xml + "<subject typeCode=\"SBJ\">";
                            xml = xml + "<patient classCode=\"PAT\">";
                            xml = xml + "<!--平台注册的患者ID -->";
                            xml = xml + "<id root=\"2.999.1.37\" extension=\"患者ID\"/>";
                            xml = xml + "<!--本地系统的患者ID -->";
                            xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_BRBH"].ToString() + "\"/>";
                            xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                            xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_BAH"].ToString() + "\"/>";
                            xml = xml + "<statusCode/>";
                            xml = xml + "<patientPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<!-- 患者姓名  -->";
                            xml = xml + "<name>" + dt_sqd.Rows[0]["F_xm"].ToString() + "</name>";
                            xml = xml + "</patientPerson>";
                            xml = xml + "</patient>";
                            xml = xml + "</subject>";
                            xml = xml + "</encounter>";
                            xml = xml + "</componentOf>";
                            xml = xml + "</actGenericStatus>";
                            xml = xml + "</subject>";
                            xml = xml + "</controlActProcess>";
                            xml = xml + "</COMT_IN001103UV01>";
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show(ee.Message);
                            return "";
                        }

                        if (debug == "1")
                            log.WriteMyLog("撤销状态XML---" + xml);

                        if (xml.Trim() != "")
                        {
                            string msgtxt = "";
                            try
                            {
                                ZSZLWebMQ_ZGQ.MQService MQSer = new LGHISJKZGQ.ZSZLWebMQ_ZGQ.MQService();
                                if (MQSer.SendMessage(xml, ref msgtxt, "QI1_037", string.Empty))
                                {
                                    MessageBox.Show("撤销登记状态成功");
                                    aa.ExecuteSQL("update T_sqd set F_djzt='' where F_sqxh='" + Ssbz + "'");

                                }
                                else
                                {
                                    MessageBox.Show("撤销登记状态失败！\r\n" + msgtxt);
                                    return "0";
                                }
                            }
                            catch (Exception ee4)
                            {
                                MessageBox.Show("撤销登记状态异常！\r\n" + ee4.Message);
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("撤销登记状态生成xml为空");
                            return "0";
                        }

                        #endregion

                        #region  申请单撤销
                        xml = "";
                        try
                        {
                            xml = xml + "<?xml version=\"1.0\"?>";
                            xml = xml + "<POOR_IN200903UV xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/POOR_IN200903UV.xsd\">";
                            xml = xml + "<!-- UUID,交互实例唯一码-->";
                            xml = xml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
                            xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                            xml = xml + "<interactionId extension=\"POOR_IN200903UV\" root=\"2.16.840.1.113883.1.6\"/>";
                            xml = xml + "<processingCode code=\"P\"/>";
                            xml = xml + "<processingModeCode code=\"I\"/>";
                            xml = xml + "<acceptAckCode code=\"AL\"/>";

                            xml = xml + "<receiver typeCode=\"RCV\">";
                            xml = xml + "<!-- 可以填写电话信息或者URL-->";
                            xml = xml + "<telecom value=\"\"/>";
                            xml = xml + "<device determinerCode=\"INSTANCE\" classCode=\"DEV\">";
                            xml = xml + "<id extension=\"HIP\" root=\"2.999.1.97\"/>";
                            xml = xml + "<telecom value=\"设备编号\"/>";
                            xml = xml + "<softwareName codeSystemName=\"医院信息平台系统域代码表\" codeSystem=\"2.999.2.3.2.84\" displayName=\"医院信息平台\" code=\"HIP\"/>";
                            xml = xml + "</device></receiver>";

                            xml = xml + "<sender typeCode=\"SND\"><telecom value=\"\"/>";
                            xml = xml + "<device determinerCode=\"INSTANCE\" classCode=\"DEV\"><id extension=\"PIS\" root=\"2.999.1.98\"/>";
                            xml = xml + "<telecom value=\"设备编号\"/>";
                            xml = xml + "<softwareName codeSystemName=\"医院信息平台系统域代码表\" codeSystem=\"2.999.2.3.2.84\" displayName=\"病理信息系统\" code=\"PIS\"/></device></sender>";

                            xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                            xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                            xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\">";
                            xml = xml + "<observationRequest classCode=\"OBS\" moodCode=\"RQO\">";
                            xml = xml + "<!--电子申请单编号-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" + Ssbz.Trim() + "\"/>";
                            xml = xml + "<!--医嘱ID-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + dt_sqd.Rows[0]["F_YZID"].ToString() + "\"/>";
                            xml = xml + "<!--申请单类型(需要替换本院统一字典)-->";
                            xml = xml + "<code code=\"xx\" displayName=\"" + dt_sqd.Rows[0]["F_SQDLX"].ToString() + "\" codeSystem=\"2.999.2.3.2.64\" codeSystemName=\"检查类别代码表\"/>";
                            xml = xml + "<!--申请单项目文本-->";
                            xml = xml + "<text/>";
                            xml = xml + "<!--申请单状态-->";
                            xml = xml + "<statusCode code=\"aborted\"/>";
                            xml = xml + "<!--申请单开立者（取消）-->";
                            xml = xml + "<author typeCode=\"AUT\" contextControlCode=\"OP\">";
                            xml = xml + "<!--取消时间-->";
                            xml = xml + "<time value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                            xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                            xml = xml + "<!--医务人员ID-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\"" + yhbh + "\"/>";
                            xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<!--申请单开立者（取消）-->";
                            xml = xml + "<name>" + yh + "</name>";
                            xml = xml + "</assignedPerson>";
                            xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                            xml = xml + "<!--医疗卫生机构（科室）ID-->";
                            xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                            xml = xml + "<!--申请单开立（取消）科室-->";
                            xml = xml + "<name>病理科</name>";
                            xml = xml + "</representedOrganization>";
                            xml = xml + "</assignedEntity>";
                            xml = xml + "</author>";
                            xml = xml + "</observationRequest>";
                            xml = xml + "</subject></controlActProcess></POOR_IN200903UV>";
                        }
                        catch (Exception ee4)
                        {
                            MessageBox.Show("取消申请单状态：生成xml异常：" + ee4.Message);
                            return "0";
                        }

                        if (xml.Trim() == "")
                        {
                            MessageBox.Show("取消申请单状态：生成xml为空");
                            return "0";
                        }

                        if (debug == "1")
                            log.WriteMyLog("取消申请单状态---" + xml + "");


                        try
                        {
                            string msgtxt = "";
                            ZSZLWebMQ_ZGQ.MQService MQSer = new LGHISJKZGQ.ZSZLWebMQ_ZGQ.MQService();

                            if (MQSer.SendMessage(xml, ref msgtxt, "QI1_037", string.Empty))
                            {

                                MessageBox.Show("取消申请单状态成功");
                            }
                            else
                            {
                                MessageBox.Show("取消申请单状态失败：" + msgtxt); return "0";
                            }
                        }
                        catch (Exception ee4)
                        {
                            MessageBox.Show("取消申请单状态异常：" + ee4.Message); return "0";
                        }
                        return "0";
                        #endregion
                        return "0";
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show("无此" + Sslbx);
                        log.WriteMyLog(Sslbx + Ssbz + "不存在！");
                        return "0";
                    }
            }
            else
                MessageBox.Show("识别类型不能为空");
            return "0";


        }

 
       public static string GetSQDxx(string Sslbx, string Ssbz, string debug)
       {

           dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
           DataTable dt_sqdbbxx = new DataTable();
           DataTable dt_sqd = new DataTable();
           string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
           if ( Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "诊疗卡" || Sslbx == "申请单号")
               {

               string sql ="";
               if(Sslbx == "门诊号")
                   sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_MZH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";

                if(Sslbx == "住院号")
                    sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_ZYH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";

                if(Sslbx == "诊疗卡")
                    sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_JZLSH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";

                  if(Sslbx == "申请单号")
                      sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_SQXH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";
                  if (sql.Trim() == "")
                      return "0";
               string hissql = f.ReadString(Sslbx, "hissql", sql);

               string Columns = f.ReadString(Sslbx, "Columns", "申请日期,就诊ID,姓名,性别,年龄,送检科室,送检医生,病区,床号,F_YZXMMC");
                   string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,医嘱号,姓名,性别,年龄,送检科室,送检医生,病区,床号,医嘱项目");
                   hissql = hissql.Replace("f_sbh", Ssbz.Trim());
                
                   dt_sqd = aa.GetDataTable(hissql,"sqd");

                   /////////////////////////////////////////////////////////////////////
                   if (dt_sqd == null)
                   {
                       MessageBox.Show("获取申请单信息错误：查询数据错误");
                       return "0";
                   }
                   if (dt_sqd.Rows.Count == 0)
                   {
                       MessageBox.Show("未查询到申请单信息");
                      return "0";
                   }

                   int tkts = f.ReadInteger(Sslbx, "tkts",1);
                   int count = 0;
                   if (dt_sqd.Rows.Count > tkts)
                   {
                       string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                       DataColumn dc0 = new DataColumn("序号");
                       dt_sqd.Columns.Add(dc0);

                       for (int x = 0; x < dt_sqd.Rows.Count; x++)
                       {
                           dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                       }

                       if (Columns.Trim() != "")
                           Columns = "序号," + Columns;
                       if (ColumnsName.Trim() != "")
                           ColumnsName = "序号," + ColumnsName;

                       FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                       yc.ShowDialog();
                       string rtn2 = yc.F_XH;
                       if (rtn2.Trim() == "")
                       {
                           MessageBox.Show("未选择申请项目！");
                           return "0";
                       }
                       try
                       {
                           count = int.Parse(rtn2);
                       }
                       catch
                       {
                           MessageBox.Show("请重新选择申请项目！");
                           return "0";
                       }

                   }
                   try
                   {
                       string bbmc = "";



                       string BBLB_XML = "";
                       if (tqbblb == "1")
                       {
                           dt_sqdbbxx = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_SQXH= '" + dt_sqd.Rows[count]["申请序号"].ToString().Trim() + "'  order by F_ID", "bbxx");
                           string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                           if (dt_sqdbbxx == null)
                               MessageBox.Show("查询申请单标本失败");
                           else
                           {
                               if (dt_sqdbbxx.Rows.Count <= 0)
                                   MessageBox.Show("未查询到此申请单标本记录");
                               else
                               {
                                   BBLB_XML = "<BBLB>";
                                   try
                                   {
                                       for (int x = 0; x < dt_sqdbbxx.Rows.Count; x++)
                                       {
                                           try
                                           {
                                               BBLB_XML = BBLB_XML + "<row ";
                                               BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_sqdbbxx.Rows[x]["F_XH"].ToString().Trim() + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_sqdbbxx.Rows[x]["F_BBH"].ToString().Trim() + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_sqdbbxx.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_sqdbbxx.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_sqdbbxx.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                               BBLB_XML = BBLB_XML + "/>";

                                               if (bbmc == "")
                                                   bbmc = dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim();
                                               else
                                                   bbmc = bbmc +","+ dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim();
                                           }
                                           catch (Exception eee)
                                           {
                                               MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                               tqbblb = "0";
                                               break;
                                           }
                                       }
                                   }
                                   catch (Exception e3)
                                   {
                                       MessageBox.Show("获取标本名称异常：" + e3.Message);
                                       tqbblb = "0";
                                   }
                                   BBLB_XML = BBLB_XML + "</BBLB>";
                               }
                           }
                       }

                       string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                       xml = xml + "<LOGENE>";
                       xml = xml + "<row ";
                       xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                       xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                       string xb = dt_sqd.Rows[count]["性别"].ToString().Trim();
                           if (xb == "F" || xb == "f")
                               xb = "女";
                           if (xb == "M" || xb == "m")
                               xb = "男";
                           xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                           xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                           xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["电话"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "民族=" + (char)34 + dt_sqd.Rows[count]["民族"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "职业=" + (char)34 + dt_sqd.Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "收费=" + (char)34 + dt_sqd.Rows[count]["收费"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                           xml = xml + "送检医院=" + (char)34 + dt_sqd.Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "备用1=" + (char)34 + dt_sqd.Rows[count]["备用1"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "备用2=" + (char)34 + dt_sqd.Rows[count]["备用2"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "费别=" + (char)34 + dt_sqd.Rows[count]["费别"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                           xml = xml + "/>";
                           xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                           xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                        

                          
                           if (tqbblb == "1")
                               xml = xml + BBLB_XML;
                           xml = xml + "</LOGENE>";

                       if (debug == "1")
                           log.WriteMyLog(xml);
                       return xml;


                   }
                   catch (Exception e)
                   {
                       MessageBox.Show("获取申请单信息异常：" + e.Message);
                       return "0";
                   }
               }
               else
               {
                   return "0";
               }
       }


       public static string GetYZxx(string Sslbx, string Ssbz, string debug)
       {
           string exp = "";
           string pathWEB = f.ReadString(Sslbx, "wsurl", ""); //获取sz.ini中设置的webservicesurl
           string sjdw = f.ReadString(Sslbx, "sjdw", "1"); //获取sz.ini中设置的webservicesurl
           string mrks = f.ReadString(Sslbx, "mrks", ""); //获取sz.ini中设置的mrks
      
           if (Sslbx != "")
           {
               //if (Sslbx == "病历号")
               //{
               //    int len = Ssbz.Trim().Length;
               //    if (len < 10)
               //    {
               //        for (int z = 0; z < 10 - len; z++)
               //        {
               //            Ssbz = "0" + Ssbz;
               //        }
               //    }

               //}
               string rtn_XML = "";

               if (Sslbx == "诊疗卡" || Sslbx == "病历号")
               {
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
                       MessageBox.Show("连接WebService服务器异常:" + ee.Message.ToString());
                       log.WriteMyLog("参数：" + Ssbz + ";异常消息：" + ee.Message.ToString());
                       return "0";
                   }

                   if (debug == "1")
                       log.WriteMyLog(RUCAN + "\r\n" + rtn_XML);

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
                       return "0";
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
                           xml = xml + "住院号=" + (char)34 + zyh + (char)34 + " ";
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



                           string CSRQ = dt.Rows[0]["BIRTHDAY"].ToString().Trim().Substring(0, 10);

                           string datatime = DateTime.Today.Date.ToString();

                           if (CSRQ != "")
                           {
                               if (CSRQ.Contains("-"))
                                   CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                               int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                               int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                               int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                               if (Year >= 1)
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
                           xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                       }
                       catch (Exception ee)
                       {
                           exp = exp + ee.Message.ToString();
                           xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                       }
                       //----------------------------------------------------------
                       xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
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
                       if (sjdw.Trim() == "0")
                           xml = xml + "送检医院=" + (char)34 + "" + (char)34 + " ";
                       else
                           xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                       //----------------------------------------------------------
                       xml = xml + "医嘱项目=" + (char)34 + "^"+dt.Rows[0]["ITEM_NAME"].ToString().Trim() + (char)34 + " ";
                       //----------------------------------------------------------
                       xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                       //----------------------------------------------------------
                       xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                       //----------------------------------------------------------
                       try
                       {
                           xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
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

                       MessageBox.Show("提取信息出错;" + e.ToString());
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

       public static string getbrxx(string url, string RUCAN, string sjdw, string debug, string mrks)
       {
           string exp = "";
           ZSZLWeb.Service zszl = new LGHISJKZGQ.ZSZLWeb.Service();
           zszl.Url = url;
           string rtn_XML = "";
           try
           {
               if (debug == "1")
                   MessageBox.Show("调阅webservice地址：" + zszl.Url);
               rtn_XML = zszl.GetPatientBaseInfoByCardorIcCard(RUCAN);
           }
           catch (Exception ee)
           {
               MessageBox.Show("连接WebService服务器异常:" + ee.Message.ToString());

               return "0";
           }

           if (debug == "1")
               log.WriteMyLog(RUCAN + "\r\n" + rtn_XML);

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

               MessageBox.Show("未查到相应的记录！"); return "0";

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
                   xml = xml + "住院号=" + (char)34 + zyh + (char)34 + " ";
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



                   string CSRQ = dt.Rows[0]["BIRTHDAY"].ToString().Trim().Substring(0, 10);

                   string datatime = DateTime.Today.Date.ToString();

                   if (CSRQ != "")
                   {
                       if (CSRQ.Contains("-"))
                           CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                       int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                       int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                       int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                       if (Year >= 1)
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
                   xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
               }
               catch (Exception ee)
               {
                   exp = exp + ee.Message.ToString();
                   xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
               }
               //----------------------------------------------------------
               xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
               //----------------------------------------------------------
               try
               {
                   if (mrks.Trim() != "")
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
               if (sjdw.Trim() == "0")
                   xml = xml + "送检医院=" + (char)34 + "" + (char)34 + " ";
               else
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
                   xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
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

               MessageBox.Show("提取信息出错;" + e.ToString());
               log.WriteMyLog("xml解析错误---" + e.ToString());
               return "0";
           }
       }

       
    }
}
