using System;
using System.Web.UI;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

using Model.DataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.Base
{
    public abstract partial class CommonInquireEntity<TEntity> : InquireEntity<TEntity>
         where TEntity : class,new()
    {

        protected Control resultInfo;
        protected EntityItemList<EIVOEntityDataContext, TEntity> itemList;
        protected global::System.Web.UI.WebControls.PlaceHolder inquiryHolder;
        protected eIVOGo.Module.UI.PageAction actionItem;
        protected eIVOGo.Module.UI.FunctionTitleBar functionTitle;
        protected UserProfileMember _userProfile;
        protected global::System.Web.UI.UserControl urlGo;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += CommonInquireEntity_Load;
            _userProfile = WebPageUtility.UserProfile;
        }

        void CommonInquireEntity_Load(object sender, EventArgs e)
        {

            if (PrepareInquiryElement != null)
            {
                PrepareInquiryElement(inquiryHolder);
            }
            if (PrepareResultInfo != null)
            {
                PrepareResultInfo(resultInfo);
            }
            if (PrepareItemList != null)
            {
                itemList = PrepareItemList();
                resultInfo.Controls.Add(itemList);
            }
            if (PrepareNaming != null)
            {
                PrepareNaming(actionItem, functionTitle);
            }
            if (PrepareQueryDirective != null)
            {
                PrepareQueryDirective(urlGo);
            }

            itemList.Visible = resultInfo.Visible = btnQuery.CommandArgument == "Query";

        }

        protected override void btnQuery_Click(object sender, EventArgs e)
        {
            base.btnQuery_Click(sender, e);
            resultInfo.Visible = true;
        }

        protected override void buildQueryItem()
        {

        }

        public Action<Control> PrepareResultInfo
        { get; set; }

        public Func<EntityItemList<EIVOEntityDataContext, TEntity>> PrepareItemList
        { get; set; }

        public Action<Control> PrepareInquiryElement
        { get; set; }

        public Action<eIVOGo.Module.UI.PageAction, eIVOGo.Module.UI.FunctionTitleBar> PrepareNaming
        { get; set; }

        public Action<UserControl> PrepareQueryDirective
        { get; set; }

        public EntityItemList<EIVOEntityDataContext, TEntity> QueryItemList
        {
            get
            {
                return itemList;
            }
        }

        //public List<IInquireEntity<TEntity>> InquiryItem
        //{
        //    get
        //    {
        //        if (_inquiryItem == null)
        //            _inquiryItem = new List<IInquireEntity<TEntity>>();
        //        return _inquiryItem;
        //    }
        //}

        //public Expression<Func<TEntity, bool>> DefaultQueryExpression
        //{
        //    get
        //    {
        //        return _queryExpr;
        //    }
        //    set
        //    {
        //        _queryExpr = value;
        //    }
        //}
    }

    public interface IInquireEntity<TEntity>
         where TEntity : class,new()
    {
        Expression<Func<TEntity, bool>> BuildQueryExpression(Expression<Func<TEntity, bool>> queryExpr);
        bool QueryRequired
        { get; set; }
        bool HasSet
        { get; set; }
        String AlertMessage
        { get; set; }
    }

    public partial class EntityUIField<TEntity> : UserControl
         where TEntity : class,new()
    {
        [System.ComponentModel.Bindable(true)]
        public TEntity DataItem
        { get; set; }

        public override void DataBind()
        {
            if (DataItem == null)
                DataItem = Page.GetDataItem() as TEntity;
            if (DataItem != null)
                base.DataBind();
        }
    }

}