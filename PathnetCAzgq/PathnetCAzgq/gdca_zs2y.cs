using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using readini;
using System.Data;
using LG_ZGQ;
using PathHISZGQJK;
using dbbase;


namespace PathnetCAzgq
{
    class gdca_zs2y
    {

         private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      //   GDCAWSCOMLib.WSClientComClass GDCAWSCom = new WSClientComClass();

      //   ATL_COMLib.GdcaClass GDCACom = new GdcaClass();

         String Csigncert;
         String Ssigncert;
         String signValue;
         String signtext;
         String signtexten;
         String verifydata;
        string wsurl = "";
        string debug = "";
        string msg = "";
        //public string ca(string yhxx)
        //{

           
        //    string getblh = "";
        //    string type = "";
        //    string type2 = "";
        //    string yhm = "";

        //    string yhmc = "";
        //    string yhbh = "";
        //    string yhmm = "";
        //    string bglx = "";
        //    string bgxh = "";
        //    string keyname = "";



        //    string[] getyhxx = yhxx.Split('^');
        //    if (getyhxx.Length == 5)
        //    {
        //        type = getyhxx[0];
        //        yhm = getyhxx[1];
        //        yhmc = getyhxx[3].Trim();
        //        yhbh = getyhxx[2];
        //        yhmm = getyhxx[4];
        //    }
        //    else
        //    {
        //        type2 = getyhxx[0];
        //        getblh = getyhxx[1];
        //        bgxh = getyhxx[2];
        //        bglx = getyhxx[3];
        //        type = getyhxx[4];
        //        yhm = getyhxx[5];
        //        yhmc = getyhxx[6].Trim();
        //        yhbh = getyhxx[7];
        //        yhmm = getyhxx[8];
        //    }


        //    //ȡ�����ǰ
        //    if(type=="QXSH" && type2=="SHQTYSZQZ")
        //    {
        //        string CAGDBZ="";
        //        string  sqlstr="";
        //        if(bglx.ToLower()=="bc")
        //        {
        //            sqlstr = "select  F_CAGDZT from T_BCBG  where F_BLH='" + getblh + "' and F_BC_BGXH='" + bgxh + "'";
        //        }
        //        else if (bglx.ToLower() == "bd")
        //        {
        //            sqlstr = "select  F_CAGDZT from T_BdBG  where F_BLH='" + getblh + "' and F_Bd_BGXH='" + bgxh + "'";
        //        }
        //        else
        //            sqlstr = "select  F_CAGDZT from T_JCXX  where F_BLH='" + getblh + "'";

     
        //              dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "pathnet", "4s3c2a1p");
        //            DataTable  dt1=new DataTable();
        //           dt1= aa.GetDataTable(sqlstr,"CAGD");
        //             if (dt1== null ||dt1.Rows.Count <=0 )
        //            {
        //            return "0"; //�����ܳ��֣��Է���һ
        //            }

        //            if (dt1.Rows[0]["F_CAGDZT"].ToString().Trim() == "�ѹ鵵")
        //            {
        //                MessageBox.Show("�˱����ѱ��鵵������ȡ����ˣ�\r\n����ȡ����ˣ�����ϵCA�鵵�ҽ��г����鵵��");
        //                return  "0";
        //            }
        //            else
        //            {
        //                return  "1";
        //            }
 
        //    }
   


        //    string Server = f.ReadString("sqlserver", "Server", "192.168.70.252").Replace("\0", "");
        //    string DataBase = f.ReadString("sqlserver", "DataBase", "pathnet").Replace("\0", "");
        //    string UserID = f.ReadString("sqlserver", "UserID", "pathnet").Replace("\0", "");
        //    string PassWord = f.ReadString("sqlserver", "PassWord", "4s3c2a1p").Replace("\0", "");
        //    string constr_jcxx = "Data Source=" + Server + ";Initial Catalog=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

        //    msg = getSZ_String("CA", "msgage", "1").Replace("\0", ""); ;

        //     debug = f.ReadString("CA", "debug", "").Replace("\0", "");

        //     string qxshyzkey = getSZ_String("CA", "qxshqyzkey", "0").Replace("\0", "");
        //    //string shhyzkey = f.ReadString("CA", "shhyzkey", "0").Replace("\0", "");
        //     string yzsfzh = getSZ_String("CA", "yzsfzh", "").Replace("\0", "");
        //     string sydw = getSZ_String("CA", "sydw", "").Replace("\0", "");

