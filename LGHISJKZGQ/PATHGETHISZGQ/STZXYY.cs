using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using LGHISJKZGQ;
using System.Net;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    //汕头中心医院
    class STZXYY
    {
        //汕头中心医院
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
         
            if (Sslbx != "")
            {

                string iszzk = f.ReadString(Sslbx, "iszzk", "").Trim();
              //  string rtn = "";
                //----------------------------------------------------------------------------------------------------------------------
                //-----------住院号-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "住院号")
                {

                    ////////SELECT  * FROM OPENQUERY([HIS_ZY] ,'select * from hisdbstzx.dbo.view_bl_patient_info where ZYH = ''724016'' ORDER BY RYRQ DESC ')
                    string con_str = f.ReadString("住院号", "odbcsql_zgq", "Data Source=172.16.0.55;Initial Catalog=hqint;User Id=bl;Password=bl123");
                    string hiszysql = f.ReadString("住院号", "hissql_zgq", "SELECT  * FROM OPENQUERY([HIS_ZY] ,'select * from hisdbstzx.dbo.view_bl_patient_info where ZYH = ''f_hissbh'' ORDER BY RYRQ DESC')");

                    hiszysql = hiszysql.Replace("f_hissbh", Ssbz.Trim());

                    DataSet ds = new DataSet();
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter(hiszysql, sqlcon);
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("数据库连接失败," + ee.Message);
                    }
                        if (ds.Tables[0].Rows.Count <= 0)
                        {
                            MessageBox.Show("无此住院号信息！");
                            return "0";
                        }

                        if (ds.Tables[0].Rows.Count > 1)
                        {

                            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                            string blhstr = "";
                            int y = ds.Tables[0].Rows.Count;
                            for (int z = 0; z < ds.Tables[0].Rows.Count; z++)
                            {
                                DataTable bljc = new DataTable();
                                bljc = aa.GetDataTable("select F_blh,F_SQXH,F_BRBH,F_XM from T_jcxx where F_SQXH='" + ds.Tables[0].Rows[z]["order_sn"].ToString() + "' and F_BRBH='" + ds.Tables[0].Rows[z]["patient_id"].ToString() + "'", "blxx");
                               if (bljc.Rows.Count > 0)
                                {
                               
                                    ds.Tables[0].Rows.RemoveAt(z);
                                    z = z - 1;
                                    blhstr = blhstr + bljc.Rows[0]["F_blh"].ToString()+";";
                          
                                }
                            }
                            if (ds.Tables[0].Rows.Count <=0)
                            {
                                MessageBox.Show("此条申请已登记过，不能再登记。病理号：" + blhstr);
                                return "0";
                            }

                            stzxyy_select st_s = new stzxyy_select(ds, Sslbx);
                            st_s.ShowDialog();


                            string je = st_s.A;  //金额
                            string sj = st_s.B;   //时间
                            string bm = st_s.C;   //编码

                            if (je == "" || sj == "" || bm == "")
                            {
                                return "0";
                            }
                            DataTable dtNew = new DataTable();
                            DataView view = new DataView();
                            view.Table = ds.Tables[0];
                            view.RowFilter = "order_sn='" + bm.Trim() + "' and sfje='" + je + "' and  ryrq='" + sj + "'"; 
                               dtNew = view.ToTable();

                            if (dtNew.Rows.Count < 1)
                            {
                                MessageBox.Show("无此住院号信息！");
                                return "0";
                            }

                            try
                            {
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                xml = xml + "病人编号=" + (char)34 + dtNew.Rows[0]["patient_id"].ToString() + (char)34 + " ";
                                xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "申请序号=" + (char)34 + dtNew.Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "住院号=" + (char)34 + dtNew.Rows[0]["zyh"].ToString() + (char)34 + " ";
                                xml = xml + "姓名=" + (char)34 + dtNew.Rows[0]["xm"].ToString() + (char)34 + " ";
                                string xb = dtNew.Rows[0]["sex_name"].ToString().Trim();
                                if (xb == "2")
                                    xml = xml + "性别=" + (char)34 + "女" + (char)34 + " ";
                                else
                                    xml = xml + "性别=" + (char)34 + "男" + (char)34 + " ";

                                xml = xml + "年龄=" + (char)34 + dtNew.Rows[0]["nl"].ToString() + (char)34 + " ";

                                xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "地址=" + (char)34 + dtNew.Rows[0]["lxdz"].ToString() + (char)34 + "   ";
                                xml = xml + "电话=" + (char)34 + dtNew.Rows[0]["lxdh"].ToString() + (char)34 + " ";
                                xml = xml + "病区=" + (char)34 + dtNew.Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "床号=" + (char)34 + dtNew.Rows[0]["cwdm"].ToString() + (char)34 + " ";
                                xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                                xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "送检科室=" + (char)34 + dtNew.Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "送检医生=" + (char)34 + dtNew.Rows[0]["ysxm"].ToString() + (char)34 + " ";

                              

                                if (iszzk != "1")
                                {
                                    try
                                    {
                                        xml = xml + "收费=" + (char)34 + float.Parse(dtNew.Rows[0]["sfje"].ToString().Trim()).ToString() + (char)34 + " ";
                                    }
                                    catch
                                    {
                                        xml = xml + "收费=" + (char)34 + "0" + (char)34 + " ";
                                    }
                                }
                                xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                                xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
                                xml = xml + "医嘱项目=" + (char)34 + dtNew.Rows[0]["order_name"].ToString() + "^" + dtNew.Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<临床病史><![CDATA[" + "入院时间：" + dtNew.Rows[0]["ryrq"].ToString() + "]]></临床病史>";
                                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                xml = xml + "</LOGENE>";

                                return xml;

                            }
                            catch (Exception eee)
                            {
                                MessageBox.Show("XML生成异常，" + eee.Message);
                                return "0";
                            }
                        }
                        else
                        {
                            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                             DataTable bljc = new DataTable();
                                bljc = aa.GetDataTable("select F_blh,F_SQXH,F_BRBH,F_XM from T_jcxx where F_SQXH='" + ds.Tables[0].Rows[0]["order_sn"].ToString() + "' and F_BRBH='" + ds.Tables[0].Rows[0]["patient_id"].ToString() + "'", "blxx");
                                if (bljc.Rows.Count > 0)
                                {
                                    MessageBox.Show("此条申请已登记过，不能再登记。病理号：" + bljc.Rows[0]["F_blh"].ToString() + ",姓名：" + bljc.Rows[0]["F_XM"].ToString());
                                    return "0";
                                }
                         
                            try
                            {
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["patient_id"].ToString() + (char)34 + " ";
                                xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[0]["zyh"].ToString() + (char)34 + " ";
                                xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["xm"].ToString() + (char)34 + " ";
                                string xb = ds.Tables[0].Rows[0]["sex_name"].ToString().Trim();
                                if (xb == "2")
                                    xml = xml + "性别=" + (char)34 + "女" + (char)34 + " ";
                                else
                                    xml = xml + "性别=" + (char)34 + "男" + (char)34 + " ";

                                xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[0]["nl"].ToString() + (char)34 + " ";

                                xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[0]["lxdz"].ToString() + (char)34 + "   ";
                                xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["lxdh"].ToString() + (char)34 + " ";
                                xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["cwdm"].ToString() + (char)34 + " ";
                                xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                                xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["ysxm"].ToString() + (char)34 + " ";

                                if (iszzk != "1")
                                {
                                    try
                                    {
                                        xml = xml + "收费=" + (char)34 + float.Parse(ds.Tables[0].Rows[0]["sfje"].ToString().Trim()).ToString() + (char)34 + " ";
                                    }
                                    catch
                                    {
                                        xml = xml + "收费=" + (char)34 + "0" + (char)34 + " ";
                                    }
                                }
                                xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                                xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
                                xml = xml + "医嘱项目=" + (char)34 + ds.Tables[0].Rows[0]["order_name"].ToString() + "^" + ds.Tables[0].Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<临床病史><![CDATA[" + "入院时间：" + ds.Tables[0].Rows[0]["ryrq"].ToString() + "]]></临床病史>";
                                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                xml = xml + "</LOGENE>";

                                return xml;

                            }
                            catch (Exception eee)
                            {
                                MessageBox.Show("XML生成异常，" + eee.Message);
                                return "0";
                            }
                        }
                    }
              

　　　　　　　　//----------------------------------------------------------------------------------------------------------------------
                //-----------ＩＤ号-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "ID号")
                {
                       /////通过存储过程调用的 MzInterfacePath_getinfo_Seleted @pid,varchar(12) 

                    string con_str = f.ReadString("ID号", "odbcsql_zgq", "Data Source=172.16.0.30;Initial Catalog=hisdb_stzx;User Id=bl;Password=bl123");
                    DataSet ds = new DataSet();
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter("MzInterfacePath_getinfo_Seleted", sqlcon);
                        objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        objAdapter.SelectCommand.Parameters.Add("@p_id", SqlDbType.VarChar, 12);
                        objAdapter.SelectCommand.Parameters["@p_id"].Value = Ssbz.Trim();
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch(Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("数据库连接失败"+ee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                         MessageBox.Show("无此ID号信息！");
                        return "0";
                    }

                    if (ds.Tables[0].Rows.Count > 1)
                    {
                      //  LG_GETHIS_ZGQ.cszxyy cs = new LG_GETHIS_ZGQ.cszxyy(ds1, 1);

                        stzxyy_select st_s = new stzxyy_select(ds, Sslbx);
                        st_s.ShowDialog();
                    

                        string JYSH = st_s.A;
                        string GRQ = st_s.B;

                        if (JYSH == "" || GRQ == "")
                        {
                            return "0";
                        }
                        DataTable dtNew = new DataTable();
                        DataView view = new DataView();
                        view.Table = ds.Tables[0];
                        view.RowFilter = "GHRQ='" + GRQ.Trim() + "' and JYSH='" + JYSH + "'";
                        dtNew = view.ToTable();


                        if (dtNew.Rows.Count< 1)
                        {     MessageBox.Show("无此ID号信息！");
                         return "0";
                        } 
                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + dtNew.Rows[0]["JYSH"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + dtNew.Rows[0]["DNAME"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + dtNew.Rows[0]["SEX"].ToString() + (char)34 + " ";
                           // xml = xml + "年龄=" + (char)34 + dtNew.Rows[0]["NL"].ToString() + (char)34 + " ";
                               string nl = dtNew.Rows[0]["NL"].ToString();
                            if (nl.Contains("岁") || nl.Contains("天") || nl.Contains("月"))
                                xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                            else
                                xml = xml + "年龄=" + (char)34 + nl + "岁" + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + dtNew.Rows[0]["KSMC"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + dtNew.Rows[0]["YSMC"].ToString() + (char)34 + " ";


                            if (iszzk != "1")
                            {
                                try
                                {
                                    xml = xml + "收费=" + (char)34 + float.Parse(dtNew.Rows[0]["SFJE"].ToString().Trim()).ToString() + (char)34 + " ";
                                }
                                catch
                                {
                                    xml = xml + "收费=" + (char)34 + "0" + (char)34 + " ";
                                }
                            }


                            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + dtNew.Rows[0]["JYXM"].ToString() + "^" + dtNew.Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "收费时间：" + dtNew.Rows[0]["GHRQ"].ToString() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            return xml;

                        }
                        catch (Exception ee)
                        {
                          MessageBox.Show("xml生成异常，"+ee.Message);
                            return "0";
                        }
                    }
                    else
                    {

                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["DNAME"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[0]["SEX"].ToString() + (char)34 + " ";

                            string nl = ds.Tables[0].Rows[0]["NL"].ToString();
                            if (nl.Contains("岁") || nl.Contains("天") || nl.Contains("月"))
                                xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                            else
                                xml = xml + "年龄=" + (char)34 + nl + "岁" + (char)34 + " ";


                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["KSMC"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["YSMC"].ToString() + (char)34 + " ";

                            if (iszzk != "1")
                            {

                                try
                                {
                                    xml = xml + "收费=" + (char)34 + float.Parse(ds.Tables[0].Rows[0]["SFJE"].ToString().Trim()).ToString() + (char)34 + " ";
                                }
                                catch
                                {
                                    xml = xml + "收费=" + (char)34 + "0" + (char)34 + " ";
                                }
                            }

                            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + ds.Tables[0].Rows[0]["JYXM"].ToString() + "^"+ds.Tables[0].Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "收费时间：" + ds.Tables[0].Rows[0]["GHRQ"].ToString() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            return xml;

                        }
                        catch (Exception ee)
                        {
                          
                                MessageBox.Show("xml生成异常，"+ee.Message);
                            return "0";
                        }
                    }
                }

                //----------------------------------------------------------------------------------------------------------------------
                //-----------门诊列表-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "门诊列表")
                {
                    /////通过存储过程调用的 MzInterfacePath_getinfo,门诊列表
                    //////exec his.hisdbstzx.dbo.MzInterfacePath_getinfo   门诊列表
                    //////exec his.hisdbstzx.dbo.MzInterfacePath_getinfo_Select  @pid    ID号取信息（id号）
                    //////exec his.hisdbstzx.dbo.MzInterfacePath_getinfo_Update  @pid @jy_sn   回写标记(id号，项目编码)

                    string con_str = f.ReadString("门诊列表", "odbcsql_zgq", "Data Source=172.16.0.30;Initial Catalog=hisdb_stzx;User Id=bl;Password=bl123");
                    DataSet ds = new DataSet();

                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter("MzInterfacePath_getinfo", sqlcon);
                        objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("数据库连接失败,"+ee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count <= 0)
                    {    MessageBox.Show("无需要的信息！");
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        //int  x=0;
                        //if (ds.Tables[0].Columns.Contains("JYSH") && ds.Tables[0].Columns.Contains("JYXM"))
                        //{
                        //    x=1;
                        //}

                        stzxyy_select st_s = new stzxyy_select(ds, Sslbx);
                        st_s.ShowDialog();
                        string JYSH = st_s.A.Trim();
                        string GRQ = st_s.B;
                        string brbh = st_s.C;

                        if(JYSH=="" || GRQ==""||brbh=="")
                        {   return "0";
                        }
                        DataTable dtNew = new DataTable();
                        DataView view = new DataView();
                        view.Table = ds.Tables[0];
                       
                       
                        //if (x == 0)
                        //    view.RowFilter = "BRBH='" + JYSH + "' and GHRQ='" + GRQ + "'";
                        //else
                            view.RowFilter = "BRBH='" + brbh + "' and GHRQ='" + GRQ.Trim() + "' and JYSH='" + JYSH + "'"; 
                    
                        dtNew = view.ToTable();

                        if (dtNew.Rows.Count < 1)
                        {
                            MessageBox.Show("无此ID号信息！");
                            return "0";
                        }
                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + dtNew.Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + dtNew.Rows[0]["DNAME"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + dtNew.Rows[0]["SEX"].ToString() + (char)34 + " ";
                            string nl = dtNew.Rows[0]["NL"].ToString();
                            if (nl.Contains("岁") || nl.Contains("天") || nl.Contains("月"))
                                xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                            else
                                xml = xml + "年龄=" + (char)34 + nl + "岁" + (char)34 + " ";


                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + dtNew.Rows[0]["KSMC"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + dtNew.Rows[0]["YSMC"].ToString() + (char)34 + " ";


                            if (iszzk != "1")
                            {

                                try
                                {
                                    xml = xml + "收费=" + (char)34 + float.Parse(dtNew.Rows[0]["SFJE"].ToString().Trim()).ToString() + (char)34 + " ";
                                }
                                catch
                                {
                                    xml = xml + "收费=" + (char)34 + "0" + (char)34 + " ";
                                }
                            }

                            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
                           
                            xml = xml + "医嘱项目=" + (char)34 + dtNew.Rows[0]["JYXM"].ToString() + "^" + dtNew.Rows[0]["JYSH"].ToString() + (char)34 + " ";
                         
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "收费时间：" + dtNew.Rows[0]["GHRQ"].ToString() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            return xml;

                        }
                        catch (Exception ee)
                        {
                                MessageBox.Show("xml生成异常，"+ee.Message);
                            return "0";
                        }
                    }
                }


                //----------------------------------------------------------------------------------------------------------------------
                //-----------体检号-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "体检号")
                {
                    string con_str = f.ReadString("体检号", "odbcsql_zgq", "Data Source=172.16.0.40;Initial Catalog=tj_db_setup;User Id=bl;Password=bl123");
                    //exec pro_bl_get_tj_info  @as_in_no   体检
                    DataSet ds = new DataSet();
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter("pro_bl_get_tj_info", sqlcon); 
                        objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        objAdapter.SelectCommand.Parameters.Add("@as_in_no", SqlDbType.VarChar, 12);
                        objAdapter.SelectCommand.Parameters["@as_in_no"].Value = Ssbz.Trim();
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("数据库连接失败,"+ee.ToString());
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count <= 0)
                    {    MessageBox.Show("无此体检号信息！");
                        return rtn_xml();
                    }


                    try
                    {
                      

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["体检号"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + "" + (char)34 + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["体检ID"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["体检号"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["姓名"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds.Tables[0].Rows[0]["性别"].ToString() + (char)34 + " ";
                        string nl = ds.Tables[0].Rows[0]["年龄"].ToString();
                        if (nl.Contains("岁") || nl.Contains("天") || nl.Contains("月"))
                        xml = xml + "年龄=" + (char)34 +nl  + (char)34 + " ";
                        else
                        xml = xml + "年龄=" + (char)34 + nl+"岁" + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " "; 
                        string dz=ds.Tables[0].Rows[0]["地址"].ToString();
                        string dw=ds.Tables[0].Rows[0]["单位"].ToString();  
                        if (string.IsNullOrEmpty(dz))
                            dz = "";
                        if (string.IsNullOrEmpty(dw))
                            dw = "";
                     
                        xml = xml + "地址=" + (char)34 +dz+"     "+dw + (char)34 + "   ";
                        if(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["电话"].ToString()))
                         xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        else
                        xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["电话"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ""+ (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                     
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "体检时间：" + ds.Tables[0].Rows[0]["体检时间"].ToString() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "体检" + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                   
                        return xml;

                    }
                    catch (Exception ee)
                    {
                      
                            MessageBox.Show("xml生成异常，"+ee.ToString());
                        return "0";
                    }
                }


            
                MessageBox.Show("无此" + Sslbx);
                return "0";

            }
            MessageBox.Show("识别类型不能为空" );
            return "0";

        }
        public static string rtn_xml()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";

            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 + "汕头市中心医院" + (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + (char)34 + " ";
            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
            xml = xml + "</LOGENE>";

            return xml;
        }

    }
}

