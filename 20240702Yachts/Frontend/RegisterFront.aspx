﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterFront.aspx.cs" Inherits="_20240702Yachts.Frontend.RegisterFront" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>

<!-- [Favicon] icon -->
<link rel="icon" href="../Backend/dist/assets/images/favicon.svg" type="image/x-icon" />
<!-- [Font] Family -->
<link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@300;400;500;600&display=swap" rel="styles0heet" />
<!-- [Tabler Icons] https://tablericons.com -->
<link rel="stylesheet" href="../Backend/dist/assets/fonts/tabler-icons.min.css" />
<!-- [Feather Icons] https://feathericons.com -->
<link rel="stylesheet" href="../Backend/dist/assets/fonts/feather.css" />
<!-- [Font Awesome Icons] https://fontawesome.com/icons -->
<link rel="stylesheet" href="../Backend/dist/assets/fonts/fontawesome.css" />
<!-- [Material Icons] https://fonts.google.com/icons -->
<link rel="stylesheet" href="../Backend/dist/assets/fonts/material.css" />
<!-- [Template CSS Files] -->
<link rel="stylesheet" href="../Backend/dist/assets/css/style.css" id="main-style-link" />
<link rel="stylesheet" href="../Backend/dist/assets/css/style-preset.css" />
<!-- Google tag (gtag.js) -->
<%--<script async src="https://www.googletagmanager.com/gtag/js?id=G-14K1GBX9FG"></script>--%>
<%--<script>
    window.dataLayer = window.dataLayer || [];
    function gtag() {
        dataLayer.push(arguments);
    }
    gtag('js', new Date());

    gtag('config', 'G-14K1GBX9FG');
</script>--%>
<!-- WiserNotify -->
<script>
    !(function () {
        if (window.t4hto4) console.log('WiserNotify pixel installed multiple time in this page');
        else {
            window.t4hto4 = !0;
            var t = document,
                e = window,
                n = function () {
                    var e = t.createElement('script');
                    (e.type = 'text/javascript'),
                        (e.async = !0),
                        (e.src = 'https://pt.wisernotify.com/pixel.js?ti=1jclj6jkfc4hhry'),
                        document.body.appendChild(e);
                };
            'complete' === t.readyState ? n() : window.attachEvent ? e.attachEvent('onload', n) : e.addEventListener('load', n, !1);
        }
    })();
</script>
<!-- Microsoft clarity -->
<script type="text/javascript">
    (function (c, l, a, r, i, t, y) {
        c[a] =
            c[a] ||
            function () {
                (c[a].q = c[a].q || []).push(arguments);
            };
        t = l.createElement(r);
        t.async = 1;
        t.src = 'https://www.clarity.ms/tag/' + i;
        y = l.getElementsByTagName(r)[0];
        y.parentNode.insertBefore(t, y);
    })(window, document, 'clarity', 'script', 'gkn6wuhrtb');
</script>







<body>
    <form id="form1" runat="server">
        <div>

            <!-- [ Pre-loader ] start -->
            <div class="loader-bg">
                <div class="loader-track">
                    <div class="loader-fill"></div>
                </div>
            </div>
            <!-- [ Pre-loader ] End -->

            <div class="auth-main">
                <div class="auth-wrapper v1">
                    <div class="auth-form">
                        <div class="position-relative my-5">
                            <div class="auth-bg">
                                <span class="r"></span>
                                <span class="r s"></span>
                                <span class="r s"></span>
                                <span class="r"></span>
                            </div>
                            <div class="card mb-0">
                                <div class="card-body">
                                    <div class="text-center">
                                        <a href="#">
                                            <%--<img src="../assets/images/logo-dark.svg" alt="img" />--%>
                                            <img src="../Backend/dist/assets/images/logo.svg" style="width: 265px;" alt="img" />
                                        </a>
                                    </div>
                                    <h4 class="text-center f-w-500 mt-4 mb-3">Sign up</h4>
                                    <%--<div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group mb-3">
                                                <input type="text" class="form-control" placeholder="First Name" />
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group mb-3">
                                                <input type="text" class="form-control" placeholder="Last Name" />
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%-- Name --%>
                                    <%--要新增的 User 資料--%>
                                    <asp:Label ID="LabelCreateName" CssClass="col-form-label" runat="server" Text="Name"></asp:Label>
                                    <asp:TextBox ID="TextBoxCreateName" CssClass="form-control mb-3" runat="server" Height="24px"></asp:TextBox>

                                    <asp:Label ID="LabelCreateAccount" CssClass="col-form-label" runat="server" Text="Account"></asp:Label>
                                    <asp:TextBox ID="TextBoxCreateAccount" CssClass="form-control mb-3" runat="server" Height="24px"></asp:TextBox>

                                    <asp:Label ID="LabelCreatePassword" CssClass="col-form-label" runat="server" Text="Password"></asp:Label>
                                    <asp:TextBox ID="TextBoxCreatePassword" CssClass="form-control mb-3" runat="server" Height="24px"></asp:TextBox>

                                    <asp:Label ID="LabelCreateEmail" CssClass="col-form-label" runat="server" Text="Email"></asp:Label>
                                    <asp:TextBox ID="TextBoxCreateEmail" CssClass="form-control mb-3" runat="server" Height="24px"></asp:TextBox>


                                    <%--<div class="form-group mb-3">
                                        <input type="email" class="form-control" placeholder="Email Address" />
                                    </div>
                                    <div class="form-group mb-3">
                                        <input type="password" class="form-control" placeholder="Password" />
                                    </div>
                                    <div class="form-group mb-3">
                                        <input type="password" class="form-control" placeholder="Confirm Password" />
                                    </div>--%>
                                    <div class="d-flex mt-1 justify-content-between">
                                        <div class="form-check">
                                            <input class="form-check-input input-primary" type="checkbox" id="customCheckc1" checked="" />
                                            <label class="form-check-label text-muted" for="customCheckc1">I agree to all the Terms & Condition</label>
                                        </div>
                                    </div>
                                    <div class="text-center mt-4">
                                        <%--<button type="button" class="btn btn-primary shadow px-sm-4">Sign up</button>--%>
                                        <asp:Button ID="ButtonSignUp" CssClass="btn btn-primary shadow px-sm-4" runat="server" Text="Sign up" OnClick="ButtonSignUp_Click" />
                                    </div>
                                    <div class="d-flex justify-content-between align-items-end mt-4">
                                        <h6 class="f-w-500 mb-0">Already have an Account?</h6>
                                        <a href="./LoginFront.aspx" class="link-primary">Login</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- [ Main Content ] end -->
            <!-- [ Main Content ] end -->


        </div>
    </form>
</body>

<!-- Required Js -->
<script src="../Backend/dist/assets/js/plugins/popper.min.js"></script>
<script src="../Backend/dist/assets/js/plugins/simplebar.min.js"></script>
<script src="../Backend/dist/assets/js/plugins/bootstrap.min.js"></script>
<script src="../Backend/dist/assets/js/fonts/custom-font.js"></script>
<script src="../Backend/dist/assets/js/pcoded.js"></script>
<script src="../Backend/dist/assets/js/plugins/feather.min.js"></script>

</html>
