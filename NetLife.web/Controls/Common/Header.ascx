<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="NetLife.web.Controls.Common.Header" %>
<%@ Register Src="Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="SlideHeader.ascx" TagName="SlideHeader" TagPrefix="uc2" %>
<%@ Register Src="../Advs/Adv.ascx" TagName="Adv" TagPrefix="uc3" %>
<div class="header">
    <div class="container" style="height: 100px">
        <div class="col-md-3" style="padding-top: 0px">
            <a href="/">
                <div style="padding-left: 20px;"><img src="/Images/logo.png" /></div>
            </a>
        </div>
        <div class="col-md-9 pull-right">
            <div style="padding: 5px 0; float: right">
                <uc3:Adv ID="Adv1" PositionId="1" ClassName="" runat="server" />
            </div>

        </div>
    </div>
    <uc1:Menu ID="Menu1" runat="server" />
    <uc2:SlideHeader ID="SlideHeader1" runat="server" />
    <uc3:Adv ID="Adv2" PositionId="2" ClassName="" runat="server" />
</div>


