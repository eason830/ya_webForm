<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteFrontend.Master" AutoEventWireup="true" CodeBehind="NewFront2.aspx.cs" Inherits="_20240702Yachts.Frontend.NewFront2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

       <!--遮罩-->
    <div class="bannermasks">
        <img src="./html/images/newbanner.jpg" alt="&quot;&quot;" width="967" height="371" /></div>
    <!--遮罩結束-->

    <div class="banner">
        <ul>
            <li>
                <img src="./html/images/newbanner.jpg" alt="Tayana Yachts" /></li>
        </ul>

    </div>

    <div class="conbg">
        <!--------------------------------左邊選單開始---------------------------------------------------->
        <div class="left">

            <div class="left1">
                <p><span>NEWS</span></p>
                <ul>
                    <li><a href="#">News & Events</a></li>

                </ul>



            </div>




        </div>







        <!--------------------------------左邊選單結束---------------------------------------------------->

        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb"><a href="#">Home</a> >> <a href="#">News </a>>> <a href="#"><span class="on1">News & Events</span></a></div>
        <div class="right">
            <div class="right1">
                <div class="title"><span>News & Events</span></div>

                <!--------------------------------內容開始---------------------------------------------------->
                <div class="box3">
                    <h4 ID="newsTitle" runat="server"></h4>
                    
                    <%--<asp:TextBox ID="newsContent" runat="server"></asp:TextBox>--%>

                    <%--<p><img src="images/pit009.jpg" alt="&quot;&quot;" /></p>--%>
                    <%--On Display at the Seattle Boats Afloat Show, Lake Union, January 25 - February 3, 2007
The luxury Tayana 48 Pilothouse is the perfect boat for the Northwest Cruiser or, for that matter, anywhere in the world. Designed for minimal exterior maintenance, and equipped for maximun creature comforts and ease of sailing. Enjoy aft cockpit sailing with comfortable seating for nice weather cruising or duck inside and sail from the inside steering station when it is gnarly out. Large salon windows with a pair of overhead hatches give the skipper plenty of visibility to see all around as well as up to check sail shape and trim.--%>
                </div>

                <asp:Literal ID="newsContent" runat="server"></asp:Literal>


                <asp:Literal ID="groupImg" runat="server"></asp:Literal>

                <!--下載開始-->
                <%--<div class="downloads">
                    <p>
                        <img src="images/downloads.gif" alt="&quot;&quot;" /></p>
                    <ul>
                        <li><a href="#">Downloads 001</a></li>
                        <li><a href="#">Downloads 001</a></li>
                        <li><a href="#">Downloads 001</a></li>
                        <li><a href="#">Downloads 001</a></li>
                        <li><a href="#">Downloads 001</a></li>
                    </ul>
                </div>--%>
                <!--下載結束-->

                <%--<div class="buttom001"><a href="#">
                    <img src="images/back.gif" alt="&quot;&quot;" width="55" height="28" /></a></div>--%>

                <!--------------------------------內容結束------------------------------------------------------>
            </div>
        </div>

        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>


</asp:Content>
