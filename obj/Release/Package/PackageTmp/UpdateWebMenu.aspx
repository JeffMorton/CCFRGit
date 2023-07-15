<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="UpdateWebMenu.aspx.vb" Inherits="CCFRW19.UpdateWebMenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script>
         function alertuser(msg) {
             alert(msg);
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="pPage" style="width:610px;" >      
     <span class="pHeader">Web Menu Table</span><br />
           <span class="pHeader1">Warning, don't make changes to this table unless you are sure you know what you are doing!</span>
           <br/><br/><br/>

     <asp:GridView ID="WebMenuGridView"
            datasourceid ="WebMenuSource"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            EmptyDataText="No data available."
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowcommand="RowCommands"
            runat="server" DataKeyNames="MenuID"  HeaderStyle-BackColor="Wheat" ShowFooter="true"
            EnableTheming="False" Width="715px"><Columns>
                <asp:TemplateField >
                    <HeaderTemplate>
                    <pre class="pStrongText">Id    Menu Text                  Description     ParentID    url                    Order   M1  M2  PH</pre>                                                                                                                                                                                                                            </div>
                        
                    </HeaderTemplate>
                    <ItemTemplate>
                        
                        <asp:TextBox ID="MenuID" runat="server" Text='<%# Bind("MenuID") %>' width="25px" />
                        <asp:textbox ID="Text" Width="150px" runat="server" textmode="MultiLine" Text='<%# Bind("Text") %>' />
                        <asp:textbox ID="Description" Width="120px" runat="server" textmode="MultiLine" Text='<%# Bind("Description") %>' />
                        <asp:TextBox ID="ParentID" runat="server" Text='<%# Bind("ParentID") %>'  width="25px" />
                        <asp:textbox ID="URL" Width="150px" runat="server" textmode="MultiLine" Text='<%# Bind("URL") %>' />
                         <asp:textbox ID="Ordr" Width="25px" runat="server" text='<%# Bind("Ordr") %>' />&nbsp;&nbsp;
                       <asp:CheckBox ID="onMainMaster" runat="server" checked='<%# Bind("onMainMaster") %>'  />&nbsp;
                        <asp:Checkbox ID="onMemberMaster" runat="server" checked='<%# Bind("onMemberMaster") %>' />&nbsp;
                        <asp:CheckBox ID="ProgHistory" runat="server" checked='<%# Bind("ProgHistory") %>'  />
                       <asp:Button Text="Update" runat="server" Width="55px"  CommandArgument='<%# Eval("MenuID") %>'  CommandName="Update" />
                   </ItemTemplate>

                    

                </asp:TemplateField>
            </Columns>
      
        </asp:GridView>
           <p>M1 indicates that the menu item is on the public menu</p>
           <p>M2 indicates that the menu item is on the members only menu</p>
           <p>PH indicates that the menu item is on Program History page</p><br /><br />
           <asp:Button ID="Update" runat="server" Text="Update Web Menus" />
           <asp:Button ID="Restore" runat="server" Text="Restore Prior Web Menus" />

       </div>
        <asp:SqlDataSource ID="WebMenuSource" runat="server"
            selectcommand ="SELECT [MenuID]
      ,[Text]
      ,[Description]
      ,[ParentID]
      ,[URL]
      ,[ordr]
      ,[onMainMaster]
      ,[onMemberMaster]
      ,[ProgHistory]
       FROM [dbo].[WebMenu]"

            updatecommand ="UPDATE [dbo].[WebMenu]
   SET [Text] =@text,
      ,[Description] = @description,
      ,[ParentID] = @parentid,
      ,[URL] = @url,
      ,[ordr] = @ordr,
      ,[onMainMaster] = @onMainMaster,
      ,[onMemberMaster] = @onMemberMaster,
      ,[ProgHistory] = @onProgHistroy
      
 WHERE Menuid=@menuid"
            deletecommand ="Delete webMenu where Menuid=@MenuID"
            ></asp:SqlDataSource>
</asp:Content>
