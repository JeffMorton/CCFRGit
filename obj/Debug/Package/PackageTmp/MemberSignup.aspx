<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="true"   MasterPageFile="~/member.Master" CodeBehind="MemberSignup.aspx.vb" Inherits="CCFRW19.MemberSignup" %>
 <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are you sure you want to cancel this reservation?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
         }
         function DConfirm() {
             var confirm_value = document.createElement("INPUT");
             confirm_value.type = "hidden";
             confirm_value.name = "confirm_value";
             if (confirm("Delete this guest??")) {
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	 <div class="pPage" style=" color:black; width: 725px">
               <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:Label ID="TitleL" cssclass="pHeader" runat="server" Text="Label"></asp:Label>

        <asp:Panel ID="MemberPanel" runat="server">

     <asp:Label ID="lbEventID" runat="server" Width="499px"></asp:Label>
    <table  style="width: 74%;height: 70px;  margin-left: 48px; margin-top: 17px">
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
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
 	Please check the box of the people coming to the event and select the meal option.<br /><br />
	

	  <asp:FormView ID="FormView1" runat="server" 
        DataSourceID="FVSource" 
        DataKeyNames="ID" >
                 <EditItemTemplate>
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell>
                                &nbsp<asp:Label ID="MemberName" runat="server" Width="185px" Text='<%# Eval("mfFullName") %>'></asp:Label>
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
                                &nbsp<asp:Label ID="SpouseName" runat="server" Text='<%# Eval("sfFullName") %>' Width="185px" CssClass="pNormalText" />
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
     
        </asp:SqlDataSource>
	      <asp:SqlDataSource ID="MealSource" runat="server"></asp:SqlDataSource>
 	</asp:Panel>
	
         <asp:Panel ID="GuestPanel" runat="server">
	<br /><br />
             
             To add guests, enter their name  in the last row, select a meal and click on the Add button. <br /><br />
             <asp:Label CssClass="pHeader2" Text="Guests" runat="server"/>
        <asp:GridView ID="GuestGridView"
           CssClass="auto-style4"
            AlternatingRowStyle-CssClass="pNormatext"
            AutoGenerateColumns="False"
            AllowPaging="false"
            ShowHeaderWhenEmpty="true"
            onrowdeleting="DeleteGuest"
            onrowcommand="GridCommand"
            showfooter="true"
            runat="server" DataKeyNames="ID" AlternatingRowStyle-BackColor="#C2D69B" HeaderStyle-BackColor="Wheat" 
            EnableTheming="False" Width="713px"><Columns>
                <asp:TemplateField HeaderText="Guests">
                    <HeaderTemplate>
                        <asp:Label ID="GLastName" runat="server" Width="195px"  Text="Last Name" />
                        <asp:Label ID="GFirstName" runat="server" Width="185px"  Text="First Name" />
                        <asp:Label ID="GuestMeal" runat="server" Width="150px"  Text="Meal" />
                    </HeaderTemplate>
                    <itemTemplate>
                        <asp:label ID="GLastName" Width="195px" runat="server" Text='<%# Bind("GLastName") %>' />
                        <asp:label ID="GFirstName" Width="185px" runat="server" Text='<%# Bind("GFirstName") %>' />
                        <asp:label ID="GuestMeal" runat="server" Width="190px"  Text='<%#Bind("Meal") %>'  />
                        <asp:TextBox ID="ID" runat="server" Text='<%# Bind("ID") %>' Visible="False" />
                         <asp:TextBox ID="WebID" runat="server" Text='<%# Bind("webID") %>' Visible="false" />
                         <asp:TextBox ID="MemberID" runat="server" Text='<%# Bind("MemberID") %>' Visible="false" />
                         <asp:TextBox ID="EventID" runat="server" Text='<%# Bind("EventID") %>' Visible="false" />
                       <asp:Checkbox ID="AddedToMealsOwed" runat="server" Checked='<%# Bind("AddedToMealsOwed") %>' Visible="false" />
                        <asp:Button Text="Delete" runat="server" CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" Width="120px" height="30px" OnClientClick = "DConfirm()" />
    
                    </itemTemplate>

                    <FooterTemplate>
                        <asp:TextBox ID="GLastName" runat="server" Width="185px" Text='<%# Bind("GLastName") %>' />
                        <asp:TextBox ID="GFirstName" runat="server" Width="180px" Text='<%# Bind("GFirstName") %>'/>
                        <asp:DropDownList ID="GuestMeal" runat="server" Width="190px" DataSourceID="MealSource" DataValueField="Category" DataTextField="Meal"></asp:DropDownList> 
                        <asp:Button Text="Add" runat="server" Width="120px" height="30px" CommandName="Insert" />
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
			</asp:Panel>
        
        <br /><asp:Table runat="server"><asp:TableFooterRow ><asp:TableCell>
        		<asp:Button ID="Review" runat="server" Text="Review and Pay" Width="183px" height="40px"/></asp:TableCell>
            <asp:tablecell><asp:TableCell Width="50px"></asp:TableCell>
               <span style="padding-left:70px"> <asp:Button ID="Cancel" runat="server" height="40px" Text="Cancel Reservation"  OnClick = "OnConfirm" OnClientClick="Confirm()"/></span>
            </asp:tablecell>
                         </asp:TableFooterRow></asp:Table><br /><br />
       <asp:Panel ID="Panel3" runat="server" height="90px"  width="715px"  BorderStyle="Solid" BorderWidth="1px" Visible="false">
           
           <asp:Table ID="Table1" runat="server">

               <asp:TableRow>
                   <asp:TableCell>
                       <asp:CheckBox ID="chkDues" runat="server" visible="false"/>
                                       </asp:TableCell>
                   <asp:TableCell>
                       <asp:Label ID="lbDuesOwed" runat="server" Text="Label" Visible="false"></asp:Label>
                   </asp:TableCell>

               </asp:TableRow>
               <asp:TableRow>

               </asp:TableRow> <asp:TableRow>

               </asp:TableRow>
               <asp:TableRow>
                   <asp:TableCell>
                        <asp:CheckBox ID="chkMeals" runat="server" visible =" false"/>
                    </asp:TableCell>
                   <asp:TableCell>
                       <asp:Label ID="lbMealsOwed" runat="server" Text="Label" Visible="false"></asp:Label>
                   </asp:TableCell>

               </asp:TableRow>
           </asp:Table>
            <asp:Label ID="Message" runat="server" cssCLASS="pHeader2" Text="" /><br />
      
     </asp:Panel>
        
    
        
       <asp:Label ID="lbError" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="Black"
           Height="42px" Width="603px"></asp:Label><br />
       <br />

          <asp:SqlDataSource ID="SQLDataSource1" runat="server"   SelectCommand="select ID, Meal, Category from  dbo.EventMealCategory(@EventID) where not meal is null  order by place">

              <SelectParameters>
             <asp:SessionParameter
                 Name="EventID"
                 SessionField="EventID" />
         </SelectParameters>
          </asp:SqlDataSource>

    
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="AdditionalContent" runat="server">
</asp:Content>
