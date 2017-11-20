
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    /// <summary>
    /// �����еڶ�ҽԺ, 
    /// 1,�ڷ�������װmq�ͻ��ˣ�ͨ��webservice����
    /// 2,�ͻ����ϰ�װmq�ͻ��ˣ�ֱ������
    /// </summary>
    class fzs2y
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        private static string weburl = "";
        public static string ptxml(string Sslbx, string Ssbz,string debug)
        {

             debug = f.ReadString(Sslbx, "debug", "");
            int ljfs = f.ReadInteger(Sslbx, "ljfs", 1);
            weburl = f.ReadString(Sslbx, "wsurl", "");

            if (Sslbx != "")
            {

                if (Sslbx=="�����(��)")
                {
                  string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10020</Fid></AccessControl><MessageHeader><Fid>BS10020</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><onceFlag>0</onceFlag><startNum></startNum><endNum></endNum><Msg></Msg><query item=\"IC_CARD_ID\" compy=\"=\" value=\"'" + Ssbz + "'\" splice=\"and\"/><order item=\"\" sort=\"\"/></MsgInfo></ESBEntry>";

                    return getmzbrxx("BS10020", putcmsg, debug, ljfs);
                 
                }
                if (Sslbx == "סԺ��(��)")
                {

                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10001</Fid></AccessControl><MessageHeader><Fid>BS10001</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><onceFlag>0</onceFlag><startNum></startNum><endNum></endNum><Msg></Msg><query item=\"Pat_Admit_ID\" compy=\" like \" value=\"'" + Ssbz + "%'\" splice=\"and\"/><order item=\"\" sort=\"\"/></MsgInfo></ESBEntry>";

                      return getzybrxx("BS10001", putcmsg, debug, ref pt, ljfs);

                }


                if (Sslbx == "סԺ��(����)" || Sslbx == "סԺ��")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25016</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  residence_no like '" + Ssbz + "%'</Msg></MsgInfo></ESBEntry>";

                    string rtnxml = getsqdxx("BS25016", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("���ص�xml�ַ���:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "����" || Sslbx == "�����(����)" || Sslbx == "�����")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25017</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  ic_card_id= '" + Ssbz + "' </Msg></MsgInfo></ESBEntry>";
                    string rtnxml = getsqdxx("BS25017", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("���ص�xml�ַ���:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "סԺ�����")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25016</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  apply_no= '" + Ssbz + "'</Msg></MsgInfo></ESBEntry>";
                    string rtnxml = getsqdxx("BS25016", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("���ص�xml�ַ���:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "���������")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25017</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  apply_no= '" + Ssbz + "'</Msg></MsgInfo></ESBEntry>";
                    string rtnxml = getsqdxx("BS25017", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("���ص�xml�ַ���:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "�����б�")
                {
                    fzs2y_mzlb mzlb = new fzs2y_mzlb();
                    if (mzlb.ShowDialog() == DialogResult.Yes)
                    {
                        string IC_CARD_ID = mzlb.IC_CARD_ID;
                        if (IC_CARD_ID.Trim() == "")
                        {
                            MessageBox.Show("��ѡ��Ĳ��˿���Ϊ��,�޷���ȡ��Ϣ��");
                            return "0";
                        }
                        else
                        {
                            string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25017</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  ic_card_id= '" + IC_CARD_ID + "'</Msg></MsgInfo></ESBEntry>";
                            string rtnxml = getsqdxx("BS25017", putcmsg, debug, ljfs, Ssbz);
                            if (debug == "1")
                                log.WriteMyLog("���ص�xml�ַ���:" + rtnxml);
                            return rtnxml;
                        }

                    }
                    else
                    {
                        MessageBox.Show("δѡ����,�޷���ȡ��Ϣ��");
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

       //��ȡ���˻�����Ϣ
        private static string getzybrxx(string fid, string putcmsg, string debug, ref PT_XML pt,int ljfs)
        {
            if (debug == "1")
                log.WriteMyLog("��Σ�" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }
            else
            {
               
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
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
                MessageBox.Show("��ȡʧ�ܣ�����Ϊ��");
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
               
                    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
                    pt.myDictionary["����"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
                
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
            //ҽ��
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:author/ns:assignedAuthor/ns:assignedPerson/ns:name", nsMgr);
                pt.myDictionary["�ͼ�ҽ��"] = ppp.InnerText;
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
                    //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    //{
                    //    //������
                    //       pt.myDictionary["���˱��"] = + ppp2.Attributes["extension"].Value.ToString();
                    //}
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

                //ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:component/ns:structuredBody/ns:component/ns:section/ns:entry/ns:observation", nsMgr);

                //foreach (XmlNode ppp2 in ppplist)
                //{
                //    //סԺ����
                //    if (ppp2["code"].Attributes["code"].Value.ToString() == "DE02.10.090.00")
                //    {
                //        pt.myDictionary["����ID"] = ppp2["value"].Attributes["value"].Value.ToString();
                //        break;

                //    }


                //}
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
            if (pt.myDictionary["����"].Trim() == "")
            {
                MessageBox.Show("��ȡ���ݴ���");
                return "0";
            }


            string ex = "";
            string xml = pt.rtn_XML(ref ex);
            if (ex.Trim() != "")
                log.WriteMyLog(ex);

            return xml;

        }

        //��ȡ���ﲡ�˻�����Ϣ
        private static string getmzbrxx(string fid, string putcmsg, string debug, int ljfs)
        {


            if (debug == "1")
                log.WriteMyLog("��Σ�" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }
            else
            {
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
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
                MessageBox.Show("��ȡʧ�ܣ�����Ϊ��");
                return "0";
            }
            string RetCon = "";
           
            string RetCode = "";

            string bobys = "";

            XmlNode xmlok = null;
            XmlNodeList xmlNL = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;
                if (RetCode != "1")
                {
                    MessageBox.Show(RetCon);
                    return "0";
                }


                xmlNL = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");
                foreach (XmlNode xn in xmlNL)
                {
                    XmlNode xmlok_DATA = null;

                    try
                    {
                        xd.LoadXml(xn.InnerText);
                        xmlok_DATA = xd.SelectSingleNode("/body");
                    }
                    catch
                    {
                    }

                    bobys = bobys + xmlok_DATA.InnerXml;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("��ȡ��Ϣ�쳣,��������ֵ�쳣��" + e1.Message);
                return "0";
            }

     
            //ת��dataset
            XmlNode xmlok_DATA2 = null;

            try
            {
                xd.LoadXml("<body>" + bobys + "</body>");
                xmlok_DATA2 = xd.SelectSingleNode("/body");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("����DATA�쳣��" + xmlok_e.Message);
                return "0";
            }
            if (xmlok_DATA2.InnerXml.Trim() == "")
            {
                MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
                return "0";
            }
            DataSet ds1 = new DataSet();

            try
            {
                StringReader sr = new StringReader(xmlok_DATA2.OuterXml);
                XmlReader xr = new XmlTextReader(sr);
                ds1.ReadXml(xr);

            }
            catch (Exception eee)
            {
                MessageBox.Show("XMLתdataset�쳣��" + eee.Message);
                return "0";
            }

            if (ds1.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("δ��ѯ���˲�����Ϣ");
                return "0";
            }


            int xh = 0;


            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["SICK_ID"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����ID=" + (char)34 + ds1.Tables[0].Rows[xh]["IC_CARD_ID"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + ""+ (char)34 + " ";


            xml = xml + "�����=" + (char)34 + ds1.Tables[0].Rows[xh]["IC_CARD_ID"].ToString().Trim() + (char)34 + " ";
                xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
           
            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["NAME"].ToString().Trim() + (char)34 + " ";

            string xb = ds1.Tables[0].Rows[xh]["SEX"].ToString().Trim();
            if (xb == "0") xb = "��";
            else
                if (xb == "1") xb = "Ů"; else xb = "";
            xml = xml + "�Ա�=" + (char)34 + xb+ (char)34 + " ";

           string nl= ZGQClass.CsrqToAge(ds1.Tables[0].Rows[xh]["BIRTHDATE"].ToString().Trim());
           xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "��ַ=" + (char)34 + ds1.Tables[0].Rows[xh]["COMMUNICATE_ADDRESS"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�绰=" + (char)34 + ds1.Tables[0].Rows[xh]["FAMILY_PHONE"].ToString().Trim() + (char)34 + " ";
            xml = xml + "���֤��=" + (char)34 + ds1.Tables[0].Rows[xh]["ID_CARD_NO"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["NATION"].ToString().Trim() + (char)34 + " ";
            xml = xml + "ְҵ=" + (char)34 + ds1.Tables[0].Rows[xh]["PROFESSION"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�շ�=" + (char)34 + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            xml = xml + "�ѱ�=" + (char)34 + ds1.Tables[0].Rows[xh]["RATE_TYPE"].ToString().Trim() + (char)34 + " ";
            
            xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + ""+ "]]></�ٴ����>";
            xml = xml + "</LOGENE>";


            return xml;

        }

        //��ȡ���뵥��Ϣ
        private static string getsqdxx(string fid, string putcmsg, string debug, int ljfs, string Ssbz)
        {


            if (debug == "1")
                log.WriteMyLog("��Σ�" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }
            else
            {
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
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
                MessageBox.Show("��ȡʧ�ܣ�����Ϊ��");
                return "0";
            }
            string RetCon = "";
          
            string RetCode = "";

            string bobys = "";

            XmlNode xmlok = null;
            XmlNodeList xmlNL = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;

                if (RetCode != "1")
                {
                    MessageBox.Show(RetCon);
                    return "0";
                }


                xmlNL = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");
                foreach (XmlNode xn in xmlNL)
                {
                    XmlNode xmlok_DATA = null;

                    try
                    {
                        xd.LoadXml(xn.InnerText);
                        xmlok_DATA = xd.SelectSingleNode("/body");
                    }
                    catch
                    {
                    }

                    bobys = bobys + xmlok_DATA.InnerXml;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("��ȡ��Ϣ�쳣,��������ֵ�쳣��" + e1.Message);
                return "0";
            }

            

            //ת��dataset
            XmlNode xmlok_DATA2 = null;

            try
            {
                xd.LoadXml("<body>" + bobys + "</body>");
                xmlok_DATA2 = xd.SelectSingleNode("/body");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("����DATA�쳣��" + xmlok_e.Message);
                return "0";
            }
            if (xmlok_DATA2.InnerXml.Trim() == "")
            {
                MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
                return "0";
            }
            DataSet ds1 = new DataSet();

            try
            {
                StringReader sr = new StringReader(xmlok_DATA2.OuterXml);
                XmlReader xr = new XmlTextReader(sr);
                ds1.ReadXml(xr);

            }
            catch (Exception eee)
            {
                MessageBox.Show("XMLתdataset�쳣��" + eee.Message);
                return "0";
            }

            if (ds1.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("δ��ѯ���˲�����Ϣ");
                return "0";
            }


            int xh = 0;

            if (ds1.Tables[0].Rows.Count > 1)
            {

                string Columns = "insur_no,apply_no,name,sex,age,residence_no,bed_no,apply_dept,apply_doctor,item_name,now_diagnosis";//��ʾ����Ŀ��Ӧ�ֶ�
                string ColumnsName = "ҽ������,���ݺ�,����,�Ա�,����,סԺ��,����,�ͼ����,�ͼ�ҽ��,��Ŀ����,�ٴ����";//��ʾ����Ŀ����
                if (fid == "BS25017")
                {
                    Columns = "insur_no,apply_no,name,sex,age,apply_dept,apply_doctor,item_name,now_diagnosis";//��ʾ����Ŀ��Ӧ�ֶ�
                    ColumnsName = "ҽ������,���ݺ�,����,�Ա�,����,�ͼ����,�ͼ�ҽ��,��Ŀ����,�ٴ����";//��ʾ����Ŀ����
                }

                string xsys = "1"; //ѡ����������Ŀ
                DataColumn dc0 = new DataColumn("���");
                ds1.Tables[0].Columns.Add(dc0);
                for (int x = 0; x < ds1.Tables[0].Rows.Count; x++)
                {
                    ds1.Tables[0].Rows[x][ds1.Tables[0].Columns.Count - 1] = x;
                }
                if (Columns.Trim() != "")
                    Columns = "���," + Columns;
                if (ColumnsName.Trim() != "")
                    ColumnsName = "���," + ColumnsName;

                FRM_YZ_SELECT yc = new FRM_YZ_SELECT(ds1.Tables[0], Columns, ColumnsName, xsys);
                yc.ShowDialog();
                string rtn2 = yc.F_XH;
                if (rtn2.Trim() == "")
                {
                    MessageBox.Show("δѡ��������Ŀ��");
                    return "0";
                }
                try
                {
                    xh = int.Parse(rtn2);
                }
                catch
                {
                    MessageBox.Show("������ѡ��������Ŀ��");
                    return "0";
                }
            }
            else
            {
                if (ds1.Tables[0].Rows.Count < 1)
                {
                    MessageBox.Show("δ��ѯ���˲�����Ϣ");
                    return "0";
                }
            }

            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["sick_id"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����ID=" + (char)34 + ds1.Tables[0].Rows[xh]["insur_no"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_no"].ToString().Trim() + (char)34 + " ";

            if (fid == "BS25016")
            {
                xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "סԺ��=" + (char)34 + ds1.Tables[0].Rows[xh]["residence_no"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["dept_name"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["bed_no"].ToString().Trim() + (char)34 + " ";
            }
            else
            {
                string mzh = ds1.Tables[0].Rows[xh]["insur_no"].ToString().Trim();
                if (mzh == "")
                    mzh = Ssbz;
                xml = xml + "�����=" + (char)34 + mzh + (char)34 + " ";
                xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            }
            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["name"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�Ա�=" + (char)34 + ds1.Tables[0].Rows[xh]["sex"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["age"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["marital_status"].ToString().Trim() + (char)34 + " ";
            xml = xml + "��ַ=" + (char)34 + ds1.Tables[0].Rows[xh]["communicate_address"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�绰=" + (char)34 + ds1.Tables[0].Rows[xh]["family_phone"].ToString().Trim() + (char)34 + " ";
            xml = xml + "���֤��=" + (char)34 + ds1.Tables[0].Rows[xh]["id_card_no"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["nation"].ToString().Trim() + (char)34 + " ";
            xml = xml + "ְҵ=" + (char)34 + ds1.Tables[0].Rows[xh]["profession"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�ͼ����=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_dept"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_doctor"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�շ�=" + (char)34 + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + ds1.Tables[0].Rows[xh]["item_name"].ToString().Trim() + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            xml = xml + "�ѱ�=" + (char)34 + ds1.Tables[0].Rows[xh]["rate_type"].ToString().Trim() + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + ds1.Tables[0].Rows[xh]["table_type"].ToString().Trim() + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + ds1.Tables[0].Rows[xh]["clinical_history"].ToString().Trim() + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + ds1.Tables[0].Rows[xh]["now_diagnosis"].ToString().Trim() + "]]></�ٴ����>";
            xml = xml + "</LOGENE>";


            return xml;

        }

        //����MQ�ͻ���
        private static bool MQ(string fid, string putcmsg, string debug, ref string getcmsg)
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

                

                if (getcmsg.Trim() == "")
                {
                    MessageBox.Show("��ȡ��Ϣʧ�ܣ����ؿ�");
                    return false;
                }
                //�Ͽ�
                MQManagment.disconnectMQ();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }

            return true;
        }

    
    }
}
