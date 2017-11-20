using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using dbbase;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Net;

namespace LGHISJKZGQ
{
    /// <summary>
    /// �㶫ʡ��ҽԺ    Servlet /Json
    /// </summary>
    class gdszyy
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            string exp = "";
            string JsonSvcWeb = f.ReadString(Sslbx, "JsonSvcWeb", "http://192.9.200.56:9080/ChasSvc/services/JsonSvc4Ris").Replace("\0", "").Trim(); ; //��ȡsz.ini�����õ�webservicesurl

            string mrks = f.ReadString(Sslbx, "mrks", "").Replace("\0", "").Trim(); ; //��ȡsz.ini�����õ�mrks
            debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim(); ; //��ȡsz.ini�����õ�mrks
            string MZHISWeb = f.ReadString(Sslbx, "WSURL", "").Replace("\0", "").Trim(); ; 

            if (Sslbx != "")
            {
                string Content = "";

                if (Sslbx == "�걾��")
                {
                      dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string BBLB_XML = "";
            DataTable dt_bb = new DataTable();
        
                dt_bb = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_BBTMH= '" + Ssbz + "' ", "bbxx");
            
                if (dt_bb == null)
                {
                    MessageBox.Show("���ݿ������쳣");
                    return "0";
                }
              
                    if (dt_bb.Rows.Count <= 0)
                    {
                        MessageBox.Show("δ��ѯ���˱걾��Ϣ");
                        return "0";
                    }

                    string sqdh = dt_bb.Rows[0]["F_SQXH"].ToString();
                    string brlb = dt_bb.Rows[0]["F_BRLB"].ToString();

                    if (brlb == "סԺ")
                    {
                        Sslbx = "סԺ�����";
                        Ssbz = sqdh;
                    }
                    else
                    {
                        Sslbx = "���������";
                        Ssbz = sqdh;
                    }

                }

                if (Sslbx == "סԺ�����" || Sslbx == "סԺ��(��Ժ)" || Sslbx == "סԺ��(��ɳ)" || Sslbx == "סԺ��(����)" || Sslbx == "סԺ��(��ѧ��)" || Sslbx == "סԺ��")
                {
                    string yydm = f.ReadString(Sslbx, "yydm", "1").Replace("\0", "").Trim(); ; //��ȡsz.ini�����õ�webservicesurl
                    #region  ��ȡסԺ���뵥��Ϣ

                    IPAddress addr = new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
                    string Ipaddr = addr.ToString();

                    DataTable dt_sqd = new DataTable();

                    string zyh = ""; string sqdh = "";
       
                    if (Sslbx == "סԺ�����")
                        sqdh = Ssbz.Trim();
                    else
                        zyh = Ssbz.Trim();
                  
                    try
                    {
                      
                        MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("gb2312"));
                        JsonWriter writer = new JsonTextWriter(sw);
                        Encoding eee = sw.Encoding;
                        writer.WriteStartObject();
                        writer.WritePropertyName("reqHeader");
                        writer.WriteStartObject();
                        writer.WritePropertyName("callFunction");
                        writer.WriteValue("searchPatientExaminationApply");
                        writer.WritePropertyName("systemId");
                        writer.WriteValue("PACS.BL");
                        writer.WritePropertyName("reqTimestamp");
                        writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WritePropertyName("terminalIp");
                        writer.WriteValue(Ipaddr);
                        writer.WriteEndObject();
                        writer.WritePropertyName("requestForm");
                        writer.WriteStartObject();
                        writer.WritePropertyName("requestFormHisNo");
                        writer.WriteValue(sqdh);
                        writer.WriteEndObject();
                        writer.WritePropertyName("patient");
                        writer.WriteStartObject();
                        writer.WritePropertyName("hospital");
                        writer.WriteStartObject();
                        writer.WritePropertyName("hospitalCode");
                        writer.WriteValue(yydm);
                        writer.WriteEndObject();
                        writer.WritePropertyName("patientNo");
                        writer.WriteValue(zyh);
                        writer.WriteEndObject();

                        writer.WriteEndObject();
                        writer.Flush();


                        string rtnrequest = GetHisSQD(JsonSvcWeb, ms.ToArray(),debug);

                        if (rtnrequest == "0")
                        {
                       log.WriteMyLog("��ȡסԺ���뵥��Ϣʧ�ܣ�HIS������ϢΪ��");
                            return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                           
                        }
                        try
                        {
                            JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);
                            string requestForms = ja["requestForms"].ToString().Trim();

                            if (requestForms.Trim() == "")
                            {
                                log.WriteMyLog("��ȡסԺ���뵥��Ϣʧ�ܣ�δ��ѯ���������!");
                                return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                               
                            }
                            //����
                            DataColumn name = new DataColumn("name");
                            dt_sqd.Columns.Add(name);
                            //�Ա�
                            DataColumn sexName = new DataColumn("sexName");
                            dt_sqd.Columns.Add(sexName);
                            //����
                            DataColumn ageYear = new DataColumn("ageYear");
                            dt_sqd.Columns.Add(ageYear);
                            //birthday
                            DataColumn birthday = new DataColumn("birthday");
                            dt_sqd.Columns.Add(birthday);
                            //idCardNo
                            DataColumn idCardNo = new DataColumn("idCardNo");
                            dt_sqd.Columns.Add(idCardNo);
                            //phone
                            DataColumn phone = new DataColumn("phone");
                            dt_sqd.Columns.Add(phone);
                            //address
                            DataColumn address = new DataColumn("address");
                            dt_sqd.Columns.Add(address);
                            //סԺ������
                            DataColumn patientNo = new DataColumn("patientNo");
                            dt_sqd.Columns.Add(patientNo);

                            //empiid
                            DataColumn empiid = new DataColumn("empiid");
                            dt_sqd.Columns.Add(empiid);
                            //���˾����
                            DataColumn encounterNo = new DataColumn("encounterNo");
                            dt_sqd.Columns.Add(encounterNo);
                            //����ҽԺ
                            DataColumn hospitalName = new DataColumn("hospitalName");
                            dt_sqd.Columns.Add(hospitalName);
                            //�������
                            DataColumn departmentName = new DataColumn("departmentName");
                            dt_sqd.Columns.Add(departmentName);
                            //����
                            DataColumn wardName = new DataColumn("wardName");
                            dt_sqd.Columns.Add(wardName);
                            //����
                            DataColumn bedName = new DataColumn("bedName");
                            dt_sqd.Columns.Add(bedName);
                            //�ͼ�ҽ��
                            DataColumn employeeName = new DataColumn("employeeName");
                            dt_sqd.Columns.Add(employeeName);
                            //��������
                            DataColumn encounterType = new DataColumn("encounterType");
                            dt_sqd.Columns.Add(encounterType);
                            //�������
                            DataColumn requestFormHisNo = new DataColumn("requestFormHisNo");
                            dt_sqd.Columns.Add(requestFormHisNo);
                            //���뵥״̬
                            DataColumn statusName = new DataColumn("statusName");
                            dt_sqd.Columns.Add(statusName);
                            //ҽ����Ŀ
                            DataColumn requestType = new DataColumn("requestType");
                            dt_sqd.Columns.Add(requestType);
                            //��ע
                            DataColumn memo = new DataColumn("memo");
                            dt_sqd.Columns.Add(memo);
                            //��鲿λ
                            DataColumn orderName = new DataColumn("orderName");
                            dt_sqd.Columns.Add(orderName);

                            //�ٴ���ʷ
                            DataColumn clinicalHistory = new DataColumn("clinicalHistory");
                            dt_sqd.Columns.Add(clinicalHistory);

                            //�ٴ����
                            DataColumn diagnosis = new DataColumn("diagnosis");
                            dt_sqd.Columns.Add(diagnosis);

                            //�����뵥
                            DataColumn orderItemHisNo = new DataColumn("orderItemHisNo");
                            dt_sqd.Columns.Add(orderItemHisNo);


                            JArray jsonVals = JArray.Parse(requestForms);
                            for (int x = 0; x < jsonVals.Count; x++)
                            {
                                DataRow dr = dt_sqd.NewRow();
                                dt_sqd.Rows.Add(dr);
                                //����
                                dr["name"] = jsonVals[x]["patient"]["name"].ToString();
                                try
                                {
                                    //�Ա�
                                    dr["sexName"] = jsonVals[x]["patient"]["sexName"].ToString();
                                }
                                catch
                                {
                                    dr["sexName"] = "";
                                }
                                try
                                {
                                    //����
                                    dr["ageYear"] = jsonVals[x]["patient"]["ageYear"].ToString();
                                }
                                catch
                                {
                                    dr["ageYear"] = "";
                                }
                                try
                                {
                                    //birthday
                                    dr["birthday"] = jsonVals[x]["patient"]["birthday"].ToString();
                                }
                                catch
                                {
                                    dr["birthday"] = "";
                                }
                                    //idCardNo
                                try
                                {
                                    dr["idCardNo"] = jsonVals[x]["patient"]["idCardNo"].ToString();
                                }
                                catch
                                {
                                    dr["idCardNo"] = "";
                                }
                                try
                                {
                                    //phone
                                    dr["phone"] = jsonVals[x]["patient"]["phone"].ToString();
                                }
                                catch
                                {
                                    dr["phone"] = "";
                                }
                                try
                                {
                                    //address
                                    dr["address"] = jsonVals[x]["patient"]["address"].ToString();
                                }
                                catch
                                {
                                    dr["address"] = "";

                                }
                                try
                                {
                                    //סԺ������
                                    dr["patientNo"] = jsonVals[x]["patient"]["patientNo"].ToString();
                                }
                                catch
                                {
                                    dr["patientNo"] = "";

                                }

                                try
                                {
                                    //empiid
                                    dr["empiid"] = jsonVals[x]["patient"]["empid"].ToString();
                                }
                                catch
                                {
                                    dr["empiid"] = "";

                                }

                                try
                                {
                                    //���˾����
                                    dr["encounterNo"] = jsonVals[x]["patient"]["encounterNo"].ToString();
                                }
                                catch
                                {
                                    dr["encounterNo"] = "";
                                }
                                try
                                {
                                    //����ҽԺ
                                    dr["hospitalName"] = jsonVals[x]["patient"]["hospital"]["hospitalName"].ToString();
                                }
                                catch
                                {
                                    dr["hospitalName"] = "";
                                }
                                try
                                {
                                    //�������
                                    dr["departmentName"] = jsonVals[x]["patient"]["department"]["departmentName"].ToString();
                                }
                                catch
                                {
                                    dr["departmentName"] = "";
                                }
                                try
                                {
                                    //����
                                    dr["wardName"] = jsonVals[x]["patient"]["location"]["wardName"].ToString();
                                }
                                catch
                                {
                                    dr["wardName"] = "";
                                }
                                try
                                {
                                    //����
                                    dr["bedName"] = jsonVals[x]["patient"]["location"]["bedName"].ToString();
                                }
                                catch
                                {
                                    dr["bedName"] = "";
                                }
                                try
                                {
                                    //�ͼ�ҽ��
                                    dr["employeeName"] = jsonVals[x]["patient"]["chargeDoctor"]["employeeName"].ToString();
                                }
                                catch
                                {
                                    dr["employeeName"] = "";
                                }
                                try
                                {
                                    //��������
                                    dr["encounterType"] = jsonVals[x]["patient"]["encounterType"].ToString();
                                }
                                catch
                                {
                                    dr["encounterType"] = "";
                                }
                                    //�������
                                dr["requestFormHisNo"] = jsonVals[x]["requestFormHisNo"].ToString();
                                try
                                {
                                    //���뵥ҽ��
                                    dr["requestType"] = jsonVals[x]["requestType"].ToString();
                                }
                                catch
                                {
                                    dr["requestType"] = "";
                                }


                                //�ٴ���ʷ
                                dr["clinicalHistory"] = "";


                                //�ٴ����
                                dr["diagnosis"] = "";
                                dr["orderItemHisNo"] = "";
                                dr["orderName"] = "";
                                //�����Ŀ(OrderItem)�б�

                              
                                JArray jarray22 = JArray.Parse(jsonVals[x]["orderItem"].ToString());
                                foreach (JObject jj in jarray22)
                                {
                                    try
                                    {
                                        if (dr["orderItemHisNo"] == "")
                                            dr["orderItemHisNo"] = jj["orderItemHisNo"].ToString();
                                        else
                                            dr["orderItemHisNo"] = dr["orderItemHisNo"].ToString() + "|" + jj["orderItemHisNo"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                     //��鲿λ

                                    try
                                    {
                                        JArray jarray33 = JArray.Parse(jj["subItems"].ToString());

                                        foreach (JObject jj33 in jarray33)
                                        {

                                            if (dr["orderName"] == "")
                                                dr["orderName"] = jj33["orderName"].ToString();
                                            else
                                                dr["orderName"] = dr["orderName"].ToString() + "|" + jj33["orderName"].ToString();

                                        }
                                    }
                                    catch
                                    {
                                    }
                
                                }
 
                            }

                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog("��ȡסԺ���뵥��Ϣʧ�ܣ����������쳣��" + ee4.Message);
                            return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                            return "0";
                        }


                    }
                    catch (Exception ex)
                    {

                        log.WriteMyLog("��ȡסԺ���뵥��Ϣʧ���쳣��" + ex.Message);
                        return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                        return "0";
                    }
                    int count = 0;

                    if (dt_sqd.Rows.Count <= 0)
                    {
                        log.WriteMyLog("��ȡסԺ���뵥��Ϣʧ�ܣ�δ��ѯ�����סԺ��Ϣ");
                        return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                        return "0";
                    }
                    if (dt_sqd.Rows.Count > 1)
                    {

                        string Columns = f.ReadString(Sslbx, "Columns", "patientNo,requestFormHisNo,statusName,name,sexName,ageYear,hospitalName,departmentName,bedName,requestType,orderName,memo");//��ʾ����Ŀ��Ӧ�ֶ�
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "������,�������,״̬,����,�Ա�,����,ҽԺ,����,����,ҽ����Ŀ,��鲿λ,��ע");//��ʾ����Ŀ����
                        string xsys = f.ReadString(Sslbx, "xsys", "1"); //ѡ����������Ŀ
                        DataColumn dc0 = new DataColumn("���");
                        dt_sqd.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqd.Rows.Count; x++)
                        {
                            dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                        }

                        if (Columns.Trim() != "")
                            Columns = "���," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "���," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                        yc.ShowDialog();

                        if(yc.DialogResult != DialogResult.Yes)
                        {
                             MessageBox.Show("δѡ��������Ŀ��");
                            return "0";
                        }
                        string rtn2 = yc.F_XH;
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("δѡ��������Ŀ��");
                            return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("������ѡ��������Ŀ��");
                            return "0";
                        }


                    }

                    //��ѯ�����Ϣ
                    string F_LCBS = "";
                    string F_LCZD = "";
                    string F_FY = "";
                    string F_HY = ""; string bbmc = ""; string F_BBMC = ""; string F_SJYS = "";
                    if (dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim() != "")
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream();
                            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("gb2312"));
                            JsonWriter writer = new JsonTextWriter(sw);
                            Encoding eee = sw.Encoding;
                            writer.WriteStartObject();
                            writer.WritePropertyName("reqHeader");
                            writer.WriteStartObject();
                            writer.WritePropertyName("callFunction");
                            writer.WriteValue("queryApplyContent");
                            writer.WritePropertyName("systemId");
                            writer.WriteValue("PACS.BL");
                            writer.WritePropertyName("reqTimestamp");
                            writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            writer.WritePropertyName("terminalIp");
                            writer.WriteValue(Ipaddr);
                            writer.WriteEndObject();
                            writer.WritePropertyName("requestForm");
                            writer.WriteStartObject();
                            writer.WritePropertyName("requestFormHisNo");
                            writer.WriteValue(dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim());
                            writer.WriteEndObject();
                            writer.WriteEndObject();
                            writer.Flush();


                            string rtnrequest = GetHisSQD(JsonSvcWeb, ms.ToArray(), debug);

                            if (rtnrequest == "0")
                                return "0";

                            JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);

                            try
                            {
                                //�ٴ����
                                dt_sqd.Rows[count]["diagnosis"] = ja["applyContent"]["examinationContent"]["clinicalDiagnosis"].ToString().Trim();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["diagnosis"] = "";
                            }
                            try
                            {
                                //bbmc
                                F_BBMC = ja["applyContent"]["examinationContent"]["sendExaminationObject"].ToString().Trim();
                            }
                            catch
                            {
                            }
                            try
                            {
                                //���� //��ʷ //���� //����ʷ
                                dt_sqd.Rows[count]["clinicalHistory"] = "���ߣ�" + ja["applyContent"]["examinationContent"]["chiefComplaint"].ToString().Trim() + "    �ֲ�ʷ��" + ja["applyContent"]["examinationContent"]["presentHistory"].ToString().Trim() + "    ������" + ja["applyContent"]["examinationContent"]["professionsCondition"].ToString().Trim() + "    ����ʷ��" + ja["applyContent"]["examinationContent"]["familyHistory"].ToString().Trim() + "    ��Ժ��ϣ�" + ja["applyContent"]["examinationContent"]["diagnosis"].ToString().Trim();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["clinicalHistory"] = "";
                            }
                            //  F_FY = ja["applyContent"]["patientBaseInfo"]["charges"].ToString();
                            //����
                            try
                            {
                                F_HY = ja["applyContent"]["patientBaseInfo"]["marriage"].ToString();
                            }
                            catch
                            {
                            }
                            //����ҽ��
                            try
                            {
                                dt_sqd.Rows[count]["employeeName"] = ja["applyContent"]["patientBaseInfo"]["reqDoctor"].ToString();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["employeeName"] = "";
                            }
                            try
                            {
                                dt_sqd.Rows[count]["requestType"] = ja["applyContent"]["examinationContent"]["examinationContent"].ToString();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["requestType"] = "";
                            }
                        }
                        catch (Exception ee2)
                        {
                            log.WriteMyLog("��ȡסԺ���뵥�����Ϣ�쳣��"+ee2.Message);
                            //return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);

                        }
                    }
               

                    //��ȡ�걾��Ϣ
                          string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string bbxml = "";
                    string bbmc2="";
                    if (tqbblb == "1")
                    {
                        bbxml = GetBbxx(dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim(), ref bbmc2);
                    }

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[count]["empiid"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                       
                            xml = xml + "����ID=" + (char)34 + dt_sqd.Rows[count]["encounterNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt_sqd.Rows[count]["patientNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["name"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        string xb = dt_sqd.Rows[count]["sexName"].ToString().Trim();
                        xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";

                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["ageYear"].ToString().Trim() + "��" + (char)34 + " ";

                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + F_HY + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "��ַ=" + (char)34 + dt_sqd.Rows[count]["phone"].ToString().Trim()+"^" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_sqd.Rows[count]["address"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["wardName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["bedName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt_sqd.Rows[count]["idCardNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_sqd.Rows[count]["departmentName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqd.Rows[count]["employeeName"] + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "�շ�=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "�걾����=" + (char)34 + F_BBMC + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + dt_sqd.Rows[count]["hospitalName"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqd.Rows[count]["requestType"].ToString().Trim()+"^" + dt_sqd.Rows[count]["orderItemHisNo"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_sqd.Rows[count]["clinicalHistory"] + "]]></�ٴ���ʷ>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_sqd.Rows[count]["diagnosis"] + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }


                        if (tqbblb == "1")
                            xml = xml + bbxml;

                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());


                        return xml;
                    }
                    catch (Exception e)
                    {

                  
                        log.WriteMyLog("��ȡסԺ���뵥��Ϣʧ�ܣ�xml��������---" + e.Message.ToString());
                        return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                        return "0";
                    }
                    #endregion
                }
                if (Sslbx == "���������" || Sslbx == "���ƿ���" || Sslbx == "�Һ����")
                {
                    #region  ���������

                    string request_xml = "";
                    if (Sslbx == "���ƿ���")
                        request_xml = "<Request><CardNo>" + Ssbz.Trim() + "</CardNo><StartDate>" + DateTime.Now.AddDays(-60).ToString("yyyyMMdd") + "</StartDate>"
                 + "<EndDate>" + DateTime.Now.ToString("yyyyMMdd") + "</EndDate><EmpiId></EmpiId><RegNo></RegNo><CheckNumber></CheckNumber><Flag>0</Flag></Request>";

                    if (Sslbx == "�Һ����")
                        request_xml = "<Request><CardNo></CardNo><StartDate>" + DateTime.Now.AddDays(-60).ToString("yyyyMMdd") + "</StartDate>"
                 + "<EndDate>" + DateTime.Now.ToString("yyyyMMdd") + "</EndDate><EmpiId></EmpiId><RegNo>" + Ssbz.Trim() + "</RegNo><CheckNumber></CheckNumber><Flag>1</Flag></Request>";

                    if (Sslbx == "���������")
                        request_xml = "<Request><CardNo></CardNo><StartDate>" + DateTime.Now.AddDays(-60).ToString("yyyyMMdd") + "</StartDate>"
                + "<EndDate>" + DateTime.Now.ToString("yyyyMMdd") + "</EndDate><EmpiId></EmpiId><RegNo></RegNo><CheckNumber>" + Ssbz.Trim() + "</CheckNumber><Flag>2</Flag></Request>";


                    if (request_xml.Trim() == "")
                    {
                        MessageBox.Show("��ѯ�����쳣");
                        return "0";
                    }

                    if (debug == "1")
                        log.WriteMyLog("��ѯ������" + request_xml);

                    string rtn_xml ="";
                     try
                    {
                        gdszyyMzHisWeb.HisForInspectItemService mzhis = new LGHISJKZGQ.gdszyyMzHisWeb.HisForInspectItemService();
                        if (MZHISWeb.Trim()!="")
                        mzhis.Url = MZHISWeb;
                        rtn_xml = mzhis.GetCheckRecord(request_xml);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("��������WebService�����쳣��" + ee.Message+"\r\n");
                        return "0";
                    }
                    if (rtn_xml.Trim() == "")
                    {
                        MessageBox.Show("��ȡ�������뵥��Ϣʧ�ܣ�������ϢΪ�գ�\r\n�Ƿ���ȡ���˻�����Ϣ");
                        
                      return   GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
                      
                    }

                    if (debug == "1")
                        log.WriteMyLog(rtn_xml);
   
                      XmlNode xmlok_DATA = null;
            XmlDocument xd2 = new XmlDocument();
            try
            {
                xd2.LoadXml(rtn_xml);
                xmlok_DATA = xd2.SelectSingleNode("/Response");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("��ȡ�������뵥��Ϣʧ��1" + "\r\n�Ƿ���ȡ���˻�����Ϣ");
                log.WriteMyLog("��ȡ�������뵥��Ϣʧ�ܣ�����DATA�쳣��" + xmlok_e.Message);
                return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
              
            }

            string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
            string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

            if (ResultCode != "0")
            {
                MessageBox.Show("" + ErrorMsg+"\r\n�Ƿ���ȡ���˻�����Ϣ");
                log.WriteMyLog("��ȡ�������뵥��Ϣʧ�ܣ�" + ErrorMsg);
                return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
            }

            if (xmlok_DATA["Info"].InnerText == "")
            {
                MessageBox.Show("��ȡ�������뵥��Ϣʧ�ܣ�Info�ڵ�Ϊ��" + "\r\n�Ƿ���ȡ���˻�����Ϣ");
                return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
            }

          XmlNodeList  node_list = xd2.SelectNodes("/Response/Info/Record");
          if (node_list.Count <= 0)
          {
              MessageBox.Show("δ��ѯ���������뵥��Ϣ\r\n�Ƿ���ȡ���˻�����Ϣ");
              return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
          }


          DataTable dt_sqd = new DataTable();
          //����
          DataColumn dc_PatientName = new DataColumn("PatientName");
          dt_sqd.Columns.Add(dc_PatientName);
          //�Ա�
          DataColumn dc_Sex = new DataColumn("Sex");
          dt_sqd.Columns.Add(dc_Sex);
          //��������
          DataColumn dc_Birthday = new DataColumn("Birthday");
          dt_sqd.Columns.Add(dc_Birthday);
          //������
          DataColumn dc_MedicalNo = new DataColumn("MedicalNo");
          dt_sqd.Columns.Add(dc_MedicalNo);
          //���֤��
          DataColumn dc_IdCard = new DataColumn("IdCard");
          dt_sqd.Columns.Add(dc_IdCard);
          //EmpiId
          DataColumn dc_EmpiId = new DataColumn("EmpiId");
          dt_sqd.Columns.Add(dc_EmpiId);


          //�������ID
          DataColumn dc_CheckNumber = new DataColumn("CheckNumber");
          dt_sqd.Columns.Add(dc_CheckNumber);
          //�����������
          DataColumn dc_AppDeptName = new DataColumn("AppDeptName");
          dt_sqd.Columns.Add(dc_AppDeptName);
          //ִ�п�������
          DataColumn dc_ExecDeptName = new DataColumn("ExecDeptName");
          dt_sqd.Columns.Add(dc_ExecDeptName);
          //����ҽ������
          DataColumn dc_DoctorName = new DataColumn("DoctorName");
          dt_sqd.Columns.Add(dc_DoctorName);
          //�����Ŀ����
          DataColumn dc_CheckItemName = new DataColumn("CheckItemName");
          dt_sqd.Columns.Add(dc_CheckItemName);
          //�����Ŀ����
          DataColumn dc_CheckType = new DataColumn("CheckType");
          dt_sqd.Columns.Add(dc_CheckType);
          //�ܽ��
          DataColumn dc_Money = new DataColumn("Money");
          dt_sqd.Columns.Add(dc_Money);
          //�������
          DataColumn dc_Diagnosis = new DataColumn("Diagnosis");
          dt_sqd.Columns.Add(dc_Diagnosis);
          //������Դ
          DataColumn dc_Source = new DataColumn("Source");
          dt_sqd.Columns.Add(dc_Source);
          //����
          DataColumn dc_Age = new DataColumn("Age");
          dt_sqd.Columns.Add(dc_Age);
          //
          DataColumn dc_RegNo = new DataColumn("RegNo");
          dt_sqd.Columns.Add(dc_RegNo);
          //Ժ��
          DataColumn dc_HospitalShortName = new DataColumn("HospitalShortName");
          dt_sqd.Columns.Add(dc_HospitalShortName);
          //��ʷ
          DataColumn dc_ClinicHistory = new DataColumn("ClinicHistory");
          dt_sqd.Columns.Add(dc_ClinicHistory);

          string PatientName = xmlok_DATA["Info"]["PatientName"].InnerText.Trim();
          string Sex = xmlok_DATA["Info"]["Sex"].InnerText.Trim();
          string Birthday = xmlok_DATA["Info"]["Birthday"].InnerText.Trim();
          string MedicalNo = xmlok_DATA["Info"]["MedicalNo"].InnerText.Trim();
          string IdCard = "";

                  
          string RegNo = xmlok_DATA["Info"]["RegNo"].InnerText.Trim();
    
          string empiid = xmlok_DATA["Info"]["EmpiId"].InnerText.Trim();
    

          try
          {
           IdCard=   xmlok_DATA["Info"]["IdCard"].InnerText.Trim();
          }
          catch
          {
          }
        
          foreach (XmlNode node in node_list)
          {
              DataRow dr = dt_sqd.NewRow();
              dt_sqd.Rows.Add(dr);
              dr["PatientName"] = PatientName;
              dr["Sex"] = Sex;
              dr["Birthday"] = Birthday;
              dr["MedicalNo"] = MedicalNo;
              dr["IdCard"] = IdCard;
              dr["CheckNumber"] = node["CheckNumber"].InnerText.Trim();
              dr["AppDeptName"] = node["AppDeptName"].InnerText.Trim();
              dr["ExecDeptName"] = node["ExecDeptName"].InnerText.Trim();
              dr["DoctorName"] = node["DoctorName"].InnerText.Trim();
              dr["CheckItemName"] = node["CheckItemName"].InnerText.Trim();
              dr["CheckType"] = node["CheckType"].InnerText.Trim();
              dr["Money"] = node["Money"].InnerText.Trim();
              dr["Diagnosis"] = node["Diagnosis"].InnerText.Trim();
              dr["RegNo"] = RegNo.Trim();
              dr["EmpiId"] = empiid.Trim();

              dr["HospitalShortName"] = node["HospitalShortName"].InnerText.Trim();
              dr["ClinicHistory"] = node["ClinicHistory"].InnerText.Trim();

               string Source=node["Source"].InnerText.Trim();

               if (Source == "1") dr["Source"] = "����";
               else if (Source == "2") dr["Source"] = "סԺ";
               else if (Source == "3") dr["Source"] = "���";
             
                dr["Age"]= ZGQClass.CsrqToAge(dr["Birthday"].ToString());
               
          }
         

                    if (dt_sqd.Rows.Count < 1)
                    {
                        MessageBox.Show("δ�鵽��Ӧ���������뵥����");
                        return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
                    }

                    int tkts = f.ReadInteger(Sslbx, "tkts", 2);

                    int  count=0;
                    string sqxh = "";
                    string yzxm = "";
                    if (dt_sqd.Rows.Count >= tkts)
                    {
                        DataColumn dc0 = new DataColumn("���");
                        dt_sqd.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqd.Rows.Count; x++)
                        {
                            dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                        }


                        GDSZYY_YZ_SELECT yc = new GDSZYY_YZ_SELECT(dt_sqd);
                        yc.ShowDialog();

                        if (yc.DialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("δѡ��������Ŀ��"); return "0";
                        }

                        string rtn2 = yc.F_XH;
                        sqxh = yc.F_sqxh;
                        yzxm = yc.F_yzxm;
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("δѡ��������Ŀ��"); return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("������ѡ��������Ŀ��"); return "0";
                        }
                    }
                    else
                    {
                        sqxh = dt_sqd.Rows[count]["CheckNumber"].ToString().Trim();
                        yzxm = dt_sqd.Rows[count]["CheckItemName"].ToString().Trim();
                    }


                    //��ȡ�걾��Ϣ
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string bbxml = "";
                    string bbmc2 = "";
                    if (tqbblb == "1")
                    {
                        bbxml = GetBbxx(sqxh, ref bbmc2);
                    }

                     //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[count]["EmpiId"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + dt_sqd.Rows[count]["RegNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + sqxh + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�����=" + (char)34 + dt_sqd.Rows[count]["MedicalNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["PatientName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                        string xb = dt_sqd.Rows[count]["Sex"].ToString().Trim();
                        xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["Age"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt_sqd.Rows[count]["IdCard"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_sqd.Rows[count]["AppDeptName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqd.Rows[count]["DoctorName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "�걾����=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + dt_sqd.Rows[count]["HospitalShortName"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + yzxm + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                        xml = xml + "�������=" + (char)34 + dt_sqd.Rows[count]["Source"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_sqd.Rows[count]["ClinicHistory"].ToString() + "]]></�ٴ���ʷ>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_sqd.Rows[count]["Diagnosis"].ToString() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }

                        if (tqbblb == "1")
                            xml = xml + bbxml;
                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("��ȡ�������뵥��Ϣ�쳣��"+e.Message);
                        log.WriteMyLog("��ȡ�������뵥��Ϣ�쳣��" + e.Message);
                        return "0";
                    }

                    #endregion
                }
                //if (Sslbx == "����")
                //{
                //    #region  ����


                //    string rtn_xml = "";
                //    try
                //    {
                     
                //        if (MZHISWeb.Trim() != "")
                //            mzhis.Url = MZHISWeb;
                //        rtn_xml = mzhis.getLoadPaintInfoResult(Sslbx);
                //    }
                //    catch (Exception ee)
                //    {
                //        MessageBox.Show("����WebService�쳣��" + ee.Message + "\r\n");
                //        return "0";
                //    }
                //    if (rtn_xml.Trim() == "")
                //    {
                //        MessageBox.Show("��ȡ�����Ϣʧ�ܣ�������ϢΪ��");

                //        return "0";

                //    }

                //    if (debug == "1")
                //        log.WriteMyLog(rtn_xml);

                //    XmlNode xn = null;
                //    XmlDocument xd2 = new XmlDocument();
                //    try
                //    {
                //        xd2.LoadXml(rtn_xml);
                //        xn = xd2.SelectSingleNode("/getLoadPaintInfoResult");
                //    }
                //    catch (Exception xmlok_e)
                //    {
                //        MessageBox.Show("��ȡ�����Ϣʧ��:" + xmlok_e.Message);
                //        return "0";
                //    }

                  

                //    //-����xml----------------------------------------------------
                //    try
                //    {

                //        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                //        xml = xml + "<LOGENE>";
                //        xml = xml + "<row ";
                //        try
                //        {
                //            xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[count]["EmpiId"].ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "����ID=" + (char)34 + dt_sqd.Rows[count]["RegNo"].ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "�������=" + (char)34 + sqxh + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {

                //            xml = xml + "�����=" + (char)34 + dt_sqd.Rows[count]["MedicalNo"].ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        try
                //        {
                //            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        try
                //        {
                //            xml = xml + "����=" + (char)34 + xn["DOC_NameO"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                    
                //        try
                //        {
                //            string xb = xn["sex"].InnerText.ToString().Trim();
                //            if (xb == "M") xb = "��";
                //            if (xb == "F") xb = "Ů";
                //            xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                //        }
                //        catch
                //        {
                //            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        xml = xml + "����=" + (char)34 + xn["age"].InnerText.ToString().Trim() + (char)34 + " ";

                //        //----------------------------------------------------------

                //        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                //        //----------------------------------------------------------
                //        try
                //        {

                //            xml = xml + "��ַ=" + (char)34 + xn["Address_1 "].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "�绰=" + (char)34 + xn["Phone_1"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "����=" + (char)34 + xn["BED_NUM"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                //        //----------------------------------------------------------
                //        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "�ͼ����=" + (char)34 + xn["LOCATION"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                //        }
                //        //----------------------------------------------------------

                //        try
                //        {
                //            xml = xml + "�ͼ�ҽ��=" + (char)34 + xn["Ord_By"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------
                //        xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------

                //        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";

                //        //----------------------------------------------------------
                //        xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------
                //        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                //        //----------------------------------------------------------
                //        xml = xml + "����2=" + (char)34 + xn["organ_Name"].InnerText.ToString().Trim() + (char)34 + " ";
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "�������=" + (char)34 +"���" + (char)34 + " ";
                //        }
                //        catch
                //        {
                //            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        xml = xml + "/>";
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "<�ٴ���ʷ><![CDATA[" + xn["spec_intm"].InnerText.ToString().Trim() + "]]></�ٴ���ʷ>";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "<�ٴ����><![CDATA[" + xn["comments"].InnerText.ToString().Trim() + "]]></�ٴ����>";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                //        }

                    
                //        xml = xml + "</LOGENE>";

                //        if (debug == "1" && exp.Trim() != "")
                //            log.WriteMyLog(exp.Trim());
                //        return xml;
                //    }
                //    catch (Exception e)
                //    {
                //        MessageBox.Show("��ȡ�����Ϣ�쳣��" + e.Message);
                   
                //        return "0";
                //    }

                //    #endregion
                //}

                if (Sslbx == "����ȡ��ִ��")
                {
                    #region  ����ȡ��ִ��



                    string DeptId = f.ReadString(Sslbx, "deptid", "60027").Replace("\0", "").Trim(); ;
                    string yhgh = f.ReadString(Sslbx, "yhbh", "").Replace("\0", "").Trim(); 

                    string request_xml = "<Request><CheckNumber>" + Ssbz.Trim() + "</CheckNumber><CancelDeptId>" + DeptId + "</CancelDeptId><CancelUserNo>" + yhgh + "</CancelUserNo></Request>";

                    if (debug == "1")
                        log.WriteMyLog("����ȡ��ִ��ʧ�ܣ���ѯ������" + request_xml);
                    string rtn_xml = "";
                    try
                    {
                        gdszyyMzHisWeb.HisForInspectItemService mzhis = new LGHISJKZGQ.gdszyyMzHisWeb.HisForInspectItemService();
                        if (MZHISWeb!="")
                        mzhis.Url = MZHISWeb;
                        rtn_xml = mzhis.CancelExecCheckItem(request_xml);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("����ȡ��ִ��ʧ�ܣ�����WebService�����쳣��" + ee.Message + "\r\n");
                        return "0";
                    }
                    if (rtn_xml.Trim() == "")
                    {
                        MessageBox.Show("����ȡ��ִ��ʧ�ܣ�������ϢΪ�գ�");
                        return "0";
                    }


                    if (debug == "1")
                        log.WriteMyLog(rtn_xml);

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(rtn_xml);
                        xmlok_DATA = xd2.SelectSingleNode("/Response");
                    }
                    catch (Exception xmlok_e)
                    {

                        MessageBox.Show("����DATA�쳣��" + xmlok_e.Message);
                        return "0";
                    }

                    string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
                    string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

                    if (ResultCode != "0")
                    {
                        MessageBox.Show("����ȡ��ִ��ʧ�ܣ�" + ErrorMsg);
                        return "0";
                    }
                    else
                    {
                        MessageBox.Show("����ȡ���ɹ���" + ErrorMsg);
                        return "0";
                    }

                    #endregion
                }
               if (Sslbx == "סԺȡ���Ǽ�")
                {
                    #region   ��дסԺHIS״̬
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable jcxx = new DataTable();
                        try
                        {
                            jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + Ssbz.Trim() + "'", "jcxx");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                            return "0";
                        }
                        if (jcxx == null)
                        {
                            MessageBox.Show("�������ݿ����������⣡");
                            return "0";
                        }
                        if (jcxx.Rows.Count < 1)
                        {
                             MessageBox.Show("�޴˲���ż�¼��");
                                     return "0";
                        }


                        if (jcxx.Rows[0]["F_brlb"].ToString() != "סԺ")
                        {
                             MessageBox.Show("��סԺ���˲���ȡ���Ǽǣ�");
                             return "0";
                        }
                        string yzxmid = "";
                        try
                        {
                            yzxmid = jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1].Trim();
                        }
                        catch
                        {
                        }

                        if (yzxmid == "" || jcxx.Rows[0]["F_SQXH"].ToString() == "")
                        {
                          MessageBox.Show("���뵥�Ż������뵥��Ϊ�գ�����ȡ���Ǽǣ�");
                         return "0";
                        }


                            string sqdzt = ""; string sqdztCode = "";
                             sqdztCode = "CLDS.RIUNR.0000"; sqdzt = "ȡ���Ǽ�"; 
                          

                            string actionCode = "cancel";
                            string actionName = "PACS�˵�";
                            MemoryStream ms = new MemoryStream();
                            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("gb2312"));
                            JsonWriter writer = new JsonTextWriter(sw);
                            Encoding eee = sw.Encoding;
                            writer.WriteStartObject();
                            writer.WritePropertyName("reqHeader");
                            writer.WriteStartObject();
                            writer.WritePropertyName("callFunction");
                            writer.WriteValue("updateExaminationApply");
                            writer.WritePropertyName("systemId");
                            writer.WriteValue("PACS.BL");
                            writer.WritePropertyName("reqTimestamp");
                            writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            writer.WriteEndObject();
                            //���뵥  33333333333333333333
                            writer.WritePropertyName("requestForms");
                            writer.WriteStartArray();
                            writer.WriteStartObject();
                            //���뵥��
                            writer.WritePropertyName("requestFormHisNo");
                            writer.WriteValue(jcxx.Rows[0]["F_SQXH"].ToString());
                            //���뵥����
                            writer.WritePropertyName("requestName");
                            writer.WriteValue("");
                            //����
                            writer.WritePropertyName("requestFormSysNo");
                            writer.WriteValue( Ssbz.Trim());
                            //������
                            writer.WritePropertyName("requestType");
                            writer.WriteValue("");
                            //�����Ŀ�б�2222222222222
                            writer.WritePropertyName("orderItem");
                            writer.WriteStartArray();
                            string[] orderItemHisNos = yzxmid.Split('|');
                            foreach (string orderItemHisNo in orderItemHisNos)
                            {
                                if (orderItemHisNo.Trim() != "")
                                {
                                    writer.WriteStartObject();
                                    //�����뵥��
                                    writer.WritePropertyName("orderItemHisNo");
                                    writer.WriteValue(orderItemHisNo);
                                    //�����Ŀ����
                                    writer.WritePropertyName("orderCode");
                                    writer.WriteValue("");
                                    //�����Ŀ����
                                    writer.WritePropertyName("orderName");
                                    writer.WriteValue("");
                                    //�����Ŀ״̬
                                    writer.WritePropertyName("statusCode");
                                    writer.WriteValue(sqdztCode);
                                    //�����Ŀ״̬����
                                    writer.WritePropertyName("statusName");
                                    writer.WriteValue(sqdzt);
                                    writer.WriteEndObject();
                                }
                            }
                            writer.WriteEndArray();
                            ////22222222222222222222222222222222       

                            //����111111111111111111
                            writer.WritePropertyName("action");
                            writer.WriteStartArray();
                            writer.WriteStartObject();
                            //��������
                            writer.WritePropertyName("actionCode");
                            writer.WriteValue(actionCode);
                            //��������
                            writer.WritePropertyName("actionName");
                            writer.WriteValue(actionName);
                            //����ʱ��
                            writer.WritePropertyName("actionTime");
                            writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                            //������ע
                            writer.WritePropertyName("memo");
                            writer.WriteValue("");

                            //������   
                            writer.WritePropertyName("actor");
                            writer.WriteStartObject();
                            ///////////////////////////////////
                            //������ҽԺ
                            writer.WritePropertyName("hospital");
                            writer.WriteStartObject();
                            //������ҽԺ
                            writer.WritePropertyName("hospitalCode");
                            if (jcxx.Rows[0]["F_sjdw"].ToString() == "��Ժ" || jcxx.Rows[0]["F_sjdw"].ToString() == "��Ժ")
                                writer.WriteValue("1");
                            else if (jcxx.Rows[0]["F_sjdw"].ToString() == "��ѧ��ҽԺ" || jcxx.Rows[0]["F_sjdw"].ToString() == "��ѧ��")
                                writer.WriteValue("2");
                            else if (jcxx.Rows[0]["F_sjdw"].ToString() == "��ɳ��Ժ" || jcxx.Rows[0]["F_sjdw"].ToString() == "��ɳ")
                                writer.WriteValue("3");
                            else if (jcxx.Rows[0]["F_sjdw"].ToString() == "�����Ժ" || jcxx.Rows[0]["F_sjdw"].ToString() == "����")
                                writer.WriteValue("4");
                            else
                                writer.WriteValue("1");

                            //������ҽԺ
                            writer.WritePropertyName("hospitalName");
                            writer.WriteValue(jcxx.Rows[0]["F_sjdw"].ToString());
                            writer.WriteEndObject();
                            ////////////////////////////////////
                            //�����˿���
                            writer.WritePropertyName("department");
                            writer.WriteStartObject();
                            // ��������
                            writer.WritePropertyName("departmentName");
                            writer.WriteValue("");
                            // ���Ҵ���
                            writer.WritePropertyName("departmentCode");
                            writer.WriteValue("");
                            writer.WriteEndObject();
                            ////////////////////////
                            //����Ա����Ϣ
                            writer.WritePropertyName("employee");
                            writer.WriteStartObject();
                            // ��Ա����
                            writer.WritePropertyName("employeeName");
                            writer.WriteValue("");
                            // ��Ա����
                            writer.WritePropertyName("employeeCode");
                            writer.WriteValue("");
                            writer.WriteEndObject();
                            ////////////////////////
                            writer.WriteEndObject();


                            writer.WriteEndObject();
                            writer.WriteEndArray();
                            /////11111111111111111111111111111111
                            writer.WriteEndObject();
                            writer.WriteEndArray();
                            writer.WriteEndObject();
                            /////3333333333333333333333333
                            writer.Flush();
                            string rtnrequest =GetHisSQD(JsonSvcWeb, ms.ToArray(),debug);
                            JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);
                            string respCode = ja["respHeader"]["respCode"].ToString();
                            string respMessage = ja["respHeader"]["respMessage"].ToString();

                            if (respCode != "000000")
                              MessageBox.Show("ִ��ʧ�ܣ�" + respCode + "^" + respMessage);
                            else
                              MessageBox.Show("ִ�гɹ���" + respCode + "^" + respMessage);
                             return  "0";

                        # endregion
                }
             
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");
                    return "0";
                }
            } return "0";
        }

        public static string GetHisSQD(string JsonSvcWeb, byte[] reqbyte,string  debug)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {   string reqtext = Encoding.GetEncoding("gb2312").GetString(reqbyte);

                if (debug == "1")
                    log.WriteMyLog("��Σ�" + reqtext);
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(JsonSvcWeb);
                if (webrequest == null)
                {
                    MessageBox.Show("����סԺHIS����ʧ�ܣ�\r\n" + JsonSvcWeb);
                    return "0";
                }
                webrequest.KeepAlive = true;
                webrequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                webrequest.Method = "POST";
                webrequest.Timeout = 20000;
                webrequest.ContentType = "Content-Type";

        
                string Content = "";
                //����POST����  
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(reqtext);
                    using (Stream stream = webrequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    string[] values = webrequest.Headers.GetValues("Content-Type");
                    WebResponse myResponse = webrequest.GetResponse();
                    using (Stream resStream = myResponse.GetResponseStream())//�õ���д����
                    {
                        StreamReader newReader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                        Content = newReader.ReadToEnd();
                        newReader.Close();
                    }
                }
                catch (Exception ee3)
                {
                    MessageBox.Show("��ȡסԺHIS�����쳣��" + ee3.Message);
                    return "0";
                }
                if (debug == "1")
                    log.WriteMyLog("���أ�"+Content.Trim());

                return Content;


            }
            catch(Exception  ee2)
            {
                MessageBox.Show("��ȡסԺHIS�����쳣2��" + ee2.Message);
                return "0";
            }


        }

        public static string GetMZHis(string Ssbz, string Sslbx, string MZHISWeb,string debug)
        {
            #region  Sslbx == "���ƿ���" || Sslbx == "�Һ����")

            string request_xml = "<Request><CardNo>" + Ssbz.Trim() + "</CardNo><RegNo></RegNo></Request>";
            if(Sslbx == "���ƿ���")
                request_xml = "<Request><CardNo>" + Ssbz.Trim() + "</CardNo><RegNo></RegNo></Request>";

                if(Sslbx == "�Һ����")
                    request_xml = "<Request><CardNo></CardNo><RegNo>" + Ssbz.Trim() + "</RegNo></Request>";
           

            if (debug == "1")
                log.WriteMyLog("��ѯ������" + request_xml);
            string rtn_xml = "";
            try
            {
                gdszyyMzHisWeb.HisForInspectItemService mzhis = new LGHISJKZGQ.gdszyyMzHisWeb.HisForInspectItemService();
                if (MZHISWeb.Trim()!="")
                mzhis.Url = MZHISWeb;
                rtn_xml = mzhis.GetPatientInfo(request_xml);
            }
            catch (Exception ee)
            {
                MessageBox.Show("��ȡ������Ϣʧ�ܣ���������WebService�����쳣��" + ee.Message + "\r\n");
                return "0";
            }
            if (rtn_xml.Trim() == "")
            {
                MessageBox.Show("��ȡ������Ϣʧ�ܣ�������ϢΪ�գ�");
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog(rtn_xml);

            XmlNode xmlok_DATA = null;
            XmlDocument xd2 = new XmlDocument();
            try
            {
                xd2.LoadXml(rtn_xml);
                xmlok_DATA = xd2.SelectSingleNode("/Response");
            }
            catch (Exception xmlok_e)
            {

                MessageBox.Show("��ȡ������Ϣʧ�ܣ�����DATA�쳣��" + xmlok_e.Message);
                return "0";
            }

            string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
            string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

            if (ResultCode != "0")
            {
                MessageBox.Show("��ȡ������Ϣʧ�ܣ���" + ErrorMsg);
                return "0";
            }

            if (xmlok_DATA["Info"].InnerText == "")
            {
                MessageBox.Show("��ȡ������Ϣʧ�ܣ�infoΪ��");
                return "0";
            }
            string exp = "";
            //-����xml----------------------------------------------------
            try
            {

                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "���˱��=" + (char)34 + xmlok_DATA["Info"]["EmpiId"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
          
                    xml = xml + "���˱��=" + (char)34 + +(char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����ID=" + (char)34 + xmlok_DATA["Info"]["RegNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "�����=" + (char)34 + xmlok_DATA["Info"]["MedicalNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["Info"]["PatientName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "�Ա�=" + (char)34 + xmlok_DATA["Info"]["Sex"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + ZGQClass.CsrqToAge(xmlok_DATA["Info"]["Birthday"].InnerText.Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["Info"]["Marry"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "��ַ=" + (char)34 + xmlok_DATA["Info"]["PatientPhone"].InnerText.Trim() +"^"+ (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�绰=" + (char)34 + xmlok_DATA["Info"]["Address"].InnerText.Trim()  + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "���֤��=" + (char)34 + xmlok_DATA["Info"]["IdCard"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["Info"]["Nation"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }

                //----------------------------------------------------------
                try
                {
                    xml = xml + "ְҵ=" + (char)34 + xmlok_DATA["Info"]["Occupation"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                    try
                    {
                         xml = xml + "�ͼ�ҽԺ=" + (char)34 + xmlok_DATA["Info"]["HospitalShortName"].InnerText.Trim() + (char)34 + " ";
                    }
                catch
                    {
                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "�㶫ʡ��ҽԺ" + (char)34 + " ";
                }

                //----------------------------------------------------------
                xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ѱ�=" + (char)34 + xmlok_DATA["Info"]["MedicalTreatment"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                }
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                xml = xml + "</LOGENE>";

                if (debug == "1" && exp.Trim() != "")
                    log.WriteMyLog(exp.Trim());
                return xml;
            }
            catch (Exception e)
            {
                MessageBox.Show("��ȡ������Ϣ�쳣��"+e.Message);
                log.WriteMyLog("��ȡ������Ϣ�쳣��" + e.Message);
                return "0";
            }

            #endregion
        }

        public static string GetZYHis(string Ssbz, string Sslbx, string debug,string IP)
        {
            if (Sslbx == "סԺ�����")
            {
                MessageBox.Show("��ȡסԺ�������뵥��Ϣʧ�ܣ�");
                return "0";
            }


            string ZYHISWeb = f.ReadString(Sslbx, "WSURL", "http://svc.chas.gdhtcm.com:9080/ChasSvc/services/ChasCommonPort").Replace("\0", "").Trim(); ;

            string queryMode = f.ReadString(Sslbx, "queryMode", "Current").Replace("\0", "").Trim();

            string request_xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                    + "<QueryPatientRequest xmlns=\"http://chas.hit.com/transport/integration/common/msg\">"
                    + "<reqHeader>"
                    + "<systemId>AJ</systemId>"
                    + "<reqTimestamp>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</reqTimestamp>"
                    + "<terminalIp>" + IP + "</terminalIp>"
                    + "</reqHeader>"
                    + "<patientNo>" + Ssbz + "</patientNo>"
                    + "<startDate>" + DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + "</startDate>"
                    + "<endDate>" + DateTime.Today.ToString("yyyy-MM-dd") + "</endDate>"
                    + "<dateField>AdmissionDate</dateField>"
                    + "<queryMode>" + queryMode + "</queryMode>"
                    + "</QueryPatientRequest>";


            if (debug == "1")
                log.WriteMyLog("����XML��" + request_xml);

            string ResponseXMl = "";
            try
            {
                gdszyyWS.ChasCommonSvc zyy = new LGHISJKZGQ.gdszyyWS.ChasCommonSvc();
                if (ZYHISWeb != "")
                    zyy.Url = ZYHISWeb;
                ResponseXMl = zyy.queryPatient(request_xml);
            }
            catch (Exception ee2)
            {
                MessageBox.Show("��ȡסԺ���˻�����Ϣʧ�ܣ�" + ee2.Message);
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog("���أ�" + ResponseXMl);

            if (ResponseXMl.Trim() == "")
            {
                MessageBox.Show("��ȡסԺ���˻�����Ϣʧ�ܣ�����Ϊ��");
                return "0";
            }
            try
            {
            StringReader xmlstr = null;
            XmlTextReader xmread = null;
            xmlstr = new StringReader(ResponseXMl);
            xmread = new XmlTextReader(xmlstr);
            XmlDocument readxml2 = new XmlDocument();
            try
            {
                readxml2.Load(xmread);
            }
            catch (Exception e2)
            {
                MessageBox.Show("��ȡסԺ���˻�����Ϣʧ�ܣ�" + e2.Message.ToString());
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "http://chas.hit.com/transport/integration/common/msg");
            XmlNode xmlok_DATA = null;
                xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:respHeader", nsMgr);
            
            string ResultCode = xmlok_DATA["respCode"].InnerText.Trim(); 
            string ErrorMsg = xmlok_DATA["respMessage"].InnerText.Trim();
            if (ResultCode != "000000")
            {
                MessageBox.Show("��ȡסԺ���˻�����Ϣʧ�ܣ�" + ErrorMsg);
                return "0";
            }
            try
            {
                xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:patient", nsMgr);
                if (xmlok_DATA["name"].InnerText == "")
                {
                    MessageBox.Show("��ȡסԺ���˻�����Ϣʧ�ܣ�����Ϊ��");
                    return "0";
                }
            }
            catch
            {
                MessageBox.Show("��ȡסԺ���˻�����Ϣʧ�ܣ�δ��ѯ���˲�����Ϣ" );
                return "0";
            }
            string exp = "";
            //-����xml----------------------------------------------------
      
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "���˱��=" + (char)34 + xmlok_DATA["encounterNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {

                    xml = xml + "���˱��=" + (char)34 +"" +(char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����ID=" + (char)34 +"" +(char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "סԺ��=" + (char)34 + xmlok_DATA["patientNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["name"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "�Ա�=" + (char)34 + xmlok_DATA["sexName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + ZGQClass.CsrqToAge(xmlok_DATA["birthday"].InnerText.Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "��ַ=" + (char)34 + xmlok_DATA["phone"].InnerText.Trim() + "^" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�绰=" + (char)34 + xmlok_DATA["address"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["wardName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + xmlok_DATA["bedName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "���֤��=" + (char)34 + xmlok_DATA["idCardNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }

                //----------------------------------------------------------
                try
                {
                    xml = xml + "ְҵ=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + xmlok_DATA["departmentName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + xmlok_DATA["chargeDoctorName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + xmlok_DATA["hospitalName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "�㶫ʡ��ҽԺ" + (char)34 + " ";
                }

                //----------------------------------------------------------
                xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    string brlb = xmlok_DATA["chargeDoctorName"].InnerText.Trim();
                    if (brlb == "ECTY.IP.0000")
                        brlb = "סԺ";
                    else if (brlb == "ECTY.OP.0000")
                        brlb = "����";
                    else if (brlb == "ECTY.HLC.0000")
                        brlb = "���";
                    else if (brlb == "ECTY.AEU.0000")
                        brlb = "����";
                    else
                        brlb = "סԺ";

                    xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
                }
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                xml = xml + "</LOGENE>";

               if(debug=="1")
                   log.WriteMyLog(xml);
              return xml;
            }
            catch (Exception e)
            {
                MessageBox.Show("��ȡסԺ���˻�����Ϣ�쳣��" + e.Message);
                log.WriteMyLog("��ȡ������Ϣ�쳣��" + e.Message);
                return "0";
            }


        }


        public static string GetBbxx( string sqxh,ref  string bbmc)
        {
         
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string BBLB_XML = "";
            DataTable dt_bb = new DataTable();
        
                dt_bb = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_SQXH= '" + sqxh + "'  order by F_ID", "bbxx");
                string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                if (dt_bb == null)
                {
                    return "";
                }
                else
                {
                    if (dt_bb.Rows.Count <= 0)
                    {
                        return "";
                    }
                    else
                    {
                        BBLB_XML = "<BBLB>";
                        try
                        {
                            for (int x = 0; x < dt_bb.Rows.Count; x++)
                            {
                                try
                                {
                                    BBLB_XML = BBLB_XML + "<row ";
                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_bb.Rows[x]["F_BBXH"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bb.Rows[x]["F_BBTMH"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bb.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bb.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + dt_bb.Rows[x]["F_BZ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bb.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bb.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "/>";

                                    if (bbmc == "")
                                        bbmc = dt_bb.Rows[x]["F_BBMC"].ToString().Trim();
                                    else
                                        bbmc = bbmc + "," + dt_bb.Rows[x]["F_BBMC"].ToString().Trim();
                                }
                                catch (Exception eee)
                                {
                                    MessageBox.Show("��ȡ�걾�б���Ϣ�쳣��" + eee.Message);
                                    break;
                                }
                            }
                            BBLB_XML = BBLB_XML + "</BBLB>";

                            return BBLB_XML;
                        }
                        catch (Exception e3)
                        {
                            MessageBox.Show("��ȡ�걾�����쳣��" + e3.Message);
                        }
                        
                    }
                
            }
            return "";
        }
    }
}
