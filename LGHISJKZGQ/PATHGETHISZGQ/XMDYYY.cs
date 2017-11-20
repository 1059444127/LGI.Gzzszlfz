using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;
using System.IO;
using LGHISJKZGQ;
using dbbase;
using PATHGETHISZGQ;
using PATHZGQ_HL7;
namespace LGHISJKZGQ
{
   /// <summary>
   /// 厦门第一医院  集成平台  webservice+hl7
   /// </summary>
        class XMDYYY
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private static string tqbblb = "";
        private static string yhmc = "";
        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {
            if (Sslbx.Trim() == "")
            {
                MessageBox.Show("无此" + Sslbx);
                return "0";
            }
            tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
             yhmc = f.ReadString("yh", "yhmc", "0");
           #region 获取SZ或T_SZ配置


            string xtdm = f.ReadString(Sslbx, "xtdm", "2060000").Replace("\0", "").Trim();
            string WebUrl = f.ReadString(Sslbx, "weburl", "").Replace("\0", "").Trim();
             debug = f.ReadString(Sslbx, "debug", "").Replace("\0", "").Trim();
             string isbrxx = f.ReadString(Sslbx, "isbrxx", "").Replace("\0", "").Trim();
             
           #endregion

           #region  取消检查申请

              if (Sslbx == "扣费撤销(病理号)" || Sslbx == "扣费撤销(申请号)")
            {

                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt = new DataTable();
                  if(Sslbx == "扣费撤销(病理号)")
                dt = aa.GetDataTable("select * from T_JCXX where F_BLH='" + Ssbz .Trim()+ "'", "d1");
                  else
                dt = aa.GetDataTable("select * from T_JCXX where F_SQXH='" + Ssbz.Trim() + "' and  F_SDRQ>='"+DateTime.Today.ToString("yyyy-MM-dd")+"'", "d1");
                if (dt == null)
                {
                    MessageBox.Show("数据库连接错误！");
                }
                if (dt.Rows.Count > 0)
                {
                    string brlb = dt.Rows[0]["F_brlb"].ToString().Trim();
                    if (brlb == "住院") brlb = "I";
                    else brlb = "O";

                 string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();

                string brbh = dt.Rows[0]["F_brbh"].ToString().Trim();
                        string sqxh = dt.Rows[0]["F_sqxh"].ToString().Trim();
                        if (brbh.Trim() != "" && sqxh.Trim() != "")
                        {
                            string SendReturnfee_xml = "<root><patientType>" + brlb + "</patientType><patientId>" + brbh + "</patientId><operationType>2</operationType>"
                                + "<executeDeptCode>JC06</executeDeptCode><executeDoctorCode>" + CZYGH + "</executeDoctorCode><applyNo>" + sqxh + "</applyNo></root>";

                            if (debug == "1")
                                MessageBox.Show(SendReturnfee_xml);

                            string rtn_msg2 = rtn_CallInterface("XML", "SendReturnfee", SendReturnfee_xml, "", WebUrl, debug);
                            MessageBox.Show(rtn_msg2);
                        }
                        else
                        {
                            MessageBox.Show("病人申请序号为空或病人ID号为空，不能撤销扣费");
                        }

                }
                else
                {
                    MessageBox.Show("未查到对应的数据！");
                }
                return "0";

            }
           #endregion

           #region 提取信息
              string OSQ = ""; string BBLB_XML = "";string bbmc="";
                if (Sslbx == "就诊号" || Sslbx == "新门诊提取" || Sslbx == "门诊提取")
                {
                    OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|"
                          + DateTime.Now.ToString("yyyyMMddHHmmss") + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|"
                          + DateTime.Now.ToString("MMddHHmmss") + "|||^RD|" + Ssbz + "|ORD|^申请单号||" + "\rQRF|" + xtdm + "|||O|||||1^^^"
                          + DateTime.Now.AddDays(-15).ToString("yyyyMMdd") + "^" + DateTime.Now.ToString("yyyyMMdd") + "|";

                }
                else
                    if (Sslbx == "住院号" || Sslbx == "新住院提取" || Sslbx == "住院提取")
                    {
                        OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|"
                            + DateTime.Now.ToString("yyyyMMddHHmmss")+ "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|"
                            + DateTime.Now.ToString("MMddHHmmss") +"|||^RD|" + Ssbz + "|ORD|^申请单号||" + "\rQRF|" + xtdm + "|||I|||||1^^^" 
                            + DateTime.Now.AddDays(-15).ToString("yyyyMMdd") +"^" + DateTime.Now.ToString("yyyyMMdd") + "|";

                    }
                    else

                        if (Sslbx == "申请序号" || Sslbx == "申请号")
                        {
                            OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|" + DateTime.Now.ToString("MMddHHmmss")
                                + "|||^RD|123456|ORD|" + Ssbz + "^申请单号||" + "\rQRF|" + xtdm + "|||I|||||1^^^" + DateTime.Now.AddDays(-15).ToString("yyyyMMdd")
                                + "^" + DateTime.Now.ToString("yyyyMMdd") + "|";
                        }
                        else
                            if (Sslbx == "标本条码号")
                            {
                                //获取标本列表
                               
                           
                                    string applyNo = "";
                                    string patientId = "";
                                    string visitNo = "";
                                    string visitType = "";
                                    string examItemCode = "";
                                    string examItemName = "";
                                  
                                    BBLB_XML = GetBbxx("",Ssbz, WebUrl, debug, ref applyNo, ref patientId, ref visitNo, ref visitType, ref examItemCode, ref examItemName, ref  bbmc);


                                    if (BBLB_XML == "")
                                    {
                   
                                        MessageBox.Show("未能获取标本数据");
                                        return "0";
                                    }
                                    if (applyNo == "")
                                    {
                                       // MessageBox.Show("获取标本信息失败");
                                        MessageBox.Show("未能获取标本数据");
                                        return "0";
                                    }

                                    OSQ = "MSH|^~\\&|GMS||HIS||" + DateTime.Now.ToString("yyyyMMddHHmmss") + "||OSQ^Q06|" + DateTime.Now.ToString("yyyyMMddHHmmss")
                                + "|P|2.4" + "\rQRD|" + DateTime.Now.ToString("yyyyMMddHHmmss") + "|R|I|" + DateTime.Now.ToString("MMddHHmmss")
                                + "|||^RD|123456|ORD|" + applyNo + "^申请单号||" + "\rQRF|" + xtdm + "|||I|||||1^^^" + DateTime.Now.AddDays(-15).ToString("yyyyMMdd")
                                + "^" + DateTime.Now.ToString("yyyyMMdd") + "|";
                            }
                        else
                        {
                            MessageBox.Show("无此识别类型" + Sslbx);
                            return "0";
                        }



                if (debug == "1")
                    log.WriteMyLog("获取" + Sslbx +":"+Ssbz + "的数据");


                string rtn_msg = rtn_CallInterface("HL7v2", "QueryGmsApply", OSQ,yhmc,WebUrl,debug);


                if (rtn_msg == "-1")
                    return "0";
               

                if (rtn_msg.Contains("error") || !rtn_msg.Contains("|"))
                {
                    readhl7 r7 = new readhl7();
                    int xy = 0;
                    r7.Adt01(rtn_msg, ref xy);
                    if (r7.MSA[1].Trim() != "AA")
                    {
                        MessageBox.Show(r7.MSA[3].Trim());
                    }
                    else
                        MessageBox.Show(rtn_msg);
                    return "0";
                }
                else
                {
                    //申请单
                    return jxhl7(rtn_msg, Ssbz, Sslbx, isbrxx, WebUrl, debug, BBLB_XML,bbmc);
                }
                #endregion
              
            }


