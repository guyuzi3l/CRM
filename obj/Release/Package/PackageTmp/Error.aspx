<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="CRM.Error" MasterPageFile="~/Layout.Master" %>

<asp:Content ContentPlaceHolderID="LMainContent" runat="server">
    <section>
        <div class="col-md-6 col-md-offset-3 error-content text-center welcome">
            <h1>
                <i class="fa fa-warning text-red errorIcon"></i><%Response.Write(ErrorString);%>
                    </h1>
            <p class="margin-top-25"><a class="btn btn-info" onclick="history.back();"><i class="fa fa-angle-double-left" aria-hidden="true"></i>Go Back</a></p>
        </div>
    </section>
</asp:Content>
