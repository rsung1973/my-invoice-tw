<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceItemListTemplate.ascx.cs"
    Inherits="eIVOGo.Views.Module.InvoiceItemListTemplate" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<div id="fieldsContainer"></div>
<div id="jsGrid"></div>
<uc1:JsGridInitialization runat="server" id="gridInit" FieldName="fields" JsGridSelector="#jsGrid" FieldsContainerSelector="#fieldsContainer" />
