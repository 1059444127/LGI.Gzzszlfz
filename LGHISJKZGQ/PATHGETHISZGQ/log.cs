using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace LGHISJKZGQ
{
    class log
    {
        public static void WriteMyLog(string message)
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            try
            {
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\PATHGETHISZGQ" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
                {
                    File.Create(filePath).Close();
                }
                StreamWriter sw = File.AppendText(filePath);

                sw.WriteLine("【" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】" + message);
               
                sw.WriteLine();
                sw.Close();
            }
            catch
            { }
        }
        public static string readlog()
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            string hl7log = "";
            try
            {
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\PATHGETHISZGQ-" + DateTime.Now.ToShortDateString() + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
                {
                    File.Create(filePath).Close();                 
                }
                hl7log = File.ReadAllText(filePath);
                return hl7log;
            }
            catch
            {
                return "";
            }

 
        }
        public static void clearlog()
        {
            string LOG_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "Log";
            //string hl7log = "";
            try
            {
                //日志文件路径 
                string filePath = LOG_FOLDER + "\\PATHGETHISZGQ-" + DateTime.Now.ToShortDateString() + ".log";
                if (!System.IO.Directory.Exists(LOG_FOLDER))
                {
                    Directory.CreateDirectory(LOG_FOLDER);
                }
                if (!File.Exists(filePath))//如果文件不存在 
                {
                    File.Create(filePath).Close();
                }
                File.WriteAllText(filePath, "");
                
            }
            catch
            {
                
            }
        }

    }
}
