using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using dbbase;
using System.Data.SqlClient;
using System.IO;
using ZgqClassPub;
namespace PathHISZGQJK
{
    //��ɽ��ѧ����ҽԺ
   public  class zsdxzlyyPT
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
          dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        public void pathtohis(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        {
            string bgzt = "";
            bglx = bglx.ToLower();
            if (bglx == "") bglx = "cg";
            if (bgxh == "") bgxh = "1";
         
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;

            string tjodbcsql = f.ReadString("savetohis", "tj-odbcsql", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");
            
          
          
            DataTable jcxx = new DataTable();
            DataTable dt_bc = new DataTable();
            DataTable dt_bd = new DataTable();
            DataTable dt_sqd = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                return;
            }

           bgzt=jcxx.Rows[0]["F_BGZT"].ToString().Trim();
           string sqxh = jcxx.Rows[0]["F_sqxh"].ToString().Trim();
            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                    bgzt = "ȡ�����";
            }
            string errMsg = "";
                 string hczt = f.ReadString("zgqjk", "hczt", "1").Trim();
                string hcbg = f.ReadString("zgqjk", "hcbg", "1").Trim();
                string yhmc = f.ReadString("yh", "yhmc", "1").Trim();
                string yhbh = f.ReadString("yh", "yhbh", "1").Trim();
            if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
            {
                if(debug=="1")
                log.WriteMyLog("���ش�");
                  string tjjk = f.ReadString("zgqjk", "tjjk", "1").Trim();
                string tjtoptjk = f.ReadString("zgqjk", "tjtoptjk", "1").Trim();
                ////if (tjjk == "1")
                ////{
                ////    #region ���ӿ�
                ////    if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
                ////    {
                ////        aa.ExecuteSQL("update T_jcxx_fs set F_fszt='������',F_bz='��첡������������,������' where F_blbh='" + blbh + "'  and F_fszt='δ����'");
                ////       log.WriteMyLog(blh + ",��첡���޲��˱�ţ�������");
                ////        return;
                ////    }

                ////    if (bgzt == "�����")
                ////    {
                ////        DataTable TJ_bljc = new DataTable();
                ////        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                ////        // �������
                ////        string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                ////        //��Ͻ���	Res_con
                ////        string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();
                ////        if (TJ_bljc.Rows.Count > 0)
                ////        {
                ////            if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "���LCT" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "Һ��ϸ��")
                ////            {
                ////                Res_char = Res_char + "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";

                ////                Res_char = Res_char + "��Ŀ��" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                ////                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

                ////                Res_char = Res_char + "��ԭ�壺" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
                ////                    + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

                ////                Res_char = Res_char + "��֢ϸ������" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

                ////                ///////////���/////////////////////////
                ////                Res_con = "��ϣ�" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                ////                if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                ////                    Res_con = Res_con + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                ////            }
                ////        }
                ////        string str_com = "update  tj_pacs_resulto_temp  set Res_doctor='" + jcxx.Rows[0]["F_SHYS"].ToString().Trim() + "',Res_doctor_code='',Res_date='" + DateTime.Parse(jcxx.Rows[0]["F_BGrq"].ToString().Trim()) + "',Res_char='" + Res_char + "',Res_con='" + Res_con + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                ////        if (debug == "1")
                ////           log.WriteMyLog("��д������䣺" + str_com);
                ////        SqlDB db = new SqlDB();

                ////        int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref errMsg);
                ////        if (errMsg != "" && errMsg != "OK")
                ////        {
                ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + errMsg + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_bgzt='" + bgzt + "'");
                ////           log.WriteMyLog(blh + ",��챨����ˣ��ӿ��쳣��Ϣ��" + errMsg);
                ////        }
                ////        else
                ////        {
                ////           log.WriteMyLog(blh + ",��챨�淢�����");
                ////            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='�Ѵ���',F_bz='��챨�����,�ӿ��ϴ��ɹ���' where F_blbh='" + blbh + "'  and F_fszt='δ����'");
                ////        }
                ////    }
                ////    else
                ////    {
                ////        if (bgzt == "ȡ�����")
                ////        {
                ////            string str_com = "update  tj_pacs_resulto_temp set Res_doctor='" + "" + "',Res_doctor_code='',Res_date='" + DateTime.Today + "',Res_char='" + "" + "',Res_con='" + "" + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";
                ////            if (debug == "1")
                ////               log.WriteMyLog("��д������䣺" + str_com);
                ////            SqlDB db = new SqlDB();

                ////            int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref errMsg);
                ////            if (errMsg != "" && errMsg != "OK")
                ////            {
                ////                aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + errMsg + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_bgzt='" + bgzt + "'");
                ////               log.WriteMyLog(blh + ",��챨��ȡ����ˣ��ӿ��쳣��Ϣ��" + errMsg);
                ////            }
                ////            else
                ////                aa.ExecuteSQL("update T_jcxx_fs set F_fszt='�Ѵ���',F_bz='��챨��ȡ����ˣ��ӿ��ϴ��ɹ���' where F_blbh='" + blbh + "'  and F_fszt='δ����'");
                ////        }
                ////        else
                ////            aa.ExecuteSQL("update T_jcxx_fs set F_bz='δ֪������' where F_blbh='" + blbh + "'   and F_fszt='δ����'");
                ////    }
                ////    #endregion
                ////}
                if(tjtoptjk=="1")
                {
                    try
                    {
                        dt_sqd = aa.GetDataTable("select * from T_SQD where F_SQXH='" + sqxh + "'", "dt_sqd");
                    }
                    catch (Exception ex)
                    {
                       log.WriteMyLog(ex.Message.ToString());
                        return;
                    }
                    if (dt_sqd.Rows.Count > 0)
                    {
                        if (dt_sqd.Rows[0]["F_sqdzt"].ToString().Trim() != "�ѵǼ�")
                            aa.ExecuteSQL("update T_SQD set F_sqdzt='�ѵǼ�' where F_sqxh='" + sqxh + "'");
                    }
                    ////����첡�˻�д
                    ////����pdf �����ƶ�app
                    string PdfToBase64String = "";
                    if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1" && bgzt == "�����")
                    {
                        C_pdf(blh, bgxh, bglx, jcxx, false, ref PdfToBase64String, debug);
                    }

                    if (bgzt == "ȡ�����")
                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "'");


                        if (hczt == "1" && sqxh != "")
                        {
                            if (bglx == "cg" && (bgzt == "�ѵǼ�" || bgzt == "��ȡ��" || bgzt == "�����"))
                            {
                                TJ_ZtToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                            }
                        }
                        if (hcbg == "1" && (bgzt == "�����" || bgzt == "�ѷ���"))
                        {
                            TJ_BgToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                            return;
                        }
                        else
                        {
                            if (bgzt == "ȡ�����")
                            {
                                TJ_BgHSToPt(jcxx, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                            }
                        }
                }
                return;
            }
            else
            {
                string ptjk = f.ReadString("zgqjk", "ptjk", "1").Trim();
                string hisjk = f.ReadString("zgqjk", "hisjk", "0").Trim();
          
                try
                {
                    dt_sqd = aa.GetDataTable("select * from T_SQD where F_SQXH='" + sqxh + "'", "dt_sqd");
                }
                catch (Exception ex)
                {
                   log.WriteMyLog(ex.Message.ToString());
                    return;
                }
                if (dt_sqd.Rows.Count > 0)
                {
                    if (dt_sqd.Rows[0]["F_sqdzt"].ToString().Trim() != "�ѵǼ�")
                        aa.ExecuteSQL("update T_SQD set F_sqdzt='�ѵǼ�' where F_sqxh='" + sqxh + "'");
                }
                ////����첡�˻�д
                ////����pdf �����ƶ�app
                string PdfToBase64String = "";
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1"&& bgzt=="�����")
                {
                    C_pdf(blh, bgxh, bglx, jcxx, false, ref PdfToBase64String, debug);
                }

                if (bgzt == "ȡ�����")
                     aa.ExecuteSQL("delete T_BG_PDF  where F_BLbH='" + blbh + "'");

                if (ptjk == "1")
                {

                    if (hczt == "1" && sqxh!="")
                    {
                        if (bglx == "cg" && (bgzt == "�ѵǼ�" || bgzt == "��ȡ��" || bgzt == "�����"))
                        {
                            ZtToPt(jcxx, dt_sqd, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                        }
                    }
                    if (hcbg == "1" && (bgzt == "�����" || bgzt == "�ѷ���"))
                        {
                            string bgzt2 = "";
                            try
                            {
                                if (bglx.ToLower().Trim() == "bd")
                                {
                                    dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                                    bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                                }
                                if (bglx.ToLower().Trim() == "bc")
                                {
                                    dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                                    bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                                }
                                if (bglx.ToLower().Trim() == "cg")
                                    bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                            }
                            catch { }
                      
                            BgToPt(jcxx, dt_bc, dt_bd, dt_sqd, blh, bglx, bgxh, bgzt2, yhmc, yhbh, debug);
                            return;
                        }
                        else
                        {
                            if (bgzt == "ȡ�����")
                            {
                                BgHSToPt(jcxx, blh, bglx, bgxh, bgzt, yhmc, yhbh, debug);
                            }
                        }
                }
            }
        }
       
        public void ZtToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
            if (bglx!="cg")
                return;
            if(dt_sqd.Rows.Count<=0)
            {
                log.WriteMyLog(blh + ",��������޼�¼");
                return;
            }
            string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
            string GUID = "";
            string bgztxml = ZtMsg(dt_jcxx, dt_sqd, dt_jcxx.Rows[0]["F_SQXH"].ToString(), blh, bglx, bgxh, bgzt, yhmc, yhbh,ref  GUID);
                    if (bgztxml.Trim()== "")
                    {
                         log.WriteMyLog(blh + ",����״̬����xmlΪ��");
                        return;
                    }
                    if (debug == "1")
                        log.WriteMyLog("״̬[QI1_037Exam]:" + bgztxml);
                        try
                        {
                          
                            ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                                MQSer.Url = wsurl;
                                string msgtxt = "";
                                if (MQSer.SendMessageToMQ(bgztxml, ref msgtxt, "QI1_037Exam", "PIS_ExamState", GUID, "����״̬"))
                            {
                                if (debug == "1")
                                    log.WriteMyLog(blh + ",״̬[" + bgzt + "]���ͳɹ���" + msgtxt);
                            }
                            else
                                log.WriteMyLog(blh + ",״̬[" + bgzt + "]����ʧ�ܣ�" + msgtxt);
                        }
                        catch (Exception ee4)
                        {
                            log.WriteMyLog(blh + ",״̬[" + bgzt + "]�����쳣��" + ee4.Message);
                        }

                        return;
        }
        public void BgToPt(DataTable dt_jcxx, DataTable dt_bc, DataTable dt_bd, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {

            try
            {
                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;
                string url = f.ReadString("savetohis", "blweb", "http://172.16.95.230/pathwebrpt");
                string patientSerialNO = "";
                string jzlb = "1";
                string brlb = dt_jcxx.Rows[0]["F_brlb"].ToString();

                if (dt_sqd.Rows.Count < 1)
                {
                    if (brlb == "סԺ")
                        patientSerialNO = dt_jcxx.Rows[0]["F_ZYH"].ToString().Trim();
                    else
                        patientSerialNO = dt_jcxx.Rows[0]["F_MZH"].ToString().Trim();
                    if (brlb == "סԺ")
                        jzlb = "2";
                    else
                        jzlb = "1";
                }
                else
                {
                    patientSerialNO = dt_sqd.Rows[0]["F_JZH"].ToString().Trim();
                    jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString().Trim();
                }
                DataTable dt_tx = new DataTable();
                try
                {
                    dt_tx = aa.GetDataTable("select * from T_tx where F_BLH='" + blh + "' and F_SFDY='1'", "dt_sqd");
                }
                catch (Exception ex)
                {
                }

                DataTable dt_pdf = new DataTable();
                try
                {
                    dt_pdf = aa.GetDataTable("select * from T_BG_PDF where F_BLBH='" + blbh + "'", "dt_sqd");
                }
                catch (Exception ex)
                {
                    log.WriteMyLog(ex.Message.ToString());
                }
                string filePath = "";
                if (dt_pdf.Rows.Count > 0)
                {
                    filePath = dt_pdf.Rows[0]["F_pdfPath"].ToString().Replace("ftp","http");
                }
                string GUID = "";
                string rtnxml = BgMsg(dt_jcxx, dt_bc, dt_bd, dt_tx, dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim(), blh, bglx, bgxh, bgzt, jzlb, yhmc, yhbh, filePath, url, ref GUID);
                if (rtnxml.Trim() == "")
                {
                    log.WriteMyLog(blbh + ",����XMLʧ��:��");
                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='����XMLʧ��:��' where F_blbh='" + blbh + "' and F_BGZT='�����'");
                    return;
                }
                string msgtxt = "";
                try
                {
                    if (debug == "1")
                        log.WriteMyLog("�ش�����[QI1_001-PIS_Report]:" + rtnxml);

                    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                    MQSer.Url = wsurl;
                    if (MQSer.SendMessageToMQ(rtnxml, ref msgtxt, "QI1_001", "PIS_Report", GUID, "���淢��"))//PIS_Report
                    {
                        if (debug == "1")
                            log.WriteMyLog(blbh + ",���ͳɹ�:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='���ͳɹ�',F_FSZT='�Ѵ���' where F_blbh='" + blbh + "' and F_BGZT='�����'");
                    }
                    else
                    {
                        log.WriteMyLog(blbh + ",����ʧ��:" + msgtxt);
                         aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='����ʧ�ܣ�" + msgtxt + "' where F_blbh='" + blbh + "' and F_BGZT='�����'");
                    }
                }
                catch (Exception ee4)
                {
                    log.WriteMyLog(blbh + ",�����쳣��" + ee4.Message);
                     aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='�����쳣��" + ee4.Message + "' where F_blbh='" + blbh + "' and F_BGZT='�����'");

                }
            }
            catch(Exception  ee5)
            {
                log.WriteMyLog(ee5.Message);
            }
            return;
        }
        public string BgMsg(DataTable dt_jcxx, DataTable dt_bc, DataTable dt_bd, DataTable dt_tx, string sqxh, string blh, string bglx, string bgxh, string bgzt, string jzlb, string yhmc, string yhbh, string filePath, string url,ref string GUID)
        {
            filePath = filePath.ToLower().Replace("ftp", "http");
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
              GUID= Guid.NewGuid().ToString();
            //����xml
            string rtnxml = "<?xml version=\"1.0\"?>";
            rtnxml = rtnxml + "<PRPA_IN000003UV01>";
            try
            {
                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" +  GUID + "\"/>";
                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                rtnxml = rtnxml + "<telecom value=\"�豸���\"/>";
                rtnxml = rtnxml + "<softwareName code=\"HIP\" displayName=\"����ƽ̨����ϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</receiver>";

                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                rtnxml = rtnxml + "<telecom value=\"�豸���\"/>";
                rtnxml = rtnxml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</sender>";

                rtnxml = rtnxml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";

                rtnxml = rtnxml + "<authorOrPerformer typeCode=\"AUT\">";
                rtnxml = rtnxml + "<signatureText></signatureText>";
                rtnxml = rtnxml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                rtnxml = rtnxml + "</authorOrPerformer>";
                rtnxml = rtnxml + "<subject typeCode=\"SUBJ\">";

                rtnxml = rtnxml + "<request>";
                ////!--������ˮ��-->
                rtnxml = rtnxml + "<flowID>" + blbh + "</flowID>";
                ////<!--�������� - ���������䡢�ĵ��-->
                rtnxml = rtnxml + "<adviceType>����</adviceType>";
                ////<!--HIS�������� ���� סԺ-->
                rtnxml = rtnxml + "<patienttype>" + dt_jcxx.Rows[0]["F_BRLB"].ToString() + "</patienttype>";
                ////<!-- �ĵ�ע��ʱ�� -->
                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                ////<!-- ���ߺ� -->
                rtnxml = rtnxml + "<patientNo>" + dt_jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
                ////<!-- ���߾�����ˮ�� -->
                rtnxml = rtnxml + "<patientSerialNO>" + dt_jcxx.Rows[0]["F_ZY"].ToString() + "</patientSerialNO>";
                ////<!--HIS��ҽ����ˮ��,��ʶһ������-->
                rtnxml = rtnxml + "<adviceID>" + "" +"</adviceID>";
                ////<!--�Ķ�Ӧ�����뵥Ψһ��ʶ�ţ���ʶһ�μ��-->
                rtnxml = rtnxml + "<accessionNO>" + dt_jcxx.Rows[0]["F_sqxh"].ToString() + "</accessionNO>";
                ////<!--����������-->
                rtnxml = rtnxml + "<patientName>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</patientName>";
                ////<!--����ƴ����-->
                rtnxml = rtnxml + "<patientNameSpell></patientNameSpell>";
                ////<!--��������(���磺��19870601��)-->
                rtnxml = rtnxml + "<birthDate></birthDate>";
                ////<!--�Ա�-->
                string xb = dt_jcxx.Rows[0]["F_xb"].ToString();
                if (xb == "��") xb = "1";
                else if (xb == "Ů") xb = "2";
                else xb = "0";
                rtnxml = rtnxml + "<sex>" + xb + "</sex>";
                ////	<!--�����Ŀ����-->

                if (dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim().Contains("^"))
                {
                    rtnxml = rtnxml + "<procedureCode>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] + "</procedureCode>";
                    try
                    {
                        rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] + "</procedureName>";
                    }
                    catch
                    {
                        rtnxml = rtnxml + "<procedureName></procedureName>";
                    }
                }
                else
                {
                    rtnxml = rtnxml + "<procedureCode>" + "" + "</procedureCode>";
                    rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "</procedureName>";
                }
                ////	<!--��鲿λ-->
                rtnxml = rtnxml + "<positionName>" + dt_jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] + "</positionName>";
                ////	<!--����豸����-->
                rtnxml = rtnxml + "<modalityName></modalityName>";
                ////	<!--���������CT��MRI��-->

                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
                ////	<!-- ����ҽ������ -->
                rtnxml = rtnxml + "<authorCode></authorCode>";
                ////	<!--����ҽ��-->
                if (bglx == "bc")
                {
                    rtnxml = rtnxml + "<authorName>" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "</authorName>";
                    ////	<!--����ʱ��-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--���ҽ��-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_bc.Rows[0]["F_bc_shys"].ToString() + "</reportApprover>";
                    ////	<!--���ʱ��-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
                    ////	<!--��������-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
                    ////<!--��������-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_JXSJ"].ToString() + "]]></microscopeDiagnose>";
                    ////	<!--�������-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bc.Rows[0]["F_BCZD"].ToString() + "]]></conclusionDiagnose>";
                    ////	<!--��������-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_tsjc"].ToString() + "]]></reportDiagnose>";
                }
                else if (bglx == "bd")
                {
                    rtnxml = rtnxml + "<authorName>" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "</authorName>";
                    ////	<!--����ʱ��-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--���ҽ��-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_bd.Rows[0]["F_bd_shys"].ToString() + "</reportApprover>";
                    ////	<!--���ʱ��-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
                    ////	<!--��������-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
                    ////<!--��������-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[]]></microscopeDiagnose>";
                    ////	<!--�������-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bd.Rows[0]["F_BDZD"].ToString() + "]]></conclusionDiagnose>";
                    ////	<!--��������-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[]]></reportDiagnose>";
                }
                else
                {
                    rtnxml = rtnxml + "<authorName>" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
                    ////	<!--����ʱ��-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--���ҽ��-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
                    ////	<!--���ʱ��-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
                    ////	<!--��������-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
                    ////<!--��������-->
                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_JXSJ"].ToString() + "]]></microscopeDiagnose>";
                    ////	<!--�������-->
                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_BLZD"].ToString() + "]]></conclusionDiagnose>";
                    ////	<!--��������-->
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_jcxx.Rows[0]["F_tsjc"].ToString() + "]]></reportDiagnose>";
                }

                ////	<!-- ҽ���������ұ��� -->
                rtnxml = rtnxml + "<deptCode></deptCode>";
                ////	<!-- ҽ���������� -->
                rtnxml = rtnxml + "<deptName>�����</deptName>";

                ////	<!--��������-->
                rtnxml = rtnxml + "<reportContent></reportContent>";
                ////	<!-- ��Դϵͳ���� -->
                rtnxml = rtnxml + "<sourceCode>PIS</sourceCode>";
                ////	<!-- ��Դϵͳ��� -->
                rtnxml = rtnxml + "<sourceName>PIS</sourceName>";
                rtnxml = rtnxml + "<providerOrganization>42321679-4</providerOrganization>";
                ////	<!-- �ĵ���� -->
                rtnxml = rtnxml + "<indexInSystem>" + blbh + "</indexInSystem> ";
                ////	<!-- �ĵ����ͱ��� -->
                rtnxml = rtnxml + "<typeCode></typeCode>";
                ///	<!-- �ĵ��������� -->
                rtnxml = rtnxml + "<typeCodeName></typeCodeName>";
                ////	<!-- �ĵ����� -->
                rtnxml = rtnxml + "<title></title>";
                    rtnxml = rtnxml + "<reportListURL>" + filePath + "</reportListURL>";
                ////	<!--Ӱ�����URL-->
                rtnxml = rtnxml + "<imageList>";
                try
                {
                    for (int j = 0; j < dt_tx.Rows.Count; j++)
                    {
                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + dt_jcxx.Rows[0]["F_txml"].ToString() + "/" + dt_tx.Rows[j]["F_TXM"] + "</imageURL>";
                    }
                }
                catch
                {
                }
                rtnxml = rtnxml + "</imageList>";
                ////	<!--�����ֶ�1-->";
                rtnxml = rtnxml + "<other1></other1>";
                ////	<!--�����ֶ�2-->
                rtnxml = rtnxml + "<other2></other2>";
                ////	<!--�����ֶ�3-->
                rtnxml = rtnxml + "<other3></other3>";
                ////	<!--������±�־��0-PACS������1-���Ӳ�����ȡ��2-PACS�޸�-->
                rtnxml = rtnxml + "<updateFlag></updateFlag>";

                ////	<!-- ����PDA�ĵ������ƴ� BASE64 -->
                rtnxml = rtnxml + "<body>";
                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + "" + "</text>";
                rtnxml = rtnxml + "</body>";
                rtnxml = rtnxml + "</request>";
                rtnxml = rtnxml + "</subject>";
                rtnxml = rtnxml + "</controlActProcess>";
                rtnxml = rtnxml + "</PRPA_IN000003UV01>";

                return rtnxml;
            }
            catch (Exception e2)
            {
                log.WriteMyLog(blbh + ",��������XML�쳣��" + e2.Message);
                return "";
            }
        }
        public string ZtMsg( DataTable dt_jcxx,DataTable dt_sqd, string sqxh,string blh,string bglx,string bgxh,string bgzt,string yhmc,string yhbh,ref string GUID)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
             GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"COMT_IN001103UV01\"/>";
                xml = xml + "<processingCode code=\"T\"/>";
                xml = xml + "<processingModeCode code=\"I\"/>";
                xml = xml + "<acceptAckCode code=\"AA\"/>";

                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- ������д�绰��Ϣ����URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"�豸���\"/>";
                xml = xml + "<softwareName code=\"HIP\" displayName=\"����ƽ̨����ϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"�豸���\"/>";
                xml = xml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<signatureText></signatureText>";
                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<subject typeCode=\"SUBJ\">";
                xml = xml + "<actGenericStatus classCode=\"DOCCLIN\" moodCode=\"EVN\">";
                xml = xml + "<!--ҵ��ID������ID��-->";
                xml = xml + "<id root=\"2.16.156.10011.1.1\" extension=\"" + blbh + "\"/>";
                xml = xml + "<!--ҵ����� ״̬����-->";

                if (bgzt == "�����")
                    xml = xml + "<code code=\"60\" displayName=\"���淢��\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"ҵ�����ʹ����\"/>";
                else if (bgzt == "��д����")
                    xml = xml + "<code code=\"50\" displayName=\"���\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"ҵ�����ʹ����\"/>";
                else 
                    xml = xml + "<code code=\"40\" displayName=\"ִ��\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"ҵ�����ʹ����\"/>";

                xml = xml + "<!--ҵ��״̬ ȫΪcompleted-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!--ҵ���ڼ�-->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                xml = xml + "<!--ִ�п�ʼʱ��-->";
                xml = xml + "<low value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<!--ִ�н���ʱ��-->";
                xml = xml + "<high value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "</effectiveTime>";
                xml = xml + "<!--ִ����0..*-->";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!--ҽ����ԱID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\""+yhbh+"\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + yhmc + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!--ҽ���������������ң�ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<!--��ִ�����뵥��ҽ��-->";
                xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                xml = xml + "<!--�������뵥���-->";
                xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" +sqxh + "\"/>";
                xml = xml + "<!--ҽ��ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + "" + "\"/>";
                xml = xml + "</actIntent>";
                xml = xml + "</inFulfillmentOf>";
                xml = xml + "<componentOf typeCode=\"COMP\" xsi:nil=\"false\">";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!--��(��)����ˮ��-->";
                    xml = xml + "<id root=\"2.999.1.91\" extension=\"" + dt_sqd.Rows[0]["F_MZLSH"].ToString() + "\"/>";
                    xml = xml + "<!--סԺ��ˮ�� -->";
                    xml = xml + "<id root=\"2.999.1.42\" extension=\"" + dt_sqd.Rows[0]["F_ZYLSH"].ToString() + "\"/>";
                    xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_JZLB"].ToString() + "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"�������ʹ����\" displayName=\"" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                xml = xml + "<statusCode/>";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<!--ƽ̨ע��Ļ���ID -->";
                xml = xml + "<id root=\"2.999.1.37\" extension=\"" + dt_sqd.Rows[0]["F_EMPIID"].ToString() + "\"/>";
                xml = xml + "<!--����ϵͳ�Ļ���ID -->";
                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_patientid"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_ZLH"].ToString() + "\"/>";
                xml = xml + "<statusCode/>";
                xml = xml + "<patientPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ��������  -->";
                xml = xml + "<name>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "</patientPerson>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf>";
                xml = xml + "</actGenericStatus>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</COMT_IN001103UV01>";
                return xml;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("����״̬����XML�쳣��" + ee.Message);
                return "";
            }
        }
        public void BgHSToPt(DataTable dt_jcxx, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
                    string xml = "";
                    string GUID = Guid.NewGuid().ToString();
                    try
                    {
                        xml = xml + "<?xml version=\"1.0\"?>";
                        xml = xml + "<PRPA_IN000003UV04> ";
                        xml = xml + "<!-- UUID,����ʵ��Ψһ��-->";
                        xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                        xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                        xml = xml + "<interactionId extension=\"PRPA_IN000003UV04\" root=\"2.16.840.1.113883.1.6\"/>";
 

                        xml = xml + "<receiver typeCode=\"RCV\">";
                        xml = xml + "<!-- ������д�绰��Ϣ����URL-->";
                        xml = xml + "<telecom value=\"\"/>";
                        xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                        xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                        xml = xml + "<telecom value=\"�豸���\"/>";
                        xml = xml + "<softwareName code=\"HIP\" displayName=\"����ƽ̨����ϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                        xml = xml + "</device>";
                        xml = xml + "</receiver>";

                        xml = xml + "<sender typeCode=\"SND\">";
                        xml = xml + "<telecom value=\"\"/>";
                        xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                        xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                        xml = xml + "<telecom value=\"�豸���\"/>";
                        xml = xml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                        xml = xml + "</device>";
                        xml = xml + "</sender>";

                        xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                        xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                        xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

                        xml = xml + "<!--������ˮ��-->";
                        xml = xml + "<flowID>" + blbh+ "</flowID>";
                        xml = xml + "<!--�������� - ���������䡢�ĵ��-->";
                        xml = xml + "<adviceType>����</adviceType>";
                        xml = xml + "<!--HIS�������� ���� סԺ�����-->";
                        xml = xml + "<patienttype>" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "</patienttype>";
                        xml = xml + "<!-- �ĵ�ע��ʱ�� -->";
                        xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                        xml = xml + "<!-- ��Դϵͳ���루��� -->";
                        xml = xml + "<sourceCode/>";
                        xml = xml + "<!-- ��Դϵͳ��� -->";
                        xml = xml + "<sourceName>PIS</sourceName>";
                        xml = xml + "<!-- �ĵ���ţ���� -->";
                        xml = xml + "<indexInSystem>" + blbh + "</indexInSystem>";

                        xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
                    }
                    catch (Exception ee4)
                    {
                       log.WriteMyLog(blbh + ",�����ٻ�XML�쳣��" + ee4.Message);
                         aa.ExecuteSQL("update T_jcxx_fs set F_BZ='�ٻ�XML�쳣��" + ee4.Message + "' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
                        return;
                    }
                    if (xml.Trim() == "")
                    {
                       log.WriteMyLog(blbh + ",�����ٻ�����xmlΪ��");
                         aa.ExecuteSQL("update T_jcxx_fs set F_BZ='�����ٻ�xmlΪ��' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
                        return;
                    }

                    string msgtxt = "";
                    try
                    {
                        if (debug == "1")
                           log.WriteMyLog("�����ٻأ�[QI1_001--PIS_ReportExpired]" + xml);

                 string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
               
                        MQSer.Url = wsurl;


                   if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_001", "PIS_ReportExpired", GUID,"�������"))
                        {
                            if (debug == "1")
                            {
                               log.WriteMyLog(blbh + ",�����ٻسɹ���" + msgtxt);
                            }
                            aa.ExecuteSQL("update T_jcxx_fs set GUID='"+GUID+"',F_BZ='�����ٻسɹ�:" + msgtxt + "',F_FSZT='�Ѵ���' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
                            return;
                        }
                        else
                        {
                           log.WriteMyLog(blbh + ",�����ٻ�ʧ�ܣ�" + msgtxt);
                            aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='�����ٻ�ʧ��:" + msgtxt + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='ȡ�����'");
                            return;
                        }
                    }
                    catch (Exception ee4)
                    {
                        log.WriteMyLog(blbh + ",�����ٻ��쳣��" + ee4.Message);
                         aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='�����ٻ��쳣:" + ee4.Message + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='ȡ�����'");
                        return;
                    }
        }
     
        //public void pathtohis2016(string blh, string bglx, string bgxh, string msg, string debug, string[] cslb)
        //{
        //    string qxsh = "";
        //    string xdj = "";
        //    bglx = bglx.ToLower();

        //     LGZGQClass.log.WriteMyLog("�ӿڿ�ʼ");

        //    if (cslb.Length == 5)
        //    {
        //        if (cslb[4].ToLower() == "qxsh")
        //        {
        //            //ȡ����˶���
        //            qxsh = "1";
        //        }

        //        if (cslb[3].ToLower() == "new")
        //        {
        //            xdj = "1";
        //        }

        //    }


        //    if (bglx == "")
        //        bglx = "cg";

        //    if (bgxh == "")
        //        bgxh = "0";

        //    string tjodbcsql = f.ReadString("savetohis", "tj-odbcsql", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");
        //    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");

        //    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        //    DataTable jcxx = new DataTable();
        //    try
        //    {
        //        jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message.ToString());
        //        return;
        //    }
        //    if (jcxx == null)
        //    {
        //        MessageBox.Show("�������ݿ����������⣡");
        //        return;
        //    }
        //    if (jcxx.Rows.Count < 1)
        //    {
        //        MessageBox.Show("������д���");
        //        return;
        //    }

        //    if (bglx.Trim() == "")
        //    {
        //         LGZGQClass.log.WriteMyLog("��������Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);
        //        return;
        //    }


        //    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
        //    MQSer.Url = wsurl;



        //     LGZGQClass.log.WriteMyLog("��ʼ�ش�״̬");
        //    string msgtxt = "";

        //    if (jcxx.Rows[0]["F_HXBZ"].ToString().Trim() == "")
        //    {
        //        if (bglx == "cg")
        //        {
        //            #region  �ش��������״̬

        //            string[] yzhs = jcxx.Rows[0]["F_yzid"].ToString().Split('^');
        //            foreach (string yzh in yzhs)
        //            {
        //                if (yzh != "")
        //                {

        //                    string bgztxml = bgzt_XML("�ѵǼ�", jcxx, yzh);
        //                     LGZGQClass.log.WriteMyLog(bgztxml);
        //                    if (bgztxml.Trim() != "")
        //                    {
        //                        try
        //                        {
        //                            if (MQSer.SendMessage(bgztxml, ref msgtxt, "QI1_037", string.Empty))
        //                            {
        //                                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",����ִ��״̬���ͳɹ���" + msgtxt);
        //                                aa.ExecuteSQL("update T_jcxx set F_hxbz='1' where F_blh='" + blh + "'");

        //                            }
        //                            else
        //                            {
        //                                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",����ִ��״̬����ʧ�ܣ�" + msgtxt);
        //                            }
        //                        }
        //                        catch (Exception ee4)
        //                        {
        //                             LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",����ִ��״̬�����쳣��" + ee4.Message);
        //                        }
        //                    }
        //                }
        //            }
        //            #endregion
        //        }

        //    }
        //     LGZGQClass.log.WriteMyLog("��ʼ�ش����");


        //    ////��������д�ӿ�/////zgq

        //    string bgzt = "";

        //    if (qxsh == "1")
        //    {
        //        bgzt = "ȡ�����";
        //    }


        //    if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
        //    {
        //         LGZGQClass.log.WriteMyLog("���ش�");
        //        #region ���ӿ�
        //        if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
        //        {
        //            aa.ExecuteSQL("update T_jcxx_fs set F_fszt='������',F_bz='��첡������������,������' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //             LGZGQClass.log.WriteMyLog(blh + ",��첡���޲��˱�ţ�������");
        //            return;
        //        }


        //        if (jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "�����" && bgzt != "ȡ�����")
        //        {
        //            DataTable TJ_bljc = new DataTable();
        //            TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");


        //            // �������
        //            string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
        //            //��Ͻ���	Res_con
        //            string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

        //            if (TJ_bljc.Rows.Count > 0)
        //            {
        //                if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "���LCT" || jcxx.Rows[0]["F_blk"].ToString().Trim() == "Һ��ϸ��")
        //                {
        //                    Res_char = Res_char + "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";

        //                    Res_char = Res_char + "��Ŀ��" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
        //                        + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

        //                    Res_char = Res_char + "��ԭ�壺" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
        //                        + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

        //                    Res_char = Res_char + "��֢ϸ������" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

        //                    ///////////���/////////////////////////
        //                    Res_con = "��ϣ�" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
        //                    if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
        //                        Res_con = Res_con + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
        //                }
        //            }

        //            /////////////////////////////////////////////////////

        //            string str_com = "update  tj_pacs_resulto_temp  set Res_doctor='" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "',Res_doctor_code='',Res_date='" + DateTime.Parse(jcxx.Rows[0]["F_BGrq"].ToString().Trim()) + "',Res_char='" + Res_char + "',Res_con='" + Res_con + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";


        //            if (debug == "1")
        //            {
        //                MessageBox.Show("��д������䣺" + str_com);
        //            }
        //            SqlDB db = new SqlDB();
        //            string Exceptionmessage = "";
        //            int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref Exceptionmessage);
        //            if (Exceptionmessage != "" && Exceptionmessage != "OK")
        //            {

        //                if (Exceptionmessage.Length > 200)
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='δ����' and F_bgzt='" + bgzt + "'");
        //                else
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_bgzt='" + bgzt + "'");

        //                 LGZGQClass.log.WriteMyLog(blh + ",��챨����ˣ��ӿ��쳣��Ϣ��" + Exceptionmessage);
        //            }
        //            else
        //            {
        //                aa.ExecuteSQL("update T_jcxx_fs set F_fszt='�Ѵ���',F_bz='��챨�����,�ӿ��ϴ��ɹ���' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //            }

        //        }
        //        else
        //        {
        //            //ȡ�����ʱ
        //            if (bgzt == "ȡ�����")
        //            {
        //                string str_com = "update  tj_pacs_resulto_temp set Res_doctor='" + "" + "',Res_doctor_code='',Res_date='" + DateTime.Today + "',Res_char='" + "" + "',Res_con='" + "" + "',Res_flag=2 where res_no='" + jcxx.Rows[0]["F_SQXH"].ToString().Trim() + "'";

        //                if (debug == "1")
        //                {
        //                    MessageBox.Show("��д������䣺" + str_com);
        //                }
        //                SqlDB db = new SqlDB();
        //                string Exceptionmessage = "";
        //                int x = db.ExecuteNonQuery(tjodbcsql, str_com, ref Exceptionmessage);
        //                if (Exceptionmessage != "" && Exceptionmessage != "OK")
        //                {
        //                     LGZGQClass.log.WriteMyLog(Exceptionmessage);

        //                    if (Exceptionmessage.Length > 200)
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage.Substring(0, 200) + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "'  and F_fszt='δ����' and F_bgzt='" + bgzt + "'");
        //                    else
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_bz='" + Exceptionmessage + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����' and F_bgzt='" + bgzt + "'");

        //                     LGZGQClass.log.WriteMyLog(blh + ",��챨��ȡ����ˣ��ӿ��쳣��Ϣ��" + Exceptionmessage);
        //                }
        //                else
        //                {
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='�Ѵ���',F_bz='��챨��ȡ����ˣ��ӿ��ϴ��ɹ���' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //                }
        //            }
        //            else
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='δ֪������' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");

        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //         LGZGQClass.log.WriteMyLog("��������pdf");
        //        //����첡�˻�д
        //        //����pdf �����ƶ�app
        //        string bgzt2 = "";
        //        DataTable dt_bd = new DataTable();
        //        DataTable dt_bc = new DataTable();
        //        try
        //        {
        //            if (bglx.ToLower().Trim() == "bd")
        //            {
        //                dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
        //                bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
        //            }

        //            if (bglx.ToLower().Trim() == "bc")
        //            {
        //                dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
        //                bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();
        //            }
        //            if (bglx.ToLower().Trim() == "cg")
        //              bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
        //        }
        //        catch
        //        {}

        //        if (bgzt2.Trim() == "")
        //        {
        //             LGZGQClass.log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);
        //        }
        //        if (bgzt2.Trim() == "�����" && bgzt != "ȡ�����")
        //        {
        //            string ReprotFile = ""; string jpgname = "";
        //            string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
        //            if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
        //            {
        //                #region  ����pdf

        //                //string ML = "";
        //                string message = "";
        //                ZgqClass zgq = new ZgqClass();
        //                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqClass.type.PDF, ref message, ref jpgname);

        //                string xy = "3";// ZgqClass.GetSz("ZGQJK", "sctxfs", "3");
        //                if (isrtn)
        //                {
        //                     LGZGQClass.log.WriteMyLog("����PDF�ɹ�");

        //                    //�����ƴ�
        //                    if (File.Exists(jpgname))
        //                    {
        //                        try
        //                        {
        //                            FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
        //                            Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
        //                            file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
        //                            file.Close();
        //                            ReprotFile = Convert.ToBase64String(imgByte);
        //                             LGZGQClass.log.WriteMyLog("PDFת�������ƴ��ɹ�");
        //                        }
        //                        catch (Exception ee)
        //                        {

        //                             LGZGQClass.log.WriteMyLog("PDFת�������ƴ�ʧ��");
        //                        }
        //                    }


        //                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
        //                    if (ssa == true)
        //                    {
        //                         LGZGQClass.log.WriteMyLog("�ϴ�PDF�ɹ�");
        //                        jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
        //                        ZgqClass.BGHJ(blh, "�����ϴ�", "���", "����PDF�ɹ�:" + ML + "\\" + jpgname, "ZGQJK", "����PDF");

        //                        aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
        //                        aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "')");
        //                        aa.ExecuteSQL("update T_JCXX_FS set F_bz='����PDF�ɹ�',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx.ToLower() + "' and F_BGXH='" + bgxh + "'");

        //                    }
        //                    else
        //                    {
        //                         LGZGQClass.log.WriteMyLog("����PDFʧ��");
        //                         LGZGQClass.log.WriteMyLog(message);
        //                        ZgqClass.BGHJ(blh, "�����ϴ�", "���", message, "ZGQJK", "����PDF");
        //                        aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_bz='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
        //                    }
        //                    zgq.DeleteTempFile(blh);

        //                }
        //                else
        //                    aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_BZ='" + message + "'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");

        //                #endregion
        //            }
        //             LGZGQClass.log.WriteMyLog("����pdf���");
        //            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //            if (jcxx.Rows[0]["F_SQXH"].ToString().Trim() == "")
        //            {
        //                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ", �������Ϊ��,������");
        //                aa.ExecuteSQL("update T_jcxx_fs set F_FSZt='������',F_BZ='�������Ϊ��,������' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='�����'");
        //                return;
        //            }
        //            string url = f.ReadString("savetohis", "blweb", "http://172.16.95.230/pathwebrpt");
        //            string err = "";
        //            msgtxt = "";

        //            if(debug=="1")
        //             LGZGQClass.log.WriteMyLog("�ش�����״̬");
        //            if (bglx == "cg")
        //            {
        //                #region  �ش��������״̬

        //                        string bgztxml = bgzt_XML("�����", jcxx, "");
        //                         LGZGQClass.log.WriteMyLog("�ش��������״̬XML:" + bgztxml);
        //                        if (bgztxml.Trim() != "")
        //                        {
        //                            try
        //                            {
        //                                if (MQSer.SendMessage(bgztxml, ref msgtxt, "QI1_037", string.Empty))
        //                                {
        //                                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",�������״̬���ͳɹ���" + msgtxt);
        //                                }
        //                                else
        //                                {
        //                                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",�������״̬����ʧ�ܣ�" + msgtxt);
        //                                }
        //                            }
        //                            catch (Exception ee4)
        //                            {
        //                                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",�������״̬�����쳣��" + ee4.Message);
        //                            }
        //                }
        //                #endregion
        //            }
        //             LGZGQClass.log.WriteMyLog("�ش��������״̬���");

        //            #region ��˱���ش�ƽ̨


        //            //����xml
        //            string rtnxml = "<?xml version=\"1.0\"?>";
        //            rtnxml = rtnxml + "<PRPA_IN000003UV01>";
        //            try
        //            {
        //                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
        //                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
        //                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

        //                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
        //                rtnxml = rtnxml + "<telecom value=\"\"/>";
        //                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
        //                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
        //                rtnxml = rtnxml + "<telecom value=\"�豸���\"/>";
        //                rtnxml = rtnxml + "<softwareName code=\"HIP\" displayName=\"ҽԺ��Ϣƽ̨\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
        //                rtnxml = rtnxml + "</device>";
        //                rtnxml = rtnxml + "</receiver>";

        //                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
        //                rtnxml = rtnxml + "<telecom value=\"\"/>";
        //                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
        //                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
        //                rtnxml = rtnxml + "<telecom value=\"�豸���\"/>";
        //                rtnxml = rtnxml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
        //                rtnxml = rtnxml + "</device>";
        //                rtnxml = rtnxml + "</sender>";

        //                rtnxml = rtnxml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";

        //                rtnxml = rtnxml + "<authorOrPerformer typeCode=\"AUT\">";
        //                rtnxml = rtnxml + "<signatureText></signatureText>";
        //                rtnxml = rtnxml + "<assignedDevice classCode=\"ASSIGNED\"/>";
        //                rtnxml = rtnxml + "</authorOrPerformer>";
        //                rtnxml = rtnxml + "<subject typeCode=\"SUBJ\">";

        //                rtnxml = rtnxml + "<request>";
        //                ////!--������ˮ��-->
        //                rtnxml = rtnxml + "<flowID>" + blh + "_" + bglx + "_" + bgxh + "</flowID>";
        //                ////<!--�������� - ���������䡢�ĵ��-->
        //                rtnxml = rtnxml + "<adviceType>����</adviceType>";
        //                ////<!--HIS�������� ���� סԺ-->

        //                string brlb = jcxx.Rows[0]["F_brlb"].ToString();
        //                if (brlb == "סԺ")
        //                    brlb = "2";
        //                else 
        //                    brlb = "1";
                       
        //                rtnxml = rtnxml + "<patienttype>" + brlb + "</patienttype>";
        //                ////<!-- �ĵ�ע��ʱ�� -->
        //                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
        //                ////<!-- ���ߺ� -->
        //                rtnxml = rtnxml + "<patientNo>" + jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
        //                ////<!-- ���߾�����ˮ�� -->
        //                rtnxml = rtnxml + "<patientSerialNO>" + jcxx.Rows[0]["F_ZYH"].ToString() + "</patientSerialNO>";
        //                ////<!--HIS��ҽ����ˮ��,��ʶһ������-->
        //                rtnxml = rtnxml + "<adviceID>" + jcxx.Rows[0]["F_yzid"].ToString() + "</adviceID>";
        //                ////<!--�Ķ�Ӧ�����뵥Ψһ��ʶ�ţ���ʶһ�μ��-->
        //                rtnxml = rtnxml + "<accessionNO>" + jcxx.Rows[0]["F_sqxh"].ToString() + "</accessionNO>";
        //                ////<!--����������-->
        //                rtnxml = rtnxml + "<patientName>" + jcxx.Rows[0]["F_xm"].ToString() + "</patientName>";
        //                ////<!--����ƴ����-->
        //                rtnxml = rtnxml + "<patientNameSpell></patientNameSpell>";
        //                ////<!--��������(���磺��19870601��)-->
        //                rtnxml = rtnxml + "<birthDate></birthDate>";
        //                ////<!--�Ա�-->
        //                rtnxml = rtnxml + "<sex>" + jcxx.Rows[0]["F_xb"].ToString() + "</sex>";
        //                ////	<!--�����Ŀ����-->
        //                rtnxml = rtnxml + "<procedureCode>" + jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] + "</procedureCode>";
        //                try
        //                {
        //                    rtnxml = rtnxml + "<procedureName>" + jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] + "</procedureName>";
        //                }
        //                catch
        //                {
        //                    rtnxml = rtnxml + "<procedureName></procedureName>";
        //                }
        //                ////	<!--��鲿λ-->
        //                rtnxml = rtnxml + "<positionName>" + jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] + "</positionName>";
        //                ////	<!--����豸����-->
        //                rtnxml = rtnxml + "<modalityName></modalityName>";
        //                ////	<!--���������CT��MRI��-->

        //                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
        //                ////	<!-- ����ҽ������ -->
        //                rtnxml = rtnxml + "<authorCode></authorCode>";
        //                ////	<!--����ҽ��-->
        //                if (bglx == "bc")
        //                {
        //                    rtnxml = rtnxml + "<authorName>" + dt_bc.Rows[0]["F_bc_bgys"].ToString() + "</authorName>";
        //                    ////	<!--����ʱ��-->
        //                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
        //                    ////	<!--���ҽ��-->
        //                    rtnxml = rtnxml + "<reportApprover>" + dt_bc.Rows[0]["F_bc_shys"].ToString() + "</reportApprover>";
        //                    ////	<!--���ʱ��-->
        //                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bc.Rows[0]["F_bc_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
        //                    ////	<!--��������-->
        //                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
        //                    ////<!--��������-->
        //                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_JXSJ"].ToString() + "]]></microscopeDiagnose>";
        //                    ////	<!--�������-->
        //                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bc.Rows[0]["F_BCZD"].ToString() + "]]></conclusionDiagnose>";
        //                    ////	<!--��������-->
        //                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + dt_bc.Rows[0]["F_bc_tsjc"].ToString() + "]]></reportDiagnose>";
        //                }
        //                else if (bglx == "bd")
        //                {
        //                    rtnxml = rtnxml + "<authorName>" + dt_bd.Rows[0]["F_bd_bgys"].ToString() + "</authorName>";
        //                    ////	<!--����ʱ��-->
        //                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
        //                    ////	<!--���ҽ��-->
        //                    rtnxml = rtnxml + "<reportApprover>" + dt_bd.Rows[0]["F_bd_shys"].ToString() + "</reportApprover>";
        //                    ////	<!--���ʱ��-->
        //                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_bd.Rows[0]["F_bd_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
        //                    ////	<!--��������-->
        //                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
        //                    ////<!--��������-->
        //                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[]]></microscopeDiagnose>";
        //                    ////	<!--�������-->
        //                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + dt_bd.Rows[0]["F_BDZD"].ToString() + "]]></conclusionDiagnose>";
        //                    ////	<!--��������-->
        //                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[]]></reportDiagnose>";
        //                }
        //                else
        //                {
        //                    rtnxml = rtnxml + "<authorName>" + jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
        //                    ////	<!--����ʱ��-->
        //                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
        //                    ////	<!--���ҽ��-->
        //                    rtnxml = rtnxml + "<reportApprover>" + jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
        //                    ////	<!--���ʱ��-->
        //                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";
        //                    ////	<!--��������-->
        //                    rtnxml = rtnxml + "<nakedEyeDiagnose><![CDATA[" + jcxx.Rows[0]["F_rysj"].ToString() + "]]></nakedEyeDiagnose>";
        //                    ////<!--��������-->
        //                    rtnxml = rtnxml + "<microscopeDiagnose><![CDATA[" + jcxx.Rows[0]["F_JXSJ"].ToString() + "]]></microscopeDiagnose>";
        //                    ////	<!--�������-->
        //                    rtnxml = rtnxml + "<conclusionDiagnose><![CDATA[" + jcxx.Rows[0]["F_BLZD"].ToString() + "]]></conclusionDiagnose>";
        //                    ////	<!--��������-->
        //                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + jcxx.Rows[0]["F_tsjc"].ToString() + "]]></reportDiagnose>";
        //                }

        //                ////	<!-- ҽ���������ұ��� -->
        //                rtnxml = rtnxml + "<deptCode></deptCode>";
        //                ////	<!-- ҽ���������� -->
        //                rtnxml = rtnxml + "<deptName>�����</deptName>";

        //                ////	<!--��������-->
        //                rtnxml = rtnxml + "<reportContent></reportContent>";
        //                ////	<!-- ��Դϵͳ���� -->
        //                rtnxml = rtnxml + "<sourceCode>1035</sourceCode>";
        //                ////	<!-- ��Դϵͳ��� -->
        //                rtnxml = rtnxml + "<sourceName>PIS</sourceName>";
        //                rtnxml = rtnxml + "<providerOrganization>42321679-4</providerOrganization>";
        //                ////	<!-- �ĵ���� -->
        //                rtnxml = rtnxml + "<indexInSystem>" + blh + "_" + bglx + "_" + bgxh + "</indexInSystem> ";
        //                ////	<!-- �ĵ����ͱ��� -->
        //                rtnxml = rtnxml + "<typeCode></typeCode>";
        //                ///	<!-- �ĵ��������� -->
        //                rtnxml = rtnxml + "<typeCodeName></typeCodeName>";
        //                ////	<!-- �ĵ����� -->
        //                rtnxml = rtnxml + "<title></title>";
        //                try
        //                {
        //                    //////	<!--����URL-->
        //                    //DataTable dt_pdf = new DataTable();
        //                    //dt_pdf = aa.GetDataTable("select top 10 * from dbo.T_BG_PDF where F_BLH='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "' order by F_ID desc", "T_PDF");
        //                    //if (dt_pdf.Rows.Count > 0)
        //                    //    rtnxml = rtnxml + "<reportListURL>" + url + "/pdfbg/" + dt_pdf.Rows[0]["F_ml"].ToString() + "/" + dt_pdf.Rows[0]["F_filename"].ToString() + "</reportListURL>";
        //                    //else

        //                    rtnxml = rtnxml + "<reportListURL>" + url + "/pdfbg/" + ML + "/" + blh + "/" + jpgname + "</reportListURL>";
        //                }
        //                catch
        //                {
        //                    rtnxml = rtnxml + "<reportListURL></reportListURL>";
        //                }

        //                ////	<!-- ����PDA�ĵ������ƴ� BASE64 -->
        //                rtnxml = rtnxml + "<body>";
        //                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + ReprotFile + "</text>";
        //                rtnxml = rtnxml + "</body>";
        //                ////	<!--Ӱ�����URL-->
        //                rtnxml = rtnxml + "<imageList>";
        //                try
        //                {
        //                    DataTable dt_jpg = new DataTable();
        //                    dt_jpg = aa.GetDataTable("select top 20 * from dbo.V_DYTX where F_BLH='" + jcxx.Rows[0]["F_BLH"].ToString() + "' ", "T_TX");
        //                    for (int j = 0; j < dt_jpg.Rows.Count; j++)
        //                    {
        //                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + jcxx.Rows[0]["F_txml"].ToString() + "/" + dt_jpg.Rows[j]["F_TXM"] + "</imageURL>";
        //                    }
        //                }
        //                catch
        //                {
        //                }
        //                rtnxml = rtnxml + "</imageList>";
        //                ////	<!--�����ֶ�1-->";
        //                rtnxml = rtnxml + "<other1></other1>";
        //                ////	<!--�����ֶ�2-->
        //                rtnxml = rtnxml + "<other2></other2>";
        //                ////	<!--�����ֶ�3-->
        //                rtnxml = rtnxml + "<other3></other3>";
        //                ////	<!--������±�־��0-PACS������1-���Ӳ�����ȡ��2-PACS�޸�-->
        //                rtnxml = rtnxml + "<updateFlag></updateFlag>";
        //                rtnxml = rtnxml + "</request>";

        //                rtnxml = rtnxml + "</subject>";
        //                rtnxml = rtnxml + "</controlActProcess>";
        //                rtnxml = rtnxml + "</PRPA_IN000003UV01>";

        //                //if(debug=="1")
        //                // LGZGQClass.log.WriteMyLog(rtnxml);


        //            }
        //            catch (Exception e2)
        //            {
        //                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",��������XML�쳣��" + e2.Message);
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='��������XML�쳣��" + e2.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='�����'");

        //                return;
        //            }

        //            if (debug == "1")
        //                 LGZGQClass.log.WriteMyLog("�ش����棺" + rtnxml);
        //            msgtxt = "";
        //            try
        //            {
        //                if (MQSer.SendMessage(rtnxml, ref msgtxt, "QI1_001", "PIS_Report"))
        //                {
        //                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",���淢�ͳɹ���" + msgtxt);
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='���淢�ͳɹ�',F_FSZT='�Ѵ���' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='�����'");

        //                }
        //                else
        //                {
        //                     LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",���淢��ʧ�ܣ�" + msgtxt);
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='���淢��ʧ�ܣ�" + msgtxt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='�����'");

        //                } return;
        //            }
        //            catch (Exception ee4)
        //            {
        //                 LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",���淢���쳣��" + ee4.Message);
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='���淢���쳣��" + ee4.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_BGZT='�����'");

        //            }
        //            return;

        //            #endregion

        //        }
        //        else
        //        {
        //            if (bgzt == "ȡ�����")
        //            {
        //                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
        //                if (dt2.Rows.Count > 0)
        //                {
        //                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
        //                    ZgqClass zgq = new ZgqClass();
        //                    string rtn_msg = "";
        //                    //  zgq.FTP_Delete("", dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
        //                }

        //                if (jcxx.Rows[0]["F_brlb"].ToString().Trim() != "���")
        //                {

        //                    #region  ƽ̨�������

        //                    string xml = "";
        //                    try
        //                    {
        //                        xml = xml + "<?xml version=\"1.0\"?>";
        //                        xml = xml + "<PRPA_IN000003UV04> ";
        //                        xml = xml + "<!-- UUID,����ʵ��Ψһ��-->";
        //                        xml = xml + "<id root=\"2.999.1.96\" extension=\"" + Guid.NewGuid().ToString() + "\"/>";
        //                        xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
        //                        xml = xml + "<interactionId extension=\"PRPA_IN000003UV04\" root=\"2.16.840.1.113883.1.6\"/>";
        //                        xml = xml + "<receiver typeCode=\"RCV\">";
        //                        xml = xml + "<!-- ������д�绰��Ϣ����URL-->";
        //                        xml = xml + "<telecom value=\"\"/>";
        //                        xml = xml + "<device determinerCode=\"INSTANCE\" classCode=\"DEV\">";
        //                        xml = xml + "<id extension=\"HIP\" root=\"2.999.1.97\"/>";
        //                        xml = xml + "<telecom value=\"�豸���\"/>";
        //                        xml = xml + "<softwareName codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\" codeSystem=\"2.999.2.3.2.84\" displayName=\"ҽԺ��Ϣƽ̨\" code=\"HIP\"/>";
        //                        xml = xml + "</device></receiver>";

        //                        xml = xml + "<sender typeCode=\"SND\"><telecom value=\"\"/>";
        //                        xml = xml + "<device determinerCode=\"INSTANCE\" classCode=\"DEV\"><id extension=\"PIS\" root=\"2.999.1.98\"/>";
        //                        xml = xml + "<telecom value=\"�豸���\"/>";
        //                        xml = xml + "<softwareName codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\" codeSystem=\"2.999.2.3.2.84\" displayName=\"������Ϣϵͳ\" code=\"PIS\"/></device></sender>";

        //                        xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
        //                        xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
        //                        xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

        //                        xml = xml + "<!--������ˮ��-->";
        //                        xml = xml + "<flowID>" + blh + "_" + bglx + "_" + bgxh + "</flowID>";
        //                        xml = xml + "<!--�������� - ���������䡢�ĵ��-->";
        //                        xml = xml + "<adviceType>����</adviceType>";
        //                        xml = xml + "<!--HIS�������� ���� סԺ�����-->";
        //                        xml = xml + "<patienttype>" + jcxx.Rows[0]["F_brlb"].ToString() + "</patienttype>";
        //                        xml = xml + "<!-- �ĵ�ע��ʱ�� -->";
        //                        xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</sourceTime>";
        //                        xml = xml + "<!-- ��Դϵͳ���루��� -->";
        //                        xml = xml + "<sourceCode/>";
        //                        xml = xml + "<!-- ��Դϵͳ��� -->";
        //                        xml = xml + "<sourceName>PIS</sourceName>";
        //                        xml = xml + "<!-- �ĵ���ţ���� -->";
        //                        xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
        //                    }
        //                    catch (Exception ee4)
        //                    {
        //                         LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",��������쳣��" + ee4.Message);
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_BZ='��������쳣��" + ee4.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //                        return;
        //                    }


        //                    if (xml.Trim() == "")
        //                    {
        //                         LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",�����������xmlΪ��");
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_BZ='�����������xmlΪ��' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");

        //                        return;
        //                    }

        //                    msgtxt = "";
        //                    try
        //                    {
        //                        if (MQSer.SendMessage(xml, ref msgtxt, "QI1_001", "PIS_ReportExpired"))
        //                        {
        //                            if (debug == "1")
        //                                 LGZGQClass.log.WriteMyLog(xml + "\r\nQI1_001--PIS_ReportExpired");
        //                             LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",������ճɹ���" + msgtxt);
        //                            aa.ExecuteSQL("update T_jcxx_fs set F_BZ='������ճɹ���" + msgtxt + "',F_FSZT='�Ѵ���' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //                            return;
        //                        }
        //                        else
        //                        {
        //                             LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",�������ʧ�ܣ�" + msgtxt);
        //                            aa.ExecuteSQL("update T_jcxx_fs set F_BZ='�������ʧ�ܣ�" + msgtxt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //                            return;
        //                        }
        //                    }
        //                    catch (Exception ee4)
        //                    {
        //                         LGZGQClass.log.WriteMyLog(blh + "_" + bglx + "_" + bgxh + ",��������쳣��" + ee4.Message);
        //                        aa.ExecuteSQL("update T_jcxx_fs set F_BZ='��������쳣��" + ee4.Message + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //                        return;
        //                    }
        //                    #endregion

        //                }
        //                else
        //                    aa.ExecuteSQL("update T_jcxx_fs set F_fszt='�Ѵ���',F_BZ='' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //            }
        //            else
        //                aa.ExecuteSQL("update T_jcxx_fs set F_bz='������,״̬" + bgzt + "' where F_blh='" + blh + "' and F_bglx='" + bglx + "' and F_bgxh='" + bgxh + "' and F_fszt='δ����'");
        //        }

        //    }
        //}

      
        public void C_pdf(string blh, string bgxh, string bglx, DataTable dt_jcxx, bool isToBase64String, ref string PdfToBase64String,string debug)
        {

                string blbh = blh + bglx + bgxh;

                #region  ����pdf
                if (f.ReadString("savetohis", "ispdf", "1").Trim() == "1")
                {
                    string ReprotFile = ""; string jpgname = "";
                    string ML = DateTime.Parse(dt_jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                    string message = "";

                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                  // bool isrtn = zgq.CreatePDFJPG(blh, bglx, bgxh, ref jpgname, "", ZGQ_PDFJPG.type.PDF, ref message);

                    bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, ZgqPDFJPG.Type.PDF, ref message, ref jpgname);
                    if (isrtn)
                    {
                        if (debug == "1")
                            log.WriteMyLog("����PDF�ɹ�");

                        ////�����ƴ�
                        if (File.Exists(jpgname))
                        {
                            if (isToBase64String)
                            {
                                try
                                {
                                    FileStream file = new FileStream(jpgname, FileMode.Open, FileAccess.Read);
                                    Byte[] imgByte = new Byte[file.Length];//��pdfת�� Byte�� ��������   
                                    file.Read(imgByte, 0, imgByte.Length);//�Ѷ����������뻺����   
                                    file.Close();
                                    PdfToBase64String = Convert.ToBase64String(imgByte);
                                    if (debug == "1")
                                        log.WriteMyLog("PDFת�������ƴ��ɹ�");
                                }
                                catch (Exception ee)
                                {
                                    log.WriteMyLog("PDFת�������ƴ�ʧ��:" + ee.Message);
                                }
                            }

                            string pdfpath = "";
                            bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message,3, ref pdfpath);
                          
                            if (ssa == true)
                            {
                                if (debug == "1")
                                     log.WriteMyLog("�ϴ�PDF�ɹ�");

                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                                ZgqClass.BGHJ(blh, "�ϴ�PDF�ɹ�", "���", "�ϴ�PDF�ɹ�:" + pdfpath, "ZGQJK", "�ϴ�PDF");

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLBH='" + blbh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLBH,F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME,f_pdfpath) values('" + blbh + "','" + blh + "','" + bglx + "','" + bgxh + "','" + ML + "\\" + blh + "','" + jpgname + "','" + pdfpath + "')");
                                aa.ExecuteSQL("update T_JCXX_FS set F_bz='����PDF�ɹ�',F_ISJPG='true'  where F_blbh='" + blbh  + "'");
                            }
                            else
                            {
                                if (debug == "1")
                                    log.WriteMyLog("�ϴ�PDFʧ��" + message);
                                ZgqClass.BGHJ(blh, "�ϴ�PDFʧ��", "���", message, "ZGQJK", "�ϴ�PDF");
                                aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_bz='�ϴ�PDFʧ�ܣ�" + message + "'  where F_blbh='" + blbh + "'");
                            }
                        }
                        else
                        {
                            if (debug == "1")
                                log.WriteMyLog("δ�ҵ��ļ�" + jpgname);
                            ZgqClass.BGHJ(blh, "����PDFʧ��", "���", "δ�ҵ��ļ�" + jpgname, "ZGQJK", "����PDF");
                        }


                    }
                    else
                    {
                        if (debug == "1")
                            log.WriteMyLog("����PDFʧ��" + message);
                        ZgqClass.BGHJ(blh, "����PDFʧ��", "���", "����pdfʧ��" + message, "ZGQJK", "����PDF");
                        aa.ExecuteSQL("update T_JCXX_FS set F_ISJPG='false',F_BZ='" + message + "'  where F_BLBH='" + blbh+ "'");

                    }
                    zgq.DelTempFile(blh);

                }
                #endregion
            
        }

        public void TJ_ZtToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
            if (bglx != "cg")
                return;
            if (dt_sqd.Rows.Count <= 0)
            {
               log.WriteMyLog(blh + ",��������޼�¼");
                return;
            }
            string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
            string GUID = "";
            string bgztxml = TJ_ZtMsg(dt_jcxx, dt_sqd, dt_jcxx.Rows[0]["F_SQXH"].ToString(), blh, bglx, bgxh, bgzt, yhmc, yhbh, ref  GUID);
            if (bgztxml.Trim() == "")
            {
               log.WriteMyLog(blh + ",����״̬����xmlΪ��");
                return;
            }
            if (debug == "1")
               log.WriteMyLog("״̬[QI1_073]:" + bgztxml);
            try
            {

                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                MQSer.Url = wsurl;
                string msgtxt = "";
                if (MQSer.SendMessageToMQ(bgztxml, ref msgtxt, "QI1_073", "PIS_State_PE", GUID, "����״̬"))
                {
                    if (debug == "1")
                       log.WriteMyLog(blh + ",״̬[" + bgzt + "]���ͳɹ���" + msgtxt);
                }
                else
                   log.WriteMyLog(blh + ",״̬[" + bgzt + "]����ʧ�ܣ�" + msgtxt);
            }
            catch (Exception ee4)
            {
               log.WriteMyLog(blh + ",״̬[" + bgzt + "]�����쳣��" + ee4.Message);
            }

            return;
        }  
        public void TJ_BgToPt(DataTable dt_jcxx, DataTable dt_sqd, string blh,string bglx,string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
         
            try
            {
                string blbh = blh + bglx + bgxh;
                if (bglx == "cg")
                    blbh = blh;
                if (bglx != "cg")
                    return;
                string url = f.ReadString("savetohis", "blweb", "http://172.16.95.230/pathwebrpt");
                string patientSerialNO = "";
                string jzlb = "1";
                string brlb = dt_jcxx.Rows[0]["F_brlb"].ToString();

                if (dt_sqd.Rows.Count < 1)
                {
                    if (brlb == "סԺ")
                        patientSerialNO = dt_jcxx.Rows[0]["F_ZYH"].ToString().Trim();
                    else
                        patientSerialNO = dt_jcxx.Rows[0]["F_MZH"].ToString().Trim();
                    if (brlb == "סԺ")
                        jzlb = "2";
                    else
                        jzlb = "1";
                }
                else
                {
                    patientSerialNO = dt_sqd.Rows[0]["F_JZH"].ToString().Trim();
                    jzlb = dt_sqd.Rows[0]["F_JZLB"].ToString().Trim();
                }
                DataTable dt_tx = new DataTable();
                try
                {
                    dt_tx = aa.GetDataTable("select * from T_tx where F_BLH='" + blh + "' and F_SFDY='1'", "dt_sqd");
                }
                catch (Exception ex)
                {
                }

                DataTable dt_pdf = new DataTable();
                try
                {
                    dt_pdf = aa.GetDataTable("select * from T_BG_PDF where F_BLBH='" + blbh + "'", "dt_sqd");
                }
                catch (Exception ex)
                {
                   log.WriteMyLog(ex.Message.ToString());
                }
                string filePath = "";
                if (dt_pdf.Rows.Count > 0)
                {
                    filePath = dt_pdf.Rows[0]["F_pdfPath"].ToString().Replace("ftp", "http");
                }
                string GUID = "";
                string rtnxml = TJ_BgMsg(dt_jcxx, dt_tx, dt_jcxx.Rows[0]["F_SQXH"].ToString().Trim(), blh, bglx, bgxh, bgzt, jzlb, yhmc, yhbh, filePath, url, ref GUID);
                if (rtnxml.Trim() == "")
                {
                   log.WriteMyLog(blbh + ",����XMLʧ��:��");
                    aa.ExecuteSQL("update T_jcxx_fs set F_bz='����XMLʧ��:��' where F_blbh='" + blbh + "' and F_BGZT='�����'");
                    return;
                }
                string msgtxt = "";
                try
                {
                    if (debug == "1")
                       log.WriteMyLog("�ش�����[QI1_001-PIS_Report]:" + rtnxml);

                    string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                    ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();
                    MQSer.Url = wsurl;
                    if (MQSer.SendMessageToMQ(rtnxml, ref msgtxt, "QI1_081", "PIS_Report_PE", GUID, "���淢��"))//PIS_Report
                    {
                        if (debug == "1")
                           log.WriteMyLog(blbh + ",���ͳɹ�:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='���ͳɹ�',F_FSZT='�Ѵ���' where F_blbh='" + blbh + "' and F_BGZT='�����'");
                    }
                    else
                    {
                       log.WriteMyLog(blbh + ",����ʧ��:" + msgtxt);
                        aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='����ʧ�ܣ�" + msgtxt + "' where F_blbh='" + blbh + "' and F_BGZT='�����'");
                    }
                }
                catch (Exception ee4)
                {
                   log.WriteMyLog(blbh + ",�����쳣��" + ee4.Message);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_bz='�����쳣��" + ee4.Message + "' where F_blbh='" + blbh + "' and F_BGZT='�����'");

                }
            }
            catch (Exception ee5)
            {
               log.WriteMyLog(ee5.Message);
            }
            return;
        }
        public string TJ_BgMsg(DataTable dt_jcxx, DataTable dt_tx, string sqxh, string blh, string bglx, string bgxh, string bgzt, string jzlb, string yhmc, string yhbh, string filePath, string url, ref string GUID)
        {
            filePath = filePath.ToLower().Replace("ftp", "http");
            string blbh = blh + bglx + bgxh;
            if(bglx=="cg")
                blbh = blh;
            GUID = Guid.NewGuid().ToString();
            //����xml
            string rtnxml = "<?xml version=\"1.0\"?>";
            rtnxml = rtnxml + "<PRPA_IN000003UV01>";
            try
            {
                rtnxml = rtnxml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                rtnxml = rtnxml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                rtnxml = rtnxml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"PRPA_IN000005UV01\"/>";

                rtnxml = rtnxml + "<receiver typeCode=\"RCV\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                rtnxml = rtnxml + "<telecom value=\"�豸���\"/>";
                rtnxml = rtnxml + "<softwareName code=\"HIP\" displayName=\"ҽԺ��Ϣƽ̨\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</receiver>";

                rtnxml = rtnxml + "<sender typeCode=\"SND\">";
                rtnxml = rtnxml + "<telecom value=\"\"/>";
                rtnxml = rtnxml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                rtnxml = rtnxml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                rtnxml = rtnxml + "<telecom value=\"�豸���\"/>";
                rtnxml = rtnxml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                rtnxml = rtnxml + "</device>";
                rtnxml = rtnxml + "</sender>";

                rtnxml = rtnxml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";

                rtnxml = rtnxml + "<authorOrPerformer typeCode=\"AUT\">";
                rtnxml = rtnxml + "<signatureText></signatureText>";
                rtnxml = rtnxml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                rtnxml = rtnxml + "</authorOrPerformer>";
                rtnxml = rtnxml + "<subject typeCode=\"SUBJ\">";

                rtnxml = rtnxml + "<request>";
                ////!--������ˮ��-->
                rtnxml = rtnxml + "<flowID>" + blbh + "</flowID>";
                ////<!--�������� - ���������䡢�ĵ��-->
                rtnxml = rtnxml + "<adviceType>����</adviceType>";
                ////<!--HIS�������� ���� סԺ-->
                rtnxml = rtnxml + "<patienttype>" + dt_jcxx.Rows[0]["F_BRLB"].ToString() + "</patienttype>";
                ////<!-- �ĵ�ע��ʱ�� -->
                rtnxml = rtnxml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                ////<!-- ���ߺ� -->
                rtnxml = rtnxml + "<patientNo>" + dt_jcxx.Rows[0]["F_brbh"].ToString() + "</patientNo>";
                ////<!-- ���߾�����ˮ�� -->
                rtnxml = rtnxml + "<patientSerialNO>" + dt_jcxx.Rows[0]["F_ZY"].ToString() + "</patientSerialNO>";
                ////<!--HIS��ҽ����ˮ��,��ʶһ������-->
                rtnxml = rtnxml + "<adviceID>" + "" + "</adviceID>";
                ////<!--�Ķ�Ӧ�����뵥Ψһ��ʶ�ţ���ʶһ�μ��-->
                rtnxml = rtnxml + "<accessionNO>" + dt_jcxx.Rows[0]["F_sqxh"].ToString() + "</accessionNO>";
                ////<!--����������-->
                rtnxml = rtnxml + "<patientName>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</patientName>";
                ////<!--����ƴ����-->
                rtnxml = rtnxml + "<patientNameSpell></patientNameSpell>";
                ////<!--��������(���磺��19870601��)-->
                rtnxml = rtnxml + "<birthDate></birthDate>";
                ////<!--�Ա�-->
                string xb = dt_jcxx.Rows[0]["F_xb"].ToString();
                if (xb == "��") xb = "1";
                else if (xb == "Ů") xb = "2";
                else xb = "0";
                rtnxml = rtnxml + "<sex>" + xb + "</sex>";
                ////	<!--�����Ŀ����-->

                if (dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim().Contains("^"))
                {
                    rtnxml = rtnxml + "<procedureCode>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[0] + "</procedureCode>";
                    try
                    {
                        rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Split('^')[1] + "</procedureName>";
                    }
                    catch
                    {
                        rtnxml = rtnxml + "<procedureName></procedureName>";
                    }
                }
                else
                {
                    rtnxml = rtnxml + "<procedureCode>" + "" + "</procedureCode>";
                    rtnxml = rtnxml + "<procedureName>" + dt_jcxx.Rows[0]["F_YZXM"].ToString().Trim() + "</procedureName>";
                }
                ////	<!--��鲿λ-->
                rtnxml = rtnxml + "<positionName>" + dt_jcxx.Rows[0]["F_bbmc"].ToString().Split('^')[0] + "</positionName>";
                ////	<!--����豸����-->
                rtnxml = rtnxml + "<modalityName></modalityName>";
                ////	<!--���������CT��MRI��-->

                rtnxml = rtnxml + "<reportType>" + bglx + "</reportType>";
                ////	<!-- ����ҽ������ -->
                rtnxml = rtnxml + "<authorCode></authorCode>";
                ////	<!--����ҽ��-->
               
                    rtnxml = rtnxml + "<authorName>" + dt_jcxx.Rows[0]["F_bgys"].ToString() + "</authorName>";
                    ////	<!--����ʱ��-->
                    rtnxml = rtnxml + "<reportDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_bgrq"].ToString()).ToString("yyyyMMddHHmmss") + "</reportDateTime>";
                    ////	<!--���ҽ��-->
                    rtnxml = rtnxml + "<reportApprover>" + dt_jcxx.Rows[0]["F_shys"].ToString() + "</reportApprover>";
                    ////	<!--���ʱ��-->
                    rtnxml = rtnxml + "<approverDateTime>" + DateTime.Parse(dt_jcxx.Rows[0]["F_spare5"].ToString()).ToString("yyyyMMddHHmmss") + "</approverDateTime>";


                    DataTable TJ_bljc = new DataTable();
                    TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");
                    // �������
                    string Res_char = dt_jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                    //��Ͻ���	Res_con
                    string Res_con = dt_jcxx.Rows[0]["F_blzd"].ToString().Trim();
                    if (TJ_bljc.Rows.Count > 0)
                    {
                        if (dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "���LCT" || dt_jcxx.Rows[0]["F_blk"].ToString().Trim() == "Һ��ϸ��")
                        {
                            Res_char = Res_char + "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n" + "\r\n";

                            Res_char = Res_char + "��Ŀ��" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM1"].ToString().Trim()
                                + "\r\n" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim() + "\r\n" + "\r\n";

                            Res_char = Res_char + "��ԭ�壺" + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW1"].ToString().Trim()
                                + "\r\n" + TJ_bljc.Rows[0]["F_TBS_WSW3"].ToString().Trim() + "\r\n" + TJ_bljc.Rows[0]["F_TBS_BDXM1"].ToString().Trim() + "\r\n" + "\r\n";

                            Res_char = Res_char + "��֢ϸ������" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n" + "\r\n";

                            ///////////���/////////////////////////
                            Res_con = "��ϣ�" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                            if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                Res_con = Res_con + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                        }
                    }
                
                ////	<!--��������-->
                    rtnxml = rtnxml + "<nakedEyeDiagnose></nakedEyeDiagnose>";
                    ////<!--��������-->
                    rtnxml = rtnxml + "<microscopeDiagnose></microscopeDiagnose>";
                    ////	<!--�������-->
                    rtnxml = rtnxml + "<conclusionDiagnose></conclusionDiagnose>";
                    ////	<!--��������-->���
                    rtnxml = rtnxml + "<reportDiagnose><![CDATA[" + Res_char + "]]></reportDiagnose>";

                    ////	<!--��������-->����
                    rtnxml = rtnxml + "<reportContent><![CDATA[" + Res_con + "]]></reportContent>";
                ////	<!-- ҽ���������ұ��� -->
                rtnxml = rtnxml + "<deptCode></deptCode>";
                ////	<!-- ҽ���������� -->
                rtnxml = rtnxml + "<deptName>�����</deptName>";


                ////	<!-- ��Դϵͳ���� -->
                rtnxml = rtnxml + "<sourceCode>PIS</sourceCode>";
                ////	<!-- ��Դϵͳ��� -->
                rtnxml = rtnxml + "<sourceName>PIS</sourceName>";
                rtnxml = rtnxml + "<providerOrganization>42321679-4</providerOrganization>";
                ////	<!-- �ĵ���� -->
                rtnxml = rtnxml + "<indexInSystem>" + blbh + "</indexInSystem> ";
                ////	<!-- �ĵ����ͱ��� -->
                rtnxml = rtnxml + "<typeCode></typeCode>";
                ///	<!-- �ĵ��������� -->
                rtnxml = rtnxml + "<typeCodeName></typeCodeName>";
                ////	<!-- �ĵ����� -->
                rtnxml = rtnxml + "<title></title>";
                rtnxml = rtnxml + "<reportListURL>" + filePath + "</reportListURL>";
                ////	<!--Ӱ�����URL-->
                rtnxml = rtnxml + "<imageList>";
                try
                {
                    for (int j = 0; j < dt_tx.Rows.Count; j++)
                    {
                        rtnxml = rtnxml + "<imageURL>" + url + "/images/" + dt_jcxx.Rows[0]["F_txml"].ToString() + "/" + dt_tx.Rows[j]["F_TXM"] + "</imageURL>";
                    }
                }
                catch
                {
                }
                rtnxml = rtnxml + "</imageList>";
                ////	<!--�����ֶ�1-->";
                rtnxml = rtnxml + "<other1></other1>";
                ////	<!--�����ֶ�2-->
                rtnxml = rtnxml + "<other2></other2>";
                ////	<!--�����ֶ�3-->
                rtnxml = rtnxml + "<other3></other3>";
                ////	<!--������±�־��0-PACS������1-���Ӳ�����ȡ��2-PACS�޸�-->
                rtnxml = rtnxml + "<updateFlag></updateFlag>";

                ////	<!-- ����PDA�ĵ������ƴ� BASE64 -->
                rtnxml = rtnxml + "<body>";
                rtnxml = rtnxml + "<text mediaType=\"application/pdf\" representation=\"base64\">" + "" + "</text>";
                rtnxml = rtnxml + "</body>";
                rtnxml = rtnxml + "</request>";
                rtnxml = rtnxml + "</subject>";
                rtnxml = rtnxml + "</controlActProcess>";
                rtnxml = rtnxml + "</PRPA_IN000003UV01>";

                return rtnxml;
            }
            catch (Exception e2)
            {
               log.WriteMyLog(blbh + ",��������XML�쳣��" + e2.Message);
                return "";
            }
        }
        public string TJ_ZtMsg(DataTable dt_jcxx, DataTable dt_sqd, string sqxh, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, ref string GUID)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<COMT_IN001103UV01 xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ITSVersion=\"XML_1.0\" xmlns=\"urn:hl7-org:v3\" xsi:schemaLocation=\"urn:hl7-org:v3 ../multicacheschemas/COMT_IN001103UV01.xsd\">";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId root=\"2.16.840.1.113883.1.6\" extension=\"COMT_IN001103UV01\"/>";
                xml = xml + "<processingCode code=\"T\"/>";
                xml = xml + "<processingModeCode code=\"I\"/>";
                xml = xml + "<acceptAckCode code=\"AA\"/>";

                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- ������д�绰��Ϣ����URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"�豸���\"/>";
                xml = xml + "<softwareName code=\"HIP\" displayName=\"����ƽ̨����ϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"�豸���\"/>";
                xml = xml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"EVN\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<signatureText></signatureText>";
                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<subject typeCode=\"SUBJ\">";
                xml = xml + "<actGenericStatus classCode=\"DOCCLIN\" moodCode=\"EVN\">";
                xml = xml + "<!--ҵ��ID������ID��-->";
                xml = xml + "<id root=\"2.16.156.10011.1.1\" extension=\"" + blbh + "\"/>";
                xml = xml + "<!--ҵ����� ״̬����-->";

                if (bgzt == "�����")
                    xml = xml + "<code code=\"60\" displayName=\"���淢��\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"ҵ�����ʹ����\"/>";
                else if (bgzt == "��д����")
                    xml = xml + "<code code=\"50\" displayName=\"���\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"ҵ�����ʹ����\"/>";
                else
                    xml = xml + "<code code=\"40\" displayName=\"ִ��\" codeSystem=\"2.999.2.3.2.79\" codeSystemName=\"ҵ�����ʹ����\"/>";

                xml = xml + "<!--ҵ��״̬ ȫΪcompleted-->";
                xml = xml + "<statusCode code=\"completed\"/>";
                xml = xml + "<!--ҵ���ڼ�-->";
                xml = xml + "<effectiveTime xsi:type=\"IVL_TS\">";
                xml = xml + "<!--ִ�п�ʼʱ��-->";
                xml = xml + "<low value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<!--ִ�н���ʱ��-->";
                xml = xml + "<high value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "</effectiveTime>";
                xml = xml + "<!--ִ����0..*-->";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\">";
                xml = xml + "<assignedEntity classCode=\"ASSIGNED\">";
                xml = xml + "<!--ҽ����ԱID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.4\" extension=\"" + yhbh + "\"/>";
                xml = xml + "<assignedPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<name>" + yhmc + "</name>";
                xml = xml + "</assignedPerson>";
                xml = xml + "<representedOrganization classCode=\"ORG\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!--ҽ���������������ң�ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.26\" extension=\"1035\"/>";
                xml = xml + "<name>�����</name>";
                xml = xml + "</representedOrganization>";
                xml = xml + "</assignedEntity>";
                xml = xml + "</authorOrPerformer>";
                xml = xml + "<!--��ִ�����뵥��ҽ��-->";
                xml = xml + "<inFulfillmentOf typeCode=\"FLFS\">";
                xml = xml + "<actIntent classCode=\"ACT\" moodCode=\"RQO\">";
                xml = xml + "<!--�������뵥���-->";
                xml = xml + "<id root=\"2.16.156.10011.1.24\" extension=\"" + sqxh + "\"/>";
                xml = xml + "<!--ҽ��ID-->";
                xml = xml + "<id root=\"2.16.156.10011.1.28\" extension=\"" + "" + "\"/>";
                xml = xml + "</actIntent>";
                xml = xml + "</inFulfillmentOf>";
                xml = xml + "<componentOf typeCode=\"COMP\" xsi:nil=\"false\">";
                xml = xml + "<encounter classCode=\"ENC\" moodCode=\"EVN\">";
                xml = xml + "<!--��(��)����ˮ��-->";
                xml = xml + "<id root=\"2.999.1.91\" extension=\"" + dt_sqd.Rows[0]["F_MZLSH"].ToString() + "\"/>";
                xml = xml + "<!--סԺ��ˮ�� -->";
                xml = xml + "<id root=\"2.999.1.42\" extension=\"" + dt_sqd.Rows[0]["F_ZYLSH"].ToString() + "\"/>";
                xml = xml + "<code code=\"" + dt_sqd.Rows[0]["F_JZLB"].ToString() + "\" codeSystem=\"2.16.156.10011.2.3.1.271\" codeSystemName=\"�������ʹ����\" displayName=\"" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "\"></code>";
                xml = xml + "<statusCode/>";
                xml = xml + "<subject typeCode=\"SBJ\">";
                xml = xml + "<patient classCode=\"PAT\">";
                xml = xml + "<!--ƽ̨ע��Ļ���ID -->";
                xml = xml + "<id root=\"2.999.1.37\" extension=\"" + dt_sqd.Rows[0]["F_EMPIID"].ToString() + "\"/>";
                xml = xml + "<!--����ϵͳ�Ļ���ID -->";
                xml = xml + "<id root=\"2.999.1.41\" extension=\"" + dt_sqd.Rows[0]["F_patientid"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.999.1.40\" extension=\"" + dt_sqd.Rows[0]["F_JZKH"].ToString() + "\"/>";
                xml = xml + "<id root=\"2.16.156.10011.1.13\" extension=\"" + dt_sqd.Rows[0]["F_ZLH"].ToString() + "\"/>";
                xml = xml + "<statusCode/>";
                xml = xml + "<patientPerson classCode=\"PSN\" determinerCode=\"INSTANCE\">";
                xml = xml + "<!-- ��������  -->";
                xml = xml + "<name>" + dt_jcxx.Rows[0]["F_xm"].ToString() + "</name>";
                xml = xml + "</patientPerson>";
                xml = xml + "</patient>";
                xml = xml + "</subject>";
                xml = xml + "</encounter>";
                xml = xml + "</componentOf>";
                xml = xml + "</actGenericStatus>";
                xml = xml + "</subject>";
                xml = xml + "</controlActProcess>";
                xml = xml + "</COMT_IN001103UV01>";
                return xml;
            }
            catch (Exception ee)
            {
               log.WriteMyLog("����״̬����XML�쳣��" + ee.Message);
                return "";
            }
        }
        public void TJ_BgHSToPt(DataTable dt_jcxx, string blh, string bglx, string bgxh, string bgzt, string yhmc, string yhbh, string debug)
        {
            string blbh = blh + bglx + bgxh;
            if (bglx == "cg")
                blbh = blh;
            string xml = "";
            string GUID = Guid.NewGuid().ToString();
            try
            {
                xml = xml + "<?xml version=\"1.0\"?>";
                xml = xml + "<PRPA_IN000003UV04> ";
                xml = xml + "<!-- UUID,����ʵ��Ψһ��-->";
                xml = xml + "<id root=\"2.999.1.96\" extension=\"" + GUID + "\"/>";
                xml = xml + "<creationTime value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
                xml = xml + "<interactionId extension=\"PRPA_IN000003UV04\" root=\"2.16.840.1.113883.1.6\"/>";


                xml = xml + "<receiver typeCode=\"RCV\">";
                xml = xml + "<!-- ������д�绰��Ϣ����URL-->";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.97\" extension=\"HIP\"/>";
                xml = xml + "<telecom value=\"�豸���\"/>";
                xml = xml + "<softwareName code=\"HIP\" displayName=\"����ƽ̨����ϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                xml = xml + "</device>";
                xml = xml + "</receiver>";

                xml = xml + "<sender typeCode=\"SND\">";
                xml = xml + "<telecom value=\"\"/>";
                xml = xml + "<device classCode=\"DEV\" determinerCode=\"INSTANCE\">";
                xml = xml + "<id root=\"2.999.1.98\" extension=\"PIS\"/>";
                xml = xml + "<telecom value=\"�豸���\"/>";
                xml = xml + "<softwareName code=\"PIS\" displayName=\"������Ϣϵͳ\" codeSystem=\"2.999.2.3.2.84\" codeSystemName=\"ҽԺ��Ϣƽ̨ϵͳ������\"/>";
                xml = xml + "</device>";
                xml = xml + "</sender>";

                xml = xml + "<controlActProcess classCode=\"CACT\" moodCode=\"APT\">";
                xml = xml + "<authorOrPerformer typeCode=\"AUT\"><signatureText></signatureText>";
                xml = xml + "<assignedDevice classCode=\"ASSIGNED\"/></authorOrPerformer> <subject typeCode=\"SUBJ\"><request>";

                xml = xml + "<!--������ˮ��-->";
                xml = xml + "<flowID>" + blbh + "</flowID>";
                xml = xml + "<!--�������� - ���������䡢�ĵ��-->";
                xml = xml + "<adviceType>����</adviceType>";
                xml = xml + "<!--HIS�������� ���� סԺ�����-->";
                xml = xml + "<patienttype>" + dt_jcxx.Rows[0]["F_brlb"].ToString() + "</patienttype>";
                xml = xml + "<!-- �ĵ�ע��ʱ�� -->";
                xml = xml + "<sourceTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</sourceTime>";
                xml = xml + "<!-- ��Դϵͳ���루��� -->";
                xml = xml + "<sourceCode/>";
                xml = xml + "<!-- ��Դϵͳ��� -->";
                xml = xml + "<sourceName>PIS</sourceName>";
                xml = xml + "<!-- �ĵ���ţ���� -->";
                xml = xml + "<indexInSystem>" + blbh + "</indexInSystem>";

                xml = xml + "</request></subject></controlActProcess></PRPA_IN000003UV04>";
            }
            catch (Exception ee4)
            {
               log.WriteMyLog(blbh + ",�����ٻ�XML�쳣��" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='�ٻ�XML�쳣��" + ee4.Message + "' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
                return;
            }
            if (xml.Trim() == "")
            {
               log.WriteMyLog(blbh + ",�����ٻ�����xmlΪ��");
                aa.ExecuteSQL("update T_jcxx_fs set F_BZ='�����ٻ�xmlΪ��' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
                return;
            }

            string msgtxt = "";
            try
            {
                if (debug == "1")
                   log.WriteMyLog("�����ٻأ�[QI1_081--PIS_ReportExpired_PE]" + xml);

                string wsurl = f.ReadString("savetohis", "wsurl", "http://172.16.95.230/MQService/MQService.asmx");
                ZszlMQ_ZGQ.MQService MQSer = new PathHISZGQJK.ZszlMQ_ZGQ.MQService();

                MQSer.Url = wsurl;


                if (MQSer.SendMessageToMQ(xml, ref msgtxt, "QI1_081", "PIS_ReportExpired_PE", GUID, "�������"))
                {
                    if (debug == "1")
                    {
                       log.WriteMyLog(blbh + ",�����ٻسɹ���" + msgtxt);
                    }
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='�����ٻسɹ�:" + msgtxt + "',F_FSZT='�Ѵ���' where F_blbh='" + blbh + "' and F_fszt='δ����' and F_BGZT='ȡ�����'");
                    return;
                }
                else
                {
                   log.WriteMyLog(blbh + ",�����ٻ�ʧ�ܣ�" + msgtxt);
                    aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='�����ٻ�ʧ��:" + msgtxt + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='ȡ�����'");
                    return;
                }
            }
            catch (Exception ee4)
            {
               log.WriteMyLog(blbh + ",�����ٻ��쳣��" + ee4.Message);
                aa.ExecuteSQL("update T_jcxx_fs set GUID='" + GUID + "',F_BZ='�����ٻ��쳣:" + ee4.Message + "' where F_blbh='" + blbh + "'  and F_fszt='δ����' and F_BGZT='ȡ�����'");
                return;
            }
        }
     
    }
}