            public static string rtn_CallInterface(string format, string serverName, string msgBody, string callOperator, string WebUrl,string debug)
        {

            string certificate = ZGQClass.getSZ_String("JK_ZGQ", "certificate", "5lYdPpiVdi0CxHKEhy3kqbzNlsXgNKZb");
            
            //集成平台地址

            xm1yweb.WSInterface wsif = new LGHISJKZGQ.xm1yweb.WSInterface();
           // XMDYYY_WEB.WSInterface wsif = new LGHISJKZGQ.XMDYYY_WEB.WSInterface();
       
            string msgHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><serverName>" + serverName.Trim() + "</serverName>"
              + "<format>" + format + "</format><callOperator>" + callOperator.Trim() + "</callOperator><certificate>" + certificate + "</certificate></root>";


            if (debug == "1")
                log.WriteMyLog("入参：msgHeader:" + msgHeader + "\r\nmsgBody" + msgBody);


                
             try
            {
                 if (WebUrl.Trim() != "")
                wsif.Url=WebUrl;
         
                  string rtn= wsif.CallInterface(msgHeader, msgBody);

                  if (debug == "1")
                      log.WriteMyLog("返回：" + rtn );
                  return rtn;
            }
            catch (Exception ee)
            {
                MessageBox.Show("连接webservice异常："+ee.Message);
                return "-1";
            }
        }

