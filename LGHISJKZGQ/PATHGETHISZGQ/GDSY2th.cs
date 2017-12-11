using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using LGHISJKZGQ;

namespace PATHGETHISQL
{
    //钱龙代码
    class GDSY
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        internal static string ptxml(string Sslbx, string Ssbz)
        {

            ////钱龙接口  2017-11-10 修改zgq
            string Debug = f.ReadString(Sslbx, "debug", "0");
            string Msg = f.ReadString(Sslbx, "msg", "0"); 
            string sftqbb = f.ReadString(Sslbx, "tqbblb", "1");
        
            string rexml = "0";

            if (Sslbx == "申请单")
            {
                return GetSqd(Sslbx, Ssbz, sftqbb, Debug, Msg);
            }
            if (Sslbx == "标本号")
            {
                rexml=  GetSqd(Sslbx, Ssbz, sftqbb, Debug, Msg);
                if (rexml!="0")  
                return rexml;

                return GetBBSQInfo(Ssbz, Debug, 4, sftqbb);
               
                   
            }
            if (Sslbx == "住 院")
            {
                rexml = GetSqd(Sslbx, Ssbz, sftqbb, Debug, Msg);
                if (rexml != "0") 
                    return rexml;
                return GetBBSQInfo(Ssbz, Debug, 2, "0");
            }
            if (Sslbx == "门 诊")
            {
                rexml = GetSqd(Sslbx, Ssbz, sftqbb, Debug, Msg);
                if (rexml != "0") 
                    return rexml;
                return GetBBSQInfo(Ssbz, Debug, 1, "0");
            }
           
            if (Sslbx == "东川体检")
            {
                string constring2 = f.ReadString("GDSY", "tjstring", "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;");
                ODBCHelper cc = new ODBCHelper(constring2);
                return GetTJInfo(cc, Ssbz, Debug);
            }
          
            if (Sslbx == "按申请查询")
            {
                GDSYSQForm sqform = new GDSYSQForm();
                if (sqform.DialogResult == DialogResult.OK)
                {
                    return sqform.rexml;
                }
                else
                {
                    MessageBox.Show("未选择有效的申请单");
                    return rexml;
                }
            }
            MessageBox.Show("无效的识别类型:" + Sslbx);
            return rexml;
        }

