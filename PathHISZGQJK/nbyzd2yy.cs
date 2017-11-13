using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;


using System.Net;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class nbyzd2yy
    //----------����۴�ݵ�2����ҽԺ----------------------
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {
            string pathweburl = f.ReadString("savetohis", "webservicesurl", "");
            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");

            string patid = "";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
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
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("��������ţ����ݺţ���������");
                return;
            }

        
                if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "�ѵǼ�" || bljc.Rows[0]["F_bgzt"].ToString().Trim() == "��ȡ��")
                {   
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                    {
                    patid = bljc.Rows[0]["F_zyh"].ToString().Trim();

                    if (MessageBox.Show("�Ƿ�ȷ���շ�", "ȷ���շ�", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        //------------------�շ�ȷ��-----------------------
                        string msgSF = "";
                        try
                        {
                            nbyzd2yyWEB.PisServiceLJ nbyz2yweb = new PathHISZGQJK.nbyzd2yyWEB.PisServiceLJ();
                            if (pathweburl != "") nbyz2yweb.Url = pathweburl;
                            msgSF = nbyz2yweb.AddFee(bljc.Rows[0]["F_sqxh"].ToString().Trim(), decimal.Parse("2009"), "2");
                        }
                        catch 
                        {
                            MessageBox.Show("�շ�ȷ��ʧ�ܣ�");
                        }

                        if (msgSF == "0") MessageBox.Show("�շ�ȷ�ϳɹ���");
                    }
                  }

                        //---------------------�жϴ˲����Ƿ��ѵǼ�-------------------------------
                        DataTable brxx_1 = new DataTable();
                        brxx_1 = aa.GetDataTable("select F_BLH,F_XM,F_SQXH from T_jcxx where F_SQXH='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'", "blxx");
                        if (brxx_1.Rows.Count > 1)
                            MessageBox.Show("�˲���" + brxx_1.Rows[0]["F_xm"].ToString() + "�������" + brxx_1.Rows[0]["F_sqxh"].ToString() +"�������"+brxx_1.Rows[0]["F_blh"].ToString() +"   �ѵǼǹ�����鿴�Ƿ��ظ��Ǽ�!!!");
                   }
            
            //if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "����") patid = bljc.Rows[0]["F_mzh"].ToString().Trim();

            //if (patid == "")
            //{
            //     LGZGQClass.log.WriteMyLog("��סԺ���ˣ�������");
            //    return;

            //}

            //�ش�xml
            //DataTable jcxm = aa.GetDataTable("select * from T_whtjyy_jcxm where CheckFlow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'", "jcxm");

            string inxml = "";
            inxml = inxml + "<?xml version='1.0' encoding='GB2312'?>";
            inxml = inxml + "<REPORTINFO>";
            inxml = inxml + "<ITEM>";
            inxml = inxml + "<SQDBH>" + decimal.Parse(bljc.Rows[0]["F_sqxh"].ToString().Trim()) + "</SQDBH>";
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

            if (zt == "�����") zt = "13";
            else
            {
                if (zt == "��д����") zt = "11";
                else zt = "7";
            }
            inxml = inxml + "<ZT>" + zt + "</ZT>";
            string jsybm = getymh(bljc.Rows[0]["F_jsy"].ToString().Trim());
            inxml = inxml + "<JSRY>" + jsybm + "</JSRY>";

            inxml = inxml + "<JSSJ>" + DateTime.Parse(bljc.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHMMss") + "</JSSJ>";
            string bgys = getymh(bljc.Rows[0]["F_bgys"].ToString().Trim());
            inxml = inxml + "<BGRY>" + bgys + "</BGRY>";
            string bgsj = bljc.Rows[0]["F_bgrq"].ToString().Trim();

            if (bgsj != "") bgsj = DateTime.Parse(bgsj).ToString("yyyyMMddHHMMss");

            inxml = inxml + "<BGSJ>" + bgsj + "</BGSJ>";
            string shys = getymh(bljc.Rows[0]["F_shys"].ToString().Trim());
            inxml = inxml + "<SHRY>" + shys + "</SHRY>";
            string shsj = bljc.Rows[0]["F_SPARE5"].ToString().Trim();
            if (shsj != "") shsj = DateTime.Parse(shsj).ToString("yyyyMMddHHMMss");
            inxml = inxml + "<SHSJ>" + shsj + "</SHSJ>";
            inxml = inxml + "<CXRY>" + "" + "</CXRY>";
            inxml = inxml + "<CXSJ>" + "" + "</CXSJ>";
            inxml = inxml + "<JCSJ>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</JCSJ>";
            string JCJL = "";
            if (zt == "13")
                JCJL = bljc.Rows[0]["F_blzd"].ToString().Trim();
            inxml = inxml + "<JCJL>" + JCJL + "</JCJL>";
            string f0brbh = bljc.Rows[0]["F_brbh"].ToString().Trim();
            inxml = inxml + "<WEBURL>http://192.168.10.201/pathwebrpt/index_p.asp?sick_id=" + f0brbh + "</WEBURL>";
            inxml = inxml + "<JCH>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</JCH>";
            inxml = inxml + "</ITEM>";
            inxml = inxml + "</REPORTINFO>";

            if (msg == "1")
            {
                log.WriteMyLog("�ش���xml���ݣ�" + inxml);
                MessageBox.Show(inxml);
            }
            string sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();

            string outxml = "";
            try
            {

                nbyzd2yyWEB.PisServiceLJ nbyz2yweb = new PathHISZGQJK.nbyzd2yyWEB.PisServiceLJ();
                if (pathweburl != "") nbyz2yweb.Url = pathweburl;
                outxml = nbyz2yweb.SetPISReportInfo(decimal.Parse(sqxh), inxml);

            }
            catch (Exception ee)
            {
                if (msg == "1")
                    MessageBox.Show("�ش�ʧ�ܣ�����HIS�ӿڳ���" + ee.ToString());
                log.WriteMyLog("�ش�ʧ�ܣ�����HIS�ӿڳ���" + ee.ToString());

                return;
            }

            if (outxml == "")
            {
                if (msg == "1")
                    MessageBox.Show("�ش��ɹ���");
            }
            else
            {
                log.WriteMyLog("�ش�ʧ�ܣ�ԭ��" + outxml);
                if (msg == "1")
                    MessageBox.Show("�ش�����ʧ�ܣ�" + outxml);
            }
        }

        public string getymh(string yhmc)//ͨ��ҽ������ ��ȡҽ������
        {
            if (yhmc != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select F_yhm from T_yh where F_yhmc='" + yhmc + "'", "blxx");

                    try
                    {
                     decimal  xx=  decimal.Parse(bljc.Rows[0]["F_YHM"].ToString().Trim());
                     return xx.ToString();
                    }
                    catch
                    {
                        return "";
                    }
                     
                }
                catch (Exception ee)
                {
                    log.WriteMyLog("ת��ҽ�����ų���ԭ��" + ee.ToString());
                    return "";
                }
            } return "";

        }

    }
}
