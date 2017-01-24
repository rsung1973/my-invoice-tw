<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>


<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "會員管理-修改帳號"); %>
<div class="border_gray">
    <!--表格 開始-->
    <%  Html.RenderPartial("~/Views/UserProfile/Module/ItemForm.ascx"); %>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td class="Bargain_btn" align="center">
                <button class="btn" name="btnConfirm" type="button" onclick="uiUserProfile.commit();">確定</button>
                &nbsp;
                <input name="Reset" type="reset" class="btn" value="重填" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>

<!--表格 開始-->
<script>
    var uiUserProfile;
    $(function () {
        uiUserProfile = {
            commit: function () {
                var event = event || window.event;
                var $form = $(event.target).closest('form');
                $form.ajaxForm({
                    url: "<%= Url.Action("Commit","UserProfile",new { WaitForCheck = _viewModel.WaitForCheck }) %>",
                    beforeSubmit: function () {
                        clearErrors();
                        showLoading();
                    },
                    success: function (data) {
                        if (data) {
                            $(data).appendTo($('body'));
                        }
                        hideLoading();
                    },
                    error: function () {
                        hideLoading();
                    }
                }).submit();
            },
            print: function (style) {
                if (!$('input[name="chkItem"]').is(':checked')) {
                    alert('請選擇列印資料!!');
                    return false;
                }

                $('#theForm').ajaxForm({
                    url: "<%= Url.Action("Print","InvoiceProcess") %>" + '?paperStyle=' + style,
                    beforeSubmit: function () {
                        showLoading();
                    },
                    success: function (data) {
                        hideLoading();
                        if (data) {
                            var $data = $(data);
                            $data.dialog();
                            $data.find('a').on('click', function (evt) {
                                $data.dialog('close');
                            });
                        }
                    },
                    error: function () {
                        hideLoading();
                    }
                }).submit();
            },
            download: function () {
                if ($postForm) {
                    $postForm.remove();
                }

                $postForm = $('<form method="post" />').prop('action', '<%= Url.Action("CreateXlsx","InvoiceProcess") %>')
                    .css('display', 'none').appendTo($('body'));

                $('#theForm').serializeArray().forEach(function (item, index) {
                    $('<input type="hidden">')
                        .prop('name', item.name).prop('value', item.value)
                        .appendTo($postForm);
                });
                $postForm.submit();
                //showLoading();
            },
        };
    });
</script>
<script runat="server">

    ModelSource<InvoiceItem> models;
    ModelStateDictionary _modelState;
    UserProfileViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        _viewModel = (UserProfileViewModel)ViewBag.ViewModel;
    }
</script>

