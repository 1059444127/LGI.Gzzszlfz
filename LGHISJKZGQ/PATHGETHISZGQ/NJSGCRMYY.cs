using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    class NJSGCRMYY
    {

        private static LGHISJKZGQ.IniFiles f = new LGHISJKZGQ.IniFiles(Application.StartupPath + "\\sz.ini");

        /// <summary>
        ///  // @brlb ut_bz,         --病人类别   0门诊 1住院 3体检      
        //@codetype ut_bz,      --号码类型   1住院/门诊号指病历号，2卡号， 3PatientID       
        //   --对应于his中的 patid(门诊),patid(住院)， 4CureNo对应于his,9发票号（门诊使用）      
        //   --中的ghsjh (门诊),和syxh(住院)      
        //   @code  varchar(20)    --号码，它的含义，由上面codetype指定      
        //exec usp_yjjk_getbrxx  '1','4','285606'



        //测试:
        //his数据库  192.168.42.107\this4  
        //用户：sa/sql2k
        //库：this4_0720
        //存储过程：usp_yjjk_getbrxx


        // @brlb ut_bz,         --病人类别   0门诊 1住院 3体检      
        // @codetype ut_bz,      --号码类型   1住院/门诊号指病历号，2卡号，       
        //    --对应于his中的 patid(门诊),patid(住院)， 9发票号（门诊使用）      
        //    --中的ghsjh (门诊),和syxh(住院)      
        //    @code  varchar(20)    --号码，它的含义，由上面codetype指定    


        //patientID   patientID
        //HospNo     病员号  病历号
        //patName  病人姓名
        //Sex   性别  1 男，2女
        //age+ageUnit  年龄
        //WardOrReg  病人类别   0门诊 1住院 3体检      
        //ChargeType  费别 
        //CureNo   住院号/门诊号
        //CardNo  卡号
        //applyDept 科室 代码  
        //select  name from YY_KSBMK  where id ='409'
        //ward   病区代码  ZY_BQDMK
        //bedNo      床号
        //applyDoctor  医生代码  YY_ZGBMK
        //select  name from YY_ZGBMK  where id ='5358'
        //clincDesc  临床诊断。
        //IDNum    身份号
        //Phone     电话
        //Address    地址
        //Zip  
        //Career    职业
        //Nation   民族
        //ToDoc     
        //sendNo   
        //Syxh    首页序号
        //bqmc        病区
        //yexh    
        //DeptName    科室名称
        //clinicDesc  临床诊断
        /// </summary>
        /// <param name="Sslbx"></param>
        /// <param name="Ssbz"></param>
        /// <param name="Debug"></param>
        /// <returns></returns>
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            if (Sslbx != "")
            {
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=192.168.42.107\\this4;Initial Catalog=THIS4_0720;User Id=sa;Password=sql2k;");
                string brlb = "";
                string codetype = "";

                if (Sslbx == "门诊病历号")
                {
                    brlb = "0";
                    codetype = "1";
                }
                if (Sslbx == "门诊号")
                {
                    brlb = "0";
                    codetype = "4";
                }
                if (Sslbx == "卡号")
                {
                    brlb = "0";
                    codetype = "2";
                }
                if (Sslbx == "发票号")
                {
                    brlb = "0";
                    codetype = "9";
                }
              
                if (Sslbx == "住院病历号")
                {
                    brlb = "1";
                    codetype = "1";
                }
                if (Sslbx == "住院号")
                {
                    brlb = "1";
                    codetype = "4";
                }
                if (Sslbx == "体检号")
                {
                    brlb = "3";
                    codetype = "1";
                }
                if (Sslbx == "体检卡号")
                {
                    brlb = "3";
                    codetype = "2";
                }
                if (Sslbx == "体检发票号")
                {
                    brlb = "3";
                    codetype = "9";
                }

                if (brlb == "")
                {
                    MessageBox.Show("未设置此识别类型" + Sslbx); return "0";
                }

                DataTable dt = new DataTable();
                string exec = "";

                SqlDB_ZGQ sql = new SqlDB_ZGQ();
                dt = sql.Sql_DataAdapter(odbcsql, "exec usp_yjjk_getbrxx  '" + brlb + "','" + codetype + "','" + Ssbz.Trim() + "'", ref exec);

                if (exec != "")
                {
                    MessageBox.Show(exec);
                    return "0";
                }
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("未能查询到对应的数据记录！请确认" + Sslbx + "是否正确");
                    return "0";
                }
                //无数据集时对方返回  select "F","没有指定的条件！"
                try
                {
                    MessageBox.Show(dt.Rows[0]["Column2"].ToString().Trim());
                    return "0";
                }
                catch
                {
                }

                int count = 0;

                    DataColumn dc0 = new DataColumn("序号");
                   dt.Columns.Add(dc0);

                   DataColumn dc_brlb = new DataColumn("病人类别");
                   dt.Columns.Add(dc_brlb);

                    for (int x = 0; x <dt.Rows.Count; x++)
                    {
                        dt.Rows[x][dt.Columns.Count - 2] = x;
                        //通过医生代码查询医生姓名
                       try
                       {
                           DataTable dt_ys = new DataTable();
                           dt_ys = sql.Sql_DataAdapter(odbcsql, "select top 1 name from YY_ZGBMK  where id ='" + dt.Rows[x]["ApplyDoctor"].ToString().Trim() + "'", ref exec);
                           if (dt_ys.Rows.Count > 0)
                               dt.Rows[x]["ApplyDoctor"] = dt_ys.Rows[0]["name"].ToString().Trim();
                       }
                       catch
                       {
                       }
                       //DeptName为空的情况下，通过科室代码代码查询科室名称
                       if (dt.Rows[x]["DeptName"].ToString().Trim() == null || dt.Rows[x]["DeptName"].ToString().Trim() == "")
                       {
                           try
                           {
                               DataTable dt_ks = new DataTable();
                               dt_ks = sql.Sql_DataAdapter(odbcsql, "select top 1 name from YY_KSBMK  where id ='" + dt.Rows[x]["ApplyDept"].ToString().Trim() + "'", ref exec);
                               if (dt_ks.Rows.Count > 0)
                                   dt.Rows[x]["DeptName"] = dt_ks.Rows[0]["name"].ToString().Trim();
                           }
                           catch
                           {
                           }
                       }

                       // --病人类别   0门诊 1住院 3体检      
                       dt.Rows[0]["病人类别"] = dt.Rows[0]["WardOrReg"].ToString().Trim();
                        if (dt.Rows[0]["WardOrReg"].ToString().Trim() == "1")
                           dt.Rows[0]["病人类别"] = "住院";
                         if (dt.Rows[count]["WardOrReg"].ToString().Trim() == "3")
                             dt.Rows[0]["病人类别"] = "体检";
                         if (dt.Rows[count]["WardOrReg"].ToString().Trim() == "0")
                             dt.Rows[0]["病人类别"] = "门诊";
                    }


                    // 多条数据显示选择窗体
                    if (dt.Rows.Count >= 1)
                    {
                        //dataGridView1显示样式
                        string xsys = f.ReadString(Sslbx, "xsys", "1");
                        //显示的列字段
                        string Columns = f.ReadString(Sslbx, "Columns", "HospNo,病人类别,PatName,Age,CureNo,CardNo,bqmc,BedNo,DeptName,ApplyDoctor,ClinicDesc");
                        //显示的列的别名
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "病历号,类别,姓名,年龄,住院号/门诊号,卡号,病区,床号,申请科室,申请医生,临床诊断");


                        //增加序号列
                    if (Columns.Trim() != "")
                        Columns = "序号,"+Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "序号,"+ColumnsName;



                   //显示窗体
                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt, Columns, ColumnsName, xsys);
                    yc.ShowDialog();
                    string rtn2 = yc.F_XH;

                    if (rtn2.Trim() == "")
                    {
                        MessageBox.Show("未选择数据！");
                        return "0";
                    }
                    try
                    {
                        count = int.Parse(rtn2);
                    }
                    catch
                    {
                        MessageBox.Show("请重新选择！");
                        return "0";
                    }
                }
               

                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";

                    xml = xml + "病人编号=" + (char)34 + dt.Rows[count]["PatientID"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "就诊ID=" + (char)34 + dt.Rows[count]["HospNo"].ToString().Trim() + "^" + dt.Rows[count]["CardNo"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                        if (dt.Rows[count]["WardOrReg"].ToString().Trim() == "1")
                        {
                            xml = xml + "门诊号=" + (char)34 +"" + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + dt.Rows[count]["CureNo"].ToString().Trim() + (char)34 + " ";
                        }
                        else
                        {
                            xml = xml + "门诊号=" + (char)34 + dt.Rows[count]["CureNo"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                        }

                        xml = xml + "姓名=" + (char)34 + dt.Rows[count]["PatName"].ToString().Trim() + (char)34 + " ";
                        string xb = dt.Rows[count]["Sex"].ToString().Trim();
                        if (xb == "2")
                            xb = "女";
                        if (xb == "1")
                            xb = "男";
                        xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";

                        xml = xml + "年龄=" + (char)34 + dt.Rows[count]["Age"].ToString().Trim() + dt.Rows[count]["AgeUnit"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "地址=" + (char)34 + dt.Rows[count]["Address"].ToString().Trim() + (char)34 + "   ";
                        xml = xml + "电话=" + (char)34 + dt.Rows[count]["Phone"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病区=" + (char)34 + dt.Rows[count]["bqmc"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "床号=" + (char)34 + dt.Rows[count]["BedNo"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "身份证号=" + (char)34 + dt.Rows[count]["IDNum"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "民族=" + (char)34 + dt.Rows[count]["Nation"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "职业=" + (char)34 + dt.Rows[count]["Career"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检科室=" + (char)34 + dt.Rows[0]["DeptName"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "送检医生=" + (char)34 + dt.Rows[0]["ApplyDoctor"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                        xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "费别=" + (char)34 + dt.Rows[count]["ChargeType"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "病人类别=" + (char)34 + dt.Rows[0]["病人类别"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                        xml = xml + "<临床诊断><![CDATA[" + dt.Rows[count]["ClinicDesc"].ToString().Trim() + "]]></临床诊断>";    
                       xml = xml + "</LOGENE>";
                       return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("提取信息出现异常：" + e.Message);
                    return "0";
                }

            } return "0";
        }
    }
}






