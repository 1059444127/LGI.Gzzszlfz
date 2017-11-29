namespace PathHISZGQJK.Zllyjgh
{
    public static class StructHelper
    {
        public static void FillCommonResult(T_TBS_BG tbs, StructReport structReport)
        {
            structReport.ItemName = tbs.F_FZ_JCXM;
            structReport.Result = tbs.F_FZ_BLZD;
            structReport.Explain = tbs.F_FZ_SYDZ;
            structReport.NDAND = T_TBS_BG.SubStringBetween(tbs.F_DNAZK, "浓度", @"ug/ml");
            structReport.NDA260 = T_TBS_BG.SubStringBetween(tbs.F_DNAZK, @"260/280=", @"。");
            structReport.BLZKJX = tbs.F_RNAZK;
            structReport.BLZKZLXBHL = T_TBS_BG.SubStringBetween(tbs.F_RNAZK, "肿瘤细胞含量比例约为", "%");
            if (string.IsNullOrEmpty(structReport.BLZKZLXBHL))
                structReport.BLZKZLXBHL = T_TBS_BG.SubStringBetween(tbs.F_RNAZK, "肿瘤细胞含量", "%");
            if (!string.IsNullOrEmpty(structReport.BLZKZLXBHL))
                structReport.BLZKZLXBHL = structReport.BLZKZLXBHL + "%";
        }
    }
}