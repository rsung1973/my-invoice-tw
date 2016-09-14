using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using DataAccessLayer.basis;
using eIVOGo.Helper;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using Uxnet.Web.Module.DataModel;
using Utility;
using System.Web.UI;

namespace eIVOGo.Models
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    public abstract partial class DataListSource<TEntity> : ViewUserControl
        where TEntity : class, new()
    {
        protected ModelSource<TEntity> models;
        protected Control _jsGridFields;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += ModelDataSource_Load;
            models = TempData.GetModelSource<TEntity>();
            this.JsonSerialize = this.doJsonSerialize;
        }

        void ModelDataSource_Load(object sender, EventArgs e)
        {
            if (Request["q"] == "1")
            {
                int pageIndex = 0, pageSize = 10;
                if (Request.GetRequestValue("index", out pageIndex) && pageIndex > 0)
                {
                    pageIndex--;
                }
                Request.GetRequestValue("size", out pageSize);
                RenderJsGridDataResult(pageIndex, pageSize);
            }
        }

        public void RenderJsGridDataResult(int pageIndex, int pageSize)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(JsonSerialize(serializer, pageIndex, pageSize));
            Response.End();
        }

        public ModelSource<TEntity> ModelSource
        {
            get
            {
                return models;
            }
        }

        public Func<JavaScriptSerializer, int, int, String> JsonSerialize
        {
            get;
            set;
        }

        protected virtual string doJsonSerialize(JavaScriptSerializer serializer, int pageIndex, int pageSize)
        {
            var items = models.Items;

            return serializer.Serialize(
                    new
                    {
                        data = items.Skip(pageIndex * pageSize).Take(pageSize).ToArray(),
                        itemsCount = items.Count()
                    });
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public Control JsGridFields
        {
            get
            {
                return _jsGridFields;
            }
            set
            {
                _jsGridFields = value;
                if (_jsGridFields != null)
                {
                    this.Controls.Add(_jsGridFields);
                }
            }
        }
        
    }
}