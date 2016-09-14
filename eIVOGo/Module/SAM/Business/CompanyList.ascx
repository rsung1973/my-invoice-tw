<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompanyList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Business.CompanyList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register src="~/Module/Common/DataModelCache.ascx" tagname="DataModelCache" tagprefix="uc3" %>
<%@ Register src="~/Module/Common/PageAnchor.ascx" tagname="PageAnchor" tagprefix="uc4" %>
<%@ Register  Src="~/Module/SAM/Business/ProxySettingOrganizationList.ascx" TagName="ProxySettingOrganizationList" TagPrefix="uc5" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<%@ Register src="../../UI/SocialWelfareSetup.ascx" tagname="SocialWelfareSetup" tagprefix="uc5" %>

<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" Width="100%" CellPadding="0"
        EnableViewState="false" AutoGenerateColumns="False" CssClass="table01" 
        DataSourceID="dsEntity" DataKeyNames="CompanyID">
        <Columns>
            <asp:BoundField DataField="CompanyName" HeaderText="店家名稱" ReadOnly="True" SortExpression="CompanyName" />
            <asp:BoundField DataField="ReceiptNo" HeaderText="統一編號" ReadOnly="True" SortExpression="ReceiptNo"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="UndertakerName" HeaderText="負責人姓名" ReadOnly="True" SortExpression="UndertakerName"
                ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField DataField="ContactEmail" HeaderText="電子郵件" ReadOnly="True" SortExpression="ContactEmail" />
            <asp:TemplateField HeaderText="社福機構">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).InvoiceWelfareAgencies.Count > 0 ? ((Organization)Container.DataItem).InvoiceWelfareAgencies.First().Organization.CompanyName : null%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="店家狀態">
                <ItemTemplate>
                    <%# ((Organization)Container.DataItem).OrganizationStatus.LevelExpression.Description%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="center" />
            </asp:TemplateField>
<%--             <asp:TemplateField HeaderText="店家類別">
                        <ItemTemplate>
                            <%#
                            CompanyCategory(((Organization)Container.DataItem))
                            %>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
            <asp:TemplateField ShowHeader="False" HeaderText="管理">
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                        Enabled='<%# ((Organization)Container.DataItem).OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete ?false:true%>'
                        Text="編輯" CssClass="btn" OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}", Eval(gvEntity.DataKeyNames[0])))%>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible="<%# ((Organization)Container.DataItem).OrganizationStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="停用" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認停用此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    <asp:Button ID="btnActivate" runat="server" CausesValidation="False" CommandName="Delete"
                        Visible="<%# ((Organization)Container.DataItem).OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete%>"
                        Text="啟用" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doActivate.GetConfirmedPostBackEventReference("確認啟用此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnSetup" runat="server" CausesValidation="false" CommandName="Edit"
                        Enabled='<%# ((Organization)Container.DataItem).OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete ?false:true%>'
                        Text="設定社福機構" CssClass="btn" OnClientClick='<%# doSetup.GetPostBackEventReference(String.Format("{0}", Eval(gvEntity.DataKeyNames[0])))%>' />
                    &nbsp;&nbsp;&nbsp;
                    <%--<input type="button" value="建立憑證資訊" class="btn" onclick='<%# String.Format("window.open(\"{0}?companyID={1}\",\"\",\"toolbar=no,scrollbars=1,width=300,height=200\")",VirtualPathUtility.ToAbsolute("~/SAM/CreateCertificateIdentity.aspx"),((Organization)Container.DataItem).CompanyID) %>' />--%>
                    <asp:Button ID="btnCASetup" runat="server" CausesValidation="false" Enabled='<%# ((Organization)Container.DataItem).OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete ?false:true%>'
                        Text="建立憑證資訊" CssClass="btn" OnClientClick='<%# String.Format("window.open(\"{0}?companyID={1}\",\"\",\"toolbar=no,scrollbars=1,width=640,height=200\")",VirtualPathUtility.ToAbsolute("~/SAM/CreateCertificateIdentity.aspx"),((Organization)Container.DataItem).CompanyID) %>' />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnProxy" runat="server" CausesValidation="false" 
                        Visible ='<%# checkCompany(((Organization)Container.DataItem))%>'
                        Enabled='<%# ((Organization)Container.DataItem).OrganizationStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete ?false:true%>'
                        Text="設定發票代理店家" CssClass="btn" OnClientClick='<%# doProxy.GetPostBackEventReference(((Organization)Container.DataItem).CompanyID.ToString())  %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <AlternatingRowStyle CssClass="OldLace" />
        <PagerStyle BackColor="PaleGoldenrod" HorizontalAlign="Left" BorderStyle="None" CssClass="noborder" />
        <PagerTemplate>
            <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
        <EmptyDataTemplate>
            <div align="center">
                <font color="red">查無資料!!</font></div>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
<cc1:OrganizationDataSource ID="dsEntity" runat="server">
</cc1:OrganizationDataSource>
<uc1:ActionHandler ID="doEdit" runat="server" />
<uc1:ActionHandler ID="doCreate" runat="server" />
<uc1:ActionHandler ID="doSetup" runat="server" />
<uc1:ActionHandler ID="doDelete" runat="server" />
<uc1:ActionHandler ID="doActivate" runat="server" />
<uc1:ActionHandler ID="doProxy" runat="server" />
<uc3:DataModelCache ID="modelItem" runat="server" KeyName="CompanyID" />
<uc4:PageAnchor ID="ToEdit" runat="server" TransferTo="~/SAM/EditCompany.aspx" />
<uc5:SocialWelfareSetup ID="socialWelfare" runat="server" />
<uc5:ProxySettingOrganizationList ID="ProxyOrg" runat="server" Visible="false" EnableViewState="false" />


<script runat="server">

    String CompanyCategory(Model.DataEntity.Organization Org)
    {
        return String.Join("、", Org.OrganizationCategory.Select(c => ((Naming.B2CCategoryID)c.CategoryID).ToString()));
    }
    
    bool checkCompany(Model.DataEntity.Organization Org)
    {
        return Org.OrganizationCategory.Count(c => c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號
            || c.CategoryID == (int)Naming.B2CCategoryID.店家) > 0;
    }
    
</script>