            private static string jxhl7(string rtn_msg, string Ssbz, string Sslbx, string isbrxx,string WebUrl,string debug,string BBXml,string BBmc)
        {
               string yh = f.ReadString("yh", "yhmc", "0");
            int xh = 0;
            PT_XML px = new PT_XML();
            readhl7 r7 = new readhl7();
            int count = 0;
            try
            {
            r7.Adt01(rtn_msg, ref count);

                if (r7.MSA[1].Trim() == "AA")
                {
                    try
                    {
        
                if (count > 1)
                {
                    DataTable dt = new DataTable();
                    DataColumn dc0 = new DataColumn("序号");
                    dt.Columns.Add(dc0);
                    DataColumn dc1 = new DataColumn("申请序号");
                    dt.Columns.Add(dc1);
                    DataColumn dc2 = new DataColumn("姓名");
                    dt.Columns.Add(dc2);
                    DataColumn dc3 = new DataColumn("病人类别");
                    dt.Columns.Add(dc3);
               
                    DataColumn dc4 = new DataColumn("送检科室");
                    dt.Columns.Add(dc4);
                    DataColumn dc5 = new DataColumn("送检医生");
                    dt.Columns.Add(dc5);
                 
                    DataColumn dc6 = new DataColumn("医嘱项目");
                    dt.Columns.Add(dc6);
                    DataColumn dc7 = new DataColumn("费用");
                    dt.Columns.Add(dc7);
                    DataColumn dc8 = new DataColumn("标本名称");
                    dt.Columns.Add(dc8);
                   

                    for (int x = 0; x < count; x++)
                    {
                        DataRow dr1 = dt.NewRow();
                        dt.Rows.Add(dr1);
                        dt.Rows[x][0] = x;
                     
                        //申请序号
                        dt.Rows[x][1] = r7.ORC[x, 2].Trim();
                        //姓名
                        dt.Rows[x][2] = r7.PID[5].Split('^')[0].Trim();
                        //病人类型
                        dt.Rows[x][3] = r7.QRF[4].Trim();
                        if (dt.Rows[x][3].ToString() == "I")
                            dt.Rows[x][3] = "住院";
                        else 
                            dt.Rows[x][3] = "门诊";

                        //科室
                        dt.Rows[x][4] = r7.ORC[xh, 13].Trim().Split('^')[0];
                        //医生
                        dt.Rows[x][5] = r7.ORC[x, 12].Trim();
                      //项目
                        dt.Rows[x][6] = r7.OBR[x, 4].Trim();
                        //费用
                        dt.Rows[x][7] = r7.OBR[x, 23].Trim();
                        //标本名称
                       string bbmc = r7.OBR[x, 15].Trim();
                        if (bbmc.Trim() == "^") bbmc = "";
                        dt.Rows[x][8] = bbmc;
                    
                           }
                    Frm_FJSFYBJY ffj = new Frm_FJSFYBJY(dt,"30^80^80^40^120^100^250^70^150");
                    ffj.ShowDialog();
                    if (ffj.xh == "")
                    {
                        MessageBox.Show("未选择病人检查记录");
                        return "0";
                    }
                    xh = int.Parse(ffj.xh);

                }

                px.myDictionary["姓名"] = r7.PID[5].Split('^')[0].Trim();

                string xb = r7.PID[8].Trim();
                if (xb == "F") xb = "女";
                else if (xb.Trim() == "M") xb = "男";
                else xb = "女";
                px.myDictionary["性别"] = xb;

                string brlb = r7.QRF[4].Trim();
                if (brlb == "I") brlb = "住院";
                else brlb = "门诊";

                px.myDictionary["病人类别"] = brlb;

                px.myDictionary["出生日期"] = r7.PID[7].Trim();
                if (r7.PID[13].Trim()!="")
                px.myDictionary["地址"] = r7.PID[11].Trim() ;
               if (r7.PID[11].Trim() != "")
                px.myDictionary["电话"] ="^"+r7.PID[13].Trim();

                        string  hy=r7.PID[15].Trim();
                         switch (hy)
                        {
                            case "D": hy = "离婚"; break;
                            case "M": hy = "已婚"; break;
                                  case "W": hy = "丧偶"; break;
                                  case "V": hy = "未婚"; break;
                            default: hy = ""; break;

                        }
                        px.myDictionary["婚姻"] = hy;




                        foreach (string pid3 in r7.PID[3].Trim().Split('~'))
                        {
                            try
                            {
                                //身份证号
                                if (pid3.Split('^')[4] == "PN")
                                    px.myDictionary["身份证号"] = pid3.Split('^')[0].Trim();

                            }
                            catch
                            {
                            }
                            try
                            {
                                //PI  patientid
                                if (pid3.Split('^')[4] == "PI")
                                    px.myDictionary["病人编号"] = pid3.Split('^')[0].Trim();
                            }
                            catch
                            {
                            }
                            try
                            {
                                //病案号
                                if (pid3.Split('^')[4] == "MR")
                                {
                                    px.myDictionary["民族"] = pid3.Split('^')[0].Trim();

                                 
                                }
                            }
                            catch
                            {
                            }
                         
                            try
                            {

                                //医保号
                                if (pid3.Split('^')[4] == "VN")
                                    px.myDictionary["就诊ID"] = pid3.Split('^')[0].Trim();
                            }
                            catch
                            {
                            }

                        }

                        if (px.myDictionary["民族"].Trim() == "")
                            px.myDictionary["民族"] = px.myDictionary["就诊ID"].Substring(0, px.myDictionary["就诊ID"].Length-3);

                        string bah = px.myDictionary["民族"];
                        if (brlb == "门诊")
                            px.myDictionary["门诊号"] = bah;
                        if (brlb == "住院")
                        {
                            px.myDictionary["住院号"] = bah;
                          
                        }

               
                px.myDictionary["床号"] = r7.NTE[3].Trim();

                px.myDictionary["送检科室"] = r7.ORC[xh, 13].Trim().Split('^')[0];

                px.myDictionary["病区"] = r7.ORC[xh, 13].Trim().Split('^')[0];


                px.myDictionary["申请序号"] = r7.ORC[xh, 2].Trim();
                px.myDictionary["送检医生"] = r7.ORC[xh, 12].Trim().Split('^')[1].Trim();

                string sqx = r7.OBR[0, 2].Trim();

                px.myDictionary["医嘱项目"] = r7.OBR[xh, 4].Trim();


                string bbmc1 = r7.OBR[xh, 15].Trim();
                if (bbmc1.Trim() == "^") bbmc1 = "";
                px.myDictionary["标本名称"] = bbmc1;

                px.myDictionary["收费"] = "";// r7.OBR[xh, 23].Trim();
                //try
                //{
                //    px.myDictionary["临床病史"] = r7.OBR[xh, 46].Trim().Split('~')[1].Replace("2^", "").Trim();
                //}
                //catch
                //{
                //    px.myDictionary["临床病史"] = "";
                //}
               
                 foreach (string lczd in r7.OBR[xh, 46].Trim().Split('~'))
                 {
                     //lczd
                     try
                     {
                         if (lczd.Split('^')[0] == "6")
                         {
                             px.myDictionary["临床诊断"] = lczd.Split('^')[1];
                         }
                     }
                     catch
                     {
                     }
                     //主诉
                     try
                     {
                         if (lczd.Split('^')[0] == "1")
                         {
                             px.myDictionary["临床病史"] =px.myDictionary["临床病史"]+ lczd.Split('^')[1];
                         }
                     }
                     catch
                     {
                     }
                     // 病史
                     try
                     {
                         if (lczd.Split('^')[0] == "2")
                         {
                             px.myDictionary["临床病史"] =px.myDictionary["临床病史"]+ lczd.Split('^')[1];
                         }
                     }
                     catch
                     {
                     }
                 }
               
                if (px.myDictionary["年龄"].Trim() == "")
                    px.myDictionary["年龄"] = ZGQClass.CsrqToAge(px.myDictionary["出生日期"]);

                string exep = "";
             //   MessageBox.Show(px.rtn_XML(ref exep));

               //获取标本列表
                if (BBXml.Trim() == "")
                {
                    if (tqbblb == "1")
                    {
                        string applyNo = "";
                        string patientId = "";
                        string visitNo = "";
                        string visitType = "";
                        string examItemCode = "";
                        string examItemName = "";
                        BBXml = GetBbxx(px.myDictionary["申请序号"].Trim(), "", WebUrl, debug, ref applyNo, ref patientId, ref visitNo, ref visitType, ref examItemCode, ref examItemName, ref  BBmc);
                       

                    }
                }

                if (BBmc != "")
                {
                    px.myDictionary["标本名称"] = BBmc;
                }
        
                return px.rtn_XML(BBXml, ref exep);
                }
                 catch(Exception e2)
                {
                    MessageBox.Show(e2.Message);
                    return "0";
                }
            }
            else
            {
                  //取基本信息
                if (isbrxx == "1")
                {
                    if(debug=="1")
                    log.WriteMyLog("未取得申请单信息，提取病人基本信息");

                    if (Sslbx.Trim() == "住院号" || Sslbx.Trim() == "就诊号")
                    {
                        string rtn_msg2 = "";
                        if (Sslbx.Trim() == "住院号")
                        {

                            //if (isbrxx.Trim() != "1")
                            //{
                            //    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
                            //    return "0";
                            //}
                            string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><visitNo></visitNo><inpNo>" + Ssbz + "</inpNo><patientId></patientId><wardCode></wardCode><deptCode></deptCode></root>";
                            rtn_msg2 = rtn_CallInterface("XML", "GetPatientInHospital", XML2, "", WebUrl, debug);
                        }
                        else
                        {

                            //if (isbrxx.Trim() != "1")
                            //{
                            //    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
                            //    return "0";
                            //}

                            string XML2 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><icCardNo>" + Ssbz + "</icCardNo><executeDept></executeDept></root>";
                            rtn_msg2 = rtn_CallInterface("XML", "GetDispPatientInfo", XML2, "", WebUrl, debug);
                        }



                        if (rtn_msg2 == "")
                            return "0";


                        readhl7 rr7 = new readhl7();
                        int xy = 0;
                        rr7.Adt01(rtn_msg2, ref xy);
                        if (rr7.MSA[1].Trim() != "AA")
                        {
                            MessageBox.Show(rr7.MSA[3].Trim());

                            return "0";
                        }
                        else
                        {
                            XmlNode xmlok = null;
                            XmlDocument xd = new XmlDocument();
                            try
                            {
                                xd.LoadXml(rtn_msg2);
                                xmlok = xd.SelectSingleNode("/root/patients");
                                if (xmlok.InnerXml.Trim() != "")
                                {
                                    DataSet ds1 = new DataSet();
                                    StringReader sr = new StringReader(xmlok.InnerXml);
                                    XmlReader xr = new XmlTextReader(sr);
                                    ds1.ReadXml(xr);

                                    px.myDictionary["姓名"] = ds1.Tables[0].Rows[0]["patientName"].ToString().Trim();
                                    string xb = ds1.Tables[0].Rows[0]["patientSex"].ToString().Trim();
                                    if (xb == "1") xb = "女";
                                    else if (xb.Trim() == "0") xb = "男";
                                    else xb = "";
                                    px.myDictionary["性别"] = xb;
                                    if (Sslbx.Trim() == "住院号")
                                    {
                                        px.myDictionary["病人类别"] = "住院";
                                        px.myDictionary["就诊ID"] = ds1.Tables[0].Rows[0]["visitCount"].ToString().Trim();
                                        px.myDictionary["住院号"] = ds1.Tables[0].Rows[0]["visitNo"].ToString().Trim();
                                        px.myDictionary["床号"] = ds1.Tables[0].Rows[0]["bedCode"].ToString().Trim();
                                        px.myDictionary["病区"] = ds1.Tables[0].Rows[0]["wardName"].ToString().Trim();
                                    }
                                    else
                                    {
                                        px.myDictionary["病人类别"] = "门诊";
                                        px.myDictionary["门诊号"] = ds1.Tables[0].Rows[0]["icCardNo"].ToString().Trim();
                                        px.myDictionary["病区"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();
                                    }
                                    if (ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() != "")
                                    {

                                        try
                                        {
                                            px.myDictionary["年龄"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim() + ds1.Tables[0].Rows[0]["ageUnit"].ToString().Trim();
                                        }
                                        catch
                                        {
                                            px.myDictionary["年龄"] = ds1.Tables[0].Rows[0]["patientAge"].ToString().Trim();
                                        }
                                    }
                                    px.myDictionary["地址"] = "电话：" + ds1.Tables[0].Rows[0]["telephone"].ToString().Trim();
                                    px.myDictionary["电话"] = "^地址：";
                                    px.myDictionary["病人编号"] = ds1.Tables[0].Rows[0]["patientId"].ToString().Trim();
                                    px.myDictionary["送检科室"] = ds1.Tables[0].Rows[0]["deptName"].ToString().Trim();

                                    px.myDictionary["申请序号"] = "";
                                    px.myDictionary["送检医生"] = ds1.Tables[0].Rows[0]["doctorInCharge"].ToString().Trim().Split('/')[0].Trim();
                                    px.myDictionary["费别"] = ds1.Tables[0].Rows[0]["rateTypeName"].ToString().Trim();
                                    try
                                    {
                                        px.myDictionary["临床诊断"] = ds1.Tables[0].Rows[0]["diagnosisname"].ToString().Trim();
                                    }
                                    catch
                                    {
                                        px.myDictionary["临床诊断"] = "";
                                    }

                                    string exep = "";
                                    return px.rtn_XML(ref exep);

                                }
                                else
                                {
                                    MessageBox.Show("未能查询到病人记录");
                                    return "0";
                                }
                            }
                            catch (Exception ee)
                            {
                                MessageBox.Show("XML解析错误:" + ee.Message);
                                log.WriteMyLog(rtn_msg2 + "--" + ee.Message);
                                return "0";
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(r7.MSA[1].Trim() + "|" + r7.MSA[3].Trim());
                }


                return "0";
            }
            }
            catch(Exception  ee)
            {
                MessageBox.Show(ee.Message);
                return "0";

            }
        }

            public static string  GetBbxx(string sqxh, string bbtmh, string WebUrl, string debug, ref string applyNo, ref string patientId, ref string visitNo, ref string visitType, ref string examItemCode, ref string examItemName,ref string bbmc)
            {
                
                string  BBLB_XML = "";

                string OSQ = "<?xml version=\"1.0\" encoding=\"utf-8\"?><root><applyNo>" + sqxh + "</applyNo>"
                    + "<specimenCode>" + bbtmh + "</specimenCode></root>";

          
                string rtn_msg = rtn_CallInterface("XML", "GetGmsSpecimenInfo", OSQ, yhmc, WebUrl, debug);
       
                if (rtn_msg.Trim() != "" &&  rtn_msg != "-1")
                {
                    XmlNode xmlok = null;
                    XmlDocument xd = new XmlDocument();
                    XmlNodeList xnl = null;
                    try
                    {
                        xd.LoadXml(rtn_msg);
                        xmlok = xd.SelectSingleNode("/root/returnContents/returnContent");
                        if (xmlok.InnerXml.Trim() == "")
                        {
                            //MessageBox.Show("未能获取标本数据");
                            return "";
                        }
                        else
                        {

                            applyNo = xmlok["applyNo"].InnerText;
                            patientId = xmlok["patientId"].InnerText;
                            visitNo = xmlok["visitNo"].InnerText;
                            visitType = xmlok["visitType"].InnerText;
                            examItemCode = xmlok["examItemCode"].InnerText;
                            examItemName = xmlok["examItemName"].InnerText;
                            xnl = xd.SelectNodes("/root/returnContents/returnContent/specimen");
                            if (xnl.Count > 0)
                            {

                                for (int i = 0; i < xnl.Count; i++)
                                {

                                    BBLB_XML = BBLB_XML + "<row ";
                                    BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + xnl[i]["subNo"].InnerText + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + xnl[i]["specimenCode"].InnerText.Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + xnl[i]["specimenName"].InnerText.Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + xnl[i]["remark"].InnerText + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + xnl[i]["isolatedTime"].InnerText.Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + xnl[i]["fixationTime"].InnerText.Trim() + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + yhmc + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "已接收" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                    BBLB_XML = BBLB_XML + "/>";

                                    if (bbmc == "")
                                        bbmc = xnl[i]["specimenName"].InnerText.Trim().Trim();
                                    else
                                        bbmc = bbmc + ";" + xnl[i]["specimenName"].InnerText.Trim().Trim();
                                }
                                BBLB_XML = "<BBLB>" + BBLB_XML + "</BBLB>";
                            }
                        }
                    }
                    catch(Exception  ee2)
                    {
                        MessageBox.Show("获取标本数据异常："+ee2.Message);
                        log.WriteMyLog("获取标本数据异常："+ee2.Message);
                        return "";
                    }
                
                }

                return BBLB_XML;
            }
     }

}
