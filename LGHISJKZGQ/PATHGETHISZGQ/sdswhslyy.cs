
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LGHISJKZGQ;
using System.Windows.Forms;
using dbbase;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    class sdswhslyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
          
            if (Sslbx != "")
            {
                string exp = "";

                if (Sslbx == "���뵥��"||Sslbx == "�걾�����")
                {
                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Provider=MSDAORA;Data Source=lisdb;User id=blinfo;Password=blinfo;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");
          
                    string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();

                    OleDbDB_ZGQ db = new OleDbDB_ZGQ();
                    DataTable dt_bblb = new DataTable();

                    string exp_db = "";
                    string sqxh = Ssbz.Trim();
                    if (Sslbx == "�걾�����")
                    {
                        string sqlstr_bblb = "select *  from VIEW_���������걾   WHERE  ����걾����= '" + Ssbz.Trim() + "'";

                        dt_bblb = db.OleDb_DataAdapter(odbcsql, sqlstr_bblb, ref exp_db);

                        if (dt_bblb.Rows.Count <= 0)
                        {
                            MessageBox.Show("δ��ѯ���˱걾�����¼��\r\n" + exp_db);
                            return "0";
                        }


                        sqxh = dt_bblb.Rows[0]["���뵥��"].ToString().Trim();
                    }


                    string sqlstr = "select *  from VIEW_�������뵥   WHERE  ���뵥��= " + sqxh.Trim() + "";
                    DataTable dt_SQD = new DataTable();
             
                    dt_SQD = db.OleDb_DataAdapter(odbcsql, sqlstr, ref exp_db);
                    if (exp_db.Trim() != "")
                    {
                        MessageBox.Show("����HIS���ݿ��쳣��" + exp_db);
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("δ��ѯ����������ż�¼��");
                        return "0";
                    }

                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_SQD.Rows[0]["���˱��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_SQD.Rows[0]["���뵥��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                        }
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";

                            xml = xml + "סԺ��=" + (char)34 + dt_SQD.Rows[0]["סԺ��"].ToString().Trim() + (char)34 + " ";
    
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["��������"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt_SQD.Rows[0]["�Ա�"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["����"].ToString().Trim()  + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + dt_SQD.Rows[0]["��ַ"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_SQD.Rows[0]["�绰"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
 
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["��ǰ����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                    
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt_SQD.Rows[0]["���֤����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                     
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                      
                        try
                        {
                            xml = xml + "ְҵ=" + (char)34 + dt_SQD.Rows[0]["ְҵ"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        }
                    
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_SQD.Rows[0]["�������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                       
                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_SQD.Rows[0]["����ҽ������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                 
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                      
                        string BBLB_XML = "";
                        if (tqbblb == "1")
                        {
                            string sqlstr_bblb = "select *  from VIEW_���������걾   WHERE  ���뵥��= " + sqxh.Trim() + "";
                            dt_bblb = db.OleDb_DataAdapter(odbcsql, sqlstr_bblb, ref exp_db);
                            if (dt_bblb.Rows.Count <= 0)
                            {
                                MessageBox.Show("δ��ѯ���˱걾�����¼��\r\n" + exp_db);
                            }
                            else
                            {
                                BBLB_XML = "<BBLB>";
                                try
                                {
                                    for (int x = 0; x < dt_bblb.Rows.Count; x++)
                                    {
                                        try
                                        {
                                            BBLB_XML = BBLB_XML + "<row ";
                                            BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + (x+1).ToString() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bblb.Rows[x]["����걾����"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bblb.Rows[x]["����걾����"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bblb.Rows[x]["�걾��ȡ��λ"].ToString().Trim() + (char)34 + " ";

                                            BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bblb.Rows[x]["�걾����ʱ��"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bblb.Rows[x]["�̶��걾ʱ��"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                            string bz = dt_bblb.Rows[x]["������ձ걾��־"].ToString().Trim();
                                            if (bz == "Y") bz = "����"; if (bz == "N") bz = "����"; if (bz == "") bz = "δ����";
                                            BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + bz + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "/>";
                                        }
                                        catch (Exception eee)
                                        {
                                            MessageBox.Show("��ȡ�걾�б���Ϣ�쳣��" + eee.Message);
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

                        }
                        xml = xml + "�걾����=" + (char)34 + dt_SQD.Rows[0]["�걾����"].ToString().Trim() + (char)34 + " ";
                      
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_SQD.Rows[0]["���뵥����"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        try
                        {
                            xml = xml + "����2=" + (char)34 + DateTime.Parse(dt_SQD.Rows[0]["��������"].ToString().Trim()).ToString("yyyy-MM-dd")+ (char)34 + " ";
                        }
                        catch(Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "����2=" + (char)34 +""+ (char)34 + " ";
                        }
                      
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + dt_SQD.Rows[0]["�ѱ�"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }

                            xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";

                        xml = xml + "/>";
      
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_SQD.Rows[0]["��ʷժҪ"].ToString().Trim() + "]]></�ٴ���ʷ>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
       
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_SQD.Rows[0]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
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
            MessageBox.Show("ʶ�����Ͳ���Ϊ��");
            return "0";
        }
   
    }     
}
