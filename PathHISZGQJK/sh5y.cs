using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Data.OracleClient;
using System.Windows.Forms;
using System.Data.SqlClient;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class sh5y
    {


            private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void pathtohis(string blh, string debug)
        {

            string msg = f.ReadString("savetohis", "debug", "");
            string constr = f.ReadString("savetohis", "odbcsql", "");
        
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");
          
            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
              log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }
            if (bljc.Rows[0]["F_BGZT"].ToString().Trim() == "已审核")
            {
                //查询病理视图 CDR_Report_PE
                DataTable bl_CDR_Report_PE = new DataTable();
                bl_CDR_Report_PE = aa.GetDataTable("select * from CDR_Report_PE where PEID='" + blh + "'", "bljc");

                //查询病理视图 CDR_Report_PE_TBS
                DataTable bl_CDR_Report_PE_TBS = new DataTable();
                bl_CDR_Report_PE_TBS = aa.GetDataTable("select * from CDR_Report_PE_TBS where DCID='" + blh + "'", "bljc");

                if (bl_CDR_Report_PE.Rows.Count > 0)
                {
                    //查询his中间表CDR_Report_PE
                    string select_CDR_Report_PE = "select *  from CDR_Report_PE where DCID='" + blh + "'";
                  
                    DataTable dt_CDR_Report_PE = new DataTable();
                    
                    
                        dt_CDR_Report_PE = Oracle_Select(select_CDR_Report_PE, constr);
                  
                    if (dt_CDR_Report_PE.Rows.Count > 0)
                    {
                        //存在则修改数据
                        try
                        {
                            //不存在则增加数据
                            string select_to_CDR_Report_PE = "update CDR_Report_PE  set "
                            + "PatientID='" + bl_CDR_Report_PE.Rows[0]["PatientID"].ToString().Trim() + "',PatientType='" + bl_CDR_Report_PE.Rows[0]["PatientType"].ToString().Trim()
                            + "',VisitID='" + bl_CDR_Report_PE.Rows[0]["VisitID"].ToString().Trim() + "',EffectiveFlag='" + bl_CDR_Report_PE.Rows[0]["EffectiveFlag"].ToString().Trim() + "',AuthorOrganization='" + bl_CDR_Report_PE.Rows[0]["AuthorOrganization"].ToString().Trim()
                            + "',AuthorOrganizationName='" + bl_CDR_Report_PE.Rows[0]["AuthorOrganizationName"].ToString().Trim() + "',ExaminationType='" + bl_CDR_Report_PE.Rows[0]["ExaminationType"].ToString().Trim() + "',"
                           + "ClinicID='" + bl_CDR_Report_PE.Rows[0]["ClinicID"].ToString().Trim() + "',HospizationID='" + bl_CDR_Report_PE.Rows[0]["HospizationID"].ToString().Trim() + "',Name='" + bl_CDR_Report_PE.Rows[0]["Name"].ToString().Trim() + "',"
                           + "Sex='" + bl_CDR_Report_PE.Rows[0]["Sex"].ToString().Trim() + "',Age='" + bl_CDR_Report_PE.Rows[0]["Age"].ToString().Trim() + "',"
                            + "DeptName='" + bl_CDR_Report_PE.Rows[0]["DeptName"].ToString().Trim() + "',WardArea='" + bl_CDR_Report_PE.Rows[0]["WardArea"].ToString().Trim() + "',"
                           + "SickbedID='" + bl_CDR_Report_PE.Rows[0]["SickbedID"].ToString().Trim() + "',DiagnoseCode='" + bl_CDR_Report_PE.Rows[0]["DiagnoseCode"].ToString().Trim() + "',DiagnoseName='" + bl_CDR_Report_PE.Rows[0]["DiagnoseName"].ToString().Trim() + "',"
                           + "ChiefComplaint='" + bl_CDR_Report_PE.Rows[0]["ChiefComplaint"].ToString().Trim() + "',DiseasesHistory='" + bl_CDR_Report_PE.Rows[0]["DiseasesHistory"].ToString().Trim() + "',RefExaminationResult='" + bl_CDR_Report_PE.Rows[0]["RefExaminationResult"].ToString().Trim() + "',"
                           + "OptOrESDisplay='" + bl_CDR_Report_PE.Rows[0]["OptOrESDisplay"].ToString().Trim() + "',"
                            + "FemaleMenses='" + bl_CDR_Report_PE.Rows[0]["FemaleMenses"].ToString().Trim() + "',FemalePregnancy='" + bl_CDR_Report_PE.Rows[0]["FemalePregnancy"].ToString().Trim() + "',"
                            + "FemaleIsMenopausal='" + bl_CDR_Report_PE.Rows[0]["FemaleIsMenopausal"].ToString().Trim() + "',PathologoType='" + bl_CDR_Report_PE.Rows[0]["PathologoType"].ToString().Trim() + "',SampleType='" + bl_CDR_Report_PE.Rows[0]["SampleType"].ToString().Trim() + "',"
                            + "SampleTypeName='" + bl_CDR_Report_PE.Rows[0]["SampleTypeName"].ToString().Trim() + "',SampleToponymy='" + bl_CDR_Report_PE.Rows[0]["SampleToponymy"].ToString().Trim() + "',SampleExecutor='" + bl_CDR_Report_PE.Rows[0]["SampleExecutor"].ToString().Trim() + "',"
                            + "SampleExecuteTime=to_date('" +bl_CDR_Report_PE.Rows[0]["SampleExecuteTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS'),SampleContainer='" + bl_CDR_Report_PE.Rows[0]["SampleContainer"].ToString().Trim() + "',"
                            + "SampleFixativeType='" + bl_CDR_Report_PE.Rows[0]["SampleFixativeType"].ToString().Trim() + "',SampleFixativeAmout='" + bl_CDR_Report_PE.Rows[0]["SampleFixativeAmout"].ToString().Trim() + "',SampleSizeType='" + bl_CDR_Report_PE.Rows[0]["SampleSizeType"].ToString().Trim() + "',"
                            + "SampleDescribe='" + bl_CDR_Report_PE.Rows[0]["SampleDescribe"].ToString().Trim() + "',SampleReceiveTime=to_date('" + bl_CDR_Report_PE.Rows[0]["SampleReceiveTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS'),SampleReceiver='" + bl_CDR_Report_PE.Rows[0]["SampleReceiver"].ToString().Trim() + "',"
                            + "SampleStatus='" + bl_CDR_Report_PE.Rows[0]["SampleStatus"].ToString().Trim() + "',ExaminationMethod='" + bl_CDR_Report_PE.Rows[0]["ExaminationMethod"].ToString().Trim() + "',GrossStructure='" + bl_CDR_Report_PE.Rows[0]["GrossStructure"].ToString().Trim() + "',"
                            + "MicroscopeDisplay='" + bl_CDR_Report_PE.Rows[0]["MicroscopeDisplay"].ToString().Trim() + "',"
                            + "SpecialExamination='" + bl_CDR_Report_PE.Rows[0]["SpecialExamination"].ToString().Trim() + "',ExaminationResult='" + bl_CDR_Report_PE.Rows[0]["ExaminationResult"].ToString().Trim() + "',MasculineResult='" + bl_CDR_Report_PE.Rows[0]["MasculineResult"].ToString().Trim() + "',"
                            + "Suggestion='" + bl_CDR_Report_PE.Rows[0]["Suggestion"].ToString().Trim() + "',"
                            + "ClinicalRate='" + bl_CDR_Report_PE.Rows[0]["ClinicalRate"].ToString().Trim() + "',FrozenRate='" + bl_CDR_Report_PE.Rows[0]["FrozenRate"].ToString().Trim() + "',ReportDoctorCode='" + bl_CDR_Report_PE.Rows[0]["ReportDoctorCode"].ToString().Trim() + "',"
                            + "ReportDoctor='" + bl_CDR_Report_PE.Rows[0]["ReportDoctor"].ToString().Trim() + "',ReportDateTime=to_date('" + bl_CDR_Report_PE.Rows[0]["ReportDateTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS'),ReviewDoctorCode='" + bl_CDR_Report_PE.Rows[0]["ReviewDoctorCode"].ToString().Trim() + "',"
                            + "ReviewDoctor='" + bl_CDR_Report_PE.Rows[0]["ReviewDoctor"].ToString().Trim() + "',"
                            + "ReviewDateTime=to_date('" + bl_CDR_Report_PE.Rows[0]["ReviewDateTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS'),ReportMark='" + bl_CDR_Report_PE.Rows[0]["ReportMark"].ToString().Trim() + "',ReportPrintFlag='" + bl_CDR_Report_PE.Rows[0]["ReportPrintFlag"].ToString().Trim() + "',"
                            + "ExaminationDevice='" + bl_CDR_Report_PE.Rows[0]["ExaminationDevice"].ToString().Trim() + "',FileType='" + bl_CDR_Report_PE.Rows[0]["FileType"].ToString().Trim() + "',"
                            + "FileName='" + bl_CDR_Report_PE.Rows[0]["FileName"].ToString().Trim() + "',SystemReportType='" + bl_CDR_Report_PE.Rows[0]["SystemReportType"].ToString().Trim() + "',TimeStamp=to_date('" + bl_CDR_Report_PE.Rows[0]["TimeStamp"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS')  where DCID='" + blh + "'";

                            if (debug == "1")
                                log.WriteMyLog(select_to_CDR_Report_PE);

                            int x = ExecuteNonQuery(select_to_CDR_Report_PE, constr);
                            if (x > 0)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("修改CDR_Report_PE成功");
                                aa.ExecuteSQL("update T_JCXX set F_SCBZ='1'  where F_BLH='" + blh + "'");
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog("修改CDR_Report_PE失败");
                            }

                           
                        }
                        catch (Exception ee2)
                        {
                            log.WriteMyLog(ee2.Message);
                            return;
                        }

                    }
                    else
                    {
                        try
                        {
                            //不存在则增加数据
                            string insert_to_CDR_Report_PE = "insert into CDR_Report_PE(DCID,PatientID,PatientType,VisitID, EffectiveFlag, AuthorOrganization, AuthorOrganizationName, ExaminationType,PEID,"
                           + "ClinicID,HospizationID,Name,Sex,Age,MonthAge,DeptCode,DeptName,WardArea,SickbedID,DiagnoseCode,DiagnoseName,ChiefComplaint,DiseasesHistory,RefExaminationResult,OptOrESDisplay,"
                            + "FemaleMenses,FemalePregnancy,FemaleIsMenopausal,PathologoType,SampleType,SampleTypeName,SampleToponymy,SampleExecutor,SampleExecuteTime,SampleContainer,"
                            + "SampleFixativeType,SampleFixativeAmout,SampleSizeType,SampleDescribe,SampleReceiveTime,SampleReceiver,SampleStatus,ExaminationMethod,GrossStructure,MicroscopeDisplay,"
                            + "SpecialExamination,ExaminationResult,MasculineResult,Suggestion,ClinicalRate,FrozenRate,ReportDoctorCode,ReportDoctor,ReportDateTime, ReviewDoctorCode,ReviewDoctor,"
                            + "ReviewDateTime, ReportMark,ReportPrintFlag, ImageAmount,ExaminationDevice,FileType,FileName,SystemReportType,TimeStamp) values('"
                            + bl_CDR_Report_PE.Rows[0]["DCID"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["PatientID"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["PatientType"].ToString().Trim()
                            + "','" + bl_CDR_Report_PE.Rows[0]["VisitID"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["EffectiveFlag"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["AuthorOrganization"].ToString().Trim()
                            + "','" + bl_CDR_Report_PE.Rows[0]["AuthorOrganizationName"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["ExaminationType"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["PEID"].ToString().Trim() + "','"
                           + bl_CDR_Report_PE.Rows[0]["ClinicID"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["HospizationID"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["Name"].ToString().Trim() + "','"
                           + bl_CDR_Report_PE.Rows[0]["Sex"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["Age"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["MonthAge"].ToString().Trim() + "','"
                           + bl_CDR_Report_PE.Rows[0]["DeptCode"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["DeptName"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["WardArea"].ToString().Trim() + "','"
                           + bl_CDR_Report_PE.Rows[0]["SickbedID"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["DiagnoseCode"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["DiagnoseName"].ToString().Trim() + "','"
                           + bl_CDR_Report_PE.Rows[0]["ChiefComplaint"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["DiseasesHistory"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["RefExaminationResult"].ToString().Trim() + "','"
                           + bl_CDR_Report_PE.Rows[0]["OptOrESDisplay"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["FemaleMenses"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["FemalePregnancy"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["FemaleIsMenopausal"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["PathologoType"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["SampleType"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["SampleTypeName"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["SampleToponymy"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["SampleExecutor"].ToString().Trim() + "',"
                            + "to_date('" + bl_CDR_Report_PE.Rows[0]["SampleExecuteTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS')),'" + bl_CDR_Report_PE.Rows[0]["SampleContainer"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["SampleFixativeType"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["SampleFixativeAmout"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["SampleSizeType"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["SampleDescribe"].ToString().Trim() + "',to_date('" + bl_CDR_Report_PE.Rows[0]["SampleReceiveTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS')),'" + bl_CDR_Report_PE.Rows[0]["SampleReceiver"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["SampleStatus"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["ExaminationMethod"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["GrossStructure"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["MicroscopeDisplay"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["SpecialExamination"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["ExaminationResult"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["MasculineResult"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["Suggestion"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["ClinicalRate"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["FrozenRate"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["ReportDoctorCode"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["ReportDoctor"].ToString().Trim() + "', to_date('" + bl_CDR_Report_PE.Rows[0]["ReportDateTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS')),'" + bl_CDR_Report_PE.Rows[0]["ReviewDoctorCode"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["ReviewDoctor"].ToString().Trim() + "',"
                            + "to_date('" + bl_CDR_Report_PE.Rows[0]["ReviewDateTime"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS')),'" + bl_CDR_Report_PE.Rows[0]["ReportMark"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["ReportPrintFlag"].ToString().Trim() + "',"
                            + bl_CDR_Report_PE.Rows[0]["ImageAmount"].ToString().Trim() + ",'" + bl_CDR_Report_PE.Rows[0]["ExaminationDevice"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["FileType"].ToString().Trim() + "','"
                            + bl_CDR_Report_PE.Rows[0]["FileName"].ToString().Trim() + "','" + bl_CDR_Report_PE.Rows[0]["SystemReportType"].ToString().Trim() + "', to_date('" + bl_CDR_Report_PE.Rows[0]["TimeStamp"].ToString().Trim() + "','YYYY-MM-DD HH24:MI:SS'))";

                            if (debug == "1")
                                log.WriteMyLog(insert_to_CDR_Report_PE);
                            int x = ExecuteNonQuery(insert_to_CDR_Report_PE,constr);
                            if (x > 0)
                            {
                                if (debug == "1")
                                    log.WriteMyLog("插入CDR_Report_PE成功");
                                aa.ExecuteSQL("update T_JCXX set F_SCBZ='1'  where F_BLH='" + blh + "'");
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog("插入CDR_Report_PE失败");
                            }
                        }
                        catch (Exception ee2)
                        {
                            log.WriteMyLog(ee2.Message);
                        }
                    }
                    //his CDR_Report_PE_TBS表
                    try
                    {
                        if (bl_CDR_Report_PE_TBS.Rows.Count > 0)
                        {
                            //查询hisCDR_Report_PE_TBS有无数据
                            DataTable CDR_Report_PE_TBS = new DataTable();
                            dt_CDR_Report_PE = Oracle_Select("select * from CDR_Report_PE_TBS  where DCID='"+blh+"'", constr);

                            if (dt_CDR_Report_PE.Rows.Count > 0)
                            {
                                //修改
                                string update_to_CDR_Report_PE_TBS = " update  CDR_Report_PE_TBS  set "
                                      + "SpecimenSatisfactory='" + bl_CDR_Report_PE_TBS.Rows[0]["SpecimenSatisfactory"].ToString().Trim() + "',CellQuantity='" + bl_CDR_Report_PE_TBS.Rows[0]["CellQuantity"].ToString().Trim() + "',NeckCell='" + bl_CDR_Report_PE_TBS.Rows[0]["NeckCell"].ToString().Trim()
                                      + "',MetaplasiaCell='" + bl_CDR_Report_PE_TBS.Rows[0]["MetaplasiaCell"].ToString().Trim() + "',phlogosisDegree='" + bl_CDR_Report_PE_TBS.Rows[0]["phlogosisDegree"].ToString().Trim() + "',HaemophilusInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["HaemophilusInfection"].ToString().Trim() + "',TrichomonasInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["TrichomonasInfection"].ToString().Trim()
                                      + "',CandidaInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["CandidaInfection"].ToString().Trim() + "',HPVInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["HPVInfection"].ToString().Trim() + "',HSVInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["HSVInfection"].ToString().Trim() + "',CoccobacillusInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["CoccobacillusInfection"].ToString().Trim()
                                      + "',ActinomycoticInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["ActinomycoticInfection"].ToString().Trim() + "',LeptothrixInfection='" + bl_CDR_Report_PE_TBS.Rows[0]["LeptothrixInfection"].ToString().Trim() + "' where DCID='"+blh+"'";

                                if (debug == "1")
                                    log.WriteMyLog(update_to_CDR_Report_PE_TBS);

                                int y = 0;
                               y= ExecuteNonQuery(update_to_CDR_Report_PE_TBS, constr);
                                if (y > 0)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("修改CDR_Report_PE_TBS成功");

                                }
                                else
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("修改CDR_Report_PE_TBS失败");
                                    return;
                                }

                            }
                            else
                            {
                                //插入
                                string insert_to_CDR_Report_PE_TBS = " insert into CDR_Report_PE_TBS (DocID,DCID,SpecimenSatisfactory,CellQuantity,NeckCell,MetaplasiaCell,phlogosisDegree,HaemophilusInfection,TrichomonasInfection,CandidaInfection,HPVInfection,HSVInfection,CoccobacillusInfection,ActinomycoticInfection,LeptothrixInfection)"
                               + "values('" + bl_CDR_Report_PE_TBS.Rows[0]["DocID"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["DCID"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["SpecimenSatisfactory"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["CellQuantity"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["NeckCell"].ToString().Trim()
                               + "','" + bl_CDR_Report_PE_TBS.Rows[0]["MetaplasiaCell"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["phlogosisDegree"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["HaemophilusInfection"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["TrichomonasInfection"].ToString().Trim() + "','"
                               + bl_CDR_Report_PE_TBS.Rows[0]["CandidaInfection"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["HPVInfection"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["HSVInfection"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["CoccobacillusInfection"].ToString().Trim()
                               + "','" + bl_CDR_Report_PE_TBS.Rows[0]["ActinomycoticInfection"].ToString().Trim() + "','" + bl_CDR_Report_PE_TBS.Rows[0]["LeptothrixInfection"].ToString().Trim() + "')";

                                if (debug == "1")
                                    log.WriteMyLog(insert_to_CDR_Report_PE_TBS);

                                int y = 0;
                              y=  ExecuteNonQuery(insert_to_CDR_Report_PE_TBS, constr);
                                if (y > 0)
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("插入CDR_Report_PE_TBS成功");

                                }
                                else
                                {
                                    if (debug == "1")
                                        log.WriteMyLog("插入CDR_Report_PE_TBS失败");
                                    return;
                                }
                            }
                        }
                    }catch(Exception  ee4)
                    {
                        log.WriteMyLog(ee4.Message);
                    }
                    }

            }
            else
            {
                //取消审核
                //修改数据 EffectiveFlag=0
                if (bljc.Rows[0]["F_SCBZ"].ToString().Trim() == "1")
                {
                    try
                    {

                        string update_to_CDR_Report_PE = "update CDR_Report_PE  set EffectiveFlag='0' where DCID='" + blh + "'";

                        if (debug == "1")
                            log.WriteMyLog(update_to_CDR_Report_PE);


                        int x = ExecuteNonQuery(update_to_CDR_Report_PE,constr);
                        if (x > 0)
                        {
                            if (debug == "1")
                                log.WriteMyLog("取消审核标志成功");
                            aa.ExecuteSQL("update T_JCXX set F_SCBZ='0'  where F_BLH='" + blh + "'");
                        }
                        else
                        {
                            if (debug == "1")
                            log.WriteMyLog("取消审核标志成功");
                        }
                    }
                    catch (Exception ee2)
                    {
                        log.WriteMyLog(ee2.Message);
                    }

                }
            }

        }
        public DataTable Oracle_Select(string sqlstr, string constr)
        {
            DataTable dt = new DataTable();
            OracleConnection orccon = new OracleConnection(constr);
            OracleDataAdapter orcdap = new OracleDataAdapter(sqlstr, orccon);
         
            try
            {
                orcdap.Fill(dt);
                orcdap.Dispose();
                orccon.Close();
                return dt;
            }
            catch (Exception e)
            {
                log.WriteMyLog("执行语句出错：" + e.Message + "\r\n" + sqlstr);
                orcdap.Dispose();
                orccon.Close();
                return dt;
            }
        }
        public int ExecuteNonQuery(string sqlstr, string constr)
        {
            DataTable dt = new DataTable();
            OracleConnection orccon = new OracleConnection(constr);
            OracleCommand cmd = new OracleCommand(sqlstr, orccon);
        
            int x = -1;
            try
            {
                orccon.Open();
               x= cmd.ExecuteNonQuery();
                orccon.Close();
                return x;
            }
            catch (Exception e)
            {
                log.WriteMyLog("执行语句出错：" + e.Message + "\r\n" + sqlstr);
                orccon.Close();
                return -1;
            }
        }

        
    }
}
