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
            //string bgzt = dataGridView1.CurrentRow.Cells["����״̬"].Value.ToString().Trim();
            if (jcxx.Rows.Count < 1)
            {
                 log.WriteMyLog(F_blh + "���ݴ��������޼�¼");
                return;
            }
            if (jcxx.Rows[0]["F_yzid"].ToString().Trim() == "")
            {
                log.WriteMyLog("��ҽ��ID��������");
                return;
            }
            string yzid = jcxx.Rows[0]["F_yzid"].ToString().Trim();
            int ibrlb = 0;
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() !="���")
            {
                log.WriteMyLog("����첡�ˣ�������");
                return;
            }
            if (jcxx.Rows[0]["F_by2"].ToString().Trim() == "")
            {

                log.WriteMyLog("ҽ����Ŀ����Ϊ�գ�������");
                return;
            }
            if (jcxx.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                log.WriteMyLog(F_blh + "���޲��˱�ţ�������");
                return;
            }

            dbbase.sqldb jsdaa = new sqldb(Application.StartupPath + "\\sz.ini", "jsddb");

            if (jcxx.Rows[0]["F_bgzt"].ToString().Trim() != "�����")
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
                sqlstring = sqlstring + "'',"; //cardno�ݲ�����
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
                sqlstring = sqlstring + "'������',";                
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bgrq"].ToString() + "',";
                sqlstring = sqlstring + "'3',";
                sqlstring = sqlstring + "'',";
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bgys"].ToString() + "',";
                sqlstring = sqlstring + "'307',";
                sqlstring = sqlstring + "'�����',";
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
                sqlstring = sqlstring + "'3',";  //@brlb        u5_int,   --ris,lis���ã��������cflb�������  
                sqlstring = sqlstring + "'',";   //@wcsj        u5_rq16, --ris��lis���ã�ris�����wcsj��lis����yjsj  
                sqlstring = sqlstring + "'',";   //@dyxh        varchar(10),--lis�ã�����dyxh 
                sqlstring = sqlstring + "'-1',"; //@applyno     varchar(20)='-1' ,--lis�ã�����bgdh  

                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_bbmc"].ToString() + "',";  //@jcbw        varchar(300),     -----��鲿λ���
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_rysj"].ToString() + "',"; //@jcsj        varchar(7000),  -----����������
                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_blzd"].ToString() + "',";   //@jcjl        varchar(7000),   -----�����۽��

                string ftplj =   jcxx.Rows[0]["F_txml"].ToString();
                DataTable tx = aa.GetDataTable("select * from T_tx where F_blh='"+F_blh+"' and F_sfdy='1'", "tx");

                if (tx.Rows.Count > 0)
                {
                    ftplj = ftplj +"/"+ tx.Rows[0]["F_txm"].ToString();
                }
                
                sqlstring = sqlstring + "'" + ftplj + "',";   //@jctp        varchar(1000), ------�����ӦͼƬ

                sqlstring = sqlstring + "'" + jcxx.Rows[0]["F_by2"].ToString() + "'";   //@xmdms       varchar(100)=''  -----�����Ӧ�շ���Ŀ����
                if (debug == "1")
                log.WriteMyLog(sqlstring);
                jsdaa.ExecuteSQL(sqlstring);

            }
           

        }


    }
}
