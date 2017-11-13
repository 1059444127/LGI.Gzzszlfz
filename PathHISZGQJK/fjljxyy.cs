using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using dbbase;
using System.Windows.Forms;
using ZgqClassPub;
using ZgqClassPub.DBData;


namespace PathHISZGQJK
{
    class fjljxyy
    {
        // 福建连江县医院

        //[DllImport(("GateWay.dll"), EntryPoint = "ConnectService", CharSet = CharSet.Ansi, SetLastError = false)]
        //public static extern bool ConnectService(string ServiceUrl);

        //[DllImport(("GateWay.dll"), EntryPoint = "DisconnectService", CharSet = CharSet.Ansi, SetLastError = false)]
        //public static extern bool DisconnectService();

        //[DllImport(("GateWay.dll"), EntryPoint = "UploadXmlContent", CharSet = CharSet.Ansi, SetLastError = false)]
        //public static extern bool UploadXmlContent(string XmlContent);

        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        public void pathtohis(string blh,string debug)
        {
         

            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
            int ljfs = f.ReadInteger("savetohis", "ljfs", 1);
             debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();

            string  msg = f.ReadString("savetohis", "msg", "").Replace("\0", "").Trim();
            string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();

         
           

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
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                ZgqClass.BGHJ(blh, CZY, "保存", "无申请序号（单据号），不处理！", "ZGQJK", "");
                 return;
            }




            string constr = f.ReadString("savetohis", "odbcsql", "Data Source=192.168.4.5;Initial Catalog=ESDB;User Id=sa;Password=abc123!;");

            SqlDB db = new SqlDB();
            string errmsg = "";

            try
            {
                if (bljc.Rows[0]["F_HISBJ"].ToString() == "" || bljc.Rows[0]["F_HISBJ"].ToString() == null)
                {
                    string sql1 = "update pacs_resulto_temp set flag=1  where detail_id='" + bljc.Rows[0]["F_SQXH"].ToString() + "'";

                    int x = db.ExecuteNonQuery(constr, sql1, ref errmsg);
                    if (x > 0)
                    {
                        aa.ExecuteSQL("update T_JCXX set F_HISBJ='1'  where F_blh='" + blh + "'");
                        if (debug == "1")
                            MessageBox.Show("ToHIS：回写状态成功");
                    }
                    else
                    {
                        log.WriteMyLog("ToHIS：回写状态失败：" + errmsg + "\r\n" + sql1);
                        if (debug == "1")
                            MessageBox.Show("ToHIS：回写状态失败");
                    }
                }
            }
            catch
            {
                log.WriteMyLog("ToHIS：回写状态异常");
            }
            if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
            {
         
                string sql = "update pacs_resulto_temp set flag=2,report_no='" + blh + "',check_ok='" + bljc.Rows[0]["F_RYSJ"].ToString() + "',check_result='" + bljc.Rows[0]["F_BLZD"].ToString() + "',execute_date='" + bljc.Rows[0]["F_BGRQ"].ToString() + "',doctor='" + bljc.Rows[0]["F_SHYS"].ToString() + "',check_date='" + bljc.Rows[0]["F_SPARE5"].ToString() + "',check_doctor='" + bljc.Rows[0]["F_SHYS"].ToString() + "'";
               sql=sql + "  where detail_id='" + bljc.Rows[0]["F_SQXH"].ToString() + "'";

               if (debug == "1")
                log.WriteMyLog(sql);

                int x = db.ExecuteNonQuery(constr, sql, ref errmsg);
                log.WriteMyLog("返回：" + x.ToString() + "---" + errmsg);
                if (x > 0)
                {
                    aa.ExecuteSQL("update T_JCXX set F_HISBJ='2' where F_blh='" + blh + "'");
                    if (debug == "1")
                        MessageBox.Show("ToHIS：回写报告成功");
                }
                else
                {
                    log.WriteMyLog("ToHIS：回写报告失败：" + errmsg + "\r\n" + sql);
                    if (debug == "1")
                        MessageBox.Show("ToHIS：回写报告失败");
                }
                //   return; 

            }
            else
                if (bljc.Rows[0]["F_BGZT"].ToString() == "已写报告" && bljc.Rows[0]["F_HISBJ"].ToString() == "2")
                {
                    string sql1 = "update pacs_resulto_temp set flag=-1,report_no='" + blh + "',check_ok='',check_result='',execute_date='',doctor='',check_date='',check_doctor='' "
                    + "  where detail_id='" + bljc.Rows[0]["F_SQXH"].ToString() + "'";

                    int x = db.ExecuteNonQuery(constr, sql1, ref errmsg);
                    if (x > 0)
                    {
                        aa.ExecuteSQL("update T_JCXX set F_HISBJ='-1' where F_blh='" + blh + "'");
                        if (debug == "1")
                            MessageBox.Show("ToHIS：回收报告成功");
                    }
                    else
                    {
                        log.WriteMyLog("ToHIS：回收报告失败：" + errmsg + "\r\n" + sql1);
                        if (debug == "1")
                            MessageBox.Show("ToHIS：回收报告失败");
                    }

                }
                else
                    log.WriteMyLog("ToHIS：" + bljc.Rows[0]["F_BGZT"].ToString() + ",不处理");
            //tohis
        //    tohis(blh, bljc,debug);
            //topacs
           // topacs(blh, bljc,debug,bglx,bgxh,dz);
  
        }

