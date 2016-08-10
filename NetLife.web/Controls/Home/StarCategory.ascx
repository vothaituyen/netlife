<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StarCategory.ascx.cs" Inherits="NetLife.web.Controls.Home.StarCategory" %>
<%--<div class="col-md-6 catitem" id="zone54">
    <div class="box-border">
    <asp:Literal ID="ltrCatName" runat="server"></asp:Literal>
    <div class="col-md-12 noibatmuc">
        <asp:Literal ID="ltrNotBat" runat="server"></asp:Literal>
    </div>
    <ul class="col-md-12 item-cat">
        <asp:Literal ID="lrtListNew" runat="server"></asp:Literal>
    </ul>
        </div>
</div>--%>

<% if (Cat_ID == 54){%> <!-- sao-->
  <div class="row">
    <div class="col-md-6 catitem" id="sao">
        <div class="box-border">
        <asp:Literal ID="ltrCatName" runat="server"></asp:Literal>
        <div class="col-md-12 noibatmuc">
            <asp:Literal ID="ltrNotBat" runat="server"></asp:Literal>
        </div>
        <ul class="col-md-12 item-cat">
            <asp:Literal ID="lrtListNew" runat="server"></asp:Literal>
        </ul>
            </div>
    </div>
<% } else if(Cat_ID == 67){%>
    <div class="col-md-6 catitem" id="giaitri_thtrang">
        <!--Thoi trang-->
        <div class="row">
            <div class="col-md-12">
                <div class="box-border">
                    <asp:Literal ID="ltrCatName_thoitrang" runat="server"></asp:Literal>
                    <div class="col-md-12 noibatmuc">
                        <asp:Literal ID="ltrNotBat_thoitrang" runat="server"></asp:Literal>
                    </div>
                    <ul class="col-md-12 item-cat">
                        <asp:Literal ID="lrtListNew_thoitrang" runat="server"></asp:Literal>
                    </ul>
                </div>
            </div>    
       <%} else if (Cat_ID == 68){%> <%--giai tri--%>
            <div class="col-md-12">
                <div class="box-border">
                    <asp:Literal ID="ltrCatName_giaitri" runat="server"></asp:Literal>
                    <div class="col-md-12 noibatmuc">
                        <asp:Literal ID="ltrNotBat_giaitri" runat="server"></asp:Literal>
                    </div>
                    <ul class="col-md-12 item-cat">
                        <asp:Literal ID="lrtListNew_giaitri" runat="server"></asp:Literal>
                    </ul>
             </div>
          </div>
      </div>
    </div>
</div>
<% } else {%>
    <div class="col-md-6 catitem" id="other<% = Cat_ID %>">
        <div class="box-border">
        <asp:Literal ID="ltrCatName_other" runat="server"></asp:Literal>
        <div class="col-md-12 noibatmuc">
            <asp:Literal ID="ltrNotBat_other" runat="server"></asp:Literal>
        </div>
        <ul class="col-md-12 item-cat">
            <asp:Literal ID="lrtListNew_other" runat="server"></asp:Literal>
        </ul>
            </div>
    </div>
<% } %>
