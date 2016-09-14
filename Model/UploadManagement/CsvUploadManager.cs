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
using System.Data.Linq;

namespace Model.UploadManagement
{
    public abstract class CsvUploadManager<T, TEntity, TUploadItem> : GenericManager<T,TEntity>, ICsvUploadManager, IUploadManager<TUploadItem>
        where T : DataContext, new()
        where TEntity : class,new()
        where TUploadItem : IItemUpload,new()
    {
        protected int __COLUMN_COUNT ;
        protected List<TUploadItem> _items;
        protected List<TUploadItem> _errorItems;
        protected UserProfileMember _userProfile;
        protected bool _bResult = false;
        protected bool _breakParsing = false;

        public CsvUploadManager() : base() {
            initialize();
        }
        public CsvUploadManager(GenericManager<T> manager)
            : base(manager)
        {
            initialize();
        }

        protected virtual void initialize()
        {
            __COLUMN_COUNT = 15;
        }


        public virtual void ParseData(UserProfileMember userProfile, String fileName, Encoding encoding)
        {
            _userProfile = userProfile;
            _bResult = false;

            using (StreamReader sr = new StreamReader(fileName, encoding))
            {
                sr.ReadLine();
                _items = new List<TUploadItem>();

                _bResult = true;

                String line;
                int lineIdx = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        continue;

                    lineIdx++;
                    TUploadItem item = new TUploadItem();
                    item.DataContent = line;
                    String[] column = line.Split(',');
                    if (column.Length < __COLUMN_COUNT)
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

        public List<TUploadItem> ItemList
        {
            get
            {
                return _items;
            }
        }

        public List<TUploadItem> ErrorList
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

        protected abstract bool validate(TUploadItem item);

    }

    public interface IItemUpload
    {
        string[] Columns { get; set; }
        String DataContent { get; set; }
        string Status { get; set; }
        Naming.UploadStatusDefinition UploadStatus { get; set; }
    }

    public class ItemUpload<TEntity> : IItemUpload
        where TEntity : class,new()
    {
        public TEntity Entity { get; set; }

        public string[] Columns
        {
            get;
            set;
        }

        public String DataContent
        {
            get;
            set;
        }


        public string Status
        {
            get;
            set;
        }

        public Naming.UploadStatusDefinition UploadStatus
        {
            get;
            set;
        }
    }

}