        //public void tohis(string blh,DataTable  bljc,string debug)
        //{
          

        //    string constr = f.ReadString("savetohis", "odbcsql", "Data Source=192.168.4.5;Initial Catalog=ESDB;User Id=sa;Password=abc123!;");
  
        //   SqlDB db = new SqlDB(); 
        //    string errmsg = "";

        //    try
        //    {
        //        if (bljc.Rows[0]["F_HISBJ"].ToString() == "" || bljc.Rows[0]["F_HISBJ"].ToString() == null)
        //        {
        //            string sql1 = "update pacs_resulto_temp set flag=1  where detail_id='" + bljc.Rows[0]["F_SQXH"].ToString() + "'";

        //            int x = db.ExecuteNonQuery(constr, sql1, ref errmsg);
        //            if (x > 0)
        //            {
        //                aa.ExecuteSQL("update T_JCXX set F_HISBJ='1'  where F_blh='" + blh + "'");
        //                if (debug == "1")
        //                    MessageBox.Show("ToHIS：回写状态成功");
        //            }
        //            else
        //            {
        //                 LGZGQClass.log.WriteMyLog("ToHIS：回写状态失败：" + errmsg + "\r\n" + sql1);
        //                if (debug == "1")
        //                    MessageBox.Show("ToHIS：回写状态失败");
        //            }
        //        }
        //    }
        //    catch
        //    {
        //         LGZGQClass.log.WriteMyLog("ToHIS：回写状态异常");
        //    }
        //    if (bljc.Rows[0]["F_BGZT"].ToString() == "已审核")
        //    {
        //        string sql1 = "update pacs_resulto_temp set flag=2,report_no='" + blh + "',check_ok='" + bljc.Rows[0]["F_RYSJ"].ToString() + "',check_result='" + bljc.Rows[0]["F_BLZD"].ToString() + "',execute_date='" + bljc.Rows[0]["F_BGRQ"].ToString() + "'"
        //       + "doctor='" + bljc.Rows[0]["F_BHYS"].ToString() + "',check_date='" + bljc.Rows[0]["F_SPARE5"].ToString() + "',check_doctor='" + bljc.Rows[0]["F_SHYS"].ToString() + "'"
        //        +"  where detail_id='" + bljc.Rows[0]["F_SQXH"].ToString() + "'";


