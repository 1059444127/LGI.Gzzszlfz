using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Globalization;

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
        public void Download(string filePath, string fileName,string localname,out string status)
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
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + ex.Message);
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
            catch(Exception ex)
            {
                //Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
                //status = "Error";
            }

        }

        public void Uadatedir(string dirname,string desname, out string status)
        {
            status = "OK";



            string uri = ftpURI + dirname;

            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
           

           
           reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
           reqFTP.Method = WebRequestMethods.Ftp.Rename;
           reqFTP.RenameTo = desname;
            try
            {
                FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                response.Close();

            }
            catch (Exception ex)
            {
                //Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
                //status = "Error";
            }

        }

        public void Upload(string filename, string blh, out string status)        //上传单个文件
        {
            status = "OK";

            FileInfo fileInf = new FileInfo(filename);

            string uri = ftpURI +blh+"/" +fileInf.Name;
            if (blh=="")
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
        //public void UploadFile(string file, string blh, out string status)//上传整个文件夹
        //{
        //    status = "OK";

        //    FileInfo fileInf = new FileInfo(filename);

        //    string uri = ftpURI + blh + "/" + fileInf.Name;
        //    if (blh == "")
        //        uri = ftpURI + fileInf.Name;
        //    FtpWebRequest reqFTP;



        //    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

        //    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
        //    reqFTP.KeepAlive = false;
        //    //try
        //    //{
        //    //    reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

        //    //   FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

        //    //    response.Close();
        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    MessageBox.Show(ex.ToString());
        //    //}




        //    reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

        //    reqFTP.UseBinary = true;

        //    reqFTP.ContentLength = fileInf.Length;

        //    int buffLength = 2048;

        //    byte[] buff = new byte[buffLength];

        //    int contentLen;

        //    FileStream fs = fileInf.OpenRead();

        //    try
        //    {

        //        Stream strm = reqFTP.GetRequestStream();

        //        contentLen = fs.Read(buff, 0, buffLength);

        //        while (contentLen != 0)
        //        {

        //            strm.Write(buff, 0, contentLen);

        //            contentLen = fs.Read(buff, 0, buffLength);

        //        }

        //        strm.Close();

        //        fs.Close();

        //    }

        //    catch (Exception ex)
        //    {

        //        Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
        //        status = "Error";
        //    }

        //}  
    }
    public class Insert_Standard_ErrorLog
    {
        public static void Insert(string x, string y)
        {
            MessageBox.Show(y);
            Application.Exit();
        }
    }

}

namespace PathHISZGQJK
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
        public void Download(string filePath, string fileName, string localName, out string status)
        {
            string err_msg = "";
            Download(filePath, fileName, localName, out  status, ref  err_msg);
        }
        public void Download(string filePath, string fileName, string localName, out string status, ref string err_msg)
        {
           
            FtpWebRequest reqFTP;
            status = "OK";
            for (int x = 0; x < 3; x++)
            {
                status = "OK";
                try
                {
                    FileStream outputStream = new FileStream(filePath + "\\" + localName, FileMode.Create);
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
                    err_msg = "Download Error -->" + filePath + "\\" + localName + "-->" + ex.Message;
                    status = "Error";
                }

                if (status == "OK")
                    break;

                if (File.Exists(filePath + "\\" + localName))
                    break;
            }
        }

        public void Makedir(string dirname, out string status)
        {
            Makedir("", dirname, out  status);
        }
        public void Makedir(string filePath, string dirname, out string status)
        {
            status = "OK";
            string uri = ftpURI + dirname;
            if (filePath!="")
               uri= filePath + dirname;
            FtpWebRequest reqFTP;

                status = "OK";
                try
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                    reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                    FtpWebResponse response = reqFTP.GetResponse() as FtpWebResponse;
                }
                catch (Exception ex)
                {
                    Insert_Standard_ErrorLog.Insert("FtpWeb", "Error -->创建目录异常" + uri + "-->" + ex.Message);
                    status = "Error";
                }
            

        }


        public void Upload(string filename, string path, out string status)
        {
            string msg = "";
            Upload(filename, path,"", out  status, ref  msg);
        }
        public void Upload(string filename, string path, out string status, ref string msg)
        {
            Upload(filename, path, "", out  status, ref  msg);
        }
        public void Upload(string localFile,string path, string ftpFileName, out string status,ref string msg)
        {
            status = "OK";
            FileInfo fileInf = new FileInfo(localFile);
            if (ftpFileName == "")
                ftpFileName = fileInf.Name;

            string uri = ftpURI + ftpFileName;
            if (path != "")
                uri = ftpURI + "/" + path + "/" + ftpFileName;

            for (int x = 0; x < 3; x++)
            {
                status = "OK";
                FtpWebRequest reqFTP;
                try
                {
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
                    Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error -->上传异常" + fileInf.Name + "--" + uri + "-->" + ex.Message);
                    status = "Error";
                    msg = "Upload Error -->上传异常" + fileInf.Name + "--" + uri + "-->" + ex.Message;
                }

                if (status == "OK")
                 break;
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
                ZgqClassPub.log.WriteMyLog(ee.Message);
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
                ZgqClassPub.log.WriteMyLog(y);
                Application.Exit();
            }
        }
    }

}
