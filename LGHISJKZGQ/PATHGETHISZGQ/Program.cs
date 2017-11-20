using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


            //ServiceBase[] ServicesToRun;

            //// 同一进程中可以运行多个用户服务。若要将
            //// 另一个服务添加到此进程中，请更改下行以
            //// 创建另一个服务对象。例如，
            ////
            ////   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            ////
            //ServicesToRun = new ServiceBase[] { new Service1() };

            //ServiceBase.Run(ServicesToRun);
        }
    }
}
