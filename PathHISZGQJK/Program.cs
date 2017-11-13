using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZgqClassPub;

namespace PathHISZGQJK
{
    public static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        public static void Main(string[] Args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Args.Length>0)
            {
                Application.Run(new Form1(Args));
            }
            else
            {
                Application.Run(new FormTest());
            }
        }

        public static void Send(string[] Args)
        {
            IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

            var debug = f.ReadString("savetohis", "debug", "");
            var msg = f.ReadString("savetohis", "msg", "");
            string[] aa = Args[0].Split('^');
            var execslb = aa;
            var blh = aa[0].ToString();
            var bglx = aa[1].ToString().ToLower(); //cg/bd/bc
            var bgxh = aa[2].ToString();
            var czlb = aa[3].ToString().ToLower(); //new/old
            var dz = aa[4].ToString().ToLower(); //save/qxsh/dy/qxdy
            zsdxzlyyfzPT zlyy = new zsdxzlyyfzPT();
            zlyy.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
        }
    }
}