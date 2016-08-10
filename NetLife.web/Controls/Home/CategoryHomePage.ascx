<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryHomePage.ascx.cs" Inherits="NetLife.web.Controls.Home.CategoryHomePage" %>
<div class="col-md-6 catitem groupheight" >
    <div class="box-border"  id="zone<% = Cat_ID %>">
        <asp:Literal ID="ltrCatName" runat="server"></asp:Literal>
        <div class="col-md-12 noibatmuc nb">
            <asp:Literal ID="ltrNotBat" runat="server"></asp:Literal>
        </div>
        <ul class="col-md-12 item-cat">
            <asp:Literal ID="lrtListNew" runat="server"></asp:Literal>
        </ul>
    </div>
</div>
