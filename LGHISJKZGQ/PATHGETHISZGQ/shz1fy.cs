using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Windows.Forms;
using System.Data;
using LGHISJKZGQ;
using LGHISJK;


namespace LGHISJKZGQ
{
    class shz1fy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public static string cz1yxml(string Sslbx, string Ssbz, string Debug)
        {
            dbbase.sqldb aa = new sqldb(Application.StartupPath + "\\sz.ini", "jsddb");
            string brlb = "";
            string codetype = "";
            string zxks = f.ReadString(Sslbx, "zxks", "307").Replace("\0", "");
            string yzxm = "";
            
            int sqd = 0;
            if (Sslbx == "����")
            {
                brlb = "3";
                codetype = "1";
                sqd = f.ReadInteger(Sslbx, "sqd", 0);
                zxks = f.ReadString(Sslbx, "zxks", "307").Replace("\0", "");
            }
            if (brlb == "")
            {
                MessageBox.Show("�޴�" + Sslbx);
                return "0";
            }
            string sqlstring = "exec usp_yjjk_getbrxx " + brlb + "," + codetype + ",'" + Ssbz + "',0";
            
            DataSet ds1 = aa.GetDataSet(sqlstring, "tjdt");
            try
            {
                if (ds1.Tables[0].Rows.Count < 1)
                {
                    MessageBox.Show("δ�ҵ����ˣ�");
                    return "0";
                }
            }
            catch
            {
                MessageBox.Show("HIS����ʧ�ܣ�");
                return "0";
            }

            DataTable jcxm = aa.GetDataTable("exec usp_yjjk_getwzxxm " + brlb + ",'" + Ssbz + "','" + Ssbz + "','','',0,0", "jcxm");

            string jcxmdm = "";
            int intxh = 0;
          
