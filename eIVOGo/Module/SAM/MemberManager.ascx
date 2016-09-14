<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberManager.ascx.cs"
    Inherits="eIVOGo.Module.SAM.MemberManager" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>


<asp:UpdatePanel ID="Updatepanel1" runat="server">
    <ContentTemplate>
        <!--路徑名稱-->
        <uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 會員管理" />
        <!--交易畫面標題-->
        <h1>
            <img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif"
                width="29" height="28" border="0" align="absmiddle" />會員管理</h1>
        <div id="border_gray">
            <!--表格 開始-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th colspan="5" class="Head_style_a">
                        查詢條件
                    </th>
                </tr>
                <tr>
                    <th nowrap="nowrap">
                        角色
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="RoleID" runat="server">
                            <asp:ListItem>全部</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <th nowrap="nowrap">
                        帳號
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="txtPID" runat="server" class="textfield"></asp:TextBox>
                    </td>
                    <td rowspan="2" align="center" class="Bargain_btn">
                        <asp:Button ID="btnQuery" runat="server" class="btn" OnClick="btnQuery_Click" Text="查詢" />
                    </td>
                </tr>
                <tr>
                    <th nowrap="nowrap">
                        姓名
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="txtUserName" runat="server" class="textfield"></asp:TextBox>
                    </td>
                    <th nowrap="nowrap">
                        會員狀態
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="ddlMemStatus" runat="server">
                            <asp:ListItem>全部</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" Width="100%" BorderWidth="0px"
                CellPadding="0" GridLines="None" AutoGenerateColumns="False" CssClass="table01"
                EnableViewState="False" DataSourceID="dsUserProfile">
                <Columns>
                    <asp:TemplateField HeaderText="公司名稱" SortExpression="CompanyName">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserRole.Count>0? ((UserProfile)Container.DataItem).UserRole[0].OrganizationCategory.Organization.CompanyName:null%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="角色" SortExpression="Role">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserRole.Count>0?((UserProfile)Container.DataItem).UserRole[0].UserRoleDefinition.Role:null%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UserName" HeaderText="會員姓名" SortExpression="UserName"
                        ReadOnly="True" />
                    <asp:BoundField DataField="PID" HeaderText="代號" SortExpression="PID" ReadOnly="True" />
                    <asp:BoundField DataField="EMail" HeaderText="電子郵件" SortExpression="EMail" ReadOnly="True" />
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                                Enabled='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Wait_For_Check?false:true  %>'
                                Text="編輯" CssClass="btn" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("U:{0}",((UserProfile)Container.DataItem).UID)) %>' />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete"
                                Enabled='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?false:true  %>'
                                Text="刪除" CssClass="btn" OnClientClick='<%# "if(confirm(\"確認刪除此筆資料?\")) "+ Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}",((UserProfile)Container.DataItem).UID)) %>' />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSendMail" runat="server" Enabled='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Wait_For_Check?true:false  %>'
                                CssClass="btn" Text="重發確認信" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
                <PagerTemplate>
                    <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </PagerTemplate>
            </asp:GridView>
            <!--表格 結束-->
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn" align="center">
                    <asp:Button ID="btnAdd" runat="server" class="btn" Text="新增帳號" OnClick="btnAdd_Click" />
                </td>
            </tr>
        </table>
        <!--按鈕-->
    </ContentTemplate>
</asp:UpdatePanel>
<cc1:UserProfileDataSource ID="dsUserProfile" runat="server">
</cc1:UserProfileDataSource>
