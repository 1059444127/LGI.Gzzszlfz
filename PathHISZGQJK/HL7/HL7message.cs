using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using ZgqClassPub;

namespace HL7
{
    class HL7message
    {
        public TcpClient client;
        public string serverstatus = "";
        public static string localstatus = "";
        private bool xx = true;
        public static bool yy = true;
        private Socket clientsocket;
        
        public NetworkStream netStream;
        private int port;
        private Socket sock;
        public Thread xxx;
        public TcpListener listener;
    
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private Thread thread;
        public void sendmessage(ref byte[] by3, string hl7server, string hl7port, ref string result, ref string ErrMsg, string debug)
        {
            result = "00";
            try
            {
              string  x=  connectServer(hl7server, hl7port, ref ErrMsg, debug);

              if (x == "-1")
              {
                  result = "-1";
                  return;
              }
                netStream = client.GetStream();
                netStream.Write(by3, 0, by3.Length);
                netStream.Flush();

                //����ACK�ظ�
                client.ReceiveTimeout = 10000;
                byte[] bb = new byte[4096];
                netStream.Read(bb, 0, 4096);
                string jsbb = System.Text.Encoding.UTF8.GetString(bb).Replace("\0", "");

                if (debug == "1")
                    log.WriteMyLog(jsbb);

                ErrMsg = jsbb;

                if (jsbb.IndexOf("MSA|AA") > 0)
                {
                    result = "99";
                }
                else if (jsbb.IndexOf("NO_REPORT_RECORD") > 0)
                {
                    result = "88";
                }
                else
                {
                    result = "00";
                }
   
                client.Close();
                serverstatus = "ֹͣ";
            }
            catch (Exception e)
            {
                ErrMsg = "�����쳣��" + e.Message + "\r\n" + ErrMsg;
            }
        }

        public string connectServer(string hl7server, string hl7port, ref string resultMsg, string debug)
        {
              if (serverstatus != "��ʼ")
            {
                try
                {
                    string server = hl7server;
                    try
                    {
                        port = Convert.ToInt32(hl7port);
                    }
                    catch
                    {
                        port = 8898;
                    }
                    client = new TcpClient();
                    if (server == "")
                    {
                        resultMsg = "��������ַδ����!";
                        return "-1";
                    }
                    client.Connect(server, port);
                    if (debug == "1")
                    log.WriteMyLog("Connect to server " + server + ":" + port.ToString());
                      serverstatus = "��ʼ";
                    return "0";
                }
                catch(Exception EX)
                {
                    resultMsg = "���ӷ�����ʧ��,HL7�������ķ���δ����\r\n" + EX.Message;
                    return "-1";
                }
            }
              else
              {
                  return "1";
              }
        }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="localport"></param>
        public void startlisteneing(string localport)
        {
            port = Convert.ToInt32(localport);
            if (localstatus != "��ʼ")
            {
                try
                {
                    xxx = new Thread(new ThreadStart(listeneing));
                    xxx.Start();
                    
                }
                catch 
                {

                }
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void listeneing()
        {
            yy = true;
            xx = true;
            
            //port = f.ReadInteger("HL7", "localport", 8000);
            //port = Convert.ToInt16(localport);

            listener = new TcpListener(port);
            if (localstatus=="��ʼ") return;

            listener.Start();


            log.WriteMyLog("Client listening by port:" + port.ToString());
            localstatus = "��ʼ";
            //StatusLabel1.Text = "��ʼ����";
            while (xx)
            {
                try
                {

                    sock = listener.AcceptSocket();
                    clientsocket = sock;
                    thread = new Thread(new ThreadStart(recieve));
                    thread.Start();
                }
                catch
                {
                }
                if (yy == false)
                {
                    xx = false;

                    listener.Stop();
                    xxx.Abort();
                    sock.Close();
                    try
                    {
                        thread.Abort();
                    }
                    catch { }
                }

            }

        }
        /// <summary>
        /// ��������
        /// </summary>
        private void recieve()
        {
            bool control = false;
            while (!control)
            {
                if (yy == false)
                {
                    xx = false;

                    listener.Stop();
                    xxx.Abort();
                    sock.Close();
                    control = true;
                    try
                    {
                        thread.Abort();
                    }
                    catch { }
                }
                try
                {
                   
                    byte[] by = new byte[f.ReadInteger("HL7", "receivebyte", 4096)];
                    sock.Receive(by);

                    if (by[0] == 11)
                    {
                        string ss3 = System.Text.Encoding.UTF8.GetString(by, 1, f.ReadInteger("HL7", "receivebyte", 4096)-1);
                        //MessageBox.Show(ss3);
                        log.WriteMyLog("receive from server " + f.ReadString("HL7", "Server", "") + " message:" + System.Text.Encoding.UTF8.GetString(by, 0, by.Length).Replace("\0", ""));
                        /*�ϵĽ������
                        int x = ss3.IndexOf("\r\n");
                        string l1 = ss3.Substring(0, x);
                        string[] str1 = new string[20];
                        int ii1=0;
                        while (ii1 != 999)
                        {
                            str1[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                str1[ii1+1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                        */


                        //����(�ֽ�����Ϣ)
                        int messageindex = ss3.IndexOf("MSH");
                        string ss4 = ss3;
                        int ii1 = 0;
                        byte[] ack = new byte[4096];
                        while (ii1 != 999)
                        {
                            if (messageindex < 0)
                            {

                                hl7jm.messagemain(ss3,f.ReadString("HL7","Server",""),f.ReadString("HL7","Port",""),ref ack);
                                sock.Send(ack);
                                ii1 = 998;
                            }
                            else
                            {
                                ss4 = ss3.Substring(0, messageindex);

                                hl7jm.messagemain(ss4, f.ReadString("HL7", "Server", ""), f.ReadString("HL7", "Port", ""),ref ack);
                                sock.Send(ack);
                                ss3 = ss3.Substring(messageindex + 1);
                                messageindex = ss3.IndexOf("MSH");
                                
                            }
                            
                            ii1++;
                        }

                        //string[] aaa = adtread.Adt01(ss3);

                    }



                    // control = true;


                }
                catch (Exception e)
                {
                    if (e.Message.IndexOf("���������е����������һ���ѽ���������") > 0)
                    { }
                    else
                    {
                        log.WriteMyLog(e.Message);
                    }
                }
                //  listener.Stop();
                //    StatusLabel1.Text="���ӹر�";
            }
        }

    }
}
