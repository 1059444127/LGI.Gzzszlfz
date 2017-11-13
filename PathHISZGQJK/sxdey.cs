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
    /// ɽ��ҽ���Ժ,�����뵥
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
                MessageBox.Show("�������ݿ����������⣡");
                log.WriteMyLog("�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                log.WriteMyLog("������д���");
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

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����" || bljc.Rows[0]["f_hxbz"].ToString().Trim() == "2")
                {

                string bgzt="F";
                if(bljc.Rows[0]["F_BGZT"].ToString().Trim() != "�����")
                    bgzt = "C";

                    String xml = "";
                    DataTable bltx = new DataTable();
                    bltx = aa.GetDataTable("select * from V_HIS_TX where F_blh='" + blh + "'", "bljc");

                    try
                    {

                        if (sqd != "1")
                        {
                            string sex = bljc.Rows[0]["F_XB"].ToString().Trim();
                            if (sex == "��")
                                sex = "M";
                            else
                                if (sex == "Ů")
                                    sex = "F";
                                else
                                    sex = "O";

                            string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
                            if (brlb == "סԺ")
                                brlb = "I";
                            else
                                if (brlb == "����")
                                    brlb = "O";
                                else
                                    brlb = "E";


                            string hy = bljc.Rows[0]["F_HY"].ToString().Trim();
                            if (hy == "�ѻ�")
                                hy = "M";
                            else
                                if (hy == "δ��")
                                    hy = "B";
                                else
                                    if (hy == "���")
                                        hy = "D";
                                    else
                                        if (hy == "ɥż")
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
                                    "<name disc=\"����\">" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</name>" +
                            "<sex disc=\"�Ա�\">" + sex + "</sex>" +
                            "<idno disc=\"���֤����\">" + bljc.Rows[0]["F_SFZH"].ToString().Trim() + "</idno>" +
                            "<birthdate disc=\"��������\"></birthdate>" +
                            "<address disc=\"��ַ\">" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "</address>" +
                            "<phoneno></phoneno>" +
                            "<maritalstatus>" + hy + "</maritalstatus>" +
                            "<cardno disc=\"���￨��\"></cardno>" +
                            "<deptno disc=\"������Һ�\"></deptno>" +
                            "<deptname disc=\"���������\">" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "</deptname>" +
                            "<doctorno disc=\"����ҽ����\"></doctorno>" +
                            "<doctorname disc=\"����ҽ������\">" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</doctorname>" +
                            "<patsource disc=\"������Դ\">" + brlb + "</patsource>" +
                            "<inpatno disc=\"סԺ��\">" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "</inpatno>" +
                            "<outpatno disc=\"�����\">" + bljc.Rows[0]["F_mzh"].ToString().Trim() + "</outpatno>" +
                            "<ward disc=\"����\">" + bljc.Rows[0]["F_bq"].ToString().Trim() + "</ward>" +
                            "<bedno disc=\"����\">" + bljc.Rows[0]["F_ch"].ToString().Trim() + "</bedno>" +
                            "<patid disc=\"����ID\">" +"PS"+ bljc.Rows[0]["F_blh"].ToString().Trim() + "</patid>" +
                            "<appid disc=\"����ID\">" +"PS"+ bljc.Rows[0]["F_BLH"].ToString().Trim() + "</appid>" +
                            "<studyid>" +"PS"+ blh + "</studyid>" +
                            "<clinicdiag disc=\"�ٴ����\">" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</clinicdiag>" +
                            "<clinicdesc disc=\"���Ŀ��\">" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</clinicdesc>" +
                            "<modality disc=\"�������\">PS</modality>" +
                            "<appdatetime disc=\"����ʱ��\">" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</appdatetime>" +
                            "<itemno disc=\"��Ŀ����\"></itemno>" +
                            "<itemname disc=\"��Ŀ����\">" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "</itemname>" +
                            "<price disc=\"�۸�\"></price>" +
                            "<body disc=\"��鲿λ\">" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "</body>" +
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
                                    MessageBox.Show("���뵥Ϊ�գ�����д");
                                log.WriteMyLog("���뵥Ϊ�գ�����д");
                                return;
                            }


                            xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                                     "<operation name =\"sendReportToHis\">" +
                                     "<appid disc=\"����ID\">" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</appid>" +
                                     "<checkitem disc=\"�����Ŀ\">" + bljc.Rows[0]["F_YZXM"].ToString().Trim().Replace('^',',') + "</checkitem>" +
                                     "<checkMethod disc=\"��鷽��\">" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</checkMethod>" +
                                     "<checkDesc disc=\"�������\">" + bljc.Rows[0]["F_RYSJ"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" +bljc.Rows[0]["F_jxsj"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%")+ "</checkDesc>" +
                                     "<checkResult disc=\"�����\">" + bljc.Rows[0]["F_BLZD"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%") + "%Enter%" +bljc.Rows[0]["F_TSJC"].ToString().Trim().Replace('\n', (char)36).Replace('\r', (char)36).Replace("$$", "%Enter%")+ "</checkResult>" +
                                     "<reporting_physician disc=\"����ҽ��\">" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</reporting_physician>" +
                                     "<reporting_datetime disc=\"����ʱ��\">" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</reporting_datetime>" +
                                     "<audit_physician disc=\"���ҽ��\">" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</audit_physician>";
                            if (bgzt == "F")
                            {
                                xml = xml + "<audit_datetime disc=\"���ʱ��\">" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "</audit_datetime>" +
                                      "<reportstatus>F</reportstatus>" + "</operation>";
                            }
                            else
                            {
                                xml = xml + "<audit_datetime disc=\"���ʱ��\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</audit_datetime>" +
                                    "<reportstatus>C</reportstatus>" + "</operation>";
                            }
                                  

                        }

                        if (debug == "1")
                            log.WriteMyLog("��дXML��" + xml);

                    }
                    catch (Exception ee)
                    {
                        if (msg == "1")
                            MessageBox.Show("XML�ַ����쳣" + ee.ToString());
                        log.WriteMyLog("XML�ַ����쳣" + ee.ToString());
                        return;
                    }

                    if (xml.Trim() == "")
                    {
                        if (msg == "1")
                            MessageBox.Show("XMLΪ�գ����ش�");
                        log.WriteMyLog("XMLΪ�գ����ش�");
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
                            MessageBox.Show("����ֵ��"+rtn_msg);
                        if (rtn_msg == "1")
                        {
                            if (bgzt == "F")
                                aa.ExecuteSQL("update T_JCXX  set f_hxbz='2' where F_blh='" + blh + "'");
                        }
                        else
                        {
                            if (msg == "1")
                                MessageBox.Show("��д����ֵ��" + rtn_msg);
                        }
                        }
                        catch (Exception e2)
                        {
                            if (msg == "1")
                            MessageBox.Show("����ش��쳣��" + e2.ToString());
                            log.WriteMyLog("����ش��쳣��" + e2.ToString());
                        }

                    }

                }
                else
                {


                }

            }
        
    }
}
