using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Windows.Forms;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class gzslyy
    {
        //赣州市立医院
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string debug, string dz,string msg)
        {

          

            if (bglx == "")
                bglx = "cg";
            if (bgxh == "")
                bgxh = "0";

            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            string tjtxpath = f.ReadString("savetohis", "toPDFPath", @"\\192.0.19.147\GMS");
            bglx = bglx.ToLower();

          

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



            string jpgname = "";
            string jpgpath = "";
            string ispdf = f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim();
            if (ispdf == "1")
            {
            
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

                if (bgzt2.Trim() == "已审核" && dz != "qxsh")
                {

                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    string err_msg = "";
                
                    //生成pdf
                    bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.PDF, ref err_msg, ref jpgname, ref jpgpath);
                    if (isrtn)
                    {

                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "his接口", "报告审核", "生成pdf成功:" + jpgpath + "\\" + jpgname, "ZGQJK", "pdf");

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                    }
                    else
                    {
                        log.WriteMyLog(blh + "-" + err_msg);
                        ZgqClass.BGHJ(blh, "his接口", "审核PDF", "生成pdf失败" + err_msg, "ZGQJK", "pdf");

                        if (msg == "1")
                            MessageBox.Show("病理号：" + blh + "  生成pdf失败，请重新审核！\r\n" + err_msg);
                        return;
                    }
                }
                if (dz == "qxsh")
                {
                    DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                    if (dt2.Rows.Count > 0)
                    {

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");

                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        string rtn_msg = "";
                        zgq.DelPDFFile(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
                    }
                }



                return;

            }
        }


        public bool MD_PDF_JPG(string blh, string bglx, string bgxh, string ML, ZgqClassPub.ZgqPDFJPG.Type jpgpdf, ref string err_msg, ref string fileName, ref string fielPath)
        {


            string message = ""; string jpgname = "";
            ZgqPDFJPG zgq = new ZgqPDFJPG();
            bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf, ref jpgname, "", ref message); ;
            if (isrtn)
            {
                  bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 3);
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
