<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaImg.ascx.cs" Inherits="eIVOGo.Module.UI.CaptchaImg" %>
<img runat="server" id="imgNum" enableviewstate="false" src="~/Published/CaptchaImg.ashx"  align="absmiddle" /> 
<asp:TextBox ID="txtImgID" Text="" Width="100px" runat="server" ></asp:TextBox>
請輸入圖片中的文字或數字
<script runat="server">
///底下程式供測試時自動輸入驗證碼,上線後請刪除...
/// <summary>
/// 
/// </summary>
/// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += new EventHandler(module_ui_captchaimg_ascx_PreRender);
    }

    void module_ui_captchaimg_ascx_PreRender(object sender, EventArgs e)
    {
        txtImgID.Text = ValidCode;
    }
     
</script>