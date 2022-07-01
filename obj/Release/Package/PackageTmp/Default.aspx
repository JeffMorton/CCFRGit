  <%@ Page Title="Charlottesville Committee on Foreign Relations" Language="VB" MasterPageFile="~/Publ.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="CCFRW19.Defaultc"  %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	
    <div class="pPage">
        <table style="width: 711px;">
            <tr>
                <td style="width: 284px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="Images\CCFR_LOGO_FNL.jpg" Width="238px" Height="200px" /></td>
                <td style="width: 572px; text-align: justify">Founded in 1951, The Charlottesville Committee on Foreign Relations (CCFR) is a civic, non-partisan organization dedicated to the promotion of informed discussion of 
		American foreign policy and international affairs.<br />
                    <br />
                    It has achieved a distinguished record of bringing together concerned 
		citizens in our area with leading authorities on world developments. The 
		hallmark of CCFR is the creation of opportunities for in-depth exchanges 
		on major international issues that increasingly affect our lives.</td>
            </tr>
        </table>
        <asp:Label ID="Label1" runat="server" Text="Label" Width="54px" Visible="false"></asp:Label>
    </div>


		<div class="pPage" style="width:800px;" visible="false">
            <asp:Label ID="Message" width="715px" cssclass="pNormalTextNJ" runat ="server" Text="Label" Visible="false"></asp:Label><br />
            <asp:FormView ID="FormView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1" Width="710px" Font-Bold="False" EnableModelValidation="True">
			
			<ItemTemplate >
				<h2 class="pAnnSpeaker"> <asp:Label id="SpeakerLabel" runat="server" Text='<%#Eval("Speaker") %>' /></h2>
				
				<h3 class="pAnnSpeechTitle"> <asp:Label id="SpeechTitleLabel" runat="server" Text='<%# Eval("SpeechTitle") %>' /></h3>
				
					<h3 class="pAnnDate"> <asp:Label id="EventDateLabel1" runat="server" Text='<%#Eval("EventDate", "{0:dddd MMMM dd,yyyy}") %>' /></h3>
			
				<span class="pNormalText"> <asp:Label id="SpeakerBioLabel" runat="server" Text='<%#Eval("SpeakerBio") %>' />
				<br />
				
				
				
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
	</div>
	
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [ID},[Speaker], [SpeakerBio], [EventDate], [SpeechTitle], [Meal1], [Meal2], [Meal3] FROM [Event]" >
	</asp:SqlDataSource>		
		
	</asp:Content>
		






<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    </asp:Content>

		






