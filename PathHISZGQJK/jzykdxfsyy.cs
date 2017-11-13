
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data.OleDb;
using System.Xml;
using System.Diagnostics;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
   /// <summary>
   /// 锦州医科大学附属第一医院
   /// </summary>
    class jzykdxfsyy
    {
       private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
       //string odbc2his = "";


        public void pathtohis(string blh, string bglx, string bgxh,string czlb,string dz,string debug)
        {
            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            string ksdm = f.ReadString("savetohis", "KSDM", "2030000").Trim();

            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            if (bljc == null)
            {
                log.WriteMyLog("数据库连接异常");
                return;
            }
            if (bljc.Rows.Count <= 0)
            {
                log.WriteMyLog("未查询到此报告" + blh);
                return;
            }

            string ptjk = ZgqClass.GetSz("zgqjk", "ptjk", "1").Replace("\0", "").Trim();
              string hisjk = ZgqClass.GetSz("zgqjk", "hisjk", "0").Replace("\0", "").Trim();
            string hczt = ZgqClass.GetSz("savetohis", "hczt", "0").Replace("\0", "").Trim();
            string hcbg = ZgqClass.GetSz("savetohis", "hcbg", "0").Replace("\0", "").Trim();
            string topacs = ZgqClass.GetSz("savetohis", "topacs", "0").Replace("\0", "").Trim();
           
           string brlb = bljc.Rows[0]["f_brlb"].ToString().Trim();
           string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
           if (dz == "qxsh" && bglx=="cg")
               bgzt = "取消审核";

            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim();
            
            if(sqxh=="")
            {
                log.WriteMyLog("无申请单号不处理");
                return;
            }

            string errMsg = "";
            if (bljc.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
            {

                string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");

                #region  体检
                //执行存储过程，回写登记状态
                string tjodbc = ZgqClass.GetSz("savetohis", "tjodbc", "Data Source=10.27.1.35;Initial Catalog=zonekingnet;User Id=sa;Password=zoneking;").Replace("\0", "").Trim();
                SqlDB db_tj = new SqlDB();
                #region  回传体检登记状态
                if (bljc.Rows[0]["F_TJXTBJ"].ToString().Trim() != "1")
                {
                    try
                    {
                        SqlParameter[] sqlPt = new SqlParameter[2];
                        for (int j = 0; j < sqlPt.Length; j++)
                        {
                            sqlPt[j] = new SqlParameter();
                        }
                        //申请单ID
                        sqlPt[0].ParameterName = "@Exam_No";
                        sqlPt[0].SqlDbType = SqlDbType.VarChar;
                        sqlPt[0].Direction = ParameterDirection.Input;
                        sqlPt[0].Size = 20;
                        sqlPt[0].Value = sqxh;

                  
                        sqlPt[1].ParameterName = "@StudyState";
                        sqlPt[1].SqlDbType = SqlDbType.Int;
                        sqlPt[1].Direction = ParameterDirection.Input;
                        sqlPt[1].Value = 1;
            
                        //sqlPt[2].ParameterName = "result";
                        //sqlPt[2].SqlDbType = SqlDbType.Int;
                        //sqlPt[2].Direction = ParameterDirection.Output;

                        string err_msg = "";

                        db_tj.ExecuteNonQuery(tjodbc, "dbo.Pro_Pacs_ReqStauts_BL", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
                        aa.ExecuteSQL("update  T_JCXX  set F_TJXTBJ='1'  where F_BLH='" + blh + "'");
                
                    }
                    catch(Exception   ee2)
                    {
                        log.WriteMyLog("体检申请单确认异常：" + ee2.Message);
                    }
                }
                #endregion





                #region  回传体检报告

                if (bgzt == "取消审核")
                {
                
                    db_tj.ExecuteNonQuery(tjodbc, "delete  from T_SYN_ZK_CHECK_BL   where StudyID='" + sqxh + "'", ref errMsg);
                  
                }
                if (bgzt == "已审核")
                {
                    #region 生成jpg

                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                        string errmsg = "";
                        string filename = "";
                        string tjtppath = "";
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref filename,"",ref errmsg );
                        string xy = "3";
                        if (isrtn)
                        {
                            //二进制串
                            if (!File.Exists(filename))
                            {
                                ZgqClass.BGHJ(blh, "体检生成JPG", "审核", "生成JPG失败：未找到文件" + filename, "ZGQJK", "生成JPG");
                                log.WriteMyLog("未找到文件" + filename);
                              
                                zgq.DelTempFile(blh);
                                return;
                            }

                            ZgqClass.BGHJ(blh, "体检生成JPG", "审核", "生成JPG成功", "ZGQJK", "生成JPG");
                            string pdfpath = "";
                            bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy),ref pdfpath);
                            if (ssa == true)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("上传JPG成功");
                                filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "上传JPG", "审核", "上传JPG成功:" + ML + "\\" + filename, "ZGQJK", "上传JPG");
                                tjtppath = "\\pathimages\\pdfbg\\" + ML + "\\"+blh+"\\" + filename;
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + pdfpath + "')");
                            }
                            else
                            {
                                log.WriteMyLog("上传JPG失败：" + errmsg);
                                ZgqClass.BGHJ(blh, "上传JPG", "审核", "上传JPG失败：" + errmsg, "ZGQJK", "上传JPG");
                                log.WriteMyLog("上传JPG失败：" + errmsg);
                                zgq.DelTempFile(blh);
                                return;
                            }
                        }
                        else
                        {
                            log.WriteMyLog("体检生成JPG失败：" + errmsg);
                            ZgqClass.BGHJ(blh, "生成JPG", "审核", "生成JPG失败：" + errmsg, "ZGQJK", "生成JPG");
                            zgq.DelTempFile(blh);
                            return;
                        }

                    #endregion

                    DataTable dt_tj = new DataTable();
                    dt_tj = db_tj.DataAdapter(tjodbc, "select * from T_SYN_ZK_CHECK_BL   where StudyID='" + sqxh + "'", ref errMsg);
                    if (errMsg != "")
                        log.WriteMyLog(errMsg);

                    string blzd = bljc.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_TSJC"].ToString().Trim();
                    string rysj = bljc.Rows[0]["F_rysj"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_JXSJ"].ToString().Trim();
                    string bz = "";

                    if (bljc.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                    {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                        if (TJ_bljc == null || TJ_bljc.Rows.Count <= 0)
                        {

                        }
                        else
                        {
                            rysj = "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n";
                            rysj = rysj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n";
                            rysj = rysj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\r\n";
                            rysj = rysj + "病原微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\r\n";
                            rysj = rysj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";
                            rysj = rysj + "炎症程度：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim();

                            ////////////////////////////////////
                            blzd = TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim();
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                bz = bz + "补充意见1：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                bz = bz + "补充意见2：" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim();
                        }
                    }
                    string sql_insert = "insert into T_SYN_ZK_CHECK_BL(Pacs_CheckID,CISID,StudyID,PacsItemCode,PatientNameChinese,"
                    + "PatientSex,PatientBirthday,StudyType,StudyBodyPart,ClinicDiagnose,ClinicSymptom,ClinicAdvice,IMGStrings,"
                    + "StudyState,Check_Doc,Check_Date,Report_Doc,Report_Date,Audit_Doc,Audit_Date,Status_To_Cis) values("
                    + "'" + blh + "','" + bljc.Rows[0]["F_MZH"].ToString().Trim() + "','" + sqxh + "','','" + bljc.Rows[0]["F_XM"].ToString().Trim() + "',"
                    + "'" + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZY"].ToString().Trim() + "','',"
                    + "'" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "','" + blzd + "','" + rysj + "','" + bz + "','" + tjtppath + "',5,"
                    + "'" + bljc.Rows[0]["F_QCYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "',"
                    + "'" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',"
                    + "'" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "',0"
                    + ")";

                    if (dt_tj.Rows.Count > 0)
                    {
                        sql_insert = "update  T_SYN_ZK_CHECK_BL  set Pacs_CheckID='" + blh + "',CISID='" + bljc.Rows[0]["F_MZH"].ToString().Trim()
                            + "',PatientNameChinese='"+ bljc.Rows[0]["F_XM"].ToString().Trim()+"',"
                    + "PatientSex='" + bljc.Rows[0]["F_XB"].ToString().Trim() + "',PatientBirthday='" + bljc.Rows[0]["F_ZY"].ToString().Trim()
                    + "',StudyBodyPart='" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "',ClinicDiagnose='"+blzd+"',ClinicSymptom='"+rysj+"',ClinicAdvice='"+bz
                    + "',IMGStrings='" + tjtppath + "'," + "StudyState=5,Check_Doc='" + bljc.Rows[0]["F_QCYS"].ToString().Trim()
                    + "',Check_Date='" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "',Report_Doc='" + bljc.Rows[0]["F_BGYS"].ToString().Trim()
                    + "',Report_Date='" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',Audit_Doc='" + bljc.Rows[0]["F_SHYS"].ToString().Trim()
                    + "',Audit_Date='" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "',Status_To_Cis=0  "
                    + " where StudyID='" + sqxh + "'";
                    }

                    if(debug=="1")
                    log.WriteMyLog(sql_insert);
      
                    int z = db_tj.ExecuteNonQuery(tjodbc, sql_insert, ref errMsg);
                  if (z > 0)
                  {
                      if(debug=="1")
                      log.WriteMyLog("写入数据库成功");
                      aa.ExecuteSQL("update  T_JCXX  set F_TJXTBJ='2'  where F_BLH='" + blh + "'");
                  }
                  else
                  {
                      log.WriteMyLog("写入数据失败：" + errMsg);
                  }
               
                }
                #endregion
                return;
                #endregion
            }

            if (hisjk=="1")
            {
                if (bglx == "bc" || bglx == "bd")
                    return;
                

                if (bljc.Rows[0]["F_hisbj"].ToString().Trim()==null  ||bljc.Rows[0]["F_hisbj"].ToString().Trim() != "1")
                {
                    string brlbbm = "";
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
                        brlbbm = "0";
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                        brlbbm = "1";

                    if (brlbbm != "")
                    {
                        if (bljc.Rows[0]["F_hisbj"].ToString().Trim() != "1")
                            ZtToHIS(brlbbm,sqxh, bljc.Rows[0]["F_BRBH"].ToString().Trim(), yhbh, blh, debug);
                    }
                    
                }
                
            }
            if (ptjk == "1")
            { 
                if (sqxh != "")
                {
                #region 平台接口
                    DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + sqxh + "'", "t_sqd");
                    if (dt_sqd.Rows.Count < 1)
                    {
                        log.WriteMyLog("病理号:" + blh + ";T_SQD表中无记录,不处理");
                        goto pacs;
                    }
                    if (dt_sqd.Rows[0]["F_SQDZT"].ToString() != "已登记")
                        aa.ExecuteSQL("update T_SQD set F_SQDZT='已登记' where F_sqxh='" + sqxh + "'");
                   
                    ///回传状态
                    if (hczt == "1")
                        ZtToPt(bljc, dt_sqd, blh, bglx, bgxh, bgzt, dt_sqd.Rows[0]["F_JZLB"].ToString(), yhmc, yhbh, debug);
                  
                    /// 回传报告
                    if (hcbg == "1")
                    {
                        if (bgzt == "已审核")
                            BgToPt(bljc, dt_sqd, blh, bglx, bgxh, dt_sqd.Rows[0]["F_JZLB"].ToString(), debug);
                    }
                #endregion
                }
            }
            pacs:
            if (topacs == "1")
            {
                DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");
                ToPacs(bljc, txlb, bljc.Rows[0]["F_BGZT"].ToString().Trim(),blh,debug);
            }
        }
        public void ZtToHIS(string brlbbm, string sqxh, string brbh, string czygh, string F_blh, string debug)
        {

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
            ops[1].Value =brbh;

            ops[2].ParameterName = "v_page_no";//
            ops[2].OleDbType = OleDbType.VarChar;
            ops[2].Direction = ParameterDirection.Input;
            ops[2].Size = 20;
            ops[2].Value = sqxh;

            ops[3].ParameterName = "v_opera";//
            ops[3].OleDbType = OleDbType.VarChar;
            ops[3].Direction = ParameterDirection.Input;
            ops[3].Size = 10;
            ops[3].Value = czygh;

            ops[4].ParameterName = "v_RetError";//
            ops[4].OleDbType = OleDbType.VarChar;
            ops[4].Direction = ParameterDirection.Output;
            ops[4].Size = 200;

            

            //回写登记状态
            string odbcsql_his = ZgqClass.GetSz("savetohis", "hisodbc", "Provider=MSDAORA;Data Source=ORCL68;User id=chisdb_dev;Password=chisdb_dev;").Replace("\0", "").Trim();

            OleDbDB oledb = new OleDbDB();
            string message_ee = "";
            oledb.ExecuteNonQuery(odbcsql_his, "langjia_to_charge", ref ops, CommandType.StoredProcedure, ref message_ee);

            if (message_ee.Trim() != "")
            {
                log.WriteMyLog("调HIS存储过程langjia_to_charge异常：" + message_ee);
            }
            else
            {
                MessageBox.Show(ops[4].Value.ToString());
                if (debug == "1")
                    log.WriteMyLog("回写申请单登记状态成功");
                aa.ExecuteSQL("update t_jcxx set F_hisbj='1' where f_blh='" + F_blh + "'");
            }
            return;
        }
     
        //Pt
        public void ZtToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string bgzt,string brlb,string yhmc,string yhbh,string  debug)
        {

            if (bglx == "bc" || bglx == "bd")
                return;
         
            string errmsg = "";
            string message = ZtMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh, bgzt, ref errmsg, yhmc,yhbh);

            if (message == "")
            {
                log.WriteMyLog("MQ状态生成XMl失败：" + errmsg);
                return;
            }
       
            if(debug=="1")
                log.WriteMyLog("MQ状态回传："+message);

            string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BLToFZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;

                string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS004.LQ", "BS004", jzlb, "0", "S007", "46300096", "2030000", "0");

                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS004)MQ状态发送错误：" + rtnmsg);
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS004)MQ状态发送完成：" + rtnmsg);
                }
            }
            catch(Exception  ee2)
            {
                log.WriteMyLog("(BS004)MQ状态发送异常：" + ee2.Message);
            }
            return;
        }
     
        public void BgToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string brlb,string debug)
        {
           
            //生成pdf
            string ML = DateTime.Parse(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            string errmsg = ""; string pdf2base64 = ""; string filename = "";
            bool isbase64 = true; bool scpdf = true; bool uppdf = true;
            C_PDF(ML, blh, bgxh, bglx, ref errmsg, ref isbase64, ref pdf2base64, ref filename, ref scpdf, ref uppdf, debug);
            log.WriteMyLog("10");
            if (pdf2base64.Length < 10)
            {
                log.WriteMyLog("报告PDF转base64失败");
            }
            log.WriteMyLog("生成pdf成功");
            string message = BgMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh,ref errmsg,pdf2base64);
            log.WriteMyLog("生成消息成功");
            if (message == "")
            {
                log.WriteMyLog("MQ报告生成XMl失败：" + errmsg);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='MQ报告生成XMl失败:" + errmsg+"' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                return;
            }

            if (debug == "1")
                log.WriteMyLog("MQ报告发送：" + message);

            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BL2FZMQWeb.Service fzmq = new PathHISZGQJK.BL2FZMQWeb.Service();
               // BL2FZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;

                string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
                string rtnmsg = fzmq.SendBgMsgToMQ(message, "IN.BS320.LQ", "BS320", jzlb, "0", "S007", "46300096", "2030000", "0");
               
                if (debug == "1")
                    log.WriteMyLog("(BS320)MQ报告返回：" + rtnmsg);
                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS320)MQ报告发送错误：" + rtnmsg);
                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS320)MQ报告发送错误：" + rtnmsg + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核' ");
            
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS320)MQ报告发送完成：" + rtnmsg);
                    aa.ExecuteSQL("update T_jcxx set F_CFSH='1' where F_blh='" + blh + "'");

                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS320)MQ报告发送完成：" + rtnmsg + "',F_FSZT='已处理' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核' ");
                    return;
                }
                return;
            }
            catch(Exception  ee3)
            {
                log.WriteMyLog("(BS320)MQ报告发送异常：" + ee3.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS320)MQ报告发送异常：" + ee3.Message + "'  where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                return;
            }
        }

        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + int.Parse(blh[i].ToString()).ToString();
                }
                catch { }
            }
            return x;
        }

        public string  ZtMsg(DataTable dt, DataTable dt_sqdxx, ref string xbrlb, string blh, string bglx, string bgxh, string bgzt, ref string errmsg,string yhmc,string yhbh)
        {
          

          
            string bgztbm = "";
            string bgztstr = "";

            if (bgzt == "已审核")
            {
                bgztbm = "170.003";
                bgztstr = "检查报告已审核";
            }
            if (bgzt == "已写报告")
            {
                bgztbm = "160.003";
                bgztstr = "检查已完成";
            }
     
            if (bgzt == "已取材")
            {
                bgztbm = "140.002";
                bgztstr = "检查已到检";
            }
            if (bgzt == "已登记")
            {
                bgztbm = "140.002";
                bgztstr = "检查已到检";
            }
            if (bgzt == "取消审核")
            {
                bgztbm = "990.001";
                bgztstr = "报告召回";
            }

            if (bgztstr == "" || bgztbm.Trim() == "")
            {
          
                return "";
            }
            try
            {
                xbrlb = dt_sqdxx.Rows[0]["F_jzlb"].ToString();
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
                xml = xml + "<item root=\"1.2.156.112675.1.2.1.2\" extension=\"" + dt_sqdxx.Rows[0]["F_yid"].ToString() + "\" />";
                xml = xml + "<!-- 患者ID -->";
                xml = xml + "<item root=\"1.2.156.112675.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- 就诊号 -->";
                xml = xml + "<item root=\"1.2.156.112675.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_jzh"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"  determinerCode=\"INSTANCE\">";
                xml = xml + "<!--病人科室编码-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ksbm"].ToString() + "\" root=\"1.2.156.112675.1.1.1\"/>";
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
                xml = xml + "<item extension=\""+yhbh+"\" root=\"1.2.156.112675.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 操作人姓名 必须项已使用 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\""+yhmc+"\" />";
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
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_zxksbm"].ToString() + "\" root=\"1.2.156.112675.1.2.1.6\" />";
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
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yzh"].ToString() + "\" root=\"1.2.156.112675.1.2.1.22\"/>";
                xml = xml + "<!-- 申请单号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_sqxh"].ToString() + "\" root=\"1.2.156.112675.1.2.1.21\"/>";
                xml = xml + "<!-- 报告号 -->";
                xml = xml + "<item extension=\"" + blh + "\" root=\"1.2.156.112675.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112675.1.2.1.30\"/>";
                xml = xml + "</id>";
             
                xml = xml + "<!-- 医嘱类别编码/医嘱类别名称 - 针剂药品, 材料类, 治疗类, 片剂药品, 化验类 -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.27\">";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_YZLXMC"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";
                
                xml = xml + "<!-- 标本信息 -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                string [] bbtmh = dt_sqdxx.Rows[0]["F_bbtmh"].ToString().Trim().Split('#');
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
                 
                    try
                    {
                        xml = xml + "<any value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMddHHmmss")  + "\"></any>";
                    }
                    catch
                    {
                        xml = xml + "<any value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMddHHmmss")  + "\"></any>";
                    }
                    xml = xml + "</effectiveTime>";
                    xml = xml + "<performer typeCode=\"PRF\">";
                    xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                    xml = xml + "<!-- 采集人Id -->";
                    xml = xml + "<id>";
                    xml = xml + "<item extension=\"" + getyhgh(dt.Rows[0]["F_QCYS"].ToString()) + "\" root=\"1.2.156.112675.1.1.2\"></item>";
                    xml = xml + "</id>";
                    xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                    xml = xml + "<!-- 采集人姓名 -->";
                    xml = xml + "<name xsi:type=\"BAG_EN\">";
                    xml = xml + "<item>";
                    xml = xml + "<part value=\"" + dt.Rows[0]["F_QCYS"].ToString() + "\" />";
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
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112675.1.1.93\">";
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
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112675.1.2.1.7\" />";
                xml = xml + "<!-- 就诊流水号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112675.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--就诊类别编码-->";
                xml = xml + "<code codeSystem=\"1.2.156.112675.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
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
                errmsg = "ERR:" + ex2.Message; return "";
            }
        }

        public string BgMsg(DataTable dt, DataTable dt_sqd, ref string xbrlb, string blh, string bglx, string bgxh, ref string errmsg, string pdf2base64)
        {
          
            try
            { 
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<ClinicalDocument xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../coreschemas/CDA.xsd\">";
                //    <!-- 文档适用范围编码 -->
                xml = xml + "<realmCode code=\"CN\"/>";
                //    <!-- 文档信息模型类别-标识符 -->
                //    <!-- 固定值 -->
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_HD000040\"/>";
                //    <!-- 文档标识-报告号 -->
                string bgh = blh;
                string bgys = dt.Rows[0]["f_bgys"].ToString();
                string shys = dt.Rows[0]["f_shys"].ToString();
                string bgrq = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                if (bglx == "bc")
                {
                    bgh = bgh + "_bc_" + bgxh;
                    bgys = dt.Rows[0]["f_bc_bgys"].ToString();
                    shys = dt.Rows[0]["f_bc_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                }
                if (bglx == "bd")
                {
                    bgh = bgh + "_bd_" + bgxh;
                    bgys = dt.Rows[0]["f_bd_bgys"].ToString();
                    shys = dt.Rows[0]["f_bd_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                }
                string bgysid = "";
                string shysid = "";
                string qcysid = "";
                bgysid = getyhgh(bgys);
                shysid = getyhgh(shys);
                qcysid = getyhgh(dt.Rows[0]["F_QCYS"].ToString());
             

                xml = xml + "<id root=\"S038\" extension=\"" + bgh + "\"/>";
                xml = xml + "<!-- 文档标识-名称 / 文档标识-类别编码 -->";
                xml = xml + "<!-- 固定值 -->";
                xml = xml + "<code code=\"04\" codeSystem=\"1.2.156.112675.1.1.60\" displayName=\"检查检验记录\"/>";
                xml = xml +"<!-- 文档标题文本 -->";
                xml = xml + "<title>病理检查报告</title>";
                xml = xml + "<!-- 文档生效日期 -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" />";
                xml = xml + "<!-- 文档密级编码 -->";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"2.16.840.1.113883.5.25\" codeSystemName=\"Confidentiality\" displayName=\"normal\" />";
                xml = xml + "<!-- 文档语言编码 -->";
                xml = xml + "<languageCode code=\"zh-CN\" />";
                xml = xml + "<!--服务ID-->";
                xml = xml + "<setId extension=\"BS320\"/>";
                //需要程序确定是新增还是修改
                string cfsh = "0";
                try
                {
                    cfsh = dt.Rows[0]["F_cfsh"].ToString().Trim();
                }
                catch
                {
                }
                if (bglx == "bc"|| bglx=="bd")
                {
                    cfsh = "0";
                }
                if (cfsh == "")
                    cfsh = "0";
                xml = xml + "<!-- 文档的操作版本:0表示新增, 1表示修改 -->";
                xml = xml + "<versionNumber value=\"" + cfsh + "\"/>";

                xml = xml + "<!-- 文档记录对象 -->";
                xml = xml + "<recordTarget typeCode=\"RCT\">";
                xml = xml + "<!-- 病人信息 -->";
                xml = xml + "<patientRole classCode=\"PAT\">";
                xml = xml + "<!-- 域ID -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.2\" extension=\"" + dt_sqd.Rows[0]["F_yid"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 患者ID -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.3\" extension=\"" + dt_sqd.Rows[0]["F_BRBH"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 就诊号 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.12\" extension=\"" + dt_sqd.Rows[0]["F_JZH"].ToString().Trim() + "\" />";

                xml = xml + "<!-- 病区床号信息 -->";
                xml = xml + "<addr use=\"TMP\">";
                xml = xml + "<!-- 床位号 -->";
                xml = xml + "<careOf>" + dt_sqd.Rows[0]["F_CH"].ToString().Trim() + "</careOf>";
                xml = xml + "</addr>";

                xml = xml + "<!-- 病人基本信息 -->";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 病人名称 -->";
                xml = xml + "<name>" + dt.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "<!-- 性别编码/性别名称 -->";
                xml = xml + "<administrativeGenderCode code=\"" + dt_sqd.Rows[0]["F_XBBM"].ToString().Trim() + "\" codeSystem=\"1.2.156.112675.1.1.3\" displayName=\"" + dt_sqd.Rows[0]["F_XB"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 出生日期 -->";
                xml = xml + "<birthTime value=\"" + dt_sqd.Rows[0]["F_CSRQ"].ToString() + "\" />";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                xml = xml + "<!-- 文档作者(检查报告医生, 可循环) -->";
                xml = xml + "<author typeCode=\"AUT\">";
                xml = xml + "<!-- 报告日期 -->";
                xml = xml + "<time value=\"" + DateTime.Parse(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 报告医生编码 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 报告医生名称 -->";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";

                xml = xml + "<dataEnterer typeCode=\"ENT\">";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- 记录者编码 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\"\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 记录者名称 -->";
                xml = xml + "<name>"+dt.Rows[0]["F_JSY"].ToString()+"</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</dataEnterer>";


                xml = xml + "<!-- 文档保管者(CDA中custodian为必填项) -->";
                xml = xml + "<custodian>";
                xml = xml + "<assignedCustodian>";
                xml = xml + "<representedCustodianOrganization>";
                xml = xml + "<!-- 医疗机构编码 -->";
                xml = xml + "<id root=\"1.2.156.112675\" extension=\"" + dt_sqd.Rows[0]["F_YYBM"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 医疗机构名称 -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_YYMC"].ToString().Trim() + "</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";

                xml = xml + "<!-- 电子签章信息 -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time />";
                xml = xml + "<signatureCode code=\"S\" />";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- 电子签章号-->";
                xml = xml + "<id extension=\"\" />";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";

                xml = xml + "<!-- 文档审核者(检查报告审核医师, 可循环) -->";
                xml = xml + "<authenticator>";
                xml = xml + "<!-- 审核日期 -->";
                string shsj = "";
                try
                {
                    shsj = Convert.ToDateTime(dt.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                }
                catch
                {
                }
                if (shsj == "")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                }
                if (bglx == "bc")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                if (bglx == "bd")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                xml = xml + "<time value=\"" + shsj + "\" />";
                xml = xml + "<signatureCode code=\"S\"/>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 审核医生编码 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\"" + shysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 审核医生名称 -->";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                xml = xml + "<!-- 申请医生信息 -->";
                xml = xml + "<participant typeCode=\"AUT\">";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 申请医生编码 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\""+ dt_sqd.Rows[0]["F_SQYSBM"].ToString().Trim()+"\" />";
                xml = xml + "<associatedPerson>";
                xml = xml + "<!-- 申请医生名称 -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQYS"].ToString().Trim() + "</name>";
                xml = xml + "</associatedPerson>";
               
                xml = xml + "<scopingOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
			    xml = xml + "<!-- 申请科室编码 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.1\" extension=\"" + dt_sqd.Rows[0]["F_SQKSBM"].ToString().Trim() + "\"/>";
	            xml = xml + "<!-- 申请科室名称 -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQKS"].ToString().Trim() + "</name>";
			    xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";


                xml = xml + "<!-- 关联医嘱信息 -->";
                xml = xml + "<inFulfillmentOf>";
                xml = xml + "<order>";
                xml = xml + "<!-- 关联医嘱号(可多个) -->";
                xml = xml + "<id extension=\"" + dt_sqd.Rows[0]["F_YZH"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";

                xml = xml + "<!-- 文档中医疗卫生事件的就诊场景 -->";
                xml = xml + "<componentOf typeCode=\"COMP\">";
                xml = xml + "<!-- 就诊信息 -->";
                xml = xml + "<encompassingEncounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!-- 就诊次数 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.7\" extension=\"" + dt_sqd.Rows[0]["F_jzcs"].ToString() + "\"/>";
                xml = xml + "<!-- 就诊流水号 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.6\" extension=\"" + dt_sqd.Rows[0]["F_JZLSH"].ToString() + "\"/>";			
			    xml = xml + "<!-- 就诊类别编码/就诊类别名称 -->";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_jzlb"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.80\" displayName=\"" + dt_sqd.Rows[0]["F_brlb"].ToString() + "\" />";
                xml = xml + "<effectiveTime />";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";
                //<!--
                //********************************************************
                //CDA Body
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<structuredBody>";
                //<!-- 
                //********************************************************
                //文档中患者相关信息
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"34076-0\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Information for patients section\" />";
                xml = xml + "<title>文档中患者相关信息</title>";
                xml = xml + "<!-- 患者年龄 -->";
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"397669002\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"age\" />";
                xml = xml + "<value xsi:type=\"ST\">" + dt.Rows[0]["F_nl"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";
               xml = xml + "<!-- 病区 -->";
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\" >";
                xml = xml + "<code code=\"225746001\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"ward\" />";
                xml = xml + "<!-- 病区编码 病区名称 -->";
                xml = xml + "<value xsi:type=\"SC\" code=\"" + dt_sqd.Rows[0]["F_BQBM"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.33\">" + dt_sqd.Rows[0]["F_BQ"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";

                //xml = xml + "<component>";
                //xml = xml + "<section>";
                //xml = xml + "<code code=\"49033-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Menstrual History\"></code>";
                //xml = xml + "<entry>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<code code=\"8665-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Last menstrual period start date\"></code>";
                //xml = xml + "<!--末次月经时间-->";
                //xml = xml + "<value xsi:type=\"TS\" value=\"\"></value>";
                //xml = xml + "</observation>";
                //xml = xml + "</entry>";
                //xml = xml + "<entry>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<code code=\"63890-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Age at last menstrual period \"></code>";
                //xml = xml + "<!--绝经-->";
                //xml = xml + "<value xsi:type=\"ST\"></value>";
                //xml = xml + "</observation>";
                //xml = xml + "</entry>";
                //xml = xml + "</section>";
                //xml = xml + "</component>";

                xml = xml + "</section>";
                xml = xml + "</component>";

                //<!--
                //********************************************************
                //检查章节
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"30954-2\" displayName=\"STUDIES SUMMARY\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\"/>";
                xml = xml + "<title>病理检查</title>";



                // <!-- 相关信息 -->
                xml = xml + "<entry>";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"310388008\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"relative information status\" />";
                xml = xml + "<statusCode code=\"completed\" />";
                //<!-- 定位图像信息 -->
                xml = xml + "<component>";
                xml = xml + "<supply classCode=\"SPLY\" moodCode=\"EVN\">";
                //<!-- 图像索引号(accessionNumber) -->
                xml = xml + "<id extension=\"" + "1001" + "\" />";
                xml = xml + "</supply>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- 图片信息(要求编码为BASE64), @mediaType: 图片格式(JPG格式: image/jpeg PDF格式为: application/pdf) -->";
                xml = xml + "<value xsi:type=\"ED\" mediaType=\"application/pdf\">" + pdf2base64 + "</value>";
                xml = xml + "</observationMedia>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //<!-- 检查报告类型标识编码/检查报告类型标识名称 --0正常报告，1补充报告>
                string bglxstr = "0";
                if (bglx == "bc" || bglx == "bd")
                {
                    bglxstr = "1";
                }
                xml = xml + "<code code=\"" + bglxstr + "\" codeSystem=\"1.2.156.112675.1.1.112\" displayName=\"病理检查报告\" />";
                xml = xml + "</observation>";
                xml = xml + "</component>";

             

                xml = xml + "</organizer>";
                xml = xml + "</entry>";


                ////<!--****************************************************************************-->
                xml = xml + "<!-- 检查报告条目 -->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                 xml = xml + "<id extension=\""+blh+"\" />";
                xml = xml + "<!-- 检查类型编码/检查类型名称 -->"; 
                //检查类型编码没有，名称暂用OT
                xml = xml + "<code code=\"185\" codeSystem=\"1.2.156.112675.1.1.41\" displayName=\"病理\" />";
                xml = xml + "<!-- 必须固定项 -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ////// <!-- 检查使用试剂信息 -->
                ////xml = xml + "<participant typeCode=\"CSM\">";
                ////xml = xml + "<participantRole>";
                ////xml = xml + "<playingEntity>";
                //////<!-- 试剂编码/试剂名称 -->
                ////xml = xml + "<code code=\"\" displayName=\"\" />";
                //////<!-- 试剂用量及单位 -->
                ////xml = xml + "<quantity value=\"\" unit=\"\" />";
                ////xml = xml + "</playingEntity>";
                ////xml = xml + "</participantRole>";
                ////xml = xml + "</participant>";
                xml = xml + "<!-- study -->";
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- 检查项目编码/检查项目名称 -->";
                //  string yzxm = dt.Rows[0]["F_yzxm"].ToString().Trim();
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_yzxmbm"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.88\" displayName=\"" + dt_sqd.Rows[0]["F_yzxm"].ToString() + "\"/>";
                xml = xml + "<!-- 检查备注 -->";
                xml = xml + "<text></text>";
                xml = xml + "<!-- 必须固定项 -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!-- 检查报告结果-客观所见/影像学表现(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->";
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112675.1.1.98\">";
                //xml = xml + "<originalText>" + System.Security.SecurityElement.Escape(dt.Rows[0]["F_rysj"].ToString());
                xml = xml + "<originalText> " + System.Security.SecurityElement.Escape(dt.Rows[0]["F_RYSJ"].ToString()+"\r\n"+dt.Rows[0]["F_JXSJ"].ToString()) + "</originalText>";
                xml = xml + "</value>";
                // <!-- 检查报告结果-主观提示/影像学结论(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112675.1.1.98\">";
                string blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_blzd"].ToString()+"\r\n"+dt.Rows[0]["F_TSJC"].ToString()); ;
                if (bglx == "bc")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bczd"].ToString());
                }
                if (bglx == "bd")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bdzd"].ToString());
                }
                xml = xml + "<originalText>" + blzd + "</originalText>";
                xml = xml + "</value>";

                xml = xml + "<!-- 检查方法编码/检查方法名称 -->";
                xml = xml + "<methodCode code=\"\"  codeSystem=\"1.2.156.112675.1.1.43\" displayName=\"\"/>";
                xml = xml + "<!-- 检查部位编码/检查部位名称 -->";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156112649.1.1.42\" displayName=\"\" />";
                xml = xml + "<!-- 检查医师信息 -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<!-- 诊断日期 -->";
                xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss")  + "\"/>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- 诊断医生编码 -->";
                xml = xml + "<id  root=\"1.2.156.112675.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 诊断医生名称 -->";
                xml = xml + "<name>" + dt.Rows[0]["F_bgys"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 诊断科室编码 -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.1\" extension=\"2070000\"/>";
                xml = xml + "<!-- 诊断科室名称 -->";
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + " </assignedEntity>";
                xml = xml + "</performer>";


                //<!-- 检查医师信息 -->
                xml = xml + "<performer>";
                // <!-- 检查日期 -->
                try
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                xml = xml + "<assignedEntity>";
                //<!-- 检查医生编码 -->
                xml = xml + "<id  root=\"1.2.156.112675.1.1.2\" extension=\"" + qcysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                // <!-- 检查医生名称 -->
                xml = xml + "<name>" + dt.Rows[0]["F_QCYS"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //<!-- 检查科室编码 -->
                xml = xml + "<id root=\"1.2.156.112675.1.1.1\" extension=\"2070000\"/>";
                //<!-- 检查科室名称 -->
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                string[] F_fzys = dt.Rows[0]["F_FZYS"].ToString().Split('/');
                foreach (string fzys in F_fzys)
                {
                    if (fzys != "")
                    {
                        xml = xml + "<!-- 复诊医师信息 -->";
                        xml = xml + "<participant typeCode=\"VRF\">";
                        xml = xml + "<participantRole>";
                        xml = xml + "<!-- 复诊医生编码/复核医生 -->";
                        xml = xml + "<id  root=\"1.2.156.112675.1.1.2\" extension=\"" + getyhgh(fzys) + "\"/>";
                        xml = xml + "<playingEntity>";
                        xml = xml + "<!-- 复诊医生名称 -->";
                        xml = xml + "<name>" + fzys + "</name>";
                        xml = xml + "</playingEntity>";
                        xml = xml + "</participantRole>";
                        xml = xml + "</participant>";
                    }
                }


                xml = xml + "<!-- 标本信息 -->";
                xml = xml + "<participant typeCode=\"SPC\">";
                xml = xml + "<participantRole>";
                xml = xml + "<!--标本类型-->";
                xml = xml + "<code code=\"\" displayName=\"\"></code>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";
                	
                xml = xml + "<!-- 仪器信息 -->";
                xml = xml + "<participant typeCode=\"DEV\">";

                xml = xml + "<time>";
	            xml = xml + "<low value=\"\"/>";
	            xml = xml + "<width xsi:type=\"PQ\" value=\"24\" unit=\"小时\"/>";
                xml = xml + "</time>";

                xml = xml + "<participantRole>";
                xml = xml + "<playingDevice>";
                xml = xml + "<!--仪器型号 仪器名称-->";
                xml = xml + "<manufacturerModelName code=\"\"  displayName=\"\"/>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";


                xml = xml + "<!-- 药物相关信息 -->";
                xml = xml + "<entryRelationship typeCode=\"REFR\">";
                xml = xml + "<substanceAdministration classCode=\"SBADM\" moodCode=\"EVN\">";
                xml = xml + "<!--给药方式-->";
                xml = xml + "<routeCode code=\"\"  displayName=\"\" codeSystem=\"1.2.156.112675.1.1.38\"></routeCode>";
                xml = xml + "<!--药物剂量 药物剂量单位-->";
                xml = xml + " <doseQuantity value=\"\" unit=\"\"></doseQuantity>";
                xml = xml + " <consumable>";
                xml = xml + "<manufacturedProduct>";
                xml = xml + "<manufacturedLabeledDrug>";
                xml = xml + "<!-- 试剂编码 -->";
                xml = xml + "<code code=\"\"></code>";
                xml = xml + "<!-- 试剂名称 -->";
                xml = xml + "<name xsi:type=\"\"></name>";
                xml = xml + "</manufacturedLabeledDrug>";
                xml = xml + "</manufacturedProduct>";
                xml = xml + "</consumable>";
                xml = xml + "</substanceAdministration>";
                xml = xml + "</entryRelationship>";

                //仪器或医生客观所见信息(超声心动报告等结构化所见部分的信息)
                xml = xml + "<!-- 仪器或医生客观所见信息(超声心动报告等结构化所见部分的信息) -->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"365605003\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"body measurement finding\" />";
                xml = xml + "<statusCode code=\"completed\" />";

                xml = xml + "<!-- 项目信息(可循环) -->";
                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"100\" displayName=\"AOD\" />";
                xml = xml + "<value xsi:type=\"SC\">1mm</value>";
                xml = xml + "</observation>";
                xml = xml + "</component>";

                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"200\" displayName=\"LAD\" />";
                // xml = xml + "<value xsi:type=\"SC\">1mm</value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                xml = xml + "<!-- 其它信息按上面格式添加 -->";
                xml = xml + "</organizer>";
                xml = xml + "</entryRelationship>";




                ////xml = xml + "<!-- 图像信息(能与项目对应的图像) -->";
                ////xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                ////xml = xml + "<observationMedia   classCode=\"OBS\" moodCode=\"EVN\">";
                ////xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/jpg\">" ;
                ////xml = xml + "</value>";
                ////xml = xml + "</observationMedia>";
                ////xml = xml + "</entryRelationship>";
                ////xml = xml + "<!-- 当有多个影像对应同一个study时,可以复用此entryRelationship -->";
                ////xml = xml + "</observation></component>";

                ////////////<!-- study 2 -->
                ////xml = xml + "<component typeCode=\"COMP\">";
                ////xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                ////xml = xml + "<code code=\"1\" codeSystem=\"1.2.156.112675.1.1.88\" displayName=\"\"/>";
                ////xml = xml + "<text></text>";
                ////xml = xml + "<statusCode code=\"completed\"/>";

                //////////        <!-- 检查报告结果-客观所见/影像学表现(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112675.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////////        <!-- 检查报告结果-主观提示/影像学结论(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112675.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////////        <!-- 检查方法编码/检查方法名称 -->
                ////xml = xml + "<methodCode code=\"002\"  codeSystem=\"1.2.156.112675.1.1.43\" displayName=\"\"/>";
                //////////        <!-- 检查部位编码/检查部位名称 -->
                ////xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156.112675.1.1.42\" displayName=\"\" />";
                //////////        <!-- 检查医师信息 --




                xml = xml + "<!-- 标本采集和接收信息-->";
                xml = xml + "<entryRelationship typeCode=\"SAS\" inversionInd=\"true\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<!-- 病理编号 -->";
                xml = xml + "<id extension=\""+blh+"\"/>";
                xml = xml + "<code />";
                xml = xml + "<statusCode code=\"completed\" />";
                xml = xml + "<!-- 标本采集日期(取材日期) -->";
                try
                {
                    xml = xml + "<effectiveTime value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<effectiveTime value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
            
                xml = xml + "<!--DNA提取方法-->";
                xml = xml + "<methodCode code=\"\" displayName=\"\"/>";
                xml = xml + "<!-- 取材部位编码/取材部位名称 -->";
                xml = xml + "<targetSiteCode code=\"\" displayName=\""+dt.Rows[0]["F_bbmc"].ToString()+"\" />";

                xml = xml + "<performer>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!--取样医生编码-->";
                xml = xml + "<id extension=\"\" root=\"1.2.156.112675.1.1.2\"></id>";
                xml = xml + "<assignedPerson>";
                xml = xml + "<!--取样医生名称-->";
                xml = xml + "<name>"+dt.Rows[0]["F_QCYS"].ToString()+"</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                xml = xml + "<entryRelationship typeCode=\"SAS\">";
                xml = xml + "<procedure classCode=\"TRNS\" moodCode=\"EVN\">";
                xml = xml + "<performer>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<id/>";
                xml = xml + "<!--送检医院-->";
                xml = xml + "<representedOrganization>";
                xml = xml + "<name>"+dt.Rows[0]["F_SJDW"].ToString()+"</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<participant typeCode=\"RCV\">";
                xml = xml + "<!-- 接收时间/送检时间 -->";
                xml = xml + "<time value=\""+ Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMddHHmmss")+"\" />";
                xml = xml + "<participantRole/>";
                xml = xml + "</participant>";										
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";


                xml = xml + "<entryRelationship typeCode=\"REFR\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!--原病理号-->";
                xml = xml + "<id extension=\"\"></id>";
                xml = xml + "<code></code>";
                xml = xml + "<!--原病理号临床诊断-->";
                xml = xml + "<value xsi:type=\"ED\"></value>";
                xml = xml + "</observation>";
                xml = xml + "</entryRelationship>";




                xml = xml + "</observation>";
                xml = xml + "</component>";

                //<!-- 其他项目按上面结构和格式添加 -->

                //<!-- 当系统所生成的报告中,图像无法与具体的study做对应时,使用以下部分来放置影像 -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- 影像信息(要求编码为BASE64), @mediaType: 影像格式 -->
                // xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/gif\">";
                // xml = xml + "</value>";
                // xml = xml + "</observationMedia>";
                // xml = xml + "</component>";

                ////<!-- 当有多个影像时,按照以上格式添加 -->
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";

                // <!-- 当系统中,客观所见(和主观意见)无法对应到具体的study, 
                // 而是多个study的客观所见(和主观意见)记录在同一个文本字段中,
                // 使用以下部分来放置客观所见和主观意见 -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // //<!-- @code:01表示客观所见, 02表示主观提示 -->
                // xml = xml + "<code code=\"01\" codeSystem=\"1.2.156.112675.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //// <!-- @code:01表示客观所见, 02表示主观提示 -->
                // xml = xml + "<code code=\"02\" codeSystem=\"1.2.156.112675.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";


                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                ////////********************************************************
                ////////临床资料
                ////////* *******************************************************

                //xml = xml  + "<!-- 临床资料-->";
                //xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code /><value xsi:type=\"ED\"></value></observation></entry></section></component>";
              
                
                ////////<!-- 
                ////////****************************************************************************
                ////////  #药观章节
                ////////****************************************************************************
                ////////-->

                 //xml = xml  + "<!-- 药观章节-->";
                //xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code code=\"\" displayName=\"\"/></observation></entry></section></component>";
               
                
                //////////<!-- 
                ////////********************************************************
                ////////诊断
                ////////********************************************************
                ////////-->

                xml = xml  + "<!-- 诊断-->";
                xml = xml + "<component><section><code code=\"29308-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Diagnosis\"/>";
                xml = xml + "<title>诊断</title>";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code nullFlavor=\"NA\"/>";
                xml = xml + "<entryRelationship typeCode=\"SUBJ\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- 诊断类别编码/诊断类别名称 -->";
                xml = xml + "<code code=\"\" codeSystem=\"1.2.156.112675.1.1.29\" displayName=\"\" />";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!-- 疾病编码/疾病名称(没有编码去掉@code) -->";
                xml = xml + "<value xsi:type=\"\" code=\"\" codeSystem=\"1.2.156.112675.1.1.30\" displayName=\"\" />";
                xml = xml + "</observation>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                ////////********************************************************
                ////////补充意见章节（TCT检查报告单用）
                ////////********************************************************
                ////////--> 

                //xml = xml + "<!--补充意见章节（TCT检查报告单用） -->";
                //xml = xml + "<component>";
                //xml = xml + "<section>";
                //xml = xml + "<code code=\"52535-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Other useful information\" />";
                //xml = xml + "<!-- 章节标题 -->";
                //xml = xml + "<title>补充意见信息</title>"; 
                //xml = xml + "<!-- 补充意见内容 -->";
                //xml = xml + "<text></text>";
                //xml = xml +  "</section>";
                //xml = xml + "</component>";

                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</ClinicalDocument>";

                return FormatXml(xml);
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                return "";

            }
        }

        public void C_PDF(string ML, string blh, string bgxh, string bglx, ref  string errmsg, ref  bool isbase64, ref string Base64String, ref string filename, ref bool ScPDF, ref bool UpPDF, string debug)
        {

            log.WriteMyLog("1");
            ScPDF = false; UpPDF = false;
            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
            {
                #region  生成pdf

                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref errmsg, ref filename);
                string xy = "3";
                if (isrtn)
                {
                    
                    //二进制串
                    if (!File.Exists(filename))
                    {
                        ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：未找到文件" + filename, "ZGQJK", "生成PDF");
                        log.WriteMyLog("未找到文件" + filename);
                        errmsg = "未找到文件" + filename;
                        zgq.DelTempFile(blh);
                        isbase64 = false;
                        return;
                    }
                 
                    ScPDF = true;
                    if (isbase64)
                    {
                        try
                        {
                            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                            file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                            file.Close();
                            Base64String = Convert.ToBase64String(imgByte);
                            isbase64 = true;
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("PDF转换二进制串失败");
                            errmsg = "PDF转换二进制串失败";
                            isbase64 = false;
                        }
                    }
                  

                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF成功", "ZGQJK", "生成PDF");
                    bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy));
                    if (ssa == true)
                    {
                        
                        if (debug == "1")
                            log.WriteMyLog("上传PDF成功");
                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + ML + "\\" + filename, "ZGQJK", "上传PDF");

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + filename + "')");

                        UpPDF = true;
                    }
                    else
                    {
                   
                        log.WriteMyLog("上传PDF失败：" + errmsg);
                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
                        UpPDF = false;
                    }
                    zgq.DelTempFile(blh);
                }
                else
                {
                  
                    ScPDF = false;
                    log.WriteMyLog("生成PDF失败：" + errmsg);
                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：" + errmsg, "ZGQJK", "生成PDF");
                    zgq.DelTempFile(blh);

                    return;
                }
                #endregion
            }
            else
            {
                log.WriteMyLog("不需生成PDF");
                return;
            }
        }

    
        //public string fsbg(string message, ref string rtnmsg, string brlb)
        //{


        //    MQQueueManager qMgr = null;
        //    Hashtable env = new Hashtable();

        //    rtnmsg = "";
        //    string hostname = f.ReadString("MQSERVER", "hostname", "192.168.171.64").Replace("\0", "");
        //    string channel = f.ReadString("MQSERVER", "channel", "IE.SVRCONN").Replace("\0", "");
        //    string qManager = f.ReadString("MQSERVER", "qManager", "GWI.QM").Replace("\0", "");
        //    string queueManager = f.ReadString("MQSERVER", "queueManager", "IN.BS366.LQ").Replace("\0", "");
        //    env.Clear();

        //    env.Add(IBM.XMS.MQC.HOST_NAME_PROPERTY, hostname);
        //    env.Add(IBM.XMS.MQC.CHANNEL_PROPERTY, channel);
        //    env.Add(IBM.XMS.MQC.CCSID_PROPERTY, 1208);
        //    env.Add(IBM.XMS.MQC.PORT_PROPERTY, 5000);
        //    env.Add(IBM.XMS.MQC.TRANSPORT_PROPERTY, IBM.XMS.MQC.TRANSPORT_MQSERIES);
        //    env.Add(IBM.XMS.MQC.USER_ID_PROPERTY, "mqm");
        //    String content = message;


        //    qMgr = new MQQueueManager(qManager, env);

        //    try
        //    {

        //        int openOptions = IBM.XMS.MQC.MQOO_OUTPUT
        //                | IBM.XMS.MQC.MQPMO_PASS_ALL_CONTEXT;
        //        MQMessage msg = new MQMessage();

        //        msg.CharacterSet = 1208;
        //        MQQueue queue = qMgr.AccessQueue(queueManager, openOptions);
        //        msg.Format = IBM.XMS.MQC.MQFMT_STRING;


        //        // 自定义属性内容请参考相关服务设计文档
        //        /// 消息ID
        //        msg.SetStringProperty("service_id", "BS004");

        //        //01 门诊,02 急诊,03 住院,04 体检,05 转院
        //        msg.SetStringProperty("domain_id", brlb);
        //        // 申请科室ID
        //        msg.SetStringProperty("apply_unit_id", "0");
        //        // 发送系统ID
        //        msg.SetStringProperty("send_sys_id", "S007");
        //        // 医疗机构代码
        //        msg.SetStringProperty("hospital_id", "44643245-7");
        //        // 执行科室ID
        //        msg.SetStringProperty("exec_unit_id", "2070000");
        //        // 医嘱执行分类编码   （医嘱类填写，报告类可默认传0）
        //        msg.SetStringProperty("order_exec_id", "0");


        //        // 扩展码
        //        msg.SetStringProperty("extend_sub_id", "0");


        //        msg.WriteString(content);

        //        MQPutMessageOptions pmo = new MQPutMessageOptions();
        //        pmo.Options = IBM.XMS.MQC.MQPMO_SYNCPOINT;
        //        queue.Put(msg, pmo);

        //        // queue.Put(aa);
        //        queue.Close();
        //        qMgr.Commit();

        //        //MessageBox.Show(byteToHexStr(msg.MessageId));
        //        string messageid = byteToHexStr(msg.MessageId);
        //        //到时将messageid保存到临时表中，历史数据存到F_yl7
        //        qMgr.Disconnect();
        //        rtnmsg = messageid;
        //        return "";

        //        // textBox1.Text = "";               
        //    }
        //    catch (Exception ex)
        //    {
        //        rtnmsg = ex.Message;
        //         LGZGQClass.log.WriteMyLog("消息发送异常！" + ex.Message);
        //        return "ERR";
        //    }

        //}



        public  void  ToPacs(DataTable  bljc,DataTable txlb,string bgzt,string blh,string debug)
        {
            string pacspath = f.ReadString("savetohis", "pacspath", @"\\174.30.0.105\CDA2PACS_IN").Replace("\0", "");
            if(bgzt.Trim()=="已审核")
            {
                 #region 执行存储过程————SP_TransferReport

                    string database = f.ReadString("savetohis", "pacsdbbase", "Integration");
                    string server = f.ReadString("savetohis", "pacsdbserver", @"174.30.0.105\CSSERVER");
                    string uid = f.ReadString("savetohis", "pacsuid", "pacs");
                    string pwd = f.ReadString("savetohis", "pacsdbpwd", "pacs123");
                    string ProcName = f.ReadString("savetohis", "ProcName", "SP_TransferReport");
                    SqlConnection ocn = new SqlConnection("database=" + database + ";server=" + server + ";uid=" + uid + ";pwd=" + pwd + "");
                    try
                    {
                        ocn.Open();
                        SqlCommand ocmd = new SqlCommand();
                        ocmd.Connection = ocn;
                        ocmd.CommandText = ProcName;
                        ocmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] ORAPAR = { 
                        new SqlParameter("ApplyNo",SqlDbType.VarChar, 50),               //0  HIS 检查申请单流水号
                        new SqlParameter("AccessionNo", SqlDbType.VarChar, 20),          //1   检查号（各影像系统内部检查号）
                        new SqlParameter("PatientID", SqlDbType.VarChar, 30),            //2   患者唯一主索引号
                        new SqlParameter("PatientType", SqlDbType.VarChar, 20),           //3  就诊类型（I：住院；O：门诊；E：急诊）
                        new SqlParameter("InHospitalNo", SqlDbType.VarChar, 200),          //4  住院号（住院患者必填）
                        new SqlParameter("ClinicNo", SqlDbType.VarChar, 20),              //5  门诊号（门急诊患者必填）
                        new SqlParameter("PatientName", SqlDbType.VarChar,30),            //6   患者姓名
                        new SqlParameter("Gender", SqlDbType.VarChar, 20),                  //7 性别（M：男；F：女；O：其它；U：未知）
                        new SqlParameter("BirthDate", SqlDbType.Date),                 //8  出生日期，格式为 YYYY-MM-DD
                        new SqlParameter("IdCard", SqlDbType.VarChar, 50),                   //9    身份证号
                        new SqlParameter("ContactPhone", SqlDbType.VarChar, 80),           //10   联系电话
                        new SqlParameter("ContactAddress", SqlDbType.VarChar,500),        //11   联系地址
                        new SqlParameter("InHospitalRegion",SqlDbType.VarChar,50),      //12    病区（仅住院患者）
                        new SqlParameter("BedNo", SqlDbType.VarChar,50),             //13    床号（仅住院患者）
                        new SqlParameter("ModalityType", SqlDbType.VarChar, 100),          //14   检查类型（DX/CT/MR/US/ES/PS/NM 等）
                        new SqlParameter("ApplyDepart", SqlDbType.VarChar, 100),         //15    申请科室名称
                        new SqlParameter("ApplyDoctor", SqlDbType.VarChar, 20),        //16       申请医生姓名
                        new SqlParameter("ApplyDateTime", SqlDbType.DateTime),            //17     申请日期时间(YYYY-MM-DD HH:mm:SS)
                        new SqlParameter("ExamineDatetime", SqlDbType.DateTime),           //18      检查日期时间(YYYY-MM-DD HH:mm:SS)
                        new SqlParameter("ProcedureCode", SqlDbType.VarChar, 20),             //19  检查部位代码
                        new SqlParameter("ProcedureDesc", SqlDbType.VarChar, 50),           //20       检查部位描述
                        new SqlParameter("HasImage", SqlDbType.Int),               //21    是否有图（1：有图；0：无图）
                        new SqlParameter("HasReport", SqlDbType.Int),             //22   是否有报告（1：有报告；0：无报告） 
                        new SqlParameter("ReportSee", SqlDbType.VarChar,1000),         //23   检查所见
                        new SqlParameter("ReportGet", SqlDbType.VarChar, 1000),         //24    检查所得
                        new SqlParameter("ReportDatetime", SqlDbType.DateTime),       //25  报告日期时间(YYYY-MM-DD HH:mm:SS)
                        new SqlParameter("Reporter", SqlDbType.VarChar, 20),          //26    报告医生
                        new SqlParameter("Status", SqlDbType.Int),                  //27    状态（审核：1；修改：0）
                        new SqlParameter("CardId", SqlDbType.VarChar, 20)           //28    诊疗卡号
                       // new SqlParameter("ReportGUID", SqlDbType.VarChar, 20)            //29    诊疗卡号
                        };

                        ORAPAR[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();
                        ORAPAR[1].Value = bljc.Rows[0]["F_BLH"].ToString().Trim();
                        ORAPAR[2].Value = bljc.Rows[0]["F_brbh"].ToString().Trim();
                        string f_jzlx = "";
                        if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                        {
                            f_jzlx = "I";
                        }
                        else if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
                        { f_jzlx = "O"; }
                        else if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "急诊")
                        { f_jzlx = "E"; }
                        ORAPAR[3].Value = f_jzlx;

                        ORAPAR[4].Value = bljc.Rows[0]["F_zyh"].ToString().Trim();
                        ORAPAR[5].Value = bljc.Rows[0]["F_mzH"].ToString().Trim();
                        ORAPAR[6].Value = bljc.Rows[0]["F_xm"].ToString().Trim();
                        string f_gener = "";
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "男")
                        {
                            f_gener = "M";
                        }
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "女")
                        {
                            f_gener = "F";
                        }
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "其他")
                        {
                            f_gener = "O";
                        }
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "未知")
                        {
                            f_gener = "U";
                        }
                        ORAPAR[7].Value = f_gener; //7 性别（M：男；F：女；O：其它；U：未知）
                        try
                        {
                            ORAPAR[8].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["F_BY2"].ToString().Trim()).ToString("yyyy-MM-dd"));
                        }
                        catch (Exception)
                        {

                            ORAPAR[8].Value = DBNull.Value;
                        }
                        ORAPAR[9].Value = bljc.Rows[0]["F_sfzh"].ToString().Trim();
                        ORAPAR[10].Value = bljc.Rows[0]["F_lxxx"].ToString().Trim();
                        ORAPAR[11].Value = bljc.Rows[0]["F_lxxx"].ToString();
                        ORAPAR[12].Value = bljc.Rows[0]["F_bq"].ToString().Trim();
                        ORAPAR[13].Value = bljc.Rows[0]["F_ch"].ToString().Trim();
                        ORAPAR[14].Value = "PIS";
                        ORAPAR[15].Value = bljc.Rows[0]["F_sjks"].ToString().Trim();
                        ORAPAR[16].Value = bljc.Rows[0]["F_sjys"].ToString().Trim();
                        try
                        {
                            ORAPAR[17].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception)
                        {
                            ORAPAR[17].Value = DBNull.Value;
                        }

                        try
                        {
                            ORAPAR[18].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception)
                        {
                            ORAPAR[18].Value = DBNull.Value;
                        }
                        ORAPAR[19].Value = "";
                        ORAPAR[20].Value = "病理检查";
                        if (txlb.Rows.Count > 0)
                        {
                            ORAPAR[21].Value = 1;
                        }
                        else ORAPAR[21].Value = 0;
                        if (bljc.Rows[0]["F_bgzt"].ToString().Trim().ToString() == "已审核")
                        {
                            ORAPAR[22].Value = 1;
                        }
                        else
                        {
                            ORAPAR[22].Value = 0;
                        }
                        ORAPAR[23].Value = bljc.Rows[0]["f_rysj"].ToString().Trim().ToString();
                        ORAPAR[24].Value = bljc.Rows[0]["f_blzd"].ToString().Trim().ToString();
                        try
                        {
                            ORAPAR[25].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["f_bgrq"].ToString().ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception)
                        {
                            ORAPAR[25].Value = DBNull.Value;
                        }
                        ORAPAR[26].Value = bljc.Rows[0]["f_bgys"].ToString().ToString().Trim();
                        if (bljc.Rows[0]["F_bgzt"].ToString().Trim().ToString() == "已审核")
                        {
                            ORAPAR[27].Value = 1;
                        }
                        else
                        {
                            ORAPAR[27].Value = 0;
                        }
                        ORAPAR[28].Value = bljc.Rows[0]["F_YZID"].ToString().Trim();
                       // ORAPAR[29].Value = "";



                        ORAPAR[0].Direction = ParameterDirection.Input;
                        ORAPAR[1].Direction = ParameterDirection.Input;
                        ORAPAR[2].Direction = ParameterDirection.Input;
                        ORAPAR[3].Direction = ParameterDirection.Input;
                        ORAPAR[4].Direction = ParameterDirection.Input;
                        ORAPAR[5].Direction = ParameterDirection.Input;
                        ORAPAR[6].Direction = ParameterDirection.Input;
                        ORAPAR[7].Direction = ParameterDirection.Input;
                        ORAPAR[8].Direction = ParameterDirection.Input;
                        ORAPAR[9].Direction = ParameterDirection.Input;
                        ORAPAR[10].Direction = ParameterDirection.Input;
                        ORAPAR[11].Direction = ParameterDirection.Input;
                        ORAPAR[12].Direction = ParameterDirection.Input;
                        ORAPAR[13].Direction = ParameterDirection.Input;
                        ORAPAR[14].Direction = ParameterDirection.Input;
                        ORAPAR[15].Direction = ParameterDirection.Input;
                        ORAPAR[16].Direction = ParameterDirection.Input;
                        ORAPAR[17].Direction = ParameterDirection.Input;
                        ORAPAR[18].Direction = ParameterDirection.Input;
                        ORAPAR[19].Direction = ParameterDirection.Input;
                        ORAPAR[20].Direction = ParameterDirection.Input;
                        ORAPAR[21].Direction = ParameterDirection.Input;
                        ORAPAR[22].Direction = ParameterDirection.Input;
                        ORAPAR[23].Direction = ParameterDirection.Input;
                        ORAPAR[24].Direction = ParameterDirection.Input;
                        ORAPAR[25].Direction = ParameterDirection.Input;
                        ORAPAR[26].Direction = ParameterDirection.Input;
                        ORAPAR[27].Direction = ParameterDirection.Input;
                        ORAPAR[28].Direction = ParameterDirection.Input;


                        StringBuilder sb = new StringBuilder();
                        foreach (SqlParameter parameter in ORAPAR)
                        {
                            ocmd.Parameters.Add(parameter);
                            sb.Append(parameter.ToString() + ":" + parameter.Value.ToString() + ",");
                        }
                        if (debug == "1")
                            log.WriteMyLog(blh + sb.ToString());
                        try
                        {
                            int i = 0;
                            if (ocn.State != ConnectionState.Open)
                            {
                                ocn.Open();
                            }
                            i = ocmd.ExecuteNonQuery();//执行存储过程    
                            if (i > 0)
                            {
                                ocn.Close();
                                if (debug == "1")
                                    log.WriteMyLog(blh + ",执行PACS存储过程成功！");
                            }
                            else
                            {
                                ocn.Close();
                                log.WriteMyLog(blh + " 执行PACS存储过程失败！");
                                return;
                            }

                        }
                        catch (Exception e)
                        {
                            ocn.Close();
                            log.WriteMyLog(blh + ",执行存储过程异常：" + e.Message.ToString() + "");
                            return;
                        }
                    }
                    catch (Exception ee)
                    {
                        ocn.Close();
                        log.WriteMyLog(blh + ",执行存储过程异常2：" + ee.Message);
                        return;
                    }
                    finally
                    {
                        if (ocn.State == ConnectionState.Open)
                            ocn.Close();
                    }

                    #endregion

      
                    string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                    string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                    string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                    string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                    string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                    string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                    FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                    string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
                    string stxsm = "";

                    #region 下载图像
                    if (!Directory.Exists(@"c:\temp"))
                    {
                        Directory.CreateDirectory(@"c:\temp");
                    }
                    if (!Directory.Exists(@"c:\temp\" + blh))
                    {
                        Directory.CreateDirectory(@"c:\temp\" + blh);
                    }
 
                    string xzfs = f.ReadString("savetohis", "xzfs", "ftp");//图像下载方式
                    if (xzfs == "ftp")
                    {
                        if (ftps == "1")
                        {
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                                string ftpstatus = "";
                                try
                                {
                                    fw.Download(ftplocal + "\\" + blh, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                }
                                catch (Exception ee)
                                {
                                    log.WriteMyLog("下载图像失败！！！" + ee.Message);
                                }
                                if (ftpstatus == "Error")
                                {
                                    log.WriteMyLog("下载图像失败！！！" + ftpstatus + "再补传一次");
                                    fw.Download(ftplocal + "\\" + blh, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (txpath == "")
                        {
                            log.WriteMyLog("sz.ini txpath图像目录未设置");
                            return;
                        }
                        for (int i = 0; i < txlb.Rows.Count; i++)
                        {
                            try
                            {
                                File.Copy(@"" + txpath + "" + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + blh + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                            }
                            catch
                            {
                                log.WriteMyLog("共享目录不存在！");
                                return;
                            }
                        }
                    }
                    if (debug == "1")
                        log.WriteMyLog("下载完成！！！");
                 
                    #endregion

                    #region 生成XML文件
             
                    StringBuilder inxmlSB = new StringBuilder(255);
                    try
                    {
                        inxmlSB.AppendLine("<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "UTF-8" + (char)34 + "?>");
                        inxmlSB.AppendLine("<document_info>");
                        inxmlSB.AppendLine("<patient_info>");
                        inxmlSB.AppendLine("<global_patientid>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</global_patientid>");
                        inxmlSB.AppendLine("<patientid>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</patientid>");
                        string patientname_en = System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yl4"].ToString().Trim());
                       //转拼音全拼部分已删除，外部第三方DLL

                        inxmlSB.AppendLine("<patientname_en>" + patientname_en + "</patientname_en>");
                        inxmlSB.AppendLine("<patientname_ch>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xm"].ToString().Trim()) + "</patientname_ch>");
                        inxmlSB.AppendLine("<usedname>" + "" + "</usedname>");
                        string gender = "";
                        switch (bljc.Rows[0]["F_xb"].ToString().Trim())
                        {
                            case "男": gender = "M"; break;
                            case "女": gender = "F"; break;
                            case "其他": gender = "O"; break;
                            case "未知": gender = "U"; break;
                            default:
                                break;
                        }
                        inxmlSB.AppendLine("<gender>" + System.Security.SecurityElement.Escape(gender) + "</gender> ");
                        string bgrqxml = "";
                        try
                        {
                            bgrqxml = Convert.ToDateTime(bljc.Rows[0]["F_by2"].ToString().Trim()).ToString("YYYYMMDD");
                        }
                        catch (Exception)
                        {
                            bgrqxml = "";
                        }
                        inxmlSB.AppendLine("<birthday>" + System.Security.SecurityElement.Escape(bgrqxml) + "</birthday>");
                        inxmlSB.AppendLine("</patient_info>");
                        inxmlSB.AppendLine("<order_info>");
                        inxmlSB.AppendLine("<accession_number>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLH"].ToString().Trim()) + "</accession_number>");
                        inxmlSB.AppendLine("</order_info>");
                        inxmlSB.AppendLine("<report_info>");
                        inxmlSB.AppendLine("<piclist_count>" + System.Security.SecurityElement.Escape(txlb.Rows.Count.ToString()) + "</piclist_count>");
                        if (txlb.Rows.Count > 0)
                        {
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                inxmlSB.AppendLine("<piclist" + (i + 1) + ">");
                                inxmlSB.AppendLine("<document_name>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_txm"].ToString().Trim()) + "</document_name>");
                                inxmlSB.AppendLine("<document_type>" + "JPG" + "</document_type>");
                                inxmlSB.AppendLine("<document_datetime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + "</document_datetime>");
                                inxmlSB.AppendLine("<document_comment>" + "" + "</document_comment>");
                                inxmlSB.AppendLine("</piclist" + (i + 1) + ">");
                            }
                        }
                        inxmlSB.AppendLine("</report_info>");
                        inxmlSB.AppendLine("</document_info>");
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("创建XML串异常："+ee.Message);
                        return;
                    }

                    try
                    {
                        if (debug == "1")
                            log.WriteMyLog("写入xml串为：" + inxmlSB.ToString());

                        string filePath = ftplocal + "\\" + blh + "\\" + blh + ".xml";
                        if (!System.IO.Directory.Exists(ftplocal + "\\" + blh))//如果文件夹不存在
                        {
                            Directory.CreateDirectory(ftplocal + "\\" + blh);
                        }
                        if (!File.Exists(filePath))//如果文件不存在 
                        {
                            File.Create(filePath).Close();
                        }
                        File.WriteAllText(filePath, "");
                        StreamWriter swx = File.AppendText(filePath);
                        swx.WriteLine(inxmlSB.ToString());
                        swx.Close();
                        if (debug == "1")
                            log.WriteMyLog("xml文件创建完成！！！");
                    }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog("xml文件创建失败："+ee2.Message);
                        return;
                    }
                    #endregion

                    #region 记住共享目录的用户名和密码

                    string gxuid = f.ReadString("savetohis", "gxuid", "administrator").Replace("\0", "");
                    string gxpwd = f.ReadString("savetohis", "gxpwd", "1qaz!QAZ").Replace("\0", "");
                    System.Diagnostics.Process prc = new Process();
                    prc.StartInfo.FileName = @"cmd.exe";
                    prc.StartInfo.UseShellExecute = false;
                    prc.StartInfo.RedirectStandardInput = true;
                    prc.StartInfo.RedirectStandardOutput = true;
                    prc.StartInfo.RedirectStandardError = true;
                    prc.StartInfo.CreateNoWindow = true;

                    prc.Start();
                    //string dos_cmd = "net use " + gxml + " /del";
                    //prc.StandardInput.WriteLine(dos_cmd);
                    string dos_cmd = "net use " + pacspath + " " + gxpwd + " /user:" + gxuid;
                    prc.StandardInput.WriteLine(dos_cmd);
                    prc.StandardInput.Close();

                    string output = prc.StandardOutput.ReadToEnd();

                    //MessageBox.Show(output);
                    if (output.IndexOf("命令成功完成") > 0)
                    {
                        log.WriteMyLog("连接共享图像服务器成功！");
                    }
                    else
                    {
                        log.WriteMyLog("连接共享图像服务器失败！");

                    }

                    #endregion
                    #region 上传镜下图到PACS  
                    
                    try
                    {
                        copydir.copyDirectory(@"" + ftplocal + "" + "\\" + blh, @"" + pacspath + "");

                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("上传文件失败！！！" + ee.Message);
                        return;
                    }
                    if (debug == "1")
                        log.WriteMyLog("上传文件成功");

                    #endregion

                    aa.ExecuteSQL("update t_jcxx set f_topacs='1'   where f_blh='" + blh + "'");

                }
                else
                {
                    if (bljc.Rows[0]["f_topacs"].ToString() == "1")
                    {
                        Directory.Delete(@"" + pacspath + "\\"+blh+"\\", true);
                        aa.ExecuteSQL("update t_jcxx set f_topacs='0'   where f_blh='" + blh + "'");

                    }
                    log.WriteMyLog("删除文件成功！");
                }
                aa.ExecuteSQL("update t_jcxx_fs  set f_fszt='已处理' where f_blh='" + blh + "'");
        }

        public string getyhgh(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
        private string FormatXml(string sUnformattedXml)
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
