using System;
using System.Collections.Specialized;
using System.Collections;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Summary description for DraftMailBean.
    /// </summary>
    public class PageMailGenerator : System.IDisposable
    {
        private string _msg;
        private string _url;
        private IList _recipient;
        private string _from;
        private string _sender;
        private string _smtpServer;
        private string _storePathIfFailed;
        private bool _bDisposed = false;
        private TextWriter _log;

        private object _key;

        private static HybridDictionary _instanceMgr;

        public PageMailGenerator()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void AddToManager(object key)
        {
            if (key != _key)
            {
                if (_instanceMgr == null)
                {
                    _instanceMgr = new HybridDictionary();
                }
                _key = key;
                _instanceMgr.Add(key, this);
            }
        }

        public static PageMailGenerator GetInstance(object key)
        {
            if (_instanceMgr != null && _instanceMgr.Contains(key))
            {
                return (PageMailGenerator)_instanceMgr[key];
            }
            else
            {
                return null;
            }
        }

        public PageMailGenerator(string mailMessage)
            : this()
        {
            _msg = mailMessage;
        }

        public TextWriter Log
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
            }
        }

        public string MailMessage
        {
            get
            {
                return _msg;
            }
            set
            {
                _msg = value;
            }
        }

        public string PageUrl
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
            }
        }

        public IList Recipient
        {
            get
            {
                return _recipient;
            }
            set
            {
                _recipient = value;
            }
        }

        public String RecipientInString { get; set; }


        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }

        public string Sender
        {
            get
            {
                return _sender;
            }
            set
            {
                _sender = value;
            }
        }

        public string SMTP_Server
        {
            get
            {
                return _smtpServer;
            }
            set
            {
                _smtpServer = value;
            }
        }

        public string StorePathIfFailed
        {
            get
            {
                return _storePathIfFailed;
            }
            set
            {
                _storePathIfFailed = value;
            }
        }

        public void Publish()
        {
            //²£¥Ímail body
            buildMessage();
        }

        private void buildMessage()
        {

            CDO.MessageClass message = new CDO.MessageClass();
            CDO.ConfigurationClass cfg = new CDO.ConfigurationClass();

            ADODB.Fields flds = cfg.Fields;

            flds["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = _smtpServer;
            flds["http://schemas.microsoft.com/cdo/configuration/smtpserverport"].Value = 25;
            flds["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = CDO.CdoSendUsing.cdoSendUsingPort;
            flds["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = _sender;
            flds["http://schemas.microsoft.com/cdo/configuration/smtpuserreplyemailaddress"].Value = _from;
            flds["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = CDO.CdoProtocolsAuthentication.cdoAnonymous;
            //			flds["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = "domain\username";
            //			flds["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value    = "password";
            //			flds["http://schemas.microsoft.com/cdo/configuration/smtpaccountname"].Value          = "My Name";
            flds.Update();

            message.Configuration = cfg;
            message.Charset = "utf-8";
            message.BodyPart.Charset = "utf-8";
            message.Subject = _msg;
            message.CreateMHTMLBody(_url, CDO.CdoMHTMLFlags.cdoSuppressNone, null, null);
            message.To = getSellerReceipent();
            message.From = _from;
            message.Sender = _sender;
            try
            {
                message.Send();
            }
            catch (Exception e)
            {
                Logger.Error(e);

                if (_storePathIfFailed != null && _storePathIfFailed.Length > 0)
                {
                    if (!Directory.Exists(_storePathIfFailed))
                    {
                        Directory.CreateDirectory(_storePathIfFailed);
                    }

                    string fileName = _storePathIfFailed + "\\" + System.Guid.NewGuid().ToString() + ".eml";
                    ADODB.Stream sc = message.GetStream();
                    sc.SaveToFile(fileName, ADODB.SaveOptionsEnum.adSaveCreateOverWrite);

                }
                if (_log != null)
                {
                    _log.WriteLine(e.Message);
                    _log.WriteLine(e.StackTrace);
                }
            }

        }

        private string getSellerReceipent()
        {
            List<String> items = new List<string>();
            items.Add(RecipientInString);

            if (_recipient != null && _recipient.Count > 0)
            { 
                foreach(String item in _recipient)
                {
                    items.Add(item);
                }
            }

            if (items.Count > 0)
            {
                return String.Join(", ", items.ToArray());
            }
            else
            {
                return null;
            }

        }
        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add PageMailGenerator.Dispose implementation
            if (!_bDisposed)
            {
                _bDisposed = true;
                if (_key != null && _instanceMgr != null)
                {
                    _instanceMgr.Remove(_key);
                }
            }
        }
        #endregion
    }

}
