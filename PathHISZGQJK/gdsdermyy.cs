using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using ZgqClassPub;


namespace PathHISZGQJK
{
    /// <summary>
    /// 广东省第二人民医院,北京嘉和平台
    /// </summary>
    class gdsdermyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

        public void pathtohis(string blh, string bglx, string bgxh, string czlb, string dz, string msg, string debug)
        {

            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            string hczt = ZgqClass.GetSz("savetohis", "hczt", "0").Replace("\0", "").Trim();
            string hcbg = ZgqClass.GetSz("savetohis", "hcbg", "1").Replace("\0", "").Trim();
            string ptjk = ZgqClass.GetSz("savetohis", "ptjk", "1").Replace("\0", "").Trim();
            string wsurl = ZgqClass.GetSz("savetohis", "wsurl", "").Replace("\0", "").Trim();
            string updatefsb = ZgqClass.GetSz("savetohis", "updatefsb", "1").Replace("\0", "").Trim();

            DataTable bljc = new DataTable();
            DataTable dt_bcbg = new DataTable();
            DataTable dt_bdbg = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "1";

            string blbh = blh + bglx + bgxh;

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
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请单号不处理");
                 aa.ExecuteSQL("update T_jcxx_fs set F_bz='无申请单号不处理',F_fszt='不处理'  where F_blbh='" + blbh + "' ");
                return;
            }

            /////////////////////////////////////////////////
     

            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();

            if (bglx == "bc")
            {
                dt_bcbg = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "' and F_bc_bgxh='" + bgxh + "'", "bcbg");
                if (dt_bcbg == null || dt_bcbg.Rows.Count <= 0)
                {
                    log.WriteMyLog("未查询到此报告补充报告：" + blh + "^" + bgxh);
                    if (updatefsb == "1")
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='未查询到此报告补充报告：" + blh + "^" + bgxh + "' where F_blbh='" + blbh + "'  and F_fszt='未处理'");
                    return;
                }

