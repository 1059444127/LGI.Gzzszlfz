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
        /// 取sz和数据库中配置
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
        /// <returns>设定值</returns>
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
       /// 取sz和数据库中配置
       /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
       /// <param name="Default">默认值</param>
       /// <returns>设定值</returns>
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
                aa.ExecuteSQL("insert into  T_BGHJ(F_BLH,F_RQ,F_CZY,F_WZ,F_DZ,F_NR,F_EXEMC,F_CTMC) values('" + blh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "','"
                    + yhmc + "','机器名：" + Dns.GetHostName().Trim() + ",IP 地址：" + addr.ToString() +"',','"+DZ+"','" + NR + "','"+EXEMC+"','" +CTMC + "') ");
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
                                    TimeSpan tp = DateTime.Now - DateTime.ParseExact(CSRQ.Trim(), "yyyyMMdd", Thread.CurrentThread.CurrentCulture);
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
                return m_Age.ToString()+"岁";
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
                return age.ToString()+"岁";
            }
            if(lx==3)
            {
                string strAge = string.Empty; // 年龄的字符串表示
                int intYear = 0; // 岁
                int intMonth = 0; // 月
                int intDay = 0; // 天
 
                DateTime dtNow=DateTime.Today;
                DateTime dtBirthday=DateTime.Parse(csrq);
                // 计算天数
                intDay = dtNow.Day - dtBirthday.Day;
                 if (intDay < 0)
                 {
                 dtNow = dtNow.AddMonths(-1);
                 intDay += DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
                 }
 
                // 计算月数
                intMonth = dtNow.Month - dtBirthday.Month;
                 if (intMonth < 0)
                 {
                 intMonth += 12;
                 dtNow = dtNow.AddYears(-1);
                 }
 
                // 计算年数
                intYear = dtNow.Year - dtBirthday.Year;
 
                // 格式化年龄输出
                if (intYear >= 1) // 年份输出
                {
                strAge = intYear.ToString() + "岁";
                 }
 
                if (intMonth > 0 && intYear <= 3) // 3岁以下可以输出月数
                {
                strAge += intMonth.ToString() + "月";
                 }
 
                if (intDay >= 0 && intYear <= 1) // 一岁以下可以输出天数
                {
                if (strAge.Length == 0 || intDay > 0)
                 {
                 strAge += intDay.ToString() + "日";
                 }
                 }
 
                return strAge;
            }
            return "";
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
                    catch
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

        public static DataTable DT_SQD()
        {
            DataTable dt_sqd = new DataTable();
            //姓名
            DataColumn name = new DataColumn("F_XM");
            dt_sqd.Columns.Add(name);
            //性别
            DataColumn sexName = new DataColumn("F_XB");
            dt_sqd.Columns.Add(sexName);
            //年龄
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
            //住院号
            DataColumn InpatientNo = new DataColumn("F_ZYH");
            dt_sqd.Columns.Add(InpatientNo);
            //门诊号
            DataColumn F_MZH = new DataColumn("F_MZH");
            dt_sqd.Columns.Add(F_MZH);

            //登记号
            DataColumn PatNo = new DataColumn("F_BRBH");
            dt_sqd.Columns.Add(PatNo);
            //F_YZID
            DataColumn F_YZID = new DataColumn("F_YZID");
            dt_sqd.Columns.Add(F_YZID);
            //F_SQXH
            DataColumn F_SQXH = new DataColumn("F_SQXH");
            dt_sqd.Columns.Add(F_SQXH);
            //婚姻
            DataColumn Marry = new DataColumn("F_HY");
            dt_sqd.Columns.Add(Marry);
            //民族
            DataColumn Nation = new DataColumn("F_MZ");
            dt_sqd.Columns.Add(Nation);
            //病区
            DataColumn wardName = new DataColumn("F_BQ");
            dt_sqd.Columns.Add(wardName);
            //床号
            DataColumn bedName = new DataColumn("F_CH");
            dt_sqd.Columns.Add(bedName);
            //送检医生
            DataColumn employeeName = new DataColumn("F_SJYS");
            dt_sqd.Columns.Add(employeeName);
            //送检科室
            DataColumn detpname = new DataColumn("F_SJkS");
            dt_sqd.Columns.Add(detpname);
            //送检单位
            DataColumn Hospital = new DataColumn("F_SJDW");
            dt_sqd.Columns.Add(Hospital);
            //病人类型
            DataColumn brlb = new DataColumn("F_BRLB");
            dt_sqd.Columns.Add(brlb);

            //医嘱项目
            DataColumn requestType = new DataColumn("F_YZXM");
            dt_sqd.Columns.Add(requestType);

            //检查部位
            DataColumn orderName = new DataColumn("F_BBMC");
            dt_sqd.Columns.Add(orderName);

            //临床病史
            DataColumn clinicalHistory = new DataColumn("F_LCZL");
            dt_sqd.Columns.Add(clinicalHistory);

            //临床诊断
            DataColumn diagnosis = new DataColumn("F_LCZD");
            dt_sqd.Columns.Add(diagnosis);

            return dt_sqd;

        }
    }
}
