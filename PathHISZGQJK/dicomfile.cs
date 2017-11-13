using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using System.Data;
using ZgqClassPub;
using LoadDll;

namespace PathHISJK
{
    class dicomfile
    {
        private LoadDllapi dllload = new LoadDllapi();
        private LoadDllapi dllloadxm = new LoadDllapi();
        public delegate bool wt_setDcmdataset(int lGrp, int lEle, int lInt2, int lInt4, string vr, string lInStr);
        public delegate bool wt_createdicomfile(string bmpfile, string dcmfile);
        public delegate bool wt_getPYM2(string zw,ref string yw);
        public string GetNumFromStr(string blh)
        {
            string x = "";
            for (int i = 0; i < blh.Length; i++)
            {
                try
                {
                    x = x + Convert.ToInt32(blh[i]);
                }
                catch
                { 
                }
            }
            return x;
        }

        public string createdicom(string blh, odbcdb aa,string bmpname,ref string dcmname)
        {
            DataTable jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            if (jcxx.Rows.Count < 1)
            {
                log.WriteMyLog("未找到病理号！");
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
                return "";
            }
            string hl7en2 = "";
            try
            {
                dllloadxm.initPath("wthisapi.dll");
                wt_getPYM2 xmzpy = (wt_getPYM2)dllloadxm.InvokeMethod("wt_getPYM2", typeof(wt_getPYM2));

                xmzpy(jcxx.Rows[0]["F_xm"].ToString().Trim(), ref hl7en2);

                hl7en2 = hl7en2.Trim();

            }
            catch
            { }
            finally
            {
                dllloadxm.freeLoadDll();
            }

            wt_setDcmdataset dcmset = (wt_setDcmdataset)dllload.InvokeMethod("wt_setDcmdataset", typeof(wt_setDcmdataset));

            wt_createdicomfile writedcm = (wt_createdicomfile)dllload.InvokeMethod("wt_createdicomfile", typeof(wt_createdicomfile));
            string xb = "O";
            if (jcxx.Rows[0]["F_xb"].ToString().Trim() == "男") xb = "M";
            if (jcxx.Rows[0]["F_xb"].ToString().Trim() == "女") xb = "F";
            string nl = jcxx.Rows[0]["F_age"].ToString() + "Y";
            if (jcxx.Rows[0]["F_nl"].ToString().IndexOf("岁")>0)  nl = jcxx.Rows[0]["F_age"].ToString() + "Y";
            if (jcxx.Rows[0]["F_nl"].ToString().IndexOf("月")>0)  nl = jcxx.Rows[0]["F_age"].ToString() + "M";
            if (jcxx.Rows[0]["F_nl"].ToString().IndexOf("天")>0)  nl = jcxx.Rows[0]["F_age"].ToString() + "D";
            string brlb = "";
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "住院") brlb = "INPATIENT";
            if (jcxx.Rows[0]["F_brlb"].ToString().Trim() == "门诊") brlb = "OUTPATIENT";
            try
            {
                dcmset(-1, 0x16, -1, -1, "UI", "1.2.840.10008.5.1.4.1.1.7");
                dcmset(0x8, 0x16, -1, -1, "UI", "1.2.840.10008.5.1.4.1.1.7");
                dcmset(0x8, 0x18, -1, -1, "UI", STUDYID);
                dcmset(0x8, 0x20, -1, -1, "DA", Convert.ToDateTime(jcxx.Rows[0]["F_bgrq"].ToString().Trim()).ToString("yyyyMMdd"));
                dcmset(0x8, 0x30, -1, -1, "TM", "000000");
                dcmset(0x8, 0x50, -1, -1, "SH", blh);
                dcmset(0x8, 0x60, -1, -1, "CS", "OT");
                dcmset(0x8, 0x64, -1, -1, "CS", "DI");
                dcmset(0x8, 0x70, -1, -1, "LO", "WUXI LOGENE");
                dcmset(0x8, 0x80, -1, -1, "LO", "institution name");//医院代码

                dcmset(0x10, 0x10, -1, -1, "PN", hl7en2);
                dcmset(0x10, 0x20, -1, -1, "LO", "12301231920");
                dcmset(0x10, 0x40, -1, -1, "CS", xb);
                dcmset(0x10, 0x1010, -1, -1, "AS", nl);
                dcmset(0x18, 0x15, -1, -1, "CS", "");

                dcmset(0x20, 0xd, -1, -1, "UI", STUDYID);
                dcmset(0x20, 0xe, -1, -1, "UI", STUDYID + ".1");
                dcmset(0x20, 0x10, -1, -1, "SH", blh);
                dcmset(0x20, 0x11, -1, -1, "IS", "1");
                dcmset(0x20, 0x13, -1, -1, "IS", "103");
                dcmset(0x28, 0x103, 0, -1, "US", "");
                
                dcmset(0x38, 0x10, 0, -1, "LO", jcxx.Rows[0]["F_mzh"].ToString().Trim());//门诊号
                dcmset(0x38, 0x11, 0, -1, "LO", jcxx.Rows[0]["F_zyh"].ToString().Trim());//住院号
                dcmset(0x38, 0x400, 0, -1, "LO", brlb);
                

                writedcm(bmpname, dcmname);
                
            }
            catch (Exception ex)
            {
                dcmname = "";
                return ex.Message;
            }
            return "true";

        }
    }
}
