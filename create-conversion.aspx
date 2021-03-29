<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="create-conversion.aspx.cs" Inherits="CRM.create_conversion" MasterPageFile="~/Layout.Master" %>

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
                <span class="page-title-text">Conversion</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Create Conversion</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->

    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <a href="/conversion-request.aspx" class="btn btn-primary margin-bottom-20" ><i class="fa fa-caret-left"></i>&nbsp;Back to Conversion Request List</a>
        <div class="box box-primary">
            <div class="box-header with-border">
                    <h3 class="box-title">Create Conversion Request</h3>
             </div>
            <div class="box-body row">
                <form method="post">
                   <div class="col-md-4">
                        <label>Client Id:</label>
                        <input type="number" id="clientId" name="clientId" class="form-control" placeholder="Client Id"  required/>
                    </div>
                   <div class="col-md-4">
                        <label>From Currency:</label>
                        <select class="form-control" id="from_currency" name="from_currency">
                            <option value="BTC" selected>BTC</option>
                            <option value="EUR">EUR</option>
                        </select>
                    </div>
                    <div class="col-md-4">
                        <label>To Currency:</label>
                        <select class="form-control" id="to_currency" name="to_currency">
                            <option value="BTC">BTC</option>
                            <option value="EUR" selected>EUR</option>
                        </select>
                    </div>
                    <div class="col-md-4 margin-top-20">
                        <label>Status:</label>
                        <select class="form-control" id="credited_status" name="credited_status">
                            <option value="Initial" selected>Initial</option>
                            <option value="Approved">Approved</option>
                             <option value="Rejected">Rejected</option>
                        </select>
                    </div>
                   <div class="col-md-4 margin-top-20">
                        <label>Amount:</label>
                        <input type="number" id="amount" name="amount" class="form-control" placeholder="Desired Amount" step="any" required/>
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
