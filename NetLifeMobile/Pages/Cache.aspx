<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeMobile.Master" AutoEventWireup="true" CodeBehind="Cache.aspx.cs" Inherits="NetLife.web.Pages.Cache" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:TextBox runat="server" ID="txtKey"></asp:TextBox><br/>
    <br />
    <asp:Button runat="server" Text="Get" ID="btnGet" OnClick="btnGet_Click"/>
    <br />
    <asp:Button runat="server" Text="Set" ID="btnSet" OnClick="btnSet_Click"/>
    <br />
    <asp:Button runat="server" Text="Remove" ID="btnRemove" OnClick="btnRemove_Click"/>
    <br />
    <br />
    <asp:Button runat="server" Text="View Key Cache" ID="btnViewCache" OnClick="btnViewCache_Click"/>
    <br />
    <asp:Literal runat="server" ID="ltrCache"></asp:Literal>
</asp:Content>
