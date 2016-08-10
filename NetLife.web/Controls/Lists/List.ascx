<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="List.ascx.cs" Inherits="NetLife.web.Controls.Lists.List" %>
<%@ Register Src="Paging.ascx" TagName="Paging" TagPrefix="uc1" %>
<div class="row" style="padding-right: 20px; padding-top: 20px">
    <asp:Literal ID="LtrItem" runat="server"></asp:Literal>
</div>
<uc1:Paging ID="Paging1" ActiveCss="activepage" runat="server" />


