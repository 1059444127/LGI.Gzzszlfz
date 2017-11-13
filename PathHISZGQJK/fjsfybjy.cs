
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using ZgqClassPub;
namespace PathHISZGQJK
{
    //福建省妇幼保健院 webservices+hl7
    class fjsfybjy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string msg = ""; string debug = "";
        public void pathtohis(string blh, string bglx, string bgxh, string msg1, string debug1, string[] cslb)
        {
            msg=msg1;
            debug=debug1;
            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            if (bgxh == "")
                bgxh = "0";
            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                     qxsh = "1";//取消审核动作
                   
                if (cslb[3].ToLower() == "new")
                    xdj = "1";
            }

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

        

            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

            if (qxsh == "1")
            {
                bgzt = "取消审核";
            }

          


           if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "" && jcxx.Rows[0]["F_BRBH"].ToString().Trim() == "" && jcxx.Rows[0]["F_ZYH"].ToString().Trim() == "" && jcxx.Rows[0]["F_MZH"].ToString().Trim() == "")
            {
                 log.WriteMyLog("手工录入病人不处理");
                return;
            }



            int plsc = f.ReadInteger("fsjk", "plsc", 0);
            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            string brbh = jcxx.Rows[0]["F_BRBH"].ToString().Trim();
            string brlb = jcxx.Rows[0]["F_brlb"].ToString().Trim();
            string sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

            string ZYH = jcxx.Rows[0]["F_mzh"].ToString().Trim();

            if (brlb == "住院")
                ZYH = jcxx.Rows[0]["F_ZYH"].ToString().Trim();

            if (brlb == "门诊")
                ZYH = jcxx.Rows[0]["F_YZID"].ToString().Trim();
            if (brlb == "住院") brlb = "I";
            else brlb = "O";
         
         
           

            string SFZH = jcxx.Rows[0]["F_SFZH"].ToString().Trim();
            string XM = jcxx.Rows[0]["F_XM"].ToString().Trim();
            string SJKS = jcxx.Rows[0]["F_BQ"].ToString().Trim();
            string CH = jcxx.Rows[0]["F_CH"].ToString().Trim();
            string YZXM = jcxx.Rows[0]["F_YZXM"].ToString().Trim();

            string todzbl = f.ReadString("savetohis", "todzbl", "").Replace("\0", "").Trim();
            if (plsc!=1)
            {
               //无申请序号不会写状态，不扣费
            if (sqxh != "" && bgzt != "已审核")
            {

                string iskf = f.ReadString("savetohis", "iskf", "").Replace("\0", "").Trim();
                if (iskf == "1")
                {
                    if (jcxx.Rows[0]["F_FYQR"].ToString().Trim() != "1")
                    {
                        //上传报告状态和扣费
                        string XML = "<root><patientType>" + brlb + "</patientType><patientId>" + brbh + "</patientId><operationType>1</operationType><executeDeptCode>JC06</executeDeptCode>"
                                    + "<executeDoctorCode>" + CZYGH + "</executeDoctorCode><applyNo>" + sqxh + "</applyNo></root>";
                        if (debug == "1")
                           log.WriteMyLog("扣费入参：" + XML);
                        string rtn_msg = rtn_CallInterface("XML", "SendDocumentsFee", XML, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");

                        if (rtn_msg.Contains("error"))
                        {
                            log.WriteMyLog(rtn_msg);
                            if (msg == "1")
                                MessageBox.Show("扣费返回：" + rtn_msg);
                        }
                        else
                        {
                            aa.ExecuteSQL("update T_JCXX  set F_FYQR='1' where F_BLH='" + blh + "'");
                        }
                        if (debug == "1")
                            log.WriteMyLog("扣费返回：" + rtn_msg);
                    }
                }

                    ////回状态
                    string bgzt_1 = "";
                    if (bgzt == "已登记" || bgzt == "已取材")
                        bgzt_1 = "S";

                    if (bgzt == "已写报告" || bgzt == "报告延期")
                        bgzt_1 = "R";

                    if (bgzt == "已写报告" && jcxx.Rows[0]["F_HXBJ"].ToString().Trim() == "1")
                        bgzt_1 = "D";

                    if (bgzt == "取消审核")
                        bgzt_1 = "D";

                    if (bgzt_1 == "")
                    {
                        if (debug == "1")
                            log.WriteMyLog("bgzt_1状态为空，不回写！");
                        return;
                    }

                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                              + "PID|||" + brlb + "^" + ZYH + "^^^^^^^^^^^PN||" + XM + "^||||||||||||\r"
                                  + "PV1||" + brlb + "||||||||||||||||" + ZYH + "\r"
                                + "ORC|SC|" + sqxh + "|||RC|||||||" + CZY + "^" + CZYGH + "|病理/JC06\r"
                                + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;

                    if (debug == "1")
                        log.WriteMyLog("修改报告状态入参：" + ChangeGmsApplyStatus_Hl7);

                string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");

          
            if (rtn_msg2.Contains("error"))
                {
                    if (msg == "1")
                        MessageBox.Show( rtn_msg2);
                    //if (debug == "1")
                    //     LGZGQClass.log.WriteMyLog("修改报告状态返回：" + rtn_msg2);
                    log.WriteMyLog("修改报告状态返回：" + rtn_msg2);
                }
                else
                {
                    readhl7_fjfy r7 = new readhl7_fjfy();
                int xy=0;
                    r7.Adt01(rtn_msg2,ref xy);
                    if (r7.MSA[1].Trim() == "AA")
                    {
                        if (debug == "1")
                     log.WriteMyLog(r7.MSA[3].Trim());
                    }
                    else
                    {
                        if (debug == "1")
                         log.WriteMyLog("修改报告状态返回：" + rtn_msg2);

                    if (msg == "1")
                        MessageBox.Show(r7.MSA[3].Trim());
                    log.WriteMyLog(rtn_msg2);
                    }
                }
              
            }

            //无申请序号,取消审核
            if (sqxh.Trim() == "" && bgzt == "取消审核")
            {

                ////取消审核
                string bgzt_1 = "D";
                string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                          + "PID|||" + brlb + "^" + ZYH + "^^^^^^^^^^^PN||" + XM + "^||||||||||||\r"
                              + "PV1||" + brlb + "||||||||||||||||" + ZYH + "\r"
                            + "ORC|SC|" + blh + "|||RC|||||||" + CZY + "^" + CZYGH + "|病理/JC06\r"
                            + "OBR||" + blh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;

                if (debug == "1")
                    log.WriteMyLog("取消审核入参：" + ChangeGmsApplyStatus_Hl7);

                string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");


                if (rtn_msg2.Contains("error"))
                {
                    if (msg == "1")
                        MessageBox.Show(rtn_msg2);
                    log.WriteMyLog("取消审核入参返回：" + rtn_msg2);
                }
                else
                {
                    readhl7_fjfy r7 = new readhl7_fjfy();
                    int xy = 0;
                    r7.Adt01(rtn_msg2, ref xy);
                    if (r7.MSA[1].Trim() == "AA")
                    {
                        if (debug == "1")
                            log.WriteMyLog(r7.MSA[3].Trim());
                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("取消审核入参：" + rtn_msg2);
                        else
                            log.WriteMyLog(rtn_msg2);
                        if (msg == "1")
                            MessageBox.Show(r7.MSA[3].Trim());
                        
                    }
                }

            }




           //调czf的接口方法，传电子病历
            if (todzbl=="1")
            xmzyhis(blh, "");

          

            }
            else
            {
                //同步病人金额



              //////////////////////////////////////////////////////////////////
                //回传报告
                string bz = "";
                if (bgzt == "已审核")
                {
                    string hxbj = jcxx.Rows[0]["F_HXBJ"].ToString().Trim();
                    if (hxbj != "1")
                    {
                        //上传结果
                        string zt2 = "F";

                        if (hxbj == "1" && hxbj == "0")
                            zt2 = "C";

                        string scbj = jcxx.Rows[0]["F_hxbj"].ToString().Trim();

                        string tjtxpath = f.ReadString("savetohis", "toPDFPath", @"\\198.100.100.13\files\GMS\BL").Replace("\0", "").Trim(); ;

                        string F_ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                        string filename =  blh.Trim() + "_" + bglx +"_"+ bgxh + "_"+DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
                        string pdflj = tjtxpath + "\\" + F_ML + "\\" + blh + "\\" + filename;

                        string bgys = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                        string bgysgh = getyhgh(bgys);

                        string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                        string shysgh = getyhgh(shys);
                        if (sqxh == "")
                            sqxh = blh;



                        string bgrq = DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        string SendGmsReport_hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORU^R01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
        + "PID|||" + brbh + "^" + ZYH + "^^^^^^^^^^^PN||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||||||||||||" + "\r"
        + "PV1||" + brlb + "|" + SJKS + "^^" + CH + "|||||||||||||||" + ZYH + "||||||||||||||||||||||||||" + "\r"
        + "OBR||" + sqxh + "||" + YZXM + "||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|||||||||" + jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "|^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim() + "||||||" + bgrq + "||||||||||" + bgysgh + "&" + bgys + "" + "\r"
        + "NTE|1||" + pdflj + "|Z-RP" + "\r"
        + "OBX|1|FT|^肉眼所见||" + jcxx.Rows[0]["F_RYSJ"].ToString().Trim().Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + bgrq + "||" + shysgh + "^" + shys + "||" + "\r"
        + "OBX|2|FT| ^病理诊断||" + jcxx.Rows[0]["F_BLZD"].ToString().Trim().Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + bgrq + "||" + shysgh + "^" + shys + "||" + "\r";

                        DataTable dt_tx = aa.GetDataTable("select * from V_DYTX where F_blh='" + blh + "'", "tx");
                        if (dt_tx.Rows.Count > 0)
                        {
                            for (int x = 0; x < dt_tx.Rows.Count; x++)
                            {

                                string txlj = @"http://198.100.100.14/pathwebrpt/pathimages/" + jcxx.Rows[0]["F_TXML"].ToString().Trim() + "/" + dt_tx.Rows[x]["F_TXM"].ToString();
                                SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|" + (x + 1).ToString() + "|" + dt_tx.Rows[x]["F_ID"].ToString() + "|" + blh + "|" + txlj + "||1" + "\r";
                            }
                        }
                        else
                        {
                            SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|1|1|" + blh + "|||" + "\r";
                        }

                        if (debug == "1")
                            log.WriteMyLog("报告审核，上传结果入参：" + SendGmsReport_hl7);

                        string rtn_msg2 = rtn_CallInterface("HL7v2", "SendGmsReport", SendGmsReport_hl7, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");

                        if (rtn_msg2.Contains("error"))
                        {
                            if (debug == "1")
                                log.WriteMyLog("报告审核，上传结果返回：" + rtn_msg2);
                        }
                        else
                        {


                            if (rtn_msg2.Contains("success"))
                            {
                                aa.ExecuteSQL("update T_JCXX  set F_HXBJ='1' where F_BLH='" + blh + "'");
                                if (debug == "1")
                                    log.WriteMyLog("报告审核，上传结果返回：" + rtn_msg2);
                            }
                            else
                            {
                                log.WriteMyLog("报告审核，上传结果返回错误：" + rtn_msg2);
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + rtn_msg2.Substring(0, 150) + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                                 return;
                            }

                        }
                        if (rtn_msg2.Length < 50)
                            bz = rtn_msg2 + ";";
                        else
                            bz = rtn_msg2.Substring(0, 50) + ";";

                    }
                }


                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //指定某电脑，生成pdf
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
                    string message = "";
                    // MD_JPG_PDF md = new MD_JPG_PDF();
                    string F_ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    //int x = md.CreatePDF(blh, bglx, bgxh, MD_JPG_PDF.type.PDF, ref message,F_ML,1);



                    string jpgname = "";
                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF,ref jpgname, ref message );


                    bool rtnpdf = true;
                    string xy = ZgqClass.GetSz("ZGQJK", "sctxfs", "2");
                    if (isrtn)
                    {
                        string ftpPath = "";
                        bool ssa = zgq.UpPDF(blh, jpgname, F_ML, ref message, 3, ref ftpPath);
                        if (ssa == true)
                        {
                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + F_ML + "\\" + blh + "','" + jpgname + "','" + ftpPath + "')");
                        }
                        else
                            log.WriteMyLog("上传PDF病理服务器失败:" + message);

                        string pdfml = "";
                        ssa = zgq.UpPDF(blh, jpgname, F_ML, ref message, int.Parse(xy),ref pdfml, ref ftpPath,"savetohis");
                        if (ssa == true)
                        {
                         
                            aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='" + bz + "生成PDF成功" + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");

                            DataTable dt_fs = aa.GetDataTable("select top 1 F_blh,F_FSCS  from T_jcxx_fs  where F_blbh='" + blbh + "'  and F_fszt='已处理' and F_FSCS>='5'  and F_bgzt='已审核'", "dt2");
                            if (dt_fs.Rows.Count > 0)
                            {

                                if (brbh.Trim() != "" && int.Parse(dt_fs.Rows[0]["F_FSCS"].ToString().Trim()) >= 5)
                                {
                                    if (sqxh.Trim() == "")
                                        sqxh = blh;

                                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                  + "PID|||" + brlb + "^" + ZYH + "^^^^^^^^^^^PN||" + XM + "^||||||||||||\r"
                                      + "PV1||" + brlb + "||||||||||||||||" + ZYH + "\r"
                                    + "ORC|SC|" + sqxh + "|||RC|||||||" + CZY + "^" + CZYGH + "|病理/JC06\r"
                                    + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||HS";

                                    if (debug == "1")
                                        log.WriteMyLog("修改报告状态入参：" + ChangeGmsApplyStatus_Hl7);

                                    string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");


                                    if (rtn_msg2.Contains("error"))
                                    {
                                        if (msg == "1")
                                            MessageBox.Show(rtn_msg2);
                                        log.WriteMyLog("修改报告状态返回：" + rtn_msg2);
                                    }
                                    else
                                    {
                                        readhl7_fjfy r7 = new readhl7_fjfy();
                                        int xxy = 0;
                                        r7.Adt01(rtn_msg2, ref xxy);
                                        if (r7.MSA[1].Trim() == "AA")
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog(r7.MSA[3].Trim());
                                        }
                                        else
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog("修改报告状态返回：" + rtn_msg2);

                                            if (msg == "1")
                                                MessageBox.Show(r7.MSA[3].Trim());
                                            log.WriteMyLog(rtn_msg2);
                                        }
                                    }


                                }
                            }
                        }
                        else
                        {
                            log.WriteMyLog("上传PDF平台失败:" + message);
                            rtnpdf=false;
                        }
                    }
                    
                    else
                    {
                        rtnpdf=false;
                        log.WriteMyLog("生成PDF失败:" + message);
                    }
                    zgq.DelTempFile(blh);

                    if(!rtnpdf)
                    {
                        log.WriteMyLog(message);
                        if (message.Length > 150)
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + bz + message.Substring(0, 150) + "' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                        else
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + bz + message + "' where F_blbh='" + blbh + "'   and F_fszt='未处理' and F_bgzt='" + bgzt + "'");


                        //生成pdf失败告知集成平台
                        DataTable dt_fs = aa.GetDataTable("select top 1 F_blh,F_FSCS  from T_jcxx_fs  where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_FSCS>='5'  and F_bgzt='已审核'", "dt2");
                        if (dt_fs.Rows.Count > 0)
                        {
                            DataTable dt_jcxx = new DataTable();
                            dt_jcxx = aa.GetDataTable("select * from T_JCXX where F_BLH='" + dt_fs.Rows[0]["F_BLH"].ToString().Trim() + "'", "d1");
                            if (dt_jcxx == null)
                            {
                                log.WriteMyLog("数据库连接错误！");
                            }
                            if (dt_jcxx.Rows.Count > 0)
                            {
                                //string brbh = dt_jcxx.Rows[0]["F_brbh"].ToString().Trim();
                                //string sqxh = dt_jcxx.Rows[0]["F_sqxh"].ToString().Trim();
                                //string zyh = dt_jcxx.Rows[0]["F_zyh"].ToString().Trim();
                                if (brbh.Trim() != "" && int.Parse(dt_fs.Rows[0]["F_FSCS"].ToString().Trim()) >= 5)
                                {
                                    // string SendPdfNoticeInfo_xml = "<root><pacsBillNo>" + blh + "</pacsBillNo><hisBillNo>" + sqxh + "</hisBillNo>"
                                    //     + "<patientId>" + brbh + "</patientId><visitNo>" + ZYH + "</visitNo><isSuccess>0</isSuccess></root>";

                                    // if (debug == "1")
                                    //    LGZGQClass.log.WriteMyLog(""+SendPdfNoticeInfo_xml);

                                    // string rtn_msg2 = rtn_CallInterface("XML", "SendPdfNoticeInfo", SendPdfNoticeInfo_xml, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
                                    // if (debug == "1")
                                    // LGZGQClass.log.WriteMyLog(rtn_msg2);


                                    if (sqxh.Trim() == "")
                                        sqxh = blh;

                                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                  + "PID|||" + brlb + "^" + ZYH + "^^^^^^^^^^^PN||" + XM + "^||||||||||||\r"
                                      + "PV1||" + brlb + "||||||||||||||||" + ZYH + "\r"
                                    + "ORC|SC|" + sqxh + "|||RC|||||||" + CZY + "^" + CZYGH + "|病理/JC06\r"
                                    + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||HF";

                                    if (debug == "1")
                                        log.WriteMyLog("修改报告状态入参：" + ChangeGmsApplyStatus_Hl7);

                                    string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");


                                    if (rtn_msg2.Contains("error"))
                                    {
                                        if (msg == "1")
                                            MessageBox.Show(rtn_msg2);
                                        log.WriteMyLog("修改报告状态返回：" + rtn_msg2);
                                    }
                                    else
                                    {
                                        readhl7_fjfy r7 = new readhl7_fjfy();
                                        int xxy = 0;
                                        r7.Adt01(rtn_msg2, ref xxy);
                                        if (r7.MSA[1].Trim() == "AA")
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog(r7.MSA[3].Trim());
                                        }
                                        else
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog("修改报告状态返回：" + rtn_msg2);

                                            if (msg == "1")
                                                MessageBox.Show(r7.MSA[3].Trim());
                                            log.WriteMyLog(rtn_msg2);
                                        }
                                    }


                                }
                                else
                                {

                                }


                            }

                        }
                    }
                }
                else
                {
                    if (bgzt == "取消审核")
                    {
                             aa.ExecuteSQL("update T_JCXX  set F_HXBJ='0' where F_BLH='" + blh + "'");
                             aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                            aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='删除PDF成功！' where F_blbh='" + blbh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                         
                    }
                    else
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='未知操作！' where F_blbh='" + blbh + "'  and F_fszt='未处理'");


                }

              
                return;
            }
        }

        public  string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string certificate)
        {
            string url = f.ReadString("savetohis", "url", "").Replace("\0", "").Trim();

            fjsfybjyWeb.WSInterface wsif = new fjsfybjyWeb.WSInterface();
            if (url.Trim() != "")
                wsif.Url = url;

            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";
            try
            {
                return wsif.CallInterface(msgHeader, msgBody);
            }
            catch (Exception ee)
            {


                if (debug == "1")
                  log.WriteMyLog("调用rtn_CallInterface方法异常：" + ee.ToString());
                if (msg == "1")
                    MessageBox.Show("调用rtn_CallInterface方法异常：" + ee.ToString());
                log.WriteMyLog("调用rtn_CallInterface方法异常：" + ee.ToString());
                return "-1";
            }
        }

        public string getyhgh(string yhmc)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc+ "'", "yh");

            if (dt_yh.Rows.Count > 0)
                return dt_yh.Rows[0]["F_YHBH"].ToString();
            else
                return "000";
        }


        public void xmzyhis(string blh, string yymc)
        {
           
            // dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");
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


            //if (bljc.Rows[0]["F_sqxh"].ToString().Trim() != "")
            //{
            //    if (bljc.Rows[0]["F_yzid"].ToString().Trim() == "")
            //    {
            //        OdbcConnection mzconn = new OdbcConnection();
            //        mzconn.ConnectionString = "DSN=pathnet-sqd;UID=ele_apply;Pwd=ele_apply;";
            //        mzconn.ConnectionTimeout = 60;

            //        OdbcParameter[] mzORAPAR = new OdbcParameter[13];

            //        for (int j = 0; j < mzORAPAR.Length; j++)
            //        {
            //            mzORAPAR[j] = new OdbcParameter();
            //        }
            //        mzORAPAR[0].ParameterName = "as_apply_no";//单据号
            //        mzORAPAR[0].OdbcType = OdbcType.Char;
            //        mzORAPAR[0].Direction = ParameterDirection.Input;

            //        mzORAPAR[1].ParameterName = "as_result_status";//状态
            //        mzORAPAR[1].OdbcType = OdbcType.Char;
            //        mzORAPAR[1].Direction = ParameterDirection.Input;

            //        mzORAPAR[2].ParameterName = "as_report_man";//
            //        mzORAPAR[2].OdbcType = OdbcType.Char;
            //        mzORAPAR[2].Direction = ParameterDirection.Input;

            //        mzORAPAR[3].ParameterName = "adt_report_date";//
            //        mzORAPAR[3].OdbcType = OdbcType.DateTime;
            //        mzORAPAR[3].Direction = ParameterDirection.Input;

            //        mzORAPAR[4].ParameterName = "as_examine_man";//
            //        mzORAPAR[4].OdbcType = OdbcType.Char;
            //        mzORAPAR[4].Direction = ParameterDirection.Input;

            //        mzORAPAR[5].ParameterName = "adt_examine_date";//
            //        mzORAPAR[5].OdbcType = OdbcType.DateTime;
            //        mzORAPAR[5].Direction = ParameterDirection.Input;

            //        mzORAPAR[6].ParameterName = "adt_bespeak_date";//
            //        mzORAPAR[6].OdbcType = OdbcType.DateTime;
            //        mzORAPAR[6].Direction = ParameterDirection.Input;

            //        mzORAPAR[7].ParameterName = "as_device_code";//
            //        mzORAPAR[7].OdbcType = OdbcType.Char;
            //        mzORAPAR[7].Direction = ParameterDirection.Input;

            //        mzORAPAR[8].ParameterName = "as_check_part";//
            //        mzORAPAR[8].OdbcType = OdbcType.Char;
            //        mzORAPAR[8].Direction = ParameterDirection.Input;

            //        mzORAPAR[9].ParameterName = "as_check_address";//
            //        mzORAPAR[9].OdbcType = OdbcType.Char;
            //        mzORAPAR[9].Direction = ParameterDirection.Input;

            //        mzORAPAR[10].ParameterName = "precontract_memo";//
            //        mzORAPAR[10].OdbcType = OdbcType.Char;
            //        mzORAPAR[10].Direction = ParameterDirection.Input;

            //        mzORAPAR[11].ParameterName = "as_done_flag";//
            //        mzORAPAR[11].OdbcType = OdbcType.Char;
            //        mzORAPAR[11].Direction = ParameterDirection.Input;

            //        mzORAPAR[12].ParameterName = "rs_error_ms";//
            //        mzORAPAR[12].OdbcType = OdbcType.VarChar;
            //        mzORAPAR[12].Direction = ParameterDirection.Output;
            //        mzORAPAR[12].Size = 1000;

            //        mzORAPAR[0].Value = bljc.Rows[0]["F_sqxh"].ToString().Trim();
            //        mzORAPAR[1].Value = "1";
            //        mzORAPAR[2].Value = "blk";
            //        mzORAPAR[3].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        mzORAPAR[4].Value = "blk";
            //        mzORAPAR[5].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        mzORAPAR[6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //        mzORAPAR[7].Value = bljc.Rows[0]["F_blh"].ToString().Trim();
            //        mzORAPAR[8].Value = " ";
            //        mzORAPAR[9].Value = "blk";
            //        mzORAPAR[10].Value = "";
            //        mzORAPAR[11].Value = "0";
            //        try
            //        {

            //            OdbcCommand mzcmd = new OdbcCommand();
            //            mzcmd.Connection = mzconn;
            //            //mzcmd.CommandText = "PRC_PACS_CONFIRMOUTPATIENT";//声明存储过程名
            //            mzcmd.CommandText = "{ CALL p_apply_order_info_return_pacs(?,?,?,?,?,?,?,?,?,?,?,?,?)}";


            //            mzcmd.Parameters.Add(mzORAPAR[0]);
            //            mzcmd.Parameters.Add(mzORAPAR[1]);
            //            mzcmd.Parameters.Add(mzORAPAR[2]);
            //            mzcmd.Parameters.Add(mzORAPAR[3]);
            //            mzcmd.Parameters.Add(mzORAPAR[4]);
            //            mzcmd.Parameters.Add(mzORAPAR[5]);
            //            mzcmd.Parameters.Add(mzORAPAR[6]);
            //            mzcmd.Parameters.Add(mzORAPAR[7]);
            //            mzcmd.Parameters.Add(mzORAPAR[8]);
            //            mzcmd.Parameters.Add(mzORAPAR[9]);
            //            mzcmd.Parameters.Add(mzORAPAR[10]);
            //            mzcmd.Parameters.Add(mzORAPAR[11]);
            //            mzcmd.Parameters.Add(mzORAPAR[12]);

            //            mzconn.Open();

            //            mzcmd.ExecuteNonQuery();//执行存储过程

            //            if (mzORAPAR[12].Value.ToString().Trim() != "")
            //            {
            //                MessageBox.Show("病理号：" + blh + "申请单确认不成功！");

            //                 LGZGQClass.log.WriteMyLog(blh + " " + mzORAPAR[12].Value.ToString());
            //            }
            //            else
            //            {

            //                aa.ExecuteSQL("update T_jcxx set F_yzid='ck' where F_blh='" + blh + "'");
            //                 LGZGQClass.log.WriteMyLog(blh + " 申请单确认成功");
            //                //  MessageBox.Show(ORAPAR[18].Value.ToString());

            //            }
            //        }
            //        catch (Exception e)
            //        {
            //             LGZGQClass.log.WriteMyLog(e.Message);
            //        }
            //        finally
            //        {
            //            mzconn.Close();
            //        }


            //    }
            //}



            if (bljc.Rows[0]["F_zyh"].ToString().Trim().Length < 10)
            {
                log.WriteMyLog("无住院次数，不处理！");
                return;
            }
            if (bljc.Rows[0]["F_bgzt"].ToString().Trim() != "已审核")
            {
                return;
            }

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "住院")
            {
                log.WriteMyLog("非住院病人，不处理！");
                return;
            }
            string hisname = f.ReadString("xmzy", "hisname", "sfyemr").Replace("\0", "");
            string user = f.ReadString("xmzy", "USER", "zemr").Replace("\0", "");
            string passwd = f.ReadString("xmzy", "PASSWD", "sfyemr").Replace("\0", "");
            string ConnectionString = "Data Source=" + hisname + ";user=" + user + ";password=" + passwd + ";";//写连接串
            OracleConnection conn = new OracleConnection(ConnectionString);
            OracleCommand cmd = new OracleCommand();
            string zjbsj = "0";
            string zjbtx = "0";
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = "select * from ZEMR_PACS_REPORT where PACS_NO = 'BL" + bljc.Rows[0]["F_blh"].ToString().Trim() + "' and PACS_TYPE='5.00'";//查询是否有病理报告
                conn.Open();
                OracleDataReader odr = cmd.ExecuteReader();

                if (odr.Read())//读取数据，如果返回为false的话，就说明到记录集的尾部了  
                {
                    zjbsj = "1";
                }
                else
                {
                    zjbsj = "0";
                }
                odr.Close();//关闭reader.这是一定要写的  

                cmd.CommandText = "select * from ZEMR_LIS_PACS_REPORT_IMAGE where REPORT_NO = 'BL" + bljc.Rows[0]["F_blh"].ToString().Trim() + "'";//查询是否有病理报告图像

                OracleDataReader odr2 = cmd.ExecuteReader();

                if (odr2.Read())//读取数据，如果返回为false的话，就说明到记录集的尾部了  
                {
                    zjbtx = "1";
                }
                else
                {
                    zjbtx = "0";
                }
                odr2.Close();//关闭reader.这是一定要写的  



                string oraclesqlstring, blxml;
                blxml = "";
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + "?>";
                inxml = inxml + "<root>";
                inxml = inxml + "<INSPECT_CONTENT>";
                inxml = inxml + "<RYSJ note=" + (char)34 + "肉眼所见" + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</RYSJ>";
                inxml = inxml + "<JXSJ note=" + (char)34 + "镜下所见" + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_jxsj"].ToString().Trim()) + "</JXSJ>";
                inxml = inxml + "<BLZD note=" + (char)34 + "病理诊断" + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blzd"].ToString().Trim()) + "</BLZD>";

                inxml = inxml + "</INSPECT_CONTENT>";
                inxml = inxml + "</root>";
                blxml = inxml;
                //inxml = System.Security.SecurityElement.Escape(blxml);
                byte[] blxmlb = System.Text.Encoding.Default.GetBytes(inxml);
                if (zjbsj == "1")
                {
                    oraclesqlstring = "update ZEMR_PACS_REPORT set ";
                    oraclesqlstring = oraclesqlstring + "PATIENT_ID='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "EVENT_NO='" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "PATIENT_TYPE=3,";
                    oraclesqlstring = oraclesqlstring + "PACS_TYPE=5.00,";
                    oraclesqlstring = oraclesqlstring + "PATIENT_NAME='" + bljc.Rows[0]["F_xm"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "PATIENT_SEX='" + bljc.Rows[0]["F_xb"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "PATIENT_AGE='" + bljc.Rows[0]["F_nl"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "IN_DEPT='" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "BED='" + bljc.Rows[0]["F_ch"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "WARD='" + bljc.Rows[0]["F_bq"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "ADDRESS='" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "TELEPHONE=' ',";
                    oraclesqlstring = oraclesqlstring + "MARRIAGE='" + bljc.Rows[0]["F_hy"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "PROFESSION='" + bljc.Rows[0]["F_zy"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "CHECKNO='" + bljc.Rows[0]["F_blh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "BARCODE_ID=' ',";
                    oraclesqlstring = oraclesqlstring + "INSPECT_TYPE=' ',";
                    oraclesqlstring = oraclesqlstring + "INSPECT_SUB_TYPE=' ',";
                    oraclesqlstring = oraclesqlstring + "INSPECT_NAME='病理检查',";
                    oraclesqlstring = oraclesqlstring + "INSTRUMENT_NAME=' ',";
                    oraclesqlstring = oraclesqlstring + "INSPECT_CONTENT=:INSPECT_CONTENT,";
                    oraclesqlstring = oraclesqlstring + "APPLICANT='" + bljc.Rows[0]["F_sjys"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "OPERATER='" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "INSPECTOR='" + bljc.Rows[0]["F_shys"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "OPERATER_TIME=to_date('" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "','yyyy-MM-dd HH24:mi:ss'),";//检查日期取材日期
                    oraclesqlstring = oraclesqlstring + "REPORT_TIME=to_date('" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "','yyyy-MM-dd HH24:mi:ss'),";//检查日期取材日期
                    oraclesqlstring = oraclesqlstring + "LAST_MODIFY_TIME=to_date('" + bljc.Rows[0]["F_spare5"].ToString().Trim() + "','yyyy-MM-dd HH24:mi:ss'),";//检查日期取材日期
                    oraclesqlstring = oraclesqlstring + "MODIFY_FLAG=1,";
                    oraclesqlstring = oraclesqlstring + "REMARK1=' ',";
                    oraclesqlstring = oraclesqlstring + "REMARK2=' ',";
                    oraclesqlstring = oraclesqlstring + "STATUS=0";
                    oraclesqlstring = oraclesqlstring + " where PACS_NO = 'BL" + bljc.Rows[0]["F_blh"].ToString().Trim() + "' and PACS_TYPE='5.00'";
                }
                else
                {
                    oraclesqlstring = "insert into ZEMR_PACS_REPORT(PACS_NO,PATIENT_ID,EVENT_NO,PATIENT_TYPE,PACS_TYPE,PATIENT_NAME,PATIENT_SEX,PATIENT_AGE,IN_DEPT,BED,WARD,ADDRESS,TELEPHONE,MARRIAGE,PROFESSION,CHECKNO,BARCODE_ID,INSPECT_TYPE,INSPECT_SUB_TYPE,INSPECT_NAME,INSTRUMENT_NAME,INSPECT_CONTENT,APPLICANT,OPERATER,INSPECTOR,OPERATER_TIME,REPORT_TIME,LAST_MODIFY_TIME,MODIFY_FLAG,REMARK1,REMARK2,STATUS) values(";
                    oraclesqlstring = oraclesqlstring + "'BL" + bljc.Rows[0]["F_blh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "3,";
                    oraclesqlstring = oraclesqlstring + "5.00,";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_xm"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_xb"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_nl"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_ch"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_bq"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "' ',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_hy"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_zy"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_blh"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "' ',"; //条形码
                    oraclesqlstring = oraclesqlstring + "' ',";//检查类型
                    oraclesqlstring = oraclesqlstring + "' ',";//检查子类型
                    oraclesqlstring = oraclesqlstring + "'病理检查',";//检查名称
                    oraclesqlstring = oraclesqlstring + "' ',";//仪器名称
                    oraclesqlstring = oraclesqlstring + ":INSPECT_CONTENT,";//检查内容 
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_sjys"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "'" + bljc.Rows[0]["F_shys"].ToString().Trim() + "',";
                    oraclesqlstring = oraclesqlstring + "to_date('" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "','yyyy-MM-dd HH24:mi:ss'),";//检查日期取材日期
                    oraclesqlstring = oraclesqlstring + "to_date('" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "','yyyy-MM-dd HH24:mi:ss'),";
                    oraclesqlstring = oraclesqlstring + "to_date('" + bljc.Rows[0]["F_spare5"].ToString().Trim() + "','yyyy-MM-dd HH24:mi:ss'),";
                    oraclesqlstring = oraclesqlstring + "0,";
                    oraclesqlstring = oraclesqlstring + "' ',";
                    oraclesqlstring = oraclesqlstring + "' ',";
                    oraclesqlstring = oraclesqlstring + "0)";



                }
                cmd.CommandText = @oraclesqlstring;
                OracleParameter op1 = new OracleParameter();
                op1.ParameterName = "INSPECT_CONTENT";
                op1.OracleType = OracleType.Blob;
                op1.Size = blxmlb.Length;
                op1.Value = blxmlb;
                cmd.Parameters.Add(op1);


                cmd.ExecuteNonQuery();




            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                conn.Close();
            }

            if (zjbtx == "1") return;

            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
            DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");

            string stxsm = "";


            Image bgjpg;
            string jpgguid;
            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                string ftpstatus = "";
                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);

                if (ftpstatus == "Error")
                {
                    return;
                }
                bgjpg = Image.FromFile(ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim());
                jpgguid = System.Guid.NewGuid().ToString();

                MemoryStream ms = new MemoryStream();
                bgjpg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bgbyte = ms.ToArray();
                cmd.CommandType = CommandType.Text;

                try
                {
                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    conn.Open();

                    cmd.CommandText = "insert into ZEMR_LIS_PACS_REPORT_IMAGE (REPORT_NO,REPORT_TYPE,IMAGE_ID,IMAGE_NAME,IMAGE_CONTENT,COMPRESSION,REMARK) values (";
                    cmd.CommandText = cmd.CommandText + "'BL" + bljc.Rows[0]["F_blh"].ToString().Trim() + "',";
                    cmd.CommandText = cmd.CommandText + "'2',";
                    cmd.CommandText = cmd.CommandText + "'" + jpgguid + "',";
                    cmd.CommandText = cmd.CommandText + "'" + txlb.Rows[i]["F_txm"].ToString().Trim() + "',";
                    cmd.CommandText = cmd.CommandText + ":zp,";
                    cmd.CommandText = cmd.CommandText + "'0',";
                    cmd.CommandText = cmd.CommandText + "' ')";
                    cmd.Parameters.Add("zp", System.Data.OracleClient.OracleType.Blob, bgbyte.Length);
                    cmd.Parameters[0].Value = bgbyte;
                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    conn.Close();
                }

            }




        }

    }
}

