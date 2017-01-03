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
namespace Model.Schema.TurnKey.B1401 {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1", IsNullable=false)]
    public partial class Allowance {
        
        /// <remarks/>
        public Main Main;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ProductItem", IsNullable=false)]
        public DetailsProductItem[] Details;
        
        /// <remarks/>
        public Amount Amount;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public partial class Main {
        
        /// <remarks/>
        public string AllowanceNumber;
        
        /// <remarks/>
        public string AllowanceDate;
        
        /// <remarks/>
        public MainSeller Seller;
        
        /// <remarks/>
        public MainBuyer Buyer;
        
        /// <remarks/>
        public AllowanceTypeEnum AllowanceType;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] Attachment;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public partial class MainSeller {
        
        /// <remarks/>
        public string Identifier;
        
        /// <remarks/>
        public string Name;
        
        /// <remarks/>
        public string Address;
        
        /// <remarks/>
        public string PersonInCharge;
        
        /// <remarks/>
        public string TelephoneNumber;
        
        /// <remarks/>
        public string FacsimileNumber;
        
        /// <remarks/>
        public string EmailAddress;
        
        /// <remarks/>
        public string CustomerNumber;
        
        /// <remarks/>
        public string RoleRemark;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public partial class Amount {
        
        /// <remarks/>
        public long TaxAmount;
        
        /// <remarks/>
        public long TotalAmount;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public partial class MainBuyer {
        
        /// <remarks/>
        public string Identifier;
        
        /// <remarks/>
        public string Name;
        
        /// <remarks/>
        public string Address;
        
        /// <remarks/>
        public string PersonInCharge;
        
        /// <remarks/>
        public string TelephoneNumber;
        
        /// <remarks/>
        public string FacsimileNumber;
        
        /// <remarks/>
        public string EmailAddress;
        
        /// <remarks/>
        public string CustomerNumber;
        
        /// <remarks/>
        public string RoleRemark;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public enum AllowanceTypeEnum {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1 = 1,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public partial class DetailsProductItem {
        
        /// <remarks/>
        public string OriginalInvoiceDate;
        
        /// <remarks/>
        public string OriginalInvoiceNumber;
        
        /// <remarks/>
        public string OriginalSequenceNumber;
        
        /// <remarks/>
        public string OriginalDescription;
        
        /// <remarks/>
        public decimal Quantity;
        
        /// <remarks/>
        public decimal Quantity2;
        
        /// <remarks/>
        public string Unit;
        
        /// <remarks/>
        public string Unit2;
        
        /// <remarks/>
        public decimal UnitPrice;
        
        /// <remarks/>
        public decimal UnitPrice2;
        
        /// <remarks/>
        public decimal Amount;
        
        /// <remarks/>
        public decimal Amount2;
        
        /// <remarks/>
        public long Tax;
        
        /// <remarks/>
        public string AllowanceSequenceNumber;
        
        /// <remarks/>
        public DetailsProductItemTaxType TaxType;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:GEINV:eInvoiceMessage:B1401:3.1")]
    public enum DetailsProductItemTaxType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1 = 1,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9 = 9,
    }
}
