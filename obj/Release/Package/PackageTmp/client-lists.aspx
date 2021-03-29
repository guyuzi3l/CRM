<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="client-lists.aspx.cs" Inherits="CRM.client_lists" MasterPageFile="~/Layout.Master" %>

<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" Runat="Server">

    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">View Clients</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">View Clients</li>
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
                            <label>Client ID:</label>
                            <input type="text" id="clientId" name="clientId" placeholder="Client ID" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Phone Number:</label>
                            <input type="text" id="phonenumber" name="phonenumber" placeholder="Phone Number" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Client Email:</label>
                            <input type="text" id="txtClientEmail" name="txtClientEmail" placeholder="Client Email" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>First Name:</label>
                            <input type="text" id="firstname" name="firstname" placeholder="First Name" class="form-control" />
                        </div>
                        <div class="col-md-4">
                            <label>Last Name:</label>
                            <input type="text" id="lastname" name="lastname" placeholder="Last Name" class="form-control" />
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
        <form method="post">
            <div class="box box-primary">
                <!-- /.box-header -->
                <!-- form start -->
                <div class="box-body row">
                    <div class="col-md-12 table-responsive">
                        <table id="clientsTable" class="table table-hover table-bordered table-striped">
                            <thead>
                                <tr class="table-row-header" role="row">
                                    <th>Client ID</th>
                                    <th>NAME</th>
                                    <th>EMAIL</th>
                                    <th>PHONE NUMBER</th>
                                    <th>Phone Verification</th>
                                    <th>Referral</th>
                                    <th>IP</th>
                                    <th>CREATED DATE</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="clientsBody" name="clientsBody" runat="server">
                            </tbody>
                        </table>
                        <%if (SearchHasResult == true) { %>
                            <div class="col-sm-12 col-md-8 col-md-offset-2 text-center">
                                <gwumkt:pagination ID="ClientListPager" ActiveCssClass="active" runat="server" />    
                            </div>
                        <%} %>
                    </div>
                </div>
                <!-- /.box-body -->
            </div>
        </form>
    </section>
    <!--End of Page Content-->

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" Runat="Server">
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

        $(document).ready(function () {

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

            $('.datepicker').datepicker({
                format: 'yyyy-mm-dd'
            });
        });
    </script>
       <script type="text/javascript">
           function showSearch() {
               $("#SearchParameters").toggle();
           }
    </script>
</asp:Content>