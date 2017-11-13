using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using dbbase;
using System.Windows.Forms;
using PathHISZGQJK;
using ZgqClassPub;
using LoadDll;

namespace GLYYTEST
{
    class dicomfilezh
    {
        private LoadDllapi dllload = new LoadDllapi();
        private LoadDllapi dllloadxm = new LoadDllapi();
        public delegate bool wt_setDcmdataset(int lGrp, int lEle, int lInt2, int lInt4, string vr, string lInStr);
        public delegate bool wt_createdicomfile(string bmpfile, string dcmfile);

        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + int.Parse(blh[i].ToString()).ToString();
                }
                catch
                {
                }
            }
            return x;
        }
        private static IniFiles2 f = new IniFiles2(Application.StartupPath + "\\sz.ini");

        public string jsnl(DataTable dt)
        {

            string brnl = "";
            try
            {
                string brxxnl = dt.Rows[0]["F_zy"].ToString();
                DateTime xx = Convert.ToDateTime(brxxnl);

                return xx.ToString("yyyyMMdd");
            }
            catch
            { }
            try
            {
                //读取计算日期：收到日期，报告日期，当天
                DateTime date_brnl = DateTime.Now;
                try
                {
                    date_brnl = Convert.ToDateTime(dt.Rows[0]["F_sdrq"].ToString().Trim());
                }
                catch
                {
                    try
                    {
                        date_brnl = Convert.ToDateTime(dt.Rows[0]["F_bgrq"].ToString().Trim());
                    }
                    catch
                    {
                        date_brnl = DateTime.Now;
                    }

                }

                string nl = dt.Rows[0]["F_nl"].ToString().Trim();
                string nltype = nl.Substring(nl.Length - 1, 1);
                int nls = 0;
                try
                {
                    nls = Convert.ToInt16(nl.Substring(0, nl.Length - 1));
                }
                catch
                {
                    try
                    {
                        nls = Convert.ToInt16(nl.ToString());
                    }
                    catch
                    {
                        nls = 0;
                    }
                }
                if (nltype == "岁")
                {
                    date_brnl = date_brnl.AddYears(0 - nls);
                    brnl = date_brnl.ToString("yyyy") + "0101";
                }
                else
                {
                    brnl = DateTime.Now.ToString("yyyy") + "0101";

                }


            }
            catch
            {
                brnl = DateTime.Now.ToString("yyyy") + "0101";
            }

            return brnl;

        }

        public string createdicom(string blh, sqldb aa, string bmpname, ref string dcmname, string bmpint)
        {
            string yydm = f.ReadString("dicom", "yydm", "南京鼓楼医院").Replace("\0", "");
            string sbmc = f.ReadString("dicom", "sbmc", "PIS").Replace("\0", "");
            if (bmpint == "")
            {
                bmpint = bmpname.Substring(bmpname.Length - 7, 3);
            }
            DataTable jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            if (jcxx.Rows.Count < 1)
            {
                log.WriteMyLog("未找到病理号！");
                dcmname = "";
                return "";
            }
            string STUDYID = jcxx.Rows[0]["F_STUDY_UID"].ToString().Trim();
            if (STUDYID == "")
            {
                STUDYID = "72.67.90.68.71." + GetNumFromStr(blh) + "." + DateTime.Now.ToString("yyyyMMdd.HHss");
                aa.ExecuteSQL("update T_jcxx set F_STUDY_UID ='" + STUDYID + "' where F_blh='" + blh + "'");
            }
            if ((int)dllload.initPath("wtimgapi.dll") == 0)
            {
                log.WriteMyLog("dicom生成控件调用错误！");
                dcmname = "";
                return "";
            }

            string brsr = jsnl(jcxx);

            System.Drawing.Bitmap BMP1 = new System.Drawing.Bitmap(bmpname);
            int BH = BMP1.Height;
            int BW = BMP1.Width;
            BMP1.Dispose();
            wt_setDcmdataset dcmset = (wt_setDcmdataset)dllload.InvokeMethod("wt_setDcmdataset", typeof(wt_setDcmdataset));

            wt_createdicomfile writedcm = (wt_createdicomfile)dllload.InvokeMethod("wt_createdicomfile", typeof(wt_createdicomfile));
            string xb = "O";
            if (jcxx.Rows[0]["F_xb"].ToString().Trim() == "男") xb = "M";
            if (jcxx.Rows[0]["F_xb"].ToString().Trim() == "女") xb = "F";
            string nl = jcxx.Rows[0]["F_age"].ToString() + "Y";
            string ss = jcxx.Rows[0]["F_nl"].ToString();
            if (ss.IndexOf("岁") > 0)
            {
                nl = ss.Substring(0, ss.IndexOf("岁")) + "Y";
            }
            else if (ss.IndexOf("月") > 0)
            {
                nl = ss.Substring(0, ss.IndexOf("岁")) + "M";
            }
            else if (ss.IndexOf("天") > 0)
            {
                nl = ss.Substring(0, ss.IndexOf("岁")) + "D";
            }
            string brlb = "";

            nl = "000" + nl;
            nl = nl.Substring(nl.Length - 4, 4);

            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院") brlb = "INPATIENT";
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "门诊") brlb = "OUTPATIENT";
            try
            {
                dcmset(-1, 0x16, -1, -1, "UI", "1.2.840.10008.5.1.4.1.1.7");

               
                //dcmset(0x2, 0x1, -1, -1, "OB", "00");
                dcmset(0x2, 0x2, -1, -1, "UI", STUDYID);
                dcmset(0x2, 0x3, -1, -1, "UI", STUDYID + ".1." + bmpint);
                dcmset(0x2, 0x10, -1, -1, "UI", "1.2.840.10008.1.2.1");
                dcmset(0x2, 0x12, -1, -1, "UI", "1.3.12.2.1107.5.8.2");
                dcmset(0x2, 0x13, -1, -1, "SH", "SHS_MV300_VA40B");
                dcmset(0x2, 0x16, -1, -1, "AE", "MagicView 300");

                dcmset(0x8, 0x5, -1, -1, "CS", "GB18030");
                dcmset(0x8, 0x8, -1, -1, "CS", "ORIGINAL");
                //   dcmset(0x8, 0x12, -1, -1, "DA", DateTime.Now.ToString("yyyyMMdd"));
                //   dcmset(0x8, 0x13, -1, -1, "TM", DateTime.Now.ToString("HHmmss"));

                dcmset(0x8, 0x16, -1, -1, "UI", STUDYID);
                
                dcmset(0x8, 0x18, -1, -1, "UI", STUDYID + ".1." + bmpint);

                dcmset(0x8, 0x20, -1, -1, "DA", Convert.ToDateTime(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMdd"));
                dcmset(0x8, 0x21, -1, -1, "DA", Convert.ToDateTime(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMdd"));
                //    dcmset(0x8, 0x21, -1, -1, "DA", Convert.ToDateTime(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMdd"));
                //    dcmset(0x8, 0x22, -1, -1, "DA", Convert.ToDateTime(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMdd"));
                //    dcmset(0x8, 0x23, -1, -1, "DA", Convert.ToDateTime(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMdd"));

                dcmset(0x8, 0x30, -1, -1, "TM", "010101");
                dcmset(0x8, 0x31, -1, -1, "TM", "000000");
                //    dcmset(0x8, 0x32, -1, -1, "TM", "000000");
                //    dcmset(0x8, 0x33, -1, -1, "TM", "000000");

                dcmset(0x8, 0x50, -1, -1, "SH", blh);
                dcmset(0x8, 0x54, -1, -1, "AE", "");

                dcmset(0x8, 0x60, -1, -1, "CS", sbmc);

                dcmset(0x8, 0x64, -1, -1, "CS", "DI");
                dcmset(0x8, 0x70, -1, -1, "LO", "WUXI LOGENE");
                dcmset(0x8, 0x80, -1, -1, "LO", yydm);//医院代码

                dcmset(0x8, 0x90, -1, -1, "PN", "");
                //   dcmset(0x8, 0x1010, -1, -1, "SH", " ");
                dcmset(0x8, 0x1030, -1, -1, "LO", "NORMAL");
                dcmset(0x8, 0x1080, -1, -1, "LO", "");
                //jcxx.Rows[0]["F_xm"].ToString().Trim()
                dcmset(0x10, 0x10, -1, -1, "PN", jcxx.Rows[0]["F_xm"].ToString().Trim());
                //dcmset(0x10, 0x10, -1, -1, "PN", hl7en2);

                //dcmset(0x10, 0x20, -1, -1, "LO", jcxx.Rows[0]["F_YZID"].ToString().Trim());

                dcmset(0x10, 0x20, -1, -1, "LO", jcxx.Rows[0]["F_brbh"].ToString().Trim());

                //dcmset(0x10, 0x21, -1, -1, "LO", "SITE_DEFAULT_LOCAL");

                dcmset(0x10, 0x30, -1, -1, "DA", brsr);


                dcmset(0x10, 0x40, -1, -1, "CS", xb);
                dcmset(0x10, 0x1000, -1, -1, "AS", jcxx.Rows[0]["F_BRBH"].ToString().Trim());
                dcmset(0x10, 0x1010, -1, -1, "AS", nl);

                //dcmset(0x18, 0x15, -1, -1, "CS", "");

                // dcmset(0x18, 0x1020, -1, -1, "LO", "4.0.106A (VE25A  SL08P13)");
                // dcmset(0x18, 0x5010, -1, -1, "LO", "CH4-1");
                // dcmset(0x18, 0x5022, -1, -1, "DS", "1.03575527668");

                dcmset(0x20, 0xd, -1, -1, "UI", STUDYID);
                dcmset(0x20, 0xe, -1, -1, "UI", STUDYID + ".1");
                dcmset(0x20, 0x10, -1, -1, "SH", blh);
                dcmset(0x20, 0x11, -1, -1, "IS", "1");
                dcmset(0x20, 0x13, -1, -1, "IS", "103");
                //dcmset(0x20, 0x1209, -1, -1, "IS", "2");

                //dcmset(0x20, 0x20, -1, -1, "CS", " ");
                //dcmset(0x20, 0x60, -1, -1, "CS", " ");
                //dcmset(0x20, 0x4000, -1, -1, "LT", " ");

                //dcmset(0x28, 0x2, 3, -1, "US", "");
                //dcmset(0x28, 0x4, -1, -1, "CS", "RGB");
                //dcmset(0x28, 0x26, -1, -1, "CS", "RGB");
                dcmset(0x28, 0x6, 0, -1, "US", "");
                //dcmset(0x28, 0x8, -1, -1, "IS", "1");

                // dcmset(0x28, 0x10, BH, -1, "US", "");
                // dcmset(0x28, 0x11, BW, -1, "US", "");
                dcmset(0x28, 0x14, 0, -1, "US", "");
                // dcmset(0x28, 0x100, 8, -1, "US", "");
                // dcmset(0x28, 0x101, 8, -1, "US", "");
                // dcmset(0x28, 0x102, 7, -1, "US", "");
                dcmset(0x28, 0x103, 0, -1, "US", "");


                //dcmset(0x38, 0x10, 0, -1, "LO", jcxx.Rows[0]["F_mzh"].ToString().Trim());//门诊号
                //dcmset(0x38, 0x11, 0, -1, "LO", jcxx.Rows[0]["F_zyh"].ToString().Trim());//住院号
                //  dcmset(0x38, 0x400, 0, -1, "LO", brlb);
                //FileStream fs = new FileStream(bmpname, FileMode.Open);


                writedcm(bmpname, dcmname);

            }
            catch (Exception ex)
            {
                dcmname = "";
                return ex.Message;
            }
            finally
            {
                dllload.freeLoadDll();
            }
            return "true";

        }
    }
}
