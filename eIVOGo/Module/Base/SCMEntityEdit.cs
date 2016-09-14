using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eIVOGo.Module.Base
{
    public abstract partial class SCMEntityEdit<TEntity> : SCMEntityPreview<TEntity>
         where TEntity : class,new()
    {

        protected override void modelItem_Load(object sender, EventArgs e)
        {
            if (_item == null)
            {
                _item = (TEntity)modelItem.DataItem;
                if (_item != null)
                {
                    prepareDataForViewState();
                }
                else
                {
                    prepareInitialData();
                    if (modelItem.DataItem == null)
                        modelItem.DataItem = _item;
                }
            }
        }

        protected abstract void prepareInitialData();

    }
}