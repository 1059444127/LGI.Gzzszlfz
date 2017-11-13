
using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OracleClient;
using System.Drawing;
using ZgqClassPub;
namespace PathHISZGQJK
{
    /// <summary>
    /// //广东东莞市东华医院
    /// </summary>
   class gddgsdhyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug)
        {

             debug = f.ReadString("savetohis", "debug", "");
            string connectionString = f.ReadString("savetohis", "odbcsql", "");

            string jcxx_fs = f.ReadString("savetohis", "jcxx_fs", "0");
            string tj_constr = f.ReadString("savetohis", "tj_odbcsql", "Data Source=172.18.100.20;Initial Catalog=tj_db;User Id=sa;Password=sql;");
            string rtnmsg = "";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");

            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog(blh+",病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog(blh + ",病理号有错误！");
                return;
            }

            sendDX( blh,  bljc,  debug);

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() != "住院" && bljc.Rows[0]["F_brlb"].ToString().Trim() != "体检")
            {
                log.WriteMyLog(blh + ",非住院或体检病人，不处理！");
                return;
            }

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "体检")
            {
                if (bljc.Rows[0]["F_SQXH"].ToString().Trim() == "")
                {
                    log.WriteMyLog(blh + ",无体检申请号，不处理！");
                    return;
                }

                if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                {
                   
                        string cmdText = "update  tj_pacs_resulto_temp set res_char='" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "',res_date='" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',res_doctor='" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "',res_send_date='"+DateTime.Today.ToString("yyyy-MM-dd")+"',res_flag=2  where   res_no='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";

                        int x = SQL_ExecuteNonQuery(tj_constr, cmdText, ref rtnmsg);
                        if (debug == "1" && rtnmsg.Trim()!="")
                            MessageBox.Show(rtnmsg);

                        if (x > 0)
                        {
                            aa.ExecuteSQL("update T_JCXX  set F_SCBJ='1' where F_BLH='" + blh.Trim() + "'");
                        }
                    
                }
                else
                {
                    if (bljc.Rows[0]["F_SCBJ"].ToString().Trim() == "1")
                    {

                        string cmdText = "update  tj_pacs_resulto_temp set res_char='',res_date='',res_doctor='',res_send_date='" + DateTime.Today.ToString("yyyy-MM-dd") + "',res_flag=0  where   res_no='" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                        
                        int x = SQL_ExecuteNonQuery(tj_constr, cmdText,ref rtnmsg);
                    }
                }
                return;
            }

            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核" && bljc.Rows[0]["F_brlb"].ToString().Trim()== "住院")
            {

                if (bljc.Rows[0]["F_ZYH"].ToString().Trim() == "")
                {
                    log.WriteMyLog(blh + ",无住院号，不处理！");
                    return;
                }
                string jpgname = "";
                string ftpstatus = "";
            
                try
                {  

                    mdjpg mdj = new mdjpg();

                    for (int x = 0; x < 3; x++)
                    {
                        if (!File.Exists("C:\\temp\\" + blh + "\\" + blh + "_1.jpg"))
                        {
                            mdj.BMPTOJPG(blh, ref jpgname, "", "");
                        }
                        else
                        {
                            break;
                        }
                    }

                      
                }
                catch (Exception ee)
                {
                    if (jcxx_fs=="1")
                      aa.ExecuteSQL("update T_jcxx_fs  set F_BZ='生成JPG异常：" + ee.Message + "' where F_BLH='" + blh.Trim() + "'");
                    if(debug=="1")
                    MessageBox.Show("生成JPG异常："+ee.Message);
                log.WriteMyLog(blh + ",生成JPG异常：" + ee.Message);
                }

              
                ///////////////////////////////////////////
                string imgPath = "C:\\temp\\" + blh + "\\" + blh + "_1.jpg";//图片文件所在路径  
                Byte[] imgByte;
                if (!File.Exists("C:\\temp\\" + blh + "\\" + blh + "_1.jpg"))
                {
                    imgByte = new Byte[1];
                    if (jcxx_fs == "1")
                        aa.ExecuteSQL("update T_jcxx_fs  set F_BZ='" + "未找到文件" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg" + "' where F_BLH='" + blh.Trim() + "'");
                    if (debug == "1")
                        MessageBox.Show("未找到文件" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg");
                    log.WriteMyLog(blh + ",未找到文件" + "C:\\temp\\" + blh + "\\" + blh + "_1.jpg");
                }
                else
                {
                    try
                    {
                        FileStream file = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                        imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                        file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                        file.Close();
                    }
                    catch (Exception ee3)
                    {
                        imgByte = new Byte[1];
                        if (jcxx_fs == "1")
                            aa.ExecuteSQL("update T_jcxx_fs  set F_BZ='图片转数据流异常：" + ee3.Message + "' where F_BLH='" + blh.Trim() + "'");
                        if (debug == "1")
                            MessageBox.Show("图片转数据流异常：" + ee3.Message);
                        log.WriteMyLog("图片转数据流异常：" + ee3.Message);
                    }
                }


                try
                {
                    if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                        System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                }
                catch
                {
                    log.WriteMyLog(blh + ",删除临时目录" + blh + "失败"); return;
                }
                ///////////////////////////////////////////

                string CommandText = "delete from  t_itf_blreport where  zyh='" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "' and  yxh='" + bljc.Rows[0]["F_YXH"].ToString().Trim() + "'";
                    SQL_ExecuteNonQuery(connectionString, CommandText, ref rtnmsg);
            
                //}
                //////////////////////////////////////////bljc.Rows[0]["F_SCBJ"].ToString().Trim()
                string yxh = "病理图" + DateTime.Today.ToString("yyyyMMdd") + blh;
                string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
                if(brlb=="门诊") brlb="1";
                else
                  brlb = "2";
                 
               
                string cmdText = @"insert  into t_itf_blreport(yxh,jqxh,xb,xm,nl,zyh,ch,scks,scys,scsj,jcbw,jcmc,yxsj,yxjl,bgys,bgsj,bgtp,type_int,zycs)"
            + "values('" + yxh + "','病理图','" + bljc.Rows[0]["F_XB"].ToString().Trim() + "','" + bljc.Rows[0]["F_XM"].ToString().Trim() + "','" + bljc.Rows[0]["F_NL"].ToString().Trim()
                + "','" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "','" + bljc.Rows[0]["F_CH"].ToString().Trim() + "','" + bljc.Rows[0]["F_SJKS"].ToString().Trim()
                + "','" + bljc.Rows[0]["F_SJYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "','" + bljc.Rows[0]["F_BBMC"].ToString().Trim()
                + "','" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "','" + bljc.Rows[0]["F_RYSJ"].ToString().Trim() + "','" + bljc.Rows[0]["F_BLZD"].ToString().Trim()
                + "','" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "','" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "',@p,'" + brlb + "','" + bljc.Rows[0]["F_YZID"].ToString().Trim() + "')";
                SqlConnection con = con = new SqlConnection(connectionString);
                SqlCommand sqlcom = null;
                try
                {
                    sqlcom = new SqlCommand();
                    sqlcom.Connection = con;
                    sqlcom.CommandText = cmdText;
                    con.Open();
                    sqlcom.Parameters.Add("@p", SqlDbType.Image, imgByte.Length);
                    sqlcom.Parameters["@p"].Value = imgByte;
                    int qw = sqlcom.ExecuteNonQuery();
                    sqlcom.Dispose();
                    con.Close();

                    aa.ExecuteSQL("update T_JCXX  set F_SCBJ='1',F_YXH='" + yxh + "' where F_BLH='" + blh.Trim() + "'");
                    if (jcxx_fs == "1")
                    aa.ExecuteSQL("update T_jcxx_fs  set F_FSZT='已处理' where F_BLH='" + blh.Trim() + "'");
                  
                }
                catch (Exception ee)
                {
                    con.Close(); sqlcom.Dispose();
                    if (jcxx_fs == "1")
                    aa.ExecuteSQL("update T_jcxx_fs  set F_BZ='执行数据库异常:"+ee.Message+"' where F_BLH='" + blh.Trim() + "'");
                    if(debug=="1")
                        MessageBox.Show("执行数据库异常："+ee.Message);
                    log.WriteMyLog(blh + ",执行数据库异常：" + ee.Message + "\r\n" + cmdText);
                    return;
                }

               


            }
            else
            {
                if (bljc.Rows[0]["F_SCBJ"].ToString().Trim() == "1")
                {
                   string CommandText = "delete from  t_itf_blreport where  zyh='" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "' and  yxh='" + bljc.Rows[0]["F_YXH"].ToString().Trim() + "'";
                   int x = SQL_ExecuteNonQuery(connectionString,CommandText, ref  rtnmsg);
                   if (jcxx_fs == "1")
                   aa.ExecuteSQL("delete from T_jcxx_fs  where F_BLH='" + blh.Trim() + "'");

                }
            }
        }


       public void sendDX(string blh, DataTable bljc, string debug)
       {

           string wsweb = f.ReadString("savetohis", "wsweb", "http://172.16.100.85:9180/hdepc/services/hisWebService?wsdl");

           if (bljc == null)
           {
               if (debug == "1")
                   MessageBox.Show("数据库连接失败");
               log.WriteMyLog("数据库连接失败");
               return;
           }
           if (bljc.Rows.Count <= 0)
           {
               if (debug == "1")
                   MessageBox.Show("数据库连接失败，查询数据为0");
               log.WriteMyLog("数据库连接失败，查询数据为0");
               return;
           }
           if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
           {
               string sjhm = bljc.Rows[0]["F_LXXX"].ToString().Split('^')[0].Trim();
               if (sjhm == "")
               {
                   if (debug == "1")
                       MessageBox.Show("无手机号码不发送");
                   log.WriteMyLog("无手机号码不发送");
                   return;
               }
               if (sjhm.Trim().Length!=11)
               {
                   if (debug == "1")
                       MessageBox.Show("手机号码位数不正确，不发送");
                   log.WriteMyLog("手机号码位数不正确，不发送");
                   return;
               }
               try
               {
                   long.Parse(sjhm);
               }
               catch
               {
                   if (debug == "1")
                       MessageBox.Show("手机号码格式不正确，不发送");
                   log.WriteMyLog("手机号码格式不正确，不发送");
                   return;
               }
              
                   string dataxml = "<request><head>";
                   dataxml = dataxml + "<key>hisRemind_common_pathology</key>";
                   dataxml = dataxml + "<hospcode>262</hospcode>";
                   dataxml = dataxml + "<token></token>";
                   dataxml = dataxml + "<time>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</time>";
                   dataxml = dataxml + "</head>";
                   dataxml = dataxml + "<body>";
                   dataxml = dataxml + "<typeCode>4</typeCode>";
                   dataxml = dataxml + "<checkNo>" + bljc.Rows[0]["F_brbh"].ToString() +"</checkNo>";
                   dataxml = dataxml + "<clinicHospNo>" + bljc.Rows[0]["F_yzid"].ToString() +"</clinicHospNo>";
                   dataxml = dataxml + "<hisPatientId>" + bljc.Rows[0]["F_brbh"].ToString() +"</hisPatientId>";
                   dataxml = dataxml + "<patientName>" + bljc.Rows[0]["F_xm"].ToString()+"</patientName>";
                   dataxml = dataxml + "<checkItemName>" + bljc.Rows[0]["F_yzxm"].ToString() +"</checkItemName>";
                   dataxml = dataxml + "<reportDate>" + bljc.Rows[0]["F_spare5"].ToString() +"</reportDate>";
                   dataxml = dataxml + "<docName>" + bljc.Rows[0]["F_bgys"].ToString()+"</docName>";
                   dataxml = dataxml + "<phone>" + sjhm + "</phone>";
                   dataxml = dataxml + "</body>";
                   dataxml = dataxml + "</request>";



                   string rtndata = "";
                   try
                   {
                       gddhyyweb.WSServiceService dh = new PathHISZGQJK.gddhyyweb.WSServiceService();
                       dh.Url = wsweb;
                      rtndata= dh.requestWS(dataxml);
                   }
                   catch(Exception  ee2)
                   {
                       log.WriteMyLog("webservice连接异常：" + ee2.Message + "\r\nURL:" + wsweb + "\r\n方法：requestWS()  参数：" + dataxml);
                       return;
                   }

                   if (debug == "1")
                       MessageBox.Show("返回值："+rtndata);

                   if (rtndata.Trim() == "")
                   {
                       if (debug == "1")
                           MessageBox.Show("返回值为空，发送失败");
                       log.WriteMyLog("返回值为空，发送失败");
                       return;
                   }
                   XmlNode xmlok_DATA = null;
                   XmlDocument xd2 = new XmlDocument();
                   try
                   {
                       xd2.LoadXml(rtndata);
                       xmlok_DATA = xd2.SelectSingleNode("/response/head");
                   }
                   catch (Exception xmlok_e)
                   {
                       if (debug == "1")
                           MessageBox.Show("解析xml异常，发送失败");
                       log.WriteMyLog("解析xml异常，发送失败\r\n" + rtndata);
                       return;
                   }
                   if (debug == "1")
                       MessageBox.Show(xmlok_DATA["desc"].InnerText);
                
                

               
           }
       }

     

       public int SQL_ExecuteNonQuery(string connectionString, string cmdText,ref string msg)
       {
           SqlConnection con = con = new SqlConnection(connectionString);
           SqlCommand sqlcom = new SqlCommand(cmdText, con);
           int x = -1;
           msg = "";
           try
           {
               con.Open();
               x = sqlcom.ExecuteNonQuery();
               sqlcom.Dispose();
               con.Close();
               return x;
           }
           catch (Exception ee)
           {
               con.Close(); sqlcom.Dispose();
               msg="执行数据库异常：" + ee.Message;
               return -1;
           }
       }
    }
}