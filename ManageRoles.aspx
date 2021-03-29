<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRoles.aspx.cs" Inherits="CRM.ManageRoles" MasterPageFile="~/Layout.Master" %>

<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" runat="Server" ClientIDMode="Static">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Manage Roles</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Manage Roles</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <a href="#" class="btn btn-success margin-bottom-20" onclick="showSearch();">Toggle Search</a>
        <div class="box box-primary " style="display: none;" id="SearchParameters">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <button type="reset" class="btn btn-sm btn-info pull-right"><i class="fa fa-eraser" onclick="ClearSearch()"></i>Clear Search</button>
                </div>
                <div class="col-md-12 table-responsive">
                    <div class="col-md-4">
                        <label>Role Name:</label>
                        <input type="text" id="srolename" name="srolename" placeholder="Role Name" class="form-control" />
                    </div>
                    <div class="col-md-4">
                        <label>Group Name:</label>
                        <select class="form-control" id="sgroupoptions" name="sgroupoptions" runat="server" />
                    </div>
                    <div class="col-md-4">
                         <label>Type:</label>
                                    <select class="form-control" id="stype" name="stype">
                                        <option value="">--SELECT TYPE--</option>
                                        <option value="TAB">Tab</option>
                                        <option value="FUNCTION">Function</option>
                                    </select>
                    </div>
                    <div class="col-md-4">
                        <label>Role Link:</label>
                        <input type="text" id="srolelink" name="srolelink" placeholder="Role Link" class="form-control" />
                    </div>
                    <div class="col-md-12 text-center margin-top-20">
                        <button type="button" class="btn btn-info" onclick="SearchRoles()">Search</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="box box-primary">
            <div class="box-body row">
                <div class="col-md-12 table-responsive">
                    <div class="col-md-12">
                        <button type="button" class="btn btn-info margin-top-25" data-target="#AddRoleModal" data-toggle="modal">ADD ROLES</button>
                    </div>
                </div>

                <div class="col-md-12 margin-top-20">
                    <div class="col-md-12 table-responsive">
                        <table id="rolesTable" class="table table-hover table-bordered table-striped">
                            <thead>
                                <tr class="table-row-header" role="row">
                                    <th>ID</th>
                                    <th>NAME</th>
                                    <th>GROUP NAME</th>
                                    <th>TYPE</th>
                                    <th>ROLE LINK</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody id="rolesBody" name="rolesBody" runat="server">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="AddRoleModal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Add Role</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <form method="post" runat="server" id="frmmAddRoles">
                            <div class="col-md-12 table-responsive">
                                <div class="col-md-12">
                                    <label>Role Name:</label>
                                    <input class="form-control" type="text" id="rolename" name="rolename" placeholder="Role Name" required="required" runat="server" />
                                </div>
                                <div class="col-md-12">
                                    <label>Group Name:</label>
                                    <select class="form-control" id="groupoptions" name="groupoptions" runat="server" required="required" />
                                </div>
                                <div class="col-md-12">
                                    <label>Type:</label>
                                    <select class="form-control" id="type" name="type" required="required">
                                        <option value="">--SELECT TYPE--</option>
                                        <option value="TAB">Tab</option>
                                        <option value="FUNCTION">Function</option>
                                    </select>
                                </div>
                                <div class="col-md-12" id="divRoleLink" hidden>
                                    <label>Role Link:</label>
                                    <input class="form-control" type="text" id="rolelink" name="rolelink" placeholder="Role Link" required="required" runat="server" />
                                </div>
                                <div class="col-md-12 text-center">
                                </div>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="submit" class="btn btn-info margin-top-25" id="btnSaveRole" form="frmmAddRoles">SAVE ROLE</button>
                        <button type="button" class="btn btn-secondary margin-top-25" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade gbc-modal-sm" tabindex="-1" role="dialog" id="EditRoleModal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Edit Role</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="col-md-12 table-responsive">
                            <div class="col-md-12">
                                <label>Role Name:</label>
                                <input class="form-control" type="text" id="editrolename" name="editrolename" placeholder="Role Name" required="required" runat="server" />
                            </div>
                            <div class="col-md-12">
                                <label>Group Name:</label>
                                <select class="form-control" id="editgroupoptions" name="editgroupoptions" runat="server" required="required" />
                            </div>
                            <div class="col-md-12">
                                <label>Type:</label>
                                <select class="form-control" id="edittype" name="edittype" required="required">
                                    <option value="">--SELECT TYPE--</option>
                                    <option value="TAB">Tab</option>
                                    <option value="FUNCTION">Function</option>
                                </select>
                            </div>
                            <div class="col-md-12" id="divEditRoleLink" hidden>
                                <label>Role Link:</label>
                                <input class="form-control" type="text" id="editrolelink" name="editrolelink" placeholder="Role Link" required="required" runat="server" />
                            </div>
                            <div class="col-md-12 text-center">
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-info margin-top-25" id="btnUpdateRole">UPDATE ROLE</button>
                        <button type="button" class="btn btn-secondary margin-top-25" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

