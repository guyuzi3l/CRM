<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendTransaction.aspx.cs" Inherits="CRM.SendTransaction" MasterPageFile="~/Layout.Master" %>

<asp:Content ContentPlaceHolderID="LayoutHeaderDecorator" runat="server" ClientIDMode="Static">
    <style>
        .roles-container-1 {
            background: #222d32;
            margin-bottom: 15px;
            padding-top: 13px;
            border-bottom: 1px solid #d6d6d6;
            color: #fff;
        }

        .roles-title {
            min-height: 60px;
        }

        .roles-container {
            background: #f7f7f7;
            margin-bottom: 15px;
            padding-top: 30px;
            border-bottom: 1px solid #d6d6d6;
        }

        .roles-title h4 {
            font-size: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" runat="Server" ClientIDMode="Static">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-money"></i></span>
                <span class="page-title-text">Send Transactions</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Send Transactions</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Result Area</h3>
            </div>
            <div class="box-body row">
                <div class="col-md-12">
                    <div class="alert alert-info alert-dismissible">
                        <h4><i class="icon fa fa-info"></i>Result Information</h4>
                        Result: <b><%=SendTransactionResponse %></b>
                    </div>
                </div>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Send Transaction</h3>
            </div>
            <div class="box-body row">
                <form method="post">
                    <div class="col-md-4">
                        <label>Amount:</label>
                        <input type="number" id="amount" name="amount" class="form-control" placeholder="Transaction Amount" step="any" required />
                    </div>
                    <div class="col-md-4">
                        <label>Currency:</label>
                        <select class="form-control" id="currency" name="currency">
                            <option value="BTC" selected>BTC</option>
                            <option value="USD">USD</option>
                            <option value="EUR">EUR</option>
                        </select>
                    </div>
                    <div class="col-md-4 pull-right margin-top-25">
                        <button class="btn btn-success btn-dark">Process</button>
                    </div>
                </form>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
</asp:Content>

