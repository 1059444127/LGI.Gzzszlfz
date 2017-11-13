using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using dbbase;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;
using System.Xml;
using ZgqClassPub;
using LoadDll;

namespace PathHISZGQJK
{
    /// <summary>
    /// 长沙市妇幼保健院
    /// </summary>
    class cssfy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        DataSet jb03_ds;
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        private  static string f_blh = "";
        public void pathtohis(string blh, string bglx,string bgxh,string czlb,string dz,string msg,string debug)
        {
            f_blh = blh;

            msg = f.ReadString("savetohis", "msg", "");
            string zxks = f.ReadString("门诊号", "zxks", "222");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ","+ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }


             if(jcxx.Rows[0]["F_bgzt"].ToString().Trim() == "已审核"||jcxx.Rows[0]["F_bgzt"].ToString().Trim() == "已发布")
             {
                 if (jcxx.Rows[0]["F_brlb"].ToString().Trim() != "体检")
                 {
                     string errmsg = ""; string pdfpath = ""; string filename = ""; string Base64String = "";
                     #region  生成pdf
                     string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                     C_PDF(ML, blh, bgxh, bglx, ref errmsg, ref pdfpath, ref filename, true, false, false, ref Base64String, debug);
                     #endregion
                 }
             }

             if (dz == "qxsh")
             {
                 aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
             }


