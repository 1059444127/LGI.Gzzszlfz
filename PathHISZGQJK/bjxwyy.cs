using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class bjxwyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string F_blh, string bglx, string bgxh,string czlb,string dz, string msg, string debug)
        {

            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();
            try
            {
                //审核后上传报告
                string shhscbg = f.ReadString("savetohis", "shhscbg", "1").Replace("\0", "");
                int yssj = f.ReadInteger("savetohis", "yssj", 8);
                if (bglx == "")
                    bglx = "cg";

                if (bgxh == "")
                    bgxh = "1";

                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable bljc = new DataTable();
                try
                {
                    bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return;
                }
                if (bljc == null)
                {
                    MessageBox.Show("病理数据库设置有问题！");
                    return;
                }
                if (bljc.Rows.Count < 1)
                {
                    MessageBox.Show("病理号有错误！");
                    return;
                }

                string bgzt = "";
                if (dz == "qxsh")
                    bgzt = "取消审核";
                else
                    bgzt = bljc.Rows[0]["F_BGZT"].ToString();

                if (bljc.Rows[0]["F_ZYH"].ToString().Trim() == "" && bljc.Rows[0]["F_MZH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("住院号和门诊号为空，不处理");
                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='不处理',F_BZ='住院号和门诊号为空，不处理' where F_blh='" + F_blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
                    return;
                }

                bool issc = false;
                string ftplj = "";
                if (shhscbg == "1")
                {
                    if (bgzt == "已审核" && bljc.Rows[0]["F_SFDY"].ToString().Trim() == "是")
                        issc = true;
                }

                //修改已发布状态
                if (bgzt == "已审核" && bljc.Rows[0]["F_SFDY"].ToString().Trim() == "是")
                {
                    if (bglx == "cg")
                    {
                        try
                        {
                            if (DateTime.Parse(bljc.Rows[0]["F_spare5"].ToString().Trim()).AddHours(yssj) <= DateTime.Now)
                                aa.ExecuteSQL("update T_JCXX  set F_BGZT='已发布'  where F_BLH='" + F_blh + "'");
                            else
                                issc = false;
                        }
                        catch
                        {
                        }
                    }
                }

                bool pdftoyxpt = false;
                #region  生成并上传pdf到服务器

                if (bgzt == "已发布" || issc)
                {
                    try
                    {
                        string bgzt2 = "";
                        DataTable dt_bd = new DataTable();
                        DataTable dt_bc = new DataTable();
                        try
                        {
                            if (bglx.ToLower().Trim() == "bd")
                            {
                                dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + F_blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                                bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                            }

                            if (bglx.ToLower().Trim() == "bc")
                            {
                                dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + F_blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                                bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                            }
                            if (bglx.ToLower().Trim() == "cg")
                            {
                                bgzt2 = bljc.Rows[0]["F_BGZT"].ToString();
                            }
                        }
                        catch
                        {
                        }

                        if (bgzt2.Trim() == "")
                            log.WriteMyLog("报告状态为空！不处理！" + F_blh + "^" + bglx + "^" + bgxh);

                        if ((bgzt2.Trim() == "已发布" || issc) && bgzt != "取消审核")
                        {
                            ////////生成pdf**********************************************************
                            string jpgname = "";
                            string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                            if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
                            {
                                #region  生成pdf
                                string message = "";
                                ZgqPDFJPG zgq = new ZgqPDFJPG();
                                if (debug == "1")
                                    log.WriteMyLog("开始生成PDF。。。");
                                bool isrtn = zgq.CreatePDF(F_blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, ref message);

                                string pdfpath = "";
                                if (isrtn)
                                {
                                    if (File.Exists(jpgname))
                                    {
                                        //上传病理服务器
                                        bool ssa = zgq.UpPDF(F_blh, jpgname, ML, ref message, 3,ref pdfpath);
                                        if (ssa == true)
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog("上传PDF成功");

                                            string jpgname2 = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                            ZgqClass.BGHJ(F_blh, "JK", "审核", "上传PDF成功:" + pdfpath, "ZGQJK", "上传PDF");

                                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + F_blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + F_blh + "','" + jpgname2 + "')");
                                            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='已处理',F_ispdf='true' where F_blh='" + F_blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' ");
                                          }
                                        else
                                        {
                                            if (msg == "1")
                                                MessageBox.Show("上传PDF失败：" + message);
                                            log.WriteMyLog("上传PDF失败：" + message);
                                            ZgqClass.BGHJ(F_blh, "JK", "审核", message, "ZGQJK", "上传PDF");
                                            //   aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                        }

                                        //上传服务器

                                        //string ftpml = DateTime.Now.ToString("yyyyMMdd");
                                        //bool status = false;
                                        //for (int y = 0; y < 3; y++)
                                        //{
                                        //    status = zgq.UpPDF("", jpgname, ftpml, ref message, 4);
                                        //    if (status)
                                        //        break;
                                        //}
                                        //if (status)
                                        //{
                                        //    pdftoyxpt = true;
                                        //    FileInfo fi = new FileInfo(jpgname);
                                        //    ftplj = "PIS" + "//" + ftpml + "//" + fi.Name;
                                        //    log.WriteMyLog("ftp上传成功，上传路径：" + ftplj);
                                        //}
                                    }
                                    else
                                    {
                                        if (msg == "1")
                                            MessageBox.Show("生成PDF失败:未找到文件---" + jpgname);
                                        log.WriteMyLog("生成PDF失败:未找到文件---" + jpgname);
                                        ZgqClass.BGHJ(F_blh, "JK", "审核", "上传PDF失败:未找到文件---" + jpgname, "ZGQJK", "生成PDF");
                                    }
                                }
                                else
                                {
                                    if (msg == "1")
                                        MessageBox.Show("生成PDF失败：" + message);
                                    log.WriteMyLog("生成PDF失败：" + message);
                                    ZgqClass.BGHJ(F_blh, "JK", "审核", message, "ZGQJK", "生成PDF");
                                    // aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='生成PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                                zgq.DelTempFile(F_blh);
                                #endregion
                            }
                            //////////////////////*****************************************************************
                        }
                        else
                        {
                            if (bgzt == "取消审核")
                            {
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            }
                        }
                    }
                    catch (Exception ee3)
                    {
                        log.WriteMyLog(ee3.Message);
                    }
                }
                #endregion


                if (bglx != "cg")//只传常规报告
                {
                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='不处理'  F_bz='非常规报告' where F_blh='" + F_blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理'");
                    return;
                }
               
            }
            catch(Exception  ee)
            {
                log.WriteMyLog(ee.Message);
            }

            return;

        }
    }
}
