using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using readini;
using System.Windows.Forms;
using System.Data;


namespace HL7
{
    class hl7jm
    {
        private static readini.IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        public static void messagemain(string msg,string hl7server,string hl7port,ref byte[] ack)
        {

            HL7.readhl7 adtread = new HL7.readhl7();
            string[] aaa = adtread.Adt01(msg);
           // netStream = client.GetStream();
            string hfdm = "";


            hfdm = "ORR^O02";
            //回复消息
            byte[] by2 = System.Text.Encoding.UTF8.GetBytes(adtread.MSH[0].ToString() + "|" + adtread.MSH[1].ToString() + "|BL||EMR||" + adtread.MSH[6].ToString() + "||" + hfdm + "|ACK" + adtread.MSH[9].ToString() + "|" + adtread.MSH[10].ToString() + "|" + adtread.MSH[11].ToString() + "||||||utf-8\r" + "MSA|AA|" + adtread.MSH[9].ToString() + "|0|||\r");
            byte[] by3 = new byte[by2.Length + 3];
            by3[0] = 11;
            Array.Copy(by2, 0, by3, 1, by2.Length);
            by3[by3.Length - 2] = 28;
            by3[by3.Length - 1] = 13;
            ack = by3;
            try
            {
                //删除申请单
                if (adtread.MSH[8] == "ORM^O01" && adtread.ORC[1] == "CA")
                {
                   
                    //aa.ExecuteSQL("delete from T_HL7_SQD where F_sqxh='" + adtread.ORC[4].ToString().Trim() + "'");
                    aa.ExecuteSQL("delete from T_HL7_SQD where F_yzid='" + adtread.OBR[45].Replace(@"\r", "").Split('^')[3] + "'");
                    aa.ExecuteSQL("delete from T_hl7_bblb where F_yzid='" + adtread.OBR[45].Replace(@"\r", "").Split('^')[3] + "'");

                    return;
                }

                //存档1 ORM^O01
                if (adtread.MSH[8] == "ORM^O01" && adtread.ORC[1] == "NW")
                {
                   
                    string[] fields = new string[42];
                    fields[0] = "F_brid";
                    fields[1] = "F_brxm";
                    fields[2] = "F_brsr";
                    fields[3] = "F_brxb";
                    fields[4] = "F_brdz";
                    fields[5] = "F_brdh";
                    fields[6] = "F_brly";
                    fields[7] = "F_bqxx";
                    fields[8] = "F_ksxx";
                    fields[9] = "F_zzys";
                    fields[10] = "F_zyh";
                    fields[11] = "F_jzsj";
                    fields[12] = "F_brlb";
                    fields[13] = "F_xm";
                    fields[14] = "F_xb";
                    fields[15] = "F_nl";
                    fields[16] = "F_bq";
                    fields[17] = "F_ch";
                    fields[18] = "F_messageid";
                    fields[19] = "F_mzh";
                    fields[20] = "F_sqxh";
                    fields[21] = "F_yzid";

                    fields[22] = "F_sqsj";
                    fields[23] = "F_sjys";
                    fields[24] = "F_sjks";
                    fields[25] = "F_zxks";

                    fields[26] = "F_yzxm";
                    fields[27] = "F_sldw";
                    fields[28] = "F_je";
                    fields[29] = "F_fph";
                    fields[30] = "F_hjsf";
                    fields[31] = "F_PID";
                    fields[32] = "F_PV1";
                    fields[33] = "F_ORC";
                    fields[34] = "F_OBR";
                    fields[35] = "F_barcode";
                    fields[36] = "F_bbmc";
                    fields[37] = "F_lczd";
                    fields[38] = "F_MzhOrZyh";
                    fields[39] = "F_FB";
                    fields[40] = "F_rysj";
                    fields[41] = "F_lczl";
                    string[] values = new string[42];


                    ////PID
                    values[0] = adtread.PID[3].Split('^')[0];
                    values[1] = adtread.PID[5];
                    values[2] = adtread.PID[7];
                    values[3] = adtread.PID[8];
                    values[4] = adtread.PID[11];
                    values[5] = adtread.PID[13];

                    //PV1
                    values[6] = adtread.PV1[2];
                    values[7] = adtread.PV1[3];
                    //
                    values[38] = adtread.PV1[19];
                    values[39] = adtread.PV1[20];
                    values[40] = adtread.PV1[44];

                    //OBR
                    values[8] = adtread.OBR[18] + "|" + adtread.OBR[19];

                    string bbmc = adtread.OBR[39];
                    int start = bbmc.IndexOf("送检标本#");

                    if (bbmc.IndexOf("送检标本#") > 0)
                    {
                        bbmc = bbmc.Substring(start + 5);
                        if (bbmc.IndexOf("$$") > 0)
                        {
                            bbmc = bbmc.Substring(0, bbmc.IndexOf("$$"));
                        }
                        else
                        {
                            bbmc = "";
                        }
                    }
                    else
                    {
                        bbmc = "";
                    }
                    string[] bblb = bbmc.Split(';');

                    bbmc = bblb[0];
                    //bbmc = adtread.OBR[27].Substring(adtread.OBR[27].IndexOf("送检标本#") + 5, adtread.OBR[27].IndexOf("送检标本#").ToString().Length - adtread.OBR[27].IndexOf("送检标本#").ToString().Length);

                    string barcode = "";


                    if (bbmc.Split('/').Length >= 3)
                    {
                        barcode = bbmc.Split('/')[2];
                        bbmc = bbmc.Split('/')[0] + "/" + bbmc.Split('/')[1];
                        values[35] = barcode;
                        values[36] = bbmc;
                    }
                    else
                    {
                        values[35] = "";
                        values[36] = bbmc;
                       
                    }
                    //MessageBox.Show("1");
                    /////zgq---bbmc-lczd-lczl
                    //string NTE = adtread.NTE[3];
                    //MessageBox.Show(NTE);
                    //string [] nte_1 = NTE.Replace("$$", "$").Split('$');
                    //MessageBox.Show(nte_1[0]);
                    //string lczd_1 = nte_1[0].Split('#')[1].ToString();
                    //MessageBox.Show("7");
                    //string bbmc_1 = nte_1[1].Split('#')[1].ToString() + ";" + nte_1[0].Split('#')[2].ToString() + ";" + nte_1[0].Split('#')[3].ToString();
                    ////string cqbw_1 = nte_1[1].Split('#')[1].ToString() + ";" + nte_1[0].Split('#')[2].ToString() + ";" + nte_1[0].Split('#')[3].ToString();
                    //string lczl_1 = nte_1[1].Split('#')[4].ToString();
                    //MessageBox.Show("9");
                    //bbmc_1 = bbmc_1.Replace(";;", ";");
                    //values[36] = bbmc_1;
                    //values[37] = lczd_1;
                    //values[41] = lczl_1;
                    values[41] ="";
                    values[9] = "";
                    //住院号
                    if (adtread.PV1[2].Trim() == "I")
                    {
                        values[38] = adtread.PV1[19];
                    }
                    else
                    {
                        values[10] = "";
                    }
                    //门诊号
                    if (adtread.PV1[2].Trim() == "O" || adtread.PV1[2].Trim() == "P")
                    {
                      
                        values[19] = adtread.PV1[19];
                    }
                    else
                    {
                        values[19] = "";
                    }
                  
                    values[11] = "";


                    if (adtread.PV1[2].Trim() == "I") values[12] = "住院";
                    if (adtread.PV1[2].Trim() == "O") values[12] = "门诊";
                    if (adtread.PV1[2].Trim() == "P") values[12] = "体检";
                    if (adtread.PV1[2].Trim() == "E") values[12] = "急诊";


                    //values[15] = "住院";

                    values[13] = adtread.PID[5];
                    if (adtread.PID[5].IndexOf('^') > -1)
                    {
                        values[13] = adtread.PID[5].Substring(0, adtread.PID[5].IndexOf('^'));
                    }


                    values[14] = "其他";
                    if (adtread.PID[8].Trim() == "M") values[14] = "男";
                    if (adtread.PID[8].Trim() == "F") values[14] = "女";
                 
                    try
                    {
                        DateTime tm1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime tm2 = Convert.ToDateTime(adtread.PID[7].Substring(0, 4) + "-" + adtread.PID[7].Substring(4, 2) + "-" + adtread.PID[7].Substring(6, 2));
                        values[15] = datediff(tm1, tm2);
                    }
                    catch
                    {
                        values[15] = "0岁";
                    }



                    values[16] = "";
                    values[17] = "";
                    if (adtread.PV1[3].IndexOf('^') > -1)
                    {
                        try
                        {
                            values[16] = adtread.PV1[3].Split('^')[0];
                            values[17] = adtread.PV1[3].Split('^')[2];
                        }
                        catch
                        { }
                    }
                    
                    values[18] = adtread.MSH[9];
                   
                    //ORC
                    values[20] = adtread.ORC[2];
                    values[21] = adtread.ORC[4];//收费小序号
                  //  values[21] = adtread.OBR[45].Replace(@"\r", "").Split('^')[3];
                    
                    values[22] = adtread.OBR[6];
                    values[23] = adtread.ORC[12];
                    values[24] = adtread.OBR[18]+"|"+adtread.OBR[19];
                    values[25] = adtread.OBR[20] + "|" + adtread.OBR[21];
                
                    values[26] = adtread.OBR[4];
                    values[27] = adtread.OBR[9];

                    values[28] = adtread.OBR[23];
                    values[29] = adtread.OBR[46].Replace(@"\r", "");
                    values[30] = adtread.OBR[45].Replace(@"\r", "");
                    try
                    {
                        //NTE
                        string nte = adtread.OBR[3];
                        values[37] = nte.Split('$')[0].Split('#')[1].ToString();
                    }
                    catch
                    {
                        values[37] = "";
                    }
                    string pid = "";
                    for (int i = 0; i < adtread.PID.Length; i++)
                    {
                        pid = pid + adtread.PID[i].ToString() + "|";
                    }
               
                    pid = pid.Replace(@"\r", "");
                    string pv1 = "";
                    for (int j = 0; j < adtread.PV1.Length; j++)
                    {
                        pv1 = pv1 + adtread.PV1[j].ToString() + "|";
                    }
                    string orc = "";
                    for (int ii = 0; ii < adtread.ORC.Length; ii++)
                    {
                        orc = orc + adtread.ORC[ii].ToString() + "|";
                    }
                    orc = orc.Replace(@"\r", "");
                    string obr = "";
                    //log.WriteMyLog(adtread.OBR.Length.ToString());
                    for (int jj = 0; jj < adtread.OBR.Length; jj++)
                    {
                        //log.WriteMyLog(jj.ToString()+" "+adtread.OBR[jj].ToString());
                        obr = obr + adtread.OBR[jj].ToString() + "|";
                    }
                  
                    pv1 = pv1.Replace(@"\r", "");
                    values[31] = pid;
                    values[32] = pv1;
                    values[33] = orc;
                    values[34] = obr;
                  

                    DataTable hl7check = aa.GetDataTable("select * from T_HL7_SQD where F_messageid='" + adtread.MSH[9] + "'", "checkhl7");

                    if (hl7check.Rows.Count > 0)
                    {
                        log.WriteMyLog("重发消息" + adtread.MSH[9] + "，不处理！");
                        //MessageBox.Show("重发消息");
                    }
                    else
                    {
                       
                        aa.insertsql("T_HL7_SQD", ref fields, ref values);
                        
                        string[] bbfds = new string[6];
                        string[] bbvas = new string[6];
                        bbfds[0] = "F_tmh";
                        bbfds[1] = "F_bbmc";
                        bbfds[2] = "F_bbbw";
                        bbfds[3] = "F_sl";

                        bbfds[4] = "F_sqdh";
                        bbfds[5] = "F_yzid";

                        bbvas[4] = values[20];
                        bbvas[5] = values[21];

                        for (int i = 0; i < bblb.Length; i++)
                        {
                            bbvas[0] = bblb[i].Split('/')[2];
                            bbvas[1] = bblb[i].Split('/')[1];
                            bbvas[2] = bblb[i].Split('/')[0];
                            bbvas[3] = bblb[i].Split('/')[3];
                            aa.insertsql("T_hl7_bblb", ref bbfds, ref bbvas);
                        }
                    }
                    //hl7sub.sendack(ref by3, hl7server,hl7port);
                    //netStream.Write(by3, 0, by3.Length);
                    //netStream.Flush();
                    //log.WriteMyLog("send to server " + f.ReadString("HL7", "Server", "") + ":" + f.ReadInteger("HL7", "Port", 8000).ToString() + " message:" + System.Text.Encoding.Default.GetString(by3, 0, by3.Length));
                }

            }
            catch (Exception ex)
            {
                hfdm = "ORR^O02";
                //回复消息
                by2 = System.Text.Encoding.UTF8.GetBytes(adtread.MSH[0].ToString() + "|" + adtread.MSH[1].ToString() + "|BL||EMR||" + adtread.MSH[6].ToString() + "||" + hfdm + "|ACK" + adtread.MSH[9].ToString() + "|" + adtread.MSH[10].ToString() + "|" + adtread.MSH[11].ToString() + "||||||utf-8\r" + "MSA|AE|ACK2" + adtread.MSH[9].ToString() + "|" + ex.Message.ToString() + "||\r");
                by3 = new byte[by2.Length + 3];
                by3[0] = 11;
                Array.Copy(by2, 0, by3, 1, by2.Length);
                by3[by3.Length - 2] = 28;
                by3[by3.Length - 1] = 13;
                ack = by3;
            }


            //if (adtread.MSH[8] == "ACK")
            //{
            //    string[] fields = new string[3];
            //    fields[0] = "F_messageid";
            //    fields[1] = "F_zt";
            //    fields[2] = "F_bz";
            //    string[] values = new string[3];
            //    string messagetype = adtread.MSA[2];
            //    values[0] = "";
            //    if (messagetype.IndexOf("IP") > 0)
            //    {
            //        values[0] = messagetype.Substring(0, messagetype.IndexOf("IP"));
            //    }
            //    if (messagetype.IndexOf("BG") > 0)
            //    {
            //        values[0] = messagetype.Substring(0, messagetype.IndexOf("BG"));
            //    }
            //    if (messagetype.IndexOf("AA") > 0)
            //    {
            //        values[0] = messagetype.Substring(0, messagetype.IndexOf("AA"));
            //        log.WriteMyLog(messagetype+"检查完成ACK消息，不处理！");
            //        return;

            //    }
            //    if (messagetype.IndexOf("CM") > 0)
            //    {
            //        values[0] = messagetype.Substring(0, messagetype.IndexOf("CM"));
            //        log.WriteMyLog(messagetype + "报告审核ACK消息，不处理！");
            //        return;

            //    }
            //    if (messagetype.IndexOf("OC") > 0)
            //    {
            //        values[0] = messagetype.Substring(0, messagetype.IndexOf("OC"));
            //        log.WriteMyLog(messagetype + "取消检查ACK消息，不处理！");
            //        return;

            //    }
            //    if (adtread.MSA[1] == "AA")
            //    {
            //        values[1] = "发送完成";
            //        values[2] = "";
            //    }
            //    else
            //    {
            //        values[1] = "发送失败";
            //        values[2] = "HIS错误";
            //    }

            //    dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver2");
            //    if (values[0] == "")
            //    {
            //        log.WriteMyLog(adtread.MSA[2] + " HIS错误消息，不处理！");
            //    }
            //    else
            //    {
            //        if (aa.updatesql("T_jcxx2", ref fields, ref values, "F_messageid='" + values[0] + "'"))
            //        { }
            //        else
            //        {
            //            log.WriteMyLog(adtread.MSA[2] + "更新失败！");
            //        }
            //    }
            //}



        }
        private static string datediff(DateTime tm1, DateTime tm2)
        {
            string diff = "";
            
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
                        int xxx =Convert.ToInt32( nly / 12);
                        string xxxx = xxx.ToString();
                        diff = xxxx + "岁";
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
