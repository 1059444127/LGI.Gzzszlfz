using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using LGHISJKZGQ;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;

namespace LGHISJKZGQ
{
    class NJSGCRMYY
    {

        private static LGHISJKZGQ.IniFiles f = new LGHISJKZGQ.IniFiles(Application.StartupPath + "\\sz.ini");

        /// <summary>
        ///  // @brlb ut_bz,         --�������   0���� 1סԺ 3���      
        //@codetype ut_bz,      --��������   1סԺ/�����ָ�����ţ�2���ţ� 3PatientID       
        //   --��Ӧ��his�е� patid(����),patid(סԺ)�� 4CureNo��Ӧ��his,9��Ʊ�ţ�����ʹ�ã�      
        //   --�е�ghsjh (����),��syxh(סԺ)      
        //   @code  varchar(20)    --���룬���ĺ��壬������codetypeָ��      
        //exec usp_yjjk_getbrxx  '1','4','285606'



        //����:
        //his���ݿ�  192.168.42.107\this4  
        //�û���sa/sql2k
        //�⣺this4_0720
        //�洢���̣�usp_yjjk_getbrxx


        // @brlb ut_bz,         --�������   0���� 1סԺ 3���      
        // @codetype ut_bz,      --��������   1סԺ/�����ָ�����ţ�2���ţ�       
        //    --��Ӧ��his�е� patid(����),patid(סԺ)�� 9��Ʊ�ţ�����ʹ�ã�      
        //    --�е�ghsjh (����),��syxh(סԺ)      
        //    @code  varchar(20)    --���룬���ĺ��壬������codetypeָ��    


        //patientID   patientID
        //HospNo     ��Ա��  ������
        //patName  ��������
        //Sex   �Ա�  1 �У�2Ů
        //age+ageUnit  ����
        //WardOrReg  �������   0���� 1סԺ 3���      
        //ChargeType  �ѱ� 
        //CureNo   סԺ��/�����
        //CardNo  ����
        //applyDept ���� ����  
        //select  name from YY_KSBMK  where id ='409'
        //ward   ��������  ZY_BQDMK
        //bedNo      ����
        //applyDoctor  ҽ������  YY_ZGBMK
        //select  name from YY_ZGBMK  where id ='5358'
        //clincDesc  �ٴ���ϡ�
        //IDNum    ��ݺ�
        //Phone     �绰
        //Address    ��ַ
        //Zip  
        //Career    ְҵ
        //Nation   ����
        //ToDoc     
        //sendNo   
        //Syxh    ��ҳ���
        //bqmc        ����
        //yexh    
        //DeptName    ��������
        //clinicDesc  �ٴ����
        /// </summary>
        /// <param name="Sslbx"></param>
        /// <param name="Ssbz"></param>
        /// <param name="Debug"></param>
        /// <returns></returns>
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            if (Sslbx != "")
            {
                string odbcsql = f.ReadString(Sslbx, "odbcsql", "Data Source=192.168.42.107\\this4;Initial Catalog=THIS4_0720;User Id=sa;Password=sql2k;");
                string brlb = "";
                string codetype = "";

                if (Sslbx == "���ﲡ����")
                {
                    brlb = "0";
                    codetype = "1";
                }
                if (Sslbx == "�����")
                {
                    brlb = "0";
                    codetype = "4";
                }
                if (Sslbx == "����")
                {
                    brlb = "0";
                    codetype = "2";
                }
                if (Sslbx == "��Ʊ��")
                {
                    brlb = "0";
                    codetype = "9";
                }
              
                if (Sslbx == "סԺ������")
                {
                    brlb = "1";
                    codetype = "1";
                }
                if (Sslbx == "סԺ��")
                {
                    brlb = "1";
                    codetype = "4";
                }
                if (Sslbx == "����")
                {
                    brlb = "3";
                    codetype = "1";
                }
                if (Sslbx == "��쿨��")
                {
                    brlb = "3";
                    codetype = "2";
                }
                if (Sslbx == "��췢Ʊ��")
                {
                    brlb = "3";
                    codetype = "9";
                }

                if (brlb == "")
                {
                    MessageBox.Show("δ���ô�ʶ������" + Sslbx); return "0";
                }

                DataTable dt = new DataTable();
                string exec = "";

                SqlDB_ZGQ sql = new SqlDB_ZGQ();
                dt = sql.Sql_DataAdapter(odbcsql, "exec usp_yjjk_getbrxx  '" + brlb + "','" + codetype + "','" + Ssbz.Trim() + "'", ref exec);

                if (exec != "")
                {
                    MessageBox.Show(exec);
                    return "0";
                }
                
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("δ�ܲ�ѯ����Ӧ�����ݼ�¼����ȷ��" + Sslbx + "�Ƿ���ȷ");
                    return "0";
                }
                //�����ݼ�ʱ�Է�����  select "F","û��ָ����������"
                try
                {
                    MessageBox.Show(dt.Rows[0]["Column2"].ToString().Trim());
                    return "0";
                }
                catch
                {
                }

                int count = 0;

                    DataColumn dc0 = new DataColumn("���");
                   dt.Columns.Add(dc0);

                   DataColumn dc_brlb = new DataColumn("�������");
                   dt.Columns.Add(dc_brlb);

