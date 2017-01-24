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
    <td><%= _viewModel.Year-1911 %><input name="Year" type="hidden" value="<%= _viewModel.Year %>" /></td>
    <td><select name="PeriodNo">
                        <option value="1">01-02月</option>
                        <option value="2">03-04月</option>
                        <option value="3">05-06月</option>
                        <option value="4">07-08月</option>
                        <option value="5">09-10月</option>
                        <option value="6">11-12月</option>
                    </select>
                    <script>
                        $(function () {
                            $('select[name="PeriodNo"]').val(<%= (DateTime.Now.Month+1)/2 %>);
                        });
                    </script></td>
    <td>
        <input type="text" name="TrackCode" class="form-control" placeholder="請輸入字軌" value="" data-role="add" />
    </td>
    <td>
        <a class="btn" onclick="uiTrackCodeQuery.commitItem();">新增字軌</a>
    </td>
</tr>


<script runat="server">

    TrackCodeQueryViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _viewModel = (TrackCodeQueryViewModel)ViewBag.ViewModel;
    }

</script>
