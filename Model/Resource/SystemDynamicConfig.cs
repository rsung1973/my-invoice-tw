using System;
using System.Collections;

namespace Model.Resource
{
	/// <summary>
	/// SystemDynamicConfig 的摘要描述。
	/// </summary>
	public class SystemDynamicConfig
	{
		private static SystemDynamicConfig _instance;

		private IDictionary _config;

		private SystemDynamicConfig(IDictionary dictionary)
		{
			//
			// TODO: 在此加入建構函式的程式碼
			//
			_config = dictionary;
		}

		public static void CreateInstance(IDictionary dictionary)
		{
			_instance = new SystemDynamicConfig(dictionary);
		}

		public static IDictionary Values
		{
			get
			{
				return _instance._config;
			}
		}

		public static string BankCode
		{
			get
			{
				return (string)_instance._config["bankCode"];
			}
			set
			{
				_instance._config["bankCode"] = value;
			}
		}


		public static string ServerUrl
		{
			get
			{
				return (string)_instance._config["serverUrl"];
			}
			set
			{
				_instance._config["serverUrl"] = value;
			}
		}

		public static string NegoClientUrl
		{
			get
			{
				return (string)_instance._config["negoClient"];
			}
			set
			{
				_instance._config["negoClient"] = value;
			}
		}

		public static string NegoSealUrl
		{
			get
			{
				return (string)_instance._config["negoSealUrl"];
			}
			set
			{
				_instance._config["negoSealUrl"] = value;
			}
		}

		public static string FactoringClientUrl
		{
			get
			{
				return (string)_instance._config["factoringClient"];
			}
			set
			{
				_instance._config["factoringClient"] = value;
			}
		}


		public static ApprovalPolicy @ApprovalPolicy
		{
			get
			{
				return (null!=_instance._config["approvalPolicy"])?(ApprovalPolicy)Enum.Parse(typeof(ApprovalPolicy),(string)_instance._config["approvalPolicy"]):ApprovalPolicy.NeedAll;
			}
			set
			{
				_instance._config["approvalPolicy"] = value.ToString();
			}
		}

		public static NegotiationMode @NegotiationMode
		{
			get
			{
				return (null!=_instance._config["negoMode"])?(NegotiationMode)Enum.Parse(typeof(NegotiationMode),(string)_instance._config["negoMode"]):NegotiationMode.CenterBranch;
			}
			set
			{
				_instance._config["negoMode"] = value.ToString();
			}
		}

		public static bool UseSmtpAuthentication
		{
			get
			{
				return (null!=_instance._config["smtpAuth"])?bool.Parse((string)_instance._config["smtpAuth"]):false;
			}
			set
			{
				_instance._config["smtpAuth"] = value.ToString();
			}
		}

		public static string SmtpID
		{
			get
			{
				return (string)_instance._config["smtpID"];
			}
			set
			{
				_instance._config["smtpID"] = value;
			}
		}

		public static string SmtpPWD
		{
			get
			{
				return (string)_instance._config["smtpPWD"];
			}
			set
			{
				_instance._config["smtpPWD"] = value;
			}
		}

		public static string SmtpHost
		{
			get
			{
				return (string)_instance._config["smtpHost"];
			}
			set
			{
				_instance._config["smtpHost"] = value;
			}
		}


		public static string CP1Layout
		{
			get
			{
				return (string)_instance._config["CP1Layout"];
			}
			set
			{
				_instance._config["CP1Layout"] = value;
			}
		}



	}

	public enum ApprovalPolicy
	{
		NeedAll,
		SingleOnly
	}

	public enum NegotiationMode
	{
		CenterBranch,
		AdvisingBranch
	}
		
}
