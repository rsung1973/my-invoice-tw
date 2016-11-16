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
<script>
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
</script>