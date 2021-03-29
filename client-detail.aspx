<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="client-detail.aspx.cs" Inherits="CRM.client_detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LMainContent" runat="server">
    
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Client Details</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="">View Clients</li>
            <li class="active">Client Details</li>
        </ol>
        <hr class="hr-new" />
        <a href="client-detail.aspx">client-detail.aspx</a>
    </section>
    <!--End of Page Title-->
    
    <section class="content margin-top-25" style="margin-top:-20px;">
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12">
                <a href="client-lists.aspx"><i class="fa fa-angle-double-left" aria-hidden="true"></i> Back to client list</a>
                <div class="margin-top-20">
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a href="#client-info" aria-controls="client-info" role="tab" data-toggle="tab">Client Information</a></li>
                        <li role="presentation"><a href="#client-docs" aria-controls="client-docs" role="tab" data-toggle="tab">Client Documents</a></li>
                        <li role="presentation"><a href="#client-transactions" aria-controls="client-transactions" role="tab" data-toggle="tab">Client Transactions</a></li>
                        <li role="presentation"><a href="#client-withdrawals" aria-controls="client-withdrawals" role="tab" data-toggle="tab">Client Withdrawals</a></li>
                    </ul>

                    <br />

                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="client-info">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Client Information</h3>
                                     <label class="pull-right col-xs-offset-3" style="padding-right:50px" for="c_first_name">Current BTC Balance : <i class="fa fa-btc" aria-hidden="true"></i>  <%=BtcBalance.ToString() %> || Current EUR Balance : <i class="fa fa-eur"></i>  <%=EurBalance.ToString() %> </label>
                                </div>
                                <div class="box-body">
                                    <label for="c_first_name">Clients ID - <%=Client.Id%> </label>
                                    <a href="#" class="btn btn-primary btn-sm pull-right" id="c_edit_control_switch">Update</a>
                                    <br style="clear:both;"/>

                                    <form runat="server" id="detailForm" method="post">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="c_first_name">First name</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_first_name" id="c_first_name" value="<%=Client.First_name%>" />
                                                    <input type="hidden" id="id" name="id" value="<%=Client.Id%>" />
                                                </div>
                                                <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="c_last_name">Last name</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_last_name" id="c_last_name" value="<%=Client.Last_name%>"/>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="form-group" id="emailid" hidden>
                                            <label for="c_email_add">Email address</label>
                                            <input type="email" required class="form-control cd-edit-control" name="c_email_add" id="c_email_add" value="<%=Client.Email%>"/>
                                        </div>
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="c_phone_prefix">Phone Prefix</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_phone_prefix" id="c_phone_prefix" value="<%=Client.Phone_prefix%>"/>
                                                </div>
                                                <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="c_phone_number">Phone number</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_phone_number" id="c_phone_number" value="<%=Client.Phone_number%>"/>
                                                </div>
                                            </div>
                                        </div>

                                         <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="dob">Date of Birth</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_dob" id="c_dob" value="<%=Client.dob%>"/>
                                                </div>
                                                 <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="client_ip">Client IP</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_actual_client_ip" id="c_actual_client_ip" value="<%=Client.actual_client_ip%>"/>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-6 col-lg-6">
                                                    <label for="c_country">Country</label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_country" id="c_country" value="<%=Client.Country%>"/>
                                                </div>
                                                <div class="col-sm-12 col-md-6 col-lg-6" id="maxDepoDiv">
                                                    <label for="c_max_deposit">Max Deposit&nbsp;<a href="#" onclick="$('#maxDepoDiv').hide();"><i class="fa fa-eye-slash"></i></a></label>
                                                    <input type="text" required class="form-control cd-edit-control" name="c_max_deposit" id="c_max_deposit" value="<%=Client.Max_deposit%>"/>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12 col-md-6 col-lg-6" id="refDiv">
                                                    <label for="c_referral">Referral&nbsp;<a href="#" onclick="$('#refDiv').hide();"><i class="fa fa-eye-slash"></i></a></label>
                                                    <input type="text"  class="form-control cd-edit-control" name="c_referral" id="c_referral" value="<%=Client.Referral%>"/>
                                                </div>
                                            </div>
                                        </div>


                                        <div class="form-group hidden cd-edit-display-control">
                                            <button type="submit" id="btnsave" class="btn btn-primary pull-right">Save</button>
                                            <span class="pull-right">&nbsp;</span>
                                            <button type="button" class="btn btn-danger pull-right" onclick="location.reload()">Cancel</button>
                                        </div>

                                    </form>
            
                                </div>
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="client-docs">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Client Documents</h3>
                                </div>
                                <div class="box-body box-body-scrollable">
                                    <table id="documentsTable" class="table table-hover table-bordered table-striped">
                                        <thead>
                                            <tr class="table-row-header" role="row">
                                                <th>ID</th>
                                                <th>Type</th>
                                                <th>Status</th>
                                                <th>Created Date</th>
                                                <th>Updated Date</th>
                                                <th>Expiry Date</th>
                                                <th>Subtype</th>
                                                <th>Note</th>
                                                <th>CardLastFour</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody id="documentsBody" name="documentsBody" runat="server">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>


                        <div role="tabpanel" class="tab-pane" id="client-transactions">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Client Transactions</h3>
                                </div>
                                <div class="box-body box-body-scrollable">
                                     <div class="scrollable-x-wrapper table-responsive">
                                        <table id="transactionTable" class="table table-hover table-bordered table-striped gbc-table-cell-nowrap">
                                            <thead>
                                                <tr class="table-row-header" role="row">
                                                    <th class="hideable" style="text-align:center"><a href="#" onclick="hideCol('transactionTable','hideable');"><i class="fa fa-eye-slash" style="color:black; text-align:center;"></i></a></th>
                                                    <th>CLIENT ID</th>
                                                    <th>PAYMENT REFERRENCE</th>
                                                    <th>PSP</th>
                                                    <th>CLIENT NAME</th>
                                                    <th>DEPOSIT CURRENCY</th>
                                                    <th>DEPOSIT AMOUNT</th>
                                                    <th>CURRENCY</th>
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
                                </div>
                            </div>
                        </div>

                        <div role="tabpanel" class="tab-pane" id="client-withdrawals">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Client Withdrawals</h3>
                                    <label class="pull-right col-xs-offset-3" style="padding-right:50px" for="c_first_name">Current BTC Balance : <i class="fa fa-btc" aria-hidden="true"></i>  <%=BtcBalance.ToString() %> || Current EUR Balance : <i class="fa fa-eur"></i>  <%=EurBalance.ToString() %> </label>
                                </div>
                                <div class="box-body box-body-scrollable">
                                     <div class="scrollable-x-wrapper table-responsive">
                                        <table id="withdrawalTable" class="table table-hover table-bordered table-striped gbc-table-cell-nowrap">
                                            <thead>
                                                <tr class="table-row-header" role="row">
                                                    <th class="hideable-1" style="text-align:center"><a href="#" onclick="hideCol('withdrawalTable','hideable-1');"><i class="fa fa-eye-slash" style="color:black; text-align:center;"></i></a></th>
                                                    <th class="hideable-2">Hello Sign Link &nbsp;<a href="#" onclick="hideCol('withdrawalTable','hideable-2');"><i class="fa fa-eye-slash" style="color:black; text-align:center;"></i></a></th>
                                                    <th>Id</th>
                                                    <th>Wallet</th>
                                                    <th>Client Name</th>
                                                    <th>Client Id</th>
                                                    <th class="hideable-3">Wallet Id / Ref Num &nbsp;<a href="#" onclick="hideCol('withdrawalTable','hideable-3');"><i class="fa fa-eye-slash" style="color:black; text-align:center;"></i></a></th>
                                                    <th>Amount to Send</th>
                                                    <th>USD Amount</th>
                                                    <th>Status</th>
                                                    <th>Document Status</th>
                                                    <th>Actual Amount</th>
                                                    <th>Referrence</th>
                                                    <th>Service Fee</th>
                                                    <th>Created Date</th>
                                                    <th class="hideable-4">Is Sign &nbsp;<a href="#" onclick="hideCol('withdrawalTable','hideable-4');"><i class="fa fa-eye-slash" style="color:black; text-align:center;"></i></a></th>
                                                    <th class="hideable-5">Document File &nbsp;<a href="#" onclick="hideCol('withdrawalTable','hideable-5');"><i class="fa fa-eye-slash" style="color:black; text-align:center;"></i></a></th>
                                                </tr>
                                            </thead>
                                            <tbody id="witdrawalBody" name="witdrawalBody" runat="server">
                                            </tbody>
                                        </table>
                                     </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
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

    <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="wdEditPayRefModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Edit Referrence</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form action="#" method="post">
                        <div class="form-group">
                           <input type="text" class="form-control" name="reference_hash" id="reference_hash" placeholder="Reference Hash/Number"/>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" id="UpdateWDReferrence">Update</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>


    <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="txEditNoteRefModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Edit Note and Referrence</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>Note: if you leave the fields empty it will just take the old note/referrence value</p>
                    <br />
                    <form action="#" method="post">
                        <div class="form-group">
                           <input type="text" class="form-control" name="note" id="note" placeholder="Transaction Note"/>
                            <br />
                           <input type="text" class="form-control" name="reff" id="reff" placeholder="Transaction Referrence"/>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" id="EditNoteRefBtn">Update</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

     <!-- Modal Document Sign-->
        <div class="modal fade" id="fileModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle">Document</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" id="modal-docu-id">
                         <iframe frameborder="0" id="htmlPreviewData" width="100%" onload="this.height=screen.height;"></iframe>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="LayoutHeaderDecorator" runat="server">
    <style type="text/css">
        .box-body-scrollable {
            max-height:500px; 
            overflow:auto;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="LayoutScriptDecorator" runat="server">

    <script type="text/javascript" src="script/gbtc-transaction-script.js"></script>

    <script type="text/javascript">
        
        $(document).ready(function () {

            toastr.options = {
                "positionClass": "toast-top-right"
            };

            <% if(IsUpdateSuccess == true) { %>

                var tid = setTimeout(function () {
                    toastr.success('Client Detail successfully updated!');
                }, 1000);


            <% } %>

            $('.cd-edit-control').prop('readonly', true);
            $('.cd-edit-display-control').addClass('hidden');

            $('#c_edit_control_switch').click(function () {
                $(this).remove();
                $('.cd-edit-control').prop('readonly', false);
                $('.cd-edit-display-control').removeClass('hidden');
            });
        });
        $('#c_edit_control_switch').click(function () {
            $('#emailid').toggle();
        });
        $('#btnsave').click(function (e){
            if ($('#c_first_name').val() == "" || $('#c_last_name').val() == "" || $('#c_email_add').val() == "" || $('#c_phone_prefix').val() == "" || $('#c_phone_number').val() == ""){
                toastr.error("Please fill up all fields");
                e.preventDefault();
            }
        });

        function hideCol(tablename, classname) {
            var column = "#" + tablename + " ." + classname;
            $(column).hide();
        }


        function viewFile(wdid) {

            var postdata = Object.assign({}, { id: wdid });
            $.ajax({
                type: "POST",
                url: '/withdrawal-request.aspx/GetDocuFile',
                data: JSON.stringify(postdata),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d != '') {
                        const html = res.d;
                        console.log(res.d);
                        var iframe = document.getElementById('htmlPreviewData'),
                            iframedoc = iframe.contentDocument || iframe.contentWindow.document;
                        iframedoc.body.innerHTML = html;

                        $('#fileModal').modal('toggle');
                    }
                    else {
                        console.log(res);
                        $.alert('An error occurred while processing your request.');
                    }
                },
                failure: function (res) {
                    console.log(res);
                    $.alert('An error occurred while processing your request.');
                }
            });

        }

    </script>
</asp:Content>

