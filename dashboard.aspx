<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="CRM.dashboard" MasterPageFile="~/Layout.Master" %>

<asp:Content ID="Dashboard" ContentPlaceHolderID="LMainContent" Runat="Server">
    <!--Start of Page Title-->
    <section class="content-header">
        <div class="page-title-cont">
            <i class="page-title-i">
                <span class="page-title-icon"><i class="fa fa-home"></i></span>
                <span class="page-title-text">Home</span>
            </i>
        </div>

        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-home"></i>Home</a></li>
        </ol>

        <hr class="hr-new" />
    </section>  
    <br />
    <!--GENERAL HOMEPAGE-->
    <section class="content">
        <div class="col-md-8 col-md-offset-2 error-content text-center welcome">
            <br /><br />
            <h1>WELCOME TO INSTBTC.IO CRM SYSTEM</h1>
        </div>
    </section>
</asp:Content>