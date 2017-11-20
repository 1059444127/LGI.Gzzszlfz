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
        //武夷山市立医院
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
            if (Sslbx == "门诊卡号")
            {
                lb = "1";
                if (Ssbz.Contains("；"))
                {
                    Ssbz = Ssbz.Replace("；", "").ToString().Trim();
                }
                if (Ssbz.Contains("？"))
                {
                    Ssbz = Ssbz.Replace("？", "").ToString().Trim();
                }
                if (Ssbz.Contains("?"))
                {
                    Ssbz = Ssbz.Replace("?", "").ToString().Trim();
                }
                if (!Ssbz.Contains(";"))
                    Ssbz = ";" + Ssbz;
            }
            if (Sslbx == "门诊号")
            {
                lb = "1";
               
            }
            if (Sslbx == "体检号")
            {
                lb = "5";

            }

            if ( Sslbx == "读社保卡")
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
                            MessageBox.Show("卡号：" + pCardno.ToString());

                           
                        }
                        else
                        {
                            MessageBox.Show("读卡失败, 请重试：" + pErrmsg2.ToString());
                            return "0";
                        }
                    }
                    else
                    {
                        MessageBox.Show("读卡失败, 请重试！！！");
                        return "0";
                    }
                }
                catch
                {
                    MessageBox.Show("请确认社保卡是否已正确插入！！！");
                    return "0";
                }

            }

            string rtn = "";

            if (!bool.Parse(JykBrXx(Ssbz, "3", "病理科", ksID, "", "").ToString()))
            {
                MessageBox.Show("打开网络连接异常！");
                return "0";
            }

            string getxx = JykBrXx(Ssbz, lb, "病理科", ksID, "", "");  //00165409    //337733
            if (Debug == "1")
            {
                MessageBox.Show(getxx);
                log.WriteMyLog(getxx);
            }
             if (!bool.Parse(JykBrXx(Ssbz, "4", "病理科", ksID, "", "")))
            {
                MessageBox.Show("关闭网络连接异常！");
                return "0";
            }
            if (getxx.ToString().ToLower() == "false")
            {
                MessageBox.Show("无法提取病人信息！可能无此病人申请或已收费！");
                return "0";
            }
            else
            {

                rtn = brxx(getxx, Sslbx, Ssbz);
              if (rtn == "")
              {
                  MessageBox.Show("获取病人信息异常！");
                  return "0";
              }
              if (Debug == "1")
                  log.WriteMyLog(rtn);
              return rtn;
            }

        }
        public static string brxx(string xx, string Sslbx,string Ssbz)
       {
//卡号|姓名|性别|出生日期|地址|电话|诊断|床号|身份证号｜家庭地址｜工作单位｜籍贯｜过敏反应｜检查项目数|
//检查项目号1|送检科室1|送检医生1|辅检单ID（唯一）1|辅检项目金额1|PATIENT_ID1|EVENT_ID1|送检时间1|
//检查项目号2|送检科室2|送检医生2|辅检单ID（唯一）2|辅检项目金额2|PATIENT_ID2|EVENT_ID2|送检时间2|
//检查项目号3|送检科室3|送检医生3|辅检单ID（唯一）3|辅检项目金额3|PATIENT_ID3|EVENT_ID3|送检时间3|
           
           string xml = "";

           try
           {
               string[] aa = xx.Split('|');

               xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
               xml = xml + "<LOGENE>";
               xml = xml + "<row ";
               xml = xml + "病人编号=" + (char)34 + aa[0].ToString() + (char)34 + " ";
               xml = xml + "就诊ID=" + (char)34 + aa[19].ToString()+"^"+aa[20].ToString() + (char)34 + " ";
               xml = xml + "申请序号=" + (char)34 + aa[17].ToString() + (char)34 + " ";
               string mzh = "";
               string zyh = "";
          
            if (Sslbx == "住院号")
                zyh = Ssbz;
            if (Sslbx == "门诊号" || Sslbx == "门诊卡号")
               mzh = aa[0].ToString();
           if (Sslbx == "体检号")
               mzh = Ssbz;
              if (Sslbx == "读社保卡")
                  mzh = Ssbz;
               xml = xml + "门诊号=" + (char)34 +mzh + (char)34 + " ";
               xml = xml + "住院号=" + (char)34 +zyh + (char)34 + " ";
               //姓名
               xml = xml + "姓名=" + (char)34 + aa[1].ToString() + (char)34 + " ";
               //性别  （1男2女3不详)
               string xb = "";
               switch (aa[2].ToString())
               {
                   case "1": xb = "男"; break;
                   case "2": xb = "女"; break;
                   case "3": xb = "不详"; break;
                   default: xb = ""; break;
               }
               xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";



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
              //      xml = xml + "年龄=" + (char)34 +Year + "岁" + (char)34 + " ";
              //     else
              //     {
              //        if(Year==0)
              //              xml = xml + "年龄=" + (char)34 +Month + "月"+day+"天" + (char)34 + " ";
              //        else
              //          xml = xml + "年龄=" + (char)34 +Year + "岁"+Month+"月"+ (char)34 + " ";
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
                           //xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                           if (Month > 0)
                               xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                           if (Month < 0)
                               xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                           if (Month == 0)
                           {
                               if (day >= 0)
                                   xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                               else
                                   xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                           }
                       }
                       else
                           if (Year == 0)
                           {
                               int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                               int m = day1 / 30;
                               int d = day1 % 30;
                               xml = xml + "年龄=" + (char)34 + m + "月" + d + "天" + (char)34 + " ";
                           }
                   }





               }
               catch
               {
                   xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
               }
               ////出生年月--年龄
               ////int nl = DateTime.Today.Year - int.Parse(aa[3].ToString().Substring(0, 4));
               ////xml = xml + "年龄=" + (char)34 + nl.ToString() + "岁" + (char)34 + " ";
               //
               xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
               xml = xml + "地址=" + (char)34 + aa[4].ToString() + "  单位："+aa[10].ToString() + (char)34 + "   ";
               xml = xml + "电话=" + (char)34 + aa[5].ToString() + (char)34 + " ";
               xml = xml + "病区=" + (char)34 + aa[15].ToString() + (char)34 + " ";
               xml = xml + "床号=" + (char)34 + aa[7].ToString() + (char)34 + " ";
               xml = xml + "身份证号=" + (char)34 + aa[8].ToString() + (char)34 + " ";
               xml = xml + "民族=" + (char)34 + "汉族" + (char)34 + " ";
               xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
               //送检科室
               xml = xml + "送检科室=" + (char)34 + aa[15].ToString() + (char)34 + " ";
               //送检医生
               xml = xml + "送检医生=" + (char)34 + aa[16].ToString() + (char)34 + " ";
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

               xml = xml + "收费=" + (char)34 + rmb.ToString() + (char)34 + " ";
               xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
               xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
               xml = xml + "医嘱项目=" + (char)34 + aa[14].ToString() + (char)34 + " ";
               xml = xml + "备用1=" + (char)34 + (char)34 + " ";
               if(Sslbx == "体检号")
               xml = xml + "备用2=" + (char)34 + Ssbz + (char)34 + " ";
               else
               xml = xml + "备用2=" + (char)34 + ""+ (char)34 + " ";
               xml = xml + "费别=" + (char)34 + (char)34 + " ";
               string brlb = "";
               if (Sslbx == "门诊号") brlb = "门诊";
               if (Sslbx == "读社保卡") brlb = "门诊";
               if (Sslbx == "门诊卡号") brlb = "门诊";
               if (Sslbx == "住院号") brlb = "住院";
               if (Sslbx == "体检号") brlb = "体检";
               xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
               xml = xml + "/>";
               xml = xml + "<临床病史><![CDATA[" + aa[12].ToString() + "]]></临床病史>";
               xml = xml + "<临床诊断><![CDATA[" + aa[6].ToString() + "]]></临床诊断>";
               xml = xml + "</LOGENE>";
              
               return xml;
           }
           catch (Exception  ee)
           {
               log.WriteMyLog("异常："+ee.Message.ToString());
               return "0";
           }
       }


    }
}
