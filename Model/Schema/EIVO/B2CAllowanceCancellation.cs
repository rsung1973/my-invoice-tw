﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1008
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace Model.Schema.EIVO {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class CancelAllowanceRoot {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CancelAllowance")]
        public CancelAllowanceRootCancelAllowance[] CancelAllowance;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class CancelAllowanceRootCancelAllowance {
        
        /// <remarks/>
        public string CancelAllowanceNumber;
        
        /// <remarks/>
        public string BuyerId;
        
        /// <remarks/>
        public string SellerId;
        
        /// <remarks/>
        public string AllowanceDate;
        
        /// <remarks/>
        public string CancelReason;
        
        /// <remarks/>
        public string CancelDate;
        
        /// <remarks/>
        public string CancelTime;
        
        /// <remarks/>
        public string Remark;
    }
}
