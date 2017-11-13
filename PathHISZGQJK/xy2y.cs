using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class xy2y
    {

           private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
           public void pathtohis(string blh, string debug)
           {

               string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
               string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
               string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
               debug = f.ReadString("savetohis", "debug", "");
                string qryz = f.ReadString("savetohis", "qryz", "1").Replace("\0", "").Trim();
               dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
               DataTable bljc = new DataTable();
               bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

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

               if (bljc.Rows[0]["F_YZID"].ToString().Trim() == "")
               {
                   log.WriteMyLog("ҽ��IDΪ�ղ�����");
                   return;
               }
       
               string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
               string ydid = bljc.Rows[0]["F_YZID"].ToString().Trim();

                #region  ״̬�޸�
                               if (qryz == "1")
                               {
                                   string Status = "E";
                                   if (bgzt.Trim() == "�ѵǼ�")
                                       Status = "E";
                                   else if (bgzt.Trim() == "��ȡ��")
                                       Status = "I";
                                   else if (bgzt.Trim() == "��д����")
                                       Status = "R";
                                   else if (bgzt.Trim() == "�����")
                                       Status = "S";
                                   else
                                       return;

                                   string[] yzids = ydid.Trim().Split('^');

                                   string Request_XML = "";
                                   foreach (string yzid in yzids)
                                   {
                                       if (yzid.Trim() != "")
                                       {
                                           string XML = "<ModifyStauts>"
                                           + "<Status>" + Status + "</Status>"
                                           + "<Rowid>" + yzid + "</Rowid>"
                                           + "<StudyNo>" + blh + "</StudyNo>"
                                           + "<ExeUser>" + yhbh + "@" + yhmc + "</ExeUser>"

                                           + "<RBDate>" + DateTime.Today.ToString("yyyy-MM-dd") + "</RBDate>"
                                           + "<RBTime>" + DateTime.Now.ToString("HH:mm:ss") + "</RBTime>"
                                           + "<RBLoc>�����</RBLoc>"

                                           + "</ModifyStauts>";

                                           Request_XML = Request_XML + XML;
                                       }

                                   }

                                   if (Request_XML == "")
                                   {
                                       log.WriteMyLog("�˲�����ҽ�����޷�ȡ�����룡");
                                       return;
                                   }
                                   if (debug == "1")
                                       log.WriteMyLog(Request_XML);
                         
                                   Request_XML = "<Request><ModifyStatuses>" + Request_XML + "</ModifyStatuses></Request>";


                                   if (debug == "1")
                                       log.WriteMyLog(Request_XML);

                                   //���ش���ֵ����Ҫ��������3��Ϊ����
                                   for (int x = 0; x < 3; x++)
                                   {
                                       string Response_XML = "";
                                       try
                                       {
                                           xy2yweb.XYEPACS xy2yweb = new xy2yweb.XYEPACS();
                                           if (wsurl.Trim() != "")
                                               xy2yweb.Url = wsurl;
                                           Response_XML = xy2yweb.ModifyStatus(Request_XML);

                                       }
                                       catch (Exception ee1)
                                       {
                                           log.WriteMyLog(ee1.Message);
                                           continue;
                                       }

                                       XmlNode xmlok_DATA = null;
                                       XmlDocument xd2 = new XmlDocument();
                                       try
                                       {
                                           xd2.LoadXml(Response_XML);
                                           xmlok_DATA = xd2.SelectSingleNode("/Response");
                                       }
                                       catch (Exception xmlok_e)
                                       {
                                           log.WriteMyLog(Response_XML);
                                           log.WriteMyLog("����XML�쳣��" + xmlok_e.Message);
                                           continue;
                                       }
                                       if (xmlok_DATA["ResultCode"].InnerText.Trim() != "0")
                                       {
                                           log.WriteMyLog("��ȡ����ʧ�ܣ�" + xmlok_DATA["ErrorMsg"].InnerText.Trim());
                                           continue;
                                       }
                                       else
                                       {
                                           if (debug=="1")
                                               MessageBox.Show("״̬��д�ɹ���" + xmlok_DATA["ResultCode"].InnerText.Trim());
                                           aa.ExecuteSQL("update T_JCXX set F_HISBJ='1' where F_BLH='"+blh+"'"); 
                                           break;

                                       }
                                   }
                               }
                             
                #endregion

              #region  �ش�����
               if (bgzt == "�����")
               {
                  
                   string[] yzids = ydid.Trim().Split('^');
                        string  qcys=bljc.Rows[0]["F_QCYS"].ToString().Trim();
                        string F_BGYS = bljc.Rows[0]["F_BGYS"].ToString().Trim();
                        string F_SHYS = bljc.Rows[0]["F_SHYS"].ToString().Trim();
                        string yyx = bljc.Rows[0]["F_RYSJ"].ToString().Trim();
                        if (yyx == "����")
                            yyx = "1";
                        else
                            yyx = "0";

                        string qcrq = "";
                        try
                        {
                            qcrq = DateTime.Parse(bljc.Rows[0]["F_QCRQ"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                        }

                   string blzd = bljc.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + bljc.Rows[0]["F_TSJC"].ToString().Trim();
                   foreach (string yzid in yzids)
                   {
                       string Request_XML = "";
                       if (yzid.Trim() != "")
                       {
                           Request_XML = "<Request><SetReportInfo>"
                                + "<OEOrdItemID>" + yzid + "</OEOrdItemID>"
                                + "<PatNo>" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</PatNo>"
                                + "<StudyNo>" + blh + "</StudyNo>"
                                + "<GetDocCode>" + getyhgh(qcys) + "</GetDocCode>"
                                + "<GetDocName>" + qcys + "</GetDocName>"
                                + "<ReportStatusCode>V</ReportStatusCode>"
                                + "<ReportStatusDesc>���������</ReportStatusDesc>"
                                + "<CauseOfUnsend>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_WFBGYY"].ToString().Trim()) + "</CauseOfUnsend>"
                                + "<ReportDocCode>" + getyhgh(F_BGYS) + "</ReportDocCode>"
                                + "<ReportDocDesc>" + F_BGYS + "</ReportDocDesc>"
                                + "<VerrifyDocCode>" + getyhgh(F_SHYS) + "</VerrifyDocCode>"
                                + "<VerrifyDocDesc>" + F_SHYS + "</VerrifyDocDesc>"
                                + "<ReportDate>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd") + "</ReportDate>"
                                + "<ReportTime>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("HH:mm:ss") + "</ReportTime>"
                                + "<VerrifyDate>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd") + "</VerrifyDate>"
                                + "<VerrifyTime>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("HH:mm:ss") + "</VerrifyTime>"
                                + "<Memo></Memo>"
                                + "<File></File>"
                                + "<Flag></Flag>"
                                + "<GetMaterialDate>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd") + "</GetMaterialDate>"
                                + "<GetMaterialTime>00:00:00</GetMaterialTime>"
                                + "<ExamSee>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JXSJ"].ToString().Trim())+"</ExamSee>"
                                + "<Diagnose>" +System.Security.SecurityElement.Escape(blzd.Trim())+"</Diagnose>"
                                + "<EyeSee>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_RYSJ"].ToString().Trim())+"</EyeSee>"
                                + "<Positive>" + yyx + "</Positive>"
                                + "</SetReportInfo></Request>";
                       }

                       if (Request_XML == "")
                       {
                           log.WriteMyLog("�˲�����ҽ�����޷�ȡ�����룡");
                           return;
                       }
                       if (debug == "1")
                           log.WriteMyLog(Request_XML);

                       //���ش���ֵ����Ҫ��������3��Ϊ����
                       for (int x = 0; x < 3; x++)
                       {
                           string Response_XML = "";
                           try
                           {
                               xy2yweb.XYEPACS xy2yweb = new xy2yweb.XYEPACS();
                               if (wsurl.Trim() != "")
                                   xy2yweb.Url = wsurl;
                               Response_XML = xy2yweb.SetReportInfo(Request_XML);

                           }
                           catch (Exception ee1)
                           {
                               log.WriteMyLog(ee1.Message);
                               continue;
                           }


                           if (debug == "1")
                               log.WriteMyLog(x.ToString() + "--�������ݣ�" + Response_XML);

                           XmlNode xmlok_DATA = null;
                           XmlDocument xd2 = new XmlDocument();
                           try
                           {
                               xd2.LoadXml(Response_XML);
                               xmlok_DATA = xd2.SelectSingleNode("/Response");
                           }
                           catch (Exception xmlok_e)
                           {
                               log.WriteMyLog(Response_XML);
                               log.WriteMyLog("����XML�쳣��" + xmlok_e.Message);
                               continue;
                           }
                           if (xmlok_DATA["ResultCode"].InnerText.Trim() != "0")
                           {
                               log.WriteMyLog("��ȡ����ʧ�ܣ�" + xmlok_DATA["ErrorMsg"].InnerText.Trim());
                               continue;
                           }
                           else
                           {
                               if (debug == "1")
                                   MessageBox.Show("����ش��ɹ���" + xmlok_DATA["ResultCode"].InnerText.Trim());
                               aa.ExecuteSQL("update T_JCXX set F_HISBJ='2' where F_BLH='" + blh + "'");
                               break;
                           }
                       }
                   }
               }
                #endregion

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
        
    }
}
