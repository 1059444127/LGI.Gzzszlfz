using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.Data.SqlClient;
namespace LGHISJKZGQ
{
    public partial class frm_sfjk : Form
    {

        string  Identitys =string.Empty;
        public frm_sfjk(string hm,string type1)
        {
            //����x ��1������ţ�0���������
            InitializeComponent();
            type = type1;
            if (type1=="0")
                sqxh = hm;
            if (type1=="1")
                F_blh = hm;

         }
        string sfgh = "";//�Ʒѹ���
        string sfry = "";//�Ʒ���Ա
        private string sqxh = "";//�������
        private string type = "";
        private string F_blh = "";//�����
        private string  InOrOut= "";//�������
       // string constr = "Provider='MSDAORA';data source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.2.217)(PORT = 1521)))(CONNECT_DATA =(SID = hxyy1)));user id =DHC;password=DHC";
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        DataTable dt_jcxx;//������Ϣ��-�������ݿ�
        DataTable dt_sqd; // ���뵥��Ϣ��-�������뵥��Ϣ��
        DataTable dt_sfm; // �շ���Ϣ��
        DataSet ds = new DataSet(); 
        string EXAM_NO = "";  //������ϸ�ģɣ�
        string msg = "0";     //��ʾ��Ϣ��
        public decimal sfje = 0; //�շѽ��
        string T_current_price_list_sql = ""; //�Զ���� �շ���Ŀ��current_price_list�Ĳ�ѯ����
        string T_exam_bill_items_sql = "";//�Զ���� �շ���ϸ��exam_bill_items�Ĳ�ѯ����
        LGHISJKZGQ.xyfyyyweb.DHCPisXiangYaOne xyfy = new LGHISJKZGQ.xyfyyyweb.DHCPisXiangYaOne();


        string Server = f.ReadString("sqlserverzgq", "Server", "");
        string DataBase = f.ReadString("sqlserverzgq", "DataBase", "");
        string UserID = f.ReadString("sqlserverzgq", "UserID", "");
        string PassWord = f.ReadString("sqlserverzgq", "PassWord", "");

        string orcon_str = "Provider='MSDAORA'; data source=DBSERVER;User ID=DHC;Password=DHC;";
        string odbcsql = f.ReadString("SF", "odbcsql", "");
        private void frm_sfjk_Load(object sender, EventArgs e)
        {
            // û�й��Ų�����
         
            sfgh = f.ReadString("yh", "yhbh", "").Replace("\0", "");
            sfry = f.ReadString("yh", "yhmc", "").Replace("\0", "");
            if (sfgh == "")
            {
                MessageBox.Show("���շ�Ա�޹��ţ����ܲ����շѣ�");
                this.Close();
                
            }
            //------����WEBservices----------------
            string pathweburl = f.ReadString("SF", "webservicesurl", "");
            if (pathweburl.Trim() != "")
                xyfy.Url = pathweburl;
            //--------����-oracl�����ַ���constr-------------------------------

           // string oracl_con = f.ReadString("SF", "odbcsql", "");
            //if (oracl_con.Trim() != "")
            //    constr = oracl_con;
           
            //---------�Ƿ�����Ϣ��ʾ��������--------------------------------------------------------
            msg = f.ReadString("���뵥", "msg", "");
            //�շ���Ŀ��ʾ����
            T_current_price_list_sql = f.ReadString("SF", "V_current_price_list", "");

           // �շ���ϸ��ʾ����
            T_exam_bill_items_sql = f.ReadString("SF", "V_exam_bill_items", "");
            //---------type=1Ϊ����ڵ��ã�������ţ�hm��---����������ţ�����δ�ǼǼƷ�--------------------------------------------------------
            if (type == "1")
            {
                if (F_blh.Trim() == "")
                {
                    MessageBox.Show("����Ų���Ϊ��");
                    this.Close();
                }
                    dt_jcxx = aa.GetDataTable("select * from T_JCXX where  F_BLH='" + F_blh + "'", "jcxx");

                    if (dt_jcxx.Rows.Count < 1)
                    {
                        MessageBox.Show("û�д˲������Ϣ��");
                        this.Close();
                    }

                    if (dt_jcxx.Rows[0]["F_sqxh"].ToString().Trim()=="")
                    {
                        MessageBox.Show("��������ţ�������");
                      
                        this.Close();
                      
                    }

                    InOrOut = dt_jcxx.Rows[0]["F_brlb"].ToString();
                    sqxh = dt_jcxx.Rows[0]["F_SQXH"].ToString();
                    label8.Text = "��ǰ����:" + F_blh + "  " + dt_jcxx.Rows[0]["F_xm"].ToString() + " " + dt_jcxx.Rows[0]["F_nl"].ToString() + " �����" + dt_jcxx.Rows[0]["F_mzh"].ToString() + " סԺ��" + dt_jcxx.Rows[0]["F_zyh"].ToString();
             }
            else
            {
                label8.Text = "��ǰ����: ����ţ�" + sqxh;
                if (sqxh.Trim()== "")
                {
                    MessageBox.Show("��������ţ�������");
                    this.Close();
                }

            }
            if (sqxh.Trim() != "")
            {

                //��ȡ�շ���Ŀ��
                loadxmb();
                //--------�жϲ������뵥�����Ƿ���������¼---------------------

                dt_sqd = select_sql("select * from Examapply  where checkflow='" + sqxh + "'");
                if (dt_sqd.Rows.Count < 1)
                {

                    MessageBox.Show("�������뵥����û�и������뵥��Ϣ������");
                    this.Close();
                }
                else
                {

                    if (dt_sqd.Rows[0]["jfbj"].ToString() != "1" && dt_sqd.Rows[0]["InOrOut"].ToString() == "סԺ")
                    {
                        button2.Visible = true;
                        button2.Text = "סԺ����ȷ��";
                        button1.Visible = false;
                        //MessageBox.Show("�˲�������Ŀ��ûȷ�ѣ�����ȷ�Ѻ��ڲ���");
                    }
                    if (dt_sqd.Rows[0]["zdhj"].ToString() != "1" && dt_sqd.Rows[0]["InOrOut"].ToString() == "����")
                    {
                        button2.Visible = true;
                        button2.Text = "�����Զ�����";
                        button1.Visible = false;
                        // MessageBox.Show("�˲��˻�δ�Զ����ۣ������Զ�������ȥ����");
                    }
                    try
                    {
                        Identitys = dt_sqd.Rows[0]["Identitys"].ToString();
                    }
                    catch
                    {
                        Identitys = "";
                    }
                    //ͨ�����뵥�Ŷ�Ӧhis��emr.exam_appoints_id��� EXAM_NO���Ʒ���,�����סԺ��emr.exam_appoints_id��ͬ
                    InOrOut = dt_sqd.Rows[0]["InOrOut"].ToString();
                    try
                    {
                        string exam_appoints_id_str = "select *  from emr.exam_appoints_id where CHECK_FLOW='" + sqxh + "' ";
                        if (InOrOut == "����")
                            exam_appoints_id_str = "select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + sqxh + "' ";

                        DataTable exam_appoints_id = select_orcl(exam_appoints_id_str, "��ȡEXAM_NO��");

                        EXAM_NO = exam_appoints_id.Rows[0]["EXAM_NO"].ToString().Trim();
                    }
                    catch
                    {
                        MessageBox.Show("EXAM_NO��ȡʧ�ܣ�");
                    }
                    //��ȡ���շ��б�������Ϣ
                    loadsfmx();
                }
            }
            label14.Text = Identitys;
        }
          
        ///////�������ԣ����ڷ����ܽ��
        public decimal F_sfje
        {
            get { return sfje; }
            set { this.sfje = value; }
        }

        //////�շ���Ŀ��
        private void loadxmb()
        {
            DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,prefer_price,FOREIGNER_PRICE,KEYCOLNO from current_price_list  where 1=1 " + T_current_price_list_sql, "��ȡ�շ���Ŀ");
              
                DataView dv = dt_sfxm.DefaultView;
                dv.RowFilter = "  CLASS_ON_MR like '%���%' or CLASS_ON_MR like '%����%' ";
                DataTable dt_sfxm2 = dv.ToTable("xmb2");
              ds.Tables.Add(dt_sfxm2);
             
                dataGridView2.DataSource = dt_sfxm2;
                dataGridView2.Columns[0].HeaderText = "�շѴ���";
                dataGridView2.Columns[0].Width = 80;
                dataGridView2.Columns[1].HeaderText = "�շ���Ŀ";
                dataGridView2.Columns[1].Width = 120;
                dataGridView2.Columns[2].HeaderText = "���";
                dataGridView2.Columns[2].Width = 40;
                dataGridView2.Columns[3].HeaderText = "����";
                dataGridView2.Columns[3].Width = 60;
                dataGridView2.Columns[4].HeaderText = "˵��";
                dataGridView2.Columns[4].Width = 80;
                dataGridView2.Columns[5].HeaderText = "����";

        }

        //////�շ���Ŀ��ϸ
        private void loadsfmx()
        {
            dt_sfm =select_orcl("select * from exam.exam_bill_items  where  exam_no='" + EXAM_NO + "' " + T_exam_bill_items_sql + "  order by BILLING_DATE_TIME ", "��ȡ�շ���ϸ");

            if(dt_sfm.Rows.Count>0)
            {
              dataGridView1.DataSource = dt_sfm;
              dataGridView1.AutoGenerateColumns = false;

              dataGridView1.Columns[0].HeaderText = "ִ�б�־";
              dataGridView1.Columns[0].DataPropertyName = "STATUS";
              dataGridView1.Columns[0].Width = 20;
              dataGridView1.Columns[0].Name = "STATUS";

              dataGridView1.Columns[1].HeaderText = "�շѱ�־";
              dataGridView1.Columns[1].DataPropertyName = "BILLING_ATTR";
              dataGridView1.Columns[1].Width = 20;
              dataGridView1.Columns[1].Name = "BILLING_ATTR";

              dataGridView1.Columns[2].HeaderText = "��Ŀ����";
              dataGridView1.Columns[2].DataPropertyName = "ITEM_CODE";
              dataGridView1.Columns[2].Width = 80;
              dataGridView1.Columns[2].Name = "ITEM_CODE";

              dataGridView1.Columns[3].HeaderText = "��Ŀ����";
              dataGridView1.Columns[3].DataPropertyName = "ITEM_NAME";
              dataGridView1.Columns[3].Width = 150;
              dataGridView1.Columns[3].Name = "ITEM_NAME";

              dataGridView1.Columns[4].HeaderText = "��Ŀ���";
              dataGridView1.Columns[4].DataPropertyName = "ITEM_SPEC";
              dataGridView1.Columns[4].Width = 80;
              dataGridView1.Columns[4].Name = "ITEM_SPEC";

              dataGridView1.Columns[5].HeaderText = "����";
              dataGridView1.Columns[5].DataPropertyName = "AMOUNT";
              dataGridView1.Columns[5].Name = "AMOUNT";
              dataGridView1.Columns[5].Width = 40;


              dataGridView1.Columns[6].HeaderText = "��λ";
              dataGridView1.Columns[6].DataPropertyName = "UNITS";
              dataGridView1.Columns[6].Width = 40;
              dataGridView1.Columns[6].Name = "UNITS";  
            
              dataGridView1.Columns[7].HeaderText = "����";
              dataGridView1.Columns[7].DataPropertyName = "COSTS";
              dataGridView1.Columns[7].Width = 50;
              dataGridView1.Columns[7].Name = "COSTS";

              dataGridView1.Columns[8].HeaderText = "Ӧ�շ���";
              dataGridView1.Columns[8].DataPropertyName = "Charges";
              dataGridView1.Columns[8].Width = 60;
              dataGridView1.Columns[8].Name = "Charges";

              dataGridView1.Columns[9].HeaderText = "ִ�п���";
              dataGridView1.Columns[9].DataPropertyName = "PERFORMED_BY";
              dataGridView1.Columns[9].Width = 60;
              dataGridView1.Columns[9].Name = "PERFORMED_BY";

              dataGridView1.Columns[10].HeaderText = "��������";
              dataGridView1.Columns[10].DataPropertyName = "ORDERED_BY";
              dataGridView1.Columns[10].Width = 60;
              dataGridView1.Columns[10].Name = "ORDERED_BY";

              dataGridView1.Columns[11].HeaderText = "�Ƽ�Ա";
              dataGridView1.Columns[11].DataPropertyName = "OPERATOR_NO";
              dataGridView1.Columns[11].Width = 40;
              dataGridView1.Columns[11].Name = "OPERATOR_NO";

              dataGridView1.Columns[12].HeaderText = "�Ƽ�����";
              dataGridView1.Columns[12].DataPropertyName = "BILLING_DATE_TIME";
              dataGridView1.Columns[12].Width = 120;
              dataGridView1.Columns[12].Name = "BILLING_DATE_TIME";

              dataGridView1.Columns[13].HeaderText = "����ȷ�ϱ�־";
              dataGridView1.Columns[13].DataPropertyName = "VERIFIED_INDICATOR";
              dataGridView1.Columns[13].Width = 50;
              dataGridView1.Columns[13].Name = "VERIFIED_INDICATOR";


              dataGridView1.Columns[14].HeaderText = "EXAM_NO";
              dataGridView1.Columns[14].DataPropertyName = "EXAM_NO";
              dataGridView1.Columns[14].Name = "EXAM_NO";

              dataGridView1.Columns[15].HeaderText = "��Ŀ���";
              dataGridView1.Columns[15].DataPropertyName = "EXAM_ITEM_NO";
              dataGridView1.Columns[15].Name = "EXAM_ITEM_NO";

              dataGridView1.Columns[16].HeaderText = "�Ƽ���Ŀ���";
              dataGridView1.Columns[16].DataPropertyName = "CHARGE_ITEM_NO";
              dataGridView1.Columns[16].Name = "CHARGE_ITEM_NO";

              dataGridView1.Columns[17].HeaderText = "���˱�ʶ";
              dataGridView1.Columns[17].DataPropertyName = "PATIENT_ID";
              dataGridView1.Columns[17].Name = "PATIENT_ID";

              dataGridView1.Columns[18].HeaderText = "סԺ����";
              dataGridView1.Columns[18].DataPropertyName = "VISIT_ID";
              dataGridView1.Columns[18].Name = "VISIT_ID";

              dataGridView1.Columns[19].HeaderText = "��Ŀ���";
              dataGridView1.Columns[19].DataPropertyName = "ITEM_CLASS";
              dataGridView1.Columns[19].Name = "ITEM_CLASS";

              dataGridView1.Columns[20].HeaderText = "����ҽ��";
              dataGridView1.Columns[20].DataPropertyName = "DOCTORNAME";
              dataGridView1.Columns[20].Name = "DOCTORNAME";

 


              for (int i = 0; i < dataGridView1.Rows.Count; i++)
              {
                  if (dataGridView1.Rows[i].Cells["AMOUNT"].Value.ToString().Contains("-"))
                  {
                      dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                  }
                  if (dataGridView1.Rows[i].Cells["BILLING_ATTR"].Value.ToString().Trim()=="0")
                  {
                      dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                      
                  }
                  //if (dataGridView1.Rows[i].Cells["STATUS"].Value.ToString().Trim() == "A")
                  //{
                  //    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.YellowGreen;

                  //}
                  if (dataGridView1.Rows[i].Cells["STATUS"].Value.ToString().Trim() == "F")
                  {
                      dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.YellowGreen;

                  }
              }


                  try
                  {
                      sfje = 0;
                      decimal ysf = 0;
                      for (int i = 0; i < dt_sfm.Rows.Count; i++)
                      {
                          if (dt_sfm.Rows[i]["CHARGES"].ToString() == "" || dt_sfm.Rows[i]["STATUS"].ToString() == "F")
                              continue;
                          sfje = sfje + Convert.ToDecimal(dt_sfm.Rows[i]["CHARGES"].ToString());

                          if (dt_sfm.Rows[i]["STATUS"].ToString() == "A" && dt_sfm.Rows[i]["BILLING_ATTR"].ToString() == "1")
                          {
                              ysf = ysf + Convert.ToDecimal(dt_sfm.Rows[i]["CHARGES"].ToString());
                             
                          }
                      }
                      label7.Text = sfje.ToString() + "Ԫ";
                    
                      label12.Text = "���շ��ã�" + ysf.ToString() + "Ԫ";
                      if (dt_sqd.Rows[0]["InOrOut"].ToString() == "סԺ")
                      {
                          label13.Text = "���ã�" + (sfje - ysf).ToString() + "Ԫ";
                          label12.Text = "";
                      }
                      else
                          label13.Text = "δ�շ��ã�" + (sfje - ysf).ToString() + "Ԫ";
                   
                  }
                  catch (Exception ee)
                  {
                      if (msg == "1")
                          MessageBox.Show(ee.ToString());
                      log.WriteMyLog("�ϼƷ����쳣��" + ee.ToString());
                  }
              }
              else
                  label7.Text = "0Ԫ";
               
        }

