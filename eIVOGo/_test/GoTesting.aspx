<%@ Page Language="C#" AutoEventWireup="true"  %>
<%
    (new Business.Workflow.LoginController()).NormalLogin("ifsadmin");
    Response.Redirect("~/TestAll/Index");
         %>