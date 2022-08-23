<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="Account.aspx.vb" Inherits="CCFRW19.Account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div style="width: 710px; height: 750px; overflow: scroll" class="pSmallTextNJ">
    <asp:GridView ID="AccountGridview" runat="server" AllowPaging="false" CssClass="pSmallTextNJ" AlternatingRowStyle-BackColor="#C2D69B"  AutoGenerateColumns="False"  DataKeyNames="tID" EmptyDataText="No data available." HeaderStyle-BackColor="Wheat" ShowHeaderWhenEmpty="true" Width="650" AllowSorting="True" PageSize="20">
            <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="Label1" runat="server"  Text="Check Number" Width="160px" />
                            <asp:Label ID="Label2" runat="server" Text="Date" Width="80px" />
                             <asp:Label ID="Label4" runat="server" Text="Type" Width="50px" />
                                <asp:Label ID="Label5" runat="server" Text="Category" Width="90px" />
                         <asp:Label ID="Label6" runat="server" Text="Payee" Width="100px" />
<asp:Label ID="Label3" runat="server" CssClass="pStrongTextRight" Text ="Amount" Width="165px" />

                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Table ID="Table2" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="tCheckNumber" Width="160px" runat="server" Text='<%# Bind("tCheckNumber") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="tCheckDate" runat="server" Width="75px" Text='<%# Bind("tCheckDate", "{0:MM/dd/yyyy}") %>' />
                                    </asp:TableCell><asp:TableCell HorizontalAlign="center">
                                        <asp:Label ID="tType" Width="50px"  runat="server" Text='<%# Bind("ttype") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="tCategory" Width="100px" runat="server" Text='<%# Bind("tCategory") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="tPayee" Width="190px" runat="server" Text='<%# Bind("tPayee") %>'/>
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="tCheckAmount" width="80px" cssclass="pNormalTextRight" runat="server"  Text='<%# Bind("tCheckAmount", "{0:c}") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="tBalance" runat="server" Width="90px" cssclass="pNormalTextRight" visible="True" Text='<%# Bind("tBalance", "{0:c}") %>' />
                                    </asp:TableCell></asp:TableRow></asp:Table></ItemTemplate></asp:TemplateField></Columns></asp:GridView></div><br /><asp:Label ID="Message" runat="server" cssCLASS="pHeader2" Text="" /><br />
     
</asp:Content>
