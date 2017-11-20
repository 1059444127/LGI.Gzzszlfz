using System;
using System.Collections.Generic;
using System.Text;
using LGHISJKZGQ;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    //福建省省立医院
    //存储过程  sql
    class FJSSLYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {

                Debug = "0";
                string con_str = f.ReadString("住院号", "sqlcon", "Server=192.1.168.6;Database=THIS4;User Id=logene;Password=logene;");
                Debug = f.ReadString("住院号", "Debug", "0");
                string yydm = f.ReadString("住院号", "yydm", "");//01本院，02分院
                string mzbh = f.ReadString("门诊号", "zj330", "1");

               
                string PatType = "";
                string HospNo = "";
                string CardNo = "";
                string ApplyNo = "";
                string IDInhosp = "";
                string exp = "";
                if (Sslbx == "住院号")
                {
                    PatType = "1";
                    HospNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "门诊号")
                {
                    PatType = "0";
                    if (mzbh=="1")
                     HospNo = "330" + Ssbz.Trim();
                    else
                    HospNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "住院卡号")
                {
                    PatType = "1";
                    CardNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "门诊卡号")
                {
                    PatType = "0";
                    CardNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "住院ID号")
                {
                    PatType = "1";
                    IDInhosp = Ssbz.Trim();
                }
                else
                    if (Sslbx == "门诊ID号")
                    {
                        PatType = "0";
                        IDInhosp = Ssbz.Trim();
                    }
                    else
                    {
                        MessageBox.Show("无此识别号：" + Sslbx);
                        return "0";
                    }

                ///////////////////////////////////////////////////
                DataTable dt = new DataTable();

                SqlConnection con = new SqlConnection(con_str);
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "rsp_TechGetPatInfo_logene";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter dbParameter = cmd.CreateParameter();
                    dbParameter.DbType = DbType.String;
                    dbParameter.ParameterName = "@PatType";
                    dbParameter.Value = PatType;
                    dbParameter.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter);

                    SqlParameter dbParameter2 = cmd.CreateParameter();
                    dbParameter2.DbType = DbType.String;
                    dbParameter2.ParameterName = "@HospNo";
                    dbParameter2.Value = HospNo;
                    dbParameter2.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter2);

                    SqlParameter dbParameter3 = cmd.CreateParameter();
                    dbParameter3.DbType = DbType.String;
                    dbParameter3.ParameterName = "@CardNo";
                    dbParameter3.Value = CardNo;
                    dbParameter3.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter3);

                    SqlParameter dbParameter4 = cmd.CreateParameter();
                    dbParameter4.DbType = DbType.String;
                    dbParameter4.ParameterName = "@ApplyNo";
                    dbParameter4.Value = ApplyNo;
                    dbParameter4.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter4);

                    SqlParameter dbParameter5 = cmd.CreateParameter();
                    dbParameter5.DbType = DbType.String;
                    dbParameter5.ParameterName = "@IDInHosp";
                    dbParameter5.Value = IDInhosp;
                    dbParameter5.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter5);

                    if (yydm.Trim() != "")
                    {
                        SqlParameter dbParameter6 = cmd.CreateParameter();
                        dbParameter6.DbType = DbType.String;
                        dbParameter6.ParameterName = "@yydm";
                        dbParameter6.Value = yydm;
                        dbParameter6.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(dbParameter6);
                    }

                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    con.Open();
                    dap.Fill(dt);
                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show("HIS数据库连接错误," + e.ToString());
                    log.WriteMyLog("访问HIS数据库失败！" + e.ToString());

                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("未能查询到对应的数据记录！请确认" + Sslbx + "是否正确");
                    return "0";
                }

                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    try
                    {
                        xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["PATIENTid"].ToString().Trim() + (char)34 + " ";
                    }
                    catch(Exception  ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["CARDNO"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "门诊号=" + (char)34 +"" + (char)34 + " ";
                    }

                  
                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt.Rows[0]["HOSPNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                    

                    try
                    {
                        xml = xml + "姓名=" + (char)34 + dt.Rows[0]["PATNAME"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "姓名=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "性别=" + (char)34 + dt.Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "年龄=" + (char)34 + dt.Rows[0]["AGE"].ToString().Trim() + dt.Rows[0]["AGEUNIT"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "年龄=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "地址=" + (char)34 + dt.Rows[0]["ADDRess"].ToString().Trim() + (char)34 + "   ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "地址=" + (char)34 +"" + (char)34 + "   ";
                    }

                    try
                    {
                        xml = xml + "电话=" + (char)34 + dt.Rows[0]["phone"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "病区=" + (char)34 + dt.Rows[0]["WARD"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "病区=" + (char)34 + ""+ (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "床号=" + (char)34 + dt.Rows[0]["BEDNO"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "床号=" + (char)34 +""+ (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["IDNUM"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "身份证号=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "民族=" + (char)34 + dt.Rows[0]["NATION"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                    }
                     xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                  
                    try
                    {
                        xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["APPLYDEPT"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                    }
                 
                
                    try
                    {
                        xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["docname"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                    }

                    
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        if (yydm == "02")
                            xml = xml + "送检医院=" + (char)34 + "分院" + (char)34 + " ";
                        else
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                    if (yydm == "02")
                        xml = xml + "备用2=" + (char)34 + "分院|02" + (char)34 + " ";
                    else
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";



                    try
                    {
                        xml = xml + "费别=" + (char)34 + dt.Rows[0]["ChargeType"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "费别=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "病人类别=" + (char)34 +dt.Rows[0]["WardOrReg"].ToString().Trim()  + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                    }
                    xml = xml + "/>";
                    try
                    {
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                    }

                    try
                    {
                        xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["clincdescname"].ToString().Trim() + "]]></临床诊断>";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                    }
                    xml = xml + "</LOGENE>";

                    if (Debug=="1" && exp.Trim() != "")
                        log.WriteMyLog(exp.Trim());

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

        //public static DataTable getXX(string con_str, string Sslbx, string Ssbz)
        //{

        //    DataSet ds = new DataSet();
        //    SqlConnection con = new SqlConnection(con_str);
        //    try
        //    {


        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = "BL_GetPatientInfo";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter dbParameter = cmd.CreateParameter();
        //        dbParameter.DbType = DbType.String;
        //        dbParameter.ParameterName = "@code";
        //        dbParameter.Value = Ssbz;
        //        dbParameter.Direction = ParameterDirection.Input;
        //        cmd.Parameters.Add(dbParameter);

        //        SqlParameter dbParameter2 = cmd.CreateParameter();
        //        dbParameter2.DbType = DbType.String;
        //        dbParameter2.ParameterName = "@type";
        //        dbParameter2.Value = Sslbx;
        //        dbParameter2.Direction = ParameterDirection.Input;
        //        cmd.Parameters.Add(dbParameter2);

        //        SqlDataAdapter dap = new SqlDataAdapter(cmd);
        //        con.Open();
        //        dap.Fill(ds);
        //        con.Close();
        //        cmd.Dispose();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("HIS数据库连接错误," + e.ToString());
        //        log.WriteMyLog("访问HIS数据库失败！" + e.ToString());
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    //----------------------
        //    if (ds.Tables[0].Rows.Count > 1)
        //    {


        //        Frm_bjxwyy_select yc = new Frm_bjxwyy_select(ds);
        //        yc.ShowDialog();
        //        string sjys = yc.SJYS;
        //        string sqxh = yc.SQXH;
        //        string bbmc = yc.BBMC;
        //        string zycs = yc.ZYCS;
        //        string ryrq = yc.RYRQ;

        //        DataTable dtNew = new DataTable();
        //        DataView view = new DataView();
        //        view.Table = ds.Tables[0];
        //        view.RowFilter = "F_SJYS = '" + sjys + "' and F_SQXH='" + sqxh + "' and F_bbmc='" + bbmc + "'and F_ZYCS='" + zycs + "'  and F_ryrq='" + ryrq + "'";
        //        dtNew = view.ToTable();
        //        return dtNew;
        //    }

        //    return ds.Tables[0];


        //}
    }
}
