using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class qhdxfsyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {
                string err = "";
                string tqbblb = ZGQClass.getSZ_String(Sslbx, "tqbblb", "1");
                string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                string odbcsql = ZGQClass.getSZ_String(Sslbx, "odbcsql", "Data Source=ORCL_100;User ID=jk_ycxd;Password=123");
                string Sslbx1 = Sslbx;
                if (Sslbx == "申请单号" || Sslbx == "标本条码号")
                {
                    OracleDB_ZGQ oracledb = new OracleDB_ZGQ();
                    

                    string sqdh = "";

                    if (Sslbx == "标本条码号")
                    {
                        //通过标本条码号获取申请单号
                        DataTable dt_bblb = new DataTable();
                        dt_bblb = oracledb.Oracle_DataAdapter(odbcsql, "select *  from portal_his.emr_jcsq_blmx  where tmh='" + Ssbz + "'", ref err);
                        if (dt_bblb == null)
                        {
                            MessageBox.Show("查询此标本条码号的记录异常：" + err.Trim());
                            log.WriteMyLog("查询此标本条码号的记录异常：" + err.Trim() + "\r\n" + "select *  from portal_his.emr_jcsq_blmx  where tmh='" + Ssbz + "'");
                            return "0";
                        }

                        if (dt_bblb.Rows.Count > 0)
                        {
                            sqdh = dt_bblb.Rows[0]["sqdh"].ToString().Trim();
                        }
                        else
                        {
                            MessageBox.Show("未查询到此标本条码号的记录"+ err.Trim());
                            log.WriteMyLog("未查询到此标本条码号的记录" + err.Trim() + "\r\n" + "select *  from portal_his.emr_jcsq_blmx  where tmh='" + Ssbz + "'");
                            return "0";
                        }

                    }
                    if (Sslbx == "申请单号")
                        sqdh = Ssbz;
                    err = "";
                    DataTable dt_sqd = new DataTable();

                    string sql_sqd = "select emr_jcsq.*,ksmc from portal_his.emr_jcsq,portal_his.gy_ksdm where  portal_his.emr_jcsq.kdks = portal_his.gy_ksdm.ksdm  and  sqdh='" + sqdh + "'";
                    dt_sqd = oracledb.Oracle_DataAdapter(odbcsql, sql_sqd, ref err);

                    if (dt_sqd == null)
                    {
                        MessageBox.Show("查询申请单的记录异常：" + err.Trim()+"");
                        log.WriteMyLog("查询申请单的记录异常：" + err.Trim()+"\r\n"+sql_sqd);
                        return "0";
                    }

                    if (dt_sqd.Rows.Count <= 0)
                    {
                        MessageBox.Show("未查询到申请单的记录"+err.Trim());
                        log.WriteMyLog("未查询到申请单的记录"+err.Trim()+"\r\n"+sql_sqd);
                        return "0";
                    }


                    string exp = "";
                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[0]["BRID"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        string brlb = dt_sqd.Rows[0]["jzlx"].ToString().Trim();
                        if (brlb == "1") brlb = "门诊";
                        if (brlb == "2") brlb = "住院";


                        try
                        {
                            if (brlb != "住院")
                                xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[0]["patientid"].ToString().Trim() + (char)34 + " ";
                            else
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
                            if (brlb == "住院")
                                xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[0]["patientid"].ToString().Trim() + (char)34 + " ";
                            else
                                xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[0]["brxm"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------1男2女
                        try
                        {
                            string xb = dt_sqd.Rows[0]["brxb"].ToString().Trim();

                            if (xb == "1") xb = "男";
                            else if (xb == "2") xb = "女";
                            else xb = "";
                            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[0]["brnl"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "电话=" + (char)34 + dt_sqd.Rows[0]["lxdh"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "床号=" + (char)34 + dt_sqd.Rows[0]["brch"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[0]["KSMC"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[0]["sjys"].ToString().Trim() + (char)34 + " ";
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
                        string bbmc = "";
                        if (tqbblb == "1")
                        {
                            //通过申请单号获取标本明细
                            err = "";
                            DataTable dt_bblb = new DataTable();
                            dt_bblb = oracledb.Oracle_DataAdapter(odbcsql, "select  *  from portal_his.emr_jcsq_blmx where sqdh='" + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + "'", ref err);
                            if (dt_bblb == null)
                            {
                                MessageBox.Show("查询此申请单标本明细的记录异常：" + err.Trim());
                                log.WriteMyLog("查询此申请单标本明细的记录异常：" + err.Trim() + "\r\n" + "select  *  from portal_his.emr_jcsq_blmx where sqdh='" + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + "'");
                                tqbblb = "0";
                            }
                            else
                            {
                                if (dt_bblb.Rows.Count > 0)
                                {
                                    BBLB_XML = "<BBLB>";
                                    try
                                    {
                                        for (int x = 0; x < dt_bblb.Rows.Count; x++)
                                        {
                                            try
                                            {
                                                BBLB_XML = BBLB_XML + "<row ";
                                                BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_bblb.Rows[x]["plxh"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bblb.Rows[x]["tmh"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bblb.Rows[x]["bbmc"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bblb.Rows[x]["bbbw"].ToString().Trim() + (char)34 + " "; 
                                                BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bblb.Rows[x]["bbsl"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + yhmc + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "/>";
                                                bbmc = bbmc + " " + dt_bblb.Rows[x]["bbmc"].ToString().Trim();
                                            }
                                            catch (Exception eee)
                                            {
                                                MessageBox.Show("解析标本列表信息异常：" + eee.Message);
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
                                else
                                {
                                    MessageBox.Show("此申请单无标本条码信息"+err.Trim());

                                    log.WriteMyLog("此申请单无标本条码信息" + err.Trim() + "\r\n" + "select  *  from portal_his.emr_jcsq_blmx where sqdh='" + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + "'");
                             
                                    tqbblb = "0";
                                }
                            }
                        }


                        xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "送检医院=" + (char)34 + "本院"+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[0]["sqxm"].ToString().Trim() + (char)34 + " ";
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

                            xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------

                        string bszy = "病史摘要："+dt_sqd.Rows[0]["bszy"].ToString().Trim()+"\r\n";
                        string zyjcjg = "主要检查结果：" + dt_sqd.Rows[0]["zyjcjg"].ToString().Trim()+"\r\n";
                        string ssjl = "手术记录及内镜所见"+dt_sqd.Rows[0]["ssjl"].ToString().Trim()+"\r\n";


                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + bszy + zyjcjg + ssjl + "]]></临床病史>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[0]["jbmc"].ToString().Trim() + "]]></临床诊断>";
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
                        {
                            log.WriteMyLog(exp.Trim());
                            log.WriteMyLog(xml);
                        }

                        return xml;
                    }
                    catch(Exception  e2)
                    {
                        MessageBox.Show("程序异常：" + e2.Message);
                        return "0";
                    }


                }
                else
                {
                    MessageBox.Show("无此识别号：" + Sslbx1);
                    return "0";
                }


            }
            else
            {
                MessageBox.Show("识别号不能为空：" + Sslbx);
                return "0";
            }


        }
    }
}