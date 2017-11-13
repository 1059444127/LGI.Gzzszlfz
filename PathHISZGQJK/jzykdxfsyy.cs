
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Data.OleDb;
using System.Xml;
using System.Diagnostics;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
   /// <summary>
   /// ����ҽ�ƴ�ѧ������һҽԺ
   /// </summary>
    class jzykdxfsyy
    {
       private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
       //string odbc2his = "";


        public void pathtohis(string blh, string bglx, string bgxh,string czlb,string dz,string debug)
        {
            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            string ksdm = f.ReadString("savetohis", "KSDM", "2030000").Trim();

            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            if (bljc == null)
            {
                log.WriteMyLog("���ݿ������쳣");
                return;
            }
            if (bljc.Rows.Count <= 0)
            {
                log.WriteMyLog("δ��ѯ���˱���" + blh);
                return;
            }

            string ptjk = ZgqClass.GetSz("zgqjk", "ptjk", "1").Replace("\0", "").Trim();
              string hisjk = ZgqClass.GetSz("zgqjk", "hisjk", "0").Replace("\0", "").Trim();
            string hczt = ZgqClass.GetSz("savetohis", "hczt", "0").Replace("\0", "").Trim();
            string hcbg = ZgqClass.GetSz("savetohis", "hcbg", "0").Replace("\0", "").Trim();
            string topacs = ZgqClass.GetSz("savetohis", "topacs", "0").Replace("\0", "").Trim();
           
           string brlb = bljc.Rows[0]["f_brlb"].ToString().Trim();
           string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
           if (dz == "qxsh" && bglx=="cg")
               bgzt = "ȡ�����";

            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim();
            
            if(sqxh=="")
            {
                log.WriteMyLog("�����뵥�Ų�����");
                return;
            }

            string errMsg = "";
            if (bljc.Rows[0]["F_BRLB"].ToString().Trim() == "���")
            {

                string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");

                #region  ���
                //ִ�д洢���̣���д�Ǽ�״̬
                string tjodbc = ZgqClass.GetSz("savetohis", "tjodbc", "Data Source=10.27.1.35;Initial Catalog=zonekingnet;User Id=sa;Password=zoneking;").Replace("\0", "").Trim();
                SqlDB db_tj = new SqlDB();
                #region  �ش����Ǽ�״̬
                if (bljc.Rows[0]["F_TJXTBJ"].ToString().Trim() != "1")
                {
                    try
                    {
                        SqlParameter[] sqlPt = new SqlParameter[2];
                        for (int j = 0; j < sqlPt.Length; j++)
                        {
                            sqlPt[j] = new SqlParameter();
                        }
                        //���뵥ID
                        sqlPt[0].ParameterName = "@Exam_No";
                        sqlPt[0].SqlDbType = SqlDbType.VarChar;
                        sqlPt[0].Direction = ParameterDirection.Input;
                        sqlPt[0].Size = 20;
                        sqlPt[0].Value = sqxh;

                  
                        sqlPt[1].ParameterName = "@StudyState";
                        sqlPt[1].SqlDbType = SqlDbType.Int;
                        sqlPt[1].Direction = ParameterDirection.Input;
                        sqlPt[1].Value = 1;
            
                        //sqlPt[2].ParameterName = "result";
                        //sqlPt[2].SqlDbType = SqlDbType.Int;
                        //sqlPt[2].Direction = ParameterDirection.Output;

                        string err_msg = "";

                        db_tj.ExecuteNonQuery(tjodbc, "dbo.Pro_Pacs_ReqStauts_BL", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
                        aa.ExecuteSQL("update  T_JCXX  set F_TJXTBJ='1'  where F_BLH='" + blh + "'");
                
                    }
                    catch(Exception   ee2)
                    {
                        log.WriteMyLog("������뵥ȷ���쳣��" + ee2.Message);
                    }
                }
                #endregion





                #region  �ش���챨��

                if (bgzt == "ȡ�����")
                {
                
                    db_tj.ExecuteNonQuery(tjodbc, "delete  from T_SYN_ZK_CHECK_BL   where StudyID='" + sqxh + "'", ref errMsg);
                  
                }
                if (bgzt == "�����")
                {
                    #region ����jpg

                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                        string errmsg = "";
                        string filename = "";
                        string tjtppath = "";
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref filename,"",ref errmsg );
                        string xy = "3";
                        if (isrtn)
                        {
                            //�����ƴ�
                            if (!File.Exists(filename))
                            {
                                ZgqClass.BGHJ(blh, "�������JPG", "���", "����JPGʧ�ܣ�δ�ҵ��ļ�" + filename, "ZGQJK", "����JPG");
                                log.WriteMyLog("δ�ҵ��ļ�" + filename);
                              
                                zgq.DelTempFile(blh);
                                return;
                            }

                            ZgqClass.BGHJ(blh, "�������JPG", "���", "����JPG�ɹ�", "ZGQJK", "����JPG");
                            string pdfpath = "";
                            bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy),ref pdfpath);
                            if (ssa == true)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("�ϴ�JPG�ɹ�");
                                filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "�ϴ�JPG", "���", "�ϴ�JPG�ɹ�:" + ML + "\\" + filename, "ZGQJK", "�ϴ�JPG");
                                tjtppath = "\\pathimages\\pdfbg\\" + ML + "\\"+blh+"\\" + filename;
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + pdfpath + "')");
                            }
                            else
                            {
                                log.WriteMyLog("�ϴ�JPGʧ�ܣ�" + errmsg);
                                ZgqClass.BGHJ(blh, "�ϴ�JPG", "���", "�ϴ�JPGʧ�ܣ�" + errmsg, "ZGQJK", "�ϴ�JPG");
                                log.WriteMyLog("�ϴ�JPGʧ�ܣ�" + errmsg);
                                zgq.DelTempFile(blh);
                                return;
                            }
                        }
                        else
                        {
                            log.WriteMyLog("�������JPGʧ�ܣ�" + errmsg);
                            ZgqClass.BGHJ(blh, "����JPG", "���", "����JPGʧ�ܣ�" + errmsg, "ZGQJK", "����JPG");
                            zgq.DelTempFile(blh);
                            return;
                        }

                    #endregion

                    DataTable dt_tj = new DataTable();
                    dt_tj = db_tj.DataAdapter(tjodbc, "select * from T_SYN_ZK_CHECK_BL   where StudyID='" + sqxh + "'", ref errMsg);
                    if (errMsg != "")
                        log.WriteMyLog(errMsg);

                    string blzd = bljc.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_TSJC"].ToString().Trim();
                    string rysj = bljc.Rows[0]["F_rysj"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_JXSJ"].ToString().Trim();
                    string bz = "";

                    if (bljc.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                    {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                        if (TJ_bljc == null || TJ_bljc.Rows.Count <= 0)
                        {

                        }
                        else
                        {
                            rysj = "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n";
                            rysj = rysj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n";
                            rysj = rysj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\r\n";
                            rysj = rysj + "��ԭ΢���" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\r\n";
                            rysj = rysj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";
                            rysj = rysj + "��֢�̶ȣ�" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim();

                            ////////////////////////////////////
                            blzd = TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim();
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                bz = bz + "�������1��" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                bz = bz + "�������2��" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim();
                        }
                    }
                    string sql_insert = "insert into T_SYN_ZK_CHECK_BL(Pacs_CheckID,CISID,StudyID,PacsItemCode,PatientNameChinese,"
                    + "PatientSex,PatientBirthday,StudyType,StudyBodyPart,ClinicDiagnose,ClinicSymptom,ClinicAdvice,IMGStrings,"
                    + "StudyState,Check_Doc,Check_Date,Report_Doc,Report_Date,Audit_Doc,Audit_Date,Status_To_Cis) values("
                    + "'" + blh + "','" + bljc.Rows[0]["F_MZH"].ToString().Trim() + "','" + sqxh + "','','" + bljc.Rows[0]["F_XM"].ToString().Trim() + "',"
                    + "'" + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_ZY"].ToString().Trim() + "','',"
                    + "'" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "','" + blzd + "','" + rysj + "','" + bz + "','" + tjtppath + "',5,"
                    + "'" + bljc.Rows[0]["F_QCYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "',"
                    + "'" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',"
                    + "'" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "',0"
                    + ")";

                    if (dt_tj.Rows.Count > 0)
                    {
                        sql_insert = "update  T_SYN_ZK_CHECK_BL  set Pacs_CheckID='" + blh + "',CISID='" + bljc.Rows[0]["F_MZH"].ToString().Trim()
                            + "',PatientNameChinese='"+ bljc.Rows[0]["F_XM"].ToString().Trim()+"',"
                    + "PatientSex='" + bljc.Rows[0]["F_XB"].ToString().Trim() + "',PatientBirthday='" + bljc.Rows[0]["F_ZY"].ToString().Trim()
                    + "',StudyBodyPart='" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "',ClinicDiagnose='"+blzd+"',ClinicSymptom='"+rysj+"',ClinicAdvice='"+bz
                    + "',IMGStrings='" + tjtppath + "'," + "StudyState=5,Check_Doc='" + bljc.Rows[0]["F_QCYS"].ToString().Trim()
                    + "',Check_Date='" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "',Report_Doc='" + bljc.Rows[0]["F_BGYS"].ToString().Trim()
                    + "',Report_Date='" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',Audit_Doc='" + bljc.Rows[0]["F_SHYS"].ToString().Trim()
                    + "',Audit_Date='" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "',Status_To_Cis=0  "
                    + " where StudyID='" + sqxh + "'";
                    }

                    if(debug=="1")
                    log.WriteMyLog(sql_insert);
      
                    int z = db_tj.ExecuteNonQuery(tjodbc, sql_insert, ref errMsg);
                  if (z > 0)
                  {
                      if(debug=="1")
                      log.WriteMyLog("д�����ݿ�ɹ�");
                      aa.ExecuteSQL("update  T_JCXX  set F_TJXTBJ='2'  where F_BLH='" + blh + "'");
                  }
                  else
                  {
                      log.WriteMyLog("д������ʧ�ܣ�" + errMsg);
                  }
               
                }
                #endregion
                return;
                #endregion
            }

            if (hisjk=="1")
            {
                if (bglx == "bc" || bglx == "bd")
                    return;
                

                if (bljc.Rows[0]["F_hisbj"].ToString().Trim()==null  ||bljc.Rows[0]["F_hisbj"].ToString().Trim() != "1")
                {
                    string brlbbm = "";
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "����")
                        brlbbm = "0";
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                        brlbbm = "1";

                    if (brlbbm != "")
                    {
                        if (bljc.Rows[0]["F_hisbj"].ToString().Trim() != "1")
                            ZtToHIS(brlbbm,sqxh, bljc.Rows[0]["F_BRBH"].ToString().Trim(), yhbh, blh, debug);
                    }
                    
                }
                
            }
            if (ptjk == "1")
            { 
                if (sqxh != "")
                {
                #region ƽ̨�ӿ�
                    DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + sqxh + "'", "t_sqd");
                    if (dt_sqd.Rows.Count < 1)
                    {
                        log.WriteMyLog("�����:" + blh + ";T_SQD�����޼�¼,������");
                        goto pacs;
                    }
                    if (dt_sqd.Rows[0]["F_SQDZT"].ToString() != "�ѵǼ�")
                        aa.ExecuteSQL("update T_SQD set F_SQDZT='�ѵǼ�' where F_sqxh='" + sqxh + "'");
                   
                    ///�ش�״̬
                    if (hczt == "1")
                        ZtToPt(bljc, dt_sqd, blh, bglx, bgxh, bgzt, dt_sqd.Rows[0]["F_JZLB"].ToString(), yhmc, yhbh, debug);
                  
                    /// �ش�����
                    if (hcbg == "1")
                    {
                        if (bgzt == "�����")
                            BgToPt(bljc, dt_sqd, blh, bglx, bgxh, dt_sqd.Rows[0]["F_JZLB"].ToString(), debug);
                    }
                #endregion
                }
            }
            pacs:
            if (topacs == "1")
            {
                DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");
                ToPacs(bljc, txlb, bljc.Rows[0]["F_BGZT"].ToString().Trim(),blh,debug);
            }
        }
        public void ZtToHIS(string brlbbm, string sqxh, string brbh, string czygh, string F_blh, string debug)
        {

            OleDbParameter[] ops = new OleDbParameter[5];
            for (int j = 0; j < ops.Length; j++)
            {
                ops[j] = new OleDbParameter();
            }
            ops[0].ParameterName = "v_flag_mz_zy";
            ops[0].OleDbType = OleDbType.VarChar;
            ops[0].Direction = ParameterDirection.Input;
            ops[0].Size = 20;
            ops[0].Value = brlbbm;

            ops[1].ParameterName = "v_patient_id";//
            ops[1].OleDbType = OleDbType.VarChar;
            ops[1].Direction = ParameterDirection.Input;
            ops[1].Size = 20;
            ops[1].Value =brbh;

            ops[2].ParameterName = "v_page_no";//
            ops[2].OleDbType = OleDbType.VarChar;
            ops[2].Direction = ParameterDirection.Input;
            ops[2].Size = 20;
            ops[2].Value = sqxh;

            ops[3].ParameterName = "v_opera";//
            ops[3].OleDbType = OleDbType.VarChar;
            ops[3].Direction = ParameterDirection.Input;
            ops[3].Size = 10;
            ops[3].Value = czygh;

            ops[4].ParameterName = "v_RetError";//
            ops[4].OleDbType = OleDbType.VarChar;
            ops[4].Direction = ParameterDirection.Output;
            ops[4].Size = 200;

            

            //��д�Ǽ�״̬
            string odbcsql_his = ZgqClass.GetSz("savetohis", "hisodbc", "Provider=MSDAORA;Data Source=ORCL68;User id=chisdb_dev;Password=chisdb_dev;").Replace("\0", "").Trim();

            OleDbDB oledb = new OleDbDB();
            string message_ee = "";
            oledb.ExecuteNonQuery(odbcsql_his, "langjia_to_charge", ref ops, CommandType.StoredProcedure, ref message_ee);

            if (message_ee.Trim() != "")
            {
                log.WriteMyLog("��HIS�洢����langjia_to_charge�쳣��" + message_ee);
            }
            else
            {
                MessageBox.Show(ops[4].Value.ToString());
                if (debug == "1")
                    log.WriteMyLog("��д���뵥�Ǽ�״̬�ɹ�");
                aa.ExecuteSQL("update t_jcxx set F_hisbj='1' where f_blh='" + F_blh + "'");
            }
            return;
        }
     
        //Pt
        public void ZtToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string bgzt,string brlb,string yhmc,string yhbh,string  debug)
        {

            if (bglx == "bc" || bglx == "bd")
                return;
         
            string errmsg = "";
            string message = ZtMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh, bgzt, ref errmsg, yhmc,yhbh);

            if (message == "")
            {
                log.WriteMyLog("MQ״̬����XMlʧ�ܣ�" + errmsg);
                return;
            }
       
            if(debug=="1")
                log.WriteMyLog("MQ״̬�ش���"+message);

            string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BLToFZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;

                string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS004.LQ", "BS004", jzlb, "0", "S007", "46300096", "2030000", "0");

                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS004)MQ״̬���ʹ���" + rtnmsg);
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS004)MQ״̬������ɣ�" + rtnmsg);
                }
            }
            catch(Exception  ee2)
            {
                log.WriteMyLog("(BS004)MQ״̬�����쳣��" + ee2.Message);
            }
            return;
        }
     
        public void BgToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string brlb,string debug)
        {
           
            //����pdf
            string ML = DateTime.Parse(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            string errmsg = ""; string pdf2base64 = ""; string filename = "";
            bool isbase64 = true; bool scpdf = true; bool uppdf = true;
            C_PDF(ML, blh, bgxh, bglx, ref errmsg, ref isbase64, ref pdf2base64, ref filename, ref scpdf, ref uppdf, debug);
            log.WriteMyLog("10");
            if (pdf2base64.Length < 10)
            {
                log.WriteMyLog("����PDFתbase64ʧ��");
            }
            log.WriteMyLog("����pdf�ɹ�");
            string message = BgMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh,ref errmsg,pdf2base64);
            log.WriteMyLog("������Ϣ�ɹ�");
            if (message == "")
            {
                log.WriteMyLog("MQ��������XMlʧ�ܣ�" + errmsg);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='MQ��������XMlʧ��:" + errmsg+"' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_BGZT='�����'");
                return;
            }

            if (debug == "1")
                log.WriteMyLog("MQ���淢�ͣ�" + message);

            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BL2FZMQWeb.Service fzmq = new PathHISZGQJK.BL2FZMQWeb.Service();
               // BL2FZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;

                string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
                string rtnmsg = fzmq.SendBgMsgToMQ(message, "IN.BS320.LQ", "BS320", jzlb, "0", "S007", "46300096", "2030000", "0");
               
                if (debug == "1")
                    log.WriteMyLog("(BS320)MQ���淵�أ�" + rtnmsg);
                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS320)MQ���淢�ʹ���" + rtnmsg);
                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS320)MQ���淢�ʹ���" + rtnmsg + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_BGZT='�����' ");
            
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS320)MQ���淢����ɣ�" + rtnmsg);
                    aa.ExecuteSQL("update T_jcxx set F_CFSH='1' where F_blh='" + blh + "'");

                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS320)MQ���淢����ɣ�" + rtnmsg + "',F_FSZT='�Ѵ���' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_BGZT='�����' ");
                    return;
                }
                return;
            }
            catch(Exception  ee3)
            {
                log.WriteMyLog("(BS320)MQ���淢���쳣��" + ee3.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS320)MQ���淢���쳣��" + ee3.Message + "'  where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_BGZT='�����'");
                return;
            }
        }

        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + int.Parse(blh[i].ToString()).ToString();
                }
                catch { }
            }
            return x;
        }

        public string  ZtMsg(DataTable dt, DataTable dt_sqdxx, ref string xbrlb, string blh, string bglx, string bgxh, string bgzt, ref string errmsg,string yhmc,string yhbh)
        {
          

          
            string bgztbm = "";
            string bgztstr = "";

            if (bgzt == "�����")
            {
                bgztbm = "170.003";
                bgztstr = "��鱨�������";
            }
            if (bgzt == "��д����")
            {
                bgztbm = "160.003";
                bgztstr = "��������";
            }
     
            if (bgzt == "��ȡ��")
            {
                bgztbm = "140.002";
                bgztstr = "����ѵ���";
            }
            if (bgzt == "�ѵǼ�")
            {
                bgztbm = "140.002";
                bgztstr = "����ѵ���";
            }
            if (bgzt == "ȡ�����")
            {
                bgztbm = "990.001";
                bgztstr = "�����ٻ�";
            }

            if (bgztstr == "" || bgztbm.Trim() == "")
            {
          
                return "";
            }
            try
            {
                xbrlb = dt_sqdxx.Rows[0]["F_jzlb"].ToString();
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3  ../../Schemas/POOR_IN200901UV23.xsd\">";

                xml = xml + "<!-- ��ϢID -->";
                xml = xml + "<id extension=\"BS004\" />";
                xml = xml + "<!-- ��Ϣ����ʱ�� -->";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<interactionId extension=\"POOR_IN200901UV23\" />";
                xml = xml + "<!--��Ϣ��;: P(Production); D(Debugging); T(Training) -->";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<!-- ��Ϣ����ģʽ: A(Archive); I(Initial load); R(Restore from archive); T(Current  processing) -->";
                xml = xml + "<processingModeCode code=\"R\" />";
                xml = xml + "<!-- ��ϢӦ��: AL(Always); ER(Error/reject only); NE(Never) -->";
                xml = xml + "<acceptAckCode code=\"NE\" />";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";

                xml = xml + "<!-- ������ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<!-- ������ -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ������ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"S009\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<!-- ��װ����Ϣ����(��Excel��д) -->";
                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<!-- ��Ϣ�������� @code: ���� :new �޸�:update -->";
                xml = xml + "<code code=\"update\"></code>";
                xml = xml + "<subject typeCode=\"SUBJ\" xsi:nil=\"false\">";
                xml = xml + "<placerGroup>";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<code></code>";
                xml = xml + "<!-- �������뵥״̬ ������δʹ�� -->";
                xml = xml + "<statusCode code=\"active\"></statusCode>";
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<id>";
                xml = xml + "<!--��ID -->";
                xml = xml + "<item root=\"1.2.156.112675.1.2.1.2\" extension=\"" + dt_sqdxx.Rows[0]["F_yid"].ToString() + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<item root=\"1.2.156.112675.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item root=\"1.2.156.112675.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_jzh"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"  determinerCode=\"INSTANCE\">";
                xml = xml + "<!--���˿��ұ���-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ksbm"].ToString() + "\" root=\"1.2.156.112675.1.1.1\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--���˿������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_ksmc"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--ҽ�ƻ������� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yybm"].ToString() + "\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--ҽ�ƻ������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item><part value=\"" + dt_sqdxx.Rows[0]["F_yymc"].ToString() + "\" /></item>";
                xml = xml + "</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</providerOrganization>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<time>";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</time>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- �����˱��� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\""+yhbh+"\" root=\"1.2.156.112675.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ���������� ��������ʹ�� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\""+yhmc+"\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<!--ִ�п��� -->";
                xml = xml + "<location typeCode=\"LOC\" xsi:nil=\"false\">";
                xml = xml + "<!--������δʹ�� -->";
                xml = xml + "<time />";
                xml = xml + "<!--�������/���� -->";
                xml = xml + "<serviceDeliveryLocation classCode=\"SDLOC\">";
                xml = xml + "<serviceProviderOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--ִ�п��ұ��� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_zxksbm"].ToString() + "\" root=\"1.2.156.112675.1.2.1.6\" />";
                xml = xml + "</id>";
                xml = xml + "<!--ִ�п������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_zxks"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</serviceDeliveryLocation>";
                xml = xml + "</location>";
            
                xml = xml + "<!-- 1..n��ѭ��  ҽ��״̬��Ϣ -->";
                xml = xml + "<component2>";
                xml = xml + "<!--ҽ�����-->";
                xml = xml + "<sequenceNumber value=\"1\"/>";
                xml = xml + "<observationRequest classCode=\"OBS\">";
                xml = xml + "<!-- ��������ʹ�� -->";
                xml = xml + "<id>";
                xml = xml + "<!-- ҽ���� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yzh"].ToString() + "\" root=\"1.2.156.112675.1.2.1.22\"/>";
                xml = xml + "<!-- ���뵥�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_sqxh"].ToString() + "\" root=\"1.2.156.112675.1.2.1.21\"/>";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item extension=\"" + blh + "\" root=\"1.2.156.112675.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112675.1.2.1.30\"/>";
                xml = xml + "</id>";
             
                xml = xml + "<!-- ҽ��������/ҽ��������� - ���ҩƷ, ������, ������, Ƭ��ҩƷ, ������ -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.27\">";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_YZLXMC"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";
                
                xml = xml + "<!-- �걾��Ϣ -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                string [] bbtmh = dt_sqdxx.Rows[0]["F_bbtmh"].ToString().Trim().Split('#');
                foreach (string bbtm in bbtmh)
                {
                    xml = xml + "<specimen classCode=\"SPEC\">";
                    xml = xml + "<!--�걾����� ��������ʹ�� -->";
                    xml = xml + "<id extension=\"" + bbtm + "\" />";
                    xml = xml + "<!--������Ŀδʹ�� -->";
                    xml = xml + "<code />";
                    xml = xml + "<subjectOf1 typeCode=\"SBJ\" contextControlCode=\"OP\">";
                    xml = xml + "<specimenProcessStep moodCode=\"EVN\" classCode=\"SPECCOLLECT\">";
                    xml = xml + "<!-- �ɼ����� -->";
                    xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                 
                    try
                    {
                        xml = xml + "<any value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMddHHmmss")  + "\"></any>";
                    }
                    catch
                    {
                        xml = xml + "<any value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMddHHmmss")  + "\"></any>";
                    }
                    xml = xml + "</effectiveTime>";
                    xml = xml + "<performer typeCode=\"PRF\">";
                    xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                    xml = xml + "<!-- �ɼ���Id -->";
                    xml = xml + "<id>";
                    xml = xml + "<item extension=\"" + getyhgh(dt.Rows[0]["F_QCYS"].ToString()) + "\" root=\"1.2.156.112675.1.1.2\"></item>";
                    xml = xml + "</id>";
                    xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                    xml = xml + "<!-- �ɼ������� -->";
                    xml = xml + "<name xsi:type=\"BAG_EN\">";
                    xml = xml + "<item>";
                    xml = xml + "<part value=\"" + dt.Rows[0]["F_QCYS"].ToString() + "\" />";
                    xml = xml + "</item>";
                    xml = xml + "</name>";
                    xml = xml + "</assignedPerson>";
                    xml = xml + "</assignedEntity>";
                    xml = xml + "</performer>";
                    xml = xml + "</specimenProcessStep>";
                    xml = xml + "</subjectOf1>";
                    xml = xml + "</specimen>";

                }
                xml = xml + "</specimen>";

                xml = xml + "<!-- ԭ�� -->";
                xml = xml + "<reason contextConductionInd=\"true\">";
                xml = xml + "<observation moodCode=\"EVN\" classCode=\"OBS\">";
                xml = xml + "<!-- ������ δʹ��-->";
                xml = xml + "<code></code>";
                xml = xml + "<value xsi:type=\"ST\" value=\"\"/>";
                xml = xml + "</observation>";
                xml = xml + "</reason>";
                xml = xml + "<!-- ҽ��ִ��״̬ -->";
                xml = xml + "<component1 contextConductionInd=\"true\">";
                xml = xml + "<processStep classCode=\"PROC\">";
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112675.1.1.93\">";
                xml = xml + "<!--ҽ��ִ��״̬���� -->";
                xml = xml + "<displayName value=\"" + bgztstr + "\" />";
                xml = xml + "</code>";
                xml = xml + "</processStep>";
                xml = xml + "</component1>";
                xml = xml + "</observationRequest>";
                xml = xml + "</component2>";

                xml = xml + "<!--���� -->";
                xml = xml + "<componentOf1 contextConductionInd=\"false\" xsi:nil=\"false\" typeCode=\"COMP\">";
                xml = xml + "<!--���� -->";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<id>";
                xml = xml + "<!-- ������� ��������ʹ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112675.1.2.1.7\" />";
                xml = xml + "<!-- ������ˮ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112675.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--����������-->";
                xml = xml + "<code codeSystem=\"1.2.156.112675.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
                xml = xml + "<!-- ����������� -->";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_BRLB"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!--������δʹ�� -->";
                xml = xml + "<statusCode code=\"Active\" />";
                xml = xml + "<!--���� ������δʹ�� -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\" />";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf1>";
                xml = xml + "</placerGroup>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</POOR_IN200901UV>";


                return FormatXml(xml);
            }
            catch (Exception ex2)
            {
                errmsg = "ERR:" + ex2.Message; return "";
            }
        }

        public string BgMsg(DataTable dt, DataTable dt_sqd, ref string xbrlb, string blh, string bglx, string bgxh, ref string errmsg, string pdf2base64)
        {
          
            try
            { 
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<ClinicalDocument xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../coreschemas/CDA.xsd\">";
                //    <!-- �ĵ����÷�Χ���� -->
                xml = xml + "<realmCode code=\"CN\"/>";
                //    <!-- �ĵ���Ϣģ�����-��ʶ�� -->
                //    <!-- �̶�ֵ -->
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_HD000040\"/>";
                //    <!-- �ĵ���ʶ-����� -->
                string bgh = blh;
                string bgys = dt.Rows[0]["f_bgys"].ToString();
                string shys = dt.Rows[0]["f_shys"].ToString();
                string bgrq = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                if (bglx == "bc")
                {
                    bgh = bgh + "_bc_" + bgxh;
                    bgys = dt.Rows[0]["f_bc_bgys"].ToString();
                    shys = dt.Rows[0]["f_bc_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                }
                if (bglx == "bd")
                {
                    bgh = bgh + "_bd_" + bgxh;
                    bgys = dt.Rows[0]["f_bd_bgys"].ToString();
                    shys = dt.Rows[0]["f_bd_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                }
                string bgysid = "";
                string shysid = "";
                string qcysid = "";
                bgysid = getyhgh(bgys);
                shysid = getyhgh(shys);
                qcysid = getyhgh(dt.Rows[0]["F_QCYS"].ToString());
             

                xml = xml + "<id root=\"S038\" extension=\"" + bgh + "\"/>";
                xml = xml + "<!-- �ĵ���ʶ-���� / �ĵ���ʶ-������ -->";
                xml = xml + "<!-- �̶�ֵ -->";
                xml = xml + "<code code=\"04\" codeSystem=\"1.2.156.112675.1.1.60\" displayName=\"�������¼\"/>";
                xml = xml +"<!-- �ĵ������ı� -->";
                xml = xml + "<title>�����鱨��</title>";
                xml = xml + "<!-- �ĵ���Ч���� -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" />";
                xml = xml + "<!-- �ĵ��ܼ����� -->";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"2.16.840.1.113883.5.25\" codeSystemName=\"Confidentiality\" displayName=\"normal\" />";
                xml = xml + "<!-- �ĵ����Ա��� -->";
                xml = xml + "<languageCode code=\"zh-CN\" />";
                xml = xml + "<!--����ID-->";
                xml = xml + "<setId extension=\"BS320\"/>";
                //��Ҫ����ȷ�������������޸�
                string cfsh = "0";
                try
                {
                    cfsh = dt.Rows[0]["F_cfsh"].ToString().Trim();
                }
                catch
                {
                }
                if (bglx == "bc"|| bglx=="bd")
                {
                    cfsh = "0";
                }
                if (cfsh == "")
                    cfsh = "0";
                xml = xml + "<!-- �ĵ��Ĳ����汾:0��ʾ����, 1��ʾ�޸� -->";
                xml = xml + "<versionNumber value=\"" + cfsh + "\"/>";

                xml = xml + "<!-- �ĵ���¼���� -->";
                xml = xml + "<recordTarget typeCode=\"RCT\">";
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<patientRole classCode=\"PAT\">";
                xml = xml + "<!-- ��ID -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.2\" extension=\"" + dt_sqd.Rows[0]["F_yid"].ToString().Trim() + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.3\" extension=\"" + dt_sqd.Rows[0]["F_BRBH"].ToString().Trim() + "\" />";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.12\" extension=\"" + dt_sqd.Rows[0]["F_JZH"].ToString().Trim() + "\" />";

                xml = xml + "<!-- ����������Ϣ -->";
                xml = xml + "<addr use=\"TMP\">";
                xml = xml + "<!-- ��λ�� -->";
                xml = xml + "<careOf>" + dt_sqd.Rows[0]["F_CH"].ToString().Trim() + "</careOf>";
                xml = xml + "</addr>";

                xml = xml + "<!-- ���˻�����Ϣ -->";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<name>" + dt.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "<!-- �Ա����/�Ա����� -->";
                xml = xml + "<administrativeGenderCode code=\"" + dt_sqd.Rows[0]["F_XBBM"].ToString().Trim() + "\" codeSystem=\"1.2.156.112675.1.1.3\" displayName=\"" + dt_sqd.Rows[0]["F_XB"].ToString().Trim() + "\" />";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<birthTime value=\"" + dt_sqd.Rows[0]["F_CSRQ"].ToString() + "\" />";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                xml = xml + "<!-- �ĵ�����(��鱨��ҽ��, ��ѭ��) -->";
                xml = xml + "<author typeCode=\"AUT\">";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<time value=\"" + DateTime.Parse(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                xml = xml + "<!-- ����ҽ������ -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ����ҽ������ -->";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";

                xml = xml + "<dataEnterer typeCode=\"ENT\">";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- ��¼�߱��� -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\"\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ��¼������ -->";
                xml = xml + "<name>"+dt.Rows[0]["F_JSY"].ToString()+"</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</dataEnterer>";


                xml = xml + "<!-- �ĵ�������(CDA��custodianΪ������) -->";
                xml = xml + "<custodian>";
                xml = xml + "<assignedCustodian>";
                xml = xml + "<representedCustodianOrganization>";
                xml = xml + "<!-- ҽ�ƻ������� -->";
                xml = xml + "<id root=\"1.2.156.112675\" extension=\"" + dt_sqd.Rows[0]["F_YYBM"].ToString().Trim() + "\" />";
                xml = xml + "<!-- ҽ�ƻ������� -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_YYMC"].ToString().Trim() + "</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";

                xml = xml + "<!-- ����ǩ����Ϣ -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time />";
                xml = xml + "<signatureCode code=\"S\" />";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- ����ǩ�º�-->";
                xml = xml + "<id extension=\"\" />";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";

                xml = xml + "<!-- �ĵ������(��鱨�����ҽʦ, ��ѭ��) -->";
                xml = xml + "<authenticator>";
                xml = xml + "<!-- ������� -->";
                string shsj = "";
                try
                {
                    shsj = Convert.ToDateTime(dt.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                }
                catch
                {
                }
                if (shsj == "")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                }
                if (bglx == "bc")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                if (bglx == "bd")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                xml = xml + "<time value=\"" + shsj + "\" />";
                xml = xml + "<signatureCode code=\"S\"/>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- ���ҽ������ -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\"" + shysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ���ҽ������ -->";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                xml = xml + "<!-- ����ҽ����Ϣ -->";
                xml = xml + "<participant typeCode=\"AUT\">";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- ����ҽ������ -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.2\" extension=\""+ dt_sqd.Rows[0]["F_SQYSBM"].ToString().Trim()+"\" />";
                xml = xml + "<associatedPerson>";
                xml = xml + "<!-- ����ҽ������ -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQYS"].ToString().Trim() + "</name>";
                xml = xml + "</associatedPerson>";
               
                xml = xml + "<scopingOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
			    xml = xml + "<!-- ������ұ��� -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.1\" extension=\"" + dt_sqd.Rows[0]["F_SQKSBM"].ToString().Trim() + "\"/>";
	            xml = xml + "<!-- ����������� -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQKS"].ToString().Trim() + "</name>";
			    xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";


                xml = xml + "<!-- ����ҽ����Ϣ -->";
                xml = xml + "<inFulfillmentOf>";
                xml = xml + "<order>";
                xml = xml + "<!-- ����ҽ����(�ɶ��) -->";
                xml = xml + "<id extension=\"" + dt_sqd.Rows[0]["F_YZH"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";

                xml = xml + "<!-- �ĵ���ҽ�������¼��ľ��ﳡ�� -->";
                xml = xml + "<componentOf typeCode=\"COMP\">";
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<encompassingEncounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!-- ������� -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.7\" extension=\"" + dt_sqd.Rows[0]["F_jzcs"].ToString() + "\"/>";
                xml = xml + "<!-- ������ˮ�� -->";
                xml = xml + "<id root=\"1.2.156.112675.1.2.1.6\" extension=\"" + dt_sqd.Rows[0]["F_JZLSH"].ToString() + "\"/>";			
			    xml = xml + "<!-- ����������/����������� -->";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_jzlb"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.80\" displayName=\"" + dt_sqd.Rows[0]["F_brlb"].ToString() + "\" />";
                xml = xml + "<effectiveTime />";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";
                //<!--
                //********************************************************
                //CDA Body
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<structuredBody>";
                //<!-- 
                //********************************************************
                //�ĵ��л��������Ϣ
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"34076-0\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Information for patients section\" />";
                xml = xml + "<title>�ĵ��л��������Ϣ</title>";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"397669002\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"age\" />";
                xml = xml + "<value xsi:type=\"ST\">" + dt.Rows[0]["F_nl"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";
               xml = xml + "<!-- ���� -->";
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\" >";
                xml = xml + "<code code=\"225746001\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"ward\" />";
                xml = xml + "<!-- �������� �������� -->";
                xml = xml + "<value xsi:type=\"SC\" code=\"" + dt_sqd.Rows[0]["F_BQBM"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.33\">" + dt_sqd.Rows[0]["F_BQ"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";

                //xml = xml + "<component>";
                //xml = xml + "<section>";
                //xml = xml + "<code code=\"49033-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Menstrual History\"></code>";
                //xml = xml + "<entry>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<code code=\"8665-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Last menstrual period start date\"></code>";
                //xml = xml + "<!--ĩ���¾�ʱ��-->";
                //xml = xml + "<value xsi:type=\"TS\" value=\"\"></value>";
                //xml = xml + "</observation>";
                //xml = xml + "</entry>";
                //xml = xml + "<entry>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<code code=\"63890-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Age at last menstrual period \"></code>";
                //xml = xml + "<!--����-->";
                //xml = xml + "<value xsi:type=\"ST\"></value>";
                //xml = xml + "</observation>";
                //xml = xml + "</entry>";
                //xml = xml + "</section>";
                //xml = xml + "</component>";

                xml = xml + "</section>";
                xml = xml + "</component>";

                //<!--
                //********************************************************
                //����½�
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"30954-2\" displayName=\"STUDIES SUMMARY\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\"/>";
                xml = xml + "<title>������</title>";



                // <!-- �����Ϣ -->
                xml = xml + "<entry>";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"310388008\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"relative information status\" />";
                xml = xml + "<statusCode code=\"completed\" />";
                //<!-- ��λͼ����Ϣ -->
                xml = xml + "<component>";
                xml = xml + "<supply classCode=\"SPLY\" moodCode=\"EVN\">";
                //<!-- ͼ��������(accessionNumber) -->
                xml = xml + "<id extension=\"" + "1001" + "\" />";
                xml = xml + "</supply>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- ͼƬ��Ϣ(Ҫ�����ΪBASE64), @mediaType: ͼƬ��ʽ(JPG��ʽ: image/jpeg PDF��ʽΪ: application/pdf) -->";
                xml = xml + "<value xsi:type=\"ED\" mediaType=\"application/pdf\">" + pdf2base64 + "</value>";
                xml = xml + "</observationMedia>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //<!-- ��鱨�����ͱ�ʶ����/��鱨�����ͱ�ʶ���� --0�������棬1���䱨��>
                string bglxstr = "0";
                if (bglx == "bc" || bglx == "bd")
                {
                    bglxstr = "1";
                }
                xml = xml + "<code code=\"" + bglxstr + "\" codeSystem=\"1.2.156.112675.1.1.112\" displayName=\"�����鱨��\" />";
                xml = xml + "</observation>";
                xml = xml + "</component>";

             

                xml = xml + "</organizer>";
                xml = xml + "</entry>";


                ////<!--****************************************************************************-->
                xml = xml + "<!-- ��鱨����Ŀ -->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                 xml = xml + "<id extension=\""+blh+"\" />";
                xml = xml + "<!-- ������ͱ���/����������� -->"; 
                //������ͱ���û�У���������OT
                xml = xml + "<code code=\"185\" codeSystem=\"1.2.156.112675.1.1.41\" displayName=\"����\" />";
                xml = xml + "<!-- ����̶��� -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ////// <!-- ���ʹ���Լ���Ϣ -->
                ////xml = xml + "<participant typeCode=\"CSM\">";
                ////xml = xml + "<participantRole>";
                ////xml = xml + "<playingEntity>";
                //////<!-- �Լ�����/�Լ����� -->
                ////xml = xml + "<code code=\"\" displayName=\"\" />";
                //////<!-- �Լ���������λ -->
                ////xml = xml + "<quantity value=\"\" unit=\"\" />";
                ////xml = xml + "</playingEntity>";
                ////xml = xml + "</participantRole>";
                ////xml = xml + "</participant>";
                xml = xml + "<!-- study -->";
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- �����Ŀ����/�����Ŀ���� -->";
                //  string yzxm = dt.Rows[0]["F_yzxm"].ToString().Trim();
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_yzxmbm"].ToString() + "\" codeSystem=\"1.2.156.112675.1.1.88\" displayName=\"" + dt_sqd.Rows[0]["F_yzxm"].ToString() + "\"/>";
                xml = xml + "<!-- ��鱸ע -->";
                xml = xml + "<text></text>";
                xml = xml + "<!-- ����̶��� -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!-- ��鱨����-�͹�����/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->";
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112675.1.1.98\">";
                //xml = xml + "<originalText>" + System.Security.SecurityElement.Escape(dt.Rows[0]["F_rysj"].ToString());
                xml = xml + "<originalText> " + System.Security.SecurityElement.Escape(dt.Rows[0]["F_RYSJ"].ToString()+"\r\n"+dt.Rows[0]["F_JXSJ"].ToString()) + "</originalText>";
                xml = xml + "</value>";
                // <!-- ��鱨����-������ʾ/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112675.1.1.98\">";
                string blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_blzd"].ToString()+"\r\n"+dt.Rows[0]["F_TSJC"].ToString()); ;
                if (bglx == "bc")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bczd"].ToString());
                }
                if (bglx == "bd")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bdzd"].ToString());
                }
                xml = xml + "<originalText>" + blzd + "</originalText>";
                xml = xml + "</value>";

                xml = xml + "<!-- ��鷽������/��鷽������ -->";
                xml = xml + "<methodCode code=\"\"  codeSystem=\"1.2.156.112675.1.1.43\" displayName=\"\"/>";
                xml = xml + "<!-- ��鲿λ����/��鲿λ���� -->";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156112649.1.1.42\" displayName=\"\" />";
                xml = xml + "<!-- ���ҽʦ��Ϣ -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<!-- ������� -->";
                xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss")  + "\"/>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- ���ҽ������ -->";
                xml = xml + "<id  root=\"1.2.156.112675.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ���ҽ������ -->";
                xml = xml + "<name>" + dt.Rows[0]["F_bgys"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ��Ͽ��ұ��� -->";
                xml = xml + "<id root=\"1.2.156.112675.1.1.1\" extension=\"2070000\"/>";
                xml = xml + "<!-- ��Ͽ������� -->";
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + " </assignedEntity>";
                xml = xml + "</performer>";


                //<!-- ���ҽʦ��Ϣ -->
                xml = xml + "<performer>";
                // <!-- ������� -->
                try
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                xml = xml + "<assignedEntity>";
                //<!-- ���ҽ������ -->
                xml = xml + "<id  root=\"1.2.156.112675.1.1.2\" extension=\"" + qcysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                // <!-- ���ҽ������ -->
                xml = xml + "<name>" + dt.Rows[0]["F_QCYS"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //<!-- �����ұ��� -->
                xml = xml + "<id root=\"1.2.156.112675.1.1.1\" extension=\"2070000\"/>";
                //<!-- ���������� -->
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                string[] F_fzys = dt.Rows[0]["F_FZYS"].ToString().Split('/');
                foreach (string fzys in F_fzys)
                {
                    if (fzys != "")
                    {
                        xml = xml + "<!-- ����ҽʦ��Ϣ -->";
                        xml = xml + "<participant typeCode=\"VRF\">";
                        xml = xml + "<participantRole>";
                        xml = xml + "<!-- ����ҽ������/����ҽ�� -->";
                        xml = xml + "<id  root=\"1.2.156.112675.1.1.2\" extension=\"" + getyhgh(fzys) + "\"/>";
                        xml = xml + "<playingEntity>";
                        xml = xml + "<!-- ����ҽ������ -->";
                        xml = xml + "<name>" + fzys + "</name>";
                        xml = xml + "</playingEntity>";
                        xml = xml + "</participantRole>";
                        xml = xml + "</participant>";
                    }
                }


                xml = xml + "<!-- �걾��Ϣ -->";
                xml = xml + "<participant typeCode=\"SPC\">";
                xml = xml + "<participantRole>";
                xml = xml + "<!--�걾����-->";
                xml = xml + "<code code=\"\" displayName=\"\"></code>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";
                	
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<participant typeCode=\"DEV\">";

                xml = xml + "<time>";
	            xml = xml + "<low value=\"\"/>";
	            xml = xml + "<width xsi:type=\"PQ\" value=\"24\" unit=\"Сʱ\"/>";
                xml = xml + "</time>";

                xml = xml + "<participantRole>";
                xml = xml + "<playingDevice>";
                xml = xml + "<!--�����ͺ� ��������-->";
                xml = xml + "<manufacturerModelName code=\"\"  displayName=\"\"/>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";


                xml = xml + "<!-- ҩ�������Ϣ -->";
                xml = xml + "<entryRelationship typeCode=\"REFR\">";
                xml = xml + "<substanceAdministration classCode=\"SBADM\" moodCode=\"EVN\">";
                xml = xml + "<!--��ҩ��ʽ-->";
                xml = xml + "<routeCode code=\"\"  displayName=\"\" codeSystem=\"1.2.156.112675.1.1.38\"></routeCode>";
                xml = xml + "<!--ҩ����� ҩ�������λ-->";
                xml = xml + " <doseQuantity value=\"\" unit=\"\"></doseQuantity>";
                xml = xml + " <consumable>";
                xml = xml + "<manufacturedProduct>";
                xml = xml + "<manufacturedLabeledDrug>";
                xml = xml + "<!-- �Լ����� -->";
                xml = xml + "<code code=\"\"></code>";
                xml = xml + "<!-- �Լ����� -->";
                xml = xml + "<name xsi:type=\"\"></name>";
                xml = xml + "</manufacturedLabeledDrug>";
                xml = xml + "</manufacturedProduct>";
                xml = xml + "</consumable>";
                xml = xml + "</substanceAdministration>";
                xml = xml + "</entryRelationship>";

                //������ҽ���͹�������Ϣ(�����Ķ�����Ƚṹ���������ֵ���Ϣ)
                xml = xml + "<!-- ������ҽ���͹�������Ϣ(�����Ķ�����Ƚṹ���������ֵ���Ϣ) -->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"365605003\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"body measurement finding\" />";
                xml = xml + "<statusCode code=\"completed\" />";

                xml = xml + "<!-- ��Ŀ��Ϣ(��ѭ��) -->";
                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"100\" displayName=\"AOD\" />";
                xml = xml + "<value xsi:type=\"SC\">1mm</value>";
                xml = xml + "</observation>";
                xml = xml + "</component>";

                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"200\" displayName=\"LAD\" />";
                // xml = xml + "<value xsi:type=\"SC\">1mm</value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                xml = xml + "<!-- ������Ϣ�������ʽ��� -->";
                xml = xml + "</organizer>";
                xml = xml + "</entryRelationship>";




                ////xml = xml + "<!-- ͼ����Ϣ(������Ŀ��Ӧ��ͼ��) -->";
                ////xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                ////xml = xml + "<observationMedia   classCode=\"OBS\" moodCode=\"EVN\">";
                ////xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/jpg\">" ;
                ////xml = xml + "</value>";
                ////xml = xml + "</observationMedia>";
                ////xml = xml + "</entryRelationship>";
                ////xml = xml + "<!-- ���ж��Ӱ���Ӧͬһ��studyʱ,���Ը��ô�entryRelationship -->";
                ////xml = xml + "</observation></component>";

                ////////////<!-- study 2 -->
                ////xml = xml + "<component typeCode=\"COMP\">";
                ////xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                ////xml = xml + "<code code=\"1\" codeSystem=\"1.2.156.112675.1.1.88\" displayName=\"\"/>";
                ////xml = xml + "<text></text>";
                ////xml = xml + "<statusCode code=\"completed\"/>";

                //////////        <!-- ��鱨����-�͹�����/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112675.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////////        <!-- ��鱨����-������ʾ/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112675.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////////        <!-- ��鷽������/��鷽������ -->
                ////xml = xml + "<methodCode code=\"002\"  codeSystem=\"1.2.156.112675.1.1.43\" displayName=\"\"/>";
                //////////        <!-- ��鲿λ����/��鲿λ���� -->
                ////xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156.112675.1.1.42\" displayName=\"\" />";
                //////////        <!-- ���ҽʦ��Ϣ --




                xml = xml + "<!-- �걾�ɼ��ͽ�����Ϣ-->";
                xml = xml + "<entryRelationship typeCode=\"SAS\" inversionInd=\"true\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<id extension=\""+blh+"\"/>";
                xml = xml + "<code />";
                xml = xml + "<statusCode code=\"completed\" />";
                xml = xml + "<!-- �걾�ɼ�����(ȡ������) -->";
                try
                {
                    xml = xml + "<effectiveTime value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<effectiveTime value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
            
                xml = xml + "<!--DNA��ȡ����-->";
                xml = xml + "<methodCode code=\"\" displayName=\"\"/>";
                xml = xml + "<!-- ȡ�Ĳ�λ����/ȡ�Ĳ�λ���� -->";
                xml = xml + "<targetSiteCode code=\"\" displayName=\""+dt.Rows[0]["F_bbmc"].ToString()+"\" />";

                xml = xml + "<performer>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!--ȡ��ҽ������-->";
                xml = xml + "<id extension=\"\" root=\"1.2.156.112675.1.1.2\"></id>";
                xml = xml + "<assignedPerson>";
                xml = xml + "<!--ȡ��ҽ������-->";
                xml = xml + "<name>"+dt.Rows[0]["F_QCYS"].ToString()+"</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                xml = xml + "<entryRelationship typeCode=\"SAS\">";
                xml = xml + "<procedure classCode=\"TRNS\" moodCode=\"EVN\">";
                xml = xml + "<performer>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<id/>";
                xml = xml + "<!--�ͼ�ҽԺ-->";
                xml = xml + "<representedOrganization>";
                xml = xml + "<name>"+dt.Rows[0]["F_SJDW"].ToString()+"</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<participant typeCode=\"RCV\">";
                xml = xml + "<!-- ����ʱ��/�ͼ�ʱ�� -->";
                xml = xml + "<time value=\""+ Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMddHHmmss")+"\" />";
                xml = xml + "<participantRole/>";
                xml = xml + "</participant>";										
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";


                xml = xml + "<entryRelationship typeCode=\"REFR\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!--ԭ�����-->";
                xml = xml + "<id extension=\"\"></id>";
                xml = xml + "<code></code>";
                xml = xml + "<!--ԭ������ٴ����-->";
                xml = xml + "<value xsi:type=\"ED\"></value>";
                xml = xml + "</observation>";
                xml = xml + "</entryRelationship>";




                xml = xml + "</observation>";
                xml = xml + "</component>";

                //<!-- ������Ŀ������ṹ�͸�ʽ��� -->

                //<!-- ��ϵͳ�����ɵı�����,ͼ���޷�������study����Ӧʱ,ʹ�����²���������Ӱ�� -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- Ӱ����Ϣ(Ҫ�����ΪBASE64), @mediaType: Ӱ���ʽ -->
                // xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/gif\">";
                // xml = xml + "</value>";
                // xml = xml + "</observationMedia>";
                // xml = xml + "</component>";

                ////<!-- ���ж��Ӱ��ʱ,�������ϸ�ʽ��� -->
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";

                // <!-- ��ϵͳ��,�͹�����(���������)�޷���Ӧ�������study, 
                // ���Ƕ��study�Ŀ͹�����(���������)��¼��ͬһ���ı��ֶ���,
                // ʹ�����²��������ÿ͹�������������� -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // //<!-- @code:01��ʾ�͹�����, 02��ʾ������ʾ -->
                // xml = xml + "<code code=\"01\" codeSystem=\"1.2.156.112675.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //// <!-- @code:01��ʾ�͹�����, 02��ʾ������ʾ -->
                // xml = xml + "<code code=\"02\" codeSystem=\"1.2.156.112675.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";


                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                ////////********************************************************
                ////////�ٴ�����
                ////////* *******************************************************

                //xml = xml  + "<!-- �ٴ�����-->";
                //xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code /><value xsi:type=\"ED\"></value></observation></entry></section></component>";
              
                
                ////////<!-- 
                ////////****************************************************************************
                ////////  #ҩ���½�
                ////////****************************************************************************
                ////////-->

                 //xml = xml  + "<!-- ҩ���½�-->";
                //xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code code=\"\" displayName=\"\"/></observation></entry></section></component>";
               
                
                //////////<!-- 
                ////////********************************************************
                ////////���
                ////////********************************************************
                ////////-->

                xml = xml  + "<!-- ���-->";
                xml = xml + "<component><section><code code=\"29308-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Diagnosis\"/>";
                xml = xml + "<title>���</title>";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code nullFlavor=\"NA\"/>";
                xml = xml + "<entryRelationship typeCode=\"SUBJ\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- ���������/���������� -->";
                xml = xml + "<code code=\"\" codeSystem=\"1.2.156.112675.1.1.29\" displayName=\"\" />";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!-- ��������/��������(û�б���ȥ��@code) -->";
                xml = xml + "<value xsi:type=\"\" code=\"\" codeSystem=\"1.2.156.112675.1.1.30\" displayName=\"\" />";
                xml = xml + "</observation>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                ////////********************************************************
                ////////��������½ڣ�TCT��鱨�浥�ã�
                ////////********************************************************
                ////////--> 

                //xml = xml + "<!--��������½ڣ�TCT��鱨�浥�ã� -->";
                //xml = xml + "<component>";
                //xml = xml + "<section>";
                //xml = xml + "<code code=\"52535-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Other useful information\" />";
                //xml = xml + "<!-- �½ڱ��� -->";
                //xml = xml + "<title>���������Ϣ</title>"; 
                //xml = xml + "<!-- ����������� -->";
                //xml = xml + "<text></text>";
                //xml = xml +  "</section>";
                //xml = xml + "</component>";

                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</ClinicalDocument>";

                return FormatXml(xml);
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                return "";

            }
        }

        public void C_PDF(string ML, string blh, string bgxh, string bglx, ref  string errmsg, ref  bool isbase64, ref string Base64String, ref string filename, ref bool ScPDF, ref bool UpPDF, string debug)
        {

            log.WriteMyLog("1");
            ScPDF = false; UpPDF = false;
            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
            {
                #region  ����pdf

                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref errmsg, ref filename);
                string xy = "3";
                if (isrtn)
                {
                    
                    //�����ƴ�
                    if (!File.Exists(filename))
                    {
                        ZgqClass.BGHJ(blh, "����PDF", "���", "����PDFʧ�ܣ�δ�ҵ��ļ�" + filename, "ZGQJK", "����PDF");
                        log.WriteMyLog("δ�ҵ��ļ�" + filename);
                        errmsg = "δ�ҵ��ļ�" + filename;
                        zgq.DelTempFile(blh);
                        isbase64 = false;
                        return;
                    }
                 
                    ScPDF = true;
                    if (isbase64)
                    {
                        try
                        {
                            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
                            file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
                            file.Close();
                            Base64String = Convert.ToBase64String(imgByte);
                            isbase64 = true;
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("PDFת�������ƴ�ʧ��");
                            errmsg = "PDFת�������ƴ�ʧ��";
                            isbase64 = false;
                        }
                    }
                  

                    ZgqClass.BGHJ(blh, "����PDF", "���", "����PDF�ɹ�", "ZGQJK", "����PDF");
                    bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy));
                    if (ssa == true)
                    {
                        
                        if (debug == "1")
                            log.WriteMyLog("�ϴ�PDF�ɹ�");
                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDF�ɹ�:" + ML + "\\" + filename, "ZGQJK", "�ϴ�PDF");

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + filename + "')");

                        UpPDF = true;
                    }
                    else
                    {
                   
                        log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + errmsg);
                        ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDFʧ�ܣ�" + errmsg, "ZGQJK", "�ϴ�PDF");
                        UpPDF = false;
                    }
                    zgq.DelTempFile(blh);
                }
                else
                {
                  
                    ScPDF = false;
                    log.WriteMyLog("����PDFʧ�ܣ�" + errmsg);
                    ZgqClass.BGHJ(blh, "����PDF", "���", "����PDFʧ�ܣ�" + errmsg, "ZGQJK", "����PDF");
                    zgq.DelTempFile(blh);

                    return;
                }
                #endregion
            }
            else
            {
                log.WriteMyLog("��������PDF");
                return;
            }
        }

    
        //public string fsbg(string message, ref string rtnmsg, string brlb)
        //{


        //    MQQueueManager qMgr = null;
        //    Hashtable env = new Hashtable();

        //    rtnmsg = "";
        //    string hostname = f.ReadString("MQSERVER", "hostname", "192.168.171.64").Replace("\0", "");
        //    string channel = f.ReadString("MQSERVER", "channel", "IE.SVRCONN").Replace("\0", "");
        //    string qManager = f.ReadString("MQSERVER", "qManager", "GWI.QM").Replace("\0", "");
        //    string queueManager = f.ReadString("MQSERVER", "queueManager", "IN.BS366.LQ").Replace("\0", "");
        //    env.Clear();

        //    env.Add(IBM.XMS.MQC.HOST_NAME_PROPERTY, hostname);
        //    env.Add(IBM.XMS.MQC.CHANNEL_PROPERTY, channel);
        //    env.Add(IBM.XMS.MQC.CCSID_PROPERTY, 1208);
        //    env.Add(IBM.XMS.MQC.PORT_PROPERTY, 5000);
        //    env.Add(IBM.XMS.MQC.TRANSPORT_PROPERTY, IBM.XMS.MQC.TRANSPORT_MQSERIES);
        //    env.Add(IBM.XMS.MQC.USER_ID_PROPERTY, "mqm");
        //    String content = message;


        //    qMgr = new MQQueueManager(qManager, env);

        //    try
        //    {

        //        int openOptions = IBM.XMS.MQC.MQOO_OUTPUT
        //                | IBM.XMS.MQC.MQPMO_PASS_ALL_CONTEXT;
        //        MQMessage msg = new MQMessage();

        //        msg.CharacterSet = 1208;
        //        MQQueue queue = qMgr.AccessQueue(queueManager, openOptions);
        //        msg.Format = IBM.XMS.MQC.MQFMT_STRING;


        //        // �Զ�������������ο���ط�������ĵ�
        //        /// ��ϢID
        //        msg.SetStringProperty("service_id", "BS004");

        //        //01 ����,02 ����,03 סԺ,04 ���,05 תԺ
        //        msg.SetStringProperty("domain_id", brlb);
        //        // �������ID
        //        msg.SetStringProperty("apply_unit_id", "0");
        //        // ����ϵͳID
        //        msg.SetStringProperty("send_sys_id", "S007");
        //        // ҽ�ƻ�������
        //        msg.SetStringProperty("hospital_id", "44643245-7");
        //        // ִ�п���ID
        //        msg.SetStringProperty("exec_unit_id", "2070000");
        //        // ҽ��ִ�з������   ��ҽ������д���������Ĭ�ϴ�0��
        //        msg.SetStringProperty("order_exec_id", "0");


        //        // ��չ��
        //        msg.SetStringProperty("extend_sub_id", "0");


        //        msg.WriteString(content);

        //        MQPutMessageOptions pmo = new MQPutMessageOptions();
        //        pmo.Options = IBM.XMS.MQC.MQPMO_SYNCPOINT;
        //        queue.Put(msg, pmo);

        //        // queue.Put(aa);
        //        queue.Close();
        //        qMgr.Commit();

        //        //MessageBox.Show(byteToHexStr(msg.MessageId));
        //        string messageid = byteToHexStr(msg.MessageId);
        //        //��ʱ��messageid���浽��ʱ���У���ʷ���ݴ浽F_yl7
        //        qMgr.Disconnect();
        //        rtnmsg = messageid;
        //        return "";

        //        // textBox1.Text = "";               
        //    }
        //    catch (Exception ex)
        //    {
        //        rtnmsg = ex.Message;
        //         LGZGQClass.log.WriteMyLog("��Ϣ�����쳣��" + ex.Message);
        //        return "ERR";
        //    }

        //}



        public  void  ToPacs(DataTable  bljc,DataTable txlb,string bgzt,string blh,string debug)
        {
            string pacspath = f.ReadString("savetohis", "pacspath", @"\\174.30.0.105\CDA2PACS_IN").Replace("\0", "");
            if(bgzt.Trim()=="�����")
            {
                 #region ִ�д洢���̡�������SP_TransferReport

                    string database = f.ReadString("savetohis", "pacsdbbase", "Integration");
                    string server = f.ReadString("savetohis", "pacsdbserver", @"174.30.0.105\CSSERVER");
                    string uid = f.ReadString("savetohis", "pacsuid", "pacs");
                    string pwd = f.ReadString("savetohis", "pacsdbpwd", "pacs123");
                    string ProcName = f.ReadString("savetohis", "ProcName", "SP_TransferReport");
                    SqlConnection ocn = new SqlConnection("database=" + database + ";server=" + server + ";uid=" + uid + ";pwd=" + pwd + "");
                    try
                    {
                        ocn.Open();
                        SqlCommand ocmd = new SqlCommand();
                        ocmd.Connection = ocn;
                        ocmd.CommandText = ProcName;
                        ocmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter[] ORAPAR = { 
                        new SqlParameter("ApplyNo",SqlDbType.VarChar, 50),               //0  HIS ������뵥��ˮ��
                        new SqlParameter("AccessionNo", SqlDbType.VarChar, 20),          //1   ���ţ���Ӱ��ϵͳ�ڲ����ţ�
                        new SqlParameter("PatientID", SqlDbType.VarChar, 30),            //2   ����Ψһ��������
                        new SqlParameter("PatientType", SqlDbType.VarChar, 20),           //3  �������ͣ�I��סԺ��O�����E�����
                        new SqlParameter("InHospitalNo", SqlDbType.VarChar, 200),          //4  סԺ�ţ�סԺ���߱��
                        new SqlParameter("ClinicNo", SqlDbType.VarChar, 20),              //5  ����ţ��ż��ﻼ�߱��
                        new SqlParameter("PatientName", SqlDbType.VarChar,30),            //6   ��������
                        new SqlParameter("Gender", SqlDbType.VarChar, 20),                  //7 �Ա�M���У�F��Ů��O��������U��δ֪��
                        new SqlParameter("BirthDate", SqlDbType.Date),                 //8  �������ڣ���ʽΪ YYYY-MM-DD
                        new SqlParameter("IdCard", SqlDbType.VarChar, 50),                   //9    ���֤��
                        new SqlParameter("ContactPhone", SqlDbType.VarChar, 80),           //10   ��ϵ�绰
                        new SqlParameter("ContactAddress", SqlDbType.VarChar,500),        //11   ��ϵ��ַ
                        new SqlParameter("InHospitalRegion",SqlDbType.VarChar,50),      //12    ��������סԺ���ߣ�
                        new SqlParameter("BedNo", SqlDbType.VarChar,50),             //13    ���ţ���סԺ���ߣ�
                        new SqlParameter("ModalityType", SqlDbType.VarChar, 100),          //14   ������ͣ�DX/CT/MR/US/ES/PS/NM �ȣ�
                        new SqlParameter("ApplyDepart", SqlDbType.VarChar, 100),         //15    �����������
                        new SqlParameter("ApplyDoctor", SqlDbType.VarChar, 20),        //16       ����ҽ������
                        new SqlParameter("ApplyDateTime", SqlDbType.DateTime),            //17     ��������ʱ��(YYYY-MM-DD HH:mm:SS)
                        new SqlParameter("ExamineDatetime", SqlDbType.DateTime),           //18      �������ʱ��(YYYY-MM-DD HH:mm:SS)
                        new SqlParameter("ProcedureCode", SqlDbType.VarChar, 20),             //19  ��鲿λ����
                        new SqlParameter("ProcedureDesc", SqlDbType.VarChar, 50),           //20       ��鲿λ����
                        new SqlParameter("HasImage", SqlDbType.Int),               //21    �Ƿ���ͼ��1����ͼ��0����ͼ��
                        new SqlParameter("HasReport", SqlDbType.Int),             //22   �Ƿ��б��棨1���б��棻0���ޱ��棩 
                        new SqlParameter("ReportSee", SqlDbType.VarChar,1000),         //23   �������
                        new SqlParameter("ReportGet", SqlDbType.VarChar, 1000),         //24    �������
                        new SqlParameter("ReportDatetime", SqlDbType.DateTime),       //25  ��������ʱ��(YYYY-MM-DD HH:mm:SS)
                        new SqlParameter("Reporter", SqlDbType.VarChar, 20),          //26    ����ҽ��
                        new SqlParameter("Status", SqlDbType.Int),                  //27    ״̬����ˣ�1���޸ģ�0��
                        new SqlParameter("CardId", SqlDbType.VarChar, 20)           //28    ���ƿ���
                       // new SqlParameter("ReportGUID", SqlDbType.VarChar, 20)            //29    ���ƿ���
                        };

                        ORAPAR[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();
                        ORAPAR[1].Value = bljc.Rows[0]["F_BLH"].ToString().Trim();
                        ORAPAR[2].Value = bljc.Rows[0]["F_brbh"].ToString().Trim();
                        string f_jzlx = "";
                        if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                        {
                            f_jzlx = "I";
                        }
                        else if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "����")
                        { f_jzlx = "O"; }
                        else if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "����")
                        { f_jzlx = "E"; }
                        ORAPAR[3].Value = f_jzlx;

                        ORAPAR[4].Value = bljc.Rows[0]["F_zyh"].ToString().Trim();
                        ORAPAR[5].Value = bljc.Rows[0]["F_mzH"].ToString().Trim();
                        ORAPAR[6].Value = bljc.Rows[0]["F_xm"].ToString().Trim();
                        string f_gener = "";
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "��")
                        {
                            f_gener = "M";
                        }
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "Ů")
                        {
                            f_gener = "F";
                        }
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "����")
                        {
                            f_gener = "O";
                        }
                        if (bljc.Rows[0]["F_xb"].ToString().Trim() == "δ֪")
                        {
                            f_gener = "U";
                        }
                        ORAPAR[7].Value = f_gener; //7 �Ա�M���У�F��Ů��O��������U��δ֪��
                        try
                        {
                            ORAPAR[8].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["F_BY2"].ToString().Trim()).ToString("yyyy-MM-dd"));
                        }
                        catch (Exception)
                        {

                            ORAPAR[8].Value = DBNull.Value;
                        }
                        ORAPAR[9].Value = bljc.Rows[0]["F_sfzh"].ToString().Trim();
                        ORAPAR[10].Value = bljc.Rows[0]["F_lxxx"].ToString().Trim();
                        ORAPAR[11].Value = bljc.Rows[0]["F_lxxx"].ToString();
                        ORAPAR[12].Value = bljc.Rows[0]["F_bq"].ToString().Trim();
                        ORAPAR[13].Value = bljc.Rows[0]["F_ch"].ToString().Trim();
                        ORAPAR[14].Value = "PIS";
                        ORAPAR[15].Value = bljc.Rows[0]["F_sjks"].ToString().Trim();
                        ORAPAR[16].Value = bljc.Rows[0]["F_sjys"].ToString().Trim();
                        try
                        {
                            ORAPAR[17].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception)
                        {
                            ORAPAR[17].Value = DBNull.Value;
                        }

                        try
                        {
                            ORAPAR[18].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception)
                        {
                            ORAPAR[18].Value = DBNull.Value;
                        }
                        ORAPAR[19].Value = "";
                        ORAPAR[20].Value = "������";
                        if (txlb.Rows.Count > 0)
                        {
                            ORAPAR[21].Value = 1;
                        }
                        else ORAPAR[21].Value = 0;
                        if (bljc.Rows[0]["F_bgzt"].ToString().Trim().ToString() == "�����")
                        {
                            ORAPAR[22].Value = 1;
                        }
                        else
                        {
                            ORAPAR[22].Value = 0;
                        }
                        ORAPAR[23].Value = bljc.Rows[0]["f_rysj"].ToString().Trim().ToString();
                        ORAPAR[24].Value = bljc.Rows[0]["f_blzd"].ToString().Trim().ToString();
                        try
                        {
                            ORAPAR[25].Value = Convert.ToDateTime(Convert.ToDateTime(bljc.Rows[0]["f_bgrq"].ToString().ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception)
                        {
                            ORAPAR[25].Value = DBNull.Value;
                        }
                        ORAPAR[26].Value = bljc.Rows[0]["f_bgys"].ToString().ToString().Trim();
                        if (bljc.Rows[0]["F_bgzt"].ToString().Trim().ToString() == "�����")
                        {
                            ORAPAR[27].Value = 1;
                        }
                        else
                        {
                            ORAPAR[27].Value = 0;
                        }
                        ORAPAR[28].Value = bljc.Rows[0]["F_YZID"].ToString().Trim();
                       // ORAPAR[29].Value = "";



                        ORAPAR[0].Direction = ParameterDirection.Input;
                        ORAPAR[1].Direction = ParameterDirection.Input;
                        ORAPAR[2].Direction = ParameterDirection.Input;
                        ORAPAR[3].Direction = ParameterDirection.Input;
                        ORAPAR[4].Direction = ParameterDirection.Input;
                        ORAPAR[5].Direction = ParameterDirection.Input;
                        ORAPAR[6].Direction = ParameterDirection.Input;
                        ORAPAR[7].Direction = ParameterDirection.Input;
                        ORAPAR[8].Direction = ParameterDirection.Input;
                        ORAPAR[9].Direction = ParameterDirection.Input;
                        ORAPAR[10].Direction = ParameterDirection.Input;
                        ORAPAR[11].Direction = ParameterDirection.Input;
                        ORAPAR[12].Direction = ParameterDirection.Input;
                        ORAPAR[13].Direction = ParameterDirection.Input;
                        ORAPAR[14].Direction = ParameterDirection.Input;
                        ORAPAR[15].Direction = ParameterDirection.Input;
                        ORAPAR[16].Direction = ParameterDirection.Input;
                        ORAPAR[17].Direction = ParameterDirection.Input;
                        ORAPAR[18].Direction = ParameterDirection.Input;
                        ORAPAR[19].Direction = ParameterDirection.Input;
                        ORAPAR[20].Direction = ParameterDirection.Input;
                        ORAPAR[21].Direction = ParameterDirection.Input;
                        ORAPAR[22].Direction = ParameterDirection.Input;
                        ORAPAR[23].Direction = ParameterDirection.Input;
                        ORAPAR[24].Direction = ParameterDirection.Input;
                        ORAPAR[25].Direction = ParameterDirection.Input;
                        ORAPAR[26].Direction = ParameterDirection.Input;
                        ORAPAR[27].Direction = ParameterDirection.Input;
                        ORAPAR[28].Direction = ParameterDirection.Input;


                        StringBuilder sb = new StringBuilder();
                        foreach (SqlParameter parameter in ORAPAR)
                        {
                            ocmd.Parameters.Add(parameter);
                            sb.Append(parameter.ToString() + ":" + parameter.Value.ToString() + ",");
                        }
                        if (debug == "1")
                            log.WriteMyLog(blh + sb.ToString());
                        try
                        {
                            int i = 0;
                            if (ocn.State != ConnectionState.Open)
                            {
                                ocn.Open();
                            }
                            i = ocmd.ExecuteNonQuery();//ִ�д洢����    
                            if (i > 0)
                            {
                                ocn.Close();
                                if (debug == "1")
                                    log.WriteMyLog(blh + ",ִ��PACS�洢���̳ɹ���");
                            }
                            else
                            {
                                ocn.Close();
                                log.WriteMyLog(blh + " ִ��PACS�洢����ʧ�ܣ�");
                                return;
                            }

                        }
                        catch (Exception e)
                        {
                            ocn.Close();
                            log.WriteMyLog(blh + ",ִ�д洢�����쳣��" + e.Message.ToString() + "");
                            return;
                        }
                    }
                    catch (Exception ee)
                    {
                        ocn.Close();
                        log.WriteMyLog(blh + ",ִ�д洢�����쳣2��" + ee.Message);
                        return;
                    }
                    finally
                    {
                        if (ocn.State == ConnectionState.Open)
                            ocn.Close();
                    }

                    #endregion

      
                    string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                    string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                    string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                    string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                    string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                    string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                    FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                    string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
                    string stxsm = "";

                    #region ����ͼ��
                    if (!Directory.Exists(@"c:\temp"))
                    {
                        Directory.CreateDirectory(@"c:\temp");
                    }
                    if (!Directory.Exists(@"c:\temp\" + blh))
                    {
                        Directory.CreateDirectory(@"c:\temp\" + blh);
                    }
 
                    string xzfs = f.ReadString("savetohis", "xzfs", "ftp");//ͼ�����ط�ʽ
                    if (xzfs == "ftp")
                    {
                        if (ftps == "1")
                        {
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                stxsm = stxsm + txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                                string ftpstatus = "";
                                try
                                {
                                    fw.Download(ftplocal + "\\" + blh, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                }
                                catch (Exception ee)
                                {
                                    log.WriteMyLog("����ͼ��ʧ�ܣ�����" + ee.Message);
                                }
                                if (ftpstatus == "Error")
                                {
                                    log.WriteMyLog("����ͼ��ʧ�ܣ�����" + ftpstatus + "�ٲ���һ��");
                                    fw.Download(ftplocal + "\\" + blh, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (txpath == "")
                        {
                            log.WriteMyLog("sz.ini txpathͼ��Ŀ¼δ����");
                            return;
                        }
                        for (int i = 0; i < txlb.Rows.Count; i++)
                        {
                            try
                            {
                                File.Copy(@"" + txpath + "" + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + blh + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                            }
                            catch
                            {
                                log.WriteMyLog("����Ŀ¼�����ڣ�");
                                return;
                            }
                        }
                    }
                    if (debug == "1")
                        log.WriteMyLog("������ɣ�����");
                 
                    #endregion

                    #region ����XML�ļ�
             
                    StringBuilder inxmlSB = new StringBuilder(255);
                    try
                    {
                        inxmlSB.AppendLine("<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "UTF-8" + (char)34 + "?>");
                        inxmlSB.AppendLine("<document_info>");
                        inxmlSB.AppendLine("<patient_info>");
                        inxmlSB.AppendLine("<global_patientid>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</global_patientid>");
                        inxmlSB.AppendLine("<patientid>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</patientid>");
                        string patientname_en = System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yl4"].ToString().Trim());
                       //תƴ��ȫƴ������ɾ�����ⲿ������DLL

                        inxmlSB.AppendLine("<patientname_en>" + patientname_en + "</patientname_en>");
                        inxmlSB.AppendLine("<patientname_ch>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xm"].ToString().Trim()) + "</patientname_ch>");
                        inxmlSB.AppendLine("<usedname>" + "" + "</usedname>");
                        string gender = "";
                        switch (bljc.Rows[0]["F_xb"].ToString().Trim())
                        {
                            case "��": gender = "M"; break;
                            case "Ů": gender = "F"; break;
                            case "����": gender = "O"; break;
                            case "δ֪": gender = "U"; break;
                            default:
                                break;
                        }
                        inxmlSB.AppendLine("<gender>" + System.Security.SecurityElement.Escape(gender) + "</gender> ");
                        string bgrqxml = "";
                        try
                        {
                            bgrqxml = Convert.ToDateTime(bljc.Rows[0]["F_by2"].ToString().Trim()).ToString("YYYYMMDD");
                        }
                        catch (Exception)
                        {
                            bgrqxml = "";
                        }
                        inxmlSB.AppendLine("<birthday>" + System.Security.SecurityElement.Escape(bgrqxml) + "</birthday>");
                        inxmlSB.AppendLine("</patient_info>");
                        inxmlSB.AppendLine("<order_info>");
                        inxmlSB.AppendLine("<accession_number>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLH"].ToString().Trim()) + "</accession_number>");
                        inxmlSB.AppendLine("</order_info>");
                        inxmlSB.AppendLine("<report_info>");
                        inxmlSB.AppendLine("<piclist_count>" + System.Security.SecurityElement.Escape(txlb.Rows.Count.ToString()) + "</piclist_count>");
                        if (txlb.Rows.Count > 0)
                        {
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                inxmlSB.AppendLine("<piclist" + (i + 1) + ">");
                                inxmlSB.AppendLine("<document_name>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_txm"].ToString().Trim()) + "</document_name>");
                                inxmlSB.AppendLine("<document_type>" + "JPG" + "</document_type>");
                                inxmlSB.AppendLine("<document_datetime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + "</document_datetime>");
                                inxmlSB.AppendLine("<document_comment>" + "" + "</document_comment>");
                                inxmlSB.AppendLine("</piclist" + (i + 1) + ">");
                            }
                        }
                        inxmlSB.AppendLine("</report_info>");
                        inxmlSB.AppendLine("</document_info>");
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("����XML���쳣��"+ee.Message);
                        return;
                    }

                    try
                    {
                        if (debug == "1")
                            log.WriteMyLog("д��xml��Ϊ��" + inxmlSB.ToString());

                        string filePath = ftplocal + "\\" + blh + "\\" + blh + ".xml";
                        if (!System.IO.Directory.Exists(ftplocal + "\\" + blh))//����ļ��в�����
                        {
                            Directory.CreateDirectory(ftplocal + "\\" + blh);
                        }
                        if (!File.Exists(filePath))//����ļ������� 
                        {
                            File.Create(filePath).Close();
                        }
                        File.WriteAllText(filePath, "");
                        StreamWriter swx = File.AppendText(filePath);
                        swx.WriteLine(inxmlSB.ToString());
                        swx.Close();
                        if (debug == "1")
                            log.WriteMyLog("xml�ļ�������ɣ�����");
                    }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog("xml�ļ�����ʧ�ܣ�"+ee2.Message);
                        return;
                    }
                    #endregion

                    #region ��ס����Ŀ¼���û���������

                    string gxuid = f.ReadString("savetohis", "gxuid", "administrator").Replace("\0", "");
                    string gxpwd = f.ReadString("savetohis", "gxpwd", "1qaz!QAZ").Replace("\0", "");
                    System.Diagnostics.Process prc = new Process();
                    prc.StartInfo.FileName = @"cmd.exe";
                    prc.StartInfo.UseShellExecute = false;
                    prc.StartInfo.RedirectStandardInput = true;
                    prc.StartInfo.RedirectStandardOutput = true;
                    prc.StartInfo.RedirectStandardError = true;
                    prc.StartInfo.CreateNoWindow = true;

                    prc.Start();
                    //string dos_cmd = "net use " + gxml + " /del";
                    //prc.StandardInput.WriteLine(dos_cmd);
                    string dos_cmd = "net use " + pacspath + " " + gxpwd + " /user:" + gxuid;
                    prc.StandardInput.WriteLine(dos_cmd);
                    prc.StandardInput.Close();

                    string output = prc.StandardOutput.ReadToEnd();

                    //MessageBox.Show(output);
                    if (output.IndexOf("����ɹ����") > 0)
                    {
                        log.WriteMyLog("���ӹ���ͼ��������ɹ���");
                    }
                    else
                    {
                        log.WriteMyLog("���ӹ���ͼ�������ʧ�ܣ�");

                    }

                    #endregion
                    #region �ϴ�����ͼ��PACS  
                    
                    try
                    {
                        copydir.copyDirectory(@"" + ftplocal + "" + "\\" + blh, @"" + pacspath + "");

                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("�ϴ��ļ�ʧ�ܣ�����" + ee.Message);
                        return;
                    }
                    if (debug == "1")
                        log.WriteMyLog("�ϴ��ļ��ɹ�");

                    #endregion

                    aa.ExecuteSQL("update t_jcxx set f_topacs='1'   where f_blh='" + blh + "'");

                }
                else
                {
                    if (bljc.Rows[0]["f_topacs"].ToString() == "1")
                    {
                        Directory.Delete(@"" + pacspath + "\\"+blh+"\\", true);
                        aa.ExecuteSQL("update t_jcxx set f_topacs='0'   where f_blh='" + blh + "'");

                    }
                    log.WriteMyLog("ɾ���ļ��ɹ���");
                }
                aa.ExecuteSQL("update t_jcxx_fs  set f_fszt='�Ѵ���' where f_blh='" + blh + "'");
        }

        public string getyhgh(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
        private string FormatXml(string sUnformattedXml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

   
    }
}
