

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    /// <summary>
    /// �Ϻ��мζ�����ҽҽԺ  webservices  --����
    /// </summary>
    class shsjdqzyyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            string exp = "";
            string pathWEB = f.ReadString(Sslbx, "URL", ""); //��ȡsz.ini�����õ�webservicesurl

            if (Sslbx != "")
            {
                shsjdzyyyweb.FGPLService jdzyyy = new LGHISJKZGQ.shsjdzyyyweb.FGPLService();
                if (pathWEB.Trim() != "")
                    jdzyyy.Url = pathWEB;

              
                 if (Sslbx == "�������뵥��"||Sslbx == "סԺ���뵥��" || Sslbx == "סԺ��(����)" || Sslbx == "�����(����)")
                {
                    string GetApplyBill_XML = "";
                    if (Sslbx == "�������뵥��")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo></OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo></InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId>"+Ssbz.Trim()+"</RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>O</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                     if (Sslbx == "סԺ���뵥��")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo></OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo></InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId>"+Ssbz.Trim()+"</RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>I</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                    if (Sslbx == "סԺ��(����)")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo></OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo>"+Ssbz.Trim()+"</InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId></RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>I</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                    if (Sslbx == "�����(����)")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo>"+Ssbz.Trim()+"</OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo></InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId></RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>O</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                    if (GetApplyBill_XML.Trim()=="")
                    {
                        return "0";
                    }
                  //  shsjdqzyyyweb.FGPACSService jdzyyy = new PATHGETHISZGQ.shsjdqzyyyweb.FGPACSService();
                    string GetApplyBill_RTN = "";
                    try
                    {
                        GetApplyBill_RTN = jdzyyy.GetApplyBill(GetApplyBill_XML);
                    }
                    catch
                    {
                        MessageBox.Show("����GetApplyBill�����쳣�����������Ƿ�����!");
                        return "0";
                    }

                   if (GetApplyBill_RTN.Trim() == "")
                    {
                        MessageBox.Show("ȡ��Ϣ�쳣��������ϢΪ��");
                        return "0";
                    }
                 
                    //try
                    //{
                   
                        XmlNodeList xmlok_DATAs = null;
                        XmlDocument xd2 = new XmlDocument();

                        DataTable dt = new DataTable();
                        DataColumn dc0 = new DataColumn("���");
                        dt.Columns.Add(dc0);
                        DataColumn dc1 = new DataColumn("�����");
                        dt.Columns.Add(dc1);
                        DataColumn dc3 = new DataColumn("סԺ��");
                        dt.Columns.Add(dc3);
                        DataColumn dc4 = new DataColumn("��������");
                        dt.Columns.Add(dc4);
                        DataColumn dc5 = new DataColumn("��������");
                        dt.Columns.Add(dc5);
                        DataColumn dc6 = new DataColumn("����");
                        dt.Columns.Add(dc6);
                        DataColumn dc7 = new DataColumn("�Ա�");
                        dt.Columns.Add(dc7);
                        DataColumn dc8 = new DataColumn("����");
                        dt.Columns.Add(dc8);
                        DataColumn dc9 = new DataColumn("��������");
                        dt.Columns.Add(dc9);
                        DataColumn dc10 = new DataColumn("���֤��");
                        dt.Columns.Add(dc10);
                        DataColumn dc11 = new DataColumn("��ϵ�绰");
                        dt.Columns.Add(dc11);
                        DataColumn dc12 = new DataColumn("�Һű��");
                        dt.Columns.Add(dc12);
                        DataColumn dc13 = new DataColumn("��������");
                        dt.Columns.Add(dc13);
                        DataColumn dc14 = new DataColumn("��λ��");
                        dt.Columns.Add(dc14);

                        DataColumn dc15 = new DataColumn("���뵥���");
                        dt.Columns.Add(dc15);
                        DataColumn dc16 = new DataColumn("�����������");
                        dt.Columns.Add(dc16);
                        DataColumn dc17 = new DataColumn("����ҽ������");
                        dt.Columns.Add(dc17);
                        DataColumn dc18 = new DataColumn("���뵥״̬");
                        dt.Columns.Add(dc18);
                        DataColumn dc19 = new DataColumn("��ʷժҪ");
                        dt.Columns.Add(dc19);
                        DataColumn dc20 = new DataColumn("�ٴ����");
                        dt.Columns.Add(dc20);
                        DataColumn dc21 = new DataColumn("��Ŀ����");
                        dt.Columns.Add(dc21);
                        DataColumn dc22 = new DataColumn("��Ŀ����");
                        dt.Columns.Add(dc22);
                        DataColumn dc23 = new DataColumn("��鲿λ����");
                        dt.Columns.Add(dc23);
                        DataColumn dc24 = new DataColumn("����ʱ��");
                        dt.Columns.Add(dc24);


                        if (Debug == "1")
                            log.WriteMyLog(GetApplyBill_RTN);
                        try
                        {
                            xd2.LoadXml(GetApplyBill_RTN);
                            xmlok_DATAs = xd2.SelectNodes("/ApplyBillInfo/PatientInfos");
                            if (xmlok_DATAs.Count <= 0)
                            {
                                MessageBox.Show("δ�ҵ���Ӧ�ļ�¼"); return "0";
                            }

                            foreach (XmlNode xmlok_DATA2 in xmlok_DATAs)
                            {
                                XmlNode xmlok_DATA = xmlok_DATA2.FirstChild;
                                //���뵥��Ϣ
                                XmlNodeList xmlok_ApplyBills = xmlok_DATA["ApplyBills"].ChildNodes;

                                foreach (XmlNode xmlok_ApplyBill in xmlok_ApplyBills)
                                {

                                    DataRow dr = dt.NewRow();

                                    //���뵥���
                                    dr["���뵥���"] = xmlok_ApplyBill["RequestId"].InnerText;
                                    //�����������
                                    dr["�����������"] = xmlok_ApplyBill["ReqLocationName"].InnerText;
                                    //����ҽ������
                                    dr["����ҽ������"] = xmlok_ApplyBill["ReqDoctorName"].InnerText;
                                    //���뵥״̬
                                    dr["���뵥״̬"] = xmlok_ApplyBill["RequestStatus"].InnerText;
                                    //����ʱ��
                                    dr["����ʱ��"] = xmlok_ApplyBill["RequestDate"].InnerText;
                                    //��ʷժҪ
                                    dr["��ʷժҪ"] = xmlok_ApplyBill["MedicalHistory"].InnerText;
                                    //�ٴ����
                                    dr["�ٴ����"] = xmlok_ApplyBill["ClinicDiagnosis"].InnerText;

                                    string jcbb = "";
                                    string xmbm = "";
                                    string xmmc = "";
                                    XmlNodeList xmlok_ApplyItems = xmlok_ApplyBill["ApplyItems"].ChildNodes;
                                    //   MessageBox.Show(xmlok_ApplyItems.Count.ToString());
                                    foreach (XmlNode xmlok_ApplyItem in xmlok_ApplyItems)
                                    {
                                        //      MessageBox.Show(xmlok_ApplyItem["ItemCode"].InnerText);
                                        //��Ŀ���� 
                                        xmbm = xmlok_ApplyItem["ItemCode"].InnerText;
                                        //��Ŀ����
                                        xmmc = xmlok_ApplyItem["ItemName"].InnerText;
                                        //��鲿λ����
                                        jcbb = jcbb + xmlok_ApplyItem["CheckPointName"].InnerText + ",";
                                    }

                                    //��Ŀ����
                                    dr["��Ŀ����"] = xmbm;
                                    //��Ŀ����
                                    dr["��Ŀ����"] = xmmc;
                                    //��鲿λ����
                                    dr["��鲿λ����"] = jcbb;


                                    //�����
                                    dr["�����"] = xmlok_DATA["OutPatientNo"].InnerText;
                                    //סԺ��
                                    dr["סԺ��"] = xmlok_DATA["InPatientNo"].InnerText;
                                    //��������
                                    string  jzlx=xmlok_DATA["EncounterType"].InnerText;
                                    if(jzlx=="O") jzlx="����";
                                    else if(jzlx=="I") jzlx="סԺ";
                                    else  jzlx="";


                                    dr["��������"] =jzlx ;
                                    //��������
                                    string hzlx= xmlok_DATA["PatientType"].InnerText;
                                    if(hzlx=="99") hzlx="ȫ�����";
                                    else if(hzlx=="40") hzlx="��ũ��"; 
                                      else if(hzlx=="41") hzlx="��ֱ��"; 
                                      else if(hzlx=="42") hzlx="�ش󼲲�"; 
                                      else if(hzlx=="43") hzlx="��ũ���Է�"; 
                                      else if(hzlx=="10") hzlx="��ͨ����";
                                     else if(hzlx=="30") hzlx="��ҽ��"; 
                                    else  hzlx="";
                                    dr["��������"] =hzlx;
                                    //����
                                    dr["����"] = xmlok_DATA["PatientName"].InnerText;
                                    //�Ա�
                                    string xb= xmlok_DATA["GenderCode"].InnerText;
                                      if(xb=="F") xb="��";
                                       else if(xb=="M") xb="Ů";
                                      else  if (xb == "0") xb = "Ů";
                                      else if (xb == "1") xb = "��";
                                      else xb = "";
                                    dr["�Ա�"] =xb;

                                    //���� 
                                    dr["����"] = xmlok_DATA["Age"].InnerText;
                                    //���֤��
                                    dr["���֤��"] = xmlok_DATA["SSNumber"].InnerText;
                                    //��ϵ�绰
                                    dr["��ϵ�绰"] = xmlok_DATA["RelationshipTel"].InnerText;
                                    //��������
                                    dr["��������"] = xmlok_DATA["DateOfBirth"].InnerText;
                                    //�Һű��  0-1X  15  ���ﲡ�˱�����д
                                    dr["�Һű��"] = xmlok_DATA["RegCode"].InnerText;
                                    //�����������
                                    //dr["�����������"] = xmlok_DATA["LocationName"].InnerText;
                                    //��������
                                    dr["��������"] = xmlok_DATA["WardName"].InnerText;
                                    //�������
                                    // dr["�������"] = xmlok_DATA["DiagnosisName"].InnerText;
                                    //��λ��
                                    dr["��λ��"] = xmlok_DATA["BedNo"].InnerText;
                                    //ҽ������
                                    //dr["ҽ������"] = xmlok_DATA["DoctorName"].InnerText;
                                    dt.Rows.Add(dr);

                                }
                            }


                            for (int x = 0; x < dt.Rows.Count; x++)
                            {
                                dt.Rows[x][0] = x;
                            }

                        }
                        catch 
                        {
                            MessageBox.Show("��ѯ���ݣ�δ�ҵ���Ӧ�ļ�¼!\r\n" + GetApplyBill_RTN);
                            log.WriteMyLog("δ�ҵ���Ӧ�ļ�¼,����DATA�쳣��" + GetApplyBill_RTN);
                            
                            return "0";
                            //try
                            //{
                            //XmlNode xmlok_DATA_Info = null;
                            //XmlDocument xd_Info = new XmlDocument();
                            //xd_Info.LoadXml(GetApplyBill_RTN);
                            //xmlok_DATA_Info = xd_Info.SelectSingleNode("/ApplyBillInfo");
                            //if (xmlok_DATA_Info["PatientInfos"].InnerText != "")
                            //    MessageBox.Show("��ȡ��Ϣ����" + xmlok_DATA_Info["Info"].InnerText);
                            //return "0";
                            //}
                            //catch
                            //{
                            //MessageBox.Show("����DATA�쳣��" + xmlok_e.ToString());
                            //return "0";
                            //}
                        }
                        DataTable dt_brxx = new DataTable();  dt_brxx = dt;
                        int count = 0;
                        if (dt.Rows.Count > 1)
                        {
                           
                            FRM_SP_SELECT yc = new FRM_SP_SELECT(dt, -1, "���,���뵥���,����,סԺ��,�����,�Һű��,��������,��������,��λ��,�����������,����ҽ������,����ʱ��,�ٴ����,��Ŀ����");
                            yc.ShowDialog();
                            string rtn2 = yc.F_STRING[0];
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("δѡ�����뵥��");
                                return "0";
                            }
                            try
                            {
                                count = int.Parse(rtn2);
                            }
                            catch
                            {
                                MessageBox.Show("������ѡ�����뵥��");
                                return "0";
                            }
                        }
                        else
                        {
                           count = 0;
                        }
                  
                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_brxx.Rows[count]["�Һű��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_brxx.Rows[count]["���뵥���"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�����=" + (char)34 + dt_brxx.Rows[count]["�����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt_brxx.Rows[count]["סԺ��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_brxx.Rows[count]["����"].ToString().Trim() + (char)34 + " ";

                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt_brxx.Rows[count]["�Ա�"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            if (dt_brxx.Rows[count]["����"].ToString().Trim() == "")
                            {
                             
                                string CSRQ = dt_brxx.Rows[count]["��������"].ToString().Trim();

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
                                        //xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                        if (Month > 0)
                                            xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                        if (Month < 0)
                                            xml = xml + "����=" + (char)34 + (Year - 1) + "��" + (char)34 + " ";
                                        if (Month == 0)
                                        {
                                            if (day >= 0)
                                                xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                            else
                                                xml = xml + "����=" + (char)34 + (Year - 1) + "��" + (char)34 + " ";
                                        }
                                    }
                                    else
                                        if (Year == 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                            int m = day1 / 30;
                                            int d = day1 % 30;
                                            xml = xml + "����=" + (char)34 + m + "��" + d + "��" + (char)34 + " ";
                                        }
                                }
                            }
                            else
                                xml = xml + "����=" + (char)34 + dt_brxx.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                      
                        }
                        catch (Exception ee)
                        {
                          
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_brxx.Rows[count]["��ϵ�绰"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_brxx.Rows[count]["��������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_brxx.Rows[count]["��λ��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt_brxx.Rows[count]["���֤��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_brxx.Rows[count]["�����������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_brxx.Rows[count]["����ҽ������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            string bbmc = dt_brxx.Rows[count]["��鲿λ����"].ToString().Trim();
                            xml = xml + "�걾����=" + (char)34 + bbmc.Remove(bbmc.Length-1) + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "�걾����=" + (char)34 +"" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "ҽ����Ŀ=" + (char)34 + dt_brxx.Rows[count]["��Ŀ����"].ToString().Trim() + "^" + dt_brxx.Rows[count]["��Ŀ����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "ҽ����Ŀ=" + (char)34 +"" + (char)34 + " ";
                       
                        }
                            //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + dt_brxx.Rows[count]["��������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_brxx.Rows[count]["��������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_brxx.Rows[count]["��ʷժҪ"].ToString().Trim() + "]]></�ٴ���ʷ>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_brxx.Rows[count]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";
                     
                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;
                    }
                    catch (Exception e)
                    {
                        if (Debug == "1")
                            MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                 }





                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                 if (Sslbx == "�����" || Sslbx == "סԺ��")
                 {
                     string zyh = "";
                     string mzh = "";
                     string brlb = "";
                     if (Sslbx == "�����")
                     {
                         mzh = Ssbz;
                         brlb = "O";
                     }
                     if (Sslbx == "סԺ��")
                     {
                         zyh = Ssbz;
                         brlb = "I";
                     }

                     string GetPatientInfo_XML = "<PatientInfo><OutPatientNo>" + mzh + "</OutPatientNo><InPatientNo>" + zyh + "</InPatientNo><EncounterType>" + brlb + "</EncounterType>";
                        GetPatientInfo_XML=GetPatientInfo_XML+"<RegDate>"+DateTime.Now.AddDays(-7).ToString("yyyyMMddHHmmss")+"</RegDate><InDeptCode></InDeptCode><WardCode></WardCode></PatientInfo>";

                       
                        shsjdqzyyyweb2.FGPublicService jdqzyyy = new LGHISJKZGQ.shsjdqzyyyweb2.FGPublicService();
                    if (pathWEB.Trim() != "")
                        jdqzyyy.Url = pathWEB;


                    try
                    {
                        string GetPatientInfo_RTN = jdqzyyy.GetPatientInfo(GetPatientInfo_XML);
                        if (Debug == "1")
                            log.WriteMyLog(GetPatientInfo_RTN);


                        XmlDocument xd2 = new XmlDocument();
                        XmlNode xmlok_DATA = null;
                        try
                        {
                            xd2.LoadXml(GetPatientInfo_RTN);
                            xmlok_DATA = xd2.SelectSingleNode("/PatientInfos");
                        }
                        catch (Exception xmlok_e)
                        {
                            MessageBox.Show("����DATA�쳣��" + xmlok_e.ToString());
                            return xmlstr();
                        }
                        if (xmlok_DATA.InnerXml.Trim() == "")
                        {
                            MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
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
                                MessageBox.Show("תdataset�쳣��" + eee.ToString());
                            log.WriteMyLog("תdataset�쳣:" + eee);
                            return xmlstr();
                        }
                        int count = 0;
                        string isdtcx = f.ReadString(Sslbx, "isdtcx", "0"); //��ȡsz.ini�����õĶ����Ƿ���ʾѡ����

                        if (isdtcx == "1")
                        {
                            if (ds.Tables[0].Rows.Count > 1)
                            {
                                DataColumn dc0 = new DataColumn("���");
                                ds.Tables[0].Columns.Add(dc0);

                                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                                {
                                    ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                                }


                                FRM_YZ_SELECT yc = new FRM_YZ_SELECT(ds.Tables[0], "���,RegCode,PatientName,LocationName,DoctorName,EncounterDate,WardName,BedNo,DiagnosisName", "���,�Һű��,����,�������,ҽ������,����ʱ��,��������,��λ��,�ٴ����", "");
                                yc.ShowDialog();
                                string rtn2 = yc.F_XH;
                                if (rtn2.Trim() == "")
                                {
                                    MessageBox.Show("δѡ�����뵥��");
                                    return "0";
                                }
                                try
                                {
                                    count = int.Parse(rtn2);
                                }
                                catch
                                {
                                    MessageBox.Show("������ѡ�����뵥��");
                                    return "0";
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[count]["RegCode"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[count]["OutPatientNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + ds.Tables[0].Rows[count]["InPatientNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["PatientName"].ToString().Trim() + (char)34 + " ";

                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            //�Ա�
                            string xb = ds.Tables[0].Rows[count]["GenderCode"].ToString().Trim();
                            if (xb == "F") xb = "��";
                            else if (xb == "M") xb = "Ů";
                            else
                                if (xb == "0") xb = "Ů";
                                else if (xb == "1") xb = "��";
                                else  xb = "";
                            xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            if (ds.Tables[0].Rows[count]["Age"].ToString().Trim() == "")
                            {
                               
                                string CSRQ = ds.Tables[0].Rows[count]["DateOfBirth"].ToString().Trim();

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
                                        //xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                        if (Month > 0)
                                            xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                        if (Month < 0)
                                            xml = xml + "����=" + (char)34 + (Year - 1) + "��" + (char)34 + " ";
                                        if (Month == 0)
                                        {
                                            if (day >= 0)
                                                xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                            else
                                                xml = xml + "����=" + (char)34 + (Year - 1) + "��" + (char)34 + " ";
                                        }
                                    }
                                    else
                                        if (Year == 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                            int m = day1 / 30;
                                            int d = day1 % 30;
                                            xml = xml + "����=" + (char)34 + m + "��" + d + "��" + (char)34 + " ";
                                        }
                                }
                            }
                            else
                            {
                                string nl = ds.Tables[0].Rows[count]["Age"].ToString().Trim();
                                if (nl.Contains("��") || nl.Contains("��") || nl.Contains("��"))
                                    xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                                else
                                    xml = xml + "����=" + (char)34 + nl + "��" + (char)34 + " ";
                            }
                        }
                        catch (Exception ee)
                        {

                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[count]["RelationshipTel"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["WardName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["BedNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + ds.Tables[0].Rows[count]["SSNumber"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[count]["LocationName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[count]["DoctorName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            string hzlx = ds.Tables[0].Rows[count]["PatientType"].ToString().Trim();
                            if (hzlx == "99") hzlx = "ȫ�����";
                            else if (hzlx == "40") hzlx = "��ũ��";
                            else if (hzlx == "41") hzlx = "��ֱ��";
                            else if (hzlx == "42") hzlx = "�ش󼲲�";
                            else if (hzlx == "43") hzlx = "��ũ���Է�";
                            else if (hzlx == "10") hzlx = "��ͨ����";
                            else if (hzlx == "30") hzlx = "��ҽ��";
                            else hzlx = "";

                            xml = xml + "�ѱ�=" + (char)34 + hzlx + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            //��������
                            string jzlx = ds.Tables[0].Rows[count]["EncounterType"].ToString().Trim();
                            if (jzlx == "O") jzlx = "����";
                            else if (jzlx == "I") jzlx = "סԺ";
                            else jzlx = "";
                            xml = xml + "�������=" + (char)34 + jzlx + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + ds.Tables[0].Rows[count]["DiagnosisName"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;
                    }
                    catch(Exception  eee)
                    {
                        MessageBox.Show("ִ�в�ѯ�쳣��"+eee.ToString());
                    }
                 }
                 else
                 {
                     MessageBox.Show("�޴�" + Sslbx);
                     if (Debug == "1")
                         log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");

                     return "0";

                 }
            } return "0";


        }
        public static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "���˱��=" + (char)34 + (char)34 + " ";
            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + (char)34 + " ";
            xml = xml + "�����=" + (char)34 + (char)34 + " ";
            xml = xml + "סԺ��=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";

            xml = xml + "�Ա�=" + (char)34 + (char)34 + " ";

            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "��ַ=" + (char)34 + (char)34 + "   ";
            xml = xml + "�绰=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "���֤��=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "ְҵ=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽ��=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "�ͼ�ҽ��=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
            xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 +"��Ժ"+ (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";

            xml = xml + "�������=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + "]]></�ٴ����>";
            xml = xml + "</LOGENE>";
            return xml;
        }
    }
}
