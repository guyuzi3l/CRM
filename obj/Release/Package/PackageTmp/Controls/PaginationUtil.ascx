<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaginationUtil.ascx.cs" Inherits="CRM.PaginationUtil" %>

<asp:Repeater ID="PaginationRepeater" runat="server">
    <HeaderTemplate>
        <nav aria-label="Page navigation">
            <ul class="pagination">
                <%if (!String.IsNullOrWhiteSpace(FirstPageUrl))
                {%><li><a href="<%= FirstPageUrl%>">First</a></li><%}%>
    </HeaderTemplate>
    <ItemTemplate>
        <li class="<%# DataBinder.Eval(Container.DataItem, "CssClass")  %>">
            <a href="<%# DataBinder.Eval(Container.DataItem, "Url")  %>">
                <%# Eval("Value").ToString() %>
            </a>
        </li>
    </ItemTemplate>
    <FooterTemplate>
                <%if (!String.IsNullOrWhiteSpace(LastPageUrl))
                {%><li><a href="<%= LastPageUrl%>">Last</a></li><%}%>
            </ul>
        </nav>
    </FooterTemplate>
</asp:Repeater>
