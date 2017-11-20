using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using HL7;
using System.Xml;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    //��ҽ�����Ժ ����ƽ̨  webservice+hl7��ȡ������Ϣ
    class AYD2FY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            if (Sslbx.Trim() == "")
            {
                MessageBox.Show("�޴�" + Sslbx);
                return "0";
            }

            #region ��ȡSZ��T_SZ����


            string xtdm = f.ReadString(Sslbx, "xtdm", "2060000").Replace("\0", "").Trim();
            string WebUrl = f.ReadString(Sslbx, "WebUrl", "http://223.220.200.45:1506/services/WSInterface?wsdl").Replace("\0", "").Trim();
            debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim();
         
            string certificate = f.ReadString(Sslbx, "certificate", "ZmmJ9RMCKAUxFsiwl/08iiA3J17G0OpI").Replace("\0", "").Trim();
            #endregion

   

            #region ��ȡ������Ϣ
            string OSQ = "";
            if ( Sslbx == "����")
            {

                if (Ssbz.Length > 19)
                {
                    try
                    {
                        Ssbz = Ssbz.Substring(9, 10);
                    }
                    catch
                    {
                        log.WriteMyLog("��ȡ�����쳣Substring(9, 10)��" + Ssbz);
                    }
                }
                if (debug == "1")
                    log.WriteMyLog("���ţ�"+Ssbz);

                string rtn = SP_SELECT.ptxml2(Sslbx, Ssbz, debug, "");
                if (rtn != "0")
                    return rtn;

                if (debug == "1")
                    log.WriteMyLog("δ��ѯ�����뵥��¼����ȡ������Ϣ");

                OSQ = "<root><patientId></patientId><visitNo>" + Ssbz + "</visitNo></root>";

            }else
            if (Sslbx == "�����")
            {

                string rtn = SP_SELECT.ptxml2(Sslbx, Ssbz, debug,"");
                if (rtn != "0")
                    return rtn;

                if (debug == "1")
                    log.WriteMyLog("δ��ѯ�����뵥��¼����ȡ������Ϣ");
              
                OSQ = "<root><patientId></patientId><visitNo>"+Ssbz+"</visitNo></root>";

            }
            else
                if (Sslbx == "סԺ��")
                {



                    string rtn = SP_SELECT.ptxml2(Sslbx, Ssbz, debug,"");
                   if (rtn != "0")
                       return rtn;

                    if(debug=="1")
                   log.WriteMyLog("δ��ѯ�����뵥��¼����ȡ������Ϣ");

                    OSQ = "<root><patientId></patientId><visitNo>"+Ssbz+"</visitNo></root>";
                }
                    else
                    {
                        MessageBox.Show("�޴�ʶ������" + Sslbx);
                        return "0";
                    }

              aydefyweb.WSInterface ayd2yy = new LGHISJKZGQ.aydefyweb.WSInterface();
              if (WebUrl.Trim() != "")
                  ayd2yy.Url = WebUrl;

              string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>GetPatientBaseInfoIndex</serverName>"
              + "<format>xml</format><callOperator></callOperator><certificate>" + certificate + "</certificate></root>";

            if (debug == "1")
                log.WriteMyLog("��Σ�msgHeader:" + msgHeader + "\r\nmsgBody" + OSQ);


            try
            {
              
                string rtn="";
                try
                {
                   
                    rtn = ayd2yy.CallInterface(msgHeader, OSQ);
                }
                catch(Exception e1)
                {
                    MessageBox.Show("����webservice�쳣��" + e1.Message);
                    return "0";
                }
                if (debug == "1")
                    log.WriteMyLog("���أ�" + rtn);

                if(rtn=="<?xml version=\"1.0\" encoding=\"utf-8\"?><root/>")
                {   MessageBox.Show("δ��ѯ��������Ϣ");
                    return "0";
                }

                 PT_XML px = new PT_XML();

                
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(rtn);
                        xmlok = xd.SelectSingleNode("/root/patients/patient");
                     }
                catch
                {
                    MessageBox.Show("δ��ѯ��������Ϣ");
                       return "0";
                }


                       px.myDictionary["����"] = xmlok["patientName"].InnerText;
                        string xb = xmlok["patientSex"].InnerText;
                        if (xb == "1") xb = "Ů";
                        else if (xb.Trim() == "0") xb = "��";
                        else xb = "";
                        px.myDictionary["�Ա�"] = xb;

                        if (Sslbx.Trim() == "סԺ��")
                        {   px.myDictionary["�������"] = "סԺ";
                             px.myDictionary["סԺ��"] =  xmlok["visitNo"].InnerText;
                             try
                             {
                                 px.myDictionary["�ͼ����"] = xmlok["admissionDept"].InnerText;
                             }
                             catch
                             {
                             }
                             px.myDictionary["����ID"] = xmlok["visitNum"].InnerText;
                        }
                            else
                        {
                                px.myDictionary["�������"] = "����";
                               px.myDictionary["�����"] =  xmlok["visitNo"].InnerText;
                               try
                               {
                                   px.myDictionary["�ͼ����"] = xmlok["cureDept"].InnerText;
                               }
                               catch
                               {
                               }
                               if (Sslbx.Trim() == "����")
                                   px.myDictionary["����ID"] = Ssbz;
                        }
                        
                         
                        px.myDictionary["����"] =ZGQClass.CsrqToAge(xmlok["patientBirthdate"].InnerText);   
                        px.myDictionary["��ַ"] = xmlok["commPostCode"].InnerText;
                        px.myDictionary["�绰"] =xmlok["telephone"].InnerText;
                        px.myDictionary["���˱��"] = xmlok["patientId"].InnerText;
                    
                        px.myDictionary["�������"] = "";
                        px.myDictionary["�ͼ�ҽ��"] = "";
                        px.myDictionary["�ѱ�"] ="";
                        px.myDictionary["�ٴ����"] = "";
                        px.myDictionary["����"] = "";
                        px.myDictionary["���֤��"] =  xmlok["identityNo"].InnerText;
                        
                        string exep = "";
                        return px.rtn_XML(ref exep);

                   
                }
                catch (Exception ee)
                {
                    MessageBox.Show( ee.Message);
                  
                    return "0";
                }
         
            #endregion

        }


        public static string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string WebUrl, string debug, string certificate)
        {

          
         

            //����ƽ̨��ַ

           
            aydefyweb.WSInterface ayd2yy = new LGHISJKZGQ.aydefyweb.WSInterface();


            if (WebUrl.Trim() != "")
                ayd2yy.Url = WebUrl;

            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";


            if (debug == "1")
                log.WriteMyLog("��Σ�msgHeader:" + msgHeader + "\r\nmsgBody" + msgBody);


            try
            {
                MessageBox.Show(ayd2yy.Url);
                string rtn = ayd2yy.CallInterface(msgHeader, msgBody);

                if (debug == "1")
                    log.WriteMyLog("���أ�" + rtn);
                return rtn;
            }
            catch (Exception ee)
            {
                MessageBox.Show("����webservice�쳣��" + ee.Message);
                return "-1";
            }
        }

        //private static string jxhl7(string rtn_msg, string Ssbz, string Sslbx, string isbrxx, string WebUrl, string debug)
        //{

        //    int xh = 0;
        //    PT_XML px = new PT_XML();
        //    readhl7 r7 = new readhl7();
        //    int count = 0;
        //    try
        //    {
        //        r7.Adt01(rtn_msg, ref count);

        //        if (r7.MSA[1].Trim() == "AA")
        //        {
        //            try
        //            {

        //                if (count >= 1)
        //                {
        //                    DataTable dt = new DataTable();
        //                    DataColumn dc0 = new DataColumn("���");
        //                    dt.Columns.Add(dc0);
        //                    DataColumn dc1 = new DataColumn("�������");
        //                    dt.Columns.Add(dc1);
        //                    DataColumn dc2 = new DataColumn("����");
        //                    dt.Columns.Add(dc2);
        //                    DataColumn dc3 = new DataColumn("�������");
        //                    dt.Columns.Add(dc3);

        //                    DataColumn dc4 = new DataColumn("�ͼ����");
        //                    dt.Columns.Add(dc4);
        //                    DataColumn dc5 = new DataColumn("�ͼ�ҽ��");
        //                    dt.Columns.Add(dc5);

        //                    DataColumn dc6 = new DataColumn("ҽ����Ŀ");
        //                    dt.Columns.Add(dc6);
        //                    DataColumn dc7 = new DataColumn("����");
        //                    dt.Columns.Add(dc7);
        //                    DataColumn dc8 = new DataColumn("�걾����");
        //                    dt.Columns.Add(dc8);


        //                    for (int x = 0; x < count; x++)
        //                    {
        //                        DataRow dr1 = dt.NewRow();
        //                        dt.Rows.Add(dr1);
        //                        dt.Rows[x][0] = x;

        //                        //�������
        //                        dt.Rows[x][1] = r7.ORC[x, 2].Trim();
        //                        //����
        //                        dt.Rows[x][2] = r7.PID[5].Split('^')[0].Trim();
        //                        //��������
        //                        dt.Rows[x][3] = r7.QRF[4].Trim();
        //                        if (dt.Rows[x][3].ToString() == "I")
        //                            dt.Rows[x][3] = "סԺ";
        //                        else
        //                            dt.Rows[x][3] = "����";

        //                        //����
        //                        dt.Rows[x][4] = r7.ORC[xh, 13].Trim().Split('^')[0];
        //                        //ҽ��
        //                        dt.Rows[x][5] = r7.ORC[x, 12].Trim();
        //                        //��Ŀ
        //                        dt.Rows[x][6] = r7.OBR[x, 4].Trim();
        //                        //����
        //                        dt.Rows[x][7] = r7.OBR[x, 23].Trim();
        //                        //�걾����
        //                        string bbmc = r7.OBR[x, 15].Trim();
        //                        if (bbmc.Trim() == "^") bbmc = "";
        //                        dt.Rows[x][8] = bbmc;

        //                    }
        //                    Frm_FJSFYBJY ffj = new Frm_FJSFYBJY(dt, "30^80^80^40^120^100^250^70^150");
        //                    ffj.ShowDialog();
        //                    if (ffj.xh == "")
        //                    {
        //                        MessageBox.Show("δѡ���˼���¼");
        //                        return "0";
        //                    }
        //                    xh = int.Parse(ffj.xh);

        //                }

        //                px.myDictionary["����"] = r7.PID[5].Split('^')[0].Trim();

        //                string xb = r7.PID[8].Trim();
        //                if (xb == "F") xb = "Ů";
        //                else if (xb.Trim() == "M") xb = "��";
        //                else xb = "Ů";
        //                px.myDictionary["�Ա�"] = xb;

        //                string brlb = r7.QRF[4].Trim();
        //                if (brlb == "I") brlb = "סԺ";
        //                else brlb = "����";

        //                px.myDictionary["�������"] = brlb;

        //                px.myDictionary["��������"] = r7.PID[7].Trim();
        //                if (r7.PID[13].Trim() != "")
        //                    px.myDictionary["��ַ"] = r7.PID[13].Trim();
        //                if (r7.PID[11].Trim() != "")
        //                    px.myDictionary["�绰"] = "^" + r7.PID[11].Trim();

        //                string hy = r7.PID[15].Trim();
        //                switch (hy)
        //                {
        //                    case "D": hy = "���"; break;
        //                    case "M": hy = "�ѻ�"; break;
        //                    case "W": hy = "ɥż"; break;
        //                    case "V": hy = "δ��"; break;
        //                    default: hy = ""; break;

        //                }
        //                px.myDictionary["����"] = hy;




        //                foreach (string pid3 in r7.PID[3].Trim().Split('~'))
        //                {
        //                    try
        //                    {
        //                        //���֤��
        //                        if (pid3.Split('^')[4] == "PN")
        //                            px.myDictionary["���֤��"] = pid3.Split('^')[0].Trim();

        //                    }
        //                    catch
        //                    {
        //                    }
        //                    try
        //                    {
        //                        //PI  patientid
        //                        if (pid3.Split('^')[4] == "PI")
        //                            px.myDictionary["���˱��"] = pid3.Split('^')[0].Trim();
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    try
        //                    {
        //                        //������
        //                        if (pid3.Split('^')[4] == "VN")
        //                            px.myDictionary["����"] = pid3.Split('^')[0].Trim();
        //                    }
        //                    catch
        //                    {
        //                    } try
        //                    {

        //                        //ҽ����
        //                        if (pid3.Split('^')[4] == "VN")
        //                            px.myDictionary["����ID"] = pid3.Split('^')[0].Trim();
        //                    }
        //                    catch
        //                    {
        //                    }

        //                }


        //                if (brlb == "����")
        //                    px.myDictionary["�����"] = px.myDictionary["����"];
        //                if (brlb == "סԺ")
        //                    px.myDictionary["סԺ��"] = px.myDictionary["����"];


        //                px.myDictionary["����"] = r7.NTE[3].Trim();

        //                px.myDictionary["�ͼ����"] = r7.ORC[xh, 13].Trim().Split('^')[0];

        //                px.myDictionary["����"] = r7.ORC[xh, 13].Trim().Split('^')[0];


        //                px.myDictionary["�������"] = r7.ORC[xh, 2].Trim();
        //                px.myDictionary["�ͼ�ҽ��"] = r7.ORC[xh, 12].Trim().Split('^')[1].Trim();

        //                string sqx = r7.OBR[0, 2].Trim();

        //                px.myDictionary["ҽ����Ŀ"] = r7.OBR[xh, 4].Trim();


        //                string bbmc1 = r7.OBR[xh, 15].Trim();
        //                if (bbmc1.Trim() == "^") bbmc1 = "";
        //                px.myDictionary["�걾����"] = bbmc1;

        //                px.myDictionary["�շ�"] = r7.OBR[xh, 23].Trim();
        //                //try
        //                //{
        //                //    px.myDictionary["�ٴ���ʷ"] = r7.OBR[xh, 46].Trim().Split('~')[1].Replace("2^", "").Trim();
        //                //}
        //                //catch
        //                //{
        //                //    px.myDictionary["�ٴ���ʷ"] = "";
        //                //}

        //                foreach (string lczd in r7.OBR[xh, 46].Trim().Split('~'))
        //                {
        //                    //lczd
        //                    try
        //                    {
        //                        if (lczd.Split('^')[0] == "6")
        //                        {
        //                            px.myDictionary["�ٴ����"] = lczd.Split('^')[1];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    //����
        //                    try
        //                    {
        //                        if (lczd.Split('^')[0] == "1")
        //                        {
        //                            px.myDictionary["�ٴ���ʷ"] = px.myDictionary["�ٴ���ʷ"] + lczd.Split('^')[1];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    // ��ʷ
        //                    try
        //                    {
        //                        if (lczd.Split('^')[0] == "2")
        //                        {
        //                            px.myDictionary["�ٴ���ʷ"] = px.myDictionary["�ٴ���ʷ"] + lczd.Split('^')[1];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                }

        //                if (px.myDictionary["����"].Trim() == "")
        //                    px.myDictionary["����"] = ZGQClass.CsrqToAge(px.myDictionary["��������"]);

        //                string exep = "";
        //                //   MessageBox.Show(px.rtn_XML(ref exep));

        //                return px.rtn_XML(ref exep);
        //            }
        //            catch (Exception e2)
        //            {
        //                MessageBox.Show(e2.Message);
        //                return "0";
        //            }
        //        }
        //        else
        //        {
        //            //ȡ������Ϣ
        //            if (isbrxx == "1")
        //            {
        //                if (debug == "1")
        //                    log.WriteMyLog("δȡ�����뵥��Ϣ����ȡ���˻�����Ϣ");

        //                if (Sslbx.Trim() == "סԺ��" || Sslbx.Trim() == "�����")
        //                {
        //                    string rtn_msg2 = "";
        //                    if (Sslbx.Trim() == "סԺ��")
        //                    {

        //                        //if (isbrxx.Trim() != "1")
        //                        //{
        //                        //    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
        //                        //    return "0";
        //                        //}
        //                        string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><visitNo></visitNo><inpNo>" + Ssbz + "</inpNo><patientId></patientId><wardCode></wardCode><deptCode></deptCode></root>";
        //                        rtn_msg2 = rtn_CallInterface("XML", "GetPatientInHospital", XML2, "", WebUrl, debug);
        //                    }
        //                    else
        //                    {

        //                        //if (isbrxx.Trim() != "1")
        //                        //{
        //                        //    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
        //                        //    return "0";
        //                        //}

        //                        string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><icCardNo>" + Ssbz + "</icCardNo><executeDept></executeDept></root>";
        //                        rtn_msg2 = rtn_CallInterface("XML", "GetDispPatientInfo", XML2, "", WebUrl, debug);
        //                    }



        //                    if (rtn_msg2 == "")
        //                        return "0";


        //                    readhl7 rr7 = new readhl7();
        //                    int xy = 0;
        //                    rr7.Adt01(rtn_msg2, ref xy);
        //                    if (rr7.MSA[1].Trim() != "AA")
        //                    {
        //                        MessageBox.Show(rr7.MSA[3].Trim());

        //                        return "0";
        //                    }
        //                    else
        //                    {
        //                        XmlNode xmlok = null;
        //                        XmlDocument xd = new XmlDocument();
        //                        try
        //                        {
        //                            xd.LoadXml(rtn_msg2);
        //                            xmlok = xd.SelectSingleNode("/root/patients");
        //                            if (xmlok.InnerXml.Trim() != "")
        //                            {
        //                                DataSet ds1 = new DataSet();
        //                                StringReader sr = new StringReader(xmlok.InnerXml);
        //                                XmlReader xr = new XmlTextReader(sr);
        //                                ds1.ReadXml(xr);

        //                                px.myDictionary["����"] = ds1.Tables[0].Rows[0]["patientName"].ToString().Trim();
        //                                string xb = ds1.Tables[0].Rows[0]["patientSex"].ToString().Trim();
        //                                if (xb == "1") xb = "Ů";
        //                                else if (xb.Trim() == "0") xb = "��";
        //                                else xb = "";
        //                                px.myDictionary["�Ա�"] = xb;
        //                                if (Sslbx.Trim() == "סԺ��")
        //                                {
        //                                    px.myDictionary["�������"] = "סԺ";
        //                                    px.myDictionary["����ID"] = ds1.Tables[0].Rows[0]["visitCount"].ToString().Trim();
        //                                    px.myDictionary["סԺ��"] = ds1.Tables[0].Rows[0]["visitNo"].ToString().Trim();
        //                                    px.myDictionary["����"] = ds1.Tables[0].Rows[0]["bedCode"].ToString().Trim();
        //                                    px.myDictionary["����"] = ds1.Tables[0].Rows[0]["wardName"].ToString().Trim();
        //                                }
        //                                else
        //                                {
        //                                    px.myDictionary["�������"] = "����";
        //                                    px.myDictionary["�����"] = ds1.Tables[0].Rows[0]["icCardNo"].ToString().Trim();
        //                                    px.myDictionary["����"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();
        //                                }
        //                                if (ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() != "")
        //                                {

        //                                    try
        //                                    {
        //                                        px.myDictionary["����"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() + ds1.Tables[0].Rows[0]["ageUnit"].ToString().Trim();
        //                                    }
        //                                    catch
        //                                    {
        //                                        px.myDictionary["����"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim();
        //                                    }
        //                                }
        //                                px.myDictionary["��ַ"] = "�绰��" + ds1.Tables[0].Rows[0]["telephone"].ToString().Trim();
        //                                px.myDictionary["�绰"] = "^��ַ��";
        //                                px.myDictionary["���˱��"] = ds1.Tables[0].Rows[0]["patientId"].ToString().Trim();
        //                                px.myDictionary["�ͼ����"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();

        //                                px.myDictionary["�������"] = "";
        //                                px.myDictionary["�ͼ�ҽ��"] = ds1.Tables[0].Rows[0]["doctorInCharge"].ToString().Trim().Split('/')[0].Trim();
        //                                px.myDictionary["�ѱ�"] = ds1.Tables[0].Rows[0]["rateTypeName"].ToString().Trim();
        //                                try
        //                                {
        //                                    px.myDictionary["�ٴ����"] = ds1.Tables[0].Rows[0]["diagnosisname"].ToString().Trim();
        //                                }
        //                                catch
        //                                {
        //                                    px.myDictionary["�ٴ����"] = "";
        //                                }

        //                                string exep = "";
        //                                return px.rtn_XML(ref exep);

        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("δ�ܲ�ѯ�����˼�¼");
        //                                return "0";
        //                            }
        //                        }
        //                        catch (Exception ee)
        //                        {
        //                            MessageBox.Show("XML��������:" + ee.Message);
        //                            log.WriteMyLog(rtn_msg2 + "--" + ee.Message);
        //                            return "0";
        //                        }
        //                    }


        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
        //            }


        //            return "0";
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.Message);
        //        return "0";

        //    }
        //}


    }
}
