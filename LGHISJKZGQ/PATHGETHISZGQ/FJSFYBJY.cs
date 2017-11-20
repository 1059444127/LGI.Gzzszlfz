using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;
using LGHISJKZGQ;
using PATHZGQ_HL7;
using dbbase;
using PATHGETHISZGQ;
namespace LGHISJKZGQ
{
    /// <summary>
    ///  ����ʡ���ױ���Ժ  
    /// webservice+HL7��ʽ
    /// </summary>
    class FJSFYBJY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        static string url = "";
        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
           // string certificate = f.ReadString(Sslbx, "pzh", "").Replace("\0", "").Trim();
             url = f.ReadString(Sslbx, "url", "").Replace("\0", "").Trim();

            if (Sslbx.Trim() == "")
            {
                MessageBox.Show("�޴�" + Sslbx);
                return "0";
            }

            ////ȡ���������------------------------------------------------

            if (Sslbx == "�۷ѳ���(�����)" || Sslbx == "�۷ѳ���(�����)")
            {

                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt = new DataTable();
                  if(Sslbx == "�۷ѳ���(�����)")
                dt = aa.GetDataTable("select * from T_JCXX where F_BLH='" + Ssbz .Trim()+ "'", "d1");
                  else
                dt = aa.GetDataTable("select * from T_JCXX where F_SQXH='" + Ssbz.Trim() + "' and  F_SDRQ>='"+DateTime.Today.ToString("yyyy-MM-dd")+"'", "d1");
                if (dt == null)
                {
                    MessageBox.Show("���ݿ����Ӵ���");
                }
                if (dt.Rows.Count > 0)
                {
                    string brlb = dt.Rows[0]["F_brlb"].ToString().Trim();
                    if (brlb == "סԺ") brlb = "I";
                    else brlb = "O";

                 string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();

                string brbh = dt.Rows[0]["F_brbh"].ToString().Trim();
                        string sqxh = dt.Rows[0]["F_sqxh"].ToString().Trim();
                        if (brbh.Trim() != "" && sqxh.Trim() != "")
                        {
                            string SendReturnfee_xml = "<root><patientType>" + brlb + "</patientType><patientId>" + brbh + "</patientId><operationType>2</operationType>"
                                + "<executeDeptCode>JC06</executeDeptCode><executeDoctorCode>" + CZYGH + "</executeDoctorCode><applyNo>" + sqxh + "</applyNo></root>";

                            if (debug == "1")
                                MessageBox.Show(SendReturnfee_xml);

                            string rtn_msg2 = rtn_CallInterface("XML", "SendReturnfee", SendReturnfee_xml, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
                            MessageBox.Show(rtn_msg2);
                        }
                        else
                        {
                            MessageBox.Show("�����������Ϊ�ջ���ID��Ϊ�գ����ܳ����۷�");
                        }

                }
                else
                {
                    MessageBox.Show("δ�鵽��Ӧ�����ݣ�");
                }
                return "0";

                //// qxjcsq(Sslbx,Ssbz);

                ////ȡ���۷�
                //string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                //string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                //string brbh = jcxx.Rows[0]["F_BRBH"].ToString().Trim();
                //string brlb = jcxx.Rows[0]["F_brlb"].ToString().Trim();
                //string sqxh = jcxx.Rows[0]["F_SQXH"].ToString().Trim();
                //if (brlb == "סԺ") brlb = "I";
                //else brlb = "O";


                //if (bgzt == "�ѵǼ�" || bgzt == "��ȡ��" || bgzt == "��д����" || bgzt == "��������")
                //{
                //    if (jcxx.Rows[0]["F_FYQR"].ToString().Trim() != "1")
                //    {
                //        //�ϴ�����״̬�Ϳ۷�
                //        string XML = "<root><patientType>" + brlb + "</patientType><patientId>" + brlb + "</patientId><operationType>1</operationType><executeDeptCode>JC06</executeDeptCode>"
                //                    + "<executeDoctorCode>" + CZYGH + "</executeDoctorCode><applyNo>" + sqxh + "</applyNo></root>";
                //        string rtn_msg = rtn_CallInterface("XML", "SendDocumentsFee", XML, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
                //        MessageBox.Show(rtn_msg);
                //    }
                //    return "0";
                //}
            }

                ////��ȡ��Ϣ-----------------------------------------------------------
                string OSQ = "";
                if (Sslbx == "����")
                {
                    OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|"
                          + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|"
                          + DateTime.Now.ToString("MMddHHmmss") + "|||^RD|" + Ssbz + "|ORD|^���뵥��||" + "\rQRF|JC06|||O|||||1^^^"
                          + DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss") + "^" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|";

                }
                else
                    if (Sslbx == "סԺ��")
                    {
                        OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|" + DateTime.Now.ToString("yyyyMMddHHmmss")
                            + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|" + DateTime.Now.ToString("MMddHHmmss")
                            + "|||^RD|" + Ssbz + "|ORD|^���뵥��||" + "\rQRF|JC06|||I|||||1^^^" + DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss")
                            + "^" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|";

                    }
                    else
                    {
                        MessageBox.Show("�޴�ʶ������" + Sslbx);
                        return "0";
                    }

                if (debug == "1")
                log.WriteMyLog(OSQ);
                string rtn_msg = rtn_CallInterface("HL7v2", "QueryGmsApply", OSQ, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
                if (debug == "1")
                    log.WriteMyLog(rtn_msg);

                if (rtn_msg.Contains("error") && !rtn_msg.Contains("|"))
                {
                    MessageBox.Show(rtn_msg);
                    return "0";
                }
                else
                {
                    return jxhl7(rtn_msg, Ssbz,Sslbx);
                }

            }


        public static string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string certificate)
        {

          

            fjsfybjyWeb.WSInterface wsif = new fjsfybjyWeb.WSInterface();

            if (url.Trim() != "")
                wsif.Url = url;

            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";
            try
            {
                return wsif.CallInterface(msgHeader, msgBody);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
                return "-1";
            }
        }

