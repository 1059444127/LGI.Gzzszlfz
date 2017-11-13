﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using WsForErDaiCeXu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsForErDaiCeXuTests.ServiceReference1;

namespace WsForErDaiCeXu.Tests
{
    [TestClass()]
    public class SequencingServiceTests
    {
        [TestMethod()]
        public void UploadReportTest()
        {
            SequencingServiceSoap service = new SequencingServiceSoapClient();

            SequenceReport report = new SequenceReport();
            report.PathNo = "20170001";
            report.Name = "测试1";
            report.Gender = "男性";
            report.ReportPath = @"2017\10\24";
            report.ReportName = "测试报告.doc";
            report.ReportDocBase64String = base64TestString;
           
            UploadSequenceReportRequest serviceRequest = new UploadSequenceReportRequest();
            serviceRequest.Body=new UploadSequenceReportRequestBody();
            serviceRequest.Body.report = report;

            service.UploadSequenceReport(serviceRequest);
        }

        [TestMethod()]
        public void GetBySjdw()
        {
            SequencingServiceSoap service = new SequencingServiceSoapClient();

            GetReportBySendDeptRequest request=new GetReportBySendDeptRequest();
            request.Body=new GetReportBySendDeptRequestBody();
            request.Body.deptName = "本院";
            request.Body.startReportTime=new DateTime(2017,8,21);
            request.Body.endReportTime = new DateTime(2017,9,1);

            var result =  service.GetReportBySendDept(request);
        }
        
        [TestMethod()]
        public void GetReportByPathNo()
        {
            SequencingServiceSoap service = new SequencingServiceSoapClient();

            GetReporyWithPdfByPathNoRequest request = new GetReporyWithPdfByPathNoRequest();
            request.Body = new GetReporyWithPdfByPathNoRequestBody();
            request.Body.pathNo = "EB170821154";

            var result = service.GetReporyWithPdfByPathNo(request);
        }

        #region test data

        public string base64TestString =
            "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";

        #endregion
    }
}