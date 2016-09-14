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
    public partial class InquireAllowance : InquireAllowanceBasic
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += InquireAllowance_Load;
        }

        void InquireAllowance_Load(object sender, EventArgs e)
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
            else if (Request["q"] == "f")
            {
                buildQueryItem();
                var item = itemList.GetQueryResult(itemList.Select()).FirstOrDefault();
                if (item != null)
                {
                    Page.Items["dataItem"] = item;
                    Server.Transfer("~/_test/TestFields.aspx");
                }
            }
            else
            {
                buildQueryItem();
                var item = itemList.GetQueryResult(itemList.Select()).FirstOrDefault();
                itemList.PrepareJsGridField(item);
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
                return table.Join(buildDefaultQuery(ctx, ctx.GetTable<InvoiceAllowance>()),
                    d => d.DocID, i => i.AllowanceID, (d, i) => d)
                    .OrderByDescending(d => d.DocID);
            };
        }
    }
}