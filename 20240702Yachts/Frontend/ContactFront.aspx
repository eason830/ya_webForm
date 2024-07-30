<%@ Page Title="" Language="C#" MasterPageFile="~/Frontend/SiteFrontend.Master" AutoEventWireup="true" CodeBehind="ContactFront.aspx.cs" Inherits="_20240702Yachts.Frontend.ContactFront" %>

<%-- 當使用 recaptcha 控制項，下列一行會自動產生出來 --%>
<%@ Register Assembly="Recaptcha.Web" Namespace="Recaptcha.Web.UI.Controls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!--遮罩-->
    <%--    <div class="bannermasks">
        <img src="./html/images/contact.jpg" alt="&quot;&quot;" width="967" height="371" />
    </div>--%>
    <!--遮罩結束-->

    <div class="banner">
        <ul>
            <li>
                <img src="./html/images/contact.jpg" alt="Tayana Yachts" width="967" height="371" />
            </li>
        </ul>
    </div>


    <div class="conbg">
        <!--------------------------------左邊選單開始---------------------------------------------------->
        <div class="left">

            <div class="left1">
                <p><span>CONTACT</span></p>
                <ul>
                    <li><a href="./ContactFront.aspx">contacts</a></li>
                </ul>



            </div>




        </div>







        <!--------------------------------左邊選單結束---------------------------------------------------->

        <!--------------------------------右邊選單開始---------------------------------------------------->
        <div id="crumb"><a href="./IndexFront.aspx">Home</a> >> <a href="./ContactFront.aspx"><span class="on1">Contact</span></a></div>
        <div class="right">
            <div class="right1">
                <div class="title"><span>Contact</span></div>

                <!--------------------------------內容開始---------------------------------------------------->
                <!--表單-->
                <div class="from01">
                    <p>
                        Please Enter your contact information<span class="span01">*Required</span>
                    </p>
                    <br />
                    <table>
                        <tr>
                            <td class="from01td01">Name :</td>
                            <td>
                                <%--<span>*</span><input type="text" name="textfield" id="textfield" />--%>
                                <span>*</span><asp:TextBox ID="TextBoxName" runat="server" name="Name" type="text" class="{validate:{required:true,messages:{required:'Required'}}}" Style="width: 250px;" required="" aria-required="true" oninput="setCustomValidity('');" oninvalid="setCustomValidity('Required!')" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">Email :</td>
                            <td>
                                <%--<span>*</span><input type="text" name="textfield" id="textfield" />--%>
                                <span>*</span><asp:TextBox ID="TextBoxEmail" runat="server" name="Email" type="text" class="{validate:{required:true,email:true,messages:{required:'Required',email:'Please check the E-mail format is correct'}}}" Style="width: 250px;" required="" aria-required="true" oninput="setCustomValidity('');" oninvalid="setCustomValidity('Required!')" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">Phone :</td>
                            <td>
                                <%--<span>*</span><input type="text" name="textfield" id="textfield" />--%>
                                <span>*</span><asp:TextBox runat="server" name="Phone" type="text" ID="TextBoxPhone" class="{validate:{required:true, messages:{required:'Required'}}}" Style="width: 250px;" required="" aria-required="true" oninput="setCustomValidity('');" oninvalid="setCustomValidity('Required!')" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">Country :</td>
                            <td>
                                <span>*</span>

                                <%--<select name="select" id="select">
                        <option>Annapolis</option>
                    </select>--%>

                                <asp:DropDownList ID="DropDownListCountry" runat="server" name="Country" DataSourceID="SqlDataSource1" DataTextField="countryName" DataValueField="countryName"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" ProviderName="<%$ ConnectionStrings:20240702YachtsConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [Country]"></asp:SqlDataSource>


                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><span>*</span>Brochure of interest  *Which Brochure would you like to view?</td>
                        </tr>
                        <tr>
                            <td class="from01td01">&nbsp;</td>
                            <td>

                                <%--<select name="select" id="select">
                        <option>Dynasty 72 </option>
                    </select>--%>

                                <%-- type => yachtsModel--%>
                                <%-- 連接資料庫拿型號放入下拉選單 --%>
                                <asp:DropDownList ID="DropDownListYachts" name="Yachts" runat="server" DataTextField="yachtsModel" DataValueField="yachtsModel"></asp:DropDownList>


                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">Comments:</td>
                            <td>
                                <%--<textarea name="textarea" id="textarea" cols="45" rows="5"></textarea>--%>
                                <asp:TextBox ID="TextBoxComments" runat="server" name="Comments" TextMode="MultiLine" Rows="2" Columns="20" Style="height: 150px; width: 330px;" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <%-- 機器人驗證放置的地方 --%>
                                <!-- Render recaptcha API script (非必要，同頁使用兩個以上時才需要)-->
                                <%--<cc1:RecaptchaApiScript ID="RecaptchaApiScript1" runat="server" />--%>
                                <!-- Render recaptcha widget -->
                                <cc1:RecaptchaWidget ID="Recaptcha1" runat="server" />

                                <%--顯示驗證有沒有通過的訊息  --%>
                                <asp:Label ID="lblMessage" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="from01td01">&nbsp;</td>

                            <%-- mail --%>
                            <%-- xdzu bszk mleh yhts --%>

                            <%-- 送出 submit 的 button  --%>
                            <td class="f_right">
                                <%-- old submit button --%>
                                <%--<a href="#"><img src="./html/images/buttom03.gif" alt="submit" width="59" height="25" /></a>--%>
                                <asp:ImageButton ID="ImageButton1" runat="server" Width="59" Height="25" AlternateText="submit" ImageUrl="./html/images/buttom03.gif" OnClick="ImageButton1_Click" />
                            </td>

                        </tr>
                    </table>
                </div>
                <!--表單-->

                <div class="box1">
                    <span class="span02">Contact with us</span><br />
                    Thanks for your enjoying our web site as an introduction to the Tayana world and our range of yachts.
