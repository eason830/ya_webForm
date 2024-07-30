<%@ Page Title="" Language="C#" MasterPageFile="~/Backend/SiteBackend.Master" AutoEventWireup="true" CodeBehind="YachtManagement.aspx.cs" Inherits="_20240702Yachts.Backend.YachtManagement" %>

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
                                <h5 class="mb-0">Yacht Management</h5>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <ul class="breadcrumb mb-0">
                                <li class="breadcrumb-item"><a href="../dashboard/index.html">Home</a></li>
                                <li class="breadcrumb-item"><a href="javascript: void(0)">Other</a></li>
                                <li class="breadcrumb-item" aria-current="page">Yacht Management</li>
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
                            <h5 style="display: flex;">Yacht</h5>
                        </div>
                        <div class="card-body">

                            <div class="input-group my-3">

                                <%-- 選擇 Yacht 型號 --%>
                                <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="yachtsModel" DataValueField="id" AutoPostBack="True"  Width="20%" Font-Bold="True" class="btn btn-outline-primary dropdown-toggle" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT * FROM [Yachts]"></asp:SqlDataSource>

                                <%-- 上傳某型號的圖片 --%>
                                <asp:FileUpload ID="imageUpload" runat="server" class="btn btn-outline-primary btn-block" AllowMultiple="True" />
                                <asp:Button ID="UploadBtn" runat="server" Text="Upload" class="btn btn-primary" OnClick="UploadBtn_Click" />

                            </div>

                            <hr />

                            <h6>Banner Image List :</h6>
                            <h6><span class="badge rounded-pill bg-success text-dark">* The first image will be the home page banner !</span></h6>
                            <h6>Step1. To upload one image to be the home page banner.</h6>
                            <h6>Step2. Then upload other images.</h6>


                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" class="my-3 mx-auto" AutoPostBack="True" CellPadding="10" RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged"></asp:RadioButtonList>
                            <asp:Button ID="DelImageBtn" runat="server" Text="Delete Image" type="button" class="btn btn-danger btn-sm" OnClientClick="return confirm('Are you sure you want to delete？')" Visible="False" OnClick="DelImageBtn_Click" />


                        </div>
                    </div>
                </div>
                <!-- [ sample-page ] end -->

          
            </div>
            <!-- [ Main Content ] end -->

            <hr />


            <div class="row">

                <%--Add Model start --%>
                <asp:Panel ID="PanelAddModel" runat="server" CssClass="col-sm-4" Visible="true">

                    <div class="card">
                        <div class="card-header">
                            <h5>Add Model</h5>
                        </div>

                        <div class="card-body">

                            <!-- [ Main Content ] start -->

                            <asp:CheckBox ID="CBoxNewDesign" runat="server" Text="NewDesign" Width="50%" />

                            <asp:CheckBox ID="CBoxNewBuilding" runat="server" Text="NewBuilding" Width="50%" CssClass="mt-1"/>

                            <div class="input-group mb-3 mt-3">
                                <asp:TextBox ID="TBoxAddYachtModel" runat="server" type="text" class="form-control" placeholder="Model" Width="30%"></asp:TextBox>
                                <asp:TextBox ID="TBoxAddYachtLength" runat="server" type="text" class="form-control" placeholder="Length"></asp:TextBox>

                                <div class="input-group-append">
                                    <asp:Button ID="BtnAddYacht" runat="server"  Text="Add" class="btn btn-outline-primary btn-block h-100" OnClick="BtnAddYacht_Click" />
                                </div>

                            </div>

      
                        </div>

                    </div>
                </asp:Panel>

                <%-- Add Model end --%>

                <%-- Yacht Model List start --%>
                <asp:Panel ID="PanelYachtModelList" runat="server" CssClass="col-sm-8" Visible="true">

                    <div class="card">
                        <div class="card-header">
                            <h5>Yacht Model List</h5>
                        </div>

                        <div class="card-body">

                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="id"  CssClass="table table-responsive"  OnRowEditing="GridView1_RowEditing" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowUpdating="GridView1_RowUpdating" OnRowDeleting="GridView1_RowDeleting" OnRowDeleted="GridView1_RowDeleted" OnRowUpdated="GridView1_RowUpdated">
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" />
                                    <asp:TemplateField HeaderText="yachtsModel" SortExpression="yachtsModel">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBoxYachtsModel" runat="server" Text='<%# Bind("yachtsModel") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelYachtsModel" runat="server" Text='<%# Bind("yachtsModel") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="isNewDesign" SortExpression="isNewDesign">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBoxIsNewDesignEdit" runat="server" Checked='<%# Bind("isNewDesign") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxIsNewDesign" runat="server" Checked='<%# Bind("isNewDesign") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="isNewBuilding" SortExpression="isNewBuilding">
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="CheckBoxIsNewBuildingEdit" runat="server" Checked='<%# Bind("isNewBuilding") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBoxIsNewBuilding" runat="server" Checked='<%# Bind("isNewBuilding") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="createTime" HeaderText="createTime" SortExpression="createTime" ReadOnly="True" />
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                            </asp:GridView>


                            <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:20240702YachtsConnectionString %>" SelectCommand="SELECT [id], [yachtsModel], [isNewDesign], [isNewBuilding], [createTime] FROM [Yachts]"></asp:SqlDataSource>--%>


                        </div>

                    </div>
                </asp:Panel>

                <%-- Yacht Model List end --%>
            </div>

        </div>
    </div>
    <!-- [ Main Content ] end -->



</asp:Content>
