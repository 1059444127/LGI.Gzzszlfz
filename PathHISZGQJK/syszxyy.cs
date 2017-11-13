using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using ZgqClassPub;
using HL7;

namespace PathHISZGQJK
{
    /// <summary>
    /// 邵阳市中心医院--费用确认接口--HL7
    /// </summary>
    class syszxyy
    {
        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {
            string hl7server = f.ReadString("savetohis", "hl7server", "");
            string hl7port = f.ReadString("savetohis", "hl7port", "");
            string debug = f.ReadString("savetohis", "debug", "");
            blhgy = blh;

            string patid = "";
           odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }


            DataTable hl7xx = aa.GetDataTable("select * from T_HL7_sqd where substring(F_sqxh,0,charindex('^',F_sqxh+'^',0)) ='" + bljc.Rows[0]["F_SQXH"].ToString() + "'", "HL7");
          //  DataTable hl7xx = aa.GetDataTable("select * from T_HL7_sqd where F_sqxh='" + bljc.Rows[0]["F_SQXH"].ToString() + "'", "HL7");
            


            if (hl7xx.Rows.Count < 1)
            {
                MessageBox.Show("数据错误，主表无记录");
               log.WriteMyLog(blh + "数据错误，主表无记录");
                return;
            }
            if (hl7xx.Rows[0]["F_DJBZ"].ToString().Trim() != "1" )
            {
                aa.ExecuteSQL("update T_HL7_sqd  set F_DJBZ='1'  where substring(F_sqxh,0,charindex('^',F_sqxh+'^',0)) ='" + bljc.Rows[0]["F_SQXH"].ToString() + "'");
            }



            if (hl7xx.Rows[0]["F_FYQRBJ"].ToString().Trim() != "1")
            {

                string pid = hl7xx.Rows[0]["F_PID"].ToString().Trim();
                string pv1 = hl7xx.Rows[0]["F_PV1"].ToString().Trim();
                string orc = hl7xx.Rows[0]["F_ORC"].ToString().Trim();
                string obr = hl7xx.Rows[0]["F_OBR"].ToString().Trim();


                ///邵阳市中心医院 回传需转成gbk编码
                string XXX = @"^~\&";
                string s12message = "MSH|" + XXX + "|BL|LOGENE|PT||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + hl7xx.Rows[0]["F_messageid"].ToString() + "|P|2.4|||||gbk\r";

                s12message = s12message + pid + "\r";
                s12message = s12message + pv1 + "\r";
                string[] orc1 = orc.Split('|');
                string orc2 = "ORC|SC";
                for (int x = 2; x < orc1.Length; x++)
                {
                    if (x == 5)
                    {
                        orc2 = orc2 + "|SC";
                        continue;
                    }
                    if (x == 25)
                    {
                        orc2 = orc2 + "|1";
                        continue;
                    }
                    orc2 = orc2 + "|" + orc1[x];
                }

                s12message = s12message + orc2 + "\r";

                string[] obr2 = obr.Split('|');
                string obr1 = "";
                obr2[27] = "1";

                obr2[23] = obr2[23].Split('^')[0] + "^1";
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
                if (debug == "1")
                    log.WriteMyLog("send to server " + hl7server + ":" + hl7port + " message:" + System.Text.Encoding.GetEncoding("gbk").GetString(by3, 0, by3.Length));

                HL7message b = new HL7message();
                string result = "";
                string errMsg = "";
                b.sendmessage(ref by3, hl7server, hl7port, ref result,ref errMsg, debug);

                if (result == "99")
                {
                    aa.ExecuteSQL("update  T_HL7_sqd   set  F_FYQRBJ='1' where F_sqxh ='" + hl7xx.Rows [0]["F_sqxh"].ToString().Trim() + "'");
                }
                else
                {
                   
                    MessageBox.Show(blh+",费用确认信息回传失败");
                    log.WriteMyLog("费用确认信息回传失败" + errMsg);
                }
            }


        }
    }
}
   