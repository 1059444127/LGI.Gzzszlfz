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
using HL7;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Data.Odbc;
using PathHISZGQJK;
namespace PathnetCAzgq
{
    //��ҽ��һ��Ժ
    class xydyfy
    {
        string appid = "2c9484f231dca3ad0131f9cc15690029";
        string CN = string.Empty;
        string DN = string.Empty;
        string SN = string.Empty;
        string webservice = "http://172.20.89.23:8080/webServices/authService";
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_TimeSign", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern StringBuilder XJCA_TimeSign(string ip, string djid, StringBuilder hash);

        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_SignSeal", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern bool XJCA_SignSeal(string src, StringBuilder signxml,ref int len);


        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetSealBMPB", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern bool XJCA_GetSealBMPB(string filepath, int times);

        [DllImport("xjcaDTS.dll", EntryPoint = "AZT_TimeSign", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern void AZT_TimeSign(string ip, string djid,ref string hash);

        private static IniFiles f = new IniFiles("sz.ini");

        public string ca(string yhxx)
        {
            string webservices = "";
            //-------��ȡsz�����õĲ���---------------------
            string msg = f.ReadString("CA", "msg", "");
            string web = f.ReadString("CA", "webservices", "");
            if (web.Trim() != "")
                webservices = web;
            string appid_1 = f.ReadString("CA", "appid", "");
            if (web.Trim() != "")
                appid = appid_1;

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


            //Login_CA("SHQTYSZQZ^" + psCURBLH + "^1^CG^QXSH^" + psYHM + "^" + psYHMC + "^" + psYHBH + "^" + psYHMM)
            // ��ȡkey����Ϣ---------------------------------------------------------------------------------------------
            xjcaTechATLLib.xjcaTechATLLib xj = new xjcaTechATLLib.xjcaTechATLLib();
            try
            {
                ////���ؼ�
                // MessageBox.Show(xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString());
                //MessageBox.Show(xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString());

                //if (xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString() == "0")
                //{
                //    MessageBox.Show("����δ����");
                //    return "0";
                //}
                //if (xj.XJCA_KeyInsert("HaiTai Cryptographic Service Provider for xjca").ToString() == "1")
                //{
                //    MessageBox.Show("keyδ����");
                //    return "0";
                //}
                //����̬��
                if (XJCAverify.XJCA_CspInstalled() == false)
                {
                    MessageBox.Show("����δ����");
                    return "0";
                }
                if (XJCAverify.XJCA_KeyInserted() == false)
                {
                    MessageBox.Show("keyδ����");
                    return "0";
                }
            }
            catch
            { }


            //-------------��֤key���Ƿ����-------------------------------------------------------------------------------
            string dn = "";
            string[] aaa_ca;
            try
            {
                ////����̬��
                ////-----SN---
                //StringBuilder sn_strB = new StringBuilder(100);
                //int sn_len = 0;
                //XJCAverify.XJCA_GetCertSN(2, sn_strB, ref sn_len);
                //SN = sn_strB.ToString();
                ////----DN--------
                //StringBuilder dn = new StringBuilder(400);
                //int dn_len = 0;
                //XJCAverify.XJCA_GetCertDN(dn, ref dn_len);

                //���ؼ� 
                //---SN-----
                string KeyID = xj.XJCA_GetCertSN();
                //----DN----------------
                dn = xj.XJCA_GetCertDN();
                if (KeyID.Trim() == "")
                {
                    MessageBox.Show("֤�����к�Ϊ��");
                    return "0";
                }
                if (dn.Trim() == "10011")
                {
                    MessageBox.Show("KEY��δ���룬�����KEY�̣�����");
                    return "0";
                }
                aaa_ca = dn.ToString().Split(',');
            }
            catch
            {
                MessageBox.Show("KEY��δ���룬�����KEY�̣�����");
                return "0";
            }

            //----------------------------------------------
            //--------------��ȡKEY���û���--------------------
            //----------------------------------------------
            try
            {
                keyname = aaa_ca[aaa_ca.Length - 1].Substring(4);
                if (keyname.Contains("."))
                {
                    if (!keyname.Contains(yhmc))
                    {
                        MessageBox.Show("��ǰ��������KEY�û�����������ִ�д˲���������");
                        return "0";
                    }
                }
                else
                {
                    if (yhmc.Trim() != keyname)
                    {
                        MessageBox.Show("��ǰ��������KEY�û�����������ִ�д˲���������");
                        return "0";
                    }
                }
            }
            catch
            {
                MessageBox.Show("KEY��δ����");
                return "0";
            }
            try
            {
                keyname = aaa_ca[aaa_ca.Length - 1].Substring(4);
                string keyYHGH = aaa_ca[1].Substring(3);

                string xml = xj.XJCA_CertAuth(webservice, appid, keyname);

                if (xml == "")
                {
                    MessageBox.Show("KEY����֤ʧ��,�����Ǵ�KEY�û�δע�ᣡ");
                    return "0";
                }

                XmlNode xmlok = null;
                XmlDocument xd = new XmlDocument();

                xd.LoadXml(xml);
                xmlok = xd.SelectSingleNode("/XJCA_CertAppAuth");
                string bool_key = xmlok["success"].InnerText.ToString();
                //----------------------------------------------
                //---------------������Ƿ���KEY��������--------------------

                if (bool_key == "false")
                {
                    MessageBox.Show("��֤ʧ�ܣ���������Ȩ���������");
                    return "0";
                }
                if (keyname.Contains("."))
                    keyname = keyname.Substring(0, keyname.IndexOf('.'));

                if (yhmc.Trim() != keyname)
                {

                    MessageBox.Show("��ǰ��������KEY�û�����������ִ�д˲���������");
                    return "0";
                }
                //------------------������֤-----------------------


                if (keyYHGH.Trim() != yhbh.Trim())
                {
                    MessageBox.Show("��ǰ�����˹�����KEY�û����Ų���������ִ�д˲���������");
                    return "0";
                }
            }
            catch
            {
                MessageBox.Show("��ȡKEY���û��쳣");
                return "0";

            }




            ////------------------------------------ 
            ////----------------key������֤--------- 
            ////------------------------------------ 
            if (msg == "1")
                MessageBox.Show("������֤��ʼ");

            try
            {

                //--��̬��---
                StringBuilder signxml = new StringBuilder(5000);
                string xmlstr = "12345";
                int len = 0;
                bool ca_yz = XJCA_SignSeal(xmlstr, signxml, ref len);
                if (!ca_yz)
                {
                    if (msg == "1")
                        MessageBox.Show("��֤δͨ��");
                    return "0";
                }
                //-�ؼ�--
                //string signxml = xj.XJCA_SignStr("HaiTai Cryptographic Service Provider for xjca", xmlstr);

                //string[] getsignxml1 = signxml.ToString().Split(',');
                // string signxml_str1 = getsignxml1[0].Trim();
                //if (signxml.ToString() == "" || cert1 == "" || signxml_str1 == "")
                //{
                //    MessageBox.Show("��֤δͨ��");
                //    return "0";
                //}

            }
            catch (Exception eeee)
            {
                log.WriteMyLog(eeee.ToString());
                MessageBox.Show("��֤�쳣1");
                return "0";
            }

            if (msg == "1")
                MessageBox.Show("������֤���");
            //--------��֤���----------
            //-----------��ʼǩ��------------


            /////////////////////////
            //***********************\
            /////////////////////////

            if (type == "QZ")
            {
                if (msg == "1")
                    MessageBox.Show("��ʼǩ��������");

                string sql_str = "select * from T_JCXX where  F_BLH='" + getblh + "'  and  F_BGZT='�����'";

                //�������
                if (bglx == "BC")
                {
                    sql_str = "select * from V_BCBG_SH where  F_BLH='" + getblh + "' and F_BC_BGZT='�����'and F_BC_BGXH='" + bgxh + "'";
                }
                //С�������
                if (bglx == "BD")
                {
                    sql_str = "select * from V_BDBG_bfk where  F_BLH='" + getblh + "' and  F_BD_BGZT='�����' and F_BD_BGXH='" + bgxh + "'";

                }


                DataTable dtyhbh = aa.GetDataTable(sql_str, "table");
                aa.Close();

                if (msg == "1")
                    MessageBox.Show("��ʼʱ���������");
                //-----------------ǩ����֤-----------------
                //----------------------------------------------
                //----------------ʱ���-------------------------


                //ʱ��������ַ 
                string IP = "http://172.20.89.23:8080/xjcaTimestamp/services/sign";

                //ҵ�񵥾ݺ�
                string djid = getblh + "^1^CG";
                //�ؼ�����
                string hash = "";

                if (bglx == "BC")
                {
                    hash = dtyhbh.Rows[0]["F_bczd"].ToString().Trim();
                    djid = getblh + "^" + dtyhbh.Rows[0]["F_BC_BGXH"].ToString().Trim() + "^BC";
                }
                else
                {
                    if (bglx == "BD")
                    {
                        hash = dtyhbh.Rows[0]["F_bdzd"].ToString().Trim();
                        djid = getblh + "^" + dtyhbh.Rows[0]["F_BD_BGXH"].ToString().Trim() + "^BD";
                    }
                    else
                    {
                        hash = dtyhbh.Rows[0]["F_blzd"].ToString().Trim();
                        djid = getblh + "^1^CG";
                    }
                }
                if (msg == "1")
                    MessageBox.Show(djid + "\n" + hash);
                //����ֵʱ���xml��TimeSign_XML����Ҫд�����ݿ�
                MessageBox.Show("1");
               // StringBuilder md5 = new StringBuilder(Getmd5(hash));
               // MessageBox.Show("2");
             //   StringBuilder TimeSign_XML = new StringBuilder(); 
               //  TimeSign_XML = XJCA_TimeSign(IP, djid, md5);
                string TimeSign_XML = "";
                string md5 = Getmd5(hash);
                MessageBox.Show(md5);
                try
                {
                    AZT_TimeSign(IP, djid,ref md5);
                }catch(Exception  ee)
                {
                    MessageBox.Show(ee.ToString());
                }
                MessageBox.Show(md5);
                MessageBox.Show("3");
                if (msg == "1")
                {
                   // MessageBox.Show("ע�⣺");
                    MessageBox.Show(TimeSign_XML.ToString());
                   // MessageBox.Show(md5.ToString());
                }
                try
                {

                    //------------��ǩ��ʱ��д�����ݿ� T_JCXX------------
                    XmlDocument TimeSign_xd = new XmlDocument();
                    TimeSign_xd.LoadXml(TimeSign_XML.ToString());
                    string TimeSign_time = TimeSign_xd.SelectSingleNode("/xjca").ChildNodes[3].InnerText.ToString();
                    aa.ExecuteSQL("update T_JCXX  set F_YL7='" + TimeSign_time + "' where F_BLH='" + getblh + "'");

                }
                catch (Exception des)
                {
                    log.WriteMyLog("��ǩ��ʱ��д�����ݿ� T_JCXX�쳣");
                }
                if (msg == "1")
                    MessageBox.Show("ʱ������");
                //------------------------------------------------------
                //---------------ǩ��----------------------------------
                //ǩ��----------------------------------
                if (msg == "1")
                    MessageBox.Show("��ʼǩ�¡�����");
                int len = 40000;
                StringBuilder signxml = new StringBuilder(40000);
                //ǩ��ԭ��
                string xmlstr = "";
                if (msg == "1")
                    MessageBox.Show(bglx);
                if (bglx == "BC")
                {
                    xmlstr = "<BL><F_BLH>" + dtyhbh.Rows[0]["F_BLH"].ToString() + "</F_BLH>";

                    xmlstr = xmlstr + "<F_BCZD>" + dtyhbh.Rows[0]["F_BCZD"].ToString() + "</F_BCZD>";
                    xmlstr = xmlstr + "<F_BC_BGYS>" + dtyhbh.Rows[0]["F_BC_BGYS"].ToString() + "</F_BC_BGYS>";
                    xmlstr = xmlstr + "<F_BC_SHYS>" + dtyhbh.Rows[0]["F_BC_SHYS"].ToString() + "</F_BC_SHYS>";
                    xmlstr = xmlstr + "<F_BC_BGRQ>" + dtyhbh.Rows[0]["F_BC_BGRQ"].ToString() + "</F_BC_BGRQ>";
                    xmlstr = xmlstr + "<F_BC_BGXH>" + dtyhbh.Rows[0]["F_BC_BGXH"].ToString() + "</F_BC_BGXH></BL>";
                }
                else
                {
                    //С�������
                    if (bglx == "BD")
                    {
                        xmlstr = "<BL><F_BLH>" + dtyhbh.Rows[0]["F_BLH"].ToString() + "</F_BLH>";
                        xmlstr = xmlstr + "<F_XM>" + dtyhbh.Rows[0]["F_XM"].ToString() + "</F_XM>";
                        xmlstr = xmlstr + "<F_XB>" + dtyhbh.Rows[0]["F_XB"].ToString() + "</F_XB>";
                        xmlstr = xmlstr + "<F_NL>" + dtyhbh.Rows[0]["F_NL"].ToString() + "</F_NL>";
                        xmlstr = xmlstr + "<F_zyh>" + dtyhbh.Rows[0]["F_zyh"].ToString() + "</F_zyh>";
                        xmlstr = xmlstr + "<F_BDZD>" + dtyhbh.Rows[0]["F_BDZD"].ToString() + "</F_BDZD>";
                        xmlstr = xmlstr + "<F_BD_BGYS>" + dtyhbh.Rows[0]["F_BD_BGYS"].ToString() + "</F_BD_BGYS>";
                        xmlstr = xmlstr + "<F_BD_SHYS>" + dtyhbh.Rows[0]["F_BD_SHYS"].ToString() + "</F_BD_SHYS>";
                        xmlstr = xmlstr + "<F_BD_BGRQ>" + dtyhbh.Rows[0]["F_BD_BGRQ"].ToString() + "</F_BD_BGRQ>";
                        xmlstr = xmlstr + "<F_BD_BGXH>" + dtyhbh.Rows[0]["F_BD_BGXH"].ToString() + "</F_BD_BGXH></BL>";
                    }
                    else
                    {
                        xmlstr = "<BL><F_BLH>" + dtyhbh.Rows[0]["F_BLH"].ToString() + "</F_BLH>";
                        xmlstr = xmlstr + "<F_sqxh>" + dtyhbh.Rows[0]["F_sqxh"].ToString() + "</F_sqxh>";
                        xmlstr = xmlstr + "<F_XM>" + dtyhbh.Rows[0]["F_XM"].ToString() + "</F_XM>";
                        xmlstr = xmlstr + "<F_XB>" + dtyhbh.Rows[0]["F_XB"].ToString() + "</F_XB>";
                        xmlstr = xmlstr + "<F_NL>" + dtyhbh.Rows[0]["F_NL"].ToString() + "</F_NL>";
                        xmlstr = xmlstr + "<F_zyh>" + dtyhbh.Rows[0]["F_zyh"].ToString() + "</F_zyh>";
                        xmlstr = xmlstr + "<F_mzh>" + dtyhbh.Rows[0]["F_mzh"].ToString() + "</F_mzh>";
                        xmlstr = xmlstr + "<F_ch>" + dtyhbh.Rows[0]["F_ch"].ToString() + "</F_ch>";
                        xmlstr = xmlstr + "<F_blzd>" + dtyhbh.Rows[0]["F_blzd"].ToString() + "</F_blzd>";
                        xmlstr = xmlstr + "<F_bgys>" + dtyhbh.Rows[0]["F_bgys"].ToString() + "</F_bgys>";
                        xmlstr = xmlstr + "<F_bgrq>" + dtyhbh.Rows[0]["F_bgrq"].ToString() + "</F_bgrq>";
                        xmlstr = xmlstr + "<F_shys>" + dtyhbh.Rows[0]["F_shys"].ToString() + "</F_shys>";
                        xmlstr = xmlstr + "<F_spare5>" + dtyhbh.Rows[0]["F_spare5"].ToString() + "</F_spare5></BL>";
                    }
                }
                if (xmlstr == "")
                {
                    log.WriteMyLog("ǩ����������Ϊ��");
                    return "0";
                }
                if (msg == "1")
                    MessageBox.Show("ǩ������" + xmlstr);
                try
                {
                    //�ؼ�
                    //  string   signxml= xj.XJCA_SignStr(appid,xmlstr);

                    // ��̬��
                    //XJCA_SignSeal������signxml ����ֵ��ǩ�·��ص���Ϣ ����ǩ�����ݣ�֤�����ݣ�֤��id���ԡ���������������ǩ�����ݺ�֤��id��
                    // XJCAverifyyfy.XJCA_SignSeal(xmlstr, signxml, len);

                    bool ca_yz = XJCA_SignSeal(xmlstr, signxml, ref len);
                    if (!ca_yz)
                    {
                        return "0";
                    }

                    else
                    {
                        if (msg == "1")
                            MessageBox.Show("ǩ��ͨ������������ֵ");
                        try
                        {
                            // ǩ�·��ص���Ϣsignxml ����ǩ�����ݣ�֤�����ݣ�֤��id���ԡ���������������ǩ�����ݺ�֤��id
                            string[] getsignxml = signxml.ToString().Split(',');
                            //ǩ������
                            string signxml_str = getsignxml[0];
                            //֤������
                            string cert = getsignxml[1];
                            //֤��id
                            string certID = getsignxml[2];
                            //--------��signxml,,TimeSign_XML�ȷ���ֵ �������ݿ�-------------

                            if (msg == "1")
                                MessageBox.Show("��������ֵ��ɣ������м��");
                            dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");

                            try
                            {

                                string sqlstr = "insert into T_CAXX(blh,RQ,TimeSignXML,signxml,BGNR,keyname,cert,certID,CAZT,BGLX,BGXH) values ('" + dtyhbh.Rows[0]["F_BLH"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TimeSign_XML + "','" + signxml.ToString() + "','" + xmlstr + "','" + keyname + "','" + cert + "','" + certID + "','����֤','" + bglx + "','" + bgxh + "') ";
                                if (msg == "1")
                                    MessageBox.Show(sqlstr);
                                int x = bb.ExecuteSQL(sqlstr);
                                bb.Close();
                                if (x < 1)
                                {
                                    log.WriteMyLog("ǩ�½��ʧ��");
                                    return "0";
                                }

                            }
                            catch (Exception ee)
                            {
                                bb.Close();
                                log.WriteMyLog("ǩ�½���쳣:���ݿ�д��ʧ��," + ee.ToString());
                                return "0";
                            }
                            if (msg == "1")
                                MessageBox.Show("�����м�����");
                        }
                        catch
                        {
                            MessageBox.Show("��������ֵ��д�������쳣");
                            return "0";
                        }
                    }
                    if (msg == "1")
                        MessageBox.Show("ǩ�����");
                }
                catch
                {
                    if (msg == "1")
                        MessageBox.Show("ǩ���쳣");
                    return "0";
                }

                //----------------------------------------------
                //----------------��ȡǩ��ͼƬ----------------------
                //----------------------------------------------
                if (msg == "1")
                    MessageBox.Show("��ȡǩ��ͼƬ��ʼ������");
                bool ss = XJCA_GetSealBMPB("D:\\pathqc\\rpt-szqm\\YSBMP\\" + keyname + ".bmp", 1);
                if (ss)
                {
                    if (msg == "1")
                        MessageBox.Show("��ȡǩ��ͼƬ��ɣ���ʼ�ϴ�����");
                    //----------------------------------------------
                    //----------------�ϴ�ǩ��ͼƬ��ftp---------------------
                    //----------------------------------------------
                    string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                    string ftpUserID = f.ReadString("ftp", "user", "");
                    string ftpPassword = f.ReadString("ftp", "pwd", "");
                    string ftpRemotePath = f.ReadString("ftp", "szqmPath", "/pathimages/rpt-szqm/YSBMP/");

                    string ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
                    string status = "";
                    FtpWeb fw = new FtpWeb(ftpServerIP, ftpRemotePath, ftpUserID, ftpPassword);

                    fw.Upload("D:\\pathqc\\rpt-szqm\\YSBMP\\" + keyname + ".bmp", "", out status);
                    if (status == "Error")
                    {
                        MessageBox.Show("ǩ��ͼƬ�ϴ�ʧ�ܣ���������ˣ�");
                        return "0";
                    }
                    if (msg == "1")
                        MessageBox.Show("ǩ��ͼƬ�ϴ��ɹ�");
                    return "1";
                }
                if (msg == "1")
                    MessageBox.Show("ǩ��ͼƬ��ȡʧ��");
                return "0";

            }
            if (type == "QXSH")
            {
                if (msg == "1")
                    MessageBox.Show("ȡ����ˡ���");
                aa.Close();

                dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");
                try
                {
                    if (msg == "1")
                        MessageBox.Show("ȡ����ˡ���ɾ���м���¼��ʼ");
                    string sqlstr = "delete  T_CAXX where blh='" + getblh + "'  and BGLX='" + bglx.Trim() + "' and BGXH='" + bgxh + "'";
                    int x = bb.ExecuteSQL(sqlstr);
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("ȡ��������");
                    return "1";
                }
                catch (Exception eee)
                {
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("ȡ������쳣");
                    log.WriteMyLog(eee.ToString());
                    return "0";
                }

            }
            if (type == "QXQZ" && (bglx == "BC" || bglx == "BD"))
            {
                if (msg == "1")
                    MessageBox.Show("ȡ��ǩ�ֿ�ʼ����");
                aa.Close();

                dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");
                try
                {
                    if (msg == "1")
                        MessageBox.Show("ȡ��ǩ�֡���ɾ���м���¼��ʼ");
                    string sqlstr = "delete  T_CAXX where blh='" + getblh + "'  and BGLX='" + bglx.Trim() + "' and BGXH='" + bgxh + "'";
                    int x = bb.ExecuteSQL(sqlstr);
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("ȡ��ǩ����ɡ���");
                    return "1";
                }
                catch (Exception eee)
                {
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("ȡ��ǩ���쳣");
                    log.WriteMyLog(eee.ToString());
                    return "0";
                }


            }

            if (msg == "1")
                MessageBox.Show("��ɲ���");
            aa.Close();

                return "1";
    }
        public String Getmd5(String s)
        {
            //System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            //bytes = md5.ComputeHash(bytes);
            //md5.Clear();
            //string ret = "";
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                   
            //}
            //return ret.PadLeft(32, '0');   \

            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(s);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.AppendFormat("{0:x2}", hash[i]);

            }
            return sb.ToString();
        }
    }
}
