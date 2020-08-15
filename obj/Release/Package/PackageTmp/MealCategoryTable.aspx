<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="MealCategoryTable.aspx.vb" Inherits="CCFRW19.MealCategoryTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
           <script type = "text/javascript">
               function alertuser(msg) {
                   alert(msg);
               }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="pPage" style="width:610px;" >      
     <span class="pHeader">Meal Category Table</span><br/><br/><br/>
     <asp:GridView ID="MealCatGridView"
            datasourceid ="MealcatSource"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            EmptyDataText="No data available."
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowcommand="RowCommands"
            onrowdeleting ="DeleteRow"
            runat="server" DataKeyNames="ID"  HeaderStyle-BackColor="Wheat" ShowFooter="true"
            EnableTheming="False" Width="400px"><Columns>
                <asp:TemplateField HeaderText="Guests">
                    <HeaderTemplate>
                        <asp:Label ID="Label1" runat="server" Text="Category" />

                    </HeaderTemplate>
                    <ItemTemplate>
                        
                          <asp:TextBox ID="ID" runat="server" width="25px" Text='<%# Bind("ID") %>'  />
                      <asp:textbox ID="MealCategory" Width="205px" runat="server" Text='<%# Bind("MealCategory") %>' />
                       <asp:Button Text="Update" runat="server" Width="55px"  CommandArgument='<%# Eval("ID") %>'  CommandName="Update" />
                       <asp:Button Text="Delete" runat="server" Width="55px"  CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" />
                   </ItemTemplate>

                    <FooterTemplate>
                        <asp:label ID="ID" runat="server" width="28px" Text='<%# Bind("ID") %>'  />
                        <asp:TextBox ID="MealCategory" runat="server" Width="205px" Text='<%# Bind("MealCategory") %>' />
                        <asp:Button Text="Add" runat="server" Width="55px" CommandName="Insert" />
                    </FooterTemplate>

                </asp:TemplateField>
            </Columns>
      
        </asp:GridView></div>
        <asp:SqlDataSource ID="MealCatSource" runat="server"
            selectcommand ="select ID,MealCategory from mealcategory"
            updatecommand ="Update MealCategory set MealCategory =@Mealcategory where ID=@ID"
            insertcommand ="Insert into Mealcategory (MealCategory) values (@Mealcategory)"
            deletecommand ="Delete Mealcategory where id=@ID"
            ></asp:SqlDataSource>
</asp:Content>   
