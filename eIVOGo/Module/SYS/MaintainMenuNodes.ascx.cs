using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

using Business.Helper;
using Model.Security.MembershipManagement;
using Uxnet.Com.Providers;
using Uxnet.Web.Module.SiteAction;
using Utility;

namespace OpenSite.module.sam
{
    public partial class MaintainMenuNodes : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(MenuPath))
                this.dsMenu.DataFile = getStoredMenuPath();
        }

        [Bindable(true)]
        public String MenuPath
        {
            get
            {
                return ViewState["menuPath"] as String;
            }
            set
            {
                ViewState["menuPath"] = value;
                cloneMenuFile(value);
            }
        }

        private void cloneMenuFile(string menuPath)
        {
            if (!String.IsNullOrEmpty(menuPath))
            {
                String clonedFile = Path.Combine(Logger.LogDailyPath, Path.GetFileName(menuPath));
                File.Copy(Path.Combine(SiteMenuBar.MenuManager.StoredPath, menuPath), clonedFile, true);
            }
        }

        private String getStoredMenuPath()
        {
            return Path.Combine(Logger.LogDailyPath, MenuPath);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.webPageTree.PageTree.SelectedNodeChanged += new EventHandler(PageTree_SelectedNodeChanged);
        }

        void PageTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            menuUrl.Text = "~" + webPageTree.PageTree.SelectedNode.ValuePath;
        }

        protected void btnDeleteNode_Click(object sender, EventArgs e)
        {
            if (menuTree.CheckedNodes.Count > 0)
            {
                XmlDocument menuDoc = dsMenu.GetXmlDocument();
                List<XmlNode> nodes = new List<XmlNode>();

                foreach (TreeNode node in menuTree.CheckedNodes)
                {
                    XmlNode menuNode = menuDoc.SelectSingleNode(node.DataPath);
                    if (menuNode != null)
                        nodes.Add(menuNode);
                }

                foreach (XmlNode menuNode in nodes)
                    menuNode.ParentNode.RemoveChild(menuNode);

                dsMenu.Save();
                menuTree.DataBind();
            }
        }

        protected void menuTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeNode node = menuTree.SelectedNode;
            if (node != null)
            {
                XmlDocument menuDoc = dsMenu.GetXmlDocument();
                this.menuName.Text = node.Text;
                XmlNode menuNode = menuDoc.SelectSingleNode(node.DataPath);
                this.menuUrl.Text = menuNode.Attributes["url"].Value;
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            TreeNode node = menuTree.SelectedNode;
            if (node != null)
            {
                XmlDocument menuDoc = dsMenu.GetXmlDocument();
                XmlNode menuNode = menuDoc.SelectSingleNode(node.DataPath);
                menuNode.Attributes["value"].Value = menuName.Text;
                menuNode.Attributes["url"].Value = menuUrl.Text;

                dsMenu.Save();
                menuTree.DataBind();
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            TreeNode node = menuTree.SelectedNode;
            if (node == null)
            {
                node = menuTree.Nodes[0];
            }

            XmlDocument menuDoc = dsMenu.GetXmlDocument();

            XmlNode menuNode = menuDoc.SelectSingleNode(node.DataPath);
            XmlNode newNode;
            if (rbMenuItem.Checked || rbRoot.Checked)
            {
                newNode = menuDoc.CreateElement("menuItem");
            }
            else
            {
                newNode = menuDoc.CreateElement("workItem");
            }
            XmlAttribute attr = menuDoc.CreateAttribute("value");
            attr.Value = menuName.Text;
            newNode.Attributes.Append(attr);
            attr = menuDoc.CreateAttribute("url");
            attr.Value = menuUrl.Text;
            newNode.Attributes.Append(attr);
            menuNode.AppendChild(newNode);

            dsMenu.Save();
            menuTree.DataBind();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(MenuPath))
            {
                File.Copy(getStoredMenuPath(), Path.Combine(SiteMenuBar.MenuManager.StoredPath, MenuPath), true);
                SiteMenuBar.MenuManager.Reload(MenuPath);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            cloneMenuFile(MenuPath);
            menuTree.DataBind();
        }

    }
}