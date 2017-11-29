namespace PathHISZGQJK.Zllyjgh
{
    public class PCR_EGFR : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);
        }


    }
}