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
    //新医大一附院
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
            //-------获取sz中设置的参数---------------------
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
            // 获取key盘信息---------------------------------------------------------------------------------------------
            xjcaTechATLLib.xjcaTechATLLib xj = new xjcaTechATLLib.xjcaTechATLLib();
            try
            {
                ////调控件
                // MessageBox.Show(xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString());
                //MessageBox.Show(xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString());

                //if (xj.XJCA_CspInstalled("HaiTai Cryptographic Service Provider for xjca").ToString() == "0")
                //{
                //    MessageBox.Show("驱动未安好");
                //    return "0";
                //}
                //if (xj.XJCA_KeyInsert("HaiTai Cryptographic Service Provider for xjca").ToString() == "1")
                //{
                //    MessageBox.Show("key未插入");
                //    return "0";
                //}
                //调动态库
                if (XJCAverify.XJCA_CspInstalled() == false)
                {
                    MessageBox.Show("驱动未安好");
                    return "0";
                }
                if (XJCAverify.XJCA_KeyInserted() == false)
                {
                    MessageBox.Show("key未插入");
                    return "0";
                }
            }
            catch
            { }


            //-------------验证key盘是否插入-------------------------------------------------------------------------------
            string dn = "";
            string[] aaa_ca;
            try
            {
                ////调动态库
                ////-----SN---
                //StringBuilder sn_strB = new StringBuilder(100);
                //int sn_len = 0;
                //XJCAverify.XJCA_GetCertSN(2, sn_strB, ref sn_len);
                //SN = sn_strB.ToString();
                ////----DN--------
                //StringBuilder dn = new StringBuilder(400);
                //int dn_len = 0;
                //XJCAverify.XJCA_GetCertDN(dn, ref dn_len);

                //调控件 
                //---SN-----
                string KeyID = xj.XJCA_GetCertSN();
                //----DN----------------
                dn = xj.XJCA_GetCertDN();
                if (KeyID.Trim() == "")
                {
                    MessageBox.Show("证书序列号为空");
                    return "0";
                }
                if (dn.Trim() == "10011")
                {
                    MessageBox.Show("KEY盘未插入，请插入KEY盘！！！");
                    return "0";
                }
                aaa_ca = dn.ToString().Split(',');
            }
            catch
            {
                MessageBox.Show("KEY盘未插入，请插入KEY盘！！！");
                return "0";
            }

            //----------------------------------------------
            //--------------获取KEY中用户名--------------------
            //----------------------------------------------
            try
            {
                keyname = aaa_ca[aaa_ca.Length - 1].Substring(4);
                if (keyname.Contains("."))
                {
                    if (!keyname.Contains(yhmc))
                    {
                        MessageBox.Show("当前操作人与KEY用户不符，不能执行此操作！！！");
                        return "0";
                    }
                }
                else
                {
                    if (yhmc.Trim() != keyname)
                    {
                        MessageBox.Show("当前操作人与KEY用户不符，不能执行此操作！！！");
                        return "0";
                    }
                }
            }
            catch
            {
                MessageBox.Show("KEY盘未插入");
                return "0";
            }
            try
            {
                keyname = aaa_ca[aaa_ca.Length - 1].Substring(4);
                string keyYHGH = aaa_ca[1].Substring(3);

                string xml = xj.XJCA_CertAuth(webservice, appid, keyname);

                if (xml == "")
                {
                    MessageBox.Show("KEY盘验证失败,可能是此KEY用户未注册！");
                    return "0";
                }

                XmlNode xmlok = null;
                XmlDocument xd = new XmlDocument();

                xd.LoadXml(xml);
                xmlok = xd.SelectSingleNode("/XJCA_CertAppAuth");
                string bool_key = xmlok["success"].InnerText.ToString();
                //----------------------------------------------
                //---------------审核人是否是KEY的所有者--------------------

                if (bool_key == "false")
                {
                    MessageBox.Show("认证失败！！！你无权操作此软件");
                    return "0";
                }
                if (keyname.Contains("."))
                    keyname = keyname.Substring(0, keyname.IndexOf('.'));

                if (yhmc.Trim() != keyname)
                {

                    MessageBox.Show("当前操作人与KEY用户不符，不能执行此操作！！！");
                    return "0";
                }
                //------------------工号验证-----------------------


                if (keyYHGH.Trim() != yhbh.Trim())
                {
                    MessageBox.Show("当前操作人工号与KEY用户工号不符，不能执行此操作！！！");
                    return "0";
                }
            }
            catch
            {
                MessageBox.Show("获取KEY盘用户异常");
                return "0";

            }




            ////------------------------------------ 
            ////----------------key密码验证--------- 
            ////------------------------------------ 
            if (msg == "1")
                MessageBox.Show("密码验证开始");

            try
            {

                //--动态库---
                StringBuilder signxml = new StringBuilder(5000);
                string xmlstr = "12345";
                int len = 0;
                bool ca_yz = XJCA_SignSeal(xmlstr, signxml, ref len);
                if (!ca_yz)
                {
                    if (msg == "1")
                        MessageBox.Show("验证未通过");
                    return "0";
                }
                //-控件--
                //string signxml = xj.XJCA_SignStr("HaiTai Cryptographic Service Provider for xjca", xmlstr);

                //string[] getsignxml1 = signxml.ToString().Split(',');
                // string signxml_str1 = getsignxml1[0].Trim();
                //if (signxml.ToString() == "" || cert1 == "" || signxml_str1 == "")
                //{
                //    MessageBox.Show("验证未通过");
                //    return "0";
                //}

            }
            catch (Exception eeee)
            {
                log.WriteMyLog(eeee.ToString());
                MessageBox.Show("验证异常1");
                return "0";
            }

            if (msg == "1")
                MessageBox.Show("密码验证完成");
            //--------验证完毕----------
            //-----------开始签名------------


            /////////////////////////
            //***********************\
            /////////////////////////

            if (type == "QZ")
            {
                if (msg == "1")
                    MessageBox.Show("开始签名。。。");

                string sql_str = "select * from T_JCXX where  F_BLH='" + getblh + "'  and  F_BGZT='已审核'";

                //补充审核
                if (bglx == "BC")
                {
                    sql_str = "select * from V_BCBG_SH where  F_BLH='" + getblh + "' and F_BC_BGZT='已审核'and F_BC_BGXH='" + bgxh + "'";
                }
                //小冰冻审核
                if (bglx == "BD")
                {
                    sql_str = "select * from V_BDBG_bfk where  F_BLH='" + getblh + "' and  F_BD_BGZT='已审核' and F_BD_BGXH='" + bgxh + "'";

                }


                DataTable dtyhbh = aa.GetDataTable(sql_str, "table");
                aa.Close();

                if (msg == "1")
                    MessageBox.Show("开始时间戳。。。");
                //-----------------签章验证-----------------
                //----------------------------------------------
                //----------------时间戳-------------------------


                //时间戳服务地址 
                string IP = "http://172.20.89.23:8080/xjcaTimestamp/services/sign";

                //业务单据号
                string djid = getblh + "^1^CG";
                //关键内容
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
                //返回值时间戳xml（TimeSign_XML）需要写人数据库
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
                   // MessageBox.Show("注意：");
                    MessageBox.Show(TimeSign_XML.ToString());
                   // MessageBox.Show(md5.ToString());
                }
                try
                {

                    //------------将签名时间写入数据库 T_JCXX------------
                    XmlDocument TimeSign_xd = new XmlDocument();
                    TimeSign_xd.LoadXml(TimeSign_XML.ToString());
                    string TimeSign_time = TimeSign_xd.SelectSingleNode("/xjca").ChildNodes[3].InnerText.ToString();
                    aa.ExecuteSQL("update T_JCXX  set F_YL7='" + TimeSign_time + "' where F_BLH='" + getblh + "'");

                }
                catch (Exception des)
                {
                    log.WriteMyLog("将签名时间写入数据库 T_JCXX异常");
                }
                if (msg == "1")
                    MessageBox.Show("时间戳完成");
                //------------------------------------------------------
                //---------------签章----------------------------------
                //签章----------------------------------
                if (msg == "1")
                    MessageBox.Show("开始签章。。。");
                int len = 40000;
                StringBuilder signxml = new StringBuilder(40000);
                //签章原文
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
                    //小冰冻审核
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
                    log.WriteMyLog("签章数据内容为空");
                    return "0";
                }
                if (msg == "1")
                    MessageBox.Show("签章内容" + xmlstr);
                try
                {
                    //控件
                    //  string   signxml= xj.XJCA_SignStr(appid,xmlstr);

                    // 动态库
                    //XJCA_SignSeal方法，signxml 返回值，签章返回的信息 包含签章数据，证书内容，证书id，以“，”隔开，保存签章数据和证书id，
                    // XJCAverifyyfy.XJCA_SignSeal(xmlstr, signxml, len);

                    bool ca_yz = XJCA_SignSeal(xmlstr, signxml, ref len);
                    if (!ca_yz)
                    {
                        return "0";
                    }

                    else
                    {
                        if (msg == "1")
                            MessageBox.Show("签章通过，解析返回值");
                        try
                        {
                            // 签章返回的信息signxml 包含签章数据，证书内容，证书id，以“，”隔开，保存签章数据和证书id
                            string[] getsignxml = signxml.ToString().Split(',');
                            //签章数据
                            string signxml_str = getsignxml[0];
                            //证书内容
                            string cert = getsignxml[1];
                            //证书id
                            string certID = getsignxml[2];
                            //--------将signxml,,TimeSign_XML等返回值 存入数据库-------------

                            if (msg == "1")
                                MessageBox.Show("解析返回值完成，插入中间库");
                            dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");

                            try
                            {

                                string sqlstr = "insert into T_CAXX(blh,RQ,TimeSignXML,signxml,BGNR,keyname,cert,certID,CAZT,BGLX,BGXH) values ('" + dtyhbh.Rows[0]["F_BLH"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + TimeSign_XML + "','" + signxml.ToString() + "','" + xmlstr + "','" + keyname + "','" + cert + "','" + certID + "','已验证','" + bglx + "','" + bgxh + "') ";
                                if (msg == "1")
                                    MessageBox.Show(sqlstr);
                                int x = bb.ExecuteSQL(sqlstr);
                                bb.Close();
                                if (x < 1)
                                {
                                    log.WriteMyLog("签章结果失败");
                                    return "0";
                                }

                            }
                            catch (Exception ee)
                            {
                                bb.Close();
                                log.WriteMyLog("签章结果异常:数据库写入失败," + ee.ToString());
                                return "0";
                            }
                            if (msg == "1")
                                MessageBox.Show("插入中间库完成");
                        }
                        catch
                        {
                            MessageBox.Show("解析返回值或写入数据异常");
                            return "0";
                        }
                    }
                    if (msg == "1")
                        MessageBox.Show("签章完成");
                }
                catch
                {
                    if (msg == "1")
                        MessageBox.Show("签章异常");
                    return "0";
                }

                //----------------------------------------------
                //----------------获取签章图片----------------------
                //----------------------------------------------
                if (msg == "1")
                    MessageBox.Show("获取签章图片开始。。。");
                bool ss = XJCA_GetSealBMPB("D:\\pathqc\\rpt-szqm\\YSBMP\\" + keyname + ".bmp", 1);
                if (ss)
                {
                    if (msg == "1")
                        MessageBox.Show("获取签章图片完成，开始上传。。");
                    //----------------------------------------------
                    //----------------上传签章图片至ftp---------------------
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
                        MessageBox.Show("签章图片上传失败，请重新审核！");
                        return "0";
                    }
                    if (msg == "1")
                        MessageBox.Show("签章图片上传成功");
                    return "1";
                }
                if (msg == "1")
                    MessageBox.Show("签章图片获取失败");
                return "0";

            }
            if (type == "QXSH")
            {
                if (msg == "1")
                    MessageBox.Show("取消审核。。");
                aa.Close();

                dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");
                try
                {
                    if (msg == "1")
                        MessageBox.Show("取消审核。。删除中间库记录开始");
                    string sqlstr = "delete  T_CAXX where blh='" + getblh + "'  and BGLX='" + bglx.Trim() + "' and BGXH='" + bgxh + "'";
                    int x = bb.ExecuteSQL(sqlstr);
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("取消审核完成");
                    return "1";
                }
                catch (Exception eee)
                {
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("取消审核异常");
                    log.WriteMyLog(eee.ToString());
                    return "0";
                }

            }
            if (type == "QXQZ" && (bglx == "BC" || bglx == "BD"))
            {
                if (msg == "1")
                    MessageBox.Show("取消签字开始。。");
                aa.Close();

                dbbase.odbcdb bb = new odbcdb("DSN=pathnet-ca;UID=pathnet;PWD=4s3c2a1p", "", "");
                try
                {
                    if (msg == "1")
                        MessageBox.Show("取消签字。。删除中间库记录开始");
                    string sqlstr = "delete  T_CAXX where blh='" + getblh + "'  and BGLX='" + bglx.Trim() + "' and BGXH='" + bgxh + "'";
                    int x = bb.ExecuteSQL(sqlstr);
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("取消签字完成。。");
                    return "1";
                }
                catch (Exception eee)
                {
                    bb.Close();
                    if (msg == "1")
                        MessageBox.Show("取消签字异常");
                    log.WriteMyLog(eee.ToString());
                    return "0";
                }


            }

            if (msg == "1")
                MessageBox.Show("完成操作");
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
