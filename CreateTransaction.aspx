<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateTransaction.aspx.cs" Inherits="CRM.CreateTransaction" MasterPageFile="~/Layout.Master" %>

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
                <span class="page-title-text">Transactions</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Create Transactions</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <a href="/transaction-lists.aspx" class="btn btn-primary margin-bottom-20" ><i class="fa fa-caret-left"></i>&nbsp;Back to Transaction List</a>
        <div class="box box-primary">
            <div class="box-header with-border">
                    <h3 class="box-title">Add Transaction</h3>
             </div>
            <div class="box-body row">
                <form method="post">
                   <div class="col-md-4">
                        <label>Client Id:</label>
                        <input type="number" id="clientId" name="clientId" class="form-control" placeholder="Client Id"  required/>
                    </div>
                    <div class="col-md-4">
                        <label>Payment Referrence:</label>
                        <input type="text" id="pspRef" name="pspRef" class="form-control" placeholder="Transaction Payment Referrence" required/>
                   </div>
                    <div class="col-md-4">
                        <label>Payment Status:</label>
                        <select id="pspStatus" name="pspStatus" class="form-control" >
                            <option value="Initial" selected>Initial</option>
                            <option value="Approved">Approved</option>
                            <option value="Rejected">Rejected</option>
                        </select>
                   </div>
                      <div class="col-md-4 margin-top-20">
                            <label>Credited Status:</label>
                            <select id="creditedStatus" name="creditedStatus" class="form-control">
                                <option value="Credited" selected>Credited</option>
                                <option value="NotCredited">Not Credited</option>
                            </select>
                        </div>
                    <div class="col-md-4 margin-top-20">
                        <label>Payment Processor:</label>
                        <select id="pspId" name="pspId" class="form-control" runat="server">
                        </select>
                   </div>
                    <div class="col-md-4 margin-top-20">
                        <label>Amount:</label>
                        <input type="number" id="amount" name="amount" class="form-control" placeholder="Transaction Amount" step="any" required/>
                    </div>
                    <div class="col-md-4 margin-top-20">
                        <label>Currency:</label>
                        <select class="form-control" id="currency" name="currency">
                            <option value="USD" selected>USD</option>
                            <option value="EUR">EUR</option>
                            <option value="GBP">GBP</option>
                            <option value="AUD">AUD</option>
                        </select>
                    </div>
                    <div class="col-md-4 margin-top-20">
                        <label>Note:</label>
                        <input type="text" id="note" name="note" class="form-control" placeholder="Transaction Note" required/>
                    </div>
                    <div class="col-md-4 margin-top-20">
                        <label>PIN/TradingAccountId:</label>
                        <input type="text" id="PIN" name="PIN" class="form-control" placeholder="Client's Trading Account Id" />
                    </div>
                     <div class="col-md-4 margin-top-20">
                        <label>Card Holdername:</label>
                        <input type="text" id="cardHolder" name="cardHolder" class="form-control" placeholder="Card Holder Name" />
                   </div>
                     <div class="col-md-4 margin-top-20">
                        <label>Card Last 4:</label>
                        <input type="text" id="cardLast4" name="cardLast4" class="form-control" placeholder="Card Last 4 Digits" />
                   </div>
                    <div class="col-md-4 margin-top-20">
                        <label>Card Expiry:</label>
                        <input type="text" id="cardExpiry" name="cardExpiry" class="form-control" placeholder="Card Expiry" />
                   </div>
                    <div class="col-md-12 pull-right margin-top-20">
                        <button class="btn btn-info btn-dark">Submit</button>
                    </div>
                </form>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
</asp:Content>
