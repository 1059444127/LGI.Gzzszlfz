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
    class nbxsrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            if (Sslbx != "")
            {
                string exp = "";

                if (Sslbx == "�����˷�")
                {
                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + Ssbz + "'", "bljc");


                    if (bljc == null)
                    {
                        MessageBox.Show("����ţ�" + Ssbz + ",�������ݿ����������⣡");
                        return "0";
                    }
                    if (bljc.Rows.Count < 1)
                    {
                        MessageBox.Show("����ţ�" + Ssbz + ",������д���");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
                    {
                        MessageBox.Show("�������Ϊ�գ�����ȡ���˷�");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "����")
                    {
                        MessageBox.Show("�����ﲡ��,����ȡ���˷�");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_mzqf"].ToString().Trim() != "1")
                    {
                        MessageBox.Show("δȷ�ѻ�Ǳ�ϵͳȷ�ѣ�����ȡ���˷�");
                        return "0";
                    }
                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "����" && bljc.Rows[0]["F_mzqf"].ToString().Trim() != "1")
                    {
                        SqlParameter[] Parameters = new SqlParameter[5];

                        for (int j = 0; j < Parameters.Length; j++)
                        {
                            Parameters[j] = new SqlParameter();
                        }
                        Parameters[0].ParameterName = "sqdh";
                        Parameters[0].SqlDbType = SqlDbType.VarChar;
                        Parameters[0].Direction = ParameterDirection.Input;
                        Parameters[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                        Parameters[1].ParameterName = "czgh";
                        Parameters[1].SqlDbType = SqlDbType.VarChar;
                        Parameters[1].Direction = ParameterDirection.Input;
                        Parameters[1].Value = yhbh;

                        Parameters[2].ParameterName = "bz";
                        Parameters[2].SqlDbType = SqlDbType.Int;
                        Parameters[2].Direction = ParameterDirection.Input;
                        Parameters[2].Value = 9;

                        Parameters[3].ParameterName = "zxysh";
                        Parameters[3].SqlDbType = SqlDbType.VarChar;
                        Parameters[3].Direction = ParameterDirection.Input;
                        Parameters[3].Value = yhmc;

                        Parameters[4].ParameterName = "return";
                        Parameters[4].SqlDbType = SqlDbType.Int;
                        Parameters[4].Direction = ParameterDirection.ReturnValue;

                        string except = "";
                        db.Sql_ExecuteNonQuery(odbcsql, "p_yjqr", ref Parameters, CommandType.StoredProcedure, ref except);
                        if (except != "")
                            MessageBox.Show("�������ݿ�ʧ�ܣ����ﲡ���˷�ʧ��");
                        else
                        {
                            try
                            {
                                if (int.Parse(Parameters[4].Value.ToString()) < 0)
                                {
                                    MessageBox.Show("���ﲡ���˷�ʧ��:" + Parameters[4].Value);
                                }
                                else
                                {
                                    aa.ExecuteSQL("update T_JCXX set F_MZQF='0' where F_BLH='" + Ssbz + "'");
                                    MessageBox.Show("���ﲡ���˷ѳɹ�");
                                }
                            }
                            catch (Exception ee1)
                            {
                                MessageBox.Show("�����˷��쳣��" + ee1.Message);
                            }
                        }
                        return "0";
                    } return "0";
                }
                if (Sslbx == "���뵥��"||Sslbx == "�걾�����")
                {
                    string sqxh=Ssbz.Trim();
                    if (Sslbx == "�걾�����")
                        sqxh = Ssbz.Trim().Split('_')[0].Trim();

                    string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string debug = f.ReadString(Sslbx, "debug", "0");
          
                    string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string sqlstr = "select *  from dbo.V_pa_apply   WHERE  apply_id= '" + sqxh.Trim() + "'";
                    DataTable dt_SQD = new DataTable();
                    string exp_db = "";
                    dt_SQD = db.Sql_DataAdapter(odbcsql, sqlstr, ref exp_db);
                    if (exp_db.Trim() != "")
                        MessageBox.Show("����HIS���ݿ��쳣��" + exp_db);

                    if (dt_SQD == null)
                    {
                        MessageBox.Show("δ��ѯ��������ż�¼��");
                        return "0";
                    }

                    if (dt_SQD.Rows.Count <= 0)
                    {
                        MessageBox.Show("δ��ѯ����������ż�¼��");
                        return "0";
                    }

                    //ȡ�շ�je




                    //-����xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "���˱��=" + (char)34 + dt_SQD.Rows[0]["PATIENT_ID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�������=" + (char)34 + dt_SQD.Rows[0]["APPLY_ID"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                    
                            xml = xml + "�����=" + (char)34 + dt_SQD.Rows[0]["INPATIENT_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "סԺ��=" + (char)34 + dt_SQD.Rows[0]["PATIENT_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PATIENT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�Ա�=" + (char)34 + dt_SQD.Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["AGE"].ToString().Trim() + "��" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["HY"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "��ַ=" + (char)34 + dt_SQD.Rows[0]["ADDRESS"].ToString().Trim() + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�绰=" + (char)34 + dt_SQD.Rows[0]["TELPHONE"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PATIENT_DEPT"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["PATIENT_BED"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "���֤��=" + (char)34 + dt_SQD.Rows[0]["SFZH"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����=" + (char)34 + dt_SQD.Rows[0]["MZ"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "ְҵ=" + (char)34 + dt_SQD.Rows[0]["ZY"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ͼ����=" + (char)34 + dt_SQD.Rows[0]["APPLY_DEPT_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_SQD.Rows[0]["APPLY_DOCTOR_NAME"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                       double  je=0;

                        string  brlb=dt_SQD.Rows[0]["PATIENT_TYPE"].ToString().Trim();
                        if (brlb == "����" || brlb=="סԺ")
                           je= get_je(odbcsql, brlb, dt_SQD.Rows[0]["APPLY_ID"].ToString().Trim(), debug);
                        xml = xml + "�շ�=" + (char)34 + je.ToString() + (char)34 + " ";
                        //----------------------------------------------------------
                        ///////////////////////////
                        string BBLB_XML = "";
                        if (tqbblb == "1")
                        {
                            string sqlstr_mx = "select *  from dbo.V_pa_apply_mx   WHERE  APPLY_ID= '" + sqxh.Trim() + "'";
                            DataTable dt_SQD_MX = new DataTable();
                            string exp_db_mx = "";
                            dt_SQD_MX = db.Sql_DataAdapter(odbcsql, sqlstr_mx, ref exp_db_mx);
                            if (exp_db_mx.Trim() != "")
                                MessageBox.Show("����HIS���ݿ��쳣��" + exp_db_mx);
                            else
                            {
                                if (dt_SQD_MX == null)
                                {
                                    MessageBox.Show("δ��ѯ��������ż�¼��");
                                }
                                else
                                {

                                    if (dt_SQD_MX.Rows.Count <= 0)
                                    {
                                        MessageBox.Show("δ��ѯ����������ż�¼��");
                                    }
                                    else
                                    {
                                        BBLB_XML = "<BBLB>";
                                        try
                                        {
                                            for (int x = 0; x < dt_SQD_MX.Rows.Count; x++)
                                            {
                                                try
                                                {
                                                    BBLB_XML = BBLB_XML + "<row ";
                                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_SQD_MX.Rows[x]["BBXH"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_SQD_MX.Rows[x]["APPLY_BARCODE"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_SQD_MX.Rows[x]["BBMC"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_SQD_MX.Rows[x]["BWMC"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_SQD_MX.Rows[x]["LTSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_SQD_MX.Rows[x]["GDSJ"].ToString().Trim() + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
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
                            }
                        }
                        xml = xml + "�걾����=" + (char)34 + dt_SQD.Rows[0]["BBMC"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_SQD.Rows[0]["ORDER_NAME"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "����2=" + (char)34 + DateTime.Parse(dt_SQD.Rows[0]["csrq"].ToString().Trim()).ToString("yyyy-MM-dd")+ (char)34 + " ";
                        }
                        catch(Exception ee4)
                        {
                            log.WriteMyLog(ee4.Message);
                            xml = xml + "����2=" + (char)34 +""+ (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "�ѱ�=" + (char)34 + dt_SQD.Rows[0]["FEE_CLASS"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";//.Replace("\"", "&quot;")
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<�ٴ����><![CDATA[" + dt_SQD.Rows[0]["DIAGNOSIS"].ToString().Trim() + "]]></�ٴ����>";
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
         return "0";
        }
        private static double get_je(string odbcsql, string brlb,string sqdh, string debug)
        {
              SqlDB_ZGQ db = new SqlDB_ZGQ();
                SqlParameter[] Parameters = new SqlParameter[2];

                for (int j = 0; j < Parameters.Length; j++)
                {
                    Parameters[j] = new SqlParameter();
                }
                Parameters[0].ParameterName = "sqdh";
                Parameters[0].SqlDbType = SqlDbType.VarChar;
                Parameters[0].Direction = ParameterDirection.Input;
                Parameters[0].Value = sqdh.Trim();

                Parameters[1].ParameterName = "return";
                Parameters[1].SqlDbType = SqlDbType.Int;
                Parameters[1].Direction = ParameterDirection.Output;


                string except = "";
            DataTable  dt=new DataTable();
            if (brlb.Trim()=="סԺ")
                  dt = db.Sql_DataAdapter(odbcsql, "p_pa_zysfxx", ref Parameters, CommandType.StoredProcedure, ref except);
            else
                  dt = db.Sql_DataAdapter(odbcsql, "p_pa_mzsfxx", ref Parameters,CommandType.StoredProcedure, ref except);
      
                  
               if(dt==null)
               {
                     if(debug=="1")
                         MessageBox.Show("���û�ȡʧ��:��ȡ����ʧ��\r\n"  +except);
                   return 0;
               }
            if(dt.Rows.Count<=0)
            {
                  if(debug=="1")
                      MessageBox.Show("���û�ȡʧ��:��������Ϊ0\r\n" + except);
                 return 0;
            }
                
                    try
                    {
                        if (int.Parse(Parameters[1].Value.ToString()) < 0)
                        {
                              if(debug=="1")
                            MessageBox.Show("���û�ȡʧ��:" + Parameters[4].Value);
                           log.WriteMyLog("���û�ȡʧ��:" + Parameters[4].Value);
                             return 0;
                        }
                       double  je=0;
                       for(int x=0;x<dt.Rows.Count;x++)
                       {
                           je=double.Parse(dt.Rows[x]["je"].ToString().Trim())+je;
                       }

                        return  je;

                    }
                    catch (Exception ee1)
                    {
                        if(debug=="1")
                            MessageBox.Show("���û�ȡ�쳣:" + ee1.Message);
                        log.WriteMyLog("���û�ȡ�쳣:" + ee1.Message);
                         return 0;
                    }
                }

    }     
}
