<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="MemberUpdate.aspx.vb" Inherits="CCFRW19.MemberUpdate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Delete Member  ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
     }
      </script>
     <script type = "text/javascript" >
         function alertuser(msg) {
             alert(msg);
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
 <div class="pPage">
 <span class="pHeader">Member Profile</span><br/>
     <br />

        <strong>Select Member:</strong> <asp:DropDownList ID="ddlSelectName" AutoPostBack="true" runat="server" Height="27px" Width="347px" DataSourceID="NameSource" OnSelectedIndexChanged="SelectName_Index_Changed"        DataTextField="RosterName"
        DataValueField="ID">
    </asp:DropDownList>
    <asp:SqlDataSource ID="NameSource" runat="server" selectcommand="select RosterName, Id from member order by lastname,firstname "></asp:SqlDataSource>
 <br /><br />

    
     
     <asp:Panel ID="pMain" runat="server">
 <asp:Panel BorderStyle="Groove" BorderColor="Black" Height="280px" Width =" 330px" BackColor="wheat" runat = "server">
  <asp:FormView ID="MemberForm" runat="server" DatakeyNames ="ID">
      <HeaderTemplate>
          <asp:Label ID ="headermember" runat="server" Text = "Member Information"  Width =" 350px" cssclass="pHeader2"></asp:Label>
          
          <br/>
         
      </HeaderTemplate>
      <EditItemTemplate>
          <asp:Table runat="server">


               <asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label4" runat="server" Text="Member ID:" width="105px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>

                      <asp:label ID="ID" runat="server" Text='<%# Bind("ID") %>'></asp:label>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label3" runat="server" Text="*First Name:" width="105px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>

                      <asp:TextBox ID="FirstName" runat="server" width="190px"  Text='<%# Bind("FirstName") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="LName" runat="server" Text="*Last Name:" width="105px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:TextBox ID="LastName" runat="server" width="190px"  Text='<%# Bind("LastName") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="LNick" runat="server" Text="Nick Name:" width="105px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:TextBox ID="NickName" runat="server" width="190px"  Text='<%# Bind("NickName") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label1" runat="server" Text="E-Mail:" width="105px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:TextBox ID="MEmail" runat="server" width="190px"  Text='<%# Bind("MEmail") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label8" runat="server" Text="Cell:" width="105px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="Cellphone" runat="server" Width="190px" Text='<%# Bind("CellPhone") %>'></asp:TextBox>
                             <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="Cellphone"
                                 Mask="999-999-9999"
                                 ClearMaskOnLostFocus="False" />
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label12" runat="server" Text="Position:" width="105px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:DropDownList ID="Position" width="195px" selectedValue='<%# Bind("Position") %>' runat="server"
                            DataSourceID="PositionSource"
                            DataValueField="OfficeName"
                            DataTextField="OfficeName">
                      </asp:DropDownList>
                   </asp:TableCell></asp:TableRow><asp:TableRow>
                   <asp:TableCell>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell></asp:TableCell><asp:TableCell>
                        <asp:Button ID="MComm" runat="server" Text="Committees Assignments" OnClick="MemberCommittees" width="200px"/></asp:TableCell>
                   </asp:TableRow><asp:TableRow>
                 <asp:TableCell>
                      <asp:Label ID="lAdministrator" runat="server" Text="Administrator:" width="105px" CssClass="pNormalTextRight" />
                      </asp:TableCell><asp:TableCell>
                      <asp:CheckBox ID="Admin" checked='<%# Bind("Admin")  %>' runat="server" />
                      
                           <asp:Label ID="Label" runat="server" Text="Print Nametag:" width="115px" CssClass="pNormalTextRight" />
                            <asp:CheckBox ID="PrintNameTag" checked='<%# Bind("PrintNameTag")  %>' runat="server" />

                        </asp:TableCell></asp:TableRow><asp:TableRow>   
                    <asp:TableCell>
                      <asp:Label ID="Label19" runat="server" Text="Vaccinated:" width="105px" CssClass="pNormalTextRight" />
                      </asp:TableCell><asp:TableCell>
                      <asp:CheckBox ID="MVaccinated" checked='<%# Bind("MVaccinated")  %>' runat="server" />
</asp:TableCell></asp:TableRow></asp:Table></EditItemTemplate></asp:FormView></asp:Panel><div style ="margin-left:355px; margin-top:-285px" >     
 
                      <asp:Panel BorderStyle="Groove" BorderColor="Black" Height="280px" Width =" 330px" BackColor="wheat" runat = "server">
                          <asp:FormView ID="SpouseForm" runat="server" DataKeyNames="ID">
                              <HeaderTemplate>
                                  <asp:Label ID="headermember" runat="server" Text="Member Information" Width=" 320px" CssClass="pHeader2"></asp:Label><br>
                                  <br />
                                  
                              </HeaderTemplate>
                              <EditItemTemplate>

                                  <asp:Table runat="server">
                                      <asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label2" runat="server" Text="First Name:" width="105px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>

                                              <asp:TextBox ID="SpouseFirstName" runat="server" width="190px" Text='<%# Bind("SpouseFirstName") %>'></asp:TextBox>
                                          </asp:TableCell></asp:TableRow><asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label4" runat="server" Text="Last Name:" width="105px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>
                                              <asp:TextBox ID="SpouseLastName" runat="server" width="190px" Text='<%# Bind("SpouseLastName") %>'></asp:TextBox>
                                          </asp:TableCell></asp:TableRow><asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label5" runat="server" Text="Nick Name:" width="105px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>
                                              <asp:TextBox ID="SpouseNickName" runat="server" width="190px" Text='<%# Bind("SpouseNickName") %>'></asp:TextBox>
                                          </asp:TableCell></asp:TableRow><asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label6" runat="server" Text="E-Mail:" width="105px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>
                                              <asp:TextBox ID="Email" runat="server" width="190px" Text='<%# Bind("SpouseEMail") %>'></asp:TextBox>
                                          </asp:TableCell></asp:TableRow><asp:TableRow>
                                         <asp:TableCell>
                                             <asp:Label ID="Label8" runat="server" Text="Cell:" width="105px" CssClass="pNormalTextRight" />
                                         </asp:TableCell><asp:TableCell>
                                             <asp:TextBox ID="SCellphone" runat="server" Width="190px" Text='<%# Bind("SCellPhone") %>'></asp:TextBox>
                                             <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="SCellphone"
                                                 Mask="999-999-9999"
                                                 ClearMaskOnLostFocus="False" /></asp:Tablecell></asp:Tablerow>
                                      
                                      <asp:Tablerow><asp:TableCell><asp:Label ID="Label12" runat="server" Text="Position:" width="105px" CssClass="pNormalTextRight" /></asp:TableCell> <asp:TableCell> 
                                          <asp:DropDownList ID="SPosition" width="195px" selectedValue='<%# Bind("SPosition") %>' runat="server"
                                                DataSourceID="PositionSource"
                                                DataValueField="OfficeName"
                                                DataTextField="OfficeName">
                                          </asp:DropDownList>
                                       </asp:TableCell></asp:TableRow><asp:TableRow>
                                       
                   
                        <asp:TableCell></asp:TableCell><asp:TableCell>
                        <asp:Button ID="MComm" runat="server" Text="Committees Assignments" OnClick="SpouseCommittees" width="200px"/></asp:TableCell>
                                         </asp:TableRow><asp:TableRow>
                 <asp:TableCell>
                      <asp:Label ID="lAdministrator" runat="server" Text="Administrator:" width="105px" CssClass="pNormalTextRight" />
                      </asp:TableCell><asp:TableCell>
                      <asp:CheckBox ID="SAdmin" checked='<%# Bind("SAdmin")  %>' runat="server" />
                      
                           <asp:Label ID="Label" runat="server" Text="Print Nametag:" width="115px" CssClass="pNormalTextRight" />
                            <asp:CheckBox ID="SpousePrintNameTag" checked='<%# Bind("SpousePrintNameTag")  %>' runat="server" />

                        </asp:TableCell></asp:TableRow><asp:TableRow>   
                    <asp:TableCell>
                      <asp:Label ID="Label19" runat="server" Text="Vaccinated:" width="105px" CssClass="pNormalTextRight" />
                      </asp:TableCell><asp:TableCell>
                      <asp:CheckBox ID="SVaccinated" checked='<%# Bind("SVaccinated")  %>' runat="server" />
</asp:TableCell></asp:TableRow></asp:Table></EditItemTemplate></asp:FormView></asp:Panel></div><br/><br/><asp:Panel BorderStyle="Groove" BorderColor="Black" Height="290px" Width =" 580px"  BackColor="wheat" runat = "server">

         <asp:FormView ID="AddressForm" runat="server" DataKeyNames="ID">
             <HeaderTemplate>
                 <asp:Label ID="headermember" runat="server" Text="General Information" Width=" 600px"   CssClass="pHeader2"></asp:Label><br>
                 <br />
             </HeaderTemplate>
             <EditItemTemplate>

                 <asp:Table runat="server">
                     <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label2" runat="server" Text="Address:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>

                             <asp:TextBox ID="Address" runat="server" Width="300px" Text='<%# Bind("Address") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label4" runat="server" Text="City:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="City" runat="server" Width="300px" Text='<%# Bind("City") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label5" runat="server" Text="State:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="State" runat="server" Width="300px" Text='<%# Bind("State") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label6" runat="server" Text="Zip:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="Zip" runat="server" Width="300px" Text='<%# Bind("Zip") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label7" runat="server" Text="Telephone:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="Telephone" runat="server" Width="300px" Text='<%# Bind("Telephone") %>'></asp:TextBox>

                             <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="Telephone"
                                 Mask="999-999-9999"
                                 ClearMaskOnLostFocus="False" />

                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label10" runat="server" Text="*Roster Name:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="RosterName" runat="server" Width="300px" Text='<%# Bind("RosterName") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label11" runat="server" Text="Envelope Name:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="EnvelopeName" runat="server" Width="300px" Text='<%# Bind("EnvelopeName") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label13" runat="server" Text="Combined Nick Name:" width="190px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="CombinedNickName" runat="server" Width="300px" Text='<%# Bind("CombinedNickName") %>'></asp:TextBox>
                         </asp:TableCell></asp:TableRow><asp:TableRow><asp:TableCell>
                             <asp:Label ID="Label17" runat="server" Text="Dues Owed:" width="190px" CssClass="pNormalTextRight" />
                </asp:tablecell><asp:tablecell>
                             <asp:TextBox ID="DuesOwed" runat="server" Width="80px" Text='<%#  Bind("DuesOwed", "{0:N2}") %>'></asp:TextBox>
                             <asp:Label ID="Label18" runat="server" Text="Meals Owed:" width="100px" CssClass="pNormalTextRight" />
                             <asp:TextBox ID="TextBox1" runat="server" Width="80px" Text='<%# Bind("MealsOwed", "{0:N2}") %>'></asp:TextBox>

                </asp:TableCell></asp:TableRow><asp:TableRow>   
                  <asp:TableCell>
                      <asp:Label ID="Label14" runat="server" Text="New Member:" width="190px" CssClass="pNormalTextRight" />
                      </asp:TableCell><asp:TableCell>
                      <asp:CheckBox ID="NewMember" checked='<%# Bind("NewMember")  %>' runat="server" />
                           <asp:Label ID="Label15" runat="server" Text="No Reports:" width="115px" CssClass="pNormalTextRight" />
                      <asp:CheckBox ID="NoReports" checked='<%# Bind("DontincludeonReports")  %>' runat="server" />
                               <asp:Label ID="Label22" runat="server" Text="Email Only:" width="115px" CssClass="pNormalTextRight" />
                      <asp:CheckBox ID="EmailOnly" checked='<%# Bind("EmailOnly")  %>' runat="server" />

             </asp:TableCell></asp:TableRow></asp:Table></EditItemTemplate></asp:FormView></asp:Panel><br/><asp:Button ID="UpdateM" runat="server" Text="Save Changes"  Width="120px" height="30px"/>
                    <asp:Button ID="AddMember" runat="server" Text="Add New Member"  Onclick= "AddMem" Width="120px" height="30px"/>
                    <asp:Button ID="MailLogin" runat="server" Text="Send Login Info" Width="120px" height="30px"/>
                    <asp:Label ID="Label16" runat="server" Text="* indicated required fields"></asp:Label><asp:Button ID="DeleteM"  runat="server" OnClick = "OnConfirm" Width="120px" height="30px" Text = "Delete Member" OnClientClick = "Confirm()"/>
</asp:panel>
        <asp:Label ID="lblMessage" runat="server"  width="715px" Text=""></asp:Label></div><asp:SqlDataSource ID="memSource" runat="server" 
        SelectCommand="SELECT * FROM [Member] WHERE ([ID] = @ID)" 
        UpdateCommand= "Update Member set firstname = @firstname, lastname = @lastname, NickName = @Nickname,[E-Mail] = @MEmail, Position= @Position,
                        Cellphone = replace(replace(@Cellphone,'-',''),'_','') ,
                      Admin= @Admin, PrintNameTag = @PrintNameTag, MVaccinated = @MVaccinated   where [ID] = @ID" >
        <SelectParameters>
            <asp:SessionParameter SessionField="UserID" Name="ID" ></asp:SessionParameter>
        </SelectParameters>
        
        
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SSource" runat="server"
        SelectCommand="SELECT * FROM [Member] WHERE ([ID] = @ID)"
        UpdateCommand="Update Member set  Spousefirstname = @Spousefirstname, Spouselastname = @Spouselastname,SPosition = @SPosition,SAdmin = @SAdmin,SVaccinated = @SVaccinated, SCellphone = replace(replace(@SCellphone,'-',''),'_','') ,
                    SpouseNickName = @SpouseNickname,SpouseEmail = @SpouseEmail, SpousePrintNameTag = @SpousePrintNameTag where [ID] = @ID">
        <SelectParameters>
            <asp:SessionParameter SessionField="UserID" Name="ID"></asp:SessionParameter>
        </SelectParameters>

        
    </asp:SqlDataSource>

  
  <asp:SqlDataSource ID="AddressSource" runat="server" 
        SelectCommand="SELECT * FROM [Member] WHERE ([ID] = @ID)" 
        UpdateCommand= "Update Member set  Address= @Address, city= @city, state = @state, Zip=@Zip, 
         Telephone = replace(@Telephone,'-',''),  RosterName = @RosterName, NewMember = @NewMember,DontIncludeonReports = @DontIncludeonReports,
         EnvelopeName = @EnvelopeName, CombinedNickName = @CombinedNickName,DuesOwed=@DuesOwed, MealsOwed = @MealsOwed, EmailOnly =  @EmailOnly  where [ID] = @ID" >
        <SelectParameters>
            <asp:SessionParameter SessionField="UserID" Name="ID" ></asp:SessionParameter>
        </SelectParameters>
        
       
    </asp:SqlDataSource>
    <asp:SqlDataSource ID= "PositionSource" runat="server" SelectCommand="select id,OfficeName from offices order by officeorder" />
    <asp:SqlDataSource ID= "ProgramCommitteeSource" runat="server" SelectCommand="select id, Position from ProgramCommitteeMembership order by ID" />
    <asp:SqlDataSource ID= "LunchCommitteeSource" runat="server" SelectCommand="select id, Position from LunchCommitteeMembership order by ID" />


  
</asp:Content>
