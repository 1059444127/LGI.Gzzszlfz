using System;
using dbbase;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PathHISZGQJK.Zllyjgh.Tests
{
    [TestClass]
    public class StructReportTests
    {
        [TestMethod]
        public void GetXmlTest()
        {
            var aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            var bggs = "PCR-MSI";
            var blh = "1711109007-3";

            var dtTbsReport = aa.GetDataTable($" select * from t_tbs_bg t where t.f_blh='{blh}' ", "table1");
            if (dtTbsReport == null || dtTbsReport.Rows.Count == 0)
                Assert.Fail("没有找到tbs报告");

            var tbs = T_TBS_BG.DataRowToModel(dtTbsReport.Rows[0]);
            var r = new StructReport();

            var xml = r.GetXml(tbs, bggs);
            Console.WriteLine(xml);
        }
    }
}