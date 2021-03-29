<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="conversion-request.aspx.cs" Inherits="CRM.conversion_request" MasterPageFile="~/Layout.Master" %>


<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" runat="Server" ClientIDMode="Static">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Conversion Request</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Conversion Request</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <a href="/create-conversion.aspx" class="btn btn-primary margin-bottom-20" ><i class="fa fa-paper-plane"></i>&nbsp;Create Conversion Request</a>
           <div class="box box-primary">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <button id ="btnReset" class="btn btn-sm btn-info pull-right" onclick="ReloadForm();"><i class="fa fa-eraser"></i>Clear Search</button>
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
                            <label>Created Date:</label>
                            <input class="form-control datepicker value-field" name="txtCreatedDate" id="txtCreatedDate" data-date-format="yyyy-mm-dd" placeholder="Created Date" readonly runat="server">
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
                                    <th>ID</th>
                                    <th>Client ID</th>
                                    <th>Client Name</th>
                                    <th>From Amount</th>
                                    <th>From Currency</th>
                                    <th>Status</th>
                                    <th>To Amount</th>
                                    <th>To Currency</th>
                                    <th>Created Date</th>
                                    <th>Current BTC BALANCE</th>
                                    <th>Current EUR BALANCE</th>
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
    </section>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
    <script>

        function ReloadForm()
        {
            location.reload();
        }

         $(".class-change-status").on('click', function () {
            console.log('asdasd');
            var d = { id: $(this).data('document-id') };
            $("#statusModal").modal();
             console.log(d);

            $('#selectStatusBtn').click(function () {

                var postdata = Object.assign({}, d, { selectedStatus: $('#selectStatus').val()});
                $("#statusModal").modal('toggle');

                $("#loader").show();

                $.ajax({
                    type: "POST",
                    url: '/conversion-request.aspx/UpdateConversionRequest',
                    data: JSON.stringify(postdata),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (res) {
                        console.log(res);
                        toastr["success"](res);
                        setTimeout(function () {
                            window.location.reload();
                        }, 3000);
                    },
                    failure: function (res) {
                        console.log(res);
                        $.alert('An error occurred while processing your request.');
                    }
                });
            });

        });

         $('#clientsTable').DataTable();
            $('.datepicker').datepicker({
                autoclose: true
            });

        $("#tblContainer").floatingScroll();
    </script>
</asp:Content>
