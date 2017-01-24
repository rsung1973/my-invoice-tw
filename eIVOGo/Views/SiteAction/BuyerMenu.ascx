<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<div class="navbar-default sidebar" role="navigation">
    <div class="sidebar-nav navbar-collapse">
        <ul class="nav" id="side-menu">
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/UserProfile/EditMySelf") %>"><i class="fa fa-dashboard fa-fw"></i>帳號管理</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireForIncoming") %>"><i class="fa fa-dashboard fa-fw"></i>資料查詢／列印／匯出</a>
            </li>
        </ul>
    </div>
    <!-- /.sidebar-collapse -->
</div>


<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }


</script>
