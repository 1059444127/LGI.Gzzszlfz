using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using dbbase;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class nbxsrmyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug, string msg)
        {
           
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            
            if (bljc == null)
            {
                MessageBox.Show("����ţ�" + blh + ",�������ݿ����������⣡");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("����ţ�" + blh + ",������д���");
                return;
            }
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
            {
                log.WriteMyLog(blh + "�������Ϊ�գ����ش���");
                return;
            }

//           p_yjqr��sqdh��czgh��bz��zxysh��return INT OUTPUT��
//���ܣ��Բ��������뵥ȷ��ִ�л�ȡ��ִ��
//    ������
//    sqdh�����뵥�ţ�varchar(8)����Ӧ��ͼ�ֶ�V_pa_apply. APPLY_ID
//czgh���������ţ�varchar(10)��Ҫ���룡
//bz��ִ�б�־��integer 0Ϊȡ����1Ϊִ�У�9Ϊ�˷�
//zxysh��ִ��ҽ�� varchar(4)��bzΪ1ʱҪ���룡
//    ���أ�return INT
//������Ϊ�ɹ���С����Ϊʧ��
        //ȷ��
          
               string odbcsql = f.ReadString("savetohis", "odbcsqlhis", "Data Source=172.16.10.1;Initial Catalog=xsylzx;User Id=xrh;Password=xsrmyy18;");
                string  yhmc=f.ReadString("yh", "yhmc", "").Replace("\0","").Trim();
                string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
                SqlDB  sqldb=new SqlDB();

            //if(bljc.Rows[0]["F_mzqf"].ToString().Trim()!="1")
            //{
            //  SqlParameter []  Parameters=new SqlParameter[5];

            //  for (int j = 0; j < Parameters.Length; j++)
            //  {
            //      Parameters[j] = new SqlParameter();
            //  }
            //   Parameters[0].ParameterName = "sqdh";
            //   Parameters[0].SqlDbType = SqlDbType.VarChar;
            //   Parameters[0].Direction = ParameterDirection.Input;
            //   Parameters[0].Value=bljc.Rows[0]["F_SQXH"].ToString().Trim(); 
              
            //    Parameters[1].ParameterName = "czgh";
            //   Parameters[1].SqlDbType = SqlDbType.VarChar;
            //   Parameters[1].Direction = ParameterDirection.Input;
            //   Parameters[1].Value = yhbh; 

            //    Parameters[2].ParameterName = "bz";
            //   Parameters[2].SqlDbType = SqlDbType.Int;
            //   Parameters[2].Direction = ParameterDirection.Input;
            //   Parameters[2].Value=1; 

            //    Parameters[3].ParameterName = "zxysh";
            //   Parameters[3].SqlDbType = SqlDbType.VarChar;
            //   Parameters[3].Direction = ParameterDirection.Input;
            //   Parameters[3].Value=yhmc;

            //   Parameters[4].ParameterName = "return";
            //   Parameters[4].SqlDbType = SqlDbType.Int;
            //   Parameters[4].Direction = ParameterDirection.Output;

              
            //    string except="";
            //    sqldb.ExecuteNonQuery(odbcsql,"p_yjqr",ref Parameters,CommandType.StoredProcedure,ref except);
            //    if (except != "")
            //        MessageBox.Show("ȷ�Ϸ���ʧ�ܣ�" + except);
            //    else
            //    {
            //        try
            //        {
            //            if (int.Parse(Parameters[4].Value.ToString()) < 0)
            //            {
            //                MessageBox.Show("ȷ�Ϸ���ʧ��:" + Parameters[4].Value);
            //            }
            //            else
            //            {
            //                aa.ExecuteSQL("update T_JCXX set F_MZQF='1' where F_BLH='" + blh + "'");
            //                if (debug == "1")
            //                    MessageBox.Show("ȷ�Ϸ��óɹ�");
            //            }
            //        }
            //        catch(Exception  ee1)
            //        {
            //            MessageBox.Show("ȷ�Ϸ����쳣��"+ee1.Message);
            //        }
            //    }
            //}





            //            p_blsqd_zt��sqdh��ztmc��ztrq��czgh,  mark, return INT OUTPUT��
            //���ܣ��������鵥�����״̬���ظ�HIS
            //������
            //sqdh �����뵥��varchar(8), ��Ӧ��ͼ�ֶ�V_pa_apply. APPLY_ID
            //ztmc��״̬���� varchar(32), �磺�ѵǼ�/��ȡ�Ĵ���/�Ѱ���/����Ƭ/��������/��д����/�����
            //ztrq��״̬�������� datetime
            //czgh���������ţ�varchar(10)��Ҫ���룡
            //mark��  ��עvarchar(80) �ɴ���
            //    result����������varchar(600)
            //���أ�return INT
            //������Ϊ�ɹ���С����Ϊʧ��  
        //��״̬
         
            SqlParameter[] Parameters_2 = new SqlParameter[7];
            for (int j = 0; j < Parameters_2.Length; j++)
            {
                Parameters_2[j] = new SqlParameter();
            }
            Parameters_2[0].ParameterName = "sqdh";
            Parameters_2[0].SqlDbType = SqlDbType.VarChar;
            Parameters_2[0].Direction = ParameterDirection.Input;
            Parameters_2[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();
       
            Parameters_2[1].ParameterName = "ztmc";
            Parameters_2[1].SqlDbType = SqlDbType.VarChar;
            Parameters_2[1].Direction = ParameterDirection.Input;
            string bgzt=bljc.Rows[0]["F_BGZT"].ToString().Trim();
            if(bgzt=="��ȡ��")  bgzt="��ȡ�Ĵ���";
            Parameters_2[1].Value = bgzt.Trim();

            Parameters_2[2].ParameterName = "ztrq";
            Parameters_2[2].SqlDbType = SqlDbType.DateTime;
            Parameters_2[2].Direction = ParameterDirection.Input;
            Parameters_2[2].Value = DateTime.Now;
     
            Parameters_2[3].ParameterName = "czgh";
            Parameters_2[3].SqlDbType = SqlDbType.VarChar;
            Parameters_2[3].Direction = ParameterDirection.Input;
            Parameters_2[3].Value = yhbh;


            Parameters_2[4].ParameterName = "mark";
            Parameters_2[4].SqlDbType = SqlDbType.VarChar;
            Parameters_2[4].Direction = ParameterDirection.Input;
            Parameters_2[4].Value = bljc.Rows[0]["F_WFBGYY"].ToString().Trim();

            Parameters_2[5].ParameterName = "blxh";
            Parameters_2[5].SqlDbType = SqlDbType.VarChar;
            Parameters_2[5].Direction = ParameterDirection.Input;
            Parameters_2[5].Value = blh;

            Parameters_2[6].ParameterName = "return";
            Parameters_2[6].SqlDbType = SqlDbType.Int;
            Parameters_2[6].Direction = ParameterDirection.Output;
           
        
            string except2 = "";
          
            sqldb.ExecuteNonQuery(odbcsql, "p_blsqd_zt", ref Parameters_2, CommandType.StoredProcedure, ref except2);
     
            if (except2 != "")
            {
                if(debug=="1")
                    MessageBox.Show("�������ݿ�ʧ�ܣ�״̬��дʧ��:" + except2);

            }
            else
            {
                try
                {
                    if (int.Parse(Parameters_2[6].Value.ToString()) < 0)
                    {
                        if (debug == "1")
                            MessageBox.Show("״̬��дʧ��:" + Parameters_2[6].Value);
                    }
                    else
                    {
                      
                        if (debug == "1")
                            MessageBox.Show("״̬��д�ɹ�");
                    }
                }
                catch (Exception ee2)
                {
                    if (debug == "1")
                    MessageBox.Show("״̬��д�쳣��" + ee2.Message);
               
                }
            }


            //�ر���
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
            {

//p_blsqd_zdjg ��sqdh��czgh��result��return INT OUTPUT��
//���ܣ��������鵥����Ͻ�����ظ�HIS
//������
//sqdh �����뵥��varchar(8), ��Ӧ��ͼ�ֶ�V_pa_apply. APPLY_ID
//czgh���������ţ�varchar(10)��Ҫ���룡
//    result����������varchar(600)
//���أ�return INT
//������Ϊ�ɹ���С����Ϊʧ��

                //DataTable bljc_bc = new DataTable();
                //bljc_bc = aa.GetDataTable("select * from T_bcbg where F_BLH='" + blh + "' and F_BC_BGZT='�����'", "bcbg");
                string rysj = bljc.Rows[0]["F_RYSJ"].ToString().Trim();
                if (rysj.Trim() != "")
                    rysj = ("����������" + rysj + " \r\n ");

                string jxsj = bljc.Rows[0]["F_JXSJ"].ToString().Trim();
                if (jxsj.Trim() != "")
                    jxsj = ("����������" + jxsj + " \r\n ");

                string tsjc = bljc.Rows[0]["F_TSJC"].ToString().Trim();
                if (tsjc.Trim() != "")
                    tsjc = ("�����飺" + tsjc + "\r\n");
                string zd=rysj+jxsj+tsjc+bljc.Rows[0]["F_BLZD"].ToString().Trim();

                SqlParameter[] Parameters_bg = new SqlParameter[4];
                for (int j = 0; j < Parameters_bg.Length; j++)
                {
                    Parameters_bg[j] = new SqlParameter();
                }

                Parameters_bg[0].ParameterName = "sqdh";
                Parameters_bg[0].SqlDbType = SqlDbType.VarChar;
                Parameters_bg[0].Direction = ParameterDirection.Input;
                Parameters_bg[0].Value = bljc.Rows[0]["F_SQXH"].ToString().Trim();

                Parameters_bg[1].ParameterName = "czgh";
                Parameters_bg[1].SqlDbType = SqlDbType.VarChar;
                Parameters_bg[1].Direction = ParameterDirection.Input;
                Parameters_bg[1].Value = yhbh;

                Parameters_bg[2].ParameterName = "result";
                Parameters_bg[2].SqlDbType = SqlDbType.VarChar;
                Parameters_bg[2].Direction = ParameterDirection.Input;
                Parameters_bg[2].Value = zd.Trim();

                Parameters_bg[3].ParameterName = "return";
                Parameters_bg[3].SqlDbType = SqlDbType.Int;
                Parameters_bg[3].Direction = ParameterDirection.Output;


                 except2 = "";
                sqldb.ExecuteNonQuery(odbcsql, "p_blsqd_zdjg", ref Parameters_bg, CommandType.StoredProcedure, ref except2);

                if (except2 != "")
                {
                    if (debug == "1")
                        MessageBox.Show("�������ݿ�ʧ�ܣ������дʧ��:" + except2);
                    log.WriteMyLog("�������ݿ�ʧ�ܣ������дʧ��:" + except2);
                }
                else
                {
                    try
                    {
                        if (int.Parse(Parameters_bg[3].Value.ToString()) < 0)
                        {
                            if (debug == "1")
                                MessageBox.Show("�����дʧ��:" + Parameters_bg[3].Value);
                            log.WriteMyLog("�����дʧ��:" + Parameters_bg[3].Value);
                        }
                        else
                        {
                         aa.ExecuteSQL("update T_JCXX set F_SCBJ='1' where F_BLH='" + blh + "'");
                            if (debug == "1")
                                MessageBox.Show("�����д�ɹ�");
                        }
                    }
                    catch (Exception ee2)
                    {
                        if (debug == "1")
                            MessageBox.Show("�����д�쳣��" + ee2.Message);
                         log.WriteMyLog("�����д�쳣��" + ee2.Message);
                    }
                }
               





            }

        }

        //private string  get_yhgh(string F_yhmc)
        //{
        //    try
        //    {
        //        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        //        DataTable bljc = new DataTable();
        //        bljc = aa.GetDataTable("select top 1 F_YHBH from T_YH where F_YHMC='" + F_yhmc + "'", "bljc");
        //        if (bljc.Rows.Count > 0)
        //            return bljc.Rows[0][0].ToString();
        //        else
        //            return "";
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}
    }
}
