using System;
using System.Collections.Generic;
using System.Text;
using HL7;
using readini;
using dbbase;
using System.Data;
using System.Windows.Forms;

namespace PathHISZGQJK
{
    class szszyyhl7
    {
        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {

            string debug = f.ReadString("savetohis", "debug", "");
            if (debug == "1")
                log.WriteMyLog("确费接口");

            string hl7server = f.ReadString("savetohis", "hl7server", "");
            string hl7port = f.ReadString("savetohis", "hl7port", "");

            string msg = f.ReadString("savetohis", "msg", "");
            string lry = f.ReadString("yh", "yhmc", "");
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

            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim();


            DataTable hl7xx = new DataTable();

            hl7xx = aa.GetDataTable("select * from T_HL7_sqd where  F_sqxh ='" + sqxh + "' ", "HL7");

            if (hl7xx.Rows.Count < 1)
            {
                if (msg == "1")
                    MessageBox.Show("确认费用失败，申请单表无记录");
                log.WriteMyLog(blh + "~确认费用失败，申请单表无记录");
                return;
            }
            if (hl7xx.Rows[0]["F_DJBZ"].ToString().Trim() != "1")
            {
                if (debug == "1")
                    log.WriteMyLog("update T_HL7_sqd  set F_DJBZ='1'  where F_hjsf='" + sfxh + "' and F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'  and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");

                aa.ExecuteSQL("update T_HL7_sqd  set F_DJBZ='1'  where F_hjsf='" + sfxh + "' and F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'  and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
            }



            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已登记" || bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {
                string wczt = "SC";
                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已登记")
                    wczt = "SC";
                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                    wczt = "CM";
                try
                {
                    string pid = hl7xx.Rows[0]["F_PID"].ToString().Trim();
                    string pv1 = hl7xx.Rows[0]["F_PV1"].ToString().Trim();
                    string orc = hl7xx.Rows[0]["F_ORC"].ToString().Trim();
                    string obr = hl7xx.Rows[0]["F_OBR"].ToString().Trim();



                    string XXX = @"^~\&";
                    string s12message = "MSH|" + XXX + "|BLXT|BLXT|MediII|MediII|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + hl7xx.Rows[0]["F_messageid"].ToString() + "|P|2.4\r";

                    s12message = s12message + pid + "\r";
                    s12message = s12message + pv1 + "\r";
                    string[] orc1 = orc.Split('|');
                    string[] obr1 = obr.Split('|');

                    string orc2 = "ORC|SC";
                    for (int x = 2; x < orc1.Length; x++)
                    {
                        if (x == 5)
                        {
                            orc2 = orc2 + "|" + wczt;
                            continue;

                        }
                        if (x == 9)
                        {
                            orc2 = orc2 + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            continue;
                        }
                        if (x == 10)
                        {
                            orc2 = orc2 + "|" + lry;
                            continue;
                        }
                        if (x == 17)
                        {
                            orc2 = orc2 + "|" + hl7xx.Rows[0]["F_KSXX"].ToString().Trim();
                            continue;
                        }

                        orc2 = orc2 + "|" + orc1[x];
                    }

                    s12message = s12message + orc2 + "\r";

                    if (wczt == "CM")
                    {
                        string obr2 = "";
                        for (int x = 0; x < obr1.Length; x++)
                        {
                            if (x == 8)
                            {
                                obr2 = obr2 + "|" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                                continue;
                            }
                            if (x == 10)
                            {
                                obr2 = obr2 + "|" + bljc.Rows[0]["F_bgys"].ToString();
                                continue;
                            }
                            if (x == 34)
                            {
                                obr2 = obr2 + "|^" + bljc.Rows[0]["F_SHYS"].ToString();
                                continue;
                            }
                            obr2 = obr2 + "|" + obr1[x];

                        }
                        s12message = s12message + obr2 + "\r";
                    }
                    else
                        s12message = s12message + obr + "\r";

                    byte[] by2 = System.Text.Encoding.UTF8.GetBytes(s12message);

                    byte[] by3 = new byte[by2.Length + 3];
                    by3[0] = 11;
                    Array.Copy(by2, 0, by3, 1, by2.Length);
                    by3[by3.Length - 2] = 28;
                    by3[by3.Length - 1] = 13;
                    if (debug == "1")
                        log.WriteMyLog("send to server " + hl7server + ":" + hl7port + " message:" + System.Text.Encoding.UTF8.GetString(by3, 0, by3.Length));

                    HL7message b = new HL7message();
                    string result = "";
                    b.sendmessage(ref by3, hl7server, hl7port, ref result, debug);

                    if (result == "99")
                    {
                        // aa.ExecuteSQL("update  T_HL7_sqd   set  F_FYQRBJ='1' where F_hjsf='" + sfxh + "' and F_sqxh ='" + sqxh + "' and  F_brid='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'   and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
                    }
                    else
                    {
                        result = "00";
                        if (msg == "1")
                            MessageBox.Show(blh + ",状态回传失败:" + result);
                        log.WriteMyLog(blh + "~状态回传失败失败:" + result);
                    }
                }
                catch (Exception ee2)
                {
                    log.WriteMyLog("状态回传异常：" + ee2.Message);

                }
                //}
                //else
                //{
                //    if (debug == "1")
                //        log.WriteMyLog("F_FYQRBJ=1，不处理");

                //}


            }
        }
    }
}
