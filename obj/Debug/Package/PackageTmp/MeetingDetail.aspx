<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Publ.Master" CodeBehind="MeetingDetail.aspx.vb" Inherits="CCFRW19.MeetingDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="pPage">
       
     <span class="pHeader">Meeting Detail <br />
         </span>


		<div class="pPage" style="width:800px;" visible="false">
            <asp:FormView ID="FormView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1" Width="710px" Font-Bold="False" EnableModelValidation="True" OnDataBound="cleanup">
			
			<ItemTemplate >
				<h2 class="pAnnSpeaker"> <asp:Label id="SpeakerLabel" runat="server" Text='<%#Eval("Speaker") %>' /></h2>
				
				<h3 class="pAnnSpeechTitle"> <asp:Label id="SpeechTitleLabel" runat="server" Text='<%# Eval("SpeechTitle") %>' /></h3>
				
					<%--<h3 class="pAnnDate"> <asp:Label id="EventDateLabel1" runat="server" Text='<%#Eval("EventDate", "{0:dddd MMMM dd,yyyy}") %>' /></h3>--%>
			
				<span class="pNormalText"> <asp:Label id="SpeakerBioLabel" runat="server" Text='<%#Eval("SpeakerBio") %>' />
				<br />
				
					
			</ItemTemplate>
			
			<HeaderStyle horizontalalign="Center"/>
			<PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
			<RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Justify" />
	</asp:FormView>
	</div></div>
	
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand="SELECT [ID},[Speaker], [SpeakerBio], [EventDate], [SpeechTitle] FROM [Event]" >
	</asp:SqlDataSource>		
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder4" runat="server">
</asp:Content>