        //         LGZGQClass.log.WriteMyLog(sql1);
        //           int x = db.ExecuteNonQuery(constr, sql1, ref errmsg);
        //            LGZGQClass.log.WriteMyLog("返回：" + x.ToString()+"---"+errmsg);
        //           if (x>0)
        //           {
        //               aa.ExecuteSQL("update T_JCXX set F_HISBJ='2' where F_blh='" + blh + "'");
        //               if (debug == "1")
        //                   MessageBox.Show("ToHIS：回写报告成功");
        //           }
        //           else
        //           {
        //                LGZGQClass.log.WriteMyLog("ToHIS：回写报告失败：" + errmsg + "\r\n" + sql1);
        //               if (debug == "1")
        //                   MessageBox.Show("ToHIS：回写报告失败");
        //           }
        //             //   return; 

        //    }
        //    else
        //    if (bljc.Rows[0]["F_BGZT"].ToString() == "已写报告" && bljc.Rows[0]["F_HISBJ"].ToString()=="2")
        //    {
        //        string sql1 = "update pacs_resulto_temp set flag=-1,report_no='" + blh + "',check_ok='',check_result='',execute_date='',doctor='',check_date='',check_doctor='' "
        //        +"  where detail_id='" + bljc.Rows[0]["F_SQXH"].ToString() + "'";

        //          int x = db.ExecuteNonQuery(constr, sql1, ref errmsg);
        //          if (x>0)
        //          {
        //              aa.ExecuteSQL("update T_JCXX set F_HISBJ='-1' where F_blh='" + blh + "'");
        //              if (debug == "1")
        //                  MessageBox.Show("ToHIS：回收报告成功");
        //          }
        //          else
        //          {
        //               LGZGQClass.log.WriteMyLog("ToHIS：回收报告失败：" + errmsg + "\r\n" + sql1);
        //              if (debug == "1")
        //                  MessageBox.Show("ToHIS：回收报告失败");
        //          }
      
        //    }
        //    else
        //         LGZGQClass.log.WriteMyLog("ToHIS：" + bljc.Rows[0]["F_BGZT"].ToString() + ",不处理");

        //}

        //public void topacs(string blh, DataTable bljc,string  debug,string  bglx,string bgxh,string  dz)
        //{


        //    if (dz != "save")
        //        return;

        //    string ServiceUrl = f.ReadString("savetohis", "ServiceUrl", "192.9.200.1");
        //    string bgzt = "";
        //    string SHRQ = "";
        //    string bgys = "";
        //    string shys = "";
        //    string fzys = "";
        //    string bgrq = "";
        //    string blzd = "";
        //    string rysj = "";
        //    if (bglx == "cg")
        //    {
        //        bgzt = bljc.Rows[0]["F_BGZT"].ToString().Trim();
        //        SHRQ = bljc.Rows[0]["F_SPARE5"].ToString().Trim();
        //        bgys = bljc.Rows[0]["F_BGYS"].ToString().Trim();
        //        shys = bljc.Rows[0]["F_SHYS"].ToString().Trim();
        //        fzys = bljc.Rows[0]["F_FZYS"].ToString().Trim();
        //        bgrq = bljc.Rows[0]["F_BGRQ"].ToString().Trim();
        //        rysj = System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "\r\n" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JXSJ"].ToString().Trim());
        //        blzd = System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLZD"].ToString().Trim()) + "\r\n" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_TSJC"].ToString().Trim());
        //    }
        //    if (bglx == "bd")
        //    {
        //        DataTable bdbg = new DataTable();
        //        bdbg = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "' and F_bd_BGXH='" + bgxh + "'", "bd");
        //        bgzt = bdbg.Rows[0]["F_BD_BGZT"].ToString().Trim();
        //        SHRQ = bdbg.Rows[0]["F_BD_BGRQ"].ToString().Trim();
        //        bgys = bdbg.Rows[0]["F_bd_BGYS"].ToString().Trim();
        //        shys = bdbg.Rows[0]["F_bd_SHYS"].ToString().Trim();
        //        fzys = bdbg.Rows[0]["F_bd_FZYS"].ToString().Trim();
        //        bgrq = bdbg.Rows[0]["F_bd_BGRQ"].ToString().Trim();
        //        rysj = "";
        //        blzd = System.Security.SecurityElement.Escape(bdbg.Rows[0]["F_BdZD"].ToString().Trim());

