<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Publ.master" CodeBehind="ReservationsClosed.aspx.vb" Inherits="CCFRW19.ReservationsClosed" 
    title="Reservations Closed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pPage">
        <span class="pHeader"> Reservations Are Closed</span>
        <br />
        <br />
        Online reservations for this event have closed. Please email <asp:HyperLink id="hyperlink1" 
                  
                  NavigateUrl="mailto:reservations@ccfrcville.org"
                  Text="reservations@ccfrcville.org"
                  Target="_new"
                  runat="server"/> to see if late additions or changes to reservations can be accommodated.<br /><br />Reservations close at midnight on the Monday before the meeting.
    </div>
</asp:Content>
