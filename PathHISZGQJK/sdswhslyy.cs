using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.Xml;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class sdswhslyy
    {

        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug)
        {

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");
            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
               string wsurl = f.ReadString("savetohis", "wsurl", "http://10.101.2.170:9008/MZWebService.asmx").Replace("\0", "").Trim();

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
            if (bljc.Rows[0]["F_SQXH"].ToString().Trim()=="")
            {
              
                 log.WriteMyLog("无申请序号不处理");
                return;
            }
            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
            string ExecutePostion = "0";
            if (bgzt == "已审核")
            {
                ExecutePostion = "4";
            }
            if (bgzt == "已写报告")
                {
                    ExecutePostion = "3";
                }
                if (bgzt == "已登记" || bgzt == "已取材")
                {
                    ExecutePostion = "2";
                }

                if (ExecutePostion == "0")
                    return;

                string xml = "<Request><HisCheckRequestID>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</HisCheckRequestID>"
                + "<ExecutePostion>" + ExecutePostion + "</ExecutePostion>"
                + "<Executer>" + yhbh + "</Executer></Request>";


            string  rtn_xml="";
            try{
            sdwhslyyWeb.MZWebService sdwh = new PathHISZGQJK.sdwhslyyWeb.MZWebService();
            if(wsurl.Trim()!="")
                sdwh.Url=wsurl;

            rtn_xml= sdwh.AlterCheckExecution(xml);
            }
            catch(Exception  e1)
            {
                 log.WriteMyLog("连接webservice服务异常:"+e1.Message);
                return;
            }

            if(rtn_xml.Trim()=="")
            {
                 log.WriteMyLog("webservice服务返回为空");
                return;
            }

              if (debug == "1")
               log.WriteMyLog(debug);

          try
          {
              XmlNode xmlok = null;
              XmlDocument xd = new XmlDocument();
              try
              {
                  xd.LoadXml(rtn_xml);
                  xmlok = xd.SelectSingleNode("/Response");
              }
              catch
              {
                  if (debug == "1")
                      MessageBox.Show("XML解析错误");
                  return ;
              }
              if (debug == "1")
                  log.WriteMyLog(xmlok["resultMessage"].InnerText);
          }
          catch
          {
          }
           return;
        }
        public string getyhgh(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
