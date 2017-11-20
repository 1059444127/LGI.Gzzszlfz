using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data;
using dbbase;

namespace LGHISJKZGQ
{
    //����2Ժ
    class xy2y
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            string exp = "";
            string WSURL = f.ReadString(Sslbx, "WSURL", "").Replace("\0", "").Trim();  //��ȡsz.ini�����õ�webservicesurl
            Debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim(); ;
               string isFY = f.ReadString(Sslbx, "isFY", "1").Replace("\0", "").Trim();
            if (Sslbx != "")
            {


                string rtn_XML = "";

                if (Sslbx == "ȡ������")
                {

                    #region
                    string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                         string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();

                    odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            
            DataTable jcxx = new DataTable();
            jcxx = aa.GetDataTable("select * from  T_JCXX  where F_BLH='" + Ssbz + "'", "jcxx");
            if (jcxx == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                return "0";
            }
            if (jcxx.Rows.Count < 1)
            { MessageBox.Show("�˲���Ų����ڣ�");
                return "0";
            }
            if (jcxx.Rows[0]["F_YZID"].ToString().Trim()=="")
            {
                MessageBox.Show("�˲�����ҽ�����޷�ȡ�����룡");
                return "0";
            }

                    string [] yzids=jcxx.Rows[0]["F_YZID"].ToString().Split('^');
                  
                    string Request_XML = "";
             foreach(string  yzid in yzids)
             {
                 if(yzid.Trim()!="")
                 {
                     Request_XML=Request_XML+"<ModifyStauts>"
                     + "<Status>U</Status>"
                     + "<Rowid>" + yzid + "</Rowid>"
                     + "<StudyNo>" + Ssbz + "</StudyNo>"
                     + "<ExeUser>" + yhbh + "@" + yhmc + "</ExeUser>"
                     + "<RBDate>" + DateTime.Today.ToString("yyyy-MM-dd") + "</RBDate>"
                     + "<RBTime>" + DateTime.Now.ToString("HH:mm:ss") + "</RBTime>"
                     + "<RBLoc>�����</RBLoc>"
                     + "</ModifyStauts>";
                 }
             }

             if (Request_XML == "")
             {
                 MessageBox.Show("�˲�����ҽ�����޷�ȡ�����룡");
                 return "0";
             }
             Request_XML = "<Request><ModifyStatuses>" + Request_XML + "</ModifyStatuses></Request>";


                    if (Debug == "1")
                        log.WriteMyLog(Request_XML);


                    string Response_XML = "";
                    try
                    {
                        xy2yWebService.XYEPACS xy2yweb = new LGHISJKZGQ.xy2yWebService.XYEPACS();
                        if (WSURL.Trim() != "")
                            xy2yweb.Url = WSURL;
                        Response_XML = xy2yweb.ModifyStatus(Request_XML);

                    }
                    catch (Exception ee1)
                    {
                        MessageBox.Show(ee1.Message);
                        log.WriteMyLog(ee1.Message);
                        return "0";
                    }

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(Response_XML);
                        xmlok_DATA = xd2.SelectSingleNode("/Response");
                    }
                    catch (Exception xmlok_e)
                    {
                        log.WriteMyLog(rtn_XML);
                        MessageBox.Show("����XML�쳣��" + xmlok_e.Message);
                        return "0";
                    }
                    if (xmlok_DATA["ResultCode"].InnerText.Trim() != "0")
                    {
                        MessageBox.Show("��ȡ����ʧ�ܣ�" + xmlok_DATA["ErrorMsg"].InnerText.Trim());
                        return "0";
                    }
                    #endregion

                }

