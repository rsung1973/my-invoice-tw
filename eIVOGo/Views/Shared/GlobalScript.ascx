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

<script>
    var $currentModal;
    $(function () {
        $('input[type="button"]').addClass('btn');
        $('button').addClass('btn');
        //$('form').on('submit', function () {
        //    var $currentModal = showLoadingModal();
        //});
        $.datepicker.setDefaults($.datepicker.regional['zh-tw']);
        $('.form_date').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
    });

    function clearErrors() {
        $('.error input,select,textarea').removeClass('error')
        $('label.error').remove();
    }

    function showInvoiceModal(docID) {
        var element = event.target;
        $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowInvoiceModal.aspx")%>', 'docID=' + docID, function (html) {
                //            $(element).after($(html).find("#mainContent"));
                $(html).find("#mainContent").dialog({
                    width: 640,
                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            });
        }

        function showAllowanceModal(docID) {
            var element = event.target;
            $.post('<%= VirtualPathUtility.ToAbsolute("~/Helper/ShowAllowanceModal.aspx")%>', 'docID=' + docID, function (html) {
                $(html).find("#mainContent").dialog({
                    width: 640,
                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close");
                            }
                        }
                    ]
                });
            });
        }

        function showLoadingModal() {
            var $body = $('body');
            //var $modal = $('<div style="position: absolute;top: 0;left: 0;z-index: 1000;width: 100%;opacity: 0.5;filter: Alpha(opacity=50);background: gray;"><img src="<%= VirtualPathUtility.ToAbsolute("~/images/loading.gif") %>" style="position: absolute;top:0;left:0;right:0;bottom:0;margin:auto;max-height:100%;max-width:100%;"></div>');
            var $modal = $('<div style="position: absolute;top: 0;left: 0;z-index: 1000;width: 100%;opacity: 0.5;filter: Alpha(opacity=50);background: gray;"><img src="<%= VirtualPathUtility.ToAbsolute("~/images/loading.gif") %>" style="position: absolute;"></div>');
            var $img = $modal.find('img');
            $modal.css('height', $body.css('height'));
            $img.css('top', $body.scrollTop() + screen.height / 2 - 48);
            $img.css('left', screen.width / 2 - 24);
            $body.append($modal);
            return $modal;
        }

        function actionHandler(url, data, done, width, height) {
            $('<div>').load(url, data, function (evt) {
                var $this = $(this);
                $this.dialog({
                    width: width,
                    height: height,
<%--                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close").remove();
                            }
                        }
                    ],--%>
                    close: function (evt, ui) {
                        $this.remove();
                        if (typeof (done) == 'function') {
                            done();
                        }
                    }
                });
            });
        }

        function invokeAction(url, data, done) {
            $.post(url, data, function (html) {
                console.log(html);
                var $s = $(html);
                $('body').append($s);
                $s.remove();
                if (typeof (done) == 'function') {
                    done();
                }
            });
        }

        function validateForm(formElement) {
            var isValid = true;
            $(formElement).find('label.error').remove();
            $(formElement).find('.error').removeClass('error');
            $(formElement.elements).each(function (idx, elmt) {
                var $elmt = $(elmt);
                elmt.setCustomValidity('');
                if (!$elmt.is(':hidden') && $elmt.parents().filter(':hidden').length == 0 && !elmt.checkValidity()) {
                    isValid = false;
                    $elmt.addClass('error');
                    if ($elmt.prop('placeholder')) {
                        //elmt.setCustomValidity($elmt.prop('placeholder'));
                        $('<label class="error"></label>').text($elmt.prop('placeholder'))
                            .insertAfter($elmt);
                    }
                }
            });
            return isValid;
        }

</script>

<script runat="server">

    ModelSource<InvoiceItem> models;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
    }

</script>
