
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using dbbase;
using readini;

using System.Security.Cryptography;
using System.Xml;
using System.Xml.XPath;
using System.Data.Odbc;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using LG_ZGQ;

namespace PathnetCAzgq
{
    //厦门中山医院
    public  class xmzsyy
    {

        [DllImport("fjcaLibrary.dll")]
        extern static byte GetDeviceInfoOnUsbKey(StringBuilder outData);
        [DllImport("fjcaLibrary.dll")]
        extern static byte OpenUsbKey(string passWord);
        [DllImport("fjcaLibrary.dll")]
        extern static byte SignDataOnUsbKey(string originData, StringBuilder signData);
        [DllImport("fjcaLibrary.dll")]
        extern static byte GetSignCertOnUsbKey(StringBuilder signCert);
        [DllImport("fjcaLibrary.dll")]
        extern static byte CloseUsbKey();
        [DllImport("fjcaLibrary.dll")]
        extern static byte GetRandomByLengthOnMultiServer(string serverList, int length, StringBuilder outData);
        [DllImport("fjcaLibrary.dll")]
        extern static byte LoginAuthenticationOnMultiServer(string serverList, string signData, string cert, string originData);
        [DllImport("fjcaLibrary.dll")]
        extern static byte GetCertName(string cert, StringBuilder certName);
        [DllImport("fjcaLibrary.dll")]
        extern static byte GetCertExtensionInfoById(string cert, string id, StringBuilder info);
        [DllImport("fjcaLibrary.dll")]
        extern static byte IsUsbKeyConnected();

      
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public string ca(string yhxx)
        {
            bool yzpassword = false;

           
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
           
            //-------获取sz中设置的参数---------------------
            string debug = f.ReadString("CA", "debug", "");
            string msg = f.ReadString("CA", "message", "1");
            string ftpServerIP = f.ReadString("ftp", "ftpip", "");
            string ftpUserID = f.ReadString("ftp", "user", "");
            string ftpPassword = f.ReadString("ftp", "pwd", "");
            string ftpszqmbmp = f.ReadString("ftp", "szqmbmp", "pathsetup/pathqc/rpt-szqm/ysbmp");
            string IP = f.ReadString("CA", "IP", "192.1.33.143:7000").Trim();
                string yzzsyxx = f.ReadString("CA", "yzzsyxx", "0").Trim();


                string ServerIP = f.ReadString("sqlserver", "Server", "192.10.33.84");
                string sqlcon = "Server=192.10.33.84;Database=pathnet_ca;User ID=pathnet;Password=4s3c2a1p;Trusted_Connection=False;";
         
            string getblh = "";
            string type = "";
            string type2 = "";
            string yhm = "";

            string yhmc = "";
            string yhbh = "";
            string yhmm = "";
            string bglx = "";
            string bgxh = "";
            string keyname = "";
         
            string[] getyhxx = yhxx.Split('^');
            if (getyhxx.Length == 5)
            {
                type = getyhxx[0];
                yhm = getyhxx[1];
                yhmc = getyhxx[3];
                yhbh = getyhxx[2];
                yhmm = getyhxx[4];
            }
            else
            {
                type2 = getyhxx[0];
                getblh = getyhxx[1];
                bgxh = getyhxx[2];
                bglx = getyhxx[3].ToLower();
                type = getyhxx[4];
                yhm = getyhxx[5];
                yhmc = getyhxx[6];
                yhbh = getyhxx[7];
                yhmm = getyhxx[8];
            }


        
            string password = string.Empty;
            //string KEY_sbDeviceInfo = f.ReadString("CA_KEY", "sbDeviceInfo", "").Trim();
            //string KEY_userName = f.ReadString("CA_KEY", "userName", "").Trim();
            //password = f.ReadString("CA_KEY", "pwd", "").Trim();
            //if (key == "0")
            //{
            //    return "1";
            //}

          //------------------------------------------------------------------
          //------------------------------------------------------------------
          //审核前执行 ,验证KEY
          //------------------------------------------------------------------
          //------------------------------------------------------------------
         
            if (type == "SH")
            {
           
                //if (!axSBFjCAEnAndSign1.OpenFJCAUSBKey())
                //{
                //    axSBFjCAEnAndSign1.CloseUSBKey();
                //    return;
                //}
                //axSBFjCAEnAndSign1.CloseUSBKey();


                //读取Key的硬件介质号
                //if (debug == "1")
                //    MessageBox.Show("读取Key的硬件介质号");

                //StringBuilder sbDeviceInfo = new StringBuilder(256);
                //if (GetDeviceInfoOnUsbKey(sbDeviceInfo) == 0)
                //{
                //    if (msg=="1")
                //    MessageBox.Show("读取Key的硬件介质号失败，请确认Key已经插入电脑。");
                //return "0";
                //}
                //if (debug == "1")
                //    MessageBox.Show("读取Key的硬件介质号:" + sbDeviceInfo.ToString());

               // yzpassword = false;

    //            if (sbDeviceInfo.ToString() == KEY_sbDeviceInfo)
    //             {
                     
    //                     if (password != "")
    //                         yzpassword = true;
                    

    //             }

    //            bool  savepwd=true;

    //yzmm:  
    //             if (!yzpassword)
    //            {
                             
    //                Frm_XMZSYY_Login login = new Frm_XMZSYY_Login();
    //                if (login.ShowDialog() != DialogResult.Yes)
    //                {
    //                    if (msg == "1")
    //                        MessageBox.Show("用户登录取消,验证失败");
    //                    return "0";
    //                }
    //                password = login.password;
    //                 savepwd=login.savepwd;
    //                if (password == string.Empty)
    //                {
    //                    if (msg == "1")
    //                        MessageBox.Show("密码输入错误,不能为空");
    //                    return "0";
    //                }

    //            }

                if (debug == "1")
                    MessageBox.Show("打开Key");
                //打开Key
                if (OpenUsbKey("") == 0)
                {
                   
                        //if (MessageBox.Show("打开Key失败,可能密码输入错误\r\n是否重试", "密码验证", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        //{
                        //    yzpassword = false;
                        //    goto yzmm;
                        //}
                        //else
                    if (msg == "1")
                        MessageBox.Show("打开Key失败,Key未插入或密码输入错误");
                            return "0";
                }
                if (debug == "1")
                    MessageBox.Show("打开Key成功");
                //if (savepwd)
                //{
                //    f.WriteString("CA_KEY", "sbDeviceInfo", sbDeviceInfo.ToString());
                //    // f.WriteString("CA_KEY", "userName", KEY_YH);
                //    f.WriteString("CA_KEY", "pwd", password);
                //}
                //else
                //{
                //    f.WriteString("CA_KEY", "sbDeviceInfo", "");
                //    // f.WriteString("CA_KEY", "userName", KEY_YH);
                //    f.WriteString("CA_KEY", "pwd", "");
                //}
               if(debug=="1")
                   MessageBox.Show("获取签名证书");
                //获取签名证书
                StringBuilder sbSignCert = new StringBuilder(4096);
                if (GetSignCertOnUsbKey(sbSignCert) == 0)
                {
                    CloseUsbKey();
                    if (msg == "1")
                        MessageBox.Show("获取签名证书失败");
                    return "0";
                }
                if (debug == "1")
                MessageBox.Show("证书："+sbSignCert.ToString());

                //sfzh
                StringBuilder sbInfo = new StringBuilder(1024);
                GetCertExtensionInfoById(sbSignCert.ToString(), "1.2.86.21.1.1", sbInfo);

                string KEY_SFZH = sbInfo.ToString().Substring(9, sbInfo.Length - 9);
                if (debug == "1")
                MessageBox.Show("身份证号："+KEY_SFZH);

                // 证书有效性验证通过，展示证书信息。

                StringBuilder sbCertName = new StringBuilder(1024);
                if (GetCertName(sbSignCert.ToString(), sbCertName) == 0)
                {
                    CloseUsbKey();
                    if (msg == "1")
                        MessageBox.Show("获取签名证书失败");
                    return "0";
                }
                string KEY_YH = sbCertName.ToString();
                if (debug == "1")
                MessageBox.Show("KEY用户：" + KEY_YH);


              
                //关闭Key
                CloseUsbKey();

                //验证用户名

                if (yhmc.Trim() != KEY_YH)
                {
                    if (msg == "1")
                        MessageBox.Show("软件使用者与KEY用户不一致,验证失败！");
                    return "0";
                }

                if (yhbh.Trim() != KEY_SFZH)
                {
                    if (msg == "1")
                        MessageBox.Show("用户身份证号与KEY中身份证号不一致,验证失败！");
                    return "0";
                }

                if (yzzsyxx == "1")
                {
                    if (debug == "1")
                        MessageBox.Show("获取随机数");

                    //// 获取随机数
                    StringBuilder sbRandom = new StringBuilder(64);
                    if (GetRandomByLengthOnMultiServer(IP, 32, sbRandom) == 0 || sbRandom.ToString().Length <= 0 || sbRandom.ToString() == string.Empty)
                    {
                        CloseUsbKey();
                        if (msg == "1")
                            MessageBox.Show("从服务器获取随机数失败");
                        return "0";
                    }
                    if (debug == "1")
                        MessageBox.Show("获取随机数完成");
                    //签名SignDataOnUsbKey
                  
                    StringBuilder sbSignData = new StringBuilder(4096);
                    if (SignDataOnUsbKey(sbRandom.ToString(), sbSignData) == 0)
                    {
                        CloseUsbKey();
                        if (msg == "1")
                            MessageBox.Show("签名失败");
                        return "0";
                    }
                    if (debug == "1")
                        MessageBox.Show("签名完成");

                    // 验证证书有效性
                    if (LoginAuthenticationOnMultiServer(IP, sbSignData.ToString(), sbSignCert.ToString(), sbRandom.ToString()) == 0)
                    {
                        CloseUsbKey();
                        if (msg == "1")
                            MessageBox.Show("连接服务器验证证书有效性失败");
                        return "0";
                    }
                    if (debug == "1")
                        MessageBox.Show("验证证书有效性完成");
                }

                if (debug == "1")
                MessageBox.Show("验证通过");

                return "1";
            
                    
          }
            //***************************************************************
            ////审核后执行，数字签名，生成pdf和edu文件，并上传，写数据库等
           
            if (type == "QZ")
            {
                //***************************************************************
                if(debug=="1")
                MessageBox.Show("审核签字");


            string origindata = ""; 
           
             DataTable dt_jcxx = new DataTable();
            dt_jcxx=aa.GetDataTable("select * from T_JCXX where  F_BLH='" + getblh + "'","cgbg");

            if (dt_jcxx == null)
            {
                if (msg == "1")
                    MessageBox.Show("连接数据库异常");
                return  "0";
            }
            if (dt_jcxx.Rows.Count <= 0)
            {
                if (msg == "1")
                    MessageBox.Show("T_JCXX查询数据异常");
                return "0";
            }
                 string  shys=dt_jcxx.Rows[0]["F_shys"].ToString();
                string  spare5=dt_jcxx.Rows[0]["F_spare5"].ToString();
            if (bglx == "cg")
            {
                if (dt_jcxx.Rows[0]["F_BGZT"].ToString() != "已审核")
                {
                    if (msg == "1")
                        MessageBox.Show("报告未审核");
                    return "0";
                }
                origindata = "病理号：" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&报告类型：常规报告&报告序号：" + bgxh + "&&性别：" + dt_jcxx.Rows[0]["F_XB"].ToString() + "&年龄：" + dt_jcxx.Rows[0]["F_nl"].ToString()
                    + "&住院号：" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&门诊号：" + dt_jcxx.Rows[0]["F_mzh"].ToString() + "&科室：" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                    + "&病理诊断：" + dt_jcxx.Rows[0]["F_blzd"].ToString() + "&报告医生：" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "&复诊医生：" + dt_jcxx.Rows[0]["F_FZYS"].ToString()
                    + "&审核医生：" + dt_jcxx.Rows[0]["F_SHYS"].ToString() + "&报告日期：" + dt_jcxx.Rows[0]["F_bgrq"].ToString() + "&审核日期：" + dt_jcxx.Rows[0]["F_spare5"].ToString();

            }
            //补充审核
            if (bglx == "bc")
            {
                DataTable dt_bc = new DataTable();
              dt_bc=  aa.GetDataTable("select * from T_BCBG where  F_BLH='" + getblh + "' and F_BC_BGZT='已审核'and F_BC_BGXH='" + bgxh + "'","bcbg");
              if (dt_bc == null)
                {
                    if (msg == "1")
                        MessageBox.Show("连接数据库异常");
                    return "0";
                }
                if (dt_bc.Rows.Count <= 0)
                {
                    if (msg == "1")
                        MessageBox.Show("T_BCBG查询数据异常");
                    return "0";
                }
                origindata = "病理号：" + dt_jcxx.Rows[0]["f_blh"].ToString()+"&报告类型：补充报告&报告序号：" + bgxh + "&性别：" + dt_jcxx.Rows[0]["F_XB"].ToString() + "&年龄：" + dt_jcxx.Rows[0]["F_nl"].ToString()
                   + "&住院号：" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&门诊号：" + dt_jcxx.Rows[0]["F_mzh"].ToString() + "&科室：" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                   + "&病理诊断：" + dt_bc.Rows[0]["F_BCZD"].ToString() + "&报告医生：" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "&复诊医生：" + dt_bc.Rows[0]["F_bc_FZYS"].ToString()
                   + "&审核医生：" + dt_bc.Rows[0]["F_bc_SHYS"].ToString() + "&报告日期：" + dt_bc.Rows[0]["F_bc_bgrq"].ToString() + "&审核日期：" + dt_bc.Rows[0]["F_bc_spare5"].ToString();

                    shys=dt_bc.Rows[0]["F_bc_shys"].ToString();
                  spare5=dt_bc.Rows[0]["F_bc_spare5"].ToString();
            }
            //小冰冻审核
            if (bglx == "bd")
            {

                DataTable dt_bd = new DataTable();
                dt_bd = aa.GetDataTable("select * from T_BDBG  where  F_BLH='" + getblh + "' and  F_BD_BGZT='已审核' and F_BD_BGXH='" + bgxh + "'", "bcbg");
                if (dt_bd == null)
                {
                    if (msg == "1")
                        MessageBox.Show("连接数据库异常");
                    return "0";
                }
                if (dt_bd.Rows.Count <= 0)
                {
                    if (msg == "1")
                        MessageBox.Show("T_BDBG查询数据异常");
                    return "0";
                }
                origindata = "病理号：" + dt_jcxx.Rows[0]["f_blh"].ToString() + "&报告类型：冰冻报告&报告序号：" + bgxh + "&性别：" + dt_jcxx.Rows[0]["F_XB"].ToString() + "&年龄：" + dt_jcxx.Rows[0]["F_nl"].ToString()
                   + "&住院号：" + dt_jcxx.Rows[0]["F_zyh"].ToString() + "&门诊号：" + dt_jcxx.Rows[0]["F_mzh"].ToString() + "&科室：" + dt_jcxx.Rows[0]["F_sjks"].ToString()
                   + "&病理诊断：" + dt_bd.Rows[0]["F_BdZD"].ToString() + "&报告医生：" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "&复诊医生：" + dt_bd.Rows[0]["F_fz_FZYS"].ToString()
                   + "&审核医生：" + dt_bd.Rows[0]["F_bd_SHYS"].ToString() + "&报告日期：" + dt_bd.Rows[0]["F_bd_bgrq"].ToString() ;

                     shys=dt_bd.Rows[0]["F_bd_shys"].ToString();
                  spare5=dt_bd.Rows[0]["F_bd_bgrq"].ToString();
            }

            if (origindata.Trim() == "")
            {
                if (msg == "1")
                    MessageBox.Show("数字签名内容为空");
                return "0";
            }

         
            if (debug == "1")
                MessageBox.Show("打开key");

             
                if (OpenUsbKey("") == 0)
                {
                    if (msg == "1")
                    MessageBox.Show("打开Key失败,签名失败");
                    return "0";
                }
                if (debug == "1")
                    MessageBox.Show("打开key完成");
              
              
                //获取签名证书
                StringBuilder sbSignCert = new StringBuilder(4096);
                if (GetSignCertOnUsbKey(sbSignCert) == 0)
                {
                    CloseUsbKey();
                    if (msg == "1")
                    MessageBox.Show("获取签名证书失败");
                    return "0";
                }
                if (debug == "1")
                    MessageBox.Show("获取签名证书完成");
              
                StringBuilder sbSignData = new StringBuilder(4096);

                if (SignDataOnUsbKey(origindata, sbSignData) == 0)
                {
                    CloseUsbKey();
                    if (msg == "1")
                    MessageBox.Show("签名失败");
                    return "0";
                }
                if (debug == "1")
                    MessageBox.Show("签名完成");

                SqlDB_ZGQ sqldb = new SqlDB_ZGQ();
                string errmsg = "";
              int x=  sqldb.Sql_ExecuteNonQuery(sqlcon, "insert into T_CAXX(F_BLH,F_BGLX,F_BGXH,F_SHYS,F_SPARE5,F_CZY,F_QZNR,F_SignCert,F_SignData) values('" + getblh + "','" + bglx + "','" + bgxh
                    + "','"+shys+"','"+spare5+"','" + yhmc + "','" + origindata + "','" + sbSignCert.ToString() + "','" + sbSignData.ToString() + "')", ref errmsg);
              if (debug == "1")
              {
                  if(x>=1)
                  MessageBox.Show("写入T_CAXX完成");
                  else
                  MessageBox.Show("写入T_CAXX失败："+errmsg);
              }

                   string szqmlj = getSZ_String("view", "szqmlj", @"\\192.10.33.84\pathqc\rpt-szqm\YSBMP\");
                   if (!File.Exists(szqmlj + yhmc + ".bmp"))
                   {

                       DataTable dt_ca = new DataTable();
                       dt_ca = sqldb.Sql_DataAdapter(sqlcon, "select TOP 1 * from t_userInfo  where IdCardNumber='" + yhbh.Trim() + "'", ref errmsg);

                       if (dt_ca == null)
                       {
                           if (msg == "1")
                               MessageBox.Show("连接数据库pathnet_CA异常");
                           return "0";
                       }
                       if (dt_ca.Rows.Count <= 0)
                       {
                           MessageBox.Show("未能查询到用户(" + yhmc + ")的签名图片");
                           return "0";
                       }
                       if (debug == "1")
                           MessageBox.Show("查询t_userInfo完成");

                       try
                       {
                           byte[] MyData = new byte[0];
                           MyData = (byte[])dt_ca.Rows[0]["Image"];//读取图片的位流
                           int ArraySize = MyData.GetUpperBound(0);//获得数据库中存储的位流数组的维度上限，用作读取流的上限

                           FileStream fs = new FileStream(@"c:\temp\" + yhmc + ".jpg", FileMode.OpenOrCreate, FileAccess.Write);
                           fs.Write(MyData, 0, ArraySize);
                           fs.Close();


                         Bitmap source = new Bitmap(@"c:\temp\" + yhmc + ".jpg");
                        Bitmap bmp = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                 
                     Graphics.FromImage(bmp).DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height));

                    
                           bmp.Save(szqmlj + yhmc + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                           if (szqmlj.Substring(0, 2) != "//")
                               ZgqFtpWeb.FtpUpload(ftpUserID, ftpPassword, ftpServerIP, szqmlj + yhmc + ".bmp", ftpszqmbmp, ref errmsg);
                           bmp.Save(szqmlj + yhmc + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
             



                           //File.Copy(@"c:\temp\" + yhmc + ".jpg", szqmlj + yhmc + ".jpg");
                       }
                       catch (Exception e3)
                       {
                           MessageBox.Show(e3.Message);
                           return "0";
                       }
                       if (debug == "1")
                           MessageBox.Show("上传签字图片完成");
                   }
                if (debug == "1")
                    MessageBox.Show("签字完成");
                return "1";
            
            }

            

            //***************************************************************
            //////取消审核前
            if (type == "QXSH")  
            {
                return "1";
            }
           



            //***************************************************************
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            //////取消审核后 ,同时删除数据库ca记录
            //------------------------------------------------------------------
            //------------------------------------------------------------------
            if (type == "QXQZ" )  //&& (bglx == "BC" || bglx == "BD")
            {
               

                SqlDB_ZGQ sqldb = new SqlDB_ZGQ();
                string errmsg = "";
                sqldb.Sql_ExecuteNonQuery(sqlcon, "delete from  T_CAXX  where  F_BLH='" + getblh + "'  and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'", ref errmsg);
                return "1";
           }
            return "1";
           }

        public static string getSZ_String(string Section, string Ident,string Default)
        {
            string T_szvalue = "";
            string szvalue = "";

            szvalue = f.ReadString(Section, Ident, "").Replace("\0", "").Trim();

            if (szvalue.Trim() == "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable DT_sz = new DataTable();
                    DT_sz = aa.GetDataTable("select top 1 F_SZZ from T_SZ where F_XL='" + Ident + "'  and F_DL='" + Section + "'", "sz");

                    if (DT_sz.Rows.Count <= 0)
                    {
                        return Default;
                    }
                    else
                    {
                        T_szvalue = DT_sz.Rows[0]["F_SZZ"].ToString().Trim();
                            return T_szvalue;
                    }
                }
                catch (Exception e1)
                {
                    return Default;
                }
            }
            else
                return szvalue;

        }


   

    }
}
