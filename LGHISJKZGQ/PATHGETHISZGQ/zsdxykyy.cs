using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.Odbc;
using dbbase;
using System.Xml;
using System.IO;

namespace LGHISJKZGQ
{
    class zsdxykyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            string exp = "";

            if (Sslbx != "")
            {
                string hissql = f.ReadString(Sslbx, "hissql", "").Replace("\0", "").Trim();
                string ptjk = f.ReadString("zgqjk", "ptjk", "1").Replace("\0", "").Trim();

                if (Sslbx == "取消登记")
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    int x = aa.ExecuteSQL("update T_SQD set F_SQDZT='取消登记' where F_sqxh='" + Ssbz.Trim() + "'");
                    if (x > 0)
                        MessageBox.Show("取消登记成功");
                    else
                        MessageBox.Show("取消登记失败");
                    return "0";
                }
                if (Sslbx == "取消申请单")
                {
                    string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                     ZtToHis(Sslbx, Ssbz.Trim(), yhmc, yhbh, debug);
                     dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                     DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + Ssbz.Trim() + "'", "t_sqd");
                    if (dt_sqd.Rows.Count < 1)
                    {
                        MessageBox.Show("平台撤销申请单失败:病理数据库表中无此申请单记录");
                        return  "0";
                    }
                     string errmsg = "";
                     string message = ZtMsg(dt_sqd, ref errmsg, yhmc, yhbh);
                     if (message == "")
                     {
                         return "0";
                     }