            try
            {
                if (jcxm.Rows.Count > 0)
                {
                  
                    //1508250002
                    DataTable dtx = new DataTable();
                    dtx.Columns.Add("���", Type.GetType("System.String"));
                    dtx.Columns.Add("ҽ����Ŀ", Type.GetType("System.String"));
                    dtx.Columns.Add("����", Type.GetType("System.String"));
                    dtx.Columns.Add("����", Type.GetType("System.String"));
                    dtx.Columns.Add("����ʱ��", Type.GetType("System.String"));
                    dtx.Columns.Add("�������", Type.GetType("System.String"));
                    dtx.Columns.Add("ִ�п���", Type.GetType("System.String"));

                    string[] dtxrow = new string[dtx.Columns.Count];
                    for (int i = 0; i < jcxm.Rows.Count; i++)
                    {
                       
                        if (jcxm.Rows[i]["ExecDept"].ToString() != "307" && jcxm.Rows[i]["ExecDept"].ToString() != "�����")
                        {   
                           jcxm.Rows.RemoveAt(i);i--; 
                            continue;
                        }

                            dtxrow[0] = i.ToString();
                            dtxrow[1] = jcxm.Rows[i]["ItemName"].ToString();
                            dtxrow[2] = jcxm.Rows[i]["Price"].ToString();
                            dtxrow[3] = jcxm.Rows[i]["ItemQty"].ToString();
                            dtxrow[4] = jcxm.Rows[i]["ApplyTime"].ToString();
                            dtxrow[5] = jcxm.Rows[i]["ApplyDept"].ToString();
                            dtxrow[6] = jcxm.Rows[i]["ExecDept"].ToString();
                            dtx.Rows.Add(dtxrow);
                        
                    }

                    if (dtx.Rows.Count >1)
                    {
                      
                        yzxz_yfy from2 = new yzxz_yfy(dtx);
                     
                        string xhb = "";

                        if (from2.ShowDialog() == DialogResult.OK)
                        {
                            xhb = from2.xh;
                            intxh = Convert.ToInt16(xhb);
                            jcxmdm = jcxm.Rows[intxh]["LogNo"].ToString().Trim();
                            yzxm = jcxm.Rows[intxh]["ItemName"].ToString().Trim();
                        }
                        else
                        {
                            MessageBox.Show("δѡ��ҽ����");
                            return "0";
                        }
                    }
                    else
                    {
                        jcxmdm = jcxm.Rows[0]["LogNo"].ToString().Trim();
                        yzxm = jcxm.Rows[0]["ItemName"].ToString().Trim();
                    }

                }
                else
                {
                    if (MessageBox.Show("δ�ҵ�ҽ����Ϣ���Ƿ���ȡ������Ϣ��", "����", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    { }
                    else
                    {
                        return "0";
                    }

                }
            }
            catch
            {
                if (MessageBox.Show("δ�ҵ�ҽ����Ϣ���Ƿ���ȡ������Ϣ��", "����", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                { }
                else
                {
                    return "0";
                }
            }

            int xh = 0;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                if (f.ReadInteger(Sslbx, "cardno", 0) != 1)
                {
                    xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["PatientID"].ToString().Trim() + (char)34 + " ";
                }
                else
                {
                    xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["CardNO"].ToString().Trim() + (char)34 + " ";
                }
                //MessageBox.Show(ds1.Tables[0].Rows[xh]["CardNO"].ToString().Trim());
                //MessageBox.Show(f.ReadInteger(Sslbx, "cardno", 0).ToString());
                try
                {
                    xml = xml + "����ID=" + (char)34 + ds1.Tables[0].Rows[xh]["CureNo"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����ID=" + (char)34 + (char)34 + " ";
                }
                xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
               
                xml = xml + "�����=" + (char)34 + (char)34 + " ";
                
                
                
                xml = xml + "סԺ��=" + (char)34 + (char)34 + " ";
                
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["PatName"].ToString().Trim() + (char)34 + " ";
                string xb = "";
                if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "1") xb = "��";
                if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "2") xb = "Ů";
                if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "3") xb = "����";

                xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";

                string ppp = ds1.Tables[0].Rows[xh]["BIRTH"].ToString().Trim();
                string pppp = "";
                string nl = "";
                try
                {
                    pppp = ppp.Substring(0, 4) + "-" + ppp.Substring(4, 2) + "-" + ppp.Substring(6, 2);
                    nl = datediff(DateTime.Now, Convert.ToDateTime(pppp));

                }
                catch
                {
                }
                

                xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                xml = xml + "����=" + (char)34 + (char)34 + " ";
                try
                {
                    xml = xml + "��ַ=" + (char)34 + ds1.Tables[0].Rows[xh]["Address"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "��ַ=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "�绰=" + (char)34 + ds1.Tables[0].Rows[xh]["Phone"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�绰=" + (char)34 + (char)34 + " ";
                }
                
                xml = xml + "����=" + (char)34 + (char)34 + " ";
                
                try
                {
                    xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["BedNo"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "���֤��=" + (char)34 + ds1.Tables[0].Rows[xh]["IDNum"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "���֤��=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["Nation"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "ְҵ=" + (char)34 + ds1.Tables[0].Rows[xh]["Career"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "ְҵ=" + (char)34 + (char)34 + " ";
                }
                
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + ds1.Tables[0].Rows[xh]["DeptName"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ����=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + ds1.Tables[0].Rows[xh]["ToDoc"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + (char)34 + " ";
                }
                
                //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
                //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
                xml = xml + "�շ�=" + (char)34 + (char)34 + " ";
                xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                xml = xml + "ҽ����Ŀ=" + (char)34 + yzxm + (char)34 + " ";
                xml = xml + "����1=" + (char)34 + (char)34 + " ";

                xml = xml + "����2=" + (char)34 + jcxmdm + (char)34 + " ";


               


                string fkfs = "";

                //if (ds1.Tables[0].Rows[xh]["ChargeType"].ToString().Trim() == "7") fkfs = "�ɱ�";

                xml = xml + "�ѱ�=" + (char)34 + fkfs + (char)34 + " ";
               
                xml = xml + "�������=" + (char)34 + "���" + (char)34 + " ";
                
                xml = xml + "/>";
                xml = xml + "<�ٴ���ʷ><![CDATA[" + " " + "]]></�ٴ���ʷ>";
                xml = xml + "<�ٴ����><![CDATA[" + " " + "]]></�ٴ����>";
                xml = xml + "</LOGENE>";
                if (Debug == "1")
                    log.WriteMyLog("���ص�xml�ַ���:" + xml);
                return xml;

            }
            else
            {
                MessageBox.Show("�޴�" + Sslbx);
                return "0";
            }
            

        }
        private static string datediff(DateTime tm1, DateTime tm2)
        {
            string diff = "";
            //tm2 = Convert.ToDateTime("2010-01-02");
            try
            {
                if (tm2 > tm1)
                {
                    diff = "0��";
                }
                else
                {
                    int nly = (tm1.Year - tm2.Year) * 12 + (tm1.Month - tm2.Month);
                    if (nly > 12)
                    {
                        int xxxx = nly / 12;
                        diff = Convert.ToString(xxxx) + "��";
                    }
                    else
                    {
                        TimeSpan ts = tm1 - tm2;
                        if (ts.Days < 31)
                        {
                            diff = Convert.ToString(ts.Days) + "��";
                        }
                        else
                        {
                            diff = Convert.ToString(nly) + "��";
                        }
                    }

                }
            }
            catch
            {
                diff = "0��";
            }
            return diff;
        }
    }
}
