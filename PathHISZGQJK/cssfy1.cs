using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using HL7;
using readini;
using dbbase;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;

namespace PathHISZGQJK
{
    /// <summary>
    /// 长沙市妇幼保健院
    /// </summary>
    class cssfy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
       
        public void pathtohis(string blh, string debug)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            msg = f.ReadString("savetohis", "msg", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() != "体检")
            {
                //不检查审核状态
                if (jcxx.Rows[0]["F_bgzt"].ToString().Trim() != "已审核")
                {

                  return;
                }
                //string bgzt = dataGridView1.CurrentRow.Cells["报告状态"].Value.ToString().Trim();
                if (jcxx.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog(blh + "，无病人编号，不处理！");
                    return;
                }

                string brlb = "";
             
                string brbh = "";
                string brxb = "";
                string zyh = "";
                string yzid = "";
                string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                string sdrq = jcxx.Rows[0]["F_sdrq"].ToString().Trim();
                string bgrq = jcxx.Rows[0]["F_bgrq"].ToString().Trim();
                brxb = jcxx.Rows[0]["F_xb"].ToString().Trim();
                brxm = jcxx.Rows[0]["F_xm"].ToString().Trim();
                brbh = jcxx.Rows[0]["F_brbh"].ToString().Trim();
               // blh = jcxx.Rows[0]["F_blh"].ToString().Trim();
                zyh = jcxx.Rows[0]["F_zyh"].ToString().Trim();
                yzid = jcxx.Rows[0]["F_yzid"].ToString().Trim();
                string his_blh = "";
                if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                {

                    brlb = "1";
                    his_blh = jcxx.Rows[0]["F_zyh"].ToString().Trim();
                    //zybh = jcxx.Rows[0]["F_yzid"].ToString().Trim();
                }
                else
                {
                    if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
                    {
                        brlb = "0";
                        his_blh = jcxx.Rows[0]["F_mzh"].ToString().Trim();
                    }
                    //zybh = jcxx.Rows[0]["F_brbh"].ToString().Trim();
                }
                bg01(brlb, brbh, blh, "1", his_blh, brxb, bgrq, yzid, sdrq, shys);
            }
            else
            {
                //------------------------------
              //  string msg = f.ReadString("savetohis", "msg", "");
                //string odbcstr = f.ReadString("savetohis", "odbcsql", "Data Source=192.168.0.1;Initial Catalog=zonekingnet;User Id=sa;Password=zoneking;"); //获取sz.ini中设置的odbcsql
                string odbcstr = f.ReadString("savetohis", "odbcsql", "DSN=pathnet-tj;UID=sa;PWD=zoneking;"); //获取sz.ini中设置的odbcsql

                if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("无体检号申请单号，不处理！");
                    return;
                }
                if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                {

                    string jpgname = "";
                    string ftpstatus = "";
                    mdjpg mdj = new mdjpg();
                    try
                    {
                        mdj.BMPTOJPG(blh, ref jpgname, "", "");
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
                    string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/bgjpg").Replace("\0", "");
                    string tjtxpath = f.ReadString("savetohis", "tjtxpath", "bgjpg");


                    string jpgpath = "";
                    if (ftps == "1")
                    {
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                        string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";

                        try
                        {
                            //判断ftp上是否存在该jpg文件
                            if (fw.fileCheckExist(ftpURI, blh + "_1.jpg"))
                            {
                                //删除ftp上的jpg文件
                                fw.fileDelete(ftpURI, blh + "_1.jpg").ToString();
                            }
                            //上传新生成的jpg文件
                            fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_1.jpg", "", out status);

                            if (status == "Error")
                            {
                                MessageBox.Show("报告jpg上传失败，请重新审核！");
                                return;
                            }
                        }
                        catch
                        {
                            if (msg == "1")
                                MessageBox.Show("上传报告jpg文件异常");
                            return;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            log.WriteMyLog("sz.ini txpath图像目录未设置");
                            return;
                        }
                        try
                        {

                            File.Copy(ftplocal + "\\" + blh + "\\" + blh + "_1.jpg", tjtxpath + "\\" + blh + "_1.jpg", true);
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("源路径：" + ftplocal + "\\" + blh + "_1.jpg" + "\n" + " 目标路径：" + tjtxpath + "\\" + blh + "_1.jpg");
                            log.WriteMyLog("复制文件异常！" + ee.ToString());
                            return;
                        }

                    }
                    if (msg == "1")
                        MessageBox.Show("上传报告jpg文件完成");
                    try
                    {
                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch
                    {
                        log.WriteMyLog("删除临时目录" + blh + "失败"); return;
                    }

                    //////////////////////////////////////////////////////////
                    ///////回写体检数据库-T_SYN_TCT_CHECK////////////////////////////////////

                    string tj_blzd = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                    string tj_jcsj = jcxx.Rows[0]["F_rysj"].ToString().Trim();
                    string tj_ysyj = jcxx.Rows[0]["F_BZ"].ToString().Trim();
                    DataTable TJ_bljc = new DataTable();
                    TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                    if (TJ_bljc.Rows.Count > 0)
                    {
                        if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT体检" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                        {
                            tj_jcsj = "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + "病原微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\n";
                            ////////////////////////////////////
                            tj_blzd = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\n" + "炎症程度：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim();
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                tj_ysyj = tj_ysyj + "补充意见1：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\n";
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                tj_ysyj = tj_ysyj + "补充意见2：" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\n";
                        }

                    }
                    tj_blzd = tj_blzd + "\n" + tj_ysyj;

                    if (msg == "1")
                    {
                        MessageBox.Show(tj_jcsj);
                        MessageBox.Show(tj_blzd);

                    }



                    string cmdtxt = @"insert into T_SYN_TCT_CHECK(PACS_CheckID,CISID,PACSItemCode,PatientNameChinese,PatientSex,StudyType,StudyBodyPart,ClinicDiagnose,ClinicSymptom,ClinicAdvice,IMGStrings,StudyState,Check_Doc,Check_Date,Report_Doc,Report_Date,Audit_Doc,Audit_Date) values("
                     + "'" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "','" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "','" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_XB"].ToString().Trim() + "',"
                    + "'BL','" + jcxx.Rows[0]["F_bbmc"].ToString().Trim() + "','" + tj_blzd + "','" + tj_jcsj + "','','" + @"pathimages/bgjpg/" + blh + "_1.jpg" + "',5,'" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "','" + jcxx.Rows[0]["F_SDRQ"].ToString().Trim() + "',"
                    + "'" + jcxx.Rows[0]["F_bgys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_bgrq"].ToString().Trim() + "','" + jcxx.Rows[0]["F_shys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "')";

                   // int x = SQL_ExecuteNonQuery(odbcstr, cmdtxt, msg);
                    int x = Odbc_ExecuteNonQuery(odbcstr, cmdtxt, msg);
                    if (msg == "1")
                    {
                        MessageBox.Show("影响行数：" + x.ToString());
                    }
                    if (x < 1)
                        log.WriteMyLog("回写体检接口失败。");
                    else
                        aa.ExecuteSQL("update T_JCXX  set F_SCBJ='1' where F_BLH='" + blh.Trim() + "'");
                }
                else
                {
                    if (jcxx.Rows[0]["F_SCBJ"].ToString().Trim() == "1")
                    {
                        string cmdtxt = "delete from  T_SYN_TCT_CHECK where  PACS_CheckID='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "' and  CISID='" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "' and PACSItemCode='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "'";
                        int x = Odbc_ExecuteNonQuery(odbcstr, cmdtxt, msg);

                    }
                }
            }
        }
        static string msg = "";
        public int SQL_ExecuteNonQuery(string connectionString, string cmdText, string msg)
        {
            SqlConnection con = con = new SqlConnection(connectionString);
            SqlCommand sqlcom = new SqlCommand(cmdText, con);
            int x = -1;

            if (msg == "1")
                MessageBox.Show(cmdText);
            try
            {
                con.Open();
                x = sqlcom.ExecuteNonQuery();
                sqlcom.Dispose();
                con.Close();
                return x;
            }
            catch (Exception ee)
            {
                con.Close(); sqlcom.Dispose();
                log.WriteMyLog("回传异常：" + ee.ToString());
                return -1;
            }
        }
        public int Odbc_ExecuteNonQuery(string connectionString, string cmdText, string msg)
        {
            OdbcConnection con = con = new OdbcConnection(connectionString);
            OdbcCommand sqlcom = new OdbcCommand(cmdText, con);
            int x = -1;

            if (msg == "1")
                MessageBox.Show(cmdText);
            try
            {
                con.Open();
                x = sqlcom.ExecuteNonQuery();
                sqlcom.Dispose();
                con.Close();
                return x;
            }
            catch (Exception ee)
            {
                con.Close(); sqlcom.Dispose();
                log.WriteMyLog("回传异常：" + ee.ToString());
                return -1;
            }
        }



