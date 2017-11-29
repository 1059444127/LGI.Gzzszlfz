namespace PathHISZGQJK.Zllyjgh
{
    public class PCR_EGFR : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);

            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW1.Trim()))
                structReport.UnStructItems.Add(new UNStructItem {ItemName = tbs.F_TBS_WSW1, Result = tbs.F_TBS_BDXM1});
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW2.Trim()))
                structReport.UnStructItems.Add(new UNStructItem {ItemName = tbs.F_TBS_WSW2, Result = tbs.F_TBS_BDXM2});
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW3.Trim()))
                structReport.UnStructItems.Add(new UNStructItem {ItemName = tbs.F_TBS_WSW3, Result = tbs.F_TBS_BDXM3});
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW4.Trim()))
                structReport.UnStructItems.Add(new UNStructItem {ItemName = tbs.F_TBS_WSW4, Result = tbs.F_TBS_BDXM4});
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW5.Trim()))
                structReport.UnStructItems.Add(new UNStructItem {ItemName = tbs.F_TBS_WSW5, Result = tbs.F_TBS_BDXM5});
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW6.Trim()))
                structReport.UnStructItems.Add(new UNStructItem {ItemName = tbs.F_TBS_WSW6, Result = tbs.F_TBS_BDXM6});
        }


    }
}