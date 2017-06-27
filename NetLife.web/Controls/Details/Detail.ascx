<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Detail.ascx.cs" Inherits="NetLife.web.Controls.Details.Detail" %>
<%@ Register Src="../Advs/Adv.ascx" TagName="Adv" TagPrefix="uc1" %>
    <div class="row chuyen-muc-cat">
        <ul>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </ul>
    </div>
    <div class="row breadcrumbdt">
        <a href="/">
            <img style="padding-bottom: 3px" src="/Images/home-black.jpg" /></a>
        <span>
            <img src="/Images/next.jpg" />
        </span>
        <asp:Literal runat="server" ID="ltrCattxt"></asp:Literal>
        <asp:Literal runat="server" ID="ltrCatParent"></asp:Literal>
    </div>
 
<div class="row detailinfo">
    <h1>
        <asp:Literal runat="server" ID="ltrTitle"></asp:Literal></h1>

   
    <div class="row detailcontent">
        <div class="row">
            <div style="width: 300px; float: left">
                
                <div class="row initcontent">
                    <asp:Literal runat="server" ID="ltrDes"></asp:Literal>
                </div>
                 
                <div class="row">
                    <ul style="padding: 0 0 0 10px">
                        <asp:Literal runat="server" ID="ltrRelate"></asp:Literal>
                    </ul>
                </div>
            </div>
            <div style="float: right; width: 300px; padding-bottom: 10px;padding-left: 10px">
                <uc1:Adv ID="Adv2" PositionId="29" runat="server" />
            </div>
        </div>
      
        <div class="netbody" id="abdplayer">
            <asp:Literal runat="server" ID="ltrContent"></asp:Literal>
        </div>
    </div>


    <!-- -->

</div>
<div class="row detailinfo" >
    <div class="row infoshare">
        <div class="col-md-10">
            <span class="nvb" style="display: none">
                <asp:Literal runat="server" ID="ltrAthor" Visible="False"></asp:Literal></span><span class="gio"><asp:Literal runat="server" ID="ltrTime"></asp:Literal></span><span class="ngay">
                    <asp:Literal runat="server" ID="ltrDate"></asp:Literal></span><span class="thumuc"><asp:Literal runat="server" ID="ltrCatName"></asp:Literal></span>
                        <div class="row likefb">
                            <iframe scrolling="no" frameborder="0" src="//www.facebook.com/plugins/like.php?href=http://netlife.vn/<% = Request.RawUrl.ToString() %>&amp;width&amp;layout=button_count&amp;action=like&amp;show_faces=true&amp;share=true&amp;height=20&amp;appId=1526016807656820&amp;width=480px"
                                style="border: medium none; overflow: hidden; height: 20px; margin-top: 0px; width: 100%" allowtransparency="true"></iframe>
                        </div>
           
        </div>
    </div>
    </div>

<div class="row">
    <div style="float: left; width: 50%; padding-top: 10px">
        <uc1:Adv ID="Adv1" PositionId="28" runat="server" />
    </div>
    <div style="float: right;width: 50%;padding-top: 10px">
        <uc1:Adv ID="Adv211" PositionId="35" runat="server" />
    </div>
</div>


<div id="tags" runat="server" class="row tags">
    <p class="col-md-1">TAGS</p>
    <div class="col-md-11" style="height: 23px; overflow: hidden">
        <asp:Literal runat="server" ID="ltrKeyword"></asp:Literal>
    </div>
</div>


<div class="row tinlq" id="lienquan" runat="server">
    <h3>Tin mới nhất
    </h3>
    <ul class="relatenews row">
        <asp:Literal runat="server" ID="LiteralNews1"></asp:Literal>
    </ul>
    <ul id="listnews" class="relatenews" runat="server" visible="false">
        <asp:Literal runat="server" ID="LiteralNews2"></asp:Literal>
    </ul>
</div>
<div class="relatenews row" id="Div2" runat="server">
  <uc1:Adv ID="Adv4" PositionId="44" runat="server" />
    </div>
<div class="row tinlq" id="Div1" runat="server">
    <h3>Tin liên quan
    </h3>
    <ul class="relatenews row">
        <asp:Literal runat="server" ID="ltrListRelate"></asp:Literal>
    </ul>
    <ul id="relatenews" class="relatenews" runat="server" visible="false">
        <asp:Literal runat="server" ID="ltrListRelate2"></asp:Literal>
    </ul>
</div>

<%--<div class="row tinlq">
    <h3>Tin cùng chuyên mục</h3>
    <ul>
        <asp:Literal runat="server" ID="ltrOther"></asp:Literal>
    </ul>
</div>--%>
<div class="relatenews row" id="Div3" runat="server">
  <uc1:Adv ID="Adv5" PositionId="45" runat="server" />
    </div>
<div class="row tinlq" id="cungchuyenmuc" runat="server">
    <h3>Tin cùng chuyên mục</h3>
    <ul class="relatenews row">
        <asp:Literal runat="server" ID="LiteralOther1"></asp:Literal>
    </ul>
    <ul id="othernews" class="relatenews" runat="server" visible="false">
        <asp:Literal runat="server" ID="LiteralOther2"></asp:Literal>
    </ul>
</div>
<div class="relatenews row" id="Div4" runat="server">
  <uc1:Adv ID="Adv3" PositionId="46" runat="server" />
    </div>
<div class="row tinlq">
	<!-- Native adsense -->
        <h3>Có thể bạn quan tâm</h3>
	<uc1:Adv ID="Adv6" PositionId="54" runat="server" />
</div>
<div class="row tinlq hide">
    <h3>Video</h3>
    <ul class="relatenews row">
        <asp:Literal runat="server" ID="ltrVideo"></asp:Literal>
    </ul>
</div>



<div id="fb-root"></div>
<script>
    $(document).ready(function () {
        $(".likefb").children().css("width", "100%");
     });
    </script>
<script>(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/vi_VN/sdk.js#xfbml=1&version=v2.3";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>
