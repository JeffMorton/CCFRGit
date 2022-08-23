<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="CommitteeList.aspx.vb" Inherits="CCFRW19.CommitteeList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
          <script type = "text/javascript">
               function alertuser(msg) {
                   alert(msg);
               }
          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="pPage" style="width:610px;" >      
     <span class="pHeader">Committee List Table</span><br/><br/><br/>
     <asp:GridView ID="CommListGridView"
            datasourceid ="CommListSource"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            EmptyDataText="No data available."
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowcommand="RowCommands"
            onrowdeleting ="DeleteRow"
            runat="server" DataKeyNames="ID"  HeaderStyle-BackColor="Wheat" ShowFooter="true"
            EnableTheming="False" Width="400px"><Columns>
                <asp:TemplateField HeaderText="Committees">
                    <HeaderTemplate>
                        <asp:Label ID="Label1" runat="server" Text="Committees" />

                    </HeaderTemplate>
                    <ItemTemplate>
                        
                          <asp:Label ID="ID" runat="server" width="25px" Text='<%# Bind("ID") %>' visible="false" />
                      <asp:textbox ID="ComName" Width="205px" runat="server" Text='<%# Bind("ComName") %>' />
                       <asp:Button Text="Update" runat="server" Width="55px"  CommandArgument='<%# Eval("ID") %>'  CommandName="Update" />
                       <asp:Button Text="Delete" runat="server" Width="55px"  CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" />
                   </ItemTemplate>

                    <FooterTemplate>
                        <asp:label ID="ID" runat="server" width="28px" Text='<%# Bind("ID") %>'  />
                        <asp:TextBox ID="ComName" runat="server" Width="205px" Text='<%# Bind("ComName") %>' />
                        <asp:Button Text="Add" runat="server" Width="55px" CommandName="Insert" />
                    </FooterTemplate>

                </asp:TemplateField>
            </Columns>
      
        </asp:GridView></div>
        <asp:SqlDataSource ID="CommListSource" runat="server"
            selectcommand ="select ID,ComName from CommitteeList where comName > 'A'"
            updatecommand ="Update CommitteeList set ComName =@ComName where ID=@ID"
            insertcommand ="Insert into CommitteeList (ComName) values (@ComName)"
            deletecommand ="Delete CommitteeList where id=@ID"
            ></asp:SqlDataSource>
</asp:Content>
