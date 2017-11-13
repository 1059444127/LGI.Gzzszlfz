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
    public class zsdxzlyyfzPTTests
    {
        [TestMethod()]
        public void pathtohisTest()
        {
            var sender = new zsdxzlyyfzPT();
            sender.pathtohis("CE1700004", "","1","","0",new []{""});
        }
    }
}