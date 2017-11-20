using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using LGHISJKZGQ;
using dbbase;
using HL7;
namespace LGHISJKZGQ
{
    class SYSZXYY
    {
       /// <summary>
       /// ����������ҽԺ--HL7-�������뵥
       /// </summary>
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        // private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
       
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
          
            if (Sslbx != "")
            {
               
                ///�������뵥
                if (Sslbx == "�������뵥")
                {
                   
            string hl7server = f.ReadString("savetohis", "hl7server", "");
            string hl7port = f.ReadString("savetohis", "hl7port", "");
            string msg = f.ReadString("savetohis", "msg", "");
           odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            
            DataTable hl7xx = new DataTable();
            hl7xx = aa.GetDataTable("select * from  T_HL7_sqd  where substring(F_sqxh,0,charindex('^',F_sqxh+'^',0))='" + Ssbz + "'", "bl_sqd");
            if (hl7xx == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                return "0";
            }
            if (hl7xx.Rows.Count < 1)
            {
                MessageBox.Show("�޴˲������뵥�ż�¼��������");
                return "0";
            }
          
            if (hl7xx.Rows[0]["F_FYQRBJ"].ToString().Trim()!="1")
            {
               MessageBox.Show("������ż�¼δ������ϵͳȷ�Ϸ��ã�������");
                return "0";
            }
            if (MessageBox.Show("�Ƿ�ȷ���Բ��ˣ�" + hl7xx.Rows[0]["F_XM"].ToString().Trim() + ",����ţ�" + Ssbz + "\n����ȡ�����뵥���˷ѣ�����", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return "0";
            }
            string pid = hl7xx.Rows[0]["F_PID"].ToString().Trim();
            string pv1 = hl7xx.Rows[0]["F_PV1"].ToString().Trim();
            string orc = hl7xx.Rows[0]["F_ORC"].ToString().Trim();
            string obr = hl7xx.Rows[0]["F_OBR"].ToString().Trim();


            ///����������ҽԺ �ش���ת��gbk����
            string XXX = @"^~\&";
            string s12message = "MSH|" + XXX + "|BL|LOGENE|PT||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + hl7xx.Rows[0]["F_messageid"].ToString() + "|P|2.4|||||gbk\r";

            s12message = s12message + pid + "\r";
            s12message = s12message + pv1 + "\r";
            string[] orc1 = orc.Split('|');
            string orc2 = "ORC|OC";
            for (int x = 2; x < orc1.Length; x++)
            {
                if (x == 5)
                {
                    orc2 = orc2 + "|OC";
                    continue;
                }
                if (x == 25)
                {
                    if(hl7xx.Rows[0]["F_brly"].ToString().Trim()=="I")
                    orc2 = orc2 + "|0";
                    else
                    orc2 = orc2 + "|3";
                    continue;
                }
                    orc2 = orc2 + "|" + orc1[x];
            }

            s12message = s12message + orc2 + "\r";

            string[] obr2 = obr.Split('|');
            string obr1 = "";
            obr2[27] = "1";

            obr2[23] = obr2[23].Split('^')[0]+"^2";
            obr1 = "OBR";
            for (int x = 1; x < obr2.Length; x++)
            {
                obr1 = obr1 + "|" + obr2[x];
            }
            s12message = s12message + obr1 + "\r";
            byte[] by2 = System.Text.Encoding.GetEncoding("gbk").GetBytes(s12message);

            byte[] by3 = new byte[by2.Length + 3];
            by3[0] = 11;
            Array.Copy(by2, 0, by3, 1, by2.Length);
            by3[by3.Length - 2] = 28;
            by3[by3.Length - 1] = 13;
            if (msg=="1")
            log.WriteMyLog("send to server " + hl7server + ":" + hl7port + " message:" + System.Text.Encoding.GetEncoding("gbk").GetString(by3, 0, by3.Length));

            HL7message b = new HL7message();
            string result="";
            b.sendmessage(ref by3, hl7server, hl7port, ref result, msg);

            if (result == "99")
            MessageBox.Show("�˷����");
            else
              MessageBox.Show("�˷�ʧ��");
          return "0";
              }

                    MessageBox.Show("�޴�" + Sslbx);
                    return "0";

                }

                MessageBox.Show("ʶ�����Ͳ���Ϊ��");
                return "0";

            }
        
    }
}
