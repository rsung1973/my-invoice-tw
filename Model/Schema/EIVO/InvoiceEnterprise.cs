﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.34209
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 
namespace Model.Schema.EIVO {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class InvoiceEnterpriseRoot {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("InvoiceEnterprise")]
        public InvoiceEnterpriseRootInvoiceEnterprise[] InvoiceEnterprise;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class InvoiceEnterpriseRootInvoiceEnterprise {
        
        /// <remarks/>
        public string SellerId;
        
        /// <remarks/>
        public string SellerName;
        
        /// <remarks/>
        public string Address;
        
        /// <remarks/>
        public string TEL;
        
        /// <remarks/>
        public string Email;
        
        /// <remarks/>
        public string ContactName;
        
        /// <remarks/>
        public string ContactPhone;
        
        /// <remarks/>
        public string ContactMobilePhone;
        
        /// <remarks/>
        public string UndertakerName;
        
        /// <remarks/>
        public byte InvoiceType;
    }
}