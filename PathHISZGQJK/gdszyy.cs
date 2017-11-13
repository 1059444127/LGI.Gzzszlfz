        using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Xml;
using PathHISZGQJK;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class gdszyy
    {

        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                {
                    //取消审核动作
                    qxsh = "1";
                }

                if (cslb[3].ToLower() == "new")
                {
                    xdj = "1";
                }

            }
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";

              string JsonSvcWeb = f.ReadString("savetohis", "JsonSvcWeb", "http://192.9.200.56:9080/ChasSvc/services/JsonSvc4Ris").Replace("\0", "").Trim();
              string yydm = f.ReadString("savetohis", "yydm", "1").Replace("\0", "").Trim();
              string MZHISWeb = f.ReadString("savetohis", "WSURL", "http://192.9.199.12:8002/HisForInspectItemService.asmx").Replace("\0", "").Trim(); //获取sz.ini中设置的mrks
              string tomzhis = f.ReadString("savetohis", "tomzhis", "1").Replace("\0", "").Trim();
              string tozyhis = f.ReadString("savetohis", "tozyhis", "1").Replace("\0", "").Trim(); 

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }
           
            if (bglx.Trim() == "")
            {
                log.WriteMyLog("报告类型为空，不处理！" + blh + "^" + bglx + "^" + bgxh);
                return;
            }

            string bgzt = "";

            if (qxsh == "1")
                bgzt = "取消审核";
            else
                bgzt=jcxx.Rows[0]["F_BGZT"].ToString();

            if(tozyhis=="1")
            {
            #region   回写住院HIS状态
            if (jcxx.Rows[0]["F_brlb"].ToString() == "住院")
            {
                if (bglx == "cg" || jcxx.Rows[0]["F_SQXH"].ToString() !="" )
                {

                    string yzxmid="";
                    try
                    {
                        yzxmid = jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1];
                    }
                    catch
                    {
                    }
                    if (yzxmid == "")
                        return;

                    string actionCode = "register";
                    string actionName = "PACS登记";
                    string sqdzt = ""; string sqdztCode = "";
                    if (bgzt == "已登记") { sqdztCode = "CLDS.RISREGISTER.0000"; sqdzt = "登记"; }
                    else if (bgzt == "已审核") { sqdztCode = "CLDS.RISAUDIT.0000"; sqdzt = "审核报告"; actionCode = "updateStatus"; actionName = "更新状态"; }
                    else if (bgzt == "取消审核") { sqdztCode = "CLDS.RISFINISH.0000"; sqdzt = "已检查"; actionCode = "updateStatus"; actionName = "更新状态"; }
                    else if (bgzt == "报告延期") { sqdztCode = "CLDS.RISFINISH.0000"; sqdzt = "已检查"; actionCode = "updateStatus"; actionName = "更新状态"; }
                    else if (bgzt == "已作废") { sqdztCode = "CLDS.RIUNR.0000"; sqdzt = "取消登记"; actionCode = "cancel"; actionName = "退单"; }
                    else
                        return;
                

                        MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("gb2312"));
                        JsonWriter writer = new JsonTextWriter(sw);
                        Encoding eee = sw.Encoding;
                        writer.WriteStartObject();
                        writer.WritePropertyName("reqHeader");
                        writer.WriteStartObject();
                        writer.WritePropertyName("callFunction");
                        writer.WriteValue("updateExaminationApply");
                        writer.WritePropertyName("systemId");
                        writer.WriteValue("PACS.BL");
                        writer.WritePropertyName("reqTimestamp");
                        writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WriteEndObject();
                //申请单  33333333333333333333
                        writer.WritePropertyName("requestForms");
                        writer.WriteStartArray();
                        writer.WriteStartObject();
                                  //申请单号
                                    writer.WritePropertyName("requestFormHisNo");
                                    writer.WriteValue(jcxx.Rows[0]["F_SQXH"].ToString());
                                   //申请单名称
                                    writer.WritePropertyName("requestName");
                                    writer.WriteValue("");
                                   //检查号
                                    writer.WritePropertyName("requestFormSysNo");
                                    writer.WriteValue(blh);
                                     //检查分类
                                    writer.WritePropertyName("requestType");
                                    writer.WriteValue("");
                                                                    //检查项目列表2222222222222
                                            writer.WritePropertyName("orderItem");
                                            writer.WriteStartArray();
                                            string[] orderItemHisNos = yzxmid.ToString().Split('|');
                                            foreach (string orderItemHisNo in orderItemHisNos)
                                            {
                                                if (orderItemHisNo.Trim() != "")
                                                {
                                                    writer.WriteStartObject();
                                                    //子申请单号
                                                    writer.WritePropertyName("orderItemHisNo");
                                                    writer.WriteValue(orderItemHisNo);
                                                    //检查项目编码
                                                    writer.WritePropertyName("orderCode");
                                                    writer.WriteValue("");
                                                    //检查项目名称
                                                    writer.WritePropertyName("orderName");
                                                    writer.WriteValue("");
                                                    //检查项目状态
                                                    writer.WritePropertyName("statusCode");
                                                    writer.WriteValue(sqdztCode);
                                                    //检查项目状态名称
                                                    writer.WritePropertyName("statusName");
                                                    writer.WriteValue(sqdzt);

                                                    ////检查项目部位
                                                    //writer.WritePropertyName("subItems");
                                                    //writer.WriteStartArray();
                                                    //writer.WriteStartObject();
                                                    //        //检查项目部位编码
                                                    //        writer.WritePropertyName("orderCode");
                                                    //        writer.WriteValue("");
                                                    //        //检查项目部位名称
                                                    //        writer.WritePropertyName("orderName");
                                                    //        writer.WriteValue("");
                                                    //writer.WriteEndObject();
                                                    //writer.WriteEndArray();

                                                    writer.WriteEndObject();
                                                }
                                            }
                                             writer.WriteEndArray();
                                               ////22222222222222222222222222222222       
                            
                                       //操作111111111111111111
                                    writer.WritePropertyName("action");
                                    writer.WriteStartArray();
                                    writer.WriteStartObject();
                                             //操作代码
                                            writer.WritePropertyName("actionCode");
                                            writer.WriteValue(actionCode);
                                             //操作名称
                                            writer.WritePropertyName("actionName");
                                            writer.WriteValue(actionName);
                                            //操作时间
                                            writer.WritePropertyName("actionTime");
                                            writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                              
                                            //操作备注
                                            writer.WritePropertyName("memo");
                                            writer.WriteValue(jcxx.Rows[0]["F_WFBGYY"].ToString());
                                           
                                           //操作人   
                                            writer.WritePropertyName("actor");
                                            writer.WriteStartObject();
                                                     ///////////////////////////////////
                                                    //操作人医院
                                                    writer.WritePropertyName("hospital");
                                                    writer.WriteStartObject();
                                                    //操作人医院
                                                    writer.WritePropertyName("hospitalCode");
                                                    if (jcxx.Rows[0]["F_sjdw"].ToString() == "大院" || jcxx.Rows[0]["F_sjdw"].ToString() == "总院")
                                                        writer.WriteValue("1");
                                                    else if (jcxx.Rows[0]["F_sjdw"].ToString() == "大学城医院" || jcxx.Rows[0]["F_sjdw"].ToString() == "大学城")
                                                        writer.WriteValue("2");
                                                    else if (jcxx.Rows[0]["F_sjdw"].ToString() == "二沙分院" || jcxx.Rows[0]["F_sjdw"].ToString() == "二沙")
                                                        writer.WriteValue("3");
                                                    else if (jcxx.Rows[0]["F_sjdw"].ToString() == "芳村分院" || jcxx.Rows[0]["F_sjdw"].ToString() == "芳村")
                                                        writer.WriteValue("4");
                                                    else
                                                        writer.WriteValue("1");

                                                    //操作人医院
                                                    writer.WritePropertyName("hospitalName");
                                                    writer.WriteValue(jcxx.Rows[0]["F_sjdw"].ToString());
                                                    writer.WriteEndObject();
                                                    ////////////////////////////////////
                                                    //操作人科室
                                                    writer.WritePropertyName("department");
                                                    writer.WriteStartObject();
                                                    // 科室名称
                                                    writer.WritePropertyName("departmentName");
                                                    writer.WriteValue("");
                                                    // 科室代码
                                                    writer.WritePropertyName("departmentCode");
                                                    writer.WriteValue("");
                                                    writer.WriteEndObject();
                                                   ////////////////////////
                                                     //操作员工信息
                                                    writer.WritePropertyName("employee");
                                                    writer.WriteStartObject();
                                                    // 人员名称
                                                    writer.WritePropertyName("employeeName");
                                                    writer.WriteValue("");
                                                    // 人员代码
                                                    writer.WritePropertyName("employeeCode");
                                                    writer.WriteValue("");
                                                    writer.WriteEndObject();
                                                  ////////////////////////
                                            writer.WriteEndObject();

                               
                                      writer.WriteEndObject();
                                      writer.WriteEndArray();
                                     /////11111111111111111111111111111111
                        writer.WriteEndObject();
                        writer.WriteEndArray();
                        writer.WriteEndObject();
                    /////3333333333333333333333333
;
                        writer.Flush();

                  
                      string rtnrequest = GetHisSQD(JsonSvcWeb, ms.ToArray(),debug);

                      if (debug == "1")
                          log.WriteMyLog("出参：" + rtnrequest);

                      JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);
                      string respCode = ja["respHeader"]["respCode"].ToString();
                     string respMessage = ja["respHeader"]["respMessage"].ToString();

                     if (respCode != "000000")
                     {
                          log.WriteMyLog("执行失败：" + respCode + "^" + respMessage);
                     
                     }
                     else
                     {
                         if(debug=="1")
                            log.WriteMyLog("执行成功：" + respCode + "^" + respMessage);
                      
                     }

                }
            }
            #endregion
            }
            if (tomzhis == "1")
            {
                #region  门诊状态执行
                if (jcxx.Rows[0]["F_brlb"].ToString() == "门诊" || jcxx.Rows[0]["F_brlb"].ToString() == "体检")
                {
                    if (bglx == "cg" && jcxx.Rows[0]["F_SQXH"].ToString().Trim() != "")
                    {
                        if (jcxx.Rows[0]["F_HXBJ"].ToString().Trim() != "1" || bgzt=="已审核")
                        {
                            string deptid = f.ReadString("savetohis", "deptid", "60027").Replace("\0", "").Trim();
                            string yhgh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();

                            if (yhgh.Trim() == "")
                                yhgh = "0502";
                            string Flag = "1";
                            if (bgzt == "已审核") Flag = "2";

                            gdszyyMzHisWeb.HisForInspectItemService mzhis = new gdszyyMzHisWeb.HisForInspectItemService();
                               
                            try
                            {
                                mzhis.Url = MZHISWeb;
                            }
                            catch(Exception  ee2)
                            {
                                log.WriteMyLog("门诊HIS web服务连接异常："+ee2.Message);
                                return;
                            }

                            string [] sqdh=jcxx.Rows[0]["F_SQXH"].ToString().Split(new char[2]{'|','|'});
                            for (int x = 0; x < sqdh.Length;x++ )
                            {
                                if (sqdh[x].Trim() == "")
                                    continue;

                                string request_xml = "<Request><CheckNumber>" + sqdh[x].Trim() + "</CheckNumber><ExecDeptId>" + deptid + "</ExecDeptId><ExecUserNo>" + yhgh + "</ExecUserNo><Flag>" + Flag + "</Flag></Request>";
                                if (debug == "1")
                                    log.WriteMyLog("门诊项目执行条件：" + request_xml);
                                string rtn_xml = "";
                                try
                                {
                                    rtn_xml = mzhis.ExecuteCheckItem(request_xml);
                                    if (rtn_xml.Trim() == "")
                                    {
                                        //  MessageBox.Show("门诊项目执行失败：HIS返回空");
                                        log.WriteMyLog("门诊项目执行失败！返回消息为空！");
                                    }
                                    else
                                    {
                                        if (debug == "1")
                                            log.WriteMyLog(rtn_xml);

                                        XmlNode xmlok_DATA = null;
                                        XmlDocument xd2 = new XmlDocument();
                                        try
                                        {
                                            xd2.LoadXml(rtn_xml);
                                            xmlok_DATA = xd2.SelectSingleNode("/Response");
                                            string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
                                            string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

                                            if (ResultCode != "0")
                                            {
                                                log.WriteMyLog("门诊取项目执行失败：" + ErrorMsg);
                                                //  MessageBox.Show("门诊项目执行失败：" + ErrorMsg);
                                            }
                                            else
                                            {
                                                aa.ExecuteSQL("update  T_JCXX  set F_HXBJ='1'  where F_BLH='" + blh + "'");
                                                if (debug == "1")
                                                    log.WriteMyLog("门诊项目执行成功：" + ErrorMsg);
                                            }
                                        }
                                        catch (Exception xmlok_e)
                                        {
                                            // MessageBox.Show("门诊项目执行失败：解析DATA异常");
                                            log.WriteMyLog("解析DATA异常：" + xmlok_e.Message + "\r\n" + rtn_xml);
                                        }
                                    }
                                }
                                catch (Exception ee)
                                {
                                    //     MessageBox.Show("门诊项目执行失败：" + ee.Message);
                                    log.WriteMyLog("门诊项目执行失败：连接WebService服务异常：" + ee.Message + "\r\n");
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            if (bgzt == "已审核" || bgzt == "取消审核")
                {


                string bgzt2 = "";
                DataTable dt_bd = new DataTable();
                DataTable dt_bc = new DataTable();
                try
                {
                    if (bglx.ToLower().Trim() == "bd")
                    {
                        dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                        bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                    }
           
                    if (bglx.ToLower().Trim() == "bc")
                    {
                         dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                        bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                    }
                    if (bglx.ToLower().Trim() == "cg")
                    {
                        // DataTable jcxx2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                        bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                    }
                }
                catch
                {
                }

                if (bgzt2.Trim() == "")
                    log.WriteMyLog("报告状态为空！不处理！" + blh + "^" + bglx + "^" + bgxh);

                if (bgzt2.Trim() == "已审核" && bgzt != "取消审核")
                {
                    //////////////////////生成pdf**********************************************************
                    string jpgname = "";  
                    string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    if (f.ReadString("savetohis", "ispdf", "1").Replace("\0", "").Trim() == "1")
                    {
                        #region  生成pdf

                        string message = "";
                        ZgqPDFJPG zgq = new ZgqPDFJPG();
                        if (debug == "1")
                            log.WriteMyLog("开始生成PDF。。。");
                        bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref  
                            message, ref jpgname);

                        string xy = "3";
                        if (isrtn)
                        {
                            if (File.Exists(jpgname))
                            {
                                bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                                if (ssa == true)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("上传PDF成功");

                                    jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                    ZgqClass.BGHJ(blh, "JK", "审核", "上传PDF成功:" + ML + "\\" + blh + "\\" + jpgname, "ZGQJK", "上传PDF");

                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_bz='上传PDF成功',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

                                }
                                else
                                {
                                    log.WriteMyLog("上传PDF失败：" + message);
                                    ZgqClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "上传PDF");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                                }
                            }
                            else
                            {
                                log.WriteMyLog("生成PDF失败:未找到文件---" + jpgname);
                                ZgqClass.BGHJ(blh, "JK", "审核", "上传PDF失败:未找到文件---" + jpgname, "ZGQJK", "生成PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='上传PDF失败:未找到文件---" + jpgname + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            }
                        }
                        else
                        {

                            log.WriteMyLog("生成PDF失败：" + message);
                            ZgqClass.BGHJ(blh, "JK", "审核", message, "ZGQJK", "生成PDF");
                            aa.ExecuteSQL("update T_JCXX_FS set F_ISPDF='false',F_bz='生成PDF失败：" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

                        }
                        zgq.DelTempFile(blh);
                        #endregion
                   }
                  
                    //////////////////////*****************************************************************
                }
                else
                {
                    if (bgzt == "取消审核")
                    {
                       
                        DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                        if (dt2.Rows.Count <= 0)
                            aa.ExecuteSQL("update T_jcxx_fs set  F_fszt='已处理',F_bz='取消审核成功！' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='未处理' and F_bgzt='" + bgzt + "'");
                        else
                        {
                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                             aa.ExecuteSQL("update T_JCXX_FS set F_bz='取消审核,删除PDF成功！',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "' and F_bgzt='取消审核'");
                        }

                        #region  报告回收
                        #endregion
                    }
                    else
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='不处理,状态" + bgzt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='未处理' and F_bgzt='取消审核'");
                }

            }
        }

        public static string GetHisSQD(string JsonSvcWeb, byte[] reqbyte, string debug)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                string reqtext = Encoding.GetEncoding("gb2312").GetString(reqbyte);

                if (debug == "1")
                    log.WriteMyLog("入参：" + reqtext);

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(JsonSvcWeb);
                if (webrequest == null)
                {
                  log.WriteMyLog("连接住院HIS服务失败：\r\n" + JsonSvcWeb);
                    return "0";
                }
                webrequest.KeepAlive = true;
               // webrequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                webrequest.Method = "POST";
                webrequest.Timeout = 10000;
                webrequest.ContentType = "Content-Type";


           
                string Content = "";
                //发送POST数据  
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(reqtext);
                    using (Stream stream = webrequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    string[] values = webrequest.Headers.GetValues("Content-Type");
                    WebResponse myResponse = webrequest.GetResponse();
                    using (Stream resStream = myResponse.GetResponseStream())//得到回写的流
                    {
                        StreamReader newReader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                        Content = newReader.ReadToEnd();
                        newReader.Close();
                    }
                }
                catch (Exception ee3)
                {
                    log.WriteMyLog("更新住院HIS数据异常：" + ee3.Message);
                    return "0";
                }

                return Content;


            }
            catch (Exception ee2)
            {
                log.WriteMyLog("更新住院HIS数据异常2：" + ee2.Message);
                return "0";
            }


        }

    }
}
