<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="OverviewManagement.aspx.cs" Inherits="_20240702Yachts.Backend.OverviewManagement" %>


<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%-- 在 <head> 內引用 PDF.js 用來在頁面預覽 PDF 內容 --%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.9.359/pdf.min.js"></script>

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
                                <h5 class="mb-0">Overview Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">Overview Management</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!-- [ breadcrumb ] end -->

            <!-- [ Main Content ] start -->
            <div class="row">
                <!-- [ sample-page ] start -->
                <div class="col-sm-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex;">Yachts Overview</h5>
                        </div>
                        <div class="card-body">

                            <h6>YachYacht Model :</h6>
                            <asp:DropDownList ID="DListModel" runat="server" DataSourceID="SqlDataSource1" DataTextField="yachtsModel" DataValueField="id" AutoPostBack="True" Width="100%" Font-Bold="True" class="btn btn-outline-primary dropdown-toggle" OnSelectedIndexChanged="DListModel_SelectedIndexChanged"></asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT * FROM [Yachts]"></asp:SqlDataSource>


                            <hr />

                            <h6>Dimensions Image :</h6>
                            <asp:Literal ID="LiteralDimImg" runat="server"></asp:Literal>

                            <div clsss="input-group my-3">
                                <asp:FileUpload ID="DimImgUpload" runat="server" class="btn btn-outline-primary btn-block" Width="70%" />
                                <asp:Button ID="BtnUploadDimImg" runat="server" Text="Upload" class="btn btn-primary" Width="25%" Style="height: 46px; display: inline-block" OnClick="BtnUploadDimImg_Click" />
                            </div>

                            <span class="badge rounded-pill bg-warning text-dark mt-3">*Upload by No Choose File Could Clean File!</span>
                            <hr />

                            <h6>Downloads File :</h6>
                            <asp:Literal ID="PDFpreview" runat="server"></asp:Literal>

                            <div clsss="input-group my-3">
                                <asp:FileUpload ID="FileUpload1" runat="server" class="btn btn-outline-primary btn-block" Width="70%" />
                                <asp:Button ID="BtnUploadFile" runat="server" Text="Upload" class="btn btn-primary" Width="25%" Style="height: 46px; display: inline-block" OnClick="BtnUploadFile_Click" />
                            </div>

                            <span class="badge rounded-pill bg-warning text-dark mt-3">*Upload by No Choose File Could Clean File!</span>

                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] end -->

                <%-- Overview Dimensions Text start --%>
                <asp:Panel ID="PanelOverviewDimensionsText" runat="server" CssClass="col-sm-8" Visible="true">
                    <div class="card" style="height: calc(100% - 24px);">
                        <div class="card-header">
                            <h5>Overview Dimensions Text</h5>
                        </div>

                        <div class="card-body">

                            <h6>Dimensions Text :</h6>

                            <table class="table table-hover">

                                <thead>

                                    <tr class="table-info">

                                        <th>Item
                                            <asp:Button ID="AddRow" runat="server" Text="Add Row" class="btn btn-outline-primary btn-sm py-0 px-1 align-top mx-5" OnClick="AddRow_Click" />
                                        </th>

                                        <th>Value
                                            <asp:Button ID="DeleteRow" runat="server" Text="Delete Row" class="btn btn-outline-danger btn-sm py-0 px-1 align-top mx-5" OnClick="DeleteRow_Click" />
                                        </th>
                                    </tr>


                                </thead>

                                <tbody>

                                    <asp:Literal ID="LitDimensionsHtml" runat="server"></asp:Literal>

                                    <tr>
                                        <th>
                                            <p class="d-inline-block m-r-20">Dimensions Image</p>
                                        </th>

                                        <td>
                                            <asp:TextBox ID="TBoxDimImg" runat="server" type="text" class="form-control" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <th>
                                            <p class="d-inline-block m-r-20">Downloads Title</p>
                                        </th>

                                        <td class="table-info">
                                            <asp:TextBox ID="TBoxDLTitle" runat="server" type="text" class="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <th>
                                            <p class="d-inline-block m-r-20">Downloads File</p>
                                        </th>
                                        <td>
                                            <asp:TextBox ID="TBoxDLFile" runat="server" type="text" class="form-control" ReadOnly="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th>
                                            <p class="d-inline-block m-r-20">Video URL</p>
                                        </th>
                                        <td class="table-info">
                                            <asp:TextBox ID="TBoxVideo" runat="server" type="text" class="form-control" TextMode="Url"></asp:TextBox>
                                        </td>
                                    </tr>

                                </tbody>

                                <tfoot>
                                    <tr>
                                        <th>
                                            <asp:Label ID="LabUpdateTitle" runat="server" Text="Click for Update"></asp:Label>
                                        </th>
                                        <td>
                                            <asp:Button ID="BtnUpdateDimensionsList" runat="server" Text="Update Dimensions List" class="btn btn-outline-primary btn-block" OnClick="BtnUpdateDimensionsList_Click" />
                                            <asp:Label ID="LabUpdateDimensionsList" runat="server" Text="*Upload Success!" ForeColor="green" class="d-flex justify-content-center" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </tfoot>


                            </table>

                        </div>

                    </div>
                </asp:Panel>
                <%-- Overview Dimensions Text end --%>
            </div>
            <!-- [ Main Content ] end -->

            <hr />


            <div class="row">

                <%-- Overview Content start --%>
                <asp:Panel ID="PanelOverviewContent" runat="server" CssClass="col-sm-6" Visible="true">

                    <div class="card">
                        <div class="card-header">
                            <h5>Overview Content</h5>
                        </div>

                        <div class="card-body">

                            <!-- [ Main Content ] start -->
                            <h6>Main Content :</h6>
                            <%--  ckeditor --%>
                            <CKEditor:CKEditorControl ID="CKEditorControl1" runat="server" BasePath="/Scripts/ckeditor/" Toolbar="Bold|Italic|Underline|Strike|Subscript|Superscript|-|RemoveFormat
                            NumberedList|BulletedList|-|Outdent|Indent|-|JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock|-|BidiLtr|BidiRtl
                            /
                            Styles|Format|Font|FontSize
                            TextColor|BGColor
                            Link|Image"
                                Height="400px">
                            </CKEditor:CKEditorControl>

                            <asp:Label ID="UploadOverviewContentLab" runat="server" Text="" Visible="False" ForeColor="#009933" class="d-flex justify-content-center"></asp:Label>
                            <asp:Button ID="UploadOverviewContentBtn" runat="server" Text="Upload Overview Content" class="btn btn-outline-primary btn-block mt-3" OnClick="UploadOverviewContentBtn_Click" />

                        </div>

                    </div>
                </asp:Panel>

                <%-- Overview Content end --%>
            </div>

        </div>
    </div>
    <!-- [ Main Content ] end -->




</asp:Content>
