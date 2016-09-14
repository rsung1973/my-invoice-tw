using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Model.Schema.TXN;

namespace Model.Schema.TurnKey
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "A1401" /*AnonymousType = true*/)]
    public partial class RootResponseForA1401 : RootResponse
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Invoice", Order = 1)]
        public Model.Schema.TurnKey.A1401.Invoice[] Invoice;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DataNumber", Order = 2)]
        public String[] DataNumber;
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "A0501" /*AnonymousType = true*/)]
    public partial class RootResponseForA0501 : RootResponse
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CancelInvoice", Order = 1)]
        public Model.Schema.TurnKey.A0501.CancelInvoice[] CancelInvoice;
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlInclude(typeof(Model.Schema.TurnKey.RootResponseForA1401))]
    public partial class RootA1401 : Root
    {

    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlInclude(typeof(Model.Schema.TurnKey.RootResponseForA0501))]
    public partial class RootA0501 : Root
    {

    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "B1401" /*AnonymousType = true*/)]
    public partial class RootResponseForB1401 : RootResponse
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Allowance", Order = 1)]
        public Model.Schema.TurnKey.B1401.Allowance[] Allowance;
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "B0501" /*AnonymousType = true*/)]
    public partial class RootResponseForB0501 : RootResponse
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CancelAllowance", Order = 1)]
        public Model.Schema.TurnKey.B0501.CancelAllowance[] CancelAllowance;
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlInclude(typeof(Model.Schema.TurnKey.RootResponseForB1401))]
    public partial class RootB1401 : Root
    {

    }


    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlInclude(typeof(Model.Schema.TurnKey.RootResponseForB0501))]
    public partial class RootB0501 : Root
    {

    }
        
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "A1401",Namespace="urn:GEINV:eInvoiceMessage:A1401:3.0" /*AnonymousType = true*/)]
    public partial class RootResponseSingleA1401 : RootResponse
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Invoice", Order = 1)]
        public Model.Schema.TurnKey.A1401.Invoice Invoice;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public int InvoiceID;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public String DataNumber;

    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [XmlInclude(typeof(Model.Schema.TurnKey.RootResponseSingleA1401))]
    public partial class RootSingleA1401 : Root
    {

    }

}

namespace Model.Schema.TurnKey.A1401 {
    using System.Xml.Serialization;

    public partial class Invoice
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public String DataNumber;

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public String InvoiceID;

    }

}