        //     wsurl = getSZ_String("CA", "webservice", "http://192.168.8.38:8080/AGW/services/AGWService?wsdl");
        //    ////��ˣ�
        //    ////���ǰ  SH^ZPT^123456^����ͨ^1
        //    ////��˺�  TYSZQZ^�����^1^CG^QZ^ZPT^����ͨ^123456^1
        //    ////ȡ����ˣ�
        //    ////ȡ�����ǰ  SHQTYSZQZ^�����^1^CG^QXSH^ZPT^����ͨ^123456^1
        //    ////ȡ����˺�  TYSZQZ^�����^1^CG^QXQZ^ZPT^����ͨ^123456^1

        //    ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    ///
        //    ///���ǰ
        //    ///
        //    if (type == "SH")
        //    {
        //        if (debug == "1")
        //            MessageBox.Show("���ǰ��֤KEY������");
        //        //��֤key
        //        string key_yhmc = ""; string key_sfzh = ""; string key_sydw = "";
        //        bool yzbool = YZKEY(debug,ref key_sydw,ref key_yhmc,ref key_sfzh);
        //        GDCAWSCom.Finalize();
        //        GDCACom.GDCA_Finalize();
        //        if (!yzbool)
        //            return "0";
        //        if (key_yhmc.Trim() != yhmc.Trim())
        //        {
        //            if(msg=="1")
        //            MessageBox.Show("KEY�û��뵱ǰϵͳ�û���һ�£���֤��ͨ����" + key_yhmc);
        //            return "0";
        //        }

        //        if (sydw.Trim() != "")
        //        {
        //            if (key_sydw.Trim() != sydw.Trim())
        //            {
        //                if (msg == "1")
        //                MessageBox.Show("ʹ�õ�λ��һ�£���֤��ͨ����" + key_sydw); return "0";
        //            }
        //        }
        //        if (yzsfzh.Trim() == "1")
        //        {
        //            if (key_sfzh.Trim() != yhbh.Trim())
        //            {
        //                if (msg == "1")
        //                MessageBox.Show("���֤�Ų�һ�£���֤��ͨ����" + key_sfzh); return "0";
        //            }
        //        }
               
        //        return "1";
        //    }
        //    ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    ///
        //    ///��˺�
        //    ///
        //    if (type == "QZ" && type2 == "TYSZQZ")
        //    {
        //        if (debug == "1")
        //            MessageBox.Show("��˺�����ǩ�֡�����");

        //        int ret;
        //        ret = GDCACom.GDCA_isLogin(2);
        //        if (ret != 0)
        //        {
        //            string key_yhmc = ""; string key_sfzh = ""; string key_sydw = "";
        //            bool yzbool = YZKEY(debug,ref key_sydw,ref key_yhmc,ref key_sfzh);

        //            if (!yzbool)
        //            {
        //                GDCAWSCom.Finalize();
        //                GDCACom.GDCA_Finalize();
        //                return "1";
        //            }
        //            if (key_yhmc.Trim() != yhmc.Trim())
        //            {
        //                GDCAWSCom.Finalize();
        //                GDCACom.GDCA_Finalize();
        //                if (msg == "1")
        //                MessageBox.Show("KEY�û��뵱ǰϵͳ�û���һ�£���֤��ͨ����" + key_yhmc); return "1";
        //            }

        //            if (sydw.Trim() != "")
        //            {
        //                if (key_sydw.Trim() != sydw.Trim())
        //                {
        //                    GDCAWSCom.Finalize();
        //                    GDCACom.GDCA_Finalize();
        //                    if (msg == "1")
        //                    MessageBox.Show("ʹ�õ�λ��һ�£���֤��ͨ����" + key_sydw); return "1";
        //                }
        //            }
        //            if (yzsfzh.Trim() == "1")
        //            {
        //                if (key_sfzh.Trim() != yhbh.Trim())
        //                {
        //                    GDCAWSCom.Finalize();
        //                    GDCACom.GDCA_Finalize();
        //                    if (msg == "1")
        //                    MessageBox.Show("���֤�Ų�һ�£���֤��ͨ����" + key_sfzh); return "1";
        //                }
        //            }
        //        }
                
