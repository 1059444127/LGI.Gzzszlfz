using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using dbbase;
using PathHISZGQJK;
using ZgqClassPub;

namespace PathHISZGQJK
{
    // ��ҽ��Ժ���ӿ�

////    Create proc proc_get_bl_result
////@applyid varchar(50),    --���뵥��
////@patientname varchar(50),  --����
////@Diagnosis varchar(3072),  --��Ͻ���
////@Feature varchar(1024),  --����
////@reportdoctor varchar(50),  --����ҽ������
////@reporttime datetime,  	--����ʱ��
////@filepath varchar(100),  	--ͼ�����·��
////@sex varchar(50),  		--�����Ա�
////@age varchar(50) 			--����

    class nysy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string F_blh, string yymc)
        {
            string debug = f.ReadString("savetohis", "debug", "");
            string odbcsql = ZgqClass.GetSz("ZGQJK", "odbcsql", "Data Source=192.168.6.43;Initial Catalog=nysy_pe;User Id=sa;Password=sa;");
            string yhmc = f.ReadString("yh", "yhmc", "-").Replace("/0", "");

            string txpath = ZgqClass.GetSz("ZGQJK", "txpath", "");
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            DataTable bljc = new DataTable();
            try
            {
                bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

             if (bljc == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                return;
            }

             if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "���")
                {
                    log.WriteMyLog("����첡�ˣ�������");
                    return;
                }
                if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("��������뵥�ţ�������");
                    return;
                }
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
            {

                if (bljc.Rows[0]["F_TXML"].ToString().Trim() != "")
                {
                   
                        DataTable txlb = new DataTable();
                        txlb = aa.GetDataTable("select * from T_tx where F_blh='" + F_blh + "' and F_sfdy='1'", "txlb");


                        if (txlb.Rows.Count > 0)
                        {
                            if(txpath.Trim()=="")
                            txpath =   bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + txlb.Rows[0]["F_txm"].ToString().Trim();
                            else
                                 txpath =txpath+"\\"+   bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + txlb.Rows[0]["F_txm"].ToString().Trim();

                        }
                        else
                            txpath = "";
                }
                else
                    txpath = "";

                try
                {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + F_blh + "'", "blxx");
                       
                    
                       SqlParameter[] mzORAPAR = new SqlParameter[9];
                        for (int j = 0; j < mzORAPAR.Length; j++)
                        {
                            mzORAPAR[j] = new SqlParameter();
                        }
                        mzORAPAR[0].ParameterName = "@applyid";
                        mzORAPAR[0].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[0].Direction = ParameterDirection.Input;
                        mzORAPAR[0].Size = 50;

                        mzORAPAR[1].ParameterName = "@patientname";//
                        mzORAPAR[1].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[1].Direction = ParameterDirection.Input;
                        mzORAPAR[1].Size = 50;

                        mzORAPAR[2].ParameterName = "@Diagnosis";//
                        mzORAPAR[2].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[2].Direction = ParameterDirection.Input;
                        mzORAPAR[2].Size = 3000;

                        mzORAPAR[3].ParameterName = "@Feature";//
                        mzORAPAR[3].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[3].Direction = ParameterDirection.Input;
                        mzORAPAR[3].Size = 1024;

                        mzORAPAR[4].ParameterName = "@reportdoctor";// 
                        mzORAPAR[4].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[4].Direction = ParameterDirection.Input;
                        mzORAPAR[4].Size = 50;

                        mzORAPAR[5].ParameterName = "@reporttime";// 
                        mzORAPAR[5].SqlDbType = SqlDbType.DateTime;
                        mzORAPAR[5].Direction = ParameterDirection.Input;

                        mzORAPAR[6].ParameterName = "@filepath";// 
                        mzORAPAR[6].SqlDbType = SqlDbType.VarChar;;
                        mzORAPAR[6].Direction = ParameterDirection.Input;
                      mzORAPAR[6].Size = 100;

                        mzORAPAR[7].ParameterName = "@sex";// 
                        mzORAPAR[7].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[7].Direction = ParameterDirection.Input;
                        mzORAPAR[7].Size = 50;

                        mzORAPAR[8].ParameterName = "@age";// 
                        mzORAPAR[8].SqlDbType = SqlDbType.VarChar;
                        mzORAPAR[8].Direction = ParameterDirection.Input;
                        mzORAPAR[8].Size = 50;

                        //////////////////////////////////////////////////////

                        string tj_blzd = bljc.Rows[0]["F_blzd"].ToString().Trim();
                        string tj_jcsj = bljc.Rows[0]["F_jxsj"].ToString().Trim();

                        if (TJ_bljc.Rows.Count > 0)
                        {
                            if (bljc.Rows[0]["F_BGGS"].ToString().Trim() == "TBS" || bljc.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                            {
                                tj_jcsj =  "�걾�����:" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           "+TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           " + TJ_bljc.Rows[0]["F_tbs_xbxm1"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           " + TJ_bljc.Rows[0]["F_tbs_xbxm2"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "           " + TJ_bljc.Rows[0]["F_tbs_xbxm3"].ToString().Trim() + "\r\n";

                                tj_jcsj = tj_jcsj + "��֢�̶ȣ�" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";
                               
                                ////////////////////////////////////
                                tj_blzd =  TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "�������1��" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "�������2��" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\r\n";
                            }
                           
                            //if (bljc.Rows[0]["F_blk"].ToString().Trim() == "HPV")
                            //{
                            //    tj_jcsj = tj_jcsj + "��ⷽ����" + TJ_bljc.Rows[0]["F_FZ_JCFF"].ToString().Trim() + "\n" + "������ݣ�" + TJ_bljc.Rows[0]["F_FZ_JCWD"].ToString().Trim() + "\n";
                            //    tj_blzd = "�������" + TJ_bljc.Rows[0]["F_FZ_JCJG"].ToString().Trim();// "�������" + TJ_bljc.Rows[0]["F_FZ_YYX"].ToString().Trim();
                            //}
                        }
                        if (debug == "1")
                        {
                            MessageBox.Show(tj_jcsj);
                            MessageBox.Show(tj_blzd);
                            MessageBox.Show(txpath);
                        }
                        /////////////////////////////////////////////////////
                        mzORAPAR[0].Value = bljc.Rows[0]["f_sqxh"].ToString().Trim();   //������뵥��
                        mzORAPAR[1].Value = bljc.Rows[0]["f_xm"].ToString().Trim();  // ����
                        mzORAPAR[2].Value = tj_blzd.Trim();   // ��Ͻ���
                        mzORAPAR[3].Value = tj_jcsj.Trim();   //����
                        mzORAPAR[4].Value = bljc.Rows[0]["f_bgys"].ToString().Trim();   // ���ҽ������
                        mzORAPAR[5].Value = DateTime.Parse(bljc.Rows[0]["f_bgrq"].ToString().Trim());   //���ʱ��
                        mzORAPAR[6].Value = txpath;   // ͼ�����·��
                        mzORAPAR[7].Value = bljc.Rows[0]["f_xb"].ToString().Trim();   // �Ա�
                        mzORAPAR[8].Value = bljc.Rows[0]["f_nl"].ToString().Trim();   // ����
                        ////////////////////////////////////////////////////
                        SqlConnection ocn = new SqlConnection(odbcsql);
                        try
                        {
                           SqlCommand mzcmd = new SqlCommand();
                            mzcmd.Connection = ocn;
                            mzcmd.CommandType=CommandType.StoredProcedure;
                            mzcmd.CommandText = "proc_get_bl_result";
                            mzcmd.Parameters.Add(mzORAPAR[0]);
                            mzcmd.Parameters.Add(mzORAPAR[1]);
                            mzcmd.Parameters.Add(mzORAPAR[2]);
                            mzcmd.Parameters.Add(mzORAPAR[3]);
                            mzcmd.Parameters.Add(mzORAPAR[4]);
                            mzcmd.Parameters.Add(mzORAPAR[5]);
                            mzcmd.Parameters.Add(mzORAPAR[6]);
                            mzcmd.Parameters.Add(mzORAPAR[7]);
                            mzcmd.Parameters.Add(mzORAPAR[8]);
                      
                            try
                            {
                                ocn.Open();
                                if (debug == "1")
                                    MessageBox.Show("���ݿ������");

                                mzcmd.ExecuteNonQuery();

                                if (debug == "1")
                                    MessageBox.Show("���ݿ�ִ������,��д�ɹ�");

                                ocn.Close();
                             
                                ZgqClass.BGHJ(F_blh,yhmc, "���","��챨���д�ɹ�","ZGQJK","ZGQJK");
                            }
                            catch (Exception eee)
                            {
                                ocn.Close();
                                log.WriteMyLog("���ݿ�ִ���쳣��" + eee.Message);
                                if (debug == "1")
                                    MessageBox.Show("���ݿ�ִ���쳣��" + eee.Message.ToString());
                                ZgqClass.BGHJ(F_blh, yhmc, "���", "��챨���дʧ�ܣ�" + eee.Message, "ZGQJK", "ZGQJK");
                                return;
                            }
                            finally
                            {
                                if (ocn.State == ConnectionState.Open)
                                    ocn.Close();
                            }
                           
                        }
                        catch (Exception e)
                        {
                            log.WriteMyLog("��������"+e.Message.ToString());
                        }
                    }
                catch (Exception ee)
                {
                    log.WriteMyLog("��������2"+ee.Message.ToString());
                }

            }
        }  
    }
}
