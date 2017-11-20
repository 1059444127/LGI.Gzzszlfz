using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace LGHISJKZGQ
{
    class zjgsdyrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            string exp = "";
            string wsweb = f.ReadString(Sslbx, "wsweb", ""); //��ȡsz.ini�����õ�webservicesurl
            Debug = f.ReadString(Sslbx, "debug", "");

            if (Sslbx != "")
            {
                zjgsdyrmyyweb.PlatformCommonSvr zjg = new LGHISJKZGQ.zjgsdyrmyyweb.PlatformCommonSvr();
                if (wsweb != "")
                    zjg.Url = wsweb;
                string rtn_XML = "";

                #region ����
                if (Sslbx == "�����")
                {

                    string xmlcontext = "<Response><bill_id>" + Ssbz + "</bill_id></Response>";

                    if (Debug == "1")
                        log.WriteMyLog(xmlcontext);
                    try
                    {
                        rtn_XML = zjg.CurServer("LANGJIA", "OS001", xmlcontext);
                        if (Debug == "1")
                            log.WriteMyLog("���أ�" + rtn_XML);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("���ӷ������쳣��" + ee.Message);

                        log.WriteMyLog("�쳣��Ϣ��" + ee.ToString());
                        return "0";
                    }
                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("ȡ��Ϣ�쳣��������ϢΪ��");
                        return "0";
                    }

                    DataSet ds = new DataSet();
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        StringReader sr = new StringReader(rtn_XML);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("תdataset�쳣��" + eee.Message);
                        log.WriteMyLog("תdataset�쳣:" + eee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("δ�鵽��Ӧ�ļ�¼��");
                        return "0";
                    }

                    try
                    {
                        MessageBox.Show(ds.Tables[0].Rows[0]["Msg"].ToString());
                        return "0";
                    }
                    catch
                    {
                    }
                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";

                        xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[0]["CardID"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[0]["cureno"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["Name"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            string xb = ds.Tables[0].Rows[0]["Gender"].ToString().Trim();
                            if (xb == "1") xb = "��";
                            if (xb == "0") xb = "Ů";
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
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["age"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + ds.Tables[0].Rows[0]["address"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
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
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[0]["DeptName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[0]["doctorname"].ToString().Trim() + (char)34 + " ";
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
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        //----------------------------------------------------------
                        xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        //----------------------------------------------------------
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        if (Debug == "1")
                            log.WriteMyLog(xml);

                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }
                #endregion 

                #region סԺ��
                ///
                if (Sslbx == "סԺ��")
                {
                   
                       string  xmlcontext="<Response><hospitalid>" + Ssbz + "</hospitalid></Response>";
    
                    if (Debug == "1")
                        log.WriteMyLog(xmlcontext);
                    try
                    {
                        rtn_XML = zjg.CurServer("LANGJIA", "IS001", xmlcontext);
                        if (Debug == "1")
                            log.WriteMyLog("���أ�"+rtn_XML);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("���ӷ������쳣��" + ee.Message);

                        log.WriteMyLog("�쳣��Ϣ��" + ee.ToString());
                        return "0";
                    }
                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("ȡ��Ϣ�쳣��������ϢΪ��");
                        return "0";
                    }

                    DataSet ds = new DataSet();
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        StringReader sr = new StringReader(rtn_XML);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("תdataset�쳣��" + eee.Message);
                        log.WriteMyLog("תdataset�쳣:" + eee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("δ�鵽��Ӧ�ļ�¼��");
                        return "0";
                    }
      
                    try
                    {
                        MessageBox.Show(ds.Tables[0].Rows[0]["Msg"].ToString());
                        return "0";
                    }
                    catch
                    {
                    }
                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                       
                       xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[0]["his_id"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                       xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                       xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                       xml = xml + "סԺ��=" + (char)34 + ds.Tables[0].Rows[0]["hospitalid"].ToString().Trim() + (char)34 + " ";
                       xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["patname"].ToString().Trim() + (char)34 + " ";
                       
                        //----------------------------------------------------------
                        try
                        {
                            string xb = ds.Tables[0].Rows[0]["sex"].ToString().Trim();
                            if (xb == "M") xb = "��";
                            if (xb == "F") xb = "Ů";
                            xml = xml + "�Ա�=" + (char)34 +xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            int age = DateTime.Today.Year - DateTime.Parse(ds.Tables[0].Rows[0]["birth"].ToString().Trim()).Year;
                         
                            xml = xml + "����=" + (char)34 + age.ToString() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                       
                         xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                       
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + ds.Tables[0].Rows[0]["address"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[0]["telepone"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["wardname"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["Bed"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                       
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[0]["wardname"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[0]["maindoctor"].ToString().Trim() + (char)34 + " ";
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
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                            xml = xml + "�ѱ�=" + (char)34 +"" + (char)34 + " ";
                          
                            xml = xml + "�������=" + (char)34 +"סԺ" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                            xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        if (Debug == "1")
                            log.WriteMyLog(xml);

                        return xml;

                    }
                    catch (Exception e)
                    {

                        MessageBox.Show("��ȡ��Ϣ����" + e.Message);
                        log.WriteMyLog("��ȡ��Ϣ����" + e.Message);
                        return "0";
                    }
                 
               
                
                }
   #endregion
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    return "0";

                }
            } return "0";


        }
    }
}
