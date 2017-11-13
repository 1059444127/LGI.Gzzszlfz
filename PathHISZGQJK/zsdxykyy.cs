using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using IBM.WMQ;
using PathHISZGQJK;
using System.Xml;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
   /// <summary>
   /// 中山大学眼科医院
   /// 审核后给临床医院发短信，告诉医生，病人报告出来了
   /// 通过工号在oa系统里取电话号码发送
   /// </summary>
    class zsdxykyy
    {
       private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

       string odbc_fsdx = "";
       string odbc2his = "";
    
    
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
       
             string yhmc = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
             string yhbh = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
              debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            
              string ksdm = f.ReadString("savetohis", "KSDM", "70500").Trim();
              string ptjk = ZgqClass.GetSz("zgqjk", "ptjk", "0").Replace("\0", "").Trim();
              string hisjk = ZgqClass.GetSz("zgqjk", "hisjk", "0").Replace("\0", "").Trim();
              string hczt = ZgqClass.GetSz("zgqjk", "hczt", "0").Replace("\0", "").Trim();
              string hcbg = ZgqClass.GetSz("zgqjk", "hcbg", "0").Replace("\0", "").Trim();
              string tzdx = ZgqClass.GetSz("savetohis", "tzdx", "1").Replace("\0", "").Trim();
              string fsdx = ZgqClass.GetSz("savetohis", "fsdx", "1").Replace("\0", "").Trim();
                
            //通知短信数据库
              odbc_fsdx = f.ReadString("savetohis", "odbc_fsdx", "Data Source=192.168.168.155;Initial Catalog=iOffice;User Id=ywkbb;Password=123456;").Replace("\0", "").Trim();
            //his数据库
             odbc2his = f.ReadString("savetohis", "odbc2his", "Data Source=192.168.171.138;Initial Catalog=HIS_PathNet;User Id=bl;Password=bl;").Replace("\0", "").Trim();

             dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
                  bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");

                  if (bljc == null)
                  {
                      log.WriteMyLog("数据库连接异常");
                      return;
                  }
                  if (bljc.Rows.Count <= 0)
                  {
                      log.WriteMyLog("未查询到此报告" + blh);
                      return;
                  }
                  if (fsdx == "1")
                  {
                      #region  发送短信
                      if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核")
                      {
                          if (bljc.Rows[0]["F_BRLB"].ToString().Trim() != "住院")
                          {
                              log.WriteMyLog("非住院病人,不发送短信");
                          }
                          else
                          {
                              //审核后发送
                              string err = "";
                              string sjys = bljc.Rows[0]["F_SJYS"].ToString().Trim();
                              string sjysid = bljc.Rows[0]["F_BY2"].ToString().Trim();
                              string sjhm = "";
                              if (sjys != "" && sjysid != "")
                              {
                                  sjhm = getMobile(sjysid, ref err);
                                  sjhm = sjhm.Trim();

                                  if (sjhm.Trim() == "")
                                  {
                                      log.WriteMyLog("获取医生手机号码失败" + err);
                                  }
                                  else
                                  {
                                      if (sjhm.Length == 11)
                                      {
                                          try
                                          {
                                              //int sendto = int.Parse(sjhm);
                                      
                                              string fsnr = ZgqClass.GetSz("fsdx", "fsnr", "");
                                              if (fsnr.Trim() == "")
                                                  fsnr = "尊敬的{送检医生}医生：病人:{姓名},{性别},{年龄},住院号:{住院号},床号{床号},此病人的病理报告结果已经出来了，请及时查阅。【病理科】";
                                              fsnr = fsnr.Replace("{送检医生}", bljc.Rows[0]["F_SJYS"].ToString().Trim());
                                              fsnr = fsnr.Replace("{姓名}", bljc.Rows[0]["F_XM"].ToString().Trim());
                                              fsnr = fsnr.Replace("{性别}", bljc.Rows[0]["F_XB"].ToString().Trim());
                                              fsnr = fsnr.Replace("{年龄}", bljc.Rows[0]["F_NL"].ToString().Trim());
                                              fsnr = fsnr.Replace("{住院号}", bljc.Rows[0]["F_ZYH"].ToString().Trim());
                                              fsnr = fsnr.Replace("{床号}", bljc.Rows[0]["F_CH"].ToString().Trim());
                                              fsnr = fsnr.Replace("{送检科室}", bljc.Rows[0]["F_SJKS"].ToString().Trim());
                                              fsnr = fsnr.Replace("{门诊号}", bljc.Rows[0]["F_MZH"].ToString().Trim());
                                              fsnr = fsnr.Replace("{报告医生}", bljc.Rows[0]["F_BGYS"].ToString().Trim());
                                              fsnr = fsnr.Replace("{审核医生}", bljc.Rows[0]["F_SHYS"].ToString().Trim());
                                              err = "";
                                              bool rtnbool = MessageSend(sjhm.Trim(), fsnr, sjysid, sjys, ref  err);
                                              if (rtnbool)
                                              {
                                                  aa.ExecuteSQL("insert into T_FSDX(F_BLH,F_CZY,F_FSSJ,F_FSNR,F_SJHM,F_JSR) values('" + blh + "','" + yhmc + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + fsnr.Trim() + "','" + sjhm.ToString().Trim() + "','" + sjys + "')");
                                              }
                                              else
                                              {
                                                  log.WriteMyLog(err);
                                              }
                                          }
                                          catch (Exception ee2)
                                          {
                                              log.WriteMyLog(ee2.Message);
                                          }
                                      }
                                      else
                                      {
                                          log.WriteMyLog("发送短信:医生手机号码格式不正确" + sjhm);
                                      }
                                  }
                              }
                              else
                              {
                                  log.WriteMyLog("发送短信:送检医生或医生ID为空不处理");
                              }
                          }
                      }
                      #endregion
                  }

                 


            string  sqxh=bljc.Rows[0]["F_SQXH"].ToString().Trim();
            if(sqxh!="")
            {
               #region 平台接口
                if (ptjk == "1")
                {
                    ///平台接口
                    ///
                    DataTable dt_sqd = aa.GetDataTable("select * from T_SQD where F_sqxh='" + sqxh + "'", "t_sqd");
                    if (dt_sqd.Rows.Count < 1)
                    {
                       log.WriteMyLog("病理科号:" + blh + ";T_SQD表中无记录,不处理");

                    }
                    else
                    {
                        if (dt_sqd.Rows[0]["F_SQDZT"].ToString() != "已登记")
                        {

                            aa.ExecuteSQL("update T_SQD set F_SQDZT='已登记' where F_sqxh='" + sqxh + "'");
                        }


                        string brlb = bljc.Rows[0]["F_BRLB"].ToString().Trim();
                        string bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
                        #region   回传状态
                        if (cslb[4].ToLower() == "qxsh")
                            bgzt = "取消审核";
                        if (hczt == "1")
                        {

                            ZtToPt(bljc, dt_sqd, blh, bglx, bgxh, bgzt, brlb, yhmc, yhbh, debug);
                        }
                        #endregion

                        #region  回传报告
                        if (hcbg == "1")
                        {
                            if (bgzt == "已审核")
                            {
                                BgToPt(bljc, dt_sqd, blh, bglx, bgxh, brlb, debug);
                            }
                        }
                        #endregion
                    }
                }
                #endregion

               #region HIS接口
               if (hisjk == "1" )
               {

                   if (bglx == "cg")
                   {

                       ///HIS接口
                       #region  确认HIS申请单
                       if ((bljc.Rows[0]["F_HISBJ"].ToString().Trim() != "1" && bljc.Rows[0]["F_HISBJ"].ToString().Trim() != "2"))
                       {
                           ZtToHis(blh, sqxh, yhbh, debug);
                       }
                       #endregion
                   }

             
                  
                        if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
                        {
                            if (hcbg == "1")
                            {
                                #region  回传报告
                                DataTable bljc_bc = new DataTable();
                                bljc_bc = aa.GetDataTable("select * from T_bcbg where F_BLH='" + blh + "' and F_BC_BGZT='已审核'", "bcbg");
                                string blzd = bljc.Rows[0]["F_blzd"].ToString().Trim() + "\r\n";

                                ///补充报告
                                string str_bcbg = "";
                           
                                try
                                {
                                    for (int z = 0; z < bljc_bc.Rows.Count; z++)
                                    {
                                        str_bcbg = str_bcbg + ("补充报告" + bljc_bc.Rows[z]["F_BC_BGXH"].ToString() + ":" + bljc_bc.Rows[z]["F_BCZD"].ToString() + "\r\n");
                                    }
                                }
                                catch
                                {
                                }

                                string xml = "";
                                try
                                {

                                    #region XML
                                    xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                                    xml = xml + "<ArrayOfExamReported_Result>";
                                    xml = xml + "<ExamReported_Result>";
                                    xml = xml + "<ReportII>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportII>";
                                    if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
                                        xml = xml + "<EncounterII>" + "1_" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";//0_m,1_z,
                                    else
                                        xml = xml + "<EncounterII>" + "0_" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";//0_m,1_z,

                                    xml = xml + "<OrderIIs>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</OrderIIs>";
                                    xml = xml + "<ExamineNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ExamineNo>";
                                    xml = xml + "<ClinicItemII></ClinicItemII>";
                                    xml = xml + "<ExamineName>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</ExamineName>";
                                    xml = xml + "<ReportTitle>病理" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "检查报告单</ReportTitle>";
                                    xml = xml + "<BodyParts>" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "</BodyParts>";

                                    string rysj = bljc.Rows[0]["F_RYSJ"].ToString().Trim();
                                    if (rysj.Trim() != "")
                                        rysj = ("肉眼所见：" + rysj + " \r\n ").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                                    string jxsj = bljc.Rows[0]["F_JXSJ"].ToString().Trim();
                                    if (jxsj.Trim() != "")
                                        jxsj = ("镜下所见：" + jxsj + " \r\n ").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");

                                    string tsjc = bljc.Rows[0]["F_TSJC"].ToString().Trim();
                                    if (tsjc.Trim() != "")
                                        tsjc = ("特殊检查：" + tsjc + "\r\n").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;");


                                    xml = xml + "<ReportDesc>" + rysj + jxsj + "</ReportDesc>";
                                    xml = xml + "<ReportConclusion>" + tsjc + ("病理诊断：" + blzd + str_bcbg).Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + "</ReportConclusion>";
                                    xml = xml + "<DiagnoseKindCode></DiagnoseKindCode>";
                                    xml = xml + "<DiagnoseKindCodeSystem></DiagnoseKindCodeSystem>";
                                    xml = xml + "<DiagnoseKindCodeName></DiagnoseKindCodeName>";
                                    xml = xml + "<DiagnoseDesc></DiagnoseDesc>";
                                    xml = xml + "<DiseaseII></DiseaseII>";
                                    xml = xml + "<ICDCode></ICDCode>";
                                    xml = xml + "<DiseaseName></DiseaseName>";
                                    xml = xml + "<DicomAccessNum></DicomAccessNum>";
                                    xml = xml + "<DicomModality></DicomModality>";
                                    xml = xml + "<DiagnosisMethod></DiagnosisMethod>";
                                    xml = xml + "<RepeatNumber></RepeatNumber>";
                                    xml = xml + "<MachineRoomName></MachineRoomName>";
                                    xml = xml + "<DeviceName></DeviceName>";
                                    xml = xml + "<DicomStudyUid></DicomStudyUid>";
                                    xml = xml + "<ExamineEmployeeII>" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</ExamineEmployeeII>";
                                    try
                                    {
                                        xml = xml + "<ExamineOn>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</ExamineOn>";
                                    }
                                    catch (Exception ee2)
                                    {
                                        xml = xml + "<ExamineOn>" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "</ExamineOn>";
                                    }
                                    xml = xml + "<AuditEmployeeII>" + bljc.Rows[0]["F_SHYS"].ToString().Trim() + "</AuditEmployeeII>";
                                    xml = xml + "<AuditOn>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</AuditOn>";
                                    try
                                    {
                                        xml = xml + "<ReportOn>" + DateTime.Parse(bljc.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</ReportOn>";
                                    }
                                    catch (Exception ee2)
                                    {
                                        xml = xml + "<ReportOn>" + bljc.Rows[0]["F_BGRQ"].ToString().Trim() + "</ReportOn>";
                                    }
                                    xml = xml + "<ReportNo></ReportNo>";
                                    xml = xml + "<EffectiveTime></EffectiveTime>";
                                    xml = xml + "<RowVersion>" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-ddTHH:mm:ss") + "</RowVersion>";
                                    xml = xml + "</ExamReported_Result>";
                                    xml = xml + "</ArrayOfExamReported_Result>";

                                    if (debug == "1")
                                        log.WriteMyLog("回传XML：" + xml);
                                    #endregion
                                }
                                catch (Exception e4)
                                {
                                    log.WriteMyLog("病理号：" + blh + ",接口拼接XML异常" + e4.Message.ToString());
                                    return;
                                }

                                if (xml.Trim() != "")
                                    BgToHis(blh, bljc.Rows[0]["F_BRBH"].ToString().Trim(), xml, debug, "EVT.Exam.Reported", "检查结果发布事件通知", ksdm);

                                #endregion
                            }
                        }
                        else
                        {

                           # region 取消审核
                            if (bljc.Rows[0]["F_HISBJ"].ToString().Trim() == "2" && bljc.Rows[0]["F_BGZT"].ToString().Trim()=="已写报告")
                            {
                                //撤销报告
                                string xml = "";
                                try
                                {
                                     xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                                    xml = xml + "<ArrayOfExamReported_Result>";
                                    xml = xml + "<ExamReported_Result>";
                                    xml = xml + "<ReportII>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportII>";
                                    xml = xml + "<EncounterII>" + bljc.Rows[0]["F_BRBH"].ToString().Trim() + "</EncounterII>";
                                    xml = xml + "<OrderIIs>" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "</OrderIIs>";
                                    xml = xml + "<ExamineNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ExamineNo>";
                                    xml = xml + "<ClinicItemII></ClinicItemII>";
                                    xml = xml + "<ExamineName>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</ExamineName>";
                                    xml = xml + "<ReportTitle>病理" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "检查报告单</ReportTitle>";
                                    xml = xml + "<BodyParts>" + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "</BodyParts>";
                                    xml = xml + "<ReportDesc>" + "" + "</ReportDesc>";
                                    xml = xml + "<ReportConclusion>" + "" + "</ReportConclusion>";
                                    xml = xml + "<DiagnoseKindCode></DiagnoseKindCode>";
                                    xml = xml + "<DiagnoseKindCodeSystem></DiagnoseKindCodeSystem>";
                                    xml = xml + "<DiagnoseKindCodeName></DiagnoseKindCodeName>";
                                    xml = xml + "<DiagnoseDesc></DiagnoseDesc>";
                                    xml = xml + "<DiseaseII></DiseaseII>";
                                    xml = xml + "<ICDCode></ICDCode>";
                                    xml = xml + "<DiseaseName></DiseaseName>";
                                    xml = xml + "<DicomAccessNum></DicomAccessNum>";
                                    xml = xml + "<DicomModality></DicomModality>";
                                    xml = xml + "<DiagnosisMethod></DiagnosisMethod>";
                                    xml = xml + "<RepeatNumber></RepeatNumber>";
                                    xml = xml + "<MachineRoomName></MachineRoomName>";
                                    xml = xml + "<DeviceName></DeviceName>";
                                    xml = xml + "<DicomStudyUid></DicomStudyUid>";
                                    xml = xml + "<ExamineEmployeeII>" + "" + "</ExamineEmployeeII>";
                                    xml = xml + "<ExamineOn>" + "" + "</ExamineOn>";
                                    xml = xml + "<AuditEmployeeII>" + "" + "</AuditEmployeeII>";
                                    xml = xml + "<AuditOn>" + "" + "</AuditOn>";
                                    xml = xml + "<ReportOn>" + "" + "</ReportOn>";
                                    xml = xml + "<ReportNo>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</ReportNo>";
                                    xml = xml + "<EffectiveTime></EffectiveTime>";
                                    xml = xml + "<RowVersion>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "</RowVersion>";
                                    xml = xml + "</ExamReported_Result>";
                                    xml = xml + "</ArrayOfExamReported_Result>";

                                      if (debug == "1")
                                    log.WriteMyLog("取消报告XML：" + xml);
                               
                            }
                            catch (Exception e4)
                            {
                                log.WriteMyLog("病理号：" + blh + ",取消报告XML异常" + e4.Message.ToString());
                                return;
                            }

                                if(xml.Trim()!="")
                             BgToHis(blh, bljc.Rows[0]["F_BRBH"].ToString().Trim(), xml, debug, "EVT.Exam.ReportCanceled", "取消检查结果发布事件通知", ksdm);
                               
                                
                             }
                           #endregion
                        }

                }
               #endregion
             }
         
                     
         }

        //发送短信
        public bool MessageSend(string sendto, string content, string empid, string sendtoname, ref string err)
        {

            SqlDB sqlcon = new SqlDB();
            err = "";
            int x = sqlcon.ExecuteNonQuery(odbc_fsdx, "insert into iOffice.dbo.ifMobileInf(sendto,content,instamp,empid,sendtoname) values('" + sendto + "','" + content + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + empid + "','" + sendtoname + "')", ref err);
            if (x > 0)
                return true;
            else
                return false;

        }
        public string getMobile(string userID, ref string err)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            SqlDB sqlcon = new SqlDB();
            err = "";
            DataTable dt = new DataTable();
            dt = sqlcon.DataAdapter(odbc_fsdx, "select  Mobile from  iOffice.dbo.mrbaseinf  where  LoginID='" + userID + "'", ref err);

            if (dt == null)
            {
                return "";

            }
            if (dt.Rows.Count > 0)
                return dt.Rows[0]["Mobile"].ToString();
            else
                return "";

        }

        //HIS
        public void  ZtToHis(string blh,string sqxh,string yhgh,string  debug )
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                       try
                       {
                           int.Parse(sqxh);
                       }
                       catch
                       {
                           log.WriteMyLog("申请序号格式不正确");
                           return;
                       }

                       try
                       {
                       SqlParameter[] sqlPt = new SqlParameter[3];
                       for (int j = 0; j < sqlPt.Length; j++)
                       {
                           sqlPt[j] = new SqlParameter();
                       }
                       //申请单ID
                       sqlPt[0].ParameterName = "In_FunctionRequestID";
                       sqlPt[0].SqlDbType = SqlDbType.Int;
                       sqlPt[0].Direction = ParameterDirection.Input;
                       sqlPt[0].Value = int.Parse(sqxh);

                       //操作员工工号
                       sqlPt[1].ParameterName = "In_OperatorEmployeeNo";
                       sqlPt[1].SqlDbType = SqlDbType.NVarChar;
                       sqlPt[1].Direction = ParameterDirection.Input;
                       sqlPt[1].Size = 10;
                       sqlPt[1].Value =yhgh ;
                       //取消标志
                       sqlPt[2].ParameterName = "Out_StatusFlag";
                       sqlPt[2].SqlDbType = SqlDbType.TinyInt;
                       sqlPt[2].Direction = ParameterDirection.Output;
                       sqlPt[2].Value = 0;

                       string err_msg = "";
                       SqlDB db = new SqlDB();
                       db.ExecuteNonQuery(odbc2his, "pConfirmFunctionRequest", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
                       if (int.Parse(sqlPt[2].Value.ToString()) == 1)
                       {   aa.ExecuteSQL("update  T_JCXX  set F_HISBJ='1'  where F_BLH='"+blh+"'");
                       log.WriteMyLog("确认申请单成功");
                           if(debug=="1")
                           MessageBox.Show("确认确定申请单成功");
                       }
                       else
                       {
                           aa.ExecuteSQL("update  T_JCXX  set F_HISBJ='-1'  where F_BLH='" + blh + "'");
                            log.WriteMyLog("确认申请单失败:"+sqlPt[2].Value.ToString() + "："+err_msg);return;
                           if(debug=="1")
                               MessageBox.Show(sqlPt[2].Value.ToString() + "：" + err_msg);
                       }
                   }
                   catch(Exception  ee1)
                   {
                       log.WriteMyLog("确定申请单失败：异常："+ee1.Message);return;
                   }
                   }
        public void  BgToHis(string  blh,string brbh,string XML,string  debug,string EventName, string Description, string OrganizationII)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string bglx = "cg";
            string bgxh = "0";
           try
           {
               SqlParameter[] sqlPt = new SqlParameter[10];
               for (int j = 0; j < sqlPt.Length; j++)
               {
                   sqlPt[j] = new SqlParameter();
               }

             
               //事件标识符
               sqlPt[0].ParameterName = "in_EventII";
               sqlPt[0].SqlDbType = SqlDbType.NVarChar;
               sqlPt[0].Direction = ParameterDirection.Input;
               sqlPt[0].Size=128;
               sqlPt[0].Value = "Logene.Pathology^" + blh;
              
               //事件名称
               sqlPt[1].ParameterName = "in_EventName";
               sqlPt[1].SqlDbType = SqlDbType.NVarChar;
               sqlPt[1].Direction = ParameterDirection.Input;
               sqlPt[1].Size=128;
               sqlPt[1].Value = EventName;
             
                //发送者名称
               sqlPt[2].ParameterName = "in_Sender";
               sqlPt[2].SqlDbType = SqlDbType.NVarChar;
               sqlPt[2].Direction = ParameterDirection.Input;
               sqlPt[2].Size=128;
               sqlPt[2].Value = "Logene.Pathology";
              
                //事件描述
               sqlPt[3].ParameterName = "in_Description";
               sqlPt[3].SqlDbType = SqlDbType.NVarChar;
               sqlPt[3].Direction = ParameterDirection.Input;
               sqlPt[3].Size=128;
               sqlPt[3].Value = Description;
              
               //患者的就诊记录标识符
               sqlPt[4].ParameterName = "in_SubjectII";
               sqlPt[4].SqlDbType = SqlDbType.NVarChar;
               sqlPt[4].Direction = ParameterDirection.Input;
               sqlPt[4].Size = 128;
               sqlPt[4].Value =brbh ;
              
                //事件关联的科室标识符
               sqlPt[5].ParameterName = "in_OrganizationII";
               sqlPt[5].SqlDbType = SqlDbType.NVarChar;
               sqlPt[5].Direction = ParameterDirection.Input;
               sqlPt[5].Size = 128;
               sqlPt[5].Value = OrganizationII;
          
               //事件发生时间
               sqlPt[6].ParameterName = "in_EventDateTime";
               sqlPt[6].SqlDbType = SqlDbType.DateTime;
               sqlPt[6].Direction = ParameterDirection.Input;
               sqlPt[6].Value =DateTime.Now ;
            
                //用来记录事件的结束时间
               sqlPt[7].ParameterName = "in_EventEndOn";
               sqlPt[7].SqlDbType = SqlDbType.DateTime;
               sqlPt[7].Direction = ParameterDirection.Input;
               sqlPt[7].Value =DateTime.Now ;
              
                //事件内容
               sqlPt[8].ParameterName = "in_EventData";
               sqlPt[8].SqlDbType = SqlDbType.NText;
               sqlPt[8].Direction = ParameterDirection.Input;
               sqlPt[8].Value =XML ;
             
               //事件写入平台时间
               sqlPt[9].ParameterName = "in_RowVersion";
               sqlPt[9].SqlDbType = SqlDbType.DateTime;
               sqlPt[9].Direction = ParameterDirection.Input;
               sqlPt[9].Value =DateTime.Now ;
          
               string err_msg = "";
               SqlDB db = new SqlDB();
              int x= db.ExecuteNonQuery(odbc2his, "pEventPublish", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
           

               if (x>0)
               {
                   if (debug == "1")
                   log.WriteMyLog(Description + "：成功"  );


               if (Description == "取消检查结果发布事件通知")
               {

                   aa.ExecuteSQL("update T_jcxx_fs set F_bz='取消检查结果成功',F_FSZT='已处理'  where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
               }
               else
               {
                   aa.ExecuteSQL("update  T_JCXX  set F_HISBJ='2' where F_BLH='" + blh + "'");
                   aa.ExecuteSQL("update T_jcxx_fs set F_bz='发布结果成功',F_FSZT='已处理' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核'");

               }
                   return;
               }
               else
               {
                   if (debug == "1")
                   log.WriteMyLog(Description + "：失败 " + sqlPt[2].Value.ToString());
                   if (Description == "取消检查结果发布事件通知")
                   {
                       log.WriteMyLog("取消报告失败 " + err_msg); 
                      // if (debug == "1")
                         //  MessageBox.Show(sqlPt[2].Value.ToString() + "取消报告");

                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='取消报告失败" + err_msg + "',F_FSZT='已处理' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='取消审核'");
            
                   }
                   else
                   {
  
                       log.WriteMyLog("发布结果失败 " + err_msg); return;
                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='发布结果失败" + err_msg + "' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核'");
            
                   }
                   
               }
           }
           catch(Exception  ee1)
           {
               log.WriteMyLog("发布结果异常：" + ee1.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='发布结果异常：" + ee1.Message + "' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' ");
            
               return;
           }
          
        }

        //Pt
        public void ZtToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string bgzt,string brlb,string yhmc,string yhbh,string  debug)
        {
            string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();
            if (bglx == "bc" || bglx == "bd")
                return;
         
            string errmsg = "";
            string message = ZtMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh, bgzt, ref errmsg, yhmc,yhbh);

            if (message == "")
            {
                log.WriteMyLog("MQ状态生成XMl失败：" + errmsg);
                return;
            }
       
            if(debug=="1")
                log.WriteMyLog("MQ状态回传："+message);

            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BLToFZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;

                string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS004.LQ", "BS004", jzlb, "0", "S009", "45541605-3", "70500", "0");

                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS004)MQ状态发送错误：" + rtnmsg);
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS004)MQ状态发送完成：" + rtnmsg);
                }
            }
            catch(Exception  ee2)
            {
                log.WriteMyLog("(BS004)MQ状态发送异常：" + ee2.Message);
            }
            return;
        }
        public void BgToPt(DataTable  dt,DataTable  dt_sqd,string blh,string bglx,string bgxh,string brlb,string debug)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            //生成pdf
            string ML = DateTime.Parse(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
            string errmsg = ""; string pdf2base64 = ""; string filename = "";
            bool isbase64 = true; bool scpdf = true; bool uppdf = true;
            C_PDF(ML, blh, bgxh, bglx, ref errmsg, ref isbase64, ref pdf2base64, ref filename, ref scpdf, ref uppdf, debug);
            if (pdf2base64.Length < 10)
            {
                log.WriteMyLog("报告PDF转base64失败");
            }
            string jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString();

            string message = BgMsg(dt, dt_sqd, ref brlb, blh, bglx, bgxh,ref errmsg,pdf2base64);
    
            if (message == "")
            {
                log.WriteMyLog("MQ报告生成XMl失败：" + errmsg);
                aa.ExecuteSQL("update T_jcxx_fs set F_bz='MQ报告生成XMl失败:" + errmsg+"' where F_blbh='" + blh + bglx+ bgxh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                return;
            }

            if (debug == "1")
                log.WriteMyLog("MQ报告发送：" + message);

            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();
            try
            {
                BLToFZMQWS.Service fzmq = new PathHISZGQJK.BLToFZMQWS.Service();
                if (wsurl != "")
                    fzmq.Url = wsurl;
                string rtnmsg = fzmq.SendZtMsgToMQ(message, "IN.BS366.LQ", "BS366", jzlb, "0", "S009", "45541605-3", "70500", "0");

                if (debug == "1")
                    log.WriteMyLog("(BS366)MQ报告返回：" + rtnmsg);
                if (rtnmsg.Contains("ERR"))
                {
                    log.WriteMyLog("(BS366)MQ报告发送错误：" + rtnmsg);
                     aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS366)MQ报告发送错误:" + rtnmsg + "' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核' ");
            
                    return;
                }
                else
                {
                    log.WriteMyLog("(BS366)MQ报告发送完成：" + rtnmsg);
                     aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS366)MQ报告发送完成:" + rtnmsg + "',F_FSZT='已处理' where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核' ");
                    return;
                }
                return;
            }
            catch(Exception  ee3)
            {
                log.WriteMyLog("(BS366)MQ报告发送异常：" + ee3.Message);
                 aa.ExecuteSQL("update T_jcxx_fs set F_bz='(BS366)MQ报告发送异常:" + ee3.Message + "'  where F_blbh='" + blh + bglx + bgxh + "' and F_fszt='未处理' and F_BGZT='已审核'");
                return;
            }
        }

        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + int.Parse(blh[i].ToString()).ToString();
                }
                catch { }
            }
            return x;
        }

        public string  ZtMsg(DataTable dt, DataTable dt_sqdxx, ref string xbrlb, string blh, string bglx, string bgxh, string bgzt, ref string errmsg,string yhmc,string yhbh)
        {
          

          
            string bgztbm = "";
            string bgztstr = "";

            if (bgzt == "已审核")
            {
                bgztbm = "170.003";
                bgztstr = "检查报告已审核";
            }
            if (bgzt == "已写报告")
            {
                bgztbm = "160.003";
                bgztstr = "检查已完成";
            }

            if (bgzt == "已取材")
            {
                bgztbm = "140.002";
                bgztstr = "检查已到检";
            }
            if (bgzt == "已登记")
            {
                bgztbm = "140.002";
                bgztstr = "检查已到检";
            }
            if (bgzt == "取消审核")
            {
                bgztbm = "990.001";
                bgztstr = "报告召回";
            }

            if (bgztstr == "" || bgztbm.Trim() == "")
            {
          
                return "";
            }
            try
            {
                xbrlb = dt_sqdxx.Rows[0]["F_jzlb"].ToString();
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<POOR_IN200901UV ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3  ../../Schemas/POOR_IN200901UV23.xsd\">";

                xml = xml + "<!-- 消息ID -->";
                xml = xml + "<id extension=\"BS004\" />";
                xml = xml + "<!-- 消息创建时间 -->";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\" />";
                xml = xml + "<!-- 交互ID -->";
                xml = xml + "<interactionId extension=\"POOR_IN200901UV23\" />";
                xml = xml + "<!--消息用途: P(Production); D(Debugging); T(Training) -->";
                xml = xml + "<processingCode code=\"P\" />";
                xml = xml + "<!-- 消息处理模式: A(Archive); I(Initial load); R(Restore from archive); T(Current  processing) -->";
                xml = xml + "<processingModeCode code=\"R\" />";
                xml = xml + "<!-- 消息应答: AL(Always); ER(Error/reject only); NE(Never) -->";
                xml = xml + "<acceptAckCode code=\"NE\" />";
                xml = xml + "<!-- 接受者 -->";
                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";

                xml = xml + "<!-- 接受者ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<!-- 发送者 -->";
                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 发送者ID -->";
                xml = xml + "<id>";
                xml = xml + "<item root=\"\" extension=\"S009\"/>";
                xml = xml + "</id>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<!-- 封装的消息内容(按Excel填写) -->";
                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<!-- 消息交互类型 @code: 新增 :new 修改:update -->";
                xml = xml + "<code code=\"update\"></code>";
                xml = xml + "<subject typeCode=\"SUBJ\" xsi:nil=\"false\">";
                xml = xml + "<placerGroup>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<code></code>";
                xml = xml + "<!-- 检验申请单状态 必须项未使用 -->";
                xml = xml + "<statusCode code=\"active\"></statusCode>";
                xml = xml + "<!-- 患者信息 -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<id>";
                xml = xml + "<!--域ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.2\" extension=\"" + dt_sqdxx.Rows[0]["F_yid"].ToString() + "\" />";
                xml = xml + "<!-- 患者ID -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.3\" extension=\"" + dt_sqdxx.Rows[0]["F_brbh"].ToString() + "\" />";
                xml = xml + "<!-- 就诊号 -->";
                xml = xml + "<item root=\"1.2.156.112678.1.2.1.12\" extension=\"" + dt_sqdxx.Rows[0]["F_jzh"].ToString() + "\" />";
                xml = xml + "</id>";
                xml = xml + "<providerOrganization classCode=\"ORG\"  determinerCode=\"INSTANCE\">";
                xml = xml + "<!--病人科室编码-->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_ksbm"].ToString() + "\" root=\"1.2.156.112678.1.1.1\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--病人科室名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_ksmc"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "<asOrganizationPartOf classCode=\"PART\">";
                xml = xml + "<wholeOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--医疗机构代码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yybm"].ToString() + "\"/>";
                xml = xml + "</id>";
                xml = xml + "<!--医疗机构名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item><part value=\"" + dt_sqdxx.Rows[0]["F_yymc"].ToString() + "\" /></item>";
                xml = xml + "</name>";
                xml = xml + "</wholeOrganization>";
                xml = xml + "</asOrganizationPartOf>";
                xml = xml + "</providerOrganization>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "<!-- 操作人 -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<time>";
                xml = xml + "<!-- 操作日期 -->";
                xml = xml + "<any value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"></any>";
                xml = xml + "</time>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 操作人编码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\""+yhbh+"\" root=\"1.2.156.112678.1.1.2\"></item>";
                xml = xml + "</id>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 操作人姓名 必须项已使用 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\""+yhmc+"\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<!--执行科室 -->";
                xml = xml + "<location typeCode=\"LOC\" xsi:nil=\"false\">";
                xml = xml + "<!--必须项未使用 -->";
                xml = xml + "<time />";
                xml = xml + "<!--就诊机构/科室 -->";
                xml = xml + "<serviceDeliveryLocation classCode=\"SDLOC\">";
                xml = xml + "<serviceProviderOrganization determinerCode=\"INSTANCE\" classCode=\"ORG\">";
                xml = xml + "<!--执行科室编码 -->";
                xml = xml + "<id>";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_zxksbm"].ToString() + "\" root=\"1.21.156.112649.1.1.1\" />";
                xml = xml + "</id>";
                xml = xml + "<!--执行科室名称 -->";
                xml = xml + "<name xsi:type=\"BAG_EN\">";
                xml = xml + "<item>";
                xml = xml + "<part value=\"" + dt_sqdxx.Rows[0]["F_zxks"].ToString() + "\" />";
                xml = xml + "</item>";
                xml = xml + "</name>";
                xml = xml + "</serviceProviderOrganization>";
                xml = xml + "</serviceDeliveryLocation>";
                xml = xml + "</location>";
            
                xml = xml + "<!-- 1..n可循环  医嘱状态信息 -->";
                xml = xml + "<component2>";
                xml = xml + "<!--医嘱序号-->";
                xml = xml + "<sequenceNumber value=\"1\"/>";
                xml = xml + "<observationRequest classCode=\"OBS\">";
                xml = xml + "<!-- 必须项已使用 -->";
                xml = xml + "<id>";
                xml = xml + "<!-- 医嘱号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_yzh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.22\"/>";
                xml = xml + "<!-- 申请单号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_sqxh"].ToString() + "\" root=\"1.2.156.112678.1.2.1.21\"/>";
                xml = xml + "<!-- 报告号 -->";
                xml = xml + "<item extension=\"" + blh + "\" root=\"1.2.156.112678.1.2.1.24\"/>";
                xml = xml + "<!-- StudyInstanceUID -->";
                xml = xml + "<item extension=\"\" root=\"1.2.156.112678.1.2.1.30\"/>";
                xml = xml + "</id>";
             
                xml = xml + "<!-- 医嘱类别编码/医嘱类别名称 - 针剂药品, 材料类, 治疗类, 片剂药品, 化验类 -->";
                xml = xml + "<code code=\"" + dt_sqdxx.Rows[0]["F_YZLXBM"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.27\">";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_YZLXMC"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<statusCode />";
                xml = xml + "<!-- 必须项未使用 -->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\" />";
                
                xml = xml + "<!-- 标本信息 -->";
                xml = xml + "<specimen typeCode=\"SPC\">";
                string [] bbtmh = dt_sqdxx.Rows[0]["F_bbtmh"].ToString().Trim().Split('#');
                foreach (string bbtm in bbtmh)
                {
                    xml = xml + "<specimen classCode=\"SPEC\">";
                    xml = xml + "<!--标本条码号 必须项已使用 -->";
                    xml = xml + "<id extension=\"" + bbtm + "\" />";
                    xml = xml + "<!--必须项目未使用 -->";
                    xml = xml + "<code />";
                    xml = xml + "<subjectOf1 typeCode=\"SBJ\" contextControlCode=\"OP\">";
                    xml = xml + "<specimenProcessStep moodCode=\"EVN\" classCode=\"SPECCOLLECT\">";
                    xml = xml + "<!-- 采集日期 -->";
                    xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                 
                    try
                    {
                        xml = xml + "<any value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMddHHmmss")  + "\"></any>";
                    }
                    catch
                    {
                        xml = xml + "<any value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMddHHmmss")  + "\"></any>";
                    }
                    xml = xml + "</effectiveTime>";
                    xml = xml + "<performer typeCode=\"PRF\">";
                    xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                    xml = xml + "<!-- 采集人Id -->";
                    xml = xml + "<id>";
                    xml = xml + "<item extension=\"" + getyhgh(dt.Rows[0]["F_QCYS"].ToString()) + "\" root=\"1.2.156.112678.1.1.2\"></item>";
                    xml = xml + "</id>";
                    xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                    xml = xml + "<!-- 采集人姓名 -->";
                    xml = xml + "<name xsi:type=\"BAG_EN\">";
                    xml = xml + "<item>";
                    xml = xml + "<part value=\"" + dt.Rows[0]["F_QCYS"].ToString() + "\" />";
                    xml = xml + "</item>";
                    xml = xml + "</name>";
                    xml = xml + "</assignedPerson>";
                    xml = xml + "</assignedEntity>";
                    xml = xml + "</performer>";
                    xml = xml + "</specimenProcessStep>";
                    xml = xml + "</subjectOf1>";
                    xml = xml + "</specimen>";

                }
                xml = xml + "</specimen>";

                xml = xml + "<!-- 原因 -->";
                xml = xml + "<reason contextConductionInd=\"true\">";
                xml = xml + "<observation moodCode=\"EVN\" classCode=\"OBS\">";
                xml = xml + "<!-- 必须项 未使用-->";
                xml = xml + "<code></code>";
                xml = xml + "<value xsi:type=\"ST\" value=\"\"/>";
                xml = xml + "</observation>";
                xml = xml + "</reason>";
                xml = xml + "<!-- 医嘱执行状态 -->";
                xml = xml + "<component1 contextConductionInd=\"true\">";
                xml = xml + "<processStep classCode=\"PROC\">";
                xml = xml + "<code code=\"" + bgztbm + "\" codeSystem=\"1.2.156.112678.1.1.93\">";
                xml = xml + "<!--医嘱执行状态名称 -->";
                xml = xml + "<displayName value=\"" + bgztstr + "\" />";
                xml = xml + "</code>";
                xml = xml + "</processStep>";
                xml = xml + "</component1>";
                xml = xml + "</observationRequest>";
                xml = xml + "</component2>";

                xml = xml + "<!--就诊 -->";
                xml = xml + "<componentOf1 contextConductionInd=\"false\" xsi:nil=\"false\" typeCode=\"COMP\">";
                xml = xml + "<!--就诊 -->";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<id>";
                xml = xml + "<!-- 就诊次数 必须项已使用 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZCS"].ToString() + "\" root=\"1.2.156.112678.1.2.1.7\" />";
                xml = xml + "<!-- 就诊流水号 -->";
                xml = xml + "<item extension=\"" + dt_sqdxx.Rows[0]["F_JZLSH"].ToString() + "\" root=\"1.2.156.112678.1.2.1.6\"/>";

                xml = xml + "</id>";
                xml = xml + "<!--就诊类别编码-->";
                xml = xml + "<code codeSystem=\"1.2.156.112678.1.1.80\" code=\"" + dt_sqdxx.Rows[0]["F_JZLB"].ToString() + "\">";
                xml = xml + "<!-- 就诊类别名称 -->";
                xml = xml + "<displayName value=\"" + dt_sqdxx.Rows[0]["F_BRLB"].ToString() + "\" />";
                xml = xml + "</code>";
                xml = xml + "<!--必须项未使用 -->";
                xml = xml + "<statusCode code=\"Active\" />";
                xml = xml + "<!--病人 必须项未使用 -->";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\" />";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf1>";
                xml = xml + "</placerGroup>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</POOR_IN200901UV>";

                return FormatXml(xml);
               
            }
            catch (Exception ex2)
            {
                errmsg = "ERR:" + ex2.Message; return "";
            }
        }

        public string BgMsg(DataTable dt, DataTable dt_sqd, ref string xbrlb, string blh, string bglx, string bgxh, ref string errmsg, string pdf2base64)
        {
            try
            {
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
                xml = xml + "<ClinicalDocument xmlns=\"urn:hl7-org:v3\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"urn:hl7-org:v3 ../coreschemas/CDA.xsd\">";
                //    <!-- 文档适用范围编码 -->
                xml = xml + "<realmCode code=\"CN\"/>";
                //    <!-- 文档信息模型类别-标识符 -->
                //    <!-- 固定值 -->
                xml = xml + "<typeId root=\"2.16.840.1.113883.1.3\" extension=\"POCD_HD000040\"/>";
                //    <!-- 文档标识-报告号 -->
                string bgh = blh;
                string bgys = dt.Rows[0]["f_bgys"].ToString();
                string shys = dt.Rows[0]["f_shys"].ToString();
                string bgrq = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                if (bglx == "bc")
                {
                    bgh = bgh + "_bc_" + bgxh;
                    bgys = dt.Rows[0]["f_bc_bgys"].ToString();
                    shys = dt.Rows[0]["f_bc_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                }
                if (bglx == "bd")
                {
                    bgh = bgh + "_bd_" + bgxh;
                    bgys = dt.Rows[0]["f_bd_bgys"].ToString();
                    shys = dt.Rows[0]["f_bd_shys"].ToString();
                    bgrq = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                }
                string bgysid = "";
                string shysid = "";
                string qcysid = "";
                bgysid = getyhgh(bgys);
                shysid = getyhgh(shys);
                qcysid = getyhgh(dt.Rows[0]["F_QCYS"].ToString());
             

                xml = xml + "<id root=\"S038\" extension=\"" + bgh + "\"/>";
                xml = xml + "<!-- 文档标识-名称 / 文档标识-类别编码 -->";
                xml = xml + "<!-- 固定值 -->";
                xml = xml + "<code code=\"04\" codeSystem=\"1.2.156.112678.1.1.60\" displayName=\"检查检验记录\"/>";
                xml = xml +"<!-- 文档标题文本 -->";
                xml = xml + "<title>病理检查报告</title>";
                xml = xml + "<!-- 文档生效日期 -->";
                xml = xml + "<effectiveTime value=\"" + DateTime.Now.ToString("yyyyMMdd") + "\" />";
                xml = xml + "<!-- 文档密级编码 -->";
                xml = xml + "<confidentialityCode code=\"N\" codeSystem=\"2.16.840.1.113883.5.25\" codeSystemName=\"Confidentiality\" displayName=\"normal\" />";
                xml = xml + "<!-- 文档语言编码 -->";
                xml = xml + "<languageCode code=\"zh-CN\" />";
                xml = xml + "<!--服务ID-->";
	            xml = xml + "<setId extension=\"BS366\"/>";
                //需要程序确定是新增还是修改
                string cfsh = "0";
                try
                {
                    cfsh = dt.Rows[0]["F_cfsh"].ToString().Trim();
                }
                catch
                {
                }
                if (bglx == "bc"|| bglx=="bd")
                {
                    cfsh = "0";
                }
                
                xml = xml + "<!-- 文档的操作版本:0表示新增, 1表示修改 -->";
                xml = xml + "<versionNumber value=\"" + cfsh + "\"/>";

                xml = xml + "<!-- 文档记录对象 -->";
                xml = xml + "<recordTarget typeCode=\"RCT\">";
                xml = xml + "<!-- 病人信息 -->";
                xml = xml + "<patientRole classCode=\"PAT\">";
                xml = xml + "<!-- 域ID -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.2\" extension=\"" + dt_sqd.Rows[0]["F_yid"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 患者ID -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.3\" extension=\"" + dt_sqd.Rows[0]["F_BRBH"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 就诊号 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.12\" extension=\"" + dt_sqd.Rows[0]["F_JZH"].ToString().Trim() + "\" />";

                xml = xml + "<!-- 病区床号信息 -->";
                xml = xml + "<addr use=\"TMP\">";
                xml = xml + "<!-- 床位号 -->";
                xml = xml + "<careOf>" + dt_sqd.Rows[0]["F_CH"].ToString().Trim() + "</careOf>";
                xml = xml + "</addr>";

                xml = xml + "<!-- 病人基本信息 -->";
                xml = xml + "<patient classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 病人名称 -->";
                xml = xml + "<name>" + dt.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "<!-- 性别编码/性别名称 -->";
                xml = xml + "<administrativeGenderCode code=\"" + dt_sqd.Rows[0]["F_XBBM"].ToString().Trim() + "\" codeSystem=\"1.2.156.112678.1.1.3\" displayName=\"" + dt_sqd.Rows[0]["F_XB"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 出生日期 -->";
                xml = xml + "<birthTime value=\"" + dt_sqd.Rows[0]["F_CSRQ"].ToString() + "\" />";
                xml = xml + "</patient>";
                xml = xml + "</patientRole>";
                xml = xml + "</recordTarget>";

                xml = xml + "<!-- 文档作者(检查报告医生, 可循环) -->";
                xml = xml + "<author typeCode=\"AUT\">";
                xml = xml + "<!-- 报告日期 -->";
                xml = xml + "<time value=\"" + DateTime.Parse(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<assignedAuthor classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 报告医生编码 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 报告医生名称 -->";
                xml = xml + "<name>" + bgys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedAuthor>";
                xml = xml + "</author>";

                xml = xml + "<!-- 文档保管者(CDA中custodian为必填项) -->";
                xml = xml + "<custodian>";
                xml = xml + "<assignedCustodian>";
                xml = xml + "<representedCustodianOrganization>";
                xml = xml + "<!-- 医疗机构编码 -->";
                xml = xml + "<id root=\"1.2.156.112678\" extension=\"" + dt_sqd.Rows[0]["F_YYBM"].ToString().Trim() + "\" />";
                xml = xml + "<!-- 医疗机构名称 -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_YYMC"].ToString().Trim() + "</name>";
                xml = xml + "</representedCustodianOrganization>";
                xml = xml + "</assignedCustodian>";
                xml = xml + "</custodian>";

                xml = xml + "<!-- 电子签章信息 -->";
                xml = xml + "<legalAuthenticator>";
                xml = xml + "<time />";
                xml = xml + "<signatureCode code=\"S\" />";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- 电子签章号-->";
                xml = xml + "<id extension=\"\" />";
                xml = xml + "</assignedEntity>";
                xml = xml + "</legalAuthenticator>";

                xml = xml + "<!-- 文档审核者(检查报告审核医师, 可循环) -->";
                xml = xml + "<authenticator>";
                xml = xml + "<!-- 审核日期 -->";
                string shsj = "";
                try
                {
                    shsj = Convert.ToDateTime(dt.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                }
                catch
                {
                }
                if (shsj == "")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                }
                if (bglx == "bc")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                if (bglx == "bd")
                {
                    try
                    {
                        shsj = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                    }
                    catch
                    {
                    }
                    if (shsj == "")
                    {
                        try
                        {
                            shsj = Convert.ToDateTime(dt.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss");
                        }
                        catch
                        {
                        }
                    }

                }
                xml = xml + "<time value=\"" + shsj + "\" />";
                xml = xml + "<signatureCode code=\"S\"/>";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 审核医生编码 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.2\" extension=\"" + shysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 审核医生名称 -->";
                xml = xml + "<name>" + shys + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authenticator>";


                xml = xml + "<!-- 申请医生信息 -->";
                xml = xml + "<participant typeCode=\"AUT\">";
                xml = xml + "<associatedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!-- 申请医生编码 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.2\" extension=\""+ dt_sqd.Rows[0]["F_SQYSBM"].ToString().Trim()+"\" />";
                xml = xml + "<associatedPerson>";
                xml = xml + "<!-- 申请医生名称 -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQYS"].ToString().Trim() + "</name>";
                xml = xml + "</associatedPerson>";
                xml = xml + "<scopingOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
			    xml = xml + "<!-- 申请科室编码 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.1\" extension=\"" + dt_sqd.Rows[0]["F_SQKSBM"].ToString().Trim() + "\"/>";
	            xml = xml + "<!-- 申请科室名称 -->";
                xml = xml + "<name>" + dt_sqd.Rows[0]["F_SQKS"].ToString().Trim() + "</name>";
			    xml = xml + "</scopingOrganization>";
                xml = xml + "</associatedEntity>";
                xml = xml + "</participant>";


                xml = xml + "<!-- 关联医嘱信息 -->";
                xml = xml + "<inFulfillmentOf>";
                xml = xml + "<order>";
                xml = xml + "<!-- 关联医嘱号(可多个) -->";
                xml = xml + "<id extension=\"" + dt_sqd.Rows[0]["F_YZH"].ToString().Trim() + "\"/>";
                xml = xml + "</order>";
                xml = xml + "</inFulfillmentOf>";

                xml = xml + "<!-- 文档中医疗卫生事件的就诊场景 -->";
                xml = xml + "<componentOf typeCode=\"COMP\">";
                xml = xml + "<!-- 就诊信息 -->";
                xml = xml + "<encompassingEncounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!-- 就诊次数 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.7\" extension=\"" + dt_sqd.Rows[0]["F_jzcs"].ToString() + "\"/>";
                xml = xml + "<!-- 就诊流水号 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.2.1.6\" extension=\"" + dt_sqd.Rows[0]["F_JZLSH"].ToString() + "\"/>";			
			    xml = xml + "<!-- 就诊类别编码/就诊类别名称 -->";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_jzlb"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.80\" displayName=\"" + dt_sqd.Rows[0]["F_brlb"].ToString() + "\" />";
                xml = xml + "<effectiveTime />";
                xml = xml + "</encompassingEncounter>";
                xml = xml + "</componentOf>";
                //<!--
                //********************************************************
                //CDA Body
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<structuredBody>";
                //<!-- 
                //********************************************************
                //文档中患者相关信息
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"34076-0\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Information for patients section\" />";
                xml = xml + "<title>文档中患者相关信息</title>";
                xml = xml + "<!-- 患者年龄 -->";
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"397669002\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"age\" />";
                xml = xml + "<value xsi:type=\"ST\">" + dt.Rows[0]["F_nl"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";
               xml = xml + "<!-- 病区 -->";
                xml = xml + "<entry>";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\" >";
                xml = xml + "<code code=\"225746001\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"ward\" />";
                xml = xml + "<!-- 病区编码 病区名称 -->";
                xml = xml + "<value xsi:type=\"SC\" code=\"" + dt_sqd.Rows[0]["F_BQBM"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.33\">" + dt_sqd.Rows[0]["F_BQ"].ToString() + "</value>";
                xml = xml + "</observation>";
                xml = xml + "</entry>";

                xml = xml + "<component>";
				xml = xml + "<section>";
				xml = xml + "<code code=\"49033-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Menstrual History\"></code>";
				xml = xml + "<entry>";
				xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
				xml = xml + "<code code=\"8665-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Last menstrual period start date\"></code>";
				xml = xml + "<!--末次月经时间-->";
				xml = xml + "<value xsi:type=\"TS\" value=\"\"></value>";
				xml = xml + "</observation>";
				xml = xml + "</entry>";
				xml = xml + "<entry>";
				xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
				xml = xml + "<code code=\"63890-8\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Age at last menstrual period \"></code>";
				xml = xml + "<!--绝经-->";
				xml = xml + "<value xsi:type=\"ST\"></value>";
				xml = xml + "</observation>";
				xml = xml + "</entry>";
				xml = xml + "</section>";
				xml = xml + "</component>";

                xml = xml + "</section>";
                xml = xml + "</component>";

                //<!--
                //********************************************************
                //检查章节
                //********************************************************
                //-->
                xml = xml + "<component>";
                xml = xml + "<section>";
                xml = xml + "<code code=\"30954-2\" displayName=\"STUDIES SUMMARY\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\"/>";
                xml = xml + "<title>病理检查</title>";



                // <!-- 相关信息 -->
                xml = xml + "<entry>";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<code code=\"310388008\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"relative information status\" />";
                xml = xml + "<statusCode code=\"completed\" />";
                //<!-- 定位图像信息 -->
                xml = xml + "<component>";
                xml = xml + "<supply classCode=\"SPLY\" moodCode=\"EVN\">";
                //<!-- 图像索引号(accessionNumber) -->
                xml = xml + "<id extension=\"" + "1001" + "\" />";
                xml = xml + "</supply>";
                xml = xml + "</component>";

                //xml = xml + "<component>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- 检查报告类型标识编码/检查报告类型标识名称 --0正常报告，1补充报告>
                //string bglxstr = "0";
                //if (bglx == "bc" || bglx == "bd")
                //{
                //    bglxstr = "1";
                //}
                //xml = xml + "<code code=\"" + bglxstr + "\" codeSystem=\"1.2.156.112678.1.1.112\" displayName=\"病理检查报告\" />";
                //xml = xml + "</observation>";
                //xml = xml + "</component>";

                xml = xml + "<component>";
                xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
				xml = xml + "<!-- 图片信息(要求编码为BASE64), @mediaType: 图片格式(JPG格式: image/jpeg PDF格式为: application/pdf) -->";
                xml = xml + "<value xsi:type=\"ED\" mediaType=\"application/pdf\">" + pdf2base64 + "</value>";
				xml = xml + "</observationMedia>";
                xml = xml + "</component>";

                xml = xml + "</organizer>";
                xml = xml + "</entry>";


                ////<!--****************************************************************************-->
                xml = xml + "<!-- 检查报告条目 -->";
                xml = xml + "<entry typeCode=\"DRIV\">";
                xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                xml = xml + "<!-- 检查类型编码/检查类型名称 -->"; 
                //检查类型编码没有，名称暂用OT
                xml = xml + "<code code=\"OT\" codeSystem=\"1.2.156.112678.1.1.41\" displayName=\"病理\" />";
                xml = xml + "<!-- 必须固定项 -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                ////// <!-- 检查使用试剂信息 -->
                ////xml = xml + "<participant typeCode=\"CSM\">";
                ////xml = xml + "<participantRole>";
                ////xml = xml + "<playingEntity>";
                //////<!-- 试剂编码/试剂名称 -->
                ////xml = xml + "<code code=\"\" displayName=\"\" />";
                //////<!-- 试剂用量及单位 -->
                ////xml = xml + "<quantity value=\"\" unit=\"\" />";
                ////xml = xml + "</playingEntity>";
                ////xml = xml + "</participantRole>";
                ////xml = xml + "</participant>";
                xml = xml + "<!-- study -->";
                xml = xml + "<component typeCode=\"COMP\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!-- 检查项目编码/检查项目名称 -->";
                //  string yzxm = dt.Rows[0]["F_yzxm"].ToString().Trim();
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_yzxmbm"].ToString() + "\" codeSystem=\"1.2.156.112678.1.1.88\" displayName=\"" + dt_sqd.Rows[0]["F_yzxm"].ToString() + "\"/>";
                xml = xml + "<!-- 检查备注 -->";
                xml = xml + "<text></text>";
                xml = xml + "<!-- 必须固定项 -->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!-- 检查报告结果-客观所见/影像学表现(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->";
                xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112678.1.1.98\">";
                //xml = xml + "<originalText>" + System.Security.SecurityElement.Escape(dt.Rows[0]["F_rysj"].ToString());
                xml = xml + "<originalText> " + System.Security.SecurityElement.Escape(dt.Rows[0]["F_RYSJ"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_JXSJ"].ToString()) + "</originalText>";
                xml = xml + "</value>";
                // <!-- 检查报告结果-主观提示/影像学结论(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112678.1.1.98\">";
                string blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_blzd"].ToString()) + System.Security.SecurityElement.Escape(dt.Rows[0]["F_TSJC"].ToString()); ;
                if (bglx == "bc")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bczd"].ToString());
                }
                if (bglx == "bd")
                {
                    blzd = System.Security.SecurityElement.Escape(dt.Rows[0]["F_bdzd"].ToString());
                }
                xml = xml + "<originalText>" + blzd + "</originalText>";
                xml = xml + "</value>";

                xml = xml + "<!-- 检查方法编码/检查方法名称 -->";
                xml = xml + "<methodCode code=\"\"  codeSystem=\"1.2.156.112678.1.1.43\" displayName=\"\"/>";
                xml = xml + "<!-- 检查部位编码/检查部位名称 -->";
                xml = xml + "<targetSiteCode code=\"\" codeSystem=\"1.2.156112649.1.1.42\" displayName=\"\" />";
                xml = xml + "<!-- 检查医师信息 -->";
                xml = xml + "<performer typeCode=\"PRF\">";
                xml = xml + "<!-- 诊断日期 -->";
                xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss")  + "\"/>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!-- 诊断医生编码 -->";
                xml = xml + "<id  root=\"1.2.156.112678.1.1.2\" extension=\"" + bgysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                xml = xml + "<!-- 诊断医生名称 -->";
                xml = xml + "<name>" + dt.Rows[0]["F_bgys"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- 诊断科室编码 -->";
                xml = xml + "<id root=\"1.2.156.112678.1.1.1\" extension=\"2070000\"/>";
                xml = xml + "<!-- 诊断科室名称 -->";
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + " </assignedEntity>";
                xml = xml + "</performer>";


                //<!-- 检查医师信息 -->
                xml = xml + "<performer>";
                // <!-- 检查日期 -->
                try
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<time value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                xml = xml + "<assignedEntity>";
                //<!-- 检查医生编码 -->
                xml = xml + "<id  root=\"1.2.156.112678.1.1.2\" extension=\"" + qcysid + "\"/>";
                xml = xml + "<assignedPerson determinerCode=\"INSTANCE\" classCode=\"PSN\">";
                // <!-- 检查医生名称 -->
                xml = xml + "<name>" + dt.Rows[0]["F_QCYS"].ToString() + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                //<!-- 检查科室编码 -->
                xml = xml + "<id root=\"1.2.156.112678.1.1.1\" extension=\"2070000\"/>";
                //<!-- 检查科室名称 -->
                xml = xml + "<name>病理科</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                string [] F_fzys=dt.Rows[0]["F_FZYS"].ToString().Split('/');
                foreach (string fzys in F_fzys)
                {
                    if (fzys != "")
                    {
                        xml = xml + "<!-- 复诊医师信息 -->";
                        xml = xml + "<participant typeCode=\"VRF\">";
                        xml = xml + "<participantRole>";
                        xml = xml + "<!-- 复诊医生编码/复核医生 -->";
                        xml = xml + "<id  root=\"1.2.156.112678.1.1.2\" extension=\""+getyhgh(fzys)+"\"/>";
                        xml = xml + "<playingEntity>";
                        xml = xml + "<!-- 复诊医生名称 -->";
                        xml = xml + "<name>"+fzys+"</name>";
                        xml = xml + "</playingEntity>";
                        xml = xml + "</participantRole>";
                        xml = xml + "</participant>";
                    }
                }


                xml = xml + "<!-- 标本信息 -->";
                xml = xml + "<participant typeCode=\"SPC\">";
                xml = xml + "<participantRole>";
                xml = xml + "<!--标本类型-->";
                xml = xml + "<code code=\"\" displayName=\"\"></code>";	
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                xml = xml + "<!-- 仪器信息 -->";
                xml = xml + "<participant typeCode=\"DEV\">";
                xml = xml + "<participantRole>";
                xml = xml + "<playingDevice>";
                xml = xml + "<!--仪器型号 仪器名称-->";
                xml = xml + "<manufacturerModelName code=\"LOGIQ-9\"  displayName=\"Agilent Mx3000P\"/>";
                xml = xml + "</playingDevice>";
                xml = xml + "</participantRole>";
                xml = xml + "</participant>";

                //仪器或医生客观所见信息(超声心动报告等结构化所见部分的信息)
                // xml = xml + "<!-- 仪器或医生客观所见信息(超声心动报告等结构化所见部分的信息) -->";
                // xml = xml + "<entryRelationship typeCode=\"COMP\">";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"365605003\" codeSystem=\"2.16.840.1.113883.6.96\" codeSystemName=\"SNOMED CT\" displayName=\"body measurement finding\" />";
                // xml = xml + "<statusCode code=\"completed\" />";

                // xml = xml + "<!-- 项目信息(可循环) -->";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"100\" displayName=\"AOD\" />";
                // xml = xml + "<!--<value xsi:type=\"SC\">1mm</value>-->";
                // xml = xml + "<value xsi:type=\"PQ\" value=\"73\" unit=\"毫秒\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // xml = xml + "<code code=\"200\" displayName=\"LAD\" />";
                // xml = xml + "<value xsi:type=\"SC\">1mm</value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                //xml = xml + "<component>";
                //xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<code code=\"300\" displayName=\"FS\" />";
                //xml = xml + "<value xsi:type=\"SC\">33.3%</value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";

                // xml = xml + "<!-- 其它信息按上面格式添加 -->";
                // xml = xml + "</organizer>";
                // xml = xml + "</entryRelationship>";




                // <!-- 图像信息(能与项目对应的图像) -->
                //xml = xml + "<entryRelationship typeCode=\"SPRT\">";
                //xml = xml + "<observationMedia   classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/jpg\">" ;
                //xml = xml + "</value>";
                //xml = xml + "</observationMedia>";
                //xml = xml + "</entryRelationship>";
                //<!-- 当有多个影像对应同一个study时,可以复用此entryRelationship -->


                ////xml = xml + "</observation></component>";
                ////////<!-- study 2 -->
                ////xml = xml + "<component typeCode=\"COMP\">";
                ////xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                ////xml = xml + "<code code=\"1\" codeSystem=\"1.2.156.112678.1.1.88\" displayName=\"\"/>";
                ////xml = xml + "<text></text>";
                ////xml = xml + "<statusCode code=\"completed\"/>";

                //////        <!-- 检查报告结果-客观所见/影像学表现(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"01\" codeSystem=\"1.2.156.112678.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////        <!-- 检查报告结果-主观提示/影像学结论(能够与项目对应时的填写处 - @code:01表示客观所见, 02表示主观提示) -->
                ////xml = xml + "<value xsi:type=\"CD\" code=\"02\" codeSystem=\"1.2.156.112678.1.1.98\">";
                ////xml = xml + "<originalText></originalText>";
                ////xml = xml + "</value>";
                //////        <!-- 检查方法编码/检查方法名称 -->
                ////xml = xml + "<methodCode code=\"002\"  codeSystem=\"1.2.156.112678.1.1.43\" displayName=\"\"/>";
                //////        <!-- 检查部位编码/检查部位名称 -->
                ////xml = xml + "<targetSiteCode code=\"009\" codeSystem=\"1.2.156.112678.1.1.42\" displayName=\"\" />";
                //////        <!-- 检查医师信息 --

                xml = xml + "<!-- 标本采集和接收信息-->";
                xml = xml + "<entryRelationship typeCode=\"SAS\" inversionInd=\"true\">";
                xml = xml + "<procedure classCode=\"PROC\" moodCode=\"EVN\">";
                xml = xml + "<!-- 病理编号 -->";
                xml = xml + "<id extension=\""+blh+"\"/>";
                xml = xml + "<code />";
                xml = xml + "<statusCode code=\"completed\" />";
                xml = xml + "<!-- 标本采集日期(取材日期) -->";
                try
                {
                    xml = xml + "<effectiveTime value=\"" + Convert.ToDateTime(dt.Rows[0]["F_qcRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
                catch
                {
                    xml = xml + "<effectiveTime value=\"" + Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMdd") + "0000" + "\"/>";
                }
            
                xml = xml + "<!--DNA提取方法-->";
                xml = xml + "<methodCode code=\"\" displayName=\"\"/>";
                xml = xml + "<!-- 取材部位编码/取材部位名称 -->";
                xml = xml + "<targetSiteCode code=\"\" displayName=\""+dt.Rows[0]["F_bbmc"].ToString()+"\" />";

                xml = xml + "<performer>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<!--取样医生编码-->";
                xml = xml + "<id extension=\"\" root=\"1.2.156.112678.1.1.2\"></id>";
                xml = xml + "<assignedPerson>";
                xml = xml + "<!--取样医生名称-->";
                xml = xml + "<name>"+dt.Rows[0]["F_QCYS"].ToString()+"</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";

                xml = xml + "<entryRelationship typeCode=\"SAS\">";
                xml = xml + "<procedure classCode=\"TRNS\" moodCode=\"EVN\">";
                xml = xml + "<performer>";
                xml = xml + "<assignedEntity>";
                xml = xml + "<id/>";
                xml = xml + "<!--送检医院-->";
                xml = xml + "<representedOrganization>";
                xml = xml + "<name>"+dt.Rows[0]["F_SJDW"].ToString()+"</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</performer>";
                xml = xml + "<participant typeCode=\"RCV\">";
                xml = xml + "<!-- 接收时间/送检时间 -->";
                xml = xml + "<time value=\""+ Convert.ToDateTime(dt.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMMddHHmmss")+"\" />";
                xml = xml + "<participantRole/>";
                xml = xml + "</participant>";										
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";
                xml = xml + "</procedure>";
                xml = xml + "</entryRelationship>";


                xml = xml + "<entryRelationship typeCode=\"REFR\">";
                xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                xml = xml + "<!--原病理号-->";
                xml = xml + "<id extension=\"\"></id>";
                xml = xml + "<code></code>";
                xml = xml + "<!--原病理号临床诊断-->";
                xml = xml + "<value xsi:type=\"ED\"></value>";
                xml = xml + "</observation>";
                xml = xml + "</entryRelationship>";




                xml = xml + "</observation>";
                xml = xml + "</component>";

                //<!-- 其他项目按上面结构和格式添加 -->

                //<!-- 当系统所生成的报告中,图像无法与具体的study做对应时,使用以下部分来放置影像 -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observationMedia classCode=\"OBS\" moodCode=\"EVN\">";
                ////<!-- 影像信息(要求编码为BASE64), @mediaType: 影像格式 -->
                // xml = xml + "<value xsi:type=\"ED\" mediaType=\"image/gif\">";
                // xml = xml + "</value>";
                // xml = xml + "</observationMedia>";
                // xml = xml + "</component>";

                ////<!-- 当有多个影像时,按照以上格式添加 -->
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";

                // <!-- 当系统中,客观所见(和主观意见)无法对应到具体的study, 
                // 而是多个study的客观所见(和主观意见)记录在同一个文本字段中,
                // 使用以下部分来放置客观所见和主观意见 -->
                // xml = xml + "<component>";
                // xml = xml + "<organizer classCode=\"BATTERY\" moodCode=\"EVN\">";
                // xml = xml + "<statusCode code=\"completed\"/>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                // //<!-- @code:01表示客观所见, 02表示主观提示 -->
                // xml = xml + "<code code=\"01\" codeSystem=\"1.2.156.112678.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "<component>";
                // xml = xml + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //// <!-- @code:01表示客观所见, 02表示主观提示 -->
                // xml = xml + "<code code=\"02\" codeSystem=\"1.2.156.112678.1.1.98\" displayName=\"\" />";
                // xml = xml + "<value xsi:type=\"ED\"></value>";
                // xml = xml + "</observation>";
                // xml = xml + "</component>";
                // xml = xml + "</organizer>";
                // xml = xml + "</component>";


                xml = xml + "</organizer>";
                xml = xml + "</entry>";
                xml = xml + "</section>";
                xml = xml + "</component>";


                ////////********************************************************
                ////////临床资料
                ////////* *******************************************************

                //xml = xml  + "<!-- 临床资料-->";
                //xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code /><value xsi:type=\"ED\"></value></observation></entry></section></component>";
              
                
                ////////<!-- 
                ////////****************************************************************************
                ////////  #药观章节
                ////////****************************************************************************
                ////////-->

                 //xml = xml  + "<!-- 药观章节-->";
                //xml = xml + "<component><section><entry><observation classCode=\"OBS\" moodCode=\"EVN\"><code code=\"\" displayName=\"\"/></observation></entry></section></component>";
               
                
                //////////<!-- 
                ////////********************************************************
                ////////诊断
                ////////********************************************************
                ////////-->

                //xml = xml  + "<!-- 诊断-->";
                //xml = xml  + "<component><section><code code=\"29308-4\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Diagnosis\"/>";
                //xml = xml  + "<title>诊断</title>";
                //xml = xml  + "<entry typeCode=\"DRIV\">";
                //xml = xml  + "<act classCode=\"ACT\" moodCode=\"EVN\">";
                //xml = xml  + "<code nullFlavor=\"NA\"/>";
                //xml = xml  + "<entryRelationship typeCode=\"SUBJ\">";
                //xml = xml  + "<observation classCode=\"OBS\" moodCode=\"EVN\">";
                //xml = xml  + "<!-- 诊断类别编码/诊断类别名称 -->";
                //xml = xml  + "<code code=\"\" codeSystem=\"1.2.156.112678.1.1.29\" displayName=\"\" />";
                //xml = xml  + "<statusCode code=\"completed\"/>";
                //xml = xml  + "<!-- 疾病编码/疾病名称(没有编码去掉@code) -->";
                //xml = xml  + "<value xsi:type=\"\" code=\"\" codeSystem=\"1.2.156.112678.1.1.30\" displayName=\"\" />";
                //xml = xml  + "</observation>";
                //xml = xml  + "</entryRelationship>";
                //xml = xml  + "</act>";
                //xml = xml  + "</entry>";
                //xml = xml  + "</section>";
                //xml = xml  + "</component>";


                ////////********************************************************
                ////////补充意见章节（TCT检查报告单用）
                ////////********************************************************
                ////////--> 

                //xml = xml + "<!--补充意见章节（TCT检查报告单用） -->";
                //xml = xml + "<component>";
                //xml = xml + "<section>";
                //xml = xml + "<code code=\"52535-2\" codeSystem=\"2.16.840.1.113883.6.1\" codeSystemName=\"LOINC\" displayName=\"Other useful information\" />";
                //xml = xml + "<!-- 章节标题 -->";
                //xml = xml + "<title>补充意见信息</title>"; 
                //xml = xml + "<!-- 补充意见内容 -->";
                //xml = xml + "<text></text>";
                //xml = xml +  "</section>";
                //xml = xml + "</component>";

                xml = xml + "</structuredBody>";
                xml = xml + "</component>";
                xml = xml + "</ClinicalDocument>";

               

                return FormatXml(xml);
              
            }
            catch (Exception ee)
            {
                errmsg = ee.Message;
                log.WriteMyLog(ee.Message);
                return "";

            }
        }

        public void C_PDF(string ML, string blh, string bgxh, string bglx, ref  string errmsg, ref  bool isbase64, ref string Base64String, ref string filename, ref bool ScPDF, ref bool UpPDF, string debug)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

            ScPDF = false; UpPDF = false;
            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
            {
                #region  生成pdf

                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref errmsg, ref filename);
                string xy = "3";
                if (isrtn)
                {
                    //二进制串
                    if (!File.Exists(filename))
                    {
                        ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：未找到文件" + filename, "ZGQJK", "生成PDF");
                        log.WriteMyLog("未找到文件" + filename);
                        errmsg = "未找到文件" + filename;
                        zgq.DelTempFile(blh);
                        isbase64 = false;
                        return;
                    }
                    ScPDF = true;
                    if (isbase64)
                    {
                        try
                        {
                            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length];//把pdf转成 Byte型 二进制流   
                            file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   
                            file.Close();
                            Base64String = Convert.ToBase64String(imgByte);
                            isbase64 = true;
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("PDF转换二进制串失败");
                            errmsg = "PDF转换二进制串失败";
                            isbase64 = false;
                        }
                    }

                    string pdfpath = "";

                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF成功", "ZGQJK", "生成PDF");
                    bool ssa = zgq.UpPDF(blh, filename, ML, ref errmsg, int.Parse(xy),ref pdfpath);
                    if (ssa == true)
                    {
                        if (debug == "1")
                            log.WriteMyLog("上传PDF成功");
                        filename = filename.Substring(filename.LastIndexOf('\\') + 1);
                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF成功:" + ML + "\\" + filename, "ZGQJK", "上传PDF");

                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blh + bglx + bgxh + "'");
                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,F_PDFPATH) values('"+blh+bglx+bgxh+"','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + filename + "','" + pdfpath + "')");

                        UpPDF = true;
                    }
                    else
                    {
                        log.WriteMyLog("上传PDF失败：" + errmsg);
                        ZgqClass.BGHJ(blh, "上传PDF", "审核", "上传PDF失败：" + errmsg, "ZGQJK", "上传PDF");
                        UpPDF = false;
                    }
                    zgq.DelTempFile(blh);
                }
                else
                {
                    ScPDF = false;
                    log.WriteMyLog("生成PDF失败：" + errmsg);
                    ZgqClass.BGHJ(blh, "生成PDF", "审核", "生成PDF失败：" + errmsg, "ZGQJK", "生成PDF");
                    zgq.DelTempFile(blh);

                    return;
                }
                #endregion
            }
            else
            {
                log.WriteMyLog("不需生成PDF");
                return;
            }
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        //public string fsbg(string message, ref string rtnmsg, string brlb)
        //{


        //    MQQueueManager qMgr = null;
        //    Hashtable env = new Hashtable();

        //    rtnmsg = "";
        //    string hostname = f.ReadString("MQSERVER", "hostname", "192.168.171.64").Replace("\0", "");
        //    string channel = f.ReadString("MQSERVER", "channel", "IE.SVRCONN").Replace("\0", "");
        //    string qManager = f.ReadString("MQSERVER", "qManager", "GWI.QM").Replace("\0", "");
        //    string queueManager = f.ReadString("MQSERVER", "queueManager", "IN.BS366.LQ").Replace("\0", "");
        //    env.Clear();

        //    env.Add(IBM.XMS.MQC.HOST_NAME_PROPERTY, hostname);
        //    env.Add(IBM.XMS.MQC.CHANNEL_PROPERTY, channel);
        //    env.Add(IBM.XMS.MQC.CCSID_PROPERTY, 1208);
        //    env.Add(IBM.XMS.MQC.PORT_PROPERTY, 5000);
        //    env.Add(IBM.XMS.MQC.TRANSPORT_PROPERTY, IBM.XMS.MQC.TRANSPORT_MQSERIES);
        //    env.Add(IBM.XMS.MQC.USER_ID_PROPERTY, "mqm");
        //    String content = message;


        //    qMgr = new MQQueueManager(qManager, env);

        //    try
        //    {

        //        int openOptions = IBM.XMS.MQC.MQOO_OUTPUT
        //                | IBM.XMS.MQC.MQPMO_PASS_ALL_CONTEXT;
        //        MQMessage msg = new MQMessage();

        //        msg.CharacterSet = 1208;
        //        MQQueue queue = qMgr.AccessQueue(queueManager, openOptions);
        //        msg.Format = IBM.XMS.MQC.MQFMT_STRING;


        //        // 自定义属性内容请参考相关服务设计文档
        //        /// 消息ID
        //        msg.SetStringProperty("service_id", "BS004");

        //        //01 门诊,02 急诊,03 住院,04 体检,05 转院
        //        msg.SetStringProperty("domain_id", brlb);
        //        // 申请科室ID
        //        msg.SetStringProperty("apply_unit_id", "0");
        //        // 发送系统ID
        //        msg.SetStringProperty("send_sys_id", "S007");
        //        // 医疗机构代码
        //        msg.SetStringProperty("hospital_id", "44643245-7");
        //        // 执行科室ID
        //        msg.SetStringProperty("exec_unit_id", "2070000");
        //        // 医嘱执行分类编码   （医嘱类填写，报告类可默认传0）
        //        msg.SetStringProperty("order_exec_id", "0");


        //        // 扩展码
        //        msg.SetStringProperty("extend_sub_id", "0");


        //        msg.WriteString(content);

        //        MQPutMessageOptions pmo = new MQPutMessageOptions();
        //        pmo.Options = IBM.XMS.MQC.MQPMO_SYNCPOINT;
        //        queue.Put(msg, pmo);

        //        // queue.Put(aa);
        //        queue.Close();
        //        qMgr.Commit();

        //        //MessageBox.Show(byteToHexStr(msg.MessageId));
        //        string messageid = byteToHexStr(msg.MessageId);
        //        //到时将messageid保存到临时表中，历史数据存到F_yl7
        //        qMgr.Disconnect();
        //        rtnmsg = messageid;
        //        return "";

        //        // textBox1.Text = "";               
        //    }
        //    catch (Exception ex)
        //    {
        //        rtnmsg = ex.Message;
        //         LGZGQClass.log.WriteMyLog("消息发送异常！" + ex.Message);
        //        return "ERR";
        //    }

        //}


        public string getyhgh(string yhmc)
        {
            try
            {
                dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                DataTable dt_yh = aa.GetDataTable("select  top 1 F_YHBH from T_YH where F_YHMC='" + yhmc + "'", "yh");

                if (dt_yh.Rows.Count > 0)
                    return dt_yh.Rows[0]["F_YHBH"].ToString();
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }
        private string FormatXml(string sUnformattedXml)
        {
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sUnformattedXml);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = '\t';
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }
   
    }
}
