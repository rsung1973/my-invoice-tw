using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Model.DataEntity;
using Utility;

namespace eIVOGo.Controllers
{
    public class SampleController<TEntity> : Controller
        where TEntity : class, new()
    {
        protected ModelSource<TEntity> models;

        protected SampleController() :base()
        {
            models = new ModelSource<TEntity>();
            
        }

        public ModelSource<TEntity> DataSource
        {
            get
            {
                return models;
            }
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            models.Dispose();
        }
    }
}