using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dbbase;
using PathHISJK;

namespace PathHISZGQJK
{
    /// <summary>
    /// 获取医嘱项目,数据源是t_cyc,支持通过拼音码(自动生成),以及两个备用字段过滤,返回string
    /// </summary>
    public partial class DiseaseSelector : Form
    {
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");

        /// <summary>
        /// 疾病与病理库的关联,如果不传入该字段,则不会以病例库作为查询条件
        /// </summary>
        public string F_BLK { get; set; }

        public CYC_Item SelectedItem { get; set; }

        public List<CYC_Item> CycItems { get; set; } = null; 

        public DiseaseSelector()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 过滤数据,并绑定到listbox
        /// </summary>
        private void FilterItems()
        {
            listBox1.Items.Clear();
            if (CycItems == null)
                return;
            
            var listMached = new List<string>();

            //高精度匹配在前面
            foreach (CYC_Item item in CycItems)
            {
                string by1 = item.ZJC1.Trim();
                string by2 = item.ZJC2.Trim();
                string pym = item.PYM.ToString();
                string itemName = item.CYC_MC.Trim();
                string filterStr = txtFilter.Text;

                if (pym.ToUpper().Equals(filterStr.ToUpper()) ||
                    by1.ToUpper().Equals(filterStr.ToUpper()) ||
                    by2.ToUpper().Equals(filterStr.ToUpper()) ||
                    itemName.ToUpper().Equals(filterStr.ToUpper()))
                {
                    listBox1.Items.Add(item);
                    listMached.Add(itemName);
                }
            }

            //再进行一次模糊匹配,排在后面
            foreach (CYC_Item item in CycItems)
            {
                string by1 = item.ZJC1.Trim();
                string by2 = item.ZJC2.Trim();
                string pym = item.PYM.ToString();
                string itemName = item.CYC_MC.Trim();
                string filterStr = txtFilter.Text;

                if (pym.ToUpper().Contains(filterStr.ToUpper()) ||
                    by1.ToUpper().Contains(filterStr.ToUpper()) ||
                    by2.ToUpper().Contains(filterStr.ToUpper()) ||
                    itemName.ToUpper().Contains(filterStr.ToUpper()))
                {
                    if (listMached.Contains(itemName) == false) //过滤掉已经精确匹配到的s
                    {
                        listBox1.Items.Add(item);
                    }
                }
            }
        }

        private void GetItems()
        {
            //如果外部传入了列表,则不再重新读取
            if (CycItems==null)
            {
                string fl = "f_fzbl_zdjb"; //f_fzbl_yzxm
                string sqlWhereBlk = "";
                if (string.IsNullOrEmpty(F_BLK?.Trim()) == false)
                    sqlWhereBlk = $" and f_zjc1 = '{F_BLK}' ";
                string sql = $" select * from T_CYC where f_cyc_fl='{fl}' {sqlWhereBlk} order by F_CYC_MC ";

                var _dtItems = aa.GetDataTable(sql, "table1");
                CycItems=new List<CYC_Item>();
                foreach (DataRow row in _dtItems.Rows)
                {
                    CycItems.Add(new CYC_Item(row));
                }
            }

            //通过名称获取拼音码
            foreach (CYC_Item cycItem in CycItems)
            {
                cycItem.PYM = GetSpellCode(cycItem.CYC_MC);
            }
        }

        /// <summary>
        /// 上下键移动光标,回车确定,esc取消退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (listBox1.SelectedIndex > 0)
                    listBox1.SelectedIndex -= 1;
            }
            else if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (listBox1.SelectedIndex < listBox1.Items.Count - 1)
                    listBox1.SelectedIndex += 1;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                if (listBox1.SelectedItem != null)
                {
                    SelectedItem = (CYC_Item) listBox1.SelectedItem;
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else if (listBox1.Items.Count > 0) //如果列表中没有选中任何项目,按回车时默认取第一个
                {
                    SelectedItem = (CYC_Item)listBox1.SelectedItem;
                    this.DialogResult = DialogResult.OK;
                    Close();
                }

            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FilterItems();
        }

        private void TestItemSelector_Load(object sender, EventArgs e)
        {

            FilterItems();
            txtFilter.Focus();
        }

        public new DialogResult ShowDialog(IWin32Window form)
        {
            GetItems();
            var r = DialogResult.Cancel;
            if (CycItems?.Count > 0)
                r = base.ShowDialog(form);
            return r;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           var index = listBox1.IndexFromPoint(e.Location);
            if (index != -1)
                textBox1_KeyDown(null,new KeyEventArgs(Keys.Enter));
        }

        #region 获取拼音码

        /// <summary>
        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串
        /// </summary>
        /// <param name="CnStr">汉字字符串</param>
        /// <returns>相对应的汉语拼音首字母串</returns>
        public static string GetSpellCode(string CnStr)
        {
            string strTemp = "";

            int iLen = CnStr.Length;

            int i = 0;

            for (i = 0; i <= iLen - 1; i++)
            {
                strTemp += GetCharSpellCode(CnStr.Substring(i, 1));
            }

            return strTemp;
        }

        /// <summary>
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
        /// </summary>
        /// <param name="CnChar">单个汉字</param>
        /// <returns>单个大写字母</returns>
        private static string GetCharSpellCode(string CnChar)
        {
            long iCnChar;

            byte[] ZW = Encoding.Default.GetBytes(CnChar);

            //如果是字母，则直接返回

            if (ZW.Length == 1)
            {
                return CnChar.ToUpper();
            }

            else
            {
                // get the array of byte from the single char

                int i1 = (short) (ZW[0]);

                int i2 = (short) (ZW[1]);

                iCnChar = i1*256 + i2;
            }

            // iCnChar match the constant
            if (CnChar == "血") return "X";
            else if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }

            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }
            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else

                return ("?");
        }

        #endregion
    }

    public class CYC_Item
    {
        public string CYC_MC { get; set; } = "";
        public string PYM { get; set; } = "";
        public  string FL { get; set; } = "";
        public string ZJC1 { get; set; } = "";
        public string ZJC2 { get; set; } = "";

        public CYC_Item()
        {
            
        }

        public CYC_Item(DataRow drCycItem)
        {
            this.CYC_MC = drCycItem["F_CYC_MC"].ToString();
            this.FL = drCycItem["F_CYC_FL"].ToString();
            this.ZJC1 = drCycItem["F_ZJC1"].ToString();
            this.ZJC2 = drCycItem["F_ZJC2"].ToString();
        }

        public override string ToString()
        {
            return this.CYC_MC;
        }
    }
}