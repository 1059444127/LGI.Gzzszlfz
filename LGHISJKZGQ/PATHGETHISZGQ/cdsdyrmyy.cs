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
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    /// <summary>
    /// 常德市第一人民医院
    /// </summary>
    class cdsdyrmyy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
         
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {

          //  string pathWEB = f.ReadString(Sslbx, "webservicesurl", ""); //获取sz.ini中设置的webservicesurl
          
            string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
            string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
            if (Sslbx != "")
            {
               
                if (Sslbx == "条码号")
                {
                    #region 条码号
                    string  sqdh="";
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_bbxx = new DataTable();
                    dt_bbxx = aa.GetDataTable("select * from T_sqd_bbxx where F_TMH='" + Ssbz.Trim() + "'", "bbtmh");

                    if (dt_bbxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此标本条码信息"); return "0";
                    }
            
                        sqdh=dt_bbxx.Rows[0]["F_SQXH"].ToString().Trim();
                        DataTable dt_sqdxx = new DataTable();
                        dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_sqxh='" +sqdh + "' ", "sqdxx");

                         if (dt_sqdxx.Rows.Count <= 0)
                        {
                            MessageBox.Show("无此标本条码对应申请单信息"); return "0";
                        }

                        DataTable dt_yzxx = new DataTable();
                        dt_yzxx = aa.GetDataTable("select * from T_sqd_yzxx where F_sqxh='" +sqdh + "'", "yzxx");

                        DataTable dt_bbxx2 = new DataTable();
                        dt_bbxx2 = aa.GetDataTable("select * from T_sqd_bbxx where F_sqxh='" + sqdh + "'", "bbxx");


                        if (dt_bbxx2.Rows.Count > 1)
                        {
                            if (tqbblb == "1")
                            {
                                bool yzbbxx = true;
                                Frm_CDRMYY cd = new Frm_CDRMYY(sqdh, dt_yzxx, dt_bbxx2, dt_sqdxx, Ssbz.Trim(), yzbbxx);
                                cd.ShowDialog();

                                if (cd.DialogResult == DialogResult.Yes)
                                {
                                }
                                else
                                {
                                    if (yzbbxx)
                                    {
                                        MessageBox.Show("标本袋数量不正确,不能提取信息！");
                                        return "0";
                                    }
                                }
                            }
                        }

                    

                    DataTable dt = new DataTable();
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[0]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[0]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[0]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_sqdxx.Rows[0]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[0]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "未婚"; break;
                            case "2": MARRIED = "已婚"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[0]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 +"" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[0]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_sqdxx.Rows[0]["F_SQYS"].ToString().Trim() + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_sqdxx.Rows[0]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqdxx.Rows[0]["F_YZXMBM"].ToString().Trim()+"^"+dt_sqdxx.Rows[0]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "病人类别=" + (char)34 + dt_sqdxx.Rows[0]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[0]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";


                        if (tqbblb == "1")
                        {
                         string   BBLB_XML = "<BBLB>";
                            try
                            {
                                for (int x = 0; x < dt_bbxx2.Rows.Count; x++)
                                {
                                    try
                                    {
                                        BBLB_XML = BBLB_XML + "<row ";
                                        BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 +(x+1).ToString()+ (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bbxx2.Rows[x]["F_TMH"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bbxx2.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bbxx2.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "/>";
                                    }
                                    catch (Exception eee)
                                    {
                                        MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                        tqbblb = "0";
                                        break;
                                    }
                                }
                            }
                            catch (Exception e3)
                            {
                                MessageBox.Show("获取标本名称异常：" + e3.Message);
                                tqbblb = "0";
                            }
                            BBLB_XML = BBLB_XML + "</BBLB>";

                            if (tqbblb == "1")
                                xml = xml + BBLB_XML;
                        }

                     

                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "申请单号")
                {
                    #region 申请单号
                    string sqdh = Ssbz.Trim();
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                   
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_sqxh='" + sqdh + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此对应申请单记录(" + sqdh + ")"); return "0";
                    }

                    DataTable dt_bbxx2 = new DataTable();
                    dt_bbxx2 = aa.GetDataTable("select * from T_sqd_bbxx where F_sqxh='" + sqdh + "'", "bbxx");

                    //if (dt_bbxx2.Rows.Count <= 0)
                    //{
                    //    MessageBox.Show("无此标本条码信息"); return "0";
                    //}
                    //if (dt_bbxx2.Rows.Count > 1)
                    //{
                    //    if (tqbblb == "1")
                    //    {
                           
                    //            DataTable dt_yzxx = new DataTable();
                    //            dt_yzxx = aa.GetDataTable("select * from T_sqd_yzxx where F_sqxh='" + sqdh + "'", "yzxx");


                    //            bool yzbbxx = false;
                    //            Frm_CDRMYY cd = new Frm_CDRMYY(sqdh, dt_yzxx, dt_bbxx2, dt_sqdxx, Ssbz.Trim(), yzbbxx);
                    //            cd.ShowDialog();

                    //            if (cd.DialogResult == DialogResult.Yes)
                    //            {
                    //            }
                    //            else
                    //            {
                    //                if (yzbbxx)
                    //                {
                    //                    MessageBox.Show("标本带数量不正确,不能提取信息！");
                    //                    return "0";
                    //                }
                    //            }
                            
                    //    }
                    //}

                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[0]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[0]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[0]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[0]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[0]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[0]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[0]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_sqdxx.Rows[0]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[0]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "未婚"; break;
                            case "2": MARRIED = "已婚"; break;
                            default: MARRIED = ""; break;

                        }

                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[0]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[0]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[0]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[0]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[0]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[0]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_sqdxx.Rows[0]["F_SQYS"].ToString().Trim() + (char)34 + " ";

                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_sqdxx.Rows[0]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqdxx.Rows[0]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[0]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";

                        xml = xml + "病人类别=" + (char)34 + dt_sqdxx.Rows[0]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[0]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";


                        if (tqbblb == "1")
                        {
                          string   BBLB_XML = "<BBLB>";
                            try
                            {
                                for (int x = 0; x < dt_bbxx2.Rows.Count; x++)
                                {
                                    try
                                    {
                                        BBLB_XML = BBLB_XML + "<row ";
                                        BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + (x + 1).ToString() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_bbxx2.Rows[x]["F_TMH"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_bbxx2.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_bbxx2.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_bbxx2.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                        BBLB_XML = BBLB_XML + "/>";
                                    }
                                    catch (Exception eee)
                                    {
                                        MessageBox.Show("获取标本列表信息异常：" + eee.Message);
                                        tqbblb = "0";
                                        break;
                                    }
                                }
                            }
                            catch (Exception e3)
                            {
                                MessageBox.Show("获取标本名称异常：" + e3.Message);
                                tqbblb = "0";
                            }
                            BBLB_XML = BBLB_XML + "</BBLB>";

                            if (tqbblb == "1")
                                xml = xml + BBLB_XML;

                        }

                       
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "就诊号")
                {
                    #region 就诊号

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where f_jzh='" + Ssbz.Trim() + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此对应申请单记录(" + Ssbz.Trim() + ")"); return "0";
                    }


                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_zyh,F_BQ,F_CH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//显示的项目对应字段
                        string ColumnsName = "申请单号,病人类别,姓名,性别,年龄,住院号,病区,床号,送检科室,送检医生,申请日期,项目名称";//显示的项目名称
                        string xsys = "1"; //选择条件的项目
                        DataColumn dc0 = new DataColumn("序号");
                        dt_sqdxx.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqdxx.Rows.Count; x++)
                        {
                            dt_sqdxx.Rows[x][dt_sqdxx.Columns.Count - 1] = x;
                        }
                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请项目！");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);

                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请项目！");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("请重新选择申请项目！");
                            return "0";
                        }
                    }
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "未婚"; break;
                            case "2": MARRIED = "已婚"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "住院号(新)" || Sslbx == "住院号新" || Sslbx == "住院号" || Sslbx == "新住院号")
                {
                    #region 住院号

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_zyh='" + Ssbz.Trim() + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此对应申请单记录(" + Ssbz.Trim() + ")"); return "0";
                    }


                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_zyh,F_BQ,F_CH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//显示的项目对应字段
                        string ColumnsName = "申请单号,病人类别,姓名,性别,年龄,住院号,病区,床号,送检科室,送检医生,申请日期,项目名称";//显示的项目名称
                        string xsys = "1"; //选择条件的项目
                        DataColumn dc0 = new DataColumn("序号");
                        dt_sqdxx.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqdxx.Rows.Count; x++)
                        {
                            dt_sqdxx.Rows[x][dt_sqdxx.Columns.Count - 1] = x;
                        }
                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请项目！");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);

                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请项目！");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("请重新选择申请项目！");
                            return "0";
                        }
                    }
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "未婚"; break;
                            case "2": MARRIED = "已婚"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "卡号" || Sslbx == "门诊号")
                {
                    #region 卡号

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_mzh='" +  Ssbz.Trim() + "' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此对应申请单记录(" + Ssbz.Trim() + ")"); return "0";
                    }

                  
                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_MZH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//显示的项目对应字段
                        string ColumnsName = "申请单号,病人类别,姓名,性别,年龄,门诊号,送检科室,送检医生,申请日期,项目名称";//显示的项目名称
                        string xsys = "1"; //选择条件的项目
                        DataColumn dc0 = new DataColumn("序号");
                        dt_sqdxx.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqdxx.Rows.Count; x++)
                        {
                            dt_sqdxx.Rows[x][dt_sqdxx.Columns.Count - 1] = x;
                        }
                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请项目！");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);
                                
                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请项目！");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("请重新选择申请项目！");
                            return "0";
                        }
                    }
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "未婚"; break;
                            case "2": MARRIED = "已婚"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                if (Sslbx == "ID号"||Sslbx == "门诊ID号")
                {
                    #region ID号

                    int xh = 0;

                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    DataTable dt_sqdxx = new DataTable();
                    dt_sqdxx = aa.GetDataTable("select * from T_sqd where F_brbh='000" + Ssbz.Trim() + "00' and (F_djzt='' or f_djzt is null) ", "sqdxx");

                    if (dt_sqdxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("无此对应申请单记录(" + Ssbz.Trim() + ")"); return "0";
                    }

                    if (dt_sqdxx.Rows.Count > 1)
                    {
                        string Columns = "F_SQXH,f_brlb,F_XM,F_XB,F_NL,F_MZH,F_SQKS,F_SQYS,F_SQRQ,F_YZXMMC";//显示的项目对应字段
                        string ColumnsName = "申请单号,病人类别,姓名,性别,年龄,门诊号,送检科室,送检医生,申请日期,项目名称";//显示的项目名称
                        string xsys = "1"; //选择条件的项目
                        DataColumn dc0 = new DataColumn("序号");
                        dt_sqdxx.Columns.Add(dc0);
                        for (int x = 0; x < dt_sqdxx.Rows.Count; x++)
                        {
                            dt_sqdxx.Rows[x][dt_sqdxx.Columns.Count - 1] = x;
                        }
                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqdxx, Columns, ColumnsName, xsys);
                        yc.ShowDialog();
                        if (yc.DialogResult == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请项目！");
                                return "0";
                            }
                            try
                            {
                                xh = int.Parse(rtn2);

                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请项目！");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("请重新选择申请项目！");
                            return "0";
                        }
                    }
                    //-返回xml----------------------------------------------------
                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRBH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_sqdxx.Rows[xh]["F_jzcs"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQXH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + dt_sqdxx.Rows[xh]["F_ZYH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "姓名=" + (char)34 + dt_sqdxx.Rows[xh]["F_XM"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_sqdxx.Rows[xh]["F_XB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dt_sqdxx.Rows[xh]["F_NL"].ToString().Trim() + (char)34 + " ";

                        string MARRIED = dt_sqdxx.Rows[xh]["F_HY"].ToString().Trim();
                        switch (MARRIED)
                        {
                            case "1": MARRIED = "未婚"; break;
                            case "2": MARRIED = "已婚"; break;
                            default: MARRIED = ""; break;

                        }
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_sqdxx.Rows[xh]["F_DH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_sqdxx.Rows[xh]["F_BQ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_sqdxx.Rows[xh]["F_CH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_sqdxx.Rows[xh]["F_SFZH"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt_sqdxx.Rows[xh]["F_MZ"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQKS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_sqdxx.Rows[xh]["F_SQYS"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_sqdxx.Rows[xh]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + dt_sqdxx.Rows[xh]["F_YZXMBM"].ToString().Trim() + "^" + dt_sqdxx.Rows[xh]["F_YZXMMC"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt_sqdxx.Rows[xh]["F_BRLB"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_sqdxx.Rows[xh]["F_LCZD"].ToString().Trim() + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";
                        return xml;

                    }
                    catch (Exception ee)
                    {

                        MessageBox.Show(ee.Message.ToString());
                        return "0";
                    }
                    #endregion

                }
                else
                {
                    MessageBox.Show("无此" + Sslbx);
                    return "0";
                }

            }
            else
            {
                MessageBox.Show("无此" + Sslbx);
                if (Debug == "1")
                    log.WriteMyLog(Sslbx + Ssbz + "不存在！");

                return "0";
            }

             }
        public  static string xmlstr()
        {
            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";
            xml = xml + "病人编号=" + (char)34 + (char)34 + " ";
            xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "申请序号=" + (char)34 + (char)34 + " ";
            xml = xml + "门诊号=" + (char)34 + (char)34 + " ";
            xml = xml + "住院号=" + (char)34 + (char)34 + " ";
            xml = xml + "姓名=" + (char)34 + (char)34 + " ";

            xml = xml + "性别=" + (char)34 + (char)34 + " ";

            xml = xml + "年龄=" + (char)34 + (char)34 + " ";
            xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
            xml = xml + "地址=" + (char)34 + (char)34 + "   ";
            xml = xml + "电话=" + (char)34 + (char)34 + " ";
            xml = xml + "病区=" + (char)34 + (char)34 + " ";
            xml = xml + "床号=" + (char)34 + (char)34 + " ";
            xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
            xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
            xml = xml + "职业=" + (char)34 + (char)34 + " ";
            xml = xml + "送检科室=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医生=" + (char)34 + (char)34 + " ";
            //xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            //xml = xml + "送检医生=" + (char)34 +"" + (char)34 + " ";

            //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
            //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 + (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            xml = xml + "费别=" + (char)34 + (char)34 + " ";

            xml = xml + "病人类别=" + (char)34 + (char)34 + " ";
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + "]]></临床诊断>";
            xml = xml + "</LOGENE>";
            return xml;
        }
      }
}

