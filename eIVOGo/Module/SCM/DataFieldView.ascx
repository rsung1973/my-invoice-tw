<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataFieldView.ascx.cs" Inherits="eIVOGo.Module.SCM.DataFieldView" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%# showData(((GridViewRow)Container).DataItem) %>
<script runat="server">
    private object showData(object dataItem)
    {
        if (dataItem is Model.SCMDataEntity.PURCHASE_ORDER_DETAILS)
        {
            var item = ((Model.SCMDataEntity.PURCHASE_ORDER_DETAILS)dataItem).SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_ATTRIBUTE_MAPPING.Where(a => a.PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME == DataTemplateField.HeaderText).FirstOrDefault();
            return item != null ? item.PRODUCTS_ATTR_VALUE : "N/A";
        }
        else if (dataItem is Model.SCMDataEntity.WAREHOUSE_WARRANT_DETAILS)
        {
            var item = ((Model.SCMDataEntity.WAREHOUSE_WARRANT_DETAILS)dataItem).PURCHASE_ORDER_DETAILS.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_ATTRIBUTE_MAPPING.Where(a => a.PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME == DataTemplateField.HeaderText).FirstOrDefault();
            return item != null ? item.PRODUCTS_ATTR_VALUE : "N/A";
        }
        else if (dataItem is Model.SCMDataEntity.SALES_PROMOTION_PRODUCTS)
        {
            var item = ((Model.SCMDataEntity.SALES_PROMOTION_PRODUCTS)dataItem).PRODUCTS_DATA.PRODUCTS_ATTRIBUTE_MAPPING.Where(a => a.PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME == DataTemplateField.HeaderText).FirstOrDefault();
            return item != null ? item.PRODUCTS_ATTR_VALUE : "N/A";
        }
        else if (dataItem is Model.SCMDataEntity.PRODUCTS_WAREHOUSE_MAPPING)
        {
            var item = getPRODUCTS_ATTR(((Model.SCMDataEntity.PRODUCTS_WAREHOUSE_MAPPING)dataItem).PRODUCTS_SN);
            return item != null ? item.PRODUCTS_ATTR_VALUE : "N/A";
        }
        else if (dataItem is Model.SCMDataEntity.BUYER_ORDERS_DETAILS)
        {
            var item = ((Model.SCMDataEntity.BUYER_ORDERS_DETAILS)dataItem).PRODUCTS_DATA.PRODUCTS_ATTRIBUTE_MAPPING.Where(a => a.PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME == DataTemplateField.HeaderText).FirstOrDefault();
            return item != null ? item.PRODUCTS_ATTR_VALUE : "N/A";
        }
        else if (dataItem is Model.SCMDataEntity.PURCHASE_ORDER_RETURNED_DETAILS)
        {
            var item = ((Model.SCMDataEntity.PURCHASE_ORDER_RETURNED_DETAILS)dataItem).PRODUCTS_DATA.PRODUCTS_ATTRIBUTE_MAPPING.Where(a => a.PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME == DataTemplateField.HeaderText).FirstOrDefault();
            return item != null ? item.PRODUCTS_ATTR_VALUE : "N/A";
        }
        return "N/A";
    }

    private PRODUCTS_ATTRIBUTE_MAPPING getPRODUCTS_ATTR(int sn)
    {
        //PRODUCTS_ATTRIBUTE_MAPPING item = dsEntity.CreateDataManager().GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Where (w=>w.PRODUCTS_DATA.PRODUCTS_SN == sn).FirstOrDefault ();
        PRODUCTS_ATTRIBUTE_MAPPING item = dsEntity.CreateDataManager().GetTable<PRODUCTS_ATTRIBUTE_MAPPING>().Where(a => a.PRODUCTS_DATA.PRODUCTS_SN == sn && a.PRODUCTS_ATTRIBUTE_NAME.PRODUCTS_ATTR_NAME == DataTemplateField.HeaderText).FirstOrDefault();
        return item;
    }
</script>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
