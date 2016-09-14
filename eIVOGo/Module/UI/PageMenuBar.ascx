<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageMenuBar.ascx.cs"  Inherits="eIVOGo.Module.UI.PageMenuBar" %>

<%@ Register src="SiteMenuBar.ascx" tagname="TreeMenu" tagprefix="uc1" %>

<script runat="server">
  

    
</script>
<div><img runat="server" id="img1" enableviewstate="false" src="~/images/sider_title.jpg" width="215" height="65" alt=""/></div>
<div class="menu_top">
<table width="180" border="0" cellspacing="0" cellpadding="0">
    <tbody>
       
        <tr>
            <td valign="top" >
                
                <uc1:TreeMenu ID="TreeMenu1" runat="server" />
                
            </td>
        </tr>
      
    </tbody>
</table>
</div>
<div class="menu_buttom"></div>
