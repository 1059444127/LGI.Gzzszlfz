using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using ZgqClassPub;


namespace PathHISZGQJK
{
    class xjca
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public void CAQM(string blh, string bglx, string bgxh, string msg, string debug, string dz)
        {
            bglx = bglx.ToLower();
            if (bglx == "")
                bglx = "cg";
            if (bgxh == "")
                bgxh = "1";

            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;


            try
            {
                string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "");
                string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "");
                string CAQZ = f.ReadString("savetohis", "CAQZ", "1").Replace("\0", "");

                debug = f.ReadString("savetohis", "debug", "");
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable bljc = new DataTable();
                bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

                string logname = "PATHZGQCAJK";

                if (bljc == null)
                {
                    log.WriteMyLog("病理数据库设置有问题！", logname);
                    return;
                }
                if (bljc.Rows.Count < 1)
                {
                    log.WriteMyLog("病理号有错误", logname);
                    return;
                }

                string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
                DataTable dt_bc = new DataTable();
                if (bglx == "bc")
                {
                    try
                    {
                        dt_bc = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "'  and F_bc_bgxh='" + bgxh + "'", "bc");
                        if (dt_bc.Rows.Count >= 0)
                            bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString().Trim();
                    }
                    catch
                    { }
                }

                DataTable dt_bd = new DataTable();
                if (bglx == "bd")
                {
                    try
                    {
                        dt_bd = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "'  and F_bd_bgxh='" + bgxh + "'", "bd");
                        if (dt_bc.Rows.Count >= 0)
                            bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString().Trim();
                    }
                    catch
                    { }
                }
                if (dz == "qxsh")
                    bgzt = "取消审核";
              
                if (bgzt == "已审核" && CAQZ == "1")
                {
                    XJCAClass2 xjca = new XJCAClass2();
                    xjca.CaQZ(blh, bglx, bgxh, debug, bljc, yhmc, bgzt);
                }
            }
            catch
            {
            }
        }
    }

    class XJCAClass2
    {

        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_SignSeal", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern bool XJCA_SignSeal(string src, StringBuilder signxml, ref Int32 len);
        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetSealBMPB", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern bool XJCA_GetSealBMPB(string filepath, int times);
        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_MakePdf417ToFile", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern void XJCA_MakePdf417ToFile(StringBuilder ucData, int nDataLen, string szBmpFileName, int nClumn, int nErr, int nHLRatio, int nHeight);
        [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_TimeSign", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern string XJCA_TimeSign(string ip, string djid, StringBuilder hash);

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public string CaQZ(string blh, string bglx, string bgxh, string debug, DataTable dt_cg, string yhmc, string bgzt)
        {
            string msg = f.ReadString("savetohis", "camsg", "0").Replace("\0", "");
            string logname = "CAZGQJK";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string blbh = blh;
            if (bglx == "bc" || bglx == "bd")
                blbh = blh + bglx + bgxh;

            try
            {
                if (debug == "1")
                    log.WriteMyLog("开始签字。。。", logname);

                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                //补充审核
                string sql_str = "";
                if (bglx == "bc")
                {
                    sql_str = "select * from T_BCBG where  F_BLH='" + blh + "' and F_BC_BGZT='已审核' and F_BC_BGXH='" + bgxh + "'";
                    dt_bc = aa.GetDataTable(sql_str, "bcbg");
                    if (dt_bc.Rows.Count <= 0)
                    {
                        log.WriteMyLog("未查询到此补充报告" + blbh, logname);
                        return "0";
                    }
                }
                //小冰冻审核
                if (bglx == "bd")
                {
                    sql_str = "select * from T_BDBG where  F_BLH='" + blh + "' and  F_BD_BGZT='已审核' and F_BD_BGXH='" + bgxh + "'";
                    dt_bd = aa.GetDataTable(sql_str, "bdbg");
                    if (dt_bd.Rows.Count <= 0)
                    {
                        log.WriteMyLog("未查询到此冰冻报告" + blbh, logname);
                        return "0";
                    }
                }

                string TimeSign_XML = "";
                //时间戳服务地址
                string yztime = f.ReadString("CA", "yztime", "0");
                if (yztime == "1")
                {

                    string timeip = f.ReadString("CA", "timeip_web", "http://172.20.89.23:8080/xjcaTimestamp/services/sign");
                    string hash = blbh + "^" + dt_cg.Rows[0]["F_XM"].ToString().Trim() + "^" + dt_cg.Rows[0]["F_NL"].ToString().Trim() + "^"
                        + dt_cg.Rows[0]["F_XM"].ToString().Trim() + "^" + dt_cg.Rows[0]["F_BRBH"].ToString().Trim() + "^" + dt_cg.Rows[0]["F_SQXH"].ToString().Trim() + "^" + dt_cg.Rows[0]["F_ZYH"].ToString().Trim() + "^" + dt_cg.Rows[0]["F_MZH"].ToString().Trim() + "^" + dt_cg.Rows[0]["F_SFZH"].ToString().Trim();
                    if (debug == "1")
                        log.WriteMyLog("验证时间戳:" + blbh + "\n" + hash, logname);
                    //返回值时间戳xml（TimeSign_XML）需要写人数据库
                    StringBuilder md5 = new StringBuilder(Getmd5(hash));
                    try
                    {
                        TimeSign_XML = XJCA_TimeSign(timeip, blbh, md5);
                    }
                    catch (Exception ee2)
                    {
                        if (msg == "1")
                            MessageBox.Show("CA签名:时间戳验证失败:" + ee2.Message);
                        ZgqClass.BGHJ(blh, yhmc, "数字签名", "时间戳验证失败:" + ee2.Message, "ZGQJK", "CA签名");
                        log.WriteMyLog("CA签名:时间戳验证失败：" + ee2.Message, logname);
                        return "0";
                    }
                    if (debug == "1")
                    {
                        log.WriteMyLog("CA签名:TimeSign_XML:" + TimeSign_XML, logname);
                    }

                    if (debug == "1")
                        log.WriteMyLog("CA签名:时间戳完成", logname);
                }


                //签章----------------------------------
                if (debug == "1")
                    log.WriteMyLog("CA签名:开始签章。。。", logname);


                //签章原文
                string xmlstr = "";
                string bgwy = "";

                if (bglx == "cg" || bglx == "")
                {
                    xmlstr = xmlstr + "<F_BLZD><![CDATA[" + dt_cg.Rows[0]["F_BLZD"].ToString() + "]]></F_BLZD>";
                    xmlstr = xmlstr + "<F_BGYS>" + dt_cg.Rows[0]["F_BGYS"].ToString() + "</F_BGYS>";
                    xmlstr = xmlstr + "<F_SHYS>" + dt_cg.Rows[0]["F_SHYS"].ToString() + "</F_SHYS>";
                    xmlstr = xmlstr + "<F_BGRQ>" + dt_cg.Rows[0]["F_BGRQ"].ToString() + "</F_BGRQ>";
                    xmlstr = xmlstr + "<F_SPARE5>" + dt_cg.Rows[0]["F_spare5"].ToString() + "</F_SPARE5>";
                    xmlstr = xmlstr + "<F_BGLX>" + "cg1" + "</F_BGLX>";
                }
                if (bglx == "bc" && dt_bc.Rows.Count > 0)
                {
                    xmlstr = xmlstr + "<F_BLZD><![CDATA[" + dt_bc.Rows[0]["F_BCZD"].ToString() + "]]></F_BLZD>";
                    xmlstr = xmlstr + "<F_BGYS>" + dt_bc.Rows[0]["F_BC_BGYS"].ToString() + "</F_BGYS>";
                    xmlstr = xmlstr + "<F_SHYS>" + dt_bc.Rows[0]["F_BC_SHYS"].ToString() + "</F_SHYS>";
                    xmlstr = xmlstr + "<F_BGRQ>" + dt_bc.Rows[0]["F_BC_BGRQ"].ToString() + "</F_BGRQ>";
                    xmlstr = xmlstr + "<F_SPARE5>" + dt_bc.Rows[0]["F_BC_spare5"].ToString() + "</F_SPARE5>";
                    xmlstr = xmlstr + "<F_BGLX>" + "bc" + bgxh + "</F_BGLX>";
                }
                if (bglx == "bd")
                {
                    xmlstr = xmlstr + "<F_BLZD><![CDATA[" + dt_bd.Rows[0]["F_BDZD"].ToString() + "]]></F_BLZD>";
                    xmlstr = xmlstr + "<F_BGYS>" + dt_bd.Rows[0]["F_BD_BGYS"].ToString() + "</F_BGYS>";
                    xmlstr = xmlstr + "<F_SHYS>" + dt_bd.Rows[0]["F_BD_SHYS"].ToString() + "</F_SHYS>";
                    xmlstr = xmlstr + "<F_BGRQ>" + dt_bd.Rows[0]["F_BD_BGRQ"].ToString() + "</F_BGRQ>";
                    xmlstr = xmlstr + "<F_SPARE5>" + dt_bd.Rows[0]["F_BD_BGRQ"].ToString() + "</F_SPARE5>";
                    xmlstr = xmlstr + "<F_BGLX>" + "bd" + dt_bd.Rows[0]["F_BD_BGXH"].ToString() + "</F_BGLX>";
                }
                if (xmlstr == "")
                {
                    log.WriteMyLog("CA签名:签章数据内容为空,请重新审核", logname);
                    if (msg == "1")
                        MessageBox.Show("CA签名失败:签章数据内容为空");
                    ZgqClass.BGHJ(blh, yhmc, "数字签名", "签章数据内容为空", "ZGQJK", "CA签名");
                    return "0";
                }
                string bgyw = "";
                bgyw = "<BL>";
                bgyw = bgyw + "<F_BLBH>" + blbh + "</F_BLBH>";
                bgyw = bgyw + "<F_BLH>" + blh + "</F_BLH>";
                bgyw = bgyw + "<F_XM>" + blh + "</F_XM>";
                bgyw = bgyw + "<F_XB>" + blh + "</F_XB>";
                bgyw = bgyw + "<F_NL>" + blh + "</F_NL>";
                bgyw = bgyw + "<F_ZYH>" + blh + "</F_ZYH>";
                bgyw = bgyw + "<F_MZH>" + blh + "</F_MZH>";
                bgyw = bgyw + "<F_SQXH>" + blh + "</F_SQXH>";
                bgyw = bgyw + "<F_YZID>" + blh + "</F_YZID>";
                bgyw = bgyw + "<F_BRBH>" + blh + "</F_BRBH>";
                bgyw = bgyw + "<F_SFZH>" + blh + "</F_SFZH>";
                bgyw = bgyw + "<F_SDRQ>" + blh + "</F_SDRQ>";
                bgyw = bgyw + "<F_CH>" + blh + "</F_CH>";
                bgyw = bgyw + xmlstr + "</BL>";

                if (debug == "1")
                    log.WriteMyLog("CA签名:签章内容:" + bgyw, logname);


                try
                {
                 
                    int len = 0;
                    StringBuilder signxml = new StringBuilder(100000);

                    bool qzrtn = XJCA_SignSeal(Getmd5(bgyw), signxml, ref len);

                    if (debug == "1")
                        log.WriteMyLog("CA签名:" + signxml.ToString(), logname);
                    if (!qzrtn)
                    {
                        log.WriteMyLog("CA签名:数字签名失败,请重新审核" + qzrtn, logname);

                        if (msg == "1")
                            MessageBox.Show("CA签名:数字签名失败,请重新审核" + qzrtn);
                        ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:数字签名失败" + qzrtn, "ZGQJK", "CA签名");
                        return "0";
                    }
                    else
                    {
                        string sign = signxml.ToString().Trim();
                        if (sign == "")
                        {
                            log.WriteMyLog("CA签名:KEY密码验证失败:签名失败", logname);
                            if (msg == "1")
                                MessageBox.Show("CA签名:KEY密码验证失败:签名失败");
                            ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:KEY密码验证失败:签名失败", "ZGQJK", "CA签名");
                            return "0";
                        }

                        if (sign.Split(',')[0].Trim() == "")
                        {
                            log.WriteMyLog("CA签名:KEY密码验证失败:签名失败", logname);
                            if (msg == "1")
                                MessageBox.Show("CA签名失败:KEY密码验证失败:签名失败");
                            ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:KEY密码验证失败:签名失败", "ZGQJK", "CA签名");

                            return "0";
                        }

                        if (debug == "1")
                            log.WriteMyLog("CA签名:签字通过，解析返回值", logname);
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
                            //将signxml,,TimeSign_XML等返回值 存入数据库-------------
                            try
                            {
                                string sqlstr = "insert into T_CAXX(F_blbh,blh,RQ,KeyName,bgnr,TimeSign_XML,signxml,cert,certID) values ('" + blbh + "','" + blh + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + yhmc + "','" + bgyw + "','" + TimeSign_XML + "','" + signxml.ToString() + "','" + cert + "','" + certID + "') ";

                                int x = SQL_ExecuteNonQuery(sqlstr);

                                if (x <= 0)
                                {
                                    log.WriteMyLog("CA签名:写入数据库T_SZQM失败", logname);
                                    if (msg == "1")
                                        MessageBox.Show("CA签名:写入数据库T_SZQM失败");
                                    ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:写入数据库T_SZQM失败", "ZGQJK", "CA签名");

                                    return "0";
                                }
                                else
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("CA签名:插入T_SZQM完成", logname);
                                    if (bglx == "cg")
                                        aa.ExecuteSQL("update T_JCXX  set F_SZQZ='1' where F_BLH='" + blh + "'");
                                    if (bglx == "bc")
                                        aa.ExecuteSQL("update T_BCBG  set F_SZQZ='1' where F_BLH='" + blh + "' and F_bc_bgxh='" + bgxh + "'");
                                    if (bglx == "bd")
                                        aa.ExecuteSQL("update T_BDBG  set F_SZQZ='1' where F_BLH='" + blh + "' and F_bd_bgxh='" + bgxh + "'");
                                    ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:签名成功", "ZGQJK", "CA签名");
                                }

                            }
                            catch (Exception ee7)
                            {
                                log.WriteMyLog("CA签名:写入数据库异常:" + ee7.Message, logname);
                                if (msg == "1")
                                    MessageBox.Show("CA签名:写入数据库异常:" + ee7.Message);
                                ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:写入数据库异常:" + ee7.Message, "ZGQJK", "CA签名");

                                return "0";
                            }

                        }
                        catch (Exception ee8)
                        {

                            log.WriteMyLog("CA签名:解析签名数据异常:" + ee8.Message, logname);
                            if (msg == "1")
                                MessageBox.Show("CA签名:解析签名数据异常:" + ee8.Message);
                            ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:解析签名数据异常:" + ee8.Message, "ZGQJK", "CA签名");

                            return "0";
                        }
                    }

                    log.WriteMyLog("CA签名:签名完成", logname);
                }
                catch (Exception ee9)
                {
                    log.WriteMyLog("CA签名:签名异常:" + ee9.Message, logname);
                    if (msg == "1")
                        MessageBox.Show("CA签名:签名异常:" + ee9.Message);
                    ZgqClass.BGHJ(blh, yhmc, "数字签名", "CA签名:签名异常:" + ee9.Message, "ZGQJK", "CA签名");

                    return "0";
                }

                //----------------------------------------------
                //----------------获取签章图片----------------------
                //----------------------------------------------
                string getbmpfs = f.ReadString("CA", "getbmpfs", "2").Replace("\0", "").Trim();
                string upbmpfs = f.ReadString("CA", "upbmpfs", "0").Replace("\0", "").Trim();
                string bmppath = ZgqClass.GetSz("view", "szqmlj", Application.StartupPath.ToString() + "\\rpt-szqm\\ysbmp").Replace("\0", "");



                if (getbmpfs == "0")
                {
                    goto GetEwm;
                }
                if (getbmpfs == "2")
                {
                    if (File.Exists(bmppath + "\\" + yhmc + ".bmp"))
                    {
                        goto GetEwm;
                    }
                }
                if (!Directory.Exists(Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\"))
                {
                    Directory.CreateDirectory(Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\");
                }
                if (debug == "1")
                    log.WriteMyLog("获取签章图片开始。。。", logname);
                bool ss = XJCA_GetSealBMPB(Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\" + yhmc + ".bmp", 1);
                // XJCA_ResetDevice();
                if (ss)
                {
                    if (!File.Exists(Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\" + yhmc + ".bmp"))
                    {
                        log.WriteMyLog("获取签章图片失败" + Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\" + yhmc + ".bmp", logname);
                    }

                    if (debug == "1")
                        log.WriteMyLog("获取签章图片成功，开始上传。。", logname);

                    if (upbmpfs == "1")
                    {
                        if (debug == "1")
                            log.WriteMyLog("上传签名图片:" + Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\" + yhmc + ".bmp --->" + bmppath + yhmc + ".bmp", logname);

                        File.Copy(Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\" + yhmc + ".bmp", bmppath + yhmc + ".bmp", true);
                        if (!File.Exists(bmppath + yhmc + ".bmp"))
                        {
                            log.WriteMyLog("签章图片上传失败" + bmppath + yhmc + ".bmp", logname);
                            return "0";
                        }
                        if (debug == "1")
                            log.WriteMyLog("签章图片上传成功", logname);

                        return "1";
                    }
                    if (upbmpfs == "2")
                    {

                        ///////上传签章图片至ftp---------------------
                        string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                        string ftpUserID = f.ReadString("ftp", "user", "");
                        string ftpPassword = f.ReadString("ftp", "pwd", "");
                        string ftpszqmpath = f.ReadString("ca", "ftpszqmpath", "/pathsetup/pathqc/rpt-szqm/YSBMP/");
                        string ftpURI = "ftp://" + ftpServerIP + "/" + ftpszqmpath + "/";
                        string status = "";
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpszqmpath, ftpUserID, ftpPassword);

                        fw.Upload(Application.StartupPath.ToString() + "\\rpt-szqm\\YSBMP\\" + yhmc + ".bmp", "", out status);
                        if (status == "Error")
                        {
                            log.WriteMyLog("CA签名:签章图片上传失败，请重新审核", logname);
                            return "0";
                        }
                        if (debug == "1")
                            log.WriteMyLog("签章图片上传成功", logname);
                    }

                }
                else
                {
                    if (debug == "1")
                        log.WriteMyLog("签章图片获取失败,请重新审核", logname);
                    return "1";
                }


            /////验章
            // XJCAverify.(M_signxml, M_signxml.Length, Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP", 7, 3, 3, 0);

GetEwm:
                # region 获取二维码图片
                string get2wm = f.ReadString("ca", "get2wm", "0").Replace("\0", "");

                if (get2wm == "1")
                {
                    if (!Directory.Exists(Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\"))
                    {
                        Directory.CreateDirectory(Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\");
                    }
                    else
                    {
                        DirectoryInfo dir = new DirectoryInfo(Application.StartupPath.ToString() + @"\rpt-szqm\MakePdfFile\");
                        foreach (FileInfo ff in dir.GetFiles("*.*"))
                            ff.Delete();
                    }

                    StringBuilder M_signxml = new StringBuilder(blbh, 10000);
                    XJCA_MakePdf417ToFile(M_signxml, M_signxml.Length, Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP", 7, 3, 3, 0);
                    //XJCA_ResetDevice();
                    if (!File.Exists(Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP"))
                    {
                        log.WriteMyLog("二维码不存在" + Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP", logname);
                        return "1";
                    }
                    string up2wmfs = f.ReadString("ca", "up2wmfs", "1").Replace("\0", "");
                    if (up2wmfs == "1")
                    {
                        string ewmpath = f.ReadString("ca", "ewmpath", Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile").Replace("\0", "");
                        if (debug == "1")
                            log.WriteMyLog("上传二维码图片:" + Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP --->" + ewmpath + "\\" + blbh + "EWM.BMP", logname);
                        File.Copy(Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP", ewmpath + "\\" + blbh + "EWM.BMP", true);
                        if (!File.Exists(ewmpath + "\\" + blbh + "EWM.BMP"))
                        {
                            log.WriteMyLog("二维码图片上传失败" + ewmpath + "\\" + blbh + "EWM.BMP", logname);
                            return "0";
                        }
                        if (debug == "1")
                            log.WriteMyLog("二维码图片上传成功", logname);
                        return "1";
                    }
                    if (upbmpfs == "2")
                    {

                        ///////上传签章图片至ftp---------------------
                        string ftpServerIP = f.ReadString("ftp", "ftpip", "");
                        string ftpUserID = f.ReadString("ftp", "user", "");
                        string ftpPassword = f.ReadString("ftp", "pwd", "");
                        string ftpewmpath = f.ReadString("ca", "ftpewmpath", "/pathimages/szqm/MakePdfFile/");

                        string ftpURI = "ftp://" + ftpServerIP + "/" + ftpewmpath + "/";
                        string status = "";
                        FtpWeb fw = new FtpWeb(ftpServerIP, ftpewmpath, ftpUserID, ftpPassword);
                        fw.Upload(Application.StartupPath.ToString() + "\\rpt-szqm\\MakePdfFile\\" + blbh + "EWM.BMP", "", out status);
                        if (status == "Error")
                        {
                            log.WriteMyLog("二维码图片上传失败，请重新审核！", logname);
                            return "0";
                        }
                        if (debug == "1")
                            log.WriteMyLog("二维码图片上传成功", logname);
                    }
                }
                return "1";
            }
            catch (Exception ee1)
            {
                log.WriteMyLog(ee1.Message, logname);
                return "0";
            }
                #endregion

        }
        public String Getmd5(String s)
        {

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
        public int SQL_ExecuteNonQuery(string sql)
        {
            int x = 0;
            string constr = f.ReadString("ca", "ca_odbc", "Data Source=172.16.89.30;Initial Catalog=pathnet-ca;User Id=pathnet;Password=4s3c2a1p;").Trim().Replace("\0", "").Trim();
          
            SqlConnection con = new SqlConnection(constr);
            SqlCommand com = null;
            try
            {
                com = new SqlCommand(sql, con);
                con.Open();
                x = com.ExecuteNonQuery();
                con.Close();
                com.Dispose();
                return x;
            }
            catch (Exception ee)
            {
                MessageBox.Show("执行数据异常：" + ee.Message);
                log.WriteMyLog(sql);
                con.Close();
                com.Dispose();
                return -1;
            }
        }
    }
}
