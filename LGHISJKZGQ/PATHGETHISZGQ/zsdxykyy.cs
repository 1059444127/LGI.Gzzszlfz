using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.Odbc;
using dbbase;
using System.Xml;
using System.IO;

namespace LGHISJKZGQ
{
    class zsdxykyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            string exp = "";

            if (Sslbx != "")
            {
                string hissql = f.ReadString(Sslbx, "hissql", "").Replace("\0", "").Trim();
                string ptjk = f.ReadString("zgqjk", "ptjk", "1").Replace("\0", "").Trim();

                if (Sslbx == "ȡ���Ǽ�")
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    int x = aa.ExecuteSQL("update T_SQD set F_SQDZT='ȡ���Ǽ�' where F_sqxh='" + Ssbz.Trim() + "'");
                    if (x > 0)
                        MessageBox.Show("ȡ���Ǽǳɹ�");
                    else
                        MessageBox.Show("ȡ���Ǽ�ʧ��");
                    return "0";
                }
                if (Sslbx == "ȡ�����뵥")
                {
                    string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                    string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                     ZtToHis(Sslbx, Ssbz.Trim(), yhmc, yhbh, debug);
                     dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                     DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + Ssbz.Trim() + "'", "t_sqd");
                    if (dt_sqd.Rows.Count < 1)
                    {
                        MessageBox.Show("ƽ̨�������뵥ʧ��:�������ݿ�����޴����뵥��¼");
                        return  "0";
                    }
                     string errmsg = "";
                     string message = ZtMsg(dt_sqd, ref errmsg, yhmc, yhbh);
                     if (message == "")
                     {
                         return "0";
                     }

