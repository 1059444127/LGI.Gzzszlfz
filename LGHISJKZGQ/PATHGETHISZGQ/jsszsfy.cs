using System;
using System.Collections.Generic;
using System.Text;
using LGHISJKZGQ;
using System.Windows.Forms;
using System.Xml;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    //�ո�һ
    class jsszsfy
    {
      
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            string pathWEB = f.ReadString(Sslbx, "url", "http://10.1.1.71/webapp/PathologyService.asmx").Trim(); //��ȡsz.ini�����õ�webservicesurl
            string debug = f.ReadString(Sslbx, "debug", "");
            string yq = f.ReadString(Sslbx, "yq", "2");
           
            if(yq.Trim()=="ʮ����")  yq="1";
             if(yq.Trim()=="ƽ��")  yq="2";
 
            if (Sslbx != "")
            {

                if (Sslbx == "סԺ��" || Sslbx == "�����" || Sslbx == "��Ʊ��" || Sslbx == "ʮ����סԺ��" || Sslbx == "ʮ���������" || Sslbx == "ʮ���ַ�Ʊ��" || Sslbx == "ƽ��סԺ��" || Sslbx == "ƽ�������" || Sslbx == "ƽ����Ʊ��")
                {

                    string  Request_xml="";
                    if (Sslbx == "סԺ��")
                    {
                        Request_xml="<Request>"
                                    +"<yq>"+yq+"</yq>"
                                    +"<paitent_id></paitent_id>"
                                    +"<real_no></real_no>"
                                    +"<inpatient_no>"+Ssbz.Trim()+"</inpatient_no>"			//סԺ��
                                    +"</Request>";

                    }

                    if (Sslbx == "�����")
                    {
                        Request_xml="<Request>"
                                    +"<yq>"+yq+"</yq>"							//Ժ����1��ʮ���֣�2��ƽ����
                                    +"<paitent_id>"+Ssbz.Trim()+"</paitent_id>"		//����ţ����
                                    +"<real_no></real_no>"				//��Ʊ�ţ���ѡ��
                                    +"<inpatient_no></inpatient_no>"
                                    +"</Request>";
                    }

                    if (Sslbx == "��Ʊ��")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>" + yq + "</yq>"							//Ժ����1��ʮ���֣�2��ƽ����
                                    + "<paitent_id></paitent_id>"		//����ţ����
                                    + "<real_no>" + Ssbz.Trim() + "</real_no>"				//��Ʊ�ţ���ѡ��
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "ʮ����סԺ��")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>1</yq>"
                                    + "<paitent_id></paitent_id>"
                                    + "<real_no></real_no>"
                                    + "<inpatient_no>" + Ssbz.Trim() + "</inpatient_no>"			//סԺ��
                                    + "</Request>";

                    }

                    if (Sslbx == "ʮ���������")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>1</yq>"							//Ժ����1��ʮ���֣�2��ƽ����
                                    + "<paitent_id>" + Ssbz.Trim() + "</paitent_id>"		//����ţ����
                                    + "<real_no></real_no>"				//��Ʊ�ţ���ѡ��
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "ʮ���ַ�Ʊ��")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>1</yq>"							//Ժ����1��ʮ���֣�2��ƽ����
                                    + "<paitent_id></paitent_id>"		//����ţ����
                                    + "<real_no>" + Ssbz.Trim() + "</real_no>"				//��Ʊ�ţ���ѡ��
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "ƽ��סԺ��")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>2</yq>"
                                    + "<paitent_id></paitent_id>"
                                    + "<real_no></real_no>"
                                    + "<inpatient_no>" + Ssbz.Trim() + "</inpatient_no>"			//סԺ��
                                    + "</Request>";

                    }

                    if (Sslbx == "ƽ�������")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>2</yq>"							//Ժ����1��ʮ���֣�2��ƽ����
                                    + "<paitent_id>" + Ssbz.Trim() + "</paitent_id>"		//����ţ����
                                    + "<real_no></real_no>"				//��Ʊ�ţ���ѡ��
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "ƽ����Ʊ��")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>2</yq>"							//Ժ����1��ʮ���֣�2��ƽ����
                                    + "<paitent_id></paitent_id>"		//����ţ����
                                    + "<real_no>" + Ssbz.Trim() + "</real_no>"				//��Ʊ�ţ���ѡ��
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if(Request_xml.Trim()=="")
                    {
                      MessageBox.Show("��������Ϊ��");return "0";
                    }


                    jssfyweb.PathologyService  sfysev=new jssfyweb.PathologyService();
                    sfysev.Url = pathWEB;

                    if (debug == "1")
                        log.WriteMyLog("�������XMl��" + Request_xml);
                  
                    string   Response_xml="";
                    try
                    {
                    Response_xml= sfysev.GetPatientInfo(Request_xml);
                    }
                    catch(Exception e1)
                    {
                        MessageBox.Show("��ȡ��Ϣ�쳣��" + e1.Message);
                        log.WriteMyLog("��ȡ��Ϣ�쳣��" + sfysev.Url + "\r\n" + e1.Message);
                        return "0";
                    }
                    if (debug == "1")
                        log.WriteMyLog("����XMl��" + Response_xml);

                    if (Response_xml =="")
                    {
                        MessageBox.Show("����XML����Ϊ�գ���ȡ��Ϣʧ��"); return "0";
                    }
                    //-------------------------------

                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(Response_xml);
                        xmlok = xd.SelectSingleNode("/Response");
                     }
                    catch(Exception e2) 
                    {
                        if (debug == "1")
                            log.WriteMyLog("XML��������:" + e2.Message);
                            MessageBox.Show("XML��������:"+e2.Message); 
                        return "0";
                    }
                    
                    //-����xml----------------------------------------------------
                    PT_XML px = new PT_XML();
                    try
                    {

                            px.myDictionary["���˱��"] = xmlok["bl_no"].InnerText;

                            if (Sslbx.Contains("��Ʊ��"))
                                px.myDictionary["����ID"] = Ssbz.Trim();
                            else
                                px.myDictionary["����ID"] = "";
                        px.myDictionary["�������"] = "";

                        if (Sslbx.Contains("�����"))
                        {
                            px.myDictionary["�����"] = xmlok["patient_id"].InnerText;
                            px.myDictionary["סԺ��"] = "";
                        }
                        else
                        {
                            px.myDictionary["�����"] ="";
                            px.myDictionary["סԺ��"] = xmlok["inpatient_no"].InnerText;
                        }

                        px.myDictionary["����"] = xmlok["name"].InnerText;
                        px.myDictionary["����"] = xmlok["age"].InnerText+"��";
                        px.myDictionary["�Ա�"] = xmlok["sex"].InnerText;

                        if (px.myDictionary["�Ա�"].Trim() == "1")
                            px.myDictionary["�Ա�"] = "��";
                        if (px.myDictionary["�Ա�"].Trim() == "2")
                            px.myDictionary["�Ա�"] = "Ů";

                        px.myDictionary["����"] = xmlok["marry"].InnerText;
                        if (px.myDictionary["����"] == "1")
                            px.myDictionary["����"] = "�ѻ�";
                        if (px.myDictionary["����"] == "2")
                            px.myDictionary["����"] = "δ��";
                        if (px.myDictionary["����"] == "3")
                            px.myDictionary["����"] = "ɥż";
                        if (px.myDictionary["����"] == "4")
                            px.myDictionary["����"] = "����";
                        if (px.myDictionary["����"] == "5")
                            px.myDictionary["����"] = "����";


                        px.myDictionary["��ַ"] = xmlok["address"].InnerText;
                        px.myDictionary["�绰"] = xmlok["mobile"].InnerText;
                        try
                        {
                            px.myDictionary["����"] = xmlok["ward_name"].InnerText;
                            px.myDictionary["����"] = xmlok["bed_no"].InnerText;
                        }
                        catch
                        {
                        }
                        
                        px.myDictionary["���֤��"] = xmlok["social_no"].InnerText;
                        px.myDictionary["����"] = "";// xmlok["nation"].InnerText;
                        px.myDictionary["ְҵ"] = xmlok["occupation"].InnerText;

                        if (Sslbx.Contains("�����"))
                        {
                            px.myDictionary["�ͼ����"] = xmlok["unit_name"].InnerText;
                        }
                        else
                            px.myDictionary["�ͼ����"] = "";
                       

                        px.myDictionary["�ͼ�ҽ��"] = "";
                        px.myDictionary["�շ�"] ="";
                        px.myDictionary["�걾����"] = "";
                        px.myDictionary["�ͼ�ҽԺ"] = "��Ժ";
                        px.myDictionary["ҽ����Ŀ"] = "";
                        px.myDictionary["����1"] ="";
                        px.myDictionary["����2"] ="";

                        try
                        {
                            px.myDictionary["�ѱ�"] = xmlok["response_type"].InnerText;
                        }
                        catch
                        {
                            try
                            {
                                px.myDictionary["�ѱ�"] = xmlok["responce_type"].InnerText;
                            }
                            catch
                            {
                            }
                        }
                        if (px.myDictionary["�ѱ�"].Trim() == "9")
                            px.myDictionary["�ѱ�"] = "ҽ��";
                        if (px.myDictionary["�ѱ�"].Trim() == "1")
                            px.myDictionary["�ѱ�"] = "�Է�";

                        px.myDictionary["�������"] = xmlok["type"].InnerText;
                        if (px.myDictionary["�������"].Trim() == "1")
                            px.myDictionary["�������"] = "����";
                        if (px.myDictionary["�������"].Trim() == "2")
                            px.myDictionary["�������"] = "סԺ";

                        px.myDictionary["�ٴ���ʷ"] = xmlok["medical_record"].InnerText;
                        px.myDictionary["�ٴ����"] = xmlok["diag_name"].InnerText;


                        if (px.myDictionary["����"].Trim() == "")
                        {
                            MessageBox.Show("δ��ѯ����ؼ�¼����"); return "0";
                        }

                        string exep = "";
                        return px.rtn_XML(ref exep);
                    }
                    catch (Exception ee4)
                    {
                      
                        MessageBox.Show(ee4.Message); 
                        if (Debug == "1")
                        log.WriteMyLog("����xml�쳣��"+ee4.Message);
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    return "0";
                }
                }
            else
            {
                MessageBox.Show("Sslbx����Ϊ��" );
                return "0";
            }

             }
      }
}

