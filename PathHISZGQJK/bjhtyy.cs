using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.Odbc;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class bjhtyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void bjhtyy_hx(string F_blh, string yymc)
        {
            string debug = f.ReadString("savetohis", "debug", "");
            if (debug == "1")
            {
              //  MessageBox.Show("ZGQ接口回写开始");
              
            }

            string usr = f.ReadString("bjhtyy", "user", "interfacepat");
            string pwd = f.ReadString("bjhtyy", "pwd", "INTERFACEPAT");
            string dsn = f.ReadString("bjhtyy", "dsn", "pathnet-his");
            string connstring = "DSN=" + dsn + ";uid=" + usr + ";pwd=" + pwd + "";

          
            string odbcsql = connstring;// f.ReadString("savetohis", "odbcsql", "DSN=pathnet-his;UID=interfacepat;PWD=INTERFACEPAT");
     
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", ""); 
            DataTable jcxx = new DataTable();    
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx.Rows.Count > 0  && jcxx.Rows[0]["f_sqxh"].ToString().Trim() !="")
            {
                if (debug == "1")
                {
                    MessageBox.Show("ZGQ接口回写状态");

                }

                string brlb = "";
                string ztbz = "";
                string sqxh = jcxx.Rows[0]["f_sqxh"].ToString().Trim();
                if (jcxx.Rows[0]["f_zyh"].ToString().Trim() != "" || jcxx.Rows[0]["f_mzh"].ToString().Trim() != "")
                {

                    if (jcxx.Rows[0]["f_zyh"].ToString().Trim() != "")
                    {
                        brlb = "住院";
                    }
                    if (jcxx.Rows[0]["f_mzh"].ToString().Trim() != "")
                    {
                        brlb = "门诊";
                    }
                }
                else
                {
                    if(jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
                        brlb = "体检";
                    else
                    brlb = "门诊";
                }
                if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "已审核")
                {

                    if (jcxx.Rows[0]["F_HXZT"].ToString().Trim() == "" || jcxx.Rows[0]["F_HXZT"].ToString().Trim() == "2")
                    {
                        ztbz = "1";
                        oper_sqd(F_blh,brlb, ztbz,sqxh,debug);
                        aa.ExecuteSQL("update t_jcxx set f_HXZT='1' where f_blh='" + F_blh + "'");
                    }
                  
                }
                else
                {


                        ztbz = "2";
                        oper_sqd(F_blh, brlb, ztbz, sqxh, debug);
                        aa.ExecuteSQL("update t_jcxx set f_HXZT='2' where f_blh='" + F_blh + "'");
                     
                }

            
                ////增加体检回写接口/////zgq
                try
                {
                    if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "体检" && jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                    {
                         //if (debug == "1")
                         //   MessageBox.Show("体检报告回传");
                    
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + F_blh + "'", "blxx");
                        OdbcParameter[] mzORAPAR = new OdbcParameter[11];
                        for (int j = 0; j < mzORAPAR.Length; j++)
                        {
                            mzORAPAR[j] = new OdbcParameter();
                        }
                        mzORAPAR[0].ParameterName = "Sequenceno";
                        mzORAPAR[0].OdbcType = OdbcType.VarChar;
                        mzORAPAR[0].Direction = ParameterDirection.Input;
                        mzORAPAR[0].Size = 50;

                        mzORAPAR[1].ParameterName = "CheckDescription";//
                        mzORAPAR[1].OdbcType = OdbcType.VarChar;
                        mzORAPAR[1].Direction = ParameterDirection.Input;
                        mzORAPAR[1].Size = 1000;

                        mzORAPAR[2].ParameterName = "CheckConclusion";//
                        mzORAPAR[2].OdbcType = OdbcType.VarChar;
                        mzORAPAR[2].Direction = ParameterDirection.Input;
                        mzORAPAR[2].Size = 1000;

                        mzORAPAR[3].ParameterName = "Checkdoccode";//
                        mzORAPAR[3].OdbcType = OdbcType.VarChar;
                        mzORAPAR[3].Direction = ParameterDirection.Input;
                        mzORAPAR[3].Size = 200;

                        mzORAPAR[4].ParameterName = "Checkdocname";// 
                        mzORAPAR[4].OdbcType = OdbcType.VarChar;
                        mzORAPAR[4].Direction = ParameterDirection.Input;
                        mzORAPAR[4].Size = 200;

                        mzORAPAR[5].ParameterName = "Checkdate";// 
                        mzORAPAR[5].OdbcType = OdbcType.Date;
                        mzORAPAR[5].Direction = ParameterDirection.Input;

                        mzORAPAR[6].ParameterName = "Picture_Qty";// 
                        mzORAPAR[6].OdbcType = OdbcType.Int;
                        mzORAPAR[6].Direction = ParameterDirection.Output;

                        mzORAPAR[7].ParameterName = "Err_code";// 
                        mzORAPAR[7].OdbcType = OdbcType.Int;
                        mzORAPAR[7].Direction = ParameterDirection.Output;

                        mzORAPAR[8].ParameterName = "Err_text";// 
                        mzORAPAR[8].OdbcType = OdbcType.VarChar;
                        mzORAPAR[8].Direction = ParameterDirection.Output;
                        mzORAPAR[8].Size = 200;

                        mzORAPAR[9].ParameterName = "AuditOpercode";// 
                        mzORAPAR[9].OdbcType = OdbcType.VarChar;
                        mzORAPAR[9].Direction = ParameterDirection.Input;
                        mzORAPAR[9].Size = 200;

                        mzORAPAR[10].ParameterName = "AuditOpername";// 
                        mzORAPAR[10].OdbcType = OdbcType.VarChar;
                        mzORAPAR[10].Direction = ParameterDirection.Input;
                        mzORAPAR[10].Size = 200;
                        //////////////////////////////////////////////////////
                      
                        string tj_blzd = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                        string tj_jcsj = "";

                     
                        if (jcxx.Rows[0]["F_xb"].ToString().Trim() != "男")
                        { 
                            string mcyj = jcxx.Rows[0]["F_mcyj"].ToString().Trim();
                            string sfjj = jcxx.Rows[0]["F_sfjj"].ToString().Trim();

                            if (mcyj.Trim() != "")
                                tj_jcsj = tj_jcsj + "末次月经：" + mcyj + "\n";
                           if (sfjj.Trim() != "")
                                tj_jcsj = tj_jcsj + "是否绝经：" + sfjj + "\n";
                        }

                       tj_jcsj =tj_jcsj+ jcxx.Rows[0]["F_rysj"].ToString().Trim();

                        if (TJ_bljc.Rows.Count > 0)
                        {
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT门诊" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT进口")
                            {
                                tj_jcsj =tj_jcsj+ "标本满意度:" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "  " + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + "\n" + "反应性细胞变化:" + "\n" + TJ_bljc.Rows[0]["F_JGH_CH1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_JGH_CH2"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_JGH_CH3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_JGH_CH4"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_JGH_CH5"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_JGH_CH6"].ToString().Trim() + "\n";
                              
                                tj_jcsj = tj_jcsj + "\n" + "微生物项目:" + "\n" + TJ_bljc.Rows[0]["F_JGH_HP1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_JGH_HP2"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_JGH_HP3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_JGH_HP4"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_JGH_HP5"].ToString().Trim() + "\n";
                              
                                tj_jcsj = tj_jcsj + "\n" + "上皮细胞情况:" + "\n" + TJ_bljc.Rows[0]["F_TBS_LZXB"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_XXB"].ToString().Trim() + "\n";
                                ////////////////////////////////////
                                tj_blzd = "镜检分析及建议：" + TJ_bljc.Rows[0]["F_TBS_ZD"].ToString().Trim() + "\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\n";
                            }
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT国产")
                            {
                              
                                tj_jcsj =tj_jcsj+ "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + "病原微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\n"+TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\n";
                                ////////////////////////////////////
                                tj_blzd = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim()+"\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见1：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见2：" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\n";
                            }
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "HPV")
                            {
                                tj_jcsj = tj_jcsj + "检测方法：" + TJ_bljc.Rows[0]["F_FZ_JCFF"].ToString().Trim() + "\n" + "检测内容：" + TJ_bljc.Rows[0]["F_FZ_JCWD"].ToString().Trim() + "\n";
                                tj_blzd = "检测结果：" + TJ_bljc.Rows[0]["F_FZ_JCJG"].ToString().Trim();// "检查结果：" + TJ_bljc.Rows[0]["F_FZ_YYX"].ToString().Trim();
                            }
                        }
                        if (debug == "1")
                        {
                            MessageBox.Show(tj_jcsj);
                            MessageBox.Show(tj_blzd);
                        }
                        /////////////////////////////////////////////////////
                        mzORAPAR[0].Value = jcxx.Rows[0]["f_sqxh"].ToString().Trim();   //体检申请单号
                        mzORAPAR[1].Value = tj_jcsj.Trim();   //检查所见
                        mzORAPAR[2].Value = tj_blzd.Trim();   // 检查所得
                        mzORAPAR[3].Value = "";   //检查医生工号
                        mzORAPAR[4].Value = jcxx.Rows[0]["f_bgys"].ToString().Trim();   // 检查医生姓名
                        mzORAPAR[5].Value = DateTime.Parse(jcxx.Rows[0]["f_bgrq"].ToString().Trim());   //检查时间
                        mzORAPAR[9].Value = "";   // 审核医生工号
                        mzORAPAR[10].Value = jcxx.Rows[0]["f_shys"].ToString().Trim();   // 审核医生姓名
                        ////////////////////////////////////////////////////
                        OdbcConnection ocn = new OdbcConnection(odbcsql);
                        try
                        {
                            OdbcCommand mzcmd = new OdbcCommand();
                            mzcmd.Connection = ocn;
                            mzcmd.CommandText = "{ CALL hthis.PROC_PEIS_GETBLRESULTINFO(?,?,?,?,?,?,?,?,?,?,?)}";
                            mzcmd.Parameters.Add(mzORAPAR[0]);
                            mzcmd.Parameters.Add(mzORAPAR[1]);
                            mzcmd.Parameters.Add(mzORAPAR[2]);
                            mzcmd.Parameters.Add(mzORAPAR[3]);
                            mzcmd.Parameters.Add(mzORAPAR[4]);
                            mzcmd.Parameters.Add(mzORAPAR[5]);
                            mzcmd.Parameters.Add(mzORAPAR[6]);
                            mzcmd.Parameters.Add(mzORAPAR[7]);
                            mzcmd.Parameters.Add(mzORAPAR[8]);
                            mzcmd.Parameters.Add(mzORAPAR[9]);
                            mzcmd.Parameters.Add(mzORAPAR[10]);
                            try
                            {
                                ocn.Open();
                                if (debug == "1")
                                    MessageBox.Show("数据库打开正常");

                                mzcmd.ExecuteNonQuery();
                                if (debug == "1")
                                    MessageBox.Show("数据库执行正常");

                                ocn.Close();
                            }
                            catch (Exception eee)
                            {
                                ocn.Close();
                               log.WriteMyLog("数据库执行异常，" + eee.ToString());
                               if (debug == "1")
                                   MessageBox.Show("数据库执行异常："+eee.ToString());

                                return;
                            }
                            finally
                            {
                                if (ocn.State == ConnectionState.Open)
                                    ocn.Close();
                            }
                            if (debug == "1")
                            {
                                MessageBox.Show(mzORAPAR[8].Value.ToString());
                            }
                            if (mzORAPAR[8].Value.ToString().Trim()== "")
                            {
                                if (debug == "1")
                                    log.WriteMyLog(F_blh + "体检回传成功！");
                            }
                            else
                            {
                                if (debug == "1")
                                    MessageBox.Show("体检回传失败：" + mzORAPAR[8].Value.ToString().Trim());
                                log.WriteMyLog("体检回传失败，" + F_blh + "," + mzORAPAR[6].Value.ToString().Trim() + "," + mzORAPAR[7].Value.ToString().Trim() +","+ mzORAPAR[8].Value.ToString().Trim());
                            }
                        }
                        catch (Exception e)
                        {
                            log.WriteMyLog(e.Message.ToString());
                        }
                    }
                }
                catch (Exception ee)
                {
                    log.WriteMyLog(ee.Message.ToString());
                }

            }
        }  
        
        public void  oper_sqd(string F_blh,string brlb, string ztbz,string sqxh,string debug)
        {
             string usr = f.ReadString("bjhtyy", "user", "sd_");
            string pwd = f.ReadString("bjhtyy", "pwd", "sd_");
            string dsn = f.ReadString("bjhtyy", "dsn", "pathnet-his");
            string connstring = "DSN=" + dsn + ";uid=" + usr + ";pwd=" + pwd + "";
            OdbcConnection ocn = new OdbcConnection(connstring);
            
            
            //ocmd.Parameters.Add(new OdbcParameter("@Par_BillNO", OdbcType.VarChar, 50));
            //ocmd.Parameters.Add(new OdbcParameter("@Par_BillState", OdbcType.VarChar, 50));
            //ocmd.Parameters.Add(new OdbcParameter("@Par_BillOper", OdbcType.VarChar, 50));
            //ocmd.Parameters.Add(new OdbcParameter("@Par_BillDept", OdbcType.VarChar, 50));
            //ocmd.Parameters.Add(new OdbcParameter("@Par_PatientType", OdbcType.VarChar, 50));
            //ocmd.Parameters.Add(new OdbcParameter("@Par_ErrText", OdbcType.VarChar, 50));
            //ocmd.Parameters.Add(new OdbcParameter("@Par_RtnCode", OdbcType.Int));

            //ocmd.Parameters[0].Value = sqxh;
            //ocmd.Parameters[1].Value = ztbz;
            //ocmd.Parameters[2].Value = f.ReadString("yh", "yhmc", "登记员").ToString().Trim();
            //ocmd.Parameters[3].Value = "病理科";
            //ocmd.Parameters[4].Value =brlb;

            //ocmd.Parameters[0].Direction = ParameterDirection.Input;
            //ocmd.Parameters[1].Direction = ParameterDirection.Input;
            //ocmd.Parameters[2].Direction = ParameterDirection.Input;
            //ocmd.Parameters[3].Direction = ParameterDirection.Input;
            //ocmd.Parameters[4].Direction = ParameterDirection.Input;
            //ocmd.Parameters[5].Direction = ParameterDirection.Output;
            //ocmd.Parameters[6].Direction = ParameterDirection.Output;

            OdbcParameter[] mzORAPAR = new OdbcParameter[7];

            for (int j = 0; j < mzORAPAR.Length; j++)
            {
                mzORAPAR[j] = new OdbcParameter();
            }
            mzORAPAR[0].ParameterName = "Par_BillNO";
            mzORAPAR[0].OdbcType = OdbcType.VarChar;
            mzORAPAR[0].Direction = ParameterDirection.Input;
            mzORAPAR[0].Size = 200;



            mzORAPAR[1].ParameterName = "Par_BillState";//
            mzORAPAR[1].OdbcType = OdbcType.VarChar;
            mzORAPAR[1].Direction = ParameterDirection.Input;
            mzORAPAR[1].Size = 200;

            mzORAPAR[2].ParameterName = "Par_BillOper";//
            mzORAPAR[2].OdbcType = OdbcType.VarChar;
            mzORAPAR[2].Direction = ParameterDirection.Input;
            mzORAPAR[2].Size = 200;

            mzORAPAR[3].ParameterName = "Par_BillDept";//
            mzORAPAR[3].OdbcType = OdbcType.VarChar;
            mzORAPAR[3].Direction = ParameterDirection.Input;
            mzORAPAR[3].Size = 200;

            mzORAPAR[4].ParameterName = "Par_PatientType";// 
            mzORAPAR[4].OdbcType = OdbcType.VarChar;
            mzORAPAR[4].Direction = ParameterDirection.Input;
            mzORAPAR[4].Size = 200;


            mzORAPAR[5].ParameterName = "Par_ErrText";// 
            mzORAPAR[5].OdbcType = OdbcType.VarChar;
            mzORAPAR[5].Direction = ParameterDirection.Output;
            mzORAPAR[5].Size = 200;
           

            mzORAPAR[6].ParameterName = "Par_RtnCode";// 
            mzORAPAR[6].OdbcType = OdbcType.Numeric;
            mzORAPAR[6].Direction = ParameterDirection.Output;
          

            mzORAPAR[0].Value = sqxh;
            mzORAPAR[1].Value = ztbz;
            mzORAPAR[2].Value = f.ReadString("yh", "yhmc", "登记员").ToString().Trim();
            mzORAPAR[3].Value = "1023";
            mzORAPAR[4].Value = brlb;


            try
            {

                OdbcCommand mzcmd = new OdbcCommand();
                mzcmd.Connection =ocn;
                
                mzcmd.CommandText = "{ CALL hthis.PRC_PACS_UPDATEAPPLYBILL(?,?,?,?,?,?,?)}";


                mzcmd.Parameters.Add(mzORAPAR[0]);
                mzcmd.Parameters.Add(mzORAPAR[1]);
                mzcmd.Parameters.Add(mzORAPAR[2]);
                mzcmd.Parameters.Add(mzORAPAR[3]);
                mzcmd.Parameters.Add(mzORAPAR[4]);
                mzcmd.Parameters.Add(mzORAPAR[5]);
                mzcmd.Parameters.Add(mzORAPAR[6]);

                ocn.Open();


                mzcmd.ExecuteNonQuery();

                if (mzORAPAR[6].Value.ToString() == "1")
                {

                    if (debug == "1")
                    {
                        log.WriteMyLog(F_blh + "申请单状态更新成功成功！");
                      
                    }
                }

                else
                {
                    if (debug == "1")
                    {
                        log.WriteMyLog(F_blh + mzORAPAR[5].Value.ToString().Trim());
                    }
                }
            }
            catch (Exception e)
            {
                if (debug == "1")
                MessageBox.Show(e.Message.ToString());
            log.WriteMyLog(e.Message.ToString());
            }

        }
    }
}
