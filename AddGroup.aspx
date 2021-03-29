<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddGroup.aspx.cs" Inherits="CRM.AddGroup" MasterPageFile="~/Layout.Master" %>

<asp:Content ID="DocumentList" ContentPlaceHolderID="LMainContent" runat="Server" ClientIDMode="Static">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-tasks"></i></span>
                <span class="page-title-text">Add Group</span>
            </i>
        </div>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
            <li class="active">Add Group</li>
        </ol>
        <hr class="hr-new" />
    </section>
    <!--End of Page Title-->
    <!--Start of Page Content-->
    <section class="content margin-top-25">
        <div class="box box-primary">
            <div class="box-body row">
                <form method="post" runat="server" id="frmmAddGroup">
                    <div class="col-md-12 table-responsive">
                        <div class="col-md-6">
                            <label>Group Name:</label>
                            <input class="form-control" type="text" id="groupname" name="groupname" placeholder="Group Name" required="required" runat="server" />
                        </div>
                        <div class="col-md-6">
                            <button type="submit" class="btn btn-info margin-top-25" id="btnSave">SAVE</button>
                        </div>
                    </div>
                </form>
                <div class="col-md-12 margin-top-20">
                    <div class="col-md-12 table-responsive">
                        <table id="groupsTable" class="table table-hover table-bordered table-striped">
                            <thead>
                                <tr class="table-row-header" role="row">
                                    <th>ID</th>
                                    <th>NAME</th>
                                </tr>
                            </thead>
                            <tbody id="groupsBody" name="groupsBody" runat="server">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

<asp:Content ContentPlaceHolderID="LayoutScriptDecorator" runat="Server" ClientIDMode="Static">
    <script type="text/javascript">
        <% var t = toastrUtilities.GetToast(); %>
            <%if (t != null)
        {%>
        toastr.<%=t.Value.Key%>('<%=t.Value.Value%>');
            <%}%>

        $("#frmmAddGroup").on("submit", function (e) {
            var gname = $("#groupname").val();
            if (!gname) {
                toastr.error("Please enter group name")
                $("#loader-container").hide();
                e.preventDefault();
            }
        });
    </script>
</asp:Content>