        //    }
        //    if (bglx == "bc")
        //    {
        //        DataTable bcbg = new DataTable();
        //        bcbg = aa.GetDataTable("select * from T_BcBG where F_blh='" + blh + "' and F_bc_BGXH='" + bgxh + "'", "bc");
        //        bgzt = bcbg.Rows[0]["F_BC_BGZT"].ToString().Trim();
        //        SHRQ = bcbg.Rows[0]["F_BC_SPARE5"].ToString().Trim();
        //        bgys = bcbg.Rows[0]["F_bc_BGYS"].ToString().Trim();
        //        shys = bcbg.Rows[0]["F_bc_SHYS"].ToString().Trim();
        //        fzys = bcbg.Rows[0]["F_bc_FZYS"].ToString().Trim();
        //        bgrq = bcbg.Rows[0]["F_bc_BGRQ"].ToString().Trim();
        //        rysj = System.Security.SecurityElement.Escape(bcbg.Rows[0]["F_bc_JXSJ"].ToString().Trim());
        //        blzd = System.Security.SecurityElement.Escape(bcbg.Rows[0]["F_BcZD"].ToString().Trim()) + "\r\n" + System.Security.SecurityElement.Escape(bcbg.Rows[0]["F_BC_TSJC"].ToString().Trim());

        //    }
        //    //病理号^cg/bd/bc^bgxh^new/old^save/qxsh (同济)
        //    if (bgzt == "已审核")
        //    {
        //        //---------------------------------------
        //        //***************************************
        //        //---回写蓝韵pacs
        //        //***************************************
        //        //---------------------------------------

        //        if (debug == "1")
        //             LGZGQClass.log.WriteMyLog("开始回写pacs");

        //        string xmlstr = "";
        //        string ReprotFile = "";
        //        //-----------------------------------------------------------
        //        string jpgname = "";
        //        //mdjpg jpgxx = new mdjpg();
        //        //try
        //        //{
        //        //    if (bgxh == "")
        //        //        bgxh = "0";
        //        //    jpgxx.BMPTOJPG(blh, ref jpgname, bglx, bgxh);
        //        //    //-读图片
        //        //    string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
        //        //    string imgPath = ftplocal + "\\" + blh + "\\" + blh + "_" + bglx + "_" + bgxh + "_1.jpg";//图片文件所在路径  

        //        //    FileStream file = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
        //        //    Byte[] imgByte = new Byte[file.Length];//把图片转成 Byte型 二进制流   
        //        //    file.Read(imgByte, 0, imgByte.Length);//把二进制流读入缓冲区   

        //        //    file.Close();
        //        //    ReprotFile = Convert.ToBase64String(imgByte);

        //        //    try
        //        //    {
        //        //        if (System.IO.Directory.Exists(@"c:\temp\" + blh))
        //        //            System.IO.Directory.Delete(@"c:\temp\" + blh, true);
        //        //    }
        //        //    catch
        //        //    {
        //        //         LGZGQClass.log.WriteMyLog("回写pacs,删除报告JPG失败");
        //        //    }
        //        //}
        //        //catch (Exception ee)
        //        //{
        //        //     LGZGQClass.log.WriteMyLog("回写pacs,生成报告JPG失败" + ee.Message);
        //        //    ReprotFile = "";
        //        //}
        //        //-----------------------------------------------------------------

