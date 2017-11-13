using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using ZgqClassPub;


namespace PathHISZGQJK
{
    /// <summary>
    /// �㶫ʡ�ڶ�����ҽԺ,�����κ�ƽ̨
    /// </summary>
    class gdsdermyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

        public void pathtohis(string blh, string bglx, string bgxh, string czlb, string dz, string msg, string debug)
        {

            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            string hczt = ZgqClass.GetSz("savetohis", "hczt", "0").Replace("\0", "").Trim();
            string hcbg = ZgqClass.GetSz("savetohis", "hcbg", "1").Replace("\0", "").Trim();
            string ptjk = ZgqClass.GetSz("savetohis", "ptjk", "1").Replace("\0", "").Trim();
            string wsurl = ZgqClass.GetSz("savetohis", "wsurl", "").Replace("\0", "").Trim();
            string updatefsb = ZgqClass.GetSz("savetohis", "updatefsb", "1").Replace("\0", "").Trim();

            DataTable bljc = new DataTable();
            DataTable dt_bcbg = new DataTable();
            DataTable dt_bdbg = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "1";

            string blbh = blh + bglx + bgxh;

            if (bljc == null)
            {
                log.WriteMyLog("���ݿ������쳣");
                return;
            }
            if (bljc.Rows.Count <= 0)
            {
                log.WriteMyLog("δ��ѯ���˱���" + blh);
                return;
            }
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog("�����뵥�Ų�����");
                 aa.ExecuteSQL("update T_jcxx_fs set F_bz='�����뵥�Ų�����',F_fszt='������'  where F_blbh='" + blbh + "' ");
                return;
            }

            /////////////////////////////////////////////////
     

            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();

            if (bglx == "bc")
            {
                dt_bcbg = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "' and F_bc_bgxh='" + bgxh + "'", "bcbg");
                if (dt_bcbg == null || dt_bcbg.Rows.Count <= 0)
                {
                    log.WriteMyLog("δ��ѯ���˱��油�䱨�棺" + blh + "^" + bgxh);
                    if (updatefsb == "1")
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='δ��ѯ���˱��油�䱨�棺" + blh + "^" + bgxh + "' where F_blbh='" + blbh + "'  and F_fszt='δ����'");
                    return;
                }

