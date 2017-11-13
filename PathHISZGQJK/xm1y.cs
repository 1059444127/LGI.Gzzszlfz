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
    class xm1y  //������ҵ����ƽ̨-���ŵ�һҽԺ
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
          {
              //��ȫУ��ƾ֤��ƾ֤���ǽӿڵļ����룬Ҳ�ǵ��÷�����ݱ�ʶ���ɼ���ƽ̨�ṩ��LIS��PACS.
              string certificate = "W7Djq45nkVdLxkxljMhIDsHW7+sKz7Q5"; 

            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
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

            string visitType = "";   ////	--�������ͣ�1�ż���2סԺ 3��죩
            string visitNo = "";      //����Ż�סԺ��
              string brlb= bljc.Rows[0]["F_brlb"].ToString().Trim();
              if (brlb == "����")
              {
                  visitType = "1";
                  visitNo = bljc.Rows[0]["F_MZH"].ToString().Trim();
              }
              if (brlb == "סԺ")
              {
                  visitType = "2";
                  visitNo = bljc.Rows[0]["F_zyh"].ToString().Trim();
              }
              if (brlb == "���")
                  visitType = "3";

            //string sqxh=bljc.Rows[0]["F_sqxh"].ToString().Trim();
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
            //string pacsBillNo = bljc.Rows[0]["F_blh"].ToString().Trim();
              int  reAuditFlag=0;//�Ƿ��ظ����
            //�Ƿ��ظ����
            string  F_SFCFSH =bljc.Rows[0]["F_SFCFSH"].ToString().Trim();
            if (F_SFCFSH.Trim() == "")
                reAuditFlag = 0;
            else
                reAuditFlag = 1;

            string ztbm = "3";   //״̬���
          
            if(zt=="�ѵǼ�")
             ztbm="3";
          
             if (zt == "��ȡ��")
             ztbm = "5";

            if (zt == "��д����" && reAuditFlag==0)
            ztbm = "6"; 
        
            if (zt == "�����")
            ztbm = "7";

            if (zt == "��д����" && reAuditFlag==1)
            { ztbm = "77"; reAuditFlag = 0; } 


           
            

            string yhmc= f.ReadString("yh","yhmc","").Replace("\0", "");;
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "");
          //  // ���ı���״̬
          //if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
          // sqxh = " ";

            string xmlstr = "<?xml version='1.0' encoding='UTF-8'?>";
            try
            {
                xmlstr = xmlstr + "<root>";
                xmlstr = xmlstr + "<visitType>" + visitType + "</visitType>";//	--�������ͣ�1�ż���2סԺ 3��죩
                xmlstr = xmlstr + "<visitNo>" + bljc.Rows[0]["F_by2"].ToString().Trim() + "</visitNo>";//--����Һź�/סԺ��/����/�����
                xmlstr = xmlstr + "<patientId>" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</patientId>";// -- ����ID 
                xmlstr = xmlstr + "<pacsBillNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</pacsBillNo>";//--pacs���浥��
                xmlstr = xmlstr + "<hisApplyNo>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</hisApplyNo>";//--his���ݺ�
                xmlstr = xmlstr + "<clinicCode></clinicCode>";//HIS�շ���Ŀ����
                xmlstr = xmlstr + "<clinicName></clinicName>";// ��Ŀ����
                xmlstr = xmlstr + "<reportStatus>" + ztbm + "</reportStatus>";// -- ����״̬����

                xmlstr = xmlstr + "<reAuditFlag>" + reAuditFlag + "</reAuditFlag>";//-�ظ���˱�־��1-��ʶ�ظ����
                xmlstr = xmlstr + "<changeTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</changeTime>";// -- �ı�ʱ��
                xmlstr = xmlstr + "<changeOperator>" + yhmc + "/" + yhbh + "</changeOperator>";  // --�޸Ĳ����ˣ�����/����
                xmlstr = xmlstr + "</root>";
            }
            catch
            {
                if (msg == "1")
                    MessageBox.Show("����xml�쳣��");
                log.WriteMyLog("����xml�쳣");
                return;
            }
         
              try
                {

                    xm1yWeb.WSInterface xm1y = new PathHISZGQJK.xm1yWeb.WSInterface();
                    string webServicesURL = f.ReadString("savetohis", "webServicesURL", ""); 
                    if (webServicesURL.Trim()!="")
                        xm1y.Url = webServicesURL;

                    string ztstr = xm1y.CallInterface("UpdatePacsReportStatus", xmlstr, "", certificate);
                  
               if (ztstr.Contains("error"))
                    {
                        if (msg == "1")
                         MessageBox.Show("�ش�����״̬ʧ�ܣ�ԭ��" + ztstr);
                         log.WriteMyLog("�ش�����״̬ʧ�ܣ�ԭ��" + ztstr);
                    }
                    else
                    {
                      
                     }
                  
               
               }
                 catch (Exception e)
                {
                    if (msg == "1")
                        MessageBox.Show("�ش�����״̬�쳣��");
                    log.WriteMyLog("�ش�����״̬�쳣��ԭ��" + e.ToString());
                    return;
                }

                if (zt == "�����")
                {
                    try
                    {
                        aa.ExecuteSQL("update T_JCXX set F_SFCFSH='1' where F_BLH='" + blh + "'");
                    }
                    catch
                    { return;
                    } 
                } return;
              }

    }
             
    }

