using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class bjxwyy
    {
        //��������ҽԺ
        //�洢����  exec BL_GetPatientInfo  '114640813','����'   �����סԺ�����ۣ�
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            
            if (Sslbx != "")
            {


                string con_str = f.ReadString(Sslbx, "odbcsql", "Server=.;Database=pathnet;User Id=pathnet;Password=4s3c2a1p;");
                string Sslbx1 = Sslbx;
                if (Sslbx == "סԺ��")
                    Sslbx1 = "סԺ";
                if (Sslbx == "�����")
                    Sslbx1 = "����";
                if (Sslbx == "��Ժ��")
                    Sslbx1 = "��Ժ";
                if (Sslbx == "�����")
                    Sslbx1 = "����";
                DataTable  dt=new DataTable();
                if (Sslbx1 != "")
                  dt= getXX(con_str, Sslbx1, Ssbz);
                else
                {
                    MessageBox.Show("�޴�ʶ��ţ�" + Sslbx1);
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
                    xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����ID=" + (char)34 + dt.Rows[0]["F_ZYCS"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�������=" + (char)34 + dt.Rows[0]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�����=" + (char)34 + dt.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "סԺ��=" + (char)34 + dt.Rows[0]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�Ա�=" + (char)34 + dt.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";

                    string datatime=DateTime.Today.Date.ToString();
                    if (dt.Rows[0]["F_RYRQ"].ToString().Trim() != "")
                        datatime = dt.Rows[0]["F_RYRQ"].ToString();
                    string CSRQ = dt.Rows[0]["F_CSRQ"].ToString();
                    try
                    {
                        int Year = DateTime.Parse(datatime).Year - DateTime.Parse(CSRQ).Year;
                        int Month = DateTime.Parse(datatime).Month - DateTime.Parse(CSRQ).Month;
                        int day = DateTime.Parse(datatime).Day - DateTime.Parse(CSRQ).Day;


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
                         int day1=DateTime.Parse(datatime).DayOfYear - DateTime.Parse(CSRQ).DayOfYear;

                         int m = day1 / 30;
                         int d = day1 % 30;
                         xml = xml + "����=" + (char)34 + m +"��"+ d+"��" + (char)34 + " ";
                        }

                    }
                    catch
                    {
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["F_HY"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                    xml = xml + "�绰=" + (char)34 + dt.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + dt.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "ְҵ=" + (char)34 + dt.Rows[0]["F_ZY"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["F_SJYS"].ToString().Trim() + (char)34 + " ";

                    xml = xml + "�շ�=" + (char)34 +"" + (char)34 + " ";

                    xml = xml + "�걾����=" + (char)34 + dt.Rows[0]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                    xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["F_YZXM"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                    xml = xml + "����2=" + (char)34 + (char)34 + " ";
                    xml = xml + "�ѱ�=" + (char)34 + dt.Rows[0]["F_FB"].ToString().Trim() + (char)34 + " ";

                    xml = xml + "�������=" + (char)34 + dt.Rows[0]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + dt.Rows[0]["F_LCBS"].ToString().Trim() + "    " + "��Ժʱ�䣺" + dt.Rows[0]["F_RYRQ"].ToString().Trim() + "]]></�ٴ���ʷ>";
                    xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
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

        public static DataTable getXX(string con_str, string Sslbx, string Ssbz)
        {
         
            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(con_str);
            try
            {

           
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "BL_GetPatientInfo";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter dbParameter = cmd.CreateParameter();
            dbParameter.DbType = DbType.String;
            dbParameter.ParameterName = "@code";
            dbParameter.Value = Ssbz;
            dbParameter.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter);

            SqlParameter dbParameter2 = cmd.CreateParameter();
            dbParameter2.DbType = DbType.String;
            dbParameter2.ParameterName = "@type";
            dbParameter2.Value = Sslbx;
            dbParameter2.Direction = ParameterDirection.Input;
            cmd.Parameters.Add(dbParameter2);

            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            con.Open();
            dap.Fill(ds);
            con.Close();
            cmd.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("HIS���ݿ����Ӵ���,"+e.ToString());
                log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�" + e.ToString());
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            //----------------------
            if (ds.Tables[0].Rows.Count > 1)
            {


                Frm_bjxwyy_select yc = new Frm_bjxwyy_select(ds);
                yc.ShowDialog();
                string sjys = yc.SJYS;
                string sqxh = yc.SQXH;
                string bbmc = yc.BBMC;  
                string zycs = yc.ZYCS;
                string ryrq= yc.RYRQ;

                DataTable dtNew = new DataTable();
                DataView view = new DataView();
                view.Table = ds.Tables[0];
                view.RowFilter = "F_SJYS = '" + sjys + "' and F_SQXH='" + sqxh + "' and F_bbmc='" + bbmc + "'and F_ZYCS='" + zycs + "'  and F_ryrq='" + ryrq + "'";
                dtNew = view.ToTable();
                return dtNew;
            }

            return ds.Tables[0];


        }
    }
}