        //        ///�ͻ���ǩ��////////////////////////////////////////////
        //        LG_ZGQ.SqlDB_ZGQ sqldb = new SqlDB_ZGQ();


        //        string sql_str = "select * from T_JCXX where  F_BLH='" + getblh + "'  and  F_BGZT='�����'";
        //        //�������
        //        if (bglx == "BC")
        //            sql_str = "select * from V_BCBG_SH where  F_BLH='" + getblh + "' and F_BC_BGZT='�����'and F_BC_BGXH='" + bgxh + "'";
        //        //С�������
        //        if (bglx == "BD")
        //            sql_str = "select * from V_BDBG_bfk where  F_BLH='" + getblh + "' and  F_BD_BGZT='�����' and F_BD_BGXH='" + bgxh + "'";
        //        string execp = "";
        //        DataTable dtyhbh = sqldb.Sql_DataAdapter(constr_jcxx, sql_str, ref execp);
       
        //        if (dtyhbh == null)
        //        {
        //            GDCAWSCom.Finalize();
        //            GDCACom.GDCA_Finalize();
        //            MessageBox.Show("��ȡ��������Ϊʧ�ܣ�" + execp.Trim());
        //            return "1";
        //        }

        //        if (dtyhbh.Rows.Count <= 0)
        //        {
        //            GDCAWSCom.Finalize();
        //            GDCACom.GDCA_Finalize();
        //            MessageBox.Show("��ѯ��������Ϊ0��" + execp.Trim()+"\r\n" + sql_str);
        //            return "1";
        //        }
                
        //        //ҵ�񵥾ݺ�
        //        string djid = getblh + "^"+bglx+"^"+bgxh+"";
        //        //�ؼ�����
        //        string hash = "";

        //        if (bglx == "BC")
        //        {
        //            hash = dtyhbh.Rows[0]["F_bczd"].ToString().Trim();
        //            djid = getblh + "^" + dtyhbh.Rows[0]["F_BC_BGXH"].ToString().Trim() + "^BC";
        //        }
        //        else
        //            if (bglx == "BD")
        //            {
        //                hash = dtyhbh.Rows[0]["F_bdzd"].ToString().Trim();
        //                djid = getblh + "^" + dtyhbh.Rows[0]["F_BD_BGXH"].ToString().Trim() + "^BD";
        //            }
        //            else
        //            {
        //                hash = dtyhbh.Rows[0]["F_blzd"].ToString().Trim();
        //                djid = getblh + "^1^CG";
        //            }

        //        signtexten = GDCACom.GDCA_Base64Encode(djid + "^" + hash);
        //        signValue = GDCACom.GDCA_OpkiSignData("LAB_USERCERT_SIG", 4, Csigncert, signtexten, 32772, 0);
        //        ret = GDCACom.GetError();

        //        GDCAWSCom.Finalize();
        //        GDCACom.GDCA_Finalize();
        //        if (ret != 0)
        //        {
        //            if (msg == "1")
        //            MessageBox.Show("ǩ��ʧ��");
                    
        //            return "1";
        //        }

        //        string  sql_str_ca="select * from T_CAXX  where F_BLH='"+getblh+"' and F_BGLX='"+bglx+"'  and F_BGXH='"+bgxh+"'";
        //        DataTable dt_CA = new DataTable();
        //        dt_CA = sqldb.Sql_DataAdapter(constr_jcxx, sql_str_ca, ref execp);
        //        if (dt_CA.Rows.Count > 0)
        //        {
        //            sqldb.Sql_ExecuteNonQuery(constr_jcxx, "delete  from T_CAXX  where F_BLH='" + getblh + "' and F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'", ref execp);
        //        }
        //        string insert_ca = "insert into T_CAXX(F_BLH,F_BGLX,F_BGXH,F_CZY,F_BLZD,F_signtexten,F_signValue,F_TimeStamp) values('"+getblh+"','"+bglx+"','"+bgxh
        //            + "','" + yhmc + "','" + hash + "','"+signtexten+"','"+signValue+"','')";
        //        sqldb.Sql_ExecuteNonQuery(constr_jcxx, insert_ca, ref execp);

