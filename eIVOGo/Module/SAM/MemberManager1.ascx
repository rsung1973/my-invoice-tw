<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MemberManager1.ascx.cs" Inherits="eIVOGo.Module.SAM.MemberManager1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="Model" Namespace="Model.Locale" TagPrefix="cc2" %>

<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>

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
            <th colspan="4" class="Head_style_a">
                查詢條件
            </th>
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">
                角色
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="RoleID" runat="server">
                    <asp:ListItem>全部</asp:ListItem>
                </asp:DropDownList>
            </td>
            <th nowrap="nowrap" width="120">
                帳號
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtPID" runat="server" class="textfield"></asp:TextBox>
            </td>
            <!--td rowspan="2" align="center" class="Bargain_btn">
                <asp:Button ID="btnQuery" runat="server" class="btn" OnClick="btnQuery_Click" Text="查詢" />
            </td-->
        </tr>
        <tr>
            <th nowrap="nowrap" width="120">
                姓名
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtUserName" runat="server" class="textfield"></asp:TextBox>
            </td>
            <th nowrap="nowrap" width="120">
                會員狀態
            </th>
            <td class="tdleft">
                <asp:DropDownList ID="ddlMemStatus" runat="server">
                    <asp:ListItem>全部</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>
<!--暫時停止會員新增帳號功能-->
<%--<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnAdd" runat="server" class="btn" Text="新增帳號" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>--%>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="Button1" runat="server" class="btn" OnClick="btnQuery_Click" Text="查詢" />
        </td>
    </tr>
</table>
<!--按鈕-->
    <div id="divResult" visible="false" runat="server">
        <h1><img runat="server" enableviewstate="false" id="img4" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />查詢結果</h1>
        <!--表格 開始-->
        <div id="border_gray">
            <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" 
                CssClass="table01" Width="100%" ClientIDMode="Static" 
                BorderWidth="0px" CellPadding="0" GridLines="None" AllowPaging="True" EnableViewState="false" >
                <Columns>
                    <asp:TemplateField HeaderText="公司名稱">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserRole.Count>0? ((UserProfile)Container.DataItem).UserRole[0].OrganizationCategory.Organization.CompanyName:null%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="角色">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserRole.Count>0?((UserProfile)Container.DataItem).UserRole[0].UserRoleDefinition.Role:null%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="會員姓名" DataField="UserName" ReadOnly="True" 
                        SortExpression="UserName" />
                    <asp:BoundField HeaderText="代號" DataField="PID" ReadOnly="True" 
                        SortExpression="PID" />
                    <asp:BoundField HeaderText="電子郵件" DataField="EMail" ReadOnly="True" 
                        SortExpression="EMail" />
                    <asp:TemplateField HeaderText="會員狀態">
                        <ItemTemplate>
                            <%# ((UserProfile)Container.DataItem).UserProfileStatus.LevelExpression.Description%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="管理">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CssClass="btn" Text="編輯" Enabled='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?false:true  %>'
                                onclientclick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("U:{0}",((UserProfile)Container.DataItem).UID)) + "; return false;" %>' />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnDelete" runat="server" CssClass="btn" Text='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?"啟用":"停用" %>' Enabled='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Wait_For_Check?false:true  %>'
                                onclientclick='<%# String.Format("if(confirm(\"確認{0}此筆資料?\")) {1} ", ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Mark_To_Delete?"啟用":"停用", Page.ClientScript.GetPostBackEventReference(this, String.Format("D:{0}",((UserProfile)Container.DataItem).UID)))  + "; return false;"%>' />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSendMail" runat="server" CssClass="btn" Text="重送確認信" Enabled='<%# ((UserProfile)Container.DataItem).UserProfileStatus.CurrentLevel==(int)Naming.MemberStatusDefinition.Wait_For_Check?true:false  %>'
                                onclientclick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("M:{0}",((UserProfile)Container.DataItem).UID)) + "; return false;" %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
                <PagerTemplate>
                    <uc2:PagingControl ID="pagingList" runat="server" />
                </PagerTemplate>
            </asp:GridView>
            <center>
            <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
            </center>
        </div>
    </div>

<cc1:UserProfileDataSource ID="dsUserProfile" runat="server"></cc1:UserProfileDataSource>
