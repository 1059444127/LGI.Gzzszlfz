using System;
using System.Collections.Generic;
using System.Text;
using readini;
using dbbase;
using System.Windows.Forms;
using System.Data;
using Netca_PDFSign_COM;
using System.IO;
using System.Drawing;
using netcapscertapphlpLib;
using PathHISZGQJK;
using LGZGQClass;
using NetcaPkiLib;
using NETCA;



namespace PathnetCAzgq
{
    class gdsdermyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public string ca(string yhxx)
        {

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            ////-------��ȡsz�����õĲ���---------------------
            string debug = f.ReadString("CA", "debug", "").Replace("\0", "").Trim(); 
            string msg = f.ReadString("CA", "message", "1").Replace("\0", "").Trim();
            string isywb64 = f.ReadString("CA", "ywb64", "1").Replace("\0", "").Trim();
        
            string getblh = "";
            string type = "";
            string type2 = "";
            string yhm = "";

            string yhmc = "";
            string yhbh = "";
            string yhmm = "";
            string bglx = "";
            string bgxh = "";

            #region CA��½
            if (ZGQClass.GetSZ("CA", "ca", "").Replace("\0", "").Trim() == "1")
            {
                if (yhxx == "")
                {
                    return Login();
                }
            }
            #endregion
            string[] getyhxx = yhxx.Split('^');
            if (getyhxx.Length == 5)
            {
                type = getyhxx[0];
                yhm = getyhxx[1];
                yhmc = getyhxx[3];
                yhbh = getyhxx[2];
                yhmm = getyhxx[4];
            }
            else
            {
                type2 = getyhxx[0];
                getblh = getyhxx[1];
                bgxh = getyhxx[2];
                bglx = getyhxx[3].ToLower();
                type = getyhxx[4];
                yhm = getyhxx[5];
                yhmc = getyhxx[6];
                yhbh = getyhxx[7];
                yhmm = getyhxx[8];
            }


            #region ���ǰ��֤KEY

           
            if (type == "SH")
            {

                return YZKEY(msg, yhmc, yhbh);
            }
            #endregion

             string blbh = getblh + bglx + bgxh;
            #region ��˺�ִ�У�����ǩ��

