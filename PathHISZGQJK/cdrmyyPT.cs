using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Collections;
using ZgqClassPub;


namespace PathHISZGQJK
{
    class cdrmyyPT
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");


        string debug = "";
        string ReprotFile = "";
        public void pathtohis_PT(string blh, string bglx, string bgxh, string msg, string debug1, string[] cslb)
        {
           dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
          DataTable dt_jcxx = new DataTable();
          MessageBox.Show(bgxh);

          if (bgxh == "")
              bgxh ="0";
          debug = f.ReadString("savetohis", "debug", "0");
         string  fsms = f.ReadString("savetohis", "fsms", "0");
          if (bglx == "bc")
              return;
          if (bglx == "bd")
              return;

            if (bglx == "bc")
            {
                dt_jcxx = aa.GetDataTable("select * from T_jcxx a left join T_bcbg b on a.F_blh= b.F_blh where a.F_blh='" + blh + "' and b.F_bc_bgxh='" + bgxh + "'", "jcxx");
            }
            else
            {
                dt_jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");

            }
            if (dt_jcxx == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                 log.WriteMyLog("�������ݿ����������⣡");
                return;
            }
            if (dt_jcxx.Rows.Count < 1)
            {
                //   MessageBox.Show("������д���");
                aa.ExecuteSQL("update T_JCXX_FS set F_bz='JCXX�����޼�¼',F_FSZT='������'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                 log.WriteMyLog("������д���");
                return;
            }

            //����pdf����ת�ɶ����������ַ���

            string ReprotFile = ""; string jpgname = "";
            string ML = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");


            if (f.ReadString("savetohis", "ispdf", "0").Trim() == "1")
            {
                #region  ����pdf
                string message = "";
                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF,ref jpgname,"", ref message);

                string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                if (isrtn)
                {
                    if (debug == "1")
                        log.WriteMyLog("����PDF�ɹ�");
                    //�����ƴ�
                    if (File.Exists(jpgname))
                    {
                        try
                        {
                            if (f.ReadString("savetohis", "hxbg", "0") == "1")
                            {
                                FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
                                Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
                                file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
                                file.Close();
                                ReprotFile = Convert.ToBase64String(imgByte);
                                //   LGZGQClass.log.WriteMyLog("PDFת�������ƴ��ɹ�"); 
                            }
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                            if (ssa == true)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("�ϴ�PDF�ɹ�");
                                MessageBox.Show(bgxh);

                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "�����ϴ�", "���", "����PDF�ɹ�:" + ML + "\\" + jpgname, "ZGQJK", "����PDF");
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                                aa.ExecuteSQL("update  T_JCXX_FS set F_ISPDF='1' where  F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            
                            }
                            else
                            {
                               log.WriteMyLog("�ϴ�PDFʧ��");
                            }
                        }
                        catch (Exception ee)
                        {
                             log.WriteMyLog("PDFת�������ƴ�ʧ��");
                        }
                    }
                    else
                    {
                        log.WriteMyLog("δ�ҵ��ļ�" + jpgname);
                        ZgqClass.BGHJ(blh, "�����ϴ�", "���", "δ�ҵ��ļ�" + jpgname, "ZGQJK", "����PDF");
                    }
                    zgq.DelTempFile(blh);

                }
                else
                {
                    log.WriteMyLog("����PDFʧ�ܣ�" + message);
                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                }

                #endregion
            }

            if (dt_jcxx.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("��������ţ����ݺţ���������");
                aa.ExecuteSQL("update T_JCXX_FS set F_bz='�������Ϊ�ղ�����',F_FSZT='������'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                return;
            }


            if (f.ReadString("savetohis", "hxzt", "0") == "1")
            {
                #region ��д״̬
                if (bglx == "cg")
                {


                    DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + dt_jcxx.Rows[0]["F_sqxh"].ToString().Trim() + "'", "jcxx");
                    if (dt_sqd.Rows.Count < 1)
                    {
                        aa.ExecuteSQL("delete  T_jcxx_zt where F_BLH='" + blh + "'");
                        return;
                    }
                    string message = ""; string brlb = "";

                    log.WriteMyLog("����״̬XML");
                    //���ɷ���״̬��xml
                    bool rtn1 = bgzt_msg(dt_jcxx, dt_sqd, ref message, ref brlb, blh);
                    if (rtn1)
                    {

                        if (debug == "1")
                            log.WriteMyLog(message);
                        //����״̬
                        string rtn_zt = "";
                        if (fsms=="1")
                       rtn_zt= fszt_Web(blh, message, brlb, debug);
                        else
                       rtn_zt = fszt(blh, message, brlb, debug);

                       if (rtn_zt.Substring(0, 3) == "ERR")
                        {
                            aa.ExecuteSQL(" update T_jcxx_zt  set F_bz='" + rtn_zt + "'  where F_BLH='" + blh + "'");
                            log.WriteMyLog("״̬��Ϣ����ʧ�ܣ�" + rtn_zt);
                        }
                        else
                        {
                            if (debug == "")
                                log.WriteMyLog("״̬��Ϣ���ͳɹ���" + rtn_zt);
                            aa.ExecuteSQL("delete  T_jcxx_zt where F_BLH='" + blh + "'");
                        }

                    }
                    else
                    {
                        if (message == "")
                            aa.ExecuteSQL("delete  T_jcxx_zt where F_BLH='" + blh + "'");
                        log.WriteMyLog("����״̬xmlʧ�ܣ�" + message);
                    }
                }


                #endregion

            }

