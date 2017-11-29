using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathHISZGQJK.Zllyjgh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathHISZGQJK.Zllyjgh.Tests
{
    [TestClass()]
    public class T_TBS_BGTests
    {
        [TestMethod()]
        public void SubStringBetweenTest()
        {
            var s = T_TBS_BG.SubStringBetween("符合检测标准，浓度37.2ug/ml，260/280=2.02。", "浓度", @"ug/ml");
        }
    }
}