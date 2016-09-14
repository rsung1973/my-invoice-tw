<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelInvoice.ascx.cs" Inherits="eIVOGo.Module.EIVO.CancelInvoice" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <br />
            <input type="button" id="btnPreview" name="btnPreview" value="作廢開立預覽" class="btn" />
        </td>
    </tr>
</table>
<script>
    $('#btnPreview').on('click', function () {
        var cancelInvoice = [];
        var i = 0;
        var hasError = false;
        var $reason = $('input[name="cancelReason"]');
        $('input[name="chkItem"]').each(function (idx) {
            var $this = $(this);
            if ($this.is(':checked')) {
                if (!$reason.eq(idx) || $reason.eq(idx).val() == '') {
                    alert('第' + (idx + 1) + '筆作廢票票請輸入作廢原因!!');
                    hasError = true;
                    return;
                }
                cancelInvoice[i] = {
                    'invoiceID' : $this.val(),
                    'cancelReason' : $reason.eq(idx).val()
                };
                i++;
            }
        });
        if (!hasError) {
            if (cancelInvoice.length > 0) {
                $.post('<%= VirtualPathUtility.ToAbsolute("~/_Test/WebDump.ashx")%>', JSON.stringify(cancelInvoice), function (html) {
                    $('<div></div>').html(html).dialog({ width: 640 });
                });
            } else {
                alert('請勾選欲作廢之發票!!');
            }
        }
    });
</script>