using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.OracleClient;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    /// <summary>
    /// 厦门中山医院
    /// </summary>
    class xmzsyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string msg1, string debug, string[] cslb)
        {

              string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            string jkmsg = f.ReadString("jkmsg", "jkmsg", "0").Replace("\0", "");

            
            f.WriteInteger("jkmsg", "jkmsg", 0);

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                {
                    //取消审核动作
                    qxsh = "1";
                }

                if (cslb[3].ToLower() == "new")
                {
                    xdj = "1";
                }

            }
    
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
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

            if (bglx.Trim() == "")
            {
                log.WriteMyLog("报告类型为空，不处理！" + blh + "^" + bglx + "^" + bgxh);
                return;
            }
            string bgzt = "";

            if (qxsh == "1")
            {
                bgzt = "取消审核";
            }
            else
                bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
           
            int plsc = f.ReadInteger("fsjk", "plsc", 0);
            string msg = f.ReadString("savetohis", "msg","1");
            if (plsc != 1)
            {
              

                ////增加体检回写接口/////zgq
                if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
                {
                 
                    # region  体检回写接口
                    if (jcxx.Rows[0]["F_BRBH"].ToString().Trim() == "")
                    {
                        ZgqClass.BGHJ(blh, "体检接口", "体检审核", "体检病人无病人编号，不处理", "ZGQJK", "体检接口");
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='体检病人无病人编号，不处理'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");

                        log.WriteMyLog(blh + ",体检病人无病人编号，不处理！");
                        if (jkmsg == "1")
                            MessageBox.Show("此体检病人无病人编号，不处理！");
                        return;
                    }
        
                      string err_msg = "";
                        string constr = f.ReadString("savetohis", "tj_odbcsql", "Provider='MSDAORA';data source=ZSYYTJ;User ID=SD_PE_LIS;Password=SD_PE_LIS;");
                    OleDbDB db = new OleDbDB();


                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

                        if (debug == "1")
                            log.WriteMyLog("执行体检");

                       //体检确认
                        if (jcxx.Rows[0]["F_TJBJ"].ToString().Trim() != "1" )
                        {
                            try
                            {
                             
                               OleDbParameter[] oledbPt = new OleDbParameter[4];

                               for (int j = 0; j < oledbPt.Length; j++)
                               {
                                   oledbPt[j] = new OleDbParameter();
                               }
                               oledbPt[0].ParameterName = "para_sfywid";
                               oledbPt[0].OleDbType = OleDbType.Decimal;
                               oledbPt[0].Direction = ParameterDirection.Input;
                               oledbPt[0].Size = 10;
                               oledbPt[0].Value = Decimal.Parse(jcxx.Rows[0]["F_BRBH"].ToString().Trim());

                               oledbPt[1].ParameterName = "para_flag";//
                               oledbPt[1].OleDbType = OleDbType.Char;
                               oledbPt[1].Direction = ParameterDirection.Input;
                               oledbPt[1].Size = 1;
                               oledbPt[1].Value = '1';

                               oledbPt[2].ParameterName = "para_result";//
                               oledbPt[2].OleDbType = OleDbType.Char;
                               oledbPt[2].Direction = ParameterDirection.Output;
                               oledbPt[2].Size = 1;

                               oledbPt[3].ParameterName = "para_msg";//
                               oledbPt[3].OleDbType = OleDbType.VarChar;
                               oledbPt[3].Direction = ParameterDirection.Output;
                               oledbPt[3].Size = 200;

                               if (debug == "1")
                                   log.WriteMyLog("执行标记:" + oledbPt[0].Value.ToString() + "@" + oledbPt[1].Value.ToString() + "@" + err_msg);

                               //删除
                               db.ExecuteNonQuery(constr, "updateBL", ref oledbPt, CommandType.StoredProcedure, ref err_msg);

                                if (debug == "1")
                                    log.WriteMyLog("体检执行标记返回：" + oledbPt[2].Value.ToString() + "@" + oledbPt[3].Value.ToString() + "@" + err_msg);

                                if (oledbPt[2].Value.ToString() != "Y")
                                    log.WriteMyLog("体检确认失败：" + oledbPt[2].Value.ToString() + "@" + oledbPt[3].Value.ToString() + "\r\n" + err_msg);
                                else
                                {
                                    aa.ExecuteSQL("update T_JCXX  set F_TJBJ='1' where F_BLH='" + blh + "'");
                                }
                            }
                            catch(Exception  e1)
                            {
                                log.WriteMyLog(e1.Message);
                            }
                            //                    确认过程：updateBL
                            //--para_sfywid 收费业务ID
                            //--para_flag   0：取消 1：确认
                            //--para_result  N:失败 Y:成功
                            //procedure updateBL(para_sfywid in  number, para_flag   in  char， para_result out char);

                            
                        }
                        if (bgzt.Trim() == "已审核")
                        {
 
                            string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                          
                            string jpgname = "";
                            string jpgpath = "";
            

                            //生成jpg
                            bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.JPG, ref err_msg, ref jpgname, ref jpgpath);
                           
                            if (isrtn)
                            {
                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                               ZgqClass.BGHJ(blh, "体检接口", "体检审核", "生成jpg成功:" + jpgpath + "\\" + jpgname, "ZGQJK", "体检生成jpg");
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                            }
                            else
                            {
                                log.WriteMyLog(blh + "-" + err_msg);
                                ZgqClass.BGHJ(blh, "体检接口", "体检审核", "生成JPG格式图片失败" + err_msg, "ZGQJK", "体检接口");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='生成JPG格式图片失败：" + err_msg + "'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                if (jkmsg == "1" || msg == "1")
                                    MessageBox.Show("病理号：" + blh + "  生成JPG格式图片失败，请重新审核！\r\n" + err_msg);

                                return;
                            }

                            # region 回传体检报告



                            // 诊断描述
                            string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                            //诊断结论	Res_con
                            string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

                            if (TJ_bljc.Rows.Count > 0)
                            {
                                if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                                {
                                    Res_char = Res_char + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n";

                                    Res_char = Res_char + "细胞量：" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n细胞成分：" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim()
                                        + "\r\n炎细胞：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";

                                    Res_char = Res_char + "微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n";

                                    ///////////诊断/////////////////////////
                                    Res_con = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                                    if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                        Res_con = Res_con + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                }
                            }

                            /////////////////////////////////////////////////////
                            string path = f.ReadString("savetohis", "jpgpath", @"\\192.10.33.84\pdfbg\");
                            jpgpath = path + jpgpath + "\\" + jpgname;
                            string sql_insert = "insert into BL_TJ_TJJGB0(SFYWID,TJH000,SQXMID,TZMS00,KSZD00,YXLJ00,JYYS00,SHYS00,JYRQ00,JYSJ00,SHRQ00,SHSJ00,JCXMLX,SFYX00)"
                            + " values(" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "," + jcxx.Rows[0]["F_mzh"].ToString().Trim() + "," + jcxx.Rows[0]["F_YZXM"].ToString().Trim().Split('^')[0] + ",'"
                            + Res_char + "','" + Res_con + "','" + jpgpath + "','" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "','" + jcxx.Rows[0]["F_shys"].ToString().Trim() + "','"
                            + DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMdd") + "','" + DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("HH:mm:ss")
                            + "','" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMdd") + "','" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("HH:mm:ss")
                            + "','BL','0')";

                           
                            string sql_del = "delete from BL_TJ_TJJGB0  where SFYWID=" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + " and TJH000=" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + " and SQXMID="
                                  + jcxx.Rows[0]["F_YZXM"].ToString().Trim().Split('^')[0] + " and JCXMLX='BL'";
                            
                            //删除 
                            if (debug == "1")
                                log.WriteMyLog("删除体检表，语句："+sql_del);

                          int x=  db.ExecuteNonQuery(constr, sql_del, ref err_msg);
                            if (debug == "1")
                                log.WriteMyLog("删除体检表：" + err_msg + "@" + x.ToString());
              

                             if (debug == "1")
                                log.WriteMyLog("回写体检表，语句：" + sql_insert );
                            //插入
                             x = db.ExecuteNonQuery(constr, sql_insert, ref err_msg);

                            if (debug == "1")
                                 log.WriteMyLog("回写体检表：" + err_msg+"@"+x.ToString());
                           
                            if (x < 0)
                            {
                                ZgqClass.BGHJ(blh, "体检接口", "体检审核", "回写体检报告失败：" + err_msg, "ZGQJK", "体检接口");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写体检报告失败：" + err_msg + "'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                log.WriteMyLog(blh + "-" + err_msg);
                                if (jkmsg == "1" || msg == "1")
                                    MessageBox.Show("病理号：" + blh + "-回写体检报告失败，请重新审核！\r\n" + err_msg);

                            }
                            else
                            {
                                ZgqClass.BGHJ(blh, "体检接口", "体检审核", "回写体检报告成功", "ZGQJK", "体检接口");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                aa.ExecuteSQL("update T_JCXX  set F_TJBJ='2' where F_BLH='" + blh + "'");
                                if (jkmsg == "1")
                                    MessageBox.Show("病理号：" + blh + "-回写体检报告成功");
                            }

                            return;
                            #endregion
                        }
                        else
                        {
                            if (bgzt == "取消审核")
                            {

                                //删除中间表
                                string str_sql = "delete from BL_TJ_TJJGB0  where SFYWID=" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + " and TJH000=" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + " and SQXMID="
                                    + jcxx.Rows[0]["F_YZXM"].ToString().Trim().Split('^')[0] + " and JCXMLX='BL'";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("回写体检表，语句：" + str_sql);
                                }

                                int x = db.ExecuteNonQuery(constr, str_sql, ref err_msg);
                                if (x < 0)
                                {
                                    ZgqClass.BGHJ(blh, "体检接口", "体检取消审核", "取消体检报告失败：" + err_msg, "ZGQJK", "体检接口");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='取消体检报告失败：" + err_msg + "'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                    log.WriteMyLog(blh + "-" + err_msg);
                                    if (msg == "1")
                                        MessageBox.Show("病理号：" + blh + "-取消体检报告失败！\r\n");
                                }
                                else
                                {
                                    ZgqClass.BGHJ(blh, "体检接口", "体检取消审核", "取消体检报告成功", "ZGQJK", "体检接口");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                    aa.ExecuteSQL("update T_JCXX  set F_TJBJ='1' where F_BLH='" + blh + "'");
                                }

                                //删除T_BG_PDF记录
                                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                                if (dt2.Rows.Count > 0)
                                {
                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    //删除ftp上pdf文件
                                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                                    string rtn_msg = "";
                                    zgq.DelPDFFile(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
                                 
                                }
                                else
                                {
                                    log.WriteMyLog(blh + ",T_BG_PDF中未能找到记录");
                                }
                                return;
                                  
                            }
                            return;

                        }
                        return;


                    # endregion
                } 
            else
                {
                    #region  生成pdf


                    //非体检病人回写
               
                    string bgzt2 = "";
                    try
                    {
                        if (bglx.ToLower().Trim() == "bd")
                        {
                            DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                            bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                        }
                        if (bglx.ToLower().Trim() == "bc")
                        {
                            DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                            bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                        }
                        if (bglx.ToLower().Trim() == "cg")
                        {
                            // DataTable jcxx2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                            bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                        }
                    }
                    catch(Exception  e5)
                    {
                        log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh+e5.Message);
                    }

                    if (bgzt2.Trim() == "")
                    {
                        log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);
                    }

                    if (bgzt2.Trim() == "已审核" && bgzt != "取消审核")
                    {

                        string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                        string err_msg = "";
                        string jpgname = "";
                        string jpgpath = "";
                      

                        //生成jpg
                        bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.PDF, ref err_msg, ref jpgname, ref jpgpath);
                        if (isrtn)
                        {

                            jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                            ZgqClass.BGHJ(blh, "his接口", "报告审核", "生成pdf成功:" + ML + "\\" + jpgname, "ZGQJK", "pdf");
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='"+bglx+"' and F_BGXH='"+bgxh+"'");
                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                        }
                        else
                        {
                            log.WriteMyLog(blh + "-" + err_msg);
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='生成pdf失败" + err_msg+"'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            ZgqClass.BGHJ(blh, "his接口", "审核PDF", "生成pdf失败" + err_msg, "ZGQJK", "pdf");
                            if (msg == "1" || jkmsg == "1")
                                MessageBox.Show("病理号：" + blh + "  生成pdf失败，请重新审核！\r\n" + err_msg);
                            return;
                        }
                    }
                    else
                    {
                        if (bgzt == "取消审核")
                        {
                            DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                            if (dt2.Rows.Count > 0)
                            {

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            }
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        }
                    }

                    return;

                    # endregion
                }

            }
            else
            {
                //批量上传

            }
        }

/// <summary>
 ///  生成pdf或jpg
/// </summary>
/// <param name="blh"></param>
/// <param name="bglx"></param>
/// <param name="bgxh"></param>
/// <param name="ML"></param>
/// <param name="jpgpdf"></param>
/// <param name="err_msg"></param>
/// <param name="fileName"></param>
/// <param name="fielPath"></param>
/// <returns></returns>
        public bool MD_PDF_JPG(string blh, string bglx, string bgxh, string ML, ZgqPDFJPG.Type jpgpdf, ref string err_msg, ref string fileName, ref string fielPath)
        {

             
                string message = ""; string jpgname = "";
                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf, ref message, ref jpgname);
              
                string xy = f.ReadString("ZGQJK", "sctxfs", "3");
                if (isrtn)
                {
                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                    if (ssa == true)
                    {
                        //jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                        fileName=jpgname;
                        fielPath=ML+"\\"+blh;
                        err_msg = "";
                        zgq.DelTempFile(blh);
                        return true;

                     }
                    else
                    {
                        err_msg = message;
                        zgq.DelTempFile(blh);
                        return false;
                    }
                }
                else
                {
                    zgq.DelTempFile(blh);
                    err_msg = message;
                    return false;
                }
        }
    }
}