As all the designs in our range are semi-custom built, we are glad to offer a personal service to all our potential customers. 
If you have any questions about our yachts or would like to take your interest a stage further, please feel free to contact us.
                </div>

                <div class="list03">
                    <p>
                        <span>TAYANA HEAD OFFICE</span><br />
                        NO.60 Haichien Rd. Chungmen Village Linyuan Kaohsiung Hsien 832 Taiwan R.O.C<br />
                        tel. +886(7)641 2422<br />
                        fax. +886(7)642 3193<br />
                        info@tayanaworld.com<br />
                    </p>
                </div>


                <div class="list03">
                    <p>
                        <span>SALES DEPT.</span><br />
                        +886(7)641 2422  ATTEN. Mr.Basil Lin<br />
                        <br />
                    </p>
                </div>

                <div class="box4">
                    <h4>Location</h4>
                    <p>
                        <%--<iframe width="695" height="518" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="http://maps.google.com/maps?f=d&amp;source=s_d&amp;saddr=%E5%8F%B0%E7%81%A3%E9%AB%98%E9%9B%84%E5%B8%82%E5%B0%8F%E6%B8%AF%E5%8D%80%E4%B8%AD%E5%B1%B1%E5%9B%9B%E8%B7%AF%E9%AB%98%E9%9B%84%E5%B0%8F%E6%B8%AF%E6%A9%9F%E5%A0%B4&amp;daddr=%E5%8F%B0%E7%81%A3%E9%AB%98%E9%9B%84%E5%B8%82%E6%9E%97%E5%9C%92%E5%8D%80%E4%B8%AD%E9%96%80%E6%9D%91%E6%B5%B7%E5%A2%98%E8%B7%AF%EF%BC%96%EF%BC%90%E8%99%9F&amp;hl=zh-en&amp;geocode=FRthWAEdwlwsByGxkQ4S1t-ckinNS9aM0xxuNDELEXJZh6Soqg%3BFRRmVwEdMKssBym5azbzl-JxNDGd62mwtzGaDw&amp;aq=0&amp;oq=%E9%AB%98%E9%9B%84%E5%B0%8F%E6%B8%AF%E6%A9%9F&amp;sll=22.50498,120.36792&amp;sspn=0.008356,0.016512&amp;g=%E5%8F%B0%E7%81%A3%E9%AB%98%E9%9B%84%E5%B8%82%E6%9E%97%E5%9C%92%E5%8D%80%E4%B8%AD%E9%96%80%E6%9D%91%E6%B5%B7%E5%A2%98%E8%B7%AF%EF%BC%96%EF%BC%90%E8%99%9F&amp;mra=ls&amp;ie=UTF8&amp;t=m&amp;ll=22.537135,120.360718&amp;spn=0.08213,0.119133&amp;z=13&amp;output=embed"></iframe>--%>
                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d1640.4162622388355!2d120.311535218927!3d22.63103546636723!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x346e04924b8f6fdd%3A0xb513b63ca984f160!2z6auY6ZuE5biC5paw6IiI5Y2A5L-h576p5ZyL5rCR5bCP5a24!5e0!3m2!1szh-TW!2stw!4v1719911192534!5m2!1szh-TW!2stw" width="695" height="518" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
                    </p>

                </div>




                <!--------------------------------內容結束------------------------------------------------------>

            </div>
        </div>

        <!--------------------------------右邊選單結束---------------------------------------------------->
    </div>




</asp:Content>
