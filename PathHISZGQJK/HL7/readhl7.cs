using System;
using System.Collections.Generic;
using System.Text;

namespace HL7
{
    class readhl7
    {
        public  string[] Adt01(string msg)
        {
            msg = msg.Replace("\0", "");
            msg = msg.Replace("\r", "\r\n");
            msg = msg.Replace("\r\n\n", "\r\n");
            msg = msg.Replace("\r\n\r\n", "\r\n");
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
            for (int i = 0; i < DG1.Length; i++)
            {
                DG1[i] = "";
            }

            adtall(msg);            
            return MSH;
        }
        public string[] MSH = new string[30];
        public string[] EVN = new string[30];
        public string[] PID = new string[60];
        public string[] PV1 = new string[70];
        public string[] ORC = new string[50];
        public string[] OBR = new string[60];
        public string[] NTE = new string[20];
        public string[] NTEBASE = new string[25];
        public string[] MSA = new string[17];
        public string[] DG1 = new string[40];
        public void adtall(string msg)
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
                    if (l1.IndexOf("|") > 0)
                    {
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
                        if (l1.Substring(0, l1.IndexOf("|")) == "DG1")
                        {
                            ii1 = 0;
                            if (l1.Split('|')[1].Trim() != "")
                                ii1 = 999;
                            while (ii1 != 999)
                            {

                                DG1[ii1] = l1.Substring(0, l1.IndexOf("|"));

                                l1 = l1.Substring(l1.IndexOf("|") + 1, l1.Length - l1.IndexOf("|") - 1);
                                if (l1.IndexOf("|") < 0)
                                {

                                    DG1[ii1 + 1] = l1;
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
