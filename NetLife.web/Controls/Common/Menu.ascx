<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="NetLife.web.Controls.Common.Menu" %>
<%@ Import Namespace="ATVCommon" %>
<div class="menu">
    <div class="container">
        <ul class="col-md-12 nav nav-tabs">
            <li id="ichome"><a href="/">
                <img src="/Images/icon-home.png" /></a></li>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </ul>
        <div class="col-md-3">
            <div class="ip col-md-12">
                <input type="text" class="col-md-10" placeholder="Nhập nội dung tìm kiếm..." name="keyword" id="keyword"
                    style="margin-bottom: 0" onfocus="if(this.value=='Từ khoá') this.value='';"/>
                <button class="col-md-2" id="btn-search" onclick="search()">
                    <img src="/Images/ic-search.jpg" />
                </button>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">

    function search() {
        var key = $('#keyword').val();
        if (key != "" && key != "Tìm kiếm")
            location.href = "/Pages/Search?key=" + $('#keyword').val();
        else alert("Bạn chưa nhập từ khóa!");
        return false;
    }

    $('#keyword').keypress(function (event) {
        if (event.which == 13) {
            event.preventDefault();
            search();
        }
    });
    showCurrent();
    function showCurrent() {
        if (<%= Lib.QueryString.ParentCategoryID%> > 0) {
            $("#li<% = Lib.QueryString.ParentCategoryID %>").addClass("activemn");
        } else {
            $("#li<% = Lib.QueryString.CategoryID %>").addClass("activemn");
        }
    }
</script>
