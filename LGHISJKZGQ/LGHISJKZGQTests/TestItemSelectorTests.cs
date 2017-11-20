using Microsoft.VisualStudio.TestTools.UnitTesting;
using LGHISJKZGQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LGHISJKZGQ.Tests
{
    [TestClass()]
    public class TestItemSelectorTests
    {
        [TestMethod()]
        public void TestItemSelectorTest()
        {
            var f = new TestItemSelector();

            List<CYC_Item> items = new List<CYC_Item>();
            CYC_Item item = new CYC_Item()
            {
                CYC_MC="MC",ZJC1 = "ZJC1",ZJC2 ="ZJC2",
            };
            items.Add(item);
            f.CycItems = items;
           
            var r = f.ShowDialog();
            var s = f.SelectedTestItemName;
            Console.WriteLine(s);
        }
    }
}