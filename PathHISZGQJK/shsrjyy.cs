using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using dbbase;
using System.Windows.Forms;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class shsrjyy
    {

        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug)
        {
            SqlDB db = new SqlDB();
            string msg = f.ReadString("savetohis", "msg", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql", "");
            string ksmc = f.ReadString("savetohis", "ksmc", "东病理科");
            string ksdm = f.ReadString("savetohis", "ksdm", "13610002");
            
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
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
              
                log.WriteMyLog("申请序号为空，不处理！");
                return;
            }



            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {


                SqlParameter[] SqlParameters = new SqlParameter[28];

                SqlParameter repno = new SqlParameter();
                repno.ParameterName = "repno";
                repno.Value = bljc.Rows[0]["F_BLH"].ToString().Trim();
                SqlParameters[0] = repno;

                SqlParameter reqno = new SqlParameter();
                reqno.ParameterName = "reqno";//

                if (bljc.Rows[0]["F_SQXH"].ToString().Trim().Contains("^"))
                    reqno.Value = bljc.Rows[0]["F_SQXH"].ToString().Trim().Split('^')[1].Trim();
                else
                    reqno.Value = "0";
                SqlParameters[1] = reqno;

                SqlParameter syxh = new SqlParameter();
                syxh.ParameterName = "syxh";
                if(bljc.Rows[0]["F_BRLB"].ToString().Trim()=="住院")
                    syxh.Value = bljc.Rows[0]["F_SQXH"].ToString().Trim().Split('^')[0].Trim();
                else
                syxh.Value = "0";
                SqlParameters[2] = syxh;

                SqlParameter patid = new SqlParameter();
                patid.ParameterName ="patid";
                if(bljc.Rows[0]["F_BRLB"].ToString().Trim()=="门诊")
                {
                      try
                      {
                          patid.Value = bljc.Rows[0]["F_SQXH"].ToString().Trim().Split('^')[2].Trim();
                      }
                       catch
                      { 
                           patid.Value = "0";
                      }
                }
                else
                    patid.Value = "0";
                SqlParameters[3] = patid;



                SqlParameter Blh = new SqlParameter();
                Blh.ParameterName = "blh";
                Blh.Value = bljc.Rows[0]["F_YZID"].ToString().Trim();
                SqlParameters[4] = Blh;

                SqlParameter cardno = new SqlParameter();
                cardno.ParameterName = "cardno";
                cardno.Value = bljc.Rows[0]["F_BRBH"].ToString().Trim();
                 SqlParameters[5] = cardno;

                SqlParameter hzxm = new SqlParameter();
                hzxm.ParameterName = "hzxm";
                hzxm.Value = bljc.Rows[0]["F_XM"].ToString().Trim();
                SqlParameters[6] = hzxm;

                SqlParameter Sex = new SqlParameter();
                Sex.ParameterName = "sex";
                Sex.Value = bljc.Rows[0]["F_XB"].ToString().Trim();
                SqlParameters[7] = Sex;

                SqlParameter Age = new SqlParameter();
                Age.ParameterName = "age";
                Age.Value = bljc.Rows[0]["F_NL"].ToString().Trim();
                SqlParameters[8] = Age;

                SqlParameter sjksdm = new SqlParameter();
                sjksdm.ParameterName = "sjksdm";
                sjksdm.Value = " ";
                SqlParameters[9] = sjksdm;

                SqlParameter sjksmc = new SqlParameter();
                sjksmc.ParameterName = "sjksmc";
                sjksmc.Value = bljc.Rows[0]["F_SJKS"].ToString().Trim();
                SqlParameters[10] = sjksmc;

                SqlParameter bqdm = new SqlParameter();
                bqdm.ParameterName = "bqdm";
                bqdm.Value = " ";
                SqlParameters[11] = bqdm;

                SqlParameter bqmc = new SqlParameter();
                bqmc.ParameterName = "bqmc";
                bqmc.Value = bljc.Rows[0]["F_BQ"].ToString().Trim();
                SqlParameters[12] = bqmc;

                SqlParameter cwdm = new SqlParameter();
                cwdm.ParameterName = "cwdm";
                cwdm.Value = bljc.Rows[0]["F_CH"].ToString().Trim();
                SqlParameters[13] = cwdm;

                SqlParameter sjysdm = new SqlParameter();
                sjysdm.ParameterName = "sjysdm";
                sjysdm.Value = " ";
                SqlParameters[14] = sjysdm;

                SqlParameter sjysmc = new SqlParameter();
                sjysmc.ParameterName = "sjysxm";
                sjysmc.Value = bljc.Rows[0]["F_SJYS"].ToString().Trim();
                SqlParameters[15] = sjysmc;

                SqlParameter sjrq = new SqlParameter();
                sjrq.ParameterName = "sjrq";
                sjrq.Value = DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyyMMddHH:mm:ss");
                SqlParameters[16] = sjrq;

                SqlParameter replb = new SqlParameter();
                replb.ParameterName = "replb";
                replb.Value = "1";
                SqlParameters[17] = replb;

                SqlParameter replbmc = new SqlParameter();
                replbmc.ParameterName = "replbmc";
                replbmc.Value = "病理报告单";
                SqlParameters[18] = replbmc;

                SqlParameter reprq = new SqlParameter();
                reprq.ParameterName = "reprq";
                reprq.Value =DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMddHH:mm:ss");
                SqlParameters[19] = reprq;

                SqlParameter xtbz = new SqlParameter();
                xtbz.SqlDbType = SqlDbType.Int;
                xtbz.ParameterName = "xtbz";
                if (bljc.Rows[0]["F_BRLB"].ToString().Trim() == "住院")
                    xtbz.Value = 1;
                else
                {
                    if (bljc.Rows[0]["F_BRLB"].ToString().Trim() == "体检")
                          xtbz.Value = 3;
                      else
                          xtbz.Value = 0;
                }
                SqlParameters[20] = xtbz;

                SqlParameter jcbw = new SqlParameter();
                jcbw.ParameterName = "jcbw";
                jcbw.Value =bljc.Rows[0]["F_BBMC"].ToString().Trim();
                SqlParameters[21] = jcbw;

                SqlParameter jcysdm = new SqlParameter();
                jcysdm.ParameterName = "jcysdm";
                jcysdm.Value = getysgh(bljc.Rows[0]["F_BGYS"].ToString().Trim());
                SqlParameters[22] = jcysdm;

                SqlParameter jcysxm = new SqlParameter();
                jcysxm.ParameterName = "jcysxm";
                jcysxm.Value = bljc.Rows[0]["F_BGYS"].ToString().Trim();
                SqlParameters[23] = jcysxm;

                SqlParameter jcksdm = new SqlParameter();
                jcksdm.ParameterName = "jcksdm";
                jcksdm.Value = ksdm;
                SqlParameters[24] = jcksdm;

                SqlParameter jcksmc = new SqlParameter();
                jcksmc.ParameterName = "jcksmc";
                jcksmc.Value =ksmc;
                SqlParameters[25] = jcksmc;

                SqlParameter pubtime = new SqlParameter();
                pubtime.ParameterName = "pubtime";
                pubtime.Value = DateTime.Now.ToString("yyyyMMddHH:mm:ss");
                SqlParameters[26] = pubtime;

                SqlParameter isly = new SqlParameter();
                isly.ParameterName = "isly";
                isly.Value = 0;
                SqlParameters[27] = isly;

                string message = "";
            
                int a = db.ExecuteNonQuery(odbcsql, "usp_yjjk_dj_releasereport",ref SqlParameters, CommandType.StoredProcedure, ref message);
                if (a < 1 && message.Trim() != "")
                {
                    log.WriteMyLog("回传报告异常，" + message.Trim());
                    return;
                }
                else
                {
                     aa.ExecuteSQL("update T_JCXX  set F_SCBJ='0' where F_BLH='" + blh.Trim() + "'");

                    string[] Xmdm_str = new string[] { "bw", "jcsj", "jcjl", "jyz" };
                    string[] Xmmc_str = new string[] { "检查部位", "检查所见", "检查结论", "建议值" };


                    for (int x = 0; x < Xmdm_str.Length; x++)
                    { 
                        SqlParameter[] SqlParameters2 = new SqlParameter[14];

                    SqlParameter repno_1 = new SqlParameter();
                    repno_1.ParameterName = "repno";
                    repno_1.Value = bljc.Rows[0]["F_BLH"].ToString().Trim();
                    SqlParameters2[0] = repno_1;


                    SqlParameter replb_1 = new SqlParameter();
                    replb_1.ParameterName = "replb";
                    replb_1.Value = "1";//100900005
                    SqlParameters2[1] = replb_1;

                  
                    SqlParameter Xmdw = new SqlParameter();
                    Xmdw.ParameterName = "xmdw";
                    Xmdw.Value = "";
                    SqlParameters2[5] = Xmdw;
                 
                    SqlParameter jgckz = new SqlParameter();
                    jgckz.ParameterName = "jgckz";
                    jgckz.Value = "";
                    SqlParameters2[6] = jgckz;

                    SqlParameter qqmxxh = new SqlParameter();
                    qqmxxh.ParameterName = "mxxh";
                    qqmxxh.Value = 0;
                    SqlParameters2[7] = qqmxxh;

                    SqlParameter xjbz = new SqlParameter();
                    xjbz.ParameterName = "xjbz";
                    xjbz.Value ="";
                    SqlParameters2[8] = xjbz;

                    SqlParameter xjmc = new SqlParameter();
                    xjmc.ParameterName = "xjmc";
                    xjmc.Value ="";
                    SqlParameters2[9] = xjmc;
                 
                    SqlParameter kssmc = new SqlParameter();
                    kssmc.ParameterName = "kssmc";
                    kssmc.Value ="";
                    SqlParameters2[10] = kssmc;

                    SqlParameter gmjg = new SqlParameter();
                    gmjg.ParameterName = "gmjg";
                    gmjg.Value = "";
                    SqlParameters2[11] = gmjg;

                  

                    SqlParameter jgxh = new SqlParameter();
                    jgxh.ParameterName = "jgxh";
                    jgxh.Value = "1";
                    SqlParameters2[12] = jgxh;

                    SqlParameter gdbz = new SqlParameter();
                    gdbz.ParameterName = "gdbz";
                    gdbz.Value = "0";
                    SqlParameters2[13] = gdbz;
                  

                  
                         SqlParameter Xmdm = new SqlParameter();
                         Xmdm.ParameterName = "xmdm";
                         Xmdm.Value = Xmdm_str[x].Trim();
                         SqlParameters2[2] = Xmdm;

                         SqlParameter Xmmc = new SqlParameter();
                         Xmmc.ParameterName = "xmmc";
                         Xmmc.Value = Xmmc_str[x].Trim();
                         SqlParameters2[3] = Xmmc;

                         SqlParameter Xmjg = new SqlParameter();
                         Xmjg.ParameterName = "xmjg";

                        string blzd= bljc.Rows[0]["F_BLZD"].ToString().Trim();
                        if (bljc.Rows[0]["F_tsjc"].ToString().Trim() != "")
                            blzd = blzd + "   "+"\r\n" + bljc.Rows[0]["F_tsjc"].ToString().Trim();

                        switch(Xmdm_str[x].Trim())
                        {
                            case "bw": Xmjg.Value = bljc.Rows[0]["F_BBMC"].ToString().Trim(); break;
                            case "jcsj": Xmjg.Value = bljc.Rows[0]["F_RYSJ"].ToString().Trim()+"\r\n" + bljc.Rows[0]["F_JXSJ"].ToString().Trim(); break;
                            case "jcjl": Xmjg.Value = blzd; break;
                            case "jyz": Xmjg.Value = bljc.Rows[0]["F_YYX"].ToString().Trim(); break;
                            default: Xmjg.Value = ""; break;
                        }
                       
                         SqlParameters2[4] = Xmjg;
                      
                       int b= db.ExecuteNonQuery(odbcsql, "usp_yjjk_dj_releasereportmx", ref SqlParameters2,CommandType.StoredProcedure,ref message);
                       if (b < 1 && message.Trim() != "")
                       { 
                           log.WriteMyLog(x.ToString()+"回传报告异常，" + message.Trim());
                       }
                       else
                       {
                            aa.ExecuteSQL("update T_JCXX  set F_SCBJ='1' where F_BLH='" + blh.Trim() + "'");
                       }
                    }
                }

           
            }
            else
            {

                if (bljc.Rows[0]["F_SCBJ"].ToString().Trim() == "1" || bljc.Rows[0]["F_SCBJ"].ToString().Trim() == "0")
                {
                    string message = "";
                   ///repno	30	报告单号   ^F_BLH   replb	10	报告类别    1  ^hslb	1	类别（0：回收1：查询）   0

                
                    SqlParameter [] SqlParameters = new SqlParameter[3];

                      SqlParameter repno = new SqlParameter();
                      repno.ParameterName ="repno";
                      repno.Value = bljc.Rows[0]["F_BLH"].ToString().Trim();

                      SqlParameter replb = new SqlParameter();
                      replb.ParameterName ="replb";
                      replb.Value = "1";

                      SqlParameter hslb = new SqlParameter();
                      hslb.ParameterName ="hslb";
                      hslb.Value ="0";

                      SqlParameters[0] = repno;
                      SqlParameters[1] = replb;
                      SqlParameters[2] = hslb;
            
                  int x= db.ExecuteNonQuery(odbcsql, "usp_yjjk_dj_retrievereport",ref SqlParameters,CommandType.StoredProcedure,ref message);
                  if (x < 1 && message.Trim() != "")
                      log.WriteMyLog("取消报告异常，" + message.Trim());
                }
            }
        }

        public string getysgh(string ysmc)
        {
          
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable yh = new DataTable();
            try
            {
                yh = aa.GetDataTable("select * from T_YH where F_YHMC='" + ysmc.Trim() + "'", "yh");
                if (yh.Rows.Count > 0)
                {
                    return yh.Rows[0]["F_YHM"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }

        }

    }
}
