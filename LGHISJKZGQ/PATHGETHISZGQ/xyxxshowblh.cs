using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class xyxxshowblh : Form
    {
        public xyxxshowblh(string blh)
        {
            InitializeComponent();
            comboBox1.DataSource = blh.Split('^');
        }

        private void xyxxshowblh_Load(object sender, EventArgs e)
        {

        }
        string F_blh = "";
        public string  getblh
        {
            get { return F_blh; }
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            F_blh = "";
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F_blh = comboBox1.Text.ToString();
           
            this.Close();
        }
    }
}