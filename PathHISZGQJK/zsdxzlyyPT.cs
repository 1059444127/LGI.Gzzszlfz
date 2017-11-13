using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.IO;
using ZgqClassPub;
namespace PathHISZGQJK
{
    //中山大学肿瘤医院
   public  class zsdxzlyyPT
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
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

            string tjodbcsql = f.ReadString("savetohis", "tj-odbcsql", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");
            
          
          
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

           bgzt=jcxx.Rows[0]["F_BGZT"].ToString().Trim();
           string sqxh = jcxx.Rows[0]["F_sqxh"].ToString().Trim();
            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                    bgzt = "取消审核";
            }
            string errMsg = "";
                 string hczt = f.ReadString("zgqjk", "hczt", "1").Trim();
                string hcbg = f.ReadString("zgqjk", "hcbg", "1").Trim();
                string yhmc = f.ReadString("yh", "yhmc", "1").Trim();
                string yhbh = f.ReadString("yh", "yhbh", "1").Trim();
            if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
            {
                if(debug=="1")
                log.WriteMyLog("体检回传");
                  string tjjk = f.ReadString("zgqjk", "tjjk", "1").Trim();
                string tjtoptjk = f.ReadString("zgqjk", "tjtoptjk", "1").Trim();
                ////if (tjjk == "1")
                ////{
                ////    #region 体检接口
                ////    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                ////    {
                ////        aa.ExecuteSQL("update T_jcxx_fs set F_fszt='不处理',F_bz='体检病人无体检申请号,不处理！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                ////       log.WriteMyLog(blh + ",体检病人无病人编号，不处理！");
                ////        return;
                ////    }

                ////    if (bgzt == "已审核")
                ////    {
                ////        DataTable TJ_bljc = new DataTable();
                ////        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                ////        // 诊断描述
                ////        string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                ////        //诊断结论	Res_con
                ////        string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                ////        if (TJ_bljc.Rows.Count > 0)
                ////        {
                ////            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "体检LCT" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "液基细胞")
                ////            {
                ////                Res_char = Res_char + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";

                ////                Res_char = Res_char + "项目：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                ////                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

                ////                Res_char = Res_char + "病原体：" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
                ////                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

                ////                Res_char = Res_char + "炎症细胞量：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

                ////                ///////////诊断/////////////////////////
                ////                Res_con = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                ////                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                ////                    Res_con = Res_con + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                ////            }
                ////        }
                ////        string str_com = "update  tj_pacs_resulto_temp  set Res_doctor='" + jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',Res_doctor_code='',Res_date='" + DateTime.Parse(jcxx.Rows[0]["F_BGrq"].ToString().Trim()) + "',Res_char='" + Res_char + "',Res_con='" + Res_con + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                ////        if (debug == "1")
                ////           log.WriteMyLog("回写体检表，语句：" + str_com);
                ////        SqlDB db = new SqlDB();

                ////        int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref errMsg);
                ////        if (errMsg != "" && errMsg != "OK")
                ////        {
                ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + errMsg + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                ////           log.WriteMyLog(blh + ",体检报告审核，接口异常信息：" + errMsg);
                ////        }
                ////        else
                ////        {
                ////           log.WriteMyLog(blh + ",体检报告发送完成");
                ////            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告审核,接口上传成功！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                ////        }
                ////    }
                ////    else
                ////    {
                ////        if (bgzt == "取消审核")
                ////        {
                ////            string str_com = "update  tj_pacs_resulto_temp set Res_doctor='" + "" + "',Res_doctor_code='',Res_date='" + DateTime.Today + "',Res_char='" + "" + "',Res_con='" + "" + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                ////            if (debug == "1")
                ////               log.WriteMyLog("回写体检表，语句：" + str_com);
                ////            SqlDB db = new SqlDB();

                ////            int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref errMsg);
                ////            if (errMsg != "" && errMsg != "OK")
                ////            {
                ////                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + errMsg + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                ////               log.WriteMyLog(blh + ",体检报告取消审核，接口异常信息：" + errMsg);
                ////            }
                ////            else
                ////                aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_bz='体检报告取消审核，接口上传成功！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                ////        }
                ////        else
                ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='未知操作！' where F_blbh='" + blbh + "'   and F_fszt='未处理'");
                ////    }
                ////    #endregion
                ////}
                if(tjtoptjk=="1")
                {
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
                        C_pdf(blh, bgxh, bglx, jcxx, false, ref PdfToBase64String, debug);
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
                            TJ_BgToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
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
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1"&& bgzt=="已审核")
                {
                    C_pdf(blh, bgxh, bglx, jcxx, false, ref PdfToBase64String, debug);
                }

                if (bgzt == "取消审核")
                     aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "'");

                if (ptjk == "1")
                {

                    if (hczt == "1" && sqxh!="")
                    {
                        if (bglx == "cg" && (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已审核"))
                        {
                            ZtToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                        }
                    }
                    if (hcbg == "1" && (bgzt == "已审核" || bgzt == "已发布"))
                        {
                            string bgzt2 = "";
                            try
                            {
                                if (bglx.ToLower().Trim() == "bd")
                                {
                                    dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                                    bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                                }
                                if (bglx.ToLower().Trim() == "bc")
                                {
                                    dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                                    bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                                }
                                if (bglx.ToLower().Trim() == "cg")
                                    bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                            }
                            catch { }
                      
                            BgToPt(jcxx, dt_bc, dt_bd, dt_sqd, blh, bglx, bgxh, bgzt2, yhmc, yhbh, debug);
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
       
        public void ZtToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
            if (bglx!="cg")
                return;
            if(dt_sqd.Rows.Count<=0)
            {
                log.WriteMyLog(blh + ",申请表中无记录");
                return;
            }
            string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
            string GUID = "";
            string bgztxml = ZtMsg(dt_jcxx, dt_sqd, dt_jcxx.Rows[0]["F_SQXH"].ToString(), blh, bglx, bgxh, bgzt, yhmc, yhbh,ref  GUID);
                    if (bgztxml.Trim()== "")
                    {
                         log.WriteMyLog(blh + ",报告状态生成xml为空");
                        return;
                    }
                    if (debug == "1")
                        log.WriteMyLog("状态[QI1_037Exam]:" + bgztxml);
                        try
                        {
                          
                            ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                                MQSer.Url = wsurl;
                                string msgtxt = "";
                                if (MQSer.SendMessageToMQ(bgztxml, ref msgtxt, "QI1_037Exam", "PIS_ExamState", GUID, "返回状态"))
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
        public void BgToPt(DataTable dt_jcxx, DataTable dt_bc, DataTable dt_bd, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {

            try
            {
                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;
                string url = f.ReadString("savetohis", "blweb", "http://172.16.95.230/pathwebrpt");
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
                    filePath = dt_pdf.Rows[0]["F_pdfPath"].ToString().Replace("ftp","http");
                }
                string GUID = "";
                string rtnxml = BgMsg(dt_jcxx, dt_bc, dt_bd, dt_tx, dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim(), blh, bglx, bgxh, bgzt, jzlb, yhmc, yhbh, filePath, url, ref GUID);
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
                        log.WriteMyLog("回传报告[QI1_001-PIS_Report]:" + rtnxml);

                    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                    MQSer.Url = wsurl;
                    if (MQSer.SendMessageToMQ(rtnxml, ref msgtxt, "QI1_001", "PIS_Report", GUID, "报告发布"))//PIS_Report
                    {
                        if (debug == "1")
                            log.WriteMyLog(blbh + ",发送成功:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送成功',F_FSZT='已处理' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    }
                    else
                    {
                        log.WriteMyLog(blbh + ",发送失败:" + msgtxt);
                         aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送失败：" + msgtxt + "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    }
                }
                catch (Exception ee4)
                {
                    log.WriteMyLog(blbh + ",发送异常：" + ee4.Message);
                     aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送异常：" + ee4.Message + "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");

                }
            }
            catch(Exception  ee5)
            {
                log.WriteMyLog(ee5.Message);
            }
            return;
        }
        public string BgMsg(DataTable dt_jcxx, DataTable dt_bc, DataTable dt_bd, DataTable dt_tx, string sqxh, string blh, string bglx, string bgxh, string bgzt, string jzlb, string yhmc, string yhbh, string filePath, string url,ref string GUID)
        {
            filePath = filePath.ToLower().Replace("ftp", "http");
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
              GUID= Guid.NewGuid().ToString();
            //生成xml
            string rtnxml = "<?xml version=\"1.0\"?>";
            rtnxml = rtnxml + "<PRPA_IN000003UV01>";
            try
            {
                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" +  GUID + "\"/>";
                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml + "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</receiver>";

                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
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
                rtnxml = rtnxml + "<adviceType>病理</adviceType>";
                ////<!--HIS病人类型 门诊 住院-->
                rtnxml = rtnxml + "<patienttype>" + dt_jcxx.Rows[0]["F_BRLB"].ToString() + "</patienttype>";
                ////<!-- 文档注册时间 -->
                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                ////<!-- 患者号 -->
                rtnxml = rtnxml + "<patientNo>" + dt_jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
                ////<!-- 患者就诊流水号 -->
                rtnxml = rtnxml + "<patientSerialNO>" + dt_jcxx.Rows[0]["F_ZY"].ToString() + "</patientSerialNO>";
                ////<!--HIS的医嘱流水号,标识一次申请-->
                rtnxml = rtnxml + "<adviceID>" + "" +"</adviceID>";
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
                    rtnxml = rtnxml + "<procedureCode>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] + "</procedureCode>";
                    try
                    {
                        rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] + "</procedureName>";
                    }
                    catch
                    {
                        rtnxml = rtnxml + "<procedureName></procedureName>";
                    }
                }
                else
                {
                    rtnxml = rtnxml + "<procedureCode>" + "" + "</procedureCode>";
                    rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "</procedureName>";
                }
                ////	<!--检查部位-->
                rtnxml = rtnxml + "<positionName>" + dt_jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] + "</positionName>";
                ////	<!--检查设备名称-->
                rtnxml = rtnxml + "<modalityName></modalityName>";
                ////	<!--报告类别，如CT，MRI等-->

                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
                ////	<!-- 报告医生编码 -->
                rtnxml = rtnxml + "<authorCode></authorCode>";
                ////	<!--报告医生-->
                if (bglx == "bc")
                {
                    rtnxml = rtnxml + "<authorName>" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_bc.Rows[0]["F_bc_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
                    ////	<!--肉眼所见-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
                    ////<!--镜下所见-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_JXSJ"].ToString() + "]]></microscopeDiagnose>";
                    ////	<!--报告结论-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bc.Rows[0]["F_BCZD"].ToString() + "]]></conclusionDiagnose>";
                    ////	<!--报告所见-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_tsjc"].ToString() + "]]></reportDiagnose>";
                }
                else if (bglx == "bd")
                {
                    rtnxml = rtnxml + "<authorName>" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_bd.Rows[0]["F_bd_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
                    ////	<!--肉眼所见-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
                    ////<!--镜下所见-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[]]></microscopeDiagnose>";
                    ////	<!--报告结论-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bd.Rows[0]["F_BDZD"].ToString() + "]]></conclusionDiagnose>";
                    ////	<!--报告所见-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[]]></reportDiagnose>";
                }
                else
                {
                    rtnxml = rtnxml + "<authorName>" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
                    ////	<!--肉眼所见-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
                    ////<!--镜下所见-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_JXSJ"].ToString() + "]]></microscopeDiagnose>";
                    ////	<!--报告结论-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_BLZD"].ToString() + "]]></conclusionDiagnose>";
                    ////	<!--报告所见-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_tsjc"].ToString() + "]]></reportDiagnose>";
                }

                ////	<!-- 医生所属科室编码 -->
                rtnxml = rtnxml + "<deptCode></deptCode>";
                ////	<!-- 医生所属科室 -->
                rtnxml = rtnxml + "<deptName>病理科</deptName>";

                ////	<!--报告所得-->
                rtnxml = rtnxml + "<reportContent></reportContent>";
                ////	<!-- 来源系统编码 -->
                rtnxml = rtnxml + "<sourceCode>PIS</sourceCode>";
                ////	<!-- 来源系统简称 -->
                rtnxml = rtnxml + "<sourceName>PIS</sourceName>";
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
                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + dt_jcxx.Rows[0]["F_txml"].ToString() + "/" + dt_tx.Rows[j]["F_TXM"] + "</imageURL>";
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
                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + "" + "</text>";
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
        public string ZtMsg( DataTable dt_jcxx,DataTable dt_sqd, string sqxh,string blh,string bglx,string bgxh,string bgzt,string yhmc,string yhbh,ref string GUID)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
             GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
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
                xml = xml + "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
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
                    xml = xml + "<code code=\"60\" displayName=\"报告发布\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else if (bgzt == "已写报告")
                    xml = xml + "<code code=\"50\" displayName=\"完成\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else 
                    xml = xml + "<code code=\"40\" displayName=\"执行\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";

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
                xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\""+yhbh+"\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + yhmc + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!--医疗卫生机构（科室）ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<!--所执行申请单或医嘱-->";
                xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                xml = xml + "<!--电子申请单编号-->";
                xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" +sqxh + "\"/>";
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
                    xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_JZLB"].ToString() + "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"患者类型代码表\" displayName=\"" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                xml = xml + "<statusCode/>";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<!--平台注册的患者ID -->";
                xml = xml + "<id root=\"2.999.1.37\" extension=\"" + dt_sqd.Rows[0]["F_EMPIID"].ToString() + "\"/>";
                xml = xml + "<!--本地系统的患者ID -->";
                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_patientid"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_ZLH"].ToString() + "\"/>";
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
        public void BgHSToPt(DataTable dt_jcxx, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
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
                        xml = xml + "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                        xml = xml + "</device>";
                        xml = xml + "</receiver>";

                        xml = xml + "<sender typeCode=\"SND\">";
                        xml = xml + "<telecom value=\"\"/>";
                        xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                        xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                        xml = xml + "<telecom value=\"设备编号\"/>";
                        xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                        xml = xml + "</device>";
                        xml = xml + "</sender>";

                        xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                        xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                        xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

                        xml = xml + "<!--报告流水号-->";
                        xml = xml + "<flowID>" + blbh+ "</flowID>";
                        xml = xml + "<!--报告类型 - 超声、放射、心电等-->";
                        xml = xml + "<adviceType>病理</adviceType>";
                        xml = xml + "<!--HIS病人类型 门诊 住院（必填）-->";
                        xml = xml + "<patienttype>" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "</patienttype>";
                        xml = xml + "<!-- 文档注册时间 -->";
                        xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                        xml = xml + "<!-- 来源系统编码（必填） -->";
                        xml = xml + "<sourceCode/>";
                        xml = xml + "<!-- 来源系统简称 -->";
                        xml = xml + "<sourceName>PIS</sourceName>";
                        xml = xml + "<!-- 文档编号（必填） -->";
                        xml = xml + "<indexInSystem>" + blbh + "</indexInSystem>";

                        xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
                    }
                    catch (Exception ee4)
                    {
                       log.WriteMyLog(blbh + ",报告召回XML异常：" + ee4.Message);
                         aa.ExecuteSQL("update T_jcxx_fs set F_BZ='召回XML异常：" + ee4.Message + "' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                        return;
                    }
                    if (xml.Trim() == "")
                    {
                       log.WriteMyLog(blbh + ",报告召回生成xml为空");
                         aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告召回xml为空' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                        return;
                    }

                    string msgtxt = "";
                    try
                    {
                        if (debug == "1")
                           log.WriteMyLog("报告召回：[QI1_001--PIS_ReportExpired]" + xml);

                 string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
               
                        MQSer.Url = wsurl;


                   if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_001", "PIS_ReportExpired", GUID,"报告回收"))
                        {
                            if (debug == "1")
                            {
                               log.WriteMyLog(blbh + ",报告召回成功：" + msgtxt);
                            }
                            aa.ExecuteSQL("update T_jcxx_fs set GUID='"+GUID+"',F_BZ='报告召回成功:" + msgtxt + "',F_FSZT='已处理' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                            return;
                        }
                        else
                        {
                           log.WriteMyLog(blbh + ",报告召回失败：" + msgtxt);
                            aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回失败:" + msgtxt + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                            return;
                        }
                    }
                    catch (Exception ee4)
                    {
                        log.WriteMyLog(blbh + ",报告召回异常：" + ee4.Message);
                         aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回异常:" + ee4.Message + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                        return;
                    }
        }
     
        //public void pathtohis2016(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        //{
        //    string qxsh = "";
        //    string xdj = "";
        //    bglx = bglx.ToLower();

        //     LGZGQClass.log.WriteMyLog("接口开始");

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


        //    if (bglx == "")
        //        bglx = "cg";

        //    if (bgxh == "")
        //        bgxh = "0";

        //    string tjodbcsql = f.ReadString("savetohis", "tj-odbcsql", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");
        //    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");

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


        //    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
        //    MQSer.Url = wsurl;



        //     LGZGQClass.log.WriteMyLog("开始回传状态");
        //    string msgtxt = "";

        //    if (jcxx.Rows[0]["F_HXBZ"].ToString().Trim() == "")
        //    {
        //        if (bglx == "cg")
        //        {
        //            #region  回传报告完成状态

        //            string[] yzhs = jcxx.Rows[0]["F_yzid"].ToString().Split('^');
        //            foreach (string yzh in yzhs)
        //            {
        //                if (yzh != "")
        //                {

        //                    string bgztxml = bgzt_XML("已登记", jcxx, yzh);
        //                     LGZGQClass.log.WriteMyLog(bgztxml);
        //                    if (bgztxml.Trim() != "")
        //                    {
        //                        try
        //                        {
        //                            if (MQSer.SendMessage(bgztxml, ref msgtxt, "QI1_037", string.Empty))
        //                            {
        //                                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告执行状态发送成功：" + msgtxt);
        //                                aa.ExecuteSQL("update T_jcxx set F_hxbz='1' where F_blh='" + blh + "'");

        //                            }
        //                            else
        //                            {
        //                                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告执行状态发送失败：" + msgtxt);
        //                            }
        //                        }
        //                        catch (Exception ee4)
        //                        {
        //                             LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告执行状态发送异常：" + ee4.Message);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }

        //    }
        //     LGZGQClass.log.WriteMyLog("开始回传完成");


        //    ////增加体检回写接口/////zgq

        //    string bgzt = "";

        //    if (qxsh == "1")
        //    {
        //        bgzt = "取消审核";
        //    }


        //    if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
        //    {
        //         LGZGQClass.log.WriteMyLog("体检回传");
        //        #region 体检接口
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
        //        #endregion
        //    }
        //    else
        //    {
        //         LGZGQClass.log.WriteMyLog("常规生成pdf");
        //        //非体检病人回写
        //        //生成pdf 用于移动app
        //        string bgzt2 = "";
        //        DataTable dt_bd = new DataTable();
        //        DataTable dt_bc = new DataTable();
        //        try
        //        {
        //            if (bglx.ToLower().Trim() == "bd")
        //            {
        //                dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
        //                bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
        //            }

        //            if (bglx.ToLower().Trim() == "bc")
        //            {
        //                dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
        //                bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
        //            }
        //            if (bglx.ToLower().Trim() == "cg")
        //              bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
        //        }
        //        catch
        //        {}

        //        if (bgzt2.Trim() == "")
        //        {
        //             LGZGQClass.log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);
        //        }
        //        if (bgzt2.Trim() == "已审核" && bgzt != "取消审核")
        //        {
        //            string ReprotFile = ""; string jpgname = "";
        //            string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
        //            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
        //            {
        //                #region  生成pdf

        //                //string ML = "";
        //                string message = "";
        //                ZgqClass zgq = new ZgqClass();
        //                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqClass.type.PDF, ref message, ref jpgname);

        //                string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
        //                if (isrtn)
        //                {
        //                     LGZGQClass.log.WriteMyLog("生成PDF成功");

        //                    //二进制串
        //                    if (File.Exists(jpgname))
        //                    {
        //                        try
        //                        {
        //                            FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
        //                            Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
        //                            file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
        //                            file.Close();
        //                            ReprotFile = Convert.ToBase64String(imgByte);
        //                             LGZGQClass.log.WriteMyLog("PDF转换二进制串成功");
        //                        }
        //                        catch (Exception ee)
        //                        {

        //                             LGZGQClass.log.WriteMyLog("PDF转换二进制串失败");
        //                        }
        //                    }


        //                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
        //                    if (ssa == true)
        //                    {
        //                         LGZGQClass.log.WriteMyLog("上传PDF成功");
        //                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
        //                        ZgqClass.BGHJ(blh, "批量上传", "审核", "生成PDF成功:" + ML + "\\" + jpgname, "ZGQJK", "生成PDF");

        //                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
        //                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
        //                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='生成PDF成功',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

        //                    }
        //                    else
        //                    {
        //                         LGZGQClass.log.WriteMyLog("生成PDF失败");
        //                         LGZGQClass.log.WriteMyLog(message);
        //                        ZgqClass.BGHJ(blh, "批量上传", "审核", message, "ZGQJK", "生成PDF");
        //                        aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_bz='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
        //                    }
        //                    zgq.DeleteTempFile(blh);

        //                }
        //                else
        //                    aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

        //                #endregion
        //            }
        //             LGZGQClass.log.WriteMyLog("生成pdf完成");
        //            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
        //            {
        //                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ", 申请序号为空,不处理");
        //                aa.ExecuteSQL("update T_jcxx_fs set F_FSZt='不处理',F_BZ='申请序号为空,不处理' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='已审核'");
        //                return;
        //            }
        //            string url = f.ReadString("savetohis", "blweb", "http://172.16.95.230/pathwebrpt");
        //            string err = "";
        //            msgtxt = "";

        //            if(debug=="1")
        //             LGZGQClass.log.WriteMyLog("回传报告状态");
        //            if (bglx == "cg")
        //            {
        //                #region  回传报告完成状态

        //                        string bgztxml = bgzt_XML("已审核", jcxx, "");
        //                         LGZGQClass.log.WriteMyLog("回传报告完成状态XML:" + bgztxml);
        //                        if (bgztxml.Trim() != "")
        //                        {
        //                            try
        //                            {
        //                                if (MQSer.SendMessage(bgztxml, ref msgtxt, "QI1_037", string.Empty))
        //                                {
        //                                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告完成状态发送成功：" + msgtxt);
        //                                }
        //                                else
        //                                {
        //                                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告完成状态发送失败：" + msgtxt);
        //                                }
        //                            }
        //                            catch (Exception ee4)
        //                            {
        //                                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告完成状态发送异常：" + ee4.Message);
        //                            }
        //                }
        //                #endregion
        //            }
        //             LGZGQClass.log.WriteMyLog("回传报告完成状态完成");

        //            #region 审核报告回传平台


        //            //生成xml
        //            string rtnxml = "<?xml version=\"1.0\"?>";
        //            rtnxml = rtnxml + "<PRPA_IN000003UV01>";
        //            try
        //            {
        //                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
        //                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
        //                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

        //                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
        //                rtnxml = rtnxml + "<telecom value=\"\"/>";
        //                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
        //                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
        //                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
        //                rtnxml = rtnxml + "<softwareName code=\"HIP\" displayName=\"医院信息平台\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
        //                rtnxml = rtnxml + "</device>";
        //                rtnxml = rtnxml + "</receiver>";

        //                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
        //                rtnxml = rtnxml + "<telecom value=\"\"/>";
        //                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
        //                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
        //                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
        //                rtnxml = rtnxml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
        //                rtnxml = rtnxml + "</device>";
        //                rtnxml = rtnxml + "</sender>";

        //                rtnxml = rtnxml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";

        //                rtnxml = rtnxml + "<authorOrPerformer typeCode=\"AUT\">";
        //                rtnxml = rtnxml + "<signatureText></signatureText>";
        //                rtnxml = rtnxml + "<assignedDevice classCode=\"ASSIGNED\"/>";
        //                rtnxml = rtnxml + "</authorOrPerformer>";
        //                rtnxml = rtnxml + "<subject typeCode=\"SUBJ\">";

        //                rtnxml = rtnxml + "<request>";
        //                ////!--报告流水号-->
        //                rtnxml = rtnxml + "<flowID>" + blh + "_" + bglx + "_" + bgxh + "</flowID>";
        //                ////<!--报告类型 - 超声、放射、心电等-->
        //                rtnxml = rtnxml + "<adviceType>病理</adviceType>";
        //                ////<!--HIS病人类型 门诊 住院-->

        //                string brlb = jcxx.Rows[0]["F_brlb"].ToString();
        //                if (brlb == "住院")
        //                    brlb = "2";
        //                else 
        //                    brlb = "1";
                       
        //                rtnxml = rtnxml + "<patienttype>" + brlb + "</patienttype>";
        //                ////<!-- 文档注册时间 -->
        //                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
        //                ////<!-- 患者号 -->
        //                rtnxml = rtnxml + "<patientNo>" + jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
        //                ////<!-- 患者就诊流水号 -->
        //                rtnxml = rtnxml + "<patientSerialNO>" + jcxx.Rows[0]["F_ZYH"].ToString() + "</patientSerialNO>";
        //                ////<!--HIS的医嘱流水号,标识一次申请-->
        //                rtnxml = rtnxml + "<adviceID>" + jcxx.Rows[0]["F_yzid"].ToString() + "</adviceID>";
        //                ////<!--的对应的申请单唯一标识号，标识一次检查-->
        //                rtnxml = rtnxml + "<accessionNO>" + jcxx.Rows[0]["F_sqxh"].ToString() + "</accessionNO>";
        //                ////<!--患者中文名-->
        //                rtnxml = rtnxml + "<patientName>" + jcxx.Rows[0]["F_xm"].ToString() + "</patientName>";
        //                ////<!--患者拼音名-->
        //                rtnxml = rtnxml + "<patientNameSpell></patientNameSpell>";
        //                ////<!--出生日期(例如：‘19870601’)-->
        //                rtnxml = rtnxml + "<birthDate></birthDate>";
        //                ////<!--性别-->
        //                rtnxml = rtnxml + "<sex>" + jcxx.Rows[0]["F_xb"].ToString() + "</sex>";
        //                ////	<!--检查项目代码-->
        //                rtnxml = rtnxml + "<procedureCode>" + jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] + "</procedureCode>";
        //                try
        //                {
        //                    rtnxml = rtnxml + "<procedureName>" + jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] + "</procedureName>";
        //                }
        //                catch
        //                {
        //                    rtnxml = rtnxml + "<procedureName></procedureName>";
        //                }
        //                ////	<!--检查部位-->
        //                rtnxml = rtnxml + "<positionName>" + jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] + "</positionName>";
        //                ////	<!--检查设备名称-->
        //                rtnxml = rtnxml + "<modalityName></modalityName>";
        //                ////	<!--报告类别，如CT，MRI等-->

        //                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
        //                ////	<!-- 报告医生编码 -->
        //                rtnxml = rtnxml + "<authorCode></authorCode>";
        //                ////	<!--报告医生-->
        //                if (bglx == "bc")
        //                {
        //                    rtnxml = rtnxml + "<authorName>" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "</authorName>";
        //                    ////	<!--报告时间-->
        //                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
        //                    ////	<!--审核医生-->
        //                    rtnxml = rtnxml + "<reportApprover>" + dt_bc.Rows[0]["F_bc_shys"].ToString() + "</reportApprover>";
        //                    ////	<!--审核时间-->
        //                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
        //                    ////	<!--肉眼所见-->
        //                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
        //                    ////<!--镜下所见-->
        //                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_JXSJ"].ToString() + "]]></microscopeDiagnose>";
        //                    ////	<!--报告结论-->
        //                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bc.Rows[0]["F_BCZD"].ToString() + "]]></conclusionDiagnose>";
        //                    ////	<!--报告所见-->
        //                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_tsjc"].ToString() + "]]></reportDiagnose>";
        //                }
        //                else if (bglx == "bd")
        //                {
        //                    rtnxml = rtnxml + "<authorName>" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "</authorName>";
        //                    ////	<!--报告时间-->
        //                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
        //                    ////	<!--审核医生-->
        //                    rtnxml = rtnxml + "<reportApprover>" + dt_bd.Rows[0]["F_bd_shys"].ToString() + "</reportApprover>";
        //                    ////	<!--审核时间-->
        //                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
        //                    ////	<!--肉眼所见-->
        //                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
        //                    ////<!--镜下所见-->
        //                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[]]></microscopeDiagnose>";
        //                    ////	<!--报告结论-->
        //                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bd.Rows[0]["F_BDZD"].ToString() + "]]></conclusionDiagnose>";
        //                    ////	<!--报告所见-->
        //                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[]]></reportDiagnose>";
        //                }
        //                else
        //                {
        //                    rtnxml = rtnxml + "<authorName>" + jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
        //                    ////	<!--报告时间-->
        //                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
        //                    ////	<!--审核医生-->
        //                    rtnxml = rtnxml + "<reportApprover>" + jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
        //                    ////	<!--审核时间-->
        //                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
        //                    ////	<!--肉眼所见-->
        //                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
        //                    ////<!--镜下所见-->
        //                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + jcxx.Rows[0]["F_JXSJ"].ToString() + "]]></microscopeDiagnose>";
        //                    ////	<!--报告结论-->
        //                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + jcxx.Rows[0]["F_BLZD"].ToString() + "]]></conclusionDiagnose>";
        //                    ////	<!--报告所见-->
        //                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + jcxx.Rows[0]["F_tsjc"].ToString() + "]]></reportDiagnose>";
        //                }

        //                ////	<!-- 医生所属科室编码 -->
        //                rtnxml = rtnxml + "<deptCode></deptCode>";
        //                ////	<!-- 医生所属科室 -->
        //                rtnxml = rtnxml + "<deptName>病理科</deptName>";

        //                ////	<!--报告所得-->
        //                rtnxml = rtnxml + "<reportContent></reportContent>";
        //                ////	<!-- 来源系统编码 -->
        //                rtnxml = rtnxml + "<sourceCode>1035</sourceCode>";
        //                ////	<!-- 来源系统简称 -->
        //                rtnxml = rtnxml + "<sourceName>PIS</sourceName>";
        //                rtnxml = rtnxml + "<providerOrganization>42321679-4</providerOrganization>";
        //                ////	<!-- 文档编号 -->
        //                rtnxml = rtnxml + "<indexInSystem>" + blh + "_" + bglx + "_" + bgxh + "</indexInSystem> ";
        //                ////	<!-- 文档类型编码 -->
        //                rtnxml = rtnxml + "<typeCode></typeCode>";
        //                ///	<!-- 文档类型名称 -->
        //                rtnxml = rtnxml + "<typeCodeName></typeCodeName>";
        //                ////	<!-- 文档标题 -->
        //                rtnxml = rtnxml + "<title></title>";
        //                try
        //                {
        //                    //////	<!--报告URL-->
        //                    //DataTable dt_pdf = new DataTable();
        //                    //dt_pdf = aa.GetDataTable("select top 10 * from dbo.T_BG_PDF where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' order by F_ID desc", "T_PDF");
        //                    //if (dt_pdf.Rows.Count > 0)
        //                    //    rtnxml = rtnxml + "<reportListURL>" + url + "/pdfbg/" + dt_pdf.Rows[0]["F_ml"].ToString() + "/" + dt_pdf.Rows[0]["F_filename"].ToString() + "</reportListURL>";
        //                    //else

        //                    rtnxml = rtnxml + "<reportListURL>" + url + "/pdfbg/" + ML + "/" + blh + "/" + jpgname + "</reportListURL>";
        //                }
        //                catch
        //                {
        //                    rtnxml = rtnxml + "<reportListURL></reportListURL>";
        //                }

        //                ////	<!-- 报告PDA文档二进制串 BASE64 -->
        //                rtnxml = rtnxml + "<body>";
        //                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + ReprotFile + "</text>";
        //                rtnxml = rtnxml + "</body>";
        //                ////	<!--影像调阅URL-->
        //                rtnxml = rtnxml + "<imageList>";
        //                try
        //                {
        //                    DataTable dt_jpg = new DataTable();
        //                    dt_jpg = aa.GetDataTable("select top 20 * from dbo.V_DYTX where F_BLH='" + jcxx.Rows[0]["F_BLH"].ToString() + "' ", "T_TX");
        //                    for (int j = 0; j < dt_jpg.Rows.Count; j++)
        //                    {
        //                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + jcxx.Rows[0]["F_txml"].ToString() + "/" + dt_jpg.Rows[j]["F_TXM"] + "</imageURL>";
        //                    }
        //                }
        //                catch
        //                {
        //                }
        //                rtnxml = rtnxml + "</imageList>";
        //                ////	<!--备用字段1-->";
        //                rtnxml = rtnxml + "<other1></other1>";
        //                ////	<!--备用字段2-->
        //                rtnxml = rtnxml + "<other2></other2>";
        //                ////	<!--备用字段3-->
        //                rtnxml = rtnxml + "<other3></other3>";
        //                ////	<!--结果更新标志；0-PACS新增，1-电子病历读取，2-PACS修改-->
        //                rtnxml = rtnxml + "<updateFlag></updateFlag>";
        //                rtnxml = rtnxml + "</request>";

        //                rtnxml = rtnxml + "</subject>";
        //                rtnxml = rtnxml + "</controlActProcess>";
        //                rtnxml = rtnxml + "</PRPA_IN000003UV01>";

        //                //if(debug=="1")
        //                // LGZGQClass.log.WriteMyLog(rtnxml);


        //            }
        //            catch (Exception e2)
        //            {
        //                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告生成XML异常：" + e2.Message);
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告生成XML异常：" + e2.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='已审核'");

        //                return;
        //            }

        //            if (debug == "1")
        //                 LGZGQClass.log.WriteMyLog("回传报告：" + rtnxml);
        //            msgtxt = "";
        //            try
        //            {
        //                if (MQSer.SendMessage(rtnxml, ref msgtxt, "QI1_001", "PIS_Report"))
        //                {
        //                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告发送成功：" + msgtxt);
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告发送成功',F_FSZT='已处理' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='已审核'");

        //                }
        //                else
        //                {
        //                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告发送失败：" + msgtxt);
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告发送失败：" + msgtxt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='已审核'");

        //                } return;
        //            }
        //            catch (Exception ee4)
        //            {
        //                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告发送异常：" + ee4.Message);
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告发送异常：" + ee4.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='已审核'");

        //            }
        //            return;

        //            #endregion

        //        }
        //        else
        //        {
        //            if (bgzt == "取消审核")
        //            {
        //                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
        //                if (dt2.Rows.Count > 0)
        //                {
        //                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
        //                    ZgqClass zgq = new ZgqClass();
        //                    string rtn_msg = "";
        //                    //  zgq.FTP_Delete("", dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
        //                }

        //                if (jcxx.Rows[0]["F_brlb"].ToString().Trim() != "体检")
        //                {

        //                    #region  平台报告回收

        //                    string xml = "";
        //                    try
        //                    {
        //                        xml = xml + "<?xml version=\"1.0\"?>";
        //                        xml = xml + "<PRPA_IN000003UV04> ";
        //                        xml = xml + "<!-- UUID,交互实例唯一码-->";
        //                        xml = xml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
        //                        xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
        //                        xml = xml + "<interactionId extension=\"PRPA_IN000003UV04\" root=\"2.16.840.1.113883.1.6\"/>";
        //                        xml = xml + "<receiver typeCode=\"RCV\">";
        //                        xml = xml + "<!-- 可以填写电话信息或者URL-->";
        //                        xml = xml + "<telecom value=\"\"/>";
        //                        xml = xml + "<device determinerCode=\"INSTANCE\" classCode=\"DEV\">";
        //                        xml = xml + "<id extension=\"HIP\" root=\"2.999.1.97\"/>";
        //                        xml = xml + "<telecom value=\"设备编号\"/>";
        //                        xml = xml + "<softwareName codeSystemName=\"医院信息平台系统域代码表\" codeSystem=\"2.999.2.3.2.84\" displayName=\"医院信息平台\" code=\"HIP\"/>";
        //                        xml = xml + "</device></receiver>";

        //                        xml = xml + "<sender typeCode=\"SND\"><telecom value=\"\"/>";
        //                        xml = xml + "<device determinerCode=\"INSTANCE\" classCode=\"DEV\"><id extension=\"PIS\" root=\"2.999.1.98\"/>";
        //                        xml = xml + "<telecom value=\"设备编号\"/>";
        //                        xml = xml + "<softwareName codeSystemName=\"医院信息平台系统域代码表\" codeSystem=\"2.999.2.3.2.84\" displayName=\"病理信息系统\" code=\"PIS\"/></device></sender>";

        //                        xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
        //                        xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
        //                        xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

        //                        xml = xml + "<!--报告流水号-->";
        //                        xml = xml + "<flowID>" + blh + "_" + bglx + "_" + bgxh + "</flowID>";
        //                        xml = xml + "<!--报告类型 - 超声、放射、心电等-->";
        //                        xml = xml + "<adviceType>病理</adviceType>";
        //                        xml = xml + "<!--HIS病人类型 门诊 住院（必填）-->";
        //                        xml = xml + "<patienttype>" + jcxx.Rows[0]["F_brlb"].ToString() + "</patienttype>";
        //                        xml = xml + "<!-- 文档注册时间 -->";
        //                        xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</sourceTime>";
        //                        xml = xml + "<!-- 来源系统编码（必填） -->";
        //                        xml = xml + "<sourceCode/>";
        //                        xml = xml + "<!-- 来源系统简称 -->";
        //                        xml = xml + "<sourceName>PIS</sourceName>";
        //                        xml = xml + "<!-- 文档编号（必填） -->";
        //                        xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
        //                    }
        //                    catch (Exception ee4)
        //                    {
        //                         LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告回收异常：" + ee4.Message);
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告回收异常：" + ee4.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //                        return;
        //                    }


        //                    if (xml.Trim() == "")
        //                    {
        //                         LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告回收生成xml为空");
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告回收生成xml为空' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");

        //                        return;
        //                    }

        //                    msgtxt = "";
        //                    try
        //                    {
        //                        if (MQSer.SendMessage(xml, ref msgtxt, "QI1_001", "PIS_ReportExpired"))
        //                        {
        //                            if (debug == "1")
        //                                 LGZGQClass.log.WriteMyLog(xml + "\r\nQI1_001--PIS_ReportExpired");
        //                             LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告回收成功：" + msgtxt);
        //                            aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告回收成功：" + msgtxt + "',F_FSZT='已处理' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //                            return;
        //                        }
        //                        else
        //                        {
        //                             LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告回收失败：" + msgtxt);
        //                            aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告回收失败：" + msgtxt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //                            return;
        //                        }
        //                    }
        //                    catch (Exception ee4)
        //                    {
        //                         LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",报告回收异常：" + ee4.Message);
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告回收异常：" + ee4.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //                        return;
        //                    }
        //                    #endregion

        //                }
        //                else
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_BZ='' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //            }
        //            else
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='不处理,状态" + bgzt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
        //        }

        //    }
        //}

      
        public void C_pdf(string blh, string bgxh, string bglx, DataTable dt_jcxx, bool isToBase64String, ref string PdfToBase64String,string debug)
        {

                string blbh = blh + bglx + bgxh;

                #region  生成pdf
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                {
                    string ReprotFile = ""; string jpgname = "";
                    string ML = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    string message = "";

                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                  // bool isrtn = zgq.CreatePDFJPG(blh, bglx, bgxh, ref jpgname, "", ZGQ_PDFJPG.type.PDF, ref message);

                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref message, ref jpgname);
                    if (isrtn)
                    {
                        if (debug == "1")
                            log.WriteMyLog("生成PDF成功");

                        ////二进制串
                        if (File.Exists(jpgname))
                        {
                            if (isToBase64String)
                            {
                                try
                                {
                                    FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
                                    Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                                    file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
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
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message,3, ref pdfpath);
                          
                            if (ssa == true)
                            {
                                if (debug == "1")
                                     log.WriteMyLog("上传PDF成功");

                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "上传PDF成功", "审核", "上传PDF成功:" + pdfpath, "ZGQJK", "上传PDF");

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='生成PDF成功',F_ISJPG='true'  where F_blbh='" + blbh  + "'");
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog("上传PDF失败" + message);
                                ZgqClass.BGHJ(blh, "上传PDF失败", "审核", message, "ZGQJK", "上传PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_bz='上传PDF失败：" + message + "'  where F_blbh='" + blbh + "'");
                            }
                        }
                        else
                        {
                            if (debug == "1")
                                log.WriteMyLog("未找到文件" + jpgname);
                            ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "未找到文件" + jpgname, "ZGQJK", "生成PDF");
                        }


                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("生成PDF失败" + message);
                        ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "生成pdf失败" + message, "ZGQJK", "生成PDF");
                        aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_BZ='" + message + "'  where F_BLBH='" + blbh+ "'");

                    }
                    zgq.DelTempFile(blh);

                }
                #endregion
            
        }

        public void TJ_ZtToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
            if (bglx != "cg")
                return;
            if (dt_sqd.Rows.Count <= 0)
            {
               log.WriteMyLog(blh + ",申请表中无记录");
                return;
            }
            string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
            string GUID = "";
            string bgztxml = TJ_ZtMsg(dt_jcxx, dt_sqd, dt_jcxx.Rows[0]["F_SQXH"].ToString(), blh, bglx, bgxh, bgzt, yhmc, yhbh, ref  GUID);
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
        public void TJ_BgToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh,string bglx,string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
         
            try
            {
                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;
                if (bglx != "cg")
                    return;
                string url = f.ReadString("savetohis", "blweb", "http://172.16.95.230/pathwebrpt");
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
                string rtnxml = TJ_BgMsg(dt_jcxx, dt_tx, dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim(), blh, bglx, bgxh, bgzt, jzlb, yhmc, yhbh, filePath, url, ref GUID);
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
                       log.WriteMyLog("回传报告[QI1_001-PIS_Report]:" + rtnxml);

                    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                    MQSer.Url = wsurl;
                    if (MQSer.SendMessageToMQ(rtnxml, ref msgtxt, "QI1_081", "PIS_Report_PE", GUID, "报告发布"))//PIS_Report
                    {
                        if (debug == "1")
                           log.WriteMyLog(blbh + ",发送成功:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送成功',F_FSZT='已处理' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    }
                    else
                    {
                       log.WriteMyLog(blbh + ",发送失败:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送失败：" + msgtxt + "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                    }
                }
                catch (Exception ee4)
                {
                   log.WriteMyLog(blbh + ",发送异常：" + ee4.Message);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='发送异常：" + ee4.Message + "' where F_blbh='" + blbh + "' and F_BGZT='已审核'");

                }
            }
            catch (Exception ee5)
            {
               log.WriteMyLog(ee5.Message);
            }
            return;
        }
        public string TJ_BgMsg(DataTable dt_jcxx, DataTable dt_tx, string sqxh, string blh, string bglx, string bgxh, string bgzt, string jzlb, string yhmc, string yhbh, string filePath, string url, ref string GUID)
        {
            filePath = filePath.ToLower().Replace("ftp", "http");
            string blbh = blh + bglx + bgxh;
            if(bglx=="cg")
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
                rtnxml = rtnxml + "<softwareName code=\"HIP\" displayName=\"医院信息平台\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</receiver>";

                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                rtnxml = rtnxml + "<telecom value=\"设备编号\"/>";
                rtnxml = rtnxml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
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
                rtnxml = rtnxml + "<adviceType>病理</adviceType>";
                ////<!--HIS病人类型 门诊 住院-->
                rtnxml = rtnxml + "<patienttype>" + dt_jcxx.Rows[0]["F_BRLB"].ToString() + "</patienttype>";
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
                    rtnxml = rtnxml + "<procedureCode>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] + "</procedureCode>";
                    try
                    {
                        rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] + "</procedureName>";
                    }
                    catch
                    {
                        rtnxml = rtnxml + "<procedureName></procedureName>";
                    }
                }
                else
                {
                    rtnxml = rtnxml + "<procedureCode>" + "" + "</procedureCode>";
                    rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "</procedureName>";
                }
                ////	<!--检查部位-->
                rtnxml = rtnxml + "<positionName>" + dt_jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] + "</positionName>";
                ////	<!--检查设备名称-->
                rtnxml = rtnxml + "<modalityName></modalityName>";
                ////	<!--报告类别，如CT，MRI等-->

                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
                ////	<!-- 报告医生编码 -->
                rtnxml = rtnxml + "<authorCode></authorCode>";
                ////	<!--报告医生-->
               
                    rtnxml = rtnxml + "<authorName>" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
                    ////	<!--报告时间-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--审核医生-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
                    ////	<!--审核时间-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";


                    DataTable TJ_bljc = new DataTable();
                    TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                    // 诊断描述
                    string Res_char = dt_jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                    //诊断结论	Res_con
                    string Res_con = dt_jcxx.Rows[0]["F_blzd"].ToString().Trim();
                    if (TJ_bljc.Rows.Count > 0)
                    {
                        if (dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "体检LCT" || dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "液基细胞")
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
                rtnxml = rtnxml + "<deptCode></deptCode>";
                ////	<!-- 医生所属科室 -->
                rtnxml = rtnxml + "<deptName>病理科</deptName>";


                ////	<!-- 来源系统编码 -->
                rtnxml = rtnxml + "<sourceCode>PIS</sourceCode>";
                ////	<!-- 来源系统简称 -->
                rtnxml = rtnxml + "<sourceName>PIS</sourceName>";
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
                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + dt_jcxx.Rows[0]["F_txml"].ToString() + "/" + dt_tx.Rows[j]["F_TXM"] + "</imageURL>";
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
                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + "" + "</text>";
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
        public string TJ_ZtMsg(DataTable dt_jcxx, DataTable dt_sqd, string sqxh, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, ref string GUID)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
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
                xml = xml + "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
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
                    xml = xml + "<code code=\"60\" displayName=\"报告发布\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else if (bgzt == "已写报告")
                    xml = xml + "<code code=\"50\" displayName=\"完成\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";
                else
                    xml = xml + "<code code=\"40\" displayName=\"执行\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"业务活动类型代码表\"/>";

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
                xml = xml + "<name>病理科</name>";
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
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_JZLB"].ToString() + "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"患者类型代码表\" displayName=\"" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                xml = xml + "<statusCode/>";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<!--平台注册的患者ID -->";
                xml = xml + "<id root=\"2.999.1.37\" extension=\"" + dt_sqd.Rows[0]["F_EMPIID"].ToString() + "\"/>";
                xml = xml + "<!--本地系统的患者ID -->";
                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_patientid"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_ZLH"].ToString() + "\"/>";
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
        public void TJ_BgHSToPt(DataTable dt_jcxx, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
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
                xml = xml + "<softwareName code=\"HIP\" displayName=\"集成平台总线系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"设备编号\"/>";
                xml = xml + "<softwareName code=\"PIS\" displayName=\"病理信息系统\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"医院信息平台系统域代码表\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

                xml = xml + "<!--报告流水号-->";
                xml = xml + "<flowID>" + blbh + "</flowID>";
                xml = xml + "<!--报告类型 - 超声、放射、心电等-->";
                xml = xml + "<adviceType>病理</adviceType>";
                xml = xml + "<!--HIS病人类型 门诊 住院（必填）-->";
                xml = xml + "<patienttype>" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "</patienttype>";
                xml = xml + "<!-- 文档注册时间 -->";
                xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                xml = xml + "<!-- 来源系统编码（必填） -->";
                xml = xml + "<sourceCode/>";
                xml = xml + "<!-- 来源系统简称 -->";
                xml = xml + "<sourceName>PIS</sourceName>";
                xml = xml + "<!-- 文档编号（必填） -->";
                xml = xml + "<indexInSystem>" + blbh + "</indexInSystem>";

                xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
            }
            catch (Exception ee4)
            {
               log.WriteMyLog(blbh + ",报告召回XML异常：" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='召回XML异常：" + ee4.Message + "' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }
            if (xml.Trim() == "")
            {
               log.WriteMyLog(blbh + ",报告召回生成xml为空");
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='报告召回xml为空' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }

            string msgtxt = "";
            try
            {
                if (debug == "1")
                   log.WriteMyLog("报告召回：[QI1_081--PIS_ReportExpired_PE]" + xml);

                string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();

                MQSer.Url = wsurl;


                if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_081", "PIS_ReportExpired_PE", GUID, "报告回收"))
                {
                    if (debug == "1")
                    {
                       log.WriteMyLog(blbh + ",报告召回成功：" + msgtxt);
                    }
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回成功:" + msgtxt + "',F_FSZT='已处理' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
                    return;
                }
                else
                {
                   log.WriteMyLog(blbh + ",报告召回失败：" + msgtxt);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回失败:" + msgtxt + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                    return;
                }
            }
            catch (Exception ee4)
            {
               log.WriteMyLog(blbh + ",报告召回异常：" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='报告召回异常:" + ee4.Message + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");
                return;
            }
        }
     
    }
}
