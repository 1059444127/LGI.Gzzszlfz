using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using dbbase;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    class jzykdxfsyy
    {
        //����ҽ�ƴ�ѧ����ҽԺ
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            string exp = "";
              string errMsg = "";
            if (Sslbx != "")
            {
                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                   debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim();
                   string odbc = f.ReadString(Sslbx, "odbc", "Provider=MSDAORA;Data Source=ORCL68;User id=chisdb_dev;Password=chisdb_dev;").Replace("\0", "").Trim();
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string ptjk = f.ReadString(Sslbx, "ptjk", "1");
                    if (Sslbx == "ȡ���ǼǱ��")
                    {
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                       int x= aa.ExecuteSQL("update T_SQD set F_SQDZT='ȡ���Ǽ�' where F_sqxh='" + Ssbz.Trim() + "'");
                       if (x > 0)
                           MessageBox.Show("ȡ���Ǽǳɹ�");
                       else
                           MessageBox.Show("ȡ���Ǽ�ʧ��");
                       return "0";

                    }

                    if (Sslbx == "�˷�")
                    {
                    //    string odbc = f.ReadString(Sslbx, "odbc", "Provider=MSDAORA;Data Source=ORCL68;User id=chisdb_dev;Password=chisdb_dev;").Replace("\0", "").Trim();
                        #region  �˷�
                        string yhgh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim(); ;

                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable jcxx = new DataTable();
                        try
                        {
                            jcxx = aa.GetDataTable("select * from T_JCXX where F_BLH='" + Ssbz + "'", "jcxx");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                            return "0";
                        }
                        if (jcxx == null)
                        {
                            MessageBox.Show("���ݿ������쳣");
                            return "0";
                        }
                        if (jcxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("ȡ����Ч��δ��ѯ����Ӧ���뵥��¼");
                            return "0";
                        }


                        string brlbbm = "";
                        if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "����")
                            brlbbm = "0";
                        if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "סԺ")
                            brlbbm = "1";
                        if (brlbbm == "")
                        {
                            MessageBox.Show("�������סԺ���ˣ������˷�");
                            return "0";
                        }

                        OleDbParameter[] ops = new OleDbParameter[5];
                        for (int j = 0; j < ops.Length; j++)
                        {
                            ops[j] = new OleDbParameter();
                        }
                        ops[0].ParameterName = "v_flag_mz_zy";
                        ops[0].OleDbType = OleDbType.VarChar;
                        ops[0].Direction = ParameterDirection.Input;
                        ops[0].Size = 20;
                        ops[0].Value = brlbbm;

                        ops[1].ParameterName = "v_patient_id";//
                        ops[1].OleDbType = OleDbType.VarChar;
                        ops[1].Direction = ParameterDirection.Input;
                        ops[1].Size = 20;
                        ops[1].Value = jcxx.Rows[0]["F_BRBH"].ToString();

                        ops[2].ParameterName = "v_page_no";//
                        ops[2].OleDbType = OleDbType.VarChar;
                        ops[2].Direction = ParameterDirection.Input;
                        ops[2].Size = 20;
                        ops[2].Value = jcxx.Rows[0]["F_SQXH"].ToString();

                        ops[3].ParameterName = "v_opera";//
                        ops[3].OleDbType = OleDbType.VarChar;
                        ops[3].Direction = ParameterDirection.Input;
                        ops[3].Size = 10;
                        ops[3].Value = yhgh;

                        ops[4].ParameterName = "v_RetError";//
                        ops[4].OleDbType = OleDbType.VarChar;
                        ops[4].Direction = ParameterDirection.Output;
                        ops[4].Size = 200;

                        //��д�Ǽ�״̬

                        OleDbDB_ZGQ oledb = new OleDbDB_ZGQ();
                        string message_ee = "";
                        oledb.OleDb_ExecuteNonQuery(odbc, "langjia_to_charge_bak", ref ops, CommandType.StoredProcedure, ref message_ee);

                        if (message_ee.Trim() != "")
                        {
                            MessageBox.Show("�˷ѱ��ʧ�ܣ���HIS�洢�����쳣--" + message_ee);
                        }
                        else
                        {
                            if (ops[4].Value.ToString() == "2")
                            {
                                 MessageBox.Show("�˷ѱ�ǳɹ�");
                                aa.ExecuteSQL("update t_jcxx set F_hisbj='0' where f_blh='" + Ssbz + "'");
                            }
                            else
                            {
                                MessageBox.Show("�˷ѱ��ʧ�ܣ�"+ ops[4].Value.ToString());
                            }
                           
                        }
                        return "0";
                        #endregion
                    }
                    if (Sslbx == "�����ID��")
                    {
                        #region
                        string sqlstr = "";
                     
                            sqlstr = "select * from view_langjia_apply_no WHERE �������='" + Ssbz + "'";

                        errMsg = "";
                        OleDbDB_ZGQ db = new OleDbDB_ZGQ();
                        DataTable dt_his_sqd = db.OleDb_DataAdapter(odbc, sqlstr, ref errMsg);
                        if (dt_his_sqd.Rows.Count <= 0)
                        {
                               sqlstr = "select * from view_langjia_apply_no WHERE HISID='" + Ssbz + "'  and  rownum<=20";
                               dt_his_sqd = db.OleDb_DataAdapter(odbc, sqlstr, ref errMsg);
                               if (dt_his_sqd.Rows.Count <= 0)
                                return GetBrxx(odbc, Sslbx, Ssbz);
                        
                        }
                       
                            int count = 0;
                            if (dt_his_sqd.Rows.Count > tkts)
                            {
                                string xsys = f.ReadString(Sslbx, "xsys", "1"); //ѡ����������Ŀ
                                DataColumn dc0 = new DataColumn("���");
                                dt_his_sqd.Columns.Add(dc0);
                                string Columns = f.ReadString(Sslbx, "Columns", "��������,�������,סԺ�������,��������,�����Ա�,��������,�ٴ������������,�ٴ�����ҽ������,��鲿λ,�ٴ����");
                                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "��������,���뵥��,סԺ�������,����,�Ա�,����,�ͼ����,�ͼ�ҽ��,��鲿λ,�ٴ����");
                                for (int x = 0; x < dt_his_sqd.Rows.Count; x++)
                                {
                                    dt_his_sqd.Rows[x][dt_his_sqd.Columns.Count - 1] = x;
                                }

                                if (Columns.Trim() != "")
                                    Columns = "���," + Columns;
                                if (ColumnsName.Trim() != "")
                                    ColumnsName = "���," + ColumnsName;

                                FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_his_sqd, Columns, ColumnsName, xsys);
                                yc.ShowDialog();
                                string rtn2 = yc.F_XH;
                                if (rtn2.Trim() == "")
                                {
                                    MessageBox.Show("δѡ��������Ŀ��");
                                    return GetBrxx(odbc, Sslbx, Ssbz);
                                }
                                try
                                {
                                    count = int.Parse(rtn2);
                                }
                                catch
                                {
                                    MessageBox.Show("������ѡ��������Ŀ��");
                                    return GetBrxx(odbc, Sslbx, Ssbz);
                                }
                            }

                            try
                            {
                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                xml = xml + "<LOGENE>";
                                xml = xml + "<row ";
                                string brlx = dt_his_sqd.Rows[count]["��������"].ToString().Trim();
                                if (brlx == "1") brlx = "����";
                                if (brlx == "2") brlx = "סԺ";
                                xml = xml + "���˱��=" + (char)34 + dt_his_sqd.Rows[count]["HISID"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�������=" + (char)34 + dt_his_sqd.Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                                if (brlx == "2")
                                {
                                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "סԺ��=" + (char)34 + dt_his_sqd.Rows[count]["סԺ�������"].ToString().Trim() + (char)34 + " ";
                                }
                                else
                                {
                                    xml = xml + "�����=" + (char)34 + dt_his_sqd.Rows[count]["סԺ�������"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                                }
                                xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["��������"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "�Ա�=" + (char)34 + dt_his_sqd.Rows[count]["�����Ա�"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["��������"].ToString().Trim() + dt_his_sqd.Rows[count]["���˵�λ"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "��ַ=" + (char)34 + dt_his_sqd.Rows[count]["��ϵ��ַ"].ToString().Trim() + (char)34 + "   ";
                                xml = xml + "�绰=" + (char)34 + dt_his_sqd.Rows[count]["��ϵ�绰"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "���֤��=" + (char)34 + dt_his_sqd.Rows[count]["���֤��"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�ͼ����=" + (char)34 + dt_his_sqd.Rows[count]["�ٴ������������"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_his_sqd.Rows[count]["�ٴ�����ҽ������"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                                xml = xml + "ҽ����Ŀ=" + (char)34 + dt_his_sqd.Rows[count]["��鲿λ"].ToString().Trim() + (char)34 + " ";
                                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                                xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                                xml = xml + "�������=" + (char)34 + brlx + (char)34 + " ";
                                xml = xml + "/>";
                                xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_his_sqd.Rows[count]["�ٴ�֢״"].ToString().Trim() + "|�س�|" + dt_his_sqd.Rows[count]["��ʷ������"].ToString().Trim() + "]]></�ٴ���ʷ>";
                                xml = xml + "<�ٴ����><![CDATA[" + dt_his_sqd.Rows[count]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
                                xml = xml + "</LOGENE>";
                                return xml;
                            }
                            catch (Exception ee1)
                            {
                                MessageBox.Show("��ȡHIS���뵥��Ϣ���������²���\r\n" + ee1.Message);
                                return GetBrxx(odbc, Sslbx, Ssbz);
                            }
                       
                        #endregion
                        
                    }
                    if (Sslbx == "סԺ��" || Sslbx == "�����" || Sslbx == "ID��" || Sslbx == "���뵥��")
                    {
                        #region 
                        string sqlstr = "";
                        if (ptjk == "1")
                        {
                            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                            if (Sslbx == "סԺ��")
                                sqlstr = "select * from  T_SQD  where F_ZYH='" + Ssbz.Trim() + "'  and F_sqdzt!='�ѵǼ�'";
           
                            if (Sslbx == "�����")
                            
                                sqlstr = "select * from  T_SQD  where F_MZH='" + Ssbz.Trim() + "'  and F_sqdzt!='�ѵǼ�'";
                            if (Sslbx == "ID��")
                                sqlstr = "select * from  T_SQD  where F_BRBH='" + Ssbz.Trim() + "'  and F_sqdzt!='�ѵǼ�'";
                            if (Sslbx == "���뵥��")
                                sqlstr = "select * from  T_SQD  where F_sqxh='JC" + Ssbz.Trim() + "'  and F_sqdzt!='�ѵǼ�'";
                            if (sqlstr == "")
                            {
                                MessageBox.Show("����Ĳ�ѯ���"); return "0";
                            }

                            DataTable dt_sqd = aa.GetDataTable(sqlstr, "dt_sqd");
                            if (dt_sqd.Rows.Count > 0)
                            {
                                int count = 0;
                                if (dt_sqd.Rows.Count > tkts)
                                {
                                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //ѡ����������Ŀ
                                    DataColumn dc0 = new DataColumn("���");
                                    dt_sqd.Columns.Add(dc0);

                                    string Columns = f.ReadString(Sslbx, "Columns", "F_brbh,F_SQXH,F_BRLB,F_XM,F_XB,F_NL,F_SQKS,F_SQYS,F_YZXM,F_BBMC");
                                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "ID��,���뵥��,�������,����,�Ա�,����,�ͼ����,�ͼ�ҽ��,ҽ����Ŀ,�걾����");

                                    for (int x = 0; x < dt_sqd.Rows.Count; x++)
                                    {
                                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                                    }

                                    if (Columns.Trim() != "")
                                        Columns = "���," + Columns;
                                    if (ColumnsName.Trim() != "")
                                        ColumnsName = "���," + ColumnsName;

                                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                                    yc.ShowDialog();
                                    string rtn2 = yc.F_XH;
                                    if (rtn2.Trim() == "")
                                    {
                                        MessageBox.Show("δѡ��������Ŀ��");
                                        return "0";
                                    }
                                    try
                                    {
                                        count = int.Parse(rtn2);
                                    }
                                    catch
                                    {
                                        MessageBox.Show("������ѡ��������Ŀ��");
                                        return "0";
                                    }
                                }

                                try
                                {
                                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                    xml = xml + "<LOGENE>";
                                    xml = xml + "<row ";
                                    xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[count]["F_brbh"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����ID=" + (char)34 + dt_sqd.Rows[count]["F_JZLSH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�������=" + (char)34 + dt_sqd.Rows[count]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�����=" + (char)34 + dt_sqd.Rows[count]["F_MZH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "סԺ��=" + (char)34 + dt_sqd.Rows[count]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["F_XM"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�Ա�=" + (char)34 + dt_sqd.Rows[count]["F_XB"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["F_NL"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["F_HY"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "��ַ=" + (char)34 + dt_sqd.Rows[count]["F_DZ"].ToString().Trim() + (char)34 + "   ";
                                    xml = xml + "�绰=" + (char)34 + dt_sqd.Rows[count]["F_DH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["F_BQ"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["F_CH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "���֤��=" + (char)34 + dt_sqd.Rows[count]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["F_JZCS"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "ְҵ=" + (char)34 + dt_sqd.Rows[count]["F_CSRQ"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�ͼ����=" + (char)34 + dt_sqd.Rows[count]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqd.Rows[count]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�걾����=" + (char)34 + dt_sqd.Rows[count]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                                    xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqd.Rows[count]["F_YZXM"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                                    xml = xml + "����2=" + (char)34 + (char)34 + " ";
                                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�������=" + (char)34 + dt_sqd.Rows[count]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "/>";
                                    xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_sqd.Rows[count]["F_LCBS"].ToString().Trim() + "|�س�|" + dt_sqd.Rows[count]["F_LCZL"].ToString().Trim() + "]]></�ٴ���ʷ>";
                                    xml = xml + "<�ٴ����><![CDATA[" + dt_sqd.Rows[count]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
                                    xml = xml + "</LOGENE>";
                                    return xml;
                                }
                                catch (Exception ee1)
                                {
                                    MessageBox.Show("��ȡ�������Ϣ���������²���\r\n" + ee1.Message);
                                }
                            }
                        }


                        if (Sslbx == "סԺ��")
                         sqlstr = "select * from view_langjia_apply_no WHERE סԺ�������='" + Ssbz + "'  and ��������='2' and  rownum<20";
                        
                        if (Sslbx == "�����")
                        sqlstr = "select * from view_langjia_apply_no WHERE סԺ�������='" + Ssbz + "'  and ��������='1' and  rownum<20";
                       
                        if (Sslbx == "ID��")
                         sqlstr = "select * from view_langjia_apply_no WHERE HISID='" + Ssbz + "'  and  rownum<50";
                        if (Sslbx == "���뵥��")
                            sqlstr = "select * from view_langjia_apply_no WHERE �������='" + Ssbz + "'";

                        if (sqlstr == "")
                        {
                            MessageBox.Show("����Ĳ�ѯ���2"); return "0";
                        }

                            errMsg = "";
                            OleDbDB_ZGQ db = new OleDbDB_ZGQ();
                            DataTable dt_his_sqd = db.OleDb_DataAdapter(odbc, sqlstr, ref errMsg);
                            if (dt_his_sqd.Rows.Count > 0)
                            {
                                int count = 0;
                                if (dt_his_sqd.Rows.Count > tkts)
                                {
                                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //ѡ����������Ŀ
                                    DataColumn dc0 = new DataColumn("���");
                                    dt_his_sqd.Columns.Add(dc0);

                                    string Columns = f.ReadString(Sslbx, "Columns", "��������,�������,סԺ�������,��������,�����Ա�,��������,�ٴ������������,�ٴ�����ҽ������,��鲿λ,�ٴ����");
                                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "��������,���뵥��,סԺ�������,����,�Ա�,����,�ͼ����,�ͼ�ҽ��,��鲿λ,�ٴ����");

                                    for (int x = 0; x < dt_his_sqd.Rows.Count; x++)
                                    {
                                        dt_his_sqd.Rows[x][dt_his_sqd.Columns.Count - 1] = x;
                                    }

                                    if (Columns.Trim() != "")
                                        Columns = "���," + Columns;
                                    if (ColumnsName.Trim() != "")
                                        ColumnsName = "���," + ColumnsName;

                                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_his_sqd, Columns, ColumnsName, xsys);
                                    yc.ShowDialog();
                                    string rtn2 = yc.F_XH;
                                    if (rtn2.Trim() == "")
                                    {
                                        MessageBox.Show("δѡ��������Ŀ��");
                                        return GetBrxx(odbc, Sslbx, Ssbz);
                                    }
                                    try
                                    {
                                        count = int.Parse(rtn2);
                                    }
                                    catch
                                    {
                                        MessageBox.Show("������ѡ��������Ŀ��");
                                        return GetBrxx(odbc, Sslbx, Ssbz);
                                    }
                                }

                                try
                                {
                                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                                    xml = xml + "<LOGENE>";
                                    xml = xml + "<row ";
                                    string brlx = dt_his_sqd.Rows[count]["��������"].ToString().Trim();
                                    if (brlx == "1") brlx = "����";
                                    if (brlx == "2") brlx = "סԺ";
                                    xml = xml + "���˱��=" + (char)34 + dt_his_sqd.Rows[count]["HISID"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�������=" + (char)34 + dt_his_sqd.Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                                    if (brlx == "2")
                                    {
                                        xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                                        xml = xml + "סԺ��=" + (char)34 + dt_his_sqd.Rows[count]["סԺ�������"].ToString().Trim() + (char)34 + " ";
                                    }
                                    else
                                    {
                                        xml = xml + "�����=" + (char)34 + dt_his_sqd.Rows[count]["סԺ�������"].ToString().Trim() + (char)34 + " ";
                                        xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                                    }
                                    xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["��������"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�Ա�=" + (char)34 + dt_his_sqd.Rows[count]["�����Ա�"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["��������"].ToString().Trim() + dt_his_sqd.Rows[count]["���˵�λ"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "��ַ=" + (char)34 + dt_his_sqd.Rows[count]["��ϵ��ַ"].ToString().Trim() + (char)34 + "   ";
                                    xml = xml + "�绰=" + (char)34 + dt_his_sqd.Rows[count]["��ϵ�绰"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "���֤��=" + (char)34 + dt_his_sqd.Rows[count]["���֤��"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����=" + (char)34 + dt_his_sqd.Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�ͼ����=" + (char)34 + dt_his_sqd.Rows[count]["�ٴ������������"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_his_sqd.Rows[count]["�ٴ�����ҽ������"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                                    xml = xml + "ҽ����Ŀ=" + (char)34 + dt_his_sqd.Rows[count]["��鲿λ"].ToString().Trim() + (char)34 + " ";
                                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                                    xml = xml + "����2=" + (char)34 + (char)34 + " ";
                                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                                    xml = xml + "�������=" + (char)34 + brlx + (char)34 + " ";
                                    xml = xml + "/>";
                                    xml = xml + "<�ٴ���ʷ><![CDATA[" + dt_his_sqd.Rows[count]["�ٴ�֢״"].ToString().Trim() + "|�س�|" + dt_his_sqd.Rows[count]["��ʷ������"].ToString().Trim() + "]]></�ٴ���ʷ>";
                                    xml = xml + "<�ٴ����><![CDATA[" + dt_his_sqd.Rows[count]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
                                    xml = xml + "</LOGENE>";
                                    return xml;
                                }
                                catch (Exception ee1)
                                {
                                    MessageBox.Show("��ȡHIS���뵥��Ϣ���������²���\r\n" + ee1.Message);
                                    return GetBrxx(odbc, Sslbx, Ssbz);
                                }
                            }
                            else
                                return GetBrxx(odbc, Sslbx, Ssbz);
                      
                        #endregion
                    }
                    else
                    {
                        MessageBox.Show(Sslbx + "����δ���ã�");
                        return "0";
                    }
            }
            else
                MessageBox.Show("");
            return "0";
        }

        public static string GetBrxx(string odbc, string brlb, string ssbz)
          {

           
              string errMsg = "";
               OleDbDB_ZGQ db = new OleDbDB_ZGQ();
              string  sql="";

               if(brlb=="סԺ��")
                  sql= "select  * from view_langjia_patient  WHERE סԺ��='" + ssbz + "' and rownum<10 ";
               if (brlb == "�����")
                  sql= "select  * from view_langjia_patient  WHERE �����='" + ssbz + "' and rownum<10 ";
                if(brlb=="ID��")
                  sql= "select  * from view_langjia_patient  WHERE ���˱��='" + ssbz + "' and rownum<10 ";


                if (sql == "")
                {
                    MessageBox.Show("��ȡ��Ϣʧ��"); return "0";
                }
                DataTable dt_his = db.OleDb_DataAdapter(odbc,sql, ref errMsg);
                if (dt_his.Rows.Count > 0)
                {
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_his.Rows[0]["���˱��"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                         if(brlb=="סԺ��")
                            xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                         else
                              xml = xml + "�����=" + (char)34 +dt_his.Rows[0]["�����"].ToString().Trim() + (char)34 + " ";

                           if(brlb=="סԺ��")
                            xml = xml + "סԺ��=" + (char)34 + dt_his.Rows[0]["סԺ��"].ToString().Trim() + (char)34 + " ";
                           else
                             xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + dt_his.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "�Ա�=" + (char)34 + dt_his.Rows[0]["�Ա�"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + dt_his.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + dt_his.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "��ַ=" + (char)34 + dt_his.Rows[0]["��ַ"].ToString().Trim() + (char)34 + "   ";
                            xml = xml + "�绰=" + (char)34 + dt_his.Rows[0]["�绰"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "����=" + (char)34 + dt_his.Rows[0]["����"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "���֤��=" + (char)34 + dt_his.Rows[0]["���֤��"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_his.Rows[0]["�ͼ����"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_his.Rows[0]["�ͼ�ҽ��"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + dt_his.Rows[0]["�ѱ�"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_his.Rows[0]["enc_type"].ToString().Trim() +(char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_his.Rows[0]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        return xml;
                    }
                    catch (Exception ee1)
                    {
                        MessageBox.Show("��ȡHIS��Ϣ���������²���\r\n" + ee1.Message);
                        return "0";
                    }
                }
                else
                MessageBox.Show("δ��ѯ���˺����¼"); return "0";
            }


     

    }
}
