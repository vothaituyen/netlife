<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeWeb.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NetLife.web.Default" %>

<%@ Register Src="Controls/Home/HighlightsHomePage.ascx" TagName="HighlightsHomePage" TagPrefix="uc1" %>
<%@ Register Src="Controls/Home/StarCategory.ascx" TagName="StarCategory" TagPrefix="uc2" %>
<%@ Register Src="Controls/Home/CategoryHomePage.ascx" TagName="CategoryHomePage" TagPrefix="uc3" %>
<%@ Register Src="Controls/Advs/Adv.ascx" TagName="Adv" TagPrefix="uc4" %>
<%@ Register Src="Controls/Home/VideoClip.ascx" TagName="VideoClip" TagPrefix="uc6" %>
<%@ Register Src="Controls/Lists/Hot.ascx" TagName="Hot" TagPrefix="uc7" %>
<%--<%@ Register Src="Controls/Lists/HotRight.ascx" TagName="HotRight" TagPrefix="uc8" %>--%>
<%--<%@ Register Src="Controls/Home/Hot.ascx" TagName="Hot" TagPrefix="uc5" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-7-custom">
            <div class="col-left">
                <uc1:HighlightsHomePage ID="HighlightsHomePage1" Top="6" runat="server" />

            </div>
            <div class="row content">
                <!--Sao:54-->
                <uc2:StarCategory ID="StarCategory1" Top="9" Cat_ID="54" runat="server" />
                <!--Yêu:82-->
                <%-- <uc3:CategoryHomePage ID="CategoryHomePage3" Top="3" Cat_ID="82" runat="server" />--%>
                <!--Xem:67-->
                <%--  <uc3:CategoryHomePage ID="CategoryHomePage2" Cat_ID="67" runat="server" />--%>
                <!--Nghe:68-->
                <%--  <uc3:CategoryHomePage ID="CategoryHomePage1" Cat_ID="68" runat="server" />--%>

                <!-- Gioi tre-->
                <uc2:StarCategory ID="CategoryHomePage1" Top="3" Cat_ID="50" runat="server" />
                <!-- Giai tri-->
                <uc2:StarCategory ID="CategoryHomePage2" Top="3" Cat_ID="47" runat="server" />

                <uc2:StarCategory ID="CategoryHomePage3" Top="3" Cat_ID="129" runat="server" />

                <uc2:StarCategory ID="CategoryHomePage4" Top="3" Cat_ID="132" runat="server" />

                <uc2:StarCategory ID="CategoryHomePage5" Top="3" Cat_ID="138" runat="server" />


                <script> $(document).ready(function () {
     var colright = $("#zone82").height() + $("#zone67").height() + $("#zone68").height() + 40;
     var colleft = $("#zone54").height();
     if (colleft > colright) { $("#zone68").height((colleft - $("#zone82").height() - $("#zone67").height() - 44)); } else { $("#zone54 .box-border").height(colright + 4); }
 });</script>
            </div>
            <!--Bóng:77-->
            <div class="row">
                <!-- Choi-->
               <%-- <uc2:StarCategory ID="CategoryHomePage3" Top="3" Cat_ID="129" runat="server" />--%>
                <!-- Suc khoe-->
               <%-- <uc2:StarCategory ID="StarCategory2" Cat_ID="132" runat="server" />--%>
            </div>
            <div class="row">
                <!--Gia dinh-->
               <%-- <uc2:StarCategory ID="StarCategory3" Cat_ID="136" runat="server" />--%>
                <%--Xa hoi--%>
                <%--<uc2:StarCategory ID="StarCategory4" Cat_ID="47" runat="server" />--%>

                <%--<uc2:StarCategory ID="StarCategory5" Cat_ID="83" runat="server" />--%>
                <script>
                    $(document).ready(function () {
                        equalHeight($("#zone50, #zone83"));
                        equalHeight($("#zone77, #zone47"));
                    });
                </script>
            </div>
        </div>
        <div class="col-md-3-custom col-right">
            <uc4:Adv ID="Adv2" PositionId="11" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv3" PositionId="12" ClassName="row padbt10 fl" runat="server" />
            <%--<uc5:Hot ID="Hot1" runat="server" />
            <uc5:Hot ID="Hot2" runat="server" />
            <uc5:Hot ID="Hot3" runat="server" />--%>
            <uc4:Adv ID="Adv1" PositionId="9" ClassName="row padbt10 fl" runat="server" />
            <uc7:Hot ID="Hot1" runat="server" />
            <%--<uc8:HotRight ID="Hot2" runat="server" />--%>

            
            <uc4:Adv ID="Adv6" PositionId="10" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv4" PositionId="18" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv5" PositionId="19" ClassName="row padbt10 fl" runat="server" />
            <uc6:VideoClip ID="VideoClip1" Cat_ID="142" runat="server" />
            <uc4:Adv ID="Adv7" PositionId="20" ClassName="row padbt10 fl" runat="server" />
            <uc4:Adv ID="Adv8" PositionId="21" ClassName="row padbt10 fl" runat="server" />
            <%--<div id="vmcadsstick2" class="fl" style="width: 300px">
                <div id="vmcitem2">
                    <div class="row padbt10 fl" id="zone6" style="height: 600px">
                    </div>
                </div>
                <script type="text/javascript">
                    $(document).ready(function () {
                        vmcLoadScript("#zone6", "http://e.eclick.vn/delivery/zone/2746.js");
                        $(window).VADSSticky("vmcadsstick2", "zonefooter", "vmcitem2");
                    });
                </script>
            </div>--%>
        </div>
    </div>
</asp:Content>
