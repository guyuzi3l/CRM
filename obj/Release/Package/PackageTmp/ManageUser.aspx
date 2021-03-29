<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUser.aspx.cs" Inherits="CRM.ManageUser" MasterPageFile="~/Layout.Master" %>

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
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Manage User</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Manage User</li>
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
                    <div class="col-md-5">
                        <label>Username:</label>
                        <input type="text" id="susername" name="susername" placeholder="Username" class="form-control" />
                    </div>
                    <div class="col-md-5">
                        <label>Email:</label>
                        <input type="text" id="semail" name="semail" placeholder="Email" class="form-control" />
                    </div>
                    <div class="col-md-2 margin-top-25">
                        <button type="button" class="btn btn-info" onclick="SearchUsers()">Search</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-body row">
                <form method="post">
                    <div class="col-md-12 margin-bottom-20">
                        <div class="col-md-12">
                            <button type="button" class="btn btn-info" onclick="openNav('addUserDiv')">Add User</button>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-12 table-responsive">
                            <table id="usersTable" class="table table-hover table-bordered table-striped">
                                <thead>
                                    <tr class="table-row-header" role="row">
                                        <th>ID</th>
                                        <th>USERNAME</th>
                                        <th>EMAIL</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody id="usersBody" name="usersBody" runat="server">
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div id="addUserDiv" class="overlay-1">
                        <div class="cold-md-12 div-header margin-bottom-20">
                            <label class="div-header-title">ADD USER</label>
                            <button class="closebtn btn btn-danger" onclick="closeNav('addUserDiv')" style="padding: 1px 6px !important;"><i class="fa fa-times" aria-hidden="true"></i></button>
                        </div>
                        <div class="overlay-content col-md-12">
                            <div class="text-left">
                                <div class="col-md-12 table-responsive">
                                    <div class="form-group col-md-4">
                                        <label>Username:</label>
                                        <input class="form-control" type="text" id="username" name="username" placeholder="Username" required="required" />
                                    </div>
                                    <div class="form-group col-md-4">
                                        <label>Password:</label>
                                        <input class="form-control" type="password" id="password" name="password" placeholder="Password" required="required" />
                                    </div>
                                    <div class="form-group col-md-4">
                                        <label>Email:</label>
                                        <input class="form-control" type="email" id="email" name="email" placeholder="Email" required="required" />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group col-md-12 table-responsive">
                                        <div class="col-md-12 roles-container-1 table-responsive">
                                            <div class="selectall col-md-3">
                                                <input type="checkbox" id="selecctall" />
                                                <h4>Toggle All</h4>
                                            </div>
                                            <div class="col-md-9">
                                                <p>&nbsp;</p>
                                            </div>
                                        </div>
                                        <%foreach (var rg in roleGroup)
                                            { %>
                                        <div class="page-title-cont-1">
                                            <i class="page-title-i">
                                                <span class="page-title-icon"><i class="fa fa-gears"></i></span>
                                                <span class="page-title-text"><%Response.Write(rg.GroupName);%> Roles</span>
                                            </i>
                                        </div>
                                        <div class="col-md-12 roles-container">
                                            <%rolePrint = rolesModels.Where(c => c.GroupId == rg.GroupId).ToList();
                                                foreach (var role in rolePrint)
                                                {%>
                                            <div class='col-md-2 form-group roles-title'>
                                                <input type='checkbox' class='checkbox1' name='Roles' value='<%Response.Write(role.Id);%>' /><h4><%Response.Write(role.Name);%></h4>
                                            </div>
                                            <%} %>
                                        </div>
                                        <%} %>
                                    </div>
                                </div>
                                <div class="col-md-12 text-center">
                                    <div class="col-md-12">
                                        <button type="submit" class="btn btn-info margin-bottom-20">SAVE</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
                <div id="EditUserDiv" class="overlay-1">
                    <div class="cold-md-12 div-header margin-bottom-20">
                        <label class="div-header-title">EDIT USER</label>
                        <button class="closebtn btn btn-danger" onclick="closeNav('EditUserDiv')" style="padding: 1px 6px !important;"><i class="fa fa-times" aria-hidden="true"></i></button>
                    </div>
                    <div class="overlay-content col-md-12">
                        <div class="text-left">
                            <div class="col-md-12 table-responsive">
                                <div class="form-group col-md-4">
                                    <label>Username:</label>
                                    <input class="form-control" type="text" id="editusername" name="editusername" placeholder="Username" required="required" />
                                </div>
                                <div class="form-group col-md-4">
                                    <label>Password:</label>
                                    <input class="form-control" type="password" id="editpassword" name="editpassword" placeholder="Password" required="required" />
                                </div>
                                <div class="form-group col-md-4">
                                    <label>Email:</label>
                                    <input class="form-control" type="email" id="editemail" name="editemail" placeholder="Email" required="required" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group col-md-12 table-responsive">
                                    <div class="col-md-12 roles-container-1 table-responsive">
                                        <div class="selectall col-md-3">
                                            <input type="checkbox" id="editselecctall" />
                                            <h4>Toggle All</h4>
                                        </div>
                                        <div class="col-md-9">
                                            <p>&nbsp;</p>
                                        </div>
                                    </div>
                                    <%foreach (var rg in roleGroup)
                                        { %>
                                    <div class="page-title-cont-1">
                                        <i class="page-title-i">
                                            <span class="page-title-icon"><i class="fa fa-gears"></i></span>
                                            <span class="page-title-text"><%Response.Write(rg.GroupName);%> Roles</span>
                                        </i>
                                    </div>
                                    <div class="col-md-12 roles-container">
                                        <%rolePrint = rolesModels.Where(c => c.GroupId == rg.GroupId).ToList();
                                            foreach (var role in rolePrint)
                                            {%>
                                        <div class='col-md-2 form-group roles-title'>
                                            <input type='checkbox' class='editcheckbox1' name='editRoles' value='<%Response.Write(role.Id);%>' /><h4><%Response.Write(role.Name);%></h4>
                                        </div>
                                        <%} %>
                                    </div>
                                    <%} %>
                                </div>
                            </div>
                            <div class="col-md-12 text-center">
                                <div class="col-md-12">
                                    <button type="button" class="btn btn-info margin-bottom-20" id="btnUpdateUser">UPDATE</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
    <script>
        ToastResult();

        $(document).ready(function () {
            $('#selecctall').click(function (event) {  //on click 
                if (this.checked) { // check select status
                    $('.checkbox1').each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes with class "checkbox1"               
                    });
                } else {
                    $('.checkbox1').each(function () { //loop through each checkbox
                        this.checked = false; //deselect all checkboxes with class "checkbox1"                       
                    });
                }
            });

            $('#editselecctall').click(function (event) {  //on click 
                if (this.checked) { // check select status
                    $('.editcheckbox1').each(function () { //loop through each checkbox
                        this.checked = true;  //select all checkboxes with class "editcheckbox1"               
                    });
                } else {
                    $('.editcheckbox1').each(function () { //loop through each checkbox
                        this.checked = false; //deselect all checkboxes with class "editcheckbox1"                       
                    });
                }
            });
        });

        function ToastResult() {
            <% var t = toastrUtilities.GetToast(); %>
            <%if (t != null)
        {%>
            toastr.<%=t.Value.Key%>('<%=t.Value.Value%>');
            <%}%>
        };

        function GetUser(parameter) {
            $("#loader").show();
            $.ajax({
                type: 'POST',
                url: 'ManageUser.aspx/GetUser',
                data: "{'parameter':'" + parameter + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    var obj = data.d;
                    if (obj !== null) {
                        $("#editusername").val(obj.UserModel.Username);
                        $("#editemail").val(obj.UserModel.Email);
                        $("#editpassword").val("");
                        var roles = obj.ListRoles;
                        $('.editcheckbox1').each(function () { //loop through each checkbox
                            this.checked = false; //deselect all checkboxes with class "editcheckbox1"                       
                        });
                        $.each(roles, function (id, role) {
                            $(".editcheckbox1[value=" + role + "]").prop("checked", "true"); //select all checkboxes with class designated roles    
                        });
                        openNav('EditUserDiv');
                        $('#btnUpdateUser').attr('onclick', 'UpdateUser("' + parameter + '")');
                        $("#loader").hide();
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function UpdateUser(parameter) {
            $("#loader").show();
            $.ajax({
                type: 'POST',
                url: 'ManageUser.aspx/UpdateUser',
                data: "{'parameter':'" + parameter + "','username':'" + $("#editusername").val() + "','password':'" + $("#editpassword").val() + "','email':'" + $("#editemail").val() + "','roles':'" + $('input[name="editRoles"]:checked').map(function () { return this.value; }).get().join(',') + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    var obj = data.d;
                    if (obj !== null || obj !== "") {
                        closeNav('EditUserDiv');
                        $("#usersBody").html(obj);
                        toastr.success('Successfully Saved')
                        $("#loader").hide();
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function updateTextArea() {
            var allRoles = [];
            $("input[name='editRoles'] :checked").each(function () {
                allRoles.push($(this).val());
            });
            return allRoles;
        }

        function showSearch() {
            $("#SearchParameters").toggle();
        }

        function SearchUsers() {
            $("#loader").show();
            $.ajax({
                type: 'POST',
                url: 'ManageUser.aspx/SearchUser',
                data: "{'userName':'" + $("#susername").val() + "','eMail':'" + $("#semail").val() + "'}",
                contentType: 'application/json; charset = utf-8',
                success: function (data) {
                    var obj = data.d;
                    if (obj !== null || obj !== "") {
                        $("#usersBody").html(obj);
                        $("#loader").hide();
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }

        function ClearSearch() {
            $("#susername").val("");
            $("#semail").val("");
        }

    </script>
</asp:Content>


