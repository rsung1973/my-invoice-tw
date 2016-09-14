<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUploadMailMessage.ascx.cs"
    Inherits="eIVOGo.Module.EIVO.WebUploadMailMessage" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc3" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Register Src="../Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc4" %>
<%@ Register Src="../Common/DataModelCache.ascx" TagName="DataModelCache" TagPrefix="uc5" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc6" %>


<uc1:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 登錄掛號郵件" />
<uc2:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="登錄掛號郵件" />
<div id="border_gray">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th>
                寄件過程
            </th>
            <td class="tdleft">
                <asp:RadioButtonList ID="rbDelivery" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="True" EnableViewState="True" OnSelectedIndexChanged="rbDelivery_SelectedIndexChanged"
                    RepeatLayout="Flow">
                    <asp:ListItem Value="1303">初次寄送</asp:ListItem>
                    <asp:ListItem Value="1309">退回重寄</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <th>
                郵件張數
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMail_Amount" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                發票號碼(起號)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtInv_No_begin" runat="server"></asp:TextBox>
            </td>
            <th>
                發票號碼(迄號)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtInv_No_end" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                郵件號碼1(流水號碼段)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMail_No_1" runat="server"></asp:TextBox>
            </td>
            <th>
                郵件號碼2(固定號碼段)
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMail_No_2" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                寄件日期
            </th>
            <td class="tdleft" colspan="3">
                <uc4:CalendarInputDatePicker ID="DeliveryDate" runat="server" />
            </td>
        </tr>
    </table>
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="加入預覽" OnClick="btnQuery_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="清除預覽" OnClick="btnClear_Click" />
        </td>
    </tr>
</table>
<div id="divResult" visible="false" runat="server">
    <uc2:FunctionTitleBar ID="FunctionTitleBar2" runat="server" ItemName="資料預覽" />
    <div class="border_gray">
        <div runat="server" id="ResultTitle">
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <!--查詢結果_資料呈現-->
                <asp:GridView ID="gvEntity" runat="server" AutoGenerateColumns="False" Width="100%"
                    GridLines="None" CellPadding="0" CssClass="table01" AllowPaging="False" ClientIDMode="Static"
                    EnableViewState="False" ShowFooter="True">
                    <Columns>
                        <asp:TemplateField HeaderText="寄送日期">
                            <ItemTemplate>
                                <input type="text" name="deliveryDate" value='<%# String.Format("{0:yyyy/MM/dd}", ((InvoiceDeliveryTracking)Container.DataItem).DeliveryDate )%>'
                                    readonly="readonly" size="8" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GoogleID">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.InvoiceBuyer.CustomerID %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="發票號碼">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.TrackCode + ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.No %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="掛號號碼">
                            <ItemTemplate>
                                <input type="text" name="trackingNo" value='<%# ((InvoiceDeliveryTracking)Container.DataItem).TrackingNo1 %>'
                                    maxlength="8" size="8" />
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).TrackingNo2 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="150" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收件人">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.InvoiceBuyer.ContactName %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="收件人地址">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).InvoiceItem.InvoiceBuyer.Address %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="備考">
                            <ItemTemplate>
                                <%# ((InvoiceDeliveryTracking)Container.DataItem).DuplicateCount %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnMerge" runat="server" Text="合併" Visible="<%# !((InvoiceDeliveryTracking)Container.DataItem).MergedItem %>" OnClientClick='<%# doMerge.GetPostBackEventReference(String.Format("{0}",_itemIndex)) %>' /><br />
                                <asp:Button ID="btnDelete" runat="server" Text="刪除" Visible="<%# !((InvoiceDeliveryTracking)Container.DataItem).MergedItem %>" OnClientClick='<%# doDelete.GetPostBackEventReference(String.Format("{0}",_itemIndex++)) %>' />
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
                        <uc3:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </PagerTemplate>
                    <RowStyle />
                    <EditRowStyle />
                </asp:GridView>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn">
                            <asp:Button ID="btnSave" CssClass="btn" runat="server" Text="確定" OnClick="btnSave_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="CSV存檔" 
                                onclick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </table>
        </div>
    </div>
