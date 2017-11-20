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
using System.Data.SqlClient;
using LGHISJKZGQ;
using System.Net;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    class CQYCYY
    {
        //����ҽ�ƴ�ѧ��������ҽԺ
        //oracle ��
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {
                
                string con_str = f.ReadString(Sslbx, "odbcsql", " Data Source=.;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p;");
                //string sql_str = f.ReadString(Sslbx, "hissql", @"select  F_ZYH as סԺ��,F_MZH as �����,F_XM as ����,F_XB as �Ա�,F_NL as ����,F_CH as ����,F_SJKS as �ͼ����,F_LCZD as �ٴ����,F_SQXH as �������,F_BBMC as �걾����,F_SDRQ as ����ʱ�� from  T_JCXX ");

                string aa = "";
                DataTable dt;
              
                if (Sslbx == "סԺ��")
                {
                    aa = @"select ҽ��ID as ���˱��,�������, '' as �ѱ�,סԺ��,�����,����,�Ա�,����,'' as ����, '' as ��ַ,�绰,'' AS ����,������ as ����,'' as ���֤��,'����' as ����,'' as ְҵ,�������� as �ͼ����,����ҽ�� as �ͼ�ҽ��,�ٴ����,�ٴ����� as �ٴ���ʷ,'0' as �շ�,''  as ����ID, ����ID as �������,��λ as �걾����,'��Ժ' AS �ͼ�ҽԺ,�����Ŀ as ҽ����Ŀ, ����ʱ��  from  jk_bl where  סԺ��='" + Ssbz + "'   order by ����ʱ�� desc ";
                
               
                }
                if (Sslbx == "�����")
                {
                    aa = @"select ҽ��ID as ���˱��,�������, '' as �ѱ�,סԺ��,�����,����,�Ա�,����,'' as ����, '' as ��ַ,�绰,'' AS ����,������ as ����,'' as ���֤��,'����' as ����,'' as ְҵ,�������� as �ͼ����,����ҽ�� as �ͼ�ҽ��,�ٴ����,�ٴ����� as �ٴ���ʷ,'0' as �շ�,''  as ����ID, ����ID as �������,��λ as �걾����,'��Ժ' AS �ͼ�ҽԺ,�����Ŀ as ҽ����Ŀ ,����ʱ�� from  jk_bl where  �����='" + Ssbz + "'   order by ����ʱ�� desc ";
                
                }
                if (Sslbx == "����")
                {
                    aa = @"select ҽ��ID as ���˱��,�������, '' as �ѱ�,סԺ��,�����,����,�Ա�,����,'' as ����, '' as ��ַ,�绰,'' AS ����,������ as ����,'' as ���֤��,'����' as ����,'' as ְҵ,�������� as �ͼ����,����ҽ�� as �ͼ�ҽ��,�ٴ����,�ٴ����� as �ٴ���ʷ,'0' as �շ�,''  as ����ID, ����ID��as �������,��λ as �걾����,'��Ժ' AS �ͼ�ҽԺ,�����Ŀ as ҽ����Ŀ,����ʱ��  from  jk_bl where  ����='" + Ssbz + "'  and  �������='���' order by ����ʱ�� desc ";
                
                }
                if (Sslbx == "ҽ����")
                {
                    aa = @"select ҽ��ID as ���˱��,�������, '' as �ѱ�,סԺ��,�����,����,�Ա�,����,'' as ����, '' as ��ַ,�绰,'' AS ����,������ as ����,'' as ���֤��,'����' as ����,'' as ְҵ,�������� as �ͼ����,����ҽ�� as �ͼ�ҽ��,�ٴ����,�ٴ����� as �ٴ���ʷ,'0' as �շ�,''  as ����ID, ����ID as �������,��λ as �걾����,'��Ժ' AS �ͼ�ҽԺ,�����Ŀas ҽ����Ŀ,����ʱ��  from  jk_bl where  ����ID='" + Ssbz + "'   order by ����ʱ�� desc ";
                
                }

                if (aa == "")
                {
                    MessageBox.Show("��ѯ���������");
                    return "0";
                }
               
                try
                {
                  
                    dt = quxinxi(con_str, aa);

                  if (dt.Rows.Count < 1)
                  {
                      MessageBox.Show("�޴˲�����Ϣ��" + Sslbx + ":" + Ssbz,"��ʾ");
                      return "0";
                  }
                 
                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["���˱��"].ToString() + (char)34 + " ";
                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "�������=" + (char)34 + dt.Rows[0]["�������"].ToString() + (char)34 + " ";
                    xml = xml + "�����=" + (char)34 + dt.Rows[0]["�����"].ToString() + (char)34 + " ";
                    xml = xml + "סԺ��=" + (char)34 + dt.Rows[0]["סԺ��"].ToString() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString() + (char)34 + " ";
                    xml = xml + "�Ա�=" + (char)34 + dt.Rows[0]["�Ա�"].ToString() + (char)34 + " ";

                    xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString() + (char)34 + " ";

                    xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString() + (char)34 + " ";
                    xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["��ַ"].ToString() + (char)34 + "   ";
                    xml = xml + "�绰=" + (char)34 + dt.Rows[0]["�绰"].ToString() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString() + (char)34 + " ";
                    xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["���֤��"].ToString() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString() + (char)34 + " ";
                    xml = xml + "ְҵ=" + (char)34 + dt.Rows[0]["ְҵ"].ToString() + (char)34 + " ";
                    xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["�ͼ����"].ToString() + (char)34 + " ";
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["�ͼ�ҽ��"].ToString() + (char)34 + " ";

                    xml = xml + "�շ�=" + (char)34 + dt.Rows[0]["�շ�"].ToString() + (char)34 + " ";

                    xml = xml + "�걾����=" + (char)34 + dt.Rows[0]["�걾����"].ToString() + (char)34 + " ";
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                    xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["ҽ����Ŀ"].ToString() + (char)34 + " ";
                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                    xml = xml + "����2=" + (char)34 + (char)34 + " ";
                    xml = xml + "�ѱ�=" + (char)34 + dt.Rows[0]["�ѱ�"].ToString() + (char)34 + " ";

                    xml = xml + "�������=" + (char)34 + dt.Rows[0]["�������"].ToString() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + dt.Rows[0]["�ٴ���ʷ"].ToString() + "]]></�ٴ���ʷ>";
                    xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["�ٴ����"].ToString() + "]]></�ٴ����>";
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
            } 

        public static DataTable quxinxi(string con_str,string sql_str)
            {
                DataSet ds = new DataSet();
                OracleConnection orcon = new OracleConnection(con_str); ;
                try
                {


                    OracleDataAdapter orda = new OracleDataAdapter(sql_str, orcon);

                    orcon.Open();
                    orda.Fill(ds);
                    orcon.Close();
                    orda.Dispose();
                }

                catch (Exception e)
                {
                    MessageBox.Show("HIS���ݿ����Ӵ���");
                    log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�" + e.ToString());
                }
                finally
                {
                    if (orcon.State == ConnectionState.Open)
                        orcon.Close();
                }

             if (ds.Tables[0].Rows.Count > 1)
                   {
                       Frm_cqycyy yc = new Frm_cqycyy(ds);
                       yc.ShowDialog();
                    
                       string   index = yc.A;

                       DataTable dtNew = new DataTable();  
                      DataView view = new DataView();
                            view.Table = ds.Tables[0];
                            view.RowFilter = "������� = '"+index+"'";
                            dtNew = view.ToTable();
                           return dtNew;
                   }
                 
                 return  ds.Tables[0];
            

            }

    }
} 