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
    //厦门一院    凯特ca
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
            //-------获取sz中设置的参数---------------------
            string msg = f.ReadString("CA", "msg", "");
               string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                string ftpUserID = f.ReadString("ftp", "user", "");
                string ftpPassword = f.ReadString("ftp", "pwd", "");
                string ftpRemotePath = f.ReadString("ftp", "szqmPath", "pathimages/szqm");
                string key = f.ReadString("CA", "key", "1");

            //虚拟打印机
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
          //审核前执行 ,验证KEY
          //------------------------------------------------------------------
          //------------------------------------------------------------------
         
            if (type == "SH")
            {
               
                //验证key
              string  rtn_username=KEY_YZ();
             
              if (rtn_username == "-1")
              {
                  return "0";
              }

              if (yhmc.Trim() != rtn_username.Trim())
              {
                  MessageBox.Show("登录系统用户与KEY盘所有者不一致，不能审核！");
                  MessageBox.Show("登录系统用户:" + yhmc.Trim() + "\r\nKEY盘所有者:" + rtn_username.Trim());
                  return "0";
              }
              
                //判断虚拟打印机是否存在

                //获取默认打印机
                PrintDocument fPrintDocument = new PrintDocument();
            
                string mrpri = fPrintDocument.PrinterSettings.PrinterName;
                //判断默认打印机是不是虚拟打印机
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
                        MessageBox.Show("未找到虚拟打印机Foxit PDF Printer，不能数字签名");
                        return "0";
                    }
                }      
          }
            //***************************************************************
            ////审核后执行，数字签名，生成pdf和edu文件，并上传，写数据库等
           
            if (type == "QZ")
            {
                //***************************************************************
                 //--生成pdf文件
                //MessageBox.Show("审核签字");
                //////设置打印机。
              //获取默认打印机
                PrintDocument fPrintDocument = new PrintDocument();
               
            string mrpri = fPrintDocument.PrinterSettings.PrinterName;
              //判断默认打印机是不是虚拟打印机
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
                    MessageBox.Show("未找到虚拟打印机Foxit PDF Printer，不能数字签名");
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
                //删除这个目录下的所有文件
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
                //删除这个目录下的所有文件
                if (Directory.GetFiles(@"c:\temp\szqm\").Length > 0)
                {
                    foreach (string var in Directory.GetFiles(@"c:\temp\szqm\"))
                    {
                     
                        File.Delete(var);
                    }
                }
            }

              //发送报告格式给虚拟打印机，生成pdf文

            mdpdf dd = new mdpdf();
            string name = "";
            dd.BMPTOJPG(getblh, ref name, bglx, bgxh);
             //发送后，还原默认打印机
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
                      MessageBox.Show("复制文件错误,不能数字签名，请重新审核");
                      return "0";
                  }
              }
              else
              {
                  MessageBox.Show("pdf生成失败，不能数字签名，请重新审核"); return "0";

              }
              //MessageBox.Show("签名开始");
              Process.Start("d:\\pathqc\\XM1Y_CA_SHOW.exe", yhxx);
                return "1";
            }

            //***************************************************************
            //////取消审核前
            if (type == "QXSH")  
            {
                  
              }
           



            //***************************************************************
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //////取消审核后 ,删除ftp上的 edc文件，同时删除数据库ca记录
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            if (type == "QXQZ" )  //&& (bglx == "BC" || bglx == "BD")
            {
             
            FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);
            string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            try
            {
                //判断ftp上是否存在该edc文件
                //if (fw.fileCheckExist(ftpURI, getblh + "_" + bgxh + "_" + bglx + ".edc"))
                //{
                //    //删除ftp上的edc文件
                //    fw.fileDelete(ftpURI, getblh + "_" + bgxh + "_" + bglx + ".edc").ToString();
                //}
               
            }
            catch
            {
                return "删除FTP上edc文件异常，请重新审核！";
            }


            //写路径到数据库
            DBdata db = new DBdata("pathnet_ca");
            try
            {
                string sqlstr = "delete  T_CAXX where  blh='" + getblh + "'  and  bglx='" + bglx + "'  and  bgxh='" + bgxh + "'";

                int x = db.Execute_sql(sqlstr);
               

            }
            catch (Exception ee)
            {
              
              MessageBox.Show("删除中间表数据失败");
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
                string[] asUserList = UserList.Split(spl1, StringSplitOptions.RemoveEmptyEntries);  //针对插多个key的问题，demo只显示单个key签名结果
                if (asUserList.Length == 0)
                {
                    MessageBox.Show("未检测到KEY盘，请确认KEY盘是否插入!");
                    return "-1";
                }
                foreach (string strtemp in asUserList)
                {
                    string[] asUserList2 = strtemp.Split(spl2, StringSplitOptions.RemoveEmptyEntries);
                    _iError = axKTSOF1.SOF_Login("", "");
                    if (_iError != 0)  //使用前必须先登陆证书！成功返回0，失败返回剩余次数，锁死返回 -1
                    {
                        MessageBox.Show("KEY盘验证失败或密码输入错误!");
                        break;
                    }
                    _strSignData = axKTSOF1.SOF_SignData(asUserList2[1], ""); //对数据签名
                    _strCertData = axKTSOF1.SOF_ExportUserCert(asUserList2[1]);  //导出证书

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
                MessageBox.Show("获取KEY用户异常，异常代码："+eee.ToString());
                return "-1";
            }
        }
        enum CertSign : byte  //用于标识需提取的证书信息
        {
            SGD_GET_CERT_VERSION = 0x00000001,//证书版本
            SGD_GET_CERT_SERIAL = 0x00000002,//证书序列号
            SGD_GET_CERT_SIGNALG = 0x00000003,//证书签名算法标识
            SGD_GET_CERT_ISSUER_C = 0x00000004,//证书颁发者国家(C)
            SGD_GET_CERT_ISSUER_O = 0x00000005,//证书颁发者组织名(O)
            SGD_GET_CERT_ISSUER_OU = 0x00000006,//证书颁发者部门名(OU)
            SGD_GET_CERT_ISSUER_S = 0x00000007,//证书颁发者所在的省、自治区、直辖市(S)
            SGD_GET_CERT_ISSUER_CN = 0x00000008,//证书颁发者通用名称(CN)
            SGD_GET_CERT_ISSUER_L = 0x00000009,//证书颁发者所在的城市、地区(L)
            SGD_GET_CERT_ISSUER_E = 0x00000010,//证书颁发者Email
            SGD_GET_CERT_NOTBEFORE = 0x00000011,//证书有效期：起始日期
            SGD_GET_CERT_AFTER = 0x00000012,//证书有效期：终止日期
            SGD_GET_CERT_SUBJECT_C = 0x00000013,//证书拥有者国家(C )
            SGD_GET_CERT_SUBJECT_O = 0x00000014,//证书拥有者组织名(O)
            SGD_GET_CERT_SUBJECT_OU = 0x00000015,//证书拥有者部门名(OU)
            SGD_GET_CERT_SUBJECT_S = 0x00000016,//证书拥有者所在的省、自治区、直辖市(S)
            SGD_GET_CERT_SUBJECT_CN = 0x00000017,//证书拥有者通用名称(CN)
            SGD_GET_CERT_SUBJECT_L = 0x00000018,//证书拥有者所在的城市、地区(L)
            SGD_GET_CERT_SUBJECT_E = 0x00000019,//证书拥有者Email
            SGD_GET_CERT_ISSUER_DN = 0x00000020,//证书颁发者DN
            SGD_GET_CERT_SUBJECT_DN = 0x00000021,//证书拥有者DN
            SGD_GET_CERT_DER_PUBKEY = 0x00000022,//证书公钥信息
            SGD_GET_CERT_DER_EXTENSIONS = 0x00000023,//证书扩展项信息
            SGD_EXT_AUTHORITYKEYIDENTIFIER = 0x00000024,//颁发者密钥标识符
            SGD_EXT_SUBJECTKEYIDENTIFIER = 0x00000025,//证书持有者密钥标识符
            SGD_EXT_KEYUSAGE = 0x00000026,	  //密钥用途
            SGD_EXT_PRIVATEKEYUSAGEPERIO = 0x00000027,//私钥有效期
            SGD_EXT_CERTIFICATEPOLICIES = 0x00000028,//证书策略
            SGD_EXT_POLICYMAPPINGS = 0x00000029,//策略映射
            SGD_EXT_BASICCONSTRAINTS = 0x00000030,//基本限制
            SGD_EXT_POLICYCONSTRAINTS = 0x00000031,//策略限制
            SGD_EXT_EXTKEYUSAGE = 0x00000032,//扩展密钥用途
            SGD_EXT_CRLDISTRIBUTIONPO = 0x00000033,//CRL发布点
            SGD_EXT_NETSCAPE_CERT_TYPE = 0x00000034,//netscape属性
            SGD_EXT_CERT_UNIQUEID = 0x00000035,//证书实体唯一标识
            SGD_EXT_IDENTIFYCARDNUMBER = 0x00000036,//个人身份证号码
            SGD_EXT_INURANCENUMBER = 0x00000037,//个人社会保险号
            SGD_EXT_ICREGISTRATIONNUMBER = 0x00000038,//企业工商注册号
            SGD_EXT_ORGANIZATIONCODE = 0x00000039,//企业组织机构代码
            SGD_EXT_TAXATIONNUMBER = 0x00000040	//税务登记证号
        }

    }
}