                if (Sslbx == "����" || Sslbx == "סԺ��" || Sslbx == "����ID")
                {
                    string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                    string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string DepCode = f.ReadString(Sslbx, "DepCode", "2310000").Replace("\0", "").Trim();
                    if (yhbh == "")
                        yhbh = "0783";

                    string kh = ""; string zyh = ""; string brid = "";
                    string Inpatient = "N";
                    if (Sslbx == "����")
                    {
                        kh = Ssbz.Trim().Replace("\0", "").Trim();
                    }
                    if (Sslbx == "סԺ��")
                    {
                        zyh = Ssbz.Trim();
                        Inpatient = "Y";
                    }
                    if (Sslbx == "����ID")
                    {
                        brid = Ssbz.Trim();
                    }
                          
                    string Request_XML = "<Request>"
                        + "<StartDate>" + DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd") + "</StartDate>"
                        + "<EndDate>" + DateTime.Today.ToString("yyyy-MM-dd") + "</EndDate>"
                        + "<CardNo>" + kh + "</CardNo>"
                        + "<PatNo>" + brid + "</PatNo>"
                        + "<UserId>" + yhbh + "</UserId>"
                        + "<DepCode>" + DepCode + "</DepCode>"
                        + "<Inpatient>" + Inpatient + "</Inpatient>"
                        + "<WardCode></WardCode>"
                        + "<InpatientNo>" + zyh + "</InpatientNo>"
                        + "</Request>";

                    if (Debug == "1")
                        log.WriteMyLog(Request_XML);
                  

                    string Response_XML = "";
                    try
                    {
                        xy2yWebService.XYEPACS xy2yweb = new LGHISJKZGQ.xy2yWebService.XYEPACS();
                        if (WSURL.Trim() != "")
                            xy2yweb.Url = WSURL;
                        Response_XML = xy2yweb.RegInfo(Request_XML);

                    }
                    catch (Exception ee1)
                    {
                        MessageBox.Show("��ȡ�����쳣��"+ee1.Message);
                        log.WriteMyLog(ee1.Message);
                        return "0";
                    }

                    if (Response_XML == "")
                    {
                        MessageBox.Show("WebService����Ϊ�գ���ȡ����ʧ�ܣ�");
                        return "0";
                    }
                   
                    if(Debug=="1")
                        log.WriteMyLog(Response_XML);


                    try
                    {
                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(Response_XML);
                        xmlok_DATA = xd2.SelectSingleNode("/Response");
                    }
                    catch (Exception xmlok_e)
                    {
                        log.WriteMyLog(rtn_XML);
                        MessageBox.Show("����XML�쳣��" + xmlok_e.Message);
                        return "0";
                    }
                    if (xmlok_DATA["ResultCode"].InnerText.Trim() != "0")
                    {
                        MessageBox.Show("��ȡ����ʧ�ܣ�" + xmlok_DATA["ErrorMsg"].InnerText.Trim());
                        return "0";
                    }

                    XmlNode xn2 = xd2.SelectSingleNode("/Response/FindOrds/FindOrd");
                 
                    DataTable dt_sqd = ZGQClass.DT_SQD();
        
                        DataRow dr =dt_sqd.NewRow();
                        dt_sqd.Rows.Add(dr);
                        //PatInfo ������Ϣ
                       dr["F_XM"]=xn2["PatInfo"]["Name"].InnerText.Trim();
                       dr["F_BRBH"] = xn2["PatInfo"]["PatNo"].InnerText.Trim();
                       dr["F_XB"] = xn2["PatInfo"]["Sex"].InnerText.Trim().Split('@')[1];
                       dr["F_NL"] = xn2["PatInfo"]["Age"].InnerText.Trim();
                       dr["F_HY"] = xn2["PatInfo"]["Marry"].InnerText.Trim();
                       dr["F_DZ"] = xn2["PatInfo"]["Address"].InnerText.Trim();
                       dr["F_DH"] = "^"+xn2["PatInfo"]["Telephone"].InnerText.Trim();
                       try
                       {
                           dr["F_SFZH"] = xn2["PatInfo"]["IDCard"].InnerText.Trim();
                       }
                       catch
                       {
                           dr["F_SFZH"] = "";
                       }
                       dr["F_MZ"] = xn2["PatInfo"]["Nation"].InnerText.Trim();
                       dr["F_ZYH"] = xn2["PatInfo"]["InpatientNo"].InnerText.Trim();
                       dr["F_SJDW"] = "��Ժ";// xn2["PatInfo"]["Hospital"].InnerText.Trim();
                       //AdmInfo/AdmRecord ������Ϣ
                       dr["F_MZH"] = xn2["AdmInfo"]["AdmRecord"]["AdmDR"].InnerText.Trim();
                       dr["F_BRLB"] = xn2["AdmInfo"]["AdmRecord"]["AdmType"].InnerText.Trim().Split('@')[1];
                       dr["F_SJKS"] = xn2["AdmInfo"]["AdmRecord"]["Ward"].InnerText.Trim().Split('@')[1];// xn2["AdmInfo"]["AdmRecord"]["Loc"].InnerText.Trim().Split('@')[1];
                       dr["F_SJYS"] = xn2["AdmInfo"]["AdmRecord"]["Doctor"].InnerText.Trim().Split('@')[1];
                       dr["F_CH"] = xn2["AdmInfo"]["AdmRecord"]["BedNo"].InnerText.Trim();
                       dr["F_BQ"] = xn2["AdmInfo"]["AdmRecord"]["Ward"].InnerText.Trim().Split('@')[1];
                       dr["F_LCZD"] = xn2["AdmInfo"]["AdmRecord"]["ClinicDiagnose"].InnerText.Trim();
                       dr["F_LCZL"] = xn2["AdmInfo"]["AdmRecord"]["OperationInfo"].InnerText.Trim();
                      
                      
                       //// AdmInfo/AdmRecord/HISOrders/Order�շ���Ϣ ѭ��

                       DataTable dt_yzxx = new DataTable();
                   
                       DataColumn OrdRowid = new DataColumn("OrdRowid");
                       dt_yzxx.Columns.Add(OrdRowid);
                   
                       DataColumn OrdName = new DataColumn("OrdName");
                       dt_yzxx.Columns.Add(OrdName);
                
                       DataColumn OrdNum = new DataColumn("OrdNum");
                       dt_yzxx.Columns.Add(OrdNum);
                      
                       DataColumn ItemPrice = new DataColumn("ItemPrice");
                       dt_yzxx.Columns.Add(ItemPrice);
                  
                       DataColumn BillStatus = new DataColumn("BillStatus");
                       dt_yzxx.Columns.Add(BillStatus);
                   
                       DataColumn ApplyDate = new DataColumn("ApplyDate");
                       dt_yzxx.Columns.Add(ApplyDate);
                   
                       DataColumn AppTime = new DataColumn("AppTime");
                       dt_yzxx.Columns.Add(AppTime);
                
                       DataColumn OrdStatus = new DataColumn("OrdStatus");
                       dt_yzxx.Columns.Add(OrdStatus);
                
                       DataColumn OrdLoc = new DataColumn("OrdLoc");
                       dt_yzxx.Columns.Add(OrdLoc);

                       DataColumn RisBody = new DataColumn("RisBody");
                       dt_yzxx.Columns.Add(RisBody);
                       DataColumn UpdateUser = new DataColumn("UpdateUser");
                       dt_yzxx.Columns.Add(UpdateUser);

                       XmlDocument p1 = new XmlDocument();
                       p1.LoadXml(xn2["AdmInfo"]["AdmRecord"]["HISOrders"].OuterXml);
                       //ҽ����Ϣ
                       XmlNodeList ppplist2 = p1.SelectNodes("/HISOrders/Order");

                       foreach (XmlNode xn3 in ppplist2)
                       {
                            DataRow dr_yzxx =dt_yzxx.NewRow();
                           dt_yzxx.Rows.Add(dr_yzxx);

                           dr_yzxx["OrdRowid"] = xn3["OrdRowid"].InnerText.Trim();
                           dr_yzxx["OrdName"] = xn3["OrdName"].InnerText.Trim();
                           dr_yzxx["OrdNum"] = xn3["OrdNum"].InnerText.Trim();
                           dr_yzxx["ItemPrice"] = xn3["ItemPrice"].InnerText.Trim();
                           dr_yzxx["BillStatus"] = xn3["BillStatus"].InnerText.Trim();
                           dr_yzxx["ApplyDate"] = xn3["ApplyDate"].InnerText.Trim();
                           dr_yzxx["AppTime"] = xn3["AppTime"].InnerText.Trim();
                           dr_yzxx["OrdStatus"] = xn3["OrdStatus"].InnerText.Trim();
                           dr_yzxx["OrdLoc"] = xn3["OrdLoc"].InnerText.Trim();
                           dr_yzxx["RisBody"] = xn3["RisBody"].InnerText.Trim();
                           dr_yzxx["UpdateUser"] = xn3["UpdateUser"].InnerText.Trim();

                           if (dr["F_YZID"].ToString().Trim() == "")
                               dr["F_YZID"] = xn3["OrdRowid"].InnerText.Trim();
                           else
                               dr["F_YZID"] = dr["F_YZID"]+"^"+ xn3["OrdRowid"].InnerText.Trim();
                       }

                       if (isFY == "1")
                       {
                           xy2yShowFY xy = new xy2yShowFY(dt_yzxx);
                           xy.ShowDialog();
                       }

                    //-����xml----------------------------------------------------
                 

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + dt_sqd.Rows[0]["F_YZID"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "�����=" + (char)34 + kh.Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt_sqd.Rows[0]["F_ZYh"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt_sqd.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["F_NL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["F_HY"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + dt_sqd.Rows[0]["F_DZ"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_sqd.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt_sqd.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            if(dt_sqd.Rows[0]["F_brlb"].ToString().Trim()=="סԺ")
                                xml = xml + "�ͼ����=" + (char)34 + dt_sqd.Rows[0]["F_SJKS"].ToString().Trim() + (char)34 + " ";
                            else
                                xml = xml + "�ͼ����=" + (char)34 + "����" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 +"" + (char)34 + " ";
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
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqd.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 +""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + ""+(char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_sqd.Rows[0]["F_brlb"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_sqd.Rows[0]["F_LCZL"].ToString().Trim() + "]]></�ٴ���ʷ>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_sqd.Rows[0]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" )
                            log.WriteMyLog(xml);

                        if(exp.Trim() != "")
                            log.WriteMyLog( exp.Trim());

                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.Message.ToString());
                        return "0";
                    }
                }

                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    return "0";

                }
            } return "0";


        }
  

    }
}
