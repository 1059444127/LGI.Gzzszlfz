using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LGHISJKZGQ;
using dbbase;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace LGHISJKZGQ
{
    class ZGQClass
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        /// <summary>
        /// ȡsz�����ݿ�������
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
        /// <returns>�趨ֵ</returns>
        /// 
        public static string getSZ_String(string Section, string Ident)
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
                        return szvalue;
                    }
                    else
                    {
                        T_szvalue = DT_sz.Rows[0]["F_SZZ"].ToString().Trim();
                        return T_szvalue;
                    }
                }
                catch 
                {
                    return szvalue;
                }
            }
            else
                return szvalue;

        }
       /// <summary>
       /// ȡsz�����ݿ�������
       /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
       /// <param name="Default">Ĭ��ֵ</param>
       /// <returns>�趨ֵ</returns>
        public static string getSZ_String(string Section, string Ident,string Default)
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
                        T_szvalue = DT_sz.Rows[0]["F_SZZ"].ToString().Trim();
                            return T_szvalue;
                    }
                }
                catch
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
                aa.ExecuteSQL("insert into  T_BGHJ(F_BLH,F_RQ,F_CZY,F_WZ,F_DZ,F_NR,F_EXEMC,F_CTMC) values('" + blh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "','"
                    + yhmc + "','��������" + Dns.GetHostName().Trim() + ",IP ��ַ��" + addr.ToString() +"',','"+DZ+"','" + NR + "','"+EXEMC+"','" +CTMC + "') ");
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
                                    TimeSpan tp = DateTime.Now - DateTime.ParseExact(CSRQ.Trim(), "yyyyMMdd", Thread.CurrentThread.CurrentCulture);
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

        public static string  CsrqToAge(string  csrq,int lx)
        {

                if (csrq.Trim() == "")
                   return "";
                    try
                    {
                    string CSRQ =DateTime.Parse(csrq.Trim()).ToString("yyyyMMdd");
                  return   ToAge(CSRQ,lx);
                    }
                     catch
                    {
                         return "";
                     }

        }
        public static string SfzhToAge(string sfzh, int lx)
        {

            if (sfzh.Length < 16)
            {
                return "0";
            }
            string csrq = sfzh.Substring(6, 8);
           return  ToAge(csrq,lx);
        }

        public static string ToAge(string csrq, int lx)
        {

            if (csrq.Length >=16)
            csrq = csrq.Substring(6, 8);

            if (lx == 1)
            {
                int m_Y1 = DateTime.Parse(csrq).Year;
                int m_Y2 = DateTime.Now.Year;
                int m_Age = m_Y2 - m_Y1;
                return m_Age.ToString()+"��";
            }
            if (lx == 2)
            {
                DateTime now=DateTime.Today;
                DateTime birthDate = DateTime.Parse(csrq);
                int age = now.Year - birthDate.Year; ;

                if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                {
                    age--;
                }
                return age.ToString()+"��";
            }
            if(lx==3)
            {
                string strAge = string.Empty; // ������ַ�����ʾ
                int intYear = 0; // ��
                int intMonth = 0; // ��
                int intDay = 0; // ��
 
                DateTime dtNow=DateTime.Today;
                DateTime dtBirthday=DateTime.Parse(csrq);
                // ��������
                intDay = dtNow.Day - dtBirthday.Day;
                 if (intDay < 0)
                 {
                 dtNow = dtNow.AddMonths(-1);
                 intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
                 }
 
                // ��������
                intMonth = dtNow.Month - dtBirthday.Month;
                 if (intMonth < 0)
                 {
                 intMonth += 12;
                 dtNow = dtNow.AddYears(-1);
                 }
 
                // ��������
                intYear = dtNow.Year - dtBirthday.Year;
 
                // ��ʽ���������
                if (intYear >= 1) // ������
                {
                strAge = intYear.ToString() + "��";
                 }
 
                if (intMonth > 0 && intYear <= 3) // 3�����¿����������
                {
                strAge += intMonth.ToString() + "��";
                 }
 
                if (intDay >= 0 && intYear <= 1) // һ�����¿����������
                {
                if (strAge.Length == 0 || intDay > 0)
                 {
                 strAge += intDay.ToString() + "��";
                 }
                 }
 
                return strAge;
            }
            return "";
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
                    catch
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

        public static DataTable DT_SQD()
        {
            DataTable dt_sqd = new DataTable();
            //����
            DataColumn name = new DataColumn("F_XM");
            dt_sqd.Columns.Add(name);
            //�Ա�
            DataColumn sexName = new DataColumn("F_XB");
            dt_sqd.Columns.Add(sexName);
            //����
            DataColumn ageYear = new DataColumn("F_NL");
            dt_sqd.Columns.Add(ageYear);
            //birthdayF_XM
            DataColumn birthday = new DataColumn("F_CSRQ");
            dt_sqd.Columns.Add(birthday);
            //idCardNo
            DataColumn idCardNo = new DataColumn("F_SFZH");
            dt_sqd.Columns.Add(idCardNo);
            //phone
            DataColumn phone = new DataColumn("F_DH");
            dt_sqd.Columns.Add(phone);
            //address
            DataColumn address = new DataColumn("F_DZ");
            dt_sqd.Columns.Add(address);
            //סԺ��
            DataColumn InpatientNo = new DataColumn("F_ZYH");
            dt_sqd.Columns.Add(InpatientNo);
            //�����
            DataColumn F_MZH = new DataColumn("F_MZH");
            dt_sqd.Columns.Add(F_MZH);

            //�ǼǺ�
            DataColumn PatNo = new DataColumn("F_BRBH");
            dt_sqd.Columns.Add(PatNo);
            //F_YZID
            DataColumn F_YZID = new DataColumn("F_YZID");
            dt_sqd.Columns.Add(F_YZID);
            //F_SQXH
            DataColumn F_SQXH = new DataColumn("F_SQXH");
            dt_sqd.Columns.Add(F_SQXH);
            //����
            DataColumn Marry = new DataColumn("F_HY");
            dt_sqd.Columns.Add(Marry);
            //����
            DataColumn Nation = new DataColumn("F_MZ");
            dt_sqd.Columns.Add(Nation);
            //����
            DataColumn wardName = new DataColumn("F_BQ");
            dt_sqd.Columns.Add(wardName);
            //����
            DataColumn bedName = new DataColumn("F_CH");
            dt_sqd.Columns.Add(bedName);
            //�ͼ�ҽ��
            DataColumn employeeName = new DataColumn("F_SJYS");
            dt_sqd.Columns.Add(employeeName);
            //�ͼ����
            DataColumn detpname = new DataColumn("F_SJkS");
            dt_sqd.Columns.Add(detpname);
            //�ͼ쵥λ
            DataColumn Hospital = new DataColumn("F_SJDW");
            dt_sqd.Columns.Add(Hospital);
            //��������
            DataColumn brlb = new DataColumn("F_BRLB");
            dt_sqd.Columns.Add(brlb);

            //ҽ����Ŀ
            DataColumn requestType = new DataColumn("F_YZXM");
            dt_sqd.Columns.Add(requestType);

            //��鲿λ
            DataColumn orderName = new DataColumn("F_BBMC");
            dt_sqd.Columns.Add(orderName);

            //�ٴ���ʷ
            DataColumn clinicalHistory = new DataColumn("F_LCZL");
            dt_sqd.Columns.Add(clinicalHistory);

            //�ٴ����
            DataColumn diagnosis = new DataColumn("F_LCZD");
            dt_sqd.Columns.Add(diagnosis);

            return dt_sqd;

        }
    }
}
