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
    public partial class InvoiceRoot {
        
        /// <remarks/>
        public string CompanyBan;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Invoice")]
        public InvoiceRootInvoice[] Invoice;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class InvoiceRootInvoice {
        
        /// <remarks/>
        public string InvoiceNumber;
        
        /// <remarks/>
        public string InvoiceDate;
        
        /// <remarks/>
        public string InvoiceTime;
        
        /// <remarks/>
        public string DataNumber;
        
        /// <remarks/>
        public string DataDate;
        
        /// <remarks/>
        public string GoogleId;
        
        /// <remarks/>
        public string SellerId;
        
        /// <remarks/>
        public string BuyerName;
        
        /// <remarks/>
        public string BuyerId;
        
        /// <remarks/>
        public string CustomsClearanceMark;
        
        /// <remarks/>
        public string InvoiceType;
        
        /// <remarks/>
        public string DonateMark;
        
        /// <remarks/>
        public string CarrierType;
        
        /// <remarks/>
        public string CarrierId1;
        
        /// <remarks/>
        public string CarrierId2;
        
        /// <remarks/>
        public string PrintMark;
        
        /// <remarks/>
        public string NPOBAN;
        
        /// <remarks/>
        public string RandomNumber;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("InvoiceItem")]
        public InvoiceRootInvoiceInvoiceItem[] InvoiceItem;
        
        /// <remarks/>
        public decimal SalesAmount;
        
        /// <remarks/>
        public decimal FreeTaxSalesAmount;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FreeTaxSalesAmountSpecified;
        
        /// <remarks/>
        public decimal ZeroTaxSalesAmount;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ZeroTaxSalesAmountSpecified;
        
        /// <remarks/>
        public byte TaxType;
        
        /// <remarks/>
        public decimal TaxRate;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaxRateSpecified;
        
        /// <remarks/>
        public decimal TaxAmount;
        
        /// <remarks/>
        public decimal TotalAmount;
        
        /// <remarks/>
        public decimal DiscountAmount;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DiscountAmountSpecified;
        
        /// <remarks/>
        public string CustomerID;
        
        /// <remarks/>
        public string DataSequenceNo;
        
        /// <remarks/>
        public string ContactName;
        
        /// <remarks/>
        public string EMail;
        
        /// <remarks/>
        public string Address;
        
        /// <remarks/>
        public string Phone;
        
        /// <remarks/>
        public InvoiceRootInvoiceContact Contact;
        
        /// <remarks/>
        public InvoiceRootInvoiceCustomerDefined CustomerDefined;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class InvoiceRootInvoiceInvoiceItem {
        
        /// <remarks/>
        public string Description;
        
        /// <remarks/>
        public decimal Quantity;
        
        /// <remarks/>
        public string Unit;
        
        /// <remarks/>
        public decimal UnitPrice;
        
        /// <remarks/>
        public decimal Amount;
        
        /// <remarks/>
        public decimal SequenceNumber;
        
        /// <remarks/>
        public string Item;
        
        /// <remarks/>
        public string Remark;
        
        /// <remarks/>
        public byte TaxType;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaxTypeSpecified;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class InvoiceRootInvoiceContact {
        
        /// <remarks/>
        public string Name;
        
        /// <remarks/>
        public string Address;
        
        /// <remarks/>
        public string TEL;
        
        /// <remarks/>
        public string Email;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class InvoiceRootInvoiceCustomerDefined {
        
        /// <remarks/>
        public string ProjectNo;
        
        /// <remarks/>
        public string PurchaseNo;
        
        /// <remarks/>
        public short StampDutyFlag;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StampDutyFlagSpecified;
    }
}