        //private static string jxhl7(string rtn_msg, string Ssbz)
        //{

        //    PT_XML px = new PT_XML();
        //    readhl7 r7 = new readhl7();
        //    r7.Adt01(rtn_msg);
        //    try
        //    {
        //        if (r7.MSA[1].Trim() == "AA")
        //        {
        //            try
        //            {
        //                px.myDictionary["����"] = r7.PID[5].Split('^')[0].Trim();

        //                string xb = r7.PID[8].Trim();
        //                if (xb == "F") xb = "Ů";
        //                else if (xb.Trim() == "M") xb = "��";
        //                else xb = "";
        //                px.myDictionary["�Ա�"] = xb;

        //                string brlb = r7.QRF[4].Trim();
        //                if (brlb == "I") brlb = "סԺ";
        //                else brlb = "����";

        //                px.myDictionary["�������"] = brlb;

        //                px.myDictionary["��������"] = r7.PID[7].Trim();

        //                //��ַ�绰λ�û���
        //                px.myDictionary["��ַ"] ="�绰��"+ r7.PID[13].Trim() + "^";// r7.PID[11].Trim();

        //                px.myDictionary["�绰"] ="��ַ��"+ r7.PID[11].Trim();

        //                px.myDictionary["����"] = r7.PID[15].Trim();

        //                px.myDictionary["����ID"] = r7.PID[17].Trim();//ҽ������

        //                px.myDictionary["���˱��"] = r7.PID[3].Trim().Split('^')[0].Trim();

        //                if (brlb == "����")
        //                    px.myDictionary["�����"] = Ssbz;
        //                else
        //                    px.myDictionary["סԺ��"] = Ssbz;
        //                try
        //                {
        //                    px.myDictionary["���֤��"] = r7.PID[3].Trim().Split('^')[8].Trim().Split('~')[1].Trim();
        //                }
        //                catch
        //                {
        //                    px.myDictionary["���֤��"] = "";
        //                }

        //                px.myDictionary["����"] = r7.NTE[3].Trim();

        //                if (brlb == "����")
        //                px.myDictionary["�ͼ����"] = r7.ORC[13].Trim();

