using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using dbbase;
using ZgqClassPub;
using ZgqClassPub.DBData;


namespace PathHISZGQJK
{
    class hnhhsy
    {
        ///湖南怀化第三人民医院
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug)
        {

            debug = f.ReadString("savetohis", "debug", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql", "Data Source=.;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p; ");
            string err_msg = "";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

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

            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog("申请序号为空不处理！");
                ZgqClass.BGHJ(blh, "HIS接口", "审核", "申请序号为空不处理", "ZGQJK", "HIS接口");
                return;
            }

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已登记" || bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已取材"|| bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已写报告")
            {
              

                if (bljc.Rows[0]["F_HXBJ"].ToString().Trim() == "1")
                    return;

                //if (bljc.Rows[0]["F_HXBJ"].ToString().Trim() != "2")
                //    return;

                SqlDB db = new SqlDB();
                try
                {
                    SqlParameter[] oledbPt = new SqlParameter[2];

                    for (int j = 0; j < oledbPt.Length; j++)
                    {
                        oledbPt[j] = new SqlParameter();
                    }
                    oledbPt[0].ParameterName = "OrderID";
                    oledbPt[0].SqlDbType = SqlDbType.VarChar;
                    oledbPt[0].Direction = ParameterDirection.Input;
                    oledbPt[0].Size = 32;
                    oledbPt[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                    oledbPt[1].ParameterName = "OrderStatus";//
                    oledbPt[1].SqlDbType = SqlDbType.Int;
                    oledbPt[1].Direction = ParameterDirection.Input;
                    oledbPt[1].Value = 1;

                  
                     int x=db.ExecuteNonQuery(odbcsql, "SP_SyncOrderStatusToHIS", ref oledbPt, CommandType.StoredProcedure, ref err_msg);

                    if (x < 0)
                    {
                        log.WriteMyLog("执行回写标记异常：" + err_msg);
                        ZgqClass.BGHJ(blh, "HIS接口", "保存", "执行回写标记异常：" + err_msg, "ZGQJK", "HIS接口");
                    }
                    else
                    {
                        ZgqClass.BGHJ(blh, "HIS接口", "保存", "执行回写标记正常", "ZGQJK", "HIS接口");
                           aa.ExecuteSQL("update  T_JCXX  set  F_HXBJ='1' where F_BLH='"+bljc.Rows[0]["F_BLH"].ToString().Trim()+"'");
                    }
                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message); return;
                }
            }
            else
                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                {
                    SqlDB db = new SqlDB();


                    try
                    {
                        SqlParameter[] oledbPt = new SqlParameter[2];

                        for (int j = 0; j < oledbPt.Length; j++)
                        {
                            oledbPt[j] = new SqlParameter();
                        }
                        oledbPt[0].ParameterName = "OrderID";
                        oledbPt[0].SqlDbType = SqlDbType.VarChar;
                        oledbPt[0].Direction = ParameterDirection.Input;
                        oledbPt[0].Size = 32;
                        oledbPt[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                        oledbPt[1].ParameterName = "OrderStatus";//
                        oledbPt[1].SqlDbType = SqlDbType.Int;
                        oledbPt[1].Direction = ParameterDirection.Input;
                        oledbPt[1].Value = 2;
                       
                        int x = db.ExecuteNonQuery(odbcsql, "SP_SyncOrderStatusToHIS", ref oledbPt, CommandType.StoredProcedure, ref err_msg);
                       
                        if (x < 0)
                        {
                            log.WriteMyLog("执行回写审核标记异常：" + err_msg);
                            ZgqClass.BGHJ(blh, "HIS接口", "审核", "执行回写审核标记异常：" + err_msg, "ZGQJK", "HIS接口");
                        }
                        else
                        {
                            ZgqClass.BGHJ(blh, "HIS接口", "审核", "执行回写审核标记正常", "ZGQJK", "HIS接口");
                        }
                     
                    }
                    catch (Exception e)
                    {
                        log.WriteMyLog(e.Message);
                        return;
                    }
                    //回写报告
                    try
                    {
                        SqlParameter[] oledbPt = new SqlParameter[6];

                        for (int j = 0; j < oledbPt.Length; j++)
                        {
                            oledbPt[j] = new SqlParameter();
                        }
                        oledbPt[0].ParameterName = "OrderID";
                        oledbPt[0].SqlDbType = SqlDbType.VarChar;
                        oledbPt[0].Direction = ParameterDirection.Input;
                        oledbPt[0].Size = 32;
                        oledbPt[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                        oledbPt[1].ParameterName = "ReportGuid";
                        oledbPt[1].SqlDbType = SqlDbType.VarChar;
                        oledbPt[1].Direction = ParameterDirection.Input;
                        oledbPt[1].Size = 128;
                        oledbPt[1].Value = bljc.Rows[0]["F_BLH"].ToString().Trim();

                        oledbPt[2].ParameterName = "WYSText";
                        oledbPt[2].SqlDbType = SqlDbType.VarChar;
                        oledbPt[2].Direction = ParameterDirection.Input;
                        oledbPt[2].Size = 4000;
                        oledbPt[2].Value = bljc.Rows[0]["F_RYSJ"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_JXSJ"].ToString().Trim();

                        oledbPt[3].ParameterName = "WYGText";
                        oledbPt[3].SqlDbType = SqlDbType.VarChar;
                        oledbPt[3].Direction = ParameterDirection.Input;
                        oledbPt[3].Size = 4000;
                        oledbPt[3].Value = bljc.Rows[0]["F_blzd"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_tsjc"].ToString().Trim();

                        oledbPt[4].ParameterName = "PatientID";
                        oledbPt[4].SqlDbType = SqlDbType.VarChar;
                        oledbPt[4].Direction = ParameterDirection.Input;
                        oledbPt[4].Size = 50;
                        oledbPt[4].Value = bljc.Rows[0]["F_BRBH"].ToString().Trim() ;

                        oledbPt[5].ParameterName = "CheckedDate";
                        oledbPt[5].SqlDbType = SqlDbType.VarChar;
                        oledbPt[5].Direction = ParameterDirection.Input;
                        oledbPt[5].Size = 50;
                        oledbPt[5].Value = bljc.Rows[0]["F_SPARE5"].ToString().Trim();
                     
                        int x = db.ExecuteNonQuery(odbcsql, "SP_SyncApprovedReportToHIS", ref oledbPt, CommandType.StoredProcedure, ref err_msg);
                   
                        if (x < 0)
                        {
                            log.WriteMyLog("执行回写审核报告异常：" + err_msg);
                            ZgqClass.BGHJ(blh, "HIS接口", "审核", "执行回写审核报告异常：" + err_msg, "ZGQJK", "HIS接口");
                        }
                        else
                        {
                            ZgqClass.BGHJ(blh, "HIS接口", "审核", "执行回写审核报告正常", "ZGQJK", "HIS接口");
                             aa.ExecuteSQL("update  T_JCXX  set  F_HXBJ='2' where F_BLH='"+bljc.Rows[0]["F_BLH"].ToString().Trim()+"'");
                        }
                        return;
                    }
                    catch (Exception e)
                    {
                        log.WriteMyLog(e.Message); return;
                    }

                }

        }
    }
}
