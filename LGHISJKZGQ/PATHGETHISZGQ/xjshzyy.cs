using System;
using System.Collections.Generic;
using System.Text;
using HL7;
using dbbase;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data;
using System.Xml;
using System.IO;
using PATHGETHISZGQ;
namespace LGHISJKZGQ
{ /// <summary>
    /// 新疆石河子大学附属医院--费用确认接口--HL7
    /// </summary>
    class xjshzyy
    {
         private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
      
       
        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {

            string pathWEB = f.ReadString("病历号", "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
            if (Sslbx != "")
            {

                if (Sslbx == "撤销申请(HL7)")
                {
                    
                    string hl7server = f.ReadString(Sslbx, "hl7server", "");
                    string hl7port = f.ReadString(Sslbx, "hl7port", "");
                   
                    odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

                    try
                    {
                        DataTable hl7xx = aa.GetDataTable("select * from T_HL7_sqd where F_sqxh ='" + Ssbz.Split('^')[0].Trim() + "'", "HL7");

                        if (hl7xx == null)
                        {
                            MessageBox.Show("取消医嘱失败，查询申请表失败");
                            return "0";
                        }

                        if (hl7xx.Rows.Count < 1)
                        {
                            MessageBox.Show("取消医嘱失败，申请单表无记录");
                            return "0";
                        }
                        else
                        {
                          
                            string pid = hl7xx.Rows[0]["F_PID"].ToString().Trim();
                            string pv1 = hl7xx.Rows[0]["F_PV1"].ToString().Trim();
                            string orc = hl7xx.Rows[0]["F_ORC"].ToString().Trim();
                            string obr = hl7xx.Rows[0]["F_OBR"].ToString().Trim();
                            string XXX = @"^~\&";
                            string s12message = "MSH|" + XXX + "|BL|LOGENE|PT||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||ORM^O01|" + hl7xx.Rows[0]["F_messageid"].ToString() + "|P|2.4|||||gbk\r";

                            s12message = s12message + pid + "\r";
                            s12message = s12message + pv1 + "\r";
                            string[] orc1 = orc.Split('|');
                            string[] obr2 = obr.Split('|');
                            
                            string orc2 = "ORC|SC";
                            for (int x = 2; x < orc1.Length; x++)
                            {
                                if (x == 5)
                                {
                                    orc2 = orc2 + "|SC";
                                    continue;
                                }
                                if (x == 25)
                                {
                                    //门诊 3 ，住院2
                                    if (hl7xx.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                                    orc2 = orc2 + "|0";
                                    else
                                    orc2 = orc2 + "|3";
                                    continue;
                                }
                                if (x == 2)
                                {
                                    orc2 = orc2 + "|" + hl7xx.Rows[0]["F_hjsf"].ToString().Trim();
                                    continue;
                                }
                                if (x == 4)
                                {
                                    orc2 = orc2 + "|" + orc1[4] + "^^" + orc1[2].Split('^')[0];
                                    continue;
                                }
                                orc2 = orc2 + "|" + orc1[x];
                            }
                          
                            s12message = s12message + orc2 + "\r";
                            string obr1 = "";
                            obr2[27] = "1";

                            obr2[23] = obr2[23].Split('^')[0] + "^1";
                            obr1 = "OBR||" + hl7xx.Rows[0]["F_hjsf"].ToString().Trim();
                          
                            for (int x = 3; x < obr2.Length; x++)
                            {
                                if (x == 23)
                                {
                                    if (hl7xx.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                                        obr1 = obr1 + "|" + obr2[x].Split('^')[0].Replace("&", "").Trim() + "^2";
                                    else
                                        obr1 = obr1 + "|" + obr2[x].Split('^')[0].Replace("&", "").Trim() + "^1";
                                }
                                else
                                    obr1 = obr1 + "|" + obr2[x];
                            }
                          
                            s12message = s12message + obr1 + "\r";
                            byte[] by2 = System.Text.Encoding.UTF8.GetBytes(s12message);
                            byte[] by3 = new byte[by2.Length + 3];
                            by3[0] = 11;
                            Array.Copy(by2, 0, by3, 1, by2.Length);
                           
                            by3[by3.Length - 2] = 28;
                            by3[by3.Length - 1] = 13;
                            if (debug == "1")
                                log.WriteMyLog("send to server " + hl7server + ":" + hl7port + " message:" + System.Text.Encoding.UTF8.GetString(by3, 0, by3.Length));
                            HL7message b = new HL7message();
                            string result = "";
                          
                            b.sendmessage(ref by3, hl7server, hl7port, ref result, debug);
                           
                            if (result == "99")
                            {
                                MessageBox.Show("申请号" + Ssbz + ",撤销医嘱成功！");
                                aa.ExecuteSQL("delete  T_HL7_sqd  where F_sqxh ='" + hl7xx.Rows[0]["F_sqxh"].ToString().Trim() + "'");
                            }
                            else
                            {
                                MessageBox.Show("申请号" + Ssbz + ",撤销医嘱失败！");
                            }
                        }
                        return "0";
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("申请号" + Ssbz + ",撤销医嘱异常：！" + ee.Message); return "0";
                    }

                }


                /////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////
                ////wervices/////////////////////////////////////////////////////////////////////

                if (Sslbx == "撤销申请")
                {

                    return "0";
                }

                //不用
                //string rtn_XML = "";
                
                //if (Sslbx == "门诊申请号" || Sslbx == "住院申请号" || Sslbx == "体检申请号" || Sslbx == "住院号" || Sslbx == "门诊号" || Sslbx == "卡号" || Sslbx == "门诊patid" || Sslbx == "住院patid" || Sslbx == "挂号序号" || Sslbx == "住院首页序号")
                //{
                //    string issqd = f.ReadString(Sslbx, "issqd", "1");


                //    string codetype = "5";

                //    string brlb_1 = "0";
                //    if (Sslbx == "门诊申请号")
                //    {
                //        brlb_1 = "0"; codetype = "5";
                //    }
                //    if (Sslbx == "住院申请号")
                //    {
                //        brlb_1 = "1"; codetype = "5";
                //    }
                //    if (Sslbx == "体检申请号")
                //    {
                //        brlb_1 = "3"; codetype = "5";
                //    }

                //    if (Sslbx == "住院号")
                //    {
                //        brlb_1 = "1"; codetype = "1";
                //    }
                //    if (Sslbx == "门诊号")
                //    {
                //        brlb_1 = "0"; codetype = "1";
                //    }
                //    if (Sslbx == "卡号")
                //    {
                //        brlb_1 = "0"; codetype = "2";
                //    }
                //    if (Sslbx == "门诊patid")
                //    {
                //        brlb_1 = "0"; codetype = "3";
                //    }
                //    if (Sslbx == "住院patid")
                //    {
                //        brlb_1 = "1"; codetype = "3";
                //    }
                //    if (Sslbx == "挂号序号")
                //    {
                //        brlb_1 = "0"; codetype = "4";
                //    }
                //    if (Sslbx == "住院首页序号")
                //    {
                //        brlb_1 = "1"; codetype = "4";
                //    }

                //    xjshz_winning_web.WinningHIIPService xjshzweb = new xjshz_winning_web.WinningHIIPService();

                //    string retXml; int returnCode = -1;

                //    retXml = " <WinResponse><RowItem><PatientID>7</PatientID><HospNo>20000003</HospNo><PatName>测试2001</PatName><Sex>1</Sex><Age>20</Age><AgeUnit>岁</AgeUnit><WardOrReg></WardOrReg><ChargeType>101</ChargeType><CureNo>2</CureNo><CardNo>2001</CardNo><ApplyDept>103</ApplyDept><Ward>8103</Ward><BedNo>05+1</BedNo><ApplyDoctor>10012</ApplyDoctor><ClincDesc></ClincDesc><IDNum></IDNum><Phone></Phone><Address></Address><Zip></Zip><Career>工程技术</Career><Nation>汉族</Nation><ToDoc>10012</ToDoc><SendNo></SendNo><Syxh>2</Syxh><bqmc>心血管一科</bqmc><yexh>0</yexh><DeptName></DeptName><Jzlsh></Jzlsh><Klx></Klx><ClincName></ClincName><Birthday>19950821</Birthday></RowItem></WinResponse>";
                //    returnCode = 0;
                //    bool returnCodespecified;
                //  //  xjshzweb.TechService("JB01", @"<WinRequest><codetype>" + codetype + "</codetype><code>" + Ssbz + "</code><brlb>" + brlb_1 + "</brlb></WinRequest> ", out returnCode, out returnCodespecified, out retXml);
                //    log.WriteMyLog("入参数：" + @"<WinRequest><codetype>" + codetype + "</codetype><code>" + Ssbz + "</code><brlb>" + brlb_1 + "</brlb></WinRequest> ");
                //    log.WriteMyLog("返回：" + returnCode + " -- " + retXml);
                    

                //     if (returnCode==0)//调用成功
                //    {

                //        XmlDocument doc = new XmlDocument();

                //        //转dataset

                //        if (debug == "1") log.WriteMyLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",返回xml:" + retXml);

                //        XmlNode xmlok_DATA = null;
                //        try
                //        {
                //            doc.LoadXml(retXml);
                //            xmlok_DATA = doc.SelectSingleNode("/");
                //        }
                //        catch (Exception xmlok_e)
                //        {
                //            MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                //            return "0";
                //        }
                //        if (xmlok_DATA.InnerXml.Trim() == "")
                //        {
                //            MessageBox.Show("未找到对应的记录！");
                //            return "0";
                //        }

                //        DataSet ds = new DataSet();
                //        try
                //        {
                //            StringReader sr = new StringReader(xmlok_DATA.OuterXml);
                //            XmlReader xr = new XmlTextReader(sr);
                //            ds.ReadXml(xr);
                //        }
                //        catch (Exception eee)
                //        {
                //            MessageBox.Show("取信息失败,转dataset异常：" + eee.Message);
                //            return "0";
                //        }


                //        if (issqd == "1")
                //        {

                //            DataTable dt = new DataTable();

                //            int jb03_returnCode = -1; bool jb03_returnCodespecified = false; string jb03_retxml = "";

                //            string syxh = ds.Tables[0].Rows[0]["syxh"].ToString().Trim();
                //            if (syxh.Trim() == "")
                //                syxh = "0";

                //            string sendno = ds.Tables[0].Rows[0]["sendno"].ToString().Trim();
                //            if (sendno.Trim() == "")
                //                sendno = "0";

                //            string sJB03_str = @"<WinRequest>"
                //                + "<blh>" + ds.Tables[0].Rows[0]["hospno"].ToString().Trim() + "</blh>"
                //                + "<brlb>" + brlb_1 + "</brlb>"
                //                + "<patid>" + ds.Tables[0].Rows[0]["patientid"].ToString().Trim() + "</patid>"
                //                + "<syxh>" + syxh + "</syxh>"
                //                + "<qqxh>0</qqxh>"
                //                + "<tjrybh>0</tjrybh>"
                //                + "<rq1></rq1>"
                //                + "<rq2></rq2>"
                //                + "<zxks>307</zxks>"
                //                + "<sqdxh>" + sendno + "</sqdxh>"
                //                + "</WinRequest>";

                //           // xjshzweb.TechService("JB03", sJB03_str, out jb03_returnCode, out jb03_returnCodespecified, out jb03_retxml);
                //            jb03_retxml = "<WinResponse><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>46</qqxh><qqmxxh>25</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:01</qqrq><itemcode>8936</itemcode><itemname>病理体液涂片</itemname><price>80.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>0</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>59</qqxh><qqmxxh>35</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>9108</itemcode><itemname>病理大体标本摄像</itemname><price>30.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>0</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>84</qqxh><qqmxxh>56</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:33:49</qqrq><itemcode>9090</itemcode><itemname>免疫荧光染色诊断</itemname><price>60.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>0</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>145</qqxh><qqmxxh>76</qqmxxh><qqksmc>病科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082219:27:00</qqrq><itemcode>8921</itemcode><itemname>手术标本检查与诊断</itemname><price>140.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>0</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>9</qqxh><qqmxxh>8</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082123:14:40</qqrq><itemcode>270300002B</itemcode><itemname>内镜组织活检检查与诊断超过两个蜡</itemname><price>10.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>10</qqxh><qqmxxh>9</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082123:14:40</qqrq><itemcode>270300004</itemcode><itemname>骨髓组织活检检查与诊断</itemname><price>100.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>12</qqxh><qqmxxh>10</qqmxxh><qqksmc>病科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082123:14:40</qqrq><itemcode>270300006B</itemcode><itemname>截肢标本病理检查与诊断超过两个蜡</itemname><price>30.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>45</qqxh><qqmxxh>24</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:23:42</qqrq><itemcode>270300003B</itemcode><itemname>局部切除组织活检检查与诊断超过两</itemname><price>10.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>47</qqxh><qqmxxh>26</qqmxxh><qqksmc>病科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270300003B</itemcode><itemname>局部切除组织活检检查与诊断超过两</itemname><price>10.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>48</qqxh><qqmxxh>27</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270300003B</itemcode><itemname>局部切除组织活检检查与诊断超过两</itemname><price>10.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>49</qqxh><qqmxxh>28</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270300004</itemcode><itemname>骨髓组织活检检查与诊断</itemname><price>100.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>51</qqxh><qqmxxh>29</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270400001A</itemcode><itemname>冰冻切片检查与诊断</itemname><price>160.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>53</qqxh><qqmxxh>30</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270400001A</itemcode><itemname>冰冻切片检查与诊断</itemname><price>160.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>55</qqxh><qqmxxh>31</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270700001</itemcode><itemname>原位杂交技术</itemname><price>120.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>56</qqxh><qqmxxh>32</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270300002B</itemcode><itemname>内镜组织活检检查与诊断超过两个蜡</itemname><price>10.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>57</qqxh><qqmxxh>33</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270300002A</itemcode><itemname>内镜组织活检检查与诊断</itemname><price>60.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>57</qqxh><qqmxxh>34</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:31:26</qqrq><itemcode>270500001</itemcode><itemname>特殊染色及酶组织化学染色诊断(每</itemname><price>40.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>85</qqxh><qqmxxh>57</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:33:49</qqrq><itemcode>270300002A</itemcode><itemname>内镜组织活检检查与诊断</itemname><price>60.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>87</qqxh><qqmxxh>58</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:33:49</qqrq><itemcode>270300002A</itemcode><itemname>内镜组织活检检查与诊断</itemname><price>60.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>94</qqxh><qqmxxh>61</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082211:33:49</qqrq><itemcode>270500002a</itemcode><itemname>全自动快速免疫组织化学染色诊断</itemname><price>130.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem><RowItem><blh>20000003</blh><brlb>1</brlb><patid>7</patid><syxh>2</syxh><qqxh>144</qqxh><qqmxxh>75</qqmxxh><qqksmc>病理科</qqksmc><ysmc>田斌</ysmc><qqrq>2015082219:21:23</qqrq><itemcode>270300003A</itemcode><itemname>局部切除组织活检检查与诊断</itemname><price>60.0000</price><itemqty>1.00</itemqty><itemunit>次</itemunit><url></url><itemtype>1</itemtype><qqlx>0</qqlx></RowItem></WinResponse>";


                //            log.WriteMyLog("入参数：" + sJB03_str);
                //            log.WriteMyLog("返回：" + jb03_returnCode + " -- " + jb03_retxml);
                //            jb03_returnCode = 0;
                //            if (jb03_returnCode == 0)//调用成功
                //            {

                //                XmlDocument doc_2 = new XmlDocument();
                //                //转dataset

                //                if (debug == "1")
                //                    log.WriteMyLog("返回xml:" + jb03_retxml);

                //                XmlNode xmlok_DATA_2 = null;
                //                try
                //                {
                //                    doc_2.LoadXml(jb03_retxml);
                //                    xmlok_DATA_2 = doc_2.SelectSingleNode("/");
                //                }
                //                catch (Exception xmlok2_e)
                //                {
                //                    log.WriteMyLog("解析申请单xml异常：" + xmlok2_e.Message);
                //                    return return_xml(ds, Sslbx, brlb_1);
                //                }
                //                if (xmlok_DATA_2.InnerXml.Trim() == "")
                //                {
                //                    return return_xml(ds,Sslbx,brlb_1);
                //                }
                //                DataTable dt_sqd = new DataTable();
                //                try
                //                {
                //                    DataSet ds_sqd_1 = new DataSet();
                //                    StringReader sr_2 = new StringReader(xmlok_DATA_2.OuterXml);
                //                    XmlReader xr_2 = new XmlTextReader(sr_2);
                //                    ds_sqd_1.ReadXml(xr_2);
                                  
                //                    dt_sqd = ds_sqd_1.Tables[0];
                //                    int z = dt_sqd.Rows.Count;
                                   
                //                    for (int x = 0; x < z; x++)
                //                    {
                //                        if (dt_sqd.Rows[x]["qqksmc"].ToString().Trim() != "病理科")
                //                        {
                //                            dt_sqd.Rows[x].Delete();x--; z--;
                //                        }
                //                    }

                //                }
                //                catch (Exception ee4)
                //                {
                //                    log.WriteMyLog("取信息失败,转dataset异常：" + ee4.Message);
                //                    return return_xml(ds, Sslbx, brlb_1);
                //                }
                            
                //                if (dt_sqd.Rows.Count <= 0)
                //                    return return_xml(ds, Sslbx, brlb_1);
                               
                //                int count = 0;
                //                if (dt_sqd.Rows.Count > 1)
                //                {
                //                    DataColumn dc0 = new DataColumn("序号");
                //                    dt_sqd.Columns.Add(dc0);
                //                    int z = dt_sqd.Rows.Count;
                //                    for (int x = 0; x < z; x++)
                //                    {
                //                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                //                    }


                //                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, "序号,blh,syxh,qqxh,qqksmc,ysmc,itemname,itemcode,price,itemqty,itemtype", "序号,病历号,首页序号,申请序号,科室,医生,项目名称,项目代码,项目价格,数量,项目类别", "");
                //                    yc.ShowDialog();
                //                    string rtn2 = yc.F_XH;
                //                    if (rtn2.Trim() == "")
                //                    {
                //                        MessageBox.Show("未选择申请项目！");
                //                        return "0";
                //                    }
                //                    try
                //                    {
                //                        count = int.Parse(rtn2);
                //                    }
                //                    catch
                //                    {
                //                        MessageBox.Show("请重新选择申请项目！");
                //                        return "0";
                //                    }
                //                }

                //                try
                //                {

                //                    PT_XML pt = new PT_XML();
                //                    pt.myDictionary["病人编号"] = ds.Tables[0].Rows[0]["patientid"].ToString().Trim();
                //                    pt.myDictionary["就诊ID"] = ""; ds.Tables[0].Rows[0]["cardno"].ToString().Trim();
                //                    pt.myDictionary["申请序号"] = ds.Tables[0].Rows[0]["sendno"].ToString().Trim();

                //                    if (Sslbx == "住院申请号" || Sslbx == "住院号" || Sslbx.Contains("住院"))
                //                    {
                //                        pt.myDictionary["门诊号"] = "";
                //                        pt.myDictionary["住院号"] = ds.Tables[0].Rows[0]["hospno"].ToString().Trim();
                //                    }
                //                    else
                //                    {
                //                        pt.myDictionary["门诊号"] = ds.Tables[0].Rows[0]["hospno"].ToString().Trim();
                //                        pt.myDictionary["住院号"] = "";
                //                    }

                //                    pt.myDictionary["姓名"] = ds.Tables[0].Rows[0]["patname"].ToString().Trim();

                //                    string xb = ds.Tables[0].Rows[0]["sex"].ToString().Trim();
                //                    if (xb == "1") xb = "男";
                //                    else if (xb == "2") xb = "女";
                //                    else xb = "";
                //                    pt.myDictionary["性别"] = xb;
                //                    pt.myDictionary["年龄"] = ds.Tables[0].Rows[0]["age"].ToString().Trim() + ds.Tables[0].Rows[0]["ageunit"].ToString().Trim();
                //                    pt.myDictionary["婚姻"] = "";
                //                    pt.myDictionary["地址"] = ds.Tables[0].Rows[0]["address"].ToString().Trim();
                //                    pt.myDictionary["电话"] = ds.Tables[0].Rows[0]["phone"].ToString().Trim();
                //                    pt.myDictionary["病区"] = ds.Tables[0].Rows[0]["bqmc"].ToString().Trim();
                //                    pt.myDictionary["床号"] = ds.Tables[0].Rows[0]["bedno"].ToString().Trim();
                //                    pt.myDictionary["身份证号"] = ds.Tables[0].Rows[0]["idnum"].ToString().Trim();
                //                    pt.myDictionary["民族"] = ds.Tables[0].Rows[0]["Nation"].ToString().Trim(); ;
                //                    pt.myDictionary["职业"] = ds.Tables[0].Rows[0]["career"].ToString().Trim();
                //                    pt.myDictionary["送检科室"] = ds.Tables[0].Rows[0]["deptName"].ToString().Trim();
                //                    pt.myDictionary["送检医生"] = dt_sqd.Rows[0]["ysmc"].ToString().Trim();
                //                    pt.myDictionary["收费"] = "";
                //                    pt.myDictionary["标本名称"] = "";
                //                    pt.myDictionary["送检医院"] = "本院";
                //                    pt.myDictionary["医嘱项目"] = dt_sqd.Rows[count]["itemname"].ToString().Trim();
                //                    pt.myDictionary["备用1"] = "";
                //                    pt.myDictionary["备用2"] = "";
                //                    pt.myDictionary["费别"] = "";
                //                    if (brlb_1 == "1")
                //                        pt.myDictionary["病人类别"] = "住院";
                //                    else if (brlb_1 == "3")
                //                        pt.myDictionary["病人类别"] = "体检";
                //                    else
                //                        pt.myDictionary["病人类别"] = "门诊";

                //                    pt.myDictionary["临床病史"] = "";
                //                    try
                //                    {
                //                        pt.myDictionary["临床诊断"] = ds.Tables[0].Rows[0]["ClincName"].ToString().Trim();
                //                    }
                //                    catch
                //                    {
                //                    }

                //                    string ex = "";
                //                    return pt.rtn_XML(ref ex);
                //                }
                //                catch (Exception e)
                //                {
                //                    MessageBox.Show("解析xml异常：" + e.Message);
                //                    return "0";
                //                }
                //            }
                //            else
                //                return return_xml(ds, Sslbx, brlb_1);
                //        }
                //        else
                //            return return_xml(ds, Sslbx, brlb_1);
                //    }
                //    else//调用失败
                //    {
                //        MessageBox.Show("未查询到相关信息！");
                //        return "0";
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("无此识别类型：" + Sslbx);
                //    return "0";

                //}
            } return "0";
        }


        public static string return_xml(DataSet ds, string Sslbx, string brlb_1)
        {
            try
            {

                PT_XML pt = new PT_XML();
                pt.myDictionary["病人编号"] = ds.Tables[0].Rows[0]["patientid"].ToString().Trim();
                pt.myDictionary["就诊ID"] = ""; ds.Tables[0].Rows[0]["cardno"].ToString().Trim();
                pt.myDictionary["申请序号"] = ds.Tables[0].Rows[0]["sendno"].ToString().Trim();

                if (Sslbx == "住院申请号" || Sslbx == "住院号" || Sslbx.Contains("住院"))
                {
                    pt.myDictionary["门诊号"] = "";
                    pt.myDictionary["住院号"] = ds.Tables[0].Rows[0]["hospno"].ToString().Trim();
                }
                else
                {
                    pt.myDictionary["门诊号"] = ds.Tables[0].Rows[0]["hospno"].ToString().Trim();
                    pt.myDictionary["住院号"] = "";
                }

                pt.myDictionary["姓名"] = ds.Tables[0].Rows[0]["patname"].ToString().Trim();

                string xb = ds.Tables[0].Rows[0]["sex"].ToString().Trim();
                if (xb == "1") xb = "男";
                else if (xb == "2") xb = "女";
                else xb = "";
                pt.myDictionary["性别"] = xb;
                pt.myDictionary["年龄"] = ds.Tables[0].Rows[0]["age"].ToString().Trim() + ds.Tables[0].Rows[0]["ageunit"].ToString().Trim();
                pt.myDictionary["婚姻"] = "";
                pt.myDictionary["地址"] = ds.Tables[0].Rows[0]["address"].ToString().Trim();
                pt.myDictionary["电话"] = ds.Tables[0].Rows[0]["phone"].ToString().Trim();
                pt.myDictionary["病区"] = ds.Tables[0].Rows[0]["bqmc"].ToString().Trim();
                pt.myDictionary["床号"] = ds.Tables[0].Rows[0]["bedno"].ToString().Trim();
                pt.myDictionary["身份证号"] = ds.Tables[0].Rows[0]["idnum"].ToString().Trim();
                pt.myDictionary["民族"] = ds.Tables[0].Rows[0]["Nation"].ToString().Trim(); ;
                pt.myDictionary["职业"] = ds.Tables[0].Rows[0]["career"].ToString().Trim();
                pt.myDictionary["送检科室"] = ds.Tables[0].Rows[0]["deptName"].ToString().Trim();
                pt.myDictionary["送检医生"] = ds.Tables[0].Rows[0]["todoc"].ToString().Trim();
                pt.myDictionary["收费"] = "";
                pt.myDictionary["标本名称"] = "";
                pt.myDictionary["送检医院"] = "本院";
                pt.myDictionary["医嘱项目"] = "";
                pt.myDictionary["备用1"] = "";
                pt.myDictionary["备用2"] = "";
                pt.myDictionary["费别"] = "";
                if (brlb_1 == "1")
                    pt.myDictionary["病人类别"] = "住院";
                else if (brlb_1 == "3")
                    pt.myDictionary["病人类别"] = "体检";
                else
                    pt.myDictionary["病人类别"] = "门诊";

                pt.myDictionary["临床病史"] = "";
                try
                {
                    pt.myDictionary["临床诊断"] = ds.Tables[0].Rows[0]["ClincName"].ToString().Trim();
                }
                catch
                {
                }

                string ex = "";
                return pt.rtn_XML(ref ex);
            }
            catch (Exception e)
            {
                MessageBox.Show("解析xml异常：" + e.Message);
                return "0";
            }
        }
    }
}
   