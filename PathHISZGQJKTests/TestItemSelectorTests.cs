using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathHISZGQJK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathHISZGQJK.Tests
{
    [TestClass()]
    public class TestItemSelectorTests
    {
        [TestMethod()]
        public void TestItemSelectorTest()
        {
            DiseaseSelector f = new DiseaseSelector();
            f.ShowInTaskbar = false;
            
            f.ShowDialog();
        }

        [TestMethod()]
        public void GetSpellCodeTest()
        {
            Assert.Fail();
        }
    }
}