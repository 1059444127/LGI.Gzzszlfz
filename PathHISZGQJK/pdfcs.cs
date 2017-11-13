
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
using System.Diagnostics;
using System.Threading;
using ZgqClassPub;
namespace PathHISZGQJK
{
    
    class pdfcs
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string debug = "";
        public void pdfjpg(string blh, string bglx, string bgxh, string czlb,string dz, string msg, string debug,string yymc)
        {
            bglx = bglx.ToLower();
              if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "1";

           string blbh=blh+bglx+bgxh;
            if(bglx=="cg")
                blbh=blh;


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
             if (dz == "qxsh"||dz == "取消审核")
                {
                    //取消审核动作
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "'");
                }

         
                string bgzt= jcxx.Rows[0]["F_BGZT"].ToString();
                try
                {
                    if (bglx.ToLower().Trim() == "bd")
                    {
                        DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt= dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    //    jpgname=blh+bglx+bgxh+"_"+dt_bd.Rows[0]["F_BD_BGRQ"].ToString();
                    }
                    if (bglx.ToLower().Trim() == "bc")
                    {
                        DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                        bgzt= dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                   //     jpgname=blh+bglx+bgxh+"_"+dt_bc.Rows[0]["F_BC_SPARE5"].ToString();
                    }
                    if (bglx.ToLower().Trim() == "cg")
                    {
                        bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                   //     jpgname=blh+bglx+bgxh+"_"+jcxx.Rows[0]["F_SPARE5"].ToString();
                    }
                }
                catch (Exception e5)
                {
                    log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh + e5.Message);
                }
             
                 if(bgzt!="已审核")
                     return;

                  string type = f.ReadString("savetohis", "type", "pdf").Trim().ToLower();
                   
                   

                    
                   ////生成pdf或jpg
                  string jpgname = "";//生成pdf或jpg文件名
                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");//ftp 上目录，可为空
                   ZgqPDFJPG zgq = new ZgqPDFJPG();
                    string message = "";
                   // string pdfpath = "";
                   // if (zgq.CreatePDF(blh, bglx, bgxh, ref jpgname, "", "", ZgqPDFJPG.Type.PDF, ref message))
                   // {
                   //     log.WriteMyLog("[" + blh + "]生成PDF成功");
                   //     //生成pdf成功上传pdf到病理服务器
                   //     if (zgq.UpPDF(blh, jpgname, ML, ref message, 3, ref pdfpath))
                   //     {
                   //         log.WriteMyLog("[" + blh + "]上传PDF成功:" + pdfpath);
                   //         jpgname = pdfpath.Substring(pdfpath.LastIndexOf('\\') + 1);
                   //         aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                   //         aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                   //     }
                   //     else
                   //         log.WriteMyLog("[" + blh + "]上传PDF失败:" + message);
                   // }
                   // else
                   // {
                   //     log.WriteMyLog("["+blh+"]生成PDF失败:" + message);
                   //     ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "生成pdf失败" + message, "ZGQJK", "生成PDF");
                   // }



                    bool isrtn = false;
                    if (yymc.Trim() == "pdf")
                        isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", "", ref message);
                   else
                        isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", "", ref message);

                    if (!isrtn)
                    {
                        log.WriteMyLog("生成PDF失败" + message);
                        ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "生成pdf失败" + message, "ZGQJK", "生成PDF");
                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("生成PDF成功");

                        ////二进制串
                        if (File.Exists(jpgname))
                        {

                            //上传
                            string pdfpath = "";
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message,3, ref pdfpath);
                          
                            if (ssa == true)
                            {
                                if (debug == "1")
                                 log.WriteMyLog("上传PDF成功");

                                jpgname = pdfpath.Substring(pdfpath.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "上传PDF成功", "审核", "上传PDF成功:" + pdfpath, "ZGQJK", "上传PDF");
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                            }
                            else
                            {
                                log.WriteMyLog("上传PDF失败" + message);
                                ZgqClass.BGHJ(blh, "上传PDF失败", "审核", message, "ZGQJK", "上传PDF");
                            }
                        }
                        else
                        {
                             log.WriteMyLog("未找到PDF文件" + jpgname);
                            ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "未找到文件" + jpgname, "ZGQJK", "生成PDF");
                        }
                    }
                    zgq.DelTempFile(blh);

    
                return;

            }
        }
}
