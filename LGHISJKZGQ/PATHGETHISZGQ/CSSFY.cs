using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.Odbc;
using dbbase;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 长沙市妇幼保健院-体检接口
    /// </summary>
    class CSSFY
    {
          ///
          ///体检
          ///
        private static LGHISJKZGQ.IniFiles f = new LGHISJKZGQ.IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx == "体检号" || Sslbx == "体检条码号")
            {
                DataSet ds = new DataSet();
                OdbcConnection sqlcon = null;
                string odbcstr = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet-TJ;UID=sa;PWD=zoneking;"); //获取sz.ini中设置的odbcsql
                //2017年10月9日15:56:52 刘冬阳
                //查询条件从ID改为EXAM_NO,解决一个人多个条码时,分不清项目的问题.
                string sqlstr = f.ReadString(Sslbx, "hissql", "select ID as 病人编号, '体检'  as 病人类别,'-' as 费别,'' as 住院号,'' as 门诊号,Name as 姓名,Sex as 性别,cast(datediff(year,BirthDay,getdate()) as varchar)+'岁' as 年龄,'' as 婚姻,Address as 地址,Tel as 电话,'' AS 病区,'' as 床号,'-' as 身份证号,'' as 民族,'' as 职业,'体检中心' as 送检科室,QYDoctor AS 送检医生,'' as 临床诊断,' ' as 临床病史,'' as 收费,Pacs_Item_Code as 就诊ID,Exam_No as 申请序号,'' as 标本名称,'本院' AS 送检医院,Item_Name as 医嘱项目,'' as 备用1,dwmc as 备用2 from V_TCT_InterFace WHERE exam_no = 'f_sbh' AND StudyType='BL'  order by Exam_No  desc");
                sqlstr = sqlstr.Replace("f_sbh", "T"+Ssbz.Trim());
                string count = f.ReadString(Sslbx, "count", "");
                if (Debug == "1")
                    MessageBox.Show(sqlstr);
                try
                {
                    sqlcon = new OdbcConnection(odbcstr);
                    OdbcDataAdapter sqlda = new OdbcDataAdapter(sqlstr, sqlcon);
                    sqlda.Fill(ds, "tjxx");
                    sqlda.Dispose();
                }
                catch(Exception ee)
                {
                    MessageBox.Show("体检系统数据库连接错误！！！"+ee.ToString());
                    return "0";
                }
                finally
                {
                    if (sqlcon.State == ConnectionState.Open)
                        sqlcon.Close();
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("此体检号无记录！");
                    return "0";
                }

                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 1)
                {
                    string Columns = f.ReadString(Sslbx, "Columns", "");
                    string Col = f.ReadString(Sslbx, "RowFilter", "");



                    if (count.Trim() == "")
                        count = "-1";

                    FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0], int.Parse(count), Columns, Col);
                    yc.ShowDialog();
                    string string1 = yc.F_STRING[0];
                    string string2 = yc.F_STRING[1];
                    string string3 = yc.F_STRING[2];
                    string string4 = yc.F_STRING[3];

                    if (string1.Trim() == "" && string2.Trim() == "")
                    {
                        MessageBox.Show("未选择记录");
                        return "0";
                    }
                    DataView view = new DataView();
                    view.Table = ds.Tables[0];


                    string odr = "" + ds.Tables[0].Columns[0].ColumnName + "='" + string1 + "'  and  " + ds.Tables[0].Columns[1].ColumnName + "='" + string2 + "'  and  " + ds.Tables[0].Columns[2].ColumnName + "='" + string3 + "' and  " + ds.Tables[0].Columns[3].ColumnName + "='" + string4 + "'";

                    if (Col.Trim() != "")
                    {
                        string[] colsss = Col.Split(',');
                        odr = "" + colsss[0] + "='" + yc.F_STRING[0] + "'";
                        if (colsss.Length > 1)
                        {
                            for (int i = 1; i < colsss.Length; i++)
                            {
                                if (i < 4)
                                    odr = odr + " and  " + colsss[i] + "='" + yc.F_STRING[i] + "' ";
                            }
                        }
                    }
                    ///   MessageBox.Show(odr);
                    view.RowFilter = odr;

                    dt = view.ToTable();
                }
                else
                    dt = ds.Tables[0];

           
                if (dt.Rows.Count<1)
                {
                    MessageBox.Show("此体检号无记录！！！");
                    return "0";
                }
                //-返回xml----------------------------------------------------
                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["病人编号"].ToString() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["就诊ID"].ToString() + (char)34 + " ";
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
                    xml = xml + "送检医院=" + (char)34 + dt.Rows[0]["送检医院"].ToString() + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["医嘱项目"].ToString() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + dt.Rows[0]["备用1"].ToString() + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + dt.Rows[0]["备用2"].ToString() + (char)34 + " ";
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
    }
}
