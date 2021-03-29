<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transaction-lists.aspx.cs" Inherits="CRM.transaction_lists" MasterPageFile="~/Layout.Master" EnableViewState="true" %>


<asp:Content ID="Transaction" ContentPlaceHolderID="LMainContent" runat="server">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">View Transactions</span>
            </i>
        </div>

        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
        </ol>

        <hr class="hr-new" />
    </section>
    <!-- End of Start Page-->

    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <a href="/CreateTransaction.aspx" class="btn btn-primary margin-bottom-20" ><i class="fa fa-paper-plane"></i>&nbsp;Create Transaction</a>
        <a href="#" class="btn btn-success margin-bottom-20" onclick="showSearch();">Toggle Search</a>
        <div class="box box-primary " style="display: none;" id="SearchParameters">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <button type="reset" form="formSearch" class="btn btn-sm btn-info pull-right"><i class="fa fa-eraser"></i> Clear Search</button>
                </div>
                <div class="col-md-12 table-responsive">
                    <form method="get" id="formSearch">
                        <div class="col-md-4">
                            <label>PSP:</label>
                            <select id="pspName" name="pspName" class="form-control" runat="server">
                            </select>
                        </div>
                        <div class="col-md-4">
                            <label>PSP Status:</label>
                            <select id="pspStatus" name="pspStatus" class="form-control">
                                <option></option>
                                <option value="Pending">Pending</option>
                                <option value="Approved">Approved</option>
                                <option value="Rejected">Rejected</option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <label>CREDITED Status:</label>
                            <select id="creditedStatus" name="creditedStatus" class="form-control">
                                <option></option>
                                <option value="Credited">Credited</option>
                                <option value="NotCredited">Not Credited</option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <label>Client ID:</label>
                            <input type="text" id="txtClientId" name="txtClientId" placeholder="Client ID" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Client Email:</label>
                            <input type="text" id="txtClientEmail" name="txtClientEmail" placeholder="Client Email" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Client Name:</label>
                            <input type="text" id="txtClientName" name="txtClientName" placeholder="Client Name" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Referral:</label>
                            <input type="text" id="txtReferral" name="txtReferral" placeholder="Referral" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Created Date:</label>
                            <input class="form-control datepicker value-field" name="txtCreatedDate" id="txtCreatedDate" data-date-format="yyyy-mm-dd" placeholder="Created Date">
                        </div>
                        <div class="col-md-12 text-center margin-top-20">
                            <button type="submit" class="btn btn-info">Search</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
        <div class="box box-primary">
            <!-- /.box-header -->
            <!-- form start -->
            <div class="box-body row">
                <div class="col-md-12">
                    <div class="scrollable-x-wrapper table-responsive">
                        <table id="clientsTable" class="table table-hover table-bordered table-striped gbc-table-cell-nowrap">
                            <thead>
                                <tr class="table-row-header" role="row">
                                    <th></th>
                                    <th>CLIENT ID</th>
                                    <th>PAYMENT REFERRENCE</th>
                                    <th>PSP</th>
                                    <th>CLIENT NAME</th>
                                    <th>REFERRAL</th>
                                    <th>DEPOSIT CURRENCY</th>
                                    <th>DEPOSIT AMOUNT</th>
                                    <th>EXCHANGE CURRENCY</th>
                                    <th>EXCHANGE AMOUNT</th>
                                    <th>Cardholder Name</th>
                                    <th>CARD LAST4</th>
                                    <th>PSP STATUS</th>
                                    <th>Notes</th>
                                    <th>Transaction Type</th>
                                    <th>DATE</th>
                                </tr>
                            </thead>
                            <tbody id="transactionBody" name="transactionBody" runat="server">
                            </tbody>
                        </table>
                    </div>
                    <div class="col-sm-12 col-md-8 col-md-offset-2 text-center">
                        <gwumkt:pagination ID="TransactionListPager" ActiveCssClass="active" runat="server" />
                    </div>
                </div>
            </div>
            <!-- /.box-body -->
        </div>
    </section>

    <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="pspModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Change Transaction Status</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form action="#" method="post">
                        <div class="form-group">
                            <select class="form-control" name="selectPspStatus" id="selectPspStatus">
                                <option value="Approved">Approved</option>
                                <option value="Pending">Pending</option>
                                <option value="Rejected">Rejected</option>
                            </select>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" id="selectPspStatusBtn">Change Status</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="credTransactionModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Change Credit Transaction Status</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form action="#" method="post">
                        <div class="form-group">
                            <select class="form-control" name="creditedStatusSelected" id="creditedStatusSelected">
                                <option value="Credited">Credited</option>
                                <option value="NotCredited">Not Credited</option>
                            </select>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" id="selectCredStatusBtn">Change Status</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="LayoutScriptDecorator" runat="server">

    <script type="text/javascript" src="script/gbtc-transaction-script.js"></script>
    
    <script type="text/javascript">
        function showSearch() {
            $("#SearchParameters").toggle();
        }
    </script>
</asp:Content>
