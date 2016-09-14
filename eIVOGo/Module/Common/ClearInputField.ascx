<%@ Control Language="C#" AutoEventWireup="true" %>
<input type="button" value="重填" class="btn" onclick="javascript:clearInput();" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (ScriptManager.GetCurrent(Page) != null)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clearInput", buildScript(), true);
        }
        else
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "clearInput", buildScript(), true);
        }
    }

    private String buildScript()
    {
        return @"
    function clearInput() {
        var items = document.forms[0].getElementsByTagName('input');
        for (var idx = 0; idx < items.length; idx++) {
            var item = items[idx];
            switch (item.type) {
                case 'radio':
                case 'checkbox':
                    item.checked = false;
                    break;
                case 'button':
                case 'file':
                case 'hidden':
                case 'image':
                case 'reset':
                case 'submit':
                    break;
                case 'password':
                case 'text':
                    item.value = '';
            }
        }
    }
            ";
    }
    
</script>