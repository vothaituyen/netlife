<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Header.ascx.cs" Inherits="NetLifeMobile.Controls.Common.Header" %>
<div class="header">
    <div class="row header-mb">
        <ul id="accordion" class="col-xs-6 accordion">

            <li class="ahome">
                <div class="link"><a href="/">Trang Chủ </a></div>
            </li>

            <asp:Repeater ID="rptCategory" runat="server" OnItemDataBound="rptCategory_ItemDataBound">
                <ItemTemplate>
                    <li>
                        <div class="link"><i class="fa fa-chevron-down"></i><a href="<%# Eval("Href") %>"><%# Eval("Cat_Name") %></a></div>
                        <ul class="submenu">
                            <asp:Repeater ID="rptSubCategory" runat="server">
                                <ItemTemplate>
                                    <li><a href="<%# Eval("Href") %>"><%# Eval("Cat_Name") %></a></li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </li>
                </ItemTemplate>
            </asp:Repeater>

        </ul>
        <div id="addcl"></div>
        <div class="col-xs-3 menumb" id="mn">
            <a href="javascript:void(0);" onclick="showmenu(accordion);" id="menu-click" class="nav-expander">
                <span>
                    <img src="/Images/bg-menu.jpg" /></span>
            </a>

        </div>
        <div class="col-xs-6 logo-mb" id="lg">
            <a href="/">
                <img class="img-responsive" src="/Images/logo.png" />
            </a>
        </div>
        <div class="col-xs-3 searchmb">
            <a class="search-mb" id="open_popup" name="open_popup" rel="miendatwebPopup" href="#popup_content">
                <span>
                    <img src="/Images/bg-search.jpg" /></span>
            </a>

        </div>
        <div id="popup_content" class="popup col-sm-12">

            <input class="col-sm-9" style="padding: 4px 5px; border: none" type="text" name="keyword" id="keyword" onfocus="if(this.value=='Từ khoá') this.value='';" placeholder="Nhập từ tìm kiếm..." />
            <input class="col-sm-3 btnsearch" type="button" id="btnSearch" onclick="search()" value="Tìm kiếm" />
        </div>
    </div>
</div>

<script type="text/javascript" language="JavaScript">
    $(function () {
        $('a[rel*=miendatwebPopup]').showPopup({
            top: 200, //khoảng cách popup cách so với phía trên
            closeButton: ".close_popup", //khai báo nút close cho popup
            scroll: false, //cho phép scroll khi mở popup, mặc định là không cho phép
            onClose: function () {
                //sự kiện cho phép gọi sau khi đóng popup, cho phép chúng ta gọi 1 số sự kiện khi đóng popup, bạn có thể để null ở đây
            }
        });
    });


    function search() {
        var key = $('#keyword').val();
        if (key != "" && key != "Tìm kiếm")
            location.href = "/Pages/Search.aspx?key=" + $('#keyword').val();
        else alert("Bạn chưa nhập từ khóa!");
        return false;
    }

    $('#keyword').keypress(function (event) {
        if (event.which == 13) {
            event.preventDefault();
            search();
        }
    });
</script>