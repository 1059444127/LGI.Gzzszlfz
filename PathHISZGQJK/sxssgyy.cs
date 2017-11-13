using System;
using System.Collections.Generic;
using System.Text;
using ZgqClassPub;
using dbbase;
using System.Data;
using System.Windows.Forms;
using ZgqClassPub.DBData;
using System.Data.Odbc;

namespace PathHISZGQJK
{
    //陕西省森工医院
    class sxssgyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string F_blh)
        {
            string debug = f.ReadString("savetohis", "debug", "");
            string odbcsql = f.ReadString("savetohis", "odbcsql", "DSN=pathnet-his;UID=pacsuser;PWD=PACSsgyy0804;");
        
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + F_blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("连接数据库错误");
                return;
            }

            if (jcxx.Rows[0]["f_brlb"].ToString().Trim() == "体检")
            {
                tjhx(F_blh, jcxx, debug);
                return;
            }

            if (jcxx.Rows[0]["f_sqxh"].ToString().Trim() != "" && jcxx.Rows[0]["F_YZID"].ToString().Trim() != "")
            {
               
                OdbcDB  db=new OdbcDB();
                string errMsg="";
                string select_str = "select * from  RIS_COMMONREPORT where APPNO='" + jcxx.Rows[0]["f_sqxh"].ToString().Trim()
                    + "' and DIAGNOSEID='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "'  and REPORTID='" + F_blh + "'";

                if (debug == "1")
                    log.WriteMyLog("查询HIS表:" + select_str);

                DataTable  dt_ris = db.DataAdapter(odbcsql, select_str, ref errMsg);

                if (debug == "1")
                    log.WriteMyLog("查询HIS表结果:" + dt_ris.Rows.Count.ToString() + "  " + errMsg);

                string bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
                if (bgzt == "已审核")
                {
                    string insert_str = "";
                    if (dt_ris.Rows.Count > 0)
                    {
                        insert_str = "update   RIS_COMMONREPORT  set  REPORTID='" + F_blh + "',CHECKITEM='" + jcxx.Rows[0]["F_YZXM"].ToString().Trim()
                           + "',CHECKRESULT='" + jcxx.Rows[0]["F_RYSJ"].ToString().Trim() + "', INHOSDIAGNOSE='" + jcxx.Rows[0]["F_BLZD"].ToString().Trim()
                           + "',REPORTOPERATOR='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',REPORTTIME=TO_CHAR(TIMESTAMP('" + jcxx.Rows[0]["F_BGRQ"].ToString().Trim() + "'),'YYYY-MM-DD HH24:MI:SS')"
                           + ",AUDITPERSON ='" + jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',AUDITTIME=TO_CHAR(TIMESTAMP('" + jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "'),'YYYY-MM-DD HH24:MI:SS')  "
                           + " where  REPORTID='" + F_blh + "' and APPNO='" + jcxx.Rows[0]["f_sqxh"].ToString().Trim() + "' and DIAGNOSEID='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "'";         
                    }
                    else
                    {
                        insert_str = "insert  into   RIS_COMMONREPORT(REPORTID,APPNO,DIAGNOSEID,CHECKITEM,CHECKRESULT,INHOSDIAGNOSE, REPORTOPERATOR,REPORTTIME,AUDITPERSON,AUDITTIME,CHECKWAY) "
                            + " values('" + F_blh + "','" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "','" + jcxx.Rows[0]["F_YZID"].ToString().Trim() +"',"
                            + "'" + jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "','" + jcxx.Rows[0]["F_RYSJ"].ToString().Trim() + "','" + jcxx.Rows[0]["F_BLZD"].ToString().Trim() + "',"
                            + "'" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',TO_CHAR(TIMESTAMP('" + jcxx.Rows[0]["F_BGRQ"].ToString().Trim() + "'),'YYYY-MM-DD HH24:MI:SS'),"
                            + "'" + jcxx.Rows[0]["f_shys"].ToString().Trim() + "',TO_CHAR(TIMESTAMP('" + jcxx.Rows[0]["F_SPARE5"].ToString().Trim() + "'),'YYYY-MM-DD HH24:MI:SS'),'-')";
                    }
               

                    if (debug == "1")
                         log.WriteMyLog("回写HIS结果语句:"+insert_str);
       
                    if (db.ExecuteNonQuery(odbcsql, insert_str, ref errMsg) > 0)
                    {
                        if (debug == "1")
                        log.WriteMyLog("回写HIS结果成功");
                    }
                    else
                        log.WriteMyLog("回写HIS结果失败:" + errMsg);
                }
                else
                {
                    if (bgzt == "已写报告" && dt_ris.Rows.Count > 0)
                    {
                        string update_str = "delete RIS_COMMONREPORT where REPORTID='"+F_blh+"' and APPNO='" + jcxx.Rows[0]["f_sqxh"].ToString().Trim() + "' and DIAGNOSEID='" + jcxx.Rows[0]["F_YZID"].ToString().Trim() + "'";
                        if (debug == "1")
                            log.WriteMyLog("取消审核删除中间表数据:" + update_str);
                        if (db.ExecuteNonQuery(odbcsql, update_str, ref errMsg) > 0)
                        {
                            if(debug=="1")
                            log.WriteMyLog("删除HIS中间表数据成功");
                        }
                        else
                            log.WriteMyLog("删除HIS中间表数据失败:" + errMsg);
                    }
                }
              

            }
        }

        public void tjhx(string blh, DataTable  dt_jcxx,string debug)
        {
         
            string tjodbc = f.ReadString("savetohis","tjodbc","DSN=pathnet-tj;UID=interfacepat;PWD=INTERFACEPAT");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
           
                

                string brlb = "";
                string ztbz = "";
                string sqxh = dt_jcxx.Rows[0]["f_sqxh"].ToString().Trim();
                if (dt_jcxx.Rows[0]["F_brlb"].ToString().Trim() != "体检" || dt_jcxx.Rows[0]["F_brbh"].ToString().Trim()== "")
                {
                    return;
                }
                try
                {
                    if (dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                    {
                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                        OdbcParameter[] mzORAPAR = new OdbcParameter[11];
                        for (int j = 0; j < mzORAPAR.Length; j++)
                        {
                            mzORAPAR[j] = new OdbcParameter();
                        }
                        mzORAPAR[0].ParameterName = "Sequenceno";
                        mzORAPAR[0].OdbcType = OdbcType.VarChar;
                        mzORAPAR[0].Direction = ParameterDirection.Input;
                        mzORAPAR[0].Size = 50;

                        mzORAPAR[1].ParameterName = "CheckDescription";//
                        mzORAPAR[1].OdbcType = OdbcType.VarChar;
                        mzORAPAR[1].Direction = ParameterDirection.Input;
                        mzORAPAR[1].Size = 1000;

                        mzORAPAR[2].ParameterName = "CheckConclusion";//
                        mzORAPAR[2].OdbcType = OdbcType.VarChar;
                        mzORAPAR[2].Direction = ParameterDirection.Input;
                        mzORAPAR[2].Size = 1000;

                        mzORAPAR[3].ParameterName = "Checkdoccode";//
                        mzORAPAR[3].OdbcType = OdbcType.VarChar;
                        mzORAPAR[3].Direction = ParameterDirection.Input;
                        mzORAPAR[3].Size = 200;

                        mzORAPAR[4].ParameterName = "Checkdocname";// 
                        mzORAPAR[4].OdbcType = OdbcType.VarChar;
                        mzORAPAR[4].Direction = ParameterDirection.Input;
                        mzORAPAR[4].Size = 200;

                        mzORAPAR[5].ParameterName = "Checkdate";// 
                        mzORAPAR[5].OdbcType = OdbcType.Date;
                        mzORAPAR[5].Direction = ParameterDirection.Input;

                        mzORAPAR[6].ParameterName = "Picture_Qty";// 
                        mzORAPAR[6].OdbcType = OdbcType.Int;
                        mzORAPAR[6].Direction = ParameterDirection.Output;

                        mzORAPAR[7].ParameterName = "Err_code";// 
                        mzORAPAR[7].OdbcType = OdbcType.Int;
                        mzORAPAR[7].Direction = ParameterDirection.Output;

                        mzORAPAR[8].ParameterName = "Err_text";// 
                        mzORAPAR[8].OdbcType = OdbcType.VarChar;
                        mzORAPAR[8].Direction = ParameterDirection.Output;
                        mzORAPAR[8].Size = 200;

                        mzORAPAR[9].ParameterName = "AuditOpercode";// 
                        mzORAPAR[9].OdbcType = OdbcType.VarChar;
                        mzORAPAR[9].Direction = ParameterDirection.Input;
                        mzORAPAR[9].Size = 200;

                        mzORAPAR[10].ParameterName = "AuditOpername";// 
                        mzORAPAR[10].OdbcType = OdbcType.VarChar;
                        mzORAPAR[10].Direction = ParameterDirection.Input;
                        mzORAPAR[10].Size = 200;
                        //////////////////////////////////////////////////////

                        string tj_blzd = dt_jcxx.Rows[0]["F_blzd"].ToString().Trim();
                        string tj_jcsj = "";


                        if (dt_jcxx.Rows[0]["F_xb"].ToString().Trim() != "男")
                        {
                            string mcyj = dt_jcxx.Rows[0]["F_mcyj"].ToString().Trim();
                            string sfjj = dt_jcxx.Rows[0]["F_sfjj"].ToString().Trim();

                            if (mcyj.Trim() != "")
                                tj_jcsj = tj_jcsj + "末次月经：" + mcyj + "\n";
                            if (sfjj.Trim() != "")
                                tj_jcsj = tj_jcsj + "是否绝经：" + sfjj + "\n";
                        }

                        tj_jcsj = tj_jcsj + dt_jcxx.Rows[0]["F_rysj"].ToString().Trim();

                        if (TJ_bljc.Rows.Count > 0)
                        {

                            if (dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT国产")
                            {

                                tj_jcsj = tj_jcsj + "标本满意度：" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["f_tbs_xbl"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_XBXM3"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + "病原微生物：" + TJ_bljc.Rows[0]["F_TBS_WSW6"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim() + "\n";
                                tj_jcsj = tj_jcsj + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\n";
                                ////////////////////////////////////
                                tj_blzd = "诊断：" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\n" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见1：" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\n";
                                if (TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() != "")
                                    tj_blzd = tj_blzd + "补充意见2：" + TJ_bljc.Rows[0]["F_TBS_BCYJ2"].ToString().Trim() + "\n";
                            }
                           
                        }
                        if (debug == "1")
                        {
                         log.WriteMyLog(tj_jcsj);
                            log.WriteMyLog(tj_blzd);
                        }
                        /////////////////////////////////////////////////////
                        mzORAPAR[0].Value = dt_jcxx.Rows[0]["f_sqxh"].ToString().Trim();   //体检申请单号
                        mzORAPAR[1].Value = tj_jcsj.Trim();   //检查所见
                        mzORAPAR[2].Value = tj_blzd.Trim();   // 检查所得
                        mzORAPAR[3].Value = "";   //检查医生工号
                        mzORAPAR[4].Value = dt_jcxx.Rows[0]["f_bgys"].ToString().Trim();   // 检查医生姓名
                        mzORAPAR[5].Value = DateTime.Parse(dt_jcxx.Rows[0]["f_bgrq"].ToString().Trim());   //检查时间
                        mzORAPAR[9].Value = "";   // 审核医生工号
                        mzORAPAR[10].Value = dt_jcxx.Rows[0]["f_shys"].ToString().Trim();   // 审核医生姓名
                        ////////////////////////////////////////////////////
                       OdbcDB  db=new OdbcDB();
                        string errMsg="";
                        try
                        {
                          int x= db.ExecuteNonQuery(tjodbc,"{ CALL hthis.PROC_PEIS_GETBLRESULTINFO(?,?,?,?,?,?,?,?,?,?,?)}",ref mzORAPAR,CommandType.StoredProcedure,ref errMsg);
                          
                            
                            if (mzORAPAR[8].Value.ToString().Trim() == "")
                            {
                                if (debug == "1")
                                    log.WriteMyLog(blh + "体检回传成功！");
                                ZgqClass.BGHJ(blh,"ZGQJK", "审核", "体检回传成功", "ZGQJK", "体检接口");
                                
                            }
                            else
                            {
                                 ZgqClass.BGHJ(blh,"ZGQJK", "审核", "体检回传失败:"+mzORAPAR[6].Value.ToString().Trim() + "," + mzORAPAR[7].Value.ToString().Trim() + "," + mzORAPAR[8].Value.ToString().Trim(), "ZGQJK", "体检接口");
                                log.WriteMyLog("体检回传失败:" + blh + "," + mzORAPAR[6].Value.ToString().Trim() + "," + mzORAPAR[7].Value.ToString().Trim() + "," + mzORAPAR[8].Value.ToString().Trim());
                            }
                        }
                        catch (Exception e)
                        {
                              ZgqClass.BGHJ(blh,"ZGQJK", "审核", "体检回传异常:"+e.Message.ToString().Trim(), "ZGQJK", "体检接口");
                            log.WriteMyLog(e.Message.ToString());
                        }
                    }
                }
                catch (Exception ee)
                {
                      ZgqClass.BGHJ(blh,"ZGQJK", "审核", "体检回传异常:"+ee.Message.ToString().Trim(), "ZGQJK", "体检接口");
                    log.WriteMyLog(ee.Message.ToString());
                }

            }
        
      
    }
}
