<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagPrefix="uc1" TagName="EnumSelector" %>

<tr>
    <td><%  Html.RenderPartial("~/Views/BusinessRelationship/Module/GroupMemberSelector.ascx",
                new InputViewModel
                {
                    Name = "CompanyID"
                });   %></td>
    <td>
        <input type="text" name="CompanyName" placeholder="請輸入相對營業人名稱" value="" />
    </td>
    <td>
        <input type="text" name="ReceiptNo" placeholder="請輸入統一編號" value="" /></td>
    <td>
        <uc1:EnumSelector runat="server" ID="businessSelector" FieldName="BusinessType" TypeName="Model.Locale.Naming+InvoiceCenterBusinessType, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
    </td>
    <td>
        <input type="text" name="ContactEmail" placeholder="請輸入聯絡人電子郵件" value="" /></td>
    <td>
        <input type="text" name="Addr" placeholder="請輸入地址" value="" /></td>
    <td>
        <input type="text" name="Phone" placeholder="請輸入電話" value="" /></td>
    <td>
        <input type="text" name="CustomerNo" placeholder="請輸入店號" value="" /></td>
    <td>
        <uc1:EnumSelector runat="server" ID="levelSelector" FieldName="CompanyStatus" TypeName="Model.Locale.Naming+BusinessRelationshipStatus, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
    </td>
    <td>
        <a class="btn" onclick="uiInquireBusiness.commitItem();">新增相對營業人</a>
    </td>
</tr>


<script runat="server">

    //Model.DataEntity.DocumentFlow _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //_model = (Model.DataEntity.DocumentFlow)this.Model;
    }

</script>
