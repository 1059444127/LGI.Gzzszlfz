using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.OleDb;
using ZgqClassPub;
using GLYYTEST;
using System.IO;
using LGDICOM;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    /// <summary>
    /// ����ҽѧԺ����ҽԺ
    /// </summary>
    class lzyxyfsyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string F_blh, string msg,string debug)
        {
        
           string czy = f.ReadString("yh", "yhmc", "");
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("���ݿ������쳣");
                return;
            }
            if (jcxx.Rows.Count <= 0)
            {
                log.WriteMyLog("δ��ѯ���˱���" + F_blh);
                return;
            }

            string  bgzt=jcxx.Rows[0]["F_BGZT"].ToString().Trim();

            if (bgzt == "����Ƭ" || bgzt == "�Ѱ���")
                return;


           string   sqxh= jcxx.Rows[0]["f_sqxh"].ToString().Trim();
            
                  OleDbDB oledb = new OleDbDB();
                  string odbcsql_his = f.ReadString("savetohis", "odbcsql_his", "Provider='MSDAORA';data source=DBSERVER;user id =pacs;password=pacs;");
                  string tohis = f.ReadString("savetohis", "tohis", "1");
                  string topacs = f.ReadString("savetohis", "topacs", "0");
                  string scdicom = f.ReadString("savetohis", "scdicom", "0");
                  string fyqr = f.ReadString("savetohis", "fyqr", "0");
                  debug = f.ReadString("savetohis", "debug", "0");

                if (tohis == "1" )
                {
                    if(sqxh.Trim()=="")
                    {
                         log.WriteMyLog("TOHIS:�������Ϊ�ղ�����");
                    }

                    #region HIS ��д״̬ȷ�Ϸ���


                    if (jcxx.Rows[0]["F_HXJFBZ"].ToString().Trim() != "1" && !jcxx.Rows[0]["F_BRLB"].ToString().Trim().Contains("���"))
                    {

                        if (fyqr == "1")
                        {

                            try
                            {
                                OleDbParameter[] ops = new OleDbParameter[3];
                                for (int j = 0; j < ops.Length; j++)
                                {
                                    ops[j] = new OleDbParameter();
                                }
                                ops[0].ParameterName = "v_test_no";
                                ops[0].OleDbType = OleDbType.VarChar;
                                ops[0].Direction = ParameterDirection.Input;
                                ops[0].Size = 20;
                                ops[0].Value = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

                                ops[1].ParameterName = "v_operator";//
                                ops[1].OleDbType = OleDbType.VarChar;
                                ops[1].Direction = ParameterDirection.Input;
                                ops[1].Size = 10;
                                ops[1].Value = czy.Trim();

                                ops[2].ParameterName = "v_flag";//
                                ops[2].OleDbType = OleDbType.VarChar;
                                ops[2].Direction = ParameterDirection.Input;
                                ops[2].Size = 10;
                                ops[2].Value = "0";
                                //��д�Ǽ�״̬
                             string   message_ee = "";
                                if (debug == "1")
                                    log.WriteMyLog(F_blh + ",��д����״̬");
                                oledb.ExecuteNonQuery(odbcsql_his, "his_vs_exam_inp_fee", ref ops, CommandType.StoredProcedure, ref message_ee);

                                if (message_ee.Trim() != "")
                                {

                                    log.WriteMyLog(F_blh + ",��д�Ǽ�״̬����HIS�洢����his_vs_exam_inp_fee�쳣��" + message_ee);
                                }
                                else
                                {
                                    if (debug == "1")
                                        log.WriteMyLog(F_blh + ",��д�Ǽ�״̬�ɹ�");
                                    //aa.ExecuteSQL("update t_jcxx set F_HXJFBZ='1' where f_blh='" + F_blh + "'");
                                }


                                if (debug == "1")
                                    log.WriteMyLog(F_blh + ",��д�Ʒ�״̬");
                                //��д�շ�״̬
                                OleDbParameter[] ops2 = new OleDbParameter[3];
                                for (int j = 0; j < ops2.Length; j++)
                                {
                                    ops2[j] = new OleDbParameter();
                                }
                                ops2[0].ParameterName = "v_test_no";
                                ops2[0].OleDbType = OleDbType.VarChar;
                                ops2[0].Direction = ParameterDirection.Input;
                                ops2[0].Size = 20;
                                ops2[0].Value = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

                                ops2[1].ParameterName = "v_operator";//
                                ops2[1].OleDbType = OleDbType.VarChar;
                                ops2[1].Direction = ParameterDirection.Input;
                                ops2[1].Size = 10;
                                ops2[1].Value = czy.Trim();

                                ops2[2].ParameterName = "v_flag";//
                                ops2[2].OleDbType = OleDbType.VarChar;
                                ops2[2].Direction = ParameterDirection.Input;
                                ops2[2].Size = 10;
                                ops2[2].Value = "1";
                                string message_ee2 = "";
                                //   OleDbbd.Odbc_ExecuteNonQuery(odbcsql_his, "{ CALL his_vs_exam_inp_fee(?,?,?)}", ref ops2, ref message_ee2);
                                oledb.ExecuteNonQuery(odbcsql_his, "his_vs_exam_inp_fee", ref ops2, CommandType.StoredProcedure, ref message_ee2);
                                if (message_ee2.Trim() != "")
                                {
                                    log.WriteMyLog(F_blh + ",��д�շ�״̬����HIS�洢����his_vs_exam_inp_fee�쳣��" + message_ee2);
                                }
                                else
                                {
                                    if (debug == "1")
                                        log.WriteMyLog(F_blh + ",��д�Ʒ�״̬�ɹ�");
                                    aa.ExecuteSQL("update t_jcxx set F_HXJFBZ='1' where f_blh='" + F_blh + "'");
                                }
                            }
                            catch (Exception err)
                            {
                                log.WriteMyLog(F_blh + ",��д�Ʒ�״̬�쳣��" + err.Message);
                            }
                        }


                    }
                    #endregion

                    #region ��д���棨his���������
                    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "" && bgzt == "�����")
                    {

                        if (debug == "1")
                            log.WriteMyLog(F_blh+",��HIS����");
                        try
                        {
                            string yyx = jcxx.Rows[0]["F_YYX"].ToString().Trim();
                            if (yyx.Contains("����"))
                                yyx = "1";
                            else
                                yyx = "0";
                            string blzd = jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_TSJC"].ToString().Trim();
                            string rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_JXSJ"].ToString().Trim();
                            if (bgzt == "�����")
                            {
                                //DataTable TJ_bljc = new DataTable();
                                //TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + F_blh + "'", "blxx");
                                //if (TJ_bljc.Rows.Count > 0)
                                //{
                                //    if (jcxx.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                                //    {
                                //        rysj = rysj.Trim() + "�걾�����:" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "  " + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + "\r\n" + "��Ӧ��ϸ���仯:" + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH2"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_CH3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH4"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_CH5"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH6"].ToString().Trim() + "\r\n";

                                //        rysj = rysj + "\r\n" + "΢������Ŀ:" + "\r\n" + TJ_bljc.Rows[0]["F_JGH_HP1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_HP2"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_HP3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_HP4"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_HP5"].ToString().Trim() + "\r\n";

                                //        rysj = rysj + "\r\n" + "��Ƥϸ�����:" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_LZXB"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XXB"].ToString().Trim() + "\r\n";
                                //        ////////////////////////////////////
                                //        blzd = TJ_bljc.Rows[0]["F_TBS_ZD"].ToString().Trim() ;

                                //        if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                //            blzd = blzd + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                //    }
                                //}
                            }

                            //////////////////////////////////////////////////////////////////

                            OleDbParameter[] mzORAPAR = new OleDbParameter[24];
                            for (int j = 0; j < mzORAPAR.Length; j++)
                            {
                                mzORAPAR[j] = new OleDbParameter();
                            }
                            //�������뵥id
                            mzORAPAR[0].ParameterName = "v_exam_no";
                            mzORAPAR[0].OleDbType = OleDbType.VarChar;
                            mzORAPAR[0].Direction = ParameterDirection.Input;
                            mzORAPAR[0].Size = 20;
                            mzORAPAR[0].Value = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

                            //������
                            mzORAPAR[1].ParameterName = "v_exam_para";//
                            mzORAPAR[1].OleDbType = OleDbType.VarChar;
                            mzORAPAR[1].Direction = ParameterDirection.Input;
                            mzORAPAR[1].Size = 1000;
                            mzORAPAR[1].Value = "";

                            //�������
                            mzORAPAR[2].ParameterName = "v_description";//
                            mzORAPAR[2].OleDbType = OleDbType.VarChar;
                            mzORAPAR[2].Direction = ParameterDirection.Input;
                            mzORAPAR[2].Size = 2000;
                            if (bgzt == "�����")
                                mzORAPAR[2].Value = rysj;
                            else
                                mzORAPAR[2].Value = "";

                            //ӡ�󣨽��ۣ�
                            mzORAPAR[3].ParameterName = "v_impression";//
                            mzORAPAR[3].OleDbType = OleDbType.VarChar;
                            mzORAPAR[3].Direction = ParameterDirection.Input;
                            mzORAPAR[3].Size = 2000;
                            if (bgzt == "�����")
                                mzORAPAR[3].Value = blzd;
                            else
                                mzORAPAR[3].Value = "";

                            //����
                            mzORAPAR[4].ParameterName = "v_recommendation";// 
                            mzORAPAR[4].OleDbType = OleDbType.VarChar;
                            mzORAPAR[4].Direction = ParameterDirection.Input;
                            mzORAPAR[4].Size = 2000;
                            if (bgzt == "�����")
                                mzORAPAR[4].Value = jcxx.Rows[0]["F_BZ"].ToString().Trim();
                            else
                                mzORAPAR[4].Value = "";

                            //�Ƿ����ԣ�0���ԣ�1���ԣ�
                            mzORAPAR[5].ParameterName = "v_lsabnormal";// 
                            mzORAPAR[5].OleDbType = OleDbType.VarChar;
                            mzORAPAR[5].Direction = ParameterDirection.Input;
                            mzORAPAR[5].Size = 1;
                            if (bgzt == "�����")
                                mzORAPAR[5].Value = yyx;
                            else
                                mzORAPAR[5].Value = "";

                            //��ע������״̬��
                            mzORAPAR[6].ParameterName = "v_memo";// 
                            mzORAPAR[6].OleDbType = OleDbType.VarChar;
                            mzORAPAR[6].Direction = ParameterDirection.Input;
                            mzORAPAR[6].Size = 2000;
                            mzORAPAR[6].Value = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

                            //���˱�ʾ��
                            mzORAPAR[7].ParameterName = "v_patient_id";// 
                            mzORAPAR[7].OleDbType = OleDbType.VarChar;
                            mzORAPAR[7].Direction = ParameterDirection.Input;
                            mzORAPAR[7].Size = 20;
                            mzORAPAR[7].Value = jcxx.Rows[0]["F_BRBH"].ToString().Trim();

                            //����
                            mzORAPAR[8].ParameterName = "v_name";// 
                            mzORAPAR[8].OleDbType = OleDbType.VarChar;
                            mzORAPAR[8].Direction = ParameterDirection.Input;
                            mzORAPAR[8].Size = 20;
                            mzORAPAR[8].Value = jcxx.Rows[0]["F_XM"].ToString().Trim();

                            //�������ں�ʱ��
                            mzORAPAR[9].ParameterName = "v_SPM_RECVED_DATE";// 
                            mzORAPAR[9].OleDbType = OleDbType.VarChar;
                            mzORAPAR[9].Direction = ParameterDirection.Input;
                            mzORAPAR[9].Size = 20;
                            mzORAPAR[9].Value = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");

                            //������ں�ʱ��
                            mzORAPAR[10].ParameterName = "v_exam_date_time";// 
                            mzORAPAR[10].OleDbType = OleDbType.VarChar;
                            mzORAPAR[10].Direction = ParameterDirection.Input;
                            mzORAPAR[10].Size = 20;
                            mzORAPAR[10].Value = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");

                            //������Ա
                            mzORAPAR[11].ParameterName = "v_technician";//
                            mzORAPAR[11].OleDbType = OleDbType.VarChar;
                            mzORAPAR[11].Direction = ParameterDirection.Input;
                            mzORAPAR[11].Size = 20;
                            mzORAPAR[11].Value = jcxx.Rows[0]["F_qcys"].ToString().Trim();

                            //���ҽ��
                            mzORAPAR[12].ParameterName = "v_confirm_doct";//
                            mzORAPAR[12].OleDbType = OleDbType.VarChar;
                            mzORAPAR[12].Direction = ParameterDirection.Input;
                            mzORAPAR[12].Size = 10;
                            if (bgzt == "�����")
                                mzORAPAR[12].Value = jcxx.Rows[0]["F_shys"].ToString().Trim();
                            else
                                mzORAPAR[12].Value = "";

                            //������ں�ʱ��
                            mzORAPAR[13].ParameterName = "v_confirm_date_time";//
                            mzORAPAR[13].OleDbType = OleDbType.VarChar;
                            mzORAPAR[13].Direction = ParameterDirection.Input;
                            mzORAPAR[13].Size = 20;
                            if (bgzt == "�����")
                                mzORAPAR[13].Value = DateTime.Parse(jcxx.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                mzORAPAR[13].Value =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //�豸�ͺ�
                            mzORAPAR[14].ParameterName = "v_equipment_no";// 
                            mzORAPAR[14].OleDbType = OleDbType.VarChar;
                            mzORAPAR[14].Direction = ParameterDirection.Input;
                            mzORAPAR[14].Size = 8;
                            mzORAPAR[14].Value = "";

                            //Ѫֵ
                            mzORAPAR[15].ParameterName = "v_hp_value";// 
                            mzORAPAR[15].OleDbType = OleDbType.VarChar;
                            mzORAPAR[15].Direction = ParameterDirection.Input;
                            mzORAPAR[15].Size = 10;
                            mzORAPAR[15].Value = "";

                            //�������ں�ʱ��
                            mzORAPAR[16].ParameterName = "v_report_date_time";// 
                            mzORAPAR[16].OleDbType = OleDbType.VarChar;
                            mzORAPAR[16].Direction = ParameterDirection.Input;
                            mzORAPAR[16].Size = 20;
                            if (bgzt == "�����")
                                mzORAPAR[16].Value =DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                mzORAPAR[16].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //����ҽ��
                            mzORAPAR[17].ParameterName = "v_reporter";// 
                            mzORAPAR[17].OleDbType = OleDbType.VarChar;
                            mzORAPAR[17].Direction = ParameterDirection.Input;
                            mzORAPAR[17].Size = 10;
                            if (bgzt == "�����")
                                mzORAPAR[17].Value = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                            else
                                mzORAPAR[17].Value = "";
                            //ԤԼ���ں�ʱ��
                            mzORAPAR[18].ParameterName = "v_scheduled_date_time";// 
                            mzORAPAR[18].OleDbType = OleDbType.VarChar;
                            mzORAPAR[18].Direction = ParameterDirection.Input;
                            mzORAPAR[18].Size = 20;
                            mzORAPAR[18].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            //�����𣨲���
                            mzORAPAR[19].ParameterName = "v_StudyType";// 
                            mzORAPAR[19].OleDbType = OleDbType.VarChar;
                            mzORAPAR[19].Direction = ParameterDirection.Input;
                            mzORAPAR[19].Size = 10;
                            mzORAPAR[19].Value = "����";

                            //��鲿λ
                            mzORAPAR[20].ParameterName = "v_StudyBodyPart";// 
                            mzORAPAR[20].OleDbType = OleDbType.VarChar;
                            mzORAPAR[20].Direction = ParameterDirection.Input;
                            mzORAPAR[20].Size = 100;
                            mzORAPAR[20].Value = jcxx.Rows[0]["F_BBMC"].ToString().Trim();

                            //��鲿λ����
                            mzORAPAR[21].ParameterName = "v_StudyBodyPartCode";//
                            mzORAPAR[21].OleDbType = OleDbType.VarChar;
                            mzORAPAR[21].Direction = ParameterDirection.Input;
                            mzORAPAR[21].Size = 20;
                            mzORAPAR[21].Value = "";

                            //jpgͼ��·����ʹ��^����
                            mzORAPAR[22].ParameterName = "v_IMGStrings";//
                            mzORAPAR[22].OleDbType = OleDbType.VarChar;
                            mzORAPAR[22].Direction = ParameterDirection.Input;
                            mzORAPAR[22].Size = 500;
                            if (bgzt == "�����")
                                mzORAPAR[22].Value = "";
                            else
                                mzORAPAR[22].Value = "";

                            //������Ժ(�������)
                            mzORAPAR[23].ParameterName = "v_inp_outp_flag";//
                            mzORAPAR[23].OleDbType = OleDbType.VarChar;
                            mzORAPAR[23].Direction = ParameterDirection.Input;
                            mzORAPAR[23].Size = 20;
                            mzORAPAR[23].Value = jcxx.Rows[0]["F_BRLB"].ToString().Trim();
                            /////////////////////////////////////////////
                        
                            string message_ee = "";
                            //odbcbd.Odbc_ExecuteNonQuery(odbcsql_his, "{ CALL his_vs_exam_inteface_outp(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}",ref mzORAPAR, ref message_ee);
                            oledb.ExecuteNonQuery(odbcsql_his, "his_vs_exam_inteface_outp", ref mzORAPAR,CommandType.StoredProcedure, ref message_ee);
                           
                            if (message_ee.Trim() != "")
                            {
             
                                log.WriteMyLog(F_blh + ",��дHIS���棬��HIS�洢����his_vs_exam_inteface_outp�쳣��" + message_ee);
                            }
                            else
                            {
                                if (debug == "1")
                                  log.WriteMyLog(F_blh+",��дhis�������");

                                if(bgzt=="�����")
                                  aa.ExecuteSQL("update t_jcxx set F_HXHIS='2' where f_blh='" + F_blh + "'");
                                //else
                                //  aa.ExecuteSQL("update t_jcxx set F_HXHIS='1' where f_blh='" + F_blh + "'");

                            }
                          
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog(F_blh + ",��дHIS�����쳣��" + ee.Message);
                        }
                    }
                    #endregion
                }

                //�ѵǼǣ���ȡ�ģ��������ڣ����
              
                if (topacs == "1")
                {
                      #region pacs��д


                    ///��д���棨pacs��(סԺ)
                    /// ��ͨ���洢����GETPHOTONO_BL_LYBL��ȡӰ��źͼ��ţ��ٻ�д�м��д״̬��δ��ԭ��
                    ///
                    if (jcxx.Rows[0]["F_BRBH"].ToString().Trim() != "" && jcxx.Rows[0]["F_ZYH"].ToString().Trim() != "")
                    {
                        if (debug == "1")
                            log.WriteMyLog(F_blh+",��дPACS");
                        string odbcsql_pacs = f.ReadString("savetohis", "odbcsql_pacs", "Provider='MSDAORA';data source=HHPACS;user id =BLUSER;password=blpassword;");
                        try
                        {
                             OleDbParameter[] mzORAPAR = new OleDbParameter[5];
                            for (int j = 0; j < mzORAPAR.Length; j++)
                            {
                                mzORAPAR[j] = new OleDbParameter();
                            }
                            //hisid��
                            mzORAPAR[0].ParameterName = "sHISID";
                            mzORAPAR[0].OleDbType = OleDbType.VarChar;
                            mzORAPAR[0].Direction = ParameterDirection.Input;
                            mzORAPAR[0].Size = 16;
                            mzORAPAR[0].Value = jcxx.Rows[0]["F_BRBH"].ToString().Trim();

                            //סԺ��
                            mzORAPAR[1].ParameterName = "sInHospitalNo";//
                            mzORAPAR[1].OleDbType = OleDbType.VarChar;
                            mzORAPAR[1].Direction = ParameterDirection.Input;
                            mzORAPAR[1].Size = 20;
                            mzORAPAR[1].Value = jcxx.Rows[0]["F_ZYH"].ToString().Trim();

                            //�����
                            mzORAPAR[2].ParameterName = "sBL";//
                            mzORAPAR[2].OleDbType = OleDbType.VarChar;
                            mzORAPAR[2].Direction = ParameterDirection.Input;
                            mzORAPAR[2].Size = 20;
                            mzORAPAR[2].Value = jcxx.Rows[0]["F_BLH"].ToString().Trim();

                            //Ӱ���
                            mzORAPAR[3].ParameterName = "sPhotoNo";//
                            mzORAPAR[3].OleDbType = OleDbType.VarChar;
                            mzORAPAR[3].Direction = ParameterDirection.Output;
                            mzORAPAR[3].Size = 20;
                            //����
                            mzORAPAR[4].ParameterName = "iStudyID";// 
                            mzORAPAR[4].OleDbType = OleDbType.Numeric;
                            mzORAPAR[4].Direction = ParameterDirection.Output;
                            mzORAPAR[4].Size = 20;
                            /////////////////////////////////////////////
                            string pacsid = jcxx.Rows[0]["F_iStudyID"].ToString().Trim();
                            string iStudyID = "";
                            string sPhotoNo = "";
                            if (pacsid != "" && pacsid.Contains("^"))
                            {
                                try
                                {
                                    iStudyID = jcxx.Rows[0]["F_iStudyID"].ToString().Trim().Split('^')[0];
                                    sPhotoNo = jcxx.Rows[0]["F_iStudyID"].ToString().Trim().Split('^')[1];
                                }
                                catch
                                {
                                }
                            }
                            
                            if (iStudyID == "" || sPhotoNo=="")
                            {
                                string message_ee = "";
                          
                              oledb.ExecuteNonQuery(odbcsql_pacs, "GETPHOTONO_BL_LYBL", ref mzORAPAR, CommandType.StoredProcedure, ref message_ee);
                                if (message_ee.Trim() != "")
                                {
                                    log.WriteMyLog(F_blh + ",��ȡӰ��źͼ���ʧ�ܣ�ִ��PACS�洢����GETPHOTONO_BL_LYBL�쳣��" + message_ee);
                                  // return;
                                }
                               iStudyID= mzORAPAR[4].Value.ToString().Trim();
                               sPhotoNo = mzORAPAR[3].Value.ToString().Trim();
                                //��¼Ӱ��źͼ���
                               aa.ExecuteSQL("update t_jcxx set  F_iStudyID='" + iStudyID +"^"+sPhotoNo + "' where f_blh='" + F_blh + "'");
                            }
                          
                            if (sPhotoNo.Trim() != "" && iStudyID.Trim() != "")
                            {
                                string pacs_select = @"select * from  R_BLREQUSITION_LYBL  where HISID='" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "' and PHOTONO='" + sPhotoNo + "' and SBL='" + jcxx.Rows[0]["F_blh"].ToString().Trim()
                                    + "' and HISEXAMNO='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'  and STUDYID='" + iStudyID + "' ";

                                string message_ee1 = "";
                                DataTable dt = oledb.DataAdapter(odbcsql_pacs, pacs_select, ref message_ee1);
                                if (message_ee1.Trim() != "")
                                {
                                    log.WriteMyLog(F_blh + ",��ѯPACS�м���쳣��" + message_ee1);
                                    return;
                                }
                                string pacs_sql = "";
                                if (dt.Rows.Count > 0)
                                {
                                    ///�޸��м������  
                                    ///
                                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "�����")
                                    {
                                        pacs_sql = @"update R_BLREQUSITION_LYBL  set REPORTDOCTOR='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',status='" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "',bz='" + jcxx.Rows[0]["F_WFBGYY"].ToString().Trim() + "'  where HISID='" + jcxx.Rows[0]["F_BRBH"].ToString().Trim()
                                            + "' and PHOTONO='" + sPhotoNo + "' and SBL='" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "' and HISEXAMNO='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'  and STUDYID='" + iStudyID + "'";
                                    }
                                    else
                                    {

                                       
                                        pacs_sql = @"update R_BLREQUSITION_LYBL  set CHECKDOCTOR='" + jcxx.Rows[0]["F_QCYS"].ToString().Trim() + "',REPORTDOCTOR='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim()
                                            + "',METHOD='" + "" + "',EYESEE='" + jcxx.Rows[0]["F_jxsj"].ToString().Trim() + "',RESULT='" + jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_TSJC"].ToString().Trim()
                                            + "',REPORTDATE=to_date('" + DateTime.Parse(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),CHIEFDOCTOR='" + jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',status='" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "',bz=''   where HISID='" + jcxx.Rows[0]["F_BRBH"].ToString().Trim()
                                            + "' and PHOTONO='" + sPhotoNo + "' and SBL='" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "' and HISEXAMNO='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'  and STUDYID='" + iStudyID + "'";
                                    }
                                }
                                else
                                {
                                    ///�м�����������

                                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "�����")
                                    {
                                        pacs_sql = @"insert into R_BLREQUSITION_LYBL(HISID,PHOTONO,NAME,SEX,AGE,AGEDW,RACE,PLACEOFBIRTH,ADDRESS,LODGEHOSPITAL,LODGESECTION,LODGEDOCTOR,LODGEDATE,CLINICNO,INHOSPITALNO,BEDNO,DIAGNOSIS,CHECKDATE,CHECKDOCTOR,REPORTDOCTOR,METHOD,EYESEE,RESULT,CHIEFDOCTOR,CLASSNAME,PARTOFCHECK,PATNAME,SBL,STUDYID,HISEXAMNO,CLIISINPAT,status,bz) values('"
                                              + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "','" + sPhotoNo + "','" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_xb"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_AGE"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_MZ"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_lxxx"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_sjdw"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjks"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjys"].ToString().Trim()
                                             + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + "','" + jcxx.Rows[0]["F_zyh"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_ch"].ToString().Trim() + "','" + jcxx.Rows[0]["F_lczd"].ToString().Trim() + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_qcys"].ToString().Trim() + "','','','','','','����','" + jcxx.Rows[0]["F_bbmc"].ToString().Trim() + "','" + jcxx.Rows[0]["F_xm"].ToString().Trim() + "','" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "','" + iStudyID + "','" + jcxx.Rows[0]["F_SQXH"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_brlb"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "','" + jcxx.Rows[0]["F_WFBgYY"].ToString().Trim() + "')";
                                    }
                                    else
                                    {
                                      
                                        pacs_sql = @"insert into R_BLREQUSITION_LYBL(HISID,PHOTONO,NAME,SEX,AGE,AGEDW,RACE,PLACEOFBIRTH,ADDRESS,LODGEHOSPITAL,LODGESECTION,LODGEDOCTOR,LODGEDATE,CLINICNO,INHOSPITALNO,BEDNO,DIAGNOSIS,CHECKDATE,CHECKDOCTOR,REPORTDOCTOR,METHOD,EYESEE,RESULT,REPORTDATE,CHIEFDOCTOR,CLASSNAME,PARTOFCHECK,PATNAME,SBL,STUDYID,HISEXAMNO,CLIISINPAT,status,bz) values('" 
                                            + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "','" + sPhotoNo + "','" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_xb"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_AGE"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_MZ"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_lxxx"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_sjdw"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjks"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjys"].ToString().Trim()
                                                   + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-MM-dd hh24:mi:ss'),'" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + "','" + jcxx.Rows[0]["F_zyh"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_ch"].ToString().Trim() + "','" + jcxx.Rows[0]["F_lczd"].ToString().Trim() + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_qcys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_bgys"].ToString().Trim() + "','" + ""
                                                   + "','" + jcxx.Rows[0]["F_jxsj"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_tsjc"].ToString().Trim()
                                                   + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_shys"].ToString().Trim() + "','����','" + jcxx.Rows[0]["F_bbmc"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_xm"].ToString().Trim() + "','" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "','" + iStudyID + "','" + jcxx.Rows[0]["F_SQXH"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_brlb"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "','')";
                                    }
                                }


                                if (pacs_sql.Trim() != "")
                                {
                                    string message_ee12 = "";
                                    if (debug == "1")
                                    {
                                        log.WriteMyLog(pacs_sql);
                                    }

                                    oledb.ExecuteNonQuery(odbcsql_pacs, pacs_sql, ref message_ee12);
                                    if (message_ee12.Trim() != "")
                                    {
                                        log.WriteMyLog(F_blh + ",��дPACS�м���쳣��" + message_ee12);
                                    }
                                    else
                                    {
                                        if (bgzt == "�����")
                                        {
                                            aa.ExecuteSQL("update t_jcxx set F_HXPACS='2' where f_blh='" + F_blh + "'");

                                            if (scdicom == "1")
                                            {

                                                if (debug == "1")
                                                    log.WriteMyLog("��ʼ��dicom");
                                                psdicom dicom = new psdicom();
                                                if (dicom.psdicomfile(F_blh))
                                                {
                                                    if (debug == "1")
                                                        log.WriteMyLog("��dicom�ɹ�");
                                                    aa.ExecuteSQL("update t_jcxx set F_HXPACS='3' where f_blh='" + F_blh + "'");

                                                }
                                                else
                                                {
                                                    log.WriteMyLog("��dicomʧ��");
                                                }
                                            }
                                     
                                        }
                                        if (debug == "1")
                                       log.WriteMyLog(F_blh+",��дpacs���");
                                      
                                    }
                                }
                                else
                                {
                                    log.WriteMyLog(F_blh + ",��дPACS�м�����ִ�����Ϊ��");
                                }

                            }
                            else
                            {
                                log.WriteMyLog(F_blh + ",��ȡpacsӰ��Ż����ʧ��");
                            }

                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog(F_blh + ",��дpacs�쳣��" + ee.Message);
                        }
                    }
                    else
                        log.WriteMyLog(F_blh + ",��дpacs:brbh��zyhΪ�� ������");
                    #endregion 
                }
            
        }





        public void pathtohis20160125(string F_blh, string msg, string debug)
        {

            string czy = f.ReadString("yh", "yhmc", "");
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

            if (bgzt == "����Ƭ" || bgzt == "�Ѱ���")
                return;


            if (jcxx.Rows.Count > 0 && jcxx.Rows[0]["f_sqxh"].ToString().Trim() != "")
            {
                OleDbDB oledb = new OleDbDB();
                string odbcsql_his = f.ReadString("savetohis", "odbcsql_his", "Provider='MSDAORA';data source=DBSERVER;user id =pacs;password=pacs;");
                string tohis = f.ReadString("savetohis", "tohis", "1");
                string topacs = f.ReadString("savetohis", "topacs", "1");
                string fyqr = f.ReadString("savetohis", "fyqr", "0");
                debug = f.ReadString("savetohis", "debug", "0");

                if (tohis == "1")
                {

                    #region ��д���棨his��
                    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "")
                    {

                        if (debug == "1")
                            log.WriteMyLog(F_blh + ",��HIS����");
                        try
                        {
                            string yyx = jcxx.Rows[0]["F_YYX"].ToString().Trim();
                            if (yyx.Contains("����"))
                                yyx = "1";
                            else
                                yyx = "0";
                            string blzd = jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_TSJC"].ToString().Trim();
                            string rysj = jcxx.Rows[0]["F_RYSJ"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_JXSJ"].ToString().Trim();
                            if (bgzt == "�����")
                            {
                                //DataTable TJ_bljc = new DataTable();
                                //TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + F_blh + "'", "blxx");
                                //if (TJ_bljc.Rows.Count > 0)
                                //{
                                //    if (jcxx.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                                //    {
                                //        rysj = rysj.Trim() + "�걾�����:" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "  " + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + "\r\n" + "��Ӧ��ϸ���仯:" + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH2"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_CH3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH4"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_CH5"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_CH6"].ToString().Trim() + "\r\n";

                                //        rysj = rysj + "\r\n" + "΢������Ŀ:" + "\r\n" + TJ_bljc.Rows[0]["F_JGH_HP1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_HP2"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_HP3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_JGH_HP4"].ToString().Trim() + "\r\n";
                                //        rysj = rysj + TJ_bljc.Rows[0]["F_JGH_HP5"].ToString().Trim() + "\r\n";

                                //        rysj = rysj + "\r\n" + "��Ƥϸ�����:" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_LZXB"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XXB"].ToString().Trim() + "\r\n";
                                //        ////////////////////////////////////
                                //        blzd = TJ_bljc.Rows[0]["F_TBS_ZD"].ToString().Trim() ;

                                //        if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                //            blzd = blzd + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                //    }
                                //}
                            }

                            //////////////////////////////////////////////////////////////////

                            OleDbParameter[] mzORAPAR = new OleDbParameter[24];
                            for (int j = 0; j < mzORAPAR.Length; j++)
                            {
                                mzORAPAR[j] = new OleDbParameter();
                            }
                            //�������뵥id
                            mzORAPAR[0].ParameterName = "v_exam_no";
                            mzORAPAR[0].OleDbType = OleDbType.VarChar;
                            mzORAPAR[0].Direction = ParameterDirection.Input;
                            mzORAPAR[0].Size = 20;
                            mzORAPAR[0].Value = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

                            //������
                            mzORAPAR[1].ParameterName = "v_exam_para";//
                            mzORAPAR[1].OleDbType = OleDbType.VarChar;
                            mzORAPAR[1].Direction = ParameterDirection.Input;
                            mzORAPAR[1].Size = 1000;
                            mzORAPAR[1].Value = "";

                            //�������
                            mzORAPAR[2].ParameterName = "v_description";//
                            mzORAPAR[2].OleDbType = OleDbType.VarChar;
                            mzORAPAR[2].Direction = ParameterDirection.Input;
                            mzORAPAR[2].Size = 2000;
                            if (bgzt == "�����")
                                mzORAPAR[2].Value = rysj;
                            else
                                mzORAPAR[2].Value = "";

                            //ӡ�󣨽��ۣ�
                            mzORAPAR[3].ParameterName = "v_impression";//
                            mzORAPAR[3].OleDbType = OleDbType.VarChar;
                            mzORAPAR[3].Direction = ParameterDirection.Input;
                            mzORAPAR[3].Size = 2000;
                            if (bgzt == "�����")
                                mzORAPAR[3].Value = blzd;
                            else
                                mzORAPAR[3].Value = "";

                            //����
                            mzORAPAR[4].ParameterName = "v_recommendation";// 
                            mzORAPAR[4].OleDbType = OleDbType.VarChar;
                            mzORAPAR[4].Direction = ParameterDirection.Input;
                            mzORAPAR[4].Size = 2000;
                            if (bgzt == "�����")
                                mzORAPAR[4].Value = jcxx.Rows[0]["F_BZ"].ToString().Trim();
                            else
                                mzORAPAR[4].Value = "";

                            //�Ƿ����ԣ�0���ԣ�1���ԣ�
                            mzORAPAR[5].ParameterName = "v_lsabnormal";// 
                            mzORAPAR[5].OleDbType = OleDbType.VarChar;
                            mzORAPAR[5].Direction = ParameterDirection.Input;
                            mzORAPAR[5].Size = 1;
                            if (bgzt == "�����")
                                mzORAPAR[5].Value = yyx;
                            else
                                mzORAPAR[5].Value = "";

                            //��ע������״̬��
                            mzORAPAR[6].ParameterName = "v_memo";// 
                            mzORAPAR[6].OleDbType = OleDbType.VarChar;
                            mzORAPAR[6].Direction = ParameterDirection.Input;
                            mzORAPAR[6].Size = 2000;
                            mzORAPAR[6].Value = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

                            //���˱�ʾ��
                            mzORAPAR[7].ParameterName = "v_patient_id";// 
                            mzORAPAR[7].OleDbType = OleDbType.VarChar;
                            mzORAPAR[7].Direction = ParameterDirection.Input;
                            mzORAPAR[7].Size = 20;
                            mzORAPAR[7].Value = jcxx.Rows[0]["F_BRBH"].ToString().Trim();

                            //����
                            mzORAPAR[8].ParameterName = "v_name";// 
                            mzORAPAR[8].OleDbType = OleDbType.VarChar;
                            mzORAPAR[8].Direction = ParameterDirection.Input;
                            mzORAPAR[8].Size = 20;
                            mzORAPAR[8].Value = jcxx.Rows[0]["F_XM"].ToString().Trim();

                            //�������ں�ʱ��
                            mzORAPAR[9].ParameterName = "v_SPM_RECVED_DATE";// 
                            mzORAPAR[9].OleDbType = OleDbType.VarChar;
                            mzORAPAR[9].Direction = ParameterDirection.Input;
                            mzORAPAR[9].Size = 20;
                            mzORAPAR[9].Value = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");

                            //������ں�ʱ��
                            mzORAPAR[10].ParameterName = "v_exam_date_time";// 
                            mzORAPAR[10].OleDbType = OleDbType.VarChar;
                            mzORAPAR[10].Direction = ParameterDirection.Input;
                            mzORAPAR[10].Size = 20;
                            mzORAPAR[10].Value = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");

                            //������Ա
                            mzORAPAR[11].ParameterName = "v_technician";//
                            mzORAPAR[11].OleDbType = OleDbType.VarChar;
                            mzORAPAR[11].Direction = ParameterDirection.Input;
                            mzORAPAR[11].Size = 20;
                            mzORAPAR[11].Value = jcxx.Rows[0]["F_qcys"].ToString().Trim();

                            //���ҽ��
                            mzORAPAR[12].ParameterName = "v_confirm_doct";//
                            mzORAPAR[12].OleDbType = OleDbType.VarChar;
                            mzORAPAR[12].Direction = ParameterDirection.Input;
                            mzORAPAR[12].Size = 10;
                            if (bgzt == "�����")
                                mzORAPAR[12].Value = jcxx.Rows[0]["F_shys"].ToString().Trim();
                            else
                                mzORAPAR[12].Value = "";

                            //������ں�ʱ��
                            mzORAPAR[13].ParameterName = "v_confirm_date_time";//
                            mzORAPAR[13].OleDbType = OleDbType.VarChar;
                            mzORAPAR[13].Direction = ParameterDirection.Input;
                            mzORAPAR[13].Size = 20;
                            if (bgzt == "�����")
                                mzORAPAR[13].Value = DateTime.Parse(jcxx.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                mzORAPAR[13].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //�豸�ͺ�
                            mzORAPAR[14].ParameterName = "v_equipment_no";// 
                            mzORAPAR[14].OleDbType = OleDbType.VarChar;
                            mzORAPAR[14].Direction = ParameterDirection.Input;
                            mzORAPAR[14].Size = 8;
                            mzORAPAR[14].Value = "";

                            //Ѫֵ
                            mzORAPAR[15].ParameterName = "v_hp_value";// 
                            mzORAPAR[15].OleDbType = OleDbType.VarChar;
                            mzORAPAR[15].Direction = ParameterDirection.Input;
                            mzORAPAR[15].Size = 10;
                            mzORAPAR[15].Value = "";

                            //�������ں�ʱ��
                            mzORAPAR[16].ParameterName = "v_report_date_time";// 
                            mzORAPAR[16].OleDbType = OleDbType.VarChar;
                            mzORAPAR[16].Direction = ParameterDirection.Input;
                            mzORAPAR[16].Size = 20;
                            if (bgzt == "�����")
                                mzORAPAR[16].Value = DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                mzORAPAR[16].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            //����ҽ��
                            mzORAPAR[17].ParameterName = "v_reporter";// 
                            mzORAPAR[17].OleDbType = OleDbType.VarChar;
                            mzORAPAR[17].Direction = ParameterDirection.Input;
                            mzORAPAR[17].Size = 10;
                            if (bgzt == "�����")
                                mzORAPAR[17].Value = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                            else
                                mzORAPAR[17].Value = "";
                            //ԤԼ���ں�ʱ��
                            mzORAPAR[18].ParameterName = "v_scheduled_date_time";// 
                            mzORAPAR[18].OleDbType = OleDbType.VarChar;
                            mzORAPAR[18].Direction = ParameterDirection.Input;
                            mzORAPAR[18].Size = 20;
                            mzORAPAR[18].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            //�����𣨲���
                            mzORAPAR[19].ParameterName = "v_StudyType";// 
                            mzORAPAR[19].OleDbType = OleDbType.VarChar;
                            mzORAPAR[19].Direction = ParameterDirection.Input;
                            mzORAPAR[19].Size = 10;
                            mzORAPAR[19].Value = "����";

                            //��鲿λ
                            mzORAPAR[20].ParameterName = "v_StudyBodyPart";// 
                            mzORAPAR[20].OleDbType = OleDbType.VarChar;
                            mzORAPAR[20].Direction = ParameterDirection.Input;
                            mzORAPAR[20].Size = 100;
                            mzORAPAR[20].Value = jcxx.Rows[0]["F_BBMC"].ToString().Trim();

                            //��鲿λ����
                            mzORAPAR[21].ParameterName = "v_StudyBodyPartCode";//
                            mzORAPAR[21].OleDbType = OleDbType.VarChar;
                            mzORAPAR[21].Direction = ParameterDirection.Input;
                            mzORAPAR[21].Size = 20;
                            mzORAPAR[21].Value = "";

                            //jpgͼ��·����ʹ��^����
                            mzORAPAR[22].ParameterName = "v_IMGStrings";//
                            mzORAPAR[22].OleDbType = OleDbType.VarChar;
                            mzORAPAR[22].Direction = ParameterDirection.Input;
                            mzORAPAR[22].Size = 500;
                            if (bgzt == "�����")
                                mzORAPAR[22].Value = "";
                            else
                                mzORAPAR[22].Value = "";

                            //������Ժ(�������)
                            mzORAPAR[23].ParameterName = "v_inp_outp_flag";//
                            mzORAPAR[23].OleDbType = OleDbType.VarChar;
                            mzORAPAR[23].Direction = ParameterDirection.Input;
                            mzORAPAR[23].Size = 20;
                            mzORAPAR[23].Value = jcxx.Rows[0]["F_BRLB"].ToString().Trim();
                            /////////////////////////////////////////////

                            string message_ee = "";
                            //odbcbd.Odbc_ExecuteNonQuery(odbcsql_his, "{ CALL his_vs_exam_inteface_outp(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}",ref mzORAPAR, ref message_ee);
                            oledb.ExecuteNonQuery(odbcsql_his, "his_vs_exam_inteface_outp", ref mzORAPAR, CommandType.StoredProcedure, ref message_ee);

                            if (message_ee.Trim() != "")
                            {

                                log.WriteMyLog(F_blh + ",��дHIS״̬�򱨸棬��HIS�洢����his_vs_exam_inteface_outp�쳣��" + message_ee);
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog(F_blh + ",��дhis�������");





                                if (bgzt == "�����")
                                    aa.ExecuteSQL("update t_jcxx set F_HXHIS='2' where f_blh='" + F_blh + "'");



                                #region HIS ��д״̬ȷ�Ϸ���


                                if (jcxx.Rows[0]["F_HXJFBZ"].ToString().Trim() != "1" && !jcxx.Rows[0]["F_BRLB"].ToString().Trim().Contains("���"))
                                {

                                    if (fyqr == "1")
                                    {

                                        try
                                        {
                                            OleDbParameter[] ops = new OleDbParameter[3];
                                            for (int j = 0; j < ops.Length; j++)
                                            {
                                                ops[j] = new OleDbParameter();
                                            }
                                            ops[0].ParameterName = "v_test_no";
                                            ops[0].OleDbType = OleDbType.VarChar;
                                            ops[0].Direction = ParameterDirection.Input;
                                            ops[0].Size = 20;
                                            ops[0].Value = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

                                            ops[1].ParameterName = "v_operator";//
                                            ops[1].OleDbType = OleDbType.VarChar;
                                            ops[1].Direction = ParameterDirection.Input;
                                            ops[1].Size = 10;
                                            ops[1].Value = czy.Trim();

                                            ops[2].ParameterName = "v_flag";//
                                            ops[2].OleDbType = OleDbType.VarChar;
                                            ops[2].Direction = ParameterDirection.Input;
                                            ops[2].Size = 10;
                                            ops[2].Value = "0";
                                            //��д�Ǽ�״̬
                                            message_ee = "";
                                            if (debug == "1")
                                                log.WriteMyLog(F_blh + ",��д����״̬");
                                            oledb.ExecuteNonQuery(odbcsql_his, "his_vs_exam_inp_fee", ref ops, CommandType.StoredProcedure, ref message_ee);

                                            if (message_ee.Trim() != "")
                                            {

                                                log.WriteMyLog(F_blh + ",��д�Ǽ�״̬����HIS�洢����his_vs_exam_inp_fee�쳣��" + message_ee);
                                            }
                                            else
                                            {
                                                if (debug == "1")
                                                    log.WriteMyLog(F_blh + ",��д�Ǽ�״̬�ɹ�");
                                                //aa.ExecuteSQL("update t_jcxx set F_HXJFBZ='1' where f_blh='" + F_blh + "'");
                                            }


                                            if (debug == "1")
                                                log.WriteMyLog(F_blh + ",��д�Ʒ�״̬");
                                            //��д�շ�״̬
                                            OleDbParameter[] ops2 = new OleDbParameter[3];
                                            for (int j = 0; j < ops2.Length; j++)
                                            {
                                                ops2[j] = new OleDbParameter();
                                            }
                                            ops2[0].ParameterName = "v_test_no";
                                            ops2[0].OleDbType = OleDbType.VarChar;
                                            ops2[0].Direction = ParameterDirection.Input;
                                            ops2[0].Size = 20;
                                            ops2[0].Value = jcxx.Rows[0]["F_SQXH"].ToString().Trim();

                                            ops2[1].ParameterName = "v_operator";//
                                            ops2[1].OleDbType = OleDbType.VarChar;
                                            ops2[1].Direction = ParameterDirection.Input;
                                            ops2[1].Size = 10;
                                            ops2[1].Value = czy.Trim();

                                            ops2[2].ParameterName = "v_flag";//
                                            ops2[2].OleDbType = OleDbType.VarChar;
                                            ops2[2].Direction = ParameterDirection.Input;
                                            ops2[2].Size = 10;
                                            ops2[2].Value = "1";
                                            string message_ee2 = "";
                                            //   OleDbbd.Odbc_ExecuteNonQuery(odbcsql_his, "{ CALL his_vs_exam_inp_fee(?,?,?)}", ref ops2, ref message_ee2);
                                            oledb.ExecuteNonQuery(odbcsql_his, "his_vs_exam_inp_fee", ref ops2, CommandType.StoredProcedure, ref message_ee2);
                                            if (message_ee2.Trim() != "")
                                            {
                                                log.WriteMyLog(F_blh + ",��д�շ�״̬����HIS�洢����his_vs_exam_inp_fee�쳣��" + message_ee2);
                                            }
                                            else
                                            {
                                                if (debug == "1")
                                                    log.WriteMyLog(F_blh + ",��д�Ʒ�״̬�ɹ�");
                                                aa.ExecuteSQL("update t_jcxx set F_HXJFBZ='1' where f_blh='" + F_blh + "'");
                                            }
                                        }
                                        catch (Exception err)
                                        {
                                            log.WriteMyLog(F_blh + ",��д�Ʒ�״̬�쳣��" + err.Message);
                                        }
                                    }


                                }
                                #endregion

                                //else
                                //  aa.ExecuteSQL("update t_jcxx set F_HXHIS='1' where f_blh='" + F_blh + "'");

                            }

                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog(F_blh + ",��дHIS�����쳣��" + ee.Message);
                        }
                    }
                    #endregion
                }

                //�ѵǼǣ����ϻ����������ڣ����

                if (topacs == "1")
                {
                    #region pacs��д


                    ///��д���棨pacs��(סԺ)
                    /// ��ͨ���洢����GETPHOTONO_BL_LYBL��ȡӰ��źͼ��ţ��ٻ�д�м��д״̬��δ��ԭ��
                    ///
                    if (jcxx.Rows[0]["F_ZYH"].ToString().Trim() != "" && jcxx.Rows[0]["F_YZID"].ToString().Trim() != "")
                    {
                        if (debug == "1")
                            log.WriteMyLog(F_blh + ",��дPACS");
                        string odbcsql_pacs = f.ReadString("savetohis", "odbcsql_pacs", "Provider='MSDAORA';data source=HHPACS;user id =BLUSER;password=blpassword;");
                        try
                        {
                            OleDbParameter[] mzORAPAR = new OleDbParameter[5];
                            for (int j = 0; j < mzORAPAR.Length; j++)
                            {
                                mzORAPAR[j] = new OleDbParameter();
                            }
                            //hisid��
                            mzORAPAR[0].ParameterName = "sHISID";
                            mzORAPAR[0].OleDbType = OleDbType.VarChar;
                            mzORAPAR[0].Direction = ParameterDirection.Input;
                            mzORAPAR[0].Size = 16;
                            mzORAPAR[0].Value = jcxx.Rows[0]["F_YZID"].ToString().Trim();

                            //סԺ��
                            mzORAPAR[1].ParameterName = "sInHospitalNo";//
                            mzORAPAR[1].OleDbType = OleDbType.VarChar;
                            mzORAPAR[1].Direction = ParameterDirection.Input;
                            mzORAPAR[1].Size = 20;
                            mzORAPAR[1].Value = jcxx.Rows[0]["F_ZYH"].ToString().Trim();

                            //�����
                            mzORAPAR[2].ParameterName = "sBL";//
                            mzORAPAR[2].OleDbType = OleDbType.VarChar;
                            mzORAPAR[2].Direction = ParameterDirection.Input;
                            mzORAPAR[2].Size = 20;
                            mzORAPAR[2].Value = jcxx.Rows[0]["F_BLH"].ToString().Trim();

                            //Ӱ���
                            mzORAPAR[3].ParameterName = "sPhotoNo";//
                            mzORAPAR[3].OleDbType = OleDbType.VarChar;
                            mzORAPAR[3].Direction = ParameterDirection.Output;
                            mzORAPAR[3].Size = 20;
                            //����
                            mzORAPAR[4].ParameterName = "iStudyID";// 
                            mzORAPAR[4].OleDbType = OleDbType.Numeric;
                            mzORAPAR[4].Direction = ParameterDirection.Output;
                            mzORAPAR[4].Size = 20;
                            /////////////////////////////////////////////
                            string pacsid = jcxx.Rows[0]["F_iStudyID"].ToString().Trim();
                            string iStudyID = "";
                            string sPhotoNo = "";
                            if (pacsid != "" && pacsid.Contains("^"))
                            {
                                iStudyID = jcxx.Rows[0]["F_iStudyID"].ToString().Trim().Split('^')[0];
                                sPhotoNo = jcxx.Rows[0]["F_iStudyID"].ToString().Trim().Split('^')[1];
                            }

                            if (iStudyID == "" || sPhotoNo == "")
                            {
                                string message_ee = "";

                                oledb.ExecuteNonQuery(odbcsql_pacs, "GETPHOTONO_BL_LYBL", ref mzORAPAR, CommandType.StoredProcedure, ref message_ee);
                                if (message_ee.Trim() != "")
                                {
                                    log.WriteMyLog(F_blh + ",��ȡӰ��źͼ���ʧ�ܣ�ִ��PACS�洢����GETPHOTONO_BL_LYBL�쳣��" + message_ee);
                                    // return;
                                }
                                iStudyID = mzORAPAR[4].Value.ToString().Trim();
                                sPhotoNo = mzORAPAR[3].Value.ToString().Trim();
                                //��¼Ӱ��źͼ���
                                aa.ExecuteSQL("update t_jcxx set  F_iStudyID='" + iStudyID + "^" + sPhotoNo + "' where f_blh='" + F_blh + "'");
                            }

                            if (sPhotoNo.Trim() != "" && iStudyID.Trim() != "")
                            {
                                string pacs_select = @"select * from  R_BLREQUSITION_LYBL  where HISID='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "' and PHOTONO='" + sPhotoNo + "' and SBL='" + jcxx.Rows[0]["F_blh"].ToString().Trim()
                                    + "' and HISEXAMNO='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'  and STUDYID='" + iStudyID + "' ";

                                string message_ee1 = "";
                                DataTable dt = oledb.DataAdapter(odbcsql_pacs, pacs_select, ref message_ee1);
                                if (message_ee1.Trim() != "")
                                {
                                    log.WriteMyLog(F_blh + ",��ѯPACS�м���쳣��" + message_ee1);
                                    return;
                                }
                                string pacs_sql = "";
                                if (dt.Rows.Count > 0)
                                {
                                    ///�޸��м������  
                                    ///
                                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "�����")
                                    {
                                        pacs_sql = @"update R_BLREQUSITION_LYBL  set REPORTDOCTOR='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',status='" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "',bz='" + jcxx.Rows[0]["F_WFBGYY"].ToString().Trim() + "'  where HISID='" + jcxx.Rows[0]["F_YZID"].ToString().Trim()
                                            + "' and PHOTONO='" + sPhotoNo + "' and SBL='" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "' and HISEXAMNO='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'  and STUDYID='" + iStudyID + "'";
                                    }
                                    else
                                    {
                                        pacs_sql = @"update R_BLREQUSITION_LYBL  set CHECKDOCTOR='" + jcxx.Rows[0]["F_QCYS"].ToString().Trim() + "',REPORTDOCTOR='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim()
                                            + "',METHOD='" + jcxx.Rows[0]["F_rysj"].ToString().Trim() + "',EYESEE='" + jcxx.Rows[0]["F_jxsj"].ToString().Trim() + "',RESULT='" + jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_TSJC"].ToString().Trim()
                                            + "',REPORTDATE=to_date('" + DateTime.Parse(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),CHIEFDOCTOR='" + jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',status='" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "',bz=''   where HISID='" + jcxx.Rows[0]["F_YZID"].ToString().Trim()
                                            + "' and PHOTONO='" + sPhotoNo + "' and SBL='" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "' and HISEXAMNO='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'  and STUDYID='" + iStudyID + "'";
                                    }
                                }
                                else
                                {
                                    ///�м�����������

                                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "�����")
                                    {
                                        pacs_sql = @"insert into R_BLREQUSITION_LYBL(HISID,PHOTONO,NAME,SEX,AGE,AGEDW,RACE,PLACEOFBIRTH,ADDRESS,LODGEHOSPITAL,LODGESECTION,LODGEDOCTOR,LODGEDATE,CLINICNO,INHOSPITALNO,BEDNO,DIAGNOSIS,CHECKDATE,CHECKDOCTOR,REPORTDOCTOR,METHOD,EYESEE,RESULT,CHIEFDOCTOR,CLASSNAME,PARTOFCHECK,PATNAME,SBL,STUDYID,HISEXAMNO,CLIISINPAT,status,bz) values('"
                                              + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "','" + sPhotoNo + "','" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_xb"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_AGE"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_MZ"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_lxxx"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_sjdw"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjks"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjys"].ToString().Trim()
                                             + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + "','" + jcxx.Rows[0]["F_zyh"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_ch"].ToString().Trim() + "','" + jcxx.Rows[0]["F_lczd"].ToString().Trim() + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_qcys"].ToString().Trim() + "','','','','','','����','" + jcxx.Rows[0]["F_bbmc"].ToString().Trim() + "','" + jcxx.Rows[0]["F_xm"].ToString().Trim() + "','" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "','" + iStudyID + "','" + jcxx.Rows[0]["F_SQXH"].ToString().Trim()
                                             + "','" + jcxx.Rows[0]["F_brlb"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "','" + jcxx.Rows[0]["F_WFBgYY"].ToString().Trim() + "')";
                                    }
                                    else
                                    {
                                        pacs_sql = @"insert into R_BLREQUSITION_LYBL(HISID,PHOTONO,NAME,SEX,AGE,AGEDW,RACE,PLACEOFBIRTH,ADDRESS,LODGEHOSPITAL,LODGESECTION,LODGEDOCTOR,LODGEDATE,CLINICNO,INHOSPITALNO,BEDNO,DIAGNOSIS,CHECKDATE,CHECKDOCTOR,REPORTDOCTOR,METHOD,EYESEE,RESULT,REPORTDATE,CHIEFDOCTOR,CLASSNAME,PARTOFCHECK,PATNAME,SBL,STUDYID,HISEXAMNO,CLIISINPAT,status,bz) values('"
                                            + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "','" + sPhotoNo + "','" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_xb"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_AGE"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_MZ"].ToString().Trim() + "','','" + jcxx.Rows[0]["F_lxxx"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_sjdw"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjks"].ToString().Trim() + "','" + jcxx.Rows[0]["F_sjys"].ToString().Trim()
                                                   + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + "','" + jcxx.Rows[0]["F_zyh"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_ch"].ToString().Trim() + "','" + jcxx.Rows[0]["F_lczd"].ToString().Trim() + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_qcys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_bgys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_rysj"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_jxsj"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_tsjc"].ToString().Trim()
                                                   + "',to_date('" + DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + jcxx.Rows[0]["F_shys"].ToString().Trim() + "','����','" + jcxx.Rows[0]["F_bbmc"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_xm"].ToString().Trim() + "','" + jcxx.Rows[0]["F_blh"].ToString().Trim() + "','" + iStudyID + "','" + jcxx.Rows[0]["F_SQXH"].ToString().Trim()
                                                   + "','" + jcxx.Rows[0]["F_brlb"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BGZT"].ToString().Trim() + "','')";
                                    }
                                }


                                if (pacs_sql.Trim() != "")
                                {
                                    string message_ee12 = "";
                                    if (debug == "1")
                                    {
                                        log.WriteMyLog(pacs_sql);
                                    }

                                    oledb.ExecuteNonQuery(odbcsql_pacs, pacs_sql, ref message_ee12);
                                    if (message_ee12.Trim() != "")
                                    {
                                        log.WriteMyLog(F_blh + ",��дPACS�м���쳣��" + message_ee12);
                                    }
                                    else
                                    {
                                        if (debug == "1")
                                            log.WriteMyLog(F_blh + ",��дpacs���");
                                        if (bgzt == "�����")
                                            aa.ExecuteSQL("update t_jcxx set F_HXPACS='2' where f_blh='" + F_blh + "'");
                                    }
                                }
                                else
                                {
                                    log.WriteMyLog(F_blh + ",��дPACS�м�����ִ�����Ϊ��");
                                }

                            }
                            else
                            {
                                log.WriteMyLog(F_blh + ",��ȡpacsӰ��Ż����ʧ��");
                            }

                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog(F_blh + ",��дpacs�쳣��" + ee.Message);
                        }
                    }
                    #endregion
                }
            }
        }

    }

}
