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
                <a href="<%= VirtualPathUtility.ToAbsolute("~/Account/AccountIndex") %>"><i class="fa fa-dashboard fa-fw"></i>使用者管理</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/BusinessRelationship/MaintainRelationship") %>"><i class="fa fa-dashboard fa-fw"></i>相對營業人資料維護</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceNo/MaintainInvoiceNoInterval") %>"><i class="fa fa-dashboard fa-fw"></i>電子發票號碼維護</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/Index") %>"><i class="fa fa-dashboard fa-fw"></i>資料查詢／列印／匯出</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceNo/VacantNoIndex") %>"><i class="fa fa-dashboard fa-fw"></i>上期發票空白號碼查詢／匯出</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceQuery/InvoiceMediaReport") %>"><i class="fa fa-dashboard fa-fw"></i>發票媒體申報檔查詢／匯出</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/IssuingNotice") %>"><i class="fa fa-dashboard fa-fw"></i>重送開立發票通知</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireToAuthorize") %>"><i class="fa fa-dashboard fa-fw"></i>核准重印發票</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireToCancel") %>"><i class="fa fa-dashboard fa-fw"></i>線上作廢發票</a>
            </li>
            <li>
                <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireToMIG") %>"><i class="fa fa-dashboard fa-fw"></i>下載MIG檔案</a>
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
