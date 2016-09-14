<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<script type="text/javascript">
    $(document).ready(
        function () {
            var chkBox = $("input[id$='chkAll']");
            chkBox.click(
            function () {
                $("#gvEntity INPUT[type='checkbox']")
                .attr('checked', chkBox
                .is(':checked'));
            });

            // To deselect CheckAll when a GridView CheckBox        
            // is unchecked        
            $("#gvEntity INPUT[type='checkbox']").click(
            function (e) {
                if (!$(this)[0].checked) {
                    chkBox.attr("checked", false);
                }
            });
        });
</script>
<input id="chkAll" name="chkAll" type="checkbox" />


