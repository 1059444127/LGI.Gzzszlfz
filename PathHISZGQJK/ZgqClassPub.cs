
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
                    errMsg = ("抛出异常:" + ex.Message.ToString());
                    //异常
                    return false;
                }
                if (jcxx == null)
                {
                    errMsg = "病理数据库设置有问题！";
                    return false;
                }
                if (jcxx.Rows.Count < 1)
                {
                    errMsg = "病理号有错误！";
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
                            errMsg = "病理数据库设置有问题T_BDBG";
                            return false;
                        }
                        if (dt_bd.Rows.Count < 1)
                        {
                            errMsg = "病理号有错误T_BDBG";
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
                            errMsg = "病理数据库设置有问题T_BCBG";
                            return false;
                        }
                        if (dt_bc.Rows.Count < 1)
                        {
                            errMsg = "病理号有错误T_BCBG";
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
                    log.WriteMyLog("报告不存在:" + blh + bglx + bgxh);
                    errMsg = "报告不存在:" + blh + bglx + bgxh;
                    return false;
                }
                if (bgzt != "已审核" && bgzt != "已发布")
                {
                    errMsg = "报告未审核";
                    return false;
                }
                if (filename.Trim() == "")
                {
                    errMsg = "日期不能为空";
                    return false;
                }
                if (bgzt == "已审核" || bgzt == "已发布")
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
                        errMsg = "未找到PDF文件" + fileName;
                        return false;
                    }
                }
                else
                {
                    errMsg = "报告未审核";
                    return false;
                }
            }
            catch (Exception e4)
            {
                errMsg = "CreatePDF方法异常：" + e4.Message;
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
                        errMsg = "[CreatePDF]"+blh+"生成失败:病理数据库设置有问题T_jcxx";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    errMsg = "[CreatePDF]" + blh + "生成失败:查询数据库T_JCXX异常," + ex.Message.ToString();
                    return false;
                }

                //清空c:\temp目录
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
                    if (bgzt != "已审核" && bgzt != "已发布")
                    {
                        errMsg = "[CreatePDF]"+blh+"生成失败:常规报告未审核或未发布";
                        return false;
                    }
                }


                DataTable txlb = aa.GetDataTable("select  * from T_tx where F_blh='" + blh + "' and F_sfdy='1'", "txlb");
                string txlbs = "";

                if (txlb == null)
                {
                    errMsg = "[CreatePDF]" + blh + "生成失败:病理数据库设置有问题T_tx";
                    return false;
                }
                if (!downtx(blh, jcxx.Rows[0]["F_txml"].ToString().Trim(), txlb, ref txlbs, localPath, ref errMsg))
                {
                    errMsg = "[CreatePDF]" + blh + "生成失败:下载图片失败," + errMsg;
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
                string bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
                string bmppath = ZgqClass.GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");
                string yszmlb = ZgqClass.GetSz("All", "yszmlb", "f_shys");      
                if(bglx == "cg")
                {
                    bggs = jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
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
                            errMsg = "[CreatePDF]" + blh + "生成失败:查询数据异常T_BDBG";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsg = "[CreatePDF]" + blh + "生成失败:查询数据异常T_BDBG," + ex.Message.ToString();
                        return false;
                    }
                    string bd_bggs = "冰冻";
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
                        errMsg = "[CreatePDF]" + blh + "生成失败:查询数据异常T_BDBG";
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
                                        foreach (string name in BDBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
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
                                        foreach (string name in BDBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
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
                        errMsg = "[CreatePDF]" + blh + "生成失败:冰冻异常," + ee_bc.Message;
                        return false;
                    }
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG WHERE T_JCXX.F_BLH = T_BDBG.F_BLH AND T_JCXX.F_BLH ='" + blh + "' and F_BD_BGXH='" + bgxh + "'";

                       if (bd_bggs.Trim() == "")
                        bd_bggs = "冰冻";
      
                        if (bcbddytx)
                            bggs = bd_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
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
                            errMsg = "[CreatePDF]" + blh + "生成失败:查询数据异常T_BCBG";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMsg = "[CreatePDF]" + blh + "生成失败::补充报告异常T_BCBG," + ex.Message.ToString();
                        return false;
                    }
                    string bc_bggs = "补充";
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
                            errMsg = "[CreatePDF]" + blh + "生成失败:查询数据异常T_BCBG";
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
                                        foreach (string name in BCBG.Rows[0]["f_bc_bgys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
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
                                    foreach (string name in BCBG.Rows[0]["f_bc_fzys"].ToString().Trim().Replace(',', '/').Replace('，', '/').Split('/'))
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
                        errMsg = "[CreatePDF]" + blh + "生成失败:补充生成PDF异常," + ee_bc.Message;
                        return false;
                    }
                        if (bc_bggs.Trim() == "")
                            bc_bggs = "补充";

                        if (bcbddytx)
                            bggs = bc_bggs + "-" + txlb.Rows.Count.ToString() + "图.frf";
                        else
                            bggs = bc_bggs + ".frf";
                    sSQL_DY = "SELECT * FROM T_JCXX,T_BCBG WHERE T_JCXX.F_BLH = T_BCBG.F_BLH AND T_JCXX.F_BLH ='" + blh + "' and F_BC_BGXH='" + bgxh + "'";
                }

                    if (bgzt != "已审核" && bgzt != "已发布")
                    {
                        errMsg = "[CreatePDF]" + blh + "生成失败:"+bglx+bgxh+"报告未审核或未发布";
                        return false;
                    }


                  foreach(string bmp in  sbmp.Split(','))
                    {
                        if (bmp.Trim() != "")
                        {
                            if (!File.Exists(bmp))
                            {
                                errMsg = "[CreatePDF]" + blh + "生成失败:未找到图片," + bmp;
                            return false;
                            }
                        }
                    }
              
                string inibglj = ZgqClass.GetSz("dybg", "dybglj", "").Replace("\0", "");

                if (inibglj.Trim() == "")
                    inibglj = Application.StartupPath.ToString();

                string sBGGSName = inibglj + "\\" + rptpath2 + "\\" + jcxx.Rows[0]["F_bggs"].ToString().Trim() + "-" + txlb.Rows.Count.ToString() + "图.frf";
              
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

                //判断报告格式是否存在
                if (!File.Exists(sBGGSName))
                {
                    errMsg = "[CreatePDF]" + blh + "生成失败:报告格式不存在," + sBGGSName;
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
                                log.WriteMyLog("Createjpg:pr.print完成");
                            fileName = sJPGNAME.Replace(".", "_1.");

                        }
                        else
                        {
                            pr.printpdf(sSQL_DY, IntPtr.Zero, sbmp, stxsm, sBGGSName, sJPGNAME,"");
                            if (debug == "1")
                                log.WriteMyLog("CreatePDF:pr.print完成");
                            fileName = sJPGNAME;
                        }

                    }
                    catch (Exception e3)
                    {
                        errMsg = "[CreatePDF]" + blh + "生成失败:调用prreport异常," + e3.Message; 
                        rtn = false;
                    }

                    if (!File.Exists(fileName))
                    {
                        errMsg = "[CreatePDF]" + blh + "生成失败:本地未找到文件" + fileName;
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
                errMsg = "[CreatePDF]" + blh + "生成失败:异常," + e4.Message;
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
                //清空c:\temp目录
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
                //下载FTP参数
                string ftpserver = ZgqClass.GetSz("ftp", "ftpip", "").Replace("\0", "");
                string ftpuser = ZgqClass.GetSz("ftp", "user", "ftpuser").Replace("\0", "");
                string ftppwd = ZgqClass.GetSz("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = ZgqClass.GetSz("ftp", "ftplocal", "c:\\temp\\").Replace("\0", "");
                string ftpremotepath = ZgqClass.GetSz("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                string ftps = ZgqClass.GetSz("ftp", "ftp", "").Replace("\0", "");
                string txpath = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");

                FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
                //共享目录
                string gxml = ZgqClass.GetSz("txpath", "txpath", "").Replace("\0", "");

                DataTable dt_tx = aa.GetDataTable("select * from T_tx where F_blh='" + blh + "' and F_sfdy='1'", "txlb");
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
                                    break;
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
                                    { }
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
            catch (Exception e4)
            {

                errMsg = "下载图像异常:" + e4.Message;
                return false;
            }

        }
        public bool downtx(string blh, string txml, DataTable dt_tx, ref string txlbs, string localpath, ref string errMsg)
        {

            if (localpath == "")
                localpath = @"c:\temp";

            //清空c:\temp_sr目录
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
                                break;
                        }
                        if (ftpstatus == "Error")
                        {
                            log.WriteMyLog("FTP下载图像出错！");
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



        public bool UpPDF(string blh, string jpgpath, string ml, ref string errMsg, int lb)
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
                                        errMsg = "FTP创建目录异常";
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
                                    errMsg = "FTP创建目录异常";
                                    return false;
                                }
                            }
                            ftpURI = ftpURI + "/" + blh + "/";

                            //if (debug=="1")
                            // LGZGQClass.log.WriteMyLog("判断ftp上是否存在该文件");
                            //判断ftp上是否存在该jpg文件
                            //if (fw.fileCheckExist(ftpURI, jpgname))
                            //{
                            //    //删除ftp上的jpg文件
                            //    fw.fileDelete(ftpURI, jpgname).ToString();
                            //}
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件");
                            fw.Upload(jpgpath, ml + "/" + blh, out status, ref errMsg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                errMsg = "PDF上传失败，请重新审核！";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件结果：" + status + "\r\n" + errMsg);
                            //判断ftp上是否存在该jpg文件
                            try
                            {

                                if (fw.fileCheckExist(ftpURI, jpgname))
                                {
                                    status = "OK";
                                }
                                else
                                {
                                    errMsg = "PDF上传失败，请重新审核！";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                errMsg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            errMsg = "上传PDF异常:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            errMsg = "sz.ini中[ZGQJK]下toPDFPath图像目录未设置";
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
                                        errMsg = tjtxpath + "\\" + ml + "\\" + blh + "--创建目录异常";
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
                                errMsg = "上传PDF异常";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            errMsg = "上传异常:" + ee3.Message.ToString();
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
                    errMsg = "未找到文件" + jpgpath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                errMsg = "UpPDF方法异常：" + e4.Message;
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
                //---上传jpg----------
                //----------------上传至ftp---------------------
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
                                        errMsg = "FTP创建目录异常";
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
                                if (!fw.fileCheckExist(ftpURI, blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";

                                    fw.Makedir(ftpURI, blh, out stat);

                                    if (stat != "OK")
                                    {
                                        errMsg = "FTP创建目录异常";
                                        return false;
                                    }
                                }
                                ftpURI = ftpURI + blh + "/";

                                ml = ml + "/" + blh;
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件");
                            fw.Upload(filePath, ml, out status, ref errMsg);
                            //  Thread.Sleep(1000);
                            if (status == "Error")
                            {
                                errMsg = "PDF上传失败，请重新审核！";
                                status = "Error";
                            }
                            if (debug == "1")
                                log.WriteMyLog("上传新生成的文件结果：" + status + "\r\n" + errMsg);
                            //判断ftp上是否存在该jpg文件
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
                                    errMsg = "PDF上传失败，请重新审核！";
                                    status = "Error";
                                }

                            }
                            catch (Exception err2)
                            {
                                errMsg = "检查该文件是否上传成功异常:" + err2.Message.ToString() + "\r\n" + ftpURI + jpgname;
                                status = "Error";
                                return false;
                            }
                        }
                        catch (Exception eee)
                        {
                            errMsg = "上传PDF异常:" + eee.Message.ToString();
                            status = "Error";
                            return false;
                        }
                    }
                    else
                    {
                        if (txpath == "")
                        {
                            errMsg = "sz.ini中[" + sz_section + "]下toPDFPath图像目录未设置";
                            return false;
                        }
                        try
                        {
                            if (ml.Trim() != "")
                            {
                                //判断ml目录是否存在
                                if (!System.IO.Directory.Exists(txpath + "\\" + ml))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(txpath + "\\" + ml);
                                    }
                                    catch
                                    {
                                        errMsg = txpath + "\\" + ml + "--创建目录异常";
                                        return false;
                                    }
                                }

                                txpath = txpath + "\\" + ml;
                            }
                            if (blh.Trim() != "")
                            {
                                //判断ml目录是否存在
                                if (!System.IO.Directory.Exists(txpath + "\\" + blh))
                                {
                                    //目录不存在，创建
                                    string stat = "";
                                    try
                                    {
                                        System.IO.Directory.CreateDirectory(txpath + "\\" + blh);
                                    }
                                    catch
                                    {
                                        errMsg = txpath + "\\" + blh + "--创建目录异常";
                                        return false;
                                    }
                                }
                                ml = ml + "\\" + blh;
                                txpath = txpath + "\\" + blh;
                            }
                            //判断共享上是否存在该pdf文件
                            if (File.Exists(txpath + "\\" + jpgname))
                            {
                                //删除共享上的pdf文件
                                File.Delete(txpath + "\\" + jpgname);
                            }
                            //判断共享上是否存在该pdf文件

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
                                errMsg = "上传PDF异常";
                                return false;
                            }
                        }
                        catch (Exception ee3)
                        {
                            errMsg = "上传异常:" + ee3.Message.ToString();
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
                    errMsg = "未找到文件:" + filePath + "";
                    return false;
                }
            }
            catch (Exception e4)
            {
                errMsg = "UpPDF方法异常：" + e4.Message;
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
                    //判断ftp上是否存在该jpg文件
                    if (fw.fileCheckExist(ftpURI, fileName))
                    {
                        //删除ftp上的jpg文件
                        fw.fileDelete(ftpURI, fileName).ToString();
                    }
                    return true;
                }
                catch (Exception eee)
                {
                    errMsg = "删除ftp上PDF异常:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    errMsg = "sz.ini中[savetohis]下PDFPath图像目录未设置";
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
                    errMsg = "删除文件异常:" + ee.Message.ToString();
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
                  //删除ftp上的jpg文件
                    fw.fileDelete(filePath).ToString();
                    return true;
                }
                catch (Exception eee)
                {
                    errMsg = "删除ftp上PDF异常:" + eee.Message;
                    return false;
                }
            }
            else
            {
                if (tjtxpath == "")
                {
                    errMsg = "sz.ini中[savetohis]下PDFPath图像目录未设置";
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
                    errMsg = "删除文件异常:" + ee.Message.ToString();
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
        /// 文件存在检查
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
                    //   LGZGQClass.log.WriteMyLog("关闭数据流异常");
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
        /// 连接FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="FtpUserID">用户名</param>
        /// <param name="FtpPassword">密码</param>
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
        /// 下载
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
        /// 删除文件
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
        /// 文件存在检查
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
                    //   LGZGQClass.log.WriteMyLog("关闭数据流异常");
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
        /// 上传文件到Ftp服务器
        /// </summary>
        /// <param name="uri">把上传的文件保存为ftp服务器文件的uri,如ftp://192.168.1.104/pathiamges/123.jpg</param>
        /// <param name="UpLoadFile">要上传的本地的文件路径，如C:\temp\123.jpg</param>

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
                throw new Exception("FTP上传文件异常:" + ex.Message.ToString());
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
                ErrMsg = "FTP上传文件异常:" + ex.Message.ToString();
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
                ErrMsg = "FTP上传文件异常:" + ex.Message.ToString();
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
        /// 从ftp下载文件到本地服务器
        /// </summary>
        /// <param name="downloadUrl">要下载的ftp文件路径，如ftp://192.168.1.104/pathimages/123.jpg</param>
        /// <param name="saveFileUrl">本地保存文件的路径，如(@"C:\temp\123.jpg"</param>
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
                throw new Exception("FTP下载文件异常:" + ex.Message.ToString());
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
                ErrMsg = "下载文件异常:" + ex.Message.ToString();
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
                ErrMsg = "下载文件异常:" + ex.Message.ToString();
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
        /// 从FTP下载文件到本地服务器,支持断点下载
        /// </summary>
        /// <param name="ftpUri">ftp文件路径，如"ftp://localhost/test.txt"</param>
        /// <param name="saveFile">保存文件的路径，如C:\\test.txt</param>
        public void BreakPointDownLoadFile(string ftpUri, string saveFile)
        {
            System.IO.FileStream fs = null;
            System.Net.FtpWebResponse ftpRes = null;
            System.IO.Stream resStrm = null;
            try
            {
                //下载文件的URI
                Uri u = new Uri(ftpUri);
                //设定下载文件的保存路径
                string downFile = saveFile;
                //FtpWebRequest的作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //设定用户名和密码
                ftpReq.Credentials = new System.Net.NetworkCredential(ftpUser, ftpPassWord);
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")设定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求终了后关闭连接
                ftpReq.KeepAlive = false;
                //使用ASCII方式传送
                ftpReq.UseBinary = false;
                //设定PASSIVE方式无效
                ftpReq.UsePassive = false;

                //判断是否继续下载
                //继续写入下载文件的FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //继续下载
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //一般下载
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //取得FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //为了下载文件取得Stream
                resStrm = ftpRes.GetResponseStream();
                //写入下载的数据
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
                throw new Exception("FTP下载文件异常:" + ex.Message.ToString());
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
                //下载文件的URI
                Uri u = new Uri(ftpUri);
                //设定下载文件的保存路径
                string downFile = saveFile;
                //FtpWebRequest的作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //设定用户名和密码
                ftpReq.Credentials = new System.Net.NetworkCredential(ftpUser, ftpPassWord);
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")设定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求终了后关闭连接
                ftpReq.KeepAlive = false;
                //使用ASCII方式传送
                ftpReq.UseBinary = false;
                //设定PASSIVE方式无效
                ftpReq.UsePassive = false;

                //判断是否继续下载
                //继续写入下载文件的FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //继续下载
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //一般下载
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //取得FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //为了下载文件取得Stream
                resStrm = ftpRes.GetResponseStream();
                //写入下载的数据
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
                ErrMsg = ("FTP下载文件异常:" + ex.Message.ToString());
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
                //下载文件的URI
                Uri u = new Uri(ftpUri);
                //设定下载文件的保存路径
                string downFile = saveFile;
                //FtpWebRequest的作成
                System.Net.FtpWebRequest ftpReq = (System.Net.FtpWebRequest)
                    System.Net.WebRequest.Create(u);
                //设定用户名和密码
                ftpReq.Credentials = new System.Net.NetworkCredential(FtpUser, FtpPassWord);
                //MethodにWebRequestMethods.Ftp.DownloadFile("RETR")设定
                ftpReq.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                //要求终了后关闭连接
                ftpReq.KeepAlive = false;
                //使用ASCII方式传送
                ftpReq.UseBinary = false;
                //设定PASSIVE方式无效
                ftpReq.UsePassive = false;

                //判断是否继续下载
                //继续写入下载文件的FileStream

                if (System.IO.File.Exists(downFile))
                {
                    //继续下载
                    ftpReq.ContentOffset = (new System.IO.FileInfo(downFile)).Length;
                    fs = new System.IO.FileStream(
                       downFile, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
                else
                {
                    //一般下载
                    fs = new System.IO.FileStream(
                        downFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                }

                //取得FtpWebResponse
                ftpRes = (System.Net.FtpWebResponse)ftpReq.GetResponse();
                //为了下载文件取得Stream
                resStrm = ftpRes.GetResponseStream();
                //写入下载的数据
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
                ErrMsg = ("FTP下载文件异常:" + ex.Message.ToString());
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


        #region 从FTP上下载整个文件夹，包括文件夹下的文件和文件夹

        /// <summary>
        /// 列出FTP服务器上面当前目录的所有文件和目录
        /// </summary>
        /// <param name="ftpUri">FTP目录</param>
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
                throw new Exception("获取文件列表出错，错误信息如下：" + ex.ToString());
            }
            string Datastring = ftpFileListReader.ReadToEnd();
            return GetList(Datastring);

        }

        /// <summary>
        /// 列出FTP目录下的所有文件
        /// </summary>
        /// <param name="ftpUri">FTP目录</param>
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
        /// 列出FTP目录下的所有目录
        /// </summary>
        /// <param name="ftpUri">FRTP目录</param>
        /// <returns>目录列表</returns>
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
        /// 获得文件和目录列表
        /// </summary>
        /// <param name="datastring">FTP返回的列表字符信息</param>
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
        /// 从Unix格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
        private FileStruct ParseFileStructFromUnixStyleRecord(string Record)
        {
            FileStruct f = new FileStruct();
            string processstr = Record.Trim();
            f.Flags = processstr.Substring(0, 10);
            f.IsDirectory = (f.Flags[0] == 'd');
            processstr = (processstr.Substring(11)).Trim();
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
            f.Owner = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            f.Group = _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);
            _cutSubstringFromStringWithTrim(ref processstr, ' ', 0);   //跳过一部分
            string yearOrTime = processstr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            if (yearOrTime.IndexOf(":") >= 0)  //time
            {
                processstr = processstr.Replace(yearOrTime, DateTime.Now.Year.ToString());
            }
            f.CreateTime = DateTime.Parse(_cutSubstringFromStringWithTrim(ref processstr, ' ', 8));
            f.Name = processstr;   //最后就是名称
            return f;
        }

        /// <summary>
        /// 从Windows格式中返回文件信息
        /// </summary>
        /// <param name="Record">文件信息</param>
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
        /// 按照一定的规则进行字符串截取
        /// </summary>
        /// <param name="s">截取的字符串</param>
        /// <param name="c">查找的字符</param>
        /// <param name="startIndex">查找的位置</param>
        private string _cutSubstringFromStringWithTrim(ref string s, char c, int startIndex)
        {
            int pos1 = s.IndexOf(c, startIndex);
            string retString = s.Substring(0, pos1);
            s = (s.Substring(pos1)).Trim();
            return retString;
        }
        /// <summary>
        /// 判断文件列表的方式Window方式还是Unix方式
        /// </summary>
        /// <param name="recordList">文件信息列表</param>
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
        /// 从FTP下载整个文件夹 
        /// </summary> 
        /// <param name="ftpDir">FTP文件夹路径</param> 
        /// <param name="saveDir">保存的本地文件夹路径</param> 
        public void DownFtpDir(string ftpDir, string saveDir)
        {
            List<FileStruct> files = ListFilesAndDirectories(ftpDir);
            if (!Directory.Exists(saveDir))
            {
                Directory.CreateDirectory(saveDir);
            }
            foreach (FileStruct f in files)
            {
                if (f.IsDirectory) //文件夹，递归查询
                {
                    DownFtpDir(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                }
                else //文件，直接下载
                {
                    DownLoadFile(ftpDir + "/" + f.Name, saveDir + "\\" + f.Name);
                }
            }
        }


        #endregion
    }
    #region 文件信息结构
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
        public string FileName; //INI文件名
        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        //类的构造函数，传递INI文件名
        public IniFiles(string AFileName)
        {
            // 判断文件是否存在
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:搞清枚举的用法
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
                //文件不存在，建立文件
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#表格配置档案");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini文件不存在"));
                }
            }
            //必须是完全路径，不能是相对路径
            FileName = fileInfo.FullName;
        }
        public IniFiles()
        {
            string AFileName ="sz.ini";
            // 判断文件是否存在
            FileInfo fileInfo = new FileInfo(AFileName);
            //Todo:搞清枚举的用法
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
                //文件不存在，建立文件
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#表格配置档案");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini文件不存在"));
                }
            }
            //必须是完全路径，不能是相对路径
            FileName = fileInfo.FullName;
        }
        //写INI文件
        public void WriteString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, FileName))
            {
                throw (new ApplicationException("写Ini文件出错"));
            }
        }
        //读取INI文件指定
        public string ReadString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Replace("\0", "").Trim();
        }

        //读整数
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
        //写整数
        public void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        //读布尔
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
        //写Bool
        public void WriteBool(string Section, string Ident, bool Value)
        {
            WriteString(Section, Ident, Convert.ToString(Value));
        }

        //从Ini文件中，将指定的Section名称中的所有Ident添加到列表中
        public void ReadSection(string Section, StringCollection Idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear();

            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0),
             FileName);
            //对Section进行解析
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
        //从Ini文件中，读取所有的Sections的名称
        public void ReadSections(StringCollection SectionList)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
             Buffer.GetUpperBound(0), FileName);
            GetStringsFromBuffer(Buffer, bufLen, SectionList);
        }
        //读取指定的Section的所有Value到列表中
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
        ////读取指定的Section的所有Value到列表中，
        //public void ReadSectionValues(string Section, NameValueCollection Values,char splitString)
        //{　 string sectionValue;
        //　　string[] sectionValueSplit;
        //　　StringCollection KeyList = new StringCollection();
        //　　ReadSection(Section, KeyList);
        //　　Values.Clear();
        //　　foreach (string key in KeyList)
        //　　{
        //　　　　sectionValue=ReadString(Section, key, "");
        //　　　　sectionValueSplit=sectionValue.Split(splitString);
        //　　　　Values.Add(key, sectionValueSplit[0].ToString(),sectionValueSplit[1].ToString());

        //　　}
        //}
        //清除某个Section
        public void EraseSection(string Section)
        {
            //
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {

                throw (new ApplicationException("无法清除Ini文件中的Section"));
            }
        }
        //删除某个Section下的键
        public void DeleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }
        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //检查某个Section下的某个键值是否存在
        public bool ValueExists(string Section, string Ident)
        {
            //
            StringCollection Idents = new StringCollection();
            ReadSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }

        //确保资源的释放
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
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\PATHHISZGQJK" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
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
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\" + logName + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
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
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\PATHHISZGQJK" + DateTime.Now.ToShortDateString() + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
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
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\PATHHISZGQJK" + DateTime.Now.ToShortDateString() + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
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
                log.WriteMyLog("打印控件调用错误！");
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
        /// <param name="dy">1:带预览 2:直接打印 3:设计 10:生成jpg 13:pdf</param>
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
                log.WriteMyLog("打印控件调用错误！");
                return 0;
            }
            report2 xx = (report2)dllload.InvokeMethod("report2", typeof(report2));

            //1:带预览 2:直接打印 3:设计 10:生成jpg 13:pdf
            long longxx = xx(dy, ConnectionString, sqlstring, image, imagename, yymc, bgname, jpgname);
            dllload.freeLoadDll();
            return longxx;
        }
        public long printjpg(string sqlstring, IntPtr handle, string image, string imagename, string bgname, string jpgname, string yymc)
        {
            if ((int)dllload.initPath("reportdll2.dll") == 0)
            {
                log.WriteMyLog("打印控件调用错误！");
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
                log.WriteMyLog("打印控件调用错误！");
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

