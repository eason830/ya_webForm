<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteFrontend.Master" AutoEventWireup="true" CodeBehind="DealersFront.aspx.cs" Inherits="_20240702Yachts.Frontend.DealersFront" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <!--------------------------------左邊選單開始---------------------------------------------------->
    <div class="left">

        <div class="left1">
            <p><span>DEALERS</span></p>
            <ul>
                <%--<li><a href="#">Unite States</a></li>--%>
                <%--<li><a href="#">Europe</a></li>--%>
                <%--<li><a href="#">Asia</a></li>--%>

                <%-- 根據資料庫的 Country 顯示出來 --%>
                <asp:Literal ID="LiteralCountry" runat="server"></asp:Literal>


            </ul>



        </div>




    </div>







    <!--------------------------------左邊選單結束---------------------------------------------------->

    <!--------------------------------右邊選單開始---------------------------------------------------->
    <div id="crumb"><a href="#">Home</a> >> <a href="#">Dealers </a>>> <a href="#"><span class="on1" id="LabLink" runat="server">Unite States</span></a></div>
    <div class="right">
        <div class="right1">
            <div class="title"><span id="LitTitle" runat="server">Unite States</span></div>

            <!--------------------------------內容開始---------------------------------------------------->
            <div class="box2_list">
                <ul>

                    <asp:Literal ID="LiteralDealerList" runat="server"></asp:Literal>

<%--                    <li>
                        <div class="list02">
                            <ul>
                                <li class="list02li">
                                    <div>
                                        <p>
                                            <img src="images/dealers001.jpg" />>
                                        </p>
                                    </div>
                                </li>
                                <li><span>Annapolis</span><br />
                                    Noyce Yachts<br />
                                    Contact：Mr. Robert Noyce
                                    <br />
                                    Address：4880 Church Lane Galesville, MD 20765
                                    <br />
                                    TEL：(410)263-3346
                                    <br />
                                    E-mail：Robert@noyceyachts.com
                                    <br />
                                    <a href="http://www.noyceyachts.com" target="_blank">www.noyceyachts.com</a></li>
                            </ul>
                        </div>
                    </li>


                    <li>
                        <div class="list02">
                            <ul>
                                <li class="list02li">
                                    <div>
                                        <p>
                                            <img src="images/dealers002.jpg" alt="&quot;&quot;" />
                                        </p>
                                    </div>
                                </li>
                                <li><span>San Francisco</span><br />
                                    Pacific Yacht Imports<br />
                                    Contact：Mr. Neil Weinberg<br />
                                    Address：Grand Marina 2051 Grand Street# 12 Alameda, CA 94501, USA<br />
                                    TEL：(510)865-2541<br />
                                    FAX：(510)865-2369<br />

                                    <a href="http://www.pacificyachtimports.net" target="_blank">www.pacificyachtimports.net</a></li>
                            </ul>
                        </div>
                    </li>

                    <li>
                        <div class="list02">
                            <ul>
                                <li class="list02li">
                                    <div>
                                        <img src="images/dealers003.jpg" alt="&quot;&quot;" />
                                    </div>
                                </li>
                                <li><span>Seattle</span><br />
                                    Seattle Yachts<br />
                                    Contact：Ted Griffin<br />
                                    Address：Shilshole Bay Marina 7001 Seaview Ave NW, Suite 150 Seattle
                                    <br />
                                    WA 98117<br />
                                    TEL：(206.789.8044<br />
                                    FAX：(206.789.3976<br />
                                    Cell：(206.819.7137<br />
                                    E-mail：ted@seattleyachts.com<br />
                                    <a href="http://www.seattleyachts.com" target="_blank">www.seattleyachts.com</a><br />
                                </li>
                            </ul>
                        </div>
                    </li>


                    <li>
                        <div class="list02">
                            <ul>
                                <li class="list02li">
                                    <div>
                                        <img src="images/dealers004.jpg" alt="&quot;&quot;" />
                                    </div>
                                </li>
                                <li><span>Seattle</span><br />
                                    Seattle Yachts<br />
                                    Contact：Ted Griffin<br />
                                    Address：Shilshole Bay Marina 7001 Seaview Ave NW, Suite 150 Seattle
                                    <br />
                                    WA 98117<br />
                                    TEL：(206.789.8044<br />
                                    FAX：(206.789.3976<br />
                                    Cell：(206.819.7137<br />
                                    E-mail：ted@seattleyachts.com<br />
                                    <a href="http://www.seattleyachts.com" target="_blank">www.seattleyachts.com</a><br />
                                </li>
                            </ul>
                        </div>
                    </li>--%>


                </ul>

                <div class="pagenumber">| <span>1</span> | <a href="#">2</a> | <a href="#">3</a> | <a href="#">4</a> | <a href="#">5</a> |  <a href="#">Next</a>  <a href="#">LastPage</a></div>
                <div class="pagenumber1">Items：<span>89</span>  |  Pages：<span>1/9</span></div>


            </div>

            <!--------------------------------內容結束------------------------------------------------------>
        </div>
    </div>

    <!--------------------------------右邊選單結束---------------------------------------------------->





</asp:Content>
