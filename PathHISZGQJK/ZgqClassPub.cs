
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Data.Odbc;
using LoadDll;

namespace ZgqClassPub
{
    public class ZgqClass
    {
        private static IniFiles f = new IniFiles(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\sz.ini");

       /// <summary>
       /// ȡsz�����ݿ�������
       /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
       /// <param name="Default">Ĭ��ֵ</param>
       /// <returns>�趨ֵ</returns>
        public static string GetSz(string Section, string Ident,string Default)
        {
            string T_szvalue = "";
            string szvalue = "";

            szvalue = f.ReadString(Section, Ident, "").Replace("\0", "").Trim();

            if (szvalue.Trim() == "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable DT_sz = new DataTable();
                    DT_sz = aa.GetDataTable("select top 1 F_SZZ from T_SZ where F_XL='" + Ident + "'  and F_DL='" + Section + "'", "sz");

                    if (DT_sz.Rows.Count <= 0)
                    {
                        return Default;
                    }
                    else
                    {
                        T_szvalue = DT_sz.Rows[0]["F_SZZ"].ToString().Replace("\0", "").Trim();
                            return T_szvalue;
                    }
                }
                catch (Exception e1)
                {
                    return Default;
                }
            }
            else
                return szvalue;

        }

        /// <summary>
        /// ����ۼ�
        /// </summary>
        /// <param name="blh">�����</param>
        /// <param name="yhmc">�û�����</param>
        /// <param name="DZ">����</param>
        /// <param name="NR">����</param>
        /// <param name="EXEMC">EXE����</param>
        /// <param name="CTMC">��������</param>
        public static void BGHJ(string blh, string yhmc, string DZ, string NR, string EXEMC, string CTMC)
        {
            try
            {
                 IPAddress addr = new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                aa.ExecuteSQL("insert into  T_BGHJ(F_BLH,F_RQ,F_CZY,F_WZ,F_DZ,F_NR,F_EXEMC,F_CTMC) values('" + blh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"
                    + yhmc + "','��������" + Dns.GetHostName().Trim() + ",IP ��ַ��" + addr.ToString() +"','"+DZ+"','" + NR + "','"+EXEMC+"','" +CTMC + "') ");
            }
            catch
            {
            }
        }
        
