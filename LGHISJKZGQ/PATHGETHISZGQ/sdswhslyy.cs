
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LGHISJKZGQ;
using System.Windows.Forms;
using dbbase;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    class sdswhslyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
          
            if (Sslbx != "")
            {
                string exp = "";

                if (Sslbx == "申请单号"||Sslbx == "标本条码号")
                {
                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Provider=MSDAORA;Data Source=lisdb;User id=blinfo;Password=blinfo;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");
          
                    string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();

                    OleDbDB_ZGQ db = new OleDbDB_ZGQ();
                    DataTable dt_bblb = new DataTable();

                    string exp_db = "";
                    string sqxh = Ssbz.Trim();
                    if (Sslbx == "标本条码号")
                    {
                        string sqlstr_bblb = "select *  from VIEW_病理手术标本   WHERE  送验标本条码= '" + Ssbz.Trim() + "'";

                        dt_bblb = db.OleDb_DataAdapter(odbcsql, sqlstr_bblb, ref exp_db);

                        if (dt_bblb.Rows.Count <= 0)
                        {
                            MessageBox.Show("未查询到此标本条码记录！\r\n" + exp_db);
                            return "0";
                        }


                        sqxh = dt_bblb.Rows[0]["申请单号"].ToString().Trim();
                    }


                    string sqlstr = "select *  from VIEW_病理申请单   WHERE  申请单号= " + sqxh.Trim() + "";
                    DataTable dt_SQD = new DataTable();
             
                    dt_SQD = db.OleDb_DataAdapter(odbcsql, sqlstr, ref exp_db);
                    if (exp_db.Trim() != "")
                    {
                        MessageBox.Show("连接HIS数据库异常：" + exp_db);
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
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
                            xml = xml + "病人编号=" + (char)34 + dt_SQD.Rows[0]["病人编号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt_SQD.Rows[0]["申请单号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                        }
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";

                            xml = xml + "住院号=" + (char)34 + dt_SQD.Rows[0]["住院号"].ToString().Trim() + (char)34 + " ";
    
                            xml = xml + "姓名=" + (char)34 + dt_SQD.Rows[0]["患者姓名"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + dt_SQD.Rows[0]["性别"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + dt_SQD.Rows[0]["年龄"].ToString().Trim()  + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + dt_SQD.Rows[0]["婚姻"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + dt_SQD.Rows[0]["地址"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt_SQD.Rows[0]["电话"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + dt_SQD.Rows[0]["病区"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
 
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt_SQD.Rows[0]["当前床号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                    
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt_SQD.Rows[0]["身份证号码"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                     
                        try
                        {
                            xml = xml + "民族=" + (char)34 + dt_SQD.Rows[0]["民族"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                      
                        try
                        {
                            xml = xml + "职业=" + (char)34 + dt_SQD.Rows[0]["职业"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                    
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_SQD.Rows[0]["申请科室"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                       
                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_SQD.Rows[0]["申请医生姓名"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                 
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                      
                        string BBLB_XML = "";
                        if (tqbblb == "1")
                        {
                            string sqlstr_bblb = "select *  from VIEW_病理手术标本   WHERE  申请单号= " + sqxh.Trim() + "";
                            dt_bblb = db.OleDb_DataAdapter(odbcsql, sqlstr_bblb, ref exp_db);
                            if (dt_bblb.Rows.Count <= 0)
                            {
                                MessageBox.Show("未查询到此标本条码记录！\r\n" + exp_db);
                            }
                            else
                            {
                                BBLB_XML = "<BBLB>";
                                try
                                {
                                    for (int x = 0; x < dt_bblb.Rows.Count; x++)
                                    {
                                        try
                                        {
                                            BBLB_XML = BBLB_XML + "<row ";
                                            BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + (x+1).ToString() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bblb.Rows[x]["送验标本条码"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bblb.Rows[x]["送验标本名称"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bblb.Rows[x]["标本采取部位"].ToString().Trim() + (char)34 + " ";

                                            BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bblb.Rows[x]["标本离体时间"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bblb.Rows[x]["固定标本时间"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                            string bz = dt_bblb.Rows[x]["病理接收标本标志"].ToString().Trim();
                                            if (bz == "Y") bz = "核收"; if (bz == "N") bz = "拒收"; if (bz == "") bz = "未核收";
                                            BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + bz + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "/>";
                                        }
                                        catch (Exception eee)
                                        {
                                            MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                            tqbblb = "0";
                                            break;
                                        }
                                    }
                                }
                                catch (Exception e3)
                                {
                                    MessageBox.Show("获取标本名称异常：" + e3.Message);
                                    tqbblb = "0";
                                }
                                BBLB_XML = BBLB_XML + "</BBLB>";
                            }

                        }
                        xml = xml + "标本名称=" + (char)34 + dt_SQD.Rows[0]["标本名称"].ToString().Trim() + (char)34 + " ";
                      
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_SQD.Rows[0]["申请单类型"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        try
                        {
                            xml = xml + "备用2=" + (char)34 + DateTime.Parse(dt_SQD.Rows[0]["出生日期"].ToString().Trim()).ToString("yyyy-MM-dd")+ (char)34 + " ";
                        }
                        catch(Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "备用2=" + (char)34 +""+ (char)34 + " ";
                        }
                      
                        try
                        {
                            xml = xml + "费别=" + (char)34 + dt_SQD.Rows[0]["费别"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }

                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";

                        xml = xml + "/>";
      
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + dt_SQD.Rows[0]["病史摘要"].ToString().Trim() + "]]></临床病史>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
       
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_SQD.Rows[0]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
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
                        log.WriteMyLog("xml解析错误---" + e.Message);
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    return "0";
                }
            }
            MessageBox.Show("识别类型不能为空");
            return "0";
        }
   
    }     
}
