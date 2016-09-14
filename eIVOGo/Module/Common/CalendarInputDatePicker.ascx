<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.Common.CalendarInputDatePicker" %>
<asp:TextBox ID="txtDate" CssClass="textfield" runat="server" Columns="10" ReadOnly="True" /><asp:Label
    ID="errorMsg" runat="server" Text="請選擇日期!!" Visible="false" EnableViewState="false"
    ForeColor="Red"></asp:Label>
    
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(module_common_calendarinputdatepicker_ascx_Load);
    }

    void module_common_calendarinputdatepicker_ascx_Load(object sender, EventArgs e)
    {

        //if (Page.Header.FindControl("uiCSS") == null)
        //{
        //    HtmlLink uiCSS = new HtmlLink();
        //    uiCSS.ID = "uiCSS";
        //    uiCSS.Href = "http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css";
        //    uiCSS.Attributes["rel"] = "stylesheet";
        //    uiCSS.Attributes["type"] = "text/css";
        //    Page.Header.Controls.Add(uiCSS);
        //}
        
        //Page.ClientScript.RegisterClientScriptInclude("jQuery", VirtualPathUtility.ToAbsolute("~/Scripts/jquery.min.js"));
        //Page.ClientScript.RegisterClientScriptInclude("jQueryUI",VirtualPathUtility.ToAbsolute("~/Scripts/jquery-ui.min.js"));
        //Page.ClientScript.RegisterClientScriptInclude("locale",VirtualPathUtility.ToAbsolute("~/Scripts/jquery.ui.datepicker-zh-TW.js"));

        StringBuilder sb = new StringBuilder();
        sb.Append(@"$(document).ready(function () {
          Sys.Application.add_init(appl_init);
          Sys.WebForms.PageRequestManager.getInstance().add_endRequest(appl_init);
          function appl_init() {
          $.datepicker.setDefaults($.datepicker.regional['zh-tw']);");
        sb.Append(@"
          $(""#").Append(txtDate.ClientID).Append("\").datepicker({showButtonPanel: true, changeYear: true, changeMonth: true, yearRange:'2012:+0'}); }});\r\n");

        Page.ClientScript.RegisterStartupScript(this.GetType(), txtDate.ClientID, sb.ToString(), true);
            
    }
</script>