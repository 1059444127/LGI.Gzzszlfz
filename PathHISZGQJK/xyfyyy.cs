
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Data.OleDb;
using ZgqClassPub;


namespace PathHISZGQJK
{
    //���Ÿ�һҽԺ
    class xyfyyy
    {
       
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        string constr = "Provider='MSDAORA';data source=DBSERVER;user id =DHC;password=DHC";

        public void pathtohis(string blh, string bglx, string bgxh,string czlx,string dz, string debug, string[] cslb)
        {
            string qxsh = "";
            bglx = bglx.ToLower();
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "1";

            if(bglx=="")
             bglx = "cg";
            if (bgxh == "")
             bgxh = "0";
            string pathweburl = f.ReadString("SF", "webservicesurl", "http://192.168.2.51:57772/csp/dhcens/DHC.Pis.XiangYaOne.BS.Web.cls").Replace("\0", "").Trim(); ;
            string constr = f.ReadString("SF", "odbcsql", "Provider='MSDAORA';data source=DBSERVER;user id =DHC;password=DHC").Replace("\0", "").Trim(); ;
            string sfsf = f.ReadString("SF", "sfsf", "").Replace("\0", "").Trim(); ;

            string msg = f.ReadString("savetohis", "msg", "1").Replace("\0", "").Trim(); ;
            string hczt = f.ReadString("savetohis", "hczt", "").Replace("\0", "").Trim(); ;
            string hcbg = f.ReadString("savetohis", "hcbg", "1").Replace("\0", "").Trim(); ;
            string hcbd = f.ReadString("savetohis", "hcbd", "0");

            xyfyWeb.DHCPisXiangYaOne xyfy = new xyfyWeb.DHCPisXiangYaOne();
            if (pathweburl.Trim() != "")
                xyfy.Url = pathweburl;

             dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            
            string yh = f.ReadString("yh", "yhmc", "").Replace("\0", "");
         
            string funName = "ExamStatus";

            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "'  and F_BD_BGZT='�����'", "bdbg");
            if (bljc == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                log.WriteMyLog("�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                log.WriteMyLog("������д���");
                return;
            }
            string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog(blh + ",��������ţ����ݺţ���������");
                return;
            }

            string brlx = bljc.Rows[0]["F_brlb"].ToString().Trim();
            string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

            if (dz == "qxsh")
                bgzt = "ȡ�����";
            //һ����� F_sqxh Ϊ����������
            //�������+����ֿ������뵥��ֻ����һ������ţ��Һϲ�������ˣ���Ĭ�� F_sqxhΪ����������ţ�F_sqxh2Ϊ����������
            string bd_or_cg = "0";
            //�ϲ���� �������뵥�ţ��ڶ������뵥��
            string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
            //�����ϴ����
            //string bdscbj = bljc.Rows[0]["F_bdscbj"].ToString().Trim();
            //�ش����ı������뵥�������޸�examapply�еı��

