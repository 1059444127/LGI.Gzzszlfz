
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using PATHGETHISZGQ;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 福州市第二医院, 
    /// 1,在服务器上装mq客户端，通过webservice连接
    /// 2,客户端上安装mq客户端，直接连接
    /// </summary>
    class fzs2y
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        private static string weburl = "";
        public static string ptxml(string Sslbx, string Ssbz,string debug)
        {

             debug = f.ReadString(Sslbx, "debug", "");
            int ljfs = f.ReadInteger(Sslbx, "ljfs", 1);
            weburl = f.ReadString(Sslbx, "wsurl", "");

            if (Sslbx != "")
            {

                if (Sslbx=="门诊号(新)")
                {
                  string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10020</Fid></AccessControl><MessageHeader><Fid>BS10020</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><onceFlag>0</onceFlag><startNum></startNum><endNum></endNum><Msg></Msg><query item=\"IC_CARD_ID\" compy=\"=\" value=\"'" + Ssbz + "'\" splice=\"and\"/><order item=\"\" sort=\"\"/></MsgInfo></ESBEntry>";

                    return getmzbrxx("BS10020", putcmsg, debug, ljfs);
                 
                }
                if (Sslbx == "住院号(新)")
                {

                    PT_XML pt = new PT_XML();
                    string putcmsg = "<ESBEntry><AccessControl><UserName></UserName><Password></Password><Fid>BS10001</Fid></AccessControl><MessageHeader><Fid>BS10001</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><onceFlag>0</onceFlag><startNum></startNum><endNum></endNum><Msg></Msg><query item=\"Pat_Admit_ID\" compy=\" like \" value=\"'" + Ssbz + "%'\" splice=\"and\"/><order item=\"\" sort=\"\"/></MsgInfo></ESBEntry>";

                      return getzybrxx("BS10001", putcmsg, debug, ref pt, ljfs);

                }


                if (Sslbx == "住院号(申请)" || Sslbx == "住院号")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25016</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  residence_no like '" + Ssbz + "%'</Msg></MsgInfo></ESBEntry>";

                    string rtnxml = getsqdxx("BS25016", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("返回的xml字符串:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "卡号" || Sslbx == "门诊号(申请)" || Sslbx == "门诊号")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25017</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  ic_card_id= '" + Ssbz + "' </Msg></MsgInfo></ESBEntry>";
                    string rtnxml = getsqdxx("BS25017", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("返回的xml字符串:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "住院申请号")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25016</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  apply_no= '" + Ssbz + "'</Msg></MsgInfo></ESBEntry>";
                    string rtnxml = getsqdxx("BS25016", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("返回的xml字符串:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "门诊申请号")
                {
                    string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25017</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  apply_no= '" + Ssbz + "'</Msg></MsgInfo></ESBEntry>";
                    string rtnxml = getsqdxx("BS25017", putcmsg, debug, ljfs, Ssbz);
                    if (debug == "1")
                        log.WriteMyLog("返回的xml字符串:" + rtnxml);
                    return rtnxml;
                }
                if (Sslbx == "门诊列表")
                {
                    fzs2y_mzlb mzlb = new fzs2y_mzlb();
                    if (mzlb.ShowDialog() == DialogResult.Yes)
                    {
                        string IC_CARD_ID = mzlb.IC_CARD_ID;
                        if (IC_CARD_ID.Trim() == "")
                        {
                            MessageBox.Show("所选择的病人卡号为空,无法获取信息！");
                            return "0";
                        }
                        else
                        {
                            string putcmsg = "<ESBEntry><MessageHeader><Fid>BS25017</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><Msg> and  ic_card_id= '" + IC_CARD_ID + "'</Msg></MsgInfo></ESBEntry>";
                            string rtnxml = getsqdxx("BS25017", putcmsg, debug, ljfs, Ssbz);
                            if (debug == "1")
                                log.WriteMyLog("返回的xml字符串:" + rtnxml);
                            return rtnxml;
                        }

                    }
                    else
                    {
                        MessageBox.Show("未选择病人,无法获取信息！");
                        return "0";
                    }
                  
                }
                else
                    MessageBox.Show("无此" + Sslbx);
                return "0";
            }
            else
            {
                MessageBox.Show("识别号不能为空");
                return "0";
            }

        }

       //获取病人基本信息
        private static string getzybrxx(string fid, string putcmsg, string debug, ref PT_XML pt,int ljfs)
        {
            if (debug == "1")
                log.WriteMyLog("入参：" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }
            else
            {
               
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }
            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("提取失败，返回为空");
                return "0";
            }

          
            string RetCon = "";
            string Msg = "";
            string RetCode = "";

            XmlNode xmlok = null;
            XmlNode xmlok_msg = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");
                xmlok_msg = xd.SelectSingleNode("/ESBEntry/MsgInfo");
                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;
                Msg = xmlok_msg["Msg"].InnerText;
            }
            catch (Exception e1)
            {
                MessageBox.Show("提取住院信息异常,解析返回值异常：" + e1.Message);
                return "0";
            }

            if (RetCode != "1")
            {
                MessageBox.Show(RetCon);
                return "0";
            }

            StringReader xmlstr = null;
            XmlTextReader xmread = null;
            xmlstr = new StringReader(Msg);
            xmread = new XmlTextReader(xmlstr);
            XmlDocument readxml2 = new XmlDocument();
            try
            {
                readxml2.Load(xmread);
            }
            catch (Exception e2)
            {
                MessageBox.Show("读XML失败：" + e2.Message);
                return "0";
            }
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(readxml2.NameTable);
            nsMgr.AddNamespace("ns", "urn:hl7-org:v3");


            XmlNode ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:id", nsMgr);
            //////姓名
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:name", nsMgr);
                pt.myDictionary["姓名"] = ppp.InnerText;
            }
            catch
            {
            }

            //性别
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:administrativeGenderCode", nsMgr);
                pt.myDictionary["性别"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //出身日期
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:birthTime", nsMgr);
                pt.myDictionary["出生日期"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            ///婚姻\
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:maritalStatusCode", nsMgr);
                pt.myDictionary["婚姻"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }
            //民族
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:ethnicGroupCode", nsMgr);
                pt.myDictionary["民族"] = ppp.Attributes["displayName"].Value;
            }
            catch
            {
            }

            //////电话
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:telecom", nsMgr);
                pt.myDictionary["电话"] = ppp.Attributes["value"].Value;
            }
            catch
            {
            }
            //地址
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:addr/ns:state", nsMgr);
                pt.myDictionary["地址"] = ppp.InnerText;
            }
            catch
            {
            }
            //身份证
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:id", nsMgr);
                pt.myDictionary["身份证号"] = ppp.Attributes["extension"].InnerText;
            }
            catch
            {
            }
            //年龄
            try
            {
               
                    ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:patient/ns:age", nsMgr);
                    pt.myDictionary["年龄"] = ppp.Attributes["value"].InnerText + ppp.Attributes["unit"].InnerText;
                
            }
            catch
            {
            }

            //病区
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["病区"] = ppp.InnerText;
            }
            catch
            {
            }
            //科室
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:name", nsMgr);
                pt.myDictionary["送检科室"] = ppp.InnerText;
            }
            catch
            {
            }
            //医生
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:author/ns:assignedAuthor/ns:assignedPerson/ns:name", nsMgr);
                pt.myDictionary["送检医生"] = ppp.InnerText;
            }
            catch
            {
            }
            //床号
            try
            {
                ppp = readxml2.SelectSingleNode("/ns:ClinicalDocument/ns:componentOf/ns:encompassingEncounter/ns:location/ns:healthCareFacility/ns:serviceProviderOrganization/ns:asOrganizationPartOf/ns:wholeOrganization/ns:id", nsMgr);
                pt.myDictionary["床号"] = ppp.Attributes["extension"].Value;
            }
            catch
            {
            }
            try
            {
                XmlNodeList ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:recordTarget/ns:patientRole/ns:id", nsMgr);
                foreach (XmlNode ppp2 in ppplist)
                {

                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.12")
                    {
                        pt.myDictionary["住院号"] = ppp2.Attributes["extension"].Value.ToString();

                    }
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.19")
                    {
                        //就诊卡号
                        pt.myDictionary["就诊ID"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    //{
                    //    //病案号
                    //       pt.myDictionary["病人编号"] = + ppp2.Attributes["extension"].Value.ToString();
                    //}
                    if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.100")
                    {
                        //padid
                        pt.myDictionary["病人编号"] = ppp2.Attributes["extension"].Value.ToString();
                    }
                    //if (ppp2.Attributes["root"].Value == "2.16.156.10011.1.13")
                    //{
                    //    //门诊号
                    //   pt.myDictionary["门诊号"]=ppp2.Attributes["extension"].Value.ToString();

                    //}
                }

                //ppplist = readxml2.SelectNodes("/ns:ClinicalDocument/ns:component/ns:structuredBody/ns:component/ns:section/ns:entry/ns:observation", nsMgr);

                //foreach (XmlNode ppp2 in ppplist)
                //{
                //    //住院次数
                //    if (ppp2["code"].Attributes["code"].Value.ToString() == "DE02.10.090.00")
                //    {
                //        pt.myDictionary["就诊ID"] = ppp2["value"].Attributes["value"].Value.ToString();
                //        break;

                //    }


                //}
            }
            catch
            {
            }
            string xb = pt.myDictionary["性别"].Trim();
            if (xb == "1") xb = "男";
            else if (xb == "2") xb = "女";
            pt.myDictionary["性别"] = xb;
            pt.myDictionary["病人类别"] = "住院";
            if (pt.myDictionary["年龄"].Trim() == "")
                pt.myDictionary["年龄"] = ZGQClass.CsrqToAge(pt.myDictionary["出生日期"]);
            if (pt.myDictionary["姓名"].Trim() == "")
            {
                MessageBox.Show("提取数据错误");
                return "0";
            }


            string ex = "";
            string xml = pt.rtn_XML(ref ex);
            if (ex.Trim() != "")
                log.WriteMyLog(ex);

            return xml;

        }

        //获取门诊病人基本信息
        private static string getmzbrxx(string fid, string putcmsg, string debug, int ljfs)
        {


            if (debug == "1")
                log.WriteMyLog("入参：" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }
            else
            {
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }
            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("提取失败，返回为空");
                return "0";
            }
            string RetCon = "";
           
            string RetCode = "";

            string bobys = "";

            XmlNode xmlok = null;
            XmlNodeList xmlNL = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;
                if (RetCode != "1")
                {
                    MessageBox.Show(RetCon);
                    return "0";
                }


                xmlNL = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");
                foreach (XmlNode xn in xmlNL)
                {
                    XmlNode xmlok_DATA = null;

                    try
                    {
                        xd.LoadXml(xn.InnerText);
                        xmlok_DATA = xd.SelectSingleNode("/body");
                    }
                    catch
                    {
                    }

                    bobys = bobys + xmlok_DATA.InnerXml;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("提取信息异常,解析返回值异常：" + e1.Message);
                return "0";
            }

     
            //转成dataset
            XmlNode xmlok_DATA2 = null;

            try
            {
                xd.LoadXml("<body>" + bobys + "</body>");
                xmlok_DATA2 = xd.SelectSingleNode("/body");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                return "0";
            }
            if (xmlok_DATA2.InnerXml.Trim() == "")
            {
                MessageBox.Show("未找到对应的记录！");
                return "0";
            }
            DataSet ds1 = new DataSet();

            try
            {
                StringReader sr = new StringReader(xmlok_DATA2.OuterXml);
                XmlReader xr = new XmlTextReader(sr);
                ds1.ReadXml(xr);

            }
            catch (Exception eee)
            {
                MessageBox.Show("XML转dataset异常：" + eee.Message);
                return "0";
            }

            if (ds1.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("未查询到此病人信息");
                return "0";
            }


            int xh = 0;


            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["SICK_ID"].ToString().Trim() + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["IC_CARD_ID"].ToString().Trim() + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + ""+ (char)34 + " ";


            xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[xh]["IC_CARD_ID"].ToString().Trim() + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
           
            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[xh]["NAME"].ToString().Trim() + (char)34 + " ";

            string xb = ds1.Tables[0].Rows[xh]["SEX"].ToString().Trim();
            if (xb == "0") xb = "男";
            else
                if (xb == "1") xb = "女"; else xb = "";
            xml = xml + "性别=" + (char)34 + xb+ (char)34 + " ";

           string nl= ZGQClass.CsrqToAge(ds1.Tables[0].Rows[xh]["BIRTHDATE"].ToString().Trim());
           xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[xh]["COMMUNICATE_ADDRESS"].ToString().Trim() + (char)34 + " ";
            xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[xh]["FAMILY_PHONE"].ToString().Trim() + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[xh]["ID_CARD_NO"].ToString().Trim() + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + ds1.Tables[0].Rows[xh]["NATION"].ToString().Trim() + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + ds1.Tables[0].Rows[xh]["PROFESSION"].ToString().Trim() + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + ds1.Tables[0].Rows[xh]["RATE_TYPE"].ToString().Trim() + (char)34 + " ";
            
            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + ""+ "]]></临床诊断>";
            xml = xml + "</LOGENE>";


            return xml;

        }

        //获取申请单信息
        private static string getsqdxx(string fid, string putcmsg, string debug, int ljfs, string Ssbz)
        {


            if (debug == "1")
                log.WriteMyLog("入参：" + putcmsg);

            string getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }
            else
            {
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                    return "0";
                }
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }
            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("提取失败，返回为空");
                return "0";
            }
            string RetCon = "";
          
            string RetCode = "";

            string bobys = "";

            XmlNode xmlok = null;
            XmlNodeList xmlNL = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;

                if (RetCode != "1")
                {
                    MessageBox.Show(RetCon);
                    return "0";
                }


                xmlNL = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");
                foreach (XmlNode xn in xmlNL)
                {
                    XmlNode xmlok_DATA = null;

                    try
                    {
                        xd.LoadXml(xn.InnerText);
                        xmlok_DATA = xd.SelectSingleNode("/body");
                    }
                    catch
                    {
                    }

                    bobys = bobys + xmlok_DATA.InnerXml;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("提取信息异常,解析返回值异常：" + e1.Message);
                return "0";
            }

            

            //转成dataset
            XmlNode xmlok_DATA2 = null;

            try
            {
                xd.LoadXml("<body>" + bobys + "</body>");
                xmlok_DATA2 = xd.SelectSingleNode("/body");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                return "0";
            }
            if (xmlok_DATA2.InnerXml.Trim() == "")
            {
                MessageBox.Show("未找到对应的记录！");
                return "0";
            }
            DataSet ds1 = new DataSet();

            try
            {
                StringReader sr = new StringReader(xmlok_DATA2.OuterXml);
                XmlReader xr = new XmlTextReader(sr);
                ds1.ReadXml(xr);

            }
            catch (Exception eee)
            {
                MessageBox.Show("XML转dataset异常：" + eee.Message);
                return "0";
            }

            if (ds1.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("未查询到此病人信息");
                return "0";
            }


            int xh = 0;

            if (ds1.Tables[0].Rows.Count > 1)
            {

                string Columns = "insur_no,apply_no,name,sex,age,residence_no,bed_no,apply_dept,apply_doctor,item_name,now_diagnosis";//显示的项目对应字段
                string ColumnsName = "医保卡号,单据号,姓名,性别,年龄,住院号,床号,送检科室,送检医生,项目名称,临床诊断";//显示的项目名称
                if (fid == "BS25017")
                {
                    Columns = "insur_no,apply_no,name,sex,age,apply_dept,apply_doctor,item_name,now_diagnosis";//显示的项目对应字段
                    ColumnsName = "医保卡号,单据号,姓名,性别,年龄,送检科室,送检医生,项目名称,临床诊断";//显示的项目名称
                }

                string xsys = "1"; //选择条件的项目
                DataColumn dc0 = new DataColumn("序号");
                ds1.Tables[0].Columns.Add(dc0);
                for (int x = 0; x < ds1.Tables[0].Rows.Count; x++)
                {
                    ds1.Tables[0].Rows[x][ds1.Tables[0].Columns.Count - 1] = x;
                }
                if (Columns.Trim() != "")
                    Columns = "序号," + Columns;
                if (ColumnsName.Trim() != "")
                    ColumnsName = "序号," + ColumnsName;

                FRM_YZ_SELECT yc = new FRM_YZ_SELECT(ds1.Tables[0], Columns, ColumnsName, xsys);
                yc.ShowDialog();
                string rtn2 = yc.F_XH;
                if (rtn2.Trim() == "")
                {
                    MessageBox.Show("未选择申请项目！");
                    return "0";
                }
                try
                {
                    xh = int.Parse(rtn2);
                }
                catch
                {
                    MessageBox.Show("请重新选择申请项目！");
                    return "0";
                }
            }
            else
            {
                if (ds1.Tables[0].Rows.Count < 1)
                {
                    MessageBox.Show("未查询到此病人信息");
                    return "0";
                }
            }

            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["sick_id"].ToString().Trim() + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["insur_no"].ToString().Trim() + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_no"].ToString().Trim() + (char)34 + " ";

            if (fid == "BS25016")
            {
                xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[xh]["residence_no"].ToString().Trim() + (char)34 + " ";
                xml = xml + "病区=" + (char)34 + ds1.Tables[0].Rows[xh]["dept_name"].ToString().Trim() + (char)34 + " ";
                xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[xh]["bed_no"].ToString().Trim() + (char)34 + " ";
            }
            else
            {
                string mzh = ds1.Tables[0].Rows[xh]["insur_no"].ToString().Trim();
                if (mzh == "")
                    mzh = Ssbz;
                xml = xml + "门诊号=" + (char)34 + mzh + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
            }
            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[xh]["name"].ToString().Trim() + (char)34 + " ";
            xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[xh]["sex"].ToString().Trim() + (char)34 + " ";
            xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[xh]["age"].ToString().Trim() + (char)34 + " ";
            xml = xml + "婚姻=" + (char)34 + ds1.Tables[0].Rows[xh]["marital_status"].ToString().Trim() + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[xh]["communicate_address"].ToString().Trim() + (char)34 + " ";
            xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[xh]["family_phone"].ToString().Trim() + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[xh]["id_card_no"].ToString().Trim() + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + ds1.Tables[0].Rows[xh]["nation"].ToString().Trim() + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + ds1.Tables[0].Rows[xh]["profession"].ToString().Trim() + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_dept"].ToString().Trim() + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_doctor"].ToString().Trim() + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + ds1.Tables[0].Rows[xh]["item_name"].ToString().Trim() + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + ds1.Tables[0].Rows[xh]["rate_type"].ToString().Trim() + (char)34 + " ";
            xml = xml + "病人类别=" + (char)34 + ds1.Tables[0].Rows[xh]["table_type"].ToString().Trim() + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + ds1.Tables[0].Rows[xh]["clinical_history"].ToString().Trim() + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[xh]["now_diagnosis"].ToString().Trim() + "]]></临床诊断>";
            xml = xml + "</LOGENE>";


            return xml;

        }

        //连接MQ客户端
        private static bool MQ(string fid, string putcmsg, string debug, ref string getcmsg)
        {
            try
            {
                MQDLL.MQFunction MQManagment = new MQDLL.MQFunction();
                long ret = 0;
                //连接
                ret = MQManagment.connectMQ();

                if (ret != 1)
                {
                    MessageBox.Show("连接MQ服务失败!");
                    return false;
                }
                if (debug == "1")
                    MessageBox.Show("连接MQ服务成功");

                string cmsgid = "";
                getcmsg = "";
                ret = 0;
                ret = MQManagment.putMsg(fid, putcmsg, ref cmsgid);
                ret = MQManagment.getMsgById(fid, cmsgid, 10000, ref getcmsg);

                

                if (getcmsg.Trim() == "")
                {
                    MessageBox.Show("提取信息失败，返回空");
                    return false;
                }
                //断开
                MQManagment.disconnectMQ();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }

            return true;
        }

    
    }
}
