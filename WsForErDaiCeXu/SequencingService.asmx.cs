using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Services;
using dbbase;

namespace WsForErDaiCeXu
{
    /// <summary>
    /// Summary description for SequencingService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SequencingService : System.Web.Services.WebService
    {
        private odbcdb _aa = null;

        public SequencingService()
        {
            _aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        }

        /// <summary>
        /// 根据病理系统登记时间范围,查询二代测序申请单
        /// </summary>
        /// <param name="startRegTime">开始登记时间</param>
        /// <param name="endRegTime">结束登记时间</param>
        /// <returns></returns>
        [WebMethod]
        public List<SequenceRequest> GetPatientsForSequence(DateTime startRegTime, DateTime endRegTime)
        {
            log.WriteMyLog("收到请求:GetPatientsForSequence" +
                           "\r\n来自:" + this.Context.Request.UserHostAddress +
                           "\r\n入参:" +
                           $"\r\nstartRegTime:" + startRegTime+
                           $"\r\nendRegTime:" + endRegTime);

            string sqlWhere = " and  (  1=1 ";
            sqlWhere += $" and convert(datetime,f_sdrq+':00') >=  convert(datetime,'{startRegTime}')";
            sqlWhere += $" and convert(datetime,f_sdrq+':00') <=  convert(datetime,'{endRegTime}')";
            sqlWhere += " ) ";
            sqlWhere += $" and f_blk='二代测序' ";
            sqlWhere += $" and f_bgzt<>'已审核' ";

            var lstJcxx = (new T_JCXX_DAL()).GetBySqlWhere(sqlWhere);

            var lstSr = new List<SequenceRequest>();
            foreach (T_JCXX jcxx in lstJcxx)
            {
                var sr = new SequenceRequest();
                sr.PatientId = jcxx.F_BRBH;
                sr.Name = jcxx.F_XM;
                sr.Gender = jcxx.F_XB;
                sr.PathNo = jcxx.F_BLH;
                sr.RegTime = Convert.ToDateTime(jcxx.F_SDRQ + ":00");
                sr.TestItemName = jcxx.F_YZXM;

                lstSr.Add(sr);
            }

            return lstSr;
        }

        [WebMethod]
        public bool UploadSequenceReport(SequenceReport report)
        {
            log.WriteMyLog("收到请求:UploadSequenceReport" +
                           "\r\n来自:" + this.Context.Request.UserHostAddress +
                           "\r\n入参:" +
                           $"\r\nreport:" + report.Name+"_"+report.PathNo);

            if (report == null)
                return true;

            //插入数据库
            InsertLog(report);

            //文件转换为Word并放在指定位置
            SaveFile(report);

            return true;
        }

        [WebMethod]
        public List<SequenceReport> GetReportBySendDept(string deptName, DateTime startReportTime, DateTime endReportTime)
        {
            log.WriteMyLog("收到请求:GetReportBySendDept" +
                           "\r\n来自:" + this.Context.Request.UserHostAddress+
                           "\r\n入参:" +
                           $"\r\ndeptName:"+deptName+
                           $"\r\nstartReportTime:"+ startReportTime +
                           $"\r\nendReportTime"+endReportTime);

            string sqlWhere = " and  (  1=1 ";
            sqlWhere += $" and convert(datetime,f_spare5) >=  convert(datetime,'{startReportTime}')";
            sqlWhere += $" and convert(datetime,f_spare5) <=  convert(datetime,'{endReportTime}')";
            sqlWhere += " ) ";
          //  sqlWhere += $" and f_blk='二代测序' ";
            sqlWhere += $" and f_bgzt='已审核' ";
            sqlWhere += $" and f_sjdw='{deptName}' ";

            var lstJcxx = (new T_JCXX_DAL()).GetBySqlWhere(sqlWhere);

            var lstSr = new List<SequenceReport>();
            foreach (T_JCXX jcxx in lstJcxx)
            {
                var sr = new SequenceReport();
                sr.PatientId = jcxx.F_BRBH;
                sr.Name = jcxx.F_XM;
                sr.Gender = jcxx.F_XB;
                sr.PathNo = jcxx.F_BLH;
                sr.RegTime = Convert.ToDateTime(jcxx.F_SDRQ + ":00");
                sr.TestItemName = jcxx.F_YZXM;
                sr.SendDept = jcxx.F_SJDW;
                sr.Diag = jcxx.F_BLZD;
                sr.ReportTime = Convert.ToDateTime(jcxx.F_BGRQ);
                lstSr.Add(sr);
            }

            return lstSr;
        }

        [WebMethod]
        public SequenceReport GetReporyWithPdfByPathNo(string pathNo)
        {
            log.WriteMyLog("收到请求:GetReporyWithPdfByPathNo\r\n" +
                           "来自:" + this.Context.Request.UserHostAddress +
                           "\r\n入参:" +
                           $"\r\npathNo:" + pathNo);

            string sqlWhere="";
            //sqlWhere += $" and f_blk='二代测序' ";
            sqlWhere += $" and f_bgzt='已审核' ";
            sqlWhere += $" and f_blh='{pathNo}' ";

            var lstJcxx = (new T_JCXX_DAL()).GetBySqlWhere(sqlWhere);
            if (lstJcxx.Count == 0)
                throw new Exception("没有找到该报告,或报告尚未审核!");
            //获得pdf地址
            var dtPdf = _aa.GetDataTable($"select * from t_bg_pdf where f_blh = '{pathNo}' ","dt1");
            if(dtPdf.Rows.Count==0)
                throw new Exception("该病人没有生成PDF!");

            //获取本地pdf存放地址,读取并转成base64
            var drPdf = dtPdf.Rows[0];
            var pdfbgLocalPath = ConfigurationManager.AppSettings["pdfbgLocalPath"];
            var pdfLocalFileName= pdfbgLocalPath + "\\" + drPdf["F_ML"] + "\\" + drPdf["F_FILENAME"].ToString();
            FileStream filestream = null;
            try
            {
                filestream = new FileStream(pdfLocalFileName, FileMode.Open);
                byte[] bt = new byte[filestream.Length];
                //调用read读取方法  
                filestream.Read(bt, 0, bt.Length);
                var base64Str = Convert.ToBase64String(bt, Base64FormattingOptions.None);

                var jcxx = lstJcxx[0];
                var sr = new SequenceReport();
                sr.PatientId = jcxx.F_BRBH;
                sr.Name = jcxx.F_XM;
                sr.Gender = jcxx.F_XB;
                sr.PathNo = jcxx.F_BLH;
                sr.RegTime = Convert.ToDateTime(jcxx.F_SDRQ + ":00");
                sr.ReportTime = Convert.ToDateTime(jcxx.F_SPARE5);
                sr.TestItemName = jcxx.F_YZXM;
                sr.SendDept = jcxx.F_SJDW;
                sr.Diag = jcxx.F_BLZD;
                sr.PdfBase64 = base64Str;

                return sr;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                filestream?.Close();
            }

        }

        private void InsertLog(SequenceReport report)
        {
            string sql =
                $@" insert into t_sequence_report_log (id,blh,file_path,file_name,
                        brbh,xm,xb)
                    values 
                    ('{Guid.NewGuid()}','{report.PathNo}','{report.ReportPath}','{report.ReportName}',
                    '{report.PatientId}','{report.Name}','{report.Gender}') ";

            var count = _aa.ExecuteSQL(sql);
            if (count == 0)
            {
                log.WriteMyLog("二代测序报告插入数据库失败,sql:\r\n"+sql);
                throw new Exception("二代测序报告插入数据库失败,sql:\r\n" + sql);
            }
        }

