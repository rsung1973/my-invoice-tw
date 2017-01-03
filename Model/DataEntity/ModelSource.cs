using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using DataAccessLayer.basis;
using Model.Locale;
using Uxnet.Com.DataAccessLayer.Models;

namespace Model.DataEntity
{
    public class ModelSource<TEntity> : ModelSource<EIVOEntityDataContext, TEntity>
        where TEntity : class,new()
    {

        public ModelSource() : base() { }
        public ModelSource(GenericManager<EIVOEntityDataContext> manager) : base(manager) { }

        //private void applyModelSource()
        //{
        //    _inquiry.ModelSource = this;
        //}


        public Naming.DataResultMode ResultModel
        {
            get;
            set;
        }

        public int InquiryPageSize
        { get; set; }

        public int InquiryPageIndex
        { get; set; }
    }

    public partial class ModelSourceInquiry<TEntity> : ModelSourceInquiry<EIVOEntityDataContext, TEntity>
        where TEntity : class,new()
    {


    }

    
}