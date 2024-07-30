<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="SpecificationManagement.aspx.cs" Inherits="_20240702Yachts.Backend.SpecificationManagement"  ValidateRequest="false"%>

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
                                <h5 class="mb-0">Specification Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">Specification Management</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!-- [ breadcrumb ] end -->

            <!-- [ Main Content ] start -->
            <div class="row">
                <!-- [ sample-page ] start -->
                <div class="col-sm-5">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex;">Yachts Layout</h5>
                        </div>
                        <div class="card-body">

                            <h6>YachYacht Model :</h6>
                            <asp:DropDownList ID="DListModel" runat="server" DataSourceID="SqlDataSource1" DataTextField="yachtsModel" DataValueField="id" AutoPostBack="True" Width="100%" Font-Bold="True" class="btn btn-outline-primary dropdown-toggle" OnSelectedIndexChanged="DListModel_SelectedIndexChanged"></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT * FROM [Yachts]"></asp:SqlDataSource>


                            <hr />

                            <h6>Layout & Deck Image :</h6>
                            <h6><span class="badge rounded-pill bg-warning text-dark">* The maximum upload size at once is 10MB !</span></h6>


                            <div clsss="input-group my-3">
                                <asp:FileUpload ID="imageUpload" runat="server" class="btn btn-outline-primary btn-block" Width="70%" />
                                <asp:Button ID="UploadImgBtn" runat="server" Text="Upload" class="btn btn-primary" Width="25%" Style="height: 46px; display: inline-block" OnClick="UploadImgBtn_Click" />
                            </div>

                            <hr />

                            <h6>Group Image List :</h6>

                            <asp:RadioButtonList ID="RadioButtonListImg" runat="server" class="my-3 mx-auto" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonListImg_SelectedIndexChanged"></asp:RadioButtonList>
                            <asp:Button ID="DelImageBtn" runat="server" Text="Delete Image" type="button" class="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete？')" Visible="False" OnClick="DelImageBtn_Click" />

                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] end -->

                <%-- DetailSpecification start --%>
                <asp:Panel ID="PanelDetailSpecification" runat="server" CssClass="col-sm-7" Visible="true">
                    <div class="card" style="height: calc(100% - 24px);">
                        <div class="card-header">
                            <h5>Detail Specification</h5>
                        </div>

                        <div class="card-body">

                            <h6>Detail Title :</h6>
                            <asp:DropDownList ID="DListDetailTitle" runat="server" DataSourceID="SqlDataSource2" DataTextField="detailTitleSort" DataValueField="id" AutoPostBack="True" Width="100%" Font-Bold="True" class="btn btn-outline-primary dropdown-toggle" OnSelectedIndexChanged="DListDetailTitle_SelectedIndexChanged"></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT * FROM [DetailTitleSort]"></asp:SqlDataSource>

                            <hr />

                            <h6>Add Detail :</h6>
                            <asp:TextBox ID="TboxDetail" runat="server" type="text" class="form-control" placeholder="Enter detail text" TextMode="MultiLine" Height="100px"></asp:TextBox>
                            <asp:Button ID="BtnAddDetail" runat="server" Text="Add Detail" class="btn btn-outline-primary btn-block mt-3" OnClick="BtnAddDetail_Click" />

                            <hr />

                            <h6>Detail List :</h6>
                            <asp:RadioButtonList ID="RadioButtonListDetail" runat="server" class="my-3 mx-auto" AutoPostBack="True" RepeatDirection="Vertical" Width="100%" OnSelectedIndexChanged="RadioButtonListDetail_SelectedIndexChanged"></asp:RadioButtonList>
                            <asp:Button ID="BtnDelDetail" runat="server" Text="Delete Detail" type="button" class="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete？')" Visible="False" OnClick="BtnDelDetail_Click" />


                        </div>

                    </div>
                </asp:Panel>
                <%-- DetailSpecification end --%>
            </div>
            <!-- [ Main Content ] end -->

            <hr />


            <div class="row">

                <%-- Add New Detail Title start --%>
                <asp:Panel ID="PanelAddNewDetailTitle" runat="server" CssClass="col-sm-4" Visible="true">

                    <div class="card">
                        <div class="card-header">
                            <h5>Add New Detail Title</h5>
                        </div>

                        <div class="card-body">

                            <h6>Add New Title :</h6>
                            <div class="input-group mb-3">
                                <asp:TextBox ID="TBoxAddNewTitle" runat="server" type="text" class="form-control" placeholder="Enter new title"></asp:TextBox>
                                <div class="input-group-append">
                                    <asp:Button ID="BtnAddNewTitle" runat="server" Text="Add" class="btn btn-outline-primary btn-block" OnClick="BtnAddNewTitle_Click"/>
                                </div>
                            </div>

                        </div>

                    </div>
                </asp:Panel>

                <%-- Add New Detail Title end --%>

                <%-- Detail Title List start--%>
                <asp:Panel ID="PanelDetailTitleList" runat="server" CssClass="col-sm-8" Visible="true">

                    <div class="card">
                        <div class="card-header">
                            <h5>Detail Title List</h5>
                        </div>

                        <div class="card-body">

                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id" OnRowDeleted="GridView1_RowDeleted" OnRowUpdated="GridView1_RowUpdated" CssClass="table table-responsive" OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                    <asp:TemplateField HeaderText="detailTitleSort" SortExpression="detailTitleSort">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxDetailTitleSort" runat="server" Text='<%# Bind("detailTitleSort") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelDetailTitleSort" runat="server" Text='<%# Bind("detailTitleSort") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="creatTime" HeaderText="creatTime" SortExpression="creatTime" ReadOnly="True" />
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                            </asp:GridView>


                            <%--<asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT * FROM [DetailTitleSort]"></asp:SqlDataSource>--%>


                        </div>

                    </div>
                </asp:Panel>
                <%-- Detail Title List end--%>
            </div>

        </div>
    </div>
    <!-- [ Main Content ] end -->




</asp:Content>
