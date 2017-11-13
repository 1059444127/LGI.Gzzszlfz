using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using dbbase;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    class xyyy_gzl
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        public  void pathtohis(string blh,string  bgzt)
        {

            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            
            string jsyz =f.ReadString("BLKJXKH","jsyzlx","深切,白片,连切").Replace("\0", "").Trim();
              string tjyz =f.ReadString("BLKJXKH","tjyzlx", "免疫组化,特殊检查").Replace("\0", "").Trim(); 

            jsyz = jsyz.Replace(",", "','");
            tjyz = tjyz.Replace(",", "','");


            string err = "";
            string BL_constr = f.ReadString("BLKJXKH", "odbc","Data Source=.;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p;").Replace("\0", "").Trim(); ;
            SqlDB sqldb = new SqlDB();

            DataTable  dt_bg=new DataTable();
            dt_bg = sqldb.DataAdapter(BL_constr, "select F_BLH,F_BGYS,F_FZYS,F_SHYS,F_BGZT,F_SQXH,F_BGLRY,F_JSY from T_jcxx where F_blh='" + blh + "'", ref err);
             if(dt_bg==null)
            {
              //("bingli查询数据错误："+err);
                return;
            }
            if(dt_bg.Rows.Count<=0)
            {
                log.WriteMyLog(blh+"未查询到记录");
                return;
            }
       


            string sqxh=dt_bg.Rows[0]["F_sqxh"].ToString().Trim();
           
            string bgys = dt_bg.Rows[0]["F_bgys"].ToString().Trim();
            string fzys = dt_bg.Rows[0]["F_fzys"].ToString().Trim();
            string shys = dt_bg.Rows[0]["F_shys"].ToString().Trim();
               string bgzt2 = dt_bg.Rows[0]["F_bgzt"].ToString().Trim();
            

              if(sqxh=="")
               { 
                aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='申请序号为空不处理',F_FSZT='不处理'  where F_blh='" + blh + "'  ");
                return;
               }

            if(bgzt=="SH" &&bgzt2!="已审核" )
               { 
                aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='报告未审核不处理',F_FSZT='不处理'  where F_blh='" + blh + "' and  F_BGZT='已审核'");
                return;
               }
              string  rpt_xml="";
               if (bgzt == "QXSH")
               {

                   rpt_xml = rpt_xml + "<ExamBLItemList>"
               + "<CheckFlow>" + sqxh + "</CheckFlow>"
               + "<ExamGroup>" + blh + "</ExamGroup>"
               + "<ScheduledDate></ScheduledDate>"
               + "<Note></Note>"
               + "<Operator></Operator>"
               + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
               + "<Status>0</Status>"
               + "<Notice></Notice>"
               + "<ExamAddress></ExamAddress>"
               + "</ExamBLItemList>";

                   log.WriteMyLog(blh + "：" + rpt_xml);

                   rpt_xml = "<Request><ExamStatusBL><ExamBLList>" + rpt_xml + "</ExamBLList></ExamStatusBL></Request>";
                   string rtn_xml = "";
                   try
                   {
                       xyfyWeb.DHCPisXiangYaOne xyfy = new xyfyWeb.DHCPisXiangYaOne();
                       xyfy.Url = "http://192.168.2.51:57772/csp/dhcens/DHC.Pis.XiangYaOne.BS.Web.cls";
                       string pathweburl = f.ReadString("savetohis", "wsurl", "");
                       if (pathweburl.Trim() != "")
                           xyfy.Url = pathweburl;

                       rtn_xml = xyfy.DhcService("ExamStatusBL", rpt_xml);
                   }
                   catch (Exception ee4)
                   {

                       log.WriteMyLog(ee4.Message.ToString());
                       aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='连接webservice异常：" + ee4.Message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='取消审核' ");
                       return;
                   }
                   try
                   {
                       XmlDataDocument xd = new XmlDataDocument();
                       xd.LoadXml(rtn_xml);
                       XmlNode xn = xd.SelectSingleNode("/Response/ExamStatusBLReturn");

                       if (xn["Returncode"].InnerText.ToString() == "0")
                       {
                           log.WriteMyLog(xn["ResultContent"].InnerText.ToString() + "：" + rtn_xml);
                           aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGZT='取消审核' ");
                       }
                       else
                       {
                           log.WriteMyLog(xn["ResultContent"].InnerText.ToString() + "：" + rtn_xml);
                           aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='" + xn["ResultContent"].InnerText.ToString() + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='取消审核' ");
                       }
                   }
                   catch (Exception ee3)
                   {

                       log.WriteMyLog(ee3.Message.ToString() + "：" + rtn_xml);
                       aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='" + ee3.Message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='取消审核' ");
                       return;
                   }


                   return;
               }

               if (bgzt == "SH")
               {



                   //用户工号
                   DataTable Dt_YH = new DataTable();
                   Dt_YH = sqldb.DataAdapter(BL_constr, "select F_YHBH,F_YHMC,F_YH_BY1,F_YH_BY2 from T_YH ", ref err);





                   try
                   {
                       //报告
                       #region
                       //报告医生

                       if (dt_bg.Rows[0]["F_bgys"].ToString().Trim() != "")
                       {
                           string[] bgyss = dt_bg.Rows[0]["F_bgys"].ToString().Trim().Split('/');
                           string BGYSXML = "";
                           foreach (string f_bgys in bgyss)
                           {
                               if (f_bgys != "")
                               {
                                   string bgysBH = Get_YHBH(f_bgys, Dt_YH);
                                   if (bgysBH != "")
                                   {
                                       if (BGYSXML == "")
                                           BGYSXML = bgysBH + "#1";
                                       else
                                           BGYSXML = BGYSXML + "," + bgysBH + "#1";
                                   }
                               }
                           }
                           rpt_xml = rpt_xml + "<ExamBLItemList>"
                           + "<CheckFlow>" + sqxh + "</CheckFlow>"
                           + "<ExamGroup>" + blh + "</ExamGroup>"
                           + "<ScheduledDate></ScheduledDate>"
                           + "<Note></Note>"
                           + "<Operator>" + BGYSXML + "</Operator>"
                           + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                           + "<Status>4</Status>"
                           + "<Notice>1</Notice>"
                           + "<ExamAddress></ExamAddress>"
                           + "</ExamBLItemList>";
                       }

                       //复诊医生
                       if (dt_bg.Rows[0]["F_fzys"].ToString().Trim() != "")
                       {
                           string[] fzyss = dt_bg.Rows[0]["F_fzys"].ToString().Trim().Split('/');
                           string FZYSXML = "";
                           foreach (string f_fzys in fzyss)
                           {

                               if (f_fzys != "")
                               { 
                                   string fzysBH = Get_YHBH(f_fzys, Dt_YH);
                                   if (FZYSXML == "")
                                       FZYSXML = fzysBH + "#1";
                                   else
                                       FZYSXML = FZYSXML + "," + fzysBH + "#1";
                               }
                           }
                           rpt_xml = rpt_xml + "<ExamBLItemList>"
                           + "<CheckFlow>" + sqxh + "</CheckFlow>"
                           + "<ExamGroup>" + blh + "</ExamGroup>"
                           + "<ScheduledDate></ScheduledDate>"
                           + "<Note></Note>"
                           + "<Operator>" + FZYSXML + "</Operator>"
                           + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                           + "<Status>4</Status>"
                           + "<Notice>2</Notice>"
                           + "<ExamAddress></ExamAddress>"
                           + "</ExamBLItemList>";
                       }

                       //审核医生
                       string shysBH = Get_YHBH(shys, Dt_YH);
                       rpt_xml = rpt_xml + "<ExamBLItemList>"
                       + "<CheckFlow>" + sqxh + "</CheckFlow>"
                       + "<ExamGroup>" + blh + "</ExamGroup>"
                       + "<ScheduledDate></ScheduledDate>"
                       + "<Note></Note>"
                       + "<Operator>" + shys + "</Operator>"
                       + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                       + "<Status>4</Status>"
                       + "<Notice>3</Notice>"
                       + "<ExamAddress></ExamAddress>"
                       + "</ExamBLItemList>";

                       //录入员
                       if (dt_bg.Rows[0]["F_BGLRY"].ToString().Trim() != "")
                       {
                           string[] LRY = dt_bg.Rows[0]["F_BGLRY"].ToString().Trim().Split('/');
                           string LRY_XML = "";
                           foreach (string F_LRY in LRY)
                           {
                              
                               if (F_LRY != "")
                               {
                                   string LRYBH = Get_YHBH(F_LRY, Dt_YH);
                                   if (LRY_XML == "")
                                       LRY_XML = LRYBH + "#1";
                                   else
                                       LRY_XML = LRY_XML + "," + LRYBH + "#1";
                               }
                           }
                           rpt_xml = rpt_xml + "<ExamBLItemList>"
                           + "<CheckFlow>" + sqxh + "</CheckFlow>"
                           + "<ExamGroup>" + blh + "</ExamGroup>"
                           + "<ScheduledDate></ScheduledDate>"
                           + "<Note></Note>"
                           + "<Operator>" + LRY_XML + "</Operator>"
                           + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                           + "<Status>4</Status>"
                           + "<Notice>11</Notice>"
                           + "<ExamAddress></ExamAddress>"
                           + "</ExamBLItemList>";
                       }

                       #endregion

                       //登记
                       #region

                       string jsy = dt_bg.Rows[0]["F_JSY"].ToString().Trim();
                       if (jsy != "")
                       {
                           string jsyBH = Get_YHBH(jsy, Dt_YH);
                           rpt_xml = rpt_xml + "<ExamBLItemList>"
                + "<CheckFlow>" + sqxh + "</CheckFlow>"
                + "<ExamGroup>" + blh + "</ExamGroup>"
                + "<ScheduledDate></ScheduledDate>"
                + "<Note></Note>"
                + "<Operator>" + jsyBH + "#1</Operator>"
                + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                + "<Status>4</Status>"
                + "<Notice>4</Notice>"
                + "<ExamAddress></ExamAddress>"
                + "</ExamBLItemList>";
                       }


                       #endregion

                       //取材
                       #region
                       DataTable dt_qc = new DataTable();
                       dt_qc = sqldb.DataAdapter(BL_constr, "select F_QCYS,F_JLY  from T_QCMX where F_blh='" + blh + "'", ref err);
                       if (dt_qc == null)
                       {

                       }
                       else if (dt_qc.Rows.Count <= 0)
                       {


                       }
                       else
                       {


                           DataView dv_qc = dt_qc.DefaultView;

                           DataTable dt_qcys = dv_qc.ToTable(true, "F_QCYS");
                           DataTable dt_jly = dv_qc.ToTable(true, "F_JLY");


                           //取材医生
                           string qcys_xml = "";
                           for (int x = 0; x < dt_qcys.Rows.Count; x++)
                           {
                               string qcys = dt_qcys.Rows[x]["F_QCYS"].ToString();
                               if (qcys != "")
                               {
                                   dv_qc.RowFilter = " F_QCYS='" + qcys + "'";

                                   string qcysBH = Get_YHBH(qcys, Dt_YH);
                                   DataTable dt_count = dv_qc.ToTable();
                                   if (qcys_xml == "")
                                       qcys_xml = qcysBH + "#" + dt_count.Rows.Count.ToString();
                                   else
                                       qcys_xml = qcys_xml + "," + qcysBH + "#" + dt_count.Rows.Count.ToString();
                               }
                           }
                           if (qcys_xml != "")
                           {
                               rpt_xml = rpt_xml + "<ExamBLItemList>"
                   + "<CheckFlow>" + sqxh + "</CheckFlow>"
                   + "<ExamGroup>" + blh + "</ExamGroup>"
                   + "<ScheduledDate></ScheduledDate>"
                   + "<Note></Note>"
                   + "<Operator>" + qcys_xml + "</Operator>"
                   + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                   + "<Status>1</Status>"
                   + "<Notice>5</Notice>"
                   + "<ExamAddress></ExamAddress>"
                   + "</ExamBLItemList>";
                           }

                           //取材录入员
                           string jly_xml = "";
                           for (int x = 0; x < dt_jly.Rows.Count; x++)
                           {
                               string jly = dt_jly.Rows[x]["F_JLY"].ToString();
                               if (jly != "")
                               {
                                   dv_qc.RowFilter = " F_JLY='" + jly + "'";
                                   DataTable dt_count = dv_qc.ToTable();
                                   string jlyBH = Get_YHBH(jly, Dt_YH);
                                   if (jly_xml == "")
                                       jly_xml = jlyBH + "#" + dt_count.Rows.Count.ToString();
                                   else
                                       jly_xml = jly_xml + "," + jlyBH + "#" + dt_count.Rows.Count.ToString();
                               }
                           }
                           if (jly_xml != "")
                           {
                               rpt_xml = rpt_xml + "<ExamBLItemList>"
                   + "<CheckFlow>" + sqxh + "</CheckFlow>"
                   + "<ExamGroup>" + blh + "</ExamGroup>"
                   + "<ScheduledDate></ScheduledDate>"
                   + "<Note></Note>"
                   + "<Operator>" + jly_xml + "</Operator>"
                   + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                   + "<Status>1</Status>"
                   + "<Notice>6</Notice>"
                   + "<ExamAddress></ExamAddress>"
                   + "</ExamBLItemList>";
                           }

                       }
                       #endregion


                       //包埋
                       #region
                       DataTable dt_bm = new DataTable();
                       dt_bm = sqldb.DataAdapter(BL_constr, "select F_CZY,F_LKH  from T_LK where F_blh='" + blh + "'", ref err);
                       if (dt_bm == null)
                       {

                       }

                       else if (dt_bm.Rows.Count <= 0)
                       {

                       }
                       else
                       {
                           DataView dv_bm = dt_bm.DefaultView;

                           DataTable dt_bmczy = dv_bm.ToTable(true, "F_CZY");


                           //包埋操作员
                           string BM_xml = "";
                           for (int x = 0; x < dt_bmczy.Rows.Count; x++)
                           {
                               string bmczy = dt_bmczy.Rows[x]["F_CZY"].ToString();
                               if (bmczy != "")
                               {
                                   dv_bm.RowFilter = " F_CZY='" + bmczy + "'";
                                   DataTable dt_count = dv_bm.ToTable();
                                   string bmczyBH = Get_YHBH(bmczy, Dt_YH);
                                   if (BM_xml == "")
                                       BM_xml = bmczyBH + "#" + dt_count.Rows.Count.ToString();
                                   else
                                       BM_xml = BM_xml + "," + bmczyBH + "#" + dt_count.Rows.Count.ToString();
                               }
                           }
                           if (BM_xml != "")
                           {
                               rpt_xml = rpt_xml + "<ExamBLItemList>"
                   + "<CheckFlow>" + sqxh + "</CheckFlow>"
                   + "<ExamGroup>" + blh + "</ExamGroup>"
                   + "<ScheduledDate></ScheduledDate>"
                   + "<Note></Note>"
                   + "<Operator>" + BM_xml + "</Operator>"
                   + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                   + "<Status>1</Status>"
                   + "<Notice>5</Notice>"
                   + "<ExamAddress></ExamAddress>"
                   + "</ExamBLItemList>";
                           }
                       }
                       #endregion

                       //切片
                       #region
                       DataTable dt_qp = new DataTable();
                       dt_qp = sqldb.DataAdapter(BL_constr, "select F_CZY,F_RWLY  from T_QP where F_blh='" + blh + "' and   F_RWLY not  in ('" + tjyz + "')  and   F_RWLY not  in ('" + jsyz + "') ", ref err);
                       if (dt_qp == null)
                       {


                       }

                       else if (dt_qp.Rows.Count <= 0)
                       {

                       }
                       else
                       {
                           DataView dv_qp = dt_qp.DefaultView;
                           DataTable dt_qpczy = dv_qp.ToTable(true, "F_CZY");

                           //切片操作员
                           string QP_XML = "";
                           for (int x = 0; x < dt_qpczy.Rows.Count; x++)
                           {
                               string qpczy = dt_qpczy.Rows[x]["F_CZY"].ToString();
                               if (qpczy != "")
                               {
                                   dv_qp.RowFilter = " F_CZY='" + qpczy + "'";
                                   DataTable dt_count = dv_qp.ToTable();
                                   string qpczyBH = Get_YHBH(qpczy, Dt_YH);
                                   if (QP_XML == "")
                                       QP_XML = qpczyBH + "#" + dt_count.Rows.Count.ToString();
                                   else
                                       QP_XML = QP_XML + "," + qpczyBH + "#" + dt_count.Rows.Count.ToString();
                               }
                           }
                           if (QP_XML != "")
                           {
                               rpt_xml = rpt_xml + "<ExamBLItemList>"
                   + "<CheckFlow>" + sqxh + "</CheckFlow>"
                   + "<ExamGroup>" + blh + "</ExamGroup>"
                   + "<ScheduledDate></ScheduledDate>"
                   + "<Note></Note>"
                   + "<Operator>" + QP_XML + "</Operator>"
                   + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                   + "<Status>1</Status>"
                   + "<Notice>8</Notice>"
                   + "<ExamAddress></ExamAddress>"
                   + "</ExamBLItemList>";
                           }


                       }

                       #endregion

                       //技术医嘱操作员
                       #region
                       DataTable dt_jsyzqp = new DataTable();
                       dt_jsyzqp = sqldb.DataAdapter(BL_constr, "select F_CZY,F_RWLY  from T_QP where F_blh='" + blh + "' and F_RWLY in ('" + jsyz + "')", ref err);
                       if (dt_jsyzqp == null)
                       {

                       }

                       else if (dt_jsyzqp.Rows.Count <= 0)
                       {

                       }
                       else
                       {
                           DataView dv_qp = dt_jsyzqp.DefaultView;
                           DataTable dt_qpczy = dv_qp.ToTable(true, "F_CZY");

                           //切片操作员
                           string QP_XML = "";
                           for (int x = 0; x < dt_qpczy.Rows.Count; x++)
                           {
                               string qpczy = dt_qpczy.Rows[x]["F_CZY"].ToString();
                               if (qpczy != "")
                               {
                                   dv_qp.RowFilter = " F_CZY='" + qpczy + "'";
                                   DataTable dt_count = dv_qp.ToTable();
                                   string qpczyBH = Get_YHBH(qpczy, Dt_YH);
                                   if (QP_XML == "")
                                       QP_XML = qpczyBH + "#" + dt_count.Rows.Count.ToString();
                                   else
                                       QP_XML = QP_XML + "," + qpczyBH + "#" + dt_count.Rows.Count.ToString();
                               }
                           }
                           if (QP_XML != "")
                           {
                               rpt_xml = rpt_xml + "<ExamBLItemList>"
                   + "<CheckFlow>" + sqxh + "</CheckFlow>"
                   + "<ExamGroup>" + blh + "</ExamGroup>"
                   + "<ScheduledDate></ScheduledDate>"
                   + "<Note></Note>"
                   + "<Operator>" + QP_XML + "</Operator>"
                   + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                   + "<Status>4</Status>"
                   + "<Notice>9</Notice>"
                   + "<ExamAddress></ExamAddress>"
                   + "</ExamBLItemList>";
                           }

                       }

                       #endregion

                       //特检医嘱操作员
                       #region
                       DataTable dt_tjyzqp = new DataTable();
                       dt_tjyzqp = sqldb.DataAdapter(BL_constr, "select F_CZY,F_RWLY  from T_QP where F_blh='" + blh + "' and F_RWLY in ('" + tjyz + "')", ref err);
                       if (dt_tjyzqp == null)
                       {

                       }

                       else if (dt_tjyzqp.Rows.Count <= 0)
                       {

                       }
                       else
                       {
                           DataView dv_qp = dt_tjyzqp.DefaultView;
                           DataTable dt_qpczy = dv_qp.ToTable(true, "F_CZY");

                           //切片操作员
                           string QP_XML = "";
                           for (int x = 0; x < dt_qpczy.Rows.Count; x++)
                           {
                               string qpczy = dt_qpczy.Rows[x]["F_CZY"].ToString();
                               if (qpczy != "")
                               {
                                   dv_qp.RowFilter = " F_CZY='" + qpczy + "'";
                                   DataTable dt_count = dv_qp.ToTable();
                                   string qpczyBH = Get_YHBH(qpczy, Dt_YH);
                                   if (QP_XML == "")
                                       QP_XML = qpczyBH + "#" + dt_count.Rows.Count.ToString();
                                   else
                                       QP_XML = QP_XML + "," + qpczyBH + "#" + dt_count.Rows.Count.ToString();
                               }
                           }
                           if (QP_XML != "")
                           {
                               rpt_xml = rpt_xml + "<ExamBLItemList>"
                   + "<CheckFlow>" + sqxh + "</CheckFlow>"
                   + "<ExamGroup>" + blh + "</ExamGroup>"
                   + "<ScheduledDate></ScheduledDate>"
                   + "<Note></Note>"
                   + "<Operator>" + QP_XML + "</Operator>"
                   + "<OperatorDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</OperatorDate>"
                   + "<Status>4</Status>"
                   + "<Notice>10</Notice>"
                   + "<ExamAddress></ExamAddress>"
                   + "</ExamBLItemList>";
                           }

                       }

                       #endregion

                   }
                   catch (Exception ee2)
                   {
                       log.WriteMyLog(blh + "：" + ee2.Message);

                       aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='" + ee2.Message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='已审核' ");

                       return;
                   }
                   string rtn_xml = "";
                   log.WriteMyLog(blh + "：" + rpt_xml);

                   rpt_xml = "<Request><ExamStatusBL><ExamBLList>" + rpt_xml + "</ExamBLList></ExamStatusBL></Request>";

                   try
                   {
                       xyfyWeb.DHCPisXiangYaOne xyfy = new xyfyWeb.DHCPisXiangYaOne();
                       xyfy.Url = "http://192.168.2.51:57772/csp/dhcens/DHC.Pis.XiangYaOne.BS.Web.cls";
                       string pathweburl = f.ReadString("savetohis", "wsurl", "");
                       if (pathweburl.Trim() != "")
                           xyfy.Url = pathweburl;

                       rtn_xml = xyfy.DhcService("ExamStatusBL", rpt_xml);
                   }
                   catch (Exception ee4)
                   {

                       log.WriteMyLog(ee4.Message.ToString());
                       aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='连接webservice异常：" + ee4.Message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='已审核' ");
                       return;
                   }
                   try
                   {
                       XmlDataDocument xd = new XmlDataDocument();
                       xd.LoadXml(rtn_xml);
                       XmlNode xn = xd.SelectSingleNode("/Response/ExamStatusBLReturn");

                       if (xn["Returncode"].InnerText.ToString() == "0")
                       {
                           log.WriteMyLog(xn["ResultContent"].InnerText.ToString() + "：" + rtn_xml);
                           aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='',F_FSZT='已处理'  where F_blh='" + blh + "' and F_BGZT='已审核' ");
                       }
                       else
                       {
                           log.WriteMyLog(xn["ResultContent"].InnerText.ToString() + "：" + rtn_xml);
                           aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='" + xn["ResultContent"].InnerText.ToString() + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='已审核' ");
                       }
                   }
                   catch (Exception ee3)
                   {

                       log.WriteMyLog(ee3.Message.ToString() + "：" + rtn_xml);
                       aa.ExecuteSQL("update T_JCXX_JXKH set F_bz='" + ee3.Message + "',F_FSZT='未处理'  where F_blh='" + blh + "' and F_BGZT='已审核' ");
                       return;
                   }
               }


        }

        public static string Get_YHBH(string userName,DataTable  DT_YH)
        {


            if (DT_YH == null)
            {
                return userName;
            }
            if (DT_YH.Rows.Count <= 0)
            {
                return userName;
            }
            DataView dv_yh = DT_YH.DefaultView;
            dv_yh.RowFilter = " F_YHMC='" + userName + "'";
            DataTable DT = dv_yh.ToTable();
            if (DT.Rows.Count > 0)
            {
                string yhbh = DT.Rows[0]["F_YHBH"].ToString().Trim();
                if (yhbh == "")
                    return userName;
                else
                    return DT.Rows[0]["F_YHBH"].ToString().Trim();
            }
            else
                return userName;
        }


    }
}
