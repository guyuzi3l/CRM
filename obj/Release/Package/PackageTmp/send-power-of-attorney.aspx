<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="send-power-of-attorney.aspx.cs" Inherits="CRM.send_power_of_attorney" MasterPageFile="~/Layout.Master" %>

<asp:Content ID="Dashboard" ContentPlaceHolderID="LMainContent" runat="Server">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Send Power of Attorney</span>
            </i>
        </div>

        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li>Send Power of Attorney</li>
        </ol>

        <hr class="hr-new" />
    </section>
    <br />
    <!--GENERAL HOMEPAGE-->
    <section class="content">
        <div class="row">
            <div class="col-md-6">
                <a href="#" class="btn btn-success margin-bottom-20" onclick="showSearch();">Toggle Search</a>
            </div>
        </div>
        <div class="box box-primary " style="display: none;" id="SearchParameters">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <button type="reset" form="formSearch" class="btn btn-sm btn-info pull-right"><i class="fa fa-eraser"></i>Clear Search</button>
                </div>
                <div class="col-md-12 table-responsive">
                    <form method="POST" id="formSearch">
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
                            <label>Phone Number:</label>
                            <input type="text" id="txtPhoneNumber" name="txtPhoneNumber" placeholder="Phone Number" class="form-control" />
                        </div>
                        <div class="col-md-12 text-center margin-top-20">
                            <button type="submit" class="btn btn-info">Search</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-center">
                <div class="box box-primary">
                    <div class="box-body">
                        <table class="table table-hover table-bordered table-striped table-no-wrap text-center">
                            <thead>
                                <tr class="table-row-header">
                                    <th>Client ID</th>
                                    <th>Client Name</th>
                                    <th>Client Email</th>
                                    <th>Phone Number</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="clientsBody" runat="server">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="PowerOfAttorneyModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title text-center">Send Power of Attorney</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <label>Card Holder Name:</label>
                            <input type="text" id="cardHolderName" name="cardHolderName" placeholder="Card Holder Name" class="form-control letters-only" />
                        </div>
                        <div class="col-md-12">
                            <label>Allowed Amount:</label>
                            <input type="text" id="amount" name="amount" placeholder="Amount" class="form-control numeric" />
                        </div>
                        <div class="col-md-12">
                            <label>Last 4 Digits:</label>
                            <input type="text" id="lastdigits" name="lastdigits" placeholder="Last 4 Digits" class="form-control numeric" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" id="btnSendPowerOfAttorney" onclick="SendPOADoc()">Send Now</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="LayoutScriptDecorator" ClientIDMode="Static" runat="server">
    <script>
        function showSearch() {
            $("#SearchParameters").toggle();
        }

        function SendPOA(param) {
            $("#btnSendPowerOfAttorney").attr("onClick", "SendPOADoc('" + param + "')");
            $("#PowerOfAttorneyModal").modal();
        }

        function SendPOADoc(param) {
            var cardholdername = $("#cardHolderName").val();
            var amount = $("#amount").val();
            var lastdigits = $("#lastdigits").val();

            if (cardholdername === "") {
                toastr.error("Please Input Card Holder Name");
                return false;
            }

            if (amount === "") {
                toastr.error("Please Input Amount");
                return false;
            }

            if (lastdigits === "") {
                toastr.error("Please Input Last 4 Digits");
                return false;
            }

            $('#loader').show();
            $.ajax({
                type: 'POST',
                url: '/send-power-of-attorney.aspx/SendPowerOfAttorney',
                data: "{'CardHolderName':'" + cardholdername + "','Amount':'" + amount + "','LastFourDigits':'" + lastdigits + "','Parameter':'" + param + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    $('#loader').hide();
                    $("#PowerOfAttorneyModal").modal("hide");
                    toastr.success("Power of Attorney form Successfully sent to Client.");
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }
    </script>
</asp:Content>