        /// <summary>
        /// ͨ���û����ƻ�ȡ�û����
        /// </summary>
        /// <param name="yhmc">�û�����</param>
        /// <returns>�û����,δ�ҵ�����0</returns>
        public static string GetYHBH(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "0";
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// �������ڼ�������
        /// </summary>
        /// <param name="csrq">��������</param>
        /// <returns>����</returns>
        public static string  CsrqToAge(string  csrq)
        {
            try
            {
                if (csrq.Trim() == "")
                    {
                        return "";
                    }
                    else
                    {
                        string CSRQ = csrq.Trim();
                        DateTime dtime = new DateTime();
                        try
                        {
                            if (CSRQ.Contains("-"))
                            {
                                TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                dtime = dtime.Add(tp);
                            }
                            else
                            {
                                if (CSRQ.Contains("/"))
                                {
                                    TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                    dtime = dtime.Add(tp);
                                }
                                else
                                {
                                    if (CSRQ.Length > 8)
                                        CSRQ = CSRQ.Substring(0, 8);
                                    TimeSpan tp = DateTime.Now - DateTime.Parse(string.Format("{0:0000-00-00}", Convert.ToInt32(CSRQ.ToString())));
                                    dtime = dtime.Add(tp);
                                }
                            }
                            int Year = dtime.Year - 1;
                            int Month = dtime.Month - 1;
                            int day = dtime.Day;

                            if (Year >= 2)
                                return  Year + "��" ;
                            else
                            {
                                if (Year == 0)
                                {
                                    if (Month<=0)
                                        return  day + "��";
                                    else
                                    return Month + "��" + day + "��";
                                }
                                else
                                    return +Year + "��" + Month + "��";
                            }
                        }
                        catch
                        {
                           return  ""; 
                        }
                    }
                
            }
            catch
            {
                return  "";
            }

        }

        /// <summary>
        /// ���֤�ż�������
        /// </summary>
        /// <param name="csrq">���֤��</param>
        /// <returns>����</returns>
        public static string  SfzhToAge(string  sfzh)
        {

            if (sfzh.Length < 16)
            {
                return "0";
            }
            string csrq = sfzh.Substring(6, 8);
          
            try
            {
                if (csrq.Trim() == "")
                    {
                        return "";
                    }
                    else
                    {
                        string CSRQ = csrq.Trim();
                        DateTime dtime = new DateTime();
                        try
                        {
                            if (CSRQ.Contains("-"))
                            {
                                TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                dtime = dtime.Add(tp);
                            }
                            else
                            {
                                if (CSRQ.Contains("/"))
                                {
                                    TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                    dtime = dtime.Add(tp);
                                }
                                else
                                {
                                    if (CSRQ.Length > 8)
                                        CSRQ = CSRQ.Substring(0, 8);
                                    TimeSpan tp = DateTime.Now - DateTime.Parse(string.Format("{0:0000-00-00}", Convert.ToInt32(CSRQ.ToString())));
                                    dtime = dtime.Add(tp);
                                }
                            }
                            int Year = dtime.Year - 1;
                            int Month = dtime.Month - 1;
                            int day = dtime.Day;

                            if (Year >= 2)
                                return  Year + "��" ;
                            else
                            {
                                if (Year == 0)
                                {
                                    if (Month<=0)
                                        return  day + "��";
                                    else
                                    return Month + "��" + day + "��";
                                }
                                else
                                    return +Year + "��" + Month + "��";
                            }
                        }
                        catch
                        {
                           return  ""; 
                        }
                    }
                
            }
            catch
            {
                return  "";
            }

        }

        /// <summary>
        /// LOGENE_XML��ʽmyDictionary
          /// </summary>
        public Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        /// <summary>
        /// ��ʼ�� myDictionary
        /// </summary>
        public void PT_XML()
        {
            myDictionary.Add("���˱��", "");
            myDictionary.Add("����ID", "");
            myDictionary.Add("�������", "");
            myDictionary.Add("�����", "");
            myDictionary.Add("סԺ��", "");
            myDictionary.Add("����", "");
            myDictionary.Add("�Ա�", "");
            myDictionary.Add("����", "");
            myDictionary.Add("����", "");
            myDictionary.Add("��ַ", "");
            myDictionary.Add("�绰", "");
            myDictionary.Add("����", "");
            myDictionary.Add("����", "");
            myDictionary.Add("���֤��", "");
            myDictionary.Add("����", "");
            myDictionary.Add("ְҵ", "");
            myDictionary.Add("�ͼ����", "");
            myDictionary.Add("�ͼ�ҽ��", "");
            myDictionary.Add("�շ�", "");
            myDictionary.Add("�걾����", "");
            myDictionary.Add("�ͼ�ҽԺ", "��Ժ");
            myDictionary.Add("ҽ����Ŀ", "");
            myDictionary.Add("����1", "");
            myDictionary.Add("����2", "");
            myDictionary.Add("�ѱ�", "");
            myDictionary.Add("�������", "");
            myDictionary.Add("�ٴ���ʷ", "");
            myDictionary.Add("�ٴ����", "");
            myDictionary.Add("��������", "");
        }

        /// <summary>
        /// ����  LOGENE_XML
        /// </summary>
        /// <param name="exep"></param>
        /// <returns></returns>
        public string rtn_XML(ref string exep)
        {
            try
            {
                exep = "";
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "���˱��=" + (char)34 + myDictionary["���˱��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ����˱���쳣\r\n";
                    xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����ID=" + (char)34 + myDictionary["����ID"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ�����ID�쳣\r\n";
                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�������=" + (char)34 + myDictionary["�������"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ���������쳣\r\n";
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�����=" + (char)34 + myDictionary["�����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ�������쳣\r\n";
                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "סԺ��=" + (char)34 + myDictionary["סԺ��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ�סԺ���쳣\r\n";
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�Ա�=" + (char)34 + myDictionary["�Ա�"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��Ա��쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                if (myDictionary["����"].Trim() == "" && myDictionary["��������"].Trim() != "")
                {
                    try
                    {
                        string nl = "";
                        string CSRQ = myDictionary["��������"].Trim();
                        if (CSRQ.Length > 10)
                            CSRQ = CSRQ.Substring(0, 10);

                        string datatime = DateTime.Today.Date.ToString();

                        if (CSRQ != "")
                        {
                            if (CSRQ.Contains("-"))
                                CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                            int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                            int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                            int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                            if (Year >= 3)
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

                                if (Year > 0 && Year < 3)
                                {
                                    if ((Year - 1) == 0)
                                    {
                                        if (Month <= 0)
                                        {
                                            if (day > 0)
                                                xml = xml + "����=" + (char)34 + (12 + Month) + "��" + day + "��" + (char)34 + " ";
                                            else
                                                xml = xml + "����=" + (char)34 + (12 + Month - 1) + "��" + (30 + day) + "��" + (char)34 + " ";
                                        }
                                        else
                                            xml = xml + "����=" + (char)34 + Year + "��" + (Month) + "��" + (char)34 + " ";
                                    }
                                    else
                                    {
                                        if (Month > 0)
                                            xml = xml + "����=" + (char)34 + Year + "��" + Month + "��" + (char)34 + " ";
                                        else
                                            xml = xml + "����=" + (char)34 + (Year - 1) + "��" + (12 + Month) + "��" + (char)34 + " ";

                                    }

                                }
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
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                    }
                }
                else
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";

                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "��ַ=" + (char)34 + myDictionary["��ַ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ���ַ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�绰=" + (char)34 + myDictionary["�绰"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�绰=" + (char)34 + " " + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��绰�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }

                /////////////////////////////////////////////////////////////////

                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "���֤��=" + (char)34 + myDictionary["���֤��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ����֤���쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "ְҵ=" + (char)34 + myDictionary["ְҵ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�ְҵ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + myDictionary["�ͼ����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ͼ�����쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + myDictionary["�ͼ�ҽ��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ͼ�ҽ���쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�շ�=" + (char)34 + myDictionary["�շ�"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��շ��쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�걾����=" + (char)34 + myDictionary["�걾����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��걾�����쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + myDictionary["�ͼ�ҽԺ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ͼ�ҽԺ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "ҽ����Ŀ=" + (char)34 + myDictionary["ҽ����Ŀ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�ҽ����Ŀ�쳣\r\n";
                }


                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����1=" + (char)34 + myDictionary["����1"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�����1�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����2=" + (char)34 + myDictionary["����2"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����2=" + (char)34 + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�����2�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ѱ�=" + (char)34 + myDictionary["�ѱ�"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ѱ��쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�������=" + (char)34 + myDictionary["�������"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ���������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "/>";

                try
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + myDictionary["�ٴ���ʷ"].Trim() + "]]></�ٴ���ʷ>";
                }
                catch
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                    exep = exep + "��ȡ�ֶΣ��ٴ���ʷ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "<�ٴ����><![CDATA[" + myDictionary["�ٴ����"].Trim() + "]]></�ٴ����>";
                }
                catch
                {
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                    exep = exep + "��ȡ�ֶΣ��ٴ�����쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "</LOGENE>";

                return xml;
            }
            catch
            {
                return "0";
            }

        }

        /// <summary>
        /// ��ʽ��XML
        /// </summary>
        /// <param name="sUnformattedXml"></param>
        /// <returns></returns>
        private string FormatXml(string sUnformattedXml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

    }

    public class ZgqPDFJPG
    {
        private static IniFiles f = new IniFiles(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\sz.ini");
        public enum Type { JPG, PDF };

        public bool CreatePDF(string blh, string bglx, string bgxh, ZgqPDFJPG.Type type1, ref string fileName, ref string errMsg)
        {
            try
            {
                bglx = bglx.ToLower();
                string filename = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    errMsg = ("�׳��쳣:" + ex.Message.ToString());
                    //�쳣
                    return false;
                }
                if (jcxx == null)
                {
                    errMsg = "�������ݿ����������⣡";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    errMsg = "������д���";
                    return false;
                }

                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                string bgzt = "";

                filename = "";
                try
                {
                    if (bglx.ToLower() == "bd")
                    {
                        dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        if (dt_bd == null)
                        {
                            errMsg = "�������ݿ�����������T_BDBG";
                            return false;
                        }
                        if (dt_bd.Rows.Count < 1)
                        {
                            errMsg = "������д���T_BDBG";
                            return false;
                        }
                        bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                        filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                    }
                    if (bglx.ToLower() == "bc")
                    {

                        dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");

                        if (dt_bc == null)
                        {
                            errMsg = "�������ݿ�����������T_BCBG";
                            return false;
                        }
                        if (dt_bc.Rows.Count < 1)
                        {
                            errMsg = "������д���T_BCBG";
                            return false;
                        }
                        bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                        filename = dt_bc.Rows[0]["F_BC_SPARE5"].ToString();
                    }
                    if (bglx.ToLower() == "cg")
                    {
                        bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                        filename = jcxx.Rows[0]["F_SPARE5"].ToString();
                    }
                }
                catch
                {
                    log.WriteMyLog("���治����:" + blh + bglx + bgxh);
                    errMsg = "���治����:" + blh + bglx + bgxh;
                    return false;
                }
                if (bgzt != "�����" && bgzt != "�ѷ���")
                {
                    errMsg = "����δ���";
                    return false;
                }
                if (filename.Trim() == "")
                {
                    errMsg = "���ڲ���Ϊ��";
                    return false;
                }
                if (bgzt == "�����" || bgzt == "�ѷ���")
                {
                    if (fileName == "")
                    {
                        try
                        {
                            if (type1.ToString().ToLower() == "pdf")
                                fileName = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
                            else
                                fileName = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";
                        }
                        catch
                        {
                            if (type1.ToString().ToLower() == "pdf")
                                fileName = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + filename.Trim() + ".pdf";
                            else
                                fileName = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + filename.Trim() + ".jpg";
                        }
                    }

                    string rptpath = ZgqClass.GetSz("ZGQJK", "rptpath", "").Replace("\0", "").Trim();

                    bool pdf1 = CreatePDF(blh, bglx, bgxh, type1, ref fileName, rptpath, ref errMsg);
                    if (!pdf1)
                        return false;

                    if (File.Exists(fileName))
                        return true;
                    else
                    {
                        errMsg = "δ�ҵ�PDF�ļ�" + fileName;
                        return false;
                    }
                }
                else
                {
                    errMsg = "����δ���";
                    return false;
                }
            }
            catch (Exception e4)
            {
                errMsg = "CreatePDF�����쳣��" + e4.Message;
                return false;
            }
        }
        public bool CreatePDF(string blh, string bglx, string bgxh, ZgqPDFJPG.Type type1, ref string filename, string rptpath, ref  string errMsg)
        {
           return    CreatePDF( blh,  bglx,  bgxh,  type1, ref  filename,  rptpath,"", ref   errMsg);
        }
        public bool CreatePDF(string blh, string bglx, string bgxh, ZgqPDFJPG.Type type1, ref string fileName, string rptPath, string localPath, ref  string errMsg,DateTime? dateStamp=null)
        {
            bool rtn = false;
            errMsg = "";

            string bgzt = "";
            if(!dateStamp.HasValue)
                dateStamp=DateTime.Now;
            string filename2 = blh + bglx + bgxh + "_" + dateStamp.Value.ToString("yyyyMMddHHmmss");

            try
            {
                string status = "";
                bglx = bglx.ToLower();
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                    if (jcxx == null)
                    {
                        errMsg = "[CreatePDF]"+blh+"����ʧ��:�������ݿ�����������T_jcxx";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    errMsg = "[CreatePDF]" + blh + "����ʧ��:��ѯ���ݿ�T_JCXX�쳣," + ex.Message.ToString();
                    return false;
                }

                //���c:\tempĿ¼
                if (localPath == "")
                    localPath = @"c:\temp";

                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }
                localPath = localPath + "\\" + blh;
                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }
                else
                {
                    try
                    {
                        System.IO.Directory.Delete(localPath, true);
                        System.IO.Directory.CreateDirectory(localPath);
                    }
                    catch
                    {
                    }
                }

                if (bglx == "cg")
                {
                    bgzt = jcxx.Rows[0]["F_bgzt"].ToString().Trim();
                    if (bgzt != "�����" && bgzt != "�ѷ���")
                    {
                        errMsg = "[CreatePDF]"+blh+"����ʧ��:���汨��δ��˻�δ����";
                        return false;
                    }
                }


                DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + blh + "' and F_sfdy='1'", "txlb");
                string txlbs = "";

                if (txlb == null)
                {
                    errMsg = "[CreatePDF]" + blh + "����ʧ��:�������ݿ�����������T_tx";
                    return false;
                }
                if (!downtx(blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), txlb, ref txlbs, localPath, ref errMsg))
                {
                    errMsg = "[CreatePDF]" + blh + "����ʧ��:����ͼƬʧ��," + errMsg;
                    return false;
                }


                string sbmp = "";
                string stxsm = "";
           
                string rptpath2 = "rpt";

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                    sbmp = sbmp + localPath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
                }
                string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + blh + "'";
                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                string bmppath = ZgqClass.GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                string yszmlb = ZgqClass.GetSz("All", "yszmlb", "f_shys");      
                if(bglx == "cg")
                {
                    bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    bgzt = jcxx.Rows[0]["F_bgzt"].ToString().Trim();
                    try
                    {
                        filename2 = blh + "_"+bglx + bgxh + "_" + dateStamp.Value.ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {}
           
                    if (ZgqClass.GetSz("rpt", "szqm", "0") == "1")
                    {
                        rptpath2 = "rpt-szqm";
                         stxsm = stxsm + " ,";
                        foreach (string ysname in yszmlb.Split(','))
                        {
                            if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") )
                            {
                                if (ysname.ToLower().Trim() == "f_shys")
                                {
                                    sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                                }

                                if (ysname.ToLower().Trim() == "f_bgys")
                                {
                                    foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {
                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }
                  }
                }
          
                 
                bool bcbddytx = false;
                if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
                    bcbddytx = true;

                if (bglx == "bd")
                {
                    DataTable BDBG = new DataTable();
                    try
                    {
                        BDBG = aa.GetDataTable("select * from T_BDBG  where F_blh='" + blh + "'  and F_BD_BGXH='" + bgxh + "'", "bcbg");
                        if (BDBG == null)
                        {
                            errMsg = "[CreatePDF]" + blh + "����ʧ��:��ѯ�����쳣T_BDBG";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:��ѯ�����쳣T_BDBG," + ex.Message.ToString();
                        return false;
                    }
                    string bd_bggs = "����";
                    if (BDBG.Rows.Count > 0)
                    {
                        string shrq = "";
                        try
                        {
                            shrq = BDBG.Rows[0]["F_BD_SPARE5"].ToString().Trim();
                        }
                        catch
                        {
                            shrq =  BDBG.Rows[0]["F_BD_BGRQ"].ToString().Trim();
                        }
                        bgzt = BDBG.Rows[0]["F_BD_BGZT"].ToString().Trim();
                        try
                        {
                            filename2 = blh + "_" + bglx + bgxh + "_" + dateStamp.Value.ToString("yyyyMMddHHmmss");
                        }
                        catch
                        { }
                        try
                        {
                            bd_bggs = BDBG.Rows[0]["F_BD_BGGS"].ToString().Trim();
                        }
                        catch
                        { }
                    }
                    else
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:��ѯ�����쳣T_BDBG";
                        return false;
                    }

                    try
                    {
                        if (ZgqClass.GetSz("rpt", "bcbgszqm", "0") == "1")
                        {
                            rptpath2 = "rpt-szqm";
                            stxsm = stxsm + " ,";
                            foreach (string ysname in yszmlb.Split(','))
                            {
                                int bgys2shys = -1;
                                if (ZgqClass.GetSz("rpt", "bgys2shys", "1") == "1")
                                    bgys2shys = 1;

                                if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys"))
                                {
                                    if (ysname.ToLower().Trim() == "f_shys")
                                    {
                                        sbmp = sbmp + bmppath + "\\" + BDBG.Rows[0]["F_bd_shys"].ToString().Trim() + ".bmp,";
                                        continue;
                                    }
                                    if (ysname.ToLower().Trim() == "f_bgys")
                                    {
                                        foreach (string name in BDBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                        {
                                            if (name.Trim() != "")
                                            {
                                                if (name == BDBG.Rows[0]["F_bc_shys"].ToString().Trim() && bgys2shys == 1)
                                                    continue;
                                                sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                            }
                                        }
                                    }
                                }

                                try
                                {
                                    if (ysname.ToLower().Trim() == "f_fzys")
                                    {
                                        foreach (string name in BDBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                        {
                                            if (name.Trim() != "")
                                                sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                        }
                                    }
                                }
                                catch
                                { }
                            }
                        }
                    }
                    catch (Exception ee_bc)
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:�����쳣," + ee_bc.Message;
                        return false;
                    }
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + blh + "' and F_BD_BGXH='" + bgxh + "'";

                       if (bd_bggs.Trim() == "")
                        bd_bggs = "����";
      
                        if (bcbddytx)
                            bggs = bd_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                        else
                            bggs = bd_bggs + ".frf";
                }

                if (bglx == "bc")
                {
                    DataTable BCBG = new DataTable();
                    try
                    {
                        BCBG = aa.GetDataTable("select * from T_BCBG  where F_blh='" + blh + "'  and F_BC_BGXH='" + bgxh + "'", "bcbg");
                        if (BCBG == null)
                        {
                            errMsg = "[CreatePDF]" + blh + "����ʧ��:��ѯ�����쳣T_BCBG";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��::���䱨���쳣T_BCBG," + ex.Message.ToString();
                        return false;
                    }
                    string bc_bggs = "����";
                        if (BCBG.Rows.Count > 0)
                        {
                            bgzt = BCBG.Rows[0]["F_BC_BGZT"].ToString().Trim();
                            try
                            {
                                filename2 = blh + "_" + bglx + bgxh + "_" + dateStamp.Value.ToString("yyyyMMddHHmmss");
                            }
                            catch
                            { }
                            try
                            {
                                bc_bggs = BCBG.Rows[0]["F_BC_BGGS"].ToString().Trim();
                            }
                            catch
                            { }
                        }
                        else
                        {
                            errMsg = "[CreatePDF]" + blh + "����ʧ��:��ѯ�����쳣T_BCBG";
                            return false;
                        }

                        try
                        {
                        if (ZgqClass.GetSz("rpt", "bcbgszqm", "0") == "1")
                        {
                            rptpath2 = "rpt-szqm";
                            stxsm = stxsm + " ,";
                            foreach (string ysname in yszmlb.Split(','))
                            {
                                int bgys2shys = -1;
                                if (ZgqClass.GetSz("rpt", "bgys2shys", "1") == "1")
                                    bgys2shys = 1;

                                if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys"))
                                {
                                    if (ysname.ToLower().Trim() == "f_shys")
                                    {
                                        sbmp = sbmp + bmppath + "\\" + BCBG.Rows[0]["F_bc_shys"].ToString().Trim() + ".bmp,";
                                        continue;
                                    }
                                    if (ysname.ToLower().Trim() == "f_bgys")
                                    {
                                        foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                        {
                                            if (name.Trim() != "")
                                            {
                                                if (name == BCBG.Rows[0]["F_bc_shys"].ToString().Trim() && bgys2shys == 1)
                                                    continue;
                                                sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                            }
                                        }
                                    }
                                }

                                if (ysname.ToLower().Trim() == "f_fzys")
                                {
                                    foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {
                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception  ee_bc)
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:��������PDF�쳣," + ee_bc.Message;
                        return false;
                    }
                        if (bc_bggs.Trim() == "")
                            bc_bggs = "����";

                        if (bcbddytx)
                            bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                        else
                            bggs = bc_bggs + ".frf";
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + blh + "' and F_BC_BGXH='" + bgxh + "'";
                }

                    if (bgzt != "�����" && bgzt != "�ѷ���")
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:"+bglx+bgxh+"����δ��˻�δ����";
                        return false;
                    }


                  foreach(string bmp in  sbmp.Split(','))
                    {
                        if (bmp.Trim() != "")
                        {
                            if (!File.Exists(bmp))
                            {
                                errMsg = "[CreatePDF]" + blh + "����ʧ��:δ�ҵ�ͼƬ," + bmp;
                            return false;
                            }
                        }
                    }
              
                string inibglj = ZgqClass.GetSz("dybg", "dybglj", "").Replace("\0", "");

                if (inibglj.Trim() == "")
                    inibglj = Application.StartupPath.ToString();

                string sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
              
                      if (rptPath!="")
                          sBGGSName = inibglj + "\\" + rptPath + "\\" + bggs;
                      else
                          sBGGSName = inibglj + "\\" + rptpath2 + "\\" + bggs;


                      string sJPGNAME = localPath + "\\" + fileName;
                      if (fileName == "")
                      {
                          if (type1.ToString().Trim().ToLower() == "jpg")
                          sJPGNAME = localPath + "\\" + filename2.Trim()+".JPG";
                          else
                              sJPGNAME = localPath + "\\" + filename2.Trim() + ".PDF";
                      }

                //�жϱ����ʽ�Ƿ����
                if (!File.Exists(sBGGSName))
                {
                    errMsg = "[CreatePDF]" + blh + "����ʧ��:�����ʽ������," + sBGGSName;
                    return false;
                }
                string debug = f.ReadString("savetohis", "debug2", "");
                for (int x = 0; x < 3; x++)
                {
                    prreport pr = new prreport();
                    try
                    {
                        if (type1.ToString().Trim().ToLower() == "jpg")
                        {
                            pr.printjpg(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, "");
                            if (debug == "1")
                                log.WriteMyLog("Createjpg:pr.print���");
                            fileName = sJPGNAME.Replace(".", "_1.");

                        }
                        else
                        {
                            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,"");
                            if (debug == "1")
                                log.WriteMyLog("CreatePDF:pr.print���");
                            fileName = sJPGNAME;
                        }

                    }
                    catch (Exception e3)
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:����prreport�쳣," + e3.Message; 
                        rtn = false;
                    }

                    if (!File.Exists(fileName))
                    {
                        errMsg = "[CreatePDF]" + blh + "����ʧ��:����δ�ҵ��ļ�" + fileName;
                        rtn = false;
                        continue;
                    }
                        errMsg = "";
                        rtn = true;
                        break;
                }
                return rtn;

            }
            catch (Exception e4)
            {
                errMsg = "[CreatePDF]" + blh + "����ʧ��:�쳣," + e4.Message;
                return false;
            }
        }

        public bool downtx(string blh, string txml, odbcdb aa, ref string txlbs, ref string localpath, ref string errMsg)
        {
            errMsg = "";

            if (localpath == "")
                localpath = @"c:\temp";

            localpath = localpath + "\\" + blh;
            try
            {
                //���c:\tempĿ¼
                if (!System.IO.Directory.Exists(localpath))
                    System.IO.Directory.CreateDirectory(localpath);
                else
                {
                    try
                    {
                        System.IO.Directory.Delete(localpath, true);
                        System.IO.Directory.CreateDirectory(@"c:\temp\" + blh);
                    }
                    catch (Exception e1)
                    {
                    }
                }
                //����FTP����
                string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
                string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                string ftppwd = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
                string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
                string txpath = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");

                FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
                //����Ŀ¼
                string gxml = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");

                DataTable dt_tx = aa.GetDataTable("select * from T_tx where F_blh='" + blh + "' and F_sfdy='1'", "txlb");
                string txm = "";

                if (ftps == "1")//FTP���ط�ʽ
                {
                    for (int i = 0; i < dt_tx.Rows.Count; i++)
                    {
                        txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();
                        string ftpstatus = "";
                        try
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                fw.Download(localpath, txml + "/" + txm, txm, out ftpstatus);
                                if (ftpstatus != "Error")
                                    break;
                            }
                            if (ftpstatus == "Error")
                            {
                                log.WriteMyLog("FTP����ͼ�����");
                                localpath = "";
                                return false;
                            }
                            else
                            {
                                if (f.ReadInteger("TX", "ZOOM", 0) == 1)
                                {
                                    int picx = f.ReadInteger("TX", "picx", 320);
                                    int picy = f.ReadInteger("TX", "picy", 240);
                                    try
                                    {
                                        prreport.txzoom(localpath + "\\" + txm, localpath + "\\" + txm, picx, picy);
                                    }
                                    catch
                                    { }
                                }
                                txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txm + "</Image>";
                            }
                        }
                        catch
                        {
                            log.WriteMyLog("FTP����ͼ�����");
                            return false;
                        }
                    }
                    return true;
                }
                else //�������ط�ʽ
                {
                    if (txpath == "")
                    {
                        log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                        return false;
                    }

                    for (int i = 0; i < dt_tx.Rows.Count; i++)
                    {
                        txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();
                        try
                        {
                            try
                            {
                                for (int x = 0; x < 3; x++)
                                {
                                    File.Copy(txpath + txml + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), true);
                                    if (File.Exists(localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim()))
                                        break;
                                }
                                txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                            }
                            catch
                            { }
                        }
                        catch
                        {
                            log.WriteMyLog("����Ŀ¼�����ڣ�");
                            localpath = "";
                            return false;
                        }

                    }
                    return true;
                }
            }
            catch (Exception e4)
            {

                errMsg = "����ͼ���쳣:" + e4.Message;
                return false;
            }

        }
        public bool downtx(string blh, string txml, DataTable dt_tx, ref string txlbs, string localpath, ref string errMsg)
        {

            if (localpath == "")
                localpath = @"c:\temp";

            //���c:\temp_srĿ¼
            if (!System.IO.Directory.Exists(localpath))
                System.IO.Directory.CreateDirectory(localpath);
            else
            {
                try
                {
                    System.IO.Directory.Delete(localpath, true);
                    System.IO.Directory.CreateDirectory(localpath);
                }
                catch
                {
                }
            }

            //����FTP����
            string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = ZgqClass.GetSz("ftp", "pwd", "4s3c2a1p").Replace("\0", "");
            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            //����Ŀ¼
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");
            string txm = "";

            if (ftps == "1")//FTP���ط�ʽ
            {
                for (int i = 0; i < dt_tx.Rows.Count; i++)
                {
                    txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();
                    string ftpstatus = "";
                    try
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            fw.Download(localpath, txml + "/" + txm, txm, out ftpstatus);
                            if (ftpstatus != "Error")
                                break;
                        }
                        if (ftpstatus == "Error")
                        {
                            log.WriteMyLog("FTP����ͼ�����");
                            return false;
                        }
                        else
                        {
                            if (f.ReadInteger("TX", "ZOOM", 0) == 1)
                            {
                                int picx = f.ReadInteger("TX", "picx", 320);
                                int picy = f.ReadInteger("TX", "picy", 240);
                                try
                                {
                                    prreport.txzoom(localpath + "\\" + txm, localpath + "\\" + txm, picx, picy);
                                }
                                catch
                                { }
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txm + "</Image>";
                        }
                    }
                    catch
                    {
                        log.WriteMyLog("FTP����ͼ�����");
                        return false;
                    }
                }
                return true;
            }
            else //�������ط�ʽ
            {
                if (txpath == "")
                {
                    log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                    return false;
                }

                for (int i = 0; i < dt_tx.Rows.Count; i++)
                {
                    txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();
                    try
                    {
                        try
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                File.Copy(txpath + txml + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim(), true);
                                if (File.Exists(localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim()))
                                    break;
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + dt_tx.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }
                        catch
                        { }
                    }
                    catch
                    {
                        log.WriteMyLog("����Ŀ¼�����ڣ�");
                        localpath = "";
                        return false;
                    }

                }
                return true;

            }
        }



        public bool UpPDF(string blh, string jpgpath, string ml, ref string errMsg, int lb)
        {
            try
            {
                string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
                //---�ϴ�jpg----------
                //----------------�ϴ���ftp---------------------
                string status = "";
                string ftps = string.Empty;
                string ftpServerIP = string.Empty;
                string ftpUserID = string.Empty; ;
                string ftpPassword = string.Empty;
                string ftplocal = string.Empty;
                string ftpRemotePath = string.Empty;
                string tjtxpath = ZgqClass.GetSz("savetohis", "toPDFPath", @"\\192.0.19.147\GMS");
                string debug = ZgqClass.GetSz("savetohis", "debug", "0");
                string txpath = ZgqClass.GetSz("txpath", "txpath", @"E:\pathimages");

                if (lb == 3)
                {
                    ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
                }
                if (lb == 1)
                {
                    ftps = "0";
                }
                if (lb == 2)
                {
                    ftps = "0"; tjtxpath = txpath;
                }
                if (lb == 4)
                {
                    ftps = ZgqClass.GetSz("ftpup", "ftp", "1").Replace("\0", "");
                    ftpServerIP = ZgqClass.GetSz("ftpup", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZgqClass.GetSz("ftpup", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZgqClass.GetSz("ftpup", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZgqClass.GetSz("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZgqClass.GetSz("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");

                }


                if (File.Exists(jpgpath))
                {
                    if (ftps == "1")
                    {
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                        string ftpURI = @"ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                        try
                        {

                            if (debug == "1")
                                log.WriteMyLog("���ftpĿ¼������");

                            //�յ�����Ŀ¼
                            if (ml.Trim() != "")
                            {
                                //�ж�Ŀ¼�Ƿ����
                                if (!fw.fileCheckExist(ftpURI, ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    fw.Makedir(ml, out stat);
                                    if (stat != "OK")
                                    {
                                        errMsg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                            //�����Ŀ¼
                            //�ж�Ŀ¼�Ƿ����
                            // MessageBox.Show("1--"+ftpURI);
                            if (!fw.fileCheckExist(ftpURI, blh))
                            {
                                //Ŀ¼�����ڣ�����
                                string stat = "";

                                fw.Makedir(ftpURI, blh, out stat);

                                if (stat != "OK")
                                {
                                    errMsg = "FTP����Ŀ¼�쳣";
                                    return false;
                                }
                            }
                            ftpURI = ftpURI + "/" + blh + "/";

                            //if (debug=="1")
                            // LGZGQClass.log.WriteMyLog("�ж�ftp���Ƿ���ڸ��ļ�");
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            //if (fw.fileCheckExist(ftpURI, jpgname))
                            //{
                            //    //ɾ��ftp�ϵ�jpg�ļ�
                            //    fw.fileDelete(ftpURI, jpgname).ToString();
                            //}
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            fw.Upload(jpgpath, ml + "/" + blh, out status, ref errMsg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                errMsg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + errMsg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                }
                                else
                                {
                                    errMsg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                errMsg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            errMsg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            errMsg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml + "\\" + blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml + "\\" + blh);
                                    }
                                    catch
                                    {
                                        errMsg = tjtxpath + "\\" + ml + "\\" + blh + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + ml + "\\" + blh;
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //ɾ�������ϵ�pdf�ļ�
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�

                            File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                            // Thread.Sleep(1000);
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                                status = "OK";
                            else
                            {
                                errMsg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            errMsg = "�ϴ��쳣:" + ee3.Message.ToString();
                            return false;
                        }
                    }

                    if (status == "OK")
                        return true;
                    else
                        return false;
                }
                else
                {
                    errMsg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                errMsg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }
        public bool UpPDF(string blh, string filePath, string ml, ref string errMsg, int lb, ref string ftpPath)
        {
            string pdfml = "";
            bool stat = true;
            for (int i = 0; i < 3; i++)
            {
                stat= UpPDF(blh, filePath, ml, ref  errMsg, lb, ref  pdfml, ref  ftpPath, "ftp");
                if (stat)
                    break;
            }
            return stat;
        }
        public bool UpPDF(string blh, string filePath, string ml, ref string errMsg, int lb, ref string pdfml, ref string ftpPath, string sz_section)
        {
            try
            {
                if (sz_section == "")
                    sz_section = "ftpup";

                ftpPath = "";
                errMsg = "";
                string jpgname = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                //---�ϴ�jpg----------
                //----------------�ϴ���ftp---------------------
                string status = "";
                string ftps = "";
                string ftpServerIP = "";
                string ftpUserID = "";
                string ftpPassword = "";
                string ftplocal = "";
                string ftpRemotePath = "";


                string debug = "";

                string txpath = ZgqClass.GetSz("txpath", "txpath", @"E:\pathimages");

                if (lb == 1)
                {
                    ftps = "0";
                    txpath = ZgqClass.GetSz("txpath", "txpath", @"E:\pathimages");
                }
                if (lb == 2)
                {
                    ftps = "0";
                    txpath = ZgqClass.GetSz("savetohis", "toPDFPath", "");
                }
                if (lb == 3 || lb == 0)
                {
                    ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZgqClass.GetSz("ftp", "ToPDFPath", @"pathimages/pdfbg").Replace("\0", "");
                }
                if (lb == 4)
                {
                    ftps = ZgqClass.GetSz("ftpup", "ftp", "").Replace("\0", "");
                    ftpServerIP = ZgqClass.GetSz("ftpup", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZgqClass.GetSz("ftpup", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZgqClass.GetSz("ftpup", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZgqClass.GetSz("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZgqClass.GetSz("ftpup", "ToPDFPath", "pathimages/pdfbg").Replace("\0", "");
                }
                if (lb == 5)
                {
                    ftps = "1";
                    ftpServerIP = ZgqClass.GetSz(sz_section, "ftpip", "").Replace("\0", "");
                    ftpUserID = ZgqClass.GetSz(sz_section, "ftpuser", "ftpuser").Replace("\0", "");
                    ftpPassword = ZgqClass.GetSz(sz_section, "ftppwd", "pacs").Replace("\0", "");
                    ftplocal = ZgqClass.GetSz(sz_section, "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZgqClass.GetSz(sz_section, "ftppdfpath", "pathimages/pdfbg").Replace("\0", "");
                }

                if (File.Exists(filePath))
                {
                    if (ftps == "1")
                    {
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                        string ftpURI = @"ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                        try
                        {

                            if (debug == "1")
                                log.WriteMyLog("���ftpĿ¼������");

                            //�յ�����Ŀ¼
                            if (ml.Trim() != "")
                            {
                                //�ж�Ŀ¼�Ƿ����
                                if (!fw.fileCheckExist(ftpURI, ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    fw.Makedir(ml, out stat);
                                    if (stat != "OK")
                                    {
                                        errMsg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                            //�����Ŀ¼
                            //�ж�Ŀ¼�Ƿ����
                            // MessageBox.Show("1--"+ftpURI);
                            if (blh.Trim() != "")
                            {
                                if (!fw.fileCheckExist(ftpURI, blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";

                                    fw.Makedir(ftpURI, blh, out stat);

                                    if (stat != "OK")
                                    {
                                        errMsg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI + blh + "/";

                                ml = ml + "/" + blh;
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            fw.Upload(filePath, ml, out status, ref errMsg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                errMsg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + errMsg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {
                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                    ftpPath = ftpURI + "" + jpgname;
                                    pdfml = "/" + ml + "/" + jpgname;
                                }
                                else
                                {
                                    errMsg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                errMsg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            errMsg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (txpath == "")
                        {
                            errMsg = "sz.ini��[" + sz_section + "]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(txpath + "\\" + ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(txpath + "\\" + ml);
                                    }
                                    catch
                                    {
                                        errMsg = txpath + "\\" + ml + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }

                                txpath = txpath + "\\" + ml;
                            }
                            if (blh.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(txpath + "\\" + blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(txpath + "\\" + blh);
                                    }
                                    catch
                                    {
                                        errMsg = txpath + "\\" + blh + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                ml = ml + "\\" + blh;
                                txpath = txpath + "\\" + blh;
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            if (File.Exists(txpath + "\\" + jpgname))
                            {
                                //ɾ�������ϵ�pdf�ļ�
                                File.Delete(txpath + "\\" + jpgname);
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�

                            File.Copy(filePath, txpath + "\\" + jpgname, true);
                            // Thread.Sleep(1000);
                            if (File.Exists(txpath + "\\" + jpgname))
                            {
                                status = "OK";
                                ftpPath = txpath + "\\" + jpgname;
                                pdfml = ml + "\\" + jpgname;
                            }
                            else
                            {
                                errMsg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            errMsg = "�ϴ��쳣:" + ee3.Message.ToString();
                            return false;
                        }
                    }

                    if (status == "OK")
                    {

                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    errMsg = "δ�ҵ��ļ�:" + filePath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                errMsg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }


        public bool DelPDFFile(string ml, string fileName, ref string errMsg)
        {
            string status = "";
            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
            string ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
            string tjtxpath = ZgqClass.GetSz("zgqjk", "PDFPath", "e:\\pathimages\\jpgbg");

            if (ftps == "1")
            {
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";
                try
                {
                    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                    if (fw.fileCheckExist(ftpURI, fileName))
                    {
                        //ɾ��ftp�ϵ�jpg�ļ�
                        fw.fileDelete(ftpURI, fileName).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    errMsg = "ɾ��ftp��PDF�쳣:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    errMsg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
                    return false;
                }
                try
                {
                    
                    if (ml != "")
                    {
                        tjtxpath=tjtxpath + "\\" + ml;
                    }
                    if (File.Exists(tjtxpath + "\\" + fileName))
                        File.Delete(tjtxpath + "\\" + fileName);
                
                    return true;
                }
                catch (Exception ee)
                {
                    errMsg = "ɾ���ļ��쳣:" + ee.Message.ToString();
                    return false;
                }
            }
        }
        public bool DelPDFFile(string filePath, ref string errMsg)
        {
            string status = "";
            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
            string ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
            string tjtxpath = ZgqClass.GetSz("zgqjk", "PDFPath", "e:\\pathimages\\jpgbg");

            if (ftps == "1")
            {
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                try
                {
                  //ɾ��ftp�ϵ�jpg�ļ�
                    fw.fileDelete(filePath).ToString();
                    return true;
                }
                catch (Exception eee)
                {
                    errMsg = "ɾ��ftp��PDF�쳣:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    errMsg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
                    return false;
                }
                try
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);

                    return true;
                }
                catch (Exception ee)
                {
                    errMsg = "ɾ���ļ��쳣:" + ee.Message.ToString();
                    return false;
                }
            }
        }

        public void DelTempFile(string blh)
        {
            DelTempFile(@"c:\temp\", blh);
        }
        public void DelTempFile(string filePath, string blh)
        {
            try
            {
                System.IO.Directory.Delete(filePath + blh, true);
            }
            catch
            {
            }
        }

        public void Base64StringToFile(string strbase64, string filename)
        {
            try
            {
                strbase64 = strbase64.Replace(' ', '+');
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(strbase64));
                FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] b = stream.ToArray();
                fs.Write(b, 0, b.Length);
                fs.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void FileToBase64String(ref string strbase64, string filename)
        {
            strbase64 = "";
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
                file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
                file.Close();
                strbase64 = Convert.ToBase64String(imgByte);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        public class Insert_Standard_ErrorLog
        {
            public static void Insert(string x, string y)
            {
                //  MessageBox.Show(y);
                log.WriteMyLog(y);
                Application.Exit();
            }
        }
    }

    public class ZgqFtpWeb
    {
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        /// <summary>
        /// ����FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP���ӵ�ַ</param>
        /// <param name="FtpRemotePath">ָ��FTP���ӳɹ���ĵ�ǰĿ¼, �����ָ����Ĭ��Ϊ��Ŀ¼</param>
        /// <param name="FtpUserID">�û���</param>
        /// <param name="FtpPassword">����</param>
        public ZgqFtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            if (FtpRemotePath != "")
            {
                ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            }
            else
            {
                ftpURI = "ftp://" + ftpServerIP + "/";
            }

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// 

        public bool FtpDownload(string ftpURL, string ml, string fileName, string localPath, string localName, ref string ErrMsg)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath + "\\" + localName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + ml + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message);
                ErrMsg = "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message;
                return false;
            }
        }

        public bool FtpDownload(string ftpPath, string localPath, ref string err_msg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                err_msg = "Download Error -->" + localPath + "-->" + ex.Message;
                return false;
            }
        }

        public static bool FtpDownload(string ftpUser, string ftpPwd, string ftpURL, string fileName, string localPath, string localName, ref string ErrMsg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath + "\\" + localName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message);
                ErrMsg = "Download Error --> " + localPath + "\\" + localName + "-->" + ex.Message;
                return false;
            }
        }

        public static bool FtpDownload(string ftpUser, string ftpPwd, string ftpPath, string localPath, ref string err_msg)
        {

            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localPath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                err_msg = "Download Error -->" + localPath + "-->" + ex.Message;
                return false;
            }
        }


        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="status"></param>
        public bool FtpMakedir(string ftpURI, string dirname, ref  string ErrMsg)
        {

            string uri = ftpURI + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                return false;
            }
        }

        public static bool FtpMakedir(string ftpUser, string ftpPwd, string ftpURI, string dirname, ref  string ErrMsg)
        {

            string uri = ftpURI + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                return true;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// �ϴ�
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="path"></param>
        /// <param name="status"></param>
        /// <param name="msg"></param>
        public bool FtpUpload(string filename, string ml, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + ml + "/" + fileInf.Name;
            if (ml == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public bool FtpUpload(string ftpURI, string ml, string filename, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + ml + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public static bool FtpUpload(string ftpUser, string ftpPwd, string ftpURI, string filename, string ml, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + ml + "/" + fileInf.Name;
            if (ml == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }

        public static bool FtpUpload(string ftpUser, string ftpPwd, string ftpURI, string filename, ref string ErrMsg)
        {

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                FileStream fs = fileInf.OpenRead();
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
                return true;
            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                ErrMsg = "Upload Error --> " + uri + "-->" + ex.Message;
                return false;
            }

        }



        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filePath"></param>
        public bool FtpDelete(string ftpURL, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpURL + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public bool FtpDelete(string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public static bool FtpDelete(string ftpUser, string ftpPwd, string ftpURL, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpURL + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public static bool FtpDelete(string ftpUser, string ftpPwd, string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }

        /// <summary>
        /// �ļ����ڼ��
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public bool FtpCheckFile(string ftpPath, string ftpName)
        {

            string url = ftpPath;
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {

                }
            }
            return success;

        }

        public static bool FtpCheckFile(string ftpUser, string ftpPwd, string ftpPath, string ftpName)
        {

            string url = ftpPath;
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUser, ftpPwd);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {

                }
            }
            return success;

        }
        public void Makedir(string filePath, string ftpUserID, string ftpPassword, string dirname, out string status)
        {
            status = "OK";
            string uri = filePath + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
            }
            catch (Exception ex)
            {
                log.WriteMyLog("Error --> " + uri + "-->" + ex.Message);
                status = "Error";
            }

        }

        public void FtpUpload(string ftpURI, string ftpUserID, string ftpPassword, string filename, out string status, ref string msg)
        {
            status = "OK";
            FileInfo fileInf = new FileInfo(filename);
            string uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;
            try
            {

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                reqFTP.UseBinary = true;

                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;

                byte[] buff = new byte[buffLength];

                int contentLen;

                FileStream fs = fileInf.OpenRead();


                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                log.WriteMyLog("Upload Error -->" + uri + "-->" + ex.Message);
                status = "Error";
                msg = "Upload Error --> " + uri + "-->" + ex.Message;
            }

        }

        public void Download(string ftpURI, string ftpUserID, string ftpPassword, string fileName, string localFilePath, string localName, out string status, ref string err_msg)
        {
            status = "OK";
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(localFilePath + "\\" + localName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {

                err_msg = "Download Error -->" + localFilePath + "\\" + localName + "-->" + ex.Message;
                status = "Error";
            }
        }

        /// <summary>
        /// �ļ����ڼ��
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public bool fileCheckExist(string ftpPath, string ftpUserID, string ftpPassword, string ftpName)
        {

            string url = ftpPath;

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {
                    //   LGZGQClass.log.WriteMyLog("�ر��������쳣");
                }
            }
            return success;

        }

        public class Insert_Standard_ErrorLog
        {
            public static void Insert(string x, string y)
            {
                //  MessageBox.Show(y);
                log.WriteMyLog(y);
                Application.Exit();
            }
        }
    }

    public class FtpWeb
    {
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        /// <summary>
        /// ����FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP���ӵ�ַ</param>
        /// <param name="FtpRemotePath">ָ��FTP���ӳɹ���ĵ�ǰĿ¼, �����ָ����Ĭ��Ϊ��Ŀ¼</param>
        /// <param name="FtpUserID">�û���</param>
        /// <param name="FtpPassword">����</param>
        public FtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            if (FtpRemotePath != "")
            {
                ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            }
            else
            {
                ftpURI = "ftp://" + ftpServerIP + "/";
            }

        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void Download(string filePath, string fileName, string localname, out string status)
        {
            status = "OK";
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + localname, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + filePath + "\\" + localname + "-->" + ex.Message);
                status = "Error:";
            }
        }

        public void Download(string filePath, string fileName, string localname, out string status, ref string err_msg)
        {
            status = "OK";
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + localname, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {

                err_msg = "Download Error -->" + filePath + "\\" + localname + "-->" + ex.Message;
                status = "Error";
            }
        }

        public void Makedir(string dirname, out string status)
        {
            Makedir("", dirname, out status);
        }

        public void Makedir(string filePath, string dirname, out string status)
        {
            status = "OK";
            string uri = ftpURI + dirname;
            if(filePath!="")
                uri = filePath + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                status = "Error";
            }

        }

        //public void Makedir(string dirname, out string status)
        //{
        //    status = "OK";



        //    string uri = ftpURI + dirname;

        //    FtpWebRequest reqFTP;



        //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

        //    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        //    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

        //    try
        //    {
        //        FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;

        //    }
        //    catch
        //    {
        //        //Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
        //        //status = "Error";
        //    }

        //}


        public void Upload(string filename, string path, out string status, ref string msg)
        {


            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + path + "/" + fileInf.Name;
            if (path == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                //try
                //{
                //    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                //   FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

                //    response.Close();
                //}
                //catch(Exception ex)
                //{
                //    MessageBox.Show(ex.ToString());
                //}


                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                reqFTP.UseBinary = true;

                reqFTP.ContentLength = fileInf.Length;

                int buffLength = 2048;

                byte[] buff = new byte[buffLength];

                int contentLen;

                FileStream fs = fileInf.OpenRead();


                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri + "-->" + ex.Message);
                status = "Error";
                msg = "Upload Error --> " + uri + "-->" + ex.Message;
            }

        }

        public void Upload(string filename, string path, out string status)
        {
            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + path + "/" + fileInf.Name;
            if (path == "")
                uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            FileStream fs = fileInf.OpenRead();

            try
            {

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }
            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + uri + "-->" + ex.Message);
                status = "Error";

            }

        }

        public void Upload(string filename, string name, string blh, out string status)
        {
            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + blh + "/" + name;
            if (blh == "")
                uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            //try
            //{
            //    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

            //   FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

            //    response.Close();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}


            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            FileStream fs = fileInf.OpenRead();

            try
            {

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
                status = "Error";
            }

        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filePath"></param>
        public bool fileDelete(string ftpPath, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public bool fileDelete(string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        /// <summary>
        /// �ļ����ڼ��
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public bool fileCheckExist(string ftpPath, string ftpName)
        {

            string url = ftpPath;

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception ee)
            {
                log.WriteMyLog(ee.Message);
                success = false;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    }
                }
                catch
                {
                    //   LGZGQClass.log.WriteMyLog("�ر��������쳣");
                }
            }
            return success;

        }



        public class Insert_Standard_ErrorLog
        {
            public static void Insert(string x, string y)
            {
                //  MessageBox.Show(y);
                log.WriteMyLog(y);
                Application.Exit();
            }
        }
    }

    public class FTPHelper
    {
        private string ftpUser;
        private string ftpPassWord;
        public FTPHelper(string ftpUser, string ftpPassWord)
        {
            this.ftpUser = ftpUser;
            this.ftpPassWord = ftpPassWord;
        }
        /// <summary>
        /// �ϴ��ļ���Ftp������
        /// </summary>
        /// <param name="uri">���ϴ����ļ�����Ϊftp�������ļ���uri,��ftp://192.168.1.104/pathiamges/123.jpg</param>
        /// <param name="UpLoadFile">Ҫ�ϴ��ı��ص��ļ�·������C:\temp\123.jpg</param>

        public void UpLoadFile(string UpLoadUri, string UpLoadFile)
        {

            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;
            try
            {
                Uri uri = new Uri(UpLoadUri);
                FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(uri);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
                uploadRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);
                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(UpLoadFile, FileMode.Open);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }
                requestStream.Close();
                uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("FTP�ϴ��ļ��쳣:" + ex.Message.ToString());
            }
            finally
            {
                if (uploadResponse != null)
                    uploadResponse.Close();
                if (fileStream != null)
                    fileStream.Close();
                if (requestStream != null)
                    requestStream.Close();
            }
        }
        public bool UpLoadFile(string UpLoadUri, string UpLoadFile, ref string ErrMsg)
        {
            ErrMsg = "";
            bool UpRtn = false;
            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;
            try
            {
                Uri uri = new Uri(UpLoadUri);
                FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(uri);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
                uploadRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);
                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(UpLoadFile, FileMode.Open);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }
                requestStream.Close();
                uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
                UpRtn = true;
            }
            catch (Exception ex)
            {
                ErrMsg = "FTP�ϴ��ļ��쳣:" + ex.Message.ToString();
                UpRtn = false;
            }
            finally
            {
                if (uploadResponse != null)
                    uploadResponse.Close();
                if (fileStream != null)
                    fileStream.Close();
                if (requestStream != null)
                    requestStream.Close();

            }
            return UpRtn;
        }
        public bool UpLoadFile(string UpLoadUri, string UpLoadFile, string FtpUser, string FtpPassWord, ref string ErrMsg)
        {
            ErrMsg = "";
            bool UpRtn = false;
            Stream requestStream = null;
            FileStream fileStream = null;
            FtpWebResponse uploadResponse = null;
            try
            {
                Uri uri = new Uri(UpLoadUri);
                FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(uri);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
                uploadRequest.Credentials = new NetworkCredential(FtpUser, FtpPassWord);
                requestStream = uploadRequest.GetRequestStream();
                fileStream = File.Open(UpLoadFile, FileMode.Open);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    requestStream.Write(buffer, 0, bytesRead);
                }
                requestStream.Close();
                uploadResponse = (FtpWebResponse)uploadRequest.GetResponse();
                UpRtn = true;
            }
            catch (Exception ex)
            {
                ErrMsg = "FTP�ϴ��ļ��쳣:" + ex.Message.ToString();
                UpRtn = false;
            }
            finally
            {
                if (uploadResponse != null)
                    uploadResponse.Close();
                if (fileStream != null)
                    fileStream.Close();
                if (requestStream != null)
                    requestStream.Close();

            }
            return UpRtn;
        }
        /// <summary>
        /// ��ftp�����ļ������ط�����
        /// </summary>
        /// <param name="downloadUrl">Ҫ���ص�ftp�ļ�·������ftp://192.168.1.104/pathimages/123.jpg</param>
        /// <param name="saveFileUrl">���ر����ļ���·������(@"C:\temp\123.jpg"</param>
        public void DownLoadFile(string DownLoadUrl, string SaveFileUrl)
        {
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(DownLoadUrl);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                downloadRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);
                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();
                fileStream = File.Create(SaveFileUrl);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FTP�����ļ��쳣:" + ex.Message.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        public bool DownLoadFile(string DownLoadUrl, string SaveFileUrl, ref string ErrMsg)
        {
            ErrMsg = "";
            bool UpRtn = false;
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(DownLoadUrl);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                downloadRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);
                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();
                fileStream = File.Create(SaveFileUrl);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
                UpRtn = true;
            }
            catch (Exception ex)
            {
                ErrMsg = "�����ļ��쳣:" + ex.Message.ToString();
                UpRtn = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }

            }
            return UpRtn;
        }
        public bool DownLoadFile(string DownLoadUrl, string SaveFileUrl, string FtpUser, string FtpPassWord, ref string ErrMsg)
        {
            ErrMsg = "";
            bool UpRtn = false;
            Stream responseStream = null;
            FileStream fileStream = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(DownLoadUrl);
                downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                downloadRequest.Credentials = new NetworkCredential(FtpUser, FtpPassWord);
                FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();
                responseStream = downloadResponse.GetResponseStream();
                fileStream = File.Create(SaveFileUrl);
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = responseStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                        break;
                    fileStream.Write(buffer, 0, bytesRead);
                }
                UpRtn = true;
            }
            catch (Exception ex)
            {
                ErrMsg = "�����ļ��쳣:" + ex.Message.ToString();
                UpRtn = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (responseStream != null)
                {
                    responseStream.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return UpRtn;
        }


        /// <summary>
        /// ��FTP�����ļ������ط�����,֧�ֶϵ�����
        /// </summary>
        /// <param name="ftpUri">ftp�ļ�·������"ftp://localhost/test.txt"</param>
        /// <param name="saveFile">�����ļ���·������C:\\test.txt</param>
        public void BreakPointDownLoadFile(string ftpUri, string saveFile)
        {
            System.IO.FileStream fs = null;
            System.Net.FtpWebResponse ftpRes = null;
            System.IO.Stream resStrm = null;
            try
            {
                //�����ļ���URI
                Uri u = new Uri(ftpUri);
                //�趨�����ļ��ı���·��
                string downFile = saveFile;
                //FtpWebRequest������
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //�趨�û���������
                ftpReq.Credentials = new System.Net.NetworkCredential(ftpUser, ftpPassWord);
                //Method��WebRequestMethods.Ftp.DownloadFile("RETR")�趨
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //Ҫ�����˺�ر�����
                ftpReq.KeepAlive = false;
                //ʹ��ASCII��ʽ����
                ftpReq.UseBinary = false;
                //�趨PASSIVE��ʽ��Ч
                ftpReq.UsePassive = false;

                //�ж��Ƿ��������
                //����д�������ļ���FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //��������
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //һ������
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //ȡ��FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //Ϊ�������ļ�ȡ��Stream
                resStrm = ftpRes.GetResponseStream();
                //д�����ص�����
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readSize = resStrm.Read(buffer, 0, buffer.Length);
                    if (readSize == 0)
                        break;
                    fs.Write(buffer, 0, readSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FTP�����ļ��쳣:" + ex.Message.ToString());
            }
            finally
            {
                fs.Close();
                resStrm.Close();
                ftpRes.Close();
            }
        }
        public bool BreakPointDownLoadFile(string ftpUri, string saveFile, ref string ErrMsg)
        {
            ErrMsg = "";
            bool UpRtn = false;
            System.IO.FileStream fs = null;
            System.Net.FtpWebResponse ftpRes = null;
            System.IO.Stream resStrm = null;
            try
            {
                //�����ļ���URI
                Uri u = new Uri(ftpUri);
                //�趨�����ļ��ı���·��
                string downFile = saveFile;
                //FtpWebRequest������
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //�趨�û���������
                ftpReq.Credentials = new System.Net.NetworkCredential(ftpUser, ftpPassWord);
                //Method��WebRequestMethods.Ftp.DownloadFile("RETR")�趨
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //Ҫ�����˺�ر�����
                ftpReq.KeepAlive = false;
                //ʹ��ASCII��ʽ����
                ftpReq.UseBinary = false;
                //�趨PASSIVE��ʽ��Ч
                ftpReq.UsePassive = false;

                //�ж��Ƿ��������
                //����д�������ļ���FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //��������
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //һ������
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //ȡ��FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //Ϊ�������ļ�ȡ��Stream
                resStrm = ftpRes.GetResponseStream();
                //д�����ص�����
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readSize = resStrm.Read(buffer, 0, buffer.Length);
                    if (readSize == 0)
                        break;
                    fs.Write(buffer, 0, readSize);
                }
                UpRtn = true; ;
            }
            catch (Exception ex)
            {
                ErrMsg = ("FTP�����ļ��쳣:" + ex.Message.ToString());
                UpRtn = false;
            }
            finally
            {
                fs.Close();
                resStrm.Close();
                ftpRes.Close();
            }
            return UpRtn;
        }
        public bool BreakPointDownLoadFile(string ftpUri, string saveFile, string FtpUser, string FtpPassWord, ref string ErrMsg)
        {
            ErrMsg = "";
            bool UpRtn = false;
            System.IO.FileStream fs = null;
            System.Net.FtpWebResponse ftpRes = null;
            System.IO.Stream resStrm = null;
            try
            {
                //�����ļ���URI
                Uri u = new Uri(ftpUri);
                //�趨�����ļ��ı���·��
                string downFile = saveFile;
                //FtpWebRequest������
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //�趨�û���������
                ftpReq.Credentials = new System.Net.NetworkCredential(FtpUser, FtpPassWord);
                //Method��WebRequestMethods.Ftp.DownloadFile("RETR")�趨
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //Ҫ�����˺�ر�����
                ftpReq.KeepAlive = false;
                //ʹ��ASCII��ʽ����
                ftpReq.UseBinary = false;
                //�趨PASSIVE��ʽ��Ч
                ftpReq.UsePassive = false;

                //�ж��Ƿ��������
                //����д�������ļ���FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //��������
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //һ������
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //ȡ��FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //Ϊ�������ļ�ȡ��Stream
                resStrm = ftpRes.GetResponseStream();
                //д�����ص�����
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int readSize = resStrm.Read(buffer, 0, buffer.Length);
                    if (readSize == 0)
                        break;
                    fs.Write(buffer, 0, readSize);
                }
                UpRtn = true; ;
            }
            catch (Exception ex)
            {
                ErrMsg = ("FTP�����ļ��쳣:" + ex.Message.ToString());
                UpRtn = false;
            }
            finally
            {
                fs.Close();
                resStrm.Close();
                ftpRes.Close();
            }
            return UpRtn;
        }


        #region ��FTP�����������ļ��У������ļ����µ��ļ����ļ���

        /// <summary>
        /// �г�FTP���������浱ǰĿ¼�������ļ���Ŀ¼
        /// </summary>
        /// <param name="ftpUri">FTPĿ¼</param>
        /// <returns></returns>
        public List<FileStruct> ListFilesAndDirectories(string ftpUri)
        {
            WebResponse webresp = null;
            StreamReader ftpFileListReader = null;
            FtpWebRequest ftpRequest = null;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(new Uri(ftpUri));
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftpRequest.Credentials = new NetworkCredential(ftpUser, ftpPassWord);
                webresp = ftpRequest.GetResponse();
                ftpFileListReader = new StreamReader(webresp.GetResponseStream(), Encoding.Default);
            }
            catch (Exception ex)
            {
                throw new Exception("��ȡ�ļ��б����������Ϣ���£�" + ex.ToString());
            }
            string Datastring = ftpFileListReader.ReadToEnd();
            return GetList(Datastring);

        }

        /// <summary>
        /// �г�FTPĿ¼�µ������ļ�
        /// </summary>
        /// <param name="ftpUri">FTPĿ¼</param>
        /// <returns></returns>
        public List<FileStruct> ListFiles(string ftpUri)
        {
            List<FileStruct> listAll = ListFilesAndDirectories(ftpUri);
            List<FileStruct> listFile = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (!file.IsDirectory)
                {
                    listFile.Add(file);
                }
            }
            return listFile;
        }


        /// <summary>
        /// �г�FTPĿ¼�µ�����Ŀ¼
        /// </summary>
        /// <param name="ftpUri">FRTPĿ¼</param>
        /// <returns>Ŀ¼�б�</returns>
        public List<FileStruct> ListDirectories(string ftpUri)
        {
            List<FileStruct> listAll = ListFilesAndDirectories(ftpUri);
            List<FileStruct> listDirectory = new List<FileStruct>();
            foreach (FileStruct file in listAll)
            {
                if (file.IsDirectory)
                {
                    listDirectory.Add(file);
                }
            }
            return listDirectory;
        }

        /// <summary>
        /// ����ļ���Ŀ¼�б�
        /// </summary>
        /// <param name="datastring">FTP���ص��б��ַ���Ϣ</param>
        private List<FileStruct> GetList(string datastring)
        {
            List<FileStruct> myListArray = new List<FileStruct>();
            string[] dataRecords = datastring.Split('\n');
            FileListStyle _directoryListStyle = GuessFileListStyle(dataRecords);
            foreach (string s in dataRecords)
            {
                if (_directoryListStyle != FileListStyle.Unknown && s != "")
                {
                    FileStruct f = new FileStruct();
                    f.Name = "..";
                    switch (_directoryListStyle)
                    {
                        case FileListStyle.UnixStyle:
                            f = ParseFileStructFromUnixStyleRecord(s);
                            break;
                        case FileListStyle.WindowsStyle:
                            f = ParseFileStructFromWindowsStyleRecord(s);
                            break;
                    }
                    if (!(f.Name == "." || f.Name == ".."))
                    {
                        myListArray.Add(f);
                    }
                }
            }
            return myListArray;
        }
        /// <summary>
        /// ��Unix��ʽ�з����ļ���Ϣ
        /// </summary>
        /// <param name="Record">�ļ���Ϣ</param>
        private FileStruct ParseFileStructFromUnixStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //����һ����
            f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //����һ����
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            if (yearOrTime.IndexOf(":") >= 0)  //time
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }
            f.CreateTime = DateTime.Parse(_cutSubstringFromStringWithTrim(ref processstr, ' ', 8));
            f.Name = processstr;   //����������
            return f;
        }

        /// <summary>
        /// ��Windows��ʽ�з����ļ���Ϣ
        /// </summary>
        /// <param name="Record">�ļ���Ϣ</param>
        private FileStruct ParseFileStructFromWindowsStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            string dateStr = processstr.Substring(0, 8);
            processstr = (processstr.Substring(8, processstr.Length - 8)).Trim();
            string timeStr = processstr.Substring(0, 7);
            processstr = (processstr.Substring(7, processstr.Length - 7)).Trim();
            DateTimeFormatInfo myDTFI = new CultureInfo("en-US", false).DateTimeFormat;
            myDTFI.ShortTimePattern = "t";
            f.CreateTime = DateTime.Parse(dateStr + " " + timeStr, myDTFI);
            if (processstr.Substring(0, 5) == "<DIR>")
            {
                f.IsDirectory = true;
                processstr = (processstr.Substring(5, processstr.Length - 5)).Trim();
            }
            else
            {
                string[] strs = processstr.Split(new char[] { ' ' }, 2);// StringSplitOptions.RemoveEmptyEntries);   // true);
                processstr = strs[1];
                f.IsDirectory = false;
            }
            f.Name = processstr;
            return f;
        }
        /// <summary>
        /// ����һ���Ĺ�������ַ�����ȡ
        /// </summary>
        /// <param name="s">��ȡ���ַ���</param>
        /// <param name="c">���ҵ��ַ�</param>
        /// <param name="startIndex">���ҵ�λ��</param>
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
        /// <summary>
        /// �ж��ļ��б�ķ�ʽWindow��ʽ����Unix��ʽ
        /// </summary>
        /// <param name="recordList">�ļ���Ϣ�б�</param>
        private FileListStyle GuessFileListStyle(string[] recordList)
        {
            foreach (string s in recordList)
            {
                if (s.Length > 10
                 && Regex.IsMatch(s.Substring(0, 10), "(-|d)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)(-|r)(-|w)(-|x)"))
                {
                    return FileListStyle.UnixStyle;
                }
                else if (s.Length > 8
                 && Regex.IsMatch(s.Substring(0, 8), "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                {
                    return FileListStyle.WindowsStyle;
                }
            }
            return FileListStyle.Unknown;
        }

        /// <summary> 
        /// ��FTP���������ļ��� 
        /// </summary> 
        /// <param name="ftpDir">FTP�ļ���·��</param> 
        /// <param name="saveDir">����ı����ļ���·��</param> 
        public void DownFtpDir(string ftpDir, string saveDir)
        {
            List<FileStruct> files = ListFilesAndDirectories(ftpDir);
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            foreach (FileStruct f in files)
            {
                if (f.IsDirectory) //�ļ��У��ݹ��ѯ
                {
                    DownFtpDir(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                }
                else //�ļ���ֱ������
                {
                    DownLoadFile(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                }
            }
        }


        #endregion
    }
    #region �ļ���Ϣ�ṹ
    public struct FileStruct
    {
        public string Flags;
        public string Owner;
        public string Group;
        public bool IsDirectory;
        public DateTime CreateTime;
        public string Name;
    }
    public enum FileListStyle
    {
        UnixStyle,
        WindowsStyle,
        Unknown
    }
    #endregion
    public class IniFiles
    {
        public string FileName; //INI�ļ���
        //������дINI�ļ���API����
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        //��Ĺ��캯��������INI�ļ���
        public IniFiles(string AFileName)
        {
            // �ж��ļ��Ƿ����
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:����ö�ٵ��÷�
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
                //�ļ������ڣ������ļ�
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#������õ���");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini�ļ�������"));
                }
            }
            //��������ȫ·�������������·��
            FileName = fileInfo.FullName;
        }
        public IniFiles()
        {
            string AFileName ="sz.ini";
            // �ж��ļ��Ƿ����
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:����ö�ٵ��÷�
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
                //�ļ������ڣ������ļ�
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#������õ���");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini�ļ�������"));
                }
            }
            //��������ȫ·�������������·��
            FileName = fileInfo.FullName;
        }
        //дINI�ļ�
        public void WriteString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, FileName))
            {
                throw (new ApplicationException("дIni�ļ�����"));
            }
        }
        //��ȡINI�ļ�ָ��
        public string ReadString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //�����趨0��ϵͳĬ�ϵĴ���ҳ���ı��뷽ʽ�������޷�֧������
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Replace("\0", "").Trim();
        }

        //������
        public int ReadInteger(string Section, string Ident, int Default)
        {
            string intStr = ReadString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }
        //д����
        public void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        //������
        public bool ReadBool(string Section, string Ident, bool Default)
        {
            try
            {
                return Convert.ToBoolean(ReadString(Section, Ident, Convert.ToString(Default)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }
        //дBool
        public void WriteBool(string Section, string Ident, bool Value)
        {
            WriteString(Section, Ident, Convert.ToString(Value));
        }

        //��Ini�ļ��У���ָ����Section�����е�����Ident��ӵ��б���
        public void ReadSection(string Section, StringCollection Idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear();

            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0),
             FileName);
            //��Section���н���
            GetStringsFromBuffer(Buffer, bufLen, Idents);
        }

        private void GetStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }
        //��Ini�ļ��У���ȡ���е�Sections������
        public void ReadSections(StringCollection SectionList)
        {
            //Note:�������Bytes��ʵ�֣�StringBuilderֻ��ȡ����һ��Section
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
             Buffer.GetUpperBound(0), FileName);
            GetStringsFromBuffer(Buffer, bufLen, SectionList);
        }
        //��ȡָ����Section������Value���б���
        public void ReadSectionValues(string Section, NameValueCollection Values)
        {
            StringCollection KeyList = new StringCollection();
            ReadSection(Section, KeyList);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, ReadString(Section, key, ""));

            }
        }
        /**/
        ////��ȡָ����Section������Value���б��У�
        //public void ReadSectionValues(string Section, NameValueCollection Values,char splitString)
        //{�� string sectionValue;
        //����string[] sectionValueSplit;
        //����StringCollection KeyList = new StringCollection();
        //����ReadSection(Section, KeyList);
        //����Values.Clear();
        //����foreach (string key in KeyList)
        //����{
        //��������sectionValue=ReadString(Section, key, "");
        //��������sectionValueSplit=sectionValue.Split(splitString);
        //��������Values.Add(key, sectionValueSplit[0].ToString(),sectionValueSplit[1].ToString());

        //����}
        //}
        //���ĳ��Section
        public void EraseSection(string Section)
        {
            //
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {

                throw (new ApplicationException("�޷����Ini�ļ��е�Section"));
            }
        }
        //ɾ��ĳ��Section�µļ�
        public void DeleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }
        //Note:����Win9X����˵��Ҫʵ��UpdateFile�����������е�����д���ļ�
        //��Win NT, 2000��XP�ϣ�����ֱ��д�ļ���û�л��壬���ԣ�����ʵ��UpdateFile
        //ִ�����Ini�ļ����޸�֮��Ӧ�õ��ñ��������»�������
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //���ĳ��Section�µ�ĳ����ֵ�Ƿ����
        public bool ValueExists(string Section, string Ident)
        {
            //
            StringCollection Idents = new StringCollection();
            ReadSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }

        //ȷ����Դ���ͷ�
        ~IniFiles()
        {
            UpdateFile();
        }
    }

    public class log
    {
        public static void WriteMyLog(string message)
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            try
            {
                //��־�ļ�·�� 
                string filePath = LOG_FOLDER + "\\PATHHISZGQJK" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//����ļ������� 
                {
                    File.Create(filePath).Close();
                }
                StreamWriter sw = File.AppendText(filePath);

                sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + message);

                sw.WriteLine();
                sw.Close();
            }
            catch
            { }
        }
        public static void WriteMyLog(string logName,string message)
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            try
            {
                //��־�ļ�·�� 
                string filePath = LOG_FOLDER + "\\" + logName + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//����ļ������� 
                {
                    File.Create(filePath).Close();
                }
                StreamWriter sw = File.AppendText(filePath);
                sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + message);
                sw.WriteLine();
                sw.Close();
            }
            catch
            { }
        }
        public static string readlog()
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            string hl7log = "";
            try
            {
                //��־�ļ�·�� 
                string filePath = LOG_FOLDER + "\\PATHHISZGQJK" + DateTime.Now.ToShortDateString() + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//����ļ������� 
                {
                    File.Create(filePath).Close();
                }
                hl7log = File.ReadAllText(filePath);
                return hl7log;
            }
            catch
            {
                return "";
            }


        }
        public static void clearlog()
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            //string hl7log = "";
            try
            {
                //��־�ļ�·�� 
                string filePath = LOG_FOLDER + "\\PATHHISZGQJK" + DateTime.Now.ToShortDateString() + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//����ļ������� 
                {
                    File.Create(filePath).Close();
                }
                File.WriteAllText(filePath, "");

            }
            catch
            {

            }
        }

    }

