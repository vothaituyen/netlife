<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Detail.ascx.cs" Inherits="NetLifeMobile.Controls.Details.Detail" %>
<%@ Import Namespace="ATVCommon" %>
<%@ Register Src="../Advs/Ads.ascx" TagName="Ads" TagPrefix="uc1" %>
<div class="row" style="padding-bottom: 10px;">
    <div class="col-xs-12 breadcrumb-mb">
        <asp:Literal runat="server" ID="ltrCattxt"></asp:Literal>
        <asp:Literal runat="server" ID="ltrCatParent"></asp:Literal>
    </div>
</div>
<div class="row content-detail">
    <div class="row likefb">
                    <iframe frameborder="0" scrolling="no" allowtransparency="true" style="border: none; overflow: hidden; width: 450px; height: 25px; margin-top: 10px"
                        src="http://www.facebook.com/plugins/like.php?href=http://netlife.com.vn/<% = Request.RawUrl.ToString() %>&amp;layout=button_count&amp;show_faces=true&amp;share=true&amp;width=450&amp;action=like&amp;colorscheme=light&amp;height=20"></iframe>

                </div>
    <h2 class="col-xs-12 tit-detail">
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
    </h2>
             
   
    <h3 class="col-xs-12 sub-title">
        <asp:Literal ID="ltrDes" runat="server"></asp:Literal>
    </h3>

     <div class="row" style="padding-bottom: 10px">
        <center>   <uc1:Ads ID="Ads1" PositionId="30" runat="server" /></center>
    </div>
    <div id="abdplayer" class="row content-info">
        
        <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
    </div>
         <div class="col-xs-12 infoshare">
        <span class="nvb" runat="server" visible="False">
            <asp:Literal ID="ltrAthor" runat="server"></asp:Literal></span><span class="gio"><asp:Literal ID="ltrTime" runat="server"></asp:Literal></span><span class="ngay"><asp:Literal ID="ltrDate" runat="server"></asp:Literal></span>
             <div class="row likefb">
                    <iframe frameborder="0" scrolling="no" allowtransparency="true" style="border: none; overflow: hidden; width: 450px; height: 25px; margin-top: 10px"
                        src="http://www.facebook.com/plugins/like.php?href=http://netlife.com.vn/<% = Request.RawUrl.ToString() %>&amp;layout=button_count&amp;show_faces=true&amp;share=true&amp;width=450&amp;action=like&amp;colorscheme=light&amp;height=20"></iframe>

                </div>
    </div>
    <script>
        $(document).ready(function () {
            
            ////$("img").css("width", "100%");
            ////$("div").css("width", "100%");
            //$(".detail_content").css("width", "100%");
            //$(".detail_content").find("img").css("width", "100%");

            ////$(".header-mb").find("div").removeAttr("style");
            ////$(".header").find("div").removeAttr("style");
            ////$(".header").find("img").removeAttr("style");
            ////$("span").find("img").removeAttr("style");
            ////$("a").find("img").removeAttr("style");


            //$("div.VCSortableInPreviewMode").each(function (index) {
            //    $(this).width("100%");
            //    $(this).height("100%");
            //});
            
            $(".content").find("[style]").css("width", "100%");
            $(".content").find("[width]").css("width", "100%");
            $(".vmc_ads_viewport").find("img").removeAttr("style");
            $(".likefb").children().css("width", "100%");
        });
        $(".content-info img").css({ 'style': '', 'width': 'auto', 'height': 'auto' });
    </script>
</div>
<div class="row">
    <center> <uc1:Ads ID="Ads3" PositionId="32" runat="server" /></center>
</div>


<div class="row tags" runat="server" id="tags">
    <p class="col-xs-2">TAGS</p>
    <div class="col-xs-10">
        <asp:Literal runat="server" ID="ltrKeyword"></asp:Literal>
    </div>
</div>
<div class="row" style="padding-top: 10px">
    <center>    <uc1:Ads ID="Ads2" PositionId="33" runat="server" /></center>
</div>
<div class="row tinlq" runat="server" id="relate">
    <h2>Tin Liên quan</h2>
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltrListRelate"></asp:Literal>
    </ul>
</div>
<div class="row" style="padding-top: 10px">
    <center>    <uc1:Ads ID="Ads4" PositionId="34" runat="server" /></center>
</div>
<div class="row tinlq">
    <h2>Tin khác cùng chuyên mục</h2>
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltrOther"></asp:Literal>
    </ul>
</div>
<!--
<script type="text/javascript">
    var _abdm = _abdm || [];
    /* load placement for account: netlife, site: http://m.netlife.vn, size: 1x1 - mobile, zone: popup */
    _abdm.push(["1428987667", "Popup", "1428997052"]);
</script>
<script src="http://media.m.ambientplatform.vn/js/m_adnetwork.js" type="text/javascript"></script>
<noscript>
    <div style="bottom: 0; position: absolute; margin-left: auto; margin-right: auto; left: 0; right: 0;"><a href="http://click.m.ambientplatform.vn/247/admServerNs/zid_1428997052/wid_1428987667/" target="_blank">
        <img src="http://delivery.m.ambientplatform.vn/247/mnoscript/zid_1428997052/wid_1428987667/" /></a></div>
</noscript>
    -->
