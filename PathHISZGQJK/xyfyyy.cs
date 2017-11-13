
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Data.OleDb;
using ZgqClassPub;


namespace PathHISZGQJK
{
    //湘雅附一医院
    class xyfyyy
    {
       
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        string constr = "Provider='MSDAORA';data source=DBSERVER;user id =DHC;password=DHC";

        public void pathtohis(string blh, string bglx, string bgxh,string czlx,string dz, string debug, string[] cslb)
        {
            string qxsh = "";
            bglx = bglx.ToLower();
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "1";

            if(bglx=="")
             bglx = "cg";
            if (bgxh == "")
             bgxh = "0";
            string pathweburl = f.ReadString("SF", "webservicesurl", "http://192.168.2.51:57772/csp/dhcens/DHC.Pis.XiangYaOne.BS.Web.cls").Replace("\0", "").Trim(); ;
            string constr = f.ReadString("SF", "odbcsql", "Provider='MSDAORA';data source=DBSERVER;user id =DHC;password=DHC").Replace("\0", "").Trim(); ;
            string sfsf = f.ReadString("SF", "sfsf", "").Replace("\0", "").Trim(); ;

            string msg = f.ReadString("savetohis", "msg", "1").Replace("\0", "").Trim(); ;
            string hczt = f.ReadString("savetohis", "hczt", "").Replace("\0", "").Trim(); ;
            string hcbg = f.ReadString("savetohis", "hcbg", "1").Replace("\0", "").Trim(); ;
            string hcbd = f.ReadString("savetohis", "hcbd", "0");

            xyfyWeb.DHCPisXiangYaOne xyfy = new xyfyWeb.DHCPisXiangYaOne();
            if (pathweburl.Trim() != "")
                xyfy.Url = pathweburl;

             dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            
            string yh = f.ReadString("yh", "yhmc", "").Replace("\0", "");
         
            string funName = "ExamStatus";

            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "'  and F_BD_BGZT='已审核'", "bdbg");
            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                log.WriteMyLog("病理号有错误！");
                return;
            }
            string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog(blh + ",无申请序号（单据号），不处理！");
                return;
            }

            string brlx = bljc.Rows[0]["F_brlb"].ToString().Trim();
            string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

            if (dz == "qxsh")
                bgzt = "取消审核";
            //一般情况 F_sqxh 为常规的申请号
            //如果冰冻+常规分开开申请单，只生成一个病理号，且合并病理号了，，默认 F_sqxh为冰冻的申请号，F_sqxh2为常规的申请号
            string bd_or_cg = "0";
            //合并后的 常规申请单号（第二份申请单）
            string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
            //冰冻上传标记
            //string bdscbj = bljc.Rows[0]["F_bdscbj"].ToString().Trim();
            //回传过的冰冻申请单，不再修改examapply中的标记

            if (hczt == "1" &&  bglx == "cg")
            {
                #region 状态回写
                try
                {
                    Execute_sql("update examapply set jszt='已执行' where jszt<>'已执行' and checkflow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'");
                }
                catch
                {
                }

                if (brlx == "住院")
                    funName = "ExamStatusIp";  //住院的方法名
                else
                    if (brlx == "门诊")
                        funName = "ExamStatus";   //门诊的方法名   ExamReport
                    else
                    {
                        log.WriteMyLog(blh + ",非住院非门诊病人不处理，不处理！");
                        return;
                    }
                /////////////////////////////////
             
                string Operator = "";
                string Status = "";  // 1:预约;2:登记;3,4:完成;5:取消登记;6:取消预约;7:取消申请单8.延期

                //回写常规申请单状态
                if (bgzt == "已登记")
                {
                    Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                    Status = "201";
                }
                else
                    if (bgzt == "已取材")
                    {

                        Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                        Status = "201";
                    }
                    else

                        if (bgzt == "报告延期")
                        {
                            Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                            Status = "80";
                        }
                        else
                            if (bgzt == "已审核")
                            {
                                Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                                Status = "901";
                            }
                            else
                                if (bgzt == "已写报告")
                                {
                                    Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                    Status = "90";
                                }
                                else
                                {
                                    Status = "";
                                }

                if (Status == "")
                {
                    log.WriteMyLog(blh + ",常规申请单号状态为空，不回传");
                    return;
                }

                //修改门诊收费状态，登记后就不能退费了
                #region  修改门诊收费状态
                if (sfsf == "1")
                {
                    if (brlx == "门诊" && bljc.Rows[0]["F_MZSFBZ"].ToString().Trim() != "1")
                    {
                        string EXAM_NO = "";
                        try
                        {
                            string exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "' ";
                            DataTable exam_appoints_id = select_orcl(exam_appoints_id_str, "获取EXAM_NO号");
                            EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();

                            if (EXAM_NO != "")
                            {
                                DataTable dt_sfm = select_orcl("select * from exam.exam_bill_items  where  status='A' and billing_attr='1' and performed_by='90' and not  RCPT_NO is null and  exam_no='" + EXAM_NO + "' ", "获取收费明细");
                                DataTable dta = select_orcl("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='检查' and ADVICE_ID like '" + EXAM_NO + "%'", "查询outp_bill_items");
                                // MessageBox.Show("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='检查' and ADVICE_ID like '" + EXAM_NO + "%'");
                                if (dta.Rows.Count > 0)
                                {
                                    for (int x = 0; x < dt_sfm.Rows.Count; x++)
                                    {
                                        string outp_bill_items_str = "update  outp_bill_items set status='1' where  status is null  and  Rcpt_no='" + dt_sfm.Rows[x]["Rcpt_no"].ToString().Trim() + "'and ITEM_class='" + dt_sfm.Rows[x]["ITEM_class"].ToString().Trim() + "'and PERFORMED_BY='90' and costs='" + dt_sfm.Rows[x]["costs"].ToString().Trim() + "'and ADVICE_ID='" + dt_sfm.Rows[x]["exam_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["exam_item_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["charge_item_no"].ToString().Trim() + "'";
                                        int y = insert_orcl(outp_bill_items_str, "退费信息-修改outp_bill_items");
                                        if (y > 0)
                                        {
                                            aa.ExecuteSQL("update T_JCXX set F_MZSFBZ='1'  where F_blh='" + blh + "'");
                                        }
                                    }
                                }
                            }


                        }
                        catch (Exception ees)
                        {
                            log.WriteMyLog(blh + ",修改门诊收费状态异常：" + ees.Message.ToString());
                        }
                    }
                }
                #endregion

                //回写冰冻申请单状态
                if (hcbd == "1")
                {
                    if (dt_bd.Rows.Count > 0)
                    {
                        Operator = dt_bd.Rows[0]["F_BD_BGYS"].ToString().Trim();
                        Status = "901";
                    }

                }



                string ExamStatus_XML = "<Request><ExamStatus>"
                + " <CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
                + "<Operator>" + Operator.Trim() + "</Operator>"
                + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                + "<Status>" + Status + "</Status>"
                + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

                string rtn_Status = "";

                if (debug == "1")
                 log.WriteMyLog("回写状态XML：" + ExamStatus_XML);
                //****************************************************
                //回写状态
                //****************************************************
                try
                {
                    rtn_Status = xyfy.DhcService(funName, ExamStatus_XML);
                }
                catch (Exception e)
                {
                    log.WriteMyLog(blh + ",回写状态异常：" + e.Message.ToString());
                    return;
                }
                //****************************************************
                //回写状态,返回值xml的解析
                //****************************************************
                try
                {
                    if (rtn_Status == "")
                    {
                        log.WriteMyLog(blh + "回写状态：返回值为空");
                        return;
                    }
                }
                catch(Exception  ee2)
                {
                    log.WriteMyLog(blh + "回写状态异常："+ee2.Message);
                }
                if (debug == "1")
                    log.WriteMyLog(rtn_Status);
                try
                {
                    XmlDataDocument xd = new XmlDataDocument();
                    xd.LoadXml(rtn_Status);
                    XmlNode xn = xd.SelectSingleNode("/Response");

                    if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        log.WriteMyLog(blh + "回写状态失败：" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                        return;
                    }
                    else
                    {
                        if (debug == "1")
                           log.WriteMyLog("回传状态成功");
                    }
                }
                catch (Exception rtne)
                {
                 
                    log.WriteMyLog(blh + ",回写状态失败，返回值XML解析异常：" + rtn_Status + "&" + rtne.ToString());
                    return;
                }

                #endregion
                //合并后的 常规申请单号（第二份申请单）string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                #region  常规申请单号（第二份申请单）

                if (sqxh2.Trim() != "")
                {
                    if (bgzt == "已登记")
                    {
                        Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                        Status = "201";
                    }
                    else
                        if (bgzt == "已取材")
                        {

                            Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                            Status = "201";
                        }
                        else
                            if (bgzt == "报告延期")
                            {
                                Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                Status = "80";
                            }
                            else
                                if (bgzt == "已审核")
                                {
                                    Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                                    Status = "901";
                                }
                                else
                                    if (bgzt == "已写报告")
                                    {
                                        Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                        Status = "90";
                                    }
                                    else
                                        if (Status == "")
                                        {
                                            log.WriteMyLog(blh + ",常规申请单号状态2为空，不回传");
                                            return;
                                        }
                    string ExamStatus_XML2 = "<Request><ExamStatus>"
               + " <CheckFlow>" + bljc.Rows[0]["F_sqxh2"].ToString().Trim() + "</CheckFlow>"
               + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
               + "<Operator>" + Operator.Trim() + "</Operator>"
               + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
               + "<Status>" + Status + "</Status>"
               + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

                    string rtn_Status2 = "";



                    if (debug == "1")
                     log.WriteMyLog("回写状态XML：" + ExamStatus_XML2);
                    //****************************************************
                    //回写状态
                    //****************************************************
                    try
                    {
                        rtn_Status2 = xyfy.DhcService(funName, ExamStatus_XML2);
                    }
                    catch (Exception e)
                    {
                        log.WriteMyLog(blh + "回写状态异常，" + e.Message.ToString());
                        return;
                    }
                    //****************************************************
                    //回写状态,返回值xml的解析
                    //****************************************************
                    try
                    {
                        if (rtn_Status2 == "")
                        {
                            log.WriteMyLog(blh + "回写状态失败：返回值为空");
                            return;
                        }
                    }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog(blh + "回写状态失败：返回值异常" + ee2.Message);
                        return;
                    }
                    try
                    {
                        XmlDataDocument xd = new XmlDataDocument();
                        xd.LoadXml(rtn_Status2);
                        XmlNode xn = xd.SelectSingleNode("/Response");

                        if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                        {
                            log.WriteMyLog(blh + ",回写状态失败：" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                            return;
                        }
                        else
                        {
                            if (debug == "1")
                              log.WriteMyLog("回传状态成功");
                        }
                    }
                    catch (Exception rtne)
                    {
                        log.WriteMyLog(blh + "回写状态异常，返回值XML解析异常：" + rtn_Status2 + "&" + rtne.ToString());
                        return;
                    }

                }
                #endregion
            }


            if (bgzt == "已审核")
            {
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                {
                    //生成jpg文件
                    string jpgname = "";
                    #region  生成pdf

                    string message = "";
                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                    if (debug == "1")
                        log.WriteMyLog("开始生成PDF。。。");
                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref message, ref jpgname);

                    string xy = "3";
                    if (isrtn)
                    {
                        if (File.Exists(jpgname))
                        {
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                            if (ssa == true)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("上传PDF成功");

                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "JK", "审核", "上传PDF成功:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "上传PDF");

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='true' where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");

                            }
                            else
                            {
                                log.WriteMyLog("上传PDF失败：" + message);
                                ZgqClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "上传PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                                //  return;
                            }
                        }
                        else
                        {
                            log.WriteMyLog("生成PDF失败:未找到文件---" + jpgname);
                            ZgqClass.BGHJ(blh, "JK", "审核", "上传PDF失败:未找到文件---" + jpgname, "ZGQJK", "生成PDF");
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败:未找到文件---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核' ");
                            //  return;
                        }
                    }
                    else
                    {

                        log.WriteMyLog("生成PDF失败：" + message);
                        ZgqClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "生成PDF");
                        aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='生成PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                        // return;
                    }
                    #endregion
                }
            }

            ////////////////////////////////////////////////////////
            /////回传冰冻报告//////////////////////////////////////
            /////////////////////////////////////////////////////////
            if (hcbg == "1")
            {
                #region   回传冰冻报告
                //if (hcbd == "1" && dt_bd.Rows.Count > 0)
                //{

                //    //回传报告
                //    string ExamReportXML = "";
                //    string blzd_bd = "冰冻结果：" + dt_bd.Rows[0]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                //    for (int i = 1; i < dt_bd.Rows.Count; i++)
                //    {
                //        blzd_bd = blzd_bd + "     第" + (i + 1).ToString() + "次冰冻结果：" + dt_bd.Rows[i]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                //    }
                //    if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
                //    {
                //        string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                //        if (sqxh == "")
                //            sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
                //        string blzd = "常规病理诊断：" + bljc.Rows[0]["F_blzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                //        blzd_bd = blzd + "    " + blzd_bd;
                //        ExamReportXML = "<Request><ExamReport>"
                //      + "<CheckFlow>" + sqxh + "</CheckFlow>"
                //        + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                //          + "<ExecLink></ExecLink>"
                //          + "<ExamName></ExamName>"
                //          + "<Data>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Data>"
                //          + "<Findings>" + "" + "</Findings>"
                //          + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                //          + "<Id></Id>"
                //          + "<ExamPara></ExamPara>"
                //          + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                //          + "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                //          + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                //           + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                //            + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                //             + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"
                //                 + "<ReqDocID></ReqDocID>"
                //                + "<ReqDateTime></ ReqDateTime>"
                //                + "<ScheduledDocID></ScheduledDocID>"
                //                + "<ScheduledDateTime></ScheduledDateTime>"
                //                + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                //                + "<RegDateTime>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegDateTime>"
                //                + "<TechnicianID></TechnicianID>"
                //                + "<ExamDateTime></ExamDateTime>"
                //                + "<ReporterID>" + getyhgh(bljc.Rows[0]["F_bgYS"].ToString().Trim()) + "</ReporterID>"
                //                + "<ReportDateTime>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd") + "</ReporterDateTime>"
                //                + "<VerifyDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDocID>"
                //                + "<VerifyDateTime>" + DateTime.Parse(bljc.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd") + "</VerifyDateTime>"
                //                + "<IssueDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</IssueDocID>"
                //                + "<IssueDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</IssueDateTime>"
                //                + "<Status>4</Status>"
                //                + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</OperatorDateTime>"
                //          + "<ExamItemsList><ExaminationItems>"
                //          + "<Code></Code><Name></Name><Value></Value>"
                //          + "</ExaminationItems></ExamItemsList></ExamReport></Request>";
                //    }
                //    else
                //    {
                //        ExamReportXML = "<Request><ExamReport>"
                //   + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                //     + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/冰冻.asp?blh=" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</CheckLink>"
                //       + "<ExecLink></ExecLink>"
                //       + "<ExamName></ExamName>"
                //       + "<Data>" + dt_bd.Rows[0]["F_BD_bgrq"].ToString().Trim() + "</Data>"
                //       + "<Findings>" + "" + "</Findings>"
                //       + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                //       + "<Id></Id>"
                //       + "<ExamPara></ExamPara>"
                //       + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                //       + "<Reporter>" + dt_bd.Rows[0]["F_BD_bgys"].ToString().Trim() + "</Reporter>"
                //       + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                //        + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                //         + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                //          + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"

                //                                      + "<ReqDocID></ReqDocID>"
                //                + "<ReqDateTime></ ReqDateTime>"
                //                + "<ScheduledDocID></ScheduledDocID>"
                //                + "<ScheduledDateTime></ScheduledDateTime>"
                //                + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                //                + "<RegDateTime>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegDateTime>"
                //                + "<TechnicianID></TechnicianID>"
                //                + "<ExamDateTime></ExamDateTime>"
                //                + "<ReporterID>" + "" + "</ReporterID>"
                //                + "<ReportDateTime>" + "" + "</ReporterDateTime>"
                //                + "<VerifyDocID>" + "" + "</VerifyDocID>"
                //                + "<VerifyDateTime>" + "" + "</VerifyDateTime>"
                //                + "<IssueDocID>" + "" + "</IssueDocID>"
                //                + "<IssueDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</IssueDateTime>"
                //                + "<Status>4</Status>"
                //                + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</OperatorDateTime>"

                //       + "<ExamItemsList><ExaminationItems>"
                //       + "<Code></Code><Name></Name><Value></Value>"
                //       + "</ExaminationItems></ExamItemsList></ExamReport></Request>";

                //    }
                  
                //    if (ExamReportXML.Trim() == "")
                //    {
                //         LGZGQClass.log.WriteMyLog(blh + "生成xml错误，不能回传");
                //        return;
                //    }
                //    if (debug == "1")
                //        LGZGQClass.log.WriteMyLog("回写报告XML：" + ExamReportXML);

                //    string rtnExamReport = "";
                //    try
                //    {
                //        rtnExamReport = xyfy.DhcService("ExamReport", ExamReportXML);
                //    }
                //    catch (Exception eReport)
                //    {
                //         LGZGQClass.log.WriteMyLog(blh + "回写报告异常：" + eReport.Message);
                //        return;
                //    }
                //    if (debug == "1")
                //        LGZGQClass.log.WriteMyLog("回写报告返回值" + rtnExamReport);

                //    if (rtnExamReport == "")
                //    {
                //         LGZGQClass.log.WriteMyLog(blh + ",回写诊断：返回值为空");
                //        return;
                //    }
                //    try
                //    {
                //        XmlDataDocument xd2 = new XmlDataDocument();
                //        xd2.LoadXml(rtnExamReport);
                //        XmlNode xn2 = xd2.SelectSingleNode("/Response");
                //        if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                //        {
                //             LGZGQClass.log.WriteMyLog(ExamReportXML);
                //             LGZGQClass.log.WriteMyLog(blh + "回传报告失败，原因：" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                //        }
                //        else
                //        {
                //            if (debug == "1")
                //                 LGZGQClass.log.WriteMyLog("回传报告成功 " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                //            if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
                //                aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");
                //        }
                //        if (bgzt == "已写报告" && bljc.Rows[0]["F_scbj"].ToString().Trim() == "1")
                //        {
                //            string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                //            if (sqxh == "")
                //                sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
                //            //////////////////////////////////////////
                //            //回传报告
                //            string ExamReportXML3 = "<Request><ExamReportNew>"
                //            + "<CheckFlow>" + sqxh + "</CheckFlow>"
                //              + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                //                + "<ExecLink></ExecLink>"
                //                + "<ExamName></ExamName>"
                //                + "<Data>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Data>"
                //                + "<Findings>" + "" + "</Findings>"
                //                + "<Result>" + "  " + "</Result>"
                //                + "<Id></Id>"
                //                + "<ExamPara></ExamPara>"
                //                + "<IsAbnormal>" + " " + "</IsAbnormal>"
                //                + "<Reporter>" + " " + "</Reporter>"
                //                + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                //                + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                //                + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"

                //                + "<ReqDocID></ReqDocID>"
                //                + "<ReqDateTime></ ReqDateTime>"
                //                + "<ScheduledDocID></ScheduledDocID>"
                //                + "<ScheduledDateTime></ScheduledDateTime>"
                //                + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                //                + "<RegDateTime>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegDateTime>"
                //                + "<TechnicianID></TechnicianID>"
                //                + "<ExamDateTime></ExamDateTime>"
                //                + "<ReporterID>" + "" + "</ReporterID>"
                //                + "<ReportDateTime>" + "" + "</ReporterDateTime>"
                //                + "<VerifyDocID>" + "" + "</VerifyDocID>"
                //                + "<VerifyDateTime>" + "" + "</VerifyDateTime>"
                //                + "<IssueDocID>" + "" + "</IssueDocID>"
                //                + "<IssueDateTime>" + "" + "</IssueDateTime>"
                //                + "<Status>4</Status>"
                //                + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</OperatorDateTime>"
                //                + "<ExamItemsList><ExaminationItems>"
                //                + "<Code></Code><Name></Name><Value></Value>"
                //                + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                //            if (debug == "1")
                //                MessageBox.Show("回写报告XML：" + ExamReportXML3);


                //            //****************************************************
                //            //回写报告
                //            //****************************************************
                //            string rtnExamReport3 = "";

                //            try
                //            {
                //                rtnExamReport3 = xyfy.DhcService("ExamReport", ExamReportXML3);
                //            }
                //            catch (Exception eReport)
                //            {
                //                if (msg == "1")
                //                    MessageBox.Show("回写报告异常，原因：" + eReport.ToString());
                //                 LGZGQClass.log.WriteMyLog(ExamReportXML3);
                //                 LGZGQClass.log.WriteMyLog("回写报告异常，原因：" + eReport.ToString());
                //                return;
                //            }
                //            //****************************************************
                //            //回写状态返回的xml，解析
                //            //****************************************************

                //            if (debug == "1")
                //            {
                //                MessageBox.Show("回写报告返回值" + rtnExamReport3.ToString());
                //            }
                //            if (rtnExamReport3 == "")
                //            {
                //                 LGZGQClass.log.WriteMyLog(blh + ",回写诊断：返回值为空");
                //                //if (msg == "1")
                //                //    MessageBox.Show("回写诊断：返回值为空");

                //                return;
                //            }

                //            //------------------------------
                //            try
                //            {
                //                XmlDataDocument xd3 = new XmlDataDocument();
                //                xd3.LoadXml(rtnExamReport3);
                //                XmlNode xn3 = xd3.SelectSingleNode("/Response");
                //                if (xn3.FirstChild["Returncode"].InnerText.ToString() == "-1")
                //                {
                //                     LGZGQClass.log.WriteMyLog(ExamReportXML3);
                //                    //if (msg == "1")
                //                    //    MessageBox.Show("回传报告失败，原因：" + xn3.FirstChild["ResultContent"].InnerText.ToString());
                //                     LGZGQClass.log.WriteMyLog(blh + "回传报告，" + xn3.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn3.FirstChild["Returncode"].InnerText.ToString());
                //                }
                //                else
                //                {
                //                    if (debug == "1")
                //                        MessageBox.Show("回传报告成功 " + xn3.FirstChild["ResultContent"].InnerText.ToString());
                //                    aa.GetDataTable("update T_jcxx  set F_scbj='0'  where F_blh='" + blh + "'", "blx2");

                //                }


                //            }
                //            catch (Exception rtnee)
                //            {
                //                if (msg == "1")
                //                    MessageBox.Show("回传报告,解析XML错误,抛出异常，" + rtnee.ToString());
                //                 LGZGQClass.log.WriteMyLog(ExamReportXML3);
                //                 LGZGQClass.log.WriteMyLog("回传报告,解析XML错误,抛出异常：" + rtnee.ToString());
                //                return;

                //            }

                //        }
                //    }
                //    catch (Exception rtnee)
                //    {
                //        if (msg == "1")
                //        {
                //            MessageBox.Show("回传报告,解析XML错误,抛出异常，" + rtnee.ToString());
                //        }

                //         LGZGQClass.log.WriteMyLog("回传报告,解析XML错误,抛出异常：" + rtnee.ToString());
                //        return;

                //    }


                //    return;
                //}
                #endregion
                //****************************************************
                //回写常规报告，拼xml
                //****************************************************region

                if (bgzt == "取消审核")
                {
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                    if (bglx == "cg")
                    {
                        #region 取消审核
                        string ExamReportXML = "<Request><ExamReportNew>"
                            + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                            + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                            + "<ExecLink></ExecLink>"
                            + "<ExamName></ExamName>"
                            + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                            + "<Findings>" + "" + "</Findings>"
                            + "<Result>" + "  " + "</Result>"
                            + "<Id></Id>"
                            + "<ExamPara></ExamPara>"
                            + "<IsAbnormal>" + " " + "</IsAbnormal>"
                            //+ "<Reporter>" + " " + "</Reporter>"
                            //+ "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                            //+ "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                            //+ "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                            //+ "<REQDATETIME>" + DateTime.Parse(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</REQDATETIME>"
                         
                            + "<ReqDocID></ReqDocID>"
                            + "<ReqDateTime></ReqDateTime>"
                            + "<ScheduledDocID></ScheduledDocID>"
                            + "<ScheduledDateTime></ScheduledDateTime>"
                            + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                            + "<RegDateTime>" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</RegDateTime>"
                            + "<TechnicianID></TechnicianID>"
                            + "<ExamDateTime></ExamDateTime>"
                            + "<ReporterID>" + "" + "</ReporterID>"
                            + "<ReportDateTime>" + "" + "</ReportDateTime>"
                            + "<VerifyDocID>" + "" + "</VerifyDocID>"
                            + "<VerifyDateTime>" + "" + "</VerifyDateTime>"
                            + "<IssueDocID>" + "" + "</IssueDocID>"
                            + "<IssueDateTime>" + "" + "</IssueDateTime>"
                            + "<Status>4</Status>"
                            + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDateTime>"
                            + "<ExamItemsList><ExaminationItems>"
                            + "<Code></Code><Name></Name><Value></Value>"
                            + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                        if (debug == "1")
                            log.WriteMyLog("取消审核XML：" + ExamReportXML);

                        string rtnExamReport = "";
                        try
                        {
                            rtnExamReport = xyfy.DhcService("ExamReportNew", ExamReportXML);
                        }
                        catch (Exception eReport)
                        {
                            log.WriteMyLog("取消审核：" + eReport.Message);
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "取消审核失败：" + eReport.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                            return;
                        }
                        if (debug == "1")
                            log.WriteMyLog("取消审核返回：" + rtnExamReport.ToString());

                        try
                        {
                            if (rtnExamReport == "")
                            {
                                log.WriteMyLog(blh + ",取消审核失败：返回值为空");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "取消审核失败：返回值为空" + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                                return;
                            }
                        }
                        catch
                        {
                            log.WriteMyLog(blh + ",取消审核失败：返回值为异常");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "取消审核失败：返回值为异常" + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                            return;
                        }

                        try
                        {
                            XmlDataDocument xd2 = new XmlDataDocument();
                            xd2.LoadXml(rtnExamReport);
                            XmlNode xn2 = xd2.SelectSingleNode("/Response");
                            if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                            {

                                log.WriteMyLog("取消审核失败：" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "取消审核失败：" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString() + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");

                                return;
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog("取消审核成功： " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                                aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='取消审核成功',F_fszt='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                                return;
                            }
                        }
                        catch (Exception rtnee)
                        {
                            log.WriteMyLog(blh + "取消审核异常：" + rtnee.Message);
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + rtnee.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                            return;
                        }
                        #endregion
                    }
                    return;
                }

                if (bgzt == "已审核" && bglx=="cg")
                {
                    #region 回写常规报告
                    //回传报告
                    string ExamReportXML = "<Request><ExamReportNew>"
                    + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                    + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                    + "<ExecLink></ExecLink>"
                    + "<ExamName></ExamName>"
                    + "<Date>" +DateTime.Parse( bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</Date>"
                    + "<Findings>" + "" + "</Findings>"
                    + "<Result><![CDATA[" + @bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "]]></Result>"
                    + "<Id></Id>"
                    + "<ExamPara></ExamPara>"
                    + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                    //+ "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                    //+ "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                    //+ "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                    //+ "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                    //+ "<REQDATETIME>" +DateTime.Parse( bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</REQDATETIME>"
                  
                    + "<ReqDocID></ReqDocID>"
                    + "<ReqDateTime></ReqDateTime>"
                    + "<ScheduledDocID></ScheduledDocID>"
                    + "<ScheduledDateTime></ScheduledDateTime>"

                    + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                    + "<RegDateTime>" + DateTime.Parse(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</RegDateTime>"
                    + "<TechnicianID></TechnicianID>"
                    + "<ExamDateTime></ExamDateTime>"
                    + "<ReporterID>" + getyhgh(bljc.Rows[0]["F_BGYS"].ToString().Trim()) + "</ReporterID>"
                    + "<ReportDateTime>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</ReportDateTime>"
                    + "<VerifyDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDocID>"
                    + "<VerifyDateTime>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</VerifyDateTime>"
                    + "<IssueDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</IssueDocID>"
                    + "<IssueDateTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</IssueDateTime>"
                    + "<Status>4</Status>"
                    + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDateTime>"
                    + "<ExamItemsList><ExaminationItems>"
                    + "<Code></Code><Name></Name><Value></Value>"
                    + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                    if (debug == "1")
                        log.WriteMyLog("回写报告XML：" + ExamReportXML);

                    //****************************************************
                    //回写报告
                    //****************************************************
                    string rtnExamReport = "";
                    try
                    {
                        rtnExamReport = xyfy.DhcService("ExamReportNew", ExamReportXML);
                    }
                    catch (Exception eReport)
                    {
                        log.WriteMyLog(blh + "回写报告异常：" + eReport.Message);
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='回传报告失败：" + eReport.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                        return;
                    }
                    if (debug == "1")
                        log.WriteMyLog("回写报告返回值：" + rtnExamReport);
                    //****************************************************
                    //回写状态返回的xml，解析
                    //****************************************************
                    try
                    {
                        if (rtnExamReport == "")
                        {
                            log.WriteMyLog(blh + ",回传报告：返回值为空");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='回传报告失败：返回值为空'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                            return;
                        }

                        XmlDataDocument xd2 = new XmlDataDocument();
                        xd2.LoadXml(rtnExamReport);
                        XmlNode xn2 = xd2.SelectSingleNode("/Response");
                        if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                        {
                            log.WriteMyLog("回传报告失败：" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='回传报告失败：" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString() + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                            return;
                        }
                        else
                        {
                            if (debug == "1")
                                log.WriteMyLog("回传报告成功：" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                            aa.ExecuteSQL("update T_jcxx  set F_scbj='2'  where F_blh='" + blh + "'");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='回传报告成功',F_FSZT='已处理'   where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                        }
                    }
                    catch (Exception rtnee)
                    {
                        log.WriteMyLog(blh + "回传报告失败：解析XML异常--" + rtnee.Message);
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='回传报告失败：解析XML异常--" + rtnee.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                        return;
                    }
                    #endregion
                }
                return; 
            }
            return;
        }
        public void pathtohis20161026(string blh, string yymc)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
          string  msg = f.ReadString("savetohis", "msg", "1");

            xyfyWeb.DHCPisXiangYaOne xyfy = new xyfyWeb.DHCPisXiangYaOne();
            xyfy.Url = "http://192.168.2.64/csp/dhcens/DHC.Ris.XiangYaOne.BS.Web.cls";
            string pathweburl = f.ReadString("SF", "webservicesurl", "");

            string odbcsql = f.ReadString("SF", "odbcsql", "");
            if (odbcsql.Trim() != "")
                constr = odbcsql;

            if (pathweburl.Trim() != "")
                xyfy.Url = pathweburl;

            /////////////////////////////////////////////////////////////////////////
            //是否上传收费标记
            string sfsf = f.ReadString("SF", "sfsf", "");

            string debug = f.ReadString("savetohis", "debug", "");
            string yh = f.ReadString("yh", "yhmc", "").Replace("\0", "");

            //是否上传冰冻报告
            string bdhx = f.ReadString("savetohis", "bdhx", "0");
            string funName = "ExamStatus";


            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "'  and F_BD_BGZT='已审核'", "bdbg");
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

            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog(blh + ",无申请序号（单据号），不处理！");
                return;
            }

            string brlx = bljc.Rows[0]["F_brlb"].ToString().Trim();



            //一般情况 F_sqxh 为常规的申请号
            //如果冰冻+常规分开开申请单，只生成一个病理号，且合并病理号了，，默认 F_sqxh为冰冻的申请号，F_sqxh2为常规的申请号

            string bd_or_cg = "0";
            //合并后的 常规申请单号（第二份申请单）
            string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
            //冰冻上传标记
            //string bdscbj = bljc.Rows[0]["F_bdscbj"].ToString().Trim();
            //回传过的冰冻申请单，不再修改examapply中的标记

            try
            {
                Execute_sql("update examapply set jszt='已执行' where jszt<>'已执行' and checkflow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'");
            }
            catch
            {

            }

            if (brlx == "住院")
                funName = "ExamStatusIp";  //住院的方法名
            else
                if (brlx == "门诊")
                    funName = "ExamStatus";   //门诊的方法名   ExamReport
                else
                {
                    log.WriteMyLog(blh + ",非住院非门诊病人不处理，不处理！");
                      return;
                }
            
            /////////////////////////////////
            string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
            string Operator = "";
            string Status = "";  // 1:预约;2:登记;3,4:完成;5:取消登记;6:取消预约;7:取消申请单8.延期

            //回写常规申请单状态
            if (bgzt == "已登记")
            {
                Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                Status = "201";
            }
            else
                if (bgzt == "已取材")
                {

                    Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                    Status = "201";
                }
                else

                    if (bgzt == "报告延期")
                    {
                        Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                        Status = "80";
                    }
                    else
                        if (bgzt == "已审核")
                        {
                            Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                            Status = "901";
                        }
                        else
                            if (bgzt == "已写报告")
                            {
                                Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                Status = "90";
                            }
                            else
                            {
                                Status = "";
                            }

            if (Status == "")
            {
                log.WriteMyLog(blh + ",常规申请单号状态为空，不回传");
                return;
            }

            //修改门诊收费状态，登记后就不能退费了

            #region  修改门诊收费状态
            if (sfsf == "1")
            {
                if (brlx == "门诊" && bljc.Rows[0]["F_MZSFBZ"].ToString().Trim()!="1")
                {
                    string EXAM_NO = "";
                    try
                    {
                        string exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "' ";
                        DataTable exam_appoints_id = select_orcl(exam_appoints_id_str, "获取EXAM_NO号");
                        EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();

                        if (EXAM_NO != "")
                        {

                            DataTable dt_sfm = select_orcl("select * from exam.exam_bill_items  where  status='A' and billing_attr='1' and performed_by='90' and not  RCPT_NO is null and  exam_no='" + EXAM_NO + "' ", "获取收费明细");
                            DataTable dta = select_orcl("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='检查' and ADVICE_ID like '" + EXAM_NO + "%'", "查询outp_bill_items");
                            // MessageBox.Show("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='检查' and ADVICE_ID like '" + EXAM_NO + "%'");
                            if (dta.Rows.Count > 0)
                            {
                                for (int x = 0; x < dt_sfm.Rows.Count; x++)
                                {
                                    string outp_bill_items_str = "update  outp_bill_items set status='1' where  status is null  and  Rcpt_no='" + dt_sfm.Rows[x]["Rcpt_no"].ToString().Trim() + "'and ITEM_class='" + dt_sfm.Rows[x]["ITEM_class"].ToString().Trim() + "'and PERFORMED_BY='90' and costs='" + dt_sfm.Rows[x]["costs"].ToString().Trim() + "'and ADVICE_ID='" + dt_sfm.Rows[x]["exam_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["exam_item_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["charge_item_no"].ToString().Trim() + "'";
                                    int y = insert_orcl(outp_bill_items_str, "退费信息-修改outp_bill_items");
                                    if (y > 0)
                                    {
                                      aa.ExecuteSQL("update T_JCXX set F_MZSFBZ='1'  where F_blh='" + blh + "'");
                                    }
                                }
                            }
                        }


                    }
                    catch (Exception ees)
                    {
                        if (msg == "1")
                            MessageBox.Show("修改门诊收费状态异常：" + ees.ToString());
                        log.WriteMyLog(blh + ",修改门诊收费状态异常：" + ees.ToString());
                    }


                }
            }
            #endregion 

            //回写冰冻申请单状态
            if (bdhx == "1")
            {
                if (dt_bd.Rows.Count > 0)
                {
                    Operator = dt_bd.Rows[0]["F_BD_BGYS"].ToString().Trim();
                    Status = "901";
                }

            }


            string ExamStatus_XML = "<Request><ExamStatus>"
            + " <CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
            + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
            + "<Operator>" + Operator.Trim() + "</Operator>"
            + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
            + "<Status>" + Status + "</Status>"
            + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

            string rtn_Status = "";
            if (debug == "1")
                MessageBox.Show("回写状态XML：" + ExamStatus_XML);
            //****************************************************
            //回写状态
            //****************************************************
            try
            {
                rtn_Status = xyfy.DhcService(funName, ExamStatus_XML);
            }
            catch (Exception e)
            {
                if (msg == "1")
                    MessageBox.Show("回写状态异常，" + e.ToString());
                log.WriteMyLog(ExamStatus_XML);
                log.WriteMyLog(blh + ",回写状态异常，" + e.ToString());
                return;
            }
            //****************************************************
            //回写状态,返回值xml的解析
            //****************************************************

            if (rtn_Status == "")
            {
                if (msg == "1")
                    MessageBox.Show("回写状态错误：返回值为空");
                log.WriteMyLog(ExamStatus_XML);
                log.WriteMyLog(blh + "回写状态：返回值为空");

                return;
            }
            try
            {
                XmlDataDocument xd = new XmlDataDocument();
                xd.LoadXml(rtn_Status);
                XmlNode xn = xd.SelectSingleNode("/Response");

                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                {
                    if (msg == "1")
                        MessageBox.Show("回写状态失败：" + xn.FirstChild["ResultContent"].InnerText.ToString());
                    log.WriteMyLog(ExamStatus_XML);
                    log.WriteMyLog(blh + "回写状态失败：" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                    return;
                }
                else
                {
                    if (debug == "1")
                        MessageBox.Show("回传状态成功");
                }
            }
            catch (Exception rtne)
            {
                if (msg == "1")
                    MessageBox.Show("回写状态失败，返回XML解析异常：" + rtne.ToString());
                log.WriteMyLog(ExamStatus_XML);
                log.WriteMyLog(blh + ",回写状态失败，返回值XML解析异常：" + rtn_Status + "&" + rtne.ToString());
                return;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //回传合并后的 常规申请单状态 F_sqxh2
            //合并后的 常规申请单号（第二份申请单）
            //string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();


            if (sqxh2.Trim() != "")
            {
                if (bgzt == "已登记")
                {
                    Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                    Status = "201";
                }
                else
                    if (bgzt == "已取材")
                    {

                        Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                        Status = "201";
                    }
                    else
                        if (bgzt == "报告延期")
                        {
                            Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                            Status = "80";
                        }
                        else
                            if (bgzt == "已审核")
                            {
                                Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                                Status = "901";
                            }
                            else
                                if (bgzt == "已写报告")
                                {
                                    Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                    Status = "90";
                                }
                                else
                                    if (Status == "")
                                    {
                                        log.WriteMyLog(blh + ",常规申请单号状态2为空，不回传");
                                        return;
                                    }
                string ExamStatus_XML2 = "<Request><ExamStatus>"
           + " <CheckFlow>" + bljc.Rows[0]["F_sqxh2"].ToString().Trim() + "</CheckFlow>"
           + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
           + "<Operator>" + Operator.Trim() + "</Operator>"
           + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
           + "<Status>" + Status + "</Status>"
           + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

                string rtn_Status2 = "";



                if (debug == "1")
                    MessageBox.Show("回写状态XML：" + ExamStatus_XML2);
                //****************************************************
                //回写状态
                //****************************************************
                try
                {
                    rtn_Status2 = xyfy.DhcService(funName, ExamStatus_XML2);
                }
                catch (Exception e)
                {
                    if (msg == "1")
                        MessageBox.Show("回写状态异常," + e.ToString());
                    log.WriteMyLog(ExamStatus_XML2);
                    log.WriteMyLog(blh + "回写状态异常，" + e.ToString());
                    return;
                }
                //****************************************************
                //回写状态,返回值xml的解析
                //****************************************************

                if (rtn_Status2 == "")
                {
                    if (msg == "1")
                        MessageBox.Show("回写状态失败：返回值为空");

                    log.WriteMyLog(blh + "回写状态失败：返回值为空");

                    return;
                }
                try
                {
                    XmlDataDocument xd = new XmlDataDocument();
                    xd.LoadXml(rtn_Status2);
                    XmlNode xn = xd.SelectSingleNode("/Response");

                    if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (msg == "1")
                            MessageBox.Show("回写状态失败：" + xn.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog(ExamStatus_XML2);
                        log.WriteMyLog(blh + ",回写状态失败：" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                        return;
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("回传状态成功");
                    }
                }
                catch (Exception rtne)
                {
                    if (msg == "1")
                        MessageBox.Show("回写状态异常，返回XML解析异常：" + rtne.ToString());
                    log.WriteMyLog(ExamStatus_XML2);
                    log.WriteMyLog(blh + "回写状态异常，返回值XML解析异常：" + rtn_Status2 + "&" + rtne.ToString());
                    return;
                }

            }
            ////////////////////////////////////////////////////////
            /////回传冰冻报告//////////////////////////////////////
            /////////////////////////////////////////////////////////

            #region   回传冰冻报告
            if (bdhx == "1" && dt_bd.Rows.Count > 0)
            {
                //生成jpg文件
                // 生成jpg格式报告文件
                //----------------jpg至ftp---------------------

                string status = "";
                string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/blbgjpg").Replace("\0", "");
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                // string ml = DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Year.ToString();
                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                string jpgname = "";
                string ftpstatus = "";
                mdjpg mdj = new mdjpg();
                ////////////////////////////////////////////
                ////冰冻报告jgp
                for (int i = 0; i < dt_bd.Rows.Count; i++)
                {
                    string bgxh = dt_bd.Rows[i]["F_BD_BGXH"].ToString();
                    try
                    {
                        mdj.BMPTOJPG(blh, ref jpgname, "bd", bgxh);
                    }
                    catch (Exception ee)
                    {
                        if (msg == "1")
                            MessageBox.Show("冰冻报告生成JPG文件异常：" + ee.ToString());
                        log.WriteMyLog(blh + "冰冻报告生成JPG文件异常：" + ee.ToString());
                    }

                    //////////////////////////////////////////////////
                    try
                    {

                        //判断ftp上是否存在该jpg文件
                        if (fw.fileCheckExist(ftpURI, blh + "_bd_" + bgxh + ".jpg"))
                        {
                            //删除ftp上的jpg文件
                            fw.fileDelete(ftpURI, blh + "_bd_" + bgxh + ".jpg").ToString();
                        }
                        //上传新生成的jpg文件
                        fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_bd_" + bgxh + "_1.jpg", "", out status);

                        if (status == "Error")
                        {
                            if (msg == "1")
                                MessageBox.Show("冰冻报告jpg上传失败，请重新审核！");
                            log.WriteMyLog(blh + "冰冻报告jpg上传失败，请重新审核！");
                        }
                    }
                    catch (Exception e3)
                    {

                        if (msg == "1")
                            MessageBox.Show("上传冰冻报告jpg文件异常" + e3.ToString());
                        log.WriteMyLog(blh + "上传冰冻报告jpg文件异常" + e3.ToString());

                    }
                    try
                    {
                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch
                    {
                        if (msg == "1")
                            MessageBox.Show("删除临时目录" + blh + "失败");
                        log.WriteMyLog(blh + "删除临时目录" + blh + "失败");
                    }
                }

    
                ////////////////////////////////////////////////
                ////常规报告jpg
                if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
                {
                    string bglx = "cg";
                    string bgxh = "1";


                     jpgname = "";
                     string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
                    {
                        #region  生成pdf

                        string message = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        if (debug == "1")
                            log.WriteMyLog("开始生成PDF。。。");
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref message, ref jpgname);

                        string xy = "3";
                        if (isrtn)
                        {
                            if (File.Exists(jpgname))
                            {
                                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                if (ssa == true)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("上传PDF成功");

                                    jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                    ZgqClass.BGHJ(blh, "JK", "审核", "上传PDF成功:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "上传PDF");

                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='上传PDF成功',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                                }
                                else
                                {
                                    log.WriteMyLog("上传PDF失败：" + message);
                                    ZgqClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "上传PDF");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("生成PDF失败:未找到文件---" + jpgname);
                                ZgqClass.BGHJ(blh, "JK", "审核", "上传PDF失败:未找到文件---" + jpgname, "ZGQJK", "生成PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败:未找到文件---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            }
                        }
                        else
                        {

                            log.WriteMyLog("生成PDF失败：" + message);
                            ZgqClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "生成PDF");
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='生成PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        }
                        zgq.DelTempFile(blh);
                        #endregion
                    }

                    //mdj.BMPTOJPG(blh, ref jpgname, "cg", "1");
                    //try
                    //{
                    //    //判断ftp上是否存在该jpg文件
                    //    if (fw.fileCheckExist(ftpURI, blh + "_cg_1.jpg"))
                    //    {
                    //        //删除ftp上的jpg文件
                    //        fw.fileDelete(ftpURI, blh + "_cg_1.jpg").ToString();
                    //    }
                    //    //上传新生成的jpg文件
                    //    fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_cg_1_1.jpg", "", out status);

                    //    if (status == "Error")
                    //    {
                    //        if (msg == "1")
                    //            MessageBox.Show("报告jpg上传失败，请重新审核！");
                    //         LGZGQClass.log.WriteMyLog(blh + "报告jpg上传失败，请重新审核！");
                    //    }
                    //}
                    //catch (Exception e4)
                    //{

                    //    if (msg == "1")
                    //        MessageBox.Show("上传报告jpg文件异常:" + e4.ToString());
                    //     LGZGQClass.log.WriteMyLog(blh + "上传报告jpg文件异常:" + e4.ToString());
                    //}
                    //try
                    //{
                    //    if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                    //        System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    //}
                    //catch
                    //{
                    //     LGZGQClass.log.WriteMyLog("删除临时目录" + blh + "失败");
                    //}
                }
                //////////////////////////////////////////




                //回传报告
                string ExamReportXML = "";
                string blzd_bd = "冰冻结果：" + dt_bd.Rows[0]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                for (int i = 1; i < dt_bd.Rows.Count; i++)
                {
                    blzd_bd = blzd_bd + "     第" + (i + 1).ToString() + "次冰冻结果：" + dt_bd.Rows[i]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                }


                if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
                {
                    string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                    if (sqxh == "")
                        sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();



                    string blzd = "常规病理诊断：" + bljc.Rows[0]["F_blzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                    blzd_bd = blzd + "    " + blzd_bd;
                    ExamReportXML = "<Request><ExamReportNew>"
                  + "<CheckFlow>" + sqxh + "</CheckFlow>"
                    + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                      + "<ExecLink></ExecLink>"
                      + "<ExamName></ExamName>"
                      + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                      + "<Findings>" + "" + "</Findings>"
                      + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                      + "<Id></Id>"
                      + "<ExamPara></ExamPara>"
                      + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                      + "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                      + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                       + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                        + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                         + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"
                      + "<ExamItemsList><ExaminationItems>"
                      + "<Code></Code><Name></Name><Value></Value>"
                      + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";
                }
                else
                {
                    ExamReportXML = "<Request><ExamReportNew>"
               + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                 + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/冰冻.asp?blh=" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</CheckLink>"
                   + "<ExecLink></ExecLink>"
                   + "<ExamName></ExamName>"
                   + "<Date>" + dt_bd.Rows[0]["F_BD_bgrq"].ToString().Trim() + "</Date>"
                   + "<Findings>" + "" + "</Findings>"
                   + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                   + "<Id></Id>"
                   + "<ExamPara></ExamPara>"
                   + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                   + "<Reporter>" + dt_bd.Rows[0]["F_BD_bgys"].ToString().Trim() + "</Reporter>"
                   + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                    + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                     + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                      + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"
                   + "<ExamItemsList><ExaminationItems>"
                   + "<Code></Code><Name></Name><Value></Value>"
                   + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                }
                if (debug == "1")
                    MessageBox.Show("回写报告XML：" + ExamReportXML);
                if (ExamReportXML.Trim() == "")
                {

                    if (msg == "1")
                        MessageBox.Show("生成xml错误，不能回传");
                    log.WriteMyLog(blh + "生成xml错误，不能回传");
                    return;
                }

                //****************************************************
                //回写报告
                //****************************************************
                string rtnExamReport = "";

                try
                {
                    rtnExamReport = xyfy.DhcService("ExamReportNew", ExamReportXML);
                }
                catch (Exception eReport)
                {
                    if (msg == "1")
                        MessageBox.Show("回写报告异常：" + eReport.ToString());
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + "回写报告异常：" + eReport.ToString());
                    return;
                }
                //****************************************************
                //回写状态返回的xml，解析
                //****************************************************

                if (debug == "1")
                {
                    MessageBox.Show("回写报告返回值" + rtnExamReport.ToString());
                }
                if (rtnExamReport == "")
                {
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + ",回写诊断：返回值为空");

                    //if (msg == "1")
                    //    MessageBox.Show("回写诊断：返回值为空");

                    return;
                }

                //------------------------------
                try
                {
                    XmlDataDocument xd2 = new XmlDataDocument();
                    xd2.LoadXml(rtnExamReport);
                    XmlNode xn2 = xd2.SelectSingleNode("/Response");
                    if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (msg == "1")
                            MessageBox.Show("回传报告失败，原因：" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog(ExamReportXML);
                        log.WriteMyLog(blh + "回传报告失败，原因：" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("回传报告成功 " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
                            aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");

                    }

                    ////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////
                    /////////撤销审核
                    //////////////////////
                    if (bgzt == "已写报告" && bljc.Rows[0]["F_scbj"].ToString().Trim() == "1")
                    {

                        string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                        if (sqxh == "")
                            sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
                        //////////////////////////////////////////
                        //回传报告
                        string ExamReportXML3 = "<Request><ExamReportNew>"
                        + "<CheckFlow>" + sqxh + "</CheckFlow>"
                          + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                            + "<ExecLink></ExecLink>"
                            + "<ExamName></ExamName>"
                            + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                            + "<Findings>" + "" + "</Findings>"
                            + "<Result>" + "  " + "</Result>"
                            + "<Id></Id>"
                            + "<ExamPara></ExamPara>"
                            + "<IsAbnormal>" + " " + "</IsAbnormal>"
                            + "<Reporter>" + " " + "</Reporter>"
                            + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                            + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                            + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                            
                            + "<ReqDocID></ReqDocID>"
                            + "<ReqDateTime></ ReqDateTime>"
                            + "<ScheduledDocID></ScheduledDocID>"
                            + "<ScheduledDateTime></ScheduledDateTime>"
                            + "<RegDocID>"+getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim())+"</RegDocID>"
                            + "<RegDateTime>"+bljc.Rows[0]["F_SDRQ"].ToString().Trim()+"</RegDateTime>"
                            + "<TechnicianID></TechnicianID>"
                            + "<ExamDateTime></ExamDateTime>"
                            + "<ReporterID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</ReporterID>"
                            + "<ReportDateTime>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd") + "</ReporterDateTime>"
                            + "<VerifyDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDocID>"
                            + "<VerifyDateTime>" + DateTime.Parse(bljc.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd") + "</VerifyDateTime>"
                            + "<IssueDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</IssueDocID>"
                            + "<IssueDateTime>"+DateTime.Now.ToString("yyyy-MM-dd")+"</IssueDateTime>"
                            + "<Status>4</Status>"
                            + "<OperatorDateTime>"+DateTime.Now.ToString("yyyy-MM-dd")+"</OperatorDateTime>"

                            + "<ExamItemsList><ExaminationItems>"
                            + "<Code></Code><Name></Name><Value></Value>"
                            + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                        if (debug == "1")
                            MessageBox.Show("回写报告XML：" + ExamReportXML3);


                        //****************************************************
                        //回写报告
                        //****************************************************
                        string rtnExamReport3 = "";

                        try
                        {
                            rtnExamReport3 = xyfy.DhcService("ExamReportNew", ExamReportXML3);
                        }
                        catch (Exception eReport)
                        {
                            if (msg == "1")
                                MessageBox.Show("回写报告异常，原因：" + eReport.ToString());
                            log.WriteMyLog(ExamReportXML3);
                            log.WriteMyLog("回写报告异常，原因：" + eReport.ToString());
                            return;
                        }
                        //****************************************************
                        //回写状态返回的xml，解析
                        //****************************************************

                        if (debug == "1")
                        {
                            MessageBox.Show("回写报告返回值" + rtnExamReport3.ToString());
                        }
                        if (rtnExamReport3 == "")
                        {
                            log.WriteMyLog(blh + ",回写诊断：返回值为空");
                            //if (msg == "1")
                            //    MessageBox.Show("回写诊断：返回值为空");

                            return;
                        }

                        //------------------------------
                        try
                        {
                            XmlDataDocument xd3 = new XmlDataDocument();
                            xd3.LoadXml(rtnExamReport3);
                            XmlNode xn3 = xd3.SelectSingleNode("/Response");
                            if (xn3.FirstChild["Returncode"].InnerText.ToString() == "-1")
                            {
                                log.WriteMyLog(ExamReportXML3);
                                //if (msg == "1")
                                //    MessageBox.Show("回传报告失败，原因：" + xn3.FirstChild["ResultContent"].InnerText.ToString());
                                log.WriteMyLog(blh + "回传报告，" + xn3.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn3.FirstChild["Returncode"].InnerText.ToString());
                            }
                            else
                            {
                                if (debug == "1")
                                    MessageBox.Show("回传报告成功 " + xn3.FirstChild["ResultContent"].InnerText.ToString());
                                aa.GetDataTable("update T_jcxx  set F_scbj='0'  where F_blh='" + blh + "'", "blx2");

                            }


                        }
                        catch (Exception rtnee)
                        {
                            if (msg == "1")
                                MessageBox.Show("回传报告,解析XML错误,抛出异常，" + rtnee.ToString());
                            log.WriteMyLog(ExamReportXML3);
                            log.WriteMyLog("回传报告,解析XML错误,抛出异常：" + rtnee.ToString());
                            return;

                        }

                    }
                }
                catch (Exception rtnee)
                {
                    if (msg == "1")
                    {
                        MessageBox.Show("回传报告,解析XML错误,抛出异常，" + rtnee.ToString());
                    }

                    log.WriteMyLog("回传报告,解析XML错误,抛出异常：" + rtnee.ToString());
                    return;

                }


                return;
            }
            #endregion


            //****************************************************
            //回写常规报告，拼xml
            //****************************************************region
            #region 回写常规报告
            if (bgzt == "已审核")
            {

                //生成jpg文件
                // 生成jpg格式报告文件
                string jpgname = "";
                string ftpstatus = "";
                mdjpg mdj = new mdjpg();
                string bglx = "cg";
                string bgxh = "1";
                try
                {
                    mdj.BMPTOJPG(blh, ref jpgname, bglx, bgxh);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
                //---上传jpg----------
                //----------------上传签章jpg至ftp---------------------

                string status = "";
                string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/blbgjpg").Replace("\0", "");
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                // string ml = DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Year.ToString();
                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";

                try
                {

                    ////判断目录是否存在
                    //if (!fw.fileCheckExist(ftpURI, ml))
                    //{
                    //    //目录不存在，创建
                    //    string stat = "";
                    //    fw.Makedir(ml, out stat);
                    //}
                    //判断ftp上是否存在该jpg文件
                    if (fw.fileCheckExist(ftpURI, blh + "_" + bglx + "_" + bgxh + ".jpg"))
                    {
                        //删除ftp上的jpg文件
                        fw.fileDelete(ftpURI, blh + "_" + bglx + "_" + bgxh + ".jpg").ToString();
                    }
                    //上传新生成的jpg文件
                    fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_" + bglx + "_" + bgxh + "_1.jpg", "", out status);

                    if (status == "Error")
                    {
                        MessageBox.Show("报告jpg上传失败，请重新审核！");
                    }

                }
                catch
                {
                    MessageBox.Show("上传报告jpg文件异常");
                }

                try
                {
                    if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                        System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                }
                catch
                {
                    log.WriteMyLog("删除临时目录" + blh + "失败");
                }


                //////////////////////////////////////////
                //回传报告
                string ExamReportXML = "<Request><ExamReport>"
                + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                  + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                    + "<ExecLink></ExecLink>"
                    + "<ExamName></ExamName>"
                    + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                    + "<Findings>" + "" + "</Findings>"
                    + "<Result>" + @bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                    + "<Id></Id>"
                    + "<ExamPara></ExamPara>"
                    + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                    + "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                    + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                     + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                      + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                       + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"



                    + "<ExamItemsList><ExaminationItems>"
                    + "<Code></Code><Name></Name><Value></Value>"
                    + "</ExaminationItems></ExamItemsList></ExamReport></Request>";

                if (debug == "1")
                    MessageBox.Show("回写报告XML：" + ExamReportXML);

                //****************************************************
                //回写报告
                //****************************************************
                string rtnExamReport = "";

                try
                {
                    rtnExamReport = xyfy.DhcService("ExamReport", ExamReportXML);
                }
                catch (Exception eReport)
                {
                    if (msg == "1")
                        MessageBox.Show("回写报告异常：" + eReport.ToString());
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + "回写报告异常：" + eReport.ToString());
                    return;
                }
                //****************************************************
                //回写状态返回的xml，解析
                //****************************************************

                if (debug == "1")
                {
                    MessageBox.Show("回写报告返回值" + rtnExamReport.ToString());
                }
                if (rtnExamReport == "")
                {
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + ",回写诊断：返回值为空");
                    if (debug == "1")
                        MessageBox.Show("回写诊断：返回值为空");
                    return;
                }

                //------------------------------
                try
                {
                    XmlDataDocument xd2 = new XmlDataDocument();
                    xd2.LoadXml(rtnExamReport);
                    XmlNode xn2 = xd2.SelectSingleNode("/Response");
                    if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (debug == "1")
                            MessageBox.Show("回传报告失败，原因：" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog(ExamReportXML);
                        log.WriteMyLog("回传报告，" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("回传报告成功 " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");

                    }


                }
                catch (Exception rtnee)
                {
                    if (msg == "1")
                    {
                        MessageBox.Show("回传报告,解析XML错误,抛出异常，" + rtnee.ToString());
                    }
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + "回传报告,解析XML错误,抛出异常：" + rtnee.ToString());
                    return;

                }

            }
            #endregion   

            #region
            if (bgzt == "已写报告" && bljc.Rows[0]["F_scbj"].ToString().Trim() == "1")
            {


                //////////////////////////////////////////
                //回传报告
                string ExamReportXML = "<Request><ExamReport>"
                + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                  + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                    + "<ExecLink></ExecLink>"
                    + "<ExamName></ExamName>"
                    + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                    + "<Findings>" + "" + "</Findings>"
                    + "<Result>" + "  " + "</Result>"
                    + "<Id></Id>"
                    + "<ExamPara></ExamPara>"
                    + "<IsAbnormal>" + " " + "</IsAbnormal>"
                    + "<Reporter>" + " " + "</Reporter>"
                    + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                     + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                      + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                       + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"



                    + "<ExamItemsList><ExaminationItems>"
                    + "<Code></Code><Name></Name><Value></Value>"
                    + "</ExaminationItems></ExamItemsList></ExamReport></Request>";

                if (debug == "1")
                    MessageBox.Show("回写报告XML：" + ExamReportXML);


                //****************************************************
                //回写报告
                //****************************************************
                string rtnExamReport = "";

                try
                {
                    rtnExamReport = xyfy.DhcService("ExamReport", ExamReportXML);
                }
                catch (Exception eReport)
                {
                    if (msg == "1")
                        MessageBox.Show("回写报告异常：" + eReport.ToString());
                    log.WriteMyLog("回写报告异常：" + eReport.ToString());
                    return;
                }
                //****************************************************
                //回写状态返回的xml，解析
                //****************************************************

                if (debug == "1")
                {
                    MessageBox.Show("回写报告返回值" + rtnExamReport.ToString());
                }
                if (rtnExamReport == "")
                {
                    log.WriteMyLog(blh + ",回写诊断：返回值为空");
                    if (debug == "1")
                        MessageBox.Show("回写诊断：返回值为空");

                    return;
                }

                //------------------------------
                try
                {
                    XmlDataDocument xd2 = new XmlDataDocument();
                    xd2.LoadXml(rtnExamReport);
                    XmlNode xn2 = xd2.SelectSingleNode("/Response");
                    if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (debug == "1")
                            MessageBox.Show("回传报告失败，原因：" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog("回传报告，" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("回传报告成功 " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        aa.GetDataTable("update T_jcxx  set F_scbj='0'  where F_blh='" + blh + "'", "blx2");

                    }


                }
                catch (Exception rtnee)
                {
                 log.WriteMyLog(blh + "回传报告,解析XML错误,抛出异常：" + rtnee.Message);
                    return;
                }
            }
            #endregion 取消审核
            return;
        }
        //执行oracle数据库查询操作，传sql语句，返回datatable
        private DataTable select_orcl(string orcl_strsql, string sm)
        {


            OleDbConnection orcl_con = new OleDbConnection(constr);
            OleDbDataAdapter orcl_dap = new OleDbDataAdapter(orcl_strsql, orcl_con);
            DataTable dt_bill_items = new DataTable();
            try
            {
                orcl_con.Open();
                orcl_dap.Fill(dt_bill_items);
                orcl_con.Close();
            }
            catch (Exception orcl_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("orcle数据库查询操作错误，" + sm + "--" + orcl_ee.Message);
                return dt_bill_items;
            }
            return dt_bill_items;
        }
        //执行oracle数据库查询操作，insert，update，delete，传sql语句，返回影响行数
        private int insert_orcl(string orcl_strsql, string sm)
        {
            OleDbConnection orcl_con = new OleDbConnection(constr);
            OleDbCommand ocdc = new OleDbCommand(orcl_strsql, orcl_con);
            int x = 0;
            try
            {
                orcl_con.Open();
                x = ocdc.ExecuteNonQuery();
                orcl_con.Close();
                ocdc.Dispose();
            }
            catch (Exception insert_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("插入orcal数据库插入错误" + sm + "--" + insert_ee.Message);
                return 0;
            }
            return x;

        }

        public int Execute_sql(string sqlstr)
        {
            string Server = f.ReadString("sqlserverzgq", "Server", "");
            string DataBase = f.ReadString("sqlserverzgq", "DataBase", "");
            string UserID = f.ReadString("sqlserverzgq", "UserID", "");
            string PassWord = f.ReadString("sqlserverzgq", "PassWord", "");
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = con = new SqlConnection(constr);
            SqlCommand sqlcom = null;
            try
            {
                con.Open();
                sqlcom = new SqlCommand(sqlstr, con);
                int x = sqlcom.ExecuteNonQuery();
                con.Close();
                sqlcom.Clone();

                return x;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("执行SQL语句异常，" + sqlstr + ",\r\n 异常原因：" + ee.Message);
                con.Close();
                sqlcom.Clone();
                return -1;
            }
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
                    return yhmc;
            }
            catch
            {
                return yhmc;
            }
        }
    }
}
