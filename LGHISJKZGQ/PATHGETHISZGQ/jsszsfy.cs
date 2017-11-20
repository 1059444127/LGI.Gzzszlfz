using System;
using System.Collections.Generic;
using System.Text;
using LGHISJKZGQ;
using System.Windows.Forms;
using System.Xml;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    //苏附一
    class jsszsfy
    {
      
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            string pathWEB = f.ReadString(Sslbx, "url", "http://10.1.1.71/webapp/PathologyService.asmx").Trim(); //获取sz.ini中设置的webservicesurl
            string debug = f.ReadString(Sslbx, "debug", "");
            string yq = f.ReadString(Sslbx, "yq", "2");
           
            if(yq.Trim()=="十梓街")  yq="1";
             if(yq.Trim()=="平江")  yq="2";
 
            if (Sslbx != "")
            {

                if (Sslbx == "住院号" || Sslbx == "门诊号" || Sslbx == "发票号" || Sslbx == "十梓街住院号" || Sslbx == "十梓街门诊号" || Sslbx == "十梓街发票号" || Sslbx == "平江住院号" || Sslbx == "平江门诊号" || Sslbx == "平江发票号")
                {

                    string  Request_xml="";
                    if (Sslbx == "住院号")
                    {
                        Request_xml="<Request>"
                                    +"<yq>"+yq+"</yq>"
                                    +"<paitent_id></paitent_id>"
                                    +"<real_no></real_no>"
                                    +"<inpatient_no>"+Ssbz.Trim()+"</inpatient_no>"			//住院号
                                    +"</Request>";

                    }

                    if (Sslbx == "门诊号")
                    {
                        Request_xml="<Request>"
                                    +"<yq>"+yq+"</yq>"							//院区（1：十梓街；2：平江）
                                    +"<paitent_id>"+Ssbz.Trim()+"</paitent_id>"		//门诊号（必填）
                                    +"<real_no></real_no>"				//发票号（可选）
                                    +"<inpatient_no></inpatient_no>"
                                    +"</Request>";
                    }

                    if (Sslbx == "发票号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>" + yq + "</yq>"							//院区（1：十梓街；2：平江）
                                    + "<paitent_id></paitent_id>"		//门诊号（必填）
                                    + "<real_no>" + Ssbz.Trim() + "</real_no>"				//发票号（可选）
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "十梓街住院号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>1</yq>"
                                    + "<paitent_id></paitent_id>"
                                    + "<real_no></real_no>"
                                    + "<inpatient_no>" + Ssbz.Trim() + "</inpatient_no>"			//住院号
                                    + "</Request>";

                    }

                    if (Sslbx == "十梓街门诊号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>1</yq>"							//院区（1：十梓街；2：平江）
                                    + "<paitent_id>" + Ssbz.Trim() + "</paitent_id>"		//门诊号（必填）
                                    + "<real_no></real_no>"				//发票号（可选）
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "十梓街发票号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>1</yq>"							//院区（1：十梓街；2：平江）
                                    + "<paitent_id></paitent_id>"		//门诊号（必填）
                                    + "<real_no>" + Ssbz.Trim() + "</real_no>"				//发票号（可选）
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "平江住院号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>2</yq>"
                                    + "<paitent_id></paitent_id>"
                                    + "<real_no></real_no>"
                                    + "<inpatient_no>" + Ssbz.Trim() + "</inpatient_no>"			//住院号
                                    + "</Request>";

                    }

                    if (Sslbx == "平江门诊号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>2</yq>"							//院区（1：十梓街；2：平江）
                                    + "<paitent_id>" + Ssbz.Trim() + "</paitent_id>"		//门诊号（必填）
                                    + "<real_no></real_no>"				//发票号（可选）
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if (Sslbx == "平江发票号")
                    {
                        Request_xml = "<Request>"
                                    + "<yq>2</yq>"							//院区（1：十梓街；2：平江）
                                    + "<paitent_id></paitent_id>"		//门诊号（必填）
                                    + "<real_no>" + Ssbz.Trim() + "</real_no>"				//发票号（可选）
                                    + "<inpatient_no></inpatient_no>"
                                    + "</Request>";
                    }

                    if(Request_xml.Trim()=="")
                    {
                      MessageBox.Show("传出参数为空");return "0";
                    }


                    jssfyweb.PathologyService  sfysev=new jssfyweb.PathologyService();
                    sfysev.Url = pathWEB;

                    if (debug == "1")
                        log.WriteMyLog("传入参数XMl：" + Request_xml);
                  
                    string   Response_xml="";
                    try
                    {
                    Response_xml= sfysev.GetPatientInfo(Request_xml);
                    }
                    catch(Exception e1)
                    {
                        MessageBox.Show("提取信息异常：" + e1.Message);
                        log.WriteMyLog("提取信息异常：" + sfysev.Url + "\r\n" + e1.Message);
                        return "0";
                    }
                    if (debug == "1")
                        log.WriteMyLog("返回XMl：" + Response_xml);

                    if (Response_xml =="")
                    {
                        MessageBox.Show("返回XML数据为空，提取信息失败"); return "0";
                    }
                    //-------------------------------

                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(Response_xml);
                        xmlok = xd.SelectSingleNode("/Response");
                     }
                    catch(Exception e2) 
                    {
                        if (debug == "1")
                            log.WriteMyLog("XML解析错误:" + e2.Message);
                            MessageBox.Show("XML解析错误:"+e2.Message); 
                        return "0";
                    }
                    
                    //-返回xml----------------------------------------------------
                    PT_XML px = new PT_XML();
                    try
                    {

                            px.myDictionary["病人编号"] = xmlok["bl_no"].InnerText;

                            if (Sslbx.Contains("发票号"))
                                px.myDictionary["就诊ID"] = Ssbz.Trim();
                            else
                                px.myDictionary["就诊ID"] = "";
                        px.myDictionary["申请序号"] = "";

                        if (Sslbx.Contains("门诊号"))
                        {
                            px.myDictionary["门诊号"] = xmlok["patient_id"].InnerText;
                            px.myDictionary["住院号"] = "";
                        }
                        else
                        {
                            px.myDictionary["门诊号"] ="";
                            px.myDictionary["住院号"] = xmlok["inpatient_no"].InnerText;
                        }

                        px.myDictionary["姓名"] = xmlok["name"].InnerText;
                        px.myDictionary["年龄"] = xmlok["age"].InnerText+"岁";
                        px.myDictionary["性别"] = xmlok["sex"].InnerText;

                        if (px.myDictionary["性别"].Trim() == "1")
                            px.myDictionary["性别"] = "男";
                        if (px.myDictionary["性别"].Trim() == "2")
                            px.myDictionary["性别"] = "女";

                        px.myDictionary["婚姻"] = xmlok["marry"].InnerText;
                        if (px.myDictionary["婚姻"] == "1")
                            px.myDictionary["婚姻"] = "已婚";
                        if (px.myDictionary["婚姻"] == "2")
                            px.myDictionary["婚姻"] = "未婚";
                        if (px.myDictionary["婚姻"] == "3")
                            px.myDictionary["婚姻"] = "丧偶";
                        if (px.myDictionary["婚姻"] == "4")
                            px.myDictionary["婚姻"] = "离异";
                        if (px.myDictionary["婚姻"] == "5")
                            px.myDictionary["婚姻"] = "其他";


                        px.myDictionary["地址"] = xmlok["address"].InnerText;
                        px.myDictionary["电话"] = xmlok["mobile"].InnerText;
                        try
                        {
                            px.myDictionary["病区"] = xmlok["ward_name"].InnerText;
                            px.myDictionary["床号"] = xmlok["bed_no"].InnerText;
                        }
                        catch
                        {
                        }
                        
                        px.myDictionary["身份证号"] = xmlok["social_no"].InnerText;
                        px.myDictionary["民族"] = "";// xmlok["nation"].InnerText;
                        px.myDictionary["职业"] = xmlok["occupation"].InnerText;

                        if (Sslbx.Contains("门诊号"))
                        {
                            px.myDictionary["送检科室"] = xmlok["unit_name"].InnerText;
                        }
                        else
                            px.myDictionary["送检科室"] = "";
                       

                        px.myDictionary["送检医生"] = "";
                        px.myDictionary["收费"] ="";
                        px.myDictionary["标本名称"] = "";
                        px.myDictionary["送检医院"] = "本院";
                        px.myDictionary["医嘱项目"] = "";
                        px.myDictionary["备用1"] ="";
                        px.myDictionary["备用2"] ="";

                        try
                        {
                            px.myDictionary["费别"] = xmlok["response_type"].InnerText;
                        }
                        catch
                        {
                            try
                            {
                                px.myDictionary["费别"] = xmlok["responce_type"].InnerText;
                            }
                            catch
                            {
                            }
                        }
                        if (px.myDictionary["费别"].Trim() == "9")
                            px.myDictionary["费别"] = "医保";
                        if (px.myDictionary["费别"].Trim() == "1")
                            px.myDictionary["费别"] = "自费";

                        px.myDictionary["病人类别"] = xmlok["type"].InnerText;
                        if (px.myDictionary["病人类别"].Trim() == "1")
                            px.myDictionary["病人类别"] = "门诊";
                        if (px.myDictionary["病人类别"].Trim() == "2")
                            px.myDictionary["病人类别"] = "住院";

                        px.myDictionary["临床病史"] = xmlok["medical_record"].InnerText;
                        px.myDictionary["临床诊断"] = xmlok["diag_name"].InnerText;


                        if (px.myDictionary["姓名"].Trim() == "")
                        {
                            MessageBox.Show("未查询到相关记录数据"); return "0";
                        }

                        string exep = "";
                        return px.rtn_XML(ref exep);
                    }
                    catch (Exception ee4)
                    {
                      
                        MessageBox.Show(ee4.Message); 
                        if (Debug == "1")
                        log.WriteMyLog("生成xml异常："+ee4.Message);
                        return "0";
                    }
                }
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    return "0";
                }
                }
            else
            {
                MessageBox.Show("Sslbx不能为空" );
                return "0";
            }

             }
      }
}

