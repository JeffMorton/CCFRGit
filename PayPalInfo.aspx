<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="PayPalInfo.aspx.vb" Inherits="CCFRW19.PayPalInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
          <span class="pHeader">Information from PayPal</span><br /><br />
<div class="pNormalText" >   <asp:Label ID="Label7" runat="server" Text="EventID: " /><asp:TextBox ID="EventID" runat="server" />&nbsp;&nbsp;
<asp:Label ID="Label8" runat="server" Text="Count: " /><asp:TextBox ID="Cnt" runat="server" /> <br /><br /></div>
    <asp:Button ID="SortbyDate" runat="server" Text="Sort by Date" /><asp:Button ID="SortbyName" runat="server" Text="Sort by Name" /><br /><br />
    <asp:Button ID="Top500" runat="server" Text="Latest 500" />
 
    <div style="width: 715px; height: 650px; overflow: scroll" class="pSmallTextNJ">
    <asp:GridView ID="ppGridview" runat="server" AllowPaging="false" CssClass="pVSmallTextNJ" AlternatingRowStyle-BackColor="#C2D69B"  AutoGenerateColumns="False"  DataKeyNames="ID" EmptyDataText="No data available." HeaderStyle-BackColor="Wheat" ShowHeaderWhenEmpty="true" Width="680" AllowSorting="True" PageSize="20">
            <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="Label1" runat="server"  Text="ID" Width="40px" />
                            <asp:Label ID="Label2" runat="server" Text="Date" Width="150px" />
                             <asp:Label ID="Label4" runat="server" Text="Transaction No." Width="120px" />
                                <asp:Label ID="Label5" runat="server" Text="MemberID" Width="66px" />
                         <asp:Label ID="Label6" runat="server" Text="Member Name" Width="140px" />
<asp:Label ID="Label3" runat="server" CssClass="pStrongTextRight" Text ="Amount" Width="90px" />

                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Table ID="Table2" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="ID" Width="40px" runat="server" Text='<%# Bind("ID") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="RegDate" runat="server" Width="150px" Text='<%# Bind("Regdate") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="TransNo" Width="140px" runat="server" Text='<%# Bind("TransNo") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="MemberID" Width="50px" runat="server"  Text='<%# Bind("MemberID") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="mlFullName" Width="150px" runat="server" Text='<%# Bind("mlFullName") %>'/>
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="Amount" width="80px" class="pNormalTextRight" runat="server"  Text='<%# Bind("Amount", "{0:c}") %>' />
                                    </asp:TableCell></asp:TableRow></asp:Table></ItemTemplate></asp:TemplateField></Columns></asp:GridView></div><br />
    <asp:Label ID="Message" runat="server" cssCLASS="pHeader2" Text="" /><br />
     
</asp:Content>
