<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Categorys.ascx.cs" Inherits="NetLifeMobile.Controls.Home.Categorys" %>
    <div class="col-xs-12 item-cat">
        <asp:Literal ID="ltrCatName" runat="server"></asp:Literal>
            <div class="row item-content">
                 <asp:Literal ID="ltrNotBat" runat="server"></asp:Literal>
                <div class="col-xs-12">
                    <ul>
                         <asp:Literal ID="lrtListNew" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>
        </div>
<%--<script type="text/javascript">
    $(document).ready(function () {
        $(".news_home span.title").each(function (index) {
            if ($(this).width() < 210) {
                $(this).text($(this).text().substr(0, 67) + "...");
            }
        });
    });
</script>--%>