        //        try
        //        {
        //            xmlstr = "<?xml version='1.0' encoding='GB2312' ?><StudyInfo StationType='1'>";
        //            xmlstr = xmlstr + "<PatientCode4></PatientCode4>";
        //            //病人ID
        //            xmlstr = xmlstr + "<PatientID>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</PatientID>";
        //            xmlstr = xmlstr + "<PatientName>" + bljc.Rows[0]["F_XM"].ToString().Trim() + "</PatientName> ";
        //            xmlstr = xmlstr + "<PatientNameEn></PatientNameEn>";
        //            xmlstr = xmlstr + "<PatientSex>" + bljc.Rows[0]["F_xb"].ToString().Trim() + "</PatientSex>";
        //            xmlstr = xmlstr + "<DateOfBirth></DateOfBirth> ";
        //            xmlstr = xmlstr + "<Citizenship /><IDCardNumber /><Occupation />";
        //            xmlstr = xmlstr + "<Address>" + bljc.Rows[0]["F_lxxx"].ToString().Trim() + "</Address>";
        //            xmlstr = xmlstr + "<TelPhone /><MedicalAlert /><ContrastAllergies /> ";
        //            xmlstr = xmlstr + "<RegisteDate>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</RegisteDate>";
        //            //检查ID
        //            xmlstr = xmlstr + "<StudyID>" + bljc.Rows[0]["F_BLH"].ToString().Trim() + "</StudyID>";
        //            xmlstr = xmlstr + "<BedNumber>" + bljc.Rows[0]["F_CH"].ToString().Trim() + "</BedNumber>";
        //            xmlstr = xmlstr + "<InPatientNumber>" + bljc.Rows[0]["F_zyh"].ToString().Trim() + "</InPatientNumber>";
        //            xmlstr = xmlstr + "<ClinicSymptom>" + bljc.Rows[0]["F_lczl"].ToString().Trim() + "</ClinicSymptom>";
        //            xmlstr = xmlstr + "<ClinicDiagnose>" + bljc.Rows[0]["F_lczd"].ToString().Trim() + "</ClinicDiagnose>";
        //            xmlstr = xmlstr + "<RequestMemo />";
        //            xmlstr = xmlstr + "<AppliedDoctor>" + bljc.Rows[0]["F_sjys"].ToString().Trim() + "</AppliedDoctor>";
        //            xmlstr = xmlstr + "<AppliedDate></AppliedDate>";
        //            xmlstr = xmlstr + "<ArrivedDate>" + bljc.Rows[0]["F_SDRQ"].ToString().Trim() + "</ArrivedDate>";
        //            xmlstr = xmlstr + "<BodyPart>" + bljc.Rows[0]["F_bbmc"].ToString().Trim() + "</BodyPart>";
        //            xmlstr = xmlstr + "<Modality></Modality>";

        //            xmlstr = xmlstr + "<ScanType /><FinishTime>" + bgrq + "</FinishTime>";
        //            xmlstr = xmlstr + "<AppliedDepartment>" + bljc.Rows[0]["F_sjks"].ToString().Trim() + "</AppliedDepartment>";

        //            xmlstr = xmlstr + "<Technician>" + bljc.Rows[0]["F_bgys"].ToString().Trim() + "</Technician>";

        //            xmlstr = xmlstr + "<StudyDiagnose>" + rysj + "</StudyDiagnose>";
        //            xmlstr = xmlstr + "<StudyTechnic /><StudyStatus>已审核</StudyStatus>";

        //            xmlstr = xmlstr + "<StudyResult>" + blzd + "</StudyResult>";

        //            xmlstr = xmlstr + "<FirstReportTime>" + bgrq + "</FirstReportTime>";

        //            xmlstr = xmlstr + "<FirstDoctor>" + bgys + "</FirstDoctor>";

        //            xmlstr = xmlstr + "<SecondReportTime>" + SHRQ + "</SecondReportTime>";

        //            xmlstr = xmlstr + "<SecondDoctor>" + shys + "</SecondDoctor> ";
        //            xmlstr = xmlstr + "<Register>" + bljc.Rows[0]["F_jsy"].ToString().Trim() + "</Register>";
        //            xmlstr = xmlstr + "<Fee></Fee><Age>" + bljc.Rows[0]["F_NL"].ToString().Trim() + "</Age>";
        //            xmlstr = xmlstr + "<Positive></Positive><ExamDepartment>病理科</ExamDepartment>   ";

