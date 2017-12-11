namespace PathHISZGQJK.Zllyjgh
{
    public class 基因重排 : ITbsAnalyzier
    {
        public void FillStructReport(T_TBS_BG tbs, StructReport structReport)
        {
            StructHelper.FillCommonResult(tbs, structReport);
            structReport.Explain=@"采用BIOMED-2标准化基因重排克隆性分析系统，通过多重PCR技术检测免疫球蛋白(Ig)及T细胞抗原受体（TCR）克隆
性基因重排。用于淋巴组织增殖性疾病的辅助诊断。最终诊断结果需结合形态学、免疫表型及临床。";
        }
    }
}