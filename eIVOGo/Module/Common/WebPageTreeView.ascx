<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.Common.WebPageTreeView" %>
            <asp:TreeView ID="pageTree" runat="server" CssClass="inputText" ExpandDepth="1" ImageSet="XPFileExplorer" DataSourceID="dsPageTree" ShowLines="True">
                <DataBindings>
                    <asp:TreeNodeBinding DataMember="web" />
                    <asp:TreeNodeBinding DataMember="directory" TextField="name"  />
                    <asp:TreeNodeBinding DataMember="file" TextField="name"  />
                </DataBindings>
            </asp:TreeView>
<asp:XmlDataSource ID="dsPageTree" runat="server" DataFile="~/WebPageTree.xml"></asp:XmlDataSource>
