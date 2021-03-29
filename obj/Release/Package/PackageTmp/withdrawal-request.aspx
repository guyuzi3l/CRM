<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="withdrawal-request.aspx.cs" Inherits="CRM.withdrawal_request" MasterPageFile="~/Layout.Master" %>


<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" runat="Server" ClientIDMode="Static">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">View Withdrawals</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">View Documents</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">

        <div class="row">
            <div class="col-md-6">
                <%--  <a href="#" class="btn btn-primary margin-bottom-20" ><i class="fa fa-paper-plane"></i>&nbsp;Create Withdrawal Request</a>--%>
                <a href="#" class="btn btn-success margin-bottom-20" onclick="showSearch();">Toggle Search</a>
            </div>
            <%-- <div class="col-md-6">
                <div class="row">
                    <div class="col-sm-12 col-md-6">
                        <select class="form-control order-input" id="orderby">
                            <%foreach (var key in OrderByDictionary.Keys){  %>
                                <option value="<%=key%>" <%=(key == OrderBy? "selected":"" )%>>Sort by <%=key%></option>
                            <%} %>
                        </select>
                    </div>
                    <div class="col-sm-12 col-md-6">
                        <select class="form-control order-input" id="mode">
                            <option value="desc" <%=("desc" == OrderByMode.ToLower()? "selected":"" )%>>DESC</option>
                            <option value="asc" <%=("asc" == OrderByMode.ToLower()? "selected":"" )%> >ASC</option>
                        </select>
                    </div>
                </div>
            </div>--%>
        </div>

        <br />

        <div class="box box-primary " style="display: none;" id="SearchParameters">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <button type="reset" form="formSearch" class="btn btn-sm btn-info pull-right"><i class="fa fa-eraser"></i>Clear Search</button>
                </div>
                <div class="col-md-12 table-responsive">
                    <form method="POST" id="formSearch" runat="server">
                        <div class="col-md-4">
                            <label>Status:</label>
                            <select id="txtStatus" name="txtStatus" class="form-control" runat="server">
                                <option></option>
                                <option value="Initial">Initial</option>
                                <option value="Approved">Approved</option>
                                <option value="Rejected">Rejected</option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <label>Client ID:</label>
                            <input type="text" id="txtClientId" name="txtClientId" placeholder="Client ID" class="form-control" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <label>Client Email:</label>
                            <input type="text" id="txtClientEmail" name="txtClientEmail" placeholder="Client Email" class="form-control" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <label>Client Name:</label>
                            <input type="text" id="txtClientName" name="txtClientName" placeholder="Client Name" class="form-control" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <label>Wallet ID:</label>
                            <input type="text" id="txtWalletId" name="txtWalletId" placeholder="Wallet Id" class="form-control" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <label>Document ID:</label>
                            <input type="text" id="txtDocumentId" name="txtDocumentId" placeholder="Document Id" class="form-control" runat="server" />
                        </div>
                        <div class="col-md-4">
                            <label>Document Status:</label>
                            <select id="txtDocStatus" name="txtDocStatus" class="form-control" runat="server">
                            </select>
                        </div>
                        <div class="col-md-4">
                            <label>Created Date:</label>
                            <input class="form-control datepicker value-field" name="txtCreatedDate" id="txtCreatedDate" data-date-format="yyyy-mm-dd" placeholder="Created Date" readonly runat="server">
                        </div>
                         <div class="col-md-4">
                            <label>Hash:</label>
                            <input type="text" id="txtHash" name="txtHash" placeholder="Refference Hash" class="form-control" runat="server" />
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
                <div class="col-md-12 table-responsive">
                    <h3 class="box-title">Result -
                        <label id="rowCount" runat="server">0 Record Found</label></h3>
                </div>
            </div>
            <div class="box-body row">
                <div class="col-md-12">
                    <div class="col-md-12 table-responsive" id="tblContainer">
                        <table id="documentsTable" class="table table-hover table-bordered table-striped table-no-wrap">
                            <thead>
                                <tr class="table-row-header" role="row">
                                    <th></th>
                                    <th>Hello Sign Link</th>
                                    <th>Withdrawal Id</th>
                                    <th>Wallet</th>
                                    <th>Client Name</th>
                                    <th>Client Id</th>
                                    <th>Wallet Id / Ref Num</th>
                                    <th>Amount to Send</th>
                                    <th>USD Amount</th>
                                    <th>Reference</th>
                                    <th>Status</th>
                                    <th>Document Status</th>
                                    <th>Actual Amount</th>
                                    <th>Service Fee</th>
                                    <th>Created Date</th>
                                    <th>Signed?</th>
                                    <th>Document</th>
                                    <th>Current BTC Balance</th>
                                    <th>Current EUR Balance</th>
                                    <th>Referral</th>
                                </tr>
                            </thead>
                            <tbody id="resultBody" name="resultBody" runat="server">
                            </tbody>
                        </table>
                        <div class="col-sm-12 col-md-8 col-md-offset-2 text-center">
                            <gwumkt:pagination ID="DocListPager" ActiveCssClass="active" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.box-body -->
        </div>

        <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="statusModal">
            <div class="modal-dialog" role="status">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Change Status</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <form action="#" method="post">
                            <div class="form-group">
                                <select class="form-control" name="selectStatus" id="selectStatus">
                                    <option value="Approved">Approved</option>
                                    <option value="Initial">Initial</option>
                                    <option value="Rejected">Rejected</option>
                                </select>
                                <br />
                                <br />
                                <input type="text" class="form-control" name="reference_hash" id="reference_hash" placeholder="Reference Hash/Number" />
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-info" id="selectStatusBtn">Change Status</button>
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

    </section>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
    <script type="text/javascript">

        function getQueryString() {
            var vars = [];

            // Get the start index of the query string
            var qsi = window.location.href.indexOf('?');
            if (qsi == -1)
                return vars;

            // Get the query string
            var qs = window.location.href.slice(qsi + 1);

            // Check if there is a subsection reference
            sri = qs.indexOf('#');
            if (sri >= 0)
                qs = qs.slice(0, sri);

            // Build the associative array
            var hashes = qs.split('&');
            for (var i = 0; i < hashes.length; i++) {
                var sep = hashes[i].indexOf('=');
                if (sep <= 0)
                    continue;
                var key = decodeURIComponent(hashes[i].slice(0, sep));
                var val = decodeURIComponent(hashes[i].slice(sep + 1));
                vars[key] = val;
            }

            return vars;
        }

        function showSearch() {
            $("#SearchParameters").toggle();
        }

        $('document').ready(function () {

            $('.order-input').change(function () {

                var queryString = getQueryString();
                var mode = $('#mode').val();
                var orderby = $('#orderby').val();

                queryString['orderby'] = orderby;
                queryString['mode'] = mode;

                var queryAsString = "?";

                var keys = Object.keys(queryString);

                keys.forEach(function (k, index) {
                    console.log(queryString[k], k);
                    queryAsString += k + '=' + queryString[k];

                    if ((index + 1) != keys.length)
                        queryAsString += "&";
                });

                console.log(queryAsString);

                location.assign(window.location.href.split('?')[0] + queryAsString);

            });

            $('#clientsTable').DataTable();
            $('.datepicker').datepicker({
                autoclose: true
            });

            $("#tblContainer").floatingScroll();
        });

        $(".class-change-status").on('click', function () {
            console.log('asdasd');
            var d = { id: $(this).data('document-id') };
            $("#statusModal").modal();

            $('#selectStatusBtn').click(function () {

                var postdata = Object.assign({}, d, { selectedStatus: $('#selectStatus').val(), ref_hash: $('#reference_hash').val() });
                $("#statusModal").modal('toggle');

                $("#loader").show();

                $.ajax({
                    type: "POST",
                    url: '/withdrawal-request.aspx/ProcessWithdrawalRequest',
                    data: JSON.stringify(postdata),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        if (res.d.Success === true) {
                            window.location.reload();
                        }
                    },
                    failure: function (res) {
                        console.log(res);
                        $.alert('An error occurred while processing your request.');
                    }
                });
            });

        });

        function viewFile(wdid)
        {
           
            var postdata = Object.assign({}, {id:wdid});
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
