
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data.SqlClient;
using dbbase;

using System.IO;
using System.Collections;
using System.Xml;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class fzs2y
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public void fj2yhis(string blh, string yymc)
        {

            string bglx = "cg";
            string bgxh = "0";

            string CZY = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string CZYGH = f.ReadString("yh", "yhbh", "").Replace("\0", "").Trim();
              int ljfs = f.ReadInteger("savetohis", "ljfs", 1);
              string debug = f.ReadString("savetohis", "debug", "").Replace("\0", "").Trim();
            
              string msg = f.ReadString("savetohis", "msg", "").Replace("\0", "").Trim();
              string wsurl = f.ReadString("savetohis", "wsurl", "").Replace("\0", "").Trim();

            string brlb = "";
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
           
            if (bljc.Rows[0]["F_bgzt"].ToString().Trim() != "已审核")
            {
                return;
            }
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() =="")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                ZgqClass.BGHJ(blh, CZY, "保存", "无申请序号（单据号），不处理！", "ZGQJK", "");

                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='无申请序号（单据号），不处理',F_FSZT='不处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                return;
            }
            if (bljc.Rows[0]["F_brbh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无病人编号，不处理！");
                ZgqClass.BGHJ(blh, CZY, "保存", "无病人编号，不处理！", "ZGQJK", "");
                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='无病人编号，不处理',F_FSZT='不处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
            
                return;
            }
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院") brlb = "IN";
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊") brlb = "OU";
            if (brlb == "")
            {
                log.WriteMyLog("非住院或门诊病人，不处理！");
                ZgqClass.BGHJ(blh, CZY, "保存", "非住院或门诊病人，不处理！", "ZGQJK", "");
                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='非住院或门诊病人，不处理',F_FSZT='不处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
            
                return;

            }
            
            
            
            
            

            //string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            //string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            //string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            //string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            //string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            //string ftps = f.ReadString("ftp", "ftp", "").Replace("\0", "");
            //string txpath = f.ReadString("txpath", "txpath", "").Replace("\0", "");
            //FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);

            //string ftpserver2 = f.ReadString("ftpup", "ftpip", "").Replace("\0", "");
            //string ftpuser2 = f.ReadString("ftpup", "user", "ftpuser").Replace("\0", "");
            //string ftppwd2 = f.ReadString("ftpup", "pwd", "ftp").Replace("\0", "");
            //string ftplocal2 = f.ReadString("ftpup", "ftplocal", "c:\\temp").Replace("\0", "");
            //string ftpremotepath2 = f.ReadString("ftpup", "ftpremotepath", "").Replace("\0", "");
            //string ftps2 = f.ReadString("ftp", "ftp", "").Replace("\0", "");
            //FtpWeb fwup = new FtpWeb(ftpserver2, ftpremotepath2, ftpuser2, ftppwd2);
            
            string txml = bljc.Rows[0]["F_txml"].ToString().Trim();
            string bcbg = "\r\n";
            DataTable bcbgtb = aa.GetDataTable("select * from T_bcbg where F_blh='" + blh + "' and F_bc_bgzt='已审核'", "bcbg");
            for (int i = 0; i < bcbgtb.Rows.Count; i++)
            {
                bcbg = bcbg + "[补充报告" + bcbgtb.Rows[i]["F_bc_bgxh"].ToString().Trim() + "]" + bcbgtb.Rows[i]["F_bczd"].ToString().Trim()+"\r\n";

            }
            DataTable txlb = aa.GetDataTable("select top 4 * from V_dytx where F_blh='" + blh + "'", "txlb");
            DataTable TBS = aa.GetDataTable("select * from T_tbs_bg where F_blh='" + blh + "'", "tbs");

            string stxsm = "";


            //Im

            string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalone=" + (char)34 + "yes" + (char)34 + "?>";
            inxml = inxml + "<ExamList>";
            inxml = inxml + "<XmlDocument>";
            inxml = inxml + "<Header>";
            inxml = inxml + "<Application>" + "BL" + "</Application>";
            inxml = inxml + "<CreateTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</CreateTime>";
            inxml = inxml + "<VersionID>1,0,0,3</VersionID>";
            inxml = inxml + "</Header>";

            inxml = inxml + "<Event>";
            inxml = inxml + "<Code>C</Code>";
            inxml = inxml + "<Desc>病理报告</Desc>";
            inxml = inxml + "<Operator>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Operator>";
            inxml = inxml + "<UniqueID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_BLH"].ToString().Trim()) + "</UniqueID>";
            inxml = inxml + "</Event>";

            inxml = inxml + "<Patient>";
            inxml = inxml + "<CardNo></CardNo>";
            inxml = inxml + "<SiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</SiCard>";
            inxml = inxml + "<MiCard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzid"].ToString().Trim()) + "</MiCard>";
            inxml = inxml + "<SickNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brbh"].ToString().Trim()) + "</SickNo>";
            inxml = inxml + "<PatientClass  code=" + brlb + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_brlb"].ToString().Trim()) + "</PatientClass>";
            inxml = inxml + "<InPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_zyh"].ToString().Trim()) + "</InPatientNo>";
            inxml = inxml + "<VisitID></VisitID>";
            inxml = inxml + "<OutPatientNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mzh"].ToString().Trim()) + "</OutPatientNo>";
            inxml = inxml + "<PatientName>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xm"].ToString().Trim()) + "</PatientName>";
            inxml = inxml + "<Phoneticize></Phoneticize>";
            inxml = inxml + "<BirthDate></BirthDate>";
            inxml = inxml + "<Age>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_nl"].ToString().Trim()) + "</Age>";            
            inxml = inxml + "<Height></Height>";
            inxml = inxml + "<Weight></Weight>";
            inxml = inxml + "<BedNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_ch"].ToString().Trim()) + "</BedNo>";
            inxml = inxml + "<NativePlace></NativePlace>";
            inxml = inxml + "<Nationality></Nationality>";
            inxml = inxml + "<Sex>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_xb"].ToString().Trim()) + "</Sex>";
            inxml = inxml + "<Nation>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_mz"].ToString().Trim()) + "</Nation>";
            inxml = inxml + "<Address>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lxxx"].ToString().Trim()) + "</Address>";
            inxml = inxml + "<ZipCode></ZipCode>";
            inxml = inxml + "<PhoneNumber></PhoneNumber>";
            inxml = inxml + "<MaritalStatus>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_hy"].ToString().Trim()) + "</MaritalStatus>";
            inxml = inxml + "<Identity><Type>ID</Type><Name>居民身份证</Name><Number>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sfzh"].ToString().Trim()) + "</Number></Identity>";
            inxml = inxml + "<Memo></Memo>";
            inxml = inxml + "<Degree></Degree>";
            inxml = inxml + "<ChargeType></ChargeType>";

            inxml = inxml + "</Patient>";

            inxml = inxml + "<Exam>";
         
            inxml = inxml + "<HospitalName></HospitalName>"; 

            inxml = inxml + " <MasterID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blh"].ToString().Trim())+"</MasterID>"; 

            inxml = inxml + "<ApplyNo>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_SQXH"].ToString().Trim()) + "</ApplyNo>";
            inxml = inxml + "<PatientID>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blh"].ToString().Trim()) + "</PatientID>";
            inxml = inxml + "<OutRegNo></OutRegNo>";
            inxml = inxml + "<ExamStatus code=\"70\">确认报告</ExamStatus>";
            inxml = inxml + "<ExamTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</ExamTime>";
            inxml = inxml + "<ExamClass>病理</ExamClass>";
            inxml = inxml + "<ExamSubClass>"+System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blk"].ToString().Trim())+"</ExamSubClass>";
            inxml = inxml + "<ExamItem>" ;
            inxml = inxml + "<Item code=" + (char)34 + (char)34 + " Cost=" + (char)34 + (char)34 + " Charge=" + (char)34 + (char)34 + " OrderNo=" + (char)34 + (char)34 + ">" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_yzxm"].ToString().Trim()) + "</Item>";
            inxml = inxml + "</ExamItem>";
            inxml = inxml + "<ExamOrgan><Organ>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bbmc"].ToString().Trim()) + "</Organ></ExamOrgan>";
            inxml = inxml + "<Device></Device>";
            inxml = inxml + "<ExamGroup></ExamGroup>";
            inxml = inxml + "<PerformDept>病理科</PerformDept>";
            inxml = inxml + "<Technican>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Technican>";
            inxml = inxml + "<ExamDuration></ExamDuration>";
            inxml = inxml + "<ReqHospital></ReqHospital>";
            inxml = inxml + "<Regisiter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_JSY"].ToString().Trim()) + "</Regisiter>";
            inxml = inxml + "<ReqDept>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjks"].ToString().Trim()) + "</ReqDept>";
            inxml = inxml + "<ReqWard>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bq"].ToString().Trim()) + "</ReqWard>";
            inxml = inxml + "<ReqPhysician>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sjys"].ToString().Trim()) + "</ReqPhysician>";
            inxml = inxml + "<ScheduledTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ScheduledTime>";
            inxml = inxml + "<ReqTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_sdrq"].ToString().Trim()) + " 00:00:00</ReqTime>";
            inxml = inxml + "<PhysSign></PhysSign>";
            inxml = inxml + "<ClinSymp></ClinSymp>";
            inxml = inxml + "<ClinDiag>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_lczd"].ToString().Trim()) + "</ClinDiag>";
             inxml = inxml + "<Revisit></Revisit>";
             inxml = inxml + "<Charge></Charge>";
             inxml = inxml + "<Cost></Cost>";
             inxml = inxml + "<ChargeFlag></ChargeFlag>";
            inxml = inxml + "<Memo></Memo>";
            inxml = inxml + "<ExamRecord></ExamRecord>";
            inxml = inxml + "<Revisit></Revisit>";
             inxml = inxml + "<StudyUID></StudyUID>";
            inxml = inxml + "<AccessionNo></AccessionNo>";
            inxml = inxml + "</Exam>";

            inxml = inxml + "<Report>";
            if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
            {
                if (TBS.Rows.Count < 0)
                {
                    log.WriteMyLog(bljc.Rows[0]["F_blh"].ToString() + "此TCT报告无结构化报告内容！");

                }
                else
                {
                    inxml = inxml + "<GMReport>";
                    inxml = inxml + "<SampleQuality>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BBMYD"].ToString().Trim()) + "</SampleQuality>";
                    inxml = inxml + "<CellCount>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBL"].ToString().Trim()) + "</CellCount>";
                    inxml = inxml + "<CellItemOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBXM1"].ToString().Trim()) + "</CellItemOne>";
                    inxml = inxml + "<CellItemTwo>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_XBXM2"].ToString().Trim()) + "</CellItemTwo>";
                    inxml = inxml + "<AnimalculeOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW1"].ToString().Trim()) + "</AnimalculeOne>";
                    inxml = inxml + "<AnimalculeTwo>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW2"].ToString().Trim()) + "</AnimalculeTwo>";
                    inxml = inxml + "<AnimalculeThree>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_WSW3"].ToString().Trim()) + "</AnimalculeThree>";
                    inxml = inxml + "<VirusItemOne>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BDXM1"].ToString().Trim()) + "</VirusItemOne>";
                    inxml = inxml + "<Inflammation>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_YZCD"].ToString().Trim()) + "</Inflammation>";
                    inxml = inxml + "<Erythrocyte>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_HXB"].ToString().Trim()) + "</Erythrocyte>";
                    
                    inxml = inxml + "</GMReport>";
                }
            }
            inxml = inxml + "<ReportNo>1</ReportNo>";
            inxml = inxml + "<Category></Category>";
            inxml = inxml + "<Description>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_rysj"].ToString().Trim()) + "</Description>";
            if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
            {
                if (TBS.Rows.Count > 0)
                {
                    inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBSZD"].ToString().Trim()) + "</Impression>";
                }
            }
            else
            {
                inxml = inxml + "<Impression>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_blzd"].ToString().Trim()) + bcbg + "</Impression>";
            }
            inxml = inxml + "<ExamParam></ExamParam>";
            if (bljc.Rows[0]["F_blk"].ToString().Trim().ToUpper() == "TCT")
            {
                if (TBS.Rows.Count > 0)
                {
                    inxml = inxml + "<Recommendation>" + System.Security.SecurityElement.Escape(TBS.Rows[0]["F_TBS_BCYJ1"].ToString().Trim()+"\r\n"+TBS.Rows[0]["F_TBS_BCYJ2"].ToString().Trim()) + "</Recommendation>";
                }
            }
            else
            {
                inxml = inxml + "<Recommendation></Recommendation>";
            }
            inxml = inxml + "<AbNormalFlag></AbNormalFlag>";
            inxml = inxml + "<Reporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgys"].ToString().Trim()) + "</Reporter>";
            inxml = inxml + "<ReportTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_bgrq"].ToString().Trim()) + " 00:00:00</ReportTime>";
            inxml = inxml + "<MasterReporter>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_shys"].ToString().Trim()) + "</MasterReporter>";
            inxml = inxml + "<MasterTime>" + System.Security.SecurityElement.Escape(bljc.Rows[0]["F_spare5"].ToString().Trim()).Substring(0,10) + "</MasterTime>";
            inxml = inxml + "<PrintTime></PrintTime>";
          //  <ReportContent Type="PDF">BASE64</ReportContent>
            inxml = inxml + "<ReportImageArray ImageType=\"JPG\">";
            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                inxml = inxml + "<ReportImage>";
                inxml = inxml + "<Path>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_txm"].ToString().Trim()) + "</Path>";
                inxml = inxml + "<Pos></Pos>";
                inxml = inxml + "<Desc></Desc>";
                inxml = inxml + "<SopInstanceUID>" + System.Security.SecurityElement.Escape(txlb.Rows[i]["F_ID"].ToString().Trim()) + "</SopInstanceUID>";
                inxml = inxml + "<ImageContent></ImageContent>";
                inxml = inxml + "</ReportImage>";
            }

            inxml = inxml + "</ReportImageArray>";
            inxml = inxml + "</Report>";
            inxml = inxml + "<Study>";

            inxml = inxml + "<ImageType>JPG</ImageType>";
            inxml = inxml + "<ImageCount>" + txlb.Rows.Count + "</ImageCount>";
            inxml = inxml + "<Modality>BL</Modality>";
            inxml = inxml + "<StudyUID></StudyUID>";

            inxml = inxml + "<SerialArray>";
            inxml = inxml + "<SerialInfo description=\"\" uid=\"\" number=\"\">";
            inxml = inxml + "<ImageArray>";
            inxml = inxml + "<ImagePath uid=\"\" number=\"\" KeyFlag=\"\">"+System.Security.SecurityElement.Escape("pathimages\\"+txml)+"</ImagePath>";
            inxml = inxml + "</ImageArray></SerialInfo>";
            inxml = inxml + "</SerialArray>";
            inxml = inxml + "</Study>";
            
            inxml = inxml + "</XmlDocument>";
             inxml = inxml + "</ExamList>";

            string  putcmsg="<ESBEntry><AccessControl><UserName/><Password/><Fid>BS25018</Fid></AccessControl><MessageHeader><Fid>BS25018</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S04</TargetSysCode><MsgDate>"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                +"</MsgDate></MessageHeader><MsgInfo><Msg><![CDATA["+inxml+"]]></Msg></MsgInfo></ESBEntry>";

            //给平台发送数据
            string getcmsg="";string errmsg="";
            if (sendxml("BS25018", putcmsg, debug, ljfs, wsurl, ref getcmsg, ref errmsg))
            {
                ZgqClass.BGHJ(blh, CZY, "审核", "上传平台成功", "ZGQJK", "");
               if (msg == "1")
                MessageBox.Show(getcmsg);

            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
              
            }
            else
            {
                ZgqClass.BGHJ(blh, CZY, "审核", "上传平台失败," + errmsg, "ZGQJK", "");
                log.WriteMyLog(errmsg);
                if (msg=="1")
                 MessageBox.Show(errmsg);

                if(getcmsg=="")
                  aa.ExecuteSQL("update T_JCXX_FS set F_BZ='平台返回空',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
                else
                  aa.ExecuteSQL("update T_JCXX_FS set F_BZ='" + getcmsg + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");
            
            }

          //string ftpstatusUP = "";
       
          //fwup.Makedir("BL" + blh, out ftpstatusUP);
       
          //fwup.Makedir("BL" + blh + "/" + "ReportImage", out ftpstatusUP);
        
          //for (int i = 0; i < txlb.Rows.Count; i++)
          //{
          //    fwup.Upload(ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim(), "BL" + blh + "/ReportImage", out ftpstatusUP);
          //    if (ftpstatusUP == "Error")
          //    {
          //        ZgqClass.BGHJ(blh, CZY, "审核", "上传图像Error", "ZGQJK", "");
          //        return;
          //    }
          //}
                    
        }

        private bool sendxml(string fid, string putcmsg, string debug, int ljfs, string wsurl, ref string getcmsg,ref string errmsg)
        {


            if (debug == "1")
                log.WriteMyLog("平台入参：" + putcmsg);

            getcmsg = "";
            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                //try
                //{
                //    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                //}
                //catch (Exception e1)
                //{
                //    errmsg="异常：" + e1.Message;
                //    return false;
                //}
            }
            else
            {
                EHSBMQWeb.Service ehsb = new PathHISZGQJK.EHSBMQWeb.Service();
                if (wsurl.Trim() != "")
                    ehsb.Url = wsurl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    errmsg = "异常：" + e1.Message;
                   return false;
                
                }
            }

            if (!rtn)
            {
                errmsg = "异常：" + err_msg;
                return false;
            }
            if (debug == "1")
                log.WriteMyLog("平台返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                errmsg = "平台返回为空";
                return false; 
            }
            string RetCon = "";
            string Msg = "";
            string RetCode = "";

          

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
                 
                    errmsg = RetCon;
                    return false; 
                }
                else
                    return true;
            }
            catch (Exception e1)
            {
                errmsg = "提取信息异常,解析返回值异常：" + e1.Message;
              return  false;
            }
        }

        //连接MQ客户端
        //private static bool MQ(string fid, string putcmsg, string debug, ref string getcmsg)
        //{
        //    try
        //    {
        //        MQDLL.MQFunction MQManagment = new MQDLL.MQFunction();
        //        long ret = 0;
        //        //连接
        //        ret = MQManagment.connectMQ();

        //        if (ret != 1)
        //        {
        //          LGZGQClass.log.WriteMyLog("连接MQ服务失败!");
        //            return false;
        //        }
               
            
        //        string cmsgid = "";
        //        getcmsg = "";
        //        ret = 0;
        //        ret = MQManagment.putMsg(fid, putcmsg, ref cmsgid);
        //        ret = MQManagment.getMsgById(fid, cmsgid, 10000, ref getcmsg);



        //        if (getcmsg.Trim() == "")
        //        {
        //             LGZGQClass.log.WriteMyLog("提取信息失败，返回空");
        //            return false;
        //        }
        //        //断开
        //        MQManagment.disconnectMQ();
        //    }
        //    catch (Exception ee)
        //    {
        //         LGZGQClass.log.WriteMyLog(ee.Message);
        //        return false;
        //    }

        //    return true;
        //}

    }
}
