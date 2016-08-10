<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SlideHeader.ascx.cs" Inherits="NetLife.web.Controls.Common.SlideHeader" %>
<div class="slides" style="display: none">
    <div class="container">
        <div class="slider1">
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
    </div>
</div>
<script> $(document).ready(function() {
    $(".slides").show();
});</script>