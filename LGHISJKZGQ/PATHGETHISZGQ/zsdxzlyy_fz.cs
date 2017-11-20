using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Data;
using dbbase;
using ZgqClassPub.DBData;

namespace LGHISJKZGQ
{
    class zsdxzlyy_fz
    {

        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static string ptxml(string Sslbx, string Ssbz, string debug)
        {

            debug = f.ReadString(Sslbx, "debug", "");
            string msg = f.ReadString(Sslbx, "msg", "");

            //先尝试从平台推送的数据中获取
            var ptXml = GetSQDxx(Sslbx, Ssbz, debug);
            if (ptXml != "" && ptXml != "0")
                return ptXml;

            if (Sslbx != "")
            {
                if (Sslbx == "病理医嘱列表")
                {
                    zsdxzlyy_FZYZ_FRM zszl = new zsdxzlyy_FZYZ_FRM();
                    if (zszl.ShowDialog() == DialogResult.Yes)
                    {
                        var yz = GetTestItem();
                        var xml = zszl.GetLogeneXml.Replace("--yz--",yz);
                        return xml;
                    }
                    return "0";
                }

                #region 根据LIS识别号查询

                if (Sslbx == "LIS识别号")
                {
                    DataTable dtTj = GetTjxx(Sslbx, Ssbz);
                    if (dtTj != null && dtTj.Rows.Count > 0)
                    {
                        #region 根据体检号查询,生成并返回xml

                        var dr = dtTj.Rows[0];

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dr["病人编号"] + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dr["就诊ID"] + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dr["申请序号"] + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + "门诊号" + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "住院号" + (char)34 + " ";

                        xml = xml + "姓名=" + (char)34 + dr["姓名"] + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dr["性别"] + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dr["年龄"] + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + dr["婚姻"] + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + dr["地址"] + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dr["电话"] + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dr["病区"] + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dr["床号"] + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dr["身份证号"] + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dr["民族"] + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + dr["职业"] + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dr["送检科室"] + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dr["送检医生"] + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + dr["收费"] + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dr["标本名称"] + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + dr["送检医院"] + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + GetTestItem() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + dr["备用1"] + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + dr["备用2"] + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dr["费别"] + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dr["病人类别"] + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + dr["临床病史"] + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dr["临床诊断"] + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        if (debug == "1")
                            log.WriteMyLog(xml);
                        return xml;

                        #endregion
                    }

                    DataTable dtTjtm = GetTjtmxx(Sslbx, Ssbz);
                    if (dtTjtm != null && dtTjtm.Rows.Count > 0)
                    {
                        #region 根据体检条码查询信息,生成并返回xml
                        var dr = dtTjtm.Rows[0];

                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dr["病人编号"] + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dr["就诊ID"] + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + dr["申请序号"] + (char)34 + " ";
                        xml = xml + "门诊号=" + (char)34 + "门诊号" + (char)34 + " ";
                        xml = xml + "住院号=" + (char)34 + "住院号" + (char)34 + " ";

                        xml = xml + "姓名=" + (char)34 + dr["姓名"] + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dr["性别"] + (char)34 + " ";
                        xml = xml + "年龄=" + (char)34 + dr["年龄"] + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + dr["婚姻"] + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + dr["地址"] + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dr["电话"] + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dr["病区"] + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dr["床号"] + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dr["身份证号"] + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dr["民族"] + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + dr["职业"] + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dr["送检科室"] + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dr["送检医生"] + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + dr["收费"] + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dr["标本名称"] + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + dr["送检医院"] + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + GetTestItem() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + dr["备用1"] + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + dr["备用2"] + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dr["费别"] + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dr["病人类别"] + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + dr["临床病史"] + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dr["临床诊断"] + "]]></临床诊断>";
                        xml = xml + "</LOGENE>";

                        if (debug == "1")
                            log.WriteMyLog(xml);
                        return xml;
                        #endregion
                    }

                    #region 根据条码号\医嘱号,生成并返回xml

                    Ssbz = Ssbz.ToUpper();
                    DataTable dt_yzxx = new DataTable();
                    DataTable dt_hisbrxx = new DataTable();
                    DataTable dt_Lis = new DataTable();

                    //通过标本瓶子上的条码号取LIS系统取信息
                    dt_Lis = GetLisxx(Sslbx, Ssbz);

                    if (dt_Lis.Rows.Count <= 0)
                    {
                        MessageBox.Show("LIS系统中未查询到此条码标本信息");
                        return "0";
                    }
                    //通过LIS系统中的病历号查询HIS基本信息
                    dt_hisbrxx = GetHisBrxx("病历号", dt_Lis.Rows[0]["bc_in_no"].ToString().Trim());

                    //通过LIS系统中的病历号查询HIS医嘱信息，并判断LIS系统中的医嘱项目和HIS的医嘱项目是否相同
                    DataTable dt_hisyzxx = GetHisYzxx("病历号", dt_Lis.Rows[0]["bc_in_no"].ToString().Trim());
                    DataView dv = dt_hisyzxx.DefaultView;
                    dv.RowFilter = "ITEM_NAME='" + dt_Lis.Rows[0]["bc_his_name"].ToString().Trim() + "'";
                    if (dv.Count > 0)
                        dt_yzxx = dv.ToTable();
                    else
                        dt_yzxx = dt_hisyzxx;

                    int count = 0;

                    #region  HIS中未查询到病人基本数据，取LIS中数据

                    if (dt_hisbrxx.Rows.Count <= 0)
                    {
                        if (msg == "1")
                            MessageBox.Show("HIS系统中未查询到此病人信息");
                        try
                        {
                            //lis医嘱,如果有分子病理的医嘱,直接取,否则弹窗选择
                            var drsFzyz= new List<DataRow>();
                            string lisYz = "";
                            foreach (DataRow row in dt_Lis.Rows)
                            {
                                if (row["type_name"].ToString() == "分子诊断室")
                                {
                                    drsFzyz.Add(row);
                                }
                            }

                            //当获取到的LIS分子医嘱大于1,则弹框让医生选择,否则直接取唯一的一条.
                            //如果一条都没有,则弹框让医生在常用词中选择
                            if (drsFzyz.Count > 1)
                            {
                                //处理当LIS里面分子医嘱大于1的情况
                                var cycItems = new List<CYC_Item>();
                                foreach (DataRow dataRow in drsFzyz)
                                {
                                    cycItems.Add(new CYC_Item {CYC_MC = dataRow["bc_his_name"].ToString()});
                                }
                                TestItemSelector fs = new TestItemSelector();
                                fs.CycItems = cycItems;
                                var r = fs.ShowDialog();
                                if (r == DialogResult.OK)
                                    lisYz = fs.SelectedTestItemName;
                            }
                            else if (drsFzyz.Count == 1)
                            {
                                lisYz = drsFzyz[0]["bc_his_name"].ToString();
                            }
                            else
                                lisYz = GetTestItem();

                            string xml = "<?xml version=" + (char) 34 + "1.0" + (char) 34 + " encoding=" + (char) 34 +
                                         "gbk" + (char) 34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char) 34 + dt_Lis.Rows[count]["bc_pid"].ToString() + (char) 34 + " ";
                            xml = xml + "就诊ID=" + (char) 34 + dt_Lis.Rows[count]["bc_bar_code"].ToString() + (char) 34 +
                                  " ";
                            xml = xml + "申请序号=" + (char) 34 + "" + (char) 34 + " ";
                            string brlb = dt_Lis.Rows[count]["state"].ToString().Trim();
                            if (brlb == "门诊")
                            {
                                xml = xml + "门诊号=" + (char) 34 + dt_Lis.Rows[count]["bc_in_no"].ToString().Trim() +
                                      (char) 34 + " ";
                                xml = xml + "住院号=" + (char) 34 + "" + (char) 34 + " ";
                            }
                            else
                            {
                                xml = xml + "门诊号=" + (char) 34 + "" + (char) 34 + " ";
                                xml = xml + "住院号=" + (char) 34 + dt_Lis.Rows[count]["bc_in_no"].ToString().Trim() +
                                      (char) 34 + " ";
                            }
                            xml = xml + "姓名=" + (char) 34 + dt_Lis.Rows[count]["bc_name"].ToString().Trim() + (char) 34 +
                                  " ";
                            xml = xml + "性别=" + (char) 34 + dt_Lis.Rows[count]["bc_sex"].ToString().Trim() + (char) 34 +
                                  " ";
                            string age = dt_Lis.Rows[count]["age"].ToString().Trim();
                            xml = xml + "年龄=" + (char) 34 + age + (char) 34 + " ";
                            xml = xml + "婚姻=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "地址=" + (char) 34 + "" + (char) 34 + "   ";
                            xml = xml + "电话=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "病区=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "床号=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "身份证号=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "民族=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "职业=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "送检科室=" + (char) 34 + dt_Lis.Rows[count]["bc_d_name"].ToString().Trim() +
                                  (char) 34 + " ";
                            xml = xml + "送检医生=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "收费=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "标本名称=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "送检医院=" + (char) 34 + "本院" + (char) 34 + " ";
                            //                            xml = xml + "医嘱项目=" + (char) 34 + dt_Lis.Rows[count]["bc_his_name"].ToString().Trim() +
                            //                                  (char) 34 + " ";
                            
                            //通过lis获取到医嘱时,取LIS医嘱,不弹框选择
                            xml = xml + "医嘱项目=" + (char) 34 + lisYz +
                                  (char) 34 + " ";

                            xml = xml + "备用1=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "备用2=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "费别=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "病人类别=" + (char) 34 + brlb + (char) 34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            if (debug == "1")
                                log.WriteMyLog(xml);
                            return xml;
                        }
                        catch (Exception eee)
                        {
                            MessageBox.Show(eee.Message);
                        }
                        return "0";
                    }

                    #endregion

                    #region  HIS中未查询到医嘱，取LIS中数据和HIS病人基本信息

                    if (dt_yzxx.Rows.Count <= 0)
                    {
                        if (msg == "1")
                            MessageBox.Show("HIS系统中未查询到此病人的医嘱信息");
                        try
                        {
                            string xml = "<?xml version=" + (char) 34 + "1.0" + (char) 34 + " encoding=" + (char) 34 +
                                         "gbk" + (char) 34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char) 34 + dt_Lis.Rows[count]["bc_pid"].ToString() + (char) 34 + " ";
                            xml = xml + "就诊ID=" + (char) 34 + dt_Lis.Rows[count]["bc_bar_code"].ToString() + (char) 34 +
                                  " ";
                            xml = xml + "申请序号=" + (char) 34 + "" + (char) 34 + " ";
                            string brlb = dt_Lis.Rows[count]["state"].ToString().Trim();
                            if (brlb == "门诊")
                            {
                                xml = xml + "门诊号=" + (char) 34 + dt_Lis.Rows[count]["bc_in_no"].ToString().Trim() +
                                      (char) 34 + " ";
                                xml = xml + "住院号=" + (char) 34 + "" + (char) 34 + " ";
                            }
                            else
                            {
                                xml = xml + "门诊号=" + (char) 34 + "" + (char) 34 + " ";
                                xml = xml + "住院号=" + (char) 34 + dt_Lis.Rows[count]["bc_in_no"].ToString().Trim() +
                                      (char) 34 + " ";
                            }
                            xml = xml + "姓名=" + (char) 34 + dt_Lis.Rows[count]["bc_name"].ToString().Trim() + (char) 34 +
                                  " ";
                            xml = xml + "性别=" + (char) 34 + dt_Lis.Rows[count]["bc_sex"].ToString().Trim() + (char) 34 +
                                  " ";
                            string age = dt_Lis.Rows[count]["age"].ToString().Trim();
                            xml = xml + "年龄=" + (char) 34 + age + (char) 34 + " ";
                            xml = xml + "婚姻=" + (char) 34 + dt_hisbrxx.Rows[0]["marry"].ToString() + (char) 34 + " ";
                            xml = xml + "地址=" + (char) 34 + "" + (char) 34 + "   ";
                            xml = xml + "电话=" + (char) 34 + dt_hisbrxx.Rows[0]["tel"].ToString() + (char) 34 + " ";
                            xml = xml + "病区=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "床号=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "身份证号=" + (char) 34 + dt_hisbrxx.Rows[0]["idenno"].ToString() + (char) 34 + " ";
                            xml = xml + "民族=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "职业=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "送检科室=" + (char) 34 + dt_Lis.Rows[count]["bc_d_name"].ToString().Trim() +
                                  (char) 34 + " ";
                            xml = xml + "送检医生=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "收费=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "标本名称=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "送检医院=" + (char) 34 + "本院" + (char) 34 + " ";
                            //                            xml = xml + "医嘱项目=" + (char) 34 + dt_Lis.Rows[count]["bc_his_name"].ToString().Trim() +
                            //                                  (char) 34 + " ";
                            xml = xml + "医嘱项目=" + (char) 34 + GetTestItem() + (char) 34 + " ";
                            xml = xml + "备用1=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "备用2=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "费别=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "病人类别=" + (char) 34 + brlb + (char) 34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            if (debug == "1")
                                log.WriteMyLog(xml);
                            return xml;
                        }
                        catch (Exception eee)
                        {
                            MessageBox.Show(eee.Message);
                        }
                        return "0";
                    }

                    #endregion

                    #region  有医嘱信息，取医嘱信息和HIS信息

                    string Columns = f.ReadString(Sslbx, "Columns",
                        "APPLY_DATE,PAT_TYPE,ITEM_CODE,ITEM_NAME,APPLY_DEPT,DOCT_NAME");
                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,类别,项目代码,项目名称,科室,医生");
                    int tkts = f.ReadInteger(Sslbx, "tkts", 1);

                    if (dt_yzxx.Rows.Count > tkts)
                    {
                        string xsys = f.ReadString(Sslbx, "xsys", "2"); //选择条件的项目

                        DataColumn dc0 = new DataColumn("序号");
                        dt_yzxx.Columns.Add(dc0);

                        for (int x = 0; x < dt_yzxx.Rows.Count; x++)
                        {
                            dt_yzxx.Rows[x][dt_yzxx.Columns.Count - 1] = x;
                        }

                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_yzxx, Columns, ColumnsName, xsys);
                        if (yc.ShowDialog() == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请项目！");
                                return "0";
                            }
                            try
                            {
                                count = int.Parse(rtn2);
                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请项目！");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("未选择申请项目！");
                            return "0";
                        }
                    }