        //            xmlstr = xmlstr + "<ThirdDoctor>" + fzys + "</ThirdDoctor>";

        //            xmlstr = xmlstr + "<ThirdReportTime /><Result1 /> <Result2 /> <Result3 /> <Result4 /> <Result5 /> <Result6 /> <Result7 /> <Result8 /> ";
        //            xmlstr = xmlstr + "<TrackTime /> <TrackResult />";
        //            xmlstr = xmlstr + "<PositiveType>" + bljc.Rows[0]["F_YYX"].ToString().Trim() + "</PositiveType> <TrackDoctor />";

        //            xmlstr = xmlstr + "<PatientType>" + bljc.Rows[0]["F_brlb"].ToString().Trim() + "</PatientType>";
        //            xmlstr = xmlstr + "<StudyType>" + bljc.Rows[0]["F_BLK"].ToString().Trim() + "</StudyType>";
        //            xmlstr = xmlstr + "<ModalityType>BL</ModalityType>";
        //            xmlstr = xmlstr + "<IsTracked /> <Weight /> <Height /><ReportType /> <AcrIndex />";
        //            //常规，冰冻，补充
        //            xmlstr = xmlstr + "<StudyCode>" + "BL_" + blh + "_" + bglx + "_" + bgxh + "</StudyCode>";

        //            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
        //                xmlstr = xmlstr + "<HisCode1>" + bljc.Rows[0]["F_ZYH"].ToString().Trim() + "</HisCode1>";
        //            else
        //                xmlstr = xmlstr + "<HisCode1>" + bljc.Rows[0]["F_MZH"].ToString().Trim() + "</HisCode1>";
        //            xmlstr = xmlstr + "<HisCode2>" + bljc.Rows[0]["F_sqxh"].ToString().Trim() + "</HisCode2>";
        //            xmlstr = xmlstr + "<HisCode3>" + bljc.Rows[0]["F_brbh"].ToString().Trim() + "</HisCode3> ";
        //            xmlstr = xmlstr + "<HisCode4>" + bljc.Rows[0]["F_YZID"].ToString().Trim() + "</HisCode4>";
        //            xmlstr = xmlstr + "<StudyUIDS><StudyUID></StudyUID></StudyUIDS>";
        //            xmlstr = xmlstr + "<ReportFile>" + ReprotFile + "</ReportFile><ReportFileType/>";
        //            xmlstr = xmlstr + "<TrueOfClinic>" + bljc.Rows[0]["F_SFFH"].ToString().Trim() + "</TrueOfClinic></StudyInfo>";
        //        }
        //        catch (Exception ee)
        //        {
        //             LGZGQClass.log.WriteMyLog("ToPACS：xml生成异常:" + ee.Message);
        //            return;
        //        }

        //        string rtn = "";
        //        try
        //        {
        //            if (debug == "1")
        //                 LGZGQClass.log.WriteMyLog("ToPACS： xml：" + xmlstr);

        //            if (ConnectService(ServiceUrl))
        //            {
        //                if (UploadXmlContent(xmlstr))
        //                {
        //                    if (debug == "1")
        //                        MessageBox.Show("true");
        //                    aa.ExecuteSQL("update T_JCXX set F_PACSBJ='2' where F_blh='" + blh + "'");
        //                }
        //                else
        //                {
        //                     LGZGQClass.log.WriteMyLog("ToPACS：回写失败,false");
        //                    if (debug == "1")
        //                        MessageBox.Show("ToPACS：回写失败,false");
        //                }
        //            }
        //            else
        //            {
        //                 LGZGQClass.log.WriteMyLog("ToPACS：连接数据库错误:" + ServiceUrl);
        //            }
        //            DisconnectService();


        //        }
        //        catch (Exception e2)
        //        {
        //             LGZGQClass.log.WriteMyLog("ToPACS：异常:" + e2.Message);
        //            return;
        //        }
        //    } return;

        //}
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
