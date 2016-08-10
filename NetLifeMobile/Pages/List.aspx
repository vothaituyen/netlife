<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeMobile.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="NetLifeMobile.Pages.List" %>

<%@ Register Src="../Controls/Lists/List.ascx" TagName="List" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:List ID="List1" runat="server" />
</asp:Content>
