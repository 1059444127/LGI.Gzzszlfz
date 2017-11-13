
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading;
using ZgqClassPub;
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

            string blbh = blh + bglx + bgxh;
            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            
            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            string isbghj = f.ReadString("zgqjk", "isbghj", "1").Replace("\0", "").Trim();
         
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
                ZgqClass.BGHJ(blh, CZY, "����", "���������,������", "ZGQJK", "");
                aa.ExecuteSQL("update T_JCXX_FS set F_bz='���������,������',F_FSZT='�Ѵ���'  where F_blbh='" + blbh + "' ");
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
            if (brlb == "סԺ")
                brlb = "I";
            else if (brlb == "���")
                brlb = "T";
            else
                brlb = "O";
         
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
            int hcbg = f.ReadInteger("savetohis", "hcbg", 1);
     
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
                                  //+ "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||||||||||||\r"
                                  + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                    + "ORC|SC|" + sqxh + "|||RC|||||" + CZYGH + "^" + CZY + "\r"
                                    + "OBR||" + sqxh + "|" + "BL_"+blbh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;
                        if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
                        {
                            ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                //+ "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"
                                  + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||||||||||||\r"
                                  + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                    + "ORC|SC|" + sqxh + "|||RC|||||" + CZYGH + "^" + CZY + "\r"
                                    + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||" + bgzt_1;
                        }

                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı���״̬��Σ�" + ChangeGmsApplyStatus_Hl7);

                        string rtn_msg2 = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);


                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı���״̬ƽ̨���أ�" + rtn_msg2);

                        if (rtn_msg2.Trim() == "-1")
                        {
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-��д����״̬ʧ�ܣ�" + rtn_msg2);
                            if (isbghj == "1")
                                ZgqClass.BGHJ(blh, CZY, "����", "��д����״̬ʧ��:-1", "ZGQJK", "��д����״̬:" + bgzt);
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
                                    ZgqClass.BGHJ(blh, CZY, "����", "��д����״̬�ɹ�:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:" + bgzt);
                            }
                            else
                            {
                                log.WriteMyLog("��д����״̬ʧ�ܣ�" + r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZgqClass.BGHJ(blh, CZY, "����", "��д����״̬ʧ��:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:" + bgzt);
                            }
                        }
                    }
                    if (debug == "1")
                        log.WriteMyLog("�ش�״̬����");
