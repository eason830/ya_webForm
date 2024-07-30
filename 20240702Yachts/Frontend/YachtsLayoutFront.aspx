<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteYachts.Master" AutoEventWireup="true" CodeBehind="YachtsLayoutFront.aspx.cs" Inherits="_20240702Yachts.Frontend.YachtsLayoutFront" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box6">
        <p>Layout & deck plan</p>

        <ul>
            <asp:Literal ID="ContentHtml" runat="server"></asp:Literal>

        </ul>
    </div>

    <div class="clear"></div>
    
     <p class="topbuttom">
       <img src="./html/images/top.gif" alt="top" />
     </p>


</asp:Content>
