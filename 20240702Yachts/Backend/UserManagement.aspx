<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="_20240702Yachts.Backend.UserManagement" %>

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
                                <h5 class="mb-0">User Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">User Management</li>
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
                            <h5 style="display: flex;">User List
                                
                                <!-- Button trigger modal -->
                                <button type="button" style="margin-left: auto;" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                                    Add User
                                </button>

                                <!-- Modal -->
                                <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">New User</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                            </div>
                                            <div class="modal-body">
                                                <%--要新增的 User 資料--%>
                                                <asp:Label ID="LabelCreateName" CssClass="col-form-label" runat="server" Text="Name"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateName" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <asp:Label ID="LabelCreateAccount"  CssClass="col-form-label" runat="server" Text="Account"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateAccount" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <asp:Label ID="LabelCreatePassword"  CssClass="col-form-label" runat="server" Text="Password"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreatePassword" CssClass="form-control mb-3" runat="server"></asp:TextBox>

                                                <asp:Label ID="LabelCreateEmail"  CssClass="col-form-label" runat="server" Text="Email"></asp:Label>
                                                <asp:TextBox ID="TextBoxCreateEmail" CssClass="form-control mb-3" runat="server"></asp:TextBox>


                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="ButtonCreateUser" runat="server" CssClass="btn btn-primary" Text="Create" OnClick="ButtonCreateUser_Click" />
                                                <%--<button type="button" class="btn btn-primary">Create</button>--%>
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </h5>
                        </div>
                        <div class="card-body">

                            <%-- 這邊放入 GridView 去做使用者管理 --%>
                            <asp:GridView ID="GridViewUser" runat="server" CssClass="table table-responsive" AutoGenerateColumns="False" DataKeyNames="id" OnRowEditing="GridViewUser_RowEditing" OnRowCancelingEdit="GridViewUser_RowCancelingEdit" OnRowUpdating="GridViewUser_RowUpdating" OnRowDeleting="GridViewUser_RowDeleting" OnDataBound="GridViewUser_DataBound">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                    <asp:TemplateField HeaderText="account" SortExpression="account">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxAccount" runat="server" Text='<%# Bind("account") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelAccount" runat="server" Text='<%# Bind("account") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="password" SortExpression="password">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxPassword" runat="server" Text='<%# Bind("password") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelPassword" runat="server" Text='<%# Bind("password") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="email" SortExpression="email">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxEmail" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelEmail" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="name" SortExpression="name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxName" runat="server" Text='<%# Bind("name") %>' style="width:50px;"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelName" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="permission" SortExpression="permission">
                                        <EditItemTemplate>
                                            <%--<asp:TextBox ID="TextBoxPermission" runat="server" Text='<%# Bind("permission") %>' style="width:50px;"></asp:TextBox>--%>

                                            <asp:DropDownList ID="DropDownListPermission" runat="server" SelectedValue='<%# Bind("permission") %>'>
                                                 <asp:ListItem Text="admin" Value="admin"></asp:ListItem>
                                                 <asp:ListItem Text="user" Value="user"></asp:ListItem>
                                            </asp:DropDownList>

                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelPermission" runat="server" Text='<%# Bind("permission") %>'></asp:Label>

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="createDate" HeaderText="createDate" SortExpression="createDate" />--%>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                            </asp:GridView>



                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] end -->
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
                document.getElementById('<%= TextBoxCreateAccount.ClientID %>').value = '';
                document.getElementById('<%= TextBoxCreatePassword.ClientID %>').value = '';
                document.getElementById('<%= TextBoxCreateEmail.ClientID %>').value = '';
            });
        });
    </script>

</asp:Content>