        //       //// ////�����ǩ����֤/////////////////////////
        //       //// int ret;
        //      // ret = GDCAWSCom.PKCS1Verify("123", "123", Csigncert, signtext, signValue);
        //       //// ret = GDCAWSCom.GetError();
        //       //// if (ret != 0)
        //       //// {
        //       ////     MessageBox.Show("����������֤ǩ��ʧ��");
        //       ////     return;
        //       //// }
        //       //// MessageBox.Show("����������֤ǩ���ɹ�");

        //       ///////�Ӹ�ʱ�������֤//////////
        //       //// String TimeStamp = GDCAWSCom.SealTimeStamp("123", "123", signtext);
        //       //// int ret = GDCAWSCom.GetError();
        //       //// if (ret != 0)
        //       //// {
        //       ////     MessageBox.Show("�Ӹ�ʱ���ʧ��");
        //       ////     return "0";
        //       //// }
        //       //// textBox4.Text = TimeStamp;
        //       //// String StspCert = "MIIENDCCA52gAwIBAgIPByAAIAcEAwEAEAAAABYpMA0GCSqGSIb3DQEBBQUAMIIBJDENMAsGA1UEBh4EAEMATjEbMBkGA1UECB4SAEcAdQBhAG4AZwBkAG8AbgBnMRswGQYDVQQHHhIARwB1AGEAbgBnAHoAaABvAHUxPTA7BgNVBAoeNABHAEQAQwBBACAAQwBlAHIAdABpAGYAaQBjAGEAdABlACAAQQB1AHQAaABvAHIAaQB0AHkxRzBFBgNVBAsePgBHAHUAYQBuAGcAZABvAG4AZwAgAEMAZQByAHQAaQBmAGkAYwBhAHQAZQAgAEEAdQB0AGgAbwByAGkAdAB5MVEwTwYDVQQDHkgARwBEAEMAQQAgAEcAdQBhAG4AZwBkAG8AbgBnACAAQwBlAHIAdABpAGYAaQBjAGEAdABlACAAQQB1AHQAaABvAHIAaQB0AHkwHhcNMDcwNDAzMDMwOTA2WhcNMTIwNDAzMDMwOTA2WjCB1DENMAsGA1UEBh4EAEMATjEPMA0GA1UECB4GXn9OHHcBMQ8wDQYDVQQHHgZef13eXgIxPTA7BgNVBAoeNABHAEQAQwBBACAAQwBlAHIAdABpAGYAaQBjAGEAdABlACAAQQB1AHQAaABvAHIAaQB0AHkxRzBFBgNVBAsePgBHAHUAYQBuAGcAZABvAG4AZwAgAEMAZQByAHQAaQBmAGkAYwBhAHQAZQAgAEEAdQB0AGgAbwByAGkAdAB5MRkwFwYDVQQDHhAARwBEAEMAQQAgAFQAUwBQMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCyyA/XFVWhEpItJxBa8iYo5bjDPkJ/WkEJtyqpjx1cCyf0HTGP/1vBLSbZ8fVqq7HUjOBzu8kNaLcRWHPgi8rfY9H+mWMRqpVqOYemsxdcp5FdutaY05ptiH3Thbqcnm1RIUZKXwxWx3y0FxGCkv9TVwqV8pEh564tZDqLOmnQjwIDAQABo4G0MIGxMB0GA1UdDgQWBBTrLbGqJjDcw4ULA2AjSXTqAKXwwzAOBgNVHQ8BAf8EBAMCBkAwHwYDVR0jBBgwFoAUd0MJEPcVizqwEdJMeweUBPAUVEYwFwYFKlYVAQEBAQAECzEzNTgwMDAwODg1MBUGBSpWFQECAQEABAlnZC5nb3YuY24wFwYFKlYVAQMBAQAECzA3MDEyMDAwNzA0MBYGA1UdJQEB/wQMMAoGCCsGAQUFBwMIMA0GCSqGSIb3DQEBBQUAA4GBAF+ddxSV1rSrW+IyHeAJ/KszObKeDMtS64/EMaUHYRzZxIo6+B5fCBi+kJYVqJCgBwF+Nq/nPpxHTUlrx3Py3KxnGECPJ/acJWQbb2FlLiW/096+PSg5srtt3Tsrt61U1X6lDYSWmPX0pMYAzlaXrYNP0Jm4hnMuEbOrfPefgHv5";
        //      // String StampTime = GDCAWSCom.VerifyTimeStamp("123", "123", signtext, TimeStamp, StspCert);
        //       //// ret = GDCAWSCom.GetError();
        //       //// if (ret != 0)
        //       //// {
        //       ////     MessageBox.Show("ʱ�����֤ʧ��");
        //       ////     return;
        //       //// }
        //       //// MessageBox.Show("ʱ�����֤�ɹ�:" + StampTime);

