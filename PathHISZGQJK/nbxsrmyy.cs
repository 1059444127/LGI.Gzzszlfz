using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using dbbase;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class nbxsrmyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug, string msg)
        {
           
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            
            if (bljc == null)
            {
                MessageBox.Show("病理号：" + blh + ",病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号：" + blh + ",病理号有错误！");
                return;
            }
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog(blh + "申请序号为空，不回传！");
                return;
            }

//           p_yjqr（sqdh，czgh，bz，zxysh，return INT OUTPUT）
//功能：对病理检查申请单确认执行或取消执行
//    参数：
//    sqdh：申请单号，varchar(8)，对应视图字段V_pa_apply. APPLY_ID
//czgh：操作工号，varchar(10)，要求传入！
//bz：执行标志，integer 0为取消，1为执行，9为退费
//zxysh：执行医生 varchar(4)，bz为1时要求传入！
//    返回：return INT
//大于零为成功，小于零为失败
        //确费
          
               string odbcsql = f.ReadString("savetohis", "odbcsqlhis", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                string  yhmc=f.ReadString("yh", "yhmc", "").Replace("\0","").Trim();
                string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                SqlDB  sqldb=new SqlDB();

            //if(bljc.Rows[0]["F_mzqf"].ToString().Trim()!="1")
            //{
            //  SqlParameter []  Parameters=new SqlParameter[5];

            //  for (int j = 0; j < Parameters.Length; j++)
            //  {
            //      Parameters[j] = new SqlParameter();
            //  }
            //   Parameters[0].ParameterName = "sqdh";
            //   Parameters[0].SqlDbType = SqlDbType.VarChar;
            //   Parameters[0].Direction = ParameterDirection.Input;
            //   Parameters[0].Value=bljc.Rows[0]["F_SQXH"].ToString().Trim(); 
              
            //    Parameters[1].ParameterName = "czgh";
            //   Parameters[1].SqlDbType = SqlDbType.VarChar;
            //   Parameters[1].Direction = ParameterDirection.Input;
            //   Parameters[1].Value = yhbh; 

            //    Parameters[2].ParameterName = "bz";
            //   Parameters[2].SqlDbType = SqlDbType.Int;
            //   Parameters[2].Direction = ParameterDirection.Input;
            //   Parameters[2].Value=1; 

            //    Parameters[3].ParameterName = "zxysh";
            //   Parameters[3].SqlDbType = SqlDbType.VarChar;
            //   Parameters[3].Direction = ParameterDirection.Input;
            //   Parameters[3].Value=yhmc;

            //   Parameters[4].ParameterName = "return";
            //   Parameters[4].SqlDbType = SqlDbType.Int;
            //   Parameters[4].Direction = ParameterDirection.Output;

              
            //    string except="";
            //    sqldb.ExecuteNonQuery(odbcsql,"p_yjqr",ref Parameters,CommandType.StoredProcedure,ref except);
            //    if (except != "")
            //        MessageBox.Show("确认费用失败：" + except);
            //    else
            //    {
            //        try
            //        {
            //            if (int.Parse(Parameters[4].Value.ToString()) < 0)
            //            {
            //                MessageBox.Show("确认费用失败:" + Parameters[4].Value);
            //            }
            //            else
            //            {
            //                aa.ExecuteSQL("update T_JCXX set F_MZQF='1' where F_BLH='" + blh + "'");
            //                if (debug == "1")
            //                    MessageBox.Show("确认费用成功");
            //            }
            //        }
            //        catch(Exception  ee1)
            //        {
            //            MessageBox.Show("确认费用异常："+ee1.Message);
            //        }
            //    }
            //}





            //            p_blsqd_zt（sqdh，ztmc，ztrq，czgh,  mark, return INT OUTPUT）
            //功能：将病理检查单的最近状态返回给HIS
            //参数：
            //sqdh ：申请单号varchar(8), 对应视图字段V_pa_apply. APPLY_ID
            //ztmc：状态名称 varchar(32), 如：已登记/已取材处理/已包埋/已制片/报告延期/已写报告/已审核
            //ztrq：状态操作日期 datetime
            //czgh：操作工号，varchar(10)，要求传入！
            //mark：  备注varchar(80) 可传空
            //    result：病理结果，varchar(600)
            //返回：return INT
            //大于零为成功，小于零为失败  
        //回状态
         
            SqlParameter[] Parameters_2 = new SqlParameter[7];
            for (int j = 0; j < Parameters_2.Length; j++)
            {
                Parameters_2[j] = new SqlParameter();
            }
            Parameters_2[0].ParameterName = "sqdh";
            Parameters_2[0].SqlDbType = SqlDbType.VarChar;
            Parameters_2[0].Direction = ParameterDirection.Input;
            Parameters_2[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();
       
            Parameters_2[1].ParameterName = "ztmc";
            Parameters_2[1].SqlDbType = SqlDbType.VarChar;
            Parameters_2[1].Direction = ParameterDirection.Input;
            string bgzt=bljc.Rows[0]["F_BGZT"].ToString().Trim();
            if(bgzt=="已取材")  bgzt="已取材处理";
            Parameters_2[1].Value = bgzt.Trim();

            Parameters_2[2].ParameterName = "ztrq";
            Parameters_2[2].SqlDbType = SqlDbType.DateTime;
            Parameters_2[2].Direction = ParameterDirection.Input;
            Parameters_2[2].Value = DateTime.Now;
     
            Parameters_2[3].ParameterName = "czgh";
            Parameters_2[3].SqlDbType = SqlDbType.VarChar;
            Parameters_2[3].Direction = ParameterDirection.Input;
            Parameters_2[3].Value = yhbh;


            Parameters_2[4].ParameterName = "mark";
            Parameters_2[4].SqlDbType = SqlDbType.VarChar;
            Parameters_2[4].Direction = ParameterDirection.Input;
            Parameters_2[4].Value = bljc.Rows[0]["F_WFBGYY"].ToString().Trim();

            Parameters_2[5].ParameterName = "blxh";
            Parameters_2[5].SqlDbType = SqlDbType.VarChar;
            Parameters_2[5].Direction = ParameterDirection.Input;
            Parameters_2[5].Value = blh;

            Parameters_2[6].ParameterName = "return";
            Parameters_2[6].SqlDbType = SqlDbType.Int;
            Parameters_2[6].Direction = ParameterDirection.Output;
           
        
            string except2 = "";
          
            sqldb.ExecuteNonQuery(odbcsql, "p_blsqd_zt", ref Parameters_2, CommandType.StoredProcedure, ref except2);
     
            if (except2 != "")
            {
                if(debug=="1")
                    MessageBox.Show("连接数据库失败，状态回写失败:" + except2);

            }
            else
            {
                try
                {
                    if (int.Parse(Parameters_2[6].Value.ToString()) < 0)
                    {
                        if (debug == "1")
                            MessageBox.Show("状态回写失败:" + Parameters_2[6].Value);
                    }
                    else
                    {
                      
                        if (debug == "1")
                            MessageBox.Show("状态回写成功");
                    }
                }
                catch (Exception ee2)
                {
                    if (debug == "1")
                    MessageBox.Show("状态回写异常：" + ee2.Message);
               
                }
            }


            //回报告
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {

//p_blsqd_zdjg （sqdh，czgh，result，return INT OUTPUT）
//功能：将病理检查单的诊断结果返回给HIS
//参数：
//sqdh ：申请单号varchar(8), 对应视图字段V_pa_apply. APPLY_ID
//czgh：操作工号，varchar(10)，要求传入！
//    result：病理结果，varchar(600)
//返回：return INT
//大于零为成功，小于零为失败

                //DataTable bljc_bc = new DataTable();
                //bljc_bc = aa.GetDataTable("select * from T_bcbg where F_BLH='" + blh + "' and F_BC_BGZT='已审核'", "bcbg");
                string rysj = bljc.Rows[0]["F_RYSJ"].ToString().Trim();
                if (rysj.Trim() != "")
                    rysj = ("肉眼所见：" + rysj + " \r\n ");

                string jxsj = bljc.Rows[0]["F_JXSJ"].ToString().Trim();
                if (jxsj.Trim() != "")
                    jxsj = ("镜下所见：" + jxsj + " \r\n ");

                string tsjc = bljc.Rows[0]["F_TSJC"].ToString().Trim();
                if (tsjc.Trim() != "")
                    tsjc = ("特殊检查：" + tsjc + "\r\n");
                string zd=rysj+jxsj+tsjc+bljc.Rows[0]["F_BLZD"].ToString().Trim();

                SqlParameter[] Parameters_bg = new SqlParameter[4];
                for (int j = 0; j < Parameters_bg.Length; j++)
                {
                    Parameters_bg[j] = new SqlParameter();
                }

                Parameters_bg[0].ParameterName = "sqdh";
                Parameters_bg[0].SqlDbType = SqlDbType.VarChar;
                Parameters_bg[0].Direction = ParameterDirection.Input;
                Parameters_bg[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                Parameters_bg[1].ParameterName = "czgh";
                Parameters_bg[1].SqlDbType = SqlDbType.VarChar;
                Parameters_bg[1].Direction = ParameterDirection.Input;
                Parameters_bg[1].Value = yhbh;

                Parameters_bg[2].ParameterName = "result";
                Parameters_bg[2].SqlDbType = SqlDbType.VarChar;
                Parameters_bg[2].Direction = ParameterDirection.Input;
                Parameters_bg[2].Value = zd.Trim();

                Parameters_bg[3].ParameterName = "return";
                Parameters_bg[3].SqlDbType = SqlDbType.Int;
                Parameters_bg[3].Direction = ParameterDirection.Output;


                 except2 = "";
                sqldb.ExecuteNonQuery(odbcsql, "p_blsqd_zdjg", ref Parameters_bg, CommandType.StoredProcedure, ref except2);

                if (except2 != "")
                {
                    if (debug == "1")
                        MessageBox.Show("连接数据库失败，报告回写失败:" + except2);
                    log.WriteMyLog("连接数据库失败，报告回写失败:" + except2);
                }
                else
                {
                    try
                    {
                        if (int.Parse(Parameters_bg[3].Value.ToString()) < 0)
                        {
                            if (debug == "1")
                                MessageBox.Show("报告回写失败:" + Parameters_bg[3].Value);
                            log.WriteMyLog("报告回写失败:" + Parameters_bg[3].Value);
                        }
                        else
                        {
                         aa.ExecuteSQL("update T_JCXX set F_SCBJ='1' where F_BLH='" + blh + "'");
                            if (debug == "1")
                                MessageBox.Show("报告回写成功");
                        }
                    }
                    catch (Exception ee2)
                    {
                        if (debug == "1")
                            MessageBox.Show("报告回写异常：" + ee2.Message);
                         log.WriteMyLog("报告回写异常：" + ee2.Message);
                    }
                }
               





            }

        }

        //private string  get_yhgh(string F_yhmc)
        //{
        //    try
        //    {
        //        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        //        DataTable bljc = new DataTable();
        //        bljc = aa.GetDataTable("select top 1 F_YHBH from T_YH where F_YHMC='" + F_yhmc + "'", "bljc");
        //        if (bljc.Rows.Count > 0)
        //            return bljc.Rows[0][0].ToString();
        //        else
        //            return "";
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}
    }
}
