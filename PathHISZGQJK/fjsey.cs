using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using PathHISZGQJK;
using ZgqClassPub;


namespace PathHISZGQJK
{
    //福建省第二医院
    class fjsey
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
      //  string msg = ""; string debug = "";
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {

            string jkmsg = f.ReadString("jkmsg", "jkmsg", "0").Replace("\0", "");
            f.WriteInteger("jkmsg", "jkmsg", 0);

            bglx = bglx.ToLower();
            if (bglx.ToLower().Trim() == "bd")
                return;




             debug = f.ReadString("savetohis", "debug", "0");
             msg = f.ReadString("savetohis", "msg", "0");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(blh+","+ex.Message);
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show(blh + ",病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show(blh+",病理号有错误！");
                return;
            }
            string qxsh = "";
         

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                    qxsh = "1";//取消审核动作
            }

            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";

            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
            try
            {
                if (bglx.ToLower().Trim() == "bd")
                {
                    DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                }
                if (bglx.ToLower().Trim() == "bc")
                {
                    DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                }
            }
            catch
            {
            }

          

            if (qxsh == "1")
             bgzt = "取消审核";

             if (bgzt != "取消审核" && bgzt != "已审核" && bglx=="cg")
                return;




            if (bgzt.Trim() == "")
            {
                log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh); return;
            }


            if (bgzt != "取消审核" && bgzt != "已审核")
                return;






            if (bgzt.Trim() == "已审核")
            {
               
                string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
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
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                        if (jkmsg == "1")
                            MessageBox.Show(blh + ",上传PDF成功");
                    }
                    else
                    {
                        if (msg == "1" || jkmsg == "1")
                            MessageBox.Show(blh + ",上传PDF失败，请重新审核！\r\n异常消息：" + message);

                        log.WriteMyLog(message);
                        ZgqClass.BGHJ(blh, "批量上传", "审核", message, "ZGQJK", "生成PDF");
                        aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_BZ='" + message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

                    }
                    zgq.DelTempFile(blh);

                }
                else
                {

                    if (msg == "1" || jkmsg == "1")
                        MessageBox.Show(blh + ",生成PDF失败，请重新审核！\r\n异常消息：" + message);
                    log.WriteMyLog(message);
                    ZgqClass.BGHJ(blh, "接口", "审核", message, "ZGQJK", "生成PDF");
                    aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_BZ='" + message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    zgq.DelTempFile(blh);

                }
                return;

            }
            else
            {
             
                if(bgzt == "取消审核")
                {
                    //删除T_BG_PDF记录
                    DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                    if (dt2.Rows.Count > 0)
                    {
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        //删除ftp上pdf文件
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        string rtn_msg = "";
                        zgq.DelPDFFile(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);

                    }
                    else
                    {
                        log.WriteMyLog(blh + ",T_BG_PDF中未能找到记录");
                    }
                     aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理',F_ISPDF=''  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                 
                }
              

            } 
            return;
        }
    }
}
