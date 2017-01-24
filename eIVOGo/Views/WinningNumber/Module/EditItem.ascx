<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagPrefix="uc1" TagName="EnumSelector" %>

<tr>
    <td><%= _model.Year %></td>
    <td><%= String.Format("{0:00}-{1:00}月",_model.Period*2-1,_model.Period*2) %></td>
    <td><uc1:EnumSelector runat="server" ID="Rank" FieldName="Rank" TypeName="Model.Locale.Naming+EditableWinningPrizeType, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" /></td>
    <td>
    </td>
    <td>
        <input type="text" name="WinningNo" placeholder="請輸入中獎號碼" value="<%= _model.WinningNO %>" data-role="edit" />
    </td>
    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiWinningNo.commitItem(<%= _model.WinningID %>,<%= _model.Year %>,<%= _model.Period %>);">確定</a></li>
                <li><a class="btn" onclick="uiWinningNo.showItem(<%= _model.WinningID %>);">取消</a></li>
            </ul>
        </div>
    </td>
</tr>


<script runat="server">

    UniformInvoiceWinningNumber _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (UniformInvoiceWinningNumber)this.Model;
        Rank.DefaultValue = _model.Rank.ToString();
    }

</script>
