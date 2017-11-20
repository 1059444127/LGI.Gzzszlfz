using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PathnetCAzgq
{
    class readhl7
    {
        public string[] Adt01(string msg,ref System.Data.DataTable brxx)
        {
            try
            {
                dtbrxx.Columns.Add("eip");
                dtbrxx.Columns.Add("name");
                dtbrxx.Columns.Add("sex");
                dtbrxx.Columns.Add("id");
                dtbrxx.Columns.Add("csrq");
                dtbrxx.Columns.Add("address");
                dtbrxx.Columns.Add("phone");

                dtjcxx.Columns.Add("影像号");
                dtjcxx.Columns.Add("检查项目");
                dtjcxx.Columns.Add("检查日期");

            }
            catch
            { }
            dtbrxx.Rows.Clear();
            dtjcxx.Rows.Clear();
            msg = msg.Replace("\0", "");
            msg = msg.Substring(1);
            msg = msg.Substring(0,msg.Length-2);
            msg = msg.Replace("\r", "\r\n");
            msg = msg.Replace("\r\n\n", "\r\n");
           // msg = msg.Replace("\r\n\r\n", "\r\n");
            for (int i = 0; i < MSH.Length; i++)
            {
                MSH[i] = "";
            }
            for (int i = 0; i < EVN.Length; i++)
            {
                EVN[i] = "";
            }
            for (int i = 0; i < PID.Length; i++)
            {
                PID[i] = "";
                
            }
            for (int i = 0; i < PV1.Length; i++)
            {
                PV1[i] = "";
            }
            for (int i = 0; i < ORC.Length; i++)
            {
                ORC[i] = "";
            }
            for (int i = 0; i < OBR.Length; i++)
            {
                OBR[i] = "";
            }
            for (int i = 0; i < NTE.Length; i++)
            {
                NTE[i] = "";
            }
            for (int i = 0; i < NTEBASE.Length; i++)
            {
                NTEBASE[i] = "";
            }
            for (int i = 0; i < MSA.Length; i++)
            {
                MSA[i] = "";
            }
            for (int i = 0; i < PR1.Length; i++)
            {
                PR1[i] = "";
                //dtjcxx.Columns.Add("X" + i.ToString() + "X");
            }
            
            adtall(msg,ref brxx);
            brxx = dtbrxx;

            return MSH;
        }
        public System.Data.DataTable dtbrxx = new System.Data.DataTable();
        
        public System.Data.DataTable dtjcxx = new System.Data.DataTable();
        public string[] strbrxx = new string[7];
        public string[] strjcxx = new string[3];
        public string[] PR1= new string[21];
        public string[] MSH = new string[21];
        public string[] EVN = new string[7];
        public string[] PID = new string[31];
        public string[] PV1 = new string[53];
        public string[] ORC = new string[21];
        public string[] OBR = new string[48];
        public string[] NTE = new string[5];
        public string[] NTEBASE = new string[5];
        public string[] MSA = new string[7];
        public string[] ERR = new string[7];
        public string[] QRD = new string[14];
        public string[] QRF = new string[14];


        
        
        
        public void adtall(string msg,ref System.Data.DataTable brxx)
        {
            string l1 = "";
            int x = 0;
            int jj1 = 0;
            int ii1 = 0;
            string[] str1 = new string[20];
            while (jj1 != 999)
            {
                x = msg.IndexOf("\r\n");
                if (x >= 0)
                {
                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "MSH")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            MSH[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                MSH[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }
                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "EVN")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            EVN[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                EVN[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "PID")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            PID[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                PID[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                        string[] brbz =new string[PID[3].Split('~').Length];
                        for (int bi = 0; bi < PID[3].Split('~').Length; bi++)
                        {
                            if (PID[3].Split('~')[bi].Split('^')[5]=="EIP")
                            {
                                strbrxx[0] = PID[3].Split('~')[bi].Split('^')[0];
                            }
                        }
                        //strbrxx[0] = PID[3];
                        strbrxx[1] = PID[5].Split('^')[0];
                        
                        strbrxx[2] = PID[8];

                        if (PID[8] == "M") strbrxx[2] = "男";
                        if (PID[8] == "F") strbrxx[2] = "女";
                        if (PID[8] == "O") strbrxx[2] = "其他";
                        if (PID[8] == "U") strbrxx[2] = "未知";

                        strbrxx[3] = PID[19];
                        strbrxx[4] = PID[7];
                        strbrxx[5] = PID[11];
                        try
                        {
                            strbrxx[6] = PID[13].Split('^')[8];
                        }
                        catch
                        {
 
                        }
                        dtbrxx.Rows.Add(strbrxx);
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "PR1")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            PR1[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                PR1[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                        string[] brbz = new string[PID[3].Split('~').Length];
                        for (int bi = 0; bi < PID[3].Split('~').Length; bi++)
                        {
                            if (PID[3].Split('~')[bi].Split('^')[5] == "EIP")
                            {
                                
                               strjcxx[0] = PID[3].Split('~')[bi].Split('^')[0];
                            }
                        }
                        //strbrxx[0] = PID[3];
                       
                            strjcxx[1] = PR1[4];
                            strjcxx[2] = PR1[5];

                       
                        dtjcxx.Rows.Add(strjcxx);
                        
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "MSA")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            MSA[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                MSA[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "ERR")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            ERR[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                ERR[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "QRD")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            QRD[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                QRD[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "QRF")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            QRF[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                QRF[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }

                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "PV1")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {
                            PV1[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                PV1[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }
                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "OBR")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {

                            OBR[ii1] = l1.Substring(0, l1.IndexOf("|"));

                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {

                                OBR[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }
                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "ORC")
                    {
                        ii1 = 0;
                        while (ii1 != 999)
                        {

                            ORC[ii1] = l1.Substring(0, l1.IndexOf("|"));

                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {

                                ORC[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                    }
                    l1 = msg.Substring(0, x);
                    if (l1.Substring(0, l1.IndexOf("|")) == "NTE")
                    {
                        NTEBASE[0] = "";
                        NTEBASE[1] = "";
                        NTEBASE[2] = "";
                        NTEBASE[3] = "";
                        NTEBASE[4] = "";

                        ii1 = 0;
                        while (ii1 != 999)
                        {

                            NTEBASE[ii1] = l1.Substring(0, l1.IndexOf("|"));
                            l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                            if (l1.IndexOf("|") < 0)
                            {
                                NTEBASE[ii1 + 1] = l1;
                                ii1 = 998;
                            }
                            ii1++;
                        }
                        if (NTEBASE[1] == "1")
                        {
                            NTE[0] = NTEBASE[3];
                        }
                        if (NTEBASE[1] == "2")
                        {
                            NTE[1] = NTEBASE[3];
                        }
                        if (NTEBASE[1] == "3")
                        {
                            NTE[2] = NTEBASE[3];
                        }
                        if (NTEBASE[1] == "4")
                        {
                            NTE[3] = NTEBASE[3];
                        }
                        if (NTEBASE[1] == "5")
                        {
                            NTE[4] = NTEBASE[3];
                        }
                    }
                    msg = msg.Substring(x + 2);
                }
                else
                {
                    jj1 = 998;
                }
                jj1++;
            }
        }
    }
}
