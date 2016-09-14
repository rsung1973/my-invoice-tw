<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Base.InvoiceItemList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceDate.ascx" TagPrefix="uc1" TagName="InvoiceDate" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/BuyerCustomerID.ascx" TagPrefix="uc1" TagName="BuyerCustomerID" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PurchaseOrderNo.ascx" TagPrefix="uc1" TagName="PurchaseOrderNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceSeller.ascx" TagPrefix="uc1" TagName="InvoiceSeller" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceSellerReceiptNo.ascx" TagPrefix="uc1" TagName="InvoiceSellerReceiptNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceNoPreview.ascx" TagPrefix="uc1" TagName="InvoiceNoPreview" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SalesAmount.ascx" TagPrefix="uc1" TagName="SalesAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/TaxAmount.ascx" TagPrefix="uc1" TagName="TaxAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/TotalAmount.ascx" TagPrefix="uc1" TagName="TotalAmount" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/WinningInvoice.ascx" TagPrefix="uc1" TagName="WinningInvoice" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceDonation.ascx" TagPrefix="uc1" TagName="InvoiceDonation" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/BuyerReceiptNo.ascx" TagPrefix="uc1" TagName="BuyerReceiptNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceRemark.ascx" TagPrefix="uc1" TagName="InvoiceRemark" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SMSNotification.ascx" TagPrefix="uc1" TagName="SMSNotification" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceCarrierNo.ascx" TagPrefix="uc1" TagName="InvoiceCarrierNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceBuyerField/CustomerName.ascx" TagPrefix="uc1" TagName="CustomerName" %>
<%@ Register Src="~/Module/EIVO/InvoiceBuyerField/ContactName.ascx" TagPrefix="uc1" TagName="ContactName" %>
<%@ Register Src="~/Module/EIVO/InvoiceBuyerField/Phone.ascx" TagPrefix="uc1" TagName="Phone" %>
<%@ Register Src="~/Module/EIVO/InvoiceBuyerField/Address.ascx" TagPrefix="uc1" TagName="Address" %>





<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<!--表格 開始-->
<asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="True" ClientIDMode="Static"
    EnableViewState="False" DataSourceID="dsEntity" ShowFooter="True">
    <Columns>
        <asp:TemplateField HeaderText="開立發票營業人">
            <ItemTemplate>
                <uc1:InvoiceSeller runat="server" ID="InvoiceSeller" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="發票">
            <ItemTemplate>
                <uc1:InvoiceNoPreview runat="server" ID="InvoiceNoPreview" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="客戶名稱">
            <ItemTemplate>
                <uc1:CustomerName runat="server" ID="CustomerName" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="客戶統編">
            <ItemTemplate>
                <uc1:BuyerReceiptNo runat="server" ID="BuyerReceiptNo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="聯絡人姓名">
            <ItemTemplate>
                <uc1:ContactName runat="server" ID="ContactName" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="連絡電話">
            <ItemTemplate>
                <uc1:Phone runat="server" ID="Phone" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="地址">
            <ItemTemplate>
                <uc1:Address runat="server" ID="Address" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="管理">
            <ItemTemplate>
                <input type="button" name="btnEdit" value="編輯" onclick='<%# "editInvoiceBuyer(" + (Container.DataItemIndex+1).ToString() + "," + Eval("DocID").ToString() + ");" %>' />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>
    </Columns>
    <FooterStyle />
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
<center>
    <span style="font-size:larger;color:red;"><%# _totalRecordCount==0 ? "查無資料":null %></span>
</center>
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
<script>
    function editInvoiceBuyer(index, docID) {
        $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/EditInvoiceBuyer.aspx")%>','index='+index+'&docID=' + docID  , function (html) {
            var tr = $("#gvEntity tr").eq(index);
            tr.empty();
            tr.html(html);
            $("body").scrollTop(tr.offset().top);
            //tr.find("td").last().append("<input type='hidden' name='index' value='" + index + "," + docID + "' />");
        });
        //event.preventDefault();
    }

    function updateInvoiceBuyer(index, docID) {
        var tr = $("#gvEntity tr").eq(index);
        $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/UpdateInvoiceBuyer.aspx")%>', tr.find("input,select,textArea").serialize() + '&index=' + index + '&docID=' + docID, function (html) {
            tr.empty();
            tr.html(html);
            $("body").scrollTop(tr.offset().top);
        });
    }

    function loadInvoiceBuyer(index, docID) {
        $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/LoadInvoiceBuyer.aspx")%>', 'index=' + index + '&docID=' + docID, function (html) {
            var tr = $("#gvEntity tr").eq(index);
            tr.empty();
            tr.html(html);
            $("body").scrollTop(tr.offset().top);
        });
    }

    function queryZipCode(index) {

        var xmlhttp = new XMLHttpRequest();
        var addr = $("#gvEntity tr").eq(index).find("textarea[name='Address']");
        var url = encodeURI("<%= VirtualPathUtility.ToAbsolute("~/Helper/LoadZipCode.ashx")%>?adrs=" + addr.text());

        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                var myArr = JSON.parse(xmlhttp.responseText);
                showZip(myArr);
            }
        }
        xmlhttp.open("GET", url, true);
        xmlhttp.send();

        function showZip(arr) {
            addr.after("<font color='red'>郵遞區號:" + arr.zipcode + "</font>");
            alert("郵遞區號:" + arr.zipcode);
        }

    }

</script>