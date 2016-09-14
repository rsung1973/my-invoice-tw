<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="Utility" %>
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<script runat="server">

    CDS_Document _item;
    int _index;
    int _docID;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += helper_editinvoicebuyer_aspx_PreRender;

        doUpdate();
    }

    void doUpdate()
    { 
        if (Request["index"] != null && int.TryParse(Request["index"], out _index))
        {
            if (Request["docID"] != null && int.TryParse(Request["docID"], out _docID))
            {
                var mgr =  dsEntity.CreateDataManager();
                _item = mgr.EntityList.Where(d => d.DocID == _docID).FirstOrDefault();
                InvoiceBuyer buyer;

                if (_item == null || _item.InvoiceItem == null)
                {
                    Page.Items["message"] = "發票資料不存在!!";
                }
                else if ((buyer = _item.InvoiceItem.InvoiceBuyer) == null)
                {
                    Page.Items["message"] = "發票買受人資料不存在!!";
                }
                else
                {
                    buyer.CustomerName = Request["CustomerName"].TrimOrNull();
                    buyer.ContactName = Request["ContactName"].TrimOrNull();
                    buyer.Address = Request["Address"].TrimOrNull();
                    buyer.Phone = Request["Phone"].TrimOrNull();

                    mgr.SubmitChanges();
                    
                    Page.Items["message"] = "資料已更新!!";
                }
                
                Server.Transfer("LoadInvoiceBuyer.aspx");
            }
        }
    }

    void helper_editinvoicebuyer_aspx_PreRender(object sender, EventArgs e)
    {
        if (_item != null)
        {
            Page.Items["message"] = "資料已更新!!";
            Server.Transfer("LoadInvoiceBuyer.aspx");
        }
        else
        {
            this.Visible = false;
        }
    }
    
</script>
