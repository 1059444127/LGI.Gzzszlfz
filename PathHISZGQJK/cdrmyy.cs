using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using ZgqClassPub;

namespace PathHISZGQJK
{
    //------常德第一人民医院------------------------
    class cdrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      
        public void pathtohis(string blh, string yymc)
        {
         
          string msg = f.ReadString("savetohis", "msg", "");
            string url = f.ReadString("savetohis", "url", "");
            string bdfk = f.ReadString("bdfk", "bdfk", "");//冰冻是否分库
            string sbmc = f.ReadString("savetohis", "sbmc", "2070000").Replace("\0", "");//设备名称
           
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
         
          
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
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }
        


            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();

            string bdzd = ""; string bd_rq = "";string bd_ys ="";
            ////////////////////////////////////////////////////
            bd_rq = bljc.Rows[0]["F_bgrq"].ToString();
            if(bd_rq=="")
                bd_rq = bljc.Rows[0]["F_sdrq"].ToString();
            if (bdfk == "0")
            {

                //--------冰冻报告-----------
                DataTable bljc_bd = new DataTable();
                bljc_bd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "'", "bdxx");
                if (bljc_bd.Rows.Count > 0)
                {
                    for (int i = 0; i < bljc_bd.Rows.Count; i++)
                    {
                        if (bljc_bd.Rows[i]["F_BD_BGZT"].ToString() == "已审核")
                        {
                            bdzd = bdzd + "快速冰冻诊断" + bljc_bd.Rows[i]["F_BD_BGXH"].ToString() + ":" + bljc_bd.Rows[i]["F_BDZD"].ToString() + "      ";

                            bd_rq = bljc_bd.Rows[0]["F_BD_BGRQ"].ToString();
                            bd_ys = bljc_bd.Rows[0]["F_BD_BGYS"].ToString();
                        }
                    }
                    
                }
            }
              /////////////////////////////////////////////////////////////////////////////////
            string yyx="";
            if(bljc.Rows[0]["F_YYX"].ToString().Trim()=="阳性")
                yyx="1";
               if(bljc.Rows[0]["F_YYX"].ToString().Trim()=="阴性")
                yyx="0";


            string xml = "";
            if (bgzt != "已审核")
            {

               
                if (bgzt == "报告延期")
                    bdzd = bdzd + "\r\n此病人报告还未审核，不能查看诊断结果。此报告状态：" + bljc.Rows[0]["F_BGZT"].ToString().Trim() + "，报告缓发原因：" + bljc.Rows[0]["F_WFBGYY"].ToString().Trim();
                else
                     bdzd =bdzd+"\r\n此病人报告还未审核，不能查看诊断结果。此报告状态：" + bljc.Rows[0]["F_BGZT"].ToString().Trim();
               
                xml = "<funderService functionName='Pacs_InsertexamReport_medex'>"
                          + "<value>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</value>"         //病人ID                    
                          + "<value>" + bljc.Rows[0]["F_YZID"].ToString().Trim() + "</value>"         //次数                    
                          + "<value>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</value>"         //申请单号        
                          + "<value></value>"         //所见 
                         
                          + "<value>" + bdzd + "</value>"         //诊断 
                          + "<value>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</value>"                                                         //影像号
                          + "<value>" + DateTime.Parse(bd_rq).ToString("yyyy-MM-dd HH:mm:ss") + "</value>"         //报告时间                       
                          + "<value></value>"  //操作人编码  
                          + "<value>" + bd_ys + "</value>"         //操作人名字 
                          + "<value>" + "" + "</value>"         //设备名   
                          + "<value>" + "" + "</value>"         //阴性阳性   
                          + "</funderService>";
            }

            if (bgzt == "已审核")
            {
                xml = "<funderService functionName='Pacs_InsertexamReport_medex'>"
                      + "<value>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</value>"         //病人ID                    
                      + "<value>" + bljc.Rows[0]["F_YZID"].ToString().Trim() + "</value>"         //次数                    
                      + "<value>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</value>"         //申请单号        
                      + "<value>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</value>"         //所见 
                      + "<value>" + bljc.Rows[0]["F_blzd"].ToString().Trim() + "</value>"         //诊断 
                      + "<value>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</value>"                                                         //影像号
                      + "<value>" +DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "</value>"         //报告时间                       
                      + "<value>" + getymh(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</value>"  //操作人编码  
                      + "<value>" + bljc.Rows[0]["F_shys"].ToString().Trim() + "</value>"         //操作人名字    
                      + "<value>" + sbmc + "</value>"         //设备名   
                      + "<value>" +yyx + "</value>"         //阴性阳性   
                      + "</funderService>";
            }
           
                try
                {
                    if (msg == "1")
                        MessageBox.Show(xml.ToString());

                    cdyyWeb.ICalculateService cd = new cdyyWeb.ICalculateService();

                    string mess = cd.funInterFace(xml);
                    if (msg == "1")
                        MessageBox.Show(mess.ToString());
                    if (mess.Trim() == "")
                    {

                        if (msg == "1")
                            MessageBox.Show(mess.ToString());
                        log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "回写失败：" + mess);
                           return;
                    }
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                  
                    try
                    {
                        xd.LoadXml(mess);
                        xmlok = xd.SelectSingleNode("/root");
                        string messae = xmlok["result"].InnerText.ToString();
                        if (messae != "1")
                        {
                            if (msg == "1")
                                MessageBox.Show(messae.ToString());
                            log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString().Trim() + "回写失败：" + messae);
                        }
                    }
                    catch (Exception eee)
                    {
                        if (msg == "1")
                            log.WriteMyLog(eee.ToString());

                        MessageBox.Show("解析xml文件错误：" + mess);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.ToString());
                    log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString() + ",回写his错误" + ee.ToString());
                }
                return;
        }

        //通过医生名称 获取医生编码
        public string getymh(string yhmc)
        {
            if (yhmc != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select F_yhbh from T_yh where F_yhmc='" + yhmc + "'", "blxx");
                    return bljc.Rows[0]["F_yhbh"].ToString().Trim();
                }
                catch (Exception ee)
                {
                    log.WriteMyLog("转换医生工号出错！原因：" + ee.ToString());
                    return "0";
                }
            } return "0";

        }
    }
}


