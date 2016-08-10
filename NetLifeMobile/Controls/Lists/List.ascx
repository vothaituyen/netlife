<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="List.ascx.cs" Inherits="NetLifeMobile.Controls.Lists.List" %>
<%@ Register Src="Paging.ascx" TagName="Paging" TagPrefix="uc1" %>
<%@ Register src="../Advs/Ads.ascx" tagname="Ads" tagprefix="uc2" %>
<div class="row" style="padding-bottom: 10px;">

    <div class="col-xs-12 breadcrumb-mb">
        <asp:HyperLink runat="server" ID="hplNext"></asp:HyperLink>
        
    </div>

</div>
<div class="row nb-mb">
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
</div>
<div class="clearfix"></div>
<div class="row" style="padding-top: 10px">
    <uc2:Ads ID="Ads1" PositionId="30" runat="server" />
</div>
 
<div class="row list" id="tbDetails">
    <asp:Literal ID="LtrItem" runat="server"></asp:Literal>
</div>
<div class="row more">
    <uc1:Paging ID="Paging1" runat="server" />
    <%-- <a href="#">
        <div class="col-xs-12 xt">
            Xem thêm
        </div>
    </a>--%>
    <input type="button" id="btnLoad" style="display: none" value="Load More Data" />

</div>
<div class="row" >
    <uc2:Ads ID="Ads2" PositionId="31" runat="server" />
</div>
<%--<script type="text/javascript">
    $(function () {
        $('#btnLoad').click(function () {
            debugger;
            var nextval = $('#tbDetails .item-list').length - 1;
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/WebService.asmx/BindDatatable",
                //data: "{'count':'" + nextval + "'}",
                data : {count : 10, parentId : 0, catId : 101, pageIndex : 10},
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        for (var i = 0; i < data.d.length; i++) {
                            //$("#tbDetails").append("<tr><td>" + data.d[i].UserId + "</td><td>" + data.d[i].UserName + "</td><td>" + data.d[i].Education + "</td><td>" + data.d[i].Location + "</td></tr>");
                            $("#tbDetails").append("<div class=\"row item-list\"><a href=" + data.d[i].Url + "><div class=\"col-xs-12 pd\"><div class=\"col-xs-3 img-list-item\"> " + data.d[i].Img + " </div><div class=\"col-xs-9 info-list-item\"> <span>" + data.d[i].Date + "</span><p>" + data.d[i].Title + "</p> </div> </div> </a></div>")
                        }
                    }
                    else
                        alert('No More Records to Load')
                },
                error: function (result) {
                    alert("Error");
                }
            });
        })
    });
</script>--%>

<script type="text/javascript">
    $(document).ready(function () {
        $(".info-list-item p a").each(function (index) {
            if ($(this).parent().width() < 210) {
                $(this).text($(this).text().substr(0, 37) + "...");
            }
            else if ($(this).text().length > 50 && $(this).parent().width() < 350 && $(this).parent().width() >= 210) {
                $(this).text($(this).text().substr(0, 50) + "...");
            }
        });
    });
</script>


