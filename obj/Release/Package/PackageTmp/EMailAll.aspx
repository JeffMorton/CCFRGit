<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/admin.Master" validateRequest="false" CodeBehind="EMailAll.aspx.vb" Inherits="CCFRW19.EamilAll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
    <script src="ckeditor/ckeditor.js"></script>
<script>
     CKEDITOR.config.customConfig = 'ccfr_config.js';
     CKEDITOR.replace(Story);
   </script>    
   <script type = "text/javascript">
       function alertuser(msg) {
           alert(msg);
       }
    </script>
    <script type = "text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to send the rest of the E-Mails  ?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function alertuser(msg) {
            alert(msg);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="pPage"><div class="pPage"><div class="pHeader">Email All Members</div><br />
        <asp:Table ID="Table1" runat="server">
             <asp:TableRow>
               <asp:TableCell>
                    <asp:Label ID="Label2" runat="server" Text="Subject:" width="100px" cssclass="pStrongTextRight" />
               </asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="Subject" runat="server" />
                </asp:TableCell> </asp:TableRow>
 <asp:TableRow>
                <asp:TableCell Width="200px" >
                    <asp:Label ID="Label3" runat="server" Text="Attachment Files:" width="100px" height="150px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  Width="500px">
                    <asp:ListBox ID="ListAttachments" runat="server" Width="600px" Height =" 150px"></asp:ListBox> <asp:FileUpload ID="FileUpload1" allowMultiple="true" Width="440px" height="23px" runat="server" Font-Names="Verdana" Font-Size="11px" />           
                    <asp:Button ID="btnUpload" Text="Upload" runat="server" Height="23px" OnClick ="UploadMultipleFiles"/>
                     <asp:Button ID="Clear" Text="Clear Attachments" runat="server" Height="23px" OnClick ="ClearAttachments"  />
                </asp:TableCell>
     </asp:TableRow>
      <asp:TableRow>
                <asp:TableCell  >
                    <asp:Label ID="Label4" runat="server"  Text="Message Text:" width="100px" height="150px" cssclass="pStrongTextRight" />
                </asp:TableCell>
                <asp:TableCell  > <asp:TextBox ID="MessageText" runat="server" TextMode="MultiLine"></asp:TextBox>
<script type="text/javascript" lang="javascript">CKEDITOR.replace('<%=MessageText.ClientID%>', { customConfig: '/ccfr_config.js' });</script> 
 
                </asp:TableCell> </asp:TableRow>      
       </asp:Table>
                    <asp:Label ID="Label5" runat="server" Text="Members Receiving Emails:"></asp:Label>
            <asp:TextBox ID="ESent" runat="server" Text =" " ></asp:TextBox><br /><br />

         <asp:Button ID="Send1" runat="server" Height="33px" Width="150px" Text="Send Sample Email" OnClick ="SendSampleEmail" />
       <asp:Button ID="OnCon"  runat="server" OnClick = "OnConfirm" Height="33px" Width="150px" Text = "Send All E-Mails"  OnClientClick = "Confirm()"/>
        </div>
  

    </div>
</asp:Content>
