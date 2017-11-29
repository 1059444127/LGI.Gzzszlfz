namespace PathHISZGQJK.Zllyjgh
{
    public class HBV : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);

            //2.63E+05 copy/mL
            if (!string.IsNullOrEmpty(tbs.F_TBS_XBXM1.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "EBV-DNA定量检测",
                    Result = tbs.F_TBS_XBXM1.Replace("IU/mL","").Trim(),
                    ResultUnit = "IU/mL",
                    Reference = tbs.F_TBS_XBXM2.Replace("IU/mL", "").Trim(),
                    RefernceUnit = "IU/mL", 
                });
        }
    }
}