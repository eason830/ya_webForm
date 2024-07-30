<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteFrontend.Master" AutoEventWireup="true" CodeBehind="NewsFront.aspx.cs" Inherits="_20240702Yachts.Frontend.NewsFront" %>

<%@ Register Src="~/Pagination.ascx" TagPrefix="uc1" TagName="Pagination" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <style>
        div#pagination {
            height: 50px;
            margin-top: 3px;
        }

            div#pagination .count {
                float: left;
                padding: 5px;
            }

            div#pagination .pages {
                float: right;
                padding: 5px;
            }

        div#paginationTop .count {
            float: left;
            padding: 5px;
        }

        div#paginationTop .pages {
            float: right;
            padding: 5px;
        }

        div.pagination {
            padding: 0px;
            margin: 0px;
        }

            div.pagination a {
                padding: 2px 5px 2px 5px;
                margin: 2px;
                border: 1px solid #8dab68;
                text-decoration: none; /* no underline */
                color: #5f7f39;
            }

                div.pagination a:hover, div.pagination a:active {
                    border: 1px solid #5f7f39;
                    color: #000;
                }

            div.pagination span.current {
                padding: 2px 5px 2px 5px;
                margin: 2px;
                border: 1px solid #5f7f39;
                font-weight: bold;
                background-color: #5f7f39;
                color: #FFF;
            }

            div.pagination span.disabled {
                padding: 2px 5px 2px 5px;
                margin: 2px;
                border: 1px solid #EEE;
                color: #DDD;
            }

        .bold14 {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 14px;
            font-weight: bold;
        }

        .rederror {
            color: red;
        }
    </style>

    <!--遮罩-->
    <div class="bannermasks">
        <img src="./html/images/newbanner.jpg" alt="&quot;&quot;" width="967" height="371" />
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

                <div class="box2_list">

                    <%-- <ul>

                        <li>
                            <div class="list01">
                                <ul>
                                    <li>
                                        <div>
                                            <p>
                                                <img src="images/pit006.jpg" alt="&quot;&quot;" /></p>
                                        </div>
                                    </li>
                                    <li><span>2012-01-28</span><br />
                                        Tayana 58 CE Certificates are availableTayana 58 CE Certificates are availableTayana 58 CE Certificates are availableTayana 58 CE Certificates are availableTayana 58 CE Certificates are available</li>
                                    <li>availableTayana 58 CE Certificates are availableTayana 58 CE Certificates are availableTayana 58 CE Certificates are available</li>
                                </ul>
                            </div>
                        </li>

                        <li>
                            <div class="list01">
                                <ul>
                                    <li>
                                        <div>
                                            <p>
                                                <img src="images/pit007.jpg" alt="&quot;&quot;" /></p>
                                        </div>
                                    </li>
                                    <li><span>2012-01-28</span><br />
                                        Tayana 58 CE Certificates are available</li>
                                </ul>
                            </div>
                        </li>

                        <li>
                            <div class="list01">
                                <ul>
                                    <li>
                                        <div>
                                            <p>
                                                <img src="images/pit008.jpg" alt="&quot;&quot;" /></p>
                                        </div>
                                    </li>
                                    <li><span>2012-01-28</span><br />
                                        Tayana 58 CE Certificates are available</li>
                                </ul>
                            </div>
                        </li>

                        <li>
                            <div class="list01">
                                <ul>
                                    <li>
                                        <div>
                                            <p>
                                                <img src="images/pit006.jpg" alt="&quot;&quot;" width="300" /></p>
                                        </div>
                                    </li>
                                    <li><span>2012-01-28</span><br />
                                        Tayana 58 CE Certificates are available</li>
                                </ul>
                            </div>
                        </li>

                        <li>
                            <div class="list01">
                                <ul>
                                    <li>
                                        <div>
                                            <p>
                                                <img src="images/pit006.jpg" alt="&quot;&quot;" width="300" /></p>
                                        </div>
                                    </li>
                                    <li><span>2012-01-28</span><br />
                                        Tayana 58 CE Certificates are available</li>
                                </ul>
                            </div>
                        </li>
                    </ul>--%>


                    <ul>
                        <asp:Literal ID="newList" runat="server"></asp:Literal>
                    </ul>


                    <div class="pagenumber">
                        <uc1:Pagination runat="server" ID="Pagination" />
                    </div>

                    <%--<div class="pagenumber">| <span>1</span> | <a href="#">2</a> | <a href="#">3</a> | <a href="#">4</a> | <a href="#">5</a> |  <a href="#">Next</a>  <a href="#">LastPage</a></div>
                    <div class="pagenumber1">Items：<span>89</span>  |  Pages：<span>1/9</span></div>--%>
                </div>


                <!--------------------------------內容結束------------------------------------------------------>
            </div>
        </div>

        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>







</asp:Content>
