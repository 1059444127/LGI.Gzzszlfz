using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Windows.Forms;
using ZgqClassPub;

namespace PathHISZGQJK
{
    //�㶫ʡ����ҽԺ
    class gdsrmyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug,string msg)
        {

         
            string url = f.ReadString("savetohis", "url", "");
            string ksdm = f.ReadString("savetohis", "ksdm", "99");
            string isbghj = f.ReadString("savetohis", "isbghj", "1").Replace("\0", "");    

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");



            if (bljc == null)
            {
                MessageBox.Show("����ţ�"+blh+",�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("����ţ�" + blh + ",������д���");
                return;
            }
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "" )
            {
                BGHJ(blh, "�������Ϊ�գ����ش���", "���");
                log.WriteMyLog(blh+"�������Ϊ�գ����ش���");
                return;
            }

            gdsrmyyWeb.EventBusSvc eb = new PathHISZGQJK.gdsrmyyWeb.EventBusSvc();
            gdsrmyyWeb.EventMessage em = new PathHISZGQJK.gdsrmyyWeb.EventMessage();
            gdsrmyyWeb.EventPublishResponse epr = new PathHISZGQJK.gdsrmyyWeb.EventPublishResponse();

            ///���õ��Ӳ���Webservices��ַ��T_SZ�����û�sz�����ã�sz����
           
            if (url.Trim() != "")
                eb.Url = url;
            else
            {
                try
                {
                    DataTable T_sz = new DataTable();
                    T_sz = aa.GetDataTable("select F_SZZ from T_SZ where F_XL='bghxWebURL' ", "sz");
                    string sz_URL = T_sz.Rows[0]["F_SZZ"].ToString().Trim();
                    if (sz_URL.Trim() != "")
                        eb.Url = sz_URL;
                }
                catch(Exception  e1)
                {
                        if (msg=="1")
                        MessageBox.Show("��ȡT_SZ����bghxWebURL�����쳣");
                    BGHJ(blh, "��ȡT_SZ����bghxWebURL�����쳣", "���");
                 log.WriteMyLog("��ȡT_SZ����bghxWebURL�����쳣"); return;
                }
            }
        
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
            {

               
                    DataTable bljc_bc = new DataTable();
                    bljc_bc = aa.GetDataTable("select * from T_bcbg where F_BLH='" + blh + "' and F_BC_BGZT='�����'", "bcbg");

                  //���
                    DataTable TJ_bljc = new DataTable();
                    TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "T_TBS");

                    string  tj_jcsj="";
                    string tj_blzd = "";


                    string blzd =  bljc.Rows[0]["F_blzd"].ToString().Trim()+"\r\n";//.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                     if (TJ_bljc.Rows.Count > 0)
                        {
                            if (bljc.Rows[0]["F_blk"].ToString().Trim().Contains("TCT"))
                            {
                                tj_jcsj = tj_jcsj + "�걾�����:" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["f_tbs_xbxm1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["f_tbs_xbxm2"].ToString().Trim() + "\r\n";
                                tj_jcsj = tj_jcsj + "\r\n" + "��ԭ΢����:" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n";
                               
                              ////////////////////////////////////
                                tj_blzd = "" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n";
                            
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                blzd = tj_blzd;
                            }
                          
                        }
                    
                                    ///���䱨��
                    string str_bcbg = "";
                 
                    try
                    {
                        for (int z = 0; z < bljc_bc.Rows.Count; z++)
                        {
                            str_bcbg = str_bcbg + ("���䱨��" + bljc_bc.Rows[z]["F_BC_BGXH"].ToString() + ":" + bljc_bc.Rows[z]["F_BCZD"].ToString()+ "\r\n");//.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") ;
                        }
                    }
                    catch (Exception e3)
                    {
                        if (msg == "1")
                            MessageBox.Show("����ţ�" + blh + ",�����쳣��" + e3.Message.ToString());
                       log.WriteMyLog("����ţ�" + blh + ",�����쳣��" + e3.Message.ToString());
                       BGHJ(blh, "�����쳣��" + e3.Message.ToString(), "���");
                    }


                    string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
              
                    xml = xml + "<ArrayOfExamReported_Result>";
                    xml = xml + "<ExamReported_Result>"; 
                try
                {
                   

                    xml = xml + "<ReportII>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportII>";
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                    xml = xml + "<EncounterII>" +"1_"+bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";//0_m,1_z,
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
                        jxsj = ("����������" + jxsj +" \r\n ").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                  
                    string tsjc = bljc.Rows[0]["F_TSJC"].ToString().Trim();
                    if (tsjc.Trim() != "")
                        tsjc = ("�����飺" + tsjc + "\r\n").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");


                    xml = xml + "<ReportDesc>" + rysj + jxsj + tj_jcsj.Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;")+"</ReportDesc>";
                    xml = xml + "<ReportConclusion>" + tsjc + ("������ϣ�" + blzd + str_bcbg).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;")+"</ReportConclusion>";
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
                    xml = xml + "<ExamineOn>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</ExamineOn>";
                    }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog("����ʱ���ʽת���쳣��"+bljc.Rows[0]["F_BGRQ"].ToString().Trim()+"\r\n"+ee2.Message);
                        return;
                    }
                    xml = xml + "<AuditEmployeeII>" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</AuditEmployeeII>";
                    xml = xml + "<AuditOn>" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "</AuditOn>";
                    try
                    {
                    xml = xml + "<ReportOn>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</ReportOn>";
                          }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog("����ʱ���ʽת���쳣��"+bljc.Rows[0]["F_BGRQ"].ToString().Trim()+"\r\n"+ee2.Message);
                        return;
                    }
                    xml = xml + "<ReportNo></ReportNo>";
                    xml = xml + "<EffectiveTime></EffectiveTime>";
                    xml = xml + "<RowVersion>" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "</RowVersion>";
                    xml = xml + "</ExamReported_Result>";
                    xml = xml + "</ArrayOfExamReported_Result>";

                 
                    //�ش�����
                    em.EventII = "Logene.Pathology^" + blh;
                    em.EventName = "EVT.Exam.Reported";
                    em.Sender = "Logene.Pathology";
                    //em.Receiver ="";
                    em.Description = "����������¼�֪ͨ";
                    em.SubjectII = bljc.Rows[0]["F_BRBH"].ToString().Trim();
                    em.OrganizationII = ksdm;
                    //em.OriginalEventII = "";
                    em.EventDateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //em.EventEndOn= "";
                    em.EventData = xml;
                    //em.ContentMime = "";
                    //em.Custom1 = "";
                    //em.Custom2 ="";
                    //em.Custom3 = "";
                    //em.Custom4 = "";ee
                    //em.Custom5 = "";
                    if (debug == "1")
                        log.WriteMyLog("�ش�XML��"+xml);
                }
                catch (Exception e4)
                {
                    if(msg=="1")
                    MessageBox.Show("����ţ�" + blh + ",�ӿ�ƴ��XML�쳣" + e4.Message.ToString());
                    log.WriteMyLog("����ţ�" + blh + ",�ӿ�ƴ��XML�쳣" + e4.Message.ToString());
                    BGHJ(blh,"�ӿ�ƴ��XML�쳣" + e4.Message.ToString(), "���");
                    return;
                }
            try
            {
                epr = eb.Publish(em);
            }
            catch(Exception ee)
            {

                if (msg == "1")
                MessageBox.Show("����ţ�" + blh + ",�ӿڳ����쳣�����淢���쳣��" + ee.Message.ToString());

                BGHJ(blh,"���淢���쳣��" + ee.Message.ToString(), "���");
                log.WriteMyLog("����ţ�" + blh + ",�ӿڳ����쳣�����淢���쳣��" + ee.Message.ToString());
                return;
            }
            if (epr.Result < 0)
            {
                if (msg == "1")
                MessageBox.Show("����ţ�" + blh + ",���淢��ʧ�ܣ�����ֵ��" + epr.Message);
               BGHJ(blh, "���淢��ʧ�ܣ�����ֵ��" + epr.Message, "���");
                log.WriteMyLog("����ţ�" + blh + ",���淢��ʧ�ܣ�����ֵ��" + epr.Message);
                return;
            }
            else
            {
                aa.ExecuteSQL("update T_JCXX  set F_SCBJ='1' where F_BLH='" + blh.Trim() + "'");
                BGHJ(blh, "���淢���ɹ�", "���");
                aa.ExecuteSQL("update T_JCXX_FS  set F_FSZT='�ѷ���'  where F_BLH='" + blh.Trim() + "' ");

                //if (debug == "1")
                //     LGZGQClass.log.WriteMyLog("����ţ�" + blh + ",�����ϴ��ɹ�");
            }

            }
            else
            {

                if (bljc.Rows[0]["F_SCBJ"].ToString().Trim() == "1" && bljc.Rows[0]["F_BGZT"].ToString().Trim() == "��д����")
                {
                  //��������
                    string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
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
                    xml = xml + "<ReportDesc>" +"" + "</ReportDesc>";
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
                    xml = xml + "<AuditOn>" + ""+ "</AuditOn>";
                    xml = xml + "<ReportOn>" + "" + "</ReportOn>";
                    xml = xml + "<ReportNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportNo>";
                    xml = xml + "<EffectiveTime></EffectiveTime>";
                    xml = xml + "<RowVersion>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</RowVersion>";
                    xml = xml + "</ExamReported_Result>";
                    xml = xml + "</ArrayOfExamReported_Result>";


                    //�ش�����
                    em.EventII = "Logene.Pathology^" + blh;
                    em.EventName = "EVT.Exam.ReportCanceled";
                    em.Sender = "Logene.Pathology";
                    //em.Receiver ="";
                    em.Description = "ȡ������������¼�֪ͨ";
                    em.SubjectII = bljc.Rows[0]["F_BRBH"].ToString().Trim();
                    em.OrganizationII = ksdm;
                    //em.OriginalEventII = "";
                    em.EventDateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //em.EventEndOn= "";
                    em.EventData = xml;
                    //em.ContentMime = "";
                    //em.Custom1 = "";
                    //em.Custom2 ="";
                    //em.Custom3 = "";
                    //em.Custom4 = "";
                    //em.Custom5 = "";
                    if (debug == "1")
                        log.WriteMyLog("����ţ�" + blh + ",ȡ�����XML��" + xml);
                    try
                    {
                        epr = eb.Publish(em);
                    }
                    catch (Exception ee)
                    {
                  
                            BGHJ(blh, "��������쳣" + ee.Message.ToString(), "ȡ�����");
                        if (msg == "1")
                        MessageBox.Show("����ţ�" + blh + ",�ӿڳ����쳣����������쳣" + ee.Message.ToString());

                         log.WriteMyLog("����ţ�" + blh + ",�ӿڳ����쳣����������쳣" + ee.Message.ToString());
                        return;
                    }
                    if (epr.Result < 0)
                    {
                     
                            BGHJ(blh, "�������ʧ�ܣ�" + epr.Message, "ȡ�����");
                        if (msg == "1")
                        MessageBox.Show("����ţ�" + blh + ",�������ʧ�ܣ�����ֵ��" + epr.Message);
                        log.WriteMyLog("����ţ�" + blh + ",�������ʧ�ܣ�����ֵ��" + epr.Message);
                        return;
                    }
                    else
                    {
                        aa.ExecuteSQL("update T_JCXX  set F_SCBJ='0' where F_BLH='" + blh.Trim() + "'");
                        aa.ExecuteSQL("delete  from T_JCXX_FS   where F_BLH='" + blh.Trim() + "'");

                        BGHJ(blh, "������ճɹ���" + epr.Message, "ȡ�����");
                        //if (debug == "1")
                        //     LGZGQClass.log.WriteMyLog("����ţ�" + blh + ",ȡ����˳ɹ�");
                    }
              
                }
            }
        }

        private void BGHJ(string blh, string err_nr, string F_CTMC)
        {
            try
            {
                string yhmc = f.ReadString("yh", "yhmc", "-").Replace("/0", "");
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                aa.ExecuteSQL("insert into  T_BGHJ(F_BLH,F_RQ,F_CZY,F_WZ,F_DZ,F_NR,F_EXEMC,F_CTMC) values('" + blh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + yhmc + "','','�ӿ�','" + err_nr + "','RPT','" + "������" + F_CTMC + "') ");
            }
            catch
            {
            }


        }
    }
}
