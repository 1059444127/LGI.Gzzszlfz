using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.IO;
using ZgqClassPub;

namespace PathHISJK
{
    class tzyysm
    {

        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        /// <summary>
        /// 台州医院手麻加PDF文件回传2016-03-02修改chj
        /// </summary>
        /// <param name="F_blh">病理号</param>
        /// <param name="debug"></param>
        /// <param name="cgbcbd">常规补充冰冻</param>
        /// <param name="bgxh">报告序号</param>
        /// <param name="newold">新建或者已经存在</param>
        /// <param name="saveqxsh">保存或者取消审核</param>
        public void tzyyhx(string F_blh, string debug, string cgbcbd, string bgxh, string newold, string saveqxsh)
        {

            if (bgxh.Trim() == "")
                bgxh = "0";
            string bglx = cgbcbd.ToLower().Trim();

            //string ds = f.ReadString("tzyysm", "datasource", "se");
            string usr = f.ReadString("tzyysm", "user", "sd_");
            string pwd = f.ReadString("tzyysm", "pwd", "sd_");
            string dsn = f.ReadString("tzyysm", "dsn", "pathnet-his");
            string dqdsn = f.ReadString("savetohis", "dqdsn", "pathnet");  ///当前DSN
            dbbase.odbcdb aa = new odbcdb("DSN="+dqdsn+";UID=pathnet;PWD=4s3c2a1p", "", "");

            DataTable jcxx = new DataTable();
            DataTable bcbg = new DataTable();
            DataTable bdbg = new DataTable();
            
            //后改odbc连接
            string connstring = "DSN="+dsn+";uid="+usr+";pwd="+pwd+"";
            OdbcConnection ocn = new OdbcConnection(connstring);
           
            OdbcCommand ocmd= new OdbcCommand();
            ocmd.Connection = ocn;
            //string ConnectionString = "data source="+ds+";User id="+usr+";password="+pwd+"";//写连接串
            //OracleConnection ocn = new OracleConnection(ConnectionString);
            //OracleCommand ocmd = new OracleCommand();
            //ocmd.Connection = ocn;

            string ser = "select * from bsrun.SM_BLSQD";                 //查询中间表是否含有此条记录。
            string upda = "update bsrun.SM_BLSQD set ";
            
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                
            }
            catch (Exception ex)
            {
               log.WriteMyLog(ex.Message.ToString());
               aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='" + ex.Message + "'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                return;
            }
            if (jcxx == null)
            {
                log.WriteMyLog("连接数据库异常");
                return;
            }
            if (jcxx.Rows.Count > 0)
            {
                string server = szz(aa,"pdfserver");
                string database = szz(aa, "pdfdb");
                string user = szz(aa, "pdfuser");
                string sqlpwd = szz(aa, "pdfpwd");

                SqlConnection sqlcon = new SqlConnection("server=" + server + ";database=" + database + ";uid=" + user + ";pwd=" + sqlpwd + "");
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcon;
                string hcpdf = "";
               
                try
                {
                    hcpdf = szz(aa, "hcpdf");
                }
                catch
                {
                    hcpdf = "0";  
                }

                if (cgbcbd == "cg")
                {
                    string bdhxyesno = f.ReadString("savetohis", "bdhx", "yes").ToString().Trim();
                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                    {

                        if (bdhxyesno == "yes")
                        {
                            #region 冰冻回传
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "冰冻" && jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "")
                            {
                                ocn.Open();
                                string zyh = jcxx.Rows[0]["f_zyh"].ToString().Trim();
                                string bgrq = "to_date('" + jcxx.Rows[0]["f_spare5"] + "','yyyy-MM-dd hh24:mi:ss')";
                                string sqxh = jcxx.Rows[0]["f_sqxh"].ToString().Trim();
                                string blzd = jcxx.Rows[0]["f_blzd"].ToString();
                                string shys = jcxx.Rows[0]["f_shys"].ToString();
                                string jsy = jcxx.Rows[0]["f_jsy"].ToString().Trim();
                                string jsy1 = "";
                                if (jsy == "")
                                {
                                    jsy1 = "登记员";
                                }
                                else
                                {
                                    jsy1 = jsy;
                                }
                                ///2013-12-31   信息科要求 接收
                                try
                                {
                                    upda += "JSGH='" + jsy1 + "',BGRQ=" + bgrq + ",JCJG='" + blzd + "',BGSHGH='"+shys+"' where JLXH='" + sqxh + " '";
                                    ocmd.CommandText = upda;
                                    log.WriteMyLog(upda);
                                    int j = ocmd.ExecuteNonQuery();
                                    if (j > 0)
                                    {
                                        if (debug == "1")
                                        {
                                            aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='',F_FSZT='已处理'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'  and F_BGZT='已审核'");
                            
                                            log.WriteMyLog(F_blh + "冰冻结果回写成功!");
                                        }
                                    }
                                }
                                catch (Exception EX)
                                {
                                    log.WriteMyLog(EX.Message.ToString());
                                    aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='" + EX.Message + "'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                                }

                                #region
                                //else
                                //{
                                //    try
                                //    {
                                //        upda += "JSGH='" + jsy1 + "',JCJG='' where JLXH='" + sqxh + "'";
                                //        ocmd.CommandText = upda;
                                //        int j = ocmd.ExecuteNonQuery();
                                //        if (j > 0)
                                //        {
                                //            if (debug == "1")
                                //            {
                                //                 LGZGQClass.log.WriteMyLog(F_blh + "冰冻登记成功!");
                                //            }
                                //        }
                                //    }
                                //    catch (Exception EX)
                                //    {
                                //         LGZGQClass.log.WriteMyLog(EX.Message.ToString());
                                //    }
                                //}
                                #endregion

                            }
                            #endregion
                        }
                        if (hcpdf == "1")
                        {
                            #region   //回传PDF写二进制到数据库
                            try
                            {
                                scpdf(F_blh, cgbcbd, sqlcmd, bgxh, debug, jcxx, sqlcon,aa);
                            }
                            catch (Exception e2)
                            {
                                log.WriteMyLog(F_blh + e2.Message.ToString());
                                aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='" + e2.Message + "'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                            }
                            #endregion
                        }
                    }

                    else
                    {


                        if (hcpdf=="1")
                        {
                            if (saveqxsh == "qxsh")
                            {
                                try
                                {
                                    retun_dyzt(F_blh, cgbcbd,bgxh, debug, jcxx,aa,sqlcon);
                                }
                                catch (Exception eee)
                                {
                                   log.WriteMyLog("返回检查报告状态时出错1" + eee.Message.ToString());
                                   aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='" + "返回检查报告状态时出错1" + eee.Message.ToString() + "'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and  F_BGZT='取消审核'");
                                }
                            }
                            try
                            {
                                delepdftable(F_blh, cgbcbd, sqlcmd, bgxh, debug, sqlcon);
                            }
                            catch
                            {
                               log.WriteMyLog("删除报告PD所在表记录失败1!");
                               
                            }     
                           
                        }
                        if (bdhxyesno == "yes")
                        {
                            #region 冰冻回传需要登记
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "冰冻" && jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "")
                            {
                                ocn.Open();
                                string zyh = jcxx.Rows[0]["f_zyh"].ToString().Trim();
                                string bgrq = "to_date('" + jcxx.Rows[0]["f_spare5"] + "','yyyy-MM-dd hh24:mi:ss')";
                                string sqxh = jcxx.Rows[0]["f_sqxh"].ToString().Trim();
                                string blzd = jcxx.Rows[0]["f_blzd"].ToString();
                                string shys = jcxx.Rows[0]["f_shys"].ToString();
                                string jsy = jcxx.Rows[0]["f_jsy"].ToString().Trim();
                                string jsy1 = "";
                                if (jsy == "")
                                {
                                    jsy1 = "登记员";
                                }
                                else
                                {
                                    jsy1 = jsy;
                                }
                                ///2013-12-31   信息科要求 接收

                                try
                                {
                                    upda += "JSGH='" + jsy1 + "',BGSHGH='',JCJG='' where JLXH='" + sqxh + "'";
                                    ocmd.CommandText = upda;
                                    int j = ocmd.ExecuteNonQuery();
                                    if (j > 0)
                                    {
                                        if (debug == "1")
                                        {
                                            log.WriteMyLog(F_blh + "冰冻登记成功!");
                                        }
                                    }
                                }
                                catch (Exception EX)
                                {
                                    log.WriteMyLog(EX.Message.ToString());
                                 }




                            }
                            #endregion
                        }
                    }

                }
                if (cgbcbd == "bc")
                {
                    try
                    {
                        bcbg = aa.GetDataTable("select * from t_bcbg where f_blh='" + F_blh + "' and f_bc_bgxh='" + bgxh + "'","bcbg");
                    }
                    catch
                    {
                        log.WriteMyLog("未查询到补充报告");
                        return;
                    }
                    if (bcbg.Rows.Count > 0)
                    {
                        if (hcpdf == "1" && bcbg.Rows[0]["F_BC_BZ"].ToString().Trim()!="门诊收费")
                        {
                            //判断是否存在表blbg中的记录
                           
                            if (bcbg.Rows[0]["F_BC_BGZT"].ToString().Trim() == "已审核")
                            {
                                scpdf(F_blh, cgbcbd, sqlcmd, bgxh, debug, jcxx, sqlcon,aa);
                            }
                            else
                            {
                                if (saveqxsh == "qxsh")
                                {
                                    try
                                    {
                                        retun_dyzt(F_blh,cgbcbd,bgxh, debug, jcxx,aa,sqlcon);
                                    }
                                    catch (Exception eee)
                                    {
                                       log.WriteMyLog("返回检查报告状态时出错2" + eee.Message.ToString());
                                       aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='返回检查报告状态时出错2" + eee.Message.ToString() + "'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'  and F_bgzt='取消审核'");
                            
                                    }
                                }
                                try
                                {
                                    delepdftable(F_blh, cgbcbd, sqlcmd, bgxh, debug, sqlcon);
                                }
                                catch
                                {
                                   log.WriteMyLog("删除报告PD所在表记录失败2!");
                                 
                                }
                                 
                            }
                        }
                        else
                            aa.ExecuteSQL("update T_jcxx_fs_sm set F_fszt='不处理',F_BZ='门诊收费'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'  and F_bgzt='已审核'");
                            
                    }

                }
                if (cgbcbd == "bd")
                {
                    try
                    {
                        bdbg = aa.GetDataTable("select * from t_bdbg where f_blh='" + F_blh + "' and f_bd_bgxh='" + bgxh + "'","bdbg");
                    }
                    catch
                    {
                        return;
                    }
                    if (bdbg.Rows.Count > 0)
                    {
                        if (hcpdf == "1")
                        {
                            
                            if (bdbg.Rows[0]["F_BD_BGZT"].ToString().Trim() == "已审核")
                            {
                                scpdf(F_blh, cgbcbd, sqlcmd, bgxh, debug, jcxx, sqlcon,aa);
                            }
                            else
                            {
                                if (saveqxsh == "qxsh")
                                {
                                    try
                                    {
                                        retun_dyzt(F_blh, cgbcbd,bgxh, debug, jcxx,aa,sqlcon);
                                    }
                                    catch (Exception eee)
                                    {
                                     log.WriteMyLog("返回检查报告状态时出错3"+eee.Message.ToString());
                                     aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='返回检查报告状态时出错3" + eee.Message.ToString() + "'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                            
                                    }
                                    try
                                    {
                                        delepdftable(F_blh, cgbcbd,sqlcmd,bgxh, debug, sqlcon);
                                    }
                                    catch
                                    {
                                      log.WriteMyLog("删除报告PD所在表记录失败3!");
                                      aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='='删除报告PD所在表记录失败3'  where F_blh='" + F_blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='取消审核'");
                            
                                    }
                                }
                               
                            }
                        }
                    }



                }
                sqlcon.Close();

           }
            
              ocn.Close();
              aa.Close();
           
      }
          
        
        /// <summary>
        /// 生成PDF文件并上传表
        /// </summary>
        /// <param name="cgbcbd"></param>
        /// <param name="sqlcmd"></param>
        private void scpdf(string F_blh,string cgbcbd, SqlCommand sqlcmd,string bgxh,string debug,DataTable jcxx,SqlConnection sqlcon,dbbase.odbcdb aa)
        {
            #region    生成pdf读入二进制流
            string pdfname = "";

            try
            {
                string message = "";
                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(F_blh, cgbcbd, bgxh, ZgqPDFJPG.Type.PDF, ref message, ref pdfname);

                if (!isrtn)
                {
                    log.WriteMyLog(F_blh+"^"+cgbcbd+"^"+bgxh+",生成PDF失败：" + message);
                    aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='生成PDF失败：" + message + "'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                    return;
                }

                //二进制串
                if (!File.Exists(pdfname))
                {
                    log.WriteMyLog("未查询到pdf文件" + pdfname);
                    aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='未查询到pdf文件" + pdfname + "'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");

                    return;
                }

                FileStream fs = null;
                try
                {
                    fs = new FileStream(pdfname, FileMode.Open, FileAccess.Read);
                    Byte[] image = new Byte[fs.Length];
                    fs.Read(image, 0, image.Length);
                    fs.Close();

                    //2016-03-11 
                    string insertsql = "insert into BLBGPDF(blh,bglx,bgxh,patientid,pdf,sdrq,patientname,bbmc,hospitalid,blk)values(@blh,@bglx,@bgxh,@patientid,@pdf,@sdrq,@patientname,@bbmc,@hospitalid,@blk)";
                    try
                    {
                        if (debug == "1")
                        {
                            log.WriteMyLog(insertsql);
                        }
                        sqlcmd.CommandText = insertsql;
                        sqlcmd.Parameters.Add("@blh", SqlDbType.VarChar);
                        sqlcmd.Parameters.Add("@bglx", SqlDbType.VarChar);
                        sqlcmd.Parameters.Add("@bgxh", SqlDbType.VarChar);
                        sqlcmd.Parameters.Add("@patientid", SqlDbType.VarChar);
                        sqlcmd.Parameters.Add("@pdf", SqlDbType.Binary);

                        sqlcmd.Parameters.Add("@sdrq", SqlDbType.VarChar);
                        sqlcmd.Parameters.Add("@patientname", SqlDbType.NVarChar);
                        sqlcmd.Parameters.Add("@bbmc", SqlDbType.NVarChar);
                        sqlcmd.Parameters.Add("@hospitalid", SqlDbType.VarChar);
                        sqlcmd.Parameters.Add("@blk", SqlDbType.NVarChar);


                        sqlcmd.Parameters["@blh"].Value = F_blh;
                        sqlcmd.Parameters["@bglx"].Value = cgbcbd;
                        sqlcmd.Parameters["@bgxh"].Value = bgxh;
                        sqlcmd.Parameters["@patientid"].Value = jcxx.Rows[0]["F_MZH"].ToString().Trim() + jcxx.Rows[0]["F_ZYH"].ToString().Trim();
                        sqlcmd.Parameters["@pdf"].Value = image;

                        sqlcmd.Parameters["@sdrq"].Value = jcxx.Rows[0]["F_SDRQ"].ToString().Trim();
                        sqlcmd.Parameters["@patientname"].Value = jcxx.Rows[0]["F_XM"].ToString().Trim();
                        sqlcmd.Parameters["@BBMC"].Value = jcxx.Rows[0]["F_BBMC"].ToString().Trim();
                        if (jcxx.Rows[0]["F_BLK"].ToString().Trim().Contains("路桥"))
                        {
                            sqlcmd.Parameters["@hospitalid"].Value = "5760003";
                        }
                        else
                        {
                            sqlcmd.Parameters["@hospitalid"].Value = szz(aa, "hospitalid").Trim();
                        }
                        sqlcmd.Parameters["@blk"].Value = jcxx.Rows[0]["F_BLK"].ToString().Trim();


                        int count = sqlcmd.ExecuteNonQuery();//执行存储过程

                        if (count > 0)
                        {
                            aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='',F_ISPDF='true',F_fszt='已处理',f_fssj='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");

                            if (debug == "1")
                            {
                                log.WriteMyLog(F_blh + "PDF报告上传成功！");
                            }
                            if (cgbcbd == "cg" && (jcxx.Rows[0]["F_ZYH"].ToString().Trim() != "" || jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院"))
                            {
                                aa.ExecuteSQL("update t_jcxx set f_sfdy='是' where f_blh='" + F_blh + "'");
                                aa.ExecuteSQL("insert into t_bghj(f_blh,f_rq,f_czy,f_wz,f_dz,f_nr,f_exemc,f_ctmc)values('" + F_blh + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + f.ReadString("yh", "yhmc", "").ToString().Trim() + "','" + System.Net.Dns.GetHostName() + "','打印','" + cgbcbd + "报告" + bgxh + "发送到临床','RPT','打印或批量打印')");


                            }
                            if (cgbcbd == "bc" && (jcxx.Rows[0]["F_ZYH"].ToString().Trim() != "" || jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院"))
                            {
                                aa.ExecuteSQL("update t_bcbg set f_bc_dyzt='是' where  f_blh='" + F_blh + "' and f_bc_bgxh='" + bgxh + "'");
                                aa.ExecuteSQL("insert into t_bghj(f_blh,f_rq,f_czy,f_wz,f_dz,f_nr,f_exemc,f_ctmc)values('" + F_blh + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + f.ReadString("yh", "yhmc", "").ToString().Trim() + "','" + System.Net.Dns.GetHostName() + "','打印','" + cgbcbd + "报告" + bgxh + "发送到临床','RPT','打印或批量打印')");

                            }
                            if (cgbcbd == "bd" && (jcxx.Rows[0]["F_ZYH"].ToString().Trim() != "" || jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院"))
                            {
                                aa.ExecuteSQL("update t_bdbg set f_bd_dyzt='是' where  f_blh='" + F_blh + "' and f_bd_bgxh='" + bgxh + "'");
                                aa.ExecuteSQL("insert into t_bghj(f_blh,f_rq,f_czy,f_wz,f_dz,f_nr,f_exemc,f_ctmc)values('" + F_blh + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + f.ReadString("yh", "yhmc", "").ToString().Trim() + "','" + System.Net.Dns.GetHostName() + "','打印','" + cgbcbd + "报告" + bgxh + "发送到临床','RPT','打印或批量打印')");

                            }
                        }

                        else
                        {
                           log.WriteMyLog(F_blh + "回传PDF文件不成功！");
                           aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='回传PDF文件不成功'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                 
                        }

                    }
                    catch (Exception e)
                    {
                        log.WriteMyLog("回写数据异常：" + e.Message.ToString());
                        aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='回写数据异常：" + e.Message.ToString() + "'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                     
                    }
                }
                catch (Exception e2)
                {
                    log.WriteMyLog("读取PDF异常：" + e2.Message.ToString());
                    aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='读取PDF异常：" + e2.Message.ToString() + "'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
                }
                finally
                {

                    fs.Close();
                }


                if (szz(aa, "scpdf") == "1")
                {
                    if (System.IO.Directory.Exists(@"c:\temp\" + F_blh))
                        System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                }

            }
            catch (Exception e3)
            {

                log.WriteMyLog("接口程序异常：" + e3.Message.ToString());
                aa.ExecuteSQL("update T_jcxx_fs_sm set F_bz='接口程序异常：" + e3.Message.ToString() + "'  where F_blh='" + F_blh + "' and F_BGLX='" + cgbcbd + "' and F_BGXH='" + bgxh + "' and F_BGZT='已审核'");
             
                return;

            }
          
            #endregion 
        }

        /// <summary>
        /// 删除pdf所在表中的记录（常规补充冰冻）
        /// </summary>
        /// <param name="cgbcbd"></param>
        /// <param name="sqlcmd"></param>
        private void delepdftable(string F_blh, string cgbcbd, SqlCommand sqlcmd, string bgxh, string debug,SqlConnection sqlcon)
        {

            #region  删除PDF文件
       
            string del = "delete from BLBGPDF WHERE blh='" + F_blh + "' and bglx='" + cgbcbd + "' and bgxh='" + bgxh + "'";
           
            try
            {
               
                log.WriteMyLog("删除语句为:" + del);
                SqlCommand cmd = new SqlCommand(del, sqlcon);
                int count = cmd.ExecuteNonQuery();//执行存储过程

                if (count > 0)
                {
                    if (debug == "1")
                    {
                        
                        log.WriteMyLog(F_blh + "删除PDF成功！");
                    }
                }

                else
                {
                    if (debug == "1")
                    {
                        log.WriteMyLog(F_blh+"报告类型为"+cgbcbd+"报告序号为"+bgxh+"删除PDF文件不成功不存在或还没有写入过文件！");
                    }
                }
               
            }
            catch (Exception e)
            {
               log.WriteMyLog(e.Message.ToString());
            }
            #endregion
        }


        /// <summary>
        /// 返回一个病人的报告是否已经被打印，0表示没有被打印过或者不存在记录
        /// </summary>
        /// <param name="F_blh"></param>
        /// <param name="cgbcbd"></param>
        /// <param name="sqlcmd"></param>
        /// <param name="bgxh"></param>
        /// <param name="debug"></param>
        /// <param name="jcxx"></param>
        /// <param name="sqlcon"></param>
        /// <param name="aa"></param>
        /// <returns></returns>
        private void  retun_dyzt(string F_blh, string cgbcbd,string bgxh, string debug, DataTable jcxx,dbbase.odbcdb aa,SqlConnection sqlcon)
        {

            //string server = f.ReadString("tzyypdf", "server", "172.16.80.50");
            //string database = f.ReadString("tzyypdf", "db", "ReportServer");
            //string user = f.ReadString("tzyypdf", "User", "ReportServer");
            //string sqlpwd = f.ReadString("tzyypdf", "Pwd", "tzhospital012");
            //SqlConnection sqlcon = new SqlConnection("server=" + server + ";database=" + database + ";uid=" + user + ";pwd=" + sqlpwd + "");
            //sqlcon.Open();

            ///判断是否打印过BLBGPDF
            string blbgpdfsql = "select * from blbgpdf where blh='" + F_blh + "' and bglx='" + cgbcbd + "' and bgxh='" + bgxh + "'";
            DataSet ds_blbgpdf = new DataSet();
            SqlCommand sqlcmd2 = new SqlCommand(blbgpdfsql, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd2);
            sda.Fill(ds_blbgpdf);
            try
            {
                if (ds_blbgpdf != null)
                {
                    if (ds_blbgpdf.Tables[0].Rows.Count > 0)
                    {
                        if (ds_blbgpdf.Tables[0].Rows[0]["Printed"].ToString().Trim() == "1" || ds_blbgpdf.Tables[0].Rows[0]["Printed"].ToString().Trim() == "是")
                        {
                            if (cgbcbd == "cg")
                            {
                              log.WriteMyLog("病理号为:" + F_blh + "的报告已经被临床打印，请及时联系临床医生或病人！");
                            }
                            if (cgbcbd == "bc")
                            {
                              log.WriteMyLog("病理号为:" + F_blh + "的补充报告"+bgxh+"已经被临床打印，请及时联系临床医生或病人！");
                            }
                            if (cgbcbd == "bd")
                            {
                               log.WriteMyLog("病理号为:" + F_blh + "的冰冻报告" + bgxh + "已经被临床打印，请及时联系临床医生或病人！");
                            }
                        }
                        
                    }

                }
              
            }
            catch(Exception  eee)
            {
                sqlcon.Close();
                log.WriteMyLog(F_blh+eee.Message.ToString());
               
            }
         
            
        }
        public static string szz(dbbase.odbcdb aa, string xl)
        {
            DataTable T_SZ = new DataTable();
            T_SZ = aa.GetDataTable("SELECT * FROM T_SZ WHERE F_XL like '%" + xl + "%'", "T_SZ");
            if (T_SZ.Rows.Count > 0)
            {
                return T_SZ.Rows[0]["F_SZZ"].ToString();
            }
            else
            {
                return "";
            }
        }
     
    }
    }

