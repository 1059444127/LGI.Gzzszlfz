using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.Data;
using HL7;
using System.Xml;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    //安医大二附院 集成平台  webservice+hl7，取基本信息
    class AYD2FY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            if (Sslbx.Trim() == "")
            {
                MessageBox.Show("无此" + Sslbx);
                return "0";
            }

            #region 获取SZ或T_SZ配置


            string xtdm = f.ReadString(Sslbx, "xtdm", "2060000").Replace("\0", "").Trim();
            string WebUrl = f.ReadString(Sslbx, "WebUrl", "http://223.220.200.45:1506/services/WSInterface?wsdl").Replace("\0", "").Trim();
            debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim();
         
            string certificate = f.ReadString(Sslbx, "certificate", "ZmmJ9RMCKAUxFsiwl/08iiA3J17G0OpI").Replace("\0", "").Trim();
            #endregion

   

            #region 提取基本信息
            string OSQ = "";
            if ( Sslbx == "卡号")
            {

                if (Ssbz.Length > 19)
                {
                    try
                    {
                        Ssbz = Ssbz.Substring(9, 10);
                    }
                    catch
                    {
                        log.WriteMyLog("截取卡号异常Substring(9, 10)：" + Ssbz);
                    }
                }
                if (debug == "1")
                    log.WriteMyLog("卡号："+Ssbz);

                string rtn = SP_SELECT.ptxml2(Sslbx, Ssbz, debug, "");
                if (rtn != "0")
                    return rtn;

                if (debug == "1")
                    log.WriteMyLog("未查询到申请单记录，获取基本信息");

                OSQ = "<root><patientId></patientId><visitNo>" + Ssbz + "</visitNo></root>";

            }else
            if (Sslbx == "门诊号")
            {

                string rtn = SP_SELECT.ptxml2(Sslbx, Ssbz, debug,"");
                if (rtn != "0")
                    return rtn;

                if (debug == "1")
                    log.WriteMyLog("未查询到申请单记录，获取基本信息");
              
                OSQ = "<root><patientId></patientId><visitNo>"+Ssbz+"</visitNo></root>";

            }
            else
                if (Sslbx == "住院号")
                {



                    string rtn = SP_SELECT.ptxml2(Sslbx, Ssbz, debug,"");
                   if (rtn != "0")
                       return rtn;

                    if(debug=="1")
                   log.WriteMyLog("未查询到申请单记录，获取基本信息");

                    OSQ = "<root><patientId></patientId><visitNo>"+Ssbz+"</visitNo></root>";
                }
                    else
                    {
                        MessageBox.Show("无此识别类型" + Sslbx);
                        return "0";
                    }

              aydefyweb.WSInterface ayd2yy = new LGHISJKZGQ.aydefyweb.WSInterface();
              if (WebUrl.Trim() != "")
                  ayd2yy.Url = WebUrl;

              string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>GetPatientBaseInfoIndex</serverName>"
              + "<format>xml</format><callOperator></callOperator><certificate>" + certificate + "</certificate></root>";

            if (debug == "1")
                log.WriteMyLog("入参：msgHeader:" + msgHeader + "\r\nmsgBody" + OSQ);


            try
            {
              
                string rtn="";
                try
                {
                   
                    rtn = ayd2yy.CallInterface(msgHeader, OSQ);
                }
                catch(Exception e1)
                {
                    MessageBox.Show("连接webservice异常：" + e1.Message);
                    return "0";
                }
                if (debug == "1")
                    log.WriteMyLog("返回：" + rtn);

                if(rtn=="<?xml version=\"1.0\" encoding=\"utf-8\"?><root/>")
                {   MessageBox.Show("未查询到病人信息");
                    return "0";
                }

                 PT_XML px = new PT_XML();

                
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    try
                    {
                        xd.LoadXml(rtn);
                        xmlok = xd.SelectSingleNode("/root/patients/patient");
                     }
                catch
                {
                    MessageBox.Show("未查询到病人信息");
                       return "0";
                }


                       px.myDictionary["姓名"] = xmlok["patientName"].InnerText;
                        string xb = xmlok["patientSex"].InnerText;
                        if (xb == "1") xb = "女";
                        else if (xb.Trim() == "0") xb = "男";
                        else xb = "";
                        px.myDictionary["性别"] = xb;

                        if (Sslbx.Trim() == "住院号")
                        {   px.myDictionary["病人类别"] = "住院";
                             px.myDictionary["住院号"] =  xmlok["visitNo"].InnerText;
                             try
                             {
                                 px.myDictionary["送检科室"] = xmlok["admissionDept"].InnerText;
                             }
                             catch
                             {
                             }
                             px.myDictionary["就诊ID"] = xmlok["visitNum"].InnerText;
                        }
                            else
                        {
                                px.myDictionary["病人类别"] = "门诊";
                               px.myDictionary["门诊号"] =  xmlok["visitNo"].InnerText;
                               try
                               {
                                   px.myDictionary["送检科室"] = xmlok["cureDept"].InnerText;
                               }
                               catch
                               {
                               }
                               if (Sslbx.Trim() == "卡号")
                                   px.myDictionary["就诊ID"] = Ssbz;
                        }
                        
                         
                        px.myDictionary["年龄"] =ZGQClass.CsrqToAge(xmlok["patientBirthdate"].InnerText);   
                        px.myDictionary["地址"] = xmlok["commPostCode"].InnerText;
                        px.myDictionary["电话"] =xmlok["telephone"].InnerText;
                        px.myDictionary["病人编号"] = xmlok["patientId"].InnerText;
                    
                        px.myDictionary["申请序号"] = "";
                        px.myDictionary["送检医生"] = "";
                        px.myDictionary["费别"] ="";
                        px.myDictionary["临床诊断"] = "";
                        px.myDictionary["民族"] = "";
                        px.myDictionary["身份证号"] =  xmlok["identityNo"].InnerText;
                        
                        string exep = "";
                        return px.rtn_XML(ref exep);

                   
                }
                catch (Exception ee)
                {
                    MessageBox.Show( ee.Message);
                  
                    return "0";
                }
         
            #endregion

        }


        public static string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string WebUrl, string debug, string certificate)
        {

          
         

            //集成平台地址

           
            aydefyweb.WSInterface ayd2yy = new LGHISJKZGQ.aydefyweb.WSInterface();


            if (WebUrl.Trim() != "")
                ayd2yy.Url = WebUrl;

            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";


            if (debug == "1")
                log.WriteMyLog("入参：msgHeader:" + msgHeader + "\r\nmsgBody" + msgBody);


            try
            {
                MessageBox.Show(ayd2yy.Url);
                string rtn = ayd2yy.CallInterface(msgHeader, msgBody);

                if (debug == "1")
                    log.WriteMyLog("返回：" + rtn);
                return rtn;
            }
            catch (Exception ee)
            {
                MessageBox.Show("连接webservice异常：" + ee.Message);
                return "-1";
            }
        }

        //private static string jxhl7(string rtn_msg, string Ssbz, string Sslbx, string isbrxx, string WebUrl, string debug)
        //{

        //    int xh = 0;
        //    PT_XML px = new PT_XML();
        //    readhl7 r7 = new readhl7();
        //    int count = 0;
        //    try
        //    {
        //        r7.Adt01(rtn_msg, ref count);

        //        if (r7.MSA[1].Trim() == "AA")
        //        {
        //            try
        //            {

        //                if (count >= 1)
        //                {
        //                    DataTable dt = new DataTable();
        //                    DataColumn dc0 = new DataColumn("序号");
        //                    dt.Columns.Add(dc0);
        //                    DataColumn dc1 = new DataColumn("申请序号");
        //                    dt.Columns.Add(dc1);
        //                    DataColumn dc2 = new DataColumn("姓名");
        //                    dt.Columns.Add(dc2);
        //                    DataColumn dc3 = new DataColumn("病人类别");
        //                    dt.Columns.Add(dc3);

        //                    DataColumn dc4 = new DataColumn("送检科室");
        //                    dt.Columns.Add(dc4);
        //                    DataColumn dc5 = new DataColumn("送检医生");
        //                    dt.Columns.Add(dc5);

        //                    DataColumn dc6 = new DataColumn("医嘱项目");
        //                    dt.Columns.Add(dc6);
        //                    DataColumn dc7 = new DataColumn("费用");
        //                    dt.Columns.Add(dc7);
        //                    DataColumn dc8 = new DataColumn("标本名称");
        //                    dt.Columns.Add(dc8);


        //                    for (int x = 0; x < count; x++)
        //                    {
        //                        DataRow dr1 = dt.NewRow();
        //                        dt.Rows.Add(dr1);
        //                        dt.Rows[x][0] = x;

        //                        //申请序号
        //                        dt.Rows[x][1] = r7.ORC[x, 2].Trim();
        //                        //姓名
        //                        dt.Rows[x][2] = r7.PID[5].Split('^')[0].Trim();
        //                        //病人类型
        //                        dt.Rows[x][3] = r7.QRF[4].Trim();
        //                        if (dt.Rows[x][3].ToString() == "I")
        //                            dt.Rows[x][3] = "住院";
        //                        else
        //                            dt.Rows[x][3] = "门诊";

        //                        //科室
        //                        dt.Rows[x][4] = r7.ORC[xh, 13].Trim().Split('^')[0];
        //                        //医生
        //                        dt.Rows[x][5] = r7.ORC[x, 12].Trim();
        //                        //项目
        //                        dt.Rows[x][6] = r7.OBR[x, 4].Trim();
        //                        //费用
        //                        dt.Rows[x][7] = r7.OBR[x, 23].Trim();
        //                        //标本名称
        //                        string bbmc = r7.OBR[x, 15].Trim();
        //                        if (bbmc.Trim() == "^") bbmc = "";
        //                        dt.Rows[x][8] = bbmc;

        //                    }
        //                    Frm_FJSFYBJY ffj = new Frm_FJSFYBJY(dt, "30^80^80^40^120^100^250^70^150");
        //                    ffj.ShowDialog();
        //                    if (ffj.xh == "")
        //                    {
        //                        MessageBox.Show("未选择病人检查记录");
        //                        return "0";
        //                    }
        //                    xh = int.Parse(ffj.xh);

        //                }

        //                px.myDictionary["姓名"] = r7.PID[5].Split('^')[0].Trim();

        //                string xb = r7.PID[8].Trim();
        //                if (xb == "F") xb = "女";
        //                else if (xb.Trim() == "M") xb = "男";
        //                else xb = "女";
        //                px.myDictionary["性别"] = xb;

        //                string brlb = r7.QRF[4].Trim();
        //                if (brlb == "I") brlb = "住院";
        //                else brlb = "门诊";

        //                px.myDictionary["病人类别"] = brlb;

        //                px.myDictionary["出生日期"] = r7.PID[7].Trim();
        //                if (r7.PID[13].Trim() != "")
        //                    px.myDictionary["地址"] = r7.PID[13].Trim();
        //                if (r7.PID[11].Trim() != "")
        //                    px.myDictionary["电话"] = "^" + r7.PID[11].Trim();

        //                string hy = r7.PID[15].Trim();
        //                switch (hy)
        //                {
        //                    case "D": hy = "离婚"; break;
        //                    case "M": hy = "已婚"; break;
        //                    case "W": hy = "丧偶"; break;
        //                    case "V": hy = "未婚"; break;
        //                    default: hy = ""; break;

        //                }
        //                px.myDictionary["婚姻"] = hy;




        //                foreach (string pid3 in r7.PID[3].Trim().Split('~'))
        //                {
        //                    try
        //                    {
        //                        //身份证号
        //                        if (pid3.Split('^')[4] == "PN")
        //                            px.myDictionary["身份证号"] = pid3.Split('^')[0].Trim();

        //                    }
        //                    catch
        //                    {
        //                    }
        //                    try
        //                    {
        //                        //PI  patientid
        //                        if (pid3.Split('^')[4] == "PI")
        //                            px.myDictionary["病人编号"] = pid3.Split('^')[0].Trim();
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    try
        //                    {
        //                        //病案号
        //                        if (pid3.Split('^')[4] == "VN")
        //                            px.myDictionary["民族"] = pid3.Split('^')[0].Trim();
        //                    }
        //                    catch
        //                    {
        //                    } try
        //                    {

        //                        //医保号
        //                        if (pid3.Split('^')[4] == "VN")
        //                            px.myDictionary["就诊ID"] = pid3.Split('^')[0].Trim();
        //                    }
        //                    catch
        //                    {
        //                    }

        //                }


        //                if (brlb == "门诊")
        //                    px.myDictionary["门诊号"] = px.myDictionary["民族"];
        //                if (brlb == "住院")
        //                    px.myDictionary["住院号"] = px.myDictionary["民族"];


        //                px.myDictionary["床号"] = r7.NTE[3].Trim();

        //                px.myDictionary["送检科室"] = r7.ORC[xh, 13].Trim().Split('^')[0];

        //                px.myDictionary["病区"] = r7.ORC[xh, 13].Trim().Split('^')[0];


        //                px.myDictionary["申请序号"] = r7.ORC[xh, 2].Trim();
        //                px.myDictionary["送检医生"] = r7.ORC[xh, 12].Trim().Split('^')[1].Trim();

        //                string sqx = r7.OBR[0, 2].Trim();

        //                px.myDictionary["医嘱项目"] = r7.OBR[xh, 4].Trim();


        //                string bbmc1 = r7.OBR[xh, 15].Trim();
        //                if (bbmc1.Trim() == "^") bbmc1 = "";
        //                px.myDictionary["标本名称"] = bbmc1;

        //                px.myDictionary["收费"] = r7.OBR[xh, 23].Trim();
        //                //try
        //                //{
        //                //    px.myDictionary["临床病史"] = r7.OBR[xh, 46].Trim().Split('~')[1].Replace("2^", "").Trim();
        //                //}
        //                //catch
        //                //{
        //                //    px.myDictionary["临床病史"] = "";
        //                //}

        //                foreach (string lczd in r7.OBR[xh, 46].Trim().Split('~'))
        //                {
        //                    //lczd
        //                    try
        //                    {
        //                        if (lczd.Split('^')[0] == "6")
        //                        {
        //                            px.myDictionary["临床诊断"] = lczd.Split('^')[1];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    //主诉
        //                    try
        //                    {
        //                        if (lczd.Split('^')[0] == "1")
        //                        {
        //                            px.myDictionary["临床病史"] = px.myDictionary["临床病史"] + lczd.Split('^')[1];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    // 病史
        //                    try
        //                    {
        //                        if (lczd.Split('^')[0] == "2")
        //                        {
        //                            px.myDictionary["临床病史"] = px.myDictionary["临床病史"] + lczd.Split('^')[1];
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                }

        //                if (px.myDictionary["年龄"].Trim() == "")
        //                    px.myDictionary["年龄"] = ZGQClass.CsrqToAge(px.myDictionary["出生日期"]);

        //                string exep = "";
        //                //   MessageBox.Show(px.rtn_XML(ref exep));

        //                return px.rtn_XML(ref exep);
        //            }
        //            catch (Exception e2)
        //            {
        //                MessageBox.Show(e2.Message);
        //                return "0";
        //            }
        //        }
        //        else
        //        {
        //            //取基本信息
        //            if (isbrxx == "1")
        //            {
        //                if (debug == "1")
        //                    log.WriteMyLog("未取得申请单信息，提取病人基本信息");

        //                if (Sslbx.Trim() == "住院号" || Sslbx.Trim() == "就诊号")
        //                {
        //                    string rtn_msg2 = "";
        //                    if (Sslbx.Trim() == "住院号")
        //                    {

        //                        //if (isbrxx.Trim() != "1")
        //                        //{
        //                        //    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
        //                        //    return "0";
        //                        //}
        //                        string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><visitNo></visitNo><inpNo>" + Ssbz + "</inpNo><patientId></patientId><wardCode></wardCode><deptCode></deptCode></root>";
        //                        rtn_msg2 = rtn_CallInterface("XML", "GetPatientInHospital", XML2, "", WebUrl, debug);
        //                    }
        //                    else
        //                    {

        //                        //if (isbrxx.Trim() != "1")
        //                        //{
        //                        //    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
        //                        //    return "0";
        //                        //}

        //                        string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><icCardNo>" + Ssbz + "</icCardNo><executeDept></executeDept></root>";
        //                        rtn_msg2 = rtn_CallInterface("XML", "GetDispPatientInfo", XML2, "", WebUrl, debug);
        //                    }



        //                    if (rtn_msg2 == "")
        //                        return "0";


        //                    readhl7 rr7 = new readhl7();
        //                    int xy = 0;
        //                    rr7.Adt01(rtn_msg2, ref xy);
        //                    if (rr7.MSA[1].Trim() != "AA")
        //                    {
        //                        MessageBox.Show(rr7.MSA[3].Trim());

        //                        return "0";
        //                    }
        //                    else
        //                    {
        //                        XmlNode xmlok = null;
        //                        XmlDocument xd = new XmlDocument();
        //                        try
        //                        {
        //                            xd.LoadXml(rtn_msg2);
        //                            xmlok = xd.SelectSingleNode("/root/patients");
        //                            if (xmlok.InnerXml.Trim() != "")
        //                            {
        //                                DataSet ds1 = new DataSet();
        //                                StringReader sr = new StringReader(xmlok.InnerXml);
        //                                XmlReader xr = new XmlTextReader(sr);
        //                                ds1.ReadXml(xr);

        //                                px.myDictionary["姓名"] = ds1.Tables[0].Rows[0]["patientName"].ToString().Trim();
        //                                string xb = ds1.Tables[0].Rows[0]["patientSex"].ToString().Trim();
        //                                if (xb == "1") xb = "女";
        //                                else if (xb.Trim() == "0") xb = "男";
        //                                else xb = "";
        //                                px.myDictionary["性别"] = xb;
        //                                if (Sslbx.Trim() == "住院号")
        //                                {
        //                                    px.myDictionary["病人类别"] = "住院";
        //                                    px.myDictionary["就诊ID"] = ds1.Tables[0].Rows[0]["visitCount"].ToString().Trim();
        //                                    px.myDictionary["住院号"] = ds1.Tables[0].Rows[0]["visitNo"].ToString().Trim();
        //                                    px.myDictionary["床号"] = ds1.Tables[0].Rows[0]["bedCode"].ToString().Trim();
        //                                    px.myDictionary["病区"] = ds1.Tables[0].Rows[0]["wardName"].ToString().Trim();
        //                                }
        //                                else
        //                                {
        //                                    px.myDictionary["病人类别"] = "门诊";
        //                                    px.myDictionary["门诊号"] = ds1.Tables[0].Rows[0]["icCardNo"].ToString().Trim();
        //                                    px.myDictionary["病区"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();
        //                                }
        //                                if (ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() != "")
        //                                {

        //                                    try
        //                                    {
        //                                        px.myDictionary["年龄"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() + ds1.Tables[0].Rows[0]["ageUnit"].ToString().Trim();
        //                                    }
        //                                    catch
        //                                    {
        //                                        px.myDictionary["年龄"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim();
        //                                    }
        //                                }
        //                                px.myDictionary["地址"] = "电话：" + ds1.Tables[0].Rows[0]["telephone"].ToString().Trim();
        //                                px.myDictionary["电话"] = "^地址：";
        //                                px.myDictionary["病人编号"] = ds1.Tables[0].Rows[0]["patientId"].ToString().Trim();
        //                                px.myDictionary["送检科室"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();

        //                                px.myDictionary["申请序号"] = "";
        //                                px.myDictionary["送检医生"] = ds1.Tables[0].Rows[0]["doctorInCharge"].ToString().Trim().Split('/')[0].Trim();
        //                                px.myDictionary["费别"] = ds1.Tables[0].Rows[0]["rateTypeName"].ToString().Trim();
        //                                try
        //                                {
        //                                    px.myDictionary["临床诊断"] = ds1.Tables[0].Rows[0]["diagnosisname"].ToString().Trim();
        //                                }
        //                                catch
        //                                {
        //                                    px.myDictionary["临床诊断"] = "";
        //                                }

        //                                string exep = "";
        //                                return px.rtn_XML(ref exep);

        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("未能查询到病人记录");
        //                                return "0";
        //                            }
        //                        }
        //                        catch (Exception ee)
        //                        {
        //                            MessageBox.Show("XML解析错误:" + ee.Message);
        //                            log.WriteMyLog(rtn_msg2 + "--" + ee.Message);
        //                            return "0";
        //                        }
        //                    }


        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
        //            }


        //            return "0";
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show(ee.Message);
        //        return "0";

        //    }
        //}


    }
}
