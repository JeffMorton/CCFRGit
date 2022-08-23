<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="PositionTable.aspx.vb" Inherits="CCFRW19.PosititionTable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pPage" style="width:610px;" >      
     <span class="pHeader">Position Table</span><br/><br/><br/>
     <asp:GridView ID="PositionGridView"
            datasourceid ="PositionSource"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            EmptyDataText="No data available."
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowcommand="RowCommands"
            onrowdeleting ="DeleteRow"
            runat="server" DataKeyNames="ID"  HeaderStyle-BackColor="Wheat" ShowFooter="true"
            EnableTheming="False" Width="400px"><Columns>
                <asp:TemplateField >
                    <HeaderTemplate>
                        <asp:Label ID="Label1" runat="server" Width="50px" Text="Position" />
                        <asp:Label ID="Label2" runat="server" Width="205px" Text="Order" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        
                        <asp:textbox ID="OfficeName" Width="205px" runat="server" Text='<%# Bind("OfficeName") %>' /><asp:textbox ID="OfficeOrder" Width="50px" runat="server" Text='<%# Bind("OfficeOrder") %>' />
                        <asp:TextBox ID="ID" runat="server" Text='<%# Bind("ID") %>' Visible="false" />
                       <asp:Button Text="Update" runat="server" Width="55px"  CommandArgument='<%# Eval("ID") %>'  CommandName="Update" />
                       <asp:Button Text="Delete" runat="server" Width="55px"  CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" />
                   </ItemTemplate>

                    <FooterTemplate>
                        <asp:TextBox ID="OfficeName" runat="server" Width="205px" Text='<%# Bind("OfficeName") %>' /> <asp:textbox ID="OfficeOrder" Width="50px" runat="server" Text='<%# Bind("OfficeOrder") %>' />
                        <asp:Button Text="Add" runat="server" Width="55px" CommandName="Insert" />
                    </FooterTemplate>

                </asp:TemplateField>
            </Columns>
      
        </asp:GridView></div>
        <asp:SqlDataSource ID="PositionSource" runat="server"
            selectcommand ="select ID,OfficeName,OfficeOrder from Offices"
            updatecommand ="Update Offices set OfficeName =@OfficeName,OfficeOrder =@OfficeOrder  where ID=@ID"
            insertcommand ="Insert into Offices (OfficeName,OfficeOrder) values (@OfficeName,@OfficeOrder)"
            deletecommand ="Delete Offices where id=@ID"
            ></asp:SqlDataSource>
</asp:Content>   

