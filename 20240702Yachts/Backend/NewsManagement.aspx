<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="NewsManagement.aspx.cs" Inherits="_20240702Yachts.Backend.NewsManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- [ Main Content ] start -->
    <div class="pc-container">
        <div class="pc-content">
            <!-- [ breadcrumb ] start -->
            <div class="page-header">
                <div class="page-block">
                    <div class="row align-items-center">
                        <div class="col-md-12">
                            <div class="page-header-title">
                                <h5 class="mb-0">News Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">News Management</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!-- [ breadcrumb ] end -->

            <!-- [ Main Content ] start -->
            <div class="row">
                <!-- [ sample-page ] start -->
                <div class="col-sm-8">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex;">News</h5>
                        </div>
                        <div class="card-body">

                            <h6>Date:</h6>
                            <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="100%" OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged">

                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <SelectedDayStyle BackColor="#3399FF" ForeColor="White" Font-Bold="True" />
                                <TitleStyle BackColor="White" BorderColor="#3399FF" BorderWidth="3px" Font-Bold="True" Font-Size="12pt" ForeColor="#3399FF" />
                                <TodayDayStyle BackColor="#CCCCCC" />

                            </asp:Calendar>

                            <hr />

                            <h6>Headline:
                                <asp:Label ID="LabIsTop" runat="server" Text="* Select item is top news !" ForeColor="red" Visible="False" CssClass="badge rounded-pill bg-warning text-dark"></asp:Label></h6>

                            <asp:RadioButtonList ID="headlineRadioBtnList" class="my-3" AutoPostBack="True" runat="server" OnSelectedIndexChanged="headlineRadioBtnList_SelectedIndexChanged"></asp:RadioButtonList>

                            <asp:Button ID="deleteNewsBtn" runat="server" Text="Delete News" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete？')" Visible="True" OnClick="deleteNewsBtn_Click" />

                            <hr />

                            <h6>Add Headline :</h6>
                            <asp:CheckBox ID="CBoxIsTop" runat="server" Text="Top Tag" Width="100%" />
                            <asp:TextBox ID="headlineTbox" runat="server" type="text" CssClass="form-control" placeholder="Enter headline text" MaxLength="75"></asp:TextBox>
                            <asp:Button ID="AddHeadlineBtn" runat="server" Text="Add Headline" CssClass="btn btn-outline-primary btn-block mt-3" OnClick="AddHeadlineBtn_Click" />


                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] end -->

                <%-- Cover Content start --%>
                <asp:Panel ID="PanelCoverContent" runat="server" CssClass="col-sm-4" Visible="false">
                    <div class="card"  style="height:calc(100% - 24px);">
                        <div class="card-header">
                            <h5>Cover Content</h5>
                        </div>

                        <div class="card-body">
                            <h6>Thumbnail: </h6>
                            <asp:Image ID="ImageThumbnail" runat="server" CssClass="form-control mb-3" style="height:100px;width:auto;"/>

                            <div class="row">
                                <asp:FileUpload ID="FileUploadThumbnail" runat="server"  CssClass="form-control mb-3" style="width:70%" />
                                <asp:Button ID="ButtonThumbnail" runat="server" Text="Upload" class="btn btn-primary mb-3 col-3 " OnClick="ButtonThumbnail_Click" />
                            </div>

                            <hr />
                            
                            <h6>Summary: </h6>
                            <asp:TextBox ID="summaryTbox" runat="server" type="text" placeholder="Enter summary text" CssClass="form-control mb-4" TextMode="MultiLine" Height="200px"></asp:TextBox>
                            <asp:Label ID="LabUploadSummary" runat="server" Text="*Upload Success!" ForeColor="green" CssClass="d-flex justify-content-center" Visible="False"></asp:Label>
                            <asp:Button ID="ButtonSummary" runat="server" Text="Upload Summary"  CssClass="btn btn-outline-primary btn-block mt-3"/>

                        </div>

                    </div>
                </asp:Panel>
                <%-- Cover Content enf --%>
            </div>
            <!-- [ Main Content ] end -->

            <hr />


            <div class="row">

                <%-- Main Content start --%>
                <asp:Panel ID="PanelMainContent" runat="server" CssClass="col-sm-6" Visible="false">

                    <div class="card">
                        <div class="card-header">
                            <h5>Main Content</h5>
                        </div>

                        <div class="card-body">
                        </div>

                    </div>
                </asp:Panel>

                <%-- Main Content end --%>

                <%-- Group Image start --%>
                <asp:Panel ID="PanelGroupImage" runat="server" CssClass="col-sm-6" Visible="false">

                    <div class="card">
                        <div class="card-header">
                            <h5>Group Image</h5>
                        </div>

                        <div class="card-body">
                        </div>

                    </div>
                </asp:Panel>

                <%-- Group Image end --%>
            </div>

        </div>
    </div>
    <!-- [ Main Content ] end -->





</asp:Content>
