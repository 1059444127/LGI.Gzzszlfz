using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using PathHISZGQJK;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class cdzyydx
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string yymc)
        {
            string debug = f.ReadString("savetohis", "debug", "");
           
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();    
            try
            {
                bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (bljc == null)
            {
                 MessageBox.Show("查询数据库失败");
                return;
            }

            if (bljc.Rows.Count <= 0)
            {
                log.WriteMyLog("JCXX表未查询到数据");
                return;
            }
            if (bljc.Rows[0]["f_brlb"].ToString().Trim() != "体检")
             {
                  log.WriteMyLog("非体检病人不处理");
                return;
             }

            string sqxh = bljc.Rows[0]["f_sqxh"].ToString().Trim();
            if (sqxh == "")
             {
                  log.WriteMyLog("体检病人无申请序号，不处理");
                return;
             }


             if (bljc.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
             {
                string odbc_tj = f.ReadString("savetohis", "tjodbc", "Data Source=10.27.1.35;Initial Catalog=zonekingnet;User Id=sa;Password=zoneking;").Replace("\0", "").Trim();
                string bglx = "cg";
                string bgxh = "1";

                 string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                SqlDB  db_tj = new SqlDB();

                 #region  执行存储过程，回写登记状态
                 string  errMsg="";
                 try
                 {
                  if(bljc.Rows[0]["F_TJXTBJ"].ToString().Trim()==null||bljc.Rows[0]["F_TJXTBJ"].ToString().Trim()=="")
                  {
                    SqlParameter[]  sqlPt=new SqlParameter[3];
          
                    for (int j = 0; j < sqlPt.Length; j++)
                    {
                        sqlPt[j] = new SqlParameter();
                    }
                    sqlPt[0].ParameterName = "@Exam_No";
                    sqlPt[0].SqlDbType=SqlDbType.VarChar;
                    sqlPt[0].Direction = ParameterDirection.Input;
                    sqlPt[0].Size = 20;
                    sqlPt[0].Value = sqxh;

                    sqlPt[1].ParameterName = "@StudyType";
                    sqlPt[1].SqlDbType=SqlDbType.VarChar;
                    sqlPt[1].Direction = ParameterDirection.Input;
                    sqlPt[1].Size = 20;
                    sqlPt[1].Value="BL";

                    sqlPt[2].ParameterName = "@StudyState";
                    sqlPt[2].SqlDbType=SqlDbType.Int;
                    sqlPt[2].Direction = ParameterDirection.Input;
                    sqlPt[2].Value=1;

                   int x= db_tj.ExecuteNonQuery(odbc_tj,"dbo.Pro_pacs_ReqStatus",ref sqlPt,CommandType.StoredProcedure, ref errMsg);
                     if(x>0)
                     {
                         aa.ExecuteSQL("update T_JCXX  set F_TJXTBJ='1' where F_blh='" + blh + "'");
                     }
                     else
                     {
                     }

                    }
                 }
                    catch (Exception ee2)
                    {
                       log.WriteMyLog("体检申请单确认异常：" + ee2.Message);
                    }
                #endregion

                #region  回传体检报告

                if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已写报告"&& bljc.Rows[0]["F_TJXTBJ"].ToString().Trim()=="2")
                {
                    db_tj.ExecuteNonQuery(odbc_tj, "delete  from T_SYN_ZK_CHECK_BL   where StudyID='" + sqxh + "'", ref errMsg);
                     aa.ExecuteSQL("update T_JCXX  set F_TJXTBJ='1' where F_blh='"+blh+"'");
                    return;
                }
                if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核")
                {
                    #region 生成jpg
                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                    string errmsg = "";
                    string filename = "";
                    string tjtppath = "";
                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref filename, "", ref errmsg);
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
                        bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, 4, ref pdfpath);
                      
                       // zgq.UpPDF(blh, filename, ML, ref errmsg, 3, ref pdfpath);
                        if (ssa == true)
                        {
                            if (debug == "1")
                               log.WriteMyLog("上传JPG成功");
                            filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                            ZgqClass.BGHJ(blh, "上传JPG", "审核", "上传JPG成功:" + pdfpath, "ZGQJK", "上传JPG");
                            tjtppath = pdfpath.Substring(pdfpath.IndexOf("/", 8));
                        }
                        else
                        {
                           log.WriteMyLog("上传JPG失败：" + errmsg);
                            ZgqClass.BGHJ(blh, "上传JPG", "审核", "上传JPG失败：" + errmsg, "ZGQJK", "上传JPG");
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
                    dt_tj = db_tj.DataAdapter(odbc_tj, "select * from T_SYN_ZK_CHECK_BL   where StudyID='" + sqxh + "'", ref errMsg);
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
                            + "',PatientNameChinese='" + bljc.Rows[0]["F_XM"].ToString().Trim() + "',"
                    + "PatientSex='" + bljc.Rows[0]["F_XB"].ToString().Trim() + "',PatientBirthday='" + bljc.Rows[0]["F_ZY"].ToString().Trim()
                    + "',StudyBodyPart='" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "',ClinicDiagnose='" + blzd + "',ClinicSymptom='" + rysj + "',ClinicAdvice='" + bz
                    + "',IMGStrings='" + tjtppath + "'," + "StudyState=5,Check_Doc='" + bljc.Rows[0]["F_QCYS"].ToString().Trim()
                    + "',Check_Date='" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "',Report_Doc='" + bljc.Rows[0]["F_BGYS"].ToString().Trim()
                    + "',Report_Date='" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',Audit_Doc='" + bljc.Rows[0]["F_SHYS"].ToString().Trim()
                    + "',Audit_Date='" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "',Status_To_Cis=0  "
                    + " where StudyID='" + sqxh + "'";
                    }

                    if (debug == "1")
                       log.WriteMyLog(sql_insert);

                    int z = db_tj.ExecuteNonQuery(odbc_tj, sql_insert, ref errMsg);
                    if (z > 0)
                    {
                        if (debug == "1")
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
      
            }

            
              

            }
        
    }
}
