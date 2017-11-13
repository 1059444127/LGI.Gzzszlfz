
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
       /// 取sz和数据库中配置
       /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
       /// <param name="Default">默认值</param>
       /// <returns>设定值</returns>
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
        /// 报告痕迹
        /// </summary>
        /// <param name="blh">病理号</param>
        /// <param name="yhmc">用户名称</param>
        /// <param name="DZ">动作</param>
        /// <param name="NR">内容</param>
        /// <param name="EXEMC">EXE名称</param>
        /// <param name="CTMC">界面名称</param>
        public static void BGHJ(string blh, string yhmc, string DZ, string NR, string EXEMC, string CTMC)
        {
         
            try
            {
                 IPAddress addr = new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                aa.ExecuteSQL("insert into  T_BGHJ(F_BLH,F_RQ,F_CZY,F_WZ,F_DZ,F_NR,F_EXEMC,F_CTMC) values('" + blh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"
                    + yhmc + "','机器名：" + Dns.GetHostName().Trim() + ",IP 地址：" + addr.ToString() +"','"+DZ+"','" + NR + "','"+EXEMC+"','" +CTMC + "') ");
            }
            catch
            {
            }
        }
        
        /// <summary>
        /// 通过用户名称获取用户编号
        /// </summary>
        /// <param name="yhmc">用户名称</param>
        /// <returns>用户编号,未找到返回0</returns>
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
        /// 出生日期计算年龄
        /// </summary>
        /// <param name="csrq">出生日期</param>
        /// <returns>年龄</returns>
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
                                return  Year + "岁" ;
                            else
                            {
                                if (Year == 0)
                                {
                                    if (Month<=0)
                                        return  day + "天";
                                    else
                                    return Month + "月" + day + "天";
                                }
                                else
                                    return +Year + "岁" + Month + "月";
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
        /// 身份证号计算年龄
        /// </summary>
        /// <param name="csrq">身份证号</param>
        /// <returns>年龄</returns>
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
                                return  Year + "岁" ;
                            else
                            {
                                if (Year == 0)
                                {
                                    if (Month<=0)
                                        return  day + "天";
                                    else
                                    return Month + "月" + day + "天";
                                }
                                else
                                    return +Year + "岁" + Month + "月";
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
        /// LOGENE_XML格式myDictionary
          /// </summary>
        public Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        /// <summary>
        /// 初始化 myDictionary
        /// </summary>
        public void PT_XML()
        {
            myDictionary.Add("病人编号", "");
            myDictionary.Add("就诊ID", "");
            myDictionary.Add("申请序号", "");
            myDictionary.Add("门诊号", "");
            myDictionary.Add("住院号", "");
            myDictionary.Add("姓名", "");
            myDictionary.Add("性别", "");
            myDictionary.Add("年龄", "");
            myDictionary.Add("婚姻", "");
            myDictionary.Add("地址", "");
            myDictionary.Add("电话", "");
            myDictionary.Add("病区", "");
            myDictionary.Add("床号", "");
            myDictionary.Add("身份证号", "");
            myDictionary.Add("民族", "");
            myDictionary.Add("职业", "");
            myDictionary.Add("送检科室", "");
            myDictionary.Add("送检医生", "");
            myDictionary.Add("收费", "");
            myDictionary.Add("标本名称", "");
            myDictionary.Add("送检医院", "本院");
            myDictionary.Add("医嘱项目", "");
            myDictionary.Add("备用1", "");
            myDictionary.Add("备用2", "");
            myDictionary.Add("费别", "");
            myDictionary.Add("病人类别", "");
            myDictionary.Add("临床病史", "");
            myDictionary.Add("临床诊断", "");
            myDictionary.Add("出生日期", "");
        }

        /// <summary>
        /// 生成  LOGENE_XML
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
                    xml = xml + "病人编号=" + (char)34 + myDictionary["病人编号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：病人编号异常\r\n";
                    xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "就诊ID=" + (char)34 + myDictionary["就诊ID"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：就诊ID异常\r\n";
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "申请序号=" + (char)34 + myDictionary["申请序号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：申请序号异常\r\n";
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "门诊号=" + (char)34 + myDictionary["门诊号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：门诊号异常\r\n";
                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "住院号=" + (char)34 + myDictionary["住院号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：住院号异常\r\n";
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "姓名=" + (char)34 + myDictionary["姓名"].Trim() + (char)34 + " ";
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "性别=" + (char)34 + myDictionary["性别"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：性别异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                if (myDictionary["年龄"].Trim() == "" && myDictionary["出生日期"].Trim() != "")
                {
                    try
                    {
                        string nl = "";
                        string CSRQ = myDictionary["出生日期"].Trim();
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

                                if (Year > 0 && Year < 3)
                                {
                                    if ((Year - 1) == 0)
                                    {
                                        if (Month <= 0)
                                        {
                                            if (day > 0)
                                                xml = xml + "年龄=" + (char)34 + (12 + Month) + "月" + day + "天" + (char)34 + " ";
                                            else
                                                xml = xml + "年龄=" + (char)34 + (12 + Month - 1) + "月" + (30 + day) + "天" + (char)34 + " ";
                                        }
                                        else
                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + (Month) + "月" + (char)34 + " ";
                                    }
                                    else
                                    {
                                        if (Month > 0)
                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + Month + "月" + (char)34 + " ";
                                        else
                                            xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (12 + Month) + "月" + (char)34 + " ";

                                    }

                                }
                                if (Year == 0)
                                {
                                    int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                    int m = day1 / 30;
                                    int d = day1 % 30;
                                    xml = xml + "年龄=" + (char)34 + m + "月" + d + "天" + (char)34 + " ";
                                }
                        }
                    }
                    catch (Exception ee)
                    {
                        xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        exep = exep + "提取字段：年龄异常\r\n";
                    }
                }
                else
                {
                    xml = xml + "年龄=" + (char)34 + myDictionary["年龄"].Trim() + (char)34 + " ";

                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "婚姻=" + (char)34 + myDictionary["婚姻"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：婚姻异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "地址=" + (char)34 + myDictionary["地址"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：地址异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "电话=" + (char)34 + myDictionary["电话"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "电话=" + (char)34 + " " + (char)34 + " ";
                    exep = exep + "提取字段：电话异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "病区=" + (char)34 + myDictionary["病区"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：病区异常\r\n";
                }

                /////////////////////////////////////////////////////////////////

                try
                {
                    xml = xml + "床号=" + (char)34 + myDictionary["床号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：床号异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "身份证号=" + (char)34 + myDictionary["身份证号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：身份证号异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "民族=" + (char)34 + myDictionary["民族"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：民族异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "职业=" + (char)34 + myDictionary["职业"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：职业异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "送检科室=" + (char)34 + myDictionary["送检科室"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：送检科室异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "送检医生=" + (char)34 + myDictionary["送检医生"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：送检医生异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "收费=" + (char)34 + myDictionary["收费"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：收费异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "标本名称=" + (char)34 + myDictionary["标本名称"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：标本名称异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "送检医院=" + (char)34 + myDictionary["送检医院"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                    exep = exep + "提取字段：送检医院异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "医嘱项目=" + (char)34 + myDictionary["医嘱项目"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：医嘱项目异常\r\n";
                }


                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "备用1=" + (char)34 + myDictionary["备用1"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                    exep = exep + "提取字段：备用1异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "备用2=" + (char)34 + myDictionary["备用2"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                    exep = exep + "提取字段：备用2异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "费别=" + (char)34 + myDictionary["费别"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：费别异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "病人类别=" + (char)34 + myDictionary["病人类别"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：病人类别异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "/>";

                try
                {
                    xml = xml + "<临床病史><![CDATA[" + myDictionary["临床病史"].Trim() + "]]></临床病史>";
                }
                catch
                {
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                    exep = exep + "提取字段：临床病史异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "<临床诊断><![CDATA[" + myDictionary["临床诊断"].Trim() + "]]></临床诊断>";
                }
                catch
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                    exep = exep + "提取字段：临床诊断异常\r\n";
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
        ////文件名格式：  审核时间_病理号_报告类型_报告序号_小号；  yyyyMMddHHmmss_123456_CG_1_1.pdf


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
                    message = "查询数据库异常：" + ex.Message.ToString();
                    return false;
                }

                //清空c:\temp目录
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
                    log.WriteMyLog("下载图片失败：" + message);
                    return false;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    if (File.Exists(localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
                    {
                        jpgname = "";
                        message = "下载图像错误，未找到图片："+localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString();
                        return false;
                    }
                }

                    if (localpath == "")
                    {
                        jpgname = "";
                        message = "PDF_JPG方法下载图像错误";
                        return false;
                    }
              
                string sbmp = "";
                string stxsm = "";
                string sJPGNAME = localpath + "\\" + filename;

             //   string sJPGNAME = localpath + "\\" + F_blh + ".jpg";

                string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                string inibglj = GetSz("dybg", "dybglj", "").Replace("\0", "");
              
                if (inibglj.Trim() == "")
                    inibglj = Application.StartupPath.ToString();
                    sBGGSName = inibglj + "\\" + rptpath + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";

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
                                    foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                    {
                                        if (name.Trim()!="")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                        }

                        if (ysname.ToLower().Trim() == "f_fzys")
                        {
                            foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                            {
                                if (name.Trim() != "" )
                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                            }
                        }
                    }
                  sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                }

                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
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
                        message = "查询数据库异常：" + ex.Message.ToString();
                        return false;
                    }
                    string bd_bggs = "冰冻";
                    if (bcbddytx)
                        bd_bggs = bd_bggs + "-" + txlb.Rows.Count.ToString() + "图";
                  
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
                                    foreach (string name in BDBG.Rows[0]["f_bd_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                    {

                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in BDBG.Rows[0]["f_bd_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }
                            if (bcbddytx)
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bd_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                            else
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bd_bggs + ".frf";
                    }

                    sSQL_DY = "SELECT * FROM T_JCXX,T_BdBG WHERE T_JCXX.F_BLH = T_BdBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_Bd_BGXH='" + bgxh + "'";
                    
                    if (bcbddytx)
                        bggs = bd_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
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
                        message = "查询数据库异常：" + ex.Message.ToString();
                        return false;
                    }
                    string bc_bggs = "补充";
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
                                bc_bggs = "补充";
                            }
                        }
                    }
                    catch
                    { }
                    if (bc_bggs.Trim() == "")
                        bc_bggs = "补充";
                    if (bcbddytx)
                        bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图";
                   
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
                                foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                {

                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }
                       if (bcbddytx)
                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                            else
                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs  + ".frf";
                    }
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                 
                    if (bcbddytx)
                        bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                    else
                        bggs = bc_bggs + ".frf";
                    sJPGNAME = localpath + "\\" + filename;
                }



                //判断报告格式是否存在
                if (!File.Exists(sBGGSName))
                {
                    message = "报告格式不存在:" + sBGGSName;
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
                                    log.WriteMyLog("pr.print完成");
                                jpgname = sJPGNAME.Replace(".", "_1.");
                            }
                            else
                            {
                                pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,debug);
                                if (debug == "1")
                                    log.WriteMyLog("pr.print完成");
                                jpgname = sJPGNAME;
                            }
                        }
                        catch (Exception e3)
                        {
                            message = "生成pdf异常，调用prreport异常:" + e3.Message;
                            rtn = false;
                        }

                        if (!File.Exists(jpgname))
                        {
                            message = "PDF_JPG-本地未找到文件1";
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
                message = "PDF_JPG方法异常:" + e4.Message;
                return false;
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="f_blh">病理号</param>
        /// <param name="txml">图像目录</param>
        /// <param name="aa"></param>
        /// <param name="txlbs"></param>
        /// <param name="localpath">临时目录</param>
        /// <param name="err_msg">错误信息</param>
        /// <returns>成功或者失败</returns>
        public bool downtx(string f_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath, ref string err_msg)
        {
       
            try
            {
                //清空c:\temp目录
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
                //临时目录
                localpath = @"c:\temp\" + f_blh;

                //下载FTP参数
                string ftpserver = GetSz("ftp", "ftpip", "").Replace("\0", "");
                string ftpuser = GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                string ftppwd = GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = GetSz("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
                string ftpremotepath = GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                string ftps = GetSz("ftp", "ftp", "").Replace("\0", "");
                string txpath = GetSz("txpath", "txpath", "").Replace("\0", "");

                FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
                //共享目录
                string gxml = GetSz("txpath", "txpath", "").Replace("\0", "");
                //string gxuid = GetSz("txpath", "username", "").Replace("\0", "");
                //string gxpwd = GetSz("txpath", "password", "").Replace("\0", "");

             
                DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + f_blh + "' and F_sfdy='1'", "txlb");
                string txm = "";

                if (ftps == "1")//FTP下载方式
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
                                        log.WriteMyLog("压缩图像异常：" + ee2.Message);
                                    }

                                }
                                txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";

                            }
                        }
                        catch (Exception ee2)
                        {
                            err_msg = "FTP下载图像出错:" + ee2.Message;
                            localpath = "";
                            return false;
                        }
                    }
                  
                    return true;
                }
                else //共享下载方式
                {
                    if (txpath == "")
                    {
                        err_msg = "sz.ini txpath图像目录未设置";
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
                                    log.WriteMyLog("压缩图像异常：" + ee2.Message);
                                }
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
                        }

                        catch (Exception ee3)
                        {
                            err_msg = "共享下载图像错误:" + ee3.Message;
                            localpath = "";
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception e4)
            {
               
                 err_msg = "下载图像异常:" + e4.Message;
                            return false;
            }

        }

/// 
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="blh">病理号</param>
        /// <param name="jpgpath">文件名，完整路径</param>
        /// <param name="ml">目录</param>
        /// <param name="err_msg">错误信息</param>
        /// <param name="lb">1:指定共享目录上传（[ZGQJK]toPDFPath=)；2:共享路径上传([txpath]txpath=)；3:ftp上传([ftp]);其他ftp上传([ftpup])  </param>
        /// <returns></returns>
        /// 
        public bool UpPDF( string jpgpath, string ml, ref string err_msg, int lb)
        {
            try
            {
                string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
                //---上传jpg----------
                //----------------上传至ftp---------------------
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
                                log.WriteMyLog("检查ftp目录。。。");

                            //收到日期目录
                            if (ml.Trim() != "")
                            {
                                //判断目录是否存在
                                if (!fw.fileCheckExist(ftpURI, ml))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    fw.Makedir(ml, out stat);
                                    if (stat != "OK")
                                    {
                                        err_msg = "FTP创建目录异常";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                           
                            //if (debug=="1")
                            //log.WriteMyLog("判断ftp上是否存在该文件");
                            //判断ftp上是否存在该jpg文件
                            //if (fw.fileCheckExist(ftpURI, jpgname))
                            //{
                            //    //删除ftp上的jpg文件
                            //    fw.fileDelete(ftpURI, jpgname).ToString();
                            //}
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件");
                            fw.Upload(jpgpath, ml, out status, ref err_msg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF上传失败，请重新审核！";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件结果：" + status + "\r\n" + err_msg);
                            //判断ftp上是否存在该jpg文件
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                }
                                else
                                {
                                    err_msg = "PDF上传失败，请重新审核！";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "上传PDF异常:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini中[ZGQJK]下toPDFPath图像目录未设置";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //判断ml目录是否存在
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml ))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + ml  + "--创建目录异常";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + ml ;
                            }
                            //判断共享上是否存在该pdf文件
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //删除共享上的pdf文件
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //判断共享上是否存在该pdf文件

                            File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                            // Thread.Sleep(1000);
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                                status = "OK";
                            else
                            {
                                err_msg = "上传PDF异常";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "上传异常:" + ee3.Message.ToString();
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
                    err_msg = "未找到文件" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF方法异常：" + e4.Message;
                return false;
            }

        }
   
        public  bool UpPDF(string blh, string jpgpath, string ml, ref string err_msg,int lb)
        {
            try
            {
                string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
                //---上传jpg----------
                //----------------上传至ftp---------------------
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
                            log.WriteMyLog("检查ftp目录。。。");

                            //收到日期目录
                            if (ml.Trim() != "")
                            {
                                //判断目录是否存在
                                if (!fw.fileCheckExist(ftpURI, ml))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    fw.Makedir(ml, out stat);
                                    if (stat != "OK")
                                    {
                                        err_msg = "FTP创建目录异常";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                                //病理号目录
                                //判断目录是否存在
                           // MessageBox.Show("1--"+ftpURI);
                                if (!fw.fileCheckExist(ftpURI, blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                  
                                    fw.Makedir(ftpURI,blh, out stat);
                                   
                                    if (stat != "OK")
                                    {
                                        err_msg = "FTP创建目录异常";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI +"/"+ blh + "/";
                            
                            //if (debug=="1")
                            //log.WriteMyLog("判断ftp上是否存在该文件");
                            //判断ftp上是否存在该jpg文件
                            //if (fw.fileCheckExist(ftpURI, jpgname))
                            //{
                            //    //删除ftp上的jpg文件
                            //    fw.fileDelete(ftpURI, jpgname).ToString();
                            //}
                            if (debug == "1")
                            log.WriteMyLog("上传新生成的文件");
                            fw.Upload(jpgpath, ml+"/"+blh, out status, ref err_msg);
                          //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF上传失败，请重新审核！";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件结果："+status+"\r\n" + err_msg);
                            //判断ftp上是否存在该jpg文件
                            try
                            {
                            
                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                }
                                else
                                {
                                    err_msg = "PDF上传失败，请重新审核！";
                                    status = "Error";
                                }
                               
                            }
                            catch (Exception err2)
                            {
                                err_msg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI  + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "上传PDF异常:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini中[ZGQJK]下toPDFPath图像目录未设置";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //判断ml目录是否存在
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml+"\\"+blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml+"\\"+blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--创建目录异常";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + ml + "\\" + blh;
                            }
                            //判断共享上是否存在该pdf文件
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //删除共享上的pdf文件
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //判断共享上是否存在该pdf文件

                            File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                           // Thread.Sleep(1000);
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                                status = "OK";
                            else
                            {
                                err_msg = "上传PDF异常";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "上传异常:" + ee3.Message.ToString();
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
                    err_msg = "未找到文件" + jpgpath + "";
                    return false;
                }
            }
            catch(Exception e4)
            {
                err_msg = "UpPDF方法异常："+e4.Message;
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
                //---上传jpg----------
                //----------------上传至ftp---------------------
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
                                log.WriteMyLog("检查ftp目录。。。");

                            //收到日期目录
                            if (ml.Trim() != "")
                            {
                                //判断目录是否存在
                                if (!fw.fileCheckExist(ftpURI, ml))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    fw.Makedir(ml, out stat);
                                    if (stat != "OK")
                                    {
                                        err_msg = "FTP创建目录异常";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                            //病理号目录
                            //判断目录是否存在
                            // MessageBox.Show("1--"+ftpURI);
                            if (!fw.fileCheckExist(ftpURI, blh))
                            {
                                //目录不存在，创建
                                string stat = "";

                                fw.Makedir(ftpURI, blh, out stat);

                                if (stat != "OK")
                                {
                                    err_msg = "FTP创建目录异常";
                                    return false;
                                }
                            }
                            ftpURI = ftpURI  + blh + "/";

                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件");
                            fw.Upload(jpgpath, ml + "/" + blh, out status, ref err_msg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                err_msg = "PDF上传失败，请重新审核！";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件结果：" + status + "\r\n" + err_msg);
                            //判断ftp上是否存在该jpg文件
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                    pdfpath = ftpURI + "" + jpgname;
                                }
                                else
                                {
                                    err_msg = "PDF上传失败，请重新审核！";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "上传PDF异常:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini中[ZGQJK]下toPDFPath图像目录未设置";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //判断ml目录是否存在
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml + "\\" + blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml + "\\" + blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--创建目录异常";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + ml + "\\" + blh;
                            }
                            //判断共享上是否存在该pdf文件
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //删除共享上的pdf文件
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //判断共享上是否存在该pdf文件

                            File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                            // Thread.Sleep(1000);
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                status = "OK";
                                pdfpath = tjtxpath + "\\" + jpgname;
                            }
                            else
                            {
                                err_msg = "上传PDF异常";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "上传异常:" + ee3.Message.ToString();
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
                    err_msg = "未找到文件" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF方法异常：" + e4.Message;
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

                          //开共享
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
                                      //判断ml目录是否存在
                                      if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml + "\\" + blh))
                                      {
                                          //目录不存在，创建
                                        
                                          try
                                          {
                                              System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml + "\\" + blh);
                                          }
                                          catch
                                          {
                                              err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--创建目录异常";
                                            pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                                              return false;
                                          }
                                      }
                                      tjtxpath = tjtxpath + "\\" + ml + "\\" + blh;
                                  }
                                  //判断共享上是否存在该pdf文件
                                  if (File.Exists(tjtxpath + "\\" + jpgname))
                                  {
                                      //删除共享上的pdf文件
                                      File.Delete(tjtxpath + "\\" + jpgname);
                                  }
                                  //判断共享上是否存在该pdf文件

                                  File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                                   Thread.Sleep(500);
                                  if (File.Exists(tjtxpath + "\\" + jpgname))
                                  {
                                      pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                                      return true;
                                  }
                                  else
                                  {
                                      err_msg = "上传PDF异常1";
                                     pro.StandardInput.WriteLine("net use  \\\\" + ip + "\\ipc$ /del");
                                      return false;
                                  }

                              }
                              catch (Exception ee3)
                              {
                                  err_msg = "上传异常:" + ee3.Message.ToString();
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
                              err_msg = "上传PDF异常2:"+ee.Message;
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
                                  //判断ml目录是否存在
                                  if (!System.IO.Directory.Exists(tjtxpath + "\\" + ml + "\\" + blh))
                                  {
                                      //目录不存在，创建
                                      string stat = "";
                                      try
                                      {
                                          System.IO.Directory.CreateDirectory(tjtxpath + "\\" + ml + "\\" + blh);
                                      }
                                      catch
                                      {
                                          err_msg = tjtxpath + "\\" + ml + "\\" + blh + "--创建目录异常";
                                          return false;
                                      }
                                  }
                                  tjtxpath = tjtxpath + "\\" + ml + "\\" + blh;
                              }
                              //判断共享上是否存在该pdf文件
                              if (File.Exists(tjtxpath + "\\" + jpgname))
                              {
                                  //删除共享上的pdf文件
                                  File.Delete(tjtxpath + "\\" + jpgname);
                              }
                              //判断共享上是否存在该pdf文件

                              File.Copy(jpgpath, tjtxpath + "\\" + jpgname, true);
                              // Thread.Sleep(1000);
                              if (File.Exists(tjtxpath + "\\" + jpgname))
                                  return true;
                              else
                              {
                                  err_msg = "上传PDF异常2";
                                  return false;
                              }
                          }
                          catch (Exception ee3)
                          {
                              err_msg = "上传异常:" + ee3.Message.ToString();
                              return false;
                          }
                      }

                          return true;
                }
                else
                {
                    err_msg = "未找到文件" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF方法异常：" + e4.Message;
                return false;
            }

        }
    
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ml">目录</param>
        /// <param name="jpgname">文件名</param>
        /// <param name="err_msg">错误信息</param>
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

                    //判断ftp上是否存在该jpg文件
                    if (fw.fileCheckExist(ftpURI, jpgname))
                    {
                        //删除ftp上的jpg文件
                        fw.fileDelete(ftpURI, jpgname).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    err_msg = "删除ftp上PDF异常:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    err_msg = "sz.ini中[savetohis]下PDFPath图像目录未设置";
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
                    err_msg = "删除文件异常:" + ee.Message.ToString();
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

                    //判断ftp上是否存在该jpg文件
                    if (fw.fileCheckExist(ftpURI, jpgname))
                    {
                        //删除ftp上的jpg文件
                        fw.fileDelete(ftpURI, jpgname).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    err_msg = "删除ftp上PDF异常:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    err_msg = "sz.ini中[savetohis]下PDFPath图像目录未设置";
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
                    err_msg = "删除文件异常:" + ee.Message.ToString();
                    return false;
                }

            }

        }

        /// <summary>
        /// 创建pdf或jpg
        /// </summary>
        /// <param name="F_blh">病理号</param>
        /// <param name="bglx">报告类型</param>
        /// <param name="bgxh">报告序号</param>
        /// <param name="type1">JPG或PDF</param>
        /// <param name="err_msg">错误信息</param>
        /// <param name="F_ML">目录</param>
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
                    err_msg = ("抛出异常:" + ex.Message.ToString());
                    //异常
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "病理数据库设置有问题！";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "病理号有错误！";
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
                    err_msg = "日期不能为空";
                    return false;
                }


                if (bgzt == "已审核")
                {
                    filename = DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + "_" + F_blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + ".pdf";
                    string rptpath = GetSz("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();
                    string jpgname = "";

                    //生成PDF
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

                    ////上传PDF
                    //Thread.Sleep(1000);
                    //bool status1 = false;

                    //    status1 = UpLoad(F_blh, jpgname, F_ML, ref msg);

                    //if (!status1)
                    //{
                    //    log.WriteMyLog(F_blh + "," + bglx + "," + bgxh + ",上传PDF异常：" + msg);
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
                    //    //成功
                    //    return 2;
                    //}
                }
                else
                {
                    err_msg = "报告未审核";
                    return false;
                    //DataTable dt = new DataTable();
                    //dt = aa.GetDataTable("select * from T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx.ToLower() + "' and  F_BGXH='" + bgxh + "'", "F_BG_PDF");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    aa.ExecuteSQL("delete  T_BG_PDF   where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                    //    Delete(dt.Rows[0]["F_ML"].ToString(), dt.Rows[0]["F_FILENAME"].ToString(), ref message);
                    //}
                    ////成功
                    //return 2;

                }
            }
            catch(Exception  e4)
            {
                err_msg = "CreatePDF方法异常："+e4.Message;
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
                    err_msg = ("抛出异常:" + ex.Message.ToString());
                    //异常
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "病理数据库设置有问题！";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "病理号有错误！";
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
                    err_msg = "日期不能为空";
                    return false;
                }


                if (bgzt == "已审核" || bgzt == "已发布")
                {

                    if (type1.ToString().ToLower()=="pdf")
                    filename =F_blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh+"_"+ DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss")  + ".pdf";
                    else
                    filename = F_blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";


                    string rptpath = GetSz("ZGQJK", "rptpath", "rpt").Replace("\0", "").Trim();
                     jpgname = "";

                    //生成PDF
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
                        err_msg = "未找到PDF文件";
                        return false;
                    }

                    ////上传PDF
                    //Thread.Sleep(1000);
                    //bool status1 = false;

                    //    status1 = UpLoad(F_blh, jpgname, F_ML, ref msg);

                    //if (!status1)
                    //{
                    //    log.WriteMyLog(F_blh + "," + bglx + "," + bgxh + ",上传PDF异常：" + msg);
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
                    //    //成功
                    //    return 2;
                    //}
                }
                else
                {
                    err_msg = "报告未审核";
                    return false;
                    //DataTable dt = new DataTable();
                    //dt = aa.GetDataTable("select * from T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx.ToLower() + "' and  F_BGXH='" + bgxh + "'", "F_BG_PDF");
                    //if (dt.Rows.Count > 0)
                    //{
                    //    aa.ExecuteSQL("delete  T_BG_PDF   where F_BLH='" + F_blh + "'and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                    //    Delete(dt.Rows[0]["F_ML"].ToString(), dt.Rows[0]["F_FILENAME"].ToString(), ref message);
                    //}
                    ////成功
                    //return 2;

                }
            }
            catch (Exception e4)
            {
                err_msg = "CreatePDF方法异常：" + e4.Message;
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
                    err_msg = ("抛出异常:" + ex.Message.ToString());
                    //异常
                    return false;
                }
                if (jcxx == null)
                {
                    err_msg = "病理数据库设置有问题！";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    err_msg = "病理号有错误！";
                    return false;
                }

            
                    if (type1.ToString().ToLower() == "pdf")
                        filename =  filename + ".pdf";
                    else
                        filename =  filename + ".jpg";


                    string rptpath = GetSz("ZGQJK", "rptpath", "rpt").Replace("\0", "").Trim();
                    jpgname = "";

                    //生成PDF
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
                        err_msg = "未找到PDF文件";
                        return false;
                    }

           
            }
            catch (Exception e4)
            {
                err_msg = "CreatePDF方法异常：" + e4.Message;
                return false;
            }
        }
        /// <summary>
        /// 清空本地临时文件
        /// </summary>
        /// <param name="blh">病理号</param>
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
                Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                file.Close();
                strbase64 = Convert.ToBase64String(imgByte);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
         }

        /// <summary>
        /// 格式化XML
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
                    message = "查询数据库异常：" + ex.Message.ToString();
                    return false;
                }
                //清空c:\temp目录
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
                    log.WriteMyLog("下载图片失败：" + message);
                    return false;
                }

                for (int i = 0; i < txlb.Rows.Count; i++)
                {
                    if (!File.Exists(localpath + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
                    {
                        fileName = "";
                        message = "下载图像错误，未找到图片：" + localpath + "\\" + txlb.Rows[i]["F_TXM"].ToString();
                        return false;
                    }
                }

                string sbmp = "";
                string stxsm = "";
                string sJPGNAME = localpath + "\\" + fileName;


                string rptpath2 = "rpt";
                if (rptpath.Trim() != "")
                    rptpath2 = rptpath;

                string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                string inibglj = ZgqClass.GetSz("dybg", "dybglj", "").Replace("\0", "");

                if (inibglj.Trim() == "")
                {
                    inibglj = Application.StartupPath.ToString();
                }
                if (inibglj != "")
                {
                    sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
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
                                foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }
                        if (ysname.ToLower().Trim() == "f_fzys")
                        {
                            foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                            {
                                if (name.Trim() != "")
                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                            }
                        }
                    }

                    sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                    }
                }

                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
                bool bcbddytx = false;
                if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
                    bcbddytx = true;

                if (bglx.ToLower() == "bd")
                {
                    sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\冰冻.frf";
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
                    if (bcbddytx)
                        bggs = "冰冻" + "-" + txlb.Rows.Count.ToString() + "图.frf";
                    else
                        bggs = "冰冻.frf";
                    if (inibglj != "")
                    {
                        sBGGSName = inibglj + "\\" + rptpath2 + "\\冰冻.frf";
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
                        message = "查询数据库异常：" + ex.Message.ToString();
                        return false;
                    }
                    string bc_bggs = "补充";
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
                                bc_bggs = "补充";
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (bc_bggs.Trim() == "")
                        bc_bggs = "补充";

                    if (bcbddytx)
                        bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图";


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
                                    foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                    {
                                        if (name.Trim() != "")
                                            sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                    }
                                }
                            }

                            if (ysname.ToLower().Trim() == "f_fzys")
                            {
                                foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
                                {
                                    if (name.Trim() != "")
                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
                                }
                            }
                        }
                        if (bcbddytx)
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                        else
                            sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + ".frf";
                        if (inibglj != "")
                        {
                            if (bcbddytx)
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                            else
                                sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + ".frf";
                        }
                    }
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
                    if (bcbddytx)
                        bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                    else
                        bggs = bc_bggs + ".frf";
                    sJPGNAME = localpath + "\\" + fileName;

                }
                //判断报告格式是否存在
                if (!File.Exists(sBGGSName))
                {
                    message = "报告格式不存在:" + sBGGSName;
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
                                log.WriteMyLog("pr.print完成");
                            fileName = sJPGNAME.Replace(".", "_1.");

                        }
                        else
                        {
                            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
                            if (debug == "1")
                                log.WriteMyLog("pr.print完成");
                            fileName = sJPGNAME;
                        }

                    }
                    catch (Exception e3)
                    {
                        message = "生成pdf异常，调用prreport异常:" + e3.Message; rtn = false;
                    }

                    if (!File.Exists(fileName))
                    {
                        message = "PDF_JPG-本地未找到文件1";
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
                message = "PDF_JPG方法异常:" + e4.Message;
                return false;
            }
        }

        public bool downtx(string blh, string txml, DataTable dt_tx, ref string txlbs, ref string localpath, ref string err_msg)
        {
            //清空c:\temp_sr目录
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

            //下载FTP参数
            string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = ZgqClass.GetSz("ftp", "pwd", "4s3c2a1p").Replace("\0", "");
            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            //共享目录
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");
            string txm = "";

            if (ftps == "1")//FTP下载方式
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
                            log.WriteMyLog("FTP下载图像出错！");
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
                        log.WriteMyLog("FTP下载图像出错！");
                        return false;
                    }
                }
                return true;

            }
            else //共享下载方式
            {
                if (txpath == "")
                {
                    log.WriteMyLog("sz.ini txpath图像目录未设置");
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
                        log.WriteMyLog("共享目录不存在！");
                        localpath = "";
                        return false;
                    }

                }
                return true;

            }
        }
        public bool downtx2(string blh, string txml, DataTable dt_tx, ref string txlbs, ref string localpath, ref string err_msg)
        {
            //清空c:\temp_sr目录
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

            //下载FTP参数
            string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = ZgqClass.GetSz("ftp", "pwd", "4s3c2a1p").Replace("\0", "");
            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
            string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");

            //共享目录
            string gxuid = f.ReadString("txpath", "username", "").Replace("\0", "");
            string gxpwd = f.ReadString("txpath", "password", "").Replace("\0", "");
            string txm = "";
            string ftpURI = "";
            if (ftps == "1")//FTP下载方式
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
                            log.WriteMyLog("FTP下载图像出错！");
                            err_msg = "FTP下载图像出错-->" + txm;
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
                        log.WriteMyLog("FTP下载图像出错！");
                        err_msg = "FTP下载图像出错-->" + e1.Message;
                        return false;
                    }
                }
                return true;
            }
            else //共享下载方式
            {
                if (txpath == "")
                {
                    log.WriteMyLog("sz.ini txpath图像目录未设置");
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
                                log.WriteMyLog("共享下载图像出错！");
                                err_msg = "共享下载图像出错" + txm;
                                return false;
                            }
                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txm + "</Image>";
                        }
                        catch
                        { }
                    }
                    catch
                    {
                        log.WriteMyLog("共享目录不存在！");
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
                //---上传jpg----------
                //----------------上传至ftp---------------------
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
                                log.WriteMyLog("检查ftp目录。。。");

                            //收到日期目录
                            if (ml.Trim() != "")
                            {
                                //判断目录是否存在
                                if (!ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword, ftpURI, ml))
                                {
                                    //目录不存在，创建

                                    if (!ZgqFtpWeb.FtpMakedir(ftpUserID, ftpPassword, ftpURI, ml, ref  err_msg))
                                    {
                                        err_msg = "FTP创建目录异常";
                                        return false;
                                    }
                                }

                                ftpURI = ftpURI + ml + "/";
                            }

                            //病理号目录
                            //判断目录是否存在
                            // MessageBox.Show("1--"+ftpURI);
                            if (blh.Trim() != "")
                            {

                                if (!ZgqFtpWeb.FtpCheckFile(ftpUserID, ftpPassword, ftpURI, blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";

                                    if (!ZgqFtpWeb.FtpMakedir(ftpUserID, ftpPassword, ftpURI, blh, ref  err_msg))
                                    {
                                        err_msg = "FTP创建目录异常";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI + blh + "/";
                            }


                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件");
                            for (int x = 0; x < 3; x++)
                            {

                                if (ZgqFtpWeb.FtpUpload(ftpUserID, ftpPassword, ftpURI, jpgpath, ref err_msg))
                                    break;
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件结果：" + status + "\r\n" + err_msg);
                            //判断ftp上是否存在该jpg文件
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
                                    err_msg = "PDF上传失败，请重新审核！";
                                    status = "Error";
                                    return false;
                                }

                            }
                            catch (Exception err2)
                            {
                                err_msg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            err_msg = "上传PDF异常:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            err_msg = "sz.ini中[ZGQJK]下toPDFPath图像目录未设置";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //判断ml目录是否存在
                                if (!System.IO.Directory.Exists(tjtxpath + @"\" + ml))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + @"\" + ml);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + @"\" + ml + "--创建目录异常";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + @"\" + ml;
                            }
                            if (blh.Trim() != "")
                            {
                                //判断blh目录是否存在
                                if (!System.IO.Directory.Exists(tjtxpath + "\\" + blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(tjtxpath + "\\" + blh);
                                    }
                                    catch
                                    {
                                        err_msg = tjtxpath + "\\" + blh + "--创建目录异常";
                                        return false;
                                    }
                                }
                                tjtxpath = tjtxpath + "\\" + blh;
                            }

                            //判断共享上是否存在该pdf文件
                            if (File.Exists(tjtxpath + "\\" + jpgname))
                            {
                                //删除共享上的pdf文件
                                File.Delete(tjtxpath + "\\" + jpgname);
                            }
                            //判断共享上是否存在该pdf文件
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
                                err_msg = "上传PDF异常";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            err_msg = "上传异常:" + ee3.Message.ToString();
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
                    err_msg = "未找到文件" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                err_msg = "UpPDF方法异常：" + e4.Message;
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
        /// 连接FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="FtpUserID">用户名</param>
        /// <param name="FtpPassword">密码</param>
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
        /// 下载
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
        /// 创建目录
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
        /// 上传
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
        /// 删除文件
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
        /// 文件存在检查
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
    //    /// 创建pdf或jpg
    //    /// </summary>
    //    /// <param name="F_blh">病理号</param>
    //    /// <param name="bglx">报告类型</param>
    //    /// <param name="bgxh">报告序号</param>
    //    /// <param name="type1">JPG或PDF</param>
    //    /// <param name="err_msg">错误信息</param>
    //    /// <param name="F_ML">目录</param>
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
    //                errmsg = ("连接数据库异常:" + ex.Message);
    //                return false;
    //            }
    //            if (jcxx == null)
    //            {
    //                errmsg = "病理数据库设置有问题！";
    //                return false;
    //            }
    //            if (jcxx.Rows.Count < 1)
    //            {
    //                errmsg = "病理号有错误！";
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

    //            if (bgzt == "已审核" || bgzt == "已发布")
    //            {
    //                if (filename.Trim() == "")
    //                {
    //                    errmsg = "日期不能为空";
    //                    return false;
    //                }

    //                if (type1.ToString().ToLower() == "pdf")
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
    //                else
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";

    //                string rptpath = ZgqClass.GetSz("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();

    //                //生成PDF
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
    //                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF成功", "ZGQJK", "生成PDF");
    //                    bool ssa = UpPDF(blh, filename, ml, 0, ref errmsg, ref pdfpath);
    //                    if (ssa == true)
    //                    {
    //                        if (debug == "1")
    //                            log.WriteMyLog("上传PDF成功");
    //                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
    //                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + ml + "\\" + filename, "ZGQJK", "上传PDF");

    //                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
    //                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_pdfPath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" + blh + "','" + filename + "','" + pdfpath + "')");
    //                    }
    //                    else
    //                    {
    //                        log.WriteMyLog("上传PDF失败：" + errmsg);
    //                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
    //                    }
    //                    DeleteTempFile(blh);
    //                    return true;
    //                }
    //                else
    //                {
    //                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：未找到文件" + filename, "ZGQJK", "生成PDF");
    //                    log.WriteMyLog("未找到文件" + filename);
    //                    errmsg = "未找到文件" + filename;
    //                    DeleteTempFile(blh);
    //                    return false;
    //                }

    //            }
    //            else
    //            {
    //                errmsg = "报告未审核";
    //                return false;
    //            }
    //        }
    //        catch (Exception e4)
    //        {
    //            errmsg = "SC_PDF方法异常：" + e4.Message;
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
    //                err_msg = ("连接数据库异常:" + ex.Message);
    //                return false;
    //            }
    //            if (jcxx == null)
    //            {
    //                err_msg = "病理数据库设置有问题！";
    //                return false;
    //            }
    //            if (jcxx.Rows.Count < 1)
    //            {
    //                err_msg = "病理号有错误！";
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




    //            if (bgzt == "已审核" || bgzt == "已发布")
    //            {
    //                if (filename.Trim() == "")
    //                {
    //                    err_msg = "日期不能为空";
    //                    return false;
    //                }

    //                if (type1.ToString().ToLower() == "pdf")
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
    //                else
    //                    filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".jpg";


    //                string rptpath = ZgqClass.GetSz("savetohis", "rptpath", "rpt").Replace("\0", "").Trim();
    //                jpgname = "";

    //                //生成PDF
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
    //                    err_msg = "未找到PDF文件";
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                err_msg = "报告未审核";
    //                return false;
    //            }
    //        }
    //        catch (Exception e4)
    //        {
    //            err_msg = "CreatePDF方法异常：" + e4.Message;
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
    //                message = "查询数据库异常：" + ex.Message.ToString();
    //                return false;
    //            }

    //            //清空c:\temp目录
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

    //                log.WriteMyLog("下载图片失败：" + message);
    //                return false;
    //            }

    //            for (int i = 0; i < txlb.Rows.Count; i++)
    //            {
    //                if (File.Exists(localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString()))
    //                {
    //                    filename = "";
    //                    message = "下载图像错误，未找到图片：" + localpath + "\\" + F_blh + "\\" + txlb.Rows[i]["F_TXM"].ToString();
    //                    return false;
    //                }
    //            }

    //            if (localpath == "")
    //            {
    //                filename = "";
    //                message = "PDF_JPG方法下载图像错误";
    //                return false;
    //            }

    //            string sbmp = "";
    //            string stxsm = "";
    //            string sJPGNAME = localpath + "\\" + filename;

    //            //   string sJPGNAME = localpath + "\\" + F_blh + ".jpg";
    //            string rptpath2 = "rpt";
    //            if (rptpath.Trim() != "")
    //                rptpath2 = rptpath;

    //            string sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //            string inibglj = ZgqClass.GetSz("dybg", "dybglj", "").Replace("\0", "");

    //            if (inibglj.Trim() == "")
    //            {
    //                inibglj = Application.StartupPath.ToString();
    //            }
    //            if (inibglj != "")
    //            {
    //                sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
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
    //                            foreach (string name in jcxx.Rows[0]["f_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
    //                            {
    //                                if (name.Trim() != "")
    //                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                            }
    //                        }
    //                        // }
    //                    }




    //                    if (ysname.ToLower().Trim() == "f_fzys")
    //                    {
    //                        foreach (string name in jcxx.Rows[0]["f_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
    //                        {
    //                            if (name.Trim() != "")
    //                                sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                        }
    //                    }
    //                }


    //                sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //                if (inibglj != "")
    //                {
    //                    sBGGSName = inibglj + "\\rpt-szqm\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //                }
    //            }

    //            string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //            //  string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + F_blh + "'";

    //            string sSQL_DY = "SELECT * FROM T_JCXX left join T_TBS_BG  on  T_JCXX.F_BLH=T_TBS_BG.F_BLH  WHERE  T_JCXX.F_BLH = '" + F_blh + "'";
    //            bool bcbddytx = false;
    //            if (f.ReadInteger("bcbddytx", "bcbddytx", 0) == 1)
    //                bcbddytx = true;

    //            if (bglx.ToLower() == "bd")
    //            {
    //                sBGGSName = Application.StartupPath.ToString() + "\\" + rptpath2 + "\\冰冻.frf";
    //                sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BD_BGXH='" + bgxh + "'";
    //                if (bcbddytx)
    //                    bggs = "冰冻" + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //                else
    //                    bggs = "冰冻.frf";
    //                if (inibglj != "")
    //                {
    //                    sBGGSName = inibglj + "\\" + rptpath2 + "\\冰冻.frf";
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
    //                    message = "查询数据库异常：" + ex.Message.ToString();
    //                    return false;
    //                }


    //                string bc_bggs = "补充";

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
    //                            bc_bggs = "补充";
    //                        }
    //                    }
    //                }
    //                catch
    //                {
    //                }
    //                if (bc_bggs.Trim() == "")
    //                    bc_bggs = "补充";

    //                if (bcbddytx)
    //                    bc_bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图";


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
    //                                foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
    //                                {

    //                                    if (name.Trim() != "")
    //                                        sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                                }
    //                            }
    //                        }

    //                        if (ysname.ToLower().Trim() == "f_fzys")
    //                        {
    //                            foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
    //                            {
    //                                if (name.Trim() != "")
    //                                    sbmp = sbmp + bmppath + "\\" + name + ".bmp,";
    //                            }
    //                        }
    //                    }

    //                    // sbmp = sbmp + bmppath + "\\" + jcxx.Rows[0]["F_shys"].ToString().Trim() + ".bmp,";
    //                    if (bcbddytx)
    //                        sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //                    else
    //                        sBGGSName = Application.StartupPath.ToString() + "\\rpt-szqm\\" + bc_bggs + ".frf";
    //                    if (inibglj != "")
    //                    {
    //                        if (bcbddytx)
    //                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //                        else
    //                            sBGGSName = inibglj + "\\rpt-szqm\\" + bc_bggs + ".frf";

    //                    }
    //                }


    //                sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + F_blh + "' and F_BC_BGXH='" + bgxh + "'";
    //                if (bcbddytx)
    //                    bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
    //                else
    //                    bggs = bc_bggs + ".frf";
    //                sJPGNAME = localpath + "\\" + filename;

    //            }
    //            //判断报告格式是否存在
    //            if (!File.Exists(sBGGSName))
    //            {
    //                message = "报告格式不存在:" + sBGGSName;
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
    //                            log.WriteMyLog("pr.print完成");
    //                        filename = sJPGNAME.Replace(".", "_1.");

    //                    }
    //                    else
    //                    {
    //                        //if (debug == "1")
    //                        //    log.WriteMyLog("pr.print开始\r\n" + sSQL_DY + "\r\n" + sbmp + "\r\n" + stxsm + "\r\n" + sBGGSName + "\r\n" + sJPGNAME);
    //                        pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME, debug);
    //                        if (debug == "1")
    //                            log.WriteMyLog("pr.print完成");
    //                        filename = sJPGNAME;
    //                    }

    //                }
    //                catch (Exception e3)
    //                {
    //                    message = "生成pdf异常，调用prreport异常:" + e3.Message; rtn = false;
    //                }

    //                if (!File.Exists(filename))
    //                {
    //                    message = "PDF_JPG-本地未找到文件1";
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
    //            message = "PDF_JPG方法异常:" + e4.Message;
    //            return false;
    //        }
    //    }

    //    /// <summary>
    //    /// 下载
    //    /// </summary>
    //    /// <param name="f_blh">病理号</param>
    //    /// <param name="txml">图像目录</param>
    //    /// <param name="aa"></param>
    //    /// <param name="txlbs"></param>
    //    /// <param name="localpath">临时目录</param>
    //    /// <param name="err_msg">错误信息</param>
    //    /// <returns>成功或者失败</returns>
    //    public bool downtx(string f_blh, string txml, odbcdb aa, ref string txlbs, ref string localpath, ref string err_msg)
    //    {

    //        try
    //        {
    //            //清空c:\temp目录
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
    //            //临时目录
    //            localpath = @"c:\temp\" + f_blh;

    //            //下载FTP参数
    //            string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
    //            string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
    //            string ftppwd = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
    //            string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
    //            string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
    //            string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
    //            string txpath = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");

    //            ZGQ_FTP zgqftp = new ZGQ_FTP();

    //            //FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
    //            //共享目录
    //            string gxml = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");
    //            //string gxuid = ZgqClass.GetSz("txpath", "username", "").Replace("\0", "");
    //            //string gxpwd = ZgqClass.GetSz("txpath", "password", "").Replace("\0", "");


    //            DataTable txlb = aa.GetDataTable("select * from T_tx where F_blh='" + f_blh + "' and F_sfdy='1'", "txlb");
    //            string txm = "";

    //            if (ftps == "1")//FTP下载方式
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
    //                                    log.WriteMyLog("压缩图像异常：" + ee2.Message);
    //                                }

    //                            }
    //                            txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";

    //                        }
    //                    }
    //                    catch (Exception ee2)
    //                    {
    //                        err_msg = "FTP下载图像出错:" + ee2.Message;
    //                        localpath = "";
    //                        return false;
    //                    }
    //                }

    //                return true;
    //            }
    //            else //共享下载方式
    //            {
    //                if (txpath == "")
    //                {
    //                    err_msg = "sz.ini txpath图像目录未设置";
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
    //                                    log.WriteMyLog("压缩图像异常：" + ee2.Message);
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            err_msg = "共享下载图像失败";
    //                            return false;
    //                        }
    //                        txlbs = txlbs + "<Image INDEX=" + (char)34 + (i + 1).ToString() + (char)34 + ">" + localpath + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + "</Image>";
    //                    }

    //                    catch (Exception ee3)
    //                    {
    //                        err_msg = "共享下载图像错误:" + ee3.Message;
    //                        localpath = "";
    //                        return false;
    //                    }
    //                }
    //                return true;
    //            }
    //        }
    //        catch (Exception e4)
    //        {

    //            err_msg = "下载图像异常:" + e4.Message;
    //            return false;
    //        }

    //    }

    //    /// 
    //    /// <summary>
    //    /// 上传
    //    /// </summary>
    //    /// <param name="blh">病理号</param>
    //    /// <param name="jpgpath">文件名，完整路径</param>
    //    /// <param name="ml">目录</param>
    //    /// <param name="err_msg">错误信息</param>
    //    /// <param name="lb">1:指定共享目录上传（[ZGQJK]toPDFPath=)；2:共享路径上传([txpath]txpath=)；3:ftp上传([ftp]);其他ftp上传([ftpup])  </param>
    //    /// <returns></returns>
    //    /// 
    //    public bool UpPDF(string blh, string jpgpath, string ml, int lb, ref string err_msg, ref string pdfpath)
    //    {
    //        try
    //        {
    //            string jpgname = jpgpath.Substring(jpgpath.LastIndexOf('\\') + 1);
    //            //---上传jpg----------
    //            //----------------上传至ftp---------------------
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
    //                            log.WriteMyLog("检查ftp目录。。。");

    //                        //收到日期目录
    //                        if (ml.Trim() != "")
    //                        {
    //                            //判断目录是否存在
    //                            if (!fw.fileCheckExist(ftpURI, ftpUserID, ftpPassword, ml))
    //                            {
    //                                //目录不存在，创建
    //                                string stat = "";
    //                                fw.Makedir(ftpURI, ftpUserID, ftpPassword, ml, out stat);
    //                                if (stat != "OK")
    //                                {
    //                                    err_msg = "FTP创建目录异常";
    //                                    return false;
    //                                }
    //                            }

    //                            ftpURI = ftpURI + ml + "/";
    //                        }

    //                        //病理号目录
    //                        //判断目录是否存在
    //                        // MessageBox.Show("1--"+ftpURI);
    //                        if (blh.Trim() != "")
    //                        {
    //                            if (!fw.fileCheckExist(ftpURI, ftpUserID, ftpPassword, blh))
    //                            {
    //                                //目录不存在，创建
    //                                string stat = "";

    //                                fw.Makedir(ftpURI, ftpUserID, ftpPassword, blh, out stat);

    //                                if (stat != "OK")
    //                                {
    //                                    err_msg = "FTP创建目录异常";
    //                                    return false;
    //                                }
    //                            }
    //                            ftpURI = ftpURI + blh + "/";
    //                        }


    //                        if (debug == "1")
    //                            log.WriteMyLog("上传新生成的文件");
    //                        for (int x = 0; x < 3; x++)
    //                        {
    //                            fw.FtpUpload(ftpURI, ftpUserID, ftpPassword, jpgpath, out status, ref err_msg);
    //                            if (status != "Error")
    //                                break;
    //                        }
    //                        if (status == "Error")
    //                        {
    //                            err_msg = "PDF上传失败，请重新审核！";
    //                            status = "Error";
    //                        }
    //                        if (debug == "1")
    //                            log.WriteMyLog("上传新生成的文件结果：" + status + "\r\n" + err_msg);
    //                        //判断ftp上是否存在该jpg文件
    //                        try
    //                        {
    //                            if (fw.fileCheckExist(ftpURI, ftpUserID, ftpPassword, jpgname))
    //                            {
    //                                status = "OK";
    //                                pdfpath = ftpURI + jpgname;
    //                            }
    //                            else
    //                            {
    //                                err_msg = "PDF上传失败，请重新审核！";
    //                                status = "Error";
    //                            }

    //                        }
    //                        catch (Exception err2)
    //                        {
    //                            err_msg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
    //                            status = "Error";
    //                            return false;
    //                        }
    //                    }
    //                    catch (Exception eee)
    //                    {
    //                        err_msg = "上传PDF异常:" + eee.Message.ToString();
    //                        status = "Error";
    //                        return false;
    //                    }
    //                }
    //                else
    //                {
    //                    if (tjtxpath == "")
    //                    {
    //                        err_msg = "sz.ini中[ZGQJK]下toPDFPath图像目录未设置";
    //                        return false;
    //                    }
    //                    try
    //                    {
    //                        if (ml.Trim() != "")
    //                        {
    //                            //判断ml目录是否存在
    //                            if (!System.IO.Directory.Exists(tjtxpath + @"\" + ml))
    //                            {
    //                                //目录不存在，创建
    //                                string stat = "";
    //                                try
    //                                {
    //                                    System.IO.Directory.CreateDirectory(tjtxpath + @"\" + ml);
    //                                }
    //                                catch
    //                                {
    //                                    err_msg = tjtxpath + @"\" + ml + "--创建目录异常";
    //                                    return false;
    //                                }
    //                            }
    //                            tjtxpath = tjtxpath + @"\" + ml;
    //                        }
    //                        if (blh.Trim() != "")
    //                        {
    //                            //判断blh目录是否存在
    //                            if (!System.IO.Directory.Exists(tjtxpath + "\\" + blh))
    //                            {
    //                                //目录不存在，创建
    //                                string stat = "";
    //                                try
    //                                {
    //                                    System.IO.Directory.CreateDirectory(tjtxpath + "\\" + blh);
    //                                }
    //                                catch
    //                                {
    //                                    err_msg = tjtxpath + "\\" + blh + "--创建目录异常";
    //                                    return false;
    //                                }
    //                            }
    //                            tjtxpath = tjtxpath + "\\" + blh;
    //                        }

    //                        //判断共享上是否存在该pdf文件
    //                        if (File.Exists(tjtxpath + "\\" + jpgname))
    //                        {
    //                            //删除共享上的pdf文件
    //                            File.Delete(tjtxpath + "\\" + jpgname);
    //                        }
    //                        //判断共享上是否存在该pdf文件
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
    //                            err_msg = "上传PDF异常";
    //                            return false;
    //                        }
    //                    }
    //                    catch (Exception ee3)
    //                    {
    //                        err_msg = "上传异常:" + ee3.Message.ToString();
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
    //                err_msg = "未找到文件" + jpgpath + "";
    //                return false;
    //            }
    //        }
    //        catch (Exception e4)
    //        {
    //            err_msg = "UpPDF方法异常：" + e4.Message;
    //            return false;
    //        }

    //    }

    //    /// <summary>
    //    /// 删除
    //    /// </summary>
    //    /// <param name="ml">目录</param>
    //    /// <param name="jpgname">文件名</param>
    //    /// <param name="err_msg">错误信息</param>
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

    //                //判断ftp上是否存在该jpg文件
    //                if (fw.fileCheckExist(ftpURI, jpgname))
    //                {
    //                    //删除ftp上的jpg文件
    //                    fw.fileDelete(ftpURI, jpgname).ToString();
    //                }
    //                return true;
    //            }
    //            catch (Exception eee)
    //            {
    //                err_msg = "删除ftp上PDF异常:" + eee.Message;
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            if (tjtxpath == "")
    //            {
    //                err_msg = "sz.ini中[savetohis]下PDFPath图像目录未设置";
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
    //                err_msg = "删除文件异常:" + ee.Message.ToString();
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

    //                //判断ftp上是否存在该jpg文件
    //                if (fw.fileCheckExist(ftpURI, jpgname))
    //                {
    //                    //删除ftp上的jpg文件
    //                    fw.fileDelete(ftpURI, jpgname).ToString();
    //                }
    //                return true;
    //            }
    //            catch (Exception eee)
    //            {
    //                err_msg = "删除ftp上PDF异常:" + eee.Message;
    //                return false;
    //            }
    //        }
    //        else
    //        {
    //            if (tjtxpath == "")
    //            {
    //                err_msg = "sz.ini中[savetohis]下PDFPath图像目录未设置";
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
    //                err_msg = "删除文件异常:" + ee.Message.ToString();
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
    //    /// 清空本地临时文件
    //    /// </summary>
    //    /// <param name="blh">病理号</param>
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
    //    /// 文件存在检查
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
    //                //  log.WriteMyLog("关闭数据流异常");
    //            }
    //        }
    //        return success;

    //    }
    //}

}
