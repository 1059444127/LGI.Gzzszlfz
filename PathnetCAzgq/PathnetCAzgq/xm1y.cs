using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using dbbase;
using readini;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Data.Odbc;

using PathHISZGQJK;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using KTPKIAPPCOMCLNTLib;
using LGZGQClass;

namespace PathnetCAzgq
{
    //����һԺ    ����ca
    public  class xm1y
    {
        [DllImport("winspool.drv")]
        public static extern bool SetDefaultPrinter(String Name);
        [DllImport("KTSofDll.dll", EntryPoint = "GetUserList")]
        public extern static string GetUserList();
        [DllImport("KTSofDll.dll", EntryPoint = "Login")]
        public extern static int Login(StringBuilder id, StringBuilder pwd);
        [DllImport("KTSofDll.dll", EntryPoint = "GetSignCert")]
        public extern static StringBuilder GetSignCert(String certid);

        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");   
        private static IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");

        public string ca(string yhxx)
        {
            //-------��ȡsz�����õĲ���---------------------
            string msg = f.ReadString("CA", "msg", "");
               string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                string ftpUserID = f.ReadString("ftp", "user", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "");
                string ftpRemotePath = f.ReadString("ftp", "szqmPath", "pathimages/szqm");
                string key = f.ReadString("CA", "key", "1");

            //�����ӡ��
                string ca_pri = f.ReadString("CA", "mrdyj", "Foxit PDF Printer");
                if (ca_pri.Trim() == "")
                    ca_pri = "Foxit PDF Printer";
            string getblh = "";
            string type = "";
            string type2 = "";
            string yhm = "";

            string yhmc = "";
            string yhbh = "";
            string yhmm = "";
            string bglx = "";
            string bgxh = "";
            string keyname = "";
         
            string[] getyhxx = yhxx.Split('^');
            if (getyhxx.Length == 5)
            {
                type = getyhxx[0];
                yhm = getyhxx[1];
                yhmc = getyhxx[3];
                yhbh = getyhxx[2];
                yhmm = getyhxx[4];
            }
            else
            {
                type2 = getyhxx[0];
                getblh = getyhxx[1];
                bgxh = getyhxx[2];
                bglx = getyhxx[3];
                type = getyhxx[4];
                yhm = getyhxx[5];
                yhmc = getyhxx[6];
                yhbh = getyhxx[7];
                yhmm = getyhxx[8];
            }


            if (key == "0")
            {
                return "1";
            }

         
          //------------------------------------------------------------------
          //------------------------------------------------------------------
          //���ǰִ�� ,��֤KEY
          //------------------------------------------------------------------
          //------------------------------------------------------------------
         
            if (type == "SH")
            {
               
                //��֤key
              string  rtn_username=KEY_YZ();
             
              if (rtn_username == "-1")
              {
                  return "0";
              }

              if (yhmc.Trim() != rtn_username.Trim())
              {
                  MessageBox.Show("��¼ϵͳ�û���KEY�������߲�һ�£�������ˣ�");
                  MessageBox.Show("��¼ϵͳ�û�:" + yhmc.Trim() + "\r\nKEY��������:" + rtn_username.Trim());
                  return "0";
              }
              
                //�ж������ӡ���Ƿ����

                //��ȡĬ�ϴ�ӡ��
                PrintDocument fPrintDocument = new PrintDocument();
            
                string mrpri = fPrintDocument.PrinterSettings.PrinterName;
                //�ж�Ĭ�ϴ�ӡ���ǲ��������ӡ��
                if (mrpri != ca_pri)
                {
                    int x = 0;
                    foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                    {
                        if (fPrinterName == ca_pri)
                        {
                            x = 1;
                            break;
                        }
                    }
                    if (x != 1)
                    {
                        MessageBox.Show("δ�ҵ������ӡ��Foxit PDF Printer����������ǩ��");
                        return "0";
                    }
                }      
          }
            //***************************************************************
            ////��˺�ִ�У�����ǩ��������pdf��edu�ļ������ϴ���д���ݿ��
           
            if (type == "QZ")
            {
                //***************************************************************
                 //--����pdf�ļ�
                //MessageBox.Show("���ǩ��");
                //////���ô�ӡ����
              //��ȡĬ�ϴ�ӡ��
                PrintDocument fPrintDocument = new PrintDocument();
               
            string mrpri = fPrintDocument.PrinterSettings.PrinterName;
              //�ж�Ĭ�ϴ�ӡ���ǲ��������ӡ��
            if (mrpri != ca_pri)
            {
                int x = 0;
                foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
                {
                    if (fPrinterName == ca_pri)
                    {
                        x = 1;
                        break;
                    }
                }
                if (x == 1)
                {
                    SetDefaultPrinter(ca_pri);
                }
                else
                {
                    MessageBox.Show("δ�ҵ������ӡ��Foxit PDF Printer����������ǩ��");
                    return "0";
                }
            }
            else
            {
            }
            if (!System.IO.Directory.Exists(@"c:\temp\pdf\"))
                System.IO.Directory.CreateDirectory(@"c:\temp\pdf\");
            else
            {
                //ɾ�����Ŀ¼�µ������ļ�
                if (Directory.GetFiles(@"c:\temp\pdf\").Length > 0)
                {
                    foreach (string var in Directory.GetFiles(@"c:\temp\pdf\"))
                    {
                        File.Delete(var);
                    }
                }
            }  
            if (!System.IO.Directory.Exists(@"c:\temp\szqm\"))
                System.IO.Directory.CreateDirectory(@"c:\temp\szqm\");
            else
            {
                //ɾ�����Ŀ¼�µ������ļ�
                if (Directory.GetFiles(@"c:\temp\szqm\").Length > 0)
                {
                    foreach (string var in Directory.GetFiles(@"c:\temp\szqm\"))
                    {
                     
                        File.Delete(var);
                    }
                }
            }

              //���ͱ����ʽ�������ӡ��������pdf��

            mdpdf dd = new mdpdf();
            string name = "";
            dd.BMPTOJPG(getblh, ref name, bglx, bgxh);
             //���ͺ󣬻�ԭĬ�ϴ�ӡ��
              SetDefaultPrinter(mrpri);


              if (System.IO.File.Exists(@"c:\temp\pdf\szqm.pdf"))
              {
                  try
                  {
                      FileInfo aa = new FileInfo(@"c:\temp\pdf\szqm.pdf");
                      aa.MoveTo(@"C:\temp\szqm\"+getblh + "_" + bgxh + "_" + bglx + ".pdf");
                  }
                  catch
                  {
                      MessageBox.Show("�����ļ�����,��������ǩ�������������");
                      return "0";
                  }
              }
              else
              {
                  MessageBox.Show("pdf����ʧ�ܣ���������ǩ�������������"); return "0";

              }
              //MessageBox.Show("ǩ����ʼ");
              Process.Start("d:\\pathqc\\XM1Y_CA_SHOW.exe", yhxx);
                return "1";
            }

            //***************************************************************
            //////ȡ�����ǰ
            if (type == "QXSH")  
            {
                  
              }
           



            //***************************************************************
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //////ȡ����˺� ,ɾ��ftp�ϵ� edc�ļ���ͬʱɾ�����ݿ�ca��¼
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            if (type == "QXQZ" )  //&& (bglx == "BC" || bglx == "BD")
            {
             
            FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
            string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            try
            {
                //�ж�ftp���Ƿ���ڸ�edc�ļ�
                //if (fw.fileCheckExist(ftpURI, getblh + "_" + bgxh + "_" + bglx + ".edc"))
                //{
                //    //ɾ��ftp�ϵ�edc�ļ�
                //    fw.fileDelete(ftpURI, getblh + "_" + bgxh + "_" + bglx + ".edc").ToString();
                //}
               
            }
            catch
            {
                return "ɾ��FTP��edc�ļ��쳣����������ˣ�";
            }


            //д·�������ݿ�
            DBdata db = new DBdata("pathnet_ca");
            try
            {
                string sqlstr = "delete  T_CAXX where  blh='" + getblh + "'  and  bglx='" + bglx + "'  and  bgxh='" + bgxh + "'";

                int x = db.Execute_sql(sqlstr);
               

            }
            catch (Exception ee)
            {
              
              MessageBox.Show("ɾ���м������ʧ��");
            }

           }
            return "1";
           }

        public string KEY_YZ()
        {
            string strIP = f.ReadString("CA", "ip", "192.0.1.214:101");
            string strAppweb = f.ReadString("CA", "Appweb", "web");
            if (strIP.Trim() == "")
                strIP = "192.0.1.214:101";
            if (strAppweb.Trim() == "")
                strAppweb = "web";
            try
            {
                AxKTSOFLib.AxKTSOF axKTSOF1 = new AxKTSOFLib.AxKTSOF();
                ((System.ComponentModel.ISupportInitialize)(axKTSOF1)).BeginInit();
                GroupBox groupBox1 = new System.Windows.Forms.GroupBox();
                groupBox1.Controls.Add(axKTSOF1);
                ((System.ComponentModel.ISupportInitialize)(axKTSOF1)).EndInit();
                string _strSignData = string.Empty;
                string _strCertData = string.Empty;

                int _iError = 0;
                string _strError = string.Empty;
                SOFInterfaceClass objSOF = null;
              
                int nPort;

                objSOF = new SOFInterfaceClass();

                objSOF.SOF_PKIOpen(strIP, ref _iError);
                if (_iError != 0)
                {
                    objSOF.SOF_GetLastErrorString(ref _strError);
                    MessageBox.Show(_strError);
                    return "-1";
                }
                objSOF.SOF_SetWebAppName(strAppweb, ref _iError);
                if (_iError != 0)
                {
                    objSOF.SOF_GetLastErrorString(ref _strError);
                    MessageBox.Show(_strError);
                    return "-1";
                }
                string strRandom = string.Empty;
                objSOF.SOF_GenRandom(6, ref strRandom);
                string UserList = axKTSOF1.SOF_GetUserList();
                string[] spl1 = new string[] { "&&&" };
                string[] spl2 = new string[] { "||" };
                string[] asUserList = UserList.Split(spl1, StringSplitOptions.RemoveEmptyEntries);  //��Բ���key�����⣬demoֻ��ʾ����keyǩ�����
                if (asUserList.Length == 0)
                {
                    MessageBox.Show("δ��⵽KEY�̣���ȷ��KEY���Ƿ����!");
                    return "-1";
                }
                foreach (string strtemp in asUserList)
                {
                    string[] asUserList2 = strtemp.Split(spl2, StringSplitOptions.RemoveEmptyEntries);
                    _iError = axKTSOF1.SOF_Login("", "");
                    if (_iError != 0)  //ʹ��ǰ�����ȵ�½֤�飡�ɹ�����0��ʧ�ܷ���ʣ��������������� -1
                    {
                        MessageBox.Show("KEY����֤ʧ�ܻ������������!");
                        break;
                    }
                    _strSignData = axKTSOF1.SOF_SignData(asUserList2[1], ""); //������ǩ��
                    _strCertData = axKTSOF1.SOF_ExportUserCert(asUserList2[1]);  //����֤��

                }
                if (_iError != 0)
                {
                    return "-1";
                }
                string strCertUserName = string.Empty;
                objSOF.SOF_GetCertInfo(_strCertData, (int)CertSign.SGD_GET_CERT_SUBJECT_CN, ref strCertUserName);
                return strCertUserName;
            }
            catch (Exception eee)
            {
                MessageBox.Show("��ȡKEY�û��쳣���쳣���룺"+eee.ToString());
                return "-1";
            }
        }
        enum CertSign : byte  //���ڱ�ʶ����ȡ��֤����Ϣ
        {
            SGD_GET_CERT_VERSION = 0x00000001,//֤��汾
            SGD_GET_CERT_SERIAL = 0x00000002,//֤�����к�
            SGD_GET_CERT_SIGNALG = 0x00000003,//֤��ǩ���㷨��ʶ
            SGD_GET_CERT_ISSUER_C = 0x00000004,//֤��䷢�߹���(C)
            SGD_GET_CERT_ISSUER_O = 0x00000005,//֤��䷢����֯��(O)
            SGD_GET_CERT_ISSUER_OU = 0x00000006,//֤��䷢�߲�����(OU)
            SGD_GET_CERT_ISSUER_S = 0x00000007,//֤��䷢�����ڵ�ʡ����������ֱϽ��(S)
            SGD_GET_CERT_ISSUER_CN = 0x00000008,//֤��䷢��ͨ������(CN)
            SGD_GET_CERT_ISSUER_L = 0x00000009,//֤��䷢�����ڵĳ��С�����(L)
            SGD_GET_CERT_ISSUER_E = 0x00000010,//֤��䷢��Email
            SGD_GET_CERT_NOTBEFORE = 0x00000011,//֤����Ч�ڣ���ʼ����
            SGD_GET_CERT_AFTER = 0x00000012,//֤����Ч�ڣ���ֹ����
            SGD_GET_CERT_SUBJECT_C = 0x00000013,//֤��ӵ���߹���(C )
            SGD_GET_CERT_SUBJECT_O = 0x00000014,//֤��ӵ������֯��(O)
            SGD_GET_CERT_SUBJECT_OU = 0x00000015,//֤��ӵ���߲�����(OU)
            SGD_GET_CERT_SUBJECT_S = 0x00000016,//֤��ӵ�������ڵ�ʡ����������ֱϽ��(S)
            SGD_GET_CERT_SUBJECT_CN = 0x00000017,//֤��ӵ����ͨ������(CN)
            SGD_GET_CERT_SUBJECT_L = 0x00000018,//֤��ӵ�������ڵĳ��С�����(L)
            SGD_GET_CERT_SUBJECT_E = 0x00000019,//֤��ӵ����Email
            SGD_GET_CERT_ISSUER_DN = 0x00000020,//֤��䷢��DN
            SGD_GET_CERT_SUBJECT_DN = 0x00000021,//֤��ӵ����DN
            SGD_GET_CERT_DER_PUBKEY = 0x00000022,//֤�鹫Կ��Ϣ
            SGD_GET_CERT_DER_EXTENSIONS = 0x00000023,//֤����չ����Ϣ
            SGD_EXT_AUTHORITYKEYIDENTIFIER = 0x00000024,//�䷢����Կ��ʶ��
            SGD_EXT_SUBJECTKEYIDENTIFIER = 0x00000025,//֤���������Կ��ʶ��
            SGD_EXT_KEYUSAGE = 0x00000026,	  //��Կ��;
            SGD_EXT_PRIVATEKEYUSAGEPERIO = 0x00000027,//˽Կ��Ч��
            SGD_EXT_CERTIFICATEPOLICIES = 0x00000028,//֤�����
            SGD_EXT_POLICYMAPPINGS = 0x00000029,//����ӳ��
            SGD_EXT_BASICCONSTRAINTS = 0x00000030,//��������
            SGD_EXT_POLICYCONSTRAINTS = 0x00000031,//��������
            SGD_EXT_EXTKEYUSAGE = 0x00000032,//��չ��Կ��;
            SGD_EXT_CRLDISTRIBUTIONPO = 0x00000033,//CRL������
            SGD_EXT_NETSCAPE_CERT_TYPE = 0x00000034,//netscape����
            SGD_EXT_CERT_UNIQUEID = 0x00000035,//֤��ʵ��Ψһ��ʶ
            SGD_EXT_IDENTIFYCARDNUMBER = 0x00000036,//�������֤����
            SGD_EXT_INURANCENUMBER = 0x00000037,//������ᱣ�պ�
            SGD_EXT_ICREGISTRATIONNUMBER = 0x00000038,//��ҵ����ע���
            SGD_EXT_ORGANIZATIONCODE = 0x00000039,//��ҵ��֯��������
            SGD_EXT_TAXATIONNUMBER = 0x00000040	//˰��Ǽ�֤��
        }

    }
}
