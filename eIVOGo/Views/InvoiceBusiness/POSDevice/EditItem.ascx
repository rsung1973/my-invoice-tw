<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td>
        <input class="form-control" name="POSNo" type="text" value="<%= _model.POSNo %>" /></td>
    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="commitPOS(<%= _model.DeviceID %>);">確定</a></li>
                <li><a class="btn" onclick="showPOS(<%= _model.DeviceID %>);">取消</a></li>
                <li><a class="btn" onclick="generateCode();">自動產生</a></li>
            </ul>
        </div>
    </td>
</tr>


<script runat="server">

    POSDevice _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (POSDevice)this.Model;
    }

</script>
