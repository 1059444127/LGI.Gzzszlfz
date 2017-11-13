using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using IBM.WMQ;
using PathHISZGQJK;
using System.Xml;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
   /// <summary>
   /// ��ɽ��ѧ�ۿ�ҽԺ
   /// ��˺���ٴ�ҽԺ�����ţ�����ҽ�������˱��������
   /// ͨ��������oaϵͳ��ȡ�绰���뷢��
   /// </summary>
    class zsdxykyy
    {
       private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

       string odbc_fsdx = "";
       string odbc2his = "";
    
    
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
       
             string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
             string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
              debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            
              string ksdm = f.ReadString("savetohis", "KSDM", "70500").Trim();
              string ptjk = ZgqClass.GetSz("zgqjk", "ptjk", "0").Replace("\0", "").Trim();
              string hisjk = ZgqClass.GetSz("zgqjk", "hisjk", "0").Replace("\0", "").Trim();
              string hczt = ZgqClass.GetSz("zgqjk", "hczt", "0").Replace("\0", "").Trim();
              string hcbg = ZgqClass.GetSz("zgqjk", "hcbg", "0").Replace("\0", "").Trim();
              string tzdx = ZgqClass.GetSz("savetohis", "tzdx", "1").Replace("\0", "").Trim();
              string fsdx = ZgqClass.GetSz("savetohis", "fsdx", "1").Replace("\0", "").Trim();
                
            //֪ͨ�������ݿ�
              odbc_fsdx = f.ReadString("savetohis", "odbc_fsdx", "Data Source=192.168.168.155;Initial Catalog=iOffice;User Id=ywkbb;Password=123456;").Replace("\0", "").Trim();
            //his���ݿ�
             odbc2his = f.ReadString("savetohis", "odbc2his", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();

             dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
                  bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

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
                  if (fsdx == "1")
                  {
                      #region  ���Ͷ���
                      if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "�����")
                      {
                          if (bljc.Rows[0]["F_BRLB"].ToString().Trim() != "סԺ")
                          {
                              log.WriteMyLog("��סԺ����,�����Ͷ���");
                          }
                          else
                          {
                              //��˺���
                              string err = "";
                              string sjys = bljc.Rows[0]["F_SJYS"].ToString().Trim();
                              string sjysid = bljc.Rows[0]["F_BY2"].ToString().Trim();
                              string sjhm = "";
                              if (sjys != "" && sjysid != "")
                              {
                                  sjhm = getMobile(sjysid, ref err);
                                  sjhm = sjhm.Trim();

                                  if (sjhm.Trim() == "")
                                  {
                                      log.WriteMyLog("��ȡҽ���ֻ�����ʧ��" + err);
                                  }
                                  else
                                  {
                                      if (sjhm.Length == 11)
                                      {
                                          try
                                          {
                                              //int sendto = int.Parse(sjhm);
                                      
                                              string fsnr = ZgqClass.GetSz("fsdx", "fsnr", "");
                                              if (fsnr.Trim() == "")
                                                  fsnr = "�𾴵�{�ͼ�ҽ��}ҽ��������:{����},{�Ա�},{����},סԺ��:{סԺ��},����{����},�˲��˵Ĳ��������Ѿ������ˣ��뼰ʱ���ġ�������ơ�";
                                              fsnr = fsnr.Replace("{�ͼ�ҽ��}", bljc.Rows[0]["F_SJYS"].ToString().Trim());
                                              fsnr = fsnr.Replace("{����}", bljc.Rows[0]["F_XM"].ToString().Trim());
                                              fsnr = fsnr.Replace("{�Ա�}", bljc.Rows[0]["F_XB"].ToString().Trim());
                                              fsnr = fsnr.Replace("{����}", bljc.Rows[0]["F_NL"].ToString().Trim());
                                              fsnr = fsnr.Replace("{סԺ��}", bljc.Rows[0]["F_ZYH"].ToString().Trim());
                                              fsnr = fsnr.Replace("{����}", bljc.Rows[0]["F_CH"].ToString().Trim());
                                              fsnr = fsnr.Replace("{�ͼ����}", bljc.Rows[0]["F_SJKS"].ToString().Trim());
                                              fsnr = fsnr.Replace("{�����}", bljc.Rows[0]["F_MZH"].ToString().Trim());
                                              fsnr = fsnr.Replace("{����ҽ��}", bljc.Rows[0]["F_BGYS"].ToString().Trim());
                                              fsnr = fsnr.Replace("{���ҽ��}", bljc.Rows[0]["F_SHYS"].ToString().Trim());
                                              err = "";
                                              bool rtnbool = MessageSend(sjhm.Trim(), fsnr, sjysid, sjys, ref  err);
                                              if (rtnbool)
                                              {
                                                  aa.ExecuteSQL("insert into T_FSDX(F_BLH,F_CZY,F_FSSJ,F_FSNR,F_SJHM,F_JSR) values('" + blh + "','" + yhmc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fsnr.Trim() + "','" + sjhm.ToString().Trim() + "','" + sjys + "')");
                                              }
                                              else
                                              {
                                                  log.WriteMyLog(err);
                                              }
                                          }
                                          catch (Exception ee2)
                                          {
                                              log.WriteMyLog(ee2.Message);
                                          }
                                      }
                                      else
                                      {
                                          log.WriteMyLog("���Ͷ���:ҽ���ֻ������ʽ����ȷ" + sjhm);
                                      }
                                  }
                              }
                              else
                              {
                                  log.WriteMyLog("���Ͷ���:�ͼ�ҽ����ҽ��IDΪ�ղ�����");
                              }
                          }
                      }
                      #endregion
                  }

                 


            string  sqxh=bljc.Rows[0]["F_SQXH"].ToString().Trim();
            if(sqxh!="")
            {
               #region ƽ̨�ӿ�
                if (ptjk == "1")
                {
                    ///ƽ̨�ӿ�
                    ///
                    DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + sqxh + "'", "t_sqd");
                    if (dt_sqd.Rows.Count < 1)
                    {
                       log.WriteMyLog("����ƺ�:" + blh + ";T_SQD�����޼�¼,������");

                    }
                    else
                    {
                        if (dt_sqd.Rows[0]["F_SQDZT"].ToString() != "�ѵǼ�")
                        {

                            aa.ExecuteSQL("update T_SQD set F_SQDZT='�ѵǼ�' where F_sqxh='" + sqxh + "'");
                        }


                        string brlb = bljc.Rows[0]["F_BRLB"].ToString().Trim();
                        string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
                        #region   �ش�״̬
                        if (cslb[4].ToLower() == "qxsh")
                            bgzt = "ȡ�����";
                        if (hczt == "1")
                        {

                            ZtToPt(bljc, dt_sqd, blh, bglx, bgxh, bgzt, brlb, yhmc, yhbh, debug);
                        }
                        #endregion

                        #region  �ش�����
                        if (hcbg == "1")
                        {
                            if (bgzt == "�����")
                            {
                                BgToPt(bljc, dt_sqd, blh, bglx, bgxh, brlb, debug);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

               #region HIS�ӿ�
               if (hisjk == "1" )
               {

                   if (bglx == "cg")
                   {

                       ///HIS�ӿ�
                       #region  ȷ��HIS���뵥
                       if ((bljc.Rows[0]["F_HISBJ"].ToString().Trim() != "1" && bljc.Rows[0]["F_HISBJ"].ToString().Trim() != "2"))
                       {
                           ZtToHis(blh, sqxh, yhbh, debug);
                       }
                       #endregion
                   }

             
                  
                        if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
                        {
                            if (hcbg == "1")
                            {
                                #region  �ش�����
                                DataTable bljc_bc = new DataTable();
                                bljc_bc = aa.GetDataTable("select * from T_bcbg where F_BLH='" + blh + "' and F_BC_BGZT='�����'", "bcbg");
                                string blzd = bljc.Rows[0]["F_blzd"].ToString().Trim() + "\r\n";

                                ///���䱨��
                                string str_bcbg = "";
                           
                                try
                                {
                                    for (int z = 0; z < bljc_bc.Rows.Count; z++)
                                    {
                                        str_bcbg = str_bcbg + ("���䱨��" + bljc_bc.Rows[z]["F_BC_BGXH"].ToString() + ":" + bljc_bc.Rows[z]["F_BCZD"].ToString() + "\r\n");
                                    }
                                }
                                catch
                                {
                                }

                                string xml = "";
                                try
                                {

                                    #region XML
                                    xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                                    xml = xml + "<ArrayOfExamReported_Result>";
                                    xml = xml + "<ExamReported_Result>";
                                    xml = xml + "<ReportII>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportII>";
                                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                                        xml = xml + "<EncounterII>" + "1_" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";//0_m,1_z,
                                    else
                                        xml = xml + "<EncounterII>" + "0_" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";//0_m,1_z,

                                    xml = xml + "<OrderIIs>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</OrderIIs>";
                                    xml = xml + "<ExamineNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ExamineNo>";
                                    xml = xml + "<ClinicItemII></ClinicItemII>";
                                    xml = xml + "<ExamineName>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</ExamineName>";
                                    xml = xml + "<ReportTitle>����" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "��鱨�浥</ReportTitle>";
                                    xml = xml + "<BodyParts>" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "</BodyParts>";

                                    string rysj = bljc.Rows[0]["F_RYSJ"].ToString().Trim();
                                    if (rysj.Trim() != "")
                                        rysj = ("����������" + rysj + " \r\n ").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                                    string jxsj = bljc.Rows[0]["F_JXSJ"].ToString().Trim();
                                    if (jxsj.Trim() != "")
                                        jxsj = ("����������" + jxsj + " \r\n ").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                                    string tsjc = bljc.Rows[0]["F_TSJC"].ToString().Trim();
                                    if (tsjc.Trim() != "")
                                        tsjc = ("�����飺" + tsjc + "\r\n").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");


                                    xml = xml + "<ReportDesc>" + rysj + jxsj + "</ReportDesc>";
                                    xml = xml + "<ReportConclusion>" + tsjc + ("������ϣ�" + blzd + str_bcbg).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</ReportConclusion>";
                                    xml = xml + "<DiagnoseKindCode></DiagnoseKindCode>";
                                    xml = xml + "<DiagnoseKindCodeSystem></DiagnoseKindCodeSystem>";
                                    xml = xml + "<DiagnoseKindCodeName></DiagnoseKindCodeName>";
                                    xml = xml + "<DiagnoseDesc></DiagnoseDesc>";
                                    xml = xml + "<DiseaseII></DiseaseII>";
                                    xml = xml + "<ICDCode></ICDCode>";
                                    xml = xml + "<DiseaseName></DiseaseName>";
                                    xml = xml + "<DicomAccessNum></DicomAccessNum>";
                                    xml = xml + "<DicomModality></DicomModality>";
                                    xml = xml + "<DiagnosisMethod></DiagnosisMethod>";
                                    xml = xml + "<RepeatNumber></RepeatNumber>";
                                    xml = xml + "<MachineRoomName></MachineRoomName>";
                                    xml = xml + "<DeviceName></DeviceName>";
                                    xml = xml + "<DicomStudyUid></DicomStudyUid>";
                                    xml = xml + "<ExamineEmployeeII>" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</ExamineEmployeeII>";
                                    try
                                    {
                                        xml = xml + "<ExamineOn>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</ExamineOn>";
                                    }
                                    catch (Exception ee2)
                                    {
                                        xml = xml + "<ExamineOn>" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "</ExamineOn>";
                                    }
                                    xml = xml + "<AuditEmployeeII>" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</AuditEmployeeII>";
                                    xml = xml + "<AuditOn>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</AuditOn>";
                                    try
                                    {
                                        xml = xml + "<ReportOn>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</ReportOn>";
                                    }
                                    catch (Exception ee2)
                                    {
                                        xml = xml + "<ReportOn>" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "</ReportOn>";
                                    }
                                    xml = xml + "<ReportNo></ReportNo>";
                                    xml = xml + "<EffectiveTime></EffectiveTime>";
                                    xml = xml + "<RowVersion>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</RowVersion>";
                                    xml = xml + "</ExamReported_Result>";
                                    xml = xml + "</ArrayOfExamReported_Result>";

                                    if (debug == "1")
                                        log.WriteMyLog("�ش�XML��" + xml);
                                    #endregion
                                }
                                catch (Exception e4)
                                {
                                    log.WriteMyLog("����ţ�" + blh + ",�ӿ�ƴ��XML�쳣" + e4.Message.ToString());
                                    return;
                                }

                                if (xml.Trim() != "")
                                    BgToHis(blh, bljc.Rows[0]["F_BRBH"].ToString().Trim(), xml, debug, "EVT.Exam.Reported", "����������¼�֪ͨ", ksdm);

                                #endregion
                            }
                        }
                        else
                        {

                           # region ȡ�����
                            if (bljc.Rows[0]["F_HISBJ"].ToString().Trim() == "2" && bljc.Rows[0]["F_BGZT"].ToString().Trim()=="��д����")
                            {
                                //��������
                                string xml = "";
                                try
                                {
                                     xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                                    xml = xml + "<ArrayOfExamReported_Result>";
                                    xml = xml + "<ExamReported_Result>";
                                    xml = xml + "<ReportII>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportII>";
                                    xml = xml + "<EncounterII>" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";
                                    xml = xml + "<OrderIIs>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</OrderIIs>";
                                    xml = xml + "<ExamineNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ExamineNo>";
                                    xml = xml + "<ClinicItemII></ClinicItemII>";
                                    xml = xml + "<ExamineName>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</ExamineName>";
                                    xml = xml + "<ReportTitle>����" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "��鱨�浥</ReportTitle>";
                                    xml = xml + "<BodyParts>" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "</BodyParts>";
                                    xml = xml + "<ReportDesc>" + "" + "</ReportDesc>";
                                    xml = xml + "<ReportConclusion>" + "" + "</ReportConclusion>";
                                    xml = xml + "<DiagnoseKindCode></DiagnoseKindCode>";
                                    xml = xml + "<DiagnoseKindCodeSystem></DiagnoseKindCodeSystem>";
                                    xml = xml + "<DiagnoseKindCodeName></DiagnoseKindCodeName>";
                                    xml = xml + "<DiagnoseDesc></DiagnoseDesc>";
                                    xml = xml + "<DiseaseII></DiseaseII>";
                                    xml = xml + "<ICDCode></ICDCode>";
                                    xml = xml + "<DiseaseName></DiseaseName>";
                                    xml = xml + "<DicomAccessNum></DicomAccessNum>";
                                    xml = xml + "<DicomModality></DicomModality>";
                                    xml = xml + "<DiagnosisMethod></DiagnosisMethod>";
                                    xml = xml + "<RepeatNumber></RepeatNumber>";
                                    xml = xml + "<MachineRoomName></MachineRoomName>";
                                    xml = xml + "<DeviceName></DeviceName>";
                                    xml = xml + "<DicomStudyUid></DicomStudyUid>";
                                    xml = xml + "<ExamineEmployeeII>" + "" + "</ExamineEmployeeII>";
                                    xml = xml + "<ExamineOn>" + "" + "</ExamineOn>";
                                    xml = xml + "<AuditEmployeeII>" + "" + "</AuditEmployeeII>";
                                    xml = xml + "<AuditOn>" + "" + "</AuditOn>";
                                    xml = xml + "<ReportOn>" + "" + "</ReportOn>";
                                    xml = xml + "<ReportNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportNo>";
                                    xml = xml + "<EffectiveTime></EffectiveTime>";
                                    xml = xml + "<RowVersion>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "</RowVersion>";
                                    xml = xml + "</ExamReported_Result>";
                                    xml = xml + "</ArrayOfExamReported_Result>";

                                      if (debug == "1")
                                    log.WriteMyLog("ȡ������XML��" + xml);
                               
                            }
                            catch (Exception e4)
                            {
                                log.WriteMyLog("����ţ�" + blh + ",ȡ������XML�쳣" + e4.Message.ToString());
                                return;
                            }

                                if(xml.Trim()!="")
                             BgToHis(blh, bljc.Rows[0]["F_BRBH"].ToString().Trim(), xml, debug, "EVT.Exam.ReportCanceled", "ȡ������������¼�֪ͨ", ksdm);
                               
                                
                             }
                           #endregion
                        }

                }
               #endregion
             }
         
                     
         }

        //���Ͷ���
        public bool MessageSend(string sendto, string content, string empid, string sendtoname, ref string err)
        {

            SqlDB sqlcon = new SqlDB();
            err = "";
            int x = sqlcon.ExecuteNonQuery(odbc_fsdx, "insert into iOffice.dbo.ifMobileInf(sendto,content,instamp,empid,sendtoname) values('" + sendto + "','" + content + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + empid + "','" + sendtoname + "')", ref err);
            if (x > 0)
                return true;
            else
                return false;

        }
        public string getMobile(string userID, ref string err)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            SqlDB sqlcon = new SqlDB();
            err = "";
            DataTable dt = new DataTable();
            dt = sqlcon.DataAdapter(odbc_fsdx, "select  Mobile from  iOffice.dbo.mrbaseinf  where  LoginID='" + userID + "'", ref err);

            if (dt == null)
            {
                return "";

            }
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["Mobile"].ToString();
            else
                return "";

        }

        //HIS
        public void  ZtToHis(string blh,string sqxh,string yhgh,string  debug )
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                       try
                       {
                           int.Parse(sqxh);
                       }
                       catch
                       {
                           log.WriteMyLog("������Ÿ�ʽ����ȷ");
                           return;
                       }

                       try
                       {
                       SqlParameter[] sqlPt = new SqlParameter[3];
                       for (int j = 0; j < sqlPt.Length; j++)
                       {
                           sqlPt[j] = new SqlParameter();
                       }
                       //���뵥ID
                       sqlPt[0].ParameterName = "In_FunctionRequestID";
                       sqlPt[0].SqlDbType = SqlDbType.Int;
                       sqlPt[0].Direction = ParameterDirection.Input;
                       sqlPt[0].Value = int.Parse(sqxh);

                       //����Ա������
                       sqlPt[1].ParameterName = "In_OperatorEmployeeNo";
                       sqlPt[1].SqlDbType = SqlDbType.NVarChar;
                       sqlPt[1].Direction = ParameterDirection.Input;
                       sqlPt[1].Size = 10;
                       sqlPt[1].Value =yhgh ;
                       //ȡ����־
                       sqlPt[2].ParameterName = "Out_StatusFlag";
                       sqlPt[2].SqlDbType = SqlDbType.TinyInt;
                       sqlPt[2].Direction = ParameterDirection.Output;
                       sqlPt[2].Value = 0;

                       string err_msg = "";
                       SqlDB db = new SqlDB();
                       db.ExecuteNonQuery(odbc2his, "pConfirmFunctionRequest", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
                       if (int.Parse(sqlPt[2].Value.ToString()) == 1)
                       {   aa.ExecuteSQL("update  T_JCXX  set F_HISBJ='1'  where F_BLH='"+blh+"'");
                       log.WriteMyLog("ȷ�����뵥�ɹ�");
                           if(debug=="1")
                           MessageBox.Show("ȷ��ȷ�����뵥�ɹ�");
                       }
                       else
                       {
                           aa.ExecuteSQL("update  T_JCXX  set F_HISBJ='-1'  where F_BLH='" + blh + "'");
                            log.WriteMyLog("ȷ�����뵥ʧ��:"+sqlPt[2].Value.ToString() + "��"+err_msg);return;
                           if(debug=="1")
                               MessageBox.Show(sqlPt[2].Value.ToString() + "��" + err_msg);
                       }
                   }
                   catch(Exception  ee1)
                   {
                       log.WriteMyLog("ȷ�����뵥ʧ�ܣ��쳣��"+ee1.Message);return;
                   }
                   }
        public void  BgToHis(string  blh,string brbh,string XML,string  debug,string EventName, string Description, string OrganizationII)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string bglx = "cg";
            string bgxh = "0";
           try
           {
               SqlParameter[] sqlPt = new SqlParameter[10];
               for (int j = 0; j < sqlPt.Length; j++)
               {
                   sqlPt[j] = new SqlParameter();
               }

             
               //�¼���ʶ��
               sqlPt[0].ParameterName = "in_EventII";
               sqlPt[0].SqlDbType = SqlDbType.NVarChar;
               sqlPt[0].Direction = ParameterDirection.Input;
               sqlPt[0].Size=128;
               sqlPt[0].Value = "Logene.Pathology^" + blh;
              
               //�¼�����
               sqlPt[1].ParameterName = "in_EventName";
               sqlPt[1].SqlDbType = SqlDbType.NVarChar;
               sqlPt[1].Direction = ParameterDirection.Input;
               sqlPt[1].Size=128;
               sqlPt[1].Value = EventName;
             
                //����������
               sqlPt[2].ParameterName = "in_Sender";
               sqlPt[2].SqlDbType = SqlDbType.NVarChar;
               sqlPt[2].Direction = ParameterDirection.Input;
               sqlPt[2].Size=128;
               sqlPt[2].Value = "Logene.Pathology";
              
                //�¼�����
               sqlPt[3].ParameterName = "in_Description";
               sqlPt[3].SqlDbType = SqlDbType.NVarChar;
               sqlPt[3].Direction = ParameterDirection.Input;
               sqlPt[3].Size=128;
               sqlPt[3].Value = Description;
              
               //���ߵľ����¼��ʶ��
               sqlPt[4].ParameterName = "in_SubjectII";
               sqlPt[4].SqlDbType = SqlDbType.NVarChar;
               sqlPt[4].Direction = ParameterDirection.Input;
               sqlPt[4].Size = 128;
               sqlPt[4].Value =brbh ;
              
                //�¼������Ŀ��ұ�ʶ��
               sqlPt[5].ParameterName = "in_OrganizationII";
               sqlPt[5].SqlDbType = SqlDbType.NVarChar;
               sqlPt[5].Direction = ParameterDirection.Input;
               sqlPt[5].Size = 128;
               sqlPt[5].Value = OrganizationII;
          
               //�¼�����ʱ��
               sqlPt[6].ParameterName = "in_EventDateTime";
               sqlPt[6].SqlDbType = SqlDbType.DateTime;
               sqlPt[6].Direction = ParameterDirection.Input;
               sqlPt[6].Value =DateTime.Now ;
            
                //������¼�¼��Ľ���ʱ��
               sqlPt[7].ParameterName = "in_EventEndOn";
               sqlPt[7].SqlDbType = SqlDbType.DateTime;
               sqlPt[7].Direction = ParameterDirection.Input;
               sqlPt[7].Value =DateTime.Now ;
              
                //�¼�����
               sqlPt[8].ParameterName = "in_EventData";
               sqlPt[8].SqlDbType = SqlDbType.NText;
               sqlPt[8].Direction = ParameterDirection.Input;
               sqlPt[8].Value =XML ;
             
               //�¼�д��ƽ̨ʱ��
               sqlPt[9].ParameterName = "in_RowVersion";
               sqlPt[9].SqlDbType = SqlDbType.DateTime;
               sqlPt[9].Direction = ParameterDirection.Input;
               sqlPt[9].Value =DateTime.Now ;
          
               string err_msg = "";
               SqlDB db = new SqlDB();
              int x= db.ExecuteNonQuery(odbc2his, "pEventPublish", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
           

               if (x>0)
               {
                   if (debug == "1")
                   log.WriteMyLog(Description + "���ɹ�"  );


               if (Description == "ȡ������������¼�֪ͨ")
               {

                   aa.ExecuteSQL("update T_jcxx_fs set F_bz='ȡ��������ɹ�',F_FSZT='�Ѵ���'  where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
               }
               else
               {
                   aa.ExecuteSQL("update  T_JCXX  set F_HISBJ='2' where F_BLH='" + blh + "'");
                   aa.ExecuteSQL("update T_jcxx_fs set F_bz='��������ɹ�',F_FSZT='�Ѵ���' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='�����'");

               }
                   return;
               }
               else
               {
                   if (debug == "1")
                   log.WriteMyLog(Description + "��ʧ�� " + sqlPt[2].Value.ToString());
                   if (Description == "ȡ������������¼�֪ͨ")
                   {
                       log.WriteMyLog("ȡ������ʧ�� " + err_msg); 
                      // if (debug == "1")
                         //  MessageBox.Show(sqlPt[2].Value.ToString() + "ȡ������");

                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='ȡ������ʧ��" + err_msg + "',F_FSZT='�Ѵ���' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
            
                   }
                   else
                   {
  
                       log.WriteMyLog("�������ʧ�� " + err_msg); return;
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='�������ʧ��" + err_msg + "' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='�����'");
            
                   }
                   
               }
           }
           catch(Exception  ee1)
           {
               log.WriteMyLog("��������쳣��" + ee1.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='��������쳣��" + ee1.Message + "' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' ");
            
               return;
           }
          
        }

        //Pt
        public void ZtToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string bgzt,string brlb,string yhmc,string yhbh,string  debug)
        {
            string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
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

            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BLToFZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;

                string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS004.LQ", "BS004", jzlb, "0", "S009", "45541605-3", "70500", "0");

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
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            //����pdf
            string ML = DateTime.Parse(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            string errmsg = ""; string pdf2base64 = ""; string filename = "";
            bool isbase64 = true; bool scpdf = true; bool uppdf = true;
            C_PDF(ML, blh, bgxh, bglx, ref errmsg, ref isbase64, ref pdf2base64, ref filename, ref scpdf, ref uppdf, debug);
            if (pdf2base64.Length < 10)
            {
                log.WriteMyLog("����PDFתbase64ʧ��");
            }
            string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();

            string message = BgMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh,ref errmsg,pdf2base64);
    
            if (message == "")
            {
                log.WriteMyLog("MQ��������XMlʧ�ܣ�" + errmsg);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='MQ��������XMlʧ��:" + errmsg+"' where F_blbh='" + blh + bglx+ bgxh + "' and F_fszt='δ����' and F_BGZT='�����'");
                return;
            }

            if (debug == "1")
                log.WriteMyLog("MQ���淢�ͣ�" + message);

            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BLToFZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;
                string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS366.LQ", "BS366", jzlb, "0", "S009", "45541605-3", "70500", "0");

                if (debug == "1")
                    log.WriteMyLog("(BS366)MQ���淵�أ�" + rtnmsg);
                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS366)MQ���淢�ʹ���" + rtnmsg);
                     aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS366)MQ���淢�ʹ���:" + rtnmsg + "' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='�����' ");
            
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS366)MQ���淢����ɣ�" + rtnmsg);
                     aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS366)MQ���淢�����:" + rtnmsg + "',F_FSZT='�Ѵ���' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='�����' ");
                    return;
                }
                return;
            }
            catch(Exception  ee3)
            {
                log.WriteMyLog("(BS366)MQ���淢���쳣��" + ee3.Message);
                 aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS366)MQ���淢���쳣:" + ee3.Message + "'  where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='δ����' and F_BGZT='�����'");
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
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.2\" extension=\"" + dt_sqdxx.Rows[0]["F_yid"].ToString() + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_jzh"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"  determinerCode=\"INSTANCE\">";
                xml = xml + "<!--���˿��ұ���-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ksbm"].ToString() + "\" root=\"1.2.156.112678.1.1.1\"/>";
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
                xml = xml + "<item extension=\""+yhbh+"\" root=\"1.2.156.112678.1.1.2\"></item>";
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
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_zxksbm"].ToString() + "\" root=\"1.21.156.112649.1.1.1\" />";
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
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yzh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.22\"/>";
                xml = xml + "<!-- ���뵥�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_sqxh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.21\"/>";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item extension=\"" + blh + "\" root=\"1.2.156.112678.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112678.1.2.1.30\"/>";
                xml = xml + "</id>";
             
                xml = xml + "<!-- ҽ��������/ҽ��������� - ���ҩƷ, ������, ������, Ƭ��ҩƷ, ������ -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.27\">";
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
                    xml = xml + "<item extension=\"" + getyhgh(dt.Rows[0]["F_QCYS"].ToString()) + "\" root=\"1.2.156.112678.1.1.2\"></item>";
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
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112678.1.1.93\">";
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
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112678.1.2.1.7\" />";
                xml = xml + "<!-- ������ˮ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112678.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--����������-->";
                xml = xml + "<code codeSystem=\"1.2.156.112678.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
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
                xml = xml + "<code code=\"04\" codeSystem=\"1.2.156.112678.1.1.60\" displayName=\"�������¼\"/>";
                xml = xml +"<!-- �ĵ������ı� -->";
                xml = xml + "<title>�����鱨��</title>";
                xml = xml + "<!-- �ĵ���Ч���� -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" />";
                xml = xml + "<!-- �ĵ��ܼ����� -->";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"2.16.840.1.113883.5.25\" codeSystemName=\"Confidentiality\" displayName=\"normal\" />";
                xml = xml + "<!-- �ĵ����Ա��� -->";
                xml = xml + "<languageCode code=\"zh-CN\" />";
                xml = xml + "<!--����ID-->";
	            xml = xml + "<setId extension=\"BS366\"/>";
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
                
                xml = xml + "<!-- �ĵ��Ĳ����汾:0��ʾ����, 1��ʾ�޸� -->";
                xml = xml + "<versionNumber value=\"" + cfsh + "\"/>";

                xml = xml + "<!-- �ĵ���¼���� -->";
                xml = xml + "<recordTarget typeCode=\"RCT\">";
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<patientRole classCode=\"PAT\">";
                xml = xml + "<!-- ��ID -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.2\" extension=\"" + dt_sqd.Rows[0]["F_yid"].ToString().Trim() + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.3\" extension=\"" + dt_sqd.Rows[0]["F_BRBH"].ToString().Trim() + "\" />";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.12\" extension=\"" + dt_sqd.Rows[0]["F_JZH"].ToString().Trim() + "\" />";

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
                xml = xml + "<administrativeGenderCode code=\"" + dt_sqd.Rows[0]["F_XBBM"].ToString().Trim() + "\" codeSystem=\"1.2.156.112678.1.1.3\" displayName=\"" + dt_sqd.Rows[0]["F_XB"].ToString().Trim() + "\" />";
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
                xml = xml + "<id root=\"1.2.156.112678.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ����ҽ������ -->";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";

                xml = xml + "<!-- �ĵ�������(CDA��custodianΪ������) -->";
                xml = xml + "<custodian>";
                xml = xml + "<assignedCustodian>";
                xml = xml + "<representedCustodianOrganization>";
                xml = xml + "<!-- ҽ�ƻ������� -->";
                xml = xml + "<id root=\"1.2.156.112678\" extension=\"" + dt_sqd.Rows[0]["F_YYBM"].ToString().Trim() + "\" />";
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
                xml = xml + "<id root=\"1.2.156.112678.1.1.2\" extension=\"" + shysid + "\"/>";
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
                xml = xml + "<id root=\"1.2.156.112678.1.1.2\" extension=\""+ dt_sqd.Rows[0]["F_SQYSBM"].ToString().Trim()+"\" />";
                xml = xml + "<associatedPerson>";
                xml = xml + "<!-- ����ҽ������ -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQYS"].ToString().Trim() + "</name>";
                xml = xml + "</associatedPerson>";
                xml = xml + "<scopingOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
			    xml = xml + "<!-- ������ұ��� -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.1\" extension=\"" + dt_sqd.Rows[0]["F_SQKSBM"].ToString().Trim() + "\"/>";
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
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.7\" extension=\"" + dt_sqd.Rows[0]["F_jzcs"].ToString() + "\"/>";
                xml = xml + "<!-- ������ˮ�� -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.6\" extension=\"" + dt_sqd.Rows[0]["F_JZLSH"].ToString() + "\"/>";			
			    xml = xml + "<!-- ����������/����������� -->";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_jzlb"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.80\" displayName=\"" + dt_sqd.Rows[0]["F_brlb"].ToString() + "\" />";
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
                xml = xml + "<value xsi:type=\"SC\" code=\"" + dt_sqd.Rows[0]["F_BQBM"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.33\">" + dt_sqd.Rows[0]["F_BQ"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";

                xml = xml + "<component>";
				xml = xml + "<section>";
				xml = xml + "<code code=\"49033-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Menstrual History\"></code>";
				xml = xml + "<entry>";
				xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
				xml = xml + "<code code=\"8665-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Last menstrual period start date\"></code>";
				xml = xml + "<!--ĩ���¾�ʱ��-->";
				xml = xml + "<value xsi:type=\"TS\" value=\"\"></value>";
				xml = xml + "</observation>";
				xml = xml + "</entry>";
				xml = xml + "<entry>";
				xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
				xml = xml + "<code code=\"63890-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Age at last menstrual period \"></code>";
				xml = xml + "<!--����-->";
				xml = xml + "<value xsi:type=\"ST\"></value>";
				xml = xml + "</observation>";
				xml = xml + "</entry>";
				xml = xml + "</section>";
				xml = xml + "</component>";

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

                //xml = xml + "<component>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- ��鱨�����ͱ�ʶ����/��鱨�����ͱ�ʶ���� --0�������棬1���䱨��>
                //string bglxstr = "0";
                //if (bglx == "bc" || bglx == "bd")
                //{
                //    bglxstr = "1";
                //}
                //xml = xml + "<code code=\"" + bglxstr + "\" codeSystem=\"1.2.156.112678.1.1.112\" displayName=\"�����鱨��\" />";
                //xml = xml + "</observation>";
                //xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
				xml = xml + "<!-- ͼƬ��Ϣ(Ҫ�����ΪBASE64), @mediaType: ͼƬ��ʽ(JPG��ʽ: image/jpeg PDF��ʽΪ: application/pdf) -->";
                xml = xml + "<value xsi:type=\"ED\" mediaType=\"application/pdf\">" + pdf2base64 + "</value>";
				xml = xml + "</observationMedia>";
                xml = xml + "</component>";

                xml = xml + "</organizer>";
                xml = xml + "</entry>";


                ////<!--****************************************************************************-->
                xml = xml + "<!-- ��鱨����Ŀ -->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<!-- ������ͱ���/����������� -->"; 
                //������ͱ���û�У���������OT
                xml = xml + "<code code=\"OT\" codeSystem=\"1.2.156.112678.1.1.41\" displayName=\"����\" />";
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
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_yzxmbm"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.88\" displayName=\"" + dt_sqd.Rows[0]["F_yzxm"].ToString() + "\"/>";
                xml = xml + "<!-- ��鱸ע -->";
                xml = xml + "<text></text>";
                xml = xml + "<!-- ����̶��� -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!-- ��鱨����-�͹�����/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->";
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112678.1.1.98\">";
                //xml = xml + "<originalText>" + System.Security.SecurityElement.Escape(dt.Rows[0]["F_rysj"].ToString());
                xml = xml + "<originalText> " + System.Security.SecurityElement.Escape(dt.Rows[0]["F_RYSJ"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_JXSJ"].ToString()) + "</originalText>";
                xml = xml + "</value>";
                // <!-- ��鱨����-������ʾ/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112678.1.1.98\">";
                string blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_blzd"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_TSJC"].ToString()); ;
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
                xml = xml + "<methodCode code=\"\"  codeSystem=\"1.2.156.112678.1.1.43\" displayName=\"\"/>";
                xml = xml + "<!-- ��鲿λ����/��鲿λ���� -->";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156112649.1.1.42\" displayName=\"\" />";
                xml = xml + "<!-- ���ҽʦ��Ϣ -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<!-- ������� -->";
                xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss")  + "\"/>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- ���ҽ������ -->";
                xml = xml + "<id  root=\"1.2.156.112678.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ���ҽ������ -->";
                xml = xml + "<name>" + dt.Rows[0]["F_bgys"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ��Ͽ��ұ��� -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.1\" extension=\"2070000\"/>";
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
                xml = xml + "<id  root=\"1.2.156.112678.1.1.2\" extension=\"" + qcysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                // <!-- ���ҽ������ -->
                xml = xml + "<name>" + dt.Rows[0]["F_QCYS"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //<!-- �����ұ��� -->
                xml = xml + "<id root=\"1.2.156.112678.1.1.1\" extension=\"2070000\"/>";
                //<!-- ���������� -->
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                string [] F_fzys=dt.Rows[0]["F_FZYS"].ToString().Split('/');
                foreach (string fzys in F_fzys)
                {
                    if (fzys != "")
                    {
                        xml = xml + "<!-- ����ҽʦ��Ϣ -->";
                        xml = xml + "<participant typeCode=\"VRF\">";
                        xml = xml + "<participantRole>";
                        xml = xml + "<!-- ����ҽ������/����ҽ�� -->";
                        xml = xml + "<id  root=\"1.2.156.112678.1.1.2\" extension=\""+getyhgh(fzys)+"\"/>";
                        xml = xml + "<playingEntity>";
                        xml = xml + "<!-- ����ҽ������ -->";
                        xml = xml + "<name>"+fzys+"</name>";
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
                xml = xml + "<participantRole>";
                xml = xml + "<playingDevice>";
                xml = xml + "<!--�����ͺ� ��������-->";
                xml = xml + "<manufacturerModelName code=\"LOGIQ-9\"  displayName=\"Agilent Mx3000P\"/>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //������ҽ���͹�������Ϣ(�����Ķ�����Ƚṹ���������ֵ���Ϣ)
                // xml = xml + "<!-- ������ҽ���͹�������Ϣ(�����Ķ�����Ƚṹ���������ֵ���Ϣ) -->";
                // xml = xml + "<entryRelationship typeCode=\"COMP\">";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"365605003\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"body measurement finding\" />";
                // xml = xml + "<statusCode code=\"completed\" />";

                // xml = xml + "<!-- ��Ŀ��Ϣ(��ѭ��) -->";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"100\" displayName=\"AOD\" />";
                // xml = xml + "<!--<value xsi:type=\"SC\">1mm</value>-->";
                // xml = xml + "<value xsi:type=\"PQ\" value=\"73\" unit=\"����\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"200\" displayName=\"LAD\" />";
                // xml = xml + "<value xsi:type=\"SC\">1mm</value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                //xml = xml + "<component>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<code code=\"300\" displayName=\"FS\" />";
                //xml = xml + "<value xsi:type=\"SC\">33.3%</value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                // xml = xml + "<!-- ������Ϣ�������ʽ��� -->";
                // xml = xml + "</organizer>";
                // xml = xml + "</entryRelationship>";




                // <!-- ͼ����Ϣ(������Ŀ��Ӧ��ͼ��) -->
                //xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                //xml = xml + "<observationMedia   classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/jpg\">" ;
                //xml = xml + "</value>";
                //xml = xml + "</observationMedia>";
                //xml = xml + "</entryRelationship>";
                //<!-- ���ж��Ӱ���Ӧͬһ��studyʱ,���Ը��ô�entryRelationship -->


                ////xml = xml + "</observation></component>";
                ////////<!-- study 2 -->
                ////xml = xml + "<component typeCode=\"COMP\">";
                ////xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                ////xml = xml + "<code code=\"1\" codeSystem=\"1.2.156.112678.1.1.88\" displayName=\"\"/>";
                ////xml = xml + "<text></text>";
                ////xml = xml + "<statusCode code=\"completed\"/>";

                //////        <!-- ��鱨����-�͹�����/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112678.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////        <!-- ��鱨����-������ʾ/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112678.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////        <!-- ��鷽������/��鷽������ -->
                ////xml = xml + "<methodCode code=\"002\"  codeSystem=\"1.2.156.112678.1.1.43\" displayName=\"\"/>";
                //////        <!-- ��鲿λ����/��鲿λ���� -->
                ////xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156.112678.1.1.42\" displayName=\"\" />";
                //////        <!-- ���ҽʦ��Ϣ --

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
                xml = xml + "<id extension=\"\" root=\"1.2.156.112678.1.1.2\"></id>";
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
                // xml = xml + "<code code=\"01\" codeSystem=\"1.2.156.112678.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //// <!-- @code:01��ʾ�͹�����, 02��ʾ������ʾ -->
                // xml = xml + "<code code=\"02\" codeSystem=\"1.2.156.112678.1.1.98\" displayName=\"\" />";
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

                //xml = xml  + "<!-- ���-->";
                //xml = xml  + "<component><section><code code=\"29308-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Diagnosis\"/>";
                //xml = xml  + "<title>���</title>";
                //xml = xml  + "<entry typeCode=\"DRIV\">";
                //xml = xml  + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                //xml = xml  + "<code nullFlavor=\"NA\"/>";
                //xml = xml  + "<entryRelationship typeCode=\"SUBJ\">";
                //xml = xml  + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml  + "<!-- ���������/���������� -->";
                //xml = xml  + "<code code=\"\" codeSystem=\"1.2.156.112678.1.1.29\" displayName=\"\" />";
                //xml = xml  + "<statusCode code=\"completed\"/>";
                //xml = xml  + "<!-- ��������/��������(û�б���ȥ��@code) -->";
                //xml = xml  + "<value xsi:type=\"\" code=\"\" codeSystem=\"1.2.156.112678.1.1.30\" displayName=\"\" />";
                //xml = xml  + "</observation>";
                //xml = xml  + "</entryRelationship>";
                //xml = xml  + "</act>";
                //xml = xml  + "</entry>";
                //xml = xml  + "</section>";
                //xml = xml  + "</component>";


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
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

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

                    string pdfpath = "";

                    ZgqClass.BGHJ(blh, "����PDF", "���", "����PDF�ɹ�", "ZGQJK", "����PDF");
                    bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy),ref pdfpath);
                    if (ssa == true)
                    {
                        if (debug == "1")
                            log.WriteMyLog("�ϴ�PDF�ɹ�");
                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDF�ɹ�:" + ML + "\\" + filename, "ZGQJK", "�ϴ�PDF");

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blh + bglx + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_PDFPATH) values('"+blh+bglx+bgxh+"','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + filename + "','" + pdfpath + "')");

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

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
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
