using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using dbbase;
using System.Drawing;
using ZgqClassPub;

namespace PathHISJK
{
    class shz1fy
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public void shz1y(string F_blh, string yymc,string debug)
        {
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
            //string bgzt = dataGridView1.CurrentRow.Cells["报告状态"].Value.ToString().Trim();
            if (jcxx.Rows.Count < 1)
            {
                 log.WriteMyLog(F_blh + "数据错误，主表无记录");
                return;
            }
            if (jcxx.Rows[0]["F_yzid"].ToString().Trim() == "")
            {
                log.WriteMyLog("无医嘱ID，不处理！");
                return;
            }
            string yzid = jcxx.Rows[0]["F_yzid"].ToString().Trim();
            int ibrlb = 0;
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() !="体检")
            {
                log.WriteMyLog("非体检病人，不处理");
                return;
            }
            if (jcxx.Rows[0]["F_by2"].ToString().Trim() == "")
            {

                log.WriteMyLog("医嘱项目代码为空，不处理");
                return;
            }
            if (jcxx.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                log.WriteMyLog(F_blh + "，无病人编号，不处理！");
                return;
            }

            dbbase.sqldb jsdaa = new sqldb(Application.StartupPath + "\\sz.ini", "jsddb");

            if (jcxx.Rows[0]["F_bgzt"].ToString().Trim() != "已审核")
            {
                jsdaa.ExecuteSQL("exec usp_yjjk_jg_huishou '" + jcxx.Rows[0]["F_blh"].ToString() + "','BLBG','RIS'");
                if (debug=="1")
                log.WriteMyLog("exec usp_yjjk_jg_huishou '" + jcxx.Rows[0]["F_blh"].ToString() + "','BLBG','RIS'");
                return;
            }
            else
            {
                string sqlstring = "exec usp_yjjk_jcbrfb_z ";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_blh"].ToString() + "',";
                sqlstring = sqlstring + "0,";
                sqlstring = sqlstring + "0,";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_brbh"].ToString() + "',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_brbh"].ToString() + "',";
                sqlstring = sqlstring + "'',"; //cardno暂不输入
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_xm"].ToString() + "',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_xb"].ToString() + "',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_nl"].ToString() + "',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_sjks"].ToString() + "',";

                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bq"].ToString() + "',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_ch"].ToString() + "',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_sjys"].ToString() + "',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_sdrq"].ToString() + "',";
                sqlstring = sqlstring + "'BLBG',";
                sqlstring = sqlstring + "'病理报告',";                
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bgrq"].ToString() + "',";
                sqlstring = sqlstring + "'3',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bgys"].ToString() + "',";
                sqlstring = sqlstring + "'307',";
                sqlstring = sqlstring + "'病理科',";
                sqlstring = sqlstring + "'1',";
                sqlstring = sqlstring + "0,";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_shys"].ToString() + "',";
                sqlstring = sqlstring + "'0',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_sdrq"].ToString() + "',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'RIS',";
                sqlstring = sqlstring + "'3',";  //@brlb        u5_int,   --ris,lis共用，代表表里cflb处方类别  
                sqlstring = sqlstring + "'',";   //@wcsj        u5_rq16, --ris，lis共用，ris里代表wcsj，lis代表yjsj  
                sqlstring = sqlstring + "'',";   //@dyxh        varchar(10),--lis用，代表dyxh 
                sqlstring = sqlstring + "'-1',"; //@applyno     varchar(20)='-1' ,--lis用，代表bgdh  

                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bbmc"].ToString() + "',";  //@jcbw        varchar(300),     -----检查部位结果
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_rysj"].ToString() + "',"; //@jcsj        varchar(7000),  -----检查所见结果
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_blzd"].ToString() + "',";   //@jcjl        varchar(7000),   -----检查结论结果

                string ftplj =   jcxx.Rows[0]["F_txml"].ToString();
                DataTable tx = aa.GetDataTable("select * from T_tx where F_blh='"+F_blh+"' and F_sfdy='1'", "tx");

                if (tx.Rows.Count > 0)
                {
                    ftplj = ftplj +"/"+ tx.Rows[0]["F_txm"].ToString();
                }
                
                sqlstring = sqlstring + "'" + ftplj + "',";   //@jctp        varchar(1000), ------报告对应图片

                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_by2"].ToString() + "'";   //@xmdms       varchar(100)=''  -----报告对应收费项目代码
                if (debug == "1")
                log.WriteMyLog(sqlstring);
                jsdaa.ExecuteSQL(sqlstring);

            }
           

        }


    }
}
