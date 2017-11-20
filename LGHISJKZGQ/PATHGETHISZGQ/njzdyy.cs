using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    /// <summary>
    /// �Ͼ��д�ҽԺ
    /// </summary>
    class njzdyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {
                string exp = "";
                if (Sslbx == "���뵥��" || Sslbx == "�����")
                {
                    DataTable dt = new DataTable();
                    string odbcsql= f.ReadString(Sslbx, "odbcsql", "DSN=pathnet-sqxh;UID=bljk;PWD=bljk");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string dbtype = f.ReadString(Sslbx, "dbtype", "ODBC");

                    string djr = f.ReadString("yh", "yhmc", "").Trim();

                    if (Sslbx == "�����")
                    {
                        DataTable dt_SQD = new DataTable();
                        string sqlstr_dt_SQD = "select *  from zdhis50.view_bl_blsqd  WHERE  �����= '" + Ssbz.Trim() + "'";
                       // string sqlstr_dt_SQD = "select *  from view_bl_blsqd  WHERE  �����= '" + Ssbz.Trim() + "'";
                        if (dbtype=="ORACLE")
                        dt_SQD = getXX_Oracle(odbcsql, sqlstr_dt_SQD);
                        else
                        dt_SQD = getXX_ODBC(odbcsql, sqlstr_dt_SQD);


                        if (dt_SQD.Rows.Count <= 0)
                        {
                            MessageBox.Show("δ��ѯ��������ż�¼��");
                            return "0";
                        }

                        Ssbz = dt_SQD.Rows[0]["�������"].ToString().Trim();
                    }

                    string sqlstr = "select *  from zdhis50.view_bl_blsqd  WHERE  �������= '" + Ssbz.Trim() + "'";
                   // string sqlstr = "select *  from view_bl_blsqd  WHERE  �������= '" + Ssbz.Trim() + "'";

                    if (dbtype == "ORACLE")
                        dt= getXX_Oracle(odbcsql, sqlstr);
                    else
                        dt = getXX_ODBC(odbcsql, sqlstr);

                      if (dt.Rows.Count <= 0)
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
                            xml = xml + "���˱��=" + (char)34 + dt.Rows[0]["���˱��"].ToString().Trim() + (char)34 + " ";
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
                            xml = xml + "�������=" + (char)34 + dt.Rows[0]["�������"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�����=" + (char)34 + dt.Rows[0]["�����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt.Rows[0]["סԺ��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt.Rows[0]["�Ա�"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + dt.Rows[0]["��ַ"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt.Rows[0]["�绰"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt.Rows[0]["���֤��"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "ְҵ=" + (char)34 + dt.Rows[0]["ְҵ"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["�ͼ����"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["�ͼ�ҽ��"].ToString().Trim() + (char)34 + " ";
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
                        BBLB_XML = "<BBLB>";
                        string bbmc="";
                        try
                        {
                            for (int x = 0; x < dt.Rows.Count; x++)
                            {


                                string[] bbmcs = dt.Rows[x]["�걾���ƺͲ�λ"].ToString().Trim().Split('|');
                                foreach (string bb in bbmcs)
                                {
                                    if (bb.Trim() != "" && bb.Trim() != ",")
                                    {
                                        if (bbmc.Trim() == "")
                                            bbmc = bb.Split(',')[0].Trim();
                                        else
                                            bbmc = bbmc + "��" + bb.Split(',')[0].Trim();
                                    }
                                }
                                if (tqbblb == "1")
                                {
                                    try
                                    {
                                        BBLB_XML = BBLB_XML + "<row ";
                                        BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + (x + 1).ToString() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt.Rows[x]["�����"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt.Rows[x]["�걾���ƺͲ�λ"].ToString().Trim() + "(" + dt.Rows[x]["�걾����"].ToString().Trim() + ")" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt.Rows[x]["�걾����ʱ��"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt.Rows[x]["�걾�̶�ʱ��"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "/>";
                                    }
                                    catch(Exception  eee)
                                    {
                                        MessageBox.Show("��ȡ�걾�б���Ϣ�쳣��" + eee.Message);
                                        tqbblb = "0";
                                    }

                                }

                            }
                        }
                        catch(Exception  e3)
                        {
                            MessageBox.Show("��ȡ�걾�����쳣��" + e3.Message);
                            tqbblb = "0";
                        }
                        BBLB_XML = BBLB_XML + "</BBLB>";


                      
                        xml = xml + "�걾����=" + (char)34 + bbmc + (char)34 + " ";
                        //----------------------------------------------------------
                        string sjdw = dt.Rows[0]["�ͼ쵥λ"].ToString().Trim();
                        if (sjdw.Trim() == "")
                            sjdw = "��Ժ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + sjdw + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt.Rows[0]["ҽ����Ŀ����"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + dt.Rows[0]["�ѱ�"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt.Rows[0]["�������"].ToString().Trim() + (char)34 + " ";
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
                            string lcbs = "";

                            if (dt.Rows[0]["��������"].ToString().Trim()!="")
                               lcbs="�������ƣ�" + dt.Rows[0]["��������"].ToString().Trim()  + "\r\n";

                           if (dt.Rows[0]["�ٴ���ʷ"].ToString().Trim() != "")
                           {
                               if (lcbs != "")
                                   lcbs = lcbs + "     ||";
                               lcbs = lcbs + "�ٴ���ʷ��" + dt.Rows[0]["�ٴ���ʷ"].ToString().Trim() + "\r\n";
                           }
                          
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + ""+lcbs + "]]></�ٴ���ʷ>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[0]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
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
                        log.WriteMyLog("xml��������---" + e.ToString());
                        return "0";
                    }
                }

               
            } return "0";


        }
        public static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "���˱��=" + (char)34 + (char)34 + " ";
            xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�������=" + (char)34 + (char)34 + " ";
            xml = xml + "�����=" + (char)34 + (char)34 + " ";
            xml = xml + "סԺ��=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";

            xml = xml + "�Ա�=" + (char)34 + (char)34 + " ";

            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "��ַ=" + (char)34 + (char)34 + "   ";
            xml = xml + "�绰=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            xml = xml + "���֤��=" + (char)34 + (char)34 + " ";
            xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "ְҵ=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽ��=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "�ͼ�ҽ��=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
            xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 +"��Ժ"+ (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            xml = xml + "�ѱ�=" + (char)34 + (char)34 + " ";

            xml = xml + "�������=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + "]]></�ٴ����>";
            xml = xml + "</LOGENE>";
            return xml;
        }

         

        public static DataTable getXX_ODBC(string con_str, string sql)
        {

            DataSet ds = new DataSet();
            OdbcConnection ocn = new OdbcConnection(con_str);
            try
            {
                OdbcDataAdapter dap = new OdbcDataAdapter(sql, ocn);
                ocn.Open();
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
            return ds.Tables[0];


        }

        public static DataTable getXX_Oracle(string con_str, string sql)
        {

            DataSet ds = new DataSet();
            OracleConnection ocn = new OracleConnection(con_str);
            try
            {
                OracleDataAdapter dap = new OracleDataAdapter(sql, ocn);
                ocn.Open();
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
            return ds.Tables[0];


        }
    }
}