                    for (int x = 0; x <dt.Rows.Count; x++)
                    {
                        dt.Rows[x][dt.Columns.Count - 2] = x;
                        //ͨ��ҽ�������ѯҽ������
                       try
                       {
                           DataTable dt_ys = new DataTable();
                           dt_ys = sql.Sql_DataAdapter(odbcsql, "select top 1 name from YY_ZGBMK  where id ='" + dt.Rows[x]["ApplyDoctor"].ToString().Trim() + "'", ref exec);
                           if (dt_ys.Rows.Count > 0)
                               dt.Rows[x]["ApplyDoctor"] = dt_ys.Rows[0]["name"].ToString().Trim();
                       }
                       catch
                       {
                       }
                       //DeptNameΪ�յ�����£�ͨ�����Ҵ�������ѯ��������
                       if (dt.Rows[x]["DeptName"].ToString().Trim() == null || dt.Rows[x]["DeptName"].ToString().Trim() == "")
                       {
                           try
                           {
                               DataTable dt_ks = new DataTable();
                               dt_ks = sql.Sql_DataAdapter(odbcsql, "select top 1 name from YY_KSBMK  where id ='" + dt.Rows[x]["ApplyDept"].ToString().Trim() + "'", ref exec);
                               if (dt_ks.Rows.Count > 0)
                                   dt.Rows[x]["DeptName"] = dt_ks.Rows[0]["name"].ToString().Trim();
                           }
                           catch
                           {
                           }
                       }

                       // --�������   0���� 1סԺ 3���      
                       dt.Rows[0]["�������"] = dt.Rows[0]["WardOrReg"].ToString().Trim();
                        if (dt.Rows[0]["WardOrReg"].ToString().Trim() == "1")
                           dt.Rows[0]["�������"] = "סԺ";
                         if (dt.Rows[count]["WardOrReg"].ToString().Trim() == "3")
                             dt.Rows[0]["�������"] = "���";
                         if (dt.Rows[count]["WardOrReg"].ToString().Trim() == "0")
                             dt.Rows[0]["�������"] = "����";
                    }


                    // ����������ʾѡ����
                    if (dt.Rows.Count >= 1)
                    {
                        //dataGridView1��ʾ��ʽ
                        string xsys = f.ReadString(Sslbx, "xsys", "1");
                        //��ʾ�����ֶ�
                        string Columns = f.ReadString(Sslbx, "Columns", "HospNo,�������,PatName,Age,CureNo,CardNo,bqmc,BedNo,DeptName,ApplyDoctor,ClinicDesc");
                        //��ʾ���еı���
                        string ColumnsName = f.ReadString(Sslbx, "ColumnsName", "������,���,����,����,סԺ��/�����,����,����,����,�������,����ҽ��,�ٴ����");


                        //���������
                    if (Columns.Trim() != "")
                        Columns = "���,"+Columns;
                    if (ColumnsName.Trim() != "")
                        ColumnsName = "���,"+ColumnsName;



                   //��ʾ����
                    FRM_YZ_SELECT yc = new FRM_YZ_SELECT(dt, Columns, ColumnsName, xsys);
                    yc.ShowDialog();
                    string rtn2 = yc.F_XH;

                    if (rtn2.Trim() == "")
                    {
                        MessageBox.Show("δѡ�����ݣ�");
                        return "0";
                    }
                    try
                    {
                        count = int.Parse(rtn2);
                    }
                    catch
                    {
                        MessageBox.Show("������ѡ��");
                        return "0";
                    }
                }
               

                try
                {

                    string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                    xml = xml + "<LOGENE>";
                    xml = xml + "<row ";

                    xml = xml + "���˱��=" + (char)34 + dt.Rows[count]["PatientID"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "����ID=" + (char)34 + dt.Rows[count]["HospNo"].ToString().Trim() + "^" + dt.Rows[count]["CardNo"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                        if (dt.Rows[count]["WardOrReg"].ToString().Trim() == "1")
                        {
                            xml = xml + "�����=" + (char)34 +"" + (char)34 + " ";
                            xml = xml + "סԺ��=" + (char)34 + dt.Rows[count]["CureNo"].ToString().Trim() + (char)34 + " ";
                        }
                        else
                        {
                            xml = xml + "�����=" + (char)34 + dt.Rows[count]["CureNo"].ToString().Trim() + (char)34 + " ";
                            xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                        }

                        xml = xml + "����=" + (char)34 + dt.Rows[count]["PatName"].ToString().Trim() + (char)34 + " ";
                        string xb = dt.Rows[count]["Sex"].ToString().Trim();
                        if (xb == "2")
                            xb = "Ů";
                        if (xb == "1")
                            xb = "��";
                        xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";

                        xml = xml + "����=" + (char)34 + dt.Rows[count]["Age"].ToString().Trim() + dt.Rows[count]["AgeUnit"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "��ַ=" + (char)34 + dt.Rows[count]["Address"].ToString().Trim() + (char)34 + "   ";
                        xml = xml + "�绰=" + (char)34 + dt.Rows[count]["Phone"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[count]["bqmc"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[count]["BedNo"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "���֤��=" + (char)34 + dt.Rows[count]["IDNum"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "����=" + (char)34 + dt.Rows[count]["Nation"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "ְҵ=" + (char)34 + dt.Rows[count]["Career"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ����=" + (char)34 + dt.Rows[0]["DeptName"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�ͼ�ҽ��=" + (char)34 + dt.Rows[0]["ApplyDoctor"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                        xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                        xml = xml + "�ѱ�=" + (char)34 + dt.Rows[count]["ChargeType"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "�������=" + (char)34 + dt.Rows[0]["�������"].ToString().Trim() + (char)34 + " ";
                        xml = xml + "/>";
                        xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                        xml = xml + "<�ٴ����><![CDATA[" + dt.Rows[count]["ClinicDesc"].ToString().Trim() + "]]></�ٴ����>";    
                       xml = xml + "</LOGENE>";
                       return xml;
                }
                catch (Exception e)
                {
                    MessageBox.Show("��ȡ��Ϣ�����쳣��" + e.Message);
                    return "0";
                }

            } return "0";
        }
    }
}






