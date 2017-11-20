using System;
using System.Collections.Generic;
using System.Text;
using readini;
using dbbase;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using LGZGQClass;
using NETCA;
using Netca_PDFSign_COM;


namespace PathnetCAzgq
{
    public class gzzszlly
    {
        private static IniFiles f = new IniFiles("sz.ini");
        private string userPwd = "";

        public string ca(string yhxx)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            ////-------获取sz中设置的参数---------------------
            string debug = f.ReadString("CA", "debug", "").Replace("\0", "").Trim();
            string msg = f.ReadString("CA", "message", "1").Replace("\0", "").Trim();

            string getblh = "";
            string type = "";
            string type2 = "";
            string yhm = "";

            string yhmc = "";
            string yhbh = "";
            string yhmm = "";
            string bglx = "";
            string bgxh = "";

            #region CA登陆

            if (ZGQClass.GetSZ("CA", "ca", "").Replace("\0", "").Trim() == "1")
            {
                if (yhxx == "")
                {
                    return Login();
                }
            }

            #endregion

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
                bglx = getyhxx[3].ToLower();
                type = getyhxx[4];
                yhm = getyhxx[5];
                yhmc = getyhxx[6];
                yhbh = getyhxx[7];
                yhmm = getyhxx[8];
            }

            #region 审核前验证KEY

            if (type == "SH")
            {
                return YZKEY(msg, yhmc, yhbh);
            }

            #endregion

            string blbh = getblh + bglx + bgxh;

            #region 审核后执行，数字签名

