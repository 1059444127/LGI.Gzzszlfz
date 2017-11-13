using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace PathHISZGQJK
{
    class httpweb
    {
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder param = new StringBuilder();
            param.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
            param.AppendLine("<soap12:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap12='http://www.w3.org/2003/05/soap-envelope'>");
            param.AppendLine("<soap12:Body>");
            param.AppendLine("<PacsReportBack xmlns='http://tempuri.org/'>");
            param.AppendLine("<pInput>");
            param.AppendLine("");
            param.AppendLine("</pInput>");
            param.AppendLine("</PacsReportBack>");
            param.AppendLine("</soap12:Body>");
            param.AppendLine("</soap12:Envelope>");

            //发起请求
            Uri uri = new Uri("http://192.168.103.12:57772/csp/jhip/JHIP.PACS.BS.PacsWS.cls");
            WebRequest webRequest = WebRequest.Create(uri);
            webRequest.ContentType = "application/soap+xml; charset=utf-8";
            webRequest.Method = "POST";

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                byte[] paramBytes = Encoding.UTF8.GetBytes(param.ToString());
                // myRequest.ContentLength = paramBytes.Length;
                requestStream.Write(paramBytes, 0, paramBytes.Length);
            }
            //响应
            WebResponse webResponse = webRequest.GetResponse();
            using (StreamReader myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
            {
                MessageBox.Show(myStreamReader.ReadToEnd());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string splitChar = DateTime.Now.Ticks.ToString("x");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:1826/ZSZLWebService2010/GetBlReportService.asmx");
                request.Method = "POST";
                request.Headers.Add("authorization", "");
                request.Headers.Add("cache-control", "no-cache");
                request.Accept = "application/xml";
                request.Headers.Add("accept-Language", "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3");
                request.ContentType = "multipart/form-data; boundary=" + splitChar;
                // xml头
                StringBuilder xmlHeadSB = new StringBuilder();
                xmlHeadSB.Append("--" + splitChar);
                xmlHeadSB.Append("\r\n");
                xmlHeadSB.Append("Content-Type:application/octet-stream");
                xmlHeadSB.Append("\r\n");
                xmlHeadSB.Append("Content-Transfer-Encoding:binary");
                xmlHeadSB.Append("\r\n\r\n");
                byte[] HeadByte1 = Encoding.UTF8.GetBytes(xmlHeadSB.ToString());
                //XML
                byte[] xmlByte = Encoding.UTF8.GetBytes("");
                //文件头
                StringBuilder fileHeadSB = new StringBuilder();
                fileHeadSB.Append("--" + splitChar);
                fileHeadSB.Append("\r\n");
                fileHeadSB.Append("Content-Type:application/octet-stream");
                fileHeadSB.Append("\r\n");

                byte[] HeadByte2 = Encoding.UTF8.GetBytes(fileHeadSB.ToString());
                //尾
                byte[] postFootByte = Encoding.UTF8.GetBytes("\r\n--" + splitChar + "--\r\n");
                request.ContentLength = HeadByte1.Length + xmlByte.Length +
                    HeadByte2.Length + postFootByte.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(HeadByte1, 0, HeadByte1.Length);
                stream.Write(xmlByte, 0, xmlByte.Length);
                stream.Write(HeadByte2, 0, HeadByte2.Length);

                stream.Write(postFootByte, 0, postFootByte.Length);
                stream.Flush();
                stream.Close();
                //获取响应报文
                WebResponse response = request.GetResponse();
                Stream stream1 = response.GetResponseStream();

                string shtml = new StreamReader(stream1, Encoding.Default).ReadToEnd();
                MessageBox.Show(shtml);

            }
            catch (WebException ex)
            {
                Stream stream = ex.Response.GetResponseStream();
                string sHtml = new StreamReader(stream, System.Text.Encoding.Default).ReadToEnd();
                MessageBox.Show(sHtml);

            }
            catch (Exception ee2)
            {
                MessageBox.Show(ee2.Message);
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://192.168.103.12:57772/csp/jhip/%25SOAP.WebServiceInvoke.cls?CLS=JHIP.PACS.BS.PacsWS&OP=PacsReportBack" + "?pInput=" +"");
            request.Method = "POST";
            request.ContentType = "text/html;charset=UTF-8";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                MessageBox.Show(retString);
            }
            catch (Exception ee2)
            {
                MessageBox.Show(ee2.Message);
            }





        }

        private void button7_Click(object sender, EventArgs e)
        {
            string url = "http://192.168.103.12:57772/csp/jhip/JHIP.PACS.BS.PacsWS.CLS";
            StringBuilder param = new StringBuilder();
            param.AppendLine("<?xml version='1.0' encoding='utf-8'?>");
            param.AppendLine("<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap12='http://www.w3.org/2003/05/soap-envelope'>");
            param.AppendLine("<soap:Body>");
            param.AppendLine("<PacsReportBack xmlns='http://tempuri.org/'>");
            param.AppendLine("<pInput>");
            param.AppendLine("");
            param.AppendLine("</pInput>");
            param.AppendLine("</PacsReportBack>");
            param.AppendLine("</soap:Body>");
            param.AppendLine("</soap:Envelope>");
            byte[] bs = Encoding.UTF8.GetBytes(param.ToString());
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/soap+xml; charset=utf-8";

            myRequest.ContentLength = bs.Length;
            using (Stream reqStream = myRequest.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            string responseString = string.Empty;
            try
            {
                using (HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse())
                {
                    MessageBox.Show("11");
                    StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                    responseString = sr.ReadToEnd();
                    MessageBox.Show(responseString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
