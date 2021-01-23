<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/member.master" CodeBehind="ChangePassword.aspx.vb" Inherits="CCFRW19.ChangePassword" 
    title="Change Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	
	

</asp:Content>

   <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
       
<div class="pPage" style="height: 337px"; >
    <span class="pHeader">Change User Name and Password</span><br /><br />
	<br /> <br/>
	
	<table style="width: 100%">
		<tr>
			<td style="width: 186px" align="right" >Current User Name:</td>
			<td>&nbsp;<asp:TextBox id="CuserName" runat="server" Width="183px"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td style="width: 186px" align="right" >New User Name:</td>
			<td>&nbsp;<asp:TextBox id="NUserName"   runat="server" Width="182px" ></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td style="width: 186px" align="right" ></td>
			<td>&nbsp;
			</td>
		</tr><tr>
			<td style="width: 186px" align="right" >Current Pasword:</td>
			<td>&nbsp;<asp:TextBox id="CurrentPassword" textmode="Password" runat="server" Width="183px"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td style="width: 186px" align="right" >New Password:</td>
			<td>&nbsp;<asp:TextBox id="NewPassword" textmode="Password"   runat="server" Width="181px" ></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td style="width: 186px" align="right" >Confirm New Password:</td>
			<td>&nbsp;<asp:TextBox id="Confirm" textmode="Password" runat="server" Width="181px"></asp:TextBox>
			</td>
		</tr>
	</table><br /><br />
	
    <asp:Button ID="btnSubmit" runat="server" Text="Submit" Height="33px" Width="114px" /><br />
    <br />
    <asp:Label ID="msg" runat="server" Font-Bold="True" Font-Size="Larger" Height="32px"
        Text=" " Width="477px"></asp:Label></div>
</asp:Content>
