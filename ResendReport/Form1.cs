using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Maticsoft.DAL;
using SendPisResult;
using SendPisResult.Models;
using ZgqClassPub;

namespace ResendReport
{
    public partial class Form1 : Form
    {
        private List<T_JCXX> _lstJcxx = null;
        private int countAll=0;
        private int countSuccess=0;
        private int countFailed = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //get sqlWhere
            var blk = txtBlk.Text.Trim();
            var brlb = txtBrlb.Text.Trim();
            var strShsj1 = dteShsj1.Text;
            var strShsj2 = dteShsj2.Text;

            string sqlWhere = " and  (  1=1 ";
            if (string.IsNullOrEmpty(blk) == false)
            {
                sqlWhere += $" and f_blk = '{blk}' ";
            }
            if (string.IsNullOrEmpty(brlb) == false)
            {
                sqlWhere += $" and f_brlb = '{brlb}' ";
            }
            sqlWhere += $" and convert(datetime,f_spare5) >=  convert(datetime,'{strShsj1}')";
            sqlWhere += $" and convert(datetime,f_spare5) <=  convert(datetime,'{strShsj2}')";
            sqlWhere +=" ) ";
            if (txtSqlWhere.Text.Trim()!="")
            {
                sqlWhere = sqlWhere + " " + txtSqlWhere.Text.Trim() + " ";
            }
            _lstJcxx = (new T_JCXX_DAL()).GetBySqlWhere(sqlWhere);

            tJCXXBindingSource.DataSource = _lstJcxx;
            dataGridView1.Refresh();
            dataGridView1.AutoResizeColumns();

            countAll = _lstJcxx.Count;
            RefreshStates();
        }

        private void RefreshStates()
        {
            lblStates.Text = $"共:{countAll}条,已重传{countSuccess}条,重传失败{countFailed}条";
        }

        private void btnResend_Click(object sender, EventArgs e)
        {
            if (_lstJcxx == null) return;

            foreach (var jcxx in _lstJcxx)
            {
                var args = $"{jcxx.F_BLH}^cg^1^old^save";
                var blh = jcxx.F_BLH;
                try
                {
                    PathHISZGQJK.Program.Send(new string[] {args});
                    countSuccess++;
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"[{blh}]重传失败:" + exception + "\r\n" + exception.InnerException);
                    log.WriteMyLog($"[{blh}]重传失败:" + exception + "\r\n" + exception.InnerException);
                    countFailed++;
                }
                finally
                {
                    RefreshStates();
                    Application.DoEvents();
                }
            }

            MessageBox.Show("重传完成!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "结果重传  医院名称:" + "中山大学附属肿瘤医院";
        }
    }
}