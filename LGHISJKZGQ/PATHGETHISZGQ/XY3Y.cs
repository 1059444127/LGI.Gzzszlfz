using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OracleClient;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class XY3Y
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string xy3yxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx == "���뵥��")
            {
                LGHISJKZGQ.WebXYCX.WSGeneral xx = new LGHISJKZGQ.WebXYCX.WSGeneral();
                LGHISJKZGQ.WebXYCX.ExamOrder exam = new LGHISJKZGQ.WebXYCX.ExamOrder();
                xx.GetExamOrder(Ssbz, "", "", out exam);

                if (exam == null)
                {
                    MessageBox.Show("�޴����뵥");
                    return "0";
                }

                LGHISJKZGQ.WebXYCX.PatBasicInfo brxx = new LGHISJKZGQ.WebXYCX.PatBasicInfo();
                xx.GetPatientInfo(exam.PatientID, out brxx);

                LGHISJKZGQ.WebXYCX.ExamOrderItem jcxm = new LGHISJKZGQ.WebXYCX.ExamOrderItem();

                if (brxx == null)
                {
                    MessageBox.Show("�޴����뵥");
                    return "0";
                }
                string brlb = exam.PatientSource.ToString().Trim();
                if (brlb == "2")
                {
                    brlb = "סԺ";
                }
                else
                {
                    brlb = "����";
                }
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                xml = xml + "���˱��=" + (char)34 + brxx.PatientID.ToString().Trim() + (char)34 + " ";
                xml = xml + "����ID=" + (char)34 + (char)34 + " ";
                xml = xml + "�������=" + (char)34 + exam.ExamNo + (char)34 + " ";
                if (brlb == "����")
                {
                    xml = xml + "�����=" + (char)34 + brxx.PatientID.ToString().Trim() + (char)34 + " ";
                    xml = xml + "סԺ��=" + " " + (char)34 + " ";
                }
                else
                {
                    xml = xml + "�����=" + (char)34 + " " + (char)34 + " ";
                    xml = xml + "סԺ��=" + (char)34 + brxx.InpNo.ToString().Trim() + (char)34 + " ";
                }
                
                
                xml = xml + "����=" + (char)34 + brxx.PatName.ToString().Trim() + (char)34 + " ";
                

                xml = xml + "�Ա�=" + (char)34 + brxx.PatSex.ToString().Trim() + (char)34 + " ";
                string nl = "0��";
                try
                {
                     nl = datediff(DateTime.Now,Convert.ToDateTime(brxx.PatDOB));
                }
                catch
                { 
                }
                xml = xml + "����=" + (char)34 + nl + (char)34 + " ";
                string hy = "";
                if (brxx.MaritalStatus == "M")
                {
                    hy = "�ѻ�";
                }

                if (brxx.MaritalStatus == "B")
                {
                    hy = "δ��";
                }

                if (brxx.MaritalStatus == "D")
                {
                    hy = "����";
                }

                if (brxx.MaritalStatus == "W")
                {
                    hy = "ɥż";
                }

                if (brxx.MaritalStatus == "O")
                {
                    hy = "����";
                }

                if (brxx.MaritalStatus == null)
                {
                    hy = "";
                }

                xml = xml + "����=" + (char)34 + hy + (char)34 + " ";
                try
                {
                    xml = xml + "��ַ=" + (char)34 + "��ַ:" + brxx.MailingAddress.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "��ַ=" + (char)34 + "��ַ:" + "" + (char)34 + " ";
                }
                try
                {
                    xml = xml + "�绰=" + (char)34 + "�绰:" + brxx.NextOfKinPhone.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�绰=" + (char)34 + "�绰:" + " " + (char)34 + " ";
                }

                 try
                {
                    xml = xml + "����=" + (char)34 + exam.WardName.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                     xml = xml + "����=" + (char)34 +"" + (char)34 + " ";
                }
                
               
                
                try
                {
                    xml = xml + "����=" + (char)34 + exam.BedNo + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                }
                try
                {
                    xml = xml + "���֤��=" + (char)34 + brxx.IDNO.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "���֤��=" + (char)34 + " " + (char)34 + " ";
                }
                try
                {
                    xml = xml + "����=" + (char)34 + brxx.Nation.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + " " + (char)34 + " ";
                }
                try
                {
                    xml = xml + "ְҵ=" + (char)34 + brxx.JobName.ToString().Trim() + (char)34 + " ";
                }
                catch

                {
                    xml = xml + "ְҵ=" + (char)34 + " " + (char)34 + " ";
                }
                xml = xml + "�ͼ����=" + (char)34 + exam.ReqDeptName.ToString().Trim() + (char)34 + " ";
                xml = xml + "�ͼ�ҽ��=" + (char)34 + exam.ReqPhysician.ToString().Trim() + (char)34 + " ";
                //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
                //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
                xml = xml + "�շ�=" + (char)34 + (char)34 + " ";
                string bbmc = "";
                for (int i = 0; i < exam.ExamItems.Length; i++)
                {
                    bbmc = bbmc + exam.ExamItems[i].ExamItem+",";
                    

                }
                if (bbmc.Length > 1)
                {
                    bbmc = bbmc.Substring(0, bbmc.Length - 1);
                }
                xml = xml + "�걾����=" + (char)34 + bbmc + (char)34 + " ";
                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                xml = xml + "ҽ����Ŀ=" + (char)34 + (char)34 + " ";
                xml = xml + "����1=" + (char)34 + (char)34 + " ";
                xml = xml + "����2=" + (char)34 + (char)34 + " ";
                xml = xml + "�ѱ�=" + (char)34 + brxx.ChargeType.ToString().Trim() + (char)34 + " ";
                xml = xml + "�������=" + (char)34 + brlb + (char)34 + " ";
                xml = xml + "/>";
                xml = xml + "<�ٴ���ʷ><![CDATA[" + " " + "]]></�ٴ���ʷ>";
                xml = xml + "<�ٴ����><![CDATA[" + exam.ClinDiag.ToString().Trim() + "]]></�ٴ����>";
                xml = xml + "</LOGENE>";

                return xml;




            }

            MessageBox.Show(Sslbx + "����δ���ã�");
            if (Debug == "1")
                log.WriteMyLog(Sslbx + "����δ���ã�");
            return "0";
        }

        public static DataSet RunProc_ds(string procName, string dbstring, OracleParameter[] prams)
        {
            DataSet ds = new DataSet();
            //SqlCommand cmd = CreateCommand(procName, prams, 0, dbstring);
            OracleConnection conn = new OracleConnection(dbstring);
            OracleCommand cmd = new OracleCommand(procName, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(prams[0]);
            cmd.Parameters.Add(prams[1]);
            cmd.Parameters.Add(prams[2]);

            try
            {
                OracleDataAdapter myda = new OracleDataAdapter(cmd);
                myda.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
                log.WriteMyLog(ex.Message.ToString());
                return null;
            }
            finally
            {
                conn.Close();

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
