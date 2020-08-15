<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/member.Master" CodeBehind="DinnerClosed.aspx.vb" Inherits="CCFRW19.DinnerClosed" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="pPage">
      <asp:Panel ID="DinnerClose" runat="server">
        <span class="pHeader"> Reservations Closed</span>
        <br />
        <br />
        This dinner meeting is closed. 
        <br /><br />
        Dinner Meetings are limited to 200 attendees.  That limit has been reached.
  </asp:Panel>  </div>
    <asp:Panel ID="NotAvailable" runat="server">
         <span class="pHeader"> No Dinner Available</span>

    </asp:Panel>
</asp:Content>