    class prreport
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        StringBuilder retVal,
            int size,
        string lpFileName
        );

        [DllImport("reportdll2.dll")]
        private static extern long report32(
        int dy,
        string conn,
        string sqlm,
        string image,
        string imagename,
        string yymc,
        string bbbm,
        string outfilename
        );

        public delegate long report2(
        int dy,
        string conn,
        string sqlm,
        string image,
        string imagename,
        string yymc,
        string bbbm,
        string outfilename
        );
        private LoadDllapi dllload = new LoadDllapi();
        protected String ConnectionString;
        protected string x1;
        protected int dy;
        private delegate bool wt_zoominout(
            //IntPtr ahandle,
        string sourcefile,
        string targetfile,
        int picx,
        int picy
        );
        public static bool txzoom(string sor, string dst, int picx, int picy)
        {
            LoadDllapi dllloadz = new LoadDllapi();
            if ((int)dllloadz.initPath("imagedll.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return false;
            }
            wt_zoominout xx = (wt_zoominout)dllloadz.InvokeMethod("wt_zoominout", typeof(wt_zoominout));
            return xx(sor, dst, picx, picy);
        }
        public prreport()
        {
            string pathstr = Application.StartupPath + "\\sz.ini";
            //WritePrivateProfileString("StudentInfo", "Name", strName, pathstr);
            StringBuilder strTemp = new StringBuilder(255);
            int i = GetPrivateProfileString("sqlserver", "Server", "", strTemp, 255, pathstr);
            string server = strTemp.ToString().Trim();
            i = GetPrivateProfileString("sqlserver", "DataBase", "", strTemp, 255, pathstr);
            string database = strTemp.ToString().Trim();
            i = GetPrivateProfileString("sqlserver", "UserID", "", strTemp, 255, pathstr);
            string userid = strTemp.ToString().Trim();
            i = GetPrivateProfileString("sqlserver", "PassWord", "", strTemp, 255, pathstr);
            string password = strTemp.ToString().Trim();
            try
            {
                i = GetPrivateProfileString("bbsj", "sj", "", strTemp, 255, pathstr);

                dy = Convert.ToInt16(strTemp.ToString().Trim());
            }
            catch
            {
                dy = 1;
            }
            ConnectionString = "Provider=MSDASQL.1;Persist Security Info=True;DRIVER=SQL Server;pwd=" + password + ";SERVER=" + server + ";DATABASE=" + database + ";UID=" + userid + ";APP=pasnet";
            if (server.Trim() == "")
            {
                OdbcConnection oconn = new OdbcConnection("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p");
                oconn.Open();
                string oserver = oconn.DataSource.ToString();
                string odatabase = oconn.Database.ToString();
                oconn.Close();

                //           string odbcstring = "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p";
                //
                //             server = odbcstring.Substring(odbcstring.IndexOf("DSN=") + 4, odbcstring.IndexOf(";") - 4);
                //              RegistryKey rs;
                //              rs = Registry.LocalMachine.OpenSubKey("SOFTWARE\\ODBC\\ODBC.INI\\" + server);
                //             if (rs != null)
                //             {
                //                server = rs.GetValue("Server", "").ToString();
                //
                //            }
                //              rs.Close();


                ConnectionString = "Provider=MSDASQL.1;Persist Security Info=True;DRIVER=SQL Server;pwd=4s3c2a1p;SERVER=" + oserver + ";DATABASE=" + odatabase + ";UID=pathnet;APP=pasnet";

            }
            //sqlserverdatabase obj = new sqlserverdatabase();

            x1 = Application.StartupPath;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dy">1:��Ԥ�� 2:ֱ�Ӵ�ӡ 3:��� 10:����jpg 13:pdf</param>
        /// <param name="sqlstring"></param>
        /// <param name="handle"></param>
        /// <param name="image"></param>
        /// <param name="imagename"></param>
        /// <param name="bgname"></param>
        /// <param name="jpgname"></param>
        /// <param name="yymc"></param>
        public long print(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname, string yymc, int dy)
        {
            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return 0;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));

            //1:��Ԥ�� 2:ֱ�Ӵ�ӡ 3:��� 10:����jpg 13:pdf
            long longxx = xx(dy, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();
            return longxx;
        }
        public long printjpg(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname, string yymc)
        {
            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return 0;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));
            long longxx = xx(10, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();
            return longxx;
        }
        public long printpdf(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname, string yymc)
        {
            if ((int)dllload.initPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase+"\\reportdll2.dll") == 0)
            {
                log.WriteMyLog("��ӡ�ؼ����ô���");
                return 0;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));
            long longxx = xx(13, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();
            return longxx;
        }
    }

    class copydir
    {
        public static void copyDirectory(string sourceDirectory, string destDirectory)
        {

            if (!Directory.Exists(sourceDirectory))
            {
                Directory.CreateDirectory(sourceDirectory);
            }
            if (!Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            copyFile(sourceDirectory, destDirectory);


            string[] directionName = Directory.GetDirectories(sourceDirectory);

            foreach (string directionPath in directionName)
            {
                string directionPathTemp = destDirectory + "\\" + directionPath.Substring(sourceDirectory.Length + 1);
                copyDirectory(directionPath, directionPathTemp);
            }
        }
        public static void copyFile(string sourceDirectory, string destDirectory)
        {

            string[] fileName = Directory.GetFiles(sourceDirectory);

            foreach (string filePath in fileName)
            {
                string filePathTemp = destDirectory + "\\" + filePath.Substring(sourceDirectory.Length + 1);
                File.Copy(filePath, filePathTemp, true);

            }
        }
    }
}

