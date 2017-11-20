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

            if (Sslbx == "申请单号")
            {
                LGHISJKZGQ.WebXYCX.WSGeneral xx = new LGHISJKZGQ.WebXYCX.WSGeneral();
                LGHISJKZGQ.WebXYCX.ExamOrder exam = new LGHISJKZGQ.WebXYCX.ExamOrder();
                xx.GetExamOrder(Ssbz, "", "", out exam);

                if (exam == null)
                {
                    MessageBox.Show("无此申请单");
                    return "0";
                }

                LGHISJKZGQ.WebXYCX.PatBasicInfo brxx = new LGHISJKZGQ.WebXYCX.PatBasicInfo();
                xx.GetPatientInfo(exam.PatientID, out brxx);

                LGHISJKZGQ.WebXYCX.ExamOrderItem jcxm = new LGHISJKZGQ.WebXYCX.ExamOrderItem();

                if (brxx == null)
                {
                    MessageBox.Show("无此申请单");
                    return "0";
                }
                string brlb = exam.PatientSource.ToString().Trim();
                if (brlb == "2")
                {
                    brlb = "住院";
                }
                else
                {
                    brlb = "门诊";
                }
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                xml = xml + "病人编号=" + (char)34 + brxx.PatientID.ToString().Trim() + (char)34 + " ";
                xml = xml + "就诊ID=" + (char)34 + (char)34 + " ";
                xml = xml + "申请序号=" + (char)34 + exam.ExamNo + (char)34 + " ";
                if (brlb == "门诊")
                {
                    xml = xml + "门诊号=" + (char)34 + brxx.PatientID.ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + " " + (char)34 + " ";
                }
                else
                {
                    xml = xml + "门诊号=" + (char)34 + " " + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + brxx.InpNo.ToString().Trim() + (char)34 + " ";
                }
                
                
                xml = xml + "姓名=" + (char)34 + brxx.PatName.ToString().Trim() + (char)34 + " ";
                

                xml = xml + "性别=" + (char)34 + brxx.PatSex.ToString().Trim() + (char)34 + " ";
                string nl = "0岁";
                try
                {
                     nl = datediff(DateTime.Now,Convert.ToDateTime(brxx.PatDOB));
                }
                catch
                { 
                }
                xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                string hy = "";
                if (brxx.MaritalStatus == "M")
                {
                    hy = "已婚";
                }

                if (brxx.MaritalStatus == "B")
                {
                    hy = "未婚";
                }

                if (brxx.MaritalStatus == "D")
                {
                    hy = "离异";
                }

                if (brxx.MaritalStatus == "W")
                {
                    hy = "丧偶";
                }

                if (brxx.MaritalStatus == "O")
                {
                    hy = "其他";
                }

                if (brxx.MaritalStatus == null)
                {
                    hy = "";
                }

                xml = xml + "婚姻=" + (char)34 + hy + (char)34 + " ";
                try
                {
                    xml = xml + "地址=" + (char)34 + "地址:" + brxx.MailingAddress.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "地址=" + (char)34 + "地址:" + "" + (char)34 + " ";
                }
                try
                {
                    xml = xml + "电话=" + (char)34 + "电话:" + brxx.NextOfKinPhone.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "电话=" + (char)34 + "电话:" + " " + (char)34 + " ";
                }

                 try
                {
                    xml = xml + "病区=" + (char)34 + exam.WardName.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                     xml = xml + "病区=" + (char)34 +"" + (char)34 + " ";
                }
                
               
                
                try
                {
                    xml = xml + "床号=" + (char)34 + exam.BedNo + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                try
                {
                    xml = xml + "身份证号=" + (char)34 + brxx.IDNO.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "身份证号=" + (char)34 + " " + (char)34 + " ";
                }
                try
                {
                    xml = xml + "民族=" + (char)34 + brxx.Nation.ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                }
                try
                {
                    xml = xml + "职业=" + (char)34 + brxx.JobName.ToString().Trim() + (char)34 + " ";
                }
                catch

                {
                    xml = xml + "职业=" + (char)34 + " " + (char)34 + " ";
                }
                xml = xml + "送检科室=" + (char)34 + exam.ReqDeptName.ToString().Trim() + (char)34 + " ";
                xml = xml + "送检医生=" + (char)34 + exam.ReqPhysician.ToString().Trim() + (char)34 + " ";
                //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                xml = xml + "收费=" + (char)34 + (char)34 + " ";
                string bbmc = "";
                for (int i = 0; i < exam.ExamItems.Length; i++)
                {
                    bbmc = bbmc + exam.ExamItems[i].ExamItem+",";
                    

                }
                if (bbmc.Length > 1)
                {
                    bbmc = bbmc.Substring(0, bbmc.Length - 1);
                }
                xml = xml + "标本名称=" + (char)34 + bbmc + (char)34 + " ";
                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                xml = xml + "费别=" + (char)34 + brxx.ChargeType.ToString().Trim() + (char)34 + " ";
                xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                xml = xml + "/>";
                xml = xml + "<临床病史><![CDATA[" + " " + "]]></临床病史>";
                xml = xml + "<临床诊断><![CDATA[" + exam.ClinDiag.ToString().Trim() + "]]></临床诊断>";
                xml = xml + "</LOGENE>";

                return xml;




            }

            MessageBox.Show(Sslbx + "类型未设置！");
            if (Debug == "1")
                log.WriteMyLog(Sslbx + "类型未设置！");
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
                    diff = "0岁";
                }
                else
                {
                    int nly = (tm1.Year - tm2.Year) * 12 + (tm1.Month - tm2.Month);
                    if (nly > 12)
                    {
                        int xxxx = nly / 12;
                        diff = Convert.ToString(xxxx) + "岁";
                    }
                    else
                    {
                        TimeSpan ts = tm1 - tm2;
                        if (ts.Days < 31)
                        {
                            diff = Convert.ToString(ts.Days) + "天";
                        }
                        else
                        {
                            diff = Convert.ToString(nly) + "月";
                        }
                    }

                }
            }
            catch
            {
                diff = "0岁";
            }
            return diff;
        }


    }
}
