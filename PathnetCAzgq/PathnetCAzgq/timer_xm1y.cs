using System;
using System.Collections.Generic;
using System.Text;
using readini;

namespace PathnetCAzgq
{
    class timer_xm1y
    {
        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string aaa = "";
      //  System.Timers.Timer t = new System.Timers.Timer(1000);
          System.Timers.Timer t = new System.Timers.Timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；   
                      
        public   string  read_ini()
        {
               t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //到达时间的时候执行事件； 
            t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件；   

            return aaa;
        }
      public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            string blh = f.ReadString("xm1y", "blh", "");
            string start_show = f.ReadString("xm1y", "startshow", "");
            string start_rtn = f.ReadString("xm1y", "startrtn", "");

            if (start_show == "1" && start_rtn == "1")
            {
                t.Stop();
                aaa = "1";
                
            }

        } 
    }
}