            if (bglx == "bc" )
                return;

            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
            {
                if (jcxx.Rows[0]["F_sqxh"].ToString().Trim() != "")
                {
                   // string readxml = jb032("0", jcxx.Rows[0]["F_mzh"].ToString(), jcxx.Rows[0]["F_brbh"].ToString(), "0", jcxx.Rows[0]["F_sqxh"].ToString(), "", "", "", zxks, "0");
                    string readxml = jb032("0", jcxx.Rows[0]["F_mzh"].ToString(), jcxx.Rows[0]["F_brbh"].ToString(), "0","0", "", "", "", zxks, "0");
                    jb03_ds = new DataSet();
                    try
                    {
                        StringReader xmlstr = null;
                        XmlTextReader xmread = null;
                        xmlstr = new StringReader(readxml);
                        xmread = new XmlTextReader(xmlstr);
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.Load(xmread);

                        XmlNode node = xmldoc.SelectSingleNode("//ROWDATA");
                        string A = node.OuterXml;
                        A = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>" + A;
                        xmlstr = new StringReader(A);
                        xmread = new XmlTextReader(xmlstr);

                        jb03_ds.ReadXml(xmread);
                        if (jb03_ds.Tables.Count > 0)
                        {
                            for (int i = 0; i < jb03_ds.Tables[0].Rows.Count; i++)
                            {
                                qf01(i.ToString(), "1", "1", zxks);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                       MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ","+ex.Message);
                    }
                }

            }
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() != "体检")
            {
                //不检查审核状态
                if (jcxx.Rows[0]["F_bgzt"].ToString().Trim() != "已审核"&&bglx!="bd")
                {
                    return;
                }
                //string bgzt = dataGridView1.CurrentRow.Cells["报告状态"].Value.ToString().Trim();
                if (jcxx.Rows[0]["F_brbh"].ToString().Trim() == "")
                {
                    log.WriteMyLog(blh + "，无病人编号，不处理！");
                    return;
                }

                string brlb = "";

                string brbh = "";
                string brxb = "";
                string zyh = "";
                string yzid = "";
                string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                string sdrq = jcxx.Rows[0]["F_sdrq"].ToString().Trim();
                string bgrq = jcxx.Rows[0]["F_bgrq"].ToString().Trim();
                brxb = jcxx.Rows[0]["F_xb"].ToString().Trim();
                brxm = jcxx.Rows[0]["F_xm"].ToString().Trim();
                brbh = jcxx.Rows[0]["F_brbh"].ToString().Trim();
                // blh = jcxx.Rows[0]["F_blh"].ToString().Trim();
                zyh = jcxx.Rows[0]["F_zyh"].ToString().Trim();
                yzid = jcxx.Rows[0]["F_yzid"].ToString().Trim();
                string his_blh = "";
                if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                {

                    brlb = "1";
                    his_blh = jcxx.Rows[0]["F_zyh"].ToString().Trim();
                    //zybh = jcxx.Rows[0]["F_yzid"].ToString().Trim();
                }
                else
                {
                    if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
                    {
                        brlb = "0";
                        his_blh = jcxx.Rows[0]["F_mzh"].ToString().Trim();
                    }
                    //zybh = jcxx.Rows[0]["F_brbh"].ToString().Trim();
                }
                bg012(brlb, brbh, blh, "1", his_blh, brxb, bgrq, yzid, sdrq, shys);
            }
            else
            {
                string pdfpath = "";
                //--体检回传----------------------------
                //  string msg = f.ReadString("savetohis", "msg", "");
                //string odbcstr = f.ReadString("savetohis", "odbcsql", "Data Source=192.168.0.1;Initial Catalog=zonekingnet;User Id=sa;Password=zoneking;"); //获取sz.ini中设置的odbcsql
                string odbcstr = f.ReadString("savetohis", "odbcsql", "DSN=pathnet-tj;UID=sa;PWD=zoneking;"); //获取sz.ini中设置的odbcsql
                try
                {
                    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                    {
                        MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ",此病例无体检号或申请单号，不上传！");
                        return;
                    }
                    if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                    {

                        string jpgpath = "";
                        string ftpstatus = "";
                        mdjpg mdj = new mdjpg();
                        try
                        {

                            string errmsg = "";  string filename = "";
                            #region  生成pdf
                            string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                           // C_PDF(ML, blh, bgxh, bglx, ref errmsg, ref pdfpath, ref filename, true, false, false, ref Base64String, debug);
                       
                            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                            {
                                #region  生成pdf

                                ZgqPDFJPG zgq = new ZgqPDFJPG();
                                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.JPG, ref filename, "", ref errmsg);
                               
                                if (isrtn)
                                {
                                    //二进制串
                                    if (!File.Exists(filename))
                                    {
                                        ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：未找到文件" + filename, "ZGQJK", "生成PDF");
                                        log.WriteMyLog("未找到文件" + filename);
                                        errmsg = "未找到文件" + filename;
                                        zgq.DelTempFile(blh);
                                       
                                        return;
                                    }
                                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF成功", "ZGQJK", "生成PDF");
                              
                                    bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg,3,ref pdfpath);
                                    if (ssa == true)
                                    {
                                        if (debug == "1")
                                            log.WriteMyLog("上传PDF成功");
                                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + ML + "\\" + filename, "ZGQJK", "上传PDF");

                                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + filename + "')");
                                    }
                                    else
                                    {
                                        log.WriteMyLog("上传PDF失败：" + errmsg);
                                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
                                    }
                                    zgq.DelTempFile(blh);
                                }
                                else
                                {
                                    log.WriteMyLog("生成PDF失败：" + errmsg);
                                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：" + errmsg, "ZGQJK", "生成PDF");
                                    zgq.DelTempFile(blh);
                                    return;
                                }
                                #endregion
                            }
                            else
                            {
                                log.WriteMyLog("不需生成PDF");
                                return;
                            }
                            #endregion

                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ",生成JPG文件异常：" + ee.ToString());
                            return;
                        }
                      

                        //////////////////////////////////////////////////////////
                        ///////回写体检数据库-T_SYN_TCT_CHECK////////////////////////////////////

                        string tj_blzd = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                        string tj_jcsj = jcxx.Rows[0]["F_rysj"].ToString().Trim();
                        string tj_ysyj = jcxx.Rows[0]["F_BZ"].ToString().Trim();
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                        if (TJ_bljc.Rows.Count > 0)
                        {
                            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT体检" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                            {
                                tj_jcsj = "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + "病原微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\n";
                                ////////////////////////////////////
                                tj_blzd = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\n" + "炎症程度：" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim(); ;
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_ysyj = tj_ysyj + "补充意见1：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                    tj_ysyj = tj_ysyj + "补充意见2：" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\n";
                            }

                        }
                        tj_blzd = tj_blzd + "\n" + tj_ysyj;
                        pdfpath = pdfpath.Substring(pdfpath.IndexOf("/", 10)+1);
                        string cmdtxt = @"insert into T_SYN_TCT_CHECK(PACS_CheckID,CISID,PACSItemCode,PatientNameChinese,PatientSex,StudyType,StudyBodyPart,ClinicDiagnose,ClinicSymptom,ClinicAdvice,IMGStrings,StudyState,Check_Doc,Check_Date,Report_Doc,Report_Date,Audit_Doc,Audit_Date) values("
                         + "'" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "','" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "','" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_XB"].ToString().Trim() + "',"
                        + "'BL','" + jcxx.Rows[0]["F_bbmc"].ToString().Trim() + "','" + tj_blzd + "','" + tj_jcsj + "','','" +@pdfpath + "',5,'" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "','" + jcxx.Rows[0]["F_SDRQ"].ToString().Trim() + "',"
                        + "'" + jcxx.Rows[0]["F_bgys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_bgrq"].ToString().Trim() + "','" + jcxx.Rows[0]["F_shys"].ToString().Trim() + "','" + jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "')";

                        // int x = SQL_ExecuteNonQuery(odbcstr, cmdtxt, msg);
                        if (msg == "1")
                            MessageBox.Show("Connection=" + odbcstr + ";Command=" + cmdtxt);

                        string rtn_msg = "";
                        int x = Odbc_ExecuteNonQuery(odbcstr, cmdtxt, ref rtn_msg);
                        if (msg == "1")
                            MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ",影响行数：" + x.ToString() + ";" + rtn_msg);

                        if (x < 1)
                        {
                            MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ",体检接口执行数据(insert)失败！" + rtn_msg);
                            return;
                        }
                        else
                            aa.ExecuteSQL("update T_JCXX  set F_SCBJ='1',F_SFDY='是' where F_BLH='" + blh.Trim() + "'");

                    }
                    else
                    {
                        //取消审核
                        if (jcxx.Rows[0]["F_SCBJ"].ToString().Trim() == "1")
                        {
                            string cmdtxt = "delete from  T_SYN_TCT_CHECK where  PACS_CheckID='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "' and  CISID='" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "' and PACSItemCode='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "'";
                            string rtn_msg = "";
                            int x = Odbc_ExecuteNonQuery(odbcstr, cmdtxt, ref rtn_msg);
                            if (msg == "1")
                            {
                                MessageBox.Show("删除数据成功，影响行数：" + x.ToString());
                            }
                            if (x < 1)
                                MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ",体检接口执行数据(delete)失败！" + rtn_msg);
                            else
                                aa.ExecuteSQL("update T_JCXX  set F_SCBJ='0' where F_BLH='" + blh.Trim() + "'");

                        }
                    }
                }
                catch(Exception  ee3)
                {
                    MessageBox.Show("病理号：" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "，病人姓名：" + jcxx.Rows[0]["F_XM"].ToString().Trim() + ",体检接口异常！" + ee3.ToString());
                }
            }
        }

        public string qf01(string jb03xh, string qfzt, string sfzt,string blkmc)
        {
            int qfxh = 0;
            try
            {
                qfxh = Convert.ToInt32(jb03xh);
            }
            catch
            {
                MessageBox.Show("请正确选择记录,确费不成功！");
                return "0";
            }
            loaddll.initPath("hisinterface.dll");

            dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
            disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));
            commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));
            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = 0;
            try
            {
                 yy = init(dllconn33);
            }
            catch
            {
                yy = 0;
            }
            if (yy == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("JB01");
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "36" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "xmlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "30" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "zxksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "30" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Zxysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "30" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "qqxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Qqmxxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";

                inxml = inxml + "<FIELD attrname=" + (char)34 + "Itemcode" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "itemname" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Price" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "19" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Itemqty" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";

                inxml = inxml + "<FIELD attrname=" + (char)34 + "Xmstatus" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "1" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Sfflag" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "1" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Djlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "1" + (char)34 + "/>";

                inxml = inxml + "<FIELD attrname=" + (char)34 + "Bgdh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Bglx" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "Tssm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "100" + (char)34 + "/>";




                inxml = inxml + "<FIELD attrname=" + (char)34 + "Tjrybh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";

                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "blh=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["blh"].ToString() + (char)34 + " ";
                inxml = inxml + "brlb=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["brlb"].ToString() + (char)34 + " ";
                inxml = inxml + "xmlb=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["itemtype"].ToString() + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["patid"].ToString() + (char)34 + " ";
                inxml = inxml + "syxh=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["syxh"].ToString() + (char)34 + " ";

                inxml = inxml + "zxksdm=" + (char)34 + blkmc + (char)34 + " ";
                inxml = inxml + "Zxysdm=" + (char)34 + "" + (char)34 + " ";//blkgh
                inxml = inxml + "qqxh=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["qqxh"].ToString() + (char)34 + " ";
                inxml = inxml + "Qqmxxh=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["Qqmxxh"].ToString() + (char)34 + " ";


                inxml = inxml + "Itemcode=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["Itemcode"].ToString() + (char)34 + " ";
                inxml = inxml + "itemname=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["itemname"].ToString() + (char)34 + " ";
                inxml = inxml + "Price=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["Price"].ToString() + (char)34 + " ";
                inxml = inxml + "Itemqty=" + (char)34 + jb03_ds.Tables[0].Rows[qfxh]["Itemqty"].ToString() + (char)34 + " ";


                inxml = inxml + "Xmstatus=" + (char)34 + qfzt + (char)34 + " ";

                if (jb03_ds.Tables[0].Rows[qfxh]["brlb"].ToString() == "0")
                {
                    inxml = inxml + "Sfflag=" + (char)34 + "0" + (char)34 + " ";
                }
                else
                {
                    inxml = inxml + "Sfflag=" + (char)34 + sfzt + (char)34 + " ";
                }

                inxml = inxml + "Djlb=" + (char)34 + "0" + (char)34 + " ";

                inxml = inxml + "Bgdh=" + (char)34 + "" + (char)34 + " ";
                inxml = inxml + "Bglx=" + (char)34 + "" + (char)34 + " ";
                inxml = inxml + "Tssm=" + (char)34 + "" + (char)34 + " ";

                inxml = inxml + "tjrybh=" + (char)34 + "0" + (char)34 + "/>";
                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";

                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);

                sendemr("QF01", S2, S3);


                retstring = S3.ToString();


                DataSet dsqf = new DataSet();
                try
                {
                    StringReader xmlstr = null;
                    XmlTextReader xmread = null;
                    xmlstr = new StringReader(retstring);
                    xmread = new XmlTextReader(xmlstr);
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmread);

                    XmlNode node = xmldoc.SelectSingleNode("//ROWDATA");
                    string A = node.OuterXml;
                    A = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>" + A;
                    xmlstr = new StringReader(A);
                    xmread = new XmlTextReader(xmlstr);

                    dsqf.ReadXml(xmread);

                    if (dsqf.Tables[0].Rows[0][0].ToString() == "T")
                    {
                        string[] filesname = new string[] { "blh", "brlb", "xmlb", "patid", "syxh", "zxksdm", "zxysdm", "qqxh", "qqmxxh", "itemcode", "itemname", "price", "itemqty", "xmstatus", "sfflag", "djlb", "bgdh", "bglx", "tssm", "itemunit", "qqksmc", "ysmc", "itemtype", "tjrybh" };
                        string[] filevalue = new string[] 
                         { jb03_ds.Tables[0].Rows[qfxh]["blh"].ToString(),
                           jb03_ds.Tables[0].Rows[qfxh]["brlb"].ToString(), 
                           jb03_ds.Tables[0].Rows[qfxh]["itemtype"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["patid"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["syxh"].ToString(),
                             blkmc,
                             "",
                             jb03_ds.Tables[0].Rows[qfxh]["qqxh"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["qqmxxh"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["itemcode"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["itemname"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["price"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["itemqty"].ToString(),
                             "",
                             "",
                             "0",
                             "",
                             "",
                             "",
                             jb03_ds.Tables[0].Rows[qfxh]["itemunit"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["qqksmc"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["ysmc"].ToString(),
                             jb03_ds.Tables[0].Rows[qfxh]["itemtype"].ToString(),
                             "0"
                         };
                        if (qfzt == "1")
                        {
                            aa.insertsql("T_wn_sf", ref filesname, ref filevalue);
                        }
                        if (qfzt == "2")
                        {
                            aa.ExecuteSQL("delete T_wn_sf where blh='" + jb03_ds.Tables[0].Rows[qfxh]["blh"].ToString() + "' and brlb='" + jb03_ds.Tables[0].Rows[qfxh]["brlb"].ToString() + "' and qqxh='" + jb03_ds.Tables[0].Rows[qfxh]["qqxh"].ToString() + "' and qqmxxh='" + jb03_ds.Tables[0].Rows[qfxh]["qqmxxh"].ToString() + "'");
                        }
                    }
                    else
                    {
                        MessageBox.Show("病理号：" + f_blh + ",确认费用不成功！");
                    }
                }
                catch
                {
                    MessageBox.Show("病理号：" +f_blh+ ",确认费用不成功！！");
                }
            }
            else
            {
                MessageBox.Show("病理号：" + f_blh + ",连接His数据库失败！");
                //if (Debug == "1")
                //   LGZGQClass.log.WriteMyLog("连接His数据库失败！");
            }
            uninit();
            loaddll.freeLoadDll();
            return retstring;



        }


        public static string jb032(string brlb, string blh, string patid, string syxh, string qqxh, string tjrybh, string rq1, string rq2, string zxks, string Debug)
        {
            loaddll.initPath("hisinterface.dll");

            dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
            disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));
            commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));
            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = 0;
            try
            {
                yy = init(dllconn33);
            }
            catch
            {
            }

            if (yy == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("JB01");
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "36" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "30" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "qqxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "tjrybh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "rq1" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "rq2" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "zxks" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sqdxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "fph" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "blh=" + (char)34 + blh + (char)34 + " ";
                inxml = inxml + "brlb=" + (char)34 + brlb + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + patid + (char)34 + " ";
                inxml = inxml + "syxh=" + (char)34 + syxh + (char)34 + " ";
                inxml = inxml + "qqxh=" + (char)34 + qqxh + (char)34 + " ";
                inxml = inxml + "tjrybh=" + (char)34 + "0" + (char)34 + " ";
                inxml = inxml + "rq1=" + (char)34 + rq1 + (char)34 + " ";
                inxml = inxml + "rq2=" + (char)34 + rq2 + (char)34 + " ";


                inxml = inxml + "zxks=" + (char)34 + zxks + (char)34 + " ";
                inxml = inxml + "sqdxh=" + (char)34 + "0" + (char)34 + " ";
                inxml = inxml + "fph=" + (char)34 + "0" + (char)34 + "/>";
                //inxml = inxml + "sqdxh=" + (char)34 + zxks + (char)34 + " ";
                //inxml = inxml + "zxks=" + (char)34 + "0" + (char)34 + "/>";

                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";

                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);

                sendemr("JB03", S2, S3);
                retstring = S3.ToString();
            }
            else
            {
                MessageBox.Show("病理号：" + f_blh + ",连接His数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("病理号：" + f_blh + ",连接His数据库失败！");
            }

            uninit();
            IniFiles xf = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
            if (xf.ReadInteger("savetohis", "unload", 1) != 0)
            { }
            else
            {

                loaddll.freeLoadDll();
            }

            return retstring;



        }

        static string msg = "";
        public int SQL_ExecuteNonQuery(string connectionString, string cmdText, string msg)
        {
            SqlConnection con = con = new SqlConnection(connectionString);
            SqlCommand sqlcom = new SqlCommand(cmdText, con);
            int x = -1;

            if (msg == "1")
                MessageBox.Show(cmdText);
            try
            {
                con.Open();
                x = sqlcom.ExecuteNonQuery();
                sqlcom.Dispose();
                con.Close();
                return x;
            }
            catch (Exception ee)
            {
                con.Close(); sqlcom.Dispose();
                log.WriteMyLog("回传异常：" + ee.ToString());
                MessageBox.Show("数据库执行异常：" + ee);
                return -1;
            }
        }
        public int Odbc_ExecuteNonQuery(string connectionString, string cmdText, ref string msg)
        {
            OdbcConnection con = con = new OdbcConnection(connectionString);
            OdbcCommand sqlcom = new OdbcCommand(cmdText, con);
            int x = -1;

            try
            {
                con.Open();
                x = sqlcom.ExecuteNonQuery();
                sqlcom.Dispose();
                con.Close();
                msg = "";
                return x;
            }
            catch (Exception ee)
            {
                con.Close(); sqlcom.Dispose();
                log.WriteMyLog("回传异常：" + ee.ToString());
                msg = ee.ToString();
               // MessageBox.Show("数据库执行异常：" + ee);
                return -1;
            }
        }



        public void hcblh(string F_blh, string yymc)
        {
          
        }
        private static string brxm = "";
        private static DLLWrapper func = new DLLWrapper();

        public static LoadDllapi loaddll = new LoadDllapi();

        public delegate int dllconn(StringBuilder inifile);
        public delegate int commit(string s1, StringBuilder s2, StringBuilder s3);
        public delegate void disdllconn();

        public static string bg012(string brlb, string brbh, string blh, string Debug, string hisblh, string xb, string bgrq, string yzid, string sdrq, string shys)
        {

            loaddll.initPath("hisinterface.dll");

            dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
            disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));
            commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));
            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = 0;
            //MessageBox.Show("2");
            try
            {
                yy = init(dllconn33);
            }
            catch (Exception ex)
            {

            }
            if (yy == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("BG01");

                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "repno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "reqno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "24" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "cardno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "hzxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sex" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "4" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "age" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "3" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sjksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sjksmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "bqdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "bqmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "cwdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sjysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sjysxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sjrq" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "replb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "4" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "replbmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "reprq" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "xtbz" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "jcbw" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "200" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "jcysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "jcysxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "jcksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "jcksmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "pubtime" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "repno=" + (char)34 + blh + (char)34 + " ";
                inxml = inxml + "reqno=" + (char)34 + "0" + (char)34 + " ";


                if (brlb == "1")
                {
                    inxml = inxml + "syxh=" + (char)34 + brbh + (char)34 + " ";
                    inxml = inxml + "patid=" + (char)34 + yzid + (char)34 + " ";
                    inxml = inxml + "cardno=" + (char)34 + (char)34 + " ";
                }
                else
                {
                    inxml = inxml + "syxh=" + (char)34 + "0" + (char)34 + " ";
                    inxml = inxml + "patid=" + (char)34 + brbh + (char)34 + " ";
                    inxml = inxml + "cardno=" + (char)34 + yzid + (char)34 + " ";
                }
                inxml = inxml + "blh=" + (char)34 + hisblh + (char)34 + " ";
                inxml = inxml + "hzxm=" + (char)34 + brxm + (char)34 + " ";
                inxml = inxml + "sex=" + (char)34 + xb + (char)34 + " ";
                inxml = inxml + "age=" + (char)34 + (char)34 + " ";
                inxml = inxml + "sjksdm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "sjksmc=" + (char)34 + (char)34 + " ";
                inxml = inxml + "bqdm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "bqmc=" + (char)34 + (char)34 + " ";
                inxml = inxml + "cwdm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "sjysdm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "sjysxm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "sjrq=" + (char)34 + sdrq.Replace("-", "") + (char)34 + " ";
                inxml = inxml + "replb=" + (char)34 + "BL" + (char)34 + " ";
                inxml = inxml + "replbmc=" + (char)34 + "病理" + (char)34 + " ";
                inxml = inxml + "reprq=" + (char)34 + bgrq.Replace("-", "") + (char)34 + " ";
                inxml = inxml + "xtbz=" + (char)34 + brlb + (char)34 + " ";
                inxml = inxml + "jcbw=" + (char)34 + (char)34 + " ";
                inxml = inxml + "jcysdm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "jcysxm=" + (char)34 + shys + (char)34 + " ";
                inxml = inxml + "jcksdm=" + (char)34 + (char)34 + " ";
                inxml = inxml + "jcksmc=" + (char)34 + (char)34 + " ";
               // inxml = inxml + "pubtime=" + (char)34 + (char)34 + " ";


                inxml = inxml + "pubtime=" + (char)34 + (char)34 + "/>";
                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";

                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);
                sendemr("BG01", S2, S3);
                retstring = S3.ToString();
                log.WriteMyLog(retstring);
                if (msg == "1")
                    MessageBox.Show("病理号：" + f_blh +retstring);

            }
            else
            {
                MessageBox.Show("病理号：" + f_blh + ",连接His数据库失败！");

                log.WriteMyLog("连接His数据库失败！");
            }
            uninit();
            loaddll.freeLoadDll();

            return "0";

        }

        public static string bg01(string brlb, string brbh, string blh, string Debug, string hisblh, string xb, string bgrq, string yzid, string sdrq, string shys)
        {
         
            func.LoadDll("hisinterface.dll");
            func.LoadFun("Init");
            //func.LoadFun("Initconn");

            StringBuilder dllconn = new StringBuilder("");
            StringBuilder S1 = new StringBuilder("JB01");
            string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
            inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
            inxml = inxml + "<METADATA>";
            inxml = inxml + "<FIELDS>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "repno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "reqno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "24" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "cardno" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "hzxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sex" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "4" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "age" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "3" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjksmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "bqdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "bqmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "cwdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjysxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "12" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "sjrq" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "replb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "4" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "replbmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "32" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "reprq" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "xtbz" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcbw" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "200" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcysdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcysxm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcksdm" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "jcksmc" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "pubtime" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
            inxml = inxml + "</FIELDS>";
            inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
            inxml = inxml + "</METADATA>";
            inxml = inxml + "<ROWDATA>";
            inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
            inxml = inxml + "repno=" + (char)34 + blh + (char)34 + " ";
            inxml = inxml + "reqno=" + (char)34 + "0" + (char)34 + " ";


            if (brlb == "1")
            {
                inxml = inxml + "syxh=" + (char)34 + brbh + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + yzid + (char)34 + " ";
                inxml = inxml + "cardno=" + (char)34 + (char)34 + " ";
            }
            else
            {
                inxml = inxml + "syxh=" + (char)34 + brbh + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + brbh + (char)34 + " ";
                inxml = inxml + "cardno=" + (char)34 + yzid + (char)34 + " ";
            }
            inxml = inxml + "blh=" + (char)34 + hisblh + (char)34 + " ";
            inxml = inxml + "hzxm=" + (char)34 + brxm + (char)34 + " ";
            inxml = inxml + "sex=" + (char)34 + xb + (char)34 + " ";
            inxml = inxml + "age=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjksdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjksmc=" + (char)34 + (char)34 + " ";
            inxml = inxml + "bqdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "bqmc=" + (char)34 + (char)34 + " ";
            inxml = inxml + "cwdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjysdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjysxm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "sjrq=" + (char)34 + sdrq.Replace("-", "") + (char)34 + " ";
            inxml = inxml + "replb=" + (char)34 + "BL" + (char)34 + " ";
            inxml = inxml + "replbmc=" + (char)34 + "病理" + (char)34 + " ";
            inxml = inxml + "reprq=" + (char)34 + bgrq.Replace("-", "") + (char)34 + " ";
            inxml = inxml + "xtbz=" + (char)34 + brlb + (char)34 + " ";
            inxml = inxml + "jcbw=" + (char)34 + (char)34 + " ";
            inxml = inxml + "jcysdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "jcysxm=" + (char)34 + shys + (char)34 + " ";
            inxml = inxml + "jcksdm=" + (char)34 + (char)34 + " ";
            inxml = inxml + "jcksmc=" + (char)34 + (char)34 + " ";
            inxml = inxml + "pubtime=" + (char)34 + (char)34 + " ";


            inxml = inxml + "pubtime=" + (char)34 + (char)34 + "/>";
            inxml = inxml + "</ROWDATA>";
            inxml = inxml + "</DATAPACKET>";

            StringBuilder S2 = new StringBuilder(inxml);
            StringBuilder S3 = new StringBuilder(65536);

            if (msg == "1")
                log.WriteMyLog("回传信息:" + inxml);


            object[] Parameters = new object[] { dllconn }; // 实参为 0 

            Type[] ParameterTypes = new Type[] { typeof(StringBuilder) }; // 实参类型为 int 

            DLLWrapper.ModePass[] themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue }; // 传送方式为值传 

            Type Type_Return = typeof(int); // 返回类型为 int 
            int xx = 0;
            try
            {
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
            }
            catch
            {
                MessageBox.Show("病理号：" + f_blh +",连接HIS数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("病理号：" + f_blh + ",连接HIS数据库失败！");

                // func.UnLoadDll();
                return "0";

            }
            if (xx == 0)
            {
                func.LoadFun("SendEmr");

                Parameters = new object[] { "BG01", S2, S3 }; // 实参为 3 
                ParameterTypes = new Type[] { typeof(String), typeof(StringBuilder), typeof(StringBuilder) }; // 实参类型为 pchar 
                themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue }; // 传送方式为值传 
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);

            }
            else
            {
               MessageBox.Show("病理号：" + f_blh +",连接HIS数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("病理号：" + f_blh + ",连接HIS数据库失败！");

                //func.UnLoadDll();
                return "0";
            }
            if (xx == 0)
            {
                //func.LoadDll("UnInit");
                //Parameters = new object[] { }; // 实参为 0
                //ParameterTypes = new Type[] { }; // 实参类型为 pchar 
                //themode = new DLLWrapper.ModePass[] { }; // 传送方式为值传 
                //func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
                func.UnLoadDll();
                return S3.ToString();
            }
            else
            {
                MessageBox.Show("病理号：" + f_blh + ",病理号回写HIS失败！");
                if (Debug == "1")
                    log.WriteMyLog(S3.ToString());
                func.UnLoadDll();
                return "0";

            }
            // 弹出提示框，显示调用 myfun.Invoke 方法的结果，即调用 count 函数                        

        }



        public void C_PDF(string ML, string blh, string bgxh, string bglx, ref  string errmsg, ref string pdfpath, ref string filename, bool ScPDF, bool UpPDF, bool isbase64, ref string Base64String, string debug)
        {
            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
            {
                #region  生成pdf

                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref errmsg, ref filename);
                string xy = "3";
                if (isrtn)
                {
                    //二进制串
                    if (!File.Exists(filename))
                    {
                        ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：未找到文件" + filename, "ZGQJK", "生成PDF");
                        log.WriteMyLog("未找到文件" + filename);
                        errmsg = "未找到文件" + filename;
                        zgq.DelTempFile(blh);
                        isbase64 = false;
                        return;
                    }
                    ScPDF = true;
                    if (isbase64)
                    {
                        try
                        {
                            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                            file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                            file.Close();
                            Base64String = Convert.ToBase64String(imgByte);
                            isbase64 = true;
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("PDF转换二进制串失败");
                            errmsg = "PDF转换二进制串失败";
                            isbase64 = false;
                        }
                    }


                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF成功", "ZGQJK", "生成PDF");
                    bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy), ref pdfpath);
                    if (ssa == true)
                    {
                        if (debug == "1")
                            log.WriteMyLog("上传PDF成功");
                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + ML + "\\" + filename, "ZGQJK", "上传PDF");

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + filename + "')");


                        UpPDF = true;
                    }
                    else
                    {
                        log.WriteMyLog("上传PDF失败：" + errmsg);
                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
                        UpPDF = false;
                    }
                    zgq.DelTempFile(blh);
                }
                else
                {
                    ScPDF = false;
                    log.WriteMyLog("生成PDF失败：" + errmsg);
                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：" + errmsg, "ZGQJK", "生成PDF");
                    zgq.DelTempFile(blh);

                    return;
                }
                #endregion
            }
            else
            {
                log.WriteMyLog("不需生成PDF");
                return;
            }
        }

    }
}
