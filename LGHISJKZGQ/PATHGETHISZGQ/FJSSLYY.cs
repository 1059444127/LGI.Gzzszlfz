using System;
using System.Collections.Generic;
using System.Text;
using LGHISJKZGQ;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    //����ʡʡ��ҽԺ
    //�洢����  sql
    class FJSSLYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {

                Debug = "0";
                string con_str = f.ReadString("סԺ��", "sqlcon", "Server=192.1.168.6;Database=THIS4;User Id=logene;Password=logene;");
                Debug = f.ReadString("סԺ��", "Debug", "0");
                string yydm = f.ReadString("סԺ��", "yydm", "");//01��Ժ��02��Ժ
                string mzbh = f.ReadString("�����", "zj330", "1");

               
                string PatType = "";
                string HospNo = "";
                string CardNo = "";
                string ApplyNo = "";
                string IDInhosp = "";
                string exp = "";
                if (Sslbx == "סԺ��")
                {
                    PatType = "1";
                    HospNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "�����")
                {
                    PatType = "0";
                    if (mzbh=="1")
                     HospNo = "330" + Ssbz.Trim();
                    else
                    HospNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "סԺ����")
                {
                    PatType = "1";
                    CardNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "���￨��")
                {
                    PatType = "0";
                    CardNo = Ssbz.Trim();
                }
                else
                if (Sslbx == "סԺID��")
                {
                    PatType = "1";
                    IDInhosp = Ssbz.Trim();
                }
                else
                    if (Sslbx == "����ID��")
                    {
                        PatType = "0";
                        IDInhosp = Ssbz.Trim();
                    }
                    else
                    {
                        MessageBox.Show("�޴�ʶ��ţ�" + Sslbx);
                        return "0";
                    }

                ///////////////////////////////////////////////////
                DataTable dt = new DataTable();

                SqlConnection con = new SqlConnection(con_str);
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "rsp_TechGetPatInfo_logene";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter dbParameter = cmd.CreateParameter();
                    dbParameter.DbType = DbType.String;
                    dbParameter.ParameterName = "@PatType";
                    dbParameter.Value = PatType;
                    dbParameter.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter);

                    SqlParameter dbParameter2 = cmd.CreateParameter();
                    dbParameter2.DbType = DbType.String;
                    dbParameter2.ParameterName = "@HospNo";
                    dbParameter2.Value = HospNo;
                    dbParameter2.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter2);

                    SqlParameter dbParameter3 = cmd.CreateParameter();
                    dbParameter3.DbType = DbType.String;
                    dbParameter3.ParameterName = "@CardNo";
                    dbParameter3.Value = CardNo;
                    dbParameter3.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter3);

                    SqlParameter dbParameter4 = cmd.CreateParameter();
                    dbParameter4.DbType = DbType.String;
                    dbParameter4.ParameterName = "@ApplyNo";
                    dbParameter4.Value = ApplyNo;
                    dbParameter4.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter4);

                    SqlParameter dbParameter5 = cmd.CreateParameter();
                    dbParameter5.DbType = DbType.String;
                    dbParameter5.ParameterName = "@IDInHosp";
                    dbParameter5.Value = IDInhosp;
                    dbParameter5.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(dbParameter5);

                    if (yydm.Trim() != "")
                    {
                        SqlParameter dbParameter6 = cmd.CreateParameter();
                        dbParameter6.DbType = DbType.String;
                        dbParameter6.ParameterName = "@yydm";
                        dbParameter6.Value = yydm;
                        dbParameter6.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(dbParameter6);
                    }

                    SqlDataAdapter dap = new SqlDataAdapter(cmd);
                    con.Open();
                    dap.Fill(dt);
                    con.Close();
                    cmd.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show("HIS���ݿ����Ӵ���," + e.ToString());
                    log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�" + e.ToString());

                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("δ�ܲ�ѯ����Ӧ�����ݼ�¼����ȷ��" + Sslbx + "�Ƿ���ȷ");
                    return "0";
                }

                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    try
                    {
                        xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["PATIENTid"].ToString().Trim() + (char)34 + " ";
                    }
                    catch(Exception  ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "�����=" + (char)34 + dt.Rows[0]["CARDNO"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�����=" + (char)34 +"" + (char)34 + " ";
                    }

                  
                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt.Rows[0]["HOSPNO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                    

                    try
                    {
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["PATNAME"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "�Ա�=" + (char)34 + dt.Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["AGE"].ToString().Trim() + dt.Rows[0]["AGEUNIT"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["ADDRess"].ToString().Trim() + (char)34 + "   ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "��ַ=" + (char)34 +"" + (char)34 + "   ";
                    }

                    try
                    {
                        xml = xml + "�绰=" + (char)34 + dt.Rows[0]["phone"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["WARD"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����=" + (char)34 + ""+ (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["BEDNO"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����=" + (char)34 +""+ (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["IDNUM"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "���֤��=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "����=" + (char)34 + dt.Rows[0]["NATION"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                     xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                  
                    try
                    {
                        xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["APPLYDEPT"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                    }
                 
                
                    try
                    {
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["docname"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                    }

                    
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                        if (yydm == "02")
                            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        else
                            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                    xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                    if (yydm == "02")
                        xml = xml + "����2=" + (char)34 + "��Ժ|02" + (char)34 + " ";
                    else
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";



                    try
                    {
                        xml = xml + "�ѱ�=" + (char)34 + dt.Rows[0]["ChargeType"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�ѱ�=" + (char)34 +"" + (char)34 + " ";
                    }

                    try
                    {
                        xml = xml + "�������=" + (char)34 +dt.Rows[0]["WardOrReg"].ToString().Trim()  + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                    }
                    xml = xml + "/>";
                    try
                    {
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                    }

                    try
                    {
                        xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["clincdescname"].ToString().Trim() + "]]></�ٴ����>";
                    }
                    catch (Exception ee)
                    {
                        exp = exp + ee.ToString();
                        xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                    }
                    xml = xml + "</LOGENE>";

                    if (Debug=="1" && exp.Trim() != "")
                        log.WriteMyLog(exp.Trim());

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

        //public static DataTable getXX(string con_str, string Sslbx, string Ssbz)
        //{

        //    DataSet ds = new DataSet();
        //    SqlConnection con = new SqlConnection(con_str);
        //    try
        //    {


        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = con;
        //        cmd.CommandText = "BL_GetPatientInfo";
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter dbParameter = cmd.CreateParameter();
        //        dbParameter.DbType = DbType.String;
        //        dbParameter.ParameterName = "@code";
        //        dbParameter.Value = Ssbz;
        //        dbParameter.Direction = ParameterDirection.Input;
        //        cmd.Parameters.Add(dbParameter);

        //        SqlParameter dbParameter2 = cmd.CreateParameter();
        //        dbParameter2.DbType = DbType.String;
        //        dbParameter2.ParameterName = "@type";
        //        dbParameter2.Value = Sslbx;
        //        dbParameter2.Direction = ParameterDirection.Input;
        //        cmd.Parameters.Add(dbParameter2);

        //        SqlDataAdapter dap = new SqlDataAdapter(cmd);
        //        con.Open();
        //        dap.Fill(ds);
        //        con.Close();
        //        cmd.Dispose();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show("HIS���ݿ����Ӵ���," + e.ToString());
        //        log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�" + e.ToString());
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //    }
        //    //----------------------
        //    if (ds.Tables[0].Rows.Count > 1)
        //    {


        //        Frm_bjxwyy_select yc = new Frm_bjxwyy_select(ds);
        //        yc.ShowDialog();
        //        string sjys = yc.SJYS;
        //        string sqxh = yc.SQXH;
        //        string bbmc = yc.BBMC;
        //        string zycs = yc.ZYCS;
        //        string ryrq = yc.RYRQ;

        //        DataTable dtNew = new DataTable();
        //        DataView view = new DataView();
        //        view.Table = ds.Tables[0];
        //        view.RowFilter = "F_SJYS = '" + sjys + "' and F_SQXH='" + sqxh + "' and F_bbmc='" + bbmc + "'and F_ZYCS='" + zycs + "'  and F_ryrq='" + ryrq + "'";
        //        dtNew = view.ToTable();
        //        return dtNew;
        //    }

        //    return ds.Tables[0];


        //}
    }
}
