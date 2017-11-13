using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using dbbase;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class qhdxfsyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string F_blh, string yymc)
        {
            string debug = f.ReadString("savetohis", "debug", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql", "Data Source=ORCL_100;User ID=jk_ycxd;Password=123;");
            string yhmc = f.ReadString("yh", "yhmc", "-").Replace("\0", "");

           
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

         
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请单号，不处理！");
                return;
            }

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已登记"|| bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已取材")
            {
                string err="";
                OracleDB oracledb = new OracleDB();
                    //通申请单号确认emr_jcsq_blmx
                string sqlstr1 = "update portal_his.emr_jcsq_blmx  set  qrsj=to_date('" + DateTime.Now + "','YYYY-MM-DD HH24:MI:SS'),qryg='" + yhmc.Trim() + "' where sqdh='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
              int x= oracledb.ExecuteNonQuery(odbcsql, sqlstr1, ref err);
              if (x <= 0)
              {
                  log.WriteMyLog("更改emr_jcsq_blmx失败:" + err.Trim() + "\r\n" + sqlstr1); return;
              }
              else
              {
                  string sqlstr2 = "update portal_his.emr_jcsq  set blh='" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "', jczt=3  where sqdh='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                  int y = oracledb.ExecuteNonQuery(odbcsql, sqlstr2, ref err);
                  if (y <= 0)
                  {
                      log.WriteMyLog("回传状态失败emr_jcsq:" + err.Trim() + "\r\n" + sqlstr2); return;
                  }
              }

            }

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {

                string err = "";
                OracleDB oracledb = new OracleDB();
                //通申请单号确认emr_jcsq_blmx
                string sqlstr = "update portal_his.emr_jcsq  set blh='" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "', jczt=7,jcsj=to_date('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS'),jcys='" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "',blzd='" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "' where sqdh='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                int x = oracledb.ExecuteNonQuery(odbcsql,sqlstr, ref err);
                if (x <= 0)
                {
                    log.WriteMyLog("回传结果失败emr_jcsq:" + err.Trim() + "\r\n" + sqlstr); return;
                }
                aa.ExecuteSQL("update  T_JCXX set F_SCBJ='1'  where F_BLH='" + F_blh + "'");

            }

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已写报告" && bljc.Rows[0]["F_SCBJ"].ToString().Trim() == "1")
            {

                string err = "";
                OracleDB oracledb = new OracleDB();
                //通申请单号确认emr_jcsq_blmx
                string sqlstr = "update portal_his.emr_jcsq  set blh='" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "', jczt=3,jcys='" + "" + "',blzd='" + "" + "' where sqdh='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                int x = oracledb.ExecuteNonQuery(odbcsql, sqlstr, ref err);
                if (x <= 0)
                {
                    log.WriteMyLog("回传结果失败emr_jcsq:" + err.Trim() + "\r\n" + sqlstr); return;
                }

            }
        }  
    }
}