       ///// �����Զ�����(biao)
        public void mzzdhj()
        {
             decimal sfje = 0;
            //�����Զ�����
             try
            {
               //���뵥��Ϣ���е� ������Ŀ�����ܶ�����
                string EXAMITEMCODE = dt_sqd.Rows[0]["EXAMITEMCODE"].ToString();
                string[] ITEMCODE = EXAMITEMCODE.Split('^');

             //   string Identitys = dt_sqd.Rows[0]["Identitys"].ToString();
                //ExamItemNo�Ӻţ������м��Ĳ���
                string exam_item_no = dt_sqd.Rows[0]["ExamItemNo"].ToString();
                string[] item_no = exam_item_no.Split('^');
                
                //*********************************************
                //------����������Ŀ-��δ���--------------
                for (int y = 0; y < ITEMCODE.Length; y++)
                {
                  
                    //-----��ȡÿ��������Ŀ��Ӧ�շ���Ŀ��һ�Զ��ϵ---CLINIC_VS_CHARGE-----
                    DataTable dt_CLINIC_VS_CHARGE = select_orcl("select * from CLINIC_VS_CHARGE WHERE CLINIC_ITEM_CODE='" + ITEMCODE[y].ToString().Trim() + "'", "��ȡCHARGE_ITEM_CODE");
                  
                    for (int i = 0; i < dt_CLINIC_VS_CHARGE.Rows.Count; i++)
                    {

                        //******************************************************************
                        //*********************************************************************
                        //��ȡexam_bill_items�м�� �е�charge_item_no�������ظ����벻��
                        int charge_item_no = 1;
                        try
                        {
                            DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "' and exam_item_no='" + exam_item_no + "' order by charge_item_no desc", "��ȡcharge_item_no");
                            if (dt_bill_items.Rows.Count > 0)
                                charge_item_no = int.Parse(dt_bill_items.Rows[0]["charge_item_no"].ToString()) + 1;
                        }
                        catch (Exception exam_bill_items_ee)
                        {
                            log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());
                            return;
                           
                        }
                        //�����շ���Ŀ��Ӧ����۱���ϸ

                        DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,PREFER_PRICE,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + dt_CLINIC_VS_CHARGE.Rows[i]["CHARGE_ITEM_CODE"].ToString().Trim() + "' ", "��ȡ�շ���Ŀ");


                        decimal xmje = 0;
                        if (Identitys == "���ﲡ��")
                        {
                            xmje = decimal.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString().Trim()) * decimal.Parse(dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString().Trim());
                        }
                        else
                            if (Identitys == "��Ժְ��")
                            {
                                xmje = decimal.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString().Trim()) * decimal.Parse(dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString().Trim());
                            }
                            else
                            {
                                xmje = decimal.Parse(dt_sfxm.Rows[0]["PRICE"].ToString().Trim()) * decimal.Parse(dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString().Trim());
                            }
                        //�����շ�
                        try
                        {
                            //����
                            string orclstr2 = " insert into exam.exam_bill_items(exam_no,exam_item_no,charge_item_no,patient_id,visit_id,item_class,item_name,item_code,item_spec,amount,"
                          + "units,ordered_by,performed_by,costs,charges,billing_date_time,operator_no,verified_indicator,doctorname,billing_attr,status,dhc_key)"
                         + " values('" + EXAM_NO + "','" + exam_item_no + "','" + charge_item_no.ToString().Trim() + "','" + dt_sqd.Rows[0]["Patientid"].ToString().Trim() + "','" + dt_sqd.Rows[0]["visitid"].ToString().Trim() + "','" + dt_sfxm.Rows[0]["item_class"].ToString() + "','" + dt_sfxm.Rows[0]["item_name"].ToString() + "','" + dt_sfxm.Rows[0]["item_code"].ToString() + "','" + dt_sfxm.Rows[0]["item_spec"].ToString() + "'"
                       + ",'" + dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString() + "','" + dt_CLINIC_VS_CHARGE.Rows[i]["UNITS"].ToString().Trim() + "','90','90','" + (xmje).ToString("F2") + "','" + (xmje).ToString("F2") + "',to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'),'" + sfgh + "','1','" + sfry + "','0','A','" + EXAM_NO + "')";

                            int x = Execute_orcl(orclstr2, "�����շ���Ϣ");
                            if (x <= 0)
                            {
                                MessageBox.Show("д�м��ʧ��");
                                log.WriteMyLog("д�м��ʧ�ܣ�Ӱ������" + x.ToString());
                                return;
                            }
                            sfje = sfje + xmje;
                        }
                        catch (Exception insert_ee)
                        {
                            MessageBox.Show("д�м���׳��쳣");
                            log.WriteMyLog("д�м���׳��쳣" + insert_ee.ToString());
                            return;
                        }
                        //��for  
                    }//��for
                }
                //******************************
                //------�Զ����շ�--------------
                 
                  string[] sfxms =f.ReadString("SF", "mzsfxm", "220800006^Ƭ^Ƭ^1#270800005^ÿ���걾^/^1").Replace("\0", "").Trim().Split('#');
                foreach(string  sfmx in sfxms)
                {
                    if (sfmx.Trim() != "")
                    {
                        string ITEM_CODE = sfmx.Split('^')[0];
                        string item_spec = sfmx.Split('^')[1];
                        string units = sfmx.Split('^')[2];
                        string AMOUNT = sfmx.Split('^')[3];

                        if (AMOUNT == "")
                            AMOUNT = "1";
                        //******************************************************************
                        //*********************************************************************
                        //��ȡexam_bill_items�м�� �е�charge_item_no�������ظ����벻��
                        int charge_item_no = 1;
                        try
                        {
                            DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "' and exam_item_no='" + exam_item_no + "' order by charge_item_no desc", "��ȡcharge_item_no");
                            if (dt_bill_items.Rows.Count > 0)
                                charge_item_no = int.Parse(dt_bill_items.Rows[0]["charge_item_no"].ToString()) + 1;
                        }
                        catch (Exception exam_bill_items_ee)
                        {
                            log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());
                            return;

                        }
                        //�����շ���Ŀ��Ӧ����۱���ϸ

                        DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,PREFER_PRICE,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + ITEM_CODE + "' ", "��ȡ�շ���Ŀ");


                        decimal xmje = 0;
                        if (Identitys == "���ﲡ��")
                        {
                            xmje = decimal.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString().Trim()) * decimal.Parse(AMOUNT);
                        }
                        else
                            if (Identitys == "��Ժְ��")
                            {
                                xmje = decimal.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString().Trim()) * decimal.Parse(AMOUNT);
                            }
                            else
                            {
                                xmje = decimal.Parse(dt_sfxm.Rows[0]["PRICE"].ToString().Trim()) * decimal.Parse(AMOUNT);
                            }
                        //�����շ�
                        try
                        {
                            //����
                            string orclstr2 = " insert into exam.exam_bill_items(exam_no,exam_item_no,charge_item_no,patient_id,visit_id,item_class,item_name,item_code,item_spec,amount,"
                          + "units,ordered_by,performed_by,costs,charges,billing_date_time,operator_no,verified_indicator,doctorname,billing_attr,status,dhc_key)"
                         + " values('" + EXAM_NO + "','" + exam_item_no + "','" + charge_item_no.ToString().Trim() + "','" + dt_sqd.Rows[0]["Patientid"].ToString().Trim() + "','" + dt_sqd.Rows[0]["visitid"].ToString().Trim() + "','" + dt_sfxm.Rows[0]["item_class"].ToString() + "','" + dt_sfxm.Rows[0]["item_name"].ToString() + "','" + ITEM_CODE + "','" + item_spec + "'"
                       + ",'" + AMOUNT.Trim() + "','" + units.Trim() + "','90','90','" + (xmje).ToString("F2") + "','" + (xmje).ToString("F2") + "',to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'),'" + sfgh + "','1','" + sfry + "','0','A','" + EXAM_NO + "')";

                            int x = Execute_orcl(orclstr2, "�����շ���Ϣ");
                            if (x <= 0)
                            {
                                MessageBox.Show("д�м��ʧ��");
                                log.WriteMyLog("д�м��ʧ�ܣ�Ӱ������" + x.ToString());
                                return;
                            }
                            sfje = sfje + xmje;
                        }
                        catch (Exception insert_ee)
                        {
                            MessageBox.Show("д�м���׳��쳣");
                            log.WriteMyLog("д�м���׳��쳣" + insert_ee.ToString());
                            return;
                        }
                    }
                      
                    }


                  //---��д������Զ����۱��----------------

                string zdhj = "update  Examapply  set zdhj='1',sfje='" + sfje.ToString() + "' where CheckFlow='" + sqxh + "'";
                label7.Text = sfje.ToString()+"Ԫ";
                int rtn_zdhj = Execute_sql(zdhj);
                
            }
            catch (Exception ee)
            {
                MessageBox.Show("�����Զ����ۣ���������쳣");
                log.WriteMyLog("�׳��쳣" + ee.ToString());
                return;
            }
            if (sfje > 0)
            {
                MessageBox.Show("���ﻮ�����");
                button1.Visible = true;
                button2.Visible = false;
            }
        }

        ///// �����Զ����ۣ�ƽ̨ web������sqxh1 û�����壬��㴫.
        public void mzzdhj(string sqxh1)
        {
            decimal sfje = 0;
            //�����Զ�����
            try
            {
                //���뵥��Ϣ���е� ������Ŀ�����ܶ�����
                string EXAMITEMCODE = dt_sqd.Rows[0]["EXAMITEMCODE"].ToString();
                string[] ITEMCODE = EXAMITEMCODE.Split('^');
               // string Identitys = dt_sqd.Rows[0]["Identitys"].ToString();
                //ExamItemNo�Ӻţ������м��Ĳ���
                string exam_item_no = dt_sqd.Rows[0]["ExamItemNo"].ToString();
                string[] item_no = exam_item_no.Split('^');

                //*********************************************
                //------����������Ŀ-��δ���--------------
                for (int y = 0; y < ITEMCODE.Length; y++)
                {

                    //-----��ȡÿ��������Ŀ��Ӧ�շ���Ŀ��һ�Զ��ϵ---CLINIC_VS_CHARGE-----
                    DataTable dt_CLINIC_VS_CHARGE = select_orcl("select * from CLINIC_VS_CHARGE WHERE CLINIC_ITEM_CODE='" + ITEMCODE[y].ToString().Trim() + "'", "��ȡCHARGE_ITEM_CODE");

                    for (int i = 0; i < dt_CLINIC_VS_CHARGE.Rows.Count; i++)
                    {

                        //******************************************************************
                        //*********************************************************************
                        //��ȡexam_bill_items�м�� �е�charge_item_no�������ظ����벻��
                        int charge_item_no = 1;
                        try
                        {
                            DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "' and exam_item_no='" + exam_item_no + "' order by charge_item_no desc", "��ȡcharge_item_no");
                            if (dt_bill_items.Rows.Count > 0)
                                charge_item_no = int.Parse(dt_bill_items.Rows[0]["charge_item_no"].ToString()) + 1;
                        }
                        catch (Exception exam_bill_items_ee)
                        {
                            log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());
                            return;

                        }
                        //�����շ���Ŀ��Ӧ����۱���ϸ

                        DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,PREFER_PRICE,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + dt_CLINIC_VS_CHARGE.Rows[i]["CHARGE_ITEM_CODE"].ToString().Trim() + "' ", "��ȡ�շ���Ŀ");

                        decimal xmje = 0;
                        if (Identitys == "���ﲡ��")
                        {
                            xmje = decimal.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString().Trim()) * decimal.Parse(dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString().Trim());
                        }
                        else
                            if (Identitys == "��Ժְ��")
                            {
                                xmje = decimal.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString().Trim()) * decimal.Parse(dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString().Trim());
                            }
                            else
                            {
                                xmje = decimal.Parse(dt_sfxm.Rows[0]["PRICE"].ToString().Trim()) * decimal.Parse(dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString().Trim());
                            }

                        //�����շ�
                        try
                        {
                            //����
                            string orclstr2 = " insert into exam.exam_bill_items(exam_no,exam_item_no,charge_item_no,patient_id,visit_id,item_class,item_name,item_code,item_spec,amount,"
                          + "units,ordered_by,performed_by,costs,charges,billing_date_time,operator_no,verified_indicator,doctorname,billing_attr,status,dhc_key)"
                         + " values('" + EXAM_NO + "','" + exam_item_no + "','" + charge_item_no.ToString().Trim() + "','" + dt_sqd.Rows[0]["Patientid"].ToString().Trim() + "','" + dt_sqd.Rows[0]["visitid"].ToString().Trim() + "','" + dt_sfxm.Rows[0]["item_class"].ToString() + "','" + dt_sfxm.Rows[0]["item_name"].ToString() + "','" + dt_sfxm.Rows[0]["item_code"].ToString() + "','" + dt_sfxm.Rows[0]["item_spec"].ToString() + "'"
                       + ",'" + dt_CLINIC_VS_CHARGE.Rows[i]["AMOUNT"].ToString() + "','" + dt_CLINIC_VS_CHARGE.Rows[i]["UNITS"].ToString().Trim() + "','90','90','" + (xmje).ToString("F2") + "','" + (xmje).ToString("F2") + "',to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'),'" + sfgh + "','1','" + sfry + "','0','A','" + EXAM_NO + "')";

                            int x = Execute_orcl(orclstr2, "�����շ���Ϣ");
                            if (x <= 0)
                            {
                                MessageBox.Show("д�м��ʧ��");
                                log.WriteMyLog("д�м��ʧ�ܣ�Ӱ������" + x.ToString());
                                return;
                            }
                            sfje = sfje + xmje;
                        }
                        catch (Exception insert_ee)
                        {
                            MessageBox.Show("д�м���׳��쳣");
                            log.WriteMyLog("д�м���׳��쳣" + insert_ee.ToString());
                            return;
                        }
                        //��for  
                    }//��for
                }




                //---��д������Զ����۱��----------------

                string zdhj = "update  Examapply  set zdhj='1',sfje='" + sfje.ToString()+ "' where CheckFlow='" + sqxh + "'";
                int rtn_zdhj = Execute_sql(zdhj);
                label7.Text = sfje.ToString() + "Ԫ";
            }
            catch (Exception ee)
            {
                MessageBox.Show("�����Զ����ۣ���������쳣");
                log.WriteMyLog("�׳��쳣" + ee.ToString());
                return;
            }
            if (sfje > 0)
            {
                MessageBox.Show("���ﻮ�����");
                button1.Visible = true;
                button2.Visible = false;
            }
        }

        ///// �ھ������������Զ�����
        public void nj_mzzdhj()
        { decimal sfje = 0;
            //�ھ�����--�����Զ�����
            try
            {
                string EXAM_ITEM_NO = "1";

                ///�м����ȡEXAM_ITEM_NO��ChargeItemNo
                /////���뵥��Ϣ��sql��
                DataTable dt_sqd = select_sql("select * from Examapply  where checkflow='" + sqxh.Trim() + "'");

                if (dt_sqd.Rows.Count < 1)
                {
                    MessageBox.Show("δ�鵽���뵥��Ϣ"); this.Close();
                }
                //���뵥��Ϣ���е� ��Ŀ��Ŀ----��Ӧ  �������뵥�м�� yzxm��
                     string EXAMITEM = dt_sqd.Rows[0]["examitem"].ToString();
                  string[] E_ITEM = EXAMITEM.Split('^');
                 // string Identitys = dt_sqd.Rows[0]["Identitys"].ToString();
            //ExamItemNo�Ӻţ������м��Ĳ���
            //*********************************************
            //------����������Ŀ-��δ���--------------

                  for (int x = 0; x < E_ITEM.Length; x++)
                  {
                      //-----��ȡҽ����Ŀ��Ӧ�շ���Ŀ��һ�Զ��ϵ---yzxmmx��-----
                      DataTable dt_yzxmmx = select_sql("select * from yzxmmx WHERE yzxmmc='" + E_ITEM[x].ToString().Trim() + "'");

                      if (dt_yzxmmx.Rows.Count < 1)
                      {
                          MessageBox.Show("δ�鵽��Ӧ����Ŀ��ϸ��¼"); this.Close();
                      }

                      //ͨ��CHECK_FLOW ���emr.exam_appoints_id�����EXAM_NO�ţ������м��Ĳ���
                      DataTable dt_item_no = select_orcl("select *  from mzemr.exam_appoints_id where CHECK_FLOW='" + sqxh.Trim() + "'", "");

                      if (dt_item_no.Rows.Count > 0)
                      {
                          DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + dt_item_no.Rows[0]["exam_no"].ToString().Trim() + "' order by exam_item_no desc", "��ȡexam_item_no");
                          if (dt_bill_items.Rows.Count > 0)
                              EXAM_ITEM_NO = (int.Parse(dt_bill_items.Rows[0]["EXAM_ITEM_NO"].ToString()) + 1).ToString();
                          else
                              EXAM_ITEM_NO = "1";

                      }

                      //�����Զ�����
                      try
                      {
                          for (int i = 0; i < dt_yzxmmx.Rows.Count; i++)
                          {
                              //�����շ���Ŀ��Ӧ����۱���ϸ
                              DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,PREFER_PRICE,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + dt_yzxmmx.Rows[i]["xmid"].ToString().Trim() + "' ", "��ȡ�շ���Ŀ");

                              //int charge_item_no = 1;
                              //try
                              //{
                              //    DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + dt_item_no.Rows[0]["exam_no"].ToString().Trim() + "' and exam_item_no='" + EXAM_ITEM_NO + "' order by charge_item_no desc", "��ȡcharge_item_no");
                              //    if (dt_bill_items.Rows.Count > 0)
                              //        charge_item_no = int.Parse(dt_bill_items.Rows[0]["charge_item_no"].ToString()) + 1;
                              //}
                              //catch (Exception exam_bill_items_ee)
                              //{
                              //    log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());
                              //    return;

                              //}

                              decimal xmje = 0;
                              if (Identitys == "���ﲡ��")
                              {
                                  xmje = decimal.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString().Trim()) * 1;
                              }
                              else
                                  if (Identitys == "��Ժְ��")
                                  {
                                      xmje = decimal.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString().Trim()) * 1;
                                  }
                                  else
                                  {
                                      xmje = decimal.Parse(dt_sfxm.Rows[0]["PRICE"].ToString().Trim()) * 1;
                                  }
                              //�����շ�
                              try
                              {

                                  string orclstr2 = " insert into exam.exam_bill_items(exam_no,exam_item_no,charge_item_no,patient_id,visit_id,item_class,item_name,item_code,item_spec,amount,"
                               + "units,ordered_by,performed_by,costs,charges,billing_date_time,operator_no,verified_indicator,doctorname,billing_attr,status,dhc_key)"
                              + " values('" + EXAM_NO.Trim() + "','" + EXAM_ITEM_NO + "','" + (1 + i).ToString() + "','" + dt_sqd.Rows[0]["Patientid"].ToString().Trim() + "','" + dt_sqd.Rows[0]["visitid"].ToString().Trim() + "','" + dt_sfxm.Rows[0]["item_class"].ToString() + "','" + dt_sfxm.Rows[0]["item_name"].ToString() + "','" + dt_sfxm.Rows[0]["item_code"].ToString() + "','" + dt_sfxm.Rows[0]["item_spec"].ToString() + "'"
                            + ",'" + "1" + "','" + dt_sfxm.Rows[0]["UNITS"].ToString().Trim() + "','90','90','" + (xmje).ToString("F2") + "','" + (xmje).ToString("F2") + "',to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'),'BLK','1','" + dt_sqd.Rows[0]["REQPHYSICIAN"].ToString().Trim() + "','0','A','" + EXAM_NO.Trim() + "')";
                                  //   MessageBox.Show(orclstr2);
                                  //   textBox5.Text=orclstr2;
                                  int y = Execute_orcl(orclstr2, "�����շ���Ϣ");
                                  if (y <= 0)
                                      MessageBox.Show("д�м��ʧ�ܣ�Ӱ������0");
                                  sfje=sfje+xmje;
                              }
                              catch (Exception insert_ee)
                              {
                                  MessageBox.Show("д�м���׳��쳣" + insert_ee.ToString());
                              }
                          }
                      }
                      catch (Exception exam_bill_items_ee)
                      {
                          log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());

                      }
                  }
                  //*********************************************


                    //---��д������Զ����۱��----------------
                   if(sfje>0)
                {
                    string zdhj = "update  Examapply  set zdhj='1',sfje='" +sfje + "' where CheckFlow='" + sqxh + "'";
                    int rtn_zdhj = Execute_sql(zdhj);
                }
                
                }
                catch (Exception ee)
                {
                    MessageBox.Show("�����Զ����ۣ���������쳣");
                    log.WriteMyLog("�׳��쳣" + ee.ToString());
                    return;
                }
                if (sfje > 0)
                {
                    MessageBox.Show("���ﻮ�����");
                    button1.Visible = true;
                    button2.Visible = false;
                }
           label7.Text = sfje.ToString() + "Ԫ";
        }
        ////////���ﲹ�ѣ�ƽ̨ web������sqxh1 û�����壬��㴫
        private void mzbf(string  sqxh1)
        {

            string EXAM_ITEM_NO = "1";
            string ChargeItemNo = "1";
            if (dataGridView1.Rows.Count > 0)
            {
                /////////////////////
                ///dataGridView1��ȡEXAM_ITEM_NO��ChargeItemNo
                try
                {
                    for (int x = 0; x < dataGridView1.Rows.Count; x++)
                    {

                        if (int.Parse(EXAM_ITEM_NO) <= int.Parse(dataGridView1.Rows[x].Cells["EXAM_ITEM_NO"].Value.ToString().Trim()))
                            EXAM_ITEM_NO = (int.Parse(dataGridView1.Rows[x].Cells["EXAM_ITEM_NO"].Value.ToString())).ToString();
                    }
                }
                catch
                {
                    EXAM_ITEM_NO = "1";
                }
                //-------ChargeItemNo---------------

                try
                {
                    for (int x = 0; x < dataGridView1.Rows.Count; x++)
                    {

                        if (int.Parse(ChargeItemNo) <= int.Parse(dataGridView1.Rows[x].Cells["Charge_Item_No"].Value.ToString().Trim()))
                            ChargeItemNo = (int.Parse(dataGridView1.Rows[x].Cells["Charge_Item_No"].Value.ToString()) + 1).ToString();
                    }
                }
                catch
                {
                    ChargeItemNo = "1";
                }
            }
            else
            {
                /////////////////////
                ///�м����ȡEXAM_ITEM_NO��ChargeItemNo
                try
                {
                    DataTable dt_item_no = select_orcl("select *  from emr.exam_appoints_id where CHECK_FLOW='" + sqxh.Trim() + "'", "");

                    if (dt_item_no.Rows.Count > 0)
                    {
                        DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + dt_item_no.Rows[0]["exam_no"].ToString().Trim() + "' order by exam_item_no desc", "��ȡexam_item_no");
                        if (dt_bill_items.Rows.Count > 0)
                            EXAM_ITEM_NO = (int.Parse(dt_bill_items.Rows[0]["EXAM_ITEM_NO"].ToString()) + 1).ToString();
                        else
                            EXAM_ITEM_NO = "1";

                    }
                }
                catch (Exception exam_bill_items_ee)
                {
                    log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());

                }
            }
           ///////////////////////////////////////////////////////////////////////////////////
            string xmlstr = "<?xml version='1.0' encoding='UTF-8' ?><Request><OutBillItemsList>";

            ////��ȡdataGridView1�еķ���/////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////////////
            for (int x = 0; x < dt_sfm.Rows.Count; x++)
            {
                if (dt_sfm.Rows[x]["STATUS"].ToString().Trim() == "A" && dt_sfm.Rows[x]["BILLING_ATTR"].ToString().Trim() == "0")
                {
                    xmlstr = xmlstr + "<OutBillItems>";
                    xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                    xmlstr = xmlstr + "<ExamItemNo>" + dt_sfm.Rows[x]["Exam_Item_No"].ToString().Trim() + "</ExamItemNo>";
                    xmlstr = xmlstr + " <ChargeItemNo>" + dt_sfm.Rows[x]["Charge_Item_No"].ToString().Trim() + "</ChargeItemNo> ";
                    xmlstr = xmlstr + " <PatientId>" + dt_sfm.Rows[x]["Patient_Id"].ToString().Trim() + "</PatientId> ";
                    xmlstr = xmlstr + " <VisitId>" + dt_sfm.Rows[x]["Visit_Id"].ToString().Trim() + "</VisitId>";
                    xmlstr = xmlstr + " <ItemClass>" + dt_sfm.Rows[x]["Item_Class"].ToString().Trim() + "</ItemClass>";
                    xmlstr = xmlstr + " <ItemName>" + dt_sfm.Rows[x]["Item_Name"].ToString().Trim() + "</ItemName>";
                    xmlstr = xmlstr + " <ItemCode>" + dt_sfm.Rows[x]["Item_Code"].ToString().Trim() + "</ItemCode>";
                    xmlstr = xmlstr + " <ItemSpec>" + dt_sfm.Rows[x]["Item_Spec"].ToString().Trim() + "</ItemSpec>";
                    xmlstr = xmlstr + " <Amount>" + dt_sfm.Rows[x]["Amount"].ToString().Trim() + "</Amount>";
                    xmlstr = xmlstr + " <Units>" + dt_sfm.Rows[x]["Units"].ToString().Trim() + "</Units>";
                    xmlstr = xmlstr + " <OrderedBy>" + dt_sfm.Rows[x]["Ordered_By"].ToString().Trim() + "</OrderedBy>";
                    xmlstr = xmlstr + " <PerformedBy>" + dt_sfm.Rows[x]["Performed_By"].ToString().Trim() + "</PerformedBy>";
                    xmlstr = xmlstr + " <Costs>" + dt_sfm.Rows[x]["Costs"].ToString().Trim() + "</Costs>";
                    xmlstr = xmlstr + " <Charges>" + dt_sfm.Rows[x]["Charges"].ToString().Trim() + "</Charges>";
                    xmlstr = xmlstr + " <BillingDateTime>" + dt_sfm.Rows[x]["Billing_Date_Time"].ToString().Trim() + "</BillingDateTime>";
                    xmlstr = xmlstr + " <OperatorNo>" + dt_sfm.Rows[x]["Operator_No"].ToString().Trim() + "</OperatorNo>";
                    xmlstr = xmlstr + " <VerifiedIndicator>" + dt_sfm.Rows[x]["Verified_Indicator"].ToString().Trim() + "</VerifiedIndicator>";
                    xmlstr = xmlstr + " <DoctorName>" + dt_sfm.Rows[x]["DoctorName"].ToString().Trim() + "</DoctorName>";
                    xmlstr = xmlstr + " <BillingAttr>" + dt_sfm.Rows[x]["Billing_Attr"].ToString().Trim() + "</BillingAttr>";
                    xmlstr = xmlstr + " <Status>" + dt_sfm.Rows[x]["Status"].ToString().Trim() + "</Status>";
                    xmlstr = xmlstr + " <DhcKey>" + dt_sfm.Rows[x]["Dhc_Key"].ToString().Trim() + "</DhcKey>";
                    xmlstr = xmlstr + "</OutBillItems>";
                }
            }



                //////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////
                try
                {
                    xmlstr = xmlstr + "<OutBillItems>";
                    xmlstr = xmlstr + "<CheckFlow>" + dt_sqd.Rows[0]["CheckFlow"].ToString().Trim() + "</CheckFlow>";
                    xmlstr = xmlstr + "<ExamItemNo>" + EXAM_ITEM_NO + "</ExamItemNo>";
                    xmlstr = xmlstr + " <ChargeItemNo>" + ChargeItemNo + "</ChargeItemNo> ";
                    xmlstr = xmlstr + " <PatientId>" + dt_sqd.Rows[0]["patientid"].ToString().Trim() + "</PatientId> ";
                    xmlstr = xmlstr + " <VisitId>" + dt_sqd.Rows[0]["visitid"].ToString().Trim() + "</VisitId>";
                    xmlstr = xmlstr + " <ItemClass>" + txt_ItemClass.Text.Trim() + "</ItemClass>";
                    xmlstr = xmlstr + " <ItemName>" + textBox1.Text.Trim() + "</ItemName>";
                    xmlstr = xmlstr + " <ItemCode>" + textBox2.Text.Trim() + "</ItemCode>";
                    xmlstr = xmlstr + " <ItemSpec>" + txt_ITEM_SPEC.Text.Trim() + "</ItemSpec>";
                    xmlstr = xmlstr + " <Amount>" + numericUpDown1.Value + "</Amount>";
                    xmlstr = xmlstr + " <Units>" + laldw.Text.Trim() + "</Units>";
                    xmlstr = xmlstr + " <OrderedBy>" + "90" + "</OrderedBy>";
                    xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";
                    xmlstr = xmlstr + " <Costs>" + (double.Parse(lalCosts.Text.Trim()) * double.Parse(numericUpDown1.Value.ToString())).ToString("F2") + "</Costs>";
                    xmlstr = xmlstr + " <Charges>" + (double.Parse(lal_prefer_price.Text.Trim()) * double.Parse(numericUpDown1.Value.ToString())).ToString("F2") + "</Charges>";
                    xmlstr = xmlstr + " <BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                    xmlstr = xmlstr + " <OperatorNo>" + sfgh + "</OperatorNo>";
                    xmlstr = xmlstr + " <VerifiedIndicator>" + "01" + "</VerifiedIndicator>";
                    xmlstr = xmlstr + " <DoctorName>" + sfry + "</DoctorName>";
                    xmlstr = xmlstr + " <BillingAttr>" + "0" + "</BillingAttr>";
                    xmlstr = xmlstr + " <Status>" + "A" + "</Status>";
                    xmlstr = xmlstr + " <DhcKey>" + "999999999" + "</DhcKey>";
                    xmlstr = xmlstr + "</OutBillItems>";
                }
                catch (Exception xml_e)
                {

                    MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                    return;
                }
           
            xmlstr = xmlstr + "</OutBillItemsList></Request>";

            MessageBox.Show(xmlstr);
            string aa = "";
            try
            {
                aa = xyfy.DhcService("OutBillItems", xmlstr);


            }
            catch
            {
                MessageBox.Show("ƽ̨�����쳣");
            }

            MessageBox.Show(aa.Trim());
            try
            {
                if (aa == "")
                {
                    MessageBox.Show("����Ʒ�ʧ�ܣ����ؿ�");
                    this.Close();
                }
                XmlDataDocument xd = new XmlDataDocument();
                xd.LoadXml(aa);
                XmlNode xn = xd.SelectSingleNode("/Response");
                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                { //if (msg == "1")
                    MessageBox.Show("����Ʒ�ʧ�ܣ�ԭ��" + xn.FirstChild["ResultContent"].InnerText.ToString());
                    log.WriteMyLog("����Ʒ�ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                    return;
                }
                else
                {
                    MessageBox.Show("�Ʒѳɹ���");
                }


            }
            catch (Exception rtnee)
            {
                // if (msg == "1")

                MessageBox.Show("����Ʒ�ʧ��,����XML����,�׳��쳣");
                log.WriteMyLog("����Ʒ�ʧ��,����XML����,�׳��쳣��" + rtnee.ToString());
                return;

            }
           
        }
   
        ////////���ﲹ��(biao)
        private void mzbf()
        { //���ﲹ��
            try
            {
                //���뵥��Ϣ
                //string exam_item_no = "";
                string exam_item_no = dt_sqd.Rows[0]["ExamItemNo"].ToString();
                ////��ȡexam_item_no�������뵥�����м�¼
                string[] item_no = exam_item_no.Split('^');
                //��ȡexam_bill_items �е�charge_item_no�������ظ����벻��
                int charge_item_no = 1;
                int exam_itemno = 1;
                try
                {
                    DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "'  order by  exam_ITEM_NO desc,charge_item_no  desc", "��ȡcharge_item_no");
                    if (dt_bill_items.Rows.Count > 0)
                        exam_itemno = int.Parse(dt_bill_items.Rows[0]["exam_item_no"].ToString()) + 1;
                    //  charge_item_no = int.Parse(dt_bill_items.Rows[0]["charge_item_no"].ToString()) + 1;
                }
                catch (Exception exam_bill_items_ee)
                {

                    MessageBox.Show("�������,��ȡexam_item_no�����׳��쳣��");
                    log.WriteMyLog("��ȡexam_item_no����" + exam_bill_items_ee.ToString());
                    return;
                }
                //------------------------------------------------
                try
                {
                    //����
                    string orclstr2 = " insert into exam.exam_bill_items(exam_no,exam_item_no,charge_item_no,patient_id,visit_id,item_class,item_name,item_code,item_spec,amount,"
                  + "units,ordered_by,performed_by,costs,charges,billing_date_time,operator_no,verified_indicator,doctorname,billing_attr,status,dhc_key)"
                 + " values('" + EXAM_NO + "','" + exam_itemno.ToString() + "','" + charge_item_no.ToString().Trim() + "','" + dt_sqd.Rows[0]["patientid"].ToString().Trim() + "','"
                 + dt_sqd.Rows[0]["visitid"].ToString().Trim() + "','" + txt_ItemClass.Text.Trim() + "','" + textBox1.Text.Trim() + "','" + textBox2.Text.Trim() + "','" + txt_ITEM_SPEC.Text.Trim() + "'"
                + ",'" + numericUpDown1.Value + "','" + laldw.Text.Trim() + "','90','90','" + (double.Parse(lalCosts.Text.Trim()) * double.Parse(numericUpDown1.Value.ToString())).ToString("F2") + "','" + (double.Parse(lal_prefer_price.Text.Trim()) * double.Parse(numericUpDown1.Value.ToString())).ToString("F2")
                + "',to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss'),'" + sfgh + "','1','" + sfry + "','0','A','" + EXAM_NO + "')";
                    int x = Execute_orcl(orclstr2, "�����շ���Ϣ");
                    if (x > 0)
                        MessageBox.Show("�շѳɹ���");
                    else
                        MessageBox.Show("�շ���Ϣ����ʧ�ܣ������²�����");

                }
                catch (Exception insert_ee)
                {
                    MessageBox.Show("��������׳��쳣��");
                    log.WriteMyLog("�׳��쳣" + insert_ee.ToString());
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("�������,�׳��쳣������رգ�");
                log.WriteMyLog("�׳��쳣" + ee.ToString());
                return ;
            }
        }

        /////סԺ����
        private void zybf()
        {
            //סԺ����
             try
            {
               
                string xmlstr = "<?xml version='1.0' encoding='UTF-8' ?><Request><InpBilldetailList>";
                //-------EXAM_ITEM_NO-------------------------------
                string EXAM_ITEM_NO = "1"; 
                 string ChargeItemNo = "1";
                 if (dataGridView1.Rows.Count > 0)
                 {
                     /////////////////////
                     ///dataGridView1��ȡEXAM_ITEM_NO��ChargeItemNo
                     try
                     {
                         for (int x = 0; x < dataGridView1.Rows.Count; x++)
                         {

                             if (int.Parse(EXAM_ITEM_NO) <= int.Parse(dataGridView1.Rows[x].Cells["EXAM_ITEM_NO"].Value.ToString().Trim()))
                                 EXAM_ITEM_NO = (int.Parse(dataGridView1.Rows[x].Cells["EXAM_ITEM_NO"].Value.ToString())).ToString();
                         }
                     }
                     catch
                     {
                         EXAM_ITEM_NO = "1";
                     }
                

                     //-------ChargeItemNo---------------

                     try
                     {
                         for (int x = 0; x < dataGridView1.Rows.Count; x++)
                         {

                             if (int.Parse(ChargeItemNo) <= int.Parse(dataGridView1.Rows[x].Cells["Charge_Item_No"].Value.ToString().Trim()))
                                 ChargeItemNo = (int.Parse(dataGridView1.Rows[x].Cells["Charge_Item_No"].Value.ToString()) + 1).ToString();
                         }
                     }
                     catch
                     {
                         ChargeItemNo = "1";
                     }
                 }
                 else
                 {
                     /////////////////////
                     ///�м����ȡEXAM_ITEM_NO��ChargeItemNo
                     try
                     {
                         DataTable dt_item_no = select_orcl("select *  from emr.exam_appoints_id where CHECK_FLOW='" + sqxh.Trim() + "'", "");
                       
                        if (dt_item_no.Rows.Count > 0)
                        {
                            DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + dt_item_no.Rows[0]["exam_no"].ToString().Trim() + "' order by exam_item_no desc", "��ȡexam_item_no");
                            if (dt_bill_items.Rows.Count > 0)
                                EXAM_ITEM_NO = (int.Parse(dt_bill_items.Rows[0]["EXAM_ITEM_NO"].ToString()) + 1).ToString();
                            else
                                EXAM_ITEM_NO = "1";

                       }
                    }
                    catch (Exception exam_bill_items_ee)
                    {
                        log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());

                    } 
                 }
                 //----------------------


                try
                {  
                    xmlstr = xmlstr + "<InpBilldetail>";
                    xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                    xmlstr = xmlstr + "<ExamItemNo>" + EXAM_ITEM_NO + "</ExamItemNo>";
                    xmlstr = xmlstr + " <ChargeItemNo>" + ChargeItemNo + "</ChargeItemNo> ";
                    xmlstr = xmlstr + " <PatientId>" + dt_sqd.Rows[0]["PatientId"].ToString() + "</PatientId>";
                    xmlstr = xmlstr + "<VisitId>" + dt_sqd.Rows[0]["VisitId"].ToString() + "</VisitId>";
                    xmlstr = xmlstr + "<ItemNo>" + "" + "</ItemNo>";
                    xmlstr = xmlstr + "<ItemClass>" + txt_ItemClass.Text.Trim() + "</ItemClass>";
                    xmlstr = xmlstr + " <ItemName>" + textBox1.Text.Trim() + "</ItemName>";
                    xmlstr = xmlstr + " <ItemCode>" + textBox2.Text.Trim() + "</ItemCode>";
                    xmlstr = xmlstr + "<ItemSpec>" + txt_ITEM_SPEC.Text.Trim() + "</ItemSpec>";
                    xmlstr = xmlstr + "<Amount>" + numericUpDown1.Value + "</Amount>";  //����
                    xmlstr = xmlstr + "<Units>" + laldw.Text.Trim() + "</Units>";
                    xmlstr = xmlstr + "<OrderedBy>" + "90" + "</OrderedBy>";
                    xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";
                    xmlstr = xmlstr + "<Costs>" + (double.Parse(lalCosts.Text.Trim()) * double.Parse(numericUpDown1.Value.ToString())).ToString("F2") + "</Costs>";  //COSTS
                    xmlstr = xmlstr + "<Charges>" + (double.Parse(lal_prefer_price.Text.Trim()) * double.Parse(numericUpDown1.Value.ToString())).ToString("F2")+ "</Charges>"; //CHARGES
                    xmlstr = xmlstr + "<BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                    xmlstr = xmlstr + "<OperatorNo>" + sfgh + "</OperatorNo>";
                    xmlstr = xmlstr + "<DoctorName>" + sfry + "</DoctorName>";
                    xmlstr = xmlstr + "<DhcPbd>" + "" + "</DhcPbd>";
                    xmlstr = xmlstr + "<DHCCode>" + txtKEYCOLNO.Text.Trim() + "</DHCCode>";
                    xmlstr = xmlstr + "</InpBilldetail>";
                }
                catch (Exception xml_e)
                {
                    MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                    return;
                }

                xmlstr = xmlstr + "</InpBilldetailList></Request>";
               string aa = xyfy.DhcService("InpBilldetail", xmlstr);
           
            try
            {
                XmlDataDocument xd = new XmlDataDocument();
                xd.LoadXml(aa);
                XmlNode xn = xd.SelectSingleNode("/Response");
                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                { 
                    MessageBox.Show("סԺ����ʧ�ܣ�ԭ��" + xn.FirstChild["ResultContent"].InnerText.ToString());
                log.WriteMyLog("סԺ����ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                return;
                }
                else
                {
                    MessageBox.Show("���ѳɹ���" );
                }


            }
            catch (Exception rtnee)
            { MessageBox.Show("סԺ����ʧ��,����XML����,�׳��쳣" );
                log.WriteMyLog("סԺ����ʧ��,����XML����,�׳��쳣��" + rtnee.ToString());
             return;

            }
        }
        catch(Exception  ee)
        {
            MessageBox.Show("סԺ����ʧ��,�׳��쳣");
            log.WriteMyLog("סԺ����ʧ��,�׳��쳣��" + ee.ToString());
            return;
        }
   }

       ///// סԺ����--סԺȷ�Ϸ���(ƽ̨)
        private void zyqrsf()
        {
          //סԺȷ�Ϸ���
           //���뵥��Ϣ���е� ������Ŀ�����ܶ�����
            string EXAMITEMCODE = dt_sqd.Rows[0]["EXAMITEMCODE"].ToString();
            string[] E_ITEMCODE = EXAMITEMCODE.Split('^');

            //ExamItemNo�Ӻţ������м��Ĳ���
            string exam_item_no = dt_sqd.Rows[0]["ExamItemNo"].ToString();
            string[] item_no = exam_item_no.Split('^');
            ////////////////////////////////////////////////////////
            string xmlstr = "<?xml version='1.0' encoding='UTF-8' ?><Request><InpBilldetailList>";

            //*********************************************
            //------����������Ŀ-��δ���--------------

            for (int x = 0; x < E_ITEMCODE.Length; x++)
            {
                //-----��ȡÿ��������Ŀ��Ӧ�շ���Ŀ��һ�Զ��ϵ---CLINIC_VS_CHARGE-----
                DataTable dt_CLINIC_VS_CHARGE = select_orcl("select * from CLINIC_VS_CHARGE WHERE CLINIC_ITEM_CODE='" + E_ITEMCODE[x].ToString().Trim() + "'", "��ȡCHARGE_ITEM_CODE");
                for (int y = 0; y < dt_CLINIC_VS_CHARGE.Rows.Count; y++)
                {

                    //DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "' order by charge_item_no desc", "��ȡcharge_item_no");
                    //�����շ���Ŀ��Ӧ����۱���ϸ
                    DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,PREFER_PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,KEYCOLNO,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + dt_CLINIC_VS_CHARGE.Rows[y]["CHARGE_ITEM_CODE"].ToString() + "'  and item_spec='" + dt_CLINIC_VS_CHARGE.Rows[y]["CHARGE_ITEM_spec"].ToString() + "' and   units='" + dt_CLINIC_VS_CHARGE.Rows[y]["units"].ToString() + "'", "��ȡ�շ���Ŀ");

                    if (dt_sfxm.Rows.Count <= 0)
                    {
                        MessageBox.Show("��ȡ�����м����󣬲��ܼƷ�");
                        return;

                    }

                    try
                    {
                        xmlstr = xmlstr + "<InpBilldetail>";
                        xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                        xmlstr = xmlstr + "<ExamItemNo>" + item_no[x].ToString() + "</ExamItemNo>";
                        xmlstr = xmlstr + " <ChargeItemNo>" + (x+1) + "</ChargeItemNo> ";
                        xmlstr = xmlstr + " <PatientId>" + dt_sqd.Rows[0]["PatientId"].ToString() + "</PatientId>";
                        xmlstr = xmlstr + "<VisitId>" + dt_sqd.Rows[0]["VisitId"].ToString() + "</VisitId>";
                        xmlstr = xmlstr + "<ItemNo>" + "" + "</ItemNo>";
                        xmlstr = xmlstr + "<ItemClass>" + dt_sfxm.Rows[0]["Item_Class"].ToString() + "</ItemClass>";
                        xmlstr = xmlstr + " <ItemName>" + dt_sfxm.Rows[0]["Item_Name"].ToString() + "</ItemName>";
                        xmlstr = xmlstr + " <ItemCode>" + dt_sfxm.Rows[0]["Item_Code"].ToString() + "</ItemCode>";
                        xmlstr = xmlstr + "<ItemSpec>" + dt_sfxm.Rows[0]["Item_Spec"].ToString() + "</ItemSpec>";
                        xmlstr = xmlstr + "<Amount>" + dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString() + "</Amount>";  //����
                        xmlstr = xmlstr + "<Units>" + dt_CLINIC_VS_CHARGE.Rows[y]["Units"].ToString() + "</Units>";
                        xmlstr = xmlstr + "<OrderedBy>" + "90" + "</OrderedBy>";
                        xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";


                        if (Identitys == "���ﲡ��")
                        {
                          
                            xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Costs>";  //COSTS
                            xmlstr = xmlstr + "<Charges>" + (double.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Charges>"; //CHARGES
                       
                        }
                        else
                            if (Identitys == "��Ժְ��")
                            {
                               
                                xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Costs>";  //COSTS
                                xmlstr = xmlstr + "<Charges>" + (double.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Charges>"; //CHARGES
                       
                            }
                            else
                            {
                              
                                xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Costs>";  //COSTS
                                xmlstr = xmlstr + "<Charges>" + (double.Parse(dt_sfxm.Rows[0]["PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Charges>"; //CHARGES
                       
                            }

                        ////xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["Price"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Costs>";  //COSTS
                        ////xmlstr = xmlstr + "<Charges>" +(double.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Charges>"; //CHARGES
                        xmlstr = xmlstr + "<BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                        xmlstr = xmlstr + "<OperatorNo>" + sfgh + "</OperatorNo>";
                        xmlstr = xmlstr + "<DoctorName>" + sfry + "</DoctorName>";
                        xmlstr = xmlstr + "<DhcPbd>" + "" + "</DhcPbd>"; //Dhc_Pbd
                        xmlstr = xmlstr + "<DHCCode>" + dt_sfxm.Rows[0]["KEYCOLNO"].ToString() + "</DHCCode>";
                        xmlstr = xmlstr + "</InpBilldetail>";
                    }
                    catch (Exception xml_e)
                    {
                        MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                        return;
                    }
                }
            }

            //*********************************************
            //------�Զ����շ���Ŀ--------------
            string[] sfxms = f.ReadString("SF", "zysfxm", "220800006^Ƭ^Ƭ^1#270800005^ÿ���걾^/^1").Replace("\0", "").Trim().Split('#');

                DataTable dt_item_no = select_orcl("select *  from emr.exam_appoints_id where CHECK_FLOW='" + sqxh.Trim() + "'", "");

                foreach(string  sfmx in sfxms)
                {
                    if (sfmx.Trim() != "")
                    {
                        string ITEM_CODE = sfmx.Split('^')[0];
                        string item_spec = sfmx.Split('^')[1];
                        string units = sfmx.Split('^')[2];
                        string AMOUNT = sfmx.Split('^')[3];
                        //DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "' order by charge_item_no desc", "��ȡcharge_item_no");
                        //�����շ���Ŀ��Ӧ����۱���ϸ
                        DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,PREFER_PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,KEYCOLNO,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + ITEM_CODE + "'  and item_spec='" + item_spec + "' and   units='" + units + "'", "��ȡ�շ���Ŀ");

                        if (dt_sfxm.Rows.Count <= 0)
                        {
                            MessageBox.Show("��ȡ�����м����󣬲��ܼƷ�");
                            return;

                        }
                        string EXAM_ITEM_NO = "1";
                        if (dt_item_no.Rows.Count > 0)
                        {
                            DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + dt_item_no.Rows[0]["exam_no"].ToString().Trim() + "' order by exam_item_no desc", "��ȡexam_item_no");
                            if (dt_bill_items.Rows.Count > 0)
                                EXAM_ITEM_NO = (int.Parse(dt_bill_items.Rows[0]["EXAM_ITEM_NO"].ToString()) + 1).ToString();
                        }
                        try
                        {
                            xmlstr = xmlstr + "<InpBilldetail>";
                            xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                            xmlstr = xmlstr + "<ExamItemNo>" + EXAM_ITEM_NO + "</ExamItemNo>";
                            xmlstr = xmlstr + " <ChargeItemNo>" + 1 + "</ChargeItemNo> ";
                            xmlstr = xmlstr + " <PatientId>" + dt_sqd.Rows[0]["PatientId"].ToString() + "</PatientId>";
                            xmlstr = xmlstr + "<VisitId>" + dt_sqd.Rows[0]["VisitId"].ToString() + "</VisitId>";
                            xmlstr = xmlstr + "<ItemNo>" + "" + "</ItemNo>";
                            xmlstr = xmlstr + "<ItemClass>" + dt_sfxm.Rows[0]["Item_Class"].ToString() + "</ItemClass>";
                            xmlstr = xmlstr + " <ItemName>" + dt_sfxm.Rows[0]["Item_Name"].ToString() + "</ItemName>";
                            xmlstr = xmlstr + " <ItemCode>" + dt_sfxm.Rows[0]["Item_Code"].ToString() + "</ItemCode>";
                            xmlstr = xmlstr + "<ItemSpec>" +item_spec+ "</ItemSpec>";
                            xmlstr = xmlstr + "<Amount>" + AMOUNT + "</Amount>";  //����
                            xmlstr = xmlstr + "<Units>" + units + "</Units>";
                            xmlstr = xmlstr + "<OrderedBy>" + "90" + "</OrderedBy>";
                            xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";


                            if (Identitys == "���ﲡ��")
                            {

                                xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString()) * double.Parse(AMOUNT)).ToString("F2") + "</Costs>";  //COSTS
                                xmlstr = xmlstr + "<Charges>" + (double.Parse(dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString()) * double.Parse(AMOUNT)).ToString("F2") + "</Charges>"; //CHARGES

                            }
                            else
                                if (Identitys == "��Ժְ��")
                                {

                                    xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString()) * double.Parse(AMOUNT)).ToString("F2") + "</Costs>";  //COSTS
                                    xmlstr = xmlstr + "<Charges>" + (double.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString()) * double.Parse(AMOUNT)).ToString("F2") + "</Charges>"; //CHARGES

                                }
                                else
                                {

                                    xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["PRICE"].ToString()) * double.Parse(AMOUNT)).ToString("F2") + "</Costs>";  //COSTS
                                    xmlstr = xmlstr + "<Charges>" + (double.Parse(dt_sfxm.Rows[0]["PRICE"].ToString()) * double.Parse(AMOUNT)).ToString("F2") + "</Charges>"; //CHARGES

                                }

                            ////xmlstr = xmlstr + "<Costs>" + (double.Parse(dt_sfxm.Rows[0]["Price"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Costs>";  //COSTS
                            ////xmlstr = xmlstr + "<Charges>" +(double.Parse(dt_sfxm.Rows[0]["PREFER_PRICE"].ToString()) * double.Parse(dt_CLINIC_VS_CHARGE.Rows[y]["AMOUNT"].ToString())).ToString("F2") + "</Charges>"; //CHARGES
                            xmlstr = xmlstr + "<BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                            xmlstr = xmlstr + "<OperatorNo>" + sfgh + "</OperatorNo>";
                            xmlstr = xmlstr + "<DoctorName>" + sfry + "</DoctorName>";
                            xmlstr = xmlstr + "<DhcPbd>" + "" + "</DhcPbd>"; //Dhc_Pbd
                            xmlstr = xmlstr + "<DHCCode>" + dt_sfxm.Rows[0]["KEYCOLNO"].ToString() + "</DHCCode>";
                            xmlstr = xmlstr + "</InpBilldetail>";
                        }
                        catch (Exception xml_e)
                        {
                            MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                            return;
                        }
                    }
                }

            xmlstr = xmlstr + "</InpBilldetailList></Request>";
        
            string aa = xyfy.DhcService("InpBilldetail", xmlstr);
          try
            {
                XmlDataDocument xd = new XmlDataDocument();
                xd.LoadXml(aa);
                XmlNode xn = xd.SelectSingleNode("/Response");
                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                {
                    MessageBox.Show("סԺ����ȷ��ʧ�ܣ�ԭ��" + xn.FirstChild["ResultContent"].InnerText.ToString());
                   }
                else
                {
                    Execute_sql("update  Examapply set jfbj='1' where CheckFlow='"+sqxh+"'");
                    MessageBox.Show("סԺ����ȷ����ɣ�" + xn.FirstChild["ResultContent"].InnerText.ToString());
                    button1.Visible = true;
                    button2.Visible = false;
                }


            }
            catch (Exception rtnee)
            {
                MessageBox.Show("סԺ����ȷ��ʧ��,����XML����,�׳��쳣��" + rtnee.ToString());
            
            }
        }

        ///// �ھ�����-סԺ����--סԺȷ�Ϸ���(ƽ̨)
        private void nj_zyqrsf()
        { 
            if (dt_sqd.Rows[0]["kdxt"].ToString().Trim() != "PIS")
            {
                this.Close();
            }
            //סԺȷ�Ϸ���

             string EXAMITEM = dt_sqd.Rows[0]["examitem"].ToString();
            string[] E_ITEM = EXAMITEM.Split('^');

            //ExamItemNo�Ӻţ������м��Ĳ���
   
            ////////////////////////////////////////////////////////
            string xmlstr = "<?xml version='1.0' encoding='UTF-8' ?><Request><InpBilldetailList>";

            //*********************************************
            //------����������Ŀ-��δ���--------------
            int charge_item_no = 1;
            for (int x = 0; x < E_ITEM.Length; x++)
            {
                //���뵥��Ϣ���е� ������Ŀ�����ܶ�����
                ///// string EXAMITEMCODE = dt_sqd.Rows[0]["EXAMITEMCODE"].ToString();
                ////////////////////////////////////////////////////////
             
                try
                {

                    DataTable dt_item_no = select_orcl("select *  from emr.exam_appoints_id where CHECK_FLOW='" + sqxh.Trim() + "'", "");
                    ///////////////////////////////////////////////////////////
                    if (dt_item_no.Rows.Count > 0)
                    {
                        DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + dt_item_no.Rows[0]["exam_no"].ToString().Trim() + "' order by exam_item_no desc", "��ȡexam_item_no");
                        if (dt_bill_items.Rows.Count > 0)
                            charge_item_no = int.Parse(dt_bill_items.Rows[0]["charge_item_no"].ToString()) + 1;

                    }
                }
                catch (Exception exam_bill_items_ee)
                {
                    log.WriteMyLog("��ȡexam_bill_items����" + exam_bill_items_ee.ToString());

                }


                ////////////////////////////////////////////////////////
          
                //-----��ȡÿ��������Ŀ��Ӧ�շ���Ŀ��һ�Զ��ϵ---CLINIC_VS_CHARGE-----
                DataTable dt_CLINIC_VS_CHARGE = select_sql("select * from yzxmmx WHERE yzxmmc='" + E_ITEM[x].ToString().Trim() + "'");
                for (int y = 0; y < dt_CLINIC_VS_CHARGE.Rows.Count; y++)
                {

                    //DataTable dt_bill_items = select_orcl("select *  from exam.exam_bill_items where exam_no='" + EXAM_NO + "' order by charge_item_no desc", "��ȡcharge_item_no");
                    //�����շ���Ŀ��Ӧ����۱���ϸ
                    DataTable dt_sfxm = select_orcl("select ITEM_CODE,ITEM_NAME,UNITS,PRICE,PREFER_PRICE,ITEM_SPEC,CLASS_ON_MR,ITEM_CLASS,KEYCOLNO,FOREIGNER_PRICE from current_price_list  where ITEM_CODE='" + dt_CLINIC_VS_CHARGE.Rows[y]["xmid"].ToString() + "'    and item_spec='" + dt_CLINIC_VS_CHARGE.Rows[y]["ITEM_spec"].ToString() + "' and   units='" + dt_CLINIC_VS_CHARGE.Rows[y]["units"].ToString() + "' ", "��ȡ�շ���Ŀ");

                    try
                    {
                        xmlstr = xmlstr + "<InpBilldetail>";
                        xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                        xmlstr = xmlstr + "<ExamItemNo>" + charge_item_no + "</ExamItemNo>";
                        xmlstr = xmlstr + " <ChargeItemNo>" + (y + 1) + "</ChargeItemNo> ";
                        xmlstr = xmlstr + " <PatientId>" + dt_sqd.Rows[0]["PatientId"].ToString() + "</PatientId>";
                        xmlstr = xmlstr + "<VisitId>" + dt_sqd.Rows[0]["VisitId"].ToString() + "</VisitId>";
                        xmlstr = xmlstr + "<ItemNo>" + "" + "</ItemNo>";
                        xmlstr = xmlstr + "<ItemClass>" + dt_sfxm.Rows[0]["Item_Class"].ToString() + "</ItemClass>";
                        xmlstr = xmlstr + " <ItemName>" + dt_sfxm.Rows[0]["Item_Name"].ToString() + "</ItemName>";
                        xmlstr = xmlstr + " <ItemCode>" + dt_sfxm.Rows[0]["Item_Code"].ToString() + "</ItemCode>";
                        xmlstr = xmlstr + "<ItemSpec>" + dt_sfxm.Rows[0]["Item_Spec"].ToString() + "</ItemSpec>";
                        xmlstr = xmlstr + "<Amount>" + "1" + "</Amount>";  //����
                        xmlstr = xmlstr + "<Units>" + dt_sfxm.Rows[0]["Units"].ToString() + "</Units>";
                        xmlstr = xmlstr + "<OrderedBy>" + "90" + "</OrderedBy>";
                        xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";

                        if (Identitys == "���ﲡ��") 
                        {
                            xmlstr = xmlstr + "<Costs>" + dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString() + "</Costs>";  //COSTS
                            xmlstr = xmlstr + "<Charges>" + dt_sfxm.Rows[0]["FOREIGNER_PRICE"].ToString() + "</Charges>"; //CHARGES
                        }
                        else
                            if (Identitys == "��Ժְ��")
                            {
                                xmlstr = xmlstr + "<Costs>" + dt_sfxm.Rows[0]["PREFER_PRICE"].ToString() + "</Costs>";  //COSTS
                                xmlstr = xmlstr + "<Charges>" + dt_sfxm.Rows[0]["PREFER_PRICE"].ToString() + "</Charges>"; //CHARGES
                            }
                            else
                            {
                                xmlstr = xmlstr + "<Costs>" + dt_sfxm.Rows[0]["PRICE"].ToString() + "</Costs>";  //COSTS
                                xmlstr = xmlstr + "<Charges>" + dt_sfxm.Rows[0]["PRICE"].ToString() + "</Charges>"; //CHARGES
                            }
                        ////xmlstr = xmlstr + "<Costs>" + dt_sfxm.Rows[0]["Price"].ToString() + "</Costs>";  //COSTS
                        ////xmlstr = xmlstr + "<Charges>" + dt_sfxm.Rows[0]["PREFER_PRICE"].ToString() + "</Charges>"; //CHARGES
                        xmlstr = xmlstr + "<BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                        xmlstr = xmlstr + "<OperatorNo>" + sfgh + "</OperatorNo>";
                        xmlstr = xmlstr + "<DoctorName>" + sfry + "</DoctorName>";
                        xmlstr = xmlstr + "<DhcPbd>" + "" + "</DhcPbd>"; //Dhc_Pbd
                        xmlstr = xmlstr + "<DHCCode>" + dt_sfxm.Rows[0]["KEYCOLNO"].ToString() + "</DHCCode>";
                        xmlstr = xmlstr + "</InpBilldetail>";
                    }
                    catch (Exception xml_e)
                    {
                        MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                        return;
                    }
                }
            }

            xmlstr = xmlstr + "</InpBilldetailList></Request>";

          //  MessageBox.Show(xmlstr);
            string aa = xyfy.DhcService("InpBilldetail", xmlstr);
            try
            {
                XmlDataDocument xd = new XmlDataDocument();
                xd.LoadXml(aa);
                XmlNode xn = xd.SelectSingleNode("/Response");
                if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                {
                    MessageBox.Show("סԺ����ȷ��ʧ�ܣ�ԭ��" + xn.FirstChild["ResultContent"].InnerText.ToString());
                }
                else
                {
                    Execute_sql("update  Examapply set jfbj='1' where CheckFlow='" + sqxh + "'");
                    MessageBox.Show("סԺ����ȷ����ɣ�" + xn.FirstChild["ResultContent"].InnerText.ToString());
                    button1.Visible = true;
                    button2.Visible = false;
                }


            }
            catch (Exception rtnee)
            {
                MessageBox.Show("סԺ����ȷ��ʧ��,����XML����,�׳��쳣��" + rtnee.ToString());

            }
        }

        /////�˷ѣ������סԺ��
        private void tf()
        {
            //סԺ�˷�
            // ���뵥��Ϣ��sql��
            try
            {
                string exam_item_no = dt_sqd.Rows[0]["ExamItemNo"].ToString();
                string[] item_no = exam_item_no.Split('^');
                if (dt_sqd.Rows[0]["InOrOut"].ToString() == "סԺ")
                {
                    string   EXAM_ITEM_NO="0";
                    try
                    {
                        for (int x = 0; x < dataGridView1.Rows.Count; x++)
                        {

                            if (int.Parse(EXAM_ITEM_NO)<= int.Parse(dataGridView1.Rows[x].Cells["EXAM_ITEM_NO"].Value.ToString().Trim()))
                                EXAM_ITEM_NO = (int.Parse(dataGridView1.Rows[x].Cells["EXAM_ITEM_NO"].Value.ToString())+1).ToString();
                        }
                    }
                    catch
                    {
                       EXAM_ITEM_NO=item_no[item_no.Length - 1].ToString();
                    }

                    DataTable dt_sfxm = select_orcl("select KEYCOLNO from current_price_list  where ITEM_CODE='" + dataGridView1.CurrentRow.Cells["ITEM_CODE"].Value.ToString().Trim() + "'   and item_spec='" + dataGridView1.CurrentRow.Cells["ITEM_SPEC"].Value.ToString().Trim() + "' and   units='" + dataGridView1.CurrentRow.Cells["UNITS"].Value.ToString().Trim() + "' ", "��ȡ�շ���Ŀ2");


                  
                    if (EXAM_ITEM_NO == "0")
                        EXAM_ITEM_NO = "1";
                    string xmlstr = "<?xml version='1.0' encoding='UTF-8' ?><Request><InpBilldetailList>";
                    try
                    {
                        xmlstr = xmlstr + "<InpBilldetail>";
                        xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                        xmlstr = xmlstr + "<ExamItemNo>" + EXAM_ITEM_NO + "</ExamItemNo>";
                        xmlstr = xmlstr + " <ChargeItemNo>" + "" + "</ChargeItemNo> ";
                        xmlstr = xmlstr + " <PatientId>" + dt_sqd.Rows[0]["PatientId"].ToString() + "</PatientId>";
                        xmlstr = xmlstr + "<VisitId>" + dt_sqd.Rows[0]["VisitId"].ToString() + "</VisitId>";
                        xmlstr = xmlstr + "<ItemNo>" + "" + "</ItemNo>";
                        xmlstr = xmlstr + "<ItemClass>" + dataGridView1.CurrentRow.Cells["ITEM_CLASS"].Value.ToString().Trim() + "</ItemClass>";
                        xmlstr = xmlstr + " <ItemName>" + dataGridView1.CurrentRow.Cells["ITEM_NAME"].Value.ToString().Trim() + "</ItemName>";
                        xmlstr = xmlstr + " <ItemCode>" + dataGridView1.CurrentRow.Cells["ITEM_CODE"].Value.ToString().Trim() + "</ItemCode>";
                        xmlstr = xmlstr + "<ItemSpec>" + dataGridView1.CurrentRow.Cells["ITEM_SPEC"].Value.ToString().Trim() + "</ItemSpec>";
                        xmlstr = xmlstr + "<Amount>" + "-" + dataGridView1.CurrentRow.Cells["Amount"].Value.ToString().Trim() + "</Amount>";  //����
                        xmlstr = xmlstr + "<Units>" + dataGridView1.CurrentRow.Cells["UNITS"].Value.ToString().Trim() + "</Units>";
                        xmlstr = xmlstr + "<OrderedBy>" + "90" + "</OrderedBy>";
                        xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";
                        xmlstr = xmlstr + "<Costs>" + "-" + dataGridView1.CurrentRow.Cells["COSTS"].Value.ToString().Trim() + "</Costs>";  //COSTS
                        xmlstr = xmlstr + "<Charges>" + "-" + dataGridView1.CurrentRow.Cells["Charges"].Value.ToString().Trim() + "</Charges>"; //CHARGES
                        xmlstr = xmlstr + "<BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                        xmlstr = xmlstr + "<OperatorNo>" + sfgh + "</OperatorNo>";
                        xmlstr = xmlstr + "<DoctorName>" + sfry + "</DoctorName>";
                        xmlstr = xmlstr + "<DhcPbd>" + "" + "</DhcPbd>"; //Dhc_Pbd
                        xmlstr = xmlstr + "<DHCCode>" + dt_sfxm.Rows[0]["KEYCOLNO"].ToString() + "</DHCCode>";
                        xmlstr = xmlstr + "</InpBilldetail>";


                    }
                    catch (Exception xml_e)
                    {
                        MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                        return;
                    }

                    xmlstr = xmlstr + "</InpBilldetailList></Request>";
                
                    string aa = xyfy.DhcService("InpBilldetail", xmlstr);
                   
                    try
                    {
                        XmlDataDocument xd = new XmlDataDocument();
                        xd.LoadXml(aa);
                        XmlNode xn = xd.SelectSingleNode("/Response");
                        if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                        { 
                            MessageBox.Show("סԺ�˷�ʧ�ܣ�ԭ��" + xn.FirstChild["ResultContent"].InnerText.ToString());
                            log.WriteMyLog("סԺ�˷�ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                            return;
                        }
                        else
                        {
                            MessageBox.Show("�˷ѳɹ���");
                        }

                    }
                    catch (Exception rtnee)
                    { MessageBox.Show("סԺ�˷�ʧ��,����XML����,�׳��쳣");
                        log.WriteMyLog("סԺ�˷�ʧ��,����XML����,�׳��쳣��" + rtnee.ToString());
                        return;

                    }
                }
                else
                { 
                    //////////////////////////////////////////////////////////////////////
                    //�����˷ѣ���
                    ///////////////////////////////////////////////////////////////////////
                    if (dataGridView1.CurrentRow.Cells["STATUS"].Value.ToString().Trim() == "F")
                    {
                        MessageBox.Show("����Ŀ�����˷ѱ�ǣ����ܸ���");
                        return;
                    }
                        try
                        {

                            string orclstr2 = " update exam.exam_bill_items  set STATUS='F',Operator_No='" + sfgh + "',DoctorName='" + sfry + "',Performed_By='90',Billing_Date_Time=to_date('" + DateTime.Now.ToString() + "','yyyy-mm-dd hh24:mi:ss')  where  exam_no='" + dataGridView1.CurrentRow.Cells["exam_no"].Value.ToString().Trim() + "' and patient_id='" + dataGridView1.CurrentRow.Cells["patient_id"].Value.ToString().Trim() + "' and exam_item_no='" + dataGridView1.CurrentRow.Cells["exam_item_no"].Value.ToString().Trim() + "' and charge_item_no='" + dataGridView1.CurrentRow.Cells["charge_item_no"].Value.ToString().Trim() + "'  and ORDERED_BY='90'";
                           int x = Execute_orcl(orclstr2, "�˷���Ϣ");

                          
                            if (x > 0)
                            {
                                // MessageBox.Show("select * from  outp_bill_items where  Rcpt_no='"+dataGridView1.CurrentRow.Cells["Rcpt_no"].Value.ToString().Trim()+"'and ITEM_class='"+dataGridView1.CurrentRow.Cells["ITEM_class"].Value.ToString().Trim()+"'and PERFORMED_BY='"+dataGridView1.CurrentRow.Cells["PERFORMED_BY"].Value.ToString().Trim()+"'and costs='"+dataGridView1.CurrentRow.Cells["costs"].Value.ToString().Trim()+"'and ADVICE_ID='"+dataGridView1.CurrentRow.Cells["exam_no"].Value.ToString().Trim()+"_"+dataGridView1.CurrentRow.Cells["exam_item_no"].Value.ToString().Trim()+"_"+dataGridView1.CurrentRow.Cells["charge_item_no"].Value.ToString().Trim()+"'");
                                DataTable dta = select_orcl("select * from  outp_bill_items where  Rcpt_no='" + dataGridView1.CurrentRow.Cells["Rcpt_no"].Value.ToString().Trim() + "'and ITEM_class='" + dataGridView1.CurrentRow.Cells["ITEM_class"].Value.ToString().Trim() + "' and PERFORMED_BY='" + dataGridView1.CurrentRow.Cells["PERFORMED_BY"].Value.ToString().Trim() + "' and costs='" + dataGridView1.CurrentRow.Cells["costs"].Value.ToString().Trim() + "' and ADVICE_ID='" + dataGridView1.CurrentRow.Cells["exam_no"].Value.ToString().Trim() + "_" + dataGridView1.CurrentRow.Cells["exam_item_no"].Value.ToString().Trim() + "_" + dataGridView1.CurrentRow.Cells["charge_item_no"].Value.ToString().Trim() + "' ", "��ѯoutp_bill_items");
                               
                                if(dta.Rows.Count>0)
                                {

                                    string outp_bill_items_str = "update  outp_bill_items set status='0' where status='1' and   Rcpt_no='" + dataGridView1.CurrentRow.Cells["Rcpt_no"].Value.ToString().Trim() + "'and ITEM_class='" + dataGridView1.CurrentRow.Cells["ITEM_class"].Value.ToString().Trim() + "' and PERFORMED_BY='" + dataGridView1.CurrentRow.Cells["PERFORMED_BY"].Value.ToString().Trim() + "' and costs='" + dataGridView1.CurrentRow.Cells["costs"].Value.ToString().Trim() + "'and ADVICE_ID='" + dataGridView1.CurrentRow.Cells["exam_no"].Value.ToString().Trim() + "_" + dataGridView1.CurrentRow.Cells["exam_item_no"].Value.ToString().Trim() + "_" + dataGridView1.CurrentRow.Cells["charge_item_no"].Value.ToString().Trim() + "'";
                                int y = Execute_orcl(outp_bill_items_str, "�˷���Ϣ-�޸�outp_bill_items");
                                  if(y>0)
                                      MessageBox.Show("�˷ѳɹ���");
                                  else
                                      MessageBox.Show("�˷ѳɹ���д���ʧ��!");
                               }
                            }
                            else
                                MessageBox.Show("�˷���Ϣ����ʧ�ܣ������²�����");

                        }
                        catch (Exception insert_ee)
                        {
                            MessageBox.Show("�˷ѣ��׳��쳣��");
                            log.WriteMyLog("�˷��׳��쳣" + insert_ee.ToString());
                            return;
                        }




                   //�����˷�(web)

                  //  string xmlstr = "<?xml version='1.0' encoding='UTF-8' ?><Request><OutBillItemsList>";
                  //  try
                  //  {
                  //      xmlstr = xmlstr + "<OutBillItems>";
                  //      xmlstr = xmlstr + "<CheckFlow>" + sqxh + "</CheckFlow>";
                  //      xmlstr = xmlstr + "<ExamItemNo>" + dataGridView1.CurrentRow.Cells["EXAM_ITEM_NO"].Value.ToString().Trim() + "</ExamItemNo>";
                  //      xmlstr = xmlstr + " <ChargeItemNo>" + dataGridView1.CurrentRow.Cells["CHARGE_ITEM_NO"].Value.ToString().Trim() + "</ChargeItemNo> ";
                    
                  //      xmlstr = xmlstr + " <PatientId>" + dataGridView1.CurrentRow.Cells["PATIENT_ID"].Value.ToString().Trim() + "</PatientId> ";
                      
                  //      xmlstr = xmlstr + " <VisitId>" + dataGridView1.CurrentRow.Cells["VISIT_ID"].Value.ToString().Trim() + "</VisitId>";
                  //      xmlstr = xmlstr + " <ItemClass>" + dataGridView1.CurrentRow.Cells["ITEM_CLASS"].Value.ToString().Trim() + "</ItemClass>";
                  //      xmlstr = xmlstr + " <ItemName>" + dataGridView1.CurrentRow.Cells["Item_Name"].Value.ToString().Trim() + "</ItemName>";
                  //      xmlstr = xmlstr + " <ItemCode>" + dataGridView1.CurrentRow.Cells["Item_Code"].Value.ToString().Trim() + "</ItemCode>";
                  //      xmlstr = xmlstr + " <ItemSpec>" + dataGridView1.CurrentRow.Cells["Item_Spec"].Value.ToString().Trim() + "</ItemSpec>";
                  //      xmlstr = xmlstr + " <Amount>" + dataGridView1.CurrentRow.Cells["Amount"].Value.ToString().Trim() + "</Amount>";
                  //      xmlstr = xmlstr + " <Units>" + dataGridView1.CurrentRow.Cells["Units"].Value.ToString().Trim() + "</Units>";
                  //      xmlstr = xmlstr + " <OrderedBy>" + "90" + "</OrderedBy>";
                  //      xmlstr = xmlstr + " <PerformedBy>" + "90" + "</PerformedBy>";
                  //      xmlstr = xmlstr + " <Costs>" + dataGridView1.CurrentRow.Cells["Costs"].Value.ToString().Trim() + "</Costs>";
                        
                  //      xmlstr = xmlstr + " <Charges>" + dataGridView1.CurrentRow.Cells["Charges"].Value.ToString().Trim() + "</Charges>";
                  //      xmlstr = xmlstr + " <BillingDateTime>" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</BillingDateTime>";
                  //      xmlstr = xmlstr + " <OperatorNo>" + "LFJ1" + "</OperatorNo>";
                  //      xmlstr = xmlstr + " <VerifiedIndicator>" + dataGridView1.CurrentRow.Cells["VERIFIED_INDICATOR"].Value.ToString().Trim() + "</VerifiedIndicator>";
                  //      xmlstr = xmlstr + " <DoctorName>" + "LFJ1" + "</DoctorName>"; 
                  //      xmlstr = xmlstr + " <BillingAttr>" + "1" + "</BillingAttr>";
                  //      xmlstr = xmlstr + " <Status>" + "F" + "</Status>";
                  //      xmlstr = xmlstr + " <DhcKey>" + "" + "</DhcKey>";
                  //      xmlstr = xmlstr + "</OutBillItems>";
                  //}
                  //  catch (Exception xml_e)
                  //  {
                        
                  //      MessageBox.Show("xml�����쳣��" + xml_e.ToString());
                  //      return;
                  //  }
                  //  MessageBox.Show(xmlstr);
                  //  xmlstr = xmlstr + "</OutBillItemsList></Request>";
                   
                  //  string aa = "";
                  //  try
                  //  {
                  //     aa= xyfy.DhcService("OutBillItems", xmlstr);
                       

                  //  }
                  //  catch
                  //  {
                  //      MessageBox.Show("ƽ̨�����쳣");
                  //  }    
                  //  try
                  //  {
                  //      if (aa == "")
                  //      {
                  //          MessageBox.Show("�����˷�ʧ�ܣ����ؿ�");
                  //          return;
                  //      }
                  //      XmlDataDocument xd = new XmlDataDocument();
                  //      xd.LoadXml(aa);
                  //      XmlNode xn = xd.SelectSingleNode("/Response");
                  //      if (xn.FirstChild["Returncode"].InnerText.ToString() == "-1")
                  //      { //if (msg == "1")
                  //          MessageBox.Show("�����˷�ʧ�ܣ�ԭ��" + xn.FirstChild["ResultContent"].InnerText.ToString());
                  //          log.WriteMyLog("�����˷�ʧ�ܣ�" + xn.FirstChild["ResultContent"].InnerText.ToString() + "@" + xn.FirstChild["Returncode"].InnerText.ToString());
                  //          return;
                  //      }
                  //      else
                  //      {
                  //          MessageBox.Show("�˷ѳɹ���");
                  //      }


                  //  }
                  //  catch (Exception rtnee)
                  //  {
                  //      // if (msg == "1")

                  //      MessageBox.Show("�����˷�ʧ��,����XML����,�׳��쳣");
                  //      log.WriteMyLog("�����˷�ʧ��,����XML����,�׳��쳣��" + rtnee.ToString());
                  //      return;

                  //  }
                
                }
               
            }
            catch (Exception ee)
            {
                //if (msg == "1")
                MessageBox.Show("�˷�ʧ��,�׳��쳣");
                log.WriteMyLog("�˷�ʧ��,�׳��쳣��" + ee.ToString());
                return;
            }
     
    

        }
    
        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView2.Rows.Count > 0)
            //{
            //    sf(dataGridView2.CurrentRow.Cells["�շѴ���"].Value.ToString().Trim(), "1", dataGridView2.CurrentRow.Cells["�շ���Ŀ����"].Value.ToString(),"","0");
            //    loadsfmx(F_blh);
            //}
            
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           textBox1.Text = dataGridView2.CurrentRow.Cells["ITEM_NAME"].Value.ToString();
            textBox2.Text = dataGridView2.CurrentRow.Cells["ITEM_CODE"].Value.ToString();
            laldw.Text = dataGridView2.CurrentRow.Cells["UNITS"].Value.ToString();
            txt_ITEM_SPEC.Text = dataGridView2.CurrentRow.Cells["ITEM_SPEC"].Value.ToString();
            txt_ItemClass.Text = dataGridView2.CurrentRow.Cells["ITEM_CLASS"].Value.ToString();
            if (Identitys == "���ﲡ��")
            {
                lalCosts.Text = dataGridView2.CurrentRow.Cells["FOREIGNER_PRICE"].Value.ToString();
                lal_prefer_price.Text = dataGridView2.CurrentRow.Cells["FOREIGNER_PRICE"].Value.ToString();
            }
            else
                if (Identitys == "��Ժְ��")
                {
                    lalCosts.Text = dataGridView2.CurrentRow.Cells["PREFER_PRICE"].Value.ToString();
                    lal_prefer_price.Text = dataGridView2.CurrentRow.Cells["PREFER_PRICE"].Value.ToString();
                }
                else
                {
                      lalCosts.Text = dataGridView2.CurrentRow.Cells["PRICE"].Value.ToString();
                      lal_prefer_price.Text = dataGridView2.CurrentRow.Cells["PRICE"].Value.ToString();
                }
           // lalCosts.Text = dataGridView2.CurrentRow.Cells["PRICE"].Value.ToString();
           // lal_prefer_price.Text = dataGridView2.CurrentRow.Cells["prefer_price"].Value.ToString();
            txtKEYCOLNO.Text = dataGridView2.CurrentRow.Cells["KEYCOLNO"].Value.ToString();
            numericUpDown1.Value = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (laldw.Text == "" && txt_ITEM_SPEC.Text == "" && txt_ItemClass.Text == "" && lalCosts.Text == "")
            {
                MessageBox.Show("��ѡ���շ���Ŀ");

            }
            else
            {
                if (MessageBox.Show("�շ���Ŀ��" + textBox1.Text + "\n" + "������" + numericUpDown1.Value.ToString(), "�շ���Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    int x = 0;
                    try
                    {
                        x = int.Parse(numericUpDown1.Value.ToString());
                        if (x == 0)
                        {
                            MessageBox.Show("��������Ϊ�㣡");
                            return;
                        }
                        if (x < 0)
                        {
                            MessageBox.Show("��������С���㣡");
                            return;
                        }

                    }
                    catch
                    {
                        return;
                      
                    }
                    if (InOrOut == "����")
                    {
                        mzbf();
                     // mzsf(textBox2.Text, numericUpDown1.Value.ToString(), textBox1.Text, "", "0");
                    }
                    else
                    {
                      //  zybf(textBox2.Text, numericUpDown1.Value.ToString(), textBox1.Text, "", "0");
                     zybf();
                    }
                  
                    loadsfmx();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    laldw.Text = "";
                    txt_ITEM_SPEC.Text = "";
                    txt_ItemClass.Text = "";
                    lalCosts.Text = "";
                    numericUpDown1.Value = 1;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count<1)
            {
                return;
            }
            try
            {
                if (dt_jcxx.Rows[0]["F_BGZT"].ToString().Trim() == "�����")
                {
                    MessageBox.Show("��������ˣ������˷�");
                    return;
                }
            }
            catch
            {
            }
            //if (dataGridView1.CurrentRow.Cells["STATUS"].Value.ToString().Trim() == "A" && dataGridView1.CurrentRow.Cells["BILLING_ATTR"].Value.ToString().Trim() == "1")
          
            if (dataGridView1.CurrentRow.Cells["COSTS"].Value.ToString().Trim() == "")
            {
                MessageBox.Show("����Ŀ���Ϊ�գ������˷�");
                return;
            }
        
            if (dataGridView1.CurrentRow.Cells["Amount"].Value.ToString().Trim() == "")
            {
                MessageBox.Show("����Ŀ���Ϊ�գ������˷�");
                return;
            }
         
            if (dataGridView1.Rows.Count <= 1)
            {
                MessageBox.Show("�շ���Ŀ������Ҫ����һ���������˷�");
                return;
            }
        
                if (dataGridView1.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Amount"].Value.ToString().Trim()) < 0)
                    {
                        MessageBox.Show("�˷������˷ѷ��ã������˷ѣ�");
                        return;
                    }
                 
                  
                    //�˷ѵļ�¼��������
                    if (Convert.ToDecimal(dataGridView1.CurrentRow.Cells["COSTS"].Value.ToString().Trim()) < 0)
                    {
                        MessageBox.Show("�������˷Ѽ�¼��������");
                        return;
                    }
                  
                 
                    //      ----�˷ѣ�----
                    //  ------------------------------------------------
                    double COSTS=0;//�ܽ��
                    double Amount=0;//������
                   
                    for(int x=0;x<dataGridView1.Rows.Count;x++)
                    {
                        try
                        {
                            if (dataGridView1.CurrentRow.Cells["ITEM_CODE"].Value.ToString().Trim() == dataGridView1.Rows[x].Cells["ITEM_CODE"].Value.ToString().Trim())
                            {
                                if (dataGridView1.CurrentRow.Cells["ITEM_SPEC"].Value.ToString().Trim() == dataGridView1.Rows[x].Cells["ITEM_SPEC"].Value.ToString().Trim())
                                {
                                    if (dataGridView1.CurrentRow.Cells["units"].Value.ToString().Trim() == dataGridView1.Rows[x].Cells["units"].Value.ToString().Trim())
                                    {
                                        if (dataGridView1.CurrentRow.Cells["COSTS"].Value.ToString().Trim() == "" || dataGridView1.Rows[x].Cells["Amount"].Value.ToString().Trim() == "")
                                            continue;

                                        COSTS = COSTS + double.Parse(dataGridView1.Rows[x].Cells["COSTS"].Value.ToString());
                                        Amount = Amount + double.Parse(dataGridView1.Rows[x].Cells["Amount"].Value.ToString());
                                    }
                                }
                            }
                        }
                        catch
                        {
                             continue;
                        }

                    }
                  
                    if (COSTS <= 0)
                    {
                        MessageBox.Show("��Ŀ��"+dataGridView1.CurrentRow.Cells["ITEM_name"].Value.ToString().Trim()+"\n����Ŀ�ܽ�����0�������˷�");
                        return;
                    }
                    if (Amount <= 0)
                    {
                        MessageBox.Show("��Ŀ��" + dataGridView1.CurrentRow.Cells["ITEM_name"].Value.ToString().Trim() + "\n����Ŀ����������0�������˷�");
                        return;
                    }
                  
                    if (MessageBox.Show("�˷���Ŀ��" + dataGridView1.CurrentRow.Cells["item_name"].Value.ToString().Trim() + "\n", "�˷���Ϣ��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        tf();
                       
                    }
                }
                else
                {
                    MessageBox.Show("û�з��ÿ��ˣ�");
                }
            
                loadsfmx();
            }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           

            
            laldw.Text = "";
            txt_ITEM_SPEC.Text = "";
            txt_ItemClass.Text = "";
            lalCosts.Text = "";
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           

            laldw.Text = "";
            txt_ITEM_SPEC.Text = "";
            txt_ItemClass.Text = "";
            lalCosts.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadsfmx();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            DataView dv = ds.Tables["xmb2"].DefaultView;
            dv.RowFilter = "  ITEM_NAME like '%" + textBox5.Text.Trim() + "%' ";
            dataGridView2.DataSource = dv;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            DataView dv = ds.Tables["xmb2"].DefaultView;
            dv.RowFilter = "  ITEM_CODE like '%" + textBox6.Text.Trim() + "%' ";
            dataGridView2.DataSource = dv;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (button2.Text == "סԺ����ȷ��")
            {
                if (dt_sqd.Rows[0]["kdxt"].ToString().Trim() == "PIS")
                   nj_zyqrsf();
               else
                   zyqrsf();
               
            }
            if (button2.Text == "�����Զ�����")
            {
                if (dt_sqd.Rows[0]["kdxt"].ToString().Trim() == "PIS")
                    nj_mzzdhj();
                else
                    mzzdhj();
            }

            loadsfmx();
        }
       

        private void frm_sfjk_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            if (sqxh.Trim() == "")
            {
                F_sfje =0;
                
            }
            else
            {
            F_sfje=decimal.Parse(label7.Text.Trim().Replace('Ԫ',' ').Trim());
            string zdhj = "update  Examapply  set sfje='" + label7.Text.Trim().Replace('Ԫ', ' ').Trim() + "' where CheckFlow='" + sqxh + "'";
            try
            {
                Execute_sql(zdhj);
            }
            catch
            {
              
            }
            }

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //F_sfje = decimal.Parse(label7.Text.Trim().Replace('Ԫ', ' ').Trim());
            //string zdhj = "update  Examapply  set sfje='" + label7.Text.Trim().Replace('Ԫ', ' ').Trim() + "' where CheckFlow='" + sqxh + "'";
            //try
            //{
            //    Execute_sql(zdhj);
            //}
            //catch
            //{
            //    return ;
            //}
           // frm_sfjk_FormClosing(sender,e);
            this.Close();
        }




        public int Execute_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = con = new SqlConnection(constr);
            SqlCommand sqlcom = null;
            try
            {
                sqlcom = new SqlCommand(sqlstr, con);
                con.Open();
                int x = sqlcom.ExecuteNonQuery();
                con.Close();
                sqlcom.Clone();

                return x;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("ִ��SQL����쳣��" + sqlstr + ",\r\n �쳣ԭ��" + ee.ToString());
                con.Close();
                sqlcom.Clone();
                return -1;
            }
        }
        public DataTable select_sql(string sqlstr)
        {
            string constr = "Server=" + Server + ";Database=" + DataBase + ";User Id=" + UserID + ";Password=" + PassWord + ";";

            SqlConnection con = new SqlConnection(constr);
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter sqlda = new SqlDataAdapter(sqlstr, con);
                con.Open();
                sqlda.Fill(dt);
                con.Close();

                return dt;
            }
            catch (Exception ee)
            {
                log.WriteMyLog("ִ��SQL��ѯ����쳣��" + sqlstr + ",\r\n �쳣ԭ��" + ee.ToString());
                con.Close();
                return dt;
            }

        }
        public DataTable select_orcl(string orcl_strsql, string sm)
        {
            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbDataAdapter orcl_dap = new OleDbDataAdapter(orcl_strsql, orcl_con);
            DataTable dt_bill_items = new DataTable();
            try
            {
                orcl_con.Open();
                orcl_dap.Fill(dt_bill_items);
                orcl_con.Close();
                return dt_bill_items;
            }
            catch (Exception orcl_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("ִ��ORACLE��ѯ����쳣��" + orcl_strsql + ",\r\n �쳣ԭ��" + orcl_ee.ToString());
                return dt_bill_items;

            }

        }
        public int Execute_orcl(string orcl_strsql, string sm)
        {

            if (odbcsql.Trim() != "")
                orcon_str = odbcsql;

            OleDbConnection orcl_con = new OleDbConnection(orcon_str);
            OleDbCommand ocdc = new OleDbCommand(orcl_strsql, orcl_con);
            int x = 0;
            try
            {
                orcl_con.Open();
                x = ocdc.ExecuteNonQuery();
                orcl_con.Close();
                ocdc.Dispose();
            }
            catch (Exception insert_ee)
            {
                orcl_con.Close();
                log.WriteMyLog("ִ��ORACLE����쳣��" + orcl_strsql + ",\r\n �쳣ԭ��" + insert_ee.ToString());
                return 0;
            }
            return x;

        }
    }
}