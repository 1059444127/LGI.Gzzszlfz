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
    //������ɽҽԺ  ---ҽ��mq���ͻ���
    class xmzsyy
    {
        //private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        //PT_XML pt = new PT_XML();
        //public static string ptxml(string Sslbx, string Ssbz)
        //{
          
        //   string  debug = f.ReadString(Sslbx, "debug", "");

        //    if (Sslbx != "")
        //    {
        //        if (Sslbx == "�����" || Sslbx == "�����1")
        //        {
                    
        //            string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";

        //          string   fid = "BS10008";
        //            return  get_mzbrxm("", fid,putcmsg, debug,"");

        //        }
        //        if (Sslbx == "סԺ��" || Sslbx == "סԺ��1")
        //        {
        //            string putcmsg = "<ESBEntry><MessageHeader><Fid>BS10001</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-4-7 16:12:32</MsgDate></MessageHeader><MsgInfo><Msg> and  Pat_Admit_ID= '" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
        //            string fid = "BS10001";
        //            PT_XML pt = new PT_XML();
        //            return get_zybrxm(fid, putcmsg, debug);
                    
        //        }
        //         if (Sslbx == "��Ʊ��" || Sslbx == "��Ʊ��1")
        //        {
        //            string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS30048</Fid></AccessControl><MessageHeader><Fid>BS30048</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and CHARGES_CODE='" + Ssbz.Trim() + "'  and EXEC_DEPT='2070000' </Msg></MsgInfo></ESBEntry>";
        //              PT_XML pt = new PT_XML();
        //           // string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS30029</Fid></AccessControl><MessageHeader><Fid>BS30029</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and PRESC_NO='" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
        //            string fid = "BS30048";
        //            string brlb="����";
        //            string lczd = "";
        //            string  mzh= get_mzh(fid,putcmsg,brlb, debug);

        //            if (debug == "1")
        //                MessageBox.Show("��ȡ����ţ�" + mzh);

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
        //        MessageBox.Show("�޴�" + Sslbx);
        //        if (debug == "1")
        //            log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");

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
        //        MessageBox.Show("��XMLʧ�ܣ�" + e2.ToString());
        //        return;
        //    }
        //    XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
        //    nsMgr.AddNamespace("ns", "urn:hl7-org:v3");

        //    XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
        //    MessageBox.Show(ppp.Attributes["extension"].Value.ToString());

        //    //////����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
        //    MessageBox.Show("������" + ppp.InnerText);

        //    //�Ա�
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
        //    MessageBox.Show("�Ա�" + ppp.Attributes["displayName"].Value);

        //    //��������
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
        //    MessageBox.Show("����" + ppp.Attributes["value"].Value);

        //    ///����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
        //    MessageBox.Show("������" + ppp.Attributes["displayName"].Value);
        //    //����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
        //    MessageBox.Show("���壺" + ppp.Attributes["displayName"].Value);

        //    //////�绰
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
        //    MessageBox.Show("�绰��" + ppp.Attributes["value"].Value);
        //    //��ַ
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
        //    MessageBox.Show(ppp.InnerText);
        //    //���֤
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
        //    MessageBox.Show("���֤��" + ppp.Attributes["extension"].InnerText);
        //    //����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
        //    MessageBox.Show("���䣺" + ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText);

        //    //����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //    MessageBox.Show("������" + ppp.InnerText);
        //    //����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);

        //    MessageBox.Show("���ң�" + ppp.InnerText);
        //    //����
        //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
        //    MessageBox.Show("���ţ�" + ppp.Attributes["extension"].Value);

        //    XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
        //    foreach (XmlNode ppp2 in ppplist)
        //    {

        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
        //        {
        //            //סԺ��
        //            MessageBox.Show("סԺ�ţ�" + ppp2.Attributes["extension"].Value.ToString());

        //        }
        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
        //        {
        //            //���￨��
        //            MessageBox.Show("���￨�ţ�" + ppp2.Attributes["extension"].Value.ToString());
        //        }
        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //        {
        //            //������
        //            MessageBox.Show("�����ţ�" + ppp2.Attributes["extension"].Value.ToString());
        //        }
        //        if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
        //        {
        //            //padid
        //            MessageBox.Show("padid��" + ppp2.Attributes["extension"].Value.ToString());
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
        //     log.WriteMyLog("������" + putcmsg + "\r\n���أ�" + getcmsg);

        // if (getcmsg.Trim() == "")
        // {
        //     MessageBox.Show("��ȡʧ�ܣ�����δ��");
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
        //     MessageBox.Show("��ȡסԺ��Ϣ�쳣1��" + e1.Message);
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
        //     MessageBox.Show("��XMLʧ�ܣ�" + e2.Message);
        //     return "0";
        // }
        // XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
        // nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


        // XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
        // //////����
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
        //     pt.myDictionary["����"] = ppp.InnerText;
        // }
        // catch
        // {
        // }

        // //�Ա�
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
        //     pt.myDictionary["�Ա�"] = ppp.Attributes["displayName"].Value;
        // }
        // catch
        // {
        // }
        // //��������
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
        //     pt.myDictionary["��������"] = ppp.Attributes["value"].Value;
        // }
        // catch
        // {
        // }
        // ///����\
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
        //     pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
        // }
        // catch
        // {
        // }
        // //����
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
        //     pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
        // }
        // catch
        // {
        // }

        // //////�绰
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
        //     pt.myDictionary["�绰"] = ppp.Attributes["value"].Value;
        // }
        // catch
        // {
        // }
        // //��ַ
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
        //     pt.myDictionary["��ַ"] = ppp.InnerText;
        // }
        // catch
        // {
        // }
        // //���֤
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
        //     pt.myDictionary["���֤��"] = ppp.Attributes["extension"].InnerText;
        // }
        // catch
        // {
        // }
        // //����
        // try
        // {
        //     if (pt.myDictionary["��������"].Trim() == "")
        //     {
        //         ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
        //         pt.myDictionary["����"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
        //     }
        // }
        // catch
        // {
        // }

        // //����
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //     pt.myDictionary["����"] = ppp.InnerText;
        // }
        // catch
        // {
        // }
        // //����
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //     pt.myDictionary["�ͼ����"] = ppp.InnerText;
        // }
        // catch
        // {
        // }
        // //����
        // try
        // {
        //     ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
        //     pt.myDictionary["����"] = ppp.Attributes["extension"].Value;
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
        //             pt.myDictionary["סԺ��"] = ppp2.Attributes["extension"].Value.ToString();

        //         }
        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
        //         {
        //             //���￨��
        //             pt.myDictionary["����ID"] = ppp2.Attributes["extension"].Value.ToString();
        //         }
        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //         {
        //             //������
        //             //   pt.myDictionary["����"] =" + ppp2.Attributes["extension"].Value.ToString());
        //         }
        //         if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
        //         {
        //             //padid
        //             pt.myDictionary["���˱��"] = ppp2.Attributes["extension"].Value.ToString();
        //         }
        //         //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //         //{
        //         //    //�����
        //         //   pt.myDictionary["�����"]=ppp2.Attributes["extension"].Value.ToString();

        //         //}
        //     }

        //     ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:component/ns:structuredBody/ns:component/ns:section/ns:entry/ns:observation", nsMgr);

        //     foreach (XmlNode ppp2 in ppplist)
        //     {
        //         //סԺ����
        //         if (ppp2["code"].Attributes["code"].Value.ToString() == "DE02.10.090.00")
        //         {
        //            pt.myDictionary["����ID"]= ppp2["value"].Attributes["value"].Value.ToString();
        //            break;

        //         }


        //     }
        // }
        // catch
        // {
        // }

   



        // string xb = pt.myDictionary["�Ա�"].Trim();
        // if (xb == "1") xb = "��";
        // else if (xb == "2") xb = "Ů";
        // pt.myDictionary["�Ա�"] = xb;

        // pt.myDictionary["�������"] = "סԺ";

       

        // if (pt.myDictionary["����"].Trim()=="")
        // pt.myDictionary["����"] = ZGQClass.CsrqToAge(pt.myDictionary["��������"]);

        

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
        //        log.WriteMyLog("������" + putcmsg + "\r\n���أ�" + getcmsg);

        //    if (getcmsg.Trim() == "")
        //    {
        //        MessageBox.Show("��ȡʧ�ܣ�����δ��");
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
        //        MessageBox.Show("��ȡ������Ϣ�쳣1��" + e1.Message);
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
        //        MessageBox.Show("��XMLʧ�ܣ�" + e2.Message);
        //        return "0";
        //    }
        //    XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
        //    nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


        //    XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
        //    //////����
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
        //        pt.myDictionary["����"] = ppp.InnerText;
        //    }
        //    catch
        //    {
        //    }

        //    //�Ա�
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
        //        pt.myDictionary["�Ա�"] = ppp.Attributes["displayName"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    //��������
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
        //        pt.myDictionary["��������"] = ppp.Attributes["value"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    ///����\
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
        //        pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    //����
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
        //        pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
        //    }
        //    catch
        //    {
        //    }

        //    //////�绰
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
        //        pt.myDictionary["�绰"] = ppp.Attributes["value"].Value;
        //    }
        //    catch
        //    {
        //    }
        //    //��ַ
        //    try
        //    {
        //      //  ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
        //      //  pt.myDictionary["��ַ"] = ppp.InnerText;
        //    }
        //    catch
        //    {
        //    }
        //    //���֤
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
        //        pt.myDictionary["���֤��"] = ppp.Attributes["extension"].InnerText;
        //    }
        //    catch
        //    {
        //    }
        //    //����
        //    try
        //    {
        //        if (pt.myDictionary["��������"].Trim() == "")
        //        {
        //            ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
        //            pt.myDictionary["����"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
        //        }
        //    }
        //    catch
        //    {
        //    }

        //    //����
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //        pt.myDictionary["����"] = ppp.InnerText;
        //    }
        //    catch
        //    {
        //    }
        //    //����
        //    //try
        //    //{
        //    //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
        //    //    pt.myDictionary["�ͼ����"] = ppp.InnerText;
        //    //}
        //    //catch
        //    //{
        //    //}
        //    //����
        //    try
        //    {
        //        ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
        //        pt.myDictionary["����"] = ppp.Attributes["extension"].Value;
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
        //            //    pt.myDictionary["סԺ��"] = ppp2.Attributes["extension"].Value.ToString();

        //            //}
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
        //            {
        //                //���￨��
        //                pt.myDictionary["����ID"] = ppp2.Attributes["extension"].Value.ToString();
        //            }
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //            {
                      
        //            }
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
        //            {
        //                //padid
        //                pt.myDictionary["���˱��"] = ppp2.Attributes["extension"].Value.ToString();
        //            }
        //            if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
        //            {
        //                //�����
        //                pt.myDictionary["�����"] = ppp2.Attributes["extension"].Value.ToString();

        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }



        //    string xb = pt.myDictionary["�Ա�"].Trim();
        //    if (xb == "1") xb = "��";
        //    else if (xb == "2") xb = "Ů";
        //    pt.myDictionary["�Ա�"] = xb;

        //    pt.myDictionary["�������"] = "����";


        //    if (pt.myDictionary["����"].Trim() == "")
        //        pt.myDictionary["����"] = ZGQClass.CsrqToAge(pt.myDictionary["��������"]);

        //    if (lczd.Trim() != "")
        //        pt.myDictionary["�ٴ����"] = lczd.Trim();

        //    pt.myDictionary["����ID"] =fph;

        //    string ex = "";
        //    string xml = pt.rtn_XML(ref ex);
        //    if (ex.Trim() != "")
        //        log.WriteMyLog(ex);
        //    return xml;

        //}

        //private static string get_mzh(string fid, string putcmsg, string brlb, string debug)
        //{

        //    PT_XML pt = new PT_XML();
        //    // ͨ����Ʊ�Ż�ȡ�ٴ���Ϻ�patientid
        //    string getcmsg = "";
        //    if (!MQ(fid, putcmsg, debug, ref  getcmsg))
        //    {
              
        //        return "";
        //    }
        //    if (debug == "1")
        //        log.WriteMyLog("������" + putcmsg + "\r\n���أ�" + getcmsg);
         
        //    if (getcmsg.Trim() == "")
        //    {
        //        MessageBox.Show("��ȡ�����ʧ�ܣ�����Ϊ��");
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
        //        MessageBox.Show("��ȡ������Ϣ�쳣1��" + e1.Message); return "";
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
        //        MessageBox.Show("��ȡ������Ϣ�쳣2��" + e2.Message); return "";
        //    }

        //    frm_xmzsyy_mzfyrq fyqr = new frm_xmzsyy_mzfyrq(ds.Tables[0]);

        //    fyqr.ShowDialog();
        //        //ȷ�Ϸ���


        //        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
        //        {
        //            if( ds.Tables[0].Rows[x]["confirm_flag"].ToString().Trim()=="1")
        //            {
        //                //��ȷ��



        //            }
        //            else
        //            {
        //                //δȷ��
        //                putcmsg = "<ESBEntry><AccessControl><UserName/><Password/><Fid>BS15015</Fid></AccessControl><MessageHeader><Fid>BS15015</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-11-17 18:49:44</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>20000</endNum><Msg></Msg><query item=\"PatientID\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["PatientID"].ToString() + "\"  splice=\"and\"/><query item=\"Times\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["Times"].ToString() + "\"  splice=\"and\"/><query item=\"ReceiptTimes\" compy=\"=\" value=\""
        //              + ds.Tables[0].Rows[0]["ReceiptTimes"].ToString() + "\"  splice=\"and\"/><query item=\"OrderNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["OrderNo"].ToString() + "\"  splice=\"and\"/><query item=\"ItemNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["ItemNo"].ToString() + "\"  splice=\"and\"/><query item=\"ConfirmFlag\" compy=\"=\" value=\"1\"  splice=\"and\"/><query item=\"ConfirmUserCode\" compy=\"=\" value=\"1000\"  splice=\"and\"/></MsgInfo></ESBEntry>";
        //                fid = "BS15015";

        //                if (!MQ(fid, putcmsg, debug, ref  getcmsg))
        //                {

        //                    MessageBox.Show("����ȷ��ʧ��");
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
        //                        MessageBox.Show("����ȷ���쳣1��" + e1.Message); 
        //                    }

                           
        //                }
        //            }
        //        }
                 
            


        //    string patient_id = "";
          
        //    try
        //    {
        //        pt.myDictionary["�ͼ����"]= ds.Tables[0].Rows[0]["PRESC_SPEC_NAME"].ToString();
        //    }
        //    catch
        //    {
        //    }
        //    try
        //    {
        //        pt.myDictionary["�ͼ�ҽ��"] = ds.Tables[0].Rows[0]["PRESC_DOC_NAME"].ToString();
        //    }
        //    catch
        //    {
        //    }
       
        //    try
        //    {
        //        pt.myDictionary["�ٴ����"] = ds.Tables[0].Rows[0]["DIAG_NAME"].ToString();
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
                if (Sslbx == "�����1")
                {
                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
                    string fid = "BS10008";
                    return mq_get_mzbrxm("", fid, putcmsg, debug, "", ref  pt);

                }
                if (Sslbx == "סԺ��1")
                {
                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS10001</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-4-7 16:12:32</MsgDate></MessageHeader><MsgInfo><Msg> and  Pat_Admit_ID= '" + Ssbz.Trim() + "'</Msg></MsgInfo></ESBEntry>";
                    string fid = "BS10001";
                    return mq_get_zybrxm(fid, putcmsg, debug, ref  pt);

                }
                if (Sslbx == "��Ʊ��1")
                {
                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS30048</Fid></AccessControl><MessageHeader><Fid>BS30048</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and CHARGES_CODE='" + Ssbz.Trim() + "'  and EXEC_DEPT='2070000' </Msg></MsgInfo></ESBEntry>";
                    string fid = "BS30048";
                    string brlb = "����";
                    string lczd = "";
                    string mzh = mq_get_mzh(fid, putcmsg, brlb, debug, ref  pt);

                    if (debug == "1")
                        MessageBox.Show("��ȡ����ţ�" + mzh);

                    if (mzh.Trim() != "")
                    {

                        putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10008</Fid></AccessControl><MessageHeader><Fid>BS10008</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-01-12 14:42:58</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>2</endNum><Msg>and Patient_Id='" + mzh.Trim() + "'</Msg></MsgInfo></ESBEntry>";
                        fid = "BS10008";
                        return mq_get_mzbrxm(Ssbz.Trim(), fid, putcmsg, debug, lczd, ref  pt);
                    }
                    else
                    {
                        MessageBox.Show("��ȡ��Ϣʧ�ܣ�δ�ܲ�ѯ�������");
                        return "0";
                    }
                }
                else
                    MessageBox.Show("�޴�" + Sslbx);
                return "0";
            }
            else
            {
                MessageBox.Show("ʶ��Ų���Ϊ��");
                return "0";
            }

        }

        private static string mq_get_zybrxm(string fid, string putcmsg, string debug, ref PT_XML pt)
        {
          

            if (debug == "1")
                log.WriteMyLog("������" + putcmsg);

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
                log.WriteMyLog("���أ�" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("��ȡʧ�ܣ�����δ��");
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
                MessageBox.Show("��ȡסԺ��Ϣ�쳣,��������ֵ�쳣��" + e1.Message);
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
                MessageBox.Show("��XMLʧ�ܣ�" + e2.Message);
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


            XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
            //////����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
                pt.myDictionary["����"] = ppp.InnerText;
            }
            catch
            {
            }

            //�Ա�
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
                pt.myDictionary["�Ա�"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //��������
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
                pt.myDictionary["��������"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            ///����\
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
                pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
                pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }

            //////�绰
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
                pt.myDictionary["�绰"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            //��ַ
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
                pt.myDictionary["��ַ"] = ppp.InnerText;
            }
            catch
            {
            }
            //���֤
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
                pt.myDictionary["���֤��"] = ppp.Attributes["extension"].InnerText;
            }
            catch
            {
            }
            //����
            try
            {
                if (pt.myDictionary["��������"].Trim() == "")
                {
                    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
                    pt.myDictionary["����"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
                }
            }
            catch
            {
            }

            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["����"] = ppp.InnerText;
            }
            catch
            {
            }
            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["�ͼ����"] = ppp.InnerText;
            }
            catch
            {
            }
            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
                pt.myDictionary["����"] = ppp.Attributes["extension"].Value;
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
                        pt.myDictionary["סԺ��"] = ppp2.Attributes["extension"].Value.ToString();

                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
                    {
                        //���￨��
                        pt.myDictionary["����ID"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    {
                        //������
                        //   pt.myDictionary["����"] =" + ppp2.Attributes["extension"].Value.ToString());
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
                    {
                        //padid
                        pt.myDictionary["���˱��"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    //{
                    //    //�����
                    //   pt.myDictionary["�����"]=ppp2.Attributes["extension"].Value.ToString();

                    //}
                }

                ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:component/ns:structuredBody/ns:component/ns:section/ns:entry/ns:observation", nsMgr);

                foreach (XmlNode ppp2 in ppplist)
                {
                    //סԺ����
                    if (ppp2["code"].Attributes["code"].Value.ToString() == "DE02.10.090.00")
                    {
                        pt.myDictionary["����ID"] = ppp2["value"].Attributes["value"].Value.ToString();
                        break;

                    }


                }
            }
            catch
            {
            }


            string xb = pt.myDictionary["�Ա�"].Trim();
            if (xb == "1") xb = "��";
            else if (xb == "2") xb = "Ů";
            pt.myDictionary["�Ա�"] = xb;
            pt.myDictionary["�������"] = "סԺ";
            if (pt.myDictionary["����"].Trim() == "")
                pt.myDictionary["����"] = ZGQClass.CsrqToAge(pt.myDictionary["��������"]);
            string ex = "";
            string xml = pt.rtn_XML(ref ex);
            if (ex.Trim() != "")
                log.WriteMyLog(ex);

            return xml;

        }

        private static string mq_get_mzbrxm(string fph, string fid, string putcmsg, string debug, string lczd, ref PT_XML pt)
        {

           

            if (debug == "1")
                log.WriteMyLog("������" + putcmsg);

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
                log.WriteMyLog("���أ�" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("��ȡʧ�ܣ�����δ��");
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
                MessageBox.Show("��ȡ������Ϣ�쳣1��" + e1.Message);
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
                MessageBox.Show("��XMLʧ�ܣ�" + e2.Message);
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


            XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
            //////����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
                pt.myDictionary["����"] = ppp.InnerText;
            }
            catch
            {
            }

            //�Ա�
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
                pt.myDictionary["�Ա�"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //��������
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
                pt.myDictionary["��������"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            ///����\
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
                pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
                pt.myDictionary["����"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }

            //////�绰
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
                pt.myDictionary["�绰"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            //��ַ
            try
            {
                //  ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
                //  pt.myDictionary["��ַ"] = ppp.InnerText;
            }
            catch
            {
            }
            //���֤
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
                pt.myDictionary["���֤��"] = ppp.Attributes["extension"].InnerText;
            }
            catch
            {
            }
            //����
            try
            {
                if (pt.myDictionary["��������"].Trim() == "")
                {
                    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
                    pt.myDictionary["����"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
                }
            }
            catch
            {
            }

            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["����"] = ppp.InnerText;
            }
            catch
            {
            }
            //����
            //try
            //{
            //    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
            //    pt.myDictionary["�ͼ����"] = ppp.InnerText;
            //}
            //catch
            //{
            //}
            //����
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
                pt.myDictionary["����"] = ppp.Attributes["extension"].Value;
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
                    //    pt.myDictionary["סԺ��"] = ppp2.Attributes["extension"].Value.ToString();

                    //}
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
                    {
                        //���￨��
                        pt.myDictionary["����ID"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    {

                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
                    {
                        //padid
                        pt.myDictionary["���˱��"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    {
                        //�����
                        pt.myDictionary["�����"] = ppp2.Attributes["extension"].Value.ToString();

                    }
                }
            }
            catch
            {
            }



            string xb = pt.myDictionary["�Ա�"].Trim();
            if (xb == "1") xb = "��";
            else if (xb == "2") xb = "Ů";
            pt.myDictionary["�Ա�"] = xb;

            pt.myDictionary["�������"] = "����";


            if (pt.myDictionary["����"].Trim() == "")
                pt.myDictionary["����"] = ZGQClass.CsrqToAge(pt.myDictionary["��������"]);

            if (lczd.Trim() != "")
                pt.myDictionary["�ٴ����"] = lczd.Trim();

            pt.myDictionary["����ID"] = fph;

            string ex = "";
            string xml = pt.rtn_XML(ref ex);
            if (ex.Trim() != "")
                log.WriteMyLog(ex);
            return xml;

        }

        private static string mq_get_mzh(string fid, string putcmsg, string brlb, string debug, ref PT_XML pt)
        {
            // ͨ����Ʊ�Ż�ȡ�ٴ���Ϻ�patientid
          
            if (debug == "1")
                log.WriteMyLog("������" + putcmsg);

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
                log.WriteMyLog("���أ�" + getcmsg);


            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("��ȡ�����ʧ�ܣ�����Ϊ��");
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
                MessageBox.Show("��ȡ������Ϣ�쳣1��" + e1.Message); return "";
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
                MessageBox.Show("��ȡ������Ϣ�쳣2��" + e2.Message); return "";
            }

            frm_xmzsyy_mzfyrq fyqr = new frm_xmzsyy_mzfyrq(ds.Tables[0]);

            fyqr.ShowDialog();
            //ȷ�Ϸ���


            for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
            {
                if (ds.Tables[0].Rows[x]["confirm_flag"].ToString().Trim() == "1")
                {
                    //��ȷ��



                }
                else
                {
                    //δȷ��
                    putcmsg = "<ESBEntry><AccessControl><UserName/><Password/><Fid>BS15015</Fid></AccessControl><MessageHeader><Fid>BS15015</Fid><SourceSysCode>S42</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>2015-11-17 18:49:44</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>20000</endNum><Msg></Msg><query item=\"PatientID\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["PatientID"].ToString() + "\"  splice=\"and\"/><query item=\"Times\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["Times"].ToString() + "\"  splice=\"and\"/><query item=\"ReceiptTimes\" compy=\"=\" value=\""
                  + ds.Tables[0].Rows[0]["ReceiptTimes"].ToString() + "\"  splice=\"and\"/><query item=\"OrderNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["OrderNo"].ToString() + "\"  splice=\"and\"/><query item=\"ItemNo\" compy=\"=\" value=\"" + ds.Tables[0].Rows[0]["ItemNo"].ToString() + "\"  splice=\"and\"/><query item=\"ConfirmFlag\" compy=\"=\" value=\"1\"  splice=\"and\"/><query item=\"ConfirmUserCode\" compy=\"=\" value=\"1000\"  splice=\"and\"/></MsgInfo></ESBEntry>";
                    fid = "BS15015";


                    getcmsg = "";
                    err_msg = "";

                    if (debug == "1")
                        log.WriteMyLog("����ȷ�ϣ�" + putcmsg);
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
                        log.WriteMyLog("���÷��أ�" + getcmsg + "***" + err_msg);

                    if (!rtn)
                    {
                        MessageBox.Show("����ȷ��ʧ��");
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
                            MessageBox.Show("����ȷ���쳣��" + e1.Message);
                        }


                    }
                }
            }




            string patient_id = "";

            try
            {
                pt.myDictionary["�ͼ����"] = ds.Tables[0].Rows[0]["PRESC_SPEC_NAME"].ToString();
            }
            catch
            {
            }
            try
            {
                pt.myDictionary["�ͼ�ҽ��"] = ds.Tables[0].Rows[0]["PRESC_DOC_NAME"].ToString();
            }
            catch
            {
            }

            try
            {
                pt.myDictionary["�ٴ����"] = ds.Tables[0].Rows[0]["DIAG_NAME"].ToString();
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
                //����
                ret = MQManagment.connectMQ();

                if (ret != 1)
                {
                    MessageBox.Show("����MQ����ʧ��!");
                    return false;
                }
                if (debug == "1")
                    MessageBox.Show("����MQ����ɹ�");

           

                string cmsgid = "";
                 getcmsg = "";
                 ret = 0;
                ret = MQManagment.putMsg(fid, putcmsg, ref cmsgid);
                ret = MQManagment.getMsgById(fid, cmsgid, 10000, ref getcmsg);

                if (debug == "1")
                {
                    log.WriteMyLog("������" + putcmsg + "\r\n���أ�" + getcmsg);
                }

                if (getcmsg.Trim() == "")
                {
                    MessageBox.Show("��ȡ��Ϣʧ�ܣ����ؿ�");
                    return false;
                }
                //�Ͽ�
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
