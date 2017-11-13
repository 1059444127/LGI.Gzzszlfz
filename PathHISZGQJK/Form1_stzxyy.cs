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
using PathHISJK.Webstzxyy;


using System.Xml;
using System.Xml.XPath;

using System.Data.Common;

using System.Runtime.InteropServices;





namespace PathHISJK
{
    public partial class Form1 : Form
    {
        public Form1(string[] args1)
        {
            InitializeComponent();
            //测试区
            if (f.ReadInteger("jktest", "test", 0) == 1)
            {
                args1 = new string[1];
                args1[0] = f.ReadString("jktest","blh","232825").Replace("/0","")+",bz";
            }
            //MessageBox.Show("本程序正在运行！");
            //测试区
            try
            {

                log.WriteMyLog(args1[0]);
                //if (f.ReadInteger("jktest", "test", 0) == 1) MessageBox.Show(args1[0]);
            }
            catch
            { }
            if (args1.Length < 1)//如果没带参数则退出
            {
                fromexit = true;
                return;                
            }
            // 1,调用jk.exe时 传更多的参数。JK.EXE名字改为PathHISJK_FZ.EXE，病理号^cg/bd/bc^bgxh^new/old^save/qxsh (同济)
            //[savetohis]//原来已有
            int arg =args1[0].IndexOf(",");
            if (arg < 0)
            {
                if (args1[0].IndexOf("^")>-1)
                {   //复杂接口
                    try
                    {
                        string[] aa = args1[0].Split('^');
                        blh = aa[0].ToString();
                        dz = aa[4].ToString();
                        bglx = aa[1].ToString();
                        bgxh = aa[2].ToString();
                    }
                    catch
                    {
                        log.WriteMyLog("传入参数解析出错");
                        return;
                    } 
                    yymc = f.ReadString("savetohis", "yymc", "").Replace("\0", "");           
                }
                else
                fromexit = true;
            }
            else
            {
                try
                {
                    blh = args1[0].Substring(0, arg);    
                    string hisbz = args1[0].Substring(arg+1);
                    if (hisbz == "bz")
                    {
                        yymc = f.ReadString("savetohis", "yymc", "").Replace("\0", "");
                    }
                    else
                    {
                        yymc = hisbz;
                    }
                }
                catch
                {
                    blh = "";
                    yymc = "";
                }
            } 
        }

        public IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath+"\\sz.ini");
        

        public bool fromexit = false;
        public string blh, yymc,dz,bglx,bgxh;
        private sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
        
