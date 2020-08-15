<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/member.Master" CodeBehind="profile.aspx.vb" Inherits="CCFRW19.profile" %>
 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

       <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   

 <div class="pPage">
     <span class="pHeader">Member Profile</span><br/>
     <br />
     
  
 <asp:Panel BorderStyle="Groove" BorderColor="Black" Height="220px" Width =" 350px" BackColor="wheat" runat = "server">
  <asp:FormView ID="MemberForm" runat="server" DatakeyNames ="ID">
      <HeaderTemplate>
          <asp:Label ID ="headermember" runat="server" Text = "Member Information"  Width =" 300px" cssclass="pHeader2"></asp:Label>
          <br>
          <br/>
         
      </HeaderTemplate>
      <EditItemTemplate>
          <asp:Table runat="server">


               <asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label4" runat="server" Text="Member ID:" width="85px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>

                      <asp:label ID="MID" runat="server" Text='<%# Bind("ID") %>'></asp:label>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label3" runat="server" Text="First Name:" width="85px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>

                      <asp:TextBox ID="FirstName1" runat="server" width="240px"  Text='<%# Bind("FirstName") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="LName" runat="server" Text="Last Name:" width="85px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:TextBox ID="LastName1" runat="server" width="240px"  Text='<%# Bind("LastName") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="LNick" runat="server" Text="Nick Name:" width="85px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:TextBox ID="NickName1" runat="server" width="240px"  Text='<%# Bind("NickName") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow><asp:TableRow>
                  <asp:TableCell>
                      <asp:Label ID="Label1" runat="server" Text="E-Mail:" width="85px" CssClass="pNormalTextRight" />
                  </asp:TableCell><asp:TableCell>
                      <asp:TextBox ID="MEmail" runat="server" width="240px"  Text='<%# Bind("MEmail") %>'></asp:TextBox>
                  </asp:TableCell></asp:TableRow>
                <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label8" runat="server" Text="Cell:" Width="85px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="Cellphone" runat="server" width="240px" Text='<%# Bind("CellPhone") %>'></asp:TextBox>
                             <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="Cellphone"
                                 Mask="999-999-9999"
                                 ClearMaskOnLostFocus="False" />
                         </asp:TableCell>
                     </asp:TableRow>
          </asp:Table></EditItemTemplate></asp:FormView></asp:Panel>
     <div style ="margin-left:360px; margin-top:-240px" >     
  <br/>
                      <asp:Panel BorderStyle="Groove" BorderColor="Black" Height="220px" Width =" 350px" BackColor="wheat" runat = "server">
                          <asp:FormView ID="SpouseForm" runat="server" DataKeyNames="ID">
                              <HeaderTemplate>
                                  <asp:Label ID="headermember" runat="server" Text="Member Information" Width=" 300px" CssClass="pHeader2"></asp:Label><br>
                                  <br />
                                  <br>
                              </HeaderTemplate>
                              <EditItemTemplate>

                                  <asp:Table runat="server">
                                      <asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label2" runat="server" Text="First Name:" Width="85px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>

                                              <asp:TextBox ID="FirstName" runat="server" width="240px" Text='<%# Bind("SpouseFirstName") %>'></asp:TextBox>
                                          </asp:TableCell>
                                      </asp:TableRow>
                                      <asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label4" runat="server" Text="Last Name:" Width="85px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>
                                              <asp:TextBox ID="LastName" runat="server" width="240px" Text='<%# Bind("SpouseLastName") %>'></asp:TextBox>
                                          </asp:TableCell>
                                      </asp:TableRow>
                                      <asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label5" runat="server" Text="Nick Name:" Width="85px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>
                                              <asp:TextBox ID="NickName" runat="server" width="240px" Text='<%# Bind("SpouseNickName") %>'></asp:TextBox>
                                          </asp:TableCell>
                                      </asp:TableRow>
                                      <asp:TableRow>
                                          <asp:TableCell>
                                              <asp:Label ID="Label6" runat="server" Text="E-Mail:" Width="85px" CssClass="pNormalTextRight" />
                                          </asp:TableCell><asp:TableCell>
                                              <asp:TextBox ID="Email" runat="server" width="240px" Text='<%# Bind("SpouseEMail") %>'></asp:TextBox>
                                          </asp:TableCell>
                                      </asp:TableRow>
                                      <asp:TableRow>
                                     <asp:TableCell>
                                         <asp:Label ID="Label8" runat="server" Text="Cell:" Width="85px" CssClass="pNormalTextRight" />
                                     </asp:TableCell><asp:TableCell>
                                         <asp:TextBox ID="SCellphone" runat="server" width="240px" Text='<%# Bind("SCellPhone") %>'></asp:TextBox>
                                         <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="SCellphone"
                                             Mask="999-999-9999"
                                             ClearMaskOnLostFocus="False" />
                                     </asp:TableCell>
                                 </asp:TableRow>
                                  </asp:Table>
                              </EditItemTemplate>
                          </asp:FormView></asp:Panel></div><br/><br/>
     <div style ="margin-left:60px"  > 
     <asp:Panel BorderStyle="Groove" BorderColor="Black" Height="200px" Width =" 580px" BackColor="wheat" runat = "server">
         <asp:FormView ID="AddressForm" runat="server" DataKeyNames="ID">
             <HeaderTemplate>
                 <asp:Label ID="headermember" runat="server" Text="Address and Phone Numbers" Width=" 650px" CssClass="pHeader2"></asp:Label><br>
                 <br />
             </HeaderTemplate>
             <EditItemTemplate>

                 <asp:Table runat="server">
                     <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label2" runat="server" Text="Address:" Width="90px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>

                             <asp:TextBox ID="Address" runat="server" Width="450px" Text='<%# Bind("Address") %>'></asp:TextBox>
                         </asp:TableCell>
                     </asp:TableRow>
                     <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label4" runat="server" Text="City:" Width="90px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="City" runat="server" Width="450px" Text='<%# Bind("City") %>'></asp:TextBox>
                         </asp:TableCell>
                     </asp:TableRow>
                     <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label5" runat="server" Text="State:" Width="90px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="State" runat="server" Width="450px" Text='<%# Bind("State") %>'></asp:TextBox>
                         </asp:TableCell>
                     </asp:TableRow>
                     <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label6" runat="server" Text="Zip:" Width="90px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="Zip" runat="server" Width="450px" Text='<%# Bind("Zip") %>'></asp:TextBox>
                         </asp:TableCell>
                     </asp:TableRow>
                     <asp:TableRow>
                         <asp:TableCell>
                             <asp:Label ID="Label7" runat="server" Text="Telephone:" Width="90px" CssClass="pNormalTextRight" />
                         </asp:TableCell><asp:TableCell>
                             <asp:TextBox ID="Telephone" runat="server" Width="450px" Text='<%# Bind("Telephone") %>'></asp:TextBox>

                             <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="Telephone"
                                 Mask="999-999-9999"
                                 ClearMaskOnLostFocus="False" />

                         </asp:TableCell>
                     </asp:TableRow>
                     
                 </asp:Table>
             </EditItemTemplate>
         </asp:FormView></asp:Panel></div><br/></div>
                    <asp:Button ID="UpdateM" runat="server" Text="Save Changes"  Height="33px" Width="110px"/>
                    
    
    <asp:SqlDataSource ID="memSource" runat="server" 
        SelectCommand="SELECT * FROM [Member] WHERE ([ID] = @ID)" 
        UpdateCommand= "Update Member set firstname = @firstname, lastname = @lastname, NickName = @Nickname,[E-Mail] = @MEmail, Cellphone = replace(replace(@Cellphone,'-',''),'_','')    where [ID] = @ID" >
        <SelectParameters>
            <asp:SessionParameter SessionField="Userid" Name="ID" ></asp:SessionParameter>
        </SelectParameters>
        
      
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SSource" runat="server"
        SelectCommand="SELECT * FROM [Member] WHERE ([ID] = @ID)"
        UpdateCommand="Update Member set  Spousefirstname = @Spousefirstname, Spouselastname = @Spouselastname, SpouseNickName = @SpouseNickname,SpouseEmail = @SpouseEmail, SCellphone = replace(replace(@SCellphone,'-',''),'_','')  where [ID] = @ID">
        <SelectParameters>
            <asp:SessionParameter SessionField="Userid" Name="ID"></asp:SessionParameter>
        </SelectParameters>

        
    </asp:SqlDataSource>

  
     <asp:SqlDataSource ID="AddressSource" runat="server" 
        SelectCommand="SELECT * FROM [Member] WHERE ([ID] = @ID)" 
        UpdateCommand= "Update Member set  Address= @Address, city= @city, state = @state, Zip=@Zip, Telephone = replace(@Telephone,'-','')   where [ID] = @ID" >
        <SelectParameters>
            <asp:SessionParameter SessionField="Userid" Name="ID" ></asp:SessionParameter>
        </SelectParameters>
        
        
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
    </asp:Content>
	