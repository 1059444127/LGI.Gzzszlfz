using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using dbbase;
using System.Data;
using System.IO;
using System.Resources;
using LGHISJKZGQ;
using System.Net;
using System.Data.OracleClient;
using System.Data.SqlClient;
namespace LGHISJKZGQ
{
    class WHXHYY
    {
        //武汉协和医院
        private static IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");
     // private static dbbase.sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
           
            if (Sslbx != "")
            {
                string pathWEB = f.ReadString("医嘱号", "webservicesurl", "");
                ///门诊申请单
                if (Sslbx == "门诊号"||Sslbx == "医保号"||Sslbx == "发票号")
                {
                    int CodeType = 1002;
                    if(Sslbx=="医保号")
                        CodeType = 1003;
                    if (Sslbx == "发票号")
                        CodeType = 1004;


                    OracleConnection con = new OracleConnection();
                    OracleCommand cmd = new OracleCommand();
                    OracleParameter sqlpt = new OracleParameter();
                    OracleParameter sqlpt2 = new OracleParameter(); ;
                    OracleDataAdapter dqlda;
                    DataSet ds = new DataSet();

                    try
                    {
                        con.ConnectionString = "";
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_EOrder_For_Kodak";
                        sqlpt.ParameterName = "@CodeType";
                        sqlpt.Value = CodeType;
                        cmd.Parameters.Add(sqlpt);

                        sqlpt2.ParameterName = "@Code";
                        sqlpt2.Value = Ssbz.Trim();
                        cmd.Parameters.Add(sqlpt2);

                        dqlda = new OracleDataAdapter(cmd);
                        dqlda.Fill(ds);
                        con.Close();
                    }
                    catch(Exception xx)
                    {
                        con.Close();
                        if (Debug == "1")
                            MessageBox.Show("数据库读取错误异常！" + xx.ToString());
                        log.WriteMyLog("数据库读取错误异常！"+xx.ToString());
                        return "0";
                    }

                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                        MessageBox.Show("未找到相应的记录");
                        return "0";
                    }

                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds.Tables[0].Rows[0]["HIS_PID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds.Tables[0].Rows[0]["HIS_ACCESSION_NO"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds.Tables[0].Rows[0]["HIS_EXTER_ID"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds.Tables[0].Rows[0]["PAT_NAME_CHN"].ToString() + (char)34 + " ";
                        string sex = ds.Tables[0].Rows[0]["SEX"].ToString();
                        switch (sex)
                        {
                            case "1": xml = xml + "性别=" + (char)34 + "男" + (char)34 + " "; break;
                            case "2": xml = xml + "性别=" + (char)34 + "女" + (char)34 + " "; break;
                            case "3": xml = xml + "性别=" + (char)34 + "其他" + (char)34 + " "; break;
                            case "4": xml = xml + "性别=" + (char)34 + "未知" + (char)34 + " "; break;
                        }

                        string nl1 = ds.Tables[0].Rows[0]["BIRTH_TIME"].ToString();
                        string nl = "0岁";
                        try
                        {
                            nl = (DateTime.Now.Year - DateTime.Parse(nl1).Year).ToString();
                        }
                        catch
                        {
                            nl = "0岁";
                        }

                        xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds.Tables[0].Rows[0]["ADDRESS"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds.Tables[0].Rows[0]["PHONE"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + ds.Tables[0].Rows[0]["IN_HOSPITAL_REGION"].ToString() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + ds.Tables[0].Rows[0]["BED_NO"].ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ds.Tables[0].Rows[0]["ID_NO"].ToString() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + ds.Tables[0].Rows[0]["F_mz"].ToString() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + ds.Tables[0].Rows[0]["F_zy"].ToString() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + ds.Tables[0].Rows[0]["APPLY_DEPART"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds.Tables[0].Rows[0]["APPLY_DOCTOR"].ToString() + (char)34 + " ";
                        //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                        //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + ds.Tables[0].Rows[0]["F_sf"].ToString() + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + ds.Tables[0].Rows[0]["F_bbmc"].ToString() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + ds.Tables[0].Rows[0]["PROCEDURE_DESC"].ToString() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + ds.Tables[0].Rows[0]["F_brlb"].ToString() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + ds.Tables[0].Rows[0]["EXAM_COMMENTS"].ToString() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + ds.Tables[0].Rows[0]["OBSERVATION"].ToString() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;
                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("PATHGETHISZGQ：" + ee.ToString());
                        return "0";
                    }


                }

                ///住院申请单
               
                if (Sslbx == "医嘱号" || Sslbx == "住院号")
                {
                    string CardTypes = "";
                    if (Sslbx == "医嘱号") CardTypes = "1";
                    else CardTypes = "4";
                    string xmlstr = "<Request>"
                                    + "<CardValue>" + Ssbz + "</CardValue>"
                                    + "<CardTypes>" + CardTypes + "</CardTypes>"
                                    + "<StartDate>" + DateTime.Today.Date.AddDays(-7).ToString("yyyy-MM-dd") + "</StartDate>"
                                    + "<EndDate>" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "</EndDate>"
                                    + "<ExeLoc>1214@病理科</ExeLoc>"
                                    + "<EpsiodeType>I</EpsiodeType>"
                                    + "</Request>";

                    string rtn = "";
                 
                    try
                    {
                        LGHISJKZGQ.whxhyyWEB.WebRisService whxhyy = new LGHISJKZGQ.whxhyyWEB.WebRisService();
                      
                        if (pathWEB != "") whxhyy.Url = pathWEB;
                       
                        rtn = whxhyy.GetPatOrdList(xmlstr);

                        whxhyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn.Trim() == "" || rtn == null)
                    {

                        MessageBox.Show("没有该病人信息，请核实医嘱号或住院码是否正确！"); return "0";
                    }
                   
                    try
                    {

                        XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(rtn);
                      
                        xmlok = xd.SelectSingleNode("/Response");
                      
                        string messae = xmlok["ResultContent"].InnerXml;
                        if (xmlok["ResultCode"].InnerXml.ToString() == "-1")
                        {
                            if (messae == "医嘱已经执行或者已经停止")
                                MessageBox.Show(messae);
                            else
                                MessageBox.Show("提取信息失败！请确认该号码是否存在！");
                            return "0";
                        }
                  
                        if (messae != "查询成功")
                        {
                            MessageBox.Show(messae);
                            return "0";
                        }
                       
                        XmlNode xmlok2 = null;
                        XmlDocument xd2 = new XmlDocument();
                        try
                        {
                            xd2.LoadXml(xmlok["PatOrdLists"].InnerXml);

                            xmlok2 = xd2.SelectSingleNode("/PatOrdList");
                        }
                        catch
                        {
                            MessageBox.Show("没有该病人信息，请核实医嘱号或住院码是否正确！"); return "0";
                        }
                       
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + xmlok2["RegNo"].InnerText + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + xmlok2["AdmNo"].InnerText + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + xmlok2["OrdRowID"].InnerText + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + xmlok2["DocumentID"].InnerText + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + xmlok2["Name"].InnerText + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + xmlok2["Sex"].InnerText + (char)34 + " ";
                        int nl = DateTime.Now.Year - DateTime.Parse(xmlok2["BirthDay"].InnerText).Year;
                        xml = xml + "年龄=" + (char)34 + nl.ToString() + "岁" + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + xmlok2["Marry"].InnerText + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + xmlok2["Address"].InnerText + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + xmlok2["Telephone"].InnerText + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + xmlok2["Ward"].InnerText + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + xmlok2["BedNo"].InnerText + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + xmlok2["CredentialNo"].InnerText + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + xmlok2["Nation"].InnerText + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + xmlok2["Occupation"].InnerText + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + xmlok2["OrdLoc"].InnerText + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + xmlok2["OrdDoctor"].InnerText + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + xmlok2["OrdPrice"].InnerText + (char)34 + " ";

                        xml = xml + "标本名称=" + (char)34 + xmlok2["Position"].InnerText + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + xmlok2["OrdName"].InnerText + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + xmlok2["FeeType"].InnerText + (char)34 + " ";
                        string brlb = xmlok2["AdmType"].InnerText;
                        if (brlb == "I") brlb = "住院";
                        if (brlb == "O") brlb = "门诊";
                        if (brlb == "E") brlb = "急诊";
                        if (brlb == "I") brlb = "住院";
                        if (brlb == "H") brlb = "体检";
                        if (brlb == "N") brlb = "新生儿";
                        xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + xmlok2["OperationInfo"].InnerText + "      " + xmlok2["ClinicDisease"].InnerText + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + xmlok2["ClinicDiagnose"].InnerText + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }

                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        return "0";
                    }
                }
                if (Sslbx == "撤销医嘱申请")
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select * from T_jcxx where F_sqxh='" + Ssbz + "'", "blxx");
                    if (bljc == null || bljc.Rows.Count == 0)
                    {
                        if (MessageBox.Show("该病人还未在病理系统里登记，是否由开单科室取消医嘱申请！", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            MessageBox.Show("请通知开单科室取消该病理医嘱申请！！！");
                            return "0";
                        }
                       
                    }

                    string rtn = ""; string rtn2 = "";
                    try
                    {
                        LGHISJKZGQ.whxhyyWEB.WebRisService whxhyy = new LGHISJKZGQ.whxhyyWEB.WebRisService();
                        if (pathWEB != "") whxhyy.Url = pathWEB;


                        if (bljc != null)
                        {
                            if (bljc.Rows.Count > 0)   //取消
                            {
                                if (bljc.Rows[0]["F_bgzt"].ToString() == "已登记")
                                {
                                    string jsybh = getymh(bljc.Rows[0]["F_jsy"].ToString().Trim());
                                    string jsy = bljc.Rows[0]["F_jsy"].ToString().Trim();
                                    if (jsy.Trim() == "")
                                    { jsy = "接收员"; jsybh = "00001"; }
                                    string xmlstr = "<Request><CancelFeeApps><CancelFeeApp>"
                                       + "<OrdRowID>" + Ssbz + "</OrdRowID>"
                                       + "<OperatorCode>" + jsybh + "</OperatorCode>"
                                       + "<Operator>" + jsy + "</Operator>"
                                       + "<CacelFeeAppDate>" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "</CacelFeeAppDate>"
                                       + "<CacelFeeAppTime>" + DateTime.Today.TimeOfDay.ToString() + "</CacelFeeAppTime>"
                                       + "<CacelFeeAppReason>" + "" + "</CacelFeeAppReason>"
                                       + "</CancelFeeApp></CancelFeeApps></Request>";

                                    if (MessageBox.Show("此病人已登记，是否确认并停止该医嘱？？？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                    {
                                        MessageBox.Show("取消撤销医嘱");
                                        return "0";

                                    }
                                    rtn = whxhyy.GetCancelFeeApp(xmlstr);
                                    string messages = "";
                                    try
                                    {
                                        XmlNode xmlok = null;
                                        XmlDocument xd = new XmlDocument();
                                        xd.LoadXml(rtn);
                                        xmlok = xd.SelectSingleNode("/Response");
                                        messages = xmlok["ResultContent"].InnerText.ToString();
                                    }
                                    catch (Exception eee)
                                    {

                                        if (Debug == "1")
                                            MessageBox.Show("xml解析错误,取消失败");
                                        log.WriteMyLog(eee.ToString());
                                        return "0";
                                    }
                                    if (messages != "成功")
                                    {
                                        MessageBox.Show("取消失败");
                                        log.WriteMyLog(messages);
                                        return "0";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("此病例已取材，不能撤销医嘱");
                                    return "0";
                                }

                            }
                        }
                        //停止
                        
                            string xmlstr1 = "<Request><StopOrderRt>"
                               + "<OrdRowID>" + Ssbz + "</OrdRowID>"
                               + "<OperatorID>" + "" + "</OperatorID>"

                               + "<StopDate>" + DateTime.Today.Date.ToString("yyyy-MM-dd") + "</StopDate>"
                               + "<StopTime>" + DateTime.Today.TimeOfDay.ToString() + "</StopTime>"

                               + "</StopOrderRt></Request>";
                            rtn2 = whxhyy.StopOrder(xmlstr1);
                           
                             if (rtn2.Trim() == "" || rtn2 == null)
                                  {

                        MessageBox.Show("撤销医嘱失败！"); return "0";
                                   }
                                 
                    XmlNode xmlok2 = null;
                    XmlDocument xd2= new XmlDocument();
                    string  messtr="";
                          try
                          {
                            xd2.LoadXml(rtn2);
                            xmlok2 = xd2.SelectSingleNode("/StopOrderRp");
                              messtr = xmlok2["ResultContent"].InnerText.ToString();
                          }
                        catch
                          {

                            if (Debug == "1")
                                MessageBox.Show("xml解析错误！");
                            log.WriteMyLog(messtr);
                            MessageBox.Show("撤销失败，请联系开单科室");
                            return "0";
                          }
                       
                    
                    MessageBox.Show(messtr);
                    if (messtr != "成功")
                    {
                        log.WriteMyLog(messtr); 
                    return "0";
                        
                    }
                    }
                    catch (Exception eee)
                    {
                        if (Debug == "1")
                            MessageBox.Show("WEBService连接错误！");
                        log.WriteMyLog(eee.ToString());
                        return "0";
                    }

                  

                    return "0";
                }

                MessageBox.Show("无此" + Sslbx);
                return "0";

            }
            MessageBox.Show("识别类型不能为空");
            return "0";

        }



        public static string getymh(string yhmc)//通过医生名称 获取医生编码
        {
            if (yhmc != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable bljc = new DataTable();
                    bljc = aa.GetDataTable("select F_yhbh from T_YH where F_yhmc='" + yhmc + "'", "blxx");
                    return bljc.Rows[0]["F_yhbh"].ToString().Trim();
                }
                catch (Exception ee)
                {
                    log.WriteMyLog("转换医生工号出错！原因：" + ee.ToString());
                    return "";
                }
            } return "";
        }
    }
}

