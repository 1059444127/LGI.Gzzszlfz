using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Collections;
using ZgqClassPub;


namespace PathHISZGQJK
{
    class cdrmyyPT
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");


        string debug = "";
        string ReprotFile = "";
        public void pathtohis_PT(string blh, string bglx, string bgxh, string msg, string debug1, string[] cslb)
        {
           dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
          DataTable dt_jcxx = new DataTable();
          MessageBox.Show(bgxh);

          if (bgxh == "")
              bgxh ="0";
          debug = f.ReadString("savetohis", "debug", "0");
         string  fsms = f.ReadString("savetohis", "fsms", "0");
          if (bglx == "bc")
              return;
          if (bglx == "bd")
              return;

            if (bglx == "bc")
            {
                dt_jcxx = aa.GetDataTable("select * from T_jcxx a left join T_bcbg b on a.F_blh= b.F_blh where a.F_blh='" + blh + "' and b.F_bc_bgxh='" + bgxh + "'", "jcxx");
            }
            else
            {
                dt_jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");

            }
            if (dt_jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                 log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (dt_jcxx.Rows.Count < 1)
            {
                //   MessageBox.Show("病理号有错误！");
                aa.ExecuteSQL("update T_JCXX_FS set F_bz='JCXX表中无记录',F_FSZT='不处理'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                 log.WriteMyLog("病理号有错误！");
                return;
            }

            //生成pdf，并转成二进制流的字符串

            string ReprotFile = ""; string jpgname = "";
            string ML = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");


            if (f.ReadString("savetohis", "ispdf", "0").Trim() == "1")
            {
                #region  生成pdf
                string message = "";
                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF,ref jpgname,"", ref message);

                string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                if (isrtn)
                {
                    if (debug == "1")
                        log.WriteMyLog("生成PDF成功");
                    //二进制串
                    if (File.Exists(jpgname))
                    {
                        try
                        {
                            if (f.ReadString("savetohis", "hxbg", "0") == "1")
                            {
                                FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
                                Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                                file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                                file.Close();
                                ReprotFile = Convert.ToBase64String(imgByte);
                                //   LGZGQClass.log.WriteMyLog("PDF转换二进制串成功"); 
                            }
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                            if (ssa == true)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("上传PDF成功");
                                MessageBox.Show(bgxh);

                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "批量上传", "审核", "生成PDF成功:" + ML + "\\" + jpgname, "ZGQJK", "生成PDF");
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                                aa.ExecuteSQL("update  T_JCXX_FS set F_ISPDF='1' where  F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            
                            }
                            else
                            {
                               log.WriteMyLog("上传PDF失败");
                            }
                        }
                        catch (Exception ee)
                        {
                             log.WriteMyLog("PDF转换二进制串失败");
                        }
                    }
                    else
                    {
                        log.WriteMyLog("未找到文件" + jpgname);
                        ZgqClass.BGHJ(blh, "批量上传", "审核", "未找到文件" + jpgname, "ZGQJK", "生成PDF");
                    }
                    zgq.DelTempFile(blh);

                }
                else
                {
                    log.WriteMyLog("生成PDF失败：" + message);
                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                }

                #endregion
            }

            if (dt_jcxx.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                aa.ExecuteSQL("update T_JCXX_FS set F_bz='申请序号为空不处理',F_FSZT='不处理'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                return;
            }


            if (f.ReadString("savetohis", "hxzt", "0") == "1")
            {
                #region 回写状态
                if (bglx == "cg")
                {


                    DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + dt_jcxx.Rows[0]["F_sqxh"].ToString().Trim() + "'", "jcxx");
                    if (dt_sqd.Rows.Count < 1)
                    {
                        aa.ExecuteSQL("delete  T_jcxx_zt where F_BLH='" + blh + "'");
                        return;
                    }
                    string message = ""; string brlb = "";

                    log.WriteMyLog("生成状态XML");
                    //生成发送状态的xml
                    bool rtn1 = bgzt_msg(dt_jcxx, dt_sqd, ref message, ref brlb, blh);
                    if (rtn1)
                    {

                        if (debug == "1")
                            log.WriteMyLog(message);
                        //发送状态
                        string rtn_zt = "";
                        if (fsms=="1")
                       rtn_zt= fszt_Web(blh, message, brlb, debug);
                        else
                       rtn_zt = fszt(blh, message, brlb, debug);

                       if (rtn_zt.Substring(0, 3) == "ERR")
                        {
                            aa.ExecuteSQL(" update T_jcxx_zt  set F_bz='" + rtn_zt + "'  where F_BLH='" + blh + "'");
                            log.WriteMyLog("状态消息发送失败：" + rtn_zt);
                        }
                        else
                        {
                            if (debug == "")
                                log.WriteMyLog("状态消息发送成功：" + rtn_zt);
                            aa.ExecuteSQL("delete  T_jcxx_zt where F_BLH='" + blh + "'");
                        }

                    }
                    else
                    {
                        if (message == "")
                            aa.ExecuteSQL("delete  T_jcxx_zt where F_BLH='" + blh + "'");
                        log.WriteMyLog("生成状态xml失败：" + message);
                    }
                }


                #endregion

            }

            if (f.ReadString("savetohis", "hxbg", "0") == "1")
            {
                #region 回写报告
                if (bglx == "cg")
                {
                    if (dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "已审核")
                    {
                        log.WriteMyLog("报告未审核，不处理！");
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='报告未审核,不处理',F_FSZT='不处理'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        return;
                    }
                }

                if (bglx == "bc")
                {
                    if (dt_jcxx.Rows[0]["F_bc_BGZT"].ToString().Trim() != "已审核")
                    {
                        log.WriteMyLog("报告未审核，不处理！");
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='补充报告未审核,不处理',F_FSZT='不处理'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        return;
                    }
                }


                DataTable dt_sqd = new DataTable();
                dt_sqd = aa.GetDataTable("select * from T_SQD where  F_sqxh='" + dt_jcxx.Rows[0]["F_SQXH"].ToString() + "'", "sqd");

                if (dt_sqd.Rows.Count <= 0)
                {
                    log.WriteMyLog("T_SQD表无此申请单记录，不处理！");
                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='T_SQD表无此申请单记录，不处理',F_FSZT='不处理'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    return;
                }


                msg = ""; string brlb = ""; string uid = ""; string exceptionmsg = "";
                bool rtnbool = Rtn_Message(dt_jcxx, dt_sqd, ref msg, ref brlb, ref uid, bglx, bgxh, ref exceptionmsg, blh);
                if (rtnbool)
                {
                    if (debug == "1")
                        log.WriteMyLog(msg);
                    string rtn_msg = "";
                    if (fsms=="1")
                     rtn_msg=   fsbg_Web(msg, dt_jcxx.Rows[0]["F_BRLB"].ToString());
                    else
                      rtn_msg= fsbg(msg, dt_jcxx.Rows[0]["F_BRLB"].ToString());
                    if (rtn_msg.Substring(0, 3) == "ERR")
                    {
                        log.WriteMyLog(rtn_msg);
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='发送报告失败：" + rtn_msg + "'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    }
                    else
                    {
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + rtn_msg + "',F_FSZT='已处理'  where  F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    }
                }
                else
                {
                    log.WriteMyLog("生成XML失败：" + exceptionmsg );
                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='生成XML失败：" + exceptionmsg + "'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    return;
                }
                #endregion
            }

        }


        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + int.Parse(blh[i].ToString()).ToString();
                }
                catch { }
            }
            return x;
        }

        public bool Rtn_Message(DataTable dt,DataTable dt_sqd, ref string msg, ref string xbrlb, ref string uid, string bglx, string bgxh, ref string exceptionmsg,string blh)
        {

            try
            {
                sqldb aa = new sqldb(Application.StartupPath + "\\sz.ini", "sqlserver");
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<ClinicalDocument xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../coreschemas/CDA.xsd\">";

                //<!--===================================-->
                //<!-- 检查报告                           -->
                //<!--===================================-->
                //<!--
                //********************************************************
                //  CDA Header
                //********************************************************
                //-->
                //    <!-- 文档适用范围编码 -->
                xml = xml + "<realmCode code=\"CN\"/>";
                //    <!-- 文档信息模型类别-标识符 -->
                //    <!-- 固定值 -->
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_HD000040\"/>";
                //    <!-- 文档标识-报告号 -->
                string bgh = dt.Rows[0]["f_BLH"].ToString();
                string bgys = dt.Rows[0]["f_bgys"].ToString();
                string shys = dt.Rows[0]["f_shys"].ToString();
                string bgrq = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMdd") + "0000";
                if (bglx == "bc")
                {
                    bgh = bgh + "_bc_" + bgxh;
                    bgys = dt.Rows[0]["f_bc_bgys"].ToString();
                    shys = dt.Rows[0]["f_bc_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMdd") + "0000";
                }
                if (bglx == "bd")
                {
                    bgh = bgh + "_bd_" + bgxh;
                    bgys = dt.Rows[0]["f_bd_bgys"].ToString();
                    shys = dt.Rows[0]["f_bd_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMdd") + "0000";
                }

                string bgysid = "";
                string shysid = "";
                System.Data.DataTable bgysdt = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + bgys + "'", "bgys");
                System.Data.DataTable shysdt = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + shys + "'", "bgys");
                System.Data.DataTable qcysdt = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + dt.Rows[0]["F_QCYS"].ToString() + "'", "qcys");
                string qcysid = "";
                if (bgysdt.Rows.Count > 0)
                {
                    bgysid = bgysdt.Rows[0]["F_yhbh"].ToString().Trim();
                }

                if (shysdt.Rows.Count > 0)
                {
                    shysid = shysdt.Rows[0]["F_yhbh"].ToString().Trim();
                }
                if (qcysdt.Rows.Count > 0)
                {
                    qcysid = qcysdt.Rows[0]["F_yhbh"].ToString().Trim();
                }



                if (ReprotFile.Trim() == "")
                {
                    //生成pdf，并转成二进制流的字符串

                    string jpgname = "";
                    string ML = DateTime.Parse(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");


                    if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                    {
                        #region  生成pdf

                        //string ML = "";
                        string message = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", ref message);

                        string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                        if (isrtn)
                        {
                            if (debug == "1")
                                log.WriteMyLog("生成PDF成功");
                            //二进制串
                            if (File.Exists(jpgname))
                            {
                                try
                                {
                                    FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
                                    Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                                    file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                                    file.Close();
                                    ReprotFile = Convert.ToBase64String(imgByte);
                                    //   LGZGQClass.log.WriteMyLog("PDF转换二进制串成功"); 

                                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                    if (ssa == true)
                                    {
                                        if (debug == "1")
                                            log.WriteMyLog("上传PDF成功");
                                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                        ZgqClass.BGHJ(blh, "批量上传", "审核", "生成PDF成功:" + ML + "\\" + jpgname, "ZGQJK", "生成PDF");
                                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                                    }
                                    else
                                    {
                                        log.WriteMyLog("上传PDF失败");
                                    }
                                }
                                catch (Exception ee)
                                {
                                    log.WriteMyLog("PDF转换二进制串失败");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("未找到文件" + jpgname);
                                ZgqClass.BGHJ(blh, "批量上传", "审核", "未找到文件" + jpgname, "ZGQJK", "生成PDF");
                            }
                            zgq.DelTempFile(blh);

                        }
                        else
                        {
                            log.WriteMyLog("生成PDF失败：" + message);
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        }

                        #endregion
                    }
                }



                if (ReprotFile.Trim() == "")
                {
                    exceptionmsg = "生成pdf,未能转成二进制流,2小时后在重传";
                    log.WriteMyLog("生成pdf,未能转成二进制流,2小时后在重传");
                    msg = "";
                    return false;
                }

                xml = xml + "<id root=\"S038\" extension=\"" + bgh + "\"/>";
                // <!-- 文档标识-名称 / 文档标识-类别编码 -->
                // <!-- 固定值 -->
                //需要确认相关的值
                xml = xml + "<code code=\"04\" codeSystem=\"1.2.156.112649.1.1.60\" displayName=\"检查检验记录\"/>";
                //    <!-- 文档标题文本 -->
                xml = xml + "<title>病理检查报告</title>";
                //<!-- 文档生效日期 -->
                //需要确认生效日期（立即生效还是延迟生效）
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" />";
                // <!-- 文档密级编码 -->
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"2.16.840.1.113883.5.25\" codeSystemName=\"Confidentiality\" displayName=\"normal\" />";
                // <!-- 文档语言编码 -->
                // <!-- 固定值 -->
                xml = xml + "<languageCode code=\"zh-CN\" />";
                //需要程序确定是新增还是修改
                string bgzt = dt.Rows[0]["F_yl7"].ToString().Trim();
                if (bglx == "bc")
                {
                    bgzt = dt.Rows[0]["F_bc_yl7"].ToString().Trim();
                }
                string xzzt = "0";
                if (bgzt != "")
                {
                    xzzt = "1";
                }
                xml = xml + "<versionNumber value=\"" + xzzt + "\"/>";

                //<!-- 文档记录对象 -->
                xml = xml + "<recordTarget typeCode=\"RCT\">";
                //<!-- 病人信息 -->
                xml = xml + "<patientRole classCode=\"PAT\">";
                //  <!-- 域ID -->
                if (dt.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                    xml = xml + "<id root=\"1.2.156.112649.1.2.1.2\" extension=\"03\" />";
                else if (dt.Rows[0]["F_brlb"].ToString().Trim() == "体检")
                    xml = xml + "<id root=\"1.2.156.112649.1.2.1.2\" extension=\"04\" />";
                else
                    xml = xml + "<id root=\"1.2.156.112649.1.2.1.2\" extension=\"01\" />";
                // <!-- 患者ID -->
                string patid = dt.Rows[0]["F_brbh"].ToString().Trim();
                xml = xml + "<id root=\"1.2.156.112649.1.2.1.3\" extension=\"" + patid + "\" />";
                //<!-- 就诊号 -->
                //string jzh = dt.Rows[0]["F_MZH"].ToString();
                //xbrlb = "01";
                //if (dt.Rows[0]["F_brlb"].ToString() == "住院")
                //{
                //    xbrlb = "03";
                //    jzh = dt.Rows[0]["F_zyh"].ToString();
                //}

                xml = xml + "<id root=\"1.2.156.112649.1.2.1.12\" extension=\"" + dt_sqd.Rows[0]["F_jzh"].ToString() + "\" />";
                // <!-- 影像号 -->
                xml = xml + "<id root=\"1.2.156.112649.1.2.1.5\" extension=\"" + dt.Rows[0]["F_BLH"].ToString() + "\" />";
                // <!-- 病区床号信息 -->
                xml = xml + "<addr use=\"TMP\">";
                // <!-- 病区 -->
                xml = xml + "<houseNumber>" + dt.Rows[0]["F_BQ"].ToString() + "</houseNumber>";
                // <!-- 床位号 -->
                xml = xml + "<careOf>" + dt.Rows[0]["F_ch"].ToString() + "</careOf>";
                xml = xml + "</addr>";
                //<!-- 病人基本信息 -->
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                //<!-- 病人名称 -->
                xml = xml + "<name>" + dt.Rows[0]["F_xm"].ToString() + "</name>";
                //<!-- 性别编码/性别名称 -->
                string xb = "";
                string xbdm = "0";
                if (dt.Rows[0]["F_xb"].ToString() == "男")
                {
                    xb = "男";
                    xbdm = "1";
                }
                if (dt.Rows[0]["F_xb"].ToString() == "女")
                {
                    xb = "女";
                    xbdm = "2";
                }
                xml = xml + "<administrativeGenderCode code=\"" + xbdm + "\" codeSystem=\"1.2.156.112649.1.1.3\" displayName=\"" + xb + "\" />";
                //<!-- 出生日期 -->
                xml = xml + "<birthTime value=\"" + dt_sqd.Rows[0]["F_CSRQ"].ToString() + "\" />";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";
                //<!-- 文档作者(检查报告医生, 可循环) -->
                xml = xml + "<author typeCode=\"AUT\">";
                //<!-- 报告日期 -->
                //报告日期没有时分秒
                xml = xml + "<time value=\"" + bgrq + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                //报告医生编码暂无
                //<!-- 报告医生编码 -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                //<!-- 报告医生名称 -->
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";
                //<!-- 文档保管者(CDA中custodian为必填项) -->
                xml = xml + "<custodian>";
                xml = xml + "<assignedCustodian>";
                xml = xml + "<representedCustodianOrganization>";
                //<!-- 医疗机构编码 -->
                xml = xml + "<id root=\"1.2.156.112649\" extension=\"44643245-7\" />";
                //<!-- 医疗机构名称 -->
                xml = xml + "<name>常德市第一人民医院</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";
                //<!-- 电子签章信息 -->
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time />";
                xml = xml + "<signatureCode code=\"S\" />";
                xml = xml + "<assignedEntity>";
                //<!-- 电子签章号-->";
                xml = xml + "<id extension=\"\" />";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";

                // <!-- 文档审核者(检查报告审核医师, 可循环) -->
                xml = xml + "<authenticator>";
                //<!-- 审核日期 -->
                string shsj = "";
                try
                {
                    shsj = Convert.ToDateTime(dt.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                }
                catch
                {
                }
                if (shsj == "")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                }
                if (bglx == "bc")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                xml = xml + "<time value=\"" + shsj + "\" />";
                xml = xml + "<signatureCode code=\"S\"/>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                //<!-- 审核医生编码 -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.2\" extension=\"" + shysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                //<!-- 审核医生名称 -->
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                //<!-- 申请医生信息 -->
                xml = xml + "<participant typeCode=\"AUT\">";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                //<!-- 申请医生编码 -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.2\" extension=\"\" />";
                xml = xml + "<associatedPerson>";
                //<!-- 申请医生名称 -->
                xml = xml + "<name>" + dt.Rows[0]["F_sjys"].ToString() + "</name>";
                xml = xml + "</associatedPerson>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";
                //<!-- 关联医嘱信息 -->
                xml = xml + "<inFulfillmentOf>";
                xml = xml + "<order>";
                //<!-- 关联医嘱号(可多个) -->
                xml = xml + "<id extension=\"" + dt.Rows[0]["F_yzid"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";
                //<!-- 文档中医疗卫生事件的就诊场景 -->
                xml = xml + "<componentOf typeCode=\"COMP\">";
                //<!-- 就诊信息 -->
                xml = xml + "<encompassingEncounter classCode=\"ENC\" moodCode=\"EVN\">";
                //<!-- 就诊次数 -->
                xml = xml + "<id root=\"1.2.156.112649.1.2.1.7\" extension=\"" + dt_sqd.Rows[0]["F_jzcs"].ToString() + "\"/>";
                xml = xml + "<code /><effectiveTime />";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";
                //<!--
                //********************************************************
                //CDA Body
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<structuredBody>";
                //<!-- 
                //********************************************************
                //文档中患者相关信息
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"34076-0\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Information for patients section\" />";
                xml = xml + "<title>文档中患者相关信息</title>";
                //<!-- 患者年龄 -->
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"397669002\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"age\" />";
                xml = xml + "<value xsi:type=\"ST\">" + dt.Rows[0]["F_nl"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";

                //<!--
                //********************************************************
                //检查章节
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"30954-2\" displayName=\"STUDIES SUMMARY\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\"/>";
                xml = xml + "<title>病理检查</title>";
                // <!-- 相关信息 -->
                xml = xml + "<entry>";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"310388008\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"relative information status\" />";
                xml = xml + "<statusCode code=\"completed\" />";
                //<!-- 定位图像信息 -->
                xml = xml + "<component>";
                xml = xml + "<supply classCode=\"SPLY\" moodCode=\"EVN\">";
                //<!-- 图像索引号(accessionNumber) -->
                xml = xml + "<id extension=\"" + "1001" + "\" />";
                xml = xml + "</supply>";
                xml = xml + "</component>";
                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //<!-- 检查报告类型标识编码/检查报告类型标识名称 --0正常报告，1补充报告>
                string bglxstr = "0";
                if (bglx == "bc")
                {
                    bglxstr = "1";
                }
                xml = xml + "<code code=\"" + bglxstr + "\" codeSystem=\"1.2.156.112649.1.1.112\" displayName=\"病理检查报告\" />";
                xml = xml + "</observation>";
                xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                ////<!--****************************************************************************-->
                ////<!-- 检查报告条目 -->
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                //<!-- 检查类型编码/检查类型名称 --> 
                //检查类型编码没有，名称暂用OT
                xml = xml + "<code code=\"OT\" codeSystem=\"1.2.156.112649.1.1.41\" displayName=\"病理\" />";
                // <!-- 必须固定项 -->
                xml = xml + "<statusCode code=\"completed\"/>";
                // <!-- 检查使用试剂信息 -->
                xml = xml + "<participant typeCode=\"CSM\">";
                xml = xml + "<participantRole>";
                xml = xml + "<playingEntity>";
                //<!-- 试剂编码/试剂名称 -->
                xml = xml + "<code code=\"\" displayName=\"\" />";
                //<!-- 试剂用量及单位 -->
                xml = xml + "<quantity value=\"\" unit=\"\" />";
                xml = xml + "</playingEntity>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";
                //     <!-- study 1 -->
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //!-- 检查项目编码/检查项目名称 -->
            
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_yzxmbm"].ToString() + "\" codeSystem=\"1.2.156.112649.1.1.88\" displayName=\"" + dt_sqd.Rows[0]["F_yzxmmc"].ToString() + "\"/>";
                //<!-- 检查备注 -->
                xml = xml + "<text></text>";
                // <!-- 必须固定项 -->
                xml = xml + "<statusCode code=\"completed\"/>";
                //<!-- 检查报告结果-客观所见/影像学表现(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112649.1.1.98\">";
                xml = xml + "<originalText> " + System.Security.SecurityElement.Escape(dt.Rows[0]["F_RYSJ"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_JXSJ"].ToString()) + "</originalText>";
                xml = xml + "</value>";
                // <!-- 检查报告结果-主观提示/影像学结论(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112649.1.1.98\">";
                string blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_blzd"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_TSJC"].ToString()); ;
                if (bglx == "bc")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bczd"].ToString());
                }
                xml = xml + "<originalText>" + blzd + "</originalText>";
                xml = xml + "</value>";
                //<!-- 检查方法编码/检查方法名称 -->
                xml = xml + "<methodCode code=\"001\"  codeSystem=\"1.2.156.112649.1.1.43\" displayName=\"\"/>";
                //<!-- 检查部位编码/检查部位名称 -->
                xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156112649.1.1.42\" displayName=\"\" />";
                // <!-- 诊断医师信息 -->
                xml = xml + "<performer typeCode=\"PRF\">";
                //<!-- 诊断日期 -->
                xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                xml = xml + "<assignedEntity>";
                //<!-- 诊断医生编码 -->
                xml = xml + "<id  root=\"1.2.156.112649.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                //<!-- 诊断医生名称 -->
                xml = xml + "<name>" + dt.Rows[0]["F_bgys"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                // <!-- 诊断科室编码 -->
                //需提供准确的病理科的代码,如果没有就删除
                xml = xml + "<id root=\"1.2.156.112649.1.1.1\" extension=\"2070000\"/>";
                //<!-- 诊断科室名称 -->
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                //<!-- 检查医师信息 -->
                xml = xml + "<performer>";
                // <!-- 检查日期 -->
                try
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                xml = xml + "<assignedEntity>";
                //<!-- 检查医生编码 -->
                xml = xml + "<id  root=\"1.2.156.112649.1.1.2\" extension=\"" + qcysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                // <!-- 检查医生名称 -->
                xml = xml + "<name>" + dt.Rows[0]["F_QCYS"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //<!-- 检查科室编码 -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.1\" extension=\"2070000\"/>";
                //<!-- 检查科室名称 -->
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                ////<!-- 仪器信息 -->
                ////<participant typeCode="DEV">
                ////<participantRole>
                ////<playingDevice>
                ////<!--仪器型号 仪器名称-->
                ////<manufacturerModelName code="LOGIQ-9"  displayName="Agilent Mx3000P"/>
                ////</playingDevice>
                ////</participantRole>
                ////</participant>	

                ////<!-- 仪器或医生客观所见信息(超声心动报告等结构化所见部分的信息) -->
                ////<entryRelationship typeCode="COMP">
                ////<organizer classCode="BATTERY" moodCode="EVN">
                ////<code code="365605003" codeSystem="2.16.840.1.113883.6.96" codeSystemName="SNOMED CT" displayName="body measurement finding" />
                ////<statusCode code="completed" />

                ////<!-- 项目信息(可循环) -->
                ////<component>
                ////<observation classCode="OBS" moodCode="EVN">
                ////<code code="100" displayName="AOD" />
                ////<!--<value xsi:type="SC">1mm</value>-->
                ////<value xsi:type="PQ" value="73" unit="毫秒"></value>
                ////</observation>
                ////</component>

                ////<component>
                ////<observation classCode="OBS" moodCode="EVN">
                ////<code code="200" displayName="LAD" />
                ////<value xsi:type="SC">1mm</value>
                ////</observation>
                ////</component>
                ////<component>
                ////<observation classCode="OBS" moodCode="EVN">
                ////<code code="300" displayName="FS" />
                ////<value xsi:type="SC">33.3%</value>
                ////</observation>
                ////</component>	
                ////<!-- 其它信息按上面格式添加 -->
                ////</organizer>
                ////</entryRelationship>

                // <!-- 图像信息(能与项目对应的图像) -->
                //xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                //xml = xml + "<observationMedia   classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/jpg\">" ;
                //xml = xml + "</value>";
                //xml = xml + "</observationMedia>";
                //xml = xml + "</entryRelationship>";
                //<!-- 当有多个影像对应同一个study时,可以复用此entryRelationship -->
                xml = xml + "</observation></component>";
                ////<!-- study 2 -->
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"1\" codeSystem=\"1.2.156.112649.1.1.88\" displayName=\"\"/>";
                xml = xml + "<text></text>";
                xml = xml + "<statusCode code=\"completed\"/>";

                //        <!-- 检查报告结果-客观所见/影像学表现(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112649.1.1.98\">";
                xml = xml + "<originalText></originalText>";
                xml = xml + "</value>";
                //        <!-- 检查报告结果-主观提示/影像学结论(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112649.1.1.98\">";
                xml = xml + "<originalText></originalText>";
                xml = xml + "</value>";
                //        <!-- 检查方法编码/检查方法名称 -->
                xml = xml + "<methodCode code=\"002\"  codeSystem=\"1.2.156.112649.1.1.43\" displayName=\"\"/>";
                //        <!-- 检查部位编码/检查部位名称 -->
                xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156.112649.1.1.42\" displayName=\"\" />";
                //        <!-- 检查医师信息 -->
                xml = xml + "<performer>";
                xml = xml + "<time value=\"201112310910\"/>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<id  root=\"1.2.156.112649.1.1.2\" extension=\"9879\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112649.1.1.1\" extension=\"98712\"/>";
                xml = xml + "<name></name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";




                xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                //  <!-- 影像信息(要求编码为BASE64), @mediaType: 影像格式 -->

                if(debug=="1")
                    xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/pdf\">" + "1234567890";
                else
                xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/pdf\">" + ReprotFile;
                xml = xml + "</value>";
                xml = xml + "</observationMedia>";
                xml = xml + "</entryRelationship>";
                // <!-- 当有多个影像对应同一个study时,可以复用此entryRelationship -->
                xml = xml + "</observation>";
                xml = xml + "</component>";

                //<!-- 其他项目按上面结构和格式添加 -->

                //<!-- 当系统所生成的报告中,图像无法与具体的study做对应时,使用以下部分来放置影像 -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- 影像信息(要求编码为BASE64), @mediaType: 影像格式 -->
                // xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/gif\">";
                // xml = xml + "</value>";
                // xml = xml + "</observationMedia>";
                // xml = xml + "</component>";

                ////<!-- 当有多个影像时,按照以上格式添加 -->
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";

                // <!-- 当系统中,客观所见(和主观意见)无法对应到具体的study, 
                // 而是多个study的客观所见(和主观意见)记录在同一个文本字段中,
                // 使用以下部分来放置客观所见和主观意见 -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // //<!-- @code:01表示客观所见, 02表示主观提示 -->
                // xml = xml + "<code code=\"01\" codeSystem=\"1.2.156.112649.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //// <!-- @code:01表示客观所见, 02表示主观提示 -->
                // xml = xml + "<code code=\"02\" codeSystem=\"1.2.156.112649.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";
                //********************************************************
                //临床资料
                //* *******************************************************
                xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code /><value xsi:type=\"ED\"></value></observation></entry></section></component>";
                //<!-- 
                //****************************************************************************
                //  #药观章节
                //****************************************************************************
                //-->
                xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code code=\"123\" displayName=\"药观名称\"/></observation></entry></section></component>";
                //<!-- 
                //********************************************************
                //诊断
                //********************************************************
                //-->
                xml = xml + "<component><section><code code=\"29308-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Diagnosis\"/> "
                                   + "<title>诊断</title>"
                                   + "<entry typeCode=\"DRIV\">"
                                       + "<act classCode=\"ACT\" moodCode=\"EVN\">"
                                           + "<code nullFlavor=\"NA\"/>"
                                           + "<entryRelationship typeCode=\"SUBJ\">"
                                               + "<observation classCode=\"OBS\" moodCode=\"EVN\">"
                    // <!-- 诊断类别编码/诊断类别名称 -->
                                                   + "<code code=\"\" codeSystem=\"1.2.156.112635.1.1.29\" displayName=\"\" />"
                                                   + "<statusCode code=\"completed\"/>"
                    // <!-- 疾病编码/疾病名称(没有编码去掉@code) -->
                                                   + "<value xsi:type=\"\" code=\"\" codeSystem=\"1.2.156.112635.1.1.30\" displayName=\"\" />"
                                              + " </observation>"
                                           + "</entryRelationship>"
                                       + "</act>"
                                  + "</entry>"
                               + "</section>"
                           + "</component>";

                //<!--
                //********************************************************
                //补充意见章节（TCT检查报告单用）
                //********************************************************
                //-->
                //            <component>
                //                <section>
                //                    <code code="52535-2" codeSystem="2.16.840.1.113883.6.1" codeSystemName="LOINC" displayName="Other useful information" />
                //                    <!-- 章节标题 -->
                //                    <title>补充意见信息</title> 
                //                    <!-- 补充意见内容 -->
                //                    <text>补充意见内容</text>
                //                </section>
                //            </component>

                xml = xml + "</structuredBody>";
                xml = xml + "</component>";

                xml = xml + "</ClinicalDocument>";
                msg = xml;

                return true;
            }
            catch (Exception ee)
            {
                exceptionmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                msg = "";
                return false;

            }
        }

        public string fsbg_Web(string message,string brlb)
        {

       
            try
            {
            cdrmyymq.Service mq = new PathHISZGQJK.cdrmyymq.Service();
            string rtn=  mq.MQSendMessage(message, "BS320", brlb, "IN.BS320.LQ");

            return rtn;
                       
            }
            catch (Exception ex)
            {
                log.WriteMyLog("消息发送异常！" + ex.Message);
                return "ERR:" + ex.Message;
            }

        }
        public string fsbg(string message, string brlb)
        {


            //try
            //{

            //    string rtn = MQSendMessage(message, "BS320", brlb, "IN.BS320.LQ");

            //    return rtn;

            //}
            //catch (Exception ex)
            //{
            //     LGZGQClass.log.WriteMyLog("消息发送异常！" + ex.Message);
            //    return "ERR:" + ex.Message;
            //}

            return "";

        }
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        public string fszt_Web(string blh,string message,string brlb, string debug)
        {
         
                    try
                    {
                        cdrmyymq.Service mq = new PathHISZGQJK.cdrmyymq.Service();
                        string rtn_msg = mq.MQSendMessage(message, "BS004", brlb, "IN.BS004.LQ");
                        return rtn_msg;
                    }
                    catch (Exception ex)
                    {
                     return  "ERR:" + ex.Message;
                      
                    }
        }
        public string fszt(string blh, string message, string brlb, string debug)
        {

            //try
            //{
       
            //    string rtn_msg = MQSendMessage(message, "BS004", brlb, "IN.BS004.LQ");
            //    return rtn_msg;
            //}
            //catch (Exception ex)
            //{
            //    return "ERR:" + ex.Message;

            //}

            return "";
        }

        public bool bgzt_msg(DataTable dt_jcxx, DataTable dt_sqdxx, ref string msg, ref string xbrlb, string blh)
        {
            xbrlb = dt_sqdxx.Rows[0]["F_y_brlb"].ToString();
            string bgztbm = "";
            string bgztstr = "";
            string  bgzt=dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim();
            if (bgzt == "已审核")
            {
                bgztbm = "92";
                bgztstr = "检查报告已审核";
            }
            if (bgzt == "已写报告")
            {
                bgztbm = "73";
                bgztstr = "检查已完成";
            }
            if (bgzt == "报告延期")
            {
                bgztbm = "73";
                bgztstr = "报告延期";
            }
            if (bgzt == "已取材")
            {
                bgztbm = "51";
                bgztstr = "检查已到检";
            }
            if (bgzt == "已登记")
            {
                bgztbm = "51";
                bgztstr = "检查已到检";
            }
            if (bgzt == "取消审核")
            {
                bgztbm = "100";
                bgztstr = "报告召回";
            }

            if (bgztstr == "" || bgztbm.Trim() == "")
            {
                msg = "";
                return false;
            }
            try
            {
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3  ../../Schemas/POOR_IN200901UV23.xsd\">";

                xml = xml + "<!-- 消息ID -->";
                xml = xml + "<id extension=\"BS004\" />";
                xml = xml + "<!-- 消息创建时间 -->";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<!-- 交互ID -->";
                xml = xml + "<interactionId extension=\"POOR_IN200901UV23\" />";
                xml = xml + "<!--消息用途: P(Production); D(Debugging); T(Training) -->";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<!-- 消息处理模式: A(Archive); I(Initial load); R(Restore from archive); T(Current  processing) -->";
                xml = xml + "<processingModeCode code=\"R\" />";
                xml = xml + "<!-- 消息应答: AL(Always); ER(Error/reject only); NE(Never) -->";
                xml = xml + "<acceptAckCode code=\"NE\" />";
                xml = xml + "<!-- 接受者 -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 接受者ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";
                xml = xml + "<!-- 发送者 -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 发送者ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"S007\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";
                xml = xml + "<!-- 封装的消息内容(按Excel填写) -->";
                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<!-- 消息交互类型 @code: 新增 :new 修改:update -->";
                xml = xml + "<code code=\"update\"></code>";
                xml = xml + "<subject typeCode=\"SUBJ\" xsi:nil=\"false\">";
                xml = xml + "<placerGroup>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<code></code>";
                xml = xml + "<!-- 检验申请单状态 必须项未使用 -->";
                xml = xml + "<statusCode code=\"active\"></statusCode>";
                xml = xml + "<!-- 患者信息 -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<id>";
                xml = xml + "<!--域ID -->";
                xml = xml + "<item root=\"1.2.156.112649.1.2.1.2\" extension=\"" + "" + "\" />";
                xml = xml + "<!-- 患者ID -->";
                xml = xml + "<item root=\"1.2.156.112649.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- 就诊号 -->";
                xml = xml + "<item root=\"1.2.156.112649.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_JZH"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"";
                xml = xml + "determinerCode=\"INSTANCE\">";
                xml = xml + "<!--病人科室编码-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_KSDM"].ToString() + "\" root=\"1.2.156.112649.1.1.1\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--病人科室名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_KSMC"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--医疗机构代码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_YYDM"].ToString() + "\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--医疗机构名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item><part value=\"" + dt_sqdxx.Rows[0]["F_YYMC"].ToString() + "\" /></item>";
                xml = xml + "</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</providerOrganization>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "<!-- 操作人 -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<time>";
                xml = xml + "<!-- 操作日期 -->";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</time>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 操作人编码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"01003\" root=\"1.2.156.112649.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\"";
                xml = xml + "classCode=\"PSN\">";
                xml = xml + "<!-- 操作人姓名 必须项已使用 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"积显\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<!--执行科室 -->";
                xml = xml + "<location typeCode=\"LOC\" xsi:nil=\"false\">";
                xml = xml + "<!--必须项未使用 -->";
                xml = xml + "<time />";
                xml = xml + "<!--就诊机构/科室 -->";
                xml = xml + "<serviceDeliveryLocation classCode=\"SDLOC\">";
                xml = xml + "<serviceProviderOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--执行科室编码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ZXKSDM"].ToString() + "\" root=\"1.21.156.112649.1.1.1\" />";
                xml = xml + "</id>";
                xml = xml + "<!--执行科室名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + "病理科" + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</serviceDeliveryLocation>";
                xml = xml + "</location>";
                xml = xml + "<!-- 1..n可循环  医嘱状态信息 -->";
                xml = xml + "<component2>";
                xml = xml + "<!--医嘱序号-->";
                xml = xml + "<sequenceNumber value=\"1\"/>";
                xml = xml + "<observationRequest classCode=\"OBS\">";
                xml = xml + "<!-- 必须项已使用 -->";
                xml = xml + "<id>";
                xml = xml + "<!-- 医嘱号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_YZH"].ToString() + "\" root=\"1.2.156.112649.1.2.1.22\"/>";
                xml = xml + "<!-- 申请单号 -->";
                xml = xml + "<item extension=\"" + dt_jcxx.Rows[0]["F_SQXH"].ToString()+"\" root=\"1.2.156.112649.1.2.1.21\"/>";
                xml = xml + "<!-- 报告号 -->";
                xml = xml + "<item extension=\"" + blh + "\" root=\"1.2.156.112649.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112649.1.2.1.30\"/>";
                xml = xml + "</id>";
                xml = xml + "<!-- 医嘱类别编码/医嘱类别名称 - 针剂药品, 材料类, 治疗类, 片剂药品, 化验类 -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112649.1.1.27\">";
                xml = xml + "<displayName value=\"检查\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";
                xml = xml + "<!-- 标本信息 -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                xml = xml + "<specimen classCode=\"SPEC\">";
                xml = xml + "<!--标本条码号 必须项已使用 -->";
                xml = xml + "<id extension=\"" + "" + "\" />";
                xml = xml + "<!--必须项目未使用 -->";
                xml = xml + "<code />";
                xml = xml + "<subjectOf1 typeCode=\"SBJ\" contextControlCode=\"OP\">";
                xml = xml + "<specimenProcessStep moodCode=\"EVN\"";
                xml = xml + "classCode=\"SPECCOLLECT\">";
                xml = xml + "<!-- 采集日期 -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</effectiveTime>";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 采集人Id -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"01003\" root=\"1.2.156.112649.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\"";
                xml = xml + "classCode=\"PSN\">";
                xml = xml + "<!-- 采集人姓名 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"积显\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "</specimenProcessStep>";
                xml = xml + "</subjectOf1>";
                xml = xml + "</specimen>";
                xml = xml + "</specimen>";
                xml = xml + "<!-- 原因 -->";
                xml = xml + "<reason contextConductionInd=\"true\">";
                xml = xml + "<observation moodCode=\"EVN\" classCode=\"OBS\">";
                xml = xml + "<!-- 必须项 未使用-->";
                xml = xml + "<code></code>";
                xml = xml + "<value xsi:type=\"ST\" value=\"登记\"/>";
                xml = xml + "</observation>";
                xml = xml + "</reason>";
                xml = xml + "<!-- 医嘱执行状态 -->";
                xml = xml + "<component1 contextConductionInd=\"true\">";
                xml = xml + "<processStep classCode=\"PROC\">";
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112649.1.1.93\">";
                xml = xml + "<!--医嘱执行状态名称 -->";
                xml = xml + "<displayName value=\"" + bgztstr + "\" />";
                xml = xml + "</code>";
                xml = xml + "</processStep>";
                xml = xml + "</component1>";
                xml = xml + "</observationRequest>";
                xml = xml + "</component2>";
                xml = xml + "<!--就诊 -->";
                xml = xml + "<componentOf1 contextConductionInd=\"false\" xsi:nil=\"false\"";
                xml = xml + "typeCode=\"COMP\">";
                xml = xml + "<!--就诊 -->";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<id>";
                xml = xml + "<!-- 就诊次数 必须项已使用 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112649.1.2.1.7\" />";
                xml = xml + "<!-- 就诊流水号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112649.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--就诊类别编码-->";
                xml = xml + "<code codeSystem=\"1.2.156.112649.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
                xml = xml + "<!-- 就诊类别名称 -->";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_BRLB"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!--必须项未使用 -->";
                xml = xml + "<statusCode code=\"Active\" />";
                xml = xml + "<!--病人 必须项未使用 -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\" />";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf1>";
                xml = xml + "</placerGroup>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</POOR_IN200901UV>";
                msg = xml;

                return true;
            }
            catch(Exception  ex2)
            {
                msg = "ERR:"+ex2.Message; return false;
            }
        }

        //public string MQSendMessage(string txt, string service_id, string brlb, string queueManager)
        //{
        //    MQQueueManager qMgr = null;
        //    Hashtable env = new Hashtable();

        //string hostname = "172.16.13.42";
        //string channel = "IE.SVRCONN";
        //string qManager = "GWI.QM";
     
       
        //    hostname = f.ReadString("MQSERVER", "hostname", "172.16.13.42").Replace("\0", "");
        //    channel = f.ReadString("MQSERVER", "channel", "IE.SVRCONN").Replace("\0", "");
        //    qManager = f.ReadString("MQSERVER", "qManager", "GWI.QM").Replace("\0", "");
        // //   queueManager = f.ReadString("MQSERVER", "queueManager", "IN.BS320.LQ").Replace("\0", "");

        //    try
        //    {
        //        env.Clear();
        //        env.Add(IBM.XMS.MQC.HOST_NAME_PROPERTY, hostname);
        //        env.Add(IBM.XMS.MQC.CHANNEL_PROPERTY, channel);
        //        env.Add(IBM.XMS.MQC.CCSID_PROPERTY, 1208);
        //        env.Add(IBM.XMS.MQC.PORT_PROPERTY, 6000);
        //        env.Add(IBM.XMS.MQC.TRANSPORT_PROPERTY, IBM.XMS.MQC.TRANSPORT_MQSERIES);
        //        env.Add(IBM.XMS.MQC.USER_ID_PROPERTY, "mqm");


        //        int openOptions = MQC.MQOO_OUTPUT
        //                | MQC.MQPMO_PASS_ALL_CONTEXT;
        //        MQMessage msg = new MQMessage();
        //        //连接队列管理器
        //        qMgr = new MQQueueManager(qManager, env);
        //        msg.CharacterSet = 1208;
        //        MQQueue queue = qMgr.AccessQueue(queueManager, openOptions);
        //        msg.Format = MQC.MQFMT_STRING;

        //        // 8个消息头
        //        /// 消息ID   Y
        //        msg.SetStringProperty("service_id", service_id);
        //        //就诊类别ID  Y
        //        //01 门诊,02 急诊,03 住院,04 体检,05 转院
        //        if (brlb == "急诊") brlb = "02";
        //        if (brlb == "门诊") brlb = "01";
        //        if (brlb == "住院") brlb = "03";
        //        if (brlb == "体检") brlb = "04";
        //        if (brlb == "转院") brlb = "05";

        //        msg.SetStringProperty("domain_id", brlb);
        //        // 申请科室ID   YY
        //        msg.SetStringProperty("apply_unit_id", "0");  //可以按照实际申请科室ID填写，也可以填写0
        //        // 发送系统ID   YY
        //        msg.SetStringProperty("send_sys_id", "S007");  //每个系统都有一个系统编号，病理的系统ID是S007，是固定的。
        //        // 医疗机构代码  YY
        //        msg.SetStringProperty("hospital_id", "44643245-7"); //常德市一医院的医疗机构代码，是固定值
        //        // 执行科室ID  Y
        //        msg.SetStringProperty("exec_unit_id", "2070000");  //是执行科室编码，需要按照实际填写
        //        // 医嘱执行分类编码  
        //        /*	0201	常规检验（LIS）
        //            0101	超声检查
        //            0102	核医学检查
        //            0103	放射检查
        //            0104	病理检查
        //            0105	内窥镜检查
        //            0106	心电检查
        //            0107	膀胱镜检查
        //            0108	超声心动检查
        //            0202	血液形态学检验
        //            0301	手术麻醉
        //            0601	会诊
        //            0	未分类 */
        //        msg.SetStringProperty("order_exec_id", "0");  //常德预留字段，暂时可以填0
        //        // 扩展码   这个字段用于标识收费
        //        //msg.setStringProperty("extend_sub_id", "1");  //收费后，这个字段填1，是固定值

        //        string aa = txt;  //表示会将此XML消息发送至平台
        //        msg.WriteString(aa);
        //        MQPutMessageOptions pmo = new MQPutMessageOptions();
        //        pmo.Options = MQC.MQPMO_SYNCPOINT;
        //        queue.Put(msg, pmo);
        //        queue.Close();
        //        qMgr.Commit();
        //        string messageid = byteToHexStr(msg.MessageId);
        //        qMgr.Disconnect();
        //        return messageid;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "ERR:" + ex.Message;
        //    }
        //}
        
    }
}