        //        //ǩ��

        //        return "1";
        //    }
        //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        //    ///
        //    ///ȡ�����ǰ
        //    ///
        //    if (type == "QXSH" && type2 == "SHQTYSZQZ")
        //    {
               
        //        if (qxshyzkey.Trim() == "1")
        //        {
        //            if (debug == "1")
        //                MessageBox.Show("ȡ�����ǰ��֤KEY������");
        //            string key_yhmc = ""; string key_sfzh = ""; string key_sydw = "";
        //            bool yzbool = YZKEY(debug, ref key_sydw,ref  key_yhmc,ref key_sfzh);
        //            GDCAWSCom.Finalize();
        //            GDCACom.GDCA_Finalize();

        //            if (!yzbool)
        //                return "0";
        //            if (key_yhmc.Trim() != yhmc.Trim())
        //            {
        //                if (msg == "1")
        //                MessageBox.Show("KEY�û��뵱ǰϵͳ�û���һ�£���֤��ͨ����" + key_yhmc); return "0";
        //            }

        //            if (sydw.Trim() != "")
        //            {
        //                if (key_sydw.Trim() != sydw.Trim())
        //                {
        //                    if (msg == "1")
        //                    MessageBox.Show("ʹ�õ�λ��һ�£���֤��ͨ����" + key_sydw); return "0";
        //                }
        //            }
        //            if (yzsfzh.Trim() == "1")
        //            {
        //                if (key_sfzh.Trim() != yhbh.Trim())
        //                {
        //                    if (msg == "1")
        //                    MessageBox.Show("���֤�Ų�һ�£���֤��ͨ����" + key_sfzh); return "0";
        //                }
        //            }
        //        }
        //        return "1";
        //    }
        //    /////////////////////////////////////////////////////////////////////////////////////////////////
        //    ///
        //    ///ȡ����˺�
        //    ///
        //    if (type == "QXQZ" && type2 == "TYSZQZ")
        //    {
        //        LG_ZGQ.SqlDB_ZGQ sqldb = new SqlDB_ZGQ();
        //        if (debug == "1")
        //            MessageBox.Show("ȡ��ǩ�֡�����");
        //        try
        //        {
        //            if (debug == "1")
        //                MessageBox.Show("ɾ���м���¼������");
        //            string sqlstr = "delete from  T_CAXX where F_blh='" + getblh + "'  and F_BGLX='" + bglx.Trim() + "' and F_BGXH='" + bgxh + "'";
        //            string exp = "";
        //            int x = sqldb.Sql_ExecuteNonQuery(constr_jcxx, sqlstr, ref exp);
        //            if (debug == "1")
        //                MessageBox.Show("ȡ��ǩ����ɡ���");
        //            return "1";
        //        }
        //        catch (Exception ee5)
        //        {
        //          MessageBox.Show("ȡ��ǩ���쳣��" + ee5.Message);
        //            return "1";
        //        }
        //    }

        //    MessageBox.Show("��Ч������" + yhxx);
        //    return "1";





        //}

        //private bool YZKEY(string debug, ref  string key_sydw, ref string key_yhmc, ref string key_sfzh)
        //{
        //    if (wsurl.Trim() == "")
        //    {
        //        MessageBox.Show("��֤��ַ����Ϊ��"); return false;
        //    }

        //    //��ʼ��
        //    GDCAWSCom.Initialize(wsurl);
         
        //    Ssigncert = GDCAWSCom.GetSigCert();
        //    //log.WriteMyLog("1-3");
        //    int ret;
        //    ret = GDCAWSCom.GetError();
        //    if (ret != 0)
        //    {
        //        MessageBox.Show("�ؼ���ʼ��ʧ��");

        //        return false;
        //    }

        //    if (debug == "1")
        //        MessageBox.Show("�ؼ���ʼ���ɹ�");

          
        //    //-----------------------------------------

        //    int devicetype = 0;
        //    devicetype = GDCACom.GDCA_GetDeviceType();
        //    if (devicetype == 0)
        //    {
        //        if (msg == "1")
        //        MessageBox.Show("δ��⵽KEY������KEY�Ƿ����");
               
