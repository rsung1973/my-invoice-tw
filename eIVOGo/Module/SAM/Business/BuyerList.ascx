<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BuyerList.ascx.cs"
    Inherits="eIVOGo.Module.SAM.Business.BuyerList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<script type="text/javascript" src="http://tools.5432.tw/js/zip5.js"></script>



<p align="right"><font color="FF3937" size="3">該網頁郵遞區號生成API來源:</font><asp:HyperLink Text="http://tools.5432.tw" NavigateUrl="http://tools.5432.tw" target='_blank' runat="server"></asp:HyperLink>
    <br />(僅適用未含郵遞區號之地址資料)
    <br />備註：<font color='0300FA'>藍色</font>為缺少郵遞區號之地址資料
</p>
    
        <div class="border_gray">
            
            <asp:GridView ID="gvEntity" runat="server" AllowPaging="False" Width="100%" CellPadding="0"
                EnableViewState="False" AutoGenerateColumns="False" CssClass="table01" ClientIDMode="Static"
                DataSourceID="dsEntity" OnDataBound="gvEntity_DataBound" >
                <Columns>
                    <asp:TemplateField HeaderText="發票開立人">
                        <ItemTemplate>
                            <%# ((InvoiceBuyer)Container.DataItem).InvoiceItem.Organization.CompanyName%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GoogleID" Visible="false">
                        <ItemTemplate><%# ((InvoiceBuyer)Container.DataItem).CustomerID%></ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="發票號碼">
                        <ItemTemplate>
                            <%# ((InvoiceBuyer)Container.DataItem).InvoiceItem.TrackCode + ((InvoiceBuyer)Container.DataItem).InvoiceItem.No%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="客戶名稱">
                        <EditItemTemplate>
                            <input type="text" name="txtCustomerName" value='<%# Request["txtCustomerName"] == null ? ((InvoiceBuyer)Container.DataItem).CustomerName : Request["txtCustomerName"]%>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# ((InvoiceBuyer)Container.DataItem).CustomerName %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="統一編號">
                        <ItemTemplate>
                            <%# ((InvoiceBuyer)Container.DataItem).ReceiptNo.Equals("0000000000") ? "" : ((InvoiceBuyer)Container.DataItem).ReceiptNo%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="聯絡人姓名">
                        <EditItemTemplate>
                            <input type="text" name="txtContactName" value='<%# Request["txtContactName"] == null ? ((InvoiceBuyer)Container.DataItem).ContactName : Request["txtContactName"]%>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# ((InvoiceBuyer)Container.DataItem).ContactName %>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="連絡電話">
                        <EditItemTemplate>
                            <input type="text" name="txtPhone" value='<%# Request["txtPhone"] == null ? ((InvoiceBuyer)Container.DataItem).Phone : Request["txtPhone"]%>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%# ((InvoiceBuyer)Container.DataItem).Phone %>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="地址">
                        <EditItemTemplate>
                            <input type="text" id="my_adrs" name="txtAddr" value='<%# Request["txtAddr"] == null ? ((InvoiceBuyer)Container.DataItem).Address : Request["txtAddr"]%>' size="50%" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <%--<%# ((InvoiceBuyer)Container.DataItem).Address %>--%>
                            <%# CreateAddress((InvoiceBuyer)Container.DataItem)%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="管理">
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" CausesValidation="false" CommandName="Update"
                                Text="更新" CssClass="btn" OnClientClick='<%# doUpdate.GetPostBackEventReference(String.Format("{0},{1}", ((InvoiceBuyer)Container.DataItem).InvoiceID,Container.DataItemIndex))%>' />
                            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                Text="取消" CssClass="btn" OnClientClick='<%# doCancel.GetPostBackEventReference(Container.DataItemIndex.ToString())%>' />
                             <input type='button' class="btn" onclick='get_zip5_adrs("my_adrs")' value='查詢郵遞區號' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit"
                                Text="編輯" CssClass="btn" OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}", Container.DataItemIndex))%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <AlternatingRowStyle CssClass="OldLace" />
                <PagerStyle BackColor="PaleGoldenrod" HorizontalAlign="Left" BorderStyle="None" CssClass="noborder" />
                <PagerTemplate>
                    <uc2:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                </PagerTemplate>
            </asp:GridView>
            <center>
        <asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server" Text="查無資料!!"
            EnableViewState="false"></asp:Label>
    </center>
        </div>

        
        
<cc1:InvoiceBuyerDataSource ID="dsEntity" runat="server"></cc1:InvoiceBuyerDataSource>
        <uc1:ActionHandler ID="doEdit" runat="server" />
        <uc1:ActionHandler ID="doUpdate" runat="server" />
        <uc1:ActionHandler ID="doCancel" runat="server" />
<uc1:ActionHandler ID="doQueryAddrCode" runat="server" />

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        gvEntity.RowDataBound += gvEntity_RowDataBound;
    }

    void gvEntity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_rowIndex.HasValue && e.Row.RowIndex == _rowIndex)
        {
            Label lbl = new Label();
            lbl.ID = "viewHere";
            e.Row.Cells[0].Controls.Add(lbl);
            e.Row.ForeColor = System.Drawing.Color.Red;
            ScriptManager.RegisterStartupScript(lbl, this.GetType(), "focus",
                String.Format("document.all('{0}').scrollIntoView(false);\r\n", lbl.ClientID), true);
        }
    }
    
    String CreateAddress(InvoiceBuyer buyer)
    {
        if (buyer.Address != null)
        {
            bool check = false;

            char[] ss = buyer.Address.ToArray<char>();

            if (ss.Count() > 6)
            {
                for (int index = 0; index < 6; index++)
                {
                    if (!Regex.IsMatch(ss[index].ToString(), "^[0-9]"))
                    {
                        if (index == 3 || index == 5)
                        {
                            check = true;
                        }

                        break;
                    }
                }
            }

            String Address = "";

            if (!check)
            {
                Address = "<font color='0300FA'>" + buyer.Address + "</font>";
            }
            else
            {
                Address = buyer.Address;
            }

            return Address;
        }
        return "";
    }
        
</script>