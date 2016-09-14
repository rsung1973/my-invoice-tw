<%@ Control Language="c#" Inherits="Uxnet.Web.Module.Common.SignContext" %>
<%--<object id="signable" classid="clsid:6AD7A39B-19C8-3838-97EB-04D488275714">
</object>--%>
<object id="signable" classid="<%= String.Format("{0}#Uxnet.Distribute.Signable",VirtualPathUtility.ToAbsolute("~/Published/Uxnet.Distribute.dll")) %>">
</object>
<script type="text/javascript">
<!--

    function signContext(arg, context) {
        if(arg=='' || arg==null)
        {
            if(emptyMsg!='')
            {
                alert(emptyMsg);
                return;
            }
        }
        var signObj = CreateSignable(); //document.all('signable');
        if (signObj != null) {
            if (signObj.SignCms(arg)) {
                var form = document.forms[0];

                var element = form.all('__dataToSign');
                if (element == null) {
                    element = document.createElement('textArea');
                    element.name = "__dataToSign";
                    element.style.display="none";
                    element.style.visibility="hidden";
                    form.appendChild(element);
                }
                element.value = arg;

                element = form.all('dataSignature');
                if (element == null) {
                    element = document.createElement('textArea');
                    element.name = "dataSignature";
                    element.style.display="none";
                    element.style.visibility="hidden";
                    form.appendChild(element);
                }
                element.value = signObj.SignedMessage;

                afterSigned();
            }
            else {
                alert(signObj.ErrorMessage);
            }
        }
    }

    function signContextSilently(arg, context) {
        var signObj = CreateSignable(); //document.all('signable');
        if (signObj != null) {
            if (signObj.SignCmsSilently(arg)) {
                var form = document.forms[0];

                var element = form.all('__dataToSign');
                if (element == null) {
                    element = document.createElement('textArea');
                    element.name = "__dataToSign";
                    element.style.display="none";
                    element.style.visibility="hidden";
                    form.appendChild(element);
                }
                element.value = arg;

                element = form.all('dataSignature');
                if (element == null) {
                    element = document.createElement('textArea');
                    element.name = "dataSignature";
                    element.style.display="none";
                    element.style.visibility="hidden";
                    form.appendChild(element);
                }
                element.value = signObj.SignedMessage;

                afterSigned();
            }
            else {
                alert(signObj.ErrorMessage);
            }
        }
    }
    

    function signMessage(form, arg) {
        if (form != null) {
            var signObj = CreateSignable(); //document.all('signable');
            if (signObj != null) {
                
                if (signObj.SignCms(arg)) {

                    var element = form.all('__dataToSign');
                    if (element == null) {
                        element = document.createElement('textArea');
                    element.name = "__dataToSign";
                    element.style.display="none";
                    element.style.visibility="hidden";
                        form.appendChild(element);
                    }
                    element.value = arg;

                    element = form.all('dataSignature');
                    if (element == null) {
                        element = document.createElement('textArea');
                    element.name = "dataSignature";
                    element.style.display="none";
                    element.style.visibility="hidden";
                        form.appendChild(element);
                    }
                    element.value = signObj.SignedMessage;

                    return true;

                }
                else {
                    alert(signObj.ErrorMessage);
                }
            }
        }
        return false;
    }
    

    function ProcessCallBackError(arg, context) {
        alert(arg);
    }

    function CreateSignable() {
        var tmpDOM;

        try {
            tmpDOM = document.all('signable');  //new ActiveXObject("Uxnet.Distribute.Signable");
            if (tmpDOM.toString() != "Uxnet.Distribute.Signable") {
                if (tmpDOM.object != null && tmpDOM.object.toString() == "Uxnet.Distribute.Signable") {
                    tmpDOM = tmpDOM.object;
                }
                else {
                    tmpDOM = new ActiveXObject("Uxnet.Distribute.Signable");
                    if(tmpDOM!=null && tmpDOM.toString() != "Uxnet.Distribute.Signable") {
                        tmpDOM = null;
                    }
                }
            }
        } catch (e) {
            tmpDOM = null;
        }


        if (tmpDOM == null) {
            if (confirm("您尚未安裝網際優勢用戶端電子簽章元件,是否立即下載安裝?")) {
                window.location.href = '<%= VirtualPathUtility.ToAbsolute("~/Published/CAUsageNote.aspx") %>';
            }
        } else {
            tmpDOM.UsePfxFile = <%= this.UsePfxFile ? "true" : "false" %>;
            tmpDOM.Thumbprint = '<%= Thumbprint %>';
            tmpDOM.AppendSignerInfo = true;
        }
        return tmpDOM;
    }
    
    var emptyMsg = '<%= EmptyContentMessage %>';
	
//-->
</script>
<script runat="server">
    internal bool _usePfxFile = true;
    internal Model.Security.MembershipManagement.UserProfileMember _userProfile = Business.Helper.WebPageUtility.UserProfile;
    
    public bool UsePfxFile
    {
        get { return _usePfxFile; }
        set { _usePfxFile = value; }
    }

    public String EmptyContentMessage
    { get; set; }

    public Model.Locale.Naming.CACatalogDefinition Catalog { get; set; }

    protected override bool doVerify()
    {
        String dataSignature = Request["dataSignature"];
        String dataToSign = Request["__dataToSign"];

        if (String.IsNullOrEmpty(dataSignature) || String.IsNullOrEmpty(dataToSign))
        {
            return false;
        }
        
        dataToSign = dataToSign.Replace("\r\n", "\n").Replace("\n", "\r\n");

        var ca = getCryptoUtility();

        if (IsCmsEnveloped)
        {
            byte[] data;
            if (ca.VerifyEnvelopedPKCS7(Convert.FromBase64String(DataSignature), out data))
            {
                SignerCertificate = ca.SignerCertificate;
                return true;
            }
        }
        else
        {
            if (ca.VerifyPKCS7(dataToSign, dataSignature))
            {
                SignerCertificate = ca.SignerCertificate;
                return true;
            }
        }
        return false;
    }

    internal Uxnet.Com.Security.UseCrypto.CryptoUtility getCryptoUtility()
    {
        Uxnet.Com.Security.UseCrypto.CryptoUtility util = new Uxnet.Com.Security.UseCrypto.CryptoUtility { VerifySignatureOnly = true };
        Model.Helper.PKCS7Log log = (Model.Helper.PKCS7Log)util.CA_Log.Table.DataSet;
        log.Catalog = Catalog;
        log.OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID;
        return util;
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Visible = true;
    }
</script>
