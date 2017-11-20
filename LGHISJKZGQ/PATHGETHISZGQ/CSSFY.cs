using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.Odbc;
using dbbase;

namespace LGHISJKZGQ
{
    /// <summary>
    /// ��ɳ�и��ױ���Ժ-���ӿ�
    /// </summary>
    class CSSFY
    {
          ///
          ///���
          ///
        private static LGHISJKZGQ.IniFiles f = new LGHISJKZGQ.IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx == "����" || Sslbx == "��������")
            {
                DataSet ds = new DataSet();
                OdbcConnection sqlcon = null;
                string odbcstr = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet-TJ;UID=sa;PWD=zoneking;"); //��ȡsz.ini�����õ�odbcsql
                //2017��10��9��15:56:52 ������
                //��ѯ������ID��ΪEXAM_NO,���һ���˶������ʱ,�ֲ�����Ŀ������.
                string sqlstr = f.ReadString(Sslbx, "hissql", "select ID as ���˱��, '���'  as �������,'-' as �ѱ�,'' as סԺ��,'' as �����,Name as ����,Sex as �Ա�,cast(datediff(year,BirthDay,getdate()) as varchar)+'��' as ����,'' as ����,Address as ��ַ,Tel as �绰,'' AS ����,'' as ����,'-' as ���֤��,'' as ����,'' as ְҵ,'�������' as �ͼ����,QYDoctor AS �ͼ�ҽ��,'' as �ٴ����,' ' as �ٴ���ʷ,'' as �շ�,Pacs_Item_Code as ����ID,Exam_No as �������,'' as �걾����,'��Ժ' AS �ͼ�ҽԺ,Item_Name as ҽ����Ŀ,'' as ����1,dwmc as ����2 from V_TCT_InterFace WHERE exam_no = 'f_sbh' AND StudyType='BL'  order by Exam_No  desc");
                sqlstr = sqlstr.Replace("f_sbh", "T"+Ssbz.Trim());
                string count = f.ReadString(Sslbx, "count", "");
                if (Debug == "1")
                    MessageBox.Show(sqlstr);
                try
                {
                    sqlcon = new OdbcConnection(odbcstr);
                    OdbcDataAdapter sqlda = new OdbcDataAdapter(sqlstr, sqlcon);
                    sqlda.Fill(ds, "tjxx");
                    sqlda.Dispose();
                }
                catch(Exception ee)
                {
                    MessageBox.Show("���ϵͳ���ݿ����Ӵ��󣡣���"+ee.ToString());
                    return "0";
                }
                finally
                {
                    if (sqlcon.State == ConnectionState.Open)
                        sqlcon.Close();
                }

                if (ds.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("�������޼�¼��");
                    return "0";
                }

                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 1)
                {
                    string Columns = f.ReadString(Sslbx, "Columns", "");
                    string Col = f.ReadString(Sslbx, "RowFilter", "");



                    if (count.Trim() == "")
                        count = "-1";

                    FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0], int.Parse(count), Columns, Col);
                    yc.ShowDialog();
                    string string1 = yc.F_STRING[0];
                    string string2 = yc.F_STRING[1];
                    string string3 = yc.F_STRING[2];
                    string string4 = yc.F_STRING[3];

                    if (string1.Trim() == "" && string2.Trim() == "")
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
                    ///   MessageBox.Show(odr);
                    view.RowFilter = odr;

                    dt = view.ToTable();
                }
                else
                    dt = ds.Tables[0];

           
                if (dt.Rows.Count<1)
                {
                    MessageBox.Show("�������޼�¼������");
                    return "0";
                }
                //-����xml----------------------------------------------------
                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["���˱��"].ToString() + (char)34 + " ";
                    xml = xml + "����ID=" + (char)34 + dt.Rows[0]["����ID"].ToString() + (char)34 + " ";
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
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + dt.Rows[0]["�ͼ�ҽԺ"].ToString() + (char)34 + " ";
                    xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["ҽ����Ŀ"].ToString() + (char)34 + " ";
                    xml = xml + "����1=" + (char)34 + dt.Rows[0]["����1"].ToString() + (char)34 + " ";
                    xml = xml + "����2=" + (char)34 + dt.Rows[0]["����2"].ToString() + (char)34 + " ";
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
    }
}
