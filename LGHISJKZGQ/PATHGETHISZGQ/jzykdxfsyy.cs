using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using dbbase;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    class jzykdxfsyy
    {
        //锦州医科大学附属医院
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            string exp = "";
              string errMsg = "";
            if (Sslbx != "")
            {
                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                   debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim();
                   string odbc = f.ReadString(Sslbx, "odbc", "Provider=MSDAORA;Data Source=ORCL68;User id=chisdb_dev;Password=chisdb_dev;").Replace("\0", "").Trim();
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string ptjk = f.ReadString(Sslbx, "ptjk", "1");
                    if (Sslbx == "取消登记标记")
                    {
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                       int x= aa.ExecuteSQL("update T_SQD set F_SQDZT='取消登记' where F_sqxh='" + Ssbz.Trim() + "'");
                       if (x > 0)
                           MessageBox.Show("取消登记成功");
                       else
                           MessageBox.Show("取消登记失败");
                       return "0";

                    }

                    if (Sslbx == "退费")
                    {
                    //    string odbc = f.ReadString(Sslbx, "odbc", "Provider=MSDAORA;Data Source=ORCL68;User id=chisdb_dev;Password=chisdb_dev;").Replace("\0", "").Trim();
                        #region  退费
                        string yhgh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim(); ;

                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable jcxx = new DataTable();
                        try
                        {
                            jcxx = aa.GetDataTable("select * from T_JCXX where F_BLH='" + Ssbz + "'", "jcxx");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                            return "0";
                        }
                        if (jcxx == null)
                        {
                            MessageBox.Show("数据库连接异常");
                            return "0";
                        }
                        if (jcxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("取消无效，未查询到对应申请单记录");
                            return "0";
                        }


                        string brlbbm = "";
                        if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
                            brlbbm = "0";
                        if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                            brlbbm = "1";
                        if (brlbbm == "")
                        {
                            MessageBox.Show("非门诊或住院病人，不能退费");
                            return "0";
                        }

                        OleDbParameter[] ops = new OleDbParameter[5];
                        for (int j = 0; j < ops.Length; j++)
                        {
                            ops[j] = new OleDbParameter();
                        }
                        ops[0].ParameterName = "v_flag_mz_zy";
                        ops[0].OleDbType = OleDbType.VarChar;
                        ops[0].Direction = ParameterDirection.Input;
                        ops[0].Size = 20;
                        ops[0].Value = brlbbm;

                        ops[1].ParameterName = "v_patient_id";//
                        ops[1].OleDbType = OleDbType.VarChar;
                        ops[1].Direction = ParameterDirection.Input;
                        ops[1].Size = 20;
                        ops[1].Value = jcxx.Rows[0]["F_BRBH"].ToString();

                        ops[2].ParameterName = "v_page_no";//
                        ops[2].OleDbType = OleDbType.VarChar;
                        ops[2].Direction = ParameterDirection.Input;
                        ops[2].Size = 20;
                        ops[2].Value = jcxx.Rows[0]["F_SQXH"].ToString();

                        ops[3].ParameterName = "v_opera";//
                        ops[3].OleDbType = OleDbType.VarChar;
                        ops[3].Direction = ParameterDirection.Input;
                        ops[3].Size = 10;
                        ops[3].Value = yhgh;

                        ops[4].ParameterName = "v_RetError";//
                        ops[4].OleDbType = OleDbType.VarChar;
                        ops[4].Direction = ParameterDirection.Output;
                        ops[4].Size = 200;

                        //回写登记状态

                        OleDbDB_ZGQ oledb = new OleDbDB_ZGQ();
                        string message_ee = "";
                        oledb.OleDb_ExecuteNonQuery(odbc, "langjia_to_charge_bak", ref ops, CommandType.StoredProcedure, ref message_ee);

                        if (message_ee.Trim() != "")
                        {
                            MessageBox.Show("退费标记失败：调HIS存储过程异常--" + message_ee);
                        }
                        else
                        {
                            if (ops[4].Value.ToString() == "2")
                            {
                                 MessageBox.Show("退费标记成功");
                                aa.ExecuteSQL("update t_jcxx set F_hisbj='0' where f_blh='" + Ssbz + "'");
                            }
                            else
                            {
                                MessageBox.Show("退费标记失败："+ ops[4].Value.ToString());
                            }
                           
                        }
                        return "0";
                        #endregion
                    }
                    if (Sslbx == "申请号ID号")
                    {
                        #region
                        string sqlstr = "";
                     
                            sqlstr = "select * from view_langjia_apply_no WHERE 申请序号='" + Ssbz + "'";

                        errMsg = "";
                        OleDbDB_ZGQ db = new OleDbDB_ZGQ();
                        DataTable dt_his_sqd = db.OleDb_DataAdapter(odbc, sqlstr, ref errMsg);
                        if (dt_his_sqd.Rows.Count <= 0)
                        {
                               sqlstr = "select * from view_langjia_apply_no WHERE HISID='" + Ssbz + "'  and  rownum<=20";
                               dt_his_sqd = db.OleDb_DataAdapter(odbc, sqlstr, ref errMsg);
                               if (dt_his_sqd.Rows.Count <= 0)
                                return GetBrxx(odbc, Sslbx, Ssbz);
                        
                        }
                       
                            int count = 0;
                            if (dt_his_sqd.Rows.Count > tkts)
                            {
                                string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                                DataColumn dc0 = new DataColumn("序号");
                                dt_his_sqd.Columns.Add(dc0);
                                string Columns = f.ReadString(Sslbx, "Columns", "病人类型,申请序号,住院号门诊号,病人姓名,病人性别,病人年龄,临床申请科室名称,临床申请医生姓名,检查部位,临床诊断");
                                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "病人类型,申请单号,住院号门诊号,姓名,性别,年龄,送检科室,送检医生,检查部位,临床诊断");
                                for (int x = 0; x < dt_his_sqd.Rows.Count; x++)
                                {
                                    dt_his_sqd.Rows[x][dt_his_sqd.Columns.Count - 1] = x;
                                }

                                if (Columns.Trim() != "")
                                    Columns = "序号," + Columns;
                                if (ColumnsName.Trim() != "")
                                    ColumnsName = "序号," + ColumnsName;

                                FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_his_sqd, Columns, ColumnsName, xsys);
                                yc.ShowDialog();
                                string rtn2 = yc.F_XH;
                                if (rtn2.Trim() == "")
                                {
                                    MessageBox.Show("未选择申请项目！");
                                    return GetBrxx(odbc, Sslbx, Ssbz);
                                }
                                try
                                {
                                    count = int.Parse(rtn2);
                                }
                                catch
                                {
                                    MessageBox.Show("请重新选择申请项目！");
                                    return GetBrxx(odbc, Sslbx, Ssbz);
                                }
                            }

                            try
                            {
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                string brlx = dt_his_sqd.Rows[count]["病人类型"].ToString().Trim();
                                if (brlx == "1") brlx = "门诊";
                                if (brlx == "2") brlx = "住院";
                                xml = xml + "病人编号=" + (char)34 + dt_his_sqd.Rows[count]["HISID"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "申请序号=" + (char)34 + dt_his_sqd.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                                if (brlx == "2")
                                {
                                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "住院号=" + (char)34 + dt_his_sqd.Rows[count]["住院号门诊号"].ToString().Trim() + (char)34 + " ";
                                }
                                else
                                {
                                    xml = xml + "门诊号=" + (char)34 + dt_his_sqd.Rows[count]["住院号门诊号"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                                }
                                xml = xml + "姓名=" + (char)34 + dt_his_sqd.Rows[count]["病人姓名"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "性别=" + (char)34 + dt_his_sqd.Rows[count]["病人性别"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "年龄=" + (char)34 + dt_his_sqd.Rows[count]["病人年龄"].ToString().Trim() + dt_his_sqd.Rows[count]["病人单位"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "地址=" + (char)34 + dt_his_sqd.Rows[count]["联系地址"].ToString().Trim() + (char)34 + "   ";
                                xml = xml + "电话=" + (char)34 + dt_his_sqd.Rows[count]["联系电话"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "床号=" + (char)34 + dt_his_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "身份证号=" + (char)34 + dt_his_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "民族=" + (char)34 + dt_his_sqd.Rows[count]["就诊次数"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "送检科室=" + (char)34 + dt_his_sqd.Rows[count]["临床申请科室名称"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "送检医生=" + (char)34 + dt_his_sqd.Rows[count]["临床申请医生姓名"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                                xml = xml + "医嘱项目=" + (char)34 + dt_his_sqd.Rows[count]["检查部位"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "病人类别=" + (char)34 + brlx + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<临床病史><![CDATA[" + dt_his_sqd.Rows[count]["临床症状"].ToString().Trim() + "|回车|" + dt_his_sqd.Rows[count]["病史及体征"].ToString().Trim() + "]]></临床病史>";
                                xml = xml + "<临床诊断><![CDATA[" + dt_his_sqd.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                                xml = xml + "</LOGENE>";
                                return xml;
                            }
                            catch (Exception ee1)
                            {
                                MessageBox.Show("提取HIS申请单信息出错，请重新操作\r\n" + ee1.Message);
                                return GetBrxx(odbc, Sslbx, Ssbz);
                            }
                       
                        #endregion
                        
                    }
                    if (Sslbx == "住院号" || Sslbx == "门诊号" || Sslbx == "ID号" || Sslbx == "申请单号")
                    {
                        #region 
                        string sqlstr = "";
                        if (ptjk == "1")
                        {
                            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                            if (Sslbx == "住院号")
                                sqlstr = "select * from  T_SQD  where F_ZYH='" + Ssbz.Trim() + "'  and F_sqdzt!='已登记'";
           
                            if (Sslbx == "门诊号")
                            
                                sqlstr = "select * from  T_SQD  where F_MZH='" + Ssbz.Trim() + "'  and F_sqdzt!='已登记'";
                            if (Sslbx == "ID号")
                                sqlstr = "select * from  T_SQD  where F_BRBH='" + Ssbz.Trim() + "'  and F_sqdzt!='已登记'";
                            if (Sslbx == "申请单号")
                                sqlstr = "select * from  T_SQD  where F_sqxh='JC" + Ssbz.Trim() + "'  and F_sqdzt!='已登记'";
                            if (sqlstr == "")
                            {
                                MessageBox.Show("错误的查询语句"); return "0";
                            }

                            DataTable dt_sqd = aa.GetDataTable(sqlstr, "dt_sqd");
                            if (dt_sqd.Rows.Count > 0)
                            {
                                int count = 0;
                                if (dt_sqd.Rows.Count > tkts)
                                {
                                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                                    DataColumn dc0 = new DataColumn("序号");
                                    dt_sqd.Columns.Add(dc0);

                                    string Columns = f.ReadString(Sslbx, "Columns", "F_brbh,F_SQXH,F_BRLB,F_XM,F_XB,F_NL,F_SQKS,F_SQYS,F_YZXM,F_BBMC");
                                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "ID号,申请单号,病人类别,姓名,性别,年龄,送检科室,送检医生,医嘱项目,标本名称");

                                    for (int x = 0; x < dt_sqd.Rows.Count; x++)
                                    {
                                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                                    }

                                    if (Columns.Trim() != "")
                                        Columns = "序号," + Columns;
                                    if (ColumnsName.Trim() != "")
                                        ColumnsName = "序号," + ColumnsName;

                                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
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
                                        MessageBox.Show("请重新选择申请项目！");
                                        return "0";
                                    }
                                }

                                try
                                {
                                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                    xml = xml + "<LOGENE>";
                                    xml = xml + "<row ";
                                    xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["F_brbh"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["F_JZLSH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["F_MZH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["F_XM"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "性别=" + (char)34 + dt_sqd.Rows[count]["F_XB"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["F_NL"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["F_HY"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "地址=" + (char)34 + dt_sqd.Rows[count]["F_DZ"].ToString().Trim() + (char)34 + "   ";
                                    xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["F_DH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["F_BQ"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["F_CH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "民族=" + (char)34 + dt_sqd.Rows[count]["F_JZCS"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "职业=" + (char)34 + dt_sqd.Rows[count]["F_CSRQ"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "标本名称=" + (char)34 + dt_sqd.Rows[count]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                                    xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["F_YZXM"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                                    xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "/>";
                                    xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["F_LCBS"].ToString().Trim() + "|回车|" + dt_sqd.Rows[count]["F_LCZL"].ToString().Trim() + "]]></临床病史>";
                                    xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";
                                    xml = xml + "</LOGENE>";
                                    return xml;
                                }
                                catch (Exception ee1)
                                {
                                    MessageBox.Show("提取申请表信息出错，请重新操作\r\n" + ee1.Message);
                                }
                            }
                        }


                        if (Sslbx == "住院号")
                         sqlstr = "select * from view_langjia_apply_no WHERE 住院号门诊号='" + Ssbz + "'  and 病人类型='2' and  rownum<20";
                        
                        if (Sslbx == "门诊号")
                        sqlstr = "select * from view_langjia_apply_no WHERE 住院号门诊号='" + Ssbz + "'  and 病人类型='1' and  rownum<20";
                       
                        if (Sslbx == "ID号")
                         sqlstr = "select * from view_langjia_apply_no WHERE HISID='" + Ssbz + "'  and  rownum<50";
                        if (Sslbx == "申请单号")
                            sqlstr = "select * from view_langjia_apply_no WHERE 申请序号='" + Ssbz + "'";

                        if (sqlstr == "")
                        {
                            MessageBox.Show("错误的查询语句2"); return "0";
                        }

                            errMsg = "";
                            OleDbDB_ZGQ db = new OleDbDB_ZGQ();
                            DataTable dt_his_sqd = db.OleDb_DataAdapter(odbc, sqlstr, ref errMsg);
                            if (dt_his_sqd.Rows.Count > 0)
                            {
                                int count = 0;
                                if (dt_his_sqd.Rows.Count > tkts)
                                {
                                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                                    DataColumn dc0 = new DataColumn("序号");
                                    dt_his_sqd.Columns.Add(dc0);

                                    string Columns = f.ReadString(Sslbx, "Columns", "病人类型,申请序号,住院号门诊号,病人姓名,病人性别,病人年龄,临床申请科室名称,临床申请医生姓名,检查部位,临床诊断");
                                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "病人类型,申请单号,住院号门诊号,姓名,性别,年龄,送检科室,送检医生,检查部位,临床诊断");

                                    for (int x = 0; x < dt_his_sqd.Rows.Count; x++)
                                    {
                                        dt_his_sqd.Rows[x][dt_his_sqd.Columns.Count - 1] = x;
                                    }

                                    if (Columns.Trim() != "")
                                        Columns = "序号," + Columns;
                                    if (ColumnsName.Trim() != "")
                                        ColumnsName = "序号," + ColumnsName;

                                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_his_sqd, Columns, ColumnsName, xsys);
                                    yc.ShowDialog();
                                    string rtn2 = yc.F_XH;
                                    if (rtn2.Trim() == "")
                                    {
                                        MessageBox.Show("未选择申请项目！");
                                        return GetBrxx(odbc, Sslbx, Ssbz);
                                    }
                                    try
                                    {
                                        count = int.Parse(rtn2);
                                    }
                                    catch
                                    {
                                        MessageBox.Show("请重新选择申请项目！");
                                        return GetBrxx(odbc, Sslbx, Ssbz);
                                    }
                                }

                                try
                                {
                                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                    xml = xml + "<LOGENE>";
                                    xml = xml + "<row ";
                                    string brlx = dt_his_sqd.Rows[count]["病人类型"].ToString().Trim();
                                    if (brlx == "1") brlx = "门诊";
                                    if (brlx == "2") brlx = "住院";
                                    xml = xml + "病人编号=" + (char)34 + dt_his_sqd.Rows[count]["HISID"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "申请序号=" + (char)34 + dt_his_sqd.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                                    if (brlx == "2")
                                    {
                                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                                        xml = xml + "住院号=" + (char)34 + dt_his_sqd.Rows[count]["住院号门诊号"].ToString().Trim() + (char)34 + " ";
                                    }
                                    else
                                    {
                                        xml = xml + "门诊号=" + (char)34 + dt_his_sqd.Rows[count]["住院号门诊号"].ToString().Trim() + (char)34 + " ";
                                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                                    }
                                    xml = xml + "姓名=" + (char)34 + dt_his_sqd.Rows[count]["病人姓名"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "性别=" + (char)34 + dt_his_sqd.Rows[count]["病人性别"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "年龄=" + (char)34 + dt_his_sqd.Rows[count]["病人年龄"].ToString().Trim() + dt_his_sqd.Rows[count]["病人单位"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "地址=" + (char)34 + dt_his_sqd.Rows[count]["联系地址"].ToString().Trim() + (char)34 + "   ";
                                    xml = xml + "电话=" + (char)34 + dt_his_sqd.Rows[count]["联系电话"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "床号=" + (char)34 + dt_his_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "身份证号=" + (char)34 + dt_his_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "民族=" + (char)34 + dt_his_sqd.Rows[count]["就诊次数"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "送检科室=" + (char)34 + dt_his_sqd.Rows[count]["临床申请科室名称"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "送检医生=" + (char)34 + dt_his_sqd.Rows[count]["临床申请医生姓名"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                                    xml = xml + "医嘱项目=" + (char)34 + dt_his_sqd.Rows[count]["检查部位"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                                    xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "病人类别=" + (char)34 + brlx + (char)34 + " ";
                                    xml = xml + "/>";
                                    xml = xml + "<临床病史><![CDATA[" + dt_his_sqd.Rows[count]["临床症状"].ToString().Trim() + "|回车|" + dt_his_sqd.Rows[count]["病史及体征"].ToString().Trim() + "]]></临床病史>";
                                    xml = xml + "<临床诊断><![CDATA[" + dt_his_sqd.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                                    xml = xml + "</LOGENE>";
                                    return xml;
                                }
                                catch (Exception ee1)
                                {
                                    MessageBox.Show("提取HIS申请单信息出错，请重新操作\r\n" + ee1.Message);
                                    return GetBrxx(odbc, Sslbx, Ssbz);
                                }
                            }
                            else
                                return GetBrxx(odbc, Sslbx, Ssbz);
                      
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show(Sslbx + "类型未设置！");
                        return "0";
                    }
            }
            else
                MessageBox.Show("");
            return "0";
        }

        public static string GetBrxx(string odbc, string brlb, string ssbz)
          {

           
              string errMsg = "";
               OleDbDB_ZGQ db = new OleDbDB_ZGQ();
              string  sql="";

               if(brlb=="住院号")
                  sql= "select  * from view_langjia_patient  WHERE 住院号='" + ssbz + "' and rownum<10 ";
               if (brlb == "门诊号")
                  sql= "select  * from view_langjia_patient  WHERE 条码号='" + ssbz + "' and rownum<10 ";
                if(brlb=="ID号")
                  sql= "select  * from view_langjia_patient  WHERE 病人编号='" + ssbz + "' and rownum<10 ";


                if (sql == "")
                {
                    MessageBox.Show("提取信息失败"); return "0";
                }
                DataTable dt_his = db.OleDb_DataAdapter(odbc,sql, ref errMsg);
                if (dt_his.Rows.Count > 0)
                {
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_his.Rows[0]["病人编号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                         if(brlb=="住院号")
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                         else
                              xml = xml + "门诊号=" + (char)34 +dt_his.Rows[0]["条码号"].ToString().Trim() + (char)34 + " ";

                           if(brlb=="住院号")
                            xml = xml + "住院号=" + (char)34 + dt_his.Rows[0]["住院号"].ToString().Trim() + (char)34 + " ";
                           else
                             xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + dt_his.Rows[0]["姓名"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + dt_his.Rows[0]["性别"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "年龄=" + (char)34 + dt_his.Rows[0]["年龄"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "婚姻=" + (char)34 + dt_his.Rows[0]["婚姻"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + dt_his.Rows[0]["地址"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + dt_his.Rows[0]["电话"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + dt_his.Rows[0]["床号"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + dt_his.Rows[0]["身份证号"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_his.Rows[0]["送检科室"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_his.Rows[0]["送检医生"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dt_his.Rows[0]["费别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_his.Rows[0]["enc_type"].ToString().Trim() +(char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_his.Rows[0]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;
                    }
                    catch (Exception ee1)
                    {
                        MessageBox.Show("提取HIS信息出错，请重新操作\r\n" + ee1.Message);
                        return "0";
                    }
                }
                else
                MessageBox.Show("未查询到此号码记录"); return "0";
            }


     

    }
}
