
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
    class pdfcs
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
            string jpgname = "";
            string jpgpath = "";

                string bz = "";
                if (bgzt == "�����")
                {

                   
                            if (debug == "1")
                                log.WriteMyLog("����pdf��ʼ");

                        
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

                    }
                //ȡ����ˣ�ɾ��pdf
             
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
