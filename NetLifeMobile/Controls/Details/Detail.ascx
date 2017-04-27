<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Detail.ascx.cs" Inherits="NetLifeMobile.Controls.Details.Detail" %>
<%@ Import Namespace="ATVCommon" %>
<%@ Register Src="../Advs/Ads.ascx" TagName="Ads" TagPrefix="uc1" %>
<center><uc1:Ads ID="Ads6" PositionId="40" runat="server" /></center>
<div class="row">
    <div class="col-xs-12">
        <span id="rcorners1">
            <asp:Literal runat="server" ID="ltrCatParentMenu"></asp:Literal>
         </span>
         
         <asp:Literal runat="server" ID="ltrCatParent"></asp:Literal>
             <span id="rcorners1">|
                 </span>
         <span id="rcorners1">
            <asp:Literal runat="server" ID="ltrCattxt"></asp:Literal>
                 </span>
   </div>
</div>
<div class="row content-detail">
    <h1 class="col-xs-12 tit-detail">
        <asp:Literal ID="ltrTitle" runat="server"></asp:Literal>
    </h1>
             
   
    <h3 class="col-xs-12 sub-title">
        <asp:Literal ID="ltrDes" runat="server"></asp:Literal>
    </h3>
    	<div class="row" style="padding-bottom: 10px">
        <center>   <uc1:Ads ID="Ads1" PositionId="30" runat="server" /></center>
    </div>
    <div class="row tinhot">
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltrHot"></asp:Literal>
    </ul>
    </div>



    <div id="abdplayer" class="row content-info">
        
        <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
    </div>

         <div class="col-xs-12 infoshare" style="margin-top:5px;">
        <span class="nvb" runat="server" visible="False">
            <asp:Literal ID="ltrAthor" runat="server"></asp:Literal></span><span class="gio"><asp:Literal ID="ltrTime" runat="server"></asp:Literal></span><span class="ngay"><asp:Literal ID="ltrDate" runat="server"></asp:Literal></span>
             <div class="row likefb">
                    <iframe frameborder="0" scrolling="no" allowtransparency="true" style="border: none; overflow: hidden; width: 450px; height: 20px; margin-top: 10px"
                        src="http://www.facebook.com/plugins/like.php?href=http://m.netlife.vn<% = Request.RawUrl.ToString() %>&amp;layout=button_count&amp;show_faces=true&amp;share=true&amp;width=450&amp;action=like&amp;colorscheme=light&amp;height=20"></iframe>

                </div>
    </div>
    <script>
        $(document).ready(function () {
            $(".content").find("[style]").css("width", "100%");
            $(".content").find("[width]").css("width", "100%");
			
			$(".content-info img").removeAttr("style");
			$(".content-info img").removeAttr("height");
            $(".likefb").children().css("width", "100%");
        });
      
    </script>
</div>



<div class="row tags" runat="server" id="tags">
    <p class="col-xs-2">TAGS</p>
    <div class="col-xs-10">
        <asp:Literal runat="server" ID="ltrKeyword"></asp:Literal>
    </div>
</div>
<div class="row">
    <center> <uc1:Ads ID="Ads3" PositionId="32" runat="server" /></center>
</div>

<div class="row tinlq">
    <h2>Tin mới</h2>
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltrNew"></asp:Literal>
    </ul>
</div>
<div class="row" style="padding-top: 10px">
    <center>    <uc1:Ads ID="Ads2" PositionId="33" runat="server" /></center>
</div>

<div class="row tinlq">
    <h2>Tin nổi bật</h2>
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltrOther"></asp:Literal>
    </ul>
</div>

<div class="row" style="padding-top: 10px">
    <center>    <uc1:Ads ID="Ads4" PositionId="34" runat="server" /></center>
</div>

<div class="row tinlq" runat="server" id="relate">
    <h2>Tin liên quan</h2>
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltrListRelate"></asp:Literal>
    </ul>
</div>
   <center>    <uc1:Ads ID="Ads7" PositionId="43" runat="server" /></center>

<div class="row tinlq">
    <h2>Tin cùng chuyên mục</h2>
    <ul class="col-xs-12">
        <asp:Literal runat="server" ID="ltsameCategorys"></asp:Literal>
    </ul>
</div>





   
	
	
	





<%--<script type="text/javascript">
    var _abdm = _abdm || [];
    /* INVIEW load placement for account: netlife, site: http://m.netlife.vn, size: 1x1 - mobile, zone: popup */
    _abdm.push(["1428987667", "Popup", "1428997052"]);
</script>
<script src="http://media.m.ambientplatform.vn/js/m_adnetwork.js" type="text/javascript"></script>
<noscript>
    <div style="bottom: 0; position: absolute; margin-left: auto; margin-right: auto; left: 0; right: 0;"><a href="http://click.m.ambientplatform.vn/247/admServerNs/zid_1428997052/wid_1428987667/" target="_blank">
        <img src="http://delivery.m.ambientplatform.vn/247/mnoscript/zid_1428997052/wid_1428987667/" /></a></div>
</noscript>--%>
<!--
<div class="row">
    <center> <uc1:Ads ID="Ads5" PositionId="22" runat="server" /></center>
</div>
    -->
