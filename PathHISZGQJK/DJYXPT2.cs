using System;
using System.Collections.Generic;
using System.Text;
using readini;
using dbbase;
using System.Data;
using System.Windows.Forms;
using HL7;
using System.Data.OracleClient;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PathHISZGQJK;

namespace PathHISJK
{
    public class DJYXPT
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        private dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                {
                    //取消审核动作
                    qxsh = "1";
                }

                if (cslb[3].ToLower() == "new")
                {
                    xdj = "1";
                }

            }
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
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

            string bgzt = "";

            if (qxsh == "1")
                bgzt = "取消审核";
            else
                bgzt = jcxx.Rows[0]["F_BGZT"].ToString();


             if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
             {


            if (bgzt == "已发布" || bgzt == "取消审核")
            {


                string bgzt2 = "";
                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                try
                {
                    if (bglx.ToLower().Trim() == "bd")
                    {
                        dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    }

                    if (bglx.ToLower().Trim() == "bc")
                    {
                        dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                        bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                    }
                    if (bglx.ToLower().Trim() == "cg")
                    {
                        bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                    }
                }
                catch
                {
                }

                if (bgzt2.Trim() == "")
                    log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);

                if (bgzt2.Trim() == "已发布" && bgzt != "取消审核")
                {
                    //////////////////////生成pdf**********************************************************
                    string jpgname = "";
                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                   
                        #region  生成pdf

                        string message = "";
                        ZGQClass zgq = new ZGQClass();
                        if (debug == "1")
                            log.WriteMyLog("开始生成PDF。。。");
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZGQClass.type.PDF, ref message, ref jpgname);

                        string xy = "3";
                        if (isrtn)
                        {
                            if (File.Exists(jpgname))
                            {
                                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                if (ssa == true)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("上传PDF成功");

                                    jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                    // ZGQClass.BGHJ(blh, "JK", "审核", "上传PDF成功:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "上传PDF");

                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                   // aa.ExecuteSQL("update T_JCXX_FS set F_bz='',F_ISPDF='true',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                                }
                                else
                                {
                                    log.WriteMyLog("上传PDF失败：" + message);
                                    //ZGQClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "上传PDF");
                                   // aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("生成PDF失败:未找到文件---" + jpgname);
                                //  ZGQClass.BGHJ(blh, "JK", "审核", "上传PDF失败:未找到文件---" + jpgname, "ZGQJK", "生成PDF");
                             //   aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败:未找到文件---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            }
                        }
                        else
                        {

                            log.WriteMyLog("生成PDF失败：" + message);
                          //  aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='生成PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

                        }
                        zgq.DeleteTempFile(blh);
                        #endregion
                }
                else
                {
                    if (bgzt == "取消审核")
                    {
                         aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                    }
                   }



            }
            }
              if (f.ReadString("savetohis", "djyxpt", "1").Replace("\0", "").Trim() == "1")
              {
                  djyxpt_hx(blh, bglx, bgxh, "", cslb[4].ToLower(), "");
            }
        }

        public void djyxpt_hx(string F_blh, string bglx, string bgxh, string bgxj, string sfsave, string yymc)
        {
            if (bglx != "cg")//只传常规报告
            {
                return;
            }
            string debug = f.ReadString("savetohis", "debug", "0");
            string hospitalid = f.ReadString("savetohis", "hospitalid", "");
            string pisid = f.ReadString("savetohis", "pisid", "");
            string CUSTOM4 = f.ReadString("savetohis", "CUSTOM4", "2.16.840.1.113883.4.487.5.6.4");
            string NATIONALITY_DOMAIN = f.ReadString("savetohis", "NATIONALITY_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.16");
            string MARITAL_DOMAIN = f.ReadString("savetohis", "MARITAL_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.6");
            string DEGREE_DOMAIN = f.ReadString("savetohis", "DEGREE_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.7");
            string IDENTIFIER_FLOW_DOMAIN_ID = f.ReadString("savetohis", "IDENTIFIER_FLOW_DOMAIN_ID", "2.16.840.1.113883.4.487.5.6.10.1");
            string GENDER_DOMAIN = f.ReadString("savetohis", "GENDER_DOMAIN", "2.16.840.1.113883.4.487.5.6.1.1.5");
            string PAT_CATEGORY_SYSTEM = f.ReadString("savetohis", "PAT_CATEGORY_SYSTEM", "2.16.840.1.113883.4.487.5.6.1.1.9");
            string ETHNIC_DOMAIN = f.ReadString("savetohis", "ETHNIC_DOMAIN", "2.16.840.1.113883.4.487.2.1.1.1.21");

            DataTable bljc = new DataTable();
            
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "bljc");
            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }

            //if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "住院" && bljc.Rows[0]["F_brlb"].ToString().Trim() != "门诊")
            //{
            //    log.WriteMyLog("病人类别不为住院或门诊，不处理！");
            //    return;
            //}
            string brbh = "";
            string brlb = "";
            string brbhlb = pisid;
            brbh = bljc.Rows[0]["F_brbh"].ToString().Trim();
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
            {
                //brbh = bljc.Rows[0]["F_zyh"].ToString().Trim();
                brlb = "1";
                //brbhlb = "2.16.840.1.113883.4.487.3.4.4.3";
            }
            else
            {
                //brbh = bljc.Rows[0]["F_mzh"].ToString().Trim();
                brlb = "0";
                //brbhlb = "2.16.840.1.113883.4.487.3.4.4.1";
            }
            if (brbh == null || brbh == "")
            {
                brbh = "";
                //log.WriteMyLog("病人编号为空，不处理！");
                //return;
            }
            if (sfsave == "qxsh")
            {
                string qxshurl = "http://192.168.1.25:8087/CancelArchive/cancel.do?document_ID=" + F_blh + "&document_DomainID=2.16.840.1.113883.4.487.5.6.10";
                log.WriteMyLog(qxshurl);
                string resjon = GetHttpGet(qxshurl);
                log.WriteMyLog(resjon);
                if (resjon == "0")
                {
                    log.WriteMyLog(F_blh + "取消发布成功");
                }
                else
                {
                    try
                    {
                        JObject ja = (JObject)JsonConvert.DeserializeObject(resjon);
                        string restatus = ja["status"].ToString();
                        if (restatus == "0")
                        {
                            log.WriteMyLog(F_blh + "取消发布成功");
                        }
                        else
                        {
                            log.WriteMyLog(F_blh + "取消发布失败，原因：" + ja["errorInfo"].ToString());
                        }
                    }
                    catch(Exception ex)
                    {
                        log.WriteMyLog(F_blh + "取消发布失败，原因:" + ex.Message);
                    }
                }
            }
            else
            {
                if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已发布")
                {
                    string uuid = Guid.NewGuid().ToString();

                    string nl = bljc.Rows[0]["F_nl"].ToString().Trim();
                    string nldw = nl.Substring(nl.Length - 1, 1);
                    DateTime d1 = DateTime.Now;
                    try
                    {
                        d1 = Convert.ToDateTime(bljc.Rows[0]["F_sdrq"].ToString().Trim());
                    }
                    catch
                    {
                    }

                    //d1 = d1.AddYears(0 - Convert.ToInt16(bljc.Rows[0]["F_age"].ToString().Trim()));
                    //nl = d1.ToString("yyyy") + "-01-01";

                    if (nldw == "月")
                    {
                        try
                        {
                            d1 = d1.AddMonths(0 - Convert.ToInt16(bljc.Rows[0]["F_age"].ToString().Trim()));
                        }
                        catch
                        {
                            log.WriteMyLog("计算年龄失败，位置1，age为：" + bljc.Rows[0]["F_age"].ToString().Trim());
                            return;
                        }
                        nl = d1.ToString("yyyy-MM-") + "01";

                    }

                    else if (nldw == "天")
                    {
                        try
                        {
                            d1 = d1.AddDays(0 - Convert.ToInt16(bljc.Rows[0]["F_age"].ToString().Trim()));
                        }
                        catch
                        {
                            log.WriteMyLog("计算年龄失败，位置2，age为：" + bljc.Rows[0]["F_age"].ToString().Trim());
                            return;
                        }
                        nl = d1.ToString("yyyy-MM-dd");
                    }
                    else if (nldw == "岁")
                    {
                        if (bljc.Rows[0]["F_SFZH"] != null && bljc.Rows[0]["F_SFZH"].ToString().Trim() != "")
                        {
                            string csrq = bljc.Rows[0]["F_SFZH"].ToString().Trim();
                            if (csrq.Length == 15)
                            {
                                csrq = "19" + csrq.Substring(6, 6);
                            }
                            else
                            {
                                csrq = csrq.Substring(6, 8);
                            }
                            nl = csrq.Substring(0, 4) + "-" + csrq.Substring(4, 2) + "-" + csrq.Substring(6, 2);
                        }
                        else
                        {
                            try
                            {
                                d1 = d1.AddYears(0 - Convert.ToInt16(bljc.Rows[0]["F_age"].ToString().Trim()));
                            }
                            catch
                            {
                                log.WriteMyLog("计算年龄失败，位置3，age为：" + bljc.Rows[0]["F_age"].ToString().Trim());
                                return;
                            }
                            nl = d1.ToString("yyyy") + "-01-01";
                        }

                    }
                    else
                    {
                        if (bljc.Rows[0]["F_SFZH"] != null && bljc.Rows[0]["F_SFZH"].ToString().Trim() != "")
                        {
                            string csrq = bljc.Rows[0]["F_SFZH"].ToString().Trim();
                            if (csrq.Length == 15)
                            {
                                csrq = "19" + csrq.Substring(6, 6);
                            }
                            else
                            {
                                csrq = csrq.Substring(6, 8);
                            }
                            nl = csrq.Substring(0, 4) + "-" + csrq.Substring(4, 2) + "-" + csrq.Substring(6, 2);
                        }
                        else
                        {
                            try
                            {
                                d1 = d1.AddYears(0 - Convert.ToInt16(bljc.Rows[0]["F_age"].ToString().Trim()));
                            }
                            catch
                            {
                                log.WriteMyLog("计算年龄失败，位置4，age为：" + bljc.Rows[0]["F_age"].ToString().Trim());
                                return;
                            }
                            nl = d1.ToString("yyyy") + "-01-01";
                        }
                    }


                    string xb = "U";
                    if (bljc.Rows[0]["F_xb"].ToString().Trim() == "男")
                    {
                        xb = "M";
                    }
                    if (bljc.Rows[0]["F_xb"].ToString().Trim() == "女")
                    {
                        xb = "F";
                    }

                    string hy = "";
                    if (bljc.Rows[0]["F_HY"].ToString().Trim() == "已婚")
                    {
                        hy = "M";
                    }
                    else
                    {
                        hy = "S";
                    }
                    string hylb = "2.16.840.1.113883.4.487.3.4.1.1.6";

                    string mz = "";
                    switch (bljc.Rows[0]["F_MZ"].ToString().Trim())
                    {
                        case "汉族":
                            mz = "1";
                            break;
                        case "回族":
                            mz = "2";
                            break;
                        case "朝鲜族":
                            mz = "3";
                            break;
                        case "满族":
                            mz = "4";
                            break;
                        case "高山族":
                            mz = "5";
                            break;
                        default:
                            mz = "6";
                            break;
                    }

                    string bgys = bljc.Rows[0]["F_BGYS"].ToString().Trim();
                    string shys = bljc.Rows[0]["F_SHYS"].ToString().Trim();
                    string bgysgh = aa.GetDataTable("select * from T_YH where F_YHMC='" + bgys + "'", "bgysgh").Rows[0]["F_YHBH"].ToString().Trim();
                    string shysgh = aa.GetDataTable("select * from T_YH where F_YHMC='" + shys + "'", "shysgh").Rows[0]["F_YHBH"].ToString().Trim();

                    string hisname = f.ReadString("savetohis", "hisname", "HIUP").Replace("\0", "");
                    string user = f.ReadString("savetohis", "USER", "ATS_GATE").Replace("\0", "");
                    string passwd = f.ReadString("savetohis", "PASSWD", "ATS_GATE").Replace("\0", "");
                    string ConnectionString = "Data Source=" + hisname + ";user=" + user + ";password=" + passwd + ";";//写连接串
                    OracleConnection conn = new OracleConnection(ConnectionString);
                    OracleCommand OCMD = new OracleCommand();
                    OCMD.Connection = conn;
                    /*
                    string sqlstring = "insert into PERSON (UUID,RELEVANCE_DOMAIN,custom4,custom14,person_status,PERSON_ID,NAME,DATE_OF_BIRTH,GENDER_CD,GENDER_NAME,GENDER_DOMAIN,ETHNIC_DOMAIN,NATIONALITY_DOMAIN,MARITAL_STATUS_CD,MARITAL_STATUS_NAME,MARITAL_DOMAIN,DEGREE_DOMAIN,PROFESSION_DOMAIN,HOME_ADDRESS,VIP,HOSPITAL_DOMAIN_ID,IDENTIFIER_DOMAIN_NAME,IDENTIFIER_DOMAIN_ID,IDENTIFIER_DOMAIN_TYPE,DATE_CREATED,IDENTIFIER_ID) values(";
                    sqlstring = sqlstring + "'" + uuid + "',";
                    sqlstring = sqlstring + "'" + brbhlb + "',";
                    sqlstring = sqlstring + "'" + brbhlb + "',";
                    sqlstring = sqlstring + "'" + brbh + "',";

                    sqlstring = sqlstring + "'01',";
                    sqlstring = sqlstring + "PERSON_SEQUENCE.nextval,";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_xm"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "" + "to_date('" + nl + "', 'yyyy-MM-dd')" + ",";
                    sqlstring = sqlstring + "'" + xb + "',";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_xb"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.3.4.1.1.5',";
                    sqlstring = sqlstring + "'',";
                    sqlstring = sqlstring + "'',";
                    sqlstring = sqlstring + "'" + hy + "',";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_hy"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "'" + hylb + "',";
                    sqlstring = sqlstring + "'',";
                    sqlstring = sqlstring + "'',";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "0,";
                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.3.4',";
                    sqlstring = sqlstring + "'PIS',";
                    sqlstring = sqlstring + "'2.16.840.1.113883.4.487.3.4.8',";
                    sqlstring = sqlstring + "'ISO',";
                    sqlstring = sqlstring + "" + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')" + ",";
                    sqlstring = sqlstring + "'" + brbh + "')";
                     * */

                    string psinsert = "01";

                    try
                    {

                        OCMD.CommandText = "select * from PERSON where IDENTIFIER_DOMAIN_ID='" + pisid + "' and IDENTIFIER_ID='" + F_blh + "'";
                        conn.Open();
                        OracleDataAdapter oda = new OracleDataAdapter(OCMD);

                        DataTable dt1 = new DataTable();

                        oda.Fill(dt1);

                        if (dt1.Rows.Count > 0)
                        {
                            psinsert = "02";
                        }

                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    string sqlstring = "";
                    #region personvisit

                    int pvisitid = 0;
                    try
                    {

                        OCMD.CommandText = "select PATIENT_VISIT_SEQUENCE.nextval from dual";
                        conn.Open();
                        OracleDataAdapter oda = new OracleDataAdapter(OCMD);

                        DataTable dt1 = new DataTable();

                        oda.Fill(dt1);
                        try
                        {
                            pvisitid = Convert.ToInt32(dt1.Rows[0][0].ToString());
                        }
                        catch
                        {
                            log.WriteMyLog("转换PATIENT_VISIT_SEQUENCE失败，PATIENT_VISIT_SEQUENCE为：" + dt1.Rows[0][0].ToString());
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    //if (psinsert == "01")
                    //{
                    sqlstring = string.Format("insert into PATIENT_VISIT (PATIENT_VISIT_ID,PATIENT_ID,VISIT_FLOW_ID,NAME,DATE_OF_BIRTH,GENDER_CD,GENDER_NAME,GENDER_DOMAIN,MARITAL_STATUS,HOSPITAL_DOMAIN_ID,IDENTIFIER_DOMAIN_NAME,IDENTIFIER_DOMAIN_ID,IDENTIFIER_DOMAIN_TYPE,IDENTIFIER_FLOW_DOMAIN_NAME,IDENTIFIER_FLOW_DOMAIN_ID,IDENTIFIER_FLOW_DOMAIN_TYPE,PAT_CATEGORY,PAT_CATEGORY_SYSTEM,PATIENT_FLOW_ID,CREATE_DATE,UUID) values ({0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}',{19},'{20}')", pvisitid, F_blh, F_blh, bljc.Rows[0]["F_xm"].ToString().Trim(), "to_date('" + nl + "', 'yyyy-MM-dd')", xb, bljc.Rows[0]["F_xb"].ToString().Trim(), GENDER_DOMAIN, hy, hospitalid, "PIS", pisid, "ISO", "PIS-BLKLS", IDENTIFIER_FLOW_DOMAIN_ID, "ISO", brlb, PAT_CATEGORY_SYSTEM, F_blh, "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')", uuid);
                    //}
                    //else
                    //{
                    //    sqlstring = string.Format("UPDATE PATIENT_VISIT SET NAME='{0}',DATE_OF_BIRTH={1},GENDER_CD='{2}',GENDER_NAME='{3}' where IDENTIFIER_DOMAIN_ID='"+pisid+"' and PATIENT_ID='{4}'", bljc.Rows[0]["F_xm"].ToString().Trim(), "to_date('" + nl + "', 'yyyy-MM-dd')", xb, bljc.Rows[0]["F_xb"].ToString().Trim(), brbh);
                    //}
                    if (debug == "1")
                    {
                        log.WriteMyLog("插入PATIENT_VISIT表语句：" + sqlstring);
                    }
                    OCMD.CommandText = sqlstring;
                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    #endregion

                    int personid = 0;
                    try
                    {

                        OCMD.CommandText = "select PERSON_SEQUENCE.nextval from dual";
                        conn.Open();
                        OracleDataAdapter oda = new OracleDataAdapter(OCMD);

                        DataTable dt1 = new DataTable();

                        oda.Fill(dt1);
                        try
                        {
                            personid = Convert.ToInt32(dt1.Rows[0][0].ToString());
                        }
                        catch
                        {
                            log.WriteMyLog("转换PERSON_SEQUENCE失败，PERSON_SEQUENCE为：" + dt1.Rows[0][0].ToString());
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    //if (psinsert == "01")
                    //{
                    sqlstring = "insert into PERSON (PERSON_ID,NAME,DATE_OF_BIRTH,IDENTITY_NO,GENDER_CD,GENDER_DOMAIN,ETHNIC_GROUP_CD,ETHNIC_NAME,ETHNIC_DOMAIN,NATIONALITY_CD,NATIONALITY_NAME,NATIONALITY_DOMAIN,MARITAL_STATUS_CD,MARITAL_STATUS_NAME,MARITAL_DOMAIN,DEGREE,DEGREE_DOMAIN,DATE_CREATED,HOSPITAL_DOMAIN_ID,IDENTIFIER_DOMAIN_NAME,IDENTIFIER_DOMAIN_ID,IDENTIFIER_DOMAIN_TYPE,IDENTIFIER_ID,PERSON_STATUS,UUID,VIP,CUSTOM4,CUSTOM14,custom3,custom13) values (";
                    sqlstring = sqlstring + personid + ",";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_XM"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "" + "to_date('" + nl + "', 'yyyy-MM-dd')" + ",";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_SFZH"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "'" + xb + "',";
                    //sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.1.1.1.5',";
                    sqlstring = sqlstring + "'" + GENDER_DOMAIN + "',";
                    sqlstring = sqlstring + "'" + mz + "',";
                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_MZ"].ToString().Trim() + "',";
                    sqlstring = sqlstring + "'" + ETHNIC_DOMAIN + "',";
                    sqlstring = sqlstring + "'1',";
                    sqlstring = sqlstring + "'中国',";
                    sqlstring = sqlstring + "'" + NATIONALITY_DOMAIN + "',";
                    sqlstring = sqlstring + "'" + hy + "',";
                    string hyname = bljc.Rows[0]["F_hy"].ToString().Trim() == "已婚" ? "已婚" : "未婚";
                    sqlstring = sqlstring + "'" + hyname + "',";
                    sqlstring = sqlstring + "'" + MARITAL_DOMAIN + "',";
                    sqlstring = sqlstring + "'0',";
                    sqlstring = sqlstring + "'" + DEGREE_DOMAIN + "',";
                    sqlstring = sqlstring + "" + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')" + ",";
                    sqlstring = sqlstring + "'" + hospitalid + "',";
                    //2.16.840.1.113883.4.487.5.6
                    sqlstring = sqlstring + "'PIS',";
                    sqlstring = sqlstring + "'" + pisid + "',";
                    //2.16.840.1.113883.4.487.5.6.10
                    sqlstring = sqlstring + "'ISO',";
                    sqlstring = sqlstring + "'" + F_blh + "',";
                    sqlstring = sqlstring + "'" + psinsert + "',";
                    sqlstring = sqlstring + "'" + uuid + "',";

                    sqlstring = sqlstring + "'1',";
                    if (brlb == "1")
                    {
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.10',";
                        sqlstring = sqlstring + "'" + brbh + "',";
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.3',";
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "'";
                    }
                    else
                    {
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.9',";
                        sqlstring = sqlstring + "'" + brbh + "',";
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.1',";
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_mzh"].ToString().Trim() + "'";
                    }
                    sqlstring = sqlstring + ")";
                    //}
                    //else
                    //{
                    //    sqlstring = string.Format("UPDATE PERSON set NAME='{0}',DATE_OF_BIRTH={1},IDENTITY_NO='{2}',GENDER_CD='{3}',DATE_CREATED={4},PERSON_STATUS='{5}' where IDENTIFIER_DOMAIN_ID='"+pisid+"' and IDENTIFIER_ID='{6}'", bljc.Rows[0]["F_XM"].ToString().Trim(), "to_date('" + nl + "', 'yyyy-MM-dd')", bljc.Rows[0]["F_SFZH"].ToString().Trim(), xb, "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')", psinsert, brbh);
                    //}

                    if (debug == "1")
                    {
                        log.WriteMyLog("插入PERSON表语句：" + sqlstring);
                    }

                    OCMD.CommandText = sqlstring;
                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }



                    string bginsert = "ADD";
                    string xmlpkid = "";
                    string pdfpkid = "";
                    try
                    {

                        OCMD.CommandText = "select * from DGATE_DOCUMENT_INFO where DOCUMENT_DOMAIN_ID='" + pisid + "' and DOCUMENT_UNIQUE_ID='" + F_blh + "'";
                        conn.Open();
                        OracleDataAdapter oda = new OracleDataAdapter(OCMD);

                        DataTable dt1 = new DataTable();

                        if (debug == "1")
                        {
                            MessageBox.Show("DGATE_DOCUMENT_INFO中查询出了" + dt1.Rows.Count + "条数据");
                        }

                        oda.Fill(dt1);

                        if (dt1.Rows.Count > 0)
                        {
                            bginsert = "UPDATE";
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if (debug == "1")
                                {
                                    MessageBox.Show("FILE_TYPE=" + dt1.Rows[i]["FILE_TYPE"].ToString() + ",PK为" + dt1.Rows[i]["PK"].ToString());
                                }
                                if (dt1.Rows[i]["FILE_TYPE"].ToString() == "PDF")
                                {
                                    pdfpkid = dt1.Rows[i]["PK"].ToString();
                                }
                                else
                                {
                                    xmlpkid = dt1.Rows[i]["PK"].ToString();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    #region 写入DGATE_DOCUMENT_INFO表
                    int bgid = 0;


                    #region 生成XML

                    string xml = "<?xml version='1.0' standalone='yes'?><LOGENE>";
                    xml = xml + "<F_BLK>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLK"].ToString().Trim()) + "</F_BLK>";
                    xml = xml + "<F_BLH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLH"].ToString().Trim()) + "</F_BLH>";
                    if (bljc.Rows[0]["F_SJKS"].ToString().Contains("消化"))
                    {
                        xml = xml + "<F_EISBH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_MZ"].ToString().Trim()) + "</F_EISBH>";
                    }
                    else
                    {
                        xml = xml + "<F_EISBH></F_EISBH>";
                    }
                    xml = xml + "<F_BRBH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLH"].ToString().Trim()) + "</F_BRBH>";
                    xml = xml + "<F_SQXH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SQXH"].ToString().Trim()) + "</F_SQXH>";
                    xml = xml + "<F_YZID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YZID"].ToString().Trim()) + "</F_YZID>";
                    xml = xml + "<F_YZXM>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YZXM"].ToString().Trim()) + "</F_YZXM>";
                    xml = xml + "<F_STUDY_UID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_STUDY_UID"].ToString().Trim()) + "</F_STUDY_UID>";
                    xml = xml + "<F_XM>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_XM"].ToString().Trim()) + "</F_XM>";
                    xml = xml + "<F_XB_CODE>" + xb + "</F_XB_CODE>";
                    xml = xml + "<F_XB>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_XB"].ToString().Trim()) + "</F_XB>";
                    xml = xml + "<F_XB_CODESYSTEM>2.16.840.1.113883.4.487.3.4.1.1.5</F_XB_CODESYSTEM>";
                    xml = xml + "<F_NL>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_NL"].ToString().Trim()) + "</F_NL>";
                    xml = xml + "<F_AGE>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_AGE"].ToString().Trim()) + "</F_AGE>";
                    xml = xml + "<F_HY_CODE>" + hy + "</F_HY_CODE>";
                    xml = xml + "<F_HY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_HY"].ToString().Trim()) + "</F_HY>";
                    xml = xml + "<F_HY_CODESYSTEM>" + hylb + "</F_HY_CODESYSTEM>";
                    xml = xml + "<F_MZ_CODE></F_MZ_CODE>";
                    xml = xml + "<F_MZ></F_MZ>";
                    xml = xml + "<F_MZ_CODESYSTEM></F_MZ_CODESYSTEM>";
                    xml = xml + "<F_ZY_CODE></F_ZY_CODE>";
                    xml = xml + "<F_ZY></F_ZY>";
                    xml = xml + "<F_ZY_CODESYSTEM></F_ZY_CODESYSTEM>";
                    xml = xml + "<F_SFZH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFZH"].ToString().Trim()) + "</F_SFZH>";
                    xml = xml + "<F_LXXX>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_LXXX"].ToString().Trim()) + "</F_LXXX>";
                    xml = xml + "<F_BRLB_CODE>" + brlb + "</F_BRLB_CODE>";
                    xml = xml + "<F_BRLB>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BRLB"].ToString().Trim()) + "</F_BRLB>";
                    xml = xml + "<F_BRLB_CODESYSTEM>2.16.840.1.113883.4.487.3.4.1.1.9</F_BRLB_CODESYSTEM>";
                    xml = xml + "<F_FB_CODE></F_FB_CODE>";
                    xml = xml + "<F_FB>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_FB"].ToString().Trim()) + "</F_FB>";
                    xml = xml + "<F_FB_CODESYSTEM>2.16.840.1.113883.4.487.3.4.1.1.12</F_FB_CODESYSTEM>";
                    xml = xml + "<F_ZYH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ZYH"].ToString().Trim()) + "</F_ZYH>";
                    xml = xml + "<F_MZH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_MZH"].ToString().Trim()) + "</F_MZH>";
                    xml = xml + "<F_BQ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BQ"].ToString().Trim()) + "</F_BQ>";
                    xml = xml + "<F_SJKS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SJKS"].ToString().Trim()) + "</F_SJKS>";
                    xml = xml + "<F_CH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_CH"].ToString().Trim()) + "</F_CH>";
                    xml = xml + "<F_SJDW>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SJDW"].ToString().Trim()) + "</F_SJDW>";
                    xml = xml + "<F_SJYS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SJYS"].ToString().Trim()) + "</F_SJYS>";
                    string sdrq = "";
                    try
                    {
                        sdrq = Convert.ToDateTime(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                        sdrq = "";
                    }
                    xml = xml + "<F_SDRQ>" + System.Security.SecurityElement.Escape(sdrq) + "</F_SDRQ>";
                    xml = xml + "<F_JSY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</F_JSY>";
                    xml = xml + "<F_BBLX>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BBLX"].ToString().Trim()) + "</F_BBLX>";
                    xml = xml + "<F_BBQK>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BBQK"].ToString().Trim()) + "</F_BBQK>";
                    xml = xml + "<F_JSYY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JSYY"].ToString().Trim()) + "</F_JSYY>";
                    xml = xml + "<F_SF>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SF"].ToString().Trim()) + "</F_SF>";
                    xml = xml + "<F_BBMC>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BBMC"].ToString().Trim()) + "</F_BBMC>";
                    xml = xml + "<F_LCZD>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_LCZD"].ToString().Trim()) + "</F_LCZD>";
                    xml = xml + "<F_LCZL>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_LCZL"].ToString().Trim()) + "</F_LCZL>";
                    xml = xml + "<F_RYSJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_RYSJ"].ToString().Trim()) + "</F_RYSJ>";
                    xml = xml + "<F_QCYS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_QCYS"].ToString().Trim()) + "</F_QCYS>";
                    string qcrq = "";
                    try
                    {
                        qcrq = Convert.ToDateTime(bljc.Rows[0]["F_QCRQ"] == null ? DateTime.Now.ToLongDateString() : bljc.Rows[0]["F_QCRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                        qcrq = "";
                    }
                    xml = xml + "<F_QCRQ>" + System.Security.SecurityElement.Escape(qcrq) + "</F_QCRQ>";
                    xml = xml + "<F_JLY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JLY"].ToString().Trim()) + "</F_JLY>";
                    xml = xml + "<F_LKZS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_LKZS"].ToString().Trim()) + "</F_LKZS>";
                    xml = xml + "<F_CKZS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_CKZS"].ToString().Trim()) + "</F_CKZS>";
                    xml = xml + "<F_FY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_FY"].ToString().Trim()) + "</F_FY>";
                    xml = xml + "<F_JXSJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JXSJ"].ToString().Trim()) + "</F_JXSJ>";
                    xml = xml + "<F_BLZD>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLZD"].ToString().Trim()) + "</F_BLZD>";
                    xml = xml + "<F_TSJC>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_TSJC"].ToString().Trim()) + "</F_TSJC>";
                    xml = xml + "<F_BGYS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGYS"].ToString().Trim()) + "</F_BGYS>";
                    xml = xml + "<F_SHYS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SHYS"].ToString().Trim()) + "</F_SHYS>";
                    string bgrq = "";
                    try
                    {
                        bgrq = Convert.ToDateTime(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                        bgrq = "";
                    }
                    xml = xml + "<F_BGRQ>" + System.Security.SecurityElement.Escape(bgrq) + "</F_BGRQ>";
                    xml = xml + "<F_CZYJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_CZYJ"].ToString().Trim()) + "</F_CZYJ>";
                    xml = xml + "<F_XGYJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_XGYJ"].ToString().Trim()) + "</F_XGYJ>";
                    xml = xml + "<F_ZDGJC>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ZDGJC"].ToString().Trim()) + "</F_ZDGJC>";
                    xml = xml + "<F_YYX>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YYX"].ToString().Trim()) + "</F_YYX>";
                    xml = xml + "<F_WFBGYY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_WFBGYY"].ToString().Trim()) + "</F_WFBGYY>";
                    xml = xml + "<F_BZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BZ"].ToString().Trim()) + "</F_BZ>";
                    xml = xml + "<F_BD_SFFH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BD_SFFH"].ToString().Trim()) + "</F_BD_SFFH>";
                    xml = xml + "<F_BGZT>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGZT"].ToString().Trim()) + "</F_BGZT>";
                    xml = xml + "<F_SFCT>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFCT"].ToString().Trim()) + "</F_SFCT>";
                    xml = xml + "<F_SFDY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFDY"].ToString().Trim()) + "</F_SFDY>";
                    xml = xml + "<F_BGGS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGGS"].ToString().Trim()) + "</F_BGGS>";
                    xml = xml + "<F_GDZT>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_GDZT"].ToString().Trim()) + "</F_GDZT>";
                    xml = xml + "<F_KNHZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_KNHZ"].ToString().Trim()) + "</F_KNHZ>";
                    xml = xml + "<F_ZJYJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ZJYJ"].ToString().Trim()) + "</F_ZJYJ>";
                    xml = xml + "<F_WYYJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_WYYJ"].ToString().Trim()) + "</F_WYYJ>";
                    xml = xml + "<F_SFZT>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFZT"].ToString().Trim()) + "</F_SFZT>";
                    xml = xml + "<F_SFJG>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFJG"].ToString().Trim()) + "</F_SFJG>";
                    xml = xml + "<F_JBBM_CN>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JBBM_CN"].ToString().Trim()) + "</F_JBBM_CN>";
                    xml = xml + "<F_JBBM_ENG>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JBBM_ENG"].ToString().Trim()) + "</F_JBBM_ENG>";
                    xml = xml + "<F_JBMC>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JBMC"].ToString().Trim()) + "</F_JBMC>";
                    xml = xml + "<F_YBLH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YBLH"].ToString().Trim()) + "</F_YBLH>";
                    xml = xml + "<F_SJCL>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SJCL"].ToString().Trim()) + "</F_SJCL>";
                    xml = xml + "<F_YBLZD>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YBLZD"].ToString().Trim()) + "</F_YBLZD>";
                    xml = xml + "<F_BGFSFS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGFSFS"].ToString().Trim()) + "</F_BGFSFS>";
                    xml = xml + "<F_SCYS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SCYS"].ToString().Trim()) + "</F_SCYS>";
                    xml = xml + "<F_SFFH>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFFH"].ToString().Trim()) + "</F_SFFH>";
                    xml = xml + "<F_SPARE1>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE1"].ToString().Trim()) + "</F_SPARE1>";
                    xml = xml + "<F_SPARE2>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE2"].ToString().Trim()) + "</F_SPARE2>";
                    xml = xml + "<F_SPARE3>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE3"].ToString().Trim()) + "</F_SPARE3>";
                    xml = xml + "<F_SPARE4>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE4"].ToString().Trim()) + "</F_SPARE4>";
                    string spare5 = "";
                    try
                    {
                        spare5 = Convert.ToDateTime(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                        spare5 = "";
                    }
                    xml = xml + "<F_SPARE5>" + System.Security.SecurityElement.Escape(spare5) + "</F_SPARE5>";
                    xml = xml + "<F_SPARE6>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE6"].ToString().Trim()) + "</F_SPARE6>";
                    string spare7 = "";
                    try
                    {
                        spare7 = Convert.ToDateTime(bljc.Rows[0]["F_SPARE7"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                        spare7 = "";
                    }
                    xml = xml + "<F_SPARE7>" + System.Security.SecurityElement.Escape(spare7) + "</F_SPARE7>";
                    xml = xml + "<F_SPARE8>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE8"].ToString().Trim()) + "</F_SPARE8>";
                    xml = xml + "<F_SPARE9>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE9"].ToString().Trim()) + "</F_SPARE9>";
                    xml = xml + "<F_SPARE10>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SPARE10"].ToString().Trim()) + "</F_SPARE10>";
                    xml = xml + "<F_BY1>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BY1"].ToString().Trim()) + "</F_BY1>";
                    xml = xml + "<F_BY2>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BY2"].ToString().Trim()) + "</F_BY2>";
                    xml = xml + "<F_TXML>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_TXML"].ToString().Trim()) + "</F_TXML>";
                    xml = xml + "<F_ZPZT>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ZPZT"].ToString().Trim()) + "</F_ZPZT>";
                    xml = xml + "<F_MCYJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_MCYJ"].ToString().Trim()) + "</F_MCYJ>";
                    xml = xml + "<F_SFJJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SFJJ"].ToString().Trim()) + "</F_SFJJ>";
                    xml = xml + "<F_TBSID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_TBSID"].ToString().Trim()) + "</F_TBSID>";
                    xml = xml + "<F_TBSMC>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_TBSMC"].ToString().Trim()) + "</F_TBSMC>";
                    xml = xml + "<F_QSB_DYZT>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_QSB_DYZT"].ToString().Trim()) + "</F_QSB_DYZT>";
                    xml = xml + "<F_BGWZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGWZ"].ToString().Trim()) + "</F_BGWZ>";
                    xml = xml + "<F_BGWZ_QRSJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGWZ_QRSJ"].ToString().Trim()) + "</F_BGWZ_QRSJ>";
                    xml = xml + "<F_BGWZ_QRCZY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGWZ_QRCZY"].ToString().Trim()) + "</F_BGWZ_QRCZY>";
                    xml = xml + "<F_BBWZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BBWZ"].ToString().Trim()) + "</F_BBWZ>";
                    xml = xml + "<F_LKWZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_LKWZ"].ToString().Trim()) + "</F_LKWZ>";
                    xml = xml + "<F_QPWZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_QPWZ"].ToString().Trim()) + "</F_QPWZ>";
                    xml = xml + "<F_GDCZY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_GDCZY"].ToString().Trim()) + "</F_GDCZY>";
                    xml = xml + "<F_GDSJ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_GDSJ"].ToString().Trim()) + "</F_GDSJ>";
                    xml = xml + "<F_GDBZ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_GDBZ"].ToString().Trim()) + "</F_GDBZ>";
                    xml = xml + "<F_BGLRY>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BGLRY"].ToString().Trim()) + "</F_BGLRY>";
                    xml = xml + "<F_FZYS>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_FZYS"].ToString().Trim()) + "</F_FZYS>";
                    xml = xml + "<F_YL1>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL1"].ToString().Trim()) + "</F_YL1>";
                    xml = xml + "<F_YL2>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL2"].ToString().Trim()) + "</F_YL2>";
                    xml = xml + "<F_YL3>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL3"].ToString().Trim()) + "</F_YL3>";
                    xml = xml + "<F_YL4>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL4"].ToString().Trim()) + "</F_YL4>";
                    xml = xml + "<F_YL5>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL5"].ToString().Trim()) + "</F_YL5>";
                    xml = xml + "<F_YL6>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL6"].ToString().Trim()) + "</F_YL6>";
                    xml = xml + "<F_YL7>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL7"].ToString().Trim()) + "</F_YL7>";
                    xml = xml + "<F_YL8>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL8"].ToString().Trim()) + "</F_YL8>";
                    xml = xml + "<F_YL9>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL9"].ToString().Trim()) + "</F_YL9>";
                    xml = xml + "<F_YL10>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_YL10"].ToString().Trim()) + "</F_YL10>";
                    xml = xml + "<F_304 />";
                    xml = xml + "<F_sjc />";
                    xml = xml + "<ImageList>";
                    #region 图像处理
                    string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
                    string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
                    string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
                    string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
                    string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
                    string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
                    FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

                    string txml = bljc.Rows[0]["F_txml"].ToString().Trim();



                    //上传XML

                    //下载图像
                    DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + F_blh + "'", "txlb");
                    try
                    {
                        if (ftps == "1")//FTP下载方式
                        {
                            int i2 = 1;
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                i2 = i + 1;
                                string ftpstatus = "";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("开始下载图像");
                                }
                                //bool ftpd = APIFtp.FtpOperator.FtpFileDownLoad(txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(),ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim());
                                //fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                try
                                {
                                    fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(), out ftpstatus);
                                    //System.Diagnostics.Process.Start("pathnetdowntx.exe", txlb.Rows[i]["F_txm"].ToString().Trim() + "@" + txml);
                                }
                                catch
                                {
                                }
                                if (!File.Exists(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim()))
                                {
                                    MessageBox.Show("FTP下载图像出错！");
                                    log.WriteMyLog("FTP下载图像出错！");
                                    return;
                                }
                                else
                                {
                                    FileStream file = new FileStream(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim(), FileMode.Open, FileAccess.Read);
                                    Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                                    file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

                                    file.Close();
                                    xml = xml + "<Image>" + Convert.ToBase64String(imgByte) + "</Image>";
                                }

                                try
                                {
                                    File.Delete(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim());
                                }
                                catch
                                {
                                    log.WriteMyLog("删除图片失败");
                                }
                            }

                        }
                        else//共享下载方式
                        {
                            if (txpath == "")
                            {
                                log.WriteMyLog("sz.ini txpath图像目录未设置");
                                return;
                            }
                            int i2 = 1;
                            for (int i = 0; i < txlb.Rows.Count; i++)
                            {
                                i2 = i + 1;
                                string ftpstatus = "";
                                try
                                {
                                    File.Copy(txpath + txml + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), true);
                                    //FTP上传图像
                                    FileStream file = new FileStream(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim(), FileMode.Open, FileAccess.Read);
                                    Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                                    file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

                                    file.Close();
                                    xml = xml + "<Image>" + Convert.ToBase64String(imgByte) + "</Image>";
                                }
                                catch
                                {
                                    log.WriteMyLog("共享目录不存在！");
                                    return;
                                }

                                try
                                {
                                    File.Delete(ftplocal + @"\" + txlb.Rows[i]["F_txm"].ToString().Trim());
                                }
                                catch
                                {
                                    log.WriteMyLog("删除图片失败");
                                }
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(EX.Message);
                        }
                        log.WriteMyLog(EX.Message);
                    }
                    #endregion
                    xml = xml + "</ImageList>";
                    xml = xml + "</LOGENE>";
                    #endregion


                    //sqlstring = "INSERT INTO DGATE_DOCUMENT_INFO(PK,DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PATIENT_ID,PATIENT_DOMAIN_ID,FILE_TYPE,PAY_LOAD_TYPE,SUB_TYPE,CONTENT,START_TIME,EFFECTIVE_TIME,END_TIME,PATIENT_TYPE)values(";
                    if (bginsert == "ADD")
                    {
                        try
                        {

                            OCMD.CommandText = "select DGATE_DOCUMENT_INFO_SEQUENCE.nextval from dual";
                            OCMD.Parameters.Clear();
                            conn.Open();
                            OracleDataAdapter oda = new OracleDataAdapter(OCMD);

                            DataTable dt1 = new DataTable();

                            oda.Fill(dt1);
                            try
                            {
                                bgid = Convert.ToInt32(dt1.Rows[0][0].ToString());
                            }
                            catch
                            {
                                log.WriteMyLog("转换DGATE_DOCUMENT_INFO_SEQUENCE失败，DGATE_DOCUMENT_INFO_SEQUENCE为：" + dt1.Rows[0][0].ToString());
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            if (debug == "1")
                            {
                                MessageBox.Show(ex.Message);
                            }
                            log.WriteMyLog(ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                        sqlstring = "INSERT INTO DGATE_DOCUMENT_INFO(PK,DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PATIENT_ID,PATIENT_DOMAIN_ID,FILE_TYPE,PAY_LOAD_TYPE,SUB_TYPE,CONTENT,START_TIME,EFFECTIVE_TIME,PATIENT_TYPE,END_TIME,ASSIGNED_PERSON,DOC_NAME,ADMIT_TIME,HIS_TYPE,BED_NO,ASSIGNED_CODE,AUTHOR_CODE,AUTHOR_NAME)values(";
                        sqlstring = sqlstring + "" + bgid.ToString() + ",";
                        sqlstring = sqlstring + "'" + F_blh + "',";
                        sqlstring = sqlstring + "'" + pisid + "',";
                        sqlstring = sqlstring + "'" + F_blh + "',";
                        sqlstring = sqlstring + "'" + pisid + "',";
                        sqlstring = sqlstring + "'TRANS-XML',";
                        sqlstring = sqlstring + "'XDS.PISBG',";
                        sqlstring = sqlstring + "'" + bginsert + "',";
                        sqlstring = sqlstring + ":xml,";

                        sqlstring = sqlstring + "" + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')" + ",";
                        sqlstring = sqlstring + "" + "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                        sqlstring = sqlstring + "'" + brlb + "',";
                        sqlstring = sqlstring + "" + "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS'),";

                        sqlstring = sqlstring + "'" + shys + "',";
                        sqlstring = sqlstring + "'病理活体组织诊断报告书',";
                        sqlstring = sqlstring + "to_date('" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                        sqlstring = sqlstring + "'" + brlb + "',";
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_CH"].ToString().Trim() + "',";
                        sqlstring = sqlstring + "'" + shysgh + "',";
                        sqlstring = sqlstring + "'" + bgysgh + "',";
                        sqlstring = sqlstring + "'" + bgys + "'";
                        sqlstring = sqlstring + ")";
                    }
                    else
                    {
                        sqlstring = string.Format("UPDATE DGATE_DOCUMENT_INFO set PATIENT_ID='{0}', SUB_TYPE='UPDATE', CONTENT=:xml, START_TIME={1}, EFFECTIVE_TIME={2}, PATIENT_TYPE='{3}', END_TIME={4}, EXPORT_STATUS='X0', ASSIGNED_PERSON='{5}', ADMIT_TIME={6}, HIS_TYPE='{7}', BED_NO='{8}', ASSIGNED_CODE='{9}', AUTHOR_CODE='{10}', AUTHOR_NAME='{11}', CLOB_STATUS='0', FILE_TYPE='TRANS-XML' WHERE PK={12}", F_blh, "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')", "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')", brlb, "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')", shys, "to_date('" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')", brlb, bljc.Rows[0]["F_CH"].ToString().Trim(), shysgh, bgysgh, bgys, xmlpkid);
                        try
                        {
                            bgid = int.Parse(xmlpkid);
                        }
                        catch
                        {
                            log.WriteMyLog("转换XMLPKID失败，xmlpid为：" + xmlpkid);
                            return;
                        }
                    }

                    if (debug == "1")
                    {
                        log.WriteMyLog("插入DGATE_DOCUMENT_INFO表语句：" + sqlstring);
                    }

                    OCMD.CommandText = sqlstring;
                    OracleParameter op = new OracleParameter("xml", OracleType.Clob);
                    op.Value = xml;
                    OCMD.Parameters.Add(op);
                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                    #region 写入DGATE_EXTEND_ID_INFO表
                    sqlstring = "insert into DGATE_EXTEND_ID_INFO (PK,DOCUMENT_FK,ID,DOMAIN_ID) values(";
                    sqlstring = sqlstring + "DGATE_EXTEND_ID_INFO_SEQUENCE.nextval,";
                    sqlstring = sqlstring + bgid.ToString() + ",";
                    if (brlb == "1")
                    {
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "',";
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.3')";
                    }
                    else
                    {
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_mzh"].ToString().Trim() + "',";
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.1')";
                    }

                    if (debug == "1")
                    {
                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                    }

                    OCMD.CommandText = sqlstring;
                    OCMD.Parameters.Clear();

                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    #endregion
                    #region 写入DGATE_EXTEND_ID_INFO表
                    sqlstring = "insert into DGATE_EXTEND_ID_INFO (PK,DOCUMENT_FK,ID,DOMAIN_ID) values(";
                    sqlstring = sqlstring + "DGATE_EXTEND_ID_INFO_SEQUENCE.nextval,";
                    sqlstring = sqlstring + bgid.ToString() + ",";
                    sqlstring = sqlstring + "'" + brbh + "',";

                    if (brlb == "1")
                    {
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.10')";
                    }
                    else
                    {
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.9')";
                    }

                    if (debug == "1")
                    {
                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                    }

                    OCMD.CommandText = sqlstring;
                    OCMD.Parameters.Clear();

                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    #endregion


                    # region 生成pdf
                    string bglj = "";
                    string ftplj = "";
                    mdpdf xx = new mdpdf();
                    xx.BMPTOJPG(F_blh, ref bglj, "CG", "0");
                    if (bglj != "")
                    {
                        if (debug == "1")
                        {
                            if (File.Exists(bglj))
                            {
                                MessageBox.Show("生成PDF成功");
                            }
                            else
                            {
                                MessageBox.Show("生成PDF失败");
                            }
                        }
                        string ftpupip = f.ReadString("ftpup", "ftpip", "192.168.1.25");
                        string ftpupuser = f.ReadString("ftpup", "username", "hiupftp");
                        string ftpuppwd = f.ReadString("ftpup", "password", "1");
                        string ftpupremotepath = f.ReadString("ftpup", "remotepath", "");
                        string status;
                        FtpWeb fwup = new FtpWeb(ftpupip, ftpupremotepath, ftpupuser, ftpuppwd);
                        fwup.Makedir(DateTime.Now.ToString("yyyyMMdd"), out status);
                        if (status == "OK")
                        {
                            if (debug == "1")
                            {
                                log.WriteMyLog("FTP建立目录！");
                            }
                            fwup.Upload(bglj, DateTime.Now.ToString("yyyyMMdd"), out status);
                            if (status == "OK")
                            {
                                FileInfo fi = new FileInfo(bglj);
                                ftplj = "PIS" + "//" + DateTime.Now.ToString("yyyyMMdd") + "//" + fi.Name;
                                if (debug == "1")
                                {
                                    log.WriteMyLog("ftp上传成功，上传路径：" + ftplj);
                                }
                                if (bginsert == "ADD")
                                {
                                    try
                                    {

                                        OCMD.CommandText = "select DGATE_DOCUMENT_INFO_SEQUENCE.nextval from dual";
                                        OCMD.Parameters.Clear();
                                        conn.Open();
                                        OracleDataAdapter oda = new OracleDataAdapter(OCMD);

                                        DataTable dt1 = new DataTable();

                                        oda.Fill(dt1);
                                        try
                                        {
                                            bgid = Convert.ToInt32(dt1.Rows[0][0].ToString());
                                        }
                                        catch
                                        {
                                            log.WriteMyLog("转换DGATE_DOCUMENT_INFO_SEQUENCE失败，DGATE_DOCUMENT_INFO_SEQUENCE为：" + dt1.Rows[0][0].ToString());
                                            return;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        if (debug == "1")
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                        log.WriteMyLog(ex.Message);
                                    }
                                    finally
                                    {
                                        conn.Close();
                                    }
                                    sqlstring = "INSERT INTO DGATE_DOCUMENT_INFO(PK,DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PATIENT_ID,PATIENT_DOMAIN_ID,FILE_TYPE,PAY_LOAD_TYPE,SUB_TYPE,CONTENT,START_TIME,EFFECTIVE_TIME,PATIENT_TYPE,END_TIME,FILE_PATH,ASSIGNED_PERSON,DOC_NAME,ADMIT_TIME,HIS_TYPE,BED_NO,ASSIGNED_CODE,AUTHOR_CODE,AUTHOR_NAME)values(";
                                    sqlstring = sqlstring + "" + bgid.ToString() + ",";
                                    sqlstring = sqlstring + "'" + F_blh + "',";
                                    sqlstring = sqlstring + "'" + pisid + "',";
                                    sqlstring = sqlstring + "'" + F_blh + "',";
                                    sqlstring = sqlstring + "'" + pisid + "',";
                                    sqlstring = sqlstring + "'PDF',";
                                    sqlstring = sqlstring + "'XDS.PISBG',";
                                    sqlstring = sqlstring + "'" + bginsert + "',";
                                    sqlstring = sqlstring + "'',";

                                    sqlstring = sqlstring + "" + "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')" + ",";
                                    sqlstring = sqlstring + "" + "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                                    sqlstring = sqlstring + "'" + brlb + "',";
                                    sqlstring = sqlstring + "" + "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS'),";
                                    sqlstring = sqlstring + "'" + ftplj + "',";

                                    sqlstring = sqlstring + "'" + shys + "',";
                                    sqlstring = sqlstring + "'病理活体组织诊断报告书',";
                                    sqlstring = sqlstring + "to_date('" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')" + ",";
                                    sqlstring = sqlstring + "'" + brlb + "',";
                                    sqlstring = sqlstring + "'" + bljc.Rows[0]["F_CH"].ToString().Trim() + "',";
                                    sqlstring = sqlstring + "'" + shysgh + "',";
                                    sqlstring = sqlstring + "'" + bgysgh + "',";
                                    sqlstring = sqlstring + "'" + bgys + "'";
                                    sqlstring = sqlstring + ")";
                                }
                                else
                                {
                                    sqlstring = string.Format("UPDATE DGATE_DOCUMENT_INFO set PATIENT_ID='{0}', SUB_TYPE='UPDATE', START_TIME={1}, EFFECTIVE_TIME={2}, PATIENT_TYPE='{3}', END_TIME={4}, ASSIGNED_PERSON='{5}', ADMIT_TIME={6}, HIS_TYPE='{7}', BED_NO='{8}', ASSIGNED_CODE='{9}', AUTHOR_CODE='{10}', AUTHOR_NAME='{11}', FILE_PATH='{12}',EXPORT_STATUS='X0' WHERE PK={13}", F_blh, "to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')", "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')", brlb, "to_date('" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')", shys, "to_date('" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "', 'yyyy-MM-dd HH24:MI:SS')", brlb, bljc.Rows[0]["F_CH"].ToString().Trim(), shysgh, bgysgh, bgys, ftplj, pdfpkid);
                                    try
                                    {
                                        bgid = int.Parse(pdfpkid);
                                    }
                                    catch
                                    {
                                        log.WriteMyLog("转换pdfpkid失败，pdfpkid为：" + pdfpkid);
                                        return;
                                    }
                                }
                                if (debug == "1")
                                {
                                    log.WriteMyLog("插入DGATE_DOCUMENT_INFO表语句：" + sqlstring);
                                }

                                OCMD.CommandText = sqlstring;
                                OCMD.Parameters.Clear();
                                try
                                {
                                    conn.Open();
                                    OCMD.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    if (debug == "1")
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                    log.WriteMyLog(ex.Message);
                                }
                                finally
                                {
                                    conn.Close();
                                }

                                //清除pdf和图像
                                try
                                {
                                    Directory.Delete(Path.Combine(ftplocal, F_blh), true);
                                }
                                catch
                                {
                                    log.WriteMyLog("删除PDF失败");
                                }
                            }
                        }

                    }
                    else
                    {
                        log.WriteMyLog("生成PDF失败");
                    }
                    #endregion

                    //sqlstring = "INSERT INTO DGATE_DOCUMENT_INFO(PK,DOCUMENT_UNIQUE_ID,DOCUMENT_DOMAIN_ID,PATIENT_ID,PATIENT_DOMAIN_ID,FILE_TYPE,PAY_LOAD_TYPE,SUB_TYPE,CONTENT,START_TIME,EFFECTIVE_TIME,END_TIME,PATIENT_TYPE)values(";

                    #endregion
                    #region 写入DGATE_EXTEND_ID_INFO表
                    sqlstring = "insert into DGATE_EXTEND_ID_INFO (PK,DOCUMENT_FK,ID,DOMAIN_ID) values(";
                    sqlstring = sqlstring + "DGATE_EXTEND_ID_INFO_SEQUENCE.nextval,";
                    sqlstring = sqlstring + bgid.ToString() + ",";
                    if (brlb == "1")
                    {
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "',";
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.3')";
                    }
                    else
                    {
                        sqlstring = sqlstring + "'" + bljc.Rows[0]["F_mzh"].ToString().Trim() + "',";
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.1')";
                    }

                    if (debug == "1")
                    {
                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                    }

                    OCMD.CommandText = sqlstring;
                    OCMD.Parameters.Clear();

                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    #endregion
                    #region 写入DGATE_EXTEND_ID_INFO表
                    sqlstring = "insert into DGATE_EXTEND_ID_INFO (PK,DOCUMENT_FK,ID,DOMAIN_ID) values(";
                    sqlstring = sqlstring + "DGATE_EXTEND_ID_INFO_SEQUENCE.nextval,";
                    sqlstring = sqlstring + bgid.ToString() + ",";
                    sqlstring = sqlstring + "'" + brbh + "',";

                    if (brlb == "1")
                    {
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.10')";
                    }
                    else
                    {
                        sqlstring = sqlstring + "'2.16.840.1.113883.4.487.5.6.4.9')";
                    }

                    if (debug == "1")
                    {
                        log.WriteMyLog("插入DGATE_EXTEND_ID_INFO表语句：" + sqlstring);
                    }

                    OCMD.CommandText = sqlstring;
                    OCMD.Parameters.Clear();

                    try
                    {
                        conn.Open();
                        OCMD.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (debug == "1")
                        {
                            MessageBox.Show(ex.Message);
                        }
                        log.WriteMyLog(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                    #endregion
                }
            }
        }

        public static string GetHttpGet(string url)
        {
            HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(url);
            objWebRequest.Method = "GET";
            

            HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string rontext = sr.ReadToEnd(); // 返回的数据
            return rontext;
        }


    }
}