        private void SaveFile(SequenceReport report)
        {
            var bytes = Convert.FromBase64String(report.ReportDocBase64String);

            var path = ConfigurationManager.AppSettings["fileLocalPath"]+@"\";
            path += report.ReportPath;

            //如果目录不存在则新建
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            //组合文件名
            if (path.EndsWith(@"\") == false)
                path += @"\";
            path += report.Name+"_"+ report.PathNo + "_"+ report.ReportName;

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
            }
        }

        /// <summary>
        /// 二代测序申请单实体
        /// </summary>
        [Serializable]
        public class SequenceRequest
        {
            /// <summary>
            /// 病历号
            /// </summary>
            public string PatientId { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 性别
            /// </summary>
            public string Gender { get; set; }

            /// <summary>
            /// 病理系统登记时间
            /// </summary>
            public DateTime RegTime { get; set; }

            /// <summary>
            /// 检查项目名称
            /// </summary>
            public string TestItemName { get; set; }

            /// <summary>
            /// 病理号(病理系统唯一号)
            /// </summary>
            public string PathNo { get; set; }
        }

        [Serializable]
        public class SequenceReport
        {
            /// <summary>
            /// 病历号
            /// </summary>
            public string PatientId { get; set; }

            /// <summary>
            /// 姓名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 性别
            /// </summary>
            public string Gender { get; set; }

            /// <summary>
            /// 病理系统登记时间
            /// </summary>
            public DateTime RegTime { get; set; }

            /// <summary>
            /// 检查项目名称
            /// </summary>
            public string TestItemName { get; set; }

            /// <summary>
            /// 病理号(病理系统唯一号)
            /// </summary>
            public string PathNo { get; set; }

            /// <summary>
            /// 报告存放相对路径,如:"2017\10\24"
            /// </summary>
            public string ReportPath { get; set; }

            /// <summary>
            /// 报告文件名称
            /// </summary>
            public string ReportName { get; set; }

            /// <summary>
            /// 报告word文档base64字符串
            /// </summary>
            public string ReportDocBase64String { set; get; }

            /// <summary>
            /// 送检单位
            /// </summary>
            public string SendDept { get; set; }

            /// <summary>
            /// 病理诊断
            /// </summary>
            public string Diag { get; set; }

            /// <summary>
            /// PDF转base64string
            /// </summary>
            public string PdfBase64 { get; set; }

            /// <summary>
            /// 报告审核时间
            /// </summary>
            public DateTime ReportTime { get; set; }
        }
    }
}