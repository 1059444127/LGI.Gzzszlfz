using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using ZgqClassPub;

namespace PathHISZGQJK
{
    //------���µ�һ����ҽԺ------------------------
    class cdrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      
        public void pathtohis(string blh, string yymc)
        {
         
          string msg = f.ReadString("savetohis", "msg", "");
            string url = f.ReadString("savetohis", "url", "");
            string bdfk = f.ReadString("bdfk", "bdfk", "");//�����Ƿ�ֿ�
            string sbmc = f.ReadString("savetohis", "sbmc", "2070000").Replace("\0", "");//�豸����
           
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
        


            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();

            string bdzd = ""; string bd_rq = "";string bd_ys ="";
            ////////////////////////////////////////////////////
            bd_rq = bljc.Rows[0]["F_bgrq"].ToString();
            if(bd_rq=="")
                bd_rq = bljc.Rows[0]["F_sdrq"].ToString();
            if (bdfk == "0")
            {

                //--------��������-----------
                DataTable bljc_bd = new DataTable();
                bljc_bd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "'", "bdxx");
                if (bljc_bd.Rows.Count > 0)
                {
                    for (int i = 0; i < bljc_bd.Rows.Count; i++)
                    {
                        if (bljc_bd.Rows[i]["F_BD_BGZT"].ToString() == "�����")
                        {
                            bdzd = bdzd + "���ٱ������" + bljc_bd.Rows[i]["F_BD_BGXH"].ToString() + ":" + bljc_bd.Rows[i]["F_BDZD"].ToString() + "      ";

                            bd_rq = bljc_bd.Rows[0]["F_BD_BGRQ"].ToString();
                            bd_ys = bljc_bd.Rows[0]["F_BD_BGYS"].ToString();
                        }
                    }
                    
                }
            }
              /////////////////////////////////////////////////////////////////////////////////
            string yyx="";
            if(bljc.Rows[0]["F_YYX"].ToString().Trim()=="����")
                yyx="1";
               if(bljc.Rows[0]["F_YYX"].ToString().Trim()=="����")
                yyx="0";


            string xml = "";
            if (bgzt != "�����")
            {

               
                if (bgzt == "��������")
                    bdzd = bdzd + "\r\n�˲��˱��滹δ��ˣ����ܲ鿴��Ͻ�����˱���״̬��" + bljc.Rows[0]["F_BGZT"].ToString().Trim() + "�����滺��ԭ��" + bljc.Rows[0]["F_WFBGYY"].ToString().Trim();
                else
                     bdzd =bdzd+"\r\n�˲��˱��滹δ��ˣ����ܲ鿴��Ͻ�����˱���״̬��" + bljc.Rows[0]["F_BGZT"].ToString().Trim();
               
                xml = "<funderService functionName='Pacs_InsertexamReport_medex'>"
                          + "<value>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</value>"         //����ID                    
                          + "<value>" + bljc.Rows[0]["F_YZID"].ToString().Trim() + "</value>"         //����                    
                          + "<value>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</value>"         //���뵥��        
                          + "<value></value>"         //���� 
                         
                          + "<value>" + bdzd + "</value>"         //��� 
                          + "<value>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</value>"                                                         //Ӱ���
                          + "<value>" + DateTime.Parse(bd_rq).ToString("yyyy-MM-dd HH:mm:ss") + "</value>"         //����ʱ��                       
                          + "<value></value>"  //�����˱���  
                          + "<value>" + bd_ys + "</value>"         //���������� 
                          + "<value>" + "" + "</value>"         //�豸��   
                          + "<value>" + "" + "</value>"         //��������   
                          + "</funderService>";
            }

            if (bgzt == "�����")
            {
                xml = "<funderService functionName='Pacs_InsertexamReport_medex'>"
                      + "<value>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</value>"         //����ID                    
                      + "<value>" + bljc.Rows[0]["F_YZID"].ToString().Trim() + "</value>"         //����                    
                      + "<value>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</value>"         //���뵥��        
                      + "<value>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</value>"         //���� 
                      + "<value>" + bljc.Rows[0]["F_blzd"].ToString().Trim() + "</value>"         //��� 
                      + "<value>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</value>"                                                         //Ӱ���
                      + "<value>" +DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</value>"         //����ʱ��                       
                      + "<value>" + getymh(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</value>"  //�����˱���  
                      + "<value>" + bljc.Rows[0]["F_shys"].ToString().Trim() + "</value>"         //����������    
                      + "<value>" + sbmc + "</value>"         //�豸��   
                      + "<value>" +yyx + "</value>"         //��������   
                      + "</funderService>";
            }
           
                try
                {
                    if (msg == "1")
                        MessageBox.Show(xml.ToString());

                    cdyyWeb.ICalculateService cd = new cdyyWeb.ICalculateService();

                    string mess = cd.funInterFace(xml);
                    if (msg == "1")
                        MessageBox.Show(mess.ToString());
                    if (mess.Trim() == "")
                    {

                        if (msg == "1")
                            MessageBox.Show(mess.ToString());
                        log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "��дʧ�ܣ�" + mess);
                           return;
                    }
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                  
                    try
                    {
                        xd.LoadXml(mess);
                        xmlok = xd.SelectSingleNode("/root");
                        string messae = xmlok["result"].InnerText.ToString();
                        if (messae != "1")
                        {
                            if (msg == "1")
                                MessageBox.Show(messae.ToString());
                            log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "��дʧ�ܣ�" + messae);
                        }
                    }
                    catch (Exception eee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(eee.ToString());

                        MessageBox.Show("����xml�ļ�����" + mess);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                    log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString() + ",��дhis����" + ee.ToString());
                }
                return;
        }

        //ͨ��ҽ������ ��ȡҽ������
        public string getymh(string yhmc)
        {
            if (yhmc != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + yhmc + "'", "blxx");
                    return bljc.Rows[0]["F_yhbh"].ToString().Trim();
                }
                catch (Exception ee)
                {
                    log.WriteMyLog("ת��ҽ�����ų���ԭ��" + ee.ToString());
                    return "0";
                }
            } return "0";

        }
    }
}


