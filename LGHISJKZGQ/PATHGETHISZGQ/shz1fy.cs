using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Windows.Forms;
using System.Data;
using LGHISJKZGQ;
using LGHISJK;


namespace LGHISJKZGQ
{
    class shz1fy
    {
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public static string cz1yxml(string Sslbx, string Ssbz, string Debug)
        {
            dbbase.sqldb aa = new sqldb(Application.StartupPath + "\\sz.ini", "jsddb");
            string brlb = "";
            string codetype = "";
            string zxks = f.ReadString(Sslbx, "zxks", "307").Replace("\0", "");
            string yzxm = "";
            
            int sqd = 0;
            if (Sslbx == "体检号")
            {
                brlb = "3";
                codetype = "1";
                sqd = f.ReadInteger(Sslbx, "sqd", 0);
                zxks = f.ReadString(Sslbx, "zxks", "307").Replace("\0", "");
            }
            if (brlb == "")
            {
                MessageBox.Show("无此" + Sslbx);
                return "0";
            }
            string sqlstring = "exec usp_yjjk_getbrxx " + brlb + "," + codetype + ",'" + Ssbz + "',0";
            
            DataSet ds1 = aa.GetDataSet(sqlstring, "tjdt");
            try
            {
                if (ds1.Tables[0].Rows.Count < 1)
                {
                    MessageBox.Show("未找到病人！");
                    return "0";
                }
            }
            catch
            {
                MessageBox.Show("HIS连接失败！");
                return "0";
            }

            DataTable jcxm = aa.GetDataTable("exec usp_yjjk_getwzxxm " + brlb + ",'" + Ssbz + "','" + Ssbz + "','','',0,0", "jcxm");

            string jcxmdm = "";
            int intxh = 0;
          
            try
            {
                if (jcxm.Rows.Count > 0)
                {
                  
                    //1508250002
                    DataTable dtx = new DataTable();
                    dtx.Columns.Add("序号", Type.GetType("System.String"));
                    dtx.Columns.Add("医嘱项目", Type.GetType("System.String"));
                    dtx.Columns.Add("单价", Type.GetType("System.String"));
                    dtx.Columns.Add("数量", Type.GetType("System.String"));
                    dtx.Columns.Add("申请时间", Type.GetType("System.String"));
                    dtx.Columns.Add("申请科室", Type.GetType("System.String"));
                    dtx.Columns.Add("执行科室", Type.GetType("System.String"));

                    string[] dtxrow = new string[dtx.Columns.Count];
                    for (int i = 0; i < jcxm.Rows.Count; i++)
                    {
                       
                        if (jcxm.Rows[i]["ExecDept"].ToString() != "307" && jcxm.Rows[i]["ExecDept"].ToString() != "病理科")
                        {   
                           jcxm.Rows.RemoveAt(i);i--; 
                            continue;
                        }

                            dtxrow[0] = i.ToString();
                            dtxrow[1] = jcxm.Rows[i]["ItemName"].ToString();
                            dtxrow[2] = jcxm.Rows[i]["Price"].ToString();
                            dtxrow[3] = jcxm.Rows[i]["ItemQty"].ToString();
                            dtxrow[4] = jcxm.Rows[i]["ApplyTime"].ToString();
                            dtxrow[5] = jcxm.Rows[i]["ApplyDept"].ToString();
                            dtxrow[6] = jcxm.Rows[i]["ExecDept"].ToString();
                            dtx.Rows.Add(dtxrow);
                        
                    }

                    if (dtx.Rows.Count >1)
                    {
                      
                        yzxz_yfy from2 = new yzxz_yfy(dtx);
                     
                        string xhb = "";

                        if (from2.ShowDialog() == DialogResult.OK)
                        {
                            xhb = from2.xh;
                            intxh = Convert.ToInt16(xhb);
                            jcxmdm = jcxm.Rows[intxh]["LogNo"].ToString().Trim();
                            yzxm = jcxm.Rows[intxh]["ItemName"].ToString().Trim();
                        }
                        else
                        {
                            MessageBox.Show("未选择医嘱！");
                            return "0";
                        }
                    }
                    else
                    {
                        jcxmdm = jcxm.Rows[0]["LogNo"].ToString().Trim();
                        yzxm = jcxm.Rows[0]["ItemName"].ToString().Trim();
                    }

                }
                else
                {
                    if (MessageBox.Show("未找到医嘱信息，是否提取病人信息？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    { }
                    else
                    {
                        return "0";
                    }

                }
            }
            catch
            {
                if (MessageBox.Show("未找到医嘱信息，是否提取病人信息？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                { }
                else
                {
                    return "0";
                }
            }

            int xh = 0;
            if (ds1.Tables[0].Rows.Count > 0)
            {
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                if (f.ReadInteger(Sslbx, "cardno", 0) != 1)
                {
                    xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["PatientID"].ToString().Trim() + (char)34 + " ";
                }
                else
                {
                    xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["CardNO"].ToString().Trim() + (char)34 + " ";
                }
                //MessageBox.Show(ds1.Tables[0].Rows[xh]["CardNO"].ToString().Trim());
                //MessageBox.Show(f.ReadInteger(Sslbx, "cardno", 0).ToString());
                try
                {
                    xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["CureNo"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "就诊ID=" + (char)34 + (char)34 + " ";
                }
                xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
               
                xml = xml + "门诊号=" + (char)34 + (char)34 + " ";
                
                
                
                xml = xml + "住院号=" + (char)34 + (char)34 + " ";
                
                xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[xh]["PatName"].ToString().Trim() + (char)34 + " ";
                string xb = "";
                if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "1") xb = "男";
                if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "2") xb = "女";
                if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "3") xb = "其他";

                xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                string ppp = ds1.Tables[0].Rows[xh]["BIRTH"].ToString().Trim();
                string pppp = "";
                string nl = "";
                try
                {
                    pppp = ppp.Substring(0, 4) + "-" + ppp.Substring(4, 2) + "-" + ppp.Substring(6, 2);
                    nl = datediff(DateTime.Now, Convert.ToDateTime(pppp));

                }
                catch
                {
                }
                

                xml = xml + "年龄=" + (char)34 + nl + (char)34 + " ";
                xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
                try
                {
                    xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[xh]["Address"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "地址=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[xh]["Phone"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "电话=" + (char)34 + (char)34 + " ";
                }
                
                xml = xml + "病区=" + (char)34 + (char)34 + " ";
                
                try
                {
                    xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[xh]["BedNo"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "床号=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[xh]["IDNum"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "民族=" + (char)34 + ds1.Tables[0].Rows[xh]["Nation"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "职业=" + (char)34 + ds1.Tables[0].Rows[xh]["Career"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "职业=" + (char)34 + (char)34 + " ";
                }
                
                try
                {
                    xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[xh]["DeptName"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检科室=" + (char)34 + (char)34 + " ";
                }
                try
                {
                    xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[xh]["ToDoc"].ToString().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医生=" + (char)34 + (char)34 + " ";
                }
                
                //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
                //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
                xml = xml + "收费=" + (char)34 + (char)34 + " ";
                xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                xml = xml + "医嘱项目=" + (char)34 + yzxm + (char)34 + " ";
                xml = xml + "备用1=" + (char)34 + (char)34 + " ";

                xml = xml + "备用2=" + (char)34 + jcxmdm + (char)34 + " ";


               


                string fkfs = "";

                //if (ds1.Tables[0].Rows[xh]["ChargeType"].ToString().Trim() == "7") fkfs = "干保";

                xml = xml + "费别=" + (char)34 + fkfs + (char)34 + " ";
               
                xml = xml + "病人类别=" + (char)34 + "体检" + (char)34 + " ";
                
                xml = xml + "/>";
                xml = xml + "<临床病史><![CDATA[" + " " + "]]></临床病史>";
                xml = xml + "<临床诊断><![CDATA[" + " " + "]]></临床诊断>";
                xml = xml + "</LOGENE>";
                if (Debug == "1")
                    log.WriteMyLog("返回的xml字符串:" + xml);
                return xml;

            }
            else
            {
                MessageBox.Show("无此" + Sslbx);
                return "0";
            }
            

        }
        private static string datediff(DateTime tm1, DateTime tm2)
        {
            string diff = "";
            //tm2 = Convert.ToDateTime("2010-01-02");
            try
            {
                if (tm2 > tm1)
                {
                    diff = "0岁";
                }
                else
                {
                    int nly = (tm1.Year - tm2.Year) * 12 + (tm1.Month - tm2.Month);
                    if (nly > 12)
                    {
                        int xxxx = nly / 12;
                        diff = Convert.ToString(xxxx) + "岁";
                    }
                    else
                    {
                        TimeSpan ts = tm1 - tm2;
                        if (ts.Days < 31)
                        {
                            diff = Convert.ToString(ts.Days) + "天";
                        }
                        else
                        {
                            diff = Convert.ToString(nly) + "月";
                        }
                    }

                }
            }
            catch
            {
                diff = "0岁";
            }
            return diff;
        }
    }
}
