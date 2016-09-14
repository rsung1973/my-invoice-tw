<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintInvoice.ascx.cs" Inherits="eIVOGo.Module.Inquiry.PrintInvoice" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker" TagPrefix="uc1" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Src="../EIVO/PNewInvalidInvoicePreview.ascx" TagName="PNewInvalidInvoicePreview" TagPrefix="uc4" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<script type="text/javascript">
    $(document).ready(
        function () {
            var chkBox = $("input[id$='chkAll']");
            chkBox.click(
            function () {
                $("#gvEntity INPUT[type='checkbox']")
                .attr('checked', chkBox
                .is(':checked'));
            });

            // To deselect CheckAll when a GridView CheckBox        
            // is unchecked        
            $("#gvEntity INPUT[type='checkbox']").click(
            function (e) {
                if (!$(this)[0].checked) {
                    chkBox.attr("checked", false);
                }
            });
        });
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 發票列印" />
        <!--交易畫面標題-->
        <uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="發票列印" />
        <div id="border_gray">
            <!--表格 開始-->
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th colspan="2" class="Head_style_a">
                        查詢條件
                    </th>
                </tr>
                <tr>
                    <th>
                        發票類別
                    </th>
                    <td class="tdleft">
                        <asp:RadioButtonList ID="rdbInvoiceType" RepeatColumns="5" RepeatDirection="Horizontal"
                            runat="server" RepeatLayout="Flow" OnSelectedIndexChanged="rbdInvoiceType_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="Item" visible="false" runat="server">
                    <th>
                        查詢項目
                    </th>
                    <td class="tdleft">
                        <asp:RadioButtonList ID="rdbSearchItem" RepeatColumns="5" RepeatDirection="Horizontal"
                            runat="server" RepeatLayout="Flow" OnSelectedIndexChanged="rdbSearchItem_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr id="Type" visible="false" runat="server">
                    <th>
                        查詢類別
                    </th>
                    <td class="tdleft">
                        <asp:RadioButtonList ID="rdbPriceType" RepeatColumns="5" RepeatDirection="Horizontal"
                            runat="server" RepeatLayout="Flow">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <th>
                        公司名稱
                    </th>
                    <td class="tdleft">
                        <asp:DropDownList ID="ddlOrganization" CssClass="textfield" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th width="20%">
                        日期區間
                    </th>
                    <td class="tdleft">
                        自&nbsp;<uc1:CalendarInputDatePicker ID="DateFrom" runat="server" />
                        &nbsp;至&nbsp;<uc1:CalendarInputDatePicker ID="DateTo" runat="server" />
                    </td>
                </tr>
            </table>
            <!--表格 結束-->
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn">
                    <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" OnClick="btnQuery_Click" />
                </td>
            </tr>
        </table>
        <div id="divResult" visible="false" runat="server">
            <uc6:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="查詢結果" />
            <!--表格 開始-->
            <div id="border_gray">
                <div runat="server" id="ResultTitle">
                    <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
                        GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" /></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                    <asp:Label ID="lblID" Text='<%# ((dataType)Container.DataItem).InvoiceID%>' runat="server"
                                        Visible="false"></asp:Label></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="日期">
                                <ItemTemplate>
                                    <%# ((dataType)Container.DataItem).ChineseInvoiceDate%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="開立發票營業人">
                                <ItemTemplate>
                                    <%# ((dataType)Container.DataItem).CompanyName%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="統編">
                                <ItemTemplate>
                                    <%# ((dataType)Container.DataItem).ReceiptNo%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="發票號碼">
                                <ItemTemplate>
                                    <%# String.Format("{0}{1}",((dataType)Container.DataItem).TrackCode,((dataType)Container.DataItem).No)%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="發票金額">
                                <ItemTemplate>
                                    <%#String.Format("{0:0,0.00}", ((dataType)Container.DataItem).TotalAmount)%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="是否中獎">
                                <ItemTemplate>
                                    <%# ((dataType)Container.DataItem).check%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="捐贈單位">
                                <ItemTemplate>
                                    <%# ((dataType)Container.DataItem).DonationName%></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle />
                        <PagerStyle HorizontalAlign="Center" />
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
                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01">
                        <tr>
                            <td class="total-count" align="right">
                                共
                                <asp:Label ID="lblRowCount" Text="0" runat="server"></asp:Label>
                                筆&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;總計金額：<asp:Label ID="lblTotalSum" Text="0" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <!--表格 結束-->
                <center>
                    <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
                </center>
            </div>
            <!--按鈕-->
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="Bargain_btn">
                        <asp:Button ID="btnShow" runat="server" Text="列印發票" OnClick="btnShow_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <uc4:PNewInvalidInvoicePreview ID="PNewInvalidInvoicePreview1" runat="server" />
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="rdbInvoiceType" EventName="SelectedIndexChanged" />
        <asp:AsyncPostBackTrigger ControlID="rdbSearchItem" EventName="SelectedIndexChanged" />
        <asp:PostBackTrigger ControlID="btnQuery" />
        <asp:PostBackTrigger ControlID="gvEntity" />
        <asp:PostBackTrigger ControlID="btnShow" />
    </Triggers>
</asp:UpdatePanel>
