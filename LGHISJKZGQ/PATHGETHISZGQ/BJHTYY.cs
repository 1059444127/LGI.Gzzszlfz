using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.Odbc;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 北京航天医院体检接口——视图
    /// </summary>
    class BJHTYY
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {


        
                if (Sslbx == "体检号")
                {
                   // string con_str = f.ReadString(Sslbx, "odbcsql", "Server=.;Database=pathnet;User Id=pathnet;Password=4s3c2a1p;");
                    string odbcsql = f.ReadString("体检号", "odbcsql", "DSN=pathnet-his;UID=INTERFACEPA;PWD=INTERFACEPA");
                    string sql = "select checkupcode,EXAMCODE,NAME,GENDER,floor(months_between(sysdate,BIRTHDAY)/12)||'岁' as age,ORDERNAME,APPLYDEPARTMENT,APPLYDOCTOR,PATIENTCODE,PATIENTSOURCE,INPATIENTCODE,OUTPATIENTCODE,marriage,ADDRESS,PHONE,ZONE,BEDNO,IDCARDCODE,RACE,CLINICALDIAG  from hthis.view_ht_interface_pat WHERE checkupcode ='" + Ssbz + "'";                    
                    DataTable dt = new DataTable();
                    if (Sslbx != "")
                        dt = getXX(odbcsql, sql, Ssbz);
                    else
                    {
                        MessageBox.Show("无此识别号：" + Sslbx);
                        return "0";
                    }
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("未能查询到对应的数据记录！请确认号码是否正确");
                        return "0";
                    }

                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["PATIENTCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["checkupcode"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt.Rows[0]["EXAMCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["OUTPATIENTCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt.Rows[0]["INPATIENTCODE"].ToString().Trim() + (char)34 + " ";
                      
                        xml = xml + "姓名=" + (char)34 + dt.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        string xb = dt.Rows[0]["GENDER"].ToString().Trim();
                        if (xb == "F" || xb == "f")
                            xb = "女";
                        if (xb == "M" || xb == "m")
                            xb = "男";
                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt.Rows[0]["age"].ToString().Trim() + (char)34 + " ";
                       
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + dt.Rows[0]["ADDRESS"].ToString().Trim() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt.Rows[0]["PHONE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt.Rows[0]["ZONE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt.Rows[0]["BEDNO"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["IDCARDCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt.Rows[0]["RACE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["APPLYDEPARTMENT"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["APPLYDOCTOR"].ToString().Trim() + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["ORDERNAME"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 +"" + (char)34 + " ";

                        xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA["  + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["CLINICALDIAG"].ToString().Trim() + "]]></临床诊断>";
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
            } return "0";
        }

        public static DataTable getXX(string con_str, string sql, string Ssbz)
        {

            DataSet ds = new DataSet();
            OdbcConnection ocn = new OdbcConnection(con_str);
            try
            {
                OdbcDataAdapter dap = new OdbcDataAdapter(sql, ocn);
                ocn.Open() ;
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
           
            //----------------------
            if (ds.Tables[0].Rows.Count > 1)
            {

                Frm_bjhtyy yc = new Frm_bjhtyy(ds);
                yc.ShowDialog();
                string sqxh = yc.SQXH;
                if (sqxh.Trim() == "")
                {
                    MessageBox.Show("未选择数据，不能提取");
                    return new DataTable();
                }
                DataTable dtNew = new DataTable();
                DataView view = new DataView();
                view.Table = ds.Tables[0];
                view.RowFilter = "EXAMCODE='" + sqxh + "' ";
                dtNew = view.ToTable();
                return dtNew;
            }

            return ds.Tables[0];


        }
    }
}