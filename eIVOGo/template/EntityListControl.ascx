<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EntityListControl.ascx.cs"
    Inherits="eIVOGo.template.EntityListControl" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc4" %>
<%@ Register Src="~/Module/Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc5" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" EnableViewState="False"
        DataSourceID="dsEntity" ShowFooter="True" DataKeyNames="CompanyID">
        <Columns>
            <asp:TemplateField HeaderText="企業名稱" SortExpression="CompanyName">
                <ItemTemplate>
                    <%# ((EnterpriseGroupMember)Container.DataItem).Organization.CompanyName %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="統一編號" SortExpression="ReceiptNo">
                <ItemTemplate>
                    <%# ((EnterpriseGroupMember)Container.DataItem).Organization.ReceiptNo  %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="負責人姓名" SortExpression="UndertakerName">
                <ItemTemplate>
                    <%# ((EnterpriseGroupMember)Container.DataItem).Organization.UndertakerName%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="電子郵件" SortExpression="ContactEmail">
                <ItemTemplate>
                    <%# ((EnterpriseGroupMember)Container.DataItem).Organization.ContactEmail%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="企業狀態" SortExpression="CurrentLevel">
                <ItemTemplate>
                    <%# ((EnterpriseGroupMember)Container.DataItem).Organization.OrganizationStatus.LevelExpression.Expression %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False" HeaderText="管理">
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Enabled='<%# ((EnterpriseGroupMember)Container.DataItem).Organization.OrganizationStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete%>'
                        Text="修改" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible="<%# ((EnterpriseGroupMember)Container.DataItem).Organization.OrganizationStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="停用" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認停用此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    <asp:Button ID="btnActivate" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible="<%# ((EnterpriseGroupMember)Container.DataItem).Organization.OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="啟用" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doActivate.GetConfirmedPostBackEventReference("確認啟用此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnCASetup" runat="server" CausesValidation="false" Enabled='<%# ((EnterpriseGroupMember)Container.DataItem).Organization.OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete ?false:true%>'
                        Text="建立憑證資訊" CssClass="btn" OnClientClick='<%# String.Format("window.open(\"{0}?companyID={1}\",\"\",\"toolbar=no,scrollbars=1,width=640,height=200\");return false;",VirtualPathUtility.ToAbsolute("~/SAM/CreateCertificateIdentity.aspx"),((EnterpriseGroupMember)Container.DataItem).CompanyID) %>' />
                    <%--&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSignature" runat="server" CausesValidation="False"
                        CommandName="Signature" 
                        Text="設定發票章" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doInvoiceSignature.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />--%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <FooterStyle CssClass="total-count" />
        <PagerStyle HorizontalAlign="Center" />
        <SelectedRowStyle />
        <HeaderStyle />
        <AlternatingRowStyle CssClass="OldLace" />
        <PagerTemplate>
            <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
        <RowStyle />
        <EditRowStyle />
    </asp:GridView>
</div>
<cc1:EnterpriseGroupMemberDataSource ID="dsEntity" runat="server">
</cc1:EnterpriseGroupMemberDataSource>
<uc1:ActionHandler ID="doCreate" runat="server" />
<uc1:ActionHandler ID="doEdit" runat="server" />
<uc1:ActionHandler ID="doDelete" runat="server" />
<uc1:ActionHandler ID="doActivate" runat="server" />
<uc1:ActionHandler ID="doInvoiceSignature" runat="server" />
<uc4:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
<uc5:PageAnchor ID="ToEdit" runat="server" TransferTo="~/SAM/EditCompany.aspx" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        doInvoiceSignature.DoAction = arg =>
        {
            modelItem.DataItem = int.Parse(arg);
            Server.Transfer("~/SAM/ImportInvoiceSignature.aspx");
        };
    }
</script>