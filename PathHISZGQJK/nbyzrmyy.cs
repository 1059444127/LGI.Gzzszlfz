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
using ZgqClassPub;

namespace PathHISZGQJK
{
    //----------宁波鄞州人民医院----------------------
    class nbyzrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {

          //  MessageBox.Show("yzyz");
            string pathweburl = f.ReadString("savetohis", "webservicesurl", "");
            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");

            string patid = "";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
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
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") patid = bljc.Rows[0]["F_zyh"].ToString().Trim();
            //if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") patid = bljc.Rows[0]["F_mzh"].ToString().Trim();

            //if (patid == "")
            //{
            //     LGZGQClass.log.WriteMyLog("非住院病人，不处理！");
            //    return;

            //}

            //回传xml
            //DataTable jcxm = aa.GetDataTable("select * from T_whtjyy_jcxm where CheckFlow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'", "jcxm");

            string inxml = "";
            inxml = inxml + "<?xml version='1.0' encoding='GB2312'?>";
            inxml = inxml + "<REPORTINFO>";
            inxml = inxml + "<ITEM>";
            inxml = inxml + "<SQDBH>" + decimal.Parse(bljc.Rows[0]["F_sqxh"].ToString().Trim()) + "</SQDBH>";
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

            if (zt == "已审核") zt = "13";
            else
            {
                if (zt == "已写报告") zt = "11";
                else zt = "7";
            }
            inxml = inxml + "<ZT>" + zt + "</ZT>";
            string jsybm = getymh(bljc.Rows[0]["F_jsy"].ToString().Trim());
            inxml = inxml + "<JSRY>" + jsybm + "</JSRY>";

            inxml = inxml + "<JSSJ>" + DateTime.Parse(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHMMss") + "</JSSJ>";
            string bgys = getymh(bljc.Rows[0]["F_bgys"].ToString().Trim());
            inxml = inxml + "<BGRY>" + bgys + "</BGRY>";
            string bgsj = bljc.Rows[0]["F_bgrq"].ToString().Trim();

            if (bgsj != "") bgsj = DateTime.Parse(bgsj).ToString("yyyyMMddHHMMss");

            inxml = inxml + "<BGSJ>" + bgsj + "</BGSJ>";
            string shys = getymh(bljc.Rows[0]["F_shys"].ToString().Trim());
            inxml = inxml + "<SHRY>" + shys + "</SHRY>";
            string shsj = bljc.Rows[0]["F_SPARE5"].ToString().Trim();
            if (shsj != "") shsj = DateTime.Parse(shsj).ToString("yyyyMMddHHMMss");
            inxml = inxml + "<SHSJ>" + shsj + "</SHSJ>";
            inxml = inxml + "<CXRY>" + "" + "</CXRY>";
            inxml = inxml + "<CXSJ>" + "" + "</CXSJ>";
            inxml = inxml + "<JCSJ>" + bljc.Rows[0]["F_rysj"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</JCSJ>";
            string JCJL = "";
            if (zt == "13")
                JCJL = bljc.Rows[0]["F_blzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
            inxml = inxml + "<JCJL>" + JCJL + "</JCJL>";
            string f0brbh = bljc.Rows[0]["F_brbh"].ToString().Trim();
            if (zt == "13")
            inxml = inxml + "<WEBURL>http://192.168.0.108/pathwebrpt/index_p.asp?sick_id=" + f0brbh + "</WEBURL>";
            else
            inxml = inxml + "<WEBURL></WEBURL>";
            inxml = inxml + "<JCH>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</JCH>";
            inxml = inxml + "</ITEM>";
            inxml = inxml + "</REPORTINFO>";

            if (msg == "1")
            {
                log.WriteMyLog("回传的xml数据：" + inxml);
                MessageBox.Show(inxml);
            }
            string sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();

            string outxml = "";
            try
            {

                nbyzrmyyWeb.PisServiceLJ nbyzweb = new PathHISZGQJK.nbyzrmyyWeb.PisServiceLJ();
                if (pathweburl != "") nbyzweb.Url = pathweburl;
                outxml = nbyzweb.SetPISReportInfo(decimal.Parse(sqxh), inxml);

            }
            catch (Exception ee)
            {
                if (msg == "1")
                    MessageBox.Show("回传失败！调用HIS接口出错：" + ee.ToString());
                log.WriteMyLog("回传失败！调用HIS接口出错：" + ee.ToString());

                return;
            }

            if (outxml == "")
            {
                if (msg == "1")
                    MessageBox.Show("回传成功！");
              
            }
            else
            {
                log.WriteMyLog("回传失败！原因：" + outxml);
                if (msg == "1")
                    MessageBox.Show("回传报告失败！" + outxml);
                return;
            } 
            aa.ExecuteSQL("update  T_jcxx  set F_SCBJ='1' where F_blh='" + blh + "'");
        }
       
        public string getymh(string yhmc)//通过医生名称 获取医生编码
        {
            if (yhmc != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select F_yhm from T_yh where F_yhmc='" + yhmc + "'", "blxx");
                    return bljc.Rows[0]["F_YHM"].ToString().Trim();
                }
                catch (Exception ee)
                {
                    log.WriteMyLog("转换医生工号出错！原因：" + ee.ToString());
                    return "";
                }
            } return "";

        }

    }
}
