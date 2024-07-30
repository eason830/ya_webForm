<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteFrontend.Master" AutoEventWireup="true" CodeBehind="CompanyFront2.aspx.cs" Inherits="_20240702Yachts.Frontend.CompanyFront2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--遮罩-->
    <div class="bannermasks">
        <img src="./html/images/company.jpg" alt="&quot;&quot;" width="967" height="371" />
    </div>
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
                <p><span>COMPANY </span></p>
                <ul>
                    <li><a href="./CompanyFront1.aspx">About Us</a></li>
                    <li><a href="./CompanyFront2.aspx">Certificat</a></li>

                </ul>



            </div>




        </div>







        <!--------------------------------左邊選單結束---------------------------------------------------->

        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb"><a href="#">Home</a> >> <a href="#">Company  </a>>> <a href="#"><span class="on1">Certificat</span></a></div>
        <div class="right">
            <div class="right1">
                <div class="title"><span>Certificat</span></div>

                <!--------------------------------內容開始---------------------------------------------------->
                <div class="box3">
                    <%-- <h4>The Luxury Tayana 48 Pilothouse Lavish Woodwork Is Among</h4>
                    <p>
                        <img src="images/pit010.jpg" alt="&quot;&quot;" width="274" height="192" />
                    </p>
                    Founded in 1973, Ta Yang Building Co., Ltd. Has built over 1400 blue water cruising yachts to date. This world renowned custom yacht builder offers a large compliment of sailboats ranging from 37’ to 72’, many offer aft or center cockpit design, deck saloon or pilothouse options.<br />
                    <br />
                    In 2003, Tayana introduced the new Tayana 64 Deck Saloon, designed by Robb Ladd, which offers the latest in building techniques, large sail area and a beam of 18 feet.
                    <br />
                    <br />
                    Tayana Yachts have been considered the leader in building custom interiors for the last two decades, offering it`s clients the luxury of a living arrangement they prefer rather than have to settle for the compromise of a production boat. Using the finest in exotic woods, the best equipment such as Lewmar, Whitlock, Yanmar engines, Selden spars to name a few, Ta yang has achieved the reputation for building one of the finest semi custom blue water cruising yachts in the world, at an affordable price.
                    <br />
                    <br />
                    Peter Chen was recently appointed the General Manager of Tayana. Peter has a wide-ranging knowledge of the yacht building industry; as part of the TAYANA team, Peter’s vision is genuinely rooted in honesty and integrity. “Our aim is to create outstanding styling, live aboard comfort, and safety at sea for every proud Tayana owner.”<br />--%>

                    <asp:Literal ID="ContentText" runat="server"></asp:Literal>
                    <br />
                    <br />

                    <div class="pit">
                        <ul>
                            <%--<li></li>--%>
                            <asp:Literal ID="ContentImgV" runat="server"></asp:Literal>
                            <asp:Literal ID="ContentImgH" runat="server"></asp:Literal>

                        </ul>
                    </div>


                </div>




                <!--------------------------------內容結束------------------------------------------------------>
            </div>
        </div>

        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>





</asp:Content>
