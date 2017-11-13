using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Windows.Forms;
using dbbase;
using System.Data;
using ZgqClassPub;

namespace PathHISZGQJK
{
    /// <summary>
    ///  海南那大农垦医院
    /// </summary>
    class hnndnkyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        string msg = f.ReadString("savetohis", "msg", "1");
        string odbcsql = f.ReadString("savetohis", "odbcsql","DSN=pathnet-his;UID=db2admin;PWD=123");

        public void pathtohis(string blh, string yymc)
        {


          
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
            if (bljc.Rows[0]["F_brlb"].ToString().Trim().Contains("体检"))
            {
                log.WriteMyLog("体检病人，不处理！");
                return;
            }
            string  hxbz=bljc.Rows[0]["F_hxbz"].ToString().Trim();
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" )
            {
                string F_brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
                if (F_brlb == "住院")
                    F_brlb = "2";
                else
                    F_brlb = "1";

                 string select_sql="select F_BLH from DB2ADMIN.BL_COMMONREPORT where F_BLH='"+blh+"'";
                 int  count = Odbc_Select(select_sql);
                 if (count == -1)
                 {
                     return;
                 }
                 string str_sql = "";
                 if (count>0)
                 {
                     str_sql = "update DB2ADMIN.BL_COMMONREPORT set  F_BGLX='" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "',F_XM='" + bljc.Rows[0]["F_XM"].ToString().Trim() + "',F_XB='" + bljc.Rows[0]["F_XB"].ToString().Trim() + "',F_NL='" + bljc.Rows[0]["F_NL"].ToString().Trim() + "',F_CH='" +
                         bljc.Rows[0]["F_CH"].ToString().Trim() + "',F_BBMC='" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "',F_RYSJ='" + bljc.Rows[0]["F_RYSJ"].ToString().Trim() + "',F_GJSJWZ='" + bljc.Rows[0]["F_jxsj"].ToString().Trim() + "',F_BLZD='" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "',F_BGYS='" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "',F_FZYS='"
                       + bljc.Rows[0]["F_FZYS"].ToString().Trim() + "',F_BGRQ=timestamp('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),F_BGZT='1',F_SHRQ=timestamp('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),F_YXYX='" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "'  where F_BLH='" + blh + "'";
                 }
                 else
                 {
                     if (F_brlb == "2")
                     {// 住院-住院次数 int
                         int zycs = 1;
                         try
                         {
                             zycs = int.Parse(bljc.Rows[0]["F_BY2"].ToString().Trim());
                         }
                         catch
                         {
                         }
                         str_sql = "insert into DB2ADMIN.BL_COMMONREPORT( F_BLH,F_BGLX,F_XM,F_XB,F_NL,F_CH,F_SQDH,F_BRLX,F_ZYH,F_ZYCS,F_MZH,F_ZLH,F_BBMC,F_SJRQ,F_SJDW,F_SJKS,F_LCZD,F_RYSJ,F_GJSJWZ,F_SFYTP,F_BDZD,F_BLZD,F_BGYS,F_FZYS,F_SHYS,F_BGRQ,F_SHRQ,F_BGZT,F_YXYX,F_DYCS)" +
                             "values('" + blh + "','" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "','" + bljc.Rows[0]["F_XM"].ToString().Trim() + "','" + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_NL"].ToString().Trim() + "','" + bljc.Rows[0]["F_CH"].ToString().Trim() +
                             "','" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','" + F_brlb + "','" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "'," + zycs + ",'" + bljc.Rows[0]["F_MZH"].ToString().Trim() + "','" + bljc.Rows[0]["F_YZID"].ToString().Trim() +
                             "','" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "',timestamp('" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),'" + bljc.Rows[0]["F_SJDW"].ToString().Trim() + "','" + bljc.Rows[0]["F_SJKS"].ToString().Trim() + "','" + bljc.Rows[0]["F_lczd"].ToString().Trim() +
                             "','" + bljc.Rows[0]["F_RYSJ"].ToString().Trim() + "','" + bljc.Rows[0]["F_JXSJ"].ToString().Trim() + "',0,'" + "" + "','" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_FZYS"].ToString().Trim() +
                             "','" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "',timestamp('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),timestamp('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),'" + "1" + "','" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "',0)";
                     }
                     else
                     {
                         //门诊 无住院次数

                         str_sql = "insert into DB2ADMIN.BL_COMMONREPORT( F_BLH,F_BGLX,F_XM,F_XB,F_NL,F_CH,F_SQDH,F_BRLX,F_ZYH,F_MZH,F_ZLH,F_BBMC,F_SJRQ,F_SJDW,F_SJKS,F_LCZD,F_RYSJ,F_GJSJWZ,F_SFYTP,F_BDZD,F_BLZD,F_BGYS,F_FZYS,F_SHYS,F_BGRQ,F_SHRQ,F_BGZT,F_YXYX,F_DYCS)" +
                          "values('" + blh + "','" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "','" + bljc.Rows[0]["F_XM"].ToString().Trim() + "','" + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_NL"].ToString().Trim() + "','" + bljc.Rows[0]["F_CH"].ToString().Trim() +
                          "','" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','" + F_brlb + "','" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "','" + bljc.Rows[0]["F_MZH"].ToString().Trim() + "','" + bljc.Rows[0]["F_YZID"].ToString().Trim() +
                          "','" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "',timestamp('" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),'" + bljc.Rows[0]["F_SJDW"].ToString().Trim() + "','" + bljc.Rows[0]["F_SJKS"].ToString().Trim() + "','" + bljc.Rows[0]["F_lczd"].ToString().Trim() +
                          "','" + bljc.Rows[0]["F_RYSJ"].ToString().Trim() + "','" + bljc.Rows[0]["F_JXSJ"].ToString().Trim() + "',0,'" + "" + "','" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_FZYS"].ToString().Trim() +
                          "','" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "',timestamp('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),timestamp('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd-HH.mm.ss") + "'),'" + "1" + "','" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "',0)";
                    
                     }
                     }
                bool rtn_1= Odbc_Exec(str_sql);
                if (rtn_1)
                    aa.ExecuteSQL("update T_JCXX set F_HXBZ='2' where F_BLH='"+blh+"'");
               
              
            }
            else
            {
                if (bljc.Rows[0]["F_hxbz"].ToString().Trim() == "2")
                {
                    string str_sql = "update DB2ADMIN.BL_COMMONREPORT set F_BGZT='0',F_BGCXRQ=timestamp('" + DateTime.Now.ToString("yyyy-MM-dd-HH.mm.ss") + "')  where  F_BLH='" + blh + "'";
                 Odbc_Exec(str_sql);
                }
            }

         


        }
        public bool Odbc_Exec(string sqlstr)
        {
        
            OdbcConnection sqlcon = new OdbcConnection();
            OdbcCommand sqlcom = new OdbcCommand();
            try
            {
                sqlcon.ConnectionString = odbcsql;
                sqlcom.Connection = sqlcon;
                sqlcom.CommandText = sqlstr;
                sqlcon.Open();
              int  x = sqlcom.ExecuteNonQuery();
                if(x>0)
                return true;
                else
                return false;
            }
            catch (Exception e)
            {
                if (msg == "1") MessageBox.Show("执行数据库异常："+e.ToString());
                log.WriteMyLog("执行数据库异常，" + sqlstr + "," + odbcsql + "," + e.ToString());
                return false;
                
            }
            finally
            {
                sqlcom.Dispose();
                sqlcon.Close();
                sqlcon.Dispose();
                

            }
        }

        public int  Odbc_Select(string sqlstr)
        {

            OdbcConnection sqlcon = new OdbcConnection();
           // OdbcCommand sqlcom = new OdbcCommand();
            try
            {
                sqlcon.ConnectionString = odbcsql;
                OdbcDataAdapter oda = new OdbcDataAdapter(sqlstr, sqlcon);
                DataTable dt = new DataTable();
                oda.Fill(dt);

                return dt.Rows.Count;
            }
            catch (Exception e)
            {
                if (msg == "1") MessageBox.Show("查询数据库异常：" + e.ToString());
                log.WriteMyLog("查询数据库异常，" + sqlstr + "," + odbcsql + "," + e.ToString());
   return -1;
            }
            finally
            {
               
                sqlcon.Close();
                sqlcon.Dispose();
             

            }
        }
    }
}
