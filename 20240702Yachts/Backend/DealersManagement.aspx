<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="DealersManagement.aspx.cs" Inherits="_20240702Yachts.Backend.DealersManagement" %>

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
                                <h5 class="mb-0">Dealers Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">Dealers Management</li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <!-- [ breadcrumb ] end -->

            <!-- [ Main Content ] start -->
            <div class="row">
                <!-- [ sample-page ] start -->
                <div class="col-sm-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex;">Country List
                                <!-- Button trigger modal -->
                                <button type="button" style="margin-left: auto;" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                                    Add Country
                                </button>

                                <!-- Modal -->
                                <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">New Country</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body">
                                                <%--要新增的 Country name --%>
                                                <asp:Label ID="LabelCreateName" CssClass="col-form-label" runat="server" Text="Name"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateName" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="ButtonCreateCountry" runat="server" CssClass="btn btn-primary" Text="Create" OnClick="ButtonCreateCountry_Click" />
                                                <%--<button type="button" class="btn btn-primary">Create</button>--%>
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </h5>
                        </div>
                        <div class="card-body">

                            <asp:GridView ID="GridViewCountry" runat="server" CssClass="table table-responsive" AutoGenerateColumns="False" DataKeyNames="id" OnRowEditing="GridViewCountry_RowEditing" OnRowCancelingEdit="GridViewCountry_RowCancelingEdit" OnRowUpdating="GridViewCountry_RowUpdating" OnRowDeleting="GridViewCountry_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                    <asp:TemplateField HeaderText="countryName" SortExpression="countryName">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxCountryName" runat="server" Text='<%# Bind("countryName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelCountryName" runat="server" Text='<%# Bind("countryName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="createTime" HeaderText="createTime" SortExpression="createTime" ReadOnly="True" />
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                            </asp:GridView>


                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] end -->

                <%-- 第二個區塊 start --%>
                <div class="col-sm-12">
                    <div class="card">
                        <div class="card-header">
                            <h5 style="display: flex; align-items: center;">Dealers List

                                <%-- 下方變動 ， 要將 modal 的 Country 跟著變動，顯示的 List 也要變動成該 Country 的 Dealers List --%>
                                <asp:DropDownList CssClass="form-select ms-3" Style="width: 200px;" ID="DropDownListCountrySelect" runat="server" DataSourceID="SqlDataSource1" DataTextField="countryName" DataValueField="id" OnSelectedIndexChanged="DropDownListCountrySelect_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT * FROM [Country]"></asp:SqlDataSource>

                                <%-- 新增 dealers 的 modal --%>
                                <!-- Button trigger modal -->
                                <button type="button" style="margin-left: auto;" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal2">
                                    Add Dealers</button>

                                <!-- Modal -->
                                <div class="modal fade" id="exampleModal2" tabindex="-1" aria-labelledby="exampleModalLabe2" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabe2">New Dealers</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body">

                                                <%-- 新增需要的資料 --%>

                                                <%-- countryId *  下拉選單--%>
                                                <asp:Label ID="LabelCreateDealersCountry" CssClass="col-form-label" runat="server" Text="Country *"></asp:Label>
                                                <asp:DropDownList CssClass="form-select mb-3" Style="width: 200px;" ID="DropDownListCountrySelect2" runat="server" DataSourceID="SqlDataSource1" DataTextField="countryName" DataValueField="id"></asp:DropDownList>

                                                <%-- area *--%>
                                                <asp:Label ID="LabelCreateDealersArea" CssClass="col-form-label" runat="server" Text="Dealers Area *"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersArea" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- dealerImgPath 這個直接放上傳圖片路徑，當有上傳圖片的話，順便讓他可以設置--%>
                                                <asp:Label ID="LabelCreateDealersImg" CssClass="col-form-label" runat="server" Text="Dealers Img"></asp:Label>
                                                <asp:FileUpload ID="FileUploadCreateDealersImg" CssClass="form-control mb-3" runat="server" AllowMultiple="True" />


                                                <%-- name --%>
                                                <asp:Label ID="LabelCreateDealersName" CssClass="col-form-label" runat="server" Text="Dealers Name"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersName" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- contact --%>
                                                <asp:Label ID="LabelCreateDealersContact" CssClass="col-form-label" runat="server" Text="Contact"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersContact" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- address --%>
                                                <asp:Label ID="LabelCreateDealersAddress" CssClass="col-form-label" runat="server" Text="Address"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersAddress" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- tel --%>
                                                <asp:Label ID="LabelCreateDealersTel" CssClass="col-form-label" runat="server" Text="Tel"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersTel" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- fax --%>
                                                <asp:Label ID="LabelCreateDealersFax" CssClass="col-form-label" runat="server" Text="Fax"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersFax" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- email --%>
                                                <asp:Label ID="LabelCreateDealersEmail" CssClass="col-form-label" runat="server" Text="Email"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersEmail" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <%-- link --%>
                                                <asp:Label ID="LabelCreateDealersLink" CssClass="col-form-label" runat="server" Text="link"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateDealersLink" CssClass="form-control mb-3" runat="server"></asp:TextBox>


                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="ButtonCreateDealers" runat="server" CssClass="btn btn-primary" Text="Create" OnClick="ButtonCreateDealers_Click" />
                                                <%--<button type="button" class="btn btn-primary">Create</button>--%>
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </h5>
                        </div>
                        <div class="card-body">
                            <%-- GridViewDealers 內容 --%>
                            <asp:GridView ID="GridViewDealers" runat="server" CssClass="table table-responsive" AutoGenerateColumns="False" DataKeyNames="id" OnRowEditing="GridViewDealers_RowEditing" OnRowDeleting="GridViewDealers_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                    <asp:BoundField DataField="area" HeaderText="area" SortExpression="area" ReadOnly="True" />
                                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="圖片" SortExpression="dealerImgPath">
                                        <ItemTemplate>
                                            <img src='<%# Eval("dealerImgPath") %>' alt="Image" style="max-width: 100px; max-height: 100px;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="countryId" HeaderText="countryId" SortExpression="countryId" />--%>
                                    <asp:BoundField DataField="createDate" HeaderText="createDate" SortExpression="createDate" ReadOnly="True" />


                                    <%-- 先將原生的 edit 隱藏，改使用 modal 的 --%>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
                <%-- 第二個區塊 end   --%>


                <%-- Edit Panel 區塊 Start --%>

                <%--<button type="button" style="margin-left: auto;" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#ContentPlaceHolder1_exampleModal3">
                    edit Dealers</button>--%>

                <asp:Panel ID="exampleModal3" runat="server" CssClass="modal fade" TabIndex="-1" aria-labelledby="exampleModalLabe3" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabe3">Edit Dealers</h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">

                                <%-- 新增需要的資料 --%>
                                <asp:TextBox ID="TextBoxEditDealersId" runat="server" Style="display: none;"></asp:TextBox>

                                <%-- countryId *  下拉選單--%>
                                <asp:Label ID="LabelEditDealersCountry" CssClass="col-form-label" runat="server" Text="Country *"></asp:Label>
                                <asp:DropDownList CssClass="form-select mb-3" Style="width: 200px;" ID="DropDownListCountrySelect3" runat="server" DataSourceID="SqlDataSource1" DataTextField="countryName" DataValueField="id"></asp:DropDownList>

                                <%-- area *--%>
                                <asp:Label ID="LabelEditDealersArea" CssClass="col-form-label" runat="server" Text="Dealers Area *"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersArea" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- dealerImgPath 這個直接放上傳圖片路徑，當有上傳圖片的話，順便讓他可以設置--%>
                                <asp:Label ID="LabelEditDealersImg2" CssClass="col-form-label" runat="server" Text="Thumbnail"></asp:Label>
                                <asp:Image ID="ImageEditDealersImg" runat="server" CssClass="form-control mb-3" Style="max-width: 100px; max-height: 100px;" />

                                <asp:Label ID="LabelEditDealersImg" CssClass="col-form-label" runat="server" Text="Dealers Img"></asp:Label>
                                <asp:FileUpload ID="FileUploadEditDealersImg" CssClass="form-control mb-3" runat="server" AllowMultiple="True" />


                                <%-- name --%>
                                <asp:Label ID="LabelEditDealersName" CssClass="col-form-label" runat="server" Text="Dealers Name"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersName" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- contact --%>
                                <asp:Label ID="LabelEditDealersContact" CssClass="col-form-label" runat="server" Text="Contact"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersContact" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- address --%>
                                <asp:Label ID="LabelEditDealersAddress" CssClass="col-form-label" runat="server" Text="Address"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersAddress" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- tel --%>
                                <asp:Label ID="LabelEditDealersTel" CssClass="col-form-label" runat="server" Text="Tel"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersTel" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- fax --%>
                                <asp:Label ID="LabelEditDealersFax" CssClass="col-form-label" runat="server" Text="Fax"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersFax" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- email --%>
                                <asp:Label ID="LabelEditDealersEmail" CssClass="col-form-label" runat="server" Text="Email"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersEmail" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                <%-- link --%>
                                <asp:Label ID="LabelEditDealersLink" CssClass="col-form-label" runat="server" Text="link"></asp:Label>
                                <asp:TextBox ID="TextBoxEditDealersLink" CssClass="form-control mb-3" runat="server"></asp:TextBox>


                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="ButtonEditDealers" runat="server" CssClass="btn btn-primary" Text="Edit" OnClick="ButtonEditDealers_Click" />
                                <%--<button type="button" class="btn btn-primary">Create</button>--%>
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                            </div>
                        </div>
                    </div>

                </asp:Panel>


                <%-- Edit Panel 區塊 End   --%>
            </div>
            <!-- [ Main Content ] end -->
        </div>
    </div>
    <!-- [ Main Content ] end -->

    <%-- script --%>
    <%-- 當 modal 關閉的時候將內容清空 --%>
    <!-- Custom JS to handle modal close event -->
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var exampleModalEl = document.getElementById('exampleModal');
            exampleModalEl.addEventListener('hidden.bs.modal', function (event) {
                // 清空文本框的內容
                document.getElementById('<%= TextBoxCreateName.ClientID %>').value = '';
            });

            var exampleModalE2 = document.getElementById('exampleModal2');
            exampleModalE2.addEventListener('hidden.bs.modal', function (event) {
                // 清空文本框的內容

                // countryId => 還原預設值 => 抓另外一個 dropDownList的值
                document.getElementById('<%= DropDownListCountrySelect2.ClientID %>').value = document.getElementById('<%= DropDownListCountrySelect.ClientID %>').value;

                // area
                document.getElementById('<%= TextBoxCreateDealersArea.ClientID %>').value = '';

                // dealerImgPath => 將上傳的檔案刪除
                document.getElementById('<%= FileUploadCreateDealersImg.ClientID %>').value = '';

                // name
                document.getElementById('<%= TextBoxCreateDealersName.ClientID %>').value = '';


                <%-- contact --%>
                document.getElementById('<%= TextBoxCreateDealersContact.ClientID %>').value = '';


                <%-- address --%>
                document.getElementById('<%= TextBoxCreateDealersAddress.ClientID %>').value = '';

                <%-- tel --%>
                document.getElementById('<%= TextBoxCreateDealersTel.ClientID %>').value = '';

                <%-- fax --%>
                document.getElementById('<%= TextBoxCreateDealersFax.ClientID %>').value = '';

                <%-- email --%>
                document.getElementById('<%= TextBoxCreateDealersEmail.ClientID %>').value = '';

                <%-- link --%>
                document.getElementById('<%= TextBoxCreateDealersLink.ClientID %>').value = '';


            });
        });


        function clearAddDealersForm() {
            // 清空文本框的內容

            // countryId => 還原預設值 => 抓另外一個 dropDownList的值
            document.getElementById('<%= DropDownListCountrySelect2.ClientID %>').value = document.getElementById('<%= DropDownListCountrySelect.ClientID %>').value;

            // area
            document.getElementById('<%= TextBoxCreateDealersArea.ClientID %>').value = '';

            // dealerImgPath => 將上傳的檔案刪除
            document.getElementById('<%= FileUploadCreateDealersImg.ClientID %>').value = '';

            // name
            document.getElementById('<%= TextBoxCreateDealersName.ClientID %>').value = '';


             <%-- contact --%>
            document.getElementById('<%= TextBoxCreateDealersContact.ClientID %>').value = '';


            <%-- address --%>
            document.getElementById('<%= TextBoxCreateDealersAddress.ClientID %>').value = '';

            <%-- tel --%>
            document.getElementById('<%= TextBoxCreateDealersTel.ClientID %>').value = '';

             <%-- fax --%>
            document.getElementById('<%= TextBoxCreateDealersFax.ClientID %>').value = '';

            <%-- email --%>
            document.getElementById('<%= TextBoxCreateDealersEmail.ClientID %>').value = '';

            <%-- link --%>
            document.getElementById('<%= TextBoxCreateDealersLink.ClientID %>').value = '';
        }


        function showModal() {
            var myModal = new bootstrap.Modal(document.getElementById('ContentPlaceHolder1_exampleModal3'), {});
            myModal.show();
        }


        function scrollToBottom() {
            //window.scrollTo(0, document.body.scrollHeight);
            window.scrollTo(0, 500);
        }


    </script>

</asp:Content>
