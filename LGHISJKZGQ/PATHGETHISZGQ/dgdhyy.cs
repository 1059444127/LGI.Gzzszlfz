using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    class dgdhyy
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            if (Sslbx != "")
            {
                string exp = "";
                string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                if (Sslbx == "�������뵥" || Sslbx == "�������뵥��")
                {

                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");

                    string sqlstr = "select *  from dbo.view_bdqpjcsqd   WHERE  registerid_int= '" + Ssbz.Trim() + "'";

                    DataTable dt_SQD = new DataTable();
                    string exp_db = "";
                    dt_SQD = db.Sql_DataAdapter(odbcsql, sqlstr, ref exp_db);


                    if (dt_SQD == null)
                    {
                        MessageBox.Show("���ݿ������쳣��" + exp_db);
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("δ��ѯ����������ż�¼��");
                        return "0";
                    }

                    //ȡ�շ�je
                    if (debug == "1")
                        MessageBox.Show(exp_db);

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_SQD.Rows[0]["patientid_vchr"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + dt_SQD.Rows[0]["iptimes_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_SQD.Rows[0]["registerid_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt_SQD.Rows[0]["PatientIpNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PatientName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt_SQD.Rows[0]["PatientSex"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PatientAge"].ToString().Trim()  + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
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
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_SQD.Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
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
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PatientBedNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
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
                        try
                        {
                            xml = xml + "ְҵ=" + (char)34 + dt_SQD.Rows[0]["PatientJob"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_SQD.Rows[0]["PatientDept"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_SQD.Rows[0]["SJYS"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------


                        xml = xml + "�շ�=" + (char)34 + "501" + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�걾����=" + (char)34 + dt_SQD.Rows[0]["BBMC"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
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
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_SQD.Rows[0]["BLZY"].ToString().Trim() + "]]></�ٴ���ʷ>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_SQD.Rows[0]["LCZD"].ToString().Trim() + "]]></�ٴ����>";
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
                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.Message);
                        return "0";
                    }
                }
                if (Sslbx == "���뵥��" || Sslbx == "���没�����뵥��")
                {
                   
                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");

                    string sqlstr = "select *  from dbo.view_bljcsqd   WHERE  registerid_int= '" + Ssbz.Trim() + "'";
                   
                    DataTable dt_SQD = new DataTable();
                    string exp_db = "";
                    dt_SQD = db.Sql_DataAdapter(odbcsql, sqlstr, ref exp_db);
                

                    if (dt_SQD == null)
                    {
                        MessageBox.Show("���ݿ������쳣��"+exp_db);
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("δ��ѯ����������ż�¼��");
                        return "0";
                    }

                    //ȡ�շ�je
                    if(debug=="1")
                        MessageBox.Show(exp_db); 

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_SQD.Rows[0]["patientid_vchr"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����ID=" + (char)34 + dt_SQD.Rows[0]["iptimes_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_SQD.Rows[0]["registerid_int"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�����=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt_SQD.Rows[0]["PatientIpNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PatientName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt_SQD.Rows[0]["PatientSex"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PatientAge"].ToString().Trim()  + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
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
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_SQD.Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PatientBedNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
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
                        try
                        {
                            xml = xml + "ְҵ=" + (char)34 + dt_SQD.Rows[0]["PatientJob"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_SQD.Rows[0]["PatientDept"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_SQD.Rows[0]["SJYS"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                    
                        xml = xml + "�շ�=" + (char)34 + "201" + (char)34 + " ";
                        //----------------------------------------------------------
                        ///////////////////////////
                        string BBLB_XML = "";
                        if (tqbblb == "1")
                        {
                           
                                        BBLB_XML = "<BBLB>";
                                        try
                                        {
                                              if(dt_SQD.Rows[0]["BBMC1"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW1"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"1" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC1"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW1"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }

                                            if(dt_SQD.Rows[0]["BBMC2"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW2"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"2" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC2"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW2"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }

                                             if(dt_SQD.Rows[0]["BBMC3"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW3"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"3" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC3"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW3"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC4"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW4"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"4" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC4"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW4"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC5"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW5"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"5" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC5"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW5"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC6"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW6"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"6" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC6"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW6"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC7"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW7"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"7"+ (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC7"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW7"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                             if(dt_SQD.Rows[0]["BBMC8"].ToString().Trim()!="" ||dt_SQD.Rows[0]["CQBW8"].ToString().Trim()!="" )
                                              {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +"8" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 +"" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD.Rows[0]["BBMC8"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD.Rows[0]["CQBW8"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD.Rows[0]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD.Rows[0]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "/>";
                                              }
                                                }
                                                catch (Exception eee)
                                                {
                                                    MessageBox.Show("��ȡ�걾�б���Ϣ�쳣��" + eee.Message);
                                                    tqbblb = "0";
                                                   
                                                }
                                            
                                        
                                     
                                        BBLB_XML = BBLB_XML + "</BBLB>";
                          
                        }
                        xml = xml + "�걾����=" + (char)34 + dt_SQD.Rows[0]["BBMC"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + ""+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
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
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_SQD.Rows[0]["SSSJ"].ToString().Trim() + "]]></�ٴ���ʷ>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_SQD.Rows[0]["LCZD"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                        }

                        if (tqbblb == "1")
                            xml = xml + BBLB_XML;
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("��ȡ��Ϣ���������²���");
                        log.WriteMyLog("xml��������---" + e.Message);
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
                    return "0";
                }
            }
            return "0";
        }
      
    }
}
