<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="upload-docu.aspx.cs" Inherits="CRM.upload_docu" %>


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
                <span class="page-title-icon"><i class="fa fa-send"></i></span>
                <span class="page-title-text">Upload Document</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Upload Document</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <a href="/document-lists.aspx" class="btn btn-primary margin-bottom-20"><i class="fa fa-arrow"></i>&nbsp;Back to Document</a>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Upload Client Document</h3>
            </div>
            <div class="box-body row">
                <form method="post" enctype="multipart/form-data">
                    <div class="col-md-4">
                        <label>Client Id:</label>
                        <input type="number" name="clientId" class="form-control" required />
                    </div>
                    <div class="col-md-4">
                        <label>Document Type: </label>
                        <select id="documentType" name="documentType" class="form-control modal-selectbox">
                            <option>--Please Select Document Type--</option>
                            <option value="Proof of ID">Proof of ID</option>
                            <option value="Proof of Residence">Proof of Residence</option>
                            <option id="documentType_cc" value="Credit Card">Credit Card</option>
                            <option value="Power of Attorney">Power of Attorney</option>
                            <option value="Selfie">Selfie</option>
                            <option value="Swift">Swift</option>
                            <option value="Credit Card">Credit Card</option>
                            <option value="Source of Funds">Source of Funds</option>
                        </select>
                    </div>
                    <div class="col-md-4">
                        <label>Document Subtype: </label>
                        <select id="documentSubtype" name="documentSubtype" class="form-control modal-selectbox">
                            <option selected="selected" disabled="disabled">--Please Select Document Sub Type--</option>
                        </select>

                    </div>
                    <div class="col-md-4">
                        <label>Upload Document: </label>
                        <input type="file" id="documentUpload" name="documentUpload" class="form-control" />
                    </div>
                    <div class="col-md-4 margin-top-25">
                        <button class="btn btn-success btn-dark" id="btnUpload">Upload</button>
                    </div>
                </form>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
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

        var Selfie = [
            { display: "Selfie", value: "Selfie" }
        ];

        var SourceOfFunds = [
            { display: "Source of Funds", value: "Source of Funds" }
        ];

        var Swift = [
            { display: "Swift", value: "Swift" }
        ];

        function list(array_list) {
            $("#documentSubtype").html(""); //reset child options
            $(array_list).each(function (i) { //populate child options 
                $("#documentSubtype").append("<option value=\"" + array_list[i].value + "\">" + array_list[i].display + "</option>");
            });
        }

        $("#documentType").on("change", function () {
            loadDocSubType();
        });

        if ($("#documentType").val != null || $("#documentType").val() != "") {
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
                case "Selfie":
                    list(Selfie);
                    break;
                case "Source of Funds":
                    list(SourceOfFunds);
                    break;
                case "Swift":
                    list(Swift);
                    break;
            }

        }

        $("#btnUpload").on("click", function (e) {
            if ($("#documentType").val() == null || $("#documentType").val() == "") {
                console.log('asd')
                toastr["error"]("Please Select Document Type");
                e.preventDefault();
            }
            else if ($("#documentSubtype").val() == null || $("#documentSubtype").val() == "") {
                toastr["error"]("Please Select Document Sub Type");
                e.preventDefault();
            }
        });
    </script>
</asp:Content>
