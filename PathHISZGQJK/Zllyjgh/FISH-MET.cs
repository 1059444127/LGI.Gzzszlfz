namespace PathHISZGQJK.Zllyjgh
{
    public class FISH_MET : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);

            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW1.Trim()))
                structReport.UnStructItems.Add(new UNStructItem
                {
                    ItemName = "MET基因扩增",
                    Result = tbs.F_TBS_WSW1
                });
            //平均MET基因拷贝数/细胞=3.52
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW3.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "平均MET基因拷贝数",
                    Result = tbs.F_TBS_WSW3.Replace("平均MET基因拷贝数/细胞=", "").Trim(),
                    ResultUnit = "拷贝数/细胞"
                });
            //平均CEP7拷贝数/细胞=2.16
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW4.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "平均CEP7拷贝数",
                    Result = tbs.F_TBS_WSW4.Replace("平均CEP7拷贝数/细胞=", "").Trim(),
                    ResultUnit = "拷贝数/细胞"
                });
            //MET/CEP7比值=1.62
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW5.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "MET/CEP7比值",
                    Result = tbs.F_TBS_WSW5.Replace("MET/CEP7比值=", "").Trim(),
                });
        }
    }
}