<asp:Content ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
    <script type="text/javascript">

        ToastResult();
        FormSubmit();
        ChooseType();

        function ToastResult() {
            <% var t = toastrUtilities.GetToast(); %>
            <%if (t != null)
        {%>
            toastr.<%=t.Value.Key%>('<%=t.Value.Value%>');
            <%}%>
        };

        function FormSubmit() {
            $("#frmmAddRoles").on("submit", function (e) {
                var rname = $("#rolename").val();
                var gval = $("#groupoptions").val();
                if (!rname) {
                    toastr.error("Please enter role name")
                    $("#loader-container").hide();
                    e.preventDefault();
                } else if (!gval) {
                    toastr.error("Please select group")
                    $("#loader-container").hide();
                    e.preventDefault();
                }
            });
        };

        function ChooseType() {
            $("#type").on("change", function (e) {
                if ($("#type").val() == "FUNCTION") {
                    $("#divRoleLink").hide();
                    $("#rolelink").removeAttr('required');
                    $("#rolelink").val('');
                }
                else if ($("#type").val() == "TAB") {
                    $("#divRoleLink").show();
                    $("#rolelink").prop('required', true);
                }
            });

            $("#edittype").on("change", function (e) {
                if ($("#edittype").val() == "FUNCTION") {
                    $("#divEditRoleLink").hide();
                    $("#editrolelink").removeAttr('required');
                    $("#editrolelink").val('');

                }
                else if ($("#edittype").val() == "TAB") {
                    $("#divEditRoleLink").show();
                    $("#editrolelink").prop('required', true);
                }
            });
        };

        function ChangeType() {

            if ($("#edittype").val() == "FUNCTION") {
                $("#divEditRoleLink").hide();
                $("#editrolelink").removeAttr('required');
                $("#editrolelink").val('');

            }
            else if ($("#edittype").val() == "TAB") {
                $("#divEditRoleLink").show();
                $("#editrolelink").prop('required', true);
            }
        };

        function UpdateRoles(parameter) {
            $("#loader").show();
            $.ajax({
                type: 'POST',
                url: 'ManageRoles.aspx/UpdateRoles',
                data: "{'parameter':'" + parameter + "','roleName':'" + $("#editrolename").val() + "','groupOption':'" + $("#editgroupoptions").val() + "','roleType':'" + $("#edittype").val() + "','roleLink':'" + $("#editrolelink").val() + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    var obj = data.d;
                    if (obj !== null || obj !== "") {
                        $("#EditRoleModal").modal('hide');
                        $("#rolesBody").html(obj);
                        toastr.success('Successfully Saved')
                        $("#loader").hide();
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function GetRole(parameter) {
            $.ajax({
                type: 'POST',
                url: 'ManageRoles.aspx/GetRole',
                data: "{'parameter':'" + parameter + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    var obj = data.d;
                    if (obj !== null) {
                        $("#editrolename").val(obj.Name)
                        $("#editgroupoptions").val(obj.GroupId)
                        $("#edittype").val(obj.Type)
                        $("#editrolelink").val(obj.RoleLink)
                        ChangeType();
                        $('#btnUpdateRole').attr('onclick', 'UpdateRoles("' + parameter + '")');
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function showSearch() {
            $("#SearchParameters").toggle();
        }

        function SearchRoles() {
            $("#loader").show();
            $.ajax({
                type: 'POST',
                url: 'ManageRoles.aspx/SearchRole',
                data: "{'roleName':'" + $("#srolename").val() + "','groupOption':'" + $("#sgroupoptions").val() + "','roleType':'" + $("#stype").val() + "','roleLink':'" + $("#srolelink").val() + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    var obj = data.d;
                    if (obj !== null || obj !== "") {
                        $("#rolesBody").html(obj);
                        $("#loader").hide();
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function ClearSearch() {
            $("#srolename").val("");
            $("#sgroupoptions").val("");
            $("#stype").val("");
            $("#srolelink").val("");
        }

    </script>
</asp:Content>


