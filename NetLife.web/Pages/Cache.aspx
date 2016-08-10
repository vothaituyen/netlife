<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeWeb.Master" AutoEventWireup="true" CodeBehind="Cache.aspx.cs" Inherits="NetLife.web.Pages.Cache" %>
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
    <asp:Button runat="server" Text="Giai ma" ID="Button1" OnClick="Button1_Click"/>
    <br />
    <asp:Button runat="server" Text="Remove All" ID="Button2" OnClick="Button2_Click"/>
    <br/>
    <asp:Literal runat="server" ID="ltrCache"></asp:Literal>
</asp:Content>
