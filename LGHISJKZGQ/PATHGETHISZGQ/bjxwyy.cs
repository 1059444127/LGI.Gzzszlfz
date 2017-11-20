using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class bjxwyy
    {
        //北京宣武医院
        //存储过程  exec BL_GetPatientInfo  '114640813','门诊'   （门诊，住院，留观）
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            
            if (Sslbx != "")
            {


                string con_str = f.ReadString(Sslbx, "odbcsql", "Server=.;Database=pathnet;User Id=pathnet;Password=4s3c2a1p;");
                string Sslbx1 = Sslbx;
                if (Sslbx == "住院号")
                    Sslbx1 = "住院";
                if (Sslbx == "门诊号")
                    Sslbx1 = "门诊";
                if (Sslbx == "留院号")
                    Sslbx1 = "留院";
                if (Sslbx == "急诊号")
                    Sslbx1 = "急诊";
                DataTable  dt=new DataTable();
                if (Sslbx1 != "")
                  dt= getXX(con_str, Sslbx1, Ssbz);
                else
                {
                    MessageBox.Show("无此识别号：" + Sslbx1);
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
                    xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["F_ZYCS"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "申请序号=" + (char)34 + dt.Rows[0]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt.Rows[0]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "姓名=" + (char)34 + dt.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "性别=" + (char)34 + dt.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";

                    string datatime=DateTime.Today.Date.ToString();
                    if (dt.Rows[0]["F_RYRQ"].ToString().Trim() != "")
                        datatime = dt.Rows[0]["F_RYRQ"].ToString();
                    string CSRQ = dt.Rows[0]["F_CSRQ"].ToString();
                    try
                    {
                        int Year = DateTime.Parse(datatime).Year - DateTime.Parse(CSRQ).Year;
                        int Month = DateTime.Parse(datatime).Month - DateTime.Parse(CSRQ).Month;
                        int day = DateTime.Parse(datatime).Day - DateTime.Parse(CSRQ).Day;


                        if (Year>=3)
                        {

                            if (Month>0)
                               xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                            if (Month < 0)
                               xml = xml + "年龄=" + (char)34 + (Year-1) + "岁" + (char)34 + " ";
                            if (Month == 0)
                            {
                               if(day>=0)
                                   xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                               else
                                 xml = xml + "年龄=" + (char)34 + (Year-1) + "岁" + (char)34 + " ";
                            }
                        }
                        if (Year > 0 && Year < 3)
                        {
                            if((Year - 1)== 0)
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
                                xml = xml + "年龄=" + (char)34 +Year+"岁"+ Month + "月"+ (char)34 + " ";
                            else
                                xml = xml + "年龄=" + (char)34 + (Year-1) + "岁" + (12 + Month) + "月" + (char)34 + " ";
                           
                            }

                        }
                        if (Year== 0)
                        {
                         int day1=DateTime.Parse(datatime).DayOfYear - DateTime.Parse(CSRQ).DayOfYear;

                         int m = day1 / 30;
                         int d = day1 % 30;
                         xml = xml + "年龄=" + (char)34 + m +"月"+ d+"天" + (char)34 + " ";
                        }

                    }
                    catch
                    {
                        xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                    }
                    xml = xml + "婚姻=" + (char)34 + dt.Rows[0]["F_HY"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                    xml = xml + "电话=" + (char)34 + dt.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病区=" + (char)34 + dt.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "床号=" + (char)34 + dt.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "职业=" + (char)34 + dt.Rows[0]["F_ZY"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["F_SJYS"].ToString().Trim() + (char)34 + " ";

                    xml = xml + "收费=" + (char)34 +"" + (char)34 + " ";

                    xml = xml + "标本名称=" + (char)34 + dt.Rows[0]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["F_YZXM"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                    xml = xml + "费别=" + (char)34 + dt.Rows[0]["F_FB"].ToString().Trim() + (char)34 + " ";

                    xml = xml + "病人类别=" + (char)34 + dt.Rows[0]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<临床病史><![CDATA[" + dt.Rows[0]["F_LCBS"].ToString().Trim() + "    " + "入院时间：" + dt.Rows[0]["F_RYRQ"].ToString().Trim() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";
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

        public static DataTable getXX(string con_str, string Sslbx, string Ssbz)
        {
         
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(con_str);
            try
            {

           
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "BL_GetPatientInfo";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = DbType.String;
            dbParameter.ParameterName = "@code";
            dbParameter.Value = Ssbz;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);

            SqlParameter dbParameter2 = cmd.CreateParameter();
            dbParameter2.DbType = DbType.String;
            dbParameter2.ParameterName = "@type";
            dbParameter2.Value = Sslbx;
            dbParameter2.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter2);

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();
            dap.Fill(ds);
            con.Close();
            cmd.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("HIS数据库连接错误,"+e.ToString());
                log.WriteMyLog("访问HIS数据库失败！" + e.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            //----------------------
            if (ds.Tables[0].Rows.Count > 1)
            {


                Frm_bjxwyy_select yc = new Frm_bjxwyy_select(ds);
                yc.ShowDialog();
                string sjys = yc.SJYS;
                string sqxh = yc.SQXH;
                string bbmc = yc.BBMC;  
                string zycs = yc.ZYCS;
                string ryrq= yc.RYRQ;

                DataTable dtNew = new DataTable();
                DataView view = new DataView();
                view.Table = ds.Tables[0];
                view.RowFilter = "F_SJYS = '" + sjys + "' and F_SQXH='" + sqxh + "' and F_bbmc='" + bbmc + "'and F_ZYCS='" + zycs + "'  and F_ryrq='" + ryrq + "'";
                dtNew = view.ToTable();
                return dtNew;
            }

            return ds.Tables[0];


        }
    }
}
