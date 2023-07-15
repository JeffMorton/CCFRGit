<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="CommitteeAsignments.aspx.vb" EnableEventValidation="True"  Inherits="CCFRW19.Committees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="main.css"  />
    <title></title>
</head><script type = "text/javascript">

           function DConfirm() {
               var confirm_value = document.createElement("INPUT");
               confirm_value.type = "hidden";
               confirm_value.name = "confirm_value";
               if (confirm("Delete this Committee??")) {
                   confirm_value.value = "Yes";
               } else {
                   confirm_value.value = "No";
               }
               document.forms[0].appendChild(confirm_value);
           }

</script>
         <script>
             function alertuser(msg) {
                 alert(msg);
             }

         </script>

<body>
    
    <form id="form1" runat="server">
     <asp:Panel ID="Panel1" style="margin-left:150px;border-color:black;border-style:solid;width:650px" runat="server">
       <asp:Panel ID="Panel2" style="margin-left:15px;width:6350px" runat="server">
       
 <div class="pPage" style=" color:black; width: 635px; ">
     <span class="pHeader">Committee Assignments</span><br/><br/>
    <asp:Label ID="Label1" runat="server" Text="Member Name"></asp:Label>    <asp:TextBox ID="Name" runat="server"></asp:TextBox><br/><br/>
     <asp:GridView ID="CommitteesGridView"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            EmptyDataText="No data available."
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowcommand="RowCommands"
            onrowdeleting ="DeleteRow"
            runat="server" DataKeyNames="ID"  HeaderStyle-BackColor="Wheat" ShowFooter="true"
            EnableTheming="False" Width="600px"  ><Columns>
                <asp:TemplateField HeaderText="Committees">
                    
                    <ItemTemplate>
                        
                          <asp:label ID="ID" runat="server" width="25px" Text='<%# Bind("ID") %>' visible="false" />
                                       
                          <asp:textbox ID="Committee" width="240px"  Text='<%# Bind("Committee") %>' ReadOnly="True" runat="server" />&nbsp;

                               
                          <asp:textbox ID="Position" width="183px"  Text='<%# Bind("Position") %>' runat="server" ReadOnly="True" />
                      
                       <asp:Button Text="Delete" runat="server"   CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" onclientclick="Dconfirm"  Width="120px" height="30px" />
                   </ItemTemplate>
  
                    <FooterTemplate>
          
                         <asp:label ID="ID" runat="server" width="28px" Text='<%# Bind("ID") %>' visible="false" />
                 <asp:DropDownList ID="Committee" width="250px" selectedValue='<%# Bind("Committee") %>' runat="server"
                            DataSourceID="ComNameSource"
                            DataValueField="ComName"
                            DataTextField="ComName">
                      </asp:DropDownList>                

                        <asp:DropDownList ID="Position" width="195px" selectedValue='<%# Bind("Position") %>' runat="server"
                            DataSourceID="PositionSource"
                            DataValueField="Position"
                            DataTextField="Position">
                      </asp:DropDownList> 
                 
                        <asp:Button ID="Add" Text="Add" runat="server"  CommandName="Insert"  Width="120px" height="30px" />
                    </FooterTemplate>

                </asp:TemplateField>
            </Columns>
      
        </asp:GridView></div><br /><br />
        <asp:SqlDataSource ID="PositionSource" runat="server"
            selectcommand ="select Position from CommitteePosition order by position"
            ></asp:SqlDataSource>  

           <asp:SqlDataSource ID="ComNameSource" runat="server"
            selectcommand ="select ID,ComName from committeelist order by comname"
            ></asp:SqlDataSource>  
         <br /><br /><asp:Button ID="ExitForm" runat="server" Text="Exit"  Width="120px" height="30px" /><br /><br />
 </asp:Panel>  </asp:Panel></form>
</body>
</html>
