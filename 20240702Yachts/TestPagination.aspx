<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPagination.aspx.cs" Inherits="_20240702Yachts.TestPagination" %>

<%@ Register Src="~/Pagination.ascx" TagPrefix="uc1" TagName="Pagination" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <style>
        div#pagination {
            height: 50px;
            margin-top: 3px;
        }

            div#pagination .count {
                float: left;
                padding: 5px;
            }

            div#pagination .pages {
                float: right;
                padding: 5px;
            }

        div#paginationTop .count {
            float: left;
            padding: 5px;
        }

        div#paginationTop .pages {
            float: right;
            padding: 5px;
        }

        div.pagination {
            padding: 0px;
            margin: 0px;
        }

            div.pagination a {
                padding: 2px 5px 2px 5px;
                margin: 2px;
                border: 1px solid #8dab68;
                text-decoration: none; /* no underline */
                color: #5f7f39;
            }

                div.pagination a:hover, div.pagination a:active {
                    border: 1px solid #5f7f39;
                    color: #000;
                }

            div.pagination span.current {
                padding: 2px 5px 2px 5px;
                margin: 2px;
                border: 1px solid #5f7f39;
                font-weight: bold;
                background-color: #5f7f39;
                color: #FFF;
            }

            div.pagination span.disabled {
                padding: 2px 5px 2px 5px;
                margin: 2px;
                border: 1px solid #EEE;
                color: #DDD;
            }

        .bold14 {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 14px;
            font-weight: bold;
        }

        .rederror {
            color: red;
        }
    </style>

</head>

<link rel="stylesheet" href="./Css/pagination.css" />

<body>
    <form id="form1" runat="server">
        <div>

            <asp:Literal ID="LiteralTest" runat="server"></asp:Literal>
            <uc1:Pagination runat="server" ID="Pagination" />

        </div>
    </form>
</body>
</html>
