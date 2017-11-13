
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
    /// 新疆石河子大学附属医院--费用确认接口--HL7
    /// </summary>
    class xjshzdxfsyy
    {

        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {

            string debug = f.ReadString("savetohis", "debug", "");
            if(debug=="1")
            log.WriteMyLog("确费接口");

            string hl7server = f.ReadString("savetohis", "hl7server", "");
            string hl7port = f.ReadString("savetohis", "hl7port", "");
           
            string msg = f.ReadString("savetohis", "msg", "");
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
         
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "体检")
            {
                PathHISJK.shz1fy ss = new PathHISJK.shz1fy();
                ss.shz1y(blh, yymc,debug);
                return;
            }

            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }



            if (bljc.Rows[0]["F_yzid"].ToString().Trim() == "")
            {
                log.WriteMyLog("无医嘱ID，不处理！");
                return;
            }

            string sfxh = "";
            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim().Split('^')[0];

            try
            {
                sfxh = bljc.Rows[0]["F_SQXH"].ToString().Trim().Split('^')[1];
            }
            catch
            {
                log.WriteMyLog("获取sfxh失败");
                return;
            }
            DataTable hl7xx = new DataTable();
           
                hl7xx = aa.GetDataTable("select * from T_HL7_sqd where  F_hjsf='" + sfxh + "'  and  F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "' and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'", "HL7");

            if(debug=="1")
                log.WriteMyLog("select * from T_HL7_sqd where  F_hjsf='" + sfxh + "'  and  F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "' and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
            //  DataTable hl7xx = aa.GetDataTable("select * from T_HL7_sqd where F_sqxh='" + bljc.Rows[0]["F_SQXH"].ToString() + "'", "HL7");
            


            if (hl7xx.Rows.Count < 1)
            {
                if(msg=="1")
                MessageBox.Show("确认费用失败，申请单表无记录");
                log.WriteMyLog(blh + "~确认费用失败，申请单表无记录");
                return;
            }
            if (hl7xx.Rows[0]["F_DJBZ"].ToString().Trim() != "1" )
            {
                if (debug == "1")
                    log.WriteMyLog("update T_HL7_sqd  set F_DJBZ='1'  where F_hjsf='" + sfxh + "' and F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'  and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
         
                aa.ExecuteSQL("update T_HL7_sqd  set F_DJBZ='1'  where F_hjsf='" + sfxh + "' and F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'  and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
            }



            if (hl7xx.Rows[0]["F_FYQRBJ"].ToString().Trim() != "1")
            {

                try
                {
                    string pid = hl7xx.Rows[0]["F_PID"].ToString().Trim();
                    string pv1 = hl7xx.Rows[0]["F_PV1"].ToString().Trim();
                    string orc = hl7xx.Rows[0]["F_ORC"].ToString().Trim();
                    string obr = hl7xx.Rows[0]["F_OBR"].ToString().Trim();



                    string XXX = @"^~\&";
                    string s12message = "MSH|" + XXX + "|BL|LOGENE|PT||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + hl7xx.Rows[0]["F_messageid"].ToString() + "|P|2.4|||||gbk\r";

                    s12message = s12message + pid + "\r";
                    s12message = s12message + pv1 + "\r";
                    string[] orc1 = orc.Split('|');
                    string[] obr2 = obr.Split('|');

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
                        if (x == 2)
                        {
                            orc2 = orc2 + "|" + hl7xx.Rows[0]["F_hjsf"].ToString().Trim();
                            continue;
                        }
                        if (x == 4)
                        {
                            orc2 = orc2 + "|" + orc1[4] + "^^" + orc1[2].Split('^')[0];
                            continue;
                        }
                        orc2 = orc2 + "|" + orc1[x];
                    }

                    s12message = s12message + orc2 + "\r";


                    string obr1 = "";
                    obr2[27] = "1";

                    obr2[23] = obr2[23].Split('^')[0] + "^1";
                    obr1 = "OBR||" + hl7xx.Rows[0]["F_hjsf"].ToString().Trim();
                    for (int x = 3; x < obr2.Length; x++)
                    {
                        if (x == 23)
                        {
                            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                                obr1 = obr1 + "|" + obr2[x].Split('^')[0].Replace("&", "").Trim() + "^1";
                            else
                                obr1 = obr1 + "|" + obr2[x].Split('^')[0].Replace("&", "").Trim() + "^0";
                        }
                        else
                            obr1 = obr1 + "|" + obr2[x];
                    }
                    s12message = s12message + obr1 + "\r";
                    byte[] by2 = System.Text.Encoding.UTF8.GetBytes(s12message);

                    byte[] by3 = new byte[by2.Length + 3];
                    by3[0] = 11;
                    Array.Copy(by2, 0, by3, 1, by2.Length);
                    by3[by3.Length - 2] = 28;
                    by3[by3.Length - 1] = 13;
                    if (debug == "1")
                        log.WriteMyLog("send to server " + hl7server + ":" + hl7port + " message:" + System.Text.Encoding.UTF8.GetString(by3, 0, by3.Length));

                    HL7message b = new HL7message();
                    string result = ""; string errMsg = "";
                    b.sendmessage(ref by3, hl7server, hl7port, ref result,ref errMsg, debug);

                    if (result == "99")
                    {
                        aa.ExecuteSQL("update  T_HL7_sqd   set  F_FYQRBJ='1' where F_hjsf='" + sfxh + "' and F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'   and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
                    }
                    else
                    {
                       
                        if (msg == "1")
                            MessageBox.Show(blh + ",费用确认信息回传失败:" + result);
                        log.WriteMyLog(blh + "~费用确认信息回传失败:" + result+"  " + errMsg);
                    }
                }
                catch(Exception  ee2)
                {
                    log.WriteMyLog("确费异常："+ee2.Message);

                }
            }
            else
            {
                if(debug=="1")
                log.WriteMyLog("F_FYQRBJ=1，不处理");

            }


        }
    }
}
   