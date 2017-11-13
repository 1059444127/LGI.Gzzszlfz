using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ZgqClassPub;

namespace PathHISZGQJK
{
    public class ClassZGQJK
    {
        public IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        public string blh, yymc,bglx,bgxh,dz,czlb,msg,debug;

        public string JKToHIS(string args)
        {
            bglx = ""; bgxh = ""; czlb = "";
            string[] execslb = args.Split(',');
            try
            {
                if (args.Contains(","))
                {
                         int arg = args.IndexOf(",");
                        try
                        {
                            blh = args.Substring(0, arg);
                            string hisbz = args.Substring(arg + 1);
                            if (hisbz == "bz")
                            {
                                yymc = f.ReadString("savetohis", "yymc2", "123").Replace("\0", "");
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
                
                else
                {  //复杂接口,打印时上传
                    //args1[0] = 病理号^cg/bd/bc^bgxh^new/old^save/qxsh
                    if (args.IndexOf("^") > -1)
                    {
                        try
                        {
                            string[] aa = args.Split('^');
                            blh = aa[0].ToString();
                            bglx = aa[1].ToString().ToLower();//cg/bd/bc
                            bgxh = aa[2].ToString();
                            czlb = aa[3].ToString().ToLower();//new/old
                            dz = aa[4].ToString().ToLower();//save/qxsh/dy/qxdy
                        }
                        catch
                        {
                            log.WriteMyLog("传入参数解析出错");
                            return "0";
                        }

                        yymc = f.ReadString("savetohis", "yymc", "123").Replace("\0", "");
                    }
                    else
                        return "0";
                }
            }
            catch
            {
                MessageBox.Show("参数无效");
                return "0";
            }
          

               debug = f.ReadString("savetohis", "debug", "");
               msg = f.ReadString("savetohis", "msg", "");
               if (debug == "1")
                    log.WriteMyLog(yymc+"   "+ args);


                if (yymc == "zsdxzlyypt")// 中山大学肿瘤医院平台
                {
                    zsdxzlyyPT zlyy = new zsdxzlyyPT();
                    zlyy.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    return "1";
                }
                if (yymc == "zjstzyy")//浙江省台州医院
                {
                    zjstzyy gdsz = new zjstzyy();
                    gdsz.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    return "1";
                }
      
                MessageBox.Show(yymc + "无此医院参数zgq！", "ZGQJK", MessageBoxButtons.OK);
                return "1";
        }
    }
}
