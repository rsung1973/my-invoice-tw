using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace Model.UploadManagement
{
    public abstract class XmlUploadManager<T, TEntity> : GenericManager<T, TEntity>, ICsvUploadManager, IUploadManager<ItemUpload<TEntity>>
        where T : DataContext, new()
        where TEntity : class,new()
    {
        protected List<ItemUpload<TEntity>> _items;
        protected List<ItemUpload<TEntity>> _errorItems;
        protected UserProfileMember _userProfile;
        protected bool _bResult = false;
        protected bool _breakParsing = false;

        public XmlUploadManager() : base() {
            initialize();
        }
        public XmlUploadManager(GenericManager<T> manager)
            : base(manager)
        {
            initialize();
        }

        protected virtual void initialize()
        {
        }


        public virtual void ParseData(UserProfileMember userProfile, String fileName, Encoding encoding)
        {
            _userProfile = userProfile;
            _bResult = false;

            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            var items = ((TEntity[])doc.DeserializeDataContract<TEntity[]>()).Select(i => new ItemUpload<TEntity>
            {
                Entity = i,
                UploadStatus = Naming.UploadStatusDefinition.等待匯入
            });

            _items = new List<ItemUpload<TEntity>>();
            _bResult = true;
            _items.AddRange(items);

            int index = 1;
            foreach (var item in _items)
            {
                if (!validate(item))
                {
                    item.Status = String.Format("第{0}筆:{1}", ++index, item.Status);
                }
            }

            if (!IsValid)
            {
                _errorItems = _items.Where(i => !String.IsNullOrEmpty(i.Status)).ToList();
            }
        }

        public bool IsValid
        {
            get
            {
                return _bResult;
            }
        }

        public List<ItemUpload<TEntity>> ItemList
        {
            get
            {
                return _items;
            }
        }

        public List<ItemUpload<TEntity>> ErrorList
        {
            get
            {
                return _errorItems;
            }
        }


        public int ItemCount
        {
            get
            {
                return _items.Count;
            }
        }

        protected abstract void doSave();

        public bool Save()
        {
            if (_bResult)
            {
                doSave();
                foreach (var item in _items)
                {
                    item.UploadStatus = Naming.UploadStatusDefinition.匯入成功;
                }
                return true;
            }
            return false;
        }

        protected abstract bool validate(ItemUpload<TEntity> item);

    }

}
