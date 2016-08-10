<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeMobile.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="NetLifeMobile.Pages.Search" %>

<%@ Register src="../Controls/Lists/Paging.ascx" tagname="Paging" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row" style="padding-bottom: 10px;">
            <asp:Literal ID="ltrAlert" runat="server"></asp:Literal>
    </div>
    <div class="clearfix"></div>
    <div class="row list" id="tbDetails">
        <asp:Repeater ID="rptData" runat="server">
            <ItemTemplate>
                <div class="row item-list">
                    
                        <div class="col-xs-12 pd">
                            <div class="col-xs-4 img-list-item">
                               <%#Eval("News_Image") %>
                            </div>
                            <div class="col-xs-8 info-list-item">
                                <span><%# Convert.ToDateTime(Eval("NEWS_PUBLISHDATE")).ToString("dd-MM-yyyy HH:mm") %></span>
                                <p>
                                   <a href="<%#Eval("NEWS_Url") %>"> <%#Eval("NEWS_TITLE") %></a>
                                </p>
                            </div>
                        </div>
                    
                </div>
            </ItemTemplate>
        </asp:Repeater>
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

</asp:Content>
