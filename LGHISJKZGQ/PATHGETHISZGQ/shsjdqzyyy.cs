

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 上海市嘉定区中医医院  webservices  --复高
    /// </summary>
    class shsjdqzyyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
      
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            string exp = "";
            string pathWEB = f.ReadString(Sslbx, "URL", ""); //获取sz.ini中设置的webservicesurl

            if (Sslbx != "")
            {
                shsjdzyyyweb.FGPLService jdzyyy = new LGHISJKZGQ.shsjdzyyyweb.FGPLService();
                if (pathWEB.Trim() != "")
                    jdzyyy.Url = pathWEB;

              
                 if (Sslbx == "门诊申请单号"||Sslbx == "住院申请单号" || Sslbx == "住院号(申请)" || Sslbx == "门诊号(申请)")
                {
                    string GetApplyBill_XML = "";
                    if (Sslbx == "门诊申请单号")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo></OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo></InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId>"+Ssbz.Trim()+"</RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>O</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                     if (Sslbx == "住院申请单号")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo></OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo></InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId>"+Ssbz.Trim()+"</RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>I</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                    if (Sslbx == "住院号(申请)")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo></OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo>"+Ssbz.Trim()+"</InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId></RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>I</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                    if (Sslbx == "门诊号(申请)")
                    {
                        GetApplyBill_XML = "<PatientInfo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<OutPatientNo>"+Ssbz.Trim()+"</OutPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<InPatientNo></InPatientNo>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RegCode></RegCode>";
                        GetApplyBill_XML = GetApplyBill_XML + "<RequestId></RequestId>";
                        GetApplyBill_XML = GetApplyBill_XML + "<EncounterType>O</EncounterType>";
                        GetApplyBill_XML = GetApplyBill_XML + "</PatientInfo>";
                    }
                    if (GetApplyBill_XML.Trim()=="")
                    {
                        return "0";
                    }
                  //  shsjdqzyyyweb.FGPACSService jdzyyy = new PATHGETHISZGQ.shsjdqzyyyweb.FGPACSService();
                    string GetApplyBill_RTN = "";
                    try
                    {
                        GetApplyBill_RTN = jdzyyy.GetApplyBill(GetApplyBill_XML);
                    }
                    catch
                    {
                        MessageBox.Show("调用GetApplyBill方法异常，请检查网络是否正常!");
                        return "0";
                    }

                   if (GetApplyBill_RTN.Trim() == "")
                    {
                        MessageBox.Show("取信息异常，返回消息为空");
                        return "0";
                    }
                 
                    //try
                    //{
                   
                        XmlNodeList xmlok_DATAs = null;
                        XmlDocument xd2 = new XmlDocument();

                        DataTable dt = new DataTable();
                        DataColumn dc0 = new DataColumn("序号");
                        dt.Columns.Add(dc0);
                        DataColumn dc1 = new DataColumn("门诊号");
                        dt.Columns.Add(dc1);
                        DataColumn dc3 = new DataColumn("住院号");
                        dt.Columns.Add(dc3);
                        DataColumn dc4 = new DataColumn("就诊类型");
                        dt.Columns.Add(dc4);
                        DataColumn dc5 = new DataColumn("患者类型");
                        dt.Columns.Add(dc5);
                        DataColumn dc6 = new DataColumn("姓名");
                        dt.Columns.Add(dc6);
                        DataColumn dc7 = new DataColumn("性别");
                        dt.Columns.Add(dc7);
                        DataColumn dc8 = new DataColumn("年龄");
                        dt.Columns.Add(dc8);
                        DataColumn dc9 = new DataColumn("出生日期");
                        dt.Columns.Add(dc9);
                        DataColumn dc10 = new DataColumn("身份证号");
                        dt.Columns.Add(dc10);
                        DataColumn dc11 = new DataColumn("联系电话");
                        dt.Columns.Add(dc11);
                        DataColumn dc12 = new DataColumn("挂号编号");
                        dt.Columns.Add(dc12);
                        DataColumn dc13 = new DataColumn("病区名称");
                        dt.Columns.Add(dc13);
                        DataColumn dc14 = new DataColumn("床位号");
                        dt.Columns.Add(dc14);

                        DataColumn dc15 = new DataColumn("申请单编号");
                        dt.Columns.Add(dc15);
                        DataColumn dc16 = new DataColumn("申请科室名称");
                        dt.Columns.Add(dc16);
                        DataColumn dc17 = new DataColumn("申请医生姓名");
                        dt.Columns.Add(dc17);
                        DataColumn dc18 = new DataColumn("申请单状态");
                        dt.Columns.Add(dc18);
                        DataColumn dc19 = new DataColumn("病史摘要");
                        dt.Columns.Add(dc19);
                        DataColumn dc20 = new DataColumn("临床诊断");
                        dt.Columns.Add(dc20);
                        DataColumn dc21 = new DataColumn("项目编码");
                        dt.Columns.Add(dc21);
                        DataColumn dc22 = new DataColumn("项目名称");
                        dt.Columns.Add(dc22);
                        DataColumn dc23 = new DataColumn("检查部位名称");
                        dt.Columns.Add(dc23);
                        DataColumn dc24 = new DataColumn("开单时间");
                        dt.Columns.Add(dc24);


                        if (Debug == "1")
                            log.WriteMyLog(GetApplyBill_RTN);
                        try
                        {
                            xd2.LoadXml(GetApplyBill_RTN);
                            xmlok_DATAs = xd2.SelectNodes("/ApplyBillInfo/PatientInfos");
                            if (xmlok_DATAs.Count <= 0)
                            {
                                MessageBox.Show("未找到对应的记录"); return "0";
                            }

                            foreach (XmlNode xmlok_DATA2 in xmlok_DATAs)
                            {
                                XmlNode xmlok_DATA = xmlok_DATA2.FirstChild;
                                //申请单信息
                                XmlNodeList xmlok_ApplyBills = xmlok_DATA["ApplyBills"].ChildNodes;

                                foreach (XmlNode xmlok_ApplyBill in xmlok_ApplyBills)
                                {

                                    DataRow dr = dt.NewRow();

                                    //申请单编号
                                    dr["申请单编号"] = xmlok_ApplyBill["RequestId"].InnerText;
                                    //申请科室名称
                                    dr["申请科室名称"] = xmlok_ApplyBill["ReqLocationName"].InnerText;
                                    //申请医生姓名
                                    dr["申请医生姓名"] = xmlok_ApplyBill["ReqDoctorName"].InnerText;
                                    //申请单状态
                                    dr["申请单状态"] = xmlok_ApplyBill["RequestStatus"].InnerText;
                                    //开单时间
                                    dr["开单时间"] = xmlok_ApplyBill["RequestDate"].InnerText;
                                    //病史摘要
                                    dr["病史摘要"] = xmlok_ApplyBill["MedicalHistory"].InnerText;
                                    //临床诊断
                                    dr["临床诊断"] = xmlok_ApplyBill["ClinicDiagnosis"].InnerText;

                                    string jcbb = "";
                                    string xmbm = "";
                                    string xmmc = "";
                                    XmlNodeList xmlok_ApplyItems = xmlok_ApplyBill["ApplyItems"].ChildNodes;
                                    //   MessageBox.Show(xmlok_ApplyItems.Count.ToString());
                                    foreach (XmlNode xmlok_ApplyItem in xmlok_ApplyItems)
                                    {
                                        //      MessageBox.Show(xmlok_ApplyItem["ItemCode"].InnerText);
                                        //项目编码 
                                        xmbm = xmlok_ApplyItem["ItemCode"].InnerText;
                                        //项目名称
                                        xmmc = xmlok_ApplyItem["ItemName"].InnerText;
                                        //检查部位名称
                                        jcbb = jcbb + xmlok_ApplyItem["CheckPointName"].InnerText + ",";
                                    }

                                    //项目编码
                                    dr["项目编码"] = xmbm;
                                    //项目名称
                                    dr["项目名称"] = xmmc;
                                    //检查部位名称
                                    dr["检查部位名称"] = jcbb;


                                    //门诊号
                                    dr["门诊号"] = xmlok_DATA["OutPatientNo"].InnerText;
                                    //住院号
                                    dr["住院号"] = xmlok_DATA["InPatientNo"].InnerText;
                                    //就诊类型
                                    string  jzlx=xmlok_DATA["EncounterType"].InnerText;
                                    if(jzlx=="O") jzlx="门诊";
                                    else if(jzlx=="I") jzlx="住院";
                                    else  jzlx="";


                                    dr["就诊类型"] =jzlx ;
                                    //患者类型
                                    string hzlx= xmlok_DATA["PatientType"].InnerText;
                                    if(hzlx=="99") hzlx="全额记帐";
                                    else if(hzlx=="40") hzlx="新农合"; 
                                      else if(hzlx=="41") hzlx="大病直补"; 
                                      else if(hzlx=="42") hzlx="重大疾病"; 
                                      else if(hzlx=="43") hzlx="新农合自费"; 
                                      else if(hzlx=="10") hzlx="普通病人";
                                     else if(hzlx=="30") hzlx="市医保"; 
                                    else  hzlx="";
                                    dr["患者类型"] =hzlx;
                                    //姓名
                                    dr["姓名"] = xmlok_DATA["PatientName"].InnerText;
                                    //性别
                                    string xb= xmlok_DATA["GenderCode"].InnerText;
                                      if(xb=="F") xb="男";
                                       else if(xb=="M") xb="女";
                                      else  if (xb == "0") xb = "女";
                                      else if (xb == "1") xb = "男";
                                      else xb = "";
                                    dr["性别"] =xb;

                                    //年龄 
                                    dr["年龄"] = xmlok_DATA["Age"].InnerText;
                                    //身份证号
                                    dr["身份证号"] = xmlok_DATA["SSNumber"].InnerText;
                                    //联系电话
                                    dr["联系电话"] = xmlok_DATA["RelationshipTel"].InnerText;
                                    //出生日期
                                    dr["出生日期"] = xmlok_DATA["DateOfBirth"].InnerText;
                                    //挂号编号  0-1X  15  门诊病人必须填写
                                    dr["挂号编号"] = xmlok_DATA["RegCode"].InnerText;
                                    //就诊科室名称
                                    //dr["就诊科室名称"] = xmlok_DATA["LocationName"].InnerText;
                                    //病区名称
                                    dr["病区名称"] = xmlok_DATA["WardName"].InnerText;
                                    //诊断名称
                                    // dr["诊断名称"] = xmlok_DATA["DiagnosisName"].InnerText;
                                    //床位号
                                    dr["床位号"] = xmlok_DATA["BedNo"].InnerText;
                                    //医生姓名
                                    //dr["医生姓名"] = xmlok_DATA["DoctorName"].InnerText;
                                    dt.Rows.Add(dr);

                                }
                            }


                            for (int x = 0; x < dt.Rows.Count; x++)
                            {
                                dt.Rows[x][0] = x;
                            }

                        }
                        catch 
                        {
                            MessageBox.Show("查询数据：未找到对应的记录!\r\n" + GetApplyBill_RTN);
                            log.WriteMyLog("未找到对应的记录,解析DATA异常：" + GetApplyBill_RTN);
                            
                            return "0";
                            //try
                            //{
                            //XmlNode xmlok_DATA_Info = null;
                            //XmlDocument xd_Info = new XmlDocument();
                            //xd_Info.LoadXml(GetApplyBill_RTN);
                            //xmlok_DATA_Info = xd_Info.SelectSingleNode("/ApplyBillInfo");
                            //if (xmlok_DATA_Info["PatientInfos"].InnerText != "")
                            //    MessageBox.Show("提取信息错误：" + xmlok_DATA_Info["Info"].InnerText);
                            //return "0";
                            //}
                            //catch
                            //{
                            //MessageBox.Show("解析DATA异常：" + xmlok_e.ToString());
                            //return "0";
                            //}
                        }
                        DataTable dt_brxx = new DataTable();  dt_brxx = dt;
                        int count = 0;
                        if (dt.Rows.Count > 1)
                        {
                           
                            FRM_SP_SELECT yc = new FRM_SP_SELECT(dt, -1, "序号,申请单编号,姓名,住院号,门诊号,挂号编号,就诊类型,病区名称,床位号,申请科室名称,申请医生姓名,开单时间,临床诊断,项目名称");
                            yc.ShowDialog();
                            string rtn2 = yc.F_STRING[0];
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请单！");
                                return "0";
                            }
                            try
                            {
                                count = int.Parse(rtn2);
                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请单！");
                                return "0";
                            }
                        }
                        else
                        {
                           count = 0;
                        }
                  
                    //-返回xml----------------------------------------------------
                    try
                    {

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + dt_brxx.Rows[count]["挂号编号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + dt_brxx.Rows[count]["申请单编号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "门诊号=" + (char)34 + dt_brxx.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + dt_brxx.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + dt_brxx.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";

                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "性别=" + (char)34 + dt_brxx.Rows[count]["性别"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            if (dt_brxx.Rows[count]["年龄"].ToString().Trim() == "")
                            {
                             
                                string CSRQ = dt_brxx.Rows[count]["出生日期"].ToString().Trim();

                                string datatime = DateTime.Today.Date.ToString();

                                if (CSRQ != "")
                                {
                                    if (CSRQ.Contains("-"))
                                        CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                                    int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                    int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                    int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                                    if (Year >= 1)
                                    {
                                        //xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                        if (Month > 0)
                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                        if (Month < 0)
                                            xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                        if (Month == 0)
                                        {
                                            if (day >= 0)
                                                xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                            else
                                                xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                        }
                                    }
                                    else
                                        if (Year == 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                            int m = day1 / 30;
                                            int d = day1 % 30;
                                            xml = xml + "年龄=" + (char)34 + m + "月" + d + "天" + (char)34 + " ";
                                        }
                                }
                            }
                            else
                                xml = xml + "年龄=" + (char)34 + dt_brxx.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                      
                        }
                        catch (Exception ee)
                        {
                          
                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + dt_brxx.Rows[count]["联系电话"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + dt_brxx.Rows[count]["病区名称"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + dt_brxx.Rows[count]["床位号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + dt_brxx.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + dt_brxx.Rows[count]["申请科室名称"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + dt_brxx.Rows[count]["申请医生姓名"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            string bbmc = dt_brxx.Rows[count]["检查部位名称"].ToString().Trim();
                            xml = xml + "标本名称=" + (char)34 + bbmc.Remove(bbmc.Length-1) + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "标本名称=" + (char)34 +"" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "医嘱项目=" + (char)34 + dt_brxx.Rows[count]["项目编码"].ToString().Trim() + "^" + dt_brxx.Rows[count]["项目名称"].ToString().Trim() + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "医嘱项目=" + (char)34 +"" + (char)34 + " ";
                       
                        }
                            //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "费别=" + (char)34 + dt_brxx.Rows[count]["患者类型"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病人类别=" + (char)34 + dt_brxx.Rows[count]["就诊类型"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + dt_brxx.Rows[count]["病史摘要"].ToString().Trim() + "]]></临床病史>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + dt_brxx.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        xml = xml + "</LOGENE>";
                     
                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;
                    }
                    catch (Exception e)
                    {
                        if (Debug == "1")
                            MessageBox.Show("提取信息出错，请重新操作");
                        log.WriteMyLog("xml解析错误---" + e.ToString());
                        return "0";
                    }
                 }





                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                 if (Sslbx == "门诊号" || Sslbx == "住院号")
                 {
                     string zyh = "";
                     string mzh = "";
                     string brlb = "";
                     if (Sslbx == "门诊号")
                     {
                         mzh = Ssbz;
                         brlb = "O";
                     }
                     if (Sslbx == "住院号")
                     {
                         zyh = Ssbz;
                         brlb = "I";
                     }

                     string GetPatientInfo_XML = "<PatientInfo><OutPatientNo>" + mzh + "</OutPatientNo><InPatientNo>" + zyh + "</InPatientNo><EncounterType>" + brlb + "</EncounterType>";
                        GetPatientInfo_XML=GetPatientInfo_XML+"<RegDate>"+DateTime.Now.AddDays(-7).ToString("yyyyMMddHHmmss")+"</RegDate><InDeptCode></InDeptCode><WardCode></WardCode></PatientInfo>";

                       
                        shsjdqzyyyweb2.FGPublicService jdqzyyy = new LGHISJKZGQ.shsjdqzyyyweb2.FGPublicService();
                    if (pathWEB.Trim() != "")
                        jdqzyyy.Url = pathWEB;


                    try
                    {
                        string GetPatientInfo_RTN = jdqzyyy.GetPatientInfo(GetPatientInfo_XML);
                        if (Debug == "1")
                            log.WriteMyLog(GetPatientInfo_RTN);


                        XmlDocument xd2 = new XmlDocument();
                        XmlNode xmlok_DATA = null;
                        try
                        {
                            xd2.LoadXml(GetPatientInfo_RTN);
                            xmlok_DATA = xd2.SelectSingleNode("/PatientInfos");
                        }
                        catch (Exception xmlok_e)
                        {
                            MessageBox.Show("解析DATA异常：" + xmlok_e.ToString());
                            return xmlstr();
                        }
                        if (xmlok_DATA.InnerXml.Trim() == "")
                        {
                            MessageBox.Show("未找到对应的记录！");
                            return xmlstr();
                        }

                        DataSet ds = new DataSet();
                        try
                        {
                            StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                            XmlReader xr = new XmlTextReader(sr);
                            ds.ReadXml(xr);
                        }
                        catch (Exception eee)
                        {
                            if (Debug == "1")
                                MessageBox.Show("转dataset异常：" + eee.ToString());
                            log.WriteMyLog("转dataset异常:" + eee);
                            return xmlstr();
                        }
                        int count = 0;
                        string isdtcx = f.ReadString(Sslbx, "isdtcx", "0"); //获取sz.ini中设置的多条是否显示选择窗体

                        if (isdtcx == "1")
                        {
                            if (ds.Tables[0].Rows.Count > 1)
                            {
                                DataColumn dc0 = new DataColumn("序号");
                                ds.Tables[0].Columns.Add(dc0);

                                for (int x = 0; x < ds.Tables[0].Rows.Count; x++)
                                {
                                    ds.Tables[0].Rows[x][ds.Tables[0].Columns.Count - 1] = x;
                                }


                                FRM_YZ_SELECT yc = new FRM_YZ_SELECT(ds.Tables[0], "序号,RegCode,PatientName,LocationName,DoctorName,EncounterDate,WardName,BedNo,DiagnosisName", "序号,挂号编号,姓名,就诊科室,医生名称,就诊时间,病区名称,床位号,临床诊断", "");
                                yc.ShowDialog();
                                string rtn2 = yc.F_XH;
                                if (rtn2.Trim() == "")
                                {
                                    MessageBox.Show("未选择申请单！");
                                    return "0";
                                }
                                try
                                {
                                    count = int.Parse(rtn2);
                                }
                                catch
                                {
                                    MessageBox.Show("请重新选择申请单！");
                                    return "0";
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        try
                        {
                            xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[count]["RegCode"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[count]["OutPatientNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "住院号=" + (char)34 + ds.Tables[0].Rows[count]["InPatientNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[count]["PatientName"].ToString().Trim() + (char)34 + " ";

                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            //性别
                            string xb = ds.Tables[0].Rows[count]["GenderCode"].ToString().Trim();
                            if (xb == "F") xb = "男";
                            else if (xb == "M") xb = "女";
                            else
                                if (xb == "0") xb = "女";
                                else if (xb == "1") xb = "男";
                                else  xb = "";
                            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            if (ds.Tables[0].Rows[count]["Age"].ToString().Trim() == "")
                            {
                               
                                string CSRQ = ds.Tables[0].Rows[count]["DateOfBirth"].ToString().Trim();

                                string datatime = DateTime.Today.Date.ToString();

                                if (CSRQ != "")
                                {
                                    if (CSRQ.Contains("-"))
                                        CSRQ = DateTime.Parse(CSRQ).ToString("yyyyMMdd");
                                    int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                    int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                    int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;

                                    if (Year >= 1)
                                    {
                                        //xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                        if (Month > 0)
                                            xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                        if (Month < 0)
                                            xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                        if (Month == 0)
                                        {
                                            if (day >= 0)
                                                xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                            else
                                                xml = xml + "年龄=" + (char)34 + (Year - 1) + "岁" + (char)34 + " ";
                                        }
                                    }
                                    else
                                        if (Year == 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                            int m = day1 / 30;
                                            int d = day1 % 30;
                                            xml = xml + "年龄=" + (char)34 + m + "月" + d + "天" + (char)34 + " ";
                                        }
                                }
                            }
                            else
                            {
                                string nl = ds.Tables[0].Rows[count]["Age"].ToString().Trim();
                                if (nl.Contains("岁") || nl.Contains("月") || nl.Contains("天"))
                                    xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                                else
                                    xml = xml + "年龄=" + (char)34 + nl + "岁" + (char)34 + " ";
                            }
                        }
                        catch (Exception ee)
                        {

                            exp = exp + ee.ToString();
                            xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[count]["RelationshipTel"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[count]["WardName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[count]["BedNo"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[count]["SSNumber"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[count]["LocationName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------

                        try
                        {
                            xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[count]["DoctorName"].ToString().Trim() + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {

                            xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        }
                        catch
                        {
                            xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";

                        }
                        //----------------------------------------------------------
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        //----------------------------------------------------------
                        try
                        {
                            string hzlx = ds.Tables[0].Rows[count]["PatientType"].ToString().Trim();
                            if (hzlx == "99") hzlx = "全额记帐";
                            else if (hzlx == "40") hzlx = "新农合";
                            else if (hzlx == "41") hzlx = "大病直补";
                            else if (hzlx == "42") hzlx = "重大疾病";
                            else if (hzlx == "43") hzlx = "新农合自费";
                            else if (hzlx == "10") hzlx = "普通病人";
                            else if (hzlx == "30") hzlx = "市医保";
                            else hzlx = "";

                            xml = xml + "费别=" + (char)34 + hzlx + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        }
                        //----------------------------------------------------------
                        try
                        {

                            //就诊类型
                            string jzlx = ds.Tables[0].Rows[count]["EncounterType"].ToString().Trim();
                            if (jzlx == "O") jzlx = "门诊";
                            else if (jzlx == "I") jzlx = "住院";
                            else jzlx = "";
                            xml = xml + "病人类别=" + (char)34 + jzlx + (char)34 + " ";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "/>";
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        }
                        //----------------------------------------------------------
                        try
                        {
                            xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[count]["DiagnosisName"].ToString().Trim() + "]]></临床诊断>";
                        }
                        catch (Exception ee)
                        {
                            exp = exp + ee.ToString();
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        }
                        xml = xml + "</LOGENE>";

                        if (Debug == "1" && exp.Trim() != "")
                            log.WriteMyLog(exp.Trim());

                        return xml;
                    }
                    catch(Exception  eee)
                    {
                        MessageBox.Show("执行查询异常："+eee.ToString());
                    }
                 }
                 else
                 {
                     MessageBox.Show("无此" + Sslbx);
                     if (Debug == "1")
                         log.WriteMyLog(Sslbx + Ssbz + "不存在！");

                     return "0";

                 }
            } return "0";


        }
        public static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + (char)34 + " ";
            xml = xml + "门诊号=" + (char)34 + (char)34 + " ";
            xml = xml + "住院号=" + (char)34 + (char)34 + " ";
            xml = xml + "姓名=" + (char)34 + (char)34 + " ";

            xml = xml + "性别=" + (char)34 + (char)34 + " ";

            xml = xml + "年龄=" + (char)34 + (char)34 + " ";
            xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + (char)34 + "   ";
            xml = xml + "电话=" + (char)34 + (char)34 + " ";
            xml = xml + "病区=" + (char)34 + (char)34 + " ";
            xml = xml + "床号=" + (char)34 + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + (char)34 + " ";
            //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
            //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 +"本院"+ (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + (char)34 + " ";

            xml = xml + "病人类别=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + "]]></临床诊断>";
            xml = xml + "</LOGENE>";
            return xml;
        }
    }
}
