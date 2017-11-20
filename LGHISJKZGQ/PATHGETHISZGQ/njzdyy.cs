using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 南京中大医院
    /// </summary>
    class njzdyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {
                string exp = "";
                if (Sslbx == "申请单号" || Sslbx == "条码号")
                {
                    DataTable dt = new DataTable();
                    string odbcsql= f.ReadString(Sslbx, "odbcsql", "DSN=pathnet-sqxh;UID=bljk;PWD=bljk");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string dbtype = f.ReadString(Sslbx, "dbtype", "ODBC");

                    string djr = f.ReadString("yh", "yhmc", "").Trim();

                    if (Sslbx == "条码号")
                    {
                        DataTable dt_SQD = new DataTable();
                        string sqlstr_dt_SQD = "select *  from zdhis50.view_bl_blsqd  WHERE  条码号= '" + Ssbz.Trim() + "'";
                       // string sqlstr_dt_SQD = "select *  from view_bl_blsqd  WHERE  条码号= '" + Ssbz.Trim() + "'";
                        if (dbtype=="ORACLE")
                        dt_SQD = getXX_Oracle(odbcsql, sqlstr_dt_SQD);
                        else
                        dt_SQD = getXX_ODBC(odbcsql, sqlstr_dt_SQD);


                        if (dt_SQD.Rows.Count <= 0)
                        {
                            MessageBox.Show("未查询到此条码号记录！");
                            return "0";
                        }

                        Ssbz = dt_SQD.Rows[0]["申请序号"].ToString().Trim();
                    }

                    string sqlstr = "select *  from zdhis50.view_bl_blsqd  WHERE  申请序号= '" + Ssbz.Trim() + "'";
                   // string sqlstr = "select *  from view_bl_blsqd  WHERE  申请序号= '" + Ssbz.Trim() + "'";

                    if (dbtype == "ORACLE")
                        dt= getXX_Oracle(odbcsql, sqlstr);
                    else
                        dt = getXX_ODBC(odbcsql, sqlstr);

                      if (dt.Rows.Count <= 0)
                      {
                          MessageBox.Show("未查询到此申请序号记录！");
                          return "0";
                      }
                                        

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["病人编号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt.Rows[0]["申请序号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["门诊号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt.Rows[0]["住院号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt.Rows[0]["姓名"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + dt.Rows[0]["性别"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + dt.Rows[0]["年龄"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + dt.Rows[0]["婚姻"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + dt.Rows[0]["地址"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt.Rows[0]["电话"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + dt.Rows[0]["病区"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt.Rows[0]["床号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["身份证号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + dt.Rows[0]["民族"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + dt.Rows[0]["职业"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["送检科室"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["送检医生"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        ///////////////////////////
                        string BBLB_XML = "";
                        BBLB_XML = "<BBLB>";
                        string bbmc="";
                        try
                        {
                            for (int x = 0; x < dt.Rows.Count; x++)
                            {


                                string[] bbmcs = dt.Rows[x]["标本名称和部位"].ToString().Trim().Split('|');
                                foreach (string bb in bbmcs)
                                {
                                    if (bb.Trim() != "" && bb.Trim() != ",")
                                    {
                                        if (bbmc.Trim() == "")
                                            bbmc = bb.Split(',')[0].Trim();
                                        else
                                            bbmc = bbmc + "、" + bb.Split(',')[0].Trim();
                                    }
                                }
                                if (tqbblb == "1")
                                {
                                    try
                                    {
                                        BBLB_XML = BBLB_XML + "<row ";
                                        BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + (x + 1).ToString() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt.Rows[x]["条码号"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt.Rows[x]["标本名称和部位"].ToString().Trim() + "(" + dt.Rows[x]["标本数量"].ToString().Trim() + ")" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt.Rows[x]["标本离体时间"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt.Rows[x]["标本固定时间"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "/>";
                                    }
                                    catch(Exception  eee)
                                    {
                                        MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                        tqbblb = "0";
                                    }

                                }

                            }
                        }
                        catch(Exception  e3)
                        {
                            MessageBox.Show("获取标本名称异常：" + e3.Message);
                            tqbblb = "0";
                        }
                        BBLB_XML = BBLB_XML + "</BBLB>";


                      
                        xml = xml + "标本名称=" + (char)34 + bbmc + (char)34 + " ";
                        //----------------------------------------------------------
                        string sjdw = dt.Rows[0]["送检单位"].ToString().Trim();
                        if (sjdw.Trim() == "")
                            sjdw = "本院";
                        xml = xml + "送检医院=" + (char)34 + sjdw + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["医嘱项目名称"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + dt.Rows[0]["费别"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病人类别=" + (char)34 + dt.Rows[0]["病人类别"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            string lcbs = "";

                            if (dt.Rows[0]["手术名称"].ToString().Trim()!="")
                               lcbs="手术名称：" + dt.Rows[0]["手术名称"].ToString().Trim()  + "\r\n";

                           if (dt.Rows[0]["临床病史"].ToString().Trim() != "")
                           {
                               if (lcbs != "")
                                   lcbs = lcbs + "     ||";
                               lcbs = lcbs + "临床病史：" + dt.Rows[0]["临床病史"].ToString().Trim() + "\r\n";
                           }
                          
                            xml = xml + "<临床病史><![CDATA[" + ""+lcbs + "]]></临床病史>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }

                        if (tqbblb == "1")
                            xml = xml + BBLB_XML;
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("提取信息出错，请重新操作");
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                }

               
            } return "0";


        }
        public static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + (char)34 + " ";
            xml = xml + "门诊号=" + (char)34 + (char)34 + " ";
            xml = xml + "住院号=" + (char)34 + (char)34 + " ";
            xml = xml + "姓名=" + (char)34 + (char)34 + " ";

            xml = xml + "性别=" + (char)34 + (char)34 + " ";

            xml = xml + "年龄=" + (char)34 + (char)34 + " ";
            xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + (char)34 + "   ";
            xml = xml + "电话=" + (char)34 + (char)34 + " ";
            xml = xml + "病区=" + (char)34 + (char)34 + " ";
            xml = xml + "床号=" + (char)34 + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + (char)34 + " ";
            //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
            //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 +"本院"+ (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + (char)34 + " ";

            xml = xml + "病人类别=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + "]]></临床诊断>";
            xml = xml + "</LOGENE>";
            return xml;
        }

         

        public static DataTable getXX_ODBC(string con_str, string sql)
        {

            DataSet ds = new DataSet();
            OdbcConnection ocn = new OdbcConnection(con_str);
            try
            {
                OdbcDataAdapter dap = new OdbcDataAdapter(sql, ocn);
                ocn.Open();
                dap.Fill(ds);
                ocn.Close();
            }
            catch (Exception eee)
            {
                ocn.Close();
                log.WriteMyLog("数据库执行异常，" + eee.ToString());

                MessageBox.Show("数据库执行异常：" + eee.ToString());
                return ds.Tables[0];
            }
            finally
            {
                if (ocn.State == ConnectionState.Open)
                    ocn.Close();
            }
            return ds.Tables[0];


        }

        public static DataTable getXX_Oracle(string con_str, string sql)
        {

            DataSet ds = new DataSet();
            OracleConnection ocn = new OracleConnection(con_str);
            try
            {
                OracleDataAdapter dap = new OracleDataAdapter(sql, ocn);
                ocn.Open();
                dap.Fill(ds);
                ocn.Close();
            }
            catch (Exception eee)
            {
                ocn.Close();
                log.WriteMyLog("数据库执行异常，" + eee.ToString());

                MessageBox.Show("数据库执行异常：" + eee.ToString());
                return ds.Tables[0];
            }
            finally
            {
                if (ocn.State == ConnectionState.Open)
                    ocn.Close();
            }
            return ds.Tables[0];


        }
    }
}
