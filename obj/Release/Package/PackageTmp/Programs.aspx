<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/Publ.master" CodeBehind="Programs.aspx.vb" Inherits="CCFRW19.Programs"  title="Past Programs" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="maincontent" runat="server">
	<div class="pPage"> 

    <span class="pHeader">Programs 
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></span><br /><br />

      <asp:Repeater id="Repeater1" runat="server" 
          onitemdatabound="Repeater1_ItemDataBound">
          <ItemTemplate>
              <table>
                  <tr>
                      <td>
                          <asp:Label ID="EventDate" Width="105" runat="server" Text='<%#Eval("EventDate","{0:MMMM dd}:")%>'></asp:Label>
                      </td>
                      <td>
                          <asp:Label ID="Preface" Text='<%#Eval("Preface") %>' runat="server" />
                          <asp:Label ID="Speaker" CssClass='pStrongText' Text=' <%#Eval("Speaker")%>' runat="server" />
                          <asp:Label runat="server" CssClass='PNormalTextNJ' Text='<%#Eval("ShortSpeakerBio")%>'>  </asp:Label>
                          <asp:Label runat="server" Cssclass='pItalic' ID="lbTitle" Text='<%#Eval("SpeechTitle")%>' />
                      </td>
                  </tr>
                  <br />
                  <br />
              </table>
          </ItemTemplate>
          <FooterTemplate>
<table> <tr>
 <td>
 <asp:Label ID="lblEmptyData"
        Text="Programs will be listed at a later date." runat="server" Visible="false">
 </asp:Label>
 </td>
 </tr>
 </table>           
 </FooterTemplate>
    </asp:Repeater></div>
</asp:Content>
