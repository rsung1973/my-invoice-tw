<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><%= _model.Year %></td>
    <td><%= String.Format("{0:00}-{1:00}月",_model.Period*2-1,_model.Period*2) %></td>
    <td><%= ((Naming.WinningPrizeType)_model.Rank).ToString() %></td>
    <td><%= String.Format("{0:##,###,###,###}",_model.Bonus) %></td>
    <td><%= _model.WinningNO %></td>
    <td>
    <%  if (isEditable( _model))
        { %>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiWinningNo.editItem(<%= _model.WinningID %>);">修改</a></li>
                <li><a class="btn" onclick="uiWinningNo.deleteItem(<%= _model.WinningID %>);">刪除</a></li>
            </ul>
        </div>
    <%  } %>
</td>
</tr>


<script runat="server">

    UniformInvoiceWinningNumber _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (UniformInvoiceWinningNumber)this.Model;
    }

    bool isEditable(UniformInvoiceWinningNumber item)
    {
        switch((Naming.WinningPrizeType)item.Rank)
        {
            case Naming.WinningPrizeType.特別獎:
            case Naming.WinningPrizeType.特獎:
            case Naming.WinningPrizeType.頭獎:
            case Naming.WinningPrizeType.增開六獎:
                return true;
            default:
                return false;
        }
    }

</script>
