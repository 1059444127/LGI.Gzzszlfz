using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    static class Program
    {
        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());


            //ServiceBase[] ServicesToRun;

            //// ͬһ�����п������ж���û�������Ҫ��
            //// ��һ��������ӵ��˽����У������������
            //// ������һ������������磬
            ////
            ////   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            ////
            //ServicesToRun = new ServiceBase[] { new Service1() };

            //ServiceBase.Run(ServicesToRun);
        }
    }
}