                    try
                    {
                        string xml = "<?xml version=" + (char) 34 + "1.0" + (char) 34 + " encoding=" + (char) 34 + "gbk" +
                                     (char) 34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char) 34 + dt_hisbrxx.Rows[0]["PATIENTID"].ToString() + (char) 34 + " ";
                        xml = xml + "就诊ID=" + (char) 34 + dt_hisbrxx.Rows[count]["ic_cardno"].ToString().Trim() +
                              (char) 34 + " ";
                        xml = xml + "申请序号=" + (char) 34 + "" + (char) 34 + " ";
                        string brlb = dt_yzxx.Rows[count]["PAT_TYPE"].ToString().Trim();
                        if (brlb == "住院")
                        {
                            xml = xml + "门诊号=" + (char) 34 + "" + (char) 34 + " ";
                            xml = xml + "住院号=" + (char) 34 + dt_hisbrxx.Rows[0]["CARD_NO"].ToString().Trim() + (char) 34 +
                                  " ";
                        }
                        else
                        {
                            xml = xml + "门诊号=" + (char) 34 + dt_hisbrxx.Rows[0]["CARD_NO"].ToString().Trim() + (char) 34 +
                                  " ";
                            xml = xml + "住院号=" + (char) 34 + "" + (char) 34 + " ";
                        }
                        xml = xml + "姓名=" + (char) 34 + dt_hisbrxx.Rows[0]["NAME"].ToString().Trim() + (char) 34 + " ";
                        xml = xml + "性别=" + (char) 34 + dt_hisbrxx.Rows[0]["SEX"].ToString().Trim() + (char) 34 + " ";
                        string age = dt_Lis.Rows[count]["age"].ToString().Trim();
                        xml = xml + "年龄=" + (char) 34 + age + (char) 34 + " ";
                        xml = xml + "婚姻=" + (char) 34 + dt_hisbrxx.Rows[0]["MARRY"].ToString().Trim() + (char) 34 + " ";
                        xml = xml + "地址=" + (char) 34 + "" + (char) 34 + "   ";
                        xml = xml + "电话=" + (char) 34 + dt_hisbrxx.Rows[0]["TEL"].ToString().Trim() + (char) 34 + " ";
                        xml = xml + "病区=" + (char) 34 + dt_yzxx.Rows[count]["DOCT_DEPT"].ToString().Trim() + (char) 34 +
                              " ";
                        xml = xml + "床号=" + (char) 34 + dt_yzxx.Rows[count]["BED_NO"].ToString().Trim() + (char) 34 +
                              " ";
                        xml = xml + "身份证号=" + (char) 34 + dt_hisbrxx.Rows[0]["IDENNO"].ToString().Trim() + (char) 34 +
                              " ";
                        xml = xml + "民族=" + (char) 34 + "" + (char) 34 + " ";
                        xml = xml + "职业=" + (char) 34 + dt_yzxx.Rows[0]["clinic_code"].ToString() + (char) 34 + " ";
                        xml = xml + "送检科室=" + (char) 34 + dt_yzxx.Rows[count]["APPLY_DEPT"].ToString().Trim() +
                              (char) 34 + " ";
                        xml = xml + "送检医生=" + (char) 34 + dt_yzxx.Rows[count]["DOCT_NAME"].ToString().Trim() + (char) 34 +
                              " ";
                        xml = xml + "收费=" + (char) 34 + "" + (char) 34 + " ";
                        xml = xml + "标本名称=" + (char) 34 + dt_yzxx.Rows[count]["SAMPLE_NAME"].ToString().Trim() +
                              (char) 34 + " ";
                        xml = xml + "送检医院=" + (char) 34 + "本院" + (char) 34 + " ";
//                        xml = xml + "医嘱项目=" + (char) 34 + dt_yzxx.Rows[count]["ITEM_CODE"].ToString().Trim() + "^" +
//                              dt_yzxx.Rows[count]["ITEM_NAME"].ToString().Trim() + (char) 34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + GetTestItem() + (char)34 + " ";
                        xml = xml + "备用1=" + (char) 34 + "" + (char) 34 + " ";
                        xml = xml + "备用2=" + (char) 34 + "" + (char) 34 + " ";
                        xml = xml + "费别=" + (char) 34 + dt_yzxx.Rows[count]["PACT_NAME"].ToString().Trim() + (char) 34 +
                              " ";
                        xml = xml + "病人类别=" + (char) 34 + brlb + (char) 34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + dt_yzxx.Rows[count]["CLINIC_DIAGNOSE"].ToString().Trim() +
                              "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_yzxx.Rows[count]["CLINIC_HISTORY"].ToString().Trim() +
                              "]]></临床诊断>";

