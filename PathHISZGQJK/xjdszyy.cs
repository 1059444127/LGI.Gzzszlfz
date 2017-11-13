using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class xjdszyy  //新疆独山子医院
    {
        private static IniFiles f = new IniFiles("sz.ini");
        private static string blhgy = "";

        string orcon_str = "Data Source= dszrmyy;User ID=ZHIY_BJXW_PACS;Password=PACS09TOBJXW";
        

        public void pathtohis(string blh, string yymc)
        {

            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
            string sfsctx = f.ReadString("savetohis", "sfsctx", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql", "");
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            
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
            //-----------
            int brlb = 0;
            switch (bljc.Rows[0]["F_brlb"].ToString().Trim())
            {
                case "门诊": brlb = 1; break;
                case "急诊": brlb = 2; break;
                case "住院": brlb = 3; break;
                case "体检": brlb = 4; break;
                default: brlb = 0; break;
            }
            //---------------
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {

                ////-----------------查询----------
                string select_to_his = "select *  from ZEMR_PACS_REPORT where PACS_NO='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                OracleConnection orcon = new OracleConnection(orcon_str);
                OracleCommand orcom = new OracleCommand(select_to_his, orcon);


                bool x = false;
                try
                {
                    orcon.Open();
                    OracleDataReader dr = orcom.ExecuteReader();
                    x = dr.HasRows;
                    dr.Close();
                    orcom.Dispose();
                }
                catch (Exception e)
                {
                    log.WriteMyLog("查询状态出错：" + e.ToString());
                    orcom.Dispose();
                    orcon.Close(); return;
                   
                   
                }
                finally
                {
                    orcom.Dispose();
                    orcon.Close();

                }

                //-------------增加----------------
                int count = 0;
                string Oraclestring = "";
                decimal MODIFY_FLAG = 0;
                string blzd = "<desc note=\"检查描述\">" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</desc><diag note=\"诊断\">" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "</diag>";
                MessageBox.Show(blzd.ToString());
                
                ////-----------------  MODIFY_FLAG = 1;重新审核—修改----------
                //-----------------  MODIFY_FLAG =0;审核—增加----------
               
               // Byte[] blzdByte = System.Text.Encoding.ASCII.GetBytes(blzd);//把图片转成 Byte型 二进制流   

                Byte[] blzdByte = System.Text.Encoding.Default.GetBytes(blzd);


                                if (x)
                                {
                                    MODIFY_FLAG = 1;

                                    Oraclestring = @"update  ZEMR_PACS_REPORT  set INSPECT_CONTENT=:ZD,OPERATER='" + bljc.Rows[0]["F_BGYS"].ToString().Trim()
                                        + "',INSPECTOR='" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "',REPORT_TIME=to_date('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS'),LAST_MODIFY_TIME=to_date('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS')" +
                                        ", MODIFY_FLAG='" + MODIFY_FLAG + "',REMARK1='" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "',QUANTITY_FLAG='" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "' where PACS_NO='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";

                             
                                }
                                else
                                {

                                    Oraclestring = @"insert  into ZEMR_PACS_REPORT(PACS_NO,PATIENT_ID,EVENT_NO,PATIENT_TYPE,PACS_TYPE,PATIENT_NAME,
                                    PATIENT_SEX,PATIENT_AGE,IN_DEPT,BED,WARD,ADDRESS,TELEPHONE,MARRIAGE,PROFESSION,CHECKNO,BARCODE_ID,
                                    INSPECT_TYPE,INSPECT_SUB_TYPE,INSPECT_NAME,INSTRUMENT_NAME,INSPECT_CONTENT,APPLICANT,OPERATER,
                                    INSPECTOR,OPERATER_TIME,REPORT_TIME,LAST_MODIFY_TIME,MODIFY_FLAG,REMARK1,REMARK2,STATUS,APPLY_NO,QUANTITY_FLAG) 
                                     values ('" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZYH"].ToString().Trim()
                                         + "','" + brlb + "'," + MODIFY_FLAG + ",'" + bljc.Rows[0]["F_XM"].ToString().Trim() + "','"
                                             + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_NL"].ToString().Trim() + "','"
                                             + bljc.Rows[0]["F_SJKS"].ToString().Trim() + "','" + bljc.Rows[0]["F_CH"].ToString().Trim() + "','"
                                             + bljc.Rows[0]["F_BQ"].ToString().Trim() + "','" + bljc.Rows[0]["F_LXXX"].ToString().Trim() + "','" + "" + "','"
                                             + bljc.Rows[0]["F_HY"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZY"].ToString().Trim() + "','"
                                             + bljc.Rows[0]["F_BLH"].ToString().Trim() + "','" + "" + "','" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "','" + "" + "','"
                               + "" + "','',:ZD,'"
                                   + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','"
                                   + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + "" + "',to_date('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS'),"
                                   + "to_date('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS')," + MODIFY_FLAG + ",'" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "','" + "" + "'," + MODIFY_FLAG + ",'" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "')";

                                }
                                MessageBox.Show(Oraclestring);
                                OracleCommand orcom_insert = new OracleCommand();

                                orcom_insert.Connection = orcon;
                                orcom_insert.CommandText = Oraclestring;
                                orcom_insert.Parameters.Add("ZD", System.Data.OracleClient.OracleType.Blob, blzdByte.Length);
                                orcom_insert.Parameters["ZD"].Value = blzdByte;


                                try
                                {
                                    orcon.Open();

                                    count = orcom_insert.ExecuteNonQuery();
                                    orcom_insert.Dispose();
                                    orcon.Close();
                                }
                                catch (Exception ee)
                                {
                                    log.WriteMyLog("回传报告出错：" + ee.ToString());
                                    orcom_insert.Dispose();
                                    orcon.Close();
                                    return;
                                }
                                finally
                                {
                                    orcom_insert.Dispose();
                                    orcon.Close();

                                }


                //下面程序中图像信息
                if (sfsctx.Trim() == "1")//FTP下载方式
                {
                   
                  //下载FTP参数
                    string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                    string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                    string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                    string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                    string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                    string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                    FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                    string txml = bljc.Rows[0]["F_txml"].ToString().Trim();

                    
                    //下载图像
                    DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "'", "txlb");
                    if (txlb.Rows.Count < 1)
                    {
                        log.WriteMyLog("该病人无图像，不上传");
                        return;
                    }
                   
                   
                    int i2 = 1;
                    for (int i = 0; i < txlb.Rows.Count; i++)
                    {
                        
                        if (i >= txlb.Rows.Count)
                            return;
                     
                        bool tx_count = false;
                       // string strcon11 = "server =.; uid =pathnet;pwd =4s3c2a1p;database =pathnet";
                        OracleConnection orcon_tx = new OracleConnection(orcon_str); 
                       
                        try
                        {

                            //-----------判断该图片是否已上传--------------
                            string txOracle_select = @"select * from ZEMR_LIS_PACS_REPORT_IMAGE  where IMAGE_ID='" + txlb.Rows[i]["F_txm"].ToString().Trim() + "'";
                            try
                            {
                                OracleCommand orcom_tx = new OracleCommand(txOracle_select, orcon_tx);

                                 orcon_tx.Open();
                                OracleDataReader Oracledr_select = orcom_tx.ExecuteReader();
                                tx_count = Oracledr_select.HasRows;
                                orcom_tx.Dispose();
                                orcon_tx.Close();
                            }
                            catch (Exception tx_e)
                            {
                                orcon_tx.Close();
                                log.WriteMyLog("判断该图片是否已上传错误" + tx_e.ToString());
                                return;
                            }
                        
                            if (tx_count)
                                continue;
                            //--------------------------

                            i2 = i + 1;
                            string ftpstatus = "";
                            fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                            if (ftpstatus == "Error")
                            {
                                log.WriteMyLog("FTP下载图像出错！");
                                return;
                            }
                            //----------------加载图像---------------------
                            string imgPath = ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim();//图片文件所在路径  
                       
                            FileStream file = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                            file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                           
                            file.Close();
                           
                            //===================上传图片=====================
                           
                                try
                                {
                                  
                                    orcon_tx.Open();
                                    string txOracle = @"insert  into ZEMR_LIS_PACS_REPORT_IMAGE(REPORT_NO,REPORT_TYPE,IMAGE_ID,IMAGE_NAME,
                                                                            IMAGE_CONTENT,COMPRESSION,REMARK) values ('" + bljc.Rows[0]["F_SQXH"].ToString() + "',2,'"
                                      + txlb.Rows[i]["F_txm"].ToString().Trim() + "','" + txlb.Rows[i]["F_txm"].ToString().Trim() + "',:p,0,'')";

                                    OracleCommand orcom_tx1 = new OracleCommand();
                                    orcom_tx1.Connection = orcon_tx;
                                    orcom_tx1.CommandText = txOracle;
                                    orcom_tx1.Parameters.Add("p",System.Data.OracleClient.OracleType.Blob,imgByte.Length);
                                    orcom_tx1.Parameters["p"].Value = imgByte;
                                    int qw = orcom_tx1.ExecuteNonQuery();
                                    orcom_tx1.Dispose();
                                    orcon_tx.Close();
                                
                                  
                                }
                                catch (Exception orcom_tx_e)
                                {

                                    orcon_tx.Close();
                                    log.WriteMyLog("上传图片错误_1" + orcom_tx_e.ToString());
                                    return;
                                }

                        }
                        catch (Exception e_e)
                        {
                        
                            orcon_tx.Close();
                            log.WriteMyLog("上传图片错误" + e_e.ToString());
                            return;
                        }
                    }


                }


            }


        }
    }
    }



