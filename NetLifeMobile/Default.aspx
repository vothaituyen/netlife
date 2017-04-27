<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeMobile.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NetLifeMobile.Default" %>

<%@ Register Src="Controls/Home/HighlightsHomePage.ascx" TagName="HighlightsHomePage" TagPrefix="uc1" %>

<%@ Register Src="Controls/Home/News.ascx" TagName="News" TagPrefix="uc2" %>

<%@ Register Src="Controls/Home/Categorys.ascx" TagName="Categorys" TagPrefix="uc3" %>

<%@ Register Src="Controls/Advs/Ads.ascx" TagName="Ads" TagPrefix="uc4" %>



   

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <center> <uc4:Ads ID="Ads4" PositionId="40" runat="server" /></center>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
    <uc1:HighlightsHomePage ID="HighlightsHomePage1" runat="server" />
    <div class="row">
       <center>  <uc4:Ads ID="Ads3" PositionId="30" runat="server" /></center>
    </div>
    <uc2:News ID="News1" runat="server" />
    <div class="row">
        <center> <uc4:Ads ID="Ads2" PositionId="31" runat="server" /></center>
    </div>
    <div class="row nb-cat">
        <asp:Panel runat="server" ID="pnControl"></asp:Panel>
        <div class="row">
          <center> <uc4:Ads ID="Ads1" PositionId="32" runat="server" /></center>
        </div>
        
    </div>
</asp:Content>