</div>
<uc5:DataModelCache ID="modelItem" runat="server" KeyName="DeliveryTracking" />
<uc6:ActionHandler ID="doDelete" runat="server" />
<uc6:ActionHandler ID="doMerge" runat="server" />
<cc1:InvoiceDataSource ID="dsEntity" runat="server">
</cc1:InvoiceDataSource>
<script runat="server">

    protected int _itemIndex;
    protected int _scrollIndex = -2;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(module_eivo_webuploadmailmessage_ascx_Load);
        this.PreRender += new EventHandler(module_eivo_webuploadmailmessage_ascx_PreRender);

        doDelete.DoAction = arg =>
            {
                _scrollIndex = int.Parse(arg);

                int index = int.Parse(arg);

                if (index >= 0)
                {
                    DeliveryList.RemoveAt(index);

                    if (DeliveryList.Count > 0)
                    {
                        _scrollIndex = index;
                        for (; index < DeliveryList.Count; index++)
                        {
                            DeliveryList[index].TrackingNo1 = (int.Parse(DeliveryList[index].TrackingNo1) - 1).ToString();
                        }
                    }
                    else
                    {
                        divResult.Visible = false;
                    }
                }
                                
            };

        doMerge.DoAction = arg =>
            {
                int index = int.Parse(arg);
                
                if (index > 0)
                {
                    var baseItem = DeliveryList[index - 1];
                    baseItem.MergedItem = true;
                    if (baseItem.DuplicateCount.HasValue)
                        baseItem.DuplicateCount++;
                    else
                        baseItem.DuplicateCount = 2;

                    DeliveryList[index].MergedItem = true;
                    
                    _scrollIndex = index;
                    for (; index < DeliveryList.Count; index++)
                    {
                        DeliveryList[index].TrackingNo1 = (int.Parse(DeliveryList[index].TrackingNo1)-1).ToString();
                    }

                }
            };
        
        gvEntity.RowDataBound += new GridViewRowEventHandler(gvEntity_RowDataBound);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "deliveryDate", "$(\"input[name='deliveryDate']\").datepicker({showButtonPanel: true, changeYear: true, changeMonth: true, yearRange:'2012:+0'});", true);
        rbDelivery.SelectedValue = Request[rbDelivery.UniqueID] != null ? Request[rbDelivery.UniqueID] : "1303";
        DeliveryStatus = (Naming.InvoiceDeliveryStatus)Enum.Parse(typeof(Naming.InvoiceDeliveryStatus), rbDelivery.SelectedValue);
    }

    void gvEntity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex == _scrollIndex)
        {
            //Label lbl = new Label();
            //lbl.ID = "viewHere";
            //e.Row.Cells[0].Controls.Add(lbl);
            e.Row.ForeColor = System.Drawing.Color.Red;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "focus", @"
                $('html, body').animate({
                        scrollTop: $('#gvEntity tr:eq(" + _scrollIndex + @")').offset().top
                    }, 2000);", true);
            

//            ScriptManager.RegisterStartupScript(lbl, this.GetType(), "focus", @"
//                $('html, body').animate({
//                        scrollTop: $('#" + lbl.ClientID + @"').offset().top
//                    }, 2000);", true);
        }
    }

    void module_eivo_webuploadmailmessage_ascx_PreRender(object sender, EventArgs e)
    {
        _itemIndex = 0;
        if (DeliveryList.Count > 0)
        {
            divResult.Visible = true;
            gvEntity.DataSource = DeliveryList;
            gvEntity.DataBind();
        }
    }

    void module_eivo_webuploadmailmessage_ascx_Load(object sender, EventArgs e)
    {
        //if (!this.IsPostBack)
        //{
        //    DeliveryDate.DateTimeValue = DateTime.Today;
        //}
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        DeliveryList.Clear();
        divResult.Visible = false;
    }

    protected void rbDelivery_SelectedIndexChanged(object sender, EventArgs e)
    {
        DeliveryList.Clear();
        divResult.Visible = false;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (DeliveryList != null && DeliveryList.Count > 0)
        {
            Response.Clear();
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}({1:yyyy-MM-dd}).csv", Server.UrlEncode("大宗掛號函件存根"), DateTime.Today));
            Response.HeaderEncoding = Encoding.GetEncoding(950);

            using (System.IO.StreamWriter Output = new System.IO.StreamWriter(Response.OutputStream, Encoding.GetEncoding(950)))
            {
                Output.WriteLine("掛號號碼,姓名,	寄件地名 (或地址)	,是否回,是否航空,是否印刷物,重量,郵資,備考");
                int start = 0, to = 0;
                foreach (var item in DeliveryList)
                {
                    if (start < to)
                    {
                        start++;
                        continue;
                    }
                    Output.Write(item.TrackingNo1);
                    Output.Write(",");
                    Output.Write(item.InvoiceItem.InvoiceBuyer.ContactName);
                    Output.Write(",");
                    Output.Write(item.InvoiceItem.InvoiceBuyer.Address);
                    Output.Write(",,,,,,");
                    Output.WriteLine(item.DuplicateCount);

                    if (item.DuplicateCount.HasValue)
                    {
                        start = 1;
                        to = item.DuplicateCount.Value;
                    }
                }
            }
            

            Response.Flush();
            Response.End();
            
        }
    }
    
    
</script>
