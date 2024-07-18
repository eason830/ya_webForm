<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="CompanyManagement.aspx.cs" Inherits="_20240702Yachts.Backend.CompanyManagement" ValidateRequest="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="pc-container">
        <div class="pc-content">
            <!-- [ breadcrumb ] start -->
            <div class="page-header">
                <div class="page-block">
                    <div class="row align-items-center">
                        <div class="col-md-12">
                            <div class="page-header-title">
                                <h5 class="mb-0">Company Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">Company Management</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!-- [ breadcrumb ] end -->

            <!-- [ Main Content ] start -->
            <div class="row">
                <!-- [ sample-page ] about start -->
                <div class="col-sm-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex;">About Us
                              
                              <!-- Button trigger modal -->
                                <%--<button type="button" style="margin-left: auto;" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                                    Add Company
                                </button>--%>

                                <!-- Modal -->
                                <%--<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">New User</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body">


                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="ButtonCreateUser" runat="server" CssClass="btn btn-primary" Text="Create" />
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>


                            </h5>
                        </div>
                        <div class="card-body">

                            <%-- 這邊放入 GridView 去做使用者管理 --%>


                            <%-- 測試 ckeditor --%>
                            <CKEditor:CKEditorControl ID="CKEditorControl1" runat="server" BasePath="/Scripts/ckeditor/" Toolbar="Bold|Italic|Underline|Strike|Subscript|Superscript|-|RemoveFormat
                            NumberedList|BulletedList|-|Outdent|Indent|-|JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock|-|BidiLtr|BidiRtl
                            /
                            Styles|Format|Font|FontSize
                            TextColor|BGColor
                            Link|Image"
                                Height="400px"></CKEditor:CKEditorControl><!-- [ Main Content ] start -->



                            <asp:Label ID="UploadAboutUsLab" runat="server" Text="Label" Visible="False" ForeColor="#009933" class="d-flex justify-content-center"></asp:Label>
                            <asp:Button ID="UploadAboutUsBtn" runat="server" Text="Upload About Us Content" class="btn btn-outline-primary btn-block mt-3" OnClick="UploadAboutUsBtn_Click" />

                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] about end -->

                <%-- certificat start --%>
                <div class="col-sm-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex;">Certificat</h5>
                        </div>

                        <div class="card-body">

                            <%-- 文字輸入框 --%>
                            <h6>Content Text :</h6>
                            <asp:TextBox ID="certificatTbox" runat="server" type="text" class="form-control" placeholder="Enter certificat text" TextMode="MultiLine" Height="200px"></asp:TextBox>
                            <asp:Label ID="uploadCertificatLab" runat="server" Visible="False" ForeColor="#009933" class="d-flex justify-content-center"></asp:Label>
                            <asp:Button ID="uploadCertificatBtn" runat="server" Text="Upload Certificat Text" class="btn btn-outline-primary btn-block mt-3" OnClick="uploadCertificatBtn_Click" />

                            <hr />

                            <%-- 證書上傳圖片，使用相簿的方式，直式 --%>
                            <h6 style="margin-top: 25px;">Upload Vertical Group Image :</h6>
                            <div class="input-group my-3">
                                <asp:FileUpload ID="imageUploadV" runat="server" class="btn btn-outline-primary btn-block" AllowMultiple="True" />
                                <asp:Button ID="UploadVBtn" runat="server" Text="Upload" class="btn btn-primary" OnClick="UploadVBtn_Click" />
                            </div>
                            <h6>Vertical Image List :</h6>
                            <asp:RadioButtonList ID="RadioButtonListV" runat="server" class="my-3 mx-auto" AutoPostBack="True" RepeatDirection="Horizontal" RepeatColumns="3" CellPadding="10" OnSelectedIndexChanged="RadioButtonListV_SelectedIndexChanged"></asp:RadioButtonList>
                            <asp:Button ID="DelVImageBtn" runat="server" Text="Delete Image" type="button" class="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete？')" Visible="False" OnClick="DelVImageBtn_Click" />

                            <hr />

                            <%-- 證書上傳圖片，使用相簿的方式，橫式 --%>
                            <h6 style="margin-top: 25px;">Upload Horizontal Group Image :</h6>
                            <div class="input-group my-3">
                                <asp:FileUpload ID="imageUploadH" runat="server" class="btn btn-outline-primary btn-block" AllowMultiple="True" />
                                <asp:Button ID="UploadHBtn" runat="server" Text="Upload" class="btn btn-primary" OnClick="UploadHBtn_Click" />
                            </div>
                            <h6>Horizontal Image List :</h6>
                            <asp:RadioButtonList ID="RadioButtonListH" runat="server" class="my-3 mx-auto" AutoPostBack="True" CellPadding="10" RepeatColumns="2" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonListH_SelectedIndexChanged"></asp:RadioButtonList>
                            <asp:Button ID="DelHImageBtn" runat="server" Text="Delete Image" type="button" class="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete？')" Visible="False" OnClick="DelHImageBtn_Click" />


                        </div>
                    </div>
                </div>


                <%-- certificat end   --%>
            </div>
            <!-- [ Main Content ] end -->
        </div>
    </div>
    <!-- [ Main Content ] end -->


</asp:Content>
