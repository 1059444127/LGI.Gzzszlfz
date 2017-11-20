using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;
using LGHISJKZGQ;
using System.Runtime.InteropServices;

namespace LGHISJKZGQ
{
    //��ɽ��ѧ��������ҽԺ  webservice
    class zsdxzlyy
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
         string exp = "";
         string pathWEB = f.ReadString(Sslbx, "wsurl", ""); //��ȡsz.ini�����õ�webservicesurl
         string sjdw = f.ReadString(Sslbx, "sjdw", "1"); //��ȡsz.ini�����õ�webservicesurl
         string mrks = f.ReadString(Sslbx, "mrks", ""); //��ȡsz.ini�����õ�mrks
         debug = f.ReadString(Sslbx, "debug", ""); //��ȡsz.ini�����õ�mrks
       
            if (Sslbx != "")
            {
                if (Sslbx == "������")
                {
                    int len = Ssbz.Trim().Length;
                    if (len < 10)
                    {
                        for (int z = 0; z < 10 - len; z++)
                        {
                            Ssbz = "0" + Ssbz;
                        }
                    }

                }
                string rtn_XML = "";

  
              
              if (Sslbx == "���ƿ�����" || Sslbx == "���ƿ�" || Sslbx == "������")
                {
                    string jzkh="";
                    if (Sslbx == "���ƿ�����" )
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
                                Sslbx="���ƿ�";
                            }
                            else
                            {  MessageBox.Show("������ʧ��");
                               UnlinkCard();
                               return "0";
                            }
                        }
                        else
                        { MessageBox.Show("���Ӷ�����ʧ��"); return "0"; }
                    }



                    string type = "1";
                    if (Sslbx == "���ƿ�")
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
                            MessageBox.Show("����webservice��ַ��" + zszl.Url);
                        rtn_XML = zszl.GetPatientInfoByCardorIcCard(RUCAN);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("����WebService�������쳣:"+ee.Message.ToString());
                        log.WriteMyLog("������" + Ssbz + ";�쳣��Ϣ��" + ee.Message.ToString());
                        return "0";
                    }

                    if (debug == "1")
                        log.WriteMyLog(RUCAN+"\r\n"+rtn_XML);

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
                        string ResultCode = xd2.SelectSingleNode("/Response/ResultCode").InnerText;
                        string ErrorMsg = xd2.SelectSingleNode("/Response/ErrorMsg").InnerText;
                        if (ResultCode != "0")
                        {
                          //  MessageBox.Show("��ȡ��Ϣʧ�ܣ�" + ErrorMsg.Trim());
                            //ȡ������Ϣ
                            return getbrxx(zszl.Url, RUCAN, sjdw, debug, mrks);
                        }
                        else
                            xmlok_DATA = xd2.SelectSingleNode("/Response/PatientInfoList");
                    }
                    catch (Exception xmlok_e)
                    {
                         return  "0";
                    }
                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        //MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
                        //return "0";
                        //ȡ������Ϣ
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
                        MessageBox.Show("תdataset�쳣��" + eee.Message.ToString());
                        log.WriteMyLog("תdataset�쳣:" + eee);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {

                    //    //�����뵥��Ϣ ��������Ϣ

                    //    MessageBox.Show("12");
                    //    rtn_XML = "";
                    //    try
                    //    {
                    //        rtn_XML = zszl.GetPatientBaseInfoByCardorIcCard(RUCAN);
                    //    }
                    //    catch (Exception ee)
                    //    {
                    //        MessageBox.Show("����WebService�������쳣:" + ee.Message.ToString());
                    //        log.WriteMyLog("������" + Ssbz + ";�쳣��Ϣ��" + ee.Message.ToString());
                    //        return "0";
                    //    }

                    //    //if (debug == "1")
                    //    //    log.WriteMyLog(RUCAN + "\r\n" + rtn_XML);

                    //    if (rtn_XML.Trim() == "")
                    //    {
                    //        MessageBox.Show("ȡ��Ϣ�쳣��������ϢΪ��");
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
                    //            MessageBox.Show("��ȡ��Ϣʧ�ܣ�" + ErrorMsg_1.Trim());
                    //            return "0";
                    //        }
                    //        else
                    //            xmlok_DATA_1 = xd2_1.SelectSingleNode("/Response/PatientInfoList");
                    //    }
                    //    catch (Exception xmlok_e2)
                    //    {
                    //        MessageBox.Show("����XML�쳣��" + xmlok_e2.ToString());
                    //        return "0";
                    //    }
                    //    if (xmlok_DATA_1.InnerXml.Trim() == "")
                    //    {
                    //        MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
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
                    //        MessageBox.Show("תdataset�쳣��" + eee.Message.ToString());
                    //        log.WriteMyLog("תdataset�쳣:" + eee);
                    //        return "0";
                    //    }



                    //    if (ds.Tables[0].Rows.Count < 1)
                    //    {
                    //        MessageBox.Show("δ�鵽��Ӧ�ļ�¼��"); return  "0";
                    //    }

                        return getbrxx(zszl.Url, RUCAN, sjdw, debug, mrks);
                    }
                    DataTable dt = new DataTable();
                  //  dt = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                       
                        string Columns = f.ReadString(Sslbx, "Columns", "PATIENTID,CARD_NO,IC_CARDNO,NAME");//��ʾ����Ŀ
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "PATIENTID,CARD_NO,IC_CARDNO,NAME");//��ʾ����Ŀ
                        string Col = f.ReadString(Sslbx, "RowFilter", ""); //ѡ����������Ŀ
                        string xsys = f.ReadString(Sslbx, "xsys", "1"); //ѡ����������Ŀ

                        FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0], -1, Columns, ColumnsName, Col, xsys);
                        yc.ShowDialog();
                        string string1 = yc.F_STRING[0];
                        string string2 = yc.F_STRING[1];
                        string string3 = yc.F_STRING[2];
                        string string4 = yc.F_STRING[3];

                        if (string1.Trim() == "" && string2.Trim() == "")
                        {
                            MessageBox.Show("δѡ���¼");
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
                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["PATIENTID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + dt.Rows[0]["IC_CARDNO"].ToString().Trim() + (char)34 + " ";
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
                        string zyh = "";
                        //string mzh = ""; 
                        //if (dt.Rows[0]["FLAG"].ToString().Trim()=="1")
                            zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();
                        //else
                        //    mzh = dt.Rows[0]["CARD_NO"].ToString().Trim();

                     
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
                            xml = xml + "סԺ��=" + (char)34 + zyh+ (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string xb = dt.Rows[0]["SEX_CODE"].ToString().Trim();
                            if (xb.Trim() == "F") xb = "Ů";
                            else if (xb.Trim() == "M") xb = "��";
                            else xb = "";
                            xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                         
                      
                           
                            string CSRQ = dt.Rows[0]["BIRTHDAY"].ToString().Trim().Substring(0,10);
                        
                            string datatime = DateTime.Today.Date.ToString();

                            if (CSRQ != "")
                            {
                                if (CSRQ.Contains("-"))
                                    CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                                int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                                if (Year >=1)
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
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string hy = dt.Rows[0]["MARI"].ToString().Trim();
                            if (hy == "M") hy = "�ѻ�";
                            else if (hy == "B") hy = "δ��";
                            else hy = "";
                            //else if (hy == "D") hy = "���";
                            //else if (hy == "W") hy = "ɥż";
                          
                            xml = xml + "����=" + (char)34 + hy + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                           // xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["HOME"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt.Rows[0]["HOME_TEL"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["BED_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["IDENNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["DEPT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["DOCT_NAME"].ToString().Trim() + (char)34 + " ";
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
                        if(sjdw.Trim()=="0")
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "" + (char)34 + " ";
                        else
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["ITEM_NAME"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 +""+ (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            if (dt.Rows[0]["FLAG"].ToString().Trim() == "1")
                                      xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
                                else
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
                            xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["DIAG_NAME"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                      

                        return xml;
                    }
                    catch (Exception e)
                    {
                     
                        MessageBox.Show("��ȡ��Ϣ����;"+e.ToString());
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                        log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");

                    return "0";

                }
            } return "0";


        }
        
        public static string getbrxx(string url, string RUCAN,string  sjdw, string debug,string mrks)
        {
         string exp = "";
               ZSZLWeb.Service zszl = new LGHISJKZGQ.ZSZLWeb.Service();
                    zszl.Url =url;
               string rtn_XML = "";
                    try
                    {
                        if (debug == "1")
                            MessageBox.Show("����webservice��ַ��" + zszl.Url);
                        rtn_XML = zszl.GetPatientBaseInfoByCardorIcCard(RUCAN);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("����WebService�������쳣:"+ee.Message.ToString());
                     
                        return "0";
                    }

                    if (debug == "1")
                        log.WriteMyLog(RUCAN+"\r\n"+rtn_XML);

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
                        string ResultCode = xd2.SelectSingleNode("/Response/ResultCode").InnerText;
                        string ErrorMsg = xd2.SelectSingleNode("/Response/ErrorMsg").InnerText;
                        if (ResultCode != "0")
                        {
                            MessageBox.Show("��ȡ��Ϣʧ�ܣ�HIS���أ�" + ErrorMsg.Trim());
                            return "0";
                        }
                        else
                            xmlok_DATA = xd2.SelectSingleNode("/Response/PatientInfoList");
                    }
                    catch (Exception xmlok_e)
                    {
                        MessageBox.Show("����XML�쳣��" + xmlok_e.ToString());
                        return "0";
                    }
                    if (xmlok_DATA.InnerXml.Trim() == "")
                    {
                        MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
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
                            MessageBox.Show("תdataset�쳣��" + eee.Message.ToString());
                        log.WriteMyLog("תdataset�쳣:" + eee);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count < 1)
                    {

                            MessageBox.Show("δ�鵽��Ӧ�ļ�¼��"); return  "0";
       
                    }
                    DataTable dt = new DataTable();
                   dt = ds.Tables[0];
                 
                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["PATIENTID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + dt.Rows[0]["IC_CARDNO"].ToString().Trim() + (char)34 + " ";
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
                      //  string zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();
                        string zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();

                        //if (dt.Rows[0]["FLAG"].ToString().Trim()=="1")
                        //    zyh = dt.Rows[0]["CARD_NO"].ToString().Trim();
                        //else
                        //    mzh = dt.Rows[0]["CARD_NO"].ToString().Trim();

                     
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
                            xml = xml + "סԺ��=" + (char)34 + zyh +(char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string xb = dt.Rows[0]["SEX_CODE"].ToString().Trim();
                            if (xb.Trim() == "F") xb = "Ů";
                            else if (xb.Trim() == "M") xb = "��";
                            else xb = "";
                            xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                         
                      
                          
                            string CSRQ = dt.Rows[0]["BIRTHDAY"].ToString().Trim().Substring(0,10);
                        
                            string datatime = DateTime.Today.Date.ToString();

                            if (CSRQ != "")
                            {
                                if (CSRQ.Contains("-"))
                                    CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                                int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                                if (Year >=1)
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
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            string hy = dt.Rows[0]["MARI"].ToString().Trim();
                            if (hy == "M") hy = "�ѻ�";
                            else if (hy == "B") hy = "δ��";
                            else hy = "";
                            //else if (hy == "D") hy = "���";
                            //else if (hy == "W") hy = "ɥż";
                          
                            xml = xml + "����=" + (char)34 + hy + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                           // xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["HOME"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt.Rows[0]["HOME_TEL"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["BED_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["IDENNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            if (mrks.Trim()!="")
                             xml = xml + "�ͼ����=" + (char)34 + mrks.Trim() + (char)34 + " ";
                            else
                            xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["DEPT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
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
                        if(sjdw.Trim()=="0")
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "" + (char)34 + " ";
                        else
                               xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 +""+ (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            if (dt.Rows[0]["FLAG"].ToString().Trim() == "1")
                                xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
                            else
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
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                      

                        return xml;
                    }
                    catch (Exception e)
                    {
                     
                        MessageBox.Show("��ȡ��Ϣ����;"+e.ToString());
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }
               
        

        }
    }

