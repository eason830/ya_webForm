<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteYachts.Master" AutoEventWireup="true" CodeBehind="YachtsOverviewFront.aspx.cs" Inherits="_20240702Yachts.Frontend.YachtsOverviewFront" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="box1">

        <asp:Literal ID="ContentHtml" runat="server"></asp:Literal>

        <%--With the world renowned pedigree combination of Ta Yang Yacht Builders, Andrew Winch Designs, and Bill Dixon Naval Architects, the Tayana Dynasty 72 ranks as an exceptional high performance cruising yacht. Space abounds in the Dynasty 72, with two spacious cockpits and a sunbathing area on the deck. The central cockpit houses twin steering positions with outdoor dining for eight and access forward into the pilothouse. All control and command equipment is readily available for minimal crew handling. The aft cockpit is accessed from the large owner's cabin and provides a pleasant seating area which opens out through a drop-down transom to the bathing platform. The Dynasty is very much a semi-custom yacht. The interior styling, furniture, and fabrics will reflect the owner's ideals and will blend with an extensive range of high quality fittings and equipment. The technical specification of the yacht will be to a very high standard. Three interior styles have been developed by Andrew Winch. Two owner versions each have four staterooms but different positions for the galley; a charter version has six double cabins with en suite heads. All versions have separate crew quarters, and all versions have the magnificent split level pilot house connecting the forward and aft lower accommodation levels. Custom interiors are available to fit the needs of you and your crew. Ta Yang has been constructing first class yachts for many years. The reputation of Chinese craftsmen over thousands of years is renowned, and it is the combination of their skills with modern design and naval architecture that has created the Tayana Dynasty 72.<br />
        --%>
        <br />

    </div>

    <div class="box3" id="dimensionTable" runat="server">
        <h4 id="dimensionTitle" runat="server"></h4>
        <table class="table02">
            <tr>
                <td class="table02td01">
                    <table>
                        <tbody>
                            <asp:Literal ID="DimensionTableHtml" runat="server"></asp:Literal>
                        </tbody>
<%--                        <tr>
                            <th>L.O.A.</th>
                            <td>72’-0”</td>
                        </tr>
                        <tr class="tr003">
                            <th>L.W.L.</th>
                            <td>60’-10”</td>
                        </tr>
                        <tr>
                            <th>Beam</th>
                            <td>20’-0”</td>
                        </tr>
                        <tr class="tr003">
                            <th>Draft (Fin Keel)</th>
                            <td>8’-6”</td>
                        </tr>
                        <tr>
                            <th>Displacement</th>
                            <td>96100lbs</td>
                        </tr>
                        <tr class="tr003">
                            <th>Ballast (Fin Keel)</th>
                            <td>24850lbs</td>
                        </tr>
                        <tr>
                            <th>Sail Area (Main + 150% Triangle)<br />
                                Main (9.0 oz)<br />
                                Stays (9.0 oz)<br />
                                No. 1 Genoa (7.2 oz)<br />
                                Genoa (150%) (7.2 oz)<br />
                                I :<br />
                                J :<br />
                                P :<br />
                                E :</th>
                            <td>2748 sq.
                                      <br />
                                ft996 sq. ft<br />
                                386 sq. ft<br />
                                1167 sq. ft<br />
                                1782 sq. ft<br />
                                87’-0”<br />
                                27’-1.75”<br />
                                75’-4”<br />
                                26’-0”<br />
                            </td>
                        </tr>
                        <tr class="tr003">
                            <th>D/L=191.47Ballast/Displacement</th>
                            <td>28.10%</td>
                        </tr>
                        <tr>
                            <th>Exterior Style, Interior Designer</th>
                            <td>Andrew Winch</td>
                        </tr>
                        <tr class="tr003">
                            <th>Naval Architect Designer</th>
                            <td>Bill Dixon</td>
                        </tr>--%>
                    </table>
                </td>

                <%--<td><img src="./html/images/ya01.jpg" alt="&quot;&quot;" width="278" height="345" /></td>--%>
                <asp:Literal ID="DimensionsImgHtml" runat="server"></asp:Literal>
            </tr>
        </table>


    </div>
    <p class="topbuttom">
        <img src="./html/images/top.gif" alt="top" />
    </p>

    <!--下載開始-->
    <div id="divDownload" class="downloads" runat="server">
        <p>
            <img src="./html/images/downloads.gif" alt="&quot;&quot;" />
        </p>
        <ul>
            <asp:Literal ID="DownloadsHtml" runat="server"></asp:Literal>
            <%--<li><a href="#">Downloads 001</a></li>
            <li><a href="#">Downloads 001</a></li>
            <li><a href="#">Downloads 001</a></li>
            <li><a href="#">Downloads 001</a></li>
            <li><a href="#">Downloads 001</a></li>--%>

        </ul>
    </div>
    <!--下載結束-->

</asp:Content>
