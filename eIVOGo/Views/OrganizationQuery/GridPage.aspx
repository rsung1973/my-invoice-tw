<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<script runat="server">

    ModelSource<Organization> models;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        IEnumerable<Organization> items = (IEnumerable<Organization>)Model;
        models = TempData.GetModelSource<Organization>();
        
        Response.Clear();
        Response.ContentType = "application/json";
        Response.Write(serializer.Serialize(
                new
                {
                    data = GetQueryResult(items),
                    itemsCount = models.Items.Count()
                }));
        Response.End();
    }

    public IEnumerable<object> GetQueryResult(IEnumerable<Organization> items)
    {
        return items.Select(d => new
        {
            d.CompanyID,
            d.ReceiptNo,
            d.UndertakerName,
            d.ContactEmail,
            Status = d.OrganizationStatus!=null && d.OrganizationStatus.LevelExpression!=null ? d.OrganizationStatus.LevelExpression.Description : null,
            d.CompanyName,
            Editable = d.OrganizationStatus != null && d.OrganizationStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete,
            Enabled = d.OrganizationStatus!=null && d.OrganizationStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete,
            ApplyProxy =  checkCompany(d)
        });
    }    

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }
    
    bool checkCompany(Model.DataEntity.Organization Org)
    {
        return Org.OrganizationCategory.Count(c => c.CategoryID == (int)Naming.B2CCategoryID.店家發票自動配號
            || c.CategoryID == (int)Naming.B2CCategoryID.店家) > 0;
    }
    

</script>