using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PathnetCAzgq
{
    public partial class frm_gdca_pwd : Form
    {
        public frm_gdca_pwd()
        {
            InitializeComponent();
        }

        private void frm_gdca_pwd_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
                MessageBox.Show("ÃÜÂë²»ÄÜÎª¿Õ£¡");
            else
            {
               pwd=textBox1.Text.Trim();
               this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }
        string pwd = "";
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        public String key_pwd
        {
            get { return pwd; }
        }
    }
}