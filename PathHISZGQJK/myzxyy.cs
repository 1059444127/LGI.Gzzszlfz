using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dbbase;
using System.Data.Odbc;
using ZgqClassPub;


namespace PathHISJK
{
    class myzxyy
    {

        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        OdbcConnection cn = null;
        
        /// <summary>
        ///  dbbase.odbcdb aa = new odbcdb("DSN=pathnet-his2;UID=sys_sqsf;PWD=sqsfglxt", "", "");
        ///DataTable dzbl_jcjg = new DataTable();
       /// </summary>
        public void myzxyy_hx(string blh, string debug)
        {
            string usr = f.ReadString("myzxyy", "usr", "sys_sqsf");
            string pwd = f.ReadString("myzxyy", "pwd", "sqsfglxt");
            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();

            DataTable brxx = new DataTable();


          
            brxx = aa.GetDataTable("select * from T_JCXX where F_blh='" + blh + "'", "brxx");

            if (brxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (brxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }
           
           cn= new OdbcConnection("DSN=pathnet-his;UID="+usr+";PWD="+pwd+"");
            try
            {

                    string brbh = brxx.Rows[0]["F_brbh"].ToString().Trim();
                    string rysj = brxx.Rows[0]["F_RYSJ"].ToString().Trim();
                    string zyh = brxx.Rows[0]["F_zyh"].ToString().Trim();
                    string mzh = brxx.Rows[0]["F_mzh"].ToString().Trim();
                    string bgzt = brxx.Rows[0]["F_BGZT"].ToString().Trim();
                    string sqxh = brxx.Rows[0]["F_SQXH"].ToString().Trim();
                    string sdrq = brxx.Rows[0]["F_SDRQ"].ToString().Trim();
                    string bgys = brxx.Rows[0]["F_BGYS"].ToString().Trim();
                    string blzd = brxx.Rows[0]["F_BlZD"].ToString().Trim();
                    string deljcid = "select * from dzbl_jcjg where jcdh='" + blh + "'";
                    
                    string brlb = brxx.Rows[0]["F_BRLB"].ToString().Trim();
                  
                    if (brbh != "")
                    {

                                DataTable dt = new DataTable();
                                try
                                {
                                    cn.Open();
                                    OdbcDataAdapter dap = new OdbcDataAdapter(deljcid, cn);
                                    dap.Fill(dt);
                                    dap.Dispose();
                                    cn.Close();
                                }
                                catch (Exception ee1)
                                {
                                    MessageBox.Show("执行数据库失败：" + ee1.Message);
                                    return;
                                }

                            if (bgzt == "已审核")
                            {
                                if (sqxh != "")                  //申请序号不为空才回写标志
                                {
                                    if (brxx.Rows[0]["F_HXBZ"].ToString() != "1")
                                    {
                                        upstatus(blh, brlb,sqxh,debug);
                                    }
                                }

                                string shrq = brxx.Rows[0]["F_SPARE5"].ToString().Trim();
                                string shys = brxx.Rows[0]["F_shys"].ToString().Trim();
                                string blk = brxx.Rows[0]["F_Blk"].ToString().Trim();
                              
                                if (brlb == "住院"||brlb == "门诊")
                                {
                                    string in_jcjg = "";
                                    if (dt.Rows.Count > 0)
                                        in_jcjg = "update  dzbl_jcjg  set jcid='" + sqxh + "',jcsj='" + rysj + "',jcjl='" + blzd + "',zdys='" + bgys + "',shys='" + shys + "',sblx='病理',jcrq='" + sdrq + "',bgrq='" + shrq + "',jbdm='',jbmc='',zyh='" + brbh + "',khid=''  where   jcdh='" + blh + "'";
                                    else
                                        in_jcjg = "insert into dzbl_jcjg(jcid,jcsj,jcjl,zdys,shys,sblx,jcrq,bgrq,jbdm,jbmc,zyh,khid,jcdh)values('" + sqxh + "','" + rysj + "','" + blzd + "','" + bgys + "','" + shys + "','病理','" + sdrq + "','" + shrq + "','','','" + brbh + "','','" + blh + "')";

                                    try
                                    {

                                        OdbcCommand cmd2 = new OdbcCommand(in_jcjg, cn);
                                        cn.Open();
                                        int z = cmd2.ExecuteNonQuery();
                                        cn.Close();
                                        if (z > 0)
                                        {
                                            log.WriteMyLog(blh + "写入HIS成功;");
                                            if(debug=="1")
                                                log.WriteMyLog(blh + "，执行语句;" + in_jcjg);
                                            ZgqClass.BGHJ(blh, CZY, "审核", "写入HIS成功", "ZGQJK", "");
                                        }
                                        else
                                        {
                                            log.WriteMyLog(blh + "写入HIS失败;" + in_jcjg);
                                            ZgqClass.BGHJ(blh, CZY, "审核", "写入HIS失败", "ZGQJK", "");
                                        }

                                    }
                                    catch (Exception ee1)
                                    {
                                        MessageBox.Show("执行数据库失败：" + ee1.Message);
                                        return;
                                    }
                                
                                }
                                else
                                {
                                    if (debug == "1")
                                    {
                                        log.WriteMyLog(blh + "非门诊或住院病人,不处理");
                                        
                                    }
                                    ZgqClass.BGHJ(blh, CZY, "审核", "非门诊或住院病人,不处理", "ZGQJK", "");
                                    return;
                                }



                             

                            }
                            else
                            {
                                if (sqxh != "")
                                {
                                    if (brxx.Rows[0]["F_HXBZ"].ToString() != "1")
                                    {
                                        upstatus(blh, brlb, sqxh, debug);
                                    }
                                }

                                //取消审核
                                if(bgzt == "已写报告")
                                {
                                    

                                    string in_jcjg = "";
                                    if (dt.Rows.Count > 0)
                                    {
                                        in_jcjg = "update  dzbl_jcjg  set jcsj='',jcjl='',zdys='',shys='',bgrq=''   where   jcdh='" + blh + "'";

                                        try
                                        {

                                            OdbcCommand cmd2 = new OdbcCommand(in_jcjg, cn);
                                            cn.Open();
                                            int z = cmd2.ExecuteNonQuery();
                                            cn.Close();
                                            if (z > 0)
                                            {
                                                log.WriteMyLog(blh + "取消审核更改HIS记录成功");

                                                ZgqClass.BGHJ(blh, CZY, "取消审核", "更改HIS记录成功", "ZGQJK", "");
                                            }
                                            else
                                            {
                                                log.WriteMyLog(blh + "取消审核更改HIS记录失败：" + in_jcjg);
                                                ZgqClass.BGHJ(blh, CZY, "取消审核", "更改HIS记录失败", "ZGQJK", "");
                                            }

                                        }
                                        catch (Exception ee1)
                                        {
                                            MessageBox.Show("执行数据库失败：" + ee1.Message);
                                            log.WriteMyLog(blh + "取消审核执行数据库失败：" + ee1.Message+"\r\n"+ in_jcjg);
                                            ZgqClass.BGHJ(blh, CZY, "取消审核", "更改HIS记录失败", "ZGQJK", "");
                                            return;
                                        }
                                    }
                                }



                            }
                    }
                    else
                    {
                        if (sqxh != "")
                        {

                            if (brxx.Rows[0]["F_HXBZ"].ToString() != "1")
                            {
                                upstatus(blh, brlb, sqxh, debug);
                            }

                           
                        }
                    }

                
            }
            catch (Exception e)
            {
                cn.Close();
                ZgqClass.BGHJ(blh, CZY, "审核", "异常："+e.Message.ToString(), "ZGQJK", "");
                MessageBox.Show(e.Message.ToString());
            }
               
            }
        public void upstatus(string blh, string f_brlb,string sqxh,string debug)
        {
            string upmzsqd = "update dzbl_jcsqd set jsbz='1' where jcid='" + sqxh + "'";
            string upzysqd = "update zybl_zyyz_jcsqd set jsbz='1' where jcid='" + sqxh + "'";

            OdbcCommand cmd = new OdbcCommand();
            try
            {

                cmd.Connection = cn;
                cn.Open();
                if (f_brlb == "住院")
                {
                    cmd.CommandText = upzysqd;
                    int k = cmd.ExecuteNonQuery();
                    aa.ExecuteSQL("update t_jcxx set f_hxbz='1' where f_blh='" + blh + "'");
                    if (k > 1)
                    {
                        if (debug == "1")
                        {
                            log.WriteMyLog(blh + "在HIS中回写标志成功！");
                        }
                    }
                }
                if (f_brlb == "门诊")
                {
                    cmd.CommandText = upmzsqd;
                    int k = cmd.ExecuteNonQuery();
                    aa.ExecuteSQL("update t_jcxx set f_hxbz='1' where f_blh='" + blh + "'");
                    if (k > 1)
                    {
                        if (debug == "1")
                        {
                            log.WriteMyLog(blh + "在HIS中回写标志成功！");
                        }
                    }
                }
                cn.Close();
            }
            catch(Exception  ee2)
            {
                MessageBox.Show("执行数据库异常："+ee2.Message);
            }
            
        }

            
            
        }

    }

