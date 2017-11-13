using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using dbbase;
using PathHISZGQJK;
using ZgqClassPub;

namespace PathHISZGQJK
{
    // 南医三院体检接口

////    Create proc proc_get_bl_result
////@applyid varchar(50),    --申请单号
////@patientname varchar(50),  --姓名
////@Diagnosis varchar(3072),  --诊断结论
////@Feature varchar(1024),  --描述
////@reportdoctor varchar(50),  --报告医生姓名
////@reporttime datetime,  	--报告时间
////@filepath varchar(100),  	--图像绝对路径
////@sex varchar(50),  		--病人性别
////@age varchar(50) 			--年龄

    class nysy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string F_blh, string yymc)
        {
            string debug = f.ReadString("savetohis", "debug", "");
            string odbcsql = ZgqClass.GetSz("ZGQJK", "odbcsql", "Data Source=192.168.6.43;Initial Catalog=nysy_pe;User Id=sa;Password=sa;");
            string yhmc = f.ReadString("yh", "yhmc", "-").Replace("/0", "");

            string txpath = ZgqClass.GetSz("ZGQJK", "txpath", "");
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

             if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "体检")
                {
                    log.WriteMyLog("非体检病人，不处理！");
                    return;
                }
                if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("无体检申请单号，不处理！");
                    return;
                }
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {

                if (bljc.Rows[0]["F_TXML"].ToString().Trim() != "")
                {
                   
                        DataTable txlb = new DataTable();
                        txlb = aa.GetDataTable("select * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");


                        if (txlb.Rows.Count > 0)
                        {
                            if(txpath.Trim()=="")
                            txpath =   bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + txlb.Rows[0]["F_txm"].ToString().Trim();
                            else
                                 txpath =txpath+"\\"+   bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + txlb.Rows[0]["F_txm"].ToString().Trim();

                        }
                        else
                            txpath = "";
                }
                else
                    txpath = "";

                try
                {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + F_blh + "'", "blxx");
                       
                    
                       SqlParameter[] mzORAPAR = new SqlParameter[9];
                        for (int j = 0; j < mzORAPAR.Length; j++)
                        {
                            mzORAPAR[j] = new SqlParameter();
                        }
                        mzORAPAR[0].ParameterName = "@applyid";
                        mzORAPAR[0].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[0].Direction = ParameterDirection.Input;
                        mzORAPAR[0].Size = 50;

                        mzORAPAR[1].ParameterName = "@patientname";//
                        mzORAPAR[1].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[1].Direction = ParameterDirection.Input;
                        mzORAPAR[1].Size = 50;

                        mzORAPAR[2].ParameterName = "@Diagnosis";//
                        mzORAPAR[2].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[2].Direction = ParameterDirection.Input;
                        mzORAPAR[2].Size = 3000;

                        mzORAPAR[3].ParameterName = "@Feature";//
                        mzORAPAR[3].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[3].Direction = ParameterDirection.Input;
                        mzORAPAR[3].Size = 1024;

                        mzORAPAR[4].ParameterName = "@reportdoctor";// 
                        mzORAPAR[4].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[4].Direction = ParameterDirection.Input;
                        mzORAPAR[4].Size = 50;

                        mzORAPAR[5].ParameterName = "@reporttime";// 
                        mzORAPAR[5].SqlDbType = SqlDbType.DateTime;
                        mzORAPAR[5].Direction = ParameterDirection.Input;

                        mzORAPAR[6].ParameterName = "@filepath";// 
                        mzORAPAR[6].SqlDbType = SqlDbType.VarChar;;
                        mzORAPAR[6].Direction = ParameterDirection.Input;
                      mzORAPAR[6].Size = 100;

                        mzORAPAR[7].ParameterName = "@sex";// 
                        mzORAPAR[7].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[7].Direction = ParameterDirection.Input;
                        mzORAPAR[7].Size = 50;

                        mzORAPAR[8].ParameterName = "@age";// 
                        mzORAPAR[8].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[8].Direction = ParameterDirection.Input;
                        mzORAPAR[8].Size = 50;

                        //////////////////////////////////////////////////////

                        string tj_blzd = bljc.Rows[0]["F_blzd"].ToString().Trim();
                        string tj_jcsj = bljc.Rows[0]["F_jxsj"].ToString().Trim();

                        if (TJ_bljc.Rows.Count > 0)
                        {
                            if (bljc.Rows[0]["F_BGGS"].ToString().Trim() == "TBS" || bljc.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                            {
                                tj_jcsj =  "标本满意度:" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           "+TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           " + TJ_bljc.Rows[0]["F_tbs_xbxm1"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           " + TJ_bljc.Rows[0]["F_tbs_xbxm2"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           " + TJ_bljc.Rows[0]["F_tbs_xbxm3"].ToString().Trim() + "\r\n";

                                tj_jcsj = tj_jcsj + "炎症程度：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";
                               
                                ////////////////////////////////////
                                tj_blzd =  TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见1：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见2：" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\r\n";
                            }
                           
                            //if (bljc.Rows[0]["F_blk"].ToString().Trim() == "HPV")
                            //{
                            //    tj_jcsj = tj_jcsj + "检测方法：" + TJ_bljc.Rows[0]["F_FZ_JCFF"].ToString().Trim() + "\n" + "检测内容：" + TJ_bljc.Rows[0]["F_FZ_JCWD"].ToString().Trim() + "\n";
                            //    tj_blzd = "检测结果：" + TJ_bljc.Rows[0]["F_FZ_JCJG"].ToString().Trim();// "检查结果：" + TJ_bljc.Rows[0]["F_FZ_YYX"].ToString().Trim();
                            //}
                        }
                        if (debug == "1")
                        {
                            MessageBox.Show(tj_jcsj);
                            MessageBox.Show(tj_blzd);
                            MessageBox.Show(txpath);
                        }
                        /////////////////////////////////////////////////////
                        mzORAPAR[0].Value = bljc.Rows[0]["f_sqxh"].ToString().Trim();   //体检申请单号
                        mzORAPAR[1].Value = bljc.Rows[0]["f_xm"].ToString().Trim();  // 姓名
                        mzORAPAR[2].Value = tj_blzd.Trim();   // 诊断结论
                        mzORAPAR[3].Value = tj_jcsj.Trim();   //描述
                        mzORAPAR[4].Value = bljc.Rows[0]["f_bgys"].ToString().Trim();   // 检查医生姓名
                        mzORAPAR[5].Value = DateTime.Parse(bljc.Rows[0]["f_bgrq"].ToString().Trim());   //检查时间
                        mzORAPAR[6].Value = txpath;   // 图像绝对路径
                        mzORAPAR[7].Value = bljc.Rows[0]["f_xb"].ToString().Trim();   // 性别
                        mzORAPAR[8].Value = bljc.Rows[0]["f_nl"].ToString().Trim();   // 年龄
                        ////////////////////////////////////////////////////
                        SqlConnection ocn = new SqlConnection(odbcsql);
                        try
                        {
                           SqlCommand mzcmd = new SqlCommand();
                            mzcmd.Connection = ocn;
                            mzcmd.CommandType=CommandType.StoredProcedure;
                            mzcmd.CommandText = "proc_get_bl_result";
                            mzcmd.Parameters.Add(mzORAPAR[0]);
                            mzcmd.Parameters.Add(mzORAPAR[1]);
                            mzcmd.Parameters.Add(mzORAPAR[2]);
                            mzcmd.Parameters.Add(mzORAPAR[3]);
                            mzcmd.Parameters.Add(mzORAPAR[4]);
                            mzcmd.Parameters.Add(mzORAPAR[5]);
                            mzcmd.Parameters.Add(mzORAPAR[6]);
                            mzcmd.Parameters.Add(mzORAPAR[7]);
                            mzcmd.Parameters.Add(mzORAPAR[8]);
                      
                            try
                            {
                                ocn.Open();
                                if (debug == "1")
                                    MessageBox.Show("数据库打开正常");

                                mzcmd.ExecuteNonQuery();

                                if (debug == "1")
                                    MessageBox.Show("数据库执行正常,回写成功");

                                ocn.Close();
                             
                                ZgqClass.BGHJ(F_blh,yhmc, "审核","体检报告回写成功","ZGQJK","ZGQJK");
                            }
                            catch (Exception eee)
                            {
                                ocn.Close();
                                log.WriteMyLog("数据库执行异常，" + eee.Message);
                                if (debug == "1")
                                    MessageBox.Show("数据库执行异常：" + eee.Message.ToString());
                                ZgqClass.BGHJ(F_blh, yhmc, "审核", "体检报告回写失败：" + eee.Message, "ZGQJK", "ZGQJK");
                                return;
                            }
                            finally
                            {
                                if (ocn.State == ConnectionState.Open)
                                    ocn.Close();
                            }
                           
                        }
                        catch (Exception e)
                        {
                            log.WriteMyLog("参数错误："+e.Message.ToString());
                        }
                    }
                catch (Exception ee)
                {
                    log.WriteMyLog("参数错误2"+ee.Message.ToString());
                }

            }
        }  
    }
}
