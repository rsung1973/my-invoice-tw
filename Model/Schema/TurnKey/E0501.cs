﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 
namespace Model.Schema.TurnKey.E0501 {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:GEINV:eInvoiceMessage:E0501:3.1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:GEINV:eInvoiceMessage:E0501:3.1", IsNullable=false)]
    public partial class InvoiceAssignNo {
        
        /// <remarks/>
        public string Ban;
        
        /// <remarks/>
        public InvoiceTypeEnum InvoiceType;
        
        /// <remarks/>
        public string YearMonth;
        
        /// <remarks/>
        public string InvoiceTrack;
        
        /// <remarks/>
        public string InvoiceBeginNo;
        
        /// <remarks/>
        public string InvoiceEndNo;
        
        /// <remarks/>
        public long InvoiceBooklet;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:GEINV:eInvoiceMessage:E0501:3.1")]
    public enum InvoiceTypeEnum {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01")]
        Item01 = 1,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02")]
        Item02,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("07")]
        Item07,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("08")]
        Item08,
    }
}
