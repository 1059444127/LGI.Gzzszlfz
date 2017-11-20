using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;

namespace LGHISJKZGQ
{
    class NBYZ2Y
    {
        //����۴��2Ժ
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static IniFiles w = new IniFiles(Application.StartupPath + "\\pathgethis.ini");
        //private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
 
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string pathWEB = f.ReadString("������", "webservicesurl", ""); //��ȡsz.ini�����õ�webservicesurl
            string msg = w.ReadString("pathgethis", "msg", "");
            if (Sslbx != "")
            {

             if (Sslbx == "������")
                {
                    LGHISJKZGQ.nbyz2yWEB.PisServiceLJ yz2y = new LGHISJKZGQ.nbyz2yWEB.PisServiceLJ();
                    if (pathWEB != "") yz2y.Url = pathWEB;
                    string sBillInfo = "";
                    string sSampleInfo = "";
                    string Mes = "";
                    try
                    {
                        Mes = yz2y.PatBillWritePIS(Ssbz, ref sBillInfo, ref sSampleInfo, 1);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("�˱걾�����벻���ڣ���˶ԣ�");
                        if (Debug == "1")
                            MessageBox.Show("��ʾ����" + ee.ToString());
                        log.WriteMyLog("������" + Ssbz + "����ֵ��" + sBillInfo + "��ʾ��Ϣ��" + Mes);

                        return "0";
                    }

                    //MessageBox.Show(sBillInfo);
                    //MessageBox.Show(sSampleInfo);
                    if (Mes != "")
                    {
                        MessageBox.Show("HIS��" + Mes); return "0";
                    }

                    if (sBillInfo.Trim() == "" || sBillInfo == null)
                    {
                        MessageBox.Show("�����뵥������Ϊ�գ�");
                        return "0";
                    }
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(sBillInfo);
                        xmlok = xd.SelectSingleNode("/BILL");
                    }
                    catch
                    {
                        MessageBox.Show("��ȡ��Ϣ����");
                        log.WriteMyLog("XML�������󷵻�xml��" + sBillInfo + "--" + Mes);
                        return xmlstr();
                    }
                 //----------------------------------------------------------------
                //--�ж����ݿ����Ƿ����д�����Ų�����Ϣ-----------------------------------------------------

                DataTable brxx_1 = new DataTable();
                brxx_1 = aa.GetDataTable("select F_BLH,F_XM,F_SQXH from T_jcxx where F_SQXH='" + xmlok["HIS_KEYNO"].InnerText + "'", "blxx");
                aa.Close();
                if (brxx_1.Rows.Count >= 1)
                {
                    MessageBox.Show("�˲���:" + brxx_1.Rows[0]["F_xm"].ToString() + "������ţ�" + brxx_1.Rows[0]["F_sqxh"].ToString() + "������ţ�" + brxx_1.Rows[0]["F_blh"].ToString() + "���ѵǼǹ��������ظ��Ǽ�!!!");
                  DialogResult  dr= MessageBox.Show("�Ƿ��ظ��Ǽǣ�����","�Ƿ��ظ��Ǽ�",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

                  if (dr == DialogResult.No)
                    return xmlstr();
                }

                    //�걾����------------------------------------------------------------

                    XmlDocument xd1 = new XmlDocument();
                    string bbmc = "";
                    string bbbm = "";
                    int index = 0;
                    try
                    {
                        xd1.LoadXml(sSampleInfo);
                        XmlNodeList xnl = xd1.SelectNodes("/SAMPLES/SAMPLE");

                        index = xnl.Count;

                        for (int x = 0; x < xnl.Count; x++)
                        {
                            XmlNode xmlok1 = xnl[x];
                            bbmc = bbmc.Trim() + "  " + xmlok1["COLLECTBODY"].InnerText;
                            if (bbbm == "")
                                bbbm = xmlok1["SAMPLENO"].InnerText;
                            else
                                bbbm = bbbm + "," + xmlok1["SAMPLENO"].InnerText;
                        }
                    }
                    catch { if (Debug == "1") MessageBox.Show("�걾XML��������"); return "0"; }
                    if (Mes != "")
                    {
                        MessageBox.Show(Mes); return "0";
                    }

                    if (index > 1)
                    {

                        string bbbm_1 = w.ReadString("pathgethis", "bbmh", "");
                        string bbsl_1 = w.ReadString("pathgethis", "bbsl", "");
                        string sqxh_1 = w.ReadString("pathgethis", "sqxh", "");

                        string newsqxh = xmlok["HIS_KEYNO"].InnerText;
                        //string ysbbbm_1 = f.ReadString("pathgethis", "ysbbmh", "");
                        //string ysbbsl = f.ReadString("pathgethis", "ysbbsl", "");
                        if (sqxh_1 != newsqxh && sqxh_1 != bbsl_1)
                        {
                            if (MessageBox.Show("�˲��˹���" + index + "���걾\n�걾��δɨ�꣬�Ƿ�ȷ��ɨ���²���", "�Ƿ����", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {

                                w.WriteString("pathgethis", "bbsl", "");
                                w.WriteString("pathgethis", "bbmh", "");
                                w.WriteString("pathgethis", "ysbbmh", "");
                                w.WriteString("pathgethis", "ysbbsl", "");
                                w.WriteString("pathgethis", "sqxh", "");
                                bbbm_1 = w.ReadString("pathgethis", "bbmh", "");
                                bbsl_1 = w.ReadString("pathgethis", "bbsl", "");
                                sqxh_1 = w.ReadString("pathgethis", "sqxh", "");



                                w.WriteString("pathgethis", "bbsl", "");
                                w.WriteString("pathgethis", "bbmh", "");
                                w.WriteString("pathgethis", "ysbbmh", "");
                                w.WriteString("pathgethis", "ysbbsl", "");
                                w.WriteString("pathgethis", "sqxh", "");

                        }
                            else

                                return "0";
                        }

                        if (bbbm_1 == "" && bbsl_1 == "" && sqxh_1 == "")
                        {

                            //  string bbmhstr= w.ReadString("pathgethis", "bbmh", "");
                            w.WriteString("pathgethis", "bbsl", index.ToString());
                            w.WriteString("pathgethis", "bbmh", bbbm);
                            w.WriteString("pathgethis", "sqxh", newsqxh);
                            bbbm_1 = w.ReadString("pathgethis", "bbmh", "");
                            bbsl_1 = w.ReadString("pathgethis", "bbsl", "");
                            sqxh_1 = w.ReadString("pathgethis", "sqxh", "");
                        }

                        if (bbbm_1 != "" && bbsl_1 != "" && sqxh_1 != "")
                        { //д���ձ걾���������--------------------------------------------------------------
                            //string ysqxh = w.ReadString("pathgethis", "sqxh", "");

                            string ysbbmc = w.ReadString("pathgethis", "ysbbmh", "");
                            if (ysbbmc == "")
                                w.WriteString("pathgethis", "ysbbmh", Ssbz);
                            else
                            {
                                if (ysbbmc.Contains(Ssbz))
                                {
                                    if (msg == "1") MessageBox.Show("�ظ��������룬������ɨ�裬"); return "0";
                                }
                                w.WriteString("pathgethis", "ysbbmh", ysbbmc + "," + Ssbz);
                            }
                            //д���ձ걾����------------------------------------------------------------------------

                            string ysbbsl = w.ReadString("pathgethis", "ysbbsl", "");
                            if (ysbbsl == "")
                                w.WriteString("pathgethis", "ysbbsl", "1");
                            else
                            {
                                int y = int.Parse(ysbbsl) + 1;
                                w.WriteString("pathgethis", "ysbbsl", y.ToString());
                            }
                        }

                        //-----------���ñ걾�����ͱ���----------------------------------- 
                        //���������Ų���ͬ ���pathgethis.ini����д
                        //���pathgethis��ini������
                        //---�ж��Ƿ���ɽ��ܱ걾--------------------------------------------------
                        string bbbm_2 = w.ReadString("pathgethis", "bbmh", "");
                        string bbsl_2 = w.ReadString("pathgethis", "bbsl", "");
                        string ysbbbm_2 = w.ReadString("pathgethis", "ysbbmh", "");
                        string ysbbsl_2 = w.ReadString("pathgethis", "ysbbsl", "");


                        if (bbsl_2 != ysbbsl_2)
                        {
                            int y = int.Parse(bbsl_2) - int.Parse(ysbbsl_2);
                            if (msg == "1")
                                MessageBox.Show("�˲��˹���" + index + "���걾\n�걾��δɨ����ɣ������...\n����" + y.ToString() + "���걾δɨ��");

                            return "0";
                        }
                    }
                    
                   


                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + xmlok["MEDRECNO"].InnerText + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + xmlok["HIS_KEYNO"].InnerText + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + xmlok["OUTPATIENTNO"].InnerText + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + xmlok["INHOSPITALNO"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + xmlok["PNAME"].InnerText + (char)34 + " ";
                        string PSEX = xmlok["PSEX"].InnerText;
                        if (PSEX == "F") PSEX = "Ů";
                        else
                        {
                            if (PSEX == "M") PSEX = "��"; else PSEX = " ";
                        }
                        xml = xml + "�Ա�=" + (char)34 + PSEX + (char)34 + " ";
                        string nl1 = xmlok["PBIRTHDAY"].InnerText;
                        string nl = "0��";
                        try
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "��"; }
                        catch
                        { nl = "0��"; }
                        xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                        string MARRIED = xmlok["MARRIED"].InnerText;
                        switch (MARRIED)
                        {
                            case "Y": MARRIED = "�ѻ�"; break;
                            case "W": MARRIED = "δ��"; break;
                            case "L": MARRIED = "����"; break;
                            case "S": MARRIED = "ɥż"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "����=" + (char)34 + MARRIED + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + xmlok["ADDRESS"].InnerText + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + xmlok["PTELEPHONENO"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + xmlok["WARD"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + xmlok["BEDNO"].InnerText + (char)34 + " ";
                        //xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        // xml = xml + "����=" + (char)34 +""+ (char)34 + " ";

                        xml = xml + "���֤��=" + (char)34 + xmlok["IDCARD"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + xmlok["VOCATION"].InnerText + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + xmlok["SUBMITDEPT"].InnerText + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + xmlok["SUBMITDOC"].InnerText + (char)34 + " ";
              
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + xmlok["MEDICALTYPE"].InnerText + (char)34 + " ";
                        string PATIENTTYPE = xmlok["PATIENTTYPE"].InnerText;
                        if (PATIENTTYPE == "1000") PATIENTTYPE = "����";
                        if (PATIENTTYPE == "2000") PATIENTTYPE = "����";
                        if (PATIENTTYPE == "3000") PATIENTTYPE = "סԺ";
                        if (PATIENTTYPE == "4000") PATIENTTYPE = "���";
                        xml = xml + "�������=" + (char)34 + PATIENTTYPE + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "�ٴ���ʷ:" + xmlok["HISCONSULTATION"].InnerText + "          ����������" + xmlok["OPERATIONRESULT"].InnerText + "      Ӱ��ѧ������" + xmlok["OPSDESCRIPTION"].InnerText + "]]></�ٴ���ʷ>";
                       xml = xml + "<�ٴ����><![CDATA[" + xmlok["DISEASE"].InnerText + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";

                        w.WriteString("pathgethis", "bbsl", "");
                        w.WriteString("pathgethis", "bbmh", "");
                        w.WriteString("pathgethis", "ysbbmh", "");
                        w.WriteString("pathgethis", "ysbbsl", "");
                        w.WriteString("pathgethis", "sqxh", "");
                        //------ȷ���շ�-----
                        //if (PATIENTTYPE == "סԺ")
                        //{
                        //    if (MessageBox.Show("�˲���δ�շѣ��Ƿ��շ�", "�Ƿ��շ�", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //    {
                        //        string msgSF = "";
                        //        try
                        //        {
                        //            msgSF = yz2y.AddFee(xmlok["HIS_KEYNO"].InnerText, decimal.Parse("11"), "2");
                        //        }
                        //        catch (Exception ee)
                        //        {

                        //            MessageBox.Show("�շ�ʧ�ܣ�");
                        //            if (Debug == "1")
                        //                log.WriteMyLog("�շ�ʧ��," + ee.ToString());
                        //        }
                        //        if (msgSF == "0") MessageBox.Show("�շѳɹ���");

                        //    }
                        //}
                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        return "0";
                    }
                }
                //----------------------------------------------------------------------------
                if (Sslbx == "���뵥��")
                {
                      //----------------------------------------------------------------
                    //--�ж����ݿ����Ƿ����д�����Ų�����Ϣ-----------------------------------------------------
                    
                    DataTable brxx_1 = new DataTable();

                    brxx_1 = aa.GetDataTable("select F_BLH,F_XM,F_SQXH from T_jcxx where F_SQXH='" + Ssbz + "'", "blxx");
                    aa.Close();
                    if (brxx_1.Rows.Count >= 1)
                    {
                        MessageBox.Show("�˲���:" + brxx_1.Rows[0]["F_xm"].ToString() + "������ţ�" + brxx_1.Rows[0]["F_sqxh"].ToString() + "������ţ�" + brxx_1.Rows[0]["F_blh"].ToString() + "���ѵǼǹ��������ظ��Ǽ�!!!");
                        DialogResult dr = MessageBox.Show("�Ƿ��ظ��Ǽǣ�����", "�Ƿ��ظ��Ǽ�", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (dr == DialogResult.No)
                            return xmlstr();
                    }
                   
                    //------------------------------------------------------------------------------------
                    LGHISJKZGQ.nbyz2yWEB.PisServiceLJ yz2y = new LGHISJKZGQ.nbyz2yWEB.PisServiceLJ();
                    if (pathWEB != "") yz2y.Url = pathWEB;
                    string sBillInfo = "";
                    string sSampleInfo = "";
                    string Mes = "";

                   

                    try
                    {
                        Mes = yz2y.PatBillWritePIS(Ssbz,ref sBillInfo,ref sSampleInfo, 0);
                        //log.WriteMyLog(sBillInfo + "&" + sSampleInfo);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("�����뵥�Ų����ڣ���˶ԣ�");
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        log.WriteMyLog("������" + Ssbz + "����ֵ��" + sBillInfo + "��ʾ��Ϣ��" + Mes);

                        return "0";
                    }
                    //--------------------------------
                    if (sBillInfo.Trim() == "" || sBillInfo == null)
                    {
                        MessageBox.Show("�����뵥������Ϊ�գ�");
                        return "0";
                    }
                 
                
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(sBillInfo);
                        xmlok = xd.SelectSingleNode("/BILL");
                    }
                    catch { if (Debug == "1") MessageBox.Show("XML��������"); return "0"; }
                    //�걾����------------------------------------------------------------

                    XmlDocument xd1 = new XmlDocument();
                    string bbmc = "";
                    string bbbm = "";
                    int index = 0;
                    try
                    {
                        xd1.LoadXml(sSampleInfo);
                        XmlNodeList xnl = xd1.SelectNodes("/SAMPLES/SAMPLE");
                        index = xnl.Count;
                        //if (index > 1)
                        //    MessageBox.Show("�˲��˹���" + index + "���걾!");
                        for (int x = 0; x < xnl.Count; x++)
                        {
                            XmlNode xmlok1 = xnl[x];
                            bbmc = bbmc.Trim() + " " + xmlok1["COLLECTBODY"].InnerText;
                            if (bbbm == "")
                                bbbm = xmlok1["SAMPLENO"].InnerText;
                            else
                                bbbm = bbbm + "," + xmlok1["SAMPLENO"].InnerText;
                        }
                    }
                    catch { if (Debug == "1") MessageBox.Show("�걾XML��������"); return "0"; }
                    if (Mes != "")
                    {
                        MessageBox.Show(Mes); return "0";
                    }

                  
                    //------------------- �շѽ��-------------------------
                    //int jine = 0;//���
                    //string sBillInfo1 = "";
                    //string sSampleInfo1 = "";
                    //string schargeInfo1 = "";//�շ���Ϣ
                    //MessageBox.Show("1");
                    //try
                    //{
                    //    string mesaage = yz2y.PatBillChargeWritePIS(Ssbz.Trim(), ref  sBillInfo1, ref  sSampleInfo1, ref  schargeInfo1, 0);
                    //    MessageBox.Show("2");
                    //}
                    //catch(Exception ew)
                    //{
                    //    MessageBox.Show(ew.ToString()); log.WriteMyLog(schargeInfo1);
                    //    throw;
                    //}
                   
                    
                    //XmlNode xmlok = null;
                    //XmlDocument xd = new XmlDocument();

                    //try
                    //{
                    //    xd.LoadXml(sBillInfo);
                    //    xmlok = xd.SelectSingleNode("/BILL");
                    //}
                    //catch
                    //{
                    //    if (Debug == "1") log.WriteMyLog("XML��������");
                    //    throw;
                    //}


                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + xmlok["MEDRECNO"].InnerText + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + xmlok["HIS_KEYNO"].InnerText + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + xmlok["OUTPATIENTNO"].InnerText + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + xmlok["INHOSPITALNO"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + xmlok["PNAME"].InnerText + (char)34 + " ";
                        string PSEX = xmlok["PSEX"].InnerText;
                        if (PSEX == "F") PSEX = "Ů";
                        else
                        {
                            if (PSEX == "M") PSEX = "��"; else PSEX = " ";
                        }
                        xml = xml + "�Ա�=" + (char)34 + PSEX + (char)34 + " ";
                        string nl1 = xmlok["PBIRTHDAY"].InnerText;
                        string nl = "0��";
                        try
                        { nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString() + "��"; }
                        catch
                        { nl = "0��"; }
                        xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                        string MARRIED = xmlok["MARRIED"].InnerText;
                        switch (MARRIED)
                        {
                            case "Y": MARRIED = "�ѻ�"; break;
                            case "W": MARRIED = "δ��"; break;
                            case "L": MARRIED = "����"; break;
                            case "S": MARRIED = "ɥż"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "����=" + (char)34 + MARRIED + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + xmlok["ADDRESS"].InnerText + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + xmlok["PTELEPHONENO"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + xmlok["WARD"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + xmlok["BEDNO"].InnerText + (char)34 + " ";
                        //xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        // xml = xml + "����=" + (char)34 +""+ (char)34 + " ";

                        xml = xml + "���֤��=" + (char)34 + xmlok["IDCARD"].InnerText + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + xmlok["VOCATION"].InnerText + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + xmlok["SUBMITDEPT"].InnerText + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + xmlok["SUBMITDOC"].InnerText + (char)34 + " ";
                        //xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
                        //xml = xml + "�ͼ�ҽ��=" + (char)34 +"" + (char)34 + " ";

                        //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
                        //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + xmlok["MEDICALTYPE"].InnerText + (char)34 + " ";
                        string PATIENTTYPE = xmlok["PATIENTTYPE"].InnerText;
                        if (PATIENTTYPE == "1000") PATIENTTYPE = "����";
                        if (PATIENTTYPE == "2000") PATIENTTYPE = "����";
                        if (PATIENTTYPE == "3000") PATIENTTYPE = "סԺ";
                        if (PATIENTTYPE == "4000") PATIENTTYPE = "���";
                        xml = xml + "�������=" + (char)34 + PATIENTTYPE + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "�ٴ���ʷ:" + xmlok["HISCONSULTATION"].InnerText + "          ����������" + xmlok["OPERATIONRESULT"].InnerText + "      Ӱ��ѧ������" + xmlok["OPSDESCRIPTION"].InnerText + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + xmlok["DISEASE"].InnerText + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        // ȷ���շ�
                        //if (PATIENTTYPE == "סԺ")
                        //{
                        //    if (MessageBox.Show("�Ƿ��շ�", "�Ƿ��շ�", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        //    {
                        //        string msgSF = "";
                        //        try
                        //        {
                        //            msgSF = yz2y.AddFee(xmlok["HIS_KEYNO"].InnerText, decimal.Parse("11"), "2");
                        //        }
                        //        catch (Exception ee)
                        //        {
                        //            MessageBox.Show("�շ�ʧ�ܣ�");
                        //        }

                        //        if (msgSF == "0") MessageBox.Show("�շѳɹ���");

                        //    }
                        //}
                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        return "0";
                    }
                }
                if (Sslbx == "�������뵥")
                {

                    string inxml = "";
                    inxml = inxml + "<?xml version='1.0' encoding='GB2312'?>";
                    inxml = inxml + "<REPORTINFO>";
                    inxml = inxml + "<ITEM>";
                    inxml = inxml + "<SQDBH>" + decimal.Parse(Ssbz.ToString().Trim()) + "</SQDBH>";
                    inxml = inxml + "<ZT>" + "99" + "</ZT>";
                    inxml = inxml + "<JSRY></JSRY>";
                    inxml = inxml + "<JSSJ></JSSJ>";
                    inxml = inxml + "<BGRY></BGRY>";
                    inxml = inxml + "<BGSJ></BGSJ>";
                    inxml = inxml + "<SHRY></SHRY>";
                    inxml = inxml + "<SHSJ></SHSJ>";
                    inxml = inxml + "<CXRY></CXRY>";
                    inxml = inxml + "<CXSJ>" + DateTime.Now.ToString("yyyyMMddHHMMss") + "</CXSJ>";
                    inxml = inxml + "<JCSJ></JCSJ>";
                    inxml = inxml + "<JCJL></JCJL>";
                    inxml = inxml + "<WEBURL></WEBURL>";
                    inxml = inxml + "<JCH></JCH>";
                    inxml = inxml + "</ITEM>";
                    inxml = inxml + "</REPORTINFO>";

                    if (Debug == "1")
                    {
                        log.WriteMyLog("�ش���xml���ݣ�" + inxml);
                    }
                    string outxml = "";
                    LGHISJKZGQ.nbyz2yWEB.PisServiceLJ yz2yweb = new LGHISJKZGQ.nbyz2yWEB.PisServiceLJ();

                    //--------�˷�

                    string msgstr = "";
                    try
                    {
                        if (pathWEB != "") yz2yweb.Url = pathWEB;
                        msgstr = yz2yweb.DelFee(Ssbz, decimal.Parse("11"), "4");
                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show("�˷�ʧ�ܣ�");
                        log.WriteMyLog("�˷�ʧ�ܣ�����HIS�ӿڳ���" + ee.ToString());
                        return "0";
                    }
                    if (msgstr == "0")
                        MessageBox.Show("�˷ѳɹ�������");
                    else
                        log.WriteMyLog("�˷�ʧ�ܣ�" + msgstr.ToString());
                    // ��������
                    try
                    {
                        if (pathWEB != "") yz2yweb.Url = pathWEB;
                        outxml = yz2yweb.SetPISReportInfo(decimal.Parse(Ssbz), inxml);
                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show("���뵥����ʧ�ܣ���ȷ�����뵥���Ƿ���ڣ�");
                        log.WriteMyLog("���뵥����ʧ�ܣ�����HIS�ӿڳ���" + ee.ToString());
                        return "0";
                    }

                    if (outxml == "")
                        MessageBox.Show("���뵥�����ɹ���");
                     else
                    {
                        log.WriteMyLog("���뵥����ʧ�ܣ�ԭ��" + outxml);
                        if (Debug == "1")
                            MessageBox.Show("���뵥����ʧ�ܣ�" + outxml);
                        return "0";
                    }
                 
                        return "0";
                    
                }
            //        //�Ʒ�
                //if (Sslbx == "�Ʒ�")
                //{
                //    PATHGETHISZGQ.nbyz2yWEB.PisServiceLJ yz2yweb = new PATHGETHISZGQ.nbyz2yWEB.PisServiceLJ();
                //    string msgstr = "";
                //    try
                //    {
                //        if (pathWEB != "") yz2yweb.Url = pathWEB;
                //        msgstr = yz2yweb.AddFee(Ssbz, decimal.Parse("11"), "2");
                //    }
                //    catch (Exception ee)
                //    {

                //        MessageBox.Show("���뵥����ʧ�ܣ���ȷ�����뵥���Ƿ���ڣ�");
                //        log.WriteMyLog("���뵥����ʧ�ܣ�����HIS�ӿڳ���" + ee.ToString());
                //        return "0";
                //    }
                //    MessageBox.Show(msgstr);
                //    return "0";
                //}

                MessageBox.Show("�޴�" + Sslbx);
                return "0";
           
            }
            else
            {
                MessageBox.Show("�޴�" + Sslbx);
                if (Debug == "1")
                    log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");

                return "0";
            }

        }
        public static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "���˱��=" + (char)34 + (char)34 + " ";
            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + (char)34 + " ";
            xml = xml + "�����=" + (char)34 + (char)34 + " ";
            xml = xml + "סԺ��=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";

            xml = xml + "�Ա�=" + (char)34 + (char)34 + " ";

            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "��ַ=" + (char)34 + (char)34 + "   ";
            xml = xml + "�绰=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "���֤��=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "ְҵ=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽ��=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "�ͼ�ҽ��=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
            xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 +"��Ժ"+ (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";

            xml = xml + "�������=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + "]]></�ٴ����>";
            xml = xml + "</LOGENE>";
            return xml;
        }

        }
}


