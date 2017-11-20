using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using LGHISJKZGQ;
using System.Net;
using System.Runtime.InteropServices;
namespace LGHISJKZGQ
{
    class WYSSLYY
    {
        //����ɽ����ҽԺ
        private static IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");
       // private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
        [DllImport(("JykBrxxDll.dll"),EntryPoint = "JykBrXx", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern string JykBrXx(string code,string bz,string ks,string gzh,string yqname,string Formbz);

        [DllImport(("fzyktclient.dll"), EntryPoint = "ykt_opendevice", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ykt_opendevice(int nDeviceType, int nCom, int nBaud, string pRegion, out IntPtr pWarnmsg, out IntPtr pErrmsg);
        [DllImport(("fzyktclient.dll"), EntryPoint = "ykt_readopencardno_local", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ykt_readopencardno_local(string pRegion, string pDeptid, out IntPtr pDeptKey, out  IntPtr pCardtype, StringBuilder pCardno, out  IntPtr pWarnmsg, out IntPtr pErrmsg);
        [DllImport(("fzyktclient.dll"), EntryPoint = "ykt_closedevice", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern int ykt_closedevice(string pWarnmsg, string pErrmsg);
       
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            string ksID = "JC12";

           
            if (Sslbx == "")  
            {
                return "0";
            }

           

            string lb = "2";
            if (Sslbx == "���￨��")
            {
                lb = "1";
                if (Ssbz.Contains("��"))
                {
                    Ssbz = Ssbz.Replace("��", "").ToString().Trim();
                }
                if (Ssbz.Contains("��"))
                {
                    Ssbz = Ssbz.Replace("��", "").ToString().Trim();
                }
                if (Ssbz.Contains("?"))
                {
                    Ssbz = Ssbz.Replace("?", "").ToString().Trim();
                }
                if (!Ssbz.Contains(";"))
                    Ssbz = ";" + Ssbz;
            }
            if (Sslbx == "�����")
            {
                lb = "1";
               
            }
            if (Sslbx == "����")
            {
                lb = "5";

            }

            if ( Sslbx == "���籣��")
            {
                try
                {
                    string pRegion = "3507";
                    IntPtr pWarnmsg1 = new IntPtr();
                    IntPtr pErrmsg1 = new IntPtr();


                    int x = ykt_opendevice(10, 9, 2, pRegion, out pWarnmsg1, out pErrmsg1);
                    if (x == 0)
                    {
                        // IntPtr pDeptID = new IntPtr(0001);
                        string pDeptID = "0001";
                        IntPtr pDeptKey, pCardtype, pWarnmsg2, pErrmsg2;
                        StringBuilder pCardno = new StringBuilder(25);

                        int xx = ykt_readopencardno_local(pRegion, pDeptID, out pDeptKey, out pCardtype,  pCardno, out pWarnmsg2, out  pErrmsg2);
                        string pWarnmsg3 = "";
                        string pErrmsg3 = "";
                        int xxx = ykt_closedevice(pWarnmsg3, pErrmsg3);
                        if (xx == 0)
                        {
                            lb = "1";
                            Ssbz = pCardno.ToString();
                           if(Debug=="1")
                            MessageBox.Show("���ţ�" + pCardno.ToString());

                           
                        }
                        else
                        {
                            MessageBox.Show("����ʧ��, �����ԣ�" + pErrmsg2.ToString());
                            return "0";
                        }
                    }
                    else
                    {
                        MessageBox.Show("����ʧ��, �����ԣ�����");
                        return "0";
                    }
                }
                catch
                {
                    MessageBox.Show("��ȷ���籣���Ƿ�����ȷ���룡����");
                    return "0";
                }

            }

            string rtn = "";

            if (!bool.Parse(JykBrXx(Ssbz, "3", "�����", ksID, "", "").ToString()))
            {
                MessageBox.Show("�����������쳣��");
                return "0";
            }

            string getxx = JykBrXx(Ssbz, lb, "�����", ksID, "", "");  //00165409    //337733
            if (Debug == "1")
            {
                MessageBox.Show(getxx);
                log.WriteMyLog(getxx);
            }
             if (!bool.Parse(JykBrXx(Ssbz, "4", "�����", ksID, "", "")))
            {
                MessageBox.Show("�ر����������쳣��");
                return "0";
            }
            if (getxx.ToString().ToLower() == "false")
            {
                MessageBox.Show("�޷���ȡ������Ϣ�������޴˲�����������շѣ�");
                return "0";
            }
            else
            {

                rtn = brxx(getxx, Sslbx, Ssbz);
              if (rtn == "")
              {
                  MessageBox.Show("��ȡ������Ϣ�쳣��");
                  return "0";
              }
              if (Debug == "1")
                  log.WriteMyLog(rtn);
              return rtn;
            }

        }
        public static string brxx(string xx, string Sslbx,string Ssbz)
       {
//����|����|�Ա�|��������|��ַ|�绰|���|����|���֤�ţ���ͥ��ַ��������λ�������������Ӧ�������Ŀ��|
//�����Ŀ��1|�ͼ����1|�ͼ�ҽ��1|���쵥ID��Ψһ��1|������Ŀ���1|PATIENT_ID1|EVENT_ID1|�ͼ�ʱ��1|
//�����Ŀ��2|�ͼ����2|�ͼ�ҽ��2|���쵥ID��Ψһ��2|������Ŀ���2|PATIENT_ID2|EVENT_ID2|�ͼ�ʱ��2|
//�����Ŀ��3|�ͼ����3|�ͼ�ҽ��3|���쵥ID��Ψһ��3|������Ŀ���3|PATIENT_ID3|EVENT_ID3|�ͼ�ʱ��3|
           
           string xml = "";

           try
           {
               string[] aa = xx.Split('|');

               xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
               xml = xml + "<LOGENE>";
               xml = xml + "<row ";
               xml = xml + "���˱��=" + (char)34 + aa[0].ToString() + (char)34 + " ";
               xml = xml + "����ID=" + (char)34 + aa[19].ToString()+"^"+aa[20].ToString() + (char)34 + " ";
               xml = xml + "�������=" + (char)34 + aa[17].ToString() + (char)34 + " ";
               string mzh = "";
               string zyh = "";
          
            if (Sslbx == "סԺ��")
                zyh = Ssbz;
            if (Sslbx == "�����" || Sslbx == "���￨��")
               mzh = aa[0].ToString();
           if (Sslbx == "����")
               mzh = Ssbz;
              if (Sslbx == "���籣��")
                  mzh = Ssbz;
               xml = xml + "�����=" + (char)34 +mzh + (char)34 + " ";
               xml = xml + "סԺ��=" + (char)34 +zyh + (char)34 + " ";
               //����
               xml = xml + "����=" + (char)34 + aa[1].ToString() + (char)34 + " ";
               //�Ա�  ��1��2Ů3����)
               string xb = "";
               switch (aa[2].ToString())
               {
                   case "1": xb = "��"; break;
                   case "2": xb = "Ů"; break;
                   case "3": xb = "����"; break;
                   default: xb = ""; break;
               }
               xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";



               /////
               try
               {
               string CSRQ = aa[3].ToString();
              
              //DateTime dt = new DateTime();
              // try
              // {
              // if (CSRQ.Contains("-"))
              // { 
              //     TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
              //     dt = dt.Add(tp);
              // }
              // else
              // {
              //     TimeSpan tp = DateTime.Now - DateTime.Parse(string.Format("{0:0000-00-00}", Convert.ToInt32(CSRQ.ToString())));
              //     dt = dt.Add(tp);
              // }
              //     int Year = dt.Year -1;
              //     int Month = dt.Month-1;
              //     int day = dt.Day;

              //     if (Year >=3)
              //      xml = xml + "����=" + (char)34 +Year + "��" + (char)34 + " ";
              //     else
              //     {
              //        if(Year==0)
              //              xml = xml + "����=" + (char)34 +Month + "��"+day+"��" + (char)34 + " ";
              //        else
              //          xml = xml + "����=" + (char)34 +Year + "��"+Month+"��"+ (char)34 + " ";
              //     }


                   /////////////////////////////////////////////
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
               catch
               {
                   xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
               }
               ////��������--����
               ////int nl = DateTime.Today.Year - int.Parse(aa[3].ToString().Substring(0, 4));
               ////xml = xml + "����=" + (char)34 + nl.ToString() + "��" + (char)34 + " ";
               //
               xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
               xml = xml + "��ַ=" + (char)34 + aa[4].ToString() + "  ��λ��"+aa[10].ToString() + (char)34 + "   ";
               xml = xml + "�绰=" + (char)34 + aa[5].ToString() + (char)34 + " ";
               xml = xml + "����=" + (char)34 + aa[15].ToString() + (char)34 + " ";
               xml = xml + "����=" + (char)34 + aa[7].ToString() + (char)34 + " ";
               xml = xml + "���֤��=" + (char)34 + aa[8].ToString() + (char)34 + " ";
               xml = xml + "����=" + (char)34 + "����" + (char)34 + " ";
               xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
               //�ͼ����
               xml = xml + "�ͼ����=" + (char)34 + aa[15].ToString() + (char)34 + " ";
               //�ͼ�ҽ��
               xml = xml + "�ͼ�ҽ��=" + (char)34 + aa[16].ToString() + (char)34 + " ";
               int rmb = 0;
               try
               {
                   int count = int.Parse(aa[13].ToString());

                   for (int x = 0; x < count; x++)
                       rmb = rmb + int.Parse(aa[x * 8 + 18].ToString().Trim());
               }
               catch
               {
               }

               xml = xml + "�շ�=" + (char)34 + rmb.ToString() + (char)34 + " ";
               xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
               xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
               xml = xml + "ҽ����Ŀ=" + (char)34 + aa[14].ToString() + (char)34 + " ";
               xml = xml + "����1=" + (char)34 + (char)34 + " ";
               if(Sslbx == "����")
               xml = xml + "����2=" + (char)34 + Ssbz + (char)34 + " ";
               else
               xml = xml + "����2=" + (char)34 + ""+ (char)34 + " ";
               xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";
               string brlb = "";
               if (Sslbx == "�����") brlb = "����";
               if (Sslbx == "���籣��") brlb = "����";
               if (Sslbx == "���￨��") brlb = "����";
               if (Sslbx == "סԺ��") brlb = "סԺ";
               if (Sslbx == "����") brlb = "���";
               xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
               xml = xml + "/>";
               xml = xml + "<�ٴ���ʷ><![CDATA[" + aa[12].ToString() + "]]></�ٴ���ʷ>";
               xml = xml + "<�ٴ����><![CDATA[" + aa[6].ToString() + "]]></�ٴ����>";
               xml = xml + "</LOGENE>";
              
               return xml;
           }
           catch (Exception  ee)
           {
               log.WriteMyLog("�쳣��"+ee.Message.ToString());
               return "0";
           }
       }


    }
}
