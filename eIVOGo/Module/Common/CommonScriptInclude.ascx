<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/jquery-1.11.3.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/jquery-ui-1.11.3.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/bootstrap.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/jquery.ui.datepicker-zh-TW.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/jquery.form.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/uxeivo.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/rwd-table.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/jquery.twbsPagination.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/vendor/metisMenu/metisMenu.min.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/sb-admin-2.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/math.min.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/jquery.blockUI.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/linq.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/linq.jquery.js") %>"></script>
<script type="text/javascript" src="<%=VirtualPathUtility.ToAbsolute("~/Scripts/stringformat-1.11.min.js") %>"></script>

<script>

    var $global = (function () {

        return {
            registerCloseEvent: function ($tab) {
                $tab.find(".closeTab").click(function () {

                    //there are multiple elements which has .closeTab icon so close the tab whose close icon is clicked
                    var tabContentId = $(this).parent().attr("href");
                    $(this).parent().parent().remove(); //remove li of tab
                    $('#masterTab a:last').tab('show'); // Select first tab
                    $(tabContentId).remove(); //remove respective tab content

                });
            },
            showTab: function (tabId) {
                $('#masterTab a[href="#' + tabId + '"]').tab('show');
            },
            createTab: function (tabId, tabText, tabContent, show) {
                var newTab = $('<li role="presentation"></li>')
                        .append($('<a href="#masterHome" class="tab-link" role="tab" data-toggle="tab"></a>')
                            .attr('href', '#' + tabId).attr('aria-controls', tabId).text(tabText)
                            .append($('<button class="close closeTab"><i class="fa fa-times" aria-hidden="true"></i></button>')));
                newTab.appendTo($('#masterTab'));
                $('<div role="tabpanel" class="tab-pane"></div>').attr('id', tabId)
                    .append(tabContent).appendTo($('#masterTabContent'));
                this.registerCloseEvent(newTab);
                if (show)
                    this.showTab(tabId);
            },
        };
    })();

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    function showLoading(autoHide,onBlock) {
        $.blockUI({
            message:  '<img src="<%= VirtualPathUtility.ToAbsolute("~/images/loading.gif") %>" /><h1>Loading</h1>', 
            css: {
                border: 'none',
                padding: '15px',
                backgroundColor: '#000',
                '-webkit-border-radius': '10px',
                '-moz-border-radius': '10px',
                opacity: .5,
                color: '#fff'
            },
            // 背景圖層
            overlayCSS:  { 
                backgroundColor: '#3276B1', 
                opacity:         0.6, 
                cursor:          'wait' 
            },
            onBlock: onBlock
        });

        if(autoHide)
            setTimeout($.unblockUI, 5000);
    }

    function hideLoading() {
        $.unblockUI();
    }

    function initSort (sort,offset) {

        $('.itemList th').each(function (idx, elmt) {
            var $this = $(this);
            if(sort.indexOf(idx+offset)>=0) {
                $this.attr('aria-sort', 'ascending');
                $this.append('<i class="fa fa-sort-asc" aria-hidden="true"></i>')
                    .append($('<input type="hidden" name="sort"/>').val(idx+1));
            } else if(sort.indexOf(-idx-offset)>=0) {
                $this.attr('aria-sort', 'desending');
                $this.append('<i class="fa fa-sort-desc" aria-hidden="true"></i>')
                    .append($('<input type="hidden" name="sort"/>').val(-idx-1));
            }
        });
    }

    function buildSort(inquire, currentPageIndex, offset) {

        var chkBox = $(".itemList input[name='chkAll']");
        var chkItem = $(".itemList input[name='chkItem']");
        chkBox.click(function () {
            chkItem.prop('checked', chkBox.is(':checked'));
        });

        chkItem.click(function (e) {
            if (!$(this).is(':checked')) {
                chkBox.prop('checked', false);
            }
        });

        $('.itemList th').each(function (idx, elmt) {
            var $this = $(this);
            if (!$this.is('[aria-sort="other"]')) {
                if (!$this.is('[aria-sort]')) {
                    $this.append('<i class="fa fa-sort" aria-hidden="true"></i>')
                        .append('<input type="hidden" name="sort"/>');
                    $this.attr('aria-sort', 'none');
                }
                $this.on('click', function (evt) {
                    var $target = $(this);
                    $target.find('i').remove();
                    if ($target.is('[aria-sort="none"]')) {
                        $target.append('<i class="fa fa-sort-asc" aria-hidden="true"></i>');
                        $target.attr('aria-sort', 'ascending');
                        $target.find('input[name="sort"]').val(idx + offset);
                    } else if ($target.is('[aria-sort="ascending"]')) {
                        $target.append('<i class="fa fa-sort-desc" aria-hidden="true"></i>');
                        $target.attr('aria-sort', 'descending');
                        $target.find('input[name="sort"]').val(-idx - offset);
                    } else {
                        $target.append('<i class="fa fa-sort" aria-hidden="true"></i>');
                        $target.attr('aria-sort', 'none');
                        $target.find('input[name="sort"]').val('');
                    }
                    inquire(currentPageIndex, function (data) {
                        var $node = $('.itemList').next();
                        $('.itemList').remove();
                        $node.before(data);
                    });
                });
            }
        });
    }
</script>