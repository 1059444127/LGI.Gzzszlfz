using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using LGHISJKZGQ;
using System.Net;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    //��ͷ����ҽԺ
    class STZXYY
    {
        //��ͷ����ҽԺ
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
         
            if (Sslbx != "")
            {

                string iszzk = f.ReadString(Sslbx, "iszzk", "").Trim();
              //  string rtn = "";
                //----------------------------------------------------------------------------------------------------------------------
                //-----------סԺ��-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "סԺ��")
                {

                    ////////SELECT  * FROM OPENQUERY([HIS_ZY] ,'select * from hisdbstzx.dbo.view_bl_patient_info where ZYH = ''724016'' ORDER BY RYRQ DESC ')
                    string con_str = f.ReadString("סԺ��", "odbcsql_zgq", "Data Source=172.16.0.55;Initial Catalog=hqint;User Id=bl;Password=bl123");
                    string hiszysql = f.ReadString("סԺ��", "hissql_zgq", "SELECT  * FROM OPENQUERY([HIS_ZY] ,'select * from hisdbstzx.dbo.view_bl_patient_info where ZYH = ''f_hissbh'' ORDER BY RYRQ DESC')");

                    hiszysql = hiszysql.Replace("f_hissbh", Ssbz.Trim());

                    DataSet ds = new DataSet();
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter(hiszysql, sqlcon);
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("���ݿ�����ʧ��," + ee.Message);
                    }
                        if (ds.Tables[0].Rows.Count <= 0)
                        {
                            MessageBox.Show("�޴�סԺ����Ϣ��");
                            return "0";
                        }

                        if (ds.Tables[0].Rows.Count > 1)
                        {

                            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                            string blhstr = "";
                            int y = ds.Tables[0].Rows.Count;
                            for (int z = 0; z < ds.Tables[0].Rows.Count; z++)
                            {
                                DataTable bljc = new DataTable();
                                bljc = aa.GetDataTable("select F_blh,F_SQXH,F_BRBH,F_XM from T_jcxx where F_SQXH='" + ds.Tables[0].Rows[z]["order_sn"].ToString() + "' and F_BRBH='" + ds.Tables[0].Rows[z]["patient_id"].ToString() + "'", "blxx");
                               if (bljc.Rows.Count > 0)
                                {
                               
                                    ds.Tables[0].Rows.RemoveAt(z);
                                    z = z - 1;
                                    blhstr = blhstr + bljc.Rows[0]["F_blh"].ToString()+";";
                          
                                }
                            }
                            if (ds.Tables[0].Rows.Count <=0)
                            {
                                MessageBox.Show("���������ѵǼǹ��������ٵǼǡ�����ţ�" + blhstr);
                                return "0";
                            }

                            stzxyy_select st_s = new stzxyy_select(ds, Sslbx);
                            st_s.ShowDialog();


                            string je = st_s.A;  //���
                            string sj = st_s.B;   //ʱ��
                            string bm = st_s.C;   //����

                            if (je == "" || sj == "" || bm == "")
                            {
                                return "0";
                            }
                            DataTable dtNew = new DataTable();
                            DataView view = new DataView();
                            view.Table = ds.Tables[0];
                            view.RowFilter = "order_sn='" + bm.Trim() + "' and sfje='" + je + "' and  ryrq='" + sj + "'"; 
                               dtNew = view.ToTable();

                            if (dtNew.Rows.Count < 1)
                            {
                                MessageBox.Show("�޴�סԺ����Ϣ��");
                                return "0";
                            }

                            try
                            {
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                xml = xml + "���˱��=" + (char)34 + dtNew.Rows[0]["patient_id"].ToString() + (char)34 + " ";
                                xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�������=" + (char)34 + dtNew.Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "סԺ��=" + (char)34 + dtNew.Rows[0]["zyh"].ToString() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + dtNew.Rows[0]["xm"].ToString() + (char)34 + " ";
                                string xb = dtNew.Rows[0]["sex_name"].ToString().Trim();
                                if (xb == "2")
                                    xml = xml + "�Ա�=" + (char)34 + "Ů" + (char)34 + " ";
                                else
                                    xml = xml + "�Ա�=" + (char)34 + "��" + (char)34 + " ";

                                xml = xml + "����=" + (char)34 + dtNew.Rows[0]["nl"].ToString() + (char)34 + " ";

                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "��ַ=" + (char)34 + dtNew.Rows[0]["lxdz"].ToString() + (char)34 + "   ";
                                xml = xml + "�绰=" + (char)34 + dtNew.Rows[0]["lxdh"].ToString() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + dtNew.Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + dtNew.Rows[0]["cwdm"].ToString() + (char)34 + " ";
                                xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                                xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�ͼ����=" + (char)34 + dtNew.Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "�ͼ�ҽ��=" + (char)34 + dtNew.Rows[0]["ysxm"].ToString() + (char)34 + " ";

                              

                                if (iszzk != "1")
                                {
                                    try
                                    {
                                        xml = xml + "�շ�=" + (char)34 + float.Parse(dtNew.Rows[0]["sfje"].ToString().Trim()).ToString() + (char)34 + " ";
                                    }
                                    catch
                                    {
                                        xml = xml + "�շ�=" + (char)34 + "0" + (char)34 + " ";
                                    }
                                }
                                xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
                                xml = xml + "ҽ����Ŀ=" + (char)34 + dtNew.Rows[0]["order_name"].ToString() + "^" + dtNew.Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                                xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<�ٴ���ʷ><![CDATA[" + "��Ժʱ�䣺" + dtNew.Rows[0]["ryrq"].ToString() + "]]></�ٴ���ʷ>";
                                xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                                xml = xml + "</LOGENE>";

                                return xml;

                            }
                            catch (Exception eee)
                            {
                                MessageBox.Show("XML�����쳣��" + eee.Message);
                                return "0";
                            }
                        }
                        else
                        {
                            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                             DataTable bljc = new DataTable();
                                bljc = aa.GetDataTable("select F_blh,F_SQXH,F_BRBH,F_XM from T_jcxx where F_SQXH='" + ds.Tables[0].Rows[0]["order_sn"].ToString() + "' and F_BRBH='" + ds.Tables[0].Rows[0]["patient_id"].ToString() + "'", "blxx");
                                if (bljc.Rows.Count > 0)
                                {
                                    MessageBox.Show("���������ѵǼǹ��������ٵǼǡ�����ţ�" + bljc.Rows[0]["F_blh"].ToString() + ",������" + bljc.Rows[0]["F_XM"].ToString());
                                    return "0";
                                }
                         
                            try
                            {
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[0]["patient_id"].ToString() + (char)34 + " ";
                                xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "סԺ��=" + (char)34 + ds.Tables[0].Rows[0]["zyh"].ToString() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["xm"].ToString() + (char)34 + " ";
                                string xb = ds.Tables[0].Rows[0]["sex_name"].ToString().Trim();
                                if (xb == "2")
                                    xml = xml + "�Ա�=" + (char)34 + "Ů" + (char)34 + " ";
                                else
                                    xml = xml + "�Ա�=" + (char)34 + "��" + (char)34 + " ";

                                xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["nl"].ToString() + (char)34 + " ";

                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "��ַ=" + (char)34 + ds.Tables[0].Rows[0]["lxdz"].ToString() + (char)34 + "   ";
                                xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[0]["lxdh"].ToString() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["cwdm"].ToString() + (char)34 + " ";
                                xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                                xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[0]["dept_name"].ToString() + (char)34 + " ";
                                xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[0]["ysxm"].ToString() + (char)34 + " ";

                                if (iszzk != "1")
                                {
                                    try
                                    {
                                        xml = xml + "�շ�=" + (char)34 + float.Parse(ds.Tables[0].Rows[0]["sfje"].ToString().Trim()).ToString() + (char)34 + " ";
                                    }
                                    catch
                                    {
                                        xml = xml + "�շ�=" + (char)34 + "0" + (char)34 + " ";
                                    }
                                }
                                xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
                                xml = xml + "ҽ����Ŀ=" + (char)34 + ds.Tables[0].Rows[0]["order_name"].ToString() + "^" + ds.Tables[0].Rows[0]["order_sn"].ToString() + (char)34 + " ";
                                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                                xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<�ٴ���ʷ><![CDATA[" + "��Ժʱ�䣺" + ds.Tables[0].Rows[0]["ryrq"].ToString() + "]]></�ٴ���ʷ>";
                                xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                                xml = xml + "</LOGENE>";

                                return xml;

                            }
                            catch (Exception eee)
                            {
                                MessageBox.Show("XML�����쳣��" + eee.Message);
                                return "0";
                            }
                        }
                    }
              

����������������//----------------------------------------------------------------------------------------------------------------------
                //-----------�ɣĺ�-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "ID��")
                {
                       /////ͨ���洢���̵��õ� MzInterfacePath_getinfo_Seleted @pid,varchar(12) 

                    string con_str = f.ReadString("ID��", "odbcsql_zgq", "Data Source=172.16.0.30;Initial Catalog=hisdb_stzx;User Id=bl;Password=bl123");
                    DataSet ds = new DataSet();
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter("MzInterfacePath_getinfo_Seleted", sqlcon);
                        objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        objAdapter.SelectCommand.Parameters.Add("@p_id", SqlDbType.VarChar, 12);
                        objAdapter.SelectCommand.Parameters["@p_id"].Value = Ssbz.Trim();
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch(Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("���ݿ�����ʧ��"+ee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                         MessageBox.Show("�޴�ID����Ϣ��");
                        return "0";
                    }

                    if (ds.Tables[0].Rows.Count > 1)
                    {
                      //  LG_GETHIS_ZGQ.cszxyy cs = new LG_GETHIS_ZGQ.cszxyy(ds1, 1);

                        stzxyy_select st_s = new stzxyy_select(ds, Sslbx);
                        st_s.ShowDialog();
                    

                        string JYSH = st_s.A;
                        string GRQ = st_s.B;

                        if (JYSH == "" || GRQ == "")
                        {
                            return "0";
                        }
                        DataTable dtNew = new DataTable();
                        DataView view = new DataView();
                        view.Table = ds.Tables[0];
                        view.RowFilter = "GHRQ='" + GRQ.Trim() + "' and JYSH='" + JYSH + "'";
                        dtNew = view.ToTable();


                        if (dtNew.Rows.Count< 1)
                        {     MessageBox.Show("�޴�ID����Ϣ��");
                         return "0";
                        } 
                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "���˱��=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "�������=" + (char)34 + dtNew.Rows[0]["JYSH"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "�����=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + dtNew.Rows[0]["DNAME"].ToString() + (char)34 + " ";
                            xml = xml + "�Ա�=" + (char)34 + dtNew.Rows[0]["SEX"].ToString() + (char)34 + " ";
                           // xml = xml + "����=" + (char)34 + dtNew.Rows[0]["NL"].ToString() + (char)34 + " ";
                               string nl = dtNew.Rows[0]["NL"].ToString();
                            if (nl.Contains("��") || nl.Contains("��") || nl.Contains("��"))
                                xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                            else
                                xml = xml + "����=" + (char)34 + nl + "��" + (char)34 + " ";

                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "�ͼ����=" + (char)34 + dtNew.Rows[0]["KSMC"].ToString() + (char)34 + " ";
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dtNew.Rows[0]["YSMC"].ToString() + (char)34 + " ";


                            if (iszzk != "1")
                            {
                                try
                                {
                                    xml = xml + "�շ�=" + (char)34 + float.Parse(dtNew.Rows[0]["SFJE"].ToString().Trim()).ToString() + (char)34 + " ";
                                }
                                catch
                                {
                                    xml = xml + "�շ�=" + (char)34 + "0" + (char)34 + " ";
                                }
                            }


                            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
                            xml = xml + "ҽ����Ŀ=" + (char)34 + dtNew.Rows[0]["JYXM"].ToString() + "^" + dtNew.Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "����1=" + (char)34 + (char)34 + " ";
                            xml = xml + "����2=" + (char)34 + (char)34 + " ";
                            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";
                            xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "�շ�ʱ�䣺" + dtNew.Rows[0]["GHRQ"].ToString() + "]]></�ٴ���ʷ>";
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                            xml = xml + "</LOGENE>";

                            return xml;

                        }
                        catch (Exception ee)
                        {
                          MessageBox.Show("xml�����쳣��"+ee.Message);
                            return "0";
                        }
                    }
                    else
                    {

                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["DNAME"].ToString() + (char)34 + " ";
                            xml = xml + "�Ա�=" + (char)34 + ds.Tables[0].Rows[0]["SEX"].ToString() + (char)34 + " ";

                            string nl = ds.Tables[0].Rows[0]["NL"].ToString();
                            if (nl.Contains("��") || nl.Contains("��") || nl.Contains("��"))
                                xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                            else
                                xml = xml + "����=" + (char)34 + nl + "��" + (char)34 + " ";


                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[0]["KSMC"].ToString() + (char)34 + " ";
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[0]["YSMC"].ToString() + (char)34 + " ";

                            if (iszzk != "1")
                            {

                                try
                                {
                                    xml = xml + "�շ�=" + (char)34 + float.Parse(ds.Tables[0].Rows[0]["SFJE"].ToString().Trim()).ToString() + (char)34 + " ";
                                }
                                catch
                                {
                                    xml = xml + "�շ�=" + (char)34 + "0" + (char)34 + " ";
                                }
                            }

                            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
                            xml = xml + "ҽ����Ŀ=" + (char)34 + ds.Tables[0].Rows[0]["JYXM"].ToString() + "^"+ds.Tables[0].Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "����1=" + (char)34 + (char)34 + " ";
                            xml = xml + "����2=" + (char)34 + (char)34 + " ";
                            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";
                            xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "�շ�ʱ�䣺" + ds.Tables[0].Rows[0]["GHRQ"].ToString() + "]]></�ٴ���ʷ>";
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                            xml = xml + "</LOGENE>";

                            return xml;

                        }
                        catch (Exception ee)
                        {
                          
                                MessageBox.Show("xml�����쳣��"+ee.Message);
                            return "0";
                        }
                    }
                }

                //----------------------------------------------------------------------------------------------------------------------
                //-----------�����б�-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "�����б�")
                {
                    /////ͨ���洢���̵��õ� MzInterfacePath_getinfo,�����б�
                    //////exec his.hisdbstzx.dbo.MzInterfacePath_getinfo   �����б�
                    //////exec his.hisdbstzx.dbo.MzInterfacePath_getinfo_Select  @pid    ID��ȡ��Ϣ��id�ţ�
                    //////exec his.hisdbstzx.dbo.MzInterfacePath_getinfo_Update  @pid @jy_sn   ��д���(id�ţ���Ŀ����)

                    string con_str = f.ReadString("�����б�", "odbcsql_zgq", "Data Source=172.16.0.30;Initial Catalog=hisdb_stzx;User Id=bl;Password=bl123");
                    DataSet ds = new DataSet();

                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter("MzInterfacePath_getinfo", sqlcon);
                        objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("���ݿ�����ʧ��,"+ee.Message);
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count <= 0)
                    {    MessageBox.Show("����Ҫ����Ϣ��");
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        //int  x=0;
                        //if (ds.Tables[0].Columns.Contains("JYSH") && ds.Tables[0].Columns.Contains("JYXM"))
                        //{
                        //    x=1;
                        //}

                        stzxyy_select st_s = new stzxyy_select(ds, Sslbx);
                        st_s.ShowDialog();
                        string JYSH = st_s.A.Trim();
                        string GRQ = st_s.B;
                        string brbh = st_s.C;

                        if(JYSH=="" || GRQ==""||brbh=="")
                        {   return "0";
                        }
                        DataTable dtNew = new DataTable();
                        DataView view = new DataView();
                        view.Table = ds.Tables[0];
                       
                       
                        //if (x == 0)
                        //    view.RowFilter = "BRBH='" + JYSH + "' and GHRQ='" + GRQ + "'";
                        //else
                            view.RowFilter = "BRBH='" + brbh + "' and GHRQ='" + GRQ.Trim() + "' and JYSH='" + JYSH + "'"; 
                    
                        dtNew = view.ToTable();

                        if (dtNew.Rows.Count < 1)
                        {
                            MessageBox.Show("�޴�ID����Ϣ��");
                            return "0";
                        }
                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "���˱��=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "�������=" + (char)34 + dtNew.Rows[0]["JYSH"].ToString() + (char)34 + " ";
                            xml = xml + "�����=" + (char)34 + dtNew.Rows[0]["BRBH"].ToString() + (char)34 + " ";
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + dtNew.Rows[0]["DNAME"].ToString() + (char)34 + " ";
                            xml = xml + "�Ա�=" + (char)34 + dtNew.Rows[0]["SEX"].ToString() + (char)34 + " ";
                            string nl = dtNew.Rows[0]["NL"].ToString();
                            if (nl.Contains("��") || nl.Contains("��") || nl.Contains("��"))
                                xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                            else
                                xml = xml + "����=" + (char)34 + nl + "��" + (char)34 + " ";


                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "�ͼ����=" + (char)34 + dtNew.Rows[0]["KSMC"].ToString() + (char)34 + " ";
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dtNew.Rows[0]["YSMC"].ToString() + (char)34 + " ";


                            if (iszzk != "1")
                            {

                                try
                                {
                                    xml = xml + "�շ�=" + (char)34 + float.Parse(dtNew.Rows[0]["SFJE"].ToString().Trim()).ToString() + (char)34 + " ";
                                }
                                catch
                                {
                                    xml = xml + "�շ�=" + (char)34 + "0" + (char)34 + " ";
                                }
                            }

                            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
                           
                            xml = xml + "ҽ����Ŀ=" + (char)34 + dtNew.Rows[0]["JYXM"].ToString() + "^" + dtNew.Rows[0]["JYSH"].ToString() + (char)34 + " ";
                         
                            xml = xml + "����1=" + (char)34 + (char)34 + " ";
                            xml = xml + "����2=" + (char)34 + (char)34 + " ";
                            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";
                            xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "�շ�ʱ�䣺" + dtNew.Rows[0]["GHRQ"].ToString() + "]]></�ٴ���ʷ>";
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                            xml = xml + "</LOGENE>";

                            return xml;

                        }
                        catch (Exception ee)
                        {
                                MessageBox.Show("xml�����쳣��"+ee.Message);
                            return "0";
                        }
                    }
                }


                //----------------------------------------------------------------------------------------------------------------------
                //-----------����-----------------------------------------------------------------------------------------------------
                //----------------------------------------------------------------------------------------------------------------------
                if (Sslbx == "����")
                {
                    string con_str = f.ReadString("����", "odbcsql_zgq", "Data Source=172.16.0.40;Initial Catalog=tj_db_setup;User Id=bl;Password=bl123");
                    //exec pro_bl_get_tj_info  @as_in_no   ���
                    DataSet ds = new DataSet();
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlDataAdapter objAdapter = new SqlDataAdapter("pro_bl_get_tj_info", sqlcon); 
                        objAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        objAdapter.SelectCommand.Parameters.Add("@as_in_no", SqlDbType.VarChar, 12);
                        objAdapter.SelectCommand.Parameters["@as_in_no"].Value = Ssbz.Trim();
                        objAdapter.Fill(ds, "brxx");
                        objAdapter.Dispose();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("���ݿ�����ʧ��,"+ee.ToString());
                        return "0";
                    }
                    if (ds.Tables[0].Rows.Count <= 0)
                    {    MessageBox.Show("�޴�������Ϣ��");
                        return rtn_xml();
                    }


                    try
                    {
                      

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[0]["����"].ToString() + (char)34 + " ";
                        xml = xml + "����ID=" + "" + (char)34 + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[0]["���ID"].ToString() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[0]["����"].ToString() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[0]["����"].ToString() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + ds.Tables[0].Rows[0]["�Ա�"].ToString() + (char)34 + " ";
                        string nl = ds.Tables[0].Rows[0]["����"].ToString();
                        if (nl.Contains("��") || nl.Contains("��") || nl.Contains("��"))
                        xml = xml + "����=" + (char)34 +nl  + (char)34 + " ";
                        else
                        xml = xml + "����=" + (char)34 + nl+"��" + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " "; 
                        string dz=ds.Tables[0].Rows[0]["��ַ"].ToString();
                        string dw=ds.Tables[0].Rows[0]["��λ"].ToString();  
                        if (string.IsNullOrEmpty(dz))
                            dz = "";
                        if (string.IsNullOrEmpty(dw))
                            dw = "";
                     
                        xml = xml + "��ַ=" + (char)34 +dz+"     "+dw + (char)34 + "   ";
                        if(string.IsNullOrEmpty(ds.Tables[0].Rows[0]["�绰"].ToString()))
                         xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        else
                        xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[0]["�绰"].ToString() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + ""+ (char)34 + " ";
                        xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + "����" + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                     
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + "���" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "���ʱ�䣺" + ds.Tables[0].Rows[0]["���ʱ��"].ToString() + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + "���" + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                   
                        return xml;

                    }
                    catch (Exception ee)
                    {
                      
                            MessageBox.Show("xml�����쳣��"+ee.ToString());
                        return "0";
                    }
                }


            
                MessageBox.Show("�޴�" + Sslbx);
                return "0";

            }
            MessageBox.Show("ʶ�����Ͳ���Ϊ��" );
            return "0";

        }
        public static string rtn_xml()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";

            xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��ͷ������ҽԺ" + (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
            xml = xml + "</LOGENE>";

            return xml;
        }

    }
}

