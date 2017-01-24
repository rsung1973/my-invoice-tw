<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Business.Helper" %>
<%@ Import Namespace="Utility" %>

<img id="imgNum" src="<%= Url.Action("CaptchaImg", "Account", new { code = _encryptedCode }) %>"  align="absmiddle" /> 
<input name="ValidCode" type="text" value="" placeholder="請輸入圖片中的文字或數字" />
<input name="EncryptedCode" type="hidden" value="<%= _encryptedCode %>" />
<script runat="server">

    String _validCode;
    String _encryptedCode;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _validCode = ValueValidity.CreateRandomStringCode(6);
        _encryptedCode = Convert.ToBase64String(AppResource.Instance.Encrypt(Encoding.Default.GetBytes(_validCode)));
    }


</script>