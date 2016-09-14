﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.261
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此原始程式碼由 xsd 版本=4.0.30319.1 自動產生。
// 
namespace Model.Schema.TurnKey.A0401
{
    using System.Xml.Serialization;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0", IsNullable = false)]
    public partial class Invoice
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public Main Main;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ProductItem", IsNullable = false)]
        public DetailsProductItem[] Details;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public Amount Amount;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public partial class Main
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string InvoiceNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string InvoiceDate;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time", Order = 2)]
        public System.DateTime InvoiceTime;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InvoiceTimeSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public MainSeller Seller;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public MainBuyer Buyer;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string CheckNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public MainBuyerRemark BuyerRemark;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BuyerRemarkSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string MainRemark;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public CustomsClearanceMarkEnum CustomsClearanceMark;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CustomsClearanceMarkSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string TaxCenter;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string PermitDate;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string PermitWord;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string PermitNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string Category;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string RelateNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public InvoiceTypeEnum InvoiceType;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public string GroupMark;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public DonateMarkEnum DonateMark;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public partial class MainSeller
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Identifier;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Address;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string PersonInCharge;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string TelephoneNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string FacsimileNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string EmailAddress;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string CustomerNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string RoleRemark;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public partial class Amount
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public long SalesAmount;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public TaxTypeEnum TaxType;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public decimal TaxRate;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public long TaxAmount;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public long TotalAmount;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public long DiscountAmount;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DiscountAmountSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public decimal OriginalCurrencyAmount;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool OriginalCurrencyAmountSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public decimal ExchangeRate;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExchangeRateSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public CurrencyCodeEnum Currency;

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CurrencySpecified;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public enum TaxTypeEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1 = 1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2 = 2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3 = 3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4 = 4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9 = 9,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public enum CurrencyCodeEnum
    {

        AED = 2,
        AFN = 204,
        ALL = 4,
        AMD = 5,
        ANG = 6,
        AOA = 7,
        ARS = 10,
        AUD = 12,
        AWG = 13,
        AZN = 205,
        BAM = 15,
        BBD = 16,
        BDT = 17,
        BGN = 20,
        BHD = 21,
        BIF = 22,
        BMD = 23,
        BND = 24,
        BOB = 25,
        BRL = 27,
        BSD = 28,
        BTN = 29,
        BWP = 30,
        BYR = 32,
        BZD = 33,
        CAD = 34,
        CDF = 35,
        CHF = 36,
        CLP = 38,
        CNY = 39,
        COP = 40,
        CRC = 41,
        CUP = 42,
        CVE = 43,
        CYP = 44,
        CZK = 45,
        DJF = 47,
        DKK = 48,
        DOP = 49,
        DZD = 50,
        EGP = 54,
        ERN = 55,
        ETB = 57,
        EUR = 58,
        FJD = 60,
        FKP = 61,
        GBP = 63,
        GEL = 64,
        GGP = 206,
        GHS = 207,
        GIP = 66,
        GMD = 67,
        GNF = 68,
        GTQ = 70,
        GYD = 72,
        HKD = 73,
        HNL = 74,
        HRK = 75,
        HTG = 76,
        HUF = 77,
        IDR = 78,
        ILS = 80,
        IMP = 208,
        INR = 81,
        IQD = 82,
        IRR = 83,
        ISK = 84,
        JEP = 209,
        JMD = 86,
        JOD = 87,
        JPY = 88,
        KES = 89,
        KGS = 90,
        KHR = 91,
        KMF = 92,
        KPW = 93,
        KRW = 94,
        KWD = 95,
        KYD = 96,
        KZT = 97,
        LAK = 98,
        LBP = 99,
        LKR = 100,
        LRD = 101,
        LSL = 102,
        LTL = 103,
        LVL = 105,
        LYD = 106,
        MAD = 107,
        MDL = 108,
        MGA = 210,
        MKD = 110,
        MMK = 111,
        MNT = 112,
        MOP = 113,
        MRO = 114,
        MTL = 115,
        MUR = 116,
        MVR = 117,
        MWK = 118,
        MXN = 119,
        MYR = 121,
        MZN = 211,
        NAD = 123,
        NGN = 124,
        NIO = 125,
        NOK = 127,
        NPR = 128,
        NZD = 129,
        OMR = 130,
        PAB = 131,
        PEN = 132,
        PGK = 133,
        PHP = 134,
        PKR = 135,
        PLN = 136,
        PYG = 138,
        QAR = 139,
        RON = 212,
        RSD = 213,
        RUB = 141,
        RWF = 143,
        SAR = 144,
        SBD = 145,
        SCR = 146,
        SDG = 214,
        SEK = 149,
        SGD = 150,
        SHP = 151,
        SLL = 154,
        SOS = 155,
        SPL = 215,
        SRD = 216,
        STD = 157,
        SVC = 158,
        SYP = 159,
        SZL = 160,
        THB = 161,
        TJS = 217,
        TMM = 163,
        TND = 164,
        TOP = 165,
        TRY = 218,
        TTD = 168,
        TVD = 219,
        TWD = 0,
        TZS = 169,
        UAH = 170,
        UGX = 171,
        USD = 172,
        UYU = 174,
        UZS = 175,
        VEB = 176,
        VEF = 203,
        VND = 177,
        VUV = 178,
        WST = 179,
        XAF = 180,
        XAG = 181,
        XAU = 182,
        XCD = 187,
        XDR = 188,
        XOF = 192,
        XPD = 193,
        XPF = 194,
        XPT = 195,
        YER = 197,
        ZAR = 199,
        ZMK = 200,
        ZWD = 202,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public partial class MainBuyer
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Identifier;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Name;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Address;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string PersonInCharge;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string TelephoneNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string FacsimileNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string EmailAddress;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string CustomerNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string RoleRemark;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public enum MainBuyerRemark
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1 = 1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2 = 2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3 = 3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4 = 4,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public enum CustomsClearanceMarkEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1 = 1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2 = 2,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public enum InvoiceTypeEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("01")]
        Item01 = 1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("02")]
        Item02 = 2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("03")]
        Item03 = 3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("04")]
        Item04 = 4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("05")]
        Item05 = 5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("06")]
        Item06 = 6,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public enum DonateMarkEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:GEINV:eInvoiceMessage:A0401:3.0")]
    public partial class DetailsProductItem
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Description;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public decimal Quantity;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Unit;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public decimal UnitPrice;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public decimal Amount;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string SequenceNumber;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Remark;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string RelateNumber;
    }
}
