<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Visitor" %>
<%@ Import Namespace="Utility" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register src="../Module/SCM/CreateBuyerOrder.ascx" tagname="CreateBuyerOrder" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <title></title>
    <style type="text/css">
        .test
        {
            overflow: scroll;
            height: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
  
   
    <uc1:CreateBuyerOrder ID="CreateBuyerOrder1" runat="server" />
 
   
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //        Page.Error += new EventHandler(Page_Error);
        if (Request["id"] != null)
        {
        }
        else if (Page.PreviousPage != null)
        {
        }
        this.Load += new EventHandler(published_testpage_aspx_Load);

    }

    void Selector_DataBound(object sender, EventArgs e)
    {
    }

    void published_testpage_aspx_Load(object sender, EventArgs e)
    {
        //if (this.IsPostBack)
        //{
        //    PRODUCTS_DATA item = ((String)ViewState["item"]).DeserializeDataContract<PRODUCTS_DATA>();
        //    dsEntity.CreateDataManager().EntityList.Attach(item);
        //}
        //else
        //{
        //    ViewState["item"] = dsEntity.CreateDataManager().EntityList.First().SerializeDataContract();
        //}
    }

    //void Page_Error(object sender, EventArgs e)
    //{
    //    Exception ex = Server.GetLastError();
    //    if (ex != null)
    //    {
    //        Page.Items["error"] = ex;
    //        Server.ClearError();
    //        Server.Execute("~/Published/ErrorPage.aspx", Response.Output);
    //    }
    //}
    
</script>
