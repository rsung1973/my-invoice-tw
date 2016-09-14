using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Uxnet.Web.Module.Common;
using Uxnet.Web.Module.DataModel;
using System.ComponentModel;
using System.Web.Script.Serialization;

namespace eIVOGo.Module.Base
{
    public abstract partial class JsGridDataSource<T, TEntity> : EntityDataSource<T, TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {

        public void RenderJsGridDataResult(int pageIndex, int pageSize)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(doJsonSerialize(serializer, pageIndex, pageSize));
            Response.End();
        }

        protected virtual string doJsonSerialize(JavaScriptSerializer serializer, int pageIndex, int pageSize)
        {
            var items = this.Select();

            return serializer.Serialize(
                    new
                    {
                        data = items.Skip(pageIndex * pageSize).Take(pageSize).ToArray(),
                        itemsCount = items.Count()
                    });
        }

        public virtual IEnumerable<Object> GetQueryResult(IEnumerable<TEntity> items)
        {
            return items;
        }

        public virtual IEnumerable<Object> GetCsvResult(IEnumerable<TEntity> items)
        {
            return items;
        }

        public virtual IEnumerable<Object> GetCsv4BuyerAddrResult(IEnumerable<TEntity> items)
        {
            return items;
        }

        public virtual void PrepareJsGridField(Object item)
        {

        }
            

        [Bindable(true)]
        public abstract bool AllowPaging
        {
            get;
            set;
        }

        [Bindable(true)]
        public abstract bool PrintMode
        { get; set; }
        
    }
}