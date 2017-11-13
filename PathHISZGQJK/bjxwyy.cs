using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.IO;
using System.Data.OracleClient;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class bjxwyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string F_blh, string bglx, string bgxh,string czlb,string dz, string msg, string debug)
        {

            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();
            try
            {
                //��˺��ϴ�����
                string shhscbg = f.ReadString("savetohis", "shhscbg", "1").Replace("\0", "");
                int yssj = f.ReadInteger("savetohis", "yssj", 8);
                if (bglx == "")
                    bglx = "cg";

                if (bgxh == "")
                    bgxh = "1";

                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable bljc = new DataTable();
                try
                {
                    bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return;
                }
                if (bljc == null)
                {
                    MessageBox.Show("�������ݿ����������⣡");
                    return;
                }
                if (bljc.Rows.Count < 1)
                {
                    MessageBox.Show("������д���");
                    return;
                }

                string bgzt = "";
                if (dz == "qxsh")
                    bgzt = "ȡ�����";
                else
                    bgzt = bljc.Rows[0]["F_BGZT"].ToString();

                if (bljc.Rows[0]["F_ZYH"].ToString().Trim() == "" && bljc.Rows[0]["F_MZH"].ToString().Trim() == "")
                {
                    log.WriteMyLog("סԺ�ź������Ϊ�գ�������");
                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='������',F_BZ='סԺ�ź������Ϊ�գ�������' where F_blh='" + F_blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
                    return;
                }

                bool issc = false;
                string ftplj = "";
                if (shhscbg == "1")
                {
                    if (bgzt == "�����" && bljc.Rows[0]["F_SFDY"].ToString().Trim() == "��")
                        issc = true;
                }

                //�޸��ѷ���״̬
                if (bgzt == "�����" && bljc.Rows[0]["F_SFDY"].ToString().Trim() == "��")
                {
                    if (bglx == "cg")
                    {
                        try
                        {
                            if (DateTime.Parse(bljc.Rows[0]["F_spare5"].ToString().Trim()).AddHours(yssj) <= DateTime.Now)
                                aa.ExecuteSQL("update T_JCXX  set F_BGZT='�ѷ���'  where F_BLH='" + F_blh + "'");
                            else
                                issc = false;
                        }
                        catch
                        {
                        }
                    }
                }

                bool pdftoyxpt = false;
                #region  ���ɲ��ϴ�pdf��������

                if (bgzt == "�ѷ���" || issc)
                {
                    try
                    {
                        string bgzt2 = "";
                        DataTable dt_bd = new DataTable();
                        DataTable dt_bc = new DataTable();
                        try
                        {
                            if (bglx.ToLower().Trim() == "bd")
                            {
                                dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + F_blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                                bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                            }

                            if (bglx.ToLower().Trim() == "bc")
                            {
                                dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + F_blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                                bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                            }
                            if (bglx.ToLower().Trim() == "cg")
                            {
                                bgzt2 = bljc.Rows[0]["F_BGZT"].ToString();
                            }
                        }
                        catch
                        {
                        }

                        if (bgzt2.Trim() == "")
                            log.WriteMyLog("����״̬Ϊ�գ�������" + F_blh + "^" + bglx + "^" + bgxh);

                        if ((bgzt2.Trim() == "�ѷ���" || issc) && bgzt != "ȡ�����")
                        {
                            ////////����pdf**********************************************************
                            string jpgname = "";
                            string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                            if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
                            {
                                #region  ����pdf
                                string message = "";
                                ZgqPDFJPG zgq = new ZgqPDFJPG();
                                if (debug == "1")
                                    log.WriteMyLog("��ʼ����PDF������");
                                bool isrtn = zgq.CreatePDF(F_blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, ref message);

                                string pdfpath = "";
                                if (isrtn)
                                {
                                    if (File.Exists(jpgname))
                                    {
                                        //�ϴ����������
                                        bool ssa = zgq.UpPDF(F_blh, jpgname, ML, ref message, 3,ref pdfpath);
                                        if (ssa == true)
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog("�ϴ�PDF�ɹ�");

                                            string jpgname2 = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                            ZgqClass.BGHJ(F_blh, "JK", "���", "�ϴ�PDF�ɹ�:" + pdfpath, "ZGQJK", "�ϴ�PDF");

                                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + F_blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + F_blh + "','" + jpgname2 + "')");
                                            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='�Ѵ���',F_ispdf='true' where F_blh='" + F_blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' ");
                                          }
                                        else
                                        {
                                            if (msg == "1")
                                                MessageBox.Show("�ϴ�PDFʧ�ܣ�" + message);
                                            log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + message);
                                            ZgqClass.BGHJ(F_blh, "JK", "���", message, "ZGQJK", "�ϴ�PDF");
                                            //   aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                        }

                                        //�ϴ�������

                                        //string ftpml = DateTime.Now.ToString("yyyyMMdd");
                                        //bool status = false;
                                        //for (int y = 0; y < 3; y++)
                                        //{
                                        //    status = zgq.UpPDF("", jpgname, ftpml, ref message, 4);
                                        //    if (status)
                                        //        break;
                                        //}
                                        //if (status)
                                        //{
                                        //    pdftoyxpt = true;
                                        //    FileInfo fi = new FileInfo(jpgname);
                                        //    ftplj = "PIS" + "//" + ftpml + "//" + fi.Name;
                                        //    log.WriteMyLog("ftp�ϴ��ɹ����ϴ�·����" + ftplj);
                                        //}
                                    }
                                    else
                                    {
                                        if (msg == "1")
                                            MessageBox.Show("����PDFʧ��:δ�ҵ��ļ�---" + jpgname);
                                        log.WriteMyLog("����PDFʧ��:δ�ҵ��ļ�---" + jpgname);
                                        ZgqClass.BGHJ(F_blh, "JK", "���", "�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname, "ZGQJK", "����PDF");
                                    }
                                }
                                else
                                {
                                    if (msg == "1")
                                        MessageBox.Show("����PDFʧ�ܣ�" + message);
                                    log.WriteMyLog("����PDFʧ�ܣ�" + message);
                                    ZgqClass.BGHJ(F_blh, "JK", "���", message, "ZGQJK", "����PDF");
                                    // aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='����PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                                zgq.DelTempFile(F_blh);
                                #endregion
                            }
                            //////////////////////*****************************************************************
                        }
                        else
                        {
                            if (bgzt == "ȡ�����")
                            {
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + F_blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            }
                        }
                    }
                    catch (Exception ee3)
                    {
                        log.WriteMyLog(ee3.Message);
                    }
                }
                #endregion


                if (bglx != "cg")//ֻ�����汨��
                {
                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='������'  F_bz='�ǳ��汨��' where F_blh='" + F_blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
                    return;
                }
               
            }
            catch(Exception  ee)
            {
                log.WriteMyLog(ee.Message);
            }

            return;

        }
    }
}
