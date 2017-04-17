<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StarCategory.ascx.cs" Inherits="NetLife.web.Controls.Home.StarCategory" %>
<%--<div class="col-md-6 catitem" id="zone54">
    <div class="box-border">
    <asp:Literal ID="ltrCatName" runat="server"></asp:Literal>
    <div class="col-md-12 noibatmuc">
        <asp:Literal ID="ltrNotBat" runat="server"></asp:Literal>
    </div>
    <ul class="col-md-12 item-cat">
        <asp:Literal ID="lrtListNew" runat="server"></asp:Literal>
    </ul>
        </div>
</div>--%>


    <div class="col-md-6 catitem" id="other<% = Cat_ID %>">
        <div class="box-border">
        <asp:Literal ID="ltrCatName_other" runat="server"></asp:Literal>
        <div class="col-md-12 noibatmuc">
            <asp:Literal ID="ltrNotBat_other" runat="server"></asp:Literal>
        </div>
        <ul class="col-md-12 item-cat">
            <asp:Literal ID="lrtListNew_other" runat="server"></asp:Literal>
        </ul>
            </div>
    </div>

