using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using HL7;

namespace PathHISJK
{
    public class FtpWeb
    {
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="FtpUserID">用户名</param>
        /// <param name="FtpPassword">密码</param>
        public FtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            if (FtpRemotePath != "")
            {
                ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
            }
            else
            {
                ftpURI = "ftp://" + ftpServerIP + "/";
            }
           
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void Download(string filePath, string fileName, string localname, out string status)
        {
            status = "OK";
            FtpWebRequest reqFTP;
        try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + localname, FileMode.Create);
              
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
           
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
              
                ftpStream.Close();
                outputStream.Close();
                response.Close();
              
            }
            catch (Exception ex)
            {
       
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + filePath + "\\" + localname + "-->"  + ex.Message);
                status = "Error:";
            }
        }

        public void Download(string filePath, string fileName, string localname,out string status, ref string err_msg)
        {
            status = "OK";
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + localname, FileMode.Create);
             
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
                
            }
            catch (Exception ex)
            {

             err_msg="Download Error -->"+filePath + "\\" + localname+"-->" + ex.Message;
             status = "Error";
            }
        }

        public void Makedir(string dirname, out string status)
        {
            status = "OK";
            string uri = ftpURI + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri+"-->" + ex.Message);
                status = "Error";
            }

        }

        public void Makedir(string filePath, string dirname, out string status)
        {
            status = "OK";
            string uri = filePath + dirname;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Error --> " + uri + "-->" + ex.Message);
                status = "Error";
            }

        }

        //public void Makedir(string dirname, out string status)
        //{
        //    status = "OK";



        //    string uri = ftpURI + dirname;

        //    FtpWebRequest reqFTP;



        //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

        //    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        //    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

        //    try
        //    {
        //        FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;

        //    }
        //    catch
        //    {
        //        //Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
        //        //status = "Error";
        //    }

        //}


        public void Upload(string filename, string path, out string status,ref string msg)
        {
         
              
            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + path + "/" + fileInf.Name;
            if (path == "")
                uri = ftpURI + fileInf.Name;

            FtpWebRequest reqFTP;
           try
            {
           reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            //try
            //{
            //    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

            //   FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

            //    response.Close();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}


            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            FileStream fs = fileInf.OpenRead();

         
                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->" + uri+"-->"  + ex.Message);
                status = "Error";
                msg = "Upload Error --> " + uri+"-->" + ex.Message;
            }

        }

        public void Upload(string filename, string path, out string status)
        {
            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + path + "/" + fileInf.Name;
            if (path == "")
                uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            FileStream fs = fileInf.OpenRead();

            try
            {

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }
            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + uri+"-->" + ex.Message);
                status = "Error";
                
            }

        }

        public void Upload(string filename,string name, string blh, out string status)
        {
            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI + "/" + blh + "/" + name;
            if (blh == "")
                uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            //try
            //{
            //    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

            //   FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

            //    response.Close();
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}


            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

            reqFTP.UseBinary = true;

            reqFTP.ContentLength = fileInf.Length;

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            FileStream fs = fileInf.OpenRead();

            try
            {

                Stream strm = reqFTP.GetRequestStream();

                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
                status = "Error";
            }

        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public bool fileDelete(string ftpPath, string ftpName)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
               // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath + "//" + ftpName;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();
               
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        public bool fileDelete(string ftpPath)
        {

            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                // FileInfo fileInf = new FileInfo(filename);

                string uri = ftpPath;

                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                try
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                    if (ftpResponseStream != null)
                    {
                        ftpResponseStream.Close();
                    }
                    if (ftpWebResponse != null)
                    {
                        ftpWebResponse.Close();
                    }
                }
                catch
                {
                }
            }
            return success;
        }
        /// <summary>
        /// 文件存在检查
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="ftpName"></param>
        /// <returns></returns>
        public bool fileCheckExist(string ftpPath, string ftpName)
        {
           
                string url = ftpPath;

                bool success = false;
                FtpWebRequest ftpWebRequest = null;
                WebResponse webResponse = null;
                StreamReader reader = null;
                try
                {
                    ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(@url));
                    ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    ftpWebRequest.KeepAlive = false;
                    webResponse = ftpWebRequest.GetResponse();
                    reader = new StreamReader(webResponse.GetResponseStream());
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        if (line == ftpName)
                        {
                            success = true;
                            break;
                        }
                        line = reader.ReadLine();
                    }
                }
                catch (Exception ee)
                {
                    log.WriteMyLog(ee.Message);
                    success = false;
                }
                finally
                {
                    try
                    {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (webResponse != null)
                    {
                        webResponse.Close();
                    } 
                    }
                    catch
                    {
                      //   LGZGQClass.log.WriteMyLog("关闭数据流异常");
                    }
                }
                return success;
           
        }


    
    public class Insert_Standard_ErrorLog
    {
        public static void Insert(string x, string y)
        {
          //  MessageBox.Show(y);
            log.WriteMyLog(y);
            Application.Exit();
        }
    }
    }

}