                     if (debug == "1")
                         log.WriteMyLog("取消申请单MQ状态回传：" + message);
                     string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
                     string wsurl = f.ReadString(Sslbx, "wsurl", "http://172.168.35.249/BLToMQWebSrv/FZMQService.asmx").Replace("\0", "").Trim();
                     try
                     {
                         BLToFZMQWS.Service fzmq = new BLToFZMQWS.Service();
                         if (wsurl != "")
                             fzmq.Url = wsurl;

                         string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS004.LQ", "BS004", jzlb, "0", "S009", "45541605-3", "70500", "0");

                         if (rtnmsg.Contains("ERR"))
                         {
                           MessageBox.Show("平台取消申请单失败：" + rtnmsg);
                             return "0";
                         }

                     }
                     catch (Exception ee2)
                     {
                         MessageBox.Show("平台取消申请单异常:" + ee2.Message);
                         return "0";
                     }  
                    int x = aa.ExecuteSQL("delete  from T_SQD  where F_sqxh='" + Ssbz.Trim() + "'");
                    return "0";
                }

                if (ptjk != "1")
                {
          
                    return GetHisXX(Sslbx, Ssbz, debug);
                }

                if (Sslbx == "住院号")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,'' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_sqks as 送检科室,F_SQYS AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZH as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_CSRQ as 出生日期,F_BY1 as 备用1,F_SQYSBM as 备用2,F_SQRQ as 申请日期 from [pathnet].[dbo].[T_SQD] WHERE  F_ZYH='f_sbh' and F_sqdzt!='已登记' order by F_ID desc";
                }
                if (Sslbx == "门诊流水号")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,'' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_sqks as 送检科室,F_SQYS AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZH as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_CSRQ as 出生日期,F_BY1 as 备用1,F_SQYSBM as 备用2,F_SQRQ as 申请日期 from [pathnet].[dbo].[T_SQD] WHERE  F_JZLSH='f_sbh' and F_sqdzt!='已登记' order by F_ID desc";
                }
                if (Sslbx == "诊疗卡")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,'' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_sqks as 送检科室,F_SQYS AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZH as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_CSRQ as 出生日期,F_BY1 as 备用1,F_SQYSBM as 备用2,F_SQRQ as 申请日期 from [pathnet].[dbo].[T_SQD] WHERE  F_MZH='f_sbh' and F_sqdzt!='已登记' order by F_ID desc";
                }
                if (Sslbx == "申请单号")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,'' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_sqks as 送检科室,F_SQYS AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZH as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_CSRQ as 出生日期,F_BY1 as 备用1,F_SQYSBM as 备用2,F_SQRQ as 申请日期 from [pathnet].[dbo].[T_SQD] WHERE  F_SQXH='f_sbh' and F_sqdzt!='已登记' order by F_ID desc";
                }
                if (Sslbx == "医嘱号")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,'' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_sqks as 送检科室,F_SQYS AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZH as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_CSRQ as 出生日期,F_BY1 as 备用1,F_SQYSBM as 备用2,F_SQRQ as 申请日期 from [pathnet].[dbo].[T_SQD] WHERE  F_YZH='f_sbh' and F_sqdzt!='已登记' order by F_ID desc";
                }
           

                string Columns = f.ReadString(Sslbx, "Columns", "申请日期,申请序号,就诊ID,姓名,性别,年龄,送检科室,送检医生,病区,床号,医嘱项目");
                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,申请序号,医嘱号,姓名,性别,年龄,送检科室,送检医生,病区,床号,医嘱项目");
                hissql = hissql.Replace("f_sbh", Ssbz.Trim());
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;");
                if (hissql.Trim() == "")
                {
            
                    return GetHisXX(Sslbx, Ssbz, debug);
                }
                string exec = "";
                DataSet ds = new DataSet();
                OdbcDB_ZGQ odbc = new OdbcDB_ZGQ();
                ds = odbc.Odbc_DataAdapter_DataSet(odbcsql, hissql, ref exec);
                if (exec != "")
                {
                    MessageBox.Show(exec);
                    return GetHisXX(Sslbx, Ssbz, debug);
                }
                /////////////////////////////////////////////////////////////////////
                if (ds.Tables[0] == null)
                {
                    MessageBox.Show("获取申请单信息错误：查询数据错误");
                    return GetHisXX(Sslbx, Ssbz, debug);
                }
                if (ds.Tables[0].Rows.Count <= 0)
                {
                
                    //string odbcstr = f.ReadString(Sslbx, "odbc", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();

                    //if (Sslbx == "住院号")
                    //    return GetBrxx(2, Ssbz, 0, 0, "", debug, odbcstr, "0");
                    //if (Sslbx == "门诊流水号")
                    //{
                    //    int mzlsh = 0;
                    //    try
                    //    {
                    //        mzlsh = int.Parse(Ssbz);
                    //    }
                    //    catch
                    //    {
                    //        MessageBox.Show("门诊流水号格式不正确");
                    //        return "0";
                    //    }
                    //    return GetBrxx(1, "", mzlsh, 0, "", debug, odbcstr, "0");
                    //}
                    //if (Sslbx == "诊疗卡")
                    //    return GetBrxx(5, "", 0, 0, Ssbz, debug, odbcstr, "0");

                    //MessageBox.Show("未获取到相关信息");
                    //return "0";
                    return GetHisXX(Sslbx, Ssbz, debug);
                }

       
                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                int count = 0;
                if (ds.Tables[0].Rows.Count > tkts)
                {
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
                        MessageBox.Show("请重新选择申请项目！");
                        return "0";
                    }
                }
                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
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
                    xml = xml + "地址=" + (char)34 +"" + (char)34 + "   ";
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
                    xml = xml + "<临床病史><![CDATA[" + ""+ds.Tables[0].Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                    xml = xml + "/>";
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("获取申请单信息异常：" + e.Message);
                    return "0";
                }
            }
            else
            {
                MessageBox.Show("识别类型不能为空");
                return "0";
            }
        }
                
    public static string GetHisXX(string Sslbx, string Ssbz, string debug)
      {
       
          ////  1-按门诊流水号查询
////2-按住院号查询
////3-按技诊申请时间和申请技诊科室查询
////4-按标本条码查询
////5-按诊疗卡号查询
////6-按申请单号查询

//正式环境IP192.168.171.138  srvhisdb
//测试环境IP 192.168.171.50 svrdb
      
                    string odbcstr = f.ReadString(Sslbx, "odbc_his", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();
                    debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim(); ;
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");

                    if (Sslbx == "住院号")
                    {
                        string rtn = GetSqdxx(2, Ssbz, 0, 0, "", 0, debug, odbcstr, tqbblb);
                        if (rtn == "0")
                            return GetBrxx(2, Ssbz, 0, 0, "", debug, odbcstr, "0");
                          
                        else
                            return rtn;
                    }

                    if (Sslbx == "门诊流水号")
                    {
                        int mzlsh = 0;
                        try
                        {
                            mzlsh = int.Parse(Ssbz);
                        }
                        catch
                        {
                            MessageBox.Show("门诊流水号格式不正确");
                            return "0";
                        }
                        string rtn = GetSqdxx(1, "", mzlsh, 0, "", 0, debug, odbcstr, tqbblb);
                        if (rtn == "0")
                            return GetBrxx(1, "", mzlsh, 0, "", debug, odbcstr, "0");
                        else
                            return rtn;
                    }
                    if (Sslbx == "诊疗卡")
                    {

                        string rtn = GetSqdxx(5, "", 0, 0, Ssbz, 0, debug, odbcstr, tqbblb);
                        if (rtn == "0")
                            return GetBrxx(5, "", 0, 0, Ssbz, debug, odbcstr, "0");
                        else
                            return rtn;
                    }
                    if (Sslbx == "申请单号")
                    {
                        int sqdh = 0;
                        try
                        {
                            sqdh = int.Parse(Ssbz);
                        }
                        catch
                        {
                            MessageBox.Show("申请单号格式不正确");
                            return "0";
                        }

                        string rtn = GetSqdxx(6, "", 0, 0, "", sqdh, debug, odbcstr, tqbblb);
                        return rtn;
                    }
                    if (Sslbx == "取消HIS申请单")
                    {
                        #region  取消申请单
                        string yhgh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim(); ;
                        int sqdh = 0;
                        try
                        {

                            sqdh = int.Parse(Ssbz);
                        }
                        catch
                        {
                            MessageBox.Show("取消失败：申请单格式不正确");
                            return "0";
                        }

                        if (sqdh == 0)
                        {
                            MessageBox.Show("取消失败：申请单格式不正确");
                            return "0";
                        }
                        SqlParameter[] sqlPt = new SqlParameter[3];
                        for (int j = 0; j < sqlPt.Length; j++)
                        {
                            sqlPt[j] = new SqlParameter();
                        }
                        //申请单ID
                        sqlPt[0].ParameterName = "In_FunctionRequestID";
                        sqlPt[0].SqlDbType = SqlDbType.Int;
                        sqlPt[0].Direction = ParameterDirection.Input;
                        sqlPt[0].Value = sqdh;

                        //操作员工工号
                        sqlPt[1].ParameterName = "In_OperatorEmployeeNo";
                        sqlPt[1].SqlDbType = SqlDbType.NVarChar;
                        sqlPt[1].Direction = ParameterDirection.Input;
                        sqlPt[1].Size = 10;
                        sqlPt[1].Value = yhgh;

                        //取消标志
                        sqlPt[2].ParameterName = "Out_StatusFlag";
                        sqlPt[2].SqlDbType = SqlDbType.TinyInt;
                        sqlPt[2].Direction = ParameterDirection.Output;
                        sqlPt[2].Value = 0;


                        string err_msg = "";
                        SqlDB_ZGQ db = new SqlDB_ZGQ();
                        int x = db.Sql_ExecuteNonQuery(odbcstr, "pCancelFunctionRequest", ref sqlPt, CommandType.StoredProcedure, ref err_msg);

                        if (int.Parse(sqlPt[2].Value.ToString()) == 1)
                            MessageBox.Show("取消成功");
                        else
                            MessageBox.Show(sqlPt[2].Value.ToString() + "：取消失败");
                        return "0";
                        #endregion
                    }
                    else
                        MessageBox.Show("无此" + Sslbx);
            return "0";
        }

        private static string GetBrxx(int Flag, string zyh, int mzlsh, int bbtmh, string zlkh,  string debug, string odbcstr, string tqbblb)
        {
         

            SqlParameter[] sqlPt = new SqlParameter[6];
            for (int j = 0; j < sqlPt.Length; j++)
            {
                sqlPt[j] = new SqlParameter();
            }
            //标记
            sqlPt[0].ParameterName = "In_Flag";
            sqlPt[0].SqlDbType = SqlDbType.TinyInt;
            sqlPt[0].Direction = ParameterDirection.Input;
            sqlPt[0].Value = Flag;

            //住院号
            sqlPt[1].ParameterName = "In_IPSeqNoText";
            sqlPt[1].SqlDbType = SqlDbType.NVarChar;
            sqlPt[1].Direction = ParameterDirection.Input;
            sqlPt[1].Size = 14;
            sqlPt[1].Value = zyh;

            //挂号日期
            sqlPt[2].ParameterName = "In_RegisterDate";
            sqlPt[2].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[2].Direction = ParameterDirection.Input;
            sqlPt[2].Value = DateTime.Today;
            //门诊流水号
            sqlPt[3].ParameterName = "In_SeqNo";
            sqlPt[3].SqlDbType = SqlDbType.Int;
            sqlPt[3].Direction = ParameterDirection.Input;
            sqlPt[3].Value = mzlsh;
            //标本条码id
            sqlPt[4].ParameterName = "In_FunctionSampleLabelID";
            sqlPt[4].SqlDbType = SqlDbType.Int;
            sqlPt[4].Direction = ParameterDirection.Input;
            sqlPt[4].Value = bbtmh;

            //诊疗卡号
            sqlPt[5].ParameterName = "In_PatientCardNo";
            sqlPt[5].SqlDbType = SqlDbType.NVarChar;
            sqlPt[5].Direction = ParameterDirection.Input;
            sqlPt[5].Size = 30;
            sqlPt[5].Value = zlkh;



            DataTable dt_brxx = new DataTable();
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            string err_msg = "";

           
            dt_brxx = db.Sql_DataAdapter(odbcstr, "pGetPatientInfoForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
 
            if (dt_brxx == null)
            {
                MessageBox.Show(err_msg); return "0";
            }
            if (dt_brxx.Rows.Count <=0)
            {
                MessageBox.Show("未查询到相关记录"); return "0";
            }
                   

            //-返回xml----------------------------------------------------
            try
            {


                string brlb = "1";// dt_brxx.Rows[0]["SourceFlag"].ToString().Trim();
                if (brlb == "0")
                    brlb = "门诊";
                if (brlb == "1")
                    brlb = "住院";
                if (brlb == "2")
                    brlb = "体检";

                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "病人编号=" + (char)34 + dt_brxx.Rows[0]["PatientID"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------0
                try
                {
                    if (brlb == "门诊")
                        xml = xml + "就诊ID=" + (char)34 + dt_brxx.Rows[0]["SeqNo"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "住院")
                        xml = xml + "就诊ID=" + (char)34 + dt_brxx.Rows[0]["InPatientID"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "体检")
                        xml = xml + "就诊ID=" + (char)34 + dt_brxx.Rows[0]["RegisterID"].ToString().Trim() + (char)34 + " ";
                   
                }
                catch (Exception ee)
                {
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "门诊号=" + (char)34 + dt_brxx.Rows[0]["PatientCardNo"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {

                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "住院号=" + (char)34 + dt_brxx.Rows[0]["IPSeqNoText"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
     
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "姓名=" + (char)34 + dt_brxx.Rows[0]["PatientName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
   
                    xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                string xb = dt_brxx.Rows[0]["PatientSex"].ToString().Trim();
                xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                //----------------------------------------------------------
                try
                {
                    xml = xml + "年龄=" + (char)34 + ZGQClass.CsrqToAge(dt_brxx.Rows[0]["PatientBirthDay"].ToString().Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "婚姻=" + (char)34 + dt_brxx.Rows[0]["Marriage"].ToString().Trim() + (char)34 + " ";

                //----------------------------------------------------------
                try
                {

                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "电话=" + (char)34 + dt_brxx.Rows[0]["Phone"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                 
                    xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "病区=" + (char)34 + dt_brxx.Rows[0]["IPDepartmentName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                  
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "床号=" + (char)34 + dt_brxx.Rows[0]["SickBedNo"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                 
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "身份证号=" + (char)34 + dt_brxx.Rows[0]["IdentityCardNo"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                   
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "民族=" + (char)34 + dt_brxx.Rows[0]["RaceDesc"].ToString().Trim() + (char)34 + " ";

                //----------------------------------------------------------
                xml = xml + "职业=" + (char)34 + dt_brxx.Rows[0]["ProfessionDesc"].ToString().Trim() + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "送检科室=" + (char)34 + dt_brxx.Rows[0]["DepartmentName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                   
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                 
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------

                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";

                //----------------------------------------------------------
                xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "费别=" + (char)34 + dt_brxx.Rows[0]["PatientTypeListName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                   
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                catch (Exception ee)
                {
                  
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                catch (Exception ee)
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                xml = xml + "</LOGENE>";
                return xml;
            }
            catch (Exception ee1)
            {

                MessageBox.Show("提取信息出错;" + ee1.Message);
                return "0";
            }
         
        }

        private static string GetSqdxx(int Flag, string  zyh, int mzlsh, int bbtmh,string  zlkh,int sqdh, string debug, string odbcstr, string tqbblb)
        {

            SqlParameter[] sqlPt = new SqlParameter[10];
            for (int j = 0; j < sqlPt.Length; j++)
            {
                sqlPt[j] = new SqlParameter();
            }
            //标记
            sqlPt[0].ParameterName = "In_Flag";
            sqlPt[0].SqlDbType = SqlDbType.TinyInt;
            sqlPt[0].Direction = ParameterDirection.Input;
            sqlPt[0].Value = Flag;

            //开始日期
            sqlPt[1].ParameterName = "In_StartDate";
            sqlPt[1].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[1].Direction = ParameterDirection.Input;
            sqlPt[1].Value = DateTime.Today.AddDays(-10);

            //结束日期
            sqlPt[2].ParameterName = "In_EndDate";
            sqlPt[2].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[2].Direction = ParameterDirection.Input;
            sqlPt[2].Value = DateTime.Today;
            //申请科室编码串
            sqlPt[3].ParameterName = "In_RequestDepartmentNos";
            sqlPt[3].SqlDbType = SqlDbType.NVarChar;
            sqlPt[3].Direction = ParameterDirection.Input;
            sqlPt[3].Size = 10;
            sqlPt[3].Value = "70500";
            //住院号
            sqlPt[4].ParameterName = "In_IPSeqNoText";
            sqlPt[4].SqlDbType = SqlDbType.NVarChar;
            sqlPt[4].Direction = ParameterDirection.Input;
            sqlPt[3].Size = 14;
            sqlPt[4].Value = zyh; 
            //挂号日期
            sqlPt[5].ParameterName = "In_RegisterDate";
            sqlPt[5].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[5].Direction = ParameterDirection.Input;
            sqlPt[5].Value = DateTime.Today; 
            //门诊流水号
            sqlPt[6].ParameterName = "In_SeqNo";
            sqlPt[6].SqlDbType = SqlDbType.Int;
            sqlPt[6].Direction = ParameterDirection.Input;
            sqlPt[6].Value = mzlsh; 
            //标本条码ID
            sqlPt[7].ParameterName = "In_FunctionSampleLabelID";
            sqlPt[7].SqlDbType = SqlDbType.Int;
            sqlPt[7].Direction = ParameterDirection.Input;
            sqlPt[7].Value = bbtmh;
            //诊疗卡号
            sqlPt[8].ParameterName = "In_PatientCardNo";
            sqlPt[8].SqlDbType = SqlDbType.NVarChar;
            sqlPt[8].Direction = ParameterDirection.Input;
            sqlPt[8].Size = 30;
            sqlPt[8].Value = zlkh;
            //申请单号
            sqlPt[9].ParameterName = "In_FunctionRequestID";
            sqlPt[9].SqlDbType = SqlDbType.Int;
            sqlPt[9].Direction = ParameterDirection.Input;
            sqlPt[9].Value = sqdh;

            DataTable dt_sqdxx = new DataTable();
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            string err_msg = "";
            dt_sqdxx = db.Sql_DataAdapter(odbcstr, "pGetFunctionRequestForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);

            if (dt_sqdxx == null)
            {
                MessageBox.Show(err_msg); return "0";
            }
            if (dt_sqdxx.Rows.Count < 1)
            {
                MessageBox.Show("未查询到相关申请单记录");
                return "0";
            }

            int xh = 0;
            string yzxm = "";
            if (dt_sqdxx.Rows.Count >= 1)
            {

                string Columns = "RequestDateTime,FunctionRequestID,PatientName,PatientSex,IPSeqNoText,SickBedNo,RequestDepartmentName,RequestEmployeeName";//显示的项目对应字段
                string ColumnsName = "申请日期,申请单号,姓名,性别,住院号,床号,送检科室,送检医生";//显示的项目名称
                string xsys = "1"; //选择条件的项目
                DataColumn dc0 = new DataColumn("序号");
                dt_sqdxx.Columns.Add(dc0);
                for (int x = 0; x < dt_sqdxx.Rows.Count; x++)
                {
                    dt_sqdxx.Rows[x][dt_sqdxx.Columns.Count - 1] = x;
                }
                if (Columns.Trim() != "")
                    Columns = "序号," + Columns;
                if (ColumnsName.Trim() != "")
                    ColumnsName = "序号," + ColumnsName;

                ZSYKYY_SELECT_YZMX yc = new ZSYKYY_SELECT_YZMX(dt_sqdxx, Columns, ColumnsName, xsys, odbcstr);
                yc.ShowDialog();
                if (yc.DialogResult == DialogResult.Yes)
                {
                    string rtn2 = yc.F_XH;
                    yzxm = yc.F_yzxm;
                    if (rtn2.Trim() == "")
                    {
                        MessageBox.Show("未选择申请单！");
                        return "0";
                    }
                    try
                    {
                        xh = int.Parse(rtn2);
                    }
                    catch
                    {
                        MessageBox.Show("请重新选择申请单！");
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("请重新选择申请单！");
                    return "0";
                }
            }
            //  string yzxm = "";
            //try
            //{
            //    string sqxh = "";
            //    //获取申请单明细
            //    SqlParameter[] sqlPt_sqdmx = new SqlParameter[1];
            //    sqlPt[0] = new SqlParameter();
            //    //标记
            //    sqlPt[0].ParameterName = "In_FunctionRequestIDs";
            //    sqlPt[0].SqlDbType = SqlDbType.NVarChar;
            //    sqlPt[0].Size = 200;
            //    sqlPt[0].Direction = ParameterDirection.Input;
            //    sqlPt[0].Value = dt_sqdxx.Rows[xh]["FunctionRequestID"].ToString().Trim();

                
            //    DataTable dt_sqdmx = new DataTable();

            //    dt_sqdmx = db.Sql_DataAdapter(odbcstr, "pGetFunctionRequestListForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
            //    if (dt_sqdmx == null || dt_sqdmx.Rows.Count < 1)
            //    {

            //    }
            //    else
            //    {
            //        yzxm = dt_sqdmx.Rows[0]["ItemID"].ToString().Trim() + "^" + dt_sqdmx.Rows[0]["ItemName"].ToString().Trim();
            //    }
            //}
            //catch
            //{
            //}

                //-返回xml----------------------------------------------------
                try
                {

                    string brlb = "1";// dt_sqdxx.Rows[xh]["SourceFlag"].ToString().Trim();
                    if (brlb == "0")
                        brlb = "门诊";
                    if (brlb == "1")
                        brlb = "住院";
                    if (brlb == "2")
                        brlb = "体检";


                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    try
                    {
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[xh]["PatientID"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        if(brlb=="门诊")
                            xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["SeqNo"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "住院")
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["InPatientID"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "体检")
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["RegisterID"].ToString().Trim() + (char)34 + " ";
                   
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[xh]["FunctionRequestID"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {

                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[xh]["PatientCardNo"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    try
                    {
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[xh]["IPSeqNoText"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    try
                    {
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[xh]["PatientName"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                     
                        xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------


                    xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[xh]["PatientSex"].ToString().Trim() + (char)34 + " ";

                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "年龄=" + (char)34 + ZGQClass.CsrqToAge(dt_sqdxx.Rows[xh]["PatientBirthDay"].ToString().Trim()) + (char)34 + " ";
                    }
                    catch
                    {
                        xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    xml = xml + "婚姻=" + (char)34 + dt_sqdxx.Rows[xh]["Marriage"].ToString().Trim() + (char)34 + " ";

                    //----------------------------------------------------------
                    try
                    {

                        xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                  
                        xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[xh]["Phone"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                    
                        xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[xh]["IPDepartmentName"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[xh]["SickBedNo"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                     
                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[xh]["IdentityCardNo"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                   
                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[xh]["RaceDesc"].ToString().Trim() + (char)34 + " ";

                    //----------------------------------------------------------
                    xml = xml + "职业=" + (char)34 + dt_sqdxx.Rows[xh]["ProfessionDesc"].ToString().Trim() + (char)34 + " ";
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[xh]["RequestDepartmentName"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                    }
                    //----------------------------------------------------------
                    string ViceDirectorEmployeeID="";
                     string RequestEmployeeName="";
                    try{
                        ViceDirectorEmployeeID = dt_sqdxx.Rows[xh]["ViceDirectorEmployeeName"].ToString().Trim();
                    }
                    catch
                    {
                    }

                    try
                    {
                        RequestEmployeeName = dt_sqdxx.Rows[xh]["RequestEmployeeName"].ToString().Trim();
                    }
                    catch
                    {
                    }
                    if (ViceDirectorEmployeeID=="")
                        xml = xml + "送检医生=" + (char)34 + RequestEmployeeName + (char)34 + " ";
                    else
                        xml = xml + "送检医生=" + (char)34 + ViceDirectorEmployeeID + "/" + RequestEmployeeName + (char)34 + " ";
                    //----------------------------------------------------------

                    xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                    //----------------------------------------------------------
                    xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                    //----------------------------------------------------------

                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";

                    //----------------------------------------------------------
                    xml = xml + "医嘱项目=" + (char)34 + yzxm + (char)34 + " ";
                    //----------------------------------------------------------
                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                    //----------------------------------------------------------
                    xml = xml + "备用2=" + (char)34 + dt_sqdxx.Rows[xh]["RequestEmployeeNo"].ToString().Trim() + (char)34 + " ";
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                    xml = xml + "/>";
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                    }
                    catch (Exception ee)
                    {
                     
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[xh]["diseaseName"].ToString().Trim() + "]]></临床诊断>";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                    }
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception ee1)
                {

                    MessageBox.Show("提取信息出错;" + ee1.Message);
                    return "0";
                }
        }

        public static string ZtMsg(DataTable dt_sqdxx, ref string errmsg, string yhmc, string yhbh)
        {
            string bgztbm = "160.009";
            string bgztstr = "取消申请单";

            try
            {
              string   xbrlb = dt_sqdxx.Rows[0]["F_jzlb"].ToString();
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3  ../../Schemas/POOR_IN200901UV23.xsd\">";

                xml = xml + "<!-- 消息ID -->";
                xml = xml + "<id extension=\"BS004\" />";
                xml = xml + "<!-- 消息创建时间 -->";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<!-- 交互ID -->";
                xml = xml + "<interactionId extension=\"POOR_IN200901UV23\" />";
                xml = xml + "<!--消息用途: P(Production); D(Debugging); T(Training) -->";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<!-- 消息处理模式: A(Archive); I(Initial load); R(Restore from archive); T(Current  processing) -->";
                xml = xml + "<processingModeCode code=\"R\" />";
                xml = xml + "<!-- 消息应答: AL(Always); ER(Error/reject only); NE(Never) -->";
                xml = xml + "<acceptAckCode code=\"NE\" />";
                xml = xml + "<!-- 接受者 -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";

                xml = xml + "<!-- 接受者ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<!-- 发送者 -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 发送者ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"S009\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<!-- 封装的消息内容(按Excel填写) -->";
                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<!-- 消息交互类型 @code: 新增 :new 修改:update -->";
                xml = xml + "<code code=\"update\"></code>";
                xml = xml + "<subject typeCode=\"SUBJ\" xsi:nil=\"false\">";
                xml = xml + "<placerGroup>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<code></code>";
                xml = xml + "<!-- 检验申请单状态 必须项未使用 -->";
                xml = xml + "<statusCode code=\"active\"></statusCode>";
                xml = xml + "<!-- 患者信息 -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<id>";
                xml = xml + "<!--域ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.2\" extension=\"" + dt_sqdxx.Rows[0]["F_yid"].ToString() + "\" />";
                xml = xml + "<!-- 患者ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- 就诊号 -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_jzh"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"  determinerCode=\"INSTANCE\">";
                xml = xml + "<!--病人科室编码-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ksbm"].ToString() + "\" root=\"1.2.156.112678.1.1.1\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--病人科室名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_ksmc"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--医疗机构代码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yybm"].ToString() + "\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--医疗机构名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item><part value=\"" + dt_sqdxx.Rows[0]["F_yymc"].ToString() + "\" /></item>";
                xml = xml + "</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</providerOrganization>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "<!-- 操作人 -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<time>";
                xml = xml + "<!-- 操作日期 -->";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</time>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 操作人编码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + yhbh + "\" root=\"1.2.156.112678.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 操作人姓名 必须项已使用 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + yhmc + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<!--执行科室 -->";
                xml = xml + "<location typeCode=\"LOC\" xsi:nil=\"false\">";
                xml = xml + "<!--必须项未使用 -->";
                xml = xml + "<time />";
                xml = xml + "<!--就诊机构/科室 -->";
                xml = xml + "<serviceDeliveryLocation classCode=\"SDLOC\">";
                xml = xml + "<serviceProviderOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--执行科室编码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_zxksbm"].ToString() + "\" root=\"1.21.156.112649.1.1.1\" />";
                xml = xml + "</id>";
                xml = xml + "<!--执行科室名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_zxks"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</serviceDeliveryLocation>";
                xml = xml + "</location>";

                xml = xml + "<!-- 1..n可循环  医嘱状态信息 -->";
                xml = xml + "<component2>";
                xml = xml + "<!--医嘱序号-->";
                xml = xml + "<sequenceNumber value=\"1\"/>";
                xml = xml + "<observationRequest classCode=\"OBS\">";
                xml = xml + "<!-- 必须项已使用 -->";
                xml = xml + "<id>";
                xml = xml + "<!-- 医嘱号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yzh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.22\"/>";
                xml = xml + "<!-- 申请单号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_sqxh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.21\"/>";
                xml = xml + "<!-- 报告号 -->";
                xml = xml + "<item extension=\"" + "" + "\" root=\"1.2.156.112678.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112678.1.2.1.30\"/>";
                xml = xml + "</id>";

                xml = xml + "<!-- 医嘱类别编码/医嘱类别名称 - 针剂药品, 材料类, 治疗类, 片剂药品, 化验类 -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.27\">";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_YZLXMC"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";

                xml = xml + "<!-- 标本信息 -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                string[] bbtmh = dt_sqdxx.Rows[0]["F_bbtmh"].ToString().Trim().Split('#');
                foreach (string bbtm in bbtmh)
                {
                    xml = xml + "<specimen classCode=\"SPEC\">";
                    xml = xml + "<!--标本条码号 必须项已使用 -->";
                    xml = xml + "<id extension=\"" + bbtm + "\" />";
                    xml = xml + "<!--必须项目未使用 -->";
                    xml = xml + "<code />";
                    xml = xml + "<subjectOf1 typeCode=\"SBJ\" contextControlCode=\"OP\">";
                    xml = xml + "<specimenProcessStep moodCode=\"EVN\" classCode=\"SPECCOLLECT\">";
                    xml = xml + "<!-- 采集日期 -->";
                    xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                    xml = xml + "<any value=\"" + Convert.ToDateTime(DateTime.Now.ToString("yyyyMMddHHmmss")) + "\"></any>";
              
                    xml = xml + "</effectiveTime>";
                    xml = xml + "<performer typeCode=\"PRF\">";
                    xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                    xml = xml + "<!-- 采集人Id -->";
                    xml = xml + "<id>";
                    xml = xml + "<item extension=\"" +yhbh + "\" root=\"1.2.156.112678.1.1.2\"></item>";
                    xml = xml + "</id>";
                    xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                    xml = xml + "<!-- 采集人姓名 -->";
                    xml = xml + "<name xsi:type=\"BAG_EN\">";
                    xml = xml + "<item>";
                    xml = xml + "<part value=\"" + yhmc + "\" />";
                    xml = xml + "</item>";
                    xml = xml + "</name>";
                    xml = xml + "</assignedPerson>";
                    xml = xml + "</assignedEntity>";
                    xml = xml + "</performer>";
                    xml = xml + "</specimenProcessStep>";
                    xml = xml + "</subjectOf1>";
                    xml = xml + "</specimen>";

                }
                xml = xml + "</specimen>";

                xml = xml + "<!-- 原因 -->";
                xml = xml + "<reason contextConductionInd=\"true\">";
                xml = xml + "<observation moodCode=\"EVN\" classCode=\"OBS\">";
                xml = xml + "<!-- 必须项 未使用-->";
                xml = xml + "<code></code>";
                xml = xml + "<value xsi:type=\"ST\" value=\"\"/>";
                xml = xml + "</observation>";
                xml = xml + "</reason>";
                xml = xml + "<!-- 医嘱执行状态 -->";
                xml = xml + "<component1 contextConductionInd=\"true\">";
                xml = xml + "<processStep classCode=\"PROC\">";
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112678.1.1.93\">";
                xml = xml + "<!--医嘱执行状态名称 -->";
                xml = xml + "<displayName value=\"" + bgztstr + "\" />";
                xml = xml + "</code>";
                xml = xml + "</processStep>";
                xml = xml + "</component1>";
                xml = xml + "</observationRequest>";
                xml = xml + "</component2>";

                xml = xml + "<!--就诊 -->";
                xml = xml + "<componentOf1 contextConductionInd=\"false\" xsi:nil=\"false\" typeCode=\"COMP\">";
                xml = xml + "<!--就诊 -->";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<id>";
                xml = xml + "<!-- 就诊次数 必须项已使用 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112678.1.2.1.7\" />";
                xml = xml + "<!-- 就诊流水号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112678.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--就诊类别编码-->";
                xml = xml + "<code codeSystem=\"1.2.156.112678.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
                xml = xml + "<!-- 就诊类别名称 -->";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_BRLB"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!--必须项未使用 -->";
                xml = xml + "<statusCode code=\"Active\" />";
                xml = xml + "<!--病人 必须项未使用 -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\" />";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf1>";
                xml = xml + "</placerGroup>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</POOR_IN200901UV>";

                return FormatXml(xml);

            }
            catch (Exception ex2)
            {
              MessageBox.Show("平台撤销申请单XML生成异常:" + ex2.Message);
                return "";
            }
        }

        public static void ZtToHis(string Sslbx, string sqxh, string yhmc, string yhgh, string debug)
        {
            try
            {
                int.Parse(sqxh);
            }
            catch
            {
                MessageBox.Show("申请序号格式不正确");
                return;
            }
            string odbc2his = f.ReadString("Sslbx", "odbc2his", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();
            try
            {
                SqlParameter[] sqlPt = new SqlParameter[3];
                for (int j = 0; j < sqlPt.Length; j++)
                {
                    sqlPt[j] = new SqlParameter();
                }
                //申请单ID
                sqlPt[0].ParameterName = "In_FunctionRequestID";
                sqlPt[0].SqlDbType = SqlDbType.Int;
                sqlPt[0].Direction = ParameterDirection.Input;
                sqlPt[0].Value = int.Parse(sqxh);

                //操作员工工号
                sqlPt[1].ParameterName = "In_OperatorEmployeeNo";
                sqlPt[1].SqlDbType = SqlDbType.NVarChar;
                sqlPt[1].Direction = ParameterDirection.Input;
                sqlPt[1].Size = 10;
                sqlPt[1].Value = yhgh;
                //取消标志
                sqlPt[2].ParameterName = "Out_StatusFlag";
                sqlPt[2].SqlDbType = SqlDbType.TinyInt;
                sqlPt[2].Direction = ParameterDirection.Output;
                sqlPt[2].Value = 0;

                string err_msg = "";
                SqlDB_ZGQ db = new SqlDB_ZGQ();
                db.Sql_ExecuteNonQuery(odbc2his, "pCancelFounctionRequest", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
                if (int.Parse(sqlPt[2].Value.ToString()) == 1)
                {
                  MessageBox.Show("HIS:取消申请单成功");
                }
                else
                {
                    MessageBox.Show("HIS:取消申请单失败:" + sqlPt[2].Value.ToString());
                }
            }
            catch (Exception ee1)
            {
                MessageBox.Show("HIS:取消申请单异常:" + ee1.Message);
            }
        }
        private static string FormatXml(string sUnformattedXml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }
     

    }
}
