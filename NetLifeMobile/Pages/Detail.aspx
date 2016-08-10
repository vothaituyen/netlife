<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeMobile.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="NetLifeMobile.Pages.Detail" %>

<%@ Register Src="../Controls/Details/Detail.ascx" TagName="Detail" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:Detail ID="Detail1" runat="server" />
</asp:Content>
