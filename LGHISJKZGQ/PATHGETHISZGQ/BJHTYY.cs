using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.Odbc;

namespace LGHISJKZGQ
{
    /// <summary>
    /// ��������ҽԺ���ӿڡ�����ͼ
    /// </summary>
    class BJHTYY
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {


        
                if (Sslbx == "����")
                {
                   // string con_str = f.ReadString(Sslbx, "odbcsql", "Server=.;Database=pathnet;User Id=pathnet;Password=4s3c2a1p;");
                    string odbcsql = f.ReadString("����", "odbcsql", "DSN=pathnet-his;UID=INTERFACEPA;PWD=INTERFACEPA");
                    string sql = "select checkupcode,EXAMCODE,NAME,GENDER,floor(months_between(sysdate,BIRTHDAY)/12)||'��' as age,ORDERNAME,APPLYDEPARTMENT,APPLYDOCTOR,PATIENTCODE,PATIENTSOURCE,INPATIENTCODE,OUTPATIENTCODE,marriage,ADDRESS,PHONE,ZONE,BEDNO,IDCARDCODE,RACE,CLINICALDIAG  from hthis.view_ht_interface_pat WHERE checkupcode ='" + Ssbz + "'";                    
                    DataTable dt = new DataTable();
                    if (Sslbx != "")
                        dt = getXX(odbcsql, sql, Ssbz);
                    else
                    {
                        MessageBox.Show("�޴�ʶ��ţ�" + Sslbx);
                        return "0";
                    }
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("δ�ܲ�ѯ����Ӧ�����ݼ�¼����ȷ�Ϻ����Ƿ���ȷ");
                        return "0";
                    }

                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["PATIENTCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt.Rows[0]["checkupcode"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt.Rows[0]["EXAMCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt.Rows[0]["OUTPATIENTCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt.Rows[0]["INPATIENTCODE"].ToString().Trim() + (char)34 + " ";
                      
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        string xb = dt.Rows[0]["GENDER"].ToString().Trim();
                        if (xb == "F" || xb == "f")
                            xb = "Ů";
                        if (xb == "M" || xb == "m")
                            xb = "��";
                        xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["age"].ToString().Trim() + (char)34 + " ";
                       
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["ADDRESS"].ToString().Trim() + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt.Rows[0]["PHONE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["ZONE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["BEDNO"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["IDCARDCODE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["RACE"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["APPLYDEPARTMENT"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["APPLYDOCTOR"].ToString().Trim() + (char)34 + " ";

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["ORDERNAME"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 +"" + (char)34 + " ";

                        xml = xml + "�������=" + (char)34 + "���" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA["  + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["CLINICALDIAG"].ToString().Trim() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";

                        return xml;


                    }
                    catch (Exception e)
                    {
                        if (Debug == "1")
                            MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    if (Debug == "1")
                        log.WriteMyLog(Sslbx + Ssbz + "�����ڣ�");

                    return "0";
                }
            } return "0";
        }

        public static DataTable getXX(string con_str, string sql, string Ssbz)
        {

            DataSet ds = new DataSet();
            OdbcConnection ocn = new OdbcConnection(con_str);
            try
            {
                OdbcDataAdapter dap = new OdbcDataAdapter(sql, ocn);
                ocn.Open() ;
                dap.Fill(ds);
                ocn.Close();
                }
                catch (Exception eee)
                {
                    ocn.Close();
                    log.WriteMyLog("���ݿ�ִ���쳣��" + eee.ToString());
                  
                        MessageBox.Show("���ݿ�ִ���쳣��" + eee.ToString());
                        return ds.Tables[0];
                }
                finally
                {
                    if (ocn.State == ConnectionState.Open)
                        ocn.Close();
                }
           
            //----------------------
            if (ds.Tables[0].Rows.Count > 1)
            {

                Frm_bjhtyy yc = new Frm_bjhtyy(ds);
                yc.ShowDialog();
                string sqxh = yc.SQXH;
                if (sqxh.Trim() == "")
                {
                    MessageBox.Show("δѡ�����ݣ�������ȡ");
                    return new DataTable();
                }
                DataTable dtNew = new DataTable();
                DataView view = new DataView();
                view.Table = ds.Tables[0];
                view.RowFilter = "EXAMCODE='" + sqxh + "' ";
                dtNew = view.ToTable();
                return dtNew;
            }

            return ds.Tables[0];


        }
    }
}