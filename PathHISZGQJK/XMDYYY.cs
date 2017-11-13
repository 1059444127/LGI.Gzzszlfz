
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
using System.Threading;
using ZgqClassPub;
namespace PathHISZGQJK
{
    //厦门第一医院 webservices+hl7
    class XMDYYY
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        int hczt = 0; int hcbg = 0;
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string dz)
        {
            bglx = bglx.ToLower();
            if (bglx == "")
                bglx = "cg";
            if (bgxh == "")
                bgxh = "1";

            string blbh = blh + bglx + bgxh;
            if(bglx=="cg")
              blbh = blh ;

            string yh = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            
            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
         
          
            //回写状态   客户端用
             hczt = f.ReadInteger("zgqjk", "hczt", 0);
             hcbg = f.ReadInteger("zgqjk", "hcbg", 0);

            int ptjk = f.ReadInteger("zgqjk", "ptjk", 0);
            int hisjk = f.ReadInteger("zgqjk", "hisjk", 0);
            int pacsjk = f.ReadInteger("zgqjk", "pacsjk", 0);
            int pdftopt = f.ReadInteger("zgqjk", "pdftopt", 1);
            int pdftopacs = f.ReadInteger("zgqjk", "pdftopacs", 1);


            string PTPDFPath = f.ReadString("zypt", "toPDFPath", @"\\192.0.19.75\ljgms").Replace("\0", "").Trim();
              
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                log.WriteMyLog(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
               log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                log.WriteMyLog("病理号有错误！");
                aa.ExecuteSQL("update T_JCXX_FS set F_fszt='不处理',F_BZ='病理号不存在'  where F_blbh='" + blbh + "'");
                return;
            }

            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
            if (dz == "qxsh")
                bgzt = "取消审核";


            if (jcxx.Rows[0]["F_SQXH"].ToString().Trim()=="")
            {
                log.WriteMyLog("无申请序号,不处理");
                ZgqClass.BGHJ(blh, yh, "保存", "无申请序号,不处理", "ZGQJK", "");
                aa.ExecuteSQL("update T_JCXX_FS set F_fszt='不处理',F_BZ='无申请序号不处理'  where F_blbh='" + blbh + "'");
                return;
            }

            if (bgzt == "取消审核")
            {
                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blbh='" + blbh + "'", "dt2");
                if (dt2.Rows.Count > 0)
                {
                    try
                    {
                        if (dt2.Rows[0]["F_ptpdflj"].ToString() != "")
                        {
                            //判断共享上是否存在该pdf文件
                            if (File.Exists(dt2.Rows[0]["F_ptpdflj"].ToString()))
                            {
                                //删除共享上的pdf文件
                                File.Delete(dt2.Rows[0]["F_ptpdflj"].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "' ");
                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blbh='" + blbh + "' and F_BGZT='取消审核'");
                }
            }

            if (hczt == 1 && bglx == "cg")
            {
                if (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已写报告" || bgzt == "报告延期" || bgzt == "取消审核")
                {
                    ///厦门智业平台接口，回传状态
                    if (ptjk == 1)
                    {
                        ZtToPt(blbh, blh, bgzt, jcxx, yh, yhbh, debug);
                    }
                    ///上传pacs
                    if (pacsjk == 1 && jcxx.Rows[0]["f_topacs"].ToString().Trim()=="")
                        BgToPacs(blh, bglx, bgxh, bgzt, jcxx, "", debug);
                    //老his接口
                    if (hisjk == 1)
                    {
                        xm1y xm1 = new xm1y();
                        xm1.pathtohis(blh, "");
                    }
                    return;
                }
             
            }

            //回写报告
           
            if (hcbg == 1)
            {
                string bz = "";
                DataTable dt_bc = new DataTable();
                DataTable dt_bd= new DataTable();
                if (bglx == "bc")
                {
                    dt_bc = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "'  and F_bc_bgxh='" + bgxh + "'", "bc");
                    if (dt_bc.Rows.Count <= 0)
                    {
                        log.WriteMyLog("无此补充报告"+blbh);
              
                        aa.ExecuteSQL("update T_JCXX_FS set F_fszt='不处理',F_BZ='无此补充报告"+blbh+"'  where F_blbh='" + blbh + "'");
                    }
                    try
                    {
                        bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString().Trim();
                    }
                    catch (Exception e1)
                    {
                        log.WriteMyLog("BC报告状态错误:" + e1.Message);
                       return;
                    }
                }
                if (bglx == "bd")
                {
                    dt_bd = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "'  and F_bd_bgxh='" + bgxh + "'", "bd");
                    if (dt_bd.Rows.Count <= 0)
                    {
                        log.WriteMyLog("无此冰冻报告" + blbh);

                        aa.ExecuteSQL("update T_JCXX_FS set F_fszt='不处理',F_BZ='无此冰冻报告" + blbh + "'  where F_blbh='" + blbh + "'");
                    }
                    try
                    {
                        bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString().Trim();
                    }
                    catch(Exception  e1)
                    {
                        log.WriteMyLog("BD报告状态错误:"+e1.Message);
                         return;
                    }
                }

                if (dz == "qxsh")
                    bgzt = "取消审核";

                #region  取消审核，删除pdf
                if (bgzt == "取消审核")
                {
                    DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blbh='" + blbh + "'", "dt2");
                    if (dt2.Rows.Count > 0)
                    {
                        try
                        {
                            if (dt2.Rows[0]["F_ptpdflj"].ToString() != "")
                            {
                                //判断共享上是否存在该pdf文件
                                if (File.Exists(dt2.Rows[0]["F_ptpdflj"].ToString()))
                                {
                                    //删除共享上的pdf文件
                                    File.Delete(dt2.Rows[0]["F_ptpdflj"].ToString());
                                }
                            }
                        }
                        catch
                        {
                        }
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "' ");
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blbh='" + blbh + "' and F_BGZT='取消审核'");
                    }
                }
                #endregion

                if (bgzt == "已审核")
                {
                    //上传结果
               
                        string PacspdfPath = "";
                        string PtPdfPath = "";
                        string ispdf = f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim();
                        if (ispdf == "1")
                        {
                            #region  生成pdf,上传pdf
                            if (bgzt.Trim() == "已审核")
                            {
                                ZgqPDFJPG zgq = new ZgqPDFJPG();
                                try
                                {
                                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                                    string pdfpath = "";
                                    string message = "";
                                    string jpgname = "";
                                    pdfpath = "";
                                    //生成pdf
                                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref message, ref jpgname);
                                    if (isrtn)
                                    {
                                        if (File.Exists(jpgname))
                                        {
                                            string ip = f.ReadString("zypt", "ip", "192.0.19.147").Replace("\0", "").Trim(); ;
                                            string userName = f.ReadString("zypt", "userName", "LJGMS").Replace("\0", "").Trim(); ;
                                            string pwd = f.ReadString("zypt", "pwd", "lj2012+zy15").Replace("\0", "").Trim(); ;
                                            string filepath = f.ReadString("zypt", "filepath", "gms").Replace("\0", "").Trim(); ;

                                            string jpgname2 = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                            //pdf上传病理服务器
                                            bool ssa = false;
                                          //  bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 3, ref pdfpath);
                                            for (int y = 0; y < 3; y++)
                                            {
                                               ssa= zgq.UpPDF(blh, jpgname, ML, ref message, 3, ref pdfpath);
                                                if (ssa)
                                                    break;
                                            }
                                            if (ssa)
                                            {
                                                if (debug == "1")
                                                    log.WriteMyLog("上传pdf成功:" + pdfpath);
                                                ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传pdf成功:" + pdfpath, "ZGQJK", "pdf");
                                            }
                                            else
                                            {
                                                log.WriteMyLog("上传pdf失败:" + message);
                                                ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传pdf成功:" + pdfpath, "ZGQJK", "ZGQJK");
                                            }
                                            aa.ExecuteSQL("insert  into T_BG_PDF(F_blbh,F_BLH,F_BGLX,F_BGXH,F_pdfname,F_pdfpath,F_ptpdflj) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + jpgname2 + "','" + pdfpath + "','" + PtPdfPath + "')");
                                            PtPdfPath = pdfpath.Replace("ftp", "http");
                                            //pdf上传平台存储
                                            if (pdftopt == 1)
                                            {
                                                ssa = false;
                                                for (int y = 0; y < 3; y++)
                                                {
                                                    ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 2, ref PtPdfPath, ref pdfpath, "zypt");
                                               
                                                    if (ssa)
                                                        break;
                                                }
                                                if (ssa)
                                                {
                                                    if (debug == "1")
                                                        log.WriteMyLog("上传pdf到平台成功:" + pdfpath);
                                                    PtPdfPath = pdfpath;
                                                    ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传pdf到平台成功", "ZGQJK", "ZGQJK");
                                                }
                                                else
                                                {
                                                    log.WriteMyLog("上传pdf到平台失败:" + message);
                                                    ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传pdf到平台失败:" + message, "ZGQJK", "ZGQJK");
                                                }
                                            }

                                            ///pdf上传岱嘉医技平台
                                            if (pdftopacs==1)
                                            {
                                                ssa = false;
                                             
                                                if (pacsjk == 1)
                                                {
                                                    #region  生成并上传pdf到服务器

                                                    string ftpml = DateTime.Now.ToString("yyyyMMdd");

                                                    for (int y = 0; y < 3; y++)
                                                    {
                                                        ssa = zgq.UpPDF("", jpgname, ftpml, ref message, 3, ref PacspdfPath, ref pdfpath, "djyxpt");
                                                        if (ssa)
                                                            break;
                                                    }
                                                    if (ssa)
                                                    {
                                                        //FileInfo fi = new FileInfo(jpgname);
                                                        //  PacspdfPath = "PIS" + "//" + ftpml + "//" + fi.Name;

                                                        PacspdfPath = PacspdfPath;
                                                        if (debug == "1")
                                                            log.WriteMyLog("上传PDF到PACS成功：" + PacspdfPath);
                                                        ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传pdf到PACS成功:" + PacspdfPath, "ZGQJK", "ZGQJK");
                                                    }
                                                    else
                                                    {
                                                        log.WriteMyLog("上传PDF到PACS失败:" + message);
                                                        ZgqClass.BGHJ(blh, "ZGQJK", "审核", "PDF上传PACS失败:" + message, "ZGQJK", "ZGQJK");
                                                    }
                                                     #endregion
                                                }
                                            }

                                        }
                                        else
                                        {
                                            log.WriteMyLog("未找到PDF文件:" + jpgname);
                                            ZgqClass.BGHJ(blh, "ZGQJK", "审核", "未找到PDF文件:" + jpgname, "ZGQJK", "ZGQJK");
                                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='未找到PDF文件:" + jpgname + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                                            zgq.DelTempFile(blh);
                                            return;
                                        }
                                           
                                    }
                                    else
                                    {
                                        log.WriteMyLog("生成PDF文件失败:" + message);
                                        ZgqClass.BGHJ(blh, "ZGQJK", "审核", "生成PDF文件失败:" + message, "ZGQJK", "ZGQJK");
                                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='生成pdf失败：" + message + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                                        zgq.DelTempFile(blh);
                                        return;
                                    }
                                    zgq.DelTempFile(blh);
                                }
                                catch(Exception   ee3)
                                {
                                    log.WriteMyLog("生成PDF文件异常:" + ee3.Message);
                                    ZgqClass.BGHJ(blh, "ZGQJK", "审核", "生成PDF文件异常:" + ee3.Message, "ZGQJK", "ZGQJK");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='生成PDF文件异常:" + ee3.Message + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                                    zgq.DelTempFile(blh);
                                }
                            }
                           
                            # endregion
                        }

                        ///上传平台
                        if (ptjk == 1)
                            BgToPt(blbh, blh, bglx, bgxh, bgzt, jcxx, dt_bc, dt_bd, PtPdfPath, debug);

                        ///上传pacs
                        if( pacsjk==1 && bglx=="cg")
                            BgToPacs(blh, bglx, bgxh, bgzt, jcxx, PacspdfPath, debug);
                        return;
                    }
            }
                return;
        }


        public void ZtToPt(string blbh,string blh, string bgzt, DataTable dt_jcxx, string yh, string yhbh, string debug)
        {
            try
            {
                #region  回状态(不包括审核)
                if (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已写报告" || bgzt == "报告延期" || bgzt == "取消审核")
                {

                    string bgzt_1 = "";
                    if (bgzt == "已登记" || bgzt == "已取材")
                        bgzt_1 = "S";

                    if (bgzt == "已写报告" || bgzt == "报告延期")
                        bgzt_1 = "R";

                    if (bgzt == "已审核")
                        bgzt_1 = "F";

                    if (bgzt == "取消审核")
                        bgzt_1 = "C";

                    if (bgzt_1 == "")
                    {
                        log.WriteMyLog("bgzt_1状态为空，不回写！");
                        return;
                    }

                    string brbh = dt_jcxx.Rows[0]["F_BRBH"].ToString().Trim();
                    string brlb = dt_jcxx.Rows[0]["F_brlb"].ToString().Trim();
                    string sqxh = dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim();
                    if (brlb == "住院") brlb = "I";
                    else brlb = "O";

                    string ZYH = dt_jcxx.Rows[0]["F_YZID"].ToString().Trim();
                    string SFZH = dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim();
                    string XM = dt_jcxx.Rows[0]["F_XM"].ToString().Trim();
                    string SJKS = dt_jcxx.Rows[0]["F_BQ"].ToString().Trim();
                    string CH = dt_jcxx.Rows[0]["F_CH"].ToString().Trim();
                    string YZXM = dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim();

                    string xtdm = f.ReadString("savetohis", "xtdm", "2060000").Replace("\0", "").Trim();
                    string certificate = f.ReadString("savetohis", "certificate", "5lYdPpiVdi0CxHKEhy3kqbzNlsXgNKZb").Replace("\0", "").Trim();
                    string tjtxpath = f.ReadString("savetohis", "toPDFPath", @"\\192.0.19.147\GMS").Replace("\0", "").Trim(); ;

                    string shys = dt_jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                    string shysbh = getyhgh(shys);

                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                              + "PID|||" + dt_jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"

                              + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                + "ORC|SC|" + sqxh + "|||RC|||||||" + yhbh + "^" + yh + "\r"
                                + "OBR||" + sqxh + "|" + blbh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;

                    if (debug == "1")
                        log.WriteMyLog(blh + "-修改报告状态入参：" + ChangeGmsApplyStatus_Hl7);

                    string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);

                    if (debug == "1")
                        log.WriteMyLog(blh + "-修改状态平台返回：" + rtn_msg2);

                    if (rtn_msg2.Trim() == "-1")
                    {
                        log.WriteMyLog(blh + "-[" + bgzt + "]回写状态失败：" + rtn_msg2);
                        ZgqClass.BGHJ(blh, yh, "ZGQJK", "[" + bgzt + "]回写状态失败:-1", "ZGQJK", "ZGQJK");
                        return;
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
                            ZgqClass.BGHJ(blh, yh, "ZGQJK", "[" + bgzt + "]回写状态成功:" + r7.MSA[3].Trim(), "ZGQJK", "ZGQJK");
                        }
                        else
                        {
                            log.WriteMyLog("回写报告状态失败：" + r7.MSA[3].Trim());
                            ZgqClass.BGHJ(blh, yh, "ZGQJK", "[" + bgzt + "]回写状态失败:" + r7.MSA[3].Trim(), "ZGQJK", "ZGQJK");
                        }
                    }

                }

                #endregion
            }
            catch (Exception e2)
            {
                log.WriteMyLog("回写报告状态异常：" + e2.Message);
                ZgqClass.BGHJ(blh, yh, "ZGQJK", "[" + bgzt + "]回写状态异常:" + e2.Message, "ZGQJK", "ZGQJK");
            }
        }
        public void BgToPt(string blbh,string blh, string bglx, string bgxh, string bgzt, DataTable jcxx, DataTable dt_bc, DataTable dt_bd, string PtPdfPath, string debug)
        {
            try
            {
                string txwebpath = ZgqClass.GetSz("ZGQJK", "txwebpath", @"http://192.0.1.75/pathimages").Replace("\0", "").Trim();
                string brbh = jcxx.Rows[0]["F_BRBH"].ToString().Trim();
                string brlb = jcxx.Rows[0]["F_brlb"].ToString().Trim();
                string sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim();
                if (brlb == "住院") brlb = "I";
                else brlb = "O";

                string ZYH = jcxx.Rows[0]["F_YZID"].ToString().Trim();
                string SFZH = jcxx.Rows[0]["F_SFZH"].ToString().Trim();
                string XM = jcxx.Rows[0]["F_XM"].ToString().Trim();
                string SJKS = jcxx.Rows[0]["F_BQ"].ToString().Trim();
                string CH = jcxx.Rows[0]["F_CH"].ToString().Trim();
                string YZXM = jcxx.Rows[0]["F_YZXM"].ToString().Trim();

                string xtdm = f.ReadString("zypt", "xtdm", "2060000").Replace("\0", "").Trim();
                string certificate = f.ReadString("zypt", "certificate", "5lYdPpiVdi0CxHKEhy3kqbzNlsXgNKZb").Replace("\0", "").Trim();


                string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                string shysbh = getyhgh(shys);
                if (bglx == "cg" && bgzt == "已审核")
                {
                    #region 回写审核状态
                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                  + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"

                                  + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                    + "ORC|SC|" + sqxh + "|" + blbh + "||RC|||||||" + shysbh + "^" + shys + "\r"
                                    + "OBR||" + sqxh + "|" + blbh + "|" + YZXM + "|||||||||||||||||||||F";

                    if (debug == "1")
                        log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-修改审核状态入参：" + ChangeGmsApplyStatus_Hl7);

                    string rtn_msg_zt = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);

                    if (debug == "1")
                       log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-修改审核状态平台返回：" + rtn_msg_zt);

                    if (rtn_msg_zt.Trim() == "-1")
                    {
                        ZgqClass.BGHJ(blh, shys, "ZGQJK", "回写审核状态失败:-1", "ZGQJK", "ZGQJK");
                        log.WriteMyLog(blh + ",回写审核状态失败：" + rtn_msg_zt);
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写审核状态失败'  where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                        //  return;
                    }
                    else
                    {
                        readhl7_fjfy r7 = new readhl7_fjfy();
                        int xy = 0;
                        r7.Adt01(rtn_msg_zt, ref xy);
                        if (r7.MSA[1].Trim() == "AA")
                        {
                            if (debug == "1")
                               log.WriteMyLog(r7.MSA[3].Trim());
                            ZgqClass.BGHJ(blh, shys, "ZGQJK", "回写审核状态成功:" + r7.MSA[3].Trim(), "ZGQJK", "ZGQJK");


                        }
                        else
                        {
                            ZgqClass.BGHJ(blh, shys, "ZGQJK", "回写审核状态失败:" + r7.MSA[3].Trim(), "ZGQJK", "ZGQJK");
                            log.WriteMyLog("回写审核状态失败：" + r7.MSA[3].Trim());
                        }
                    }
                    #endregion
                }
                if (bgzt == "已审核")
                {
                    #region 回传报告

                    //TBS报告
                    string bggs = jcxx.Rows[0]["F_BGGS"].ToString().Trim();
                    string TBSSJ = "";
                    string rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim();
                    string xjsj = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                    string blzd = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                    string tsjc = jcxx.Rows[0]["F_tsjc"].ToString().Trim();

                    string jcfd = jcxx.Rows[0]["F_BLK"].ToString().Trim();
                    if (bggs == "TBS")
                    {
                        DataTable DT_TBS = new DataTable();
                        DT_TBS = aa.GetDataTable("select * from T_TBS_BG where F_blh='" + blh + "'", "jcxx");
                        if (DT_TBS.Rows.Count > 0)
                        {
                            TBSSJ = "抹片分析：" + DT_TBS.Rows[0]["F_TBS_MPFX"].ToString().Trim() + "\r\n";
                            TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";
                            TBSSJ = "细胞项目：" + DT_TBS.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\r\n";
                            TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n";
                            TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\r\n";

                            TBSSJ = "\r\n微生物项目：" + DT_TBS.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\r\n";
                            TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n";
                            TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n";

                            TBSSJ = "\r\n病毒项目：" + DT_TBS.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";
                            TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_BDXM2"].ToString().Trim() + "\r\n";

                            TBSSJ = "\r\n抹片质数：" + DT_TBS.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "";
                            TBSSJ = "\r\n炎症细胞/遮盖比率*：：" + DT_TBS.Rows[0]["F_TBS_ZGBL"].ToString().Trim() + "";

                            rysj = TBSSJ;
                            blzd = DT_TBS.Rows[0]["F_TBSZD"].ToString().Trim();
                            jcfd = DT_TBS.Rows[0]["F_TBS_JYFF"].ToString().Trim();
                        }


                    }

                    // string PtPdfPath = tjtxpath + "\\" + jpgpath + "\\" + jpgname;
                    string bgys = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                    string qcys = jcxx.Rows[0]["F_QCYS"].ToString().Trim();
                    string qcrq = jcxx.Rows[0]["F_QCrq"].ToString().Trim();
                    string bgrq = "";//DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                    string shrq = ""; //DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");

                    try
                    {
                        qcrq = DateTime.Parse(qcrq).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (bglx == "cg")
                    {
                        bgrq = DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        shrq = DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                    }
                    if (bglx == "bc")
                    {
                        bgys = dt_bc.Rows[0]["F_bc_BGYS"].ToString().Trim();
                        shys = dt_bc.Rows[0]["F_bc_SHYS"].ToString().Trim();
                        bgrq = DateTime.Parse(dt_bc.Rows[0]["F_bc_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        shrq = DateTime.Parse(dt_bc.Rows[0]["F_bc_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim();
                        xjsj = dt_bc.Rows[0]["F_bc_jxsj"].ToString().Trim();
                        blzd = dt_bc.Rows[0]["F_bczd"].ToString().Trim();
                        tsjc = dt_bc.Rows[0]["F_bc_tsjc"].ToString().Trim();
                    }
                    if (bglx == "bd")
                    {
                        bgys = dt_bd.Rows[0]["F_bd_BGYS"].ToString().Trim();
                        shys = dt_bd.Rows[0]["F_bd_SHYS"].ToString().Trim();
                        bgrq = DateTime.Parse(dt_bd.Rows[0]["F_bd_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        shrq = bgrq;
                        rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim();
                        xjsj = "";
                        blzd = dt_bd.Rows[0]["F_bdzd"].ToString().Trim();
                        tsjc = "";
                    }
                    string bgysgh = getyhgh(bgys);
                    string shysgh = getyhgh(shys);
                    string qcysgh = getyhgh(qcys);
                    string hxbj = jcxx.Rows[0]["F_HXBJ"].ToString().Trim();

                    string zt2 = "F";
                    if (hxbj == "1")
                        zt2 = "C";

                    string xb = jcxx.Rows[0]["F_XB"].ToString().Trim();
                    if (xb == "女") xb = "F";
                    else if (xb.Trim() == "男") xb = "M";
                    else xb = "U";


                    string SendGmsReport_hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORU^R01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
        + "PID|||" + brbh + "^^^^PI~" + ZYH + "^^^^VN||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||^" + jcxx.Rows[0]["F_nl"].ToString().Trim() + "|" + xb + "|||||||" + "\r"
        + "PV1||" + brlb + "|" + SJKS + "^^" + CH + "||||^||||||||||||" + ZYH + "|||||||||||||||||||||||||" + "\r"

        + "OBR||" + sqxh + "|" + blbh + "|" + YZXM + "||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|||" + qcysgh + "^" + qcys
        + "|" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "||||^^^&" + jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "|^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim()
        + "||||||" + bgrq + "|||||||||^|" + bgysgh + "&" + bgys + "^^" + "||||||^" + jcxx.Rows[0]["F_JSY"].ToString().Trim() + "||2060000^病理科||||||1^~6^" + jcxx.Rows[0]["F_LCZD"].ToString().Trim().Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "\r"



        + "NTE|1||" + PtPdfPath + "|Z-RP" + "\r"
        + "OBX|1|FT|^肉眼所见||" + (rysj.Trim()).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + "\r"
        + "OBX|2|FT| ^镜下所见||" + (xjsj).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcfd + "|" + "\r"
        + "OBX|3|FT| ^特殊检查||" + (tsjc).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcfd + "|" + "\r"
        + "OBX|4|FT| ^病理诊断||" + (blzd).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcfd + "|" + "\r";

                    DataTable dt_tx = aa.GetDataTable("select * from V_DYTX where F_blh='" + blh + "'", "tx");
                    if (dt_tx.Rows.Count > 0)
                    {
                        for (int x = 0; x < dt_tx.Rows.Count; x++)
                        {
                            string txlj = txwebpath + "/" + jcxx.Rows[0]["F_TXML"].ToString().Trim() + "/" + dt_tx.Rows[x]["F_TXM"].ToString();
                            SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|" + (x + 1).ToString() + "|" + dt_tx.Rows[x]["F_ID"].ToString() + "|" + blbh + "|" + txlj + "||1" + "\r";
                        }
                    }
                    else
                    {
                        SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|1|1|" + blbh + "|||" + "\r";
                    }
                    if (debug == "1")
                        log.WriteMyLog("回写结果:" + SendGmsReport_hl7);


                    string rtn_msg2 = rtn_CallInterface("HL7v2", "SendGmsReport", SendGmsReport_hl7, "", certificate);
                    if (rtn_msg2 == "" || rtn_msg2.Contains("Error"))
                    {
                       log.WriteMyLog("回写结果失败:" + rtn_msg2);

                        ZgqClass.BGHJ(blh, "ZGQJK", "审核", "回写结果失败：" + rtn_msg2, "ZGQJK", "ZGQJK");
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写结果失败:" + rtn_msg2 + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                        return;
                    }


                    if (rtn_msg2.Contains("error"))
                    {

                       log.WriteMyLog("回写结果错误:" + rtn_msg2);

                        ZgqClass.BGHJ(blh, "ZGQJK", "审核", "回写结果错误:" + rtn_msg2, "ZGQJK", "ZGQJK");
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='平台返回：" + rtn_msg2 + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核'");
                        return;
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
                                ZgqClass.BGHJ(blh, "ZGQJK", "审核", "回写结果成功:" + r7.MSA[3].Trim(), "ZGQJK", "ZGQJK");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写结果成功',F_FSZT='已处理'  where F_blbh='" + blbh + "' and F_BGZT='已审核' ");
                            }
                            else
                            {
                                log.WriteMyLog("回写结果失败：" + r7.MSA[3].Trim());
                                ZgqClass.BGHJ(blh, "ZGQJK", "审核", "回写结果失败:" + r7.MSA[3].Trim(), "ZGQJK", "ZGQJK");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写结果失败:" + r7.MSA[3].Trim() + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核' ");

                                return;
                            }
                       
                    }

                    #endregion
                }
            }
            catch(Exception  ee3)
            {
                log.WriteMyLog("回写结果到平台异常:" + ee3.Message);
                ZgqClass.BGHJ(blh, "ZGQJK", "审核", "回写结果到平台异常:" + ee3.Message, "ZGQJK", "ZGQJK");
                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写结果到平台异常:" + ee3.Message + "'  where F_blbh='" + blbh + "' and F_BGZT='已审核' ");
                return;
            }
        }

   
         
        public void BgToPacs(string blh, string bglx, string bgxh,string bgzt,DataTable dt_jcxx,string ftplj, string debug)
        {

            if (dt_jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
                return;

            if (bgzt != "已登记" && bgzt != "已取材" && bgzt != "已审核")
            {
                return;
            }

            string F_blh ="BL"+blh ;

            if(debug=="1")
                log.WriteMyLog("[PACS]开始上传报告toPACS");
            try
            {
                //审核后上传报告
                if (dt_jcxx.Rows[0]["F_ZYH"].ToString().Trim() == "" && dt_jcxx.Rows[0]["F_MZH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("[PACS]住院号和门诊号为空，不处理");
                    return;
                }

                
                bool pdftoyxpt = false;
                if (ftplj != "")
                    pdftoyxpt = true;

                string hospitalid = f.ReadString("djyxpt", "hospitalid", "2.16.840.1.113883.4.487.2.2");
                string pisid = f.ReadString("djyxpt", "pisid", "2.16.840.1.113883.4.487.2.2.8");
                string CUSTOM4 = f.ReadString("djyxpt", "CUSTOM4", "2.16.840.1.113883.4.487.5.6.4");
                string NATIONALITY_DOMAIN = f.ReadString("djyxpt", "NATIONALITY_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.16");
                string MARITAL_DOMAIN = f.ReadString("djyxpt", "MARITAL_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.6");
                string DEGREE_DOMAIN = f.ReadString("djyxpt", "DEGREE_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.7");
                string IDENTIFIER_FLOW_DOMAIN_ID = f.ReadString("djyxpt", "IDENTIFIER_FLOW_DOMAIN_ID", "2.16.840.1.113883.4.487.2.2.8.2");
                string GENDER_DOMAIN = f.ReadString("djyxpt", "GENDER_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.5");
                string PAT_CATEGORY_SYSTEM = f.ReadString("djyxpt", "PAT_CATEGORY_SYSTEM", "2.16.840.1.113883.4.487.5.6.1.1.9");
                string ETHNIC_DOMAIN = f.ReadString("djyxpt", "ETHNIC_DOMAIN", "2.16.840.1.113883.4.487.2.2.1.1.17");

                string hisname = f.ReadString("djyxpt", "hisname", "XMDYHIUP").Replace("\0", "");
                string user = f.ReadString("djyxpt", "USER", "ats_gate_pis").Replace("\0", "");
                string passwd = f.ReadString("djyxpt", "PASSWD", "dyyylj2017").Replace("\0", "");
                string ConnectionString = "Data Source=XMDYHIUP;User ID=ats_gate_pis;Password=dyyylj2017;";//写连接串

                string yhmc = f.ReadString("yh", "yhmc", "XDYHIUP").Replace("\0", "");
                string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "");

                string brbh = "";
                string brlb = "";
                string brbhlb = pisid;
                brbh = dt_jcxx.Rows[0]["F_brbh"].ToString().Trim();
                string F_SQXH =  dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim();
                if (dt_jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                {
                    brlb = "1";
                }
                else
                    if (dt_jcxx.Rows[0]["F_brlb"].ToString().Trim() == "体检")
                    {
                        brlb = "3";
                    }
                else
                {
                    brlb = "0";
                }

             
                        try
                        {
                            string uuid = Guid.NewGuid().ToString();
                            string nl = dt_jcxx.Rows[0]["F_nl"].ToString().Trim();
                            try
                            {
                                string nldw = nl.Substring(nl.Length - 1, 1);
                                DateTime d1 = DateTime.Now;
                                try
                                {
                                    d1 = Convert.ToDateTime(dt_jcxx.Rows[0]["F_sdrq"].ToString().Trim());
                                }
                                catch
                                {
                                }
                                if (nldw == "月")
                                {
                                    try
                                    {
                                        d1 = d1.AddMonths(0 - Convert.ToInt16(dt_jcxx.Rows[0]["F_age"].ToString().Trim()));
                                    }
                                    catch
                                    {
                                       log.WriteMyLog("计算年龄失败，位置1，age为：" + dt_jcxx.Rows[0]["F_age"].ToString().Trim());

                                    }
                                    nl = d1.ToString("yyyy-MM-") + "01";

                                }

                                else if (nldw == "天")
                                {
                                    try
                                    {
                                        d1 = d1.AddDays(0 - Convert.ToInt16(dt_jcxx.Rows[0]["F_age"].ToString().Trim()));
                                    }
                                    catch
                                    {
                                       log.WriteMyLog("计算年龄失败，位置2，age为：" + dt_jcxx.Rows[0]["F_age"].ToString().Trim());

                                    }
                                    nl = d1.ToString("yyyy-MM-dd");
                                }
                                else if (nldw == "岁")
                                {
                                    if (dt_jcxx.Rows[0]["F_SFZH"] != null && dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim() != "")
                                    {
                                        string csrq = dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim();
                                        if (csrq.Length == 15)
                                        {
                                            csrq = "19" + csrq.Substring(6, 6);
                                        }
                                        else
                                        {
                                            csrq = csrq.Substring(6, 8);
                                        }
                                        nl = csrq.Substring(0, 4) + "-" + csrq.Substring(4, 2) + "-" + csrq.Substring(6, 2);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            d1 = d1.AddYears(0 - Convert.ToInt16(dt_jcxx.Rows[0]["F_age"].ToString().Trim()));
                                        }
                                        catch
                                        {
                                           log.WriteMyLog("计算年龄失败，位置3，age为：" + dt_jcxx.Rows[0]["F_age"].ToString().Trim());

                                        }
                                        nl = d1.ToString("yyyy") + "-01-01";
                                    }

                                }
                                else
                                {
                                    if (dt_jcxx.Rows[0]["F_SFZH"] != null && dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim() != "")
                                    {
                                        string csrq = dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim();
                                        if (csrq.Length == 15)
                                        {
                                            csrq = "19" + csrq.Substring(6, 6);
                                        }
                                        else
                                        {
                                            csrq = csrq.Substring(6, 8);
                                        }
                                        nl = csrq.Substring(0, 4) + "-" + csrq.Substring(4, 2) + "-" + csrq.Substring(6, 2);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            d1 = d1.AddYears(0 - Convert.ToInt16(dt_jcxx.Rows[0]["F_age"].ToString().Trim()));
                                        }
                                        catch
                                        {
                                           log.WriteMyLog("计算年龄失败，位置4，age为：" + dt_jcxx.Rows[0]["F_age"].ToString().Trim());

                                        }
                                        nl = d1.ToString("yyyy") + "-01-01";
                                    }
                                }
                            }
                            catch
                            {
                               log.WriteMyLog("计算年龄失败");
                            }
                            string xb = "U";
                            if (dt_jcxx.Rows[0]["F_xb"].ToString().Trim() == "男")
                            {
                                xb = "M";
                            }
                            if (dt_jcxx.Rows[0]["F_xb"].ToString().Trim() == "女")
                            {
                                xb = "F";
                            }

                            string hy = "";
                            if (dt_jcxx.Rows[0]["F_HY"].ToString().Trim() == "已婚")
                            {
                                hy = "M";
                            }
                            else
                            {
                                hy = "S";
                            }
                            string hylb = "2.16.840.1.113883.4.487.3.4.1.1.6";

                            string mz = "0";
                            switch (dt_jcxx.Rows[0]["F_MZ"].ToString().Trim())
                            {
                                case "汉族":
                                    mz = "1";
                                    break;
                                case "回族":
                                    mz = "2";
                                    break;
                                case "朝鲜族":
                                    mz = "3";
                                    break;
                                case "满族":
                                    mz = "4";
                                    break;
                                case "高山族":
                                    mz = "5";
                                    break;
                                default:
                                    mz = "0";
                                    break;
                            }

                            string bgys = dt_jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                            string shys = dt_jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                            string bgysgh = getyhgh(bgys);
                            string shysgh = getyhgh(shys);
                            if (brlb == "1")
                            {
                                IDENTIFIER_FLOW_DOMAIN_ID = "2.16.840.1.113883.4.487.2.2.4.4";
                            }
                            else
                                if (brlb == "3")
                                {
                                    IDENTIFIER_FLOW_DOMAIN_ID = "2.16.840.1.113883.4.487.2.2.4.6";
                                }
                                else
                                {
                                    IDENTIFIER_FLOW_DOMAIN_ID = "2.16.840.1.113883.4.487.2.2.4.2";
                                }


                            OracleConnection conn = new OracleConnection(ConnectionString);
                            OracleCommand OCMD = new OracleCommand();
                            OCMD.Connection = conn;

                            string psinsert = "01";

                            #region PERSON
                            try
                            {
                                OCMD.CommandText = "select * from PERSON where  IDENTIFIER_DOMAIN_ID='" + pisid + "'   and  IDENTIFIER_ID='" + F_blh + "'";
                                OracleDataAdapter oda = new OracleDataAdapter(OCMD);
                                DataTable dt1 = new DataTable();
                                oda.Fill(dt1);
                                if (dt1.Rows.Count > 0)
                                {
                                    psinsert = "02";
                                }
                            }
                            catch (Exception ex)
                            {
                               log.WriteMyLog("[PACS][PERSON]" + ex.Message);
                            }
                 

                            string sqlstring = "";
                            if (psinsert == "01")
                            {
                                //写入
                                sqlstring = "insert into PERSON (NAME,DATE_OF_BIRTH,IDENTITY_NO,GENDER_CD,GENDER_DOMAIN,ETHNIC_GROUP_CD,"
                            +"ETHNIC_NAME,ETHNIC_DOMAIN,NATIONALITY_CD,NATIONALITY_NAME,NATIONALITY_DOMAIN,MARITAL_STATUS_CD,MARITAL_STATUS_NAME,"
                            +"MARITAL_DOMAIN,DEGREE,DEGREE_DOMAIN,DATE_CREATED,HOSPITAL_DOMAIN_ID,IDENTIFIER_DOMAIN_NAME,IDENTIFIER_DOMAIN_ID,"
                            + "IDENTIFIER_DOMAIN_TYPE,IDENTIFIER_ID,PERSON_STATUS,UUID,VIP,CUSTOM4,CUSTOM14,RELEVANCE_ID,RELEVANCE_DOMAIN,PROFESSION_DOMAIN) values (";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_XM"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "" + "to_date('" + nl + "', 'yyyy-MM-dd')" + ",";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'" + xb + "',";
                                //sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.1.1.1.5',";
                                sqlstring = sqlstring + "'" + GENDER_DOMAIN + "',";
                                sqlstring = sqlstring + "'" + mz + "',";

                                sqlstring = sqlstring + "'" +"汉族" + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.1.1.17" + "',";
                                sqlstring = sqlstring + "'0',";
                                sqlstring = sqlstring + "'中国',";
                                sqlstring = sqlstring + "'" + NATIONALITY_DOMAIN + "',";
                                sqlstring = sqlstring + "'" + hy + "',";
                                string hyname = dt_jcxx.Rows[0]["F_hy"].ToString().Trim() == "已婚" ? "已婚" : "未婚";
                                sqlstring = sqlstring + "'" + hyname + "',";
                                sqlstring = sqlstring + "'" + MARITAL_DOMAIN + "',";
                                sqlstring = sqlstring + "'0',";
                                sqlstring = sqlstring + "'" + DEGREE_DOMAIN + "',";
                                sqlstring = sqlstring + "" + "sysdate,";
                                sqlstring = sqlstring + "'" + hospitalid + "',";
                                //2.16.840.1.113883.4.487.5.6
                                sqlstring = sqlstring + "'PIS',";
                                sqlstring = sqlstring + "'" + pisid + "',";


                                //2.16.840.1.113883.4.487.5.6.10
                                sqlstring = sqlstring + "'ISO',";
                                sqlstring = sqlstring + "'" + F_blh + "',";
                                sqlstring = sqlstring + "'" + psinsert + "',";
                                sqlstring = sqlstring + "'" + uuid + "',";

                                sqlstring = sqlstring + "'1',";
                                if (brlb == "1")
                                {
                                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.4.3',";
                                    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_zyh"].ToString().Trim() + "',";
                                  //  sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.3',";
                                    //sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_zyh"].ToString().Trim() + "'";
                                }
                                else
                                    if (brlb == "3")
                                    {
                                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.4.5',";
                                        sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_MZH"].ToString().Trim() + "',";
                                        //  sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.3',";
                                       // sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_zyh"].ToString().Trim() + "'";
                                    }
                                else
                                {
                                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.4.1',";
                                    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_MZH"].ToString().Trim() + "',";
                                  //  sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.1',";
                                   // sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_mzh"].ToString().Trim() + "'";
                                }

                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_YZID"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.8.2" + "',";
                             //   sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.1.1.8" + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.1.1.8" + "'";
                                sqlstring = sqlstring + ")";
                            }
                            else
                            {
                                //更新
                                //写入
                                sqlstring = "update  PERSON  set NAME='" + dt_jcxx.Rows[0]["F_XM"].ToString().Trim()
                                    + "',IDENTITY_NO='" + dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim() + "',GENDER_CD='" + xb
                                    + "',GENDER_DOMAIN='" + GENDER_DOMAIN + "',ETHNIC_DOMAIN='" + ETHNIC_DOMAIN
                                    + "',NATIONALITY_DOMAIN='" + NATIONALITY_DOMAIN + "'  where IDENTIFIER_ID='" + F_blh + "'";
                            }

                            if (debug == "1")
                            {
                               log.WriteMyLog("写入PERSON表语句：" + sqlstring);
                            }
                            OCMD.CommandText = sqlstring;
                            try
                            {
                                conn.Open();
                               int y= OCMD.ExecuteNonQuery();
                                if(y>0)
                                    log.WriteMyLog("写入PERSON表成功");
                                else
                                    log.WriteMyLog("写入PERSON表失败");
                            }
                            catch (Exception ex)
                            {
                                log.WriteMyLog("写入PERSON表异常：" + ex.Message);
                            }
                            finally
                            {
                                conn.Close();
                            }
                            #endregion

                            #region PATIENT_VISIT

                              psinsert = "01";
                            try
                            {
                                OCMD.CommandText = "select * from PATIENT_VISIT where  IDENTIFIER_DOMAIN_ID='" + pisid + "' and PATIENT_ID='" + F_blh + "'";
                                OracleDataAdapter oda = new OracleDataAdapter(OCMD);
                                DataTable dt1 = new DataTable();
                                oda.Fill(dt1);
                                if (dt1.Rows.Count > 0)
                                {
                                    psinsert = "02";
                                }
                            }
                            catch (Exception ex)
                            {
                                log.WriteMyLog("查询PATIENT_VISIT异常：" + ex.Message);
                            }



                            if (psinsert == "01")
                            {
                                sqlstring = string.Format("insert into PATIENT_VISIT (PATIENT_ID,VISIT_FLOW_ID,NAME,DATE_OF_BIRTH,GENDER_CD,GENDER_NAME,"//6
                                    + "GENDER_DOMAIN,MARITAL_STATUS,HOSPITAL_DOMAIN_ID,IDENTIFIER_DOMAIN_NAME,IDENTIFIER_DOMAIN_ID,IDENTIFIER_DOMAIN_TYPE,"//6
                                    + "IDENTIFIER_FLOW_DOMAIN_NAME,IDENTIFIER_FLOW_DOMAIN_ID,IDENTIFIER_FLOW_DOMAIN_TYPE,PAT_CATEGORY,PAT_CATEGORY_SYSTEM,"//5  
                                    + "PATIENT_FLOW_ID,CREATE_DATE,UUID,IDENTITY_NO,PAT_VIP,ADMISSION_DOMAIN,OLD_STATUS,PAT_RATE,PATIENT_CLASS,PAT_CATEGORY_NAME,"//10
                                    + "PAT_CURRENT_ROOM,PAT_CURRENT_BED,PAT_CUURENT_DEP,PAT_DETER_BED) values ('{0}','{1}','{2}',{3},'{4}','{5}',"
                                    + "'{6}','{7}','{8}','{9}','{10}','{11}',"
                                    + "'{12}','{13}','{14}','{15}','{16}',"
                                    + "'{17}',sysdate,'{18}','{19}','{20}','{21}','0','0.5','{22}','{23}',"
                                    + "'{24}','{25}','{26}','{27}')",
                                    F_blh, F_SQXH, dt_jcxx.Rows[0]["F_xm"].ToString().Trim(),"to_date('" + nl + "', 'yyyy-MM-dd')", xb, dt_jcxx.Rows[0]["F_xb"].ToString().Trim(),
                                    GENDER_DOMAIN, hy, hospitalid,"PIS", pisid, "ISO",
                                    "PIS-JYLS", "2.16.840.1.113883.4.487.2.2.8.2", "ISO", brlb, PAT_CATEGORY_SYSTEM,
                                    dt_jcxx.Rows[0]["F_yzid"].ToString().Trim(),uuid, dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim(),
                                    "0", "2.16.840.1.113883.4.487.2.2.1.1.10", brlb, dt_jcxx.Rows[0]["F_BRLB"].ToString().Trim(),
                                    dt_jcxx.Rows[0]["F_BQ"].ToString().Trim(), dt_jcxx.Rows[0]["F_CH"].ToString().Trim(),dt_jcxx.Rows[0]["F_SJKS"].ToString().Trim(), dt_jcxx.Rows[0]["F_BQ"].ToString().Trim());

                            }
                            else
                            {
                                sqlstring = string.Format("update PATIENT_VISIT set VISIT_FLOW_ID='{1}',NAME='{2}',DATE_OF_BIRTH={3},GENDER_CD='{4}',GENDER_NAME='{5}',"

                                      + "IDENTIFIER_FLOW_DOMAIN_ID='{6}',PAT_CATEGORY='{7}',PAT_CATEGORY_SYSTEM='{8}',PATIENT_FLOW_ID='{9}',"
                                      + "UUID='{10}',IDENTITY_NO='{11}',PATIENT_CLASS='{12}',PAT_CATEGORY_NAME='{13}',"
                                      + "PAT_CURRENT_ROOM='{14}',PAT_CURRENT_BED='{15}',PAT_CUURENT_DEP='{16}',PAT_DETER_BED='{17}'  where PATIENT_ID='{0}'", F_blh,
                                        F_SQXH, dt_jcxx.Rows[0]["F_xm"].ToString().Trim(), "to_date('" + nl + "', 'yyyy-MM-dd')", xb, dt_jcxx.Rows[0]["F_xb"].ToString().Trim(),
                                       "2.16.840.1.113883.4.487.2.2.8.2", brlb, PAT_CATEGORY_SYSTEM, dt_jcxx.Rows[0]["F_yzid"].ToString().Trim(), 
                                       uuid, dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim(), brlb, dt_jcxx.Rows[0]["F_BRLB"].ToString().Trim(), 
                                      dt_jcxx.Rows[0]["F_BQ"].ToString().Trim(), dt_jcxx.Rows[0]["F_CH"].ToString().Trim(),dt_jcxx.Rows[0]["F_SJKS"].ToString().Trim(), dt_jcxx.Rows[0]["F_BQ"].ToString().Trim());
                            }
                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入PATIENT_VISIT表语句：" + sqlstring);
                                }
                                OCMD.CommandText = sqlstring;
                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入PATIENT_VISIT表成功");
                                    else
                                        log.WriteMyLog("插入PATIENT_VISIT表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入PATIENT_VISIT异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                           
                            
                            #endregion

                            string bgztbm="BL05";
                            if (hczt == 1 && bgzt != "已审核")
                            {
                               
                                #region 状态写入SGATE_STATUS_INFO表
                                string ztinsert = "ADD";
                                //try
                                //{
                                //    OCMD.CommandText = "select * from SGATE_STATUS_INFO where  DOCUMENT_DOMAIN_ID='" + pisid + "'   and  PATIENT_ID='" + F_blh + "'";
                                //    OracleDataAdapter oda = new OracleDataAdapter(OCMD);
                                //    DataTable dt1 = new DataTable();
                                //    oda.Fill(dt1);
                                //    if (dt1.Rows.Count > 0)
                                //    {
                                //        ztinsert = "UPDATE";
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    log.WriteMyLog("[PACS][SGATE_STATUS_INFO]" + ex.Message);
                                //}

                                sqlstring = "insert into SGATE_STATUS_INFO(PATIENT_ID,PATIENT_DOMAIN_ID,PAT_NAME,PAT_CATEGORY,PAT_CATEGORY_SYSTEM,DOCUMENT_NAME,"
                                + "DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PAY_LOAD_TYPE,SUB_TYPE,REQUEST_NUMBER,REQUEST_DOMAIN,ORDER_NUMBER,ORDER_DOMAIN,"
                                + "EVENT_STATUS,EVENT_TIME,EVENT_OPERATOR_ID,EVENT_OPERATOR_NAME,EVENT_CREATE_TIME,DCM_STATUS,DICOM_STUDY_TIME,DCM_MODALITY,EVENT_HIUP_STATUS) values(";
                                sqlstring = sqlstring + "'" + F_blh + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.8" + "',";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_XM"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'" + brlb + "',";
                                sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.1.1.9',";
                                sqlstring = sqlstring + "'病理报告单',";

                                sqlstring = sqlstring + "'" + F_SQXH + "',";
                                sqlstring = sqlstring + "'" + pisid + "',";
                                sqlstring = sqlstring + "'XDS.PISBG',";
                                sqlstring = sqlstring + "'" + ztinsert + "',";
                                sqlstring = sqlstring + "'" + F_SQXH + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.4.9" + "',";
                                sqlstring = sqlstring + "'" + "" + "',";
                                sqlstring = sqlstring + "'" + "" + "',";

                                sqlstring = sqlstring + "'BL05',";
                                sqlstring = sqlstring + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd hh24:mi:ss')" + ",";
                                sqlstring = sqlstring + "'" + yhbh + "',";
                                sqlstring = sqlstring + "'" + yhmc + "',";
                                sqlstring = sqlstring + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-MM-dd hh24:mi:ss')" + ",";
                                sqlstring = sqlstring + "0,";
                                sqlstring = sqlstring + "'" + DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "'" + ",";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_BLK"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'0')";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入SGATE_STATUS_INFO表语句：" + sqlstring);
                                }

                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();

                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入SGATE_STATUS_INFO表成功");
                                    else
                                        log.WriteMyLog("插入SGATE_STATUS_INFO表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入SGATE_STATUS_INFO表异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                #endregion

                                #region  查询SGATE_STATUS_INFO
                                OCMD.CommandText = "select PK from SGATE_STATUS_INFO where  DOCUMENT_DOMAIN_ID='" + pisid + "'   and  PATIENT_ID='" + F_blh + "' order by  EVENT_CREATE_TIME desc ";
                                DataTable dt_SGATE_STATUS_INFO = new DataTable();
                                OracleDataAdapter odaa = new OracleDataAdapter(OCMD);

                                int pk = 0;
                                odaa.Fill(dt_SGATE_STATUS_INFO);
                                if (dt_SGATE_STATUS_INFO.Rows.Count > 0)
                                {
                                    try
                                    {
                                        pk = int.Parse(dt_SGATE_STATUS_INFO.Rows[0]["PK"].ToString());
                                    }
                                    catch
                                    {
                                    }

                                }
                                #endregion
                                if (pk == 0)
                                   return ;

                                #region 写入SGATE_EXTEND_ID_INFO表
                                sqlstring = "insert into SGATE_EXTEND_ID_INFO(STATUS_FK,ID,DOMAIN_ID) values(";
                                sqlstring = sqlstring + "" + pk + ",";
                                sqlstring = sqlstring + "'" + F_SQXH + "',";
                                sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.8.2')";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                                }
                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();

                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表成功");
                                    else
                                        log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                #endregion

                                aa.ExecuteSQL("update T_JCXX  set f_topacs='1' where F_blh='" + blh + "'");
                                return;
                            }

                            if (bgzt == "已审核"&& hcbg==1)
                            {

                                #region 审核状态写入SGATE_STATUS_INFO表
                                string ztinsert = "ADD";
                                //try
                                //{
                                //    OCMD.CommandText = "select * from SGATE_STATUS_INFO where  DOCUMENT_DOMAIN_ID='" + pisid + "'   and  PATIENT_ID='" + F_blh + "'";
                                //    OracleDataAdapter oda = new OracleDataAdapter(OCMD);
                                //    DataTable dt1 = new DataTable();
                                //    oda.Fill(dt1);
                                //    if (dt1.Rows.Count > 0)
                                //    {
                                //        ztinsert = "UPDATE";
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    log.WriteMyLog("[PACS][SGATE_STATUS_INFO]" + ex.Message);
                                //}

                                sqlstring = "insert into SGATE_STATUS_INFO(PATIENT_ID,PATIENT_DOMAIN_ID,PAT_NAME,PAT_CATEGORY,PAT_CATEGORY_SYSTEM,DOCUMENT_NAME,"
                                + "DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PAY_LOAD_TYPE,SUB_TYPE,REQUEST_NUMBER,REQUEST_DOMAIN,ORDER_NUMBER,ORDER_DOMAIN,"
                                + "EVENT_STATUS,EVENT_TIME,EVENT_OPERATOR_ID,EVENT_OPERATOR_NAME,EVENT_CREATE_TIME,DCM_STATUS,DICOM_STUDY_TIME,DCM_MODALITY,EVENT_HIUP_STATUS) values(";//DICOM_STUDY_TIME
                                sqlstring = sqlstring + "'" + F_blh + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.8" + "',";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_XM"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'" + brlb + "',";
                                sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.1.1.9',";
                                sqlstring = sqlstring + "'病理报告单',";

                                sqlstring = sqlstring + "'" + F_SQXH + "',";
                                sqlstring = sqlstring + "'" + pisid + "',";
                                sqlstring = sqlstring + "'XDS.PISBG',";
                                sqlstring = sqlstring + "'" + ztinsert + "',";
                                sqlstring = sqlstring + "'" + F_SQXH + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.4.9" + "',";
                                sqlstring = sqlstring + "'" + "" + "',";
                                sqlstring = sqlstring + "'" + "" + "',";

                                sqlstring = sqlstring + "'BL03',";
                                sqlstring = sqlstring + "to_date('" + DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-MM-dd hh24:mi:ss')" + ",";
                                sqlstring = sqlstring + "'" + getyhgh(dt_jcxx.Rows[0]["F_SHYS"].ToString().Trim()) + "',";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', 'yyyy-MM-dd hh24:mi:ss')" + ",";
                                sqlstring = sqlstring + "0,";
                                sqlstring = sqlstring + "'" + DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "',";
                                sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_BLK"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'0')";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入SGATE_STATUS_INFO表语句：" + sqlstring);
                                }

                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();

                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入SGATE_STATUS_INFO表成功");
                                    else
                                        log.WriteMyLog("插入SGATE_STATUS_INFO表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入SGATE_STATUS_INFO表异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                #endregion

                                #region  查询SGATE_STATUS_INFO
                                OCMD.CommandText = "select PK from SGATE_STATUS_INFO where  DOCUMENT_DOMAIN_ID='" + pisid + "'   and  PATIENT_ID='" + F_blh + "' order by  EVENT_CREATE_TIME desc ";
                                DataTable dt_SGATE_STATUS_INFO = new DataTable();
                                OracleDataAdapter odaa = new OracleDataAdapter(OCMD);

                                int pk = 0;
                                odaa.Fill(dt_SGATE_STATUS_INFO);
                                if (dt_SGATE_STATUS_INFO.Rows.Count > 0)
                                {
                                    try
                                    {
                                        pk = int.Parse(dt_SGATE_STATUS_INFO.Rows[0]["PK"].ToString());
                                    }
                                    catch
                                    {
                                    }

                                }
                                #endregion
                                if (pk == 0)
                                    goto DOCUMENT;

                                #region 写入SGATE_EXTEND_ID_INFO表
                                sqlstring = "insert into SGATE_EXTEND_ID_INFO(STATUS_FK,ID,DOMAIN_ID) values(";
                                sqlstring = sqlstring + "" + pk + ",";
                                sqlstring = sqlstring + "'" + F_SQXH + "',";
                                sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.8.2')";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                                }
                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();

                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表成功");
                                    else
                                        log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入SGATE_EXTEND_ID_INFO表异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                #endregion
                               
                                DOCUMENT:

                                #region DGATE_DOCUMENT_INFO

                                string bginsert = "ADD";
                                string pdfbginsert = "ADD";
                                string xmlpkid = "";
                                string pdfpkid = "";
                                try
                                {

                                    OCMD.CommandText = "select * from DGATE_DOCUMENT_INFO where PATIENT_ID='" + F_blh + "' and DOCUMENT_DOMAIN_ID='" + pisid + "' and DOCUMENT_UNIQUE_ID='" + F_SQXH + "' ";

                                    OracleDataAdapter oda = new OracleDataAdapter(OCMD);
                                    DataTable dt1 = new DataTable();
                                    oda.Fill(dt1);
                                    if (dt1.Rows.Count > 0)
                                    {
                                  
                                        for (int i = 0; i < dt1.Rows.Count; i++)
                                        {
                                            if (debug == "1")
                                            {
                                                log.WriteMyLog("FILE_TYPE=" + dt1.Rows[i]["FILE_TYPE"].ToString() + ",PK为" + dt1.Rows[i]["PK"].ToString());
                                            }
                                            if (dt1.Rows[i]["FILE_TYPE"].ToString() == "PATH-PDF")
                                            {
                                                pdfpkid = dt1.Rows[i]["PK"].ToString().Trim();
                                                pdfbginsert = "UPDATE";
                                            }
                                            else
                                            {
                                                xmlpkid = dt1.Rows[i]["PK"].ToString().Trim();
                                                bginsert = "UPDATE";
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("查询DGATE_DOCUMENT_INFO表异常：" + ex.Message);
                                }
                                finally
                                {

                                }
                                #endregion

                                #region 写入DGATE_DOCUMENT_INFO表
                                int bgid = 0;
                                #region 生成XML

                                string xml = "<?xml version='1.0' standalone='yes'?><LOGENE>";
                                xml = xml + "<F_BLK>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BLK"].ToString().Trim()) + "</F_BLK>";
                                xml = xml + "<F_BLH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BLH"].ToString().Trim()) + "</F_BLH>";
                                //if (dt_jcxx.Rows[0]["F_SJKS"].ToString().Contains("消化"))
                                //{
                                //    xml = xml + "<F_EISBH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_MZ"].ToString().Trim()) + "</F_EISBH>";
                                //}
                                //else
                                //{
                                //    xml = xml + "<F_EISBH></F_EISBH>";
                                //}
                                xml = xml + "<F_BRBH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BLH"].ToString().Trim()) + "</F_BRBH>";
                                xml = xml + "<F_SQXH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim()) + "</F_SQXH>";
                                xml = xml + "<F_YZID>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YZID"].ToString().Trim()) + "</F_YZID>";
                                xml = xml + "<F_YZXM>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim()) + "</F_YZXM>";
                                xml = xml + "<F_STUDY_UID>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_STUDY_UID"].ToString().Trim()) + "</F_STUDY_UID>";
                                xml = xml + "<F_XM>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_XM"].ToString().Trim()) + "</F_XM>";
                                xml = xml + "<F_XB_CODE>" + xb + "</F_XB_CODE>";
                                xml = xml + "<F_XB>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_XB"].ToString().Trim()) + "</F_XB>";
                                xml = xml + "<F_XB_CODESYSTEM>2.16.840.1.113883.4.487.3.4.1.1.5</F_XB_CODESYSTEM>";
                                xml = xml + "<F_NL>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_NL"].ToString().Trim()) + "</F_NL>";
                                xml = xml + "<F_AGE>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_AGE"].ToString().Trim()) + "</F_AGE>";
                                xml = xml + "<F_HY_CODE>" + hy + "</F_HY_CODE>";
                                xml = xml + "<F_HY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_HY"].ToString().Trim()) + "</F_HY>";
                                xml = xml + "<F_HY_CODESYSTEM>" + hylb + "</F_HY_CODESYSTEM>";
                                xml = xml + "<F_MZ_CODE></F_MZ_CODE>";
                                xml = xml + "<F_MZ></F_MZ>";
                                xml = xml + "<F_MZ_CODESYSTEM></F_MZ_CODESYSTEM>";
                                xml = xml + "<F_ZY_CODE></F_ZY_CODE>";
                                xml = xml + "<F_ZY></F_ZY>";
                                xml = xml + "<F_ZY_CODESYSTEM></F_ZY_CODESYSTEM>";
                                xml = xml + "<F_SFZH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFZH"].ToString().Trim()) + "</F_SFZH>";
                                xml = xml + "<F_LXXX>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_LXXX"].ToString().Trim()) + "</F_LXXX>";
                                xml = xml + "<F_BRLB_CODE>" + brlb + "</F_BRLB_CODE>";
                                xml = xml + "<F_BRLB>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BRLB"].ToString().Trim()) + "</F_BRLB>";
                                xml = xml + "<F_BRLB_CODESYSTEM>2.16.840.1.113883.4.487.3.4.1.1.9</F_BRLB_CODESYSTEM>";
                                xml = xml + "<F_FB_CODE></F_FB_CODE>";
                                xml = xml + "<F_FB>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_FB"].ToString().Trim()) + "</F_FB>";
                                xml = xml + "<F_FB_CODESYSTEM>2.16.840.1.113883.4.487.3.4.1.1.12</F_FB_CODESYSTEM>";
                                xml = xml + "<F_ZYH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_ZYH"].ToString().Trim()) + "</F_ZYH>";
                                xml = xml + "<F_MZH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_MZH"].ToString().Trim()) + "</F_MZH>";
                                xml = xml + "<F_BQ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BQ"].ToString().Trim()) + "</F_BQ>";
                                xml = xml + "<F_SJKS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SJKS"].ToString().Trim()) + "</F_SJKS>";
                                xml = xml + "<F_CH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_CH"].ToString().Trim()) + "</F_CH>";
                                xml = xml + "<F_SJDW>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SJDW"].ToString().Trim()) + "</F_SJDW>";
                                xml = xml + "<F_SJYS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SJYS"].ToString().Trim()) + "</F_SJYS>";
                                string sdrq = "";
                                try
                                {
                                    sdrq = Convert.ToDateTime(dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    sdrq = "";
                                }
                                xml = xml + "<F_SDRQ>" + System.Security.SecurityElement.Escape(sdrq) + "</F_SDRQ>";
                                xml = xml + "<F_JSY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JSY"].ToString().Trim()) + "</F_JSY>";
                                xml = xml + "<F_BBLX>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BBLX"].ToString().Trim()) + "</F_BBLX>";
                                xml = xml + "<F_BBQK>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BBQK"].ToString().Trim()) + "</F_BBQK>";
                                xml = xml + "<F_JSYY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JSYY"].ToString().Trim()) + "</F_JSYY>";
                                xml = xml + "<F_SF>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SF"].ToString().Trim()) + "</F_SF>";
                                xml = xml + "<F_BBMC>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BBMC"].ToString().Trim()) + "</F_BBMC>";
                                xml = xml + "<F_LCZD>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_LCZD"].ToString().Trim()) + "</F_LCZD>";
                                xml = xml + "<F_LCZL>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_LCZL"].ToString().Trim()) + "</F_LCZL>";
                                xml = xml + "<F_RYSJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_RYSJ"].ToString().Trim()) + "</F_RYSJ>";
                                xml = xml + "<F_QCYS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_QCYS"].ToString().Trim()) + "</F_QCYS>";
                                string qcrq = "";
                                try
                                {
                                    qcrq = Convert.ToDateTime(dt_jcxx.Rows[0]["F_QCRQ"] == null ? DateTime.Now.ToLongDateString() : dt_jcxx.Rows[0]["F_QCRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    qcrq = "";
                                }
                                xml = xml + "<F_QCRQ>" + System.Security.SecurityElement.Escape(qcrq) + "</F_QCRQ>";
                                xml = xml + "<F_JLY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JLY"].ToString().Trim()) + "</F_JLY>";
                                xml = xml + "<F_LKZS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_LKZS"].ToString().Trim()) + "</F_LKZS>";
                                xml = xml + "<F_CKZS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_CKZS"].ToString().Trim()) + "</F_CKZS>";
                                xml = xml + "<F_FY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_FY"].ToString().Trim()) + "</F_FY>";
                                xml = xml + "<F_JXSJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JXSJ"].ToString().Trim()) + "</F_JXSJ>";
                                xml = xml + "<F_BLZD>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BLZD"].ToString().Trim()) + "</F_BLZD>";
                                xml = xml + "<F_TSJC>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_TSJC"].ToString().Trim()) + "</F_TSJC>";
                                xml = xml + "<F_BGYS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGYS"].ToString().Trim()) + "</F_BGYS>";
                                xml = xml + "<F_SHYS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SHYS"].ToString().Trim()) + "</F_SHYS>";
                                string bgrq = "";
                                try
                                {
                                    bgrq = Convert.ToDateTime(dt_jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    bgrq = "";
                                }
                                xml = xml + "<F_BGRQ>" + System.Security.SecurityElement.Escape(bgrq) + "</F_BGRQ>";
                                xml = xml + "<F_CZYJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_CZYJ"].ToString().Trim()) + "</F_CZYJ>";
                                xml = xml + "<F_XGYJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_XGYJ"].ToString().Trim()) + "</F_XGYJ>";
                                xml = xml + "<F_ZDGJC>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_ZDGJC"].ToString().Trim()) + "</F_ZDGJC>";
                                xml = xml + "<F_YYX>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YYX"].ToString().Trim()) + "</F_YYX>";
                                xml = xml + "<F_WFBGYY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_WFBGYY"].ToString().Trim()) + "</F_WFBGYY>";
                                xml = xml + "<F_BZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BZ"].ToString().Trim()) + "</F_BZ>";
                                xml = xml + "<F_BD_SFFH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BD_SFFH"].ToString().Trim()) + "</F_BD_SFFH>";
                                xml = xml + "<F_BGZT>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim()) + "</F_BGZT>";
                                xml = xml + "<F_SFCT>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFCT"].ToString().Trim()) + "</F_SFCT>";
                                xml = xml + "<F_SFDY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFDY"].ToString().Trim()) + "</F_SFDY>";
                                xml = xml + "<F_BGGS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGGS"].ToString().Trim()) + "</F_BGGS>";
                                xml = xml + "<F_GDZT>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_GDZT"].ToString().Trim()) + "</F_GDZT>";
                                xml = xml + "<F_KNHZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_KNHZ"].ToString().Trim()) + "</F_KNHZ>";
                                xml = xml + "<F_ZJYJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_ZJYJ"].ToString().Trim()) + "</F_ZJYJ>";
                                xml = xml + "<F_WYYJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_WYYJ"].ToString().Trim()) + "</F_WYYJ>";
                                xml = xml + "<F_SFZT>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFZT"].ToString().Trim()) + "</F_SFZT>";
                                xml = xml + "<F_SFJG>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFJG"].ToString().Trim()) + "</F_SFJG>";
                                xml = xml + "<F_JBBM_CN>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JBBM_CN"].ToString().Trim()) + "</F_JBBM_CN>";
                                xml = xml + "<F_JBBM_ENG>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JBBM_ENG"].ToString().Trim()) + "</F_JBBM_ENG>";
                                xml = xml + "<F_JBMC>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_JBMC"].ToString().Trim()) + "</F_JBMC>";
                                xml = xml + "<F_YBLH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YBLH"].ToString().Trim()) + "</F_YBLH>";
                                xml = xml + "<F_SJCL>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SJCL"].ToString().Trim()) + "</F_SJCL>";
                                xml = xml + "<F_YBLZD>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YBLZD"].ToString().Trim()) + "</F_YBLZD>";
                                xml = xml + "<F_BGFSFS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGFSFS"].ToString().Trim()) + "</F_BGFSFS>";
                                xml = xml + "<F_SCYS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SCYS"].ToString().Trim()) + "</F_SCYS>";
                                xml = xml + "<F_SFFH>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFFH"].ToString().Trim()) + "</F_SFFH>";
                                xml = xml + "<F_SPARE1>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE1"].ToString().Trim()) + "</F_SPARE1>";
                                xml = xml + "<F_SPARE2>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE2"].ToString().Trim()) + "</F_SPARE2>";
                                xml = xml + "<F_SPARE3>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE3"].ToString().Trim()) + "</F_SPARE3>";
                                xml = xml + "<F_SPARE4>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE4"].ToString().Trim()) + "</F_SPARE4>";
                                string spare5 = "";
                                try
                                {
                                    spare5 = Convert.ToDateTime(dt_jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    spare5 = "";
                                }
                                xml = xml + "<F_SPARE5>" + System.Security.SecurityElement.Escape(spare5) + "</F_SPARE5>";
                                xml = xml + "<F_SPARE6>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE6"].ToString().Trim()) + "</F_SPARE6>";
                                string spare7 = "";
                                try
                                {
                                    spare7 = Convert.ToDateTime(dt_jcxx.Rows[0]["F_SPARE7"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    spare7 = "";
                                }
                                xml = xml + "<F_SPARE7>" + System.Security.SecurityElement.Escape(spare7) + "</F_SPARE7>";
                                xml = xml + "<F_SPARE8>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE8"].ToString().Trim()) + "</F_SPARE8>";
                                xml = xml + "<F_SPARE9>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE9"].ToString().Trim()) + "</F_SPARE9>";
                                xml = xml + "<F_SPARE10>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SPARE10"].ToString().Trim()) + "</F_SPARE10>";
                                xml = xml + "<F_BY1>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BY1"].ToString().Trim()) + "</F_BY1>";
                                xml = xml + "<F_BY2>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BY2"].ToString().Trim()) + "</F_BY2>";
                                xml = xml + "<F_TXML>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_TXML"].ToString().Trim()) + "</F_TXML>";
                                xml = xml + "<F_ZPZT>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_ZPZT"].ToString().Trim()) + "</F_ZPZT>";
                                xml = xml + "<F_MCYJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_MCYJ"].ToString().Trim()) + "</F_MCYJ>";
                                xml = xml + "<F_SFJJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_SFJJ"].ToString().Trim()) + "</F_SFJJ>";
                                xml = xml + "<F_TBSID>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_TBSID"].ToString().Trim()) + "</F_TBSID>";
                                xml = xml + "<F_TBSMC>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_TBSMC"].ToString().Trim()) + "</F_TBSMC>";
                                xml = xml + "<F_QSB_DYZT>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_QSB_DYZT"].ToString().Trim()) + "</F_QSB_DYZT>";
                                xml = xml + "<F_BGWZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGWZ"].ToString().Trim()) + "</F_BGWZ>";
                                xml = xml + "<F_BGWZ_QRSJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGWZ_QRSJ"].ToString().Trim()) + "</F_BGWZ_QRSJ>";
                                xml = xml + "<F_BGWZ_QRCZY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGWZ_QRCZY"].ToString().Trim()) + "</F_BGWZ_QRCZY>";
                                xml = xml + "<F_BBWZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BBWZ"].ToString().Trim()) + "</F_BBWZ>";
                                xml = xml + "<F_LKWZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_LKWZ"].ToString().Trim()) + "</F_LKWZ>";
                                xml = xml + "<F_QPWZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_QPWZ"].ToString().Trim()) + "</F_QPWZ>";
                                xml = xml + "<F_GDCZY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_GDCZY"].ToString().Trim()) + "</F_GDCZY>";
                                xml = xml + "<F_GDSJ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_GDSJ"].ToString().Trim()) + "</F_GDSJ>";
                                xml = xml + "<F_GDBZ>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_GDBZ"].ToString().Trim()) + "</F_GDBZ>";
                                xml = xml + "<F_BGLRY>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_BGLRY"].ToString().Trim()) + "</F_BGLRY>";
                                xml = xml + "<F_FZYS>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_FZYS"].ToString().Trim()) + "</F_FZYS>";
                                xml = xml + "<F_YL1>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL1"].ToString().Trim()) + "</F_YL1>";
                                xml = xml + "<F_YL2>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL2"].ToString().Trim()) + "</F_YL2>";
                                xml = xml + "<F_YL3>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL3"].ToString().Trim()) + "</F_YL3>";
                                xml = xml + "<F_YL4>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL4"].ToString().Trim()) + "</F_YL4>";
                                xml = xml + "<F_YL5>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL5"].ToString().Trim()) + "</F_YL5>";
                                xml = xml + "<F_YL6>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL6"].ToString().Trim()) + "</F_YL6>";
                                xml = xml + "<F_YL7>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL7"].ToString().Trim()) + "</F_YL7>";
                                xml = xml + "<F_YL8>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL8"].ToString().Trim()) + "</F_YL8>";
                                xml = xml + "<F_YL9>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL9"].ToString().Trim()) + "</F_YL9>";
                                xml = xml + "<F_YL10>" + System.Security.SecurityElement.Escape(dt_jcxx.Rows[0]["F_YL10"].ToString().Trim()) + "</F_YL10>";
                                xml = xml + "<F_304></F_304>";
                                xml = xml + "<F_sjc></F_sjc>";
                                xml = xml + "<F_JCXM></F_JCXM>";
                                xml = xml + "<F_Z></F_Z>";
                                xml = xml + "<F_TBLX></F_TBLX>";
                                xml = xml + "<F_JCJG></F_JCJG>";
                                xml = xml + "<F_CKWX></F_CKWX>";
                                xml = xml + "<F_STS></F_STS>";
                                xml = xml + "<F_QY></F_QY>";

                                xml = xml + "<ImageList>";
                                #region 图像处理
                                string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                                string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                                string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                                string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                                string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                                string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                                FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                                string txml = dt_jcxx.Rows[0]["F_txml"].ToString().Trim();
                                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                                //上传XML

                                //下载图像
                                DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");
                                try
                                {
                                    if (ftps == "1")//FTP下载方式
                                    {
                                        int i2 = 1;
                                        for (int i = 0; i < txlb.Rows.Count; i++)
                                        {
                                            i2 = i + 1;
                                            string ftpstatus = "";

                                            if (debug == "1")
                                            {
                                               log.WriteMyLog("开始下载图像");
                                            }
                                            try
                                            {
                                                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                            }
                                            catch
                                            {
                                            }
                                            if (!File.Exists(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim()))
                                            {
                                                log.WriteMyLog("FTP下载图像出错！");
                                            }
                                            else
                                            {
                                                FileStream file = new FileStream(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim(), FileMode.Open, FileAccess.Read);
                                                Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                                                file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

                                                file.Close();
                                                xml = xml + "<Image>" + Convert.ToBase64String(imgByte) + "</Image>";
                                            }

                                            try
                                            {
                                                File.Delete(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim());
                                            }
                                            catch
                                            {
                                                log.WriteMyLog("删除图片失败");
                                            }
                                        }

                                    }
                                    else//共享下载方式
                                    {
                                        if (txpath == "")
                                        {
                                            log.WriteMyLog("sz.ini txpath图像目录未设置");
                                            return;
                                        }
                                        int i2 = 1;
                                        for (int i = 0; i < txlb.Rows.Count; i++)
                                        {
                                            i2 = i + 1;
                                            string ftpstatus = "";
                                            try
                                            {
                                                File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                                                //FTP上传图像
                                                FileStream file = new FileStream(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim(), FileMode.Open, FileAccess.Read);
                                                Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                                                file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

                                                file.Close();
                                                xml = xml + "<Image>" + Convert.ToBase64String(imgByte) + "</Image>";
                                            }
                                            catch
                                            {
                                                log.WriteMyLog("共享目录不存在！");
                                                return;
                                            }

                                            try
                                            {
                                                File.Delete(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim());
                                            }
                                            catch
                                            {
                                                log.WriteMyLog("删除图片失败");
                                            }
                                        }
                                    }
                                }
                                catch (Exception EX)
                                {

                                    log.WriteMyLog(EX.Message);
                                }
                                #endregion
                                xml = xml + "</ImageList>";
                                xml = xml + "</LOGENE>";
                                #endregion

                                if (xmlpkid=="")
                                {
                                    bginsert = "ADD";
                                }
                                    sqlstring = "INSERT INTO DGATE_DOCUMENT_INFO(DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PATIENT_ID,PATIENT_DOMAIN_ID,"
                                    + "REQUEST_NUMBER,FILE_TYPE,PAY_LOAD_TYPE,SUB_TYPE,CONTENT,START_TIME,EFFECTIVE_TIME,PATIENT_TYPE,END_TIME,"
                                    + "ASSIGNED_PERSON,DOC_NAME,ADMIT_TIME,HIS_TYPE,BED_NO,ASSIGNED_CODE,AUTHOR_CODE,AUTHOR_NAME,"
                                    + "ITEM_NAME,REQUEST_DOMAIN,TPOS_PATH,PAT_CATEGORY_SYSTEM,PAT_CATEGORY,PAT_NAME,ACCESSIONNUM,BODY_PART,"
                                    + "DIAGNOSIS_METHOD_CODE,DICOM_STUDY_TIME,DICOM_NUM,MODALITY,FILE_SYSTEM_FK)values(";
                                    sqlstring = sqlstring + "'" + F_SQXH + "',";
                                    sqlstring = sqlstring + "'" + pisid + "',";
                                    sqlstring = sqlstring + "'" + F_blh + "',";
                                    sqlstring = sqlstring + "'" + pisid + "',";

                                    sqlstring = sqlstring + "'" + F_SQXH + "',";
                                    sqlstring = sqlstring + "'TRANS-XML',";
                                    sqlstring = sqlstring + "'XDS.PISBG',";
                                    sqlstring = sqlstring + "'" + bginsert + "',";
                                    sqlstring = sqlstring + ":xml,";
                                    sqlstring = sqlstring + "" + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')" + ",";
                                    sqlstring = sqlstring + "" + "to_date('" + dt_jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                                    sqlstring = sqlstring + "'" + brlb + "',";
                                    sqlstring = sqlstring + "" + "to_date('" + dt_jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS'),";

                                    sqlstring = sqlstring + "'" + shys + "',";
                                    sqlstring = sqlstring + "'病理诊断报告',";
                                    sqlstring = sqlstring + "to_date('" + dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                                    sqlstring = sqlstring + "'" + brlb + "',";
                                    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_CH"].ToString().Trim() + "',";
                                    sqlstring = sqlstring + "'" + shysgh + "',";
                                    sqlstring = sqlstring + "'" + bgysgh + "',";
                                    sqlstring = sqlstring + "'" + bgys + "',";

                                    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "',";
                                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.4.9',";
                                    sqlstring = sqlstring + "'" + "" + "',";
                                    sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.1.1.9" + "',";
                                    sqlstring = sqlstring + "'" + brlb + "',";
                                    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_XM"].ToString().Trim() + "',";
                                    sqlstring = sqlstring + "'" + F_SQXH+ "',";
                                    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "',";

                                    sqlstring = sqlstring + "'病理报告',";
                                    sqlstring = sqlstring + "to_date('" + dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim() + "','yyyy-MM-dd HH24:MI:SS'),";
                                    sqlstring = sqlstring + "'" + brbh + "',";
                                    sqlstring = sqlstring + "'病理报告','1')";
                               

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入DGATE_DOCUMENT_INFO表语句：" + sqlstring);
                                }

                                OCMD.CommandText = sqlstring;
                                OracleParameter op = new OracleParameter("xml", OracleType.Clob);

                                log.WriteMyLog("xml" + xml);
                                op.Value = xml;
                                OCMD.Parameters.Add(op);
                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入DGATE_DOCUMENT_INFO表成功");
                                    else
                                        log.WriteMyLog("插入DGATE_DOCUMENT_INFO表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入DGATE_DOCUMENT_INFO表异常" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }

                                #endregion

                               
                                 #region DGATE_DOCUMENT_INFO
                                string sqltxt = "select * from DGATE_DOCUMENT_INFO where PATIENT_ID='" + F_blh + "' and DOCUMENT_DOMAIN_ID='" + pisid + "' and DOCUMENT_UNIQUE_ID='" + F_SQXH + "' and FILE_TYPE='TRANS-XML' order by PK desc";
                                 
                                 try
                                 {
                                     xmlpkid = "";
                                      pdfpkid = "";
                                     OracleDataAdapter oda = new OracleDataAdapter(sqltxt, conn);
                                     DataTable dt1 = new DataTable();
                                     oda.Fill(dt1);
                                     if (dt1.Rows.Count > 0)
                                     {
                                         xmlpkid = dt1.Rows[0]["PK"].ToString().Trim();
                                     }

                                 }
                                 catch (Exception ex)
                                 {
                                     log.WriteMyLog("查询DGATE_DOCUMENT_INFO表异常：" + ex.Message + "   " + sqltxt);
                                 }
                                 finally
                                 {

                                 }
                                 #endregion

                                #region 写入DGATE_EXTEND_ID_INFO表
                                sqlstring = "insert into DGATE_EXTEND_ID_INFO (DOCUMENT_FK,ID,DOMAIN_ID) values(";
                                //  sqlstring = sqlstring + "DGATE_EXTEND_ID_INFO_SEQUENCE.nextval,";
                                sqlstring = sqlstring + xmlpkid + ",";
                                sqlstring = sqlstring + "'"+F_SQXH + "',";
                                //if (brlb == "1")
                                //{
                                //    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_zyh"].ToString().Trim() + "',";
                                //    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.3')";
                                //}
                                //else
                                //{
                                //    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_mzh"].ToString().Trim() + "',";
                                sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.8.2" + "')";
                                //}

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                                }

                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();

                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表成功");
                                    else
                                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表失败");

                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                #endregion


                                # region  写入DGATE_DOCUMENT_INFO
                                if (pdftoyxpt)
                                {
                                    if (pdfpkid == "")
                                    {
                                        pdfbginsert = "ADD";
                                    }
                                        sqlstring = "INSERT INTO DGATE_DOCUMENT_INFO(DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PATIENT_ID,PATIENT_DOMAIN_ID,"
                                  + "REQUEST_NUMBER,FILE_TYPE,PAY_LOAD_TYPE,SUB_TYPE,CONTENT,START_TIME,EFFECTIVE_TIME,PATIENT_TYPE,END_TIME,"
                                  + "ASSIGNED_PERSON,DOC_NAME,ADMIT_TIME,HIS_TYPE,BED_NO,ASSIGNED_CODE,AUTHOR_CODE,AUTHOR_NAME,"
                                  + "ITEM_NAME,REQUEST_DOMAIN,TPOS_PATH,PAT_CATEGORY_SYSTEM,PAT_CATEGORY,PAT_NAME,ACCESSIONNUM,BODY_PART,"
                                  + "DIAGNOSIS_METHOD_CODE,DICOM_STUDY_TIME,DICOM_NUM,MODALITY,FILE_SYSTEM_FK)values(";
                                        sqlstring = sqlstring + "'" + F_SQXH + "',";
                                        sqlstring = sqlstring + "'" + pisid + "',";
                                        sqlstring = sqlstring + "'" + F_blh + "',";
                                        sqlstring = sqlstring + "'" + pisid + "',";

                                        sqlstring = sqlstring + "'" + F_SQXH + "',";
                                        sqlstring = sqlstring + "'PATH-PDF',";
                                        sqlstring = sqlstring + "'XDS.PISBG',";
                                        sqlstring = sqlstring + "'" + pdfbginsert + "',";
                                        sqlstring = sqlstring + "'pdf',";
                                        sqlstring = sqlstring + "" + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')" + ",";
                                        sqlstring = sqlstring + "" + "to_date('" + dt_jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                                        sqlstring = sqlstring + "'" + brlb + "',";
                                        sqlstring = sqlstring + "" + "to_date('" + dt_jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS'),";

                                        sqlstring = sqlstring + "'" + shys + "',";
                                        sqlstring = sqlstring + "'病理诊断报告',";
                                        sqlstring = sqlstring + "to_date('" + dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                                        sqlstring = sqlstring + "'" + brlb + "',";
                                        sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_CH"].ToString().Trim() + "',";
                                        sqlstring = sqlstring + "'" + shysgh + "',";
                                        sqlstring = sqlstring + "'" + bgysgh + "',";
                                        sqlstring = sqlstring + "'" + bgys + "',";

                                        sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "',";
                                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.4.9',";
                                        sqlstring = sqlstring + "'" + ftplj + "',";
                                        sqlstring = sqlstring + "'" + "2.16.840.1.113883.4.487.2.2.1.1.9" + "',";
                                        sqlstring = sqlstring + "'" + brlb + "',";
                                        sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_XM"].ToString().Trim() + "',";
                                        sqlstring = sqlstring + "'" + F_SQXH + "',";
                                        sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "',";

                                        sqlstring = sqlstring + "'病理报告',";
                                        sqlstring = sqlstring + "to_date('" + dt_jcxx.Rows[0]["F_SDRQ"].ToString().Trim() + "','yyyy-MM-dd HH24:MI:SS'),";
                                        sqlstring = sqlstring + "'" + brbh + "',";
                                        sqlstring = sqlstring + "'病理报告','1')";
                                   
                                    if (debug == "1")
                                    {
                                        log.WriteMyLog("插入DGATE_DOCUMENT_INFO表语句：" + sqlstring);
                                    }

                                    OCMD.CommandText = sqlstring;
                                    OCMD.Parameters.Clear();
                                    try
                                    {
                                        conn.Open();
                                        int y = OCMD.ExecuteNonQuery();
                                        if (y > 0)
                                            log.WriteMyLog("插入DGATE_DOCUMENT_INFO表成功");
                                        else
                                            log.WriteMyLog("插入DGATE_DOCUMENT_INFO表失败");
                                    }
                                    catch (Exception ex)
                                    {

                                        log.WriteMyLog("插入DGATE_DOCUMENT_INFO表异常:" + ex.Message);
                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                }

                                #endregion

                                #region DGATE_DOCUMENT_INFO

                                try
                                {

                                    OCMD.CommandText = "select * from DGATE_DOCUMENT_INFO where PATIENT_ID='" + F_blh + "' and DOCUMENT_DOMAIN_ID='" + pisid + "' and DOCUMENT_UNIQUE_ID='" + F_SQXH + "' and FILE_TYPE='PATH-PDF' order by PK desc";
                                    pdfpkid = "";
                                    OracleDataAdapter oda = new OracleDataAdapter(OCMD);
                                    DataTable dt1 = new DataTable();
                                    oda.Fill(dt1);
                                    if (dt1.Rows.Count > 0)
                                    {
                                        pdfpkid = dt1.Rows[0]["PK"].ToString().Trim();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("查询DGATE_DOCUMENT_INFO表异常：" + ex.Message);
                                }
                                finally
                                {

                                }
                                #endregion

                                #region 写入DGATE_EXTEND_ID_INFO表
                                sqlstring = "insert into DGATE_EXTEND_ID_INFO (DOCUMENT_FK,ID,DOMAIN_ID) values(";
                             //   sqlstring = sqlstring + "ats_gate_pis.dgate_extend_id_info_seq.nextval,";
                                sqlstring = sqlstring + pdfpkid.ToString() + ",";
                               // if (brlb == "1")
                                //{
                                    sqlstring = sqlstring + "'" + F_SQXH+ "',";
                                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.2.2.8.2')";
                                //}
                                //else
                                //{
                                //    sqlstring = sqlstring + "'" + dt_jcxx.Rows[0]["F_mzh"].ToString().Trim() + "',";
                                //    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.1')";
                                //}

                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                                }

                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();
                                  
                                try
                                {
                                    conn.Open();
                                    int y = OCMD.ExecuteNonQuery();
                                    if (y > 0)
                                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表成功");
                                    else
                                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表失败");
                                }
                                catch (Exception ex)
                                {
                                    log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表异常：" + ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }
                                #endregion

                                aa.ExecuteSQL("update T_JCXX  set f_topacs='2' where F_blh='" + blh + "'");
                              
                            }

                            log.WriteMyLog("上传PACS完成");
                            if(bglx=="cg")
                           // aa.ExecuteSQL("update T_JCXX  set F_TOPACS='1' where F_BLH='"+blh+"'");
                            ZgqClass.BGHJ(blh, "ZGQJK", "审核PDF", "上传PACS平台成功", "ZGQJK", "上传PACS");
                        }
                        catch(Exception   ee2)
                        {
                            ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传PACS异常:" + ee2.Message, "ZGQJK", "上传PACS");
                            log.WriteMyLog("[PACS]异常2:"+ee2.Message);
                        }
                    }
                   catch(Exception   ee3)
                        {
                            log.WriteMyLog("[PACS]异常3:"+ee3.Message);
                            ZgqClass.BGHJ(blh, "ZGQJK", "审核", "上传PACS异常2:" + ee3.Message, "ZGQJK", "上传PACS");
                        }
            }
  
        public  string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string certificate)
        {
            string WebUrl = f.ReadString("savetohis", "WebUrl", "").Replace("\0", "").Trim();
            //集成平台地址
            if (WebUrl == "")
            {
                WebUrl = ZgqClass.GetSz("ZGQJK", "WebUrl","");
            }
            xmyyweb.WSInterface wsif = new PathHISZGQJK.xmyyweb.WSInterface();
          

            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";
            try
            { 
                if (WebUrl.Trim() != "")
                wsif.Url = WebUrl;
                return wsif.CallInterface(msgHeader, msgBody);
            }
            catch (Exception ee)
            {
                log.WriteMyLog("调用rtn_CallInterface方法异常：" + ee.Message.ToString());
                 return "Error:" + ee.Message;
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
                    return "000";
            }
            catch
            {
                return "000";
            }
        }

        public void pathtohis20170505(string blh, string bglx, string bgxh, string msg1, string debug1, string[] cslb)
        {
            #region 20170505

            //        bglx = bglx.ToLower();
            //        if (bglx == "")
            //            bglx = "cg";
            //        if (bgxh == "")
            //            bgxh = "0";

            //        string blbh = blh + bglx + bgxh;

            //        string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            //        string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();

            //        debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();

            //        string isbghj = f.ReadString("savetohis", "isbghj", "1").Replace("\0", "").Trim();


            //        string xtdm = f.ReadString("savetohis", "xtdm", "2060000").Replace("\0", "").Trim(); ;

            //        string certificate = f.ReadString("savetohis", "certificate", "5lYdPpiVdi0CxHKEhy3kqbzNlsXgNKZb").Replace("\0", "").Trim();
            //        string tjtxpath = f.ReadString("savetohis", "toPDFPath", @"\\192.0.19.147\GMS").Replace("\0", "").Trim(); ;

            //        //回写状态   客户端用
            //        int hczt = f.ReadInteger("savetohis", "hczt", 0);
            //        int hcbg = f.ReadInteger("savetohis", "hcbg", 0);
            //        int ptjk = f.ReadInteger("savetohis", "ptjk", 0);
            //        int hisjk = f.ReadInteger("savetohis", "hisjk", 0);
            //        int topacs = f.ReadInteger("savetohis", "topacs", 0);

            //        msg = msg1;

            //        string qxsh = "";
            //        bglx = bglx.ToLower();

            //        if (cslb.Length == 5)
            //        {
            //            if (cslb[4].ToLower() == "qxsh")
            //                qxsh = "1";//取消审核动作

            //        }

            //        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            //        DataTable jcxx = new DataTable();
            //        try
            //        {
            //            jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            //        }
            //        catch (Exception ex)
            //        {
            //            LGZGQClass.log.WriteMyLog(ex.Message.ToString());
            //            return;
            //        }
            //        if (jcxx == null)
            //        {
            //            LGZGQClass.log.WriteMyLog("病理数据库设置有问题！");
            //            return;
            //        }
            //        if (jcxx.Rows.Count < 1)
            //        {
            //            LGZGQClass.log.WriteMyLog("病理号有错误！");
            //            return;
            //        }


            //        if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
            //        {
            //            LGZGQClass.log.WriteMyLog("无申请序号,不处理");
            //            ZgqClass.BGHJ(blh, CZY, "保存", "无申请序号,不处理", "ZGQJK", "");
            //            aa.ExecuteSQL("update T_JCXX_FS set F_fszt='不处理',F_BZ='无申请序号,不处理'  where F_blbh='" + blbh + "'");

            //            return;
            //        }

            //        string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

            //        if (qxsh == "1")
            //        {
            //            bgzt = "取消审核";
            //        }



            //        string brbh = jcxx.Rows[0]["F_BRBH"].ToString().Trim();
            //        string brlb = jcxx.Rows[0]["F_brlb"].ToString().Trim();
            //        string sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim();
            //        if (brlb == "住院") brlb = "I";
            //        else brlb = "O";

            //        string ZYH = jcxx.Rows[0]["F_YZID"].ToString().Trim();
            //        //if (brlb == "I")
            //        //    ZYH = jcxx.Rows[0]["F_YZID"].ToString().Trim();

            //        string SFZH = jcxx.Rows[0]["F_SFZH"].ToString().Trim();
            //        string XM = jcxx.Rows[0]["F_XM"].ToString().Trim();
            //        string SJKS = jcxx.Rows[0]["F_BQ"].ToString().Trim();
            //        string CH = jcxx.Rows[0]["F_CH"].ToString().Trim();
            //        string YZXM = jcxx.Rows[0]["F_YZXM"].ToString().Trim();





            //        if (hczt == 1)
            //        {
            //            if (bglx == "cg" && ptjk == 1)
            //            {
            //                #region  回状态(不包括审核)
            //                if (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已写报告" || bgzt == "报告延期" || bgzt == "取消审核")
            //                {

            //                    string bgzt_1 = "";
            //                    if (bgzt == "已登记" || bgzt == "已取材")
            //                        bgzt_1 = "S";

            //                    if (bgzt == "已写报告" || bgzt == "报告延期")
            //                        bgzt_1 = "R";

            //                    if (bgzt == "已审核")
            //                        bgzt_1 = "F";

            //                    if (bgzt == "已写报告" && jcxx.Rows[0]["F_HXBJ"].ToString().Trim() == "1")
            //                        bgzt_1 = "C";

            //                    if (bgzt == "取消审核")
            //                        bgzt_1 = "C";


            //                    if (bgzt_1 == "")
            //                    {
            //                        LGZGQClass.log.WriteMyLog("bgzt_1状态为空，不回写！");
            //                        return;
            //                    }


            //                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
            //                              + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"

            //                              + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
            //                                + "ORC|SC|" + sqxh + "|||RC|||||||" + CZYGH + "^" + CZY + "\r"
            //                                + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;

            //                    if (debug == "1")
            //                        LGZGQClass.log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-修改报告状态入参：" + ChangeGmsApplyStatus_Hl7);

            //                    string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);

            //                    if (debug == "1")
            //                        LGZGQClass.log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-修改报告状态平台返回：" + rtn_msg2);

            //                    if (rtn_msg2.Trim() == "-1")
            //                    {
            //                        LGZGQClass.log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-回写报告状态失败：" + rtn_msg2);
            //                        if (isbghj == "1")
            //                            ZgqClass.BGHJ(blh, CZY, "保存", "回写报告状态失败:-1", "ZGQJK", "回写报告状态:" + bgzt);
            //                        return;
            //                    }
            //                    else
            //                    {
            //                        readhl7_fjfy r7 = new readhl7_fjfy();
            //                        int xy = 0;
            //                        r7.Adt01(rtn_msg2, ref xy);
            //                        if (r7.MSA[1].Trim() == "AA")
            //                        {
            //                            if (debug == "1")
            //                                LGZGQClass.log.WriteMyLog(r7.MSA[3].Trim());
            //                            if (isbghj == "1")
            //                                ZgqClass.BGHJ(blh, CZY, "保存", "回写报告状态成功:" + r7.MSA[3].Trim(), "ZGQJK", "回写报告状态:" + bgzt);
            //                        }
            //                        else
            //                        {
            //                            LGZGQClass.log.WriteMyLog("回写报告状态失败：" + r7.MSA[3].Trim());
            //                            if (isbghj == "1")
            //                                ZgqClass.BGHJ(blh, CZY, "保存", "回写报告状态失败:" + r7.MSA[3].Trim(), "ZGQJK", "回写报告状态:" + bgzt);
            //                        }
            //                    }

            //                }

            //                #endregion
            //            }

            //            if (hisjk == 1)
            //            {
            //                xm1y xm1 = new xm1y();
            //                xm1.pathtohis(blh, "");
            //            }

            //        }

            //        //回写报告   plsc用

            //        if (hcbg == 1)
            //        {
            //            string bz = "";


            //            DataTable dt_bc = new DataTable();
            //            DataTable dt_bd = new DataTable();
            //            if (bglx == "bc")
            //            {
            //                dt_bc = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "'  and F_bc_bgxh='" + bgxh + "'", "bc");
            //                try
            //                {
            //                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString().Trim();
            //                }
            //                catch
            //                {
            //                    LGZGQClass.log.WriteMyLog("报告状态错误");
            //                }
            //            }
            //            if (bglx == "bd")
            //            {
            //                dt_bd = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "'  and F_bd_bgxh='" + bgxh + "'", "bd");
            //                try
            //                {
            //                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString().Trim();
            //                }
            //                catch
            //                {
            //                    LGZGQClass.log.WriteMyLog("报告状态错误");
            //                }
            //            }


            //            if (qxsh == "1")
            //            {
            //                bgzt = "取消审核";
            //            }





            //            if (bgzt == "已审核")
            //            {
            //                //上传结果
            //                string txwebpath = ZgqClass.GetSz("ZGQJK", "txwebpath", @"http://192.0.1.75/pathimages").Replace("\0", "").Trim();

            //                if (bglx == "cg")
            //                {
            //                    #region 回写审核状态
            //                    string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
            //                                  + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"

            //                                  + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
            //                                    + "ORC|SC|" + sqxh + "|" + blh + "||RC|||||||" + CZYGH + "^" + CZY + "\r"
            //                                    + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||F";

            //                    if (debug == "1")
            //                        LGZGQClass.log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-修改报告审核状态入参：" + ChangeGmsApplyStatus_Hl7);

            //                    string rtn_msg_zt = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);

            //                    if (debug == "1")
            //                        LGZGQClass.log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-修改报告审核状态平台返回：" + rtn_msg_zt);

            //                    if (rtn_msg_zt.Trim() == "-1")
            //                    {

            //                        if (isbghj == "1")
            //                            ZgqClass.BGHJ(blh, CZY, "保存", "回写报告审核状态失败:-1", "ZGQJK", "回写报告状态:已审核");
            //                        LGZGQClass.log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-回写报告状态失败：" + rtn_msg_zt);
            //                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='回写报告审核状态失败'  where F_blbh='" + blbh + "'");

            //                        return;
            //                    }
            //                    else
            //                    {
            //                        readhl7_fjfy r7 = new readhl7_fjfy();
            //                        int xy = 0;
            //                        r7.Adt01(rtn_msg_zt, ref xy);
            //                        if (r7.MSA[1].Trim() == "AA")
            //                        {
            //                            if (debug == "1")
            //                                LGZGQClass.log.WriteMyLog(r7.MSA[3].Trim());
            //                            if (isbghj == "1")
            //                                ZgqClass.BGHJ(blh, CZY, "保存", "回写报告状态成功:" + r7.MSA[3].Trim(), "ZGQJK", "回写报告状态:已审核");


            //                        }
            //                        else
            //                        {

            //                            if (isbghj == "1")
            //                                ZgqClass.BGHJ(blh, CZY, "保存", "回写报告状态失败:" + r7.MSA[3].Trim(), "ZGQJK", "回写报告状态:已审核");
            //                            LGZGQClass.log.WriteMyLog("回写报告状态失败：" + r7.MSA[3].Trim());
            //                        }
            //                    }

            //                    #endregion
            //                }
            //                string jpgname = "";
            //                string jpgpath = "";
            //                string pdflj_pacs = "";
            //                string ispdf = f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim();
            //                if (ispdf == "1")
            //                {
            //                    #region  生成pdf

            //                    if (bgzt.Trim() == "已审核")
            //                    {

            //                        string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            //                        string err_msg = "";


            //                        ZgqClass zgq = new ZgqClass();
            //                        string pdfpath = "";
            //                        //生成pdf
            //                        bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqClass.type.PDF, ref err_msg, ref jpgname, ref jpgpath, ref pdfpath);
            //                        if (isrtn)
            //                        {
            //                            //pdf上传pacs

            //                            if (topacs == 1)
            //                            {
            //                                #region  生成并上传pdf到服务器

            //                                if (File.Exists(jpgname))
            //                                {
            //                                    //上传PACS服务器
            //                                    string Errmsg = "";
            //                                    string ftpml = DateTime.Now.ToString("yyyyMMdd");
            //                                    bool status = false;
            //                                    for (int y = 0; y < 3; y++)
            //                                    {
            //                                        status = zgq.UpPDF(jpgname, ftpml, ref Errmsg, 4);
            //                                        if (status)
            //                                            break;
            //                                    }
            //                                    if (status)
            //                                    {
            //                                        FileInfo fi = new FileInfo(jpgname);
            //                                        pdflj_pacs = "PIS" + "//" + ftpml + "//" + fi.Name;
            //                                        if (debug == "1")
            //                                            log.WriteMyLog("PDF上传PACS成功,路径：" + pdflj_pacs);
            //                                    }
            //                                    else
            //                                    {
            //                                        log.WriteMyLog("PDF上传PACS失败,路径：" + Errmsg);
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    LGZGQClass.log.WriteMyLog("[PACS]未找到PDF文件:" + pdflj_pacs);
            //                                }
            //                                #endregion

            //                            }
            //                            jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
            //                            ZgqClass.BGHJ(blh, "his接口", "报告审核", "生成pdf成功:" + jpgpath + "\\" + jpgname, "ZGQJK", "pdf");
            //                            aa.ExecuteSQL("insert  into T_BG_PDF(F_blbh,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "','" + pdfpath + "')");
            //                            zgq.DeleteTempFile(blh);
            //                        }
            //                        else
            //                        {
            //                            LGZGQClass.log.WriteMyLog(blh + "-生成pdf失败：" + err_msg);
            //                            ZgqClass.BGHJ(blh, "his接口", "审核PDF", "生成pdf失败" + err_msg, "ZGQJK", "pdf");
            //                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='生成pdf失败" + err_msg + "'  where F_blbh='" + blbh + "'");

            //                            if (msg == "1")
            //                                MessageBox.Show("病理号：" + blh + "  生成pdf失败，请重新审核！\r\n" + err_msg);
            //                            zgq.DeleteTempFile(blh);
            //                            return;
            //                        }
            //                    }

            //                    # endregion
            //                }

            //                ///上传pacs
            //                if (bglx == "cg")
            //                    BgToPacs(blh, bglx, bgxh, bgzt, jcxx, dt_bc, dt_bd, pdflj_pacs, debug);


            //                #region 回传报告




            //                //TBS报告
            //                string bggs = jcxx.Rows[0]["F_BGGS"].ToString().Trim();
            //                string TBSSJ = "";
            //                string rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim();
            //                string xjsj = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
            //                string blzd = jcxx.Rows[0]["F_blzd"].ToString().Trim();
            //                string tsjc = jcxx.Rows[0]["F_tsjc"].ToString().Trim();

            //                string jcfd = jcxx.Rows[0]["F_BLK"].ToString().Trim();
            //                if (bggs == "TBS")
            //                {
            //                    DataTable DT_TBS = new DataTable();
            //                    DT_TBS = aa.GetDataTable("select * from T_TBS_BG where F_blh='" + blh + "'", "jcxx");
            //                    if (DT_TBS.Rows.Count > 0)
            //                    {
            //                        TBSSJ = "抹片分析：" + DT_TBS.Rows[0]["F_TBS_MPFX"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "细胞项目：" + DT_TBS.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\r\n";

            //                        TBSSJ = "\r\n微生物项目：" + DT_TBS.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n";

            //                        TBSSJ = "\r\n病毒项目：" + DT_TBS.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";
            //                        TBSSJ = "          " + DT_TBS.Rows[0]["F_TBS_BDXM2"].ToString().Trim() + "\r\n";

            //                        TBSSJ = "\r\n抹片质数：" + DT_TBS.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "";
            //                        TBSSJ = "\r\n炎症细胞/遮盖比率*：：" + DT_TBS.Rows[0]["F_TBS_ZGBL"].ToString().Trim() + "";

            //                        rysj = TBSSJ;
            //                        blzd = DT_TBS.Rows[0]["F_TBSZD"].ToString().Trim();
            //                        jcfd = DT_TBS.Rows[0]["F_TBS_JYFF"].ToString().Trim();
            //                    }


            //                }

            //                string pdflj = tjtxpath + "\\" + jpgpath + "\\" + jpgname;
            //                string bgys = jcxx.Rows[0]["F_BGYS"].ToString().Trim();

            //                string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();

            //                string qcys = jcxx.Rows[0]["F_QCYS"].ToString().Trim();

            //                string qcrq = jcxx.Rows[0]["F_QCrq"].ToString().Trim();
            //                string bgrq = "";//DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
            //                string shrq = ""; //DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");

            //                try
            //                {
            //                    qcrq = DateTime.Parse(qcrq).ToString("yyyyMMddHHmmss");
            //                }
            //                catch
            //                {
            //                }
            //                if (bglx == "cg")
            //                {
            //                    bgrq = DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
            //                    shrq = DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");
            //                }
            //                if (bglx == "bc")
            //                {
            //                    bgys = dt_bc.Rows[0]["F_bc_BGYS"].ToString().Trim();
            //                    shys = dt_bc.Rows[0]["F_bc_SHYS"].ToString().Trim();
            //                    bgrq = DateTime.Parse(dt_bc.Rows[0]["F_bc_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
            //                    shrq = DateTime.Parse(dt_bc.Rows[0]["F_bc_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");
            //                    rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim();
            //                    xjsj = dt_bc.Rows[0]["F_bc_jxsj"].ToString().Trim();
            //                    blzd = dt_bc.Rows[0]["F_bczd"].ToString().Trim();
            //                    tsjc = dt_bc.Rows[0]["F_bc_tsjc"].ToString().Trim();
            //                }
            //                if (bglx == "bd")
            //                {
            //                    bgys = dt_bd.Rows[0]["F_bd_BGYS"].ToString().Trim();
            //                    shys = dt_bd.Rows[0]["F_bd_SHYS"].ToString().Trim();
            //                    bgrq = DateTime.Parse(dt_bd.Rows[0]["F_bd_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
            //                    shrq = bgrq;
            //                    rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim();
            //                    xjsj = "";
            //                    blzd = dt_bd.Rows[0]["F_bdzd"].ToString().Trim();
            //                    tsjc = "";
            //                }
            //                string bgysgh = getyhgh(bgys);
            //                string shysgh = getyhgh(shys);
            //                string qcysgh = getyhgh(qcys);
            //                string hxbj = jcxx.Rows[0]["F_HXBJ"].ToString().Trim();

            //                string zt2 = "F";
            //                if (hxbj == "1")
            //                    zt2 = "C";

            //                string xb = jcxx.Rows[0]["F_XB"].ToString().Trim();
            //                if (xb == "女") xb = "F";
            //                else if (xb.Trim() == "男") xb = "M";
            //                else xb = "U";


            //                string SendGmsReport_hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORU^R01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
            //+ "PID|||" + brbh + "^^^^PI~" + ZYH + "^^^^VN||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||^" + jcxx.Rows[0]["F_nl"].ToString().Trim() + "|" + xb + "|||||||" + "\r"
            //+ "PV1||" + brlb + "|" + SJKS + "^^" + CH + "||||^||||||||||||" + ZYH + "|||||||||||||||||||||||||" + "\r"

            //+ "OBR||" + sqxh + "|" + blh + "|" + YZXM + "||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|||" + qcysgh + "^" + qcys
            //+ "|" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "||||^^^&" + jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "|^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim()
            //+ "||||||" + bgrq + "|||||||||^|" + bgysgh + "&" + bgys + "^^" + "||||||^" + jcxx.Rows[0]["F_JSY"].ToString().Trim() + "||2060000^病理科||||||1^~6^" + jcxx.Rows[0]["F_LCZD"].ToString().Trim().Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "\r"



            //+ "NTE|1||" + pdflj + "|Z-RP" + "\r"
            //+ "OBX|1|FT|^肉眼所见||" + (rysj.Trim()).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + "\r"
            //+ "OBX|2|FT| ^镜下所见||" + (xjsj).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcfd + "|" + "\r"
            //+ "OBX|3|FT| ^特殊检查||" + (tsjc).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcfd + "|" + "\r"
            //+ "OBX|4|FT| ^病理诊断||" + (blzd).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcfd + "|" + "\r";

            //                DataTable dt_tx = aa.GetDataTable("select * from V_DYTX where F_blh='" + blh + "'", "tx");
            //                if (dt_tx.Rows.Count > 0)
            //                {
            //                    for (int x = 0; x < dt_tx.Rows.Count; x++)
            //                    {

            //                        string txlj = txwebpath + "/" + jcxx.Rows[0]["F_TXML"].ToString().Trim() + "/" + dt_tx.Rows[x]["F_TXM"].ToString();
            //                        SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|" + (x + 1).ToString() + "|" + dt_tx.Rows[x]["F_ID"].ToString() + "|" + blh + "|" + txlj + "||1" + "\r";
            //                    }
            //                }
            //                else
            //                {
            //                    SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|1|1|" + blh + "|||" + "\r";
            //                }
            //                if (debug == "1")
            //                    LGZGQClass.log.WriteMyLog("报告审核回写结果：" + SendGmsReport_hl7);


            //                string rtn_msg2 = rtn_CallInterface("HL7v2", "SendGmsReport", SendGmsReport_hl7, "", certificate);

            //                if (rtn_msg2.Contains("error"))
            //                {
            //                    if (msg == "1")
            //                        MessageBox.Show("报告审核回写结果错误：" + rtn_msg2);
            //                    LGZGQClass.log.WriteMyLog("报告审核回写结果错误：" + rtn_msg2);
            //                    if (isbghj == "1")
            //                        ZgqClass.BGHJ(blh, "批量上传", "审核", "报告审核回写结果错误:" + rtn_msg2, "ZGQJK", "报告审核回写结果");
            //                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='" + rtn_msg2 + "'  where F_blbh='" + blbh + "'");
            //                    return;
            //                }
            //                else
            //                {
            //                    readhl7_fjfy r7 = new readhl7_fjfy();
            //                    int xy = 0;
            //                    r7.Adt01(rtn_msg2, ref xy);
            //                    if (r7.MSA[1].Trim() == "AA")
            //                    {
            //                        if (debug == "1")
            //                            LGZGQClass.log.WriteMyLog(r7.MSA[3].Trim());
            //                        if (isbghj == "1")
            //                            ZgqClass.BGHJ(blh, "批量上传", "审核", "报告审核回写结果成功:" + r7.MSA[3].Trim(), "ZGQJK", "报告审核回写结果");

            //                    }
            //                    else
            //                    {
            //                        if (msg == "1")
            //                            MessageBox.Show(r7.MSA[3].Trim());
            //                        LGZGQClass.log.WriteMyLog("报告审核回写结果失败：" + r7.MSA[3].Trim());
            //                        if (isbghj == "1")
            //                            ZgqClass.BGHJ(blh, "批量上传", "审核", "报告审核回写结果失败:" + r7.MSA[3].Trim(), "ZGQJK", "报告审核回写结果");
            //                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='报告审核回写结果失败:" + r7.MSA[3].Trim() + "'  where F_blbh='" + blbh + "' ");

            //                        return;
            //                    }

            //                }

            //                #endregion

            //                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blbh='" + blbh + "' ");
            //                aa.ExecuteSQL("update T_JCXX set F_HXBJ='1' where F_blh='" + blh + "'");
            //                return;

            //            }
            //            //取消审核，删除pdf
            //            #region
            //            if (bgzt == "取消审核")
            //            {
            //                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blbh='" + blbh + "'", "dt2");
            //                if (dt2.Rows.Count > 0)
            //                {

            //                    //判断共享上是否存在该pdf文件
            //                    if (File.Exists(tjtxpath + "\\" + dt2.Rows[0]["F_ML"] + "\\" + dt2.Rows[0]["F_FILENAME"]))
            //                    {

            //                        //删除共享上的pdf文件
            //                        File.Delete(tjtxpath + "\\" + dt2.Rows[0]["F_ML"] + "\\" + dt2.Rows[0]["F_FILENAME"]);
            //                    }
            //                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "' ");
            //                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blbh='" + blbh + "' and F_BGZT='取消审核'");

            //                }
            //            }
            //            #endregion
            //        }

            //        return;

            #endregion
        }


    }
}
