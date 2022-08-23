<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Publ.master" CodeBehind="LunchClosed.aspx.vb" Inherits="CCFRW19.LunchClosed" 
    title="Lunch Closed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="pPage">
    <asp:Panel ID="LunchClosed" runat="server">
        <span class="pHeader"> Reservations Closed</span>
        <br />
        <br />
        This lunch meeting is closed. 
        <br /><br />
        Lunch Meetings are limited to 25 attendees.  That limit has been reached.
   </asp:Panel> </div>
    <asp:Panel ID="NotAvailable" runat="server">
         <span class="pHeader"> No Lunch Available</span>

    </asp:Panel>
</asp:Content>
