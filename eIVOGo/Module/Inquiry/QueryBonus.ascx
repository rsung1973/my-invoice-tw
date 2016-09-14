<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBonus.ascx.cs" Inherits="eIVOGo.Module.Inquiry.QueryBonus" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc2" %>
<%@ Register src="../Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc1" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc3" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc4" %>
<%@ Register src="../EIVO/PNewInvalidInvoicePreview.ascx" tagname="PNewInvalidInvoicePreview" tagprefix="uc5" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc3:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 查詢中獎清冊" />
        <!--交易畫面標題-->
        <uc4:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="查詢中獎清冊" />
        <div id="border_gray">
        <!--表格 開始-->
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th colspan="2" class="Head_style_a">查詢條件</th>
            </tr>
            <div id="uxb2b" visible="false" runat="server">
            <tr>
                <th>查詢類別</th>
                <td class="tdleft">
                    <asp:RadioButton ID="rdbType1" Checked="true" GroupName="R2" Text="依會員" AutoPostBack="true"
                        runat="server" oncheckedchanged="rdbType_CheckedChanged" />
                    &nbsp;
                    <asp:RadioButton ID="rdbType2" GroupName="R2" Text="依載具" AutoPostBack="true"
                        runat="server" oncheckedchanged="rdbType_CheckedChanged" />
                    <asp:Label ID="lblDevice" Visible="false" runat="server" Text="依載具"></asp:Label>
                    <asp:DropDownList ID="ddlDevice" CssClass="textfield" Visible="false" runat="server" 
                        AutoPostBack="True" onselectedindexchanged="ddlDevice_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>UXB2B條碼卡號</th>
                <td class="tdleft">
                    <asp:TextBox ID="txtUxb2bBarCode" CssClass="textfield" Width="100" runat="server"></asp:TextBox>
                  （共20碼）</td>
            </tr>
            </div>
            <tr>
                <th width="20%">日期區間</th>
                <td class="tdleft">
                    <asp:DropDownList ID="ddlRange1" runat="server"></asp:DropDownList>
                    &nbsp;~&nbsp;<asp:DropDownList ID="ddlRange2" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>顯示捐贈</th>
                <td class="tdleft">
                    <asp:RadioButton ID="rdbAll" GroupName="V1" Text="全部（含未捐贈）" Checked="true" CssClass="R3" runat="server" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RadioButton ID="rdbDonate" GroupName="V1" Text="只顯示捐贈發票" CssClass="R3" runat="server" />
                </td>
            </tr>
        </table>
        <!--表格 結束-->
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td class="Bargain_btn">
                <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" onclick="btnQuery_Click" />
            </td>
          </tr>
        </table>

        <div id="divResult" visible="false" runat="server">
        <uc4:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
        <!--表格 開始-->
        <div id="border_gray">
                    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%" GridLines="None" CellPadding="0" CssClass="table01" EnableViewState="false" AllowPaging="True" >
                    <Columns>
                        <asp:TemplateField HeaderText="期別" > <ItemTemplate><%# ((dataType)Container.DataItem).range%></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="發票號碼" > <ItemTemplate>
                            <asp:LinkButton ID="lbtn" runat="server" Text='<%# String.Format("{0}{1}",((dataType)Container.DataItem).TrackCode,((dataType)Container.DataItem).DonateMark.Equals("0") ? ((dataType)Container.DataItem).No : ((dataType)Container.DataItem).No.Substring(0,5)+"***")%>' 
                             CausesValidation="false" CommandName="Edit" OnClientClick='<%# Page.ClientScript.GetPostBackEventReference(this, String.Format("S:{0}",((dataType)Container.DataItem).id)) + "; return false;" %>' />
                            </ItemTemplate>  
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="開立發票營業人" > <ItemTemplate><%# ((dataType)Container.DataItem).CompanyName%></ItemTemplate>  
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="營業地址" > <ItemTemplate><%# ((dataType)Container.DataItem).Addr%></ItemTemplate>  
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="載具種類" > <ItemTemplate><%# ((dataType)Container.DataItem).CarrierType%></ItemTemplate>  
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="捐贈單位" > <ItemTemplate><%# ((dataType)Container.DataItem).Donation%></ItemTemplate>  
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>                 
                    </Columns>         
                    <FooterStyle />
                    <PagerStyle HorizontalAlign="right" />
                    <SelectedRowStyle />
                    <HeaderStyle />
                    <AlternatingRowStyle CssClass="OldLace" />
                        <PagerTemplate>
                            <span>
                            <uc2:PagingControl ID="pagingIndex" runat="server" />
                            </span>
                        </PagerTemplate>
                    <RowStyle />
                    <EditRowStyle />
                    </asp:GridView>
                    <center>
                    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
                    </center>
        <!--表格 結束-->
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn">
                    <uc1:PrintingButton2 ID="PrintingButton21" runat="server" Visible="false" />
                </td>
            </tr>
        </table>
        </div>
        <uc5:PNewInvalidInvoicePreview ID="PNewInvalidInvoicePreview1" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rdbType1" EventName="CheckedChanged" />
        <asp:AsyncPostBackTrigger ControlID="rdbType2" EventName="CheckedChanged" />
        <asp:AsyncPostBackTrigger ControlID="ddlDevice" EventName="SelectedIndexChanged" />
        <asp:PostBackTrigger ControlID="btnQuery" />
        <asp:PostBackTrigger ControlID="gvEntity" />
        <asp:PostBackTrigger ControlID="PrintingButton21" />
    </Triggers>
</asp:UpdatePanel>