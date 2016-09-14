using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using DataAccessLayer.basis;

namespace Model.InvoiceManagement
{
    public abstract class GoogleUploadManager<TEntity, TGoogleItem> : EIVOEntityManager<TEntity>, IGoogleUploadManager
        where TEntity : class,new()
        where TGoogleItem : IItemUpload,new()
    {
        protected int __COLUMN_COUNT ;
        protected List<TGoogleItem> _items;
        protected List<TGoogleItem> _errorItems;
        protected UserProfileMember _userProfile;
        protected bool _bResult = false;
        protected bool _breakParsing = false;

        public GoogleUploadManager() : base() {
            initialize();
        }
        public GoogleUploadManager(GenericManager<EIVOEntityDataContext> manager) : base(manager) {
            initialize();
        }

        protected virtual void initialize()
        {
            __COLUMN_COUNT = 15;     //若CSV檔要增加備註欄位,則此數值要調成16   2013/01/07 Howard
        }


        public virtual void ParseData(UserProfileMember userProfile, String fileName, Encoding encoding)
        {
            _userProfile = userProfile;
            _bResult = false;

            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                sr.ReadLine();
                _items = new List<TGoogleItem>();

                _bResult = true;

                String line;
                int lineIdx = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        continue;

                    lineIdx++;
                    TGoogleItem item = new TGoogleItem();
                    String[] column = encoding.CodePage == 936 ? line.ToTraditional().Split(',') : line.Split(',');
                    if (column.Length != __COLUMN_COUNT)
                    {
                        item.Columns = (String[])Array.CreateInstance(typeof(String), __COLUMN_COUNT);
                        item.Status = String.Format("第{0}筆:資料欄位錯誤", lineIdx);
                        _bResult = false;
                    }
                    else
                    {
                        item.Columns = column.Select(s => s.Trim()).ToArray();
                        validate(item);
                    }

                    if (String.IsNullOrEmpty(item.Status))
                    {
                        item.UploadStatus = Naming.UploadStatusDefinition.等待匯入;
                    }
                    else
                    {
                        item.UploadStatus = Naming.UploadStatusDefinition.資料錯誤;
                        item.Status = String.Format("第{0}筆:{1}", lineIdx, item.Status.Substring(1));
                    }
                    _items.Add(item);

                    if (_breakParsing)
                        break;
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

        public List<TGoogleItem> ItemList
        {
            get
            {
                return _items;
            }
        }

        public List<TGoogleItem> ErrorList
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
                    //                    item.Status = Enum.GetName(typeof(Naming.UploadStatusDefinition), Naming.UploadStatusDefinition.匯入成功);
                    item.UploadStatus = Naming.UploadStatusDefinition.匯入成功;
                }
                return true;
            }
            return false;
        }

        protected abstract bool validate(TGoogleItem item);

    }

    public interface IItemUpload
    {
        string[] Columns { get; set; }
        string Status { get; set; }
        Naming.UploadStatusDefinition UploadStatus { get; set; }
    }

}
