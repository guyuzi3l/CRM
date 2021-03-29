<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="document-lists.aspx.cs" Inherits="CRM.document_lists" MasterPageFile="~/Layout.Master" %>

<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" runat="Server">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">View Documents</span>
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
                <a href="#" class="btn btn-success margin-bottom-20" onclick="showSearch();">Toggle Search</a>
            </div>
            <div class="col-md-6">
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
            </div>
        </div>

        <br />

        <div class="box box-primary " style="display: none;" id="SearchParameters">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <button type="reset" form="formSearch" class="btn btn-sm btn-info pull-right"><i class="fa fa-eraser"></i> Clear Search</button>
                </div>
                <div class="col-md-12 table-responsive">
                    <form method="get" id="formSearch">
                        <div class="col-md-4">
                            <label>Type:</label>
                            <select id="txtType" name="txtType" class="form-control">
                                <option></option>
                                <option value="Proof of ID">Proof of ID</option>
                                <option value="Proof of Residence">Proof of Residence</option>
                                <option value="Credit Card">Credit Card</option>
                            </select>
                        </div>
                        <div class="col-md-4">
                            <label>Status:</label>
                            <select id="txtStatus" name="txtStatus" class="form-control">
                                <option></option>
                                <option value="Initial">Initial</option>
                                <option value="Approved">Approved</option>
                                <option value="Rejected">Rejected</option>
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
                            <label>Created Date:</label>
                            <input class="form-control datepicker value-field" name="txtCreatedDate" id="txtCreatedDate" data-date-format="yyyy-mm-dd" placeholder="Created Date" readonly>
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
                <div class="col-md-12 table-responsive">
                    <a href="/upload-docu.aspx" class="btn btn-primary margin-bottom-20" ><i class="fa fa-paper-plane"></i>&nbsp;Upload Document</a>
                    <table id="documentsTable" class="table table-hover table-bordered table-striped">
                        <thead>
                            <tr class="table-row-header" role="row">
                                <th>ID</th>
                                <th>Client ID</th>
                                <th>Client Name</th>
                                <th>Type</th>
                                <th>Status</th>
                                <th>Created Date</th>
                                <th>Updated Date</th>
                                <th>Expiry Date</th>
                                <th>Subtype</th>
                                <th>Note</th>
                                <th>File</th>
                                <th>CardLastFour</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody id="documentsBody" name="documentsBody" runat="server">
                        </tbody>
                    </table>

                    <div class="col-sm-12 col-md-8 col-md-offset-2 text-center">
                        <gwumkt:pagination ID="DocListPager" ActiveCssClass="active" runat="server" />
                    </div>
                </div>
            </div>
            <!-- /.box-body -->
        </div>

    </section>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server">
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

                keys.forEach(function (k,index) {
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
        });
    </script>
</asp:Content>


