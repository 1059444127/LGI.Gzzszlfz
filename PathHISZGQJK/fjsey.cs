using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using PathHISZGQJK;
using ZgqClassPub;


namespace PathHISZGQJK
{
    //����ʡ�ڶ�ҽԺ
    class fjsey
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
      //  string msg = ""; string debug = "";
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {

            string jkmsg = f.ReadString("jkmsg", "jkmsg", "0").Replace("\0", "");
            f.WriteInteger("jkmsg", "jkmsg", 0);

            bglx = bglx.ToLower();
            if (bglx.ToLower().Trim() == "bd")
                return;




             debug = f.ReadString("savetohis", "debug", "0");
             msg = f.ReadString("savetohis", "msg", "0");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(blh+","+ex.Message);
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show(blh + ",�������ݿ����������⣡");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show(blh+",������д���");
                return;
            }
            string qxsh = "";
         

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                    qxsh = "1";//ȡ����˶���
            }

            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";

            string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
            try
            {
                if (bglx.ToLower().Trim() == "bd")
                {
                    DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                    bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                }
                if (bglx.ToLower().Trim() == "bc")
                {
                    DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                    bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                }
            }
            catch
            {
            }

          

            if (qxsh == "1")
             bgzt = "ȡ�����";

             if (bgzt != "ȡ�����" && bgzt != "�����" && bglx=="cg")
                return;




            if (bgzt.Trim() == "")
            {
                log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh); return;
            }


            if (bgzt != "ȡ�����" && bgzt != "�����")
                return;






            if (bgzt.Trim() == "�����")
            {
               
                string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                string message = ""; string jpgname = "";
                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", ref message);
         
                string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                if (isrtn)
                {
                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                    if (ssa == true)
                    {
                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "�����ϴ�", "���", "����PDF�ɹ�:" + ML + "\\" + jpgname, "ZGQJK", "����PDF");


                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                        if (jkmsg == "1")
                            MessageBox.Show(blh + ",�ϴ�PDF�ɹ�");
                    }
                    else
                    {
                        if (msg == "1" || jkmsg == "1")
                            MessageBox.Show(blh + ",�ϴ�PDFʧ�ܣ���������ˣ�\r\n�쳣��Ϣ��" + message);

                        log.WriteMyLog(message);
                        ZgqClass.BGHJ(blh, "�����ϴ�", "���", message, "ZGQJK", "����PDF");
                        aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_BZ='" + message + "',F_FSZT='δ����'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

                    }
                    zgq.DelTempFile(blh);

                }
                else
                {

                    if (msg == "1" || jkmsg == "1")
                        MessageBox.Show(blh + ",����PDFʧ�ܣ���������ˣ�\r\n�쳣��Ϣ��" + message);
                    log.WriteMyLog(message);
                    ZgqClass.BGHJ(blh, "�ӿ�", "���", message, "ZGQJK", "����PDF");
                    aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_BZ='" + message + "',F_FSZT='δ����'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    zgq.DelTempFile(blh);

                }
                return;

            }
            else
            {
             
                if(bgzt == "ȡ�����")
                {
                    //ɾ��T_BG_PDF��¼
                    DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                    if (dt2.Rows.Count > 0)
                    {
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                        //ɾ��ftp��pdf�ļ�
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        string rtn_msg = "";
                        zgq.DelPDFFile(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);

                    }
                    else
                    {
                        log.WriteMyLog(blh + ",T_BG_PDF��δ���ҵ���¼");
                    }
                     aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='�Ѵ���',F_ISPDF=''  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                 
                }
              

            } 
            return;
        }
    }
}
