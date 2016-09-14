using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ModelExtension.Properties;
using Utility;

namespace ModelExtension.MessageManagement
{
    public class SMSHelper
    {
        private EV8DSMS.SMS smsService;
        private string sessionKey = "";
        private string processMsg = "";
        private string batchID = "";
        private double credit = 0;

        public SMSHelper()
        {

        }


        public bool Start()
        {
            smsService = new EV8DSMS.SMS();
            return getConnection(Settings.Default.EV8D_ID, Settings.Default.EV8D_PASSWORD);
        }

        public void Close()
        {
            this.closeConnection();
            smsService.Dispose();
        }

        public bool SendSMS(string subject, string content, string mobile)
        {
            return sendSMS(subject, content, mobile, String.Empty);
        }

        public String GetDeliveryStatus()
        {
            SMSStatus status;
            if (batchID != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.smsService.getDeliveryStatus(this.sessionKey, batchID, null));
                XmlElement elmt = (XmlElement)doc.SelectSingleNode("/SMS_LOG/GET_DELIVERY_STATUS/SMS/STATUS");
                if (elmt != null)
                {
                    return Enum.TryParse(elmt.InnerText, out status) ? status.ToString() : elmt.InnerText;
                }
            }
            else if (processMsg != null)
            {
                return Enum.TryParse(processMsg, out status) ? status.ToString() : processMsg;
            }
            return null;
        }

        /// <summary>
        /// 提供啟動連結服務，傳送簡訊前必須先取得取得SessionKey，
        /// 藉由此SessionKey進行後續簡訊傳送之服務，SessionKey有效期為單日，隔日即失效。
        /// 建議於完成傳送作業後，執行closeConnection以確保安全。
        /// </summary>
        /// <param name="userID">登入帳號</param>
        /// <param name="password">密碼</param>
        /// <returns>true:取得連線成功；false:取得連線失敗</returns>
        private bool getConnection(string userID, string password)
        {
            bool success = false;
            try
            {
                string resultXml = this.smsService.getConnection(userID, password);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(resultXml);
                string returnCode = xmlDoc.SelectSingleNode("/SMS/GET_CONNECTION/CODE").InnerText;
                if (returnCode.Equals("0"))
                {
                    this.sessionKey = xmlDoc.SelectSingleNode("/SMS/GET_CONNECTION/SESSION_KEY").InnerText;
                    this.processMsg = xmlDoc.SelectSingleNode("/SMS/GET_CONNECTION/DESCRIPTION").InnerText;
                    success = true;
                }
                else
                {
                    this.processMsg = xmlDoc.SelectSingleNode("/SMS/GET_CONNECTION/DESCRIPTION").InnerText;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.processMsg = ex.ToString();
            }
            return success;
        }

        /// <summary>
        /// SessionKey有效期為單日，隔日即失效。建議於完成傳送作業後，執行closeConnection以確保安全
        /// </summary>
        /// <returns>true:關閉成功；false:關閉失敗</returns>
        private bool closeConnection()
        {
            bool success = false;
            try
            {
                string resultString = this.smsService.closeConnection(this.sessionKey);
                if (!resultString.StartsWith("-"))
                {
                    this.sessionKey = "";
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.processMsg = ex.ToString();
            }
            return success;
        }

        /// <summary>
        /// 傳送簡訊
        /// </summary>
        /// <param name="subject">簡訊主旨，主旨不會隨著簡訊內容發送出去。用以註記本次發送之用途。可傳入空字串。</param>
        /// <param name="content">簡訊發送內容</param>
        /// <param name="mobile">接收人之手機號碼。格式為: +886912345678或09123456789。多筆接收人時，請以半形逗點隔開( , )，如0912345678,0922333444。</param>
        /// <param name="sendTime">簡訊預定發送時間。-立即發送：請傳入空字串。-預約發送：請傳入預計發送時間，若傳送時間小於系統接單時間，將不予傳送。格式為YYYYMMDDhhmnss；例如:預約2009/01/31 15:30:00發送，則傳入20090131153000。若傳遞時間已逾現在之時間，將立即發送。</param>
        /// <returns>true:傳送成功；false:傳送失敗</returns>
        private bool sendSMS(string subject, string content, string mobile, string sendTime)
        {
            bool success = false;
            try
            {
                if (!string.IsNullOrEmpty(sendTime))
                {
                    try
                    {
                        //檢查傳送時間格式是否正確
                        DateTime checkDt = DateTime.ParseExact(sendTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        if (!sendTime.Equals(checkDt.ToString("yyyyMMddHHmmss")))
                        {
                            this.processMsg = "傳送時間格式錯誤";
                            return false;
                        }
                    }
                    catch
                    {
                        this.processMsg = "傳送時間格式錯誤";
                        return false;
                    }
                }

                batchID = null;
                string resultString = this.smsService.sendSMS(this.sessionKey, subject, content, mobile, sendTime);
                if (!resultString.StartsWith("-"))
                {
                    /* 
                     * 傳送成功 回傳字串內容格式為：CREDIT,SENDED,COST,UNSEND,BATCH_ID，各值中間以逗號分隔。
                     * CREDIT：發送後剩餘點數。負值代表發送失敗，系統無法處理該命令
                     * SENDED：發送通數。
                     * COST：本次發送扣除點數
                     * UNSEND：無額度時發送的通數，當該值大於0而剩餘點數等於0時表示有部份的簡訊因無額度而無法被發送。
                     * BATCH_ID：批次識別代碼。為一唯一識別碼，可藉由本識別碼查詢發送狀態。格式範例：220478cc-8506-49b2-93b7-2505f651c12e
                     */
                    string[] split = resultString.Split(',');
                    this.credit = Convert.ToDouble(split[0]);
                    TotalSent = int.Parse(split[1]);
                    Cost = double.Parse(split[2]);
                    TotalUnsent = int.Parse(split[3]);
                    this.batchID = split[4];
                    processMsg = split[4];
                    success = true;
                }
                else
                {
                    //傳送失敗
                    this.processMsg = resultString;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.processMsg = ex.ToString();
            }
            return success;
        }

        /// <summary>
        /// 取得帳號餘額
        /// </summary>
        /// <returns>true:取得成功；false:取得失敗</returns>
        public bool GetCredit()
        {
            bool success = false;
            try
            {
                string resultString = this.smsService.getCredit(this.sessionKey);
                if (!resultString.StartsWith("-"))
                {
                    this.credit = Convert.ToDouble(resultString);
                    success = true;
                }
                else
                {
                    this.processMsg = resultString;
                }
            }
            catch (Exception ex)
            {
                this.processMsg = ex.ToString();
            }
            return success;
        }

        public bool CheckConnected()
        {
            if (!string.IsNullOrEmpty(this.sessionKey))
            {
                return true;
            }
            return false;
        }

        public string ProcessMsg
        {
            get
            {
                return this.processMsg;
            }
        }

        public string BatchID
        {
            get
            {
                return this.batchID;
            }
        }

        public double Credit
        {
            get
            {
                return this.credit;
            }
        }

        public double Cost
        {
            get;
            private set;
        }

        public int TotalSent
        {
            get;
            private set;
        }

        public int TotalUnsent
        {
            get;
            private set;
        }
    }
}
