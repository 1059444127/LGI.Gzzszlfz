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
    /// ��ɳ�и��ױ���Ժ
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
                MessageBox.Show("�������ݿ����������⣡");
                log.WriteMyLog("�������ݿ����������⣡");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                log.WriteMyLog("������д���");
                return;
            }
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() != "���")
            {
                //��������״̬
                if (jcxx.Rows[0]["F_bgzt"].ToString().Trim() != "�����")
                {

                  return;
                }
                //string bgzt = dataGridView1.CurrentRow.Cells["����״̬"].Value.ToString().Trim();
                if (jcxx.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog(blh + "���޲��˱�ţ�������");
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
                if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                {

                    brlb = "1";
                    his_blh = jcxx.Rows[0]["F_zyh"].ToString().Trim();
                    //zybh = jcxx.Rows[0]["F_yzid"].ToString().Trim();
                }
                else
                {
                    if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "����")
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
                //string odbcstr = f.ReadString("savetohis", "odbcsql", "Data Source=192.168.0.1;Initial Catalog=zonekingnet;User Id=sa;Password=zoneking;"); //��ȡsz.ini�����õ�odbcsql
                string odbcstr = f.ReadString("savetohis", "odbcsql", "DSN=pathnet-tj;UID=sa;PWD=zoneking;"); //��ȡsz.ini�����õ�odbcsql

                if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("���������뵥�ţ�������");
                    return;
                }
                if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
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
                        MessageBox.Show("����jpg�ɹ�");
                    //---�ϴ�jpg----------
                    //----------------�ϴ�ǩ��jpg��ftp---------------------
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
                            //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                            if (fw.fileCheckExist(ftpURI, blh + "_1.jpg"))
                            {
                                //ɾ��ftp�ϵ�jpg�ļ�
                                fw.fileDelete(ftpURI, blh + "_1.jpg").ToString();
                            }
                            //�ϴ������ɵ�jpg�ļ�
                            fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_1.jpg", "", out status);

                            if (status == "Error")
                            {
                                MessageBox.Show("����jpg�ϴ�ʧ�ܣ���������ˣ�");
                                return;
                            }
                        }
                        catch
                        {
                            if (msg == "1")
                                MessageBox.Show("�ϴ�����jpg�ļ��쳣");
                            return;
                        }
                    }
                    else
                    {
                        if (tjtxpath == "")
                        {
                            log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                            return;
                        }
                        try
                        {

                            File.Copy(ftplocal + "\\" + blh + "\\" + blh + "_1.jpg", tjtxpath + "\\" + blh + "_1.jpg", true);
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("Դ·����" + ftplocal + "\\" + blh + "_1.jpg" + "\n" + " Ŀ��·����" + tjtxpath + "\\" + blh + "_1.jpg");
                            log.WriteMyLog("�����ļ��쳣��" + ee.ToString());
                            return;
                        }

                    }
                    if (msg == "1")
                        MessageBox.Show("�ϴ�����jpg�ļ����");
                    try
                    {
                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch
                    {
                        log.WriteMyLog("ɾ����ʱĿ¼" + blh + "ʧ��"); return;
                    }

                    //////////////////////////////////////////////////////////
                    ///////��д������ݿ�-T_SYN_TCT_CHECK////////////////////////////////////

                    string tj_blzd = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                    string tj_jcsj = jcxx.Rows[0]["F_rysj"].ToString().Trim();
                    string tj_ysyj = jcxx.Rows[0]["F_BZ"].ToString().Trim();
                    DataTable TJ_bljc = new DataTable();
                    TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                    if (TJ_bljc.Rows.Count > 0)
                    {
                        if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT���" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                        {
                            tj_jcsj = "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + "��ԭ΢���" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\n";
                            tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\n";
                            ////////////////////////////////////
                            tj_blzd = "��ϣ�" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\n" + "��֢�̶ȣ�" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim();
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                tj_ysyj = tj_ysyj + "�������1��" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\n";
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                tj_ysyj = tj_ysyj + "�������2��" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\n";
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
                        MessageBox.Show("Ӱ��������" + x.ToString());
                    }
                    if (x < 1)
                        log.WriteMyLog("��д���ӿ�ʧ�ܡ�");
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
                log.WriteMyLog("�ش��쳣��" + ee.ToString());
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
                log.WriteMyLog("�ش��쳣��" + ee.ToString());
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
            inxml = inxml + "replbmc=" + (char)34 + "����" + (char)34 + " ";
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
                log.WriteMyLog("�ش���Ϣ:" + inxml);


            object[] Parameters = new object[] { dllconn }; // ʵ��Ϊ 0 

            Type[] ParameterTypes = new Type[] { typeof(StringBuilder) }; // ʵ������Ϊ int 

            DLLWrapper.ModePass[] themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue }; // ���ͷ�ʽΪֵ�� 

            Type Type_Return = typeof(int); // ��������Ϊ int 
            int xx = 0;
            try
            {
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
            }
            catch
            {
                MessageBox.Show("����HIS���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�");

                // func.UnLoadDll();
                return "0";

            }
            if (xx == 0)
            {
                func.LoadFun("SendEmr");

                Parameters = new object[] { "BG01", S2, S3 }; // ʵ��Ϊ 3 
                ParameterTypes = new Type[] { typeof(String), typeof(StringBuilder), typeof(StringBuilder) }; // ʵ������Ϊ pchar 
                themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue }; // ���ͷ�ʽΪֵ�� 
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);

            }
            else
            {
                MessageBox.Show("����HIS���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�");

                //func.UnLoadDll();
                return "0";
            }
            if (xx == 0)
            {
                //func.LoadDll("UnInit");
                //Parameters = new object[] { }; // ʵ��Ϊ 0
                //ParameterTypes = new Type[] { }; // ʵ������Ϊ pchar 
                //themode = new DLLWrapper.ModePass[] { }; // ���ͷ�ʽΪֵ�� 
                //func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
                func.UnLoadDll();
                return S3.ToString();
            }
            else
            {
                MessageBox.Show("����Ż�дHISʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog(S3.ToString());
                func.UnLoadDll();
                return "0";

            }
            // ������ʾ����ʾ���� myfun.Invoke �����Ľ���������� count ����                        

        }
    
    }
}
