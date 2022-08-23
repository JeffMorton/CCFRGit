<%@ Page Title="" Language="vb" AutoEventWireup="false"EnableEventValidation="true"  MasterPageFile="~/admin.Master" CodeBehind="EventSignUp.aspx.vb" Inherits="CCFRW19.EventSignUp" %>
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
         <script type = "text/javascript">
             function CConfirm() {
                 var confirm_value = document.createElement("INPUT");
                 confirm_value.type = "hidden";
                 confirm_value.name = "confirm_value";
                 if (confirm("Do you want to cancel this Reservation?")) {
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
  <div class="pPage"   >
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <strong>Event Date:</strong> <asp:Label ID="EventDate" runat="server" Text="Label"></asp:Label>
        <strong>Meal Cost:</strong> <asp:Label ID="Cost" runat="server" Text="Label"></asp:Label>
        <strong>Type:</strong> <asp:label ID="Type" runat="server" Text="Label"></asp:label>
        <strong>Event ID:</strong> <asp:Label ID="hEventID" runat="server" Text=""></asp:Label>
        <strong>Member ID:</strong> <asp:Label ID="hMemberID" runat="server" Text=""></asp:Label>

        <br /><br />
            <strong>Select Member:</strong> <asp:DropDownList ID="MemNameDLL" AutoPostBack="true" runat="server" Height="27px" Width="347px" DataSourceID="NameSource" OnSelectedIndexChanged="MemNameDLL_Index_Changed"
        DataTextField="RosterName"
        DataValueField="ID">
    </asp:DropDownList>
    <asp:SqlDataSource ID="NameSource" runat="server" selectcommand="select RosterName, Id from member order by lastname,firstname "></asp:SqlDataSource>
      
      <asp:Panel ID="MainPanel" width="715px" height="600px"  runat="server">
    
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
                         tmpMemberSignUp ON Member.ID = tmpMemberSignUp.MemberID where member.id=@UserID and tmpMembersignup.Eventid = @Eventid"
            updatecommand="Update tmpmembersignup set MemberAttend = @MemberAttend, SpouseAttend = @SpouseAttend, MemberMeal = @Membermeal,SpouseMeal=@SpouseMeal where ID = @ID">
        <SelectParameters>
            <asp:sessionparameter  SessionField="UserID" Name=UserID />
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
                        <asp:Label ID="Label1" runat="server" Width="195px" Text="Last Name" />
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
                        <asp:Button Text="Delete" runat="server" CommandArgument='<%# Eval("ID") %>'  CommandName="Delete" Width="120px" height="30px" OnClientClick = "Confirm()" />
    
                    </itemTemplate>

                    <FooterTemplate>
                        <asp:TextBox ID="GLastName" runat="server" Width="185px" Text='<%# Bind("GLastName") %>' />
                        <asp:TextBox ID="GFirstName" runat="server" Width="180px" Text='<%# Bind("GFirstName") %>'/>
                        <asp:DropDownList ID="GuestMeal" runat="server" Width="190px" DataValueField="MealCategory" DataTextField="Meal"></asp:DropDownList> 
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

       <asp:Panel ID="Panel3" runat="server" height="145px"  width="350px"  BorderStyle="Solid" BorderWidth="1px">
           <asp:FormView ID="AccForm" runat="server" DataKeyNames="tID" Width=" 350px"  DataSourceID="ACCSource">
               
               <InsertItemTemplate>
                   <asp:Table ID="Table1" runat="server">
                       <asp:TableRow>
                           <asp:TableCell>
                               <asp:Label ID="Label9" runat="server" Width="90px" Text="Meals Owed:" CssClass="pNormalTextRight" />
                               <asp:TextBox ID="MealsOwed" Width="65px" runat="server" Text='<%# Session("MealsOwed") %>' />
                           </asp:TableCell><asp:TableCell>
                               <asp:Label ID="Label16" runat="server" Width="90px" Text="Use:" CssClass="pNormalTextRight" />
                               <asp:CheckBox ID="UseMealsOwed" runat="server" />
                           </asp:TableCell></asp:TableRow><asp:TableRow>
                           <asp:TableCell>
                               <asp:Label ID="Label10" runat="server" Width="90px" Text="Dues Owed:" CssClass="pNormalTextRight" />
                               <asp:Label ID="DuesOwed" Width="65px" runat="server" Text='<%# Session("DuesOwed") %>' />
                           </asp:TableCell><asp:TableCell>
                               <asp:Label ID="Label17" runat="server" Width="90px" Text="Pay Dues:" CssClass="pNormalTextRight" />
                               <asp:CheckBox ID="PayDues" Width="30px"  runat="server" />
                           </asp:TableCell></asp:TableRow><asp:TableRow>
                               
                           <asp:TableCell>
                               <asp:Label ID="labTotalCost" runat="server" Width="110px" Text="Today's Charge:" CssClass="pNormalTextRight" />
                               <asp:label ID="TotalCost" Width="65px" runat="server" Text='<%# Session("TodayCharges") %>' />
                          </asp:TableCell><asp:TableCell>
                               <asp:Label ID="Label13" runat="server" Width="90px" Text="No Refunds:" CssClass="pNormalTextRight" />
                               <asp:CheckBox ID="NoRefund" Width="30px"  runat="server" />
                           </asp:TableCell></asp:TableRow><asp:TableRow>
                           <asp:TableCell>
                               <asp:Label ID="Label14" runat="server" Width="105px" Text=" " CssClass="pNormalTextRight" />
                           </asp:TableCell></asp:TableRow><asp:TableRow>
                           <asp:TableCell>
                               <asp:Label ID="Label11" runat="server" Width="90px" Text="Check No:" CssClass="pNormalTextRight" />
                               <asp:TextBox ID="CheckNumber" Width="75px" Text='<%# Bind("tCheckNumber") %>' runat="server" />
                           </asp:TableCell><asp:TableCell>
                               <asp:Label ID="Label12" runat="server" Width="75px" Text="Amount:" CssClass="pNormalTextRight" />
                              <asp:TextBox ID="CheckAmount" Width="75px" Text='<%# Bind("tCheckAmount", "{0:c}") %>' runat="server" />
                           </asp:TableCell></asp:TableRow>
                       
                   </asp:Table></InsertItemTemplate></asp:FormView>
           <asp:SqlDataSource ID="AccSource" runat="server"
               InsertCommand="INSERT INTO [dbo].[Account] (tType,tChecknumber,tCheckamount,tPayee,tCategory,tEventDate,tDateEntered) values ('D',@tCheckNumber,@tCheckAmount,@tPayee,@Category,@EventDate,@DateEntered)">
               <SelectParameters>
            <asp:sessionparameter  SessionField="UserID" Name=MemberID />
            
        </SelectParameters> 
                               <InsertParameters>
                                   <asp:sessionparameter sessionfield="FullName" name=tpayee></asp:sessionparameter>
                                   <asp:SessionParameter SessionField="Category" Name =Category />
                                   <asp:SessionParameter SessionField="EventDate" name=EventDate />
                                  <asp:SessionParameter SessionField="DateEntered" name=DateEntered />
                               </InsertParameters>

               
           </asp:SqlDataSource></asp:Panel><br />
          <asp:Button ID="btnSave" runat="server" Text="Save" Width="140px" height="30px"  />
       <asp:Button ID="btnComputeCost" runat="server" Text="Compute Cost" Width="140px" height="30px" onclick="ComputeCost" />
          <asp:Button ID="Cancel" runat ="server" Text="Cancel Reservation" Width="140px" height="30px" OnClick="CancelRes" OnClientClick = "CConfirm()"/>
          <div style ="margin-left:357px; margin-top:-210px" >     
  <br />
  <asp:Panel ID="Panel4" width="30px" Height="250px" runat="server">
            <asp:GridView ID="PmtGridview" runat="server" AllowPaging="false" AlternatingRowStyle-BackColor="#C2D69B" AlternatingRowStyle-CssClass="pNormatext" AutoGenerateColumns="False" CssClass="pNormalText" DataKeyNames="tID" EmptyDataText="No data available." HeaderStyle-BackColor="Wheat" ShowHeaderWhenEmpty="true" Width="350" AllowSorting="True"><Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="Label1" runat="server" Text="Check Number" Width="160px" /><asp:Label ID="Label2" runat="server" Text="Date" Width="90px" /><asp:Label ID="Label3" runat="server" CssClass="pStrongTextRight" Text ="Amount" Width="90px" /></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Table ID="Table2" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="CheckNumber" Width="160px" runat="server" Text='<%# Bind("tCheckNumber") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="CheckDate" runat="server" Width="90px" Text='<%# Bind("tCheckDate", "{0:MM/dd/yyyy}") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="CheckAmount" runat="server" Width="90px" CssClass="pNormalTextRight" Text='<%# Bind("tCheckAmount", "{0:c}") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="tPayee" runat="server" Text='<%# Bind("tPayee") %>' Visible="false" />
                                    </asp:TableCell></asp:TableRow></asp:Table></ItemTemplate></asp:TemplateField></Columns></asp:GridView></asp:Panel></div><br /><br />
          <asp:Label ID="Message" runat="server" cssCLASS="pHeader2" Text="" /><br />
             

         </asp:Panel> 
     

       </div>

</asp:Content>