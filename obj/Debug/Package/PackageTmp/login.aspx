<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Publ.master" CodeBehind="login.aspx.vb" Inherits="CCFRW19.login" 
    title="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server" >

   <div class="pPage" >
    <span class="pHeader">Member Login</span><br /><br />
    Enter your user User Name and Password.<br />
        <br />
        <asp:Login ID="Login1" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4"
            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="medium"
            ForeColor="#333333" Width="482px" Height="178px" DisplayRememberMe="false" PasswordRecoveryUrl="Passrecovery.aspx" >
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
    </div>

</asp:Content>
