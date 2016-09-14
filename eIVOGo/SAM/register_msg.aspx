<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/UI/RegisterMessage.ascx" tagname="RegisterMessage" tagprefix="uc1" %>
<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:RegisterMessage ID="msgItem" runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(sam_register_msg_aspx_Load);
    }

    void sam_register_msg_aspx_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request["msg"]))
        {
            msgItem.Message = Request["msg"];
        }
        if (!String.IsNullOrEmpty(Request["action"]))
        {
            msgItem.ActionName = Request["action"];
        }
        if (!String.IsNullOrEmpty(Request["back"]))
        {
            msgItem.GoBackText = Request["back"];
        }
        if (!String.IsNullOrEmpty(Request["backUrl"]))
        {
            msgItem.GoBackUrl = Request["backUrl"];
        }
    }
</script>
