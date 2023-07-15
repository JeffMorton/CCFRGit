<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Board.aspx.vb" MasterPageFile="~/Publ.Master" Inherits="CCFRW19.Leadership" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="pPage" style=" color:black; width: 675px; ">
       <span class="pHeader">Leadership</span><br /><br />

           <asp:Label ID="Address" Width ="715px" cssclass="pHeader2" runat="server" Text=""></asp:Label><br />
    <div style ="  position: absolute; top: 210px;    left: 150px; text-align:center" >     
          <asp:label ID="tb1" runat="server" Width="250px" text-align="center" style="left:10px;top:300px" />
</div>

           <div style ="  position: absolute; top: 210px;    left: 425px; text-align:center" >     
 
<asp:label ID="tb2" runat="server" Width="250px" text-align="center" style="left:310px;top:300px"/>
</div>
 <div style ="  position: absolute; top: 210px;    left: 700px; text-align:center" >     

           <asp:label ID="tb3" runat="server" Width="250px" text-align="center" style="left:510px;top:300px"/>

 
          </div>

 	 
</asp:Content>
