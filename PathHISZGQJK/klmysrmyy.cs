using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using PathHISZGQJK;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class klmysrmyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        private static string blhgy = "";
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        public void pathtohis(string blh, string yymc)
        {

            string bglx = "cg";
            string bgxh = "0";
            //安全校验凭证，凭证既是接口的检验码，也是调用方的身份标识，由集成平台提供给LIS、PACS.
            string certificate = "7pzOrESsiv8VnB6RD2FXmndLaJCYpiY7";

            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
            string debug = f.ReadString("savetohis", "debug", "");
          
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "体检" && bljc.Rows[0]["F_sqxh"].ToString().Trim()=="")
            {
                pathtoTJ(blh,bglx,bgxh, bljc,  debug);
                return;
            }

            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }

            string visitType = "";   ////	--就诊类型（1门急诊2住院 3体检）
            string visitNo = "";      //门诊号或住院号
            string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
            if (brlb == "门诊")
            {
                visitType = "1";
                visitNo = bljc.Rows[0]["F_MZH"].ToString().Trim();
            }
            if (brlb == "住院")
            {
                visitType = "2";
                visitNo = bljc.Rows[0]["F_zyh"].ToString().Trim();
            }
            if (brlb == "体检")
                visitType = "3";

            //string sqxh=bljc.Rows[0]["F_sqxh"].ToString().Trim();
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
            //string pacsBillNo = bljc.Rows[0]["F_blh"].ToString().Trim();

            string ztbm = "3";   //状态编号
            int reAuditFlag = 0;//是否重复审核
            if (zt == "已登记")
            {
                ztbm = "3";
            }
            if (zt == "已取材")
            {
                ztbm = "5";
            }

            if (zt == "已写报告")
            {
                ztbm = "6";
            }

            int F_SFCFSH = 0;

            try
            {

                F_SFCFSH = int.Parse(bljc.Rows[0]["F_SFCFSH"].ToString().Trim());

            }
            catch
            {
            }

            if (zt == "已审核")
            {
                ztbm = "7";

                if (F_SFCFSH >= 1)
                    reAuditFlag = 1;
            }

            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", ""); ;
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "");
            //  // 更改报告状态
            //if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            // sqxh = " ";

            string xmlstr = "<?xml version='1.0' encoding='UTF-8'?>";
            try
            {
                xmlstr = xmlstr + "<root>";
                xmlstr = xmlstr + "<visitType>" + visitType + "</visitType>";//	--就诊类型（1门急诊2住院 3体检）
                xmlstr = xmlstr + "<visitNo>" + bljc.Rows[0]["F_by2"].ToString().Trim() + "</visitNo>";//--门诊挂号号/住院号/体检号/条码号
                xmlstr = xmlstr + "<patientId>" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</patientId>";// -- 病人ID 
                xmlstr = xmlstr + "<pacsBillNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</pacsBillNo>";//--pacs报告单号
                xmlstr = xmlstr + "<hisApplyNo>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</hisApplyNo>";//--his单据号
                xmlstr = xmlstr + "<clinicCode></clinicCode>";//HIS收费项目代码
                xmlstr = xmlstr + "<clinicName></clinicName>";// 项目名称
                xmlstr = xmlstr + "<reportStatus>" + ztbm + "</reportStatus>";// -- 报告状态编码
                xmlstr = xmlstr + "<reAuditFlag>" + reAuditFlag + "</reAuditFlag>";//-重复审核标志：1-标识重复审核
                xmlstr = xmlstr + "<changeTime>" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "</changeTime>";// -- 改变时间
                xmlstr = xmlstr + "<changeOperator>" + yhmc + "/" + yhbh + "</changeOperator>";  // --修改操作人：姓名/工号
                xmlstr = xmlstr + "</root>";
            }
            catch
            {
                if (msg == "1")
                    MessageBox.Show("生成xml异常！");
                log.WriteMyLog("生成xml异常");
                return;
            }

            try
            {
                klmyWeb.WSInterface klmy = new PathHISZGQJK.klmyWeb.WSInterface();
                string webServicesURL = f.ReadString("savetohis", "webServicesURL", "");
                if (webServicesURL.Trim() != "")
                    klmy.Url = webServicesURL;

                string ztstr = klmy.CallInterface("UpdatePacsReportStatus", xmlstr, "", certificate);

                if (ztstr.Contains("error"))
                {
                    if (msg == "1")
                        MessageBox.Show("回传报告状态失败！原因：" + ztstr);
                    log.WriteMyLog("回传报告状态失败！原因：" + ztstr);
                }
                else
                {
                    if (msg == "1")
                        MessageBox.Show("回传报告状态成功！");
                }


            }
            catch (Exception e)
            {
                if (msg == "1")
                    MessageBox.Show("回传报告状态异常！");
                log.WriteMyLog("回传报告状态异常！原因：" + e.ToString());
                return;
            }

            if (zt == "已审核")
            {
                try
                {
                    aa.ExecuteSQL("update T_JCXX set F_SFCFSH='" + (F_SFCFSH + 1).ToString() + "' where F_BLH='" + blh + "'");
                }
                catch
                {
                    return;
                }
            } return;
        }

        public void pathtoTJ(string blh, string bglx, string bgxh, DataTable jcxx, string debug)
        {


          ////增加体检回写接口/////zgq
            if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检" && jcxx.Rows[0]["F_brbh"].ToString().Trim() == "体检")
                {
                   // 生成pdf
                    string err_msg = "";
                    string constr = f.ReadString("savetohis", "tj_odbcsql", "Data Source=192.168.90.236;Initial Catalog=tj_xlms;User Id=xlmsuser;Password=topsky;");
  
                     SqlDB db = new SqlDB();
                  

                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                    {

                        string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                       
                        string jpgname = "";
                        string jpgpath = "";
                       
                        //生成jpg
                        bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.JPG, ref err_msg, ref jpgname, ref jpgpath);
                        if (isrtn)
                        {

                            jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                            ZgqClass.BGHJ(blh, "体检接口", "审核", "生成jpg成功:" + ML + "\\" + jpgname, "ZGQJK", "jpg");

                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");
                        }
                        else
                        {
                            log.WriteMyLog(blh + "-" + err_msg);
                            ZgqClass.BGHJ(blh, "体检接口", "审核", "生成jpg失败" + err_msg, "ZGQJK", "jpg");
                            if (debug== "1" )
                                MessageBox.Show("病理号：" + blh + "  生成jpg失败，请重新审核！\r\n" + err_msg);
                         //   return;
                        }

                        # region 回传体检报告
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

                        // 诊断描述
                        string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                        //诊断结论	Res_con
                        string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

                        if (TJ_bljc.Rows.Count > 0)
                        {
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                            {
                                Res_char = Res_char + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n";

                                Res_char = Res_char  + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim()
                                    + "\r\n病原微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() 
                                    + "\r\n"+TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n"+TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";

                                Res_char = Res_char + "炎症程度：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";

                                ///////////诊断/////////////////////////
                                Res_con = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    Res_con = Res_con + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                            }
                        }

                        /////////////////////////////////////////////////////
                        string path = f.ReadString("savetohis", "jpgpath", @"\\192.10.33.84\pdfbg\");
                        jpgpath = path + jpgpath + "\\" + jpgname;
                     
                        string sql_insert = "insert into tj_pacsjgb(Djlsh,Zhxmbh,jg,jcsj,Jl,shr,Shrq,Tpzt,jcqkfzsm,Tplj,czy,Sysdatetime,Jkid,flag)"
                            + " values('" + jcxx.Rows[0]["F_MZH"].ToString().Trim() + "','" + jcxx.Rows[0]["F_YZID"].ToString().Trim()+ "','"
                            + Res_con + "','" + Res_char + "','','" + jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "','"
                            + jcxx.Rows[0]["F_spare5"].ToString().Trim() + "','1','病理检查','" + jpgpath + ";','" + jcxx.Rows[0]["F_bgYS"].ToString().Trim() + "','" + DateTime.Now + "',2,'0')";



                        string sql_del = "delete from tj_pacsjgb  where Djlsh='" + jcxx.Rows[0]["F_MZH"].ToString().Trim() + "' and Zhxmbh='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "' and jcqkfzsm='病理检查'";

                        //删除 
                        if (debug == "1")
                            log.WriteMyLog("删除体检表，语句：" + sql_del);

                        int x = db.ExecuteNonQuery(constr, sql_del, ref err_msg);
                        if (debug == "1")
                            log.WriteMyLog("删除体检表：" + err_msg + "@" + x.ToString());


                        if (debug == "1")
                            log.WriteMyLog("回写体检表，语句：" + sql_insert);
                        //插入
                        x = db.ExecuteNonQuery(constr, sql_insert, ref err_msg);

                        if (debug == "1")
                            log.WriteMyLog("回写体检表：" + err_msg + "@" + x.ToString());

                        if (x < 0)
                        {
                            ZgqClass.BGHJ(blh, "体检接口", "审核", "回写体检报告失败：" + err_msg, "ZGQJK", "体检接口");

                            log.WriteMyLog(blh + "-" + err_msg);
                            if (debug == "1")
                                MessageBox.Show("病理号：" + blh + "-回写体检报告失败，请重新审核！\r\n" + err_msg);

                        }
                        else
                        {
                            ZgqClass.BGHJ(blh, "体检接口", "审核", "回写体检报告成功", "ZGQJK", "体检接口");
                            aa.ExecuteSQL("update T_JCXX  set F_TJBJ='2' where F_BLH='" + blh + "'");
                            if (debug == "1")
                                MessageBox.Show("病理号：" + blh + "-回写体检报告成功");
                        }

                        return;
                        #endregion
                    }
                    else
                    {
                        if (jcxx.Rows[0]["F_TJBJ"].ToString().Trim() == "2")
                        {
                            DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                            if (dt2.Rows.Count > 0)
                            {
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                //删除ftp上pdf文件
                                ZgqPDFJPG zgq = new ZgqPDFJPG();
                                string rtn_msg = "";
                                zgq.DelPDFFile(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);

                            }
                            string sql_del = "delete from tj_pacsjgb  where Djlsh='" + jcxx.Rows[0]["F_MZH"].ToString().Trim() + "' and Zhxmbh='" + jcxx.Rows[0]["F_YZID"].ToString().Trim()+ "' and jcqkfzsm='病理检查'";

                            //删除 
                            if (debug == "1")
                                log.WriteMyLog("删除体检表，语句：" + sql_del);

                            int x = db.ExecuteNonQuery(constr, sql_del, ref err_msg);

                            if (debug == "1")
                                log.WriteMyLog("删除体检表：" + err_msg + "@" + x.ToString());

                            return;
                        }
                    }

                 
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
            bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf, ref jpgname, "", ref message);

            string xy = f.ReadString("ZGQJK", "sctxfs", "3");
            if (isrtn)
            {
                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                if (ssa == true)
                {
                    //jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                    fileName = jpgname;
                    fielPath = ML + "\\" + blh;
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
