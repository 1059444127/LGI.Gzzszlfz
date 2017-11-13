
using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OracleClient;
using System.Drawing;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class qz2y
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        public void pathtohis(string F_blh, string debug,string arg)
        {
           
            string msg = f.ReadString("savetohis", "msg", "");
            string topacs = f.ReadString("savetohis", "topacs", "");
            string orcon_str = "Data Source= dzbl;User ID=adt;Password=haitaiinc";


            string dz = "";
            if (arg.Trim() != "")
            {
                if (arg.Contains("^"))
                {
                    string[] args = arg.Split('^');
                    dz = args[4].ToString();
                    F_blh = args[0].ToString();
                }
            }



            string bglx = "cg";
            string bgxh = "1";
            string odbcsql = f.ReadString("savetohis", "odbcsql", "");
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;


            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "bljc");

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
            if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无病人编号（单据号），不处理！");
                return;
            }


            int reAuditFlag = 0;//是否重复审核
            //是否重复审核
            string F_SFCFSH = bljc.Rows[0]["F_SFCFSH"].ToString().Trim();
            if (F_SFCFSH.Trim() == "")
                reAuditFlag = 0;
            else
                reAuditFlag = 1;
            //-----------
            string brlb1 = "o";
            string IPID = bljc.Rows[0]["F_MZH"].ToString().Trim();
            string url = "http://192.168.177.20/pathwebrpt/index_dzblcx.asp?blh=" + F_blh;
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
            {
                IPID = bljc.Rows[0]["F_ZYH"].ToString().Trim();

                brlb1 = "i";
            }


            //查询中间库，判断此病例信息是否存在----------
            string select_to_his = "select *  from ht_ExamReportInfo where RecordFlow='" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "'  and  EXAM_RPT_FLOW='" + F_blh + "'";
            OracleConnection orcon = new OracleConnection(orcon_str);
            OracleCommand orcom1 = new OracleCommand(select_to_his, orcon);
            bool x = false;
            try
            {
                orcon.Open();
                OracleDataReader dr = orcom1.ExecuteReader();
                x = dr.HasRows;
                dr.Close();
                orcom1.Dispose();
            }
            catch (Exception e)
            {
                log.WriteMyLog("查询状态出错：" + e.ToString());
                orcom1.Dispose();
                orcon.Close();
                goto aa;

            }
            finally
            {
                orcom1.Dispose();
                orcon.Close();
            }




            //-----------------------------------------------------------
            //取消审核
            if (dz == "qxsh" || dz == "qxdy")
            {

                if (msg == "1")
                    MessageBox.Show("取消打印或取消审核,修改电子病历中间库的报告状态;" + dz);
                //取消审核，判断T_JCXX表中的 F_SFCFFH标记
                //if (F_SFCFSH == "1")
                //{
                if (x)
                {
                    //修改电子病历中间库的报告状态
                    string Oraclestring = @"update  ht_ExamReportInfo  set AUDIT_STATUS='N' where RecordFlow='" + bljc.Rows[0]["F_blh"].ToString().Trim() + "'  and  EXAM_RPT_FLOW='" + F_blh + "'";
                    //OracleConnection orcon = new OracleConnection(orcon_str);
                    OracleCommand orcom_insert = new OracleCommand(Oraclestring, orcon);
                    int count = -1;
                    if (msg == "1")
                        MessageBox.Show("回写语句：" + Oraclestring);
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
                        goto aa;
                    }
                    finally
                    {
                        orcom_insert.Dispose();
                        orcon.Close();
                    }

                    if (msg == "1")
                        MessageBox.Show("影响行数" + count.ToString());
                }

                // }

            }



            //---------------
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" || dz == "dy" || bljc.Rows[0]["F_BGZT"].ToString().Trim() == "报告延期")
            {
                if (msg == "1")
                    MessageBox.Show("打印或审核,写入或修改电子病历中间库;" + dz);
                //修改审核标记  
                try
                { aa.ExecuteSQL("update T_JCXX set F_SFCFSH='1' where F_BLH='" + F_blh + "'"); }
                catch
                { }
                // 生成jpg格式报告文件
                string jpgname = "";
                string ftpstatus = "";
                mdjpg mdj = new mdjpg();

                try
                {
                    mdj.BMPTOJPG(F_blh, ref jpgname, bglx, bgxh);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
                if (msg == "1")
                MessageBox.Show("生成jpg成功");
                //---上传jpg----------
                //----------------上传签章jpg至ftp---------------------

                string status = "";
                string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages").Replace("\0", "");
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                string ml = DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Year.ToString();
                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/" + ml + "/";
                string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");

                if (ftps == "1")
                {
                   try
                   {

                    //判断目录是否存在
                    if (!fw.fileCheckExist(ftpURI, ml))
                    {
                        //目录不存在，创建
                        string stat = "";
                        fw.Makedir(ml, out stat);
                    }
                    //判断ftp上是否存在该jpg文件
                    if (fw.fileCheckExist(ftpURI + "/" + ml, F_blh + "_" + bglx + "_" + bgxh + ".jpg"))
                    {
                        //删除ftp上的jpg文件
                        fw.fileDelete(ftpURI + "/" + ml, F_blh + "_" + bglx + "_" + bgxh + ".jpg").ToString();
                    }
                    //上传新生成的jpg文件
                    fw.Upload("C:\\temp\\" + F_blh + "\\" + F_blh + "_" + bglx + "_" + bgxh + "_1.jpg", ml, out status);

                    if (status == "Error")
                    {
                        MessageBox.Show("报告jpg上传失败，请重新审核！");
                    }
                   }
                    catch
                    {
                        if (msg == "1")
                        MessageBox.Show("上传报告jpg文件异常");
                    }
                  }
                else
                {
                    if (txpath == "")
                    {
                        log.WriteMyLog("sz.ini txpath图像目录未设置");
                        goto aa;
                    }
                        try
                        {

                            File.Copy(ftplocal + "\\" + F_blh + "\\" + F_blh + "_" + bglx + "_" + bgxh + "_1.jpg", txpath  + "BLBG" +"\\"+ ml+ "\\" + F_blh + "_" + bglx + "_" + bgxh + "_1.jpg", true);
                        }
                        catch(Exception  ee)
                        {
                            log.WriteMyLog("源路径：" + txpath + "BLBG" + "\\"+ml + "\\"  + F_blh + "_" + bglx + "_" + bgxh + "_1.jpg" + "\n" + " 目标路径：" + ftplocal + "\\" + F_blh + "\\" + F_blh + "_" + bglx + "_" + bgxh + "_1.jpg");
                            log.WriteMyLog("复制文件异常！"+ee.ToString());
                            goto aa;
                        }
                    
                } 
                    try
                    {
                        if (System.IO.Directory.Exists(@"c:\temp\" + F_blh))
                            System.IO.Directory.Delete(@"c:\temp\" + F_blh, true);
                    }
                    catch
                    {
                        log.WriteMyLog("删除临时目录" + F_blh + "失败");
                    }
                    
                  



               

                if (msg == "1")
                    MessageBox.Show("上传报告jpg文件完成");
                //--------------------------------------------
                //-------------拼EXAM_RPT_CONT字段--------------

                string blzd = "<id>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</id><code></code>";
                blzd = blzd + "<name></name><findings>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</findings><url>" + url + "</url><result>" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "</result>";
                blzd = blzd + "<examination_items><code></code><name></name><value></value></examination_items>";
                blzd = blzd + "<examination_image><id></id><content></content></examination_image>";

                string Oraclestring = "";

                string AUDIT_TIME = "";
                if (bljc.Rows[0]["F_spare5"].ToString().Trim() == "")
                {
                    AUDIT_TIME = bljc.Rows[0]["F_bgrq"].ToString().Trim().Substring(0, 10);
                }
                else
                {

                    try
                    {
                        AUDIT_TIME = bljc.Rows[0]["F_spare5"].ToString().Trim().Substring(0, 10);
                    }
                    catch
                    {
                        AUDIT_TIME = bljc.Rows[0]["F_bgrq"].ToString().Trim().Substring(0, 10);
                    }
                }


                //                                                       
                if (x)
                {  //更新数据
                    Oraclestring = @"update  ht_ExamReportInfo  set INOUT_FLAG='" + brlb1 + "',IPID='" + IPID + "',EXAM_DIAGNOSE='" + bljc.Rows[0]["F_lczd"].ToString().Trim()
                        + "',EXAM_RPT_CONT='" + blzd + "', AUDIT_USER_ID='" + "" + "',AUDIT_USER_NAME='" + bljc.Rows[0]["F_SHYS"].ToString().Trim()
                        + "',AUDIT_TIME='" + AUDIT_TIME + "',REMARK='" + bljc.Rows[0]["F_bz"].ToString().Trim()
                        + "',ImpFlag='' where RecordFlow='" + bljc.Rows[0]["F_blh"].ToString().Trim() + "'  and  EXAM_RPT_FLOW='" + F_blh + "'";

                }
                else
                {
                    //新插入数据    


                    Oraclestring = @"insert  into ht_ExamReportInfo(RecordFlow,EXAM_RPT_FLOW,ORDER_SN,PID,INOUT_FLAG,IPID,
                    EXAM_CODE,EXAM_NAME,PERFORMED_DEPT_CODE,PERFORMED_DEPT,EXAM_ORDER_NAME,EXAM_DIAGNOSE,EXAM_RPT_CONT,EXAM_REPORTER_CODE,EXAM_REPORTER,EXAM_TIME,AUDIT_STATUS,
                    AUDIT_USER_ID,AUDIT_USER_NAME,AUDIT_TIME,DANGER_FLAG,REMARK,APPLIER_ID,APPLIER,
                    APPLIER_DEPT_CODE,APPLIER_DEPT,APPLY_TIME,ImpFlag,ReturnDesc) 
                     values ('" + bljc.Rows[0]["F_blh"].ToString().Trim() + "','" + F_blh + "','" + bljc.Rows[0]["F_YZID"].ToString().Trim()
                         + "','" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "','" + brlb1 + "','" + IPID + "','" + "" + "','" + "病理检查" + "','"
                         + "" + "','" + "病理科" + "','" + bljc.Rows[0]["F_blk"].ToString().Trim() + "','" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "','"
                         + blzd + "','" + "" + "','" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "','" + bljc.Rows[0]["F_bgrq"].ToString().Trim().Substring(0, 10) + "','" + "Y" + "','" + "" + "','" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','"
               + AUDIT_TIME + "','N','" + bljc.Rows[0]["F_bz"].ToString().Trim() + "','" + "" + "','" + bljc.Rows[0]["F_sjys"].ToString().Trim()
               + "','" + "" + "','" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "','','','')";

                }
                if (msg == "1")
                    MessageBox.Show("回写语句：" + Oraclestring);

                OracleCommand orcom_insert = new OracleCommand();
                orcom_insert.Connection = orcon;
                orcom_insert.CommandText = Oraclestring;

                int count = -1;
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
                    goto aa;
                }
                finally
                {
                    orcom_insert.Dispose();
                    orcon.Close();

                }
                if (msg == "1")
                    MessageBox.Show("影响行数" + count.ToString());
                if (count < 1)
                {
                    log.WriteMyLog("回传报告出错：影响行数" + count.ToString());
                }

            }

                //---------------------------------------------------------
                //-----------------------------------------------------------
                //-------------------------------------------------------------

  aa:          if (topacs == "1")
            {
                if (msg == "1")
                    MessageBox.Show("回传pacs开始");

                string brlb = "";
                string blh = F_blh;
                if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") brlb = "IN";
                if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") brlb = "OU";
                if (brlb == "")
                {
                    if (msg == "1")
                        MessageBox.Show("非住院或门诊病人，不处理");
                    log.WriteMyLog("非住院或门诊病人，不处理！");
                    return;

                }


                if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核" || bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已写报告")
                {

                    if (bljc.Rows[0]["F_SFDY"].ToString().Trim() == "是")
                    {
                       
                        string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                        string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                        string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                        string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                        string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                        string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                        FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                        string ftpserver2 = f.ReadString("ftpup", "ftpip", "").Replace("\0", "");
                        string ftpuser2 = f.ReadString("ftpup", "user", "ftpuser").Replace("\0", "");
                        string ftppwd2 = f.ReadString("ftpup", "pwd", "ftp").Replace("\0", "");
                        string ftplocal2 = f.ReadString("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath2 = f.ReadString("ftpup", "ftpremotepath", "").Replace("\0", "");
                        string ftps2 = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                        FtpWeb fwup = new FtpWeb(ftpserver2, ftpremotepath2, ftpuser2, ftppwd2);

                        string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
                        string bcbg = "\r\n";
                        DataTable bcbgtb = aa.GetDataTable("select * from T_bcbg where F_blh='" + F_blh + "' and F_bc_bgzt='已审核'", "bcbg");
                        for (int i = 0; i < bcbgtb.Rows.Count; i++)
                        {
                            bcbg = bcbg + "[补充报告" + bcbgtb.Rows[i]["F_bc_bgxh"].ToString().Trim() + "]" + bcbgtb.Rows[i]["F_bczd"].ToString().Trim() + "\r\n";

                        }
                        DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + F_blh + "'", "txlb");
                        DataTable TBS = aa.GetDataTable("select * from T_tbs_bg where F_blh='" + F_blh + "'", "tbs");

                        string stxsm = "";

                    
                        Image bgjpg;
                        string jpgguid;
                        if (ftps == "1")
                        {
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                                string ftpstatus = "";
                                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);

                                if (ftpstatus == "Error")
                                {
                                    log.WriteMyLog("ftp出错！");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (txpath == "")
                            {
                                log.WriteMyLog("sz.ini txpath图像目录未设置");
                                return;
                            }
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                                string ftpstatus = "";
                                try
                                {
                                    File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                                }
                                catch
                                {
                                    log.WriteMyLog("共享目录不存在！");
                                    return;
                                }
                            }
                        }
                        if (msg == "1")
                            MessageBox.Show("回传pacs：图像下载完成");


                        string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "UTF-8" + (char)34 + " standalone=" + (char)34 + "yes" + (char)34 + "?>";
                        inxml = inxml + "<XmlDocument>";
                        inxml = inxml + "<Header>";
                        inxml = inxml + "<Application>" + "LOGENE" + "</Application>";
                        inxml = inxml + "<CreateTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</CreateTime>";
                        inxml = inxml + "<VersionID>1,0,0,3</VersionID>";
                        inxml = inxml + "</Header>";

                        inxml = inxml + "<Event>";
                        inxml = inxml + "<Code>C</Code>";
                        inxml = inxml + "<Desc>病理报告</Desc>";
                        inxml = inxml + "<Operator>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Operator>";
                        inxml = inxml + "<UniqueID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blh"].ToString().Trim()) + "</UniqueID>";
                        inxml = inxml + "</Event>";

                        inxml = inxml + "<Patient>";
                        inxml = inxml + "<CardNo></CardNo>";
                        inxml = inxml + "<SiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</SiCard>";
                        inxml = inxml + "<MiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</MiCard>";
                        inxml = inxml + "<SickNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</SickNo>";
                        inxml = inxml + "<PatientClass>" + brlb + "</PatientClass>";
                        inxml = inxml + "<InPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_zyh"].ToString().Trim()) + "</InPatientNo>";
                        inxml = inxml + "<OutPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mzh"].ToString().Trim()) + "</OutPatientNo>";
                        inxml = inxml + "<PatientName>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xm"].ToString().Trim()) + "</PatientName>";
                        inxml = inxml + "<Phoneticize></Phoneticize>";
                        inxml = inxml + "<BirthDate></BirthDate>";
                        inxml = inxml + "<Age>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_nl"].ToString().Trim()) + "</Age>";
                        inxml = inxml + "<Height></Height>";
                        inxml = inxml + "<Weight></Weight>";
                        inxml = inxml + "<BedNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ch"].ToString().Trim()) + "</BedNo>";
                        inxml = inxml + "<NativePlace></NativePlace>";
                        inxml = inxml + "<Nationality></Nationality>";
                        inxml = inxml + "<Sex>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xb"].ToString().Trim()) + "</Sex>";
                        inxml = inxml + "<Nation>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mz"].ToString().Trim()) + "</Nation>";
                        inxml = inxml + "<Address>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lxxx"].ToString().Trim()) + "</Address>";
                        inxml = inxml + "<ZipCode></ZipCode>";
                        inxml = inxml + "<PhoneNumber></PhoneNumber>";
                        inxml = inxml + "<MaritalStatus>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_hy"].ToString().Trim()) + "</MaritalStatus>";
                        inxml = inxml + "<Identity>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sfzh"].ToString().Trim()) + "</Identity>";
                        inxml = inxml + "<Memo></Memo>";
                        inxml = inxml + "</Patient>";

                        inxml = inxml + "<Exam>";
                        inxml = inxml + "<PatientID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blh"].ToString().Trim()) + "</PatientID>";
                        inxml = inxml + "<ExamStatus>70</ExamStatus>";
                        inxml = inxml + "<ExamTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</ExamTime>";
                        inxml = inxml + "<ExamClass>病理</ExamClass>";
                        inxml = inxml + "<ExamSubClass>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blk"].ToString().Trim()) + "</ExamSubClass>";
                        inxml = inxml + "<ExamItem>";
                        inxml = inxml + "<Item code=" + (char)34 + (char)34 + " Cost=" + (char)34 + (char)34 + " Charge=" + (char)34 + (char)34 + " OrderNo=" + (char)34 + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzxm"].ToString().Trim()) + "</Item>";
                        inxml = inxml + "</ExamItem>";
                        inxml = inxml + "<ExamOrgan><Organ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bbmc"].ToString().Trim()) + "</Organ></ExamOrgan>";
                        inxml = inxml + "<Device></Device>";
                        inxml = inxml + "<ExamGroup></ExamGroup>";
                        inxml = inxml + "<PerformDept>病理科</PerformDept>";
                        inxml = inxml + "<Technican>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Technican>";
                        inxml = inxml + "<ExamDuration></ExamDuration>";
                        inxml = inxml + "<Regisiter></Regisiter>";
                        inxml = inxml + "<ReqDept>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjks"].ToString().Trim()) + "</ReqDept>";
                        inxml = inxml + "<ReqWard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bq"].ToString().Trim()) + "</ReqWard>";
                        inxml = inxml + "<ReqPhysician>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjys"].ToString().Trim()) + "</ReqPhysician>";
                        inxml = inxml + "<ScheduledTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ScheduledTime>";
                        inxml = inxml + "<ReqTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ReqTime>";
                        inxml = inxml + "<PhysSign></PhysSign>";
                        inxml = inxml + "<ClinSymp></ClinSymp>";
                        inxml = inxml + "<ClinDiag>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</ClinDiag>";
                        inxml = inxml + "<Memo></Memo>";
                        inxml = inxml + "<ExamRecord></ExamRecord>";
                        inxml = inxml + "<Revisit></Revisit>";
                        inxml = inxml + "</Exam>";

                        inxml = inxml + "<Report>";
                        //泉州二泉不发tct报告
                        //if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
                        //{
                        //    if (TBS.Rows.Count < 0)
                        //    {
                        //         LGZGQClass.log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString() + "此TCT报告无结构化报告内容！");

                        //    }
                        //    else
                        //    {
                        //        inxml = inxml + "<GMReport>";
                        //        inxml = inxml + "<SampleQuality>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BBMYD"].ToString().Trim()) + "</SampleQuality>";
                        //        inxml = inxml + "<CellCount>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBL"].ToString().Trim()) + "</CellCount>";
                        //        inxml = inxml + "<CellItemOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBXM1"].ToString().Trim()) + "</CellItemOne>";
                        //        inxml = inxml + "<CellItemTwo>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBXM2"].ToString().Trim()) + "</CellItemTwo>";
                        //        inxml = inxml + "<AnimalculeOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW1"].ToString().Trim()) + "</AnimalculeOne>";
                        //        inxml = inxml + "<AnimalculeTwo>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW2"].ToString().Trim()) + "</AnimalculeTwo>";
                        //        inxml = inxml + "<AnimalculeThree>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW3"].ToString().Trim()) + "</AnimalculeThree>";
                        //        inxml = inxml + "<VirusItemOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BDXM1"].ToString().Trim()) + "</VirusItemOne>";
                        //        inxml = inxml + "<Inflammation>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_YZCD"].ToString().Trim()) + "</Inflammation>";
                        //        inxml = inxml + "<Erythrocyte>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_HXB"].ToString().Trim()) + "</Erythrocyte>";

                        //        inxml = inxml + "</GMReport>";
                        //    }
                        //}
                        inxml = inxml + "<ReportNo>1</ReportNo>";
                        inxml = inxml + "<Category></Category>";
                        inxml = inxml + "<Description>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</Description>";
                        if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
                        {
                            if (TBS.Rows.Count > 0)
                            {
                                inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBSZD"].ToString().Trim()) + "</Impression>";
                            }
                        }
                        else
                        {
                            inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blzd"].ToString().Trim()) + bcbg + "</Impression>";
                        }
                        inxml = inxml + "<ExamParam></ExamParam>";
                        if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
                        {
                            if (TBS.Rows.Count > 0)
                            {
                                inxml = inxml + "<Recommendation>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BCYJ1"].ToString().Trim()) + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BCYJ2"].ToString().Trim()) + "</Recommendation>";
                            }
                        }
                        else
                        {
                            inxml = inxml + "<Recommendation></Recommendation>";
                        }
                        inxml = inxml + "<AbNormalFlag></AbNormalFlag>";
                        inxml = inxml + "<Reporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Reporter>";
                        inxml = inxml + "<ReportTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</ReportTime>";
                        inxml = inxml + "<MasterReporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</MasterReporter>";
                        inxml = inxml + "<MasterTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Substring(0, 10) + "</MasterTime>";
                        inxml = inxml + "<PrintTime></PrintTime>";
                        inxml = inxml + "<ReportImageArray>";
                        for (int i = 0; i < txlb.Rows.Count; i++)
                        {
                            inxml = inxml + "<ReportImage>";
                            inxml = inxml + "<Path>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_txm"].ToString().Trim()) + "</Path>";
                            inxml = inxml + "<Pos></Pos>";
                            inxml = inxml + "<Desc></Desc>";
                            inxml = inxml + "</ReportImage>";

                        }

                        inxml = inxml + "</ReportImageArray>";
                        inxml = inxml + "</Report>";

                        inxml = inxml + "<Image>";
                        inxml = inxml + "<ImageType></ImageType>";
                        inxml = inxml + "<ImageCount></ImageCount>";
                        inxml = inxml + "<Modality>GM</Modality>";
                        inxml = inxml + "<StudyUID></StudyUID>";

                        inxml = inxml + "</Image>";
                        inxml = inxml + "</XmlDocument>";

                    
                        string filePath = ftplocal + "\\" + blh + ".xml";
                        if (!System.IO.Directory.Exists(ftplocal))
                        {
                            Directory.CreateDirectory(ftplocal);
                        }
                        if (!File.Exists(filePath))//如果文件不存在 
                        {
                            File.Create(filePath).Close();
                        }
                        File.WriteAllText(filePath, "");
                        StreamWriter swx = File.AppendText(filePath);

                        //swx.WriteLine("-------------------------------------------------------------------------------------");
                        //swx.WriteLine("Date:" + DateTime.Now.ToShortDateString() + " Time:" + DateTime.Now.ToString("HH:mm:ss"));
                        swx.WriteLine(inxml);
                        //sw.WriteLine(ex.StackTrace);
                        //swx.WriteLine();
                        swx.Close();

                        string ftpstatusUP = "";
                        fwup.Makedir("BL" + blh, out ftpstatusUP);
                        fwup.Makedir("BL" + blh + "/" + "ReportImage", out ftpstatusUP);
                        for (int i = 0; i < txlb.Rows.Count; i++)
                        {

                          
                            fwup.Upload(ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), "BL" + blh + "/ReportImage", out ftpstatusUP);
                            if (ftpstatusUP == "Error")
                            {
                              
                                return;
                            }
                        }
                        fwup.Upload(ftplocal + "\\" + blh + ".xml", "BL" + blh, out ftpstatusUP);
                        if (msg == "1")
                            MessageBox.Show("回传pacs：xml完成");

                    }
                }

            }
        }
        //------------------
        public void pathtohis_fz(string blh, string bglx, string bgxh,string czlb,string dz)
        {
            if (bglx == "")
                bglx = "cg";
            if (bgxh == "")
                bgxh = "0";

            bglx = bglx.ToLower();
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            string msg = f.ReadString("savetohis", "msg", "");
            string debug = f.ReadString("savetohis", "debug", "");
            string topacs = f.ReadString("savetohis", "topacs", "1");
             string todzbl = f.ReadString("savetohis", "todzbl", "1");
             string isjpg = f.ReadString("savetohis", "isjpg", "1");
             string ispdf = f.ReadString("savetohis", "ispdf", "0");
           
            string orcon_str = "Data Source= dzbl;User ID=adt;Password=haitaiinc";

            string jkmsg = f.ReadString("jkmsg", "jkmsg", "0").Replace("\0", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql", "");
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

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
            if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                ZgqClass.BGHJ(blh, CZY, "保存", "病人编号（单据号）,不处理", "ZGQJK", "");
                log.WriteMyLog("["+blh+"]无病人编号（单据号），不处理");
                return;
            }

            DataTable bcbgtb = new DataTable();
            if(bglx=="bc")
                bcbgtb=aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "' and F_bc_BGXH='" + bgxh + "'", "bcbg");

            DataTable bdbgtb = new DataTable();
            if (bglx == "bd")
                bdbgtb = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "' and F_bd_BGXH='" + bgxh + "'", "bcbg");

             //--save/qxsh/dy/qxdy

            if (dz == "save")
            {
              
                    if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "报告延期")
                    {
                        dz="bgyq";
                    }
                    if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" || bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已发布")
                    {
                         dz="sh";
                    }
                if(bglx=="bd")
                {

                     if (bcbgtb.Rows[0]["F_bd_BGZT"].ToString().Trim() == "已审核")
                    {
                         dz="sh";
                    }
                }
                
                if(bglx=="bc")
                {
                    
                     if (bcbgtb.Rows[0]["F_bc_BGZT"].ToString().Trim() == "已审核")
                    {
                         dz="sh";
                    }
                }
            }

             if (dz=="sh"||dz=="bgyq"||dz=="qxsh")
             {
                 #region  生成jpg
                 if (isjpg=="1")
                 {
                     try
                     {
                         if (dz == "dy" || dz == "sh")
                         {
                             if (debug == "1")
                                 MessageBox.Show("开始生成jpg：" + dz);
                             string jpgname = "";
                             string jpgpath = "";
                             string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                             string err_msg = "";

                             //生成pdf
                             bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.JPG, ref err_msg, ref jpgname, ref jpgpath);
                             if (isrtn)
                             {
                                 jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                 ZgqClass.BGHJ(blh, "his接口", "审核或打印", "生成jpg成功:" + jpgpath + "\\" + jpgname, "ZGQJK", "jpg");
                                 aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                 aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('"+blbh+"','" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");
                                 if (debug == "1")
                                     MessageBox.Show("生成jpg成功：" + dz);
                             }
                             else
                             {
                                 log.WriteMyLog("["+blh+"]"+ err_msg);
                                 ZgqClass.BGHJ(blh, "his接口", "审核或打印", "生成jpg失败" + err_msg, "ZGQJK", "jpg");

                                 if (debug == "1")
                                     MessageBox.Show("病理号：" + blh + "  生成jpg失败，请重新审核！\r\n" + err_msg);

                             }

                         }
                         //删除
                         if (dz == "qxsh" || dz == "qxdy")
                         {
                             if (debug == "1")
                                 MessageBox.Show("删除jpg：" + dz);
                             DataTable dt_pdf = aa.GetDataTable("select *   from  T_BG_PDF where F_BLBH='" + blbh + "' ", "pdf");

                            
                             if (dt_pdf.Rows.Count > 0)
                             {
                                 ZgqPDFJPG zgq = new ZgqPDFJPG();
                                 string err_msg = "";
                                 zgq.DelPDFFile(dt_pdf.Rows[0]["F_ML"].ToString(), dt_pdf.Rows[0]["F_FILENAME"].ToString(), ref err_msg);
                             }
                             aa.ExecuteSQL("delete from T_BG_PDF  where F_BLBH='" + blbh + "' ");
                         }
                     }
                     catch
                     {
                     }
                 }
                  # endregion

                int reAuditFlag = 0;//是否重复审核
                //是否重复审核
                string F_SFCFSH = bljc.Rows[0]["F_SFCFSH"].ToString().Trim();
                if (F_SFCFSH.Trim() == "")
                    reAuditFlag = 0;
                else
                    reAuditFlag = 1;

                //-----------
                ////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////
                ////审核/打印/报告延期（延期原因）//取消打印/取消审核

                #region toDZBL
                string brlb1 = "o";
                string IPID = bljc.Rows[0]["F_MZH"].ToString().Trim();
                string url = f.ReadString("savetohis", "pathwebrpt", "http://192.168.177.20/pathwebrpt/index_jpgbgcx.asp?blh=") + blh;

                if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                {
                    IPID = bljc.Rows[0]["F_ZYH"].ToString().Trim();

                    brlb1 = "i";
                }
                 string ErrMsg="";
                PtoDZBL(debug, bljc,bcbgtb,bdbgtb, blh, bglx, bgxh, orcon_str, dz, url, brlb1, IPID,ref ErrMsg);
                # endregion

                ////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////
                ////审核/打印//取消打印/取消审核

              

                ////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////
                //审核/打印//取消打印/取消审核

                #region  topacs
                 string ErrMsg2="";
                if (topacs == "1")
                {
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "住院" && bljc.Rows[0]["F_brlb"].ToString().Trim() != "门诊")
                    {
                        log.WriteMyLog("["+blh+"]"+"topacs:非住院或门诊病人，不处理！");
                    }
                    else
                    PtoPacs(debug, bljc, blh,ref ErrMsg2);
                }
                # endregion
                if (bglx == "cg")
                {
                    try
                    {
                        DataTable bljc2 = new DataTable();
                        bljc2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");
                        if (bljc2.Rows[0]["F_SCPACS"].ToString() == "1" && bljc2.Rows[0]["F_SCDZBL"].ToString() == "1")
                        {
                            aa.ExecuteSQL("update T_JCXX_FS set F_FSZT='已处理',F_FSSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where F_BLBH='" + blbh + "'");
                        }
                        else
                        {
                              aa.ExecuteSQL("update T_JCXX_FS set F_BZ='"+ErrMsg+";"+ErrMsg2+"' where F_BLBH='" + blbh + "'");
                     
                        }
                    }
                    catch
                    {
                    }
                }
               
 

             }
            }
        

        public void PtoDZBL(string debug, DataTable bljc, DataTable dt_bc,DataTable dt_bd,string blh, string bglx, string bgxh, string orcon_str,string dz,string url,string brlb1,string IPID,ref string  ErrMsg)
        {
            if (debug == "1")
               log.WriteMyLog("["+blh+"]"+"开始回传电子病历：" + dz);

            try
            {
                //  if (dz == "dy"||dz=="sh"||dz=="bgyq"||dz="qxsh"||dz=="qxdy")
                //查询中间库，判断此病例信息是否存在----------
                string select_to_his = "select *  from ht_ExamReportInfo where RecordFlow='" + blh + "-" + bglx + "-" + bgxh + "'  and  EXAM_RPT_FLOW='" + blh + "-" + bglx + "-" + bgxh + "'";
                OracleConnection orcon = new OracleConnection(orcon_str);
                OracleCommand orcom1 = new OracleCommand(select_to_his, orcon);
                bool x = false;
                try
                {
                    orcon.Open();
                    OracleDataReader dr = orcom1.ExecuteReader();
                    x = dr.HasRows;
                    dr.Close();
                    orcom1.Dispose();
                }
                catch (Exception e)
                {
                    ErrMsg = e.Message;
                    log.WriteMyLog("["+blh+"]"+"查询状态出错：" + e.Message);
                    orcom1.Dispose();
                    orcon.Close();
                    return ;

                }
                finally
                {
                    orcom1.Dispose();
                    orcon.Close();
                }

                //-----------------------------------------------------------
                //取消审核
                if (dz == "qxsh" || dz == "qxdy")
                {

                    if (debug == "1")
                        log.WriteMyLog("["+blh+"]"+"取消打印或取消审核,修改电子病历中间库的报告状态;" + dz);

                    if (x)
                    {
                        //修改电子病历中间库的报告状态
                        string Oraclestring = @"update  ht_ExamReportInfo  set AUDIT_STATUS='N',ImpFlag='3',ReturnDesc=''  where RecordFlow='" + blh + "-" + bglx + "-" + bgxh + "'  and  EXAM_RPT_FLOW='" + blh + "-" + bglx + "-" + bgxh + "'";
                        OracleCommand orcom_insert = new OracleCommand(Oraclestring, orcon);
                        int count = -1;
                        if (debug == "1")
                            log.WriteMyLog("["+blh+"]"+"回写语句：" + Oraclestring);
                        try
                        {
                            orcon.Open();
                            count = orcom_insert.ExecuteNonQuery();
                            orcom_insert.Dispose();
                            orcon.Close();
                            if (debug == "1")
                                log.WriteMyLog("["+blh+"]"+"取消审核回写完成：" + count.ToString());
                            return ;
                        }
                        catch (Exception ee)
                        {
                            ErrMsg = ee.Message;
                            log.WriteMyLog("["+blh+"]"+"取消审核出错：" + ee.Message);
                         return ;
                        }
                        finally
                        {
                            orcom_insert.Dispose();
                            orcon.Close();
                        }
               
                      
                    }
                    return ;

                }



                //---------------
                if (dz == "dy" || dz == "sh" || dz == "bgyq")
                {
                    if (debug == "1")
                            log.WriteMyLog("["+blh+"]"+"打印或审核或延期,写入或修改电子病历中间库;" + dz);
                    //修改审核标记  
                        aa.ExecuteSQL("update T_JCXX set F_SFCFSH='1' where F_BLH='" + blh + "'");
                   
                    //--------------------------------------------
                    //-------------拼EXAM_RPT_CONT字段--------------

                    string blzd2 = bljc.Rows[0]["F_BLZD"].ToString().Trim();
                    if (bglx == "bc")
                    {
                        blzd2 = dt_bc.Rows[0]["F_BCZD"].ToString().Trim();
                    }
                    if (bglx == "bd")
                    {
                        blzd2 = dt_bd.Rows[0]["F_BdZD"].ToString().Trim();
                    }

                    if (dz == "bgyq")
                        blzd2 = "报告延期： 延期原因：" + bljc.Rows[0]["F_WFBGYY"].ToString().Trim();

                    string blzd = "<id>" + blh + "-" + bglx + "-" + bgxh + "</id><code></code>";
                    blzd = blzd + "<name></name><findings>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</findings><url>" + url + "</url><result>" + blzd2 + "</result>";
                    blzd = blzd + "<examination_items><code></code><name></name><value></value></examination_items>";
                    blzd = blzd + "<examination_image><id></id><content></content></examination_image>";

                    string Oraclestring = "";

                    string AUDIT_TIME = "";
                    string bgys = bljc.Rows[0]["F_bgys"].ToString().Trim();
                    string bgrq = bljc.Rows[0]["F_bgrq"].ToString().Trim().Substring(0, 10);
                    string shys = bljc.Rows[0]["F_shys"].ToString().Trim();
                    if (bljc.Rows[0]["F_spare5"].ToString().Trim() == "")
                    {
                        AUDIT_TIME = bljc.Rows[0]["F_bgrq"].ToString().Trim().Substring(0, 10);
                    }
                    else
                    {

                        try
                        {
                            AUDIT_TIME = bljc.Rows[0]["F_spare5"].ToString().Trim().Substring(0, 10);
                        }
                        catch
                        {
                            AUDIT_TIME = bljc.Rows[0]["F_bgrq"].ToString().Trim().Substring(0, 10);
                        }
                    }
                    if (bglx == "bd")
                    {
                        AUDIT_TIME = dt_bd.Rows[0]["F_BD_BGRQ"].ToString().Trim().Substring(0, 10);
                        bgys = dt_bd.Rows[0]["F_BD_BGys"].ToString().Trim();
                        bgrq = dt_bd.Rows[0]["F_BD_BGRQ"].ToString().Trim().Substring(0, 10);
                        shys = dt_bd.Rows[0]["F_Bd_shys"].ToString().Trim();
                    }
                    if (bglx == "bc")
                    {
                        AUDIT_TIME = dt_bc.Rows[0]["F_Bc_BGRQ"].ToString().Trim().Substring(0, 10);
                        bgys = dt_bc.Rows[0]["F_Bc_BGys"].ToString().Trim();
                        bgrq = dt_bc.Rows[0]["F_Bc_BGRQ"].ToString().Trim().Substring(0, 10);
                        shys = dt_bc.Rows[0]["F_Bc_shys"].ToString().Trim();
                    }
                    //                                                       
                    if (x)
                    {  //更新数据
                        Oraclestring = @"update  ht_ExamReportInfo  set INOUT_FLAG='" + brlb1 + "',IPID='" + IPID + "',EXAM_DIAGNOSE='" + bljc.Rows[0]["F_lczd"].ToString().Trim()
                            + "',EXAM_RPT_CONT='" + blzd + "', AUDIT_USER_ID='" + "" + "',AUDIT_USER_NAME='" + shys 
                            + "',AUDIT_TIME='" + AUDIT_TIME + "',REMARK='" + bljc.Rows[0]["F_bz"].ToString().Trim()
                            + "',ImpFlag='0',AUDIT_STATUS='Y',ReturnDesc='' where RecordFlow='" + blh + "-" + bglx + "-" + bgxh + "'  and  EXAM_RPT_FLOW='" + blh + "-" + bglx + "-" + bgxh + "'";

                    }
                    else
                    {
                        //新插入数据    


                        Oraclestring = @"insert  into ht_ExamReportInfo(RecordFlow,EXAM_RPT_FLOW,ORDER_SN,PID,INOUT_FLAG,IPID,
                    EXAM_CODE,EXAM_NAME,PERFORMED_DEPT_CODE,PERFORMED_DEPT,EXAM_ORDER_NAME,EXAM_DIAGNOSE,EXAM_RPT_CONT,EXAM_REPORTER_CODE,EXAM_REPORTER,EXAM_TIME,AUDIT_STATUS,
                    AUDIT_USER_ID,AUDIT_USER_NAME,AUDIT_TIME,DANGER_FLAG,REMARK,APPLIER_ID,APPLIER,
                    APPLIER_DEPT_CODE,APPLIER_DEPT,APPLY_TIME,ImpFlag,ReturnDesc) 
                     values ('" + blh + "-" + bglx + "-" + bgxh + "','" + blh + "-" + bglx + "-" + bgxh + "','" + bljc.Rows[0]["F_YZID"].ToString().Trim()
                             + "','" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "','" + brlb1 + "','" + IPID + "','" + "" + "','" + "病理检查" + "','"
                             + "" + "','" + "病理科" + "','" + bljc.Rows[0]["F_blk"].ToString().Trim() + "','" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "','"
                             + blzd + "','" + "" + "','" + bgys + "','" + bgrq + "','" + "Y" + "','" + "" + "','" + shys + "','"
                   + AUDIT_TIME + "','N','" + bljc.Rows[0]["F_bz"].ToString().Trim() + "','" + "" + "','" + bljc.Rows[0]["F_sjys"].ToString().Trim()
                   + "','" + "" + "','" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "','','','')";

                    }
                    if (debug == "1")
                     log.WriteMyLog("["+blh+"]"+"回写语句：" + Oraclestring);

                    OracleCommand orcom_insert = new OracleCommand();
                    orcom_insert.Connection = orcon;
                    orcom_insert.CommandText = Oraclestring;

                    int count = -1;
                    try
                    {
                        orcon.Open();
                        count = orcom_insert.ExecuteNonQuery();
                        orcom_insert.Dispose();
                        orcon.Close();

                        if (count > 0 && bglx == "cg")
                        {
                            aa.ExecuteSQL("update T_JCXX set F_SCDZBL='1' where F_BLH='" + blh + "'");
                            ErrMsg = "";
                        }
                        if (debug == "1")
                            log.WriteMyLog("["+blh+"]"+"回传报告完成" + count.ToString());
                        
                        return ;
                    }
                    catch (Exception ee)
                    {
                        ErrMsg = ee.Message;
                        log.WriteMyLog("["+blh+"]"+"回传报告出错：" + ee.Message);
                    }
                    finally
                    {
                        orcom_insert.Dispose();
                        orcon.Close();

                    }
                  
                    if (count < 1)
                    {
                        log.WriteMyLog("["+blh+"]"+"回传报告出错：影响行数" + count.ToString());
                        return ;
                    }
                    return ;
                }
            }
            catch(Exception  ee2)
            {
               log.WriteMyLog("["+blh+"]"+"异常：" + ee2.Message); 
                return ;
            }
        }

        public void PtoPacs(string debug, DataTable bljc,string blh,ref string  ErrMsg)
        {
              if (debug == "1")
                 log.WriteMyLog("["+blh+"]"+"回传pacs开始");

                string brlb = "";
            
                if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") brlb = "IN";
                if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") brlb = "OU";
                if (brlb == "")
                {
                    ErrMsg = "非住院或门诊病人，不处理";
                    log.WriteMyLog("["+blh+"]"+"非住院或门诊病人，不处理！");
                    return;
                }


                if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核" || bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已写报告")
                {

                    if (bljc.Rows[0]["F_SFDY"].ToString().Trim() == "是")
                    {

                        string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                        string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                        string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                        string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                        string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                        string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                        FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                        string ftpserver2 = f.ReadString("ftpup", "ftpip", "").Replace("\0", "");
                        string ftpuser2 = f.ReadString("ftpup", "user", "ftpuser").Replace("\0", "");
                        string ftppwd2 = f.ReadString("ftpup", "pwd", "ftp").Replace("\0", "");
                        string ftplocal2 = f.ReadString("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath2 = f.ReadString("ftpup", "ftpremotepath", "").Replace("\0", "");
                        string ftps2 = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                        FtpWeb fwup = new FtpWeb(ftpserver2, ftpremotepath2, ftpuser2, ftppwd2);

                        string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
                        string bcbg = "\r\n";
                        DataTable bcbgtb = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "' and F_bc_DYZT='是'", "bcbg");
                        for (int i = 0; i < bcbgtb.Rows.Count; i++)
                        {
                            bcbg = bcbg + "[补充报告" + bcbgtb.Rows[i]["F_bc_bgxh"].ToString().Trim() + "]" + bcbgtb.Rows[i]["F_bczd"].ToString().Trim() + "\r\n";

                        }
                        DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");
                        DataTable TBS = aa.GetDataTable("select * from T_tbs_bg where F_blh='" + blh + "'", "tbs");

                        string stxsm = "";


                        Image bgjpg;
                        string jpgguid;
                        if (ftps == "1")
                        {
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                                string ftpstatus = "";
                                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);

                                if (ftpstatus == "Error")
                                {
                                    ErrMsg = "ftp下载错误";
                                    log.WriteMyLog("["+blh+"]"+"ftp出错！");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (txpath == "")
                            {
                                ErrMsg = "txpath图像目录未设置";
                                log.WriteMyLog("["+blh+"]"+"sz.ini txpath图像目录未设置");
                                return;
                            }
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                                string ftpstatus = "";
                                try
                                {
                                    File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                                }
                                catch
                                {
                                    ErrMsg = "共享目录不存在";
                                  log.WriteMyLog("["+blh+"]"+"共享目录不存在！");
                                    return;
                                }
                            }
                        }
                       log.WriteMyLog("["+blh+"]"+"回传pacs：图像下载完成");


                        string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "UTF-8" + (char)34 + " standalone=" + (char)34 + "yes" + (char)34 + "?>";
                        inxml = inxml + "<XmlDocument>";
                        inxml = inxml + "<Header>";
                        inxml = inxml + "<Application>" + "LOGENE" + "</Application>";
                        inxml = inxml + "<CreateTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</CreateTime>";
                        inxml = inxml + "<VersionID>1,0,0,3</VersionID>";
                        inxml = inxml + "</Header>";

                        inxml = inxml + "<Event>";
                        inxml = inxml + "<Code>C</Code>";
                        inxml = inxml + "<Desc>病理报告</Desc>";
                        inxml = inxml + "<Operator>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Operator>";
                        inxml = inxml + "<UniqueID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blh"].ToString().Trim()) + "</UniqueID>";
                        inxml = inxml + "</Event>";

                        inxml = inxml + "<Patient>";
                        inxml = inxml + "<CardNo></CardNo>";
                        inxml = inxml + "<SiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</SiCard>";
                        inxml = inxml + "<MiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</MiCard>";
                        inxml = inxml + "<SickNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</SickNo>";
                        inxml = inxml + "<PatientClass>" + brlb + "</PatientClass>";
                        inxml = inxml + "<InPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_zyh"].ToString().Trim()) + "</InPatientNo>";
                        inxml = inxml + "<OutPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mzh"].ToString().Trim()) + "</OutPatientNo>";
                        inxml = inxml + "<PatientName>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xm"].ToString().Trim()) + "</PatientName>";
                        inxml = inxml + "<Phoneticize></Phoneticize>";
                        inxml = inxml + "<BirthDate></BirthDate>";
                        inxml = inxml + "<Age>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_nl"].ToString().Trim()) + "</Age>";
                        inxml = inxml + "<Height></Height>";
                        inxml = inxml + "<Weight></Weight>";
                        inxml = inxml + "<BedNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ch"].ToString().Trim()) + "</BedNo>";
                        inxml = inxml + "<NativePlace></NativePlace>";
                        inxml = inxml + "<Nationality></Nationality>";
                        inxml = inxml + "<Sex>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xb"].ToString().Trim()) + "</Sex>";
                        inxml = inxml + "<Nation>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mz"].ToString().Trim()) + "</Nation>";
                        inxml = inxml + "<Address>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lxxx"].ToString().Trim()) + "</Address>";
                        inxml = inxml + "<ZipCode></ZipCode>";
                        inxml = inxml + "<PhoneNumber></PhoneNumber>";
                        inxml = inxml + "<MaritalStatus>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_hy"].ToString().Trim()) + "</MaritalStatus>";
                        inxml = inxml + "<Identity>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sfzh"].ToString().Trim()) + "</Identity>";
                        inxml = inxml + "<Memo></Memo>";
                        inxml = inxml + "</Patient>";

                        inxml = inxml + "<Exam>";
                        inxml = inxml + "<PatientID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blh"].ToString().Trim()) + "</PatientID>";
                        inxml = inxml + "<ExamStatus>70</ExamStatus>";
                        inxml = inxml + "<ExamTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</ExamTime>";
                        inxml = inxml + "<ExamClass>病理</ExamClass>";
                        inxml = inxml + "<ExamSubClass>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blk"].ToString().Trim()) + "</ExamSubClass>";
                        inxml = inxml + "<ExamItem>";
                        inxml = inxml + "<Item code=" + (char)34 + (char)34 + " Cost=" + (char)34 + (char)34 + " Charge=" + (char)34 + (char)34 + " OrderNo=" + (char)34 + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzxm"].ToString().Trim()) + "</Item>";
                        inxml = inxml + "</ExamItem>";
                        inxml = inxml + "<ExamOrgan><Organ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bbmc"].ToString().Trim()) + "</Organ></ExamOrgan>";
                        inxml = inxml + "<Device></Device>";
                        inxml = inxml + "<ExamGroup></ExamGroup>";
                        inxml = inxml + "<PerformDept>病理科</PerformDept>";
                        inxml = inxml + "<Technican>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Technican>";
                        inxml = inxml + "<ExamDuration></ExamDuration>";
                        inxml = inxml + "<Regisiter></Regisiter>";
                        inxml = inxml + "<ReqDept>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjks"].ToString().Trim()) + "</ReqDept>";
                        inxml = inxml + "<ReqWard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bq"].ToString().Trim()) + "</ReqWard>";
                        inxml = inxml + "<ReqPhysician>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjys"].ToString().Trim()) + "</ReqPhysician>";
                        inxml = inxml + "<ScheduledTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ScheduledTime>";
                        inxml = inxml + "<ReqTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ReqTime>";
                        inxml = inxml + "<PhysSign></PhysSign>";
                        inxml = inxml + "<ClinSymp></ClinSymp>";
                        inxml = inxml + "<ClinDiag>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</ClinDiag>";
                        inxml = inxml + "<Memo></Memo>";
                        inxml = inxml + "<ExamRecord></ExamRecord>";
                        inxml = inxml + "<Revisit></Revisit>";
                        inxml = inxml + "</Exam>";

                        inxml = inxml + "<Report>";
                        //泉州二泉不发tct报告
                        //if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
                        //{
                        //    if (TBS.Rows.Count < 0)
                        //    {
                        //         LGZGQClass.log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString() + "此TCT报告无结构化报告内容！");

                        //    }
                        //    else
                        //    {
                        //        inxml = inxml + "<GMReport>";
                        //        inxml = inxml + "<SampleQuality>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BBMYD"].ToString().Trim()) + "</SampleQuality>";
                        //        inxml = inxml + "<CellCount>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBL"].ToString().Trim()) + "</CellCount>";
                        //        inxml = inxml + "<CellItemOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBXM1"].ToString().Trim()) + "</CellItemOne>";
                        //        inxml = inxml + "<CellItemTwo>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBXM2"].ToString().Trim()) + "</CellItemTwo>";
                        //        inxml = inxml + "<AnimalculeOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW1"].ToString().Trim()) + "</AnimalculeOne>";
                        //        inxml = inxml + "<AnimalculeTwo>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW2"].ToString().Trim()) + "</AnimalculeTwo>";
                        //        inxml = inxml + "<AnimalculeThree>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW3"].ToString().Trim()) + "</AnimalculeThree>";
                        //        inxml = inxml + "<VirusItemOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BDXM1"].ToString().Trim()) + "</VirusItemOne>";
                        //        inxml = inxml + "<Inflammation>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_YZCD"].ToString().Trim()) + "</Inflammation>";
                        //        inxml = inxml + "<Erythrocyte>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_HXB"].ToString().Trim()) + "</Erythrocyte>";

                        //        inxml = inxml + "</GMReport>";
                        //    }
                        //}
                        inxml = inxml + "<ReportNo>1</ReportNo>";
                        inxml = inxml + "<Category></Category>";
                        inxml = inxml + "<Description>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</Description>";
                        //if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
                        //{
                        //    if (TBS.Rows.Count > 0)
                        //    {
                        //        inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBSZD"].ToString().Trim()) + "</Impression>";
                        //    }
                        //}
                        //else
                        //{
                            inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blzd"].ToString().Trim()+"\r\n"+ bcbg) + "</Impression>";
                        //}
                        inxml = inxml + "<ExamParam></ExamParam>";
                        //if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
                        //{
                        //    if (TBS.Rows.Count > 0)
                        //    {
                     //   inxml = inxml + "<Recommendation>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BCYJ1"].ToString().Trim()) + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BCYJ2"].ToString().Trim()) + "</Recommendation>";
                        //    }
                        //}
                        //else
                        //{
                            inxml = inxml + "<Recommendation></Recommendation>";
                      //  }
                        inxml = inxml + "<AbNormalFlag></AbNormalFlag>";
                        inxml = inxml + "<Reporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Reporter>";
                        inxml = inxml + "<ReportTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</ReportTime>";
                        inxml = inxml + "<MasterReporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</MasterReporter>";
                        inxml = inxml + "<MasterTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Substring(0, 10) + "</MasterTime>";
                        inxml = inxml + "<PrintTime></PrintTime>";
                        inxml = inxml + "<ReportImageArray>";
                        for (int i = 0; i < txlb.Rows.Count; i++)
                        {
                            inxml = inxml + "<ReportImage>";
                            inxml = inxml + "<Path>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_txm"].ToString().Trim()) + "</Path>";
                            inxml = inxml + "<Pos></Pos>";
                            inxml = inxml + "<Desc></Desc>";
                            inxml = inxml + "</ReportImage>";

                        }

                        inxml = inxml + "</ReportImageArray>";
                        inxml = inxml + "</Report>";

                        inxml = inxml + "<Image>";
                        inxml = inxml + "<ImageType></ImageType>";
                        inxml = inxml + "<ImageCount></ImageCount>";
                        inxml = inxml + "<Modality>GM</Modality>";
                        inxml = inxml + "<StudyUID></StudyUID>";

                        inxml = inxml + "</Image>";
                        inxml = inxml + "</XmlDocument>";


                        string filePath = ftplocal + "\\" + blh + ".xml";
                        if (!System.IO.Directory.Exists(ftplocal))
                        {
                            Directory.CreateDirectory(ftplocal);
                        }
                        if (!File.Exists(filePath))//如果文件不存在 
                        {
                            File.Create(filePath).Close();
                        }
                        File.WriteAllText(filePath, "");
                        StreamWriter swx = File.AppendText(filePath);

                        //swx.WriteLine("-------------------------------------------------------------------------------------");
                        //swx.WriteLine("Date:" + DateTime.Now.ToShortDateString() + " Time:" + DateTime.Now.ToString("HH:mm:ss"));
                        swx.WriteLine(inxml);
                        //sw.WriteLine(ex.StackTrace);
                        //swx.WriteLine();
                        swx.Close();

                        string ftpstatusUP = "";
                        fwup.Makedir("BL" + blh, out ftpstatusUP);
                        fwup.Makedir("BL" + blh + "/" + "ReportImage", out ftpstatusUP);
                        for (int i = 0; i < txlb.Rows.Count; i++)
                        {

                            fwup.Upload(ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), "BL" + blh + "/ReportImage", out ftpstatusUP);
                            if (ftpstatusUP == "Error")
                            {
                                ErrMsg = "上传图片错误";
                                return;
                            }
                        }
                        fwup.Upload(ftplocal + "\\" + blh + ".xml", "BL" + blh, out ftpstatusUP);

                        if (ftpstatusUP != "Error")
                        {
                            aa.ExecuteSQL("update T_JCXX set F_SCPACS='1' where F_BLH='" + blh + "'");
                            ErrMsg = "";
                        }

                      log.WriteMyLog("["+blh+"]"+"回传pacs：xml完成");

                    }
                }

            }

        public void PtoPacs2(string debug, DataTable bljc,string blh, string bglx, string bgxh,string dz)
           {
               if (debug == "1")
                   MessageBox.Show("回传pacs开始");

               string brlb = "";
               if (bglx == "bd")
                   return;


               if (dz == "dy" || dz == "sh" || dz == "bgyq")
               {
                   string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                   string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                   string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                   string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                   string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                   string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                   string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                   FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                   string ftpserver2 = f.ReadString("ftpup", "ftpip", "").Replace("\0", "");
                   string ftpuser2 = f.ReadString("ftpup", "user", "ftpuser").Replace("\0", "");
                   string ftppwd2 = f.ReadString("ftpup", "pwd", "ftp").Replace("\0", "");
                   string ftplocal2 = f.ReadString("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
                   string ftpremotepath2 = f.ReadString("ftpup", "ftpremotepath", "").Replace("\0", "");
                   string ftps2 = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                   FtpWeb fwup = new FtpWeb(ftpserver2, ftpremotepath2, ftpuser2, ftppwd2);

                   string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
                   string blzd = bljc.Rows[0]["F_BLZD"].ToString().Trim();
                   DataTable bcbgtb = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "' and F_bc_bgxh='" + bgxh + "'", "bcbg");
                   if (bglx == "bc")
                   {

                       blzd = bcbgtb.Rows[0]["F_bczd"].ToString().Trim() + "\r\n";

                   }
                   DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");
                   DataTable TBS = aa.GetDataTable("select * from T_tbs_bg where F_blh='" + blh + "'", "tbs");

                   string stxsm = "";


                   Image bgjpg;
                   string jpgguid;
                   if (ftps == "1")
                   {
                       for (int i = 0; i < txlb.Rows.Count; i++)
                       {
                           stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                           string ftpstatus = "";
                           fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);

                           if (ftpstatus == "Error")
                           {
                               log.WriteMyLog("topacs:ftp出错！");
                               return;
                           }
                       }
                   }
                   else
                   {
                       if (txpath == "")
                       {
                           log.WriteMyLog("topacs:sz.ini txpath图像目录未设置");
                           return;
                       }
                       for (int i = 0; i < txlb.Rows.Count; i++)
                       {
                           stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                           string ftpstatus = "";
                           try
                           {
                               File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                           }
                           catch
                           {
                               log.WriteMyLog("topacs:共享目录不存在！");
                               return;
                           }
                       }
                   }
                   if (debug == "1")
                       MessageBox.Show("topacs：图像下载完成");


                   string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "UTF-8" + (char)34 + " standalone=" + (char)34 + "yes" + (char)34 + "?>";
                   inxml = inxml + "<XmlDocument>";
                   inxml = inxml + "<Header>";
                   inxml = inxml + "<Application>" + "LOGENE" + "</Application>";
                   if (bglx == "bc")
                       inxml = inxml + "<CreateTime>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_BGRQ"].ToString().Trim()) + " 00:00:00</CreateTime>";
                   else
                       inxml = inxml + "<CreateTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</CreateTime>";

                   inxml = inxml + "<VersionID>1,0,0,3</VersionID>";
                   inxml = inxml + "</Header>";



                   inxml = inxml + "<Event>";
                   inxml = inxml + "<Code>C</Code>";
                   if (bglx == "bc")
                       inxml = inxml + "<Desc>病理补充报告</Desc>";
                   else
                       inxml = inxml + "<Desc>病理报告</Desc>";
                   if (bglx == "bc")
                       inxml = inxml + "<Operator>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "</Operator>";
                   else
                       inxml = inxml + "<Operator>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Operator>";

                   inxml = inxml + "<UniqueID>" + System.Security.SecurityElement.Escape(blh + bglx + bgxh) + "</UniqueID>";
                   inxml = inxml + "</Event>";

                   inxml = inxml + "<Patient>";
                   inxml = inxml + "<CardNo></CardNo>";
                   inxml = inxml + "<SiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</SiCard>";
                   inxml = inxml + "<MiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</MiCard>";
                   inxml = inxml + "<SickNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</SickNo>";
                   inxml = inxml + "<PatientClass>" + brlb + "</PatientClass>";
                   inxml = inxml + "<InPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_zyh"].ToString().Trim()) + "</InPatientNo>";
                   inxml = inxml + "<OutPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mzh"].ToString().Trim()) + "</OutPatientNo>";
                   inxml = inxml + "<PatientName>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xm"].ToString().Trim()) + "</PatientName>";
                   inxml = inxml + "<Phoneticize></Phoneticize>";
                   inxml = inxml + "<BirthDate></BirthDate>";
                   inxml = inxml + "<Age>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_nl"].ToString().Trim()) + "</Age>";
                   inxml = inxml + "<Height></Height>";
                   inxml = inxml + "<Weight></Weight>";
                   inxml = inxml + "<BedNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ch"].ToString().Trim()) + "</BedNo>";
                   inxml = inxml + "<NativePlace></NativePlace>";
                   inxml = inxml + "<Nationality></Nationality>";
                   inxml = inxml + "<Sex>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xb"].ToString().Trim()) + "</Sex>";
                   inxml = inxml + "<Nation>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mz"].ToString().Trim()) + "</Nation>";
                   inxml = inxml + "<Address>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lxxx"].ToString().Trim()) + "</Address>";
                   inxml = inxml + "<ZipCode></ZipCode>";
                   inxml = inxml + "<PhoneNumber></PhoneNumber>";
                   inxml = inxml + "<MaritalStatus>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_hy"].ToString().Trim()) + "</MaritalStatus>";
                   inxml = inxml + "<Identity>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sfzh"].ToString().Trim()) + "</Identity>";
                   inxml = inxml + "<Memo></Memo>";
                   inxml = inxml + "</Patient>";



                   inxml = inxml + "<Exam>";
                   inxml = inxml + "<PatientID>" + System.Security.SecurityElement.Escape(blh) + "</PatientID>";
                   inxml = inxml + "<ExamStatus>70</ExamStatus>";
                   inxml = inxml + "<ExamTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_qcrq"].ToString().Trim()) + " 00:00:00</ExamTime>";

                   inxml = inxml + "<ExamClass>病理</ExamClass>";
                   inxml = inxml + "<ExamSubClass>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blk"].ToString().Trim()) + "</ExamSubClass>";
                   inxml = inxml + "<ExamItem>";
                   inxml = inxml + "<Item code=" + (char)34 + (char)34 + " Cost=" + (char)34 + (char)34 + " Charge=" + (char)34 + (char)34 + " OrderNo=" + (char)34 + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzxm"].ToString().Trim()) + "</Item>";
                   inxml = inxml + "</ExamItem>";
                   inxml = inxml + "<ExamOrgan><Organ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bbmc"].ToString().Trim()) + "</Organ></ExamOrgan>";
                   inxml = inxml + "<Device></Device>";
                   inxml = inxml + "<ExamGroup></ExamGroup>";
                   inxml = inxml + "<PerformDept>病理科</PerformDept>";

                   if (bglx == "bc")
                       inxml = inxml + "<Technican>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "</Technican>";
                   else
                       inxml = inxml + "<Technican>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Technican>";

                   inxml = inxml + "<ExamDuration></ExamDuration>";
                   inxml = inxml + "<Regisiter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_jsy"].ToString().Trim()) + "</Regisiter>";
                   inxml = inxml + "<ReqDept>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjks"].ToString().Trim()) + "</ReqDept>";
                   inxml = inxml + "<ReqWard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bq"].ToString().Trim()) + "</ReqWard>";
                   inxml = inxml + "<ReqPhysician>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjys"].ToString().Trim()) + "</ReqPhysician>";
                   inxml = inxml + "<ScheduledTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ScheduledTime>";
                   inxml = inxml + "<ReqTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ReqTime>";
                   inxml = inxml + "<PhysSign></PhysSign>";
                   inxml = inxml + "<ClinSymp></ClinSymp>";
                   inxml = inxml + "<ClinDiag>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</ClinDiag>";
                   inxml = inxml + "<Memo></Memo>";
                   inxml = inxml + "<ExamRecord></ExamRecord>";
                   inxml = inxml + "<Revisit></Revisit>";
                   inxml = inxml + "</Exam>";


                   inxml = inxml + "<Report>";
                   inxml = inxml + "<ReportNo>" + bgxh + "</ReportNo>";
                   inxml = inxml + "<Category></Category>";
                   inxml = inxml + "<Description>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</Description>";

                   inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(blzd.Trim()) + "</Impression>";
                   inxml = inxml + "<ExamParam></ExamParam>";
                   inxml = inxml + "<Recommendation></Recommendation>";
                   inxml = inxml + "<AbNormalFlag></AbNormalFlag>";
                   if (bglx == "bc")
                   {
                       inxml = inxml + "<Reporter>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "</Reporter>";
                       inxml = inxml + "<ReportTime>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_BGRQ"].ToString().Trim()).Substring(0, 10) + " 00:00:00</ReportTime>";
                       inxml = inxml + "<MasterReporter>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_SHYS"].ToString().Trim()) + "</MasterReporter>";
                       inxml = inxml + "<MasterTime>" + System.Security.SecurityElement.Escape(bcbgtb.Rows[0]["F_BC_bgrq"].ToString().Trim()).Substring(0, 10) + " 00:00:00<</MasterTime>";

                   }
                   else
                   {
                       inxml = inxml + "<Reporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Reporter>";
                       inxml = inxml + "<ReportTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Substring(0, 10) + " 00:00:00</ReportTime>";
                       inxml = inxml + "<MasterReporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</MasterReporter>";
                       inxml = inxml + "<MasterTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Substring(0, 10) + " 00:00:00<</MasterTime>";

                   }
                   inxml = inxml + "<PrintTime>" + System.Security.SecurityElement.Escape(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Trim()) + "</PrintTime>";
                   inxml = inxml + "<ReportImageArray>";
                   for (int i = 0; i < txlb.Rows.Count; i++)
                   {
                       inxml = inxml + "<ReportImage>";
                       inxml = inxml + "<Path>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_txm"].ToString().Trim()) + "</Path>";
                       inxml = inxml + "<Pos></Pos>";
                       inxml = inxml + "<Desc></Desc>";
                       inxml = inxml + "</ReportImage>";

                   }

                   inxml = inxml + "</ReportImageArray>";
                   inxml = inxml + "</Report>";



                   inxml = inxml + "<Image>";
                   inxml = inxml + "<ImageType></ImageType>";
                   inxml = inxml + "<ImageCount></ImageCount>";
                   inxml = inxml + "<Modality>GM</Modality>";
                   inxml = inxml + "<StudyUID></StudyUID>";

                   inxml = inxml + "</Image>";
                   inxml = inxml + "</XmlDocument>";


                   string filePath = ftplocal + "\\" + blh + bglx + bgxh + ".xml";
                   if (!System.IO.Directory.Exists(ftplocal))
                   {
                       Directory.CreateDirectory(ftplocal);
                   }
                   if (!File.Exists(filePath))//如果文件不存在 
                   {
                       File.Create(filePath).Close();
                   }
                   File.WriteAllText(filePath, "");
                   StreamWriter swx = File.AppendText(filePath);
                   swx.WriteLine(inxml);
                   swx.Close();
                   string ftpstatusUP = "";
                   fwup.Makedir("BL" + blh + bglx + bgxh, out ftpstatusUP);
                   fwup.Makedir("BL" + blh + bglx + bgxh + "/" + "ReportImage", out ftpstatusUP);
                   for (int i = 0; i < txlb.Rows.Count; i++)
                   {
                       fwup.Upload(ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), "BL" + blh + bglx + bgxh + "/ReportImage", out ftpstatusUP);
                       if (ftpstatusUP == "Error")
                       {

                           return;
                       }
                   }
                   fwup.Upload(ftplocal + "\\" + blh + ".xml", "BL" + blh + bglx + bgxh, out ftpstatusUP);
                   if (debug == "1")
                       MessageBox.Show("topacs：xml完成");

               }
        }

        public bool MD_PDF_JPG(string blh, string bglx, string bgxh, string ML, ZgqPDFJPG.Type jpgpdf, ref string err_msg, ref string fileName, ref string fielPath)
        {
            string message = "";
            string jpgname = "";
            ZgqPDFJPG zgq = new ZgqPDFJPG();

            bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf,ref jpgname, ref message);

            string xy = f.ReadString("savetohis", "sctxfs", "3");

            if (isrtn)
            {
                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                    zgq.DelTempFile(blh);
                if (ssa == true)
                {
                    fileName = jpgname;
                    fielPath = ML + "\\" + blh;
                    err_msg = "";
                    return true;
                }
                else
                {
                    err_msg = message;
                    return false;
                }
            }
            else
            {
                zgq.DelTempFile(blh);
                err_msg = message;
                return false;
            }
        }

    }
}
