namespace PathHISZGQJK.Zllyjgh
{
    public class PCR_MSI : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);
            structReport.NDA260 = T_TBS_BG.SubStringBetween(tbs.F_DNAZK, @"260/280=", @"正常标本浓度").Trim();
        }


    }
}