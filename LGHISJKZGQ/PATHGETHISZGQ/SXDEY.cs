using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.IO;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    class SXDEY
    {
        /// <summary>
        /// 山西大二院，西门子，webservices
        /// </summary>
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        

        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {

                string in_xml = "";

                if (Sslbx == "就诊卡号")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno>" + Ssbz + "</cardno>" +
                " <inpatno></inpatno>" +
                "<outpatno></outpatno>" +
                "<appid></appid></operation>"+
                "<modality>PS</modality>";

                }else
                if (Sslbx == "住院号")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno></cardno>" +
                " <inpatno>" + Ssbz + "</inpatno>" +
                "<outpatno></outpatno>" +
                "<appid></appid></operation>"+
                "<modality>PS</modality>";
                }
                else
                if (Sslbx == "门诊号")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno></cardno>" +
                " <inpatno></inpatno>" +
                "<outpatno>" + Ssbz + "</outpatno>" +
                "<appid></appid></operation>"+
                 "<modality>PS</modality>";
                }
                else
                if (Sslbx == "申请ID")
                {
                    in_xml = "<?xml version=\"1.0\" encoding=\"gb2312\"?>" +
                "<operation name =\"getExamOrders\">" +
                "<cardno></cardno>" +
                " <inpatno></inpatno>" +
                "<outpatno></outpatno>" +
                "<appid>" + Ssbz + "</appid></operation>"+
                 "<modality>PS</modality>";
                }
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    return "0";
                }

                 if (in_xml.Trim() == "")
                {   MessageBox.Show("无此" + Sslbx);
                      return "0";
                }

                SXDEYWeb.MatrixService dey = new LGHISJKZGQ.SXDEYWeb.MatrixService();
               
                 
                    string Result = "";
                    string weburl = "";
                    weburl = f.ReadString(Sslbx, "URL", "");//从sz.ini 中获取webservice的地址，要求不写死
                    if (weburl != "")
                        dey.Url = weburl;


                    try
                    {
                        Result = dey.getExamOrders(in_xml); //获取门诊病人信息 ，参数2（ 0表示门诊，1表示住院）
                       
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("调用异常："+ee.ToString());
                      log.WriteMyLog("接收参数:" + Sslbx + "," + Ssbz + "," + ee.ToString());
                        return "0";
                    }

                    //xml解析
                    if (Debug=="1")
                    MessageBox.Show("返回xml："+Result);
             
                    if (Result.Trim() == "" || Result==null)
                    {
                     MessageBox.Show("未查到数据记录！");
                     log.WriteMyLog("未查到数据记录");

                     return "0";
                    }
                    else
                    {
              
                        //DataSet ds1 = new DataSet();

                        //XmlDocument xd = new XmlDocument();
                        //if (Debug == "1")
                        //  MessageBox.Show(xmlstr);
                        //try
                        //{

                        //    StringReader sr = new StringReader(Result);
                        //    XmlReader xr = new XmlTextReader(sr);
                        //    ds1.ReadXml(xr);
                        //}
                        //catch(Exception  e2)
                        //{ 
                        //    MessageBox.Show("XML解析异常;"+e2.ToString());
                        //    log.WriteMyLog("XML解析异常"+e2.ToString()); 
                        //    return "0";
                        //}

                        XmlNode xmlok_DATA = null;
                        XmlDocument xdt = new XmlDocument();
                        try
                        {
                            xdt.LoadXml(Result);
                            xmlok_DATA = xdt.SelectSingleNode("/operation/orders");
                        }
                        catch (Exception xmlok_e)
                        {
                            MessageBox.Show("解析DATA异常：" + xmlok_e.ToString());

                            return "0";
                        }
                   
                        if (!xmlok_DATA.HasChildNodes)
                        {
                            MessageBox.Show("未找到对应的记录！");
                            return "0";
                        }
                        if (!xmlok_DATA.FirstChild.HasChildNodes)
                        {
                            MessageBox.Show("未找到对应的记录！");
                            return "0";
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
                            MessageBox.Show("转dataset异常：" + eee.ToString());
                            log.WriteMyLog("转dataset异常:" + eee);
                            return "0";
                        }
                  
                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            MessageBox.Show("未查到相应的记录！");
                            return "0";
                        }
                  
                        DataTable dt = new DataTable();
                        if (ds.Tables[0].Rows.Count >1)
                        {
                             string Columns = f.ReadString(Sslbx, "Columns", "");//显示的项目
                             string Col = f.ReadString(Sslbx, "RowFilter", ""); //选择条件的项目


                                FRM_SP_SELECT yc = new FRM_SP_SELECT(ds.Tables[0],-1, Columns, Col);
                                yc.ShowDialog();
                                string string1 = yc.F_STRING[0];
                                string string2 = yc.F_STRING[1];
                                string string3 = yc.F_STRING[2];
                                string string4 = yc.F_STRING[3];

                                if (string1.Trim() == "" && string2.Trim() == "" && string3.Trim() == "" && string4.Trim() == "")
                                {
                                    MessageBox.Show("未选择记录");
                                    return "0";
                                }
                                DataView view = new DataView();
                                view.Table = ds.Tables[0];


                                string odr = "" + ds.Tables[0].Columns[0].ColumnName + "='" + string1 + "'  and  " + ds.Tables[0].Columns[1].ColumnName + "='" + string2 + "'  and  " + ds.Tables[0].Columns[2].ColumnName + "='" + string3 + "' and  " + ds.Tables[0].Columns[3].ColumnName + "='" + string4 + "'";

                                if (Col.Trim() != "")
                                {
                                    string[] colsss = Col.Split(',');
                                    odr = "" + colsss[0] + "='" + yc.F_STRING[0] + "'";
                                    if (colsss.Length > 1)
                                    {
                                        for (int i = 1; i < colsss.Length; i++)
                                        {
                                            if (i < 4)
                                                odr = odr + " and  " + colsss[i] + "='" + yc.F_STRING[i] + "' ";
                                        }
                                    }
                                }
                                view.RowFilter = odr;
                                dt = view.ToTable();
                        }
                        else
                        {
                            dt = ds.Tables[0];
                        }
                     //   dt = ds.Tables[0];
                        string exp = "";
                        //-返回xml----------------------------------------------------
                        try
                        {

                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            try
                            {
                                xml = xml + "病人编号=" + (char)34 + dt.Rows[0]["patid"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "就诊ID=" + (char)34 + dt.Rows[0]["cardno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "就诊ID=" + (char)34 +"" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "申请序号=" + (char)34 + dt.Rows[0]["appid"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "门诊号=" + (char)34 + dt.Rows[0]["outpatno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                            try
                            {
                                xml = xml + "住院号=" + (char)34 + dt.Rows[0]["inpatno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                            try
                            {
                                xml = xml + "姓名=" + (char)34 + dt.Rows[0]["name"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                         
                            try
                            {
                                   string xb=dt.Rows[0]["sex"].ToString().Trim();
                                if(xb.Trim()=="F") xb="女";
                                else   if(xb.Trim()=="M") xb="男";
                                else  xb="";
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
                                //string nl="";
                            string CSRQ = dt.Rows[0]["birthdate"].ToString().Trim();
                          
                                    string datatime=DateTime.Today.Date.ToString();

                                    if (CSRQ != "")
                                    { 
                                        int Year = DateTime.Parse(datatime).Year - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Year;
                                        int Month = DateTime.Parse(datatime).Month - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Month;
                                        int day = DateTime.Parse(datatime).Day - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).Day;
                                      
                                        if (Year>=3)
                                        {

                                            if (Month>0)
                                               xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                            if (Month < 0)
                                               xml = xml + "年龄=" + (char)34 + (Year-1) + "岁" + (char)34 + " ";
                                            if (Month == 0)
                                            {
                                               if(day>=0)
                                                   xml = xml + "年龄=" + (char)34 + Year + "岁" + (char)34 + " ";
                                               else
                                                 xml = xml + "年龄=" + (char)34 + (Year-1) + "岁" + (char)34 + " ";
                                            }
                                        }
                                        if (Year > 0 && Year < 3)
                                        {
                                            if((Year - 1)== 0)
                                            {
                                                if (Month <= 0)
                                                {
                                                    if (day > 0)
                                                        xml = xml + "年龄=" + (char)34 + (12 + Month) + "月" + day + "天" + (char)34 + " ";
                                                    else
                                                        xml = xml + "年龄=" + (char)34 + (12 + Month - 1) + "月" + (30 + day) + "天" + (char)34 + " ";
                                                }
                                                else
                                                    xml = xml + "年龄=" + (char)34 + Year + "岁" + (Month) + "月" + (char)34 + " ";
                                            }
                                            else
                                            {
                                            if (Month > 0)
                                                xml = xml + "年龄=" + (char)34 +Year+"岁"+ Month + "月"+ (char)34 + " ";
                                            else
                                                xml = xml + "年龄=" + (char)34 + (Year-1) + "岁" + (12 + Month) + "月" + (char)34 + " ";
                                           
                                            }

                                        }
                                        if (Year== 0)
                                        {
                                            int day1 = DateTime.Parse(datatime).DayOfYear - DateTime.ParseExact(CSRQ, "yyyyMMdd", null).DayOfYear;

                                         int m = day1 / 30;
                                         int d = day1 % 30;
                                         xml = xml + "年龄=" + (char)34 + m +"月"+ d+"天" + (char)34 + " ";
                                        }
                                    }
                                    

                              //  xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {

                                string hy=dt.Rows[0]["maritalstatus"].ToString().Trim();
                                if(hy=="M") hy="已婚";
                                else if(hy=="B") hy="未婚";
                                   else if(hy=="D") hy="离婚";
                                   else if(hy=="W") hy="丧偶";
                                else hy="";
                                xml = xml + "婚姻=" + (char)34 + hy + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "地址=" + (char)34 + dt.Rows[0]["address"].ToString().Trim() + (char)34 + "   ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "电话=" + (char)34 + dt.Rows[0]["phoneno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "病区=" + (char)34 + dt.Rows[0]["ward"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "床号=" + (char)34 + dt.Rows[0]["bedno"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "身份证号=" + (char)34 + dt.Rows[0]["idno"].ToString().Trim() + (char)34 + " ";
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
                                xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["deptname"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";

                            }
                            //----------------------------------------------------------

                            try
                            {
                                xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["doctorname"].ToString().Trim() + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------

                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "标本名称=" + (char)34 + dt.Rows[0]["BodySite"].ToString().Trim() + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "医嘱项目=" + (char)34 + dt.Rows[0]["itemno"].ToString().Trim()+"^"+dt.Rows[0]["itemname"].ToString().Trim() + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            //----------------------------------------------------------
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            //----------------------------------------------------------
                            try
                            {
                                xml = xml + "费别=" + (char)34 +"" + (char)34 + " ";
                            }
                            catch (Exception ee)
                            {
                                exp = exp + ee.ToString();
                                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            }
                            //----------------------------------------------------------
                            try
                            {
                                string brlb=dt.Rows[0]["patsource"].ToString().Trim();
                                 if(brlb=="I") brlb="住院";
                                else if(brlb=="O") brlb="门诊";
                                   else if(brlb=="E") brlb="体检";
                                else brlb="";


                                xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
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
                                xml = xml + "<临床诊断><![CDATA[" + dt.Rows[0]["clinicdiag"].ToString().Trim() + "]]></临床诊断>";
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
                         
                                MessageBox.Show("提取信息出错,"+e.ToString());
                            log.WriteMyLog("xml解析错误---" + e.ToString());
                            return "0";
                        }
                    }
            
                    
             }
            else
            {
                MessageBox.Show("无此" + Sslbx);
                if (Debug == "1")
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");

             //   return "0";
            } return "0";

    }
    }
}
