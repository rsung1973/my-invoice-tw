<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditBuyer.ascx.cs" Inherits="eIVOGo.Module.SAM.EditBuyer" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc2" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>

<!--路徑名稱-->


<div id="mainpage" runat="server">
    <uc2:PageAction ID="actionItem" runat="server" ItemName="買受人資料" />
    <!--交易畫面標題-->
    <h1>
        <img id="img4" runat="server" enableviewstate="false" src="~/images/icon_search.gif"
            width="29" height="28" border="0" align="absmiddle" />買受人資料</h1>
    <div id="border_gray">
<!--表格 開始-->
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <td colspan="4" class="Head_style_a">
                    買受人基本資料
                </td>
            </tr>
            <tr>
                <th width="150">
                    客戶名稱
                </th>
                <td class="tdleft">
                    <%# _entity.CustomerName %>
                </td>
                <th nowrap="nowrap" width="150">
                    統一編號
                </th>
                <td class="tdleft">
                    <%# _entity.ReceiptNo.Equals("0000000000") ? "" : _entity.ReceiptNo%>
                </td>
            </tr>
            <tr>
                <th width="150">
                    聯絡人姓名
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="ContactName" runat="server" Text='<%# _entity.ContactName %>' />
                </td>
                <th width="150">
                    聯絡電話
                </th>
                <td class="tdleft">
                    <asp:TextBox ID="Phone" runat="server" Text='<%# _entity.Phone %>' />
                </td>
            </tr>
            <tr>
                <th width="150">
                    地址
                </th>
                <td colspan="3" class="tdleft">
                    <asp:TextBox ID="Addr" Columns="68" runat="server" Text='<%# _entity.Address %>' />
                </td>
            </tr>
        </table>
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn" align="center">
                <asp:Button ID="UpdateButton" runat="server" CausesValidation="True" CssClass="btn" CommandName="Update" Text="確定" />
                &nbsp;
                <input name="Reset" type="reset" class="btn" value="重填" />
            </td>
        </tr>
    </table>
</div>

<cc1:InvoiceBuyerDataSource ID="dsEntity" runat="server">
</cc1:InvoiceBuyerDataSource>
<uc1:DataModelCache ID="modelItem" runat="server" KeyName="InvoiceID" />
<uc3:ActionHandler ID="doConfirm" runat="server" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        UpdateButton.OnClientClick = doConfirm.GetPostBackEventReference(null);
    }
</script>