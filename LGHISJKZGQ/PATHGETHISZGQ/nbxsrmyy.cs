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
    class nbxsrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            if (Sslbx != "")
            {
                string exp = "";

                if (Sslbx == "门诊退费")
                {
                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + Ssbz + "'", "bljc");


                    if (bljc == null)
                    {
                        MessageBox.Show("病理号：" + Ssbz + ",病理数据库设置有问题！");
                        return "0";
                    }
                    if (bljc.Rows.Count < 1)
                    {
                        MessageBox.Show("病理号：" + Ssbz + ",病理号有错误！");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
                    {
                        MessageBox.Show("申请序号为空，不能取消退费");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
                    {
                        MessageBox.Show("非门诊病人,不能取消退费");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_mzqf"].ToString().Trim() != "1")
                    {
                        MessageBox.Show("未确费或非本系统确费，不能取消退费");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊" && bljc.Rows[0]["F_mzqf"].ToString().Trim() != "1")
                    {
                        SqlParameter[] Parameters = new SqlParameter[5];

                        for (int j = 0; j < Parameters.Length; j++)
                        {
                            Parameters[j] = new SqlParameter();
                        }
                        Parameters[0].ParameterName = "sqdh";
                        Parameters[0].SqlDbType = SqlDbType.VarChar;
                        Parameters[0].Direction = ParameterDirection.Input;
                        Parameters[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                        Parameters[1].ParameterName = "czgh";
                        Parameters[1].SqlDbType = SqlDbType.VarChar;
                        Parameters[1].Direction = ParameterDirection.Input;
                        Parameters[1].Value = yhbh;

                        Parameters[2].ParameterName = "bz";
                        Parameters[2].SqlDbType = SqlDbType.Int;
                        Parameters[2].Direction = ParameterDirection.Input;
                        Parameters[2].Value = 9;

                        Parameters[3].ParameterName = "zxysh";
                        Parameters[3].SqlDbType = SqlDbType.VarChar;
                        Parameters[3].Direction = ParameterDirection.Input;
                        Parameters[3].Value = yhmc;

                        Parameters[4].ParameterName = "return";
                        Parameters[4].SqlDbType = SqlDbType.Int;
                        Parameters[4].Direction = ParameterDirection.ReturnValue;

                        string except = "";
                        db.Sql_ExecuteNonQuery(odbcsql, "p_yjqr", ref Parameters, CommandType.StoredProcedure, ref except);
                        if (except != "")
                            MessageBox.Show("连接数据库失败，门诊病人退费失败");
                        else
                        {
                            try
                            {
                                if (int.Parse(Parameters[4].Value.ToString()) < 0)
                                {
                                    MessageBox.Show("门诊病人退费失败:" + Parameters[4].Value);
                                }
                                else
                                {
                                    aa.ExecuteSQL("update T_JCXX set F_MZQF='0' where F_BLH='" + Ssbz + "'");
                                    MessageBox.Show("门诊病人退费成功");
                                }
                            }
                            catch (Exception ee1)
                            {
                                MessageBox.Show("门诊退费异常：" + ee1.Message);
                            }
                        }
                        return "0";
                    } return "0";
                }
                if (Sslbx == "申请单号"||Sslbx == "标本条码号")
                {
                    string sqxh=Ssbz.Trim();
                    if (Sslbx == "标本条码号")
                        sqxh = Ssbz.Trim().Split('_')[0].Trim();

                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");
          
                    string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string sqlstr = "select *  from dbo.V_pa_apply   WHERE  apply_id= '" + sqxh.Trim() + "'";
                    DataTable dt_SQD = new DataTable();
                    string exp_db = "";
                    dt_SQD = db.Sql_DataAdapter(odbcsql, sqlstr, ref exp_db);
                    if (exp_db.Trim() != "")
                        MessageBox.Show("连接HIS数据库异常：" + exp_db);

                    if (dt_SQD == null)
                    {
                        MessageBox.Show("未查询到此条码号记录！");
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("未查询到此申请序号记录！");
                        return "0";
                    }

                    //取收费je




                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_SQD.Rows[0]["PATIENT_ID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt_SQD.Rows[0]["APPLY_ID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                    
                            xml = xml + "门诊号=" + (char)34 + dt_SQD.Rows[0]["INPATIENT_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt_SQD.Rows[0]["PATIENT_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_SQD.Rows[0]["PATIENT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + dt_SQD.Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "年龄=" + (char)34 + dt_SQD.Rows[0]["AGE"].ToString().Trim() + "岁" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + dt_SQD.Rows[0]["HY"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + dt_SQD.Rows[0]["ADDRESS"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt_SQD.Rows[0]["TELPHONE"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + dt_SQD.Rows[0]["PATIENT_DEPT"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt_SQD.Rows[0]["PATIENT_BED"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt_SQD.Rows[0]["SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + dt_SQD.Rows[0]["MZ"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "职业=" + (char)34 + dt_SQD.Rows[0]["ZY"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_SQD.Rows[0]["APPLY_DEPT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_SQD.Rows[0]["APPLY_DOCTOR_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                       double  je=0;

                        string  brlb=dt_SQD.Rows[0]["PATIENT_TYPE"].ToString().Trim();
                        if (brlb == "门诊" || brlb=="住院")
                           je= get_je(odbcsql, brlb, dt_SQD.Rows[0]["APPLY_ID"].ToString().Trim(), debug);
                        xml = xml + "收费=" + (char)34 + je.ToString() + (char)34 + " ";
                        //----------------------------------------------------------
                        ///////////////////////////
                        string BBLB_XML = "";
                        if (tqbblb == "1")
                        {
                            string sqlstr_mx = "select *  from dbo.V_pa_apply_mx   WHERE  APPLY_ID= '" + sqxh.Trim() + "'";
                            DataTable dt_SQD_MX = new DataTable();
                            string exp_db_mx = "";
                            dt_SQD_MX = db.Sql_DataAdapter(odbcsql, sqlstr_mx, ref exp_db_mx);
                            if (exp_db_mx.Trim() != "")
                                MessageBox.Show("连接HIS数据库异常：" + exp_db_mx);
                            else
                            {
                                if (dt_SQD_MX == null)
                                {
                                    MessageBox.Show("未查询到此条码号记录！");
                                }
                                else
                                {

                                    if (dt_SQD_MX.Rows.Count <= 0)
                                    {
                                        MessageBox.Show("未查询到此申请序号记录！");
                                    }
                                    else
                                    {
                                        BBLB_XML = "<BBLB>";
                                        try
                                        {
                                            for (int x = 0; x < dt_SQD_MX.Rows.Count; x++)
                                            {
                                                try
                                                {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_SQD_MX.Rows[x]["BBXH"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_SQD_MX.Rows[x]["APPLY_BARCODE"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD_MX.Rows[x]["BBMC"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD_MX.Rows[x]["BWMC"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD_MX.Rows[x]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD_MX.Rows[x]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
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
                            }
                        }
                        xml = xml + "标本名称=" + (char)34 + dt_SQD.Rows[0]["BBMC"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + dt_SQD.Rows[0]["ORDER_NAME"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "备用2=" + (char)34 + DateTime.Parse(dt_SQD.Rows[0]["csrq"].ToString().Trim()).ToString("yyyy-MM-dd")+ (char)34 + " ";
                        }
                        catch(Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "备用2=" + (char)34 +""+ (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + dt_SQD.Rows[0]["FEE_CLASS"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_SQD.Rows[0]["DIAGNOSIS"].ToString().Trim() + "]]></临床诊断>";
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
         return "0";
        }
        private static double get_je(string odbcsql, string brlb,string sqdh, string debug)
        {
              SqlDB_ZGQ db = new SqlDB_ZGQ();
                SqlParameter[] Parameters = new SqlParameter[2];

                for (int j = 0; j < Parameters.Length; j++)
                {
                    Parameters[j] = new SqlParameter();
                }
                Parameters[0].ParameterName = "sqdh";
                Parameters[0].SqlDbType = SqlDbType.VarChar;
                Parameters[0].Direction = ParameterDirection.Input;
                Parameters[0].Value = sqdh.Trim();

                Parameters[1].ParameterName = "return";
                Parameters[1].SqlDbType = SqlDbType.Int;
                Parameters[1].Direction = ParameterDirection.Output;


                string except = "";
            DataTable  dt=new DataTable();
            if (brlb.Trim()=="住院")
                  dt = db.Sql_DataAdapter(odbcsql, "p_pa_zysfxx", ref Parameters, CommandType.StoredProcedure, ref except);
            else
                  dt = db.Sql_DataAdapter(odbcsql, "p_pa_mzsfxx", ref Parameters,CommandType.StoredProcedure, ref except);
      
                  
               if(dt==null)
               {
                     if(debug=="1")
                         MessageBox.Show("费用获取失败:获取数据失败\r\n"  +except);
                   return 0;
               }
            if(dt.Rows.Count<=0)
            {
                  if(debug=="1")
                      MessageBox.Show("费用获取失败:返回行数为0\r\n" + except);
                 return 0;
            }
                
                    try
                    {
                        if (int.Parse(Parameters[1].Value.ToString()) < 0)
                        {
                              if(debug=="1")
                            MessageBox.Show("费用获取失败:" + Parameters[4].Value);
                           log.WriteMyLog("费用获取失败:" + Parameters[4].Value);
                             return 0;
                        }
                       double  je=0;
                       for(int x=0;x<dt.Rows.Count;x++)
                       {
                           je=double.Parse(dt.Rows[x]["je"].ToString().Trim())+je;
                       }

                        return  je;

                    }
                    catch (Exception ee1)
                    {
                        if(debug=="1")
                            MessageBox.Show("费用获取异常:" + ee1.Message);
                        log.WriteMyLog("费用获取异常:" + ee1.Message);
                         return 0;
                    }
                }

    }     
}
