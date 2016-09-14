<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="PagingControl.ascx.cs"
    Inherits="Uxnet.Web.Module.Ajax.PagingControl" %>
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" id="table-count">
    <tr>
        <td>
            <asp:PlaceHolder ID="PlaceHolder3" runat="server" Visible="<%# _currentPageIndex >= __PAGING_SIZE %>">|
                <a href="javascript:" onclick="gotoPage(<%# Math.Max(0, _currentPageIndex - __PAGING_SIZE) + 1 %>);">上<%= __PAGING_SIZE %>頁</a>
            </asp:PlaceHolder>
            <asp:Repeater ID="rpList" runat="server" EnableViewState="false">
                <ItemTemplate>
                    |
                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="<%# (int)Container.DataItem != _currentPageIndex %>">
                    <a href="javascript:gotoPage(<%# (int)Container.DataItem + 1 %>);" onclick="gotoPage(<%# (int)Container.DataItem + 1 %>);"><%# (int)Container.DataItem + 1 %></a>
                </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="<%# (int)Container.DataItem == _currentPageIndex %>">
                        <%# (int)Container.DataItem + 1 %>
                    </asp:PlaceHolder>
                </ItemTemplate>
            </asp:Repeater>
            <asp:PlaceHolder ID="PlaceHolder4" runat="server" Visible="<%# (PageCount - ((int)(_currentPageIndex / __PAGING_SIZE)) * __PAGING_SIZE) > __PAGING_SIZE %>">|
                <a href="javascript:" onclick="gotoPage(<%# Math.Min(PageCount - 1, _currentPageIndex + __PAGING_SIZE) + 1 %>);">下<%= __PAGING_SIZE %>頁</a>
            </asp:PlaceHolder>
            <input type="text" name="pageNum" value="<%= _currentPageIndex >= 0 ? (_currentPageIndex + 1).ToString() : "" %>" size="4" class="textfield" />
            &nbsp;<input type="button" value="頁數" class="btn" />
            &nbsp;</td>
        <td align="right" nowrap="nowrap">
            <%= String.Format("總筆數：{0} &nbsp;&nbsp;&nbsp;總頁數：{1}", _recordCount, PageCount) %>
        </td>
    </tr>
</table>
<script>
    if (typeof gotoPage == 'undefined') {
        function gotoPage(pageNum) {
            $('#table-count input[name="pageNum"]').val(pageNum);
            if (typeof submitPagingIndex != 'undefined' && submitPagingIndex != undefined) {
                submitPagingIndex(pageNum);
            }
        };
    }
</script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += module_ajax_pagingcontrol_ascx_PreRender;
        int intVal;
        if (int.TryParse(Request["pageNum"], out intVal))
        {
            CurrentPageIndex = intVal - 1;
        }
    }

    void module_ajax_pagingcontrol_ascx_PreRender(object sender, EventArgs e)
    {
        DataBind();
    }

    public override void DataBind()
    {
        base.DataBind();
        
        if (PageCount > 0)
        {
            if (_currentPageIndex >= PageCount)
            {
                _currentPageIndex = PageCount - 1;
            }
            else if (_currentPageIndex < 0)
            {
                _currentPageIndex = 0;
            }
        }
        else
        {
            _currentPageIndex = -1;
        }


        int startIndex = _currentPageIndex >= 0 ? _currentPageIndex / __PAGING_SIZE * __PAGING_SIZE : 0;

        rpList.DataSource = Enumerable.Range(startIndex, Math.Min(PageCount - startIndex, __PAGING_SIZE));
        rpList.DataBind();
    }    
</script>