        private static string GetBBSQInfo( string Ssbz, string Debug, int Flag, string sftqbb)
        {
            string constring1 = f.ReadString("GDSY", "hisstring", "DSN=pathnethisgdsy;UID=pathnetuser;PWD=userpathnet;");
            ODBCHelper bb = new ODBCHelper(constring1);

            if (Flag == 4)
            {
                int a = 0;
                if (int.TryParse(Ssbz, out a) == false)
                {
                    Ssbz = Ssbz.Substring(4);
                }
            }

            OdbcCommand cmd = bb.GetStoredProcCommond("{call pGetFunctionRequestForPathNet(?,?,?,?,?,?,?,?)}");
            bb.AddInParameter(cmd, "@In_Flag", DbType.Int16, Flag);
            bb.AddInParameter(cmd, "@In_StartDate", DbType.String, "");
            bb.AddInParameter(cmd, "@In_EndDate", DbType.String, "");
            bb.AddInParameter(cmd, "@In_RequestDepartmentNos", DbType.String, "");
            if (Flag == 1)
            {
                bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, "");
                bb.AddInParameter(cmd, "@In_RegisterDate", DbType.DateTime, Ssbz.Substring(0, 10));
                int mzls;
                try
                {
                    mzls = int.Parse(Ssbz.Substring(11));
                }
                catch
                {
                    MessageBox.Show("请检查输入的门诊号格式是否正确！");
                    return "0";
                }
                bb.AddInParameter(cmd, "@In_SeqNo", DbType.Int32, mzls);
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, 0);
            }
            else if (Flag == 2)
            {
                bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, Ssbz);
                bb.AddInParameter(cmd, "@In_RegisterDate", DbType.String, "");
                bb.AddInParameter(cmd, "@In_SeqNo", DbType.Int32, 0);
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, 0);
            }
            else if (Flag == 4)
            {
                bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, "");
                bb.AddInParameter(cmd, "@In_RegisterDate", DbType.String, "");
                bb.AddInParameter(cmd, "@In_SeqNo", DbType.String, "");
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, Ssbz);
            }
            else
            {
                GDSYSQForm sqform = new GDSYSQForm();
                if (sqform.DialogResult == DialogResult.OK)
                {
                    return sqform.rexml;
                }
            }
            OdbcDataAdapter sqlda = new OdbcDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            if (ds.Tables == null || ds.Tables.Count < 1)
                return GetBBInfo(bb, Ssbz, Debug, Flag, sftqbb);

            DataTable dt = ds.Tables[0];
            if (Debug == "1")
            {
                MessageBox.Show("查询到数据条数：" + dt.Rows.Count.ToString());
            }
            if (dt == null || dt.Rows.Count < 1)
                return GetBBInfo(bb, Ssbz, Debug, Flag, sftqbb);

            GDSYUNIForm uniform = new GDSYUNIForm();
            uniform.indt = dt;
            if (uniform.ShowDialog() == DialogResult.OK)
            {
                DataRow outdr = uniform.outdr;
                if (outdr == null) return GetBBInfo(bb, Ssbz, Debug, Flag, sftqbb);
                //DataRow outdr = dt.Rows[0];
                LogeneXmlHelper lxp = new LogeneXmlHelper();
                lxp.F_ZYH = outdr["IPSeqNoText"] == null ? "" : outdr["IPSeqNoText"].ToString().Trim();
                lxp.F_BRBH = outdr["PatientID"] == null ? "" : outdr["PatientID"].ToString().Trim();
                //empiid存职业字段
                lxp.F_ZY = GetEMPIID(lxp.F_BRBH);

                lxp.F_CH = outdr["SickBedNo"] == null ? "" : outdr["SickBedNo"].ToString().Trim();
                lxp.F_XM = outdr["PatientName"] == null ? "" : outdr["PatientName"].ToString().Trim();
                lxp.F_XB = outdr["PatientSex"] == null ? "" : outdr["PatientSex"].ToString().Trim();
                //string birth = outdr["PatientBirthDay"] == null ? "" : outdr["PatientBirthDay"].ToString().Trim();
                //lxp.F_NL = CalculateAgeCorrect(DateTime.Parse(birth), DateTime.Now).ToString(); ;
                lxp.F_NL = outdr["PatientAge"] == null ? "" : outdr["PatientAge"].ToString().Trim();
                lxp.F_MZH = outdr["SeqNo"] == null ? "" : (outdr["SeqNo"].ToString().Trim() == "-1" ? " " : outdr["SeqNo"].ToString().Trim());
                lxp.F_SJKS = outdr["RequestDepartmentName"] == null ? "" : outdr["RequestDepartmentName"].ToString().Trim();
                lxp.F_SJYS = outdr["RequestEmployeeName"] == null ? "" : outdr["RequestEmployeeName"].ToString().Trim();
                lxp.F_JZID = outdr["InPatientID"] == null ? "" : outdr["InPatientID"].ToString().Trim();
                lxp.F_SQXH = outdr["FunctionRequestID"] == null ? "" : outdr["FunctionRequestID"].ToString().Trim();
                lxp.F_YZXM = outdr["ItemName"] == null ? "" : outdr["ItemName"].ToString().Trim();
                lxp.F_HY = outdr["Marriage"] == null ? "" : outdr["Marriage"].ToString();
                lxp.F_ADDRESS = outdr["Address"] == null ? "" : outdr["Address"].ToString();
                lxp.F_TELEPHONE = outdr["Phone"] == null ? "" : outdr["Phone"].ToString();
                lxp.F_SFZH = outdr["IdentityCardNo"] == null ? "" : outdr["IdentityCardNo"].ToString();
                lxp.F_MZ = outdr["RaceDesc"] == null ? "" : outdr["RaceDesc"].ToString();
             //   lxp.F_ZY = outdr["ProfessionDesc"] == null ? "" : outdr["ProfessionDesc"].ToString();
                lxp.F_LCZD = outdr["diseaseName"] == null ? "" : outdr["diseaseName"].ToString();
                lxp.F_BQ = outdr["IPDepartmentName"] == null ? "" : outdr["IPDepartmentName"].ToString();
                if (Flag == 1)
                {
                    lxp.F_BRLB = "门诊";
                }
                else if (Flag == 2)
                {
                    lxp.F_BRLB = "住院";
                }


                
                if (sftqbb == "1")
                {
                   

                    lxp.BBLBLIST = TQBBLB(bb, Ssbz);
                    if (Debug == "1")
                        MessageBox.Show("标本数量："+lxp.BBLBLIST.Count.ToString());
                    if (lxp.BBLBLIST == null)
                    {
                        MessageBox.Show("未查询到标本信息");
                    }
                    else
                    {
                        if (Debug == "1")
                            MessageBox.Show("返回XML：" + lxp.ReturnBBLBXML());
                        return lxp.ReturnBBLBXML();
                    }
                }
                if (Debug == "1")
                    MessageBox.Show("返回XML：" + lxp.ReturnLogeneXML());
                return lxp.ReturnLogeneXML();
            }

            //GDSYMutiForm mutiform = new GDSYMutiForm();
            //mutiform.indt = dt;
            //if (mutiform.ShowDialog() == DialogResult.OK)
            //{
            //    return mutiform.rexml;
            //}

            return GetBBInfo(bb, Ssbz, Debug, Flag, sftqbb);
        }

        private static string GetBBInfo(ODBCHelper bb, string Ssbz, string Debug, int Flag, string sftqbb)
        {
            OdbcCommand cmd = bb.GetStoredProcCommond("{call pGETPatientInfoForPathNet(?,?,?,?,?)}");
            bb.AddInParameter(cmd, "@In_Flag", DbType.Int16, Flag);
            if (Flag == 1)
            {
                bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, "");
                bb.AddInParameter(cmd, "@In_RegisterDate", DbType.DateTime, Ssbz.Substring(0, 10));
                int mzls;
                try
                {
                    mzls = int.Parse(Ssbz.Substring(11));
                }
                catch
                {
                    MessageBox.Show("请检查输入的门诊号格式是否正确！");
                    return "0";
                }
                bb.AddInParameter(cmd, "@In_SeqNo", DbType.Int32, mzls);
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, 0);
            }
            else if (Flag == 2)
            {
                bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, Ssbz);
                bb.AddInParameter(cmd, "@In_RegisterDate", DbType.String, "");
                bb.AddInParameter(cmd, "@In_SeqNo", DbType.String, "");
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, 0);
            }
            else if (Flag == 4)
            {
                bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, "");
                bb.AddInParameter(cmd, "@In_RegisterDate", DbType.String, "");
                bb.AddInParameter(cmd, "@In_SeqNo", DbType.String, "");
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, Ssbz);
            }
            
            OdbcDataAdapter sqlda = new OdbcDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            if (ds.Tables == null || ds.Tables.Count < 1)
                return "0";
            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count < 1)
                return "0";

            LogeneXmlHelper lxp = new LogeneXmlHelper();
            lxp.F_ZYH = dt.Rows[0]["IPSeqNOText"] == null ? "" : dt.Rows[0]["IPSeqNOText"].ToString().Trim();
            lxp.F_BRBH = dt.Rows[0]["patientid"] == null ? "" : dt.Rows[0]["patientid"].ToString().Trim();
            //empiid存职业字段
            lxp.F_ZY = GetEMPIID(lxp.F_BRBH);


            lxp.F_CH = dt.Rows[0]["sickbedno"] == null ? "" : dt.Rows[0]["sickbedno"].ToString().Trim();
            lxp.F_XM = dt.Rows[0]["patientname"] == null ? "" : dt.Rows[0]["patientname"].ToString().Trim();
            string birthday = dt.Rows[0]["patientbirthday"] == null ? "" : dt.Rows[0]["patientbirthday"].ToString().Trim();
            if (birthday != "")
            {
                try
                {
                    DateTime birth = DateTime.Parse(birthday);
                    int nl = CalculateAgeCorrect(birth, DateTime.Now);
                    lxp.F_NL = nl.ToString() + "岁";
                }
                catch
                {
                    lxp.F_NL = "";
                }
            }

            lxp.F_XB = dt.Rows[0]["patientsex"] == null ? "" : dt.Rows[0]["patientsex"].ToString().Trim();
            lxp.F_HY = dt.Rows[0]["marriage"] == null ? "" : dt.Rows[0]["marriage"].ToString().Trim();
            lxp.F_SJKS = dt.Rows[0]["departmentname"] == null ? "" : dt.Rows[0]["departmentname"].ToString().Trim();
            lxp.F_ADDRESS = dt.Rows[0]["address"] == null ? "" : dt.Rows[0]["address"].ToString().Trim();
            lxp.F_TELEPHONE = dt.Rows[0]["phone"] == null ? "" : dt.Rows[0]["phone"].ToString().Trim();
            switch (Flag)
            {
                case 1:
                    lxp.F_BRLB = "门诊";
                    break;
                case 2:
                    lxp.F_BRLB = "住院";
                    break;
                default:
                    lxp.F_BRLB = dt.Rows[0]["PatientTypeListName"] == null ? "" : dt.Rows[0]["PatientTypeListName"].ToString().Trim();
                    break;
            }

            if (sftqbb == "1")
            {

                if (Debug == "1")
                {
                    MessageBox.Show("取标本列表" );
                }
                lxp.BBLBLIST = TQBBLB(bb, Ssbz);

                if (lxp.BBLBLIST == null)
                {
                    MessageBox.Show("未查询到标本信息");
                }
                else
                {
                    if (Debug == "1")
                        MessageBox.Show("返回XML：" + lxp.ReturnBBLBXML());
                    return lxp.ReturnBBLBXML();
                }
            }

            return lxp.ReturnLogeneXML();
        }

        public static string GetTJInfo(ODBCHelper cc, string Ssbz, string Debug)
        {
            OdbcCommand cmd = cc.GetOdbcStringCommond("select * from V_TJ_BRINFO where tjno = '" + Ssbz + "'");
            DataSet ds = cc.ExecuteDataSet(cmd);

            if (ds.Tables == null || ds.Tables.Count < 1)
                return "0";
            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count < 1)
                return "0";

            LogeneXmlHelper lxp = new LogeneXmlHelper();
            lxp.F_BRBH = dt.Rows[0]["tjno"] == null ? "" : dt.Rows[0]["tjno"].ToString().Trim();
            lxp.F_XM = dt.Rows[0]["name"] == null ? "" : dt.Rows[0]["name"].ToString().Trim();
            lxp.F_NL = dt.Rows[0]["age"] == null ? "" : dt.Rows[0]["age"].ToString().Trim();
            lxp.F_XB = dt.Rows[0]["sex"] == null ? "" : dt.Rows[0]["sex"].ToString().Trim();
            lxp.F_SJKS = dt.Rows[0]["department"] == null ? "" : dt.Rows[0]["department"].ToString().Trim();
            lxp.F_ADDRESS = dt.Rows[0]["home_address"] == null ? "" : dt.Rows[0]["home_address"].ToString().Trim();
            lxp.F_TELEPHONE = dt.Rows[0]["home_tel"] == null ? "" : dt.Rows[0]["home_tel"].ToString().Trim();
            lxp.F_BRLB = "体检";

            return lxp.ReturnLogeneXML();
        }

        public static int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }

        private static List<BBLB> TQBBLB(ODBCHelper bb, string Ssbz)
        {
            List<BBLB> bblbs = new List<BBLB>();
            try
            {
              
                OdbcCommand cmd = bb.GetStoredProcCommond("{call pGetFunctionSampleByLabelIDForPathNet(?)}");
                bb.AddInParameter(cmd, "@In_FunctionSampleLabelID", DbType.Int32, Ssbz);
                OdbcDataAdapter sqlda = new OdbcDataAdapter(cmd);
                DataSet ds = new DataSet();
                sqlda.Fill(ds);

                if (ds.Tables == null || ds.Tables.Count < 1)
                    return bblbs;

                DataTable dt = ds.Tables[0];

                if (dt == null || dt.Rows.Count < 1)
                    return bblbs;



                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    BBLB bblb = new BBLB();
                    bblb.F_BBXH = (i + 1).ToString();
                    bblb.F_BBTMH = dt.Rows[i]["FunctionSampleLabelID"] == null ? "" : dt.Rows[i]["FunctionSampleLabelID"].ToString().Trim();
                    bblb.F_BBMC = dt.Rows[i]["SampleRemark"] == null ? "" : dt.Rows[i]["SampleRemark"].ToString().Trim();
                    bblb.F_LTSJ = dt.Rows[i]["SampleGatherDateTime"] == null ? "" : dt.Rows[i]["SampleGatherDateTime"].ToString().Trim();
                    bblb.F_JSSJ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string jsy = f.ReadString("yh", "yhmc", "").Replace("\0", "");
                    bblb.F_JSY = jsy;
                    bblb.F_BBZT = "已接收";
                    bblbs.Add(bblb);

                   
                }

                return bblbs;
            }
            catch(Exception  e)
            {
                MessageBox.Show(e.Message);
                return bblbs;
            }

        }

        public static string GetEMPIID(string patientid)
        {
            if (patientid.Trim() == "")
                return "";

            string constring2 = f.ReadString("GDSYEMPIID", "odbc", "Data Source=svrmainopsql;Initial Catalog=his_op_publicinterface;User Id=empi;Password=empi;");
            string sqlstr = f.ReadString("GDSYEMPIID", "sqlstr", "select top 1 EMPI_ID  from his_op_publicinterface.dbo.empi_view   where PATIENT_ID= ");
       
        
            SqlConnection con = new SqlConnection(constring2);

            if (sqlstr.Trim() == "")
            {
                log.WriteMyLog("获取EMPIID异常：" + sqlstr);
                return "";
            }
            try
            {
           
                sqlstr = sqlstr + " '" + patientid + "'";

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(sqlstr, con);
                con.Open();
                da.Fill(dt);
               
                da.Dispose(); con.Close();
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["EMPI_ID"].ToString().Trim();
                else
                return "";
            }
            catch (Exception ee)
            {
                log.WriteMyLog( "获取EMPIID异常：" + ee.Message);
                con.Close();
                return "";
            }


            return "";
        }

        public static string GetSqd(string Sslbx, string Ssbz, string tqbblb, string Debug, string Msg)
        {
            string ErrMsg="";
            try
            {
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;");
                int tkts = f.ReadInteger(Sslbx, "tkts", 0);
                string hissql = f.ReadString(Sslbx, "hissql", "select top 1000 F_brbh as 病人编号,F_brlb 病人类别,F_FB as 费别,F_ZYH as 住院号,F_MZH as 门诊号,F_XM as 姓名,F_XB as 性别,F_NL as 年龄,'' as 婚姻,F_DZ as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJYS AS 送检医生,F_LCZD as 临床诊断,F_lczl as 临床病史,'' as 收费,F_JZCS as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXM as 医嘱项目,F_BY1 as 备用1,F_BY2 as 备用2,F_JZLSH from T_SQD  WHERE 1=1 ");
                ZgqClassPub.DBData.OdbcDB db = new ZgqClassPub.DBData.OdbcDB();

                string cxtj = "";

                if (Sslbx == "标本号")
                {
                    DataTable dt_bb = db.DataAdapter(odbcsql, "select top 1 F_SQXH from T_SQD_BB  where F_BBH ='" + Ssbz + "'", ref ErrMsg);
                    if (dt_bb.Rows.Count <= 0)
                    {
                        if (Msg == "1")
                            MessageBox.Show("未查询到此标本号数据(MQ)");
                        return "0";
                    }
                    cxtj = " and F_SQXH='" + dt_bb.Rows[0]["F_SQXH"].ToString().Trim() + "' ";
                }
                else if (Sslbx == "住 院")
                {
                    cxtj = " and F_ZYH='" + Ssbz + "' ";
                }
                else if (Sslbx == "门 诊")
                {
                    cxtj = " and F_MZH='" + Ssbz + "' ";
                }
                else if (Sslbx == "申请单")
                {
                    cxtj = " and F_SQXH='" + Ssbz + "' ";
                }
                else
                {
                }
                if (cxtj.Trim() == "")
                    return "0";

                hissql = hissql + cxtj + " order by F_SQRQ desc";

                DataTable dt_sqd = new DataTable();
                dt_sqd = db.DataAdapter(odbcsql, hissql, ref ErrMsg);

                int count = 0;
                if (dt_sqd.Rows.Count <= 0)
                {
                    if (Msg == "1")
                        MessageBox.Show("未查询到申请单数据(MQ)");
                    return "0";
                }

                if (dt_sqd.Rows.Count > tkts)
                {

                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                    DataColumn dc0 = new DataColumn("序号");
                    dt_sqd.Columns.Add(dc0);

                    for (int x = 0; x < dt_sqd.Rows.Count; x++)
                    {
                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                    }
                    string Columns = f.ReadString(Sslbx, "Columns", "申请序号,F_jzlsh,姓名,性别,医嘱项目,送检科室");//显示的项目对应字段
                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请序号,就诊号,姓名,性别,医嘱项目,送检科室");//显示的项目名称

                    if (Columns.Trim() != "")
                        Columns = "序号," + Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "序号," + ColumnsName;

                    string rtn2 = "0";
                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                    if (yc.ShowDialog() == DialogResult.Yes)
                    {
                        count = int.Parse(yc.F_XH);
                    }
                    else
                    {
                        MessageBox.Show("未选择申请项目！");
                        return "0";
                    }
                   

                }

                string sqxh = dt_sqd.Rows[count]["申请序号"].ToString().Trim();
                string BBLB_XML = ""; string bbmc = "";
                if (sqxh != "")
                {
                    #region 提取标本信息
                    if (tqbblb == "1")
                    {
                        string YHMC = f.ReadString("YH", "YHMC", "");
                        DataTable dt_bbxx = new DataTable();
                        dt_bbxx = db.DataAdapter(odbcsql, "select * from T_SQD_BB  where F_SQXH ='" + sqxh + "'", ref ErrMsg);
                        if (dt_bbxx.Rows.Count <= 0)
                        {
                            if (Msg == "1")
                                MessageBox.Show("未查询到此申请单的标本记录");
                        }
                        else
                        {
                            BBLB_XML = "<BBLB>";

                            for (int x = 0; x < dt_bbxx.Rows.Count; x++)
                            {
                                try
                                {
                                    BBLB_XML = BBLB_XML + "<row ";
                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_bbxx.Rows[x]["F_XH"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bbxx.Rows[x]["F_BBH"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bbxx.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bbxx.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bbxx.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bbxx.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + YHMC + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "/>";

                                    //if (bbmc == "")
                                    //    bbmc = F_BBXHdt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim();
                                    //else
                                    //    bbmc = bbmc +","+ dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim();
                                }
                                catch (Exception eee)
                                {
                                    MessageBox.Show("获取标本列表信息异常:" + eee.Message);
                                    tqbblb = "0";
                                    break;
                                }
                            }
                            BBLB_XML = BBLB_XML + "</BBLB>";
                        }
                    }
                    #endregion
                }



                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                xml = xml + "申请序号=" + (char)34 + sqxh + (char)34 + " ";
                xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                xml = xml + "性别=" + (char)34 + dt_sqd.Rows[count]["性别"].ToString().Trim() + (char)34 + " ";

                xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                 xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " "; 
              
                xml = xml + "地址=" + (char)34 + dt_sqd.Rows[count]["地址"].ToString().Trim() + (char)34 + "   "; 
             
                 xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["电话"].ToString().Trim() + (char)34 + " "; 
         
                xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
               xml = xml + "民族=" + (char)34 + dt_sqd.Rows[count]["民族"].ToString().Trim() + (char)34 + " "; 
              
                try
                {
                    //empiid存职业字段
                    if (dt_sqd.Rows[count]["病人编号"].ToString()!="")
                    xml = xml + "职业=" + (char)34 + GetEMPIID(dt_sqd.Rows[count]["病人编号"].ToString()) + (char)34 + " ";
                    else
                    xml = xml + "职业=" + (char)34 + dt_sqd.Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                }
                catch { xml = xml + "职业=" + (char)34 + "" + (char)34 + " "; }
                xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                try { xml = xml + "收费=" + (char)34 + dt_sqd.Rows[count]["收费"].ToString().Trim() + (char)34 + " "; }
                catch { xml = xml + "收费=" + (char)34 + "" + (char)34 + " "; }
                xml = xml + "标本名称=" + (char)34 + dt_sqd.Rows[count]["标本名称"].ToString().Trim() + (char)34 + " ";
                xml = xml + "送检医院=" + (char)34 + dt_sqd.Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                try { xml = xml + "备用1=" + (char)34 + dt_sqd.Rows[count]["备用1"].ToString().Trim() + (char)34 + " "; }
                catch { xml = xml + "备用1=" + (char)34 + ""+ (char)34 + " "; }
                try { xml = xml + "备用2=" + (char)34 + dt_sqd.Rows[count]["备用2"].ToString().Trim() + (char)34 + " "; }
                catch { xml = xml + "备用2=" + (char)34 + "" + (char)34 + " "; }
                try { xml = xml + "费别=" + (char)34 + dt_sqd.Rows[count]["费别"].ToString().Trim() + (char)34 + " "; }
                catch { xml = xml + "费别=" + (char)34 + ""+ (char)34 + " "; }
                xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                xml = xml + "原病理号=" + (char)34 + "" + (char)34 + " ";
                
                xml = xml + "/>";
                xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";

                if (tqbblb == "1")
                    xml = xml + BBLB_XML;
                xml = xml + "</LOGENE>";

                return xml;
                }
            catch(Exception   ee2)
            {
                if (Msg == "1")
                    MessageBox.Show("查询申请单数据异常(MQ):" + ee2.Message);
                return "0";
            }
          
        }
        
    }
}
