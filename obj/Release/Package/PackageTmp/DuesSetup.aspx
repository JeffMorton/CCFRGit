<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="DuesSetup.aspx.vb" Inherits="CCFRW19.DuesSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script>
         function alertuser(msg) {
             alert(msg);
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="pPage" style="height:492px; color:black; width: 725px;">
	
	
<span class="pHeader">Dues Setup

</span><br /><br /><br /><br /><br /><br /><br /><br />
           <asp:Table ID="Table1" runat="server">
               <asp:TableHeaderRow id="Table1HeaderRow" 
            BackColor="LightBlue"
            runat="server">
            <asp:TableHeaderCell 
                Scope="Column" 
                Text="" />
            <asp:TableHeaderCell  
                Scope="Column" 
                Text="" />
            <asp:TableHeaderCell 
                Scope="Column"  
                Text="New Dues" />
            <asp:TableHeaderCell 
                Scope="Column"  
                Text="Count" />
        </asp:TableHeaderRow>  
               <asp:TableRow>
                   <asp:TableCell>
                   <asp:label runat="server" text="Standard Dues Amount: " />
                   </asp:TableCell><asp:TableCell>
                       <asp:Label ID="StandardDues" runat="server" />
                   </asp:TableCell><asp:TableCell>
                       <asp:TextBox ID="SDues" runat="server" Width="70px" style="text-align: right"></asp:TextBox>
                   </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="CFull" runat="server" Width="70px" text=0 style="text-align: right"></asp:TextBox>
                   </asp:TableCell>
               </asp:TableRow>
               <asp:TableRow>
                   <asp:TableCell>
                   <asp:label runat="server" text="Dues Amount for Members who Joined after : " />
                   </asp:TableCell><asp:TableCell>
                       <asp:Label ID="halfduesdate" runat="server" />
                   </asp:TableCell><asp:TableCell>
                       <asp:TextBox ID="halfdues" runat="server" Width="70px" style="text-align: right"></asp:TextBox>
                   </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="CHalf" runat="server" Width="70px" text=0 style="text-align: right"></asp:TextBox>
                   </asp:TableCell>
               </asp:TableRow>
               <asp:TableRow>
                   <asp:TableCell>
                   <asp:label runat="server" text="Dues Amount for Members who Joined after : " />
                   </asp:TableCell><asp:TableCell>
                       <asp:Label ID="ZeroDuesDate" runat="server" />
                   </asp:TableCell><asp:TableCell>
                       <asp:TextBox ID="Zerodues" runat="server" Width="70px"  style="text-align: right"></asp:TextBox>
                   </asp:TableCell>
                    <asp:TableCell>
                       <asp:TextBox ID="Czero" runat="server" Width="70px" text=0 style="text-align: right"></asp:TextBox>
                   </asp:TableCell>
               </asp:TableRow>
           </asp:Table><br /><br /><br />
           <asp:Button ID="ProcessDues" runat="server" Text="Process Dues" Height="30px" Width="120px" OnClick="Process" />
           </div>
</asp:Content>
