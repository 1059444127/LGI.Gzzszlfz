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
using System.Data.SqlClient;
using LGHISJKZGQ;
using System.Net;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    class CQYCYY
    {
        //重庆医科大学附属永川医院
        //oracle 表
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {
                
                string con_str = f.ReadString(Sslbx, "odbcsql", " Data Source=.;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p;");
                //string sql_str = f.ReadString(Sslbx, "hissql", @"select  F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_CH as 床号,F_SJKS as 送检科室,F_LCZD as 临床诊断,F_SQXH as 申请序号,F_BBMC as 标本名称,F_SDRQ as 申请时间 from  T_JCXX ");

                string aa = "";
                DataTable dt;
              
                if (Sslbx == "住院号")
                {
                    aa = @"select 医嘱ID as 病人编号,病人类别, '' as 费别,住院号,门诊号,姓名,性别,年龄,'' as 婚姻, '' as 地址,电话,'' AS 病区,病床号 as 床号,'' as 身份证号,'汉族' as 民族,'' as 职业,科室名称 as 送检科室,申请医生 as 送检医生,临床诊断,临床表现 as 临床病史,'0' as 收费,''  as 就诊ID, 申请ID as 申请序号,部位 as 标本名称,'本院' AS 送检医院,检查项目 as 医嘱项目, 申请时间  from  jk_bl where  住院号='" + Ssbz + "'   order by 申请时间 desc ";
                
               
                }
                if (Sslbx == "门诊号")
                {
                    aa = @"select 医嘱ID as 病人编号,病人类别, '' as 费别,住院号,门诊号,姓名,性别,年龄,'' as 婚姻, '' as 地址,电话,'' AS 病区,病床号 as 床号,'' as 身份证号,'汉族' as 民族,'' as 职业,科室名称 as 送检科室,申请医生 as 送检医生,临床诊断,临床表现 as 临床病史,'0' as 收费,''  as 就诊ID, 申请ID as 申请序号,部位 as 标本名称,'本院' AS 送检医院,检查项目 as 医嘱项目 ,申请时间 from  jk_bl where  门诊号='" + Ssbz + "'   order by 申请时间 desc ";
                
                }
                if (Sslbx == "体检号")
                {
                    aa = @"select 医嘱ID as 病人编号,病人类别, '' as 费别,住院号,门诊号,姓名,性别,年龄,'' as 婚姻, '' as 地址,电话,'' AS 病区,病床号 as 床号,'' as 身份证号,'汉族' as 民族,'' as 职业,科室名称 as 送检科室,申请医生 as 送检医生,临床诊断,临床表现 as 临床病史,'0' as 收费,''  as 就诊ID, 申请ID　as 申请序号,部位 as 标本名称,'本院' AS 送检医院,检查项目 as 医嘱项目,申请时间  from  jk_bl where  体检号='" + Ssbz + "'  and  病人类别='体检' order by 申请时间 desc ";
                
                }
                if (Sslbx == "医嘱号")
                {
                    aa = @"select 医嘱ID as 病人编号,病人类别, '' as 费别,住院号,门诊号,姓名,性别,年龄,'' as 婚姻, '' as 地址,电话,'' AS 病区,病床号 as 床号,'' as 身份证号,'汉族' as 民族,'' as 职业,科室名称 as 送检科室,申请医生 as 送检医生,临床诊断,临床表现 as 临床病史,'0' as 收费,''  as 就诊ID, 申请ID as 申请序号,部位 as 标本名称,'本院' AS 送检医院,检查项目as 医嘱项目,申请时间  from  jk_bl where  申请ID='" + Ssbz + "'   order by 申请时间 desc ";
                
                }

                if (aa == "")
                {
                    MessageBox.Show("查询语句有问题");
                    return "0";
                }
               
                try
                {
                  
                    dt = quxinxi(con_str, aa);

                  if (dt.Rows.Count < 1)
                  {
                      MessageBox.Show("无此病人信息，" + Sslbx + ":" + Ssbz,"提示");
                      return "0";
                  }
                 
                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["病人编号"].ToString() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "申请序号=" + (char)34 + dt.Rows[0]["申请序号"].ToString() + (char)34 + " ";
                    xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["门诊号"].ToString() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt.Rows[0]["住院号"].ToString() + (char)34 + " ";
                    xml = xml + "姓名=" + (char)34 + dt.Rows[0]["姓名"].ToString() + (char)34 + " ";
                    xml = xml + "性别=" + (char)34 + dt.Rows[0]["性别"].ToString() + (char)34 + " ";

                    xml = xml + "年龄=" + (char)34 + dt.Rows[0]["年龄"].ToString() + (char)34 + " ";

                    xml = xml + "婚姻=" + (char)34 + dt.Rows[0]["婚姻"].ToString() + (char)34 + " ";
                    xml = xml + "地址=" + (char)34 + dt.Rows[0]["地址"].ToString() + (char)34 + "   ";
                    xml = xml + "电话=" + (char)34 + dt.Rows[0]["电话"].ToString() + (char)34 + " ";
                    xml = xml + "病区=" + (char)34 + dt.Rows[0]["病区"].ToString() + (char)34 + " ";
                    xml = xml + "床号=" + (char)34 + dt.Rows[0]["床号"].ToString() + (char)34 + " ";
                    xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["身份证号"].ToString() + (char)34 + " ";
                    xml = xml + "民族=" + (char)34 + dt.Rows[0]["民族"].ToString() + (char)34 + " ";
                    xml = xml + "职业=" + (char)34 + dt.Rows[0]["职业"].ToString() + (char)34 + " ";
                    xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["送检科室"].ToString() + (char)34 + " ";
                    xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["送检医生"].ToString() + (char)34 + " ";

                    xml = xml + "收费=" + (char)34 + dt.Rows[0]["收费"].ToString() + (char)34 + " ";

                    xml = xml + "标本名称=" + (char)34 + dt.Rows[0]["标本名称"].ToString() + (char)34 + " ";
                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["医嘱项目"].ToString() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                    xml = xml + "费别=" + (char)34 + dt.Rows[0]["费别"].ToString() + (char)34 + " ";

                    xml = xml + "病人类别=" + (char)34 + dt.Rows[0]["病人类别"].ToString() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<临床病史><![CDATA[" + dt.Rows[0]["临床病史"].ToString() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["临床诊断"].ToString() + "]]></临床诊断>";
                    xml = xml + "</LOGENE>";

                    return xml;


                }
                catch (Exception e)
                {
                    if (Debug == "1")
                        MessageBox.Show("提取信息出错，请重新操作");
                    log.WriteMyLog("xml解析错误---" + e.ToString());
                    return "0";
                }
            }
            else
            {
                MessageBox.Show("无此" + Sslbx);
                if (Debug == "1")
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");

                return "0";
            }
            } 

        public static DataTable quxinxi(string con_str,string sql_str)
            {
                DataSet ds = new DataSet();
                OracleConnection orcon = new OracleConnection(con_str); ;
                try
                {


                    OracleDataAdapter orda = new OracleDataAdapter(sql_str, orcon);

                    orcon.Open();
                    orda.Fill(ds);
                    orcon.Close();
                    orda.Dispose();
                }

                catch (Exception e)
                {
                    MessageBox.Show("HIS数据库连接错误");
                    log.WriteMyLog("访问HIS数据库失败！" + e.ToString());
                }
                finally
                {
                    if (orcon.State == ConnectionState.Open)
                        orcon.Close();
                }

             if (ds.Tables[0].Rows.Count > 1)
                   {
                       Frm_cqycyy yc = new Frm_cqycyy(ds);
                       yc.ShowDialog();
                    
                       string   index = yc.A;

                       DataTable dtNew = new DataTable();  
                      DataView view = new DataView();
                            view.Table = ds.Tables[0];
                            view.RowFilter = "申请序号 = '"+index+"'";
                            dtNew = view.ToTable();
                           return dtNew;
                   }
                 
                 return  ds.Tables[0];
            

            }

    }
} 