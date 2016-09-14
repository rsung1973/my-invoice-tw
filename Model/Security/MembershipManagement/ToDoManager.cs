using System;
using System.Collections.Specialized;

using Model.BaseManagement;

namespace Model.Security.MembershipManagement
{
	/// <summary>
	/// Summary description for ToDoManager.
	/// </summary>
	public class ToDoManager
	{
		private HybridDictionary _toDo;		//用來儲存待處理或處理程序中的事項

		public ToDoManager()
		{
			//
			// TODO: Add constructor logic here
			//
			_toDo  = new HybridDictionary();
		}

        public void Clear()
        {
            _toDo.Clear();
        }

		public IManager	LCUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["LCUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("LCUpload");
				_toDo.Add("LCUpload",value);
			}
		}

		public IManager	DraftUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["DraftUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("DraftUpload");
				_toDo.Add("DraftUpload",value);
			}
		}

		public IManager	InvoiceUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["InvoiceUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("InvoiceUpload");
				_toDo.Add("InvoiceUpload",value);
			}
		}

		public IManager	ARUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["ARUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("ARUpload");
				_toDo.Add("ARUpload",value);
			}
		}

		public IManager	POUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["POUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("POUpload");
				_toDo.Add("POUpload",value);
			}
		}

		public IManager	PCUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["PCUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("PCUpload");
				_toDo.Add("PCUpload",value);
			}
		}

		public IManager	BAUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["BAUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("BAUpload");
				_toDo.Add("BAUpload",value);
			}
		}

		public IManager	DAUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["DAUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("DAUpload");
				_toDo.Add("DAUpload",value);
			}
		}


		public IManager	PPOUploadManager
		{
			get
			{
				object obj;
				return (obj=_toDo["PPOUpload"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("PPOUpload");
				_toDo.Add("PPOUpload",value);
			}
		}

		public IManager	InboxManager
		{
			get
			{
				object obj;
				return (obj=_toDo["inboxMgr"])!=null?(IManager)obj:null;
			}
			set
			{
				_toDo.Remove("inboxMgr");
				_toDo.Add("inboxMgr",value);
			}
		}





	}
}
