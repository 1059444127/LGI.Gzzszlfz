using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using dbbase;
using System.Data;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    //厦门中山医院  ---医惠mq，客户机
    class xmzsyy
    {
        //private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        //PT_XML pt = new PT_XML();
        //public static string ptxml(string Sslbx, string Ssbz)
        //{
          
        //   string  debug = f.ReadString(Sslbx, "debug", "");

        //    if (Sslbx != "")
        //    {
        //        if (Sslbx == "门诊号" || Sslbx == "门诊号1")
        //        {
                    
        //            string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";

        //          string   fid = "BS10008";
        //            return  get_mzbrxm("", fid,putcmsg, debug,"");

        //        }
        //        if (Sslbx == "住院号" || Sslbx == "住院号1")
        //        {
        //            string putcmsg = "<ESBEntry><MessageHeader><Fid>BS10001</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-4-7 16:12:32</MsgDate></MessageHeader><MsgInfo><Msg> and  Pat_Admit_ID= '" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
        //            string fid = "BS10001";
        //            PT_XML pt = new PT_XML();
        //            return get_zybrxm(fid, putcmsg, debug);
                    
        //        }
        //         if (Sslbx == "发票号" || Sslbx == "发票号1")
        //        {
        //            string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS30048</Fid></AccessControl><MessageHeader><Fid>BS30048</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and CHARGES_CODE='" + Ssbz.Trim() + "'  and EXEC_DEPT='2070000' </Msg></MsgInfo></ESBEntry>";
        //              PT_XML pt = new PT_XML();
        //           // string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS30029</Fid></AccessControl><MessageHeader><Fid>BS30029</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and PRESC_NO='" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
        //            string fid = "BS30048";
        //            string brlb="门诊";
        //            string lczd = "";
        //            string  mzh= get_mzh(fid,putcmsg,brlb, debug);

        //            if (debug == "1")
        //                MessageBox.Show("获取门诊号：" + mzh);

        //            if (mzh.Trim() != "")
        //            {
        //                putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + mzh.Trim() + "'</Msg></MsgInfo></ESBEntry>";

        //                 fid = "BS10008";
        //                 return get_mzbrxm(Ssbz.Trim(),fid, putcmsg, debug, lczd);
        //            }
        //            else
        //            {
        //                return "0";
        //            }
                     
        //            return "0";
        //        }
        //        return "0";
        //    }
        //    else
        //    {
        //        MessageBox.Show("无此" + Sslbx);
        //        if (debug == "1")
        //            log.WriteMyLog(Sslbx + Ssbz + "不存在！");

        //        return "0";
        //    }

        //}



        //private void savezy(string inxml)
        //{

        //    StringReader xmlstr = null;
        //    XmlTextReader xmread = null;
        //    xmlstr = new StringReader(inxml);
        //    xmread = new XmlTextReader(xmlstr);
        //    XmlDocument readxml2 = new XmlDocument();
        //    try
        //    {
        //        readxml2.Load(xmread);
        //    }
        //    catch (Exception e2)
        //    {
        //        MessageBox.Show("读XML失败：" + e2.ToString());
        //        return;
        //    }
        //    XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
        //    nsMgr.AddNamespace("ns", "urn:hl7-org:v3");

        //    XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
        //    MessageBox.Show(ppp.Attributes["extension"].Value.ToString());

        //    //////姓名
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
        //    MessageBox.Show("姓名：" + ppp.InnerText);

        //    //性别
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
        //    MessageBox.Show("性别：" + ppp.Attributes["displayName"].Value);

        //    //出身日期
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
        //    MessageBox.Show("出身：" + ppp.Attributes["value"].Value);

        //    ///婚姻
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
        //    MessageBox.Show("婚姻：" + ppp.Attributes["displayName"].Value);
        //    //民族
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
        //    MessageBox.Show("民族：" + ppp.Attributes["displayName"].Value);

        //    //////电话
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
        //    MessageBox.Show("电话：" + ppp.Attributes["value"].Value);
        //    //地址
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
        //    MessageBox.Show(ppp.InnerText);
        //    //身份证
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
        //    MessageBox.Show("身份证：" + ppp.Attributes["extension"].InnerText);
        //    //年龄
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
        //    MessageBox.Show("年龄：" + ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText);

        //    //病区
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //    MessageBox.Show("病区：" + ppp.InnerText);
        //    //科室
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);

        //    MessageBox.Show("科室：" + ppp.InnerText);
        //    //床号
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
        //    MessageBox.Show("床号：" + ppp.Attributes["extension"].Value);

        //    XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
        //    foreach (XmlNode ppp2 in ppplist)
        //    {

        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
        //        {
        //            //住院号
        //            MessageBox.Show("住院号：" + ppp2.Attributes["extension"].Value.ToString());

        //        }
        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
        //        {
        //            //就诊卡号
        //            MessageBox.Show("就诊卡号：" + ppp2.Attributes["extension"].Value.ToString());
        //        }
        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //        {
        //            //病案号
        //            MessageBox.Show("病案号：" + ppp2.Attributes["extension"].Value.ToString());
        //        }
        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
        //        {
        //            //padid
        //            MessageBox.Show("padid：" + ppp2.Attributes["extension"].Value.ToString());
        //        }
        //    }

        //}

        //private static string get_zybrxm(string fid,string putcmsg, string debug )
        //{

        //     PT_XML pt = new PT_XML();
        //    string   getcmsg="";
        // if (!MQ(fid, putcmsg, debug, ref  getcmsg))
        // {
        //     return "0";
        // }
        // if (debug == "1")
        //     log.WriteMyLog("参数：" + putcmsg + "\r\n返回：" + getcmsg);

        // if (getcmsg.Trim() == "")
        // {
        //     MessageBox.Show("提取失败，返回未空");
        //     return "0";
        // }
        // string RetCon = "";
        // string Msg = "";
        // string RetCode = "";

        // XmlNode xmlok = null;
        // XmlNode xmlok_msg = null;
        // XmlDocument xd = new XmlDocument();
        // try
        // {
        //     xd.LoadXml(getcmsg);
        //     xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");
        //     xmlok_msg = xd.SelectSingleNode("/ESBEntry/MsgInfo");
        //     RetCon = xmlok["RetCon"].InnerText;
        //     RetCode = xmlok["RetCode"].InnerText;
        //     Msg = xmlok_msg["Msg"].InnerText;
        // }
        // catch (Exception e1)
        // {
        //     MessageBox.Show("提取住院信息异常1：" + e1.Message);
        //     return "0";
        // }

        // if (RetCode != "1")
        // {
        //     MessageBox.Show(RetCon);
        //     return "0";
        // }

       
        // StringReader xmlstr = null;
        // XmlTextReader xmread = null;
        // xmlstr = new StringReader(Msg);
        // xmread = new XmlTextReader(xmlstr);
        // XmlDocument readxml2 = new XmlDocument();
        // try
        // {
        //     readxml2.Load(xmread);
        // }
        // catch (Exception e2)
        // {
        //     MessageBox.Show("读XML失败：" + e2.Message);
        //     return "0";
        // }
        // XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
        // nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


        // XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
        // //////姓名
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
        //     pt.myDictionary["姓名"] = ppp.InnerText;
        // }
        // catch
        // {
        // }

        // //性别
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
        //     pt.myDictionary["性别"] = ppp.Attributes["displayName"].Value;
        // }
        // catch
        // {
        // }
        // //出身日期
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
        //     pt.myDictionary["出生日期"] = ppp.Attributes["value"].Value;
        // }
        // catch
        // {
        // }
        // ///婚姻\
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
        //     pt.myDictionary["婚姻"] = ppp.Attributes["displayName"].Value;
        // }
        // catch
        // {
        // }
        // //民族
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
        //     pt.myDictionary["民族"] = ppp.Attributes["displayName"].Value;
        // }
        // catch
        // {
        // }

        // //////电话
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
        //     pt.myDictionary["电话"] = ppp.Attributes["value"].Value;
        // }
        // catch
        // {
        // }
        // //地址
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
        //     pt.myDictionary["地址"] = ppp.InnerText;
        // }
        // catch
        // {
        // }
        // //身份证
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
        //     pt.myDictionary["身份证号"] = ppp.Attributes["extension"].InnerText;
        // }
        // catch
        // {
        // }
        // //年龄
        // try
        // {
        //     if (pt.myDictionary["出生日期"].Trim() == "")
        //     {
        //         ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
        //         pt.myDictionary["年龄"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
        //     }
        // }
        // catch
        // {
        // }

        // //病区
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //     pt.myDictionary["病区"] = ppp.InnerText;
        // }
        // catch
        // {
        // }
        // //科室
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //     pt.myDictionary["送检科室"] = ppp.InnerText;
        // }
        // catch
        // {
        // }
        // //床号
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
        //     pt.myDictionary["床号"] = ppp.Attributes["extension"].Value;
        // }
        // catch
        // {
        // }
        // try
        // {
        //     XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
        //     foreach (XmlNode ppp2 in ppplist)
        //     {

        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
        //         {
        //             pt.myDictionary["住院号"] = ppp2.Attributes["extension"].Value.ToString();

        //         }
        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
        //         {
        //             //就诊卡号
        //             pt.myDictionary["就诊ID"] = ppp2.Attributes["extension"].Value.ToString();
        //         }
        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //         {
        //             //病案号
        //             //   pt.myDictionary["姓名"] =" + ppp2.Attributes["extension"].Value.ToString());
        //         }
        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
        //         {
        //             //padid
        //             pt.myDictionary["病人编号"] = ppp2.Attributes["extension"].Value.ToString();
        //         }
        //         //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //         //{
        //         //    //门诊号
        //         //   pt.myDictionary["门诊号"]=ppp2.Attributes["extension"].Value.ToString();

        //         //}
        //     }

        //     ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:component/ns:structuredBody/ns:component/ns:section/ns:entry/ns:observation", nsMgr);

        //     foreach (XmlNode ppp2 in ppplist)
        //     {
        //         //住院次数
        //         if (ppp2["code"].Attributes["code"].Value.ToString() == "DE02.10.090.00")
        //         {
        //            pt.myDictionary["就诊ID"]= ppp2["value"].Attributes["value"].Value.ToString();
        //            break;

        //         }


        //     }
        // }
        // catch
        // {
        // }

   



        // string xb = pt.myDictionary["性别"].Trim();
        // if (xb == "1") xb = "男";
        // else if (xb == "2") xb = "女";
        // pt.myDictionary["性别"] = xb;

        // pt.myDictionary["病人类别"] = "住院";

       

        // if (pt.myDictionary["年龄"].Trim()=="")
        // pt.myDictionary["年龄"] = ZGQClass.CsrqToAge(pt.myDictionary["出生日期"]);

        

        // string ex = "";
        // string xml= pt.rtn_XML(ref ex);
        // if (ex.Trim() != "")
        //     log.WriteMyLog(ex);
        //    return  xml;

        //}

        //private static string get_mzbrxm(string fph,string fid, string putcmsg, string debug, string lczd)
        //{

        //    PT_XML pt = new PT_XML();
        //    string getcmsg = "";
        //    if (!MQ(fid, putcmsg, debug, ref  getcmsg))
        //    {
        //        return "0";
        //    }
        //    if (debug == "1")
        //        log.WriteMyLog("参数：" + putcmsg + "\r\n返回：" + getcmsg);

        //    if (getcmsg.Trim() == "")
        //    {
        //        MessageBox.Show("提取失败，返回未空");
        //        return "0";
        //    }
        //    string RetCon = "";
        //    string Msg = "";
        //    string RetCode = "";

        //    XmlNode xmlok = null;
        //    XmlNode xmlok_msg = null;
        //    XmlDocument xd = new XmlDocument();
        //    try
        //    {
        //        xd.LoadXml(getcmsg);
        //        xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");
        //        xmlok_msg = xd.SelectSingleNode("/ESBEntry/MsgInfo");
        //        RetCon = xmlok["RetCon"].InnerText;
        //        RetCode = xmlok["RetCode"].InnerText;
        //        Msg = xmlok_msg["Msg"].InnerText;
        //    }
        //    catch (Exception e1)
        //    {
        //        MessageBox.Show("提取门诊信息异常1：" + e1.Message);
        //        return "0";
        //    }

        //    if (RetCode != "1")
        //    {
        //        MessageBox.Show(RetCon);
        //        return "0";
        //    }

           
        //    StringReader xmlstr = null;
        //    XmlTextReader xmread = null;
        //    xmlstr = new StringReader(Msg);
        //    xmread = new XmlTextReader(xmlstr);
        //    XmlDocument readxml2 = new XmlDocument();
        //    try
        //    {
        //        readxml2.Load(xmread);
        //    }
        //    catch (Exception e2)
        //    {
        //        MessageBox.Show("读XML失败：" + e2.Message);
        //        return "0";
        //    }
        //    XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
        //    nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


        //    XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
        //    //////姓名
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
        //        pt.myDictionary["姓名"] = ppp.InnerText;
        //    }
        //    catch
        //    {
        //    }

        //    //性别
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
        //        pt.myDictionary["性别"] = ppp.Attributes["displayName"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    //出身日期
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
        //        pt.myDictionary["出生日期"] = ppp.Attributes["value"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    ///婚姻\
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
        //        pt.myDictionary["婚姻"] = ppp.Attributes["displayName"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    //民族
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
        //        pt.myDictionary["民族"] = ppp.Attributes["displayName"].Value;
        //    }
        //    catch
        //    {
        //    }

        //    //////电话
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
        //        pt.myDictionary["电话"] = ppp.Attributes["value"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    //地址
        //    try
        //    {
        //      //  ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
        //      //  pt.myDictionary["地址"] = ppp.InnerText;
        //    }
        //    catch
        //    {
        //    }
        //    //身份证
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
        //        pt.myDictionary["身份证号"] = ppp.Attributes["extension"].InnerText;
        //    }
        //    catch
        //    {
        //    }
        //    //年龄
        //    try
        //    {
        //        if (pt.myDictionary["出生日期"].Trim() == "")
        //        {
        //            ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
        //            pt.myDictionary["年龄"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    //病区
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //        pt.myDictionary["病区"] = ppp.InnerText;
        //    }
        //    catch
        //    {
        //    }
        //    //科室
        //    //try
        //    //{
        //    //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //    //    pt.myDictionary["送检科室"] = ppp.InnerText;
        //    //}
        //    //catch
        //    //{
        //    //}
        //    //床号
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
        //        pt.myDictionary["床号"] = ppp.Attributes["extension"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    try
        //    {
        //        XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
        //        foreach (XmlNode ppp2 in ppplist)
        //        {

        //            //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
        //            //{
        //            //    pt.myDictionary["住院号"] = ppp2.Attributes["extension"].Value.ToString();

        //            //}
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
        //            {
        //                //就诊卡号
        //                pt.myDictionary["就诊ID"] = ppp2.Attributes["extension"].Value.ToString();
        //            }
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //            {
                      
        //            }
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
        //            {
        //                //padid
        //                pt.myDictionary["病人编号"] = ppp2.Attributes["extension"].Value.ToString();
        //            }
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //            {
        //                //门诊号
        //                pt.myDictionary["门诊号"] = ppp2.Attributes["extension"].Value.ToString();

        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }



        //    string xb = pt.myDictionary["性别"].Trim();
        //    if (xb == "1") xb = "男";
        //    else if (xb == "2") xb = "女";
        //    pt.myDictionary["性别"] = xb;

        //    pt.myDictionary["病人类别"] = "门诊";


        //    if (pt.myDictionary["年龄"].Trim() == "")
        //        pt.myDictionary["年龄"] = ZGQClass.CsrqToAge(pt.myDictionary["出生日期"]);

        //    if (lczd.Trim() != "")
        //        pt.myDictionary["临床诊断"] = lczd.Trim();

        //    pt.myDictionary["就诊ID"] =fph;

        //    string ex = "";
        //    string xml = pt.rtn_XML(ref ex);
        //    if (ex.Trim() != "")
        //        log.WriteMyLog(ex);
        //    return xml;

        //}

        //private static string get_mzh(string fid, string putcmsg, string brlb, string debug)
        //{

        //    PT_XML pt = new PT_XML();
        //    // 通过发票号获取临床诊断和patientid
        //    string getcmsg = "";
        //    if (!MQ(fid, putcmsg, debug, ref  getcmsg))
        //    {
              
        //        return "";
        //    }
        //    if (debug == "1")
        //        log.WriteMyLog("参数：" + putcmsg + "\r\n返回：" + getcmsg);
         
        //    if (getcmsg.Trim() == "")
        //    {
        //        MessageBox.Show("获取门诊号失败，返回为空");
        //        return "";
        //    }
        //    string RetCon = "";
        //    string Msg = "";
        //    string RetCode = "";

        //    XmlNodeList ppplist = null;
        //    XmlNode xmlok_msg = null;
        //    XmlDocument xd = new XmlDocument();
        //    try
        //    {
        //        xd.LoadXml(getcmsg);
        //        xmlok_msg = xd.SelectSingleNode("/ESBEntry/RetInfo");

        //        RetCon = xmlok_msg["RetCon"].InnerText;
        //        RetCode = xmlok_msg["RetCode"].InnerText;

        //    }
        //    catch (Exception e1)
        //    {
        //        MessageBox.Show("提取门诊信息异常1：" + e1.Message); return "";
        //    }

        //    if (RetCode != "1")
        //    {
        //        MessageBox.Show(RetCon); return "";
        //    }


        //    string msglist = "";
        //    try
        //    {
        //        ppplist = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");

        //        foreach (XmlNode ppp2 in ppplist)
        //        {
        //            msglist = msglist + ppp2.InnerText.ToString();
        //        }
        //    }
        //    catch(Exception  e3)
        //    {
        //        MessageBox.Show("e3:"+e3.Message); return "";
        //    }


        //    msglist = msglist.Replace("</body><body>", "");

        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        StringReader sr = new StringReader(msglist);
        //        XmlReader xr = new XmlTextReader(sr);
        //        ds.ReadXml(xr);
        //    }
        //    catch (Exception e2)
        //    {
        //        MessageBox.Show("提取门诊信息异常2：" + e2.Message); return "";
        //    }

        //    frm_xmzsyy_mzfyrq fyqr = new frm_xmzsyy_mzfyrq(ds.Tables[0]);

        //    fyqr.ShowDialog();
        //        //确认费用


        //        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        //        {
        //            if( ds.Tables[0].Rows[x]["confirm_flag"].ToString().Trim()=="1")
        //            {
        //                //已确认



        //            }
        //            else
        //            {
        //                //未确认
        //                putcmsg = "<ESBEntry><AccessControl><UserName/><Password/><Fid>BS15015</Fid></AccessControl><MessageHeader><Fid>BS15015</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-11-17 18:49:44</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>20000</endNum><Msg></Msg><query item=\"PatientID\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["PatientID"].ToString() + "\"  splice=\"and\"/><query item=\"Times\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["Times"].ToString() + "\"  splice=\"and\"/><query item=\"ReceiptTimes\" compy=\"=\" value=\""
        //              + ds.Tables[0].Rows[0]["ReceiptTimes"].ToString() + "\"  splice=\"and\"/><query item=\"OrderNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["OrderNo"].ToString() + "\"  splice=\"and\"/><query item=\"ItemNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["ItemNo"].ToString() + "\"  splice=\"and\"/><query item=\"ConfirmFlag\" compy=\"=\" value=\"1\"  splice=\"and\"/><query item=\"ConfirmUserCode\" compy=\"=\" value=\"1000\"  splice=\"and\"/></MsgInfo></ESBEntry>";
        //                fid = "BS15015";

        //                if (!MQ(fid, putcmsg, debug, ref  getcmsg))
        //                {

        //                    MessageBox.Show("费用确认失败");
        //                }
        //                else
        //                {

        //                    XmlNode xmlok = null;
        //                    //XmlDocument xdfy = new XmlDocument();
        //                    try
        //                    {
        //                        xd.LoadXml(getcmsg);
        //                        xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

        //                        RetCon = xmlok["RetCon"].InnerText;
        //                        RetCode = xmlok["RetCode"].InnerText;

        //                       if (RetCode != "1")
        //                        {
        //                            MessageBox.Show(RetCon);
        //                        }
        //                    }
        //                    catch (Exception e1)
        //                    {
        //                        MessageBox.Show("费用确认异常1：" + e1.Message); 
        //                    }

                           
        //                }
        //            }
        //        }
                 
            


        //    string patient_id = "";
          
        //    try
        //    {
        //        pt.myDictionary["送检科室"]= ds.Tables[0].Rows[0]["PRESC_SPEC_NAME"].ToString();
        //    }
        //    catch
        //    {
        //    }
        //    try
        //    {
        //        pt.myDictionary["送检医生"] = ds.Tables[0].Rows[0]["PRESC_DOC_NAME"].ToString();
        //    }
        //    catch
        //    {
        //    }
       
        //    try
        //    {
        //        pt.myDictionary["临床诊断"] = ds.Tables[0].Rows[0]["DIAG_NAME"].ToString();
        //    }
        //    catch
        //    {
               
        //    }
        //    try
        //    {
        //        patient_id = ds.Tables[0].Rows[0]["PatientID"].ToString();
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //    return patient_id;
            
        //}


        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        private static string weburl = "";
        public static string ptxml(string Sslbx, string Ssbz)
        {

            string debug = f.ReadString(Sslbx, "debug", "");

            weburl = f.ReadString(Sslbx, "weburl", "");

            if (Sslbx != "")
            {
                if (Sslbx == "门诊号1")
                {
                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
                    string fid = "BS10008";
                    return mq_get_mzbrxm("", fid, putcmsg, debug, "", ref  pt);

                }
                if (Sslbx == "住院号1")
                {
                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS10001</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-4-7 16:12:32</MsgDate></MessageHeader><MsgInfo><Msg> and  Pat_Admit_ID= '" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
                    string fid = "BS10001";
                    return mq_get_zybrxm(fid, putcmsg, debug, ref  pt);

                }
                if (Sslbx == "发票号1")
                {
                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS30048</Fid></AccessControl><MessageHeader><Fid>BS30048</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and CHARGES_CODE='" + Ssbz.Trim() + "'  and EXEC_DEPT='2070000' </Msg></MsgInfo></ESBEntry>";
                    string fid = "BS30048";
                    string brlb = "门诊";
                    string lczd = "";
                    string mzh = mq_get_mzh(fid, putcmsg, brlb, debug, ref  pt);

                    if (debug == "1")
                        MessageBox.Show("获取门诊号：" + mzh);

                    if (mzh.Trim() != "")
                    {

                        putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + mzh.Trim() + "'</Msg></MsgInfo></ESBEntry>";
                        fid = "BS10008";
                        return mq_get_mzbrxm(Ssbz.Trim(), fid, putcmsg, debug, lczd, ref  pt);
                    }
                    else
                    {
                        MessageBox.Show("获取信息失败，未能查询到门诊号");
                        return "0";
                    }
                }
                else
                    MessageBox.Show("无此" + Sslbx);
                return "0";
            }
            else
            {
                MessageBox.Show("识别号不能为空");
                return "0";
            }

        }

        private static string mq_get_zybrxm(string fid, string putcmsg, string debug, ref PT_XML pt)
        {
          

            if (debug == "1")
                log.WriteMyLog("参数：" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;
            try
            {
                rtn = MQ(fid, putcmsg,debug, ref  getcmsg);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
                return "0";
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("提取失败，返回未空");
                return "0";
            }
            string RetCon = "";
            string Msg = "";
            string RetCode = "";

            XmlNode xmlok = null;
            XmlNode xmlok_msg = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");
                xmlok_msg = xd.SelectSingleNode("/ESBEntry/MsgInfo");
                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;
                Msg = xmlok_msg["Msg"].InnerText;
            }
            catch (Exception e1)
            {
                MessageBox.Show("提取住院信息异常,解析返回值异常：" + e1.Message);
                return "0";
            }

            if (RetCode != "1")
            {
                MessageBox.Show(RetCon);
                return "0";
            }


            StringReader xmlstr = null;
            XmlTextReader xmread = null;
            xmlstr = new StringReader(Msg);
            xmread = new XmlTextReader(xmlstr);
            XmlDocument readxml2 = new XmlDocument();
            try
            {
                readxml2.Load(xmread);
            }
            catch (Exception e2)
            {
                MessageBox.Show("读XML失败：" + e2.Message);
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


            XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
            //////姓名
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
                pt.myDictionary["姓名"] = ppp.InnerText;
            }
            catch
            {
            }

            //性别
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
                pt.myDictionary["性别"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //出身日期
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
                pt.myDictionary["出生日期"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            ///婚姻\
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
                pt.myDictionary["婚姻"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //民族
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
                pt.myDictionary["民族"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }

            //////电话
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
                pt.myDictionary["电话"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            //地址
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
                pt.myDictionary["地址"] = ppp.InnerText;
            }
            catch
            {
            }
            //身份证
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
                pt.myDictionary["身份证号"] = ppp.Attributes["extension"].InnerText;
            }
            catch
            {
            }
            //年龄
            try
            {
                if (pt.myDictionary["出生日期"].Trim() == "")
                {
                    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
                    pt.myDictionary["年龄"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
                }
            }
            catch
            {
            }

            //病区
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["病区"] = ppp.InnerText;
            }
            catch
            {
            }
            //科室
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["送检科室"] = ppp.InnerText;
            }
            catch
            {
            }
            //床号
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
                pt.myDictionary["床号"] = ppp.Attributes["extension"].Value;
            }
            catch
            {
            }
            try
            {
                XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
                foreach (XmlNode ppp2 in ppplist)
                {

                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
                    {
                        pt.myDictionary["住院号"] = ppp2.Attributes["extension"].Value.ToString();

                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
                    {
                        //就诊卡号
                        pt.myDictionary["就诊ID"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    {
                        //病案号
                        //   pt.myDictionary["姓名"] =" + ppp2.Attributes["extension"].Value.ToString());
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
                    {
                        //padid
                        pt.myDictionary["病人编号"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    //{
                    //    //门诊号
                    //   pt.myDictionary["门诊号"]=ppp2.Attributes["extension"].Value.ToString();

                    //}
                }

                ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:component/ns:structuredBody/ns:component/ns:section/ns:entry/ns:observation", nsMgr);

                foreach (XmlNode ppp2 in ppplist)
                {
                    //住院次数
                    if (ppp2["code"].Attributes["code"].Value.ToString() == "DE02.10.090.00")
                    {
                        pt.myDictionary["就诊ID"] = ppp2["value"].Attributes["value"].Value.ToString();
                        break;

                    }


                }
            }
            catch
            {
            }


            string xb = pt.myDictionary["性别"].Trim();
            if (xb == "1") xb = "男";
            else if (xb == "2") xb = "女";
            pt.myDictionary["性别"] = xb;
            pt.myDictionary["病人类别"] = "住院";
            if (pt.myDictionary["年龄"].Trim() == "")
                pt.myDictionary["年龄"] = ZGQClass.CsrqToAge(pt.myDictionary["出生日期"]);
            string ex = "";
            string xml = pt.rtn_XML(ref ex);
            if (ex.Trim() != "")
                log.WriteMyLog(ex);

            return xml;

        }

        private static string mq_get_mzbrxm(string fph, string fid, string putcmsg, string debug, string lczd, ref PT_XML pt)
        {

           

            if (debug == "1")
                log.WriteMyLog("参数：" + putcmsg);

            string err_msg = "";
            string getcmsg = "";
            bool rtn = false;
            try
            {
                rtn = MQ(fid, putcmsg,debug, ref  getcmsg);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
                return "0";
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("提取失败，返回未空");
                return "0";
            }
            string RetCon = "";
            string Msg = "";
            string RetCode = "";

            XmlNode xmlok = null;
            XmlNode xmlok_msg = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");
                xmlok_msg = xd.SelectSingleNode("/ESBEntry/MsgInfo");
                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;
                Msg = xmlok_msg["Msg"].InnerText;
            }
            catch (Exception e1)
            {
                MessageBox.Show("提取门诊信息异常1：" + e1.Message);
                return "0";
            }

            if (RetCode != "1")
            {
                MessageBox.Show(RetCon);
                return "0";
            }


            StringReader xmlstr = null;
            XmlTextReader xmread = null;
            xmlstr = new StringReader(Msg);
            xmread = new XmlTextReader(xmlstr);
            XmlDocument readxml2 = new XmlDocument();
            try
            {
                readxml2.Load(xmread);
            }
            catch (Exception e2)
            {
                MessageBox.Show("读XML失败：" + e2.Message);
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


            XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
            //////姓名
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
                pt.myDictionary["姓名"] = ppp.InnerText;
            }
            catch
            {
            }

            //性别
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
                pt.myDictionary["性别"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //出身日期
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
                pt.myDictionary["出生日期"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            ///婚姻\
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
                pt.myDictionary["婚姻"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //民族
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
                pt.myDictionary["民族"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }

            //////电话
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
                pt.myDictionary["电话"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            //地址
            try
            {
                //  ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
                //  pt.myDictionary["地址"] = ppp.InnerText;
            }
            catch
            {
            }
            //身份证
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
                pt.myDictionary["身份证号"] = ppp.Attributes["extension"].InnerText;
            }
            catch
            {
            }
            //年龄
            try
            {
                if (pt.myDictionary["出生日期"].Trim() == "")
                {
                    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
                    pt.myDictionary["年龄"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
                }
            }
            catch
            {
            }

            //病区
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["病区"] = ppp.InnerText;
            }
            catch
            {
            }
            //科室
            //try
            //{
            //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
            //    pt.myDictionary["送检科室"] = ppp.InnerText;
            //}
            //catch
            //{
            //}
            //床号
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
                pt.myDictionary["床号"] = ppp.Attributes["extension"].Value;
            }
            catch
            {
            }
            try
            {
                XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
                foreach (XmlNode ppp2 in ppplist)
                {

                    //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
                    //{
                    //    pt.myDictionary["住院号"] = ppp2.Attributes["extension"].Value.ToString();

                    //}
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
                    {
                        //就诊卡号
                        pt.myDictionary["就诊ID"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    {

                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
                    {
                        //padid
                        pt.myDictionary["病人编号"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    {
                        //门诊号
                        pt.myDictionary["门诊号"] = ppp2.Attributes["extension"].Value.ToString();

                    }
                }
            }
            catch
            {
            }



            string xb = pt.myDictionary["性别"].Trim();
            if (xb == "1") xb = "男";
            else if (xb == "2") xb = "女";
            pt.myDictionary["性别"] = xb;

            pt.myDictionary["病人类别"] = "门诊";


            if (pt.myDictionary["年龄"].Trim() == "")
                pt.myDictionary["年龄"] = ZGQClass.CsrqToAge(pt.myDictionary["出生日期"]);

            if (lczd.Trim() != "")
                pt.myDictionary["临床诊断"] = lczd.Trim();

            pt.myDictionary["就诊ID"] = fph;

            string ex = "";
            string xml = pt.rtn_XML(ref ex);
            if (ex.Trim() != "")
                log.WriteMyLog(ex);
            return xml;

        }

        private static string mq_get_mzh(string fid, string putcmsg, string brlb, string debug, ref PT_XML pt)
        {
            // 通过发票号获取临床诊断和patientid
          
            if (debug == "1")
                log.WriteMyLog("参数：" + putcmsg);

            string err_msg = "";
            string getcmsg = "";
            bool rtn = false;
            try
            {
                rtn = MQ(fid, putcmsg,debug, ref  getcmsg);
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
                return "0";
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);


            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("获取门诊号失败，返回为空");
                return "";
            }
            string RetCon = "";
          
            string RetCode = "";

            XmlNodeList ppplist = null;
            XmlNode xmlok_msg = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok_msg = xd.SelectSingleNode("/ESBEntry/RetInfo");

                RetCon = xmlok_msg["RetCon"].InnerText;
                RetCode = xmlok_msg["RetCode"].InnerText;

            }
            catch (Exception e1)
            {
                MessageBox.Show("提取门诊信息异常1：" + e1.Message); return "";
            }

            if (RetCode != "1")
            {
                MessageBox.Show(RetCon); return "";
            }


            string msglist = "";
            try
            {
                ppplist = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");

                foreach (XmlNode ppp2 in ppplist)
                {
                    msglist = msglist + ppp2.InnerText.ToString();
                }
            }
            catch (Exception e3)
            {
                MessageBox.Show("e3:" + e3.Message); return "";
            }


            msglist = msglist.Replace("</body><body>", "");

            DataSet ds = new DataSet();
            try
            {
                StringReader sr = new StringReader(msglist);
                XmlReader xr = new XmlTextReader(sr);
                ds.ReadXml(xr);
            }
            catch (Exception e2)
            {
                MessageBox.Show("提取门诊信息异常2：" + e2.Message); return "";
            }

            frm_xmzsyy_mzfyrq fyqr = new frm_xmzsyy_mzfyrq(ds.Tables[0]);

            fyqr.ShowDialog();
            //确认费用


            for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
            {
                if (ds.Tables[0].Rows[x]["confirm_flag"].ToString().Trim() == "1")
                {
                    //已确认



                }
                else
                {
                    //未确认
                    putcmsg = "<ESBEntry><AccessControl><UserName/><Password/><Fid>BS15015</Fid></AccessControl><MessageHeader><Fid>BS15015</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-11-17 18:49:44</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>20000</endNum><Msg></Msg><query item=\"PatientID\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["PatientID"].ToString() + "\"  splice=\"and\"/><query item=\"Times\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["Times"].ToString() + "\"  splice=\"and\"/><query item=\"ReceiptTimes\" compy=\"=\" value=\""
                  + ds.Tables[0].Rows[0]["ReceiptTimes"].ToString() + "\"  splice=\"and\"/><query item=\"OrderNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["OrderNo"].ToString() + "\"  splice=\"and\"/><query item=\"ItemNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["ItemNo"].ToString() + "\"  splice=\"and\"/><query item=\"ConfirmFlag\" compy=\"=\" value=\"1\"  splice=\"and\"/><query item=\"ConfirmUserCode\" compy=\"=\" value=\"1000\"  splice=\"and\"/></MsgInfo></ESBEntry>";
                    fid = "BS15015";


                    getcmsg = "";
                    err_msg = "";

                    if (debug == "1")
                        log.WriteMyLog("费用确认：" + putcmsg);
                    try
                    {
                        rtn = MQ(fid, putcmsg,debug ,ref  getcmsg);
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.Message);
                        return "0";
                    }
                    if (debug == "1")
                        log.WriteMyLog("费用返回：" + getcmsg + "***" + err_msg);

                    if (!rtn)
                    {
                        MessageBox.Show("费用确认失败");
                    }
                    else
                    {

                        XmlNode xmlok = null;
                        //XmlDocument xdfy = new XmlDocument();
                        try
                        {
                            xd.LoadXml(getcmsg);
                            xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

                            RetCon = xmlok["RetCon"].InnerText;
                            RetCode = xmlok["RetCode"].InnerText;

                            if (RetCode != "1")
                            {
                                MessageBox.Show(RetCon);
                            }
                        }
                        catch (Exception e1)
                        {
                            MessageBox.Show("费用确认异常：" + e1.Message);
                        }


                    }
                }
            }




            string patient_id = "";

            try
            {
                pt.myDictionary["送检科室"] = ds.Tables[0].Rows[0]["PRESC_SPEC_NAME"].ToString();
            }
            catch
            {
            }
            try
            {
                pt.myDictionary["送检医生"] = ds.Tables[0].Rows[0]["PRESC_DOC_NAME"].ToString();
            }
            catch
            {
            }

            try
            {
                pt.myDictionary["临床诊断"] = ds.Tables[0].Rows[0]["DIAG_NAME"].ToString();
            }
            catch
            {

            }
            try
            {
                patient_id = ds.Tables[0].Rows[0]["PatientID"].ToString();
            }
            catch
            {
                return "";
            }
            return patient_id;

        }

        private static bool MQ(string fid,string putcmsg, string debug, ref string getcmsg)
        {
            try
            {
                MQDLL.MQFunction MQManagment = new MQDLL.MQFunction();
                long ret = 0;
                //连接
                ret = MQManagment.connectMQ();

                if (ret != 1)
                {
                    MessageBox.Show("连接MQ服务失败!");
                    return false;
                }
                if (debug == "1")
                    MessageBox.Show("连接MQ服务成功");

           

                string cmsgid = "";
                 getcmsg = "";
                 ret = 0;
                ret = MQManagment.putMsg(fid, putcmsg, ref cmsgid);
                ret = MQManagment.getMsgById(fid, cmsgid, 10000, ref getcmsg);

                if (debug == "1")
                {
                    log.WriteMyLog("参数：" + putcmsg + "\r\n返回：" + getcmsg);
                }

                if (getcmsg.Trim() == "")
                {
                    MessageBox.Show("提取信息失败，返回空");
                    return false;
                }
                //断开
                MQManagment.disconnectMQ();
            }
            catch(Exception  ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }

            return true;
        }
    }
}
