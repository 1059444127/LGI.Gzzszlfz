
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
    //�ɶ����¶���ҽѧʵ����
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
                MessageBox.Show("�������ݿ����������⣡");
                log.WriteMyLog("�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                log.WriteMyLog("������д���");
                return;
            }
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("��������ţ����ݺţ���������");
                return;
            }
            //-----------
          
            //---------------
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
            {
                string brbh = bljc.Rows[0]["F_brbh"].ToString().Trim();

                if (msg == "1")
                {
                    MessageBox.Show("��������ˣ�׼���ϴ�");
                }
                ///////////////
                string gs = "";

                DataTable dt_blk = new DataTable();
                dt_blk = aa.GetDataTable("select F_LISXH from T_BLK_CS where F_BLKMC='" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "'", "blk");

                if (dt_blk.Rows.Count > 0)
                    gs = dt_blk.Rows[0]["F_LISXH"].ToString().Trim();
                else
                {
                    log.WriteMyLog("��ȡ�������Ӧ��ʽ����");
                    return;
                }
                if (gs == "")
                {
                    log.WriteMyLog("��ȡ�������Ӧ��ʽ����");
                    return;
                }
                brbh = brbh.Substring(0, 8) + gs + brbh.Substring(8, 4);
                ///////////////

                ////string zm = bljc.Rows[0]["F_BLH"].ToString().Trim().Substring(0, 1);
                ////if (bljc.Rows[0]["F_BLK"].ToString().Trim()=="����")
                //// brbh = "B" + brbh;
                ////else brbh = zm + brbh;
             
                ////-----------------��ѯ----------
                if (msg == "1")
                {
                    MessageBox.Show("��ѯSAMPLE_RESULT");
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
                    log.WriteMyLog("��ѯ״̬����" + e.ToString());
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
                    MessageBox.Show("��ѯSAMPLE_RESULT��"+x.ToString());
                }
                //-------------����----------------
                ////RESULT_ANALYZE �������
                ////diagnosis_advice,    ��������
                ////assistant_info_1  ��������
                ////assistant_info_2  ������
                ////assistant_info_3  �걾�����
                ////assistant_info_4  ��Ӧ��ϸ���仯
                ////assistant_info_5  ΢������Ŀ
                ////assistant_info_6  ��Ƥϸ�����
                ////assistant_info_7  ����ҽ��
                ////assistant_info_8  �����
                ////remark   ��ע

                //RESULT_ANALYZE �������
                string RESULT_ANALYZE = bljc.Rows[0]["F_blzd"].ToString().Trim();

                //assistant_info_3  �걾�����
                string assistant_info_3 = "";
                //assistant_info_4  ��Ӧ��ϸ���仯
                string assistant_info_4 = "";
                //assistant_info_5  ΢������Ŀ
                string assistant_info_5 = "";
                //assistant_info_6  ��Ƥϸ�����
                string assistant_info_6 = "";
                //remark   ��ע 
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
                if(bljc.Rows[0]["F_blk"].ToString().Trim()=="�����黯")
                {
                    DataTable myzh_num = new DataTable();
                    myzh_num = aa.GetDataTable("select count(*) from T_TJYZ where F_blh='" + blh + "'", "myzh_num");
                       ITEM_NUM=int.Parse( myzh_num.Rows[0][0].ToString());
                }
                ////////////////////
                //ִ�����
                if (msg == "1")
                {
                    MessageBox.Show("ƴ��ִ�����");
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
                    //RESULT_ANALYZE �������
                    // diagnosis_advice,    ��������
                    //assistant_info_1  ��������
                    //assistant_info_2  ������
                    //assistant_info_3  ���鷽��
                    //assistant_info_4  �걾�����
                    //assistant_info_5  ��ԭ��
                    //assistant_info_6  ��֢�̶�
                    //assistant_info_7  ����ҽ��
                    //assistant_info_8  �����
                    //remark   ��ע

                    Oraclestring = @"insert  into dbo.SAMPLE_RESULT(id,requisition_id,RESULT_ANALYZE,remark,diagnosis_advice,assistant_info_1,assistant_info_2,assistant_info_3,assistant_info_4,assistant_info_5,assistant_info_6,assistant_info_7,assistant_info_8,ITEM_NUM,sample_name,lczd,SAMPLE_NUMBER,mcyj,recivedate,report_person,is_jj) 
                         values ('" + brbh.Trim() + "','" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "','" + RESULT_ANALYZE.Trim() + "','" + remark.Trim() + "','" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "','" + bljc.Rows[0]["F_jxsj"].ToString().Trim() + "','"
                                + bljc.Rows[0]["F_tsjc"].ToString().Trim() + "','" + assistant_info_3.Trim() + "','" + assistant_info_4.Trim() + "','" + assistant_info_5.Trim() + "','" + assistant_info_6.Trim() + "','" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "','" + ITEM_NUM + "','"
                                + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "','" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "','" + bljc.Rows[0]["F_bblx"].ToString().Trim() + "','" + bljc.Rows[0]["F_mcyj"].ToString().Trim() + "','" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SFJJ"].ToString().Trim() + "')";

                }
                if (Oraclestring.Trim() == "")
                {
                    MessageBox.Show("������䲻��Ϊ��");
                    return;
                }
                if (msg == "1")
                {
                    MessageBox.Show("ִ����䣺" + Oraclestring);
                    log.WriteMyLog("ִ����䣺" + Oraclestring);
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
                    log.WriteMyLog("�ش��������" + ee.ToString());
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
                    MessageBox.Show("���ݿ�д����ɣ����أ�"+z.ToString());
                }

                ////���������ͼ����Ϣ
                    if (sfsctx.Trim() == "1")//FTP���ط�ʽ
                    {

                        //����FTP����
                        string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                        string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                        string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                        string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "").Replace("\0", "");
                        string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                        string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                        FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                        string txml = bljc.Rows[0]["F_txml"].ToString().Trim();


                        //����ͼ��
                        DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "'", "txlb");
                        if (txlb.Rows.Count < 1)
                        {
                            log.WriteMyLog("�ò�����ͼ�񣬲��ϴ�");
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
                                    log.WriteMyLog("FTP����ͼ�����");
                                    return;
                                }

                                //===================�ϴ�ͼƬ=====================


                                //---�ϴ�jpg----------
                                //----------------�ϴ�ǩ��jpg��ftp---------------------
                                //�ϴ�FTP����
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

                                    //�ж�Ŀ¼�Ƿ����
                                    if (!fw_up.fileCheckExist(ftpURI+"/"+ml1, ml1))
                                    {
                                        //Ŀ¼�����ڣ�����
                                        string stat = "";
                                        fw_up.Makedir(ml1, out stat);
                                    }
                                    if (!fw_up.fileCheckExist(ftpURI + "/" + ml2, ml2))
                                    {
                                        //Ŀ¼�����ڣ�����
                                        string stat = "";
                                        fw_up.Makedir(ml1+"/"+ml2, out stat);
                                    }
                                    if (!fw_up.fileCheckExist(ftpURI + "/" + ml2+"/"+ml3, ml3))
                                    {
                                        //Ŀ¼�����ڣ�����
                                        string stat = "";
                                        fw_up.Makedir(ml1 + "/" + ml2+"/"+ml3, out stat);
                                    }
                                    //--------------
                            
                                    string jpgname = gs + "000000" + bh + "0" + (i+1).ToString() + ".jpg";
                                    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                                    if (fw_up.fileCheckExist(ftpURI + "/" + ml1 + "/" + ml2 + "/" + ml3 + "/", jpgname))
                                    {
                                        //ɾ��ftp�ϵ�jpg�ļ�
                                        fw_up.fileDelete(ftpURI + "/" + ml1 + "/" + ml2 + "/" + ml3, jpgname).ToString();
                                    }
                                    //�ϴ������ɵ�jpg�ļ�
                                    string errMsg = "";
                                    fw_up.Upload("C:\\temp\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ml1 + "/" + ml2 + "/" + ml3, jpgname, out status,ref errMsg);
                                     
                                    if (status == "Error")
                                    {
                                        MessageBox.Show("jpg�ϴ�ʧ�ܣ����������\r\n" + errMsg);
                                    }

                                    try
                                    {
                                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                                    }
                                    catch
                                    {
                                        log.WriteMyLog("ɾ����ʱĿ¼" + blh + "ʧ��");
                                    }


                                }
                                catch
                                {
                                    MessageBox.Show("�ϴ�jpg�ļ��쳣");

                                }

                            }
                            catch
                            {
                                MessageBox.Show("�ϴ�jpg�ļ��쳣");

                            }

                        }


                    }


            }


        }
    }
    }



