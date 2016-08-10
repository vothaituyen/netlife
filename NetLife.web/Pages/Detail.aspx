<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeWeb.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="NetLife.web.Pages.Detail" %>
<%@ Register Src="../Controls/Details/Detail.ascx" TagName="Detail" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Details/Comment.ascx" TagName="Comment" TagPrefix="uc3" %>
<%@ Register Src="../Controls/Advs/Adv.ascx" TagName="Adv" TagPrefix="uc4" %>
<%@ Register Src="../Controls/Lists/Hot.ascx" TagName="Hot" TagPrefix="uc5" %>
<%@ Register Src="../Controls/Home/VideoClip.ascx" TagName="VideoClip" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-7-custom-main">
            <div class="row" style="padding-right: 20px;">
                <uc1:Detail ID="Detail1" runat="server" />
                <uc3:Comment ID="Comment1" runat="server" />
            </div>
        </div>
        <div class="col-md-3-custom-main">
            <uc4:Adv ID="Adv3" PositionId="11"  ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv4" PositionId="12" ClassName="row padbt10 fl" runat="server" />
             <uc5:Hot ID="Hot1" runat="server" />
            <uc6:VideoClip ID="VideoClip1" Cat_ID="78" runat="server" />
            
            <uc4:Adv ID="Adv1" PositionId="9" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv2" PositionId="10" ClassName="row padbt10 fl" runat="server" />
           
            <uc4:Adv ID="Adv5" PositionId="18" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv6" PositionId="19" ClassName="row padbt10 fl" runat="server" />
             <uc4:Adv ID="Adv7" PositionId="20" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv8" PositionId="21" ClassName="row padbt10 fl" runat="server" />
             
        </div>
    </div>
</asp:Content>
