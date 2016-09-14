using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.XPath;

using Utility;

namespace eIVOGo.Module.SYS
{
    public partial class MenuFactory : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public XElement Save()
        {
            bool hasValue = false;

            XElement rootMenu = rootItem.Save(null);
            if (putOrderItem.Save(rootMenu) != null) hasValue = true;
            if (approveOrderItem.Save(rootMenu) != null) hasValue = true;
            if (returnOrderItem.Save(rootMenu) != null) hasValue = true;
            if (storeAwayItem.Save(rootMenu) != null) hasValue = true;
            if (buyerOrderItem.Save(rootMenu) != null) hasValue = true;
            if (shipItem.Save(rootMenu) != null) hasValue = true;
            if (returnItem.Save(rootMenu) != null) hasValue = true;
            if (exchangeItem.Save(rootMenu) != null) hasValue = true;
            if (mailItem.Save(rootMenu) != null) hasValue = true;
            if (inquireStockItem.Save(rootMenu) != null) hasValue = true;
            if (inquireDifferentialItem.Save(rootMenu) != null) hasValue = true;

            return rootMenu;
        }

        internal void BindData(XElement menuItem)
        {
            IEnumerable<XElement> elements = menuItem.XPathSelectElements(".//menuItem").Where(e => e.Attribute("default") == null);
            putOrderItem.RestoreCheckItem(elements);
            approveOrderItem.RestoreCheckItem(elements);
            returnOrderItem.RestoreCheckItem(elements);
            shipItem.RestoreCheckItem(elements);
            inquireDifferentialItem.RestoreCheckItem(elements);
            exchangeItem.RestoreCheckItem(elements);
            returnItem.RestoreCheckItem(elements);
            storeAwayItem.RestoreCheckItem(elements);
            mailItem.RestoreCheckItem(elements);
            buyerOrderItem.RestoreCheckItem(elements);
            inquireStockItem.RestoreCheckItem(elements);
        }

        internal void Reset()
        {
            putOrderItem.Enabled = false;
            approveOrderItem.Enabled = false;
            returnOrderItem.Enabled = false;
            shipItem.Enabled = false;
            inquireDifferentialItem.Enabled = false;
            exchangeItem.Enabled = false;
            returnItem.Enabled = false;
            storeAwayItem.Enabled = false;
            mailItem.Enabled = false;
            buyerOrderItem.Enabled = false;
            inquireStockItem.Enabled = false;
        }

        internal void SetViewOnly()
        {
            putOrderItem.ViewOnly = true;
            approveOrderItem.ViewOnly = true;
            returnOrderItem.ViewOnly = true;
            shipItem.ViewOnly = true;
            inquireDifferentialItem.ViewOnly = true;
            exchangeItem.ViewOnly = true;
            returnItem.ViewOnly = true;
            storeAwayItem.ViewOnly = true;
            mailItem.ViewOnly = true;
            buyerOrderItem.ViewOnly = true;
            inquireStockItem.ViewOnly = true;
        }

        internal String GetFirstFunctionUrl()
        {
            if (exchangeItem.Enabled)
                return exchangeItem.ActionUrl;
            else if (putOrderItem.Enabled)
                return putOrderItem.ActionUrl;
            else if (returnItem.Enabled)
                return returnItem.ActionUrl;
            else if (inquireStockItem.Enabled)
                return inquireStockItem.ActionUrl;
            else if (approveOrderItem.Enabled)
                return approveOrderItem.ActionUrl;
            else if (returnOrderItem.Enabled)
                return returnOrderItem.ActionUrl;
            else if (shipItem.Enabled)
                return shipItem.ActionUrl;
            else if (inquireDifferentialItem.Enabled)
                return inquireDifferentialItem.ActionUrl;
            else if (storeAwayItem.Enabled)
                return storeAwayItem.ActionUrl;
            else if (mailItem.Enabled)
                return mailItem.ActionUrl;
            else if (buyerOrderItem.Enabled)
                return buyerOrderItem.ActionUrl;
            else return "~/logout.aspx";
        }

        internal static string GetAuthorizationInfo(XElement element)
        {
            return element.XPathSelectElements(".//menuItem")
                                .Where(e => e.Attribute("default") == null)
                                .Select(e => e.Attribute("value").Value)
                                .Concatenate("<br/>");
        }
    }
}