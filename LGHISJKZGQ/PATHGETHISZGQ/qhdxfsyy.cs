using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class qhdxfsyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {


            if (Sslbx != "")
            {
                string err = "";
                string tqbblb = ZGQClass.getSZ_String(Sslbx, "tqbblb", "1");
                string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                string odbcsql = ZGQClass.getSZ_String(Sslbx, "odbcsql", "Data Source=ORCL_100;User ID=jk_ycxd;Password=123");
                string Sslbx1 = Sslbx;
                if (Sslbx == "���뵥��" || Sslbx == "�걾�����")
                {
                    OracleDB_ZGQ oracledb = new OracleDB_ZGQ();
                    

                    string sqdh = "";

                    if (Sslbx == "�걾�����")
                    {
                        //ͨ���걾����Ż�ȡ���뵥��
                        DataTable dt_bblb = new DataTable();
                        dt_bblb = oracledb.Oracle_DataAdapter(odbcsql, "select *  from portal_his.emr_jcsq_blmx  where tmh='" + Ssbz + "'", ref err);
                        if (dt_bblb == null)
                        {
                            MessageBox.Show("��ѯ�˱걾����ŵļ�¼�쳣��" + err.Trim());
                            log.WriteMyLog("��ѯ�˱걾����ŵļ�¼�쳣��" + err.Trim() + "\r\n" + "select *  from portal_his.emr_jcsq_blmx  where tmh='" + Ssbz + "'");
                            return "0";
                        }

                        if (dt_bblb.Rows.Count > 0)
                        {
                            sqdh = dt_bblb.Rows[0]["sqdh"].ToString().Trim();
                        }
                        else
                        {
                            MessageBox.Show("δ��ѯ���˱걾����ŵļ�¼"+ err.Trim());
                            log.WriteMyLog("δ��ѯ���˱걾����ŵļ�¼" + err.Trim() + "\r\n" + "select *  from portal_his.emr_jcsq_blmx  where tmh='" + Ssbz + "'");
                            return "0";
                        }

                    }
                    if (Sslbx == "���뵥��")
                        sqdh = Ssbz;
                    err = "";
                    DataTable dt_sqd = new DataTable();

                    string sql_sqd = "select emr_jcsq.*,ksmc from portal_his.emr_jcsq,portal_his.gy_ksdm where  portal_his.emr_jcsq.kdks = portal_his.gy_ksdm.ksdm  and  sqdh='" + sqdh + "'";
                    dt_sqd = oracledb.Oracle_DataAdapter(odbcsql, sql_sqd, ref err);

                    if (dt_sqd == null)
                    {
                        MessageBox.Show("��ѯ���뵥�ļ�¼�쳣��" + err.Trim()+"");
                        log.WriteMyLog("��ѯ���뵥�ļ�¼�쳣��" + err.Trim()+"\r\n"+sql_sqd);
                        return "0";
                    }

                    if (dt_sqd.Rows.Count <= 0)
                    {
                        MessageBox.Show("δ��ѯ�����뵥�ļ�¼"+err.Trim());
                        log.WriteMyLog("δ��ѯ�����뵥�ļ�¼"+err.Trim()+"\r\n"+sql_sqd);
                        return "0";
                    }


                    string exp = "";
                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[0]["BRID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        string brlb = dt_sqd.Rows[0]["jzlx"].ToString().Trim();
                        if (brlb == "1") brlb = "����";
                        if (brlb == "2") brlb = "סԺ";


                        try
                        {
                            if (brlb != "סԺ")
                                xml = xml + "�����=" + (char)34 + dt_sqd.Rows[0]["patientid"].ToString().Trim() + (char)34 + " ";
                            else
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
                            if (brlb == "סԺ")
                                xml = xml + "סԺ��=" + (char)34 + dt_sqd.Rows[0]["patientid"].ToString().Trim() + (char)34 + " ";
                            else
                                xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["brxm"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------1��2Ů
                        try
                        {
                            string xb = dt_sqd.Rows[0]["brxb"].ToString().Trim();

                            if (xb == "1") xb = "��";
                            else if (xb == "2") xb = "Ů";
                            else xb = "";
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

                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["brnl"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "�绰=" + (char)34 + dt_sqd.Rows[0]["lxdh"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "����=" + (char)34 + dt_sqd.Rows[0]["brch"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_sqd.Rows[0]["KSMC"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqd.Rows[0]["sjys"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        ///////////////////////////
                        string BBLB_XML = "";
                        string bbmc = "";
                        if (tqbblb == "1")
                        {
                            //ͨ�����뵥�Ż�ȡ�걾��ϸ
                            err = "";
                            DataTable dt_bblb = new DataTable();
                            dt_bblb = oracledb.Oracle_DataAdapter(odbcsql, "select  *  from portal_his.emr_jcsq_blmx where sqdh='" + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + "'", ref err);
                            if (dt_bblb == null)
                            {
                                MessageBox.Show("��ѯ�����뵥�걾��ϸ�ļ�¼�쳣��" + err.Trim());
                                log.WriteMyLog("��ѯ�����뵥�걾��ϸ�ļ�¼�쳣��" + err.Trim() + "\r\n" + "select  *  from portal_his.emr_jcsq_blmx where sqdh='" + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + "'");
                                tqbblb = "0";
                            }
                            else
                            {
                                if (dt_bblb.Rows.Count > 0)
                                {
                                    BBLB_XML = "<BBLB>";
                                    try
                                    {
                                        for (int x = 0; x < dt_bblb.Rows.Count; x++)
                                        {
                                            try
                                            {
                                                BBLB_XML = BBLB_XML + "<row ";
                                                BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_bblb.Rows[x]["plxh"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bblb.Rows[x]["tmh"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bblb.Rows[x]["bbmc"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bblb.Rows[x]["bbbw"].ToString().Trim() + (char)34 + " "; 
                                                BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bblb.Rows[x]["bbsl"].ToString().Trim() + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + yhmc + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                                BBLB_XML = BBLB_XML + "/>";
                                                bbmc = bbmc + " " + dt_bblb.Rows[x]["bbmc"].ToString().Trim();
                                            }
                                            catch (Exception eee)
                                            {
                                                MessageBox.Show("�����걾�б���Ϣ�쳣��" + eee.Message);
                                                tqbblb = "0";
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception e3)
                                    {
                                        MessageBox.Show("��ȡ�걾�����쳣��" + e3.Message);
                                        tqbblb = "0";
                                    }

                                    BBLB_XML = BBLB_XML + "</BBLB>";
                                }
                                else
                                {
                                    MessageBox.Show("�����뵥�ޱ걾������Ϣ"+err.Trim());

                                    log.WriteMyLog("�����뵥�ޱ걾������Ϣ" + err.Trim() + "\r\n" + "select  *  from portal_his.emr_jcsq_blmx where sqdh='" + dt_sqd.Rows[0]["sqdh"].ToString().Trim() + "'");
                             
                                    tqbblb = "0";
                                }
                            }
                        }


                        xml = xml + "�걾����=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ"+ (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqd.Rows[0]["sqxm"].ToString().Trim() + (char)34 + " ";
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

                            xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------

                        string bszy = "��ʷժҪ��"+dt_sqd.Rows[0]["bszy"].ToString().Trim()+"\r\n";
                        string zyjcjg = "��Ҫ�������" + dt_sqd.Rows[0]["zyjcjg"].ToString().Trim()+"\r\n";
                        string ssjl = "������¼���ھ�����"+dt_sqd.Rows[0]["ssjl"].ToString().Trim()+"\r\n";


                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + bszy + zyjcjg + ssjl + "]]></�ٴ���ʷ>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_sqd.Rows[0]["jbmc"].ToString().Trim() + "]]></�ٴ����>";
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
                        {
                            log.WriteMyLog(exp.Trim());
                            log.WriteMyLog(xml);
                        }

                        return xml;
                    }
                    catch(Exception  e2)
                    {
                        MessageBox.Show("�����쳣��" + e2.Message);
                        return "0";
                    }


                }
                else
                {
                    MessageBox.Show("�޴�ʶ��ţ�" + Sslbx1);
                    return "0";
                }


            }
            else
            {
                MessageBox.Show("ʶ��Ų���Ϊ�գ�" + Sslbx);
                return "0";
            }


        }
    }
}