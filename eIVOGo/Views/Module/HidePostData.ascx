<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<% 
    for (int i = 0; i < Request.Form.Count; i++)
    {
        if (Request.Form.GetKey(i) != "__VIEWSTATE")
            Writer.WriteLine(Html.Hidden(Request.Form.GetKey(i), Request.Form.Get(i)));
    }
%>