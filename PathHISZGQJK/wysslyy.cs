using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using ZgqClassPub;
namespace PathHISZGQJK
{
    class wysslyy
    {

        [DllImport(("JykBrxxDll.dll"), EntryPoint = "jcbgsave", CharSet = CharSet.Ansi, SetLastError = false)]
        public static extern string jcbgsave(string injcsj, string injcjl, string injcxm, string inzdys, string inzdsj, string inhish, string InFuJianID, string InLaiYuan, string InJcyhid, string InJcksid);

        //function jcbgsave(injcsj, injcjl, injcxm, inzdys, inzdsj, inhish: pchar): pchar; 
         //stdcall;external 'c:\yyxxxt\dll\Jykbrxxdll.dll' ;
         //传入injcsj-为检查所见 injcjl-检查结论 injcxm-检查项目 inzdys-诊断医生 inzdsj-诊断时间(时间格式yyyy-mm-dd hh:nn:ss)  inhish-His号
         //返回大写字符Y表示成功 否则为返回中文错误信息 如保存失败!或连接服务器失败!或传入串不能带引号,请检查!...

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string yymc)
        {


            string msg = f.ReadString("savetohis", "msg", "");


            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
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
            //if (bljc.Rows[0]["F_yzid"].ToString().Trim() == "")
            //{
            //     LGZGQClass.log.WriteMyLog("医嘱ID为空不回传");
            //    return;
            //}
            //if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            //{
            //     LGZGQClass.log.WriteMyLog("无申请序号（单据号），不处理！");
            //    return;
            //}
            string yhID = f.ReadString("yh", "yhbh", "").Replace("\0", "");
            string tohis = f.ReadString("savetohis", "tohis", "1").Replace("\0", "");
            string topacs = f.ReadString("savetohis", "topacs", "1").Replace("\0", "");
            string tojmjk = f.ReadString("savetohis", "tojmjk", "1").Replace("\0", "");


            if (bljc.Rows[0]["F_bgzt"].ToString().Trim() == "已审核")
            {
                if (msg == "1")
                {

                    log.WriteMyLog("-------------------------------");
                }

                string F_YZID = bljc.Rows[0]["F_yzid"].ToString().Trim();
                if (F_YZID == "")
                    F_YZID = "^";
                string[] yzid = F_YZID.Split('^');
                //---------------------------------------
                //***************************************
                //---回写蓝韵pacs
                //***************************************
                //---------------------------------------
                if (topacs == "1")
                {
                    if (msg == "1")
                        log.WriteMyLog("开始回写pacs");

                    string xmlstr = "";
                    string ReprotFile = "";

                    //-----------------------------------------------------------
                    string jpgname = "";
                    mdjpg jpgxx = new mdjpg();
                    try
                    {
                        jpgxx.BMPTOJPG(blh, ref jpgname, "cg", "1");

                        //-读图片
                        string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                        string imgPath = ftplocal + "\\" + blh + "\\"+blh+"_cg_1_1.jpg";//图片文件所在路径  
                       
                        FileStream file = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                        Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                        file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

                        file.Close();
                        ReprotFile = Convert.ToBase64String(imgByte);
                    
                        try
                        {
                            if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                                System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                        }
                        catch
                        {
                            log.WriteMyLog("回写pacs,删除临时报告JPG失败");
                        }
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("回写pacs,生成报告JPG失败");
                        ReprotFile = "";
                    }
                    //-----------------------------------------------------------------

                    try
                    {
                        xmlstr = "<?xml version='1.0' encoding='GB2312' ?><StudyInfo StationType='1'>";
                        xmlstr = xmlstr + "<PatientCode4></PatientCode4><PatientID>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</PatientID><PatientName>" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</PatientName> ";
                        xmlstr = xmlstr + "<PatientNameEn></PatientNameEn><PatientSex>" + bljc.Rows[0]["F_xb"].ToString().Trim() + "</PatientSex><DateOfBirth></DateOfBirth> ";
                        xmlstr = xmlstr + "<Citizenship /><IDCardNumber /><Occupation /><Address>" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "</Address><TelPhone /><MedicalAlert /><ContrastAllergies /> ";
                        xmlstr = xmlstr + "<RegisteDate>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegisteDate><StudyID>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</StudyID>";
                        xmlstr = xmlstr + "<BedNumber>" + bljc.Rows[0]["F_CH"].ToString().Trim() + "</BedNumber><InPatientNumber>" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "</InPatientNumber>";
                        xmlstr = xmlstr + "<ClinicSymptom>" + bljc.Rows[0]["F_lczl"].ToString().Trim() + "</ClinicSymptom><ClinicDiagnose>" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</ClinicDiagnose><RequestMemo />";
                        xmlstr = xmlstr + "<AppliedDoctor>" + bljc.Rows[0]["F_sjys"].ToString().Trim() + "</AppliedDoctor><AppliedDate></AppliedDate>";
                        xmlstr = xmlstr + " <ArrivedDate>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</ArrivedDate><BodyPart>" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "</BodyPart><Modality></Modality>";
                        xmlstr = xmlstr + " <ScanType /><FinishTime></FinishTime><AppliedDepartment>" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "</AppliedDepartment>";
                        xmlstr = xmlstr + "<Technician>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Technician><StudyDiagnose>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</StudyDiagnose>";
                        xmlstr = xmlstr + " <StudyTechnic /><StudyStatus>已审核</StudyStatus><StudyResult>" + bljc.Rows[0]["F_blzd"].ToString().Trim() + "</StudyResult><FirstReportTime>" + bljc.Rows[0]["F_bgrq"].ToString().Trim() + "</FirstReportTime>";
                        xmlstr = xmlstr + " <FirstDoctor>" + bljc.Rows[0]["F_BGYS"].ToString().Trim() + "</FirstDoctor><SecondReportTime>" + bljc.Rows[0]["F_SPARE5"].ToString().Trim() + "</SecondReportTime>";
                        xmlstr = xmlstr + "<SecondDoctor>" + bljc.Rows[0]["F_shys"].ToString().Trim() + "</SecondDoctor> ";
                        xmlstr = xmlstr + " <Register>" + bljc.Rows[0]["F_jsy"].ToString().Trim() + "</Register><Fee></Fee><Age>" + bljc.Rows[0]["F_NL"].ToString().Trim() + "</Age><Positive></Positive><ExamDepartment>病理科</ExamDepartment>   ";
                        xmlstr = xmlstr + " <ThirdDoctor /><ThirdReportTime /><Result1 /> <Result2 /> <Result3 /> <Result4 /> <Result5 /> <Result6 /> <Result7 /> <Result8 /> ";
                        xmlstr = xmlstr + " <TrackTime /> <TrackResult /> <PositiveType /> <TrackDoctor /> <PatientType>" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "</PatientType><StudyType>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</StudyType> <ModalityType>pis</ModalityType>";
                        xmlstr = xmlstr + " <IsTracked /> <Weight /> <Height /> <StudyTechnic /> <ReportType /> <AcrIndex /> <StudyCode>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</StudyCode> <HisCode1>" + yzid[1].ToString() + "</HisCode1> <HisCode2>" + yzid[0].ToString() + "</HisCode2> <HisCode3>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</HisCode3> ";
                        xmlstr = xmlstr + " <HisCode4>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</HisCode4><ReportFile>" + ReprotFile + "</ReportFile><TrueOfClinic>" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "</TrueOfClinic> </StudyInfo>";
                    }
                    catch
                    {
                        log.WriteMyLog("回传pacs，xml生成异常");
                        goto jkda;
                    }
                   
                    string rtn = "";
                    try
                    {
                        if (msg == "1")
                            log.WriteMyLog(xmlstr);
                        wys_pacs_web.CISService wys = new wys_pacs_web.CISService();

                        rtn = wys.Upload(xmlstr);
                        if (msg == "1")
                            log.WriteMyLog("回写pacs完成，返回" + rtn);
                    }
                    catch
                    {
                        log.WriteMyLog("回传pacs异常！" + rtn);

                    }


                }
                //---------------------------------------
                //***************************************
                //---回写居民健康
                //**************************************
                //---------------------------------------
                if (msg == "1")
                {

                    log.WriteMyLog("-------------------------------");
                }
            jkda: if (tojmjk == "1")
                {
                    if (msg == "1")
                        log.WriteMyLog("开始回写居民健康档案----");

                    if (bljc.Rows[0]["F_yzid"].ToString().Trim() == "")
                    {
                        log.WriteMyLog("医嘱ID为空不回传");
                        goto his;
                    }
                    if (bljc.Rows[0]["F_yzid"].ToString().Trim() == "^")
                    {
                        log.WriteMyLog("医嘱ID为空不回传");
                        goto his;
                    }
                    if (yzid[0].ToString().Trim() == "")
                    {
                        log.WriteMyLog("PATIENT_ID为空不回传");
                        goto his;
                    }
                    if (yzid[1].ToString().Trim() == "")
                    {
                        log.WriteMyLog("EVENT_ID为空不回传");
                        goto his;
                    }


                    string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
                    int lb = 0;
                    if (brlb == "住院")
                        lb = 2;
                    if (brlb == "门诊")
                        lb = 1;

                    string sex_id = "0";
                    if (bljc.Rows[0]["F_XB"].ToString().Trim() == "男")
                        sex_id = "1";
                    if (bljc.Rows[0]["F_XB"].ToString().Trim() == "女")
                        sex_id = "2";

                    int csrq = (2013 - int.Parse((bljc.Rows[0]["F_nl"].ToString().Trim().Split('岁'))[0].ToString()) + 1);

                    string intostr = "";
                    string qcrq = bljc.Rows[0]["F_QCrq"].ToString().Trim();
                    if (qcrq == "")
                        qcrq = bljc.Rows[0]["F_sdrq"].ToString().Trim();
                    try
                    {
                        intostr = "insert into CEHR_EXAM_MASTER(MASTER_ID,PATIENT_ID,EVENT_TYPE,EVENT_NO,RETRIEVE_DATE,CLASS_CODE,CLASS_NAME,BODY_PART_CODE,BODY_PART_NAME,EXAM_CODE,EXAM_NAME,TITLE,EFFECTIVE_TIME,CONFIDENTIALITY,";
                        intostr = intostr + "PATIENT_NAME,PATIENT_SEX,PATIENT_SEX_DETAIL,PATIENT_BIRTHDAY,AUTHOR_ID,AUTHOR_TIME,AUTHOR_NAME,AUTHENTICATOR_ID,AUTHENTICATOR_TIME,AUTHENTICATOR_NAME,PARTICIPANT_DEPT,PARTICIPANT_ID,PARTICIPANT_TIME,PARTICIPANT_NAME,ORDER_ID,";
                        intostr = intostr + "ORDER_PRIORITY,ORDER_PRIORITY_NAME,PERFORMER_DEPT_NAME,PERFORMER_DOCTOR,PERFORMER_TIME,PLAYING_DEVICE,CLINIC_DIAGNOSIS,PATIENT_CONDITION,EXAM_CAUSE,DESCRIPTION,IS_ABNORMAL) ";
                        intostr = intostr + "values('" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "','" + yzid[0].ToString() + "'," + lb + ",'" + yzid[1].ToString() + "',to_date('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'','','','";
                        intostr = intostr + bljc.Rows[0]["F_BBMC"].ToString().Trim() + "','','" + "病理检查" + "','病理" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "检查报告单',to_date('" + DateTime.Parse(bljc.Rows[0]["F_SDRQ"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'N','";
                        intostr = intostr + bljc.Rows[0]["F_XM"].ToString().Trim() + "','" + sex_id + "','" + bljc.Rows[0]["F_xb"].ToString().Trim() + "',to_date('" + DateTime.Parse(csrq.ToString() + "-01-01").ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + getyhbh(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "',to_date('" + DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss");
                        intostr = intostr + "','YYYY-MM-DD HH24:MI:SS'),'" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "','" + getyhbh(bljc.Rows[0]["F_shys"].ToString().Trim()) + "',to_date('" + DateTime.Parse(bljc.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS'),'" + bljc.Rows[0]["F_shys"].ToString().Trim() + "','";
                        intostr = intostr + bljc.Rows[0]["F_sjks"].ToString().Trim() + "','','','" + bljc.Rows[0]["F_sjys"].ToString().Trim() + "','" + bljc.Rows[0]["F_SQXH"].ToString().Trim() + "','R','','病理科','";
                        intostr = intostr + bljc.Rows[0]["F_qcys"].ToString().Trim() + "',to_date('" + DateTime.Parse(qcrq) + "','YYYY-MM-DD HH24:MI:SS'),'','" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "','','','" + bljc.Rows[0]["F_BLZD"].ToString().Trim() + "','')";

                    }
                    catch (Exception eee)
                    {
                        log.WriteMyLog("回传健康档案异常！拼接语句错误" + eee.ToString());
                        log.WriteMyLog(intostr);
                        goto his;
                    }
                    try
                    {
                        int xxxxx = insert_orcl(intostr, "");
                        if (msg == "1")
                            log.WriteMyLog("回写居民健康档案完成,影响行数" + xxxxx);

                    }
                    catch
                    {
                        log.WriteMyLog("回传健康档案异常！执行插入语句异常2");
                        return;
                    }

                }
                //------------------------------
                //---回写his
                //------------------------------
                if (msg == "1")
                {

                    log.WriteMyLog("-------------------------------");
                }
            his: if (tohis == "1")
                {
                    if (msg == "1")
                        log.WriteMyLog("开始回写his----");

                    if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
                    {
                        log.WriteMyLog("无申请序号（单据号），不处理！");
                        return;
                    }
                    string rtn = "";
                    try
                    { //来源 : 1 门诊，2 住院，
                        string brlb = bljc.Rows[0]["F_brlb"].ToString().Trim();
                        string InLaiYuan = "1";

                        if (brlb == "住院")
                            InLaiYuan = "2";

                        try
                        {
                            rtn = jcbgsave(bljc.Rows[0]["F_rysj"].ToString().Trim(), bljc.Rows[0]["F_blzd"].ToString().Trim(), "病理" + bljc.Rows[0]["F_blk"].ToString().Trim() + "检查", bljc.Rows[0]["F_bgys"].ToString().Trim(), DateTime.Parse(bljc.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyy-MM-dd HH:mm:ss"), bljc.Rows[0]["F_brbh"].ToString().Trim(), bljc.Rows[0]["F_SQXH"].ToString().Trim(), InLaiYuan, getyhbh(bljc.Rows[0]["F_BGYS"].ToString().Trim()), "JC12");
                            if (rtn.Trim() == "Y")
                            {
                                if (msg == "1")
                                    log.WriteMyLog("回写his完成，返回" + rtn);

                                return;
                            }
                            else
                            {
                                log.WriteMyLog("回写his错误，返回" + rtn);
                                return;
                            }
                        }
                        catch (Exception eReport)
                        {

                            log.WriteMyLog("回写his异常，" + eReport.ToString());
                            return;
                        }
                    }
                    catch (Exception ee)
                    {
                        log.WriteMyLog("回写his异常，" + ee.ToString());
                        return;
                    }


                }

                //---结束-------------------------------------
            }
            else
            {
                //冰冻

              
                DataTable blbd = new DataTable();
                blbd = aa.GetDataTable("select * from T_BDBG where F_bd_bgzt='已审核' and F_blh='" + blh + "'", "blbd");
                if (blbd == null)
                {
                    MessageBox.Show("病理数据库设置有问题！");
                    log.WriteMyLog("病理数据库设置有问题！");
                    return;
                }
                if (blbd.Rows.Count > 0)
                {
                    //上传冰冻报告 to 蓝韵pacs

                    string F_YZID = bljc.Rows[0]["F_yzid"].ToString().Trim();
                    if (F_YZID == "")
                        F_YZID = "^";
                    string[] yzid = F_YZID.Split('^');

                    if (topacs == "1")
                    {
                        string xmlstr = "";
                        string ReprotFile = "";

                        //-----------------------------------------------------------
                        string jpgname = "";
                        mdjpg jpgxx = new mdjpg();
                        try
                        {
                            jpgxx.BMPTOJPG(blh, ref jpgname, "bd", "1");

                            //-读图片
                            string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                            string imgPath = ftplocal + "\\" + blh + "\\"+blh+"_bd_1_1.jpg";//图片文件所在路径  
                          
                            FileStream file = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                            Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
                            file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

                            file.Close();
                            ReprotFile = Convert.ToBase64String(imgByte);
                          
                            try
                            {
                                //if (System.IO.Directory.Exists(@"c:\temp\" + blh))
                               //     System.IO.Directory.Delete(@"c:\temp\" + blh, true);
                            }
                            catch
                            {
                                log.WriteMyLog("回写pacs,删除临时冰冻报告JPG失败");
                            }
                        }
                        catch (Exception ee)
                        {
                            log.WriteMyLog("回写pacs,生成冰冻报告JPG失败");
                            ReprotFile = "";
                        }
                        //-----------------------------------------------------------------

                        try
                        {
                            xmlstr = "<?xml version='1.0' encoding='GB2312' ?><StudyInfo StationType='1'>";
                            xmlstr = xmlstr + "<PatientCode4></PatientCode4><PatientID>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</PatientID><PatientName>" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</PatientName> ";
                            xmlstr = xmlstr + "<PatientNameEn></PatientNameEn><PatientSex>" + bljc.Rows[0]["F_xb"].ToString().Trim() + "</PatientSex><DateOfBirth></DateOfBirth> ";
                            xmlstr = xmlstr + "<Citizenship /><IDCardNumber /><Occupation /><Address>" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "</Address><TelPhone /><MedicalAlert /><ContrastAllergies /> ";
                            xmlstr = xmlstr + "<RegisteDate>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegisteDate><StudyID>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</StudyID>";
                            xmlstr = xmlstr + "<BedNumber>" + bljc.Rows[0]["F_CH"].ToString().Trim() + "</BedNumber><InPatientNumber>" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "</InPatientNumber>";
                            xmlstr = xmlstr + "<ClinicSymptom>" + bljc.Rows[0]["F_lczl"].ToString().Trim() + "</ClinicSymptom><ClinicDiagnose>" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</ClinicDiagnose><RequestMemo />";
                            xmlstr = xmlstr + "<AppliedDoctor>" + bljc.Rows[0]["F_sjys"].ToString().Trim() + "</AppliedDoctor><AppliedDate></AppliedDate>";
                            xmlstr = xmlstr + " <ArrivedDate>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</ArrivedDate><BodyPart>" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "</BodyPart><Modality></Modality>";
                            xmlstr = xmlstr + " <ScanType /><FinishTime></FinishTime><AppliedDepartment>" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "</AppliedDepartment>";
                            xmlstr = xmlstr + "<Technician>" + blbd.Rows[0]["F_BD_bgys"].ToString().Trim() + "</Technician><StudyDiagnose>" + bljc.Rows[0]["F_rysj"].ToString().Trim() + "</StudyDiagnose>";
                            xmlstr = xmlstr + " <StudyTechnic /><StudyStatus>已审核</StudyStatus><StudyResult>" + blbd.Rows[0]["F_bdzd"].ToString().Trim() + "</StudyResult><FirstReportTime>" + blbd.Rows[0]["F_bd_bgrq"].ToString().Trim() + "</FirstReportTime>";
                            xmlstr = xmlstr + " <FirstDoctor>" + blbd.Rows[0]["F_bd_BGYS"].ToString().Trim() + "</FirstDoctor><SecondReportTime>" + blbd.Rows[0]["F_BD_SDRQ"].ToString().Trim() + "</SecondReportTime>";
                            xmlstr = xmlstr + "<SecondDoctor>" + blbd.Rows[0]["F_bd_shys"].ToString().Trim() + "</SecondDoctor> ";
                            xmlstr = xmlstr + " <Register>" + bljc.Rows[0]["F_jsy"].ToString().Trim() + "</Register><Fee></Fee><Age>" + bljc.Rows[0]["F_NL"].ToString().Trim() + "</Age><Positive></Positive><ExamDepartment>病理科</ExamDepartment>   ";
                            xmlstr = xmlstr + " <ThirdDoctor /><ThirdReportTime /><Result1 /> <Result2 /> <Result3 /> <Result4 /> <Result5 /> <Result6 /> <Result7 /> <Result8 /> ";
                            xmlstr = xmlstr + " <TrackTime /> <TrackResult /> <PositiveType /> <TrackDoctor /> <PatientType>" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "</PatientType><StudyType>" + "冰冻" + "</StudyType> <ModalityType>pis</ModalityType>";
                            xmlstr = xmlstr + " <IsTracked /> <Weight /> <Height /> <StudyTechnic /> <ReportType /> <AcrIndex /> <StudyCode>" + bljc.Rows[0]["F_blh"].ToString().Trim() + "</StudyCode> <HisCode1>" + yzid[1].ToString() + "</HisCode1> <HisCode2>" + yzid[0].ToString() + "</HisCode2> <HisCode3>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</HisCode3> ";
                            xmlstr = xmlstr + " <HisCode4>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</HisCode4><ReportFile>" + ReprotFile + "</ReportFile><TrueOfClinic>" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "</TrueOfClinic> </StudyInfo>";
                        }
                        catch
                        {
                            log.WriteMyLog("回传pacs，xml生成异常");
                        }
                        string rtn = "";

                 
                        try
                        {
                            if (msg == "1")
                                log.WriteMyLog(xmlstr);
                            wys_pacs_web.CISService wys = new wys_pacs_web.CISService();

                            rtn = wys.Upload(xmlstr);
                            if (msg == "1")
                                log.WriteMyLog("回写pacs完成，返回" + rtn);
                        }
                        catch
                        {
                            log.WriteMyLog("回传pacs异常！" + rtn);

                        }

                    }

                }
               
            } return;
        }

        //执行oracle数据库查询操作，insert，update，delete，传sql语句，返回影响行数
        private int insert_orcl(string orcl_strsql, string sm)
        {
            string constr = "Provider='MSDAORA';data source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.0.215)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = cehr)));user id =cehr;password=Cehr";
          
            OleDbConnection orcl_con = new OleDbConnection(constr);
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
                orcl_con.Close(); ocdc.Dispose();
             
                 log.WriteMyLog("操作orcal数据库异常--" + insert_ee.ToString());
                return 0;
            }
            return x;

        }
        private string getyhbh(string yhmc)
        {
            if (yhmc.Trim() == "")
                return "";

            dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select F_YHBH from T_yh where F_yhmc='" + yhmc + "'", "yhbh");
            aa.Close();
            if (bljc.Rows.Count == 0)
                return "";
            return bljc.Rows[0]["F_yhbh"].ToString();
        }
    }
}
