
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
using System.Xml;
using System.Xml.XPath;
using System.Data.Odbc;
using PathHISZGQJK;
namespace PathnetCAzgq
{
   
    class klmy
    {
        string appid = "8ac7328536f29d480137ea7b611e1454";
        string CN = string.Empty;
        string DN = string.Empty;
        string SN = string.Empty;
        string webservices = "http://10.71.178.5:8085/esaweb";
      
      
        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_TimeSign", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern string XJCA_TimeSign(string ip, string djid, string hash);

        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetSealBMPB", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern bool XJCA_GetSealBMPB(string filepath, int times);

        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_SignSeal", CharSet = CharSet.Ansi)]
        public static extern bool XJCA_SignSeal(string src, StringBuilder signxml, ref int len);


     //   private static IniFiles f = new IniFiles("sz.ini");
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public string ca(string yhxx)
        {
          

            //-------��ȡsz�����õĲ���---------------------
           // string msg = f.ReadString("CA", "msg", "");
           // string web = f.ReadString("CA", "webservices", "");
           // if (web.Trim() != "")
           //     webservices = web;
           // string appid_1 = f.ReadString("CA", "appid", "");


           // if (web.Trim() != "")
           //     appid = appid_1;

           // string getblh = "";
           // string type = "";
           // string type2 = "";
           // string yhm = "";

           // string yhmc = "";
           // string yhbh = "";
           // string yhmm = "";
           // string bglx = "";
           // string bgxh = "";

           // string sfzh = "";
           // string keyname = "";
            
           // string[] getyhxx = yhxx.Split('^');
           // if (getyhxx.Length == 5)
           // {
           //     type = getyhxx[0];
           //     yhm = getyhxx[1];
           //     yhmc = getyhxx[3];
           //     yhbh = getyhxx[2];
           //     yhmm = getyhxx[4];
           // }
           // else
           // {
           //     type2 = getyhxx[0];
           //     getblh = getyhxx[1];
           //     bgxh = getyhxx[2];
           //     bglx = getyhxx[3];
           //     type = getyhxx[4];
           //     yhm = getyhxx[5];
           //     yhmc = getyhxx[6];
           //     yhbh = getyhxx[7];
           //     yhmm = getyhxx[8];
           // }
       
           // //--------------��֤key���Ƿ����-------------------------------------------------------------------------------
           //     xjcaTechATLLib.xjcaTechATLLib xj = new xjcaTechATLLib.xjcaTechATLLib();
           // try
           //     {    
           //     ////���ؼ�
           //     // MessageBox.Show(xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString());
           //       //MessageBox.Show(xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString());
           
           //         //if (xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString() == "0")
           //         //{
           //         //    MessageBox.Show("����δ����");
           //         //    return "0";
           //         //}
           //         //if (xj.XJCA_KeyInsert("HaiTai Cryptographic Service Provider for xjca").ToString() == "1")
           //         //{
           //         //    MessageBox.Show("keyδ����");
           //         //    return "0";
           //         //}
           //     //����̬��
           //         if (XJCAverify.XJCA_CspInstalled() == false)
           //         {
           //             MessageBox.Show("����δ����");
           //             return "0";
           //         }
           //         if (XJCAverify.XJCA_KeyInserted() == false)
           //         {
           //             MessageBox.Show("keyδ����");
           //             return "0";
           //         }
           //     }
           //     catch
           //     {
                  
           //     }
                



           //// string dn = "";
           // string[] aaa_ca;
            
           // try
           // {    
           //      //����̬��
           //     //-----SN---
           //     StringBuilder sn_strB = new StringBuilder(100); 
           //     int sn_len=0;
           //     XJCAverify.XJCA_GetCertSN(2, sn_strB, ref sn_len);
           //     SN = sn_strB.ToString();
           //     //----DN--------
           //     StringBuilder dn = new StringBuilder(400);
           //     int dn_len = 0;
           //     XJCAverify.XJCA_GetCertDN(dn, ref dn_len);
                
           //    //���ؼ� 
           //     //---SN-----
           //     //string KeyID = xj.XJCA_GetCertSN();
           //     //----DN----------------
           //    // dn = xj.XJCA_GetCertDN();
           //     //if (KeyID.Trim() == "")
           //     //{
           //     //    MessageBox.Show("֤�����к�Ϊ��");
           //     //    return "0";
           //     //} 
 
           //     aaa_ca = dn.ToString().Split(',');
           // }
           // catch
           // {
           //     MessageBox.Show("KEY��δ���룬�����KEY�̣�����");
           //     return "0";
           // }
           // //----------------------------------------------
           // //--------------��ȡKEY���û���--------------------
           // ////----------------------------------------------
           // try
           // {     
           //     keyname = aaa_ca[aaa_ca.Length - 1].Substring(4);
           //     sfzh = aaa_ca[1].Substring(3);
                
           //     if (yhmc.Trim() != keyname)
           //     {
           //       MessageBox.Show("��ǰ��������KEY�û�����������ִ�д˲���������");
           //         return "0";
           //     }
           // }
           // catch
           // {
           //     MessageBox.Show("KEY��δ����");
           //     return "0";
           // }
              
           // ////------------------���֤����֤-----����������֤��Ϊ�գ���д��------------------
           // dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                      
           // DataTable  dt_yh= aa.GetDataTable("select F_SFZH from T_YH where F_YHMC='" + keyname + "'", "T_YH");

           // if (dt_yh.Rows.Count== 0)
           // {
           //     MessageBox.Show("��ǰ�����������KEY�û���ݲ���������ִ�д˲���������");
           //     return "0";
           // }
           // if (dt_yh.Rows[0]["F_SFZH"].ToString().Trim() == "")
           // {
           //     try
           //     {
           //         aa.ExecuteSQL("update  T_YH  set F_SFZH='" + sfzh.Trim() + "' where F_YHMC='" + keyname + "'");
           //     }
           //     catch
           //     {
                  
           //     }
           // }

           //if (dt_yh.Rows[0]["F_SFZH"].ToString().Trim() != sfzh.Trim())
           //{
             
           //     MessageBox.Show("��ǰ�����������KEY�û���ݲ���������ִ�д˲���������");
           //     return "0";
           // }
           // ////------------------------------------ 
           // ////----------------key������֤--------- 
           // ////------------------------------------ 
           
           // try{
                 
           //  //--��̬��---
           // StringBuilder signxml = new StringBuilder(10000);
           // string xmlstr="12345678";
           // int len=0;
           //  XJCA_SignSeal(xmlstr, signxml, ref len);
           //  //-�ؼ�--
           //   //string signxml = xj.XJCA_SignStr("HaiTai Cryptographic Service Provider for xjca", xmlstr);
            
           //  string[] getsignxml1 = signxml.ToString().Split(',');

           //  string signxml_str1 = getsignxml1[0].Trim();
           //  string cert1 = getsignxml1[1].Trim();

           //  if (signxml.ToString() == "" || cert1 == "" || signxml_str1=="")
           //  {
           //      MessageBox.Show("��֤δͨ��");
           //      return "0";
           //  }
             
           // }
           // catch
           // {
           //     MessageBox.Show("��֤�쳣");
           //     return "0";
           // }
          
           ////--------��֤���----------
           // //------------------------------
           ////-----------��ʼǩ��------------ //bglx = BC--���䱨��     type=QZ/QXQZ //bglx  =BD--����С����  type= QZ/QXQZ
               
           ////---------------------------

           // if (type == "QZ")
           // {
           //     string sql_str="select * from T_JCXX where  F_BLH='" + getblh + "'  and  F_BGZT='�����'";
                
           //     //�������
           //     if (bglx == "BC")
           //     {
           //         sql_str = "select * from V_BCBG_SH where  F_BLH='" + getblh + "' and F_BC_BGZT='�����'and F_BC_BGXH='" + bgxh + "'";
           //     }
           //     //С�������
           //     if (bglx == "BD")
           //     {
           //         sql_str = "select * from V_BDBG_bfk where  F_BLH='" + getblh + "' and  F_BD_BGZT='�����' and F_BD_BGXH='" + bgxh + "'";
           //     }


           //     DataTable dtyhbh = aa.GetDataTable(sql_str, "table");
           //     aa.Close();

           //     //----------------ʱ���-------------------------
           //     //ʱ��������ַ 
           //     //ҵ�񵥾ݺ�
           //    // string djid = dtyhbh.Rows[0]["F_BLH"].ToString();
           //     //�ؼ�����
           //     //string hash = dtyhbh.Rows[0]["F_BLZD"].ToString();
           //       //if (bglx == "BC")
           //       //     hash = dtyhbh.Rows[0]["F_BCZD"].ToString();
           //       //     if (bglx == "BD")
           //       //      hash = dtyhbh.Rows[0]["F_BDZD"].ToString();
           //     ////����ֵʱ���xml��TimeSign_XML����Ҫд�����ݿ�
           //     string TimeSign_XML = "";// XJCA_TimeSign(webservices, djid, hash);
                                     
           //     //MessageBox.Show("����ֵʱ���"+TimeSign_XML);
           //     ////------------��ǩ��ʱ��д�����ݿ� T_JCXX------------

           //     //XmlDocument TimeSign_xd = new XmlDocument();
           //     //TimeSign_xd.LoadXml(TimeSign_XML);
           //     //string TimeSign_time = TimeSign_xd.SelectSingleNode("/xjca").ChildNodes[3].InnerText.ToString();
           //     //if (type2 == "bd")
           //     //    aa.ExecuteSQL("update T_JCXX  set F_YL7='" + TimeSign_time + "' where F_BLH='" + getblh + "'");


           //     //ǩ��----------------------------------
           //     int len = 40000;
           //     StringBuilder signxml = new StringBuilder(40000);
           //     //ǩ��ԭ��
           //     string xmlstr = "";

           //     if (bglx == "BC")
           //     {
           //         xmlstr = "<BL><F_BLH>" + dtyhbh.Rows[0]["F_BLH"].ToString() + "</F_BLH>";
                   
           //         xmlstr = xmlstr + "<F_BCZD>" + dtyhbh.Rows[0]["F_BCZD"].ToString() + "</F_BCZD>";
           //         xmlstr = xmlstr + "<F_BC_BGYS>" + dtyhbh.Rows[0]["F_BC_BGYS"].ToString() + "</F_BC_BGYS>";
           //         xmlstr = xmlstr + "<F_BC_SHYS>" + dtyhbh.Rows[0]["F_BC_SHYS"].ToString() + "</F_BC_SHYS>";
           //         xmlstr = xmlstr + "<F_BC_BGRQ>" + dtyhbh.Rows[0]["F_BC_BGRQ"].ToString() + "</F_BC_BGRQ>";
           //         xmlstr = xmlstr + "<F_BC_BGXH>" + dtyhbh.Rows[0]["F_BC_BGXH"].ToString() + "</F_BC_BGXH></BL>";
           //     }
           //     else
           //     {
           //         //С�������
           //         if (bglx == "BD")
           //         {
           //             xmlstr = "<BL><F_BLH>" + dtyhbh.Rows[0]["F_BLH"].ToString() + "</F_BLH>";
           //             xmlstr = xmlstr + "<F_XM>" + dtyhbh.Rows[0]["F_XM"].ToString() + "</F_XM>";
           //             xmlstr = xmlstr + "<F_XB>" + dtyhbh.Rows[0]["F_XB"].ToString() + "</F_XB>";
           //             xmlstr = xmlstr + "<F_NL>" + dtyhbh.Rows[0]["F_NL"].ToString() + "</F_NL>";
           //             xmlstr = xmlstr + "<F_zyh>" + dtyhbh.Rows[0]["F_zyh"].ToString() + "</F_zyh>";
           //             xmlstr = xmlstr + "<F_BDZD>" + dtyhbh.Rows[0]["F_BDZD"].ToString() + "</F_BDZD>";
           //             xmlstr = xmlstr + "<F_BD_BGYS>" + dtyhbh.Rows[0]["F_BD_BGYS"].ToString() + "</F_BD_BGYS>";
           //             xmlstr = xmlstr + "<F_BD_SHYS>" + dtyhbh.Rows[0]["F_BD_SHYS"].ToString() + "</F_BD_SHYS>";
           //             xmlstr = xmlstr + "<F_BD_BGRQ>" + dtyhbh.Rows[0]["F_BD_BGRQ"].ToString() + "</F_BD_BGRQ>";
           //             xmlstr = xmlstr + "<F_BD_BGXH>" + dtyhbh.Rows[0]["F_BD_BGXH"].ToString() + "</F_BD_BGXH></BL>";
           //         }
           //         else
           //         {
           //             xmlstr="<BL><F_BLH>" + dtyhbh.Rows[0]["F_BLH"].ToString() + "</F_BLH>";
           //             xmlstr = xmlstr + "<F_sqxh>" + dtyhbh.Rows[0]["F_sqxh"].ToString() + "</F_sqxh>";
           //             xmlstr = xmlstr + "<F_XM>" + dtyhbh.Rows[0]["F_XM"].ToString() + "</F_XM>";
           //             xmlstr = xmlstr + "<F_XB>" + dtyhbh.Rows[0]["F_XB"].ToString() + "</F_XB>";
           //             xmlstr = xmlstr + "<F_NL>" + dtyhbh.Rows[0]["F_NL"].ToString() + "</F_NL>";
           //             xmlstr = xmlstr + "<F_zyh>" + dtyhbh.Rows[0]["F_zyh"].ToString() + "</F_zyh>";
           //             xmlstr = xmlstr + "<F_mzh>" + dtyhbh.Rows[0]["F_mzh"].ToString() + "</F_mzh>";
           //             xmlstr = xmlstr + "<F_ch>" + dtyhbh.Rows[0]["F_ch"].ToString() + "</F_ch>";
           //             xmlstr = xmlstr + "<F_blzd>" + dtyhbh.Rows[0]["F_blzd"].ToString() + "</F_blzd>";
           //             xmlstr = xmlstr + "<F_bgys>" + dtyhbh.Rows[0]["F_bgys"].ToString() + "</F_bgys>";
           //             xmlstr = xmlstr + "<F_bgrq>" + dtyhbh.Rows[0]["F_bgrq"].ToString() + "</F_bgrq>";
           //             xmlstr = xmlstr + "<F_shys>" + dtyhbh.Rows[0]["F_shys"].ToString() + "</F_shys>";
           //             xmlstr = xmlstr + "<F_spare5>" + dtyhbh.Rows[0]["F_spare5"].ToString() + "</F_spare5></BL>";
           //         }
           //     }
           //     if (xmlstr == "")
           //         return "0";

           //     try
           //     {
           //         //�ؼ�
           //         //  string   signxml= xj.XJCA_SignStr(appid,xmlstr);
                    
           //         // ��̬��
           //        //XJCA_SignSeal������signxml ����ֵ��ǩ�·��ص���Ϣ ����ǩ�����ݣ�֤�����ݣ�֤��id���ԡ���������������ǩ�����ݺ�֤��id��
           //         bool ca_yz = XJCA_SignSeal(xmlstr, signxml,ref len);
                      
           //         if (!ca_yz)
           //         {
           //             MessageBox.Show("ǩ�½��ʧ��");
           //             return "0";
           //         }
           //         else
           //         {
           //             // ǩ�·��ص���Ϣsignxml ����ǩ�����ݣ�֤�����ݣ�֤��id���ԡ���������������ǩ�����ݺ�֤��id
           //             string[] getsignxml = signxml.ToString().Split(',');
           //             //ǩ������

           //             // ǩ�·��ص���Ϣsignxml ����
           //             string signxml_str = getsignxml[0];
           //             //֤������
           //             string cert = getsignxml[1];
           //             //֤��id
           //             string certID = getsignxml[2];

           //             //--------��signxml,,TimeSign_XML�ȷ���ֵ �������ݿ�-------------
           //             dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");

           //             try
           //             {

           //                 string sqlstr = "insert into T_CAXX(blh,RQ,TimeSign_XML,signxml,BGNR,keyname,cert,certID,CAZT,BGLX,BGXH) values ('" + dtyhbh.Rows[0]["F_BLH"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TimeSign_XML + "','" + signxml_str + "','" + xmlstr + "','" + keyname + "','" + cert + "','" + certID + "','����֤','"+bglx+"','"+bgxh+"') ";
           //                 if (msg == "1")
           //                     MessageBox.Show(sqlstr);
           //                 int x = bb.ExecuteSQL(sqlstr);
           //                 bb.Close();
           //                 if (x < 1)
           //                 {
           //                     log.WriteMyLog("ǩ�½��ʧ��");
           //                     return "0";
           //                 }
           //                 if (msg == "1")
           //                     MessageBox.Show("���ݲ���ɹ�");
           //             }
           //             catch (Exception ee)
           //             {
           //                 bb.Close();
           //                 log.WriteMyLog("ǩ�½���쳣:���ݿ�д��ʧ��," + ee.ToString());
           //                 return "0";
           //             }

           //         }
           //     }
           //     catch(Exception  eee)
           //     {
           //         MessageBox.Show("ǩ���쳣"+eee.ToString());
           //         return "0";
           //     }

           //     try
           //     {
           //         //-��ȡǩ��ͼƬ----------------------
           //         bool ss = XJCA_GetSealBMPB("D:\\pathqc\\rpt-szqm\\YSBMP\\" + keyname + ".bmp", 1);
           //         if (ss)
           //         {
           //             //�ϴ�ǩ��ͼƬ��ftp---------------------
           //             string ftpServerIP = f.ReadString("ftp", "ftpip", "");
           //             string ftpUserID = f.ReadString("ftp", "user", "");
           //             string ftpPassword = f.ReadString("ftp", "pwd", "");
           //             string ftpRemotePath = f.ReadString("ftp", "szqmPath", "/setup/rpt/rpt-szqm/YSBMP/");

           //             string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
           //             string status = "";
           //             FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

           //             fw.Upload("D:\\pathqc\\rpt-szqm\\YSBMP\\" + keyname + ".bmp", "", out status);
           //             if (status == "Error")
           //             {
           //                 MessageBox.Show("ǩ��ͼƬ�ϴ�ʧ�ܣ���������ˣ�");
           //                 return "0";
           //             }
                     
           //             //-------�ϴ���ά�뵽ftp-------------
           //             DirectoryInfo dir = new DirectoryInfo(@"D:\pathqc\rpt-szqm\MakePdfFile\");
           //             foreach (FileInfo ff in dir.GetFiles("*.*"))
           //                 ff.Delete();
                      
           //             StringBuilder M_signxml = new StringBuilder(xmlstr,5000);
           //             XJCAverify.XJCA_MakePdf417ToFile(M_signxml, M_signxml.Length, "D:\\pathqc\\rpt-szqm\\MakePdfFile\\" + getblh + "_" + bgxh + "_" + bglx + ".BMP", 7, 3, 3, 0);

           //             string ftpRemotePath2 = "/pathimages/MakePdfFile/";
                      
           //             string status2 = "";
           //             FtpWeb fw2 = new FtpWeb(ftpServerIP, ftpRemotePath2, ftpUserID, ftpPassword);

           //             fw2.Upload("D:\\pathqc\\rpt-szqm\\MakePdfFile\\" + getblh + "_" + bgxh + "_" + bglx + ".BMP", "", out status);
           //             if (status2 == "Error")
           //             {                                                              
           //                 MessageBox.Show("��ά��ͼƬ�ϴ�ʧ�ܣ���������ˣ�");
           //                 return "0";
           //             }
                      
           //             return "1";
           //         }
           //         else
           //         {
           //             MessageBox.Show("��ȡǩ��ͼƬʧ�ܣ�����");
           //             return "0";
           //         }
           //     }
           //     catch 
           //     {
                   
           //         log.WriteMyLog("����ǩ��ͼƬ���ά��ͼƬ�쳣");
           //         return "0";
           //     }
             
           // }
           // if (type == "QXSH"  )
           // {
           //     aa.Close();

           //     dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");
           //     try
           //     {
           //         string sqlstr = "delete  T_CAXX where blh='" + getblh + "'  and BGLX='"+bglx.Trim()+"' and BGXH='"+bgxh+"'";
           //         int x = bb.ExecuteSQL(sqlstr);
           //         bb.Close();

           //         return "1";
           //     }
           //     catch (Exception eee)
           //     {
           //         bb.Close();
           //         MessageBox.Show(eee.ToString());
           //         return "0";
           //     }

           // }
           // if (type == "QXQZ"  && (bglx == "BC" || bglx == "BD"))
           // {
           //     aa.Close();

           //     dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");
           //     try
           //     {
           //         string sqlstr = "delete  T_CAXX where blh='" + getblh + "'  and BGLX='" + bglx.Trim() + "' and BGXH='" + bgxh + "'";
           //         int x = bb.ExecuteSQL(sqlstr);
           //         bb.Close();

           //         return "1";
           //     }
           //     catch (Exception eee)
           //     {
           //         bb.Close();
           //         MessageBox.Show(eee.ToString());
           //         return "0";
           //     }
            

           // }
            
           // aa.Close();
        
            return "1";
        }
        
        //   public static StringBuilder Getseal(string src, StringBuilder sinxml, ref int len)
        //{
        //    if (XJCA_SignSeal(src, sinxml, ref len))
        //    {
        //        return sinxml;
        //    }
        //    return null;
        //}
         }
}
