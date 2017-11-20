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
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System.Data.OleDb;

namespace LGHISJKZGQ
{
    class XYFYYY
    {
        //湘雅附一医院
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            if (Sslbx != "")
            {
                //************************************************
                //-------撤销申请单----
                //************************************************
                if (Sslbx == "撤销申请单")
                {
                    //dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet-his;UID=pathnet;PWD=4s3c2a1p", "", "");
                    xyfyyyweb.DHCPisXiangYaOne xyfy = new xyfyyyweb.DHCPisXiangYaOne();
                    string pathweburl = f.ReadString("SF", "webservicesurl", "");
                    if (pathweburl.Trim() != "")
                        xyfy.Url = pathweburl;

                    string T_exam_bill_items_sql = f.ReadString("SF", "V_exam_bill_items", "");

                    string yh =f.ReadString("yh", "yhmc", "").Replace("\0", "");

                    //if (yh.Length > 3 && yh.Length % 2 != 1)
                    //    yh = yh.Substring(0, yh.Length / 2);
                    //else
                    //    yh = "";

                    string Operator = yh;
                    string Status = "301";  //:取消申请单301
                    try
                    {
                        //判断申请单信息表中是否有该条记录
                        DataTable sqdxx = new DBdata().select_sql("select * from  Examapply  where  CheckFlow='" + Ssbz.Trim() + "'  and  (jszt='已发送' or  jszt='已执行'  or  jszt='已作废')");
                        if (sqdxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("病理申请单信息表中，无此申请号信息，请确认！！！");
                            return "0";
                        }

                        if (sqdxx.Rows[0]["jszt"].ToString().Trim() == "已作废")
                        {
                            MessageBox.Show("此申请单已作废！！！");
                            return "0";
                        }



                        dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                       DataTable  dt_jxcc=aa.GetDataTable("select * from   T_JCXX where F_SQXH='" + Ssbz.Trim() + "'","jcxx");
                       if (dt_jxcc.Rows.Count>0)
                        {
                            MessageBox.Show("此申请号在病理系统中已登记，请先取消登记再撤销！！！\n" + "姓名：" + dt_jxcc.Rows[0]["F_xm"].ToString() + "\n病理号：" + dt_jxcc.Rows[0]["F_BLH"].ToString());
                            return "0";
                        }



                        //判断此申请单是门诊还是住院

                        string brlb = sqdxx.Rows[0]["InOrOut"].ToString();

                        if (sqdxx.Rows[0]["kdxt"].ToString() == "PIS")
                        { 
                           //内镜系统开申请单
                         DBdata db = new DBdata();

                          // 查询收费记录，有收费项目的不能撤销
                            string EXAM_NO = "";
                            try
                            {
                                string exam_appoints_id_str = "select *  from emr.exam_appoints_id where CHECK_FLOW='" + Ssbz.Trim() + "' ";
                                if (brlb == "门诊")
                                    exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + Ssbz.Trim() + "' ";
                               DataTable exam_appoints_id = db.select_orcl(exam_appoints_id_str, "获取EXAM_NO号");
                               if (exam_appoints_id.Rows.Count > 0)
                                   EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();
                               else
                               {
                                   MessageBox.Show("撤销失败，不存在EXAM_NO号！"); return "0";
                               } 
                            }
                            catch
                            {
                                MessageBox.Show("撤销失败，抛出异常"); return "0";
                            }


                            DataTable dt_sfmx = db.select_orcl("select * from exam.exam_bill_items  where  exam_no='" + EXAM_NO + "' " + T_exam_bill_items_sql + "   and  Performed_By='90' ", "获取收费明细");
                          if (dt_sfmx.Rows.Count > 0)
                          {
                              MessageBox.Show("此申请单为内镜开单系统所开,撤销申请时须退出所有的收费项目！"); 
                              return　"0";
                          }
                          else
                          {
                              int x= db.Execute_sql("delete  Examapply   where CheckFlow='" + Ssbz.Trim().Trim() + "'  and (jszt='已发送' or jszt='已接收') ");
                             if (x > 0)
                                 MessageBox.Show("撤销申请单完成"); 
                             else
                                 MessageBox.Show("撤销申请单失败"); 
                              return　"0";

                          }
          


                        }
                        else
                        {
                            //正常开单
                            string funName = "ExamStatus";
                            if (brlb == "住院")
                                funName = "ExamStatusIp";

                            string ExamStatus_XML = "<Request><ExamStatus>"
                                + "<CheckFlow>" + Ssbz + "</CheckFlow>"
                               + "<ExamGroup></ExamGroup><ScheduledDate></ScheduledDate><Note></Note>"
                               + "<Operator>" + Operator + "</Operator>"
                               + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate><Status>" + Status + "</Status>"
                               + "<Notice></Notice><ExamAddress></ExamAddress></ExamStatus></Request>";




                            string rtn_Status = "";
                            //**********************************
                            //---执行撤销命令---------------
                            try
                            {
                                rtn_Status = xyfy.DhcService(funName, ExamStatus_XML);
                            }
                            catch (Exception e)
                            {
                                log.WriteMyLog("撤销申请状态错误，" + e.ToString());
                                MessageBox.Show("撤销申请状态错误,可能是网络连接问题");
                                return "0";
                            }
                            //**********************************
                            //-----解析返回值xml-------------
                            //**********************************
                            if (rtn_Status.Trim() == "")
                            {
                                MessageBox.Show("撤销申请失败，返回值为空");
                                log.WriteMyLog("撤销申请错误，返回值为空");
                                return "0";
                            }
                            //------
                            try
                            {

                                XmlDataDocument xd = new XmlDataDocument();
                                xd.LoadXml(rtn_Status);
                                XmlNode xn = xd.SelectSingleNode("/Response");

                                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                                {
                                    MessageBox.Show("撤销申请单失败！");
                                    return "0";
                                }


                                try
                                {
                                    
                                    int tn = new DBdata().Execute_sql("update  Examapply set  jszt='已作废'  where  CheckFlow='" + Ssbz + "'  and  (jszt='已发送' or  jszt='已接收')");
                                    //if (=0)
                                    MessageBox.Show("撤销申请单成功！");
                                    return "0";
                                }
                                catch
                                {
                                    MessageBox.Show("病理申请单信息表中数据删除失败，抛出异常");
                                    return "0";
                                }
                            }
                            catch (Exception xml_e)
                            {
                                MessageBox.Show("撤销申请失败，解析XML错误");
                                log.WriteMyLog("撤销申请错误，解析XML错误" + xml_e.ToString());
                                return "0";
                            }
                        }
                    }
                    catch (Exception eee)
                    {
                        MessageBox.Show("程序异常");
                        log.WriteMyLog("程序异常" + eee.ToString());
                        return "0";
                    }
                
                    //------
                }
                //************************************************
                //-------提取申请单信息，并判断是否要确认费用----
                //************************************************
            
                if (Sslbx == "申请单")
                {
                    DataTable dt_Examapply = new DataTable();
                  
                    DBdata db = new DBdata();
                    //-------提取申请单信息-----------------

                    dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_jcxx = aa.GetDataTable("select * from   T_JCXX where F_SQXH='" + Ssbz.Trim() + "'", "jcxx");
                    try
                    {

                        dt_Examapply = db.select_sql("select * from Examapply where  CheckFlow='" + Ssbz.Trim() + "'  and  (jszt='已发送' or jszt='已执行' or jszt='已作废')");
                      
                    }
                    catch (Exception ee_examapply)
                    {
                        MessageBox.Show("获取申请单信息异常，数据库连接失败！");
                        log.WriteMyLog("获取申请单信息异常" + ee_examapply.ToString());
                        return "0";
                    }

                    try
                    {
                        if (dt_Examapply.Rows.Count < 1)
                        {
                            MessageBox.Show("未找到该申请单信息");
                            log.WriteMyLog("未找到该申请单信息，获取表中行数为0");
                            return "0";
                        }
                    }
                    catch(Exception sd)
                    {
                        MessageBox.Show(sd.ToString());
                    }


                    if (dt_Examapply.Rows[0]["jszt"].ToString().Trim()=="已作废")
                        {
                            MessageBox.Show("该申请单已作废，不能提取信息。\n" + "姓名：" + dt_Examapply.Rows[0]["NAME"].ToString().Trim() + "\n申请号：" + dt_Examapply.Rows[0]["CHECKFLOW"].ToString().Trim() + "\n" + "ID:" + dt_Examapply.Rows[0]["PATIENTID"].ToString().Trim());
                          
                            return "0";
                        }


                        if (dt_jcxx.Rows.Count > 0)
                        {
                            if (MessageBox.Show("此病人已登记，请勿重复登记，是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return "0";
                            }
                        }
                  

                    //-------------------------------
                    //------判断住院病人是否计费----------------------
                    string rmb = dt_Examapply.Rows[0]["sfje"].ToString();

                    if (dt_Examapply.Rows[0]["jfbj"].ToString().Trim() != "1" && dt_Examapply.Rows[0]["INOROUT"].ToString().Trim() == "住院")
                    {

                        if (MessageBox.Show("此住院病人还未计费，是否现在计费", "收费信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                            sf.ShowDialog();
                            rmb = sf.F_sfje.ToString();
                            if (rmb == "" || rmb == "0")
                                return "0";
                        }
                        else
                        {
                            MessageBox.Show("该住院病人未确认费用，不能登记");
                            return "0";
                        }



                    }
                  
                    //------判断门诊病人是否已自动划价----------------------
                    if (dt_Examapply.Rows[0]["zdhj"].ToString().Trim()!= "1" && dt_Examapply.Rows[0]["INOROUT"].ToString().Trim() == "门诊")
                    {
                        if (MessageBox.Show("此门诊病人还未划价,可能还未交费,请确认!\n是否现在进行划价", "收费信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                            sf.ShowDialog();
                            rmb = sf.F_sfje.ToString();
                            if (rmb == "" || rmb == "0")
                                return "0";
                        }
                        else
                        {
                            MessageBox.Show("该门诊病人未划价未收费，不能登记");
                            return "0";
                        }


                    }

                    //门诊---判断是否收费
                    if (dt_Examapply.Rows[0]["INOROUT"].ToString() == "门诊")
                    {
                        DBdata db2 = new DBdata();
                        try
                        {
                            DataTable dt_exam_no = db2.select_orcl(" select exam_no  from mzemr.exam_appoints_id where CHECK_FLOW='" + dt_Examapply.Rows[0]["CHECKFLOW"].ToString().Trim() + "'", "获取exam_no");
                            if (dt_exam_no.Rows.Count > 0)
                            {
                                DataTable dt_exam_appoints_id = db2.select_orcl("select billing_attr,item_name from  exam.exam_bill_items where ordered_by='90' and performed_by='90' and exam_no='" + dt_exam_no.Rows[0]["exam_no"].ToString().Trim() + "' ", "获取收费明细");
                                if (dt_exam_appoints_id.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt_exam_appoints_id.Rows.Count; i++)
                                    {
                                        if (dt_exam_appoints_id.Rows[i]["billing_attr"].ToString() == "0")
                                        {
                                            MessageBox.Show("该病人还有收费项目未收费,不能提取信息:" + dt_exam_appoints_id.Rows[i]["item_name"].ToString());

                                            if (MessageBox.Show("是否查看收费项目", "收费信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                            {
                                                frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                                                sf.ShowDialog();
                                                rmb = sf.F_sfje.ToString();
                                                if (rmb == "" || rmb == "0")
                                                    return "0";
                                                db2.Execute_sql("update Examapply set  sfje='"+rmb+"'  where  CheckFlow='" + Ssbz.Trim() +"'");
                                                MessageBox.Show("病人有项目未交费，先交费再登记！");
                                                return "0";
                                            }
                                            else
                                            {
                                                return "0";
                                            }
                                           
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("该病人没有收费项目明细,不能提取信息");
                                    if (MessageBox.Show("是否查看收费项目", "收费信息提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        frm_sfjk sf = new frm_sfjk(Ssbz, "0");
                                        sf.ShowDialog();
                                        rmb = sf.F_sfje.ToString();
                                        if (rmb == "" || rmb == "0")
                                            return "0";
                                        db2.Execute_sql("update Examapply set  sfje='" + rmb + "'  where  CheckFlow='" + Ssbz.Trim() + "'");
                                        MessageBox.Show("病人有项目未交费，先交费再登记！");
                                        return "0";
                                    }
                                    else
                                    {
                                        return "0";
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("程序获取exam_no失败,不能提取信息");
                                return "0";
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("程序异常，异常信息：" + ee.ToString());
                            return "0";

                        }
                    }
                    ///////////////////////////////////////////////////////////////
                    //合并申请号
                    string sfhb = f.ReadString("申请单", "sfhb", "0").Replace("\0", "");
                    if (sfhb.Trim() == "1")
                    {
                        try
                        {
                            string sss = "select * from Examapply  where jszt='已执行' and   PatientID='" + dt_Examapply.Rows[0]["PatientID"].ToString() + "' and visitid='" + dt_Examapply.Rows[0]["visitid"].ToString() + "' and  inorout='" + dt_Examapply.Rows[0]["inorout"].ToString() + "' and name='" + dt_Examapply.Rows[0]["name"].ToString() + "' and deptcode='" + dt_Examapply.Rows[0]["deptcode"].ToString() + "'and  deptname='" + dt_Examapply.Rows[0]["deptname"].ToString() + "'";// and  indate>='" + DateTime.Parse(dt_Examapply.Rows[0]["indate"].ToString()).AddDays(-1).ToString() + "' and indate<='" + DateTime.Parse(dt_Examapply.Rows[0]["indate"].ToString()).AddDays(1).ToString() + "'";
                          
                            DataTable dt2 = db.select_sql(sss);
                          
                            if (dt2.Rows.Count > 0)
                            {

                                if (dt2.Rows[0]["checkflow"].ToString().Trim() !=Ssbz.Trim())
                                {


                                    if (MessageBox.Show("病人：" + dt_Examapply.Rows[0]["name"].ToString() + "，可能已送过标本组织，需要合并到之前的病理号吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                    {
                                        string blh_2 = "";
                                        for (int i = 0; i < dt2.Rows.Count; i++)
                                        {
                                            if (dt2.Rows[i]["checkflow"].ToString().Trim() != Ssbz.Trim())
                                            {

                                                DataTable dt_sqxx = aa.GetDataTable("select F_BLH,F_SQXH2 from   T_JCXX where F_SQXH='" + dt2.Rows[i]["checkflow"].ToString().Trim() + "'  and F_BGZT<>'已审核'", "sqxx");
                                                if (dt_sqxx.Rows.Count > 0)
                                                {


                                                    for (int y = 0; y < dt_sqxx.Rows.Count; y++)
                                                    {
                                                        if (dt_sqxx.Rows[y]["F_sqxh2"].ToString().Trim() == Ssbz.Trim())
                                                        {
                                                            MessageBox.Show("此病人申请号已经合并过，不能再合并，合并病理号" + dt_sqxx.Rows[y]["F_blh"].ToString().Trim());
                                                            return "0";

                                                        }
                                                        blh_2 = blh_2 + "^" + dt_sqxx.Rows[y]["F_BLH"].ToString().Trim();
                                                    }

                                                }
                                            }
                                        }
                                        //////////////////////
                                        if (blh_2.Trim().TrimStart('^').Trim() == "" || blh_2.Trim() == "")
                                        {
                                            MessageBox.Show("未查到病理号，不能合并");
                                            return "0";
                                        }
                                        xyxxshowblh ff = new xyxxshowblh(blh_2.Trim().TrimStart('^'));
                                        ff.ShowDialog();
                                        string getblh = ff.getblh;

                                        if (getblh.Trim() == "")
                                        {
                                            MessageBox.Show("病理号为空，不能合并");
                                            return "0";
                                        }
                                        else
                                        {//合并
                                            DataTable dt_bl = aa.GetDataTable("select *  from   T_JCXX where F_BLH='" + getblh.Trim() + "'", "sqxx");
                                            if (dt_bl.Rows.Count <= 0)
                                            {
                                                MessageBox.Show("获取病理号" + getblh.Trim() + "信息错误，不能合并");
                                                return "0";
                                            }

                                            decimal je_2 = decimal.Parse(dt_bl.Rows[0]["F_SF"].ToString()) + decimal.Parse(rmb);

                                            if ((aa.ExecuteSQL("update  T_JCXX  set F_SQXH2='" + Ssbz.Trim() + "',F_SF='" + je_2.ToString() + "' where  F_BLH='" + getblh.Trim() + "'")) <= 0)
                                                MessageBox.Show("不能合并，合并错误");
                                            else
                                            {
                                                db.Execute_sql("update Examapply   set jszt='已执行'  where   CheckFlow='" + Ssbz.Trim() + "'  and jszt<>'已执行' ");
                                                MessageBox.Show("合并完成，病理号：" + getblh.Trim());
                                            }
                                            return "0";

                                        }

                                    }
                                }
                                }
          
                        }
                        catch(Exception  ee3)
                        {
                            MessageBox.Show("不能合并，合并异常" + ee3.ToString());
                            return "0";
                        }
                    }

                    ///////////////////////////////////////////////////////////////////
                    //---------------------------------------------
                    string bbmc = dt_Examapply.Rows[0]["bb1"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw1"].ToString().Trim() + ")";
                    if(dt_Examapply.Rows[0]["bb2"].ToString().Trim()!="" || dt_Examapply.Rows[0]["bb2"].ToString().Trim()!="")
                        bbmc = bbmc + "," + dt_Examapply.Rows[0]["bb2"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw2"].ToString().Trim() + ")";
                    if (dt_Examapply.Rows[0]["bb3"].ToString().Trim() != "" || dt_Examapply.Rows[0]["bb3"].ToString().Trim() != "")
                        bbmc = bbmc + "," + dt_Examapply.Rows[0]["bb3"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw3"].ToString().Trim() + ")";
                    if (dt_Examapply.Rows[0]["bb4"].ToString().Trim() != "" || dt_Examapply.Rows[0]["bb4"].ToString().Trim() != "")
                        bbmc = bbmc + "," + dt_Examapply.Rows[0]["bb4"].ToString().Trim() + "(" + dt_Examapply.Rows[0]["bw4"].ToString().Trim() + ")";

                  
                  

                    //---------xml------------------------
                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    try
                    {
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_Examapply.Rows[0]["PATIENTID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_Examapply.Rows[0]["VISITID"].ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_Examapply.Rows[0]["CHECKFLOW"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_Examapply.Rows[0]["VISITNO"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_Examapply.Rows[0]["INPNO"].ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_Examapply.Rows[0]["NAME"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_Examapply.Rows[0]["SEX"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_Examapply.Rows[0]["AGE"].ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + dt_Examapply.Rows[0]["ADDRESS"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_Examapply.Rows[0]["PHONE"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_Examapply.Rows[0]["DEPTNAME"].ToString() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_Examapply.Rows[0]["BEDNO"].ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + "汉族" + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + dt_Examapply.Rows[0]["DATEOFBIRTH"].ToString() + (char)34 + " ";
                        if (dt_Examapply.Rows[0]["KDXT"].ToString()=="PIS")
                            xml = xml + "送检科室=" + (char)34 + dt_Examapply.Rows[0]["reqdept"].ToString() + (char)34 + " ";
                        else
                           xml = xml + "送检科室=" + (char)34 + dt_Examapply.Rows[0]["DEPTNAME"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_Examapply.Rows[0]["REQPHYSICIAN"].ToString() + (char)34 + " ";
                        //if (dt_Examapply.Rows[0]["sfje"].ToString().Trim() != "")
                        //    rmb = dt_Examapply.Rows[0]["sfje"].ToString();
                        xml = xml + "收费=" + (char)34 + rmb + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + bbmc.Replace("()","").ToString() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_Examapply.Rows[0]["EXAMITEM"].ToString() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " "; 
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dt_Examapply.Rows[0]["Identitys"].ToString() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_Examapply.Rows[0]["INOROUT"].ToString() + (char)34 + " ";
                        xml = xml + "/>";
                        string mcyj = dt_Examapply.Rows[0]["mcyj"].ToString().Trim();
                        if (mcyj != "")
                            mcyj = "末次月经：" + mcyj;
                        xml = xml + "<临床病史><![CDATA[" + mcyj +"  "+ dt_Examapply.Rows[0]["ClinSYMP"].ToString() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_Examapply.Rows[0]["CLINDIAG"].ToString() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;
                    }
                    catch(Exception  rtn_xml_e)
                    {
                        MessageBox.Show("拼接XML异常" + rtn_xml_e.ToString());
                        return "0";
                    }
                }
                if (Sslbx == "查询申请表")
                {

                    string path = f.ReadString("查询申请表", "exepath", "");
                    if (path.Trim() == "")
                        path = "D:\\pathqc\\xy1ysqcx.exe";
                    Process.Start(path);
                    return "0";
                }
                
                return "0";
            } return "0";
        }
    }

    class DBdata
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        string Server = f.ReadString("sqlserverzgq", "Server", "");
        string DataBase = f.ReadString("sqlserverzgq", "DataBase", "");
        string UserID = f.ReadString("sqlserverzgq", "UserID", "");
        string PassWord = f.ReadString("sqlserverzgq", "PassWord", "");

        string orcon_str = "Provider='MSDAORA'; data source=DBSERVER;User ID=DHC;Password=DHC;";
        string odbcsql = f.ReadString("SF", "odbcsql", "");

        public int Execute_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = con = new SqlConnection(constr);
            SqlCommand sqlcom = null;
            try
            {
                sqlcom = new SqlCommand(sqlstr, con);
                con.Open();
                int x = sqlcom.ExecuteNonQuery();
                con.Close();
                sqlcom.Clone();

                return x;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("执行SQL语句异常，" + sqlstr + ",\r\n 异常原因：" + ee.ToString());
                con.Close();
                sqlcom.Clone();
                return -1;
            }
        }
        public DataTable select_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = new SqlConnection(constr);
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, con);
                con.Open();
                sqlda.Fill(dt);
                con.Close();

                return dt;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("执行SQL查询语句异常，" + sqlstr + ",\r\n 异常原因：" + ee.ToString());
                con.Close();
                return dt;
            }

        }
        public DataTable select_orcl(string orcl_strsql, string sm)
        {
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbDataAdapter orcl_dap = new OleDbDataAdapter(orcl_strsql, orcl_con);
            DataTable dt_bill_items = new DataTable();
            try
            {
                orcl_con.Open();
                orcl_dap.Fill(dt_bill_items);
                orcl_con.Close();
                return dt_bill_items;
            }
            catch (Exception orcl_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("执行ORACLE查询语句异常，" + orcl_strsql + ",\r\n 异常原因：" + orcl_ee.ToString());
                return dt_bill_items;

            }

        }
        public int Execute_orcl(string orcl_strsql, string sm)
        {

            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbCommand ocdc = new OleDbCommand(orcl_strsql, orcl_con);
            int x = 0;
            try
            {
                orcl_con.Open();
                x = ocdc.ExecuteNonQuery();
                orcl_con.Close();
                ocdc.Dispose();
            }
            catch (Exception insert_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("执行ORACLE语句异常，" + orcl_strsql + ",\r\n 异常原因：" + insert_ee.ToString());
                return 0;
            }
            return x;

        }
    }
}
