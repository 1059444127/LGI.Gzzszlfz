using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using dbbase;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class sxeyy
    {
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void sxeyy_hx(string F_blh)
        {
            DataTable jcxx = new DataTable();
            DataTable bltx = new DataTable();
            string xml_check;  //更新检查状态
            string xml_report; //更新报告状态
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                bltx = aa.GetDataTable("select * from t_tx where F_blh='" + F_blh + "'", "bltx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }


            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }
            #region  获取系统设置表中msg即写日志开关及webservice地址链接
            string msg = null;
            DataTable T_SZ = new DataTable();
            T_SZ = aa.GetDataTable("select F_SZZ,F_XL from T_SZ where F_DL='JK' and (F_XL='JK_MSG' or F_XL='JK_WebServicesURL')", "T_SZ");
            string URL = null;

            for (int y = 0; y < T_SZ.Rows.Count; y++)
            {
                if (T_SZ.Rows[y]["F_XL"].ToString().Trim() == "JK_MSG")
                {

                    msg = T_SZ.Rows[0]["F_SZZ"].ToString().Trim();
                }
                if (T_SZ.Rows[y]["F_XL"].ToString().Trim() == "JK_WebServicesURL")
                {
                    URL = T_SZ.Rows[y]["F_SZZ"].ToString().Trim();
                }


            }
            if (URL == "")
            {
                URL = "http://168.192.8.10:8080/WebService/services/MatrixService?wsdl";
                    
            }
            #endregion
            if (jcxx.Rows.Count > 0)
            {
                #region 报告状态未审核
                if (jcxx.Rows[0]["F_BGZT"].ToString() != "已审核")
                {
                    xml_check = update_xmlcheck(jcxx, jcxx.Rows[0]["F_sqxh"].ToString().Trim(), F_blh, "SC", jcxx.Rows[0]["F_BLK"].ToString().Trim());
                    if (jcxx.Rows[0]["F_HXBZ"].ToString().Trim() == "" && jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "")     //HXBZ 1-表示已更新状态   2-报告已传过
                    {


                        ///此处增加webservice执行xml,更改状态
                        if (ExcuteWebservice("JCZT", xml_check, URL) == "1")
                        {
                            aa.ExecuteSQL("update t_jcxx set f_hxbz='1' where f_blh='" + F_blh + "'");
                            if (msg == "1")
                            {
                                log.WriteMyLog(F_blh + "更改检查状态成功！");
                            }

                        }
                        else
                        {
                            log.WriteMyLog(F_blh + "更改检查状态失败！");
                        }

                    }


                    else if (jcxx.Rows[0]["F_HXBZ"].ToString().Trim() == "2")//2表示已经回写过诊断，此处要更新为空的状态
                    {

                        xml_report = update_xmlreportnoapply(jcxx, "C", bltx);


                        //此处增加更新报告诊断置为空状态
                        if (ExcuteWebservice("BG", xml_report, URL) == "1")
                        {

                            if (msg == "1")
                            {
                                log.WriteMyLog(F_blh + "清空报告内容成功！");
                            }

                        }
                        else
                        {
                            log.WriteMyLog(F_blh + "清空报告内容失败！");
                        }

                    }
                    if (msg == "1")
                    {
                        log.WriteMyLog(F_blh + "回写标志为空或者已经更改过检查状态！");
                    }
                }
                #endregion

                #region 报告状态已审核
                if (jcxx.Rows[0]["F_BGZT"].ToString() == "已审核")
                {

                    if (jcxx.Rows[0]["F_sqxh"].ToString().Trim() != "")
                    {



                        //先更新检查状态为检查完成--上传报告--hxbz改为2已上传报告


                        //此处增加webservice执行xml,更改状态
                        if (jcxx.Rows[0]["F_HXBZ"].ToString().Trim() != "2")
                        {
                            xml_check = update_xmlcheck(jcxx, jcxx.Rows[0]["F_sqxh"].ToString().Trim(), F_blh, "CM", jcxx.Rows[0]["F_BLK"].ToString().Trim());
                            if (ExcuteWebservice("JCZT", xml_check, URL) == "1")
                            {

                                if (msg == "1")
                                {
                                    log.WriteMyLog(F_blh + "更改检查状态成功,改为CM检查完成！");
                                }

                            }
                            else
                            {
                                log.WriteMyLog(F_blh + "更改检查状态失败！");
                            }
                        }

                    }
                    if (msg == "1")
                    {
                        log.WriteMyLog(F_blh + "开始执行报告xml拼写并回传！");
                    }

                    xml_report = update_xmlreportnoapply(jcxx, "F", bltx);
                    if (ExcuteWebservice("BG", xml_report, URL) == "1")
                    {
                        aa.ExecuteSQL("update t_jcxx set f_hxbz='2' where f_blh='" + F_blh + "'");
                        if (msg == "1")
                        {
                            log.WriteMyLog(F_blh + "更新报告成功！");
                        }

                    }
                    else
                    {
                        log.WriteMyLog(F_blh + "更新报告失败！");
                    }


                }
                #endregion
            }
        }





        /// <summary>
        /// 更新检查状态
        /// </summary>
        /// <param name="sqxh">申请id -申请序号</param>
        /// <param name="blh">检查号-病理号</param>
        /// <param name="status">检查状态 SC=开始检查 CM=检查完成 DC=停止检查 DA=检查数据可用（图像已匹配）</param>
        /// <returns></returns>
        private string update_xmlcheck(DataTable bljc, string sqxh, string blh, string status, string blk)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                   "<operation name =\"updateStudyStatus\">" +
                   "<appid disc=\"申请ID\">" + sqxh + "</appid>" +
                   "<modality disc=\"检查设备\">PS</modality>" +
                   "<studyid disc=\"检查号\">" + blh + "</studyid>" +
                // "<checkMethod disc=\"检查方法\">" + blk + "</checkMethod>" +
                  "<status disc=\"检查状态\">" + status + "</status>" +
                  "<studydatetime  disc=\"检查时间\">" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</studydatetime>" +
                  "<technician_id disc=\"检查技师工号\">" + f.ReadString("yh", "yhbh", "").ToString().Trim() + "</technician_id>" +
                  "<technician_name disc=\"检查技师姓名\">" + f.ReadString("yh", "yhmc", "").ToString().Trim() + "</technician_name>" +
                  "</operation>";
            return xml;
        }

        /// <summary>
        /// 拼写回传报告字符串
        /// </summary>
        /// <param name="bljc">jcxx信息表DataTable</param>
        /// <param name="reportstatus">报告标志</param>
        /// <returns>返回报告xml</returns>
        private string update_xmlreport(DataTable bljc, string reportstatus, DataTable bltx)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?> " + " <operation name =\"sendReportToHis\">" +

                                      "<appid disc=\"申请ID\">" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</appid>" +
                                      "<checkitem disc=\"检查项目\">" + bljc.Rows[0]["F_YZXM"].ToString().Trim().Replace('^', ',') + "</checkitem>" +
                                        "<studyid disc=\"检查号\">" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</studyid>" +
                                       "<modality  disc=\"检查设备\">PS</modality >" +
                                       "<checkMethod disc=\"检查方法\">" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</checkMethod>";

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {
                xml = xml + "<checkDesc>" + bljc.Rows[0]["F_RYSJ"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" + bljc.Rows[0]["F_jxsj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "</checkDesc>" +
                                      "<checkResult>" + bljc.Rows[0]["F_BLZD"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" + bljc.Rows[0]["F_TSJC"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "</checkResult>" +
                                      "<reporting_physician disc=\"报告医生\">" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</reporting_physician>" +
                                      "<reporting_datetime disc=\"报告时间\">" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</reporting_datetime>" +
                                      "<audit_physician disc=\"审核医生\">" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</audit_physician>";
                xml = xml + "<audit_datetime disc=\"审核时间\">" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</audit_datetime>" +
                      "<reportstatus>" + reportstatus + "</reportstatus>";
                xml = xml + "<images>";
                for (int x = 0; x < bltx.Rows.Count; x++)
                {
                    xml = xml + "<image>" + "pathimages\\" + bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + bltx.Rows[x]["F_TXM"].ToString().Trim() + "</image>";
                }
                xml = xml + "</images>" + "</operation>";

            }
            else
            {
                xml = xml + "<checkDesc>" + "" + "</checkDesc>" +
                                      "<checkResult>" +
                                      "<reporting_physician disc=\"报告医生\"/>" +
                                      "<reporting_datetime disc=\"报告时间\"/>" +
                                      "<audit_physician disc=\"审核医生\"/>";
                xml = xml + "<images/>";
                xml = xml + "<audit_datetime disc=\"审核时间\">" + "" + "</audit_datetime>" + "<reportstatus>" + reportstatus + "</reportstatus>" + "</operation>";
            }
            //  string xml1 = System.Text.Encoding.Default.GetString(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("GB2312").GetBytes(xml)).ToArray());
            return xml;
        }


        /// <summary>
        /// 拼写回传报告xml，无申请单时
        /// </summary>
        /// <param name="bljc">jcxx表</param>
        /// <param name="reportstatus">报告标志</param>
        /// <returns></returns>
        private string update_xmlreportnoapply(DataTable bljc, string reportstatus, DataTable bltx)
        {
            string sex = bljc.Rows[0]["F_XB"].ToString().Trim();
            if (sex == "男")
                sex = "M";
            else
                if (sex == "女")
                    sex = "F";
                else
                    sex = "O";

            string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
            if (brlb == "住院")
                brlb = "I";
            else
                if (brlb == "门诊")
                    brlb = "O";
                else
                    brlb = "E";


            string hy = bljc.Rows[0]["F_HY"].ToString().Trim();
            if (hy == "已婚")
                hy = "M";
            else
                if (hy == "未婚")
                    hy = "B";
                else
                    if (hy == "离婚")
                        hy = "D";
                    else
                        if (hy == "丧偶")
                            hy = "W";
                        else
                            hy = "O";
            string patid = null;
            if (bljc.Rows[0]["F_BRBH"].ToString().Trim() == "")
            {
                patid = "PS" + bljc.Rows[0]["F_BLH"].ToString().Trim();
            }
            else
            {
                patid = bljc.Rows[0]["F_BRBH"].ToString().Trim();
            }
            string appid = null;
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                appid = "PS" + bljc.Rows[0]["F_BLH"].ToString().Trim();
            }
            else
            {
                appid = bljc.Rows[0]["F_SQXH"].ToString().Trim();
            }

            string itemname = null;
            if (bljc.Rows[0]["F_YZXM"].ToString().Trim() == "")
            {
                itemname = bljc.Rows[0]["F_BLK"].ToString().Trim();
            }
            else
            {
                itemname = bljc.Rows[0]["F_YZXM"].ToString().Trim();
            }


            string xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                               "<operation name =\"sendReportToMatrix\">" +
                               "<appid disc=\"申请ID\">" + appid + "</appid>" +
                               "<name disc=\"姓名\">" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</name>" +
                              "<sex disc=\"性别\">" + sex + "</sex>" +
                              "<idno disc=\"身份证号码\">" + bljc.Rows[0]["F_SFZH"].ToString().Trim() + "</idno>" +
                              "<birthdate disc=\"出生日期\"></birthdate>" +
                              "<address disc=\"地址\">" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "</address>" +
                              "<phoneno></phoneno>" +
                              "<maritalstatus>" + hy + "</maritalstatus>" +
                              "<cardno disc=\"就诊卡号\"></cardno>" +
                              "<deptno disc=\"申请科室号\"></deptno>" +
                              "<deptname disc=\"申请科室名\">" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "</deptname>" +
                              "<doctorno disc=\"申请医生号\"></doctorno>" +
                              "<doctorname disc=\"申请医生名称\">" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "</doctorname>" +
                              "<patsource disc=\"病人来源\">" + brlb + "</patsource>" +
                              "<inpatno disc=\"住院号\">" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "</inpatno>" +
                              "<outpatno disc=\"门诊号\">" + bljc.Rows[0]["F_mzh"].ToString().Trim() + "</outpatno>" +
                              "<ward disc=\"病区\">" + bljc.Rows[0]["F_bq"].ToString().Trim() + "</ward>" +
                              "<bedno disc=\"床号\">" + bljc.Rows[0]["F_ch"].ToString().Trim() + "</bedno>" +
                              "<patid disc=\"病人ID\">" + patid + "</patid>" +

                              "<studyid>" + "PS" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</studyid>" +
                              "<clinicdiag disc=\"临床诊断\">" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</clinicdiag>" +
                              "<clinicdesc disc=\"检查目的\"></clinicdesc>" +
                              "<modality disc=\"检查类型\">PS</modality>" +
                               "<itemno disc=\"项目编码\"></itemno>" +
                               "<itemname disc=\"项目名称\">" + itemname + "</itemname>" +
                                "<price disc=\"价格\"></price>" +
                              "<body disc=\"检查部位\">" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "</body>" +
                              "<checkMethod>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</checkMethod>" +

                              "<appdatetime disc=\"申请时间\">" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</appdatetime>";




            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {
                xml = xml + "<checkDesc>" + bljc.Rows[0]["F_rysj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" + bljc.Rows[0]["F_jxsj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "</checkDesc>" +
                                 "<checkResult>" + bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" + bljc.Rows[0]["F_tsjc"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "</checkResult>" +
                                 "<reporting_physician>" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</reporting_physician>" +
                                 "<reporting_datetime>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</reporting_datetime>" +
                                 "<audit_physician>" + bljc.Rows[0]["F_shys"].ToString().Trim() + "</audit_physician>";
                xml = xml + "<audit_datetime>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</audit_datetime>";
                xml = xml + "<path>2</path>" + "<reportstatus>" + reportstatus + "</reportstatus>";
                xml = xml + "<images>";



                for (int x = 0; x < bltx.Rows.Count; x++)
                {
                    xml = xml + "<image>" + "pathimages\\" + bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + bltx.Rows[x]["F_TXM"].ToString().Trim() + "</image>";
                }
                xml = xml + "</images>" + "</operation>";

            }
            else
            {
                xml = xml + "<checkDesc/>" +
                               "<checkResult></checkResult>" +
                               "<reporting_physician></reporting_physician>" +
                               "<reporting_datetime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</reporting_datetime>" +
                               "<audit_physician></audit_physician>";

                xml = xml + "<audit_datetime>" + " " + "</audit_datetime>";

                xml = xml + "<path>2</path>" + "<reportstatus>" + reportstatus + "</reportstatus><images></images></operation>";



            }

            return xml;
        }


        /// <summary>
        /// 执行webservice  传入执行类型（JCZT,BG）,及字符串xml
        /// </summary>
        /// <param name="czlx">执行类型(JCZT-检查状态,BG)</param>
        /// <param name="xml">执行的字符串</param>
        /// <returns></returns>
        private string ExcuteWebservice(string czlx, string xml, string webservice_url)
        {
            string result = null;
            try
            {
                SXYDDEYYWeb.MatrixService sxeyy = new PathHISZGQJK.SXYDDEYYWeb.MatrixService();
                sxeyy.Proxy = null;
                sxeyy.Url = webservice_url;

                if (czlx == "JCZT")
                {

                    result = sxeyy.updateStudyStatus(xml);
                }
                else
                {
                    // result = sxeyy.sendReport(xml);
                    result = sxeyy.sendReportToMatrix(xml);
                }

            }
            catch (Exception e)
            {
                result = e.Message.ToString();
            }
            if (result != "1")
            {
                log.WriteMyLog(xml + "-------------错误原因：" + result);
            }
            return result;
        }
    }
}
