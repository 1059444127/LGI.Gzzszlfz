using System;
using System.Collections.Generic;
using System.Text;
using dbbase;
using readini;

using LGZGQClass;


namespace mysj
{
    public class bghj
    {
        private static sqldb aa = new sqldb(System.Windows.Forms.Application.StartupPath, "sqlserver");
        private static readini.IniFiles f = new IniFiles("sz.ini");
        public static void writebg(string F_bgnr,string F_blh,string F_dz,string F_ctmc)
        {
            string pcname = System.Environment.MachineName;
            string czy = "";
            string czrq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string BLH = F_blh;
            string dz = F_dz;
            string nr = F_bgnr;
            //string exemc = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string exemc = "MYSJ";
            string ctmc = F_ctmc;

            string[] values = new string[8] { BLH, czrq, czy, pcname, dz, nr, exemc, ctmc };
            string[] fields = new string[8] { "F_blh", "F_rq", "F_czy", "F_wz", "F_dz", "F_nr", "F_exemc", "F_ctmc" };

            if (aa.insertsql("T_bghj", ref fields, ref values))
            { }
            else
            {
                log.WriteMyLog("±®∏Ê∫€º£–¥»Î ß∞‹£°"); 
            }

        }
    }
}
