using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using ZgqClassPub;
using HL7;

namespace PathHISZGQJK
{
    /// <summary>
    /// 浙江省台州医院,延时执行，加入小蔡的回写
    /// </summary>
    class zjstzyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
   
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            //TZH 台州医院,EZH 恩泽医院
            string jgdm = f.ReadString("savetohis", "jgdm", "TZH").Replace("\0", "").Trim();
            string jgmc = f.ReadString("savetohis", "jgmc", "台州医院").Replace("\0", "").Trim();
            debug = f.ReadString("savetohis", "debug", "0").Replace("\0", "").Trim();

            string bgzt = "";
            string xdj = "";
            bglx = bglx.ToLower();
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;

            try
            {
            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                {
                    //取消审核动作
                    bgzt = "取消审核";
                }
            }
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }

                string bgzt2 = "";
                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                try
                {

                    if (bglx.ToLower().Trim() == "bd")
                    {
                        dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    }

                    if (bglx.ToLower().Trim() == "bc")
                    {
                        dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                        bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                    }
                    if (bglx.ToLower().Trim() == "cg")
                    {
                        bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                    }
                }
                catch
                {

                }
            //审核
                if (bgzt2 == "已审核" && bgzt != "取消审核")
                {
                    #region 审核报告回传数据中心
                    try
                    {
                        string brlb = jcxx.Rows[0]["F_BRLB"].ToString().Trim();
                        string jzh = jcxx.Rows[0]["F_MZH"].ToString().Trim();
                        if (brlb == "住院")
                        { brlb = "I"; jzh = jcxx.Rows[0]["F_ZYH"].ToString().Trim(); }
                        else if (brlb == "急诊") brlb = "E";
                        else brlb = "O";

                        string bgys = jcxx.Rows[0]["F_BGYS"].ToString().Trim();
                        string bgysgh = GetYHBH(bgys);

                        string shys = jcxx.Rows[0]["F_shYS"].ToString().Trim();
                        string shysgh = GetYHBH(shys);

                        string xb = jcxx.Rows[0]["F_XB"].ToString().Trim();
                        if (xb == "男") xb = "M";
                        else if (xb == "女") xb = "F"; else xb = "O";

                        string XXX = @"^~\&";
                        string s12message = "MSH|" + XXX + "|BL|LOGENE|PT||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORU^R01|" + Guid.NewGuid().ToString() + "|P|2.3.1|\r";

                        ////PID
                        s12message = s12message + "PID|1|" + jcxx.Rows[0]["F_brbh"].ToString().Trim() + "|" + jzh.Trim() + "|" + jcxx.Rows[0]["F_SFZH"].ToString().Trim() + "|" + jcxx.Rows[0]["F_XM"].ToString().Trim() + "||" + jcxx.Rows[0]["F_NL"].ToString().Trim() + "|"
                            + xb.Trim() + "|||" + jcxx.Rows[0]["F_LXXX"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim() + "|||||||||||||||||||" + "\r";
                        ////PV1
                        s12message = s12message + "PV1|1|" + brlb.Trim() + "|" + jcxx.Rows[0]["F_BQ"].ToString().Trim() + "^^" + jcxx.Rows[0]["F_CH"].ToString().Trim() + "^" + jgdm + "^" + jgmc + "|R|||^||||||||||^||||||||||||||||||||||||||||" + "\r";
                        ////OBR
                        s12message = s12message + "OBR|1|" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "^|" + jcxx.Rows[0]["F_BLH"].ToString().Trim() + "_" + bglx + "_" + bgxh + "^521|^" + jcxx.Rows[0]["F_BBMC"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim() + "^" + jcxx.Rows[0]["F_BBLX"].ToString().Trim() + "||" + jcxx.Rows[0]["F_bgRQ"].ToString().Trim()
                            + "|" + jcxx.Rows[0]["F_SDrq"].ToString().Trim() + "|" + jcxx.Rows[0]["F_spare5"].ToString().Trim() + "|||||" + jcxx.Rows[0]["F_lczd"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim() + "|" + jcxx.Rows[0]["F_SDRQ"].ToString().Trim()
                            + "|" + jcxx.Rows[0]["F_sjks"].ToString().Trim() + "^" + jcxx.Rows[0]["F_sjdw"].ToString().Trim() + "|" + "^" + jcxx.Rows[0]["F_sjys"].ToString().Trim() + "||||^||" + jcxx.Rows[0]["F_spare5"].ToString().Trim()
                            + "|||" + jcxx.Rows[0]["F_bgzt"].ToString().Trim() + "||||||" + jcxx.Rows[0]["F_BLK"].ToString().Trim() + "|" + bgysgh + "^" + bgys + "|" + shysgh + "^" + shys + "|^|||||||||||||" + jcxx.Rows[0]["F_BGGS"].ToString().Trim() + "\r";
                        ////OBX

                        //for (int x = 0; x < dt_bc.Rows.Count; x++)
                        //{
                        //    s12message = s12message + "OBX|0|ST|3^补充报告^^^^|1|" + dt_bd.Rows[x]["F_bczd"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\")
                        //        + "||||||F||||" + GetYHBH(dt_bc.Rows[x]["F_BC_BGYS"].ToString()) + "^" + dt_bc.Rows[x]["F_BC_BGYS"].ToString() + "|" + GetYHBH(dt_bc.Rows[x]["F_BC_shYS"].ToString()) + "^" + dt_bc.Rows[x]["F_BC_SHYS"].ToString() + "|||" + dt_bc.Rows[x]["F_bc_spare5"].ToString() + "\r";
                        //}
                        //for (int x = 0; x < dt_bd.Rows.Count; x++)
                        //{
                        //    s12message = s12message + "OBX|0|ST|4^冰冻报告^^^^|1|" + dt_bd.Rows[x]["F_bdzd"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\")
                        //        + "||||||F||||" + GetYHBH(dt_bd.Rows[x]["F_Bd_BGYS"].ToString()) + "^" + dt_bd.Rows[x]["F_Bd_BGYS"].ToString() + "|" + GetYHBH(dt_bd.Rows[x]["F_Bd_shYS"].ToString()) + "^" + dt_bd.Rows[x]["F_Bd_SHYS"].ToString() + "|||" + dt_bd.Rows[x]["F_bd_bgrq"].ToString() + "\r";
                        //}
                        if (bglx == "bc")
                        {
                            s12message = s12message + "OBX|0|补充|1^肉眼所见^^^^|1|" + jcxx.Rows[0]["F_RYSJ"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                     + "||||||F||||" + GetYHBH(dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim() + "|" + GetYHBH(dt_bc.Rows[0]["F_BC_shYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_SHYS"].ToString().Trim() + "|||" + dt_bc.Rows[0]["F_bc_spare5"].ToString().Trim() + "\r";


                            s12message = s12message + "OBX|0|补充|5^镜下所见^^^^|1|" + dt_bc.Rows[0]["F_bc_JXSJ"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                      + "||||||F||||" + GetYHBH(dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim() + "|" + GetYHBH(dt_bc.Rows[0]["F_BC_shYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_SHYS"].ToString().Trim() + "|||" + dt_bc.Rows[0]["F_bc_spare5"].ToString().Trim() + "\r";

                            s12message = s12message + "OBX|0|补充|6^特殊检查^^^^|1|" + dt_bc.Rows[0]["F_bc_TSJC"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                      + "||||||F||||" + GetYHBH(dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim() + "|" + GetYHBH(dt_bc.Rows[0]["F_BC_shYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_SHYS"].ToString().Trim() + "|||" + dt_bc.Rows[0]["F_bc_spare5"].ToString().Trim() + "\r";

                            s12message = s12message + "OBX|0|补充|3^补充报告^^^^|1|" + dt_bc.Rows[0]["F_bczd"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                  + "||||||F||||" + GetYHBH(dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_BGYS"].ToString().Trim() + "|" + GetYHBH(dt_bc.Rows[0]["F_BC_shYS"].ToString().Trim()) + "^" + dt_bc.Rows[0]["F_BC_SHYS"].ToString().Trim() + "|||" + dt_bc.Rows[0]["F_bc_spare5"].ToString().Trim() + "\r";
                        }
                        else if (bglx == "bd")
                        {
                            s12message = s12message + "OBX|0|冰冻|1^肉眼所见^^^^|1|" + jcxx.Rows[0]["F_RYSJ"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                            + "||||||F||||" + GetYHBH(dt_bd.Rows[0]["F_Bd_BGYS"].ToString().Trim()) + "^" + dt_bd.Rows[0]["F_Bd_BGYS"].ToString().Trim() + "|" + GetYHBH(dt_bd.Rows[0]["F_Bd_shYS"].ToString().Trim()) + "^" + dt_bd.Rows[0]["F_Bd_SHYS"].ToString().Trim() + "|||" + dt_bd.Rows[0]["F_bd_bgrq"].ToString().Trim() + "\r";

                            s12message = s12message + "OBX|0|冰冻|4^冰冻报告^^^^|1|" + dt_bd.Rows[0]["F_bdzd"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                + "||||||F||||" + GetYHBH(dt_bd.Rows[0]["F_Bd_BGYS"].ToString().Trim()) + "^" + dt_bd.Rows[0]["F_Bd_BGYS"].ToString().Trim() + "|" + GetYHBH(dt_bd.Rows[0]["F_Bd_shYS"].ToString().Trim()) + "^" + dt_bd.Rows[0]["F_Bd_SHYS"].ToString().Trim() + "|||" + dt_bd.Rows[0]["F_bd_bgrq"].ToString().Trim() + "\r";
                        }
                        else
                        {
                            s12message = s12message + "OBX|0|常规|1^肉眼所见^^^^|1|" + jcxx.Rows[0]["F_RYSJ"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                            + "||||||F||||" + bgysgh + "^" + bgys + "|" + shysgh + "^" + shys + "||^|" + jcxx.Rows[0]["F_spare5"].ToString().Trim() + "\r";

                            s12message = s12message + "OBX|0|常规|2^病理诊断^^^^|1|" + jcxx.Rows[0]["F_BLZD"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                  + "||||||F||||" + bgysgh + "^" + bgys + "|" + shysgh + "^" + shys + "|||" + jcxx.Rows[0]["F_spare5"].ToString().Trim() + "\r";

                            s12message = s12message + "OBX|0|常规|5^镜下所见^^^^|1|" + jcxx.Rows[0]["F_JXSJ"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                  + "||||||F||||" + bgysgh + "^" + bgys + "|" + shysgh + "^" + shys + "|||" + jcxx.Rows[0]["F_spare5"].ToString().Trim() + "\r";

                            s12message = s12message + "OBX|0|常规|6^特殊检查^^^^|1|" + jcxx.Rows[0]["F_TSJC"].ToString().Replace("\\", "\\E\\").Replace("|", "\\F\\").Replace("^", "\\S\\").Replace("&", "\\T\\").Replace("~", "\\R\\").Replace("\r\n", "\n").Replace("\n", "\\@\\").Trim()
                                  + "||||||F||||" + bgysgh + "^" + bgys + "|" + shysgh + "^" + shys + "|||" + jcxx.Rows[0]["F_spare5"].ToString().Trim() + "\r";
                        }
                        string result = ""; string ErrMsg = "";
                        if (send_HL7(s12message, ref  result, ref ErrMsg, debug))
                        {
                            //成功
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='',F_FSZT='已处理'  where F_blbh='" + blbh + "'");
                            if (bglx == "cg")
                                aa.ExecuteSQL("update T_JCXX set F_HXBJ='2'  where F_blh='" + blh + "' ");
                        }
                        else
                        {
                            //失败
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='" + ErrMsg + "'  where F_blbh='" + blbh + "' ");
                            if (bglx == "cg")
                                aa.ExecuteSQL("update T_JCXX set F_HXBJ='0'  where F_blh='" + blh + "' ");
                            log.WriteMyLog(ErrMsg);

                        }

                    }
                    catch (Exception e2)
                    {
                        try
                        {
                            if (bglx == "cg")
                                aa.ExecuteSQL("update T_JCXX set F_HXBJ='-1'  where F_blh='" + blh + "' ");
                            aa.ExecuteSQL("update T_JCXX_FS set F_bz='报告回写异常:" + e2.Message + "'  where F_blbh='" + blbh + "'");
                            log.WriteMyLog(blh + "^" + bglx + "^" + bgxh + ",报告回写异常:" + e2.Message); return;
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog(ee.Message);
                        }
                    }
                    #endregion
                }
            }
              catch(Exception ee3)
            {
               log.WriteMyLog(ee3.Message);
                }
               return;
                }

        public  bool send_HL7(string  rtnMessage, ref string result,ref string ErrMsg, string debug)
        {
            string hl7server = f.ReadString("HL7", "Server", "172.16.15.97").ToString().Replace("\0", "").Trim();
           string hl7port = f.ReadString("HL7", "Port", "30002").ToString().Replace("\0", "").Trim() ;
           try
           {

               byte[] by2 = System.Text.Encoding.UTF8.GetBytes(rtnMessage);
               //byte[] by2 = System.Text.Encoding.GetEncoding("gbk").GetBytes(s12message);
               byte[] by3 = new byte[by2.Length + 3];
               by3[0] = 11;
               Array.Copy(by2, 0, by3, 1, by2.Length);
               by3[by3.Length - 2] = 28;
               by3[by3.Length - 1] = 13;

               if (debug == "1")
                   log.WriteMyLog("send to server： " + hl7server + ":" + hl7port + " message :" + System.Text.Encoding.UTF8.GetString(by3, 0, by3.Length));

               HL7message b = new HL7message();

               b.sendmessage(ref by3, hl7server, hl7port, ref result, ref   ErrMsg, "");

               if (debug == "1")
                   log.WriteMyLog(result + "：" + ErrMsg);

               if (result == "99")
                   return true;
               else if (result == "-1")
                   return false;
               else
                   return false;

           }
           catch(Exception  ee1)
           {
               ErrMsg = ee1.Message;
               log.WriteMyLog(ErrMsg);
               return false;
           }
            
        }

        private string GetYHBH(string yhmc)
        {
            if (yhmc.Trim() == "")
                return "";
            try
            {
                dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable bljc = new DataTable();
                bljc = aa.GetDataTable("select F_YHBH from T_yh where F_yhmc='" + yhmc + "'", "yhbh");
                aa.Close();
                if (bljc.Rows.Count == 0)
                    return "";
                return bljc.Rows[0]["F_yhbh"].ToString();
            }
            catch
            {
                return "";
            }
        }

    }
}