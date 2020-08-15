<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/member.Master" debug="true"  CodeBehind="CurReservation.aspx.vb" Inherits="CCFRW19.CurReservation" 
    title="Reservation" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pPage" style="color:Black; height: 370px; ">
	<span class="pHeader"></span><span style="font-size: x-large; color: #c42100; font-family: Verdana">
        Reservation for
        <asp:Label ID="lbEventDate" runat="server" Text="Label" Width="355px"></asp:Label><br />
    </span>
    <br /><table  style="width:610px" class="pNormalText" id="Table1">
	
	<tr>
	<td style="height: 25px; width: 48px;">
        <strong>for:</strong></td>
      <td style="height: 25px; width: 325px;" >  <asp:Label  ID="lbMemberName" runat="server" Text=" " Width="211px" Font-Bold="True"></asp:Label><br />
        </td></tr>
    <tr>
    <td style="height: 21px; width: 48px;">     
        <strong>Status:</strong></td>
    <td style="height: 21px; width: 325px;" ><asp:Label ID="lbStatus" runat="server" Width="173px" Font-Bold="True"></asp:Label><br />
	</td></tr></table><br />

	<table style="width: 600px" class="pNormalText" id="MemGuest">
		<tr>
			<td  style="width: 100px">
                <strong>
                People Attending</strong></td>
			<td  style="width: 161px">&nbsp;<strong>Meal</strong> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </td>
		</tr>
		<tr>
			<td  style="width: 100px"></td>
			<td style="width: 161px"></td>
		</tr><tr>
			<td  style="width: 100px">
                Members</td>
			<td style="width: 161px">&nbsp;</td>
		</tr>
		
		
		
		<tr>
			<td  style="width: 100px">
                <asp:Label ID="Mem1" runat="server" Text=""></asp:Label>
                </td>
			<td style="width: 161px">
                <asp:Label ID="Mem1Meal" runat="server" Text=""></asp:Label>
                </td>
		</tr>
		<tr>
			<td  style="width: 100px">               
			 <asp:Label ID="mem2" runat="server" Text=""></asp:Label>

                </td>
			<td style="width: 161px">
                <asp:Label ID="Mem2Meal" runat="server" Text=""></asp:Label>
                </td>
		</tr>
		<tr>
			<td  style="width: 100px"></td>
            
			<td style="width: 161px"></td>
		</tr>
		<tr>
			<td  style="width: 100px">Guests</td>
			<td style="width: 161px"></td>
		</tr>
		<tr>
			<td  style="width: 100px">
                <asp:Label ID="G1" runat="server" Text=""></asp:Label>
			
			</td>
			<td style="width: 161px">
                <asp:Label ID="G1Meal" runat="server" Text=""></asp:Label></td>
		
			</tr>
		
	
		<tr>
			<td  style="width: 100px">
                <asp:Label ID="G2" runat="server" Text=""></asp:Label></td>
			<td style="width: 161px">                <asp:Label ID="G2Meal" runat="server" Text=""></asp:Label></td>

                		
			
		</tr>
		<tr>
			<td " style="width: 100px">
                <asp:Label ID="G3" runat="server" Text=""></asp:Label></td>
			<td style="width: 161px">
                 <asp:Label ID="G3Meal" runat="server" Text=""></asp:Label></td>
               		
		</tr>
		<tr>
			<td  style="width: 100px">
               <asp:Label ID="G4" runat="server" Text=""></asp:Label> </td>
			<td style="width: 161px">                <asp:Label ID="G4Meal" runat="server" Text=""></asp:Label></td>

  
		</tr>
		<tr>
			<td  style="width: 100px">
                <asp:Label ID="G5" runat="server" Text=""></asp:Label></td>
			<td style="width: 161px">                <asp:Label ID="G5Meal" runat="server" Text=""></asp:Label></td>
	</tr>
		<tr>
			<td  style="width: 100px">
                <asp:Label ID="G6" runat="server" Text=""></asp:Label></td>
			<td style="width: 161px">                <asp:Label ID="G6Meal" runat="server" Text=""></asp:Label></td>


		</tr>	</table><br />
		<br />	
	    
   <span class="pNormalText">Reservations cannot be changed online.  If you need to change this reservation, please e-mail  
       <a href="mailto:reservations@ccfrcville.org?subject=">Reservations</a> or contact our Treasurer, 
       <asp:Label ID="Tres" runat="server" Text=" " Width="97px"></asp:Label>at 
       <asp:Label ID="tresphone" runat="server" Text=" " Width="110px"></asp:Label><br />
   </span><br />
    <asp:Label ID="lbPending" runat="server" Font-Size="Small" Height="49px" Width="601px"></asp:Label><br /><br />
       
       <asp:Button runat="server" Text="Exit" id="Cancel" Height="33px" Width="110px"></asp:Button>
               <br />
<br />
    &nbsp;</div>
</asp:Content>
