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
    class bjyhyy  //北京燕化医院
    {
        private static IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {

            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
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

            // if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") patid = bljc.Rows[0]["F_zyh"].ToString().Trim();
            //if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") patid = bljc.Rows[0]["F_mzh"].ToString().Trim();

            //if (patid == "")
            //{
            //     LGZGQClass.log.WriteMyLog("非住院病人，不处理！");
            //    return;

            string sqlconstr = f.ReadString("savetohis", "odbcsql", "");
            string sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
            
            string  count = sql_db("select * from Status where HISSheetID='" + sqxh + "'", sqlconstr,true); //此申请单号记录是否存在
            if (count == "-1") return;
            if (int.Parse(count) >= 1)    //更新状态表中的记录
           {
               string sqlstr = "update  Status  set  StatusDateTime='" + DateTime.Now.ToString("YYYYMMDDHHMMSS") + "',Status='";
               string Status = "1";
              
               if (zt == "已审核")
               {
                   string Reportstr = "";
                
                   string xxx = sql_db("select *  from Report where HISSheetID='" + sqxh + "'", sqlconstr,true);
                   if (xxx == "-1") return;
                   if (int.Parse(xxx) >= 1)//更新Report（报告表）
                   {
                       Reportstr = "update  Report set  ReportID='" + bljc.Rows[0]["F_blh"].ToString().Trim() + "',ExamTechDesc='"+bljc.Rows[0]["F_rysj"].ToString().Trim()+"',ImageDesc='"+bljc.Rows[0]["F_jxsj"].ToString().Trim()+"',DiagnoseResult='"+
                           bljc.Rows[0]["F_blzd"].ToString().Trim() + "',ReportDoctor='" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "',ReportDateTime='" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMddHHMMss") + "',VerifyDoctor='" + bljc.Rows[0]["F_shys"].ToString().Trim() + "',VerityDateTime='" +
                           DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMddHHMMss") + "',ImageURL='http://10.101.114.7/pathwebrpt/index_y.asp?yzh='+'" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'  where  HISSheetID='" + sqxh + "'";
                   }
                   else             //添加记录
                   {
                       Reportstr = "insert into  Report (HISSheetID,ReportID,ExamTechDesc,ImageDesc,DiagnoseResult,ReportDoctor,ReportDateTime,VerifyDoctor,VerityDateTime,ImageURL,ReportImage)"
                           + "values ('" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "','" + bljc.Rows[0]["F_blh"].ToString().Trim() + "','" + bljc.Rows[0]["F_rysj"].ToString().Trim()
                           + "','" + bljc.Rows[0]["F_jxsj"].ToString().Trim() + "','" + bljc.Rows[0]["F_blzd"].ToString().Trim() + "','" + bljc.Rows[0]["F_bgys"].ToString().Trim() +
                           "','" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMddHHMMss") + "','" + bljc.Rows[0]["F_shys"].ToString().Trim() + "','" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMddHHMMss") + "','http://10.101.114.7/pathwebrpt/index_y.asp?yzh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "','')  ";
                   }
                  string xx= sql_db(Reportstr, sqlconstr,false);
                  if (xx == "-1")  return;
                  if (msg == "1") log.WriteMyLog("回传hisReport（报告表）成功，申请单号：" + sqxh + "影响行数" + xx);
                   Status = "7";
               }
               if (zt == "已写报告")
               {
                   string Reportcount = sql_db("select * from Report where HISSheetID='" + sqxh + "'", sqlconstr, true); //此申请单号记录是否存在
                   if (count == "-1") return;
                   if (int.Parse(Reportcount) > 0)
                   {
                       string Reportstr = "delete  from   Report where HISSheetID='" + sqxh + "'";
                        string xx= sql_db(Reportstr, sqlconstr,false);
                        if (msg == "1") log.WriteMyLog("删除hisReport（报告表）成功，申请单号：" + sqxh + "影响行数" + xx);
                   }
                   Status = "6";
               }
                    sqlstr = sqlstr + Status + "'  where HISSheetID='" + sqxh + "'";
                    string xc=sql_db(sqlstr, sqlconstr,false);//更新状态
                    if (msg == "1") log.WriteMyLog("更新Status（状态表）成功，申请单号：" + sqxh + "影响行数" + xc);
                    return;
                       
           }
           else              //在状态表中添加记录
           {
               
               string sqlstr = "insert into Status (HISSheetID,StatusDateTime,Status) values ('";
               sqlstr = sqlstr + sqxh + "','" + DateTime.Now.ToString("yyyyMMddHHMMss") + "','1')";
              string xx= sql_db(sqlstr, sqlconstr,false);
               if (msg == "1") log.WriteMyLog("插入Status（状态表）成功，申请单号：" + sqxh + "影响行数" + xx);
               return;
               
           }
            
            
        
        }
        public string sql_db(string sqlstr, string sqlconstr,bool yy)   
        {
            int x = 0;
            string msg = f.ReadString("savetohis", "msg", "");
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcom = new SqlCommand();
            try
            {
                sqlcon.ConnectionString = sqlconstr;
                sqlcom.Connection = sqlcon;
                sqlcom.CommandText = sqlstr;
                sqlcon.Open();
                if (yy)
                {
                    try
                    {
                        SqlDataReader sqlr = sqlcom.ExecuteReader();
                        while (sqlr.Read())
                            x = x + 1;
                        sqlr.Close(); sqlr.Close();
                    }
                    catch (Exception ee)
                    {
                        if (msg == "1") MessageBox.Show(ee.ToString());

                        log.WriteMyLog("回传his参数失败，" + sqlstr + "," + sqlconstr + "," + ee.ToString());
                    }

                }
                else
                x = sqlcom.ExecuteNonQuery();
               
               return x.ToString();
            }
            catch(Exception e) {
                if (msg == "1") MessageBox.Show(e.ToString());

                log.WriteMyLog("回传his参数失败，" + sqlstr + "," + sqlconstr+","+e.ToString());
              
                return "-1";
            }
            finally
            {sqlcom.Dispose();
                sqlcon.Close();
                sqlcon.Dispose();
                
            }


        }

    }
}
