using System.Collections.Generic;
using System.Reflection;

namespace PathHISZGQJK.Zllyjgh
{
    public class StructReport
    {
        public List<StructItem> StructItems = new List<StructItem>();
        public List<UNStructItem> UnStructItems = new List<UNStructItem>();

        public string ItemName { get; set; }

        public string Result { get; set; }

        /// <summary>
        ///     病理质控-镜下
        /// </summary>
        public string BLZKJX { get; set; }

        /// <summary>
        ///     病理质控-肿瘤细胞含量
        /// </summary>
        public string BLZKZLXBHL { get; set; }

        public string NDAND { get; set; }

        public string NDA260 { get; set; }

        public string Explain { get; set; }

        public string GetXml()
        {
            var xml = @"
                    <!-- 数值型 -->
                    <Struct>
	                    <!-- 如有多条项目则多个Item -->
";
            if (StructItems != null)
                foreach (var item in StructItems)
                {
                    xml += $@"
                    	<Item>
		                    <!-- 项目编码 -->
		                    <itemCode>{item.ItemCode}</itemCode>
		                    <!-- 项目名称 -->
		                    <itemName>{item.ItemName}</itemName>
		                    <!-- 监测结果 请用<![CDATA[]]>包括  -->
		                    <result><![CDATA[{item.Result}]]></result>
		                    <!-- 监测结果单位 请用<![CDATA[]]>包括  -->
		                    <resultUnit><![CDATA[{item.ResultUnit}]]></resultUnit>
		                    <!-- 参考值 请用<![CDATA[]]>包括 如为范围请用英文半角字符""/""隔开 如>100或<200或100/200 -->
		                    <reference><![CDATA[{item.Reference}]]></reference>
		                    <!-- 参考值单位 请用<![CDATA[]]>包括  -->
		                    <referenceUnit><![CDATA[{item.RefernceUnit}]]></referenceUnit>
		                    <!-- 其他 -->
		                    <remark>{item.Remark}</remark>
	                    </Item>
                    ";
                }
            xml += $@"
	                </Struct>
	                <!-- 非数值型 -->
	                <UNStruct>
		                <!-- 项目名称（串） -->
		                <itemName>{ItemName}</itemName>
		                <!-- 监测结果 -->
		                <result>{Result}</result>
		                <!-- 病理质控-镜下 -->
		                <BLZKJX>{BLZKJX}</BLZKJX>
		                <!-- 病理质控-肿瘤细胞含量 -->
		                <BLZKZLXBHL>{BLZKZLXBHL}</BLZKZLXBHL>
		                <!-- NDA质量评估-浓度 -->
		                <NDAND>{NDAND}</NDAND>
		                <!-- NDA质量评估-260/280 -->
		                <NDA260>{NDA260}</NDA260>
		                <!-- 监测结果诠释 -->
		                <explain>{Explain}</explain>
                        ";
            if (UnStructItems != null)
                foreach (var item in UnStructItems)
                {
                    xml += $@"
                <Item>
			        <!-- 项目编码 -->
			        <itemCode>{item.ItemCode}</itemCode>
			        <!-- 项目名称 -->
			        <itemName>{item.ItemName}</itemName>
			        <!-- 监测结果 请用<![CDATA[]]>包括 -->
		            <result><![CDATA[{item.Result}]]></result>
		        </Item>       
";
                }
            xml += $@"		
	        </UNStruct>
                        ";

            return xml;
        }

        /// <summary>
        /// 根据结构化数据和报告格式,解析tbs并返回结构化xml
        /// </summary>
        /// <param name="blh"></param>
        /// <param name="bggs"></param>
        /// <returns></returns>
        public string GetXml(T_TBS_BG tbs,string bggs)
        {
            //初始化格式解析接口
            var assembly = Assembly.GetAssembly(GetType());
            bggs = bggs.Replace("-", "_");
            ITbsAnalyzier ta = assembly.CreateInstance("PathHISZGQJK.Zllyjgh." + bggs) as ITbsAnalyzier;
            if (ta == null)
                return GetXml();
            //解析tbs
            ta.FillStructReport(tbs,this);
            //返回xml
            return GetXml();
        }

    }

    public class StructItem
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Result { get; set; }
        public string ResultUnit { get; set; }
        public string Reference { get; set; }
        public string RefernceUnit { get; set; }
        public string Remark { get; set; }
    }

    public class UNStructItem
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Result { get; set; }
    }
    
}