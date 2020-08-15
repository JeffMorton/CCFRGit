<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/member.Master" CodeBehind="EventSignup1.aspx.vb" Inherits="CCFRW19.EventSignup1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
           <script type = "text/javascript">
           function Confirm() {
               var confirm_value = document.createElement("INPUT");
               confirm_value.type = "hidden";
               confirm_value.name = "confirm_value";
               if (confirm("Do you want to delete this guest?")) {
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
       <style type="text/css">
           .auto-style4 {
               font-family: Verdana;
               color: black;
               font-weight: normal;
               font-size: small;
               font-style: normal;
               text-align: justify;
           }
       </style>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContent" runat="server">
  <div class="pPage"   >
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       
<span class="pHeader">Dinner Reservations</span>
     <asp:Label ID="lbEventID" runat="server" Width="499px"></asp:Label>
    <table  style="width: 74%; height: 70px; margin-left: 48px; margin-top: 17px">
		<tr>
			<td style="text-align: right; width: 113px; height: 10px; font-weight: bold;">Event Date:</td>
			<td style="width: 226px; height: 10px; ">
                <asp:Label ID="lbEventDate" runat="server" style="text-align: left" Font-Bold="True"></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: right; width: 113px; height: 10px;">
                <strong>Speaker:</strong></td>
			<td style="width: 226px; height: 10px;">
                <asp:Label ID="lbSpeaker" runat="server" Font-Bold="True"></asp:Label></td>
		</tr>
		<tr>
			<td style="text-align: right; width: 113px; height: 10px;">
                <strong>Cost:</strong></td>
			<td style="width: 226px; height: 10px;">
                <asp:Label ID="lbCost" runat="server" Font-Bold="True"></asp:Label></td>
		</tr>
	</table>
       
      
     
    
        <br />  <br />   <asp:Label CssClass="pHeader2" Text="Members" runat="server"/>
        <br />
       <asp:Panel ID="Panel1" width="707px" height="75px"  borderstyle="Groove" borderwidth="1px" BorderColor="Black" runat="server"><br />
    <asp:FormView ID="FormView1" runat="server" 
        DataSourceID="FVSource" 
        DataKeyNames="ID" >
                <EditItemTemplate>
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp<asp:Label ID="MemberName" runat="server" Width="250px" Text='<%# Eval("mfFullName") %>'></asp:Label>
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="Label6" runat="server" Text="Attend:" Width="55px" CssClass="pNormalTextRight" />
                            </asp:TableCell><asp:TableCell>
                                <asp:CheckBox ID="MemberAttend" Checked='<%# Bind("MemberAttend") %>' runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="Label8" runat="server" Text="Meal:" Width="85px" CssClass="pNormalTextRight" />
                            </asp:TableCell><asp:TableCell>
                                <asp:DropDownList ID="MemberMeal" runat="server" Width="200px" SelectedValue='<%# Bind("MemberMeal") %>'
                                    DataSourceID="MealSource"
                                    DataValueField="Category"
                                    DataTextField="Meal">
                                </asp:DropDownList>
                            </asp:TableCell></asp:TableRow><asp:TableRow>
                           <asp:TableCell>
                                &nbsp<asp:Label ID="SpouseName" runat="server" Text='<%# Eval("sfFullName") %>' Width="250px" CssClass="pNormalText" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="label" runat="server" Text="Attend:" Width="55px" CssClass="pNormalTextRight" />
                            </asp:TableCell><asp:TableCell>
                                <asp:CheckBox ID="SpouseAttend" Checked='<%# Bind("SpouseAttend") %>' runat="server" />
                            </asp:TableCell><asp:TableCell>
                                <asp:Label ID="Label7" runat="server" Text="Meal:" Width="85px" CssClass="pNormalTextRight" />
                            </asp:TableCell><asp:TableCell>
                                <asp:DropDownList ID="SpouseMeal" runat="server" Width="200px" SelectedValue='<%#Bind("SpouseMeal") %>'
                                    DataSourceID="MealSource"
                                    DataValueField="Category"
                                    DataTextField="Meal">
                                </asp:DropDownList>
                            </asp:TableCell><asp:TableCell>
                                <asp:TextBox ID="ID" runat="server" Text='<%# Bind("ID") %>' visible ="false"/>
                            </asp:TableCell></asp:TableRow></asp:Table></EditItemTemplate><EmptyDataTemplate>No data found</EmptyDataTemplate></asp:FormView>
           <asp:SqlDataSource ID="FVSource" runat="server"
            selectcommand="SELECT        mfFullName, Member.sfFullName, Member.ID AS MemID, Member.NewMember, Member.MealsOwed, Member.DuesOwed, Member.LastName, Member.FirstName, tmpMemberSignUp.ID, 
                         tmpMemberSignUp.MemberAttend, tmpMemberSignUp.SpouseAttend, tmpMemberSignUp.MemberMeal, tmpMemberSignUp.SpouseMeal, tmpMemberSignUp.NewMember AS Expr1, tmpMemberSignUp.EventID, 
                         tmpMemberSignUp.MemberID FROM  Member INNER JOIN
                         tmpMemberSignUp ON Member.ID = tmpMemberSignUp.MemberID where member.id=@MemberID and tmpMembersignup.Eventid = @Eventid"
            updatecommand="Update tmpmembersignup set MemberAttend = @MemberAttend, SpouseAttend = @SpouseAttend, MemberMeal = @Membermeal,SpouseMeal=@SpouseMeal where ID = @ID">
        <SelectParameters>
            <asp:sessionparameter  SessionField="UserID" Name=MemberID />
            <asp:SessionParameter SessionField="EventID" Name=Eventid />
        </SelectParameters> 
     
        </asp:SqlDataSource></asp:Panel>
      <asp:SqlDataSource ID="MealSource" runat="server"></asp:SqlDataSource>
           
       
 
<br /><asp:Label CssClass="pHeader2" Text="Guests" runat="server"/>
        <asp:GridView ID="GuestGridView"
            CssClass="auto-style4"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowdeleting="DeleteGuest"
            onrowdatabound="OnRowDataBound"
            onrowcommand="GridCommand"
            showfooter="true"
            runat="server" DataKeyNames="ID" AlternatingRowStyle-BackColor="#C2D69B" HeaderStyle-BackColor="Wheat" 
            EnableTheming="False" Width="713px"><Columns>
                <asp:TemplateField HeaderText="Guests">
                    <HeaderTemplate>
                        <asp:Label ID="Label1" runat="server" Width="205px" Text="Last Name" />
                        <asp:Label ID="GFirstName" runat="server" Width="185px" Text="First Name" />
                        <asp:Label ID="GuestMeal" runat="server" Width="150px" Text="Meal" />
                    </HeaderTemplate>
                    <itemTemplate>
                        <asp:label ID="GLastName" Width="195px" runat="server" Text='<%# Bind("GLastName") %>' />
                        <asp:label ID="GFirstName" Width="185px" runat="server" Text='<%# Bind("GFirstName") %>' />
                        <asp:label ID="GuestMeal" runat="server" Width="190px" Text='<%#Bind("Meal") %>'  />
                        <asp:TextBox ID="ID" runat="server" Text='<%# Bind("ID") %>' Visible="False" />
                         <asp:TextBox ID="WebID" runat="server" Text='<%# Bind("webID") %>' Visible="false" />
                         <asp:TextBox ID="MemberID" runat="server" Text='<%# Bind("MemberID") %>' Visible="false" />
                         <asp:TextBox ID="EventID" runat="server" Text='<%# Bind("EventID") %>' Visible="false" />
                       <asp:Checkbox ID="AddedToMealsOwed" runat="server" Checked='<%# Bind("AddedToMealsOwed") %>' Visible="false" />
                        <asp:Button Text="Delete" runat="server" CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" Width="120px" height="25px" OnClientClick = "Confirm()" />
    
                    </itemTemplate>

                    <FooterTemplate>
                        <asp:TextBox ID="GLastName" runat="server" Width="190px" Text='<%# Bind("GLastName") %>' />
                        <asp:TextBox ID="GFirstName" runat="server" Width="180px" Text='<%# Bind("GFirstName") %>'/>
                        <asp:DropDownList ID="GuestMeal" runat="server" Width="193px" DataValueField="MealCategory" DataTextField="Meal"></asp:DropDownList> 
                        <asp:Button Text="Add" runat="server" Width="120px" height="25px" CommandName="Insert" />
                    </FooterTemplate>
                   
                  
                </asp:TemplateField>
            </Columns>

        </asp:GridView><br /><br />
          <asp:SqlDataSource ID="GuestSource" runat="server"
              selectcommand="select tmpGuestSignUp.GLastName,tmpGuestSignUp.GFirstName, tmpGuestSignUp.ID,tmpGuestSignUp.AddedToMealsOwed,EventMC.Meal,webID,MemberID,EventID FROM tmpGuestSignUp INNER JOIN EventMC ON tmpGuestSignUp.EventID = EventMC.ID And tmpGuestSignUp.GuestMeal = EventMC.category where eventid = @EventID And memberid =@MemberID"
              Deletecommand="Delete from tmpguestSignup where id= @Id">
             <selectparameters>
                 <asp:SessionParameter Name="EventID" SessionField="EventID" />
                 <asp:SessionParameter Name="MemberID" SessionField="UserID" />
             </selectparameters>
             
           
              </asp:SqlDataSource>

       <asp:Panel ID="Panel3" runat="server" height="90px"  width="715px"  BorderStyle="Solid" BorderWidth="1px" Visible="false">
           
           <asp:Table ID="Table1" runat="server">

               <asp:TableRow>
                   <asp:TableCell>
                       <asp:CheckBox ID="Dues" runat="server" visible="false"/>
                                       </asp:TableCell>
                   <asp:TableCell>
                       <asp:Label ID="DuesOwed" runat="server" Text="Label" Visible="false"></asp:Label>
                   </asp:TableCell>

               </asp:TableRow>
               <asp:TableRow>

               </asp:TableRow> <asp:TableRow>

               </asp:TableRow>
               <asp:TableRow>
                   <asp:TableCell>
                        <asp:CheckBox ID="Meals" runat="server" visible =" false"/>
                    </asp:TableCell>
                   <asp:TableCell>
                       <asp:Label ID="MealsOwed" runat="server" Text="Label" Visible="false"></asp:Label>
                   </asp:TableCell>

               </asp:TableRow>
           </asp:Table>
            <asp:Label ID="Message" runat="server" cssCLASS="pHeader2" Text="" /><br />
       
     </asp:Panel>
        		<br /><asp:Button ID="Review" runat="server" Text="Review and Pay" Width="183px" />

       </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AdditionalContent" runat="server">
</asp:Content>