            if (hczt == "1" &&  bglx == "cg")
            {
                #region ״̬��д
                try
                {
                    Execute_sql("update examapply set jszt='��ִ��' where jszt<>'��ִ��' and checkflow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'");
                }
                catch
                {
                }

                if (brlx == "סԺ")
                    funName = "ExamStatusIp";  //סԺ�ķ�����
                else
                    if (brlx == "����")
                        funName = "ExamStatus";   //����ķ�����   ExamReport
                    else
                    {
                        log.WriteMyLog(blh + ",��סԺ�����ﲡ�˲�����������");
                        return;
                    }
                /////////////////////////////////
             
                string Operator = "";
                string Status = "";  // 1:ԤԼ;2:�Ǽ�;3,4:���;5:ȡ���Ǽ�;6:ȡ��ԤԼ;7:ȡ�����뵥8.����

                //��д�������뵥״̬
                if (bgzt == "�ѵǼ�")
                {
                    Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                    Status = "201";
                }
                else
                    if (bgzt == "��ȡ��")
                    {

                        Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                        Status = "201";
                    }
                    else

                        if (bgzt == "��������")
                        {
                            Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                            Status = "80";
                        }
                        else
                            if (bgzt == "�����")
                            {
                                Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                                Status = "901";
                            }
                            else
                                if (bgzt == "��д����")
                                {
                                    Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                    Status = "90";
                                }
                                else
                                {
                                    Status = "";
                                }

                if (Status == "")
                {
                    log.WriteMyLog(blh + ",�������뵥��״̬Ϊ�գ����ش�");
                    return;
                }

                //�޸������շ�״̬���ǼǺ�Ͳ����˷���
                #region  �޸������շ�״̬
                if (sfsf == "1")
                {
                    if (brlx == "����" && bljc.Rows[0]["F_MZSFBZ"].ToString().Trim() != "1")
                    {
                        string EXAM_NO = "";
                        try
                        {
                            string exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "' ";
                            DataTable exam_appoints_id = select_orcl(exam_appoints_id_str, "��ȡEXAM_NO��");
                            EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();

                            if (EXAM_NO != "")
                            {
                                DataTable dt_sfm = select_orcl("select * from exam.exam_bill_items  where  status='A' and billing_attr='1' and performed_by='90' and not  RCPT_NO is null and  exam_no='" + EXAM_NO + "' ", "��ȡ�շ���ϸ");
                                DataTable dta = select_orcl("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='���' and ADVICE_ID like '" + EXAM_NO + "%'", "��ѯoutp_bill_items");
                                // MessageBox.Show("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='���' and ADVICE_ID like '" + EXAM_NO + "%'");
                                if (dta.Rows.Count > 0)
                                {
                                    for (int x = 0; x < dt_sfm.Rows.Count; x++)
                                    {
                                        string outp_bill_items_str = "update  outp_bill_items set status='1' where  status is null  and  Rcpt_no='" + dt_sfm.Rows[x]["Rcpt_no"].ToString().Trim() + "'and ITEM_class='" + dt_sfm.Rows[x]["ITEM_class"].ToString().Trim() + "'and PERFORMED_BY='90' and costs='" + dt_sfm.Rows[x]["costs"].ToString().Trim() + "'and ADVICE_ID='" + dt_sfm.Rows[x]["exam_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["exam_item_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["charge_item_no"].ToString().Trim() + "'";
                                        int y = insert_orcl(outp_bill_items_str, "�˷���Ϣ-�޸�outp_bill_items");
                                        if (y > 0)
                                        {
                                            aa.ExecuteSQL("update T_JCXX set F_MZSFBZ='1'  where F_blh='" + blh + "'");
                                        }
                                    }
                                }
                            }


                        }
                        catch (Exception ees)
                        {
                            log.WriteMyLog(blh + ",�޸������շ�״̬�쳣��" + ees.Message.ToString());
                        }
                    }
                }
                #endregion

                //��д�������뵥״̬
                if (hcbd == "1")
                {
                    if (dt_bd.Rows.Count > 0)
                    {
                        Operator = dt_bd.Rows[0]["F_BD_BGYS"].ToString().Trim();
                        Status = "901";
                    }

                }



                string ExamStatus_XML = "<Request><ExamStatus>"
                + " <CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
                + "<Operator>" + Operator.Trim() + "</Operator>"
                + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                + "<Status>" + Status + "</Status>"
                + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

                string rtn_Status = "";

                if (debug == "1")
                 log.WriteMyLog("��д״̬XML��" + ExamStatus_XML);
                //****************************************************
                //��д״̬
                //****************************************************
                try
                {
                    rtn_Status = xyfy.DhcService(funName, ExamStatus_XML);
                }
                catch (Exception e)
                {
                    log.WriteMyLog(blh + ",��д״̬�쳣��" + e.Message.ToString());
                    return;
                }
                //****************************************************
                //��д״̬,����ֵxml�Ľ���
                //****************************************************
                try
                {
                    if (rtn_Status == "")
                    {
                        log.WriteMyLog(blh + "��д״̬������ֵΪ��");
                        return;
                    }
                }
                catch(Exception  ee2)
                {
                    log.WriteMyLog(blh + "��д״̬�쳣��"+ee2.Message);
                }
                if (debug == "1")
                    log.WriteMyLog(rtn_Status);
                try
                {
                    XmlDataDocument xd = new XmlDataDocument();
                    xd.LoadXml(rtn_Status);
                    XmlNode xn = xd.SelectSingleNode("/Response");

                    if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        log.WriteMyLog(blh + "��д״̬ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                        return;
                    }
                    else
                    {
                        if (debug == "1")
                           log.WriteMyLog("�ش�״̬�ɹ�");
                    }
                }
                catch (Exception rtne)
                {
                 
                    log.WriteMyLog(blh + ",��д״̬ʧ�ܣ�����ֵXML�����쳣��" + rtn_Status + "&" + rtne.ToString());
                    return;
                }

                #endregion
                //�ϲ���� �������뵥�ţ��ڶ������뵥��string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                #region  �������뵥�ţ��ڶ������뵥��

                if (sqxh2.Trim() != "")
                {
                    if (bgzt == "�ѵǼ�")
                    {
                        Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                        Status = "201";
                    }
                    else
                        if (bgzt == "��ȡ��")
                        {

                            Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                            Status = "201";
                        }
                        else
                            if (bgzt == "��������")
                            {
                                Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                Status = "80";
                            }
                            else
                                if (bgzt == "�����")
                                {
                                    Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                                    Status = "901";
                                }
                                else
                                    if (bgzt == "��д����")
                                    {
                                        Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                        Status = "90";
                                    }
                                    else
                                        if (Status == "")
                                        {
                                            log.WriteMyLog(blh + ",�������뵥��״̬2Ϊ�գ����ش�");
                                            return;
                                        }
                    string ExamStatus_XML2 = "<Request><ExamStatus>"
               + " <CheckFlow>" + bljc.Rows[0]["F_sqxh2"].ToString().Trim() + "</CheckFlow>"
               + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
               + "<Operator>" + Operator.Trim() + "</Operator>"
               + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
               + "<Status>" + Status + "</Status>"
               + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

                    string rtn_Status2 = "";



                    if (debug == "1")
                     log.WriteMyLog("��д״̬XML��" + ExamStatus_XML2);
                    //****************************************************
                    //��д״̬
                    //****************************************************
                    try
                    {
                        rtn_Status2 = xyfy.DhcService(funName, ExamStatus_XML2);
                    }
                    catch (Exception e)
                    {
                        log.WriteMyLog(blh + "��д״̬�쳣��" + e.Message.ToString());
                        return;
                    }
                    //****************************************************
                    //��д״̬,����ֵxml�Ľ���
                    //****************************************************
                    try
                    {
                        if (rtn_Status2 == "")
                        {
                            log.WriteMyLog(blh + "��д״̬ʧ�ܣ�����ֵΪ��");
                            return;
                        }
                    }
                    catch(Exception  ee2)
                    {
                        log.WriteMyLog(blh + "��д״̬ʧ�ܣ�����ֵ�쳣" + ee2.Message);
                        return;
                    }
                    try
                    {
                        XmlDataDocument xd = new XmlDataDocument();
                        xd.LoadXml(rtn_Status2);
                        XmlNode xn = xd.SelectSingleNode("/Response");

                        if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                        {
                            log.WriteMyLog(blh + ",��д״̬ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                            return;
                        }
                        else
                        {
                            if (debug == "1")
                              log.WriteMyLog("�ش�״̬�ɹ�");
                        }
                    }
                    catch (Exception rtne)
                    {
                        log.WriteMyLog(blh + "��д״̬�쳣������ֵXML�����쳣��" + rtn_Status2 + "&" + rtne.ToString());
                        return;
                    }

                }
                #endregion
            }


            if (bgzt == "�����")
            {
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                {
                    //����jpg�ļ�
                    string jpgname = "";
                    #region  ����pdf

                    string message = "";
                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                    if (debug == "1")
                        log.WriteMyLog("��ʼ����PDF������");
                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref message, ref jpgname);

                    string xy = "3";
                    if (isrtn)
                    {
                        if (File.Exists(jpgname))
                        {
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                            if (ssa == true)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("�ϴ�PDF�ɹ�");

                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "JK", "���", "�ϴ�PDF�ɹ�:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "�ϴ�PDF");

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='true' where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");

                            }
                            else
                            {
                                log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + message);
                                ZgqClass.BGHJ(blh, "JK", "���", message, "ZGQJK", "�ϴ�PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                                //  return;
                            }
                        }
                        else
                        {
                            log.WriteMyLog("����PDFʧ��:δ�ҵ��ļ�---" + jpgname);
                            ZgqClass.BGHJ(blh, "JK", "���", "�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname, "ZGQJK", "����PDF");
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����' ");
                            //  return;
                        }
                    }
                    else
                    {

                        log.WriteMyLog("����PDFʧ�ܣ�" + message);
                        ZgqClass.BGHJ(blh, "JK", "���", message, "ZGQJK", "����PDF");
                        aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='����PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                        // return;
                    }
                    #endregion
                }
            }

            ////////////////////////////////////////////////////////
            /////�ش���������//////////////////////////////////////
            /////////////////////////////////////////////////////////
            if (hcbg == "1")
            {
                #region   �ش���������
                //if (hcbd == "1" && dt_bd.Rows.Count > 0)
                //{

                //    //�ش�����
                //    string ExamReportXML = "";
                //    string blzd_bd = "���������" + dt_bd.Rows[0]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                //    for (int i = 1; i < dt_bd.Rows.Count; i++)
                //    {
                //        blzd_bd = blzd_bd + "     ��" + (i + 1).ToString() + "�α��������" + dt_bd.Rows[i]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                //    }
                //    if (bljc.Rows[0]["F_BGZT"].ToString() == "�����")
                //    {
                //        string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                //        if (sqxh == "")
                //            sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
                //        string blzd = "���没����ϣ�" + bljc.Rows[0]["F_blzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                //        blzd_bd = blzd + "    " + blzd_bd;
                //        ExamReportXML = "<Request><ExamReport>"
                //      + "<CheckFlow>" + sqxh + "</CheckFlow>"
                //        + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                //          + "<ExecLink></ExecLink>"
                //          + "<ExamName></ExamName>"
                //          + "<Data>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Data>"
                //          + "<Findings>" + "" + "</Findings>"
                //          + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                //          + "<Id></Id>"
                //          + "<ExamPara></ExamPara>"
                //          + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                //          + "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                //          + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                //           + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                //            + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                //             + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"
                //                 + "<ReqDocID></ReqDocID>"
                //                + "<ReqDateTime></ ReqDateTime>"
                //                + "<ScheduledDocID></ScheduledDocID>"
                //                + "<ScheduledDateTime></ScheduledDateTime>"
                //                + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                //                + "<RegDateTime>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegDateTime>"
                //                + "<TechnicianID></TechnicianID>"
                //                + "<ExamDateTime></ExamDateTime>"
                //                + "<ReporterID>" + getyhgh(bljc.Rows[0]["F_bgYS"].ToString().Trim()) + "</ReporterID>"
                //                + "<ReportDateTime>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd") + "</ReporterDateTime>"
                //                + "<VerifyDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDocID>"
                //                + "<VerifyDateTime>" + DateTime.Parse(bljc.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd") + "</VerifyDateTime>"
                //                + "<IssueDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</IssueDocID>"
                //                + "<IssueDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</IssueDateTime>"
                //                + "<Status>4</Status>"
                //                + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</OperatorDateTime>"
                //          + "<ExamItemsList><ExaminationItems>"
                //          + "<Code></Code><Name></Name><Value></Value>"
                //          + "</ExaminationItems></ExamItemsList></ExamReport></Request>";
                //    }
                //    else
                //    {
                //        ExamReportXML = "<Request><ExamReport>"
                //   + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                //     + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/����.asp?blh=" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</CheckLink>"
                //       + "<ExecLink></ExecLink>"
                //       + "<ExamName></ExamName>"
                //       + "<Data>" + dt_bd.Rows[0]["F_BD_bgrq"].ToString().Trim() + "</Data>"
                //       + "<Findings>" + "" + "</Findings>"
                //       + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                //       + "<Id></Id>"
                //       + "<ExamPara></ExamPara>"
                //       + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                //       + "<Reporter>" + dt_bd.Rows[0]["F_BD_bgys"].ToString().Trim() + "</Reporter>"
                //       + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                //        + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                //         + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                //          + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"

                //                                      + "<ReqDocID></ReqDocID>"
                //                + "<ReqDateTime></ ReqDateTime>"
                //                + "<ScheduledDocID></ScheduledDocID>"
                //                + "<ScheduledDateTime></ScheduledDateTime>"
                //                + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                //                + "<RegDateTime>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegDateTime>"
                //                + "<TechnicianID></TechnicianID>"
                //                + "<ExamDateTime></ExamDateTime>"
                //                + "<ReporterID>" + "" + "</ReporterID>"
                //                + "<ReportDateTime>" + "" + "</ReporterDateTime>"
                //                + "<VerifyDocID>" + "" + "</VerifyDocID>"
                //                + "<VerifyDateTime>" + "" + "</VerifyDateTime>"
                //                + "<IssueDocID>" + "" + "</IssueDocID>"
                //                + "<IssueDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</IssueDateTime>"
                //                + "<Status>4</Status>"
                //                + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</OperatorDateTime>"

                //       + "<ExamItemsList><ExaminationItems>"
                //       + "<Code></Code><Name></Name><Value></Value>"
                //       + "</ExaminationItems></ExamItemsList></ExamReport></Request>";

                //    }
                  
                //    if (ExamReportXML.Trim() == "")
                //    {
                //         LGZGQClass.log.WriteMyLog(blh + "����xml���󣬲��ܻش�");
                //        return;
                //    }
                //    if (debug == "1")
                //        LGZGQClass.log.WriteMyLog("��д����XML��" + ExamReportXML);

                //    string rtnExamReport = "";
                //    try
                //    {
                //        rtnExamReport = xyfy.DhcService("ExamReport", ExamReportXML);
                //    }
                //    catch (Exception eReport)
                //    {
                //         LGZGQClass.log.WriteMyLog(blh + "��д�����쳣��" + eReport.Message);
                //        return;
                //    }
                //    if (debug == "1")
                //        LGZGQClass.log.WriteMyLog("��д���淵��ֵ" + rtnExamReport);

                //    if (rtnExamReport == "")
                //    {
                //         LGZGQClass.log.WriteMyLog(blh + ",��д��ϣ�����ֵΪ��");
                //        return;
                //    }
                //    try
                //    {
                //        XmlDataDocument xd2 = new XmlDataDocument();
                //        xd2.LoadXml(rtnExamReport);
                //        XmlNode xn2 = xd2.SelectSingleNode("/Response");
                //        if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                //        {
                //             LGZGQClass.log.WriteMyLog(ExamReportXML);
                //             LGZGQClass.log.WriteMyLog(blh + "�ش�����ʧ�ܣ�ԭ��" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                //        }
                //        else
                //        {
                //            if (debug == "1")
                //                 LGZGQClass.log.WriteMyLog("�ش�����ɹ� " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                //            if (bljc.Rows[0]["F_BGZT"].ToString() == "�����")
                //                aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");
                //        }
                //        if (bgzt == "��д����" && bljc.Rows[0]["F_scbj"].ToString().Trim() == "1")
                //        {
                //            string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                //            if (sqxh == "")
                //                sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
                //            //////////////////////////////////////////
                //            //�ش�����
                //            string ExamReportXML3 = "<Request><ExamReportNew>"
                //            + "<CheckFlow>" + sqxh + "</CheckFlow>"
                //              + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                //                + "<ExecLink></ExecLink>"
                //                + "<ExamName></ExamName>"
                //                + "<Data>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Data>"
                //                + "<Findings>" + "" + "</Findings>"
                //                + "<Result>" + "  " + "</Result>"
                //                + "<Id></Id>"
                //                + "<ExamPara></ExamPara>"
                //                + "<IsAbnormal>" + " " + "</IsAbnormal>"
                //                + "<Reporter>" + " " + "</Reporter>"
                //                + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                //                + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                //                + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"

                //                + "<ReqDocID></ReqDocID>"
                //                + "<ReqDateTime></ ReqDateTime>"
                //                + "<ScheduledDocID></ScheduledDocID>"
                //                + "<ScheduledDateTime></ScheduledDateTime>"
                //                + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                //                + "<RegDateTime>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegDateTime>"
                //                + "<TechnicianID></TechnicianID>"
                //                + "<ExamDateTime></ExamDateTime>"
                //                + "<ReporterID>" + "" + "</ReporterID>"
                //                + "<ReportDateTime>" + "" + "</ReporterDateTime>"
                //                + "<VerifyDocID>" + "" + "</VerifyDocID>"
                //                + "<VerifyDateTime>" + "" + "</VerifyDateTime>"
                //                + "<IssueDocID>" + "" + "</IssueDocID>"
                //                + "<IssueDateTime>" + "" + "</IssueDateTime>"
                //                + "<Status>4</Status>"
                //                + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd") + "</OperatorDateTime>"
                //                + "<ExamItemsList><ExaminationItems>"
                //                + "<Code></Code><Name></Name><Value></Value>"
                //                + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                //            if (debug == "1")
                //                MessageBox.Show("��д����XML��" + ExamReportXML3);


                //            //****************************************************
                //            //��д����
                //            //****************************************************
                //            string rtnExamReport3 = "";

                //            try
                //            {
                //                rtnExamReport3 = xyfy.DhcService("ExamReport", ExamReportXML3);
                //            }
                //            catch (Exception eReport)
                //            {
                //                if (msg == "1")
                //                    MessageBox.Show("��д�����쳣��ԭ��" + eReport.ToString());
                //                 LGZGQClass.log.WriteMyLog(ExamReportXML3);
                //                 LGZGQClass.log.WriteMyLog("��д�����쳣��ԭ��" + eReport.ToString());
                //                return;
                //            }
                //            //****************************************************
                //            //��д״̬���ص�xml������
                //            //****************************************************

                //            if (debug == "1")
                //            {
                //                MessageBox.Show("��д���淵��ֵ" + rtnExamReport3.ToString());
                //            }
                //            if (rtnExamReport3 == "")
                //            {
                //                 LGZGQClass.log.WriteMyLog(blh + ",��д��ϣ�����ֵΪ��");
                //                //if (msg == "1")
                //                //    MessageBox.Show("��д��ϣ�����ֵΪ��");

                //                return;
                //            }

                //            //------------------------------
                //            try
                //            {
                //                XmlDataDocument xd3 = new XmlDataDocument();
                //                xd3.LoadXml(rtnExamReport3);
                //                XmlNode xn3 = xd3.SelectSingleNode("/Response");
                //                if (xn3.FirstChild["Returncode"].InnerText.ToString() == "-1")
                //                {
                //                     LGZGQClass.log.WriteMyLog(ExamReportXML3);
                //                    //if (msg == "1")
                //                    //    MessageBox.Show("�ش�����ʧ�ܣ�ԭ��" + xn3.FirstChild["ResultContent"].InnerText.ToString());
                //                     LGZGQClass.log.WriteMyLog(blh + "�ش����棬" + xn3.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn3.FirstChild["Returncode"].InnerText.ToString());
                //                }
                //                else
                //                {
                //                    if (debug == "1")
                //                        MessageBox.Show("�ش�����ɹ� " + xn3.FirstChild["ResultContent"].InnerText.ToString());
                //                    aa.GetDataTable("update T_jcxx  set F_scbj='0'  where F_blh='" + blh + "'", "blx2");

                //                }


                //            }
                //            catch (Exception rtnee)
                //            {
                //                if (msg == "1")
                //                    MessageBox.Show("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                //                 LGZGQClass.log.WriteMyLog(ExamReportXML3);
                //                 LGZGQClass.log.WriteMyLog("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                //                return;

                //            }

                //        }
                //    }
                //    catch (Exception rtnee)
                //    {
                //        if (msg == "1")
                //        {
                //            MessageBox.Show("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                //        }

                //         LGZGQClass.log.WriteMyLog("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                //        return;

                //    }


                //    return;
                //}
                #endregion
                //****************************************************
                //��д���汨�棬ƴxml
                //****************************************************region

                if (bgzt == "ȡ�����")
                {
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                    if (bglx == "cg")
                    {
                        #region ȡ�����
                        string ExamReportXML = "<Request><ExamReportNew>"
                            + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                            + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                            + "<ExecLink></ExecLink>"
                            + "<ExamName></ExamName>"
                            + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                            + "<Findings>" + "" + "</Findings>"
                            + "<Result>" + "  " + "</Result>"
                            + "<Id></Id>"
                            + "<ExamPara></ExamPara>"
                            + "<IsAbnormal>" + " " + "</IsAbnormal>"
                            //+ "<Reporter>" + " " + "</Reporter>"
                            //+ "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                            //+ "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                            //+ "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                            //+ "<REQDATETIME>" + DateTime.Parse(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</REQDATETIME>"
                         
                            + "<ReqDocID></ReqDocID>"
                            + "<ReqDateTime></ReqDateTime>"
                            + "<ScheduledDocID></ScheduledDocID>"
                            + "<ScheduledDateTime></ScheduledDateTime>"
                            + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                            + "<RegDateTime>" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</RegDateTime>"
                            + "<TechnicianID></TechnicianID>"
                            + "<ExamDateTime></ExamDateTime>"
                            + "<ReporterID>" + "" + "</ReporterID>"
                            + "<ReportDateTime>" + "" + "</ReportDateTime>"
                            + "<VerifyDocID>" + "" + "</VerifyDocID>"
                            + "<VerifyDateTime>" + "" + "</VerifyDateTime>"
                            + "<IssueDocID>" + "" + "</IssueDocID>"
                            + "<IssueDateTime>" + "" + "</IssueDateTime>"
                            + "<Status>4</Status>"
                            + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDateTime>"
                            + "<ExamItemsList><ExaminationItems>"
                            + "<Code></Code><Name></Name><Value></Value>"
                            + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                        if (debug == "1")
                            log.WriteMyLog("ȡ�����XML��" + ExamReportXML);

                        string rtnExamReport = "";
                        try
                        {
                            rtnExamReport = xyfy.DhcService("ExamReportNew", ExamReportXML);
                        }
                        catch (Exception eReport)
                        {
                            log.WriteMyLog("ȡ����ˣ�" + eReport.Message);
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "ȡ�����ʧ�ܣ�" + eReport.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='ȡ�����'");
                            return;
                        }
                        if (debug == "1")
                            log.WriteMyLog("ȡ����˷��أ�" + rtnExamReport.ToString());

                        try
                        {
                            if (rtnExamReport == "")
                            {
                                log.WriteMyLog(blh + ",ȡ�����ʧ�ܣ�����ֵΪ��");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "ȡ�����ʧ�ܣ�����ֵΪ��" + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='ȡ�����'");
                                return;
                            }
                        }
                        catch
                        {
                            log.WriteMyLog(blh + ",ȡ�����ʧ�ܣ�����ֵΪ�쳣");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "ȡ�����ʧ�ܣ�����ֵΪ�쳣" + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='ȡ�����'");
                            return;
                        }

                        try
                        {
                            XmlDataDocument xd2 = new XmlDataDocument();
                            xd2.LoadXml(rtnExamReport);
                            XmlNode xn2 = xd2.SelectSingleNode("/Response");
                            if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                            {

                                log.WriteMyLog("ȡ�����ʧ�ܣ�" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + "ȡ�����ʧ�ܣ�" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString() + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='ȡ�����'");

                                return;
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog("ȡ����˳ɹ��� " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                                aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='ȡ����˳ɹ�',F_fszt='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='ȡ�����'");
                                return;
                            }
                        }
                        catch (Exception rtnee)
                        {
                            log.WriteMyLog(blh + "ȡ������쳣��" + rtnee.Message);
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + rtnee.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='ȡ�����'");
                            return;
                        }
                        #endregion
                    }
                    return;
                }

                if (bgzt == "�����" && bglx=="cg")
                {
                    #region ��д���汨��
                    //�ش�����
                    string ExamReportXML = "<Request><ExamReportNew>"
                    + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                    + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                    + "<ExecLink></ExecLink>"
                    + "<ExamName></ExamName>"
                    + "<Date>" +DateTime.Parse( bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</Date>"
                    + "<Findings>" + "" + "</Findings>"
                    + "<Result><![CDATA[" + @bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "]]></Result>"
                    + "<Id></Id>"
                    + "<ExamPara></ExamPara>"
                    + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                    //+ "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                    //+ "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                    //+ "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                    //+ "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                    //+ "<REQDATETIME>" +DateTime.Parse( bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</REQDATETIME>"
                  
                    + "<ReqDocID></ReqDocID>"
                    + "<ReqDateTime></ReqDateTime>"
                    + "<ScheduledDocID></ScheduledDocID>"
                    + "<ScheduledDateTime></ScheduledDateTime>"

                    + "<RegDocID>" + getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</RegDocID>"
                    + "<RegDateTime>" + DateTime.Parse(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</RegDateTime>"
                    + "<TechnicianID></TechnicianID>"
                    + "<ExamDateTime></ExamDateTime>"
                    + "<ReporterID>" + getyhgh(bljc.Rows[0]["F_BGYS"].ToString().Trim()) + "</ReporterID>"
                    + "<ReportDateTime>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</ReportDateTime>"
                    + "<VerifyDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDocID>"
                    + "<VerifyDateTime>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</VerifyDateTime>"
                    + "<IssueDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</IssueDocID>"
                    + "<IssueDateTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</IssueDateTime>"
                    + "<Status>4</Status>"
                    + "<OperatorDateTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDateTime>"
                    + "<ExamItemsList><ExaminationItems>"
                    + "<Code></Code><Name></Name><Value></Value>"
                    + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                    if (debug == "1")
                        log.WriteMyLog("��д����XML��" + ExamReportXML);

                    //****************************************************
                    //��д����
                    //****************************************************
                    string rtnExamReport = "";
                    try
                    {
                        rtnExamReport = xyfy.DhcService("ExamReportNew", ExamReportXML);
                    }
                    catch (Exception eReport)
                    {
                        log.WriteMyLog(blh + "��д�����쳣��" + eReport.Message);
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ش�����ʧ�ܣ�" + eReport.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                        return;
                    }
                    if (debug == "1")
                        log.WriteMyLog("��д���淵��ֵ��" + rtnExamReport);
                    //****************************************************
                    //��д״̬���ص�xml������
                    //****************************************************
                    try
                    {
                        if (rtnExamReport == "")
                        {
                            log.WriteMyLog(blh + ",�ش����棺����ֵΪ��");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ش�����ʧ�ܣ�����ֵΪ��'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                            return;
                        }

                        XmlDataDocument xd2 = new XmlDataDocument();
                        xd2.LoadXml(rtnExamReport);
                        XmlNode xn2 = xd2.SelectSingleNode("/Response");
                        if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                        {
                            log.WriteMyLog("�ش�����ʧ�ܣ�" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ش�����ʧ�ܣ�" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString() + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                            return;
                        }
                        else
                        {
                            if (debug == "1")
                                log.WriteMyLog("�ش�����ɹ���" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                            aa.ExecuteSQL("update T_jcxx  set F_scbj='2'  where F_blh='" + blh + "'");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ش�����ɹ�',F_FSZT='�Ѵ���'   where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                        }
                    }
                    catch (Exception rtnee)
                    {
                        log.WriteMyLog(blh + "�ش�����ʧ�ܣ�����XML�쳣--" + rtnee.Message);
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ش�����ʧ�ܣ�����XML�쳣--" + rtnee.Message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' and F_BGZT='�����'");
                        return;
                    }
                    #endregion
                }
                return; 
            }
            return;
        }
        public void pathtohis20161026(string blh, string yymc)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
          string  msg = f.ReadString("savetohis", "msg", "1");

            xyfyWeb.DHCPisXiangYaOne xyfy = new xyfyWeb.DHCPisXiangYaOne();
            xyfy.Url = "http://192.168.2.64/csp/dhcens/DHC.Ris.XiangYaOne.BS.Web.cls";
            string pathweburl = f.ReadString("SF", "webservicesurl", "");

            string odbcsql = f.ReadString("SF", "odbcsql", "");
            if (odbcsql.Trim() != "")
                constr = odbcsql;

            if (pathweburl.Trim() != "")
                xyfy.Url = pathweburl;

            /////////////////////////////////////////////////////////////////////////
            //�Ƿ��ϴ��շѱ��
            string sfsf = f.ReadString("SF", "sfsf", "");

            string debug = f.ReadString("savetohis", "debug", "");
            string yh = f.ReadString("yh", "yhmc", "").Replace("\0", "");

            //�Ƿ��ϴ���������
            string bdhx = f.ReadString("savetohis", "bdhx", "0");
            string funName = "ExamStatus";


            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "'  and F_BD_BGZT='�����'", "bdbg");
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
                log.WriteMyLog(blh + ",��������ţ����ݺţ���������");
                return;
            }

            string brlx = bljc.Rows[0]["F_brlb"].ToString().Trim();



            //һ����� F_sqxh Ϊ����������
            //�������+����ֿ������뵥��ֻ����һ������ţ��Һϲ�������ˣ���Ĭ�� F_sqxhΪ����������ţ�F_sqxh2Ϊ����������

            string bd_or_cg = "0";
            //�ϲ���� �������뵥�ţ��ڶ������뵥��
            string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
            //�����ϴ����
            //string bdscbj = bljc.Rows[0]["F_bdscbj"].ToString().Trim();
            //�ش����ı������뵥�������޸�examapply�еı��

            try
            {
                Execute_sql("update examapply set jszt='��ִ��' where jszt<>'��ִ��' and checkflow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'");
            }
            catch
            {

            }

            if (brlx == "סԺ")
                funName = "ExamStatusIp";  //סԺ�ķ�����
            else
                if (brlx == "����")
                    funName = "ExamStatus";   //����ķ�����   ExamReport
                else
                {
                    log.WriteMyLog(blh + ",��סԺ�����ﲡ�˲�����������");
                      return;
                }
            
            /////////////////////////////////
            string bgzt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
            string Operator = "";
            string Status = "";  // 1:ԤԼ;2:�Ǽ�;3,4:���;5:ȡ���Ǽ�;6:ȡ��ԤԼ;7:ȡ�����뵥8.����

            //��д�������뵥״̬
            if (bgzt == "�ѵǼ�")
            {
                Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                Status = "201";
            }
            else
                if (bgzt == "��ȡ��")
                {

                    Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                    Status = "201";
                }
                else

                    if (bgzt == "��������")
                    {
                        Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                        Status = "80";
                    }
                    else
                        if (bgzt == "�����")
                        {
                            Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                            Status = "901";
                        }
                        else
                            if (bgzt == "��д����")
                            {
                                Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                Status = "90";
                            }
                            else
                            {
                                Status = "";
                            }

            if (Status == "")
            {
                log.WriteMyLog(blh + ",�������뵥��״̬Ϊ�գ����ش�");
                return;
            }

            //�޸������շ�״̬���ǼǺ�Ͳ����˷���

            #region  �޸������շ�״̬
            if (sfsf == "1")
            {
                if (brlx == "����" && bljc.Rows[0]["F_MZSFBZ"].ToString().Trim()!="1")
                {
                    string EXAM_NO = "";
                    try
                    {
                        string exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "' ";
                        DataTable exam_appoints_id = select_orcl(exam_appoints_id_str, "��ȡEXAM_NO��");
                        EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();

                        if (EXAM_NO != "")
                        {

                            DataTable dt_sfm = select_orcl("select * from exam.exam_bill_items  where  status='A' and billing_attr='1' and performed_by='90' and not  RCPT_NO is null and  exam_no='" + EXAM_NO + "' ", "��ȡ�շ���ϸ");
                            DataTable dta = select_orcl("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='���' and ADVICE_ID like '" + EXAM_NO + "%'", "��ѯoutp_bill_items");
                            // MessageBox.Show("select * from  outp_bill_items where  PERFORMED_BY='90' and status is null and class_on_mr='���' and ADVICE_ID like '" + EXAM_NO + "%'");
                            if (dta.Rows.Count > 0)
                            {
                                for (int x = 0; x < dt_sfm.Rows.Count; x++)
                                {
                                    string outp_bill_items_str = "update  outp_bill_items set status='1' where  status is null  and  Rcpt_no='" + dt_sfm.Rows[x]["Rcpt_no"].ToString().Trim() + "'and ITEM_class='" + dt_sfm.Rows[x]["ITEM_class"].ToString().Trim() + "'and PERFORMED_BY='90' and costs='" + dt_sfm.Rows[x]["costs"].ToString().Trim() + "'and ADVICE_ID='" + dt_sfm.Rows[x]["exam_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["exam_item_no"].ToString().Trim() + "_" + dt_sfm.Rows[x]["charge_item_no"].ToString().Trim() + "'";
                                    int y = insert_orcl(outp_bill_items_str, "�˷���Ϣ-�޸�outp_bill_items");
                                    if (y > 0)
                                    {
                                      aa.ExecuteSQL("update T_JCXX set F_MZSFBZ='1'  where F_blh='" + blh + "'");
                                    }
                                }
                            }
                        }


                    }
                    catch (Exception ees)
                    {
                        if (msg == "1")
                            MessageBox.Show("�޸������շ�״̬�쳣��" + ees.ToString());
                        log.WriteMyLog(blh + ",�޸������շ�״̬�쳣��" + ees.ToString());
                    }


                }
            }
            #endregion 

            //��д�������뵥״̬
            if (bdhx == "1")
            {
                if (dt_bd.Rows.Count > 0)
                {
                    Operator = dt_bd.Rows[0]["F_BD_BGYS"].ToString().Trim();
                    Status = "901";
                }

            }


            string ExamStatus_XML = "<Request><ExamStatus>"
            + " <CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
            + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
            + "<Operator>" + Operator.Trim() + "</Operator>"
            + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
            + "<Status>" + Status + "</Status>"
            + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

            string rtn_Status = "";
            if (debug == "1")
                MessageBox.Show("��д״̬XML��" + ExamStatus_XML);
            //****************************************************
            //��д״̬
            //****************************************************
            try
            {
                rtn_Status = xyfy.DhcService(funName, ExamStatus_XML);
            }
            catch (Exception e)
            {
                if (msg == "1")
                    MessageBox.Show("��д״̬�쳣��" + e.ToString());
                log.WriteMyLog(ExamStatus_XML);
                log.WriteMyLog(blh + ",��д״̬�쳣��" + e.ToString());
                return;
            }
            //****************************************************
            //��д״̬,����ֵxml�Ľ���
            //****************************************************

            if (rtn_Status == "")
            {
                if (msg == "1")
                    MessageBox.Show("��д״̬���󣺷���ֵΪ��");
                log.WriteMyLog(ExamStatus_XML);
                log.WriteMyLog(blh + "��д״̬������ֵΪ��");

                return;
            }
            try
            {
                XmlDataDocument xd = new XmlDataDocument();
                xd.LoadXml(rtn_Status);
                XmlNode xn = xd.SelectSingleNode("/Response");

                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                {
                    if (msg == "1")
                        MessageBox.Show("��д״̬ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString());
                    log.WriteMyLog(ExamStatus_XML);
                    log.WriteMyLog(blh + "��д״̬ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                    return;
                }
                else
                {
                    if (debug == "1")
                        MessageBox.Show("�ش�״̬�ɹ�");
                }
            }
            catch (Exception rtne)
            {
                if (msg == "1")
                    MessageBox.Show("��д״̬ʧ�ܣ�����XML�����쳣��" + rtne.ToString());
                log.WriteMyLog(ExamStatus_XML);
                log.WriteMyLog(blh + ",��д״̬ʧ�ܣ�����ֵXML�����쳣��" + rtn_Status + "&" + rtne.ToString());
                return;
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////////////////////
            //�ش��ϲ���� �������뵥״̬ F_sqxh2
            //�ϲ���� �������뵥�ţ��ڶ������뵥��
            //string sqxh2 = bljc.Rows[0]["F_sqxh2"].ToString().Trim();


            if (sqxh2.Trim() != "")
            {
                if (bgzt == "�ѵǼ�")
                {
                    Operator = bljc.Rows[0]["F_jsy"].ToString().Trim();
                    Status = "201";
                }
                else
                    if (bgzt == "��ȡ��")
                    {

                        Operator = bljc.Rows[0]["F_jly"].ToString().Trim();
                        Status = "201";
                    }
                    else
                        if (bgzt == "��������")
                        {
                            Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                            Status = "80";
                        }
                        else
                            if (bgzt == "�����")
                            {
                                Operator = bljc.Rows[0]["F_shys"].ToString().Trim();
                                Status = "901";
                            }
                            else
                                if (bgzt == "��д����")
                                {
                                    Operator = bljc.Rows[0]["F_bgys"].ToString().Trim();
                                    Status = "90";
                                }
                                else
                                    if (Status == "")
                                    {
                                        log.WriteMyLog(blh + ",�������뵥��״̬2Ϊ�գ����ش�");
                                        return;
                                    }
                string ExamStatus_XML2 = "<Request><ExamStatus>"
           + " <CheckFlow>" + bljc.Rows[0]["F_sqxh2"].ToString().Trim() + "</CheckFlow>"
           + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
           + "<Operator>" + Operator.Trim() + "</Operator>"
           + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
           + "<Status>" + Status + "</Status>"
           + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";

                string rtn_Status2 = "";



                if (debug == "1")
                    MessageBox.Show("��д״̬XML��" + ExamStatus_XML2);
                //****************************************************
                //��д״̬
                //****************************************************
                try
                {
                    rtn_Status2 = xyfy.DhcService(funName, ExamStatus_XML2);
                }
                catch (Exception e)
                {
                    if (msg == "1")
                        MessageBox.Show("��д״̬�쳣," + e.ToString());
                    log.WriteMyLog(ExamStatus_XML2);
                    log.WriteMyLog(blh + "��д״̬�쳣��" + e.ToString());
                    return;
                }
                //****************************************************
                //��д״̬,����ֵxml�Ľ���
                //****************************************************

                if (rtn_Status2 == "")
                {
                    if (msg == "1")
                        MessageBox.Show("��д״̬ʧ�ܣ�����ֵΪ��");

                    log.WriteMyLog(blh + "��д״̬ʧ�ܣ�����ֵΪ��");

                    return;
                }
                try
                {
                    XmlDataDocument xd = new XmlDataDocument();
                    xd.LoadXml(rtn_Status2);
                    XmlNode xn = xd.SelectSingleNode("/Response");

                    if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (msg == "1")
                            MessageBox.Show("��д״̬ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog(ExamStatus_XML2);
                        log.WriteMyLog(blh + ",��д״̬ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                        return;
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("�ش�״̬�ɹ�");
                    }
                }
                catch (Exception rtne)
                {
                    if (msg == "1")
                        MessageBox.Show("��д״̬�쳣������XML�����쳣��" + rtne.ToString());
                    log.WriteMyLog(ExamStatus_XML2);
                    log.WriteMyLog(blh + "��д״̬�쳣������ֵXML�����쳣��" + rtn_Status2 + "&" + rtne.ToString());
                    return;
                }

            }
            ////////////////////////////////////////////////////////
            /////�ش���������//////////////////////////////////////
            /////////////////////////////////////////////////////////

            #region   �ش���������
            if (bdhx == "1" && dt_bd.Rows.Count > 0)
            {
                //����jpg�ļ�
                // ����jpg��ʽ�����ļ�
                //----------------jpg��ftp---------------------

                string status = "";
                string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/blbgjpg").Replace("\0", "");
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                // string ml = DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Year.ToString();
                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                string jpgname = "";
                string ftpstatus = "";
                mdjpg mdj = new mdjpg();
                ////////////////////////////////////////////
                ////��������jgp
                for (int i = 0; i < dt_bd.Rows.Count; i++)
                {
                    string bgxh = dt_bd.Rows[i]["F_BD_BGXH"].ToString();
                    try
                    {
                        mdj.BMPTOJPG(blh, ref jpgname, "bd", bgxh);
                    }
                    catch (Exception ee)
                    {
                        if (msg == "1")
                            MessageBox.Show("������������JPG�ļ��쳣��" + ee.ToString());
                        log.WriteMyLog(blh + "������������JPG�ļ��쳣��" + ee.ToString());
                    }

                    //////////////////////////////////////////////////
                    try
                    {

                        //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                        if (fw.fileCheckExist(ftpURI, blh + "_bd_" + bgxh + ".jpg"))
                        {
                            //ɾ��ftp�ϵ�jpg�ļ�
                            fw.fileDelete(ftpURI, blh + "_bd_" + bgxh + ".jpg").ToString();
                        }
                        //�ϴ������ɵ�jpg�ļ�
                        fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_bd_" + bgxh + "_1.jpg", "", out status);

                        if (status == "Error")
                        {
                            if (msg == "1")
                                MessageBox.Show("��������jpg�ϴ�ʧ�ܣ���������ˣ�");
                            log.WriteMyLog(blh + "��������jpg�ϴ�ʧ�ܣ���������ˣ�");
                        }
                    }
                    catch (Exception e3)
                    {

                        if (msg == "1")
                            MessageBox.Show("�ϴ���������jpg�ļ��쳣" + e3.ToString());
                        log.WriteMyLog(blh + "�ϴ���������jpg�ļ��쳣" + e3.ToString());

                    }
                    try
                    {
                        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    }
                    catch
                    {
                        if (msg == "1")
                            MessageBox.Show("ɾ����ʱĿ¼" + blh + "ʧ��");
                        log.WriteMyLog(blh + "ɾ����ʱĿ¼" + blh + "ʧ��");
                    }
                }

    
                ////////////////////////////////////////////////
                ////���汨��jpg
                if (bljc.Rows[0]["F_BGZT"].ToString() == "�����")
                {
                    string bglx = "cg";
                    string bgxh = "1";


                     jpgname = "";
                     string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
                    {
                        #region  ����pdf

                        string message = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        if (debug == "1")
                            log.WriteMyLog("��ʼ����PDF������");
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref message, ref jpgname);

                        string xy = "3";
                        if (isrtn)
                        {
                            if (File.Exists(jpgname))
                            {
                                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                if (ssa == true)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("�ϴ�PDF�ɹ�");

                                    jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                    ZgqClass.BGHJ(blh, "JK", "���", "�ϴ�PDF�ɹ�:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "�ϴ�PDF");

                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ϴ�PDF�ɹ�',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                                }
                                else
                                {
                                    log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + message);
                                    ZgqClass.BGHJ(blh, "JK", "���", message, "ZGQJK", "�ϴ�PDF");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("����PDFʧ��:δ�ҵ��ļ�---" + jpgname);
                                ZgqClass.BGHJ(blh, "JK", "���", "�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname, "ZGQJK", "����PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            }
                        }
                        else
                        {

                            log.WriteMyLog("����PDFʧ�ܣ�" + message);
                            ZgqClass.BGHJ(blh, "JK", "���", message, "ZGQJK", "����PDF");
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='����PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        }
                        zgq.DelTempFile(blh);
                        #endregion
                    }

                    //mdj.BMPTOJPG(blh, ref jpgname, "cg", "1");
                    //try
                    //{
                    //    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                    //    if (fw.fileCheckExist(ftpURI, blh + "_cg_1.jpg"))
                    //    {
                    //        //ɾ��ftp�ϵ�jpg�ļ�
                    //        fw.fileDelete(ftpURI, blh + "_cg_1.jpg").ToString();
                    //    }
                    //    //�ϴ������ɵ�jpg�ļ�
                    //    fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_cg_1_1.jpg", "", out status);

                    //    if (status == "Error")
                    //    {
                    //        if (msg == "1")
                    //            MessageBox.Show("����jpg�ϴ�ʧ�ܣ���������ˣ�");
                    //         LGZGQClass.log.WriteMyLog(blh + "����jpg�ϴ�ʧ�ܣ���������ˣ�");
                    //    }
                    //}
                    //catch (Exception e4)
                    //{

                    //    if (msg == "1")
                    //        MessageBox.Show("�ϴ�����jpg�ļ��쳣:" + e4.ToString());
                    //     LGZGQClass.log.WriteMyLog(blh + "�ϴ�����jpg�ļ��쳣:" + e4.ToString());
                    //}
                    //try
                    //{
                    //    if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                    //        System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                    //}
                    //catch
                    //{
                    //     LGZGQClass.log.WriteMyLog("ɾ����ʱĿ¼" + blh + "ʧ��");
                    //}
                }
                //////////////////////////////////////////




                //�ش�����
                string ExamReportXML = "";
                string blzd_bd = "���������" + dt_bd.Rows[0]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                for (int i = 1; i < dt_bd.Rows.Count; i++)
                {
                    blzd_bd = blzd_bd + "     ��" + (i + 1).ToString() + "�α��������" + dt_bd.Rows[i]["F_BDZD"].ToString().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");
                }


                if (bljc.Rows[0]["F_BGZT"].ToString() == "�����")
                {
                    string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                    if (sqxh == "")
                        sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();



                    string blzd = "���没����ϣ�" + bljc.Rows[0]["F_blzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                    blzd_bd = blzd + "    " + blzd_bd;
                    ExamReportXML = "<Request><ExamReportNew>"
                  + "<CheckFlow>" + sqxh + "</CheckFlow>"
                    + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                      + "<ExecLink></ExecLink>"
                      + "<ExamName></ExamName>"
                      + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                      + "<Findings>" + "" + "</Findings>"
                      + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                      + "<Id></Id>"
                      + "<ExamPara></ExamPara>"
                      + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                      + "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                      + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                       + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                        + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                         + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"
                      + "<ExamItemsList><ExaminationItems>"
                      + "<Code></Code><Name></Name><Value></Value>"
                      + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";
                }
                else
                {
                    ExamReportXML = "<Request><ExamReportNew>"
               + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                 + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/����.asp?blh=" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</CheckLink>"
                   + "<ExecLink></ExecLink>"
                   + "<ExamName></ExamName>"
                   + "<Date>" + dt_bd.Rows[0]["F_BD_bgrq"].ToString().Trim() + "</Date>"
                   + "<Findings>" + "" + "</Findings>"
                   + "<Result>" + @blzd_bd.Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                   + "<Id></Id>"
                   + "<ExamPara></ExamPara>"
                   + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                   + "<Reporter>" + dt_bd.Rows[0]["F_BD_bgys"].ToString().Trim() + "</Reporter>"
                   + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                    + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                     + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                      + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"
                   + "<ExamItemsList><ExaminationItems>"
                   + "<Code></Code><Name></Name><Value></Value>"
                   + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                }
                if (debug == "1")
                    MessageBox.Show("��д����XML��" + ExamReportXML);
                if (ExamReportXML.Trim() == "")
                {

                    if (msg == "1")
                        MessageBox.Show("����xml���󣬲��ܻش�");
                    log.WriteMyLog(blh + "����xml���󣬲��ܻش�");
                    return;
                }

                //****************************************************
                //��д����
                //****************************************************
                string rtnExamReport = "";

                try
                {
                    rtnExamReport = xyfy.DhcService("ExamReportNew", ExamReportXML);
                }
                catch (Exception eReport)
                {
                    if (msg == "1")
                        MessageBox.Show("��д�����쳣��" + eReport.ToString());
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + "��д�����쳣��" + eReport.ToString());
                    return;
                }
                //****************************************************
                //��д״̬���ص�xml������
                //****************************************************

                if (debug == "1")
                {
                    MessageBox.Show("��д���淵��ֵ" + rtnExamReport.ToString());
                }
                if (rtnExamReport == "")
                {
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + ",��д��ϣ�����ֵΪ��");

                    //if (msg == "1")
                    //    MessageBox.Show("��д��ϣ�����ֵΪ��");

                    return;
                }

                //------------------------------
                try
                {
                    XmlDataDocument xd2 = new XmlDataDocument();
                    xd2.LoadXml(rtnExamReport);
                    XmlNode xn2 = xd2.SelectSingleNode("/Response");
                    if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (msg == "1")
                            MessageBox.Show("�ش�����ʧ�ܣ�ԭ��" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog(ExamReportXML);
                        log.WriteMyLog(blh + "�ش�����ʧ�ܣ�ԭ��" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("�ش�����ɹ� " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        if (bljc.Rows[0]["F_BGZT"].ToString() == "�����")
                            aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");

                    }

                    ////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////
                    /////////�������
                    //////////////////////
                    if (bgzt == "��д����" && bljc.Rows[0]["F_scbj"].ToString().Trim() == "1")
                    {

                        string sqxh = bljc.Rows[0]["F_sqxh2"].ToString().Trim();
                        if (sqxh == "")
                            sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
                        //////////////////////////////////////////
                        //�ش�����
                        string ExamReportXML3 = "<Request><ExamReportNew>"
                        + "<CheckFlow>" + sqxh + "</CheckFlow>"
                          + "<CheckLink><![CDATA[" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "]]></CheckLink>"
                            + "<ExecLink></ExecLink>"
                            + "<ExamName></ExamName>"
                            + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                            + "<Findings>" + "" + "</Findings>"
                            + "<Result>" + "  " + "</Result>"
                            + "<Id></Id>"
                            + "<ExamPara></ExamPara>"
                            + "<IsAbnormal>" + " " + "</IsAbnormal>"
                            + "<Reporter>" + " " + "</Reporter>"
                            + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                            + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                            + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                            
                            + "<ReqDocID></ReqDocID>"
                            + "<ReqDateTime></ ReqDateTime>"
                            + "<ScheduledDocID></ScheduledDocID>"
                            + "<ScheduledDateTime></ScheduledDateTime>"
                            + "<RegDocID>"+getyhgh(bljc.Rows[0]["F_JSY"].ToString().Trim())+"</RegDocID>"
                            + "<RegDateTime>"+bljc.Rows[0]["F_SDRQ"].ToString().Trim()+"</RegDateTime>"
                            + "<TechnicianID></TechnicianID>"
                            + "<ExamDateTime></ExamDateTime>"
                            + "<ReporterID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</ReporterID>"
                            + "<ReportDateTime>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd") + "</ReporterDateTime>"
                            + "<VerifyDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</VerifyDocID>"
                            + "<VerifyDateTime>" + DateTime.Parse(bljc.Rows[0]["F_spare5"].ToString().Trim()).ToString("yyyy-MM-dd") + "</VerifyDateTime>"
                            + "<IssueDocID>" + getyhgh(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</IssueDocID>"
                            + "<IssueDateTime>"+DateTime.Now.ToString("yyyy-MM-dd")+"</IssueDateTime>"
                            + "<Status>4</Status>"
                            + "<OperatorDateTime>"+DateTime.Now.ToString("yyyy-MM-dd")+"</OperatorDateTime>"

                            + "<ExamItemsList><ExaminationItems>"
                            + "<Code></Code><Name></Name><Value></Value>"
                            + "</ExaminationItems></ExamItemsList></ExamReportNew></Request>";

                        if (debug == "1")
                            MessageBox.Show("��д����XML��" + ExamReportXML3);


                        //****************************************************
                        //��д����
                        //****************************************************
                        string rtnExamReport3 = "";

                        try
                        {
                            rtnExamReport3 = xyfy.DhcService("ExamReportNew", ExamReportXML3);
                        }
                        catch (Exception eReport)
                        {
                            if (msg == "1")
                                MessageBox.Show("��д�����쳣��ԭ��" + eReport.ToString());
                            log.WriteMyLog(ExamReportXML3);
                            log.WriteMyLog("��д�����쳣��ԭ��" + eReport.ToString());
                            return;
                        }
                        //****************************************************
                        //��д״̬���ص�xml������
                        //****************************************************

                        if (debug == "1")
                        {
                            MessageBox.Show("��д���淵��ֵ" + rtnExamReport3.ToString());
                        }
                        if (rtnExamReport3 == "")
                        {
                            log.WriteMyLog(blh + ",��д��ϣ�����ֵΪ��");
                            //if (msg == "1")
                            //    MessageBox.Show("��д��ϣ�����ֵΪ��");

                            return;
                        }

                        //------------------------------
                        try
                        {
                            XmlDataDocument xd3 = new XmlDataDocument();
                            xd3.LoadXml(rtnExamReport3);
                            XmlNode xn3 = xd3.SelectSingleNode("/Response");
                            if (xn3.FirstChild["Returncode"].InnerText.ToString() == "-1")
                            {
                                log.WriteMyLog(ExamReportXML3);
                                //if (msg == "1")
                                //    MessageBox.Show("�ش�����ʧ�ܣ�ԭ��" + xn3.FirstChild["ResultContent"].InnerText.ToString());
                                log.WriteMyLog(blh + "�ش����棬" + xn3.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn3.FirstChild["Returncode"].InnerText.ToString());
                            }
                            else
                            {
                                if (debug == "1")
                                    MessageBox.Show("�ش�����ɹ� " + xn3.FirstChild["ResultContent"].InnerText.ToString());
                                aa.GetDataTable("update T_jcxx  set F_scbj='0'  where F_blh='" + blh + "'", "blx2");

                            }


                        }
                        catch (Exception rtnee)
                        {
                            if (msg == "1")
                                MessageBox.Show("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                            log.WriteMyLog(ExamReportXML3);
                            log.WriteMyLog("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                            return;

                        }

                    }
                }
                catch (Exception rtnee)
                {
                    if (msg == "1")
                    {
                        MessageBox.Show("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                    }

                    log.WriteMyLog("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                    return;

                }


                return;
            }
            #endregion


            //****************************************************
            //��д���汨�棬ƴxml
            //****************************************************region
            #region ��д���汨��
            if (bgzt == "�����")
            {

                //����jpg�ļ�
                // ����jpg��ʽ�����ļ�
                string jpgname = "";
                string ftpstatus = "";
                mdjpg mdj = new mdjpg();
                string bglx = "cg";
                string bgxh = "1";
                try
                {
                    mdj.BMPTOJPG(blh, ref jpgname, bglx, bgxh);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                }
                //---�ϴ�jpg----------
                //----------------�ϴ�ǩ��jpg��ftp---------------------

                string status = "";
                string ftpServerIP = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                string ftpUserID = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                string ftpRemotePath = f.ReadString("ftp", "bgjpgPath", "pathimages/blbgjpg").Replace("\0", "");
                FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
                // string ml = DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).Year.ToString();
                string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";

                try
                {

                    ////�ж�Ŀ¼�Ƿ����
                    //if (!fw.fileCheckExist(ftpURI, ml))
                    //{
                    //    //Ŀ¼�����ڣ�����
                    //    string stat = "";
                    //    fw.Makedir(ml, out stat);
                    //}
                    //�ж�ftp���Ƿ���ڸ�jpg�ļ�
                    if (fw.fileCheckExist(ftpURI, blh + "_" + bglx + "_" + bgxh + ".jpg"))
                    {
                        //ɾ��ftp�ϵ�jpg�ļ�
                        fw.fileDelete(ftpURI, blh + "_" + bglx + "_" + bgxh + ".jpg").ToString();
                    }
                    //�ϴ������ɵ�jpg�ļ�
                    fw.Upload("C:\\temp\\" + blh + "\\" + blh + "_" + bglx + "_" + bgxh + "_1.jpg", "", out status);

                    if (status == "Error")
                    {
                        MessageBox.Show("����jpg�ϴ�ʧ�ܣ���������ˣ�");
                    }

                }
                catch
                {
                    MessageBox.Show("�ϴ�����jpg�ļ��쳣");
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


                //////////////////////////////////////////
                //�ش�����
                string ExamReportXML = "<Request><ExamReport>"
                + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                  + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                    + "<ExecLink></ExecLink>"
                    + "<ExamName></ExamName>"
                    + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                    + "<Findings>" + "" + "</Findings>"
                    + "<Result>" + @bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)32).Replace('\r', (char)32).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</Result>"
                    + "<Id></Id>"
                    + "<ExamPara></ExamPara>"
                    + "<IsAbnormal>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</IsAbnormal>"
                    + "<Reporter>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Reporter>"
                    + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                     + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                      + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                       + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"



                    + "<ExamItemsList><ExaminationItems>"
                    + "<Code></Code><Name></Name><Value></Value>"
                    + "</ExaminationItems></ExamItemsList></ExamReport></Request>";

                if (debug == "1")
                    MessageBox.Show("��д����XML��" + ExamReportXML);

                //****************************************************
                //��д����
                //****************************************************
                string rtnExamReport = "";

                try
                {
                    rtnExamReport = xyfy.DhcService("ExamReport", ExamReportXML);
                }
                catch (Exception eReport)
                {
                    if (msg == "1")
                        MessageBox.Show("��д�����쳣��" + eReport.ToString());
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + "��д�����쳣��" + eReport.ToString());
                    return;
                }
                //****************************************************
                //��д״̬���ص�xml������
                //****************************************************

                if (debug == "1")
                {
                    MessageBox.Show("��д���淵��ֵ" + rtnExamReport.ToString());
                }
                if (rtnExamReport == "")
                {
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + ",��д��ϣ�����ֵΪ��");
                    if (debug == "1")
                        MessageBox.Show("��д��ϣ�����ֵΪ��");
                    return;
                }

                //------------------------------
                try
                {
                    XmlDataDocument xd2 = new XmlDataDocument();
                    xd2.LoadXml(rtnExamReport);
                    XmlNode xn2 = xd2.SelectSingleNode("/Response");
                    if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (debug == "1")
                            MessageBox.Show("�ش�����ʧ�ܣ�ԭ��" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog(ExamReportXML);
                        log.WriteMyLog("�ش����棬" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("�ش�����ɹ� " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        aa.GetDataTable("update T_jcxx  set F_scbj='1'  where F_blh='" + blh + "'", "blx2");

                    }


                }
                catch (Exception rtnee)
                {
                    if (msg == "1")
                    {
                        MessageBox.Show("�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                    }
                    log.WriteMyLog(ExamReportXML);
                    log.WriteMyLog(blh + "�ش�����,����XML����,�׳��쳣��" + rtnee.ToString());
                    return;

                }

            }
            #endregion   

            #region
            if (bgzt == "��д����" && bljc.Rows[0]["F_scbj"].ToString().Trim() == "1")
            {


                //////////////////////////////////////////
                //�ش�����
                string ExamReportXML = "<Request><ExamReport>"
                + "<CheckFlow>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckFlow>"
                  + "<CheckLink>" + "http://192.168.2.131/pathwebrpt/index_y.asp?sqxh=" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</CheckLink>"
                    + "<ExecLink></ExecLink>"
                    + "<ExamName></ExamName>"
                    + "<Date>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</Date>"
                    + "<Findings>" + "" + "</Findings>"
                    + "<Result>" + "  " + "</Result>"
                    + "<Id></Id>"
                    + "<ExamPara></ExamPara>"
                    + "<IsAbnormal>" + " " + "</IsAbnormal>"
                    + "<Reporter>" + " " + "</Reporter>"
                    + "<PATHOLOGYNO>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</PATHOLOGYNO>"
                     + "<REQPHYSUCUAN>" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</REQPHYSUCUAN>"
                      + "<REQPHYSUCUANCODE>" + "" + "</REQPHYSUCUANCODE>"
                       + "<REQDATETIME>" + bljc.Rows[0]["F_sdrq"].ToString().Trim() + "</REQDATETIME>"



                    + "<ExamItemsList><ExaminationItems>"
                    + "<Code></Code><Name></Name><Value></Value>"
                    + "</ExaminationItems></ExamItemsList></ExamReport></Request>";

                if (debug == "1")
                    MessageBox.Show("��д����XML��" + ExamReportXML);


                //****************************************************
                //��д����
                //****************************************************
                string rtnExamReport = "";

                try
                {
                    rtnExamReport = xyfy.DhcService("ExamReport", ExamReportXML);
                }
                catch (Exception eReport)
                {
                    if (msg == "1")
                        MessageBox.Show("��д�����쳣��" + eReport.ToString());
                    log.WriteMyLog("��д�����쳣��" + eReport.ToString());
                    return;
                }
                //****************************************************
                //��д״̬���ص�xml������
                //****************************************************

                if (debug == "1")
                {
                    MessageBox.Show("��д���淵��ֵ" + rtnExamReport.ToString());
                }
                if (rtnExamReport == "")
                {
                    log.WriteMyLog(blh + ",��д��ϣ�����ֵΪ��");
                    if (debug == "1")
                        MessageBox.Show("��д��ϣ�����ֵΪ��");

                    return;
                }

                //------------------------------
                try
                {
                    XmlDataDocument xd2 = new XmlDataDocument();
                    xd2.LoadXml(rtnExamReport);
                    XmlNode xn2 = xd2.SelectSingleNode("/Response");
                    if (xn2.FirstChild["Returncode"].InnerText.ToString() == "-1")
                    {
                        if (debug == "1")
                            MessageBox.Show("�ش�����ʧ�ܣ�ԭ��" + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        log.WriteMyLog("�ش����棬" + xn2.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn2.FirstChild["Returncode"].InnerText.ToString());
                    }
                    else
                    {
                        if (debug == "1")
                            MessageBox.Show("�ش�����ɹ� " + xn2.FirstChild["ResultContent"].InnerText.ToString());
                        aa.GetDataTable("update T_jcxx  set F_scbj='0'  where F_blh='" + blh + "'", "blx2");

                    }


                }
                catch (Exception rtnee)
                {
                 log.WriteMyLog(blh + "�ش�����,����XML����,�׳��쳣��" + rtnee.Message);
                    return;
                }
            }
            #endregion ȡ�����
            return;
        }
        //ִ��oracle���ݿ��ѯ��������sql��䣬����datatable
        private DataTable select_orcl(string orcl_strsql, string sm)
        {


            OleDbConnection orcl_con = new OleDbConnection(constr);
            OleDbDataAdapter orcl_dap = new OleDbDataAdapter(orcl_strsql, orcl_con);
            DataTable dt_bill_items = new DataTable();
            try
            {
                orcl_con.Open();
                orcl_dap.Fill(dt_bill_items);
                orcl_con.Close();
            }
            catch (Exception orcl_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("orcle���ݿ��ѯ��������" + sm + "--" + orcl_ee.Message);
                return dt_bill_items;
            }
            return dt_bill_items;
        }
        //ִ��oracle���ݿ��ѯ������insert��update��delete����sql��䣬����Ӱ������
        private int insert_orcl(string orcl_strsql, string sm)
        {
            OleDbConnection orcl_con = new OleDbConnection(constr);
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
                log.WriteMyLog("����orcal���ݿ�������" + sm + "--" + insert_ee.Message);
                return 0;
            }
            return x;

        }

        public int Execute_sql(string sqlstr)
        {
            string Server = f.ReadString("sqlserverzgq", "Server", "");
            string DataBase = f.ReadString("sqlserverzgq", "DataBase", "");
            string UserID = f.ReadString("sqlserverzgq", "UserID", "");
            string PassWord = f.ReadString("sqlserverzgq", "PassWord", "");
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = con = new SqlConnection(constr);
            SqlCommand sqlcom = null;
            try
            {
                con.Open();
                sqlcom = new SqlCommand(sqlstr, con);
                int x = sqlcom.ExecuteNonQuery();
                con.Close();
                sqlcom.Clone();

                return x;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("ִ��SQL����쳣��" + sqlstr + ",\r\n �쳣ԭ��" + ee.Message);
                con.Close();
                sqlcom.Clone();
                return -1;
            }
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
                    return yhmc;
            }
            catch
            {
                return yhmc;
            }
        }
    }
}