                        xml = xml + "</LOGENE>";

                        if (debug == "1")
                            log.WriteMyLog(xml);
                        return xml;
                    }
                    catch
                    {
                    }

                    #endregion

                    #endregion

                    //都没找到,则返回0 并且弹窗提示用户
                    MessageBox.Show("没有找到病人信息!");
                    return "0";
                }
                #endregion

                if (Sslbx == "病历号")
                {
                    Ssbz = Ssbz.ToUpper();
                    DataTable dt_yzxx = new DataTable();
                    DataTable dt_hisbrxx = new DataTable();
                  
                        //查询HIS基本信息
                    dt_hisbrxx = GetHisBrxx(Sslbx, Ssbz);

                        //查询HIS医嘱信息
                    DataTable dt_hisyzxx = GetHisYzxx(Sslbx, Ssbz);
                    dt_yzxx = dt_hisyzxx;

                    int count = 0;

                    #region  HIS中未查询到病人基本数据，取LIS中数据
                    if (dt_hisbrxx.Rows.Count <= 0)
                    {
                        MessageBox.Show("HIS系统中未查询到病人信息");
                        return "0";
                    }
                    #endregion

                    #region  HIS中未查询到医嘱，取HIS病人基本信息
                    if (dt_yzxx.Rows.Count <= 0)
                    {

                        if (msg == "1")
                            MessageBox.Show("HIS系统中未查询到此病人的医嘱信息");
                        try
                        {
                            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                            xml = xml + "<LOGENE>";
                            xml = xml + "<row ";
                            xml = xml + "病人编号=" + (char)34 + dt_hisbrxx.Rows[count]["patientid"].ToString() + (char)34 + " ";
                            xml = xml + "就诊ID=" + (char)34 + dt_hisbrxx.Rows[count]["ic_cardno"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + dt_hisbrxx.Rows[count]["card_no"].ToString().Trim() + (char)34 + " ";

                            xml = xml + "姓名=" + (char)34 + dt_hisbrxx.Rows[count]["name"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "性别=" + (char)34 + dt_hisbrxx.Rows[count]["sex"].ToString().Trim() + (char)34 + " ";
                            string age =ZGQClass.CsrqToAge(dt_hisbrxx.Rows[count]["birthday"].ToString().Trim());
                            xml = xml + "年龄=" + (char)34 + age + (char)34 + " ";
                            xml = xml + "婚姻=" + (char)34 + dt_hisbrxx.Rows[0]["marry"].ToString() + (char)34 + " ";
                            xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                            xml = xml + "电话=" + (char)34 + dt_hisbrxx.Rows[0]["tel"].ToString() + (char)34 + " ";
                            xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "身份证号=" + (char)34 + dt_hisbrxx.Rows[0]["idenno"].ToString() + (char)34 + " ";
                            xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                            xml = xml + "医嘱项目=" + (char)34 + GetTestItem()+ (char)34 + " ";
                            xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "/>";
                            xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                            xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                            xml = xml + "</LOGENE>";

                            if (debug == "1")
                                log.WriteMyLog(xml);
                            return xml;
                        }
                        catch (Exception eee)
                        {
                            MessageBox.Show(eee.Message);
                        }
                        return "0";
                    }
                    #endregion

                    #region  有医嘱信息，取医嘱信息和HIS信息

                    string Columns = f.ReadString(Sslbx, "Columns", "APPLY_DATE,PAT_TYPE,ITEM_CODE,ITEM_NAME,APPLY_DEPT,DOCT_NAME");
                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,类别,项目代码,项目名称,科室,医生");
                    int tkts = f.ReadInteger(Sslbx, "tkts", 1);

                    if (dt_yzxx.Rows.Count > tkts)
                    {
                        string xsys = f.ReadString(Sslbx, "xsys", "2"); //选择条件的项目

                        DataColumn dc0 = new DataColumn("序号");
                        dt_yzxx.Columns.Add(dc0);

                        for (int x = 0; x < dt_yzxx.Rows.Count; x++)
                        {
                            dt_yzxx.Rows[x][dt_yzxx.Columns.Count - 1] = x;
                        }

                        if (Columns.Trim() != "")
                            Columns = "序号," + Columns;
                        if (ColumnsName.Trim() != "")
                            ColumnsName = "序号," + ColumnsName;

                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_yzxx, Columns, ColumnsName, xsys);
                        if (yc.ShowDialog() == DialogResult.Yes)
                        {
                            string rtn2 = yc.F_XH;
                            if (rtn2.Trim() == "")
                            {
                                MessageBox.Show("未选择申请项目！");
                                return "0";
                            }
                            try
                            {
                                count = int.Parse(rtn2);
                            }
                            catch
                            {
                                MessageBox.Show("请重新选择申请项目！");
                                return "0";
                            }
                        }
                        else
                        {
                            MessageBox.Show("未选择申请项目！");
                            return "0";
                        }
                    }

                    try
                    {
                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                        xml = xml + "<LOGENE>";
                        xml = xml + "<row ";
                        xml = xml + "病人编号=" + (char)34 + dt_hisbrxx.Rows[0]["PATIENTID"].ToString() + (char)34 + " ";
                        xml = xml + "就诊ID=" + (char)34 + dt_hisbrxx.Rows[0]["ic_cardno"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        string brlb = dt_yzxx.Rows[count]["PAT_TYPE"].ToString().Trim();
                        if (brlb == "住院")
                        {
                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + dt_hisbrxx.Rows[0]["CARD_NO"].ToString().Trim() + (char)34 + " ";
                        }
                        else
                        {
                            xml = xml + "门诊号=" + (char)34 + dt_hisbrxx.Rows[0]["CARD_NO"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }
                        xml = xml + "姓名=" + (char)34 + dt_hisbrxx.Rows[0]["NAME"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "性别=" + (char)34 + dt_hisbrxx.Rows[0]["SEX"].ToString().Trim() + (char)34 + " ";
                        string age = ZGQClass.CsrqToAge(dt_hisbrxx.Rows[0]["BIRTHDAY"].ToString().Trim());
                        xml = xml + "年龄=" + (char)34 + age + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + dt_hisbrxx.Rows[0]["MARRY"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt_hisbrxx.Rows[0]["TEL"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt_yzxx.Rows[count]["DOCT_DEPT"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt_yzxx.Rows[count]["BED_NO"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt_hisbrxx.Rows[0]["IDENNO"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + dt_yzxx.Rows[count]["clinic_code"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt_yzxx.Rows[count]["APPLY_DEPT"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt_yzxx.Rows[count]["DOCT_NAME"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + dt_yzxx.Rows[count]["SAMPLE_NAME"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + GetTestItem() + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dt_yzxx.Rows[count]["PACT_NAME"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + dt_yzxx.Rows[count]["CLINIC_DIAGNOSE"].ToString().Trim() + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt_yzxx.Rows[count]["CLINIC_HISTORY"].ToString().Trim() + "]]></临床诊断>";

                        xml = xml + "</LOGENE>";

                        if (debug == "1")
                            log.WriteMyLog(xml);
                        return xml;
                    }
                    catch(Exception e)
                    {
                        log.WriteMyLog(e.ToString());
                    }
                    #endregion
                    return "0";
                }
                else
                {
                    MessageBox.Show("无此识别类型" +Sslbx);
                    return "0";
                }
            }
            else
                MessageBox.Show("识别类型不能为空");
            return "0";


        }

        /// <summary>
        /// 弹出窗口让用户选择项目
        /// </summary>
        /// <returns></returns>
        private static string GetTestItem()
        {
            var getItem = f.ReadString("savetohis", "xzyz", "1");
            if (getItem != "1") return "";

            using (var selector = new TestItemSelector())
            {
                var r = selector.ShowDialog();
                if (r == DialogResult.OK)
                    return selector.SelectedTestItemName;
            }
            return "";
        }

        public static DataTable GetHisBrxx(string Sslbx, string zyh)
        {
                string  odbcHis= f.ReadString(Sslbx,"odbcHis","Provider=MSDAORA;Data Source=HISORACLE;User id=fzzd;Password=fzzd0422;");
                string hissql = f.ReadString(Sslbx, "hissql", "SELECT  * from neusoft.vw_fzzd_patientinfo where card_no = '00" + zyh + "' or card_no = '0000" + zyh + "'  ");
                string ErrMsg = "";
                OleDbDB db = new OleDbDB();
                DataTable dt_lis = db.DataAdapter(odbcHis, hissql, ref ErrMsg);
                if (ErrMsg != "")
                    MessageBox.Show("查询病人基本信息异常:" + ErrMsg);
                return dt_lis;
        }
        public static DataTable GetHisYzxx(string Sslbx, string zyh)
        {
            string odbcHis = f.ReadString(Sslbx, "odbcHis", "Provider=MSDAORA;Data Source=HISORACLE;User id=fzzd;Password=fzzd0422;");
            string hissql = f.ReadString(Sslbx, "yzsql", "SELECT * from neusoft.vw_fzzd_iteminfo where card_no = '00" + zyh + "' or card_no = '0000" + zyh + "'  ");

            hissql = hissql + " order by apply_date desc ";

            string ErrMsg = "";
            OleDbDB db = new OleDbDB();
            DataTable dt_lis = db.DataAdapter(odbcHis, hissql, ref ErrMsg);
            if (ErrMsg != "")
                MessageBox.Show("查询HIS医嘱信息异常:" + ErrMsg);
            return dt_lis;
        }

        public static DataTable GetLisxx(string Sslbx,string tmh)
        {
          string  odbcLis= f.ReadString(Sslbx,"odbcLis","Data Source=172.16.95.190\\SQL2005;Initial Catalog=clab;User Id=sa;Password=admin;");

          string Lissql = f.ReadString(Sslbx, "Lissql", "SELECT  top 20  a.bc_name , (CASE a.bc_sex WHEN 1 THEN '男' WHEN 2 THEN '女' ELSE '未知' END) bc_sex, dbo.getAge(a.bc_age) as age, a.bc_d_code,a.bc_d_name , a.bc_in_no , a.bc_social_no , CASE WHEN charindex('ZY',a.bc_social_no) > 0 THEN '住院' ELSE '门诊' END as state,a.bc_bar_code , (SELECT cuv_name FROM dict_cuvette WHERE cuv_code = a.bc_cuv_code) cuv_name, (SELECT type_name FROM dict_type WHERE type_id = a.bc_ctype ) type_name, a.bc_his_name ,(SELECT p.sam_name FROM dict_sample p WHERE p.sam_id = a.bc_sam_id) sam_name,'' item_code, a.bc_pid,a.bc_print_date bc_date FROM bc_patients a WHERE EXISTS  (SELECT 1 FROM (SELECT p.bc_print_date,p.bc_pid FROM bc_patients p WHERE p.bc_bar_code = '" + tmh + "') t WHERE a.bc_print_date = t.bc_print_date AND a.bc_pid = t.bc_pid) ORDER BY substring(a.bc_bar_code,len(a.bc_bar_code)-1,2) desc");

//Params=病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,年龄,出生日期,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史
//Values=bc_pid,bc_bar_code,,,bc_in_no,bc_name,bc_sex,age,,,,,,,,,,bc_d_name,,,,"本院",bc_his_name,,,,state,,
//ColumnsName=病人编号,条码号,姓名,性别,年龄,医嘱项目
//Columns=bc_pid,bc_bar_code,bc_name,bc_sex,age,bc_his_name
//count=20
//RowFilter=type_name='分子诊断室' or type_name='' or type_name is null
    string ErrMsg="";
           SqlDB db=new SqlDB();
           DataTable dt_lis = db.DataAdapter(odbcLis, Lissql, ref ErrMsg);
           if (ErrMsg != "")
               MessageBox.Show("查询LIS条码信息异常:" + ErrMsg);
           DataView dv = dt_lis.DefaultView;
           dv.RowFilter = "type_name='分子诊断室' or type_name='' or type_name is null";
           return dv.ToTable();
        }

        public static DataTable GetTjtmxx(string Sslbx, string tmh)
        {
            string odbcTjtm = f.ReadString(Sslbx, "odbcTjtm", @"Data Source=172.16.95.190\SQL2005;Initial Catalog=clab;User Id=lj;Password=lj;  ");

            string sql = f.ReadString(Sslbx, "Tjtmsql", "select bc_in_no as 病人编号,bc_bar_code as 就诊ID," +
                                                        "appcode as 申请序号,bc_in_no  as 门诊号,'' as 住院号," +
                                                        "bc_name as 姓名,case sex when '2' then '女' when '1' then '男' else '' end as 性别," +
                                                        "age as 年龄,'' as 出生日期,'' as 婚姻,'' as  地址,'' as 电话," +
                                                        "'' as 病区,'' as 床号,'' as 身份证号,'' as  民族,'' as  职业," +
                                                        "bc_d_name as 送检科室,bc_doct_name as 送检医生,'' as 收费," +
                                                        "'' as 标本名称,'本院' as 送检医院,item_code+'^'+item as 医嘱项目," +
                                                        "'' as 备用1,unit_name as 备用2,'' as 费别,'体检'  as 病人类别," +
                                                        "'' as 临床诊断,'' as 临床病史 " +
                                                        "from PatientInfoFORPIS  " +
                                                        $"where bc_bar_code='{tmh}'  ");

            log.WriteMyLog(sql);
            //Params=病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,年龄,出生日期,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史
            //Values=bc_pid,bc_bar_code,,,bc_in_no,bc_name,bc_sex,age,,,,,,,,,,bc_d_name,,,,"本院",bc_his_name,,,,state,,
            //ColumnsName=病人编号,条码号,姓名,性别,年龄,医嘱项目
            //Columns=bc_pid,bc_bar_code,bc_name,bc_sex,age,bc_his_name
            //count=20
            //RowFilter=type_name='分子诊断室' or type_name='' or type_name is null
            string ErrMsg = "";
            SqlDB db = new SqlDB();
            DataTable dt = db.DataAdapter(odbcTjtm, sql, ref ErrMsg);
            if (ErrMsg != "")
                MessageBox.Show("根据体检条码号查询信息异常:" + ErrMsg);
            DataView dv = dt.DefaultView;
            return dv.ToTable();
        }

        public static DataTable GetTjxx(string Sslbx, string tmh)
        {
            string odbcStr = f.ReadString(Sslbx, "odbcTj", @"Data Source=172.16.95.190\SQL2005;Initial Catalog=tj_zdzl;User Id=bl;Password=admin;");

            string sql = f.ReadString(Sslbx, "Tjsql", $"select res_check_no as 病人编号,'' as 就诊ID,res_no as 申请序号,res_check_no   as 门诊号,'' as 住院号," +
                                                      $"res_name as 姓名,res_sex as 性别,res_age+'岁'  as 年龄,'' as 出生日期,res_marrige  as 婚姻,'' as  地址," +
                                                      $"res_phone as 电话,'' as 病区,'' as 床号,Replace(res_idcard,'\"','') as 身份证号,res_native as  民族,res_job  as  职业," +
                                                      $"res_dept  as 送检科室,res_doctor_code_rq as 送检医生,'' as 收费,'' as 标本名称,'本院' as 送检医院,res_item_id+' ^ '+res_item  as 医嘱项目," +
                                                      $"'' as 备用1,res_unit as 备用2,'' as 费别,'体检'  as 病人类别,res_diagnose as 临床诊断,'' as 临床病史" +
                                                      $" from tj_v_exam_req_lj " +
                                                      $"WHERE res_check_no='{tmh}'  and res_deptname='病理科' order by res_no desc");

            log.WriteMyLog(sql);
            //Params=病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,年龄,出生日期,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史
            //Values=bc_pid,bc_bar_code,,,bc_in_no,bc_name,bc_sex,age,,,,,,,,,,bc_d_name,,,,"本院",bc_his_name,,,,state,,
            //ColumnsName=病人编号,条码号,姓名,性别,年龄,医嘱项目
            //Columns=bc_pid,bc_bar_code,bc_name,bc_sex,age,bc_his_name
            //count=20
            //RowFilter=type_name='分子诊断室' or type_name='' or type_name is null
            string ErrMsg = "";
            SqlDB db = new SqlDB();
            DataTable dt = db.DataAdapter(odbcStr, sql, ref ErrMsg);
            if (ErrMsg != "")
                MessageBox.Show("根据体检号查询信息异常:" + ErrMsg);
            DataView dv = dt.DefaultView;
            return dv.ToTable();
        }

        ///***************************************************************************
//        public static string ptxml20170818(string Sslbx, string Ssbz, string debug)
//        {
//
//            debug = f.ReadString(Sslbx, "debug", ""); //获取sz.ini中设置的mrks
//
//            if (Sslbx != "")
//            {
//                if (Sslbx == "病理医嘱列表")
//                {
//                    zsdxzlyy_FZYZ_FRM zszl = new zsdxzlyy_FZYZ_FRM();
//                    if (zszl.ShowDialog() == DialogResult.Yes)
//                    {
//                        return zszl.GetLogeneXml;
//                    }
//
//
//                    return "0";
//                }
//                if (Sslbx == "体检号")
//                {
//                    zsdxzlyy_FZYZ_FRM zszl = new zsdxzlyy_FZYZ_FRM();
//                    if (zszl.ShowDialog() == DialogResult.Yes)
//                    {
//                        return zszl.GetLogeneXml;
//                    }
//
//                    return "0";
//                }
//                if (Sslbx == "体检条码" || Sslbx == "体检条码号")
//                {
//                    zsdxzlyy_FZYZ_FRM zszl = new zsdxzlyy_FZYZ_FRM();
//                    if (zszl.ShowDialog() == DialogResult.Yes)
//                    {
//                        return zszl.GetLogeneXml;
//                    }
//
//                    return "0";
//                }
//
//                if (Sslbx == "条码号" || Sslbx == "标本条码号" || Sslbx == "病历号")
//                {
//                    Ssbz = Ssbz.ToUpper();
//                    DataTable dt_his = new DataTable();
//                    DataTable dt_Lis = new DataTable();
//                    if (Sslbx == "条码号" || Sslbx == "标本条码号")
//                    {
//                        dt_Lis = GetLis(Sslbx, Ssbz);
//
//                        if (dt_Lis.Rows.Count <= 0)
//                        {
//                            MessageBox.Show("LIS系统中未查询到此条码的记录");
//                            return "0";
//                        }
//                        DataTable dt_his2 = GetHis("病历号", dt_Lis.Rows[0]["bc_in_no"].ToString().Trim());
//                        DataView dv = dt_his2.DefaultView;
//                        dv.RowFilter = "ITEM_NAME='" + dt_Lis.Rows[0]["bc_his_name"].ToString().Trim() + "'";
//                        if (dv.Count > 0)
//                            dt_his = dv.ToTable();
//                        else
//                            dt_his = dt_his2;
//                    }
//                    else
//                    {
//                        dt_his = GetHis("病历号", Ssbz.Trim());
//                    }
//
//                    int count = 0;
//                    if (dt_his.Rows.Count <= 0)
//                    {
//
//                        MessageBox.Show("HIS系统中未查询到此病历号数据");
//                        if (Sslbx == "条码号")
//                        {
//                            try
//                            {
//                                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                                xml = xml + "<LOGENE>";
//                                xml = xml + "<row ";
//                                xml = xml + "病人编号=" + (char)34 + dt_Lis.Rows[count]["bc_pid"].ToString() + (char)34 + " ";
//                                xml = xml + "就诊ID=" + (char)34 + dt_Lis.Rows[count]["bc_bar_code"].ToString() + (char)34 + " ";
//                                xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
//                                string brlb = dt_Lis.Rows[count]["state"].ToString().Trim();
//                                if (brlb == "门诊")
//                                {
//                                    xml = xml + "门诊号=" + (char)34 + dt_Lis.Rows[count]["bc_in_no"].ToString().Trim() + (char)34 + " ";
//                                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                                }
//                                else
//                                {
//                                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
//                                    xml = xml + "住院号=" + (char)34 + dt_Lis.Rows[count]["bc_in_no"].ToString().Trim() + (char)34 + " ";
//                                }
//                                xml = xml + "姓名=" + (char)34 + dt_Lis.Rows[count]["bc_name"].ToString().Trim() + (char)34 + " ";
//                                xml = xml + "性别=" + (char)34 + dt_Lis.Rows[count]["bc_sex"].ToString().Trim() + (char)34 + " ";
//                                string age = dt_Lis.Rows[count]["age"].ToString().Trim();
//                                xml = xml + "年龄=" + (char)34 + age + (char)34 + " ";
//                                xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
//                                xml = xml + "电话=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "送检科室=" + (char)34 + dt_Lis.Rows[count]["bc_d_name"].ToString().Trim() + (char)34 + " ";
//                                xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                                xml = xml + "医嘱项目=" + (char)34 + GetTestItem()+ (char)34 + " ";
//                                xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
//                                xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
//                                xml = xml + "/>";
//                                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
//                                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
//                                xml = xml + "</LOGENE>";
//
//                                if (debug == "1")
//                                    log.WriteMyLog(xml);
//                                return xml;
//                            }
//                            catch (Exception eee)
//                            {
//                                MessageBox.Show(eee.Message);
//                            }
//                        }
//
//                        return "0";
//                    }
//
//                    string Columns = f.ReadString(Sslbx, "Columns", "APPLY_DATE,CARD_NO,PAT_TYPE,NAME,SEX,BIRTHDAY,ITEM_CODE,ITEM_NAME,APPLY_DEPT,DOCT_NAME");
//                    string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,病历号,类别,姓名,性别,出生日期,项目代码,项目名称,科室,医生");
//                    int tkts = f.ReadInteger(Sslbx, "tkts", 1);
//
//                    if (dt_his.Rows.Count > tkts)
//                    {
//                        string xsys = f.ReadString(Sslbx, "xsys", "2"); //选择条件的项目
//                        DataColumn dc0 = new DataColumn("序号");
//                        dt_his.Columns.Add(dc0);
//
//                        for (int x = 0; x < dt_his.Rows.Count; x++)
//                        {
//                            dt_his.Rows[x][dt_his.Columns.Count - 1] = x;
//                        }
//
//                        if (Columns.Trim() != "")
//                            Columns = "序号," + Columns;
//                        if (ColumnsName.Trim() != "")
//                            ColumnsName = "序号," + ColumnsName;
//
//                        FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_his, Columns, ColumnsName, xsys);
//                        if (yc.ShowDialog() == DialogResult.Yes)
//                        {
//                            string rtn2 = yc.F_XH;
//                            if (rtn2.Trim() == "")
//                            {
//                                MessageBox.Show("未选择申请项目！");
//                                return "0";
//                            }
//                            try
//                            {
//                                count = int.Parse(rtn2);
//                            }
//                            catch
//                            {
//                                MessageBox.Show("请重新选择申请项目！");
//                                return "0";
//                            }
//                        }
//                        else
//                        {
//                            MessageBox.Show("未选择申请项目！");
//                            return "0";
//                        }
//                    }
//
//                    try
//                    {
//                        string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
//                        xml = xml + "<LOGENE>";
//                        xml = xml + "<row ";
//                        xml = xml + "病人编号=" + (char)34 + dt_his.Rows[count]["PATIENTID"].ToString() + (char)34 + " ";
//                        xml = xml + "就诊ID=" + (char)34 + dt_his.Rows[count]["IC_CARDNO"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
//                        string brlb = dt_his.Rows[count]["PAT_TYPE"].ToString().Trim();
//                        if (brlb == "住院")
//                        {
//                            xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
//                            xml = xml + "住院号=" + (char)34 + dt_his.Rows[count]["CARD_NO"].ToString().Trim() + (char)34 + " ";
//                        }
//                        else
//                        {
//                            xml = xml + "门诊号=" + (char)34 + dt_his.Rows[count]["CARD_NO"].ToString().Trim() + (char)34 + " ";
//                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
//                        }
//                        xml = xml + "姓名=" + (char)34 + dt_his.Rows[count]["NAME"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "性别=" + (char)34 + dt_his.Rows[count]["SEX"].ToString().Trim() + (char)34 + " ";
//                        string age = ZGQClass.CsrqToAge(dt_his.Rows[count]["BIRTHDAY"].ToString().Trim());
//                        xml = xml + "年龄=" + (char)34 + age + (char)34 + " ";
//                        xml = xml + "婚姻=" + (char)34 + dt_his.Rows[count]["MARRY"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
//                        xml = xml + "电话=" + (char)34 + dt_his.Rows[count]["TEL"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "病区=" + (char)34 + dt_his.Rows[count]["DOCT_DEPT"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "床号=" + (char)34 + dt_his.Rows[count]["BED_NO"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "身份证号=" + (char)34 + dt_his.Rows[count]["IDENNO"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "送检科室=" + (char)34 + dt_his.Rows[count]["APPLY_DEPT"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "送检医生=" + (char)34 + dt_his.Rows[count]["DOCT_NAME"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "标本名称=" + (char)34 + dt_his.Rows[count]["SAMPLE_NAME"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
//                        xml = xml + "医嘱项目=" + (char)34 + GetTestItem() + (char)34 + " ";
//                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
//                        xml = xml + "费别=" + (char)34 + dt_his.Rows[count]["PACT_NAME"].ToString().Trim() + (char)34 + " ";
//                        xml = xml + "病人类别=" + (char)34 + brlb + (char)34 + " ";
//                        xml = xml + "/>";
//                        xml = xml + "<临床病史><![CDATA[" + dt_his.Rows[count]["CLINIC_DIAGNOSE"].ToString().Trim() + "]]></临床病史>";
//                        xml = xml + "<临床诊断><![CDATA[" + dt_his.Rows[count]["CLINIC_HISTORY"].ToString().Trim() + "]]></临床诊断>";
//
//                        xml = xml + "</LOGENE>";
//
//                        if (debug == "1")
//                            log.WriteMyLog(xml);
//                        return xml;
//                    }
//                    catch
//                    {
//                    }
//                    return "0";
//                }
//                else
//                {
//                    MessageBox.Show("无此识别类型" + Sslbx);
//                    return "0";
//                }
//            }
//            else
//                MessageBox.Show("识别类型不能为空");
//            return "0";
//
//
//        }

        public static DataTable GetHis(string Sslbx, string blh)
        {
            string odbcHis = f.ReadString(Sslbx, "odbcHis", "Provider=MSDAORA;" + "Data Source=HISORACLE;" + "User id=fzzd;" + "Password=fzzd0422;");
            string hissql = f.ReadString(Sslbx, "hissql", "SELECT  * from neusoft.vw_fzzd_itemlist where card_no='" + blh + "'  ");

            hissql = hissql + " order by apply_date desc ";

            string ErrMsg = "";
            OleDbDB db = new OleDbDB();
            DataTable dt_lis = db.DataAdapter(odbcHis, hissql, ref ErrMsg);
            if (ErrMsg != "")
                MessageBox.Show("查询HIS异常:" + ErrMsg);
            return dt_lis;
        }
        public static DataTable GetLis(string Sslbx, string tmh)
        {
            string odbcLis = f.ReadString(Sslbx, "odbcLis", "Data Source=172.16.95.190\\SQL2005;Initial Catalog=clab;User Id=sa;Password=admin;");
            //todo:优化
            string Lissql = f.ReadString(Sslbx, "Lissql", "SELECT  top 20  a.bc_name , (CASE a.bc_sex WHEN 1 THEN '男' WHEN 2 THEN '女' ELSE '未知' END) bc_sex, dbo.getAge(a.bc_age) as age, a.bc_d_code,a.bc_d_name , a.bc_in_no , a.bc_social_no , CASE WHEN charindex('ZY',a.bc_social_no) > 0 THEN '住院' ELSE '门诊' END as state,a.bc_bar_code , (SELECT cuv_name FROM dict_cuvette WHERE cuv_code = a.bc_cuv_code) cuv_name, (SELECT type_name FROM dict_type WHERE type_id = a.bc_ctype ) type_name, a.bc_his_name ,(SELECT p.sam_name FROM dict_sample p WHERE p.sam_id = a.bc_sam_id) sam_name,'' item_code, a.bc_pid,a.bc_print_date bc_date FROM bc_patients a WHERE EXISTS  (SELECT 1 FROM (SELECT p.bc_print_date,p.bc_pid FROM bc_patients p WHERE p.bc_bar_code = '" + tmh + "') t WHERE a.bc_print_date = t.bc_print_date AND a.bc_pid = t.bc_pid) ORDER BY substring(a.bc_bar_code,len(a.bc_bar_code)-1,2) desc");

            //Params=病人编号,就诊ID,申请序号,门诊号,住院号,姓名,性别,年龄,出生日期,婚姻,地址,电话,病区,床号,身份证号,民族,职业,送检科室,送检医生,收费,标本名称,送检医院,医嘱项目,备用1,备用2,费别,病人类别,临床诊断,临床病史
            //Values=bc_pid,bc_bar_code,,,bc_in_no,bc_name,bc_sex,age,,,,,,,,,,bc_d_name,,,,"本院",bc_his_name,,,,state,,
            //ColumnsName=病人编号,条码号,姓名,性别,年龄,医嘱项目
            //Columns=bc_pid,bc_bar_code,bc_name,bc_sex,age,bc_his_name
            //count=20
            //RowFilter=type_name='分子诊断室' or type_name='' or type_name is null
            string ErrMsg = "";
            SqlDB db = new SqlDB();
            DataTable dt_lis = db.DataAdapter(odbcLis, Lissql, ref ErrMsg);
            if (ErrMsg != "")
                MessageBox.Show("查询LIS异常:" + ErrMsg);


            DataView dv = dt_lis.DefaultView;
            dv.RowFilter = "type_name='分子诊断室' or type_name='' or type_name is null";
            return dv.ToTable();
        }

        public static string GetSQDxx(string Sslbx, string Ssbz, string debug)
        {

            odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable dt_sqdbbxx = new DataTable();
            DataTable dt_sqd = new DataTable();
            string tqbblb = f.ReadString(Sslbx, "tqbblb", "0");
            if (Sslbx == "门诊号" || Sslbx == "住院号" || Sslbx == "诊疗卡" || Sslbx == "申请单号")
            {

                string sql = "";
                if (Sslbx == "门诊号")
                    sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_MZH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";

                if (Sslbx == "住院号")
                    sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_ZYH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";

                if (Sslbx == "诊疗卡")
                    sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_JZLSH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";

                if (Sslbx == "申请单号")
                    sql = "select F_brbh as 病人编号,F_BRLB  as 病人类别,F_FB as 费别,F_JZH as 住院号,'' as 门诊号,F_XM as 姓名 ,F_XB as 性别,F_NL as 年龄,F_HY as 婚姻, '' as 地址,F_DH as 电话,F_BQ AS 病区,F_CH as 床号,F_SFZH as 身份证号,F_MZ as 民族,F_ZY as 职业,F_SJKS as 送检科室,F_SJys AS 送检医生,F_LCZD as 临床诊断,F_LCZL as 临床病史,' ' as 收费,F_YZID as 就诊ID,F_SQXH as 申请序号,F_BBMC as 标本名称,'本院' AS 送检医院,F_YZXMBM+'^'+F_YZXMMC as 医嘱项目,'' as 出生日期,'' as 备用1,'' as 备用2,F_SQRQ as 申请日期,F_YZXMMC from [pathnet].[dbo].[T_SQD] WHERE  F_SQXH= 'f_sbh' and F_sqdzt!='aborted' and F_sqdzt!='已登记' order by F_ID desc";
                if (sql.Trim() == "")
                    return "0";
                string hissql = f.ReadString(Sslbx, "hissql", sql);

                string Columns = f.ReadString(Sslbx, "Columns", "申请日期,就诊ID,姓名,性别,年龄,送检科室,送检医生,病区,床号,F_YZXMMC");
                string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "申请日期,医嘱号,姓名,性别,年龄,送检科室,送检医生,病区,床号,医嘱项目");
                hissql = hissql.Replace("f_sbh", Ssbz.Trim());

                dt_sqd = aa.GetDataTable(hissql, "sqd");

                /////////////////////////////////////////////////////////////////////
                if (dt_sqd == null)
                {
                    MessageBox.Show("获取申请单信息错误：查询数据错误");
                    return "0";
                }
                if (dt_sqd.Rows.Count == 0)
                {
                    MessageBox.Show("未查询到申请单信息");
                    return "0";
                }

                int tkts = f.ReadInteger(Sslbx, "tkts", 1);
                int count = 0;
                if (dt_sqd.Rows.Count > tkts)
                {
                    string xsys = f.ReadString(Sslbx, "xsys", "1"); //选择条件的项目
                    DataColumn dc0 = new DataColumn("序号");
                    dt_sqd.Columns.Add(dc0);

                    for (int x = 0; x < dt_sqd.Rows.Count; x++)
                    {
                        dt_sqd.Rows[x][dt_sqd.Columns.Count - 1] = x;
                    }

                    if (Columns.Trim() != "")
                        Columns = "序号," + Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "序号," + ColumnsName;

                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt_sqd, Columns, ColumnsName, xsys);
                    yc.ShowDialog();
                    string rtn2 = yc.F_XH;
                    if (rtn2.Trim() == "")
                    {
                        MessageBox.Show("未选择申请项目！");
                        return "0";
                    }
                    try
                    {
                        count = int.Parse(rtn2);
                    }
                    catch
                    {
                        MessageBox.Show("请重新选择申请项目！");
                        return "0";
                    }

                }
                try
                {
                    string bbmc = "";



                    string BBLB_XML = "";
                    if (tqbblb == "1")
                    {
                        dt_sqdbbxx = aa.GetDataTable("select * from T_SQD_BBXX WHERE  F_SQXH= '" + dt_sqd.Rows[count]["申请序号"].ToString().Trim() + "'  order by F_ID", "bbxx");
                        string djr = f.ReadString("yh", "yhmc", "").Replace("\0", "").Trim();
                        if (dt_sqdbbxx == null)
                            MessageBox.Show("查询申请单标本失败");
                        else
                        {
                            if (dt_sqdbbxx.Rows.Count <= 0)
                                MessageBox.Show("未查询到此申请单标本记录");
                            else
                            {
                                BBLB_XML = "<BBLB>";
                                try
                                {
                                    for (int x = 0; x < dt_sqdbbxx.Rows.Count; x++)
                                    {
                                        try
                                        {
                                            BBLB_XML = BBLB_XML + "<row ";
                                            BBLB_XML = BBLB_XML + "F_BBXH=" + (char)34 + dt_sqdbbxx.Rows[x]["F_XH"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBTMH=" + (char)34 + dt_sqdbbxx.Rows[x]["F_BBH"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBMC=" + (char)34 + dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_CQBW=" + (char)34 + dt_sqdbbxx.Rows[x]["F_CQBW"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BZ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_LTSJ=" + (char)34 + dt_sqdbbxx.Rows[x]["F_LTSJ"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_GDSJ=" + (char)34 + dt_sqdbbxx.Rows[x]["F_GDSJ"].ToString().Trim() + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_JSSJ=" + (char)34 + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_JSY=" + (char)34 + djr + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBZT=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_BBPJ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_PJR=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "F_PJSJ=" + (char)34 + "" + (char)34 + " ";
                                            BBLB_XML = BBLB_XML + "/>";

                                            if (bbmc == "")
                                                bbmc = dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim();
                                            else
                                                bbmc = bbmc + "," + dt_sqdbbxx.Rows[x]["F_BBMC"].ToString().Trim();
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
                            }
                        }
                    }

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";
                    xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["就诊ID"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["申请序号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["门诊号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                    string xb = dt_sqd.Rows[count]["性别"].ToString().Trim();
                    if (xb == "F" || xb == "f")
                        xb = "女";
                    if (xb == "M" || xb == "m")
                        xb = "男";
                    xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                    xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                    xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["电话"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "民族=" + (char)34 + dt_sqd.Rows[count]["民族"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "职业=" + (char)34 + dt_sqd.Rows[count]["职业"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检科室=" + (char)34 + dt_sqd.Rows[count]["送检科室"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["送检医生"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "收费=" + (char)34 + dt_sqd.Rows[count]["收费"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "标本名称=" + (char)34 + bbmc.Trim() + (char)34 + " ";
                    xml = xml + "送检医院=" + (char)34 + dt_sqd.Rows[count]["送检医院"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用1=" + (char)34 + dt_sqd.Rows[count]["备用1"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "备用2=" + (char)34 + dt_sqd.Rows[count]["备用2"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "费别=" + (char)34 + dt_sqd.Rows[count]["费别"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "/>";
                    xml = xml + "<临床病史><![CDATA[" + dt_sqd.Rows[count]["临床病史"].ToString().Trim() + "]]></临床病史>";
                    xml = xml + "<临床诊断><![CDATA[" + dt_sqd.Rows[count]["临床诊断"].ToString().Trim() + "]]></临床诊断>";



                    if (tqbblb == "1")
                        xml = xml + BBLB_XML;
                    xml = xml + "</LOGENE>";

                    if (debug == "1")
                        log.WriteMyLog(xml);
                    return xml;


                }
                catch (Exception e)
                {
                    MessageBox.Show("获取申请单信息异常：" + e.Message);
                    return "0";
                }
            }
            else
            {
                return "0";
            }
        }

    }
}
