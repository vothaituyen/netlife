<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HighlightsHomePage.ascx.cs" Inherits="NetLife.web.Controls.Home.HighlightsHomePage" %>
<div class="row noibat">
    <div class="col-md-8">
        <asp:Literal ID="ltrnb" runat="server"></asp:Literal>
    </div>
    <div class="col-md-4 ulnb">
        <h3>TIN HOT
        </h3>
        <ul style="list-style-type:none;">
             <asp:Literal ID="ltrNews" runat="server"></asp:Literal>
        </ul>
    </div>
</div>
<div class="row bonbainb">
    <asp:Literal ID="ltrItem" runat="server"></asp:Literal>
</div>
