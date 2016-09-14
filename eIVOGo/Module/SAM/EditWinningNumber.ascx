<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditWinningNumber.ascx.cs" Inherits="eIVOGo.Module.SAM.EditWinningNumber" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register src="../UI/DynamicAddTextbox.ascx" tagname="DynamicAddTextbox" tagprefix="uc2" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc3" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <!--路徑名稱-->
        <uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增中獎號碼" />
        <!--交易畫面標題-->
        <uc3:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="新增中獎號碼" />
        <div id="border_gray">
        <!--表格 開始-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
              <tr>
                <th colspan="2" class="Head_style_a">新增中獎號碼</th>
                </tr>
              <tr>
                <th nowrap="nowrap"><span class="red">*</span> 發票年度（民國年）</th>
                <td class="tdleft">
                    <asp:DropDownList ID="ddlYear" runat="server"></asp:DropDownList>
                </td>
              </tr>
              <tr>
                <th><span class="red">*</span> 發票期別</th>
                <td class="tdleft">
                    <asp:DropDownList ID="ddlRange" runat="server"></asp:DropDownList>
                </td>
              </tr>
              <tr>
                <th>特別獎</th>
                <td class="tdleft">
                    <asp:TextBox ID="txtSpecialPrize" CssClass="textfield" runat="server" Width="80"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <th width="20%"><span class="red">*</span> 特獎</th>
                <td class="tdleft">
                    <asp:TextBox ID="txtGrandPrize" CssClass="textfield" runat="server" Width="80"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <th><span class="red">*</span> 頭獎</th>
                <td class="tdleft">                    
                    <uc2:DynamicAddTextbox ID="DynamicAddTextbox1" runat="server" />                    
                </td>
              </tr>
              <tr>
                <th>增開六獎</th>
                <td class="tdleft">
                    <uc2:DynamicAddTextbox ID="DynamicAddTextbox2" runat="server" />
                </td>
              </tr>
            </table>
        <!--表格 結束-->
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnInsert" Text="確定" CssClass="btn" runat="server" 
                    onclick="btnInsert_Click" />&nbsp;
                <asp:Button ID="btnReset" Text="重填" CssClass="btn" runat="server" 
                    onclick="btnReset_Click" />
            </td>
          </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnInsert" />
        <asp:PostBackTrigger ControlID="btnReset" />
    </Triggers>
</asp:UpdatePanel>