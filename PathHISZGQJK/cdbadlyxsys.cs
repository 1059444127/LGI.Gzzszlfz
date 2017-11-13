
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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
    //成都博奥独立医学实验室
    class cdbadlyxsys
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";

        string ConnectionString = "Provider=MSDAORA;" + "Data Source=lisdb ;" + "User id=blinfo;"+"Password=blinfo;";
          
        
    public void pathtohis(string blh, string yymc)
        {

            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
            string sfsctx = f.ReadString("savetohis", "sfsctx", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql","");
            if (odbcsql.Trim() != "")
                ConnectionString = odbcsql;

            if (msg == "1")
            {
                MessageBox.Show(blh);
            }


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
          
            //---------------
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {
                string brbh = bljc.Rows[0]["F_brbh"].ToString().Trim();

                if (msg == "1")
                {
                    MessageBox.Show("报告已审核，准备上传");
                }
                ///////////////
                string gs = "";

                DataTable dt_blk = new DataTable();
                dt_blk = aa.GetDataTable("select F_LISXH from T_BLK_CS where F_BLKMC='" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "'", "blk");

                if (dt_blk.Rows.Count > 0)
                    gs = dt_blk.Rows[0]["F_LISXH"].ToString().Trim();
                else
                {
                    log.WriteMyLog("获取病例库对应格式错误！");
                    return;
                }
                if (gs == "")
                {
                    log.WriteMyLog("获取病例库对应格式错误！");
                    return;
                }
                brbh = brbh.Substring(0, 8) + gs + brbh.Substring(8, 4);
                ///////////////

                ////string zm = bljc.Rows[0]["F_BLH"].ToString().Trim().Substring(0, 1);
                ////if (bljc.Rows[0]["F_BLK"].ToString().Trim()=="常规")
                //// brbh = "B" + brbh;
                ////else brbh = zm + brbh;
             
                ////-----------------查询----------
                if (msg == "1")
                {
                    MessageBox.Show("查询SAMPLE_RESULT");
                }
                string select_to_his = "select *  from dbo.SAMPLE_RESULT where id='" + brbh + "'  and  requisition_id='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                OleDbConnection orcon = new OleDbConnection(ConnectionString);
                OleDbCommand orcom = new OleDbCommand(select_to_his, orcon);
                bool x = false;
                try
                {
                    orcon.Open();
                    OleDbDataReader dr = orcom.ExecuteReader();
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
                if (msg == "1")
                {
                    MessageBox.Show("查询SAMPLE_RESULT，"+x.ToString());
                }
                //-------------增加----------------
                ////RESULT_ANALYZE 病理诊断
                ////diagnosis_advice,    肉眼所见
                ////assistant_info_1  镜下所见
                ////assistant_info_2  特殊检查
                ////assistant_info_3  标本满意度
                ////assistant_info_4  反应性细胞变化
                ////assistant_info_5  微生物项目
                ////assistant_info_6  上皮细胞情况
                ////assistant_info_7  报告医生
                ////assistant_info_8  病理号
                ////remark   备注

                //RESULT_ANALYZE 病理诊断
                string RESULT_ANALYZE = bljc.Rows[0]["F_blzd"].ToString().Trim();

                //assistant_info_3  标本满意度
                string assistant_info_3 = "";
                //assistant_info_4  反应性细胞变化
                string assistant_info_4 = "";
                //assistant_info_5  微生物项目
                string assistant_info_5 = "";
                //assistant_info_6  上皮细胞情况
                string assistant_info_6 = "";
                //remark   备注 
                string remark = bljc.Rows[0]["F_bz"].ToString().Trim();

                if (bljc.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                {
                    DataTable tbs = new DataTable();
                    tbs = aa.GetDataTable("select * from T_TBS_BG where F_blh='" + blh + "'", "TBSbg");
                    if (tbs.Rows.Count > 0)
                    {
                        //assistant_info_2 = tbs.Rows[0]["F_TBS_JYFF"].ToString().Trim();
                        //assistant_info_3 = tbs.Rows[0]["F_TBS_jyff"].ToString().Trim();
                        assistant_info_3 = tbs.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "   " + tbs.Rows[0]["F_TBS_XBL"].ToString().Trim() + "   " + tbs.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "   " + tbs.Rows[0]["F_TBS_XBXM2"].ToString().Trim()+"   " + tbs.Rows[0]["F_TBS_XBXM3"].ToString().Trim();
                        
                        assistant_info_4 =tbs.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";
                        assistant_info_4 = assistant_info_4 + tbs.Rows[0]["F_TBS_BDXM2"].ToString().Trim() + "\r\n";
                        assistant_info_4 = assistant_info_4 + tbs.Rows[0]["F_TBS_BDXM3"].ToString().Trim() + "\r\n";
                        assistant_info_4 = assistant_info_4 + tbs.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n"; 

                        assistant_info_5 =  tbs.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\r\n";
                        assistant_info_5 = assistant_info_5 + tbs.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n";
                        assistant_info_5 = assistant_info_5 + tbs.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n";
                        assistant_info_5 = assistant_info_5 + tbs.Rows[0]["F_TBS_WSW4"].ToString().Trim() + "\r\n";
                        assistant_info_5 = assistant_info_5 + tbs.Rows[0]["F_TBS_WSW5"].ToString().Trim() + "\r\n";
                        assistant_info_5 = assistant_info_5 + tbs.Rows[0]["F_TBS_WSW6"].ToString().Trim();

                        assistant_info_6 = tbs.Rows[0]["F_TBS_BDXM4"].ToString().Trim() + "\r\n";
                        assistant_info_6 =assistant_info_6+ tbs.Rows[0]["F_TBS_BDXM5"].ToString().Trim();

                        RESULT_ANALYZE = tbs.Rows[0]["F_TBSZD"].ToString().Trim();
                      
                        remark = tbs.Rows[0]["F_TBS_BCYJ1"].ToString().Trim();
                       
                    }
                }

                  int  ITEM_NUM=0;
                if(bljc.Rows[0]["F_blk"].ToString().Trim()=="免疫组化")
                {
                    DataTable myzh_num = new DataTable();
                    myzh_num = aa.GetDataTable("select count(*) from T_TJYZ where F_blh='" + blh + "'", "myzh_num");
                       ITEM_NUM=int.Parse( myzh_num.Rows[0][0].ToString());
                }
                ////////////////////
                //执行语句
                if (msg == "1")
                {
                    MessageBox.Show("拼接执行语句");
                }
                string Oraclestring = "";                                                                                                    
                if (x)
                {
                    Oraclestring = @"update  dbo.SAMPLE_RESULT  set RESULT_ANALYZE  ='" + RESULT_ANALYZE.Trim() + "',remark ='" + remark.Trim() + "',diagnosis_advice='" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "',assistant_info_1='" + bljc.Rows[0]["F_jxsj"].ToString().Trim()
                        + "',assistant_info_2='" + bljc.Rows[0]["F_tsjc"].ToString().Trim() + "',assistant_info_3='" + assistant_info_3.Trim() + "',assistant_info_4='" + assistant_info_4.Trim() + "',assistant_info_5='" + assistant_info_5.Trim() + "',assistant_info_6='" + assistant_info_6.Trim() + "',assistant_info_7='"
                        + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "',assistant_info_8='" + bljc.Rows[0]["F_BLH"].ToString().Trim()
                        + "',ITEM_NUM='" + ITEM_NUM + "',sample_name='" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "',lczd='" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "' ,SAMPLE_NUMBER='" + bljc.Rows[0]["F_bblx"].ToString().Trim() + "',mcyj= '" + bljc.Rows[0]["F_mcyj"].ToString().Trim() + "',recivedate='" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "',report_person='" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "',is_jj='" + bljc.Rows[0]["F_SFJJ"].ToString().Trim() + "' where id='" + brbh + "' and  requisition_id='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                }
                else
                {
                    //RESULT_ANALYZE 病理诊断
                    // diagnosis_advice,    肉眼所见
                    //assistant_info_1  镜下所见
                    //assistant_info_2  特殊检查
                    //assistant_info_3  检验方法
                    //assistant_info_4  标本满意度
                    //assistant_info_5  病原体
                    //assistant_info_6  炎症程度
                    //assistant_info_7  报告医生
                    //assistant_info_8  病理号
                    //remark   备注

                    Oraclestring = @"insert  into dbo.SAMPLE_RESULT(id,requisition_id,RESULT_ANALYZE,remark,diagnosis_advice,assistant_info_1,assistant_info_2,assistant_info_3,assistant_info_4,assistant_info_5,assistant_info_6,assistant_info_7,assistant_info_8,ITEM_NUM,sample_name,lczd,SAMPLE_NUMBER,mcyj,recivedate,report_person,is_jj) 
                         values ('" + brbh.Trim() + "','" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "','" + RESULT_ANALYZE.Trim() + "','" + remark.Trim() + "','" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "','" + bljc.Rows[0]["F_jxsj"].ToString().Trim() + "','"
                                + bljc.Rows[0]["F_tsjc"].ToString().Trim() + "','" + assistant_info_3.Trim() + "','" + assistant_info_4.Trim() + "','" + assistant_info_5.Trim() + "','" + assistant_info_6.Trim() + "','" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "','" + ITEM_NUM + "','"
                                + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "','" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "','" + bljc.Rows[0]["F_bblx"].ToString().Trim() + "','" + bljc.Rows[0]["F_mcyj"].ToString().Trim() + "','" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SFJJ"].ToString().Trim() + "')";

                }
                if (Oraclestring.Trim() == "")
                {
                    MessageBox.Show("插入语句不能为空");
                    return;
                }
                if (msg == "1")
                {
                    MessageBox.Show("执行语句：" + Oraclestring);
                    log.WriteMyLog("执行语句：" + Oraclestring);
                }


                OleDbCommand orcom_insert = new OleDbCommand(Oraclestring, orcon);
                int z = -1;
                try
                {
                    orcon.Open();
                    z = orcom_insert.ExecuteNonQuery();
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
                if (msg == "1")
                {
                    MessageBox.Show("数据库写入完成，返回："+z.ToString());
                }

                ////下面程序中图像信息
                    if (sfsctx.Trim() == "1")//FTP下载方式
                    {

                        //下载FTP参数
                        string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                        string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                        string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                        string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "").Replace("\0", "");
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
                            try
                            {

                                //--------------------------

                                i2 = i + 1;
                                string ftpstatus = "";
                                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                if (ftpstatus == "Error")
                                {
                                    log.WriteMyLog("FTP下载图像出错！");
                                    return;
                                }

                                //===================上传图片=====================


                                //---上传jpg----------
                                //----------------上传签章jpg至ftp---------------------
                                //上传FTP参数
                                string status = "";
                                string ftpServerIP_up = f.ReadString("ftpup", "ftpip", "").Replace("\0", "");
                                string ftpUserID_up = f.ReadString("ftpup", "user", "ftpuser").Replace("\0", "");
                                string ftpPassword_up = f.ReadString("ftpup", "pwd", "ftp").Replace("\0", "");
                                string ftplocal_up = f.ReadString("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                                string ftpRemotePath_up = f.ReadString("ftpup", "ftpremotepath", "pathimages").Replace("\0", "");
                                FtpWeb fw_up = new FtpWeb(ftpServerIP_up, ftpRemotePath_up, ftpUserID_up, ftpPassword_up);
                                string ml1 = bljc.Rows[0]["F_brbh"].ToString().Trim().Substring(0, 4);
                                string ml2 = bljc.Rows[0]["F_brbh"].ToString().Trim().Substring(4, 2);
                                string ml3 = bljc.Rows[0]["F_brbh"].ToString().Trim().Substring(6, 2);
                                string bh = bljc.Rows[0]["F_brbh"].ToString().Trim().Substring(8,4);
                                string ftpURI = "ftp://" + ftpServerIP_up + "/" + ftpRemotePath_up ;
                              
                                try
                                {

                                    //判断目录是否存在
                                    if (!fw_up.fileCheckExist(ftpURI+"/"+ml1, ml1))
                                    {
                                        //目录不存在，创建
                                        string stat = "";
                                        fw_up.Makedir(ml1, out stat);
                                    }
                                    if (!fw_up.fileCheckExist(ftpURI + "/" + ml2, ml2))
                                    {
                                        //目录不存在，创建
                                        string stat = "";
                                        fw_up.Makedir(ml1+"/"+ml2, out stat);
                                    }
                                    if (!fw_up.fileCheckExist(ftpURI + "/" + ml2+"/"+ml3, ml3))
                                    {
                                        //目录不存在，创建
                                        string stat = "";
                                        fw_up.Makedir(ml1 + "/" + ml2+"/"+ml3, out stat);
                                    }
                                    //--------------
                            
                                    string jpgname = gs + "000000" + bh + "0" + (i+1).ToString() + ".jpg";
                                    //判断ftp上是否存在该jpg文件
                                    if (fw_up.fileCheckExist(ftpURI + "/" + ml1 + "/" + ml2 + "/" + ml3 + "/", jpgname))
                                    {
                                        //删除ftp上的jpg文件
                                        fw_up.fileDelete(ftpURI + "/" + ml1 + "/" + ml2 + "/" + ml3, jpgname).ToString();
                                    }
                                    //上传新生成的jpg文件
                                    string errMsg = "";
                                    fw_up.Upload("C:\\temp\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ml1 + "/" + ml2 + "/" + ml3, jpgname, out status,ref errMsg);
                                     
                                    if (status == "Error")
                                    {
                                        MessageBox.Show("jpg上传失败，请重新审核\r\n" + errMsg);
                                    }

                                    try
                                    {
                                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                                    }
                                    catch
                                    {
                                        log.WriteMyLog("删除临时目录" + blh + "失败");
                                    }


                                }
                                catch
                                {
                                    MessageBox.Show("上传jpg文件异常");

                                }

                            }
                            catch
                            {
                                MessageBox.Show("上传jpg文件异常");

                            }

                        }


                    }


            }


        }
    }
    }



