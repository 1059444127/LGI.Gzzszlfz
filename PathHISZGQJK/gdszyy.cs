        using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using PathHISZGQJK;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class gdszyy
    {

        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                {
                    //ȡ����˶���
                    qxsh = "1";
                }

                if (cslb[3].ToLower() == "new")
                {
                    xdj = "1";
                }

            }
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";

              string JsonSvcWeb = f.ReadString("savetohis", "JsonSvcWeb", "http://192.9.200.56:9080/ChasSvc/services/JsonSvc4Ris").Replace("\0", "").Trim();
              string yydm = f.ReadString("savetohis", "yydm", "1").Replace("\0", "").Trim();
              string MZHISWeb = f.ReadString("savetohis", "WSURL", "http://192.9.199.12:8002/HisForInspectItemService.asmx").Replace("\0", "").Trim(); //��ȡsz.ini�����õ�mrks
              string tomzhis = f.ReadString("savetohis", "tomzhis", "1").Replace("\0", "").Trim();
              string tozyhis = f.ReadString("savetohis", "tozyhis", "1").Replace("\0", "").Trim(); 

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

            string bgzt = "";

            if (qxsh == "1")
                bgzt = "ȡ�����";
            else
                bgzt=jcxx.Rows[0]["F_BGZT"].ToString();

            if(tozyhis=="1")
            {
            #region   ��дסԺHIS״̬
            if (jcxx.Rows[0]["F_brlb"].ToString() == "סԺ")
            {
                if (bglx == "cg" || jcxx.Rows[0]["F_SQXH"].ToString() !="" )
                {

                    string yzxmid="";
                    try
                    {
                        yzxmid = jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1];
                    }
                    catch
                    {
                    }
                    if (yzxmid == "")
                        return;

                    string actionCode = "register";
                    string actionName = "PACS�Ǽ�";
                    string sqdzt = ""; string sqdztCode = "";
                    if (bgzt == "�ѵǼ�") { sqdztCode = "CLDS.RISREGISTER.0000"; sqdzt = "�Ǽ�"; }
                    else if (bgzt == "�����") { sqdztCode = "CLDS.RISAUDIT.0000"; sqdzt = "��˱���"; actionCode = "updateStatus"; actionName = "����״̬"; }
                    else if (bgzt == "ȡ�����") { sqdztCode = "CLDS.RISFINISH.0000"; sqdzt = "�Ѽ��"; actionCode = "updateStatus"; actionName = "����״̬"; }
                    else if (bgzt == "��������") { sqdztCode = "CLDS.RISFINISH.0000"; sqdzt = "�Ѽ��"; actionCode = "updateStatus"; actionName = "����״̬"; }
                    else if (bgzt == "������") { sqdztCode = "CLDS.RIUNR.0000"; sqdzt = "ȡ���Ǽ�"; actionCode = "cancel"; actionName = "�˵�"; }
                    else
                        return;
                

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
                                    writer.WriteValue(blh);
                                     //������
                                    writer.WritePropertyName("requestType");
                                    writer.WriteValue("");
                                                                    //�����Ŀ�б�2222222222222
                                            writer.WritePropertyName("orderItem");
                                            writer.WriteStartArray();
                                            string[] orderItemHisNos = yzxmid.ToString().Split('|');
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

                                                    ////�����Ŀ��λ
                                                    //writer.WritePropertyName("subItems");
                                                    //writer.WriteStartArray();
                                                    //writer.WriteStartObject();
                                                    //        //�����Ŀ��λ����
                                                    //        writer.WritePropertyName("orderCode");
                                                    //        writer.WriteValue("");
                                                    //        //�����Ŀ��λ����
                                                    //        writer.WritePropertyName("orderName");
                                                    //        writer.WriteValue("");
                                                    //writer.WriteEndObject();
                                                    //writer.WriteEndArray();

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
                                            writer.WriteValue(jcxx.Rows[0]["F_WFBGYY"].ToString());
                                           
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
;
                        writer.Flush();

                  
                      string rtnrequest = GetHisSQD(JsonSvcWeb, ms.ToArray(),debug);

                      if (debug == "1")
                          log.WriteMyLog("���Σ�" + rtnrequest);

                      JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);
                      string respCode = ja["respHeader"]["respCode"].ToString();
                     string respMessage = ja["respHeader"]["respMessage"].ToString();

                     if (respCode != "000000")
                     {
                          log.WriteMyLog("ִ��ʧ�ܣ�" + respCode + "^" + respMessage);
                     
                     }
                     else
                     {
                         if(debug=="1")
                            log.WriteMyLog("ִ�гɹ���" + respCode + "^" + respMessage);
                      
                     }

                }
            }
            #endregion
            }
            if (tomzhis == "1")
            {
                #region  ����״ִ̬��
                if (jcxx.Rows[0]["F_brlb"].ToString() == "����" || jcxx.Rows[0]["F_brlb"].ToString() == "���")
                {
                    if (bglx == "cg" && jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "")
                    {
                        if (jcxx.Rows[0]["F_HXBJ"].ToString().Trim() != "1" || bgzt=="�����")
                        {
                            string deptid = f.ReadString("savetohis", "deptid", "60027").Replace("\0", "").Trim();
                            string yhgh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();

                            if (yhgh.Trim() == "")
                                yhgh = "0502";
                            string Flag = "1";
                            if (bgzt == "�����") Flag = "2";

                            gdszyyMzHisWeb.HisForInspectItemService mzhis = new gdszyyMzHisWeb.HisForInspectItemService();
                               
                            try
                            {
                                mzhis.Url = MZHISWeb;
                            }
                            catch(Exception  ee2)
                            {
                                log.WriteMyLog("����HIS web���������쳣��"+ee2.Message);
                                return;
                            }

                            string [] sqdh=jcxx.Rows[0]["F_SQXH"].ToString().Split(new char[2]{'|','|'});
                            for (int x = 0; x < sqdh.Length;x++ )
                            {
                                if (sqdh[x].Trim() == "")
                                    continue;

                                string request_xml = "<Request><CheckNumber>" + sqdh[x].Trim() + "</CheckNumber><ExecDeptId>" + deptid + "</ExecDeptId><ExecUserNo>" + yhgh + "</ExecUserNo><Flag>" + Flag + "</Flag></Request>";
                                if (debug == "1")
                                    log.WriteMyLog("������Ŀִ��������" + request_xml);
                                string rtn_xml = "";
                                try
                                {
                                    rtn_xml = mzhis.ExecuteCheckItem(request_xml);
                                    if (rtn_xml.Trim() == "")
                                    {
                                        //  MessageBox.Show("������Ŀִ��ʧ�ܣ�HIS���ؿ�");
                                        log.WriteMyLog("������Ŀִ��ʧ�ܣ�������ϢΪ�գ�");
                                    }
                                    else
                                    {
                                        if (debug == "1")
                                            log.WriteMyLog(rtn_xml);

                                        XmlNode xmlok_DATA = null;
                                        XmlDocument xd2 = new XmlDocument();
                                        try
                                        {
                                            xd2.LoadXml(rtn_xml);
                                            xmlok_DATA = xd2.SelectSingleNode("/Response");
                                            string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
                                            string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

                                            if (ResultCode != "0")
                                            {
                                                log.WriteMyLog("����ȡ��Ŀִ��ʧ�ܣ�" + ErrorMsg);
                                                //  MessageBox.Show("������Ŀִ��ʧ�ܣ�" + ErrorMsg);
                                            }
                                            else
                                            {
                                                aa.ExecuteSQL("update  T_JCXX  set F_HXBJ='1'  where F_BLH='" + blh + "'");
                                                if (debug == "1")
                                                    log.WriteMyLog("������Ŀִ�гɹ���" + ErrorMsg);
                                            }
                                        }
                                        catch (Exception xmlok_e)
                                        {
                                            // MessageBox.Show("������Ŀִ��ʧ�ܣ�����DATA�쳣");
                                            log.WriteMyLog("����DATA�쳣��" + xmlok_e.Message + "\r\n" + rtn_xml);
                                        }
                                    }
                                }
                                catch (Exception ee)
                                {
                                    //     MessageBox.Show("������Ŀִ��ʧ�ܣ�" + ee.Message);
                                    log.WriteMyLog("������Ŀִ��ʧ�ܣ�����WebService�����쳣��" + ee.Message + "\r\n");
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            if (bgzt == "�����" || bgzt == "ȡ�����")
                {


                string bgzt2 = "";
                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                try
                {
                    if (bglx.ToLower().Trim() == "bd")
                    {
                        dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    }
           
                    if (bglx.ToLower().Trim() == "bc")
                    {
                         dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
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
                    log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);

                if (bgzt2.Trim() == "�����" && bgzt != "ȡ�����")
                {
                    //////////////////////����pdf**********************************************************
                    string jpgname = "";  
                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
                    {
                        #region  ����pdf

                        string message = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        if (debug == "1")
                            log.WriteMyLog("��ʼ����PDF������");
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref  
                            message, ref jpgname);

                        string xy = "3";
                        if (isrtn)
                        {
                            if (File.Exists(jpgname))
                            {
                                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                if (ssa == true)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("�ϴ�PDF�ɹ�");

                                    jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                    ZgqClass.BGHJ(blh, "JK", "���", "�ϴ�PDF�ɹ�:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "�ϴ�PDF");

                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='�ϴ�PDF�ɹ�',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                                }
                                else
                                {
                                    log.WriteMyLog("�ϴ�PDFʧ�ܣ�" + message);
                                    ZgqClass.BGHJ(blh, "JK", "���", message, "ZGQJK", "�ϴ�PDF");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("����PDFʧ��:δ�ҵ��ļ�---" + jpgname);
                                ZgqClass.BGHJ(blh, "JK", "���", "�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname, "ZGQJK", "����PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='�ϴ�PDFʧ��:δ�ҵ��ļ�---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            }
                        }
                        else
                        {

                            log.WriteMyLog("����PDFʧ�ܣ�" + message);
                            ZgqClass.BGHJ(blh, "JK", "���", message, "ZGQJK", "����PDF");
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='����PDFʧ�ܣ�" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

                        }
                        zgq.DelTempFile(blh);
                        #endregion
                   }
                  
                    //////////////////////*****************************************************************
                }
                else
                {
                    if (bgzt == "ȡ�����")
                    {
                       
                        DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                        if (dt2.Rows.Count <= 0)
                            aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='�Ѵ���',F_bz='ȡ����˳ɹ���' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='δ����' and F_bgzt='" + bgzt + "'");
                        else
                        {
                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                             aa.ExecuteSQL("update T_JCXX_FS set F_bz='ȡ�����,ɾ��PDF�ɹ���',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_bgzt='ȡ�����'");
                        }

                        #region  �������
                        #endregion
                    }
                    else
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='������,״̬" + bgzt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_bgzt='ȡ�����'");
                }

            }
        }

        public static string GetHisSQD(string JsonSvcWeb, byte[] reqbyte, string debug)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                string reqtext = Encoding.GetEncoding("gb2312").GetString(reqbyte);

                if (debug == "1")
                    log.WriteMyLog("��Σ�" + reqtext);

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(JsonSvcWeb);
                if (webrequest == null)
                {
                  log.WriteMyLog("����סԺHIS����ʧ�ܣ�\r\n" + JsonSvcWeb);
                    return "0";
                }
                webrequest.KeepAlive = true;
               // webrequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                webrequest.Method = "POST";
                webrequest.Timeout = 10000;
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
                    log.WriteMyLog("����סԺHIS�����쳣��" + ee3.Message);
                    return "0";
                }

                return Content;


            }
            catch (Exception ee2)
            {
                log.WriteMyLog("����סԺHIS�����쳣2��" + ee2.Message);
                return "0";
            }


        }

    }
}
