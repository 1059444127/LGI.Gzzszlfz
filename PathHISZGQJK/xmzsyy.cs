using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dbbase;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.OracleClient;
using ZgqClassPub;
using ZgqClassPub.DBData;

namespace PathHISZGQJK
{
    /// <summary>
    /// ������ɽҽԺ
    /// </summary>
    class xmzsyy
    {
        private IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");

        public void pathtohis(string blh, string bglx, string bgxh, string msg1, string debug, string[] cslb)
        {

              string qxsh = "";
            string xdj = "";
            bglx = bglx.ToLower();

            string jkmsg = f.ReadString("jkmsg", "jkmsg", "0").Replace("\0", "");

            
            f.WriteInteger("jkmsg", "jkmsg", 0);

            if (cslb.Length == 5)
            {
                if (cslb[4].ToLower() == "qxsh")
                {
                    //ȡ����˶���
                    qxsh = "1";
                }

                if (cslb[3].ToLower() == "new")
                {
                    xdj = "1";
                }

            }
    
            if (bglx == "")
                bglx = "cg";

            if (bgxh == "")
                bgxh = "0";
            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable jcxx = new DataTable();
            try
            {
                jcxx = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }
            if (jcxx == null)
            {
                MessageBox.Show("�������ݿ����������⣡");
                return;
            }
            if (jcxx.Rows.Count < 1)
            {
                MessageBox.Show("������д���");
                return;
            }

            if (bglx.Trim() == "")
            {
                log.WriteMyLog("��������Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);
                return;
            }
            string bgzt = "";

            if (qxsh == "1")
            {
                bgzt = "ȡ�����";
            }
            else
                bgzt = jcxx.Rows[0]["F_BGZT"].ToString().Trim();
           
            int plsc = f.ReadInteger("fsjk", "plsc", 0);
            string msg = f.ReadString("savetohis", "msg","1");
            if (plsc != 1)
            {
              

                ////��������д�ӿ�/////zgq
                if (jcxx.Rows[0]["F_BRLB"].ToString().Trim() == "���")
                {
                 
                    # region  ����д�ӿ�
                    if (jcxx.Rows[0]["F_BRBH"].ToString().Trim() == "")
                    {
                        ZgqClass.BGHJ(blh, "���ӿ�", "������", "��첡���޲��˱�ţ�������", "ZGQJK", "���ӿ�");
                        aa.ExecuteSQL("update T_JCXX_FS set F_BZ='��첡���޲��˱�ţ�������'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");

                        log.WriteMyLog(blh + ",��첡���޲��˱�ţ�������");
                        if (jkmsg == "1")
                            MessageBox.Show("����첡���޲��˱�ţ�������");
                        return;
                    }
        
                      string err_msg = "";
                        string constr = f.ReadString("savetohis", "tj_odbcsql", "Provider='MSDAORA';data source=ZSYYTJ;User ID=SD_PE_LIS;Password=SD_PE_LIS;");
                    OleDbDB db = new OleDbDB();


                        DataTable TJ_bljc = new DataTable();
                        TJ_bljc = aa.GetDataTable(" select top 1  *  from T_TBS_BG where  F_blh='" + blh + "'", "blxx");

                        if (debug == "1")
                            log.WriteMyLog("ִ�����");

                       //���ȷ��
                        if (jcxx.Rows[0]["F_TJBJ"].ToString().Trim() != "1" )
                        {
                            try
                            {
                             
                               OleDbParameter[] oledbPt = new OleDbParameter[4];

                               for (int j = 0; j < oledbPt.Length; j++)
                               {
                                   oledbPt[j] = new OleDbParameter();
                               }
                               oledbPt[0].ParameterName = "para_sfywid";
                               oledbPt[0].OleDbType = OleDbType.Decimal;
                               oledbPt[0].Direction = ParameterDirection.Input;
                               oledbPt[0].Size = 10;
                               oledbPt[0].Value = Decimal.Parse(jcxx.Rows[0]["F_BRBH"].ToString().Trim());

                               oledbPt[1].ParameterName = "para_flag";//
                               oledbPt[1].OleDbType = OleDbType.Char;
                               oledbPt[1].Direction = ParameterDirection.Input;
                               oledbPt[1].Size = 1;
                               oledbPt[1].Value = '1';

                               oledbPt[2].ParameterName = "para_result";//
                               oledbPt[2].OleDbType = OleDbType.Char;
                               oledbPt[2].Direction = ParameterDirection.Output;
                               oledbPt[2].Size = 1;

                               oledbPt[3].ParameterName = "para_msg";//
                               oledbPt[3].OleDbType = OleDbType.VarChar;
                               oledbPt[3].Direction = ParameterDirection.Output;
                               oledbPt[3].Size = 200;

                               if (debug == "1")
                                   log.WriteMyLog("ִ�б��:" + oledbPt[0].Value.ToString() + "@" + oledbPt[1].Value.ToString() + "@" + err_msg);

                               //ɾ��
                               db.ExecuteNonQuery(constr, "updateBL", ref oledbPt, CommandType.StoredProcedure, ref err_msg);

                                if (debug == "1")
                                    log.WriteMyLog("���ִ�б�Ƿ��أ�" + oledbPt[2].Value.ToString() + "@" + oledbPt[3].Value.ToString() + "@" + err_msg);

                                if (oledbPt[2].Value.ToString() != "Y")
                                    log.WriteMyLog("���ȷ��ʧ�ܣ�" + oledbPt[2].Value.ToString() + "@" + oledbPt[3].Value.ToString() + "\r\n" + err_msg);
                                else
                                {
                                    aa.ExecuteSQL("update T_JCXX  set F_TJBJ='1' where F_BLH='" + blh + "'");
                                }
                            }
                            catch(Exception  e1)
                            {
                                log.WriteMyLog(e1.Message);
                            }
                            //                    ȷ�Ϲ��̣�updateBL
                            //--para_sfywid �շ�ҵ��ID
                            //--para_flag   0��ȡ�� 1��ȷ��
                            //--para_result  N:ʧ�� Y:�ɹ�
                            //procedure updateBL(para_sfywid in  number, para_flag   in  char�� para_result out char);

                            
                        }
                        if (bgzt.Trim() == "�����")
                        {
 
                            string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                          
                            string jpgname = "";
                            string jpgpath = "";
            

                            //����jpg
                            bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.JPG, ref err_msg, ref jpgname, ref jpgpath);
                           
                            if (isrtn)
                            {
                                jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                               ZgqClass.BGHJ(blh, "���ӿ�", "������", "����jpg�ɹ�:" + jpgpath + "\\" + jpgname, "ZGQJK", "�������jpg");
                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                            }
                            else
                            {
                                log.WriteMyLog(blh + "-" + err_msg);
                                ZgqClass.BGHJ(blh, "���ӿ�", "������", "����JPG��ʽͼƬʧ��" + err_msg, "ZGQJK", "���ӿ�");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='����JPG��ʽͼƬʧ�ܣ�" + err_msg + "'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                if (jkmsg == "1" || msg == "1")
                                    MessageBox.Show("����ţ�" + blh + "  ����JPG��ʽͼƬʧ�ܣ���������ˣ�\r\n" + err_msg);

                                return;
                            }

                            # region �ش���챨��



                            // �������
                            string Res_char = jcxx.Rows[0]["F_jxsj"].ToString().Trim();
                            //��Ͻ���	Res_con
                            string Res_con = jcxx.Rows[0]["F_blzd"].ToString().Trim();

                            if (TJ_bljc.Rows.Count > 0)
                            {
                                if (jcxx.Rows[0]["F_blk"].ToString().Trim() == "TCT")
                                {
                                    Res_char = Res_char + "�걾����ȣ�" + TJ_bljc.Rows[0]["F_TBS_BBMYD"].ToString().Trim() + "\r\n";

                                    Res_char = Res_char + "ϸ������" + TJ_bljc.Rows[0]["F_TBS_XBL"].ToString().Trim() + "\r\nϸ���ɷ֣�" + TJ_bljc.Rows[0]["F_TBS_XBXM2"].ToString().Trim()
                                        + "\r\n��ϸ����" + TJ_bljc.Rows[0]["F_TBS_YZCD"].ToString().Trim() + "\r\n";

                                    Res_char = Res_char + "΢���" + TJ_bljc.Rows[0]["F_TBS_WSW2"].ToString().Trim() + "\r\n";

                                    ///////////���/////////////////////////
                                    Res_con = "��ϣ�" + TJ_bljc.Rows[0]["F_TBSZD"].ToString().Trim() + "\r\n" + "\r\n";
                                    if (TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() != "")
                                        Res_con = Res_con + "���������" + TJ_bljc.Rows[0]["F_TBS_BCYJ1"].ToString().Trim() + "\r\n";
                                }
                            }

                            /////////////////////////////////////////////////////
                            string path = f.ReadString("savetohis", "jpgpath", @"\\192.10.33.84\pdfbg\");
                            jpgpath = path + jpgpath + "\\" + jpgname;
                            string sql_insert = "insert into BL_TJ_TJJGB0(SFYWID,TJH000,SQXMID,TZMS00,KSZD00,YXLJ00,JYYS00,SHYS00,JYRQ00,JYSJ00,SHRQ00,SHSJ00,JCXMLX,SFYX00)"
                            + " values(" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + "," + jcxx.Rows[0]["F_mzh"].ToString().Trim() + "," + jcxx.Rows[0]["F_YZXM"].ToString().Trim().Split('^')[0] + ",'"
                            + Res_char + "','" + Res_con + "','" + jpgpath + "','" + jcxx.Rows[0]["F_BGYS"].ToString().Trim() + "','" + jcxx.Rows[0]["F_shys"].ToString().Trim() + "','"
                            + DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("yyyyMMdd") + "','" + DateTime.Parse(jcxx.Rows[0]["F_BGRQ"].ToString().Trim()).ToString("HH:mm:ss")
                            + "','" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("yyyyMMdd") + "','" + DateTime.Parse(jcxx.Rows[0]["F_SPARE5"].ToString().Trim()).ToString("HH:mm:ss")
                            + "','BL','0')";

                           
                            string sql_del = "delete from BL_TJ_TJJGB0  where SFYWID=" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + " and TJH000=" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + " and SQXMID="
                                  + jcxx.Rows[0]["F_YZXM"].ToString().Trim().Split('^')[0] + " and JCXMLX='BL'";
                            
                            //ɾ�� 
                            if (debug == "1")
                                log.WriteMyLog("ɾ��������䣺"+sql_del);

                          int x=  db.ExecuteNonQuery(constr, sql_del, ref err_msg);
                            if (debug == "1")
                                log.WriteMyLog("ɾ������" + err_msg + "@" + x.ToString());
              

                             if (debug == "1")
                                log.WriteMyLog("��д������䣺" + sql_insert );
                            //����
                             x = db.ExecuteNonQuery(constr, sql_insert, ref err_msg);

                            if (debug == "1")
                                 log.WriteMyLog("��д����" + err_msg+"@"+x.ToString());
                           
                            if (x < 0)
                            {
                                ZgqClass.BGHJ(blh, "���ӿ�", "������", "��д��챨��ʧ�ܣ�" + err_msg, "ZGQJK", "���ӿ�");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='��д��챨��ʧ�ܣ�" + err_msg + "'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                log.WriteMyLog(blh + "-" + err_msg);
                                if (jkmsg == "1" || msg == "1")
                                    MessageBox.Show("����ţ�" + blh + "-��д��챨��ʧ�ܣ���������ˣ�\r\n" + err_msg);

                            }
                            else
                            {
                                ZgqClass.BGHJ(blh, "���ӿ�", "������", "��д��챨��ɹ�", "ZGQJK", "���ӿ�");
                                aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                aa.ExecuteSQL("update T_JCXX  set F_TJBJ='2' where F_BLH='" + blh + "'");
                                if (jkmsg == "1")
                                    MessageBox.Show("����ţ�" + blh + "-��д��챨��ɹ�");
                            }

                            return;
                            #endregion
                        }
                        else
                        {
                            if (bgzt == "ȡ�����")
                            {

                                //ɾ���м��
                                string str_sql = "delete from BL_TJ_TJJGB0  where SFYWID=" + jcxx.Rows[0]["F_BRBH"].ToString().Trim() + " and TJH000=" + jcxx.Rows[0]["F_mzh"].ToString().Trim() + " and SQXMID="
                                    + jcxx.Rows[0]["F_YZXM"].ToString().Trim().Split('^')[0] + " and JCXMLX='BL'";

                                if (debug == "1")
                                {
                                    log.WriteMyLog("��д������䣺" + str_sql);
                                }

                                int x = db.ExecuteNonQuery(constr, str_sql, ref err_msg);
                                if (x < 0)
                                {
                                    ZgqClass.BGHJ(blh, "���ӿ�", "���ȡ�����", "ȡ����챨��ʧ�ܣ�" + err_msg, "ZGQJK", "���ӿ�");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='ȡ����챨��ʧ�ܣ�" + err_msg + "'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                    log.WriteMyLog(blh + "-" + err_msg);
                                    if (msg == "1")
                                        MessageBox.Show("����ţ�" + blh + "-ȡ����챨��ʧ�ܣ�\r\n");
                                }
                                else
                                {
                                    ZgqClass.BGHJ(blh, "���ӿ�", "���ȡ�����", "ȡ����챨��ɹ�", "ZGQJK", "���ӿ�");
                                    aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='cg' and F_BGXH='0'");
                                    aa.ExecuteSQL("update T_JCXX  set F_TJBJ='1' where F_BLH='" + blh + "'");
                                }

                                //ɾ��T_BG_PDF��¼
                                DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                                if (dt2.Rows.Count > 0)
                                {
                                    aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                                    //ɾ��ftp��pdf�ļ�
                                    ZgqPDFJPG zgq = new ZgqPDFJPG();
                                    string rtn_msg = "";
                                    zgq.DelPDFFile(dt2.Rows[0]["F_ML"].ToString(), dt2.Rows[0]["F_FILENAME"].ToString(), ref rtn_msg);
                                 
                                }
                                else
                                {
                                    log.WriteMyLog(blh + ",T_BG_PDF��δ���ҵ���¼");
                                }
                                return;
                                  
                            }
                            return;

                        }
                        return;


                    # endregion
                } 
            else
                {
                    #region  ����pdf


                    //����첡�˻�д
               
                    string bgzt2 = "";
                    try
                    {
                        if (bglx.ToLower().Trim() == "bd")
                        {
                            DataTable dt_bd = aa.GetDataTable("select * from T_BDBG where F_BLH='" + blh + "' and  F_BD_BGXH='" + bgxh + "'", "bd");
                            bgzt2 = dt_bd.Rows[0]["F_BD_BGZT"].ToString();
                        }
                        if (bglx.ToLower().Trim() == "bc")
                        {
                            DataTable dt_bc = aa.GetDataTable("select * from T_BCBG where F_BLH='" + blh + "' and  F_BC_BGXH='" + bgxh + "'", "bc");
                            bgzt2 = dt_bc.Rows[0]["F_BC_BGZT"].ToString();

                        }
                        if (bglx.ToLower().Trim() == "cg")
                        {
                            // DataTable jcxx2 = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "jcxx");
                            bgzt2 = jcxx.Rows[0]["F_BGZT"].ToString();
                        }
                    }
                    catch(Exception  e5)
                    {
                        log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh+e5.Message);
                    }

                    if (bgzt2.Trim() == "")
                    {
                        log.WriteMyLog("����״̬Ϊ�գ�������" + blh + "^" + bglx + "^" + bgxh);
                    }

                    if (bgzt2.Trim() == "�����" && bgzt != "ȡ�����")
                    {

                        string ML = DateTime.Parse(jcxx.Rows[0]["F_SDRQ"].ToString()).ToString("yyyyMM");
                        string err_msg = "";
                        string jpgname = "";
                        string jpgpath = "";
                      

                        //����jpg
                        bool isrtn = MD_PDF_JPG(blh, bglx, bgxh, ML, ZgqPDFJPG.Type.PDF, ref err_msg, ref jpgname, ref jpgpath);
                        if (isrtn)
                        {

                            jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                            ZgqClass.BGHJ(blh, "his�ӿ�", "�������", "����pdf�ɹ�:" + ML + "\\" + jpgname, "ZGQJK", "pdf");
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='"+bglx+"' and F_BGXH='"+bgxh+"'");
                            aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            aa.ExecuteSQL("insert  into T_BG_PDF(F_BLH,F_BGLX,F_BGXH,F_ML,F_FILENAME) values('" + blh + "','" + bglx + "','" + bgxh + "','" + jpgpath + "','" + jpgname + "')");

                        }
                        else
                        {
                            log.WriteMyLog(blh + "-" + err_msg);
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='����pdfʧ��" + err_msg+"'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                            ZgqClass.BGHJ(blh, "his�ӿ�", "���PDF", "����pdfʧ��" + err_msg, "ZGQJK", "pdf");
                            if (msg == "1" || jkmsg == "1")
                                MessageBox.Show("����ţ�" + blh + "  ����pdfʧ�ܣ���������ˣ�\r\n" + err_msg);
                            return;
                        }
                    }
                    else
                    {
                        if (bgzt == "ȡ�����")
                        {
                            DataTable dt2 = aa.GetDataTable("select top 1 * from T_BG_PDF where F_blh='" + blh + "'  and F_BGXH='" + bgxh + "' and F_BGLX='" + bglx + "'", "dt2");
                            if (dt2.Rows.Count > 0)
                            {

                                aa.ExecuteSQL("delete T_BG_PDF  where F_BLH='" + blh + "' and  F_BGLX='" + bglx + "'  and F_BGXH='" + bgxh + "'");
                            }
                            aa.ExecuteSQL("update T_JCXX_FS set F_BZ='',F_FSZT='�Ѵ���'  where F_blh='" + blh + "' and F_BGLX='" + bglx + "' and F_BGXH='" + bgxh + "'");
                        }
                    }

                    return;

                    # endregion
                }

            }
            else
            {
                //�����ϴ�

            }
        }

/// <summary>
 ///  ����pdf��jpg
/// </summary>
/// <param name="blh"></param>
/// <param name="bglx"></param>
/// <param name="bgxh"></param>
/// <param name="ML"></param>
/// <param name="jpgpdf"></param>
/// <param name="err_msg"></param>
/// <param name="fileName"></param>
/// <param name="fielPath"></param>
/// <returns></returns>
        public bool MD_PDF_JPG(string blh, string bglx, string bgxh, string ML, ZgqPDFJPG.Type jpgpdf, ref string err_msg, ref string fileName, ref string fielPath)
        {

             
                string message = ""; string jpgname = "";
                ZgqPDFJPG zgq = new ZgqPDFJPG();
                bool isrtn = zgq.CreatePDF(blh, bglx, bgxh, jpgpdf, ref message, ref jpgname);
              
                string xy = f.ReadString("ZGQJK", "sctxfs", "3");
                if (isrtn)
                {
                    bool ssa = zgq.UpPDF(blh, jpgname, ML, ref message, int.Parse(xy));
                    if (ssa == true)
                    {
                        //jpgname = jpgname.Substring(jpgname.LastIndexOf('\\') + 1);
                        fileName=jpgname;
                        fielPath=ML+"\\"+blh;
                        err_msg = "";
                        zgq.DelTempFile(blh);
                        return true;

                     }
                    else
                    {
                        err_msg = message;
                        zgq.DelTempFile(blh);
                        return false;
                    }
                }
                else
                {
                    zgq.DelTempFile(blh);
                    err_msg = message;
                    return false;
                }
        }
    }
}
