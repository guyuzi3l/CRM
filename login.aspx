<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="CRM.login" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Login" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="hold-transition login-page">
        <div class="login-box">
            <div class="login-logo">
                <a href="#">
                    <img src="/img/logo-2.png" class="crm-login-logo" />
                </a>
            </div>
            <div class="login-box-body">
                <p class="login-box-msg"><b>Sign in to INSTBTC CRM</b></p>
                <form id="Form1" method="post" runat="server">
                    <div class="form-group has-feedback">
                        <input type="text" name="username" class="form-control" id="username" placeholder="Username" runat="server" required/>
                        <i class="login-icon fa fa-user" aria-hidden="true"></i>
                    </div>
                    <div class="form-group has-feedback">
                        <input type="password" name="password" class="form-control" id="password" placeholder="Password" runat="server" required/>
                        <i class="login-icon fa fa-lock" aria-hidden="true"></i>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <button type="submit" class="btn btn-primary btn-block btn-flat">Sign In</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PageSpecificScript" runat="server">
    <script>
        <% var t = toastrUtilities.GetToast(); %>
            <%if (t != null)
            {%>
                toastr.<%=t.Value.Key%>('<%=t.Value.Value%>');
            <%}%>
    </script>
</asp:Content>