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
using System.Data.OleDb;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class xmzyjcpt  //厦门置业集成平台-新疆克拉玛依人民医院/克拉玛依中心医院
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static string blhgy = "";
        public void pathtohis(string blh, string yymc)
          {
              //安全校验凭证，凭证既是接口的检验码，也是调用方的身份标识，由集成平台提供给LIS、PACS.
              string certificate = "7pzOrESsiv8VnB6RD2FXmndLaJCYpiY7"; 

            blhgy = blh;
            string msg = f.ReadString("savetohis", "msg", "");
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

            string visitType = "";   ////	--就诊类型（1门急诊2住院 3体检）
            string visitNo = "";      //门诊号或住院号
              string brlb= bljc.Rows[0]["F_brlb"].ToString().Trim();
              if (brlb == "门诊")
              {
                  visitType = "1";
                  visitNo = bljc.Rows[0]["F_MZH"].ToString().Trim();
              }
              if (brlb == "住院")
              {
                  visitType = "2";
                  visitNo = bljc.Rows[0]["F_zyh"].ToString().Trim();
              }
              if (brlb == "体检")
                  visitType = "3";

            //string sqxh=bljc.Rows[0]["F_sqxh"].ToString().Trim();
            string zt = bljc.Rows[0]["F_bgzt"].ToString().Trim();
            //string pacsBillNo = bljc.Rows[0]["F_blh"].ToString().Trim();
         
            string ztbm = "3";   //状态编号
            int  reAuditFlag=0;//是否重复审核
            if(zt=="已登记")
            {  ztbm="3";
             }
             if (zt == "已取材")
             {
                 ztbm = "5";
             }

            if(zt=="已写报告")
            {  ztbm = "6";
            }

            int F_SFCFSH = 0;
         
            try
            {
               
                F_SFCFSH = int.Parse(bljc.Rows[0]["F_SFCFSH"].ToString().Trim());
              
            }
            catch
            {
            }
           
            if (zt == "已审核")
            { 
            ztbm = "7"; 
              
              if (F_SFCFSH >= 1)
                reAuditFlag = 1;
            }

            string yhmc= f.ReadString("yh","yhmc","").Replace("\0", "");;
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "");
          //  // 更改报告状态
          //if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
          // sqxh = " ";

            string xmlstr = "<?xml version='1.0' encoding='UTF-8'?>";
            try
            {
                xmlstr = xmlstr + "<root>";
                xmlstr = xmlstr + "<visitType>" + visitType + "</visitType>";//	--就诊类型（1门急诊2住院 3体检）
                xmlstr = xmlstr + "<visitNo>" + bljc.Rows[0]["F_by2"].ToString().Trim() + "</visitNo>";//--门诊挂号号/住院号/体检号/条码号
                xmlstr = xmlstr + "<patientId>" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</patientId>";// -- 病人ID 
                xmlstr = xmlstr + "<pacsBillNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</pacsBillNo>";//--pacs报告单号
                xmlstr = xmlstr + "<hisApplyNo>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</hisApplyNo>";//--his单据号
                xmlstr = xmlstr + "<clinicCode></clinicCode>";//HIS收费项目代码
                xmlstr = xmlstr + "<clinicName></clinicName>";// 项目名称
                xmlstr = xmlstr + "<reportStatus>" + ztbm + "</reportStatus>";// -- 报告状态编码
                xmlstr = xmlstr + "<reAuditFlag>" + reAuditFlag + "</reAuditFlag>";//-重复审核标志：1-标识重复审核
                xmlstr = xmlstr + "<changeTime>" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "</changeTime>";// -- 改变时间
                xmlstr = xmlstr + "<changeOperator>" + yhmc + "/" + yhbh + "</changeOperator>";  // --修改操作人：姓名/工号
                xmlstr = xmlstr + "</root>";
            }
            catch
            {
                if (msg == "1")
                    MessageBox.Show("生成xml异常！");
                log.WriteMyLog("生成xml异常");
                return;
            }
         
              try
                {
                    klmyWeb.WSInterface klmy = new PathHISZGQJK.klmyWeb.WSInterface();
                    string webServicesURL = f.ReadString("savetohis", "webServicesURL", ""); 
                    if (webServicesURL.Trim()!="")
                        klmy.Url = webServicesURL;
                  
                    string ztstr = klmy.CallInterface("UpdatePacsReportStatus",xmlstr, "", certificate);
                  
               if (ztstr.Contains("error"))
                    {
                        if (msg == "1")
                         MessageBox.Show("回传报告状态失败！原因：" + ztstr);
                         log.WriteMyLog("回传报告状态失败！原因：" + ztstr);
                    }
                    else
                    {
                       if (msg == "1")
                      MessageBox.Show("回传报告状态成功！");
                     }
                  
               
               }
                 catch (Exception e)
                {
                    if (msg == "1")
                        MessageBox.Show("回传报告状态异常！");
                    log.WriteMyLog("回传报告状态异常！原因：" + e.ToString());
                    return;
                }

                if (zt == "已审核")
                {
                    try
                    {
                        aa.ExecuteSQL("update T_JCXX set F_SFCFSH='" + (F_SFCFSH + 1).ToString() + "' where F_BLH='" + blh + "'");
                    }
                    catch
                    { return;
                    } 
                } return;
              }

    }
             
    }

