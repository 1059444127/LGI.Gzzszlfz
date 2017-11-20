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

namespace LGHISJKZGQ
{
    class CSZXYY
    {
        //长沙中心医院
        //webservices
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
       
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

            string pathWEB = f.ReadString(Sslbx, "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
            string Ipstr = "127.0.0.1";
            try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
            catch { Ipstr = "127.0.0.1"; }

            LGHISJKZGQ.cszxyyWEB.NYwclService cszxyy = new LGHISJKZGQ.cszxyyWEB.NYwclService();
            if (pathWEB != "")
                cszxyy.Url = pathWEB;

            if (Sslbx != "")
            {
                string rtn = "";

                #region 住院号
                if (Sslbx == "住院号")
                {
                 
                    string sqlstr = "select *,ZYBAH,XM,(case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,CWH,"
                    + "SFZH,HKDZ,LXDH,(select DMMC from SYS_FYBZ_FYLY where DM=ZYJS_HZJBXX.FB) as FB,USERID  FROM ZYJS_HZJBXX WHERE ZYBAH='" + Ssbz.Trim() + "'";

                    try
                    {
                         rtn = cszxyy.bselect(3626, "111", Ipstr, "his_iinhos", sqlstr);
                         cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "")
                    {
                        MessageBox.Show("没有该住院号信息！请核实");
                        return "0";
                    }
                    
                    try
                    {
                        DataSet ds1 = new DataSet();
                       // XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        if (Debug == "1")
                            MessageBox.Show(rtn);

                        string rtnstring="";
                        if (jcblh("select F_BLH,F_XM,F_SDRQ from T_JCXX  where F_XM='" + ds1.Tables[0].Rows[0]["XM"].ToString() + "' and F_ZYH='" + ds1.Tables[0].Rows[0]["ZYBAH"].ToString() + "' and F_SDRQ>='" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "'  and F_BY1 like '%冰%'", ref rtnstring))
                        {
                            if (MessageBox.Show("此病人可能已经登记冰冻，请确认是否继续登记！！！\r\n" + rtnstring, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return "0";
                            }
                        }

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 +ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYBAH"].ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NL"].ToString() + ds1.Tables[0].Rows[0]["NLDW"].ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[0]["SFZH"].ToString() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + ds1.Tables[0].Rows[0]["FB"].ToString() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                }
                #endregion
               
                #region 门诊号
                if (Sslbx == "门诊号")
                {
                    string sqlstr = "select MZGH_BRJZ_GH.USERID as userid,MZGH_BRJZ_GH.MZBAH as mzbah,MZGH_BRJZ_GH.GHLSH as ghlsh,MZGH_BRJZ_GH.XM as xm,MZGH_BRJZ_GH.XB as xb,"
                        + "(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then CONVERT(VARCHAR(10),"
                        + "DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,"
                        + "'' as SFZH,MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh from  MZGH_BRJZ_GH where GHLSH='" + Ssbz + "'";
                 
                   
                    try
                    {
                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);
                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！");
                        return "0";
                    }
                    if (rtn == "")
                    {
                        MessageBox.Show("没有该门诊号信息！请核实");
                        return "0";
                    }
              

                    try
                    {
                        DataSet ds1 = new DataSet();
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        if (Debug == "1") 
                            MessageBox.Show(rtn);

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["mzbah"].ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["ghlsh"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["nlmc"].ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["zz"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[0]["SFZH"].ToString() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                }
                #endregion
                

                #region 住院申请号

                if (Sslbx == "住院申请号")
                {
                    string sqgy = "";
                    try
                    {
                        Double.Parse(Ssbz);
                    }
                    catch(Exception ee)
                    {
                        MessageBox.Show("住院申请号有误,只能是12位数字"+ee.Message.ToString());
                        return "0";
                    }
                    if (Ssbz.Length > 12)
                        sqgy = Ssbz.Trim().Substring(0, 12);
                    else
                        sqgy = Ssbz;
                  //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
                    string sqlstr = "SELECT ZYJS_HZJBXX.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
         + "EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
         + "ZYJS_HZJBXX.XM,(case ZYJS_HZJBXX.XB  when '2' then '女' else '男' end)  as XB,ZYJS_HZJBXX.CWH,ZYJS_HZJBXX.NL,(case ZYJS_HZJBXX.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,ZYJS_HZJBXX.HKDZ,ZYJS_HZJBXX.LXDH,"
         + "(select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,ZYJS_HZJBXX  "
         + "WHERE      EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and ZYJS_HZJBXX.ZYBAH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ID='" + sqgy.Trim() + "'";
                 
        
             
                    try
                    {
                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_iinhos", sqlstr);
                       
                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "" || rtn == null)
                    {
                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
                        return "0";
                    }
               
                    try
                    {
                        DataSet ds1 = new DataSet();
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        if (Debug == "1") 
                            MessageBox.Show(rtn);

                        string rtnstring = "";
                        if (jcblh("select F_BLH,F_XM,F_SDRQ from T_JCXX  where F_XM='" + ds1.Tables[0].Rows[0]["XM"].ToString() + "' and F_ZYH='" + ds1.Tables[0].Rows[0]["ZYH"].ToString() + "' and F_SDRQ>='" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "'  and F_BY1 like '%冰%'", ref rtnstring))
                        {
                            if (MessageBox.Show("此病人可能已经登记冰冻，请确认是否继续登记！！！\r\n" + rtnstring, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                return "0";
                            }
                        }

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 +ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NL"].ToString()+"岁" + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 +ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        string bbmc="";
                        if(ds1.Tables[0].Rows[0]["bw1"].ToString()!="")
                            bbmc=bbmc+ds1.Tables[0].Rows[0]["bw1"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
                            bbmc = bbmc +"("+ ds1.Tables[0].Rows[0]["bb1"].ToString()+")";

                        if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

                        if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

                        if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

                        xml = xml + "标本名称=" + (char)34 +bbmc.Trim() +(char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                        xml = xml + "/>";
                        // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
                          string aa = "";
                        if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
                            aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
                        if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
                            aa = aa + "  手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();
                        xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        return xml;

                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                }
                #endregion
              
                #region 门诊申请号
                if (Sslbx == "门诊申请号")
                {
                    string sqgy = "";
                    try
                    {
                        Double.Parse(Ssbz);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("门诊申请号有误,只能是12位数字" + ee.Message.ToString());
                        return "0";
                    }
                    if (Ssbz.Length > 12)
                        sqgy = Ssbz.Trim().Substring(0, 12);
                    else
                        sqgy = Ssbz;
                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
                    string sqlstr = "SELECT MZGH_BRJZ_GH.MZBAH as mzbah,MZGH_BRJZ_GH.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
       +" EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
       +" (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
        +" (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
       +" (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
         +" (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
      +" (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
      +"  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
       +"  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
       +" (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
        +"(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
        +"(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
        +" MZGH_BRJZ_GH.XM,MZGH_BRJZ_GH.XB,'' AS CWH,"
		+"(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) when '2' then CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) end) AS NLMC,"
		+"(case MZGH_BRJZ_GH.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH,"
        + " (select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,MZGH_BRJZ_GH "
        + " WHERE    EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and MZGH_BRJZ_GH.GHLSH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ID='" + sqgy.Trim() + "'";

                  
                    try
                    {
                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);
                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "" || rtn == null)
                    {
                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
                        return "0";
                    }

                    try
                    {
                        DataSet ds1 = new DataSet();
                       // XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        if (Debug == "1") MessageBox.Show(rtn);

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["mzbah"].ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + "岁" + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        string bbmc = "";
                        if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
                            bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

                        if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

                        if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

                        if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
                        if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

                        xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                        xml = xml + "/>";
                        // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
                        string aa = "";
                        if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
                            aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString()+"     ";
                        if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
                            aa =aa+"手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();

                        xml = xml + "<临床病史><![CDATA[" +aa.Trim() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
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
                #endregion
                

                #region 住院号(申请)

                if (Sslbx == "住院号(申请)")
                {

                    string ordersql = f.ReadString("住院号(申请)", "ordersql", ""); //获取sz.ini中设置的ordersql,排序等
                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
                    string sqlstr = "SELECT ZYJS_HZJBXX.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
         + "EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
         + "ZYJS_HZJBXX.XM,(case ZYJS_HZJBXX.XB  when '2' then '女' else '男' end)  as XB,ZYJS_HZJBXX.CWH,ZYJS_HZJBXX.NL,(case ZYJS_HZJBXX.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,ZYJS_HZJBXX.HKDZ,ZYJS_HZJBXX.LXDH,"
         + "(select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,ZYJS_HZJBXX  "
         + "WHERE      EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and ZYJS_HZJBXX.ZYBAH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ZYH='" + Ssbz.Trim() + "'";

                    sqlstr = sqlstr + "  "+ordersql;
                    try
                    {
                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_iinhos", sqlstr);
                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "" || rtn == null)
                    {
                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
                        return "0";
                    }
                   
                    try
                    {
                        DataSet ds1 = new DataSet();
                        // XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        if (Debug == "1")
                        {
                            MessageBox.Show(rtn);
                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());
                        }
                      
                        if (ds1.Tables[0].Rows.Count >1)
                        {

                            Frm_cszxyy cs = new Frm_cszxyy(ds1, 1);
                            cs.ShowDialog();
                            string index = cs.A;

                           
                            DataTable dtNew = new DataTable();
                            DataView view = new DataView();
                            view.Table = ds1.Tables[0];
                            view.RowFilter = "ID = '" + index + "'";
                            dtNew = view.ToTable();

                            string rtnstring = "";
                            if (jcblh("select F_BLH,F_XM,F_SDRQ from T_JCXX  where F_XM='" + dtNew.Rows[0]["XM"].ToString() + "' and F_ZYH='" + dtNew.Rows[0]["ZYH"].ToString() + "' and F_SDRQ>='" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "'  and F_BY1 like '%冰%'", ref rtnstring))
                            {
                                if (MessageBox.Show("此病人可能已经登记冰冻，请确认是否继续登记！！！\r\n" + rtnstring, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                                {
                                    return "0";
                                }
                            }


                       
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + dtNew.Rows[0]["USERID"].ToString() + (char)34 + " "; 
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " "; 
                            xml = xml + "申请序号=" + (char)34 + dtNew.Rows[0]["ID"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + dtNew.Rows[0]["ZYH"].ToString() + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + dtNew.Rows[0]["XM"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + dtNew.Rows[0]["XB"].ToString() + (char)34 + " ";
                            xml = xml + "年龄=" + (char)34 + dtNew.Rows[0]["NL"].ToString() + "岁" + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + dtNew.Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + dtNew.Rows[0]["LXDH"].ToString() + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + dtNew.Rows[0]["CWH"].ToString() + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + dtNew.Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + dtNew.Rows[0]["YSDM"].ToString() + (char)34 + " ";

                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            string bbmc = "";
                            if (dtNew.Rows[0]["bw1"].ToString() != "")
                                bbmc = bbmc + dtNew.Rows[0]["bw1"].ToString();
                            if (dtNew.Rows[0]["bb1"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb1"].ToString() + ")";

                            if (dtNew.Rows[0]["bw2"].ToString() != "")
                                bbmc = bbmc + "," + dtNew.Rows[0]["bw2"].ToString();
                            if (dtNew.Rows[0]["bb2"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb2"].ToString() + ")";

                            if (dtNew.Rows[0]["bw3"].ToString() != "")
                                bbmc = bbmc + "," + dtNew.Rows[0]["bw3"].ToString();
                            if (dtNew.Rows[0]["bb3"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb3"].ToString() + ")";

                            if (dtNew.Rows[0]["bw4"].ToString() != "")
                                bbmc = bbmc + "," + dtNew.Rows[0]["bw4"].ToString();
                            if (dtNew.Rows[0]["bb4"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb4"].ToString() + ")";

                            xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                            xml = xml + "/>";
                            // "病史摘要：" + dtNew.Rows[0]["BSJY"].ToString()  +
                            string aa = "";
                            if (dtNew.Rows[0]["LCSJ"].ToString().Trim() != "")
                                aa = "临床所见：" + dtNew.Rows[0]["LCSJ"].ToString() + "     ";
                            if (dtNew.Rows[0]["sssj"].ToString().Trim() != "")
                                aa = aa + "  手术所见：" + dtNew.Rows[0]["sssj"].ToString();
                            xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + dtNew.Rows[0]["LCZD"].ToString()+ "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            return xml;




                        }
                        else
                        {

                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                            xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NL"].ToString() + "岁" + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            string bbmc = "";
                            if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
                                bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

                            if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

                            if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

                            if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

                            xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
                            xml = xml + "/>";
                            // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
                            string aa = "";
                            if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
                                aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
                            if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
                                aa = aa + "  手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();
                            xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            return xml;
                        }
                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.ToString());
                        return "0";
                    }
                }
                #endregion
               
                #region  门诊号(申请)
                if (Sslbx == "门诊号(申请)")
                {

                    string ordersql = f.ReadString("门诊号(申请)", "ordersql", ""); 
                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
                    string sqlstr = "SELECT MZGH_BRJZ_GH.MZBAH as mzbah,MZGH_BRJZ_GH.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
       + " EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
       + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
        + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
       + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
         + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
      + " (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
      + "  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
       + "  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
       + " (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
        + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
        + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
        + " MZGH_BRJZ_GH.XM,MZGH_BRJZ_GH.XB,'' AS CWH,"
        + "(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) when '2' then CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) end) AS NLMC,"
        + "(case MZGH_BRJZ_GH.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH,"
        + " (select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,MZGH_BRJZ_GH "
        + " WHERE    EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and MZGH_BRJZ_GH.GHLSH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ZYH='" + Ssbz.Trim() + "'";
                    sqlstr = sqlstr +"  "+ ordersql;
                    try
                    {
                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);
                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "" || rtn == null)
                    {
                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
                        return "0";
                    }

                    try
                    {
                        DataSet ds1 = new DataSet();
                        // XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);

                        if (Debug == "1")
                        {
                            MessageBox.Show(rtn);
                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());
                        }
                        if (ds1.Tables[0].Rows.Count >1)
                        {

                            Frm_cszxyy cs = new Frm_cszxyy(ds1, 1);
                            cs.ShowDialog();
                            string index = cs.A;


             

                            DataTable dtNew = new DataTable();
                            DataView view = new DataView();
                            view.Table = ds1.Tables[0];
                            view.RowFilter = "ID = '" + index + "'";
                            dtNew = view.ToTable();


                            string xml2 = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml2 = xml2 + "<LOGENE>";
                            xml2 = xml2 + "<row ";
                            xml2 = xml2 + "病人编号=" + (char)34 + dtNew.Rows[0]["USERID"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "就诊ID=" + (char)34 + dtNew.Rows[0]["mzbah"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "申请序号=" + (char)34 + dtNew.Rows[0]["ID"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "门诊号=" + (char)34 + dtNew.Rows[0]["ZYH"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "姓名=" + (char)34 + dtNew.Rows[0]["XM"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "性别=" + (char)34 + dtNew.Rows[0]["XB"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "年龄=" + (char)34 + dtNew.Rows[0]["NLMC"].ToString() + "岁" + (char)34 + " ";

                            xml2 = xml2 + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "地址=" + (char)34 + dtNew.Rows[0]["ZZ"].ToString() + (char)34 + "   ";
                            xml2 = xml2 + "电话=" + (char)34 + dtNew.Rows[0]["LXDH"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "床号=" + (char)34 + dtNew.Rows[0]["CWH"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml2 = xml2 + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "送检科室=" + (char)34 + dtNew.Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "送检医生=" + (char)34 + dtNew.Rows[0]["YSDM"].ToString() + (char)34 + " ";

                            xml2 = xml2 + "收费=" + (char)34 + "" + (char)34 + " ";
                            string bbmc = "";
                            if (dtNew.Rows[0]["bw1"].ToString() != "")
                                bbmc = bbmc + dtNew.Rows[0]["bw1"].ToString();
                            if (dtNew.Rows[0]["bb1"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb1"].ToString() + ")";

                            if (dtNew.Rows[0]["bw2"].ToString() != "")
                                bbmc = bbmc + "," + dtNew.Rows[0]["bw2"].ToString();
                            if (dtNew.Rows[0]["bb2"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb2"].ToString() + ")";

                            if (dtNew.Rows[0]["bw3"].ToString() != "")
                                bbmc = bbmc + "," + dtNew.Rows[0]["bw3"].ToString();
                            if (dtNew.Rows[0]["bb3"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb3"].ToString() + ")";

                            if (dtNew.Rows[0]["bw4"].ToString() != "")
                                bbmc = bbmc + "," + dtNew.Rows[0]["bw4"].ToString();
                            if (dtNew.Rows[0]["bb4"].ToString() != "")
                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb4"].ToString() + ")";

                            xml2 = xml2 + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                            xml2 = xml2 + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml2 = xml2 + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml2 = xml2 + "备用1=" + (char)34 + (char)34 + " ";
                            xml2 = xml2 + "备用2=" + (char)34 + (char)34 + " ";
                            xml2 = xml2 + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                            xml2 = xml2 + "/>";
                            // "病史摘要：" + ds1.Rows[0]["BSJY"].ToString()  +
                            string aa = "";
                            if (dtNew.Rows[0]["LCSJ"].ToString().Trim() != "")
                                aa = "临床所见：" + dtNew.Rows[0]["LCSJ"].ToString() + "     ";
                            if (dtNew.Rows[0]["sssj"].ToString().Trim() != "")
                                aa = aa + "手术所见：" + dtNew.Rows[0]["sssj"].ToString();

                            xml2 = xml2 + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
                            xml2 = xml2 + "<临床诊断><![CDATA[" + dtNew.Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
                            xml2 = xml2 + "</LOGENE>";

                            return xml2;



                        }
                        else
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["mzbah"].ToString() + (char)34 + " "; 
                            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                            xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + "岁" + (char)34 + " ";

                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            string bbmc = "";
                            if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
                                bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

                            if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

                            if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

                            if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
                            if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

                            xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
                            xml = xml + "/>";
                            // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
                            string aa = "";
                            if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
                                aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
                            if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
                                aa = aa + "手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();

                            xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            return xml;
                        }
                    }
                    catch (Exception ee)
                    {
                        if (Debug == "1")
                            MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                }
                #endregion
               

                #region  体检

                if (Sslbx == "体检号")
                {      int ts=-3;
                string tianshu = f.ReadString("体检号", "tianshu", ""); //获取sz.ini中设置的获取体检信息的天数
                if (tianshu.Trim() != "")
                    ts = int.Parse(tianshu);


                    string sqgy = "";
                    try
                    {
                        Double.Parse(Ssbz);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("体检号有误,只能是12位数字" + ee.ToString());
                        return "0";
                    }
                    if (Ssbz.Length > 12)
                        sqgy = Ssbz.Trim().Substring(0, 12);
                    else
                        sqgy = Ssbz;
                  
                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    string sqlstr = @"select  JKDA_TJJL_TJXM.ID,MZGH_BRJZ_GH.MZBAH,MZGH_BRJZ_GH.USERID as userid,MZGH_BRJZ_GH.GHLSH,
//MZGH_BRJZ_GH.XM as xm,MZGH_BRJZ_GH.XB as xb,
//(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),
//DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then
// CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,'' as SFZH,
//                    MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh ,JKDA_TJJL.TJSJ,JKDA_RYXX.DWMC 
//   FROM JKDA_RYXX,   
//         JKDA_TJJL,   
//         JKDA_TJJL_TJXM,   
//         MZGH_BRJZ_GH  
//   WHERE ( JKDA_TJJL.JKDAH = JKDA_RYXX.JKDAH ) and  
//         ( JKDA_TJJL_TJXM.GYID = JKDA_TJJL.ID ) and  
//         ( MZGH_BRJZ_GH.GHLSH = JKDA_TJJL.GHLSH ) and JKDA_TJJL_TJXM.ZXKS=51" ;

                    string sqlstr = @"select  JKDA_TJJL_TJXM.ID,
(SELECT DMMC FROM SYS_FYBZ_YZML WHERE SYS_FYBZ_YZML.DM=JKDA_TJJL_TJXM.XMID) AS XMMC,
MZGH_BRJZ_GH.MZBAH,
MZGH_BRJZ_GH.USERID as userid,
MZGH_BRJZ_GH.GHLSH,
MZGH_BRJZ_GH.XM as xm,
MZGH_BRJZ_GH.XB as xb,
(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),
DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then
 CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,'' as SFZH,
                    MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh ,JKDA_TJJL.TJSJ,JKDA_RYXX.DWMC 
   FROM JKDA_RYXX,   
         JKDA_TJJL,   
         JKDA_TJJL_TJXM,   
         MZGH_BRJZ_GH  
   WHERE ( JKDA_TJJL.JKDAH = JKDA_RYXX.JKDAH ) and  
         ( JKDA_TJJL_TJXM.GYID = JKDA_TJJL.ID ) and  
         ( MZGH_BRJZ_GH.GHLSH = JKDA_TJJL.GHLSH ) and JKDA_TJJL_TJXM.ZXKS=51";



                    sqlstr = sqlstr + "  and JKDA_RYXX.JKDAH='" + sqgy + "'  order by  JKDA_TJJL.TJSJ  desc ";
                  
                 
                    try
                    {
                         rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);

                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "" || rtn == null)
                    {
                        MessageBox.Show("没有该体检信息，请核实体检号是否正确！");
                        return "0";
                    }
                    
                    try
                    {
                        string index = "";

                        DataSet ds1 = new DataSet();
                        // XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        //---------------------------------------
                        if (Debug == "1")
                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());
                      
                        if(ds1.Tables[0].Rows.Count>1)
                        {
                            Frm_cszxyy cs = new Frm_cszxyy(ds1, 3);
                            cs.ShowDialog();
                            index = cs.A;
                         

                            DataTable dtNew = new DataTable();
                            DataView view = new DataView();
                            view.Table = ds1.Tables[0];
                            view.RowFilter = "ID = '" + index + "'";
                            dtNew = view.ToTable();

                            string xml2 = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml2 = xml2 + "<LOGENE>";
                            xml2 = xml2 + "<row ";
                            xml2 = xml2 + "病人编号=" + (char)34 + dtNew.Rows[0]["userid"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "就诊ID=" + (char)34 + dtNew.Rows[0]["MZBAH"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "申请序号=" + (char)34 + dtNew.Rows[0]["ID"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "门诊号=" + (char)34 + dtNew.Rows[0]["GHLSH"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "住院号=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "姓名=" + (char)34 + dtNew.Rows[0]["XM"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "性别=" + (char)34 + dtNew.Rows[0]["XB"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "年龄=" + (char)34 + dtNew.Rows[0]["NLMC"].ToString() + (char)34 + " ";

                            xml2 = xml2 + "婚姻=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "地址=" + (char)34 + dtNew.Rows[0]["ZZ"].ToString() + (char)34 + "   ";
                            xml2 = xml2 + "电话=" + (char)34 + dtNew.Rows[0]["LXDH"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "床号=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "身份证号=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "民族=" + (char)34 + " " + (char)34 + " ";
                            xml2 = xml2 + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
                            xml2 = xml2 + "送检医生=" + (char)34 + "" + (char)34 + " ";

                            xml2 = xml2 + "收费=" + (char)34 + "" + (char)34 + " ";


                            xml2 = xml2 + "标本名称=" + (char)34 +  ""+ (char)34 + " ";
                            xml2 = xml2 + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml2 = xml2 + "医嘱项目=" + (char)34 +dtNew.Rows[0]["XMMC"].ToString() +(char)34 + " ";
                            xml2 = xml2 + "备用1=" + (char)34 + dtNew.Rows[0]["DWMC"].ToString() + (char)34 + " ";
                            xml2 = xml2 + "备用2=" + (char)34 + (char)34 + " ";
                            xml2 = xml2 + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml2 = xml2 + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
                            xml2 = xml2 + "/>";

                            xml2 = xml2 + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            xml2 = xml2 + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml2 = xml2 + "</LOGENE>";

                            return xml2;



                        }

                      //-----------------------------------------
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["userid"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["MZBAH"].ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 +ds1.Tables[0].Rows[0]["GHLSH"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";


                        xml = xml + "标本名称=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + ds1.Tables[0].Rows[0]["XMMC"].ToString() +(char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + ds1.Tables[0].Rows[0]["DWMC"].ToString() + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
                        xml = xml + "/>";
                    
                        xml = xml + "<临床病史><![CDATA[" + ""+ "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
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
                //------------------------------------------------------------------------
                #endregion

                #region   体检申请号

                if (Sslbx == "体检申请号")
                {
 


                    string sqgy = "";
                    try
                    {
                        Double.Parse(Ssbz);
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("体检申请号有误,只能是12位数字" + ee.ToString());
                        return "0";
                    }
                    if (Ssbz.Length > 12)
                        sqgy = Ssbz.Trim().Substring(0, 12);
                    else
                        sqgy = Ssbz;

                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
                    string sqlstr = @"select  JKDA_TJJL_TJXM.ID,MZGH_BRJZ_GH.MZBAH,MZGH_BRJZ_GH.USERID as userid,MZGH_BRJZ_GH.GHLSH,
MZGH_BRJZ_GH.XM as xm,MZGH_BRJZ_GH.XB as xb,
(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),
DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then
 CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,'' as SFZH,
                    MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh ,JKDA_TJJL.TJSJ,JKDA_RYXX.DWMC 
   FROM JKDA_RYXX,   
         JKDA_TJJL,   
         JKDA_TJJL_TJXM,   
         MZGH_BRJZ_GH  
   WHERE ( JKDA_TJJL.JKDAH = JKDA_RYXX.JKDAH ) and  
         ( JKDA_TJJL_TJXM.GYID = JKDA_TJJL.ID ) and  
         ( MZGH_BRJZ_GH.GHLSH = JKDA_TJJL.GHLSH ) and JKDA_TJJL_TJXM.ZXKS=51";


                    sqlstr = sqlstr + " and JKDA_TJJL.TJSJ > '" + DateTime.Today.AddDays(-30) + "' and JKDA_TJJL_TJXM.ID='" + sqgy + "' ";

                  
                    try
                    {
                     //   PATHGETHISZGQ.cszxyyWEB.NYwclService cszxyy = new PATHGETHISZGQ.cszxyyWEB.NYwclService();
                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);

                        cszxyy.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("调用webservices出现问题！"); return "0";
                    }
                    if (rtn == "" || rtn == null)
                    {
                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
                        return "0";
                    }
                    log.WriteMyLog(rtn);
                    try
                    {
                        DataSet ds1 = new DataSet();
                        // XmlNode xmlok = null;
                        XmlDocument xd = new XmlDocument();

                        StringReader sr = new StringReader(rtn);
                        XmlReader xr = new XmlTextReader(sr);
                        ds1.ReadXml(xr);
                        if (Debug == "1")
                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());




                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["userid"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["MZBAH"].ToString() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["GHLSH"].ToString() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + (char)34 + " ";

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";


                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + ds1.Tables[0].Rows[0]["DWMC"].ToString() + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
                        xml = xml + "/>";

                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
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
                #endregion



                MessageBox.Show("无此" + Sslbx);
                return "0";

            }
            MessageBox.Show("识别类型不能为空" );
            return "0";

        }


        //20150109
//        public static string ptxml(string Sslbx, string Ssbz, string Debug)
//        {

//            string pathWEB = f.ReadString(Sslbx, "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
//            string Ipstr = "127.0.0.1";
//            try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
//            catch { Ipstr = "127.0.0.1"; }

//            PATHGETHISZGQ.cszxyyWEB.NYwclService cszxyy = new PATHGETHISZGQ.cszxyyWEB.NYwclService();
//            if (pathWEB != "")
//                cszxyy.Url = pathWEB;

//            if (Sslbx != "")
//            {
//                string rtn = "";

//                #region 住院号
//                if (Sslbx == "住院号")
//                {

//                    string sqlstr = "select *,ZYBAH,XM,(case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,CWH,"
//                    + "SFZH,HKDZ,LXDH,(select DMMC from SYS_FYBZ_FYLY where DM=ZYJS_HZJBXX.FB) as FB,USERID  FROM ZYJS_HZJBXX WHERE ZYBAH='" + Ssbz.Trim() + "'";

//                    try
//                    {
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_iinhos", sqlstr);
//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "")
//                    {
//                        MessageBox.Show("没有该住院号信息！请核实");
//                        return "0";
//                    }

//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        // XmlNode xmlok = null;
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        if (Debug == "1")
//                            MessageBox.Show(rtn);

//                        string rtnstring = "";
//                        //if (jcblh("select F_BLH,F_XM,F_SDRQ from T_JCXX  where F_XM='" + ds1.Tables[0].Rows[0]["XM"].ToString() + "' and F_ZYH='" + ds1.Tables[0].Rows[0]["ZYBAH"].ToString() + "' and F_SDRQ>='"+DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd")+"'  and F_BY1 like '%冰%'", ref rtnstring))
//                        //{
//                        //    if(MessageBox.Show("此病人可能已经登记冰冻，请确认是否继续登记！！！\r\n"+rtnstring,"提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2)==DialogResult.No)
//                        //    {
//                        //         return "0";
//                        //    }
//                        //}

//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYBAH"].ToString() + (char)34 + " ";
//                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NL"].ToString() + ds1.Tables[0].Rows[0]["NLDW"].ToString() + (char)34 + " ";

//                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[0]["SFZH"].ToString() + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + ds1.Tables[0].Rows[0]["FB"].ToString() + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
//                        xml = xml + "/>";
//                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
//                        xml = xml + "</LOGENE>";

//                        return xml;

//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.Message.ToString());
//                        return "0";
//                    }
//                }
//                #endregion

//                #region 门诊号
//                if (Sslbx == "门诊号")
//                {
//                    string sqlstr = "select MZGH_BRJZ_GH.USERID as userid,MZGH_BRJZ_GH.MZBAH as mzbah,MZGH_BRJZ_GH.GHLSH as ghlsh,MZGH_BRJZ_GH.XM as xm,MZGH_BRJZ_GH.XB as xb,"
//                        + "(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then CONVERT(VARCHAR(10),"
//                        + "DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,"
//                        + "'' as SFZH,MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh from  MZGH_BRJZ_GH where GHLSH='" + Ssbz + "'";


//                    try
//                    {
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);
//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！");
//                        return "0";
//                    }
//                    if (rtn == "")
//                    {
//                        MessageBox.Show("没有该门诊号信息！请核实");
//                        return "0";
//                    }


//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        if (Debug == "1")
//                            MessageBox.Show(rtn);

//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["mzbah"].ToString() + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["ghlsh"].ToString() + (char)34 + " ";
//                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["nlmc"].ToString() + (char)34 + " ";

//                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["zz"].ToString() + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[0]["SFZH"].ToString() + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
//                        xml = xml + "/>";
//                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
//                        xml = xml + "</LOGENE>";

//                        return xml;

//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.Message.ToString());
//                        return "0";
//                    }
//                }
//                #endregion

//                #region 住院申请号

//                if (Sslbx == "住院申请号")
//                {
//                    string sqgy = "";
//                    try
//                    {
//                        Double.Parse(Ssbz);
//                    }
//                    catch (Exception ee)
//                    {
//                        MessageBox.Show("住院申请号有误,只能是12位数字" + ee.Message.ToString());
//                        return "0";
//                    }
//                    if (Ssbz.Length > 12)
//                        sqgy = Ssbz.Trim().Substring(0, 12);
//                    else
//                        sqgy = Ssbz;
//                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    string sqlstr = "SELECT ZYJS_HZJBXX.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
//         + "EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
//         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
//         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
//         + "ZYJS_HZJBXX.XM,(case ZYJS_HZJBXX.XB  when '2' then '女' else '男' end)  as XB,ZYJS_HZJBXX.CWH,ZYJS_HZJBXX.NL,(case ZYJS_HZJBXX.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,ZYJS_HZJBXX.HKDZ,ZYJS_HZJBXX.LXDH,"
//         + "(select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,ZYJS_HZJBXX  "
//         + "WHERE      EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and ZYJS_HZJBXX.ZYBAH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ID='" + sqgy.Trim() + "'";



//                    try
//                    {
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_iinhos", sqlstr);

//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "" || rtn == null)
//                    {
//                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
//                        return "0";
//                    }

//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        if (Debug == "1")
//                            MessageBox.Show(rtn);

//                        string rtnstring = "";
//                        //if (jcblh("select F_BLH,F_XM,F_SDRQ from T_JCXX  where F_XM='" + ds1.Tables[0].Rows[0]["XM"].ToString() + "' and F_ZYH='" + ds1.Tables[0].Rows[0]["ZYH"].ToString() + "' and F_SDRQ>='" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "'  and F_BY1 like '%冰%'", ref rtnstring))
//                        //{
//                        //    if (MessageBox.Show("此病人可能已经登记冰冻，请确认是否继续登记！！！\r\n" + rtnstring, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
//                        //    {
//                        //        return "0";
//                        //    }
//                        //}

//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
//                        xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
//                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NL"].ToString() + "岁" + (char)34 + " ";

//                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                        string bbmc = "";
//                        if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
//                            bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

//                        if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
//                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

//                        if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
//                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

//                        if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
//                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

//                        xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
//                        xml = xml + "/>";
//                        // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
//                        string aa = "";
//                        if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
//                            aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
//                        if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
//                            aa = aa + "  手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();
//                        xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
//                        xml = xml + "</LOGENE>";

//                        return xml;

//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.Message.ToString());
//                        return "0";
//                    }
//                }
//                #endregion


//                #region 门诊申请号
//                if (Sslbx == "门诊申请号")
//                {
//                    string sqgy = "";
//                    try
//                    {
//                        Double.Parse(Ssbz);
//                    }
//                    catch (Exception ee)
//                    {
//                        MessageBox.Show("门诊申请号有误,只能是12位数字" + ee.Message.ToString());
//                        return "0";
//                    }
//                    if (Ssbz.Length > 12)
//                        sqgy = Ssbz.Trim().Substring(0, 12);
//                    else
//                        sqgy = Ssbz;
//                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    string sqlstr = "SELECT MZGH_BRJZ_GH.MZBAH as mzbah,MZGH_BRJZ_GH.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
//       + " EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
//       + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
//        + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
//       + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
//         + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
//      + " (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
//      + "  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
//       + "  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
//       + " (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
//        + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
//        + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
//        + " MZGH_BRJZ_GH.XM,MZGH_BRJZ_GH.XB,'' AS CWH,"
//        + "(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) when '2' then CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) end) AS NLMC,"
//        + "(case MZGH_BRJZ_GH.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH,"
//        + " (select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,MZGH_BRJZ_GH "
//        + " WHERE    EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and MZGH_BRJZ_GH.GHLSH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ID='" + sqgy.Trim() + "'";


//                    try
//                    {
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);
//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "" || rtn == null)
//                    {
//                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
//                        return "0";
//                    }

//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        // XmlNode xmlok = null;
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        if (Debug == "1") MessageBox.Show(rtn);

//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["mzbah"].ToString() + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
//                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
//                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + "岁" + (char)34 + " ";

//                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                        string bbmc = "";
//                        if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
//                            bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

//                        if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
//                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

//                        if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
//                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

//                        if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
//                            bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
//                        if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
//                            bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

//                        xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
//                        xml = xml + "/>";
//                        // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
//                        string aa = "";
//                        if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
//                            aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
//                        if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
//                            aa = aa + "手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();

//                        xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
//                        xml = xml + "</LOGENE>";

//                        return xml;

//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.ToString());
//                        return "0";
//                    }
//                }
//                #endregion


//                #region 住院号(申请)

//                if (Sslbx == "住院号(申请)")
//                {

//                    string ordersql = f.ReadString("住院号(申请)", "ordersql", ""); //获取sz.ini中设置的ordersql,排序等
//                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    string sqlstr = "SELECT ZYJS_HZJBXX.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
//         + "EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
//         + "(select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
//         + "(select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
//         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
//         + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
//         + "ZYJS_HZJBXX.XM,(case ZYJS_HZJBXX.XB  when '2' then '女' else '男' end)  as XB,ZYJS_HZJBXX.CWH,ZYJS_HZJBXX.NL,(case ZYJS_HZJBXX.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,ZYJS_HZJBXX.HKDZ,ZYJS_HZJBXX.LXDH,"
//         + "(select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,ZYJS_HZJBXX  "
//         + "WHERE      EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and ZYJS_HZJBXX.ZYBAH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ZYH='" + Ssbz.Trim() + "'";

//                    sqlstr = sqlstr + "  " + ordersql;
//                    try
//                    {
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_iinhos", sqlstr);
//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "" || rtn == null)
//                    {
//                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
//                        return "0";
//                    }

//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        // XmlNode xmlok = null;
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        if (Debug == "1")
//                        {
//                            MessageBox.Show(rtn);
//                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());
//                        }

//                        if (ds1.Tables[0].Rows.Count > 1)
//                        {

//                            LG_GETHIS_ZGQ.cszxyy cs = new LG_GETHIS_ZGQ.cszxyy(ds1, 1);
//                            cs.ShowDialog();
//                            string index = cs.A;


//                            DataTable dtNew = new DataTable();
//                            DataView view = new DataView();
//                            view.Table = ds1.Tables[0];
//                            view.RowFilter = "ID = '" + index + "'";
//                            dtNew = view.ToTable();

//                            string rtnstring = "";
//                            //if (jcblh("select F_BLH,F_XM,F_SDRQ from T_JCXX  where F_XM='" + dtNew.Rows[0]["XM"].ToString() + "' and F_ZYH='" + dtNew.Rows[0]["ZYH"].ToString() + "' and F_SDRQ>='" + DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd") + "'  and F_BY1 like '%冰%'", ref rtnstring))
//                            //{
//                            //    if (MessageBox.Show("此病人可能已经登记冰冻，请确认是否继续登记！！！\r\n" + rtnstring, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
//                            //    {
//                            //        return "0";
//                            //    }
//                            //}



//                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                            xml = xml + "<LOGENE>";
//                            xml = xml + "<row ";
//                            xml = xml + "病人编号=" + (char)34 + dtNew.Rows[0]["USERID"].ToString() + (char)34 + " ";
//                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "申请序号=" + (char)34 + dtNew.Rows[0]["ID"].ToString() + (char)34 + " ";
//                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "住院号=" + (char)34 + dtNew.Rows[0]["ZYH"].ToString() + (char)34 + " ";
//                            xml = xml + "姓名=" + (char)34 + dtNew.Rows[0]["XM"].ToString() + (char)34 + " ";
//                            xml = xml + "性别=" + (char)34 + dtNew.Rows[0]["XB"].ToString() + (char)34 + " ";
//                            xml = xml + "年龄=" + (char)34 + dtNew.Rows[0]["NL"].ToString() + "岁" + (char)34 + " ";

//                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "地址=" + (char)34 + dtNew.Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
//                            xml = xml + "电话=" + (char)34 + dtNew.Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "床号=" + (char)34 + dtNew.Rows[0]["CWH"].ToString() + (char)34 + " ";
//                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "送检科室=" + (char)34 + dtNew.Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
//                            xml = xml + "送检医生=" + (char)34 + dtNew.Rows[0]["YSDM"].ToString() + (char)34 + " ";

//                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                            string bbmc = "";
//                            if (dtNew.Rows[0]["bw1"].ToString() != "")
//                                bbmc = bbmc + dtNew.Rows[0]["bw1"].ToString();
//                            if (dtNew.Rows[0]["bb1"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb1"].ToString() + ")";

//                            if (dtNew.Rows[0]["bw2"].ToString() != "")
//                                bbmc = bbmc + "," + dtNew.Rows[0]["bw2"].ToString();
//                            if (dtNew.Rows[0]["bb2"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb2"].ToString() + ")";

//                            if (dtNew.Rows[0]["bw3"].ToString() != "")
//                                bbmc = bbmc + "," + dtNew.Rows[0]["bw3"].ToString();
//                            if (dtNew.Rows[0]["bb3"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb3"].ToString() + ")";

//                            if (dtNew.Rows[0]["bw4"].ToString() != "")
//                                bbmc = bbmc + "," + dtNew.Rows[0]["bw4"].ToString();
//                            if (dtNew.Rows[0]["bb4"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb4"].ToString() + ")";

//                            xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
//                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
//                            xml = xml + "/>";
//                            // "病史摘要：" + dtNew.Rows[0]["BSJY"].ToString()  +
//                            string aa = "";
//                            if (dtNew.Rows[0]["LCSJ"].ToString().Trim() != "")
//                                aa = "临床所见：" + dtNew.Rows[0]["LCSJ"].ToString() + "     ";
//                            if (dtNew.Rows[0]["sssj"].ToString().Trim() != "")
//                                aa = aa + "  手术所见：" + dtNew.Rows[0]["sssj"].ToString();
//                            xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
//                            xml = xml + "<临床诊断><![CDATA[" + dtNew.Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
//                            xml = xml + "</LOGENE>";

//                            return xml;




//                        }
//                        else
//                        {

//                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                            xml = xml + "<LOGENE>";
//                            xml = xml + "<row ";
//                            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
//                            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
//                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
//                            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                            xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                            xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NL"].ToString() + "岁" + (char)34 + " ";

//                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["HKDZ"].ToString() + (char)34 + "   ";
//                            xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
//                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
//                            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

//                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                            string bbmc = "";
//                            if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
//                                bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

//                            if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
//                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

//                            if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
//                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

//                            if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
//                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

//                            xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
//                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
//                            xml = xml + "/>";
//                            // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
//                            string aa = "";
//                            if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
//                                aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
//                            if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
//                                aa = aa + "  手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();
//                            xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
//                            xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
//                            xml = xml + "</LOGENE>";

//                            return xml;
//                        }
//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.ToString());
//                        return "0";
//                    }
//                }
//                #endregion

//                #region  门诊号(申请)
//                if (Sslbx == "门诊号(申请)")
//                {

//                    string ordersql = f.ReadString("门诊号(申请)", "ordersql", "");
//                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    string sqlstr = "SELECT MZGH_BRJZ_GH.MZBAH as mzbah,MZGH_BRJZ_GH.USERID,EMR_SQGY.ID,EMR_SQGY.ZYH, EMR_SQGY.SQKS,(select CZYXM FROM CO_QX_XTCZY WHERE CO_QX_XTCZY.CZYDM= EMR_SQGY.YSDM) AS YSDM,EMR_SQGY_SQSM.JYBS as BSJY,EMR_SQGY_SQSM.TEXT1 as LCSJ,"
//       + " EMR_SQGY_SQSM.TEXT2 as SSSJ,EMR_SQGY_SQSM.LCZD as LCZD,EMR_SQGY_SQSM.VAR1 as SJTSYQ,"
//       + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BW1,"
//        + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BW2,"
//       + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BW3,"
//         + " (select ZDY1 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BW4,"
//      + " (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='01') as BB1,"
//      + "  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='02') as BB2,"
//       + "  (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='03') as BB3,"
//       + " (select ZDY2 from EMR_SQGY_JLXM where XMDM='BB' and GYID=EMR_SQGY.ID and XMMC='04') as BB4,"
//        + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='原病理号') as QT1,"
//        + "(select XMLR from EMR_SQGY_JLXM where XMDM='QT' and GYID=EMR_SQGY.ID and XMMC='病程') as QT2,"
//        + " MZGH_BRJZ_GH.XM,MZGH_BRJZ_GH.XB,'' AS CWH,"
//        + "(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) when '2' then CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE())) end) AS NLMC,"
//        + "(case MZGH_BRJZ_GH.NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW,MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH,"
//        + " (select BMMC from CO_QX_XTBM where BMNBBH=EMR_SQGY.SQKS)  as SQKSMC,space(200) as YSQM,space(200) as TMPATH  FROM EMR_SQGY,EMR_SQGY_SQSM,MZGH_BRJZ_GH "
//        + " WHERE    EMR_SQGY.ID=EMR_SQGY_SQSM.GYID and MZGH_BRJZ_GH.GHLSH=EMR_SQGY.ZYH and EMR_SQGY.ZXKS=51 AND EMR_SQGY.ZYH='" + Ssbz.Trim() + "'";
//                    sqlstr = sqlstr + "  " + ordersql;
//                    try
//                    {
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);
//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "" || rtn == null)
//                    {
//                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
//                        return "0";
//                    }

//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        // XmlNode xmlok = null;
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);

//                        if (Debug == "1")
//                        {
//                            MessageBox.Show(rtn);
//                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());
//                        }
//                        if (ds1.Tables[0].Rows.Count > 1)
//                        {




//                            LG_GETHIS_ZGQ.cszxyy cs = new LG_GETHIS_ZGQ.cszxyy(ds1, 1);
//                            cs.ShowDialog();
//                            string index = cs.A;


//                            DataTable dtNew = new DataTable();
//                            DataView view = new DataView();
//                            view.Table = ds1.Tables[0];
//                            view.RowFilter = "ID = '" + index + "'";
//                            dtNew = view.ToTable();


//                            string xml2 = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                            xml2 = xml2 + "<LOGENE>";
//                            xml2 = xml2 + "<row ";
//                            xml2 = xml2 + "病人编号=" + (char)34 + dtNew.Rows[0]["USERID"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "就诊ID=" + (char)34 + dtNew.Rows[0]["mzbah"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "申请序号=" + (char)34 + dtNew.Rows[0]["ID"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "门诊号=" + (char)34 + dtNew.Rows[0]["ZYH"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "住院号=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "姓名=" + (char)34 + dtNew.Rows[0]["XM"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "性别=" + (char)34 + dtNew.Rows[0]["XB"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "年龄=" + (char)34 + dtNew.Rows[0]["NLMC"].ToString() + "岁" + (char)34 + " ";

//                            xml2 = xml2 + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "地址=" + (char)34 + dtNew.Rows[0]["ZZ"].ToString() + (char)34 + "   ";
//                            xml2 = xml2 + "电话=" + (char)34 + dtNew.Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "病区=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "床号=" + (char)34 + dtNew.Rows[0]["CWH"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "民族=" + (char)34 + " " + (char)34 + " ";
//                            xml2 = xml2 + "职业=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "送检科室=" + (char)34 + dtNew.Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "送检医生=" + (char)34 + dtNew.Rows[0]["YSDM"].ToString() + (char)34 + " ";

//                            xml2 = xml2 + "收费=" + (char)34 + "" + (char)34 + " ";
//                            string bbmc = "";
//                            if (dtNew.Rows[0]["bw1"].ToString() != "")
//                                bbmc = bbmc + dtNew.Rows[0]["bw1"].ToString();
//                            if (dtNew.Rows[0]["bb1"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb1"].ToString() + ")";

//                            if (dtNew.Rows[0]["bw2"].ToString() != "")
//                                bbmc = bbmc + "," + dtNew.Rows[0]["bw2"].ToString();
//                            if (dtNew.Rows[0]["bb2"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb2"].ToString() + ")";

//                            if (dtNew.Rows[0]["bw3"].ToString() != "")
//                                bbmc = bbmc + "," + dtNew.Rows[0]["bw3"].ToString();
//                            if (dtNew.Rows[0]["bb3"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb3"].ToString() + ")";

//                            if (dtNew.Rows[0]["bw4"].ToString() != "")
//                                bbmc = bbmc + "," + dtNew.Rows[0]["bw4"].ToString();
//                            if (dtNew.Rows[0]["bb4"].ToString() != "")
//                                bbmc = bbmc + "(" + dtNew.Rows[0]["bb4"].ToString() + ")";

//                            xml2 = xml2 + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
//                            xml2 = xml2 + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                            xml2 = xml2 + "医嘱项目=" + (char)34 + (char)34 + " ";
//                            xml2 = xml2 + "备用1=" + (char)34 + (char)34 + " ";
//                            xml2 = xml2 + "备用2=" + (char)34 + (char)34 + " ";
//                            xml2 = xml2 + "费别=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
//                            xml2 = xml2 + "/>";
//                            // "病史摘要：" + ds1.Rows[0]["BSJY"].ToString()  +
//                            string aa = "";
//                            if (dtNew.Rows[0]["LCSJ"].ToString().Trim() != "")
//                                aa = "临床所见：" + dtNew.Rows[0]["LCSJ"].ToString() + "     ";
//                            if (dtNew.Rows[0]["sssj"].ToString().Trim() != "")
//                                aa = aa + "手术所见：" + dtNew.Rows[0]["sssj"].ToString();

//                            xml2 = xml2 + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
//                            xml2 = xml2 + "<临床诊断><![CDATA[" + dtNew.Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
//                            xml2 = xml2 + "</LOGENE>";

//                            return xml2;



//                        }
//                        else
//                        {
//                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                            xml = xml + "<LOGENE>";
//                            xml = xml + "<row ";
//                            xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["USERID"].ToString() + (char)34 + " ";
//                            xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["mzbah"].ToString() + (char)34 + " ";
//                            xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
//                            xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["ZYH"].ToString() + (char)34 + " ";
//                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                            xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                            xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + "岁" + (char)34 + " ";

//                            xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
//                            xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[0]["CWH"].ToString() + (char)34 + " ";
//                            xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[0]["SQKSMC"].ToString() + (char)34 + " ";
//                            xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[0]["YSDM"].ToString() + (char)34 + " ";

//                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                            string bbmc = "";
//                            if (ds1.Tables[0].Rows[0]["bw1"].ToString() != "")
//                                bbmc = bbmc + ds1.Tables[0].Rows[0]["bw1"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb1"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb1"].ToString() + ")";

//                            if (ds1.Tables[0].Rows[0]["bw2"].ToString() != "")
//                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw2"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb2"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb2"].ToString() + ")";

//                            if (ds1.Tables[0].Rows[0]["bw3"].ToString() != "")
//                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw3"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb3"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb3"].ToString() + ")";

//                            if (ds1.Tables[0].Rows[0]["bw4"].ToString() != "")
//                                bbmc = bbmc + "," + ds1.Tables[0].Rows[0]["bw4"].ToString();
//                            if (ds1.Tables[0].Rows[0]["bb4"].ToString() != "")
//                                bbmc = bbmc + "(" + ds1.Tables[0].Rows[0]["bb4"].ToString() + ")";

//                            xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
//                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
//                            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
//                            xml = xml + "/>";
//                            // "病史摘要：" + ds1.Tables[0].Rows[0]["BSJY"].ToString()  +
//                            string aa = "";
//                            if (ds1.Tables[0].Rows[0]["LCSJ"].ToString().Trim() != "")
//                                aa = "临床所见：" + ds1.Tables[0].Rows[0]["LCSJ"].ToString() + "     ";
//                            if (ds1.Tables[0].Rows[0]["sssj"].ToString().Trim() != "")
//                                aa = aa + "手术所见：" + ds1.Tables[0].Rows[0]["sssj"].ToString();

//                            xml = xml + "<临床病史><![CDATA[" + aa.Trim() + "]]></临床病史>";
//                            xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[0]["LCZD"].ToString() + "]]></临床诊断>";
//                            xml = xml + "</LOGENE>";

//                            return xml;
//                        }
//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.Message.ToString());
//                        return "0";
//                    }
//                }
//                #endregion


//                #region  体检

//                if (Sslbx == "体检号")
//                {
//                    int ts = -3;
//                    string tianshu = f.ReadString("体检号", "tianshu", ""); //获取sz.ini中设置的获取体检信息的天数
//                    if (tianshu.Trim() != "")
//                        ts = int.Parse(tianshu);


//                    string sqgy = "";
//                    try
//                    {
//                        Double.Parse(Ssbz);
//                    }
//                    catch (Exception ee)
//                    {
//                        MessageBox.Show("体检号有误,只能是12位数字" + ee.ToString());
//                        return "0";
//                    }
//                    if (Ssbz.Length > 12)
//                        sqgy = Ssbz.Trim().Substring(0, 12);
//                    else
//                        sqgy = Ssbz;

//                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    //                    string sqlstr = @"select  JKDA_TJJL_TJXM.ID,MZGH_BRJZ_GH.MZBAH,MZGH_BRJZ_GH.USERID as userid,MZGH_BRJZ_GH.GHLSH,
//                    //MZGH_BRJZ_GH.XM as xm,MZGH_BRJZ_GH.XB as xb,
//                    //(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),
//                    //DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then
//                    // CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,'' as SFZH,
//                    //                    MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh ,JKDA_TJJL.TJSJ,JKDA_RYXX.DWMC 
//                    //   FROM JKDA_RYXX,   
//                    //         JKDA_TJJL,   
//                    //         JKDA_TJJL_TJXM,   
//                    //         MZGH_BRJZ_GH  
//                    //   WHERE ( JKDA_TJJL.JKDAH = JKDA_RYXX.JKDAH ) and  
//                    //         ( JKDA_TJJL_TJXM.GYID = JKDA_TJJL.ID ) and  
//                    //         ( MZGH_BRJZ_GH.GHLSH = JKDA_TJJL.GHLSH ) and JKDA_TJJL_TJXM.ZXKS=51" ;

//                    string sqlstr = @"select  JKDA_TJJL_TJXM.ID,
//(SELECT DMMC FROM SYS_FYBZ_YZML WHERE SYS_FYBZ_YZML.DM=JKDA_TJJL_TJXM.XMID) AS XMMC,
//MZGH_BRJZ_GH.MZBAH,
//MZGH_BRJZ_GH.USERID as userid,
//MZGH_BRJZ_GH.GHLSH,
//MZGH_BRJZ_GH.XM as xm,
//MZGH_BRJZ_GH.XB as xb,
//(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),
//DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then
// CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,'' as SFZH,
//                    MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh ,JKDA_TJJL.TJSJ,JKDA_RYXX.DWMC 
//   FROM JKDA_RYXX,   
//         JKDA_TJJL,   
//         JKDA_TJJL_TJXM,   
//         MZGH_BRJZ_GH  
//   WHERE ( JKDA_TJJL.JKDAH = JKDA_RYXX.JKDAH ) and  
//         ( JKDA_TJJL_TJXM.GYID = JKDA_TJJL.ID ) and  
//         ( MZGH_BRJZ_GH.GHLSH = JKDA_TJJL.GHLSH ) and JKDA_TJJL_TJXM.ZXKS=51";



//                    sqlstr = sqlstr + "  and JKDA_RYXX.JKDAH='" + sqgy + "'  order by  JKDA_TJJL.TJSJ  desc ";

//                    string Ipstr = "127.0.0.1";
//                    try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
//                    catch { Ipstr = "127.0.0.1"; }
//                    try
//                    {
//                        PATHGETHISZGQ.cszxyyWEB.NYwclService cszxyy = new PATHGETHISZGQ.cszxyyWEB.NYwclService();
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);

//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "" || rtn == null)
//                    {
//                        MessageBox.Show("没有该体检信息，请核实体检号是否正确！");
//                        return "0";
//                    }

//                    try
//                    {
//                        string index = "";

//                        DataSet ds1 = new DataSet();
//                        // XmlNode xmlok = null;
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        //---------------------------------------
//                        if (Debug == "1")
//                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());

//                        if (ds1.Tables[0].Rows.Count > 1)
//                        {
//                            LG_GETHIS_ZGQ.cszxyy cs = new LG_GETHIS_ZGQ.cszxyy(ds1, 3);
//                            cs.ShowDialog();
//                            index = cs.A;


//                            DataTable dtNew = new DataTable();
//                            DataView view = new DataView();
//                            view.Table = ds1.Tables[0];
//                            view.RowFilter = "ID = '" + index + "'";
//                            dtNew = view.ToTable();

//                            string xml2 = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                            xml2 = xml2 + "<LOGENE>";
//                            xml2 = xml2 + "<row ";
//                            xml2 = xml2 + "病人编号=" + (char)34 + dtNew.Rows[0]["userid"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "就诊ID=" + (char)34 + dtNew.Rows[0]["MZBAH"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "申请序号=" + (char)34 + dtNew.Rows[0]["ID"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "门诊号=" + (char)34 + dtNew.Rows[0]["GHLSH"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "住院号=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "姓名=" + (char)34 + dtNew.Rows[0]["XM"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "性别=" + (char)34 + dtNew.Rows[0]["XB"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "年龄=" + (char)34 + dtNew.Rows[0]["NLMC"].ToString() + (char)34 + " ";

//                            xml2 = xml2 + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "地址=" + (char)34 + dtNew.Rows[0]["ZZ"].ToString() + (char)34 + "   ";
//                            xml2 = xml2 + "电话=" + (char)34 + dtNew.Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "病区=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "床号=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "民族=" + (char)34 + " " + (char)34 + " ";
//                            xml2 = xml2 + "职业=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
//                            xml2 = xml2 + "送检医生=" + (char)34 + "" + (char)34 + " ";

//                            xml2 = xml2 + "收费=" + (char)34 + "" + (char)34 + " ";


//                            xml2 = xml2 + "标本名称=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                            xml2 = xml2 + "医嘱项目=" + (char)34 + dtNew.Rows[0]["XMMC"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "备用1=" + (char)34 + dtNew.Rows[0]["DWMC"].ToString() + (char)34 + " ";
//                            xml2 = xml2 + "备用2=" + (char)34 + (char)34 + " ";
//                            xml2 = xml2 + "费别=" + (char)34 + "" + (char)34 + " ";
//                            xml2 = xml2 + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
//                            xml2 = xml2 + "/>";

//                            xml2 = xml2 + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
//                            xml2 = xml2 + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
//                            xml2 = xml2 + "</LOGENE>";

//                            return xml2;



//                        }

//                        //-----------------------------------------
//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["userid"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["MZBAH"].ToString() + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
//                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["GHLSH"].ToString() + (char)34 + " ";
//                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + (char)34 + " ";

//                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";


//                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + ds1.Tables[0].Rows[0]["XMMC"].ToString() + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + ds1.Tables[0].Rows[0]["DWMC"].ToString() + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
//                        xml = xml + "/>";

//                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
//                        xml = xml + "</LOGENE>";

//                        return xml;

//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.ToString());
//                        return "0";
//                    }
//                }
//                //------------------------------------------------------------------------
//                #endregion

//                #region   体检申请号

//                if (Sslbx == "体检申请号")
//                {



//                    string sqgy = "";
//                    try
//                    {
//                        Double.Parse(Ssbz);
//                    }
//                    catch (Exception ee)
//                    {
//                        MessageBox.Show("体检申请号有误,只能是12位数字" + ee.ToString());
//                        return "0";
//                    }
//                    if (Ssbz.Length > 12)
//                        sqgy = Ssbz.Trim().Substring(0, 12);
//                    else
//                        sqgy = Ssbz;

//                    //  (case XB when '2' then '女' else '男' end) as XB,NL,(case NLDW when '2' then '月' when '3' then '天' else '岁' end) as NLDW
//                    string sqlstr = @"select  JKDA_TJJL_TJXM.ID,MZGH_BRJZ_GH.MZBAH,MZGH_BRJZ_GH.USERID as userid,MZGH_BRJZ_GH.GHLSH,
//MZGH_BRJZ_GH.XM as xm,MZGH_BRJZ_GH.XB as xb,
//(CASE MZGH_BRJZ_GH.NLDW WHEN '3' THEN CONVERT(VARCHAR(10),
//DATEDIFF(day,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'天' when '2' then
// CONVERT(VARCHAR(10),DATEDIFF(month,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'月' else CONVERT(VARCHAR(10),DATEDIFF(year,convert(datetime,MZGH_BRJZ_GH.CSRQ),GETDATE()))+'岁' end) AS NLMC,'' as SFZH,
//                    MZGH_BRJZ_GH.ZZ,MZGH_BRJZ_GH.LXDH as lxdh ,JKDA_TJJL.TJSJ,JKDA_RYXX.DWMC 
//   FROM JKDA_RYXX,   
//         JKDA_TJJL,   
//         JKDA_TJJL_TJXM,   
//         MZGH_BRJZ_GH  
//   WHERE ( JKDA_TJJL.JKDAH = JKDA_RYXX.JKDAH ) and  
//         ( JKDA_TJJL_TJXM.GYID = JKDA_TJJL.ID ) and  
//         ( MZGH_BRJZ_GH.GHLSH = JKDA_TJJL.GHLSH ) and JKDA_TJJL_TJXM.ZXKS=51";


//                    sqlstr = sqlstr + " and JKDA_TJJL.TJSJ > '" + DateTime.Today.AddDays(-30) + "' and JKDA_TJJL_TJXM.ID='" + sqgy + "' ";

//                    string Ipstr = "127.0.0.1";
//                    try { Ipstr = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(); }
//                    catch { Ipstr = "127.0.0.1"; }
//                    try
//                    {
//                        PATHGETHISZGQ.cszxyyWEB.NYwclService cszxyy = new PATHGETHISZGQ.cszxyyWEB.NYwclService();
//                        rtn = cszxyy.bselect(3626, "111", Ipstr, "his_clinic", sqlstr);

//                        cszxyy.Dispose();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("调用webservices出现问题！"); return "0";
//                    }
//                    if (rtn == "" || rtn == null)
//                    {
//                        MessageBox.Show("没有该申请单信息，请核实申请号是否正确！");
//                        return "0";
//                    }
//                    log.WriteMyLog(rtn);
//                    try
//                    {
//                        DataSet ds1 = new DataSet();
//                        // XmlNode xmlok = null;
//                        XmlDocument xd = new XmlDocument();

//                        StringReader sr = new StringReader(rtn);
//                        XmlReader xr = new XmlTextReader(sr);
//                        ds1.ReadXml(xr);
//                        if (Debug == "1")
//                            MessageBox.Show(ds1.Tables[0].Rows.Count.ToString());




//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[0]["userid"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[0]["MZBAH"].ToString() + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[0]["ID"].ToString() + (char)34 + " ";
//                        xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[0]["GHLSH"].ToString() + (char)34 + " ";
//                        xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[0]["XM"].ToString() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[0]["XB"].ToString() + (char)34 + " ";
//                        xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[0]["NLMC"].ToString() + (char)34 + " ";

//                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[0]["ZZ"].ToString() + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[0]["LXDH"].ToString() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + "体检科" + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";

//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";


//                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + ds1.Tables[0].Rows[0]["DWMC"].ToString() + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
//                        xml = xml + "/>";

//                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
//                        xml = xml + "</LOGENE>";

//                        return xml;

//                    }
//                    catch (Exception ee)
//                    {
//                        if (Debug == "1")
//                            MessageBox.Show(ee.ToString());
//                        return "0";
//                    }
//                }
//                #endregion



//                MessageBox.Show("无此" + Sslbx);
//                return "0";

//            }
//            MessageBox.Show("识别类型不能为空");
//            return "0";

//        }


        public static bool jcblh(string sql, ref  string rtnstring)
        {
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable dt = new DataTable();
            dt = aa.GetDataTable(sql,"d1");
            if (dt==null)
            {
                return false;
            }
            if (dt.Rows.Count > 0)
            {
                for (int x = 0; x < dt.Rows.Count; x++)
                {
                    rtnstring = rtnstring + "病理号：" + dt.Rows[x]["F_BLH"].ToString() + ";  姓名：" + dt.Rows[x]["F_XM"].ToString() + ";  收到日期：" + dt.Rows[x]["F_SDRQ"].ToString() + "\r\n";
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

