namespace PathHISZGQJK.Zllyjgh
{
    public class FISH : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);
            //检测项目
            if (!string.IsNullOrEmpty(tbs.F_FZ_JCXM.Trim()))
                structReport.UnStructItems.Add(new UNStructItem
                {
                    ItemName = "检测项目",
                    Result = tbs.F_FZ_JCXM
                });
            //判读标准
            if (!string.IsNullOrEmpty(tbs.F_FZ_SYDZ.Trim()))
                structReport.UnStructItems.Add(new UNStructItem
                {
                    ItemName = "判读标准",
                    Result = tbs.F_FZ_SYDZ
                });
        }
    }
}