        public void hcblh(string F_blh, string yymc)
        {
          
        }
        private static string brxm = "";
        private static DLLWrapper func = new DLLWrapper();
        public static string bg01(string brlb, string brbh, string blh, string Debug, string hisblh, string xb, string bgrq, string yzid, string sdrq, string shys)
        {
         
            func.LoadDll("hisinterface.dll");
            func.LoadFun("Init");
            //func.LoadFun("Initconn");

            StringBuilder dllconn = new StringBuilder("");
            StringBuilder S1 = new StringBuilder("JB01");
            string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
            inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
            inxml = inxml + "<METADATA>";
            inxml = inxml + "<FIELDS>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "repno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "reqno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "24" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "cardno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "hzxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sex" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "4" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "age" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "3" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjksmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "bqdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "bqmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "cwdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjysxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjrq" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "replb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "4" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "replbmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "reprq" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "xtbz" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcbw" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "200" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcysxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcksmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "pubtime" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "</FIELDS>";
            inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
            inxml = inxml + "</METADATA>";
            inxml = inxml + "<ROWDATA>";
            inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
            inxml = inxml + "repno=" + (char)34 + blh + (char)34 + " ";
            inxml = inxml + "reqno=" + (char)34 + "0" + (char)34 + " ";


            if (brlb == "1")
            {
                inxml = inxml + "syxh=" + (char)34 + brbh + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + yzid + (char)34 + " ";
                inxml = inxml + "cardno=" + (char)34 + (char)34 + " ";
            }
            else
            {
                inxml = inxml + "syxh=" + (char)34 + "0" + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + brbh + (char)34 + " ";
                inxml = inxml + "cardno=" + (char)34 + yzid + (char)34 + " ";
            }
            inxml = inxml + "blh=" + (char)34 + hisblh + (char)34 + " ";
            inxml = inxml + "hzxm=" + (char)34 + brxm + (char)34 + " ";
            inxml = inxml + "sex=" + (char)34 + xb + (char)34 + " ";
            inxml = inxml + "age=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjksdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjksmc=" + (char)34 + (char)34 + " ";
            inxml = inxml + "bqdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "bqmc=" + (char)34 + (char)34 + " ";
            inxml = inxml + "cwdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjysdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjysxm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjrq=" + (char)34 + sdrq.Replace("-", "") + (char)34 + " ";
            inxml = inxml + "replb=" + (char)34 + "BL" + (char)34 + " ";
            inxml = inxml + "replbmc=" + (char)34 + "病理" + (char)34 + " ";
            inxml = inxml + "reprq=" + (char)34 + bgrq.Replace("-", "") + (char)34 + " ";
            inxml = inxml + "xtbz=" + (char)34 + brlb + (char)34 + " ";
            inxml = inxml + "jcbw=" + (char)34 + (char)34 + " ";
            inxml = inxml + "jcysdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "jcysxm=" + (char)34 + shys + (char)34 + " ";
            inxml = inxml + "jcksdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "jcksmc=" + (char)34 + (char)34 + " ";
            inxml = inxml + "pubtime=" + (char)34 + (char)34 + " ";


            inxml = inxml + "pubtime=" + (char)34 + (char)34 + "/>";
            inxml = inxml + "</ROWDATA>";
            inxml = inxml + "</DATAPACKET>";

            StringBuilder S2 = new StringBuilder(inxml);
            StringBuilder S3 = new StringBuilder(65536);

            if (msg == "1")
                log.WriteMyLog("回传信息:" + inxml);


            object[] Parameters = new object[] { dllconn }; // 实参为 0 

            Type[] ParameterTypes = new Type[] { typeof(StringBuilder) }; // 实参类型为 int 

            DLLWrapper.ModePass[] themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue }; // 传送方式为值传 

