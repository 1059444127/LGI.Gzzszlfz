using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using System.Data.SqlClient;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    /// <summary>
    /// �����е�һ����ҽԺ
    /// </summary>
    class cdsdyrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
         
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

          //  string pathWEB = f.ReadString(Sslbx, "webservicesurl", ""); //��ȡsz.ini�����õ�webservicesurl
          
            string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
            if (Sslbx != "")
            {
               
                if (Sslbx == "�����")
                {
                    #region �����
                    string  sqdh="";
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_bbxx = new DataTable();
                    dt_bbxx = aa.GetDataTable("select * from T_sqd_bbxx where F_TMH='" + Ssbz.Trim() + "'", "bbtmh");

                    if (dt_bbxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("�޴˱걾������Ϣ"); return "0";
                    }
            
                        sqdh=dt_bbxx.Rows[0]["F_SQXH"].ToString().Trim();
                        DataTable dt_sqdxx = new DataTable();
                        dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_sqxh='" +sqdh + "' ", "sqdxx");

                         if (dt_sqdxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("�޴˱걾�����Ӧ���뵥��Ϣ"); return "0";
                        }

                        DataTable dt_yzxx = new DataTable();
                        dt_yzxx = aa.GetDataTable("select * from T_sqd_yzxx where F_sqxh='" +sqdh + "'", "yzxx");

                        DataTable dt_bbxx2 = new DataTable();
                        dt_bbxx2 = aa.GetDataTable("select * from T_sqd_bbxx where F_sqxh='" + sqdh + "'", "bbxx");


                        if (dt_bbxx2.Rows.Count > 1)
                        {
                            if (tqbblb == "1")
                            {
                                bool yzbbxx = true;
                                Frm_CDRMYY cd = new Frm_CDRMYY(sqdh, dt_yzxx, dt_bbxx2, dt_sqdxx, Ssbz.Trim(), yzbbxx);
                                cd.ShowDialog();

                                if (cd.DialogResult == DialogResult.Yes)
                                {
                                }
                                else
                                {
                                    if (yzbbxx)
                                    {
                                        MessageBox.Show("�걾����������ȷ,������ȡ��Ϣ��");
                                        return "0";
                                    }
                                }
                            }
                        }

                    

                    DataTable dt = new DataTable();
                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[0]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[0]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[0]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[0]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "δ��"; break;
                            case "2": MARRIED = "�ѻ�"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[0]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqdxx.Rows[0]["F_SQYS"].ToString().Trim() + (char)34 + " ";

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + dt_sqdxx.Rows[0]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqdxx.Rows[0]["F_YZXMBM"].ToString().Trim()+"^"+dt_sqdxx.Rows[0]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[0]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[0]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";


                        if (tqbblb == "1")
                        {
                         string   BBLB_XML = "<BBLB>";
                            try
                            {
                                for (int x = 0; x < dt_bbxx2.Rows.Count; x++)
                                {
                                    try
                                    {
                                        BBLB_XML = BBLB_XML + "<row ";
                                        BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +(x+1).ToString()+ (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bbxx2.Rows[x]["F_TMH"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bbxx2.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bbxx2.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
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

                            if (tqbblb == "1")
                                xml = xml + BBLB_XML;
                        }

                     

                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "���뵥��")
                {
                    #region ���뵥��
                    string sqdh = Ssbz.Trim();
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                   
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_sqxh='" + sqdh + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("�޴˶�Ӧ���뵥��¼(" + sqdh + ")"); return "0";
                    }

                    DataTable dt_bbxx2 = new DataTable();
                    dt_bbxx2 = aa.GetDataTable("select * from T_sqd_bbxx where F_sqxh='" + sqdh + "'", "bbxx");

                    //if (dt_bbxx2.Rows.Count <= 0)
                    //{
                    //    MessageBox.Show("�޴˱걾������Ϣ"); return "0";
                    //}
                    //if (dt_bbxx2.Rows.Count > 1)
                    //{
                    //    if (tqbblb == "1")
                    //    {
                           
                    //            DataTable dt_yzxx = new DataTable();
                    //            dt_yzxx = aa.GetDataTable("select * from T_sqd_yzxx where F_sqxh='" + sqdh + "'", "yzxx");


                    //            bool yzbbxx = false;
                    //            Frm_CDRMYY cd = new Frm_CDRMYY(sqdh, dt_yzxx, dt_bbxx2, dt_sqdxx, Ssbz.Trim(), yzbbxx);
                    //            cd.ShowDialog();

                    //            if (cd.DialogResult == DialogResult.Yes)
                    //            {
                    //            }
                    //            else
                    //            {
                    //                if (yzbbxx)
                    //                {
                    //                    MessageBox.Show("�걾����������ȷ,������ȡ��Ϣ��");
                    //                    return "0";
                    //                }
                    //            }
                            
                    //    }
                    //}

                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[0]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[0]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[0]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[0]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "δ��"; break;
                            case "2": MARRIED = "�ѻ�"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[0]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[0]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqdxx.Rows[0]["F_SQYS"].ToString().Trim() + (char)34 + " ";

                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + dt_sqdxx.Rows[0]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqdxx.Rows[0]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[0]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[0]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[0]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";


                        if (tqbblb == "1")
                        {
                          string   BBLB_XML = "<BBLB>";
                            try
                            {
                                for (int x = 0; x < dt_bbxx2.Rows.Count; x++)
                                {
                                    try
                                    {
                                        BBLB_XML = BBLB_XML + "<row ";
                                        BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + (x + 1).ToString() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bbxx2.Rows[x]["F_TMH"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bbxx2.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bbxx2.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
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

                            if (tqbblb == "1")
                                xml = xml + BBLB_XML;

                        }

                       
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "�����")
                {
                    #region �����

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where f_jzh='" + Ssbz.Trim() + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("�޴˶�Ӧ���뵥��¼(" + Ssbz.Trim() + ")"); return "0";
                    }


                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_zyh,F_BQ,F_CH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//��ʾ����Ŀ��Ӧ�ֶ�
                        string ColumnsName = "���뵥��,�������,����,�Ա�,����,סԺ��,����,����,�ͼ����,�ͼ�ҽ��,��������,��Ŀ����";//��ʾ����Ŀ����
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

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("δѡ��������Ŀ��");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);

                            }
                            catch
                            {
                                MessageBox.Show("������ѡ��������Ŀ��");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("������ѡ��������Ŀ��");
                            return "0";
                        }
                    }
                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "δ��"; break;
                            case "2": MARRIED = "�ѻ�"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "סԺ��(��)" || Sslbx == "סԺ����" || Sslbx == "סԺ��" || Sslbx == "��סԺ��")
                {
                    #region סԺ��

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_zyh='" + Ssbz.Trim() + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("�޴˶�Ӧ���뵥��¼(" + Ssbz.Trim() + ")"); return "0";
                    }


                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_zyh,F_BQ,F_CH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//��ʾ����Ŀ��Ӧ�ֶ�
                        string ColumnsName = "���뵥��,�������,����,�Ա�,����,סԺ��,����,����,�ͼ����,�ͼ�ҽ��,��������,��Ŀ����";//��ʾ����Ŀ����
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

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("δѡ��������Ŀ��");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);

                            }
                            catch
                            {
                                MessageBox.Show("������ѡ��������Ŀ��");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("������ѡ��������Ŀ��");
                            return "0";
                        }
                    }
                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "δ��"; break;
                            case "2": MARRIED = "�ѻ�"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "����" || Sslbx == "�����")
                {
                    #region ����

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_mzh='" +  Ssbz.Trim() + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("�޴˶�Ӧ���뵥��¼(" + Ssbz.Trim() + ")"); return "0";
                    }

                  
                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_MZH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//��ʾ����Ŀ��Ӧ�ֶ�
                        string ColumnsName = "���뵥��,�������,����,�Ա�,����,�����,�ͼ����,�ͼ�ҽ��,��������,��Ŀ����";//��ʾ����Ŀ����
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

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("δѡ��������Ŀ��");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);
                                
                            }
                            catch
                            {
                                MessageBox.Show("������ѡ��������Ŀ��");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("������ѡ��������Ŀ��");
                            return "0";
                        }
                    }
                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "δ��"; break;
                            case "2": MARRIED = "�ѻ�"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "ID��"||Sslbx == "����ID��")
                {
                    #region ID��

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_brbh='000" + Ssbz.Trim() + "00' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("�޴˶�Ӧ���뵥��¼(" + Ssbz.Trim() + ")"); return "0";
                    }

                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_MZH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//��ʾ����Ŀ��Ӧ�ֶ�
                        string ColumnsName = "���뵥��,�������,����,�Ա�,����,�����,�ͼ����,�ͼ�ҽ��,��������,��Ŀ����";//��ʾ����Ŀ����
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

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("δѡ��������Ŀ��");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);

                            }
                            catch
                            {
                                MessageBox.Show("������ѡ��������Ŀ��");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("������ѡ��������Ŀ��");
                            return "0";
                        }
                    }
                    //-����xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "���˱��=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "סԺ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�Ա�=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "δ��"; break;
                            case "2": MARRIED = "�ѻ�"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></�ٴ����>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                else
                {
                    MessageBox.Show("�޴�" + Sslbx);
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
        public  static string xmlstr()
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
            xml = xml + "�ͼ�ҽԺ=" + (char)34 + (char)34 + " ";
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
      }
}

