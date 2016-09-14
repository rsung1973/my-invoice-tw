<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyManger.ascx.cs" Inherits="eIVOGo.Module.SAM.CompanyManger" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register src="../UI/SocialWelfareSetup.ascx" tagname="SocialWelfareSetup" tagprefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register src="Business/CompanyList.ascx" tagname="CompanyList" tagprefix="uc4" %>

<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc5" %>

<%@ Register src="../Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc6" %>

<%@ Register src="../Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc7" %>

<!--路徑名稱-->


<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 店家資料維護" />
        <!--交易畫面標題-->
        <h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />店家資料維護</h1>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="top_table">
        <tr>
            <td>
                <asp:Button ID="btnAdd" runat="server" class="btn" Text="新增店家" 
                    OnClick="btnAdd_Click" />
            </td>
        </tr>
        </table>
        <div id="border_gray" style="margin-top:5px;">
            <!--表格 開始-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th colspan="2" class="Head_style_a">
                        查詢條件
                    </th>
                </tr>
                <tr>
                    <%--<th nowrap="nowrap">
                        企業角色
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="CategoryID" runat="server" class="textfield">
                            <asp:ListItem>全部</asp:ListItem>
                        </asp:DropDownList>
                    </td>--%>
                    <th nowrap="nowrap" width="120">
                        統編
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="ReceiptNo" class="textfield" runat="server"></asp:TextBox>
                    </td>
                    </tr>
                    <tr>
                    <th nowrap="nowrap width="120">
                        店家名稱
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="CompanyName" class="textfield" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th nowrap="nowrap" width="120">
                        店家狀態
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="CompanyStatus" runat="server">
                    <asp:ListItem Value="">全部</asp:ListItem>
                    <asp:ListItem Value="1103">已啟用</asp:ListItem>
                    <asp:ListItem Value="1101">已停用</asp:ListItem>
                </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <!--表格 結束-->
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn" align="center">
                    <asp:Button ID="btnQuery" runat="server" class="btn" Text="查詢" OnClick="btnQuery_Click" />
                    
                </td>
            </tr>
        </table>
        <uc5:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
        <uc4:CompanyList ID="itemList" runat="server" Visible="false" />
    </ContentTemplate>
</asp:UpdatePanel>
<uc6:PageAnchor ID="ToEdit" runat="server" TransferTo="~/SAM/AddCompany.aspx" RedirectTo="~/SAM/AddCompany.aspx" />
<uc7:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
<script runat="server">
    protected override void btnAdd_Click(object sender, EventArgs e)
    {
        modelItem.DataItem = null;
        Response.Redirect(ToEdit.RedirectTo);
    }
</script>



