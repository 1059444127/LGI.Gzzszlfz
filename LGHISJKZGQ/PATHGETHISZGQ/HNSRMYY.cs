using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace LGHISJKZGQ
{
    /// <summary>
    /// ����ʡ����ҽԺ  webservices
    /// </summary>
    class HNSRMYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
           
            string exp = "";
            string pathWEB = f.ReadString(Sslbx, "webservicesurl", ""); //��ȡsz.ini�����õ�webservicesurl
            Debug = f.ReadString(Sslbx, "debug", "");

            if (Sslbx != "")
            {
                LGHISJKZGQ.hnsrmyyWeb.TrasenWS rmyy = new LGHISJKZGQ.hnsrmyyWeb.TrasenWS();
                if (pathWEB != "")
                    rmyy.Url = pathWEB;
                string rtn_XML = "";

                //<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo>20140017175</TrmtNo><Fph></Fph></HEAD><DATA></DATA></PAYMENT>
                //<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo></TrmtNo><Fph>73400684</Fph></HEAD><DATA></DATA></PAYMENT>
                if (Sslbx == "����" || Sslbx == "��Ʊ��" || Sslbx == "�Һ����" || Sslbx == "���֤����" || Sslbx == "���ƿ�����")
                {

                    string ssbz="";
                    try
                    {
                      ssbz= Ssbz.Trim();
                    }
                    catch
                    {
                    }
                    string RUCAN ="";
                    if (Sslbx == "��Ʊ��")
                    {
                        if (ssbz.Length == 10)
                        {
                            ssbz = ssbz.Substring(0, 9);
                        }
                        RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo></TrmtNo><Fph>" + ssbz.Trim() + "</Fph><Ghxh></Ghxh></HEAD><DATA></DATA></PAYMENT>";
                    }
                    if (Sslbx == "�Һ����")
                        RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo></TrmtNo><Fph></Fph><Ghxh>" + ssbz.Trim() + "</Ghxh></HEAD><DATA></DATA></PAYMENT>";


                    if (Sslbx == "����" || Sslbx == "���ƿ�����" || Sslbx == "���֤����")
                    {
                        if (Sslbx == "���ƿ�����")
                        {
                            try
                            {
                                ssbz = ReadCardNo();
                                if (ssbz.Trim() == "") return "0";
                             }
                            catch (Exception e1)
                            {
                                MessageBox.Show(e1.Message); return "0";
                            }
                        }
                        if (Sslbx == "���֤����")
                        {
                            try
                            {
                                ssbz = getIDCardNo();
                                if (ssbz.Trim() == "") return "0";
                            }
                            catch (Exception e1)
                            {
                                MessageBox.Show(e1.Message);return "0";
                            }
                        }
                        RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><TrmtNo>" + ssbz.Trim() + "</TrmtNo><Fph></Fph><Ghxh></Ghxh></HEAD><DATA></DATA></PAYMENT>";
                    }

                    if (RUCAN == "")
                    {
                        MessageBox.Show("��Ч����");
                        return "0";
                    }
                    try
                    {

                        if (Debug == "1")
                            log.WriteMyLog(RUCAN);

                        rtn_XML = rmyy.GetMPatinfo(RUCAN);

                        if (Debug == "1")
                            log.WriteMyLog(rtn_XML);

                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("����wen�������쳣������������ϣ�" + ee.Message);
                        log.WriteMyLog("������" + Ssbz + ";�쳣��Ϣ��" + ee.Message);
                        return "0";
                    }
                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("ȡ��Ϣ�쳣��������ϢΪ��");
                        return "0";
                    }

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(rtn_XML);
                        xmlok_DATA = xd2.SelectSingleNode("/PAYMENT/DATA");
                    }
                    catch (Exception xmlok_e)
                    {
                        log.WriteMyLog(rtn_XML);
                        MessageBox.Show("����DATA�쳣��" + xmlok_e.Message);
                        return xmlstr();
                    }
                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        log.WriteMyLog(rtn_XML);
                        MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��(" + Sslbx + ":" + ssbz + ")");
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
                            MessageBox.Show("תdataset�쳣��" + eee.Message);
                        log.WriteMyLog("תdataset�쳣:" + eee.Message);
                        return xmlstr();
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("δ�鵽��Ӧ�ļ�¼��(" + Sslbx +":"+ssbz+ ")");
                        return xmlstr();
                    }
                    // DataTable dt = new DataTable();

                    int z = 0;
                    if (ds.Tables[0].Rows.Count > 1)
                    {

                        DataColumn dc0 = new DataColumn("���");
                        ds.Tables[0].Columns.Add(dc0);

                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                        }

                        Frm_hnsrmyy yc = new Frm_hnsrmyy(ds, "");
                        yc.ShowDialog();
                        //string GHRQ = yc.GHRQ;
                        //string FPH = yc.FPH;
                        //string GHXH = yc.GHXH;
                        //string CARDNO = yc.CARDNO;
                        string xh = yc.F_LAST_XH;

                        if (xh == "")
                        {
                            MessageBox.Show("δѡ���¼"); return "0";
                        }

                        z = int.Parse(xh);
                        //DataView view = new DataView();
                        //view.Table = ds.Tables[0];
                        //view.RowFilter = "GHRQ = '" + GHRQ + "' and FPH='" + FPH + "'";
                        // dt = ds.Tables[0];

                    }
                    //else
                    //{
                    //    dt = ds.Tables[0];
                    //}

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[z]["GHXH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + ds.Tables[0].Rows[z]["FPH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[z]["CARDNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + ds.Tables[0].Rows[z]["SEX"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["AGE"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["MARTIAL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + ds.Tables[0].Rows[z]["ADDREEST"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[z]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + ds.Tables[0].Rows[z]["SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["MINZU"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + ds.Tables[0].Rows[z]["ZHIYE"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[z]["KSMC"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[z]["YSMC"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
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
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + ds.Tables[0].Rows[z]["FEELB"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
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
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + ds.Tables[0].Rows[z]["ZDMC"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }

                ///
                ///
                ///
                if (Sslbx == "סԺ��")
                {
                    string zyh = Ssbz.Trim();
                    if (zyh.Length <= 6)
                        zyh = "00" + zyh;
                    string RUCAN = "<PAYMENT><HEAD><ResponseCode></ResponseCode><ResponseMsg></ResponseMsg><InpatientNo>" + zyh + "</InpatientNo></HEAD><DATA></DATA></PAYMENT>";
                    try
                    {

                        if (Debug == "1")
                            log.WriteMyLog(RUCAN);

                        rtn_XML = rmyy.GetZPatinfo(RUCAN);

                        if (Debug == "1")
                            log.WriteMyLog(rtn_XML);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("���ӷ������쳣������������ϣ�" + ee.Message);

                        log.WriteMyLog("������" + Ssbz + ";�쳣��Ϣ��" + ee.Message.ToString());
                        return "0";
                    }
                    if (rtn_XML.Trim() == "")
                    {
                        MessageBox.Show("ȡ��Ϣ�쳣��������ϢΪ��");
                        return "0";
                    }

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(rtn_XML);
                        xmlok_DATA = xd2.SelectSingleNode("/PAYMENT/DATA");

                    }
                    catch (Exception xmlok_e)
                    {
                        log.WriteMyLog(rtn_XML + "---" + xmlok_e.Message);
                        MessageBox.Show("����DATA�쳣��" + xmlok_e.Message);
                        return xmlstr();
                    }

                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
                        return xmlstr();
                    }

                    DataSet ds = new DataSet();
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                        XmlReader xr = new XmlTextReader(sr);
                        ds.ReadXml(xr);
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("תdataset�쳣��" + eee.Message);
                        log.WriteMyLog("תdataset�쳣:" + eee.Message);
                        return xmlstr();
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {
                        MessageBox.Show("δ�鵽��Ӧ�ļ�¼��");
                        return xmlstr();
                    }

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[0]["INPATIENTID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + ds.Tables[0].Rows[0]["TIMES"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + ds.Tables[0].Rows[0]["INPATIENTNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["HZXM"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + ds.Tables[0].Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string age = ds.Tables[0].Rows[0]["AGE"].ToString().Trim();
                            ////if (age.Contains("��"))
                            ////{
                            ////    age = age.Substring(0, age.IndexOf("��"))+"��";
                            ////}
                            xml = xml + "����=" + (char)34 + age + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["MARTIAL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + ds.Tables[0].Rows[0]["ADDREEST"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["INDEPT"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["BEDNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + ds.Tables[0].Rows[0]["SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["MINZU"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + ds.Tables[0].Rows[0]["ZHIYE"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[0]["SJDEPT"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[0]["YSMC"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
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
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + ds.Tables[0].Rows[0]["JSFSNAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[0]["WardOrReg"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
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
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + ds.Tables[0].Rows[0]["ZDMC"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;

                    }
                    catch (Exception e)
                    {

                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }

                ///
                ///���
                ///
                if (Sslbx == "����" || Sslbx == "��������")
                {
                    DataSet ds = new DataSet();
                    string sqlstr = "select djh as ���˱��, '���' as �������,'' as �ѱ�,'' as סԺ��,djh as �����,name as ����,sex as �Ա�,cast((datediff(year,sr,getdate())) as varchar(10))+'��'  as ����,' ' as ����,address as ��ַ,phone1 as �绰,'' AS ����,'' as ����,sfzhm as ���֤��,'' as ����,' ' as ְҵ,ksmc as �ͼ����,sqr AS �ͼ�ҽ��,'' as �ٴ����, ' ' AS �ٴ���ʷ,'' as �շ�,xmdm as ����ID,' ' as �������,' ' as �걾����,'��Ժ' AS �ͼ�ҽԺ,xmmc as ҽ����Ŀ from  v_bl_tj where  STUDYMETHOD='����' and djh= 'f_sbh'";
                    SqlConnection sqlcon = null;
                    string odbcstr = f.ReadString(Sslbx, "odbcsql", "Server=192.168.10.28;Database=TJ_Trasen;User Id=emr;Password=rmyyemr;"); //��ȡsz.ini�����õ�odbcsql
                    sqlstr = f.ReadString(Sslbx, "hissql", sqlstr);
                    sqlstr = sqlstr.Replace("f_sbh", Ssbz.Trim());
                    if (Debug == "1")
                        MessageBox.Show(sqlstr);
                    try
                    {
                        sqlcon = new SqlConnection(odbcstr);
                        SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, sqlcon);
                        sqlda.Fill(ds, "tjxx");
                        sqlda.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("���ϵͳ���ݿ����Ӵ��󣡣���");
                        return "0";
                    }
                    finally
                    {
                        if (sqlcon.State == ConnectionState.Open)
                            sqlcon.Close();
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("�������޼�¼��");
                        return "0";
                    }

                    //DataTable dt = new DataTable();
                    //if (ds.Tables[0].Rows.Count > 1)
                    //{
                    //    Frm_hnsrmyy yc = new Frm_hnsrmyy(ds, "���");
                    //    yc.ShowDialog();
                    //    string xmid = yc.FPH;
                    //    string tjh = yc.CARDNO;
                    //    if (xmid.Trim() == "")
                    //    {
                    //        MessageBox.Show("δѡ���¼");
                    //        return "0";
                    //    }
                    //    DataView view = new DataView();
                    //    view.Table = ds.Tables[0];
                    //    view.RowFilter = "����ID='" + xmid + "'  and  ���˱��='" + tjh + "'";
                    //    dt = view.ToTable();
                    //}
                    //else
                    //    dt = ds.Tables[0];

                    int z = 0;
                    if (ds.Tables[0].Rows.Count > 1)
                    {

                        DataColumn dc0 = new DataColumn("���");
                        ds.Tables[0].Columns.Add(dc0);

                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                        }

                        Frm_hnsrmyy yc = new Frm_hnsrmyy(ds, "");
                        yc.ShowDialog();
                        string xh = yc.F_LAST_XH;

                        if (xh == "")
                        {
                            MessageBox.Show("δѡ���¼"); return "0";
                        }

                        z = int.Parse(xh);
                    }

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[z]["���˱��"].ToString() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + ds.Tables[0].Rows[z]["����ID"].ToString() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[z]["�������"].ToString() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[z]["�����"].ToString() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + ds.Tables[0].Rows[z]["סԺ��"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["����"].ToString() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + ds.Tables[0].Rows[z]["�Ա�"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["����"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["����"].ToString() + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + ds.Tables[0].Rows[z]["��ַ"].ToString() + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[z]["�绰"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["����"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["����"].ToString() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + ds.Tables[0].Rows[z]["���֤��"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[z]["����"].ToString() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + ds.Tables[0].Rows[z]["ְҵ"].ToString() + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[z]["�ͼ����"].ToString() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[z]["�ͼ�ҽ��"].ToString() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + ds.Tables[0].Rows[z]["�շ�"].ToString() + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + ds.Tables[0].Rows[z]["�걾����"].ToString() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + ds.Tables[0].Rows[z]["�ͼ�ҽԺ"].ToString() + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + ds.Tables[0].Rows[z]["ҽ����Ŀ"].ToString() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + ds.Tables[0].Rows[z]["�ѱ�"].ToString() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[z]["�������"].ToString() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + ds.Tables[0].Rows[z]["�ٴ���ʷ"].ToString() + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + ds.Tables[0].Rows[z]["�ٴ����"].ToString() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
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
            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
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

        public static string getIDCardNo()
        {

            try
            {
                byte[] pucIIN1 = new byte[4];
                iPort = Syn_FindUSBReader();
                if (Syn_OpenPort(iPort) != 0)
                {
                    Syn_ClosePort(iPort);
                    MessageBox.Show("����������ʧ��"); return "";
                }
               
                if (Syn_SelectIDCard(iPort, ref  pucIIN1[0], 0) != 0)
                {
                    //Syn_ClosePort(iPort);
                    //MessageBox.Show("δ�ҵ���"); return;
                }

              
                byte[] pucIIN = new byte[4];
                byte[] pucSN = new byte[8];
                byte[] cardid = new byte[8];

                IDCardData CardMsg = new IDCardData();
                Syn_ClosePort(iPort);
                int nRet = Syn_OpenPort(iPort);
                if (nRet == 0)
                {
                    nRet = Syn_GetSAMStatus(iPort, 0);
                    nRet = Syn_StartFindIDCard(iPort, ref pucIIN[0], 0);
                    nRet = Syn_SelectIDCard(iPort, ref pucSN[0], 0);
                    if (Syn_ReadMsg(iPort, 0, ref  CardMsg) == 0)
                    {
                        Syn_ClosePort(iPort);
                        return CardMsg.IDCardNo;
                        //listBox1.Items.Add("������" + CardMsg.Name);
                        //listBox1.Items.Add("�Ա�" + CardMsg.Sex);
                        //listBox1.Items.Add("���壺" + CardMsg.Nation);
                        //listBox1.Items.Add("�������ڣ�" + CardMsg.Born);
                        //listBox1.Items.Add("סַ��" + CardMsg.Address);
                        //listBox1.Items.Add("���֤�ţ�" + CardMsg.IDCardNo);
                        //listBox1.Items.Add("��֤���أ�" + CardMsg.GrantDept);
                        //listBox1.Items.Add("��Ч�ڣ�" + CardMsg.UserLifeBegin + "-" + CardMsg.UserLifeEnd);
                        //listBox1.Items.Add("��Ƭ�ļ�����" + CardMsg.PhotoFileName);
                    }
                    else
                    {
                        Syn_ClosePort(iPort);
                        MessageBox.Show("������֤��Ϣ����!"); return "";
                    }
                }
                else
                {
                    Syn_ClosePort(iPort);
                    MessageBox.Show("�򿪶˿ڴ���"); return "";
                }
            }
            catch (Exception e)
            {
                Syn_ClosePort(iPort);
                MessageBox.Show("�����֤���쳣��"+e.Message); return "";
            }

        }

        //����
        private static string ReadCardNo()
        {
            string strCardNo = "";
            try
            {

                int nRet, nPort;
              
                byte KeyType = 0; // ��ԿA��֤
                byte[] pkey = new byte[6];
                byte[] data = new byte[8];


                byte[] pucIIN = new byte[4];
                iPort = Syn_FindUSBReader();
                if (Syn_OpenPort(iPort) != 0)
                {
                    MessageBox.Show("����������ʧ��"); return "";
                }
                Syn_ClosePort(iPort);

                if (Syn_SelectIDCard(iPort, ref  pucIIN[0], 0) != 0)
                {

                }

                #region ȡini����
                byte BlackNo = 1;
                byte DataNo = 0;
                int khcd = 11;

                int dataLength = 11;
                //Ĭ�ϲ���Ҫ
                string strToHex = "false";
                //Ĭ�ϲ���
                string khwsjl = "false";
                //Ĭ�ϲ���֤
                string strIsCheck = "false";
                //Ĭ��11
                byte Check_SID = 11;
                //Ĭ��0
                byte Check_BID = 0;
                //Ĭ�ϼ���
             
                #endregion

                pkey[0] = 255; pkey[1] = 255; pkey[2] = 255; pkey[3] = 255; pkey[4] = 255; pkey[5] = 255;
                nPort = iPort;

                if (Syn_OpenPort(nPort) == 0)
                {
                    nRet = Syn_USBHIDM1AuthenKey(nPort, KeyType, BlackNo, ref pkey[0]);
                    if (nRet == 0)
                    {
                        #region �Ƿ���Ҫ����
                        if (strIsCheck.Trim().ToLower() == "true" || strIsCheck.Trim().ToLower() == "��")
                        {
                            bool bCheckSucess = false;//�����Ƿ�ͨ��
                            int ret_check = Syn_USBHIDM1AuthenKey(nPort, KeyType, Check_SID, ref pkey[0]);// 
                            if (ret_check == 0)
                            {
                                byte[] check_data = new byte[8];
                                ret_check = Syn_USBHIDM1ReadBlock(nPort, (byte)(Check_SID * 4 + Check_BID), ref  check_data[0]);
                                if (ret_check == 0)
                                {
                                    string check = new System.Text.ASCIIEncoding().GetString(check_data).ToString();
                                    //if (check.Length > strCheck.Length) check = check.Substring(0, strCheck.Length);//�ضϳ���
                                    //if (check.Substring(0, strCheck.Length) == strCheck)
                                    //{
                                    //    bCheckSucess = true;

                                    //}
                                }
                            }
                            if (bCheckSucess == false)
                            {
                                MessageBox.Show("У�����");
                                return strCardNo;
                            }

                        }
                        #endregion
                        data = new byte[dataLength];
                        nRet = Syn_USBHIDM1ReadBlock(nPort, (byte)(BlackNo * 4 + DataNo), ref data[0]); ;  //������
                        if (nRet == 0)
                        {
                            string _kh = new System.Text.ASCIIEncoding().GetString(data).ToString(); //System.Text.ASCIIEncoding.ASCII.GetString(pblock);
                            //�Ƿ���Ҫת16����
                            if (strToHex.Trim().ToLower() == "true" || strToHex.Trim().ToLower() == "��") _kh = byteToHexStr(data);
                            //�Ƿ��Ǽ��ܿ����Ǿ���Ҫ����
                            //if(strIsCrry.Trim().ToLower()=="true" || strToHex.Trim().ToLower()=="��")  _kh=TrasenClasses.GeneralClasses.Crypto.Instance().UnCryp(_kh);
                            _kh = _kh.Trim().TrimEnd('\0');
                            if (_kh.Length > khcd) _kh = _kh.Substring(0, khcd); //ȡ��Ҫ�ĳ���
                            //���Ȳ��㡾���ų��ȡ���Ҫĩβ����
                            if (khwsjl.Trim().ToLower() == "true" || khwsjl.Trim().ToLower() == "��") _kh = _kh.PadLeft(khcd, '0');//����
                            strCardNo = _kh;
                        }
                        else
                        {
                            MessageBox.Show("��ȡ����ʧ��");
                        }
                    }
                    else
                    {
                        MessageBox.Show("�����ƿ�ʧ��");
                    }
                }
                else
                {
                    MessageBox.Show("�򿪶˿�ʧ��");
                }
                Syn_ClosePort(nPort);
            }
            catch (System.Exception err)
            {
                MessageBox.Show("�����쳣��" + err.Message);
            }
            return strCardNo;
        }
        /// <summary>
        /// �ֽ�����ת16�����ַ���
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct IDCardData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string Name; //����   
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
            public string Sex;   //�Ա�
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string Nation; //����
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string Born; //��������
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 72)]
            public string Address; //סַ
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string IDCardNo; //���֤��
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string GrantDept; //��֤����
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeBegin; // ��Ч��ʼ����
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public string UserLifeEnd;  // ��Ч��ֹ����
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 38)]
            public string reserved; // ����
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
            public string PhotoFileName; // ��Ƭ·��
        }
        /************************�˿���API *************************/

        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_OpenPort", CharSet = CharSet.Ansi)]
        public static extern int Syn_OpenPort(int iPortID);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ClosePort", CharSet = CharSet.Ansi)]
        public static extern int Syn_ClosePort(int iPortID);

        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindUSBReader", CharSet = CharSet.None)]
        private static extern int Syn_FindUSBReader();
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_FindReader", CharSet = CharSet.None)]
        private static extern int Syn_FindReader();

        /************************ SAM��API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMStatus", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMStatus(int iPortID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ResetSAM", CharSet = CharSet.Ansi)]
        public static extern int Syn_ResetSAM(int iPortID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMID", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMID(int iPortID, ref byte pucSAMID, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_GetSAMIDToStr", CharSet = CharSet.Ansi)]
        public static extern int Syn_GetSAMIDToStr(int iPortID, ref byte pcSAMID, int iIfOpen);

        /********************���֤����API *************************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_StartFindIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_StartFindIDCard(int iPortID, ref byte pucManaInfo, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SelectIDCard", CharSet = CharSet.Ansi)]
        public static extern int Syn_SelectIDCard(int iPortID, ref byte pucManaMsg, int iIfOpen);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_ReadMsg", CharSet = CharSet.Auto)]
        public static extern int Syn_ReadMsg(int iPortID, int iIfOpen, ref IDCardData pIDCardData);

        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDGetSecCardID", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDGetSecCardID(int iPortID, ref byte pucManaMsg);
        /********************������API *****************************/
        //[DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_SendSound", CharSet = CharSet.Ansi)]
        //public static extern int Syn_SendSound(int iCmdNo);
        //[DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_DelPhotoFile", CharSet = CharSet.Ansi)]
        //public static extern void Syn_DelPhotoFile();
        /***********************M1���������� (A16D-HF) ********************/
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1Reset", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1Reset(int iPort, ref uint pdwCardSN, ref byte pbSize);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1AuthenKey", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1AuthenKey(int iPort, byte KeyType, byte BlockNo, ref byte pKey);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1ReadBlock", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1ReadBlock(int iPort, byte BlockNo, ref byte pBlock);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1WriteBlock", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1WriteBlock(int iPort, byte BlockNo, ref byte pBlock);
        [DllImport("SynIDCardAPI.dll", EntryPoint = "Syn_USBHIDM1Halt", CharSet = CharSet.Ansi)]
        public static extern int Syn_USBHIDM1Halt(int iPort);
        static int iPort;
    }
}
