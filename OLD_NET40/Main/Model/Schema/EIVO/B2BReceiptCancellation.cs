﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.269
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此原始程式碼由 xsd 版本=4.0.30319.1 自動產生。
// 
namespace Model.Schema.EIVO.B2B {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class CancelReceiptRoot {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CancelReceipt")]
        public CancelReceiptRootCancelReceipt[] CancelReceipt;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class CancelReceiptRootCancelReceipt {
        
        /// <remarks/>
        public string CancelReceiptNumber;
        
        /// <remarks/>
        public string ReceiptDate;
        
        /// <remarks/>
        public string BuyerId;
        
        /// <remarks/>
        public string SellerId;
        
        /// <remarks/>
        public string CancelDate;
        
        /// <remarks/>
        public string CancelTime;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Remark;
    }
}