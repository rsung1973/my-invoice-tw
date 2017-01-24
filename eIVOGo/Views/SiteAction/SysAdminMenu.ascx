<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<div class="navbar-default sidebar" role="navigation">
    <div class="sidebar-nav navbar-collapse">
        <ul class="nav" id="side-menu">
            <%--<li class="sidebar-search">
                                <div class="input-group custom-search-form">
                                    <input type="text" class="form-control" placeholder="Search...">
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" type="button">
                                            <i class="fa fa-search"></i>
                                        </button>
                                    </span>
                                </div>
                                <!-- /input-group -->
                            </li>--%>
            <li>
                <a href="#"><i class="fa fa-sitemap fa-fw"></i>系統管理維護<span class="fa arrow"></span></a>
                <ul class="nav nav-second-level">
                    <%--<li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/CompanyManager.aspx") %>">店家資料維護</a>
                    </li>--%>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/OrganizationQuery/Index") %>">開立人資料維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/BuyerManager.aspx") %>">買受人資料維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/BusinessRelationship/MaintainRelationship") %>">相對營業人資料維護</a>
                    </li>
                    <%--<li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/SocialWelfareAgenciesManger.aspx") %>">社福機構資料維護</a>
                    </li>--%>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/TrackCode/Index") %>">電子發票字軌維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/WinningNumber/Index") %>">電子發票中獎號碼維護</a>
                    </li>
<%--                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/UnusedInvoiceList.aspx") %>">上期發票空白號碼查詢</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/Inquiry/InquireInvoice.aspx") %>">查詢發票/折讓</a>
                    </li>--%>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/EIVO/WebUploadMailMessage.aspx") %>">登錄掛號郵件號碼</a>
                    </li>
<%--                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/EIVO/WebDownloadMIG.aspx") %>">下載MIG檔案</a>
                    </li>--%>
<%--                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/Inquiry/PrintInvoices.aspx") %>">發票列印</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/ResendInvoicesMail.aspx") %>">重送開立發票通知</a>
                    </li>--%>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/UploadQRCodeKey.aspx") %>">QR Code金鑰維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/queryCALog_list.aspx") %>">查詢憑證使用記錄</a>
                    </li>
                    <li>
                        <a href="#">系統監控 <span class="fa arrow"></span></a>
                        <ul class="nav nav-third-level">
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/SystemErrorLog.aspx") %>">異常記錄查詢</a>
                            </li>
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/SystemMonitor.aspx") %>">用戶端連線狀態</a>
                            </li>
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/CheckSequentialInvoiceNo.aspx") %>">發票號連號檢查</a>
                            </li>
                        </ul>
                        <!-- /.nav-third-level -->
                    </li>
                    <li>
                        <a href="#">TurnKey監控 <span class="fa arrow"></span></a>
                        <ul class="nav nav-third-level">
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/TurnKeyMonitor.aspx") %>">TurnKey傳送狀態</a>
                            </li>
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/SAM/TurnKeyLog.aspx") %>">TurnKey發票傳送紀錄</a>
                            </li>
                        </ul>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/MaintainSystemMessage.aspx") %>">跑馬燈訊息維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SMS/DynamicSendSMSMessage.aspx") %>">客服訊息</a>
                    </li>
                    <li>
                        <a href="#">資料管理 <span class="fa arrow"></span></a>
                        <ul class="nav nav-third-level">
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/DataExchange/Index") %>">資料維護</a>
                            </li>
                            <li>
                                <a href="<%= VirtualPathUtility.ToAbsolute("~/Inquiry/VoidInvoices.aspx") %>">註銷作業</a>
                            </li>
                        </ul>
                    </li>
                </ul>
                <!-- /.nav-second-level -->
            </li>
            <li>
                <a href="#"><i class="fa fa-sitemap fa-fw"></i>發票作業<span class="fa arrow"></span></a>
                <ul class="nav nav-second-level">
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/Index") %>"><i class="fa fa-dashboard fa-fw"></i>資料查詢／列印／匯出</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceBusiness/CreateInvoice") %>"><i class="fa fa-dashboard fa-fw"></i>線上開立發票</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireToCancel") %>"><i class="fa fa-dashboard fa-fw"></i>線上作廢發票</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceNo/VacantNoIndex") %>"><i class="fa fa-dashboard fa-fw"></i>上期發票空白號碼查詢／匯出</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/IssuingNotice") %>"><i class="fa fa-dashboard fa-fw"></i>重送開立發票通知</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireToMIG") %>"><i class="fa fa-dashboard fa-fw"></i>下載MIG檔案</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceProcess/InquireToAuthorize") %>"><i class="fa fa-dashboard fa-fw"></i>核准重印發票</a>
                    </li>
                </ul>
            </li>
            <li>
                <a href="#"><i class="fa fa-sitemap fa-fw"></i>統計報表<span class="fa arrow"></span></a>
                <ul class="nav nav-second-level">
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceQuery/InvoiceReport") %>"><i class="fa fa-dashboard fa-fw"></i>發票明細查詢</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceQuery/InvoiceSummary") %>">發票統計表</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/WinningInvoice/ReportIndex") %>">中獎統計表</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/DonatedInvoice/ReportIndex") %>">捐贈統計表</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceQuery/InvoiceAttachment") %>">附件檔查詢＼下載</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/InvoiceQuery/InvoiceMediaReport") %>">媒體申報檔匯出</a>
                    </li>
                </ul>
            </li>
            <li>
                <a href="#"><i class="fa fa-bar-chart-o fa-fw"></i>系統維護<span class="fa arrow"></span></a>
                <ul class="nav nav-second-level">
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/MaintainUserRoleDefinition.aspx") %>">角色定義維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/MaintainCategoryDefinition.aspx") %>">組織單位類別維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/MaintainMenuControl.aspx") %>">工作選單維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/ApplyUserMenu.aspx") %>">工作選單套用維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/MaintainOrganizationCategory.aspx") %>">公司類別套用維護</a>
                    </li>
                    <li>
                        <a href="<%= VirtualPathUtility.ToAbsolute("~/SYS/MaintainUserProfile.aspx") %>">使用者角色維護</a>
                    </li>
                </ul>
                <!-- /.nav-second-level -->
            </li>
        </ul>
    </div>
    <!-- /.sidebar-collapse -->
</div>


<script runat="server">

    Model.Security.MembershipManagement.UserProfileMember _userProfile;
    String _menuView;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        _userProfile = Business.Helper.WebPageUtility.UserProfile;
        _menuView = "~/Views/SiteAction/" + Path.GetFileNameWithoutExtension(_userProfile.CurrentSiteMenu) + ".ascx";
    }


</script>