            if (type == "QZ")
            {


                if (debug == "1")
                    MessageBox.Show("审核签字");

                string yw = "";

                string isb64yw = f.ReadString("CA", "b64yw", "0").Replace("\0", "").Trim();

                bool WithTSA = false; //带时间戳签名
                if ((f.ReadString("ca", "WithTSA", "0").Trim().Replace("\0", "").Trim()) == "1")
                    WithTSA = true; //签名值包含原文
                bool havcount = false;
                bool isVerify = false; //网关验证
                if ((f.ReadString("ca", "isVerify", "0").Trim().Replace("\0", "").Trim()) == "1")
                    isVerify = true;
                string usapurl = f.ReadString("ca", "usapurl", "").Trim().Replace("\0", "").Trim(); //网关地址
                string svrcertb64 = f.ReadString("ca", "svrcertb64", "1").Trim().Replace("\0", "").Trim(); //网关证书
                string b64SignVal = "";
                SecuInter.X509Certificate oCert = null;
                DataTable dt_jcxx = new DataTable();
                DataTable dt_bc = new DataTable();
                DataTable dt_bd = new DataTable();
                try
                {
                    #region 原文

                    dt_jcxx = aa.GetDataTable("select * from T_JCXX where  F_BLH='" + getblh + "'", "cgbg");

                    if (dt_jcxx == null)
                    {
                        if (msg == "1")
                            MessageBox.Show("连接数据库异常");
                        return "0";
                    }
                    if (dt_jcxx.Rows.Count <= 0)
                    {
                        if (msg == "1")
                            MessageBox.Show("T_JCXX查询数据异常");
                        return "0";
                    }
                    if (bglx == "cg")
                    {
                        if (dt_jcxx.Rows[0]["F_BGZT"].ToString() != "已审核")
                        {
                            if (msg == "1")
                                MessageBox.Show("报告未审核");
                            return "0";
                        }
                        yw = "病理号:" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&常规报告&&性别:" +
                             dt_jcxx.Rows[0]["F_XB"].ToString() + "&年龄:" + dt_jcxx.Rows[0]["F_nl"].ToString()
                             + "&住院号:" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&门诊号:" +
                             dt_jcxx.Rows[0]["F_mzh"].ToString() + "&身份证号:" + dt_jcxx.Rows[0]["F_SFZH"].ToString() +
                             "&科室:" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                             + "&病理诊断:" + dt_jcxx.Rows[0]["F_blzd"].ToString() + "&报告医生:" +
                             dt_jcxx.Rows[0]["F_bgys"].ToString() + "&复诊医生:" + dt_jcxx.Rows[0]["F_FZYS"].ToString()
                             + "&审核医生:" + dt_jcxx.Rows[0]["F_SHYS"].ToString() + "&报告日期:" +
                             dt_jcxx.Rows[0]["F_bgrq"].ToString() + "&审核日期:" + dt_jcxx.Rows[0]["F_spare5"].ToString();
                    }
                    //// 补充审核
                    if (bglx == "bc")
                    {
                        dt_bc =
                            aa.GetDataTable(
                                "select * from T_BCBG where  F_BLH='" + getblh + "' and F_BC_BGZT='已审核'and F_BC_BGXH='" +
                                bgxh + "'", "bcbg");
                        if (dt_bc == null)
                        {
                            if (msg == "1")
                                MessageBox.Show("连接数据库异常");
                            return "0";
                        }
                        if (dt_bc.Rows.Count <= 0)
                        {
                            if (msg == "1")
                                MessageBox.Show("T_BCBG查询数据异常");
                            return "0";
                        }
                        yw = "病理号:" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&补充报告:" + bgxh + "&性别:" +
                             dt_jcxx.Rows[0]["F_XB"].ToString() + "&年龄:" + dt_jcxx.Rows[0]["F_nl"].ToString()
                             + "&住院号:" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&门诊号:" +
                             dt_jcxx.Rows[0]["F_mzh"].ToString() + "&身份证号:" + dt_jcxx.Rows[0]["F_SFZH"].ToString() +
                             "&科室:" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                             + "&病理诊断:" + dt_bc.Rows[0]["F_BCZD"].ToString() + "&报告医生:" +
                             dt_bc.Rows[0]["F_bc_bgys"].ToString() + "&复诊医生:" + dt_bc.Rows[0]["F_bc_FZYS"].ToString()
                             + "&审核医生:" + dt_bc.Rows[0]["F_bc_SHYS"].ToString() + "&报告日期:" +
                             dt_bc.Rows[0]["F_bc_bgrq"].ToString() + "&审核日期:" + dt_bc.Rows[0]["F_bc_spare5"].ToString();
                    }
                    ///// 小冰冻审核
                    if (bglx == "bd")
                    {
                        dt_bd =
                            aa.GetDataTable(
                                "select * from T_BDBG  where  F_BLH='" + getblh +
                                "' and  F_BD_BGZT='已审核' and F_BD_BGXH='" + bgxh + "'", "bcbg");
                        if (dt_bd == null)
                        {
                            if (msg == "1")
                                MessageBox.Show("连接数据库异常");
                            return "0";
                        }
                        if (dt_bd.Rows.Count <= 0)
                        {
                            if (msg == "1")
                                MessageBox.Show("T_BDBG查询数据异常");
                            return "0";
                        }
                        yw = "病理号:" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&冰冻报告:" + bgxh + "&性别:" +
                             dt_jcxx.Rows[0]["F_XB"].ToString() + "&年龄:" + dt_jcxx.Rows[0]["F_nl"].ToString()
                             + "&住院号:" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&门诊号:" +
                             dt_jcxx.Rows[0]["F_mzh"].ToString() + "&身份证号:" + dt_jcxx.Rows[0]["F_SFZH"].ToString() +
                             "&科室:" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                             + "&病理诊断:" + dt_bd.Rows[0]["F_BdZD"].ToString() + "&报告医生:" +
                             dt_bd.Rows[0]["F_bd_bgys"].ToString() + "&复诊医生:" + dt_bd.Rows[0]["F_bd_FZYS"].ToString()
                             + "&审核医生:" + dt_bd.Rows[0]["F_bd_SHYS"].ToString() + "&报告日期:" +
                             dt_bd.Rows[0]["F_bd_bgrq"].ToString();
                    }

                    if (yw.Trim() == "")
                    {
                        if (msg == "1")
                            MessageBox.Show("数字签名内容为空");
                        return "0";
                    }

                    #endregion

                    if (isb64yw == "1")
                        yw = changebase64(yw);

                    #region 签名

                    try
                    {
                        ////签名
                        if (WithTSA)
                            b64SignVal = NETCAPKIv4.signPKCS7WithTSA(yw, usapurl, havcount); //时间戳签名
                        else 
                            b64SignVal = NETCAPKIv4.signNETCA(yw, havcount, userPwd); //不带时间戳签名
                    }
                    catch (Exception  ee1)
                    {
                        if (msg == "1")
                            MessageBox.Show("签名失败：" + ee1.Message);
                        return "0";
                    }
                    if (b64SignVal == "")
                    {
                        if (msg == "1")
                            MessageBox.Show("签名失败");
                        return "0";
                    }

                    #endregion

                    #region 验签

                    try
                    {
                        string signTime = "";
                        oCert = NETCAPKIv4.verifyPKCS7(yw,b64SignVal,true,ref signTime);
                    }
                    catch (Exception ee2)
                    {
                        if (msg == "1")
                            MessageBox.Show("验签失败:" + ee2.Message);
                        return "0";
                    }
                    if (oCert == null)
                    {
                        if (msg == "1")
                            MessageBox.Show("验签失败");
                        return "0";
                    }

                    #endregion

//                    #region   验证证书
//
//                    if (isVerify)
//                    {
//                        try
//                        {
//                            bool bFlag = NETCAPKIv4.VerifyCert(usapurl, svrcertb64, 1, oCert); //再验证证书
//                            if (!bFlag)
//                            {
//                                MessageBox.Show("签名证书验证失败");
//                                return "0";
//                            }
//                        }
//                        catch (Exception ee3)
//                        {
//                            if (msg == "1")
//                                MessageBox.Show("签名证书验证失败:" + ee3.Message);
//                            return "0";
//                        }
//                    }
//
//                    #endregion

                    // 签字
                }
                catch (Exception ex)
                {
                    MessageBox.Show("签名异常：" + ex.Message);
                    return "0";
                }

                #region 签名完成，写数据库

                string errmsg = "";
                int x =
                    aa.ExecuteSQL(
                        "insert into T_SZQM(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_CZY,F_YW,F_SignCert,F_SignData,F_QZRQ) values('" +
                        blbh + "','"
                        + getblh + "','" + bglx + "','" + bgxh + "','" + yhmc + "','" + yw + "','"
                        + oCert.ToString() + "','" + b64SignVal.ToString() + "','" +
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                if (debug == "1")
                {
                    if (x >= 1)
                        MessageBox.Show("写入T_CAXX完成");
                    else
                        MessageBox.Show("写入T_CAXX失败：" + errmsg);
                }

                #endregion

                if (f.ReadString("ca", "hqkeytp", "0").Trim().Replace("\0", "").Trim() == "1")
                {
                    #region 获取签字图片

                    string szqmlj = ZGQClass.GetSZ("view", "szqmlj", @"\\127.0.0.1\pathqc\rpt-szqm\YSBMP\");
                    

                    try
                    {
                        IPDFSign iPDFSign = new PDFSign();
                        //选择证书
                        iPDFSign.SelectCert("netca", 0);
                        IUtilTool iUtilTool = new UtilTool();
                        //传入选中的签名证书的base64编码
                        string CertBase64 = iPDFSign.SignCertBase64Encode;
                        if (CertBase64.Trim() == "")
                        {
                            if (msg == "1")
                                MessageBox.Show("获取签名证书的base64编码失败");
                            return "0";
                        }
                        try
                        {
                           
                            //log.WriteMyLog("CertBase64:"+CertBase64);
                            byte[] image = iUtilTool.GetImageFromDevicByCert(CertBase64);
                            try
                            {
                                MemoryStream memoryStream = new MemoryStream(image, 0, image.Length);
                                memoryStream.Write(image, 0, image.Length);
                                //转成图片
                                Image ii = Image.FromStream(memoryStream);
                                log.WriteMyLog("szqmlj:"+szqmlj);
                                ii.Save(szqmlj + yhmc + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            catch (Exception ee4)
                            {
                                if (msg == "1")
                                    MessageBox.Show("保存签名图片失败：" + ee4.Message);
                                return "0";
                            }


                            string pdfszqz = ZGQClass.GetSZ("CA", "pdfszqz", "1");
                            if (pdfszqz == "1")
                            {
                                #region  生成pdf

                                string blh = getblh;
                                try
                                {
                                    if (bglx == "")
                                        bglx = "cg";
                                    if (bgxh == "")
                                        bgxh = "1";

                                    string bgzt = "";
                                    string filename = dt_jcxx.Rows[0]["F_SPARE5"].ToString();
                                    if (bglx.ToLower() == "bd")
                                    {
                                        bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                                        filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                                    }
                                    if (bglx.ToLower() == "bc")
                                    {
                                        bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                                        filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
                                    }
                                    if (bglx.ToLower() == "cg")
                                    {
                                        bgzt = dt_jcxx.Rows[0]["F_BGZT"].ToString();
                                        filename = dt_jcxx.Rows[0]["F_SPARE5"].ToString();
                                    }

                                    if (bgzt == "已审核")
                                    {
                                        try
                                        {
                                            filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" +
                                                       DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") +
                                                       ".pdf";
                                        }
                                        catch
                                        {
                                            filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + ".pdf";
                                        }
                                        string ml =
                                            DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                                        string pdfpath = "";
                                        string rptpath = ZGQClass.GetSZ("ca", "rptpath", "rpt").Replace("\0", "").Trim();

                                        string scpdffs = f.ReadString("ca", "scpdffs", "1").Replace("\0", "").Trim();
                                        ZGQ_PDFJPG zgq = new ZGQ_PDFJPG();
                                        string message = "";
                                        string filePath = "";

                                        //生成PDF
                                        string ErrMsg = "";
                                       // log.WriteMyLog("filename="+filename);
                                        bool pdf1 = zgq.CreatePDFJPG(blh, bglx, bgxh, ref filename, rptpath,
                                            ZGQ_PDFJPG.type.PDF, ref ErrMsg);
                                        log.WriteMyLog("filename=" + filename);
                                        if (!pdf1)
                                        {
                                            MessageBox.Show("生成PDF失败，请重新审核\r\n" + ErrMsg);
                                            DeleteTempFile(blh);
                                            return "0";
                                        }

                                        if (!File.Exists(filename))
                                        {
                                            MessageBox.Show("生成PDF失败，请重新审核");
                                            DeleteTempFile(blh);
                                            return "0";
                                        }

                                        filePath = filename;
                                        if (zgq.UpPDF(blh, filename, ml, 0, ref errmsg, ref pdfpath))
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog("上传PDF成功");
                                            filename = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                                            ZGQClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + ml + "\\" + filename, "ZGQJK",
                                                "上传PDF");
                                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                            aa.ExecuteSQL(
                                                "insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" +
                                                blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" +
                                                blh + "','" + filename + "','" + pdfpath + "','')");
                                        }
                                        else
                                        {
                                            MessageBox.Show("上传签字PDF失败,请重新审核\r\n" + errmsg);
                                            ZGQClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
                                        }
                                        //上传pdf

                                        iPDFSign.SetImage(image);
                                        iPDFSign.RenderMode = 3;
                                        iPDFSign.SrcFileName = filePath;
                                        string szqm_filename = filePath.Replace(".pdf", "_szqm.pdf");
                                        iPDFSign.DestFileName = szqm_filename;
                                        int xPos =
                                            int.Parse(ZGQClass.GetSZ("ca", "xPos", "350").Replace("\0", "").Trim());
                                        int yPos =
                                            int.Parse(ZGQClass.GetSZ("ca", "yPos", "100").Replace("\0", "").Trim());
                                        int width =
                                            int.Parse(ZGQClass.GetSZ("ca", "width", "80").Replace("\0", "").Trim());
                                        int height =
                                            int.Parse(ZGQClass.GetSZ("ca", "height", "40").Replace("\0", "").Trim());
                                        iPDFSign.SignPosition(1, xPos, yPos, width, height);

                                        if (File.Exists(szqm_filename))
                                        {
                                            //上传签字的pdf

                                            if (zgq.UpPDF(blh, szqm_filename, ml, 0, ref errmsg, ref pdfpath))
                                            {
                                                if (debug == "1")
                                                    log.WriteMyLog("上传签字PDF成功");
                                                szqm_filename =
                                                    szqm_filename.Substring(szqm_filename.LastIndexOf('\\') + 1);
                                                ZGQClass.BGHJ(blh, "上传PDF", "审核",
                                                    "上传签字PDF成功:" + ml + "\\" + szqm_filename, "ZGQJK", "上传PDF");
                                                aa.ExecuteSQL("delete T_BG_PDF_CA  where F_BLBH='" + blbh + "'");
                                                log.WriteMyLog("insert  into T_BG_PDF_CA(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" +
                                                    blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" +
                                                    blh + "','" + szqm_filename + "','" + pdfpath + "','szqm')");
                                                aa.ExecuteSQL(
                                                    "insert  into T_BG_PDF_CA(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" +
                                                    blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" +
                                                    blh + "','" + szqm_filename + "','" + pdfpath + "','szqm')");
                                            }
                                            else
                                            {
                                                MessageBox.Show("上传签字PDF失败,请重新审核\r\n" + errmsg);
                                                ZGQClass.BGHJ(blh, "上传签字PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK",
                                                    "上传PDF");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("PDF签字失败,请重新审核");
                                        }
                                        DeleteTempFile(blh);
                                        return "1";
                                    }
                                    else
                                    {
                                        MessageBox.Show("报告未审核");
                                        return "0";
                                    }
                                }
                                catch (Exception ee10)
                                {
                                    MessageBox.Show("生成PDF异常,请重新审核\r\n" + ee10.Message);
                                    DeleteTempFile(blh);
                                    return "0";
                                }

                                #endregion
                            }
                        }
                        catch (Exception ee5)
                        {
                            MessageBox.Show("获取签名图像异常,请重新审核\r\n" + ee5.Message);
                            return "0";
                        }
                    }
                    catch (Exception ee6)
                    {
                        MessageBox.Show("获取签名证书失败,请重新审核\r\n" + ee6.Message);
                        return "0";
                    }

                    #endregion
                }
                if (debug == "1")
                    MessageBox.Show("签字完成");

                return "1";
            }

            #endregion

            #region 取消审核

            if (type == "QXSH")
            {
                if (f.ReadString("ca", "qxshyz", "0").Trim().Replace("\0", "").Trim() == "1")
                {
                    return YZKEY(msg, yhmc, yhbh);
                }
                return "1";
            }

            #endregion

            #region 取消签字

            if (type == "QXQZ") //&& (bglx == "BC" || bglx == "BD")
            {
                aa.ExecuteSQL("delete from  T_SZQM  where  F_BLBH='" + blbh + "' ");
                aa.ExecuteSQL("delete from  T_BG_PDF  where  F_BLBH='" + blbh + "' ");
                return "1";
            }

            #endregion

            return "1";
        }


        private static DataTable GetYHXX(string CertUID)
        {
            DataTable Dt_Yhxx = new DataTable();
            if (CertUID.Trim() != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    Dt_Yhxx = aa.GetDataTable("select top 1 * from T_YH  where F_YH_BY2='" + CertUID + "' ", "YHXX");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
            return Dt_Yhxx;
        }

        private string YZKEY(string msg, string YHMC, string YHBH)
        {
            ///// 获取用户证书

            string key_userID = "";
            string key_Name = "";
            string CertUID = "";
            string key_DW = "";
            string key_Sfzh = "";
            try
            {
                try
                {
                    SecuInter.X509Certificate oCert = NETCAPKIv4.getX509Certificate(
                    NETCAPKIv4.SECUINTER_CURRENT_USER_STORE, NETCAPKIv4.SECUINTER_MY_STORE,
                    NETCAPKIv4.SECUINTER_CERTTYPE_SIGN,
                    NETCAPKIv4.SECUINTER_NETCA_YES);
                    if (oCert == null)
                    {
                        MessageBox.Show("未找到证书");
                        return "0";
                    }
                    ////用户证书绑定值
                    CertUID = (NETCAPKIv4.getX509CertificateInfo(oCert, 9));
                    ////单位
                    key_DW = (NETCAPKIv4.getX509CertificateInfo(oCert, 13));
                    ////用户名称
                    key_Name = (NETCAPKIv4.getX509CertificateInfo(oCert, 12));
                    ////证书序列号
                    //CertID=(oNETCAPKIv4.GetCertInfo(Cert, 2));
                    ////证件号
                    key_Sfzh = (NETCAPKIv4.getX509CertificateInfo(oCert, 36));
                }
                catch (Exception ee1)
                {
                    if (ee1.Message == "证书选择失败")
                    {
                        if (msg == "1")
                            MessageBox.Show("证书选择失败，请确认Key盘是否插入");
                    }
                    else
                    {
                        if (msg == "1")
                            MessageBox.Show(ee1.Message);
                    }
                    return "0";
                }
            }
            catch (Exception ee)
            {
                if (msg == "1")
                    MessageBox.Show("程序初始化失败");
                return "0";
            }

            /// 验证用户名
            if (f.ReadString("CA", "yzxm", "0").Replace("\0", "").Trim() == "1")
            {
                if (key_Name != YHMC)
                {
                    if (msg == "1")
                        MessageBox.Show("软件使用者与KEY用户不一致,验证失败！");
                    return "0";
                }
            }

            DataTable Dt_yh = GetYHXX(CertUID);

            if (Dt_yh.Rows.Count > 0)
            {
                if (f.ReadString("CA", "yzsfzh", "0").Replace("\0", "").Trim() == "1")
                {
                    if (Dt_yh.Rows[0]["F_SFZH"].ToString().Trim() != key_Sfzh)
                    {
                        if (msg == "1")
                            MessageBox.Show("用户身份证号与KEY中身份证号不一致,验证失败！");
                        return "0";
                    }
                }

                if (f.ReadString("CA", "yzyhbh", "0").Replace("\0", "").Trim() == "1")
                {
                    //获取OID值中的证件信息
                    string str1 = CertUID.Substring(CertUID.IndexOf("@") + 8);

                    //解码
                    byte[] bs = NETCAPKIv4.base64Decode(str1);
                    string Key_gh = Encoding.Default.GetString(bs);

                    if (Dt_yh.Rows[0]["F_YHBH"].ToString().Trim() != Key_gh)
                    {
                        if (msg == "1")
                            MessageBox.Show("用户工号与KEY中工号不一致,验证失败！");
                        return "0";
                    }
                }

                if (Dt_yh.Rows[0]["F_YHMC"].ToString().Trim() != YHMC)
                {
                    if (msg == "1")
                        MessageBox.Show("用户名称与KEY用户不一致,验证失败！");
                    return "0";
                }
                return "1";
            }
            else
            {
                MessageBox.Show("此Key未绑定用户");
                return "0";
            }
        }

        private string Login()
        {
            NETCAPKIv4 oNETCAPKIv4 = new NETCAPKIv4();
            ///// 获取用户证书
            string CertUID = "";
            string key_DW = "";
            string key_Name = "";
            string key_Sfzh = "";
            try
            {
                try
                {
                    SecuInter.X509Certificate oCert = NETCAPKIv4.getX509Certificate(
                    NETCAPKIv4.SECUINTER_CURRENT_USER_STORE, NETCAPKIv4.SECUINTER_MY_STORE,
                    NETCAPKIv4.SECUINTER_CERTTYPE_SIGN,
                    NETCAPKIv4.SECUINTER_NETCA_YES);
                    if (oCert == null)
                    {
                        MessageBox.Show("未找到证书");
                        return "0";
                    }
                    ////用户证书绑定值
                    CertUID = (NETCAPKIv4.getX509CertificateInfo(oCert, 9));
                    ////单位
                    key_DW = (NETCAPKIv4.getX509CertificateInfo(oCert, 13));
                    ////用户名称
                    key_Name = (NETCAPKIv4.getX509CertificateInfo(oCert, 12));
                    ////证书序列号
                    //CertID=(oNETCAPKIv4.GetCertInfo(Cert, 2));
                    ////证件号
                    key_Sfzh = (NETCAPKIv4.getX509CertificateInfo(oCert, 36));
                }
                catch (Exception ee1)
                {
                    if (ee1.Message == "证书选择失败")
                    {
                        MessageBox.Show("证书选择失败，请确认Key盘是否插入");
                    }
                    else
                    {
                        MessageBox.Show(ee1.Message);
                    }
                    return "0";
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("程序初始化失败");
                return "0";
            }


            /// 验证用户名
            DataTable Dt_yh = GetYHXX(CertUID);
            if (Dt_yh.Rows.Count > 0)
            {
                if (f.ReadString("CA", "yzsfzh", "0").Replace("\0", "").Trim() == "1")
                {
                    if (Dt_yh.Rows[0]["F_SFZH"].ToString().Trim() != key_Sfzh)
                    {
                        MessageBox.Show("用户身份证号不匹配,登陆失败！");
                        return "0";
                    }
                }
                if (f.ReadString("CA", "yzxm", "0").Replace("\0", "").Trim() == "1")
                {
                    if (key_Name != Dt_yh.Rows[0]["F_YHMC"].ToString().Trim())
                    {
                        MessageBox.Show("用户姓名不匹配,登陆失败");
                        return "0";
                    }
                }

                if (f.ReadString("CA", "yzyhbh", "0").Replace("\0", "").Trim() == "1")
                {
                    //获取OID值中的证件信息
                    string str1 = CertUID.Substring(CertUID.IndexOf("@") + 8);

                    //解码
                    byte[] bs = NETCAPKIv4.base64Decode(str1);
                    string Key_gh = Encoding.Default.GetString(bs);

                    if (Dt_yh.Rows[0]["F_YHBH"].ToString().Trim() != Key_gh)
                    {
                        MessageBox.Show("用户工号与KEY中工号不一致,验证失败！");
                        return "0";
                    }
                }

                return Dt_yh.Rows[0]["F_YHM"].ToString().Trim() + "^" + Dt_yh.Rows[0]["F_YHMM"].ToString().Trim();
            }
            else
            {
                MessageBox.Show("未查询到此用户信息或此用户Key未绑定");
                return "0";
            }
        }

        private void DeleteTempFile(string blh)
        {
            try
            {
                System.IO.Directory.Delete(@"c:\temp\" + blh, true);
            }
            catch
            {
            }
        }

        #region 将Base64编码的文本转换成普通文本

        /// <summary>
        /// 将Base64编码的文本转换成普通文本
        /// </summary>
        /// <param name="base64">Base64编码的文本</param>
        /// <returns></returns>
        public static string Base64StringToString(string base64)
        {
            if (base64 != "")
            {
                char[] charBuffer = base64.ToCharArray();
                byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
                string returnstr = Encoding.Default.GetString(bytes);
                return returnstr;
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region 字符串转为base64字符串

        public static string changebase64(string str)
        {
            if (str != "" && str != null)
            {
                byte[] b = Encoding.Default.GetBytes(str);
                string returnstr = Convert.ToBase64String(b);
                return returnstr;
            }
            else
            {
                return "";
            }
        }

        #endregion
    }
}