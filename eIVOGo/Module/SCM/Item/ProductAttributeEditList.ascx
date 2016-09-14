<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductAttributeEditList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.ProductAttributeEditList" %>
<%@ Register Src="ChooseProductsNameAttribute.ascx" TagName="ChooseProductsNameAttribute"
    TagPrefix="uc7" %>
<%@ Register Src="ProductsAttributeMappingList.ascx" TagName="ProductsAttributeMappingList"
    TagPrefix="uc8" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="border_gray">
            <!--表格 開始-->
            <h2>
                料品屬性</h2>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                    <th width="15%">
                        料品屬性名稱
                    </th>
                    <td width="35%" class="tdleft">
                        <uc7:ChooseProductsNameAttribute ID="productAttr" runat="server" />
                    </td>
                    <th width="15%" class="tdleft">
                        料品屬性內容
                    </th>
                    <td class="tdleft">
                        &nbsp;<asp:TextBox ID="attrValue" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        &nbsp;<asp:Button ID="btnAddAttri" class="btn" runat="server" Text="新增屬性及內容" OnClick="btnAddAttri_Click"
                            CausesValidation="False" />
                    </td>
                </tr>
            </table>
            <uc8:ProductsAttributeMappingList ID="productAttrList" runat="server" />
            <!--表格 結束-->
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