                bgzt = dt_bcbg.Rows[0]["F_bc_BGZT"].ToString().Trim();
            }
            if (bglx == "bd")
            {
                dt_bdbg = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "' and F_bd_bgxh='" + bgxh + "'", "bcbg");
                if (dt_bdbg == null || dt_bdbg.Rows.Count <= 0)
                {
                    log.WriteMyLog("未查询到此报告冰冻报告：" + blh + "^" + bgxh);
                    if (updatefsb == "1")
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='未查询到此报告冰冻报告：" + blh + "^" + bgxh + "' where F_blbh='" + blbh + "' and F_fszt='未处理' ");
                    return;
                }

                bgzt = dt_bdbg.Rows[0]["F_bd_BGZT"].ToString().Trim();
            }

            if (dz == "qxsh")
                bgzt = "取消审核";

            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim();
            string brlb = bljc.Rows[0]["f_brlb"].ToString().Trim();

            //////////修改申请单登记状态//////////////////////////////////////////////

            //DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + sqxh + "'", "t_sqd");
            //if (dt_sqd.Rows.Count >= 1)
            //{
            //    if (dt_sqd.Rows[0]["F_SQDZT"] != "已登记")
            //    {
            //        aa.ExecuteSQL("update T_SQD set F_SQDZT='已登记' where F_sqxh='" + sqxh + "'");
            //    }
            //}
            ////if (bgzt.Trim() == "取消审核")
            ////{
            ////    string message = BgMsg(bljc, dt_bcbg, dt_bdbg, blh, bglx, bgxh, pdfpath, ref errmsg,bgzt);

            ////    if (message == "")
            ////    {
            ////         LGZGQClass.log.WriteMyLog("报告生成XMl失败");
            ////        if (updatefsb == "1")
            ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告生成XML失败：'" + errmsg + "' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
            ////        return;
            ////    }

            ////    if (debug == "1")
            ////         LGZGQClass.log.WriteMyLog("报告发送XML：" + message);

            ////    gds2websvr.PacsWS gds2 = new gds2websvr.PacsWS();
            ////    try
            ////    {
            ////        if (wsurl != "")
            ////            gds2.Url = wsurl;
            ////        string trnmsg = gds2.PacsReportBack(message);

            ////        if (debug == "1")
            ////             LGZGQClass.log.WriteMyLog("返回：" + trnmsg);
            ////        try
            ////        {
            ////            XmlNode xmlok = null;
            ////            XmlDocument xd = new XmlDocument();
            ////            try
            ////            {
            ////                xd.LoadXml(trnmsg);
            ////                xmlok = xd.SelectSingleNode("/RESPONSE");
            ////            }
            ////            catch
            ////            {
            ////                if (debug == "1")
            ////                    MessageBox.Show("XML解析错误");
            ////                return;
            ////            }

            ////            if (xmlok["RESULT_CODE"].InnerText == "true")
            ////            {
            ////                if (debug == "1")
            ////                     LGZGQClass.log.WriteMyLog("报告取消审核上传平台：" + xmlok["RESULT_CONTENT"].InnerText);
            ////                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "',F_fszt='已处理'  where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
            ////                return;
            ////            }
            ////            else
            ////            {
            ////                 LGZGQClass.log.WriteMyLog("报告取消审核上传平台：" + xmlok["RESULT_CONTENT"].InnerText);
            ////                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
            ////                return;
            ////            }
            ////        }
            ////        catch
            ////        {
            ////        }

            ////    }
            ////    catch (System.Web.Services.Protocols.SoapException ex)
            ////    {
            ////         LGZGQClass.log.WriteMyLog(ex.Detail["error"]["text"].InnerText.ToString());
            ////        if (msg == "1")
            ////            MessageBox.Show(ex.Detail["error"]["text"].InnerText.ToString());

            ////        if (updatefsb == "1")
            ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='取消审核发送失败:" + ex.Detail["error"]["text"].InnerText.ToString() + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");

            ////    }
            ////    catch (Exception ee)
            ////    {
            ////         LGZGQClass.log.WriteMyLog("连接web服务异常：" + ee.Message);
            ////        if (msg == "1")
            ////            MessageBox.Show("连接web服务异常：" + ee.Message);

            ////        if (updatefsb == "1")
            ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='连接web服务异常：" + ee.Message + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='取消审核'");

            ////    }

            ////}
            if (bgzt.Trim() == "取消审核")
            {
                aa.ExecuteSQL("delete  from T_BG_PDF  where F_blbh='" + blbh + "' ");
                aa.ExecuteSQL("delete  from T_BG_PDF_CA  where F_blbh='" + blbh + "' ");
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='',F_fszt='已处理'  where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='取消审核'");

            }
            if (bgzt.Trim() == "已审核" || bgzt.Trim() == "已发布")
            {

                string ispdf = ZgqClass.GetSz("savetohis", "ispdf", "0").Replace("\0", "").Trim();

                 DataTable dt_pdf = new DataTable();
                 string pdfpath = ""; string errmsg = "";
                 if (ispdf == "1")
                 {
                     try
                     {
                         dt_pdf = aa.GetDataTable("select * from T_BG_PDF where F_BLBH='" + blbh + "'", "dt_sqd");
                     }
                     catch (Exception ex)
                     {
                         log.WriteMyLog(ex.Message.ToString());
                         return;
                     }
                   
                     if (dt_pdf.Rows.Count <= 0)
                     {
                         string Base64String = "";
                         #region  生成pdf
                         string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");

                         string filename = bljc.Rows[0]["F_SPARE5"].ToString();
                         if (bglx == "bc")
                             filename = dt_bcbg.Rows[0]["F_bc_SPARE5"].ToString();
                         if (bglx == "bd")
                             filename = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString();

                         filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";

                         C_PDF(blh, bgxh, bglx, ML, ref filename, ref pdfpath, false, ref Base64String, debug, ref errmsg);

                         if (pdfpath == "")
                             log.WriteMyLog("生成pdf失败");
                         #endregion
                     }
                     else
                     {
                         pdfpath = dt_pdf.Rows[0]["F_FilePath"].ToString().Trim();
                     }
                 }



                if (hcbg.Trim() == "1" && ptjk=="1")
                {
                    if (ispdf == "1")
                    {
                        if (pdfpath == "")
                        {
                            log.WriteMyLog("pdf路径为空");
                            if (updatefsb == "1")
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='pdf路径为空' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                            return;
                        }
                       pdfpath= pdfpath.Replace("ftp", "http");
                    }
                    else
                    {
                        pdfpath = "http://192.168.3.153/pathwebrpt/index_z.asp?sqxh=" + bljc.Rows[0]["F_SQXH"].ToString();
                        if (bglx == "bd")
                        pdfpath = "http://192.168.3.153/pathwebrpt/冰冻.asp?blh="+blh;
                        if (bglx == "bc")
                        pdfpath = "http://192.168.3.153/pathwebrpt/补充.asp?blh="+blh;
                    }
                    string message = BgMsg(bljc, dt_bcbg, dt_bdbg, blh, bglx, bgxh, pdfpath, ref errmsg);

                    if (message == "")
                    {
                        log.WriteMyLog("报告生成XMl失败");
                        if (updatefsb == "1")
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告生成XML失败：" + errmsg + "' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                        return;
                    }

                    if (debug == "1")
                        log.WriteMyLog("报告发送XML：" + message);

                    gds2websvr.PacsWS gds2 = new gds2websvr.PacsWS();

                    try
                    {
                        if (wsurl != "")
                            gds2.Url = wsurl;
                        string trnmsg = gds2.PacsReportBack(message);

                        if (debug == "1")
                            log.WriteMyLog("返回：" + trnmsg);


                        try
                        {
                            XmlNode xmlok = null;
                            XmlDocument xd = new XmlDocument();
                            try
                            {
                                xd.LoadXml(trnmsg);
                                xmlok = xd.SelectSingleNode("/RESPONSE");
                            }
                            catch
                            {
                                if (debug == "1")
                                    MessageBox.Show("XML解析错误");
                                return;
                            }

                            if (xmlok["RESULT_CODE"].InnerText == "true")
                            {
                                if (debug == "1")
                                    log.WriteMyLog("报告上传平台：" + xmlok["RESULT_CONTENT"].InnerText);
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "',F_fszt='已处理'  where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                                return;
                            }
                            else
                            {
                                log.WriteMyLog("报告上传平台：" + xmlok["RESULT_CONTENT"].InnerText);
                            
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "' where F_blbh='" + blbh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                                return;
                            }
                        }
                        catch
                        {
                        }

                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
                        log.WriteMyLog(ex.Detail["error"]["text"].InnerText.ToString());
                        if (msg == "1")
                            MessageBox.Show(ex.Detail["error"]["text"].InnerText.ToString());

                        if (updatefsb == "1")
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='报告发送失败:" + ex.Detail["error"]["text"].InnerText.ToString() + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='已审核'");

                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("连接web服务异常：" + ee.Message);
                        if (msg == "1")
                            MessageBox.Show("连接web服务异常：" + ee.Message);

                        if (updatefsb == "1")
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='连接web服务异常：" + ee.Message + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_BGZT='已审核'");

                    }
                }
            }


        }

        public string BgMsg(DataTable dt_brxx, DataTable dt_bcbg, DataTable dt_bdbg, string blh, string bglx, string bgxh, string pdfpath, ref string errmsg)
        {
            try
            {
                string  bglxmc="常规";
                string bgzt = dt_brxx.Rows[0]["F_BGZT"].ToString().Trim();
                string qcrq = dt_brxx.Rows[0]["F_qcrq"].ToString().Trim();
                if (qcrq == "")
                    qcrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();

                string sdrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();
                string bgrq = dt_brxx.Rows[0]["F_bgrq"].ToString().Trim();

                string shrq = dt_brxx.Rows[0]["F_spare5"].ToString().Trim();
                string rysj = dt_brxx.Rows[0]["F_rysj"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_jxsj"].ToString().Trim();
                string blzd = dt_brxx.Rows[0]["F_blzd"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_tsjc"].ToString().Trim();
                string bgys = dt_brxx.Rows[0]["F_BGYS"].ToString().Trim();
                string shys = dt_brxx.Rows[0]["F_shys"].ToString().Trim();
               

                if (bglx == "bc")
                {
                    bglxmc="补充";
                    if (dt_bcbg.Rows.Count > 0)
                    {
                        bgrq = dt_bcbg.Rows[0]["F_bc_bgrq"].ToString().Trim();
                        shrq = dt_bcbg.Rows[0]["F_bc_spare5"].ToString().Trim();
                        bgys = dt_bcbg.Rows[0]["F_bc_bgys"].ToString().Trim();
                        shys = dt_bcbg.Rows[0]["F_bc_shys"].ToString().Trim();
                        blzd = dt_bcbg.Rows[0]["F_bczd"].ToString().Trim();
                        bgzt = dt_bcbg.Rows[0]["F_bc_bgzt"].ToString().Trim();
               
                       // bglxmc = "补充报告";
                    }
                    else
                    {
                        return "";
                    }
                }
                if (bglx == "bd")
                {
                     bglxmc="冰冻";
                    if (dt_bdbg.Rows.Count > 0)
                    {
                        bgrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        shrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        bgys = dt_bdbg.Rows[0]["F_bd_bgys"].ToString().Trim();
                        shys = dt_bdbg.Rows[0]["F_bd_shys"].ToString().Trim();
                        blzd = dt_bdbg.Rows[0]["F_bdzd"].ToString().Trim();
                        bgzt = dt_bdbg.Rows[0]["F_bd_bgzt"].ToString().Trim();
                        rysj = "";
                       // bglxmc = "冰冻报告";
                    }
                    else
                    {
                        return "";
                    }
                }

                try
                {
                    sdrq = DateTime.Parse(sdrq).ToString("yyyy-MM-dd HH:mm:ss");
                    qcrq = DateTime.Parse(qcrq).ToString("yyyy-MM-dd HH:mm:ss");
                    if (bgrq != "")
                        bgrq = DateTime.Parse(bgrq).ToString("yyyy-MM-dd HH:mm:ss");
                    if (shrq != "")
                        shrq = DateTime.Parse(shrq).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                }
                if (bgzt != "已审核" && bgzt != "已发布")
                {
                    blzd = "";
                    rysj = "";
                    shrq = "";
                    bgrq = "";
                    bgys = "";
                    shys = "";
                    pdfpath = "";
                }

                string blbh = "BL" + blh + bglx + bgxh;

                string xml = "";
                //xml = xml + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../../Schemas/POOR_IN200901UV20.xsd\">";
                xml = xml + "<id extension=\"JHIPBS304\" />";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\" />";
                xml = xml + "<interactionId root=\"\" extension=\"POOR_IN200901UV20\" />";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<processingModeCode code=\"T\" />";
                xml = xml + "<acceptAckCode code=\"NE\" />";

                //  xml = xml + "<!-- 接受者 -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                // xml = xml + "<!-- 接受者 ID root 为医院内部定义各个系统 ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                //  xml = xml + "<!-- 发送者 -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                // xml = xml + "<!-- 发送者 ID root 为医院内部定义各个系统 ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension='病理系统'/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                //  xml = xml + "<!-- 封装的消息内容 -->";
                xml = xml + "<realmCode code=\"CN\"/>";
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_MT000040\"/>";
                xml = xml + "<templateId root=\"2.16.156.10011.2.1.1.26\"/>";
                // xml = xml + "<!-- 文档唯一标识 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.2.3\" extension=\"" + blbh + "\"/>";
                xml = xml + "<title>病理检查报告</title>";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"/>";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"1.2.156.112636.1.1.2.1.4.31\" codeSystemName=\"正常访问保密级别\" displayName=\"保密\"/>";
                xml = xml + "<languageCode code=\"zh-CN\"/>";
                xml = xml + "<setId/>";
                xml = xml + "<versionNumber/>";
                ////xml=xml+"<!--文档记录对象（患者） -->";
                xml = xml + "<recordTarget typeCode=\"RCT\" contextControlCode=\"OP\">";
                xml = xml + "<patientRole classCode=\"PAT\">";
                ////xml=xml+"<!--患者 ID(patient_id)标识,全院统一患者标识 -->";
                if (dt_brxx.Rows[0]["F_BRLB"].ToString().Trim()=="住院")
                xml = xml + "<id root=\"1.2.156.112636.1.2.1.4.1\" extension=\"" + dt_brxx.Rows[0]["F_ZYH"].ToString().Trim() + "\"/>";
                else
                xml = xml + "<id root=\"1.2.156.112636.1.2.1.4.1\" extension=\"" + dt_brxx.Rows[0]["F_MZH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--门（急）诊号标识 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.3\" extension=\"" + dt_brxx.Rows[0]["F_MZH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--住院号标识-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.2\" extension=\"" + dt_brxx.Rows[0]["F_ZY"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--报告单号标识（ 检查编号 EXAM_NO） -->";
               
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh+bglxmc +bgxh+ "\"/>";
              
                ////xml=xml+"<!--电子申请单编号,如果有多个申请单号，拆分为多条（ 申请号 EXAM_APPLY_NO） -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.20\" extension=\"" + dt_brxx.Rows[0]["F_SQXH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--标本编号标识，病理检查有，其他检查不需要 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"\"/>";
                ////xml=xml+"<!--影像号/放射号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.2.2.1.1\" extension=\"" + dt_brxx.Rows[0]["F_BLH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--病理号，只有病理检查有 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.26\" extension=\"" + dt_brxx.Rows[0]["F_BLH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- 患者就诊类别代码(displayName 为 FILE_VISIT_TYPE) -->";
                xml = xml + "<patientType>";

                string brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "门诊")
                    brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "住院")
                    brlbbm = "2";

                xml = xml + "<patienttypeCode code=\"" + brlbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.4.10\" codeSystemName=\"患者就诊类型代码表\" displayName=\"" + dt_brxx.Rows[0]["F_BRLB"].ToString().Trim() + "\"/>";
                xml = xml + "</patientType>";
                ////xml=xml+"<!-- 联系电话 -->";
                xml = xml + "<telecom value=\"" + "" + "\"/>";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                ////xml=xml+"<!--患者身份证号标识-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.8\" extension=\"" + dt_brxx.Rows[0]["F_SFZH"].ToString().Trim() + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_XM"].ToString().Trim() + "</name>";

                string xbbm = "0";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "男")
                    xbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "女")
                    xbbm = "2";
                xml = xml + "<administrativeGenderCode code=\"" + xbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.2.1\" codeSystemName=\"性别代码表\" displayName=\"" + dt_brxx.Rows[0]["F_XB"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- 年龄 -->";
                xml = xml + "<age unit=\"岁\" value=\"" + dt_brxx.Rows[0]["F_age"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--出生日期-->";
                xml = xml + "<birthTime value=\"" + "" + "\"/>";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                // xml = xml + "<!-- 检查报告医师（文档创作者） -->";
                xml = xml + "<author typeCode=\"AUT\" contextControlCode=\"OP\">";
                // xml = xml + "<!-- 检查报告日期 -->";
                xml = xml + "<time value=\"" + bgrq + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                // xml = xml + "<!-- 医师工号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(bgys.Trim()) + "\"/>";
                // xml = xml + "<!-- 医师姓名-->";
                xml = xml + "<assignedPerson>";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";


                //  xml = xml + "<!-- 保管机构 -->";
                xml = xml + "<custodian typeCode=\"CST\">";
                xml = xml + "<assignedCustodian classCode=\"ASSIGNED\">";
                xml = xml + "<representedCustodianOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>广东省第二人民医院</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";


                //  xml = xml + "<!-- 审核医师签名 -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time value=\"" + shrq + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!-- 医师工号  -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(shys.Trim()) + "\"/>";
                xml = xml + "<code displayName=\"审核医师\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";


                //  xml = xml + "<!-- 检查技师签名 -->";
                xml = xml + "<authenticator>";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!-- 医师工号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"\"/>";
                xml = xml + "<code displayName=\"检查技师\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_JSY"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                //   xml = xml + "<!-- 检查申请机构及科室 -->";
                xml = xml + "<participant typeCode=\"PRF\">";
                //  xml = xml + "<!-- 申请时间 -->";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<scopingOrganization>";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"" + "" + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                xml = xml + "<asOrganizationPartOf>";
                xml = xml + "<wholeOrganization>";
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>广东省第二人民医院</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";




                xml = xml + "<inFulfillmentOf>";
                //    xml = xml + "<!--医嘱信息，一份报告关联多个医嘱，重复多条-->";
                xml = xml + "<order>";
                //    xml = xml + "<!--医嘱号,医嘱唯一标识-->";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.19\" extension=\"" + "" + "\"/>";
                //   xml = xml + "<!--医嘱项目-->";
                xml = xml + "<code code=\"\" displayName=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";


                xml = xml + "<documentationOf>";
                xml = xml + "<serviceEvent>";
                //   xml = xml + "<!--检查执行时间-->";


                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.3.1.7\"/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--检查医生工号-->";
                xml = xml + "<id extension=\"" + getyhgh(dt_brxx.Rows[0]["F_qcys"].ToString().Trim()) + "\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //   xml = xml + "<!--检查医生姓名 -->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_qcys"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "</serviceEvent>";
                xml = xml + "</documentationOf>";


                //   xml = xml + "<!-- 病床号、病房、病区、科室和医院的关联 -->";
                xml = xml + "<componentOf>";
                xml = xml + "<encompassingEncounter>";
                xml = xml + "<effectiveTime/>";
                xml = xml + "<location>";
                xml = xml + "<healthCareFacility>";
                xml = xml + "<serviceProviderOrganization>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization classCode=\"" + "1"+ "\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.23\" extension=\"" + dt_brxx.Rows[0]["F_CH"].ToString().Trim() + "\"/>";
                //    xml = xml + "<!-- DE01.00.019.00 病房号 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
              
                    xml = xml + "<wholeOrganization classCode=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString().Trim() + "(" + bglxmc + ")" + "\" determinerCode=\"INSTANCE\">";

                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
              
                xml = xml + "<wholeOrganization classCode=\"NULL\" determinerCode=\"INSTANCE\">";
                ///////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                ///////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_BQ"].ToString().Trim() + "</name>";
                //    xml = xml + "<!-- DE08.10.026.00 科室名称 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //    xml = xml + "<!-- classCode 为费用 COSTS -->";
                xml = xml + "<wholeOrganization classCode=\"0\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"\"/>";
                ////////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                //    xml = xml + "<!--XXX 医院 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636\" />";
                xml = xml + "<name>广东省第二人民医院</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</location>";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";


                //  xml = xml + "<!--文档体 Body-->";
                xml = xml + "<component>";
                xml = xml + "<structuredBody classCode=\"DOCBODY\" moodCode=\"EVN\">";
                //   xml = xml + "<!--检查预约章节-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"预约信息\" />";
                //  xml = xml + "<!--预约时间-->";
                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                //  xml = xml + "<!--预约人-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                //   xml = xml + "<!--工号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJYS"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //    xml = xml + "<!--拟检查信息-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"预约信息\" />";
                //    xml = xml + "<!--拟检查时间-->";
                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                //    xml = xml + "<!--拟检查医师-->";
                xml = xml + "<participant typeCode=\"PRF\">";
                xml = xml + "<participantRole>";
                //    xml = xml + "<!--检查医师工号-->";
                xml = xml + "<id extension=\"\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //    xml = xml + "<!--检查医师姓名-->";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //    xml = xml + "<!--拟检查设备-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //    xml = xml + "<!--设备号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"27433\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName>显微镜</manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //    xml = xml + "<!--拟检查地点-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                //      xml = xml + "<!--检查室名称-->";
                xml = xml + "<location>";
                xml = xml + "<name>病理科</name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                xml = xml + "</act>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";



                //   xml = xml + "<!--检查专业章节-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                //////////////////////////
                xml = xml + "<code code=\"29545-1\" displayName=\"PHYSICAL EXAMINATION\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" />";
                xml = xml + "<title>病理检查</title>";
                //   xml = xml + "<!--人可读内容，可为空 -->";
                xml = xml + "<text/>";
                //   xml = xml + "<!--检查数据处理条目-->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                //   xml = xml + "<!--检查类别-->";
                //////////////////////////////////////
                xml = xml + "<code code=\"29576-6\" codeSystem=\"1.2.156.112636.1.1.2.1.4.40\" codeSystemName=\"检查类别代码表\" displayName=\"新病理\"/>";
                //  xml = xml + "<!--检查状态，取值范围： completed|abort|active，对应：完成|作废|正在执行-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ///////////////////////////////////////
                //  xml = xml + "<!--检查时间，如果检查状态是 completed，代表报告时间， abort 代表作废时间， active 代表检查时间（ value 依据状态可为检查时间 EXAM_DATE_TIME、 报告时间 REPORT_DATE_TIME） -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"/>";
                //  xml = xml + "<!--检查执行者，如果检查状态是 completed，代表报告医师， abort 代表作废医师， active 代表检查医师-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--工号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(dt_brxx.Rows[0]["F_SHYS"].ToString().Trim()) + "\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                //  xml = xml + "<!--name 为执行医生 1 PERFORMED_BY1-->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SHYS"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //   xml = xml + "<!--检查时间-->";
                xml = xml + "<effectiveTime>";
                //   xml = xml + "<!--RIS 登记时间-->";
                xml = xml + "<low value=\"" + sdrq + "\"/>";
                //   xml = xml + "<!--检查时间-->";
                xml = xml + "<high value=\"" + qcrq + "\"/>";
                xml = xml + "</effectiveTime>";
                //  xml = xml + "<!--样本信息，可选，病理检查等涉及到样本的填写-->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                xml = xml + "<specimenRole classCode=\"SPEC\">";
                //  xml = xml + "<!--样本编号/条码-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"" + blh + "\"/>";
                xml = xml + "<specimenPlayingEntity>";
                // xml = xml + "<!--样本类型-->";
                xml = xml + "<code code=\"DD\" codeSystem=\"1.2.156.112636.1.1.2.1.3.5\" codeSystemName=\"样本类型代码\" displayName=\"" + dt_brxx.Rows[0]["F_BBLX"].ToString().Trim() + "\"/>";
                xml = xml + "</specimenPlayingEntity>";
                xml = xml + "</specimenRole>";
                xml = xml + "</specimen>";
                //  xml = xml + "<!--检查设备-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //   xml = xml + "<!--设备号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName>显微镜</manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                ///  xml = xml + "<!--检查地点-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                //   xml = xml + "<!--检查室名称-->";
                xml = xml + "<location>";
                xml = xml + "<name>病理科</name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--检查操作相关-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.1.8\"/>";
                xml = xml + "<methodCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.41\" codeSystemName=\" 检 查 方 法 代 码 表 \" displayName=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString() + "\"/>";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.8\" codeSystemName=\" 检 查 部 位 代 码 表 \" displayName=\"" + dt_brxx.Rows[0]["f_bbmc"].ToString() + "\"/>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                //   xml = xml + "<!-- 检查报告章节 -->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code displayName=\"检查报告\"/>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                //   xml = xml + "<!--检查报告组织-->";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                //    xml = xml + "<!--报告号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blbh + "\"/>";

                //////
                xml = xml + "<statusCode code=\"completed\"/>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='404684003' displayName='Finding' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(rysj) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='282291009' displayName='Diagnosis' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(blzd) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";



                //  xml = xml + "<!-- 影像链接 -->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code  displayName='影像中心链接' />";
                xml = xml + "<reference typeCode='REFR|SPRT'>";
                xml = xml + "<externalDocument classCode='DOC' moodCode='EVN'>";
                xml = xml + "<id extension='' root=''/>";
                xml = xml + "<text><reference value='" + pdfpath + "'/></text>";
                xml = xml + "</externalDocument>";
                xml = xml + "</reference>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                // xml = xml + "<!--报告备注-->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<templateId extension=\"1.3.6.1.4.1.19376.1.5.3.1.4.2\"/>";
                xml = xml + "<code code=\"48767-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"AnnotationComment\"/>";
                xml = xml + "<text><reference value=\"" + "报告备注" + "\"/></text>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";
                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</POOR_IN200901UV>";



                return FormatXml(xml);
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
               log.WriteMyLog(ee.Message);
                return "";
            }
        }

        public string BgMsg(DataTable dt_brxx, DataTable dt_bcbg, DataTable dt_bdbg, DataTable dt_sqd, string blh, string bglx, string bgxh, string pdfpath, ref string errmsg)
        {
            try
            {

                string bgzt = dt_brxx.Rows[0]["F_BGZT"].ToString().Trim();
                string qcrq = dt_brxx.Rows[0]["F_qcrq"].ToString().Trim();
                if (qcrq == "")
                    qcrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();

                string sdrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();
                string bgrq = dt_brxx.Rows[0]["F_bgrq"].ToString().Trim();

                string shrq = dt_brxx.Rows[0]["F_spare5"].ToString().Trim();
                string rysj = dt_brxx.Rows[0]["F_rysj"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_jxsj"].ToString().Trim();
                string blzd = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_tsjc"].ToString().Trim();
                string bgys = dt_brxx.Rows[0]["F_BGYS"].ToString().Trim();
                string shys = dt_brxx.Rows[0]["F_shys"].ToString().Trim();


                if (bglx == "bc")
                {
                    if (dt_bcbg.Rows.Count > 0)
                    {
                        bgrq = dt_bcbg.Rows[0]["F_bc_bgrq"].ToString().Trim();
                        shrq = dt_bcbg.Rows[0]["F_bc_spare5"].ToString().Trim();
                        bgys = dt_bcbg.Rows[0]["F_bc_bgys"].ToString().Trim();
                        shys = dt_bcbg.Rows[0]["F_bc_shys"].ToString().Trim();
                        blzd = dt_bcbg.Rows[0]["F_bczd"].ToString().Trim();
                        bgzt = dt_bcbg.Rows[0]["F_bc_bgzt"].ToString().Trim();
                    }
                    else
                    {
                        return "";
                    }
                }
                if (bglx == "bd")
                {
                    if (dt_bdbg.Rows.Count > 0)
                    {
                        bgrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        shrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        bgys = dt_bdbg.Rows[0]["F_bd_bgys"].ToString().Trim();
                        shys = dt_bdbg.Rows[0]["F_bd_shys"].ToString().Trim();
                        blzd = dt_bcbg.Rows[0]["F_bdzd"].ToString().Trim();
                        bgzt = dt_bcbg.Rows[0]["F_bd_bgzt"].ToString().Trim();
                    }
                    else
                    {
                        return "";
                    }
                }

                try
                {
                    sdrq = DateTime.Parse(sdrq).ToString("yyyyMMdd");
                    qcrq = DateTime.Parse(qcrq).ToString("yyyyMMdd");
                    if (bgrq != "")
                        bgrq = DateTime.Parse(bgrq).ToString("yyyyMMdd");
                    if (shrq != "")
                        shrq = DateTime.Parse(shrq).ToString("yyyyMMdd");
                }
                catch
                {
                }
                if (bgzt != "已审核" && bgzt != "已发布")
                {
                    blzd = "";
                    rysj = "";
                    shrq = "";
                    bgrq = "";
                    bgys = "";
                    shys = "";
                }


                string xml = "";
                xml = xml + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../../Schemas/POOR_IN200901UV20.xsd\">";
                xml = xml + "<id extension=\"JHIPBS304\" />";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<interactionId root=\"\" extension=\"POOR_IN200901UV20\" />";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<processingModeCode code=\"T\" />";
                xml = xml + "<acceptAckCode code=\"NE\" />";

                //  xml = xml + "<!-- 接受者 -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                //   xml = xml + "<!-- 接受者 ID root 为医院内部定义各个系统 ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                //   xml = xml + "<!-- 发送者 -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                //   xml = xml + "<!-- 发送者 ID root 为医院内部定义各个系统 ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension='病理系统'/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                //  xml = xml + "<!-- 封装的消息内容 -->";
                xml = xml + "<realmCode code=\"CN\"/>";
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_MT000040\"/>";
                xml = xml + "<templateId root=\"2.16.156.10011.2.1.1.26\"/>";
                //  xml = xml + "<!-- 文档唯一标识 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.2.3\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
                xml = xml + "<title>病理检查报告</title>";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"1.2.156.112636.1.1.2.1.4.31\" codeSystemName=\"正常访问保密级别\" displayName=\"保密\"/>";
                xml = xml + "<languageCode code=\"zh-CN\"/>";
                xml = xml + "<setId/>";
                xml = xml + "<versionNumber/>";
                ////xml=xml+"<!--文档记录对象（患者） -->";
                xml = xml + "<recordTarget typeCode=\"RCT\" contextControlCode=\"OP\">";
                xml = xml + "<patientRole classCode=\"PAT\">";
                ////xml=xml+"<!--患者 ID(patient_id)标识,全院统一患者标识 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.2.1.4.1\" extension=\"" + dt_brxx.Rows[0]["F_ZYH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--门（急）诊号标识 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.3\" extension=\"" + dt_brxx.Rows[0]["F_MZH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--住院号标识-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.2\" extension=\"" + dt_brxx.Rows[0]["F_ZY"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--报告单号标识（ 检查编号 EXAM_NO） -->";
                if (bglx == "bc")
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh +"补充"+bgxh+ "\"/>";
                else if (bglx == "bc")
                    xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh + "冰冻" + bgxh + "\"/>";
                else xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh + "常规"  + "\"/>";
                ////xml=xml+"<!--电子申请单编号,如果有多个申请单号，拆分为多条（ 申请号 EXAM_APPLY_NO） -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.20\" extension=\"" + dt_brxx.Rows[0]["F_SQXH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--标本编号标识，病理检查有，其他检查不需要 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"\"/>";
                ////xml=xml+"<!--影像号/放射号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.2.2.1.1\" extension=\"\"/>";
                ////xml=xml+"<!--病理号，只有病理检查有 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.26\" extension=\"" + dt_brxx.Rows[0]["F_BLH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- 患者就诊类别代码(displayName 为 FILE_VISIT_TYPE) -->";
                xml = xml + "<patientType>";

                string brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "门诊")
                    brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "住院")
                    brlbbm = "2";

                xml = xml + "<patienttypeCode code=\"" + brlbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.4.10\" codeSystemName=\"患者就诊类型代码表\" displayName=\"" + dt_brxx.Rows[0]["F_BRLB"].ToString().Trim() + "\"/>";
                xml = xml + "</patientType>";
                ////xml=xml+"<!-- 联系电话 -->";
                xml = xml + "<telecom value=\"" + dt_brxx.Rows[0]["F_LXXX"].ToString().Trim() + "\"/>";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                ////xml=xml+"<!--患者身份证号标识-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.8\" extension=\"" + dt_brxx.Rows[0]["F_SFZH"].ToString().Trim() + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_XM"].ToString().Trim() + "</name>";

                string xbbm = "0";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "男")
                    xbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "女")
                    xbbm = "2";
                xml = xml + "<administrativeGenderCode code=\"" + xbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.2.1\" codeSystemName=\"性别代码表\" displayName=\"" + dt_brxx.Rows[0]["F_XB"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- 年龄 -->";
                xml = xml + "<age unit=\"岁\" value=\"" + dt_brxx.Rows[0]["F_age"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--出生日期-->";
                xml = xml + "<birthTime value=\"" + "" + "\"/>";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                // xml = xml + "<!-- 检查报告医师（文档创作者） -->";
                xml = xml + "<author typeCode=\"AUT\" contextControlCode=\"OP\">";
                //  xml = xml + "<!-- 检查报告日期 -->";
                xml = xml + "<time value=\"" + bgrq + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                //  xml = xml + "<!-- 医师工号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(bgys.Trim()) + "\"/>";
                //  xml = xml + "<!-- 医师姓名-->";
                xml = xml + "<assignedPerson>";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";


                // xml = xml + "<!-- 保管机构 -->";
                xml = xml + "<custodian typeCode=\"CST\">";
                xml = xml + "<assignedCustodian classCode=\"ASSIGNED\">";
                xml = xml + "<representedCustodianOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>广东省第二人民医院</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";


                // xml = xml + "<!-- 审核医师签名 -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time value=\"" + DateTime.Parse(shrq.Trim()).ToString("yyyyMMdd") + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!-- 医师工号  -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(shys.Trim()) + "\"/>";
                xml = xml + "<code displayName=\"审核医师\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";


                // xml = xml + "<!-- 检查技师签名 -->";
                xml = xml + "<authenticator>";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                // xml = xml + "<!-- 医师工号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"\"/>";
                xml = xml + "<code displayName=\"检查技师\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_JSY"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                //  xml = xml + "<!-- 检查申请机构及科室 -->";
                xml = xml + "<participant typeCode=\"PRF\">";
                //  xml = xml + "<!-- 申请时间 -->";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<scopingOrganization>";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"" + "" + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                xml = xml + "<asOrganizationPartOf>";
                xml = xml + "<wholeOrganization>";
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>广东省第二人民医院</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";




                xml = xml + "<inFulfillmentOf>";
                //  xml = xml + "<!--医嘱信息，一份报告关联多个医嘱，重复多条-->";
                xml = xml + "<order>";
                //   xml = xml + "<!--医嘱号,医嘱唯一标识-->";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.19\" extension=\"" + "" + "\"/>";
                //  xml = xml + "<!--医嘱项目-->";
                xml = xml + "<code code=\"ORG\" displayName=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";


                xml = xml + "<documentationOf>";
                xml = xml + "<serviceEvent>";
                //  xml = xml + "<!--检查执行时间-->";


                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.3.1.7\"/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--检查医生工号-->";
                xml = xml + "<id extension=\"" + getyhgh(dt_brxx.Rows[0]["F_qcys"].ToString().Trim()) + "\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //  xml = xml + "<!--检查医生姓名 -->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_qcys"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "</serviceEvent>";
                xml = xml + "</documentationOf>";


                //   xml = xml + "<!-- 病床号、病房、病区、科室和医院的关联 -->";
                xml = xml + "<componentOf>";
                xml = xml + "<encompassingEncounter>";
                xml = xml + "<effectiveTime/>";
                xml = xml + "<location>";
                xml = xml + "<healthCareFacility>";
                xml = xml + "<serviceProviderOrganization>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //   xml = xml + "<!-- DE01.00.026.00 病床号 -->";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.23\" extension=\"" + dt_brxx.Rows[0]["F_CH"].ToString().Trim() + "\"/>";
                //    xml = xml + "<!-- DE01.00.019.00 病房号 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //   xml = xml + "<!-- classCode 为检查项目 EXAM_ITEM 和检查子类名称 EXAM_SUB_CLASS -->";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                // xml = xml + "<!-- DE08.10.054.00 病区名称 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //xml = xml + "<!-- classCode 为项目代码 EXAM_ITEM_CODE -->";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                ///////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                ///////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_BQ"].ToString().Trim() + "</name>";
                // xml = xml + "<!-- DE08.10.026.00 科室名称 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                // xml = xml + "<!-- classCode 为费用 COSTS -->";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"001\"/>";
                ////////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                // xml = xml + "<!--XXX 医院 -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636\" />";
                xml = xml + "<name>广东省第二人民医院</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</location>";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";


                //  xml = xml + "<!--文档体 Body-->";
                xml = xml + "<component>";
                xml = xml + "<structuredBody classCode=\"DOCBODY\" moodCode=\"EVN\">";
                //  xml = xml + "<!--检查预约章节-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"预约信息\" />";
                //  xml = xml + "<!--预约时间-->";
                xml = xml + "<effectiveTime value=\"20090415144550.0000-0500\"/>";
                //  xml = xml + "<!--预约人-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--工号 -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"83736\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //  xml = xml + "<!--拟检查信息-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"预约信息\" />";
                //  xml = xml + "<!--拟检查时间-->";
                xml = xml + "<effectiveTime value=\"20090415144550.0000-0500\"/>";
                //  xml = xml + "<!--拟检查医师-->";
                xml = xml + "<participant typeCode=\"PRF\">";
                xml = xml + "<participantRole>";
                //  xml = xml + "<!--检查医师工号-->";
                xml = xml + "<id extension=\"54689A\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //   xml = xml + "<!--检查医师姓名-->";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--拟检查设备-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //   xml = xml + "<!--设备号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"27433\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName>设备名称</manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--拟检查地点-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                ///  xml = xml + "<!--检查室名称-->";
                xml = xml + "<location>";
                xml = xml + "<name></name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                xml = xml + "</act>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";



                //  xml = xml + "<!--检查专业章节-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                //////////////////////////
                xml = xml + "<code code=\"29545-1\" displayName=\"PHYSICAL EXAMINATION\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" />";
                xml = xml + "<title>病理检查</title>";
                //    xml = xml + "<!--人可读内容，可为空 -->";
                xml = xml + "<text/>";
                //   xml = xml + "<!--检查数据处理条目-->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                //   xml = xml + "<!--检查类别-->";
                //////////////////////////////////////
                xml = xml + "<code code=\"29576-6\" codeSystem=\"1.2.156.112636.1.1.2.1.4.40\" codeSystemName=\"检查类别代码表\" displayName=\"\"/>";
                ///  xml = xml + "<!--检查状态，取值范围： completed|abort|active，对应：完成|作废|正在执行-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ///////////////////////////////////////
                //    xml = xml + "<!--检查时间，如果检查状态是 completed，代表报告时间， abort 代表作废时间， active 代表检查时间（ value 依据状态可为检查时间 EXAM_DATE_TIME、 报告时间 REPORT_DATE_TIME） -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\"/>";
                //   xml = xml + "<!--检查执行者，如果检查状态是 completed，代表报告医师， abort 代表作废医师， active 代表检查医师-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                // xml = xml + "<!--工号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(dt_brxx.Rows[0]["F_SHYS"].ToString().Trim()) + "\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                //   xml = xml + "<!--name 为执行医生 1 PERFORMED_BY1-->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SHYS"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //   xml = xml + "<!--检查时间-->";
                xml = xml + "<effectiveTime>";
                //   xml = xml + "<!--RIS 登记时间-->";
                xml = xml + "<low value=\"" + sdrq + "\"/>";
                //  xml = xml + "<!--检查时间-->";
                xml = xml + "<high value=\"" + qcrq + "\"/>";
                xml = xml + "</effectiveTime>";
                //  xml = xml + "<!--样本信息，可选，病理检查等涉及到样本的填写-->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                xml = xml + "<specimenRole classCode=\"SPEC\">";
                //   xml = xml + "<!--样本编号/条码-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"\"/>";
                xml = xml + "<specimenPlayingEntity>";
                //   xml = xml + "<!--样本类型-->";
                xml = xml + "<code code=\"DD\" codeSystem=\"1.2.156.112636.1.1.2.1.3.5\" codeSystemName=\"样本类型代码\" displayName=\"" + dt_brxx.Rows[0]["F_BBLX"].ToString().Trim() + "\"/>";
                xml = xml + "</specimenPlayingEntity>";
                xml = xml + "</specimenRole>";
                xml = xml + "</specimen>";
                //   xml = xml + "<!--检查设备-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //    xml = xml + "<!--设备号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName></manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--检查地点-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                //    xml = xml + "<!--检查室名称-->";
                xml = xml + "<location>";
                xml = xml + "<name>病理科</name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                //    xml = xml + "<!--检查操作相关-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.1.8\"/>";
                xml = xml + "<methodCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.41\" codeSystemName=\" 检 查 方 法 代 码 表 \" displayName=\"" + dt_brxx.Rows[0]["f_blk"].ToString() + "\"/>";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.8\" codeSystemName=\" 检 查 部 位 代 码 表 \" displayName=\"" + dt_brxx.Rows[0]["f_bbmc"].ToString() + "\"/>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                //    xml = xml + "<!-- 检查报告章节 -->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code displayName=\"检查报告\"/>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                //     xml = xml + "<!--检查报告组织-->";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                //     xml = xml + "<!--报告号-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh + "\"/>";

                //////
                xml = xml + "<statusCode code=\"completed\"/>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='404684003' displayName='Finding' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(rysj) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='282291009' displayName='Diagnosis' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(blzd) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";



                //    xml = xml + "<!-- 影像链接 -->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code  displayName='影像中心链接' />";
                xml = xml + "<reference typeCode='REFR'>";
                xml = xml + "<externalDocument classCode='DOC' moodCode='EVN'>";
                xml = xml + "<id extension='' root=''/>";
                xml = xml + "<text><reference value='" + pdfpath + "'/></text>";
                xml = xml + "</externalDocument>";
                xml = xml + "</reference>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                //     xml = xml + "<!--报告备注-->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<templateId extension=\"1.3.6.1.4.1.19376.1.5.3.1.4.2\"/>";
                xml = xml + "<code code=\"48767-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"AnnotationComment\"/>";
                xml = xml + "<text><reference value=\"" + "" + "\"/></text>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";
                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</POOR_IN200901UV>";

                return xml;
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                return "";
            }
        }




   
        public void C_PDF(string blh, string bgxh, string bglx, string ml, ref string filename, ref string pdfpath, bool isToBase64String, ref string Base64String, string debug, ref  string errmsg)
        {
            string blbh = blh + bglx + bgxh;
            if (filename == "")
                filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";

            string rptpath = ZgqClass.GetSz("zgqjk", "rptpath", "rpt").Replace("\0", "").Trim();
            errmsg = "";
            ZgqPDFJPG pdf = new ZgqPDFJPG();
            bool pdf1 = pdf.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref filename, rptpath.Trim(), ref errmsg);
            if (!pdf1)
            {
                log.WriteMyLog("生成PDF文件失败:" + errmsg);
                return ;
            }
            if (File.Exists(filename))
            {
                if (isToBase64String)
                {

                    try
                    {
                        FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                        file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                        file.Close();
                        Base64String = Convert.ToBase64String(imgByte);
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("PDF转换二进制串失败");
                        errmsg = "PDF转换二进制串失败";
                        Base64String = "";
                    }
                }

                //上传pdf

                if (pdf.UpPDF(blh, filename, ml, ref errmsg, 0,ref pdfpath))
                {
                    if (debug == "1")
                        log.WriteMyLog("上传PDF成功");
                    filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                    ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + pdfpath, "ZGQJK", "上传PDF");
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" + blh + "','" + filename + "','" + pdfpath + "','')");
                }
                else
                {
                    log.WriteMyLog("上传PDF失败：" + errmsg);
                    ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
                }
            }
            else
            {
                log.WriteMyLog("生成PDF失败：" + errmsg);
                ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：" + errmsg, "ZGQJK", "生成PDF");
            }

            return;

        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
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
            StringBuilder sb = new StringBuilder();
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sUnformattedXml);
               
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
                catch(Exception  ee1)
                {
                    log.WriteMyLog(ee1.Message + "\r\n" + sUnformattedXml);
                }
                finally
                {
                    if (xtw != null)
                        xtw.Close();
                }
                return sb.ToString();
            }
            catch(Exception  ee2)
            {
                log.WriteMyLog(ee2.Message + "\r\n" + sUnformattedXml);
                return sb.ToString();
            }
        }

    }
}
