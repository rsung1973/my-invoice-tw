using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Utility;

namespace eIVOGo.Module.JsGrid
{
    public partial class InquireInvoice : InquireInvoiceBasic
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += InquireInvoice_Load;
        }

        void InquireInvoice_Load(object sender, EventArgs e)
        {
            if (Request["q"] == "1")
            {
                int pageIndex = 0, pageSize = 10;
                if (Request.GetRequestValue("index", out pageIndex) && pageIndex > 0)
                {
                    pageIndex--;
                }
                Request.GetRequestValue("size", out pageSize);

                buildQueryItem();
                itemList.RenderJsGridDataResult(pageIndex, pageSize);
            }
            else if(Request["q"] == "f")
            {
                buildQueryItem();
                var item = itemList.GetQueryResult(itemList.Select()).FirstOrDefault();
                if(item!=null)
                {
                    Page.Items["dataItem"] = item;
                    Server.Transfer("~/_test/TestFields.aspx");
                }
            }
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }

        protected override void buildQueryItem()
        {
            itemList.BuildQuery = table =>
                {
                    var ctx = (EIVOEntityDataContext)table.Context;
                    return table.Join(buildDefaultQuery(ctx, ctx.GetTable<InvoiceItem>()),
                        d => d.DocID, i => i.InvoiceID, (d, i) => d)
                        .OrderBy(i => i.InvoiceItem.InvoiceID);
                };
        }

    }
}