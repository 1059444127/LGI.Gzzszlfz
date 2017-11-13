

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
    //厦门第一医院 webservices+hl7
    class xmyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string msg = "";
        public void pathtohis(string blh, string bglx, string bgxh, string msg1, string debug1, string[] cslb)
        {

            msg=msg1;

            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

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
            //if (jcxx.Rows[0]["F_SQXH"].ToString().Trim()=="")
            //{
            //    LGZGQClass.log.WriteMyLog("无申请序号，不会写");
            //    return;
            //}

            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

            if (qxsh == "1")
            {
                bgzt = "取消审核";
            }

            int plsc = f.ReadInteger("fsjk", "plsc", 0);
            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            string brbh = jcxx.Rows[0]["F_BRBH"].ToString().Trim();
            string brlb = jcxx.Rows[0]["F_brlb"].ToString().Trim();
            string sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim();
            if (brlb == "住院") brlb = "I";
            else brlb = "O";
         
            string ZYH = jcxx.Rows[0]["F_MZH"].ToString().Trim();
            if (brlb == "I")
                ZYH = jcxx.Rows[0]["F_ZYH"].ToString().Trim();

            string SFZH = jcxx.Rows[0]["F_SFZH"].ToString().Trim();
            string XM = jcxx.Rows[0]["F_XM"].ToString().Trim();
            string SJKS = jcxx.Rows[0]["F_BQ"].ToString().Trim();
            string CH = jcxx.Rows[0]["F_CH"].ToString().Trim();
            string YZXM = jcxx.Rows[0]["F_YZXM"].ToString().Trim();

            string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            string message = ""; string jpgname = "";
            ZgqPDFJPG zgq = new ZgqPDFJPG();
            bool ispdf = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF,ref jpgname,"", ref message);

                return;
           
        }

  
    }
}
