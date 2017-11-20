using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class SXDEY
    {
        /// <summary>
        /// ɽ�����Ժ�������ӣ�webservices
        /// </summary>
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {

                string in_xml = "";

                if (Sslbx == "���￨��")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno>" + Ssbz + "</cardno>" +
                " <inpatno></inpatno>" +
                "<outpatno></outpatno>" +
                "<appid></appid></operation>"+
                "<modality>PS</modality>";

                }else
                if (Sslbx == "סԺ��")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno></cardno>" +
                " <inpatno>" + Ssbz + "</inpatno>" +
                "<outpatno></outpatno>" +
                "<appid></appid></operation>"+
                "<modality>PS</modality>";
                }
                else
                if (Sslbx == "�����")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno></cardno>" +
                " <inpatno></inpatno>" +
                "<outpatno>" + Ssbz + "</outpatno>" +
                "<appid></appid></operation>"+
                 "<modality>PS</modality>";
                }
                else
                if (Sslbx == "����ID")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno></cardno>" +
                " <inpatno></inpatno>" +
                "<outpatno></outpatno>" +
                "<appid>" + Ssbz + "</appid></operation>"+
                 "<modality>PS</modality>";
                }
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    return "0";
                }

                 if (in_xml.Trim() == "")
                {   MessageBox.Show("�޴�" + Sslbx);
                      return "0";
                }

                SXDEYWeb.MatrixService dey = new LGHISJKZGQ.SXDEYWeb.MatrixService();
               
                 
                    string Result = "";
                    string weburl = "";
                    weburl = f.ReadString(Sslbx, "URL", "");//��sz.ini �л�ȡwebservice�ĵ�ַ��Ҫ��д��
                    if (weburl != "")
                        dey.Url = weburl;


                    try
                    {
                        Result = dey.getExamOrders(in_xml); //��ȡ���ﲡ����Ϣ ������2�� 0��ʾ���1��ʾסԺ��
                       
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("�����쳣��"+ee.ToString());
                      log.WriteMyLog("���ղ���:" + Sslbx + "," + Ssbz + "," + ee.ToString());
                        return "0";
                    }

                    //xml����
                    if (Debug=="1")
                    MessageBox.Show("����xml��"+Result);
             
                    if (Result.Trim() == "" || Result==null)
                    {
                     MessageBox.Show("δ�鵽���ݼ�¼��");
                     log.WriteMyLog("δ�鵽���ݼ�¼");

                     return "0";
                    }
                    else
                    {
              
                        //DataSet ds1 = new DataSet();

                        //XmlDocument xd = new XmlDocument();
                        //if (Debug == "1")
                        //  MessageBox.Show(xmlstr);
                        //try
                        //{

                        //    StringReader sr = new StringReader(Result);
                        //    XmlReader xr = new XmlTextReader(sr);
                        //    ds1.ReadXml(xr);
                        //}
                        //catch(Exception  e2)
                        //{ 
                        //    MessageBox.Show("XML�����쳣;"+e2.ToString());
                        //    log.WriteMyLog("XML�����쳣"+e2.ToString()); 
                        //    return "0";
                        //}

                        XmlNode xmlok_DATA = null;
                        XmlDocument xdt = new XmlDocument();
                        try
                        {
                            xdt.LoadXml(Result);
                            xmlok_DATA = xdt.SelectSingleNode("/operation/orders");
                        }
                        catch (Exception xmlok_e)
                        {
                            MessageBox.Show("����DATA�쳣��" + xmlok_e.ToString());

                            return "0";
                        }
                   
                        if (!xmlok_DATA.HasChildNodes)
                        {
                            MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
                            return "0";
                        }
                        if (!xmlok_DATA.FirstChild.HasChildNodes)
                        {
                            MessageBox.Show("δ�ҵ���Ӧ�ļ�¼��");
                            return "0";
                        }
                        DataSet ds = new DataSet();

                        try
                        {
                            StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                            XmlReader xr = new XmlTextReader(sr);
                            ds.ReadXml(xr);

                        }
                        catch (Exception eee)
                        {
                            MessageBox.Show("תdataset�쳣��" + eee.ToString());
                            log.WriteMyLog("תdataset�쳣:" + eee);
                            return "0";
                        }
                  
                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            MessageBox.Show("δ�鵽��Ӧ�ļ�¼��");
                            return "0";
                        }
                  
                        DataTable dt = new DataTable();
                        if (ds.Tables[0].Rows.Count >1)
                        {
                             string Columns = f.ReadString(Sslbx, "Columns", "");//��ʾ����Ŀ
                             string Col = f.ReadString(Sslbx, "RowFilter", ""); //ѡ����������Ŀ


                                FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0],-1, Columns, Col);
                                yc.ShowDialog();
                                string string1 = yc.F_STRING[0];
                                string string2 = yc.F_STRING[1];
                                string string3 = yc.F_STRING[2];
                                string string4 = yc.F_STRING[3];

                                if (string1.Trim() == "" && string2.Trim() == "" && string3.Trim() == "" && string4.Trim() == "")
                                {
                                    MessageBox.Show("δѡ���¼");
                                    return "0";
                                }
                                DataView view = new DataView();
                                view.Table = ds.Tables[0];


                                string odr = "" + ds.Tables[0].Columns[0].ColumnName + "='" + string1 + "'  and  " + ds.Tables[0].Columns[1].ColumnName + "='" + string2 + "'  and  " + ds.Tables[0].Columns[2].ColumnName + "='" + string3 + "' and  " + ds.Tables[0].Columns[3].ColumnName + "='" + string4 + "'";

                                if (Col.Trim() != "")
                                {
                                    string[] colsss = Col.Split(',');
                                    odr = "" + colsss[0] + "='" + yc.F_STRING[0] + "'";
                                    if (colsss.Length > 1)
                                    {
                                        for (int i = 1; i < colsss.Length; i++)
                                        {
                                            if (i < 4)
                                                odr = odr + " and  " + colsss[i] + "='" + yc.F_STRING[i] + "' ";
                                        }
                                    }
                                }
                                view.RowFilter = odr;
                                dt = view.ToTable();
                        }
                        else
                        {
                            dt = ds.Tables[0];
                        }
                     //   dt = ds.Tables[0];
                        string exp = "";
                        //-����xml----------------------------------------------------
                        try
                        {

                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            try
                            {
                                xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["patid"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "����ID=" + (char)34 + dt.Rows[0]["cardno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����ID=" + (char)34 +"" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "�������=" + (char)34 + dt.Rows[0]["appid"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "�����=" + (char)34 + dt.Rows[0]["outpatno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                            try
                            {
                                xml = xml + "סԺ��=" + (char)34 + dt.Rows[0]["inpatno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                            try
                            {
                                xml = xml + "����=" + (char)34 + dt.Rows[0]["name"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                         
                            try
                            {
                                   string xb=dt.Rows[0]["sex"].ToString().Trim();
                                if(xb.Trim()=="F") xb="Ů";
                                else   if(xb.Trim()=="M") xb="��";
                                else  xb="";
                                xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                           
                            try
                            {
                                //string nl="";
                            string CSRQ = dt.Rows[0]["birthdate"].ToString().Trim();
                          
                                    string datatime=DateTime.Today.Date.ToString();

                                    if (CSRQ != "")
                                    { 
                                        int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                        int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                        int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;
                                      
                                        if (Year>=3)
                                        {

                                            if (Month>0)
                                               xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                            if (Month < 0)
                                               xml = xml + "����=" + (char)34 + (Year-1) + "��" + (char)34 + " ";
                                            if (Month == 0)
                                            {
                                               if(day>=0)
                                                   xml = xml + "����=" + (char)34 + Year + "��" + (char)34 + " ";
                                               else
                                                 xml = xml + "����=" + (char)34 + (Year-1) + "��" + (char)34 + " ";
                                            }
                                        }
                                        if (Year > 0 && Year < 3)
                                        {
                                            if((Year - 1)== 0)
                                            {
                                                if (Month <= 0)
                                                {
                                                    if (day > 0)
                                                        xml = xml + "����=" + (char)34 + (12 + Month) + "��" + day + "��" + (char)34 + " ";
                                                    else
                                                        xml = xml + "����=" + (char)34 + (12 + Month - 1) + "��" + (30 + day) + "��" + (char)34 + " ";
                                                }
                                                else
                                                    xml = xml + "����=" + (char)34 + Year + "��" + (Month) + "��" + (char)34 + " ";
                                            }
                                            else
                                            {
                                            if (Month > 0)
                                                xml = xml + "����=" + (char)34 +Year+"��"+ Month + "��"+ (char)34 + " ";
                                            else
                                                xml = xml + "����=" + (char)34 + (Year-1) + "��" + (12 + Month) + "��" + (char)34 + " ";
                                           
                                            }

                                        }
                                        if (Year== 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                         int m = day1 / 30;
                                         int d = day1 % 30;
                                         xml = xml + "����=" + (char)34 + m +"��"+ d+"��" + (char)34 + " ";
                                        }
                                    }
                                    

                              //  xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {

                                string hy=dt.Rows[0]["maritalstatus"].ToString().Trim();
                                if(hy=="M") hy="�ѻ�";
                                else if(hy=="B") hy="δ��";
                                   else if(hy=="D") hy="���";
                                   else if(hy=="W") hy="ɥż";
                                else hy="";
                                xml = xml + "����=" + (char)34 + hy + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["address"].ToString().Trim() + (char)34 + "   ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "�绰=" + (char)34 + dt.Rows[0]["phoneno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "����=" + (char)34 + dt.Rows[0]["ward"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "����=" + (char)34 + dt.Rows[0]["bedno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["idno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["deptname"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                            }
                            //----------------------------------------------------------

                            try
                            {
                                xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["doctorname"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                            xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "�걾����=" + (char)34 + dt.Rows[0]["BodySite"].ToString().Trim() + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["itemno"].ToString().Trim()+"^"+dt.Rows[0]["itemname"].ToString().Trim() + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "����1=" + (char)34 + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "����2=" + (char)34 + (char)34 + " ";
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "�ѱ�=" + (char)34 +"" + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                string brlb=dt.Rows[0]["patsource"].ToString().Trim();
                                 if(brlb=="I") brlb="סԺ";
                                else if(brlb=="O") brlb="����";
                                   else if(brlb=="E") brlb="���";
                                else brlb="";


                                xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                            }
                            xml = xml + "/>";
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["clinicdiag"].ToString().Trim() + "]]></�ٴ����>";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                            }
                            xml = xml + "</LOGENE>";

                            if (Debug == "1" && exp.Trim() != "")
                                log.WriteMyLog(exp.Trim());

                          
                            return xml;
                        }
                        catch (Exception e)
                        {
                         
                                MessageBox.Show("��ȡ��Ϣ����,"+e.ToString());
                            log.WriteMyLog("xml��������---" + e.ToString());
                            return "0";
                        }
                    }
            
                    
             }
            else
            {
                MessageBox.Show("�޴�" + Sslbx);
                if (Debug == "1")
                    log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");

             //   return "0";
            } return "0";

    }
    }
}
