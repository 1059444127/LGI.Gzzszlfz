using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    class dgdhyy
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            if (Sslbx != "")
            {
                string exp = "";
                string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                if (Sslbx == "手术申请单" || Sslbx == "冰冻申请单号")
                {

                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");

                    string sqlstr = "select *  from dbo.view_bdqpjcsqd   WHERE  registerid_int= '" + Ssbz.Trim() + "'";

                    DataTable dt_SQD = new DataTable();
                    string exp_db = "";
                    dt_SQD = db.Sql_DataAdapter(odbcsql, sqlstr, ref exp_db);


                    if (dt_SQD == null)
                    {
                        MessageBox.Show("数据库连接异常！" + exp_db);
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("未查询到此申请序号记录！");
                        return "0";
                    }

                    //取收费je
                    if (debug == "1")
                        MessageBox.Show(exp_db);

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_SQD.Rows[0]["patientid_vchr"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + dt_SQD.Rows[0]["iptimes_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt_SQD.Rows[0]["registerid_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt_SQD.Rows[0]["PatientIpNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_SQD.Rows[0]["PatientName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + dt_SQD.Rows[0]["PatientSex"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + dt_SQD.Rows[0]["PatientAge"].ToString().Trim()  + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt_SQD.Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt_SQD.Rows[0]["PatientBedNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "职业=" + (char)34 + dt_SQD.Rows[0]["PatientJob"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_SQD.Rows[0]["PatientDept"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_SQD.Rows[0]["SJYS"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------


                        xml = xml + "收费=" + (char)34 + "501" + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "标本名称=" + (char)34 + dt_SQD.Rows[0]["BBMC"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
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
                            xml = xml + "<临床病史><![CDATA[" + dt_SQD.Rows[0]["BLZY"].ToString().Trim() + "]]></临床病史>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_SQD.Rows[0]["LCZD"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }

                      
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
                if (Sslbx == "申请单号" || Sslbx == "常规病理申请单号")
                {
                   
                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");

                    string sqlstr = "select *  from dbo.view_bljcsqd   WHERE  registerid_int= '" + Ssbz.Trim() + "'";
                   
                    DataTable dt_SQD = new DataTable();
                    string exp_db = "";
                    dt_SQD = db.Sql_DataAdapter(odbcsql, sqlstr, ref exp_db);
                

                    if (dt_SQD == null)
                    {
                        MessageBox.Show("数据库连接异常！"+exp_db);
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("未查询到此申请序号记录！");
                        return "0";
                    }

                    //取收费je
                    if(debug=="1")
                        MessageBox.Show(exp_db); 

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_SQD.Rows[0]["patientid_vchr"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + dt_SQD.Rows[0]["iptimes_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt_SQD.Rows[0]["registerid_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "门诊号=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt_SQD.Rows[0]["PatientIpNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_SQD.Rows[0]["PatientName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + dt_SQD.Rows[0]["PatientSex"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + dt_SQD.Rows[0]["PatientAge"].ToString().Trim()  + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt_SQD.Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt_SQD.Rows[0]["PatientBedNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "职业=" + (char)34 + dt_SQD.Rows[0]["PatientJob"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_SQD.Rows[0]["PatientDept"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_SQD.Rows[0]["SJYS"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                    
                        xml = xml + "收费=" + (char)34 + "201" + (char)34 + " ";
                        //----------------------------------------------------------
                        ///////////////////////////
                        string BBLB_XML = "";
                        if (tqbblb == "1")
                        {
                           
                                        BBLB_XML = "<BBLB>";
                                        try
                                        {
                                              if(dt_SQD.Rows[0]["BBMC1"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW1"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"1" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC1"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW1"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }

                                            if(dt_SQD.Rows[0]["BBMC2"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW2"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"2" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC2"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW2"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }

                                             if(dt_SQD.Rows[0]["BBMC3"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW3"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"3" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC3"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW3"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC4"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW4"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"4" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC4"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW4"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC5"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW5"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"5" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC5"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW5"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC6"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW6"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"6" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC6"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW6"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC7"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW7"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"7"+ (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC7"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW7"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC8"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW8"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"8" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC8"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW8"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                                }
                                                catch (Exception eee)
                                                {
                                                    MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                                    tqbblb = "0";
                                                   
                                                }
                                            
                                        
                                     
                                        BBLB_XML = BBLB_XML + "</BBLB>";
                          
                        }
                        xml = xml + "标本名称=" + (char)34 + dt_SQD.Rows[0]["BBMC"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + ""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
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
                            xml = xml + "<临床病史><![CDATA[" + dt_SQD.Rows[0]["SSSJ"].ToString().Trim() + "]]></临床病史>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_SQD.Rows[0]["LCZD"].ToString().Trim() + "]]></临床诊断>";
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
            return "0";
        }
      
    }
}
