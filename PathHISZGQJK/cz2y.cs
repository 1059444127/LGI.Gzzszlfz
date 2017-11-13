using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using dbbase;
using ZgqClassPub;
using ZgqClassPub.DBData;


namespace PathHISZGQJK
{
    //常州二院，写存储过程
    class cz2y
    {
        private IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string yymc)
        {
            string odbcsql = f.ReadString("savetohis", "odbcsql", "DSN=pathnet-his;UID=interfacepat;PWD=INTERFACEPAT");

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }
            if (jcxx.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }

            if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {
              
                try
                {
                        OdbcParameter[] mzORAPAR = new OdbcParameter[19];
                        for (int j = 0; j < mzORAPAR.Length; j++)
                        {
                            mzORAPAR[j] = new OdbcParameter();
                        }
                        mzORAPAR[0].ParameterName = "v_req_no";
                        mzORAPAR[0].OdbcType = OdbcType.VarChar;
                        mzORAPAR[0].Direction = ParameterDirection.Input;
                        mzORAPAR[0].Size = 50;
                        mzORAPAR[0].Value="";

                        mzORAPAR[1].ParameterName = "v_local_id";//
                        mzORAPAR[1].OdbcType = OdbcType.VarChar;
                        mzORAPAR[1].Direction = ParameterDirection.Input;
                        mzORAPAR[1].Size = 50;

                        mzORAPAR[2].ParameterName = "v_patient_id";//
                        mzORAPAR[2].OdbcType = OdbcType.VarChar;
                        mzORAPAR[2].Direction = ParameterDirection.Input;
                        mzORAPAR[2].Size = 50;

                        mzORAPAR[3].ParameterName = "v_visit_id";//
                        mzORAPAR[3].OdbcType = OdbcType.VarChar;
                        mzORAPAR[3].Direction = ParameterDirection.Input;
                        mzORAPAR[3].Size = 50;

                        mzORAPAR[4].ParameterName = "v_relevant_diag";// 
                        mzORAPAR[4].OdbcType = OdbcType.VarChar;
                        mzORAPAR[4].Direction = ParameterDirection.Input;
                        mzORAPAR[4].Size = 200;

                        mzORAPAR[5].ParameterName = "v_clin_diag";// 
                        mzORAPAR[5].OdbcType = OdbcType.VarChar;
                        mzORAPAR[5].Direction = ParameterDirection.Input;
                        mzORAPAR[5].Size = 200;

                        mzORAPAR[6].ParameterName = "v_req_date";// 
                        mzORAPAR[6].OdbcType = OdbcType.DateTime;
                        mzORAPAR[6].Direction = ParameterDirection.Input;

                        mzORAPAR[7].ParameterName = "v_req_dept";// 
                        mzORAPAR[7].OdbcType = OdbcType.VarChar;
                        mzORAPAR[7].Direction = ParameterDirection.Input;
                        mzORAPAR[7].Size = 50;

                        mzORAPAR[8].ParameterName = "v_req_doctor";// 
                        mzORAPAR[8].OdbcType = OdbcType.VarChar;
                        mzORAPAR[8].Direction = ParameterDirection.Input;
                        mzORAPAR[8].Size = 50;

                        mzORAPAR[9].ParameterName = "v_exam_doctor";// 
                        mzORAPAR[9].OdbcType = OdbcType.VarChar;
                        mzORAPAR[9].Direction = ParameterDirection.Input;
                        mzORAPAR[9].Size = 50;

                        mzORAPAR[10].ParameterName = "v_report_doctor";// 
                        mzORAPAR[10].OdbcType = OdbcType.VarChar;
                        mzORAPAR[10].Direction = ParameterDirection.Input;
                        mzORAPAR[10].Size = 50;

                        mzORAPAR[11].ParameterName = "v_exam_date";// 
                        mzORAPAR[11].OdbcType = OdbcType.Date;
                        mzORAPAR[11].Direction = ParameterDirection.Input;

                        mzORAPAR[12].ParameterName = "v_report_date";// 
                        mzORAPAR[12].OdbcType = OdbcType.Date;
                        mzORAPAR[12].Direction = ParameterDirection.Input;

                        mzORAPAR[13].ParameterName = "v_exam_para";// 
                        mzORAPAR[13].OdbcType = OdbcType.VarChar;
                        mzORAPAR[13].Direction = ParameterDirection.Input;
                        mzORAPAR[13].Size = 200;

                        mzORAPAR[14].ParameterName = "v_description";// 
                        mzORAPAR[14].OdbcType = OdbcType.VarChar;
                        mzORAPAR[14].Direction = ParameterDirection.Input;
                        mzORAPAR[14].Size = 200;

                        mzORAPAR[15].ParameterName = "v_impression";// 
                        mzORAPAR[15].OdbcType = OdbcType.VarChar;
                        mzORAPAR[15].Direction = ParameterDirection.Input;
                        mzORAPAR[15].Size = 200;

                        mzORAPAR[16].ParameterName = "v_recommendation";// 
                        mzORAPAR[16].OdbcType = OdbcType.VarChar;
                        mzORAPAR[16].Direction = ParameterDirection.Input;
                        mzORAPAR[16].Size = 200;

                        mzORAPAR[17].ParameterName = "v_charge_type";// 
                        mzORAPAR[17].OdbcType = OdbcType.VarChar;
                        mzORAPAR[17].Direction = ParameterDirection.Input;
                        mzORAPAR[17].Size = 200;

                        mzORAPAR[18].ParameterName = "return_exam_no ";// 
                        mzORAPAR[18].OdbcType = OdbcType.VarChar;
                        mzORAPAR[18].Direction = ParameterDirection.Output;
                        mzORAPAR[18].Size = 200;

                        /////////////////////////////////////////////////////
                        mzORAPAR[0].Value = jcxx.Rows[0]["f_sqxh"].ToString().Trim();   //申请单号    
                        mzORAPAR[1].Value = "";   //检查号     
                        mzORAPAR[2].Value = jcxx.Rows[0]["f_zyh"].ToString().Trim();   //住院号/会员ID
                        mzORAPAR[3].Value = jcxx.Rows[0]["F_YZID"].ToString().Trim();   //住院次数/门诊次数 
                        mzORAPAR[4].Value = "";   //病历摘要       
                        mzORAPAR[5].Value = jcxx.Rows[0]["F_LCZD"].ToString().Trim();   //临床诊断
                        mzORAPAR[6].Value =DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString().Trim());   //申请时间
                        mzORAPAR[7].Value = jcxx.Rows[0]["f_sjks"].ToString().Trim();   //申请科室名称
                        mzORAPAR[8].Value = jcxx.Rows[0]["f_skys"].ToString().Trim();   //申请医师名称
                        mzORAPAR[9].Value = jcxx.Rows[0]["f_bgys"].ToString().Trim();   //检查医师名称（报告医生）
                        mzORAPAR[10].Value = jcxx.Rows[0]["f_shys"].ToString().Trim();   // 报告医师名称
                        mzORAPAR[11].Value = jcxx.Rows[0]["f_bgrq"].ToString().Trim();   //检查日期（报告日期）
                        mzORAPAR[12].Value = jcxx.Rows[0]["f_spare5"].ToString().Trim();   //报告日期
                        mzORAPAR[13].Value = ""; ;   //检查参数(返回空)
                        mzORAPAR[14].Value = jcxx.Rows[0]["f_rysj"].ToString().Trim();   //检查所见
                        mzORAPAR[15].Value = jcxx.Rows[0]["f_blzd"].ToString().Trim();   //印象
                        mzORAPAR[16].Value = "";   //建议(返回空)  
                        mzORAPAR[17].Value = "";   //费别

                        ////////////////////////////////////////////////////
                       try
                        {
                         OdbcDB  odbc=new OdbcDB();
                         string  msg="";
                        int x = odbc.ExecuteNonQuery(odbcsql,"{ CALL hthis.PROC_PEIS_GETBLRESULTINFO(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)}",ref mzORAPAR,CommandType.StoredProcedure,ref  msg);
                        if (x < 1 && msg.Trim() != "")
                            MessageBox.Show(msg);

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message.ToString());
                        }
                    
                }
                catch (Exception ee)
                {
                   MessageBox.Show(ee.Message.ToString());
                }
            }


        }
    }
}