        private void Form1_Load(object sender, EventArgs e)
        {
            if (fromexit)
            {
                MessageBox.Show("无权启动程序！");
                this.Close();
            }
              if (yymc == "stzxyy")
            {
                stzxyy();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
            }           

            MessageBox.Show(yymc + "无此医院参数！");
            this.Close();
           
        }
        /// <summary>
        /// 生成XML字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bgtext"></param>
        /// <returns></returns>
        private string sdxml(DataTable dt,string bgtext,string brlb)
        {
            string datastring;
            datastring = "<MainInfo>";
            datastring = datastring + "<SysType>Pacs_BL</SysType>";
            datastring = datastring + "<InfoList>";
            datastring = datastring + "<EntityInfo>";
            datastring = datastring + "<info colname=" + (char)34 + "影像号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_blh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "机器型号" + (char)34 + " value=" + (char)34 + "DG" + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请单号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sqxh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "姓名" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_xm"].ToString().Trim()+ (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "性别" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_xb"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "年龄" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_nl"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            if (brlb == "2")
            {
                datastring = datastring + "<info colname=" + (char)34 + "住院号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_zyh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            }
            else
            {
                datastring = datastring + "<info colname=" + (char)34 + "住院号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_mzh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            }
            datastring = datastring + "<info colname=" + (char)34 + "床号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_ch"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请科室" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sjks"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请医生" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sjys"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请时间" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sdrq"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "检查部位" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_bblx"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "检查名称" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_bbmc"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "影像所见" + (char)34 + " value=" + (char)34 + @dt.Rows[0]["F_jxsj"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "影像结论" + (char)34 + " value=" + (char)34 + @dt.Rows[0]["F_blzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "报告医生" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_bgys"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "报告时间" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_bgrq"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "报告图片" + (char)34 + " value=" + (char)34 + bgtext + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "病人类型" + (char)34 + " value=" + (char)34 + brlb + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            
            
            
            datastring = datastring + "</EntityInfo>";
            datastring = datastring + "</InfoList>";
            datastring = datastring + "</MainInfo>";

           

            try
            {
                XmlDocument xx2 = new XmlDocument();
                xx2.LoadXml(datastring);
            }
            catch
            {
                return "";
            }
            return datastring;

        }
        private string sdxml_BD(DataTable dt,DataTable dt_bd, string bgtext, string brlb)
        {
            string datastring;
            datastring = "<MainInfo>";
            datastring = datastring + "<SysType>Pacs_BL</SysType>";
            datastring = datastring + "<InfoList>";
            datastring = datastring + "<EntityInfo>";
            datastring = datastring + "<info colname=" + (char)34 + "影像号" + (char)34 + " value=" + (char)34 + dt_bd.Rows[0]["F_blh"].ToString().Trim() + "_B_" + dt_bd.Rows[0]["F_BD_BGXH"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "机器型号" + (char)34 + " value=" + (char)34 + "DG" + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请单号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sqxh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "姓名" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_xm"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "性别" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_xb"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "年龄" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_nl"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            if (brlb == "2")
            {
                datastring = datastring + "<info colname=" + (char)34 + "住院号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_zyh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            }
            else
            {
                datastring = datastring + "<info colname=" + (char)34 + "住院号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_mzh"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            }
            datastring = datastring + "<info colname=" + (char)34 + "床号" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_ch"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请科室" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sjks"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请医生" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sjys"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "申请时间" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_sdrq"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "检查部位" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_bblx"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "检查名称" + (char)34 + " value=" + (char)34 + dt.Rows[0]["F_bbmc"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "影像所见" + (char)34 + " value=" + (char)34 + "" + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "影像结论" + (char)34 + " value=" + (char)34 + @dt_bd.Rows[0]["F_BDzd"].ToString().Trim().Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\\", "&quot;").Replace("'", "&apos;").Replace("\"", "&quot;") + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "报告医生" + (char)34 + " value=" + (char)34 + dt_bd.Rows[0]["F_bd_bgys"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "报告时间" + (char)34 + " value=" + (char)34 + dt_bd.Rows[0]["F_bd_bgrq"].ToString().Trim() + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "报告图片" + (char)34 + " value=" + (char)34 + bgtext + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";
            datastring = datastring + "<info colname=" + (char)34 + "病人类型" + (char)34 + " value=" + (char)34 + brlb + (char)34 + " type=" + (char)34 + "0" + (char)34 + "/>";



            datastring = datastring + "</EntityInfo>";
            datastring = datastring + "</InfoList>";
            datastring = datastring + "</MainInfo>";



            try
            {
                XmlDocument xx2 = new XmlDocument();
                xx2.LoadXml(datastring);
            }
            catch
            {
                return "";
            }
            return datastring;

        }
        private void stzxyy()
        {
            DataTable bljc = new DataTable();
             DataTable blbd = new DataTable();
            string brlb = "";
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "bljc");
            blbd = aa.GetDataTable("select * from T_BDBG where F_blh='" + blh + "' and F_BD_BGZT='已审核'  and  F_BD_BGXH='"+bgxh+"'", "blbd");
           
            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                return;
            }
            if (bglx == "cg" && dz == "save" && bljc.Rows[0]["F_bgzt"].ToString().Trim() != "已审核")
            {
                if (bljc.Rows[0]["F_BRBH"].ToString().Trim() != "" && bljc.Rows[0]["F_YZXM"].ToString().Trim().Contains("||"))
                {
                    string con_str = f.ReadString("savetohis", "odbcsql", "Data Source=172.16.0.30;Initial Catalog=hisdbstzx;User Id=bl;Password=bl123;");
                  //  MessageBox.Show(bljc.Rows[0]["F_YZXM"].ToString().Trim().Substring(bljc.Rows[0]["F_YZXM"].ToString().LastIndexOf("||")+2));
                    SqlConnection sqlcon = new SqlConnection(con_str);
                    try
                    {
                        sqlcon.Open();
                        SqlCommand sqlcom = new SqlCommand();
                        sqlcom.Connection = sqlcon;
                        sqlcom.CommandText = "MzInterfacePath_getinfo_Update";
                        sqlcom.CommandType = CommandType.StoredProcedure;

                        sqlcom.Parameters.Add("@p_id", SqlDbType.VarChar, 12);
                        sqlcom.Parameters["@p_id"].Value = bljc.Rows[0]["F_BRBH"].ToString().Trim();

                        sqlcom.Parameters.Add("@jy_sn", SqlDbType.VarChar, 12);
                        sqlcom.Parameters["@jy_sn"].Value = bljc.Rows[0]["F_YZXM"].ToString().Trim().Substring(bljc.Rows[0]["F_YZXM"].ToString().LastIndexOf("||")+2);

                        sqlcom.ExecuteNonQuery();
                        sqlcon.Close();
                    }
                    catch (Exception ee)
                    {
                        sqlcon.Close();
                        MessageBox.Show("回传his接受标记异常"+ee.ToString());
                        return;
                    }
                }
                return;
            }

            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "住院")
            {
                brlb = "2";   
            }
            if (bljc.Rows[0]["F_brlb"].ToString().Trim() == "门诊")
            {
                brlb = "1";
            }
            if (brlb == "")
            {
                log.WriteMyLog("非住院或门诊病人，不处理！");
                return;
            }




            if (bglx =="bd" && dz== "save")
            {
                //冰冻审核

                if (blbd == null)
                  {
                MessageBox.Show("病理数据库设置有问题！");
                return;
                  }
                  if (blbd.Rows.Count < 1)
                  {
                      log.WriteMyLog("没有需要上传的病理报告");
                      return;
                  }
                //生成冰冻报告，不打图
                string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
                    string sBGGSName = Application.StartupPath.ToString() + "\\rpt\\冰冻.frf";
                    string sJPGNAME = ftplocal + "\\" + blh.Trim() + "_B_" + blbd.Rows[0]["F_BD_BGXH"].ToString()+ ".jpg";
                  string sSQL_DY = "SELECT * FROM T_JCXX,T_BDBG  WHERE T_JCXX.F_BLH *= T_BDBG.F_BLH AND T_JCXX.F_BLH = '" + blh + "'";
                  prreport pr = new prreport();
                  pr.print(sSQL_DY, this.Handle, "", "", sBGGSName, sJPGNAME);
                  Image bgjpg = Image.FromFile(ftplocal + "\\" + blh.Trim() +"_B_" + blbd.Rows[0]["F_BD_BGXH"].ToString()+ "_1.jpg");
                  MemoryStream ms = new MemoryStream();
                  bgjpg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                  byte[] bgbyte = ms.ToArray();
                  PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[] hs = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[1];
                  hs[0] = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType();
                  hs[0].Value = bgbyte;
                  hs[0].Key = blh.Trim() + "_B_" + blbd.Rows[0]["F_BD_BGXH"].ToString() + ".jpg";

                  //调用生成xml字符串子程序
                  string xml = sdxml_BD(bljc, blbd, blh.Trim() + "_B_" + blbd.Rows[0]["F_BD_BGXH"].ToString() + ".jpg", brlb);
                  if (xml == "")
                  {
                      MessageBox.Show(@"部分内容含有非法字符(<,>,@,&,\等),镜下所见和病理诊断除外，HIS写入失败！");
                      return;
                  }
                  bool sdwstrue = true;
                  bool sdwstrue1 = true;
                  bool sdwstrue2 = true;
                  bool sdwstrue3 = true;
                  int sdwsint = 0;
                  int sdwsint1 = 0;

                  //调用webservice
                  try
                  {
                      PathHISJK.Webstzxyy.clsWcfInterface sdws = new PathHISJK.Webstzxyy.clsWcfInterface();
                      string url = f.ReadString("stzxyy", "weburl", "");
                       if (url != "")
                          sdws.Url = url;
                     
                  sdws.EHRInterfaceBL(xml, hs, out  sdwsint, out sdwstrue, out sdwsint1, out sdwstrue1, out sdwstrue2, out sdwstrue3);
                    
                  }
                  catch (Exception e)
                  {
                       MessageBox.Show("Web服务器未打开！" + e.ToString());
                      return;
                  }
                  if (sdwsint < 1)
                  MessageBox.Show("HIS写入失败！" + sdwsint.ToString());
              return;
                //------------------------------------------------------------------
            }

            else
            {
              if (bljc.Rows[0]["F_bgzt"].ToString().Trim() != "已审核")
                  return;

            //下载及生成报告图片
            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            string ftplocal = f.ReadString("ftp", "ftplocal", "c:\\temp").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            FtpWeb fw = new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
            string txml= bljc.Rows[0]["F_txml"].ToString().Trim();
            DataTable txlb = aa.GetDataTable("select * from V_dytx where F_blh='" + blh + "'", "txlb");
            string sbmp = "";
            string stxsm = "";
            string sBGGSName = Application.StartupPath.ToString()+"\\rpt\\"+bljc.Rows[0]["F_bggs"].ToString().Trim() + "-"+txlb.Rows.Count.ToString()+"图.frf";
            string sJPGNAME = ftplocal + "\\" + blh.Trim() + ".jpg";
            for (int i = 0; i < txlb.Rows.Count; i++)
            {
                stxsm = stxsm+txlb.Rows[i]["F_txsm"].ToString().Trim() + ",";
                string ftpstatus="";
                fw.Download(ftplocal, txml + "/" + txlb.Rows[i]["F_txm"].ToString().Trim(), txlb.Rows[i]["F_txm"].ToString().Trim(),out ftpstatus);
                if (ftpstatus == "Error")
                {
                    return;
                }
                sbmp = sbmp+ftplocal + "\\" + txlb.Rows[i]["F_txm"].ToString().Trim() + ",";
            }
             //生成常规报告
            string sSQL_DY = "SELECT * FROM T_JCXX,T_TBS_BG WHERE T_JCXX.F_BLH *= T_TBS_BG.F_BLH AND T_JCXX.F_BLH = '" + blh + "'";
            prreport pr = new prreport();
            pr.print(sSQL_DY, this.Handle, sbmp, stxsm, sBGGSName, sJPGNAME);            
            Image bgjpg = Image.FromFile(ftplocal + "\\" + blh.Trim() + "_1.jpg");
            MemoryStream ms = new MemoryStream();
            bgjpg.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);      
            byte[] bgbyte = ms.ToArray();
            PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[] hs=null;// = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[1];
           
            hs[0] = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType();
            hs[0].Value = bgbyte;
            hs[0].Key = blh.Trim() + "_1.jpg";

             
            /////// 分子病理报告单 2页

            if (File.Exists(ftplocal + "\\" + blh.Trim() + "_2.jpg"))
            {
                try
                {
                    log.WriteMyLog("分子病理上传多页报告");
                    PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[] hs2 = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[2];

                    hs2[0] = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType();
                    hs2[0].Value = bgbyte;
                    hs2[0].Key = blh.Trim() + "_1.jpg";

                    Image bgjpg2 = Image.FromFile(ftplocal + "\\" + blh.Trim() + "_2.jpg");
                    MemoryStream ms2 = new MemoryStream();
                    bgjpg2.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bgbyte2 = ms2.ToArray();
                    hs2[1] = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType();
                    hs2[1].Value = bgbyte2;
                    hs2[1].Key = blh.Trim() + "_2.jpg";
                    hs = hs2;
                }
                catch (Exception ss)
                {
                    MessageBox.Show(ss.ToString());
                    log.WriteMyLog("分子病理上传多页失败" + ss.ToString());
                }
            }
            else
            {
                PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[] hs1 = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[1];
                hs1[0] = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType();
                hs1[0].Value = bgbyte;
                hs1[0].Key = blh.Trim() + "_1.jpg";
                hs = hs1;
            }
            //PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[] hs = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType[1];
            //hs[0] = new PathHISJK.Webstzxyy.ArrayOfKeyValueOfanyTypeanyTypeKeyValueOfanyTypeanyType();
            //hs[0].Value = bgbyte;
            //hs[0].Key = blh.Trim() + "_1.jpg";
           // hs = hs1;
        
            //调用生成xml字符串子程序
            string xml = sdxml(bljc, blh.Trim() + "_1.jpg",brlb);
           
            if (xml == "")
            {
                MessageBox.Show(@"部分内容含有非法字符(<,>,@,&,\等),镜下所见和病理诊断除外，HIS写入失败！");
                return;
            }
            bool sdwstrue=true;
            bool sdwstrue1 = true;
            bool sdwstrue2 = true;
            bool sdwstrue3 = true;
            int sdwsint = 0; 
            int sdwsint1 = 0;
         
            //调用webservice
            try
            {
                PathHISJK.Webstzxyy.clsWcfInterface sdws = new PathHISJK.Webstzxyy.clsWcfInterface();
                string url = f.ReadString("stzxyy", "weburl", "");
                if (url != "")
                {
                    sdws.Url = url;
                }
           sdws.EHRInterfaceBL(xml, hs, out  sdwsint, out sdwstrue, out sdwsint1, out sdwstrue1, out sdwstrue2, out sdwstrue3);
               aa.ExecuteSQL("update T_JCXX set F_SCBJ='1'  where  F_BLH='" + blh + "'");
                }
            catch (Exception e)
            {  MessageBox.Show("Web服务器未打开！" + e.ToString());
                return;
            }           
     
            if (sdwsint < 1)
           MessageBox.Show("HIS写入失败！"+sdwsint.ToString());
                 return;
           }
           return;
        }
        
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}