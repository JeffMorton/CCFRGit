<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" validateRequest="false" CodeBehind="EmailAttendees.aspx.vb" Inherits="CCFRW19.EmailAttendees" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.ckeditor.com/4.14.1/standard/ckeditor.js"></script>
     <script>
         CKEDITOR.config.customConfig = 'ccfr_config.js';

   </script> 
    <script type = "text/javascript">
       function alertuser(msg) {
           alert(msg);
       }
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="pPage"><div class="pHeader">Email Event Attendees</div><br />
        <asp:Table ID="Table1" runat="server">
            <asp:TableRow>
                <asp:TableCell  >
                    <asp:Label ID="Label1" runat="server" Text="Event ID:" width="100px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  >  <asp:TextBox ID="EventID" text= "" runat="server" />
                </asp:TableCell> </asp:TableRow>
           <asp:TableRow>
               <asp:TableCell>
                    <asp:Label ID="Label2" runat="server" Text="Event Date:" width="100px" cssclass="pStrongTextRight" />
               </asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="EventDate" runat="server"  width="250px"  />
                </asp:TableCell> </asp:TableRow>
            <asp:TableRow><asp:TableCell>
                  <asp:Label ID="Label6" runat="server" Text="Subject" width="100px" cssclass="pStrongTextRight" />
               </asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="mSubject" runat="server"  width="450px"  />
                   </asp:TableCell></asp:TableRow>
            
 
 <asp:TableRow>
                <asp:TableCell  >
                    <asp:Label ID="Label3" runat="server" Text="Attachment Files:" width="100px" height="150px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  >
                    <asp:ListBox ID="ListAttachments" runat="server" Width="600px" Height =" 200px"></asp:ListBox> <asp:FileUpload ID="FileUpload1" allowMultiple="true" Width="440px" height="23px" runat="server" Font-Names="Verdana" Font-Size="11px" />           
                    <asp:Button ID="btnUpload" Text="Upload" runat="server" Height="23px" OnClick ="UploadMultipleFiles"  />
                     <asp:Button ID="Clear" Text="Clear Attachments" runat="server" Height="23px" OnClick ="ClearAttachments"  />
                </asp:TableCell>
     </asp:TableRow> 
                  <asp:TableRow>
                <asp:TableCell >
                    <asp:Label ID="Label4" runat="server"  Text="Message:" width="100px" height="150px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  >  <asp:TextBox ID="MessageText" height="150px" width="500px" textmode="MultiLine"  runat ="server" />
                    <script type="text/javascript" lang="javascript">CKEDITOR.replace('<%=MessageText.ClientID%>', { customConfig: '/ccfr_config.js' });</script> 
                </asp:TableCell> </asp:TableRow>      

        </asp:Table>
        <asp:Button ID="Button2" runat="server" Text="Send Sample Email " OnClick="SendSampleEmail" />

        <asp:Button ID="Button1" runat="server" Text="Send All Emails" OnClick="SendEmailAttendees" />

        &nbsp;&nbsp;&nbsp;<asp:Label ID="Label5" runat="server" Text="EMails Sent: "></asp:Label><asp:TextBox ID="ESent" runat="server"></asp:TextBox>
    </div>
      <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
   
       
  
  
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="CCFRW19.dsattendenceGuestsTableAdapters."></asp:ObjectDataSource>

</asp:Content>
