<%@ Page Language="C#" AutoEventWireup="true"  %>
<%@ Import Namespace="Business.Helper" %>
<%
    var pid = Request["pid"] ?? "ifsadmin";
    var userProfile = Model.Security.MembershipManagement.UserProfileFactory.CreateInstance(pid);
    Context.SignOn(userProfile);
    Response.Redirect("~/BusinessRelationship/MaintainRelationship");
         %>