namespace PathHISZGQJK.Zllyjgh
{
    public class 测序 : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);
        }
    }
}