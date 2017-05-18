<%@ Page Title="" Language="C#" MasterPageFile="~/NetLifeWeb.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="NetLife.web.Pages.List" %>

<%@ Import Namespace="ATVCommon" %>

<%@ Register Src="../Controls/Lists/HighlightsList.ascx" TagName="HighlightsList" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Advs/Adv.ascx" TagName="Adv" TagPrefix="uc2" %>
<%@ Register Src="../Controls/Lists/List.ascx" TagName="List" TagPrefix="uc3" %>
<%@ Register Src="../Controls/Home/VideoClip.ascx" TagName="VideoClip" TagPrefix="uc5" %>
<%@ Register Src="../Controls/Home/Blog.ascx" TagName="Blog" TagPrefix="uc6" %>
<%--<%@ Register Src="../Controls/Lists/Hot.ascx" TagName="Hot" TagPrefix="uc4" %>--%>
<%@ Register Src="../Controls/Home/DateTime.ascx" TagName="DateTime" TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row chuyen-muc-cat">
        <ul>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </ul>
    </div>
    <div class="row">
        <div class="col-md-7-custom">
            <div class="col-left">
                <uc1:HighlightsList ID="HighlightsList1" runat="server" />
            </div>
            <uc3:List ID="List1" runat="server" />
        </div>
        <div class="col-md-3-custom col-right">
            <uc2:Adv ID="Adv2" PositionId="11" ClassName="row padbt10 fl" runat="server" />
            <uc2:Adv ID="Adv4" PositionId="12" ClassName="row padbt10 fl" runat="server" />
            <%--<uc4:Hot ID="Hot1" runat="server" />--%>
            <%--<uc5:VideoClip ID="VideoClip1" Cat_ID="134" runat="server" />--%>
            <uc2:Adv ID="Adv1" PositionId="9" ClassName="row padbt10 fl" runat="server" />
            <uc2:Adv ID="Adv3" PositionId="10" ClassName="row padbt10 fl" runat="server" />
            <uc2:Adv ID="Adv5" PositionId="18" ClassName="row padbt10 fl" runat="server" />
            <uc2:Adv ID="Adv6" PositionId="19" ClassName="row padbt10 fl" runat="server" />
            <uc2:Adv ID="Adv7" PositionId="20" ClassName="row padbt10 fl" runat="server" />
            <uc2:Adv ID="Adv8" PositionId="21" ClassName="row padbt10 fl" runat="server" />
            
            
             
        </div>
    </div>


    <script type="text/javascript">
        showCurrent();
        function showCurrent() {
            $("#li<% = Lib.QueryString.CategoryID %>").addClass("activesmn");
        }
    </script>

</asp:Content>