                bgzt = dt_bcbg.Rows[0]["F_bc_BGZT"].ToString().Trim();
            }
            if (bglx == "bd")
            {
                dt_bdbg = aa.GetDataTable("select * from T_bdbg where F_blh='" + blh + "' and F_bd_bgxh='" + bgxh + "'", "bcbg");
                if (dt_bdbg == null || dt_bdbg.Rows.Count <= 0)
                {
                    log.WriteMyLog("δ��ѯ���˱���������棺" + blh + "^" + bgxh);
                    if (updatefsb == "1")
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='δ��ѯ���˱���������棺" + blh + "^" + bgxh + "' where F_blbh='" + blbh + "' and F_fszt='δ����' ");
                    return;
                }

                bgzt = dt_bdbg.Rows[0]["F_bd_BGZT"].ToString().Trim();
            }

            if (dz == "qxsh")
                bgzt = "ȡ�����";

            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim();
            string brlb = bljc.Rows[0]["f_brlb"].ToString().Trim();

            //////////�޸����뵥�Ǽ�״̬//////////////////////////////////////////////

            //DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + sqxh + "'", "t_sqd");
            //if (dt_sqd.Rows.Count >= 1)
            //{
            //    if (dt_sqd.Rows[0]["F_SQDZT"] != "�ѵǼ�")
            //    {
            //        aa.ExecuteSQL("update T_SQD set F_SQDZT='�ѵǼ�' where F_sqxh='" + sqxh + "'");
            //    }
            //}
            ////if (bgzt.Trim() == "ȡ�����")
            ////{
            ////    string message = BgMsg(bljc, dt_bcbg, dt_bdbg, blh, bglx, bgxh, pdfpath, ref errmsg,bgzt);

            ////    if (message == "")
            ////    {
            ////         LGZGQClass.log.WriteMyLog("��������XMlʧ��");
            ////        if (updatefsb == "1")
            ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='��������XMLʧ�ܣ�'" + errmsg + "' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
            ////        return;
            ////    }

            ////    if (debug == "1")
            ////         LGZGQClass.log.WriteMyLog("���淢��XML��" + message);

            ////    gds2websvr.PacsWS gds2 = new gds2websvr.PacsWS();
            ////    try
            ////    {
            ////        if (wsurl != "")
            ////            gds2.Url = wsurl;
            ////        string trnmsg = gds2.PacsReportBack(message);

            ////        if (debug == "1")
            ////             LGZGQClass.log.WriteMyLog("���أ�" + trnmsg);
            ////        try
            ////        {
            ////            XmlNode xmlok = null;
            ////            XmlDocument xd = new XmlDocument();
            ////            try
            ////            {
            ////                xd.LoadXml(trnmsg);
            ////                xmlok = xd.SelectSingleNode("/RESPONSE");
            ////            }
            ////            catch
            ////            {
            ////                if (debug == "1")
            ////                    MessageBox.Show("XML��������");
            ////                return;
            ////            }

            ////            if (xmlok["RESULT_CODE"].InnerText == "true")
            ////            {
            ////                if (debug == "1")
            ////                     LGZGQClass.log.WriteMyLog("����ȡ������ϴ�ƽ̨��" + xmlok["RESULT_CONTENT"].InnerText);
            ////                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "',F_fszt='�Ѵ���'  where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
            ////                return;
            ////            }
            ////            else
            ////            {
            ////                 LGZGQClass.log.WriteMyLog("����ȡ������ϴ�ƽ̨��" + xmlok["RESULT_CONTENT"].InnerText);
            ////                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
            ////                return;
            ////            }
            ////        }
            ////        catch
            ////        {
            ////        }

            ////    }
            ////    catch (System.Web.Services.Protocols.SoapException ex)
            ////    {
            ////         LGZGQClass.log.WriteMyLog(ex.Detail["error"]["text"].InnerText.ToString());
            ////        if (msg == "1")
            ////            MessageBox.Show(ex.Detail["error"]["text"].InnerText.ToString());

            ////        if (updatefsb == "1")
            ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='ȡ����˷���ʧ��:" + ex.Detail["error"]["text"].InnerText.ToString() + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='ȡ�����'");

            ////    }
            ////    catch (Exception ee)
            ////    {
            ////         LGZGQClass.log.WriteMyLog("����web�����쳣��" + ee.Message);
            ////        if (msg == "1")
            ////            MessageBox.Show("����web�����쳣��" + ee.Message);

            ////        if (updatefsb == "1")
            ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='����web�����쳣��" + ee.Message + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='ȡ�����'");

            ////    }

            ////}
            if (bgzt.Trim() == "ȡ�����")
            {
                aa.ExecuteSQL("delete  from T_BG_PDF  where F_blbh='" + blbh + "' ");
                aa.ExecuteSQL("delete  from T_BG_PDF_CA  where F_blbh='" + blbh + "' ");
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='',F_fszt='�Ѵ���'  where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");

            }
            if (bgzt.Trim() == "�����" || bgzt.Trim() == "�ѷ���")
            {

                string ispdf = ZgqClass.GetSz("savetohis", "ispdf", "0").Replace("\0", "").Trim();

                 DataTable dt_pdf = new DataTable();
                 string pdfpath = ""; string errmsg = "";
                 if (ispdf == "1")
                 {
                     try
                     {
                         dt_pdf = aa.GetDataTable("select * from T_BG_PDF where F_BLBH='" + blbh + "'", "dt_sqd");
                     }
                     catch (Exception ex)
                     {
                         log.WriteMyLog(ex.Message.ToString());
                         return;
                     }
                   
                     if (dt_pdf.Rows.Count <= 0)
                     {
                         string Base64String = "";
                         #region  ����pdf
                         string ML = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");

                         string filename = bljc.Rows[0]["F_SPARE5"].ToString();
                         if (bglx == "bc")
                             filename = dt_bcbg.Rows[0]["F_bc_SPARE5"].ToString();
                         if (bglx == "bd")
                             filename = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString();

                         filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";

                         C_PDF(blh, bgxh, bglx, ML, ref filename, ref pdfpath, false, ref Base64String, debug, ref errmsg);

                         if (pdfpath == "")
                             log.WriteMyLog("����pdfʧ��");
                         #endregion
                     }
                     else
                     {
                         pdfpath = dt_pdf.Rows[0]["F_FilePath"].ToString().Trim();
                     }
                 }



                if (hcbg.Trim() == "1" && ptjk=="1")
                {
                    if (ispdf == "1")
                    {
                        if (pdfpath == "")
                        {
                            log.WriteMyLog("pdf·��Ϊ��");
                            if (updatefsb == "1")
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='pdf·��Ϊ��' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='�����'");
                            return;
                        }
                       pdfpath= pdfpath.Replace("ftp", "http");
                    }
                    else
                    {
                        pdfpath = "http://192.168.3.153/pathwebrpt/index_z.asp?sqxh=" + bljc.Rows[0]["F_SQXH"].ToString();
                        if (bglx == "bd")
                        pdfpath = "http://192.168.3.153/pathwebrpt/����.asp?blh="+blh;
                        if (bglx == "bc")
                        pdfpath = "http://192.168.3.153/pathwebrpt/����.asp?blh="+blh;
                    }
                    string message = BgMsg(bljc, dt_bcbg, dt_bdbg, blh, bglx, bgxh, pdfpath, ref errmsg);

                    if (message == "")
                    {
                        log.WriteMyLog("��������XMlʧ��");
                        if (updatefsb == "1")
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='��������XMLʧ�ܣ�" + errmsg + "' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='�����'");
                        return;
                    }

                    if (debug == "1")
                        log.WriteMyLog("���淢��XML��" + message);

                    gds2websvr.PacsWS gds2 = new gds2websvr.PacsWS();

                    try
                    {
                        if (wsurl != "")
                            gds2.Url = wsurl;
                        string trnmsg = gds2.PacsReportBack(message);

                        if (debug == "1")
                            log.WriteMyLog("���أ�" + trnmsg);


                        try
                        {
                            XmlNode xmlok = null;
                            XmlDocument xd = new XmlDocument();
                            try
                            {
                                xd.LoadXml(trnmsg);
                                xmlok = xd.SelectSingleNode("/RESPONSE");
                            }
                            catch
                            {
                                if (debug == "1")
                                    MessageBox.Show("XML��������");
                                return;
                            }

                            if (xmlok["RESULT_CODE"].InnerText == "true")
                            {
                                if (debug == "1")
                                    log.WriteMyLog("�����ϴ�ƽ̨��" + xmlok["RESULT_CONTENT"].InnerText);
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "',F_fszt='�Ѵ���'  where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='�����'");
                                return;
                            }
                            else
                            {
                                log.WriteMyLog("�����ϴ�ƽ̨��" + xmlok["RESULT_CONTENT"].InnerText);
                            
                                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + xmlok["RESULT_CONTENT"].InnerText + "' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='�����'");
                                return;
                            }
                        }
                        catch
                        {
                        }

                    }
                    catch (System.Web.Services.Protocols.SoapException ex)
                    {
                        log.WriteMyLog(ex.Detail["error"]["text"].InnerText.ToString());
                        if (msg == "1")
                            MessageBox.Show(ex.Detail["error"]["text"].InnerText.ToString());

                        if (updatefsb == "1")
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='���淢��ʧ��:" + ex.Detail["error"]["text"].InnerText.ToString() + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='�����'");

                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("����web�����쳣��" + ee.Message);
                        if (msg == "1")
                            MessageBox.Show("����web�����쳣��" + ee.Message);

                        if (updatefsb == "1")
                            aa.ExecuteSQL("update T_jcxx_fs set F_bz='����web�����쳣��" + ee.Message + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='�����'");

                    }
                }
            }


        }

        public string BgMsg(DataTable dt_brxx, DataTable dt_bcbg, DataTable dt_bdbg, string blh, string bglx, string bgxh, string pdfpath, ref string errmsg)
        {
            try
            {
                string  bglxmc="����";
                string bgzt = dt_brxx.Rows[0]["F_BGZT"].ToString().Trim();
                string qcrq = dt_brxx.Rows[0]["F_qcrq"].ToString().Trim();
                if (qcrq == "")
                    qcrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();

                string sdrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();
                string bgrq = dt_brxx.Rows[0]["F_bgrq"].ToString().Trim();

                string shrq = dt_brxx.Rows[0]["F_spare5"].ToString().Trim();
                string rysj = dt_brxx.Rows[0]["F_rysj"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_jxsj"].ToString().Trim();
                string blzd = dt_brxx.Rows[0]["F_blzd"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_tsjc"].ToString().Trim();
                string bgys = dt_brxx.Rows[0]["F_BGYS"].ToString().Trim();
                string shys = dt_brxx.Rows[0]["F_shys"].ToString().Trim();
               

                if (bglx == "bc")
                {
                    bglxmc="����";
                    if (dt_bcbg.Rows.Count > 0)
                    {
                        bgrq = dt_bcbg.Rows[0]["F_bc_bgrq"].ToString().Trim();
                        shrq = dt_bcbg.Rows[0]["F_bc_spare5"].ToString().Trim();
                        bgys = dt_bcbg.Rows[0]["F_bc_bgys"].ToString().Trim();
                        shys = dt_bcbg.Rows[0]["F_bc_shys"].ToString().Trim();
                        blzd = dt_bcbg.Rows[0]["F_bczd"].ToString().Trim();
                        bgzt = dt_bcbg.Rows[0]["F_bc_bgzt"].ToString().Trim();
               
                       // bglxmc = "���䱨��";
                    }
                    else
                    {
                        return "";
                    }
                }
                if (bglx == "bd")
                {
                     bglxmc="����";
                    if (dt_bdbg.Rows.Count > 0)
                    {
                        bgrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        shrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        bgys = dt_bdbg.Rows[0]["F_bd_bgys"].ToString().Trim();
                        shys = dt_bdbg.Rows[0]["F_bd_shys"].ToString().Trim();
                        blzd = dt_bdbg.Rows[0]["F_bdzd"].ToString().Trim();
                        bgzt = dt_bdbg.Rows[0]["F_bd_bgzt"].ToString().Trim();
                        rysj = "";
                       // bglxmc = "��������";
                    }
                    else
                    {
                        return "";
                    }
                }

                try
                {
                    sdrq = DateTime.Parse(sdrq).ToString("yyyy-MM-dd HH:mm:ss");
                    qcrq = DateTime.Parse(qcrq).ToString("yyyy-MM-dd HH:mm:ss");
                    if (bgrq != "")
                        bgrq = DateTime.Parse(bgrq).ToString("yyyy-MM-dd HH:mm:ss");
                    if (shrq != "")
                        shrq = DateTime.Parse(shrq).ToString("yyyy-MM-dd HH:mm:ss");
                }
                catch
                {
                }
                if (bgzt != "�����" && bgzt != "�ѷ���")
                {
                    blzd = "";
                    rysj = "";
                    shrq = "";
                    bgrq = "";
                    bgys = "";
                    shys = "";
                    pdfpath = "";
                }

                string blbh = "BL" + blh + bglx + bgxh;

                string xml = "";
                //xml = xml + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../../Schemas/POOR_IN200901UV20.xsd\">";
                xml = xml + "<id extension=\"JHIPBS304\" />";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\" />";
                xml = xml + "<interactionId root=\"\" extension=\"POOR_IN200901UV20\" />";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<processingModeCode code=\"T\" />";
                xml = xml + "<acceptAckCode code=\"NE\" />";

                //  xml = xml + "<!-- ������ -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                // xml = xml + "<!-- ������ ID root ΪҽԺ�ڲ��������ϵͳ ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                //  xml = xml + "<!-- ������ -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                // xml = xml + "<!-- ������ ID root ΪҽԺ�ڲ��������ϵͳ ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension='����ϵͳ'/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                //  xml = xml + "<!-- ��װ����Ϣ���� -->";
                xml = xml + "<realmCode code=\"CN\"/>";
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_MT000040\"/>";
                xml = xml + "<templateId root=\"2.16.156.10011.2.1.1.26\"/>";
                // xml = xml + "<!-- �ĵ�Ψһ��ʶ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.2.3\" extension=\"" + blbh + "\"/>";
                xml = xml + "<title>�����鱨��</title>";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"/>";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"1.2.156.112636.1.1.2.1.4.31\" codeSystemName=\"�������ʱ��ܼ���\" displayName=\"����\"/>";
                xml = xml + "<languageCode code=\"zh-CN\"/>";
                xml = xml + "<setId/>";
                xml = xml + "<versionNumber/>";
                ////xml=xml+"<!--�ĵ���¼���󣨻��ߣ� -->";
                xml = xml + "<recordTarget typeCode=\"RCT\" contextControlCode=\"OP\">";
                xml = xml + "<patientRole classCode=\"PAT\">";
                ////xml=xml+"<!--���� ID(patient_id)��ʶ,ȫԺͳһ���߱�ʶ -->";
                if (dt_brxx.Rows[0]["F_BRLB"].ToString().Trim()=="סԺ")
                xml = xml + "<id root=\"1.2.156.112636.1.2.1.4.1\" extension=\"" + dt_brxx.Rows[0]["F_ZYH"].ToString().Trim() + "\"/>";
                else
                xml = xml + "<id root=\"1.2.156.112636.1.2.1.4.1\" extension=\"" + dt_brxx.Rows[0]["F_MZH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--�ţ�������ű�ʶ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.3\" extension=\"" + dt_brxx.Rows[0]["F_MZH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--סԺ�ű�ʶ-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.2\" extension=\"" + dt_brxx.Rows[0]["F_ZY"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--���浥�ű�ʶ�� ����� EXAM_NO�� -->";
               
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh+bglxmc +bgxh+ "\"/>";
              
                ////xml=xml+"<!--�������뵥���,����ж�����뵥�ţ����Ϊ������ ����� EXAM_APPLY_NO�� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.20\" extension=\"" + dt_brxx.Rows[0]["F_SQXH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--�걾��ű�ʶ���������У�������鲻��Ҫ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"\"/>";
                ////xml=xml+"<!--Ӱ���/����� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.2.2.1.1\" extension=\"" + dt_brxx.Rows[0]["F_BLH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--����ţ�ֻ�в������� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.26\" extension=\"" + dt_brxx.Rows[0]["F_BLH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- ���߾���������(displayName Ϊ FILE_VISIT_TYPE) -->";
                xml = xml + "<patientType>";

                string brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "����")
                    brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "סԺ")
                    brlbbm = "2";

                xml = xml + "<patienttypeCode code=\"" + brlbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.4.10\" codeSystemName=\"���߾������ʹ����\" displayName=\"" + dt_brxx.Rows[0]["F_BRLB"].ToString().Trim() + "\"/>";
                xml = xml + "</patientType>";
                ////xml=xml+"<!-- ��ϵ�绰 -->";
                xml = xml + "<telecom value=\"" + "" + "\"/>";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                ////xml=xml+"<!--�������֤�ű�ʶ-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.8\" extension=\"" + dt_brxx.Rows[0]["F_SFZH"].ToString().Trim() + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_XM"].ToString().Trim() + "</name>";

                string xbbm = "0";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "��")
                    xbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "Ů")
                    xbbm = "2";
                xml = xml + "<administrativeGenderCode code=\"" + xbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.2.1\" codeSystemName=\"�Ա�����\" displayName=\"" + dt_brxx.Rows[0]["F_XB"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- ���� -->";
                xml = xml + "<age unit=\"��\" value=\"" + dt_brxx.Rows[0]["F_age"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--��������-->";
                xml = xml + "<birthTime value=\"" + "" + "\"/>";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                // xml = xml + "<!-- ��鱨��ҽʦ���ĵ������ߣ� -->";
                xml = xml + "<author typeCode=\"AUT\" contextControlCode=\"OP\">";
                // xml = xml + "<!-- ��鱨������ -->";
                xml = xml + "<time value=\"" + bgrq + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                // xml = xml + "<!-- ҽʦ���� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(bgys.Trim()) + "\"/>";
                // xml = xml + "<!-- ҽʦ����-->";
                xml = xml + "<assignedPerson>";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";


                //  xml = xml + "<!-- ���ܻ��� -->";
                xml = xml + "<custodian typeCode=\"CST\">";
                xml = xml + "<assignedCustodian classCode=\"ASSIGNED\">";
                xml = xml + "<representedCustodianOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>�㶫ʡ�ڶ�����ҽԺ</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";


                //  xml = xml + "<!-- ���ҽʦǩ�� -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time value=\"" + shrq + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!-- ҽʦ����  -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(shys.Trim()) + "\"/>";
                xml = xml + "<code displayName=\"���ҽʦ\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";


                //  xml = xml + "<!-- ��鼼ʦǩ�� -->";
                xml = xml + "<authenticator>";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!-- ҽʦ���� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"\"/>";
                xml = xml + "<code displayName=\"��鼼ʦ\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_JSY"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                //   xml = xml + "<!-- ���������������� -->";
                xml = xml + "<participant typeCode=\"PRF\">";
                //  xml = xml + "<!-- ����ʱ�� -->";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<scopingOrganization>";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"" + "" + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                xml = xml + "<asOrganizationPartOf>";
                xml = xml + "<wholeOrganization>";
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>�㶫ʡ�ڶ�����ҽԺ</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";




                xml = xml + "<inFulfillmentOf>";
                //    xml = xml + "<!--ҽ����Ϣ��һ�ݱ���������ҽ�����ظ�����-->";
                xml = xml + "<order>";
                //    xml = xml + "<!--ҽ����,ҽ��Ψһ��ʶ-->";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.19\" extension=\"" + "" + "\"/>";
                //   xml = xml + "<!--ҽ����Ŀ-->";
                xml = xml + "<code code=\"\" displayName=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";


                xml = xml + "<documentationOf>";
                xml = xml + "<serviceEvent>";
                //   xml = xml + "<!--���ִ��ʱ��-->";


                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.3.1.7\"/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--���ҽ������-->";
                xml = xml + "<id extension=\"" + getyhgh(dt_brxx.Rows[0]["F_qcys"].ToString().Trim()) + "\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //   xml = xml + "<!--���ҽ������ -->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_qcys"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "</serviceEvent>";
                xml = xml + "</documentationOf>";


                //   xml = xml + "<!-- �����š����������������Һ�ҽԺ�Ĺ��� -->";
                xml = xml + "<componentOf>";
                xml = xml + "<encompassingEncounter>";
                xml = xml + "<effectiveTime/>";
                xml = xml + "<location>";
                xml = xml + "<healthCareFacility>";
                xml = xml + "<serviceProviderOrganization>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization classCode=\"" + "1"+ "\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.23\" extension=\"" + dt_brxx.Rows[0]["F_CH"].ToString().Trim() + "\"/>";
                //    xml = xml + "<!-- DE01.00.019.00 ������ -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
              
                    xml = xml + "<wholeOrganization classCode=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString().Trim() + "(" + bglxmc + ")" + "\" determinerCode=\"INSTANCE\">";

                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
              
                xml = xml + "<wholeOrganization classCode=\"NULL\" determinerCode=\"INSTANCE\">";
                ///////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                ///////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_BQ"].ToString().Trim() + "</name>";
                //    xml = xml + "<!-- DE08.10.026.00 �������� -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //    xml = xml + "<!-- classCode Ϊ���� COSTS -->";
                xml = xml + "<wholeOrganization classCode=\"0\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"\"/>";
                ////////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                //    xml = xml + "<!--XXX ҽԺ -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636\" />";
                xml = xml + "<name>�㶫ʡ�ڶ�����ҽԺ</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</location>";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";


                //  xml = xml + "<!--�ĵ��� Body-->";
                xml = xml + "<component>";
                xml = xml + "<structuredBody classCode=\"DOCBODY\" moodCode=\"EVN\">";
                //   xml = xml + "<!--���ԤԼ�½�-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"ԤԼ��Ϣ\" />";
                //  xml = xml + "<!--ԤԼʱ��-->";
                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                //  xml = xml + "<!--ԤԼ��-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                //   xml = xml + "<!--���� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJYS"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //    xml = xml + "<!--������Ϣ-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"ԤԼ��Ϣ\" />";
                //    xml = xml + "<!--����ʱ��-->";
                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                //    xml = xml + "<!--����ҽʦ-->";
                xml = xml + "<participant typeCode=\"PRF\">";
                xml = xml + "<participantRole>";
                //    xml = xml + "<!--���ҽʦ����-->";
                xml = xml + "<id extension=\"\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //    xml = xml + "<!--���ҽʦ����-->";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //    xml = xml + "<!--�����豸-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //    xml = xml + "<!--�豸��-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"27433\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName>��΢��</manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //    xml = xml + "<!--����ص�-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                //      xml = xml + "<!--���������-->";
                xml = xml + "<location>";
                xml = xml + "<name>�����</name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                xml = xml + "</act>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";



                //   xml = xml + "<!--���רҵ�½�-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                //////////////////////////
                xml = xml + "<code code=\"29545-1\" displayName=\"PHYSICAL EXAMINATION\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" />";
                xml = xml + "<title>������</title>";
                //   xml = xml + "<!--�˿ɶ����ݣ���Ϊ�� -->";
                xml = xml + "<text/>";
                //   xml = xml + "<!--������ݴ�����Ŀ-->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                //   xml = xml + "<!--������-->";
                //////////////////////////////////////
                xml = xml + "<code code=\"29576-6\" codeSystem=\"1.2.156.112636.1.1.2.1.4.40\" codeSystemName=\"����������\" displayName=\"�²���\"/>";
                //  xml = xml + "<!--���״̬��ȡֵ��Χ�� completed|abort|active����Ӧ�����|����|����ִ��-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ///////////////////////////////////////
                //  xml = xml + "<!--���ʱ�䣬������״̬�� completed��������ʱ�䣬 abort ��������ʱ�䣬 active ������ʱ�䣨 value ����״̬��Ϊ���ʱ�� EXAM_DATE_TIME�� ����ʱ�� REPORT_DATE_TIME�� -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"/>";
                //  xml = xml + "<!--���ִ���ߣ�������״̬�� completed��������ҽʦ�� abort ��������ҽʦ�� active ������ҽʦ-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--����-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(dt_brxx.Rows[0]["F_SHYS"].ToString().Trim()) + "\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                //  xml = xml + "<!--name Ϊִ��ҽ�� 1 PERFORMED_BY1-->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SHYS"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //   xml = xml + "<!--���ʱ��-->";
                xml = xml + "<effectiveTime>";
                //   xml = xml + "<!--RIS �Ǽ�ʱ��-->";
                xml = xml + "<low value=\"" + sdrq + "\"/>";
                //   xml = xml + "<!--���ʱ��-->";
                xml = xml + "<high value=\"" + qcrq + "\"/>";
                xml = xml + "</effectiveTime>";
                //  xml = xml + "<!--������Ϣ����ѡ����������漰����������д-->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                xml = xml + "<specimenRole classCode=\"SPEC\">";
                //  xml = xml + "<!--�������/����-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"" + blh + "\"/>";
                xml = xml + "<specimenPlayingEntity>";
                // xml = xml + "<!--��������-->";
                xml = xml + "<code code=\"DD\" codeSystem=\"1.2.156.112636.1.1.2.1.3.5\" codeSystemName=\"�������ʹ���\" displayName=\"" + dt_brxx.Rows[0]["F_BBLX"].ToString().Trim() + "\"/>";
                xml = xml + "</specimenPlayingEntity>";
                xml = xml + "</specimenRole>";
                xml = xml + "</specimen>";
                //  xml = xml + "<!--����豸-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //   xml = xml + "<!--�豸��-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName>��΢��</manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                ///  xml = xml + "<!--���ص�-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                //   xml = xml + "<!--���������-->";
                xml = xml + "<location>";
                xml = xml + "<name>�����</name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--���������-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.1.8\"/>";
                xml = xml + "<methodCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.41\" codeSystemName=\" �� �� �� �� �� �� �� \" displayName=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString() + "\"/>";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.8\" codeSystemName=\" �� �� �� λ �� �� �� \" displayName=\"" + dt_brxx.Rows[0]["f_bbmc"].ToString() + "\"/>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                //   xml = xml + "<!-- ��鱨���½� -->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code displayName=\"��鱨��\"/>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                //   xml = xml + "<!--��鱨����֯-->";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                //    xml = xml + "<!--�����-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blbh + "\"/>";

                //////
                xml = xml + "<statusCode code=\"completed\"/>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='404684003' displayName='Finding' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(rysj) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='282291009' displayName='Diagnosis' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(blzd) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";



                //  xml = xml + "<!-- Ӱ������ -->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code  displayName='Ӱ����������' />";
                xml = xml + "<reference typeCode='REFR|SPRT'>";
                xml = xml + "<externalDocument classCode='DOC' moodCode='EVN'>";
                xml = xml + "<id extension='' root=''/>";
                xml = xml + "<text><reference value='" + pdfpath + "'/></text>";
                xml = xml + "</externalDocument>";
                xml = xml + "</reference>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                // xml = xml + "<!--���汸ע-->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<templateId extension=\"1.3.6.1.4.1.19376.1.5.3.1.4.2\"/>";
                xml = xml + "<code code=\"48767-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"AnnotationComment\"/>";
                xml = xml + "<text><reference value=\"" + "���汸ע" + "\"/></text>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";
                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</POOR_IN200901UV>";



                return FormatXml(xml);
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
               log.WriteMyLog(ee.Message);
                return "";
            }
        }

        public string BgMsg(DataTable dt_brxx, DataTable dt_bcbg, DataTable dt_bdbg, DataTable dt_sqd, string blh, string bglx, string bgxh, string pdfpath, ref string errmsg)
        {
            try
            {

                string bgzt = dt_brxx.Rows[0]["F_BGZT"].ToString().Trim();
                string qcrq = dt_brxx.Rows[0]["F_qcrq"].ToString().Trim();
                if (qcrq == "")
                    qcrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();

                string sdrq = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim();
                string bgrq = dt_brxx.Rows[0]["F_bgrq"].ToString().Trim();

                string shrq = dt_brxx.Rows[0]["F_spare5"].ToString().Trim();
                string rysj = dt_brxx.Rows[0]["F_rysj"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_jxsj"].ToString().Trim();
                string blzd = dt_brxx.Rows[0]["F_sdrq"].ToString().Trim() + "\r\n" + dt_brxx.Rows[0]["F_tsjc"].ToString().Trim();
                string bgys = dt_brxx.Rows[0]["F_BGYS"].ToString().Trim();
                string shys = dt_brxx.Rows[0]["F_shys"].ToString().Trim();


                if (bglx == "bc")
                {
                    if (dt_bcbg.Rows.Count > 0)
                    {
                        bgrq = dt_bcbg.Rows[0]["F_bc_bgrq"].ToString().Trim();
                        shrq = dt_bcbg.Rows[0]["F_bc_spare5"].ToString().Trim();
                        bgys = dt_bcbg.Rows[0]["F_bc_bgys"].ToString().Trim();
                        shys = dt_bcbg.Rows[0]["F_bc_shys"].ToString().Trim();
                        blzd = dt_bcbg.Rows[0]["F_bczd"].ToString().Trim();
                        bgzt = dt_bcbg.Rows[0]["F_bc_bgzt"].ToString().Trim();
                    }
                    else
                    {
                        return "";
                    }
                }
                if (bglx == "bd")
                {
                    if (dt_bdbg.Rows.Count > 0)
                    {
                        bgrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        shrq = dt_bdbg.Rows[0]["F_bd_bgrq"].ToString().Trim();
                        bgys = dt_bdbg.Rows[0]["F_bd_bgys"].ToString().Trim();
                        shys = dt_bdbg.Rows[0]["F_bd_shys"].ToString().Trim();
                        blzd = dt_bcbg.Rows[0]["F_bdzd"].ToString().Trim();
                        bgzt = dt_bcbg.Rows[0]["F_bd_bgzt"].ToString().Trim();
                    }
                    else
                    {
                        return "";
                    }
                }

                try
                {
                    sdrq = DateTime.Parse(sdrq).ToString("yyyyMMdd");
                    qcrq = DateTime.Parse(qcrq).ToString("yyyyMMdd");
                    if (bgrq != "")
                        bgrq = DateTime.Parse(bgrq).ToString("yyyyMMdd");
                    if (shrq != "")
                        shrq = DateTime.Parse(shrq).ToString("yyyyMMdd");
                }
                catch
                {
                }
                if (bgzt != "�����" && bgzt != "�ѷ���")
                {
                    blzd = "";
                    rysj = "";
                    shrq = "";
                    bgrq = "";
                    bgys = "";
                    shys = "";
                }


                string xml = "";
                xml = xml + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../../Schemas/POOR_IN200901UV20.xsd\">";
                xml = xml + "<id extension=\"JHIPBS304\" />";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<interactionId root=\"\" extension=\"POOR_IN200901UV20\" />";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<processingModeCode code=\"T\" />";
                xml = xml + "<acceptAckCode code=\"NE\" />";

                //  xml = xml + "<!-- ������ -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                //   xml = xml + "<!-- ������ ID root ΪҽԺ�ڲ��������ϵͳ ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                //   xml = xml + "<!-- ������ -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                //   xml = xml + "<!-- ������ ID root ΪҽԺ�ڲ��������ϵͳ ID-->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension='����ϵͳ'/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                //  xml = xml + "<!-- ��װ����Ϣ���� -->";
                xml = xml + "<realmCode code=\"CN\"/>";
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_MT000040\"/>";
                xml = xml + "<templateId root=\"2.16.156.10011.2.1.1.26\"/>";
                //  xml = xml + "<!-- �ĵ�Ψһ��ʶ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.2.3\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
                xml = xml + "<title>�����鱨��</title>";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"1.2.156.112636.1.1.2.1.4.31\" codeSystemName=\"�������ʱ��ܼ���\" displayName=\"����\"/>";
                xml = xml + "<languageCode code=\"zh-CN\"/>";
                xml = xml + "<setId/>";
                xml = xml + "<versionNumber/>";
                ////xml=xml+"<!--�ĵ���¼���󣨻��ߣ� -->";
                xml = xml + "<recordTarget typeCode=\"RCT\" contextControlCode=\"OP\">";
                xml = xml + "<patientRole classCode=\"PAT\">";
                ////xml=xml+"<!--���� ID(patient_id)��ʶ,ȫԺͳһ���߱�ʶ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.2.1.4.1\" extension=\"" + dt_brxx.Rows[0]["F_ZYH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--�ţ�������ű�ʶ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.3\" extension=\"" + dt_brxx.Rows[0]["F_MZH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--סԺ�ű�ʶ-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.2\" extension=\"" + dt_brxx.Rows[0]["F_ZY"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--���浥�ű�ʶ�� ����� EXAM_NO�� -->";
                if (bglx == "bc")
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh +"����"+bgxh+ "\"/>";
                else if (bglx == "bc")
                    xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh + "����" + bgxh + "\"/>";
                else xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh + "����"  + "\"/>";
                ////xml=xml+"<!--�������뵥���,����ж�����뵥�ţ����Ϊ������ ����� EXAM_APPLY_NO�� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.20\" extension=\"" + dt_brxx.Rows[0]["F_SQXH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--�걾��ű�ʶ���������У�������鲻��Ҫ -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"\"/>";
                ////xml=xml+"<!--Ӱ���/����� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.2.2.1.1\" extension=\"\"/>";
                ////xml=xml+"<!--����ţ�ֻ�в������� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.26\" extension=\"" + dt_brxx.Rows[0]["F_BLH"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- ���߾���������(displayName Ϊ FILE_VISIT_TYPE) -->";
                xml = xml + "<patientType>";

                string brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "����")
                    brlbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "סԺ")
                    brlbbm = "2";

                xml = xml + "<patienttypeCode code=\"" + brlbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.4.10\" codeSystemName=\"���߾������ʹ����\" displayName=\"" + dt_brxx.Rows[0]["F_BRLB"].ToString().Trim() + "\"/>";
                xml = xml + "</patientType>";
                ////xml=xml+"<!-- ��ϵ�绰 -->";
                xml = xml + "<telecom value=\"" + dt_brxx.Rows[0]["F_LXXX"].ToString().Trim() + "\"/>";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                ////xml=xml+"<!--�������֤�ű�ʶ-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.8\" extension=\"" + dt_brxx.Rows[0]["F_SFZH"].ToString().Trim() + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_XM"].ToString().Trim() + "</name>";

                string xbbm = "0";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "��")
                    xbbm = "1";
                if (dt_brxx.Rows[0]["F_BRBH"].ToString().Trim() == "Ů")
                    xbbm = "2";
                xml = xml + "<administrativeGenderCode code=\"" + xbbm + "\" codeSystem=\"1.2.156.112636.1.1.2.1.2.1\" codeSystemName=\"�Ա�����\" displayName=\"" + dt_brxx.Rows[0]["F_XB"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!-- ���� -->";
                xml = xml + "<age unit=\"��\" value=\"" + dt_brxx.Rows[0]["F_age"].ToString().Trim() + "\"/>";
                ////xml=xml+"<!--��������-->";
                xml = xml + "<birthTime value=\"" + "" + "\"/>";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                // xml = xml + "<!-- ��鱨��ҽʦ���ĵ������ߣ� -->";
                xml = xml + "<author typeCode=\"AUT\" contextControlCode=\"OP\">";
                //  xml = xml + "<!-- ��鱨������ -->";
                xml = xml + "<time value=\"" + bgrq + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                //  xml = xml + "<!-- ҽʦ���� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(bgys.Trim()) + "\"/>";
                //  xml = xml + "<!-- ҽʦ����-->";
                xml = xml + "<assignedPerson>";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";


                // xml = xml + "<!-- ���ܻ��� -->";
                xml = xml + "<custodian typeCode=\"CST\">";
                xml = xml + "<assignedCustodian classCode=\"ASSIGNED\">";
                xml = xml + "<representedCustodianOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>�㶫ʡ�ڶ�����ҽԺ</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";


                // xml = xml + "<!-- ���ҽʦǩ�� -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time value=\"" + DateTime.Parse(shrq.Trim()).ToString("yyyyMMdd") + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!-- ҽʦ����  -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(shys.Trim()) + "\"/>";
                xml = xml + "<code displayName=\"���ҽʦ\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";


                // xml = xml + "<!-- ��鼼ʦǩ�� -->";
                xml = xml + "<authenticator>";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<signatureCode/>";
                xml = xml + "<assignedEntity>";
                // xml = xml + "<!-- ҽʦ���� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"\"/>";
                xml = xml + "<code displayName=\"��鼼ʦ\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_JSY"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                //  xml = xml + "<!-- ���������������� -->";
                xml = xml + "<participant typeCode=\"PRF\">";
                //  xml = xml + "<!-- ����ʱ�� -->";
                xml = xml + "<time value=\"" + sdrq + "\"/>";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<scopingOrganization>";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"" + "" + "\"/>";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                xml = xml + "<asOrganizationPartOf>";
                xml = xml + "<wholeOrganization>";
                xml = xml + "<id root=\"1.2.156.112636\" extension=\"\"/>";
                xml = xml + "<name>�㶫ʡ�ڶ�����ҽԺ</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";




                xml = xml + "<inFulfillmentOf>";
                //  xml = xml + "<!--ҽ����Ϣ��һ�ݱ���������ҽ�����ظ�����-->";
                xml = xml + "<order>";
                //   xml = xml + "<!--ҽ����,ҽ��Ψһ��ʶ-->";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.19\" extension=\"" + "" + "\"/>";
                //  xml = xml + "<!--ҽ����Ŀ-->";
                xml = xml + "<code code=\"ORG\" displayName=\"" + dt_brxx.Rows[0]["F_YZXM"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";


                xml = xml + "<documentationOf>";
                xml = xml + "<serviceEvent>";
                //  xml = xml + "<!--���ִ��ʱ��-->";


                xml = xml + "<effectiveTime value=\"" + sdrq + "\"/>";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.3.1.7\"/>";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--���ҽ������-->";
                xml = xml + "<id extension=\"" + getyhgh(dt_brxx.Rows[0]["F_qcys"].ToString().Trim()) + "\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //  xml = xml + "<!--���ҽ������ -->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_qcys"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "</serviceEvent>";
                xml = xml + "</documentationOf>";


                //   xml = xml + "<!-- �����š����������������Һ�ҽԺ�Ĺ��� -->";
                xml = xml + "<componentOf>";
                xml = xml + "<encompassingEncounter>";
                xml = xml + "<effectiveTime/>";
                xml = xml + "<location>";
                xml = xml + "<healthCareFacility>";
                xml = xml + "<serviceProviderOrganization>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //   xml = xml + "<!-- DE01.00.026.00 ������ -->";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.23\" extension=\"" + dt_brxx.Rows[0]["F_CH"].ToString().Trim() + "\"/>";
                //    xml = xml + "<!-- DE01.00.019.00 ������ -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //   xml = xml + "<!-- classCode Ϊ�����Ŀ EXAM_ITEM �ͼ���������� EXAM_SUB_CLASS -->";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                // xml = xml + "<!-- DE08.10.054.00 �������� -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                //xml = xml + "<!-- classCode Ϊ��Ŀ���� EXAM_ITEM_CODE -->";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                ///////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.7\" extension=\"001\"/>";
                ///////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_BQ"].ToString().Trim() + "</name>";
                // xml = xml + "<!-- DE08.10.026.00 �������� -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                // xml = xml + "<!-- classCode Ϊ���� COSTS -->";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //////
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.1\" extension=\"001\"/>";
                ////////
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SJKS"].ToString().Trim() + "</name>";
                // xml = xml + "<!--XXX ҽԺ -->";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"1.2.156.112636\" />";
                xml = xml + "<name>�㶫ʡ�ڶ�����ҽԺ</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</location>";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";


                //  xml = xml + "<!--�ĵ��� Body-->";
                xml = xml + "<component>";
                xml = xml + "<structuredBody classCode=\"DOCBODY\" moodCode=\"EVN\">";
                //  xml = xml + "<!--���ԤԼ�½�-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"ԤԼ��Ϣ\" />";
                //  xml = xml + "<!--ԤԼʱ��-->";
                xml = xml + "<effectiveTime value=\"20090415144550.0000-0500\"/>";
                //  xml = xml + "<!--ԤԼ��-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                //  xml = xml + "<!--���� -->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"83736\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //  xml = xml + "<!--������Ϣ-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code displayName=\"ԤԼ��Ϣ\" />";
                //  xml = xml + "<!--����ʱ��-->";
                xml = xml + "<effectiveTime value=\"20090415144550.0000-0500\"/>";
                //  xml = xml + "<!--����ҽʦ-->";
                xml = xml + "<participant typeCode=\"PRF\">";
                xml = xml + "<participantRole>";
                //  xml = xml + "<!--���ҽʦ����-->";
                xml = xml + "<id extension=\"54689A\" root=\"1.2.156.112636.1.1.2.1.4.2\"/>";
                xml = xml + "<assignedPerson>";
                //   xml = xml + "<!--���ҽʦ����-->";
                xml = xml + "<name></name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--�����豸-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //   xml = xml + "<!--�豸��-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"27433\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName>�豸����</manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--����ص�-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                ///  xml = xml + "<!--���������-->";
                xml = xml + "<location>";
                xml = xml + "<name></name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                xml = xml + "</act>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";



                //  xml = xml + "<!--���רҵ�½�-->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                //////////////////////////
                xml = xml + "<code code=\"29545-1\" displayName=\"PHYSICAL EXAMINATION\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" />";
                xml = xml + "<title>������</title>";
                //    xml = xml + "<!--�˿ɶ����ݣ���Ϊ�� -->";
                xml = xml + "<text/>";
                //   xml = xml + "<!--������ݴ�����Ŀ-->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                //   xml = xml + "<!--������-->";
                //////////////////////////////////////
                xml = xml + "<code code=\"29576-6\" codeSystem=\"1.2.156.112636.1.1.2.1.4.40\" codeSystemName=\"����������\" displayName=\"\"/>";
                ///  xml = xml + "<!--���״̬��ȡֵ��Χ�� completed|abort|active����Ӧ�����|����|����ִ��-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ///////////////////////////////////////
                //    xml = xml + "<!--���ʱ�䣬������״̬�� completed��������ʱ�䣬 abort ��������ʱ�䣬 active ������ʱ�䣨 value ����״̬��Ϊ���ʱ�� EXAM_DATE_TIME�� ����ʱ�� REPORT_DATE_TIME�� -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\"/>";
                //   xml = xml + "<!--���ִ���ߣ�������״̬�� completed��������ҽʦ�� abort ��������ҽʦ�� active ������ҽʦ-->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<assignedEntity>";
                // xml = xml + "<!--����-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.4.2\" extension=\"" + getyhgh(dt_brxx.Rows[0]["F_SHYS"].ToString().Trim()) + "\" />";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                //   xml = xml + "<!--name Ϊִ��ҽ�� 1 PERFORMED_BY1-->";
                xml = xml + "<name>" + dt_brxx.Rows[0]["F_SHYS"].ToString().Trim() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";


                //   xml = xml + "<!--���ʱ��-->";
                xml = xml + "<effectiveTime>";
                //   xml = xml + "<!--RIS �Ǽ�ʱ��-->";
                xml = xml + "<low value=\"" + sdrq + "\"/>";
                //  xml = xml + "<!--���ʱ��-->";
                xml = xml + "<high value=\"" + qcrq + "\"/>";
                xml = xml + "</effectiveTime>";
                //  xml = xml + "<!--������Ϣ����ѡ����������漰����������д-->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                xml = xml + "<specimenRole classCode=\"SPEC\">";
                //   xml = xml + "<!--�������/����-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.25\" extension=\"\"/>";
                xml = xml + "<specimenPlayingEntity>";
                //   xml = xml + "<!--��������-->";
                xml = xml + "<code code=\"DD\" codeSystem=\"1.2.156.112636.1.1.2.1.3.5\" codeSystemName=\"�������ʹ���\" displayName=\"" + dt_brxx.Rows[0]["F_BBLX"].ToString().Trim() + "\"/>";
                xml = xml + "</specimenPlayingEntity>";
                xml = xml + "</specimenRole>";
                xml = xml + "</specimen>";
                //   xml = xml + "<!--����豸-->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                //    xml = xml + "<!--�豸��-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.1.5.2\" extension=\"\"/>";
                xml = xml + "<playingDevice>";
                xml = xml + "<manufacturerModelName></manufacturerModelName>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //   xml = xml + "<!--���ص�-->";
                xml = xml + "<participant typeCode=\"LOC\">";
                xml = xml + "<healthCareFacility>";
                //    xml = xml + "<!--���������-->";
                xml = xml + "<location>";
                xml = xml + "<name>�����</name>";
                xml = xml + "</location>";
                xml = xml + "</healthCareFacility>";
                xml = xml + "</participant>";

                //    xml = xml + "<!--���������-->";
                xml = xml + "<entryRelationship typeCode=\"COMP\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<templateId root=\"1.3.6.1.4.1.19376.1.3.1.8\"/>";
                xml = xml + "<methodCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.41\" codeSystemName=\" �� �� �� �� �� �� �� \" displayName=\"" + dt_brxx.Rows[0]["f_blk"].ToString() + "\"/>";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156.112636.1.1.2.1.4.8\" codeSystemName=\" �� �� �� λ �� �� �� \" displayName=\"" + dt_brxx.Rows[0]["f_bbmc"].ToString() + "\"/>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</act>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                //    xml = xml + "<!-- ��鱨���½� -->";
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code displayName=\"��鱨��\"/>";
                xml = xml + "<text/>";
                xml = xml + "<entry>";
                //     xml = xml + "<!--��鱨����֯-->";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                //     xml = xml + "<!--�����-->";
                xml = xml + "<id root=\"1.2.156.112636.1.1.2.2.1.24\" extension=\"" + blh + "\"/>";

                //////
                xml = xml + "<statusCode code=\"completed\"/>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='404684003' displayName='Finding' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(rysj) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code='282291009' displayName='Diagnosis' codeSystem='2.16.840.1.113883.6.96' codeSystemName='SNOMEDCT'/>";
                xml = xml + "<text><reference value=\"" + System.Security.SecurityElement.Escape(blzd) + "\"/></text>";
                xml = xml + "</observation>";
                xml = xml + "</component>";



                //    xml = xml + "<!-- Ӱ������ -->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<code  displayName='Ӱ����������' />";
                xml = xml + "<reference typeCode='REFR'>";
                xml = xml + "<externalDocument classCode='DOC' moodCode='EVN'>";
                xml = xml + "<id extension='' root=''/>";
                xml = xml + "<text><reference value='" + pdfpath + "'/></text>";
                xml = xml + "</externalDocument>";
                xml = xml + "</reference>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                //     xml = xml + "<!--���汸ע-->";
                xml = xml + "<component>";
                xml = xml + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                xml = xml + "<templateId extension=\"1.3.6.1.4.1.19376.1.5.3.1.4.2\"/>";
                xml = xml + "<code code=\"48767-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"AnnotationComment\"/>";
                xml = xml + "<text><reference value=\"" + "" + "\"/></text>";
                xml = xml + "</act>";
                xml = xml + "</component>";
                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";
                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</POOR_IN200901UV>";

                return xml;
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                return "";
            }
        }




   
        public void C_PDF(string blh, string bgxh, string bglx, string ml, ref string filename, ref string pdfpath, bool isToBase64String, ref string Base64String, string debug, ref  string errmsg)
        {
            string blbh = blh + bglx + bgxh;
            if (filename == "")
                filename = blh.Trim() + "_" + bglx.ToLower() + "_" + bgxh + "_" + DateTime.Parse(filename.Trim()).ToString("yyyyMMddHHmmss") + ".pdf";

            string rptpath = ZgqClass.GetSz("zgqjk", "rptpath", "rpt").Replace("\0", "").Trim();
            errmsg = "";
            ZgqPDFJPG pdf = new ZgqPDFJPG();
            bool pdf1 = pdf.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref filename, rptpath.Trim(), ref errmsg);
            if (!pdf1)
            {
                log.WriteMyLog("����PDF�ļ�ʧ��:" + errmsg);
                return ;
            }
            if (File.Exists(filename))
            {
                if (isToBase64String)
                {

                    try
                    {
                        FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
                        file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
                        file.Close();
                        Base64String = Convert.ToBase64String(imgByte);
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("PDFת�������ƴ�ʧ��");
                        errmsg = "PDFת�������ƴ�ʧ��";
                        Base64String = "";
                    }
                }

                //�ϴ�pdf

                if (pdf.UpPDF(blh, filename, ml, ref errmsg, 0,ref pdfpath))
                {
                    if (debug == "1")
                        log.WriteMyLog("�ϴ�PDF�ɹ�");
                    filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                    ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDF�ɹ�:" + pdfpath, "ZGQJK", "�ϴ�PDF");
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_FilePath,F_PDFLX) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ml + "\\" + blh + "','" + filename + "','" + pdfpath + "','')");
                }
                else
                {
                    log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + errmsg);
                    ZgqClass.BGHJ(blh, "�ϴ�PDF", "���", "�ϴ�PDFʧ�ܣ�" + errmsg, "ZGQJK", "�ϴ�PDF");
                }
            }
            else
            {
                log.WriteMyLog("����PDFʧ�ܣ�" + errmsg);
                ZgqClass.BGHJ(blh, "����PDF", "���", "����PDFʧ�ܣ�" + errmsg, "ZGQJK", "����PDF");
            }

            return;

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

        public string getyhgh(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        private string FormatXml(string sUnformattedXml)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(sUnformattedXml);
               
                StringWriter sw = new StringWriter(sb);
                XmlTextWriter xtw = null;
                try
                {
                    xtw = new XmlTextWriter(sw);
                    xtw.Formatting = Formatting.Indented;
                    xtw.Indentation = 1;
                    xtw.IndentChar = '\t';
                    xd.WriteTo(xtw);
                }
                catch(Exception  ee1)
                {
                    log.WriteMyLog(ee1.Message + "\r\n" + sUnformattedXml);
                }
                finally
                {
                    if (xtw != null)
                        xtw.Close();
                }
                return sb.ToString();
            }
            catch(Exception  ee2)
            {
                log.WriteMyLog(ee2.Message + "\r\n" + sUnformattedXml);
                return sb.ToString();
            }
        }

    }
}
