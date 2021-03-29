<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manage-documents.aspx.cs" Inherits="CRM.manage_documents" MasterPageFile="~/Layout.Master" %>

<asp:Content ContentPlaceHolderID="LayoutHeaderDecorator" runat="server">

    <style type="text/css">
        .warning-trade-mod {
            background: #e8e8e8;
            padding: 15px;
            border-radius: 5px;
            border: #c5c5c5 2px solid;
            font-size: 15px;
            color: black;
        }

        .warning-message {
            color: red;
            font-size: 13px;
        }

        .warning-container {
            margin-top: 15px;
            margin-bottom: 15px;
        }

        .warning-amount {
            color: #0074ad;
        }

        .spanClass {
            color: red;
        }
    </style>

</asp:Content>


<asp:Content ContentPlaceHolderID="LMainContent" runat="server">

    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Manage Documents</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Manage Documents</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <div class="box box-primary">
            <!-- /.box-header -->
            <!-- form start -->
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-4">
                        <a id="btnBack" type="button" class="btn btn-info margin-bottom-10 margin-top-20 btn-sm" href="/document-lists.aspx">Back to Documents</a>
                    </div>
                     <div class="col-md-4">
                        <a href="<%=aws_file %>" type="button" class="btn btn-info margin-bottom-10 margin-top-20 btn-sm" target="_blank">View Document File</a>
                    </div>
                    <div class="col-md-4" style="text-align: right" id="clientbtn" runat="server">
                    </div>
                </div>
            </div>
            <div class="box-body row">
                <div class="content">
                    <div class="col-md-12 margin-bottom-20 text-center" style="background-color: #3c8dbc; padding: 8px; margin-top: -10px;">
                        <div class="col-sm-12">
                            <label style="color: white;">CURRENT STATUS: &nbsp; <span id="currStatus" runat="server"></span>&nbsp; | &nbsp; TYPE: <span id="currType" runat="server"></span> &nbsp; | &nbsp; SUBTYPE: <span id="currSubtype" runat="server"></span></label>
                        </div>
                    </div>
                    <div class="col-md-12 table-responsive" id="imageContainer" runat="server">
                    </div>
                    <div class="col-md-12 text-center">
                        <div class="col-md-12 text-center">
                            <button id="btnSubmit" type="submit" class="btn btn-info">Change Status</button>
                            <button id="btnChangeType" type="submit" class="btn btn-info">Change Type</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.box-body -->
        </div>
    </section>
    <!--Start of Page Content-->

</asp:Content>

<asp:Content ContentPlaceHolderID="LayoutScriptDecorator" runat="server">

    <script type="text/javascript">
        self = this;

        $('#clientsTable').DataTable();

        $('.datepicker').datepicker({
            autoclose: true
        });

        function openNewTab() {
            var image = new Image();
            image.src = $('#imageEncoded').attr('src');
            var w = window.open("");
            w.document.write(image.outerHTML);
        }

        $("#btnSubmit").on("click", function () {
            $.confirm({
                title: 'Change Status and Expiry Date',
                content: '' +
                    '<div class="form-group">' +
                    '<form method=\'post\' id=\'frmSubmit\'>' +
                    '<select class=\'form-control text-center\' name=\'selectedStatus\' id=\'selectedStatus\'>' +
                    '<option>Initial</option> <option>Approved</option> <option>Rejected</option> </select> <br/> <input type=\'date\' class=\'form-control\'  name=\'expiryDate\' id=\'expiryDate\' > <br/> <input type=\'text\' class=\'form-control\'  name=\'cardLast4\' id=\'cardLast4\' placeholder=\'Please put the card last 4 if document is cc\' > </form>' +
                    '</div>',
                buttons: {
                    formSubmit: {
                        text: 'Change Status and Expiry Date',
                        btnClass: 'btn-green',
                        action: function () {
                            $("form#frmSubmit").submit();
                        }
                    },
                    cancel: function () {

                    }
                }
            });
        });

        $("#btnChangeType").on("click", function () {
            $.confirm({
                title: 'Change Type',
                content: '' +
                    '<div class="form-group">' +
                    '<label>Document Type: </label>' +
                    '<select class=\'form-control text-center\' name=\'documentType\' id=\'documentType\' onChange="TypeChange()">' +
                    '<option selected="selected" disabled="disabled">--Please Select Document Type--</option>' +
                    '<option value="Proof of ID">Proof of ID</option>' +
                    '<option value="Proof of Residence">Proof of Residence</option>' +
                    '<option id="documentType_cc" value="Credit Card">Credit Card</option>' +
                    '<option value="Power of Attorney">Power of Attorney</option>' +
                    '</select> ' +
                    '</div>' +
                    '<div class="form-group">' +
                    '<label>Document Subtype: </label>' +
                    '<select id="documentSubtype" name="documentSubtype" class="form-control modal-selectbox">' +
                    '<option selected="selected" disabled="disabled">--Please Select Document Sub Type--</option>' +
                    '</select>' +
                    '</div>',
                buttons: {
                    formSubmit: {
                        text: 'Change Type',
                        btnClass: 'btn-green',
                        action: function () {
                            ChangeType();
                        }
                    },
                    cancel: function () {

                    }
                }
            });
        });

        function ChangeType() {
            var Type = $("#documentType").val();
            var SubType = $("#documentSubtype").val();

            if (Type === "" || Type === null) {
                toastr.error("Please Enter Type");
                return false;
            }

            if (SubType === "" || SubType === null) {
                toastr.error("Please Enter Sub Type");
                return false;
            }

            $("#loader").show();
            $.ajax({
                type: 'POST',
                url: '/manage-documents.aspx/ChangeType?id=<%=docid%>',
                data: "{'Type':'" + Type + "','SubType':'" + SubType + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    obj = data.d;
                    $("#currType").text(obj.Type);
                    $("#currSubtype").text(obj.SubType);
                    toastr.success("Type Successfully Updated.");
                    $("#loader").hide();
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }
    </script>
    <script>
        var ProofOfId = [
            { display: "Passport", value: "Passport" },
            { display: "National ID", value: "National ID" },
            { display: "Driver's License", value: "Driver's License" }
        ];

        var ProofOfResidence = [
            { display: "Utility Bills", value: "Utility Bills" },
            { display: "Bank Statements", value: "Bank Statements" }
        ];

        var CreditCard = [
            { display: "Front", value: "Front" },
            { display: "Back", value: "Back" }
        ];

        var PowerOfAttorney = [
            { display: "Power of Attorney", value: "Power of Attorney" }
        ];

        function list(array_list) {
            $("#documentSubtype").html(""); //reset child options
            $(array_list).each(function (i) { //populate child options 
                $("#documentSubtype").append("<option value=\"" + array_list[i].value + "\">" + array_list[i].display + "</option>");
            });
        }

        function TypeChange() {
            loadDocSubType();
        }

        if ($("#documentType").val() !== null || $("#documentType").val() !== "") {
            console.log('test load');
            loadDocSubType();
        }

        function loadDocSubType() {
            switch ($("#documentType").val()) {
                case "Proof of ID":
                    list(ProofOfId);
                    break;
                case "Proof of Residence":
                    list(ProofOfResidence);
                    break;
                case "Credit Card":
                    list(CreditCard);
                    break;
                case "Power of Attorney":
                    list(PowerOfAttorney);
                    break;
            }
        }
    </script>
</asp:Content>