            if (f.ReadString("savetohis", "hxbg", "0") == "1")
            {
                #region ��д����
                if (bglx == "cg")
                {
                    if (dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim() != "�����")
                    {
                        log.WriteMyLog("����δ��ˣ�������");
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='����δ���,������',F_FSZT='������'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        return;
                    }
                }

                if (bglx == "bc")
                {
                    if (dt_jcxx.Rows[0]["F_bc_BGZT"].ToString().Trim() != "�����")
                    {
                        log.WriteMyLog("����δ��ˣ�������");
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='���䱨��δ���,������',F_FSZT='������'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        return;
                    }
                }


                DataTable dt_sqd = new DataTable();
                dt_sqd = aa.GetDataTable("select * from T_SQD where  F_sqxh='" + dt_jcxx.Rows[0]["F_SQXH"].ToString() + "'", "sqd");

                if (dt_sqd.Rows.Count <= 0)
                {
                    log.WriteMyLog("T_SQD���޴����뵥��¼��������");
                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='T_SQD���޴����뵥��¼��������',F_FSZT='������'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    return;
                }


                msg = ""; string brlb = ""; string uid = ""; string exceptionmsg = "";
                bool rtnbool = Rtn_Message(dt_jcxx, dt_sqd, ref msg, ref brlb, ref uid, bglx, bgxh, ref exceptionmsg, blh);
                if (rtnbool)
                {
                    if (debug == "1")
                        log.WriteMyLog(msg);
                    string rtn_msg = "";
                    if (fsms=="1")
                     rtn_msg=   fsbg_Web(msg, dt_jcxx.Rows[0]["F_BRLB"].ToString());
                    else
                      rtn_msg= fsbg(msg, dt_jcxx.Rows[0]["F_BRLB"].ToString());
                    if (rtn_msg.Substring(0, 3) == "ERR")
                    {
                        log.WriteMyLog(rtn_msg);
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='���ͱ���ʧ�ܣ�" + rtn_msg + "'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    }
                    else
                    {
                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + rtn_msg + "',F_FSZT='�Ѵ���'  where  F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    }
                }
                else
                {
                    log.WriteMyLog("����XMLʧ�ܣ�" + exceptionmsg );
                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='����XMLʧ�ܣ�" + exceptionmsg + "'  where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                    return;
                }
                #endregion
            }

        }


        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + int.Parse(blh[i].ToString()).ToString();
                }
                catch { }
            }
            return x;
        }

        public bool Rtn_Message(DataTable dt,DataTable dt_sqd, ref string msg, ref string xbrlb, ref string uid, string bglx, string bgxh, ref string exceptionmsg,string blh)
        {

            try
            {
                sqldb aa = new sqldb(Application.StartupPath + "\\sz.ini", "sqlserver");
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<ClinicalDocument xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../coreschemas/CDA.xsd\">";

                //<!--===================================-->
                //<!-- ��鱨��                           -->
                //<!--===================================-->
                //<!--
                //********************************************************
                //  CDA Header
                //********************************************************
                //-->
                //    <!-- �ĵ����÷�Χ���� -->
                xml = xml + "<realmCode code=\"CN\"/>";
                //    <!-- �ĵ���Ϣģ�����-��ʶ�� -->
                //    <!-- �̶�ֵ -->
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_HD000040\"/>";
                //    <!-- �ĵ���ʶ-����� -->
                string bgh = dt.Rows[0]["f_BLH"].ToString();
                string bgys = dt.Rows[0]["f_bgys"].ToString();
                string shys = dt.Rows[0]["f_shys"].ToString();
                string bgrq = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMdd") + "0000";
                if (bglx == "bc")
                {
                    bgh = bgh + "_bc_" + bgxh;
                    bgys = dt.Rows[0]["f_bc_bgys"].ToString();
                    shys = dt.Rows[0]["f_bc_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMdd") + "0000";
                }
                if (bglx == "bd")
                {
                    bgh = bgh + "_bd_" + bgxh;
                    bgys = dt.Rows[0]["f_bd_bgys"].ToString();
                    shys = dt.Rows[0]["f_bd_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMdd") + "0000";
                }

                string bgysid = "";
                string shysid = "";
                System.Data.DataTable bgysdt = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + bgys + "'", "bgys");
                System.Data.DataTable shysdt = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + shys + "'", "bgys");
                System.Data.DataTable qcysdt = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + dt.Rows[0]["F_QCYS"].ToString() + "'", "qcys");
                string qcysid = "";
                if (bgysdt.Rows.Count > 0)
                {
                    bgysid = bgysdt.Rows[0]["F_yhbh"].ToString().Trim();
                }

                if (shysdt.Rows.Count > 0)
                {
                    shysid = shysdt.Rows[0]["F_yhbh"].ToString().Trim();
                }
                if (qcysdt.Rows.Count > 0)
                {
                    qcysid = qcysdt.Rows[0]["F_yhbh"].ToString().Trim();
                }



                if (ReprotFile.Trim() == "")
                {
                    //����pdf����ת�ɶ����������ַ���

                    string jpgname = "";
                    string ML = DateTime.Parse(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");


                    if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                    {
                        #region  ����pdf

                        //string ML = "";
                        string message = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", ref message);

                        string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
                        if (isrtn)
                        {
                            if (debug == "1")
                                log.WriteMyLog("����PDF�ɹ�");
                            //�����ƴ�
                            if (File.Exists(jpgname))
                            {
                                try
                                {
                                    FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
                                    Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
                                    file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
                                    file.Close();
                                    ReprotFile = Convert.ToBase64String(imgByte);
                                    //   LGZGQClass.log.WriteMyLog("PDFת�������ƴ��ɹ�"); 

                                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                    if (ssa == true)
                                    {
                                        if (debug == "1")
                                            log.WriteMyLog("�ϴ�PDF�ɹ�");
                                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                        ZgqClass.BGHJ(blh, "�����ϴ�", "���", "����PDF�ɹ�:" + ML + "\\" + jpgname, "ZGQJK", "����PDF");
                                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "','" + jpgname + "')");
                                    }
                                    else
                                    {
                                        log.WriteMyLog("�ϴ�PDFʧ��");
                                    }
                                }
                                catch (Exception ee)
                                {
                                    log.WriteMyLog("PDFת�������ƴ�ʧ��");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("δ�ҵ��ļ�" + jpgname);
                                ZgqClass.BGHJ(blh, "�����ϴ�", "���", "δ�ҵ��ļ�" + jpgname, "ZGQJK", "����PDF");
                            }
                            zgq.DelTempFile(blh);

                        }
                        else
                        {
                            log.WriteMyLog("����PDFʧ�ܣ�" + message);
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        }

                        #endregion
                    }
                }



                if (ReprotFile.Trim() == "")
                {
                    exceptionmsg = "����pdf,δ��ת�ɶ�������,2Сʱ�����ش�";
                    log.WriteMyLog("����pdf,δ��ת�ɶ�������,2Сʱ�����ش�");
                    msg = "";
                    return false;
                }

                xml = xml + "<id root=\"S038\" extension=\"" + bgh + "\"/>";
                // <!-- �ĵ���ʶ-���� / �ĵ���ʶ-������ -->
                // <!-- �̶�ֵ -->
                //��Ҫȷ����ص�ֵ
                xml = xml + "<code code=\"04\" codeSystem=\"1.2.156.112649.1.1.60\" displayName=\"�������¼\"/>";
                //    <!-- �ĵ������ı� -->
                xml = xml + "<title>�����鱨��</title>";
                //<!-- �ĵ���Ч���� -->
                //��Ҫȷ����Ч���ڣ�������Ч�����ӳ���Ч��
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" />";
                // <!-- �ĵ��ܼ����� -->
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"2.16.840.1.113883.5.25\" codeSystemName=\"Confidentiality\" displayName=\"normal\" />";
                // <!-- �ĵ����Ա��� -->
                // <!-- �̶�ֵ -->
                xml = xml + "<languageCode code=\"zh-CN\" />";
                //��Ҫ����ȷ�������������޸�
                string bgzt = dt.Rows[0]["F_yl7"].ToString().Trim();
                if (bglx == "bc")
                {
                    bgzt = dt.Rows[0]["F_bc_yl7"].ToString().Trim();
                }
                string xzzt = "0";
                if (bgzt != "")
                {
                    xzzt = "1";
                }
                xml = xml + "<versionNumber value=\"" + xzzt + "\"/>";

                //<!-- �ĵ���¼���� -->
                xml = xml + "<recordTarget typeCode=\"RCT\">";
                //<!-- ������Ϣ -->
                xml = xml + "<patientRole classCode=\"PAT\">";
                //  <!-- ��ID -->
                if (dt.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                    xml = xml + "<id root=\"1.2.156.112649.1.2.1.2\" extension=\"03\" />";
                else if (dt.Rows[0]["F_brlb"].ToString().Trim() == "���")
                    xml = xml + "<id root=\"1.2.156.112649.1.2.1.2\" extension=\"04\" />";
                else
                    xml = xml + "<id root=\"1.2.156.112649.1.2.1.2\" extension=\"01\" />";
                // <!-- ����ID -->
                string patid = dt.Rows[0]["F_brbh"].ToString().Trim();
                xml = xml + "<id root=\"1.2.156.112649.1.2.1.3\" extension=\"" + patid + "\" />";
                //<!-- ����� -->
                //string jzh = dt.Rows[0]["F_MZH"].ToString();
                //xbrlb = "01";
                //if (dt.Rows[0]["F_brlb"].ToString() == "סԺ")
                //{
                //    xbrlb = "03";
                //    jzh = dt.Rows[0]["F_zyh"].ToString();
                //}

                xml = xml + "<id root=\"1.2.156.112649.1.2.1.12\" extension=\"" + dt_sqd.Rows[0]["F_jzh"].ToString() + "\" />";
                // <!-- Ӱ��� -->
                xml = xml + "<id root=\"1.2.156.112649.1.2.1.5\" extension=\"" + dt.Rows[0]["F_BLH"].ToString() + "\" />";
                // <!-- ����������Ϣ -->
                xml = xml + "<addr use=\"TMP\">";
                // <!-- ���� -->
                xml = xml + "<houseNumber>" + dt.Rows[0]["F_BQ"].ToString() + "</houseNumber>";
                // <!-- ��λ�� -->
                xml = xml + "<careOf>" + dt.Rows[0]["F_ch"].ToString() + "</careOf>";
                xml = xml + "</addr>";
                //<!-- ���˻�����Ϣ -->
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                //<!-- �������� -->
                xml = xml + "<name>" + dt.Rows[0]["F_xm"].ToString() + "</name>";
                //<!-- �Ա����/�Ա����� -->
                string xb = "";
                string xbdm = "0";
                if (dt.Rows[0]["F_xb"].ToString() == "��")
                {
                    xb = "��";
                    xbdm = "1";
                }
                if (dt.Rows[0]["F_xb"].ToString() == "Ů")
                {
                    xb = "Ů";
                    xbdm = "2";
                }
                xml = xml + "<administrativeGenderCode code=\"" + xbdm + "\" codeSystem=\"1.2.156.112649.1.1.3\" displayName=\"" + xb + "\" />";
                //<!-- �������� -->
                xml = xml + "<birthTime value=\"" + dt_sqd.Rows[0]["F_CSRQ"].ToString() + "\" />";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";
                //<!-- �ĵ�����(��鱨��ҽ��, ��ѭ��) -->
                xml = xml + "<author typeCode=\"AUT\">";
                //<!-- �������� -->
                //��������û��ʱ����
                xml = xml + "<time value=\"" + bgrq + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                //����ҽ����������
                //<!-- ����ҽ������ -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                //<!-- ����ҽ������ -->
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";
                //<!-- �ĵ�������(CDA��custodianΪ������) -->
                xml = xml + "<custodian>";
                xml = xml + "<assignedCustodian>";
                xml = xml + "<representedCustodianOrganization>";
                //<!-- ҽ�ƻ������� -->
                xml = xml + "<id root=\"1.2.156.112649\" extension=\"44643245-7\" />";
                //<!-- ҽ�ƻ������� -->
                xml = xml + "<name>�����е�һ����ҽԺ</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";
                //<!-- ����ǩ����Ϣ -->
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time />";
                xml = xml + "<signatureCode code=\"S\" />";
                xml = xml + "<assignedEntity>";
                //<!-- ����ǩ�º�-->";
                xml = xml + "<id extension=\"\" />";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";

                // <!-- �ĵ������(��鱨�����ҽʦ, ��ѭ��) -->
                xml = xml + "<authenticator>";
                //<!-- ������� -->
                string shsj = "";
                try
                {
                    shsj = Convert.ToDateTime(dt.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                }
                catch
                {
                }
                if (shsj == "")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                }
                if (bglx == "bc")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                xml = xml + "<time value=\"" + shsj + "\" />";
                xml = xml + "<signatureCode code=\"S\"/>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                //<!-- ���ҽ������ -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.2\" extension=\"" + shysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                //<!-- ���ҽ������ -->
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                //<!-- ����ҽ����Ϣ -->
                xml = xml + "<participant typeCode=\"AUT\">";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                //<!-- ����ҽ������ -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.2\" extension=\"\" />";
                xml = xml + "<associatedPerson>";
                //<!-- ����ҽ������ -->
                xml = xml + "<name>" + dt.Rows[0]["F_sjys"].ToString() + "</name>";
                xml = xml + "</associatedPerson>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";
                //<!-- ����ҽ����Ϣ -->
                xml = xml + "<inFulfillmentOf>";
                xml = xml + "<order>";
                //<!-- ����ҽ����(�ɶ��) -->
                xml = xml + "<id extension=\"" + dt.Rows[0]["F_yzid"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";
                //<!-- �ĵ���ҽ�������¼��ľ��ﳡ�� -->
                xml = xml + "<componentOf typeCode=\"COMP\">";
                //<!-- ������Ϣ -->
                xml = xml + "<encompassingEncounter classCode=\"ENC\" moodCode=\"EVN\">";
                //<!-- ������� -->
                xml = xml + "<id root=\"1.2.156.112649.1.2.1.7\" extension=\"" + dt_sqd.Rows[0]["F_jzcs"].ToString() + "\"/>";
                xml = xml + "<code /><effectiveTime />";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";
                //<!--
                //********************************************************
                //CDA Body
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<structuredBody>";
                //<!-- 
                //********************************************************
                //�ĵ��л��������Ϣ
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"34076-0\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Information for patients section\" />";
                xml = xml + "<title>�ĵ��л��������Ϣ</title>";
                //<!-- �������� -->
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"397669002\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"age\" />";
                xml = xml + "<value xsi:type=\"ST\">" + dt.Rows[0]["F_nl"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";

                //<!--
                //********************************************************
                //����½�
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"30954-2\" displayName=\"STUDIES SUMMARY\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\"/>";
                xml = xml + "<title>������</title>";
                // <!-- �����Ϣ -->
                xml = xml + "<entry>";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"310388008\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"relative information status\" />";
                xml = xml + "<statusCode code=\"completed\" />";
                //<!-- ��λͼ����Ϣ -->
                xml = xml + "<component>";
                xml = xml + "<supply classCode=\"SPLY\" moodCode=\"EVN\">";
                //<!-- ͼ��������(accessionNumber) -->
                xml = xml + "<id extension=\"" + "1001" + "\" />";
                xml = xml + "</supply>";
                xml = xml + "</component>";
                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //<!-- ��鱨�����ͱ�ʶ����/��鱨�����ͱ�ʶ���� --0�������棬1���䱨��>
                string bglxstr = "0";
                if (bglx == "bc")
                {
                    bglxstr = "1";
                }
                xml = xml + "<code code=\"" + bglxstr + "\" codeSystem=\"1.2.156.112649.1.1.112\" displayName=\"�����鱨��\" />";
                xml = xml + "</observation>";
                xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                ////<!--****************************************************************************-->
                ////<!-- ��鱨����Ŀ -->
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                //<!-- ������ͱ���/����������� --> 
                //������ͱ���û�У���������OT
                xml = xml + "<code code=\"OT\" codeSystem=\"1.2.156.112649.1.1.41\" displayName=\"����\" />";
                // <!-- ����̶��� -->
                xml = xml + "<statusCode code=\"completed\"/>";
                // <!-- ���ʹ���Լ���Ϣ -->
                xml = xml + "<participant typeCode=\"CSM\">";
                xml = xml + "<participantRole>";
                xml = xml + "<playingEntity>";
                //<!-- �Լ�����/�Լ����� -->
                xml = xml + "<code code=\"\" displayName=\"\" />";
                //<!-- �Լ���������λ -->
                xml = xml + "<quantity value=\"\" unit=\"\" />";
                xml = xml + "</playingEntity>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";
                //     <!-- study 1 -->
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //!-- �����Ŀ����/�����Ŀ���� -->
            
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_yzxmbm"].ToString() + "\" codeSystem=\"1.2.156.112649.1.1.88\" displayName=\"" + dt_sqd.Rows[0]["F_yzxmmc"].ToString() + "\"/>";
                //<!-- ��鱸ע -->
                xml = xml + "<text></text>";
                // <!-- ����̶��� -->
                xml = xml + "<statusCode code=\"completed\"/>";
                //<!-- ��鱨����-�͹�����/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112649.1.1.98\">";
                xml = xml + "<originalText> " + System.Security.SecurityElement.Escape(dt.Rows[0]["F_RYSJ"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_JXSJ"].ToString()) + "</originalText>";
                xml = xml + "</value>";
                // <!-- ��鱨����-������ʾ/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112649.1.1.98\">";
                string blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_blzd"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_TSJC"].ToString()); ;
                if (bglx == "bc")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bczd"].ToString());
                }
                xml = xml + "<originalText>" + blzd + "</originalText>";
                xml = xml + "</value>";
                //<!-- ��鷽������/��鷽������ -->
                xml = xml + "<methodCode code=\"001\"  codeSystem=\"1.2.156.112649.1.1.43\" displayName=\"\"/>";
                //<!-- ��鲿λ����/��鲿λ���� -->
                xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156112649.1.1.42\" displayName=\"\" />";
                // <!-- ���ҽʦ��Ϣ -->
                xml = xml + "<performer typeCode=\"PRF\">";
                //<!-- ������� -->
                xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                xml = xml + "<assignedEntity>";
                //<!-- ���ҽ������ -->
                xml = xml + "<id  root=\"1.2.156.112649.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                //<!-- ���ҽ������ -->
                xml = xml + "<name>" + dt.Rows[0]["F_bgys"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                // <!-- ��Ͽ��ұ��� -->
                //���ṩ׼ȷ�Ĳ���ƵĴ���,���û�о�ɾ��
                xml = xml + "<id root=\"1.2.156.112649.1.1.1\" extension=\"2070000\"/>";
                //<!-- ��Ͽ������� -->
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                //<!-- ���ҽʦ��Ϣ -->
                xml = xml + "<performer>";
                // <!-- ������� -->
                try
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                xml = xml + "<assignedEntity>";
                //<!-- ���ҽ������ -->
                xml = xml + "<id  root=\"1.2.156.112649.1.1.2\" extension=\"" + qcysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                // <!-- ���ҽ������ -->
                xml = xml + "<name>" + dt.Rows[0]["F_QCYS"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //<!-- �����ұ��� -->
                xml = xml + "<id root=\"1.2.156.112649.1.1.1\" extension=\"2070000\"/>";
                //<!-- ���������� -->
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                ////<!-- ������Ϣ -->
                ////<participant typeCode="DEV">
                ////<participantRole>
                ////<playingDevice>
                ////<!--�����ͺ� ��������-->
                ////<manufacturerModelName code="LOGIQ-9"  displayName="Agilent Mx3000P"/>
                ////</playingDevice>
                ////</participantRole>
                ////</participant>	

                ////<!-- ������ҽ���͹�������Ϣ(�����Ķ�����Ƚṹ���������ֵ���Ϣ) -->
                ////<entryRelationship typeCode="COMP">
                ////<organizer classCode="BATTERY" moodCode="EVN">
                ////<code code="365605003" codeSystem="2.16.840.1.113883.6.96" codeSystemName="SNOMED CT" displayName="body measurement finding" />
                ////<statusCode code="completed" />

                ////<!-- ��Ŀ��Ϣ(��ѭ��) -->
                ////<component>
                ////<observation classCode="OBS" moodCode="EVN">
                ////<code code="100" displayName="AOD" />
                ////<!--<value xsi:type="SC">1mm</value>-->
                ////<value xsi:type="PQ" value="73" unit="����"></value>
                ////</observation>
                ////</component>

                ////<component>
                ////<observation classCode="OBS" moodCode="EVN">
                ////<code code="200" displayName="LAD" />
                ////<value xsi:type="SC">1mm</value>
                ////</observation>
                ////</component>
                ////<component>
                ////<observation classCode="OBS" moodCode="EVN">
                ////<code code="300" displayName="FS" />
                ////<value xsi:type="SC">33.3%</value>
                ////</observation>
                ////</component>	
                ////<!-- ������Ϣ�������ʽ��� -->
                ////</organizer>
                ////</entryRelationship>

                // <!-- ͼ����Ϣ(������Ŀ��Ӧ��ͼ��) -->
                //xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                //xml = xml + "<observationMedia   classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/jpg\">" ;
                //xml = xml + "</value>";
                //xml = xml + "</observationMedia>";
                //xml = xml + "</entryRelationship>";
                //<!-- ���ж��Ӱ���Ӧͬһ��studyʱ,���Ը��ô�entryRelationship -->
                xml = xml + "</observation></component>";
                ////<!-- study 2 -->
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"1\" codeSystem=\"1.2.156.112649.1.1.88\" displayName=\"\"/>";
                xml = xml + "<text></text>";
                xml = xml + "<statusCode code=\"completed\"/>";

                //        <!-- ��鱨����-�͹�����/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112649.1.1.98\">";
                xml = xml + "<originalText></originalText>";
                xml = xml + "</value>";
                //        <!-- ��鱨����-������ʾ/Ӱ��ѧ����(�ܹ�����Ŀ��Ӧʱ����д�� - @code:01��ʾ�͹�����, 02��ʾ������ʾ) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112649.1.1.98\">";
                xml = xml + "<originalText></originalText>";
                xml = xml + "</value>";
                //        <!-- ��鷽������/��鷽������ -->
                xml = xml + "<methodCode code=\"002\"  codeSystem=\"1.2.156.112649.1.1.43\" displayName=\"\"/>";
                //        <!-- ��鲿λ����/��鲿λ���� -->
                xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156.112649.1.1.42\" displayName=\"\" />";
                //        <!-- ���ҽʦ��Ϣ -->
                xml = xml + "<performer>";
                xml = xml + "<time value=\"201112310910\"/>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<id  root=\"1.2.156.112649.1.1.2\" extension=\"9879\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112649.1.1.1\" extension=\"98712\"/>";
                xml = xml + "<name></name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";




                xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                //  <!-- Ӱ����Ϣ(Ҫ�����ΪBASE64), @mediaType: Ӱ���ʽ -->

                if(debug=="1")
                    xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/pdf\">" + "1234567890";
                else
                xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/pdf\">" + ReprotFile;
                xml = xml + "</value>";
                xml = xml + "</observationMedia>";
                xml = xml + "</entryRelationship>";
                // <!-- ���ж��Ӱ���Ӧͬһ��studyʱ,���Ը��ô�entryRelationship -->
                xml = xml + "</observation>";
                xml = xml + "</component>";

                //<!-- ������Ŀ������ṹ�͸�ʽ��� -->

                //<!-- ��ϵͳ�����ɵı�����,ͼ���޷�������study����Ӧʱ,ʹ�����²���������Ӱ�� -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- Ӱ����Ϣ(Ҫ�����ΪBASE64), @mediaType: Ӱ���ʽ -->
                // xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/gif\">";
                // xml = xml + "</value>";
                // xml = xml + "</observationMedia>";
                // xml = xml + "</component>";

                ////<!-- ���ж��Ӱ��ʱ,�������ϸ�ʽ��� -->
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";

                // <!-- ��ϵͳ��,�͹�����(���������)�޷���Ӧ�������study, 
                // ���Ƕ��study�Ŀ͹�����(���������)��¼��ͬһ���ı��ֶ���,
                // ʹ�����²��������ÿ͹�������������� -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // //<!-- @code:01��ʾ�͹�����, 02��ʾ������ʾ -->
                // xml = xml + "<code code=\"01\" codeSystem=\"1.2.156.112649.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //// <!-- @code:01��ʾ�͹�����, 02��ʾ������ʾ -->
                // xml = xml + "<code code=\"02\" codeSystem=\"1.2.156.112649.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";
                //********************************************************
                //�ٴ�����
                //* *******************************************************
                xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code /><value xsi:type=\"ED\"></value></observation></entry></section></component>";
                //<!-- 
                //****************************************************************************
                //  #ҩ���½�
                //****************************************************************************
                //-->
                xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code code=\"123\" displayName=\"ҩ������\"/></observation></entry></section></component>";
                //<!-- 
                //********************************************************
                //���
                //********************************************************
                //-->
                xml = xml + "<component><section><code code=\"29308-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Diagnosis\"/> "
                                   + "<title>���</title>"
                                   + "<entry typeCode=\"DRIV\">"
                                       + "<act classCode=\"ACT\" moodCode=\"EVN\">"
                                           + "<code nullFlavor=\"NA\"/>"
                                           + "<entryRelationship typeCode=\"SUBJ\">"
                                               + "<observation classCode=\"OBS\" moodCode=\"EVN\">"
                    // <!-- ���������/���������� -->
                                                   + "<code code=\"\" codeSystem=\"1.2.156.112635.1.1.29\" displayName=\"\" />"
                                                   + "<statusCode code=\"completed\"/>"
                    // <!-- ��������/��������(û�б���ȥ��@code) -->
                                                   + "<value xsi:type=\"\" code=\"\" codeSystem=\"1.2.156.112635.1.1.30\" displayName=\"\" />"
                                              + " </observation>"
                                           + "</entryRelationship>"
                                       + "</act>"
                                  + "</entry>"
                               + "</section>"
                           + "</component>";

                //<!--
                //********************************************************
                //��������½ڣ�TCT��鱨�浥�ã�
                //********************************************************
                //-->
                //            <component>
                //                <section>
                //                    <code code="52535-2" codeSystem="2.16.840.1.113883.6.1" codeSystemName="LOINC" displayName="Other useful information" />
                //                    <!-- �½ڱ��� -->
                //                    <title>���������Ϣ</title> 
                //                    <!-- ����������� -->
                //                    <text>�����������</text>
                //                </section>
                //            </component>

                xml = xml + "</structuredBody>";
                xml = xml + "</component>";

                xml = xml + "</ClinicalDocument>";
                msg = xml;

                return true;
            }
            catch (Exception ee)
            {
                exceptionmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                msg = "";
                return false;

            }
        }

        public string fsbg_Web(string message,string brlb)
        {

       
            try
            {
            cdrmyymq.Service mq = new PathHISZGQJK.cdrmyymq.Service();
            string rtn=  mq.MQSendMessage(message, "BS320", brlb, "IN.BS320.LQ");

            return rtn;
                       
            }
            catch (Exception ex)
            {
                log.WriteMyLog("��Ϣ�����쳣��" + ex.Message);
                return "ERR:" + ex.Message;
            }

        }
        public string fsbg(string message, string brlb)
        {


            //try
            //{

            //    string rtn = MQSendMessage(message, "BS320", brlb, "IN.BS320.LQ");

            //    return rtn;

            //}
            //catch (Exception ex)
            //{
            //     LGZGQClass.log.WriteMyLog("��Ϣ�����쳣��" + ex.Message);
            //    return "ERR:" + ex.Message;
            //}

            return "";

        }
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        public string fszt_Web(string blh,string message,string brlb, string debug)
        {
         
                    try
                    {
                        cdrmyymq.Service mq = new PathHISZGQJK.cdrmyymq.Service();
                        string rtn_msg = mq.MQSendMessage(message, "BS004", brlb, "IN.BS004.LQ");
                        return rtn_msg;
                    }
                    catch (Exception ex)
                    {
                     return  "ERR:" + ex.Message;
                      
                    }
        }
        public string fszt(string blh, string message, string brlb, string debug)
        {

            //try
            //{
       
            //    string rtn_msg = MQSendMessage(message, "BS004", brlb, "IN.BS004.LQ");
            //    return rtn_msg;
            //}
            //catch (Exception ex)
            //{
            //    return "ERR:" + ex.Message;

            //}

            return "";
        }

        public bool bgzt_msg(DataTable dt_jcxx, DataTable dt_sqdxx, ref string msg, ref string xbrlb, string blh)
        {
            xbrlb = dt_sqdxx.Rows[0]["F_y_brlb"].ToString();
            string bgztbm = "";
            string bgztstr = "";
            string  bgzt=dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim();
            if (bgzt == "�����")
            {
                bgztbm = "92";
                bgztstr = "��鱨�������";
            }
            if (bgzt == "��д����")
            {
                bgztbm = "73";
                bgztstr = "��������";
            }
            if (bgzt == "��������")
            {
                bgztbm = "73";
                bgztstr = "��������";
            }
            if (bgzt == "��ȡ��")
            {
                bgztbm = "51";
                bgztstr = "����ѵ���";
            }
            if (bgzt == "�ѵǼ�")
            {
                bgztbm = "51";
                bgztstr = "����ѵ���";
            }
            if (bgzt == "ȡ�����")
            {
                bgztbm = "100";
                bgztstr = "�����ٻ�";
            }

            if (bgztstr == "" || bgztbm.Trim() == "")
            {
                msg = "";
                return false;
            }
            try
            {
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3  ../../Schemas/POOR_IN200901UV23.xsd\">";

                xml = xml + "<!-- ��ϢID -->";
                xml = xml + "<id extension=\"BS004\" />";
                xml = xml + "<!-- ��Ϣ����ʱ�� -->";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<interactionId extension=\"POOR_IN200901UV23\" />";
                xml = xml + "<!--��Ϣ��;: P(Production); D(Debugging); T(Training) -->";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<!-- ��Ϣ����ģʽ: A(Archive); I(Initial load); R(Restore from archive); T(Current  processing) -->";
                xml = xml + "<processingModeCode code=\"R\" />";
                xml = xml + "<!-- ��ϢӦ��: AL(Always); ER(Error/reject only); NE(Never) -->";
                xml = xml + "<acceptAckCode code=\"NE\" />";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ������ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ������ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"S007\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";
                xml = xml + "<!-- ��װ����Ϣ����(��Excel��д) -->";
                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<!-- ��Ϣ�������� @code: ���� :new �޸�:update -->";
                xml = xml + "<code code=\"update\"></code>";
                xml = xml + "<subject typeCode=\"SUBJ\" xsi:nil=\"false\">";
                xml = xml + "<placerGroup>";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<code></code>";
                xml = xml + "<!-- �������뵥״̬ ������δʹ�� -->";
                xml = xml + "<statusCode code=\"active\"></statusCode>";
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<id>";
                xml = xml + "<!--��ID -->";
                xml = xml + "<item root=\"1.2.156.112649.1.2.1.2\" extension=\"" + "" + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<item root=\"1.2.156.112649.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item root=\"1.2.156.112649.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_JZH"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"";
                xml = xml + "determinerCode=\"INSTANCE\">";
                xml = xml + "<!--���˿��ұ���-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_KSDM"].ToString() + "\" root=\"1.2.156.112649.1.1.1\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--���˿������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_KSMC"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--ҽ�ƻ������� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_YYDM"].ToString() + "\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--ҽ�ƻ������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item><part value=\"" + dt_sqdxx.Rows[0]["F_YYMC"].ToString() + "\" /></item>";
                xml = xml + "</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</providerOrganization>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<time>";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</time>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- �����˱��� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"01003\" root=\"1.2.156.112649.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\"";
                xml = xml + "classCode=\"PSN\">";
                xml = xml + "<!-- ���������� ��������ʹ�� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"����\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<!--ִ�п��� -->";
                xml = xml + "<location typeCode=\"LOC\" xsi:nil=\"false\">";
                xml = xml + "<!--������δʹ�� -->";
                xml = xml + "<time />";
                xml = xml + "<!--�������/���� -->";
                xml = xml + "<serviceDeliveryLocation classCode=\"SDLOC\">";
                xml = xml + "<serviceProviderOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--ִ�п��ұ��� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ZXKSDM"].ToString() + "\" root=\"1.21.156.112649.1.1.1\" />";
                xml = xml + "</id>";
                xml = xml + "<!--ִ�п������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + "�����" + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</serviceDeliveryLocation>";
                xml = xml + "</location>";
                xml = xml + "<!-- 1..n��ѭ��  ҽ��״̬��Ϣ -->";
                xml = xml + "<component2>";
                xml = xml + "<!--ҽ�����-->";
                xml = xml + "<sequenceNumber value=\"1\"/>";
                xml = xml + "<observationRequest classCode=\"OBS\">";
                xml = xml + "<!-- ��������ʹ�� -->";
                xml = xml + "<id>";
                xml = xml + "<!-- ҽ���� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_YZH"].ToString() + "\" root=\"1.2.156.112649.1.2.1.22\"/>";
                xml = xml + "<!-- ���뵥�� -->";
                xml = xml + "<item extension=\"" + dt_jcxx.Rows[0]["F_SQXH"].ToString()+"\" root=\"1.2.156.112649.1.2.1.21\"/>";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item extension=\"" + blh + "\" root=\"1.2.156.112649.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112649.1.2.1.30\"/>";
                xml = xml + "</id>";
                xml = xml + "<!-- ҽ��������/ҽ��������� - ���ҩƷ, ������, ������, Ƭ��ҩƷ, ������ -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112649.1.1.27\">";
                xml = xml + "<displayName value=\"���\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";
                xml = xml + "<!-- �걾��Ϣ -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                xml = xml + "<specimen classCode=\"SPEC\">";
                xml = xml + "<!--�걾����� ��������ʹ�� -->";
                xml = xml + "<id extension=\"" + "" + "\" />";
                xml = xml + "<!--������Ŀδʹ�� -->";
                xml = xml + "<code />";
                xml = xml + "<subjectOf1 typeCode=\"SBJ\" contextControlCode=\"OP\">";
                xml = xml + "<specimenProcessStep moodCode=\"EVN\"";
                xml = xml + "classCode=\"SPECCOLLECT\">";
                xml = xml + "<!-- �ɼ����� -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</effectiveTime>";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- �ɼ���Id -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"01003\" root=\"1.2.156.112649.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\"";
                xml = xml + "classCode=\"PSN\">";
                xml = xml + "<!-- �ɼ������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"����\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "</specimenProcessStep>";
                xml = xml + "</subjectOf1>";
                xml = xml + "</specimen>";
                xml = xml + "</specimen>";
                xml = xml + "<!-- ԭ�� -->";
                xml = xml + "<reason contextConductionInd=\"true\">";
                xml = xml + "<observation moodCode=\"EVN\" classCode=\"OBS\">";
                xml = xml + "<!-- ������ δʹ��-->";
                xml = xml + "<code></code>";
                xml = xml + "<value xsi:type=\"ST\" value=\"�Ǽ�\"/>";
                xml = xml + "</observation>";
                xml = xml + "</reason>";
                xml = xml + "<!-- ҽ��ִ��״̬ -->";
                xml = xml + "<component1 contextConductionInd=\"true\">";
                xml = xml + "<processStep classCode=\"PROC\">";
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112649.1.1.93\">";
                xml = xml + "<!--ҽ��ִ��״̬���� -->";
                xml = xml + "<displayName value=\"" + bgztstr + "\" />";
                xml = xml + "</code>";
                xml = xml + "</processStep>";
                xml = xml + "</component1>";
                xml = xml + "</observationRequest>";
                xml = xml + "</component2>";
                xml = xml + "<!--���� -->";
                xml = xml + "<componentOf1 contextConductionInd=\"false\" xsi:nil=\"false\"";
                xml = xml + "typeCode=\"COMP\">";
                xml = xml + "<!--���� -->";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<id>";
                xml = xml + "<!-- ������� ��������ʹ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112649.1.2.1.7\" />";
                xml = xml + "<!-- ������ˮ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112649.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--����������-->";
                xml = xml + "<code codeSystem=\"1.2.156.112649.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
                xml = xml + "<!-- ����������� -->";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_BRLB"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!--������δʹ�� -->";
                xml = xml + "<statusCode code=\"Active\" />";
                xml = xml + "<!--���� ������δʹ�� -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\" />";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf1>";
                xml = xml + "</placerGroup>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</POOR_IN200901UV>";
                msg = xml;

                return true;
            }
            catch(Exception  ex2)
            {
                msg = "ERR:"+ex2.Message; return false;
            }
        }

        //public string MQSendMessage(string txt, string service_id, string brlb, string queueManager)
        //{
        //    MQQueueManager qMgr = null;
        //    Hashtable env = new Hashtable();

        //string hostname = "172.16.13.42";
        //string channel = "IE.SVRCONN";
        //string qManager = "GWI.QM";
     
       
        //    hostname = f.ReadString("MQSERVER", "hostname", "172.16.13.42").Replace("\0", "");
        //    channel = f.ReadString("MQSERVER", "channel", "IE.SVRCONN").Replace("\0", "");
        //    qManager = f.ReadString("MQSERVER", "qManager", "GWI.QM").Replace("\0", "");
        // //   queueManager = f.ReadString("MQSERVER", "queueManager", "IN.BS320.LQ").Replace("\0", "");

        //    try
        //    {
        //        env.Clear();
        //        env.Add(IBM.XMS.MQC.HOST_NAME_PROPERTY, hostname);
        //        env.Add(IBM.XMS.MQC.CHANNEL_PROPERTY, channel);
        //        env.Add(IBM.XMS.MQC.CCSID_PROPERTY, 1208);
        //        env.Add(IBM.XMS.MQC.PORT_PROPERTY, 6000);
        //        env.Add(IBM.XMS.MQC.TRANSPORT_PROPERTY, IBM.XMS.MQC.TRANSPORT_MQSERIES);
        //        env.Add(IBM.XMS.MQC.USER_ID_PROPERTY, "mqm");


        //        int openOptions = MQC.MQOO_OUTPUT
        //                | MQC.MQPMO_PASS_ALL_CONTEXT;
        //        MQMessage msg = new MQMessage();
        //        //���Ӷ��й�����
        //        qMgr = new MQQueueManager(qManager, env);
        //        msg.CharacterSet = 1208;
        //        MQQueue queue = qMgr.AccessQueue(queueManager, openOptions);
        //        msg.Format = MQC.MQFMT_STRING;

        //        // 8����Ϣͷ
        //        /// ��ϢID   Y
        //        msg.SetStringProperty("service_id", service_id);
        //        //�������ID  Y
        //        //01 ����,02 ����,03 סԺ,04 ���,05 תԺ
        //        if (brlb == "����") brlb = "02";
        //        if (brlb == "����") brlb = "01";
        //        if (brlb == "סԺ") brlb = "03";
        //        if (brlb == "���") brlb = "04";
        //        if (brlb == "תԺ") brlb = "05";

        //        msg.SetStringProperty("domain_id", brlb);
        //        // �������ID   YY
        //        msg.SetStringProperty("apply_unit_id", "0");  //���԰���ʵ���������ID��д��Ҳ������д0
        //        // ����ϵͳID   YY
        //        msg.SetStringProperty("send_sys_id", "S007");  //ÿ��ϵͳ����һ��ϵͳ��ţ������ϵͳID��S007���ǹ̶��ġ�
        //        // ҽ�ƻ�������  YY
        //        msg.SetStringProperty("hospital_id", "44643245-7"); //������һҽԺ��ҽ�ƻ������룬�ǹ̶�ֵ
        //        // ִ�п���ID  Y
        //        msg.SetStringProperty("exec_unit_id", "2070000");  //��ִ�п��ұ��룬��Ҫ����ʵ����д
        //        // ҽ��ִ�з������  
        //        /*	0201	������飨LIS��
        //            0101	�������
        //            0102	��ҽѧ���
        //            0103	������
        //            0104	������
        //            0105	�ڿ������
        //            0106	�ĵ���
        //            0107	���׾����
        //            0108	�����Ķ����
        //            0202	ѪҺ��̬ѧ����
        //            0301	��������
        //            0601	����
        //            0	δ���� */
        //        msg.SetStringProperty("order_exec_id", "0");  //����Ԥ���ֶΣ���ʱ������0
        //        // ��չ��   ����ֶ����ڱ�ʶ�շ�
        //        //msg.setStringProperty("extend_sub_id", "1");  //�շѺ�����ֶ���1���ǹ̶�ֵ

        //        string aa = txt;  //��ʾ�Ὣ��XML��Ϣ������ƽ̨
        //        msg.WriteString(aa);
        //        MQPutMessageOptions pmo = new MQPutMessageOptions();
        //        pmo.Options = MQC.MQPMO_SYNCPOINT;
        //        queue.Put(msg, pmo);
        //        queue.Close();
        //        qMgr.Commit();
        //        string messageid = byteToHexStr(msg.MessageId);
        //        qMgr.Disconnect();
        //        return messageid;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "ERR:" + ex.Message;
        //    }
        //}
        
    }
}
