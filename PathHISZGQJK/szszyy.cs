using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
    class szszyy  //苏州盛泽医院
    {
        private static IniFiles f = new IniFiles("sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
        {
           
            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
            string url= f.ReadString("savetohis", "url", "");   //从sz里取我们的web服务器的URL，这个要回传的
            string oldjk = f.ReadString("savetohis", "oldjk", "0");

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
            string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
            string sqxh = bljc.Rows[0]["F_sqxh"].ToString().Trim();
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();

            if (zt!="已审核")
              aa.ExecuteSQL("update T_HL7_sqd  set F_DJBZ='1'  where  F_sqxh ='" + sqxh + "' and  F_BRBH='" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "'  and F_brlb='" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "'");
           
            if (oldjk == "0")
                  return;
           // if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") patid = bljc.Rows[0]["F_zyh"].ToString().Trim();
            //if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") patid = bljc.Rows[0]["F_mzh"].ToString().Trim();

            //if (patid == "")
            //{
            //     LGZGQClass.log.WriteMyLog("非住院病人，不处理！");
            //    return;

            //}

            //回传xml
            //DataTable jcxm = aa.GetDataTable("select * from T_whtjyy_jcxm where CheckFlow='" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "'", "jcxm");

        
        
           
            if (brlb == "住院")
            {

                if (zt == "已审核" || zt == "已写报告")
                {
                    try
                    {
                        SZSZYY.Service szyy = new PathHISZGQJK.SZSZYY.Service();
                        szyy.Init();//初始化
                   bool  aa1= szyy.SetFlag(sqxh, 1, 1); //回写取样标记
                   if (msg == "1") MessageBox.Show(aa1.ToString());
                        url = url + sqxh;
                    bool aa2= szyy.SetCompleteWebURL(sqxh, 1, url);// 回写web连接
                     if (msg == "1") MessageBox.Show(aa1.ToString());
                        szyy.Dispose();

                        if (msg == "1") log.WriteMyLog("回传his参数成功，" + sqxh + "," + "1" + url);
                        return;
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("回传his错误，" + sqxh + "," + ee.ToString());
                        return;
                    }
                    
                }
                else 
                {
                    try
                    {
                       SZSZYY.Service szyy = new PathHISZGQJK.SZSZYY.Service();
                        szyy.Init();
                        bool aa1 = szyy.SetFlag(sqxh, 0, 1); ////回写取样标记
                      if (msg == "1") MessageBox.Show(aa1.ToString());
                        szyy.Dispose();
                      if (msg == "1") log.WriteMyLog("回传his参数成功，" + sqxh + "," + "1" );
                        return;
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("回传his错误，" + sqxh + "," + ee.ToString());
                        return;
                    }
                }
            }
            if (brlb == "门诊")
            {
                if (zt == "已审核" || zt == "已写报告")
                {
                    try
                    {
                        SZSZYY.Service szyy = new PathHISZGQJK.SZSZYY.Service();
                        szyy.Init();  //初始化
                        bool aa1 = szyy.SetFlag(sqxh, 1, 0);   //回写取样标记，
                        if (msg == "1") MessageBox.Show(aa1.ToString());
                        url = url + sqxh;
                        bool aa2 = szyy.SetCompleteWebURL(sqxh, 1, url); //回写web连接
                        if (msg == "1") MessageBox.Show(aa1.ToString());
                        szyy.Dispose();
                        if (msg == "1") log.WriteMyLog("回传his参数成功，" + sqxh + "," + "1" + url);
                        return;
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("回传his错误，" + sqxh + "," + ee.ToString());
                        return;
                    }
                }
                else
                {
                    try
                    {
                        SZSZYY.Service szyy = new PathHISZGQJK.SZSZYY.Service();
                        szyy.Init();
                        bool aa1 = szyy.SetFlag(sqxh, 0, 0); //回写取样标记，
                       if (msg == "1") MessageBox.Show(aa1.ToString());
                        szyy.Dispose();
                        if (msg == "1") log.WriteMyLog("回传his参数成功，" + sqxh + "," + "1" );
                        return;
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("回传his错误，" + sqxh + "," + ee.ToString());
                        return;
                    }
                }
            }

          
        }
    }
}
