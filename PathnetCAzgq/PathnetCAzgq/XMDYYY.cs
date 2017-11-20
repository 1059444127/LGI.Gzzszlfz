using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using readini;
using dbbase;
using System.Data;

namespace PathnetCAzgq
{
    class XMDYYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public string ca(string yhxx)
        {

          

            ////-------获取sz中设置的参数---------------------
            string debug = f.ReadString("CA", "debug", "").Replace("\0", "").Trim();
            string getblh = "";
            string type = "";
            string type2 = "";
            string yhm = "";

            string yhmc = "";
            string yhbh = "";
            string yhmm = "";
            string bglx = "";
            string bgxh = "";

            string[] getyhxx = yhxx.Split('^');
            if (getyhxx.Length == 5)
            {
                type = getyhxx[0];
                yhm = getyhxx[1];
                yhmc = getyhxx[3];
                yhbh = getyhxx[2];
                yhmm = getyhxx[4];
            }
            else
            {
                type2 = getyhxx[0];
                getblh = getyhxx[1];
                bgxh = getyhxx[2];
                bglx = getyhxx[3].ToLower();
                type = getyhxx[4];
                yhm = getyhxx[5];
                yhmc = getyhxx[6];
                yhbh = getyhxx[7];
                yhmm = getyhxx[8];
            }
            if (type == "QXSH")
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                if (bglx == "" || bglx == "cg")
                {
                    DataTable dt_jcxx = aa.GetDataTable("select * from  T_JCXX  where F_BLH='" + getblh + "'", "jcxx");

                    if (dt_jcxx == null || dt_jcxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("CA查询数据失败");return  "1";
                    }
                    if (dt_jcxx.Rows[0]["F_CAGDZT"].ToString().Trim() == "已归档" || dt_jcxx.Rows[0]["F_CAGDZT"].ToString().Trim() == "1")
                    {
                        MessageBox.Show("此报告病案室已归档,不能进行修改。\r\n如需修改,请联系病案室取消归档。");
                        return "0";
                    }
                    return "1";
                }
                if (bglx == "bc")
                {
                    DataTable dt_bc = aa.GetDataTable("select * from  T_BCBG  where F_BLH='" + getblh + "' and F_BC_BGXH='" + bgxh + "' ", "dt_bc");

                    if (dt_bc == null || dt_bc.Rows.Count <= 0)
                    {
                        MessageBox.Show("CA查询数据失败"); return "1";
                    }
                    if (dt_bc.Rows[0]["F_CAGDZT"].ToString().Trim() == "已归档" || dt_bc.Rows[0]["F_CAGDZT"].ToString().Trim() == "1")
                    {
                        MessageBox.Show("此补充报告病案室已归档,不能进行修改。\r\n如需修改,请联系病案室取消归档。");
                        return "0";
                    }
                    return "1";

                }
                if (bglx == "bd")
                {
                    DataTable dt_bd = aa.GetDataTable("select * from  T_BDBG  where F_BLH='" + getblh + "' and F_BD_BGXH='" + bgxh + "' ", "dt_bd");

                    if (dt_bd == null || dt_bd.Rows.Count <= 0)
                    {
                        MessageBox.Show("CA查询数据失败");
                        return "1";
                    }
                    if (dt_bd.Rows[0]["F_CAGDZT"].ToString().Trim() == "已归档" || dt_bd.Rows[0]["F_CAGDZT"].ToString().Trim() == "1")
                    {
                        MessageBox.Show("此冰冻报告病案室已归档,不能进行修改。\r\n如需修改,请联系病案室取消归档。");
                        return "0";
                    }
                    return "1";
                }

                return "1";
            }
            return "1";
        }
    }
}
