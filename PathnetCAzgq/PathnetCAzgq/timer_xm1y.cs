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
          System.Timers.Timer t = new System.Timers.Timer(1000);   //ʵ����Timer�࣬���ü��ʱ��Ϊ10000���룻   
                      
        public   string  read_ini()
        {
               t.Elapsed += new System.Timers.ElapsedEventHandler(theout); //����ʱ���ʱ��ִ���¼��� 
            t.AutoReset = true;   //������ִ��һ�Σ�false������һֱִ��(true)��   
            t.Enabled = true;     //�Ƿ�ִ��System.Timers.Timer.Elapsed�¼���   

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
