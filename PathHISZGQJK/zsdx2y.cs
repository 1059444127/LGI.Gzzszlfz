using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using System.Xml;
using System.Net;
using System.IO;
using System.Windows.Forms;
using ZgqClassPub;


namespace PathHISZGQJK
{
    //中山2院 平台接口
    class zsdx2y
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string dz)
        {
            bglx = bglx.ToLower();
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            string ksmc = f.ReadString("savetohis", "ksmc", "病理科（院本部）").Replace("\0", "").Trim();//病理科（南院区）
            string ksid = f.ReadString("savetohis", "ksid", "2344").Replace("\0", "").Trim();//2345
            string ksbh = f.ReadString("savetohis", "ksbh", "08002").Replace("\0", "").Trim();//08003
            debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            string addr = f.ReadString("savetohis", "addr", "192.168.70.252").Replace("\0", "").Trim();
            
            string hczt = f.ReadString("savetohis", "hczt", "1").Replace("\0", "").Trim();
            string GetToKenURL = f.ReadString("savetohis", "GetToKenURL", "http://172.18.41.126:9090/hipService").Replace("\0", "").Trim();
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
    
            if (bljc == null)
            {
               log.WriteMyLog("数据库连接异常");
                return;
            }
            if (bljc.Rows.Count <= 0)
            {
                log.WriteMyLog("未查询到此报告" + blh);
                return;
            }
            string sqxh = bljc.Rows[0]["F_SQXH"].ToString().Trim();
            if (sqxh == "")
            {
                log.WriteMyLog("无申请单号不处理");
                return;
            }
            string brlb = bljc.Rows[0]["F_BRLB"].ToString().Trim();
            string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
            if (dz == "qxsh")
                bgzt = "取消审核";
       
            
            CreatePDF( blh,  bgxh, bglx, bljc, debug,bgzt);

      
            #region 状态回传
            if (bgzt == "取消审核")
            {
                string token = GetToKen(GetToKenURL, "getToken", addr,debug).Trim();

                string REQUESTID = DateTime.Now.ToString("yyyyMMddHHmmss") + "10002";
                string xml = "<COMMONREQUEST>"
                      + "<REQUESTID>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "10001" + "</REQUESTID>"
                      + "<REQUESTIP>" + addr.ToString() + "</REQUESTIP>"
                      + "<ACCESSTOKEN>" + token + "</ACCESSTOKEN>"
                      + "<SERVICEVERSION>1.0</SERVICEVERSION>"
                      + "<SERVICECODE>B001405</SERVICECODE>"
                      + "<REQUESTDATA>"
                      + "<REQUEST>"
                      + "<HIP_OID>" + sqxh + "</HIP_OID>"
                      + "<REPORTID>"+blh+bglx+bgxh+"</REPORTID>"
                      + "</REQUEST>"
                      + "</REQUESTDATA></COMMONREQUEST>";

                try
                {
                    XmlDocument xd = QueryPostWebService(GetToKenURL, "CommonPort", xml);
                    XmlNode xn = xd.SelectSingleNode("/COMMONRESPONSE");
                    if (xn["MSGCODE"].InnerText == "000000")
                    {
                        if(debug=="1")
                            log.WriteMyLog("取消审核成功");
                    }
                    else
                    {
                        log.WriteMyLog("取消审核失败:" + xn["MSGDESC"].InnerText);
                    }
                }
                catch (Exception ee2)
                {
                    log.WriteMyLog("取消审核异常:" + ee2.Message);
                }

                return;
            }

            if( bglx == "cg" && (bgzt == "已登记" || bgzt == "已取材"))
             aa.ExecuteSQL("update T_SQD set F_SQDZT='已登记' where F_sqxh='" + sqxh + "'");

            if (hczt == "1" && bglx == "cg" && (bgzt == "已登记" || bgzt == "已取材" || bgzt == "已审核"))
            {
            
                string zt = "";
                string  shrq=bljc.Rows[0]["F_SPARE5"].ToString().Trim();
          
                if(shrq!="")
                {
        
                    try
                    {
                        shrq = DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    catch
                    {
                        shrq = ""; 
                    }
                }
         
            
                if (bgzt == "已登记" || bgzt == "已取材")
                {
                    zt = "1";
                }
                if (bgzt == "已审核")
                    zt = "2";
                if (zt != "")
                {
                    string token = GetToKen(GetToKenURL, "getToken", addr,debug).Trim();
                  
                    string REQUESTID = DateTime.Now.ToString("yyyyMMddHHmmss") + "10002";
                    string xml = "<COMMONREQUEST>"
                          + "<REQUESTID>" + REQUESTID + "</REQUESTID>"
                          + "<REQUESTIP>" + addr.ToString() + "</REQUESTIP>"
                          + "<ACCESSTOKEN>" + token + "</ACCESSTOKEN>"
                          + "<SERVICEVERSION>1.0</SERVICEVERSION>"
                          + "<SERVICECODE>B001403</SERVICECODE>"
                          + "<REQUESTDATA>"
                          + "<REQUEST>"
                          + "<FUNCTIONREQUESTFORPACSID>" + sqxh + "</FUNCTIONREQUESTFORPACSID>"
                          + "<EXAMINEDEPARTMENTID>"+ksid+"</EXAMINEDEPARTMENTID>"
                          + "<EXAMINEDEPARTMENTNO>"+ksbh+"</EXAMINEDEPARTMENTNO>"
                          + "<EXAMINEDEPARTMENTNAME>"+ksmc+"</EXAMINEDEPARTMENTNAME>"
                          + "<FLAG>" + zt + "</FLAG>"
                          + "<RECEIVEDATE>" +DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "</RECEIVEDATE>"
                          + "<RESULTDATE>" + shrq + "</RESULTDATE>"
                          + "<PACSFLAG>1</PACSFLAG>"

                          + "</REQUEST>"
                          + "</REQUESTDATA></COMMONREQUEST>";
                  
                    try
                    {
                        XmlDocument xd = QueryPostWebService(GetToKenURL, "CommonPort", xml);
                        XmlNode xn = xd.SelectSingleNode("/COMMONRESPONSE");
                        if (xn["MSGCODE"].InnerText == "000000")
                        {
                            if (debug == "1")
                                log.WriteMyLog("回传状态(" + bgzt + "):成功");
                        }
                        else
                        {

                            log.WriteMyLog("回传状态失败(" + bgzt + "):" + xn["MSGDESC"].InnerText);
                        }

                    }
                    catch (Exception ee2)
                    {
                        log.WriteMyLog("回传状态(" + bgzt + ")异常:" + ee2.Message);
                    }
         
                }
            }
     
            #endregion

        }
        public void CreatePDF(string blh, string bgxh, string bglx, DataTable dt_jcxx, string debug,string bgzt)
        {
            string blbh = blh + bglx + bgxh;
               dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            if (bglx == "cg")
                blbh = blh;
            if (bgzt == "取消审核")
            {
                try
                {
                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                }
                catch
                {
                }
                return;
            }
            
            #region  生成pdf
            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
            {
                string ReprotFile = ""; string jpgname = "";
                string ML = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                string message = "";

                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref jpgname, "", ref message);
                if (isrtn)
                {
                    if (debug == "1")
                        log.WriteMyLog("生成PDF成功");

                    ////二进制串
                    if (File.Exists(jpgname))
                    {
                        string pdfpath = "";
                        bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, 3, ref pdfpath);

                        if (ssa == true)
                        {
                            if (debug == "1")
                                log.WriteMyLog("上传PDF成功");

                            jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                            ZgqClass.BGHJ(blh, "上传PDF成功", "审核", "上传PDF成功:" + pdfpath, "ZGQJK", "上传PDF");
                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                        }
                        else
                        {
                            if (debug == "1")
                                log.WriteMyLog("上传PDF失败" + message);
                            ZgqClass.BGHJ(blh, "上传PDF失败", "审核", message, "ZGQJK", "上传PDF");
                        }
                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("未找到文件" + jpgname);
                        ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "未找到文件" + jpgname, "ZGQJK", "生成PDF");
                    }


                }
                else
                {
                    if (message == "报告未审核")
                        return;
                    else
                    {

                        if (debug == "1")
                            log.WriteMyLog("生成PDF失败" + message);
                        ZgqClass.BGHJ(blh, "生成PDF失败", "审核", "生成pdf失败" + message, "ZGQJK", "生成PDF");
                    }
                }
                zgq.DelTempFile(blh);

            }
            #endregion

        }

   
        public static string GetToKen(string URL, string MethodName, string addr,string debug)
        {
            string
            REQUESTID = DateTime.Now.ToString("yyyyMMddHHmmss") + "10001";

           
            string xml = "<GETTOKENREQUEST>"
            + "<REQUESTID>" + REQUESTID + "</REQUESTID>"
            + "<REQUESTIP>" + addr.ToString() + "</REQUESTIP>"
             + "<SYSTEMCODE>BL_SYSTEM</SYSTEMCODE>"
             + "<SYSTEMPASSWORD>0</SYSTEMPASSWORD>"
           + "</GETTOKENREQUEST>";

            if(debug=="1")
                log.WriteMyLog("GetToKen请求:"+xml);
            try
            {
                XmlDocument xd = QueryPostWebService(URL, MethodName, xml);
                XmlNode xn = xd.SelectSingleNode("/GETTOKENRESPONSE");
                return xn["ACCESSTOKEN"].InnerText.Trim();
            }
            catch(Exception  ee)
            {
                log.WriteMyLog("GetToKen异常:"+ee.Message);
                return "";
            }
        }
        public static XmlDocument QueryPostWebService(String URL, String MethodName, string Xml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName);
                request.Method = "POST";
                request.ContentType = "text/plain;charset=utf-8";
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 10000;
                StringBuilder sb = new StringBuilder();
                sb.Append(Xml.ToString());
                byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                request.ContentLength = data.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(data, 0, data.Length);
                writer.Close();
                return ReadXmlResponse(request.GetResponse());
            }
            catch(Exception  ee)
            {
                log.WriteMyLog(ee.Message);
                XmlDocument doc = new XmlDocument();
                return doc;
            }
        }
        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                String retXml = sr.ReadToEnd();
                sr.Close();

                doc.LoadXml(retXml);
            }
            catch (Exception ee)
            {
               log.WriteMyLog("[ReadXmlResponse]" + ee.Message);
            }
            return doc;
        }
    }
}
