<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc3" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagName="EnumSelector" TagPrefix="uc6" %>
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "相對營業人資料維護"); %>
<table class="top_table" border="0" cellspacing="0" cellpadding="0" width="100%">
    <tbody>
        <tr>
            <td>
                <button ID="btnAdd" type="button" class="btn" onclick="window.location.href='<%= Url.Action("ImportCounterpartBusiness","BusinessRelationship") %>';" >匯入相對營業人</button>
            </td>
        </tr>
    </tbody>
</table>
<div class="border_gray">
    <!--表格 開始-->
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    查詢條件
                </th>
            </tr>
            <%  if (_profile.IsSystemAdmin())
            { %>
            <tr>
                <th>
                    集團成員
                </th>
                <td class="tdleft">
                    <%  ViewBag.SelectAll = true;
                        Html.RenderPartial("~/Views/BusinessRelationship/Module/GroupMemberSelector.ascx");   %>
                </td>
            </tr>
            <%  } %>
            <tr>
                <th width="120">
                    營業人統編
                </th>
                <td class="tdleft">
                    <input name="ReceiptNo" type="text" />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th width="120">
                    <span class="tdleft">營業人名稱</span>
                </th>
                <td class="tdleft">
                    <input name="CompanyName" type="text" />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th width="120">
                    營業人狀態
                </th>
                <td class="tdleft">
                    <select name="CompanyStatus">
                        <option Value="">全部</option>
                        <option Value="1103">已啟用</option>
                        <option Value="1101">已停用</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th width="120">
                    營業人類別
                </th>
                <td class="tdleft">
                    <uc6:EnumSelector ID="BusinessType" FieldName="BusinessType" runat="server" SelectorIndication="全部" TypeName="Model.Locale.Naming+InvoiceCenterBusinessQueryType, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
                </td>
            </tr>
            <%--<tr>
                <th width="120">
                    自動接收
                </th>
                <td class="tdleft">
                    <select name="Entrusting">
                        <option value="">全部</option>
                        <option value="True">已啟用</option>
                        <option value="False">已停用</option>
                    </select>
                </td>
            </tr>
                        <tr>
                <th width="120">
                   主動列印
                </th>
                <td class="tdleft">
                    <select name="">
                        <option value="EntrustToPrint">全部</option>
                        <option value="True">已啟用</option>
                        <option value="False">已停用</option>
                    </select>
                </td>
            </tr>--%>
        </tbody>
    </table>
    <!--表格 結束-->
</div>
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiInquireBusiness.inquireBusiness();" >查詢</button>
            </td>
        </tr>
    </tbody>
</table>
<script>
    var uiInquireBusiness;
    $(function () {

        uiInquireBusiness = {
            $result: null,
            commitItem: function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("CommitItem","BusinessRelationship") %>', $.param($tr.find('input,select,textarea')), function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            if (value) {
                                alert('資料已更新!!');
                                $tr.remove();
                            }
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            },
            edit: function (value) {
                actionHandler('<%= VirtualPathUtility.ToAbsolute("~/Helper/EditCompany.aspx") %>',
                    { 'companyID': value },
                    function () {
                        uiInquireBusiness.inquireBusiness();
                    }, 800, 560);
            },
            inquireBusiness: function (pageNum, onPaging) {
                $('form').ajaxForm({
                    url: "<%= Url.Action("InquireBusinessRelationship","BusinessRelationship") %>" + "?pageIndex=" + pageNum,
                    beforeSubmit: function () {
                    },
                    success: function (data) {
                        if (data) {
                            if (onPaging) {
                                onPaging(data);
                            } else {
                                if (uiInquireBusiness.$result)
                                    uiInquireBusiness.$result.remove();
                                uiInquireBusiness.$result = $(data);
                                $('.queryAction').after(uiInquireBusiness.$result);
                            }
                        }
                    },
                    error: function () {
                    }
                }).submit();
            },
            deleteItem: function (businessID, masterID, relativeID) {
                if (confirm('確認刪除此營業人關係?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("DeleteItem","BusinessRelationship") %>', { businessID: businessID, masterID: masterID, relativeID: relativeID }, function (data) {
                        if (data.result) {
                            alert('資料已刪除!!')
                            $tr.remove();
                        } else {
                            alert(data.message);
                        }
                    });

                }
            },
            entrustToPrint: function (businessID, masterID, relativeID, onoff) {
                if (confirm('確認停用列印?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("SetEntrustToPrint","BusinessRelationship") %>', { businessID: businessID, masterID: masterID, relativeID: relativeID, status: onoff }, function (data) {
                        if (data) {
                            var $data = $(data);
                            if ($data.is('tr')) {
                                $tr.before($data);
                                alert('資料已更新!!');
                                $tr.remove();
                            } else {
                                $('body').append($data);
                                $data.remove();
                            }
                        }
                    });

                }
            },
            entrusting: function (businessID, masterID, relativeID, onoff) {
                if (confirm('確認啟用此營業人自動接收?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("SetEntrusting","BusinessRelationship") %>', { businessID: businessID, masterID: masterID, relativeID: relativeID, status: onoff }, function (data) {
                        if (data) {
                            var $data = $(data);
                            if ($data.is('tr')) {
                                $tr.before($data);
                                alert('資料已更新!!');
                                $tr.remove();
                            } else {
                                $('body').append($data);
                                $data.remove();
                            }
                        }
                    });

                }
            },
            activate: function (businessID, masterID, relativeID) {
                if (confirm('確認啟用此營業人?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("Activate","BusinessRelationship") %>', { businessID: businessID, masterID: masterID, relativeID: relativeID }, function (data) {
                        if (data) {
                            var $data = $(data);
                            if ($data.is('tr')) {
                                $tr.before($data);
                                alert('資料已更新!!');
                                $tr.remove();
                            } else {
                                $('body').append($data);
                                $data.remove();
                            }
                        }
                    });

                }
            },
            deactivate: function (businessID, masterID, relativeID) {
                if (confirm('確認停用此營業人?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("Deactivate","BusinessRelationship") %>', { businessID: businessID, masterID: masterID, relativeID: relativeID }, function (data) {
                        if (data) {
                            var $data = $(data);
                            if ($data.is('tr')) {
                                $tr.before($data);
                                alert('資料已更新!!');
                                $tr.remove();
                            } else {
                                $('body').append($data);
                                $data.remove();
                            }
                        }
                    });
                }
            },

            inquireUser: function (value) {
                $.post('<%= Url.Action("AccountIndex","Account",new { showTab = true, roleID = (int)Naming.EIVOUserRoleID.買受人 }) %>', { 'sellerID': value }, function (data) {
                    $global.createTab('<%= "listUser"+DateTime.Now.Ticks %>', '使用者管理', data, true);
                });
            },
        };
    });
</script>
<script runat="server">

    ModelSource<InvoiceItem> models;
    Model.Security.MembershipManagement.UserProfileMember _profile;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _profile = Business.Helper.WebPageUtility.UserProfile;
    }
</script>
