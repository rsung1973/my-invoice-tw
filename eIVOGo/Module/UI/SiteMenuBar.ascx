<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.SiteAction.SiteMenuBar" %>
<script runat="server">

    protected void mainMenu_MenuItemClick(object sender, MenuEventArgs e)
    {

    }
</script>
<asp:Menu ID="mainMenu" runat="server" StaticDisplayLevels="3" 
    DataSourceID="dsHandler" 
    EnableViewState="False"
    OnMenuItemDataBound="mainMenu_MenuItemDataBound" 
    OnPreRender="mainMenu_PreRender"  
    StaticPopOutImageTextFormatString="展開 {0}" 
    onmenuitemclick="mainMenu_MenuItemClick">
    <DataBindings>
       <asp:MenuItemBinding DataMember="menuItem" NavigateUrlField="url" TextField="value" />
    </DataBindings>
<%-- 
    <DynamicMenuStyle CssClass="submenu" />
  
    <StaticMenuStyle  CssClass= "main-menu" />
--%>
</asp:Menu>
<asp:XmlDataSource ID="dsHandler" runat="server" EnableViewState="False" TransformFile="~/resource/MenuTransformer.xsl"
    DataFile="~/resource/SysadminMenu.xml" EnableCaching="False"></asp:XmlDataSource>
