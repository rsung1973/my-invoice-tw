<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<script>
    var uiHandling;
    $(function () {
        var $postForm;
        uiHandling = {
            items: [],
            inquireTracking: function (pageNum, onPaging) {
                if (pageNum) {
                    $('input[name="pageIndex"]').val(pageNum);
                } else {
                    $('input[name="sort"]').remove();
                }
                $('#theForm').ajaxForm({
                    url: "<%= Url.Action("InquireToTrackMail","Handling",new { resultAction = ViewBag.ResultAction }) %>",
                    beforeSubmit: function () {
                        clearErrors();
                        showLoading();
                    },
                    success: function (data) {
                        if (data) {
                            var $data = $(data);
                            $('body').append($data);
                            //$data.remove();
                        }
                        hideLoading();
                    },
                    error: function () {
                        hideLoading();
                    }
                }).submit();
            },

            showDetails: function () {
                if (!uiHandling.$result) {
                    $.post('<%= Url.Action("InvoiceMailItems","Handling",new { showTable = true }) %>', { 'id': Enumerable.From(uiHandling.items).Select(function (i) { return i.InvoiceID; }).ToArray() }, function (data) {
                        uiHandling.$result = $(data);
                        $('.queryAction').after(uiHandling.$result);
                        uiHandling.$result = uiHandling.$result.find('tbody');
                    });
                } else {
                    $.post('<%= Url.Action("InvoiceMailItems","Handling",new { showTable = false }) %>', { 'id': Enumerable.From(uiHandling.items).Select(function (i) { return i.InvoiceID; }).ToArray() }, function (data) {
                        $(data).appendTo(uiHandling.$result);
                    });
                }
            },

            pack: function () {
                //var event = event || window.event;
                //var $tr = $(event.target).closest('tr');
                var allItems = Enumerable.From(uiHandling.items);
                var checkedItems = $('input[name="PackageID"]:checked').toEnumerable();
                if (checkedItems.Count() < 1) {
                    alert('請選擇合併寄件項目!!');
                    return;
                }
                var first = checkedItems.First();
                var $tr = first.closest('tr');
                var packageID = first.val();
                var items = [];
                checkedItems.ForEach(function (c) {
                    allItems.Where(function (i) { return i.PackageID == c.val(); })
                        .ForEach(function (current) {
                            current.PackageID = packageID;
                            items.push(current.InvoiceID);
                        });
                });

                $.post('<%= Url.Action("PackInvoice","Handling") %>', { 'id': items }, function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            checkedItems.ForEach(function (c) {
                                c.closest('tr').remove();
                            });
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            },

            unpack: function () {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                var packageID = $tr.find('input[name="PackageID"]').val();
                var items = Enumerable.From(uiHandling.items)
                    .Where(function (i) { return i.PackageID == packageID; });
                if (items.Count() < 1) {
                    alert('請選擇分開寄件項目!!');
                    return;
                }

                var params = items.Select(function (i) { return i.InvoiceID; }).ToArray();
                items.ForEach(function (current) {
                    current.PackageID = current.InvoiceID;
                });

                $.post('<%= Url.Action("InvoiceMailItems","Handling",new { showTable = false }) %>', { 'id': params }, function (data) {
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
            },

            processPack: function () {
                var items = $('input[name="DeliveryDate"]');
                var val = items.val();
                items.each(function (idx) {
                    $(this).val(val);
                });

                items = $('input[name="MailNo2"]');
                val = items.val();
                items.each(function (idx) {
                    $(this).val(val);
                });

                var items = $('input[name="MailNo1"]');
                var val = items.val();
                var count = $('input[name="MailingCount"]').val();
                if (!isNaN(val) && !isNaN(count)) {
                    val = parseInt(val);
                    count = parseInt(count);
                    items.each(function (idx) {
                        if (idx > 0 && idx <= count) {
                            $(this).val(val + count + idx - 1);
                        }
                    });
                }
            },

            download: function () {
                if ($postForm) {
                    $postForm.remove();
                }

                var dateItems = $('input[name="DeliveryDate"][data-package]').toEnumerable()
                    .Select(function (c) {
                        return { 'key': c.attr('data-package'), 'value': c.val() };
                    });

                var mailNo1 = $('input[name="MailNo1"][data-package]').toEnumerable()
                    .Select(function (c) {
                        return { 'key': c.attr('data-package'), 'value': c.val() };
                    });

                var mailNo2 = $('input[name="MailNo2"][data-package]').toEnumerable()
                    .Select(function (c) {
                        return { 'key': c.attr('data-package'), 'value': c.val() };
                    });

                var items = Enumerable.From(uiHandling.items).GroupBy(function (i) { return i.PackageID; })
                    .Select(function (g) {
                        return {
                            'PackageID': g.Key(),
                            'DeliveryDate': dateItems.First(function (c) { return c.key == g.Key(); }).value,
                            'MailNo1': mailNo1.First(function (c) { return c.key == g.Key(); }).value,
                            'MailNo2': mailNo2.First(function (c) { return c.key == g.Key(); }).value,
                            'InvoiceID': g.Select(function (v) { return v.InvoiceID; }).ToArray()
                        };
                    }).ToArray();


                $postForm = $('<form method="post" />').prop('action', '<%= Url.Action("DownloadXlsx","Handling") %>')
                    .css('display', 'none').appendTo($('body'));

                $('<input type="hidden" name="data">')
                    .prop('value', JSON.stringify(items))
                    .appendTo($postForm);
                $postForm.submit();
                //showLoading();
            },
        };
    });
</script>
<script runat="server">

    ModelSource<InvoiceItem> models;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }
</script>