            if (type == "QZ")
            {
                NETCAPKI oNetcaPki = new NETCAPKI();

                if (debug == "1")
                    MessageBox.Show("���ǩ��");

                string yw = "";
                
                bool WithTSA = false;//��ʱ���ǩ��
                if((f.ReadString("ca", "WithTSA", "0").Trim().Replace("\0", "").Trim())=="1")
                    WithTSA = true;//ǩ��ֵ����ԭ��
                bool havcount=false;
                bool isVerify = false; //������֤
                 if((f.ReadString("ca", "isVerify", "0").Trim().Replace("\0", "").Trim())=="1")
                     isVerify=true;
                 string usapurl = f.ReadString("ca", "usapurl", "").Trim().Replace("\0", "").Trim(); //���ص�ַ
                 string svrcertb64 = f.ReadString("ca", "svrcertb64", "1").Trim().Replace("\0", "").Trim();//����֤��
                string b64SignVal = "";
                int oCert = 0;
                DataTable dt_jcxx = new DataTable();
                  DataTable dt_bc = new DataTable();
                        DataTable dt_bd = new DataTable();
                try
                {
                    #region ԭ��
                 
                    dt_jcxx = aa.GetDataTable("select * from T_JCXX where  F_BLH='" + getblh + "'", "cgbg");

                    if (dt_jcxx == null)
                    {
                        if (msg == "1")
                            MessageBox.Show("�������ݿ��쳣");
                        return "0";
                    }
                    if (dt_jcxx.Rows.Count <= 0)
                    {
                        if (msg == "1")
                            MessageBox.Show("T_JCXX��ѯ�����쳣");
                        return "0";
                    }
                    if (bglx == "cg")
                    {
                        if (dt_jcxx.Rows[0]["F_BGZT"].ToString() != "�����")
                        {
                            if (msg == "1")
                                MessageBox.Show("����δ���");
                            return "0";
                        }
                        yw = "�����:" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&���汨��&&�Ա�:" + dt_jcxx.Rows[0]["F_XB"].ToString() + "&����:" + dt_jcxx.Rows[0]["F_nl"].ToString()
                            + "&סԺ��:" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&�����:" + dt_jcxx.Rows[0]["F_mzh"].ToString() + "&���֤��:" + dt_jcxx.Rows[0]["F_SFZH"].ToString() + "&����:" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                            + "&�������:" + dt_jcxx.Rows[0]["F_blzd"].ToString() + "&����ҽ��:" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "&����ҽ��:" + dt_jcxx.Rows[0]["F_FZYS"].ToString()
                            + "&���ҽ��:" + dt_jcxx.Rows[0]["F_SHYS"].ToString() + "&��������:" + dt_jcxx.Rows[0]["F_bgrq"].ToString() + "&�������:" + dt_jcxx.Rows[0]["F_spare5"].ToString();

                    }
                    //// �������
                    if (bglx == "bc")
                    {
                       
                        dt_bc = aa.GetDataTable("select * from T_BCBG where  F_BLH='" + getblh + "' and F_BC_BGZT='�����'and F_BC_BGXH='" + bgxh + "'", "bcbg");
                        if (dt_bc == null)
                        {
                            if (msg == "1")
                                MessageBox.Show("�������ݿ��쳣");
                            return "0";
                        }
                        if (dt_bc.Rows.Count <= 0)
                        {
                            if (msg == "1")
                                MessageBox.Show("T_BCBG��ѯ�����쳣");
                            return "0";
                        }
                        yw = "�����:" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&���䱨��:" + bgxh + "&�Ա�:" + dt_jcxx.Rows[0]["F_XB"].ToString() + "&����:" + dt_jcxx.Rows[0]["F_nl"].ToString()
                           + "&סԺ��:" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&�����:" + dt_jcxx.Rows[0]["F_mzh"].ToString() + "&���֤��:" + dt_jcxx.Rows[0]["F_SFZH"].ToString() + "&����:" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                           + "&�������:" + dt_bc.Rows[0]["F_BCZD"].ToString() + "&����ҽ��:" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "&����ҽ��:" + dt_bc.Rows[0]["F_bc_FZYS"].ToString()
                           + "&���ҽ��:" + dt_bc.Rows[0]["F_bc_SHYS"].ToString() + "&��������:" + dt_bc.Rows[0]["F_bc_bgrq"].ToString() + "&�������:" + dt_bc.Rows[0]["F_bc_spare5"].ToString();
                    }
                    ///// С�������
                    if (bglx == "bd")
                    {
                       
                        dt_bd = aa.GetDataTable("select * from T_BDBG  where  F_BLH='" + getblh + "' and  F_BD_BGZT='�����' and F_BD_BGXH='" + bgxh + "'", "bcbg");
                        if (dt_bd == null)
                        {
                            if (msg == "1")
                                MessageBox.Show("�������ݿ��쳣");
                            return "0";
                        }
                        if (dt_bd.Rows.Count <= 0)
                        {
                            if (msg == "1")
                                MessageBox.Show("T_BDBG��ѯ�����쳣");
                            return "0";
                        }
                        yw = "�����:" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&��������:" + bgxh + "&�Ա�:" + dt_jcxx.Rows[0]["F_XB"].ToString() + "&����:" + dt_jcxx.Rows[0]["F_nl"].ToString()
                           + "&סԺ��:" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&�����:" + dt_jcxx.Rows[0]["F_mzh"].ToString() + "&���֤��:" + dt_jcxx.Rows[0]["F_SFZH"].ToString() + "&����:" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                           + "&�������:" + dt_bd.Rows[0]["F_BdZD"].ToString() + "&����ҽ��:" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "&����ҽ��:" + dt_bd.Rows[0]["F_bd_FZYS"].ToString()
                           + "&���ҽ��:" + dt_bd.Rows[0]["F_bd_SHYS"].ToString() + "&��������:" + dt_bd.Rows[0]["F_bd_bgrq"].ToString();
                    }

                    if (yw.Trim() == "")
                    {
                        if (msg == "1")
                            MessageBox.Show("����ǩ������Ϊ��");
                        return "0";
                    }

                    #endregion


                    if (isywb64=="1")
                        yw = changebase64(yw);
              
                    #region ǩ��
                    try
                    {
                        ////ǩ��
                        if (WithTSA)
                            b64SignVal = oNetcaPki.SignPKCS7WithTSA(yw, havcount, usapurl); //ʱ���ǩ��
                        else
                            b64SignVal = oNetcaPki.SignPKCS7(yw, havcount);  //����ʱ���ǩ��
                    }
                    catch(Exception  ee1)
                    {
                        if (msg == "1")
                            MessageBox.Show("ǩ��ʧ�ܣ�"+ee1.Message);
                        return "0";
                    }
                    if (b64SignVal == "")
                    {
                        if (msg == "1")
                            MessageBox.Show("ǩ��ʧ��");
                        return "0";
                    }
                    #endregion

                    #region ��ǩ
                   
                         try
                         {
                             oCert = oNetcaPki.VerifyPKCS7(b64SignVal, yw);
                         }
                         catch(Exception ee2)
                         {
                             if (msg == "1")
                                 MessageBox.Show("��ǩʧ��:"+ee2.Message);
                             return "0";
                         }
                         if (oCert == 0)
                         {
                             if (msg == "1")
                                 MessageBox.Show("��ǩʧ��");
                             return "0";
                         }
                         #endregion
                   
                    #region   ��֤֤��
                         if (isVerify)
                            {
                                try
                                {
                                    bool bFlag = oNetcaPki.VerifyCert(usapurl, svrcertb64, 1, oCert);//����֤֤��
                                    if (!bFlag)
                                    {
                                        MessageBox.Show("ǩ��֤����֤ʧ��");
                                        return "0";
                                    }
                                }
                                catch(Exception ee3)
                                {
                                    if(msg=="1")
                                    MessageBox.Show("ǩ��֤����֤ʧ��:"+ee3.Message);
                                    return "0";
                                }

                            }
                         #endregion

                    // ǩ��

                 }
                catch (Exception ex)
                {
                    MessageBox.Show("ǩ���쳣��"+ex.Message);
                     return "0";
                 }
                 #region ǩ����ɣ�д���ݿ�

                 string errmsg = "";
                int x = aa.ExecuteSQL("insert into T_SZQM(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_CZY,F_YW,F_SignCert,F_SignData,F_QZRQ) values('"+blbh+"','"
                    + getblh + "','" + bglx + "','" + bgxh + "','" + yhmc + "','" + yw + "','" 
                    + oCert.ToString() + "','" + b64SignVal.ToString() + "','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
               
                if (debug == "1")
                {
                    if (x >= 1)
                        MessageBox.Show("д��T_CAXX���");
                    else
                        MessageBox.Show("д��T_CAXXʧ�ܣ�" + errmsg);
                }

                 #endregion

                

                if(f.ReadString("ca", "hqkeytp", "0").Trim().Replace("\0", "").Trim() == "1")
                {
                 #region ��ȡǩ��ͼƬ

                string szqmlj = ZGQClass.GetSZ("view", "szqmlj", @"\\127.0.0.1\pathqc\rpt-szqm\YSBMP\");

              
                    try
                    {
                     
                        IPDFSign iPDFSign = new PDFSign();
                        //ѡ��֤��
                        iPDFSign.SelectCert("netca",0);
                        IUtilTool iUtilTool = new UtilTool();
                        //����ѡ�е�ǩ��֤���base64����
                        string CertBase64 = iPDFSign.SignCertBase64Encode;
                        if (CertBase64.Trim() == "")
                        {
                            if (msg == "1")
                                MessageBox.Show("��ȡǩ��֤���base64����ʧ��");
                            return "0";
                        }
                        try
                        {
                     
                            byte[] image = iUtilTool.GetImageFromDevicByCert(CertBase64);
                            try
                            {
                                MemoryStream memoryStream = new MemoryStream(image, 0, image.Length);
                                memoryStream.Write(image, 0, image.Length);
                                //ת��ͼƬ
                                Image ii = Image.FromStream(memoryStream);
                                ii.Save(szqmlj + yhmc + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                            }
                            catch (Exception ee4)
                            {
                                if (msg == "1")
                                    MessageBox.Show("����ǩ��ͼƬʧ�ܣ�" + ee4.Message);
                                return "0";
                            }



                            string pdfszqz = ZGQClass.GetSZ("CA", "pdfszqz", "1");
                            if (pdfszqz == "1")
                            {
                                #region  ����pdf
                                string blh = getblh;
                                try
                                {
                                  
                                    if (bglx == "")
                                        bglx = "cg";
                                    if (bgxh == "")
                                        bgxh = "1";

                                    string bgzt = "";
                                    string filename = dt_jcxx.Rows[0]["F_SPARE5"].ToString();
                                    if (bglx.ToLower() == "bd")
                                    {
                                        bgzt = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                                        filename = dt_bd.Rows[0]["F_BD_bgrq"].ToString();
                                    }
                                    if (bglx.ToLower() == "bc")
                                    {
                                        bgzt = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
                                        filename = dt_bc.Rows[0]["F_Bc_SPARE5"].ToString();
                                    }
                                    if (bglx.ToLower() == "cg")
                                    {
                                        bgzt = dt_jcxx.Rows[0]["F_BGZT"].ToString();
                                        filename = dt_jcxx.Rows[0]["F_SPARE5"].ToString();
                                    }
                                   
                                    if (bgzt == "�����")
                                    {
                                        try
                                        {
                                            filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";
                                        }
                                        catch
                                        {
                                            filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + ".pdf";
                                        }
                                        string ml = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                                        string pdfpath = "";
                                        string rptpath = ZGQClass.GetSZ("ca", "rptpath", "rpt").Replace("\0", "").Trim();

                                        string scpdffs = f.ReadString("ca", "scpdffs", "1").Replace("\0", "").Trim();
                                        ZGQ_PDFJPG zgq = new ZGQ_PDFJPG();
                                        string message = ""; string filePath = "";
                              
                                            //����PDF
                                            string ErrMsg = "";
                                            bool pdf1 = zgq.CreatePDFJPG(blh, bglx, bgxh, ref filename, rptpath, ZGQ_PDFJPG.type.PDF, ref  ErrMsg);
                                            if (!pdf1)
                                            {
                                                MessageBox.Show("����PDFʧ�ܣ����������\r\n" + ErrMsg);
                                                DeleteTempFile(blh);
                                                return "0";
                                            }

                                       if (!File.Exists(filename))
                                       {
                                        MessageBox.Show("����PDFʧ�ܣ����������");
                                        DeleteTempFile(blh);
                                        return "0";
                                       }

                                        filePath = filename;
                                        if (zgq.UpPDF(blh, filename, ml, 0, ref errmsg, ref pdfpath))
                                        {
                                            if (debug == "1")
                                                log.WriteMyLog("�ϴ�PDF�ɹ�");
                                            filename = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                                            ZGQClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDF�ɹ�:" + ml + "\\" + filename, "ZGQJK", "�ϴ�PDF");
                                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" + blh + "','" + filename + "','" + pdfpath + "','')");
                                        }
                                        else
                                        {
                                            MessageBox.Show("�ϴ�ǩ��PDFʧ��,���������\r\n" + errmsg);
                                            ZGQClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDFʧ�ܣ�" + errmsg, "ZGQJK", "�ϴ�PDF");
                                        }
                                       //�ϴ�pdf

                                            iPDFSign.SetImage(image);
                                            iPDFSign.RenderMode = 3;
                                            iPDFSign.SrcFileName = filePath;
                                            string szqm_filename = filePath.Replace(".pdf", "_szqm.pdf");
                                            iPDFSign.DestFileName = szqm_filename;
                                            int xPos = int.Parse(ZGQClass.GetSZ("ca", "xPos", "350").Replace("\0", "").Trim());
                                            int yPos = int.Parse(ZGQClass.GetSZ("ca", "yPos", "100").Replace("\0", "").Trim());
                                            int width = int.Parse(ZGQClass.GetSZ("ca", "width", "80").Replace("\0", "").Trim());
                                            int height = int.Parse(ZGQClass.GetSZ("ca", "height", "40").Replace("\0", "").Trim());
                                            iPDFSign.SignPosition(1, xPos, yPos, width, height);

                                                if (File.Exists(szqm_filename))
                                                {
                                                    
                                                    //�ϴ�ǩ�ֵ�pdf
                                        
                                                     if (zgq.UpPDF(blh, szqm_filename, ml, 0, ref errmsg, ref pdfpath))
                                                    {
                                                      
                                                        if (debug == "1")
                                                            log.WriteMyLog("�ϴ�ǩ��PDF�ɹ�");
                                                        szqm_filename = szqm_filename.Substring(szqm_filename.LastIndexOf('\\') + 1);
                                                        ZGQClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�ǩ��PDF�ɹ�:" + ml + "\\" + szqm_filename, "ZGQJK", "�ϴ�PDF");
                                                        aa.ExecuteSQL("delete T_BG_PDF_CA  where F_BLBH='" + blbh + "'");
                                                        aa.ExecuteSQL("insert  into T_BG_PDF_CA(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" + blh + "','" + szqm_filename + "','" + pdfpath + "','szqm')");
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("�ϴ�ǩ��PDFʧ��,���������\r\n" + errmsg);
                                                        ZGQClass.BGHJ(blh, "�ϴ�ǩ��PDF", "���", "�ϴ�PDFʧ�ܣ�" + errmsg, "ZGQJK", "�ϴ�PDF");
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("PDFǩ��ʧ��,���������");
                                                }
                                        DeleteTempFile(blh);
                                        return "1";
                                    }
                                    else
                                    {
                                        MessageBox.Show("����δ���"); return "0";
                                    }
                                }
                                catch (Exception ee10)
                                {
                                    MessageBox.Show("����PDF�쳣,���������\r\n" + ee10.Message);
                                    DeleteTempFile(blh);
                                    return "0";
                                }
                                #endregion
                            }
                        }
                        catch (Exception ee5)
                        {
                            MessageBox.Show("��ȡǩ��ͼ���쳣,���������\r\n" + ee5.Message);
                            return "0";
                        }
                    }
                    catch (Exception ee6)
                    {
                       MessageBox.Show("��ȡǩ��֤��ʧ��,���������\r\n" + ee6.Message);
                        return "0";
                    }
                
                #endregion
                }
                if (debug == "1")
                    MessageBox.Show("ǩ�����");

                return "1";
            }
            #endregion

            #region ȡ�����
            if (type == "QXSH")
            {
                if (f.ReadString("ca", "qxshyz", "0").Trim().Replace("\0", "").Trim() == "1")
                {
                    return YZKEY(msg, yhmc,yhbh);
                }
                return "1";
            }
            #endregion

            #region
            if (type == "QXQZ")  //&& (bglx == "BC" || bglx == "BD")
            {
                aa.ExecuteSQL("delete from  T_CAXX  where  F_BLBH='" + blbh + "' ");
                aa.ExecuteSQL("delete from  T_BG_PDF  where  F_BLBH='" + blbh + "' ");
                aa.ExecuteSQL("delete from  T_BG_PDF_CA  where  F_BLBH='" + blbh + "' ");
                return "1";
            }
            #endregion

            return "1";
        }

   
        public static DataTable GetYHXX(string CertUID)
        {
            DataTable Dt_Yhxx = new DataTable();
            if (CertUID.Trim() != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    Dt_Yhxx = aa.GetDataTable("select top 1 * from T_YH  where F_YH_BY1='" + CertUID + "' ", "YHXX");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
            return Dt_Yhxx;

        }

        public  string  YZKEY(string msg,string YHMC,string YHBH)
        {
            ///// ��ȡ�û�֤��

            NETCAPKI oNetcaPki = new NETCAPKI();

            string key_userID = "";
            string key_Name = "";
            string CertUID = "";
            string key_DW = "";
            string key_Sfzh = "";
            try
            {
                /*1---���������------1*/
                string rand = oNetcaPki.GenRandom(8);

                /*2---�����������ǩ�������õ���ǩ��ֵ---2*/
                try
                {
                    string b64SignVal = oNetcaPki.SignPKCS7(rand, true);
                    int Cert = oNetcaPki.VerifyPKCS7(b64SignVal, rand);
                    ////�û�֤���ֵ
                    CertUID = (oNetcaPki.GetCertInfo(Cert, 9));
                    ////��λ
                    key_DW = (oNetcaPki.GetCertInfo(Cert, 13));
                    ////�û�����
                    key_Name = (oNetcaPki.GetCertInfo(Cert, 12));
                    ////֤�����к�
                    //CertID=(oNetcaPki.GetCertInfo(Cert, 2));
                    ////֤����
                    key_Sfzh = (oNetcaPki.GetCertInfo(Cert, 36));
                }
                catch (Exception ee1)
                {
                    if (ee1.Message == "֤��ѡ��ʧ��")
                    {
                        if (msg == "1")
                            MessageBox.Show("֤��ѡ��ʧ�ܣ���ȷ��Key���Ƿ����");
                    }
                    else
                    {
                        if (msg == "1")
                            MessageBox.Show(ee1.Message);
                    }
                    return "0";
                }
            }
            catch (Exception ee)
            {
                if (msg == "1")
                    MessageBox.Show("�����ʼ��ʧ��");
                return "0";
            }

            /// ��֤�û���
            if (f.ReadString("CA", "yzxm", "0").Replace("\0", "").Trim() == "1")
            {
                if (key_Name!=YHMC)
                {
                    if (msg == "1")
                        MessageBox.Show("���ʹ������KEY�û���һ��,��֤ʧ�ܣ�");
                    return "0";
                }
            }

            DataTable Dt_yh = GetYHXX(CertUID);
        
            if (Dt_yh.Rows.Count > 0)
            {
                if (f.ReadString("CA", "yzsfzh", "0").Replace("\0", "").Trim() == "1")
                {
                    if (Dt_yh.Rows[0]["F_SFZH"].ToString().Trim() != key_Sfzh)
                    {
                        if (msg == "1")
                            MessageBox.Show("�û����֤����KEY�����֤�Ų�һ��,��֤ʧ�ܣ�");
                        return "0";
                    }
                }

                if (f.ReadString("CA", "yzyhbh", "0").Replace("\0", "").Trim() == "1")
                {
                    //��ȡOIDֵ�е�֤����Ϣ
                    string str1 = CertUID.Substring(CertUID.IndexOf("@") + 8);

                    //����
                    byte[] bs = NETCAPKI.base64Decode(str1);
                    string Key_gh = Encoding.Default.GetString(bs);

                    if (Dt_yh.Rows[0]["F_YHBH"].ToString().Trim() != Key_gh)
                    {
                        if (msg == "1")
                            MessageBox.Show("�û�������KEY�й��Ų�һ��,��֤ʧ�ܣ�");
                        return "0";
                    }
                }

                if (Dt_yh.Rows[0]["F_YHMC"].ToString().Trim() != YHMC)
                {
                    if (msg == "1")
                        MessageBox.Show("�û�������KEY�û���һ��,��֤ʧ�ܣ�");
                    return "0";
                }
                return "1";
            }
            else
            {
                MessageBox.Show("��Keyδ���û�");
                return "0";
            }
       
        }

        public string Login()
        {

            NETCAPKI oNetcaPki = new NETCAPKI();
            ///// ��ȡ�û�֤��
            string CertUID = "";
            string key_DW = "";
            string key_Name = ""; 
            string key_Sfzh = "";
            try
            {
                /*1---���������------1*/
                string rand = oNetcaPki.GenRandom(8);

                /*2---�����������ǩ�������õ���ǩ��ֵ---2*/
                try
                {
                    string b64SignVal = oNetcaPki.SignPKCS7(rand, true);
                    int Cert = oNetcaPki.VerifyPKCS7(b64SignVal, rand);
                    ////�û�֤���ֵ
                    CertUID = (oNetcaPki.GetCertInfo(Cert, 9));
                    ////��λ
                    key_DW = (oNetcaPki.GetCertInfo(Cert, 13));
                    ////�û�����
                    key_Name = (oNetcaPki.GetCertInfo(Cert, 12));
                    ////֤�����к�
                    //CertID=(oNetcaPki.GetCertInfo(Cert, 2));
                    ////֤����
                    key_Sfzh = (oNetcaPki.GetCertInfo(Cert, 36));
                }
                catch (Exception ee1)
                {
                    if (ee1.Message == "֤��ѡ��ʧ��")
                    {
                            MessageBox.Show("֤��ѡ��ʧ�ܣ���ȷ��Key���Ƿ����");
                    }
                    else
                    {
                            MessageBox.Show(ee1.Message);
                    }
                    return "0";
                }
            }
            catch (Exception ee)
            {
                    MessageBox.Show("�����ʼ��ʧ��");
                    return "0";
            }


            /// ��֤�û���
            DataTable Dt_yh = GetYHXX(CertUID);
            if (Dt_yh.Rows.Count > 0)
            {
                if (f.ReadString("CA", "yzsfzh", "0").Replace("\0", "").Trim() == "1")
                {
                    if (Dt_yh.Rows[0]["F_SFZH"].ToString().Trim() != key_Sfzh)
                    {

                        MessageBox.Show("�û����֤�Ų�ƥ��,��½ʧ�ܣ�");
                        return "0";
                    }
                }
                if (f.ReadString("CA", "yzxm", "0").Replace("\0", "").Trim() == "1")
                {
                    if (key_Name!=Dt_yh.Rows[0]["F_YHMC"].ToString().Trim())
                    {
                        MessageBox.Show("�û�������ƥ��,��½ʧ��");
                        return "0";
                    }
                }

                if (f.ReadString("CA", "yzyhbh", "0").Replace("\0", "").Trim() == "1")
                {
                    //��ȡOIDֵ�е�֤����Ϣ
                    string str1 = CertUID.Substring(CertUID.IndexOf("@") + 8);

                    //����
                    byte[] bs = NETCAPKI.base64Decode(str1);
                    string Key_gh = Encoding.Default.GetString(bs);

                    if (Dt_yh.Rows[0]["F_YHBH"].ToString().Trim() != Key_gh)
                    {
                        MessageBox.Show("�û�������KEY�й��Ų�һ��,��֤ʧ�ܣ�");
                        return "0";
                    }
                }

                return Dt_yh.Rows[0]["F_YHM"].ToString().Trim() + "^" + Dt_yh.Rows[0]["F_YHMM"].ToString().Trim();
            }
            else
            {
                MessageBox.Show("δ��ѯ�����û���Ϣ����û�Keyδ��");
                return "0";
            }

        }

        public void DeleteTempFile(string blh)
        {
            try
            {
               System.IO.Directory.Delete(@"c:\temp\" + blh, true);
            }
            catch
            {
            }
        }

        #region ��Base64������ı�ת������ͨ�ı�
        /// <summary>
        /// ��Base64������ı�ת������ͨ�ı�
        /// </summary>
        /// <param name="base64">Base64������ı�</param>
        /// <returns></returns>
        public static string Base64StringToString(string base64)
        {
            if (base64 != "")
            {
                char[] charBuffer = base64.ToCharArray();
                byte[] bytes = Convert.FromBase64CharArray(charBuffer, 0, charBuffer.Length);
                string returnstr = Encoding.Default.GetString(bytes);
                return returnstr;
            }
            else
            {
                return "";
            }
        }
        #endregion
        #region �ַ���תΪbase64�ַ���
        public static string changebase64(string str)
        {
            if (str != "" && str != null)
            {
                byte[] b = Encoding.Default.GetBytes(str);
                string returnstr = Convert.ToBase64String(b);
                return returnstr;
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
    // class NETCAPKI
    //{
    //    private COMNCertAppHelper oCertHlp = new COMNCertAppHelper();
    //    /// <summary>
    //    /// ���������
    //    /// </summary>
    //    /// <param name="len">������ĳ���</param>
    //    /// <returns>�����</returns>
    //    public string GenRandom(short len)
    //    {
    //        string rand = oCertHlp.GenRandom(len);
    //        return rand;
    //    }

    //    /// <summary>
    //    /// ����֤��ѡ����û�ѡ��֤�飬��ѡ���֤�����P7ǩ��
    //    /// </summary>
    //    /// <param name="content">ǩ��ԭ��</param>
    //    /// <param name="isHavCont">ǩ��ֵ�Ƿ����ԭ�ģ�true����ԭ�ģ�false������ԭ��</param>
    //    /// <returns>base64�����ǩ��ֵ</returns>
    //    public string SignPKCS7(string content, bool isHavCont)
    //    {
    //        string filter = "InValidity='True'&&(IssuerCN='NETCA Organization ClassA CA'||IssuerCN='NETCA ClassB Testing and Evaluation OrganizationCA')";
    //        int oCert = oCertHlp.SelectMyCert(1, filter, 1, 1);
    //        if (oCert == 0)
    //        {
    //            throw new Exception("û��ѡ�����������֤�飡");
    //        }
    //        object oContent = oCertHlp.StrDecode("UTF-8", content);
    //        object oSignVal = oCertHlp.SignPKCS7(oCert, oContent, isHavCont, "", "");
    //        string b64SignVal = oCertHlp.StrEncode("Base64", oSignVal);
    //        return b64SignVal;
    //    }
    //    /// <summary>
    //    /// ��֤P7��ǩ��
    //    /// </summary>
    //    /// <param name="signVal">base64�����ǩ��ֵ</param>
    //    /// <param name="content">ǩ��ԭ��</param>
    //    /// <returns>��֤�ɹ�����ǩ��֤��</returns>
    //    public int VerifyPKCS7(string signVal, string content)
    //    {
    //        object oContent = null;
    //        int oSignCert = 0;
    //        object oSignVal = null;
    //        if (!String.IsNullOrEmpty(content))
    //        {
    //            oContent = oCertHlp.StrDecode("UTF-8", content);
    //        }
    //        try
    //        {
    //            oSignVal = oCertHlp.StrDecode("Base64", signVal);
    //            oSignCert = oCertHlp.SimpleVerifyPKCS7(oSignVal, oContent);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception("ǩ����֤ʧ�ܣ�");
    //        }
    //        return oSignCert;
    //    }
    //    /// <summary>
    //    /// ��ʱ���������ǩ��
    //    /// </summary>
    //    /// <param name="content">ǩ��ԭ��</param>
    //    /// <param name="isHavCont">ǩ��ֵ�Ƿ����ԭ�ģ�trueΪ����ԭ�ģ�falseΪ������ԭ��</param>
    //    /// <param name="tasUrl">ʱ���URL</param>
    //    /// <returns>ǩ��ֵ</returns>
    //    public string SignPKCS7WithTSA(string content, bool isHavCont, string tasUrl)
    //    {
    //        string filter = "(IssuerCN~'NETCA')";
    //        int oCert = oCertHlp.SelectMyCert(1, filter, 1, 1);
    //        if (oCert == 0)
    //        {
    //            throw new Exception("û��ѡ�����������֤�飡");
    //        }
    //        object oContent = oCertHlp.StrDecode("UTF-8", content);
    //        object oSignVal = oCertHlp.SignPKCS7(oCert, oContent, isHavCont, tasUrl, "");
    //        string b64SignVal = oCertHlp.StrEncode("Base64", oSignVal);
    //        return b64SignVal;
    //    }

    //    public string GetInfoFromSignedData(string signVal, string content, int type)
    //    {
    //        string val = oCertHlp.GetInfoFromSignedDataByP7(signVal, content, type);
    //        return val;
    //    }

    //    /// <summary>
    //    /// ����NETCA������֤֤����Ч��
    //    /// </summary>
    //    /// <param name="url">NETCA���ص�ַ</param>
    //    /// <param name="b64Cert">NETCA���ط�����֤�飬base64�����ַ���</param>
    //    /// <param name="gwType">NETCA�������ͣ�0Ϊ�������أ�1Ϊ��������</param>
    //    /// <param name="cert">��Ҫ��֤��֤�飬֤���������</param>
    //    /// <returns>֤����Ч����true��ʧ�����쳣��Ϣ</returns>
    //    public bool VerifyCert(string url, string b64Cert, int gwType, int cert)
    //    {
    //        COMNCertVerifier oVerify = oCertHlp.CreateCVSCertVerifier(url, b64Cert, gwType);
    //        int status = oVerify.VerifyCert(cert, "", 3, "", "");
    //        if (status == 0)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            string errMsg = ErrMsg(status);
    //            throw new Exception(errMsg, null);
    //        }
    //    }
    //    /// <summary>
    //    /// ��ȡ֤����չֵ
    //    /// </summary>
    //    /// <param name="cert">֤�����</param>
    //    /// <param name="oid">֤��oidֵ</param>
    //    /// <returns>֤����չֵ</returns>
    //    public string GetCertExtensionStringValue(int cert, string oid)
    //    {
    //        return oCertHlp.GetCertExtensionStringValue(cert, oid);
    //    }
    //    /// <summary>
    //    /// ��ȡ֤����Ϣ
    //    /// </summary>
    //    /// <param name="cert">֤�����</param>
    //    /// <param name="type">֤�������</param>
    //    /// <returns>֤����Ϣ</returns>
    //    public string GetCertInfo(int cert, int type)
    //    {
    //        return oCertHlp.GetCertInfo(cert, type);
    //    }

    //    /// <summary>
    //    /// NETCA���ط��صĴ�����ת����Ӧ����Ϣ
    //    /// </summary>
    //    /// <param name="status">NETCA���ط��ص�״̬��</param>
    //    /// <returns>������Ϣ</returns>
    //    private string ErrMsg(int status)
    //    {
    //        string msg = "";
    //        switch (status)
    //        {
    //            case 1:
    //                msg = "��֤����ʧ��";
    //                break;
    //            case 2:
    //                msg = "֤���ʽ����";
    //                break;
    //            case 3:
    //                msg = "֤�鲻����Ч����";
    //                break;
    //            case 4:
    //                msg = "��Կ��;����";
    //                break;
    //            case 5:
    //                msg = "֤�����ֲ���";
    //                break;
    //            case 6:
    //                msg = "֤����Բ���";
    //                break;
    //            case 7:
    //                msg = "֤����չ����";
    //                break;
    //            case 8:
    //                msg = "֤������֤ʧ��";
    //                break;
    //            case 9:
    //                msg = "֤�鱻ע��";
    //                break;
    //            case 10:
    //                msg = "ע��״̬����ȷ��";
    //                break;
    //            case 11:
    //                msg = "֤��δע��";
    //                break;
    //            case 12:
    //                msg = "֤�鱻��ʱ����/δ����";
    //                break;
    //            default:
    //                break;
    //        }
    //        return msg;
        
    //    }

    //    /// <summary>1.5 Base64����
    //    /// 
    //    /// </summary>
    //    /// <param name="sData"></param>
    //    /// <returns></returns>
    //    public static byte[] base64Decode(string sData)
    //    {
    //        Utilities oUtilities = new Utilities();
    //        return (byte[])oUtilities.Base64Decode(sData, Constants.NETCAPKI_BASE64_ENCODE_NO_NL);
    //    }

    //}
}
