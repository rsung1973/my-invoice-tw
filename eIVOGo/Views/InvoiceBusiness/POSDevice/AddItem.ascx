<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td>
        <input class="form-control" name="POSNo" type="text" />
    </td>
    <td>
        <a class="btn" onclick="commitPOS();">新增</a>
        <a class="btn" onclick="generateCode();">自動產生</a>
        <script>

            function deletePOS(value) {
                if (confirm('確定刪除此筆資料?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("DeletePOS", new { id = _model.CompanyID }) %>', { 'deviceID': value }, function (data) {
                        if (data.result) {
                            alert('資料已刪除!!')
                            $tr.remove();
                        } else {
                            alert(data.message);
                        }
                    });
                }
            }
            function commitPOS(value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("CommitPOS", new { id = _model.CompanyID }) %>', 'deviceID=' + value + '&' + $.param($tr.find('input,select,textarea')), function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            if (value) {
                                alert('資料已更新!!');
                                $tr.remove();
                            }
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            }

            function editPOS(value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("EditPOS", new { id = _model.CompanyID }) %>', 'deviceID=' + value , function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            $tr.remove();
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            }

            function showPOS(value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("DataItem", new { id = _model.CompanyID }) %>', 'deviceID=' + value , function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            $tr.remove();
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            }

            function generateCode() {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("GenerateGUID") %>' , function (data) {
                    if (data) {
                        $tr.find('input').val(data);
                    }
                });
            }
        </script>
    </td>
</tr>


<script runat="server">

    Organization _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (Organization)this.Model;
    }

</script>
