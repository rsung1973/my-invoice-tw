<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc1" TagName="DataModelCache" %>
<%@ Import Namespace="Utility" %>

<iframe src="<%= VirtualPathUtility.ToAbsolute("~/Helper/RenderContent.aspx?control=~/Module/SAM/Business/ProxySettingOrganizationList.ascx") %>" width="100%" height="100%"></iframe>
<uc1:DataModelCache runat="server" ID="modelItem" KeyName="CompanyID" />
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        int companyID;
        if(Request.GetRequestValue("companyID",out companyID))
        {
            modelItem.DataItem = companyID;
        }
    }
</script>
