<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Publ.Master" AutoEventWireup="true" CodeBehind="ContactUs.aspx.vb" Inherits="CCFRW19.ContactUS" %>
<%@ Import Namespace="System.Net.Mail" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
  
	<div class="pPage"; style="height: 896px;">
	<p class="auto-style4" style="height: 27px; width: 577px">
	<span class="pHeader">Contact Us:</span></p>
		
		<div style="font-size:medium;font-weight:bold;font-style:normal;color:#C42100">By Mail:
			
	
			
	</div>
	
		<span class="pNormalText">
			
	
			
	<strong>&nbsp;&nbsp;&nbsp;
	<br />
		<br />
&nbsp;&nbsp;&nbsp; Charlottesville Committee on Foreign Relations<br />
  	 &nbsp;&nbsp;&nbsp;
  	 P.O. Box 4303<br />
 	 &nbsp;&nbsp;&nbsp;
 	 Charlottesville, VA 22905</strong><br />
	<br />

	
	</span>
		<div style="font-size:medium;font-weight:bold;font-style:normal;color:#C42100">By Telephone:</div>&nbsp;<br />
<span class="pNormalText">

	&nbsp;&nbsp;&nbsp; <strong>434-923-9185</strong><br />
	<br />
	Note - this is a Voice Mail number. Please leave a message and 	someone 
	<br />
	will return your call soon.<br />
		<br />
		</span>

	
	
		<div style="font-size:medium;font-weight:bold;font-style:normal;color:#C42100">By E-Mail:</div>
		<span class="pNormalText">
		<asp:MultiView ID="YourForm" runat="server" ActiveViewIndex="0"  >
            <asp:View ID="FormContent" runat="server" >
              
            <label for="First_Name">
                    <span style="color:black"><br />
				First Name:</span>
                    <asp:TextBox ID="First_Name" runat="server" Columns="35"></asp:TextBox>
                </label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="First_Name"
                    ErrorMessage="Please enter your first name." SetFocusOnError="True" CssClass="ValidateMessage"
                    ForeColor="">* Required</asp:RequiredFieldValidator>
                <br />
                
            <label for="Last_Name">
                    Last Name:
                    <asp:TextBox ID="Last_Name" runat="server" Columns="35">
                    </asp:TextBox>
                </label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Last_Name"
                    ErrorMessage="Please enter your last name." SetFocusOnError="True" CssClass="ValidateMessage"
                    ForeColor="">* Required</asp:RequiredFieldValidator>
                <br />
               
           <label for="Phone">
                    Phone Number:
                    <asp:TextBox ID="Phone" runat="server" Columns="20">
                    </asp:TextBox>
                </label>
       <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Phone"
                    Display="Dynamic" ErrorMessage="Please enter your phone number." SetFocusOnError="True"
                    CssClass="ValidateMessage" ForeColor="">* Required</asp:RequiredFieldValidator>        
       <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Phone"
                    ErrorMessage="Please enter a valid U.S. phone number (including dashes)." SetFocusOnError="True"
                    ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" CssClass="ValidateMessage"
                    ForeColor="">* Please enter phone number (including dashes).</asp:RegularExpressionValidator>       
 					<br />

             <label for="Email">
                    Email Address:
                    <asp:TextBox ID="Email" runat="server" Columns="35">
                    </asp:TextBox>
                </label>
   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Email"
                    Display="Dynamic" ErrorMessage="Please enter your email address." SetFocusOnError="True"
                    CssClass="ValidateMessage" ForeColor="">* Required</asp:RequiredFieldValidator>
     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Email"
                    ErrorMessage="Please enter a valid email address." SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    CssClass="ValidateMessage" ForeColor="">* Please enter a valid email address.</asp:RegularExpressionValidator>
                    <br />
           <label for="Subject">
                    Subject:
                    <asp:TextBox ID="Subject" runat="server" Columns="50">
                    </asp:TextBox>
                </label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Subject"
                    ErrorMessage="Please enter a subject." SetFocusOnError="True" CssClass="ValidateMessage"
                    ForeColor="">* Required</asp:RequiredFieldValidator>
                <br />
                
                        <label for="Message">
                    Please type your message below:
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Message"
                        ErrorMessage="Please enter a message" SetFocusOnError="True" CssClass="ValidateMessage"
                        ForeColor="">* Required</asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="Message" runat="server" TextMode="MultiLine" Columns="55" Rows="10">
                    </asp:TextBox>
                </label>
                <br />
				 <asp:CheckBox ID="CheckBoxCC" runat="server" Text="Send me a carbon copy of this email." />
                <br />
                <asp:Button ID="Submit" runat="server" Text="Submit Form" onclick="SubmitForm_Click"/>
                <br /> 
            </asp:View>
            <asp:View ID="FormConfirmationMessage" runat="server">
                Your message has been sent. Thank you for contacting us.<br />
            </asp:View>
            <asp:View ID="FormErrorMessage" runat="server">
                Due to technical difficulty, your message may NOT have been sent.
               
            </asp:View>
            
        </asp:MultiView>
  </span>
	</div>
	
</asp:Content>
  
  
