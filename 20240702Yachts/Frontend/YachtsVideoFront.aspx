<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteYachts.Master" AutoEventWireup="true" CodeBehind="YachtsVideoFront.aspx.cs" Inherits="_20240702Yachts.Frontend.YachtsVideoFront" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="box6">
        <p>Video</p>

        <iframe  width="713" id="video" height="401" runat="server" src="" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy="strict-origin-when-cross-origin" allowfullscreen></iframe>

    </div>

    <p class="topbuttom">
        <img src="./html/images/top.gif" alt="top" />
    </p>


</asp:Content>
