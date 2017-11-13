
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
    
    class pdfcs
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string debug = "";
        public void pdfjpg(string blh, string bglx, string bgxh, string czlb,string dz, string msg, string debug,string yymc)
        {
            bglx = bglx.ToLower();
              if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "1";

           string blbh=blh+bglx+bgxh;
            if(bglx=="cg")
                blbh=blh;


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

            if (bglx.Trim() == "")
            {
                log.WriteMyLog("��������Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);
                return;
            }
             if (dz == "qxsh"||dz == "ȡ�����")
                {
                    //ȡ����˶���
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "'");
                }

         
                string bgzt= jcxx.Rows[0]["F_BGZT"].ToString();
                try
                {
                    if (bglx.ToLower().Trim() == "bd")
                    {
                        DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt= dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    //    jpgname=blh+bglx+bgxh+"_"+dt_bd.Rows[0]["F_BD_BGRQ"].ToString();
                    }
                    if (bglx.ToLower().Trim() == "bc")
                    {
                        DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                        bgzt= dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                   //     jpgname=blh+bglx+bgxh+"_"+dt_bc.Rows[0]["F_BC_SPARE5"].ToString();
                    }
                    if (bglx.ToLower().Trim() == "cg")
                    {
                        bgzt = jcxx.Rows[0]["F_BGZT"].ToString();
                   //     jpgname=blh+bglx+bgxh+"_"+jcxx.Rows[0]["F_SPARE5"].ToString();
                    }
                }
                catch (Exception e5)
                {
                    log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh + e5.Message);
                }
             
                 if(bgzt!="�����")
                     return;

                  string type = f.ReadString("savetohis", "type", "pdf").Trim().ToLower();
                   
                   

                    
                   ////����pdf��jpg
                  string jpgname = "";//����pdf��jpg�ļ���
                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");//ftp ��Ŀ¼����Ϊ��
                   ZgqPDFJPG zgq = new ZgqPDFJPG();
                    string message = "";
                   // string pdfpath = "";
                   // if (zgq.CreatePDF(blh, bglx, bgxh, ref jpgname, "", "", ZgqPDFJPG.Type.PDF, ref message))
                   // {
                   //     log.WriteMyLog("[" + blh + "]����PDF�ɹ�");
                   //     //����pdf�ɹ��ϴ�pdf�����������
                   //     if (zgq.UpPDF(blh, jpgname, ML, ref message, 3, ref pdfpath))
                   //     {
                   //         log.WriteMyLog("[" + blh + "]�ϴ�PDF�ɹ�:" + pdfpath);
                   //         jpgname = pdfpath.Substring(pdfpath.LastIndexOf('\\') + 1);
                   //         aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                   //         aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                   //     }
                   //     else
                   //         log.WriteMyLog("[" + blh + "]�ϴ�PDFʧ��:" + message);
                   // }
                   // else
                   // {
                   //     log.WriteMyLog("["+blh+"]����PDFʧ��:" + message);
                   //     ZgqClass.BGHJ(blh, "����PDFʧ��", "���", "����pdfʧ��" + message, "ZGQJK", "����PDF");
                   // }



                    bool isrtn = false;
                    if (yymc.Trim() == "pdf")
                        isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", "", ref message);
                   else
                        isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", "", ref message);

                    if (!isrtn)
                    {
                        log.WriteMyLog("����PDFʧ��" + message);
                        ZgqClass.BGHJ(blh, "����PDFʧ��", "���", "����pdfʧ��" + message, "ZGQJK", "����PDF");
                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("����PDF�ɹ�");

                        ////�����ƴ�
                        if (File.Exists(jpgname))
                        {

                            //�ϴ�
                            string pdfpath = "";
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message,3, ref pdfpath);
                          
                            if (ssa == true)
                            {
                                if (debug == "1")
                                 log.WriteMyLog("�ϴ�PDF�ɹ�");

                                jpgname = pdfpath.Substring(pdfpath.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "�ϴ�PDF�ɹ�", "���", "�ϴ�PDF�ɹ�:" + pdfpath, "ZGQJK", "�ϴ�PDF");
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                            }
                            else
                            {
                                log.WriteMyLog("�ϴ�PDFʧ��" + message);
                                ZgqClass.BGHJ(blh, "�ϴ�PDFʧ��", "���", message, "ZGQJK", "�ϴ�PDF");
                            }
                        }
                        else
                        {
                             log.WriteMyLog("δ�ҵ�PDF�ļ�" + jpgname);
                            ZgqClass.BGHJ(blh, "����PDFʧ��", "���", "δ�ҵ��ļ�" + jpgname, "ZGQJK", "����PDF");
                        }
                    }
                    zgq.DelTempFile(blh);

    
                return;

            }
        }
}
