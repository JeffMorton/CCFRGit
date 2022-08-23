<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/member.master" CodeBehind="PassRecover.aspx.vb" Inherits="CCFRW19.PassRecover" 
    title="Password Recovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="pPage" style =" height: 175px;">
	<div class="pHeader">Password Recovery</div><br /><br />
	
	Enter your E-Mail Address and click Submit.  You will then be asked to answer a Security Question.  After you answer this question, an E-Mail will be sent to you with your password.
<br /><br />
	<asp:Label id="LbUsername" runat="server" Text="   Enter E-Mail Address: " AssociatedControlID="Textbox1" ></asp:Label>
	<asp:textbox id="TextBox1" runat="server" Width="259px" CssClass="auto-style7" ></asp:textbox>
	<asp:Button id="Button1" runat="server" Text="Submit" Height="24px" /><br /><br />
	
	
	<asp:Label id="lbSecurityQuestion" runat="server" AssociatedControlID="SecurityQuestion" Text="Security Question: " Visible="False"></asp:Label>
	<asp:Label id="SecurityQuestion" runat="server" Visible="False" Width="307px" ></asp:Label><br /><br />
	<asp:Label id="Answer" runat="server" Text=" Security Answer: " AssociatedControlID="SQAnswer" Visible="False"></asp:Label>
	
	
	<asp:TextBox id="SQAnswer" runat="server" Visible="False" Width="254px" CssClass="auto-style6"></asp:TextBox>
    <asp:Button ID="Button2" runat="server" Text="Submit" Visible="False" /><br />
    <asp:Label ID="CorrectAnswer" runat="server" Width="336px" Visible="False"></asp:Label><br />
    <asp:TextBox ID="Password" runat="server" Visible="False"></asp:TextBox>
    <asp:TextBox id="EMailAddress" runat="server" Height="18px" Visible="False"></asp:TextBox>
    <asp:Label ID="MessageNotification" runat="server" Width="504px" Font-Bold="True" Font-Size="Large"></asp:Label></div>
    <asp:Label ID="username" runat="server" Visible="False" Width="190px"></asp:Label>
        </asp:Content>
