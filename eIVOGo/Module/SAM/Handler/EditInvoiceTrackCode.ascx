<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditInvoiceTrackCode.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Handler.EditInvoiceTrackCode" %>
<%@ Register Src="../../UI/TrackCodeYearSelector.ascx" TagName="TrackCodeYearSelector"
    TagPrefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<div class="border_gray">
    <asp:FormView ID="FormView1" runat="server" DataKeyNames="TrackID" 
        DefaultMode="Insert" DataSourceID="dsTrack" 
        oniteminserted="FormView1_ItemInserted" 
        oniteminserting="FormView1_ItemInserting" onitemupdated="FormView1_ItemUpdated" 
        onitemupdating="FormView1_ItemUpdating" Width="100%" 
        onitemcommand="FormView1_ItemCommand">
        <EditItemTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th colspan="2" class="Head_style_a">
                        修改發票字軌
                    </th>
                </tr>
                <tr>
                    <th width="20%" nowrap="nowrap">
                        <span class="red">*</span> 發票年度（民國年）
                    </th>
                    <td class="tdleft">
                        <uc1:TrackCodeYearSelector ID="TrackCodeYear" runat="server" SelectedValue='<%# Bind("Year") %>' />
                    </td>
                </tr>
                <tr>
                    <th>
                        <span class="red">*</span> 發票期別
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="PeriodNo" runat="server" SelectedValue='<%# Bind("PeriodNo") %>'>
                            <asp:ListItem Value="1">01~02月</asp:ListItem>
                            <asp:ListItem Value="2">03~04月</asp:ListItem>
                            <asp:ListItem Value="3">05~06月</asp:ListItem>
                            <asp:ListItem Value="4">07~08月</asp:ListItem>
                            <asp:ListItem Value="5">09~10月</asp:ListItem>
                            <asp:ListItem Value="6">11~12月</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <span class="red">*</span> 發票字軌
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="TrackCodeTextBox" runat="server" Columns="2" MaxLength="2" Text='<%# Bind("TrackCode") %>' />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TrackCodeTextBox"
                            ErrorMessage="字軌請輸入兩個英文字母!!" ForeColor="#FF3300" ValidationExpression="([A-Z]|[a-z]){2}"
                            ViewStateMode="Enabled" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TrackCodeTextBox"
                            Display="Dynamic" ErrorMessage="字軌請輸入兩個英文字母!!" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnUpdate" runat="server" Text="確定" CommandName="Update" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="取消" CommandName="Cancel" />
                    </td>
                </tr>
            </table>
        </EditItemTemplate>
        <InsertItemTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th colspan="2" class="Head_style_a">
                        新增發票字軌
                    </th>
                </tr>
                <tr>
                    <th width="20%" nowrap="nowrap">
                        <span class="red">*</span> 發票年度（民國年）
                    </th>
                    <td class="tdleft">
                        <uc1:TrackCodeYearSelector ID="TrackCodeYear" runat="server" SelectedValue='<%# Bind("Year") %>' />
                    </td>
                </tr>
                <tr>
                    <th>
                        <span class="red">*</span> 發票期別
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="PeriodNo" runat="server" SelectedValue='<%# Bind("PeriodNo") %>'>
                            <asp:ListItem Value="1">01~02月</asp:ListItem>
                            <asp:ListItem Value="2">03~04月</asp:ListItem>
                            <asp:ListItem Value="3">05~06月</asp:ListItem>
                            <asp:ListItem Value="4">07~08月</asp:ListItem>
                            <asp:ListItem Value="5">09~10月</asp:ListItem>
                            <asp:ListItem Value="6">11~12月</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th>
                        <span class="red">*</span> 發票字軌
                    </th>
                    <td class="tdleft">
                        <asp:TextBox ID="TrackCodeTextBox" runat="server" Columns="2" MaxLength="2" Text='<%# Bind("TrackCode") %>' />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TrackCodeTextBox"
                            ErrorMessage="字軌請輸入兩個英文字母!!" ForeColor="#FF3300" ValidationExpression="([A-Z]|[a-z]){2}"
                            ViewStateMode="Enabled" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TrackCodeTextBox"
                            Display="Dynamic" ErrorMessage="字軌請輸入兩個英文字母!!" ForeColor="#FF3300"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAddCode" runat="server" Text="確定" CommandName="Insert" CausesValidation="true" />
                        <input id="btnCancel" type="reset" value="重填" class="btn" />
                    </td>
                </tr>
            </table>
        </InsertItemTemplate>
    </asp:FormView>
</div>
<cc1:InvoiceTrackCodeDataSource ID="dsTrack" runat="server">
</cc1:InvoiceTrackCodeDataSource>

