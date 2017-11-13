using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using ZgqClassPub;
using ZgqClassPub.DBData;
namespace PathHISZGQJK
{
    //中山大学肿瘤医院
    class zsdxzlyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

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

            string tjodbcsql = f.ReadString("savetohis", "tj-odbcsql", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");

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
            {
                bgzt = "已审核";
            }


            int plsc = f.ReadInteger("fsjk", "plsc", 0);
            if (plsc != 1)
            {
                return;
            }
                ////增加体检回写接口/////zgq
                if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
                {
                    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                    {
                        aa.ExecuteSQL("update T_jcxx_fs set F_fszt='不处理',F_bz='体检病人无体检申请号,不处理！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
                        log.WriteMyLog(blh + ",体检病人无病人编号，不处理！");
                        return;
                    }


                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" && bgzt != "取消审核")
                    {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");


                        // 诊断描述
                        string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                        //诊断结论	Res_con
                        string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

                        if (TJ_bljc.Rows.Count > 0)
                        {
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "体检LCT" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "液基细胞")
                            {
                                Res_char = Res_char + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";

                                Res_char = Res_char + "项目：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

                                Res_char = Res_char + "病原体：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
                                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

                                Res_char = Res_char + "炎症细胞量：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

                                ///////////诊断/////////////////////////
                                Res_con = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    Res_con = Res_con + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                            }
                        }

                        /////////////////////////////////////////////////////

                        string str_com = "update  tj_pacs_resulto_temp  set Res_doctor='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',Res_doctor_code='',Res_date='" + DateTime.Parse(jcxx.Rows[0]["F_BGrq"].ToString().Trim()) + "',Res_char='" + Res_char + "',Res_con='" + Res_con + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";


                        if (debug == "1")
                        {
                            MessageBox.Show("回写体检表，语句：" + str_com);
                        }
                        SqlDB db = new SqlDB();
                        string Exceptionmessage = "";
                        int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref Exceptionmessage);
                        if (Exceptionmessage != "" && Exceptionmessage != "OK")
                        {

                            if (Exceptionmessage.Length > 200)
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                            else
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

                            log.WriteMyLog(blh + ",体检报告审核，接口异常信息：" + Exceptionmessage);
                        }
                        else
                        {
                            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告审核,接口上传成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
                        }

                    }
                    else
                    {
                        //取消审核时
                        if (bgzt == "取消审核")
                        {
                            string str_com = "update  tj_pacs_resulto_temp set Res_doctor='" + "" + "',Res_doctor_code='',Res_date='" + DateTime.Today + "',Res_char='" + "" + "',Res_con='" + "" + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";

                            if (debug == "1")
                            {
                                MessageBox.Show("回写体检表，语句：" + str_com);
                            }
                            SqlDB db = new SqlDB();
                            string Exceptionmessage = "";
                            int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref Exceptionmessage);
                            if (Exceptionmessage != "" && Exceptionmessage != "OK")
                            {
                                log.WriteMyLog(Exceptionmessage);

                                if (Exceptionmessage.Length > 200)
                                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                                else
                                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

                                log.WriteMyLog(blh + ",体检报告取消审核，接口异常信息：" + Exceptionmessage);
                            }
                            else
                            {
                                aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告取消审核，接口上传成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
                            }
                        }
                        else
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='未知操作！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");

                    }
                }
                else
                {
                    //非体检病人回写
                    //生成pdf 用于移动app
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
                    catch
                    {

                    }

                    if (bgzt2.Trim() == "")
                    {
                        log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);
                    }

                    if (bgzt2.Trim() == "已审核" && bgzt != "取消审核")
                    {


                        ////MD_JPG_PDF md = new MD_JPG_PDF();
                        ////int x = md.CreatePDF(blh, bglx, bgxh, MD_JPG_PDF.type.PDF, ref message);
                        ////if (x <= 0)
                        ////{
                        ////     LGZGQClass.log.WriteMyLog(message);
                        ////    if (message.Length > 200)
                        ////        aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + message.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                        ////    else
                        ////        aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

                        ////}
                        ////else
                        ////    aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='审核, 生成PDF成功' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");


                        string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                        //string ML = "";
                        string message = ""; string jpgname = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", ref message);

                        string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                        if (isrtn)
                        {
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                            if (ssa == true)
                            {
                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "批量上传", "审核", "生成PDF成功:" + ML + "\\" + jpgname, "ZGQJK", "生成PDF");

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='生成PDF成功',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                            }
                            else
                            {
                                log.WriteMyLog(message);
                                ZgqClass.BGHJ(blh, "批量上传", "审核", message, "ZGQJK", "生成PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_bz='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            }
                            zgq.DelTempFile(blh);

                        }
                        else
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    
                    }
                    else
                    {
                        if (bgzt == "取消审核")
                        {
                            DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                            if (dt2.Rows.Count <= 0)
                                aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='取消审核,删除PDF成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                            else
                            {
                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            ZgqPDFJPG zgq = new ZgqPDFJPG();
                            string rtn_msg = "";
                            zgq.DelPDFFile("", dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='取消审核,删除PDF成功！',F_JPG_errmsg='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                            }
                        }
                        else
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='未知操作！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
                    }
                  
                }
        }



        //20160104修改前
        //public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        //{
        //    string qxsh = "";
        //    string xdj = "";
        //    bglx = bglx.ToLower();

        //    if (cslb.Length == 5)
        //    {


        //        if (cslb[4].ToLower() == "qxsh")
        //        {
        //            //取消审核动作
        //            qxsh = "1";
        //        }

        //        if (cslb[3].ToLower() == "new")
        //        {
        //            xdj = "1";
        //        }

        //    }

        //    string tjodbcsql = f.ReadString("savetohis", "tj-odbcsql", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");

        //    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        //    DataTable jcxx = new DataTable();
        //    try
        //    {
        //        jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //        return;
        //    }
        //    if (jcxx == null)
        //    {
        //        MessageBox.Show("病理数据库设置有问题！");
        //        return;
        //    }
        //    if (jcxx.Rows.Count < 1)
        //    {
        //        MessageBox.Show("病理号有错误！");
        //        return;
        //    }

        //    if (bglx.Trim() == "")
        //    {
        //         LGZGQClass.log.WriteMyLog("报告类型为空，不处理！" + blh + "^" + bglx + "^" + bgxh);
        //        return;
        //    }
        //    string bgzt = "";
        //    if (qxsh == "1")
        //    {
        //        bgzt = "取消审核";
        //    }
        //    else
        //    {
        //        bgzt = "已审核";
        //    }


        //    int plsc = f.ReadInteger("fsjk", "plsc", 0);
        //    if (plsc != 1)
        //    {
        //        return;
        //    }
        //    ////增加体检回写接口/////zgq
        //    if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
        //    {
        //        if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
        //        {
        //            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='不处理',F_bz='体检病人无体检申请号,不处理！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //             LGZGQClass.log.WriteMyLog(blh + ",体检病人无病人编号，不处理！");
        //            return;
        //        }


        //        if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" && bgzt != "取消审核")
        //        {
        //            DataTable TJ_bljc = new DataTable();
        //            TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");


        //            // 诊断描述
        //            string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
        //            //诊断结论	Res_con
        //            string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

        //            if (TJ_bljc.Rows.Count > 0)
        //            {
        //                if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "体检LCT" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "液基细胞")
        //                {
        //                    Res_char = Res_char + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";

        //                    Res_char = Res_char + "项目：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
        //                        + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

        //                    Res_char = Res_char + "病原体：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
        //                        + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

        //                    Res_char = Res_char + "炎症细胞量：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

        //                    ///////////诊断/////////////////////////
        //                    Res_con = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
        //                    if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
        //                        Res_con = Res_con + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
        //                }
        //            }

        //            /////////////////////////////////////////////////////

        //            string str_com = "update  tj_pacs_resulto_temp  set Res_doctor='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',Res_doctor_code='',Res_date='" + DateTime.Parse(jcxx.Rows[0]["F_BGrq"].ToString().Trim()) + "',Res_char='" + Res_char + "',Res_con='" + Res_con + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";


        //            if (debug == "1")
        //            {
        //                MessageBox.Show("回写体检表，语句：" + str_com);
        //            }
        //            SqlDB db = new SqlDB();
        //            string Exceptionmessage = "";
        //            int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref Exceptionmessage);
        //            if (Exceptionmessage != "" && Exceptionmessage != "OK")
        //            {

        //                if (Exceptionmessage.Length > 200)
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //                else
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

        //                 LGZGQClass.log.WriteMyLog(blh + ",体检报告审核，接口异常信息：" + Exceptionmessage);
        //            }
        //            else
        //            {
        //                aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告审核,接口上传成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //            }

        //        }
        //        else
        //        {
        //            //取消审核时
        //            if (bgzt == "取消审核")
        //            {
        //                string str_com = "update  tj_pacs_resulto_temp set Res_doctor='" + "" + "',Res_doctor_code='',Res_date='" + DateTime.Today + "',Res_char='" + "" + "',Res_con='" + "" + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";

        //                if (debug == "1")
        //                {
        //                    MessageBox.Show("回写体检表，语句：" + str_com);
        //                }
        //                SqlDB db = new SqlDB();
        //                string Exceptionmessage = "";
        //                int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref Exceptionmessage);
        //                if (Exceptionmessage != "" && Exceptionmessage != "OK")
        //                {
        //                     LGZGQClass.log.WriteMyLog(Exceptionmessage);

        //                    if (Exceptionmessage.Length > 200)
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //                    else
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

        //                     LGZGQClass.log.WriteMyLog(blh + ",体检报告取消审核，接口异常信息：" + Exceptionmessage);
        //                }
        //                else
        //                {
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告取消审核，接口上传成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //                }
        //            }
        //            else
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='未知操作！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");

        //        }
        //    }
        //    else
        //    {
        //        //非体检病人回写
        //        //生成pdf 用于移动app
        //        string bgzt2 = "";
        //        try
        //        {
        //            if (bglx.ToLower().Trim() == "bd")
        //            {
        //                DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
        //                bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
        //            }
        //            if (bglx.ToLower().Trim() == "bc")
        //            {
        //                DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
        //                bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

        //            }
        //            if (bglx.ToLower().Trim() == "cg")
        //            {
        //                // DataTable jcxx2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
        //                bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
        //            }
        //        }
        //        catch
        //        {

        //        }

        //        if (bgzt2.Trim() == "")
        //        {
        //             LGZGQClass.log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);
        //        }

        //        if (bgzt2.Trim() == "已审核" && bgzt != "取消审核")
        //        {
        //            string message = "";
        //            MD_JPG_PDF md = new MD_JPG_PDF();
        //            int x = md.CreatePDF(blh, bglx, bgxh, MD_JPG_PDF.type.PDF, ref message);
        //            if (x <= 0)
        //            {
        //                 LGZGQClass.log.WriteMyLog(message);
        //                if (message.Length > 200)
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + message.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //                else
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

        //            }
        //            else
        //                aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='审核, 生成PDF成功' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //        }
        //        else
        //        {
        //            if (bgzt == "取消审核")
        //            {
        //                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
        //                if (dt2.Rows.Count <= 0)
        //                    aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='取消审核,删除PDF成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //                else
        //                {


        //                    MD_JPG_PDF md = new MD_JPG_PDF();
        //                    string message = "";
        //                    bool rtndel = md.Delete(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref message);

        //                    if (aa.ExecuteSQL("delete T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'") > 0)
        //                    {
        //                        aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='取消审核,删除PDF成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //                    }
        //                    else
        //                    {
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='取消审核,删除PDF失败！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
        //                    }
        //                }
        //            }
        //            else
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='未知操作！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");


        //        }
        //        return;
        //    }



        //}
   
    }
}
