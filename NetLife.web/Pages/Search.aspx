<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeWeb.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="NetLife.web.Pages.Search" %>

<%@ Register Src="../Controls/Lists/HighlightsList.ascx" TagName="HighlightsList" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Advs/Adv.ascx" TagName="Adv" TagPrefix="uc2" %>
<%@ Register Src="../Controls/Lists/List.ascx" TagName="List" TagPrefix="uc3" %>
<%@ Register Src="../Controls/Home/VideoClip.ascx" TagName="VideoClip" TagPrefix="uc5" %>
<%@ Register Src="../Controls/Home/Blog.ascx" TagName="Blog" TagPrefix="uc6" %>
<%@ Register Src="../Controls/Lists/Hot.ascx" TagName="Hot" TagPrefix="uc4" %>
<%@ Register Src="../Controls/Lists/Paging.ascx" TagName="Paging" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-7-custom-main">
            <div class="row ltrText">
                <asp:Literal ID="ltrAlert" runat="server"></asp:Literal>
            </div>
            <div class="row" style="padding-right: 20px;">
                <asp:Repeater ID="rptData" runat="server">
                    <ItemTemplate>
                        <div class="row item-list">
                            <div class="img-list"><a title="<%#Eval("NEWS_TITLE") %>" href="<%#Eval("NEWS_Url") %>"><%#Eval("News_Image") %></a></div>
                            <div class="info-list">
                                <h5><%# Convert.ToDateTime(Eval("NEWS_PUBLISHDATE")).ToString("dd-MM-yyyy HH:mm") %></h5>
                                <h3><a title="<%#Eval("NEWS_TITLE") %>" href="<%#Eval("NEWS_Url") %>"><%#Eval("NEWS_TITLE") %></a></h3>
                                <p><%#Eval("NEWS_INITCONTENT") %></p>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <uc7:Paging ID="Paging1" ActiveCss="activepage" runat="server" />
        </div>
        <div class="col-md-3-custom-main">
            <uc4:Hot ID="Hot1" runat="server" />
            
            <%--<uc5:VideoClip ID="VideoClip1" Cat_ID="134" runat="server" />--%>
        </div>
    </div>
</asp:Content>
