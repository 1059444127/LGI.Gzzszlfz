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
                log.WriteMyLog("读XML失败：" + e2.Message.ToString());
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
                MessageBox.Show("提取基本信息失败：解析DATA异常：" + xmlok_e.Message);
                return ;
            }
            MessageBox.Show("1");
            string ResultCode = xmlok_DATA["respCode"].InnerText.Trim(); MessageBox.Show("1");
            string ErrorMsg = xmlok_DATA["respMessage"].InnerText.Trim();
            MessageBox.Show(ErrorMsg);
            if (ResultCode != "000000")
            {
                MessageBox.Show("提取基本信息失败：：" + ErrorMsg);
                return ;
            }
            MessageBox.Show("3");
            xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:patient", nsMgr);

            if (xmlok_DATA["name"].InnerText == "")
               {
                   MessageBox.Show("提取基本信息失败：info为空");
                   return;
               }
           

           
            MessageBox.Show("4");
            string exp = "";
            //-返回xml----------------------------------------------------
      
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "病人编号=" + (char)34 + xmlok_DATA["encounterNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {

                    xml = xml + "病人编号=" + (char)34 +"" +(char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "就诊ID=" + (char)34 +"" +(char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "住院号=" + (char)34 + xmlok_DATA["patientNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "姓名=" + (char)34 + xmlok_DATA["name"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "性别=" + (char)34 + xmlok_DATA["sexName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "年龄=" + (char)34 + ZGQClass.CsrqToAge(xmlok_DATA["birthday"].InnerText.Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "地址=" + (char)34 + xmlok_DATA["phone"].InnerText.Trim() + "^" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "电话=" + (char)34 + xmlok_DATA["address"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "病区=" + (char)34 + xmlok_DATA["wardName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "床号=" + (char)34 + xmlok_DATA["bedName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "身份证号=" + (char)34 + xmlok_DATA["idCardNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "民族=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                }

                //----------------------------------------------------------
                try
                {
                    xml = xml + "职业=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "送检科室=" + (char)34 + xmlok_DATA["departmentName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "送检医生=" + (char)34 + xmlok_DATA["chargeDoctorName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "送检医院=" + (char)34 + xmlok_DATA["hospitalName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医院=" + (char)34 + "广东省中医院" + (char)34 + " ";
                }

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
                    exp = exp + ee.ToString();
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    string brlb = xmlok_DATA["chargeDoctorName"].InnerText.Trim();
                    if (brlb == "ECTY.IP.0000")
                        brlb = "住院";
                    else if (brlb == "ECTY.OP.0000")
                        brlb = "门诊";
                    else if (brlb == "ECTY.HLC.0000")
                        brlb = "体检";
                    else if (brlb == "ECTY.AEU.0000")
                        brlb = "急诊";
                    else
                        brlb = "住院";

                    xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                }
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.ToString();
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
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
              
                MessageBox.Show("解析XML异常：" + xmlok_e.Message);
                return ;
            }
            if (xmlok_DATA["ResultCode"].InnerText.Trim() != "0")
            {
                MessageBox.Show("获取数据失败：" + xmlok_DATA["ErrorMsg"].InnerText.Trim());
                return ;
            }

            XmlNodeList xnl = xd2.SelectNodes("/Response/FindOrds/FindOrd");




            foreach (XmlNode xn2 in xnl)
            {

                //PatInfo 病人信息
                MessageBox.Show(xn2["PatInfo"]["Name"].InnerText);

                //AdmInfo/AdmRecord 科室信息
                MessageBox.Show(xn2["AdmInfo"]["AdmRecord"]["AdmDR"].InnerText);

                //AdmInfo/AdmRecord/HISOrders/Order收费信息 循环
                XmlNodeList xnl2 = xn2.SelectNodes("/AdmInfo/AdmRecord/HISOrders/Order");
              
                //AdmInfo/AdmRecord/HISOrders/Order收费信息 循环

            
                XmlDocument p1 = new XmlDocument();
                p1.LoadXml(xn2["AdmInfo"]["AdmRecord"]["HISOrders"].OuterXml);
                //医嘱信息
                XmlNodeList ppplist2 = p1.SelectNodes("/HISOrders/Order");

                foreach (XmlNode xn3 in ppplist2)
                {
                    MessageBox.Show(xn3["OrdName"].InnerText);
                }
   
            }
          
        }
    }
}