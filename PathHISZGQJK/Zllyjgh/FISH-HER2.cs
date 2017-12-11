namespace PathHISZGQJK.Zllyjgh
{
    public class FISH_HER2 : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);

//            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW1.Trim()))
//                structReport.UnStructItems.Add(new UNStructItem
//                {
//                    ItemName = "HER2基因扩增",
//                    Result = tbs.F_TBS_WSW1
//                });
            //连续计数浸润性癌 30 个肿瘤细胞
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW2.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "连续计数浸润性癌",
                    Result = T_TBS_BG.SubStringBetween(tbs.F_TBS_WSW2, "连续计数浸润性癌", "个肿瘤细胞").Trim(),
                    ResultUnit = "个"
                });
            //平均HER2基因拷贝数/细胞=3.52
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW3.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "平均HER2基因拷贝数",
                    Result = tbs.F_TBS_WSW3.Replace("平均HER2基因拷贝数/细胞=","").Trim(),
                    ResultUnit = "拷贝数/细胞"
                });
            //平均CEP17拷贝数/细胞=2.02
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW4.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "平均CEP17拷贝数",
                    Result = tbs.F_TBS_WSW4.Replace("平均CEP17拷贝数/细胞=", "").Trim(),
                    ResultUnit = "拷贝数/细胞"
                });
            //HER2/CEP17比值=1.06
            if (!string.IsNullOrEmpty(tbs.F_TBS_WSW5.Trim()))
                structReport.StructItems.Add(new StructItem
                {
                    ItemName = "HER2/CEP17比值",
                    Result = tbs.F_TBS_WSW5.Replace("HER2/CEP17比值=", "").Trim(),
                    ResultUnit = ""
                });
        }
    }
}