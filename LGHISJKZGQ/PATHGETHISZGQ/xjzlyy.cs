using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;

namespace LGHISJKZGQ
{
    class xjzlyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {

            debug = f.ReadString(Sslbx, "debug", "");
        
          string  ptjk = f.ReadString(Sslbx, "ptjk", "1");
            if (Sslbx != "")
            {
                  string rtnXml = "0";
                  if (Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "门诊ID号" || Sslbx == "住院ID号" ||Sslbx == "申请单号"||Sslbx == "ID号")
                {
                    if (ptjk=="1")
                       rtnXml = GetSQD(Sslbx, Ssbz.Trim(), debug);

                    if (rtnXml == "0")
                    {
                        if (Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "门诊ID号" || Sslbx == "住院ID号")
                            return GetBrxx(Sslbx, Ssbz, debug);
                        else
                        {
                            MessageBox.Show("未查询到数据信息");
                            return "0";
                        }
                   
                    }
                    else
                        return rtnXml;
                }


                if (Sslbx == "取消登记")
                {
                    #region
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqd = new DataTable();
                    dt_sqd = aa.GetDataTable("select *  from  [pathnet].[dbo].[T_SQD]  where F_sqxh='" + Ssbz + "'", "sqd");
                    if (dt_sqd == null)
                    {
                        MessageBox.Show("连接数据库失败"); return "0";
                    }
                    if (dt_sqd.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此申请单记录,无需取消登记"); return "0";
                    }
                   
                        aa.ExecuteSQL("update T_sqd set f_sqdzt='' where f_sqxh='" + Ssbz + "' ");
                        MessageBox.Show("取消登记成功");

                    return "0";
                    #endregion
                }

                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");
                    return "0";
                }
            }
            else
                MessageBox.Show("识别类型不能为空");
            return "0";


        }

        //HIS 申请单视图
        private static string GetSQD(string Sslbx, string Ssbz, string debug)
        {

            string msg = f.ReadString(Sslbx, "msg", "");

            DataTable dt_sqd = new DataTable();
            if (Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "门诊ID号" || Sslbx == "住院ID号" || Sslbx == "ID号" || Sslbx == "申请单号" || Sslbx == "单据号")
            {
                string sql = "";
                if (Sslbx == "门诊号")
                    sql =" and  MZH= 'f_sbh'  order by sqsj desc";
                if (Sslbx == "住院号")
                     sql =" and  zyh1= 'f_sbh'  order by sqsj desc";
                if (Sslbx == "门诊ID号" || Sslbx == "住院ID号" || Sslbx == "ID号")
                   sql =" and  brid= 'f_sbh'  order by sqsj desc";
                if (Sslbx == "申请单号" || Sslbx == "单据号")
                    sql =" and  djh= 'f_sbh'  order by sqsj desc";

                string hissql = f.ReadString(Sslbx, "blsql", sql);
                if(hissql!="")
                  sql=hissql;

                if (sql.Trim() == "")
                    return "0";

               sql="select * from  pathnet.v_pathology_information_all  where 1=1 "+ sql +"  order by sqsj desc ";
               ZgqClassPub.DBData.OleDbDB db = new ZgqClassPub.DBData.OleDbDB();

                string Columns = f.ReadString(Sslbx, "Columns", "SQSJ,DJH,ZYH,MZH,XM,XB,NL,YZXM,SQKS,SQYS,BQ,CH");
                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,单据号,住院号,门诊号,姓名,性别,年龄,医嘱项目,送检科室,送检医生,病区,床号");
                 string odbcsql = f.ReadString(Sslbx, "odbcsql", "Provider=MSDAORA;Data Source=ZLYY;User id=pathnet;Password=4s3c2a1p;");
                string errmsg="";
                dt_sqd = db.DataAdapter(odbcsql, sql, ref errmsg);
                if (dt_sqd.Rows.Count == 0)
                {
                    if (msg == "1")
                        MessageBox.Show("未查询到申请单信息" + errmsg);
                    return "0";
                }
                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                int count = 0;
                if (dt_sqd.Rows.Count > tkts)
                {
                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                    DataColumn dc0 = new DataColumn("序号");
                    dt_sqd.Columns.Add(dc0);

                    for (int x = 0; x < dt_sqd.Rows.Count; x++)
                    {
                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                    }

                    if (Columns.Trim() != "")
                        Columns = "序号," + Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "序号," + ColumnsName;
              
                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                    if (yc.ShowDialog() == DialogResult.Yes)
                    {
                        string rtn2 = yc.F_XH;
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("未选择申请项目");
                            return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("请重新选择申请项目");
                            return "0";
                        }
                    }
                    else
                    {
                        MessageBox.Show("未选择申请项目");
                        return "0";
                    }
                }
                try
                {
                 

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["BRID"].ToString() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["ZYH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["DJH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["MZH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["ZYH1"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["XM"].ToString().Trim() + (char)34 + " ";
                    string xb = dt_sqd.Rows[count]["XB"].ToString().Trim();
                    if (xb == "F" || xb == "f")
                        xb = "女";
                    if (xb == "M" || xb == "m")
                        xb = "男";
                    xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                    xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["NL"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["YH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "地址=" + (char)34 + dt_sqd.Rows[count]["DZ"].ToString().Trim() + (char)34 + "   ";
                    xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["DH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["BQ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["CH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["SFZH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "民族=" + (char)34 + dt_sqd.Rows[count]["MZ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["SQKS"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["SQYS"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "标本名称=" + (char)34 + dt_sqd.Rows[count]["BBMC"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["YZXM"].ToString().Trim() + "^" + dt_sqd.Rows[count]["XMDM"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "费别=" + (char)34 + dt_sqd.Rows[count]["FZ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["BRLB"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["SZYJ"].ToString().Trim() +"<回车>"+ dt_sqd.Rows[count]["BSZY"].ToString().Trim() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["LCZD"].ToString().Trim() + "]]></临床诊断>";
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("获取申请单信息异常:" + e.Message);
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

        //T_SQD
        private static string GetSQD111(string Sslbx, string Ssbz, string debug)
        {

            string msg = f.ReadString(Sslbx, "msg", "");
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable dt_sqd = new DataTable();
            if (Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "门诊ID号" || Sslbx == "住院ID号" || Sslbx == "门诊申请号" || Sslbx == "住院申请号" || Sslbx == "申请单号" || Sslbx == "ID号")
            {
                string sql = "";
                if (Sslbx == "门诊号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_MZH= 'f_sbh'  and F_sqdzt!='已登记' and F_JSRQ>dateadd(day,-30,GETDATE()) order by F_ID desc";
                if (Sslbx == "住院号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_ZYH= 'f_sbh'  and F_sqdzt!='已登记' and F_JSRQ>dateadd(day,-30,GETDATE()) order by F_ID desc";

                if (Sslbx == "ID号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_BRBH= 'f_sbh'  and F_sqdzt!='已登记' and F_JSRQ>dateadd(day,-30,GETDATE()) order by F_ID desc";
                if (Sslbx == "门诊ID号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_BRBH= 'f_sbh' and F_brlb='门诊' and F_sqdzt!='已登记' and F_JSRQ>dateadd(day,-30,GETDATE()) order by F_ID desc";
                if (Sslbx == "住院ID号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_BRBH= 'f_sbh' and F_brlb='住院' and F_sqdzt!='已登记' and F_JSRQ>dateadd(day,-30,GETDATE()) order by F_ID desc";

                if (Sslbx == "申请单号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_SQXH= 'f_sbh'  and F_sqdzt!='已登记'  ";
                if (Sslbx == "门诊申请号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_SQXH= 'f_sbh' and F_brlb='门诊' and F_sqdzt!='已登记' ";
                if (Sslbx == "住院申请号")
                    sql = "select F_brbh as 病人编号,F_brlb as  病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_LCBS as 临床病史,F_SF as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JSRQ as 日期 from  [pathnet].[dbo].[T_SQD] WHERE  F_SQXH= 'f_sbh' and F_brlb='住院' and F_sqdzt!='已登记' ";




                string hissql = f.ReadString(Sslbx, "blsql", sql);
                if (sql.Trim() == "")
                    return "0";
                hissql = hissql.Replace("f_sbh", Ssbz.Trim());
                string Columns = f.ReadString(Sslbx, "Columns", "日期,申请序号,姓名,性别,年龄,医嘱项目,送检科室,送检医生,病区,床号");
                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "日期,申请序号,姓名,性别,年龄,医嘱项目,送检科室,送检医生,病区,床号");
                dt_sqd = aa.GetDataTable(hissql, "sqd");

                /////////////////////////////////////////////////////////////////////
                if (dt_sqd == null)
                {
                    if (msg == "1")
                        MessageBox.Show("获取申请单信息失败");
                    return "0";
                }
                if (dt_sqd.Rows.Count == 0)
                {
                    if (msg == "1")
                        MessageBox.Show("未查询到申请单信息");
                    return "0";
                }
                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                int count = 0;
                if (dt_sqd.Rows.Count > tkts)
                {
                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                    DataColumn dc0 = new DataColumn("序号");
                    dt_sqd.Columns.Add(dc0);

                    for (int x = 0; x < dt_sqd.Rows.Count; x++)
                    {
                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                    }

                    if (Columns.Trim() != "")
                        Columns = "序号," + Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "序号," + ColumnsName;

                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                    if (yc.ShowDialog() == DialogResult.Yes)
                    {
                        string rtn2 = yc.F_XH;
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("未选择申请项目");
                            return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("请重新选择申请项目");
                            return "0";
                        }
                    }
                    else
                    {
                        MessageBox.Show("未选择申请项目");
                        return "0";
                    }
                }
                try
                {


                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                    string xb = dt_sqd.Rows[count]["性别"].ToString().Trim();
                    if (xb == "F" || xb == "f")
                        xb = "女";
                    if (xb == "M" || xb == "m")
                        xb = "男";
                    xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                    xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "地址=" + (char)34 + dt_sqd.Rows[count]["地址"].ToString().Trim() + (char)34 + "   ";
                    xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["电话"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "民族=" + (char)34 + dt_sqd.Rows[count]["民族"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "职业=" + (char)34 + dt_sqd.Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "收费=" + (char)34 + dt_sqd.Rows[count]["收费"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "标本名称=" + (char)34 + dt_sqd.Rows[count]["标本名称"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医院=" + (char)34 + dt_sqd.Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + dt_sqd.Rows[count]["备用1"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + dt_sqd.Rows[count]["备用2"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "费别=" + (char)34 + dt_sqd.Rows[count]["费别"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("获取申请单信息异常:" + e.Message);
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }


        private static string GetBrxx(string Sslbx, string Ssbz, string debug)
        {

            if (Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "门诊ID号" || Sslbx == "住院ID号" || Sslbx == "门诊申请号"|| Sslbx == "住院申请号")
            {
                string hissql = "";
                if (Sslbx == "门诊号")
                    hissql = "select sick_id as 病人编号,'门诊' as 病人类别,standing as 费别,'' as 住院号,nullah_number as 门诊号,name as 姓名 ,sex as 性别,age as 年龄,marital_status as 婚姻, association_address as 地址,association_phone as 电话,'' AS 病区,'' as 床号,id_card_no as 身份证号,nation as 民族,'' as 职业,dept_name as 送检科室,doctor_name AS 送检医生,diagnosis_name as 临床诊断,' ' as 临床病史,' ' as 收费,' ' as 就诊ID,apply_no as 申请序号,' ' as 标本名称,'本院' AS 送检医院,' ' as 医嘱项目 from v_pathology_dispensary  WHERE apply_no = 'f_sbh'  order by apply_no  desc";
              
                 if (Sslbx == "门诊ID号")
                     hissql = "select sick_id as 病人编号,'门诊' as 病人类别,standing as 费别,'' as 住院号,nullah_number as 门诊号,name as 姓名 ,sex as 性别,age as 年龄,marital_status as 婚姻, association_address as 地址,association_phone as 电话,'' AS 病区,'' as 床号,id_card_no as 身份证号,nation as 民族,'' as 职业,dept_name as 送检科室,doctor_name AS 送检医生,diagnosis_name as 临床诊断,' ' as 临床病史,' ' as 收费,' ' as 就诊ID,apply_no as 申请序号,' ' as 标本名称,'本院' AS 送检医院,' ' as 医嘱项目 from v_pathology_dispensary  WHERE sick_id = 'f_sbh'   order by apply_no  desc"; 
               
                if (Sslbx == "门诊申请号")
                    hissql = "select sick_id as 病人编号,'门诊' as 病人类别,standing as 费别,'' as 住院号,nullah_number as 门诊号,name as 姓名 ,sex as 性别,age as 年龄,marital_status as 婚姻, association_address as 地址,association_phone as 电话,'' AS 病区,'' as 床号,id_card_no as 身份证号,nation as 民族,'' as 职业,dept_name as 送检科室,doctor_name AS 送检医生,diagnosis_name as 临床诊断,' ' as 临床病史,' ' as 收费,' ' as 就诊ID,apply_no as 申请序号,' ' as 标本名称,'本院' AS 送检医院,' ' as 医嘱项目 from v_pathology_dispensary  WHERE apply_no = 'f_sbh'   ";
               
                if (Sslbx == "住院号")
                    hissql = "select sick_id as 病人编号,'住院'  as 病人类别,standing as 费别, residence_no as 住院号,'' as 门诊号,name as 姓名 ,sex as 性别,age as 年龄,marital_status as 婚姻, association_address as 地址,association_phone as 电话,ward AS 病区,bed_no as 床号,id_card_no as 身份证号,nation as 民族,'' as 职业,in_dept as 送检科室,now_doctor AS 送检医生,diagnosis as 临床诊断,' ' as 临床病史,' ' as 收费,' ' as 就诊ID,' ' as 申请序号,' ' as 标本名称,'本院' AS 送检医院,' ' as 医嘱项目 from v_pathology_residence WHERE  residence_no = 'f_sbh'  order by admission_time desc";
               
                if (Sslbx == "住院ID号")
                    hissql = "select sick_id as 病人编号,'住院'  as 病人类别,standing as 费别, residence_no as 住院号,'' as 门诊号,name as 姓名 ,sex as 性别,age as 年龄,marital_status as 婚姻, association_address as 地址,association_phone as 电话,ward AS 病区,bed_no as 床号,id_card_no as 身份证号,nation as 民族,'' as 职业,in_dept as 送检科室,now_doctor AS 送检医生,diagnosis as 临床诊断,' ' as 临床病史,' ' as 收费,' ' as 就诊ID,' ' as 申请序号,' ' as 标本名称,'本院' AS 送检医院,' ' as 医嘱项目 from v_pathology_residence WHERE  sick_id = 'f_sbh'   order by admission_time desc";
             
                if (Sslbx == "住院申请号")
                    hissql = "select sick_id as 病人编号,'住院'  as 病人类别,standing as 费别, residence_no as 住院号,'' as 门诊号,name as 姓名 ,sex as 性别,age as 年龄,marital_status as 婚姻, association_address as 地址,association_phone as 电话,ward AS 病区,bed_no as 床号,id_card_no as 身份证号,nation as 民族,'' as 职业,in_dept as 送检科室,now_doctor AS 送检医生,diagnosis as 临床诊断,' ' as 临床病史,' ' as 收费,' ' as 就诊ID,' ' as 申请序号,' ' as 标本名称,'本院' AS 送检医院,' ' as 医嘱项目 from v_pathology_residence WHERE  apply_no = 'f_sbh'  order by admission_time desc";

                hissql = f.ReadString(Sslbx, "hissql", hissql);
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "Provider=MSDAORA;Data Source=ZLYY;User id=pathnet;Password=4s3c2a1p;");

                if (hissql.Trim() == "")
                {
                    MessageBox.Show("无此识别号或配置不正确");
                    return "0";
                }

                hissql = hissql.Replace("f_sbh", Ssbz.Trim());

                DataTable dt_brxx = new DataTable();
                OleDbDB_ZGQ oledb = new OleDbDB_ZGQ();
                string errmsg = "";
               dt_brxx= oledb.OleDb_DataAdapter(odbcsql, hissql, ref errmsg);

               if (dt_brxx.Rows.Count <= 0)
                {
                    MessageBox.Show("未查询到此病人信息:" + errmsg);
                    return "0";
                }
                
                try
                {
                    int count = 0;
                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "病人编号=" + (char)34 + dt_brxx.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt_brxx.Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "申请序号=" + (char)34 + dt_brxx.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "门诊号=" + (char)34 + dt_brxx.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt_brxx.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "姓名=" + (char)34 + dt_brxx.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                    string xb = dt_brxx.Rows[count]["性别"].ToString().Trim();
                    if (xb == "F" || xb == "f")
                        xb = "女";
                    if (xb == "M" || xb == "m")
                        xb = "男";
                    xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                    xml = xml + "年龄=" + (char)34 + dt_brxx.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "婚姻=" + (char)34 + dt_brxx.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "地址=" + (char)34 + dt_brxx.Rows[count]["地址"].ToString().Trim() + (char)34 + "   ";
                    xml = xml + "电话=" + (char)34 + dt_brxx.Rows[count]["电话"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病区=" + (char)34 + dt_brxx.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "床号=" + (char)34 + dt_brxx.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "身份证号=" + (char)34 + dt_brxx.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "民族=" + (char)34 + dt_brxx.Rows[count]["民族"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "职业=" + (char)34 + dt_brxx.Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检科室=" + (char)34 + dt_brxx.Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医生=" + (char)34 + dt_brxx.Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "收费=" + (char)34 + dt_brxx.Rows[count]["收费"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "标本名称=" + (char)34 + dt_brxx.Rows[count]["标本名称"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医院=" + (char)34 + dt_brxx.Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt_brxx.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "费别=" + (char)34 + dt_brxx.Rows[count]["费别"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病人类别=" + (char)34 + dt_brxx.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<临床病史><![CDATA[" + dt_brxx.Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + dt_brxx.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("获取病人信息异常:" + e.Message);
                    return "0";
                }
            }
            else
            {
                MessageBox.Show("无此识别类型" + Sslbx);
                return "0";
            }
        }

    }
}
