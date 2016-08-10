<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/NetLifeWeb.Master" Inherits="NetLife.Web.Rss.Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="rssbody content">
        <div class="rsssubbody">
            <div class="rss_text">
                Sử dụng RSS của NetLife.vn sẽ giúp bạn luôn có được những thông tin mới nhất,
                nóng nhất trong và ngoài nước!
            </div>
            <div class="rss_text_bold">
                Thêm một kênh RSS của NetLife.vn vào trang My Yahoo!
            </div>
            <div class="rss_text">
                <p>
                    - Nhấn vào nút "Add to My Yahoo!" cùng dòng với mục bạn muốn trong bảng danh mục
                    RSS của NetLife.vn
                </p>
                <p>
                    - Làm theo các chỉ dẫn để thêm mục RSS tương ứng của NetLife.vn vào trang
                    My Yahoo của bạn.
                </p>
            </div>
            <div class="rss_text_bold">
                Sử dụng chương trình đọc RSS (RSS Reader)
            </div>
            <div class="rss_text">
                <p>
                    - Chép (copy) đường dẫn (URL) tương ứng với kênh RSS mà bạn ưa thích
                </p>
                <p>
                    - Dán (paste) URL vào chương trình đọc RSS
                </p>
            </div>
            <div style="padding-top: 30px; padding-left: width:940px; float: left; margin: 0 auto;">
                <table width="680px" class="rss_table_header" cellspacing="14" cellpadding="4" border="0">
                    <tr>
                        <th>Tiêu đề</th>
                        <th>Chép URL sau vào chương trình đọc RSS</th>
                        <th></th>
                    </tr>
                </table>
                <table width="680px" class="rss_table_body" cellspacing="14" cellpadding="4" border="0">
                    <asp:Repeater ID="rptCategory" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("Cat_Name") %></td>
                                <td>
                                    <a href="/rss/<%# BOATV.Utils.UnicodeToKoDauAndGach(Eval("Cat_Name").ToString()) %>-<%#Eval("Cat_Id") %>.rss"><%# Eval("Cat_Name").ToString() %></a></td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
            </div>
            <div style="width: 940px; float: left;">
                <p class="rss_text_bold">
                    RSS là gì?
                </p>
                <p class="rss_text">
                    RSS (Really Simple Syndication) là định dạng dữ liệu dựa theo chuẩn XML được sử
                dụng để chia sẻ và phát tán nội dung Web. Việc sử dụng các chương trình đọc tin
                (News Reader, RSS Reader hay RSS Feeds) sẽ giúp bạn luôn xem được nhanh chóng tin
                tức mới nhất từ NetLife.vn. Mỗi tin dưới dạng RSS sẽ gồm : Tiêu đề, tóm tắt nội
                dung và đường dẫn nối đến trang Web chứa nội dung đầy đủ của tin.
                </p>
                <p class="rss_text_bold">
                    Chương trình đọc RSS là gì?
                </p>
                <p class="rss_text">
                    Rss Reader là phần mềm có chức năng tự động lấy tin tức đã được cấu trúc theo định
                dạng RSS. Một số phần mềm đọc RSS cho phép bạn lập lịch cập nhật tin tức. Với chương
                trình đọc RSS, bạn có thể nhấn chuột vào tiêu đề của 1 tin để đọc phần tóm tắt của
                hoặc mở ra nội dung của toàn bộ tin trong một cửa sổ trình duyệt mặc định.
                </p>
                <p class="rss_text">
                    Có rất nhiều phần mềm phục vụ khai thác tin qua định dạng RSS, bạn có thể tham khảo
                bảng các chương trình đọc RSS bên cạnh và lựa chọn cái bạn thích nhất.
                </p>
                <p class="rss_text">
                    Nếu đang sử dụng FireFox, bạn có thể tải chương trình Wizz RSS từ địa chỉ https://addons.mozilla.org/firefox/424/
                </p>
                <p class="rss_text_bold">
                    Điều kiện sử dụng
                </p>
                <p class="rss_text">
                    NetLife.vn không chịu trách nhiệm về các nội dung của các trang khác ngoài NetLife.vn
                được dẫn trong trang này. Khi sử dụng lại các tin lấy từ NetLife.vn, bạn phải ghi
                rõ nguồn tin là "Theo NetLife.vn" hoặc "NetLife.vn".
                </p>
            </div>
        </div>
    </div>
</asp:Content>
