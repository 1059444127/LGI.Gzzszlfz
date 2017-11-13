
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.Net;
using readini;
using System.IO;
using System.Threading;
using HL7;
using System.Diagnostics;
using System.Xml;
using PathHISZGQJK;

namespace LGZGQClass
{
    class LGZGQClass
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

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



        ////  private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        //private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        //string msg = "";
        //dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        ////�ļ�����ʽ��  ���ʱ��_�����_��������_�������_С�ţ�  yyyyMMddHHmmss_123456_CG_1_1.pdf


        //private LoadDllapi dllxx = new LoadDllapi();
        //private static string bggs = "";
        //public delegate int JPG2PDF(string jpgname, string pdfname);
        //public static string szweb = "";
        //public static int bgx = 0;
        //public static int bgy = 0;

        public enum type { JPG, PDF };
       /// <summary>
       /// 
       /// </summary>
       /// <param name="F_blh"></param>
       /// <param name="bglx"></param>
       /// <param name="bgxh"></param>
       /// <param name="jpgname"></param>
       /// <param name="rptpath"></param>
       /// <param name="type1"></param>
       /// <param name="filename"></param>
       /// <param name="message"></param>
       /// <returns></returns>
        public bool PDF_JPG(string F_blh, string bglx, string bgxh, ref string jpgname, string rptpath, type type1, string filename, ref  string message)
        {
         
            bool rtn = false;
            try
            {
                string status = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                    return false;
                }

                //���c:\tempĿ¼
                if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
                {
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                else
                {
                    try
                    {
                        System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                        System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                    }
                    catch
                    {
                    }
                }

                DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
                string txlbs = "";
                string localpath = "";
                message = "";
            
                if (!downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath, ref message))
                {
                    log.WriteMyLog("����ͼƬʧ�ܣ�" + message);
                    return false;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    if (File.Exists(localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
                    {
                        jpgname = "";
                        message = "����ͼ�����δ�ҵ�ͼƬ��"+localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString();
                        return false;
                    }
                }

                    if (localpath == "")
                    {
                        jpgname = "";
                        message = "PDF_JPG��������ͼ�����";
                        return false;
                    }
              
                string sbmp = "";
                string stxsm = "";
                string sJPGNAME = localpath + "\\" + filename;

             //   string sJPGNAME = localpath + "\\" + F_blh + ".jpg";

                string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                string inibglj = GetSz("dybg", "dybglj", "").Replace("\0", "");
              
                if (inibglj.Trim() == "")
                    inibglj = Application.StartupPath.ToString();
                    sBGGSName = inibglj + "\\" + rptpath + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                    sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
                }
                if (GetSz("rpt", "szqm", "0") == "1" && bglx.ToLower()=="cg")
                {
                      string bmppath = GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                    stxsm = stxsm + " ,";
                    string yszmlb = GetSz("All", "yszmlb", "f_shys");
                    bool bj = true;
                    foreach (string ysname in yszmlb.Split(','))
                    {
                        if ((ysname.ToLower().Trim() == "f_shys" ||ysname.ToLower().Trim() == "f_bgys")  && bj==true )
                        {
                            if (GetSz("rpt", "bgys2shys", "1")=="1")
                            {
                            if (jcxx.Rows[0]["F_shys"].ToString().Trim() == jcxx.Rows[0]["F_bgys"].ToString().Trim())
                                bj = false;
                            }
                       
                                if(ysname.ToLower().Trim()=="f_shys")
                                {
                                 sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
                                }
                                
                                if (ysname.ToLower().Trim() == "f_bgys")
                                {
                                    foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {
                                        if (name.Trim()!="")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                        }

                        if (ysname.ToLower().Trim() == "f_fzys")
                        {
                            foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                            {
                                if (name.Trim() != "" )
                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                            }
                        }
                    }
                  sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }

                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
               string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
                bool bcbddytx = false;
                if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
                    bcbddytx = true;

                if (bglx.ToLower() == "bd")
                {
                    DataTable BDBG = new DataTable();
                    try
                    {
                        BDBG = aa.GetDataTable("select * from T_BDBG  where F_blh='" + F_blh + "'  and F_BD_BGXH='" + bgxh + "'", "bdbg");
                    }
                    catch (Exception ex)
                    {
                        message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                        return false;
                    }
                    string bd_bggs = "����";
                    if (bcbddytx)
                        bd_bggs = bd_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ";
                  
                    sBGGSName =inibglj + "\\" + rptpath + "\\" + bd_bggs + ".frf";

                    if (GetSz("rpt", "bcbgszqm", "0") == "1")
                    {
                        string bmppath = GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                        stxsm = stxsm + " ,";

                        string yszmlb = GetSz("All", "yszmlb", "f_shys");
                        bool bj2 = true;
                        foreach (string ysname in yszmlb.Split(','))
                        {
                            if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj2 == true)
                            {
                                if (GetSz("rpt", "bgys2shys", "1") == "1")
                                {
                                    if (BDBG.Rows[0]["F_BD_shys"].ToString().Trim() == BDBG.Rows[0]["F_bd_bgys"].ToString().Trim())
                                        bj2 = false;
                                }
                                if (ysname.ToLower().Trim() == "f_shys")
                                {
                                    sbmp = sbmp + bmppath + "\\" + BDBG.Rows[0]["F_bd_shys"].ToString().Trim() + ".bmp,";
                                }
                                if (ysname.ToLower().Trim() == "f_bgys")
                                {
                                    foreach (string name in BDBG.Rows[0]["f_bd_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {

                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in BDBG.Rows[0]["f_bd_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }
                            if (bcbddytx)
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bd_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                            else
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bd_bggs + ".frf";
                    }

                    sSQL_DY = "SELECT * FROM T_JCXX,T_BdBG WHERE T_JCXX.F_BLH = T_BdBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_Bd_BGXH='" + bgxh + "'";
                    
                    if (bcbddytx)
                        bggs = bd_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    else
                        bggs = bd_bggs + ".frf";

                    sJPGNAME = localpath + "\\" + filename;
                }


                if (bglx.ToLower() == "bc")
                {
                    DataTable BCBG = new DataTable();
                    try
                    {
                        BCBG = aa.GetDataTable("select * from T_BCBG  where F_blh='" + F_blh + "'  and F_BC_BGXH='" + bgxh + "'", "bcbg");
                    }
                    catch (Exception ex)
                    {
                        message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                        return false;
                    }
                    string bc_bggs = "����";
                    try
                    {
                        if (BCBG.Rows.Count > 0)
                        {
                            try
                            {
                                bc_bggs = BCBG.Rows[0]["F_BC_BGGS"].ToString().Trim();
                            }
                            catch
                            {
                                bc_bggs = "����";
                            }
                        }
                    }
                    catch
                    { }
                    if (bc_bggs.Trim() == "")
                        bc_bggs = "����";
                    if (bcbddytx)
                        bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ";
                   
                    sBGGSName = inibglj + "\\" + rptpath + "\\" + bc_bggs + ".frf";
                
                    if (GetSz("rpt", "bcbgszqm", "0") == "1")
                    {
                        string bmppath = GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                        stxsm = stxsm + " ,";

                        string yszmlb = GetSz("All", "yszmlb", "f_shys");
                         bool bj2 = true;
                    foreach (string ysname in yszmlb.Split(','))
                    {

                        if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj2 == true)
                        {
                            if (GetSz("rpt", "bgys2shys", "1") == "1")
                            {
                                if (BCBG.Rows[0]["F_bc_shys"].ToString().Trim() == BCBG.Rows[0]["F_bc_bgys"].ToString().Trim())
                                    bj2 = false;
                            }
                            if (ysname.ToLower().Trim() == "f_shys")
                            {
                                sbmp = sbmp + bmppath + "\\" + BCBG.Rows[0]["F_bc_shys"].ToString().Trim() + ".bmp,";
                            }
                            if (ysname.ToLower().Trim() == "f_bgys")
                            {
                                foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                {

                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
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
                       if (bcbddytx)
                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                            else
                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs  + ".frf";
                    }
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                 
                    if (bcbddytx)
                        bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    else
                        bggs = bc_bggs + ".frf";
                    sJPGNAME = localpath + "\\" + filename;
                }



                //�жϱ����ʽ�Ƿ����
                if (!File.Exists(sBGGSName))
                {
                    message = "�����ʽ������:" + sBGGSName;
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
                                pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,debug);
                                if (debug == "1")
                                    log.WriteMyLog("pr.print���");
                                jpgname = sJPGNAME.Replace(".", "_1.");
                            }
                            else
                            {
                                pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,debug);
                                if (debug == "1")
                                    log.WriteMyLog("pr.print���");
                                jpgname = sJPGNAME;
                            }
                        }
                        catch (Exception e3)
                        {
                            message = "����pdf�쳣������prreport�쳣:" + e3.Message;
                            rtn = false;
                        }

                        if (!File.Exists(jpgname))
                        {
                            message = "PDF_JPG-����δ�ҵ��ļ�1";
                            rtn = false;
                            continue;
                        }
                        else
                        {
                            message = "";
                            rtn = true;
                            break;
                        }

                }
                return rtn;

            }
            catch(Exception  e4)
            {
                message = "PDF_JPG�����쳣:" + e4.Message;
                return false;
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="f_blh">�����</param>
        /// <param name="txml">ͼ��Ŀ¼</param>
        /// <param name="aa"></param>
        /// <param name="txlbs"></param>
        /// <param name="localpath">��ʱĿ¼</param>
        /// <param name="err_msg">������Ϣ</param>
        /// <returns>�ɹ�����ʧ��</returns>
        public bool downtx(string f_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath, ref string err_msg)
        {
       
            try
            {
                //���c:\tempĿ¼
                if (!System.IO.Directory.Exists(@"c:\temp\" + f_blh))
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + f_blh);
                else
                {
                    try
                    {
                        System.IO.Directory.Delete(@"c:\temp\" + f_blh, true);
                        System.IO.Directory.CreateDirectory(@"c:\temp\" + f_blh);
                    }
                    catch (Exception e1)
                    {
                    }
                }
                //��ʱĿ¼
                localpath = @"c:\temp\" + f_blh;

                //����FTP����
                string ftpserver = GetSz("ftp", "ftpip", "").Replace("\0", "");
                string ftpuser = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                string ftppwd = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = GetSz("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
                string ftpremotepath = GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                string ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
                string txpath = GetSz("txpath", "txpath", "").Replace("\0", "");

                FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
                //����Ŀ¼
                string gxml = GetSz("txpath", "txpath", "").Replace("\0", "");
                //string gxuid = GetSz("txpath", "username", "").Replace("\0", "");
                //string gxpwd = GetSz("txpath", "password", "").Replace("\0", "");

             
                DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + f_blh + "' and F_sfdy='1'", "txlb");
                string txm = "";

                if (ftps == "1")//FTP���ط�ʽ
                { 

                    for (int i = 0; i < txlb.Rows.Count; i++)
                    {
                       
                        txm = txlb.Rows[i]["F_txm"].ToString().Trim();
                        string ftpstatus = "";
                        try
                        {
                            err_msg = "";
                          
                            fw.Download(localpath, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus, ref  err_msg);
                           
                            if (ftpstatus == "Error")
                            {
                                localpath = "";
                                return false;
                            }
                            else
                            {

                                if (f.ReadInteger("TX", "ZOOM", 1) == 1)
                                {

                                    int picx = f.ReadInteger("TX", "picx", 320);
                                    int picy = f.ReadInteger("TX", "picy", 240);
                                    try
                                    {

                                        prreport.txzoom(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);

                                    }
                                    catch (Exception ee2)
                                    {
                                        log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                    }

                                }
                                txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";

                            }
                        }
                        catch (Exception ee2)
                        {
                            err_msg = "FTP����ͼ�����:" + ee2.Message;
                            localpath = "";
                            return false;
                        }
                    }
                  
                    return true;
                }
                else //�������ط�ʽ
                {
                    if (txpath == "")
                    {
                        err_msg = "sz.ini txpathͼ��Ŀ¼δ����";
                        localpath = "";
                        return false;
                    }
                    for (int i = 0; i < txlb.Rows.Count; i++)
                    {
                        txm = txlb.Rows[i]["F_txm"].ToString().Trim();

                        try
                        {
                            File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);

                            if (f.ReadInteger("TX", "ZOOM", 1) == 1)
                            {
                                int picx = f.ReadInteger("TX", "picx", 320);
                                int picy = f.ReadInteger("TX", "picy", 240);
                                try
                                {
                                    prreport.txzoom(localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
                                }
                                catch (Exception ee2)
                                {
                                    log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
                                }
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }

                        catch (Exception ee3)
                        {
                            err_msg = "��������ͼ�����:" + ee3.Message;
                            localpath = "";
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception e4)
            {
               
                 err_msg = "����ͼ���쳣:" + e4.Message;
                            return false;
            }

        }

/// 
        /// <summary>
        /// �ϴ�
        /// </summary>
        /// <param name="blh">�����</param>
        /// <param name="jpgpath">�ļ���������·��</param>
        /// <param name="ml">Ŀ¼</param>
        /// <param name="err_msg">������Ϣ</param>
        /// <param name="lb">1:ָ������Ŀ¼�ϴ���[ZGQJK]toPDFPath=)��2:����·���ϴ�([txpath]txpath=)��3:ftp�ϴ�([ftp]);����ftp�ϴ�([ftpup])  </param>
        /// <returns></returns>
        /// 
        public bool UpPDF( string jpgpath, string ml, ref string err_msg, int lb)
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
                string tjtxpath = GetSz("savetohis", "toPDFPath", @"\\192.0.19.147\GMS");
                string debug = GetSz("savetohis", "debug", "0");
                string txpath = GetSz("txpath", "txpath", @"E:\pathimages");

                if (lb == 3)
                {
                    ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = GetSz("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = GetSz("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
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
                    ftps = GetSz("ftpup", "ftp", "1").Replace("\0", "");
                    ftpServerIP = GetSz("ftpup", "ftpip", "").Replace("\0", "");
                    ftpUserID = GetSz("ftpup", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = GetSz("ftpup", "pwd", "ftp").Replace("\0", "");
                    ftplocal = GetSz("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = GetSz("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");

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
                                        err_msg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                           
                            //if (debug=="1")
                            //log.WriteMyLog("�ж�ftp���Ƿ���ڸ��ļ�");
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            //if (fw.fileCheckExist(ftpURI, jpgname))
                            //{
                            //    //ɾ��ftp�ϵ�jpg�ļ�
                            //    fw.fileDelete(ftpURI, jpgname).ToString();
                            //}
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            fw.Upload(jpgpath, ml, out status, ref err_msg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + err_msg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                }
                                else
                                {
                                    err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml ))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + ml  + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + ml ;
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
                                err_msg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
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
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }
   
        public  bool UpPDF(string blh, string jpgpath, string ml, ref string err_msg,int lb)
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
                string tjtxpath = GetSz("savetohis", "toPDFPath", @"\\192.0.19.147\GMS");
                string debug = GetSz("savetohis", "debug", "0");
                string txpath = GetSz("txpath", "txpath", @"E:\pathimages");

                if (lb == 3)
                {
                    ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = GetSz("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = GetSz("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
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
                     ftps = GetSz("ftpup", "ftp", "1").Replace("\0", "");
                     ftpServerIP = GetSz("ftpup", "ftpip", "").Replace("\0", "");
                     ftpUserID = GetSz("ftpup", "user", "ftpuser").Replace("\0", "");
                     ftpPassword = GetSz("ftpup", "pwd", "ftp").Replace("\0", "");
                     ftplocal = GetSz("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                     ftpRemotePath = GetSz("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");
        
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
                                        err_msg = "FTP����Ŀ¼�쳣";
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
                                  
                                    fw.Makedir(ftpURI,blh, out stat);
                                   
                                    if (stat != "OK")
                                    {
                                        err_msg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI +"/"+ blh + "/";
                            
                            //if (debug=="1")
                            //log.WriteMyLog("�ж�ftp���Ƿ���ڸ��ļ�");
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            //if (fw.fileCheckExist(ftpURI, jpgname))
                            //{
                            //    //ɾ��ftp�ϵ�jpg�ļ�
                            //    fw.fileDelete(ftpURI, jpgname).ToString();
                            //}
                            if (debug == "1")
                            log.WriteMyLog("�ϴ������ɵ��ļ�");
                            fw.Upload(jpgpath, ml+"/"+blh, out status, ref err_msg);
                          //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������"+status+"\r\n" + err_msg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {
                            
                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                }
                                else
                                {
                                    err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                }
                               
                            }
                            catch (Exception err2)
                            {
                                err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI  + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml+"\\"+blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml+"\\"+blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--����Ŀ¼�쳣";
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
                                err_msg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
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
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch(Exception e4)
            {
                err_msg = "UpPDF�����쳣��"+e4.Message;
                return false;
            }

        }

        public bool UpPDF(string blh, string jpgpath, string ml, ref string err_msg, int lb,ref string pdfpath)
        {
            try
            {
                pdfpath = "";
                err_msg = "";
                string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
                //---�ϴ�jpg----------
                //----------------�ϴ���ftp---------------------
                string status = "";
                string ftps = GetSz("ftpup", "ftp", "").Replace("\0", "");
                string ftpServerIP = GetSz("ftpup", "ftpip", "").Replace("\0", "");
                string ftpUserID = GetSz("ftpup", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = GetSz("ftpup", "pwd", "ftp").Replace("\0", "");
                string ftplocal = GetSz("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = GetSz("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");
                string tjtxpath = GetSz("savetohis", "toPDFPath", @"\\192.0.19.147\GMS");
                string debug = GetSz("savetohis", "debug", "0");

                string txpath = GetSz("txpath", "txpath", @"E:\pathimages");

                if (lb == 3)
                {
                    ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = GetSz("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = GetSz("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
                }
                if (lb == 1)
                {
                    ftps = "0";
                }
                if (lb == 2)
                {
                    ftps = "0"; tjtxpath = txpath;
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
                                        err_msg = "FTP����Ŀ¼�쳣";
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
                                    err_msg = "FTP����Ŀ¼�쳣";
                                    return false;
                                }
                            }
                            ftpURI = ftpURI  + blh + "/";

                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            fw.Upload(jpgpath, ml + "/" + blh, out status, ref err_msg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + err_msg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                    pdfpath = ftpURI + "" + jpgname;
                                }
                                else
                                {
                                    err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
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
                                        err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--����Ŀ¼�쳣";
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
                            {
                                status = "OK";
                                pdfpath = tjtxpath + "\\" + jpgname;
                            }
                            else
                            {
                                err_msg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
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
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }

        public bool UpPDF(string blh, string jpgpath, string ml, ref string err_msg,string filepath,string ip,string useName,string pwd)
        {
            try
            {
                string  tjtxpath="\\\\"+ip+"\\"+filepath;
             
                if (File.Exists(jpgpath))
                {
                    string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
               

                      if (useName.Trim() != "")
                      {

                          //������
                          Process pro = new Process();
                          try
                          {

                              try
                              {
                                  pro.StartInfo.FileName = "cmd.exe";
                                  pro.StartInfo.UseShellExecute = false;
                                  pro.StartInfo.RedirectStandardInput = true;
                                  pro.StartInfo.RedirectStandardOutput = true;
                                  pro.StartInfo.RedirectStandardError = true;
                                  pro.StartInfo.CreateNoWindow = true;
                                  pro.Start();
                                  pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ " + pwd + " /user:" + useName + "");
                                  ////  Thread.Sleep(1000);
                              }
                              catch
                              {
                              }
                              try
                              {
                                  if (ml.Trim() != "")
                                  {
                                      //�ж�mlĿ¼�Ƿ����
                                      if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml + "\\" + blh))
                                      {
                                          //Ŀ¼�����ڣ�����
                                        
                                          try
                                          {
                                              System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml + "\\" + blh);
                                          }
                                          catch
                                          {
                                              err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--����Ŀ¼�쳣";
                                            pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
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
                                   Thread.Sleep(500);
                                  if (File.Exists(tjtxpath + "\\" + jpgname))
                                  {
                                      pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                                      return true;
                                  }
                                  else
                                  {
                                      err_msg = "�ϴ�PDF�쳣1";
                                     pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                                      return false;
                                  }

                              }
                              catch (Exception ee3)
                              {
                                  err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
                                  try
                                  {
                                      pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                                  }
                                  catch
                                  {
                                  }
                                  return false;
                              }
                          }
                          catch (Exception ee)
                          {
                              err_msg = "�ϴ�PDF�쳣2:"+ee.Message;
                              return false;
                          }
                          finally
                          {
                              pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                          }

                      }
                      else
                      {

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
                                          err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--����Ŀ¼�쳣";
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
                                  return true;
                              else
                              {
                                  err_msg = "�ϴ�PDF�쳣2";
                                  return false;
                              }
                          }
                          catch (Exception ee3)
                          {
                              err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
                              return false;
                          }
                      }

                          return true;
                }
                else
                {
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }
    
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="ml">Ŀ¼</param>
        /// <param name="jpgname">�ļ���</param>
        /// <param name="err_msg">������Ϣ</param>
        /// <returns></returns>
        public  bool Delete(string ml, string jpgname, ref string err_msg)
        {

            string status = "";
            string ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
            string ftpServerIP = GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpUserID = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftpPassword = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpRemotePath = GetSz("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
            string tjtxpath = GetSz("JK_ZGQ", "PDFPath", "e:\\pathimages\\jpgbg");

            if (ftps == "1")
            {
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";

                try
                {

                    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                    if (fw.fileCheckExist(ftpURI, jpgname))
                    {
                        //ɾ��ftp�ϵ�jpg�ļ�
                        fw.fileDelete(ftpURI, jpgname).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    err_msg = "ɾ��ftp��PDF�쳣:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    err_msg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
                    return false;
                }
                try
                {
                    if(ml=="")
                        File.Delete(tjtxpath  + "\\" + jpgname);
                    else
                       File.Delete(tjtxpath + "\\" + ml + "\\" + jpgname);

                    return true;
                }
                catch (Exception ee)
                {
                    err_msg = "ɾ���ļ��쳣:" + ee.Message.ToString();
                    return false;
                }

            }

        }

        public bool FTP_Delete(string ml, string jpgname, ref string err_msg)
        {

            string status = "";
            string ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
            string ftpServerIP = GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpUserID = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftpPassword = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpRemotePath = GetSz("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
            string tjtxpath = GetSz("JK_ZGQ", "PDFPath", "e:\\pathimages\\jpgbg");

            if (ftps == "1")
            {
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                if (ml.Trim()!="")
                    ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";
                try
                {

                    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                    if (fw.fileCheckExist(ftpURI, jpgname))
                    {
                        //ɾ��ftp�ϵ�jpg�ļ�
                        fw.fileDelete(ftpURI, jpgname).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    err_msg = "ɾ��ftp��PDF�쳣:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    err_msg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
                    return false;
                }
                try
                {
                    if (ml == "")
                        File.Delete(tjtxpath + "\\" + jpgname);
                    else
                        File.Delete(tjtxpath + "\\" + ml + "\\" + jpgname);

                    return true;
                }
                catch (Exception ee)
                {
                    err_msg = "ɾ���ļ��쳣:" + ee.Message.ToString();
                    return false;
                }

            }

        }

        /// <summary>
        /// ����pdf��jpg
        /// </summary>
        /// <param name="F_blh">�����</param>
        /// <param name="bglx">��������</param>
        /// <param name="bgxh">�������</param>
        /// <param name="type1">JPG��PDF</param>
        /// <param name="err_msg">������Ϣ</param>
        /// <param name="F_ML">Ŀ¼</param>
        /// <returns></returns>
        public bool CreatePDF(string F_blh, string bglx, string bgxh, type type1, ref string err_msg)
        {

            try
            {
                string filename = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    err_msg = ("�׳��쳣:" + ex.Message.ToString());
                    //�쳣
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "�������ݿ����������⣡";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "������д���";
                    return false;
                }

                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                string bgzt = "";

                filename = "";
                if (bglx.ToLower() == "bd")
                {
                    dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + F_blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");

                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                }
                if (bglx.ToLower() == "bc")
                {
                    dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + F_blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                    filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
                }
                if (bglx.ToLower() == "cg")
                {
                    bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                    filename = jcxx.Rows[0]["F_SPARE5"].ToString();
                }

                if (filename.Trim() == "")
                {
                    err_msg = "���ڲ���Ϊ��";
                    return false;
                }


                if (bgzt == "�����")
                {
                    filename = DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + "_" + F_blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + ".pdf";
                    string rptpath = GetSz("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();
                    string jpgname = "";

                    //����PDF
                    string msg = "";
                    bool pdf1 = PDF_JPG(F_blh, bglx, bgxh, ref  jpgname, rptpath.Trim(), type1, filename, ref msg);
                    if (!pdf1)
                    {
                        err_msg = msg;
                        return false;
                    }
                    if (File.Exists(jpgname))
                        return true;
                    else
                        return false;

                    ////�ϴ�PDF
                    //Thread.Sleep(1000);
                    //bool status1 = false;

                    //    status1 = UpLoad(F_blh, jpgname, F_ML, ref msg);

                    //if (!status1)
                    //{
                    //    log.WriteMyLog(F_blh + "," + bglx + "," + bgxh + ",�ϴ�PDF�쳣��" + msg);
                    //    err_msg = msg;
                    //    return 0;
                    //}
                    //else
                    //{
                    //    string filename2 = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                    //    if (type1.ToString().ToLower() == "jpg")
                    //    {
                    //        filename2 = filename2.Replace(".", "_1.");
                    //        filename2 = filename2.ToLower();
                    //    }

                    //    DataTable dt = new DataTable();
                    //    dt = aa.GetDataTable("select * from T_BG_PDF  where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "'and F_BGXH='" + bgxh + "'", "F_BG_PDF");
                    //    if (dt.Rows.Count <= 0)
                    //        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + F_blh + "','" + bglx.ToLower() + "','" + bgxh + "','" + F_ML + "','" + filename2 + "')");
                    //    else
                    //    {
                    //        Delete(dt.Rows[0]["F_ML"].ToString(), dt.Rows[0]["F_FILENAME"].ToString(), ref message);
                    //        aa.ExecuteSQL("update  T_BG_PDF  set  F_ML='" + F_ML + "',F_FILENAME='" + filename2 + "'  where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "'and F_BGXH='" + bgxh + "'");
                    //    }
                    //    //�ɹ�
                    //    return 2;
                    //}
                }
                else
                {
                    err_msg = "����δ���";
                    return false;
                    //DataTable dt = new DataTable();
                    //dt = aa.GetDataTable("select * from T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx.ToLower() + "' and  F_BGXH='" + bgxh + "'", "F_BG_PDF");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    aa.ExecuteSQL("delete  T_BG_PDF   where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                    //    Delete(dt.Rows[0]["F_ML"].ToString(), dt.Rows[0]["F_FILENAME"].ToString(), ref message);
                    //}
                    ////�ɹ�
                    //return 2;

                }
            }
            catch(Exception  e4)
            {
                err_msg = "CreatePDF�����쳣��"+e4.Message;
                return false;
            }
        }

        public bool CreatePDF(string F_blh, string bglx, string bgxh, type type1, ref string err_msg,ref string jpgname)
        {

            try
            {
                string filename = "";
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "0";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    err_msg = ("�׳��쳣:" + ex.Message.ToString());
                    //�쳣
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "�������ݿ����������⣡";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "������д���";
                    return false;
                }

                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                string bgzt = "";

                filename = "";
                if (bglx.ToLower() == "bd")
                {
                    dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + F_blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");

                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                }
                if (bglx.ToLower() == "bc")
                {
                    dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + F_blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                    filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
                }
                if (bglx.ToLower() == "cg")
                {
                    bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                    filename = jcxx.Rows[0]["F_SPARE5"].ToString();
                }

                if (filename.Trim() == "")
                {
                    err_msg = "���ڲ���Ϊ��";
                    return false;
                }


                if (bgzt == "�����" || bgzt == "�ѷ���")
                {

                    if (type1.ToString().ToLower()=="pdf")
                    filename =F_blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh+"_"+ DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss")  + ".pdf";
                    else
                    filename = F_blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";


                    string rptpath = GetSz("ZGQJK", "rptpath", "rpt").Replace("\0", "").Trim();
                     jpgname = "";

                    //����PDF
                    string msg = "";
               
                    bool pdf1 = PDF_JPG(F_blh, bglx, bgxh, ref  jpgname, rptpath.Trim(), type1, filename, ref msg);
                  
                    if (!pdf1)
                    {
                        err_msg = msg;
                        return false;
                    }
                    if (File.Exists(jpgname))
                        return true;
                    else
                    {
                        err_msg = "δ�ҵ�PDF�ļ�";
                        return false;
                    }

                    ////�ϴ�PDF
                    //Thread.Sleep(1000);
                    //bool status1 = false;

                    //    status1 = UpLoad(F_blh, jpgname, F_ML, ref msg);

                    //if (!status1)
                    //{
                    //    log.WriteMyLog(F_blh + "," + bglx + "," + bgxh + ",�ϴ�PDF�쳣��" + msg);
                    //    err_msg = msg;
                    //    return 0;
                    //}
                    //else
                    //{
                    //    string filename2 = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                    //    if (type1.ToString().ToLower() == "jpg")
                    //    {
                    //        filename2 = filename2.Replace(".", "_1.");
                    //        filename2 = filename2.ToLower();
                    //    }

                    //    DataTable dt = new DataTable();
                    //    dt = aa.GetDataTable("select * from T_BG_PDF  where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "'and F_BGXH='" + bgxh + "'", "F_BG_PDF");
                    //    if (dt.Rows.Count <= 0)
                    //        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + F_blh + "','" + bglx.ToLower() + "','" + bgxh + "','" + F_ML + "','" + filename2 + "')");
                    //    else
                    //    {
                    //        Delete(dt.Rows[0]["F_ML"].ToString(), dt.Rows[0]["F_FILENAME"].ToString(), ref message);
                    //        aa.ExecuteSQL("update  T_BG_PDF  set  F_ML='" + F_ML + "',F_FILENAME='" + filename2 + "'  where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "'and F_BGXH='" + bgxh + "'");
                    //    }
                    //    //�ɹ�
                    //    return 2;
                    //}
                }
                else
                {
                    err_msg = "����δ���";
                    return false;
                    //DataTable dt = new DataTable();
                    //dt = aa.GetDataTable("select * from T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx.ToLower() + "' and  F_BGXH='" + bgxh + "'", "F_BG_PDF");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    aa.ExecuteSQL("delete  T_BG_PDF   where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                    //    Delete(dt.Rows[0]["F_ML"].ToString(), dt.Rows[0]["F_FILENAME"].ToString(), ref message);
                    //}
                    ////�ɹ�
                    //return 2;

                }
            }
            catch (Exception e4)
            {
                err_msg = "CreatePDF�����쳣��" + e4.Message;
                return false;
            }
        }

        public bool CreatePDF(string F_blh, string bglx, string bgxh, type type1,string filename, ref string err_msg, ref string jpgname)
        {

            try
            {
              
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    err_msg = ("�׳��쳣:" + ex.Message.ToString());
                    //�쳣
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "�������ݿ����������⣡";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "������д���";
                    return false;
                }

            
                    if (type1.ToString().ToLower() == "pdf")
                        filename =  filename + ".pdf";
                    else
                        filename =  filename + ".jpg";


                    string rptpath = GetSz("ZGQJK", "rptpath", "rpt").Replace("\0", "").Trim();
                    jpgname = "";

                    //����PDF
                    string msg = "";

                    bool pdf1 = PDF_JPG(F_blh, bglx, bgxh, ref  jpgname, rptpath.Trim(), type1, filename, ref msg);

                    if (!pdf1)
                    {
                        err_msg = msg;
                        return false;
                    }
                    if (File.Exists(jpgname))
                        return true;
                    else
                    {
                        err_msg = "δ�ҵ�PDF�ļ�";
                        return false;
                    }

           
            }
            catch (Exception e4)
            {
                err_msg = "CreatePDF�����쳣��" + e4.Message;
                return false;
            }
        }
        /// <summary>
        /// ��ձ�����ʱ�ļ�
        /// </summary>
        /// <param name="blh">�����</param>
        /// <returns></returns>
        public void DeleteTempFile(string blh)
        {
            try
            {
                System.IO.Directory.Delete(@"c:\temp\" + blh, true);
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



    class ZGQ_PDFJPG
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private LoadDllapi dllxx = new LoadDllapi();
        private static string bggs = "";
        public delegate int JPG2PDF(string jpgname, string pdfname);
        public static string szweb = "";
        public static int bgx = 0;
        public static int bgy = 0;

        public enum type { JPG, PDF };

        public bool CreatePDFJPG(string F_blh, string bglx, string bgxh, ref string fileName, string rptpath, type type1, ref  string message)
        {

            bool rtn = false;
            try
            {
                if (bglx == "")
                    bglx = "cg";
                if (bgxh == "")
                    bgxh = "1";
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable jcxx = new DataTable();
                try
                {
                    jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                    return false;
                }
                //���c:\tempĿ¼
                if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
                {
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                }
                else
                {
                    try
                    {
                        System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                        System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
                    }
                    catch
                    {
                    }
                }

                DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
                string txlbs = "";
                string localpath = "";
                message = "";
                if (!downtx2(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), txlb, ref txlbs, ref localpath, ref message))
                {
                    log.WriteMyLog("����ͼƬʧ�ܣ�" + message);
                    return false;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    if (!File.Exists(localpath + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
                    {
                        fileName = "";
                        message = "����ͼ�����δ�ҵ�ͼƬ��" + localpath + "\\" + txlb.Rows[i]["F_TXM"].ToString();
                        return false;
                    }
                }

                string sbmp = "";
                string stxsm = "";
                string sJPGNAME = localpath + "\\" + fileName;


                string rptpath2 = "rpt";
                if (rptpath.Trim() != "")
                    rptpath2 = rptpath;

                string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                string inibglj = ZgqClass.GetSz("dybg", "dybglj", "").Replace("\0", "");

                if (inibglj.Trim() == "")
                {
                    inibglj = Application.StartupPath.ToString();
                }
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                    sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
                }
                if (LGZGQClass.GetSz("rpt", "szqm", "0") == "1" && bglx.ToLower() == "cg")
                {
                    string bmppath = ZgqClass.GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                    stxsm = stxsm + " ,";
                    string yszmlb = ZgqClass.GetSz("All", "yszmlb", "f_shys");
                    bool bj = true;
                    foreach (string ysname in yszmlb.Split(','))
                    {
                        if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj == true)
                        {
                            if (ZgqClass.GetSz("rpt", "bgys2shys", "1") == "1")
                            {
                                if (jcxx.Rows[0]["F_shys"].ToString().Trim() == jcxx.Rows[0]["F_bgys"].ToString().Trim())
                                    bj = false;
                            }

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

                    sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    }
                }

                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
                bool bcbddytx = false;
                if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
                    bcbddytx = true;

                if (bglx.ToLower() == "bd")
                {
                    sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\����.frf";
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                    if (bcbddytx)
                        bggs = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    else
                        bggs = "����.frf";
                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\" + rptpath2 + "\\����.frf";
                    }
                    sJPGNAME = localpath + "\\" + fileName;
                }
                if (bglx.ToLower() == "bc")
                {
                    DataTable BCBG = new DataTable();
                    try
                    {
                        BCBG = aa.GetDataTable("select * from T_BCBG  where F_blh='" + F_blh + "'  and F_BC_BGXH='" + bgxh + "'", "bcbg");
                    }
                    catch (Exception ex)
                    {
                        message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
                        return false;
                    }
                    string bc_bggs = "����";
                    try
                    {
                        if (BCBG.Rows.Count > 0)
                        {
                            try
                            {
                                bc_bggs = BCBG.Rows[0]["F_BC_BGGS"].ToString().Trim();
                            }
                            catch
                            {
                                bc_bggs = "����";
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (bc_bggs.Trim() == "")
                        bc_bggs = "����";

                    if (bcbddytx)
                        bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ";


                    sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";

                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";
                    }

                    if (ZgqClass.GetSz("rpt", "bcbgszqm", "0") == "1")
                    {
                        string bmppath = GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                        stxsm = stxsm + " ,";
                        string yszmlb = ZgqClass.GetSz("All", "yszmlb", "f_shys");
                        bool bj2 = true;
                        foreach (string ysname in yszmlb.Split(','))
                        {
                            if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj2 == true)
                            {
                                if (ZgqClass.GetSz("rpt", "bgys2shys", "1") == "1")
                                {
                                    if (BCBG.Rows[0]["F_bc_shys"].ToString().Trim() == BCBG.Rows[0]["F_bc_bgys"].ToString().Trim())
                                        bj2 = false;
                                }
                                if (ysname.ToLower().Trim() == "f_shys")
                                {
                                    sbmp = sbmp + bmppath + "\\" + BCBG.Rows[0]["F_bc_shys"].ToString().Trim() + ".bmp,";
                                }
                                if (ysname.ToLower().Trim() == "f_bgys")
                                {
                                    foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
                                    {
                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
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
                        if (bcbddytx)
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                        else
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + ".frf";
                        if (inibglj != "")
                        {
                            if (bcbddytx)
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                            else
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + ".frf";
                        }
                    }
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                    if (bcbddytx)
                        bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
                    else
                        bggs = bc_bggs + ".frf";
                    sJPGNAME = localpath + "\\" + fileName;

                }
                //�жϱ����ʽ�Ƿ����
                if (!File.Exists(sBGGSName))
                {
                    message = "�����ʽ������:" + sBGGSName;
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
                            pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print���");
                            fileName = sJPGNAME.Replace(".", "_1.");

                        }
                        else
                        {
                            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print���");
                            fileName = sJPGNAME;
                        }

                    }
                    catch (Exception e3)
                    {
                        message = "����pdf�쳣������prreport�쳣:" + e3.Message; rtn = false;
                    }

                    if (!File.Exists(fileName))
                    {
                        message = "PDF_JPG-����δ�ҵ��ļ�1";
                        rtn = false;
                        continue;
                    }
                    else
                    {
                        message = "";
                        rtn = true;
                        break;
                    }

                }
                return rtn;

            }
            catch (Exception e4)
            {
                message = "PDF_JPG�����쳣:" + e4.Message;
                return false;
            }
        }

        public bool downtx(string blh, string txml, DataTable dt_tx, ref string txlbs, ref string localpath, ref string err_msg)
        {
            //���c:\temp_srĿ¼
            if (!System.IO.Directory.Exists(@"c:\temp\" + blh))
            {
                System.IO.Directory.CreateDirectory(@"c:\temp\" + blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    System.IO.Directory.CreateDirectory(@"c:\temp\" + blh);
                }
                catch
                {
                }
            }
            if (localpath == "")
                localpath = @"c:\temp\" + blh;
            else
                localpath = localpath + blh;

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
                            {
                                break;
                            }
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
                                {

                                }

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
        public bool downtx2(string blh, string txml, DataTable dt_tx, ref string txlbs, ref string localpath, ref string err_msg)
        {
            //���c:\temp_srĿ¼
            if (localpath == "")
                localpath = @"c:\temp\";
            if (!System.IO.Directory.Exists(localpath + blh))
            {
                System.IO.Directory.CreateDirectory(localpath + blh);
            }
            else
            {
                try
                {
                    System.IO.Directory.Delete(localpath + blh, true);
                    System.IO.Directory.CreateDirectory(localpath + blh);
                }
                catch
                {
                }
            }

            localpath = localpath + blh;

            //����FTP����
            string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = ZgqClass.GetSz("ftp", "pwd", "4s3c2a1p").Replace("\0", "");
            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");

            //����Ŀ¼
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");
            string txm = "";
            string ftpURI = "";
            if (ftps == "1")//FTP���ط�ʽ
            {
                if (ftpremotepath != "")
                {
                    ftpURI = "ftp://" + ftpserver + "/" + ftpremotepath + "/" + txml + "/";
                }
                else
                {
                    ftpURI = "ftp://" + ftpserver + "/" + txml + "/";
                }

                for (int i = 0; i < dt_tx.Rows.Count; i++)
                {
                    txm = dt_tx.Rows[i]["F_txm"].ToString().Trim();
                    string ftpstatus = "";
                    try
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            string ftpPath = ftpURI + txm;
                            if (ZgqFtpWeb.FtpDownload(ftpuser, ftppwd, ftpPath, localpath + "/" + txm, ref err_msg))
                            {
                                break;
                            }
                        }
                        if (!File.Exists(localpath + "/" + txm))
                        {
                            log.WriteMyLog("FTP����ͼ�����");
                            err_msg = "FTP����ͼ�����-->" + txm;
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
                                {
                                }

                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txm + "</Image>";

                        }
                    }
                    catch (Exception e1)
                    {
                        log.WriteMyLog("FTP����ͼ�����");
                        err_msg = "FTP����ͼ�����-->" + e1.Message;
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
                                File.Copy(txpath + txml + "\\" + txm, localpath + "\\" + txm, true);
                                if (File.Exists(localpath + "\\" + txm))
                                    break;
                            }
                            if (!File.Exists(localpath + "\\" + txm))
                            {
                                log.WriteMyLog("��������ͼ�����");
                                err_msg = "��������ͼ�����" + txm;
                                return false;
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txm + "</Image>";
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
        public bool UpPDF(string blh, string jpgpath, string ml, int lb, ref string err_msg, ref string pdfpath)
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
                string tjtxpath = ZgqClass.GetSz("savetohis", "PDFPath", "");
                string debug = ZgqClass.GetSz("savetohis", "debug", "0");
                string txpath = ZgqClass.GetSz("txpath", "txpath", @"E:\pathimages\");


                if (lb == 0)
                {
                    if (ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "").Trim() == "1")
                        lb = 3;
                    else
                    {
                        lb = 1;
                    }
                }
                if (lb == 1)
                {
                    ftps = "0";
                    if (tjtxpath == "")
                        tjtxpath = txpath + "pdfbg";
                    else
                        tjtxpath = txpath + tjtxpath;
                }

                if (lb == 3)
                {
                    ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
                    ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
                    ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                    ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                    ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
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

                        string ftpURI = @"ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                        try
                        {

                            if (debug == "1")
                                log.WriteMyLog("���ftpĿ¼������");

                            //�յ�����Ŀ¼
                            if (ml.Trim() != "")
                            {
                                //�ж�Ŀ¼�Ƿ����
                                if (!ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword, ftpURI, ml))
                                {
                                    //Ŀ¼�����ڣ�����

                                    if (!ZgqFtpWeb.FtpMakedir(ftpUserID, ftpPassword, ftpURI, ml, ref  err_msg))
                                    {
                                        err_msg = "FTP����Ŀ¼�쳣";
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

                                if (!ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword, ftpURI, blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";

                                    if (!ZgqFtpWeb.FtpMakedir(ftpUserID, ftpPassword, ftpURI, blh, ref  err_msg))
                                    {
                                        err_msg = "FTP����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI + blh + "/";
                            }


                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ�");
                            for (int x = 0; x < 3; x++)
                            {

                                if (ZgqFtpWeb.FtpUpload(ftpUserID, ftpPassword, ftpURI, jpgpath, ref err_msg))
                                    break;
                            }
                            if (debug == "1")
                                log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + err_msg);
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            try
                            {
                                if (ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword, ftpURI, jpgname))
                                {
                                    status = "OK";
                                    pdfpath = ftpURI + jpgname;
                                    return true;
                                }
                                else
                                {
                                    err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
                                    status = "Error";
                                    return false;
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //�ж�mlĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + @"\" + ml))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + @"\" + ml);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + @"\" + ml + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + @"\" + ml;
                            }
                            if (blh.Trim() != "")
                            {
                                //�ж�blhĿ¼�Ƿ����
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + blh))
                                {
                                    //Ŀ¼�����ڣ�����
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + blh + "--����Ŀ¼�쳣";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + blh;
                            }

                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //ɾ�������ϵ�pdf�ļ�
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //�жϹ������Ƿ���ڸ�pdf�ļ�
                            for (int x = 0; x < 3; x++)
                            {
                                File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                                if (File.Exists(tjtxpath + "\\" + jpgname))
                                    break;
                            }
                            ;
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                status = "OK";
                                pdfpath = tjtxpath + "\\" + jpgname;
                            }
                            else
                            {
                                status = "";
                                err_msg = "�ϴ�PDF�쳣";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
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
                    err_msg = "δ�ҵ��ļ�" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF�����쳣��" + e4.Message;
                return false;
            }

        }
        public void DeleteTempFile(string blh)
        {
            try
            {
                System.IO.Directory.Delete(@"c:\temp\" + blh, true);
            }
            catch
            {
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

    class ZgqFtpWeb
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
    //class ZGQ_PDFJPG
    //{
    //    private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

    //    /// <summary>
    //    /// ����pdf��jpg
    //    /// </summary>
    //    /// <param name="F_blh">�����</param>
    //    /// <param name="bglx">��������</param>
    //    /// <param name="bgxh">�������</param>
    //    /// <param name="type1">JPG��PDF</param>
    //    /// <param name="err_msg">������Ϣ</param>
    //    /// <param name="F_ML">Ŀ¼</param>
    //    /// <returns></returns>
    //    public bool SC_PDF(string blh, string bgxh, string bglx, type type1, ref string errmsg, ref string pdfpath, ref string filename, string debug)
    //    {
    //        string blbh = blh + bgxh + bgxh;
    //        try
    //        {
    //            filename = "";
    //            if (bglx == "")
    //                bglx = "cg";
    //            if (bgxh == "")
    //                bgxh = "1";
    //            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
    //            DataTable jcxx = new DataTable();
    //            try
    //            {
    //                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
    //            }
    //            catch (Exception ex)
    //            {
    //                errmsg = ("�������ݿ��쳣:" + ex.Message);
    //                return false;
    //            }
    //            if (jcxx == null)
    //            {
    //                errmsg = "�������ݿ����������⣡";
    //                return false;
    //            }
    //            if (jcxx.Rows.Count < 1)
    //            {
    //                errmsg = "������д���";
    //                return false;
    //            }

    //            DataTable dt_bd = new DataTable();
    //            DataTable dt_bc = new DataTable();
    //            string bgzt = "";

    //            filename = "";
    //            if (bglx.ToLower() == "bd")
    //            {
    //                dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
    //                bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
    //                filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
    //            }
    //            if (bglx.ToLower() == "bc")
    //            {
    //                dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
    //                bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
    //                filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
    //            }
    //            if (bglx.ToLower() == "cg")
    //            {
    //                bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
    //                filename = jcxx.Rows[0]["F_SPARE5"].ToString();
    //            }

    //            if (bgzt == "�����" || bgzt == "�ѷ���")
    //            {
    //                if (filename.Trim() == "")
    //                {
    //                    errmsg = "���ڲ���Ϊ��";
    //                    return false;
    //                }

    //                if (type1.ToString().ToLower() == "pdf")
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
    //                else
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";

    //                string rptpath = ZgqClass.GetSz("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();

    //                //����PDF
    //                string msg = "";

    //                bool pdf1 = CreatePDF(blh, bglx, bgxh, ref filename, rptpath.Trim(), type1, ref msg);

    //                if (!pdf1)
    //                {
    //                    errmsg = msg;
    //                    return false;
    //                }
    //                if (File.Exists(filename))
    //                {
    //                    string ml = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
    //                    ZgqClass.BGHJ(blh, "����PDF", "���", "����PDF�ɹ�", "ZGQJK", "����PDF");
    //                    bool ssa = UpPDF(blh, filename, ml, 0, ref errmsg, ref pdfpath);
    //                    if (ssa == true)
    //                    {
    //                        if (debug == "1")
    //                            log.WriteMyLog("�ϴ�PDF�ɹ�");
    //                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
    //                        ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDF�ɹ�:" + ml + "\\" + filename, "ZGQJK", "�ϴ�PDF");

    //                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
    //                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_pdfPath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" + blh + "','" + filename + "','" + pdfpath + "')");
    //                    }
    //                    else
    //                    {
    //                        log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + errmsg);
    //                        ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDFʧ�ܣ�" + errmsg, "ZGQJK", "�ϴ�PDF");
    //                    }
    //                    DeleteTempFile(blh);
    //                    return true;
    //                }
    //                else
    //                {
    //                    ZgqClass.BGHJ(blh, "����PDF", "���", "����PDFʧ�ܣ�δ�ҵ��ļ�" + filename, "ZGQJK", "����PDF");
    //                    log.WriteMyLog("δ�ҵ��ļ�" + filename);
    //                    errmsg = "δ�ҵ��ļ�" + filename;
    //                    DeleteTempFile(blh);
    //                    return false;
    //                }

    //            }
    //            else
    //            {
    //                errmsg = "����δ���";
    //                return false;
    //            }
    //        }
    //        catch (Exception e4)
    //        {
    //            errmsg = "SC_PDF�����쳣��" + e4.Message;
    //            return false;
    //        }

    //    }

    //    public bool C_PDF(string blh, string bglx, string bgxh, string ml, type type1, ref string err_msg, ref string jpgname)
    //    {
    //        try
    //        {
    //            string filename = "";
    //            if (bglx == "")
    //                bglx = "cg";
    //            if (bgxh == "")
    //                bgxh = "1";
    //            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
    //            DataTable jcxx = new DataTable();
    //            try
    //            {
    //                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
    //            }
    //            catch (Exception ex)
    //            {
    //                err_msg = ("�������ݿ��쳣:" + ex.Message);
    //                return false;
    //            }
    //            if (jcxx == null)
    //            {
    //                err_msg = "�������ݿ����������⣡";
    //                return false;
    //            }
    //            if (jcxx.Rows.Count < 1)
    //            {
    //                err_msg = "������д���";
    //                return false;
    //            }

    //            DataTable dt_bd = new DataTable();
    //            DataTable dt_bc = new DataTable();
    //            string bgzt = "";

    //            filename = "";
    //            if (bglx.ToLower() == "bd")
    //            {
    //                dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");

    //                bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
    //                filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
    //            }
    //            if (bglx.ToLower() == "bc")
    //            {
    //                dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
    //                bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
    //                filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
    //            }
    //            if (bglx.ToLower() == "cg")
    //            {
    //                bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
    //                filename = jcxx.Rows[0]["F_SPARE5"].ToString();
    //            }




    //            if (bgzt == "�����" || bgzt == "�ѷ���")
    //            {
    //                if (filename.Trim() == "")
    //                {
    //                    err_msg = "���ڲ���Ϊ��";
    //                    return false;
    //                }

    //                if (type1.ToString().ToLower() == "pdf")
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
    //                else
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";


    //                string rptpath = ZgqClass.GetSz("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();
    //                jpgname = "";

    //                //����PDF
    //                string msg = "";

    //                bool pdf1 = CreatePDF(blh, bglx, bgxh, ref filename, rptpath.Trim(), type1, ref msg);

    //                if (!pdf1)
    //                {
    //                    err_msg = msg;
    //                    return false;
    //                }
    //                if (File.Exists(jpgname))
    //                    return true;
    //                else
    //                {
    //                    err_msg = "δ�ҵ�PDF�ļ�";
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                err_msg = "����δ���";
    //                return false;
    //            }
    //        }
    //        catch (Exception e4)
    //        {
    //            err_msg = "CreatePDF�����쳣��" + e4.Message;
    //            return false;
    //        }
    //    }


    //    public enum type { JPG, PDF };
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="F_blh"></param>
    //    /// <param name="bglx"></param>
    //    /// <param name="bgxh"></param>
    //    /// <param name="jpgname"></param>
    //    /// <param name="rptpath"></param>
    //    /// <param name="type1"></param>
    //    /// <param name="filename"></param>
    //    /// <param name="message"></param>
    //    /// <returns></returns>

    //    public bool CreatePDF(string F_blh, string bglx, string bgxh, ref string filename, string rptpath, type type1, ref  string message)
    //    {

    //        bool rtn = false;
    //        try
    //        {
    //            string status = "";
    //            if (bglx == "")
    //                bglx = "cg";
    //            if (bgxh == "")
    //                bgxh = "1";
    //            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
    //            DataTable jcxx = new DataTable();
    //            try
    //            {
    //                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
    //            }
    //            catch (Exception ex)
    //            {
    //                message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
    //                return false;
    //            }

    //            //���c:\tempĿ¼
    //            if (!System.IO.Directory.Exists(@"c:\temp\" + F_blh))
    //            {
    //                System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
    //                    System.IO.Directory.CreateDirectory(@"c:\temp\" + F_blh);
    //                }
    //                catch
    //                {
    //                }
    //            }

    //            DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");
    //            string txlbs = "";
    //            string localpath = "";
    //            message = "";

    //            if (!downtx(F_blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), aa, ref txlbs, ref localpath, ref message))
    //            {

    //                log.WriteMyLog("����ͼƬʧ�ܣ�" + message);
    //                return false;
    //            }

    //            for (int i = 0; i < txlb.Rows.Count; i++)
    //            {
    //                if (File.Exists(localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
    //                {
    //                    filename = "";
    //                    message = "����ͼ�����δ�ҵ�ͼƬ��" + localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString();
    //                    return false;
    //                }
    //            }

    //            if (localpath == "")
    //            {
    //                filename = "";
    //                message = "PDF_JPG��������ͼ�����";
    //                return false;
    //            }

    //            string sbmp = "";
    //            string stxsm = "";
    //            string sJPGNAME = localpath + "\\" + filename;

    //            //   string sJPGNAME = localpath + "\\" + F_blh + ".jpg";
    //            string rptpath2 = "rpt";
    //            if (rptpath.Trim() != "")
    //                rptpath2 = rptpath;

    //            string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //            string inibglj = ZgqClass.GetSz("dybg", "dybglj", "").Replace("\0", "");

    //            if (inibglj.Trim() == "")
    //            {
    //                inibglj = Application.StartupPath.ToString();
    //            }
    //            if (inibglj != "")
    //            {
    //                sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //            }

    //            for (int i = 0; i < txlb.Rows.Count; i++)
    //            {
    //                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
    //                sbmp = sbmp + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
    //            }
    //            if (ZgqClass.GetSz("rpt", "szqm", "0") == "1" && bglx.ToLower() == "cg")
    //            {
    //                //  string bmppath = ZgqClass.GetSz("mdbmp", "ysbmp", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");

    //                string bmppath = ZgqClass.GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
    //                stxsm = stxsm + " ,";


    //                string yszmlb = ZgqClass.GetSz("All", "yszmlb", "f_shys");
    //                bool bj = true;
    //                foreach (string ysname in yszmlb.Split(','))
    //                {
    //                    if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj == true)
    //                    {
    //                        if (ZgqClass.GetSz("rpt", "bgys2shys", "1") == "1")
    //                        {
    //                            if (jcxx.Rows[0]["F_shys"].ToString().Trim() == jcxx.Rows[0]["F_bgys"].ToString().Trim())
    //                                bj = false;
    //                        }

    //                        if (ysname.ToLower().Trim() == "f_shys")
    //                        {
    //                            sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
    //                        }

    //                        if (ysname.ToLower().Trim() == "f_bgys")
    //                        {
    //                            foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
    //                            {
    //                                if (name.Trim() != "")
    //                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                            }
    //                        }
    //                        // }
    //                    }




    //                    if (ysname.ToLower().Trim() == "f_fzys")
    //                    {
    //                        foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
    //                        {
    //                            if (name.Trim() != "")
    //                                sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                        }
    //                    }
    //                }


    //                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //                if (inibglj != "")
    //                {
    //                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //                }
    //            }

    //            string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //            //  string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

    //            string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
    //            bool bcbddytx = false;
    //            if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
    //                bcbddytx = true;

    //            if (bglx.ToLower() == "bd")
    //            {
    //                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\����.frf";
    //                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
    //                if (bcbddytx)
    //                    bggs = "����" + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //                else
    //                    bggs = "����.frf";
    //                if (inibglj != "")
    //                {
    //                    sBGGSName = inibglj + "\\" + rptpath2 + "\\����.frf";
    //                }
    //                sJPGNAME = localpath + "\\" + filename;
    //            }
    //            if (bglx.ToLower() == "bc")
    //            {
    //                DataTable BCBG = new DataTable();
    //                try
    //                {
    //                    BCBG = aa.GetDataTable("select * from T_BCBG  where F_blh='" + F_blh + "'  and F_BC_BGXH='" + bgxh + "'", "bcbg");
    //                }
    //                catch (Exception ex)
    //                {
    //                    message = "��ѯ���ݿ��쳣��" + ex.Message.ToString();
    //                    return false;
    //                }


    //                string bc_bggs = "����";

    //                try
    //                {
    //                    if (BCBG.Rows.Count > 0)
    //                    {
    //                        try
    //                        {
    //                            bc_bggs = BCBG.Rows[0]["F_BC_BGGS"].ToString().Trim();
    //                        }
    //                        catch
    //                        {
    //                            bc_bggs = "����";
    //                        }
    //                    }
    //                }
    //                catch
    //                {
    //                }
    //                if (bc_bggs.Trim() == "")
    //                    bc_bggs = "����";

    //                if (bcbddytx)
    //                    bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ";


    //                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";

    //                if (inibglj != "")
    //                {


    //                    sBGGSName = inibglj + "\\" + rptpath2 + "\\" + bc_bggs + ".frf";
    //                }

    //                if (ZgqClass.GetSz("rpt", "bcbgszqm", "0") == "1")
    //                {
    //                    string bmppath = ZgqClass.GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
    //                    stxsm = stxsm + " ,";


    //                    string yszmlb = ZgqClass.GetSz("All", "yszmlb", "f_shys");



    //                    bool bj2 = true;
    //                    foreach (string ysname in yszmlb.Split(','))
    //                    {

    //                        if ((ysname.ToLower().Trim() == "f_shys" || ysname.ToLower().Trim() == "f_bgys") && bj2 == true)
    //                        {
    //                            if (ZgqClass.GetSz("rpt", "bgys2shys", "1") == "1")
    //                            {
    //                                if (BCBG.Rows[0]["F_bc_shys"].ToString().Trim() == BCBG.Rows[0]["F_bc_bgys"].ToString().Trim())
    //                                    bj2 = false;
    //                            }
    //                            if (ysname.ToLower().Trim() == "f_shys")
    //                            {
    //                                sbmp = sbmp + bmppath + "\\" + BCBG.Rows[0]["F_bc_shys"].ToString().Trim() + ".bmp,";
    //                            }
    //                            if (ysname.ToLower().Trim() == "f_bgys")
    //                            {
    //                                foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
    //                                {

    //                                    if (name.Trim() != "")
    //                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                                }
    //                            }
    //                        }

    //                        if (ysname.ToLower().Trim() == "f_fzys")
    //                        {
    //                            foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('��', '/').Split('/'))
    //                            {
    //                                if (name.Trim() != "")
    //                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                            }
    //                        }
    //                    }

    //                    // sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
    //                    if (bcbddytx)
    //                        sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //                    else
    //                        sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + ".frf";
    //                    if (inibglj != "")
    //                    {
    //                        if (bcbddytx)
    //                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //                        else
    //                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + ".frf";

    //                    }
    //                }


    //                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
    //                if (bcbddytx)
    //                    bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "ͼ.frf";
    //                else
    //                    bggs = bc_bggs + ".frf";
    //                sJPGNAME = localpath + "\\" + filename;

    //            }
    //            //�жϱ����ʽ�Ƿ����
    //            if (!File.Exists(sBGGSName))
    //            {
    //                message = "�����ʽ������:" + sBGGSName;
    //                return false;
    //            }
    //            string debug = f.ReadString("savetohis", "debug2", "");
    //            for (int x = 0; x < 3; x++)
    //            {

    //                prreport pr = new prreport();
    //                try
    //                {

    //                    if (type1.ToString().Trim().ToLower() == "jpg")
    //                    {
    //                        pr.print(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
    //                        if (debug == "1")
    //                            log.WriteMyLog("pr.print���");
    //                        filename = sJPGNAME.Replace(".", "_1.");

    //                    }
    //                    else
    //                    {
    //                        //if (debug == "1")
    //                        //    log.WriteMyLog("pr.print��ʼ\r\n" + sSQL_DY + "\r\n" + sbmp + "\r\n" + stxsm + "\r\n" + sBGGSName + "\r\n" + sJPGNAME);
    //                        pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
    //                        if (debug == "1")
    //                            log.WriteMyLog("pr.print���");
    //                        filename = sJPGNAME;
    //                    }

    //                }
    //                catch (Exception e3)
    //                {
    //                    message = "����pdf�쳣������prreport�쳣:" + e3.Message; rtn = false;
    //                }

    //                if (!File.Exists(filename))
    //                {
    //                    message = "PDF_JPG-����δ�ҵ��ļ�1";
    //                    rtn = false;
    //                    continue;
    //                }
    //                else
    //                {
    //                    message = "";
    //                    rtn = true;
    //                    break;
    //                }

    //            }
    //            return rtn;

    //        }
    //        catch (Exception e4)
    //        {
    //            message = "PDF_JPG�����쳣:" + e4.Message;
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// ����
    //    /// </summary>
    //    /// <param name="f_blh">�����</param>
    //    /// <param name="txml">ͼ��Ŀ¼</param>
    //    /// <param name="aa"></param>
    //    /// <param name="txlbs"></param>
    //    /// <param name="localpath">��ʱĿ¼</param>
    //    /// <param name="err_msg">������Ϣ</param>
    //    /// <returns>�ɹ�����ʧ��</returns>
    //    public bool downtx(string f_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath, ref string err_msg)
    //    {

    //        try
    //        {
    //            //���c:\tempĿ¼
    //            if (!System.IO.Directory.Exists(@"c:\temp\" + f_blh))
    //                System.IO.Directory.CreateDirectory(@"c:\temp\" + f_blh);
    //            else
    //            {
    //                try
    //                {
    //                    System.IO.Directory.Delete(@"c:\temp\" + f_blh, true);
    //                    System.IO.Directory.CreateDirectory(@"c:\temp\" + f_blh);
    //                }
    //                catch (Exception e1)
    //                {
    //                }
    //            }
    //            //��ʱĿ¼
    //            localpath = @"c:\temp\" + f_blh;

    //            //����FTP����
    //            string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
    //            string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
    //            string ftppwd = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
    //            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
    //            string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
    //            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
    //            string txpath = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");

    //            ZGQ_FTP zgqftp = new ZGQ_FTP();

    //            //FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
    //            //����Ŀ¼
    //            string gxml = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");
    //            //string gxuid = ZgqClass.GetSz("txpath", "username", "").Replace("\0", "");
    //            //string gxpwd = ZgqClass.GetSz("txpath", "password", "").Replace("\0", "");


    //            DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + f_blh + "' and F_sfdy='1'", "txlb");
    //            string txm = "";

    //            if (ftps == "1")//FTP���ط�ʽ
    //            {

    //                for (int i = 0; i < txlb.Rows.Count; i++)
    //                {

    //                    txm = txlb.Rows[i]["F_txm"].ToString().Trim();
    //                    string ftpstatus = "";
    //                    try
    //                    {
    //                        err_msg = "";

    //                        for (int x = 0; x < 3; x++)
    //                        {  
                                
    //                            if (ftpstatus != "Error")
    //                                break;
    //                        }

    //                        if (ftpstatus == "Error")
    //                        {
    //                            localpath = "";
    //                            return false;
    //                        }
    //                        else
    //                        {

    //                            if (f.ReadInteger("TX", "ZOOM", 1) == 1)
    //                            {

    //                                int picx = f.ReadInteger("TX", "picx", 320);
    //                                int picy = f.ReadInteger("TX", "picy", 240);
    //                                try
    //                                {

    //                                    prreport.txzoom(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);

    //                                }
    //                                catch (Exception ee2)
    //                                {
    //                                    log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
    //                                }

    //                            }
    //                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";

    //                        }
    //                    }
    //                    catch (Exception ee2)
    //                    {
    //                        err_msg = "FTP����ͼ�����:" + ee2.Message;
    //                        localpath = "";
    //                        return false;
    //                    }
    //                }

    //                return true;
    //            }
    //            else //�������ط�ʽ
    //            {
    //                if (txpath == "")
    //                {
    //                    err_msg = "sz.ini txpathͼ��Ŀ¼δ����";
    //                    localpath = "";
    //                    return false;
    //                }
    //                for (int i = 0; i < txlb.Rows.Count; i++)
    //                {
    //                    txm = txlb.Rows[i]["F_txm"].ToString().Trim();

    //                    try
    //                    {
    //                        for (int x = 0; x < 3; x++)
    //                        {
    //                            File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
    //                            if (File.Exists(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim()))
    //                                break;
    //                        }
    //                        if (File.Exists(localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim()))
    //                        {
    //                            if (f.ReadInteger("TX", "ZOOM", 1) == 1)
    //                            {
    //                                int picx = f.ReadInteger("TX", "picx", 320);
    //                                int picy = f.ReadInteger("TX", "picy", 240);
    //                                try
    //                                {
    //                                    prreport.txzoom(localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), localpath + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), picx, picy);
    //                                }
    //                                catch (Exception ee2)
    //                                {
    //                                    log.WriteMyLog("ѹ��ͼ���쳣��" + ee2.Message);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            err_msg = "��������ͼ��ʧ��";
    //                            return false;
    //                        }
    //                        txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
    //                    }

    //                    catch (Exception ee3)
    //                    {
    //                        err_msg = "��������ͼ�����:" + ee3.Message;
    //                        localpath = "";
    //                        return false;
    //                    }
    //                }
    //                return true;
    //            }
    //        }
    //        catch (Exception e4)
    //        {

    //            err_msg = "����ͼ���쳣:" + e4.Message;
    //            return false;
    //        }

    //    }

    //    /// 
    //    /// <summary>
    //    /// �ϴ�
    //    /// </summary>
    //    /// <param name="blh">�����</param>
    //    /// <param name="jpgpath">�ļ���������·��</param>
    //    /// <param name="ml">Ŀ¼</param>
    //    /// <param name="err_msg">������Ϣ</param>
    //    /// <param name="lb">1:ָ������Ŀ¼�ϴ���[ZGQJK]toPDFPath=)��2:����·���ϴ�([txpath]txpath=)��3:ftp�ϴ�([ftp]);����ftp�ϴ�([ftpup])  </param>
    //    /// <returns></returns>
    //    /// 
    //    public bool UpPDF(string blh, string jpgpath, string ml, int lb, ref string err_msg, ref string pdfpath)
    //    {
    //        try
    //        {
    //            string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
    //            //---�ϴ�jpg----------
    //            //----------------�ϴ���ftp---------------------
    //            string status = "";
    //            string ftps = string.Empty;
    //            string ftpServerIP = string.Empty;
    //            string ftpUserID = string.Empty; ;
    //            string ftpPassword = string.Empty;
    //            string ftplocal = string.Empty;
    //            string ftpRemotePath = string.Empty;
    //            string tjtxpath = ZgqClass.GetSz("savetohis", "PDFPath", "");
    //            string debug = ZgqClass.GetSz("savetohis", "debug", "0");
    //            string txpath = ZgqClass.GetSz("txpath", "txpath", @"E:\pathimages\");


    //            if (lb == 0)
    //            {
    //                if (ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "").Trim() == "1")
    //                    lb = 3;
    //                else
    //                {
    //                    lb = 1;
    //                }

    //            }
    //            if (lb == 1)
    //            {
    //                ftps = "0";
    //                if (tjtxpath == "")
    //                    tjtxpath = txpath + "pdfbg";
    //                else
    //                    tjtxpath = txpath + tjtxpath;
    //            }

    //            if (lb == 3)
    //            {
    //                ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
    //                ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
    //                ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
    //                ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
    //                ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
    //                ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", @"pathimages/pdfbg").Replace("\0", "");
    //            }
    //            if (lb == 4)
    //            {
    //                ftps = ZgqClass.GetSz("ftpup", "ftp", "1").Replace("\0", "");
    //                ftpServerIP = ZgqClass.GetSz("ftpup", "ftpip", "").Replace("\0", "");
    //                ftpUserID = ZgqClass.GetSz("ftpup", "user", "ftpuser").Replace("\0", "");
    //                ftpPassword = ZgqClass.GetSz("ftpup", "pwd", "ftp").Replace("\0", "");
    //                ftplocal = ZgqClass.GetSz("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
    //                ftpRemotePath = ZgqClass.GetSz("ftpup", "PDFPath", "pathimages/pdfbg").Replace("\0", "");

    //            }
    //            if (File.Exists(jpgpath))
    //            {
    //                if (ftps == "1")
    //                {
    //                    ZGQ_FTP fw = new ZGQ_FTP();
    //                    string ftpURI = @"ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
    //                    try
    //                    {

    //                        if (debug == "1")
    //                            log.WriteMyLog("���ftpĿ¼������");

    //                        //�յ�����Ŀ¼
    //                        if (ml.Trim() != "")
    //                        {
    //                            //�ж�Ŀ¼�Ƿ����
    //                            if (!fw.fileCheckExist(ftpURI, ftpUserID, ftpPassword, ml))
    //                            {
    //                                //Ŀ¼�����ڣ�����
    //                                string stat = "";
    //                                fw.Makedir(ftpURI, ftpUserID, ftpPassword, ml, out stat);
    //                                if (stat != "OK")
    //                                {
    //                                    err_msg = "FTP����Ŀ¼�쳣";
    //                                    return false;
    //                                }
    //                            }

    //                            ftpURI = ftpURI + ml + "/";
    //                        }

    //                        //�����Ŀ¼
    //                        //�ж�Ŀ¼�Ƿ����
    //                        // MessageBox.Show("1--"+ftpURI);
    //                        if (blh.Trim() != "")
    //                        {
    //                            if (!fw.fileCheckExist(ftpURI, ftpUserID, ftpPassword, blh))
    //                            {
    //                                //Ŀ¼�����ڣ�����
    //                                string stat = "";

    //                                fw.Makedir(ftpURI, ftpUserID, ftpPassword, blh, out stat);

    //                                if (stat != "OK")
    //                                {
    //                                    err_msg = "FTP����Ŀ¼�쳣";
    //                                    return false;
    //                                }
    //                            }
    //                            ftpURI = ftpURI + blh + "/";
    //                        }


    //                        if (debug == "1")
    //                            log.WriteMyLog("�ϴ������ɵ��ļ�");
    //                        for (int x = 0; x < 3; x++)
    //                        {
    //                            fw.FtpUpload(ftpURI, ftpUserID, ftpPassword, jpgpath, out status, ref err_msg);
    //                            if (status != "Error")
    //                                break;
    //                        }
    //                        if (status == "Error")
    //                        {
    //                            err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
    //                            status = "Error";
    //                        }
    //                        if (debug == "1")
    //                            log.WriteMyLog("�ϴ������ɵ��ļ������" + status + "\r\n" + err_msg);
    //                        //�ж�ftp���Ƿ���ڸ�jpg�ļ�
    //                        try
    //                        {
    //                            if (fw.fileCheckExist(ftpURI, ftpUserID, ftpPassword, jpgname))
    //                            {
    //                                status = "OK";
    //                                pdfpath = ftpURI + jpgname;
    //                            }
    //                            else
    //                            {
    //                                err_msg = "PDF�ϴ�ʧ�ܣ���������ˣ�";
    //                                status = "Error";
    //                            }

    //                        }
    //                        catch (Exception err2)
    //                        {
    //                            err_msg = "�����ļ��Ƿ��ϴ��ɹ��쳣:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
    //                            status = "Error";
    //                            return false;
    //                        }
    //                    }
    //                    catch (Exception eee)
    //                    {
    //                        err_msg = "�ϴ�PDF�쳣:" + eee.Message.ToString();
    //                        status = "Error";
    //                        return false;
    //                    }
    //                }
    //                else
    //                {
    //                    if (tjtxpath == "")
    //                    {
    //                        err_msg = "sz.ini��[ZGQJK]��toPDFPathͼ��Ŀ¼δ����";
    //                        return false;
    //                    }
    //                    try
    //                    {
    //                        if (ml.Trim() != "")
    //                        {
    //                            //�ж�mlĿ¼�Ƿ����
    //                            if (!System.IO.Directory.Exists(tjtxpath + @"\" + ml))
    //                            {
    //                                //Ŀ¼�����ڣ�����
    //                                string stat = "";
    //                                try
    //                                {
    //                                    System.IO.Directory.CreateDirectory(tjtxpath + @"\" + ml);
    //                                }
    //                                catch
    //                                {
    //                                    err_msg = tjtxpath + @"\" + ml + "--����Ŀ¼�쳣";
    //                                    return false;
    //                                }
    //                            }
    //                            tjtxpath = tjtxpath + @"\" + ml;
    //                        }
    //                        if (blh.Trim() != "")
    //                        {
    //                            //�ж�blhĿ¼�Ƿ����
    //                            if (!System.IO.Directory.Exists(tjtxpath + "\\" + blh))
    //                            {
    //                                //Ŀ¼�����ڣ�����
    //                                string stat = "";
    //                                try
    //                                {
    //                                    System.IO.Directory.CreateDirectory(tjtxpath + "\\" + blh);
    //                                }
    //                                catch
    //                                {
    //                                    err_msg = tjtxpath + "\\" + blh + "--����Ŀ¼�쳣";
    //                                    return false;
    //                                }
    //                            }
    //                            tjtxpath = tjtxpath + "\\" + blh;
    //                        }

    //                        //�жϹ������Ƿ���ڸ�pdf�ļ�
    //                        if (File.Exists(tjtxpath + "\\" + jpgname))
    //                        {
    //                            //ɾ�������ϵ�pdf�ļ�
    //                            File.Delete(tjtxpath + "\\" + jpgname);
    //                        }
    //                        //�жϹ������Ƿ���ڸ�pdf�ļ�
    //                        for (int x = 0; x < 3; x++)
    //                        {
    //                            File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
    //                            if (File.Exists(tjtxpath + "\\" + jpgname))
    //                                break;
    //                        }
    //                        // Thread.Sleep(1000);
    //                        if (File.Exists(tjtxpath + "\\" + jpgname))
    //                        {
    //                            status = "OK";
    //                            pdfpath = tjtxpath + "\\" + jpgname;
    //                        }
    //                        else
    //                        {
    //                            status = "";
    //                            err_msg = "�ϴ�PDF�쳣";
    //                            return false;
    //                        }
    //                    }
    //                    catch (Exception ee3)
    //                    {
    //                        err_msg = "�ϴ��쳣:" + ee3.Message.ToString();
    //                        return false;
    //                    }
    //                }

    //                if (status == "OK")
    //                    return true;
    //                else
    //                    return false;
    //            }
    //            else
    //            {
    //                err_msg = "δ�ҵ��ļ�" + jpgpath + "";
    //                return false;
    //            }
    //        }
    //        catch (Exception e4)
    //        {
    //            err_msg = "UpPDF�����쳣��" + e4.Message;
    //            return false;
    //        }

    //    }

    //    /// <summary>
    //    /// ɾ��
    //    /// </summary>
    //    /// <param name="ml">Ŀ¼</param>
    //    /// <param name="jpgname">�ļ���</param>
    //    /// <param name="err_msg">������Ϣ</param>
    //    /// <returns></returns>
    //    public bool Delete(string ml, string jpgname, ref string err_msg)
    //    {

    //        string status = "";
    //        string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
    //        string ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
    //        string ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
    //        string ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
    //        string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
    //        string ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
    //        string tjtxpath = ZgqClass.GetSz("JK_ZGQ", "PDFPath", "e:\\pathimages\\jpgbg");

    //        if (ftps == "1")
    //        {
    //            FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

    //            string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";

    //            try
    //            {

    //                //�ж�ftp���Ƿ���ڸ�jpg�ļ�
    //                if (fw.fileCheckExist(ftpURI, jpgname))
    //                {
    //                    //ɾ��ftp�ϵ�jpg�ļ�
    //                    fw.fileDelete(ftpURI, jpgname).ToString();
    //                }
    //                return true;
    //            }
    //            catch (Exception eee)
    //            {
    //                err_msg = "ɾ��ftp��PDF�쳣:" + eee.Message;
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            if (tjtxpath == "")
    //            {
    //                err_msg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
    //                return false;
    //            }
    //            try
    //            {
    //                if (ml == "")
    //                    File.Delete(tjtxpath + "\\" + jpgname);
    //                else
    //                    File.Delete(tjtxpath + "\\" + ml + "\\" + jpgname);

    //                return true;
    //            }
    //            catch (Exception ee)
    //            {
    //                err_msg = "ɾ���ļ��쳣:" + ee.Message.ToString();
    //                return false;
    //            }

    //        }

    //    }

    //    public bool FTP_Delete(string ml, string jpgname, ref string err_msg)
    //    {

    //        string status = "";
    //        string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
    //        string ftpServerIP = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
    //        string ftpUserID = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
    //        string ftpPassword = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
    //        string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
    //        string ftpRemotePath = ZgqClass.GetSz("ftp", "PDFPath", "pathimages\\pdfbg").Replace("\0", "");
    //        string tjtxpath = ZgqClass.GetSz("JK_ZGQ", "PDFPath", "e:\\pathimages\\jpgbg");

    //        if (ftps == "1")
    //        {
    //            FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

    //            string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
    //            if (ml.Trim() != "")
    //                ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";
    //            try
    //            {

    //                //�ж�ftp���Ƿ���ڸ�jpg�ļ�
    //                if (fw.fileCheckExist(ftpURI, jpgname))
    //                {
    //                    //ɾ��ftp�ϵ�jpg�ļ�
    //                    fw.fileDelete(ftpURI, jpgname).ToString();
    //                }
    //                return true;
    //            }
    //            catch (Exception eee)
    //            {
    //                err_msg = "ɾ��ftp��PDF�쳣:" + eee.Message;
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            if (tjtxpath == "")
    //            {
    //                err_msg = "sz.ini��[savetohis]��PDFPathͼ��Ŀ¼δ����";
    //                return false;
    //            }
    //            try
    //            {
    //                if (ml == "")
    //                    File.Delete(tjtxpath + "\\" + jpgname);
    //                else
    //                    File.Delete(tjtxpath + "\\" + ml + "\\" + jpgname);

    //                return true;
    //            }
    //            catch (Exception ee)
    //            {
    //                err_msg = "ɾ���ļ��쳣:" + ee.Message.ToString();
    //                return false;
    //            }

    //        }

    //    }

    //    public void DeleteTempFile(string blh)
    //    {
    //        try
    //        {
    //            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    /// <summary>
    //    /// ��ձ�����ʱ�ļ�
    //    /// </summary>
    //    /// <param name="blh">�����</param>
    //    /// <returns></returns>

    //}

    //class ZGQ_FTP
    //{
    //    public void Makedir(string filePath, string ftpUserID, string ftpPassword, string dirname, out string status)
    //    {
    //        status = "OK";
    //        string uri = filePath + dirname;
    //        FtpWebRequest reqFTP;
    //        reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
    //        reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
    //        reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
    //        try
    //        {
    //            FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
    //        }
    //        catch (Exception ex)
    //        {
    //            log.WriteMyLog("Error --> " + uri + "-->" + ex.Message);
    //            status = "Error";
    //        }

    //    }

    //    public void FtpUpload(string ftpURI, string ftpUserID, string ftpPassword, string filename, out string status, ref string msg)
    //    {
    //        status = "OK";
    //        FileInfo fileInf = new FileInfo(filename);
    //        string uri = ftpURI + fileInf.Name;
    //        FtpWebRequest reqFTP;
    //        try
    //        {

    //            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

    //            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
    //            reqFTP.KeepAlive = false;
    //            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

    //            reqFTP.UseBinary = true;

    //            reqFTP.ContentLength = fileInf.Length;

    //            int buffLength = 2048;

    //            byte[] buff = new byte[buffLength];

    //            int contentLen;

    //            FileStream fs = fileInf.OpenRead();


    //            Stream strm = reqFTP.GetRequestStream();

    //            contentLen = fs.Read(buff, 0, buffLength);

    //            while (contentLen != 0)
    //            {

    //                strm.Write(buff, 0, contentLen);

    //                contentLen = fs.Read(buff, 0, buffLength);

    //            }

    //            strm.Close();

    //            fs.Close();

    //        }

    //        catch (Exception ex)
    //        {

    //            log.WriteMyLog("Upload Error -->" + uri + "-->" + ex.Message);
    //            status = "Error";
    //            msg = "Upload Error --> " + uri + "-->" + ex.Message;
    //        }

    //    }

    //    public void Download(string ftpURI, string ftpUserID, string ftpPassword, string fileName, string localFilePath, string localName, out string status, ref string err_msg)
    //    {
    //        status = "OK";
    //        FtpWebRequest reqFTP;
    //        try
    //        {
    //            FileStream outputStream = new FileStream(localFilePath + "\\" + localName, FileMode.Create);

    //            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
    //            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
    //            reqFTP.UseBinary = true;
    //            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
    //            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
    //            Stream ftpStream = response.GetResponseStream();
    //            long cl = response.ContentLength;
    //            int bufferSize = 2048;
    //            int readCount;
    //            byte[] buffer = new byte[bufferSize];

    //            readCount = ftpStream.Read(buffer, 0, bufferSize);
    //            while (readCount > 0)
    //            {
    //                outputStream.Write(buffer, 0, readCount);
    //                readCount = ftpStream.Read(buffer, 0, bufferSize);
    //            }

    //            ftpStream.Close();
    //            outputStream.Close();
    //            response.Close();

    //        }
    //        catch (Exception ex)
    //        {

    //            err_msg = "Download Error -->" + localFilePath + "\\" + localName + "-->" + ex.Message;
    //            status = "Error";
    //        }
    //    }

    //    /// <summary>
    //    /// �ļ����ڼ��
    //    /// </summary>
    //    /// <param name="ftpPath"></param>
    //    /// <param name="ftpName"></param>
    //    /// <returns></returns>
    //    public bool fileCheckExist(string ftpPath, string ftpUserID, string ftpPassword, string ftpName)
    //    {

    //        string url = ftpPath;

    //        bool success = false;
    //        FtpWebRequest ftpWebRequest = null;
    //        WebResponse webResponse = null;
    //        StreamReader reader = null;
    //        try
    //        {
    //            ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
    //            ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
    //            ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
    //            ftpWebRequest.KeepAlive = false;
    //            webResponse = ftpWebRequest.GetResponse();
    //            reader = new StreamReader(webResponse.GetResponseStream());
    //            string line = reader.ReadLine();
    //            while (line != null)
    //            {
    //                if (line == ftpName)
    //                {
    //                    success = true;
    //                    break;
    //                }
    //                line = reader.ReadLine();
    //            }
    //        }
    //        catch (Exception ee)
    //        {
    //            log.WriteMyLog(ee.Message);
    //            success = false;
    //        }
    //        finally
    //        {
    //            try
    //            {
    //                if (reader != null)
    //                {
    //                    reader.Close();
    //                }
    //                if (webResponse != null)
    //                {
    //                    webResponse.Close();
    //                }
    //            }
    //            catch
    //            {
    //                //  log.WriteMyLog("�ر��������쳣");
    //            }
    //        }
    //        return success;

    //    }
    //}

}