        //        return false;
        //    }
          
        //    ret = GDCACom.GDCA_SetDeviceType(devicetype);

        //    if (ret != 0)
        //    {
        //        if (msg == "1")
        //        MessageBox.Show("�����豸����ʧ��,����KEY�Ƿ����");
                
        //        return false;

        //    }
          
        //    ret = GDCACom.GDCA_Initialize();
        //    if (ret != 0)
        //    {
        //        if (msg == "1")
        //        MessageBox.Show("��ʼ���豸ʧ��,����KEY�Ƿ����");
               
        //        return false;
        //    }
        //    //��֤key����
         
        //    if (GDCACom.GDCA_isLogin(2).ToString() != "0" )
        //    {
        //        do
        //        {
        //            frm_gdca_pwd dd = new frm_gdca_pwd();
        //            dd.ShowDialog();
        //            if (dd.DialogResult != DialogResult.Yes)
        //            {
        //                if (msg == "1")
        //                    MessageBox.Show("����δ����,������֤KEY");
        //                return false;
        //            }
        //            if (dd.key_pwd.Trim() == "")
        //            {
        //                if (msg == "1")
        //                MessageBox.Show("����δ����,������֤KEY");
                      
        //                return false;
        //            }
                  
        //            ret = GDCACom.GDCA_Login(2, dd.key_pwd);
        //            if (ret != 0)
        //            {
        //            }
        //        }
        //        while (ret != 0);
        //    }

        //    //��ȡ֤��
           
        //    Csigncert = GDCACom.GDCA_ReadLabel("LAB_USERCERT_SIG", 7);
        //    ret = GDCACom.GetError();
        //    if (ret != 0)
        //    {
        //        if (msg == "1")
        //            MessageBox.Show("��ȡǩ��֤��ʧ�ܣ�Csigncert=" + Csigncert);
              
        //        return false;
        //    }
        //     key_sydw = GDCACom.GDCA_GetInfoByOID(Csigncert, 1, "2.5.4.10", 0);
        //     key_yhmc = GDCACom.GDCA_GetInfoByOID(Csigncert, 1, "2.5.4.3", 0);
        //     key_sfzh = GDCACom.GDCA_GetInfoByOID(Csigncert, 2, "1.2.86.11.7.1", 0);
          
        //    //if (GDCACom.GetError() != 0)
        //    //{
        //    //    if (msg == "1")
        //    //    MessageBox.Show("��ȡǩ��֤��ʧ��2");
              
        //    //    return false;
        //    //}
        //     if (debug == "1")
        //         MessageBox.Show("������" + key_sydw + "\r\n�û�����" + key_yhmc + "\r\n���֤�ţ�" + key_sfzh + "\r\n" + Csigncert);
        //    //��֤֤��
        //    //String trustid = GDCAWSCom.CheckCert("123", "123", Csigncert);
        //    //ret = GDCAWSCom.GetError();
        //    //if (ret != 0)
        //    //{
        //    //    MessageBox.Show("֤����֤ʧ�ܣ�" + trustid);
        //    //    return false;
        //    //}
        //    //if (trustid.Trim() == "")
        //    //{
        //    //    MessageBox.Show("֤�����η����Ϊ�գ�");
        //    //    return false;
        //    //}

         

        //    return true;
        //}
        //public static string getSZ_String(string Section, string Ident, string Default)
        //{
        //    string T_szvalue = "";
        //    string szvalue = "";

        //    szvalue = f.ReadString(Section, Ident, "").Replace("\0", "").Trim();

        //    if (szvalue.Trim() == "")
        //    {
        //        try
        //        {
        //            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        //            DataTable DT_sz = new DataTable();
        //            DT_sz = aa.GetDataTable("select top 1 F_SZZ from T_SZ where F_XL='" + Ident + "'  and F_DL='" + Section + "'", "sz");

        //            if (DT_sz.Rows.Count <= 0)
        //            {
        //                return Default;
        //            }
        //            else
        //            {
        //                T_szvalue = DT_sz.Rows[0]["F_SZZ"].ToString().Trim();
        //                return T_szvalue;
        //            }
        //        }
        //        catch (Exception e1)
        //        {
        //            return Default;
        //        }
        //    }
        //    else
        //        return szvalue;

        //}
    }
}
