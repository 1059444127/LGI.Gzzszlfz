using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System.Data.OleDb;

namespace LGHISJKZGQ
{
    class XYFYYY
    {
        //���Ÿ�һҽԺ
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {
                //************************************************
                //-------�������뵥----
                //************************************************
                if (Sslbx == "�������뵥")
                {
                    //dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet-his;UID=pathnet;PWD=4s3c2a1p", "", "");
                    xyfyyyweb.DHCPisXiangYaOne xyfy = new xyfyyyweb.DHCPisXiangYaOne();
                    string pathweburl = f.ReadString("SF", "webservicesurl", "");
                    if (pathweburl.Trim() != "")
                        xyfy.Url = pathweburl;

                    string T_exam_bill_items_sql = f.ReadString("SF", "V_exam_bill_items", "");

                    string yh =f.ReadString("yh", "yhmc", "").Replace("\0", "");

                    //if (yh.Length > 3 && yh.Length % 2 != 1)
                    //    yh = yh.Substring(0, yh.Length / 2);
                    //else
                    //    yh = "";

                    string Operator = yh;
                    string Status = "301";  //:ȡ�����뵥301
                    try
                    {
                        //�ж����뵥��Ϣ�����Ƿ��и�����¼
                        DataTable sqdxx = new DBdata().select_sql("select * from  Examapply  where  CheckFlow='" + Ssbz.Trim() + "'  and  (jszt='�ѷ���' or  jszt='��ִ��'  or  jszt='������')");
                        if (sqdxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("�������뵥��Ϣ���У��޴��������Ϣ����ȷ�ϣ�����");
                            return "0";
                        }

                        if (sqdxx.Rows[0]["jszt"].ToString().Trim() == "������")
                        {
                            MessageBox.Show("�����뵥�����ϣ�����");
                            return "0";
                        }



                        dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                       DataTable  dt_jxcc=aa.GetDataTable("select * from   T_JCXX where F_SQXH='" + Ssbz.Trim() + "'","jcxx");
                       if (dt_jxcc.Rows.Count>0)
                        {
                            MessageBox.Show("��������ڲ���ϵͳ���ѵǼǣ�����ȡ���Ǽ��ٳ���������\n" + "������" + dt_jxcc.Rows[0]["F_xm"].ToString() + "\n����ţ�" + dt_jxcc.Rows[0]["F_BLH"].ToString());
                            return "0";
                        }



                        //�жϴ����뵥�����ﻹ��סԺ

                        string brlb = sqdxx.Rows[0]["InOrOut"].ToString();

                        if (sqdxx.Rows[0]["kdxt"].ToString() == "PIS")
                        { 
                           //�ھ�ϵͳ�����뵥
                         DBdata db = new DBdata();

                          // ��ѯ�շѼ�¼�����շ���Ŀ�Ĳ��ܳ���
                            string EXAM_NO = "";
                            try
                            {
                                string exam_appoints_id_str = "select *  from emr.exam_appoints_id where CHECK_FLOW='" + Ssbz.Trim() + "' ";
                                if (brlb == "����")
                                    exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + Ssbz.Trim() + "' ";
                               DataTable exam_appoints_id = db.select_orcl(exam_appoints_id_str, "��ȡEXAM_NO��");
                               if (exam_appoints_id.Rows.Count > 0)
                                   EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();
                               else
                               {
                                   MessageBox.Show("����ʧ�ܣ�������EXAM_NO�ţ�"); return "0";
                               } 
                            }
                            catch
                            {
                                MessageBox.Show("����ʧ�ܣ��׳��쳣"); return "0";
                            }


                            DataTable dt_sfmx = db.select_orcl("select * from exam.exam_bill_items  where  exam_no='" + EXAM_NO + "' " + T_exam_bill_items_sql + "   and  Performed_By='90' ", "��ȡ�շ���ϸ");
                          if (dt_sfmx.Rows.Count > 0)
                          {
                              MessageBox.Show("�����뵥Ϊ�ھ�����ϵͳ����,��������ʱ���˳����е��շ���Ŀ��"); 
                              return��"0";
                          }
                          else
                          {
                              int x= db.Execute_sql("delete  Examapply   where CheckFlow='" + Ssbz.Trim().Trim() + "'  and (jszt='�ѷ���' or jszt='�ѽ���') ");
                             if (x > 0)
                                 MessageBox.Show("�������뵥���"); 
                             else
                                 MessageBox.Show("�������뵥ʧ��"); 
                              return��"0";

                          }
          


                        }
                        else
                        {
                            //��������
                            string funName = "ExamStatus";
                            if (brlb == "סԺ")
                                funName = "ExamStatusIp";

                            string ExamStatus_XML = "<Request><ExamStatus>"
                                + "<CheckFlow>" + Ssbz + "</CheckFlow>"
                               + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
                               + "<Operator>" + Operator + "</Operator>"
                               + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate><Status>" + Status + "</Status>"
                               + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";




                            string rtn_Status = "";
                            //**********************************
                            //---ִ�г�������---------------
                            try
                            {
                                rtn_Status = xyfy.DhcService(funName, ExamStatus_XML);
                            }
                            catch (Exception e)
                            {
                                log.WriteMyLog("��������״̬����" + e.ToString());
                                MessageBox.Show("��������״̬����,������������������");
                                return "0";
                            }
                            //**********************************
                            //-----��������ֵxml-------------
                            //**********************************
                            if (rtn_Status.Trim() == "")
                            {
                                MessageBox.Show("��������ʧ�ܣ�����ֵΪ��");
                                log.WriteMyLog("����������󣬷���ֵΪ��");
                                return "0";
                            }
                            //------
                            try
                            {

                                XmlDataDocument xd = new XmlDataDocument();
                                xd.LoadXml(rtn_Status);
                                XmlNode xn = xd.SelectSingleNode("/Response");

                                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                                {
                                    MessageBox.Show("�������뵥ʧ�ܣ�");
                                    return "0";
                                }


                                try
                                {
                                    
                                    int tn = new DBdata().Execute_sql("update  Examapply set  jszt='������'  where  CheckFlow='" + Ssbz + "'  and  (jszt='�ѷ���' or  jszt='�ѽ���')");
                                    //if (=0)
                                    MessageBox.Show("�������뵥�ɹ���");
                                    return "0";
                                }
                                catch
                                {
                                    MessageBox.Show("�������뵥��Ϣ��������ɾ��ʧ�ܣ��׳��쳣");
                                    return "0";
                                }
                            }
                            catch (Exception xml_e)
                            {
                                MessageBox.Show("��������ʧ�ܣ�����XML����");
                                log.WriteMyLog("����������󣬽���XML����" + xml_e.ToString());
                                return "0";
                            }
                        }
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("�����쳣");
                        log.WriteMyLog("�����쳣" + eee.ToString());
                        return "0";
                    }
                
                    //------
                }
                //************************************************
                //-------��ȡ���뵥��Ϣ�����ж��Ƿ�Ҫȷ�Ϸ���----
                //************************************************
            
                if (Sslbx == "���뵥")
                {
                    DataTable dt_Examapply = new DataTable();
                  
                    DBdata db = new DBdata();
                    //-------��ȡ���뵥��Ϣ-----------------

                    dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_jcxx = aa.GetDataTable("select * from   T_JCXX where F_SQXH='" + Ssbz.Trim() + "'", "jcxx");
                    try
                    {

                        dt_Examapply = db.select_sql("select * from Examapply where  CheckFlow='" + Ssbz.Trim() + "'  and  (jszt='�ѷ���' or jszt='��ִ��' or jszt='������')");
                      
                    }
                    catch (Exception ee_examapply)
                    {
                        MessageBox.Show("��ȡ���뵥��Ϣ�쳣�����ݿ�����ʧ�ܣ�");
                        log.WriteMyLog("��ȡ���뵥��Ϣ�쳣" + ee_examapply.ToString());
                        return "0";
                    }

                    try
                    {
                        if (dt_Examapply.Rows.Count < 1)
                        {
                            MessageBox.Show("δ�ҵ������뵥��Ϣ");
                            log.WriteMyLog("δ�ҵ������뵥��Ϣ����ȡ��������Ϊ0");
                            return "0";
                        }
                    }
                    catch(Exception sd)
                    {
                        MessageBox.Show(sd.ToString());
                    }


                    if (dt_Examapply.Rows[0]["jszt"].ToString().Trim()=="������")
                        {
                            MessageBox.Show("�����뵥�����ϣ�������ȡ��Ϣ��\n" + "������" + dt_Examapply.Rows[0]["NAME"].ToString().Trim() + "\n����ţ�" + dt_Examapply.Rows[0]["CHECKFLOW"].ToString().Trim() + "\n" + "ID:" + dt_Examapply.Rows[0]["PATIENTID"].ToString().Trim());
                          
                            return "0";
                        }


                        if (dt_jcxx.Rows.Count > 0)
                        {
                            if (MessageBox.Show("�˲����ѵǼǣ������ظ��Ǽǣ��Ƿ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return "0";
                            }
                        }
                  

                    //-------------------------------
                    //------�ж�סԺ�����Ƿ�Ʒ�----------------------
                    string rmb = dt_Examapply.Rows[0]["sfje"].ToString();

                    if (dt_Examapply.Rows[0]["jfbj"].ToString().Trim() != "1" && dt_Examapply.Rows[0]["INOROUT"].ToString().Trim() == "סԺ")
                    {

                        if (MessageBox.Show("��סԺ���˻�δ�Ʒѣ��Ƿ����ڼƷ�", "�շ���Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                            sf.ShowDialog();
                            rmb = sf.F_sfje.ToString();
                            if (rmb == "" || rmb == "0")
                                return "0";
                        }
                        else
                        {
                            MessageBox.Show("��סԺ����δȷ�Ϸ��ã����ܵǼ�");
                            return "0";
                        }



                    }
                  
                    //------�ж����ﲡ���Ƿ����Զ�����----------------------
                    if (dt_Examapply.Rows[0]["zdhj"].ToString().Trim()!= "1" && dt_Examapply.Rows[0]["INOROUT"].ToString().Trim() == "����")
                    {
                        if (MessageBox.Show("�����ﲡ�˻�δ����,���ܻ�δ����,��ȷ��!\n�Ƿ����ڽ��л���", "�շ���Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                            sf.ShowDialog();
                            rmb = sf.F_sfje.ToString();
                            if (rmb == "" || rmb == "0")
                                return "0";
                        }
                        else
                        {
                            MessageBox.Show("�����ﲡ��δ����δ�շѣ����ܵǼ�");
                            return "0";
                        }


                    }

                    //����---�ж��Ƿ��շ�
                    if (dt_Examapply.Rows[0]["INOROUT"].ToString() == "����")
                    {
                        DBdata db2 = new DBdata();
                        try
                        {
                            DataTable dt_exam_no = db2.select_orcl(" select exam_no  from mzemr.exam_appoints_id where CHECK_FLOW='" + dt_Examapply.Rows[0]["CHECKFLOW"].ToString().Trim() + "'", "��ȡexam_no");
                            if (dt_exam_no.Rows.Count > 0)
                            {
                                DataTable dt_exam_appoints_id = db2.select_orcl("select billing_attr,item_name from  exam.exam_bill_items where ordered_by='90' and performed_by='90' and exam_no='" + dt_exam_no.Rows[0]["exam_no"].ToString().Trim() + "' ", "��ȡ�շ���ϸ");
                                if (dt_exam_appoints_id.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt_exam_appoints_id.Rows.Count; i++)
                                    {
                                        if (dt_exam_appoints_id.Rows[i]["billing_attr"].ToString() == "0")
                                        {
                                            MessageBox.Show("�ò��˻����շ���Ŀδ�շ�,������ȡ��Ϣ:" + dt_exam_appoints_id.Rows[i]["item_name"].ToString());

                                            if (MessageBox.Show("�Ƿ�鿴�շ���Ŀ", "�շ���Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                            {
                                                frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                                                sf.ShowDialog();
                                                rmb = sf.F_sfje.ToString();
                                                if (rmb == "" || rmb == "0")
                                                    return "0";
                                                db2.Execute_sql("update Examapply set  sfje='"+rmb+"'  where  CheckFlow='" + Ssbz.Trim() +"'");
                                                MessageBox.Show("��������Ŀδ���ѣ��Ƚ����ٵǼǣ�");
                                                return "0";
                                            }
                                            else
                                            {
                                                return "0";
                                            }
                                           
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("�ò���û���շ���Ŀ��ϸ,������ȡ��Ϣ");
                                    if (MessageBox.Show("�Ƿ�鿴�շ���Ŀ", "�շ���Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                                        sf.ShowDialog();
                                        rmb = sf.F_sfje.ToString();
                                        if (rmb == "" || rmb == "0")
                                            return "0";
                                        db2.Execute_sql("update Examapply set  sfje='" + rmb + "'  where  CheckFlow='" + Ssbz.Trim() + "'");
                                        MessageBox.Show("��������Ŀδ���ѣ��Ƚ����ٵǼǣ�");
                                        return "0";
                                    }
                                    else
                                    {
                                        return "0";
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("�����ȡexam_noʧ��,������ȡ��Ϣ");
                                return "0";
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("�����쳣���쳣��Ϣ��" + ee.ToString());
                            return "0";

                        }
                    }
                    ///////////////////////////////////////////////////////////////
                    //�ϲ������
                    string sfhb = f.ReadString("���뵥", "sfhb", "0").Replace("\0", "");
                    if (sfhb.Trim() == "1")
                    {
                        try
                        {
                            string sss = "select * from Examapply  where jszt='��ִ��' and   PatientID='" + dt_Examapply.Rows[0]["PatientID"].ToString() + "' and visitid='" + dt_Examapply.Rows[0]["visitid"].ToString() + "' and  inorout='" + dt_Examapply.Rows[0]["inorout"].ToString() + "' and name='" + dt_Examapply.Rows[0]["name"].ToString() + "' and deptcode='" + dt_Examapply.Rows[0]["deptcode"].ToString() + "'and  deptname='" + dt_Examapply.Rows[0]["deptname"].ToString() + "'";// and  indate>='" + DateTime.Parse(dt_Examapply.Rows[0]["indate"].ToString()).AddDays(-1).ToString() + "' and indate<='" + DateTime.Parse(dt_Examapply.Rows[0]["indate"].ToString()).AddDays(1).ToString() + "'";
                          
                            DataTable dt2 = db.select_sql(sss);
                          
                            if (dt2.Rows.Count > 0)
                            {

                                if (dt2.Rows[0]["checkflow"].ToString().Trim() !=Ssbz.Trim())
                                {


                                    if (MessageBox.Show("���ˣ�" + dt_Examapply.Rows[0]["name"].ToString() + "���������͹��걾��֯����Ҫ�ϲ���֮ǰ�Ĳ������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        string blh_2 = "";
                                        for (int i = 0; i < dt2.Rows.Count; i++)
                                        {
                                            if (dt2.Rows[i]["checkflow"].ToString().Trim() != Ssbz.Trim())
                                            {

                                                DataTable dt_sqxx = aa.GetDataTable("select F_BLH,F_SQXH2 from   T_JCXX where F_SQXH='" + dt2.Rows[i]["checkflow"].ToString().Trim() + "'  and F_BGZT<>'�����'", "sqxx");
                                                if (dt_sqxx.Rows.Count > 0)
                                                {


                                                    for (int y = 0; y < dt_sqxx.Rows.Count; y++)
                                                    {
                                                        if (dt_sqxx.Rows[y]["F_sqxh2"].ToString().Trim() == Ssbz.Trim())
                                                        {
                                                            MessageBox.Show("�˲���������Ѿ��ϲ����������ٺϲ����ϲ������" + dt_sqxx.Rows[y]["F_blh"].ToString().Trim());
                                                            return "0";

                                                        }
                                                        blh_2 = blh_2 + "^" + dt_sqxx.Rows[y]["F_BLH"].ToString().Trim();
                                                    }

                                                }
                                            }
                                        }
                                        //////////////////////
                                        if (blh_2.Trim().TrimStart('^').Trim() == "" || blh_2.Trim() == "")
                                        {
                                            MessageBox.Show("δ�鵽����ţ����ܺϲ�");
                                            return "0";
                                        }
                                        xyxxshowblh ff = new xyxxshowblh(blh_2.Trim().TrimStart('^'));
                                        ff.ShowDialog();
                                        string getblh = ff.getblh;

                                        if (getblh.Trim() == "")
                                        {
                                            MessageBox.Show("�����Ϊ�գ����ܺϲ�");
                                            return "0";
                                        }
                                        else
                                        {//�ϲ�
                                            DataTable dt_bl = aa.GetDataTable("select *  from   T_JCXX where F_BLH='" + getblh.Trim() + "'", "sqxx");
                                            if (dt_bl.Rows.Count <= 0)
                                            {
                                                MessageBox.Show("��ȡ�����" + getblh.Trim() + "��Ϣ���󣬲��ܺϲ�");
                                                return "0";
                                            }

                                            decimal je_2 = decimal.Parse(dt_bl.Rows[0]["F_SF"].ToString()) + decimal.Parse(rmb);

                                            if ((aa.ExecuteSQL("update  T_JCXX  set F_SQXH2='" + Ssbz.Trim() + "',F_SF='" + je_2.ToString() + "' where  F_BLH='" + getblh.Trim() + "'")) <= 0)
                                                MessageBox.Show("���ܺϲ����ϲ�����");
                                            else
                                            {
                                                db.Execute_sql("update Examapply   set jszt='��ִ��'  where   CheckFlow='" + Ssbz.Trim() + "'  and jszt<>'��ִ��' ");
                                                MessageBox.Show("�ϲ���ɣ�����ţ�" + getblh.Trim());
                                            }
                                            return "0";

                                        }

                                    }
                                }
                                }
          
                        }
                        catch(Exception  ee3)
                        {
                            MessageBox.Show("���ܺϲ����ϲ��쳣" + ee3.ToString());
                            return "0";
                        }
                    }

                    ///////////////////////////////////////////////////////////////////
                    //---------------------------------------------
                    string bbmc = dt_Examapply.Rows[0]["bb1"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw1"].ToString().Trim() + ")";
                    if(dt_Examapply.Rows[0]["bb2"].ToString().Trim()!="" || dt_Examapply.Rows[0]["bb2"].ToString().Trim()!="")
                        bbmc = bbmc + "," + dt_Examapply.Rows[0]["bb2"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw2"].ToString().Trim() + ")";
                    if (dt_Examapply.Rows[0]["bb3"].ToString().Trim() != "" || dt_Examapply.Rows[0]["bb3"].ToString().Trim() != "")
                        bbmc = bbmc + "," + dt_Examapply.Rows[0]["bb3"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw3"].ToString().Trim() + ")";
                    if (dt_Examapply.Rows[0]["bb4"].ToString().Trim() != "" || dt_Examapply.Rows[0]["bb4"].ToString().Trim() != "")
                        bbmc = bbmc + "," + dt_Examapply.Rows[0]["bb4"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw4"].ToString().Trim() + ")";

                  
                  

                    //---------xml------------------------
                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    try
                    {
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_Examapply.Rows[0]["PATIENTID"].ToString() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_Examapply.Rows[0]["VISITID"].ToString() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_Examapply.Rows[0]["CHECKFLOW"].ToString() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_Examapply.Rows[0]["VISITNO"].ToString() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_Examapply.Rows[0]["INPNO"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_Examapply.Rows[0]["NAME"].ToString() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_Examapply.Rows[0]["SEX"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_Examapply.Rows[0]["AGE"].ToString() + (char)34 + " ";

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + dt_Examapply.Rows[0]["ADDRESS"].ToString() + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_Examapply.Rows[0]["PHONE"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_Examapply.Rows[0]["DEPTNAME"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_Examapply.Rows[0]["BEDNO"].ToString() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "����" + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + dt_Examapply.Rows[0]["DATEOFBIRTH"].ToString() + (char)34 + " ";
                        if (dt_Examapply.Rows[0]["KDXT"].ToString()=="PIS")
                            xml = xml + "�ͼ����=" + (char)34 + dt_Examapply.Rows[0]["reqdept"].ToString() + (char)34 + " ";
                        else
                           xml = xml + "�ͼ����=" + (char)34 + dt_Examapply.Rows[0]["DEPTNAME"].ToString() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_Examapply.Rows[0]["REQPHYSICIAN"].ToString() + (char)34 + " ";
                        //if (dt_Examapply.Rows[0]["sfje"].ToString().Trim() != "")
                        //    rmb = dt_Examapply.Rows[0]["sfje"].ToString();
                        xml = xml + "�շ�=" + (char)34 + rmb + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + bbmc.Replace("()","").ToString() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_Examapply.Rows[0]["EXAMITEM"].ToString() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " "; 
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + dt_Examapply.Rows[0]["Identitys"].ToString() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_Examapply.Rows[0]["INOROUT"].ToString() + (char)34 + " ";
                        xml = xml + "/>";
                        string mcyj = dt_Examapply.Rows[0]["mcyj"].ToString().Trim();
                        if (mcyj != "")
                            mcyj = "ĩ���¾���" + mcyj;
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + mcyj +"  "+ dt_Examapply.Rows[0]["ClinSYMP"].ToString() + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_Examapply.Rows[0]["CLINDIAG"].ToString() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        return xml;
                    }
                    catch(Exception  rtn_xml_e)
                    {
                        MessageBox.Show("ƴ��XML�쳣" + rtn_xml_e.ToString());
                        return "0";
                    }
                }
                if (Sslbx == "��ѯ�����")
                {

                    string path = f.ReadString("��ѯ�����", "exepath", "");
                    if (path.Trim() == "")
                        path = "D:\\pathqc\\xy1ysqcx.exe";
                    Process.Start(path);
                    return "0";
                }
                
                return "0";
            } return "0";
        }
    }

    class DBdata
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        string Server = f.ReadString("sqlserverzgq", "Server", "");
        string DataBase = f.ReadString("sqlserverzgq", "DataBase", "");
        string UserID = f.ReadString("sqlserverzgq", "UserID", "");
        string PassWord = f.ReadString("sqlserverzgq", "PassWord", "");

        string orcon_str = "Provider='MSDAORA'; data source=DBSERVER;User ID=DHC;Password=DHC;";
        string odbcsql = f.ReadString("SF", "odbcsql", "");

        public int Execute_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = con = new SqlConnection(constr);
            SqlCommand sqlcom = null;
            try
            {
                sqlcom = new SqlCommand(sqlstr, con);
                con.Open();
                int x = sqlcom.ExecuteNonQuery();
                con.Close();
                sqlcom.Clone();

                return x;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("ִ��SQL����쳣��" + sqlstr + ",\r\n �쳣ԭ��" + ee.ToString());
                con.Close();
                sqlcom.Clone();
                return -1;
            }
        }
        public DataTable select_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = new SqlConnection(constr);
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, con);
                con.Open();
                sqlda.Fill(dt);
                con.Close();

                return dt;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("ִ��SQL��ѯ����쳣��" + sqlstr + ",\r\n �쳣ԭ��" + ee.ToString());
                con.Close();
                return dt;
            }

        }
        public DataTable select_orcl(string orcl_strsql, string sm)
        {
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbDataAdapter orcl_dap = new OleDbDataAdapter(orcl_strsql, orcl_con);
            DataTable dt_bill_items = new DataTable();
            try
            {
                orcl_con.Open();
                orcl_dap.Fill(dt_bill_items);
                orcl_con.Close();
                return dt_bill_items;
            }
            catch (Exception orcl_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("ִ��ORACLE��ѯ����쳣��" + orcl_strsql + ",\r\n �쳣ԭ��" + orcl_ee.ToString());
                return dt_bill_items;

            }

        }
        public int Execute_orcl(string orcl_strsql, string sm)
        {

            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbCommand ocdc = new OleDbCommand(orcl_strsql, orcl_con);
            int x = 0;
            try
            {
                orcl_con.Open();
                x = ocdc.ExecuteNonQuery();
                orcl_con.Close();
                ocdc.Dispose();
            }
            catch (Exception insert_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("ִ��ORACLE����쳣��" + orcl_strsql + ",\r\n �쳣ԭ��" + insert_ee.ToString());
                return 0;
            }
            return x;

        }
    }
}