                     if (debug == "1")
                         log.WriteMyLog("ȡ�����뵥MQ״̬�ش���" + message);
                     string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
                     string wsurl = f.ReadString(Sslbx, "wsurl", "http://172.168.35.249/BLToMQWebSrv/FZMQService.asmx").Replace("\0", "").Trim();
                     try
                     {
                         BLToFZMQWS.Service fzmq = new BLToFZMQWS.Service();
                         if (wsurl != "")
                             fzmq.Url = wsurl;

                         string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS004.LQ", "BS004", jzlb, "0", "S009", "45541605-3", "70500", "0");

                         if (rtnmsg.Contains("ERR"))
                         {
                           MessageBox.Show("ƽ̨ȡ�����뵥ʧ�ܣ�" + rtnmsg);
                             return "0";
                         }

                     }
                     catch (Exception ee2)
                     {
                         MessageBox.Show("ƽ̨ȡ�����뵥�쳣:" + ee2.Message);
                         return "0";
                     }  
                    int x = aa.ExecuteSQL("delete  from T_SQD  where F_sqxh='" + Ssbz.Trim() + "'");
                    return "0";
                }

                if (ptjk != "1")
                {
          
                    return GetHisXX(Sslbx, Ssbz, debug);
                }

                if (Sslbx == "סԺ��")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as ���˱��,F_BRLB  as �������,F_FB as �ѱ�,F_ZYH as סԺ��,F_MZH as �����,F_XM as ���� ,F_XB as �Ա�,F_NL as ����,F_HY as ����,'' as ��ַ,F_DH as �绰,F_BQ AS ����,F_CH as ����,F_SFZH as ���֤��,F_MZ as ����,F_ZY as ְҵ,F_sqks as �ͼ����,F_SQYS AS �ͼ�ҽ��,F_LCZD as �ٴ����,F_LCZL as �ٴ���ʷ,' ' as �շ�,F_YZH as ����ID,F_SQXH as �������,F_BBMC as �걾����,'��Ժ' AS �ͼ�ҽԺ,F_YZXM as ҽ����Ŀ,F_CSRQ as ��������,F_BY1 as ����1,F_SQYSBM as ����2,F_SQRQ as �������� from [pathnet].[dbo].[T_SQD] WHERE  F_ZYH='f_sbh' and F_sqdzt!='�ѵǼ�' order by F_ID desc";
                }
                if (Sslbx == "������ˮ��")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as ���˱��,F_BRLB  as �������,F_FB as �ѱ�,F_ZYH as סԺ��,F_MZH as �����,F_XM as ���� ,F_XB as �Ա�,F_NL as ����,F_HY as ����,'' as ��ַ,F_DH as �绰,F_BQ AS ����,F_CH as ����,F_SFZH as ���֤��,F_MZ as ����,F_ZY as ְҵ,F_sqks as �ͼ����,F_SQYS AS �ͼ�ҽ��,F_LCZD as �ٴ����,F_LCZL as �ٴ���ʷ,' ' as �շ�,F_YZH as ����ID,F_SQXH as �������,F_BBMC as �걾����,'��Ժ' AS �ͼ�ҽԺ,F_YZXM as ҽ����Ŀ,F_CSRQ as ��������,F_BY1 as ����1,F_SQYSBM as ����2,F_SQRQ as �������� from [pathnet].[dbo].[T_SQD] WHERE  F_JZLSH='f_sbh' and F_sqdzt!='�ѵǼ�' order by F_ID desc";
                }
                if (Sslbx == "���ƿ�")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as ���˱��,F_BRLB  as �������,F_FB as �ѱ�,F_ZYH as סԺ��,F_MZH as �����,F_XM as ���� ,F_XB as �Ա�,F_NL as ����,F_HY as ����,'' as ��ַ,F_DH as �绰,F_BQ AS ����,F_CH as ����,F_SFZH as ���֤��,F_MZ as ����,F_ZY as ְҵ,F_sqks as �ͼ����,F_SQYS AS �ͼ�ҽ��,F_LCZD as �ٴ����,F_LCZL as �ٴ���ʷ,' ' as �շ�,F_YZH as ����ID,F_SQXH as �������,F_BBMC as �걾����,'��Ժ' AS �ͼ�ҽԺ,F_YZXM as ҽ����Ŀ,F_CSRQ as ��������,F_BY1 as ����1,F_SQYSBM as ����2,F_SQRQ as �������� from [pathnet].[dbo].[T_SQD] WHERE  F_MZH='f_sbh' and F_sqdzt!='�ѵǼ�' order by F_ID desc";
                }
                if (Sslbx == "���뵥��")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as ���˱��,F_BRLB  as �������,F_FB as �ѱ�,F_ZYH as סԺ��,F_MZH as �����,F_XM as ���� ,F_XB as �Ա�,F_NL as ����,F_HY as ����,'' as ��ַ,F_DH as �绰,F_BQ AS ����,F_CH as ����,F_SFZH as ���֤��,F_MZ as ����,F_ZY as ְҵ,F_sqks as �ͼ����,F_SQYS AS �ͼ�ҽ��,F_LCZD as �ٴ����,F_LCZL as �ٴ���ʷ,' ' as �շ�,F_YZH as ����ID,F_SQXH as �������,F_BBMC as �걾����,'��Ժ' AS �ͼ�ҽԺ,F_YZXM as ҽ����Ŀ,F_CSRQ as ��������,F_BY1 as ����1,F_SQYSBM as ����2,F_SQRQ as �������� from [pathnet].[dbo].[T_SQD] WHERE  F_SQXH='f_sbh' and F_sqdzt!='�ѵǼ�' order by F_ID desc";
                }
                if (Sslbx == "ҽ����")
                {
                    if (hissql == "")
                        hissql = "select F_brbh as ���˱��,F_BRLB  as �������,F_FB as �ѱ�,F_ZYH as סԺ��,F_MZH as �����,F_XM as ���� ,F_XB as �Ա�,F_NL as ����,F_HY as ����,'' as ��ַ,F_DH as �绰,F_BQ AS ����,F_CH as ����,F_SFZH as ���֤��,F_MZ as ����,F_ZY as ְҵ,F_sqks as �ͼ����,F_SQYS AS �ͼ�ҽ��,F_LCZD as �ٴ����,F_LCZL as �ٴ���ʷ,' ' as �շ�,F_YZH as ����ID,F_SQXH as �������,F_BBMC as �걾����,'��Ժ' AS �ͼ�ҽԺ,F_YZXM as ҽ����Ŀ,F_CSRQ as ��������,F_BY1 as ����1,F_SQYSBM as ����2,F_SQRQ as �������� from [pathnet].[dbo].[T_SQD] WHERE  F_YZH='f_sbh' and F_sqdzt!='�ѵǼ�' order by F_ID desc";
                }
           

                string Columns = f.ReadString(Sslbx, "Columns", "��������,�������,����ID,����,�Ա�,����,�ͼ����,�ͼ�ҽ��,����,����,ҽ����Ŀ");
                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "��������,�������,ҽ����,����,�Ա�,����,�ͼ����,�ͼ�ҽ��,����,����,ҽ����Ŀ");
                hissql = hissql.Replace("f_sbh", Ssbz.Trim());
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;");
                if (hissql.Trim() == "")
                {
            
                    return GetHisXX(Sslbx, Ssbz, debug);
                }
                string exec = "";
                DataSet ds = new DataSet();
                OdbcDB_ZGQ odbc = new OdbcDB_ZGQ();
                ds = odbc.Odbc_DataAdapter_DataSet(odbcsql, hissql, ref exec);
                if (exec != "")
                {
                    MessageBox.Show(exec);
                    return GetHisXX(Sslbx, Ssbz, debug);
                }
                /////////////////////////////////////////////////////////////////////
                if (ds.Tables[0] == null)
                {
                    MessageBox.Show("��ȡ���뵥��Ϣ���󣺲�ѯ���ݴ���");
                    return GetHisXX(Sslbx, Ssbz, debug);
                }
                if (ds.Tables[0].Rows.Count <= 0)
                {
                
                    //string odbcstr = f.ReadString(Sslbx, "odbc", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();

                    //if (Sslbx == "סԺ��")
                    //    return GetBrxx(2, Ssbz, 0, 0, "", debug, odbcstr, "0");
                    //if (Sslbx == "������ˮ��")
                    //{
                    //    int mzlsh = 0;
                    //    try
                    //    {
                    //        mzlsh = int.Parse(Ssbz);
                    //    }
                    //    catch
                    //    {
                    //        MessageBox.Show("������ˮ�Ÿ�ʽ����ȷ");
                    //        return "0";
                    //    }
                    //    return GetBrxx(1, "", mzlsh, 0, "", debug, odbcstr, "0");
                    //}
                    //if (Sslbx == "���ƿ�")
                    //    return GetBrxx(5, "", 0, 0, Ssbz, debug, odbcstr, "0");

                    //MessageBox.Show("δ��ȡ�������Ϣ");
                    //return "0";
                    return GetHisXX(Sslbx, Ssbz, debug);
                }

       
                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                int count = 0;
                if (ds.Tables[0].Rows.Count > tkts)
                {
                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //ѡ����������Ŀ
                    DataColumn dc0 = new DataColumn("���");
                    ds.Tables[0].Columns.Add(dc0);

                    for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                    {
                        ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                    }

                    if (Columns.Trim() != "")
                        Columns = "���," + Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "���," + ColumnsName;

                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(ds.Tables[0], Columns, ColumnsName, xsys);
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
                    xml = xml + "���˱��=" + (char)34 + ds.Tables[0].Rows[count]["���˱��"].ToString() + (char)34 + " ";
                    xml = xml + "����ID=" + (char)34 + ds.Tables[0].Rows[count]["����ID"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�����=" + (char)34 + ds.Tables[0].Rows[count]["�����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "סԺ��=" + (char)34 + ds.Tables[0].Rows[count]["סԺ��"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                    string xb = ds.Tables[0].Rows[count]["�Ա�"].ToString().Trim();
                    if (xb == "F" || xb == "f")
                        xb = "Ů";
                    if (xb == "M" || xb == "m")
                        xb = "��";
                    xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";

                    xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "��ַ=" + (char)34 +"" + (char)34 + "   ";
                    xml = xml + "�绰=" + (char)34 + ds.Tables[0].Rows[count]["�绰"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "���֤��=" + (char)34 + ds.Tables[0].Rows[count]["���֤��"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����=" + (char)34 + ds.Tables[0].Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "ְҵ=" + (char)34 + ds.Tables[0].Rows[count]["ְҵ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ͼ����=" + (char)34 + ds.Tables[0].Rows[count]["�ͼ����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + ds.Tables[0].Rows[count]["�ͼ�ҽ��"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�շ�=" + (char)34 + ds.Tables[0].Rows[count]["�շ�"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�걾����=" + (char)34 + ds.Tables[0].Rows[count]["�걾����"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + ds.Tables[0].Rows[count]["�ͼ�ҽԺ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "ҽ����Ŀ=" + (char)34 + ds.Tables[0].Rows[count]["ҽ����Ŀ"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����1=" + (char)34 + ds.Tables[0].Rows[count]["����1"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����2=" + (char)34 + ds.Tables[0].Rows[count]["����2"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�ѱ�=" + (char)34 + ds.Tables[0].Rows[count]["�ѱ�"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "�������=" + (char)34 + ds.Tables[0].Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + ""+ds.Tables[0].Rows[count]["�ٴ���ʷ"].ToString().Trim() + "]]></�ٴ���ʷ>";
                    xml = xml + "<�ٴ����><![CDATA[" + ds.Tables[0].Rows[count]["�ٴ����"].ToString().Trim() + "]]></�ٴ����>";
                    xml = xml + "/>";
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("��ȡ���뵥��Ϣ�쳣��" + e.Message);
                    return "0";
                }
            }
            else
            {
                MessageBox.Show("ʶ�����Ͳ���Ϊ��");
                return "0";
            }
        }
                
    public static string GetHisXX(string Sslbx, string Ssbz, string debug)
      {
       
          ////  1-��������ˮ�Ų�ѯ
////2-��סԺ�Ų�ѯ
////3-����������ʱ������뼼����Ҳ�ѯ
////4-���걾�����ѯ
////5-�����ƿ��Ų�ѯ
////6-�����뵥�Ų�ѯ

//��ʽ����IP192.168.171.138  srvhisdb
//���Ի���IP 192.168.171.50 svrdb
      
                    string odbcstr = f.ReadString(Sslbx, "odbc_his", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();
                    debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim(); ;
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");

                    if (Sslbx == "סԺ��")
                    {
                        string rtn = GetSqdxx(2, Ssbz, 0, 0, "", 0, debug, odbcstr, tqbblb);
                        if (rtn == "0")
                            return GetBrxx(2, Ssbz, 0, 0, "", debug, odbcstr, "0");
                          
                        else
                            return rtn;
                    }

                    if (Sslbx == "������ˮ��")
                    {
                        int mzlsh = 0;
                        try
                        {
                            mzlsh = int.Parse(Ssbz);
                        }
                        catch
                        {
                            MessageBox.Show("������ˮ�Ÿ�ʽ����ȷ");
                            return "0";
                        }
                        string rtn = GetSqdxx(1, "", mzlsh, 0, "", 0, debug, odbcstr, tqbblb);
                        if (rtn == "0")
                            return GetBrxx(1, "", mzlsh, 0, "", debug, odbcstr, "0");
                        else
                            return rtn;
                    }
                    if (Sslbx == "���ƿ�")
                    {

                        string rtn = GetSqdxx(5, "", 0, 0, Ssbz, 0, debug, odbcstr, tqbblb);
                        if (rtn == "0")
                            return GetBrxx(5, "", 0, 0, Ssbz, debug, odbcstr, "0");
                        else
                            return rtn;
                    }
                    if (Sslbx == "���뵥��")
                    {
                        int sqdh = 0;
                        try
                        {
                            sqdh = int.Parse(Ssbz);
                        }
                        catch
                        {
                            MessageBox.Show("���뵥�Ÿ�ʽ����ȷ");
                            return "0";
                        }

                        string rtn = GetSqdxx(6, "", 0, 0, "", sqdh, debug, odbcstr, tqbblb);
                        return rtn;
                    }
                    if (Sslbx == "ȡ��HIS���뵥")
                    {
                        #region  ȡ�����뵥
                        string yhgh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim(); ;
                        int sqdh = 0;
                        try
                        {

                            sqdh = int.Parse(Ssbz);
                        }
                        catch
                        {
                            MessageBox.Show("ȡ��ʧ�ܣ����뵥��ʽ����ȷ");
                            return "0";
                        }

                        if (sqdh == 0)
                        {
                            MessageBox.Show("ȡ��ʧ�ܣ����뵥��ʽ����ȷ");
                            return "0";
                        }
                        SqlParameter[] sqlPt = new SqlParameter[3];
                        for (int j = 0; j < sqlPt.Length; j++)
                        {
                            sqlPt[j] = new SqlParameter();
                        }
                        //���뵥ID
                        sqlPt[0].ParameterName = "In_FunctionRequestID";
                        sqlPt[0].SqlDbType = SqlDbType.Int;
                        sqlPt[0].Direction = ParameterDirection.Input;
                        sqlPt[0].Value = sqdh;

                        //����Ա������
                        sqlPt[1].ParameterName = "In_OperatorEmployeeNo";
                        sqlPt[1].SqlDbType = SqlDbType.NVarChar;
                        sqlPt[1].Direction = ParameterDirection.Input;
                        sqlPt[1].Size = 10;
                        sqlPt[1].Value = yhgh;

                        //ȡ����־
                        sqlPt[2].ParameterName = "Out_StatusFlag";
                        sqlPt[2].SqlDbType = SqlDbType.TinyInt;
                        sqlPt[2].Direction = ParameterDirection.Output;
                        sqlPt[2].Value = 0;


                        string err_msg = "";
                        SqlDB_ZGQ db = new SqlDB_ZGQ();
                        int x = db.Sql_ExecuteNonQuery(odbcstr, "pCancelFunctionRequest", ref sqlPt, CommandType.StoredProcedure, ref err_msg);

                        if (int.Parse(sqlPt[2].Value.ToString()) == 1)
                            MessageBox.Show("ȡ���ɹ�");
                        else
                            MessageBox.Show(sqlPt[2].Value.ToString() + "��ȡ��ʧ��");
                        return "0";
                        #endregion
                    }
                    else
                        MessageBox.Show("�޴�" + Sslbx);
            return "0";
        }

        private static string GetBrxx(int Flag, string zyh, int mzlsh, int bbtmh, string zlkh,  string debug, string odbcstr, string tqbblb)
        {
         

            SqlParameter[] sqlPt = new SqlParameter[6];
            for (int j = 0; j < sqlPt.Length; j++)
            {
                sqlPt[j] = new SqlParameter();
            }
            //���
            sqlPt[0].ParameterName = "In_Flag";
            sqlPt[0].SqlDbType = SqlDbType.TinyInt;
            sqlPt[0].Direction = ParameterDirection.Input;
            sqlPt[0].Value = Flag;

            //סԺ��
            sqlPt[1].ParameterName = "In_IPSeqNoText";
            sqlPt[1].SqlDbType = SqlDbType.NVarChar;
            sqlPt[1].Direction = ParameterDirection.Input;
            sqlPt[1].Size = 14;
            sqlPt[1].Value = zyh;

            //�Һ�����
            sqlPt[2].ParameterName = "In_RegisterDate";
            sqlPt[2].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[2].Direction = ParameterDirection.Input;
            sqlPt[2].Value = DateTime.Today;
            //������ˮ��
            sqlPt[3].ParameterName = "In_SeqNo";
            sqlPt[3].SqlDbType = SqlDbType.Int;
            sqlPt[3].Direction = ParameterDirection.Input;
            sqlPt[3].Value = mzlsh;
            //�걾����id
            sqlPt[4].ParameterName = "In_FunctionSampleLabelID";
            sqlPt[4].SqlDbType = SqlDbType.Int;
            sqlPt[4].Direction = ParameterDirection.Input;
            sqlPt[4].Value = bbtmh;

            //���ƿ���
            sqlPt[5].ParameterName = "In_PatientCardNo";
            sqlPt[5].SqlDbType = SqlDbType.NVarChar;
            sqlPt[5].Direction = ParameterDirection.Input;
            sqlPt[5].Size = 30;
            sqlPt[5].Value = zlkh;



            DataTable dt_brxx = new DataTable();
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            string err_msg = "";

           
            dt_brxx = db.Sql_DataAdapter(odbcstr, "pGetPatientInfoForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
 
            if (dt_brxx == null)
            {
                MessageBox.Show(err_msg); return "0";
            }
            if (dt_brxx.Rows.Count <=0)
            {
                MessageBox.Show("δ��ѯ����ؼ�¼"); return "0";
            }
                   

            //-����xml----------------------------------------------------
            try
            {


                string brlb = "1";// dt_brxx.Rows[0]["SourceFlag"].ToString().Trim();
                if (brlb == "0")
                    brlb = "����";
                if (brlb == "1")
                    brlb = "סԺ";
                if (brlb == "2")
                    brlb = "���";

                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "���˱��=" + (char)34 + dt_brxx.Rows[0]["PatientID"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------0
                try
                {
                    if (brlb == "����")
                        xml = xml + "����ID=" + (char)34 + dt_brxx.Rows[0]["SeqNo"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "סԺ")
                        xml = xml + "����ID=" + (char)34 + dt_brxx.Rows[0]["InPatientID"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "���")
                        xml = xml + "����ID=" + (char)34 + dt_brxx.Rows[0]["RegisterID"].ToString().Trim() + (char)34 + " ";
                   
                }
                catch (Exception ee)
                {
                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "�����=" + (char)34 + dt_brxx.Rows[0]["PatientCardNo"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {

                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "סԺ��=" + (char)34 + dt_brxx.Rows[0]["IPSeqNoText"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
     
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "����=" + (char)34 + dt_brxx.Rows[0]["PatientName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
   
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                string xb = dt_brxx.Rows[0]["PatientSex"].ToString().Trim();
                xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";

                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + ZGQClass.CsrqToAge(dt_brxx.Rows[0]["PatientBirthDay"].ToString().Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "����=" + (char)34 + dt_brxx.Rows[0]["Marriage"].ToString().Trim() + (char)34 + " ";

                //----------------------------------------------------------
                try
                {

                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�绰=" + (char)34 + dt_brxx.Rows[0]["Phone"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                 
                    xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + dt_brxx.Rows[0]["IPDepartmentName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                  
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "����=" + (char)34 + dt_brxx.Rows[0]["SickBedNo"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                 
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "���֤��=" + (char)34 + dt_brxx.Rows[0]["IdentityCardNo"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                   
                    xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "����=" + (char)34 + dt_brxx.Rows[0]["RaceDesc"].ToString().Trim() + (char)34 + " ";

                //----------------------------------------------------------
                xml = xml + "ְҵ=" + (char)34 + dt_brxx.Rows[0]["ProfessionDesc"].ToString().Trim() + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + dt_brxx.Rows[0]["DepartmentName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                   
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                 
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------

                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";

                //----------------------------------------------------------
                xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "�ѱ�=" + (char)34 + dt_brxx.Rows[0]["PatientTypeListName"].ToString().Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                   
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                }
                catch (Exception ee)
                {
                  
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                catch (Exception ee)
                {
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                }
                xml = xml + "</LOGENE>";
                return xml;
            }
            catch (Exception ee1)
            {

                MessageBox.Show("��ȡ��Ϣ����;" + ee1.Message);
                return "0";
            }
         
        }

        private static string GetSqdxx(int Flag, string  zyh, int mzlsh, int bbtmh,string  zlkh,int sqdh, string debug, string odbcstr, string tqbblb)
        {

            SqlParameter[] sqlPt = new SqlParameter[10];
            for (int j = 0; j < sqlPt.Length; j++)
            {
                sqlPt[j] = new SqlParameter();
            }
            //���
            sqlPt[0].ParameterName = "In_Flag";
            sqlPt[0].SqlDbType = SqlDbType.TinyInt;
            sqlPt[0].Direction = ParameterDirection.Input;
            sqlPt[0].Value = Flag;

            //��ʼ����
            sqlPt[1].ParameterName = "In_StartDate";
            sqlPt[1].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[1].Direction = ParameterDirection.Input;
            sqlPt[1].Value = DateTime.Today.AddDays(-10);

            //��������
            sqlPt[2].ParameterName = "In_EndDate";
            sqlPt[2].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[2].Direction = ParameterDirection.Input;
            sqlPt[2].Value = DateTime.Today;
            //������ұ��봮
            sqlPt[3].ParameterName = "In_RequestDepartmentNos";
            sqlPt[3].SqlDbType = SqlDbType.NVarChar;
            sqlPt[3].Direction = ParameterDirection.Input;
            sqlPt[3].Size = 10;
            sqlPt[3].Value = "70500";
            //סԺ��
            sqlPt[4].ParameterName = "In_IPSeqNoText";
            sqlPt[4].SqlDbType = SqlDbType.NVarChar;
            sqlPt[4].Direction = ParameterDirection.Input;
            sqlPt[3].Size = 14;
            sqlPt[4].Value = zyh; 
            //�Һ�����
            sqlPt[5].ParameterName = "In_RegisterDate";
            sqlPt[5].SqlDbType = SqlDbType.SmallDateTime;
            sqlPt[5].Direction = ParameterDirection.Input;
            sqlPt[5].Value = DateTime.Today; 
            //������ˮ��
            sqlPt[6].ParameterName = "In_SeqNo";
            sqlPt[6].SqlDbType = SqlDbType.Int;
            sqlPt[6].Direction = ParameterDirection.Input;
            sqlPt[6].Value = mzlsh; 
            //�걾����ID
            sqlPt[7].ParameterName = "In_FunctionSampleLabelID";
            sqlPt[7].SqlDbType = SqlDbType.Int;
            sqlPt[7].Direction = ParameterDirection.Input;
            sqlPt[7].Value = bbtmh;
            //���ƿ���
            sqlPt[8].ParameterName = "In_PatientCardNo";
            sqlPt[8].SqlDbType = SqlDbType.NVarChar;
            sqlPt[8].Direction = ParameterDirection.Input;
            sqlPt[8].Size = 30;
            sqlPt[8].Value = zlkh;
            //���뵥��
            sqlPt[9].ParameterName = "In_FunctionRequestID";
            sqlPt[9].SqlDbType = SqlDbType.Int;
            sqlPt[9].Direction = ParameterDirection.Input;
            sqlPt[9].Value = sqdh;

            DataTable dt_sqdxx = new DataTable();
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            string err_msg = "";
            dt_sqdxx = db.Sql_DataAdapter(odbcstr, "pGetFunctionRequestForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);

            if (dt_sqdxx == null)
            {
                MessageBox.Show(err_msg); return "0";
            }
            if (dt_sqdxx.Rows.Count < 1)
            {
                MessageBox.Show("δ��ѯ��������뵥��¼");
                return "0";
            }

            int xh = 0;
            string yzxm = "";
            if (dt_sqdxx.Rows.Count >= 1)
            {

                string Columns = "RequestDateTime,FunctionRequestID,PatientName,PatientSex,IPSeqNoText,SickBedNo,RequestDepartmentName,RequestEmployeeName";//��ʾ����Ŀ��Ӧ�ֶ�
                string ColumnsName = "��������,���뵥��,����,�Ա�,סԺ��,����,�ͼ����,�ͼ�ҽ��";//��ʾ����Ŀ����
                string xsys = "1"; //ѡ����������Ŀ
                DataColumn dc0 = new DataColumn("���");
                dt_sqdxx.Columns.Add(dc0);
                for (int x = 0; x < dt_sqdxx.Rows.Count; x++)
                {
                    dt_sqdxx.Rows[x][dt_sqdxx.Columns.Count - 1] = x;
                }
                if (Columns.Trim() != "")
                    Columns = "���," + Columns;
                if (ColumnsName.Trim() != "")
                    ColumnsName = "���," + ColumnsName;

                ZSYKYY_SELECT_YZMX yc = new ZSYKYY_SELECT_YZMX(dt_sqdxx, Columns, ColumnsName, xsys, odbcstr);
                yc.ShowDialog();
                if (yc.DialogResult == DialogResult.Yes)
                {
                    string rtn2 = yc.F_XH;
                    yzxm = yc.F_yzxm;
                    if (rtn2.Trim() == "")
                    {
                        MessageBox.Show("δѡ�����뵥��");
                        return "0";
                    }
                    try
                    {
                        xh = int.Parse(rtn2);
                    }
                    catch
                    {
                        MessageBox.Show("������ѡ�����뵥��");
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("������ѡ�����뵥��");
                    return "0";
                }
            }
            //  string yzxm = "";
            //try
            //{
            //    string sqxh = "";
            //    //��ȡ���뵥��ϸ
            //    SqlParameter[] sqlPt_sqdmx = new SqlParameter[1];
            //    sqlPt[0] = new SqlParameter();
            //    //���
            //    sqlPt[0].ParameterName = "In_FunctionRequestIDs";
            //    sqlPt[0].SqlDbType = SqlDbType.NVarChar;
            //    sqlPt[0].Size = 200;
            //    sqlPt[0].Direction = ParameterDirection.Input;
            //    sqlPt[0].Value = dt_sqdxx.Rows[xh]["FunctionRequestID"].ToString().Trim();

                
            //    DataTable dt_sqdmx = new DataTable();

            //    dt_sqdmx = db.Sql_DataAdapter(odbcstr, "pGetFunctionRequestListForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
            //    if (dt_sqdmx == null || dt_sqdmx.Rows.Count < 1)
            //    {

            //    }
            //    else
            //    {
            //        yzxm = dt_sqdmx.Rows[0]["ItemID"].ToString().Trim() + "^" + dt_sqdmx.Rows[0]["ItemName"].ToString().Trim();
            //    }
            //}
            //catch
            //{
            //}

                //-����xml----------------------------------------------------
                try
                {

                    string brlb = "1";// dt_sqdxx.Rows[xh]["SourceFlag"].ToString().Trim();
                    if (brlb == "0")
                        brlb = "����";
                    if (brlb == "1")
                        brlb = "סԺ";
                    if (brlb == "2")
                        brlb = "���";


                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    try
                    {
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[xh]["PatientID"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        if(brlb=="����")
                            xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["SeqNo"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "סԺ")
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["InPatientID"].ToString().Trim() + (char)34 + " ";
                    if (brlb == "���")
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["RegisterID"].ToString().Trim() + (char)34 + " ";
                   
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["FunctionRequestID"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {

                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[xh]["PatientCardNo"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                       
                        xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    try
                    {
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[xh]["IPSeqNoText"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    try
                    {
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["PatientName"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                     
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------


                    xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[xh]["PatientSex"].ToString().Trim() + (char)34 + " ";

                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "����=" + (char)34 + ZGQClass.CsrqToAge(dt_sqdxx.Rows[xh]["PatientBirthDay"].ToString().Trim()) + (char)34 + " ";
                    }
                    catch
                    {
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["Marriage"].ToString().Trim() + (char)34 + " ";

                    //----------------------------------------------------------
                    try
                    {

                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                  
                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[xh]["Phone"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                    
                        xml = xml + "�绰=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["IPDepartmentName"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["SickBedNo"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                     
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[xh]["IdentityCardNo"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                   
                        xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------

                    xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["RaceDesc"].ToString().Trim() + (char)34 + " ";

                    //----------------------------------------------------------
                    xml = xml + "ְҵ=" + (char)34 + dt_sqdxx.Rows[xh]["ProfessionDesc"].ToString().Trim() + (char)34 + " ";
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[xh]["RequestDepartmentName"].ToString().Trim() + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";

                    }
                    //----------------------------------------------------------
                    string ViceDirectorEmployeeID="";
                     string RequestEmployeeName="";
                    try{
                        ViceDirectorEmployeeID = dt_sqdxx.Rows[xh]["ViceDirectorEmployeeName"].ToString().Trim();
                    }
                    catch
                    {
                    }

                    try
                    {
                        RequestEmployeeName = dt_sqdxx.Rows[xh]["RequestEmployeeName"].ToString().Trim();
                    }
                    catch
                    {
                    }
                    if (ViceDirectorEmployeeID=="")
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + RequestEmployeeName + (char)34 + " ";
                    else
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + ViceDirectorEmployeeID + "/" + RequestEmployeeName + (char)34 + " ";
                    //----------------------------------------------------------

                    xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                    //----------------------------------------------------------
                    xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                    //----------------------------------------------------------

                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";

                    //----------------------------------------------------------
                    xml = xml + "ҽ����Ŀ=" + (char)34 + yzxm + (char)34 + " ";
                    //----------------------------------------------------------
                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                    //----------------------------------------------------------
                    xml = xml + "����2=" + (char)34 + dt_sqdxx.Rows[xh]["RequestEmployeeNo"].ToString().Trim() + (char)34 + " ";
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                    }
                    catch (Exception ee)
                    {
                
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                    }
                    //----------------------------------------------------------
                    xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                    xml = xml + "/>";
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                    }
                    catch (Exception ee)
                    {
                     
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                    }
                    //----------------------------------------------------------
                    try
                    {
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[xh]["diseaseName"].ToString().Trim() + "]]></�ٴ����>";
                    }
                    catch (Exception ee)
                    {
                      
                        xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                    }
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;
                }
                catch (Exception ee1)
                {

                    MessageBox.Show("��ȡ��Ϣ����;" + ee1.Message);
                    return "0";
                }
        }

        public static string ZtMsg(DataTable dt_sqdxx, ref string errmsg, string yhmc, string yhbh)
        {
            string bgztbm = "160.009";
            string bgztstr = "ȡ�����뵥";

            try
            {
              string   xbrlb = dt_sqdxx.Rows[0]["F_jzlb"].ToString();
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3  ../../Schemas/POOR_IN200901UV23.xsd\">";

                xml = xml + "<!-- ��ϢID -->";
                xml = xml + "<id extension=\"BS004\" />";
                xml = xml + "<!-- ��Ϣ����ʱ�� -->";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<interactionId extension=\"POOR_IN200901UV23\" />";
                xml = xml + "<!--��Ϣ��;: P(Production); D(Debugging); T(Training) -->";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<!-- ��Ϣ����ģʽ: A(Archive); I(Initial load); R(Restore from archive); T(Current  processing) -->";
                xml = xml + "<processingModeCode code=\"R\" />";
                xml = xml + "<!-- ��ϢӦ��: AL(Always); ER(Error/reject only); NE(Never) -->";
                xml = xml + "<acceptAckCode code=\"NE\" />";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";

                xml = xml + "<!-- ������ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<!-- ������ -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ������ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"S009\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<!-- ��װ����Ϣ����(��Excel��д) -->";
                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<!-- ��Ϣ�������� @code: ���� :new �޸�:update -->";
                xml = xml + "<code code=\"update\"></code>";
                xml = xml + "<subject typeCode=\"SUBJ\" xsi:nil=\"false\">";
                xml = xml + "<placerGroup>";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<code></code>";
                xml = xml + "<!-- �������뵥״̬ ������δʹ�� -->";
                xml = xml + "<statusCode code=\"active\"></statusCode>";
                xml = xml + "<!-- ������Ϣ -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<id>";
                xml = xml + "<!--��ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.2\" extension=\"" + dt_sqdxx.Rows[0]["F_yid"].ToString() + "\" />";
                xml = xml + "<!-- ����ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_jzh"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"  determinerCode=\"INSTANCE\">";
                xml = xml + "<!--���˿��ұ���-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ksbm"].ToString() + "\" root=\"1.2.156.112678.1.1.1\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--���˿������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_ksmc"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--ҽ�ƻ������� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yybm"].ToString() + "\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--ҽ�ƻ������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item><part value=\"" + dt_sqdxx.Rows[0]["F_yymc"].ToString() + "\" /></item>";
                xml = xml + "</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</providerOrganization>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "<!-- ������ -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<time>";
                xml = xml + "<!-- �������� -->";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</time>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- �����˱��� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + yhbh + "\" root=\"1.2.156.112678.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- ���������� ��������ʹ�� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + yhmc + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<!--ִ�п��� -->";
                xml = xml + "<location typeCode=\"LOC\" xsi:nil=\"false\">";
                xml = xml + "<!--������δʹ�� -->";
                xml = xml + "<time />";
                xml = xml + "<!--�������/���� -->";
                xml = xml + "<serviceDeliveryLocation classCode=\"SDLOC\">";
                xml = xml + "<serviceProviderOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--ִ�п��ұ��� -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_zxksbm"].ToString() + "\" root=\"1.21.156.112649.1.1.1\" />";
                xml = xml + "</id>";
                xml = xml + "<!--ִ�п������� -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_zxks"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</serviceDeliveryLocation>";
                xml = xml + "</location>";

                xml = xml + "<!-- 1..n��ѭ��  ҽ��״̬��Ϣ -->";
                xml = xml + "<component2>";
                xml = xml + "<!--ҽ�����-->";
                xml = xml + "<sequenceNumber value=\"1\"/>";
                xml = xml + "<observationRequest classCode=\"OBS\">";
                xml = xml + "<!-- ��������ʹ�� -->";
                xml = xml + "<id>";
                xml = xml + "<!-- ҽ���� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yzh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.22\"/>";
                xml = xml + "<!-- ���뵥�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_sqxh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.21\"/>";
                xml = xml + "<!-- ����� -->";
                xml = xml + "<item extension=\"" + "" + "\" root=\"1.2.156.112678.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112678.1.2.1.30\"/>";
                xml = xml + "</id>";

                xml = xml + "<!-- ҽ��������/ҽ��������� - ���ҩƷ, ������, ������, Ƭ��ҩƷ, ������ -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.27\">";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_YZLXMC"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- ������δʹ�� -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";

                xml = xml + "<!-- �걾��Ϣ -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                string[] bbtmh = dt_sqdxx.Rows[0]["F_bbtmh"].ToString().Trim().Split('#');
                foreach (string bbtm in bbtmh)
                {
                    xml = xml + "<specimen classCode=\"SPEC\">";
                    xml = xml + "<!--�걾����� ��������ʹ�� -->";
                    xml = xml + "<id extension=\"" + bbtm + "\" />";
                    xml = xml + "<!--������Ŀδʹ�� -->";
                    xml = xml + "<code />";
                    xml = xml + "<subjectOf1 typeCode=\"SBJ\" contextControlCode=\"OP\">";
                    xml = xml + "<specimenProcessStep moodCode=\"EVN\" classCode=\"SPECCOLLECT\">";
                    xml = xml + "<!-- �ɼ����� -->";
                    xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                    xml = xml + "<any value=\"" + Convert.ToDateTime(DateTime.Now.ToString("yyyyMMddHHmmss")) + "\"></any>";
              
                    xml = xml + "</effectiveTime>";
                    xml = xml + "<performer typeCode=\"PRF\">";
                    xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                    xml = xml + "<!-- �ɼ���Id -->";
                    xml = xml + "<id>";
                    xml = xml + "<item extension=\"" +yhbh + "\" root=\"1.2.156.112678.1.1.2\"></item>";
                    xml = xml + "</id>";
                    xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                    xml = xml + "<!-- �ɼ������� -->";
                    xml = xml + "<name xsi:type=\"BAG_EN\">";
                    xml = xml + "<item>";
                    xml = xml + "<part value=\"" + yhmc + "\" />";
                    xml = xml + "</item>";
                    xml = xml + "</name>";
                    xml = xml + "</assignedPerson>";
                    xml = xml + "</assignedEntity>";
                    xml = xml + "</performer>";
                    xml = xml + "</specimenProcessStep>";
                    xml = xml + "</subjectOf1>";
                    xml = xml + "</specimen>";

                }
                xml = xml + "</specimen>";

                xml = xml + "<!-- ԭ�� -->";
                xml = xml + "<reason contextConductionInd=\"true\">";
                xml = xml + "<observation moodCode=\"EVN\" classCode=\"OBS\">";
                xml = xml + "<!-- ������ δʹ��-->";
                xml = xml + "<code></code>";
                xml = xml + "<value xsi:type=\"ST\" value=\"\"/>";
                xml = xml + "</observation>";
                xml = xml + "</reason>";
                xml = xml + "<!-- ҽ��ִ��״̬ -->";
                xml = xml + "<component1 contextConductionInd=\"true\">";
                xml = xml + "<processStep classCode=\"PROC\">";
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112678.1.1.93\">";
                xml = xml + "<!--ҽ��ִ��״̬���� -->";
                xml = xml + "<displayName value=\"" + bgztstr + "\" />";
                xml = xml + "</code>";
                xml = xml + "</processStep>";
                xml = xml + "</component1>";
                xml = xml + "</observationRequest>";
                xml = xml + "</component2>";

                xml = xml + "<!--���� -->";
                xml = xml + "<componentOf1 contextConductionInd=\"false\" xsi:nil=\"false\" typeCode=\"COMP\">";
                xml = xml + "<!--���� -->";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<id>";
                xml = xml + "<!-- ������� ��������ʹ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112678.1.2.1.7\" />";
                xml = xml + "<!-- ������ˮ�� -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112678.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--����������-->";
                xml = xml + "<code codeSystem=\"1.2.156.112678.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
                xml = xml + "<!-- ����������� -->";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_BRLB"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!--������δʹ�� -->";
                xml = xml + "<statusCode code=\"Active\" />";
                xml = xml + "<!--���� ������δʹ�� -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\" />";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf1>";
                xml = xml + "</placerGroup>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</POOR_IN200901UV>";

                return FormatXml(xml);

            }
            catch (Exception ex2)
            {
              MessageBox.Show("ƽ̨�������뵥XML�����쳣:" + ex2.Message);
                return "";
            }
        }

        public static void ZtToHis(string Sslbx, string sqxh, string yhmc, string yhgh, string debug)
        {
            try
            {
                int.Parse(sqxh);
            }
            catch
            {
                MessageBox.Show("������Ÿ�ʽ����ȷ");
                return;
            }
            string odbc2his = f.ReadString("Sslbx", "odbc2his", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();
            try
            {
                SqlParameter[] sqlPt = new SqlParameter[3];
                for (int j = 0; j < sqlPt.Length; j++)
                {
                    sqlPt[j] = new SqlParameter();
                }
                //���뵥ID
                sqlPt[0].ParameterName = "In_FunctionRequestID";
                sqlPt[0].SqlDbType = SqlDbType.Int;
                sqlPt[0].Direction = ParameterDirection.Input;
                sqlPt[0].Value = int.Parse(sqxh);

                //����Ա������
                sqlPt[1].ParameterName = "In_OperatorEmployeeNo";
                sqlPt[1].SqlDbType = SqlDbType.NVarChar;
                sqlPt[1].Direction = ParameterDirection.Input;
                sqlPt[1].Size = 10;
                sqlPt[1].Value = yhgh;
                //ȡ����־
                sqlPt[2].ParameterName = "Out_StatusFlag";
                sqlPt[2].SqlDbType = SqlDbType.TinyInt;
                sqlPt[2].Direction = ParameterDirection.Output;
                sqlPt[2].Value = 0;

                string err_msg = "";
                SqlDB_ZGQ db = new SqlDB_ZGQ();
                db.Sql_ExecuteNonQuery(odbc2his, "pCancelFounctionRequest", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
                if (int.Parse(sqlPt[2].Value.ToString()) == 1)
                {
                  MessageBox.Show("HIS:ȡ�����뵥�ɹ�");
                }
                else
                {
                    MessageBox.Show("HIS:ȡ�����뵥ʧ��:" + sqlPt[2].Value.ToString());
                }
            }
            catch (Exception ee1)
            {
                MessageBox.Show("HIS:ȡ�����뵥�쳣:" + ee1.Message);
            }
        }
        private static string FormatXml(string sUnformattedXml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }
     

    }
}
