<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc3" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<%@ Register Src="~/Module/SAM/Business/ProxySettingOrganizationList.ascx" TagName="ProxySettingOrganizationList" TagPrefix="uc5" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<table class="table table-small-font table-bordered table-striped table01 companyList">
    <thead>
        <tr>
            <th style="min-width: 250px">店家名稱</th>
            <th style="min-width: 80px">統一編號</th>
            <th style="min-width: 120px">負責人姓名</th>
            <th style="min-width: 250px">電子郵件</th>
            <%--<th style="min-width: 200px">社福機構</th>--%>
            <th style="min-width: 80px">店家狀態</th>
            <th style="min-width: 150px">管理</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            foreach (var item in _model)
            {
                idx++;%>
        <tr>
            <td><%= item.CompanyName %></td>
            <td><%= item.ReceiptNo %></td>
            <td><%= item.UndertakerName %></td>
            <td><pre><%= item.ContactEmail %></pre></td>
            <%--<td><%  var welfare = item.InvoiceWelfareAgencies.FirstOrDefault();
                    if (welfare != null)
                        Writer.Write(welfare.Organization.CompanyName);%>
            </td>--%>
            <td><%= item.OrganizationStatus!=null ? item.OrganizationStatus.LevelExpression.Description : null %></td>
            <td>
                <div class="btn-group <%= idx>1 ? "dropup" : "dropdown" %>" data-toggle="dropdown">
                    <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
                    <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
                    <ul class="dropdown-menu">
                        <%  if (item.OrganizationStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                            { %>
                                <li><a class="btn" onclick="editCompany(<%= item.CompanyID %>);">編輯</a></li>
                                <li><a class="btn" onclick="disableCompany(<%= item.CompanyID %>);">停用</a></li>
                                <%--<li><a class="btn" onclick="applyWelfare(<%= item.CompanyID %>);">設定社福機構</a></li>--%>
                                <li><a class="btn" onclick="applyCertificate(<%= item.CompanyID %>);">建立憑證資訊</a></li>
                            <%  if (checkCompany(item))
                                { %>
                                    <li><a class="btn" onclick="applyAgency(<%= item.CompanyID %>);">設定發票代理店家</a></li>
                            <%  } %>
                                <li><a class="btn" onclick="applyPOS(<%= item.CompanyID %>);">設定POS機號</a></li>
                            <%  if (!item.IsEnterpriseGroupMember())
                                { %>
                                <li><a class="btn" onclick="applyRelationship(<%= item.CompanyID %>);">設定為B2B營業人</a></li>
                            <%  } %>
                                <li><a class="btn" onclick="inquireUser(<%= item.CompanyID %>);">管理使用者</a></li>
                        <%  }
                            else
                            { %>
                                <li><a class="btn" onclick="enableCompany(<%= item.CompanyID %>);">啟用</a></li>
                        <%  } %>
                    </ul>
                </div>

            </td>
        </tr>
        <%  } %>
    </tbody>
    <%  if (_model.Count() == 1)
        { %>
    <tfoot>
        <tr>
            <td colspan="7" style="height:200px;"> &nbsp;</td>
        </tr>
    </tfoot>
    <%  } %>
</table>



<script runat="server">

    IEnumerable<Organization> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = ((IEnumerable<Organization>)this.Model).Skip((int)ViewBag.PageIndex * Uxnet.Web.Properties.Settings.Default.PageSize).Take(Uxnet.Web.Properties.Settings.Default.PageSize);
    }

    bool checkCompany(Model.DataEntity.Organization Org)
    {
        return Org.OrganizationCategory.Count(c => c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號
            || c.CategoryID == (int)Naming.B2CCategoryID.店家) > 0;
    }

</script>
