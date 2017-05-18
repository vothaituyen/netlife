<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="NetLife.web.Controls.Common.Footer" %>
<%@ Register src="../Advs/Adv.ascx" tagname="Adv" tagprefix="uc1" %>

        <div id="gotop" class="fa fa-angle-up" title="Trượt lên trên">
            <a href="#">
                <%--<img src="/Images/ic-backtop.jpg" />--%>
            </a>
        </div>
    <script>
        $('#gotop').click(function(){
            jQuery('html, body').animate({
                scrollTop:'0px'
            },800)});
    </script>

<div class="row padbt10">
    <div id="zonefooter" style="width: 980px; margin:0 auto">
        <uc1:Adv ID="Adv1" PositionId="17" runat="server" />
    </div>
</div>
<div class="row footer">
    <div class="container">
        <ul class="col-md-12 nav nav-tabs nav-footer">
            <li id="ic-home"><a href="/">
                <div style="margin-left: 20px;"><img src="/Images/ic-homegray.jpg" /></div></a></li>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </ul>

    </div>
    <div class="container" style="padding-top: 7px;">

<%--            <span style="font-size: 16px; padding-bottom: 10px;" class="row">Trang thông tin điện tử tổng hợp Netlife</span>
            <span class="row">Số giấy phép: 86/GP - STTTT, cấp ngày 02/11/2011 </span><span class="row">Sale and Marketing: Vision Asia Corp. </span>
            <span class="row">Địa chỉ: Lầu 10, Tòa nhà SFC, số 9 Đinh Tiên Hoàng, P. Đa Kao, Quận 1, TPHCM</span>--%>
             <div class="col-md-3 infopage" style="padding-bottom: 37px;">
                <center><img src="/Images/logo-footer.jpg" /></center>
            </div>
            <div class="col-md-9 infopage">
                <span><img style="margin-right:6px;" src="/Images/email.jpg" />Sale@visionasia.net </span>
                <span><img src="/Images/phone.jpg" />+84-8 6683-8411</span>
                <span style="font-size: 16px; padding-bottom: 10px;" class="row">Trang thông tin điện tử tổng hợp Netlife</span>
                <span class="row">Số giấy phép: 51/GP - STTTT, cấp ngày 16/11/2016 </span><span class="row">Sale and Marketing: Vision Asia Corp. </span>
                <span class="row">Địa chỉ: Tòa Nhà Khải Hoàn, 83B Hoàng Sa, Phường Đa Kao, Quận 1, TP.TPHCM</span>
             </div>

    </div>
</div>


