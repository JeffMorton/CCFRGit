<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Admin.aspx.vb" Inherits="CCFRW19.Admin1" %>

<!DOCTYPE html>
    <link href="main.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <style type="text/css">
        .auto-style2 {
            font-size: medium;
            height: 53px;
            font-style: normal;
            background: #325984;
           
        }
       

.auto-style3 {
	margin-top: -3px;
	margin-bottom: -3px;
}
</style>
</head>
<body>

        <asp:Label ID="Label1" runat="server" Text="Label"
            Width="54px" Visible="false"></asp:Label>
    <form id="form1" runat="server">
       <div class="pPage1"
           style="height: 120px; background-color:white; color:white;  font-family: Arial, Helvetica, sans-serif; font-size: x-large; font-style: italic; font-weight: bold; position: relative; left: 1px; top: 3px;">
<div style="height: 109px; background: #325984; width: 890px;margin-top:10px">
	        <img alt="RightHeader" height="115" src="Images/2_325884_g15.jpg" width="130" style="float: right" class="auto-style3" />
			<img alt="LeftHeader" height="115" src="Images/1_325884_g15.jpg" width="111" style="float: left;  left: 11px; top: 45px;" class="auto-style3" />Charlottesville Committee on Foreign 
			&nbsp;Relations
				<div class="auto-style2">
			        <br />
                    Administrative System</div></div>
           
           
           <div style="margin-left:200px">
    <span class="pHeader"  style="margin-left:100px">Administration Login</span><br /><br />
    Enter your user User Name and Password.<br />
        <br />
        <asp:Login ID="Login1" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4"
            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="medium"
            ForeColor="#333333" Width="482px" Height="178px" DisplayRememberMe="false" >
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
        </asp:Login>
 <div id="divMayus" style="visibility:hidden">Caps Lock is on.</div> 
        <br />
        &nbsp;<br />
        <asp:Label ID="MessageLabel" runat="server" Text=" " Width="352px"></asp:Label><br /><br />
        <asp:Label ID="SQLabel" runat="server" Height="52px" Visible="False" Width="461px" Text ="You current do not have a security question on file.  Please select a question below and enter an answer."></asp:Label>
        <asp:Table ID="SQTable" runat="server" Width="455px" Visible="False">
            <asp:TableRow runat="server">
                <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="Right" Width="200px">Security Question:</asp:TableCell>
                <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="Left" Width="330px"><asp:DropDownList ID="ddQuestion" runat="server" DataSourceID="SqlDataSource2"
            DataTextField="SecurityQuestion" DataValueField="SecurityQuestion" Width="300px">
        </asp:DropDownList><br /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" HorizontalAlign="Right" Width="200px" > Answer:</asp:TableCell>
                <asp:TableCell runat="server" HorizontalAlign="Left" Width="300px" > <asp:TextBox ID="txtAnswer" runat="server" width ="300px"></asp:TextBox><br />
       </asp:TableCell>
            </asp:TableRow>
        </asp:Table><br /><br />
        <asp:Button ID="SQButton" runat="server" Text="Submit" Visible="False" />&nbsp;
        <asp:TextBox ID="txtQuestion" runat="server" Visible="False"></asp:TextBox><br />
       
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
             SelectCommand="SELECT [ID], [SecurityQuestion] FROM [SecurityQuestions]">
        </asp:SqlDataSource>
        &nbsp;
        <br />
    </div></div>

    </form>
</body>
</html>
