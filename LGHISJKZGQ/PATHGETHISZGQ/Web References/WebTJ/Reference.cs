﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.1
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.1 版自动生成。
// 
#pragma warning disable 1591

namespace LGHISJKZGQ.WebTJ {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PathologySysServiceSoap", Namespace="http://tongji.org")]
    public partial class PathologySysService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetInpChargeOperationCompleted;
        
        private System.Threading.SendOrPostCallback InPatientInfoQueryOperationCompleted;
        
        private System.Threading.SendOrPostCallback OutpExamApplyQueryOperationCompleted;
        
        private System.Threading.SendOrPostCallback SetInpChargeOperationCompleted;
        
        private System.Threading.SendOrPostCallback manageXyInAdviceOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PathologySysService() {
            this.Url = global::LGHISJKZGQ.Properties.Settings.Default.PATHGETHISZGQ_WebTJ_PathologySysService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetInpChargeCompletedEventHandler GetInpChargeCompleted;
        
        /// <remarks/>
        public event InPatientInfoQueryCompletedEventHandler InPatientInfoQueryCompleted;
        
        /// <remarks/>
        public event OutpExamApplyQueryCompletedEventHandler OutpExamApplyQueryCompleted;
        
        /// <remarks/>
        public event SetInpChargeCompletedEventHandler SetInpChargeCompleted;
        
        /// <remarks/>
        public event manageXyInAdviceCompletedEventHandler manageXyInAdviceCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tongji.org/Tongji.COMM.SoapService.PathologySysService.GetInpCharge", RequestNamespace="http://tongji.org", ResponseNamespace="http://tongji.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetInpCharge(string pInput) {
            object[] results = this.Invoke("GetInpCharge", new object[] {
                        pInput});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetInpChargeAsync(string pInput) {
            this.GetInpChargeAsync(pInput, null);
        }
        
        /// <remarks/>
        public void GetInpChargeAsync(string pInput, object userState) {
            if ((this.GetInpChargeOperationCompleted == null)) {
                this.GetInpChargeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetInpChargeOperationCompleted);
            }
            this.InvokeAsync("GetInpCharge", new object[] {
                        pInput}, this.GetInpChargeOperationCompleted, userState);
        }
        
        private void OnGetInpChargeOperationCompleted(object arg) {
            if ((this.GetInpChargeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetInpChargeCompleted(this, new GetInpChargeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tongji.org/Tongji.COMM.SoapService.PathologySysService.InPatientInfoQuery", RequestNamespace="http://tongji.org", ResponseNamespace="http://tongji.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string InPatientInfoQuery(string pInput) {
            object[] results = this.Invoke("InPatientInfoQuery", new object[] {
                        pInput});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void InPatientInfoQueryAsync(string pInput) {
            this.InPatientInfoQueryAsync(pInput, null);
        }
        
        /// <remarks/>
        public void InPatientInfoQueryAsync(string pInput, object userState) {
            if ((this.InPatientInfoQueryOperationCompleted == null)) {
                this.InPatientInfoQueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInPatientInfoQueryOperationCompleted);
            }
            this.InvokeAsync("InPatientInfoQuery", new object[] {
                        pInput}, this.InPatientInfoQueryOperationCompleted, userState);
        }
        
        private void OnInPatientInfoQueryOperationCompleted(object arg) {
            if ((this.InPatientInfoQueryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.InPatientInfoQueryCompleted(this, new InPatientInfoQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tongji.org/Tongji.COMM.SoapService.PathologySysService.OutpExamApplyQuery", RequestNamespace="http://tongji.org", ResponseNamespace="http://tongji.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string OutpExamApplyQuery(string pInput) {
            object[] results = this.Invoke("OutpExamApplyQuery", new object[] {
                        pInput});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void OutpExamApplyQueryAsync(string pInput) {
            this.OutpExamApplyQueryAsync(pInput, null);
        }
        
        /// <remarks/>
        public void OutpExamApplyQueryAsync(string pInput, object userState) {
            if ((this.OutpExamApplyQueryOperationCompleted == null)) {
                this.OutpExamApplyQueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnOutpExamApplyQueryOperationCompleted);
            }
            this.InvokeAsync("OutpExamApplyQuery", new object[] {
                        pInput}, this.OutpExamApplyQueryOperationCompleted, userState);
        }
        
        private void OnOutpExamApplyQueryOperationCompleted(object arg) {
            if ((this.OutpExamApplyQueryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.OutpExamApplyQueryCompleted(this, new OutpExamApplyQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tongji.org/Tongji.COMM.SoapService.PathologySysService.SetInpCharge", RequestNamespace="http://tongji.org", ResponseNamespace="http://tongji.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string SetInpCharge(string pInput) {
            object[] results = this.Invoke("SetInpCharge", new object[] {
                        pInput});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void SetInpChargeAsync(string pInput) {
            this.SetInpChargeAsync(pInput, null);
        }
        
        /// <remarks/>
        public void SetInpChargeAsync(string pInput, object userState) {
            if ((this.SetInpChargeOperationCompleted == null)) {
                this.SetInpChargeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSetInpChargeOperationCompleted);
            }
            this.InvokeAsync("SetInpCharge", new object[] {
                        pInput}, this.SetInpChargeOperationCompleted, userState);
        }
        
        private void OnSetInpChargeOperationCompleted(object arg) {
            if ((this.SetInpChargeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SetInpChargeCompleted(this, new SetInpChargeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tongji.org/Tongji.COMM.SoapService.PathologySysService.manageXyInAdvice", RequestNamespace="http://tongji.org", ResponseNamespace="http://tongji.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string manageXyInAdvice(string type, string xmlStr) {
            object[] results = this.Invoke("manageXyInAdvice", new object[] {
                        type,
                        xmlStr});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void manageXyInAdviceAsync(string type, string xmlStr) {
            this.manageXyInAdviceAsync(type, xmlStr, null);
        }
        
        /// <remarks/>
        public void manageXyInAdviceAsync(string type, string xmlStr, object userState) {
            if ((this.manageXyInAdviceOperationCompleted == null)) {
                this.manageXyInAdviceOperationCompleted = new System.Threading.SendOrPostCallback(this.OnmanageXyInAdviceOperationCompleted);
            }
            this.InvokeAsync("manageXyInAdvice", new object[] {
                        type,
                        xmlStr}, this.manageXyInAdviceOperationCompleted, userState);
        }
        
        private void OnmanageXyInAdviceOperationCompleted(object arg) {
            if ((this.manageXyInAdviceCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.manageXyInAdviceCompleted(this, new manageXyInAdviceCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetInpChargeCompletedEventHandler(object sender, GetInpChargeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetInpChargeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetInpChargeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void InPatientInfoQueryCompletedEventHandler(object sender, InPatientInfoQueryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class InPatientInfoQueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal InPatientInfoQueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void OutpExamApplyQueryCompletedEventHandler(object sender, OutpExamApplyQueryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class OutpExamApplyQueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal OutpExamApplyQueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void SetInpChargeCompletedEventHandler(object sender, SetInpChargeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SetInpChargeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal SetInpChargeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void manageXyInAdviceCompletedEventHandler(object sender, manageXyInAdviceCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class manageXyInAdviceCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal manageXyInAdviceCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591