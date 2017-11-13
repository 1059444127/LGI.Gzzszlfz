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
    class   npsdryy      //��ƽ�еڶ�ҽԺ
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";

      //  string orcon_str = "Data Source=127.0.0.1;Initial Catalog=pathnet_ca;User Id=pathnet;Password=4s3c2a1p;";//  sql
        string orcon_str = "Data Source= npey;User ID=pacs;Password=pacs";
        

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
            int brlb = 0;
            switch (bljc.Rows[0]["F_brlb"].ToString().Trim())
            {
                case "����": brlb = 1; break;
                case "����": brlb = 2; break;
                case "סԺ": brlb = 3; break;
                case "���": brlb = 4; break;
                default: brlb = 0; break;
            }
            if (msg == "1")
                log.WriteMyLog(orcon_str);
          

            //---------------
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����" || bljc.Rows[0]["F_BGZT"].ToString().Trim() == "��д����")
            {

                ////-----------------��ѯZEMR_PACS_REPORT�Ƿ��д�����ż�¼----------
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
                    log.WriteMyLog("��ѯ״̬����" + e.ToString());
                    orcom.Dispose();
                    orcon.Close(); return;


                }
                finally
                {
                    orcom.Dispose();
                    orcon.Close();

                }
                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "��д����")
                {
                    if (x)
                    {

                        OracleCommand orcom_ZEMR_PACS_REPORT_DEL = new OracleCommand();
                        OracleCommand orcom_IMAGE_DEL = new OracleCommand("delete ZEMR_LIS_PACS_REPORT_IMAGE where REPORT_NO='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "' and REMARK like '%����ͼ��%' ", orcon);
                        orcom_ZEMR_PACS_REPORT_DEL.Connection = orcon;
                        orcom_ZEMR_PACS_REPORT_DEL.CommandText = "delete  ZEMR_PACS_REPORT where PACS_NO='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "' and  INSPECT_TYPE like '%������%' ";
                     
                        try
                        {
                            orcon.Open();
                            orcom_ZEMR_PACS_REPORT_DEL.ExecuteNonQuery();
                            orcom_ZEMR_PACS_REPORT_DEL.Dispose();
                            orcom_IMAGE_DEL.ExecuteNonQuery();
                            orcom_IMAGE_DEL.Dispose();
                            orcon.Close();
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("ɾ���������" + ee.ToString());
                            orcom_ZEMR_PACS_REPORT_DEL.Dispose();
                            orcom_IMAGE_DEL.Dispose();
                            orcon.Close();
                            return;
                        }
                        finally
                        {
                            orcom_ZEMR_PACS_REPORT_DEL.Dispose();
                            orcom_IMAGE_DEL.Dispose();
                            orcon.Close();

                        }

                    }
                }

                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
                {
                    //-------------����----------------
                    int count = 0;
                    string Oraclestring = "";
                    string blzd = "<?xml version=\"1.0\" encoding=\"gb2312\" standalone=\"no\"?><root note=\"��鱨������\"><desc note=\"�������\">" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</desc><diag note=\"���\">" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "</diag></root>";


                    ////-----------------  MODIFY_FLAG = 1;������ˡ��޸�----------
                    //-----------------  MODIFY_FLAG =0;��ˡ�����----------
                    string MODIFY_FLAG = "0";
                   // Byte[] blzdByte = System.Text.Encoding.ASCII.GetBytes(blzd);//��ͼƬת�� Byte�� ��������   
                      Byte[] blzdByte = System.Text.Encoding.Default.GetBytes(blzd);


                      //----------------------
                      string PATIENT_TYPE = "";
                      switch (bljc.Rows[0]["F_BLK"].ToString().Trim())
                      {
                          case "����": PATIENT_TYPE = "51"; break;
                          case "����": PATIENT_TYPE = "52"; break;
                          case "ϸ��ѧ": PATIENT_TYPE = "53"; break;
                          case "TCT": PATIENT_TYPE = "54"; break;
                          case "�������": PATIENT_TYPE = "55"; break;
                          default: PATIENT_TYPE = "56"; break;
                      }

                    Oraclestring = @"insert  into ZEMR_PACS_REPORT(PACS_NO,PATIENT_ID,EVENT_NO,PATIENT_TYPE,PACS_TYPE,PATIENT_NAME,
                    PATIENT_SEX,PATIENT_AGE,IN_DEPT,BED,WARD,ADDRESS,TELEPHONE,MARRIAGE,PROFESSION,CHECKNO,BARCODE_ID,
                    INSPECT_TYPE,INSPECT_SUB_TYPE,INSPECT_NAME,INSTRUMENT_NAME,INSPECT_CONTENT,APPLICANT,OPERATER,
                    INSPECTOR,OPERATER_TIME,REPORT_TIME,LAST_MODIFY_TIME,MODIFY_FLAG,REMARK1,REMARK2,STATUS) 
                     values ('" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZYH"].ToString().Trim()
                         + "','" + brlb + "'," + PATIENT_TYPE + ",'" + bljc.Rows[0]["F_XM"].ToString().Trim() + "','"
                             + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_NL"].ToString().Trim() + "','"
                             + bljc.Rows[0]["F_SJKS"].ToString().Trim() + "','" + bljc.Rows[0]["F_CH"].ToString().Trim() + "','"
                             + bljc.Rows[0]["F_BQ"].ToString().Trim() + "','" + bljc.Rows[0]["F_LXXX"].ToString().Trim() + "','" + "" + "','"
                             + bljc.Rows[0]["F_HY"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZY"].ToString().Trim() + "','"
                             + bljc.Rows[0]["F_BLH"].ToString().Trim() + "','" + "" + "','������','" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "','"
               + "" + "','',:ZD,'"
                   + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','"
                   + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + "" + "',to_date('" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS'),"
                   + "to_date('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()) + "','YYYY-MM-DD HH24:MI:SS')," + MODIFY_FLAG + ",'" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "','" + "" + "','" + MODIFY_FLAG + "')";


//                        sqlstring = @"insert  into ZEMR_PACS_REPORT(PACS_NO,PATIENT_ID,EVENT_NO,PATIENT_TYPE,PACS_TYPE,PATIENT_NAME,
//                                    PATIENT_SEX,PATIENT_AGE,IN_DEPT,BED,WARD,ADDRESS,TELEPHONE,MARRIAGE,PROFESSION,CHECKNO,BARCODE_ID,
//                                    INSPECT_TYPE,INSPECT_SUB_TYPE,INSPECT_NAME,INSTRUMENT_NAME,INSPECT_CONTENT,APPLICANT,OPERATER,
//                                    INSPECTOR,OPERATER_TIME,REPORT_TIME,LAST_MODIFY_TIME,MODIFY_FLAG,REMARK1,REMARK2,STATUS) 
//                                     values ('" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZYH"].ToString().Trim()
//                          + "','" + brlb + "'," + MODIFY_FLAG + ",'" + bljc.Rows[0]["F_XM"].ToString().Trim() + "','"
//                              + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_NL"].ToString().Trim() + "','"
//                              + bljc.Rows[0]["F_SJKS"].ToString().Trim() + "','" + bljc.Rows[0]["F_CH"].ToString().Trim() + "','"
//                              + bljc.Rows[0]["F_BQ"].ToString().Trim() + "','" + bljc.Rows[0]["F_LXXX"].ToString().Trim() + "','" + "" + "','"
//                              + bljc.Rows[0]["F_HY"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZY"].ToString().Trim() + "','"
//                              + bljc.Rows[0]["F_BLH"].ToString().Trim() + "','" + "" + "','" +"������" +"','" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "','"
//                + "" + "',''," + "@ZD" + ",'"
//                    + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','"
//                    + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + "" + "','" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()) + "','"
//                     + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()) + "','" + MODIFY_FLAG + "','" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "','" + "" + "'," + MODIFY_FLAG + ")";

//                    }
                    if(msg=="1")
                    MessageBox.Show("�ش���sql���" + Oraclestring);
                    OracleCommand orcom_insert = new OracleCommand();

                    orcom_insert.Connection = orcon;
                    orcom_insert.CommandText = Oraclestring;
                    orcom_insert.Parameters.Add(":ZD", System.Data.OracleClient.OracleType.Blob, blzdByte.Length);
                    orcom_insert.Parameters[":ZD"].Value = blzdByte;
                try
                    {
                        orcon.Open();

                        count = orcom_insert.ExecuteNonQuery();
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


                    //���������ͼ����Ϣ
                    if (sfsctx.Trim() == "1")//FTP���ط�ʽ
                    {

                        //����FTP����
                        string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                        string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                        string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                        string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                        string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
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

                            bool tx_count = false;

                            OracleConnection orcon_tx = new OracleConnection(orcon_str); 
                          
                            try
                            {

                                //-----------�жϸ�ͼƬ�Ƿ����ϴ�--------------
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
                                    log.WriteMyLog("�жϸ�ͼƬ�Ƿ����ϴ�����" + tx_e.ToString());
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
                                    log.WriteMyLog("FTP����ͼ�����");
                                    return;
                                }
                                //----------------����ͼ��---------------------
                                string imgPath = ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim();//ͼƬ�ļ�����·��  

                                FileStream file = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                                Byte[] imgByte = new Byte[file.Length];//��ͼƬת�� Byte�� ��������   
                                file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   

                                file.Close();

                                //===================�ϴ�ͼƬ=====================

                                try
                                {

                                    orcon_tx.Open();
                                    string txOracle = @"insert  into ZEMR_LIS_PACS_REPORT_IMAGE(REPORT_NO,REPORT_TYPE,IMAGE_ID,IMAGE_NAME,
                                                                            IMAGE_CONTENT,COMPRESSION,REMARK) values ('" + bljc.Rows[0]["F_SQXH"].ToString() + "',2,'"
                                      + txlb.Rows[i]["F_txm"].ToString().Trim() + "','" + txlb.Rows[i]["F_txm"].ToString().Trim() + "',"+":P"+",0,'����ͼ��')";

                                    OracleCommand orcom_tx1 = new OracleCommand();
                                  
                                    orcom_tx1.Connection = orcon_tx;
                                    orcom_tx1.CommandText = txOracle;
                                    orcom_tx1.Parameters.Add(":p",System.Data.OracleClient.OracleType.Blob,imgByte.Length);
                                   orcom_tx1.Parameters[":p"].Value = imgByte;
                                    int qw = orcom_tx1.ExecuteNonQuery();
                                    orcom_tx1.Dispose();
                                    orcon_tx.Close();


                                }
                                catch (Exception orcom_tx_e)
                                {

                                    orcon_tx.Close();
                                    log.WriteMyLog("�ϴ�ͼƬ����_1" + orcom_tx_e.ToString());
                                    return;
                                }

                            }
                            catch (Exception e_e)
                            {

                                orcon_tx.Close();
                                log.WriteMyLog("�ϴ�ͼƬ����" + e_e.ToString());
                                return;
                            }
                        }


                    }


                } return;
            }
            return;

        }
    }
    }



