using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using ZgqClassPub;

namespace PathHISZGQJK
{
    /// <summary>
    /// 山西医大二院,无申请单
    /// </summary>
    class sxdey
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug1)
        {

            string msg = f.ReadString("savetohis", "msg", "");
            string debug = f.ReadString("savetohis", "debug", "");
            string URL = f.ReadString("savetohis", "URL", "");
            string sqd = f.ReadString("savetohis", "sqd", "");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

          

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

            DataTable T_SZ = new DataTable();
            T_SZ = aa.GetDataTable("select F_SZZ,F_XL from T_SZ where F_DL='JK' and (F_XL='JK_MSG' or F_XL='JK_WebServicesURL')", "T_SZ");

            for (int y = 0; y < T_SZ.Rows.Count; y++)
            {
                if(T_SZ.Rows[0]["F_XL"].ToString().Trim()=="JK_MSG")
                {
                    if (msg.Trim() == "")
                        msg = T_SZ.Rows[0]["F_SZZ"].ToString().Trim();
                }
                if (T_SZ.Rows[0]["F_XL"].ToString().Trim() == "JK_WebServicesURL")
                {
                    if (URL.Trim() == "")
                        URL = T_SZ.Rows[0]["F_SZZ"].ToString().Trim();
                }
            }

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" || bljc.Rows[0]["f_hxbz"].ToString().Trim() == "2")
                {

                string bgzt="F";
                if(bljc.Rows[0]["F_BGZT"].ToString().Trim() != "已审核")
                    bgzt = "C";

                    String xml = "";
                    DataTable bltx = new DataTable();
                    bltx = aa.GetDataTable("select * from V_HIS_TX where F_blh='" + blh + "'", "bljc");

                    try
                    {

                        if (sqd != "1")
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
                            //MessageBox.Show(bljc.Rows[0]["F_blzd"].ToString().Trim());
                            //MessageBox.Show(bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n',(char)36).Replace('\r',(char)36));
                            //MessageBox.Show(bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%ENTER%"));
                   
                            //  Replace(textStr, @"[/n/r]", "");  string tempStr = newStr.Replace((char)13, (char)0);Replace('\n',(char)32).Replace('\r',(char)32);    
                           // return tempStr.Replace((char)10, (char)0);

                            xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                                    "<operation name =\"sendReportToMatrix\">" +
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
                            "<doctorname disc=\"申请医生名称\">" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</doctorname>" +
                            "<patsource disc=\"病人来源\">" + brlb + "</patsource>" +
                            "<inpatno disc=\"住院号\">" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "</inpatno>" +
                            "<outpatno disc=\"门诊号\">" + bljc.Rows[0]["F_mzh"].ToString().Trim() + "</outpatno>" +
                            "<ward disc=\"病区\">" + bljc.Rows[0]["F_bq"].ToString().Trim() + "</ward>" +
                            "<bedno disc=\"床号\">" + bljc.Rows[0]["F_ch"].ToString().Trim() + "</bedno>" +
                            "<patid disc=\"病人ID\">" +"PS"+ bljc.Rows[0]["F_blh"].ToString().Trim() + "</patid>" +
                            "<appid disc=\"申请ID\">" +"PS"+ bljc.Rows[0]["F_BLH"].ToString().Trim() + "</appid>" +
                            "<studyid>" +"PS"+ blh + "</studyid>" +
                            "<clinicdiag disc=\"临床诊断\">" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</clinicdiag>" +
                            "<clinicdesc disc=\"检查目的\">" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</clinicdesc>" +
                            "<modality disc=\"检查类型\">PS</modality>" +
                            "<appdatetime disc=\"申请时间\">" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</appdatetime>" +
                            "<itemno disc=\"项目编码\"></itemno>" +
                            "<itemname disc=\"项目名称\">" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "</itemname>" +
                            "<price disc=\"价格\"></price>" +
                            "<body disc=\"检查部位\">" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "</body>" +
                            "<checkMethod>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</checkMethod>" +
                            "<checkDesc>" + bljc.Rows[0]["F_rysj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" + bljc.Rows[0]["F_jxsj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "</checkDesc>" +
                            "<checkResult>" + bljc.Rows[0]["F_blzd"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" + bljc.Rows[0]["F_tsjc"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "</checkResult>" +
                            "<reporting_physician>" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</reporting_physician>" +
                            "<reporting_datetime>" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</reporting_datetime>" +
                            "<audit_physician>" + bljc.Rows[0]["F_shys"].ToString().Trim() + "</audit_physician>";
                            if (bgzt == "F")
                                xml = xml + "<audit_datetime>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</audit_datetime>";
                            else
                                xml = xml + "<audit_datetime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</audit_datetime>";

                            xml = xml + "<path>2</path>" + "<reportstatus>" + bgzt + "</reportstatus>";

                            xml = xml + "<images>";
                            for (int x = 0; x < bltx.Rows.Count; x++)
                            {
                                xml = xml + "<image>" + "pathimages\\" + bljc.Rows[0]["F_TXML"].ToString().Trim() + "\\" + bltx.Rows[x]["F_TXM"].ToString().Trim() + "</image>";
                            }
                            xml = xml + "</images>" + "</operation>";

                        }
                        else
                        {
                            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
                            {
                                if (msg == "1")
                                    MessageBox.Show("申请单为空，不会写");
                                log.WriteMyLog("申请单为空，不会写");
                                return;
                            }


                            xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                                     "<operation name =\"sendReportToHis\">" +
                                     "<appid disc=\"申请ID\">" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</appid>" +
                                     "<checkitem disc=\"检查项目\">" + bljc.Rows[0]["F_YZXM"].ToString().Trim().Replace('^',',') + "</checkitem>" +
                                     "<checkMethod disc=\"检查方法\">" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</checkMethod>" +
                                     "<checkDesc disc=\"检查描述\">" + bljc.Rows[0]["F_RYSJ"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" +bljc.Rows[0]["F_jxsj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%")+ "</checkDesc>" +
                                     "<checkResult disc=\"检查结果\">" + bljc.Rows[0]["F_BLZD"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" +bljc.Rows[0]["F_TSJC"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%")+ "</checkResult>" +
                                     "<reporting_physician disc=\"报告医生\">" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</reporting_physician>" +
                                     "<reporting_datetime disc=\"报告时间\">" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</reporting_datetime>" +
                                     "<audit_physician disc=\"审核医生\">" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</audit_physician>";
                            if (bgzt == "F")
                            {
                                xml = xml + "<audit_datetime disc=\"审核时间\">" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</audit_datetime>" +
                                      "<reportstatus>F</reportstatus>" + "</operation>";
                            }
                            else
                            {
                                xml = xml + "<audit_datetime disc=\"审核时间\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</audit_datetime>" +
                                    "<reportstatus>C</reportstatus>" + "</operation>";
                            }
                                  

                        }

                        if (debug == "1")
                            log.WriteMyLog("回写XML：" + xml);

                    }
                    catch (Exception ee)
                    {
                        if (msg == "1")
                            MessageBox.Show("XML字符串异常" + ee.ToString());
                        log.WriteMyLog("XML字符串异常" + ee.ToString());
                        return;
                    }

                    if (xml.Trim() == "")
                    {
                        if (msg == "1")
                            MessageBox.Show("XML为空，不回传");
                        log.WriteMyLog("XML为空，不回传");
                        return;
                    }
                    else
                    {
                        try
                        {

                            ////URL = "http://168.192.8.10:8080/WebService/services/MatrixService?wsdl";

                            SXYDDEYYWeb.MatrixService sdey = new PathHISZGQJK.SXYDDEYYWeb.MatrixService();
                            string rtn_msg = sdey.sendReportToMatrix(xml);
                            if (debug == "1")
                            MessageBox.Show("返回值："+rtn_msg);
                        if (rtn_msg == "1")
                        {
                            if (bgzt == "F")
                                aa.ExecuteSQL("update T_JCXX  set f_hxbz='2' where F_blh='" + blh + "'");
                        }
                        else
                        {
                            if (msg == "1")
                                MessageBox.Show("回写返回值：" + rtn_msg);
                        }
                        }
                        catch (Exception e2)
                        {
                            if (msg == "1")
                            MessageBox.Show("报告回传异常：" + e2.ToString());
                            log.WriteMyLog("报告回传异常：" + e2.ToString());
                        }

                    }

                }
                else
                {


                }

            }
        
    }
}
