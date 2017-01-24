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
using Uxnet.Web.Controllers;

namespace eIVOGo.Controllers
{
    public class SampleController<TEntity> : SampleController<EIVOEntityDataContext, TEntity>
        where TEntity : class, new()
    {

        protected SampleController() : base()
        {
            models = new ModelSource<TEntity>(models);
        }
        public new ModelSource<TEntity> DataSource
        {
            get
            {
                return (ModelSource<TEntity>)models;
            }
        }
    }
}