using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Windows.Forms;
using System.Data.Common;
using System.IO;
using System.Data.SqlClient;
using System.Data.Odbc;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class ZZZXYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        
        //private odbcdb bb = new odbcdb("DSN=pathnet-his;UID=sa;PWD=zoneking", "", "");//his数据库

        
        
 string msg = f.ReadString("savetohis", "msg", "");

        public void pathtohis(string blh, string yymc)
        {
           
            string brlb = "";
            string localconstr=f.ReadString("savetohis", "localconstr", "");
            string hisconstr = f.ReadString("savetohis", "hisconstr", "");
           
            if (msg == "1")
                MessageBox.Show(hisconstr);
            odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");//logene数据库
            PathHISJK.ODBCHelper bb = new PathHISJK.ODBCHelper(hisconstr);

            DataTable bljc = new DataTable();

            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }


            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }


            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号，不处理！");
                return;
            }
            //MessageBox.Show(yymc + "start2");


            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") brlb = "IN";
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") brlb = "OU";
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "体检") brlb = "TJ";
            if (brlb == "")
            {
                log.WriteMyLog("非住院或门诊病人，不处理！");
                return;
            }
         
            if (bljc.Rows[0]["F_BLK"].ToString().Trim() == "TCT" && brlb == "TJ")
            {
                
                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已登记")
                {
                   
                    ChangeStatus(bljc, bb);
                }
            }
          
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" && brlb == "TJ")
            {
                SaveToHIS(bljc, bb);
            }

        }

        private void SaveToHIS(DataTable bljc, PathHISJK.ODBCHelper bb)
        {
            string blh = bljc.Rows[0]["F_BLH"] == null ? "" : bljc.Rows[0]["F_BLH"].ToString().Trim();
            string sqxh = bljc.Rows[0]["F_SQXH"] == null ? "" : bljc.Rows[0]["F_SQXH"].ToString().Trim();
            string xm = bljc.Rows[0]["F_XM"] == null ? "" : bljc.Rows[0]["F_XM"].ToString().Trim();
            string xb = bljc.Rows[0]["F_XB"] == null ? "" : bljc.Rows[0]["F_XB"].ToString().Trim();
            string bbmc = bljc.Rows[0]["F_BBMC"] == null ? "" : bljc.Rows[0]["F_BBMC"].ToString().Trim();
            string rysj = bljc.Rows[0]["F_RYSJ"] == null ? "" : bljc.Rows[0]["F_RYSJ"].ToString().Trim();
            string blzd = bljc.Rows[0]["F_BLZD"] == null ? "" : bljc.Rows[0]["F_BLZD"].ToString().Trim();
            string bgys = bljc.Rows[0]["F_BGYS"] == null ? "" : bljc.Rows[0]["F_BGYS"].ToString().Trim();
            string shys = bljc.Rows[0]["F_SHYS"] == null ? "" : bljc.Rows[0]["F_SHYS"].ToString().Trim();
            string bgrq = bljc.Rows[0]["F_BGRQ"] == null ? "" : bljc.Rows[0]["F_BGRQ"].ToString().Trim();

            mdjpg xx = new mdjpg();
            string bglj = "";
            xx.BMPTOJPG(blh, ref bglj, "CG", "0");
          
            string ftpserver = f.ReadString("hisftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("hisftp", "user", "ZKFTP").Replace("\0", "");
            string ftppwd = f.ReadString("hisftp", "pwd", "ZKFTP").Replace("\0", "");
            string txpath = f.ReadString("hisftp", "txpath", "PACSDATA").Replace("\0", "");
            string ftppath = Path.Combine(ftpserver, txpath);

            if (bglj != "")
            {
                FtpWeb fw = new FtpWeb(ftpserver, txpath, ftpuser, ftppwd);
                string status = "";
                fw.Upload(@"C:\temp\" + blh + @"\" + blh + @"_1.jpg", "", out status);
               
              string insertsql = string.Format("insert into dbo.T_SYN_ZK_CHECK(PACS_CheckID,CISID,PatientNameChinese,PatientSex,PatientBirthday,StudyType,StudyBodyPart,ClinicDiagnose,ClinicSymptom,ClinicAdvice,IMGStrings,StudyState,Check_Doc,Check_Date,Report_Doc,Report_Date,Audit_Doc,Audit_Date) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')", blh, sqxh, xm, xb, "", "GJ", bbmc, blzd, rysj, "", blh + @"_1.jpg", 5, "", "", bgys, bgrq, shys, bgrq);
              
            
                OdbcCommand cmd = bb.GetOdbcStringCommond(insertsql);
                if (msg=="1")
                MessageBox.Show(cmd.Connection.ConnectionString);
                bb.ExecuteNonQuery(cmd);
            }



            
        }

        private void ChangeStatus(DataTable bljc, PathHISJK.ODBCHelper bb)
        {
            int resultnum = 3;
            OdbcCommand cmd = bb.GetStoredProcCommond("{call Pro_Pacs_ReqStauts(?,?,?,?,?)}");
            bb.AddInParameter(cmd, "@Exam_No", DbType.String, bljc.Rows[0]["F_SQXH"].ToString().Trim());
            bb.AddInParameter(cmd, "@ID", DbType.String, bljc.Rows[0]["F_SQXH"].ToString().Trim());
            bb.AddInParameter(cmd, "@StudyType", DbType.String, "GJ");
            bb.AddInParameter(cmd, "@StudyState", DbType.Int32, 1);
            bb.AddOutParameter(cmd, "@result", DbType.Int32, resultnum);
            bb.ExecuteNonQuery(cmd);
            resultnum = Convert.ToInt32(bb.GetParameter(cmd, "@result").Value);

            switch (resultnum)
            {
                case 0:
                    log.WriteMyLog("回写成功！");
                    break;
                case 1:
                    log.WriteMyLog("参数错误");
                    break;
                case 2:
                    log.WriteMyLog("更新失败");
                    break;
                default:
                    log.WriteMyLog("未知原因");
                    break;
            }
        }
    }
}