            Type Type_Return = typeof(int); // 返回类型为 int 
            int xx = 0;
            try
            {
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
            }
            catch
            {
                MessageBox.Show("连接HIS数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接HIS数据库失败！");

                // func.UnLoadDll();
                return "0";

            }
            if (xx == 0)
            {
                func.LoadFun("SendEmr");

                Parameters = new object[] { "BG01", S2, S3 }; // 实参为 3 
                ParameterTypes = new Type[] { typeof(String), typeof(StringBuilder), typeof(StringBuilder) }; // 实参类型为 pchar 
                themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue }; // 传送方式为值传 
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);

            }
            else
            {
                MessageBox.Show("连接HIS数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接HIS数据库失败！");

                //func.UnLoadDll();
                return "0";
            }
            if (xx == 0)
            {
                //func.LoadDll("UnInit");
                //Parameters = new object[] { }; // 实参为 0
                //ParameterTypes = new Type[] { }; // 实参类型为 pchar 
                //themode = new DLLWrapper.ModePass[] { }; // 传送方式为值传 
                //func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
                func.UnLoadDll();
                return S3.ToString();
            }
            else
            {
                MessageBox.Show("病理号回写HIS失败！");
                if (Debug == "1")
                    log.WriteMyLog(S3.ToString());
                func.UnLoadDll();
                return "0";

            }
            // 弹出提示框，显示调用 myfun.Invoke 方法的结果，即调用 count 函数                        

        }
    
    }
}
