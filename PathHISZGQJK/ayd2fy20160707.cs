
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using readini;
using System.Data.SqlClient;
using HL7;
using LG_ZGQ;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace PathHISZGQJK
{
    //��ҽ��2��Ժ webservices+hl7
    class ayd2fy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string msg = ""; string debug = "";
        public void pathtohis(string blh, string bglx, string bgxh, string msg1, string debug1, string[] cslb)
        {

            if (bglx == "")
                bglx = "cg";
            if (bgxh == "")
                bgxh = "0";


            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            
            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            string isbghj = f.ReadString("savetohis", "isbghj", "1").Replace("\0", "").Trim();
         
            string xtdm = f.ReadString("savetohis", "xtdm", "2060000");

            string certificate = f.ReadString("savetohis", "certificate", "ZmmJ9RMCKAUxFsiwl/08iiA3J17G0OpI");
           
            msg=msg1;

            string IP = f.ReadString("savetohis", "IP", "223.220.200.7");
            string toPDFPath = f.ReadString("savetohis", "toPDFPath", "");
            string useName = f.ReadString("savetohis", "useName", "");
            string pwd = f.ReadString("savetohis", "pwd", "");
            string tjtxpath = toPDFPath;

            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                     qxsh = "1";//ȡ����˶���
                   
                if (cslb[3].ToLower() == "new")
                    xdj = "1";
            }

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
                MessageBox.Show("�������ݿ����������⣡");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                return;
            }
            if (jcxx.Rows[0]["F_SQXH"].ToString().Trim()=="")
            {
                log.WriteMyLog("���������,������");
                ZGQClass.BGHJ(blh, CZY, "����", "���������,������", "ZGQJK", "");
                aa.ExecuteSQL("update T_JCXX_FS set F_bz='���������,������',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                       
                return;
            }

            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();

            if (qxsh == "1")
            {
                bgzt = "ȡ�����";
            }


        
            string brbh = jcxx.Rows[0]["F_BRBH"].ToString().Trim();
            string brlb = jcxx.Rows[0]["F_brlb"].ToString().Trim();
            string sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim();
            if (brlb == "סԺ") brlb = "I";
            else brlb = "O";
         
            string ZYH = jcxx.Rows[0]["F_MZH"].ToString().Trim();
            if (brlb == "I")
                ZYH = jcxx.Rows[0]["F_ZYH"].ToString().Trim();

            string SFZH = jcxx.Rows[0]["F_SFZH"].ToString().Trim();
            string XM = jcxx.Rows[0]["F_XM"].ToString().Trim();
            string SJKS = jcxx.Rows[0]["F_BQ"].ToString().Trim();
            string CH = jcxx.Rows[0]["F_CH"].ToString().Trim();
            string YZXM = jcxx.Rows[0]["F_YZXM"].ToString().Trim();

            //��д״̬   �ͻ�����
            int hczt = f.ReadInteger("savetohis", "hczt", 1);
     
            if (hczt == 1)
            {

                if (bglx != "bc" && bglx != "bd")
                {
                    if (debug == "1")
                        log.WriteMyLog("�ش�״̬");

                    #region  ��״̬(���������)
                    if (bgzt == "�ѵǼ�" || bgzt == "��ȡ��" || bgzt == "��д����" || bgzt == "��������" || bgzt == "ȡ�����")
                    {

                        string bgzt_1 = "";
                        if (bgzt == "�ѵǼ�" || bgzt == "��ȡ��")
                            bgzt_1 = "S";

                        if (bgzt == "��д����" || bgzt == "��������")
                            bgzt_1 = "R";

                        if (bgzt == "�����")
                            bgzt_1 = "F";

                        if (bgzt == "��д����" && jcxx.Rows[0]["F_HXBJ"].ToString().Trim() == "1")
                            bgzt_1 = "C";

                        if (bgzt == "ȡ�����")
                            bgzt_1 = "C";


                        if (bgzt_1 == "")
                        {
                            log.WriteMyLog("bgzt_1״̬Ϊ�գ�����д��");
                            return;
                        }

                        string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                  + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"

                                  + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                    + "ORC|SC|" + sqxh + "|||RC|||||" + CZYGH + "^" + CZY + "\r"
                                    + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;

                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı���״̬��Σ�" + ChangeGmsApplyStatus_Hl7);

                        string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);


                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı���״̬ƽ̨���أ�" + rtn_msg2);

                        if (rtn_msg2.Trim() == "-1")
                        {
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-��д����״̬ʧ�ܣ�" + rtn_msg2);
                            if (isbghj == "1")
                                ZGQClass.BGHJ(blh, CZY, "����", "��д����״̬ʧ��:-1", "ZGQJK", "��д����״̬:" + bgzt);
                            return;
                        }
                        else
                        {
                            readhl7_fjfy r7 = new readhl7_fjfy();
                            int xy = 0;
                            r7.Adt01(rtn_msg2, ref xy);
                            if (r7.MSA[1].Trim() == "AA")
                            {
                                if (debug == "1")
                                    log.WriteMyLog(r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZGQClass.BGHJ(blh, CZY, "����", "��д����״̬�ɹ�:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:" + bgzt);
                            }
                            else
                            {
                                log.WriteMyLog("��д����״̬ʧ�ܣ�" + r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZGQClass.BGHJ(blh, CZY, "����", "��д����״̬ʧ��:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:" + bgzt);
                            }
                        }




                    }
                    if (debug == "1")
                        log.WriteMyLog("�ش�״̬����");
#endregion
                }
                else
                {
                    if (debug == "1")
                        log.WriteMyLog("��������bc��bd�����ش�״̬");
                }


               
            }

            //��д����   plsc��
            int plsc = f.ReadInteger("fsjk", "plsc", 1);
          
            if (plsc == 1)
            {
                string bz = "";
                if (bgzt == "�����")
                {
                    if (debug == "1")
                        log.WriteMyLog("�ش����״̬");
                    string txwebpath = ZGQClass.getSZ_String("savetohis", "txwebpath", @"http://223.220.200.11/pathimages").Replace("\0", "").Trim();

                    //�ϴ����
                    if (bglx != "bc" && bglx != "bd")
                    {
                       
                        #region ��д���״̬
                        string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                      + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"

                                      + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                        + "ORC|SC|" + sqxh + "|||RC|||||" + CZYGH + "^" + CZY + "\r"
                                        + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||F";

                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı������״̬��Σ�" + ChangeGmsApplyStatus_Hl7);

                        string rtn_msg_zt = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);

                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı������״̬ƽ̨���أ�" + rtn_msg_zt);

                        if (rtn_msg_zt.Trim() == "-1")
                        {

                            if (isbghj == "1")
                                ZGQClass.BGHJ(blh, CZY, "����", "��д�������״̬ʧ��:-1", "ZGQJK", "��д����״̬:�����");
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-��д����״̬ʧ�ܣ�" + rtn_msg_zt);
                            return;
                        }
                        else
                        {
                            readhl7_fjfy r7 = new readhl7_fjfy();
                            int xy = 0;
                            r7.Adt01(rtn_msg_zt, ref xy);
                            if (r7.MSA[1].Trim() == "AA")
                            {
                                if (debug == "1")
                                    log.WriteMyLog(r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZGQClass.BGHJ(blh, CZY, "����", "��д����״̬�ɹ�:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:�����");


                            }
                            else
                            {

                                if (isbghj == "1")
                                    ZGQClass.BGHJ(blh, CZY, "����", "��д����״̬ʧ��:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:�����");
                                log.WriteMyLog("��д����״̬ʧ�ܣ�" + r7.MSA[3].Trim());
                            }
                        }
                    }
                    if (debug == "1")
                        log.WriteMyLog("�ش��������");
                    #endregion

                        string jpgname = "";
                        string jpgpath = "";
                        string ispdf = f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim();
                   // string txwebpath = f.ReadString("savetohis", "txwebpath", "").Replace("\0", "").Trim();
                   
                    
                        if (ispdf == "1")
                        {
                            if (debug == "1")
                                log.WriteMyLog("����pdf��ʼ");

                            #region  ����pdf
                            string bgzt2 = "";
                            try
                            {
                                if (bglx.ToLower().Trim() == "bd")
                                {
                                    DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                                    bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                                }
                                if (bglx.ToLower().Trim() == "bc")
                                {
                                    DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                                    bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                                }
                                if (bglx.ToLower().Trim() == "cg")
                                {
                                    // DataTable jcxx2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                                    bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                                }
                            }
                            catch
                            {
                            }

                            if (bgzt2.Trim() == "")
                            {
                                log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);
                            }

                            if (bgzt2.Trim() == "�����" && bgzt != "ȡ�����")
                            {

                                string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                                string err_msg = "";
                              
                               
                                ZGQClass zgq = new ZGQClass();

                                if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
                                {
                                    //����jpg
                                    bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZGQClass.type.JPG, ref err_msg, ref jpgname, ref jpgpath, IP, useName, pwd);
                                    if (isrtn)
                                    {

                                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                        ZGQClass.BGHJ(blh, "his�ӿ�", "�������", "����jpg�ɹ�:" + jpgpath + "\\" + jpgname, "ZGQJK", "pdf");
                                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                                    }
                                    else
                                    {
                                        log.WriteMyLog(blh + "-" + err_msg);
                                        ZGQClass.BGHJ(blh, "his�ӿ�", "���jpg", "����jpgʧ��" + err_msg, "ZGQJK", "pdf");

                                        if (msg == "1")
                                            MessageBox.Show("����ţ�" + blh + "  ����jpgʧ�ܣ���������ˣ�\r\n" + err_msg);
                                        return;
                                    }
                                }
                                else
                                {
                                    //����pdf
                                    bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZGQClass.type.PDF, ref err_msg, ref jpgname, ref jpgpath, IP, useName, pwd);
                                    if (isrtn)
                                    {

                                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                        ZGQClass.BGHJ(blh, "his�ӿ�", "�������", "����pdf�ɹ�:" + jpgpath + "\\" + jpgname, "ZGQJK", "pdf");

                                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                                    }
                                    else
                                    {
                                        log.WriteMyLog(blh + "-" + err_msg);
                                        ZGQClass.BGHJ(blh, "his�ӿ�", "���PDF", "����pdfʧ��" + err_msg, "ZGQJK", "pdf");

                                        if (msg == "1")
                                            MessageBox.Show("����ţ�" + blh + "  ����pdfʧ�ܣ���������ˣ�\r\n" + err_msg);
                                        return;
                                    }
                                }
                            }
                            if (debug == "1")
                                log.WriteMyLog("����pdf��jpg����");
                            # endregion
                        }


                        #region �ش�����

                        if (debug == "1")
                            log.WriteMyLog("�ش������ʼ");

                        string pdflj = tjtxpath + "\\" + jpgpath + "\\" + jpgname;

                        string bgys = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                        string bgysgh = getyhgh(bgys);

                        string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                        string shysgh = getyhgh(shys);

                        string qcys = jcxx.Rows[0]["F_QCYS"].ToString().Trim();
                        string qcysgh = getyhgh(qcys);

                        string qcrq = jcxx.Rows[0]["F_QCrq"].ToString().Trim();
                        try
                        {
                            qcrq = DateTime.Parse(qcrq).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }

                        string hxbj = jcxx.Rows[0]["F_HXBJ"].ToString().Trim();

                        string zt2 = "F";
                        if (hxbj == "1")
                            zt2 = "C";


                        string xb = jcxx.Rows[0]["F_XB"].ToString().Trim();
                        if (xb == "Ů") xb = "M";
                        else if (xb.Trim() == "��") xb = "F";
                        else xb = "U";
                    ////������ң���鷽������鲿λ���쳣��ǣ�������ң��Ǽ�ʱ�䣬�Ǽ��ˣ����ʱ�䣬����ˣ��Ա�


                        string bgrq = DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        string SendGmsReport_hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORU^R01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
        + "PID|||" + brbh + "^^^^PI||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||^" + jcxx.Rows[0]["F_nl"].ToString().Trim() + "|" + xb + "|||||||" + "\r"
        + "PV1||" + brlb + "|" + SJKS + "^^" + CH + "||||^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim() + "||||||||||||" + ZYH + "|||||||||||||||||||||||||" + "\r"
        + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|" + qcrq + "|||" + qcysgh + "^" + qcys + "||||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|" + jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "|^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim() + "||||||" + bgrq + "||||||||||" + bgysgh + "&" + bgys + "^^" + "\r"
        + "NTE|1||" + pdflj + "|Z-RP" + "\r"
        + "OBX|1|FT|^��������||" + (jcxx.Rows[0]["F_RYSJ"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_JXSJ"].ToString().Trim()).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + bgrq + "||" + shysgh + "^" + shys + "|^" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + "\r"
        + "OBX|2|FT| ^�������||" + (jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_TSJC"].ToString().Trim()).Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + bgrq + "||" + shysgh + "^" + shys + "|^" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + "\r";

                        DataTable dt_tx = aa.GetDataTable("select * from V_DYTX where F_blh='" + blh + "'", "tx");
                        if (dt_tx.Rows.Count > 0)
                        {

                            for (int x = 0; x < dt_tx.Rows.Count; x++)
                            {
                                
                                string txlj = txwebpath + "/" + jcxx.Rows[0]["F_TXML"].ToString().Trim() + "/" + dt_tx.Rows[x]["F_TXM"].ToString();
                                SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|" + (x + 1).ToString() + "|" + dt_tx.Rows[x]["F_ID"].ToString() + "|" + blh + "|" + txlj + "||1" + "\r";
                            }
                        }
                        else
                        {
                            SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|1|1|" + blh + "|||" + "\r";
                        }

                        if (debug == "1")
                            log.WriteMyLog("������˻�д�����" + SendGmsReport_hl7);



                        string rtn_msg2 = rtn_CallInterface("HL7v2", "SendGmsReport", SendGmsReport_hl7, "", certificate);

                        if (rtn_msg2.Contains("error"))
                        {
                            if (msg == "1")
                                MessageBox.Show("������˻�д�������" + rtn_msg2);
                            log.WriteMyLog("������˻�д�������" + rtn_msg2);
                            if (isbghj == "1")
                                ZGQClass.BGHJ(blh, "�����ϴ�", "���", "������˻�д�������:" + rtn_msg2, "ZGQJK", "������˻�д���");
                            aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='" + rtn_msg2 + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                            return;
                        }
                        else
                        {
                            readhl7_fjfy r7 = new readhl7_fjfy();
                            int xy = 0;
                            r7.Adt01(rtn_msg2, ref xy);
                            if (r7.MSA[1].Trim() == "AA")
                            {
                                if (debug == "1")
                                    log.WriteMyLog(r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZGQClass.BGHJ(blh, "�����ϴ�", "���", "������˻�д����ɹ�:" + r7.MSA[3].Trim(), "ZGQJK", "������˻�д���");

                            }
                            else
                            {
                                if (msg == "1")
                                    MessageBox.Show(r7.MSA[3].Trim());
                                log.WriteMyLog("������˻�д���ʧ�ܣ�" + r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZGQClass.BGHJ(blh, "�����ϴ�", "���", "������˻�д���ʧ��:" + r7.MSA[3].Trim(), "ZGQJK", "������˻�д���");
                                aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='������˻�д���ʧ��:" + r7.MSA[3].Trim() + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                                return;
                            }

                        }
                        if (debug == "1")
                            log.WriteMyLog("�ش��������");

                        #endregion
                      
                        aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                        aa.ExecuteSQL("update T_JCXX set F_HXBJ='1' where F_blh='" + blh + "'");
                        return;

                    }
                //ȡ����ˣ�ɾ��pdf
                    #region
                    if (bgzt == "ȡ�����")
                {
                    DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                    if (dt2.Rows.Count > 0)
                    {
                        //������

                        if (useName.Trim() != "")
                        {
                            Process pro = new Process();
                            try
                            {
                                pro.StartInfo.FileName = "cmd.exe";
                                pro.StartInfo.UseShellExecute = false;
                                pro.StartInfo.RedirectStandardInput = true;
                                pro.StartInfo.RedirectStandardOutput = true;
                                pro.StartInfo.RedirectStandardError = true;
                                pro.StartInfo.CreateNoWindow = true;
                                pro.Start();
                                pro.StandardInput.WriteLine("net use  \\\\" + IP + "\\ipc$ " + pwd + " /user:" + useName + "");
                                Thread.Sleep(1000);

                                if (File.Exists(tjtxpath + "\\" + dt2.Rows[0]["F_ML"] + "\\" + dt2.Rows[0]["F_FILENAME"]))
                                {

                                    //ɾ�������ϵ�pdf�ļ�
                                    File.Delete(tjtxpath + "\\" + dt2.Rows[0]["F_ML"] + "\\" + dt2.Rows[0]["F_FILENAME"]);
                                }
                            }
                            catch (Exception ee)
                            {

                            }
                            finally
                            {

                                pro.StandardInput.WriteLine("net use  \\\\" + IP + "\\ipc$ /del");
                            }
                        }
                        else
                        {
                            if (File.Exists(tjtxpath + "\\" + dt2.Rows[0]["F_ML"] + "\\" + dt2.Rows[0]["F_FILENAME"]))
                            {

                                //ɾ�������ϵ�pdf�ļ�
                                File.Delete(tjtxpath + "\\" + dt2.Rows[0]["F_ML"] + "\\" + dt2.Rows[0]["F_FILENAME"]);
                            }
                        }
                        //�жϹ������Ƿ���ڸ�pdf�ļ�
                        
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                    }
                }
                    #endregion
            }
                
                return;
           
        }


        public bool MD_PDF_JPG(string blh, string bglx, string bgxh, string ML, ZGQClass.type jpgpdf, ref string err_msg, ref string fileName, ref string fielPath, string IP, string useName, string pwd)
        {


            string message = ""; string jpgname = "";
            ZGQClass zgq = new ZGQClass();
           
            bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf, ref message, ref jpgname);
          
            string xy = f.ReadString("savetohis", "sctxfs", "1");

            if (isrtn)
            {
                bool ssa = false;
                if (useName.Trim() != "")
                {
                    //������
                    Process pro = new Process();
                  
                    try
                    {
                        pro.StartInfo.FileName = "cmd.exe";
                        pro.StartInfo.UseShellExecute = false;
                        pro.StartInfo.RedirectStandardInput = true;
                        pro.StartInfo.RedirectStandardOutput = true;
                        pro.StartInfo.RedirectStandardError = true;
                        pro.StartInfo.CreateNoWindow = true;
                        pro.Start();
                        pro.StandardInput.WriteLine("net use  \\\\" + IP + "\\ipc$ " + pwd + " /user:" + useName + "");
                        Thread.Sleep(1000);

                        ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));

                    }
                    catch (Exception ee)
                    {
                        message = ee.Message;
                        ssa = false;
                    }
                    finally
                    {

                        pro.StandardInput.WriteLine("net use  \\\\" + IP + "\\ipc$ /del");
                    }

                }
                else
                {
                    ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                }
                if (ssa == true)
                {
                    //jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                    fileName = jpgname;
                    fielPath = ML + "\\" + blh;
                    err_msg = "";
                    zgq.DeleteTempFile(blh);
                    return true;

                }
                else
                {
                    err_msg = message;
                    zgq.DeleteTempFile(blh);
                    return false;
                }
            }
            else
            {
                zgq.DeleteTempFile(blh);
                err_msg = message;
                return false;
            }
        }

        public  string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string certificate)
        {
            string WebUrl = f.ReadString("savetohis", "WebUrl", "").Replace("\0", "").Trim();
            //����ƽ̨��ַ
         
            ayd2fyweb.WSInterface  wsif = new  ayd2fyweb.WSInterface() ;
            if (WebUrl.Trim() != "")
                wsif.Url = WebUrl;

            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";
            try
            {
                return wsif.CallInterface(msgHeader, msgBody);
            }
            catch (Exception ee)
            {
              
                if (msg == "1")
                 MessageBox.Show("����webservice�쳣��" + ee.Message.ToString());
                log.WriteMyLog("����rtn_CallInterface�����쳣��" + ee.Message.ToString());
                return "-1";
            }
        }

        public string getyhgh(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "000";
            }
            catch
            {
                return "000";
            }
        }

    }
}
