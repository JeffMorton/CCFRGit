<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" CodeBehind="AdminStart.aspx.vb" Inherits="CCFRW19.AdminStart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style5 {
            width: 282px;
        }
        .auto-style6 {
            margin-top: 0px;
        }
        .auto-style7 {
            margin-left: 0px;
            margin-top: 0px;
            color: black;
            font-family: Verdana;
            font-size: small;
            font-style: normal;
            font-weight: normal;
            background-color: White;
            width: 468px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="pPage">
    </div>
		<div class="auto-style7" visible="false">
   <div style="margin-left:200px"  >   <strong>Select Event Date:</strong>    <asp:DropDownList ID="EventDateDDL" autopostback="true" runat="server" Height="27px" Width="110px" DataSourceID="DateDDL" OnSelectedIndexChanged="EventDLL_Index_Changed"
        DataTextField="EDate"
        DataValueField="ID">
        </asp:DropDownList></div> <br />
    <asp:SqlDataSource ID="DateDDL" runat="server" selectcommand="select CONVERT(VARCHAR(10), eventdate, 101) as EDate, Id from Event order by EventDate desc ">
</asp:SqlDataSource>
            <div class="pHeader" style="margin-left:250px">Event Status</div>

                <asp:Panel ID="Panel1" runat="server" Height="114px" width="340px" BorderStyle="Groove">
               <br /><div class="pNormalTextRight">
                <Table>
                    <tr><td><asp:Label runat="server" Text="Members Attending:" width="140px"  /></td><td><asp:TextBox ID="Members" runat="server"  Width="50px"></asp:TextBox></td></tr>
                   <tr><td><asp:Label runat="server" Text="Guests Attending:" width="140px"  /></td><td> <asp:TextBox ID="Guests" runat="server"  Width="50px"></asp:TextBox></td></tr>
                   <tr><td><asp:Label runat="server" Text="Total Attending:" width="140px"/></td><td><asp:TextBox ID="ETotal" runat="server"  Width="50px"></asp:TextBox><br /></td></tr> </Table></div>
</asp:panel>
 <asp:panel ID="Panel2" cssclass="pNormalTextRight" style="margin-left:350px;height:114px;Margin-top:-120px" width="340px" BorderStyle="Groove" runat="server">
 <br />   <table> <tr><td><asp:Label runat="server" Text="Percent Online:" /></td><td><asp:TextBox ID="POnLine" runat="server"  Width="50px"></asp:TextBox></td></tr>
              <tr><td>    <asp:Label runat="server" Text="Percent Mailed In:" width="140px" /></td><td> <asp:TextBox ID="PMailedIn" runat="server"  Width="50px"></asp:TextBox></td></tr>

     </table>
 </asp:panel>
                    
            <asp:Label ID="Label1" runat="server" Text="Total Members:"></asp:Label><asp:Label ID="TotalMembers" runat="server" Text=""></asp:Label>         
</div>
       
   

            <asp:FormView ID="FormView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1" Width="700px" Font-Bold="False" EnableModelValidation="True">
			
			<ItemTemplate >
				<h2 class="pAnnSpeaker"> <asp:Label id="SpeakerLabel" runat="server" Text='<%#Eval("Speaker") %>' /></h2>
				
				<h3 class="pAnnSpeechTitle"> <asp:Label id="SpeechTitleLabel" runat="server" Text='<%# Eval("SpeechTitle") %>' /></h3>
					<h3 class="pAnnDate"> <asp:Label id="EType" runat="server" Text='<%# Eval("Type") %>' /></h3>
			
					<h3 class="pAnnDate"> <asp:Label id="EventDateLabel1" runat="server" Text='<%#Eval("EventDate", "{0:dddd MMMM dd,yyyy}") %>' /></h3>
				<br />
				<span class="pNormalText"> 
				
				
				<strong>Meal A:</strong>
				<asp:Label id="Meal1Label" runat="server" Text='<%# Eval("Meal1") %>' />
				<br />
				<strong>Meal B:</strong>
				<asp:Label id="Meal2Label" runat="server" Text='<%# Eval("Meal2") %>' />
				<br />
				<strong>Meal C:</strong>
				<asp:Label id="Meal3Label" runat="server" Text='<%# Eval("Meal3") %>' />
				<br />
				<asp:Label id="EventID" runat="server"  Visible= 'false' Text='<%# Eval("ID") %>' />
				<asp:Label id="MealCost" runat="server"  Visible= 'false' Text='<%# Eval("Cost") %>' />

				</span>
	
				

			</ItemTemplate>
				<EmptyDataTemplate>
					<h2 class="pAnnSpeaker"> <asp:Label id="SpeakerLabel" runat="server" Text="No Meeting have been Scheduled"/></h2>
					<h3 class="pAnnSpeechTitle"> <asp:Label id="SpeechTitleLabel" runat="server" Text="Please check back for updates"/></h3>
					<h3 class="pAnnDate"> <asp:Label id="EventDateLabel1" runat="server" Text=""/></h3>
					<asp:Label id="EventID" runat="server"  Visible= 'false' Text=0/>
					<asp:Label id="MealCost" runat="server"  Visible= 'false' Text=0.00 />
				</EmptyDataTemplate>

			<HeaderStyle horizontalalign="Center"/>
			<PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
			<RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Justify" />
	</asp:FormView>
    <div style="margin-left:350px;margin-top:-120px">
<asp:Panel ID="Panel4" width="30px" Height="250px" runat="server" margin-left="350px">
            <asp:Label ID="err" Text="Problem Registrations" width="230px" cssclass="pHeader2" runat="server"/>
            <asp:GridView ID="ERRGridview"  runat= "server" AllowPaging="false" AlternatingRowStyle-BackColor="#C2D69B" AlternatingRowStyle-CssClass="pNormatext" AutoGenerateColumns="False" CssClass="pNormalText" DataKeyNames="ID" EmptyDataText="No data available." HeaderStyle-BackColor="Wheat" ShowHeaderWhenEmpty="true" Width="350" AllowSorting="True"><Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelH" runat="server" Text="MemberID" Width="160px" /><asp:Label ID="Label2" runat="server" Text="Name" Width="90px" /></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Table ID="Table2" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        <asp:Label ID="ID" Width="160px" runat="server" Text='<%# Bind("ID") %>' />
                                    </asp:TableCell><asp:TableCell>
                                        <asp:Label ID="mlFullName" runat="server" Width="200px" Text='<%# Bind("mlFullName") %>' />
                                    </asp:TableCell><asp:TableCell>
                                      
                                    </asp:TableCell></asp:TableRow></asp:Table></ItemTemplate></asp:TemplateField></Columns></asp:GridView></asp:Panel><asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [ID],[Speaker], [SpeakerBio], [EventDate], [SpeechTitle], [Meal1], [Meal2], [Meal3],[Type] FROM [Event]" >
	</asp:SqlDataSource>		
   </div>

</asp:Content>
