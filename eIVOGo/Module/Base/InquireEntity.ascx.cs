using System;
using System.Web.UI;
using System.ComponentModel;


namespace eIVOGo.Module.Base
{
    public abstract partial class InquireEntity<TEntity> : System.Web.UI.UserControl , IInquire
         where TEntity : class,new()
    {

        public event EventHandler Done;

        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.Button btnQuery;
        protected global::eIVOGo.Module.UI.FunctionTitleBar resultTitle;
        protected global::Uxnet.Web.Module.DataModel.DataModelCache modelItem;

        protected TEntity _item;

        protected abstract UserControl _itemList
        {
            get;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //modelItem.ItemType = typeof(TEntity);
            modelItem.Load += new EventHandler(modelItem_Load);
            this.PreRender += new EventHandler(InquireEntity_PreRender);
        }

        protected virtual void modelItem_Load(object sender, EventArgs e)
        {
            _item = (TEntity)modelItem.DataItem;
            if (_item != null)
            {
                btnQuery_Click(this, new EventArgs());
            }
        }

        [Bindable(true)]
        public bool DoDefaultQuery
        { get; set; }


        protected virtual void InquireEntity_PreRender(object sender, EventArgs e)
        {
            if (DoDefaultQuery)
            {
                btnQuery_Click(this, null);
            }

            if (btnQuery.CommandArgument == "Query")
            {
                buildQueryItem();
            }

            if (!this.IsPostBack && _item != null)
            {
                this.DataBind();
            }
        }

        protected virtual void buildQueryItem()
        {
            onDone(this, new EventArgs());
        }

        protected virtual void onDone(object sender, EventArgs e)
        {
            if (Done != null)
            {
                Done(sender,e);
            }
        }

        protected virtual void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected virtual void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.CommandArgument = "Query";
            _itemList.Visible = true;
            resultTitle.Visible = true;
        }

        public virtual void ResetQuery()
        {
            btnQuery.CommandArgument = "";
            _itemList.Visible = false;
            resultTitle.Visible = false;

        }

        public void DoQuery()
        {
            this.buildQueryItem();
        }
    }

    public abstract partial class InquireEntity : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected global::System.Web.UI.WebControls.Button btnAdd;
        protected global::System.Web.UI.WebControls.Button btnQuery;
        protected global::eIVOGo.Module.UI.FunctionTitleBar resultTitle;

        protected abstract UserControl _itemList
        {
            get;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InquireEntity_PreRender);
        }

        [Bindable(true)]
        public bool DoDefaultQuery
        { get; set; }


        protected virtual void InquireEntity_PreRender(object sender, EventArgs e)
        {
            if (DoDefaultQuery)
            {
                btnQuery_Click(this, null);
            }

            if (btnQuery.CommandArgument == "Query")
            {
                buildQueryItem();
            }

        }

        protected virtual void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            OnDone(null);
        }

        protected virtual void OnDone(EventArgs arg)
        {
            if (Done != null)
            {
                Done(this, arg);
            }
        }

        protected virtual void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected virtual void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.CommandArgument = "Query";
            _itemList.Visible = true;
            resultTitle.Visible = true;
        }

        public virtual void ResetQuery()
        {
            btnQuery.CommandArgument = "";
            _itemList.Visible = false;
            resultTitle.Visible = false;
        }
    }

    public interface IInquire
    {
        void DoQuery();
    }

}