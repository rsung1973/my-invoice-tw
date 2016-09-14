<%@ Page Language="C#" AutoEventWireup="true" ContentType="application/javascript" %>
<%@ OutputCache Duration="3600" VaryByParam="none" %>

function showInvoiceModal(docID) {
    var element = event.target;
    $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowInvoiceModal.aspx")%>', 'docID=' + docID, function (html) {
    //            $(element).after($(html).find("#mainContent"));
        $(html).find("#mainContent").dialog({ width: 640 });
    });
}

function showAllowanceModal(docID) {
    var element = event.target;
    $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowAllowanceModal.aspx")%>', 'docID=' + docID, function (html) {
        $(html).find("#mainContent").dialog({ width: 640 });
    });
}
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
       
        Response.Cache.SetCacheability(HttpCacheability.Public);
        //Response.StatusCode = 304;
    }
</script>