#endregion
                }
            }



            DataTable dt_bc = new DataTable();
            DataTable dt_bd = new DataTable();
            string bgzt2 = "";
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
            if (bgzt != "ȡ�����" && bglx.ToLower().Trim() != "cg")
                bgzt = bgzt2;

            string pdfname = "";
            string pdfpath = "";
            string filepath = "";
            string ispdf = f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim();

            if (ispdf == "1")
            {
               
                #region  ����pdf
                if (bgzt.Trim() == "�����")
                {
 if (debug == "1")
                    log.WriteMyLog("����pdf��ʼ");
                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    string err_msg = "";

       

                    //����jpg
                    bool isrtn = false;
                    if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
                        isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.JPG, ref err_msg, ref pdfname, ref pdfpath, ref filepath, IP, useName, pwd);
                    else
                        isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.PDF, ref err_msg, ref pdfname, ref pdfpath, ref filepath, IP, useName, pwd);

                    if (isrtn)
                    {
                        pdfname = pdfname.Substring(pdfname.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "his�ӿ�", "�������", "����PDF�ɹ�:" + pdfpath, "ZGQJK", "pdf");
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "' ");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_PDFPATH,F_PDFNAME) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + pdfpath + "','" + pdfname + "')");
                    }
                    else
                    {
                        log.WriteMyLog(blh + "-" + err_msg);
                        ZgqClass.BGHJ(blh, "his�ӿ�", "���PDF", "����PDFʧ��" + err_msg, "ZGQJK", "pdf");
                        if (msg == "1")
                            MessageBox.Show("����ţ�" + blh + "  ����PDFʧ�ܣ���������ˣ�\r\n" + err_msg);
                        return;
                    } 
                    if (debug == "1")
                    log.WriteMyLog("����pdf��jpg����");
                }
                else
                {
                }
                   
              
                # endregion
            }

            //��д���� 

            if (hcbg == 1)
            {
           

                if (bgzt == "�����")
                {
                    if (debug == "1")
                        log.WriteMyLog("�ش����״̬");
                    string txwebpath = ZgqClass.GetSz("savetohis", "txwebpath", @"http://223.220.200.111/pathwebrpt/images").Replace("\0", "").Trim();
                
                    ////��д�������״̬
                    if (bglx != "bc" && bglx != "bd")
                    {  
                        #region ��д���״̬
                        string ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                      + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"
                                      //+ "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||||||||||||\r"
                                      + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                        + "ORC|SC|" + sqxh + "|||RC|||||" + CZYGH + "^" + CZY + "\r"
                                        + "OBR||" + sqxh + "|" + "BL_"+blbh + "|" + YZXM + "|||||||||||||||||||||F";

                        if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
                        {

                            ChangeGmsApplyStatus_Hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
                                //+ "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI~" + ZYH + "^^^^||" + XM + "^||||||||||||\r"
                                      + "PID|||" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "^^^^PI||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||||||||||||\r"
                                      + "PV1||" + brlb + "|||||||||||||||||" + ZYH + "\r"
                                        + "ORC|SC|" + sqxh + "|||RC|||||" + CZYGH + "^" + CZY + "\r"
                                        + "OBR||" + sqxh + "|" + blh + "|" + YZXM + "|||||||||||||||||||||F";

                        }

                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı������״̬��Σ�" + ChangeGmsApplyStatus_Hl7);

                        string rtn_msg_zt = rtn_CallInterface("HL7v2", "ChangeGmsApplyStatus", ChangeGmsApplyStatus_Hl7, "", certificate);

                        if (debug == "1")
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + "-�޸ı������״̬ƽ̨���أ�" + rtn_msg_zt);

                        if (rtn_msg_zt.Trim() == "-1")
                        {

                            if (isbghj == "1")
                                ZgqClass.BGHJ(blh, CZY, "����", "��д�������״̬ʧ��:-1", "ZGQJK", "��д����״̬:�����");
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
                                    ZgqClass.BGHJ(blh, CZY, "����", "��д����״̬�ɹ�:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:�����");

                            }
                            else
                            {

                                if (isbghj == "1")
                                    ZgqClass.BGHJ(blh, CZY, "����", "��д����״̬ʧ��:" + r7.MSA[3].Trim(), "ZGQJK", "��д����״̬:�����");
                                log.WriteMyLog("��д����״̬ʧ�ܣ�" + r7.MSA[3].Trim());
                            }
                        }
                        #endregion
                    }
                        DataTable dt_pdf = new DataTable();
                        dt_pdf = aa.GetDataTable("select *  from T_BG_PDF  where F_BLBH='" + blbh + "'","pdf");
                        if (dt_pdf == null || dt_pdf.Rows.Count <= 0)
                        {
                            if (ispdf == "1")
                           {
                               #region  ����pdf
                               if (bgzt.Trim() == "�����")
                               {

                                   string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                                   string err_msg = "";

                                
                                   //����jpg
                                   bool isrtn = false;
                                   if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
                                       isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.JPG, ref err_msg, ref pdfname, ref pdfpath, ref filepath, IP, useName, pwd);
                                   else
                                       isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.PDF, ref err_msg, ref pdfname, ref pdfpath, ref filepath, IP, useName, pwd);

                                   if (isrtn)
                                   {
                                       pdfname = pdfname.Substring(pdfname.LastIndexOf('\\') + 1);
                                       ZgqClass.BGHJ(blh, "his�ӿ�", "�������", "����PDF�ɹ�:" + pdfpath , "ZGQJK", "pdf");
                                       aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "' ");
                                       aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_PDFPATH,F_PDFNAME) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + pdfpath + "','" + pdfname + "')");
                                   }
                                   else
                                   {
                                       log.WriteMyLog(blh + "-" + err_msg);
                                       ZgqClass.BGHJ(blh, "his�ӿ�", "���PDF", "����PDFʧ��" + err_msg, "ZGQJK", "pdf");
                                       if (msg == "1")
                                           MessageBox.Show("����ţ�" + blh + "  ����PDFʧ�ܣ���������ˣ�\r\n" + err_msg);
                                       return;
                                   }
                               }
                               else
                               {
                               }

                               if (debug == "1")
                                   log.WriteMyLog("����pdf��jpg����");
                               # endregion
                            }
                        }
                        else
                        {
                            pdfpath = dt_pdf.Rows[0]["F_PDFPATH"].ToString().Trim();
                            pdfname = dt_pdf.Rows[0]["F_PDFNAME"].ToString().Trim();
                        }
                        #region �ش�����

                        if (debug == "1")
                            log.WriteMyLog("�ش������ʼ");

                        string bgys = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                        string shys = jcxx.Rows[0]["F_SHYS"].ToString().Trim();
                        
                        string qcys = jcxx.Rows[0]["F_QCYS"].ToString().Trim();
                        string qcysgh = getyhgh(qcys);
                        string qcrq = jcxx.Rows[0]["F_QCrq"].ToString().Trim();
                        string bgrq = DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        string shrq = DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");

                        // �������
                        string Res_char = (jcxx.Rows[0]["F_RYSJ"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_JXSJ"].ToString().Trim());
                        //��Ͻ���	Res_con
                        string Res_con = (jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "\r\n" + jcxx.Rows[0]["F_TSJC"].ToString().Trim());
                        try
                        {
                            qcrq = DateTime.Parse(qcrq).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                        if (bglx == "bc")
                        {
                            bgys = dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim();
                            shys = dt_bc.Rows[0]["F_BC_SHYS"].ToString().Trim();
                            Res_char = (dt_bc.Rows[0]["F_BC_JXSJ"].ToString().Trim());
                            Res_con = (dt_bc.Rows[0]["F_BCZD"].ToString().Trim() + "\r\n" + dt_bc.Rows[0]["F_BC_TSJC"].ToString().Trim());
                            bgrq = DateTime.Parse(dt_bc.Rows[0]["F_bc_bgrq"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                             shrq = DateTime.Parse(dt_bc.Rows[0]["F_bc_SPARE5"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        }
                        if (bglx == "bd")
                        {
                             bgys = dt_bd.Rows[0]["F_BD_BGYS"].ToString().Trim();
                             shys = dt_bd.Rows[0]["F_BD_SHYS"].ToString().Trim();
                             Res_char = "";
                             Res_con = (dt_bd.Rows[0]["F_BDZD"].ToString().Trim());
                             bgrq = DateTime.Parse(dt_bc.Rows[0]["F_bd_bgrq"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                             shrq = DateTime.Parse(dt_bc.Rows[0]["F_bd_bgrq"].ToString().Trim()).ToString("yyyyMMddHHmmss");
                        }
                        string bgysgh = getyhgh(bgys);
                        string shysgh = getyhgh(shys);

                        string hxbj = jcxx.Rows[0]["F_HXBJ"].ToString().Trim();

                        string zt2 = "F";
                        if (hxbj == "1")
                            zt2 = "C";

                        string xb = jcxx.Rows[0]["F_XB"].ToString().Trim();
                        if (xb == "Ů") xb = "F";
                        else if (xb.Trim() == "��") xb = "M";
                        else xb = "U";
                    ////������ң���鷽������鲿λ���쳣��ǣ�������ң��Ǽ�ʱ�䣬�Ǽ��ˣ����ʱ�䣬����ˣ��Ա�
                     string  blk= jcxx.Rows[0]["F_BLK"].ToString().Trim();
                     if (blk.Contains("TCT") || blk == "LCT" || blk == "Һ��ϸ��")
                     {
                          DataTable TJ_bljc = new DataTable();
                            TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                            if (TJ_bljc.Rows.Count > 0)
                            {   
                                    Res_char = Res_char + "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";
                                    Res_char = Res_char + "     " + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                                        + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";
                                    Res_char = Res_char + "��ԭ΢���" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
                                        + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";
                                    Res_char = Res_char + "��֢�̶ȣ�" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

                                    ///////////���/////////////////////////
                                    Res_con = "TBS��ϣ�" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                                    if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                        Res_con = Res_con + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                            }
                     }
                     string SendGmsReport_hl7 = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORU^R01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
     + "PID|||" + brbh + "^^^^PI||" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "^||^" + jcxx.Rows[0]["F_nl"].ToString().Trim() + "|" + xb + "|||||||" + "\r"
     + "PV1||" + brlb + "|" + SJKS + "^^" + CH + "||||^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim() + "||||||||||||" + ZYH + "|||||||||||||||||||||||||" + "\r"
     + "OBR||" + sqxh + "|" + "BL_" + blbh + "|" + YZXM + "||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|" + qcrq + "|||" + qcysgh + "^" + qcys + "||||" + DateTime.Parse(jcxx.Rows[0]["F_sdrq"].ToString().Trim()).ToString("yyyyMMddHHmmss") + "|" + jcxx.Rows[0]["F_BBMC"].ToString().Trim() + "|^" + jcxx.Rows[0]["F_SJYS"].ToString().Trim() + "||||||" + bgrq + "||||||||||" + bgysgh + "&" + bgys + "^^" + "\r"
     + "NTE|1||" + filepath + "|Z-RP" + "\r"
     + "OBX|1|FT|^��������||" + Res_char.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + "\r"
     + "OBX|2|FT| ^�������||" + Res_con.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\\.br\\.") + "|||N|||" + zt2 + "|||" + shrq + "||" + shysgh + "^" + shys + "|^" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + "\r";


                     if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "���")
                        {
                            SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|1|pdfname|"+"BL_" + blbh + "|" + pdfpath.Replace("ftp", "http").Trim() + "||" + "\r";
                        }
                        else
                        {
                            DataTable dt_tx = aa.GetDataTable("select * from V_DYTX where F_blh='" + blh + "'", "tx");
                            if (dt_tx.Rows.Count > 0)
                            {
                                for (int x = 0; x < dt_tx.Rows.Count; x++)
                                {
                                    string txlj = txwebpath + "/" + jcxx.Rows[0]["F_TXML"].ToString().Trim() + "/" + dt_tx.Rows[x]["F_TXM"].ToString();
                                    SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|" + (x + 1).ToString() + "|" + dt_tx.Rows[x]["F_ID"].ToString() + "|" + "BL_" + blbh + "|" + txlj + "||1" + "\r";
                                }
                            }
                            else
                            {
                                SendGmsReport_hl7 = SendGmsReport_hl7 + "ZIM|1|1|" + "BL_" + blbh + "|||" + "\r";
                            }
                        }

                        if (debug == "1")
                            log.WriteMyLog("������˻�д�����" + SendGmsReport_hl7);
                        string rtn_msg2 = rtn_CallInterface("HL7v2", "SendGmsReport", SendGmsReport_hl7, "", certificate);

                        string iscfsc = ZgqClass.GetSz("zgqjk", "iscfsc", "0").Replace("\0", "").Trim();
                        if (rtn_msg2.Contains("error"))
                        {
                            if (msg == "1")
                                MessageBox.Show("������˻�д�������" + rtn_msg2);
                            log.WriteMyLog("������˻�д�������" + rtn_msg2);
                            if (isbghj == "1")
                                ZgqClass.BGHJ(blh, "�ϴ�ƽ̨", "���", "������˻�д�������:" + rtn_msg2, "ZGQJK", "������˻�д���");
                            
                            if (iscfsc=="1")
                            aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='" + rtn_msg2 + "'  where F_blbh='" + blbh + "' ");
                            else
                            aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='" + rtn_msg2 + "',F_FSZT='�Ѵ���'  where F_blbh='" + blbh + "' ");

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
                                    ZgqClass.BGHJ(blh, "�ϴ�ƽ̨", "���", "������˻�д����ɹ�:" + r7.MSA[3].Trim(), "ZGQJK", "������˻�д���");
                            }
                            else
                            {
                                if (msg == "1")
                                    MessageBox.Show(r7.MSA[3].Trim());
                                log.WriteMyLog("������˻�д���ʧ�ܣ�" + r7.MSA[3].Trim());
                                if (isbghj == "1")
                                    ZgqClass.BGHJ(blh, "�ϴ�ƽ̨", "���", "������˻�д���ʧ��:" + r7.MSA[3].Trim(), "ZGQJK", "������˻�д���");

                                if (iscfsc == "1")
                                aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='������˻�д���ʧ��:" + r7.MSA[3].Trim() + "'  where F_blbh='" + blbh + "' ");
                                else
                                    aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='������˻�д���ʧ��:" + r7.MSA[3].Trim() + "',,F_FSZT='�Ѵ���'  where F_blbh='" + blbh + "' ");

                                return;
                            }
                        }
                        if (debug == "1")
                            log.WriteMyLog("�ش��������");

                        #endregion
                      
                        aa.ExecuteSQL("update T_JCXX_FS set F_errmsg='',F_FSZT='�Ѵ���'  where F_blbh='" + blbh + "'");
                        aa.ExecuteSQL("update T_JCXX set F_HXBJ='1' where F_blh='" + blh + "'");
                        return;

                    }
                //ȡ����ˣ�ɾ��pdf
                    #region
                    if (bgzt == "ȡ�����")
                {
                    DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blbh='" + blbh + "' ", "dt2");
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
                                Thread.Sleep(500);

                                if (File.Exists(dt2.Rows[0]["F_PDFPATH"].ToString()))
                                {
                                    File.Delete(dt2.Rows[0]["F_PDFPATH"].ToString());
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
                            if (File.Exists(dt2.Rows[0]["F_PDFPATH"].ToString()))
                                File.Delete(dt2.Rows[0]["F_PDFPATH"].ToString());
                        }
                        //�жϹ������Ƿ���ڸ�pdf�ļ�
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "' ");
                    }
                }
                    #endregion
            }
                
                return;
           
        }


        public bool MD_PDF_JPG(string blh, string bglx, string bgxh, string ML, ZgqPDFJPG.Type jpgpdf, ref string err_msg, ref string fileName, ref string pdfpath, ref string filePath, string IP, string useName, string pwd)
        {


            string message = ""; string jpgname = "";
            ZgqPDFJPG zgq = new ZgqPDFJPG();
           
            bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf, ref message, ref jpgname);
          
            string xy = f.ReadString("savetohis", "sctxfs", "1");
            filePath="";
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
                        ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 1, ref filePath);
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
                
                    ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 1, ref filePath);
                }
                if (ssa)
                {
                    log.WriteMyLog(filePath);
                        ssa = false;
                        ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 3, ref pdfpath);
                        if (ssa)
                            log.WriteMyLog("����������ϴ�PDF�ɹ���" + pdfpath);
                        else
                            log.WriteMyLog("����������ϴ�PDFʧ��" + message);

                    //jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                    fileName = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                
                    err_msg = "";
                    zgq.DelTempFile(blh);
                    return true;

                }
                else
                {
                    log.WriteMyLog("�ϴ�PDF��ƽ̨��ʧ��" + filePath);
                    err_msg = message;
                    zgq.DelTempFile(blh);
                    return false;
                }
            }
            else
            {
                zgq.DelTempFile(blh);
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