        //                px.myDictionary["����"] = r7.ORC[13].Trim();

        //                px.myDictionary["�������"] = r7.ORC[2].Trim();

        //                px.myDictionary["����ҽ��"] = r7.ORC[12].Trim().Split('^')[1].Trim();

        //                px.myDictionary["ҽ����Ŀ"] = r7.OBR[4].Trim();

        //                px.myDictionary["�걾����"] = r7.OBR[15].Trim();

        //                px.myDictionary["�շ�"] = r7.OBR[23].Trim();

        //                px.myDictionary["�ٴ���ʷ"] = r7.OBR[46].Trim();

        //                px.myDictionary["�ٴ����"] = r7.DG1[3].Split('^')[1].Trim();

        //                string exep = "";
        //                return px.rtn_XML(ref exep);
        //            }
        //            catch(Exception e2)
        //            {
        //                MessageBox.Show(e2.ToString());
        //                return "0";
        //            }
        //        }
        //        else
        //        {
        //                MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());   
        //                return "0";
        //        }
        //    }
        //    catch(Exception  ee)
        //    {
        //        MessageBox.Show(ee.ToString());
        //        return "0";

        //    }
        //}

        private static string jxhl7(string rtn_msg, string Ssbz, string Sslbx)
        {
            int xh = 0;
            PT_XML px = new PT_XML();
            readhl7 r7 = new readhl7();
            int count = 0;
            try
            {
            r7.Adt01(rtn_msg, ref count);

                if (r7.MSA[1].Trim() == "AA")
                {
                    try
                    {
        
                if (count > 1)
                {
                    DataTable dt = new DataTable();
                    DataColumn dc0 = new DataColumn("���");
                    dt.Columns.Add(dc0);
                    DataColumn dc1 = new DataColumn("����");
                    dt.Columns.Add(dc1);

                    DataColumn dc3 = new DataColumn("�������");
                    dt.Columns.Add(dc3);
                    DataColumn dc4 = new DataColumn("���˱��");
                    dt.Columns.Add(dc4);
                    DataColumn dc5 = new DataColumn("�ͼ����");
                    dt.Columns.Add(dc5);
                    DataColumn dc6 = new DataColumn("����ҽ��");
                    dt.Columns.Add(dc6);
                    DataColumn dc7 = new DataColumn("�������");
                    dt.Columns.Add(dc7);
                    DataColumn dc8 = new DataColumn("ҽ����Ŀ");
                    dt.Columns.Add(dc8);
                    DataColumn dc9 = new DataColumn("�շ�");
                    dt.Columns.Add(dc9);
                    DataColumn dc10 = new DataColumn("�걾����");
                    dt.Columns.Add(dc10);
                    DataColumn dc11 = new DataColumn("�ٴ����");
                    dt.Columns.Add(dc11);

                    for (int x = 0; x < count; x++)
                    {
                        DataRow dr1 = dt.NewRow();
                        dt.Rows.Add(dr1);
                        dt.Rows[x][0] = x;
                        dt.Rows[x][1] = r7.PID[5].Split('^')[0].Trim();
                       // dt.Rows[x][2] = r7.PID[8].Trim();


                        dt.Rows[x][2] = r7.QRF[4].Trim();
                        if (dt.Rows[x][2].ToString() == "I") dt.Rows[x][2] = "סԺ";
                        else dt.Rows[x][2] = "����";
                            dt.Rows[x][3] = r7.PID[3].Trim().Split('^')[0].Trim();
                        dt.Rows[x][4] = r7.ORC[x, 13].Trim();
                        dt.Rows[x][5] = r7.ORC[x, 12].Trim();
                        dt.Rows[x][6] = r7.ORC[x, 2].Trim();
                        dt.Rows[x][7] = r7.OBR[x, 4].Trim();
                        dt.Rows[x][8] = r7.OBR[x, 23].Trim();
                        dt.Rows[x][9] = r7.OBR[x, 15].Trim();
                        dt.Rows[x][10] = r7.DG1[x, 3].Trim();
                      // MessageBox.Show(x.ToString()+"&"+r7.DG1[x,1].Trim()+"/"+r7.DG1[x,2].Trim()+"/"+r7.DG1[x,3].Trim()+"/"+r7.DG1[x,4].Trim());
                    }
                    Frm_FJSFYBJY ffj = new Frm_FJSFYBJY(dt,"30^80^50^100^100^100^100^150^40^100^150");
                    ffj.ShowDialog();
                    if (ffj.xh == "")
                    {
                        MessageBox.Show("δѡ���˼���¼");
                        return "0";
                    }
                    xh = int.Parse(ffj.xh);

                }



                px.myDictionary["����"] = r7.PID[5].Split('^')[0].Trim();

                string xb = r7.PID[8].Trim();
                if (xb == "F") xb = "Ů";
                else if (xb.Trim() == "M") xb = "��";
                else xb = "Ů";
                px.myDictionary["�Ա�"] = xb;

                string brlb = r7.QRF[4].Trim();
                if (brlb == "I") brlb = "סԺ";
                else brlb = "����";

                px.myDictionary["�������"] = brlb;

                px.myDictionary["��������"] = r7.PID[7].Trim();
                px.myDictionary["��ַ"] = "�绰��" + r7.PID[13].Trim() + "^";// r7.PID[11].Trim();
                px.myDictionary["�绰"] ="��ַ��"+ r7.PID[11].Trim();
                px.myDictionary["����"] = r7.PID[15].Trim();
                //px.myDictionary["����ID"] = r7.PID[17].Trim();//ҽ������

                px.myDictionary["���˱��"] = r7.PID[3].Trim().Split('^')[0].Trim();
                if (brlb == "����")
                {
                    px.myDictionary["�����"] = Ssbz;//����       // r7.PID[3].Split('^')[1].Trim();
                    px.myDictionary["����ID"] = r7.PID[3].Split('^')[1].Trim(); ;//������ˮ��
                }
                if (brlb == "סԺ")
                {
                    px.myDictionary["סԺ��"] = r7.PID[3].Split('^')[1].Trim();
                    px.myDictionary["����ID"] = r7.PID[17].Trim();//ҽ������
                }
                try
                {
                    px.myDictionary["���֤��"] = r7.PID[3].Trim().Split('^')[8].Trim().Split('~')[1].Trim();
                }
                catch
                {
                    px.myDictionary["���֤��"] = "";
                }
                px.myDictionary["����"] = r7.NTE[3].Trim();

                px.myDictionary["�ͼ����"] = r7.ORC[xh, 13].Trim().Split('^')[0];

                px.myDictionary["����"] = r7.ORC[xh, 13].Trim().Split('^')[0];


                px.myDictionary["�������"] = r7.ORC[xh, 2].Trim();
                px.myDictionary["�ͼ�ҽ��"] = r7.ORC[0, 12].Trim().Split('^')[1].Trim();

                string sqx = r7.OBR[0, 2].Trim();

                px.myDictionary["ҽ����Ŀ"] = r7.OBR[xh, 4].Trim();

                px.myDictionary["�걾����"] = r7.OBR[xh, 15].Trim();

                px.myDictionary["�շ�"] = r7.OBR[xh, 23].Trim();
                try
                {
                    px.myDictionary["�ٴ���ʷ"] = r7.OBR[xh, 46].Trim().Split('~')[1].Replace("2^", "").Trim();
                }
                catch
                {
                    px.myDictionary["�ٴ���ʷ"] = "";
                }
               
                        try
                {
                    px.myDictionary["�ٴ����"] = r7.DG1[xh, 3].Split('^')[1].Trim();
                }
                catch
                {
                    px.myDictionary["�ٴ����"] = r7.DG1[xh, 3].Trim();
                }
                string exep = "";
             //   MessageBox.Show(px.rtn_XML(ref exep));

                return px.rtn_XML(ref exep);
                }
                 catch(Exception e2)
                {
                    MessageBox.Show(e2.ToString());
                    return "0";
                }
            }
            else
            {

              
                if (Sslbx.Trim() == "סԺ��" || Sslbx.Trim() == "����")
                {
                    string rtn_msg2="";
                    if (Sslbx.Trim() == "סԺ��")
                    {
                        string isbrxx = f.ReadString(Sslbx, "isbrxx", "1").Replace("\0", "").Trim();
                        if (isbrxx.Trim() != "1")
                        {
                            MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
                            return "0";
                        }
                        string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><visitNo></visitNo><inpNo>" + Ssbz + "</inpNo><patientId></patientId><wardCode></wardCode><deptCode></deptCode></root>";
                        rtn_msg2 = rtn_CallInterface("XML", "GetPatientInHospital", XML2, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
                    }
                    else
                    {
                        string isbrxx = f.ReadString(Sslbx, "isbrxx", "1").Replace("\0", "").Trim();
                        if (isbrxx.Trim() != "1")
                        {
                            MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
                            return "0";
                        }

                        string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><icCardNo>" + Ssbz + "</icCardNo><executeDept></executeDept></root>";
                        rtn_msg2 = rtn_CallInterface("XML", "GetDispPatientInfo", XML2, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
                    }


                  
                    if (rtn_msg2 == "")
                        return "0";
                    if (rtn_msg2.Contains("error") && !rtn_msg2.Contains("|"))
                    {
                        MessageBox.Show(rtn_msg2);
                        log.WriteMyLog(rtn_msg2);
                        return "0";
                    }
                    else
                    {
                        XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();
                        try
                        {
                            xd.LoadXml(rtn_msg2);
                            xmlok = xd.SelectSingleNode("/root/patients");
                            if (xmlok.InnerXml.Trim() != "")
                            {
                                DataSet ds1 = new DataSet();
                                StringReader sr = new StringReader(xmlok.InnerXml);
                                XmlReader xr = new XmlTextReader(sr);
                                ds1.ReadXml(xr);

                                px.myDictionary["����"] = ds1.Tables[0].Rows[0]["patientName"].ToString().Trim();
                                string xb = ds1.Tables[0].Rows[0]["patientSex"].ToString().Trim();
                                if (xb == "1") xb = "Ů";
                                else if (xb.Trim() == "0") xb = "��";
                                else xb = "";
                                px.myDictionary["�Ա�"] = xb;
                                if (Sslbx.Trim() == "סԺ��")
                                {
                                    px.myDictionary["�������"] = "סԺ";
                                    px.myDictionary["����ID"] = ds1.Tables[0].Rows[0]["visitCount"].ToString().Trim();
                                    px.myDictionary["סԺ��"] = ds1.Tables[0].Rows[0]["visitNo"].ToString().Trim();
                                    px.myDictionary["����"] = ds1.Tables[0].Rows[0]["bedCode"].ToString().Trim();
                                    px.myDictionary["����"] = ds1.Tables[0].Rows[0]["wardName"].ToString().Trim();
                                }
                                else
                                {
                                    px.myDictionary["�������"] = "����";
                                    px.myDictionary["�����"] = ds1.Tables[0].Rows[0]["icCardNo"].ToString().Trim();
                                    px.myDictionary["����"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();
                                }
                                if (ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() != "")
                                {
                                  
                                    try
                                    {
                                        px.myDictionary["����"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() + ds1.Tables[0].Rows[0]["ageUnit"].ToString().Trim();
                                    }
                                    catch
                                    { 
                                        px.myDictionary["����"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim();
                                    }
                                }
                                px.myDictionary["��ַ"] = "" + ds1.Tables[0].Rows[0]["telephone"].ToString().Trim();
                                px.myDictionary["�绰"] = "   " ;
                                px.myDictionary["���˱��"] = ds1.Tables[0].Rows[0]["patientId"].ToString().Trim();
                                px.myDictionary["�ͼ����"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();
                              
                                px.myDictionary["�������"] = "";
                                px.myDictionary["�ͼ�ҽ��"] = ds1.Tables[0].Rows[0]["doctorInCharge"].ToString().Trim().Split('/')[0].Trim();
                                px.myDictionary["�ѱ�"] = ds1.Tables[0].Rows[0]["rateTypeName"].ToString().Trim();
                                try
                                {
                                    px.myDictionary["�ٴ����"] = ds1.Tables[0].Rows[0]["diagnosisname"].ToString().Trim();
                                }
                                catch
                                {
                                    px.myDictionary["�ٴ����"] ="";
                                }

                                string exep = "";
                                 return px.rtn_XML(ref exep);

                            }
                            else
                            {
                                MessageBox.Show("δ�ܲ�ѯ�����˼�¼");
                                return "0";
                            }
                        }
                        catch(Exception  ee)
                        {
                            MessageBox.Show("XML��������:"+ee.ToString());
                            log.WriteMyLog(rtn_msg2+"--"+ee.ToString());
                            return "0";
                        }
                    }


                }
                else
                {
                    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
                }


                return "0";
            }
            }
            catch(Exception  ee)
            {
                MessageBox.Show(ee.ToString());
                return "0";

            }
        }

//        public  static  string  qxjcsq(string Sslbx,string Ssbz )
//        {

//            string OSQ = "";
//            if (Sslbx == "ȡ���������(����)")
//            {
//                OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|"
//                      + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|"
//                      + DateTime.Now.ToString("MMddHHmmss") + "|||^RD|" + Ssbz + "|ORD|^���뵥��||" + "\rQRF|JC06|||O|||||1^^^"
//                      + DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss") + "^" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|";

//            }
//            else
//                if (Sslbx == "ȡ���������(סԺ��)")
//                {
//                    OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|" + DateTime.Now.ToString("yyyyMMddHHmmss")
//                        + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|" + DateTime.Now.ToString("MMddHHmmss")
//                        + "|||^RD|" + Ssbz + "|ORD|^���뵥��||" + "\rQRF|JC06|||I|||||1^^^" + DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss")
//                        + "^" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|";

//                }
//                else
//                {
//                    MessageBox.Show("�޴�ʶ������" + Sslbx);
//                    return "0";
//                }

//            string rtn_msg = rtn_CallInterface("HL7v2", "QueryGmsApply", OSQ, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
//            log.WriteMyLog(rtn_msg);

//            if (rtn_msg.Contains("error") && !rtn_msg.Contains("|"))
//            {
//                MessageBox.Show(rtn_msg);
//                return "0";
//            }
//            else
//            {
//                readhl7 r7 = new readhl7();
//                int x=0;
//                r7.Adt01(rtn_msg,ref x);
//                try
//                {
//                    if (r7.MSA[1].Trim() == "AA")
//                    {
//                        try
//                        {
//                            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
//                            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();

//                            string CancelGmsApply_XML = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\r"
//                                      + "PID|||" + r7.PID[3].Trim() + "||" + r7.PID[5].Trim() +"||||||||||||\r"
//+ "PV1||" + r7.QRF[4].Trim() + "|" + r7.ORC[13].Trim() + "^^" + r7.NTE[3].Trim() + "|||||||||||||||" + Ssbz + "||||||||||||||||||||||||||\r"
//+ "ORC|CA|" + r7.ORC[2].Trim(); +"||||||||||" + CZY + "^" + CZYGH + "|" + r7.ORC[13].Trim() + "\r"
//+ "OBR||" + r7.ORC[2].Trim(); +"||" + r7.OBR[4].Trim() + "|||||||||||&" + r7.OBR[15].Trim(); +"|" + r7.ORC[12].Trim() + "||||||||||||||||||||";

//                            MessageBox.Show(CancelGmsApply_XML);
//                            string rtn_msg = rtn_CallInterface("HL7v2", "CancelGmsApply", CancelGmsApply_XML, "", "FHcsj1KMiIZrotKcdKfy4vzmPhE8GRpr");
//                            MessageBox.Show(rtn_msg);

//                        }
//                        catch (Exception e2)
//                        {
//                            MessageBox.Show(e2.ToString());
//                            return "0";
//                        }
//                    }
//                    else
//                    {
//                        MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
//                        return "0";
//                    }
//                }
//                catch (Exception ee)
//                {
//                    MessageBox.Show(ee.ToString());
//                    return "0";
//                }
//            }

//        }


        
    }
}
