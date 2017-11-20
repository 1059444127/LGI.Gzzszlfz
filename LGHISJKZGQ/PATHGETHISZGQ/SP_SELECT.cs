using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    class SP_SELECT
    {
        private static LGHISJKZGQ.IniFiles f = new LGHISJKZGQ.IniFiles(Application.StartupPath + "\\sz.ini");

////        odbcsql=DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;
////hissql=select * from T_JCXX  where F_BLH='f_sbh'
////Params=病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,年龄,出生日期,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史
////Values=F_BRBH,F_YZID,F_SQXH,F_MZH,F_ZYH,F_XM,F_XB,F_nl,,F_HY,F_HY,F_HY,F_BQ,F_CH,F_SFZH,F_MZ,F_ZY,F_SJKS,F_SJYS,F_sf,F_BBMC,F_sJDW,F_YZXM,F_BY1,F_BY2,F_FB,F_brlb,F_LCZD,F_lczl

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {

                string odbcsql = f.ReadString(Sslbx, "odbcsql", "");
                string dbtype = f.ReadString(Sslbx, "dbtype", "ODBC");
                string hissql = f.ReadString(Sslbx, "hissql", "");
                string count = f.ReadString(Sslbx, "count", "");

                hissql = hissql.Replace("f_sbh", Ssbz.Trim());

                string Params = f.ReadString(Sslbx, "Params", "");
                string Values = f.ReadString(Sslbx, "Values", "");
                string ordersql = f.ReadString(Sslbx, "ordersql", "");




                if (odbcsql.Trim() == "")
                {
                    MessageBox.Show("未配置odbcsql，不能提取");
                    return "0";
                }
                if (hissql.Trim() == "")
                {
                    MessageBox.Show("未配置hissql，不能提取");
                    return "0";
                }

                string[] Params_1 = Params.Split(',');
                string[] Values_1 = Values.Split(',');

                string exec = "";
                DataTable dt = new DataTable("tab1");
                DataTable dt2 = new DataTable("tab2");

                DataSet ds = new DataSet();
                if (dbtype.ToUpper() == "OLEDB")
                {
                    OleDbDB_ZGQ oledb = new OleDbDB_ZGQ();
                    ds = oledb.OleDb_DataAdapter_DataSet(odbcsql, hissql,ref exec);
                }else
                    if (dbtype.ToUpper() == "ORACLE")
                    {
                        OracleDB_ZGQ oracle = new OracleDB_ZGQ();
                        ds = oracle.Oracle_DataAdapter_DataSet(odbcsql, hissql,ref exec);
                    }else
                        if (dbtype.ToUpper() == "SQLSERVER")
                        {
                            SqlDB_ZGQ sql=new SqlDB_ZGQ();
                            ds = sql.Sql_DataAdapter_DataSet(odbcsql, hissql,ref exec);
                        }
                        else
                        {
                            OdbcDB_ZGQ  odbc=new OdbcDB_ZGQ();
                            ds = odbc.Odbc_DataAdapter_DataSet(odbcsql, hissql,ref exec);
                        }
                if(exec!="")
                { MessageBox.Show(exec);
                    return "0";
                }
                /////////////////////////////////////////////////////////////////////
                if (ds.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("未能查询到对应的数据记录！请确认" + Sslbx + "是否正确");
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        string Columns = f.ReadString(Sslbx, "Columns", "");//显示的项目
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "");//显示的项目
                        string Col = f.ReadString(Sslbx, "RowFilter", ""); //选择条件的项目
                        string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                      
                   
                        if(count.Trim()=="")
                            count="-1";

                        FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0], int.Parse(count), Columns, ColumnsName, Col, xsys);
                    yc.ShowDialog();
                    string string1 = yc.F_STRING[0];
                    string string2 = yc.F_STRING[1];
                    string string3 = yc.F_STRING[2];
                    string string4 = yc.F_STRING[3];

                    if (string1.Trim() == "" && string2.Trim() == "" )
                    {
                        MessageBox.Show("未选择记录");
                        return "0";
                    }
                 DataView view = new DataView();
                 view.Table = ds.Tables[0];


                   string odr="" + ds.Tables[0].Columns[0].ColumnName + "='" + string1 + "'  and  " + ds.Tables[0].Columns[1].ColumnName + "='" + string2 + "'  and  " + ds.Tables[0].Columns[2].ColumnName + "='" + string3 + "' and  " + ds.Tables[0].Columns[3].ColumnName + "='" + string4 + "'";
                  
                   if (Col.Trim() != "")
                 { string [] colsss = Col.Split(',');
                     odr=""+ colsss[0] + "='" + yc.F_STRING[0] + "'";
                     if (colsss.Length > 1)
                     {
                         for (int i = 1; i < colsss.Length; i++)
                         {
                             if(i<4)
                             odr = odr + " and  " + colsss[i] + "='" + yc.F_STRING[i] + "' ";
                         }
                     }
                 }

                 view.RowFilter = odr;
                
                     dt = view.ToTable();
                    }
                    else
                        dt = ds.Tables[0];



                    try
                    {
                       
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        if (Params_1.Length < 25)
                        {
                            xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["病人编号"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["就诊ID"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + dt.Rows[0]["申请序号"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["门诊号"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + dt.Rows[0]["住院号"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + dt.Rows[0]["姓名"].ToString().Trim() + (char)34 + " ";
                            string xb = dt.Rows[0]["性别"].ToString().Trim();
                            if (xb == "F" || xb == "f")
                                xb = "女";
                            if (xb == "M" || xb == "m")
                                xb = "男";
                            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                            xml = xml + "年龄=" + (char)34 + dt.Rows[0]["年龄"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "婚姻=" + (char)34 + dt.Rows[0]["婚姻"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + dt.Rows[0]["地址"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + dt.Rows[0]["电话"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + dt.Rows[0]["病区"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + dt.Rows[0]["床号"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["身份证号"].ToString().Replace('\'',' ').Trim() + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + dt.Rows[0]["民族"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + dt.Rows[0]["职业"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["送检科室"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["送检医生"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "收费=" + (char)34 + dt.Rows[0]["收费"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34 + dt.Rows[0]["标本名称"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + dt.Rows[0]["送检医院"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + dt.Rows[0]["备用1"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + dt.Rows[0]["备用2"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + dt.Rows[0]["费别"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + dt.Rows[0]["病人类别"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "/>";

                            xml = xml + "<临床病史><![CDATA[" + dt.Rows[0]["临床病史"].ToString().Trim() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["临床诊断"].ToString().Trim() + "]]></临床诊断>";

                        }
                        else
                        {
                            for (int y = 0; y < Params_1.Length - 2; y++)
                            {

                                if (Params_1[y] == "年龄")
                                {
                                    try
                                    {
                                        if (Values_1[y].Trim() != "")
                                        {
                                            if (Values_1[y].Trim().Contains("+"))
                                            {
                                                string[] aa = Values_1[y].Trim().Split('+');
                                                xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "";
                                                for (int x = 0; x < aa.Length; x++)
                                                {
                                                    if (aa[x].Trim().Contains("\""))
                                                        xml = xml + aa[x].Trim().Replace('\"', ' ').Trim();
                                                    else
                                                        xml = xml + dt.Rows[0][aa[x].Trim()].ToString().Trim();
                                                }
                                                xml = xml + "" + (char)34 + " ";
                                            }
                                            else
                                            xml = xml + "年龄=" + (char)34 + dt.Rows[0][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                        }
                                        else
                                        {
                                            if (Values_1[y + 1].Trim() == "")
                                            {
                                                xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                            }
                                            else
                                            {
                                                string CSRQ = dt.Rows[0][Values_1[y + 1].Trim()].ToString().Trim();
                                                DateTime dtime = new DateTime();
                                                try
                                                {
                                                    if (CSRQ.Contains("-"))
                                                    {
                                                        TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                                        dtime = dtime.Add(tp);
                                                    }
                                                    else
                                                    {
                                                        if (CSRQ.Contains("/"))
                                                        {
                                                            TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                                            dtime = dtime.Add(tp);
                                                        }
                                                        else
                                                        {
                                                            if (CSRQ.Length > 8)
                                                                CSRQ = CSRQ.Substring(0, 8);
                                                            TimeSpan tp = DateTime.Now - DateTime.Parse(string.Format("{0:0000-00-00}", Convert.ToInt32(CSRQ.ToString())));
                                                            dtime = dtime.Add(tp);
                                                        }
                                                    }
                                                    int Year = dtime.Year - 1;
                                                    int Month = dtime.Month - 1;
                                                    int day = dtime.Day;

                                                    if (Year >= 3)
                                                        xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                                    else
                                                    {
                                                        if (Year == 0)
                                                            xml = xml + "年龄=" + (char)34 + Month + "月" + day + "天" + (char)34 + " ";
                                                        else
                                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + Month + "月" + (char)34 + " ";
                                                    }
                                                }
                                                catch
                                                {
                                                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (Values_1[y].Trim() == "")
                                            xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                        else
                                        {
                                            try
                                            {
                                                if (Values_1[y].Trim().Contains("+"))
                                                {
                                                    string[] aa = Values_1[y].Trim().Split('+');
                                                    xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "";
                                                    for (int x = 0; x < aa.Length; x++)
                                                    {
                                                        if (aa[x].Trim().Contains("\""))
                                                            xml = xml + aa[x].Trim().Replace('\"', ' ').Trim();
                                                        else
                                                            xml = xml + dt.Rows[0][aa[x].Trim()].ToString().Trim() + " ";
                                                    }
                                                    xml = xml + "" + (char)34 + " ";
                                                }
                                                else
                                                {
                                                    if (Values_1[y].Trim().Contains("\""))
                                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + Values_1[y].Trim().Replace('\"', ' ').Trim() + (char)34 + " ";
                                                    else
                                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + dt.Rows[0][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                                }
                                            }

                                            catch
                                            {
                                                xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                    }
                                }
                            }

                            xml = xml + "/>";
                            ////////////////////////////////////////////////////////////
                            try
                            {
                                if (Values_1[Values_1.Length - 1].Trim() == "")
                                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                                else
                                {
                                    try
                                    {
                                        if (Values_1[Values_1.Length - 1].Trim().Contains("+"))
                                        {
                                            string[] aa = Values_1[Values_1.Length - 1].Trim().Split('+');
                                            xml = xml + "" + "<临床病史><![CDATA[";
                                            for (int x = 0; x < aa.Length; x++)
                                            {
                                                if (aa[x].Trim().Contains("\""))
                                                    xml = xml + aa[x].Trim().Replace('\"', ' ').Trim()+"  ";
                                                else
                                                    xml = xml + dt.Rows[0][aa[x].Trim()].ToString().Trim() +" ";
                                            }
                                            xml = xml + "]]></临床病史>";
                                        }
                                        else
                                        {
                                            if (Values_1[Values_1.Length - 1].Trim().Contains("\""))
                                                xml = xml + "<临床病史><![CDATA["  + Values_1[Values_1.Length - 1].Trim().Replace('\"', ' ').Trim() + "]]></临床病史>";
                                            else
                                                xml = xml + "<临床病史><![CDATA[" + dt.Rows[0][Values_1[Values_1.Length - 1]].ToString().Trim() + "]]></临床病史>";
                                        }
                                    }
                                    catch
                                    {
                                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                                    }

                                }
                            }
                            catch
                            {
                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            }
                            ////////////////////////////////////////////////////////
                            //try
                            //{
                            //    if (Values_1[Values_1.Length - 2].Trim() == "")
                            //        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            //    else
                            //        xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0][Values_1[Values_1.Length - 2].Trim()].ToString().Trim() + "]]></临床诊断>";
                            //}
                            //catch
                            //{
                            //    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            //}
                            //}

                            try
                            {
                                if (Values_1[Values_1.Length - 2].Trim() == "")
                                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                else
                                {
                                    try
                                    {
                                        if (Values_1[Values_1.Length - 2].Trim().Contains("+"))
                                        {
                                            string[] aa = Values_1[Values_1.Length - 2].Trim().Split('+');
                                            xml = xml + "" + "<临床诊断><![CDATA[";
                                            for (int x = 0; x < aa.Length; x++)
                                            {
                                                if (aa[x].Trim().Contains("\""))
                                                    xml = xml + aa[x].Trim().Replace('\"', ' ').Trim()+" ";
                                                else
                                                    xml = xml + dt.Rows[0][aa[x].Trim()].ToString().Trim() + " ";
                                            }
                                            xml = xml + "]]></临床诊断>";
                                        }
                                        else
                                        {
                                            if (Values_1[Values_1.Length - 2].Trim().Contains("\""))
                                                xml = xml + "<临床诊断><![CDATA["  +  Values_1[Values_1.Length - 2].Trim().Replace('\"', ' ').Trim() + "]]></临床诊断>";
                                            else
                                                xml = xml + "<临床诊断><![CDATA["  +  dt.Rows[0][Values_1[Values_1.Length - 2]].ToString().Trim() + "]]></临床诊断>";
                                        }
                                    }
                                    catch
                                    {
                                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                    }

                                }
                            }
                            catch
                            {
                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            }
                            /////////////////////////////////////////////////////

                        }
                        xml = xml + "</LOGENE>";


                        if (Debug == "1")
                            log.WriteMyLog(xml);
                        return xml;


                    }
                    catch (Exception e)
                    {
                            MessageBox.Show("提取信息出错，异常"+e.ToString());
                        return "0";
                    }
              
            } return "0";
        }

        public static string ptxml2(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {
                Debug = f.ReadString(Sslbx, "debug", "");
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;");
                string dbtype = f.ReadString(Sslbx, "dbtype", "ODBC");
                
                string hissql = f.ReadString(Sslbx, "hissql", "");
               

                hissql = hissql.Replace("f_sbh", Ssbz.Trim());

                string Params = f.ReadString(Sslbx, "Params", "病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,出生日期,年龄,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史");
                string Values = f.ReadString(Sslbx, "Values", "病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,出生日期,年龄,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史");
                string ordersql = f.ReadString(Sslbx, "ordersql", "");
                string RowFilter = f.ReadString(Sslbx, "RowFilter", "");
                string Columns = f.ReadString(Sslbx, "Columns", "");//显示的项目对应字段
                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "");//显示的项目名称


                string csrqjsnl = f.ReadString(Sslbx, "csrqjsnl", "");
                  int jsnlfs = f.ReadInteger(Sslbx, "jsnlfs",1);
                string sfzhjsnl = f.ReadString(Sslbx, "sfzhjsnl", "");
                int tkts = f.ReadInteger(Sslbx, "tkts", 2);
                int SelCount = f.ReadInteger(Sslbx, "Count", 20);

                int count = 0;

                if (odbcsql.Trim() == "")
                {
                    MessageBox.Show("未配置odbcsql，不能提取");
                    return "0";
                }
                if (hissql.Trim() == "")
                {
                    MessageBox.Show("未配置hissql，不能提取");
                    return "0";
                }

                string[] Params_1 = Params.Split(',');
                string[] Values_1 = Values.Split(',');

                string exec = "";
                DataTable dt = new DataTable("tab1");
                DataTable dt2 = new DataTable("tab2");

               DataTable  Dt=new DataTable();
                if (dbtype.ToUpper() == "OLEDB")
                {
                    OleDbDB_ZGQ oledb = new OleDbDB_ZGQ();
                    Dt = oledb.OleDb_DataAdapter(odbcsql, hissql, ref exec);
                }
                else
                    if (dbtype.ToUpper() == "ORACLE")
                    {
                        OracleDB_ZGQ oracle = new OracleDB_ZGQ();
                        Dt = oracle.Oracle_DataAdapter(odbcsql, hissql, ref exec);
                    }
                    else
                        if (dbtype.ToUpper() == "SQLSERVER")
                        {
                            SqlDB_ZGQ sql = new SqlDB_ZGQ();
                            Dt = sql.Sql_DataAdapter(odbcsql, hissql, ref exec);
                        }
                        else
                        {
                            OdbcDB_ZGQ odbc = new OdbcDB_ZGQ();
                            Dt = odbc.Odbc_DataAdapter(odbcsql, hissql, ref exec);
                        }
                if (exec != "")
                {
                    MessageBox.Show(exec);
                    return "0";
                }
                /////////////////////////////////////////////////////////////////////

                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("未能查询到对应的数据记录！请确认" + Sslbx + "是否正确");
                    return "0";
                }

                DataTable  dt_cx=new DataTable();
                if (RowFilter.Trim() != "")
                {
                    DataView dv = Dt.DefaultView;
                    dv.RowFilter = RowFilter;
                    dt_cx = dv.ToTable();
                }
                else
                {
                    dt_cx = Dt;
                }


                if (dt_cx.Rows.Count == 0)
                {
                    MessageBox.Show("未能查询到对应的数据记录！请确认" + Sslbx + "是否正确");
                    return "0";
                }
            
                //根据身份证号或出生日期计算年龄
                if (csrqjsnl == "1" || sfzhjsnl == "1")
                {
                    try
                    {  
                      

                string csrqzdm = ""; 
                        
                        string sfzhzdm = "";
                for (int y = 0; y < Params_1.Length; y++)
                {
                    if (Params_1[y] == "出生日期")
                    {
                        csrqzdm = Values_1[y].Trim();
                        continue;
                    }
                }

                for (int y = 0; y < Params_1.Length; y++)
                {
                    if (Params_1[y] == "身份证号")
                    {
                        sfzhzdm = Values_1[y].Trim();
                        continue;
                    }

                }
                for (int x = 0; x < dt_cx.Rows.Count; x++)
                        {
                            if (sfzhjsnl == "1")
                            {
                                if (sfzhzdm != "" )
                                {

                                    if (dt_cx.Rows[x][sfzhzdm].ToString().Length > 8)
                                        dt_cx.Rows[x]["F_AGE_1"] =ZGQClass.SfzhToAge(dt_cx.Rows[x][sfzhzdm].ToString().Substring(6, 8), jsnlfs);
                                    else
                                        dt_cx.Rows[x]["F_AGE_1"] = "";
                                }
                            }
                            else
                            if (csrqjsnl == "1")
                            {
                              
                                if (csrqzdm != "" )
                                    dt_cx.Rows[x]["F_AGE_1"] = ZGQClass.CsrqToAge(dt_cx.Rows[x][csrqzdm].ToString(), jsnlfs);
                                else
                                    dt_cx.Rows[x]["F_AGE_1"] = "";
                            }
                            else
                                dt_cx.Rows[x]["F_AGE_1"] = "";
                        }
                    }
                    catch(Exception  eee)
                    {
                        MessageBox.Show(eee.Message);
                    }
                }
     
                if (SelCount >1 && dt_cx.Rows.Count>=tkts)
                {
                    if (dt_cx.Rows.Count > tkts)
                    {
                      

                        string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                        DataColumn dc0 = new DataColumn("序号");
                        dt_cx.Columns.Add(dc0);

                        for (int x = 0; x < dt_cx.Rows.Count; x++)
                        {
                            dt_cx.Rows[x][dt_cx.Columns.Count - 1] = x;
                        }

                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;
                        string rtn2 ="0";
                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_cx, Columns, ColumnsName, xsys);
                        if (yc.ShowDialog() == DialogResult.Yes)
                        {
                            rtn2 = yc.F_XH;
                        }
                        else
                        {
                            MessageBox.Show("未选择申请项目！");
                            return "0";
                        }
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("未选择申请项目！");
                            return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("请重新选择申请项目！");
                            return "0";
                        }

                    }
                }

               
                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    if (Params_1.Length <=20)
                    {
        
                        xml = xml + "病人编号=" + (char)34 + dt_cx.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_cx.Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_cx.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_cx.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_cx.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_cx.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                        string xb = dt_cx.Rows[count]["性别"].ToString().Trim();
                        if (xb == "F" || xb == "f")
                            xb = "女";
                        if (xb == "M" || xb == "m")
                            xb = "男";
                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                        xml = xml + "年龄=" + (char)34 + dt_cx.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + dt_cx.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + dt_cx.Rows[count]["地址"].ToString().Trim() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_cx.Rows[count]["电话"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_cx.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_cx.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_cx.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_cx.Rows[count]["民族"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + dt_cx.Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_cx.Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_cx.Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + dt_cx.Rows[count]["收费"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_cx.Rows[count]["标本名称"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + dt_cx.Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_cx.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + dt_cx.Rows[count]["备用1"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + dt_cx.Rows[count]["备用2"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dt_cx.Rows[count]["费别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_cx.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";

                        xml = xml + "<临床病史><![CDATA[" + dt_cx.Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_cx.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";

                    }
                    else
                    {
                  
                        for (int y = 0; y < Params_1.Length - 2; y++)
                        {

                            if (Params_1[y] == "年龄")
                            {
                               
                                if (Values_1[y].Trim() != "")
                                {
                                    if (Values_1[y].Trim().Contains("+"))
                                    {
                                        string[] aa = Values_1[y].Trim().Split('+');
                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "";
                                        for (int x = 0; x < aa.Length; x++)
                                        {
                                            if (aa[x].Trim().Contains("\""))
                                                xml = xml + aa[x].Trim().Replace('\"', ' ').Trim();
                                            else
                                                xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim();
                                        }
                                        xml = xml + "" + (char)34 + " ";
                                    }
                                    else
                                        xml = xml + "年龄=" + (char)34 + dt_cx.Rows[count][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                }
                                else
                                {
                                   
                                      if (csrqjsnl == "1" || sfzhjsnl == "1")
                                          xml = xml + "年龄=" + (char)34 + dt_cx.Rows[count]["F_AGE_1"] + (char)34 + " ";
                                      else
                                          xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                }
                              
                            }
                            else
                            {
                                try
                                {
                                    if (Values_1[y].Trim() == "")
                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                    else
                                    {
                                        try
                                        {
                                            if (Values_1[y].Trim().Contains("+"))
                                            {
                                                string[] aa = Values_1[y].Trim().Split('+');
                                                xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "";
                                                for (int x = 0; x < aa.Length; x++)
                                                {
                                                    if (aa[x].Trim().Contains("\""))
                                                    {
                                                        try
                                                        {
                                                            xml = xml + aa[x].Trim().Replace('\"', ' ').Trim();
                                                        }
                                                        catch
                                                        {
                                                            xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                        }
                                                    }
                                                    else
                                                        xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                }
                                                xml = xml + "" + (char)34 + " ";
                                            }
                                            else
                                            {
                                                if (Values_1[y].Trim().Contains("\""))
                                                {
                                                    try
                                                    {
                                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + Values_1[y].Trim().Replace('\"', ' ').Trim() + (char)34 + " ";
                                                    }
                                                    catch
                                                    {
                                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + dt_cx.Rows[count][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                                    }
                                                }
                                                else
                                                    xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + dt_cx.Rows[count][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                            }
                                        }
                                        catch
                                        {
                                            xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                        }
                                    }
                                }
                                catch
                                {
                                    xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                }
                            }
                        }

                        xml = xml + "/>";
                        ////////////////////////////////////////////////////////////
                        try
                        {
                            if (Values_1[Values_1.Length - 1].Trim() == "")
                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            else
                            {
                                try
                                {
                                    if (Values_1[Values_1.Length - 1].Trim().Contains("+"))
                                    {
                                        string[] aa = Values_1[Values_1.Length - 1].Trim().Split('+');
                                        xml = xml + "" + "<临床病史><![CDATA[";
                                        for (int x = 0; x < aa.Length; x++)
                                        {
                                            if (aa[x].Trim().Contains("\""))
                                            {
                                                try
                                                {
                                                    xml = xml + aa[x].Trim().Replace('\"', ' ').Trim() + "  ";
                                                }
                                                catch
                                                {
                                                    xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                }
                                            }
                                            else
                                                xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                        }
                                        xml = xml + "]]></临床病史>";
                                    }
                                    else
                                    {
                                        if (Values_1[Values_1.Length - 1].Trim().Contains("\""))
                                        {
                                            try
                                            {
                                                xml = xml + "<临床病史><![CDATA[" + Values_1[Values_1.Length - 1].Trim().Replace('\"', ' ').Trim() + "]]></临床病史>";
                                            }
                                            catch
                                            {
                                                xml = xml + "<临床病史><![CDATA[" + dt_cx.Rows[count][Values_1[Values_1.Length - 1]].ToString().Trim() + "]]></临床病史>";
                                            }
                                        }
                                        else
                                            xml = xml + "<临床病史><![CDATA[" + dt_cx.Rows[count][Values_1[Values_1.Length - 1]].ToString().Trim() + "]]></临床病史>";
                                    }
                                }
                                catch
                                {
                                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                                }

                            }
                        }
                        catch
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        ////////////////////////////////////////////////////////


                        try
                        {
                            if (Values_1[Values_1.Length - 2].Trim() == "")
                                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            else
                            {
                                try
                                {
                                    if (Values_1[Values_1.Length - 2].Trim().Contains("+"))
                                    {
                                        string[] aa = Values_1[Values_1.Length - 2].Trim().Split('+');
                                        xml = xml + "" + "<临床诊断><![CDATA[";
                                        for (int x = 0; x < aa.Length; x++)
                                        {
                                            if (aa[x].Trim().Contains("\""))
                                            {
                                                try
                                                {
                                                    xml = xml + aa[x].Trim().Replace('\"', ' ').Trim() + " ";
                                                }
                                                catch
                                                {
                                                    xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                }
                                            }
                                            else
                                                xml = xml + dt_cx.Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                        }
                                        xml = xml + "]]></临床诊断>";
                                    }
                                    else
                                    {
                                        if (Values_1[Values_1.Length - 2].Trim().Contains("\""))
                                        {
                                            try
                                            {
                                                xml = xml + "<临床诊断><![CDATA[" + Values_1[Values_1.Length - 2].Trim().Replace('\"', ' ').Trim() + "]]></临床诊断>";
                                            }
                                            catch
                                            {
                                                xml = xml + "<临床诊断><![CDATA[" + dt_cx.Rows[count][Values_1[Values_1.Length - 2]].ToString().Trim() + "]]></临床诊断>";
                                            }
                                        }
                                        else
                                            xml = xml + "<临床诊断><![CDATA[" + dt_cx.Rows[count][Values_1[Values_1.Length - 2]].ToString().Trim() + "]]></临床诊断>";
                                    }
                                }
                                catch
                                {
                                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                }

                            }
                        }
                        catch
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        /////////////////////////////////////////////////////

                    }
                    xml = xml + "</LOGENE>";

                    if (Debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("提取信息出错，异常" + e.ToString());
                    return "0";
                }

            } return "0";
        }
        public static string ptxml2(string Sslbx, string Ssbz, string Debug,string msg)
        {

            if (Sslbx != "")
            {
                Debug = f.ReadString(Sslbx, "debug", "");
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;");
                string dbtype = f.ReadString(Sslbx, "dbtype", "ODBC");
                string hissql = f.ReadString(Sslbx, "hissql", "");
                int count = 0;

                hissql = hissql.Replace("f_sbh", Ssbz.Trim());

                string Params = f.ReadString(Sslbx, "Params", "病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,出生日期,年龄,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史");
                string Values = f.ReadString(Sslbx, "Values", "病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,出生日期,年龄,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史");
                string ordersql = f.ReadString(Sslbx, "ordersql", "");

                string RowFilter = f.ReadString(Sslbx, "RowFilter", "");


                string csrqjsnl = f.ReadString(Sslbx, "csrqjsnl", "");
                string sfzhjsnl = f.ReadString(Sslbx, "sfzhjsnl", "");
                string istk = f.ReadString(Sslbx, "istk", "0");

                int tkts = 1;

                if (istk == "1")
                    tkts = 0;



                if (odbcsql.Trim() == "")
                {
                    if(msg=="1")
                    MessageBox.Show("未配置odbcsql，不能提取");
                    return "0";
                }
                if (hissql.Trim() == "")
                {
                    if (msg == "1")
                    MessageBox.Show("未配置hissql，不能提取");
                    return "0";
                }

                string[] Params_1 = Params.Split(',');
                string[] Values_1 = Values.Split(',');

                string exec = "";
                DataTable dt = new DataTable("tab1");
                DataTable dt2 = new DataTable("tab2");
               
                DataSet ds = new DataSet();
                if (dbtype.ToUpper() == "OLEDB")
                {
                    OleDbDB_ZGQ oledb = new OleDbDB_ZGQ();
                    ds = oledb.OleDb_DataAdapter_DataSet(odbcsql, hissql, ref exec);
                }
                else
                    if (dbtype.ToUpper() == "ORACLE")
                    {
                        OracleDB_ZGQ oracle = new OracleDB_ZGQ();
                        ds = oracle.Oracle_DataAdapter_DataSet(odbcsql, hissql, ref exec);
                    }
                    else
                        if (dbtype.ToUpper() == "SQLSERVER")
                        {
                            SqlDB_ZGQ sql = new SqlDB_ZGQ();
                            ds = sql.Sql_DataAdapter_DataSet(odbcsql, hissql, ref exec);
                        }
                        else
                        {
                            OdbcDB_ZGQ odbc = new OdbcDB_ZGQ();
                            ds = odbc.Odbc_DataAdapter_DataSet(odbcsql, hissql, ref exec);
                        }
                if (exec != "")
                {
                    if (msg == "1")
                    MessageBox.Show(exec);
                    return "0";
                }
                /////////////////////////////////////////////////////////////////////
                if (ds.Tables[0] == null)
                {
                    if (msg == "1")
                    MessageBox.Show("查询数据错误");
                    return "0";
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    if (msg == "1")
                    MessageBox.Show("未能查询到对应的数据记录！请确认" + Sslbx + "是否正确");
                    return "0";
                }


                //根据身份证号或出生日期计算年龄
                if (csrqjsnl == "1" || sfzhjsnl == "1")
                {
                    try
                    {
                        DataColumn dc = new DataColumn("F_AGE_1");
                        ds.Tables[0].Columns.Add(dc);


                        string csrqzdm = "";

                        string sfzhzdm = "";
                        for (int y = 0; y < Params_1.Length; y++)
                        {
                            if (Params_1[y] == "出生日期")
                            {
                                csrqzdm = Values_1[y].Trim();
                                continue;
                            }
                        }

                        for (int y = 0; y < Params_1.Length; y++)
                        {
                            if (Params_1[y] == "身份证号")
                            {
                                sfzhzdm = Values_1[y].Trim();
                                continue;
                            }

                        }
                        for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                        {
                            if (sfzhjsnl == "1")
                            {

                                if (sfzhzdm != "")
                                {

                                    if (ds.Tables[0].Rows[x][sfzhzdm].ToString().Length > 8)
                                        ds.Tables[0].Rows[x]["F_AGE_1"] =ZGQClass.SfzhToAge(ds.Tables[0].Rows[x][sfzhzdm].ToString().Substring(6, 8));
                                    else
                                        ds.Tables[0].Rows[x]["F_AGE_1"] = "";
                                }
                            }
                            else
                                if (csrqjsnl == "1")
                                {

                                    if (csrqzdm != "")
                                        ds.Tables[0].Rows[x]["F_AGE_1"] = ZGQClass.CsrqToAge(ds.Tables[0].Rows[x][csrqzdm].ToString());
                                    else
                                        ds.Tables[0].Rows[x]["F_AGE_1"] = "";
                                }
                                else
                                    ds.Tables[0].Rows[x]["F_AGE_1"] = "";
                        }
                    }
                    catch (Exception eee)
                    {
                        if (msg == "1")
                        MessageBox.Show(eee.Message);
                    }
                }
                //    int count = 0;
                if (ds.Tables[0].Rows.Count > tkts)
                {
                    string Columns = f.ReadString(Sslbx, "Columns", "");//显示的项目对应字段
                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "");//显示的项目名称

                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                    DataColumn dc0 = new DataColumn("序号");
                    ds.Tables[0].Columns.Add(dc0);

                    for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                    {
                        ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                    }

                    if (Columns.Trim() != "")
                        Columns = "序号," + Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "序号," + ColumnsName;

                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(ds.Tables[0], Columns, ColumnsName, xsys);
                    yc.ShowDialog();
                    string rtn2 = yc.F_XH;
                    if (rtn2.Trim() == "")
                    {

                        MessageBox.Show("未选择申请项目！");
                        return "0";
                    }
                    try
                    {
                        count = int.Parse(rtn2);
                    }
                    catch
                    {
                        if (msg == "1")
                        MessageBox.Show("请重新选择申请项目！");
                        return "0";
                    }

                }


                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    if (Params_1.Length <= 28)
                    {
                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[count]["病人编号"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + ds.Tables[0].Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                        string xb = ds.Tables[0].Rows[count]["性别"].ToString().Trim();
                        if (xb == "F" || xb == "f")
                            xb = "女";
                        if (xb == "M" || xb == "m")
                            xb = "男";
                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                        xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + ds.Tables[0].Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[count]["地址"].ToString().Trim() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[count]["电话"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + ds.Tables[0].Rows[count]["民族"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + ds.Tables[0].Rows[count]["收费"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[count]["标本名称"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + ds.Tables[0].Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + ds.Tables[0].Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + ds.Tables[0].Rows[count]["备用1"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + ds.Tables[0].Rows[count]["备用2"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + ds.Tables[0].Rows[count]["费别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + ds.Tables[0].Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";

                        xml = xml + "<临床病史><![CDATA[" + ds.Tables[0].Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";

                    }
                    else
                    {
                        for (int y = 0; y < Params_1.Length - 2; y++)
                        {

                            if (Params_1[y] == "年龄")
                            {
                                //try
                                //{
                                if (Values_1[y].Trim() != "")
                                {
                                    if (Values_1[y].Trim().Contains("+"))
                                    {
                                        string[] aa = Values_1[y].Trim().Split('+');
                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "";
                                        for (int x = 0; x < aa.Length; x++)
                                        {
                                            if (aa[x].Trim().Contains("\""))
                                                xml = xml + aa[x].Trim().Replace('\"', ' ').Trim();
                                            else
                                                xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim();
                                        }
                                        xml = xml + "" + (char)34 + " ";
                                    }
                                    else
                                        xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[count][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                }
                                else
                                {
                                    if (csrqjsnl == "1" || sfzhjsnl == "1")
                                        xml = xml + "年龄=" + (char)34 + ds.Tables[0].Rows[count]["F_AGE_1"] + (char)34 + " ";
                                    else
                                        xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                }
                                //    else
                                //    {
                                //        if (Values_1[y + 1].Trim() == "")
                                //        {
                                //            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                //        }
                                //        else
                                //        {
                                //            string CSRQ = ds.Tables[0].Rows[count][Values_1[y + 1].Trim()].ToString().Trim();
                                //            DateTime dtime = new DateTime();
                                //            try
                                //            {
                                //                if (CSRQ.Contains("-"))
                                //                {
                                //                    TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                //                    dtime = dtime.Add(tp);
                                //                }
                                //                else
                                //                {
                                //                    if (CSRQ.Contains("/"))
                                //                    {
                                //                        TimeSpan tp = DateTime.Now - DateTime.Parse(CSRQ);
                                //                        dtime = dtime.Add(tp);
                                //                    }
                                //                    else
                                //                    {
                                //                        if (CSRQ.Length > 8)
                                //                            CSRQ = CSRQ.Substring(0, 8);
                                //                        TimeSpan tp = DateTime.Now - DateTime.Parse(string.Format("{0:0000-00-00}", Convert.ToInt32(CSRQ.ToString())));
                                //                        dtime = dtime.Add(tp);
                                //                    }
                                //                }
                                //                int Year = dtime.Year - 1;
                                //                int Month = dtime.Month - 1;
                                //                int day = dtime.Day;

                                //                if (Year >= 3)
                                //                    xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                //                else
                                //                {
                                //                    if (Year == 0)
                                //                        xml = xml + "年龄=" + (char)34 + Month + "月" + day + "天" + (char)34 + " ";
                                //                    else
                                //                        xml = xml + "年龄=" + (char)34 + Year + "岁" + Month + "月" + (char)34 + " ";
                                //                }
                                //            }
                                //            catch
                                //            {
                                //                xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                //            }
                                //        }
                                //    }
                                //}
                                //catch
                                //{
                                //    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                                //}

                            }
                            else
                            {
                                try
                                {
                                    if (Values_1[y].Trim() == "")
                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                    else
                                    {
                                        try
                                        {
                                            if (Values_1[y].Trim().Contains("+"))
                                            {
                                                string[] aa = Values_1[y].Trim().Split('+');
                                                xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "";
                                                for (int x = 0; x < aa.Length; x++)
                                                {
                                                    if (aa[x].Trim().Contains("\""))
                                                    {
                                                        try
                                                        {
                                                            xml = xml + aa[x].Trim().Replace('\"', ' ').Trim();
                                                        }
                                                        catch
                                                        {
                                                            xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                        }
                                                    }
                                                    else
                                                        xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                }
                                                xml = xml + "" + (char)34 + " ";
                                            }
                                            else
                                            {
                                                if (Values_1[y].Trim().Contains("\""))
                                                {
                                                    try
                                                    {
                                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + Values_1[y].Trim().Replace('\"', ' ').Trim() + (char)34 + " ";
                                                    }
                                                    catch
                                                    {
                                                        xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + ds.Tables[0].Rows[count][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                                    }
                                                }
                                                else
                                                    xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + ds.Tables[0].Rows[count][Values_1[y].Trim()].ToString().Trim() + (char)34 + " ";
                                            }
                                        }
                                        catch
                                        {
                                            xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                        }
                                    }
                                }
                                catch
                                {
                                    xml = xml + "" + Params_1[y].Trim() + "=" + (char)34 + "" + (char)34 + " ";
                                }
                            }
                        }

                        xml = xml + "/>";
                        ////////////////////////////////////////////////////////////
                        try
                        {
                            if (Values_1[Values_1.Length - 1].Trim() == "")
                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            else
                            {
                                try
                                {
                                    if (Values_1[Values_1.Length - 1].Trim().Contains("+"))
                                    {
                                        string[] aa = Values_1[Values_1.Length - 1].Trim().Split('+');
                                        xml = xml + "" + "<临床病史><![CDATA[";
                                        for (int x = 0; x < aa.Length; x++)
                                        {
                                            if (aa[x].Trim().Contains("\""))
                                            {
                                                try
                                                {
                                                    xml = xml + aa[x].Trim().Replace('\"', ' ').Trim() + "  ";
                                                }
                                                catch
                                                {
                                                    xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                }
                                            }
                                            else
                                                xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                        }
                                        xml = xml + "]]></临床病史>";
                                    }
                                    else
                                    {
                                        if (Values_1[Values_1.Length - 1].Trim().Contains("\""))
                                        {
                                            try
                                            {
                                                xml = xml + "<临床病史><![CDATA[" + Values_1[Values_1.Length - 1].Trim().Replace('\"', ' ').Trim() + "]]></临床病史>";
                                            }
                                            catch
                                            {
                                                xml = xml + "<临床病史><![CDATA[" + ds.Tables[0].Rows[count][Values_1[Values_1.Length - 1]].ToString().Trim() + "]]></临床病史>";
                                            }
                                        }
                                        else
                                            xml = xml + "<临床病史><![CDATA[" + ds.Tables[0].Rows[count][Values_1[Values_1.Length - 1]].ToString().Trim() + "]]></临床病史>";
                                    }
                                }
                                catch
                                {
                                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                                }

                            }
                        }
                        catch
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        ////////////////////////////////////////////////////////


                        try
                        {
                            if (Values_1[Values_1.Length - 2].Trim() == "")
                                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            else
                            {
                                try
                                {
                                    if (Values_1[Values_1.Length - 2].Trim().Contains("+"))
                                    {
                                        string[] aa = Values_1[Values_1.Length - 2].Trim().Split('+');
                                        xml = xml + "" + "<临床诊断><![CDATA[";
                                        for (int x = 0; x < aa.Length; x++)
                                        {
                                            if (aa[x].Trim().Contains("\""))
                                            {
                                                try
                                                {
                                                    xml = xml + aa[x].Trim().Replace('\"', ' ').Trim() + " ";
                                                }
                                                catch
                                                {
                                                    xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                                }
                                            }
                                            else
                                                xml = xml + ds.Tables[0].Rows[count][aa[x].Trim()].ToString().Trim() + " ";
                                        }
                                        xml = xml + "]]></临床诊断>";
                                    }
                                    else
                                    {
                                        if (Values_1[Values_1.Length - 2].Trim().Contains("\""))
                                        {
                                            try
                                            {
                                                xml = xml + "<临床诊断><![CDATA[" + Values_1[Values_1.Length - 2].Trim().Replace('\"', ' ').Trim() + "]]></临床诊断>";
                                            }
                                            catch
                                            {
                                                xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[count][Values_1[Values_1.Length - 2]].ToString().Trim() + "]]></临床诊断>";
                                            }
                                        }
                                        else
                                            xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[count][Values_1[Values_1.Length - 2]].ToString().Trim() + "]]></临床诊断>";
                                    }
                                }
                                catch
                                {
                                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                                }

                            }
                        }
                        catch
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        /////////////////////////////////////////////////////

                    }
                    xml = xml + "</LOGENE>";

                    if (Debug == "1")
                        log.WriteMyLog(xml);
                    return xml;


                }
                catch (Exception e)
                {
                    if (msg == "1")
                    MessageBox.Show("提取信息出错，异常" + e.ToString());
                    return "0";
                }

            } return "0";
        }
    }
}
