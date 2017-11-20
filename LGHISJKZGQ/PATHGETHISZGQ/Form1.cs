using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace LGHISJKZGQ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           LGHISJKZGQ.lgjkzgq  ss=new lgjkzgq();

           MessageBox.Show(ss.LGGetHISINFO(textBox1.Text, comboBox1.Text, textBox3.Text, "1", ""));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        private void button3_Click(object sender, EventArgs e)
        {

            StringReader xmlstr = null;
            XmlTextReader xmread = null;
            xmlstr = new StringReader(textBox4.Text);
            xmread = new XmlTextReader(xmlstr);
            XmlDocument readxml2 = new XmlDocument();
            try
            {
                readxml2.Load(xmread);
            }
            catch (Exception e2)
            {
                log.WriteMyLog("��XMLʧ�ܣ�" + e2.Message.ToString());
                return;
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "http://chas.hit.com/transport/integration/common/msg");

         
            XmlNode xmlok_DATA = null;
           // XmlDocument xd2 = new XmlDocument();
            try
            {

                xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:respHeader", nsMgr);
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("��ȡ������Ϣʧ�ܣ�����DATA�쳣��" + xmlok_e.Message);
                return ;
            }
            MessageBox.Show("1");
            string ResultCode = xmlok_DATA["respCode"].InnerText.Trim(); MessageBox.Show("1");
            string ErrorMsg = xmlok_DATA["respMessage"].InnerText.Trim();
            MessageBox.Show(ErrorMsg);
            if (ResultCode != "000000")
            {
                MessageBox.Show("��ȡ������Ϣʧ�ܣ���" + ErrorMsg);
                return ;
            }
            MessageBox.Show("3");
            xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:patient", nsMgr);

            if (xmlok_DATA["name"].InnerText == "")
               {
                   MessageBox.Show("��ȡ������Ϣʧ�ܣ�infoΪ��");
                   return;
               }
           

           
            MessageBox.Show("4");
            string exp = "";
            //-����xml----------------------------------------------------
      
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "���˱��=" + (char)34 + xmlok_DATA["encounterNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {

                    xml = xml + "���˱��=" + (char)34 +"" +(char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����ID=" + (char)34 +"" +(char)34 + " ";
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

                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "סԺ��=" + (char)34 + xmlok_DATA["patientNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["name"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "�Ա�=" + (char)34 + xmlok_DATA["sexName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + ZGQClass.CsrqToAge(xmlok_DATA["birthday"].InnerText.Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "��ַ=" + (char)34 + xmlok_DATA["phone"].InnerText.Trim() + "^" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�绰=" + (char)34 + xmlok_DATA["address"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["wardName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["bedName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "���֤��=" + (char)34 + xmlok_DATA["idCardNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }

                //----------------------------------------------------------
                try
                {
                    xml = xml + "ְҵ=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + xmlok_DATA["departmentName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + xmlok_DATA["chargeDoctorName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + xmlok_DATA["hospitalName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "�㶫ʡ��ҽԺ" + (char)34 + " ";
                }

                //----------------------------------------------------------
                xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    string brlb = xmlok_DATA["chargeDoctorName"].InnerText.Trim();
                    if (brlb == "ECTY.IP.0000")
                        brlb = "סԺ";
                    else if (brlb == "ECTY.OP.0000")
                        brlb = "����";
                    else if (brlb == "ECTY.HLC.0000")
                        brlb = "���";
                    else if (brlb == "ECTY.AEU.0000")
                        brlb = "����";
                    else
                        brlb = "סԺ";

                    xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
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
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                xml = xml + "</LOGENE>";


                MessageBox.Show(xml);
          return ;





       //   XmlNode xmlok_DATA = null;
          XmlDocument xd2 = new XmlDocument();
            try
            {
                xd2.LoadXml(textBox4.Text.Trim());
                xmlok_DATA = xd2.SelectSingleNode("/Response");
            }
            catch (Exception xmlok_e)
            {
              
                MessageBox.Show("����XML�쳣��" + xmlok_e.Message);
                return ;
            }
            if (xmlok_DATA["ResultCode"].InnerText.Trim() != "0")
            {
                MessageBox.Show("��ȡ����ʧ�ܣ�" + xmlok_DATA["ErrorMsg"].InnerText.Trim());
                return ;
            }

            XmlNodeList xnl = xd2.SelectNodes("/Response/FindOrds/FindOrd");




            foreach (XmlNode xn2 in xnl)
            {

                //PatInfo ������Ϣ
                MessageBox.Show(xn2["PatInfo"]["Name"].InnerText);

                //AdmInfo/AdmRecord ������Ϣ
                MessageBox.Show(xn2["AdmInfo"]["AdmRecord"]["AdmDR"].InnerText);

                //AdmInfo/AdmRecord/HISOrders/Order�շ���Ϣ ѭ��
                XmlNodeList xnl2 = xn2.SelectNodes("/AdmInfo/AdmRecord/HISOrders/Order");
              
                //AdmInfo/AdmRecord/HISOrders/Order�շ���Ϣ ѭ��

            
                XmlDocument p1 = new XmlDocument();
                p1.LoadXml(xn2["AdmInfo"]["AdmRecord"]["HISOrders"].OuterXml);
                //ҽ����Ϣ
                XmlNodeList ppplist2 = p1.SelectNodes("/HISOrders/Order");

                foreach (XmlNode xn3 in ppplist2)
                {
                    MessageBox.Show(xn3["OrdName"].InnerText);
                }
   
            }
          
        }
    }
}