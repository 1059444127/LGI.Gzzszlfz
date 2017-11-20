using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using Lg.Pdf2Jpg;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    //中山大学肿瘤医院分子诊断科
    public class zsdxzlyyfzPT
    {
        private IniFiles f = new IniFiles();
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            string bgzt = "";
            bglx = bglx.ToLower();
            if (bglx == "") bglx = "cg";
            if (bgxh == "") bgxh = "1";

            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;

            DataTable jcxx = new DataTable();
            DataTable dt_bc = new DataTable();
            DataTable dt_bd = new DataTable();
            DataTable dt_sqd = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }

            bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
            string sqxh = jcxx.Rows[0]["F_sqxh"].ToString().Trim();
            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                    bgzt = "取消审核";
            }

            string tjodbc = f.ReadString("savetohis", "tjodbc",
                "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");
            string blodbc = f.ReadString("savetohis", "blodbc",
                "Data Source=172.16.95.230;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p;");

            string errMsg = "";
            string hczt = f.ReadString("zgqjk", "hczt", "1").Trim();
            string hcbg = f.ReadString("zgqjk", "hcbg", "1").Trim();

            string yhmc = f.ReadString("yh", "yhmc", "1").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "1").Trim();

            string zdscqp = f.ReadString("savetohis", "zdscqp", "1").Trim();
            //获取病理库对应的项目,如果病理库与项目不对应,则保存时提示用户.
            //多个项目用 | 隔开
            var ebvxm = f.ReadString("savetohis", "ebvxm", "1").Trim().Split('|');
            var hbvxm = f.ReadString("savetohis", "hbvxm", "1").Trim().Split('|');


            #region 保存登记时,插入切片信息

            //  log.WriteMyLog("debug生成切片1,bgzt=" + bgzt);
            if (bgzt == "已登记")
            {
                //      log.WriteMyLog("debug生成切片2,zdscqp=" + zdscqp);
                if (zdscqp == "1")
                {
                    #region string sqlInsertQp

                    string sqlInsertQp =
                        $@"
INSERT INTO [dbo].[T_QP]
           ([F_BLH]
           ,[F_RWLY]
           ,[F_LKH]
           ,[F_YZH]
           ,[F_QPTMH]
           ,[F_QPXH]
           ,[F_QPBH]
           ,[F_QPSM]
           ,[F_CZY]
           ,[F_QPSJ]
           ,[F_DYZT]
           ,[F_JGPJ]
           ,[F_PJR]
           ,[F_QPZT]
           ,[F_QPTMH2]
           ,[F_GDZT]
           ,[F_GDCZY]
           ,[F_GDSJ]
           ,[F_BZ]
           ,[F_TJH]
           ,[F_ZXSB]
           ,[F_RSY]
           ,[F_RSSJ])
     VALUES
           ( '{blh}', --<F_BLH, varchar(20),>
           '正常',  --<F_RWLY, varchar(20),>
           '{blh}',  --<F_LKH, varchar(40),>
           ' ',  --<F_YZH, varchar(100),>
           '{blh +
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           "001"}',  --<F_QPTMH, varchar(50),>
           '0',  --<F_QPXH, int,>
           '{blh}',  --<F_QPBH, varchar(50),>
           ' ',  --<F_QPSM, varchar(50),>
           ' ',  --<F_CZY, varchar(10),>
           ' ',  --<F_QPSJ, varchar(20),>
           ' ',  --<F_DYZT, varchar(2),>
           ' ',  --<F_JGPJ, varchar(30),>
           null,  --<F_PJR, varchar(20),>
           ' ',  --<F_QPZT, varchar(10),>
           ' ',  --<F_QPTMH2, varchar(10),>
           ' ',  --<F_GDZT, varchar(50),>
           ' ',  --<F_GDCZY, varchar(20),>
           ' ',  --<F_GDSJ, varchar(30),>
           ' ',  --<F_BZ, varchar(50),>
           ' ',  --<F_TJH, varchar(50),>
           ' ',  --<F_ZXSB, varchar(50),>
           ' ',  --<F_RSY, varchar(20),>
           ' '  --<F_RSSJ, varchar(20),>
			)
";

                    #endregion

                    string sqlCheckQpExists = $" select count(*) as cc from t_qp where f_blh='{blh}' ";

                    //                    log.WriteMyLog("debug生成切片3,sqlCheckQpExists=" + sqlCheckQpExists);
                    //                    log.WriteMyLog("debug生成切片4,sqlInsertQp=" + sqlInsertQp);


                    var qpCount = aa.GetDataTable(sqlCheckQpExists, "table1").Rows[0][0].ToString();
                    //           log.WriteMyLog("debug生成切片5,qpCount=" + qpCount);

                    if (qpCount == "0")
                    {
                        var insertCount = aa.ExecuteSQL(sqlInsertQp);
                        //log.WriteMyLog("debug生成切片6,insertCount=" + insertCount);
                    }
                }
            }

            #region 过滤报告项目,如果与病例库不匹配,给出提示

            if (bgzt == "已登记")
            {
                var blk = jcxx.Rows[0]["F_blk"].ToString();
                var yzxm = jcxx.Rows[0]["F_yzxm"].ToString();
                var xm = jcxx.Rows[0]["f_xm"];
                //医嘱项目被秋保存为 编号^名称 的格式,因此可能需要解析一下
                var lstYzxm = jcxx.Rows[0]["F_yzxm"].ToString().Split('^');
                if (lstYzxm.Length > 1)
                    yzxm = lstYzxm[1];

                var notMatch = true;
                if (blk == "EBV" && ebvxm.Length > 0)
                    foreach (var s in ebvxm)
                    {
                        if (yzxm == s)
                        {
                            notMatch = false;
                            break;
                        }
                    }
                else if (blk == "HBV" && hbvxm.Length > 0)
                    foreach (var s in hbvxm)
                    {
                        if (yzxm == s)
                        {
                            notMatch = false;
                            break;
                        }
                    }
                else
                {
                    notMatch = false;
                }
                if (notMatch)
                    MessageBox.Show($"注意:病人[{xm}],登记的病例库[{blk}],与医嘱项目[{yzxm}]不匹配!");
            }

            #endregion


            #region LIS标本签收

            string lisbbqs = f.ReadString("savetohis", "lisbbqs", "0");
            if (bgzt == "已登记" && lisbbqs == "1")
            {


                string hggh = f.ReadString("savetohis", "lisbbqs_hggh", "");
                string bbqsDebug = f.ReadString("savetohis", "lisbbqs_debug", "0");
                string hgxm = f.ReadString("savetohis", "lisbbqs_hgxm", "");
                string lisUrl = f.ReadString("savetohis", "liswsurl", "http://172.16.95.189:8081/PDAService.asmx");
                string yzid = jcxx.Rows[0]["F_yzid"].ToString();

                if (bbqsDebug == "1")
                    log.WriteMyLog("开始标本签收!");

                if (string.IsNullOrEmpty(yzid))
                {
                    if (bbqsDebug == "1")
                        log.WriteMyLog("标本签收被中断,因为医嘱id(条码号)为空");

                    goto 标本签收结束;
                }

                string xml = $@"<?xml version='1.0' encoding='gb2312' ?>
                                    <DocumentElement>
                                      <AccessKey></AccessKey>
                                      <MethodName>LSCTIS_UpdateBarcodeStatus</MethodName>
                                      <DataTable> 
 	                                    <lsi_barcode>{jcxx.Rows[0]["F_yzid"]}</lsi_barcode>
                                        <lsi_barcode_state>5</lsi_barcode_state>
                                        <lsi_operator_id>{hggh}</lsi_operator_id>
                                        <lsi_operator_name>{hgxm}</lsi_operator_name>
                                        <lsi_operator_date>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</lsi_operator_date>
                                      </DataTable>
                                    </DocumentElement>
                                ";

                if (bbqsDebug == "1")
                    log.WriteMyLog("标本签收XML:\r\n" + xml);


                GzzszlyyFzLis.PDAService ws = new GzzszlyyFzLis.PDAService();
                ws.Url = lisUrl;
                try
                {
                    byte[] b = Encoding.Default.GetBytes(xml);
                    //转成 Base64 形式的 System.String  
                    var xmlBase64String = Convert.ToBase64String(b);
                    var resultString = ws.Service(xmlBase64String);
                    if (resultString.Contains(@"<Result>0</Result>") == false)
                    {
                        throw new Exception(resultString);
                    }

                    if (bbqsDebug == "1")
                        log.WriteMyLog("标本签收成功!");
                }
                catch (Exception e)
                {
                    log.WriteMyLog("标本签收失败:" + e);
                }
            }

            标本签收结束:
            #endregion



            #endregion

            #region 审核时,弹出疾病选择

            if (bgzt == "已审核")
            {
                GetDisease(blh, bglx, jcxx.Rows[0]["F_blk"].ToString());
            }

            #endregion

            // //病理开出的医嘱
            if (sqxh.Contains("TJ-"))
            {
                errMsg = "";
                SqlDB db = new SqlDB();

                string sqlstr = "";
                if (bgzt == "已登记" || bgzt == "已取材")
                    sqlstr = " update [pathnet].[dbo].[T_TJYZ]  set F_YZZT='已执行',F_ZXSJ='" +
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',F_ZXR='" + yhmc + "'   where  F_TJYZH='" +
                             sqxh + "' and (F_YZZT='' or F_YZZT=' ')";
                if (bgzt == "已审核")
                    sqlstr = " update [pathnet].[dbo].[T_TJYZ]  set F_YZZT='已完成',F_ZXSJ='" +
                             DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',F_ZXR='" + yhmc + "'   where  F_TJYZH='" +
                             sqxh + "' ";
                if (sqlstr != "")
                {
                    db.ExecuteNonQuery(blodbc, sqlstr, ref errMsg);
                }

                #region 医嘱状态拿到分子病理一份,先删除后插入

                string sqlDelFzTjyz = $" delete dbo.t_tjyz where F_TJYZH='{sqxh}' ";

                #region string sqlInsertFzTjyz

                string sqlInsertFzTjyz =
                    $@"
INSERT INTO [dbo].[T_TJYZ]
           ([F_BLH]
           ,[F_TJYZH]
           ,[F_YZLX]
           ,[F_LKH]
           ,[F_BJW]
           ,[F_SL]
           ,[F_BX]
           ,[F_QD]
           ,[F_BZ]
           ,[F_SQYS]
           ,[F_SQSJ]
           ,[F_YZZT]
           ,[F_ZXSJ]
           ,[F_ZXR]
           ,[F_DKRH]
           ,[F_LX]
           ,[F_CJ]
           ,[F_FF]
           ,[F_ZXSB]
           ,[F_FSSJ]
           ,[F_YZLRY]
           ,[F_SFZT]
           ,[F_ZXXH])
select [F_BLH]
           ,[F_TJYZH]
           ,[F_YZLX]
           ,[F_LKH]
           ,[F_BJW]
           ,[F_SL]
           ,[F_BX]
           ,[F_QD]
           ,[F_BZ]
           ,[F_SQYS]
           ,[F_SQSJ]
           ,[F_YZZT]
           ,[F_ZXSJ]
           ,[F_ZXR]
           ,[F_DKRH]
           ,[F_LX]
           ,[F_CJ]
           ,[F_FF]
           ,[F_ZXSB]
           ,[F_FSSJ]
           ,[F_YZLRY]
           ,[F_SFZT]
           ,[F_ZXXH]
		   from [172.16.95.230].pathnet.dbo.t_tjyz
		   where f_tjyzh='{sqxh}' ";

                #endregion

                aa.ExecuteSQL(sqlDelFzTjyz);
                aa.ExecuteSQL(sqlInsertFzTjyz);

                #endregion


                return;
            }

            if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
            {
                if (debug == "1")
                    log.WriteMyLog("体检回传");
                string tjjk = f.ReadString("zgqjk", "tjjk", "0").Trim();
                string tjtoptjk = f.ReadString("zgqjk", "tjtoptjk", "1").Trim();
                if (tjjk == "1")
                {
                    #region 体检接口

                    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                    {
                        // aa.ExecuteSQL("update T_jcxx_fs set F_fszt='不处理',F_bz='体检病人无体检申请号,不处理！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                        log.WriteMyLog(blh + ",体检病人无病人编号，不处理！");
                        return;
                    }

                    sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim().Replace("T", "").Trim();
                    if (bgzt == "已审核")
                    {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                        // 诊断描述
                        string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                        //诊断结论	Res_con
                        string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

                        string str_com = "update  tj_pacs_resulto_temp  set Res_doctor='" +
                                         jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',Res_doctor_code='',Res_date='" +
                                         DateTime.Parse(jcxx.Rows[0]["F_BGrq"].ToString().Trim()) + "',Res_char='" +
                                         Res_char + "',Res_con='" + Res_con + "',Res_flag=2 where res_no='" + sqxh + "'";
                        if (debug == "1")
                            log.WriteMyLog("回写体检表，语句：" + str_com);
                        SqlDB db = new SqlDB();

                        int x = db.ExecuteNonQuery(tjodbc, str_com, ref errMsg);
                        if (errMsg != "" && errMsg != "OK")
                        {
                            //   aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + errMsg + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                            log.WriteMyLog(blh + ",体检报告审核，接口异常信息：" + errMsg);
                        }
                        else
                        {
                            log.WriteMyLog(blh + ",体检报告发送完成");
                            //   aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告审核,接口上传成功！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                        }
                    }
                    else
                    {
                        if (bgzt == "取消审核")
                        {
                            string str_com = "update  tj_pacs_resulto_temp set Res_doctor='" + "" +
                                             "',Res_doctor_code='',Res_date='" + DateTime.Today + "',Res_char='" + "" +
                                             "',Res_con='" + "" + "',Res_flag=2 where res_no='" + sqxh + "'";
                            if (debug == "1")
                                log.WriteMyLog("回写体检表，语句：" + str_com);
                            SqlDB db = new SqlDB();

                            int x = db.ExecuteNonQuery(tjodbc, str_com, ref errMsg);
                            if (errMsg != "" && errMsg != "OK")
                            {
                                //aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + errMsg + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                                log.WriteMyLog(blh + ",体检报告取消审核，接口异常信息：" + errMsg);
                            }
                            else
                            {
                                log.WriteMyLog(blh + ",体检报告取消审核回传完成");
                            }
                            //    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告取消审核，接口上传成功！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                        }
                    }

                    #endregion
                }
                if (tjtoptjk == "1")
                {
                    log.WriteMyLog(blh + ",开始体检发送到平台 " + "select * from T_SQD where F_SQXH='" + sqxh + "'");
                    try
                    {
                        dt_sqd = aa.GetDataTable("select * from T_SQD where F_SQXH='" + sqxh + "'", "dt_sqd");
                    }
                    catch (Exception ex)
                    {
                        log.WriteMyLog(ex.Message);
                        return;
                    }
                    if (dt_sqd.Rows.Count > 0)
                    {
                        if (dt_sqd.Rows[0]["F_sqdzt"].ToString().Trim() != "已登记")
                            aa.ExecuteSQL("update T_SQD set F_sqdzt='已登记' where F_sqxh='" + sqxh + "'");
                    }
                    ////非体检病人回写
                    ////生成pdf 用于移动app
                    string PdfToBase64String = "";
                    if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1" && bgzt == "已审核")
                    {
                        log.WriteMyLog(blh + ",开始生成体检PDF");
                        CreatePDF(blh, bgxh, bglx, jcxx, true, ref PdfToBase64String, debug);
                    }

                    if (bgzt == "取消审核")
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "'");


                    if (hczt == "1" && sqxh != "")
                    {
                        if (bglx == "cg" && (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已审核"))
                        {
                            TJ_ZtToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                        }
                    }
                    if (hcbg == "1" && (bgzt == "已审核" || bgzt == "已发布"))
                    {
                        TJ_BgToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug, PdfToBase64String);
                        return;
                    }
                    else
                    {
                        if (bgzt == "取消审核")
                        {
                            TJ_BgHSToPt(jcxx, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                        }
                    }
                }
                return;
            }
            else
            {
                string ptjk = f.ReadString("zgqjk", "ptjk", "1").Trim();
                string hisjk = f.ReadString("zgqjk", "hisjk", "0").Trim();

                try
                {
                    dt_sqd = aa.GetDataTable("select * from T_SQD where F_SQXH='" + sqxh + "'", "dt_sqd");
                }
                catch (Exception ex)
                {
                    log.WriteMyLog(ex.Message.ToString());
                    return;
                }
                if (dt_sqd.Rows.Count > 0)
                {
                    if (dt_sqd.Rows[0]["F_sqdzt"].ToString().Trim() != "已登记")
                        aa.ExecuteSQL("update T_SQD set F_sqdzt='已登记' where F_sqxh='" + sqxh + "'");
                }
                ////非体检病人回写
                ////生成pdf 用于移动app
                string PdfToBase64String = "";
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1" && bgzt == "已审核")
                {
                    CreatePDF(blh, bgxh, bglx, jcxx, true, ref PdfToBase64String, debug);
                }

                if (bgzt == "取消审核")
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");

                if (ptjk == "1")
                {
                    if (hczt == "1" && sqxh != "")
                    {
                        if (bglx == "cg" && (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已审核"))
                        {
                            ZtToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug, PdfToBase64String);
                        }
                    }
                    if (hcbg == "1" && (bgzt == "已审核" || bgzt == "已发布"))
                    {
                        string bgzt2 = "";
                        try
                        {
                            if (bglx.ToLower().Trim() == "bd")
                            {
                                dt_bd =
                                    aa.GetDataTable(
                                        "select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'",
                                        "bd");
                                bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                            }
                            if (bglx.ToLower().Trim() == "bc")
                            {
                                dt_bc =
                                    aa.GetDataTable(
                                        "select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'",
                                        "bc");
                                bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                            }
                            if (bglx.ToLower().Trim() == "cg")
                                bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                        }
                        catch
                        {
                        }

                        BgToPt(jcxx, dt_bc, dt_bd, dt_sqd, blh, bglx, bgxh, bgzt2, yhmc, yhbh, debug, PdfToBase64String);
                        return;
                    }
                    else
                    {
                        if (bgzt == "取消审核")
                        {
                            BgHSToPt(jcxx, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                        }
                    }
                }
            }
        }

        public void GetDisease(string blh, string bglx, string blk)
        {
            var getDisease = f.ReadString("savetohis", "xzjb", "0");

            if (getDisease == "1" && bglx == "cg")
            {
                var frm = new DiseaseSelector();
                frm.F_BLK = blk;
                var r = frm.ShowDialog(Application.OpenForms?[0]);
                if (r != DialogResult.OK) //用户取消了选择
                    return;
                var disease = frm.SelectedItem;
                if (disease == null)
                    return;

                //插入数据库
                string delSql = $" delete t_jcxx_zdjb where t.f_blh = '{blh}' ";
                string insertSql = $@"INSERT INTO [dbo].[T_JCXX_ZDJB]
                                       ([F_BLH]
                                       ,[F_Disease_Name]
                                       ,[F_Disease_Code])
                                 VALUES
                                       ('{blh}'
                                       ,'{disease.CYC_MC}'
                                       ,'{disease.ZJC2}') ";
                aa.ExecuteSQL(delSql);
                aa.ExecuteSQL(insertSql);
            }
        }

        public void ZtToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt,
            string yhmc, string yhbh, string debug, string pdfToBase64String)
        {
            if (bglx != "cg")
                return;
            if (dt_sqd.Rows.Count <= 0)
            {
                log.WriteMyLog(blh + ",申请表中无记录");
                return;
            }
            string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.154/MQService/MQService.asmx");
            string GUID = "";
            string bgztxml = ZtMsg(dt_jcxx, dt_sqd, dt_jcxx.Rows[0]["F_SQXH"].ToString(), blh, bglx, bgxh, bgzt, yhmc,
                yhbh, ref GUID);
            if (bgztxml.Trim() == "")
            {
                log.WriteMyLog(blh + ",报告状态生成xml为空");
                return;
            }
            if (debug == "1")
                log.WriteMyLog("状态[QI1_038]:" + bgztxml);
            try
            {
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                MQSer.Url = wsurl;
                string msgtxt = "";
                if (MQSer.SendMessageToMQ(bgztxml, ref msgtxt, "QI1_038", "PISFZ_ExamState", GUID, "返回状态"))
                {
                    if (debug == "1")
                        log.WriteMyLog(blh + ",状态[" + bgzt + "]发送成功：" + msgtxt);
                }
                else
                    log.WriteMyLog(blh + ",状态[" + bgzt + "]发送失败：" + msgtxt);
            }
            catch (Exception ee4)
            {
                log.WriteMyLog(blh + ",状态[" + bgzt + "]发送异常：" + ee4.Message);
            }

            return;
        }

        public void BgToPt(DataTable dt_jcxx, DataTable dt_bc, DataTable dt_bd, DataTable dt_sqd, string blh,
            string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug, string pdfToBase64String)
        {
            try
            {
                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;
                string url = f.ReadString("savetohis", "blweb", "http://172.16.95.154/pathwebrpt");
                string patientSerialNO = "";
                string jzlb = "1";
                string brlb = dt_jcxx.Rows[0]["F_brlb"].ToString();

                if (dt_sqd.Rows.Count < 1)
                {
                    if (brlb == "住院")
                        patientSerialNO = dt_jcxx.Rows[0]["F_ZYH"].ToString().Trim();
                    else
                        patientSerialNO = dt_jcxx.Rows[0]["F_MZH"].ToString().Trim();
                    if (brlb == "住院")
                        jzlb = "2";
                    else if (brlb == "体检")
                        jzlb = "3";
                    else
                        jzlb = "1";
                }
                else
                {
                    patientSerialNO = dt_sqd.Rows[0]["F_JZH"].ToString().Trim();
                    jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString().Trim();
                }
                DataTable dt_tx = new DataTable();
                try
                {
                    dt_tx = aa.GetDataTable("select * from T_tx where F_BLH='" + blh + "' and F_SFDY='1'", "dt_sqd");
                }
                catch (Exception ex)
                {
                }

                DataTable dt_pdf = new DataTable();
                try
                {
                    dt_pdf = aa.GetDataTable("select * from T_BG_PDF where F_BLBH='" + blbh + "'", "dt_sqd");
                }
                catch (Exception ex)
                {
                    log.WriteMyLog(ex.Message.ToString());
                }
                string filePath = "";
                if (dt_pdf.Rows.Count > 0)
                {
                    filePath = dt_pdf.Rows[0]["F_pdfPath"].ToString().Replace("ftp", "http");
                }
                string GUID = "";
                string rtnxml = BgMsg(dt_jcxx, dt_bc, dt_bd, dt_tx, dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim(), blh,
                    bglx, bgxh, bgzt, jzlb, yhmc, yhbh, filePath, url, ref GUID, pdfToBase64String);
                if (rtnxml.Trim() == "")
                {
                    log.WriteMyLog(blbh + ",生成XML失败:空");
                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='生成XML失败:空' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    return;
                }
                string msgtxt = "";
                try
                {
                    if (debug == "1")
                        log.WriteMyLog("回传报告[QI1_002-PISFZ_Report]:" + rtnxml);

                    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.154/MQService/MQService.asmx");
                    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                    MQSer.Url = wsurl;
                    if (MQSer.SendMessageToMQ(rtnxml, ref msgtxt, "QI1_002", "PISFZ_Report", GUID, "报告发布"))
                    //PISFZ_Report
                    {
                        if (debug == "1")
                            log.WriteMyLog(blbh + ",发送成功:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送成功',F_FSZT='已处理' where F_blbh='" +
                                      blbh + "' and F_BGZT='已审核'");
                    }
                    else
                    {
                        log.WriteMyLog(blbh + ",发送失败:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送失败：" + msgtxt +
                                      "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    }
                }
                catch (Exception ee4)
                {
                    log.WriteMyLog(blbh + ",发送异常：" + ee4.Message);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送异常：" + ee4.Message +
                                  "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                }
            }
            catch (Exception ee5)
            {
                log.WriteMyLog(ee5.Message);
            }
            return;
        }

        public string BgMsg(DataTable dt_jcxx, DataTable dt_bc, DataTable dt_bd, DataTable dt_tx, string sqxh,
            string blh, string bglx, string bgxh, string bgzt, string jzlb, string yhmc, string yhbh, string filePath,
            string url, ref string GUID, string pdfToBase64String)
        {
            DataRow drTbsReport = null;
            var dtTbsReport = aa.GetDataTable($" select * from t_tbs_bg t where t.f_blh='{blh}' ", "table1");
            if (dtTbsReport.Rows.Count > 0)
                drTbsReport = dtTbsReport.Rows[0];

            filePath = filePath.ToLower().Replace("ftp", "http");
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            GUID = Guid.NewGuid().ToString();
            //生成xml
            string rtnxml = "<?xml version=\"1.0\"?>";
            rtnxml = rtnxml + "<PRPA_IN000005UV01>";
            try
            {
                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml +
                         "<softwareName code=\"CDR\" displayName=\"临床数据中心CDR\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</receiver>";

                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml +
                         "<softwareName code=\"PISFZ\" displayName=\"分子诊断信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</sender>";

                rtnxml = rtnxml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";

                rtnxml = rtnxml + "<authorOrPerformer typeCode=\"AUT\">";
                rtnxml = rtnxml + "<signatureText></signatureText>";
                rtnxml = rtnxml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                rtnxml = rtnxml + "</authorOrPerformer>";
                rtnxml = rtnxml + "<subject typeCode=\"SUBJ\">";

                rtnxml = rtnxml + "<request>";
                ////!--报告流水号-->
                rtnxml = rtnxml + "<flowID>" + blbh + "</flowID>";
                ////<!--报告类型 - 超声、放射、心电等-->
                rtnxml = rtnxml + "<adviceType>分子诊断</adviceType>";
                ////<!--HIS病人类型 门诊 住院-->
                rtnxml = rtnxml + "<patienttype>" + GetBrlbCode(dt_jcxx) + "</patienttype>";
                ////<!-- 文档注册时间 -->
                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                ////<!-- 患者号 -->
                rtnxml = rtnxml + "<patientNo>" + dt_jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
                ////<!-- 患者就诊流水号 -->
                rtnxml = rtnxml + "<patientSerialNO>" + dt_jcxx.Rows[0]["F_ZY"].ToString() + "</patientSerialNO>";
                ////<!--HIS的医嘱流水号,标识一次申请-->
                rtnxml = rtnxml + "<adviceID>" + "" + "</adviceID>";
                ////<!--的对应的申请单唯一标识号，标识一次检查-->
                rtnxml = rtnxml + "<accessionNO>" + dt_jcxx.Rows[0]["F_sqxh"].ToString() + "</accessionNO>";
                ////<!--患者中文名-->
                rtnxml = rtnxml + "<patientName>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</patientName>";
                ////<!--患者拼音名-->
                rtnxml = rtnxml + "<patientNameSpell></patientNameSpell>";
                ////<!--出生日期(例如：‘19870601’)-->
                rtnxml = rtnxml + "<birthDate></birthDate>";
                ////<!--性别-->
                string xb = dt_jcxx.Rows[0]["F_xb"].ToString();
                if (xb == "男") xb = "1";
                else if (xb == "女") xb = "2";
                else xb = "0";
                rtnxml = rtnxml + "<sex>" + xb + "</sex>";
                ////	<!--检查项目代码-->

                if (dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim().Contains("^"))
                {
                    rtnxml = rtnxml + "<procedureCode>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] +
                             "</procedureCode>";
                    try
                    {
                        rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] +
                                 "</procedureName>";
                    }
                    catch
                    {
                        rtnxml = rtnxml + "<procedureName></procedureName>";
                    }
                }
                else
                {
                    rtnxml = rtnxml + "<procedureCode>" + "" + "</procedureCode>";
                    rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() +
                             "</procedureName>";
                }
                ////	<!--检查部位-->
                rtnxml = rtnxml + "<positionName>" + dt_jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] +
                         "</positionName>";
                ////	<!--检查设备名称-->
                var sbmc = "";
                if (drTbsReport != null) sbmc = drTbsReport["f_FZ_JCyq"].ToString().TrimEnd('。');
                rtnxml = rtnxml + $"<modalityName>{sbmc}</modalityName>";
                ////	<!--报告类别，如CT，MRI等-->

                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
                ////	<!-- 报告医生编码 -->
                var dtYhbm =
                    aa.GetDataTable($" select t.f_yhbh from t_yh t where t.f_yhmc='{dt_jcxx.Rows[0]["F_shys"]}' ",
                        "table1");
                var yhbm = "";
                if (dtYhbm.Rows.Count > 0)
                    yhbm = dtYhbm.Rows[0]["f_yhbh"].ToString();

                rtnxml = rtnxml + $"<authorCode>{yhbm}</authorCode>";
                ////	<!--报告医生-->
                if (bglx == "bc")
                {
                    rtnxml = rtnxml + "<authorName>" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" +
                             DateTime.Parse(dt_bc.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss") +
                             "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_bc.Rows[0]["F_bc_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" +
                             DateTime.Parse(dt_bc.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss") +
                             "</approverDateTime>";
                    ////	<!--肉眼所见-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() +
                             "]]></nakedEyeDiagnose>";
                    ////<!--镜下所见-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_JXSJ"].ToString() +
                             "]]></microscopeDiagnose>";
                    ////	<!--报告结论-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bc.Rows[0]["F_BCZD"].ToString() +
                             "]]></conclusionDiagnose>";
                    ////	<!--报告所见-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_tsjc"].ToString() +
                             "]]></reportDiagnose>";
                }
                else if (bglx == "bd")
                {
                    rtnxml = rtnxml + "<authorName>" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" +
                             DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") +
                             "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_bd.Rows[0]["F_bd_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" +
                             DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") +
                             "</approverDateTime>";
                    ////	<!--肉眼所见-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() +
                             "]]></nakedEyeDiagnose>";
                    ////<!--镜下所见-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[]]></microscopeDiagnose>";
                    ////	<!--报告结论-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bd.Rows[0]["F_BDZD"].ToString() +
                             "]]></conclusionDiagnose>";
                    ////	<!--报告所见-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[]]></reportDiagnose>";
                }
                else
                {
                    rtnxml = rtnxml + "<authorName>" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" +
                             DateTime.Parse(dt_jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") +
                             "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" +
                             DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") +
                             "</approverDateTime>";
                    ////	<!--肉眼所见-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() +
                             "]]></nakedEyeDiagnose>";
                    ////<!--镜下所见-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_JXSJ"].ToString() +
                             "]]></microscopeDiagnose>";
                    ////	<!--报告结论-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_BLZD"].ToString() +
                             "]]></conclusionDiagnose>";
                    ////	<!--报告所见-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_tsjc"].ToString() +
                             "]]></reportDiagnose>";
                }

                ////	<!-- 医生所属科室编码 -->
                rtnxml = rtnxml + $"<deptCode>{f.ReadString("savetohis", "bgksbm", "").Trim()}</deptCode>";

                ////	<!-- 医生所属科室 -->
                rtnxml = rtnxml + "<deptName>分子诊断科</deptName>";

                ////	<!--报告所得-->
                rtnxml = rtnxml + "<reportContent></reportContent>";
                ////	<!-- 来源系统编码 -->
                rtnxml = rtnxml + "<sourceCode>PISFZ</sourceCode>";
                ////	<!-- 来源系统简称 -->
                rtnxml = rtnxml + "<sourceName>PISFZ</sourceName>";
                rtnxml = rtnxml + "<providerOrganization>42321679-4</providerOrganization>";
                ////	<!-- 文档编号 -->
                rtnxml = rtnxml + "<indexInSystem>" + blbh + "</indexInSystem> ";
                ////	<!-- 文档类型编码 -->
                rtnxml = rtnxml + "<typeCode></typeCode>";
                ///	<!-- 文档类型名称 -->
                rtnxml = rtnxml + "<typeCodeName></typeCodeName>";
                ////	<!-- 文档标题 -->
                rtnxml = rtnxml + "<title></title>";
                rtnxml = rtnxml + "<reportListURL>" + filePath + "</reportListURL>";
                ////	<!--影像调阅URL-->
                rtnxml = rtnxml + "<imageList>";
                try
                {
                    for (int j = 0; j < dt_tx.Rows.Count; j++)
                    {
                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + dt_jcxx.Rows[0]["F_txml"].ToString() + "/" +
                                 dt_tx.Rows[j]["F_TXM"] + "</imageURL>";
                    }
                }
                catch
                {
                }
                rtnxml = rtnxml + "</imageList>";
                ////	<!--备用字段1-->";
                rtnxml = rtnxml + "<other1></other1>";
                ////	<!--备用字段2-->
                rtnxml = rtnxml + "<other2></other2>";
                ////	<!--备用字段3-->
                rtnxml = rtnxml + "<other3></other3>";
                ////	<!--结果更新标志；0-PACS新增，1-电子病历读取，2-PACS修改-->
                rtnxml = rtnxml + "<updateFlag></updateFlag>";

                rtnxml += $@"   	<!-- 非数值型 -->
	                                <UNStruct>
	                                <!-- 如有多条项目则多个Item -->
	                                <Item>
		                                <!-- 项目编码 -->
		                                <itemCode/>
		                                <!-- 项目名称 -->
		                                <itemName>{dt_jcxx.Rows[0]["f_yzxm"]}</itemName>
		                                <!-- 监测结果 请用<![CDATA[]]>包括  -->
		                                <result><![CDATA[{dt_jcxx.Rows[0]["f_blzd"]}]]></result>
		                                <!-- 监测结果单位 请用<![CDATA[]]>包括  -->
		                                <resultUnit><![CDATA[]]></resultUnit>
		                                <!-- 参考值 请用<![CDATA[]]>包括 如为范围请用英文半角字符 / 隔开 如>100或<200或100/200 -->
                                        <reference ><![CDATA[]]></reference>
                                        <!--参考值单位 请用 <![CDATA[]]> 包括-->
                                        <referenceUnit><![CDATA[]]></referenceUnit>
                                        <!--其他-->
                                        <remark />
                                     </Item >
                                     </UNStruct> 
                                     ";
                ////	<!-- 报告PDA文档二进制串 BASE64 -->
                rtnxml = rtnxml + "<body>";
                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + pdfToBase64String +
                         "</text>";
                rtnxml = rtnxml + "</body>";
                rtnxml = rtnxml + "</request>";
                rtnxml = rtnxml + "</subject>";
                rtnxml = rtnxml + "</controlActProcess>";
                rtnxml = rtnxml + "</PRPA_IN000005UV01>";

                return rtnxml;
            }
            catch (Exception e2)
            {
                log.WriteMyLog(blbh + ",报告生成XML异常：" + e2.Message);
                return "";
            }
        }

        public string ZtMsg(DataTable dt_jcxx, DataTable dt_sqd, string sqxh, string blh, string bglx, string bgxh,
            string bgzt, string yhmc, string yhbh, ref string GUID)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml +
                      "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"COMT_IN001103UV01\"/>";
                xml = xml + "<processingCode code=\"T\"/>";
                xml = xml + "<processingModeCode code=\"I\"/>";
                xml = xml + "<acceptAckCode code=\"AA\"/>";

                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- 可以填写电话信息或者URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"PISFZ\" displayName=\"分子诊断信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<signatureText></signatureText>";
                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<subject typeCode=\"SUBJ\">";
                xml = xml + "<actGenericStatus classCode=\"DOCCLIN\" moodCode=\"EVN\">";
                xml = xml + "<!--业务活动ID（报告ID）-->";
                xml = xml + "<id root=\"2.16.156.10011.1.1\" extension=\"" + blbh + "\"/>";
                xml = xml + "<!--业务活动类别 状态名称-->";

                if (bgzt == "已审核")
                    xml = xml +
                          "<code code=\"60\" displayName=\"报告发布\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else if (bgzt == "已写报告")
                    xml = xml +
                          "<code code=\"50\" displayName=\"完成\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else
                    xml = xml +
                          "<code code=\"40\" displayName=\"执行\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";

                xml = xml + "<!--业务活动状态 全为completed-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!--业务活动期间-->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                xml = xml + "<!--执行开始时间-->";
                xml = xml + "<low value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<!--执行结束时间-->";
                xml = xml + "<high value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "</effectiveTime>";
                xml = xml + "<!--执行者0..*-->";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!--医务人员ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\"" + yhbh + "\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + yhmc + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!--医疗卫生机构（科室）ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                xml = xml + "<name>分子诊断科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<!--所执行申请单或医嘱-->";
                xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                xml = xml + "<!--电子申请单编号-->";
                xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" + sqxh + "\"/>";
                xml = xml + "<!--医嘱ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + "" + "\"/>";
                xml = xml + "</actIntent>";
                xml = xml + "</inFulfillmentOf>";
                xml = xml + "<componentOf typeCode=\"COMP\" xsi:nil=\"false\">";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!--门(急)诊流水号-->";
                xml = xml + "<id root=\"2.999.1.91\" extension=\"" + dt_sqd.Rows[0]["F_MZLSH"].ToString() + "\"/>";
                xml = xml + "<!--住院流水号 -->";
                xml = xml + "<id root=\"2.999.1.42\" extension=\"" + dt_sqd.Rows[0]["F_ZYLSH"].ToString() + "\"/>";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_JZLB"].ToString() +
                      "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"患者类型代码表\" displayName=\"" +
                      dt_jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                xml = xml + "<statusCode/>";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<!--平台注册的患者ID -->";
                xml = xml + "<id root=\"2.999.1.37\" extension=\"" + dt_sqd.Rows[0]["F_EMPIID"].ToString() + "\"/>";
                xml = xml + "<!--本地系统的患者ID -->";
                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_patientid"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_ZLH"].ToString() +
                      "\"/>";
                xml = xml + "<statusCode/>";
                xml = xml + "<patientPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 患者姓名  -->";
                xml = xml + "<name>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "</patientPerson>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf>";
                xml = xml + "</actGenericStatus>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</COMT_IN001103UV01>";
                return xml;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("报告状态生成XML异常：" + ee.Message);
                return "";
            }
        }

        public void BgHSToPt(DataTable dt_jcxx, string blh, string bglx, string bgxh, string bgzt, string yhmc,
            string yhbh, string debug)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            string GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<?xml version=\"1.0\"?>";
                xml = xml + "<PRPA_IN000003UV04> ";
                xml = xml + "<!-- UUID,交互实例唯一码-->";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId extension=\"PRPA_IN000003UV04\" root=\"2.16.840.1.113883.1.6\"/>";


                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- 可以填写电话信息或者URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"CDR\" displayName=\"临床数据中心CDR\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"PISFZ\" displayName=\"分子诊断信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                xml = xml +
                      "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

                xml = xml + "<!--报告流水号-->";
                xml = xml + "<flowID>" + blbh + "</flowID>";
                xml = xml + "<!--报告类型 - 超声、放射、心电等-->";
                xml = xml + "<adviceType>分子诊断</adviceType>";
                xml = xml + "<!--HIS病人类型 门诊 住院（必填）-->";
                xml = xml + "<patienttype>" + GetBrlbCode(dt_jcxx) + "</patienttype>";
                xml = xml + "<!-- 文档注册时间 -->";
                xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                xml = xml + "<!-- 来源系统编码（必填）2017年8月24日 替换为取消审核人编码 -->";
                xml = xml + $"<sourceCode>{f.ReadString("yh", "yhbh", "").Trim()}</sourceCode>";
                xml = xml + "<!-- 来源系统简称 2017年8月24日 替换为取消审核人名称 -->";
                xml = xml + $"<sourceName>{f.ReadString("yh", "yhmc", "").Trim()}</sourceName>";
                xml = xml + "<!-- 文档编号（必填） -->";
                xml = xml + "<indexInSystem>" + blbh + "</indexInSystem>";

                xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
            }
            catch (Exception ee4)
            {
                log.WriteMyLog(blbh + ",报告召回XML异常：" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='召回XML异常：" + ee4.Message + "' where F_blbh='" + blbh +
                              "' and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }
            if (xml.Trim() == "")
            {
                log.WriteMyLog(blbh + ",报告召回生成xml为空");
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告召回xml为空' where F_blbh='" + blbh +
                              "' and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }

            string msgtxt = "";
            try
            {
                if (debug == "1")
                    log.WriteMyLog("报告召回：[QI1_002--PISFZ_ReportExpired]" + xml);

                string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.154/MQService/MQService.asmx");
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();

                MQSer.Url = wsurl;


                if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_002", "PISFZ_ReportExpired", GUID, "报告回收"))
                {
                    if (debug == "1")
                    {
                        log.WriteMyLog(blbh + ",报告召回成功：" + msgtxt);
                    }
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回成功:" + msgtxt +
                                  "',F_FSZT='已处理' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                    return;
                }
                else
                {
                    log.WriteMyLog(blbh + ",报告召回失败：" + msgtxt);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回失败:" + msgtxt + "' where F_blbh='" +
                                  blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                    return;
                }
            }
            catch (Exception ee4)
            {
                log.WriteMyLog(blbh + ",报告召回异常：" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回异常:" + ee4.Message +
                              "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }
        }

        /// <summary>
        /// 通过病人类型获得病人编码
        /// </summary>
        /// <param name="dt_jcxx"></param>
        /// <returns></returns>
        private static string GetBrlbCode(DataTable dt_jcxx)
        {
            if (dt_jcxx != null && dt_jcxx.Rows.Count > 0)
            {
                string brlb = "";
                if (dt_jcxx.Columns.Contains("F_BRLB"))
                    brlb = dt_jcxx.Rows[0]["F_BRLB"].ToString();
                else if (dt_jcxx.Columns.Contains("F_brlb"))
                    brlb = dt_jcxx.Rows[0]["F_brlb"].ToString();

                switch (brlb)
                {
                    case "门诊":
                        return "1";
                        break;
                    case "住院":
                        return "3";
                        break;
                    default:
                        return "9";
                        break;
                }
            }
            return 9.ToString();
        }

        public void CreatePDF(string blh, string bgxh, string bglx, DataTable dt_jcxx, bool isToBase64String,
            ref string PdfToBase64String, string debug)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;

            #region  生成pdf

            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
            {
                string ReprotFile = "";
                string pdfName = "";
                string ML = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                string message = "";
                DateTime dateStamp = DateTime.Now;

                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref pdfName, "", "", ref message,
                    dateStamp);

                //   MessageBox.Show(pdfName);

                #region 图片生成处理

                //pdf转图片
                var jpgName = blh + "_" + bglx + bgxh + "_" + dateStamp.ToString("yyyyMMddHHmmss");
                try
                {
                    Converer.ConvertPDF2Image(pdfName, @"c:\temp\", jpgName, 1, 100, ImageFormat.Jpeg,
                        Converer.Definition.Three,
                        true);
                }
                catch (Exception e)
                {
                    MessageBox.Show(
                        "生成图片报告时出现异常:" + e);
                }

                //上传图片
                string jpgPath = "";
                bool success = zgq.UpPDF(blh, @"c:\temp\" + jpgName + ".jpeg", ML, ref message, 3, ref jpgPath);
                if (!success)
                    log.WriteMyLog("上传JPG失败:" + @"c:\temp\" + jpgName + "------" + message);

                //上传完毕后删除文件
                try
                {
                    File.Delete(@"c:\temp\" + jpgName + ".jpeg");
                }
                catch (Exception e)
                {
                    log.WriteMyLog("删除临时报告图片失败:" + e);
                }

                #endregion

                if (isrtn)
                {
                    if (debug == "1")
                        log.WriteMyLog("生成PDF成功");

                    if (isToBase64String)
                    {
                        try
                        {
                            FileStream file = new FileStream(pdfName, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length]; //把pdf转成 Byte型 二进制流   
                            file.Read(imgByte, 0, imgByte.Length); //把二进制流读入缓冲区   
                            file.Close();
                            PdfToBase64String = Convert.ToBase64String(imgByte);
                            if (debug == "1")
                                log.WriteMyLog("PDF转换二进制串成功");
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("PDF转换二进制串失败:" + ee.Message);
                        }
                    }

                    string pdfpath = "";
                    bool ssa = zgq.UpPDF(blh, pdfName, ML, ref message, 3, ref pdfpath);


                    if (ssa == true)
                    {
                        if (debug == "1")
                            log.WriteMyLog("上传PDF成功");

                        pdfName = pdfName.Substring(pdfName.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "上传PDF成功", "审核", "上传PDF成功:" + pdfpath, "ZGQJK", "上传PDF");
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                        aa.ExecuteSQL(
                            "insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" +
                            blbh +
                            "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + pdfName +
                            "','" +
                            pdfpath + "')");
                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("上传PDF失败" + message);
                        ZgqClass.BGHJ(blh, "上传PDF失败", "审核", message, "ZGQJK", "上传PDF");
                    }
                }
                else
                {
                    if (debug == "1")
                        log.WriteMyLog("生成PDF失败" + message);
                    ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "生成pdf失败" + message, "ZGQJK", "生成PDF");
                }
                zgq.DelTempFile(blh);
            }

            #endregion
        }

        public void TJ_ZtToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt,
            string yhmc, string yhbh, string debug)
        {
            if (bglx != "cg")
                return;
            if (dt_sqd.Rows.Count <= 0)
            {
                log.WriteMyLog(blh + ",申请表中无记录");
                return;
            }
            string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.154/MQService/MQService.asmx");
            string GUID = "";
            string bgztxml = TJ_ZtMsg(dt_jcxx, dt_sqd, dt_jcxx.Rows[0]["F_SQXH"].ToString(), blh, bglx, bgxh, bgzt, yhmc,
                yhbh, ref GUID);
            if (bgztxml.Trim() == "")
            {
                log.WriteMyLog(blh + ",报告状态生成xml为空");
                return;
            }
            if (debug == "1")
                log.WriteMyLog("状态[QI1_073]:" + bgztxml);
            try
            {
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                MQSer.Url = wsurl;
                string msgtxt = "";
                log.WriteMyLog("开始返回体检报告到平台:" + bgztxml);
                if (MQSer.SendMessageToMQ(bgztxml, ref msgtxt, "QI1_073", "PIS_State_PE", GUID, "返回状态"))
                {
                    if (debug == "1")
                        log.WriteMyLog(blh + ",状态[" + bgzt + "]发送成功：" + msgtxt);
                }
                else
                    log.WriteMyLog(blh + ",状态[" + bgzt + "]发送失败：" + msgtxt);
            }
            catch (Exception ee4)
            {
                log.WriteMyLog(blh + ",状态[" + bgzt + "]发送异常：" + ee4.Message);
            }

            return;
        }

        public void TJ_BgToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt,
            string yhmc, string yhbh, string debug, string pdfbase64String)
        {
            try
            {
                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;
                if (bglx != "cg")
                    return;
                string url = f.ReadString("savetohis", "blweb", "http://172.16.95.154/pathwebrpt");
                string patientSerialNO = "";
                string jzlb = "1";
                string brlb = dt_jcxx.Rows[0]["F_brlb"].ToString();

                if (dt_sqd.Rows.Count < 1)
                {
                    if (brlb == "住院")
                        patientSerialNO = dt_jcxx.Rows[0]["F_ZYH"].ToString().Trim();
                    else
                        patientSerialNO = dt_jcxx.Rows[0]["F_MZH"].ToString().Trim();
                    if (brlb == "住院")
                        jzlb = "2";
                    else
                        jzlb = "1";
                }
                else
                {
                    patientSerialNO = dt_sqd.Rows[0]["F_JZH"].ToString().Trim();
                    jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString().Trim();
                }
                DataTable dt_tx = new DataTable();
                try
                {
                    dt_tx = aa.GetDataTable("select * from T_tx where F_BLH='" + blh + "' and F_SFDY='1'", "dt_sqd");
                }
                catch (Exception ex)
                {
                }

                DataTable dt_pdf = new DataTable();
                try
                {
                    dt_pdf = aa.GetDataTable("select * from T_BG_PDF where F_BLBH='" + blbh + "'", "dt_sqd");
                }
                catch (Exception ex)
                {
                    log.WriteMyLog(ex.Message.ToString());
                }
                string filePath = "";
                if (dt_pdf.Rows.Count > 0)
                {
                    filePath = dt_pdf.Rows[0]["F_pdfPath"].ToString().Replace("ftp", "http");
                }
                string GUID = "";
                string rtnxml = TJ_BgMsg(dt_jcxx, dt_tx, dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim(), blh, bglx, bgxh,
                    bgzt, jzlb, yhmc, yhbh, filePath, url, ref GUID, pdfbase64String);
                if (rtnxml.Trim() == "")
                {
                    log.WriteMyLog(blbh + ",生成XML失败:空");
                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='生成XML失败:空' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    return;
                }
                string msgtxt = "";
                try
                {
                    if (debug == "1")
                        log.WriteMyLog("回传报告[QI1_002-SFZ_Report]:" + rtnxml);

                    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.154/MQService/MQService.asmx");
                    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                    MQSer.Url = wsurl;
                    if (MQSer.SendMessageToMQ(rtnxml, ref msgtxt, "QI1_081", "PIS_Report_PE", GUID, "报告发布")) //PIS_Report
                    {
                        if (debug == "1")
                            log.WriteMyLog(blbh + ",发送成功:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送成功',F_FSZT='已处理' where F_blbh='" +
                                      blbh + "' and F_BGZT='已审核'");
                    }
                    else
                    {
                        log.WriteMyLog(blbh + ",发送失败:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送失败：" + msgtxt +
                                      "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    }
                }
                catch (Exception ee4)
                {
                    log.WriteMyLog(blbh + ",发送异常：" + ee4.Message);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送异常：" + ee4.Message +
                                  "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                }
            }
            catch (Exception ee5)
            {
                log.WriteMyLog(ee5.Message);
            }
            return;
        }

        /// <summary>
        /// 获取报告xml
        /// </summary>
        /// <param name="dt_jcxx"></param>
        /// <param name="dt_tx"></param>
        /// <param name="sqxh"></param>
        /// <param name="blh"></param>
        /// <param name="bglx"></param>
        /// <param name="bgxh"></param>
        /// <param name="bgzt"></param>
        /// <param name="jzlb"></param>
        /// <param name="yhmc"></param>
        /// <param name="yhbh"></param>
        /// <param name="filePath"></param>
        /// <param name="url"></param>
        /// <param name="GUID"></param>
        /// <returns></returns>
        public string TJ_BgMsg(DataTable dt_jcxx, DataTable dt_tx, string sqxh, string blh, string bglx, string bgxh,
            string bgzt, string jzlb, string yhmc, string yhbh, string filePath, string url, ref string GUID,
            string pdfbase64String)
        {
            DataRow drTbsReport = null;
            var dtTbsReport = aa.GetDataTable($" select * from t_tbs_bg t where t.f_blh='{blh}' ", "table1");
            if (dtTbsReport.Rows.Count > 0)
                drTbsReport = dtTbsReport.Rows[0];

            filePath = filePath.ToLower().Replace("ftp", "http");
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            GUID = Guid.NewGuid().ToString();
            //生成xml
            string rtnxml = "<?xml version=\"1.0\"?>";
            rtnxml = rtnxml + "<PRPA_IN000003UV01>";
            try
            {
                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml +
                         "<softwareName code=\"CDR\" displayName=\"临床数据中心CDR\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</receiver>";

                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml +
                         "<softwareName code=\"PISFZ\" displayName=\"分子诊断信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</sender>";

                rtnxml = rtnxml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";

                rtnxml = rtnxml + "<authorOrPerformer typeCode=\"AUT\">";
                rtnxml = rtnxml + "<signatureText></signatureText>";
                rtnxml = rtnxml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                rtnxml = rtnxml + "</authorOrPerformer>";
                rtnxml = rtnxml + "<subject typeCode=\"SUBJ\">";

                rtnxml = rtnxml + "<request>";
                ////!--报告流水号-->
                rtnxml = rtnxml + "<flowID>" + blbh + "</flowID>";
                ////<!--报告类型 - 超声、放射、心电等-->
                rtnxml = rtnxml + "<adviceType>分子诊断</adviceType>";
                ////<!--HIS病人类型 门诊 住院-->
                rtnxml = rtnxml + "<patienttype>" + GetBrlbCode(dt_jcxx) + "</patienttype>";
                ////<!-- 文档注册时间 -->
                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                ////<!-- 患者号 -->
                rtnxml = rtnxml + "<patientNo>" + dt_jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
                ////<!-- 患者就诊流水号 -->
                rtnxml = rtnxml + "<patientSerialNO>" + dt_jcxx.Rows[0]["F_ZY"].ToString() + "</patientSerialNO>";
                ////<!--HIS的医嘱流水号,标识一次申请-->
                rtnxml = rtnxml + "<adviceID>" + "" + "</adviceID>";
                ////<!--的对应的申请单唯一标识号，标识一次检查-->
                rtnxml = rtnxml + "<accessionNO>" + dt_jcxx.Rows[0]["F_sqxh"].ToString() + "</accessionNO>";
                ////<!--患者中文名-->
                rtnxml = rtnxml + "<patientName>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</patientName>";
                ////<!--患者拼音名-->
                rtnxml = rtnxml + "<patientNameSpell></patientNameSpell>";
                ////<!--出生日期(例如：‘19870601’)-->
                rtnxml = rtnxml + "<birthDate></birthDate>";
                ////<!--性别-->
                string xb = dt_jcxx.Rows[0]["F_xb"].ToString();
                if (xb == "男") xb = "1";
                else if (xb == "女") xb = "2";
                else xb = "0";
                rtnxml = rtnxml + "<sex>" + xb + "</sex>";
                ////	<!--检查项目代码-->

                if (dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim().Contains("^"))
                {
                    rtnxml = rtnxml + "<procedureCode>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] +
                             "</procedureCode>";
                    try
                    {
                        rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] +
                                 "</procedureName>";
                    }
                    catch
                    {
                        rtnxml = rtnxml + "<procedureName></procedureName>";
                    }
                }
                else
                {
                    rtnxml = rtnxml + "<procedureCode>" + "" + "</procedureCode>";
                    rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() +
                             "</procedureName>";
                }
                ////	<!--检查部位-->
                rtnxml = rtnxml + "<positionName>" + dt_jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] +
                         "</positionName>";
                ////	<!--检查设备名称-->
                var sbmc = "";
                if (drTbsReport != null) sbmc = drTbsReport["f_FZ_JCyq"].ToString().TrimEnd('。');
                rtnxml = rtnxml + $"<modalityName>{sbmc}</modalityName>";
                ////	<!--报告类别，如CT，MRI等-->
                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
                ////	<!-- 报告医生编码 -->
                var dtYhbm =
                    aa.GetDataTable($" select t.f_yhbh from t_yh t where t.f_yhmc='{dt_jcxx.Rows[0]["F_shys"]}' ",
                        "table1");
                var yhbm = "";
                if (dtYhbm.Rows.Count > 0)
                    yhbm = dtYhbm.Rows[0]["f_yhbh"].ToString();
                rtnxml = rtnxml + $"<authorCode>{yhbm}</authorCode>";
                ////	<!--报告医生-->
                rtnxml = rtnxml + "<authorName>" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
                ////	<!--报告时间-->
                rtnxml = rtnxml + "<reportDateTime>" +
                         DateTime.Parse(dt_jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") +
                         "</reportDateTime>";
                ////	<!--审核医生-->
                rtnxml = rtnxml + "<reportApprover>" + dt_jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
                ////	<!--审核时间-->
                rtnxml = rtnxml + "<approverDateTime>" +
                         DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") +
                         "</approverDateTime>";

                DataTable TJ_bljc = new DataTable();
                TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                // 诊断描述
                string Res_char = dt_jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                //诊断结论	Res_con
                string Res_con = dt_jcxx.Rows[0]["F_blzd"].ToString().Trim();
                if (TJ_bljc.Rows.Count > 0)
                {
                    if (dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "体检LCT" ||
                        dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "液基细胞")
                    {
                        Res_char = Res_char + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" +
                                   "\r\n";

                        Res_char = Res_char + "项目：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" +
                                   TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                                   + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

                        Res_char = Res_char + "病原体：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" +
                                   TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
                                   + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" +
                                   TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

                        Res_char = Res_char + "炎症细胞量：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" +
                                   "\r\n";

                        ///////////诊断/////////////////////////
                        Res_con = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                        if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                            Res_con = Res_con + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                    }
                }

                ////	<!--肉眼所见-->
                rtnxml = rtnxml + "<nakedEyeDiagnose></nakedEyeDiagnose>";
                ////<!--镜下所见-->
                rtnxml = rtnxml + "<microscopeDiagnose></microscopeDiagnose>";
                ////	<!--报告结论-->
                rtnxml = rtnxml + "<conclusionDiagnose></conclusionDiagnose>";
                ////	<!--报告所见-->结果
                rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + Res_char + "]]></reportDiagnose>";

                ////	<!--报告所得-->结论
                rtnxml = rtnxml + "<reportContent><![CDATA[" + Res_con + "]]></reportContent>";
                ////	<!-- 医生所属科室编码 -->
                rtnxml = rtnxml + $"<deptCode>{f.ReadString("savetohis", "bgksbm", "").Trim()}</deptCode>";
                ////	<!-- 医生所属科室 -->
                rtnxml = rtnxml + "<deptName>分子诊断科</deptName>";


                ////	<!-- 来源系统编码 -->
                rtnxml = rtnxml + "<sourceCode>PISFZ</sourceCode>";
                ////	<!-- 来源系统简称 -->
                rtnxml = rtnxml + "<sourceName>PISFZ</sourceName>";
                rtnxml = rtnxml + "<providerOrganization>42321679-4</providerOrganization>";
                ////	<!-- 文档编号 -->
                rtnxml = rtnxml + "<indexInSystem>" + blbh + "</indexInSystem> ";
                ////	<!-- 文档类型编码 -->
                rtnxml = rtnxml + "<typeCode></typeCode>";
                ///	<!-- 文档类型名称 -->
                rtnxml = rtnxml + "<typeCodeName></typeCodeName>";
                ////	<!-- 文档标题 -->
                rtnxml = rtnxml + "<title></title>";
                rtnxml = rtnxml + "<reportListURL>" + filePath + "</reportListURL>";
                ////	<!--影像调阅URL-->
                rtnxml = rtnxml + "<imageList>";
                try
                {
                    for (int j = 0; j < dt_tx.Rows.Count; j++)
                    {
                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + dt_jcxx.Rows[0]["F_txml"].ToString() + "/" +
                                 dt_tx.Rows[j]["F_TXM"] + "</imageURL>";
                    }
                }
                catch
                {
                }
                rtnxml = rtnxml + "</imageList>";
                ////	<!--备用字段1-->";
                rtnxml = rtnxml + "<other1></other1>";
                ////	<!--备用字段2-->
                rtnxml = rtnxml + "<other2></other2>";
                ////	<!--备用字段3-->
                rtnxml = rtnxml + "<other3></other3>";
                ////	<!--结果更新标志；0-PACS新增，1-电子病历读取，2-PACS修改-->
                rtnxml = rtnxml + "<updateFlag></updateFlag>";

                ////	<!-- 报告PDA文档二进制串 BASE64 -->
                rtnxml = rtnxml + "<body>";
                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + pdfbase64String +
                         "</text>";
                rtnxml = rtnxml + "</body>";
                rtnxml = rtnxml + "</request>";
                rtnxml = rtnxml + "</subject>";
                rtnxml = rtnxml + "</controlActProcess>";
                rtnxml = rtnxml + "</PRPA_IN000003UV01>";

                return rtnxml;
            }
            catch (Exception e2)
            {
                log.WriteMyLog(blbh + ",报告生成XML异常：" + e2.Message);
                return "";
            }
        }

        public string TJ_ZtMsg(DataTable dt_jcxx, DataTable dt_sqd, string sqxh, string blh, string bglx, string bgxh,
            string bgzt, string yhmc, string yhbh, ref string GUID)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml +
                      "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"COMT_IN001103UV01\"/>";
                xml = xml + "<processingCode code=\"T\"/>";
                xml = xml + "<processingModeCode code=\"I\"/>";
                xml = xml + "<acceptAckCode code=\"AA\"/>";

                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- 可以填写电话信息或者URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"PISFZ\" displayName=\"分子诊断信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<signatureText></signatureText>";
                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<subject typeCode=\"SUBJ\">";
                xml = xml + "<actGenericStatus classCode=\"DOCCLIN\" moodCode=\"EVN\">";
                xml = xml + "<!--业务活动ID（报告ID）-->";
                xml = xml + "<id root=\"2.16.156.10011.1.1\" extension=\"" + blbh + "\"/>";
                xml = xml + "<!--业务活动类别 状态名称-->";

                if (bgzt == "已审核")
                    xml = xml +
                          "<code code=\"60\" displayName=\"报告发布\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else if (bgzt == "已写报告")
                    xml = xml +
                          "<code code=\"50\" displayName=\"完成\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else
                    xml = xml +
                          "<code code=\"40\" displayName=\"执行\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";

                xml = xml + "<!--业务活动状态 全为completed-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!--业务活动期间-->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                xml = xml + "<!--执行开始时间-->";
                xml = xml + "<low value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<!--执行结束时间-->";
                xml = xml + "<high value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "</effectiveTime>";
                xml = xml + "<!--执行者0..*-->";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!--医务人员ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\"" + yhbh + "\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + yhmc + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!--医疗卫生机构（科室）ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                xml = xml + "<name>分子诊断科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<!--所执行申请单或医嘱-->";
                xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                xml = xml + "<!--电子申请单编号-->";
                xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" + sqxh + "\"/>";
                xml = xml + "<!--医嘱ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + "" + "\"/>";
                xml = xml + "</actIntent>";
                xml = xml + "</inFulfillmentOf>";
                xml = xml + "<componentOf typeCode=\"COMP\" xsi:nil=\"false\">";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!--门(急)诊流水号-->";
                xml = xml + "<id root=\"2.999.1.91\" extension=\"" + dt_sqd.Rows[0]["F_MZLSH"].ToString() + "\"/>";
                xml = xml + "<!--住院流水号 -->";
                xml = xml + "<id root=\"2.999.1.42\" extension=\"" + dt_sqd.Rows[0]["F_ZYLSH"].ToString() + "\"/>";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_JZLB"].ToString() +
                      "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"患者类型代码表\" displayName=\"" +
                      dt_jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                xml = xml + "<statusCode/>";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<!--平台注册的患者ID -->";
                xml = xml + "<id root=\"2.999.1.37\" extension=\"" + dt_sqd.Rows[0]["F_EMPIID"].ToString() + "\"/>";
                xml = xml + "<!--本地系统的患者ID -->";
                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_patientid"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_ZLH"].ToString() +
                      "\"/>";
                xml = xml + "<statusCode/>";
                xml = xml + "<patientPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 患者姓名  -->";
                xml = xml + "<name>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "</patientPerson>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf>";
                xml = xml + "</actGenericStatus>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</COMT_IN001103UV01>";
                return xml;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("报告状态生成XML异常：" + ee.Message);
                return "";
            }
        }

        public void TJ_BgHSToPt(DataTable dt_jcxx, string blh, string bglx, string bgxh, string bgzt, string yhmc,
            string yhbh, string debug)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            string GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<?xml version=\"1.0\"?>";
                xml = xml + "<PRPA_IN000003UV04> ";
                xml = xml + "<!-- UUID,交互实例唯一码-->";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId extension=\"PRPA_IN000003UV04\" root=\"2.16.840.1.113883.1.6\"/>";


                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- 可以填写电话信息或者URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"CDR\" displayName=\"临床数据中心CDR\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml +
                      "<softwareName code=\"PISFZ\" displayName=\"分子诊断信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                xml = xml +
                      "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

                xml = xml + "<!--报告流水号-->";
                xml = xml + "<flowID>" + blbh + "</flowID>";
                xml = xml + "<!--报告类型 - 超声、放射、心电等-->";
                xml = xml + "<adviceType>分子诊断</adviceType>";
                xml = xml + "<!--HIS病人类型 门诊 住院（必填）-->";
                xml = xml + "<patienttype>" + GetBrlbCode(dt_jcxx) + "</patienttype>";
                xml = xml + "<!-- 文档注册时间 -->";
                xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                xml = xml + "<!-- 来源系统编码（必填）2017年8月24日 替换为取消审核人编码 -->";
                xml = xml + $"<sourceCode>{f.ReadString("yh", "yhbh", "").Trim()}</sourceCode>";
                xml = xml + "<!-- 来源系统简称 2017年8月24日 替换为取消审核人名称 -->";
                xml = xml + $"<sourceName>{f.ReadString("yh", "yhmc", "").Trim()}</sourceName>";
                xml = xml + "<!-- 文档编号（必填） -->";
                xml = xml + "<indexInSystem>" + blbh + "</indexInSystem>";

                xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
            }
            catch (Exception ee4)
            {
                log.WriteMyLog(blbh + ",报告召回XML异常：" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='召回XML异常：" + ee4.Message + "' where F_blbh='" + blbh +
                              "' and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }
            if (xml.Trim() == "")
            {
                log.WriteMyLog(blbh + ",报告召回生成xml为空");
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告召回xml为空' where F_blbh='" + blbh +
                              "' and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }

            string msgtxt = "";
            try
            {
                if (debug == "1")
                    log.WriteMyLog("报告召回：[QI1_002--PISFZ_ReportExpired]" + xml);

                string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.154/MQService/MQService.asmx");
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();

                MQSer.Url = wsurl;


                if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_002", "PISFZ_ReportExpired", GUID, "报告回收"))
                {
                    if (debug == "1")
                    {
                        log.WriteMyLog(blbh + ",报告召回成功：" + msgtxt);
                    }
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回成功:" + msgtxt +
                                  "',F_FSZT='已处理' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                    return;
                }
                else
                {
                    log.WriteMyLog(blbh + ",报告召回失败：" + msgtxt);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回失败:" + msgtxt + "' where F_blbh='" +
                                  blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                    return;
                }
            }
            catch (Exception ee4)
            {
                log.WriteMyLog(blbh + ",报告召回异常：" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回异常:" + ee4.Message +
                              "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }
        }
    }
}