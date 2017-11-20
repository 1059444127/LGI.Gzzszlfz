using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using dbbase;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Net;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 广东省中医院    Servlet /Json
    /// </summary>
    class gdszyy
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            string exp = "";
            string JsonSvcWeb = f.ReadString(Sslbx, "JsonSvcWeb", "http://192.9.200.56:9080/ChasSvc/services/JsonSvc4Ris").Replace("\0", "").Trim(); ; //获取sz.ini中设置的webservicesurl

            string mrks = f.ReadString(Sslbx, "mrks", "").Replace("\0", "").Trim(); ; //获取sz.ini中设置的mrks
            debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim(); ; //获取sz.ini中设置的mrks
            string MZHISWeb = f.ReadString(Sslbx, "WSURL", "").Replace("\0", "").Trim(); ; 

            if (Sslbx != "")
            {
                string Content = "";

                if (Sslbx == "标本号")
                {
                      dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string BBLB_XML = "";
            DataTable dt_bb = new DataTable();
        
                dt_bb = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_BBTMH= '" + Ssbz + "' ", "bbxx");
            
                if (dt_bb == null)
                {
                    MessageBox.Show("数据库连接异常");
                    return "0";
                }
              
                    if (dt_bb.Rows.Count <= 0)
                    {
                        MessageBox.Show("未查询到此标本信息");
                        return "0";
                    }

                    string sqdh = dt_bb.Rows[0]["F_SQXH"].ToString();
                    string brlb = dt_bb.Rows[0]["F_BRLB"].ToString();

                    if (brlb == "住院")
                    {
                        Sslbx = "住院申请号";
                        Ssbz = sqdh;
                    }
                    else
                    {
                        Sslbx = "门诊申请号";
                        Ssbz = sqdh;
                    }

                }

                if (Sslbx == "住院申请号" || Sslbx == "住院号(大院)" || Sslbx == "住院号(二沙)" || Sslbx == "住院号(芳村)" || Sslbx == "住院号(大学城)" || Sslbx == "住院号")
                {
                    string yydm = f.ReadString(Sslbx, "yydm", "1").Replace("\0", "").Trim(); ; //获取sz.ini中设置的webservicesurl
                    #region  获取住院申请单信息

                    IPAddress addr = new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
                    string Ipaddr = addr.ToString();

                    DataTable dt_sqd = new DataTable();

                    string zyh = ""; string sqdh = "";
       
                    if (Sslbx == "住院申请号")
                        sqdh = Ssbz.Trim();
                    else
                        zyh = Ssbz.Trim();
                  
                    try
                    {
                      
                        MemoryStream ms = new MemoryStream();
                        StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("gb2312"));
                        JsonWriter writer = new JsonTextWriter(sw);
                        Encoding eee = sw.Encoding;
                        writer.WriteStartObject();
                        writer.WritePropertyName("reqHeader");
                        writer.WriteStartObject();
                        writer.WritePropertyName("callFunction");
                        writer.WriteValue("searchPatientExaminationApply");
                        writer.WritePropertyName("systemId");
                        writer.WriteValue("PACS.BL");
                        writer.WritePropertyName("reqTimestamp");
                        writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        writer.WritePropertyName("terminalIp");
                        writer.WriteValue(Ipaddr);
                        writer.WriteEndObject();
                        writer.WritePropertyName("requestForm");
                        writer.WriteStartObject();
                        writer.WritePropertyName("requestFormHisNo");
                        writer.WriteValue(sqdh);
                        writer.WriteEndObject();
                        writer.WritePropertyName("patient");
                        writer.WriteStartObject();
                        writer.WritePropertyName("hospital");
                        writer.WriteStartObject();
                        writer.WritePropertyName("hospitalCode");
                        writer.WriteValue(yydm);
                        writer.WriteEndObject();
                        writer.WritePropertyName("patientNo");
                        writer.WriteValue(zyh);
                        writer.WriteEndObject();

                        writer.WriteEndObject();
                        writer.Flush();


                        string rtnrequest = GetHisSQD(JsonSvcWeb, ms.ToArray(),debug);

                        if (rtnrequest == "0")
                        {
                       log.WriteMyLog("提取住院申请单信息失败：HIS返回消息为空");
                            return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                           
                        }
                        try
                        {
                            JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);
                            string requestForms = ja["requestForms"].ToString().Trim();

                            if (requestForms.Trim() == "")
                            {
                                log.WriteMyLog("提取住院申请单信息失败：未查询到相关数据!");
                                return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                               
                            }
                            //姓名
                            DataColumn name = new DataColumn("name");
                            dt_sqd.Columns.Add(name);
                            //性别
                            DataColumn sexName = new DataColumn("sexName");
                            dt_sqd.Columns.Add(sexName);
                            //年龄
                            DataColumn ageYear = new DataColumn("ageYear");
                            dt_sqd.Columns.Add(ageYear);
                            //birthday
                            DataColumn birthday = new DataColumn("birthday");
                            dt_sqd.Columns.Add(birthday);
                            //idCardNo
                            DataColumn idCardNo = new DataColumn("idCardNo");
                            dt_sqd.Columns.Add(idCardNo);
                            //phone
                            DataColumn phone = new DataColumn("phone");
                            dt_sqd.Columns.Add(phone);
                            //address
                            DataColumn address = new DataColumn("address");
                            dt_sqd.Columns.Add(address);
                            //住院病案号
                            DataColumn patientNo = new DataColumn("patientNo");
                            dt_sqd.Columns.Add(patientNo);

                            //empiid
                            DataColumn empiid = new DataColumn("empiid");
                            dt_sqd.Columns.Add(empiid);
                            //病人就诊号
                            DataColumn encounterNo = new DataColumn("encounterNo");
                            dt_sqd.Columns.Add(encounterNo);
                            //就诊医院
                            DataColumn hospitalName = new DataColumn("hospitalName");
                            dt_sqd.Columns.Add(hospitalName);
                            //就诊科室
                            DataColumn departmentName = new DataColumn("departmentName");
                            dt_sqd.Columns.Add(departmentName);
                            //病区
                            DataColumn wardName = new DataColumn("wardName");
                            dt_sqd.Columns.Add(wardName);
                            //床号
                            DataColumn bedName = new DataColumn("bedName");
                            dt_sqd.Columns.Add(bedName);
                            //送检医生
                            DataColumn employeeName = new DataColumn("employeeName");
                            dt_sqd.Columns.Add(employeeName);
                            //就诊类型
                            DataColumn encounterType = new DataColumn("encounterType");
                            dt_sqd.Columns.Add(encounterType);
                            //申请序号
                            DataColumn requestFormHisNo = new DataColumn("requestFormHisNo");
                            dt_sqd.Columns.Add(requestFormHisNo);
                            //申请单状态
                            DataColumn statusName = new DataColumn("statusName");
                            dt_sqd.Columns.Add(statusName);
                            //医嘱项目
                            DataColumn requestType = new DataColumn("requestType");
                            dt_sqd.Columns.Add(requestType);
                            //备注
                            DataColumn memo = new DataColumn("memo");
                            dt_sqd.Columns.Add(memo);
                            //检查部位
                            DataColumn orderName = new DataColumn("orderName");
                            dt_sqd.Columns.Add(orderName);

                            //临床病史
                            DataColumn clinicalHistory = new DataColumn("clinicalHistory");
                            dt_sqd.Columns.Add(clinicalHistory);

                            //临床诊断
                            DataColumn diagnosis = new DataColumn("diagnosis");
                            dt_sqd.Columns.Add(diagnosis);

                            //子申请单
                            DataColumn orderItemHisNo = new DataColumn("orderItemHisNo");
                            dt_sqd.Columns.Add(orderItemHisNo);


                            JArray jsonVals = JArray.Parse(requestForms);
                            for (int x = 0; x < jsonVals.Count; x++)
                            {
                                DataRow dr = dt_sqd.NewRow();
                                dt_sqd.Rows.Add(dr);
                                //姓名
                                dr["name"] = jsonVals[x]["patient"]["name"].ToString();
                                try
                                {
                                    //性别
                                    dr["sexName"] = jsonVals[x]["patient"]["sexName"].ToString();
                                }
                                catch
                                {
                                    dr["sexName"] = "";
                                }
                                try
                                {
                                    //年龄
                                    dr["ageYear"] = jsonVals[x]["patient"]["ageYear"].ToString();
                                }
                                catch
                                {
                                    dr["ageYear"] = "";
                                }
                                try
                                {
                                    //birthday
                                    dr["birthday"] = jsonVals[x]["patient"]["birthday"].ToString();
                                }
                                catch
                                {
                                    dr["birthday"] = "";
                                }
                                    //idCardNo
                                try
                                {
                                    dr["idCardNo"] = jsonVals[x]["patient"]["idCardNo"].ToString();
                                }
                                catch
                                {
                                    dr["idCardNo"] = "";
                                }
                                try
                                {
                                    //phone
                                    dr["phone"] = jsonVals[x]["patient"]["phone"].ToString();
                                }
                                catch
                                {
                                    dr["phone"] = "";
                                }
                                try
                                {
                                    //address
                                    dr["address"] = jsonVals[x]["patient"]["address"].ToString();
                                }
                                catch
                                {
                                    dr["address"] = "";

                                }
                                try
                                {
                                    //住院病案号
                                    dr["patientNo"] = jsonVals[x]["patient"]["patientNo"].ToString();
                                }
                                catch
                                {
                                    dr["patientNo"] = "";

                                }

                                try
                                {
                                    //empiid
                                    dr["empiid"] = jsonVals[x]["patient"]["empid"].ToString();
                                }
                                catch
                                {
                                    dr["empiid"] = "";

                                }

                                try
                                {
                                    //病人就诊号
                                    dr["encounterNo"] = jsonVals[x]["patient"]["encounterNo"].ToString();
                                }
                                catch
                                {
                                    dr["encounterNo"] = "";
                                }
                                try
                                {
                                    //就诊医院
                                    dr["hospitalName"] = jsonVals[x]["patient"]["hospital"]["hospitalName"].ToString();
                                }
                                catch
                                {
                                    dr["hospitalName"] = "";
                                }
                                try
                                {
                                    //就诊科室
                                    dr["departmentName"] = jsonVals[x]["patient"]["department"]["departmentName"].ToString();
                                }
                                catch
                                {
                                    dr["departmentName"] = "";
                                }
                                try
                                {
                                    //病区
                                    dr["wardName"] = jsonVals[x]["patient"]["location"]["wardName"].ToString();
                                }
                                catch
                                {
                                    dr["wardName"] = "";
                                }
                                try
                                {
                                    //床号
                                    dr["bedName"] = jsonVals[x]["patient"]["location"]["bedName"].ToString();
                                }
                                catch
                                {
                                    dr["bedName"] = "";
                                }
                                try
                                {
                                    //送检医生
                                    dr["employeeName"] = jsonVals[x]["patient"]["chargeDoctor"]["employeeName"].ToString();
                                }
                                catch
                                {
                                    dr["employeeName"] = "";
                                }
                                try
                                {
                                    //就诊类型
                                    dr["encounterType"] = jsonVals[x]["patient"]["encounterType"].ToString();
                                }
                                catch
                                {
                                    dr["encounterType"] = "";
                                }
                                    //申请序号
                                dr["requestFormHisNo"] = jsonVals[x]["requestFormHisNo"].ToString();
                                try
                                {
                                    //申请单医嘱
                                    dr["requestType"] = jsonVals[x]["requestType"].ToString();
                                }
                                catch
                                {
                                    dr["requestType"] = "";
                                }


                                //临床病史
                                dr["clinicalHistory"] = "";


                                //临床诊断
                                dr["diagnosis"] = "";
                                dr["orderItemHisNo"] = "";
                                dr["orderName"] = "";
                                //检查项目(OrderItem)列表

                              
                                JArray jarray22 = JArray.Parse(jsonVals[x]["orderItem"].ToString());
                                foreach (JObject jj in jarray22)
                                {
                                    try
                                    {
                                        if (dr["orderItemHisNo"] == "")
                                            dr["orderItemHisNo"] = jj["orderItemHisNo"].ToString();
                                        else
                                            dr["orderItemHisNo"] = dr["orderItemHisNo"].ToString() + "|" + jj["orderItemHisNo"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                     //检查部位

                                    try
                                    {
                                        JArray jarray33 = JArray.Parse(jj["subItems"].ToString());

                                        foreach (JObject jj33 in jarray33)
                                        {

                                            if (dr["orderName"] == "")
                                                dr["orderName"] = jj33["orderName"].ToString();
                                            else
                                                dr["orderName"] = dr["orderName"].ToString() + "|" + jj33["orderName"].ToString();

                                        }
                                    }
                                    catch
                                    {
                                    }
                
                                }
 
                            }

                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog("提取住院申请单信息失败：解析数据异常：" + ee4.Message);
                            return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                            return "0";
                        }


                    }
                    catch (Exception ex)
                    {

                        log.WriteMyLog("提取住院申请单信息失败异常：" + ex.Message);
                        return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                        return "0";
                    }
                    int count = 0;

                    if (dt_sqd.Rows.Count <= 0)
                    {
                        log.WriteMyLog("提取住院申请单信息失败：未查询到相关住院信息");
                        return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                        return "0";
                    }
                    if (dt_sqd.Rows.Count > 1)
                    {

                        string Columns = f.ReadString(Sslbx, "Columns", "patientNo,requestFormHisNo,statusName,name,sexName,ageYear,hospitalName,departmentName,bedName,requestType,orderName,memo");//显示的项目对应字段
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "病案号,申请序号,状态,姓名,性别,年龄,医院,科室,床号,医嘱项目,检查部位,备注");//显示的项目名称
                        string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                        DataColumn dc0 = new DataColumn("序号");
                        dt_sqd.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqd.Rows.Count; x++)
                        {
                            dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                        }

                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                        yc.ShowDialog();

                        if(yc.DialogResult != DialogResult.Yes)
                        {
                             MessageBox.Show("未选择申请项目！");
                            return "0";
                        }
                        string rtn2 = yc.F_XH;
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("未选择申请项目！");
                            return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("请重新选择申请项目！");
                            return "0";
                        }


                    }

                    //查询诊断信息
                    string F_LCBS = "";
                    string F_LCZD = "";
                    string F_FY = "";
                    string F_HY = ""; string bbmc = ""; string F_BBMC = ""; string F_SJYS = "";
                    if (dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim() != "")
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream();
                            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding("gb2312"));
                            JsonWriter writer = new JsonTextWriter(sw);
                            Encoding eee = sw.Encoding;
                            writer.WriteStartObject();
                            writer.WritePropertyName("reqHeader");
                            writer.WriteStartObject();
                            writer.WritePropertyName("callFunction");
                            writer.WriteValue("queryApplyContent");
                            writer.WritePropertyName("systemId");
                            writer.WriteValue("PACS.BL");
                            writer.WritePropertyName("reqTimestamp");
                            writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            writer.WritePropertyName("terminalIp");
                            writer.WriteValue(Ipaddr);
                            writer.WriteEndObject();
                            writer.WritePropertyName("requestForm");
                            writer.WriteStartObject();
                            writer.WritePropertyName("requestFormHisNo");
                            writer.WriteValue(dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim());
                            writer.WriteEndObject();
                            writer.WriteEndObject();
                            writer.Flush();


                            string rtnrequest = GetHisSQD(JsonSvcWeb, ms.ToArray(), debug);

                            if (rtnrequest == "0")
                                return "0";

                            JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);

                            try
                            {
                                //临床诊断
                                dt_sqd.Rows[count]["diagnosis"] = ja["applyContent"]["examinationContent"]["clinicalDiagnosis"].ToString().Trim();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["diagnosis"] = "";
                            }
                            try
                            {
                                //bbmc
                                F_BBMC = ja["applyContent"]["examinationContent"]["sendExaminationObject"].ToString().Trim();
                            }
                            catch
                            {
                            }
                            try
                            {
                                //主诉 //病史 //描述 //家族史
                                dt_sqd.Rows[count]["clinicalHistory"] = "主诉：" + ja["applyContent"]["examinationContent"]["chiefComplaint"].ToString().Trim() + "    现病史：" + ja["applyContent"]["examinationContent"]["presentHistory"].ToString().Trim() + "    所见：" + ja["applyContent"]["examinationContent"]["professionsCondition"].ToString().Trim() + "    家族史：" + ja["applyContent"]["examinationContent"]["familyHistory"].ToString().Trim() + "    入院诊断：" + ja["applyContent"]["examinationContent"]["diagnosis"].ToString().Trim();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["clinicalHistory"] = "";
                            }
                            //  F_FY = ja["applyContent"]["patientBaseInfo"]["charges"].ToString();
                            //婚姻
                            try
                            {
                                F_HY = ja["applyContent"]["patientBaseInfo"]["marriage"].ToString();
                            }
                            catch
                            {
                            }
                            //申请医生
                            try
                            {
                                dt_sqd.Rows[count]["employeeName"] = ja["applyContent"]["patientBaseInfo"]["reqDoctor"].ToString();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["employeeName"] = "";
                            }
                            try
                            {
                                dt_sqd.Rows[count]["requestType"] = ja["applyContent"]["examinationContent"]["examinationContent"].ToString();
                            }
                            catch
                            {
                                dt_sqd.Rows[count]["requestType"] = "";
                            }
                        }
                        catch (Exception ee2)
                        {
                            log.WriteMyLog("提取住院申请单诊断信息异常："+ee2.Message);
                            //return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);

                        }
                    }
               

                    //获取标本信息
                          string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string bbxml = "";
                    string bbmc2="";
                    if (tqbblb == "1")
                    {
                        bbxml = GetBbxx(dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim(), ref bbmc2);
                    }

                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["empiid"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                       
                            xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["encounterNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["requestFormHisNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["patientNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["name"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        string xb = dt_sqd.Rows[count]["sexName"].ToString().Trim();
                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                        //----------------------------------------------------------

                        xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["ageYear"].ToString().Trim() + "岁" + (char)34 + " ";

                        //----------------------------------------------------------

                        xml = xml + "婚姻=" + (char)34 + F_HY + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "地址=" + (char)34 + dt_sqd.Rows[count]["phone"].ToString().Trim()+"^" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["address"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["wardName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["bedName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["idCardNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["departmentName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["employeeName"] + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "标本名称=" + (char)34 + F_BBMC + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "送检医院=" + (char)34 + dt_sqd.Rows[count]["hospitalName"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["requestType"].ToString().Trim()+"^" + dt_sqd.Rows[count]["orderItemHisNo"].ToString().Trim() + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["clinicalHistory"] + "]]></临床病史>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["diagnosis"] + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }


                        if (tqbblb == "1")
                            xml = xml + bbxml;

                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());


                        return xml;
                    }
                    catch (Exception e)
                    {

                  
                        log.WriteMyLog("提取住院申请单信息失败：xml解析错误---" + e.Message.ToString());
                        return GetZYHis(Ssbz, Sslbx, debug, Ipaddr);
                        return "0";
                    }
                    #endregion
                }
                if (Sslbx == "门诊申请号" || Sslbx == "诊疗卡号" || Sslbx == "挂号序号")
                {
                    #region  门诊申请号

                    string request_xml = "";
                    if (Sslbx == "诊疗卡号")
                        request_xml = "<Request><CardNo>" + Ssbz.Trim() + "</CardNo><StartDate>" + DateTime.Now.AddDays(-60).ToString("yyyyMMdd") + "</StartDate>"
                 + "<EndDate>" + DateTime.Now.ToString("yyyyMMdd") + "</EndDate><EmpiId></EmpiId><RegNo></RegNo><CheckNumber></CheckNumber><Flag>0</Flag></Request>";

                    if (Sslbx == "挂号序号")
                        request_xml = "<Request><CardNo></CardNo><StartDate>" + DateTime.Now.AddDays(-60).ToString("yyyyMMdd") + "</StartDate>"
                 + "<EndDate>" + DateTime.Now.ToString("yyyyMMdd") + "</EndDate><EmpiId></EmpiId><RegNo>" + Ssbz.Trim() + "</RegNo><CheckNumber></CheckNumber><Flag>1</Flag></Request>";

                    if (Sslbx == "门诊申请号")
                        request_xml = "<Request><CardNo></CardNo><StartDate>" + DateTime.Now.AddDays(-60).ToString("yyyyMMdd") + "</StartDate>"
                + "<EndDate>" + DateTime.Now.ToString("yyyyMMdd") + "</EndDate><EmpiId></EmpiId><RegNo></RegNo><CheckNumber>" + Ssbz.Trim() + "</CheckNumber><Flag>2</Flag></Request>";


                    if (request_xml.Trim() == "")
                    {
                        MessageBox.Show("查询条件异常");
                        return "0";
                    }

                    if (debug == "1")
                        log.WriteMyLog("查询条件：" + request_xml);

                    string rtn_xml ="";
                     try
                    {
                        gdszyyMzHisWeb.HisForInspectItemService mzhis = new LGHISJKZGQ.gdszyyMzHisWeb.HisForInspectItemService();
                        if (MZHISWeb.Trim()!="")
                        mzhis.Url = MZHISWeb;
                        rtn_xml = mzhis.GetCheckRecord(request_xml);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("连接门诊WebService服务异常：" + ee.Message+"\r\n");
                        return "0";
                    }
                    if (rtn_xml.Trim() == "")
                    {
                        MessageBox.Show("提取门诊申请单信息失败：返回消息为空！\r\n是否提取病人基本信息");
                        
                      return   GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
                      
                    }

                    if (debug == "1")
                        log.WriteMyLog(rtn_xml);
   
                      XmlNode xmlok_DATA = null;
            XmlDocument xd2 = new XmlDocument();
            try
            {
                xd2.LoadXml(rtn_xml);
                xmlok_DATA = xd2.SelectSingleNode("/Response");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("提取门诊申请单信息失败1" + "\r\n是否提取病人基本信息");
                log.WriteMyLog("提取门诊申请单信息失败：解析DATA异常，" + xmlok_e.Message);
                return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
              
            }

            string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
            string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

            if (ResultCode != "0")
            {
                MessageBox.Show("" + ErrorMsg+"\r\n是否提取病人基本信息");
                log.WriteMyLog("提取门诊申请单信息失败：" + ErrorMsg);
                return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
            }

            if (xmlok_DATA["Info"].InnerText == "")
            {
                MessageBox.Show("提取门诊申请单信息失败：Info节点为空" + "\r\n是否提取病人基本信息");
                return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
            }

          XmlNodeList  node_list = xd2.SelectNodes("/Response/Info/Record");
          if (node_list.Count <= 0)
          {
              MessageBox.Show("未查询到门诊申请单信息\r\n是否提取病人基本信息");
              return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
          }


          DataTable dt_sqd = new DataTable();
          //姓名
          DataColumn dc_PatientName = new DataColumn("PatientName");
          dt_sqd.Columns.Add(dc_PatientName);
          //性别
          DataColumn dc_Sex = new DataColumn("Sex");
          dt_sqd.Columns.Add(dc_Sex);
          //出生日期
          DataColumn dc_Birthday = new DataColumn("Birthday");
          dt_sqd.Columns.Add(dc_Birthday);
          //病历号
          DataColumn dc_MedicalNo = new DataColumn("MedicalNo");
          dt_sqd.Columns.Add(dc_MedicalNo);
          //身份证号
          DataColumn dc_IdCard = new DataColumn("IdCard");
          dt_sqd.Columns.Add(dc_IdCard);
          //EmpiId
          DataColumn dc_EmpiId = new DataColumn("EmpiId");
          dt_sqd.Columns.Add(dc_EmpiId);


          //检查申请ID
          DataColumn dc_CheckNumber = new DataColumn("CheckNumber");
          dt_sqd.Columns.Add(dc_CheckNumber);
          //申请科室名称
          DataColumn dc_AppDeptName = new DataColumn("AppDeptName");
          dt_sqd.Columns.Add(dc_AppDeptName);
          //执行科室名称
          DataColumn dc_ExecDeptName = new DataColumn("ExecDeptName");
          dt_sqd.Columns.Add(dc_ExecDeptName);
          //申请医生姓名
          DataColumn dc_DoctorName = new DataColumn("DoctorName");
          dt_sqd.Columns.Add(dc_DoctorName);
          //检查项目名称
          DataColumn dc_CheckItemName = new DataColumn("CheckItemName");
          dt_sqd.Columns.Add(dc_CheckItemName);
          //检查项目类型
          DataColumn dc_CheckType = new DataColumn("CheckType");
          dt_sqd.Columns.Add(dc_CheckType);
          //总金额
          DataColumn dc_Money = new DataColumn("Money");
          dt_sqd.Columns.Add(dc_Money);
          //诊断名称
          DataColumn dc_Diagnosis = new DataColumn("Diagnosis");
          dt_sqd.Columns.Add(dc_Diagnosis);
          //病人来源
          DataColumn dc_Source = new DataColumn("Source");
          dt_sqd.Columns.Add(dc_Source);
          //年龄
          DataColumn dc_Age = new DataColumn("Age");
          dt_sqd.Columns.Add(dc_Age);
          //
          DataColumn dc_RegNo = new DataColumn("RegNo");
          dt_sqd.Columns.Add(dc_RegNo);
          //院区
          DataColumn dc_HospitalShortName = new DataColumn("HospitalShortName");
          dt_sqd.Columns.Add(dc_HospitalShortName);
          //病史
          DataColumn dc_ClinicHistory = new DataColumn("ClinicHistory");
          dt_sqd.Columns.Add(dc_ClinicHistory);

          string PatientName = xmlok_DATA["Info"]["PatientName"].InnerText.Trim();
          string Sex = xmlok_DATA["Info"]["Sex"].InnerText.Trim();
          string Birthday = xmlok_DATA["Info"]["Birthday"].InnerText.Trim();
          string MedicalNo = xmlok_DATA["Info"]["MedicalNo"].InnerText.Trim();
          string IdCard = "";

                  
          string RegNo = xmlok_DATA["Info"]["RegNo"].InnerText.Trim();
    
          string empiid = xmlok_DATA["Info"]["EmpiId"].InnerText.Trim();
    

          try
          {
           IdCard=   xmlok_DATA["Info"]["IdCard"].InnerText.Trim();
          }
          catch
          {
          }
        
          foreach (XmlNode node in node_list)
          {
              DataRow dr = dt_sqd.NewRow();
              dt_sqd.Rows.Add(dr);
              dr["PatientName"] = PatientName;
              dr["Sex"] = Sex;
              dr["Birthday"] = Birthday;
              dr["MedicalNo"] = MedicalNo;
              dr["IdCard"] = IdCard;
              dr["CheckNumber"] = node["CheckNumber"].InnerText.Trim();
              dr["AppDeptName"] = node["AppDeptName"].InnerText.Trim();
              dr["ExecDeptName"] = node["ExecDeptName"].InnerText.Trim();
              dr["DoctorName"] = node["DoctorName"].InnerText.Trim();
              dr["CheckItemName"] = node["CheckItemName"].InnerText.Trim();
              dr["CheckType"] = node["CheckType"].InnerText.Trim();
              dr["Money"] = node["Money"].InnerText.Trim();
              dr["Diagnosis"] = node["Diagnosis"].InnerText.Trim();
              dr["RegNo"] = RegNo.Trim();
              dr["EmpiId"] = empiid.Trim();

              dr["HospitalShortName"] = node["HospitalShortName"].InnerText.Trim();
              dr["ClinicHistory"] = node["ClinicHistory"].InnerText.Trim();

               string Source=node["Source"].InnerText.Trim();

               if (Source == "1") dr["Source"] = "门诊";
               else if (Source == "2") dr["Source"] = "住院";
               else if (Source == "3") dr["Source"] = "体检";
             
                dr["Age"]= ZGQClass.CsrqToAge(dr["Birthday"].ToString());
               
          }
         

                    if (dt_sqd.Rows.Count < 1)
                    {
                        MessageBox.Show("未查到相应的门诊申请单数据");
                        return GetMZHis(Ssbz, Sslbx, MZHISWeb,debug);
                    }

                    int tkts = f.ReadInteger(Sslbx, "tkts", 2);

                    int  count=0;
                    string sqxh = "";
                    string yzxm = "";
                    if (dt_sqd.Rows.Count >= tkts)
                    {
                        DataColumn dc0 = new DataColumn("序号");
                        dt_sqd.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqd.Rows.Count; x++)
                        {
                            dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                        }


                        GDSZYY_YZ_SELECT yc = new GDSZYY_YZ_SELECT(dt_sqd);
                        yc.ShowDialog();

                        if (yc.DialogResult != DialogResult.Yes)
                        {
                            MessageBox.Show("未选择申请项目！"); return "0";
                        }

                        string rtn2 = yc.F_XH;
                        sqxh = yc.F_sqxh;
                        yzxm = yc.F_yzxm;
                        if (rtn2.Trim() == "")
                        {
                            MessageBox.Show("未选择申请项目！"); return "0";
                        }
                        try
                        {
                            count = int.Parse(rtn2);
                        }
                        catch
                        {
                            MessageBox.Show("请重新选择申请项目！"); return "0";
                        }
                    }
                    else
                    {
                        sqxh = dt_sqd.Rows[count]["CheckNumber"].ToString().Trim();
                        yzxm = dt_sqd.Rows[count]["CheckItemName"].ToString().Trim();
                    }


                    //获取标本信息
                    string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
                    string bbxml = "";
                    string bbmc2 = "";
                    if (tqbblb == "1")
                    {
                        bbxml = GetBbxx(sqxh, ref bbmc2);
                    }

                     //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["EmpiId"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["RegNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + sqxh + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["MedicalNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["PatientName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                        string xb = dt_sqd.Rows[count]["Sex"].ToString().Trim();
                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch
                        {
                             xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["Age"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["IdCard"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["AppDeptName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["DoctorName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "标本名称=" + (char)34 +"" + (char)34 + " ";
                        //----------------------------------------------------------

                        xml = xml + "送检医院=" + (char)34 + dt_sqd.Rows[count]["HospitalShortName"].ToString().Trim() + (char)34 + " ";

                        //----------------------------------------------------------
                        xml = xml + "医嘱项目=" + (char)34 + yzxm + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                        xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["Source"].ToString() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["ClinicHistory"].ToString() + "]]></临床病史>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["Diagnosis"].ToString() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.Message.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }

                        if (tqbblb == "1")
                            xml = xml + bbxml;
                        xml = xml + "</LOGENE>";

                        if (debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());
                        return xml;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("提取门诊申请单信息异常："+e.Message);
                        log.WriteMyLog("提取门诊申请单信息异常：" + e.Message);
                        return "0";
                    }

                    #endregion
                }
                //if (Sslbx == "体检号")
                //{
                //    #region  体检号


                //    string rtn_xml = "";
                //    try
                //    {
                     
                //        if (MZHISWeb.Trim() != "")
                //            mzhis.Url = MZHISWeb;
                //        rtn_xml = mzhis.getLoadPaintInfoResult(Sslbx);
                //    }
                //    catch (Exception ee)
                //    {
                //        MessageBox.Show("连接WebService异常：" + ee.Message + "\r\n");
                //        return "0";
                //    }
                //    if (rtn_xml.Trim() == "")
                //    {
                //        MessageBox.Show("提取体检信息失败：返回消息为空");

                //        return "0";

                //    }

                //    if (debug == "1")
                //        log.WriteMyLog(rtn_xml);

                //    XmlNode xn = null;
                //    XmlDocument xd2 = new XmlDocument();
                //    try
                //    {
                //        xd2.LoadXml(rtn_xml);
                //        xn = xd2.SelectSingleNode("/getLoadPaintInfoResult");
                //    }
                //    catch (Exception xmlok_e)
                //    {
                //        MessageBox.Show("提取体检信息失败:" + xmlok_e.Message);
                //        return "0";
                //    }

                  

                //    //-返回xml----------------------------------------------------
                //    try
                //    {

                //        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                //        xml = xml + "<LOGENE>";
                //        xml = xml + "<row ";
                //        try
                //        {
                //            xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["EmpiId"].ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["RegNo"].ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "申请序号=" + (char)34 + sqxh + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {

                //            xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["MedicalNo"].ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        try
                //        {
                //            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        try
                //        {
                //            xml = xml + "姓名=" + (char)34 + xn["DOC_NameO"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                    
                //        try
                //        {
                //            string xb = xn["sex"].InnerText.ToString().Trim();
                //            if (xb == "M") xb = "男";
                //            if (xb == "F") xb = "女";
                //            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                //        }
                //        catch
                //        {
                //            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        xml = xml + "年龄=" + (char)34 + xn["age"].InnerText.ToString().Trim() + (char)34 + " ";

                //        //----------------------------------------------------------

                //        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";

                //        //----------------------------------------------------------
                //        try
                //        {

                //            xml = xml + "地址=" + (char)34 + xn["Address_1 "].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "电话=" + (char)34 + xn["Phone_1"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "床号=" + (char)34 + xn["BED_NUM"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";

                //        //----------------------------------------------------------
                //        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "送检科室=" + (char)34 + xn["LOCATION"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                //        }
                //        //----------------------------------------------------------

                //        try
                //        {
                //            xml = xml + "送检医生=" + (char)34 + xn["Ord_By"].InnerText.ToString().Trim() + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------

                //        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------
                //        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------

                //        xml = xml + "送检医院=" + (char)34 + "大院" + (char)34 + " ";

                //        //----------------------------------------------------------
                //        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                //        //----------------------------------------------------------
                //        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                //        //----------------------------------------------------------
                //        xml = xml + "备用2=" + (char)34 + xn["organ_Name"].InnerText.ToString().Trim() + (char)34 + " ";
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "病人类别=" + (char)34 +"体检" + (char)34 + " ";
                //        }
                //        catch
                //        {
                //            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                //        }
                //        xml = xml + "/>";
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "<临床病史><![CDATA[" + xn["spec_intm"].InnerText.ToString().Trim() + "]]></临床病史>";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                //        }
                //        //----------------------------------------------------------
                //        try
                //        {
                //            xml = xml + "<临床诊断><![CDATA[" + xn["comments"].InnerText.ToString().Trim() + "]]></临床诊断>";
                //        }
                //        catch (Exception ee)
                //        {
                //            exp = exp + ee.Message.ToString();
                //            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                //        }

                    
                //        xml = xml + "</LOGENE>";

                //        if (debug == "1" && exp.Trim() != "")
                //            log.WriteMyLog(exp.Trim());
                //        return xml;
                //    }
                //    catch (Exception e)
                //    {
                //        MessageBox.Show("提取体检信息异常：" + e.Message);
                   
                //        return "0";
                //    }

                //    #endregion
                //}

                if (Sslbx == "门诊取消执行")
                {
                    #region  门诊取消执行



                    string DeptId = f.ReadString(Sslbx, "deptid", "60027").Replace("\0", "").Trim(); ;
                    string yhgh = f.ReadString(Sslbx, "yhbh", "").Replace("\0", "").Trim(); 

                    string request_xml = "<Request><CheckNumber>" + Ssbz.Trim() + "</CheckNumber><CancelDeptId>" + DeptId + "</CancelDeptId><CancelUserNo>" + yhgh + "</CancelUserNo></Request>";

                    if (debug == "1")
                        log.WriteMyLog("门诊取消执行失败：查询条件：" + request_xml);
                    string rtn_xml = "";
                    try
                    {
                        gdszyyMzHisWeb.HisForInspectItemService mzhis = new LGHISJKZGQ.gdszyyMzHisWeb.HisForInspectItemService();
                        if (MZHISWeb!="")
                        mzhis.Url = MZHISWeb;
                        rtn_xml = mzhis.CancelExecCheckItem(request_xml);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("门诊取消执行失败：连接WebService服务异常：" + ee.Message + "\r\n");
                        return "0";
                    }
                    if (rtn_xml.Trim() == "")
                    {
                        MessageBox.Show("门诊取消执行失败！返回消息为空！");
                        return "0";
                    }


                    if (debug == "1")
                        log.WriteMyLog(rtn_xml);

                    XmlNode xmlok_DATA = null;
                    XmlDocument xd2 = new XmlDocument();
                    try
                    {
                        xd2.LoadXml(rtn_xml);
                        xmlok_DATA = xd2.SelectSingleNode("/Response");
                    }
                    catch (Exception xmlok_e)
                    {

                        MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                        return "0";
                    }

                    string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
                    string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

                    if (ResultCode != "0")
                    {
                        MessageBox.Show("门诊取消执行失败：" + ErrorMsg);
                        return "0";
                    }
                    else
                    {
                        MessageBox.Show("门诊取消成功：" + ErrorMsg);
                        return "0";
                    }

                    #endregion
                }
               if (Sslbx == "住院取消登记")
                {
                    #region   回写住院HIS状态
                        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                        DataTable jcxx = new DataTable();
                        try
                        {
                            jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + Ssbz.Trim() + "'", "jcxx");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                            return "0";
                        }
                        if (jcxx == null)
                        {
                            MessageBox.Show("病理数据库设置有问题！");
                            return "0";
                        }
                        if (jcxx.Rows.Count < 1)
                        {
                             MessageBox.Show("无此病理号记录！");
                                     return "0";
                        }


                        if (jcxx.Rows[0]["F_brlb"].ToString() != "住院")
                        {
                             MessageBox.Show("非住院病人不能取消登记！");
                             return "0";
                        }
                        string yzxmid = "";
                        try
                        {
                            yzxmid = jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1].Trim();
                        }
                        catch
                        {
                        }

                        if (yzxmid == "" || jcxx.Rows[0]["F_SQXH"].ToString() == "")
                        {
                          MessageBox.Show("申请单号或子申请单号为空，不能取消登记！");
                         return "0";
                        }


                            string sqdzt = ""; string sqdztCode = "";
                             sqdztCode = "CLDS.RIUNR.0000"; sqdzt = "取消登记"; 
                          

                            string actionCode = "cancel";
                            string actionName = "PACS退单";
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
                            writer.WriteValue( Ssbz.Trim());
                            //检查分类
                            writer.WritePropertyName("requestType");
                            writer.WriteValue("");
                            //检查项目列表2222222222222
                            writer.WritePropertyName("orderItem");
                            writer.WriteStartArray();
                            string[] orderItemHisNos = yzxmid.Split('|');
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
                            writer.WriteValue("");

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
                            writer.Flush();
                            string rtnrequest =GetHisSQD(JsonSvcWeb, ms.ToArray(),debug);
                            JObject ja = (JObject)JsonConvert.DeserializeObject(rtnrequest);
                            string respCode = ja["respHeader"]["respCode"].ToString();
                            string respMessage = ja["respHeader"]["respMessage"].ToString();

                            if (respCode != "000000")
                              MessageBox.Show("执行失败：" + respCode + "^" + respMessage);
                            else
                              MessageBox.Show("执行成功：" + respCode + "^" + respMessage);
                             return  "0";

                        # endregion
                }
             
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");
                    return "0";
                }
            } return "0";
        }

        public static string GetHisSQD(string JsonSvcWeb, byte[] reqbyte,string  debug)
        {
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {   string reqtext = Encoding.GetEncoding("gb2312").GetString(reqbyte);

                if (debug == "1")
                    log.WriteMyLog("入参：" + reqtext);
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(JsonSvcWeb);
                if (webrequest == null)
                {
                    MessageBox.Show("连接住院HIS服务失败：\r\n" + JsonSvcWeb);
                    return "0";
                }
                webrequest.KeepAlive = true;
                webrequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                webrequest.Method = "POST";
                webrequest.Timeout = 20000;
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
                    MessageBox.Show("获取住院HIS数据异常：" + ee3.Message);
                    return "0";
                }
                if (debug == "1")
                    log.WriteMyLog("返回："+Content.Trim());

                return Content;


            }
            catch(Exception  ee2)
            {
                MessageBox.Show("获取住院HIS数据异常2：" + ee2.Message);
                return "0";
            }


        }

        public static string GetMZHis(string Ssbz, string Sslbx, string MZHISWeb,string debug)
        {
            #region  Sslbx == "诊疗卡号" || Sslbx == "挂号序号")

            string request_xml = "<Request><CardNo>" + Ssbz.Trim() + "</CardNo><RegNo></RegNo></Request>";
            if(Sslbx == "诊疗卡号")
                request_xml = "<Request><CardNo>" + Ssbz.Trim() + "</CardNo><RegNo></RegNo></Request>";

                if(Sslbx == "挂号序号")
                    request_xml = "<Request><CardNo></CardNo><RegNo>" + Ssbz.Trim() + "</RegNo></Request>";
           

            if (debug == "1")
                log.WriteMyLog("查询条件：" + request_xml);
            string rtn_xml = "";
            try
            {
                gdszyyMzHisWeb.HisForInspectItemService mzhis = new LGHISJKZGQ.gdszyyMzHisWeb.HisForInspectItemService();
                if (MZHISWeb.Trim()!="")
                mzhis.Url = MZHISWeb;
                rtn_xml = mzhis.GetPatientInfo(request_xml);
            }
            catch (Exception ee)
            {
                MessageBox.Show("提取基本信息失败：连接门诊WebService服务异常：" + ee.Message + "\r\n");
                return "0";
            }
            if (rtn_xml.Trim() == "")
            {
                MessageBox.Show("提取基本信息失败：返回消息为空！");
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog(rtn_xml);

            XmlNode xmlok_DATA = null;
            XmlDocument xd2 = new XmlDocument();
            try
            {
                xd2.LoadXml(rtn_xml);
                xmlok_DATA = xd2.SelectSingleNode("/Response");
            }
            catch (Exception xmlok_e)
            {

                MessageBox.Show("提取基本信息失败：解析DATA异常：" + xmlok_e.Message);
                return "0";
            }

            string ResultCode = xmlok_DATA["ResultCode"].InnerText.Trim();
            string ErrorMsg = xmlok_DATA["ErrorMsg"].InnerText.Trim();

            if (ResultCode != "0")
            {
                MessageBox.Show("提取基本信息失败：：" + ErrorMsg);
                return "0";
            }

            if (xmlok_DATA["Info"].InnerText == "")
            {
                MessageBox.Show("提取基本信息失败：info为空");
                return "0";
            }
            string exp = "";
            //-返回xml----------------------------------------------------
            try
            {

                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "病人编号=" + (char)34 + xmlok_DATA["Info"]["EmpiId"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
          
                    xml = xml + "病人编号=" + (char)34 + +(char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "就诊ID=" + (char)34 + xmlok_DATA["Info"]["RegNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "门诊号=" + (char)34 + xmlok_DATA["Info"]["MedicalNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "姓名=" + (char)34 + xmlok_DATA["Info"]["PatientName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "性别=" + (char)34 + xmlok_DATA["Info"]["Sex"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "年龄=" + (char)34 + ZGQClass.CsrqToAge(xmlok_DATA["Info"]["Birthday"].InnerText.Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "婚姻=" + (char)34 + xmlok_DATA["Info"]["Marry"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "地址=" + (char)34 + xmlok_DATA["Info"]["PatientPhone"].InnerText.Trim() +"^"+ (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "电话=" + (char)34 + xmlok_DATA["Info"]["Address"].InnerText.Trim()  + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "身份证号=" + (char)34 + xmlok_DATA["Info"]["IdCard"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "民族=" + (char)34 + xmlok_DATA["Info"]["Nation"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                }

                //----------------------------------------------------------
                try
                {
                    xml = xml + "职业=" + (char)34 + xmlok_DATA["Info"]["Occupation"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                    try
                    {
                         xml = xml + "送检医院=" + (char)34 + xmlok_DATA["Info"]["HospitalShortName"].InnerText.Trim() + (char)34 + " ";
                    }
                catch
                    {
                xml = xml + "送检医院=" + (char)34 + "广东省中医院" + (char)34 + " ";
                }

                //----------------------------------------------------------
                xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "费别=" + (char)34 + xmlok_DATA["Info"]["MedicalTreatment"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                }
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                xml = xml + "</LOGENE>";

                if (debug == "1" && exp.Trim() != "")
                    log.WriteMyLog(exp.Trim());
                return xml;
            }
            catch (Exception e)
            {
                MessageBox.Show("提取基本信息异常："+e.Message);
                log.WriteMyLog("提取基本信息异常：" + e.Message);
                return "0";
            }

            #endregion
        }

        public static string GetZYHis(string Ssbz, string Sslbx, string debug,string IP)
        {
            if (Sslbx == "住院申请号")
            {
                MessageBox.Show("获取住院病人申请单信息失败！");
                return "0";
            }


            string ZYHISWeb = f.ReadString(Sslbx, "WSURL", "http://svc.chas.gdhtcm.com:9080/ChasSvc/services/ChasCommonPort").Replace("\0", "").Trim(); ;

            string queryMode = f.ReadString(Sslbx, "queryMode", "Current").Replace("\0", "").Trim();

            string request_xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>"
                    + "<QueryPatientRequest xmlns=\"http://chas.hit.com/transport/integration/common/msg\">"
                    + "<reqHeader>"
                    + "<systemId>AJ</systemId>"
                    + "<reqTimestamp>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</reqTimestamp>"
                    + "<terminalIp>" + IP + "</terminalIp>"
                    + "</reqHeader>"
                    + "<patientNo>" + Ssbz + "</patientNo>"
                    + "<startDate>" + DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd") + "</startDate>"
                    + "<endDate>" + DateTime.Today.ToString("yyyy-MM-dd") + "</endDate>"
                    + "<dateField>AdmissionDate</dateField>"
                    + "<queryMode>" + queryMode + "</queryMode>"
                    + "</QueryPatientRequest>";


            if (debug == "1")
                log.WriteMyLog("请求XML：" + request_xml);

            string ResponseXMl = "";
            try
            {
                gdszyyWS.ChasCommonSvc zyy = new LGHISJKZGQ.gdszyyWS.ChasCommonSvc();
                if (ZYHISWeb != "")
                    zyy.Url = ZYHISWeb;
                ResponseXMl = zyy.queryPatient(request_xml);
            }
            catch (Exception ee2)
            {
                MessageBox.Show("获取住院病人基本信息失败：" + ee2.Message);
                return "0";
            }

            if (debug == "1")
                log.WriteMyLog("返回：" + ResponseXMl);

            if (ResponseXMl.Trim() == "")
            {
                MessageBox.Show("获取住院病人基本信息失败：返回为空");
                return "0";
            }
            try
            {
            StringReader xmlstr = null;
            XmlTextReader xmread = null;
            xmlstr = new StringReader(ResponseXMl);
            xmread = new XmlTextReader(xmlstr);
            XmlDocument readxml2 = new XmlDocument();
            try
            {
                readxml2.Load(xmread);
            }
            catch (Exception e2)
            {
                MessageBox.Show("获取住院病人基本信息失败：" + e2.Message.ToString());
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "http://chas.hit.com/transport/integration/common/msg");
            XmlNode xmlok_DATA = null;
                xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:respHeader", nsMgr);
            
            string ResultCode = xmlok_DATA["respCode"].InnerText.Trim(); 
            string ErrorMsg = xmlok_DATA["respMessage"].InnerText.Trim();
            if (ResultCode != "000000")
            {
                MessageBox.Show("获取住院病人基本信息失败：" + ErrorMsg);
                return "0";
            }
            try
            {
                xmlok_DATA = readxml2.SelectSingleNode("/ns:QueryPatientResponse/ns:patient", nsMgr);
                if (xmlok_DATA["name"].InnerText == "")
                {
                    MessageBox.Show("获取住院病人基本信息失败：姓名为空");
                    return "0";
                }
            }
            catch
            {
                MessageBox.Show("获取住院病人基本信息失败：未查询到此病人信息" );
                return "0";
            }
            string exp = "";
            //-返回xml----------------------------------------------------
      
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                try
                {
                    xml = xml + "病人编号=" + (char)34 + xmlok_DATA["encounterNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {

                    xml = xml + "病人编号=" + (char)34 +"" +(char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "就诊ID=" + (char)34 +"" +(char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "住院号=" + (char)34 + xmlok_DATA["patientNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "姓名=" + (char)34 + xmlok_DATA["name"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "性别=" + (char)34 + xmlok_DATA["sexName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "年龄=" + (char)34 + ZGQClass.CsrqToAge(xmlok_DATA["birthday"].InnerText.Trim()) + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {

                    xml = xml + "地址=" + (char)34 + xmlok_DATA["phone"].InnerText.Trim() + "^" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "电话=" + (char)34 + xmlok_DATA["address"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "病区=" + (char)34 + xmlok_DATA["wardName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "床号=" + (char)34 + xmlok_DATA["bedName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "身份证号=" + (char)34 + xmlok_DATA["idCardNo"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "民族=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                }

                //----------------------------------------------------------
                try
                {
                    xml = xml + "职业=" + (char)34 +"" +(char)34 + " ";
                }
                catch
                {
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "送检科室=" + (char)34 + xmlok_DATA["departmentName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                }
                //----------------------------------------------------------

                try
                {
                    xml = xml + "送检医生=" + (char)34 + xmlok_DATA["chargeDoctorName"].InnerText.Trim() + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------

                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "送检医院=" + (char)34 + xmlok_DATA["hospitalName"].InnerText.Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医院=" + (char)34 + "广东省中医院" + (char)34 + " ";
                }

                //----------------------------------------------------------
                xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                }
                //----------------------------------------------------------
                try
                {
                    string brlb = xmlok_DATA["chargeDoctorName"].InnerText.Trim();
                    if (brlb == "ECTY.IP.0000")
                        brlb = "住院";
                    else if (brlb == "ECTY.OP.0000")
                        brlb = "门诊";
                    else if (brlb == "ECTY.HLC.0000")
                        brlb = "体检";
                    else if (brlb == "ECTY.AEU.0000")
                        brlb = "急诊";
                    else
                        brlb = "住院";

                    xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                }
                xml = xml + "/>";
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                }
                //----------------------------------------------------------
                try
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                catch (Exception ee)
                {
                    exp = exp + ee.Message.ToString();
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                }
                xml = xml + "</LOGENE>";

               if(debug=="1")
                   log.WriteMyLog(xml);
              return xml;
            }
            catch (Exception e)
            {
                MessageBox.Show("获取住院病人基本信息异常：" + e.Message);
                log.WriteMyLog("提取基本信息异常：" + e.Message);
                return "0";
            }


        }


        public static string GetBbxx( string sqxh,ref  string bbmc)
        {
         
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string BBLB_XML = "";
            DataTable dt_bb = new DataTable();
        
                dt_bb = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_SQXH= '" + sqxh + "'  order by F_ID", "bbxx");
                string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                if (dt_bb == null)
                {
                    return "";
                }
                else
                {
                    if (dt_bb.Rows.Count <= 0)
                    {
                        return "";
                    }
                    else
                    {
                        BBLB_XML = "<BBLB>";
                        try
                        {
                            for (int x = 0; x < dt_bb.Rows.Count; x++)
                            {
                                try
                                {
                                    BBLB_XML = BBLB_XML + "<row ";
                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_bb.Rows[x]["F_BBXH"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bb.Rows[x]["F_BBTMH"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bb.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bb.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + dt_bb.Rows[x]["F_BZ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bb.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bb.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "/>";

                                    if (bbmc == "")
                                        bbmc = dt_bb.Rows[x]["F_BBMC"].ToString().Trim();
                                    else
                                        bbmc = bbmc + "," + dt_bb.Rows[x]["F_BBMC"].ToString().Trim();
                                }
                                catch (Exception eee)
                                {
                                    MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                    break;
                                }
                            }
                            BBLB_XML = BBLB_XML + "</BBLB>";

                            return BBLB_XML;
                        }
                        catch (Exception e3)
                        {
                            MessageBox.Show("获取标本名称异常：" + e3.Message);
                        }
                        
                    }
                
            }
            return "";
        }
    }
}
